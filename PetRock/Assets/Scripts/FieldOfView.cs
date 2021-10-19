﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour
{
    // https://www.youtube.com/watch?v=rQG9aUWarwE  Ep01
    // https://www.youtube.com/watch?v=73Dc5JTCmKI  Ep02 => je n'ai pas fait la partie a partir de 18min

    [Min(0)] public float range = 5f;
    [Range(0,360)] public float viewAngle = 45f;
    public LayerMask targetLayerMask;
    public LayerMask obstableLayerMask;

    public float meshResolution = 0.25f;
    public int edgeResolveIteration = 4;
    public float edgeDistanceThreshold = 0.5f;
    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    PlayerParameters _playerParameters;

    private void Start()
    {
        viewMesh = new Mesh();
        viewMeshFilter.mesh = viewMesh;
        StartCoroutine(FindVisibleTargetsWithDelay(0.2f)); // J'aime pas le invoke Repeating

        _playerParameters = PlayerParameters.Instance;
    }

    private void LateUpdate()
    {
        DrawFieldOfView();
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    
    IEnumerator FindVisibleTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {

        if (_playerParameters.HasBeenOnGiantZone())
            _playerParameters.UpdatePlayerState(PlayerParameters.PreviousPlayerStates.Stressed, PlayerParameters.PlayerStates.Stressed);
        else
        {
            if(!_playerParameters.HasBeenOnStressed())
                _playerParameters.UpdatePlayerState(PlayerParameters.PreviousPlayerStates.Regular, PlayerParameters.PlayerStates.Regular);
        }

        Collider[] targets = Physics.OverlapSphere(transform.position, range, targetLayerMask);
        if (targets.Length != 0)
        {
            Transform target = targets[0].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, obstableLayerMask))
                {
                    if (!_playerParameters.IsOnSafeZone())
                    {
                        // TO DO : PLAYER IS SPOTTED
                        Debug.Log("Player is spotted");

                        if (!_playerParameters.HasBeenOnGiantZone())
                            _playerParameters.UpdatePlayerState(PlayerParameters.PreviousPlayerStates.GiantZone, PlayerParameters.PlayerStates.GiantZone);
                    }
                }
            }
        }
    }

    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(meshResolution * viewAngle);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i<stepCount; ++i)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + i * stepAngleSize;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDistanceThresholdExceeded = Mathf.Abs(oldViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;

                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDistanceThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);

                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = stepCount + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < stepCount; ++i)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, range, obstableLayerMask))
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        else return new ViewCastInfo(false, transform.position + dir * range, range, globalAngle);
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _distance, float _angle)
        {
            hit = _hit;
            point = _point;
            distance = _distance;
            angle = _angle;
        }
    }

    public void Init(float _range, float _angle)
    {
        range = _range;
        viewAngle = _angle;
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 A, Vector3 B)
        {
            pointA = A;
            pointB = B;
        }
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIteration; ++i)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDistanceThresholdExceeded = Mathf.Abs(minViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;

            if (newViewCast.hit == minViewCast.hit && !edgeDistanceThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }
}

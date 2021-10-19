using UnityEngine;
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
    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    PlayerParameters _playerParameters;

    private void Awake()
    {
        _playerParameters = PlayerParameters.Instance;
    }

    private void Start()
    {
        viewMesh = new Mesh();
        viewMeshFilter.mesh = viewMesh;
        StartCoroutine(FindVisibleTargetsWithDelay(0.2f)); // J'aime pas le invoke Repeating
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

        if (_playerParameters.previousPlayerStates == PlayerParameters.PreviousPlayerStates.GiantZone)
            _playerParameters.UpdatePlayerState(PlayerParameters.PreviousPlayerStates.Stressed, PlayerParameters.PlayerStates.Stressed);
        else
        {
            if(_playerParameters.previousPlayerStates != PlayerParameters.PreviousPlayerStates.Stressed)
                _playerParameters.playerStates = PlayerParameters.PlayerStates.Regular;
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
                    if (!PlayerParameters.Instance.inSafeZone)
                    {
                        // TO DO : PLAYER IS SPOTTED
                        Debug.Log("Player is spotted");

                        if (_playerParameters.previousPlayerStates != PlayerParameters.PreviousPlayerStates.GiantZone)
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

        for (int i = 0; i<stepCount; ++i)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + i * stepAngleSize;
            ViewCastInfo newViewCast = ViewCast(angle);
            viewPoints.Add(newViewCast.point);
            //Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * range, Color.red);
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
}

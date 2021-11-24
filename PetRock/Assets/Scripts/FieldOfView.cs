using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour
{
    // https://www.youtube.com/watch?v=rQG9aUWarwE  Ep01
    // https://www.youtube.com/watch?v=73Dc5JTCmKI  Ep02

    public EyeVision eyeVision;
    [Space]
    [SerializeField] float delayResumeNormalMovement = 2f;
    private float timerResuming = 0f;
    private bool resumingtoNormal = false;
    
    private float currentViewAngle;
    private float stayOpenTimer = 0;
    private bool flickering = false;

    [SerializeField] LayerMask targetLayerMask;
    [SerializeField] LayerMask obstableLayerMask;

    [SerializeField] float meshResolution = 0.25f;
    [SerializeField] int edgeResolveIteration = 4;
    [SerializeField] float edgeDistanceThreshold = 0.5f;
    [SerializeField] MeshFilter viewMeshFilter;
    private Mesh viewMesh;

    PlayerParameters _playerParameters;

    private bool playerSpotted = false;
    private Coroutine flickerRoutine;
    private EyeMovement eyeMovement;

    private void Start()
    {
        eyeMovement = GetComponent<EyeMovement>();
        currentViewAngle = eyeVision.viewAngle;

        if (eyeVision.seeThroughObstacle)
            obstableLayerMask = LayerMask.GetMask("Nothing");

        _playerParameters = PlayerParameters.Instance;

        viewMesh = new Mesh();
        viewMeshFilter.mesh = viewMesh;
        StartCoroutine(FindVisibleTargetsWithDelay(0.2f)); // J'aime pas le invoke Repeating
    }

    private void OnEnable()
    {
        StartCoroutine(FindVisibleTargetsWithDelay(0.2f));
    }

    private void Update()
    {
        if (playerSpotted) return;

        if (resumingtoNormal)
        {
            eyeMovement.SetPlayerSpotted(PlayerSpotState.Resuming, timerResuming / delayResumeNormalMovement);
            timerResuming += Time.deltaTime;
            if (timerResuming > delayResumeNormalMovement)
            {
                timerResuming = 0;
                resumingtoNormal = false;
                eyeMovement.SetPlayerSpotted(PlayerSpotState.NotSpoted);
            }
            else return;
        }

        if (eyeVision.canFlicker)
        {
            stayOpenTimer += Time.deltaTime;
            if (stayOpenTimer > eyeVision.timeToStayOpen && !flickering)
            {
                flickering = true;
                flickerRoutine = StartCoroutine(Flicker());
            }
        }
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
        playerSpotted = false;

        Collider[] targets = Physics.OverlapSphere(transform.position, eyeVision.range, targetLayerMask);
        if (targets.Length != 0)
        {
            Transform target = targets[0].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            
            if (Vector3.Angle(transform.forward, dirToTarget) < currentViewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, obstableLayerMask))
                {
                    if (!_playerParameters.IsOnSafeZone())
                    {
                        // TO DO : PLAYER IS SPOTTED
                        Debug.Log("Player is spotted");
                        playerSpotted = true;
                        resumingtoNormal = true;
                        eyeMovement.SetPlayerSpotted(PlayerSpotState.Spoted);
                        StopFlicker();
                        _playerParameters.UpdatePlayerState(PlayerParameters.PlayerStates.GiantZone);
                    }
                }
            }
        }
    }

    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(meshResolution * currentViewAngle);
        if (stepCount == 0)
        {
            viewMesh.Clear();
        }
        else
        {
            float stepAngleSize = currentViewAngle / stepCount;
            List<Vector3> viewPoints = new List<Vector3>();
            ViewCastInfo oldViewCast = new ViewCastInfo();

            for (int i = 0; i < stepCount; ++i)
            {
                float angle = transform.eulerAngles.y - currentViewAngle / 2 + i * stepAngleSize;
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
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, eyeVision.range, obstableLayerMask))
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        else return new ViewCastInfo(false, transform.position + dir * eyeVision.range, eyeVision.range, globalAngle);
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

    IEnumerator Flicker()
    {
        float timer = 0f;

        // CLose
        while (timer < eyeVision.timeToClose)
        {
            currentViewAngle = Mathf.Lerp(eyeVision.viewAngle, 0, eyeVision.flickerCurve.Evaluate(timer / eyeVision.timeToClose));
            timer += Time.deltaTime;
            yield return null;
        }

        // CLosed
        timer = 0;
        currentViewAngle = 0;
        yield return new WaitForSeconds(eyeVision.timeToStayClosed);

        // Open
        while (timer < eyeVision.timeToOpen)
        {
            currentViewAngle = Mathf.Lerp(0, eyeVision.viewAngle, timer / eyeVision.timeToOpen);
            timer += Time.deltaTime;
            yield return null;
        }
        currentViewAngle = eyeVision.viewAngle;
        stayOpenTimer = 0;
        flickering = false;
    }

    private void StopFlicker()
    {
        if (flickerRoutine != null)
        {
            StopCoroutine(flickerRoutine);
            currentViewAngle = eyeVision.viewAngle;
            stayOpenTimer = 0;
            flickering = false;
        }
    }
}

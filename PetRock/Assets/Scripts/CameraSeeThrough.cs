using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSeeThrough : MonoBehaviour
{
    public Transform target;
    [SerializeField] float distanceRay = 4.5f;

    private Transform obstruction;
    private MeshRenderer renderer;

    [SerializeField] bool debugMode = false;

    // Update is called once per frame
    void LateUpdate()
    {
        ViewObstructed();
    }

    void ViewObstructed()
    {
        if (target == null)
        {
            Debug.LogError("No target");
            return;
        }

        RaycastHit hit;

        if (Physics.Raycast(transform.position, target.position - transform.position, out hit, distanceRay))
        { 
            if (!hit.collider.CompareTag("Player"))
            {
                if (obstruction != hit.transform)
                {
                    if (obstruction != null && renderer != null)
                    {
                        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                    }
                    obstruction = hit.transform;
                    renderer = obstruction.GetComponent<MeshRenderer>();
                    if (renderer != null)
                        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                }
            }
            else if (obstruction != null)
            {
                if (renderer != null)
                    renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                obstruction = null;
                renderer = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!debugMode) return;

        Gizmos.DrawLine(transform.position, transform.position + distanceRay * (target.position - transform.position).normalized);
    }
}

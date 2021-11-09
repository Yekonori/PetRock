using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSeeThrough : MonoBehaviour
{
    public Transform target;
    [SerializeField] float distanceRay = 4.5f;

    private Transform obstruction;

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
                    if (obstruction != null)
                    {
                        obstruction.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                    }
                    obstruction = hit.transform;
                    obstruction.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                }
            }
            else if (obstruction != null)
            {
                obstruction.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                obstruction = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!debugMode) return;

        Gizmos.DrawLine(transform.position, transform.position + distanceRay * (target.position - transform.position).normalized);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Script Parameters
    [Header("..")]
    [SerializeField, Min(0f)] float speed = 5f;
    [SerializeField, Min(0f)] float rotationSpeed = 90f;
    //[SerializeField, Min(0f)] float gravity = 5f; // can we jump? fall ?

    [SerializeField] bool debugMode = false;
    #endregion Script Parameters

    #region Fields

    // direction and rotation
    private float dirX = 0;
    private float dirY = 0;
    private Camera cam;
    #endregion Fields

    private void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (GameManager.instance._inPause || GameManager.instance.inRockBalancing)
        {
            dirX = 0;
            dirY = 0;
        }
        Vector3 dir = new Vector3(dirX, 0, dirY);
        if (dir.magnitude < 0.1f) return;

        Quaternion rot = Quaternion.LookRotation(Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up), Vector3.up) * Quaternion.LookRotation(dir, Vector3.up);
        Quaternion rotation = Quaternion.Lerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);

        transform.position += dir.sqrMagnitude * transform.forward * speed * Time.deltaTime; // += gravity * 
        transform.rotation = rotation;
    }

    public void SetMovementDirection(float x, float y)
    {
        dirX = x;
        dirY = y;
    }

    private void OnDrawGizmos()
    {
        if (!debugMode) return;

        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
    }
}

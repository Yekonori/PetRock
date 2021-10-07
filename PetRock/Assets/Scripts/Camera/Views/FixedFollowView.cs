using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedFollowView : AView
{
    public float roll = 0f;
    [Range(0f, 180f)] public float fov = 0f;
    public Transform target;

    [SerializeField] Transform centralPoint;
    [SerializeField] float yawOffsetMax = 180f;
    [SerializeField] float pitchOffsetMax = 180f;

    private float yaw = 0f;
    private float pitch = 0f;

    private float yawCentral = 0f;
    private float pitchCentral = 0f;


    private void Update()
    {
        Vector3 dirCentral = (centralPoint.position - centralPoint.position).normalized;
        yawCentral = Mathf.Atan2(dirCentral.x, dirCentral.z) * Mathf.Rad2Deg;
        pitchCentral = -Mathf.Asin(dirCentral.y) * Mathf.Rad2Deg;

        Vector3 dir = (target.position - transform.position).normalized;
        yaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        if (Mathf.Abs(yaw - yawCentral) > yawOffsetMax)
        {
            yaw = Mathf.Sign(yaw) * yawOffsetMax;
        }

        pitch = -Mathf.Asin(dir.y) * Mathf.Rad2Deg;
        if (Mathf.Abs(pitch - pitchCentral) > pitchOffsetMax)
        {
            pitch = Mathf.Sign(pitch) * pitchOffsetMax;
        }
    }

    public override CameraConfiguration GetConfiguration()
    {
        return new CameraConfiguration(yaw, pitch, roll, transform.position, 0f, fov);
    }
}

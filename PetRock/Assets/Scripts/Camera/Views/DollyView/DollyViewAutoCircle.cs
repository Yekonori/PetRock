using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyViewAutoCircle : AView
{
    public float roll = 0f;
    public float distance = 0f;
    public float fov = 0f;

    [SerializeField] Transform target;
    [SerializeField] Rail rail;
    [SerializeField] float timeToRotate = 5;

    [Header("Stop rotating")]
    [SerializeField, Min(1)] int maxNumberTour = 3;
    private bool canMove = true;

    private Vector3 railPosition = Vector3.zero;

    private float yaw = 0f;
    private float pitch = 0f;

    private float railLength = 0f;
    private float timer = 0f;

    void Start()
    {
        railLength = rail.GetLength();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive && canMove)
        {
            timer += Time.deltaTime;

            if (timer > timeToRotate * maxNumberTour)
            {
                canMove = false;
            }
            else
            {
                railPosition = rail.GetPosition(timer / timeToRotate * railLength);

                Quaternion rot = Quaternion.LookRotation(target.position - railPosition);
                yaw = rot.eulerAngles.y;
                pitch = rot.eulerAngles.x;
            }
        }
    }

    public override CameraConfiguration GetConfiguration()
    {
        return new CameraConfiguration(yaw, pitch, roll, railPosition, distance, fov);
    }

    public float getTimeToRotate
    {
        get
        {
            return timeToRotate;
        }
    }
}

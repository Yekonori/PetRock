using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFollowView : AView
{
    [SerializeField] List<float> pitch;
    private float _pitch = 0f;
    [SerializeField] List<float> roll;
    private float _roll = 0f;
    [SerializeField] List<float> fov;
    private float _fov = 0f;
    
    [SerializeField] float yaw = 0f;
    [SerializeField] float yawSpeed = 10f;

    [SerializeField] Transform target;
    [SerializeField] Curve curve;
    [SerializeField, Range(0f, 1f)] float curvePosition;
    [SerializeField] float curveSpeed;

    private Matrix4x4 curveToWorldMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);

    private void Start()
    {
        //base.Init();
        if (!(pitch.Count == roll.Count && pitch.Count == fov.Count))
            Debug.LogError("Pitch, Roll, et Fov n'ont pas le même nombre d'élément !");
    }

    private void Update()
    {
        //if (Input.GetKey(KeyCode.RightArrow))
        //{
        //    yaw += yawSpeed * Time.deltaTime;
        //}
        //else if (Input.GetKey(KeyCode.LeftArrow))
        //{
        //    yaw -= yawSpeed * Time.deltaTime;
        //}
        
        if (Input.GetMouseButton(1)) //Right CLick
        {
            yaw += yawSpeed * Time.deltaTime * Input.GetAxis("Mouse X");
        }

        float inputMouseWheel = 0f;
        inputMouseWheel = Input.GetAxis("Mouse ScrollWheel");
        curvePosition += curveSpeed * Time.deltaTime * inputMouseWheel;
        if (curvePosition > 1f)
            curvePosition = 1f;
        if (curvePosition < 0f)
            curvePosition = 0f;

        //if (Input.GetKey(KeyCode.UpArrow))
        //{
        //    curvePosition += curveSpeed * Time.deltaTime;
        //    if (curvePosition > 1f)
        //        curvePosition = 1f;
        //}
        //else if (Input.GetKey(KeyCode.DownArrow))
        //{
        //    curvePosition -= curveSpeed * Time.deltaTime;
        //    if (curvePosition < 0f)
        //        curvePosition = 0f;
        //}

        _pitch = MultiLerp(pitch, curvePosition);
        _roll = MultiLerp(roll, curvePosition);
        _fov = MultiLerp(fov, curvePosition);
        curveToWorldMatrix = Matrix4x4.TRS(target.position, Quaternion.Euler(0f, yaw, 0f), Vector3.one);
    }

    public override CameraConfiguration GetConfiguration()
    {
        return new CameraConfiguration(yaw, _pitch, _roll, curve.GetPosition(curvePosition, curveToWorldMatrix), 0f, _fov);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        curveToWorldMatrix = Matrix4x4.TRS(target.position, Quaternion.Euler(0f, yaw, 0f), Vector3.one);
        curve.DrawGizmo(Color.green, curveToWorldMatrix);
    }

    float MultiLerp(List<float> list, float value)
    {
        if (list.Count == 0)
        {
            Debug.LogError("list of float is empty. Returning 0");
            return 0f;
        }
        else if (list.Count == 1)
        {
            //Debug.Log("list contains only 1 element. Returning it");
            return list[0];
        }

        if (value < 0f || value > 1f)
        {
            Debug.Log("Value has been clamped between 0 and 1");
            value = Mathf.Clamp(value, 0f, 1f);
        }

        for (int i = 0; i < list.Count -1; ++i)
        {
            if (value < (i+1)/(float)(list.Count - 1))
            {
                return Mathf.Lerp(list[i], list[i + 1], (list.Count - 1) * (value - i / ((list.Count - 1))));
            }
        }
        return list[list.Count - 1];
    }
}

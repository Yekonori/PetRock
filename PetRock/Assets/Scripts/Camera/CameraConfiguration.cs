using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraConfiguration
{
    [Range(0f, 360f)] public float yaw = 0f;
    [Range(-90f, 90f)] public float pitch = 0f;
    [Range(-180, 180)] public float roll = 0f;
    public Vector3 pivot = Vector3.zero;
    [Min(0f)] public float distance = 2f;
    [Range(0f, 180f)] public float fieldOfView = 60f;

    public CameraConfiguration(float _yaw, float _pitch, float _roll, Vector3 _pivot, float _distance, float _fieldOfView)
    {
        yaw = _yaw;
        pitch = _pitch;
        roll = _roll;
        pivot = _pivot;
        distance = _distance;
        fieldOfView = _fieldOfView;
    }

    public Quaternion GetRotation()
    {
        return Quaternion.Euler(pitch, yaw, roll);
    }

    public Vector3 GetPosition()
    {
        Vector3 offset = distance * Vector3.back;
        Vector3 position = pivot + offset;

        return position;
    }

    public void DrawGizmos(Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(pivot, 0.25f);
        Vector3 position = GetPosition();
        Gizmos.DrawLine(pivot, position);
        Gizmos.matrix = Matrix4x4.TRS(position, GetRotation(), Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, fieldOfView, 0.5f, 0f, Camera.main.aspect);
        Gizmos.matrix = Matrix4x4.identity;
    }



    // PARTIE TEST
    public static CameraConfiguration Scalaire(float a, CameraConfiguration config)
    {
        float yaw = a * config.yaw;
        float pitch = a * config.pitch;
        float roll = a * config.roll;
        Vector3 pivot = a * config.pivot;
        float distance = a * config.distance;
        float fieldOfView = a * config.fieldOfView;

        CameraConfiguration toReturn = new CameraConfiguration(yaw, pitch, roll, pivot, distance, fieldOfView);
        return toReturn;
    }

    public static CameraConfiguration SumConfig(CameraConfiguration A, CameraConfiguration B)
    {
        float yaw = A.yaw + B.yaw;
        float pitch = A.pitch + B.pitch;
        float roll = A.roll + B.roll;
        Vector3 pivot = A.pivot + B.pivot;
        float distance = A.distance + B.distance;
        float fieldOfView = A.fieldOfView + B.fieldOfView;

        CameraConfiguration toReturn = new CameraConfiguration(yaw, pitch, roll, pivot, distance, fieldOfView);
        return toReturn;
    }

    public static CameraConfiguration Interpolation(float t, CameraConfiguration A, CameraConfiguration B)
    {
        return SumConfig(Scalaire(1 - t, A), Scalaire(t, B));
    }

    public static CameraConfiguration ListInterpolation(float t, List<CameraConfiguration> list)
    {
        if (list.Count == 2)
        {
            return Interpolation(t, list[0], list[1]);
        }
        else
        {
            List<CameraConfiguration> list1 = new List<CameraConfiguration>(list);
            List<CameraConfiguration> list2 = new List<CameraConfiguration>(list);
            list1.RemoveAt(list.Count - 1);
            list2.RemoveAt(0);
            return SumConfig(Scalaire(1 - t, ListInterpolation(t, list1)), Scalaire(t, ListInterpolation(t, list2)));
        }
    }
}

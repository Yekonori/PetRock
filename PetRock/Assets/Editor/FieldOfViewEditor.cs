using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;
        Handles.color = Color.white;
        Vector3 viewAngleA = fov.DirFromAngle(-fov.eyeVision.viewAngle / 2, false);
        Vector3 viewAngleB = fov.DirFromAngle(fov.eyeVision.viewAngle / 2, false);
        Handles.DrawWireArc(fov.transform.position, Vector3.up, viewAngleA, fov.eyeVision.viewAngle, fov.eyeVision.range);

        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.eyeVision.range);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.eyeVision.range);
    }
}

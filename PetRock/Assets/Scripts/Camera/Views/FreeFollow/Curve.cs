using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curve : MonoBehaviour
{
    [SerializeField] Transform A;
    [SerializeField] Transform B;
    [SerializeField] Transform C;
    [SerializeField] Transform D;

    public Vector3 GetPosition(float t)
    {
        return MathUtils.CubicBezier(A.position, B.position, C.position, D.position, t);
    }

    public Vector3 GetPosition(float t, Matrix4x4 localToWorldMatrix)
    {
        return localToWorldMatrix.MultiplyPoint(GetPosition(t));
    }

    public void DrawGizmo(Color c, Matrix4x4 localToWorldMatrix)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(localToWorldMatrix.MultiplyPoint(A.position), 0.1f);
        Gizmos.DrawSphere(localToWorldMatrix.MultiplyPoint(B.position), 0.1f);
        Gizmos.DrawSphere(localToWorldMatrix.MultiplyPoint(C.position), 0.1f);
        Gizmos.DrawSphere(localToWorldMatrix.MultiplyPoint(D.position), 0.1f);

        Gizmos.color = c;
        for (int i=0; i < 30; ++i)
        {
            Gizmos.DrawSphere(GetPosition((float)i / 30f, localToWorldMatrix), 0.1f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils
{
    static Vector3 LinearBezier(Vector3 A, Vector3 B, float t)
    {
        return (1 - t) * A + t * B;
    }

    static Vector3 QuadraticBezier(Vector3 A, Vector3 B, Vector3 C, float t)
    {
        return (1 - t) * LinearBezier(A, B, t) + t * LinearBezier(B, C, t);
    }

    public static Vector3 CubicBezier(Vector3 A, Vector3 B, Vector3 C, Vector3 D, float t)
    {
        return (1 - t) * QuadraticBezier(A, B, C, t) + t * QuadraticBezier(B, C, D, t);
    }

}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ExtensionMethods
{
    public static Vector2 Rotate(this Vector2 vector, float eulerAngle)
    {
        float radians = eulerAngle * Mathf.Deg2Rad;
        return new Vector2(
            vector.x * Mathf.Cos(radians) - vector.y * Mathf.Sin(radians),
            vector.x * Mathf.Sin(radians) + vector.y * Mathf.Cos(radians)
        );
    }

    public static Vector3 RotateAroundPivot(this Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }

    public static Vector2 RotateAroundPivot(Vector2 point, Vector2 pivot, float angle)
    {
        return (point - pivot).Rotate(angle) + pivot;
    }
}

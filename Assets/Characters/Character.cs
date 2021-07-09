using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public float walkSpeed = 0.85f;
    public float rotationSpeed = 1f;

    /// <summary>
    /// In world space.
    /// </summary>
    /// <returns></returns>
    public abstract Vector2 GetMovement();

    /// <summary>
    /// In world space.
    /// </summary>
    /// <returns></returns>
    public abstract Quaternion GetRotation();

    public virtual Quaternion Getcostam()
    {
        return Quaternion.identity;
    }
}

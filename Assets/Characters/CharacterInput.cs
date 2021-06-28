using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterInput : MonoBehaviour
{
    /// <summary>
    /// In world space.
    /// </summary>
    /// <returns></returns>
    public abstract Vector2 GetDesiredMovement();

    /// <summary>
    /// In world space.
    /// </summary>
    /// <returns></returns>
    public abstract Quaternion GetDesiredRotation();
}

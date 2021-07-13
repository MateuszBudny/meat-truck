using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterState 
{
    protected Character character;

    public abstract Vector2 GetMovement();

    public abstract Quaternion GetRotation();

    /// <summary>
    /// Returns if the current state can be changed to the new state. Do not use directly!
    /// </summary>
    /// <param name="newState"></param>
    /// <returns></returns>
    public abstract bool ChangeState(CharacterState newState);

    public virtual void OnTriggerEnter(Collider collider) {}
}

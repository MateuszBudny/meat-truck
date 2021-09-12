using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterState
{
    protected Character character;

    public abstract Vector2 GetMovement();

    public abstract Quaternion GetRotation();

    public virtual void OnStateEnter(CharacterState previousState) {}

    public virtual void OnStateExit(CharacterState nextState) {}

    public virtual void OnTriggerEnter(Collider collider) {}
}

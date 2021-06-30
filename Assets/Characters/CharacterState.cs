using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterState
{
    protected NpcCharacter character;

    public CharacterState(NpcCharacter character)
    {
        this.character = character;
    }

    public abstract Vector2 GetDesiredMovement();

    public abstract Quaternion GetDesiredRotation();

    /// <summary>
    /// Returns if the current state can be changed to the new state.
    /// </summary>
    /// <param name="newState"></param>
    /// <returns></returns>
    public abstract bool ChangeState(CharacterState newState);
}

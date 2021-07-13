using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadNpcCharacterState : NpcCharacterState
{
    public DeadNpcCharacterState(NpcCharacter character) : base(character) {}

    public override bool ChangeState(CharacterState newState)
    {
        return false;
    }

    public override Vector2 GetMovement() => Vector2.zero;

    public override Quaternion GetRotation() => Quaternion.identity;
}
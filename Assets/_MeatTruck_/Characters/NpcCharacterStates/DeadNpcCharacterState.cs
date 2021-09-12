using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadNpcCharacterState : NpcCharacterState
{
    public DeadNpcCharacterState(NpcCharacter npcCharacter) : base(npcCharacter) {}

    public override void OnStateEnter(CharacterState previousState)
    {
        NpcCharacter.Controller.SetAsRagdoll();
    }

    public override Vector2 GetMovement() => Vector2.zero;

    public override Quaternion GetRotation() => Quaternion.identity;
}

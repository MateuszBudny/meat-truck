using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveNpcCharacterState : NpcCharacterState
{
    public AliveNpcCharacterState(NpcCharacter character) : base(character) {}

    public override bool ChangeState(CharacterState newState)
    {
        switch(newState)
        {
            case DeadNpcCharacterState _:
                character.SetAsRagdoll();
                return true;
            default:
                return false;
        }
    }

    public override Vector2 GetMovement()
    {
        if (character.Tracker.CurrentWaypoint)
        {
            return new Vector2(character.transform.forward.x, character.transform.forward.z) * character.walkSpeed;
        }
        else
        {
            return Vector2.zero;
        }
    }

    public override Quaternion GetRotation()
    {
       return Quaternion.Slerp(character.transform.rotation, GetDesiredRotation(), character.rotationSpeed * Time.deltaTime);
    }

    private Quaternion GetDesiredRotation()
    {
        if (character.Tracker.CurrentWaypoint)
        {
            Vector3 waypointDirection = character.Tracker.CurrentWaypoint.transform.position - character.transform.position;
            return Quaternion.LookRotation(new Vector3(
                waypointDirection.x,
                0f,
                waypointDirection.z));
        }
        else
        {
            return character.transform.rotation;
        }
    }
}

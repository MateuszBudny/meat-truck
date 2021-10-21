using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveNpcCharacterState : NpcCharacterState
{
    public AliveNpcCharacterState(NpcCharacterBehaviour npcCharacter) : base(npcCharacter) {}

    public override Vector2 GetMovement()
    {
        if (NpcCharacter.Tracker.CurrentWaypoint)
        {
            return new Vector2(characterBehaviour.transform.forward.x, characterBehaviour.transform.forward.z) * characterBehaviour.walkSpeed;
        }
        else
        {
            return Vector2.zero;
        }
    }

    public override Quaternion GetRotation()
    {
       return Quaternion.Slerp(characterBehaviour.transform.rotation, GetDesiredRotation(), characterBehaviour.rotationSpeed * Time.deltaTime);
    }

    private Quaternion GetDesiredRotation()
    {
        if (NpcCharacter.Tracker.CurrentWaypoint)
        {
            Vector3 waypointDirection = NpcCharacter.Tracker.CurrentWaypoint.transform.position - characterBehaviour.transform.position;
            return Quaternion.LookRotation(new Vector3(
                waypointDirection.x,
                0f,
                waypointDirection.z));
        }
        else
        {
            return characterBehaviour.transform.rotation;
        }
    }

    public override void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(Tags.PlayerVehicle.ToString()) || collider.CompareTag(Tags.NpcVehicle.ToString()))
        {
            NpcCharacter.ChangeState(new DeadNpcCharacterState(NpcCharacter));
        }
    }
}

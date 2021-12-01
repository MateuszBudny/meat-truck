using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingNpcCharacterState : IdleNpcCharacterState
{
    public WalkingNpcCharacterState(NpcCharacterBehaviour npcCharacter) : base(npcCharacter) {}

    public override void OnUpdate()
    {
        if(IsOnRoute)
        {
            SimpleMove();
        }
        else
        {
            OnRouteFinished();
        }
    }

    protected void SimpleMove()
    {
        Vector3 movement = new Vector3(
                GetMovement().x,
                0f,
                GetMovement().y);

        NpcCharacter.Controller.SimpleMove(movement, GetRotation());
    }

    protected virtual bool IsOnRoute => NpcCharacter.Tracker.CurrentWaypoint;

    protected virtual void OnRouteFinished()
    {
        NpcCharacter.ChangeState(new IdleNpcCharacterState(NpcCharacter));
    }

    public override Vector2 GetMovement()
    {
        if (NpcCharacter.Tracker.CurrentWaypoint)
        {
            return new Vector2(characterBehaviour.transform.forward.x, characterBehaviour.transform.forward.z) * NpcCharacter.walkSpeed;
        }
        else
        {
            return Vector2.zero;
        }
    }

    public override Quaternion GetRotation()
    {
        if (NpcCharacter.Tracker.CurrentWaypoint)
        {
            return GetSlerpedRotationTowardsTarget(NpcCharacter.Tracker.CurrentWaypoint.transform.position, NpcCharacter.rotationSpeed);
        }
        else
        {
            return NpcCharacter.transform.rotation;
        }
    }

    protected Quaternion GetSlerpedRotationTowardsTarget(Vector3 targetPosition, float rotationSpeed)
    {
        return Quaternion.Slerp(NpcCharacter.transform.rotation, GetDesiredRotation(targetPosition), rotationSpeed * Time.deltaTime);
    }

    private Quaternion GetDesiredRotation(Vector3 targetPosition)
    {
        Vector3 waypointDirection = targetPosition - characterBehaviour.transform.position;
        return Quaternion.LookRotation(new Vector3(
            waypointDirection.x,
            0f,
            waypointDirection.z));
    }
}

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

        NpcCharacterBehaviour.Controller.SimpleMove(movement, GetRotation());
    }

    protected virtual bool IsOnRoute => NpcCharacterBehaviour.Tracker.CurrentWaypoint;

    protected virtual void OnRouteFinished()
    {
        NpcCharacterBehaviour.ChangeState(new IdleNpcCharacterState(NpcCharacterBehaviour));
    }

    public override Vector2 GetMovement()
    {
        if (NpcCharacterBehaviour.Tracker.CurrentWaypoint)
        {
            return new Vector2(characterBehaviour.transform.forward.x, characterBehaviour.transform.forward.z) * NpcCharacterBehaviour.walkSpeed;
        }
        else
        {
            return Vector2.zero;
        }
    }

    public override Quaternion GetRotation()
    {
        if (NpcCharacterBehaviour.Tracker.CurrentWaypoint)
        {
            return GetSlerpedRotationTowardsTarget(NpcCharacterBehaviour.Tracker.CurrentWaypoint.transform.position, NpcCharacterBehaviour.rotationSpeed);
        }
        else
        {
            return NpcCharacterBehaviour.transform.rotation;
        }
    }

    protected Quaternion GetSlerpedRotationTowardsTarget(Vector3 targetPosition, float rotationSpeed)
    {
        return Quaternion.Slerp(NpcCharacterBehaviour.transform.rotation, GetDesiredRotation(targetPosition), rotationSpeed * Time.deltaTime);
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

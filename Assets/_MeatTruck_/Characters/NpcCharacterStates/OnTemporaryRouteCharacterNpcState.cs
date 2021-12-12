using System;
using UnityEngine;

public class OnTemporaryRouteCharacterNpcState : WalkingNpcCharacterState
{
    protected Action onCompleted;
    protected bool returnOnRoute;
    protected WaypointsTracker tempTracker;

    public OnTemporaryRouteCharacterNpcState(NpcCharacterBehaviour npcCharacter, Waypoint startingWaypoint, Action onCompleted, bool returnOnRoute) : base(npcCharacter)
    {
        this.onCompleted = onCompleted;
        this.returnOnRoute = returnOnRoute;

        tempTracker = NpcCharacter.gameObject.AddComponent<WaypointsTracker>();
        tempTracker.CurrentWaypoint = startingWaypoint;
    }

    public override void OnStateExit(CharacterState nextState)
    {
        UnityEngine.Object.Destroy(tempTracker);
    }

    protected override void OnRouteFinished()
    {
        if(returnOnRoute)
        {
            NpcCharacter.ChangeState(new WalkingNpcCharacterState(NpcCharacter));
        }
        else
        {
            base.OnRouteFinished();
        }

        onCompleted();
    }

    protected override bool IsOnRoute => tempTracker.CurrentWaypoint;

    public override Quaternion GetRotation()
    {
        return GetSlerpedRotationTowardsTarget(tempTracker.CurrentWaypoint.transform.position, characterBehaviour.rotationSpeed);
    }
}
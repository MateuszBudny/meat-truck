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

        tempTracker = NpcCharacterBehaviour.gameObject.AddComponent<WaypointsTracker>();
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
            NpcCharacterBehaviour.ChangeState(new WalkingNpcCharacterState(NpcCharacterBehaviour));
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
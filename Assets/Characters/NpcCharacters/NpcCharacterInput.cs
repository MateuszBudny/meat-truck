using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WaypointsTracker))]
public class NpcCharacterInput : CharacterInput
{
    private WaypointsTracker tracker;

    public override Vector2 GetDesiredMovement()
    {
        if(tracker.CurrentWaypoint)
        {
            return new Vector2(transform.forward.x, transform.forward.z);
        }
        else
        {
            return Vector2.zero;
        }
    }

    public override Quaternion GetDesiredRotation()
    {
        if (tracker.CurrentWaypoint)
        {
            Vector3 waypointDirection = tracker.CurrentWaypoint.transform.position - transform.position;
            return Quaternion.LookRotation(new Vector3(
                waypointDirection.x,
                0f,
                waypointDirection.z));
        }
        else
        {
            return transform.rotation;
        }
    }

    private void Awake()
    {
        tracker = GetComponent<WaypointsTracker>();
    }
}

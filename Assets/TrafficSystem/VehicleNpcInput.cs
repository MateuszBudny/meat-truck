using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WaypointsTracker))]
public class VehicleNpcInput : VehicleInput
{
    private WaypointsTracker tracker;

    protected override void Awake()
    {
        base.Awake();
        tracker = GetComponent<WaypointsTracker>();
    }

    public override float GetCurrentAcceleration()
    {
        if(tracker.CurrentWaypoint != null)
        {
            return 1f;
        }
        else
        {
            return 0f;
        }
    }

    public override float GetCurrentBraking()
    {
        if(tracker.CurrentWaypoint != null)
        {
            return 0f;
        }
        else
        {
            return 1f;
        }
    }

    public override float GetCurrentSteeringAngle()
    {
        if(tracker.CurrentWaypoint != null)
        {
            Vector3 waypointDirection = tracker.CurrentWaypoint.transform.position - transform.position;
            Vector3 vehicleForward = transform.forward;
            float angleToSteer = Vector3.SignedAngle(waypointDirection, vehicleForward, Vector3.down);
            float clampedAngleToSteer = Mathf.Clamp(angleToSteer, -VehicleController.maxSteerAngle, VehicleController.maxSteerAngle);

            return clampedAngleToSteer;
        }
        else
        {
            return 0f;
        }
    }
}

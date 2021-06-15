using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleNpcInput : VehicleInput
{
    [SerializeField]
    private Waypoint startingWaypoint;

    private Waypoint currentWaypoint;

    private void Awake()
    {
        BaseAwake();
        currentWaypoint = startingWaypoint;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag(Tags.Waypoint.ToString()))
        {
            if(collider.gameObject == currentWaypoint.gameObject)
            {
                currentWaypoint = currentWaypoint.nextWaypoints.GetRandomWeightedWaypoint();
            }
        }
    }

    public override float GetCurrentAcceleration()
    {
        if(currentWaypoint != null)
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
        if(currentWaypoint != null)
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
        if(currentWaypoint != null)
        {
            Vector3 waypointDirection = currentWaypoint.transform.position - transform.position;
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointsTracker : MonoBehaviour
{
    [SerializeField]
    private Waypoint startingWaypoint;

    public Waypoint CurrentWaypoint { get; private set; }

    private void Awake()
    {
        CurrentWaypoint = startingWaypoint;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(Tags.Waypoint.ToString()))
        {
            if (collider.gameObject == CurrentWaypoint.gameObject)
            {
                CurrentWaypoint = CurrentWaypoint.nextWaypoints.GetRandomWeightedWaypoint();
            }
        }
    }
}

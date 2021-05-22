using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Possible improvements:
// - CustomPropertyDrawer for WaypointRecord, so there is a button to create new waypoint between two waypoints
// - width property for Waypoints, so AIs are moving in a range of width, not pixel perfect as is now

public class Waypoint : MonoBehaviour
{
    public WaypointsHandler previousWaypoints;
    public WaypointsHandler nextWaypoints;

    public void AddPreviousWaypoint(Waypoint newPreviousWaypoint)
    {
        previousWaypoints.Add(newPreviousWaypoint);
        newPreviousWaypoint.nextWaypoints.Add(this);
    }

    public void AddNextWaypoint(Waypoint newNextWaypoint)
    {
        nextWaypoints.Add(newNextWaypoint);
        newNextWaypoint.previousWaypoints.Add(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.2f);
        nextWaypoints.Waypoints.ForEach(nextWaypoint =>
        {
            bool doesNextWaypointHaveThisWaypointAsPreviousWaypoint = nextWaypoint.previousWaypoints.Waypoints.Any(previousWaypointOfNextWaypoint => previousWaypointOfNextWaypoint == this);
            Color lineWithArrowGizmoColor = doesNextWaypointHaveThisWaypointAsPreviousWaypoint ? Color.green : Color.red;
            DrawArrow.ForGizmoTwoPoints(transform.position, nextWaypoint.transform.position, lineWithArrowGizmoColor);
        });
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
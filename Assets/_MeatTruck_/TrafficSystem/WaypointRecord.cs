using System;
using UnityEngine;

[Serializable]
public class WaypointRecord : SerializableWithValidation
{
    public Waypoint waypoint;
    /// <summary>
    /// The higher the weight, the highter the frequency of choosing this waypoint, if there are multiple waypoints. E.g. a waypoint of weight 2f will be chosen two times more frequent, than a waypoint of weight 1f.
    /// </summary>
    [Tooltip("The higher the weight, the highter the frequency of choosing this waypoint if there are multiple waypoints. E.g. a waypoint of weight 2f will be chosen two times more frequent, than a waypoint of weight 1f. Weight cannot be smaller than 0f.")]
    public float weight = 1f;

    public WaypointRecord(Waypoint waypoint, float weight = 1f)
    {
        this.waypoint = waypoint;
        this.weight = weight;
    }

    protected override void OnValidate()
    {
        weight = weight > 0f ? weight : 0f;
    }
}
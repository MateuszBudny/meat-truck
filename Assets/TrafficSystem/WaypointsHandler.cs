using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class WaypointsHandler
{
    public List<WaypointRecord> waypointsRecords;

    public List<Waypoint> Waypoints => waypointsRecords?.Select(record => record.waypoint).ToList();
    public float WeightsSum => waypointsRecords.Sum(record => record.weight);

    public void Add(Waypoint waypoint)
    {
        waypointsRecords.Add(new WaypointRecord(waypoint));
    }

    public void Remove(Waypoint waypoint)
    {
        WaypointRecord recordToRemove = waypointsRecords.Find(record => record.waypoint == waypoint);
        waypointsRecords.Remove(recordToRemove);
    }

    public void Replace(Waypoint waypointToRemove, Waypoint waypointToAdd)
    {
        Remove(waypointToRemove);
        Add(waypointToAdd);
    }

    public Waypoint GetRandomWeightedWaypoint()
    {
        float weightsSum = WeightsSum;
        float randomWeight = Random.Range(0f, weightsSum);
        float currentWeightsSum = 0f;
        WaypointRecord randomWeightedRecord = waypointsRecords.Find(record =>
        {
            currentWeightsSum += record.weight;
            return currentWeightsSum >= randomWeight;
        });

        return randomWeightedRecord.waypoint;
    }

    public void SortWaypointsByWeightDescending()
    {
        waypointsRecords.Sort((record1, record2) => -record1.weight.CompareTo(record2.weight));
    }
}

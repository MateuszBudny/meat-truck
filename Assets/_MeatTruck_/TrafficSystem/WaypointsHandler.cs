using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class WaypointsHandler
{
    public WeightedList<WaypointRecord> weightedWaypointsRecords;

    public List<Waypoint> Waypoints => weightedWaypointsRecords?.List.Select(record => record.waypoint).ToList();
    public float WeightsSum => weightedWaypointsRecords.List.Sum(record => record.weight);

    public void Add(Waypoint waypoint)
    {
        weightedWaypointsRecords.List.Add(new WaypointRecord(waypoint));
    }

    public void Remove(Waypoint waypoint)
    {
        WaypointRecord recordToRemove = weightedWaypointsRecords.List.Find(record => record.waypoint == waypoint);
        weightedWaypointsRecords.List.Remove(recordToRemove);
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
        WaypointRecord randomWeightedRecord = weightedWaypointsRecords.List.Find(record =>
        {
            currentWeightsSum += record.weight;
            return currentWeightsSum >= randomWeight;
        });

        return randomWeightedRecord?.waypoint;
    }

    public void SortWaypointsByWeightDescending()
    {
        weightedWaypointsRecords.List.Sort((record1, record2) => -record1.weight.CompareTo(record2.weight));
    }
}

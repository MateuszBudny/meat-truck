using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Waypoint))]
public class WaypointEditor : Editor
{
    private Waypoint Target => target as Waypoint;
    private GameObject WaypointPrefab => Resources.Load("Waypoint") as GameObject;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Create another previous Waypoint (key: PageDown)"))
        {
            AddNewPreviousWaypointToTarget();
        }

        if (GUILayout.Button("Create another next Waypoint (key: PageUp)"))
        {
            AddNewNextWaypointToTarget();
        }

        if (GUILayout.Button("Delet dis Waypoint (key: Delete)"))
        {
            DeleteThisWaypoint();
        }
    }

    private void OnSceneGUI()
    {
        Event e = Event.current;
        if (Selection.activeGameObject == Target.gameObject)
        {
            if (e.type == EventType.KeyDown)
            {
                switch (Event.current.keyCode)
                {
                    case KeyCode.PageDown:
                        AddNewPreviousWaypointToTarget();
                        e.Use();
                        break;
                    case KeyCode.PageUp:
                        AddNewNextWaypointToTarget();
                        e.Use();
                        break;
                    case KeyCode.Delete:
                        DeleteThisWaypoint();
                        e.Use();
                        break;
                }
            }
        }
    }

    private void AddNewPreviousWaypointToTarget()
    {
        AddWaypointToTarget(Target.AddPreviousWaypoint);
    }

    private void AddNewNextWaypointToTarget()
    {
        AddWaypointToTarget(Target.AddNextWaypoint);
    }

    private void DeleteThisWaypoint()
    {
        Target.previousWaypoints.Waypoints.ForEach(previousWaypoint =>
        {
            StartRecordingObjectForUndo(previousWaypoint, "Waypoints update after deletion.");
            Target.nextWaypoints.Waypoints.ForEach(nextWaypoint =>
            {
                StartRecordingObjectForUndo(nextWaypoint, "Waypoints update after deletion.");
                previousWaypoint.AddNextWaypoint(nextWaypoint);
            });

            previousWaypoint.nextWaypoints.Remove(Target);
            MarkDirty(previousWaypoint);
        });

        Target.nextWaypoints.Waypoints.ForEach(nextWaypoint =>
        {
            StartRecordingObjectForUndo(nextWaypoint, "Waypoints update after deletion.");
            nextWaypoint.previousWaypoints.Remove(Target);
            MarkDirty(nextWaypoint);
        });

        DestroyAndMarkAsDestroyed(Target.gameObject);
    }

    private void AddWaypointToTarget(Action<Waypoint> addWaypointToTargetAction)
    {
        StartRecordingObjectForUndo(Target, "Waypoints update after adding new Waypoint.");
        Waypoint newWaypoint = InstantiateNewWaypoint();
        addWaypointToTargetAction(newWaypoint);

        MarkDirty(Target);
        MarkDirty(newWaypoint);
    }

    private Waypoint InstantiateNewWaypoint()
    {
        Waypoint newWaypoint = (PrefabUtility.InstantiatePrefab(WaypointPrefab, Target.transform.parent) as GameObject).GetComponent<Waypoint>();
        newWaypoint.name = Target.name;
        newWaypoint.transform.position = Target.transform.position;
        Selection.activeGameObject = newWaypoint.gameObject;
        RegisterCreationForUndo(newWaypoint.gameObject, "Waypoints update after adding new Waypoint.");

        return newWaypoint;
    }

    private void StartRecordingObjectForUndo(UnityEngine.Object obj, string undoName)
    {
        Undo.RecordObject(obj, undoName);
    }

    private void MarkDirty(UnityEngine.Object obj)
    {
        
        PrefabUtility.RecordPrefabInstancePropertyModifications(obj);
        CollapseUndo();
    }

    private void RegisterCreationForUndo(UnityEngine.Object obj, string undoName)
    {
        Undo.RegisterCreatedObjectUndo(obj, undoName);
        CollapseUndo();
    }

    private void DestroyAndMarkAsDestroyed(UnityEngine.Object obj)
    {
        Undo.DestroyObjectImmediate(obj);
        CollapseUndo();
    }

    private void CollapseUndo()
    {
        Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
    }
}

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
        if (GUILayout.Button("Create another previous Waypoint"))
        {
            Target.AddPreviousWaypoint(InstantiateNewWaypoint());
        }

        if (GUILayout.Button("Create another next Waypoint"))
        {
            Target.AddNextWaypoint(InstantiateNewWaypoint());
        }

        if (GUILayout.Button("Delet dis Waypoint"))
        {
            Target.previousWaypoints.Waypoints.ForEach(previousWaypoint =>
            {
                previousWaypoint.nextWaypoints.Remove(Target);
                Target.nextWaypoints.Waypoints.ForEach(nextWaypoint =>
                {
                    previousWaypoint.AddNextWaypoint(nextWaypoint);
                    nextWaypoint.previousWaypoints.Remove(Target);
                    MarkDirty(previousWaypoint, "Waypoints update after deletion.");
                    MarkDirty(nextWaypoint, "Waypoints update after deletion.");
                });
            });

            // TODO: doesn't remove Target reference from previousWaypoints, when there are no nextWaypoints.

            DestroyAndMarkAsDestroyed(Target.gameObject); // TODO: not everything is saved to undo in this case
        }
    }

    private Waypoint InstantiateNewWaypoint()
    {
        Waypoint newWaypoint = (PrefabUtility.InstantiatePrefab(WaypointPrefab, Target.transform.parent) as GameObject).GetComponent<Waypoint>();
        newWaypoint.name = Target.name;
        newWaypoint.transform.position = Target.transform.position;
        Selection.activeGameObject = newWaypoint.gameObject;
        MarkDirty(Target, "Waypoints update after adding new Waypoint.");
        MarkCreation(newWaypoint.gameObject, "Waypoints update after adding new Waypoint.");

        return newWaypoint;
    }

    private void MarkDirty(Object gameObject, string undoName)
    {
        Undo.RecordObject(gameObject, undoName);
        PrefabUtility.RecordPrefabInstancePropertyModifications(gameObject);
        CollapseUndo();
    }

    private void MarkCreation(Object gameObject, string undoName)
    {
        Undo.RegisterCreatedObjectUndo(gameObject, undoName);
        CollapseUndo();
    }

    private void DestroyAndMarkAsDestroyed(Object gameObject)
    {
        Undo.DestroyObjectImmediate(gameObject);
        CollapseUndo();
    }

    private void CollapseUndo()
    {
        Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
    }
}

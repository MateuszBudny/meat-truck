using UnityEngine;
using System.Collections;
using UnityEditor;
using LowkeyFramework.AttributeSaveSystem;

[CustomEditor(typeof(SaveManager))]
public class SaveManagerEditor : Editor
{

    private string saveName = "test1234";
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SaveManager myScript = (SaveManager)target;
        if(GUILayout.Button("Save"))
        {
            myScript.Save(saveName);
        }

        if(GUILayout.Button("Save Encoded"))
        {
            myScript.Save(saveName,true);
        }

        if(GUILayout.Button("Load"))
        {
            myScript.Load(saveName);
        }

    }
}

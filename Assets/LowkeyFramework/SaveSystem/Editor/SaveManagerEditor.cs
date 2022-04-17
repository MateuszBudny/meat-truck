using UnityEngine;
using System.Collections;
using System.Text;
using System;
using System.Collections.Generic;
using UnityEditor;
using LowkeyFramework.AttributeSaveSystem;

[CustomEditor(typeof(SaveManager))]
public class SaveManagerEditor : Editor
{
    private string testSaveName = "test1234";

    private SaveManager Target => (SaveManager)target;

    private JSONEditor jsonEditor;

    private void OnEnable()
    {
        jsonEditor = ScriptableObject.CreateInstance<JSONEditor>();
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if(GUILayout.Button("Save"))
        {
            Target.Save(testSaveName);
        }

        if(GUILayout.Button("Save Encoded"))
        {
            Target.Save(testSaveName, true);
        }

        if(GUILayout.Button("Load"))
        {
            Target.Load(testSaveName);
            Target.LoadDecodeSaveFile(testSaveName + ".json", out string jsonFile);
            jsonEditor.rawText = jsonFile;
        }

        if(GUILayout.Button("Generate random key")){
            Target.key = RandomString(16);
            EditorUtility.SetDirty(Target);
        }

        jsonEditor.JsonInspectorGUI();

    }


        private static string RandomString(int length)
        {
        const string pool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        var builder = new StringBuilder();
        var random = new System.Random();

        for (var i = 0; i < length; i++)
        {
            var c = pool[random.Next(0, pool.Length)];
            builder.Append(c);
        }

        return builder.ToString();
    }
}

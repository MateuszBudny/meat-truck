using UnityEngine;
using System.Collections;
using System.Text;
using System;
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

        if(GUILayout.Button("Make random key")){
            myScript.key = RandomString(16);
        }

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

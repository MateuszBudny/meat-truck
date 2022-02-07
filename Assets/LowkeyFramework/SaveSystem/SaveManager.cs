using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class SaveManager : SingleBehaviour<SaveManager>
{
    // TODO:
    // is Newtonsoft using serialized fields only? Or is it ignoring Unity serialization? - it looks like it is ignoring serialization, but if field is set as [NonSerialized], then it is not added in json
    // lists
    // ScriptableObject by GUID reference
    // ScriptableObject by values and toggle to choose reference saving or values
    // encryption and toggle to turn it on or off (plus possibility to toggle it from script)
    // displaying contents of save files in Inspector
    // editing contents of save files in Inspector
    // add virtual function to Saveables abstract to add some way of extending this system, if someone would like to save Transform or other Unity components. Or add other way to do that, but for now I think virtual function would be the best solution
    // find a way to use SaveFileName enum or other type, so end user won't have to edit file created by me

    [SerializeField]
    private SaveFileName defaultSaveFile = 0;
    public SaveFileName DefaultSaveFile { get => defaultSaveFile; set => defaultSaveFile = value; }

    public void Save(SaveFileName saveFileName) => Save(saveFileName.ToString());

    public void Save(string saveFileName = "")
    {
        string jsonSaveFileName = GetJsonSaveFileName(saveFileName);

        // Dictionary<behaviour's GUID, Dictionary<field's name, field as object>> 
        Dictionary<string, Dictionary<string, object>> saveDictionary = new Dictionary<string, Dictionary<string, object>>();
        ForEachSaveField((behaviour, _, fieldInfo) =>
        {
            if (!saveDictionary.ContainsKey(behaviour.Guid))
            {
                saveDictionary.Add(behaviour.Guid, new Dictionary<string, object>());
            }
            saveDictionary[behaviour.Guid].Add(fieldInfo.Name, fieldInfo.GetValue(behaviour));
        });

        string jsonSave = JsonConvert.SerializeObject(saveDictionary);

        FileManager.MoveFile(jsonSaveFileName, jsonSaveFileName + ".bak");
        if (FileManager.WriteToFile(jsonSaveFileName, jsonSave))
        {
            Debug.Log("Save successful: " + jsonSaveFileName);
        }
    }

    public void Load(SaveFileName saveFileName) => Load(saveFileName.ToString());

    public void Load(string saveFileName = "")
    {
        string jsonSaveFileName = GetJsonSaveFileName(saveFileName);

        if (FileManager.LoadFromFile(jsonSaveFileName, out string saveJson))
        {
            // Dictionary<behaviour's GUID, Dictionary<field's name, field as object>> 
            Dictionary<string, Dictionary<string, object>> saveDictionary = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(saveJson);
            ForEachSaveField((behaviour, _, fieldInfo) =>
            {
                string guid = behaviour.Guid;
                string fieldName = fieldInfo.Name;
                Dictionary<string, object> savedFieldsDictionary = saveDictionary[guid];
                object fieldSavedValue = savedFieldsDictionary[fieldName];
                var T = fieldInfo.FieldType;
                fieldInfo.SetValue(behaviour, (fieldSavedValue as JObject).ToObject(fieldInfo.FieldType));
            });

            Debug.Log("Load successful: " + jsonSaveFileName);
        }
    }

    private void ForEachSaveField(Action<SaveableBehaviour, SaveField, FieldInfo> forEachSaveField)
    {
        List<SaveableBehaviour> behaviours = GetSerializedBehaviours();
        behaviours.ForEach(behaviour =>
        {
            List<FieldInfo> objectFields = behaviour.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
            objectFields.ForEach(fieldInfo =>
            {
                SaveField saveField = Attribute.GetCustomAttribute(fieldInfo, typeof(SaveField)) as SaveField;
                if (saveField != null)
                {
                    forEachSaveField(behaviour, saveField, fieldInfo);
                }
            });
        });
    }

    private List<SaveableBehaviour> GetSerializedBehaviours()
    {
        return Resources.FindObjectsOfTypeAll(typeof(SaveableBehaviour))
            .Select(behaviour => (SaveableBehaviour)behaviour)
            .Where(behaviour => IsGameObjectOnScene(behaviour.gameObject))
            .ToList();
    }


    private bool IsGameObjectOnScene(GameObject gameObject) => !EditorUtility.IsPersistent(gameObject.transform.root.gameObject) && !(gameObject.hideFlags == HideFlags.NotEditable || gameObject.hideFlags == HideFlags.HideAndDontSave);

    private string GetJsonSaveFileName(string currentSaveFileName)
    {
        return (string.IsNullOrEmpty(currentSaveFileName) ? DefaultSaveFile.ToString() : currentSaveFileName) + ".json";
    }
}
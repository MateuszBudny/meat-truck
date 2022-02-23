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
    // ScriptableObject by values and toggle to choose reference saving or values
    // encryption and toggle to turn it on or off (plus possibility to toggle it from script)
    // properties also should have possibility to set them as [SaveField]
    // find a way to use SaveFileName enum or other type, so end user won't have to edit a file created by me
    // add virtual function to Saveables abstract to add some way of extending this system, if someone would like to save Transform or other Unity component. Or add other way to do that, but for now I think virtual function would be the best solution. EDIT: Custom JsonConverters for Unity (there is such a github project) should do just fine.
    // interfaces with BeforeSave(), AfterLoad() (optionally AfterSave() and BeforeLoad())
    // clearing saves from Unity window
    // displaying contents of save files in Inspector
    // editing contents of save files in Inspector

    [SerializeField]
    private SaveFileName defaultSaveFile = 0;
    public SaveFileName DefaultSaveFile { get => defaultSaveFile; set => defaultSaveFile = value; }

    [SerializeField]
    private bool appendSaves = true;
    public bool AppendSaves { get => appendSaves; set => appendSaves = value; }

    public static List<JsonConverter> AdditionalCustomConverters = new List<JsonConverter>();

    public void Save(SaveFileName saveFileName) => Save(saveFileName.ToString());

    public void Save(string saveFileName = "")
    {
        string jsonSaveFileName = GetJsonSaveFileName(saveFileName);

        // Dictionary<behaviour's GUID, Dictionary<field's name, field as object>> 
        Dictionary<string, Dictionary<string, object>> saveDictionary;
        if (appendSaves && FileManager.LoadFromFile(jsonSaveFileName, out string saveJson) && !string.IsNullOrEmpty(saveJson))
        {
            saveDictionary = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(saveJson, GetCustomJsonConverters());
        }
        else
        {
            saveDictionary = new Dictionary<string, Dictionary<string, object>>();
        }


        ForEachSaveField(
            forEachSaveableBehaviour: behaviour =>
            {
                saveDictionary.Remove(behaviour.Guid);
            },
            forEachSaveField: (behaviour, _, fieldInfo) =>
            {
                if (!saveDictionary.ContainsKey(behaviour.Guid))
                {
                    saveDictionary.Add(behaviour.Guid, new Dictionary<string, object>());
                }
                saveDictionary[behaviour.Guid].Add(fieldInfo.Name, fieldInfo.GetValue(behaviour));
            });

        string jsonSave = JsonConvert.SerializeObject(saveDictionary, Formatting.Indented, GetCustomJsonConverters());

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

        if (FileManager.LoadFromFile(jsonSaveFileName, out string saveJson) && !string.IsNullOrEmpty(saveJson))
        {
            // Dictionary<behaviour's GUID, Dictionary<field's name, field as object>> 
            Dictionary<string, Dictionary<string, object>> saveDictionary = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(saveJson, GetCustomJsonConverters());
            ForEachSaveField((behaviour, _, fieldInfo) =>
            {
                string guid = behaviour.Guid;
                string fieldName = fieldInfo.Name;
                if (saveDictionary.TryGetValue(guid, out Dictionary<string, object> savedFieldsDictionary))
                {
                    object fieldSavedValue = savedFieldsDictionary[fieldName];
                    object fieldSavedValueAfterConvertion = (fieldSavedValue as JToken).ToObject(fieldInfo.FieldType, GetJsonSerializerWithCustomJsonConverters());
                    fieldInfo.SetValue(behaviour, fieldSavedValueAfterConvertion);
                }
            });

            Debug.Log("Load successful: " + jsonSaveFileName);
        }
    }

    private void ForEachSaveField(Action<SaveableBehaviour, SaveFieldAttribute, FieldInfo> forEachSaveField, Action<SaveableBehaviour> forEachSaveableBehaviour = null)
    {
        List<SaveableBehaviour> behaviours = GetSaveableBehaviours();
        behaviours.ForEach(behaviour =>
        {
            forEachSaveableBehaviour?.Invoke(behaviour);
            List<FieldInfo> objectFields = behaviour.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
            objectFields.ForEach(fieldInfo =>
            {
                SaveFieldAttribute saveField = Attribute.GetCustomAttribute(fieldInfo, typeof(SaveFieldAttribute)) as SaveFieldAttribute;
                if (saveField != null)
                {
                    forEachSaveField(behaviour, saveField, fieldInfo);
                }
            });
        });
    }

    private List<SaveableBehaviour> GetSaveableBehaviours()
    {
        return Resources.FindObjectsOfTypeAll(typeof(SaveableBehaviour))
            .Select(behaviour => (SaveableBehaviour)behaviour)
            .Where(behaviour => IsGameObjectOnScene(behaviour.gameObject) && !behaviour.TurnOffSavingAndLoadingForThisBehaviour)
            .ToList();
    }

    public static JsonSerializer GetJsonSerializerWithCustomJsonConverters()
    {
        JsonSerializer jsonSerializerCustomConverters = new JsonSerializer();
        GetCustomJsonConverters().ToList().ForEach(customConverter =>
        {
            jsonSerializerCustomConverters.Converters.Add(customConverter);
        });

        return jsonSerializerCustomConverters;
    }

    public static JsonConverter[] GetCustomJsonConverters()
    {
        List<JsonConverter> customJsonConverters = new List<JsonConverter>(AdditionalCustomConverters);
        customJsonConverters.Add(new ReferenceableScriptableObjectJsonConverter());
        customJsonConverters.Add(new RuntimeTypedComplexKeyDictionaryJsonConverter());

        return customJsonConverters.ToArray();
    }

    private static bool IsGameObjectOnScene(GameObject gameObject) => !EditorUtility.IsPersistent(gameObject.transform.root.gameObject) && !(gameObject.hideFlags == HideFlags.NotEditable || gameObject.hideFlags == HideFlags.HideAndDontSave);

    public static Dictionary<TKey, TValue> GetDictionaryFromList<TKey, TValue>(List<KeyValuePair<TKey, TValue>> list) => list.ToDictionary(x => x.Key, x => x.Value);

    private string GetJsonSaveFileName(string currentSaveFileName)
    {
        return (string.IsNullOrEmpty(currentSaveFileName) ? DefaultSaveFile.ToString() : currentSaveFileName) + ".json";
    }
}
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace LowkeyFramework.AttributeSaveSystem
{
    public class SaveManager : SingleBehaviour<SaveManager>
    {
        // TODO:
        // is Newtonsoft using serialized fields only? Or is it ignoring Unity serialization? - it looks like it is ignoring serialization, but if field is set as [NonSerialized], then it is not added in json
        // encryption and toggle to turn it on or off (plus possibility to toggle it from script)
        // properties also should have possibility to set them as [SaveField]
        // find a way to use SaveFileName enum or other type, so end user won't have to edit a file created by me. Best way to solve this: serialized list of file names and a button to generate enum out of this.
        // add virtual function to Saveables abstract to add some way of extending this system, if someone would like to save Transform or other Unity component. Or add other way to do that, but for now I think virtual function would be the best solution. EDIT: Custom JsonConverters for Unity (there is such a github project) should do just fine.
        // clearing saves from Unity window
        // displaying contents of save files in Inspector
        // editing contents of save files in Inspector

        [SerializeField]
        private SaveFileName defaultSaveFile = 0;
        public SaveFileName DefaultSaveFile { get => defaultSaveFile; set => defaultSaveFile = value; }

        [Tooltip("If true, then new save is based on a previous save, so save is updated by new or changed values, but values that were present in a previous save and are not present in a new save, are not deleted from save. If false, then new save is based on an empty save, so all values not present in a new save will be deleted.")]
        [SerializeField]
        private bool appendSaves = true;
        /// <summary>
        /// If true, then new save is based on a previous save, so save is updated by new or changed values, but values that were present in a previous save and are not present in a new save, are not deleted from save. If false, then new save is based on an empty save, so all values not present in a new save will be deleted.
        /// </summary>
        public bool AppendSaves { get => appendSaves; set => appendSaves = value; }

        [SerializeField]
        private bool log = true;
        public bool Log { get => log; set => log = value; }

        /// <summary>
        /// An event invoked before saving is started.
        /// </summary>
        public event Action OnBeforeSave;
        /// <summary>
        /// An event happening after saving is finished. Bool variable defines if saving is successful or not.
        /// </summary>
        public event Action<bool> OnAfterSave;
        /// <summary>
        /// An event happening before loading is started.
        /// </summary>
        public event Action OnBeforeLoad;
        /// <summary>
        /// An event happening after loading is finished. Bool variable defines if loading is successful or not.
        /// </summary>
        public event Action<bool> OnAfterLoad;

        private void Start()
        {
            OnBeforeSave += () => Debug.Log("dziaa");
        }

        /// <summary>
        /// Save SaveFields to JSON file.
        /// </summary>
        /// <param name="saveFileName"></param>
        public void Save(SaveFileName saveFileName, bool encode = false) => Save(saveFileName.ToString(), encode);

        /// <summary>
        /// Save SaveFields to JSON file.
        /// </summary>
        public void Save(string saveFileName = "", bool encode = false)
        {
            OnBeforeSave?.Invoke();
            string jsonSaveFileName = GetJsonSaveFileName(saveFileName);

            // Dictionary<behaviour's GUID, Dictionary<field's name, field as object>> 
            Dictionary<string, Dictionary<string, object>> saveDictionary;
            if (appendSaves && FileManager.LoadFromFile(jsonSaveFileName, out string saveJson) && !string.IsNullOrEmpty(saveJson) && !string.IsNullOrEmpty(DecodeJsonSave(saveJson)))
            {

                saveDictionary = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(DecodeJsonSave(saveJson));
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

            string jsonSave = JsonConvert.SerializeObject(saveDictionary, Formatting.Indented);

            if (encode)
            {
                byte[] bytesToEncode = Encoding.UTF8.GetBytes(jsonSave);
                jsonSave = Convert.ToBase64String(bytesToEncode);
            }

            FileManager.MoveFile(jsonSaveFileName, jsonSaveFileName + ".bak");
            if (FileManager.WriteToFile(jsonSaveFileName, jsonSave))
            {
                if(Log)
                {
                    Debug.Log("Save successful: " + jsonSaveFileName);
                }
                OnAfterSave?.Invoke(true);
            }
            else
            {
                OnAfterSave?.Invoke(false);
            }
        }

        /// <summary>
        /// Load from JSON file and update SaveFields.
        /// </summary>
        /// <param name="saveFileName"></param>
        public void Load(SaveFileName saveFileName) => Load(saveFileName.ToString());

        /// <summary>
        /// Load from JSON file and update SaveFields.
        /// </summary>
        /// <param name="saveFileName"></param>
        public void Load(string saveFileName = "")
        {
            OnBeforeLoad?.Invoke();
            string jsonSaveFileName = GetJsonSaveFileName(saveFileName);

            if (FileManager.LoadFromFile(jsonSaveFileName, out string saveJson) && !string.IsNullOrEmpty(saveJson))
            {
                saveJson = DecodeJsonSave(saveJson);

                if(string.IsNullOrEmpty(saveJson)){
                    Debug.LogError("Save file is not in a valid format.\nSave path: " + jsonSaveFileName);
                    OnAfterLoad?.Invoke(false);
                    return;
                }


                // Dictionary<behaviour's GUID, Dictionary<field's name, field as object>> 
                Dictionary<string, Dictionary<string, object>> saveDictionary = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(saveJson);
                ForEachSaveField((behaviour, _, fieldInfo) =>
                {
                    string guid = behaviour.Guid;
                    string fieldName = fieldInfo.Name;
                    if (saveDictionary.TryGetValue(guid, out Dictionary<string, object> savedFieldsDictionary))
                    {
                        if(savedFieldsDictionary.ContainsKey(fieldName))
                        {
                            object fieldSavedValue = savedFieldsDictionary[fieldName];
                            object fieldSavedValueAfterConvertion = (fieldSavedValue as JToken).ToObject(fieldInfo.FieldType);
                            fieldInfo.SetValue(behaviour, fieldSavedValueAfterConvertion);
                        }
                    }
                });

                if(Log)
                {
                    Debug.Log("Load successful: " + jsonSaveFileName);
                }
                OnAfterLoad?.Invoke(true);
            }
            else
            {
                OnAfterLoad?.Invoke(false);
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
            List<JsonConverter> customJsonConverters = new List<JsonConverter>();
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

        private bool IsValidJson(string json)
        {
            try
            {
                JToken token = JObject.Parse(json);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private string DecodeJsonSave(string json){
            if(IsValidJson(json)){
                return json;
            }

            //We catch exceptions when decoding because  
            //if encoded file is changed manually "Convert.FromBase64String" can throw an error due to bad characters 
            //which doesn't matter to us.
            try{
                byte[] decodedBytes = Convert.FromBase64String(json);
                string decodedJson = Encoding.UTF8.GetString(decodedBytes);

                if (IsValidJson(decodedJson)){
                    return decodedJson;
                }
            }catch(Exception ex){

            }
            
            return null;

        }

    }




}
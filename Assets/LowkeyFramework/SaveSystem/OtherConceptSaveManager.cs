using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace LowkeyFramework.AttributeSaveSystem
{
    public class OtherConceptSaveManager : SingleBehaviour<OtherConceptSaveManager>
    {
        // TODO:
        // import Newtonsoft JSON - done, it is imported by default xD
        // is Newtonsoft using serialized fields only? Or is it ignoring Unity serialization? - it is ignoring serialization

        private List<SaveableBehaviour> GetSerializedBehaviours()
        {
            return Resources.FindObjectsOfTypeAll(typeof(SaveableBehaviour))
                .Select(behaviour => (SaveableBehaviour)behaviour)
                .ToList();
        }

        public void Save()
        {
            Dictionary<string, SaveableBehaviour> saveDictionary = new Dictionary<string, SaveableBehaviour>();
            List<SaveableBehaviour> behaviours = GetSerializedBehaviours();
            Debug.Log(behaviours.Count);
            behaviours.ForEach(behaviour =>
            {
                if (Attribute.IsDefined(behaviour.GetType(), typeof(SaveFieldAttribute)))
                {
                // save object GUID as ID and field as json to that GUID
                // probably something like Dictionary<GUID as string, object with Save attribute as JSON object> 
                // it might be a little bit harder, maybe saving field's name will be necessary, so e.g.: Dictionary<GUID, (field's name, field as JSON)>
                if (!saveDictionary.ContainsKey(behaviour.Guid))
                    {
                        saveDictionary.Add(behaviour.Guid, behaviour);
                    }
                }
            });

            string saveJson = JsonConvert.SerializeObject(saveDictionary);

            if (FileManager.WriteToFile("save.json", saveJson))
            {
                Debug.Log("Save successful: save.json");
            }
        }
    }
}
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEngine;

// Adds a nice editor to edit JSON files as well as a simple text editor incase
// the editor doesn't support the types you need. It works with strings, floats
// ints and bools at the moment.
// 
// * Requires the latest version of JSON.net compatible with Unity


//If you want to edit a JSON file in the "StreammingAssets" Folder change this to DefaultAsset.
//Hacky solution to a weird problem :/
[CustomEditor(typeof(TextAsset), true)]
public class JSONEditor : Editor
{
    private bool UnableToParse => UnableToParseJson();

    private bool isTextMode, wasTextMode;

    public string rawText;
    private JObject jsonObject;
    private JProperty propertyToRename;
    private string propertyRename;
    private Dictionary<string, bool> foldOuts = new Dictionary<string, bool>();

    private bool UnableToParseJson()
    {
        try
        {
            JToken token = JObject.Parse(rawText);
            return false;
        }
        catch(Exception)
        {
            return true;
        }
    }
    public void JsonInspectorGUI()
    {
        bool enabled = GUI.enabled;
        GUI.enabled = true;
        if(jsonObject == null && rawText != null)
        {
            jsonObject = JsonConvert.DeserializeObject<JObject>(rawText);
        }

        Rect subHeaderRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight * 2.5f);
        Rect helpBoxRect = new Rect(subHeaderRect.x, subHeaderRect.y, subHeaderRect.width - subHeaderRect.width / 6 - 5f, subHeaderRect.height);
        Rect rawTextModeButtonRect = new Rect(subHeaderRect.x + subHeaderRect.width / 6 * 5, subHeaderRect.y, subHeaderRect.width / 6, subHeaderRect.height);
        EditorGUI.HelpBox(helpBoxRect, "You edit raw text if the JSON editor isn't enough by clicking the button to the right", MessageType.Info);

        GUIStyle wrappedButton = new GUIStyle("Button");
        wrappedButton.wordWrap = true;
        EditorGUI.BeginChangeCheck();
        GUI.enabled = !UnableToParse;
        isTextMode = GUI.Toggle(rawTextModeButtonRect, isTextMode, "Edit Text", wrappedButton);
        if(EditorGUI.EndChangeCheck())
        {
            GUI.FocusControl("");
        }
        GUI.enabled = true;

        if(!isTextMode)
        {
            if(jsonObject != null)
            {
                Rect initialRect = new Rect(10, 5 + EditorGUIUtility.singleLineHeight * 20, EditorGUIUtility.currentViewWidth - 20, EditorGUIUtility.singleLineHeight);

                RecursiveDrawField(false, jsonObject);
            }
        }
        else
        {
            rawText = EditorGUILayout.TextArea(rawText);
            GUIStyle helpBoxRichText = new GUIStyle(EditorStyles.helpBox);
            Texture errorIcon = EditorGUIUtility.Load("icons/console.erroricon.png") as Texture2D;

            helpBoxRichText.richText = true;

            if(UnableToParse)
            {
                GUILayout.Label(new GUIContent("Unable to parse text into JSON. Make sure there are no mistakes! Are you missing a <b>{</b>, <b>{</b> or <b>,</b>?", errorIcon), helpBoxRichText);
            }
        }

        wasTextMode = isTextMode;
        GUI.enabled = enabled;
    }

    private void RecursiveDrawField(bool indent, JToken container)
    {
        if(indent)
        {
            EditorGUI.indentLevel++;
        }

        for(int i = 0; i < container.Count(); i++)
        {
            JToken token = container.Values<JToken>().ToArray()[i];

            if(token.Type == JTokenType.Property)
            {
                JProperty property = token.Value<JProperty>();

                string propertyName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(property.Name.ToLower()) + ":";

                JToken[] children = token.Values().ToArray();
                bool hasObjects = children.Any(t => t.Type == JTokenType.Object);

                bool hasProperties = children.Any(t => t.Type == JTokenType.Property);
                bool hasMoreChildren = children.Count() > 1 || hasObjects || hasProperties;

                if(hasMoreChildren)
                {
                    GUILayout.BeginVertical();

                    if(!foldOuts.ContainsKey(property.Path))
                    {
                        foldOuts.Add(property.Path, false);
                    }
                    foldOuts[property.Path] = EditorGUILayout.Foldout(foldOuts[property.Path], propertyName);

                    if(foldOuts[property.Path])
                    {
                        RecursiveDrawField(true, token);
                    }
                }
                else
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(propertyName);
                    RecursiveDrawField(true, token);
                }

                if(!hasMoreChildren)
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
                else
                {
                    GUILayout.EndVertical();
                }
            }
            else if(token.Type == JTokenType.Object)
            {
                RecursiveDrawField(false, token);
            }
            else
            {
                //JProperty parentProperty = token.Parent.Value<JProperty>();

                switch(token.Type)
                {

                    case JTokenType.String:
                    {
                        string value = token.Value<string>();
                        value = EditorGUILayout.TextField(value);
                        token.Replace(value);
                        break;
                    }
                    case JTokenType.Float:
                    {
                        float value = token.Value<float>();
                        value = EditorGUILayout.FloatField(value);
                        token.Replace(value);
                        break;
                    }
                    case JTokenType.Integer:
                    {
                        int value = token.Value<int>();
                        value = EditorGUILayout.IntField(value);
                        token.Replace(value);
                        break;
                    }
                    case JTokenType.Boolean:
                    {
                        bool value = token.Value<bool>();
                        value = EditorGUILayout.Toggle(value);
                        token.Replace(value);
                        break;
                    }
                    case JTokenType.Null:
                    {
                        float textFieldWidth = EditorStyles.helpBox.CalcSize(new GUIContent("Null")).x;
                        GUILayout.Label("Null", EditorStyles.helpBox);
                        break;
                    }
                    case JTokenType.Array:
                    {
                        IEnumerable<JToken> array = token.Value<IEnumerable<JToken>>();

                        if(array.Count() > 0)
                        {
                            EditorGUILayout.BeginVertical();
                            GUILayout.BeginHorizontal();
                            GUILayout.Space((EditorGUI.indentLevel - 1) * 20);
                            GUILayout.Label("[");
                            GUILayout.EndHorizontal();
                        }
                        else
                        {
                            GUILayout.Label("[]");
                        }

                        foreach(JToken ele in array)
                        {
                            GUILayout.Space(10);
                            RecursiveDrawField(true, ele);
                        }
                        if(array.Count() > 0)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Space((EditorGUI.indentLevel - 1) * 20);
                            GUILayout.Label("]");
                            GUILayout.EndHorizontal();
                            EditorGUILayout.EndVertical();
                        }

                        break;
                    }
                    default:
                    {
                        GUILayout.Label(string.Format("Type '{0}' is not supported. Use text editor instead", token.Type.ToString()), EditorStyles.helpBox);
                        break;
                    }
                }
            }
        }

        if(indent)
        {
            EditorGUI.indentLevel--;
        }
    }

    private void AddNewProperty<T>(JObject jObject)
    {
        string typeName = typeof(T).Name.ToLower();
        object value = default(T);

        switch(Type.GetTypeCode(typeof(T)))
        {
            case TypeCode.Boolean:
                break;
            case TypeCode.Int32:
                typeName = "integer";
                break;
            case TypeCode.Single:
                break;
            case TypeCode.String:
                value = "";
                break;
            default:
                if(typeof(T) == typeof(JObject))
                {
                    typeName = "empty object";
                }

                value = new JObject();
                break;
        }

        string name = GetUniqueName(jObject, string.Format("new {0}", typeName));
        JProperty property = new JProperty(name, value);
        jObject.Add(property);
    }

    private string GetUniqueName(JObject jObject, string orignalName)
    {
        string uniqueName = orignalName;
        int suffix = 0;
        while(jObject[uniqueName] != null && suffix < 100)
        {
            suffix++;
            if(suffix >= 100)
            {
                Debug.LogError("Stop calling all your fields the same thing! Isn't it confusing?");
            }
            uniqueName = string.Format("{0} {1}", orignalName, suffix.ToString());
        }
        return uniqueName;
    }
}
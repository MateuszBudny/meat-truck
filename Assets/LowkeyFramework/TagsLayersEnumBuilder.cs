#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.IO;
using System.Text;

public class TagsLayersEnumBuilder : EditorWindow
{
	[MenuItem("Tools/Rebuild Tags and Layers Enums")]
	static void RebuildTagsAndLayersEnums()
	{
		string localEnumsFolderPath = "TagsAndLayers/";
		string relativeEnumsPath = $"Assets/{localEnumsFolderPath}";
		string globalEnumsPath = $"{Application.dataPath}/{localEnumsFolderPath}";

		rebuildTagsFile(globalEnumsPath + "Tags.cs");
		rebuildLayersFile(globalEnumsPath + "Layers.cs");

		AssetDatabase.ImportAsset(relativeEnumsPath + "Tags.cs", ImportAssetOptions.ForceUpdate);
		AssetDatabase.ImportAsset(relativeEnumsPath + "Layers.cs", ImportAssetOptions.ForceUpdate);
	}

	static void rebuildTagsFile(string filePath)
	{
		StringBuilder sb = new StringBuilder();

		sb.Append("//This class is auto-generated, do not modify (TagsLayersEnumBuilder.cs)\n");
		sb.Append("public abstract class Tags {\n");

		var srcArr = UnityEditorInternal.InternalEditorUtility.tags;
		var tags = new String[srcArr.Length];
		Array.Copy(srcArr, tags, tags.Length);
		Array.Sort(tags, StringComparer.InvariantCultureIgnoreCase);

		for (int i = 0, n = tags.Length; i < n; ++i)
		{
			string tagName = tags[i];

			sb.Append("\tpublic const string " + tagName + " = \"" + tagName + "\";\n");
		}

		sb.Append("}\n");

#if !UNITY_WEBPLAYER
		SaveTextToFile(sb.ToString(), filePath);
		Debug.Log($"Your tags has been successfully rebuild into {filePath}!");
#endif
	}

	static void rebuildLayersFile(string filePath)
	{
		StringBuilder sb = new StringBuilder();

		sb.Append("//This class is auto-generated, do not modify (use Tools/TagsLayersEnumBuilder)\n");
		sb.Append("public abstract class Layers {\n");

		var layers = UnityEditorInternal.InternalEditorUtility.layers;

		for (int i = 0, n = layers.Length; i < n; ++i)
		{
			string layerName = layers[i];

			sb.Append("\tpublic const string " + GetVariableName(layerName) + " = \"" + layerName + "\";\n");
		}

		sb.Append("\n");

		for (int i = 0, n = layers.Length; i < n; ++i)
		{
			string layerName = layers[i];
			int layerNumber = LayerMask.NameToLayer(layerName);
			string layerMask = layerNumber == 0 ? "1" : ("1 << " + layerNumber);

			sb.Append("\tpublic const int " + GetVariableName(layerName) + "Mask" + " = " + layerMask + ";\n");
		}

		sb.Append("\n");

		for (int i = 0, n = layers.Length; i < n; ++i)
		{
			string layerName = layers[i];
			int layerNumber = LayerMask.NameToLayer(layerName);

			sb.Append("\tpublic const int " + GetVariableName(layerName) + "Number" + " = " + layerNumber + ";\n");
		}

		sb.Append("}\n");

#if !UNITY_WEBPLAYER
		SaveTextToFile(sb.ToString(), filePath);
		Debug.Log($"Your layers has been successfully rebuild into {filePath}!");
#endif
	}

	private static string GetVariableName(string str)
	{
		return str.Replace(" ", "");
	}

	private static void SaveTextToFile(string text, string filePath)
    {
		FileInfo file = new FileInfo(filePath);
		file.Directory.Create();
		using (StreamWriter writer = file.CreateText())
		{
			writer.Write(text);
		}
	} 
}
#endif
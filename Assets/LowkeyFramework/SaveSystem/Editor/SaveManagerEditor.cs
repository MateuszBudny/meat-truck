using System.Text;
using UnityEditor;
using UnityEngine;

namespace LowkeyFramework.AttributeSaveSystem
{
    [CustomEditor(typeof(SaveManager))]
    public class SaveManagerEditor : Editor
    {
        private SaveManager Target => (SaveManager)target;

        private string testSaveName = "test1234";
        private JSONEditor jsonEditor;

        private void OnEnable()
        {
            jsonEditor = CreateInstance<JSONEditor>();
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

            if(GUILayout.Button("Generate random key"))
            {
                Target.key = RandomString(16);
                EditorUtility.SetDirty(Target);
            }

            jsonEditor.JsonInspectorGUI();
        }

        private static string RandomString(int length)
        {
            const string pool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
            StringBuilder builder = new StringBuilder();
            System.Random random = new System.Random();

            for(int i = 0; i < length; i++)
            {
                char c = pool[random.Next(0, pool.Length)];
                builder.Append(c);
            }

            return builder.ToString();
        }
    }
}
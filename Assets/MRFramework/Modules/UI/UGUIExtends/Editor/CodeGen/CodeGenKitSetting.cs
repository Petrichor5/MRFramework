using System;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace MRFramework
{
    // 全局设置
    public class CodeGenKitSetting : ScriptableObject
    {
        public string ScriptNamespace = "UIPanel";
        public string ViewDir = "Assets/Scripts/UI/View";
        public string ControllerDir = "Assets/Scripts/UI";

        private static readonly Lazy<string> dir =
            new Lazy<string>(() => "Assets/MRFramework/Modules/UI/".CreateDirIfNotExists());

        private const string fileName = "CodeGenSetting.asset";

        private static CodeGenKitSetting instance;

        public static CodeGenKitSetting Load()
        {
            if (instance) return instance;

            var filePath = dir.Value + fileName;

            if (File.Exists(filePath))
            {
                return instance = AssetDatabase.LoadAssetAtPath<CodeGenKitSetting>(filePath);
            }

            return instance = CreateInstance<CodeGenKitSetting>();
        }

        public void Save()
        {
            var filePath = dir.Value + fileName;
            if (!File.Exists(filePath))
            {
                AssetDatabase.CreateAsset(this, dir.Value + fileName);
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}

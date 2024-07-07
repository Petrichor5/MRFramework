using UnityEngine;
using UnityEditor;

namespace MRFramework
{
    public class CodeGenKitSettingsWindow : EditorWindow
    {
        private CodeGenKitSetting settings;

        [MenuItem("MRFramework/CodeGen 全局设置")]
        private static void OpenWindow()
        {
            CodeGenKitSettingsWindow window = GetWindow<CodeGenKitSettingsWindow>("CodeGen 全局设置");
            window.Show();
        }

        private void OnEnable()
        {
            settings = CodeGenKitSetting.Load();
        }

        private void OnGUI()
        {
            if (settings == null)
            {
                EditorGUILayout.HelpBox("Settings not loaded!", MessageType.Error);
                return;
            }
            
            settings.ScriptNamespace = EditorGUILayout.TextField("命名空间", settings.ScriptNamespace);
            EditorGUILayout.LabelField("生成目录：");
            settings.ViewDir = EditorGUILayout.TextField("View", settings.ViewDir);
            settings.ControllerDir = EditorGUILayout.TextField("Controller", settings.ControllerDir);
            
            if (GUILayout.Button("保存"))
            {
                settings.Save();
                AssetDatabase.Refresh();
            }
        }

        private void OnDisable()
        {
            settings.Save();
            AssetDatabase.Refresh();
        }
    }
}
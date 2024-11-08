using System.IO;
using UnityEditor;
using UnityEngine;

namespace MRFramework
{
    public class UIWindowEditor : EditorWindow
    {
        private static CodeGenTask m_Task = CodeGenKit.Instance.Task;
        private static string m_PanelViewCode;
        private static string m_PanelCode;
        private static string m_PanelViewPath;
        private static string m_PanelPath;

        private static Vector2 m_Scroll = new Vector2();

        public static void ShowWindow()
        {
            m_PanelViewCode = m_Task.PanelViewCode;
            m_PanelCode = m_Task.PanelCode;
            m_PanelViewPath = m_Task.PanelViewFolder + $"\\{m_Task.ScriptName}.View.cs";
            m_PanelPath = m_Task.PanelFolder + $"\\{m_Task.ScriptName}.cs";
            
            UIWindowEditor window = (UIWindowEditor)GetWindowWithRect(typeof(UIWindowEditor),
                new Rect(100, 50, 800, 600), true, "面板脚本生成预览");
            
            window.Show();
        }

        public void OnGUI()
        {
            //绘制ScroView
            m_Scroll = EditorGUILayout.BeginScrollView(m_Scroll, GUILayout.Height(500), GUILayout.Width(800));
            EditorGUILayout.TextArea(m_PanelViewCode);
            EditorGUILayout.EndScrollView();
            EditorGUILayout.Space();

            //绘制脚本生成路径
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextArea("脚本生成路径：" + m_PanelViewPath);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            //绘制按钮
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("生成脚本", GUILayout.Height(30)))
            {
                //按钮事件
                ButtonClick();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void ButtonClick()
        {
            // View脚本每次都创建新的
            if (!File.Exists(m_PanelViewPath))
            {
                m_PanelViewPath.GetFolderPath().CreateDirIfNotExists();
            }
            File.WriteAllText(m_PanelViewPath, m_PanelViewCode);

            // 面板逻辑脚本只会创建一次
            if (!File.Exists(m_PanelPath))
            {
                m_PanelPath.GetFolderPath().CreateDirIfNotExists();
                File.WriteAllText(m_PanelPath, m_PanelCode);
            }
            
            EditorPrefs.SetString("GeneratorClassName", m_Task.ScriptName);
            AssetDatabase.Refresh();
            
            if (EditorUtility.DisplayDialog("自动化生成工具", "生成脚本成功！", "确定"))
            {
                Close();
            }
        }
    }
}

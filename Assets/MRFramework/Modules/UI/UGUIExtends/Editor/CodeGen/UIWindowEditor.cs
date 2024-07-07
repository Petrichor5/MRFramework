using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace MRFramework
{
    public class UIWindowEditor : EditorWindow
    {
        private static CodeGenTask task = CodeGenKit.Instance.Task;
        private static Dictionary<string, string> mInsterDic;
        private static string mPanelViewCode;
        private static string mPanelCode;
        private static string mPanelViewPath;
        private static string mPanelPath;

        private static Vector2 mScroll = new Vector2();

        public static void ShowWindow()
        {
            mInsterDic = task.MethodDic;
            mPanelViewCode = task.PanelViewCode;
            mPanelCode = task.PanelCode;
            mPanelViewPath = task.PanelViewFolder + $"/{task.ScriptName}.View.cs";
            mPanelPath = task.PanelFolder + $"/{task.ScriptName}.cs";
            
            UIWindowEditor window = (UIWindowEditor)GetWindowWithRect(typeof(UIWindowEditor),
                new Rect(100, 50, 800, 700), true, "Window生成界面");

            bool hasChange = false;
            if (File.Exists(mPanelPath) && mInsterDic != null)
            { 
                string originScript = File.ReadAllText(mPanelPath);
                foreach (var item in mInsterDic)
                {
                    // 如果老代码中没有这个代码就进行插入操作
                    if (!originScript.Contains(item.Key))
                    {
                        int index = window.GetInserIndex(originScript);
                        mPanelCode = originScript.Insert(index, item.Value + "\n\r\t\t");
                        originScript = mPanelCode;
                        hasChange = true;
                    }
                }
            }

            if (!hasChange)
            {
                if (File.Exists(mPanelPath))
                {
                    mPanelCode = File.ReadAllText(mPanelPath);
                }
            }
            
            window.Show();
        }

        public void OnGUI()
        {
            //绘制ScroView
            mScroll = EditorGUILayout.BeginScrollView(mScroll, GUILayout.Height(600), GUILayout.Width(800));
            EditorGUILayout.TextArea(mPanelCode);
            EditorGUILayout.EndScrollView();
            EditorGUILayout.Space();

            //绘制脚本生成路径
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextArea("脚本生成路径：" + mPanelPath);
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
            if (!File.Exists(mPanelViewPath))
            {
                mPanelViewPath.GetFolderPath().CreateDirIfNotExists();
            }
            File.WriteAllText(mPanelViewPath, mPanelViewCode);
            
            if (!File.Exists(mPanelPath))
            {
                mPanelPath.GetFolderPath().CreateDirIfNotExists();
            }
            File.WriteAllText(mPanelPath, mPanelCode);
            
            EditorPrefs.SetString("GeneratorClassName", task.ScriptName + ".View");
            AssetDatabase.Refresh();
            
            if (EditorUtility.DisplayDialog("自动化生成工具", "生成脚本成功！", "确定"))
            {
                Close();
            }
        }

        /// <summary>
        /// 获取代码插入下标
        /// </summary>
        public int GetInserIndex(string content)
        {
            //找到UI事件组件下面的第一个public 所在的位置 进行插入
            Regex regex = new Regex("UI组件事件");
            Match match = regex.Match(content);

            Regex regex1 = new Regex("public");
            MatchCollection matchCollection = regex1.Matches(content);

            for (int i = 0; i < matchCollection.Count; i++)
            {
                if (matchCollection[i].Index > match.Index)
                {
                    return matchCollection[i].Index;
                }
            }

            return -1;
        }
    }
}

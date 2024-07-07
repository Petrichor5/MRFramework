using System;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Event = UnityEngine.Event;

namespace MRFramework
{
    public class CodeGenInspectorStyle
    {
        // 标题样式
        public readonly Lazy<GUIStyle> BigTitleStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.label)
        {
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            fontSize = 15
        });

        // 输入框样式
        public readonly Lazy<GUIStyle> TextAreaStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.textArea)
        {
            wordWrap = true
        });
    }

    [CustomEditor(typeof(CodeGen))]
    public class CodeGenInspector : Editor
    {
        private class LocaleText
        {
            public static string Tital = "代码生成设置";
            public static string ScriptNamespace = "命名空间";
            public static string ScriptName = "代码名字";
            public static string ScriptsFolder = "生成目录：";
            public static string PanelViewFolder = "PanelView";
            public static string PanelFolder = "Panel";
            public static string Generate = "生成代码";
        }

        public CodeGen CodeGenTarget => target as CodeGen;
        private string[] componentNames;
        private CodeGenInspectorStyle style = new CodeGenInspectorStyle();
        private ReorderableList list;
        
        // 初始化
        private void OnEnable()
        {
            if (string.IsNullOrEmpty(CodeGenTarget.PanelViewFolder))
            {
                var setting = CodeGenKitSetting.Load();
                CodeGenTarget.PanelViewFolder = setting.ViewDir;
            }

            if (string.IsNullOrEmpty(CodeGenTarget.PanelFolder))
            {
                var setting = CodeGenKitSetting.Load();
                CodeGenTarget.PanelFolder = setting.ControllerDir;
            }

            if (string.IsNullOrEmpty(CodeGenTarget.ScriptName))
            {
                CodeGenTarget.ScriptName = CodeGenTarget.gameObject.name;
            }

            if (string.IsNullOrEmpty(CodeGenTarget.ScriptNamespace))
            {
                var setting = CodeGenKitSetting.Load();
                CodeGenTarget.ScriptNamespace = setting.ScriptNamespace;
            }

            list = new ReorderableList(serializedObject, serializedObject.FindProperty("Binds"), true, true, true, true);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            SerializedProperty dataProperty = serializedObject.FindProperty("Binds");
            SerializedProperty property;

            // 标题
            GUILayout.Space(5);
            GUILayout.Label(LocaleText.Tital, style.BigTitleStyle.Value);
            GUILayout.Space(5);

            #region 设置相关
            // 命名空间
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(LocaleText.ScriptNamespace, GUILayout.Width(80));
                CodeGenTarget.ScriptNamespace =
                    EditorGUILayout.TextArea(CodeGenTarget.ScriptNamespace, style.TextAreaStyle.Value);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(2);

            // 代码名字
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(LocaleText.ScriptName, GUILayout.Width(80));
                CodeGenTarget.ScriptName =
                    EditorGUILayout.TextArea(CodeGenTarget.ScriptName, style.TextAreaStyle.Value);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(2);

            // 生成目录
            GUILayout.Label(LocaleText.ScriptsFolder);

            // PanelView 生成目录
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(LocaleText.PanelViewFolder, GUILayout.Width(80));
                CodeGenTarget.PanelViewFolder =
                    EditorGUILayout.TextArea(CodeGenTarget.PanelViewFolder, style.TextAreaStyle.Value);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(2);

            // Panel 生成目录
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(LocaleText.PanelFolder, GUILayout.Width(80));
                CodeGenTarget.PanelFolder =
                    EditorGUILayout.TextArea(CodeGenTarget.PanelFolder, style.TextAreaStyle.Value);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            #endregion

            #region 信息列表
            list.DoLayoutList();
            
            list.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "UI 控件管理");
            };

            list.drawElementCallback = (Rect rect, int i, bool isActive, bool isFocused) =>
            {
                rect.y += 2;

                // 对象
                property = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative("Obj");
                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y, 140, EditorGUIUtility.singleLineHeight),
                    property, GUIContent.none);

                // 类型
                if (property.objectReferenceValue is GameObject)
                {
                    // 获取对象上的所有组件
                    property = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative("Obj");
                    Component[] components = (property.objectReferenceValue as GameObject)?.GetComponents<Component>();
                    if (components != null)
                        componentNames = components
                            .Select(c => c.GetType().FullName)
                            .ToArray();

                    // 获取对象组件类型对应的 index
                    property = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative("TypeName");
                    int index = componentNames
                        .ToList()
                        .FindIndex((componentName) => componentName.Contains(property.stringValue));

                    var newIndex = EditorGUI.Popup(
                        new Rect(rect.x + 150, rect.y, 180, EditorGUIUtility.singleLineHeight),
                        index, componentNames);

                    if (index != newIndex)
                    {
                        property.stringValue = componentNames[newIndex];
                    }
                }
            };
            #endregion

            #region 拖拽区域
            GUILayout.Label("将需要生成变量的 Object 拖拽至此：");
            var sfxPathRect = EditorGUILayout.GetControlRect();
            sfxPathRect.height = 50;
            GUI.Box(sfxPathRect, string.Empty);
            EditorGUILayout.LabelField(string.Empty, GUILayout.Height(35));

            //判断检查当前事件是否是拖拽事件（更新或执行），并且鼠标位置是否在sfxPathRect定义的区域内。
            if ((Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform)
                && sfxPathRect.Contains(Event.current.mousePosition))
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (Event.current.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    foreach (var obj in DragAndDrop.objectReferences)
                    {
                        // 添加引用对象
                        CodeGenTarget.AddItem(obj);
                    }
                }

                Event.current.Use();
            }
            #endregion

            // 生成代码按钮
            if (GUILayout.Button(LocaleText.Generate, GUILayout.Height(30)))
            {
                CodeGenTask task = new CodeGenTask()
                {
                    Obj = CodeGenTarget.gameObject,
                    Binds = CodeGenTarget.Binds,
                    ScriptName = CodeGenTarget.ScriptName,
                    ScriptNamespace = CodeGenTarget.ScriptNamespace,
                    PanelViewFolder = CodeGenTarget.PanelViewFolder,
                    PanelFolder = CodeGenTarget.PanelFolder
                };
                CodeGenKit.Instance.Generate(task);
            }

            serializedObject.ApplyModifiedProperties();
            serializedObject.UpdateIfRequiredOrScript();
        }
    }
}
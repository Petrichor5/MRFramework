using System.IO;
using System.Text;

namespace MRFramework
{
    /// <summary>
    /// 代码脚本模板
    /// </summary>
    public static class CodeTemplate
    {
        private static CodeGenTask m_Task;
        
        /// <summary>
        /// 获取脚本模板
        /// </summary>
        /// <param name="task"></param>
        public static void GetTemplate(ref CodeGenTask task)
        {
            m_Task = task;
            CreatPanelScript(ref task);
            CreatPanelViewScript(ref task);
        }

        // 生成面板脚本
        private static void CreatPanelScript(ref CodeGenTask task)
        {
            // 面板脚本只会生成一次
            string panelPath = task.PanelFolder + $"/{task.ScriptName}.cs";
            if (File.Exists(panelPath)) return;

            StringBuilder writer = new StringBuilder();

            writer.AppendLine("using System.Collections;");
            writer.AppendLine("using System.Collections.Generic;");
            writer.AppendLine("using UnityEngine;");
            writer.AppendLine("using UnityEngine.UI;");
            writer.AppendLine("using MRFramework;");
            writer.AppendLine();

            string panleType = task.ScriptName.EndsWith("SV") ? "SubPanel" : "MainPanel";
            writer.AppendLine($"public partial class {task.ScriptName} : {panleType}");
            writer.AppendLine("{");
            
            // OnFirstOpen
            writer.AppendLine("\tpublic override void OnFirstOpen()");
            writer.AppendLine("\t{");
            writer.AppendLine("\t\tbase.OnFirstOpen();");
            writer.AppendLine("\t\tInitWidgets();");
            writer.AppendLine("\t\tAddEventListeners();");
            writer.AppendLine("\t}");
            writer.AppendLine();
            
            writer.AppendLine("\tpublic void InitWidgets()");
            writer.AppendLine("\t{");
            // 子面板
            CreateSubView(writer);
            // UI组件事件绑定
            InitScrollWidget(writer);
            InitButton(writer);
            InitInputField(writer);
            InitInputToggle(writer);
            writer.AppendLine("\t}");
            writer.AppendLine();

            writer.AppendLine("\tpublic void AddEventListeners()");
            writer.AppendLine("\t{");
            writer.AppendLine("\t}");
            writer.AppendLine();

            // OnOpen
            writer.AppendLine("\tpublic override void OnOpen()");
            writer.AppendLine("\t{");
            writer.AppendLine("\t\tbase.OnOpen();");
            writer.AppendLine("\t}");
            writer.AppendLine();

            // OnClose
            writer.AppendLine("\tpublic override void OnClose()");
            writer.AppendLine("\t{");
            writer.AppendLine("\t\tbase.OnClose();");
            writer.AppendLine("\t}");
            writer.AppendLine();

            // OnDispose
            writer.AppendLine("\tpublic override void OnDispose()");
            writer.AppendLine("\t{");
            writer.AppendLine("\t\tbase.OnDispose();");
            writer.AppendLine("\t}");
            writer.AppendLine();

            // UI组件事件
            writer.AppendLine($"\t#region UI组件事件");
            writer.AppendLine();
            for (int i = 0; i < task.Binds.Count; i++)
            {
                var item = task.Binds[i];
                string type = item.TypeName;
                string memberName = item.MemberName;
                string suffix;
                if (type.Contains("Button"))
                {
                    suffix = "Click";
                    string methodName = "On" + memberName.Replace("Button_", "");
                    CreateMethod(writer, methodName + suffix);
                    if (i < task.Binds.Count - 1) writer.AppendLine();
                }
                else if (type.Contains("InputField"))
                {
                    suffix = "InputChange";
                    string methodName = "On" + memberName.Replace("Input_", "");
                    CreateMethod(writer, methodName + suffix, "string text");
                    writer.AppendLine();

                    suffix = "InputEnd";
                    CreateMethod(writer, methodName + suffix, "string text");
                    if (i < task.Binds.Count - 1) writer.AppendLine();
                }
                else if (type.Contains("Toggle"))
                {
                    suffix = "ToggleChange";
                    string methodName = "On" + memberName.Replace("Toggle_", "");
                    CreateMethod(writer, methodName + suffix, "bool state, Toggle toggle");
                    if (i < task.Binds.Count - 1) writer.AppendLine();
                }
            }
            writer.AppendLine();
            writer.AppendLine($"\t#endregion");
            
            writer.AppendLine("}");

            task.PanelCode = writer.ToString();
            writer.Clear();
        }

        // 生成面板View脚本
        private static void CreatPanelViewScript(ref CodeGenTask task)
        {
            StringBuilder writer = new StringBuilder();
            
            writer.AppendLine("using MRFramework;");
            writer.AppendLine("using UnityEngine;");
            writer.AppendLine("using UnityEngine.Rendering;");
            writer.AppendLine("using MRFramework.UGUIPro;");
            writer.AppendLine();
            writer.AppendLine($"public partial class {task.ScriptName}");
            writer.AppendLine("{");

            // 声明字段
            foreach (var bindData in task.Binds)
            {
                string memberName = bindData.MemberName;
                // 隐藏字段
                // writer.AppendLine($"\t[HideInInspector] public {bindData.TypeName} {memberName};");
                
                // 不隐藏字段
                writer.AppendLine($"\tpublic {bindData.TypeName} {memberName};");
            }
            writer.AppendLine();
            
            writer.AppendLine("}");

            task.PanelViewCode = writer.ToString();
            writer.Clear();
        }
        
        private static void CreateMethod(StringBuilder writer, string methodName, string param = "")
        {
            // 声明UI组件事件
            writer.AppendLine($"\tpublic void {methodName}({param})");
            writer.AppendLine("\t{");
            if (methodName == "OnCloseButtonClick")
            {
                // 如果是子面板
                if (m_Task.ScriptName.EndsWith("SV"))
                    writer.AppendLine($"\t\tClose();");
                else
                    writer.AppendLine($"\t\tUIManager.Instance.ClosePanel<{m_Task.ScriptName}>();");
            }
            writer.AppendLine("\t}");
        }

        private static void CreateSubView(StringBuilder writer)
        {
            bool hasSubView = false;
            foreach (var bindData in m_Task.Binds)
            {
                string type = bindData.TypeName;
                string memberName = bindData.MemberName;
                if (type.StartsWith("WBP_") && type.EndsWith("SV"))
                {
                    hasSubView = true;
                    writer.AppendLine($"\t\tAddSubPanel<{memberName}>({memberName});");
                }
            }
            if (hasSubView) writer.AppendLine();
        }

        private static void InitScrollWidget(StringBuilder writer)
        {
            bool hasWidget = false;
            foreach (var bindData in m_Task.Binds)
            {
                string type = bindData.TypeName;
                if (type.Contains("List"))
                {
                    string memberName = bindData.MemberName;
                    writer.AppendLine($"\t\tAddScrollWidget({memberName});");
                    hasWidget = true;
                }
            }
            if (hasWidget) writer.AppendLine();
        }
        
        private static void InitButton(StringBuilder writer)
        {
            bool hasWidget = false;
            foreach (var bindData in m_Task.Binds)
            {
                string type = bindData.TypeName;
                if (type.Contains("Button"))
                {
                    string suffix = "Click";
                    string memberName = bindData.MemberName;
                    string methodName = memberName.Replace("Button_", "");
                    writer.AppendLine($"\t\tAddButtonClickListener({memberName}, On{methodName}{suffix});");
                    hasWidget = true;
                }
            }
            if (hasWidget) writer.AppendLine();
        }

        private static void InitInputField(StringBuilder writer)
        {
            bool hasWidget = false;
            foreach (var bindData in m_Task.Binds)
            {
                string type = bindData.TypeName;
                if (type.Contains("InputField"))
                {
                    string memberName = bindData.MemberName;
                    string methodName = memberName.Replace("Input_", "");
                    writer.AppendLine($"\t\tAddInputListener({memberName}, On{methodName}InputChange, On{methodName}InputEnd);");
                    hasWidget = true;
                }
            }
            if (hasWidget) writer.AppendLine();
        }
        
        private static void InitInputToggle(StringBuilder writer)
        {
            bool hasWidget = false;
            foreach (var bindData in m_Task.Binds)
            {
                string type = bindData.TypeName;
                if (type.Contains("Toggle"))
                {
                    string suffix = "Change";
                    string memberName = bindData.MemberName;
                    string methodName = memberName.Replace("Toggle_", "");
                    writer.AppendLine($"\t\tAddToggleClickListener({memberName}, On{methodName}Toggle{suffix});");
                    hasWidget = true;
                }
            }
            if (hasWidget) writer.AppendLine();
        }
    }
}

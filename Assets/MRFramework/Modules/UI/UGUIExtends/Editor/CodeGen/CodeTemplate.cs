using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MRFramework
{
    /// <summary>
    /// 代码脚本模板
    /// </summary>
    public static class CodeTemplate
    {
        private static CodeGenTask mTask;
        
        /// <summary>
        /// 获取脚本模板
        /// </summary>
        /// <param name="task"></param>
        public static void GetTemplate(ref CodeGenTask task)
        {
            mTask = task;
            task.MethodDic.Clear();
            CreatPanelScript(ref task);
            CreatPanelViewScript(ref task);
        }

        // 生成面板脚本
        private static void CreatPanelScript(ref CodeGenTask task)
        {
            StringBuilder writer = new StringBuilder();

            string super = "MainPanel";
            if (task.ScriptName.EndsWith("SV"))
            {
                super = "SubPanel";
            }
                        
            writer.AppendLine("using System.Collections;");
            writer.AppendLine("using System.Collections.Generic;");
            writer.AppendLine("using UnityEngine;");
            writer.AppendLine("using MRFramework;");
            writer.AppendLine();

            writer.AppendLine($"public partial class {task.ScriptName} : {super}");
            writer.AppendLine("{");
            
            // OnAwake
            writer.AppendLine("\tpublic override void OnAwake()");
            writer.AppendLine("\t{");
            writer.AppendLine();
            writer.AppendLine("\t}");
            writer.AppendLine();

            // OnOpen
            writer.AppendLine("\tpublic override void OnOpen()");
            writer.AppendLine("\t{");
            writer.AppendLine();
            writer.AppendLine("\t}");
            writer.AppendLine();

            // OnClose
            writer.AppendLine("\tpublic override void OnClose()");
            writer.AppendLine("\t{");
            writer.AppendLine();
            writer.AppendLine("\t}");
            writer.AppendLine();

            // OnDispose
            writer.AppendLine("\tpublic override void OnDispose()");
            writer.AppendLine("\t{");
            writer.AppendLine();
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
                string methodName = "On" + memberName;
                string suffix;
                if (type.Contains("Button"))
                {
                    suffix = "Click";
                    CreateMethod(writer, ref task.MethodDic, methodName + suffix);
                    if (i < task.Binds.Count - 1) writer.AppendLine();
                }
                else if (type.Contains("InputField"))
                {
                    suffix = "InputChange";
                    CreateMethod(writer, ref task.MethodDic, methodName + suffix, "string text");
                    writer.AppendLine();

                    suffix = "InputEnd";
                    CreateMethod(writer, ref task.MethodDic, methodName + suffix, "string text");
                    if (i < task.Binds.Count - 1) writer.AppendLine();
                }
                else if (type.Contains("Toggle"))
                {
                    suffix = "ToggleChange";
                    CreateMethod(writer, ref task.MethodDic, methodName + suffix, "bool state,Toggle toggle");
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
            
            bool isSubView = false;
            if (task.ScriptName.EndsWith("SV"))
            {
                isSubView = true;
            }
            
            writer.AppendLine("using UnityEngine;");
            writer.AppendLine("using UnityEngine.Rendering;");
            writer.AppendLine();
            writer.AppendLine($"public partial class {task.ScriptName}");
            writer.AppendLine("{");

            // 声明字段
            foreach (var bindData in task.Binds)
            {
                string memberName = bindData.MemberName;
                writer.AppendLine($"\t[HideInInspector] public {bindData.TypeName} {memberName};");
            }
            
            writer.AppendLine();
            
            // 声明初始化组件接口
            writer.AppendLine("\tprivate void Awake()");
            writer.AppendLine("\t{");
            if (isSubView)
            {
                writer.AppendLine("\t\tCanvasGroup = transform.Find(\"UIContent\").GetComponent<CanvasGroup>();");
            }
            else
            {
                writer.AppendLine("\t\tUIContent = transform.Find(\"UIContent\");");
                writer.AppendLine("\t\tUIMask = transform.Find(\"UIMask\").GetComponent<CanvasGroup>();");
                writer.AppendLine("\t\tCanvasGroup = UIContent.GetComponent<CanvasGroup>();");
                writer.AppendLine("\t\tSortingGroup = transform.GetComponent<SortingGroup>();");
            }
            writer.AppendLine();
            writer.AppendLine("\t\t// 组件事件绑定");
            foreach (var bindData in task.Binds)
            {
                string type = bindData.TypeName;
                string memberName = bindData.MemberName;
                string suffix = "";
                if (type.Contains("Button"))
                {
                    suffix = "Click";
                    writer.AppendLine($"\t\tAddButtonClickListener({memberName}, On{memberName}{suffix});");
                }
                if (type.Contains("InputField"))
                {
                    writer.AppendLine($"\t\tAddInputListener({memberName}, On{memberName}InputChange, On{memberName}InputEnd);");
                }
                if (type.Contains("Toggle"))
                {
                    suffix = "Change";
                    writer.AppendLine($"\t\tAddToggleClickListener({memberName}, On{memberName}Toggle{suffix});");
                }
            }
            writer.AppendLine("\t}");
            
            writer.AppendLine("}");

            task.PanelViewCode = writer.ToString();
            writer.Clear();
        }
        
        private static void CreateMethod(StringBuilder writer, ref Dictionary<string, string> methodDic, string methodName, string param = "")
        {
            // 声明UI组件事件
            writer.AppendLine($"\tpublic void {methodName}({param})");
            writer.AppendLine("\t{");
            if (methodName == "OnCloseButtonClick")
            {
                // 如果是子面板
                if (mTask.ScriptName.EndsWith("SV"))
                    writer.AppendLine($"\t\tClose();");
                else
                    writer.AppendLine($"\t\tGameModule.UIMgr.ClosePanel<{mTask.ScriptName}>();");
            }
            writer.AppendLine("\t}");

            // 存储UI组件事件 提供给后续新增代码使用
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"public void {methodName}({param})");
            builder.AppendLine("\t{");
            builder.AppendLine("\t");
            builder.AppendLine("\t}");

            methodDic.Add(methodName, builder.ToString());
        }
    }
}

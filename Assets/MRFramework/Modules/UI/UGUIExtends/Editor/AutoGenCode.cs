using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace MRFramework
{
    public class AutoGenCode
    {
        private static List<Bind> mBindList;

        public static void CodeGen()
        {
            // 获取到当前选择的对象
            GameObject selectedObject = Selection.objects.First() as GameObject;

            // 获取UI控件
            mBindList = new List<Bind>();
            TraverseGameObject(selectedObject);

            // 获取脚本名称
            string scriptName = selectedObject.name;

            // 获取UI脚本生成路径
            string panleFolderPath = GetPanelFolder(scriptName);
            panleFolderPath.CreateDirIfNotExists();

            // 获取View存储路径
            string viewFolderPath = System.IO.Path.Combine(panleFolderPath, "UIContent");
            viewFolderPath.CreateDirIfNotExists();

            CodeGenTask task = new CodeGenTask()
            {
                Obj = selectedObject,
                Binds = mBindList,
                ScriptName = scriptName,
                ScriptNamespace = "",
                PanelViewFolder = viewFolderPath,
                PanelFolder = panleFolderPath
            };
            CodeGenKit.Instance.Generate(task);
        }

        private static void TraverseGameObject(GameObject selectedObject)
        {
            // 递归遍历所有子对象
            for (int i = 0; i < selectedObject.transform.childCount; i++)
            {
                GameObject obj = selectedObject.transform.GetChild(i).gameObject;
                string objName = obj.name;
                string typeName = UIDefine.GetUGUIType(objName);

                if (!string.IsNullOrEmpty(typeName))
                {
                    Bind bind = new Bind()
                    {
                        InstanceID = obj.GetInstanceID(),
                        MemberName = RemovePrefix(obj.name),
                        TypeName = typeName
                    };
                    mBindList.Add(bind);
                }

                // 如果是子面板 就跳过获取子面板的控件，只需获取子面板
                if (typeName == objName) continue;

                TraverseGameObject(obj);
            }
        }

        private static string GetPanelFolder(string panelName) 
        {
            string path;

            // 检查面板名称是否以 "WBP" 开头
            if (!panelName.StartsWith("WBP"))
            {
                Debug.LogError("[AutoGenCode] => 主面板名称错误：" + panelName);
                return null;
            }

            // 构建路径
            string[] strs = StringUtil.SplitStr(panelName, 7);
            path = "Assets\\Scripts\\UI\\";
            for (int i = 1; i < strs.Length - 1; i++)
            {
                path = System.IO.Path.Combine(path, strs[i]);
            }

            return path;
        }

        private static string RemovePrefix(string str)
        {
            // 如果是子面板
            if (str.EndsWith("SV"))
            {
                return str;
            }

            int underscoreIndex = str.IndexOf('_');
            if (underscoreIndex >= 0)
            {
                return str.Substring(underscoreIndex + 1);
            }

            // 如果字符串中没有 "_"，则返回原字符串
            return str;
        }
    }
}
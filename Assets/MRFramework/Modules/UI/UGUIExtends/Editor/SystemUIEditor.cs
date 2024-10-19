using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace MRFramework
{
    public class SystemUIEditor : Editor
    {
        #region RaycastTarget 射线检测优化

        [InitializeOnLoadMethod]
        private static void InitEditor()
        {
            // 监听 Hierarchy 窗口发生改变的委托
            EditorApplication.hierarchyChanged += HanderRaycast;
        }

        // 取消勾选 RaycastTarget
        private static void HanderRaycast()
        {
            GameObject obj = Selection.activeGameObject;
            if (obj != null)
            {
                if (obj.name.Contains("Text"))
                {
                    Text text = obj.GetComponent<Text>();
                    if (text != null)
                    {
                        text.raycastTarget = false;
                    }
                }
                else if (obj.name.Contains("Image"))
                {
                    Image image = obj.GetComponent<Image>();
                    if (image != null)
                    {
                        image.raycastTarget = false;
                    }
                    else
                    {
                        RawImage rawImage = obj.GetComponent<RawImage>();
                        if (rawImage != null)
                        {
                            rawImage.raycastTarget = false;
                        }
                    }
                }
            }
        }

        #endregion

        [MenuItem("GameObject/UI/UIPanel/自动生成UI脚本(Shift+Z) #Z", false, 0)]
        private static void CreateAutoGenCodeScript()
        {
            AutoGenCode.CodeGen();
        }

        [MenuItem("GameObject/UI/UIPanel/一键优化合批(Shift+X) #X", false, 1)]
        [Obsolete("Obsolete")]
        public static void OptimizationUIBatch()
        {
            UILayoutTool.OptimizeBatchForMenu();
        }

        [MenuItem("GameObject/UI/UIPanel/创建 UIRoot 拼接窗口(Shift+C) #C", false, 2)]
        public static void BuildUIRoot()
        {
            GameObject uiRoot = GameObject.Instantiate(Resources.Load<GameObject>("UIRoot"));
            uiRoot.name = "UIRoot";
        }
        
        [MenuItem("GameObject/UI/UIPanel/创建子面板(Shift+0) #0", false, 3)]
        public static void BuildSubPanel()
        {
            InitBuildPanel("TempSubPanel");
        }
        
        [MenuItem("GameObject/UI/UIPanel/创建一级窗口(Shift+1) #1", false, 4)]
        public static void BuildPanel1()
        {
            InitBuildPanel("TempMainPanel");
        }
        
        [MenuItem("GameObject/UI/UIPanel/创建二级窗口(Shift+2) #2", false, 5)]
        public static void BuildPanel2()
        {
            InitBuildPanel("TempMainPanel1");
        }
        
        [MenuItem("GameObject/UI/UIPanel/创建三级窗口(Shift+3) #3", false, 6)]
        public static void BuildPanel3()
        {
            InitBuildPanel("TempMainPanel2");
        }
        
        [MenuItem("GameObject/UI/UIPanel/创建四级窗口(Shift+4) #4", false, 7)]
        public static void BuildPanel4()
        {
            InitBuildPanel("TempMainPanel3");
        }
        
        [MenuItem("GameObject/UI/UIPanel/创建五级窗口(Shift+5) #5", false, 8)]
        public static void BuildPanel5()
        {
            InitBuildPanel("TempMainPanel4");
        }

        private static void InitBuildPanel(string panelName)
        {
            GameObject uiRoot = GameObject.Find("UIRoot");
            if (uiRoot == null)
            {
                uiRoot = Instantiate(Resources.Load<GameObject>("UIRoot"));
                uiRoot.name = "UIRoot";
            }
            
            GameObject go = Instantiate(Resources.Load<GameObject>(panelName), uiRoot.transform, false);
            go.name = panelName;
        }
    }
}
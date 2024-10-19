namespace MRFramework
{
    public static class UITool
    {
        /// <summary>
        /// 获取控件类型，用于代码自动生成
        /// </summary>
        public static string GetUGUIType(string objName)
        {
            string result = "";

            // 子面板
            if (objName.StartsWith("WBP_"))
            {
                result = objName;
            }

            // 过滤，忽略获取这个控件以及它的所有子控件
            else if (objName.StartsWith("Filter_"))
            {
                result = "Filter";
            }

            // 按钮
            else if (objName.StartsWith("Button_") || objName.Equals("CloseButton"))
            {
                result = "ButtonPro";
            }

            // 文本
            else if (objName.StartsWith("Text_"))
            {
                result = "TextMeshPro";
            }

            // 列表
            else if (objName.StartsWith("List_"))
            {
                result = "ScrollViewPro";
            }

            // 图片
            else if (objName.StartsWith("Image_"))
            {
                result = "ImagePro";
            }

            // 勾选框
            else if (objName.StartsWith("Toggle_"))
            {
                result = "UnityEngine.UI.Toggle";
            }

            // 输入框
            else if (objName.StartsWith("Input_"))
            {
                result = "UnityEngine.UI.InputField";
            }

            // 滑动条
            else if (objName.StartsWith("Slider_"))
            {
                result = "SliderPro";
            }

            // 组
            else if (objName.StartsWith("Group_"))
            {
                result = "UnityEngine.GameObject";
            }

            return result;
        }
    }
}
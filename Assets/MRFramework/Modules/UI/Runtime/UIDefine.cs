
namespace MRFramework
{
    public class UIDefine
    {
        /// <summary>
        /// 关闭的UI主面板生命周期，计时结束后被销毁
        /// </summary>
        public static int PanelDiposeTime = 10;

        /// <summary>
        /// 获取控件类型，用于代码自动生成
        /// </summary>
        public static string GetUGUIType(string objName)
        {
            string result = "";

            // 按钮
            if (objName.StartsWith("Button_") || objName.Equals("CloseButton"))
            {
                result = "UnityEngine.UI.Button";
            }

            // 文本
            else if (objName.StartsWith("Text_"))
            {
                result = "UnityEngine.UI.Text";
            }
            else if (objName.StartsWith("TMP_"))
            {
                result = "TMPro.TextMeshProUGUI";
            }

            // 图片
            else if (objName.StartsWith("Image_"))
            {
                result = "UnityEngine.UI.Image";
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
                result = "UnityEngine.UI.Slider";
            }

            // 子面板
            else if (objName.EndsWith("SV"))
            {
                result = objName;
            }

            return result;
        }
    }
}
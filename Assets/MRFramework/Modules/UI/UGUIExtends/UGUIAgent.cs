using UnityEngine;
using UnityEngine.UI;

namespace MRFramework
{
    /// <summary>
    /// 扩展 UGUI 原生的功能，减少性能消耗
    /// </summary>
    public static class UGUIAgent
    {
        public static void SetVisible(this GameObject obj, bool visible)
        {
            obj.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }
        
        public static void SetVisible(this Transform trans, bool visible)
        {
            trans.localScale = visible ? Vector3.one : Vector3.zero;
        }
        
        public static void SetVisible(this Button btn, bool visible)
        {
            btn.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }
        
        public static void SetVisible(this Text text, bool visible)
        {
            text.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }
        
        public static void SetVisible(this Slider slider, bool visible)
        {
            slider.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }
        
        public static void SetVisible(this Toggle toggle, bool visible)
        {
            toggle.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }
        
        public static void SetVisible(this InputField inputField, bool visible)
        {
            inputField.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }
        
        public static void SetVisible(this RawImage rawImage, bool visible)
        {
            rawImage.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }
        
        public static void SetVisible(this ScrollRect scrollRect, bool visible)
        {
            scrollRect.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }
    }
}
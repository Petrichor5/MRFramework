#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MRFramework
{
    [Serializable]
    public class Bind
    {
        /// <summary>
        /// 组件对象
        /// </summary>
        public Object Obj;
        public int InstanceID;
        public string MemberName;
        public string TypeName;
    }

    /*
    * 已弃用，不再维护
    */
    public class CodeGen : MonoBehaviour
    {
        [HideInInspector] public List<Bind> Binds = new List<Bind>();
        [HideInInspector] public string ScriptNamespace = string.Empty;
        [HideInInspector] public string ScriptName = string.Empty;
        [HideInInspector] public string PanelViewFolder = string.Empty;
        [HideInInspector] public string PanelFolder = string.Empty;
        
        public void AddItem(Object obj)
        {
            Bind bind = new Bind()
            {
                MemberName = obj.name,
                TypeName = CheckType(obj),
                Obj = obj
            };
            Binds.Add(bind);
        }
        
        // 根据对象名规则获取对应的组件
        private string CheckType(Object obj)
        {
            string result = obj.GetType().Name;

            if (result == "GameObject" && obj.name.EndsWith("SV"))
            {
                result = obj.name;
            }
            
            if (obj.name.StartsWith("Btn") || obj.name.StartsWith("CloseButton"))
            {
                result = "UnityEngine.UI.Button";
            }
            
            if (obj.name.StartsWith("Txt"))
            {
                result = "UnityEngine.UI.Text";
            }
            
            if (obj.name.StartsWith("Img"))
            {
                result = "UnityEngine.UI.Image";
            }
        
            return result;
        }
    }
}
#endif

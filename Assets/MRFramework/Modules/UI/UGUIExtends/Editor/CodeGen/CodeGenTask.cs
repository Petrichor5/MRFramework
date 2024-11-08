using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
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

    [Serializable]
    public class CodeGenTask
    {
        // 对象 用于挂载生成的代码
        public GameObject Obj;

        // 组件
        public List<Bind> Binds = new List<Bind>();
        
        // 命名空间
        public string ScriptNamespace = string.Empty;

        // 脚本名称
        public string ScriptName = string.Empty;

        // View脚本存储路径
        public string PanelViewFolder = string.Empty;

        // 脚本存储路径
        public string PanelFolder = string.Empty;

        public string PanelViewCode = string.Empty;
        public string PanelCode = string.Empty;
    }
}
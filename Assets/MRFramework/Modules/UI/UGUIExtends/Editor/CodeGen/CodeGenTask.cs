using System;
using System.Collections.Generic;
using UnityEngine;

namespace MRFramework
{
    [Serializable]
    public class CodeGenTask
    {
        // 对象 用于挂载生成的代码
        public GameObject Obj;

        // 组件
        public List<Bind> Binds = new List<Bind>();

        // UI组件事件集合
        public Dictionary<string, string> MethodDic = new Dictionary<string, string>();
        
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
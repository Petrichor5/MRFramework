using System.Collections.Generic;
using UnityEngine;

namespace MRFramework
{
    /// <summary>
    /// 主面板
    /// 主面板不能重复打开，只能同时存在一个，由UIManager统一管理
    /// </summary>
    public class MainPanel : UIBehaviour
    {
        [HideInInspector] public int DiposeTimerKey; // 销毁面板计时器Id
    }
}
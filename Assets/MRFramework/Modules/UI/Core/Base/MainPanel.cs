using System.Collections.Generic;
using UnityEngine;

namespace MRFramework
{
    /// <summary>
    /// 主面板: 不能重复打开，只能同时存在一个，由UIManager统一管理
    /// </summary>
    public class MainPanel : PanelBase
    {
        [HideInInspector] public long DiposeTimerID; // 销毁面板计时器ID
    }
}
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "MRFramework/LogConfig")]
[Serializable]
public class LogConfig : ScriptableObject
{
    [Tooltip("是否打开日志系统")]
    public bool IsOpenLog = true;

    [Tooltip("日志打印前缀")]
    public string LogHeadFix = "### ";

    [Tooltip("是否显示时间戳")]
    public bool IsOpenTime = false;

    [Tooltip("显示线程ID")]
    public bool IsShowThreadId = false;

    [Tooltip("显示FPS")]
    public bool IsShowFPS = true;

    [Tooltip("日志文件存储开关")]
    public bool IsSaveLog = false;

    [Tooltip("显示颜色名字")]
    public bool IsShowColorName = false;

    // 暂时删除
    //[Tooltip("打开日志查看器 (运行时查看日志)")]
    //public bool IsOpenLogsViewer = false;

    // 文件存储路径
    public string LogFileSavePath => Application.persistentDataPath + "/Log/";

    // 日志文件名称
    public string LogFileName => Application.productName + " " + DateTime.Now.ToString("yyyy-MMM-dd HH-mm-ss") + ".log";
}

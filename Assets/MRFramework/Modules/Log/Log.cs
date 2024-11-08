using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

public static class Log
{
    public static LogConfig LogConfig;

    public static void Initalize()
    {
#if OPEN_LOG
        Log.InitLog(UnityEngine.Resources.Load<LogConfig>("LogConfig"));
#else
        UnityEngine.Debug.unityLogger.logEnabled = false; // 关闭Unity原生的日志系统
#endif
    }
    
    [Conditional("OPEN_LOG")]
    public static void InitLog(LogConfig logConfig)
    {
        LogConfig = logConfig;

        // 暂时删除
        //if (LogConfig.IsOpenLogsViewer)
        //{
        //    GameObject reporter = Resources.Load<GameObject>("Reporter");
        //    GameObject.Instantiate(reporter);
        //    reporter.name = "LogReporter";
        //    Log.Info("[Log LogLocalSave] 打开日志查看器");
        //}

        if (LogConfig.IsSaveLog)
        {
            LogLocalSave.Instance.InitLogFileModule(logConfig.LogFileSavePath, logConfig.LogFileName);
            Log.Info("[Log LogLocalSave] 打开日志本地保存");
        }

        if (LogConfig.IsShowFPS)
        {
            _ = FPS.Instance;
            Log.Info("[Log FPS] 打开 FPS 显示");
        }
    }

    #region 普通日志

    [Conditional("OPEN_LOG")]
    public static void Info(object obj)
    {
        if (LogConfig && !LogConfig.IsOpenLog) return;

        string log = GenerateLog(obj.ToString());
        UnityEngine.Debug.Log(log);
    }

    [Conditional("OPEN_LOG")]
    public static void Info(string obj, params object[] args)
    {
        if (LogConfig && !LogConfig.IsOpenLog) return;

        string content = string.Empty;
        if (args != null)
        {
            foreach (var item in args)
            {
                content += item;
            }
        }

        string log = GenerateLog(obj + content);
        UnityEngine.Debug.Log(log);
    }

    [Conditional("OPEN_LOG")]
    public static void Warning(object obj)
    {
        if (LogConfig && !LogConfig.IsOpenLog) return;

        string log = GenerateLog(obj.ToString());
        UnityEngine.Debug.LogWarning(log);
    }

    [Conditional("OPEN_LOG")]
    public static void Warning(string obj, params object[] args)
    {
        if (LogConfig && !LogConfig.IsOpenLog) return;

        string content = string.Empty;
        if (args != null)
        {
            foreach (var item in args)
            {
                content += item;
            }
        }

        string log = GenerateLog(obj + content);
        UnityEngine.Debug.LogWarning(log);
    }

    [Conditional("OPEN_LOG")]
    public static void Error(object obj)
    {
        if (LogConfig && !LogConfig.IsOpenLog) return;

        string log = GenerateLog(obj.ToString());
        UnityEngine.Debug.LogError(log);
    }

    [Conditional("OPEN_LOG")]
    public static void Error(string obj, params object[] args)
    {
        if (LogConfig && !LogConfig.IsOpenLog) return;

        string content = string.Empty;
        if (args != null)
        {
            foreach (var item in args)
            {
                content += item;
            }
        }

        string log = GenerateLog(obj + content);
        UnityEngine.Debug.LogError(log);
    }

    #endregion

    #region 颜色日志打印

    [Conditional("OPEN_LOG")]
    public static void ColorInfo(object obj, ELogColor color = ELogColor.None)
    {
        if (LogConfig && !LogConfig.IsOpenLog) return;

        string log = GenerateLog(obj.ToString(), color);
        log = GetLogColor(log, color);
        UnityEngine.Debug.Log(log);
    }

    [Conditional("OPEN_LOG")]
    public static void InfoGreen(object msg)
    {
        ColorInfo(msg, ELogColor.Green);
    }

    [Conditional("OPEN_LOG")]
    public static void InfoBlue(object msg)
    {
        ColorInfo(msg, ELogColor.Blue);
    }

    [Conditional("OPEN_LOG")]
    public static void InfoYellow(object msg)
    {
        ColorInfo(msg, ELogColor.Yellow);
    }

    [Conditional("OPEN_LOG")]
    public static void InfoRed(object msg)
    {
        ColorInfo(msg, ELogColor.Red);
    }

    #endregion

    public static string GenerateLog(string log, ELogColor colorType = ELogColor.None)
    {
        if (!LogConfig) return string.Empty;
        
        StringBuilder sb = new StringBuilder(LogConfig.LogHeadFix, 100);

        if (LogConfig.IsOpenTime)
            sb.AppendFormat("{0}", DateTime.Now.ToString("hh:mm:ss"));

        sb.AppendFormat(" {0}\n", log);

        if (LogConfig.IsShowThreadId)
            sb.AppendFormat("ThreadID：{0}", Thread.CurrentThread.ManagedThreadId);

        if (LogConfig.IsShowColorName)
            sb.AppendFormat(" \\ Color：{0}", colorType.ToString());

        return sb.ToString();
    }

    public static string GetLogColor(string msg, ELogColor color)
    {
        if (color == ELogColor.None)
        {
            return msg;
        }

        switch (color)
        {
            case ELogColor.Blue:
                msg = $"<color=#00E2FF>{msg}</color>";
                break;
            case ELogColor.Red:
                msg = $"<color=#FF3C00>{msg}</color>";
                break;
            case ELogColor.Yellow:
                msg = $"<color=#FFA900>{msg}</color>";
                break;
            case ELogColor.Green:
                msg = $"<color=#00FF86>{msg}</color>";
                break;
        }

        return msg;
    }
}
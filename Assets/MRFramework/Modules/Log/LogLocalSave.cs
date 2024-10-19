using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using UnityEngine;

//mManualResetEvent.WaitOne();    // 让线程进入等待，并进行阻塞
//mManualResetEvent.Set();        // 设置一个信号，表示线程时需要工作的
//mManualResetEvent.Reset();      // 重置信号，表示没有人指定需要工作

public class LogData
{
    public string Log;
    public string Trace;
    public LogType Type;
}

[MonoSingletonPath("MRFramework/Log/LogLocalSave")]
public class LogLocalSave : MonoSingleton<LogLocalSave>
{
    private StreamWriter m_StreamWriter; // 文件写入流
    private readonly ConcurrentQueue<LogData> m_ConcurrentQueue = new ConcurrentQueue<LogData>(); // 日志消息队列
    private readonly ManualResetEvent m_ManualResetEvent = new ManualResetEvent(false); // 工作信号事件
    private string m_NowTime => DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss");
    private bool m_ThreadRunning = false;

    public void InitLogFileModule(string savePath, string logFileName)
    {
        string logFilePath = Path.Combine(savePath, logFileName);
        Log.Info("[Log LogLocalSave] 日志本地保存路径：" + logFilePath);
        m_StreamWriter = new StreamWriter(logFilePath);
        Application.logMessageReceivedThreaded += OnLogMessageReceivedThreaded;
        m_ThreadRunning = true;
        Thread fileThread = new Thread(FileLogThread);
        fileThread.Start();
    }

    public void FileLogThread()
    {
        while (m_ThreadRunning)
        {
            m_ManualResetEvent.WaitOne(); // 让线程进入等待，并进行阻塞
            if (m_StreamWriter == null) break;
            while (m_ConcurrentQueue.Count > 0 && m_ConcurrentQueue.TryDequeue(out LogData data))
            {
                if (data.Type == LogType.Log)
                {
                    m_StreamWriter.Write("Log >>> ");
                    m_StreamWriter.WriteLine(data.Log);
                    m_StreamWriter.WriteLine(data.Trace);
                }
                else if (data.Type == LogType.Warning)
                {
                    m_StreamWriter.Write("Warning >>> ");
                    m_StreamWriter.WriteLine(data.Log);
                    m_StreamWriter.WriteLine(data.Trace);
                }
                else if (data.Type == LogType.Error)
                {
                    m_StreamWriter.Write("Error >>> ");
                    m_StreamWriter.WriteLine(data.Log);
                    m_StreamWriter.Write("\n");
                    m_StreamWriter.WriteLine(data.Trace);
                }

                m_StreamWriter.Write("\r\n");
            }

            // 保存当前文件内容，使其生效
            m_StreamWriter.Flush();
            m_ManualResetEvent.Reset(); // 重置信号，表示没有人指定需要工作
            Thread.Sleep(1);
        }
    }

    public void OnApplicationQuit()
    {
        Application.logMessageReceivedThreaded -= OnLogMessageReceivedThreaded;
        m_ThreadRunning = false;
        m_ManualResetEvent.Reset();
        m_StreamWriter.Close();
        m_StreamWriter = null;
    }

    private void OnLogMessageReceivedThreaded(string condition, string stackTrace, LogType type)
    {
        m_ConcurrentQueue.Enqueue(new LogData
        {
            Log = m_NowTime + " " + condition,
            Trace = stackTrace,
            Type = type
        });

        m_ManualResetEvent.Set(); // 设置一个信号，表示线程时需要工作的
    }
}
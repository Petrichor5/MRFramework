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
    private StreamWriter mStreamWriter; // 文件写入流
    private readonly ConcurrentQueue<LogData> mConcurrentQueue = new ConcurrentQueue<LogData>(); // 日志消息队列
    private readonly ManualResetEvent mManualResetEvent = new ManualResetEvent(false); // 工作信号事件
    private string mNowTime => DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss");
    private bool mThreadRunning = false;

    public void InitLogFileModule(string savePath, string logFileName)
    {
        string logFilePath = Path.Combine(savePath, logFileName);
        Log.Info("[Log LogLocalSave] 日志本地保存路径：" + logFilePath);
        mStreamWriter = new StreamWriter(logFilePath);
        Application.logMessageReceivedThreaded += OnLogMessageReceivedThreaded;
        mThreadRunning = true;
        Thread fileThread = new Thread(FileLogThread);
        fileThread.Start();
    }

    public void FileLogThread()
    {
        while (mThreadRunning)
        {
            mManualResetEvent.WaitOne(); // 让线程进入等待，并进行阻塞
            if (mStreamWriter == null) break;
            while (mConcurrentQueue.Count > 0 && mConcurrentQueue.TryDequeue(out LogData data))
            {
                if (data.Type == LogType.Log)
                {
                    mStreamWriter.Write("Log >>> ");
                    mStreamWriter.WriteLine(data.Log);
                    mStreamWriter.WriteLine(data.Trace);
                }
                else if (data.Type == LogType.Warning)
                {
                    mStreamWriter.Write("Warning >>> ");
                    mStreamWriter.WriteLine(data.Log);
                    mStreamWriter.WriteLine(data.Trace);
                }
                else if (data.Type == LogType.Error)
                {
                    mStreamWriter.Write("Error >>> ");
                    mStreamWriter.WriteLine(data.Log);
                    mStreamWriter.Write("\n");
                    mStreamWriter.WriteLine(data.Trace);
                }

                mStreamWriter.Write("\r\n");
            }

            // 保存当前文件内容，使其生效
            mStreamWriter.Flush();
            mManualResetEvent.Reset(); // 重置信号，表示没有人指定需要工作
            Thread.Sleep(1);
        }
    }

    public void OnApplicationQuit()
    {
        Application.logMessageReceivedThreaded -= OnLogMessageReceivedThreaded;
        mThreadRunning = false;
        mManualResetEvent.Reset();
        mStreamWriter.Close();
        mStreamWriter = null;
    }

    private void OnLogMessageReceivedThreaded(string condition, string stackTrace, LogType type)
    {
        mConcurrentQueue.Enqueue(new LogData
        {
            Log = mNowTime + " " + condition,
            Trace = stackTrace,
            Type = type
        });

        mManualResetEvent.Set(); // 设置一个信号，表示线程时需要工作的
    }
}
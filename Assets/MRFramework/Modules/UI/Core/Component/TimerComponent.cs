using MRFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerComponent
{
    private List<long> m_TimerIDList; // 计时器

    public long StartTimer(float duration, Action onTimerComplete = null, bool isLooping = false,
    float interval = 0f, Action<float> onInterval = null, bool calculateEscapedTime = true)
    {
        if (m_TimerIDList == null) m_TimerIDList = new List<long>();

        long timerID = TimerManager.Instance.AddTimer(duration, onTimerComplete, isLooping, interval, onInterval,
            calculateEscapedTime);
        m_TimerIDList.Add(timerID);
        return timerID;
    }

    public void RemoveTimer(int timerKey)
    {
        if (m_TimerIDList == null) return;

        TimerManager.Instance.RemoveTimer(timerKey);
        m_TimerIDList.Remove(timerKey);
    }

    public void RemoveAllTimer()
    {
        if (m_TimerIDList == null) return;

        foreach (var timerKey in m_TimerIDList)
        {
            TimerManager.Instance.RemoveTimer(timerKey);
        }

        m_TimerIDList.Clear();
    }

    public void Clear()
    {
        RemoveAllTimer();
    }
}
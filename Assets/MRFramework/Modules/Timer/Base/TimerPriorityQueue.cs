using System.Collections.Generic;

namespace MRFramework
{
    /// <summary>
    /// 计时器优先队列
    /// </summary>
    public class TimerPriorityQueue
    {
        private Dictionary<int, Timer> mTimerKey2Timer = new Dictionary<int, Timer>();
        private SortedList<Timer, int> mSortedList = new SortedList<Timer, int>();
        private int mCount = 0;

        public int Count => mCount;

        public IList<Timer> Values => mSortedList.Keys;

        public void AddTimer(Timer timer)
        {
            if (mSortedList.ContainsKey(timer))
            {
                mSortedList[timer]++;
            }
            else
            {
                mSortedList.Add(timer, 1);
            }

            mTimerKey2Timer[timer.Id] = timer;
            mCount++;
        }

        public void RemoveTimer(Timer timer)
        {
            if (mSortedList.ContainsKey(timer) && --mSortedList[timer] == 0)
            {
                mSortedList.Remove(timer);
            }

            mTimerKey2Timer.Remove(timer.Id);
            mCount--;
        }

        public Timer TryGetTimer(int timerKey)
        {
            return mTimerKey2Timer.TryGetValue(timerKey, out Timer timer) ? timer : null;
        }

        public Timer PopFirst()
        {
            if (Count == 0) return null;

            Timer timer = mSortedList.Keys[0];
            if (--mSortedList[timer] == 0)
            {
                mSortedList.RemoveAt(0);
            }

            mTimerKey2Timer.Remove(timer.Id);
            mCount--;
            return timer;
        }

        public Timer PopLast()
        {
            if (Count == 0) return null;

            int index = mSortedList.Count - 1;
            Timer timer = mSortedList.Keys[index];
            if (--mSortedList[timer] == 0)
            {
                mSortedList.RemoveAt(index);
            }

            mTimerKey2Timer.Remove(timer.Id);
            mCount--;
            return timer;
        }

        public Timer PeekFirst()
        {
            return Count == 0 ? null : mSortedList.Keys[0];
        }

        public Timer PeekLast()
        {
            return Count == 0 ? null : mSortedList.Keys[mSortedList.Count - 1];
        }

        public void Clear()
        {
            foreach (var timer in mTimerKey2Timer.Values)
            {
                timer.Clear();
            }

            mTimerKey2Timer.Clear();
            mSortedList.Clear();
            mCount = 0;
        }
    }
}
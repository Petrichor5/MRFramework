using System;
using System.Collections.Generic;

/// <summary>
/// 优先队列
/// </summary>
public class PriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
{
    private List<(TElement Element, TPriority Priority)> m_Heap = new List<(TElement, TPriority)>();

    public int Count => m_Heap.Count;

    public void Enqueue(TElement element, TPriority priority)
    {
        m_Heap.Add((element, priority));
        HeapifyUp(m_Heap.Count - 1);
    }

    public TElement Dequeue()
    {
        if (m_Heap.Count == 0) return default;

        var root = m_Heap[0].Element;
        m_Heap[0] = m_Heap[m_Heap.Count - 1];
        m_Heap.RemoveAt(m_Heap.Count - 1);

        if (m_Heap.Count > 0)
        {
            HeapifyDown(0);
        }

        return root;
    }

    public TElement Peek()
    {
        if (m_Heap.Count == 0) return default;

        return m_Heap[0].Element;
    }

    public void Clear()
    {
        m_Heap.Clear();
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parentIndex = (index - 1) / 2;
            if (m_Heap[index].Priority.CompareTo(m_Heap[parentIndex].Priority) >= 0)
                break;

            Swap(index, parentIndex);
            index = parentIndex;
        }
    }

    private void HeapifyDown(int index)
    {
        int lastIndex = m_Heap.Count - 1;

        while (index < lastIndex)
        {
            int leftChildIndex = 2 * index + 1;
            int rightChildIndex = 2 * index + 2;
            int smallestIndex = index;

            if (leftChildIndex <= lastIndex && m_Heap[leftChildIndex].Priority.CompareTo(m_Heap[smallestIndex].Priority) < 0)
            {
                smallestIndex = leftChildIndex;
            }

            if (rightChildIndex <= lastIndex && m_Heap[rightChildIndex].Priority.CompareTo(m_Heap[smallestIndex].Priority) < 0)
            {
                smallestIndex = rightChildIndex;
            }

            if (smallestIndex == index)
                break;

            Swap(index, smallestIndex);
            index = smallestIndex;
        }
    }

    private void Swap(int i, int j)
    {
        var temp = m_Heap[i];
        m_Heap[i] = m_Heap[j];
        m_Heap[j] = temp;
    }
}

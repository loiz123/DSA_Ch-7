using System;

public class MyPriorityQueue<T>
{
    private (double priority, T value)[] heap = new (double, T)[100];
    private int size = 0;

    public int Count => size;

    // -----------------------------------------------------------
    // Enqueue
    // -----------------------------------------------------------
    public void Enqueue(T value, double priority)
    {
        if (size == heap.Length)
            throw new Exception("Heap đầy");

        heap[size] = (priority, value);
        SiftUp(size);
        size++;
    }

    // -----------------------------------------------------------
    // Dequeue
    // -----------------------------------------------------------
    public T Dequeue()
    {
        if (size == 0)
            throw new Exception("Empty queue");

        T result = heap[0].value;

        heap[0] = heap[size - 1];
        size--;

        SiftDown(0);

        return result;
    }

    // -----------------------------------------------------------
    // Remove(value)
    // -----------------------------------------------------------
    public bool Remove(T value)
    {
        int idx = -1;

        for (int i = 0; i < size; i++)
        {
            if (heap[i].value.Equals(value))
            {
                idx = i;
                break;
            }
        }

        if (idx == -1)
            return false;

        heap[idx] = heap[size - 1];
        size--;

        // Khôi phục heap
        SiftUp(idx);
        SiftDown(idx);

        return true;
    }

    // -----------------------------------------------------------
    // SiftUp
    // -----------------------------------------------------------
    private void SiftUp(int i)
    {
        while (i > 0)
        {
            int parent = (i - 1) / 2;
            if (heap[i].priority >= heap[parent].priority)
                break;

            Swap(i, parent);
            i = parent;
        }
    }

    // -----------------------------------------------------------
    // SiftDown
    // -----------------------------------------------------------
    private void SiftDown(int i)
    {
        while (true)
        {
            int left = i * 2 + 1;
            int right = i * 2 + 2;
            int smallest = i;

            if (left < size && heap[left].priority < heap[smallest].priority)
                smallest = left;

            if (right < size && heap[right].priority < heap[smallest].priority)
                smallest = right;

            if (smallest == i)
                break;

            Swap(i, smallest);
            i = smallest;
        }
    }

    // -----------------------------------------------------------
    // Swap helper
    // -----------------------------------------------------------
    private void Swap(int a, int b)
    {
        (heap[a], heap[b]) = (heap[b], heap[a]);
    }
}

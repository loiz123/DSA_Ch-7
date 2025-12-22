using System;
using System.Collections;
using System.Collections.Generic;

public class MyList<T> : IEnumerable<T>
{
    private T[] arr = new T[10];
    private int count = 0;

    public int Count => count;

    public void Add(T item)
    {
        if (count == arr.Length)
        {
            T[] newArr = new T[arr.Length * 2];
            Array.Copy(arr, newArr, arr.Length);
            arr = newArr;
        }
        arr[count++] = item;
    }

    public T this[int index]
    {
        get => arr[index];
        set => arr[index] = value;
    }

    public void Reverse()
    {
        int left = 0;
        int right = count - 1;

        while (left < right)
        {
            T temp = arr[left];
            arr[left] = arr[right];
            arr[right] = temp;

            left++;
            right--;
        }
    }

    // ✔ Quan trọng: cho phép dùng foreach + LINQ Select, Where,...
    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < count; i++)
            yield return arr[i];
    }

    // Phải có để hỗ trợ IEnumerable không generic
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    public void RemoveAt(int index)
    {
        if (index < 0 || index >= count)
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
        for (int i = index; i < count - 1; i++)
        {
            arr[i] = arr[i + 1];
        }
        arr[count - 1] = default; // Clear the last element
        count--;
    }
}

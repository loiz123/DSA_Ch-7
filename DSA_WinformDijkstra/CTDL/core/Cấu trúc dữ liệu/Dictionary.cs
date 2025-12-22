using System;

public class MyDictionary<TKey, TValue>
{
    private TKey[] keys = new TKey[1000];
    private TValue[] values = new TValue[1000];
    private int count = 0;

    public int Count => count;

    public void Add(TKey key, TValue value)
    {
        if (ContainsKey(key))
            throw new Exception("Key đã tồn tại");

        keys[count] = key;
        values[count] = value;
        count++;
    }

    public bool ContainsKey(TKey key)
    {
        return IndexOf(key) != -1;
    }

    private int IndexOf(TKey key)
    {
        for (int i = 0; i < count; i++)
            if (keys[i].Equals(key))
                return i;
        return -1;
    }

    public TValue this[TKey key]
    {
        get
        {
            int idx = IndexOf(key);
            if (idx == -1)
                throw new Exception("Key không tồn tại");
            return values[idx];
        }
        set
        {
            int idx = IndexOf(key);
            if (idx == -1)
                Add(key, value);
            else
                values[idx] = value;
        }
    }

    // ⭐ PROPERTY Keys — dùng với foreach
    public TKey[] Keys
    {
        get
        {
            TKey[] arr = new TKey[count];
            Array.Copy(keys, arr, count);
            return arr;
        }
    }
}

using System.Collections.Generic;

public class PriorityQueue<T1, T2>
{
    private List<KeyValuePair<T1, T2>> queue = new List<KeyValuePair<T1, T2>>();
    private int size;
    public void Enqueue(T2 value, T1 key)
    {
        size = queue.Count;
        if(size == 0 || Compare<T2>(value, queue[size - 1].Value))
        {
            queue.Add(new KeyValuePair<T1, T2>(key, value));
        }
        else
        {
            for (int index = 0; index < queue.Count; ++index)
            {
                KeyValuePair<T1, T2> element = queue[index];
                if (Compare<T2>(value, element.Value))
                {
                    queue.Insert(index, (new KeyValuePair<T1, T2>(key, value)));
                }
            }
        }
    }

    public T1 Dequeue()
    {
        if (queue.Count > 0)
        {
            T1 highestPriority = queue[0].Key;
            queue.RemoveAt(0);
            return highestPriority;
        }
        return default(T1);
    }

    public static bool Compare<T>(T v1, T v2)
    {
        IComparer<T> comparer = Comparer<T>.Default;
        return comparer.Compare(v1, v2) < 0 ? true : false;
    }
}

#if UNITY_2021_3_OR_NEWER
using System.Buffers;
#else
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
#endif


namespace UnityPlugin.Bridge
{
    public class UnityArrayPool<T>
    {
        public static UnityArrayPool<T> Shared { get; } = new UnityArrayPool<T>();

#if UNITY_2021_3_OR_NEWER
        public T[] Rent(int minimumLength) => ArrayPool<T>.Shared.Rent(minimumLength);
        public void Return(T[] array, bool clearArray = false) => ArrayPool<T>.Shared.Return(array, clearArray);
#else
        static readonly Dictionary<int, ConcurrentQueue<T[]>> _bucket = new Dictionary<int, ConcurrentQueue<T[]>>();

        public T[] Rent(int minimumLength)
        {
            int bucketLen = GetBucketLength(minimumLength);
            if (_bucket.TryGetValue(bucketLen, out var queue))
            {
                if (queue.TryDequeue(out var result))
                {
                    return result;
                }
            }
            return new T[bucketLen];
        }

        public void Return(T[] array, bool clearArray = false)
        {
            if (array == null) return;
            int len = array.Length;
            if (clearArray) Array.Clear(array, 0, len);
            if (!_bucket.ContainsKey(len))
                _bucket[len] = new ConcurrentQueue<T[]>();
            _bucket[len].Enqueue(array);
        }

        static int GetBucketLength(int min)
        {
            int val = 1;
            while (val < min) val <<= 1;
            return val;
        }
#endif
    }
}
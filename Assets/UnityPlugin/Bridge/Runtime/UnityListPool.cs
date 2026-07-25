using System.Collections.Generic;

#if UNITY_2021_3_OR_NEWER
using UnityEngine.Pool;
#endif

namespace UnityPlugin.Bridge
{
    public static class UnityListPool<T>
    {
#if UNITY_2021_3_OR_NEWER

        public static List<T> Get() => ListPool<T>.Get();
        public static void Release(List<T> toRelease) => ListPool<T>.Release(toRelease);

#else

        static readonly UnityObjectPool<List<T>> s_ListPool = new UnityObjectPool<List<T>>(null, Clear);
        static void Clear(List<T> l) { l.Clear(); }

        public static List<T> Get() => s_ListPool.Get();
        public static void Release(List<T> toRelease) => s_ListPool.Release(toRelease);

#endif
    }
}

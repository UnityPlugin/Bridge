using System.Collections.Generic;

#if UNITY_2021_3_OR_NEWER
using UnityEngine.Pool;
#else
using UnityEngine.UI;
#endif

namespace UnityPlugin.UGUIExt
{
    public static class UnityListPool<T>
    {
        public static List<T> Get() => ListPool<T>.Get();

        public static void Release(List<T> toRelease) => ListPool<T>.Release(toRelease);
    }
}

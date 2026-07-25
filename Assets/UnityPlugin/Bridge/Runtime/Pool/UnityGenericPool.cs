#if UNITY_2021_3_OR_NEWER
using UnityEngine.Pool;
#endif

namespace UnityPlugin.Bridge
{
    public class UnityGenericPool<T> where T : class, new()
    {
#if UNITY_2021_3_OR_NEWER

        public static T Get() => GenericPool<T>.Get();
        public static void Release(T toRelease)=>  GenericPool<T>.Release(toRelease);

#else

        static readonly UnityObjectPool<T> s_Pool = new UnityObjectPool<T>(null, null);

        public static T Get() => s_Pool.Get();
        public static void Release(T toRelease) => s_Pool.Release(toRelease);

#endif
    }
}

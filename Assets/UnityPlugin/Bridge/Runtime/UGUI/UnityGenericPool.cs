#if UNITY_2021_3_OR_NEWER
using UnityEngine.Pool;
#else
using UnityEngine.UI;
#endif

namespace UnityPlugin.Bridge
{
    public class UnityGenericPool<T> where T : class, new()
    {
#if UNITY_2021_3_OR_NEWER
        public static T Get()
        {
            return GenericPool<T>.Get();
        }

        public static void Release(T toRelease)
        {
            GenericPool<T>.Release(toRelease);
        }
#else
        internal static readonly ObjectPool<T> s_Pool = new ObjectPool<T>(null, null);

        public static T Get()
        {
            return s_Pool.Get();
        }

        public static void Release(T toRelease)
        {
            s_Pool.Release(toRelease);
        }
#endif
    }
}

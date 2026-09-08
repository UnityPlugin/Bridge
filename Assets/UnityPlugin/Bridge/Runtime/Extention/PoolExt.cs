using System;

namespace UnityPlugin.Bridge
{
    public static class PoolExt
    {
        public static PoolScope<T> GetScope<T>(out T pool) where T : class, new()
        {
            var scope = new PoolScope<T>();
            pool = scope.Init();
            return scope;
        }

        public struct PoolScope<T> : IDisposable where T : class, new()
        {
            internal T _poolObj;

            internal T Init()
            {
                _poolObj = UnityGenericPool<T>.Get();
                return _poolObj;
            }

            public void Dispose()
            {
                if (_poolObj != null)
                {
                    UnityGenericPool<T>.Release(_poolObj);
                }
            }
        }
    }
}

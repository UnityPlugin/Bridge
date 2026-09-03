using System;
using System.Collections.Generic;
using System.Text;

namespace UnityPlugin.Bridge
{
    public static class StringExt
    {
        class StringCache<T>
        {
            List<T> _link;
            Dictionary<T, string> _cache;

            public StringCache(int capacity)
            {
                _link = new List<T>(capacity);
            }

            public int Capacity
            {
                get => _link.Capacity;
                set
                {
                    if (_link.Capacity != value)
                    {
                        _link.Capacity = value;
                        if (_cache != null)
                        {
                            _cache.Clear();
                            _cache = null;
                        }
                    }
                }
            }

            public bool TryGet(T key, out string result)
            {
                if (_cache == null) _cache = new Dictionary<T, string>(Capacity);

                if (_cache.TryGetValue(key, out result))
                {
                    UpdateOrder(key);
                    return true;
                }

                return false;
            }

            public void Set(T key, string value)
            {
                if (_cache == null) _cache = new Dictionary<T, string>(Capacity);

                _cache[key] = value;
                UpdateOrder(key);
            }

            void UpdateOrder(T key)
            {
                var index = _link.IndexOf(key);
                if (index >= 0)
                {
                    _link.RemoveAt(index);
                    _link.Add(key);
                }
                else
                {
                    if (_link.Count >= _link.Capacity)
                    {
                        _cache.Remove(_link[0]);
                        _link.RemoveAt(0);
                    }
                    _link.Add(key);
                }
            }
        }

        const int DEFAULT_FORMAT_CACHE_SIZE = 256;
        const int DEFAULT_LARGE_FORMAT_CACHE_SIZE = 32;

        static StringCache<long> _intToStringCache = new StringCache<long>(DEFAULT_FORMAT_CACHE_SIZE);
        public static string ToStringCache(this int target, string format = null)
        {
            var key = target * 1000L;
            if (!string.IsNullOrEmpty(format)) key += format.GetHashCode();

            if (!_intToStringCache.TryGet(key, out var result))
            {
                result = target.ToString(format);
                _intToStringCache.Set(key, result);
            }
            return result;
        }

        static StringCache<long> _floatToStringCache = new StringCache<long>(DEFAULT_FORMAT_CACHE_SIZE);
        public static string ToStringCache(this float target, string format = null)
        {
            var key = (long)(target * 1000) * 10;
            if (!string.IsNullOrEmpty(format)) key += format.GetHashCode();

            if (!_floatToStringCache.TryGet(key, out var result))
            {
                result = target.ToString(format);
                _floatToStringCache.Set(key, result);
            }

            return result;
        }

        static Dictionary<string, StringCache<long>> _multiFormatToStringCache = new Dictionary<string, StringCache<long>>(DEFAULT_LARGE_FORMAT_CACHE_SIZE);
        public static string ToStringCache(this string format, float v1, float v2 = 0, float v3 = 0, float v4 = 0, int capcity = 32)
        {
            if (string.IsNullOrEmpty(format)) return null;

            if (!_multiFormatToStringCache.TryGetValue(format, out var cache))
            {
                cache = new StringCache<long>(capcity);
                _multiFormatToStringCache[format] = cache;
            }

            var key = (long)(v1 * 1000);
            key = (key << 7) + (long)(v2 * 1000);
            key = (key << 7) + (long)(v3 * 1000);
            key = (key << 7) + (long)(v4 * 1000);

            if (!cache.TryGet(key, out var result))
            {
                result = string.Format(format, v1, v2, v3, v4);
                cache.Set(key, result);
            }

            return result;
        }

        public struct BuilderScope : IDisposable
        {
            StringBuilder _strBuilder;

            internal BuilderScope(out StringBuilder strBuilder)
            {
                _strBuilder = UnityGenericPool<StringBuilder>.Get();
                _strBuilder.Clear();
                strBuilder = _strBuilder;
            }

            public void Dispose()
            {
                UnityGenericPool<StringBuilder>.Release(_strBuilder);
            }
        }
    }
}

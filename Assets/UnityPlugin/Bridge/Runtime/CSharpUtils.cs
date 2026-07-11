using System;

#if !UNITY_2020_3_OR_NEWER
using System.Collections.Concurrent;
#endif

namespace UnityPlugin.Bridge
{
    public static class CSharpUtils
    {
        public static bool TryParseEnum(this Type enumType, string value, out object result)
        {
            return enumType.TryParseEnum(value, false, out result);
        }

        public static bool TryParseEnum(this Type enumType, string value, bool ignoreCase, out object result)
        {
#if UNITY_2022_3_OR_NEWER
            return Enum.TryParse(enumType, value, ignoreCase, out result);
#else
            result = null;
            if (enumType == null || !enumType.IsEnum || string.IsNullOrEmpty(value))
                return false;

            try
            {
                object parseVal = Enum.Parse(enumType, value, ignoreCase);
                if (Enum.IsDefined(enumType, parseVal))
                {
                    result = parseVal;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
#endif
        }

        public static void Fill<T>(this T[] target, T value)
        {
#if UNITY_2022_3_OR_NEWER
            Array.Fill(target, value);
#else
            if (target == null || target.Length == 0) return;
            for (var i = target.Length - 1; i >= 0; i--)
            {
                target[i] = value;
            }
#endif
        }

        public static void Fill<T>(this T[] target, T value, int startIndex, int count)
        {
#if UNITY_2022_3_OR_NEWER
            Array.Fill(target, value, startIndex, count);
#else
            if (target == null || target.Length == 0) return;

            var start = Math.Max(startIndex, 0);
            var end = Math.Min(startIndex + count, target.Length);
            for (var i = start; i < end; i++)
            {
                target[startIndex + i] = value;
            }
#endif
        }


#if !UNITY_2020_3_OR_NEWER
        public static void Clear<T>(this ConcurrentQueue<T> target)
        {
            if (target == null || target.Count < 1) return;
            while (target.TryDequeue(out _)) { }
        }
#endif
    }
}
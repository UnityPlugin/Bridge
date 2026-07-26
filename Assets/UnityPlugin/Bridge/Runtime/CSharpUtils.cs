using System;

#if !UNITY_2021_3_OR_NEWER
using System.Collections.Generic;
using System.Collections.Concurrent;
#endif

namespace UnityPlugin.Bridge
{
    public static class CSharpUtils
    {
        #region Type Enum

        public static bool TryParseEnum(this Type enumType, string value, out object result)
        {
            return enumType.TryParseEnum(value, false, out result);
        }

        public static bool TryParseEnum(this Type enumType, string value, bool ignoreCase, out object result)
        {
#if UNITY_2021_3_OR_NEWER
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

        #endregion

        #region Type Array

        public static void Fill<T>(this T[] target, T value)
        {
#if UNITY_2021_3_OR_NEWER
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
#if UNITY_2021_3_OR_NEWER
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

        #endregion

        #region Type Queue

#if !UNITY_2021_3_OR_NEWER

        public static bool TryDequeue<T>(this Queue<T> target, out T result)
        {
            result = default;
            if (target == null) return false;

            if (target.Count < 1) return false;
            try
            {
                result = target.Dequeue();
                return true;
            }
            catch (Exception) { }
            return false;
        }

        public static bool TryPeek<T>(this Queue<T> target, out T result)
        {
            result = default;
            if (target == null) return false;

            if (target.Count < 1) return false;
            try
            {
                result = target.Peek();
                return true;
            }
            catch (Exception) { }

            return false;
        }

#endif

        #endregion

        #region Type Stack

#if !UNITY_2021_3_OR_NEWER

        public static bool TryPop<T>(this Stack<T> target, out T result)
        {
            result = default;
            if (target == null) return false;

            if (target.Count < 1) return false;
            try
            {
                result = target.Pop();
                return true;
            }
            catch (Exception) { }
            return false;
        }

        public static bool TryPeek<T>(this Stack<T> target, out T result)
        {
            result = default;
            if (target == null) return false;

            if (target.Count < 1) return false;
            try
            {
                result = target.Peek();
                return true;
            }
            catch (Exception) { }

            return false;
        }

#endif

        #endregion

        #region Type ConcurrentQueue

#if !UNITY_2021_3_OR_NEWER
        public static void Clear<T>(this ConcurrentQueue<T> target)
        {
            if (target == null || target.Count < 1) return;
            while (target.TryDequeue(out _)) { }
        }
#endif

        #endregion
    }
}
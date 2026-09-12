using System.Text;

namespace UnityPlugin.Bridge
{
    public static class StringBuilderExt
    {
        const string TRIM_CHARS = " \t\r\n";

        public static StringBuilder ToLower(this StringBuilder target)
        {
            if (target == null) return null;

            for (var i = target.Length - 1; i >= 0; i--)
            {
                target[i] = target[i].ToLower();
            }

            return target;
        }

        public static StringBuilder ToUpper(this StringBuilder target)
        {
            if (target == null) return null;

            for (var i = target.Length - 1; i >= 0; i--)
            {
                target[i] = target[i].ToUpper();
            }

            return target;
        }

        public static StringBuilder Trim(this StringBuilder target, char trimChar)
        {
            if (target == null) return null;

            target.TrimStart(trimChar);
            target.TrimEnd(trimChar);

            return target;
        }

        public static StringBuilder Trim(this StringBuilder target, string trimStr = null)
        {
            if (target == null) return null;

            target.TrimStart(trimStr);
            target.TrimEnd(trimStr);

            return target;
        }

        public static StringBuilder TrimStart(this StringBuilder target, string trimStr = null)
        {
            if (target == null) return null;

            if (string.IsNullOrEmpty(trimStr)) trimStr = TRIM_CHARS;

            var length = 0;
            for (var i = 0; i < target.Length; i++)
            {
                if (trimStr.Contains(target[i])) length++;
                else break;
            }

            if (length > 0) target.Remove(0, length);

            return target;
        }

        public static StringBuilder TrimEnd(this StringBuilder target, string trimStr = null)
        {
            if (target == null) return null;

            if (string.IsNullOrEmpty(trimStr)) trimStr = TRIM_CHARS;

            var length = 0;
            for (var i = target.Length - 1; i >= 0; i--)
            {
                if (trimStr.Contains(target[i])) length++;
                else break;
            }

            if (length > 0) target.Remove(target.Length - length, length);

            return target;
        }

        public static StringBuilder TrimStart(this StringBuilder target, char trimChar)
        {
            if (target == null) return null;

            var length = 0;
            for (var i = 0; i < target.Length; i++)
            {
                if (trimChar == target[i]) length++;
                else break;
            }

            if (length > 0) target.Remove(0, length);

            return target;
        }

        public static StringBuilder TrimEnd(this StringBuilder target, char trimChar)
        {
            if (target == null) return null;

            var length = 0;
            for (var i = target.Length - 1; i >= 0; i--)
            {
                if (trimChar == target[i]) length++;
                else break;
            }

            if (length > 0) target.Remove(target.Length - length, length);

            return target;
        }

        internal static char ToUpper(this char c)
        {
            if (c >= 'a' && c <= 'z') c -= '\x20';
            return c;
        }

        internal static char ToLower(this char c)
        {
            if (c >= 'A' && c <= 'Z') c += '\x20';
            return c;
        }
    }
}

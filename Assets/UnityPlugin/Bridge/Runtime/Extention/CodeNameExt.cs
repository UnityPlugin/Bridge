using System.Text;

namespace UnityPlugin.Bridge
{
    public static class CodeNameExt
    {
        public enum CodeStyle
        {
            Type_PascalCase,
            Type_camelCase,
            Type_CapsSpace,
            Type_snake_case,
            Type_MACROS,
            Type_kebab_case,
        }

        enum CharType
        {
            Upper,
            Lower,
            Num,
            Other,
        }

        const string TRIM_CHARS = " _-\r\n\t";

        public static StringBuilder ToCodeName(this StringBuilder target, CodeStyle style)
        {
            if (target == null) return target;

            target.Clear().Append(target);

            var isStart = true;
            var isEnd = true;

            var currentCharType = CharType.Other;
            var lastCharType = CharType.Other;
            var i = 0;
            while (i < target.Length)
            {
                if (TRIM_CHARS.Contains(target[i]))
                {
                    while (i < target.Length && TRIM_CHARS.Contains(target[i]))
                    {
                        target.Remove(i, 1);
                    }

                    lastCharType = CharType.Other;
                    if (i >= target.Length) break;
                    if (!isEnd) isStart = true;
                }

                var c = target[i];
                if (c >= '0' && c <= '9') currentCharType = CharType.Num;
                else if (c >= 'a' && c <= 'z') currentCharType = CharType.Lower;
                else if (c >= 'A' && c <= 'Z') currentCharType = CharType.Upper;
                else currentCharType = CharType.Other;

                if (isStart)
                {
                    switch (style)
                    {
                        case CodeStyle.Type_PascalCase:
                        case CodeStyle.Type_CapsSpace:
                        case CodeStyle.Type_MACROS:
                            target[i] = target[i].ToUpper();
                            break;
                        case CodeStyle.Type_snake_case:
                        case CodeStyle.Type_kebab_case:
                            target[i] = target[i].ToLower();
                            break;
                        case CodeStyle.Type_camelCase:
                            target[i] = i == 0 ? target[i].ToLower() : target[i].ToUpper(); ;
                            break;
                    }
                    isStart = false;
                    isEnd = false;
                }
                else if (isEnd)
                {
                    switch (style)
                    {
                        case CodeStyle.Type_PascalCase:
                        case CodeStyle.Type_camelCase:
                            i--;
                            break;
                        case CodeStyle.Type_MACROS:
                        case CodeStyle.Type_snake_case:
                            target.Insert(i, '_');
                            break;
                        case CodeStyle.Type_CapsSpace:
                            target.Insert(i, ' ');
                            break;
                        case CodeStyle.Type_kebab_case:
                            target.Insert(i, '-');
                            break;
                    }
                    isEnd = false;
                    isStart = true;
                }
                else
                {
                    if (lastCharType != currentCharType)
                    {
                        if (lastCharType == CharType.Upper && currentCharType == CharType.Lower)
                        {
                            //Skip
                        }
                        else
                        {
                            isEnd = true;
                            i--;
                        }
                    }

                    if (!isEnd)
                    {
                        switch (style)
                        {
                            case CodeStyle.Type_MACROS:
                                target[i] = target[i].ToUpper();
                                break;
                            case CodeStyle.Type_PascalCase:
                            case CodeStyle.Type_CapsSpace:
                            case CodeStyle.Type_snake_case:
                            case CodeStyle.Type_kebab_case:
                            case CodeStyle.Type_camelCase:
                                target[i] = target[i].ToLower();
                                break;
                        }
                    }

                    if (i + 1 < target.Length && TRIM_CHARS.Contains(target[i + 1]))
                    {
                        isEnd = true;
                    }
                }

                lastCharType = currentCharType;
                i++;
            }

            return target;
        }

        public static string ToCodeName(this string target, CodeStyle style)
        {
            if (string.IsNullOrEmpty(target)) return target;

            using (PoolExt.GetScope<StringBuilder>(out var sb))
            {
                return sb.ToCodeName(style).ToString();
            }
        }
    }
}
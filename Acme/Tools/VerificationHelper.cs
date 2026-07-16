using System;
using System.Text.RegularExpressions;

namespace Acme.Tools
{
    /// <summary>
    /// 验证
    /// </summary>
    public static class VerificationHelper
    {
    /// <summary>
    /// 判断字符串是否为 null 或空白（保留 IsNotNull 方法名兼容旧代码，但语义为"无内容"）
    /// </summary>
    /// <param name="JudgmentObject"></param>
    /// <returns></returns>
    public static bool IsNotNull(this string JudgmentObject)
    {
        return string.IsNullOrWhiteSpace(JudgmentObject);
    }

    /// <summary>
    /// 判断字符串是否不为 null 或空白（IsNotNull 的反向）
    /// </summary>
    /// <param name="JudgmentObject"></param>
    /// <returns></returns>
    public static bool HasValue(this string JudgmentObject)
    {
        return !string.IsNullOrWhiteSpace(JudgmentObject);
    }

    /// <summary>
    /// 判断 Guid 是否为默认值（Empty）
    /// </summary>
    /// <param name="JudgmentObject"></param>
    /// <returns></returns>
    public static bool IsNotDefault(this Guid JudgmentObject)
    {
        return JudgmentObject == Guid.Empty;
    }

    /// <summary>
    /// 判断 Guid 是否不为默认值（IsNotDefault 的反向）
    /// </summary>
    /// <param name="JudgmentObject"></param>
    /// <returns></returns>
    public static bool IsDefault(this Guid JudgmentObject)
    {
        return JudgmentObject == Guid.Empty;
    }

    /// <summary>
    /// 判断 int 是否为默认值（0）
    /// </summary>
    /// <param name="JudgmentObject"></param>
    /// <returns></returns>
    public static bool IsNotDefault(this int JudgmentObject)
    {
        return JudgmentObject == 0;
    }

    /// <summary>
    /// 判断 int 是否不为默认值（IsNotDefault 的反向）
    /// </summary>
    /// <param name="JudgmentObject"></param>
    /// <returns></returns>
    public static bool IsNotZero(this int JudgmentObject)
    {
        return JudgmentObject != 0;
    }

        /// <summary>
        /// Guid是否等于默认值
        /// </summary>
        /// <param name="guidValue">guid值</param>
        /// <returns></returns>
        public static bool IsDefault(this Guid? guidValue)
        {
            return guidValue == Guid.Empty || !guidValue.HasValue;
        }

    /// <summary>
    /// 自定义正则表达式验证
    /// </summary>
    /// <param name="input">要验证的输入字符串</param>
    /// <param name="pattern">正则表达式模式</param>
    /// <returns></returns>
    public static bool RegexVerification(
        this string input,
        string pattern)
    {
        if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(pattern))
            return false;
        Regex regex = new(pattern, RegexOptions.Compiled);
        return regex.IsMatch(input.Trim());
    }
    }
}

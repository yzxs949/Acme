using System;
using System.Text.RegularExpressions;

namespace Acme.Tools;

/// <summary>
/// 验证
/// </summary>
public static class VerificationHelper
{
    /// <summary>
    /// 字符串是否不为空
    /// </summary>
    public static bool IsNotNull(this string value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }

    /// <summary>
    /// Guid是否不是默认值
    /// </summary>
    public static bool IsNotDefault(this Guid value)
    {
        return value != Guid.Empty;
    }

    /// <summary>
    /// int是否不是默认值
    /// </summary>
    public static bool IsNotDefault(this int value)
    {
        return value != 0;
    }

    /// <summary>
    /// Guid是否等于默认值
    /// </summary>
    public static bool IsDefault(this Guid? guidValue)
    {
        return !guidValue.HasValue || guidValue == Guid.Empty;
    }

    /// <summary>
    /// 自定义正则表达式验证
    /// </summary>
    /// <param name="input">验证字符串</param>
    /// <param name="pattern">正则表达式</param>
    /// <returns></returns>
    public static bool RegexVerification(this string input, string pattern)
    {
        return !string.IsNullOrEmpty(input) && Regex.IsMatch(input.Trim(), pattern);
    }
}

using System;
using System.Text.RegularExpressions;

namespace Acme.Tool
{
    /// <summary>
    /// 验证
    /// </summary>
    public static class VerificationHelper
    {
        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="JudgmentObject"></param>
        /// <returns></returns>
        public static bool IsNotNull(this string JudgmentObject)
        {
            return string.IsNullOrWhiteSpace(JudgmentObject);
        }

        /// <summary>
        /// 是否是默认值
        /// </summary>
        /// <param name="JudgmentObject"></param>
        /// <returns></returns>
        public static bool IsNotDefault(this Guid JudgmentObject)
        {
            return JudgmentObject == Guid.Empty;
        }

        /// <summary>
        /// 是否是默认值
        /// </summary>
        /// <param name="JudgmentObject"></param>
        /// <returns></returns>
        public static bool IsNotDefault(this int JudgmentObject)
        {
            return JudgmentObject == 0;
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
        /// <param name="ValidationParameters">验证字符串</param>
        /// <param name="RegularExpression">正则表达式</param>
        /// <returns></returns>
        public static bool RegexVerification(
            string ValidationParameters,
            string RegularExpression)
        {
            Regex verIdCard = new(ValidationParameters);
            bool IsOk = (ValidationParameters != "" && verIdCard.IsMatch(ValidationParameters.Trim()));
            return IsOk;
        }
    }
}

using System;
using System.Security.Cryptography;
using System.Text;

namespace Acme
{
    /// <summary>
    /// MD5加密
    /// </summary>
    public static class MD5Encryption
    {
        /// <summary>
        ///  字符串 MD5 比较
        /// </summary>
        /// <param name="text">加密文本</param>
        /// <param name="hash">MD5 字符串</param>
        /// <param name="uppercase">是否输出大写加密，默认 false</param>
        /// <returns></returns>
        public static bool Compare(string text, string hash, bool uppercase = false)
        {
            string value = Encrypt(text, uppercase);
            return hash.Equals(value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///  MD5 加密
        /// </summary>
        /// <param name="text">加密文本</param>
        /// <param name="uppercase">是否输出大写加密，默认 false</param>
        /// <returns></returns>
        public static string Encrypt(string text, bool uppercase = false)
        {
            using MD5 mD = MD5.Create();
            byte[] array = mD.ComputeHash(Encoding.UTF8.GetBytes(text));
            StringBuilder stringBuilder = new();
            foreach (var item in array)
            {
                stringBuilder.Append(item.ToString("x2"));
            }
            string text2 = stringBuilder.ToString();
            return (!uppercase) ? text2 : text2.ToUpper();
        }
    }
}

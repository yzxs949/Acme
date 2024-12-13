using System.Text;

namespace Acme.Pinyins;

/// <summary>
/// 拼音
/// </summary>
public static class Pinyin
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string GetInitials(string text)
    {
        text = text.Trim();
        StringBuilder stringBuilder = new();
        for (int i = 0; i < text.Length; i++)
        {
            string pinyin = GetPinyin(text[i]);
            if (pinyin != "")
            {
                stringBuilder.Append(pinyin[0]);
            }
        }

        return stringBuilder.ToString().ToUpper();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string GetInitials(string text, Encoding encoding)
    {
        return ConvertEncoding(GetInitials(ConvertEncoding(text, encoding, Encoding.UTF8)), Encoding.UTF8, encoding);
    }

    /// <summary>
    /// 获取拼音
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string GetPinyin(string text)
    {
        StringBuilder stringBuilder = new();
        for (int i = 0; i < text.Length; i++)
        {
            string pinyin = GetPinyin(text[i]);
            if (pinyin != "")
            {
                stringBuilder.Append(pinyin);
            }

            stringBuilder.Append(' ');
        }

        return stringBuilder.ToString().Trim();
    }

    /// <summary>
    /// 获取拼音
    /// </summary>
    /// <param name="text"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string GetPinyin(string text, Encoding encoding)
    {
        return ConvertEncoding(GetPinyin(ConvertEncoding(text.Trim(), encoding, Encoding.UTF8)), Encoding.UTF8, encoding);
    }

    /// <summary>
    /// 获取中文
    /// </summary>
    /// <param name="pinyin"></param>
    /// <returns></returns>
    public static string GetChineseText(string pinyin)
    {
        string str = pinyin.Trim().ToLower();
        var codes = PyCode.codes;
        foreach (string text in codes)
        {
            if (text.StartsWith(str + " ") || text.StartsWith(str + ":"))
            {
                return text[7..];
            }
        }

        return "";
    }

    /// <summary>
    /// 获取拼音
    /// </summary>
    /// <param name="pinyin"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string GetChineseText(string pinyin, Encoding encoding)
    {
        return ConvertEncoding(GetChineseText(ConvertEncoding(pinyin, encoding, Encoding.UTF8)), Encoding.UTF8, encoding);
    }

    /// <summary>
    /// 获取拼音
    /// </summary>
    /// <param name="ch"></param>
    /// <returns></returns>
    public static string GetPinyin(char ch)
    {
        short hashIndex = GetHashIndex(ch);
        for (int i = 0; i < PyHash.hashes[hashIndex].Length; i++)
        {
            short num = PyHash.hashes[hashIndex][i];
            if (PyCode.codes[num].IndexOf(ch, 7) != -1)
            {
                return PyCode.codes[num][..6].Trim();
            }
        }

        return ch.ToString();
    }

    /// <summary>
    /// 获取拼音
    /// </summary>
    /// <param name="ch"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string GetPinyin(char ch, Encoding encoding)
    {
        ch = ConvertEncoding(ch.ToString(), encoding, Encoding.UTF8)[0];
        return ConvertEncoding(GetPinyin(ch), Encoding.UTF8, encoding);
    }

    /// <summary>
    /// 编码
    /// </summary>
    /// <param name="text"></param>
    /// <param name="srcEncoding"></param>
    /// <param name="dstEncoding"></param>
    /// <returns></returns>
    public static string ConvertEncoding(string text, Encoding srcEncoding, Encoding dstEncoding)
    {
        byte[] bytes = srcEncoding.GetBytes(text);
        byte[] bytes2 = Encoding.Convert(srcEncoding, dstEncoding, bytes);
        return dstEncoding.GetString(bytes2);
    }

    /// <summary>
    /// 获取索引
    /// </summary>
    /// <param name="ch"></param>
    /// <returns></returns>
    private static short GetHashIndex(char ch)
    {
        return (short)(ch % (long)PyCode.codes.Count);
    }
}
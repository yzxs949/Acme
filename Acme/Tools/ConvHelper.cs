using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Acme.Apps;
namespace Acme.Tools;

/// <summary>
/// 类型转换
/// </summary>
public static class ConvHelper
{
    /// <summary>  
    /// 获取时间戳  13位
    /// </summary>  
    /// <returns></returns>  
    public static long GetTimeStamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }

    /// <summary>
    /// 字符串转换成Int32类型
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="defaultVal">转换失败默认值</param>
    /// <returns></returns>
    public static int ToInt(this string str, int defaultVal = 0)
    {
        if (int.TryParse(str, out int val))
        {
            return val;
        }
        return defaultVal;
    }

    /// <summary>
    /// 字符串转换成short类型
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="defaultVal">转换失败默认值</param>
    /// <returns></returns>
    public static short ToShort(this string str, short defaultVal = 0)
    {
        if (short.TryParse(str, out short val))
        {
            return val;
        }
        return defaultVal;
    }

    /// <summary>
    /// 字符串转换成DateTime类型
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns>转换失败返回1970-1-1</returns>
    public static DateTime ToDate(this string str)
    {
        if (DateTime.TryParse(str, out DateTime date) == false)
        {
            date = new DateTime(1970, 1, 1);
        }
        return date;
    }

    /// <summary>
    /// 字符转换成DateTime类型
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="defaultDate">转换失败返回值</param>
    public static DateTime ToDate(this string str, DateTime defaultDate)
    {
        if (DateTime.TryParse(str, out DateTime date) == false)
        {
            date = defaultDate;
        }
        return date;
    }

    /// <summary>
    /// 日期转化中文星期文本。（一二三四五六日）
    /// </summary>
    /// <param name="dt">日期</param>
    /// <returns></returns>
    public static string ToChWeekName(this DateTime dt)
    {
        return dt.DayOfWeek switch
        {
            DayOfWeek.Sunday => "日",
            DayOfWeek.Saturday => "六",
            DayOfWeek.Friday => "五",
            DayOfWeek.Thursday => "四",
            DayOfWeek.Wednesday => "三",
            DayOfWeek.Tuesday => "二",
            DayOfWeek.Monday => "一",
            _ => string.Empty,
        };
    }

    /// <summary>
    /// 文件路径转Base64位字符
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns></returns>
    public static string ToBase64String(this string filePath)
    {
        try
        {
            byte[] bytes = File.ReadAllBytes(filePath);
            return Convert.ToBase64String(bytes);
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// 绝对路径转相对路径
    /// </summary>
    /// <param name="absolutelyPath">绝对路径</param>
    /// <returns>绝对路径</returns>
    public static string ToUrlPath(this string absolutelyPath)
    {
        string UrlPath = absolutelyPath.Replace(App.Path + "\\", "");
        return UrlPath.Replace("\\", "/");
    }

    /// <summary>
    /// 相对路径转绝对路径
    /// </summary>
    /// <param name="absolutelyPath">绝对路径</param>
    /// <returns>绝对路径</returns>
    public static string ToPath(this string absolutelyPath)
    {
        if (absolutelyPath[0] != '/')
        {
            throw new Exception("参数格式不正确");
        }
        string UrlPath = $"{App.Path}{absolutelyPath[1..]}";
        return UrlPath.Replace("/", "\\");
    }

    /// <summary>
    /// DateTime转时间
    /// </summary>
    /// <param name="DataTimeValues"></param>
    /// <returns></returns>
    public static string ToTimeHHmmssString(this DateTime DataTimeValues)
    {
        return DataTimeValues.ToString($"HH:mm:ss");
    }

    /// <summary>
    /// DateTime转时间string时间格式
    /// </summary>
    /// <param name="DataTimeValues"></param>
    /// <param name="CharString1">年月日分割字符</param>
    /// <param name="CharString2">时分秒分割字符</param>
    /// <param name="CharString3">年月日到时分秒分割字符</param>
    /// <returns></returns>
    public static string ToDataTimeString(
        this DateTime DataTimeValues,
        string CharString1 = "-",
        string CharString2 = ":",
        string CharString3 = " ")
    {
        return DataTimeValues.ToString($"yyyy{CharString1}MM{CharString1}dd{CharString3}HH{CharString2}mm{CharString2}ss");
    }

    /// <summary>
    /// 将字符串编码转为 UTF-8
    /// </summary>
    /// <param name="strText">字符串</param>
    /// <returns></returns>
    public static string ToUtf8(this string strText)
    {
        byte[] buffer = Encoding.Default.GetBytes(strText);
        byte[] utf8Bytes = Encoding.Convert(Encoding.Default, Encoding.UTF8, buffer);
        return Encoding.UTF8.GetString(utf8Bytes);
    }

    /// <summary>
    /// 截取两个字符串中间的内容
    /// </summary>
    /// <param name="source">要截取的内容</param>
    /// <param name="startStr">起始字符串</param>
    /// <param name="endStr">结束字符串</param>
    /// <returns></returns>
    public static string MidStrEx(this string source, string startStr, string endStr)
    {
        int startIndex = source.IndexOf(startStr);
        if (startIndex == -1)
            return string.Empty;

        string tmp = source[(startIndex + startStr.Length)..];
        int endIndex = tmp.IndexOf(endStr);
        if (endIndex == -1)
            return string.Empty;

        return tmp.Remove(endIndex);
    }

    /// <summary>
    /// 获取随机字符串
    /// </summary>
    /// <param name="strText">随机字符内容</param>
    /// <param name="len">长度</param>
    /// <returns></returns>
    public static string ToRandomCode(this string strText, int len)
    {
        string[] strArray = strText.Split(',');
        if (strArray.Length == 0 || len <= 0)
            return string.Empty;
        StringBuilder code = new(capacity: len);
        // 使用加密安全的随机数生成器，避免验证码/随机码被预测
        for (int i = 0; i < len; i++)
        {
            code.Append(strArray[RandomNumberGenerator.GetInt32(0, strArray.Length)]);
        }
        return code.ToString();
    }

    /// <summary>
    /// 计算时间间隔
    /// </summary>
    /// <param name="startDate">开始时间</param>
    /// <param name="endDate">结束时间</param>
    /// <returns>时间间隔</returns>
    public static string LastMonth(this DateTime startDate, DateTime endDate)
    {
        // 计算两个日期之间的年、月、天间隔
        int years = endDate.Year - startDate.Year;
        int months = endDate.Month - startDate.Month;
        int days = endDate.Day - startDate.Day;

        // 调整月份和天数
        if (days < 0)
        {
            months--;
            DateTime lastMonth = endDate.AddMonths(-1);
            days += DateTime.DaysInMonth(lastMonth.Year, lastMonth.Month);
        }

        if (months < 0)
        {
            years--;
            months += 12;
        }

        string result = "";
        if (years > 0)
            result += $"{years}年";
        if (months > 0)
            result += $"{months}月";
        if (days > 0)
            result += $"{days}天";
        return result;
    }
}

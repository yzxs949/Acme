using System;
using System.IO;
using System.Text;
namespace Acme;

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
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds * 1000);
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
    /// <param name="FilesPath">文件路径</param>
    /// <returns></returns>
    public static string ToBase64String(this string FilesPath)
    {
        FileStream fsForRead = new(FilesPath, FileMode.Open);
        string base64Str = "";
        try
        {
            fsForRead.Seek(0, SeekOrigin.Begin);
            byte[] bs = new byte[fsForRead.Length];
            int log = Convert.ToInt32(fsForRead.Length);
            fsForRead.Read(bs, 0, log);
            base64Str = Convert.ToBase64String(bs);
            return base64Str;
        }
        catch
        {
            return base64Str;
        }
        finally
        {
            fsForRead.Close();
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
    /// 中文乱码转Utf8
    /// </summary>
    /// <param name="strText">字符串</param>
    /// <returns></returns>
    public static string ToUtf8(this string strText)
    {
        byte[] buffer1 = Encoding.Default.GetBytes(strText);
        byte[] buffer2 = Encoding.Convert(Encoding.UTF8, Encoding.Default, buffer1, 0, buffer1.Length);
        string strBuffer = Encoding.Default.GetString(buffer2, 0, buffer2.Length);
        return strBuffer;
    }

    /// <summary>
    /// 截取两个字符串中间的内容
    /// </summary>
    /// <param name="sourse">要截取的内容</param>
    /// <param name="startstr">第一个字符</param>
    /// <param name="endstr"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static string MidStrEx(this string sourse, string startstr, string endstr)
    {
        string result = string.Empty;
        int startindex, endindex;
        try
        {
            startindex = sourse.IndexOf(startstr);
            if (startindex == -1)
                return result;
            string tmpstr = sourse.Substring(startindex + startstr.Length);
            endindex = tmpstr.IndexOf(endstr);
            if (endindex == -1)
                return result;
            result = tmpstr.Remove(endindex);
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    /// 获取随机字符串
    /// </summary>
    /// <param name="strText">随机字符内容</param>
    /// <param name="len">长度</param>
    /// <returns></returns>
    public static string ToRandomCode(this string strText, int len)
    {
        string[] strArry = strText.Split(',');
        string code = "";
        Random rand = new(Guid.NewGuid().GetHashCode());
        for (int i = 0; i < len; i++)
        {
            code += strArry[rand.Next(0, strArry.Length)];
        }
        return code;
    }
}

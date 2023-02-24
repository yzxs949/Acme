using Newtonsoft.Json;
using System;
using System.IO;

namespace Acme.Tool
{
    /// <summary>
    /// 类型转换
    /// </summary>
    public static class ConvHelper
    {
        /// <summary>
        /// 实体转Json
        /// </summary>
        /// <param name="data">要转化的实体</param>
        /// <returns></returns>
        public static string ToJson(this object data)
        {
            return JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// Json字符串转实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <returns></returns>
        public static T ToEntity<T>(this string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return default;
            }
        }

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
            string UrlPath = $"{App.Path}{absolutelyPath.Substring(1, absolutelyPath.Length - 1)}";
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
    }
}

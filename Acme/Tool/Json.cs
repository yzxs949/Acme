using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;

namespace Acme
{
    /// <summary>
    /// Js处理类
    /// </summary>
    public static class Json
    {
        /// <summary>
        /// 序列化为Json格式
        /// </summary>
        /// <param name="obj">需要序列化的参数</param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Json字符串转化为实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static T ToEntity<T>(this string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>
        /// Json转JObject
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static JObject ToJObject(string str)
        {
            return (JObject)JsonConvert.DeserializeObject(str);
        }
    }
}

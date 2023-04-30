using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Acme
{

    /// <summary>
    /// 系统配置参数
    /// </summary>
    public static class App
    {
        private static JsonFile JsonFileVariable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        static App()
        {
            JsonFileVariable = new("appsettings.json");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetValue(string key, string name)
        {
            return JsonFileVariable.GetValue(key, name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            return JsonFileVariable.GetValue(key);
        }

        /// <summary>
        /// 系统路径
        /// </summary>
        public static string SystemPath => Directory.GetCurrentDirectory();

        /// <summary>
        /// 系统路径
        /// </summary>
        public static string Path => AppDomain.CurrentDomain.BaseDirectory;
    }
}
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
        private static IConfiguration Configuration { get; set; }

        /// <summary>
        /// 
        /// </summary>
        static App()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")//你的配置文件
                .Build();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetValue(string key, string name)
        {
            return Configuration.GetSection(key)[name];
        }

        /// <summary>
        /// 系统路径
        /// </summary>
        public static string SystemPath => Directory.GetCurrentDirectory();

        /// <summary>
        /// 
        /// </summary>
        public static string Path => AppDomain.CurrentDomain.BaseDirectory;
    }
}

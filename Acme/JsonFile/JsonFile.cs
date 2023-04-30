using Microsoft.Extensions.Configuration;
using System.IO;
namespace Acme
{

    /// <summary>
    /// 获取Json文件类
    /// </summary>
    public class JsonFile
    {
        /// <summary>
        /// 依赖注入参数
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jsonFileName">json文件名称</param>
        public JsonFile(string jsonFileName)
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(jsonFileName)//你的配置文件
                .Build();
        }

        /// <summary>
        /// 获取配置文件参数值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetValue(string key, string name)
        {
            return _configuration.GetSection(key)[name];
        }

        /// <summary>
        /// 获取配置文件参数值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            return _configuration.GetSection(key).Value;
        }
    }
}
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace Acme.Tool
{
    /// <summary>
    /// http请求通用类
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// HttpPost接口
        /// </summary>
        /// <param name="postUrl">请求地址</param>
        /// <param name="requestData">请求参数</param>
        /// <returns></returns>
        public static string HttpClientPost(string postUrl, object requestData)
        {
            HttpClient httpClient = new();
            try
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new("application/json"));
                using HttpContent httpContent = JsonContent.Create(requestData);
                httpContent.Headers.Add("Method", "POST");
                HttpResponseMessage response = httpClient.PostAsync(postUrl, httpContent).Result;
                Stream stream = response.Content.ReadAsStream();
                StreamReader reader = new(stream, Encoding.UTF8);
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// HttpGet接口
        /// </summary>
        /// <param name="getUrl">请求地址</param>
        /// <returns></returns>
        public static string HttpClientGet(string getUrl)
        {
            HttpClient httpClient = new();
            try
            {
                HttpResponseMessage response = httpClient.GetAsync(getUrl).Result;
                Stream stream = response.Content.ReadAsStream();
                StreamReader reader = new(stream, Encoding.UTF8);
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}

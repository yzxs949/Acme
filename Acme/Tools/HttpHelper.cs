using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
namespace Acme.Tools;

/// <summary>
/// http请求通用类
/// </summary>
public static class HttpHelper
{
    // 复用 HttpClient 实例避免端口耗尽（Socket exhaustion）
    private static readonly HttpClient _httpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(30)
    };

    /// <summary>
    /// HttpPost接口（同步）
    /// </summary>
    /// <param name="postUrl">请求地址</param>
    /// <param name="requestData">请求参数</param>
    /// <returns></returns>
    public static string HttpClientPost(string postUrl, object requestData)
    {
        try
        {
            using HttpContent httpContent = JsonContent.Create(requestData);
            httpContent.Headers.Add("Method", "POST");
            HttpResponseMessage response = _httpClient.PostAsync(postUrl, httpContent)
                .GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            return $"请求失败：{ex.Message}";
        }
    }

    /// <summary>
    /// HttpPost接口（异步）
    /// </summary>
    /// <param name="postUrl">请求地址</param>
    /// <param name="requestData">请求参数</param>
    /// <returns></returns>
    public static async Task<string> HttpClientPostAsync(string postUrl, object requestData)
    {
        try
        {
            using HttpContent httpContent = JsonContent.Create(requestData);
            httpContent.Headers.Add("Method", "POST");
            HttpResponseMessage response = await _httpClient.PostAsync(postUrl, httpContent);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            return $"请求失败：{ex.Message}";
        }
    }

    /// <summary>
    /// HttpGet接口（同步）
    /// </summary>
    /// <param name="getUrl">请求地址</param>
    /// <returns></returns>
    public static string HttpClientGet(string getUrl)
    {
        try
        {
            HttpResponseMessage response = _httpClient.GetAsync(getUrl).GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            return $"请求失败：{ex.Message}";
        }
    }

    /// <summary>
    /// HttpGet接口（异步）
    /// </summary>
    /// <param name="getUrl">请求地址</param>
    /// <returns></returns>
    public static async Task<string> HttpClientGetAsync(string getUrl)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(getUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            return $"请求失败：{ex.Message}";
        }
    }
}

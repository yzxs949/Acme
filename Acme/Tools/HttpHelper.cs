using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace Acme.Tools;

/// <summary>
/// HTTP 请求通用类
/// </summary>
public static class HttpHelper
{
    /// <summary>
    /// 全局共享的 HttpClient 实例（线程安全，复用连接）
    /// </summary>
    private static readonly HttpClient _httpClient = new(new SocketsHttpHandler
    {
        // 定期重建连接，感知 DNS 变更，避免长期运行服务使用过期地址
        PooledConnectionLifetime = TimeSpan.FromMinutes(5),
        // 建立连接超时
        ConnectTimeout = TimeSpan.FromSeconds(10)
    })
    {
        // 单次请求整体超时
        Timeout = TimeSpan.FromSeconds(30)
    };

    /// <summary>
    /// 异步 POST 请求，请求体按 JSON 序列化（Newtonsoft.Json）
    /// </summary>
    /// <param name="postUrl">请求地址</param>
    /// <param name="requestData">请求参数（自动序列化为 JSON）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容字符串；非 2xx 状态码或网络异常将抛出 <see cref="HttpRequestException"/></returns>
    public static async Task<string> HttpClientPostAsync(
        string postUrl,
        object requestData,
        CancellationToken cancellationToken = default)
    {
        using HttpResponseMessage response = await _httpClient
            .PostAsync(postUrl, new StringContent(requestData.ToJson(), Encoding.UTF8, "application/json"), cancellationToken)
            .ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// 异步 GET 请求
    /// </summary>
    /// <param name="getUrl">请求地址</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容字符串；非 2xx 状态码或网络异常将抛出 <see cref="HttpRequestException"/></returns>
    public static async Task<string> HttpClientGetAsync(
        string getUrl,
        CancellationToken cancellationToken = default)
    {
        using HttpResponseMessage response = await _httpClient
            .GetAsync(getUrl, cancellationToken)
            .ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// 同步 POST 请求（兼容旧版，建议优先使用异步版本）
    /// </summary>
    /// <param name="postUrl">请求地址</param>
    /// <param name="requestData">请求参数（自动序列化为 JSON）</param>
    /// <returns>响应内容字符串；失败时抛出 <see cref="HttpRequestException"/></returns>
    public static string HttpClientPost(string postUrl, object requestData)
        => HttpClientPostAsync(postUrl, requestData).GetAwaiter().GetResult();

    /// <summary>
    /// 同步 GET 请求（兼容旧版，建议优先使用异步版本）
    /// </summary>
    /// <param name="getUrl">请求地址</param>
    /// <returns>响应内容字符串；失败时抛出 <see cref="HttpRequestException"/></returns>
    public static string HttpClientGet(string getUrl)
        => HttpClientGetAsync(getUrl).GetAwaiter().GetResult();
}

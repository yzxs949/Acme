using Microsoft.AspNetCore.Http;
using System;
namespace Acme.Cookie;

/// <summary>
/// Cookie通用类
/// </summary>
public class HttpCookieService : IHttpCookieService
{
    /// <summary>
    /// 依赖注入项
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    public HttpCookieService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// 设置本地cookie
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <param name="minutes">过期时长，单位：分钟</param>
    /// <param name="httpOnly">是否设置为 HttpOnly（默认 true，防止前端脚本读取，提升安全性）</param>
    public void SetCookies(string key, string value, int minutes = 300, bool httpOnly = true)
    {
        _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, new CookieOptions
        {
            Expires = DateTime.Now.AddMinutes(minutes),
            // 默认开启 HttpOnly，避免前端脚本（XSS）窃取 Cookie
            HttpOnly = httpOnly,
            // 限制跨站发送，缓解 CSRF
            SameSite = SameSiteMode.Lax
        });
    }

    /// <summary>
    /// 删除指定的cookie
    /// </summary>
    /// <param name="key">键</param>
    public void DeleteCookies(string key)
    {
        _httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
    }

    /// <summary>
    /// 获取cookies
    /// </summary>
    /// <param name="key">键</param>
    /// <returns>返回对应的值</returns>
    public string GetCookies(string key)
    {
        _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(key, out string value);
        if (string.IsNullOrEmpty(value))
            value = string.Empty;
        return value;
    }
}

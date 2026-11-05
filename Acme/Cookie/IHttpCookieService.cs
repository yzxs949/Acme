namespace Acme.Cookie;

/// <summary>
/// HttpCookie接口
/// </summary>
public interface IHttpCookieService
{
    /// <summary>
    /// 设置Cookies
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <param name="minutes">到期时间（分钟）</param>
    /// <param name="httpOnly">是否设置为 HttpOnly（默认 true，防止前端脚本读取，提升安全性）</param>
    void SetCookies(string key, string value, int minutes = 300, bool httpOnly = true);

    /// <summary>
    /// 删除指定的cookie
    /// </summary>
    /// <param name="key">键</param>
    void DeleteCookies(string key);

    /// <summary>
    /// 获取cookies
    /// </summary>
    /// <param name="key">键</param>
    /// <returns>返回对应的值</returns>
    string GetCookies(string key);
}

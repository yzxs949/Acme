using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme;

/// <summary>
/// Session服务通用类
/// </summary>
public class HttpSessionService : IHttpSessionService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    public HttpSessionService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// 设置Session
    /// </summary>
    /// <param name="Key">键</param>
    /// <param name="Values">值</param>
    public void SetSession(string Key, string Values)
    {
        _httpContextAccessor.HttpContext.Session.SetString(Key, Values);
    }

    /// <summary>
    /// 获取Session
    /// </summary>
    /// <param name="Key">键</param>
    /// <returns></returns>
    public string GetSession(string Key)
    {
        return _httpContextAccessor.HttpContext.Session.GetString(Key);
    }

    /// <summary>
    /// 设置对象Session
    /// </summary>
    /// <param name="Key">键</param>
    /// <param name="Values">值</param>
    public void SetObjectAsJson(string Key, object Values)
    {
        _httpContextAccessor.HttpContext.Session.SetString(Key, Values.ToJson());
    }

    /// <summary>
    /// 获取对象Session
    /// </summary>
    /// <typeparam name="T">Session值类型</typeparam>
    /// <param name="key">键</param>
    /// <returns></returns>
    public T GetObjectFromJson<T>(string key)
    {
        string strJson = _httpContextAccessor.HttpContext.Session.GetString(key);
        return strJson.IsNotNull() ? default : strJson.ToEntity<T>();
    }

    /// <summary>
    /// 移除
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    public void RemoveSession(string key)
    {
        _httpContextAccessor.HttpContext.Session.Remove(key);
    }
}

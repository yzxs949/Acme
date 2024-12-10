namespace Acme.Sessions;

/// <summary>
/// Session接口服务
/// </summary>
public interface IHttpSessionService
{
    /// <summary>
    /// 设置Session
    /// </summary>
    /// <param name="Key">键</param>
    /// <param name="Values">值</param>
    void SetSession(string Key, string Values);

    /// <summary>
    /// 获取Session
    /// </summary>
    /// <param name="Key">键</param>
    /// <returns></returns>
    string GetSession(string Key);

    /// <summary>
    /// 设置对象Session
    /// </summary>
    /// <param name="Key">键</param>
    /// <param name="Values">值</param>
    void SetObjectAsJson(string Key, object Values);

    /// <summary>
    /// 获取对象Session
    /// </summary>
    /// <typeparam name="T">Session值类型</typeparam>
    /// <param name="key">键</param>
    /// <returns></returns>
    T GetObjectFromJson<T>(string key);

    /// <summary>
    /// 移除
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    void RemoveSession(string key);
}

using System;
using System.IO;
using Acme.JsonFiles;

namespace Acme.Apps;


/// <summary>
/// 系统配置参数
/// </summary>
public static class App
{
    private static readonly JsonFile JsonFileVariable;

    /// <summary>
    /// 构造函数
    /// </summary>
    static App()
    {
        JsonFileVariable = new("appsettings.json");
    }

    /// <summary>
    /// 获取配置文件中的值
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="name">名称</param>
    /// <returns></returns>
    public static string GetValue(string key, string name)
        => JsonFileVariable.GetValue(key, name);

    /// <summary>
    /// 获取配置文件中的值
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    public static string GetValue(string key)
        => JsonFileVariable.GetValue(key);

    /// <summary>
    /// 系统路径
    /// </summary>
    public static string SystemPath => Directory.GetCurrentDirectory();

    /// <summary>
    /// 系统路径
    /// </summary>
    public static string Path => AppDomain.CurrentDomain.BaseDirectory;
}
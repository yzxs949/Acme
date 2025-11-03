``` cs
using System;
using System.IO;
using Acme.JsonFiles;

namespace Acme.Apps;


/// <summary>
/// 系统配置参数
/// </summary>
public static class App
{
    private static JsonFile JsonFileVariable { get; set; }

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
    {
        return JsonFileVariable.GetValue(key, name);
    }

    /// <summary>
    /// 获取配置文件中的值
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    public static string GetValue(string key)
    {
        return JsonFileVariable.GetValue(key);
    }

    /// <summary>
    /// 系统路径
    /// </summary>
    public static string SystemPath => Directory.GetCurrentDirectory();

    /// <summary>
    /// 系统路径
    /// </summary>
    public static string Path => AppDomain.CurrentDomain.BaseDirectory;
}
```
using NLog;

namespace Acme.Nlogs;

/// <summary>
/// 日志配置类
/// </summary>
public class LogConfig
{
    /// <summary>
    /// 是否开启控制台日志
    /// </summary>
    public bool IsConsoleLogEnabled { get; set; }

    /// <summary>
    /// 是否开启文件日志
    /// </summary>
    public bool IsFileLogEnabled { get; set; }

    /// <summary>
    /// 日志注入项
    /// </summary>
    public Logger Logger => LogManager.GetCurrentClassLogger();

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="isConsoleLogEnabled">是否开启控制台日志</param>
    /// <param name="isFileLogEnabled">是否开启文件日志</param>
    public LogConfig(bool isFileLogEnabled = true, bool isConsoleLogEnabled = true)
    {
        IsFileLogEnabled = isFileLogEnabled;
        IsConsoleLogEnabled = isConsoleLogEnabled;
    }

    /// <summary>
    /// 日志配置是否为null
    /// </summary>
    /// <returns></returns>
    public bool IsNull()
    {
        return this is null;
    }
}

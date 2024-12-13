using NLog;

namespace Acme.Nlogs;

/// <summary>
/// 日志配置类
/// </summary>
/// <remarks>
/// 构造函数
/// </remarks>
/// <param name="isConsoleLogEnabled">是否开启控制台日志</param>
/// <param name="isFileLogEnabled">是否开启文件日志</param>
public class LogConfig(bool isFileLogEnabled = true, bool isConsoleLogEnabled = true)
{
    /// <summary>
    /// 是否开启控制台日志
    /// </summary>
    public bool IsConsoleLogEnabled { get; set; } = isConsoleLogEnabled;

    /// <summary>
    /// 是否开启文件日志
    /// </summary>
    public bool IsFileLogEnabled { get; set; } = isFileLogEnabled;

    /// <summary>
    /// 日志注入项
    /// </summary>
    public static Logger Logger => LogManager.GetCurrentClassLogger();

    /// <summary>
    /// 日志配置是否为null
    /// </summary>
    /// <returns></returns>
    public bool IsNull()
    {
        return this is null;
    }
}

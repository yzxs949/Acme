using System;
using Acme.Ohs;
using Microsoft.Extensions.Logging;

namespace Acme.Nlogs;
/// <summary>
/// 日志处理类
/// </summary>
/// <remarks>
/// 构造函数
/// </remarks>
/// <param name="logger"></param>
/// <param name="logConfig"></param>
public class Log<T>(ILogger<T> logger, LogConfig logConfig) : ILog<T>
{
    /// <summary>
    /// 依赖注入
    /// </summary>
    public readonly LogConfig _logConfig = logConfig;
    private readonly ILogger<T> _logger = logger;

    /// <summary>
    /// 创建日志配置
    /// </summary>
    /// <param name="isConsoleLogEnabled">是否开启控制台日志</param>
    /// <param name="isFileLogEnabled">是否开启文件日志</param>
    public static LogConfig CreateLogConfig(bool isFileLogEnabled = true, bool isConsoleLogEnabled = true)
    {
        return new LogConfig(isFileLogEnabled, isConsoleLogEnabled);
    }

    /// <summary>
    /// 跟踪日志
    /// </summary>
    /// <param name="message"></param>
    public void Trace(string message)
    {
        if (_logConfig.IsNull())
            throw Oh.OhNo("您未注入日志配置项");
        string dateime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        if (_logConfig.IsConsoleLogEnabled)
            _logger.LogTrace("{dateime}\t{message}", dateime, message);
        if (_logConfig.IsFileLogEnabled)
            LogConfig.Logger.Trace($"{dateime}\t{message}");
    }
    /// <summary>
    /// 调试日志
    /// </summary>
    /// <param name="message"></param>
    public void Debug(string message)
    {
        if (_logConfig.IsNull())
            throw Oh.OhNo("您未注入日志配置项");
        string dateime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        if (_logConfig.IsConsoleLogEnabled)
            _logger.LogDebug("{dateime}\t{message}", dateime, message);
        if (_logConfig.IsFileLogEnabled)
            LogConfig.Logger.Debug($"{dateime}\t{message}");
    }
    /// <summary>
    /// 信息日志
    /// </summary>
    /// <param name="message"></param>
    public void Info(string message)
    {
        if (_logConfig.IsNull())
            throw Oh.OhNo("您未注入日志配置项");
        string dateime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        if (_logConfig.IsConsoleLogEnabled)
            _logger.LogInformation("{dateime}\t{message}", dateime, message);
        if (_logConfig.IsFileLogEnabled)
            LogConfig.Logger.Info($"{dateime}\t{message}");
    }
    /// <summary>
    /// 警告日志
    /// </summary>
    /// <param name="message"></param>
    public void Warn(string message)
    {
        if (_logConfig.IsNull())
            throw Oh.OhNo("您未注入日志配置项");
        string dateime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        if (_logConfig.IsConsoleLogEnabled)
            _logger.LogWarning("{dateime}\t{message}", dateime, message);
        if (_logConfig.IsFileLogEnabled)
            LogConfig.Logger.Warn($"{dateime}\t{message}");
    }
    /// <summary>
    /// 错误日志
    /// </summary>
    /// <param name="message"></param>
    public void Error(string message)
    {
        if (_logConfig.IsNull())
            throw Oh.OhNo("您未注入日志配置项");
        string dateime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        if (_logConfig.IsConsoleLogEnabled)
            _logger.LogError("{dateime}\t{message}", dateime, message);
        if (_logConfig.IsFileLogEnabled)
            LogConfig.Logger.Error($"{dateime}\t{message}");

    }
    /// <summary>
    /// 严重错误日志
    /// </summary>
    /// <param name="message"></param>
    public void Fatal(string message)
    {
        if (_logConfig.IsNull())
            throw Oh.OhNo("您未注入日志配置项");
        string dateime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        if (_logConfig.IsConsoleLogEnabled)
            _logger.LogError("{dateime}\t{message}", dateime, message);
        if (_logConfig.IsFileLogEnabled)
            LogConfig.Logger.Fatal($"{dateime}\t{message}");
    }
}
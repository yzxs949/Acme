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
        string printMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\t{message}";
        if (_logConfig.IsConsoleLogEnabled)
            _logger.LogTrace(printMessage);
        if (_logConfig.IsFileLogEnabled)
            _logConfig.Logger.Trace(printMessage);

    }
    /// <summary>
    /// 调试日志
    /// </summary>
    /// <param name="message"></param>
    public void Debug(string message)
    {
        if (_logConfig.IsNull())
            throw Oh.OhNo("您未注入日志配置项");
        string printMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\t{message}";
        if (_logConfig.IsConsoleLogEnabled)
            _logger.LogDebug(printMessage);
        if (_logConfig.IsFileLogEnabled)
            _logConfig.Logger.Debug(printMessage);
    }
    /// <summary>
    /// 信息日志
    /// </summary>
    /// <param name="message"></param>
    public void Info(string message)
    {
        if (_logConfig.IsNull())
            throw Oh.OhNo("您未注入日志配置项");
        string printMessage = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\t{message}";
        if (_logConfig.IsConsoleLogEnabled)
            _logger.LogInformation(printMessage);
        if (_logConfig.IsFileLogEnabled)
            _logConfig.Logger.Info(printMessage);

    }
    /// <summary>
    /// 警告日志
    /// </summary>
    /// <param name="message"></param>
    public void Warn(string message)
    {
        if (_logConfig.IsNull())
            throw Oh.OhNo("您未注入日志配置项");
        string printMessage = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\t{message}";
        if (_logConfig.IsConsoleLogEnabled)
            _logger.LogWarning(printMessage);
        if (_logConfig.IsFileLogEnabled)
            _logConfig.Logger.Warn(printMessage);
    }
    /// <summary>
    /// 错误日志
    /// </summary>
    /// <param name="message"></param>
    public void Error(string message)
    {
        if (_logConfig.IsNull())
            throw Oh.OhNo("您未注入日志配置项");
        string printMessage = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\t{message}";
        if (_logConfig.IsConsoleLogEnabled)
            _logger.LogError(printMessage);
        if (_logConfig.IsFileLogEnabled)
            _logConfig.Logger.Error(printMessage);

    }
    /// <summary>
    /// 严重错误日志
    /// </summary>
    /// <param name="message"></param>
    public void Fatal(string message)
    {
        if (_logConfig.IsNull())
            throw Oh.OhNo("您未注入日志配置项");
        string printMessage = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\t{message}";
        if (_logConfig.IsConsoleLogEnabled)
        {
            string message1 = $"严重错误：{printMessage}";
            _logger.LogError(message1, (object[])null);
        }

        if (_logConfig.IsFileLogEnabled)
            _logConfig.Logger.Fatal(printMessage);
    }
}
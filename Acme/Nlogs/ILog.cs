namespace Acme.Nlogs;

/// <summary>
/// 日志处理接口
/// </summary>
public interface ILog<T>
{
    /// <summary>
    /// 跟踪日志
    /// </summary>
    /// <param name="message"></param>
    void Trace(string message);

    /// <summary>
    /// 调试日志
    /// </summary>
    /// <param name="message"></param>
    void Debug(string message);

    /// <summary>
    /// 信息日志
    /// </summary>
    /// <param name="message"></param>
    void Info(string message);

    /// <summary>
    /// 警告日志
    /// </summary>
    /// <param name="message"></param>
    void Warn(string message);

    /// <summary>
    /// 错误日志
    /// </summary>
    /// <param name="message"></param>
    void Error(string message);

    /// <summary>
    /// 严重错误日志
    /// </summary>
    /// <param name="message"></param>
    void Fatal(string message);
}

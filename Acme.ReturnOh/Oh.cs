namespace Acme.ReturnOh;

/// <summary>
/// 通用返回参数实体
/// </summary>
/// <typeparam name="TEntity">数据类型</typeparam>
/// <param name="Code">编码</param>
/// <param name="Msg">描述</param>
/// <param name="Data">数据</param>
public record Oh<TEntity>(int Code, string Msg, TEntity? Data) : IOh where TEntity : class
{
    /// <summary>
    /// 实例化Oh
    /// </summary>
    /// <param name="code">代码</param>
    /// <param name="msg">描述</param>
    /// <param name="data">数据</param>
    public static Oh<TEntity> NewOh(int code, string msg, TEntity data)
    {
        return new Oh<TEntity>(code, msg, data);
    }

    /// <summary>
    /// 异常处理方法
    /// </summary>
    /// <param name="msg"></param>
    /// <exception cref="T:System.Exception">异常类</exception>
    public static void Exception(string msg)
    {
        throw new Exception(msg);
    }

    /// <summary>
    /// 抛出参数异常
    /// </summary>
    /// <param name="msg">异常消息</param>
    /// <exception cref="T:System.ArgumentException">参数异常</exception>
    public static void ArgumentException(string msg)
    {
        throw new ArgumentException(msg);
    }

    /// <summary>
    /// 抛出系统异常
    /// </summary>
    /// <param name="msg">异常消息</param>
    /// <exception cref="T:System.SystemException">系统异常</exception>
    public static void SystemException(string msg)
    {
        throw new SystemException(msg);
    }

    /// <summary>
    /// 获取成功时的Oh值(data为null时)
    /// </summary>
    /// <param name="msg">描述</param>
    /// <returns></returns>
    public static Oh Success(string msg) => CV.NullValue.Success(msg);

    /// <summary>
    /// 获取失败时的Oh值(data为null时)
    /// </summary>
    /// <param name="msg">描述</param>
    /// <returns></returns>
    public static Oh Fail(string msg) => CV.NullValue.Fail(msg);
}

/// <summary>
/// 通用返回参数实体
/// </summary>
/// <param name="Code">编码</param>
/// <param name="Msg">描述</param>
/// <param name="Data">数据</param>
public record Oh(int Code, string Msg, object? Data = null) : Oh<object>(Code, Msg, Data), IOh;

/// <summary>
/// 通用返回参数实体接口
/// </summary>
public interface IOh
{

}

namespace Acme.ReturnOh;

/// <summary>
/// Oh拓展类
/// </summary>
public static class OhExtension
{
    /// <summary>
    /// 获取成功时的Oh值（默认状态码200）
    /// </summary>
    /// <param name="data">数据</param>
    /// <param name="msg">描述</param>
    /// <returns></returns>
    public static Oh Success(this object data, string msg = "成功！")
    {
        return new Oh(CV.YesCode, msg, data);
    }

    /// <summary>
    /// 获取成功时的Oh泛型值（默认状态码200）
    /// </summary>
    /// <param name="data">数据</param>
    /// <param name="msg">描述</param>
    /// <returns></returns>
    public static Oh<TEntity> Success<TEntity>(this TEntity data, string msg = "成功！") where TEntity : class
    {
        return new Oh<TEntity>(CV.YesCode, msg, data);
    }

    /// <summary>
    /// 获取成功时的Oh值（自定义状态码和消息）
    /// </summary>
    /// <param name="data">数据</param>
    /// <param name="code">状态码</param>
    /// <param name="msg">描述</param>
    /// <returns></returns>
    public static Oh Success(this object data, int code, string msg)
    {
        return new Oh(code, msg, data);
    }

    /// <summary>
    /// 获取成功时的Oh泛型值（自定义状态码和消息）
    /// </summary>
    /// <param name="data">数据</param>
    /// <param name="code">状态码</param>
    /// <param name="msg">描述</param>
    /// <returns></returns>
    public static Oh<TEntity> Success<TEntity>(this TEntity data, int code, string msg) where TEntity : class
    {
        return new Oh<TEntity>(code, msg, data);
    }

    /// <summary>
    /// 获取失败时的Oh值（默认状态码500）
    /// </summary>
    /// <param name="data">数据</param>
    /// <param name="msg">描述</param>
    /// <returns></returns>
    public static Oh Fail(this object data, string msg = "服务器错误！请联系管理员")
    {
        return new Oh(CV.NoCode, msg, data);
    }

    /// <summary>
    /// 获取失败时的Oh泛型值（默认状态码500）
    /// </summary>
    /// <param name="data">数据</param>
    /// <param name="msg">描述</param>
    /// <returns></returns>
    public static Oh<TEntity> Fail<TEntity>(this TEntity data, string msg = "服务器错误！请联系管理员") where TEntity : class
    {
        return new Oh<TEntity>(CV.NoCode, msg, data);
    }

    /// <summary>
    /// 获取失败时的Oh值（自定义状态码和消息）
    /// </summary>
    /// <param name="data">数据</param>
    /// <param name="code">状态码</param>
    /// <param name="msg">描述</param>
    /// <returns></returns>
    public static Oh Fail(this object data, int code, string msg)
    {
        return new Oh(code, msg, data);
    }

    /// <summary>
    /// 获取失败时的Oh泛型值（自定义状态码和消息）
    /// </summary>
    /// <param name="data">数据</param>
    /// <param name="code">状态码</param>
    /// <param name="msg">描述</param>
    /// <returns></returns>
    public static Oh<TEntity> Fail<TEntity>(this TEntity data, int code, string msg) where TEntity : class
    {
        return new Oh<TEntity>(code, msg, data);
    }

    /// <summary>
    /// 扩展方法，用于创建一个自定义的 Oh 实例，包含指定的数据、状态码和消息。
    /// </summary>
    /// <param name="data">数据对象。</param>
    /// <param name="code">状态码。</param>
    /// <param name="msg">消息。</param>
    /// <returns>包含指定数据、状态码和消息的 Oh 实例。</returns>
    /// <remarks>
    /// 此方法用于创建一个自定义的 Oh 实例，允许用户指定任意类型的数据、状态码和消息，用于特定的业务场景。
    /// </remarks>
    public static Oh Custom(this object data, int code, string msg)
    {
        return new Oh(code, msg, data);
    }

    /// <summary>
    /// 扩展方法，用于创建一个自定义的 Oh 实例，包含指定的实体数据、状态码和消息。
    /// </summary>
    /// <typeparam name="TEntity">实体类型。</typeparam>
    /// <param name="data">实体数据。</param>
    /// <param name="code">状态码。</param>
    /// <param name="msg">消息。</param>
    /// <returns>包含指定实体数据、状态码和消息的 Oh 实例。</returns>
    /// <remarks>
    /// 此方法用于创建一个自定义的 Oh 实例，允许用户指定实体数据、状态码和消息，用于特定的业务场景。
    /// 实体类型必须是引用类型（class），并且必须具有无参数的公共构造函数。
    /// </remarks>
    public static Oh<TEntity> Custom<TEntity>(this TEntity data, int code, string msg) where TEntity : class
    {
        return new Oh<TEntity>(code, msg, data);
    }

    /// <summary>
    /// 扩展方法，用于将布尔值转换为表示操作成功或失败的 Oh 实例。
    /// </summary>
    /// <param name="isSuccess">布尔值，指示操作是否成功。</param>
    /// <param name="successMsg">成功时的描述</param>
    /// <param name="failMsg">失败时的描述</param>
    /// <returns>表示操作成功或失败的 Oh 实例。</returns>
    public static Oh IsSuccess(this bool isSuccess, string successMsg, string failMsg)
    {
        return isSuccess ? CV.NullValue.Success(successMsg) : CV.NullValue.Fail(failMsg);
    }
}

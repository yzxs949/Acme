using System;

namespace Acme;

/// <summary>
/// 通用返回参数实体
/// </summary>
public class Oh<TEntity> where TEntity : class, new()
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="code">代码</param>
    /// <param name="msg">描述</param>
    /// <param name="data">数据</param>
    public Oh(
        int code = CV.YesCode,
        string msg = CV.BlankCharacter,
        TEntity data = null)
    {
        this.Code = code;
        this.Msg = msg;
        this.Data = data;
    }

    /// <summary>
    /// 实例化Oh
    /// </summary>
    /// <param name="code">代码</param>
    /// <param name="msg">描述</param>
    /// <param name="data">数据</param>
    public static Oh<TEntity> NewOh(
        int code = CV.YesCode,
        string msg = CV.BlankCharacter,
        TEntity data = null)
    {
        return new Oh<TEntity>(code, msg, data);
    }

    /// <summary>
    /// 异常处理方法
    /// </summary>
    /// <param name="msg"></param>
    /// <exception cref="Exception">异常类</exception>
    public static Exception OhNo(string msg)
    {
        return new Exception(msg);
    }

    /// <summary>
    /// 代码
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Msg { get; set; }

    /// <summary>
    /// 数据
    /// </summary>
    public TEntity Data { get; set; }
}

/// <summary>
/// 通用返回参数实体
/// </summary>
public class Oh : Oh<object>
{

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="code">代码</param>
    /// <param name="msg">描述</param>
    /// <param name="data">数据</param>
    public Oh(
        int code = CV.YesCode,
        string msg = CV.BlankCharacter,
        object data = null)
    {
        Code = code;
        Msg = msg;
        Data = data;
    }
}

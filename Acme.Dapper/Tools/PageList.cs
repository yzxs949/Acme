using System.Collections.Generic;

namespace Acme.Dapper.Tools;

/// <summary>
/// 分页返回结果
/// </summary>
/// <typeparam name="T">集合元素类型</typeparam>
public class PageList<T>
{
    /// <summary>
    /// 总条数
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// 页码（从 1 开始）
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// 每页大小
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// 集合数据（永不为 null）
    /// </summary>
    public List<T> Items { get; set; } = new();

    /// <summary>
    /// 总页数
    /// </summary>
    public int TotalPage => PageSize <= 0 ? 0 : (Total + PageSize - 1) / PageSize;

    /// <summary>
    /// 是否存在上一页
    /// </summary>
    public bool HasPrev => PageIndex > 1;

    /// <summary>
    /// 是否存在下一页
    /// </summary>
    public bool HasNext => PageIndex < TotalPage;
}

/// <summary>
/// 分页返回结果
/// </summary>
public class PageList
{
    /// <summary>
    /// 总条数
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// 页码（从 1 开始）
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// 每页大小
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// 集合数据（永不为 null）
    /// </summary>
    public List<object> Items { get; set; } = new();

    /// <summary>
    /// 总页数
    /// </summary>
    public int TotalPage => PageSize <= 0 ? 0 : (Total + PageSize - 1) / PageSize;

    /// <summary>
    /// 是否存在上一页
    /// </summary>
    public bool HasPrev => PageIndex > 1;

    /// <summary>
    /// 是否存在下一页
    /// </summary>
    public bool HasNext => PageIndex < TotalPage;
}

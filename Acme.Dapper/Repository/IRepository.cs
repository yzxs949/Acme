using Acme.Dapper.Enums;
using Acme.Dapper.Tools;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace Acme.Dapper.Repository;

/// <summary>
/// Dapper拓展仓储实现接口
/// </summary>
/// <typeparam name="T">实体类型</typeparam>
public interface IRepository<T>
{
    #region 新增
    /// <summary>
    /// 异步新增返回Id
    /// </summary>
    /// <param name="t">实体</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>新增项的Id</returns>
    Task<int> AddReturnIdAsync(T t, CancellationToken cancellationToken = default);

    /// <summary>
    /// 新增返回Id
    /// </summary>
    /// <param name="t">实体</param>
    /// <returns>新增项的Id</returns>
    int AddReturnId(T t);

    /// <summary>
    /// 异步新增
    /// </summary>
    /// <param name="t">实体</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<int> AddAsync(T t, CancellationToken cancellationToken = default);

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="t">实体</param>
    /// <returns></returns>
    int Add(T t);

    #endregion

    #region 删除
    /// <summary>
    /// 异步删除实体
    /// </summary>
    /// <param name="t">实体</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<bool> DeleteAsync(T t, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据Id删除
    /// </summary>
    /// <param name="Id">主键Id</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<bool> DeleteAsync(int Id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 删除实体
    /// </summary>
    /// <param name="t">实体</param>
    /// <returns></returns>
    bool Delete(T t);

    /// <summary>
    /// 根据Id删除
    /// </summary>
    /// <param name="Id">主键Id</param>
    /// <returns></returns>
    bool Delete(int Id);

    #endregion

    #region 修改
    /// <summary>
    /// 异步修改返回Id
    /// </summary>
    /// <param name="t">实体</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>被修改项的Id</returns>
    Task<int> UpdateReturnIdAsync(T t, CancellationToken cancellationToken = default);

    /// <summary>
    /// 修改返回Id
    /// </summary>
    /// <param name="t">实体</param>
    /// <returns>被修改项的Id</returns>
    int UpdateReturnId(T t);

    /// <summary>
    /// 异步修改
    /// </summary>
    /// <param name="t">实体</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<bool> UpdateAsync(T t, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据Id修改实体
    /// </summary>
    /// <param name="UpdateContext">更新内容（须包含主键 Id）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<int> UpdateByIdAsync(object UpdateContext, CancellationToken cancellationToken = default);

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="t">实体</param>
    /// <returns></returns>
    bool Update(T t);
    #endregion

    #region 获取单条
    /// <summary>
    /// 根据Id获取单条，无数据时抛出异常
    /// </summary>
    /// <param name="Id">主键Id</param>
    /// <returns></returns>
    T GetInfo(int Id);

    /// <summary>
    /// 根据Id获取单条，无数据时返回默认值
    /// </summary>
    /// <param name="Id">主键Id</param>
    /// <returns></returns>
    T GetInfoOrDefault(int Id);

    /// <summary>
    /// 按条件获取单条，无数据时抛出异常
    /// </summary>
    /// <param name="SqlWhere">条件</param>
    /// <returns></returns>
    T GetInfo(object SqlWhere);

    /// <summary>
    /// 按条件获取单条，无数据时返回默认值
    /// </summary>
    /// <param name="SqlWhere">条件</param>
    /// <returns></returns>
    T GetInfoOrDefault(object SqlWhere);

    /// <summary>
    /// 根据Id获取单条数据异步返回，无数据时抛出异常
    /// </summary>
    /// <param name="Id">主键Id</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<T> GetInfoAsync(int Id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据Id获取单条数据异步返回，无数据时返回默认值
    /// </summary>
    /// <param name="Id">主键Id</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<T> GetInfoOrDefaultAsync(int Id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 按条件获取单条，无数据时抛出异常
    /// </summary>
    /// <param name="SqlWhere">条件</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<T> GetInfoAsync(object SqlWhere, CancellationToken cancellationToken = default);

    /// <summary>
    /// 按条件获取单条，无数据时返回默认值
    /// </summary>
    /// <param name="SqlWhere">条件</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<T> GetInfoOrDefaultAsync(object SqlWhere, CancellationToken cancellationToken = default);
    #endregion

    #region 列表与分页查询
    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="SqlWhere">条件</param>
    /// <returns></returns>
    List<T> GetList(object SqlWhere);

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="SqlWhere">条件</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<List<T>> GetListAsync(object SqlWhere, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取条数
    /// </summary>
    /// <param name="SqlWhere">条件</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<int> GetCountAsync(object SqlWhere, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据sql获取分页数据
    /// </summary>
    /// <param name="sql">sql语句</param>
    /// <param name="pageIndex">页码（从 1 开始）</param>
    /// <param name="pageSize">每页显示的条数</param>
    /// <param name="orderField">排序字段</param>
    /// <param name="orderType">排序类型</param>
    /// <returns></returns>
    PageList GetPageListBySql(
        string sql,
        int pageIndex,
        int pageSize,
        string orderField = "",
        OrderType orderType = OrderType.ASC);

    /// <summary>
    /// 根据sql获取分页数据
    /// </summary>
    /// <param name="sql">sql语句</param>
    /// <param name="pageIndex">页码（从 1 开始）</param>
    /// <param name="pageSize">每页显示的条数</param>
    /// <param name="orderField">排序字段</param>
    /// <param name="orderType">排序类型</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<PageList> GetPageListBySqlAsync(
        string sql,
        int pageIndex,
        int pageSize,
        string orderField = "",
        OrderType orderType = OrderType.ASC,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据sql获取分页数据
    /// </summary>
    /// <param name="sql">sql语句</param>
    /// <param name="pageIndex">页码（从 1 开始）</param>
    /// <param name="pageSize">每页显示的条数</param>
    /// <param name="orderField">排序字段</param>
    /// <param name="orderType">排序类型</param>
    /// <typeparam name="TEntity">结果实体类型</typeparam>
    /// <returns></returns>
    PageList<TEntity> GetPageListBySql<TEntity>(
        string sql,
        int pageIndex,
        int pageSize,
        string orderField = "",
        OrderType orderType = OrderType.ASC);

    /// <summary>
    /// 根据sql获取分页数据
    /// </summary>
    /// <param name="sql">sql语句</param>
    /// <param name="pageIndex">页码（从 1 开始）</param>
    /// <param name="pageSize">每页显示的条数</param>
    /// <param name="orderField">排序字段</param>
    /// <param name="orderType">排序类型</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <typeparam name="TEntity">结果实体类型</typeparam>
    /// <returns></returns>
    Task<PageList<TEntity>> GetPageListBySqlAsync<TEntity>(
        string sql,
        int pageIndex,
        int pageSize,
        string orderField = "",
        OrderType orderType = OrderType.ASC,
        CancellationToken cancellationToken = default);
    #endregion
}

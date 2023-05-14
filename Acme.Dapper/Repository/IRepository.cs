using System.Collections.Generic;
using System.Threading.Tasks;
namespace Acme.Dapper;

/// <summary>
/// Dapper拓展仓储实现接口
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IRepository<T>
{
    #region 新增
    /// <summary>
    /// 异步新增返回Id
    /// </summary>
    /// <param name="t">实体</param>
    /// <returns>新增项的Id</returns>
    Task<int> AddReturnIdAsync(T t);


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
    /// <returns></returns>
    Task<int> AddAsync(T t);

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
    /// <returns></returns>
    Task<bool> DeleteAsync(T t);

    /// <summary>
    /// 根据Id删除
    /// </summary>
    /// <param name="Id">主键Id</param>
    /// <returns></returns>
    Task<bool> DeleteAsync(int Id);

    /// <summary>
    /// 删除实体
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    bool Delete(T t);


    /// <summary>
    /// 删除实体
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
    /// <returns>新增项的Id</returns>
    Task<int> UpdateReturnIdAsync(T t);

    /// <summary>
    /// 修改返回Id
    /// </summary>
    /// <param name="t">实体</param>
    /// <returns>新增项的Id</returns>
    int UpdateReturnId(T t);

    /// <summary>
    /// 异步修改
    /// </summary>
    /// <param name="t">实体</param>
    /// <returns></returns>
    Task<bool> UpdateAsync(T t);

    /// <summary>
    /// 根据Id修改实体
    /// </summary>
    /// <param name="UpdateContext"></param>
    /// <returns></returns>
    Task<int> UpdateByIdAsync(object UpdateContext);

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="t">实体</param>
    /// <returns></returns>
    bool Update(T t);
    #endregion

    #region 获取单条
    /// <summary>
    /// 根据Id获取单条
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    T GetInfo(int Id);

    /// <summary>
    /// 按条件获取单条
    /// </summary>
    /// <param name="SqlWhere"></param>
    /// <returns></returns>
    T GetInfo(object SqlWhere);

    /// <summary>
    /// 根据Id获取单条数据异步返回
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    Task<T> GetInfoAsync(int Id);

    /// <summary>
    /// 根据条件
    /// </summary>
    /// <param name="SqlWhere"></param>
    /// <returns></returns>
    Task<T> GetInfoAsync(object SqlWhere);

    #endregion

    #region 分页查询
    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="SqlWhere"></param>
    /// <returns></returns>
    List<T> GetList(object SqlWhere);

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="SqlWhere">条件</param>
    /// <returns></returns>
    Task<List<T>> GetListAsync(object SqlWhere);

    /// <summary>
    /// 获取条数
    /// </summary>
    /// <param name="SqlWhere"></param>
    /// <returns></returns>
    Task<int> GetCountAsync(object SqlWhere);

    /// <summary>
    /// 根据sql获取分页数据
    /// </summary>
    /// <param name="sql">sql语句</param>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">每页显示的条数</param>
    /// <param name="orderField">排序字段</param>
    /// <param name="orderType">排序类型</param>
    /// <returns></returns>
    PageList<object> GetPageListBySql(
       string sql,
       int pageIndex,
       int pageSize,
       string orderField = "",
       OrderType orderType = OrderType.ASC);

    /// <summary>
    /// 根据sql获取分页数据
    /// </summary>
    /// <param name="sql">sql语句</param>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">每页显示的条数</param>
    /// <param name="orderField">排序字段</param>
    /// <param name="orderType">排序类型</param>
    /// <returns></returns>
    Task<PageList<object>> GetPageListBySqlAsync(
         string sql,
         int pageIndex,
         int pageSize,
         string orderField = "",
         OrderType orderType = OrderType.ASC);

    /// <summary>
    /// 根据sql获取分页数据
    /// </summary>
    /// <param name="sql">sql语句</param>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">每页显示的条数</param>
    /// <param name="orderField">排序字段</param>
    /// <param name="orderType">排序类型</param>
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
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">每页显示的条数</param>
    /// <param name="orderField">排序字段</param>
    /// <param name="orderType">排序类型</param>
    /// <returns></returns>
    Task<PageList<TEntity>> GetPageListBySqlAsync<TEntity>(
         string sql,
         int pageIndex,
         int pageSize,
         string orderField = "",
         OrderType orderType = OrderType.ASC);
    #endregion
}

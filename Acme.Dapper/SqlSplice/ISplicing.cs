using Acme.Dapper.Enums;

namespace Acme.Dapper.SqlSplice;

/// <summary>
/// 拓展实现接口
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ISplicing<T> where T : class, new()
{

    #region 生成不带参数的Sql语句
    /// <summary>
    /// 动态生成添加的sql语句
    /// </summary>
    /// <param name="t">待添加的实体</param>
    /// <param name="IsItKey">第一个字段是否是自增的主键（true是(默认是) false不是）</param>
    /// <returns>新增的sql语句</returns>
    string InsertSql(T t, bool IsItKey = true);

    /// <summary>
    /// 动态生成按条件删除的sql语句
    /// </summary>
    /// <param name="CondiEntity">条件实体 如：new {Id=1}</param>
    /// <returns>按条件删除的sql语句</returns>
    string DeleteSql(object CondiEntity);

    /// <summary>
    /// 动态生成按条件更新的sql语句
    /// </summary>
    /// <param name="t">待更新到数据库的实体</param>
    /// <param name="CondiEntity">更新条件（如：new {Id=1}）</param>
    /// <param name="IsItKey">第一个字段是否是自增的主键（true是 false不是）</param>
    /// <returns>按条件更新的sql语句</returns>
    string UpdateSql(T t, object CondiEntity, bool IsItKey = true);


    /// <summary>
    /// 生成查询的sql语句
    /// </summary>
    /// <param name="CondiEntity">条件实体</param>
    /// <returns>查询的sql语句</returns>
    string SelectSql(object CondiEntity);


    /// <summary>
    /// 动态生成按查询所有的sql语句
    /// </summary>
    /// <returns>查询所有的sql语句</returns>
    public string SelectSql();

    #endregion

    #region 生成带参数的sql语句
    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    /// <param name="IsItKey"></param>
    /// <returns></returns>
    string InsertSqlParam(T t, bool IsItKey = true);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="CondiEntity"></param>
    /// <returns></returns>
    string DeleteSqlParam(object CondiEntity);

    /// <summary>
    /// 动态生成按条件更新的sql语句
    /// </summary>
    /// <param name="t">待更新到数据库的实体</param>
    /// <param name="CondiEntity">更新条件（如：new {Id=1}）</param>
    /// <param name="IsItKey">第一个字段是否是自增的主键（true是 false不是）</param>
    /// <returns>按条件更新的sql语句</returns>
    string UpdateSqlParam(T t, object CondiEntity, bool IsItKey = true);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="UpdateContext"></param>
    /// <returns></returns>
    string UpdateSqlParam(object UpdateContext);

    /// <summary>
    /// 生成判断是否存在的Sql
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    string ExistenceSql(string sql);

    /// <summary>
    /// 动态生成按条件更新的sql语句
    /// </summary>
    /// <param name="t">实体</param>
    /// <returns></returns>
    string UpdateSqlParam(T t);


    /// <summary>
    /// 生成查询的sql语句
    /// </summary>
    /// <param name="CondiEntity">条件实体</param>
    /// <returns>查询的sql语句</returns>
    string SelectSqlParam(object CondiEntity);

    /// <summary>
    /// 查询条数的Sql语句
    /// </summary>
    /// <param name="CondiEntity"></param>
    /// <returns></returns>
    string SelectCountParam(object CondiEntity);

    /// <summary>
    /// 动态生成按查询所有的sql语句
    /// </summary>
    /// <returns>查询所有的sql语句</returns>
    string SelectSqlParam();

    #endregion

    /// <summary>
    /// 生成未确定查询结果的sql语句
    /// </summary>
    string SelectNoResultSql();

    /// <summary>
    /// 获取排序的sql语句
    /// </summary>
    /// <param name="sql">sql语句</param>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">每页显示的条数</param>
    /// <param name="orderField">排序字段</param>
    /// <param name="orderType">排序类型</param>
    /// <returns></returns>
    string GetPageSql(
        string sql,
        int pageIndex,
        int pageSize,
        string orderField = "",
        OrderType orderType = OrderType.ASC);

    /// <summary>
    /// 查询语句转化为获取条数Sql
    /// </summary>
    /// <param name="sql">sql语句</param>
    /// <returns></returns>
    string GetCountSql(string sql);
}

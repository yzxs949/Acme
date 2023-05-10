using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Dapper
{
    /// <summary>
    /// Dapper通用实现接口
    /// </summary>
    public interface IDapperHelper
    {
        #region 查询列表
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="sql">查询的sql</param>
        /// <param name="param">替换参数</param>
        /// <returns></returns>
        List<T> Query<T>(string sql, object param);

        /// <summary>
        /// 异步查询列表
        /// </summary>
        /// <param name="sql">查询的sql</param>
        /// <param name="param">替换参数</param>
        /// <returns></returns>
        Task<List<T>> QueryAsync<T>(string sql, object param);

        #endregion

        #region 查询第一个数据
        /// <summary>
        /// 查询第一个数据
        /// </summary>
        /// <param name="sql">查询的sql</param>
        /// <param name="param">替换参数</param>
        /// <returns></returns>
        T QueryFirst<T>(string sql, object param);

        /// <summary>
        /// 异步查询第一个数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<T> QueryFirstAsync<T>(string sql, object param);

        /// <summary>
        /// 查询第一个数据没有返回默认值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        T QueryFirstOrDefault<T>(string sql, object param);


        /// <summary>
        /// 查询第一个数据没有返回默认值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param);
        
        #endregion

        #region 查询单条数据
        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        T QuerySingle<T>(string sql, object param);

        /// <summary>
        /// 异步查询单条数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<T> QuerySingleAsync<T>(string sql, object param);

        /// <summary>
        /// 查询单条数据没有返回默认值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        T QuerySingleOrDefault<T>(string sql, object param);

        /// <summary>
        /// 查询单条数据没有返回默认值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param);

        #endregion

        #region 执行增删改语句
        /// <summary>
        /// 异步调用执行增删改语句
        /// </summary>
        /// <param name="sql">sql，参数</param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> ExecuteAsync(string sql, object param);


        /// <summary>
        /// 增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        int Execute(string sql, object param);

        #endregion

        #region 新增

        /// <summary>
        /// 根据实体类插入表（实体类需要加标记处理）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        Task<int> InsertAsync<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null) where T : class, new();


        /// <summary>
        /// 根据实体类插入表（实体类需要加标记处理）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        int Insert<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null) where T : class, new();

        #endregion

        #region 更新
        /// <summary>
        /// 根据实体类更新表（实体类需要加标记处理）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null) where T : class, new();


        /// <summary>
        /// 根据实体类更新表（实体类需要加标记处理）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public bool Update<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null) where T : class, new();

        #endregion

        #region 删除
        /// <summary>
        /// 根据实体类删除表（实体类需要加标记处理）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null) where T : class, new();


        /// <summary>
        /// 根据实体类删除表（实体类需要加标记处理）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        bool Delete<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null) where T : class, new();

        #endregion

        #region Reader获取数据
        /// <summary>
        /// Reader获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        IDataReader ExecuteReader(string sql, object param);

        /// <summary>
        /// 异步Reader获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IDataReader> ExecuteReaderAsync(string sql, object param);

        #endregion

        #region Scalar获取数据返回object
        /// <summary>
        /// Scalar获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        object ExecuteScalar(string sql, object param);


        /// <summary>
        /// Scalar获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<T> ExecuteScalarForTAsync<T>(string sql, object param);

        #endregion

        #region Scalar获取数据返回T
        /// <summary>
        /// Scalar获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        T ExecuteScalarForT<T>(string sql, object param);

        /// <summary>
        /// Scalar获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<T> ExecuteScalarAsync<T>(string sql, object param);

        #endregion

        #region 带参数的存储过程
        /// <summary>
        /// 带参数的存储过程
        /// </summary>
        /// <param name="proc">存储过程</param>
        /// <param name="param"></param>
        /// <returns></returns>
        List<T> ExecutePro<T>(string proc, object param);


        /// <summary>
        /// 带参数的存储过程
        /// </summary>
        /// <param name="proc">存储过程名称</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        Task<List<T>> ExecuteProAsync<T>(string proc, object param);

        #endregion

        #region 事务不带参数
        /// <summary>
        /// 事务1 - 全SQL
        /// </summary>
        /// <param name="sqlarr">多条SQL</param>
        /// <returns></returns>
        int ExecuteTransaction(string[] sqlarr);


        /// <summary>
        /// 事务1 - 全SQL
        /// </summary>
        /// <param name="sqlarr">多条SQL</param>
        /// <returns></returns>
        Task<int> ExecuteTransactionAsync(string[] sqlarr);

        #endregion

        #region 事务带参数
        /// <summary>
        /// 事务2 - 声明参数
        ///demo:
        ///dic.Add("Insert into Users values (@UserName, @Email, @Address)",
        ///        new { UserName = "jack", Email = "380234234@qq.com", Address = "上海" });
        /// </summary>
        /// <returns></returns>
        int ExecuteTransaction(Dictionary<string, object> dic);

        /// <summary>
        /// 事务2 - 声明参数
        ///demo:
        ///dic.Add("Insert into Users values (@UserName, @Email, @Address)",
        ///        new { UserName = "jack", Email = "380234234@qq.com", Address = "上海" });
        /// </summary>
        /// <returns></returns>
        Task<int> ExecuteTransactionAsync(Dictionary<string, object> dic);


        #endregion
    }
}

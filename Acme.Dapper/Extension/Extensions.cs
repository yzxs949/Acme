using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Acme.Dapper
{
    /// <summary>
    /// Dapper拓展类
    /// </summary>
    public class Extensions : IExtensions
    {
        /// <summary>
        /// 链接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_connectionString"></param>
        public Extensions(
            string _connectionString)
        {
            ConnectionString = _connectionString;
        }

        #region 查询列表
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="sql">查询的sql</param>
        /// <param name="param">替换参数</param>
        /// <returns></returns>
        public List<T> Query<T>(string sql, object param)
        {
            try
            {
                using IDbConnection con = new SqlConnection(ConnectionString);
                return con.Query<T>(sql, param).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 异步查询列表
        /// </summary>
        /// <param name="sql">查询的sql</param>
        /// <param name="param">替换参数</param>
        /// <returns></returns>
        public async Task<List<T>> QueryAsync<T>(string sql, object param)
        {
            try
            {
                using IDbConnection con = new SqlConnection(ConnectionString);
                return new List<T>(await con.QueryAsync<T>(sql, param));
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 查询第一个数据
        /// <summary>
        /// 查询第一个数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QueryFirst<T>(string sql, object param)
        {
            try
            {
                using IDbConnection con = new SqlConnection(ConnectionString);
                return con.QueryFirst<T>(sql, param);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 异步查询第一个数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<T> QueryFirstAsync<T>(string sql, object param)
        {
            try
            {
                using IDbConnection con = new SqlConnection(ConnectionString);
                return await con.QueryFirstAsync<T>(sql, param);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 查询第一个数据没有返回默认值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QueryFirstOrDefault<T>(string sql, object param)
        {
            try
            {
                using IDbConnection con = new SqlConnection(ConnectionString);
                return con.QueryFirstOrDefault<T>(sql, param);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 查询第一个数据没有返回默认值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param)
        {
            try
            {
                using IDbConnection con = new SqlConnection(ConnectionString);
                return await con.QueryFirstOrDefaultAsync<T>(sql, param);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 查询单条数据
        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QuerySingle<T>(string sql, object param)
        {
            try
            {
                using IDbConnection con = new SqlConnection(ConnectionString);
                return con.QuerySingle<T>(sql, param);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 异步查询单条数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<T> QuerySingleAsync<T>(string sql, object param)
        {
            try
            {
                using IDbConnection con = new SqlConnection(ConnectionString);
                return await con.QuerySingleAsync<T>(sql, param);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 查询单条数据没有返回默认值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QuerySingleOrDefault<T>(string sql, object param)
        {
            try
            {
                using IDbConnection con = new SqlConnection(ConnectionString);
                return con.QuerySingleOrDefault<T>(sql, param);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 查询单条数据没有返回默认值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param)
        {
            try
            {
                using IDbConnection con = new SqlConnection(ConnectionString);
                return await con.QuerySingleOrDefaultAsync<T>(sql, param);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 执行增删改语句
        /// <summary>
        /// 异步调用执行增删改语句
        /// </summary>
        /// <param name="sql">sql，参数</param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(string sql, object param)
        {
            try
            {
                using IDbConnection con = new SqlConnection(ConnectionString);
                return await con.ExecuteAsync(sql, param);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int Execute(string sql, object param)
        {
            try
            {
                using IDbConnection con = new SqlConnection(ConnectionString);
                return con.Execute(sql, param);
            }
            catch (Exception)
            {
                throw;
            }
        }
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
        public async Task<int> InsertAsync<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null) where T : class, new()
        {
            try
            {
                using var conn = new SqlConnection(ConnectionString);
                return await conn.InsertAsync<T>(model, transaction, commandTimeout);
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// 根据实体类插入表（实体类需要加标记处理）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public int Insert<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null) where T : class, new()
        {
            try
            {
                using var conn = new SqlConnection(ConnectionString);
                return (int)conn.Insert<T>(model, transaction, commandTimeout);
            }
            catch (Exception)
            {
                throw;
            }
        }
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
        public async Task<bool> UpdateAsync<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null) where T : class, new()
        {
            try
            {
                using var conn = new SqlConnection(ConnectionString);
                return await conn.UpdateAsync<T>(model, transaction, commandTimeout);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根据实体类更新表（实体类需要加标记处理）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public bool Update<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null) where T : class, new()
        {
            try
            {
                using var conn = new SqlConnection(ConnectionString);
                return conn.Update<T>(model, transaction, commandTimeout);
            }
            catch (Exception)
            {
                throw;
            }
        }
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
        public async Task<bool> DeleteAsync<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null) where T : class, new()
        {
            try
            {
                using var conn = new SqlConnection(ConnectionString);
                return await conn.DeleteAsync<T>(model, transaction, commandTimeout);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根据实体类删除表（实体类需要加标记处理）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public bool Delete<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null) where T : class, new()
        {
            try
            {
                using var conn = new SqlConnection(ConnectionString);
                return conn.Delete<T>(model, transaction, commandTimeout);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Reader获取数据
        /// <summary>
        /// Reader获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sql, object param)
        {
            using IDbConnection con = new SqlConnection(ConnectionString);
            return con.ExecuteReader(sql, param);
        }

        /// <summary>
        /// 异步Reader获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IDataReader> ExecuteReaderAsync(string sql, object param)
        {
            try
            {
                using IDbConnection con = new SqlConnection(ConnectionString);
                return await con.ExecuteReaderAsync(sql, param);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Scalar获取数据返回object
        /// <summary>
        /// Scalar获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, object param)
        {
            try
            {
                using IDbConnection con = new SqlConnection(ConnectionString);
                return con.ExecuteScalar(sql, param);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Scalar获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<T> ExecuteScalarForTAsync<T>(string sql, object param)
        {
            try
            {
                using IDbConnection con = new SqlConnection(ConnectionString);
                return await con.ExecuteScalarAsync<T>(sql, param);
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region Scalar获取数据返回T
        /// <summary>
        /// Scalar获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T ExecuteScalarForT<T>(string sql, object param)
        {
            using IDbConnection con = new SqlConnection(ConnectionString);
            return con.ExecuteScalar<T>(sql, param);
        }

        /// <summary>
        /// Scalar获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<T> ExecuteScalarAsync<T>(string sql, object param)
        {
            using IDbConnection con = new SqlConnection(ConnectionString);
            return await con.ExecuteScalarAsync<T>(sql, param);
        }
        #endregion

        #region 带参数的存储过程
        /// <summary>
        /// 带参数的存储过程
        /// </summary>
        /// <param name="proc">存储过程</param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<T> ExecutePro<T>(string proc, object param)
        {
            try
            {
                using IDbConnection con = new SqlConnection(ConnectionString);
                List<T> list = con.Query<T>(proc,
                    param,
                    null,
                    true,
                    null,
                    CommandType.StoredProcedure).ToList();
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 带参数的存储过程
        /// </summary>
        /// <param name="proc">存储过程名称</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public async Task<List<T>> ExecuteProAsync<T>(string proc, object param)
        {
            try
            {
                using IDbConnection con = new SqlConnection(ConnectionString);
                List<T> list = new(await con.QueryAsync<T>(proc,
                    param,
                    null,
                    null,
                    CommandType.StoredProcedure));
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 事务不带参数
        /// <summary>
        /// 事务1 - 全SQL
        /// </summary>
        /// <param name="sqlarr">多条SQL</param>
        /// <returns></returns>
        public int ExecuteTransaction(string[] sqlarr)
        {
            try
            {
                using IDbConnection con = new SqlConnection(ConnectionString);
                using var transaction = con.BeginTransaction();
                try
                {
                    int result = 0;
                    foreach (var sql in sqlarr)
                    {
                        result += con.Execute(sql, null, transaction);
                    }
                    transaction.Commit();
                    return result;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 事务1 - 全SQL
        /// </summary>
        /// <param name="sqlarr">多条SQL</param>
        /// <returns></returns>
        public async Task<int> ExecuteTransactionAsync(string[] sqlarr)
        {
            try
            {
                using IDbConnection con = new SqlConnection(ConnectionString);
                using var transaction = con.BeginTransaction();
                try
                {
                    int result = 0;
                    foreach (var sql in sqlarr)
                    {
                        result += await con.ExecuteAsync(sql, null, transaction);
                    }

                    transaction.Commit();
                    return result;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 事务带参数

        /// <summary>
        /// 事务2 - 声明参数
        /// dic.Add("Insert into Users values (@UserName, @Email, @Address)",
        ///    new { UserName = "jack", Email = "380234234@qq.com", Address = "上海" });
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public int ExecuteTransaction(Dictionary<string, object> dic)
        {
            try
            {
                using IDbConnection con = new SqlConnection(ConnectionString);
                using var transaction = con.BeginTransaction();
                try
                {
                    int result = 0;
                    foreach (var sql in dic)
                    {
                        result += con.Execute(sql.Key, sql.Value, transaction);
                    }

                    transaction.Commit();
                    return result;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 事务2 - 声明参数
        ///demo:
        ///dic.Add("Insert into Users values (@UserName, @Email, @Address)",
        ///        new { UserName = "jack", Email = "380234234@qq.com", Address = "上海" });
        /// </summary>
        /// <returns></returns>
        public async Task<int> ExecuteTransactionAsync(Dictionary<string, object> dic)
        {
            try
            {
                using IDbConnection con = new SqlConnection(ConnectionString);
                using var transaction = con.BeginTransaction();
                try
                {
                    int result = 0;
                    foreach (var sql in dic)
                    {
                        result += await con.ExecuteAsync(sql.Key, sql.Value, transaction);
                    }
                    transaction.Commit();
                    return result;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}

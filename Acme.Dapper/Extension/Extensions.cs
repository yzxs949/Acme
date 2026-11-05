using Acme.Dapper.SqlSplice;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.Dapper.Extension
{
    /// <summary>
    /// Dapper拓展类
    /// </summary>
    public class Extensions : IExtensions
    {
        /// <summary>
        /// 链接字符串
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_connectionString">数据库连接字符串</param>
        public Extensions(string _connectionString)
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
                throw new ArgumentNullException(nameof(_connectionString), "数据库连接字符串不能为空");

            ConnectionString = _connectionString;
        }

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        private SqlConnection CreateConnection() => new(ConnectionString);

        #region 查询列表
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="sql">查询的sql</param>
        /// <param name="param">替换参数</param>
        /// <returns></returns>
        public List<T> Query<T>(string sql, object param)
        {
            using SqlConnection con = CreateConnection();
            return new List<T>(con.Query<T>(sql, param));
        }

        /// <summary>
        /// 异步查询列表
        /// </summary>
        /// <param name="sql">查询的sql</param>
        /// <param name="param">替换参数</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<List<T>> QueryAsync<T>(string sql, object param, CancellationToken cancellationToken = default)
        {
            await using SqlConnection con = CreateConnection();
            IEnumerable<T> result = await con.QueryAsync<T>(new CommandDefinition(sql, param, cancellationToken: cancellationToken))
                .ConfigureAwait(false);
            return new List<T>(result);
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
            using SqlConnection con = CreateConnection();
            return con.QueryFirst<T>(sql, param);
        }

        /// <summary>
        /// 异步查询第一个数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<T> QueryFirstAsync<T>(string sql, object param, CancellationToken cancellationToken = default)
        {
            await using SqlConnection con = CreateConnection();
            return await con.QueryFirstAsync<T>(new CommandDefinition(sql, param, cancellationToken: cancellationToken))
                .ConfigureAwait(false);
        }

        /// <summary>
        /// 查询第一个数据没有返回默认值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QueryFirstOrDefault<T>(string sql, object param)
        {
            using SqlConnection con = CreateConnection();
            return con.QueryFirstOrDefault<T>(sql, param);
        }

        /// <summary>
        /// 查询第一个数据没有返回默认值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param, CancellationToken cancellationToken = default)
        {
            await using SqlConnection con = CreateConnection();
            return await con.QueryFirstOrDefaultAsync<T>(new CommandDefinition(sql, param, cancellationToken: cancellationToken))
                .ConfigureAwait(false);
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
            using SqlConnection con = CreateConnection();
            return con.QuerySingle<T>(sql, param);
        }

        /// <summary>
        /// 异步查询单条数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<T> QuerySingleAsync<T>(string sql, object param, CancellationToken cancellationToken = default)
        {
            await using SqlConnection con = CreateConnection();
            return await con.QuerySingleAsync<T>(new CommandDefinition(sql, param, cancellationToken: cancellationToken))
                .ConfigureAwait(false);
        }

        /// <summary>
        /// 查询单条数据没有返回默认值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QuerySingleOrDefault<T>(string sql, object param)
        {
            using SqlConnection con = CreateConnection();
            return con.QuerySingleOrDefault<T>(sql, param);
        }

        /// <summary>
        /// 查询单条数据没有返回默认值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param, CancellationToken cancellationToken = default)
        {
            await using SqlConnection con = CreateConnection();
            return await con.QuerySingleOrDefaultAsync<T>(new CommandDefinition(sql, param, cancellationToken: cancellationToken))
                .ConfigureAwait(false);
        }
        #endregion

        #region 执行增删改语句
        /// <summary>
        /// 异步调用执行增删改语句
        /// </summary>
        /// <param name="sql">sql，参数</param>
        /// <param name="param"></param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(string sql, object param, CancellationToken cancellationToken = default)
        {
            await using SqlConnection con = CreateConnection();
            return await con.ExecuteAsync(new CommandDefinition(sql, param, cancellationToken: cancellationToken))
                .ConfigureAwait(false);
        }

        /// <summary>
        /// 增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int Execute(string sql, object param)
        {
            using SqlConnection con = CreateConnection();
            return con.Execute(sql, param);
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
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<int> InsertAsync<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null, CancellationToken cancellationToken = default) where T : class, new()
        {
            await using SqlConnection con = CreateConnection();
            long id = await con.InsertAsync(model, transaction, commandTimeout).ConfigureAwait(false);
            return ToInt32(id);
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
            using SqlConnection con = CreateConnection();
            return ToInt32(con.Insert(model, transaction, commandTimeout));
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
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null, CancellationToken cancellationToken = default) where T : class, new()
        {
            await using SqlConnection con = CreateConnection();
            return await con.UpdateAsync(model, transaction, commandTimeout).ConfigureAwait(false);
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
            using SqlConnection con = CreateConnection();
            return con.Update(model, transaction, commandTimeout);
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
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null, CancellationToken cancellationToken = default) where T : class, new()
        {
            await using SqlConnection con = CreateConnection();
            return await con.DeleteAsync(model, transaction, commandTimeout).ConfigureAwait(false);
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
            using SqlConnection con = CreateConnection();
            return con.Delete(model, transaction, commandTimeout);
        }
        #endregion

        #region Reader获取数据
        /// <summary>
        /// Reader获取数据。返回的 Reader 通过 CommandBehavior.CloseConnection 绑定连接，关闭 Reader 即关闭连接。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sql, object param)
        {
            SqlConnection con = CreateConnection();
            try
            {
                con.Open();
                SqlCommand command = con.CreateCommand();
                command.CommandText = sql;
                command.Parameters.AddRange(DBSqlParameter.SqlParame(param));
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                con.Dispose();
                throw;
            }
        }

        /// <summary>
        /// 异步Reader获取数据。返回的 Reader 通过 CommandBehavior.CloseConnection 绑定连接，关闭 Reader 即关闭连接。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<IDataReader> ExecuteReaderAsync(string sql, object param, CancellationToken cancellationToken = default)
        {
            SqlConnection con = CreateConnection();
            try
            {
                await con.OpenAsync(cancellationToken).ConfigureAwait(false);
                SqlCommand command = con.CreateCommand();
                command.CommandText = sql;
                command.Parameters.AddRange(DBSqlParameter.SqlParame(param));
                return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                await con.DisposeAsync().ConfigureAwait(false);
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
            using SqlConnection con = CreateConnection();
            return con.ExecuteScalar(sql, param);
        }

        /// <summary>
        /// Scalar获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public Task<T> ExecuteScalarForTAsync<T>(string sql, object param, CancellationToken cancellationToken = default)
            => ExecuteScalarAsync<T>(sql, param, cancellationToken);
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
            using SqlConnection con = CreateConnection();
            return con.ExecuteScalar<T>(sql, param);
        }

        /// <summary>
        /// Scalar获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<T> ExecuteScalarAsync<T>(string sql, object param, CancellationToken cancellationToken = default)
        {
            await using SqlConnection con = CreateConnection();
            return await con.ExecuteScalarAsync<T>(new CommandDefinition(sql, param, cancellationToken: cancellationToken))
                .ConfigureAwait(false);
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
            using SqlConnection con = CreateConnection();
            return new List<T>(con.Query<T>(proc, param, commandType: CommandType.StoredProcedure));
        }

        /// <summary>
        /// 带参数的存储过程
        /// </summary>
        /// <param name="proc">存储过程名称</param>
        /// <param name="param">参数</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<List<T>> ExecuteProAsync<T>(string proc, object param, CancellationToken cancellationToken = default)
        {
            await using SqlConnection con = CreateConnection();
            IEnumerable<T> result = await con.QueryAsync<T>(
                    new CommandDefinition(proc, param, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken))
                .ConfigureAwait(false);
            return new List<T>(result);
        }
        #endregion

        #region 事务不带参数
        /// <summary>
        /// 事务1 - 全SQL
        /// </summary>
        /// <param name="sqlarr">多条SQL</param>
        /// <returns>受影响的行数合计</returns>
        public int ExecuteTransaction(string[] sqlarr)
        {
            ArgumentNullException.ThrowIfNull(sqlarr);

            using SqlConnection con = CreateConnection();
            con.Open();
            using SqlTransaction transaction = con.BeginTransaction();
            try
            {
                int result = 0;
                foreach (string sql in sqlarr)
                    result += con.Execute(sql, null, transaction);

                transaction.Commit();
                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// 事务1 - 全SQL
        /// </summary>
        /// <param name="sqlarr">多条SQL</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>受影响的行数合计</returns>
        public async Task<int> ExecuteTransactionAsync(string[] sqlarr, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(sqlarr);

            await using SqlConnection con = CreateConnection();
            await con.OpenAsync(cancellationToken).ConfigureAwait(false);
            await using SqlTransaction transaction = (SqlTransaction)await con.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                int result = 0;
                foreach (string sql in sqlarr)
                {
                    result += await con.ExecuteAsync(new CommandDefinition(sql, cancellationToken: cancellationToken, transaction: transaction))
                        .ConfigureAwait(false);
                }

                await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
                return result;
            }
            catch
            {
                await transaction.RollbackAsync(CancellationToken.None).ConfigureAwait(false);
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
        /// <param name="dic">SQL 与参数的映射</param>
        /// <returns>受影响的行数合计</returns>
        public int ExecuteTransaction(Dictionary<string, object> dic)
        {
            ArgumentNullException.ThrowIfNull(dic);

            using SqlConnection con = CreateConnection();
            con.Open();
            using SqlTransaction transaction = con.BeginTransaction();
            try
            {
                int result = 0;
                foreach (KeyValuePair<string, object> sql in dic)
                    result += con.Execute(sql.Key, sql.Value, transaction);

                transaction.Commit();
                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// 事务2 - 声明参数
        ///demo:
        ///dic.Add("Insert into Users values (@UserName, @Email, @Address)",
        ///        new { UserName = "jack", Email = "380234234@qq.com", Address = "上海" });
        /// </summary>
        /// <param name="dic">SQL 与参数的映射</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>受影响的行数合计</returns>
        public async Task<int> ExecuteTransactionAsync(Dictionary<string, object> dic, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dic);

            await using SqlConnection con = CreateConnection();
            await con.OpenAsync(cancellationToken).ConfigureAwait(false);
            await using SqlTransaction transaction = (SqlTransaction)await con.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                int result = 0;
                foreach (KeyValuePair<string, object> sql in dic)
                {
                    result += await con.ExecuteAsync(
                            new CommandDefinition(sql.Key, sql.Value, transaction, cancellationToken: cancellationToken))
                        .ConfigureAwait(false);
                }

                await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
                return result;
            }
            catch
            {
                await transaction.RollbackAsync(CancellationToken.None).ConfigureAwait(false);
                throw;
            }
        }
        #endregion

        /// <summary>
        /// 把自增主键安全转换为 int，超出范围时明确抛出而不是产生溢出
        /// </summary>
        /// <param name="value">自增主键</param>
        private static int ToInt32(long value)
            => checked((int)value);
    }
}

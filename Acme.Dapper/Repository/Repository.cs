using Acme.Dapper.Enums;
using Acme.Dapper.Extension;
using Acme.Dapper.SqlSplice;
using Acme.Dapper.Tools;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.Dapper.Repository
{
    /// <summary>
    /// Dapper仓储拓展类
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private readonly IExtensions _dapperHelper;
        private readonly ISplicing<T> _splicing;

        /// <summary>
        /// 构建函数
        /// </summary>
        /// <param name="dapperHelper">Dapper 扩展</param>
        /// <param name="splicing">SQL 拼接器</param>
        public Repository(
            IExtensions dapperHelper,
            ISplicing<T> splicing)
        {
            _dapperHelper = dapperHelper ?? throw new ArgumentNullException(nameof(dapperHelper));
            _splicing = splicing ?? throw new ArgumentNullException(nameof(splicing));
        }

        #region 新增
        /// <summary>
        /// 异步新增返回Id
        /// </summary>
        /// <param name="t">实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>新增项的Id</returns>
        public async Task<int> AddReturnIdAsync(T t, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(t);

            string sql = _splicing.InsertSqlParam(t);
            return await _dapperHelper.ExecuteScalarAsync<int>(sql, t, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 新增返回Id
        /// </summary>
        /// <param name="t">实体</param>
        /// <returns>新增项的Id</returns>
        public int AddReturnId(T t)
        {
            ArgumentNullException.ThrowIfNull(t);

            string sql = _splicing.InsertSqlParam(t);
            return _dapperHelper.ExecuteScalarForT<int>(sql, t);
        }

        /// <summary>
        /// 异步新增
        /// </summary>
        /// <param name="t">实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public Task<int> AddAsync(T t, CancellationToken cancellationToken = default)
            => _dapperHelper.InsertAsync(t, cancellationToken: cancellationToken);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="t">实体</param>
        /// <returns></returns>
        public int Add(T t) => _dapperHelper.Insert(t);
        #endregion

        #region 删除
        /// <summary>
        /// 异步删除实体
        /// </summary>
        /// <param name="t">实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public Task<bool> DeleteAsync(T t, CancellationToken cancellationToken = default)
            => _dapperHelper.DeleteAsync(t, cancellationToken: cancellationToken);

        /// <summary>
        /// 根据Id删除
        /// </summary>
        /// <param name="Id">主键Id</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否删除成功</returns>
        public async Task<bool> DeleteAsync(int Id, CancellationToken cancellationToken = default)
        {
            string sql = _splicing.DeleteSqlParam(new { Id });
            int rows = await _dapperHelper.ExecuteAsync(sql, new { Id }, cancellationToken).ConfigureAwait(false);
            return rows > 0;
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="t">实体</param>
        /// <returns></returns>
        public bool Delete(T t) => _dapperHelper.Delete(t);

        /// <summary>
        /// 根据Id删除
        /// </summary>
        /// <param name="Id">主键Id</param>
        /// <returns>是否删除成功</returns>
        public bool Delete(int Id)
        {
            string sql = _splicing.DeleteSqlParam(new { Id });
            return _dapperHelper.Execute(sql, new { Id }) > 0;
        }
        #endregion

        #region 修改
        /// <summary>
        /// 异步修改返回Id
        /// </summary>
        /// <param name="t">实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>被修改项的Id</returns>
        public async Task<int> UpdateReturnIdAsync(T t, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(t);

            string sql = _splicing.UpdateSqlParam(t);
            return await _dapperHelper.ExecuteScalarAsync<int>(sql, t, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 修改返回Id
        /// </summary>
        /// <param name="t">实体</param>
        /// <returns>被修改项的Id</returns>
        public int UpdateReturnId(T t)
        {
            ArgumentNullException.ThrowIfNull(t);

            string sql = _splicing.UpdateSqlParam(t);
            return _dapperHelper.ExecuteScalarForT<int>(sql, t);
        }

        /// <summary>
        /// 异步修改
        /// </summary>
        /// <param name="t">实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public Task<bool> UpdateAsync(T t, CancellationToken cancellationToken = default)
            => _dapperHelper.UpdateAsync(t, cancellationToken: cancellationToken);

        /// <summary>
        /// 根据Id修改实体
        /// </summary>
        /// <param name="UpdateContext">更新内容（须包含主键 Id）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>被修改项的Id</returns>
        public async Task<int> UpdateByIdAsync(object UpdateContext, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(UpdateContext);

            string sql = _splicing.UpdateSqlParam(UpdateContext);
            return await _dapperHelper.ExecuteScalarAsync<int>(sql, UpdateContext, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="t">实体</param>
        /// <returns></returns>
        public bool Update(T t) => _dapperHelper.Update(t);
        #endregion

        #region 获取单条
        /// <summary>
        /// 根据Id获取单条数据，无数据时抛出异常
        /// </summary>
        /// <param name="Id">主键Id</param>
        /// <returns></returns>
        public T GetInfo(int Id)
        {
            var SqlWhere = new { Id };
            string sql = _splicing.SelectSqlParam(SqlWhere);
            return _dapperHelper.QuerySingle<T>(sql, SqlWhere);
        }

        /// <summary>
        /// 根据Id获取单条数据，无数据时返回默认值
        /// </summary>
        /// <param name="Id">主键Id</param>
        /// <returns></returns>
        public T GetInfoOrDefault(int Id)
        {
            var SqlWhere = new { Id };
            string sql = _splicing.SelectSqlParam(SqlWhere);
            return _dapperHelper.QueryFirstOrDefault<T>(sql, SqlWhere);
        }

        /// <summary>
        /// 按条件获取单条数据，无数据时抛出异常
        /// </summary>
        /// <param name="SqlWhere">条件</param>
        /// <returns></returns>
        public T GetInfo(object SqlWhere)
        {
            string sql = _splicing.SelectSqlParam(SqlWhere);
            return _dapperHelper.QuerySingle<T>(sql, SqlWhere);
        }

        /// <summary>
        /// 按条件获取单条数据，无数据时返回默认值
        /// </summary>
        /// <param name="SqlWhere">条件</param>
        /// <returns></returns>
        public T GetInfoOrDefault(object SqlWhere)
        {
            string sql = _splicing.SelectSqlParam(SqlWhere);
            return _dapperHelper.QueryFirstOrDefault<T>(sql, SqlWhere);
        }

        /// <summary>
        /// 异步获取单条，无数据时抛出异常
        /// </summary>
        /// <param name="Id">主键Id</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<T> GetInfoAsync(int Id, CancellationToken cancellationToken = default)
        {
            var SqlWhere = new { Id };
            string sql = _splicing.SelectSqlParam(SqlWhere);
            return await _dapperHelper.QuerySingleAsync<T>(sql, SqlWhere, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 异步获取单条，无数据时返回默认值
        /// </summary>
        /// <param name="Id">主键Id</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<T> GetInfoOrDefaultAsync(int Id, CancellationToken cancellationToken = default)
        {
            var SqlWhere = new { Id };
            string sql = _splicing.SelectSqlParam(SqlWhere);
            return await _dapperHelper.QueryFirstOrDefaultAsync<T>(sql, SqlWhere, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 异步获取单条，无数据时抛出异常
        /// </summary>
        /// <param name="SqlWhere">条件</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<T> GetInfoAsync(object SqlWhere, CancellationToken cancellationToken = default)
        {
            string sql = _splicing.SelectSqlParam(SqlWhere);
            return await _dapperHelper.QuerySingleAsync<T>(sql, SqlWhere, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 异步获取单条，无数据时返回默认值
        /// </summary>
        /// <param name="SqlWhere">条件</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<T> GetInfoOrDefaultAsync(object SqlWhere, CancellationToken cancellationToken = default)
        {
            string sql = _splicing.SelectSqlParam(SqlWhere);
            return await _dapperHelper.QueryFirstOrDefaultAsync<T>(sql, SqlWhere, cancellationToken).ConfigureAwait(false);
        }
        #endregion

        #region 列表与分页查询

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="SqlWhere">条件</param>
        /// <returns></returns>
        public List<T> GetList(object SqlWhere)
        {
            string sql = _splicing.SelectSqlParam(SqlWhere);
            return _dapperHelper.Query<T>(sql, SqlWhere);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="SqlWhere">条件</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public Task<List<T>> GetListAsync(object SqlWhere, CancellationToken cancellationToken = default)
        {
            string sql = _splicing.SelectSqlParam(SqlWhere);
            return _dapperHelper.QueryAsync<T>(sql, SqlWhere, cancellationToken);
        }

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <param name="SqlWhere">条件</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public Task<int> GetCountAsync(object SqlWhere, CancellationToken cancellationToken = default)
        {
            string sql = _splicing.SelectCountParam(SqlWhere);
            return _dapperHelper.ExecuteScalarAsync<int>(sql, SqlWhere, cancellationToken);
        }

        /// <summary>
        /// 根据sql获取分页数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="pageIndex">页码（从 1 开始）</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderType">排序类型</param>
        /// <returns></returns>
        public PageList GetPageListBySql(
            string sql,
            int pageIndex,
            int pageSize,
            string orderField = "",
            OrderType orderType = OrderType.ASC)
        {
            string countSql = _splicing.GetCountSql(sql);
            string pageSql = _splicing.GetPageSql(sql, pageIndex, pageSize, orderField, orderType);

            return new PageList
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = _dapperHelper.ExecuteScalarForT<int>(countSql, null),
                Items = _dapperHelper.Query<object>(pageSql, null),
            };
        }

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
        public async Task<PageList> GetPageListBySqlAsync(
            string sql,
            int pageIndex,
            int pageSize,
            string orderField = "",
            OrderType orderType = OrderType.ASC,
            CancellationToken cancellationToken = default)
        {
            string countSql = _splicing.GetCountSql(sql);
            string pageSql = _splicing.GetPageSql(sql, pageIndex, pageSize, orderField, orderType);

            int total = await _dapperHelper.ExecuteScalarAsync<int>(countSql, null, cancellationToken).ConfigureAwait(false);
            List<object> items = await _dapperHelper.QueryAsync<object>(pageSql, null, cancellationToken).ConfigureAwait(false);

            return new PageList
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = total,
                Items = items,
            };
        }

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
        public PageList<TEntity> GetPageListBySql<TEntity>(
            string sql,
            int pageIndex,
            int pageSize,
            string orderField = "",
            OrderType orderType = OrderType.ASC)
        {
            string countSql = _splicing.GetCountSql(sql);
            string pageSql = _splicing.GetPageSql(sql, pageIndex, pageSize, orderField, orderType);

            return new PageList<TEntity>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = _dapperHelper.ExecuteScalarForT<int>(countSql, null),
                Items = _dapperHelper.Query<TEntity>(pageSql, null),
            };
        }

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
        public async Task<PageList<TEntity>> GetPageListBySqlAsync<TEntity>(
            string sql,
            int pageIndex,
            int pageSize,
            string orderField = "",
            OrderType orderType = OrderType.ASC,
            CancellationToken cancellationToken = default)
        {
            string countSql = _splicing.GetCountSql(sql);
            string pageSql = _splicing.GetPageSql(sql, pageIndex, pageSize, orderField, orderType);

            int total = await _dapperHelper.ExecuteScalarAsync<int>(countSql, null, cancellationToken).ConfigureAwait(false);
            List<TEntity> items = await _dapperHelper.QueryAsync<TEntity>(pageSql, null, cancellationToken).ConfigureAwait(false);

            return new PageList<TEntity>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = total,
                Items = items,
            };
        }
        #endregion
    }
}

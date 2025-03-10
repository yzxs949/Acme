using Acme.Dapper.Enums;
using Acme.Dapper.Extension;
using Acme.Dapper.SqlSplice;
using Acme.Dapper.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acme.Dapper.Repository
{
    /// <summary>
    /// Dapper仓储拓展类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private readonly IExtensions _dapperHelper;
        private readonly ISplicing<T> _splicing;

        /// <summary>
        /// 构建函数
        /// </summary>
        /// <param name="dapperHelper"></param>
        /// <param name="splicing"></param>
        public Repository(
            IExtensions dapperHelper,
            ISplicing<T> splicing)
        {
            _dapperHelper = dapperHelper;
            _splicing = splicing;
        }

        #region 新增
        /// <summary>
        /// 异步新增返回Id
        /// </summary>
        /// <param name="t">实体</param>
        /// <returns>新增项的Id</returns>
        public async Task<int> AddReturnIdAsync(T t)
        {
            try
            {
                string sql = _splicing.InsertSqlParam(t);
                return await _dapperHelper.ExecuteAsync(sql, t);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 新增返回Id
        /// </summary>
        /// <param name="t">实体</param>
        /// <returns>新增项的Id</returns>
        public int AddReturnId(T t)
        {
            try
            {
                string sql = _splicing.InsertSqlParam(t);
                return _dapperHelper.Execute(sql, t);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 异步新增
        /// </summary>
        /// <param name="t">实体</param>
        /// <returns></returns>
        public async Task<int> AddAsync(T t)
        {
            try
            {
                return await _dapperHelper.InsertAsync(t);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="t">实体</param>
        /// <returns></returns>
        public int Add(T t)
        {
            try
            {
                return _dapperHelper.Insert(t);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 删除
        /// <summary>
        /// 异步删除实体
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(T t)
        {
            try
            {
                return await _dapperHelper.DeleteAsync(t);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根据Id删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(int Id)
        {
            try
            {
                T t = await GetInfoAsync(Id);
                return await _dapperHelper.DeleteAsync(t);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Delete(T t)
        {
            try
            {
                return _dapperHelper.Delete(t);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool Delete(int Id)
        {
            try
            {
                T t = GetInfo(Id);
                return _dapperHelper.Delete(t);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 修改
        /// <summary>
        /// 异步修改返回Id
        /// </summary>
        /// <param name="t">实体</param>
        /// <returns>新增项的Id</returns>
        public async Task<int> UpdateReturnIdAsync(T t)
        {
            try
            {
                string sql = _splicing.UpdateSqlParam(t);
                return await _dapperHelper.ExecuteAsync(sql, t);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 异步修改返回Id
        /// </summary>
        /// <param name="UpdateContext">实体</param>
        /// <returns>新增项的Id</returns>
        public async Task<int> UpdateByIdAsync(object UpdateContext)
        {
            try
            {
                string sql = _splicing.UpdateSqlParam(UpdateContext);
                return await _dapperHelper.ExecuteAsync(sql, UpdateContext);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 修改返回Id
        /// </summary>
        /// <param name="t">实体</param>
        /// <returns>新增项的Id</returns>
        public int UpdateReturnId(T t)
        {
            try
            {
                string sql = _splicing.InsertSqlParam(t);
                return _dapperHelper.Execute(sql, t);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 异步修改
        /// </summary>
        /// <param name="t">实体</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(T t)
        {
            try
            {
                return await _dapperHelper.UpdateAsync(t);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="t">实体</param>
        /// <returns></returns>
        public bool Update(T t)
        {
            try
            {
                return _dapperHelper.Update(t);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 获取单条
        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="Id">主键Id</param>
        /// <returns></returns>
        public T GetInfo(int Id)
        {
            try
            {
                var SqlWhere = new { Id };
                string sql = _splicing.SelectSqlParam(SqlWhere);
                return _dapperHelper.QuerySingle<T>(sql, SqlWhere);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="Id">主键Id</param>
        /// <returns></returns>
        public T GetInfoOrDefault(int Id)
        {
            var SqlWhere = new { Id };
            return GetList(SqlWhere).FirstOrDefault();
        }

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="SqlWhere">条件</param>
        /// <returns></returns>
        public T GetInfo(object SqlWhere)
        {
            try
            {
                string sql = _splicing.SelectSqlParam(SqlWhere);
                return _dapperHelper.QuerySingle<T>(sql, SqlWhere);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="SqlWhere">主键Id</param>
        /// <returns></returns>
        public T GetInfoOrDefault(object SqlWhere)
        {
            return GetList(SqlWhere).FirstOrDefault();
        }

        /// <summary>
        /// 异步获取单条
        /// </summary>
        /// <param name="Id">主键Id</param>
        /// <returns></returns>
        public async Task<T> GetInfoAsync(int Id)
        {
            try
            {
                var SqlWhere = new { Id };
                string sql = _splicing.SelectSqlParam(SqlWhere);
                return await _dapperHelper.QuerySingleAsync<T>(sql, SqlWhere);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="Id">主键Id</param>
        /// <returns></returns>
        public async Task<T> GetInfoOrDefaultAsync(int Id)
        {
            var SqlWhere = new { Id };
            List<T> list = await GetListAsync(SqlWhere);
            return list.FirstOrDefault();
        }

        /// <summary>
        /// 异步获取单条
        /// </summary>
        /// <param name="SqlWhere">条件</param>
        /// <returns></returns>
        public async Task<T> GetInfoAsync(object SqlWhere)
        {
            try
            {
                string sql = _splicing.SelectSqlParam(SqlWhere);
                return await _dapperHelper.QuerySingleAsync<T>(sql, SqlWhere);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 异步获取单条
        /// </summary>
        /// <param name="SqlWhere">条件</param>
        /// <returns></returns>
        public async Task<T> GetInfoOrDefaultAsync(object SqlWhere)
        {
            List<T> list = await GetListAsync(SqlWhere);
            return list.FirstOrDefault();
        }
        #endregion

        #region 查询

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="SqlWhere">条件</param>
        /// <returns></returns>
        public List<T> GetList(object SqlWhere)
        {
            try
            {
                string sql = _splicing.SelectSqlParam(SqlWhere);
                return _dapperHelper.Query<T>(sql, SqlWhere);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="SqlWhere">条件</param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(object SqlWhere)
        {
            try
            {
                string sql = _splicing.SelectSqlParam(SqlWhere);
                return await _dapperHelper.QueryAsync<T>(sql, SqlWhere);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <param name="SqlWhere">条件</param>
        /// <returns></returns>
        public async Task<int> GetCountAsync(object SqlWhere)
        {
            try
            {
                string sql = _splicing.SelectCountParam(SqlWhere);
                int count = await _dapperHelper.ExecuteScalarAsync<int>(sql, SqlWhere);
                return count;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根据sql获取分页数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="pageIndex">页码</param>
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
            PageList pageList = new()
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            string countSql = _splicing.GetCountSql(sql);
            int count = _dapperHelper.ExecuteScalarForT<int>(countSql, null);
            pageList.Total = count;
            string pageSql = _splicing.GetPageSql(sql, pageIndex, pageSize, orderField, orderType);
            List<object> obj = _dapperHelper.Query<object>(pageSql, null);
            pageList.Items = obj;
            return pageList;
        }

        /// <summary>
        /// 根据sql获取分页数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderType">排序类型</param>
        /// <returns></returns>
        public async Task<PageList> GetPageListBySqlAsync(
            string sql,
            int pageIndex,
            int pageSize,
            string orderField = "",
            OrderType orderType = OrderType.ASC)
        {
            PageList pageList = new()
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            string countSql = _splicing.GetCountSql(sql);
            int count = await _dapperHelper.ExecuteScalarAsync<int>(countSql, null);
            pageList.Total = count;
            string pageSql = _splicing.GetPageSql(sql, pageIndex, pageSize, orderField, orderType);
            List<object> obj = await _dapperHelper.QueryAsync<object>(pageSql, null);
            pageList.Items = obj;
            return pageList;
        }

        /// <summary>
        /// 根据sql获取分页数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderType">排序类型</param>
        /// <returns></returns>
        public PageList<TEntity> GetPageListBySql<TEntity>(
            string sql,
            int pageIndex,
            int pageSize,
            string orderField = "",
            OrderType orderType = OrderType.ASC)
        {
            PageList<TEntity> pageList = new()
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            string countSql = _splicing.GetCountSql(sql);
            int count = _dapperHelper.ExecuteScalarForT<int>(countSql, null);
            pageList.Total = count;
            string pageSql = _splicing.GetPageSql(sql, pageIndex, pageSize, orderField, orderType);
            List<TEntity> obj = _dapperHelper.Query<TEntity>(pageSql, null);
            pageList.Items = obj;
            return pageList;
        }

        /// <summary>
        /// 根据sql获取分页数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderType">排序类型</param>
        /// <returns></returns>
        public async Task<PageList<TEntity>> GetPageListBySqlAsync<TEntity>(
            string sql,
            int pageIndex,
            int pageSize,
            string orderField = "",
            OrderType orderType = OrderType.ASC)
        {
            PageList<TEntity> pageList = new()
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            string countSql = _splicing.GetCountSql(sql);
            int count = await _dapperHelper.ExecuteScalarAsync<int>(countSql, null);
            pageList.Total = count;
            string pageSql = _splicing.GetPageSql(sql, pageIndex, pageSize, orderField, orderType);
            List<TEntity> obj = await _dapperHelper.QueryAsync<TEntity>(pageSql, null);
            pageList.Items = obj;
            return pageList;
        }
        #endregion

    }
}

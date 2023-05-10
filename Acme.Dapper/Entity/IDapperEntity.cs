using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acme;

namespace Acme.Dapper
{
    /// <summary>
    /// Dapper拓展实体实现接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDapperEntity<T>
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
        #endregion
    }
}

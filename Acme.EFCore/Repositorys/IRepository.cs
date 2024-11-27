
namespace Acme.EFCore.Repositorys
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class, new()
    {

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        T Add(T entity);

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        void BeginTransaction();

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        Task BeginTransactionAsync();

        /// <summary>
        /// 提交事务
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// 提交事务
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <returns></returns>
        int Count();

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> whereLamdba);

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <returns></returns>
        Task<int> CountAsync();

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        Task<int> CountAsync(Expression<Func<T, bool>> whereLamdba);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">删除</param>
        /// <returns></returns>
        bool Delete(T entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">删除</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(T entity);

        /// <summary>
        /// 关闭事务释放资源
        /// </summary>
        void DisposeTransaction();

        /// <summary>
        /// 关闭事务释放资源
        /// </summary>
        Task DisposeTransactionAsync();

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        Task<T> GetInfoAsync(Expression<Func<T, bool>> whereLamdba);

        /// <summary>
        /// 获取单条数据返回默认值
        /// </summary>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        T GetInfoDefault(Expression<Func<T, bool>> whereLamdba);

        /// <summary>
        /// 获取单条数据返回默认值
        /// </summary>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        Task<T> GetInfoDefaultAsync(Expression<Func<T, bool>> whereLamdba);

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <returns></returns>
        List<T> GetList();

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        List<T> GetList(Expression<Func<T, bool>> whereLamdba);

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetListAsync();

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> whereLamdba);

        /// <summary>
        /// 获取Queryable
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetQueryable();

        /// <summary>
        /// 按条件获取Queryable
        /// </summary>
        /// <param name="whereLamdba">Linq语句</param>
        /// <returns></returns>
        IQueryable<T> GetQueryable(Expression<Func<T, bool>> whereLamdba);

        /// <summary>
        /// 回滚事务
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// 回滚事务
        /// </summary>
        Task RollbackTransactionAsync();

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">删除</param>
        /// <returns></returns>
        bool Update(T entity);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">删除</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(T entity);
    }
}
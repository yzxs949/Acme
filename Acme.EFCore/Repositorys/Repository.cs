using Microsoft.EntityFrameworkCore;
using NPOI.POIFS.FileSystem;

namespace Acme.EFCore.Repositorys;

/// <summary>
/// 仓储类
/// </summary>
/// <typeparam name="T"></typeparam>
public class Repository<T> : IRepository<T> where T : class, new()
{
    private readonly IBaseService _baseService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="baseService"></param>
    public Repository(IBaseService baseService)
    {
        _baseService = baseService;
    }

    /// <summary>
    /// 新增实体
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns></returns>
    public T Add(T entity)
    {
        return _baseService.Add<T>(entity);
    }

    /// <summary>
    /// 新增实体
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns></returns>
    public async Task<T> AddAsync(T entity)
    {
        return await _baseService.AddAsync<T>(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="entity">删除</param>
    /// <returns></returns>
    public bool Delete(T entity)
    {
        return _baseService.Delete<T>(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="entity">删除</param>
    /// <returns></returns>
    public async Task<bool> DeleteAsync(T entity)
    {
        return await _baseService.DeleteAsync<T>(entity);
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="entity">删除</param>
    /// <returns></returns>
    public bool Update(T entity)
    {
        return _baseService.Update<T>(entity);
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="entity">删除</param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(T entity)
    {
        return await _baseService.UpdateAsync<T>(entity);
    }

    /// <summary>
    /// 获取Queryable
    /// </summary>
    /// <returns></returns>
    public IQueryable<T> GetQueryable()
    {
        return _baseService.GetQueryable<T>();
    }

    /// <summary>
    /// 按条件获取Queryable
    /// </summary>
    /// <param name="whereLamdba">Linq语句</param>
    /// <returns></returns>
    public IQueryable<T> GetQueryable(Expression<Func<T, bool>> whereLamdba)
    {
        return _baseService.GetQueryable<T>(whereLamdba);
    }

    /// <summary>
    /// 获取单条数据
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public async Task<T> GetInfoAsync(Expression<Func<T, bool>> whereLamdba)
    {
        return await _baseService.GetInfoAsync<T>(whereLamdba);
    }

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public List<T> GetList(Expression<Func<T, bool>> whereLamdba)
    {
        return _baseService.GetList<T>(whereLamdba);
    }

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <returns></returns>

    public List<T> GetList()
    {
        return _baseService.GetList<T>();
    }

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> whereLamdba)
    {
        return await _baseService.GetListAsync<T>(whereLamdba);
    }

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <returns></returns>
    public async Task<List<T>> GetListAsync()
    {
        return await _baseService.GetListAsync<T>();
    }

    /// <summary>
    /// 获取单条数据返回默认值
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public T GetInfoDefault(Expression<Func<T, bool>> whereLamdba)
    {
        return _baseService.GetInfoDefault<T>(whereLamdba);
    }

    /// <summary>
    /// 获取单条数据返回默认值
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public async Task<T> GetInfoDefaultAsync(Expression<Func<T, bool>> whereLamdba)
    {
        return await _baseService.GetInfoDefaultAsync<T>(whereLamdba);
    }

    /// <summary>
    /// 获取条数
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public int Count(Expression<Func<T, bool>> whereLamdba)
    {
        return _baseService.Count(whereLamdba);
    }

    /// <summary>
    /// 获取条数
    /// </summary>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public async Task<int> CountAsync(Expression<Func<T, bool>> whereLamdba)
    {
        return await _baseService.CountAsync<T>(whereLamdba);
    }

    /// <summary>
    /// 获取条数
    /// </summary>
    /// <returns></returns>
    public int Count()
    {
        return _baseService.Count<T>();
    }

    /// <summary>
    /// 获取条数
    /// </summary>
    /// <returns></returns>
    public async Task<int> CountAsync()
    {
        return await _baseService.CountAsync<T>();
    }

    /// <summary>
    /// 开启事务
    /// </summary>
    /// <returns></returns>
    public void BeginTransaction()
    {
        _baseService.BeginTransaction();
    }

    /// <summary>
    /// 开启事务
    /// </summary>
    /// <returns></returns>
    public async Task BeginTransactionAsync()
    {
        await _baseService.BeginTransactionAsync();
    }

    /// <summary>
    /// 提交事务
    /// </summary>
    public void CommitTransaction()
    {
        _baseService.CommitTransaction();
    }

    /// <summary>
    /// 提交事务
    /// </summary>
    public async Task CommitTransactionAsync()
    {
        await _baseService.CommitTransactionAsync();
    }

    /// <summary>
    /// 回滚事务
    /// </summary>
    public void RollbackTransaction()
    {
        _baseService.RollbackTransaction();
    }

    /// <summary>
    /// 回滚事务
    /// </summary>
    public async Task RollbackTransactionAsync()
    {
        await _baseService.RollbackTransactionAsync();
    }

    /// <summary>
    /// 关闭事务释放资源
    /// </summary>
    public void DisposeTransaction()
    {
        _baseService.DisposeTransaction();
    }

    /// <summary>
    /// 关闭事务释放资源
    /// </summary>
    public async Task DisposeTransactionAsync()
    {
        await _baseService.DisposeTransactionAsync();
    }
}
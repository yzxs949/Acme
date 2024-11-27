namespace Acme.EFCore.BaseServices;


/// <summary>
/// EFCore实现服务类
/// </summary>
public class BaseService<TDbContext> : IBaseService<TDbContext>, IBaseService where TDbContext : DbContext
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public TDbContext dbContext { get; set; }

    /// <summary>
    /// 依赖注入数据库上下文
    /// </summary>
    /// <param name="_dbContext"></param>
    public BaseService(TDbContext _dbContext)
    {
        dbContext = _dbContext;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    public T Add<T>(T entity) where T : class
    {
        dbContext.Set<T>().Add(entity);
        dbContext.SaveChanges();
        return entity;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task<T> AddAsync<T>(T entity) where T : class
    {
        await dbContext.Set<T>().AddAsync(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// 批量新增
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public bool AddMany<T>(List<T> list) where T : class
    {
        dbContext.Set<T>().AddRange(list);
        return dbContext.SaveChanges() > 0;
    }

    /// <summary>
    /// 批量新增
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public async Task<bool> AddManyAsync<T>(List<T> list) where T : class
    {
        await dbContext.Set<T>().AddRangeAsync(list);
        return await dbContext.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool Delete<T>(T entity) where T : class
    {
        dbContext.Set<T>().Remove(entity);
        return dbContext.SaveChanges() > 0;
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task<bool> DeleteAsync<T>(T entity) where T : class
    {
        dbContext.Set<T>().Remove(entity);
        return await dbContext.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public bool DelMany<T>(List<T> list) where T : class
    {
        dbContext.Set<T>().RemoveRange(list);
        return dbContext.SaveChanges() > 0;
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public async Task<bool> DelManyAsync<T>(List<T> list) where T : class
    {
        dbContext.Set<T>().RemoveRange(list);
        return await dbContext.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool Update<T>(T entity) where T : class
    {
        dbContext.Update(entity);
        return dbContext.SaveChanges() > 0;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync<T>(T entity) where T : class
    {
        dbContext.Update(entity);
        return await dbContext.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// 批量修改
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public bool UpdateMany<T>(List<T> list) where T : class
    {
        dbContext.UpdateRange(list);
        return dbContext.SaveChanges() > 0;
    }

    /// <summary>
    /// 批量修改
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public async Task<bool> UpdateManyAsync<T>(List<T> list) where T : class
    {
        dbContext.UpdateRange(list);
        return await dbContext.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// 是否存在
    /// </summary>
    /// <param name="anyLambda"></param>
    /// <returns></returns>
    public bool Exist<T>(Expression<Func<T, bool>> anyLambda) where T : class
    {
        return dbContext.Set<T>().Any(anyLambda);
    }

    /// <summary>
    /// 是否存在
    /// </summary>
    /// <param name="anyLambda"></param>
    /// <returns></returns>
    public async Task<bool> ExistAsync<T>(Expression<Func<T, bool>> anyLambda) where T : class
    {
        return await dbContext.Set<T>().AnyAsync(anyLambda);
    }

    /// <summary>
    /// 获取Queryable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IQueryable<T> GetQueryable<T>() where T : class
    {
        return dbContext.Set<T>();
    }

    /// <summary>
    /// 按条件获取Queryable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public IQueryable<T> GetQueryable<T>(Expression<Func<T, bool>> whereLamdba) where T : class
    {
        return dbContext.Set<T>().Where(whereLamdba);
    }

    /// <summary>
    /// 获取单条数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public T GetInfo<T>(Expression<Func<T, bool>> whereLamdba) where T : class
    {
        return dbContext.Find<T>(whereLamdba);
    }

    /// <summary>
    /// 获取单条数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public async Task<T> GetInfoAsync<T>(Expression<Func<T, bool>> whereLamdba) where T : class
    {
        return await dbContext.FindAsync<T>(whereLamdba);
    }

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public List<T> GetList<T>(Expression<Func<T, bool>> whereLamdba) where T : class
    {
        return GetQueryable<T>(whereLamdba).ToList();
    }

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>

    public List<T> GetList<T>() where T : class
    {
        return GetQueryable<T>().ToList();
    }

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public async Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> whereLamdba) where T : class
    {
        return await GetQueryable<T>(whereLamdba).ToListAsync();
    }

    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<List<T>> GetListAsync<T>() where T : class
    {
        return await GetQueryable<T>().ToListAsync();
    }

    /// <summary>
    /// 获取单条数据返回默认值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public T GetInfoDefault<T>(Expression<Func<T, bool>> whereLamdba) where T : class
    {
        return dbContext.Set<T>().FirstOrDefault(whereLamdba);
    }

    /// <summary>
    /// 获取单条数据返回默认值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public async Task<T> GetInfoDefaultAsync<T>(Expression<Func<T, bool>> whereLamdba) where T : class
    {
        return await dbContext.Set<T>().FirstOrDefaultAsync(whereLamdba);
    }

    /// <summary>
    /// 获取条数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public int Count<T>(Expression<Func<T, bool>> whereLamdba) where T : class
    {
        return dbContext.Set<T>().Count(whereLamdba);
    }

    /// <summary>
    /// 获取条数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="whereLamdba"></param>
    /// <returns></returns>
    public async Task<int> CountAsync<T>(Expression<Func<T, bool>> whereLamdba) where T : class
    {
        return await dbContext.Set<T>().CountAsync(whereLamdba);
    }

    /// <summary>
    /// 获取条数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public int Count<T>() where T : class
    {
        return dbContext.Set<T>().Count();
    }

    /// <summary>
    /// 获取条数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<int> CountAsync<T>() where T : class
    {
        return await dbContext.Set<T>().CountAsync();
    }

    private IDbContextTransaction contextTransaction;

    /// <summary>
    /// 开启事务
    /// </summary>
    /// <returns></returns>
    public void BeginTransaction()
    {
        contextTransaction = dbContext.Database.BeginTransaction();
    }

    /// <summary>
    /// 开启事务
    /// </summary>
    /// <returns></returns>
    public async Task BeginTransactionAsync()
    {
        contextTransaction = await dbContext.Database.BeginTransactionAsync();
    }

    /// <summary>
    /// 提交事务
    /// </summary>
    public void CommitTransaction()
    {
        if (contextTransaction != null)
            contextTransaction.Commit();
        else
            throw Oh.OhNo("您未开启事务！");
    }

    /// <summary>
    /// 提交事务
    /// </summary>
    public async Task CommitTransactionAsync()
    {
        if (contextTransaction != null)
            await contextTransaction.CommitAsync();
        else
            throw Oh.OhNo("您未开启事务！");
    }

    /// <summary>
    /// 回滚事务
    /// </summary>
    public void RollbackTransaction()
    {
        if (contextTransaction != null)
            contextTransaction.Rollback();
        else
            throw Oh.OhNo("您未开启事务！");
    }

    /// <summary>
    /// 回滚事务
    /// </summary>
    public async Task RollbackTransactionAsync()
    {
        if (contextTransaction != null)
            await contextTransaction.RollbackAsync();
        else
            throw Oh.OhNo("您未开启事务！");
    }

    /// <summary>
    /// 关闭事务释放资源
    /// </summary>
    public void DisposeTransaction()
    {
        if (contextTransaction != null)
        {
            contextTransaction.Dispose();
            dbContext.Dispose();
        }
        else
            throw Oh.OhNo("您未开启事务！");
    }

    /// <summary>
    /// 关闭事务释放资源
    /// </summary>
    public async Task DisposeTransactionAsync()
    {
        if (contextTransaction != null)
        {
            await contextTransaction.DisposeAsync();
            await dbContext.DisposeAsync();
        }
        else
            throw Oh.OhNo("您未开启事务！");
    }
}

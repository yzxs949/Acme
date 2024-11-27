namespace Acme.EFCore.Extensions;

/// <summary>
/// EFCore拓展方法
/// </summary>
public static class CollectionExtension
{
    ///<summary>
    /// 注册Acme.EFCore的方法
    /// </summary>
    /// <param name="_services">IServiceCollection</param>
    /// <param name="_connectionString">数据库连接字符</param>
    /// <param name="logConfig">日志配置项目</param>
    public static void AddTransientBaseService<TDbContext>(
        this IServiceCollection _services,
        string _connectionString,
        LogConfig logConfig) where TDbContext : DbContext
    {
        _services.AddDbContext<TDbContext>(option => option.UseSqlServer(_connectionString), ServiceLifetime.Transient);
        _services.AddTransient(typeof(IBaseService<>), typeof(BaseService<>));
        _services.AddAcmeBuilderServer(logConfig);
    }


    ///<summary>
    /// 注册Acme.EFCore的方法(不注入Acme通用类)
    /// </summary>
    /// <param name="_services">IServiceCollection</param>
    /// <param name="_connectionString">数据库连接字符</param>
    public static void AddTransientBaseService<TDbContext>(
        this IServiceCollection _services,
        string _connectionString) where TDbContext : DbContext
    {
        _services.AddDbContext<TDbContext>(option => option.UseSqlServer(_connectionString), ServiceLifetime.Transient);
        _services.AddTransient(typeof(IBaseService<>), typeof(BaseService<>));
    }

    ///<summary>
    /// 注册Acme.EFCore使用仓储模式
    /// </summary>
    /// <param name="_services">IServiceCollection</param>
    /// <param name="_connectionString">数据库连接字符</param>
    /// <param name="logConfig">日志配置（为null需要单独注入Acme的类库）</param>
    public static void AddTransientRepository<TDbContext>(
        this IServiceCollection _services,
        string _connectionString,
        LogConfig logConfig = null) where TDbContext : DbContext
    {
        if (logConfig == null)
            _services.AddTransientBaseService<TDbContext>(_connectionString);
        else
            _services.AddTransientBaseService<TDbContext>(_connectionString, logConfig);
        _services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
    }

    ///<summary>
    /// 注册Acme.EFCore的方法
    /// </summary>
    /// <param name="_services">IServiceCollection</param>
    /// <param name="_connectionString">数据库连接字符</param>
    /// <param name="logConfig">日志配置项目</param>
    public static void AddSingletonBaseService<TDbContext>(
        this IServiceCollection _services,
        string _connectionString,
        LogConfig logConfig) where TDbContext : DbContext
    {
        _services.AddDbContext<TDbContext>(option => option.UseSqlServer(_connectionString), ServiceLifetime.Singleton);
        _services.AddSingleton(typeof(IBaseService<>), typeof(BaseService<>));
        _services.AddAcmeBuilderServer(logConfig);
    }

    ///<summary>
    /// 注册Acme.EFCore使用仓储模式
    /// </summary>
    /// <param name="_services">IServiceCollection</param>
    /// <param name="_connectionString">数据库连接字符</param>
    /// <param name="logConfig">日志配置（为null需要单独注入Acme的类库）</param>
    public static void AddSingletonRepository<TDbContext>(
        this IServiceCollection _services,
        string _connectionString,
        LogConfig logConfig = null) where TDbContext : DbContext
    {
        if (logConfig == null)
            _services.AddSingletonBaseService<TDbContext>(_connectionString);
        else
            _services.AddSingletonBaseService<TDbContext>(_connectionString, logConfig);
        _services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
    }

    ///<summary>
    /// 注册Acme.EFCore的方法（不注入Acme通用类）
    /// </summary>
    /// <param name="_services">IServiceCollection</param>
    /// <param name="_connectionString">数据库连接字符</param>
    public static void AddSingletonBaseService<TDbContext>(
        this IServiceCollection _services,
        string _connectionString) where TDbContext : DbContext
    {
        _services.AddDbContext<TDbContext>(option => option.UseSqlServer(_connectionString), ServiceLifetime.Singleton);
        _services.AddSingleton(typeof(IBaseService<>), typeof(BaseService<>));
    }
}
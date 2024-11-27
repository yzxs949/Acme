using Microsoft.Extensions.DependencyInjection;
namespace Acme.Dapper;

/// <summary>
/// 拓展类
/// </summary>
public static class AcmeDapperServiceCollectionExtensions
{
    /// <summary>
    /// 注册Acme.Dapper的方法
    /// </summary>
    /// <param name="_services">IServiceCollection</param>
    /// <param name="_connectionString">数据库连接字符</param>
    public static void AddAcmeDapperServer(
        this IServiceCollection _services,
        string _connectionString)
    {
        _services.AddTransient(IDapperHelper => new Extensions(_connectionString));
        _services.AddScoped(typeof(ILog<>), typeof(Log<>));
        _services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        _services.AddAcmeBuilderServer();
    }
}
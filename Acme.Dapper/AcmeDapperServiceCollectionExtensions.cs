using Acme.Dapper.Extension;
using Acme.Dapper.Repository;
using Acme.Nlogs;
using Microsoft.Extensions.DependencyInjection;
using System;
namespace Acme.Dapper;

/// <summary>
/// 拓展类
/// </summary>
public static class AcmeDapperServiceCollectionExtensions
{
    /// <summary>
    /// 注册Acme.Dapper相关服务
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="connectionString">数据库连接字符串</param>
    /// <exception cref="ArgumentNullException">当 connectionString 为空时抛出</exception>
    public static void AddAcmeDapper(this IServiceCollection services, string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentNullException(nameof(connectionString), "数据库连接字符串不能为空");
        services.AddTransient<IExtensions>(provider => new Extensions(connectionString));
        services.AddScoped(typeof(ILog<>), typeof(Log<>));
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddAcmeBuilderServer();
    }
}
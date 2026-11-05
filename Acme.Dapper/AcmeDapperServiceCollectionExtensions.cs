using Acme.Dapper.Extension;
using Acme.Dapper.Repository;
using Acme.Dapper.SqlSplice;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
    /// <exception cref="ArgumentNullException">当 services 或 connectionString 为空时抛出</exception>
    public static void AddAcmeDapper(this IServiceCollection services, string connectionString)
    {
        ArgumentNullException.ThrowIfNull(services);

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentNullException(nameof(connectionString), "数据库连接字符串不能为空");

        // 无状态，可全局复用
        services.TryAddSingleton<IExtensions>(_ => new Extensions(connectionString));
        services.TryAddSingleton(typeof(ISplicing<>), typeof(Splicing<>));
        services.TryAddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddAcmeBuilderServer();
    }
}

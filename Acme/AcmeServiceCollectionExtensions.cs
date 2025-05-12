using Acme.Cookie;
using Acme.Sessions;
using Microsoft.Extensions.DependencyInjection;

namespace Acme;

/// <summary>
/// 基于Acme拓展方法
/// </summary>
public static class AcmeServiceCollectionExtensions
{
    /// <summary>
    /// 添加Acme注册服务
    /// </summary>
    /// <param name="services"></param>
    public static void AddAcmeBuilderServer(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddTransient<IHttpCookieService, HttpCookieService>();
        services.AddSession();
        services.AddTransient<IHttpSessionService, HttpSessionService>();
    }
}
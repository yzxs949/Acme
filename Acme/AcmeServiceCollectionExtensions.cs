using Acme.Cookie;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
    /// <param name="logConfig"></param>
    public static void AddAcmeBuilderServer(this IServiceCollection services, LogConfig logConfig = null)
    {
        if (logConfig != null)
        {
            services.AddTransient<LogConfig>(opt => logConfig);
            services.AddTransient(typeof(ILog<>), typeof(Log<>));
        }
        services.AddHttpContextAccessor();
        services.AddTransient<IHttpCookieService, HttpCookieService>();
        services.AddSession();
        services.AddTransient<IHttpSessionService, HttpSessionService>();
    }
}
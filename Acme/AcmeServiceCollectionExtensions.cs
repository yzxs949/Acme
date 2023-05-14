using Microsoft.Extensions.DependencyInjection;
using Acme.Cookie;
namespace Acme;

/// <summary>
/// 基于Acme依赖注入
/// </summary>
public static class AcmeServiceCollectionExtensions
{
    
    /// <summary>
    /// 添加Cookie服务
    /// </summary>
    /// <param name="services"></param>
    public static void AddHttpCookieServer(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();//配置Cookie HttpContext
        services.AddTransient<IHttpCookieService, HttpCookieService>();//IOC配置 方便项目中使用
    }
}

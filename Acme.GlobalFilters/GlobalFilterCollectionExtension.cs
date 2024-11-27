using Microsoft.AspNetCore.Builder;

namespace Acme.GlobalFilters;

/// <summary>
/// 全局过滤器服务类
/// </summary>
public static class GlobalFilterCollectionExtension
{
    /// <summary>
    /// 添加全局异常过滤器
    /// </summary>
    /// <param name="services"></param>
    public static void AddExceptionServiceCollection(this IServiceCollection services)
    {
        services.AddServiceCollection<ExceptionFilter>();
    }

    /// <summary>
    /// 添加自定义过滤器
    /// </summary>
    /// <typeparam name="T">过滤器实体</typeparam>
    /// <param name="services"></param>
    public static void AddServiceCollection<T>(this IServiceCollection services) where T : class
    {
        services.AddControllersWithViews(options => { options.Filters.Add(typeof(T)); });
    }

    /// <summary>
    /// 添加获取当前地址的中间件
    /// </summary>
    /// <param name="application"></param>
    public static void AddWebAppCurrentUrlFilter(this WebApplication application) 
    {
        application.UseMiddleware<CurrentUrlFilter>();
    }
}

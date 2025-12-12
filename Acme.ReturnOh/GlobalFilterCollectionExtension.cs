using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Acme.ReturnOh;

/// <summary>
/// 全局过滤器服务类
/// </summary>
public static class GlobalFilterCollectionExtension
{
    /// <summary>
    /// 配置 Swagger 选项，添加文档信息和 XML 注释文件
    /// </summary>
    /// <param name="services">IServiceCollection 实例</param>
    /// <param name="title">Swagger 文档的标题</param>
    /// <param name="version">Swagger 文档的版本，默认为 "v1"</param>
    /// <param name="otherFiles">其他需要包含的 XML 注释文件列表，默认为 null</param>
    /// <returns>配置后的 IServiceCollection 实例</returns>
    /// <remarks>
    /// 该方法会自动包含当前程序集的 XML 注释文件，并在需要时包含其他指定的 XML 注释文件
    /// </remarks>
    public static IServiceCollection ConfigureSwaggerOptions(this IServiceCollection services, string title, string version = "v1", List<string>? otherFiles = null)
    {
        SwaggerGenServiceCollectionExtensions.AddSwaggerGen(services, delegate (SwaggerGenOptions options)
        {
            SwaggerGenOptionsExtensions.SwaggerDoc(options, version, new OpenApiInfo
            {
                Title = title,
                Version = version
            });
            string path = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
            string text = Path.Combine(AppContext.BaseDirectory, path);
            SwaggerGenOptionsExtensions.IncludeXmlComments(options, text, true);
            if (otherFiles is not null)
            {
                foreach (string str in otherFiles)
                {
                    string text2 = Path.Combine(AppContext.BaseDirectory, str + ".xml");
                    SwaggerGenOptionsExtensions.IncludeXmlComments(options, text2, false);
                }
            }
        });
        return services;
    }

    /// <summary>
    /// 配置API行为选项，自定义模型验证失败时的响应格式
    /// </summary>
    /// <param name="services">服务集合，用于注册API行为选项配置</param>
    /// <returns>返回服务集合，以便进行链式调用</returns>
    public static IServiceCollection ConfigureApiBehaviorOptions(this IServiceCollection services)
    {
        services.Configure(static delegate (ApiBehaviorOptions options)
        {
            options.InvalidModelStateResponseFactory = delegate (ActionContext context)
            {
                string? msg = (from e in context.ModelState.Values.SelectMany((ModelStateEntry v) => v.Errors) select e.ErrorMessage).FirstOrDefault<string>();
                return new BadRequestObjectResult(new Oh(400, msg ?? "", null));
            };
        });
        return services;
    }

    /// <summary>
    /// 配置MVC选项，添加自定义的异常过滤器
    /// </summary>
    /// <param name="services">服务集合，用于注册 MVC 选项配置</param>
    /// <returns>返回服务集合，以便进行链式调用</returns>
    public static IServiceCollection ConfigureMvcOptions(this IServiceCollection services)
    {
        services.Configure(delegate (MvcOptions options)
        {
            options.Filters.Add<ExceptionFilter>();
        });
        return services;
    }

    /// <summary>
    /// 配置MVC选项，添加自定义的异常过滤器
    /// </summary>
    /// <param name="services">服务集合，用于注册 MVC 选项配置</param>
    /// <returns>返回服务集合，以便进行链式调用</returns>
    public static IServiceCollection ConfigureMvcOptions<T>(this IServiceCollection services) where T : IAsyncExceptionFilter
    {
        services.Configure(delegate (MvcOptions options)
        {
            options.Filters.Add<T>();
        });
        return services;
    }

    /// <summary>
    /// 配置ExceptionFilter异常过滤器
    /// </summary>
    /// <param name="services">服务集合，用于ExceptionFilter异常过滤器</param>
    /// <returns>返回服务集合，以便进行链式调用</returns>
    public static IServiceCollection AddExceptionFilterService(this IServiceCollection services)
    {
        services.ConfigureMvcOptions<ExceptionFilter>();
        return services;
    }

    /// <summary>
    /// 配置ExceptionFilter异常过滤器
    /// </summary>
    /// <param name="services">服务集合，用于ExceptionFilter异常过滤器</param>
    /// <returns>返回服务集合，以便进行链式调用</returns>
    public static IServiceCollection AddExceptionFilterService<TExceptionFilter>(
        this IServiceCollection services)
        where TExceptionFilter : IAsyncExceptionFilter
    {
        services.ConfigureMvcOptions<TExceptionFilter>();
        return services;
    }
}

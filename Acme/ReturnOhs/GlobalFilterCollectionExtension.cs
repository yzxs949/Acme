using Acme.Constant;
using Acme.Ohs;
using Acme.ReturnOh.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Acme.ReturnOhs;

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
    /// <param name="isActionOrder">是否方法排序（默认添加）</param>
    /// <param name="enableJwtBearerAuth">是否添加JWT认证（默认添加）</param>
    /// <returns>配置后的 IServiceCollection 实例</returns>
    /// <remarks>
    /// 该方法会自动包含当前程序集的 XML 注释文件，并在需要时包含其他指定的 XML 注释文件
    /// </remarks>
    public static IServiceCollection ConfigureSwaggerOptions(
        this IServiceCollection services,
        string title,
        string version = "v1",
        List<string> otherFiles = null,
        bool isActionOrder = true,
        bool enableJwtBearerAuth = true)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(version, new OpenApiInfo
            {
                Title = title,
                Version = version
            });
            ConfigureSwaggerGenCommon(options, otherFiles, isActionOrder, enableJwtBearerAuth);
        });
        return services;
    }

    /// <summary>
    /// 配置 Swagger 选项，添加文档信息和 XML 注释文件
    /// </summary>
    /// <param name="services">IServiceCollection 实例</param>
    /// <param name="swaggerDocs">多个版本集合</param>
    /// <param name="otherFiles">其他需要包含的 XML 注释文件列表，默认为 null</param>
    /// <param name="isActionOrder">是否方法排序（默认添加）</param>
    /// <param name="enableJwtBearerAuth">是否添加JWT认证（默认添加）</param>
    /// <returns>配置后的 IServiceCollection 实例</returns>
    /// <remarks>
    /// 该方法会自动包含当前程序集的 XML 注释文件，并在需要时包含其他指定的 XML 注释文件
    /// </remarks>
    public static IServiceCollection ConfigureSwaggerOptions(
       this IServiceCollection services,
       List<SwaggerDocDto> swaggerDocs,
       List<string> otherFiles = null,
       bool isActionOrder = true,
       bool enableJwtBearerAuth = true)
    {
        services.AddSwaggerGen(options =>
        {
            foreach (SwaggerDocDto swaggerDoc in swaggerDocs)
            {
                options.SwaggerDoc(swaggerDoc.Version, new OpenApiInfo
                {
                    Title = swaggerDoc.Title,
                    Version = swaggerDoc.Version
                });
            }
            ConfigureSwaggerGenCommon(options, otherFiles, isActionOrder, enableJwtBearerAuth);
        });
        return services;
    }

    /// <summary>
    /// 单/多版本共用的 SwaggerGen 配置：XML 注释、排序、JWT 安全定义与要求
    /// </summary>
    private static void ConfigureSwaggerGenCommon(
        SwaggerGenOptions options,
        List<string> otherFiles,
        bool isActionOrder,
        bool enableJwtBearerAuth)
    {
        string path = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, path), true);
        if (otherFiles is not null)
        {
            foreach (string str in otherFiles)
            {
                string text2 = Path.Combine(AppContext.BaseDirectory, str + ".xml");
                options.IncludeXmlComments(text2, true);
            }
        }
        if (isActionOrder)
        {
            options.OrderActionsBy(o => o.RelativePath);
        }
        if (enableJwtBearerAuth)
        {
            options.AddSecurityDefinition(CV.SecuritySchemeId, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = CV.SecuritySchemeId,
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Description = "格式：Bearer {token}"
            });
        }
        options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
        {
            { new OpenApiSecuritySchemeReference("Bearer"), new List<string>() }
        });
    }

    /// <summary>
    /// 启用并配置多版本Swagger中间件，支持同时展示多个版本的API文档
    /// </summary>
    /// <param name="app">WebApplication 应用程序实例</param>
    /// <param name="swaggerDocs">多版本文档配置集合，包含每个版本的标题与版本号</param>
    /// <param name="routePrefix">Swagger访问路由前缀
    /// 空字符串：根域名直接访问（https://xxx:port）
    /// swagger：通过 https://xxx:port/swagger 访问
    /// </param>
    /// <returns>WebApplication 实例，支持链式调用</returns>
    public static WebApplication UseBathSwagger(this WebApplication app, List<SwaggerDocDto> swaggerDocs, string routePrefix = "")
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (SwaggerDocDto swaggerDoc in swaggerDocs)
            {
                string title = swaggerDoc.Title;
                string version = swaggerDoc.Version;
                options.SwaggerEndpoint($"/swagger/{version}/swagger.json", title);
            }
            options.RoutePrefix = routePrefix;
        });
        return app;
    }

    /// <summary>
    /// 启用并配置单个版本的Swagger中间件
    /// </summary>
    /// <param name="app">WebApplication 应用实例</param>
    /// <param name="title">Swagger界面显示的文档标题（如：后台管理API）</param>
    /// <param name="version">API版本号，默认值 v1</param>
    /// <param name="routePrefix">Swagger访问路由前缀
    /// 为空字符串：根域名直接访问（https://localhost:port）
    /// 填写 swagger：通过 https://localhost:port/swagger 访问
    /// </param>
    /// <returns>WebApplication 实例，支持链式调用</returns>
    public static WebApplication UseSingSwagger(
        this WebApplication app,
        string title,
        string version = "v1",
        string routePrefix = "")
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint($"/swagger/{version}/swagger.json", title);
            options.RoutePrefix = routePrefix;
        });
        return app;
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
                string msg = (from e in context.ModelState.Values.SelectMany(v => v.Errors) select e.ErrorMessage).FirstOrDefault<string>();
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

    /// <summary>
    /// 配置认证服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddOhAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        Oh result = new(401, "未授权，请登录后访问", null);
                        return context.Response.WriteAsJsonAsync(result);
                    }
                };
            });
        services.AddAuthorization();
        return services;
    }
}

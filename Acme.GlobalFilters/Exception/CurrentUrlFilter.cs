namespace Acme.GlobalFilters;

/// <summary>
/// CurrentUrl中间件
/// </summary>
public class CurrentUrlFilter
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="next"></param>
    public CurrentUrlFilter(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task Invoke(HttpContext context)
    {
        var endpoint = context.GetEndpoint() as RouteEndpoint;
        if (endpoint != null)
        {
            var controllerName = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>()?.ControllerName;
            var actionName = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>()?.ActionName;
            context.Items["ControllerName"] = controllerName;
            context.Items["ActionName"] = actionName;
            var currentUrl = $"{controllerName}/{actionName}";
            context.Items["CurrentUrl"] = currentUrl;
        }
        await _next(context);
    }
}

using Acme.Tools;

namespace Acme.GlobalFilters.Exceptions;

/// <summary>
/// 异常过滤器
/// </summary>
/// <remarks>
/// 依赖注入
/// </remarks>
/// <param name="log"></param>
public class ExceptionFilter(ILog<ExceptionFilter> log) : IExceptionFilter
{
    /// <summary>
    /// 日志类
    /// </summary>
    private readonly ILog<ExceptionFilter> _log = log;

    /// <summary>
    /// 输出页面Html
    /// </summary>
    private static string StringHtml => $@"<head><meta charset=""utf-8"" /></head><h3 style=""text-align:center;color:red;"">500：{CV.ErrorPrompt}</h3><p><hr/></p><p style=""text-align:center;color:red;"">@Message</p>";

    /// <summary>
    /// 异常处理方法
    /// </summary>
    /// <param name="context"></param>
    public void OnException(ExceptionContext context)
    {
        if (context.ActionDescriptor is ControllerActionDescriptor actionDescriptor && ExceptionFilter.IsPageRequest(actionDescriptor.MethodInfo.ReturnType))
        {
            HandleException(context.Exception, context.HttpContext.Response);
        }
        else
        {
            context.Result = new JsonResult(false.Fail($"{CV.ErrorPrompt}:{context.Exception.Message}"));
            _log.Error(context.Exception.Message);
        }
    }

    /// <summary>
    /// 页面处理方法
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="response"></param>
    private void HandleException(System.Exception exception, HttpResponse response)
    {
        response.ContentType = "text/html";
        _log.Error(exception.Message);
        response.WriteAsync(ExceptionFilter.StringHtml.Replace("@Message", exception.Message).ToUtf8());
    }

    /// <summary>
    /// 判断方法类型
    /// </summary>
    /// <param name="returnType"></param>
    /// <returns></returns>
    private static bool IsPageRequest(Type returnType)
    {
        return typeof(IActionResult).IsAssignableFrom(returnType)
            || typeof(Task<IActionResult>).IsAssignableFrom(returnType)
            || typeof(Task<ActionResult>).IsAssignableFrom(returnType)
            || typeof(ActionResult).IsAssignableFrom(returnType);
    }
}

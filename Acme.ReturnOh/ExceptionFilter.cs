using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Acme.ReturnOh;

/// <summary>
/// 异常过滤
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ExceptionFilter : Attribute, IAsyncExceptionFilter, IFilterMetadata
{
    /// <summary>
    /// 异常过滤
    /// </summary>
    /// <param name="context"></param>
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        if (context.ExceptionHandled)
            return;

        // 生产环境不暴露内部异常细节到客户端
        string userFriendlyMsg = context.Exception switch
        {
            ArgumentException => "请求参数错误",
            SystemException => CV.ErrorServer,
            _ => CV.ErrorPrompt,
        };

        int statusCode = context.Exception switch
        {
            ArgumentException => 400,
            _ => 500,
        };

        Oh result = new Oh(statusCode, userFriendlyMsg, null);
        context.ExceptionHandled = true;
        context.Result = new ObjectResult(result);
    }
}

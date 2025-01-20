using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Acme.ReturnOh;

/// <summary>
/// 异常过滤
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class ExceptionFilter : Attribute, IAsyncExceptionFilter, IFilterMetadata
{
    /// <summary>
    /// 异常过滤
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="NotImplementedException"></exception>
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        if (context.ExceptionHandled)
            return;
        Oh result = context.Exception switch
        {
            ArgumentException ex => new Oh(400, $"{ex.Message}{ex.InnerException?.Message}"),
            SystemException ex => new Oh(CV.NoCode, $"{ex.Message}{ex.InnerException?.Message}"),
            _ => new Oh(CV.NoCode, $"{context.Exception.Message}{context.Exception.InnerException?.Message}"),
        };
        await Task.Delay(20);
        context.ExceptionHandled = true;
        context.Result = new ObjectResult(result);
    }
}

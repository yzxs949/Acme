using Acme.Constant;
using Acme.Ohs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace Acme.ReturnOhs;

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
    public Task OnExceptionAsync(ExceptionContext context)
    {
        if (context.ExceptionHandled)
            return Task.CompletedTask;
        Oh result = context.Exception switch
        {
            // 参数校验异常由用户输入触发，可安全回显提示
            ArgumentException ex => new Oh(400, ex.Message),
            // 其余异常统一返回通用提示，避免泄露堆栈、路径、内部结构等敏感信息
            _ => Oh.Fail(CV.ErrorServer)
        };
        context.ExceptionHandled = true;
        context.Result = new ObjectResult(result);
        return Task.CompletedTask;
    }
}

using Acme.ReturnOh;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiDemo.Controllers;

/// <summary>
/// 天气
/// </summary>
[ApiExplorerSettings(GroupName = "v2")]
[Route("api/v2/WeatherForecast/[Action]")]
[ApiController]
public class WeatherForecastController : ControllerBase
{
    /// <summary>
    /// 获取天气预报
    /// </summary>
    /// <returns></returns>
    [HttpGet, AllowAnonymous]
    public Oh GetWeatherForecast()
    {
        Oh.SystemException("操作失败");
        return CV.NullValue.Success("获取成功");
    }
}

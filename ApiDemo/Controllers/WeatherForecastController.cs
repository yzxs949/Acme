using Acme.ReturnOh;
using ApiDemo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// 天气预报控制器
/// </summary>
[ApiController,ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/[controller]/[Action]")]
public class WeatherForecastController : ControllerBase
{
    /// <summary>
    /// 获取天气预报
    /// </summary>
    /// <returns></returns>
    [HttpGet, Authorize]
    public Oh GetWeatherForecast()
    {
        return CV.NullValue.Success("获取成功");
    }
}
using Acme.ReturnOh;
using Acme.ReturnOh.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

var swaggerConfigurs = new List<SwaggerDocDto>()
{
    new SwaggerDocDto("API 文档","v1"),
    new SwaggerDocDto("API 文档2", "v2"),
};
// 配置 Swagger
builder.Services.ConfigureSwaggerOptions(swaggerConfigurs, otherFiles: ["ApiDemo"]);

//builder.Services.ConfigureSwaggerOptions("API 文档2", "v2", otherFiles: ["ApiDemo"]);

// 配置 API 行为选项（参数验证）
builder.Services.ConfigureApiBehaviorOptions();

// 配置异常过滤器
builder.Services.AddExceptionFilterService();

// 添加 MVC
builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                Oh result = CV.NullValue.Fail(401, "未授权，请登录后访问");
                return context.Response.WriteAsJsonAsync(result);
            }
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseBathSwagger(swaggerConfigurs);
}

app.UseAuthorization();
app.MapControllers();
app.Run();
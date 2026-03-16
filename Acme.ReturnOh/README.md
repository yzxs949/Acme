# Acme.ReturnOh 使用文档

## 项目简介

Acme.ReturnOh 是一个用于 .NET 项目的通用返回参数处理库，提供了统一的 API 响应格式、全局异常处理、参数验证和 Swagger 配置功能。

### 主要功能
- 统一的 API 响应格式
- 全局异常处理
- 参数验证错误处理
- Swagger 文档配置

## 安装方法

### 通过 NuGet 安装

```bash
dotnet add package Acme.ReturnOh
```

### 支持的框架版本
- .NET 8.0
- .NET 9.0
- .NET 10.0

## 快速开始

### 1. 配置服务

在 `Program.cs` 文件中添加以下配置：

```csharp
using Acme.ReturnOh;

var builder = WebApplication.CreateBuilder(args);

// 配置 Swagger
builder.Services.ConfigureSwaggerOptions("API 文档");

// 配置 API 行为选项（参数验证）
builder.Services.ConfigureApiBehaviorOptions();

// 配置异常过滤器
builder.Services.AddExceptionFilterService();

// 添加 MVC
builder.Services.AddControllers();

var app = builder.Build();

// 启用 Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
```

### 2. 使用通用返回格式

在控制器中使用 `Oh` 类返回统一格式的响应：

```csharp
using Acme.ReturnOh;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    [HttpGet(Name = "GetWeatherForecast")]
    public Oh Get()
    {
        var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }).ToArray();
        
        return forecasts.Success("获取成功");
    }
}
```

## 核心功能

### 1. 通用返回格式

#### 基本结构

```csharp
public record Oh(int Code, string Msg, object? Data = null);
public record Oh<TEntity>(int Code, string Msg, TEntity? Data) where TEntity : class, new();
```

#### 常用方法

```csharp
// 成功响应
return data.Success(); // 默认消息："成功！"
return data.Success("操作成功"); // 自定义消息
return data.Success(200, "操作成功"); // 自定义状态码和消息

// 失败响应
return data.Fail(); // 默认消息："服务器错误！请联系管理员"
return data.Fail("操作失败"); // 自定义消息
return data.Fail(500, "操作失败"); // 自定义状态码和消息

// 自定义响应
return data.Custom(403, "权限不足"); // 自定义状态码和消息

// 布尔值转换
return isSuccess.IsSuccess("成功消息", "失败消息");
```

### 2. 异常处理

#### 全局异常过滤器

库会自动捕获并处理以下异常：
- `ArgumentException` - 返回 400 状态码
- `SystemException` - 返回 500 状态码
- 其他异常 - 返回 500 状态码

#### 手动抛出异常

```csharp
// 抛出普通异常
Oh<MyData>.Exception("操作失败");

// 抛出自定义异常
Oh<MyData>.ArgumentException("参数错误");
Oh<MyData>.SystemException("系统错误");
```

### 3. 参数验证

#### 自动处理

当使用 `[ApiController]` 特性时，参数验证错误会自动处理并返回统一格式的错误响应：

```csharp
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    [HttpPost]
    public Oh Create([FromBody] UserDto user)
    {
        // 如果 user 参数验证失败，会自动返回 400 状态码和错误消息
        return user.Success("创建成功");
    }
}

public class UserDto
{
    [Required(ErrorMessage = "用户名不能为空")]
    public string Username { get; set; }
    
    [Required(ErrorMessage = "密码不能为空")]
    [MinLength(6, ErrorMessage = "密码长度不能少于6位")]
    public string Password { get; set; }
}
```

### 4. Swagger 配置

#### 基本配置

```csharp
builder.Services.ConfigureSwaggerOptions("API 文档");
```

#### 高级配置

```csharp
// 自定义版本
builder.Services.ConfigureSwaggerOptions("API 文档", "v2");

// 包含其他 XML 注释文件
var otherXmlFiles = new List<string> { "MyProject.Model", "MyProject.Service" };
builder.Services.ConfigureSwaggerOptions("API 文档", "v1", otherXmlFiles);
```

## 高级用法

### 1. 自定义异常过滤器

```csharp
public class CustomExceptionFilter : Attribute, IAsyncExceptionFilter
{
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        if (context.ExceptionHandled)
            return;
        
        Oh result = context.Exception switch
        {
            MyCustomException ex =>
                new Oh(400, $"自定义异常：{ex.Message}"),
            _ =>
                Oh.Fail($"系统错误：{context.Exception.Message}"),
        };
        
        context.ExceptionHandled = true;
        context.Result = new ObjectResult(result);
        await Task.CompletedTask;
    }
}

// 注册自定义异常过滤器
builder.Services.AddExceptionFilterService<CustomExceptionFilter>();
```

### 2. 泛型返回类型

```csharp
[HttpGet("{id}")]
public Oh<UserDto> Get(int id)
{
    var user = _userService.GetById(id);
    if (user == null)
    {
        return new Oh<UserDto>(404, "用户不存在", null);
    }
    return user.Success<UserDto>("获取成功");
}
```

## 常量定义

库中定义了一些常用常量，可直接使用：

| 常量名 | 值 | 说明 |
|-------|-----|------|
| `CV.Yes` | "成功！" | 成功消息 |
| `CV.ErrorPrompt` | "系统错误！请联系管理员" | 系统错误提示 |
| `CV.ErrorServer` | "服务器错误！请联系管理员" | 服务器错误提示 |
| `CV.YesCode` | 200 | 成功状态码 |
| `CV.NoCode` | 500 | 失败状态码 |
| `CV.InsertYes` | "新增成功" | 新增成功消息 |
| `CV.InsertNo` | "新增失败" | 新增失败消息 |
| `CV.UpdateYes` | "更新成功" | 更新成功消息 |
| `CV.UpdateNo` | "更新失败" | 更新失败消息 |
| `CV.DeleteYes` | "删除成功" | 删除成功消息 |
| `CV.DeleteNo` | "删除失败" | 删除失败消息 |
| `CV.GetYes` | "获取成功" | 获取成功消息 |
| `CV.GetNo` | "获取失败" | 获取失败消息 |

## 示例代码

### 完整控制器示例

```csharp
using Acme.ReturnOh;
using Microsoft.AspNetCore.Mvc;

namespace MyProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public Oh GetAll()
        {
            var users = _userService.GetAll();
            return users.Success(CV.GetYes);
        }

        [HttpGet("{id}")]
        public Oh GetById(int id)
        {
            var user = _userService.GetById(id);
            if (user == null)
            {
                return CV.NullValue.Fail("用户不存在");
            }
            return user.Success(CV.GetYes);
        }

        [HttpPost]
        public Oh Create([FromBody] UserDto userDto)
        {
            var result = _userService.Create(userDto);
            return result.IsSuccess(CV.InsertYes, CV.InsertNo);
        }

        [HttpPut("{id}")]
        public Oh Update(int id, [FromBody] UserDto userDto)
        {
            var result = _userService.Update(id, userDto);
            return result.IsSuccess(CV.UpdateYes, CV.UpdateNo);
        }

        [HttpDelete("{id}")]
        public Oh Delete(int id)
        {
            var result = _userService.Delete(id);
            return result.IsSuccess(CV.DeleteYes, CV.DeleteNo);
        }
    }
}
```

### 服务层示例

```csharp
using Acme.ReturnOh;

namespace MyProject.Services
{
    public class UserService : IUserService
    {
        private readonly List<User> _users = new();

        public List<User> GetAll()
        {
            return _users;
        }

        public User GetById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public bool Create(UserDto userDto)
        {
            // 业务逻辑
            if (string.IsNullOrEmpty(userDto.Username))
            {
                Oh<User>.Exception("用户名不能为空");
            }
            
            _users.Add(new User
            {
                Id = _users.Count + 1,
                Username = userDto.Username,
                Email = userDto.Email
            });
            
            return true;
        }

        public bool Update(int id, UserDto userDto)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                Oh<User>.Exception("用户不存在");
            }
            
            user.Username = userDto.Username;
            user.Email = userDto.Email;
            
            return true;
        }

        public bool Delete(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                Oh<User>.Exception("用户不存在");
            }
            
            return _users.Remove(user);
        }
    }
}
```

## 常见问题

### 1. 如何自定义异常处理逻辑？

可以创建自定义异常过滤器并通过 `AddExceptionFilterService<T>` 方法注册。

### 2. 如何修改默认的成功/失败消息？

可以在调用 `Success`、`Fail` 方法时传入自定义消息，或直接使用 `CV` 类中定义的常量。

### 3. 如何添加自定义状态码？

可以使用 `Custom` 方法创建自定义状态码的响应：

```csharp
return data.Custom(403, "权限不足");
```

### 4. 如何在 Swagger 中显示 XML 注释？

确保项目已启用 XML 文档生成，并在 `ConfigureSwaggerOptions` 方法中包含相应的 XML 文件。

## 版本历史

- 1.7.3 - 当前版本
- 1.7.2 - 修复了一些小问题
- 1.7.1 - 优化了异常处理逻辑
- 1.7.0 - 增加了泛型返回类型支持
- 1.5.0 - 初始版本

## 总结

Acme.ReturnOh 是一个简单而强大的库，为 .NET 项目提供了统一的 API 响应格式、异常处理和 Swagger 配置功能。通过使用这个库，您可以：

1. 统一 API 响应格式，提高前端开发效率
2. 自动处理异常，减少重复代码
3. 简化参数验证错误处理
4. 快速配置 Swagger 文档

希望这个库能帮助您更高效地开发 .NET 项目！
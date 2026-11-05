# Acme

面向 .NET 8 / 9 / 10 的轻量开发套件：通用工具库、Dapper 数据访问、统一响应体与全局过滤器。

详细文档：[Acme](Acme/README.md) · [Acme.Dapper](Acme.Dapper/README.md) · [示例 API](ApiDemo/Program.cs)

[![NuGet Acme](https://img.shields.io/nuget/v/Acme.svg)](https://www.nuget.org/packages/Acme)
[![NuGet Acme.Dapper](https://img.shields.io/nuget/v/Acme.Dapper.svg)](https://www.nuget.org/packages/Acme.Dapper)
[![.NET](https://img.shields.io/badge/.NET-8.0%20%7C%209.0%20%7C%2010.0-512BD4.svg)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](https://opensource.org/licenses/MIT)
[![Gitee](https://img.shields.io/badge/Gitee-yzxs949%2Facme--v2-C71D23.svg)](https://gitee.com/yzxs949/acme-v2)

## 目录

- [项目组成](#项目组成)
- [快速开始](#快速开始)
- [能力速览](#能力速览)
- [统一响应体与异常处理](#统一响应体与异常处理)
- [数据访问](#数据访问)
- [仓库结构](#仓库结构)
- [环境与依赖](#环境与依赖)
- [构建](#构建)
- [版本一览](#版本一览)
- [贡献](#贡献)
- [许可证](#许可证)

## 项目组成

Acme 不是一个大而全的框架，而是可以独立引用的包，加上一个示例 API。

| 包 / 项目 | 定位 | 目标框架 | 版本 | 文档 |
|---|---|---|---|---|
| `Acme` | 通用工具库：JSON、类型转换、HTTP、MD5、Session / Cookie、拼音、Excel、验证码、`Oh`、配置读取、Swagger / 过滤器 | net8.0 / net9.0 / net10.0 | 5.6.8.1 | [详细文档](Acme/README.md) · [NuGet](https://www.nuget.org/packages/Acme) |
| `Acme.Dapper` | 基于 Dapper 的 SQL Server 数据访问：仓储、原生 SQL、分页、事务 | net8.0 / net9.0 / net10.0 | 5.7.0 | [详细文档](Acme.Dapper/README.md) · [NuGet](https://www.nuget.org/packages/Acme.Dapper) |
| `ApiDemo` | 示例 Web API：多版本 Swagger + 异常过滤 + `Oh` 返回 | net8.0 | — | [Program.cs](ApiDemo/Program.cs) |

设计取向：

- 静态扩展方法优先，不接管宿主应用的 DI 容器与配置体系；
- 每个包独立引用，用不到的能力不会被拖进来；
- 默认安全：Cookie 默认 `HttpOnly` + `SameSite=Lax`，随机码使用加密安全随机数，异常不向调用方泄露堆栈。

## 快速开始

按需安装：

```bash
dotnet add package Acme            # 通用工具 + 统一响应 / 过滤器 / Swagger
dotnet add package Acme.Dapper     # 需要数据库时
```

在 `Program.cs` 中注册（每一项都可选）：

```csharp
using Acme;
using Acme.Dapper;
using Acme.ReturnOhs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAcmeBuilderServer();          // HttpContextAccessor / Session / Cookie
builder.Services.AddAcmeDapper(builder.Configuration.GetConnectionString("Default")!);
builder.Services.AddExceptionFilterService();     // 异常统一转 Oh
builder.Services.ConfigureApiBehaviorOptions();   // 模型验证失败也返回 Oh
builder.Services.ConfigureSwaggerOptions("API 文档");
builder.Services.AddOhAuthentication();           // 可选：JWT，401 统一返回 Oh
builder.Services.AddControllers();

var app = builder.Build();

app.UseSession();                                 // 用到 Session 时必须
app.UseSingSwagger("API 文档", "v1", "swagger");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
```

`AddAcmeBuilderServer()` 与 `AddAcmeDapper()` 都返回 `void`，不支持链式调用。

工具方法不需要注册，直接调用：

```csharp
using Acme.Tools;

string json = new { Name = "张三", Age = 25 }.ToJson();
var user = json.ToEntity<User>();
int count = "123".ToInt();
bool ok = "user@example.com".RegexVerification(@"^[\w.+-]+@[\w.-]+\.\w+$");
string code = VerCode.GetCode(4);
```

## 能力速览

| 能力 | 入口 | 详细文档 |
|---|---|---|
| JSON 序列化 / 反序列化 | `ToJson()`、`ToEntity<T>()`、`Json.ToJObject()` | [Acme/README.md](Acme/README.md#json-处理) |
| 类型转换 / 路径 / 时间 | `ConvHelper` 扩展方法、`ToRandomCode`、`GetTimeStamp` | [Acme/README.md](Acme/README.md#数据类型转换) |
| HTTP 请求 | `HttpHelper.HttpClientGetAsync` / `HttpClientPostAsync`（保留同步兼容版） | [Acme/README.md](Acme/README.md#http-请求) |
| MD5 | `MD5Encryption.Encrypt` / `Compare` | [Acme/README.md](Acme/README.md#加密工具) |
| Session | `IHttpSessionService` | [Acme/README.md](Acme/README.md#session-管理) |
| Cookie | `IHttpCookieService`（默认 `HttpOnly`） | [Acme/README.md](Acme/README.md#cookie-管理) |
| 拼音 | `Pinyin.GetInitials` / `GetPinyin` / `GetChineseText` | [Acme/README.md](Acme/README.md#拼音处理) |
| Excel | `NpolExcelService<T>`、`NpolExcelHelper<T>`（`InputExcel` / `ExportExcel`） | [Acme/README.md](Acme/README.md#excel-处理) |
| 验证码 | `VerCode`（数字、算式、图片） | [Acme/README.md](Acme/README.md#验证码) |
| 常用校验 | `VerificationHelper`（`IsNotNull`、`IsDefault`、`RegexVerification`） | [Acme/README.md](Acme/README.md#验证工具) |
| 统一返回体 | `Oh` / `Oh<T>` + `Success()`、`Fail()`、`Custom()` | 本文下方 |
| Swagger / JWT / 过滤器 | `Acme.ReturnOhs` 下的 `ConfigureSwaggerOptions`、`AddExceptionFilterService`、`AddOhAuthentication` | 本文下方 |
| 配置读取 | `App.GetValue`、`JsonFile` | [Acme/README.md](Acme/README.md#配置管理) |
| 数据访问 | `IRepository<T>`、`IExtensions`、`ISplicing<T>` | [Acme.Dapper/README.md](Acme.Dapper/README.md) |

## 统一响应体与异常处理

`Oh` 定义在 `Acme.Ohs`，是 `record`，非泛型版本继承自 `Oh<object>`：

```csharp
public record Oh<TEntity>(int Code, string Msg, TEntity Data) where TEntity : class;
public record Oh(int Code, string Msg, object Data = null) : Oh<object>(Code, Msg, Data);
```

```csharp
using Acme.Ohs;
using Acme.Constant;

return Ok(user.Success());                       // { Code: 200, Msg: "成功！", Data: {...} }
return Ok(user.Success("创建成功"));
return BadRequest(Oh.Fail("用户不存在"));          // { Code: 500, ... }
return StatusCode(403, flag.IsSuccess("有权限", "无权限"));

// 常量：CV.YesCode(200)、CV.NoCode(500)、CV.ErrorServer
```

`AddExceptionFilterService()` 注册的过滤器（`Acme.ReturnOhs.ExceptionFilter`）统一把异常转成 `Oh`：

| 异常 | 返回 |
|---|---|
| `ArgumentException` | 400 + 异常消息（用户输入类问题，可安全回显） |
| 其他异常 | 500 + `CV.ErrorServer`（不暴露异常细节） |

需要自定义时用泛型重载注册自己的 `IAsyncExceptionFilter`：

```csharp
builder.Services.AddExceptionFilterService<MyExceptionFilter>();
```

Swagger 与 JWT：

```csharp
using Acme.ReturnOhs;
using Acme.ReturnOh.Dtos;   // SwaggerDocDto 的命名空间

// 单版本
builder.Services.ConfigureSwaggerOptions("API 文档", "v1", otherFiles: ["ApiDemo"]);
app.UseSingSwagger("API 文档", "v1", routePrefix: "swagger");

// 多版本
var docs = new List<SwaggerDocDto> { new("API 文档", "v1"), new("业务 API", "v2") };
builder.Services.ConfigureSwaggerOptions(docs);
app.UseBathSwagger(docs, routePrefix: "swagger");
```

- `otherFiles` 是其他程序集的 XML 注释文件名（不含 `.xml`），对应项目需开启 `GenerateDocumentationFile`；
- `ConfigureSwaggerOptions` 默认按路由排序并添加 JWT 安全定义，可用 `isActionOrder: false` / `enableJwtBearerAuth: false` 关闭；
- `AddOhAuthentication()` 注册 JWT Bearer 与授权策略，并把 401 输出为 `Oh` JSON。

## 数据访问

```csharp
using Acme.Dapper.Repository;
using Acme.Dapper.Enums;
using Acme.Dapper.Tools;

public class UserService(IRepository<User> repo)
{
    public Task<User> Get(int id, CancellationToken ct)
        => repo.GetInfoAsync(id, ct);

    public Task<PageList<User>> Page(CancellationToken ct)
        => repo.GetPageListBySqlAsync<User>("SELECT * FROM Users WHERE Status = 1", 1, 20, "Id", OrderType.DESC, ct);
}
```

- 仅支持 SQL Server：连接层固定 `Microsoft.Data.SqlClient`，SQL 组装全部是 T-SQL 方言；
- 实体要求 `class` + 无参构造函数，属性名即列名；
- 主键识别顺序为 `[Key]` → `[ExplicitKey]` → 约定的 `Id`；
- 实体映射、局部更新、事务、分页与常见坑见 [Acme.Dapper/README.md](Acme.Dapper/README.md)。

## 仓库结构

```text
acme-v2/
├─ Acme/                  通用工具库（NuGet: Acme）
│  ├─ Tools/              Json、ConvHelper、HttpHelper、MD5、VerCode…
│  ├─ Sessions/           IHttpSessionService
│  ├─ Cookie/             IHttpCookieService
│  ├─ Pinyins/            Pinyin
│  ├─ Excel/              NpolExcelService
│  ├─ Ohs/                Oh / OhExtension
│  ├─ ReturnOhs/          异常过滤、Swagger、JWT 扩展（命名空间 Acme.ReturnOhs）
│  └─ Apps/               App（配置读取）
├─ Acme.Dapper/           Dapper 数据访问（NuGet: Acme.Dapper）
├─ ApiDemo/               示例 Web API
└─ Acme.sln
```

> `ApiDemo` 仍引用 NuGet 上的 `Acme.ReturnOh 1.7.4`；该独立包的源码已不在本仓库，等价能力保留在 `Acme` 包的 `Acme.ReturnOhs` 命名空间中。

## 环境与依赖

| 项目 | 要求 |
|---|---|
| .NET | SDK 8.0 / 9.0 / 10.0 |
| 数据库 | SQL Server 2012+（仅 `Acme.Dapper` 需要） |
| 平台 | 跨平台；验证码绘图依赖 `ZKWeb.System.Drawing` |

主要依赖：

| 包 | 关键依赖 |
|---|---|
| `Acme` | Newtonsoft.Json 13.0.4、NPOI 2.8.1、ZKWeb.System.Drawing 4.0.1、Swashbuckle 10.3.1、JwtBearer 8.0.33 / 9.0.22 / 10.0.14 |
| `Acme.Dapper` | Dapper 2.1.89、Dapper.Contrib 2.0.78、Microsoft.Data.SqlClient 7.1.1、Microsoft.Extensions.DependencyInjection 10.0.14 |

## 构建

```bash
dotnet build Acme.sln
```

当前仓库 `Acme`、`Acme.Dapper`、`ApiDemo` 三个项目均可编译，`dotnet build Acme.sln` 为 0 警告 0 错误。

单独构建：

```bash
dotnet build Acme/Acme.csproj
dotnet build Acme.Dapper/Acme.Dapper.csproj
dotnet build ApiDemo/ApiDemo.csproj
```

## 版本一览

| 包 | 版本 | 许可证 | 详细文档 |
|---|---|---|---|
| `Acme` | 5.6.8.1 | MIT | [Acme/README.md](Acme/README.md) |
| `Acme.Dapper` | 5.7.0 | MIT | [Acme.Dapper/README.md](Acme.Dapper/README.md) |

发布说明见各 csproj 中的 `PackageReleaseNotes`。

## 贡献

欢迎通过 [Gitee Issues](https://gitee.com/yzxs949/acme-v2/issues) 反馈问题或提交 Pull Request。

1. Fork 本仓库
2. 新建分支：`git checkout -b feature/your-feature`
3. 提交改动：`git commit -m "feat: your feature"`
4. 推送分支：`git push origin feature/your-feature`
5. 提交 Pull Request

提交前请确保 `dotnet build Acme.sln` 通过。

- 仓库：<https://gitee.com/yzxs949/acme-v2>
- 邮箱：<yzxs949@163.com>
- 公众号：.NET修仙日记

## 许可证

本项目基于 [MIT License](https://opensource.org/licenses/MIT) 发布，Copyright yzxs。

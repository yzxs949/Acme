﻿# Acme

> 极简 .NET 开发，高效触手可及

[![NuGet](https://img.shields.io/nuget/v/Acme.svg)](https://www.nuget.org/packages/Acme)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Acme.svg)](https://www.nuget.org/packages/Acme)
[![.NET](https://img.shields.io/badge/.NET-8.0%20%7C%209.0%20%7C%2010.0-512BD4.svg)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)
[![Gitee](https://img.shields.io/badge/Gitee-yzxs949%2Facme-C71D23.svg)](https://gitee.com/yzxs949/acme)

Acme 是一个面向 **.NET 8 / 9 / 10** 的轻量级工具类库，聚合了日常开发中最常用的能力：JSON、类型转换、HTTP、加密、Session / Cookie、拼音、Excel、验证码、统一响应体、Swagger / JWT 与配置读取等。开箱即用，帮助你专注于业务本身。

---

## 特性

- **零负担**：大量核心能力以静态扩展方法提供，随取随用
- **多框架**：单个包同时支持 `net8.0` / `net9.0` / `net10.0`
- **模块化**：按需引用，互不耦合
- **安全默认**：Cookie 默认 `HttpOnly`、验证码使用加密安全随机数、异常详情不外泄
- **依赖注入友好**：一行 `AddAcmeBuilderServer()` 即可注册 Session / Cookie 服务
- **中文文档**：完整的 XML 注释，IDE 智能提示友好

## 支持框架

| 目标框架 | 标识 |
| :------- | :--- |
| .NET 8   | `net8.0`  |
| .NET 9   | `net9.0`  |
| .NET 10  | `net10.0` |

## 安装

使用 .NET CLI：

```bash
dotnet add package Acme
```

或使用 Package Manager：

```powershell
Install-Package Acme
```

## 快速开始

### 1. 注册服务（Session / Cookie）

```csharp
using Acme;

var builder = WebApplication.CreateBuilder(args);

// 注册 HttpContextAccessor、Session、Cookie、Session 服务
builder.Services.AddAcmeBuilderServer();

var app = builder.Build();

// 使用 Session 时必须启用会话中间件
app.UseSession();
app.Run();
```

### 2. 直接使用工具方法

```csharp
using Acme.Tools;

// 序列化 / 反序列化
string json = new { Name = "张三", Age = 25 }.ToJson();
var user = json.ToEntity<User>();

// 安全类型转换
int number = "123".ToInt();          // 123
int fallback = "abc".ToInt(0);       // 0

// MD5 加密
string hash = MD5Encryption.Encrypt("password123");
bool ok = MD5Encryption.Compare("password123", hash);
```

---

## 功能模块

| 模块 | 命名空间 | 说明 |
| :--- | :------- | :--- |
| [JSON 处理](#json-处理) | `Acme.Tools` | 基于 Newtonsoft.Json 的序列化/反序列化 |
| [类型转换](#数据类型转换) | `Acme.Tools` | 字符串安全转换、路径、时间、Base64、随机码 |
| [HTTP 请求](#http-请求) | `Acme.Tools` | 健壮的 HttpClient 封装 |
| [加密工具](#加密工具) | `Acme.Tools` | MD5 加密与校验 |
| [Session 管理](#session-管理) | `Acme.Sessions` | HTTP Session 读写 |
| [Cookie 管理](#cookie-管理) | `Acme.Cookie` | HTTP Cookie 读写（安全默认） |
| [拼音处理](#拼音处理) | `Acme.Pinyins` | 中文转拼音 / 首字母 |
| [Excel 处理](#excel-处理) | `Acme.Excel` / `Acme.Tools` | 基于 NPOI 的导入导出 |
| [验证码](#验证码) | `Acme.Tools` | 数字 / 算式 / 图片验证码 |
| [验证工具](#验证工具) | `Acme.Tools` | 空值、Guid、正则校验 |
| [通用返回对象](#通用返回对象) | `Acme.Ohs` / `Acme.Constant` | 统一 API 响应体 `Oh` |
| [全局过滤器与 Swagger](#全局过滤器与-swagger) | `Acme.ReturnOhs` | 异常过滤、Swagger、JWT |
| [配置管理](#配置管理) | `Acme.Apps` / `Acme.JsonFiles` | JSON 配置读取与路径 |

### JSON 处理

基于 `Newtonsoft.Json`，提供对象与 JSON 字符串互转。

| 方法 | 说明 |
| :--- | :--- |
| `obj.ToJson()` | 序列化为 JSON 字符串 |
| `str.ToEntity<T>()` | 反序列化为实体 |
| `Json.ToJObject(str)` | 反序列化为 `JObject` |

```csharp
using Acme.Tools;

var user = new { Name = "张三", Age = 25, Address = new { City = "北京" } };
string json = user.ToJson();

var entity = json.ToEntity<User>();
var jobj = Json.ToJObject(json);
```

### 数据类型转换

`ConvHelper` 提供字符串到常见类型的安全转换及多种实用工具。

| 方法 | 说明 |
| :--- | :--- |
| `str.ToInt(default)` / `str.ToShort(default)` | 安全转换为 `int` / `short` |
| `str.ToDate()` / `str.ToDate(default)` | 安全转换为 `DateTime` |
| `dt.ToChWeekName()` | 日期转中文星期 |
| `dt.ToTimeHHmmssString()` | 输出 `HH:mm:ss` |
| `dt.ToDataTimeString(...)` | 自定义分隔符的日期时间字符串 |
| `ConvHelper.GetTimeStamp()` | 13 位毫秒时间戳 |
| `path.ToBase64String()` | 文件转 Base64 |
| `path.ToUrlPath()` / `path.ToPath()` | 绝对路径与 Web 路径互转 |
| `str.MidStrEx(start, end)` | 截取两字符串中间内容 |
| `str.ToUtf8()` | 编码转 UTF-8 |
| `csv.ToRandomCode(len)` | 从字符集生成随机码 |
| `dt.LastMonth(endDate)` | 计算年月天间隔 |

```csharp
using Acme.Tools;

int n = "123".ToInt();
DateTime d = "2024-01-15".ToDate();
string ts = $"时间戳：{ConvHelper.GetTimeStamp()}";
string interval = new DateTime(2020, 1, 1).LastMonth(new DateTime(2024, 5, 15)); // 4年4月14天
```

### HTTP 请求

`HttpHelper` 复用单一 `HttpClient`，内置 30 秒超时、连接池 DNS 刷新、状态码校验，统一使用 Newtonsoft.Json 序列化请求体。

| 方法 | 说明 |
| :--- | :--- |
| `HttpClientPostAsync(url, data, ct)` | 异步 POST，请求体自动 JSON 序列化 |
| `HttpClientGetAsync(url, ct)` | 异步 GET |
| `HttpClientPost(url, data)` | 同步 POST（兼容旧版） |
| `HttpClientGet(url)` | 同步 GET（兼容旧版） |

> 非 2xx 状态码或网络异常将抛出 `HttpRequestException`。

```csharp
using Acme.Tools;

// 推荐异步 + 可取消
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
string text = await HttpHelper.HttpClientGetAsync("https://api.example.com/users?id=1", cts.Token);

string resp = await HttpHelper.HttpClientPostAsync(
    "https://api.example.com/update",
    new { UserId = 1, Action = "update" });
```

### 加密工具

`MD5Encryption` 提供 MD5 加密与校验。

```csharp
using Acme.Tools;

string lower = MD5Encryption.Encrypt("password123");        // 小写
string upper = MD5Encryption.Encrypt("password123", true);  // 大写
bool match = MD5Encryption.Compare("password123", lower);
```

> MD5 不适用于高敏感数据，密码场景建议使用 BCrypt / Argon2 等。

### Session 管理

通过 `IHttpSessionService` 读写 Session，支持字符串与对象（JSON）。

```csharp
using Acme.Sessions;

public class AccountController(IHttpSessionService session)
{
    public void Login(string userName)
    {
        session.SetSession("CurrentUser", userName);
        session.SetObjectAsJson("UserInfo", new { UserName = userName, Role = "Admin" });
    }

    public object GetInfo()
    {
        string user = session.GetSession("CurrentUser");
        var info = session.GetObjectFromJson<dynamic>("UserInfo");
        return new { user, info };
    }

    public void Logout() => session.RemoveSession("CurrentUser");
}
```

### Cookie 管理

通过 `IHttpCookieService` 读写 Cookie。`SetCookies` 默认开启 `HttpOnly` 与 `SameSite=Lax`。

| 方法 | 说明 |
| :--- | :--- |
| `SetCookies(key, value, minutes = 300, httpOnly = true)` | 写入 Cookie |
| `GetCookies(key)` | 读取 Cookie（不存在返回空串） |
| `DeleteCookies(key)` | 删除 Cookie |

```csharp
using Acme.Cookie;

public class HomeController(IHttpCookieService cookie)
{
    public void Set()
    {
        // 安全 Cookie（HttpOnly，前端脚本不可读）
        cookie.SetCookies("AuthToken", "abc123", minutes: 60);

        // 需要前端 JS 读取时（如主题、语言偏好）关闭 HttpOnly
        cookie.SetCookies("UserPreferences", "theme=dark", httpOnly: false);
    }

    public string Read() => cookie.GetCookies("AuthToken");
}
```

### 拼音处理

将中文转换为完整拼音或拼音首字母。

| 方法 | 说明 |
| :--- | :--- |
| `GetInitials(text)` | 拼音首字母（大写） |
| `GetPinyin(text)` | 完整拼音（空格分隔） |
| `GetChineseText(pinyin)` | 拼音转中文 |
| `ConvertEncoding(text, src, dst)` | 编码转换 |

```csharp
using Acme.Pinyins;

string initials = Pinyin.GetInitials("中文拼音处理"); // ZWPYCL
string full = Pinyin.GetPinyin("中文");              // zhong wen
string chinese = Pinyin.GetChineseText("zhong");
```

### Excel 处理

基于 NPOI，提供泛型导入导出。`NpolExcelService<TEntity>` 为 DI 友好的实例类，`NpolExcelHelper<T>` 为静态方法版，二者用法一致。

| 方法 | 说明 |
| :--- | :--- |
| `InputExcel(IFormFile file, string sheetName = null)` | 从上传文件导入 |
| `InputExcel(string fileName, string sheetName = null)` | 从文件路径导入 |
| `ExportExcel(list, templatePath, startRow, sheetName, depositPath, fileName)` | 按模板导出 |

```csharp
using Acme.Excel;

public class Product
{
    public string Name { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedDate { get; set; }
}

// 导入
var service = new NpolExcelService<Product>();
List<Product> list = service.InputExcel("products.xlsx");
List<Product> fromForm = service.InputExcel(formFile, "Sheet1");

// 导出（需提供模板文件）
string path = service.ExportExcel(
    list,
    templatePath: "template.xlsx",
    startRow: 2,
    sheetName: "产品列表",
    depositPath: "exports",
    fileName: "产品数据");
```

> 导入会跳过空行，单元格类型转换失败时保留默认值，不中断整表导入；支持 `.xls` / `.xlsx` / `.xlsm`（后缀大小写不敏感）。

### 验证码

生成数字、算式验证码及验证码图片。

| 方法 | 说明 |
| :--- | :--- |
| `GetCode(len)` | 指定长度数字验证码（0-9） |
| `GetCodeBy100(len)` | 指定长度数字验证码（1-99） |
| `GetOperator()` | 随机运算符 `+ - × ÷` |
| `GetEquationCode(out string equation)` | 生成算式验证码并返回结果 |
| `CreateValidateGraphic1(code)` / `CreateValidateGraphic2(code)` | 生成验证码图片（两种样式） |
| `CalcByDataTable(expression)` | 计算简单四则表达式 |

```csharp
using Acme.Tools;

string code = VerCode.GetCode(4);
int result = VerCode.GetEquationCode(out string equation); // equation 如 "23+45=?"
byte[] image = VerCode.CreateValidateGraphic1(code);
```

### 验证工具

`VerificationHelper` 提供常用校验扩展方法。

| 方法 | 说明 |
| :--- | :--- |
| `str.IsNotNull()` | 是否非空（含空白字符） |
| `guid.IsNotDefault()` / `int.IsNotDefault()` | 是否非默认值 |
| `guid?.IsDefault()` | 可空 Guid 是否为空 |
| `str.RegexVerification(pattern)` | 正则匹配 |

```csharp
using Acme.Tools;

bool notEmpty = "hello".IsNotNull();
bool idOk = userId.IsNotDefault();
bool emailOk = "user@example.com".RegexVerification(@"^[\w.+-]+@[\w.-]+\.\w+$");
```

### 通用返回对象

`Oh` 提供统一 API 响应体（`Code` / `Msg` / `Data`），配合扩展方法快速构造成功或失败响应。

```csharp
using Acme.Ohs;
using Acme.Constant;

// 成功 / 失败
return Ok(user.Success());                 // { Code: 200, Msg: "成功！", Data: {...} }
return Ok(user.Success("创建成功"));
return BadRequest(Oh.Fail("用户不存在"));
return Ok(hasPermission.IsSuccess("有权限", "无权限"));

// 自定义状态码
return StatusCode(400, new Oh(400, "更新失败", null));
```

常用常量见 `CV`：`YesCode`(200)、`NoCode`(500)、`Yes`、`ErrorServer` 等。

### 全局过滤器与 Swagger

`GlobalFilterCollectionExtension` 提供异常过滤器注册、Swagger（单/多版本）、JWT 认证与模型验证响应格式化。

```csharp
// 注册服务
builder.Services.AddExceptionFilterService();       // 全局异常过滤器
builder.Services.ConfigureApiBehaviorOptions();      // 模型验证失败统一返回 Oh
builder.Services.ConfigureSwaggerOptions("我的 API"); // 单版本
builder.Services.AddOhAuthentication();              // JWT 认证

// 多版本 Swagger
// builder.Services.ConfigureSwaggerOptions(new List<SwaggerDocDto>
// {
//     new("API v1", "v1"),
//     new("API v2", "v2")
// });

// 启用中间件
app.UseSingSwagger("我的 API", "v1", "swagger");
// app.UseBathSwagger(swaggerDocs, "swagger");
app.UseAuthentication();
app.UseAuthorization();
```

> `ExceptionFilter` 对 `ArgumentException` 返回 400 并回显提示，其余异常统一返回 500 通用提示，避免泄露堆栈等敏感信息。

### 配置管理

`App` 读取根目录 `appsettings.json`，`JsonFile` 读取任意 JSON 配置文件。

| 成员 | 说明 |
| :--- | :--- |
| `App.GetValue(key)` / `App.GetValue(key, name)` | 读取配置值 |
| `App.Path` | 应用程序域基础目录 |
| `App.SystemPath` | 当前工作目录 |
| `new JsonFile(file).GetValue(key)` | 读取指定 JSON 配置 |

```csharp
using Acme.Apps;
using Acme.JsonFiles;

string db = App.GetValue("ConnectionStrings:DefaultConnection");
string basePath = App.Path;

var custom = new JsonFile("customSettings.json");
string apiKey = custom.GetValue("ThirdParty:ApiKey");
```

---

## 依赖

| 依赖包 | 用途 | .NET 8 | .NET 9 | .NET 10 |
| :----- | :--- | :----- | :----- | :------ |
| Newtonsoft.Json | JSON 序列化 | 13.0.4 | 13.0.4 | 13.0.4 |
| NPOI | Excel 操作 | 2.8.1 | 2.8.1 | 2.8.1 |
| ZKWeb.System.Drawing | 验证码绘图 | 4.0.1 | 4.0.1 | 4.0.1 |
| Microsoft.Extensions.Caching.Abstractions | 缓存抽象 | 10.0.14 | 10.0.14 | 10.0.14 |
| Microsoft.Extensions.DependencyInjection | 依赖注入 | 10.0.14 | 10.0.14 | 10.0.14 |
| Microsoft.Extensions.Features | 特性抽象 | 10.0.14 | 10.0.14 | 10.0.14 |
| Microsoft.AspNetCore.Authentication.JwtBearer | JWT 认证 | 8.0.33 | 9.0.22 | 10.0.14 |
| Swashbuckle.AspNetCore.* | Swagger 文档 | 10.3.1 | 10.3.1 | 10.3.1 |

## 版本兼容性

- .NET 8.0：支持
- .NET 9.0：支持
- .NET 10.0：支持

## 贡献

欢迎通过 [Gitee Issues](https://gitee.com/yzxs949/acme/issues) 反馈问题或提出建议。

1. Fork 本仓库
2. 新建分支：`git checkout -b feature/your-feature`
3. 提交改动：`git commit -m "feat: your feature"`
4. 推送分支：`git push origin feature/your-feature`
5. 提交 Pull Request

提交 Pull Request 前，请确保解决方案能够成功编译（`dotnet build`）。

## 许可证

本项目基于 [MIT](LICENSE) 许可证开源。

## 联系

- 邮箱：<yzxs949@163.com>
- 微信公众号：.NET修仙日记
- 仓库地址：<https://gitee.com/yzxs949/acme>

![.NET修仙日记](https://raw.githubusercontent.com/yzxs949/FilePath/refs/heads/main/Image/%E4%BA%8C%E7%BB%B4%E7%A0%81.jpg)

## 赞赏支持

如果这个项目对您有帮助，欢迎请作者喝杯咖啡，您的支持是持续维护的动力。

| 支付宝 | 微信 |
| :---: | :---: |
| ![支付宝](https://raw.githubusercontent.com/yzxs949/FilePath/refs/heads/main/Image/zfb.jpg) | ![微信](https://raw.githubusercontent.com/yzxs949/FilePath/refs/heads/main/Image/wx.jpg) |

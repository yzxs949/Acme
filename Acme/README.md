# Acme .NET 工具类库

**版本：5.6.5** | **支持框架：.NET 8.0 / 9.0 / 10.0**

Acme 是一个 .NET 工具类库，提供 JSON 处理、类型转换、HTTP 请求、加密、Session/Cookie、拼音、Excel、验证码、通用返回格式等功能模块。

---

## 快速开始

### 安装

```bash
dotnet add package Acme
```

### 服务注册

```csharp
using Acme;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAcmeBuilderServer(); // 注册 HttpContext、Session、Cookie 服务
var app = builder.Build();
```

### 基本使用

```csharp
using Acme.Tools;

// JSON 序列化
string json = new { Name = "张三", Age = 25 }.ToJson();

// 类型安全转换
int number = "123".ToInt();

// MD5 加密
string hash = MD5Encryption.Encrypt("password123");
```

---

## 核心功能

### 1. JSON 处理

> 基于 `Newtonsoft.Json`

```csharp
using Acme.Tools;

// 对象 → JSON
string json = obj.ToJson();

// JSON → 实体
var user = jsonString.ToEntity<User>();
var dyn = jsonString.ToEntity<dynamic>();

// JSON → JObject
JObject jo = Json.ToJObject(str);
```

### 2. 数据类型转换

> 安全转换，失败返回默认值，不抛异常

```csharp
using Acme.Tools;

// 安全类型转换
int  i   = "123".ToInt();          // 123
int  i2  = "abc".ToInt(0);         // 0（默认值）
short s  = "45".ToShort();         // 45
DateTime d = "2025-01-15".ToDate(); // 失败返回 1970-1-1

// 路径处理
string webPath   = @"C:\app\file.jpg".ToUrlPath();  // 绝对→相对
string filePath  = "/files/img.jpg".ToPath();        // 相对→绝对
string base64    = @"C:\img\photo.jpg".ToBase64String();

// 随机码（密码学安全）
string code = "0,1,2,3,4,5,6,7,8,9".ToRandomCode(6);

// 时间处理
long stamp = ConvHelper.GetTimeStamp();               // 13位毫秒
string wd  = DateTime.Now.ToChWeekName();              // "一"~"日"
string tm  = DateTime.Now.ToTimeHHmmssString();        // "14:30:00"
string dt  = DateTime.Now.ToDataTimeString("/", ":");  // "2025/01/15 14:30:00"
string gap = DateTime.Now.LastMonth(DateTime.Now.AddMonths(-3)); // "3月"

// 字符串截取
string mid = "Hello【world】End".MidStrEx("【", "】"); // "world"

// 编码修复
string utf8 = garbledText.ToUtf8();
```

### 3. HTTP 请求

> 使用全局复用 `HttpClient` 单例，避免端口耗尽。推荐使用异步方法。

```csharp
using Acme.Tools;

// GET（同步）
string res = HttpHelper.HttpClientGet("https://api.example.com/users?id=1");

// POST（同步）
string res2 = HttpHelper.HttpClientPost("https://api.example.com/update",
    new { UserId = 1, Action = "update" });

// 异步方法（推荐）
string res3 = await HttpHelper.HttpClientGetAsync(url);
string res4 = await HttpHelper.HttpClientPostAsync(url, data);

// 错误处理
try
{
    string result = await HttpHelper.HttpClientGetAsync(url);
    var data = result.ToEntity<dynamic>();
}
catch (Exception ex)
{
    // 失败时返回 "请求失败：{错误信息}"
}
```

### 4. MD5 加密

```csharp
using Acme.Tools;

string hash      = MD5Encryption.Encrypt("text");          // 小写
string hashUpper = MD5Encryption.Encrypt("text", true);   // 大写
bool match       = MD5Encryption.Compare("text", hash);   // true
```

> **注意**：MD5 适用于非敏感场景。敏感数据建议使用 BCrypt、Argon2 等。

### 5. Session 管理

**接口：** `IHttpSessionService` → **实现：** `HttpSessionService`

```csharp
using Acme.Sessions;

// 字符串存储
_sessionService.SetSession("UserId", "123");
string val = _sessionService.GetSession("UserId");

// 对象存储（JSON 序列化）
_sessionService.SetObjectAsJson("Cart", cartData);
var cart = _sessionService.GetObjectFromJson<Cart>("Cart");

// 移除
_sessionService.RemoveSession("UserId");
```

### 6. Cookie 管理

**接口：** `IHttpCookieService` → **实现：** `HttpCookieService`

> Cookie 默认启用 `HttpOnly` 和 `SameSite=Lax`。

```csharp
using Acme.Cookie;

// 设置（默认300分钟）
_cookieService.SetCookies("Token", "abc123", 60);

// 读取
string token = _cookieService.GetCookies("Token");

// 删除
_cookieService.DeleteCookies("Token");
```

### 7. 通用返回格式（Oh）

**所在文件：** `Ohs/Oh.cs`、`Ohs/OhExpand.cs`

```csharp
using Acme.Ohs;

// 成功
return data.Success();              // { Code: 200, Msg: "成功！", Data: data }
return data.Success("操作成功");

// 失败
return data.Fail();                 // { Code: 500, Msg: "服务器错误！", Data: data }

// 自定义状态码
return data.Custom(403, "权限不足");

// 泛型版本
return data.Success<UserDto>("获取成功");

// 静态创建
var result = Oh<MyData>.NewOh(200, "成功", myData);
```

### 8. 拼音处理

```csharp
using Acme.Pinyins;

// 获取拼音首字母
string initials = Pinyin.GetInitials("中文拼音处理"); // "ZWPYCL"

// 获取完整拼音
string pinyin = Pinyin.GetPinyin("中文"); // "zhong wen"

// 编码转换
string encoded = Pinyin.ConvertEncoding("文本", Encoding.UTF8, Encoding.ASCII);
```

### 9. Excel 处理

> 基于 NPOI，支持导入/导出。导出路径已内置路径遍历防护。

```csharp
using Acme.Excel;
using Acme.Tools;

// 定义模型
public class Product { public string Name { get; set; } public int Stock { get; set; } }

// 方式一：静态工具类
List<Product> list = NpolExcelHelper<Product>.InputExcel(@"C:\data\products.xlsx");
string path = NpolExcelHelper<Product>.ExportExcel(list, @"template.xlsx");

// 方式二：依赖注入服务
public class ProductController : Controller
{
    private readonly NpolExcelService<Product> _excel;

    public ProductController(NpolExcelService<Product> excel) => _excel = excel;

    [HttpPost("import")]
    public IActionResult Import(IFormFile file) => Ok(_excel.InputExcel(file));

    [HttpGet("export")]
    public IActionResult Export()
    {
        string filePath = _excel.ExportExcel(
            new List<Product> { new Product { Name = "笔记本", Stock = 50 } },
            templatePath: @"templates\report.xlsx",
            depositPath: "exports",
            fileName: "report"
        );
        return PhysicalFile(filePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            Path.GetFileName(filePath));
    }
}
```

### 10. 验证码生成

> 使用密码学安全随机数 `RandomNumberGenerator`，防止验证码被预测。

```csharp
using Acme.Tools;

// 数字验证码
string code = VerCode.GetCode(6);  // 6位数字

// 算式验证码
string equation;
int result = VerCode.GetEquationCode(out equation);
// equation = "12+34=?", result = 46

// 验证码图片（byte[] 直接返回给前端）
byte[] img = VerCode.CreateValidateGraphic2(VerCode.GetCode(4));
```

### 11. 验证工具

```csharp
using Acme.Tools;

// 字符串判空
bool has = "张三".HasValue();            // true（推荐）
bool isNull = "".IsNotNull();            // true（原有方法）

// Guid 默认值判断
Guid.Empty.IsDefault();                  // true

// int 非零判断
0.IsNotZero();                           // false

// 正则验证
"user@ex.com".RegexVerification(@"^[\w-]+@[\w-]+(\.[\w-]+)+$"); // true
```

### 12. 配置管理

**App 类：** 读取 `appsettings.json` | **JsonFile 类：** 读取自定义 JSON 文件

```csharp
using Acme.Apps;
using Acme.JsonFiles;

// 读取 appsettings.json
string conn = App.GetValue("ConnectionStrings:DefaultConnection");
string api  = App.GetValue("AppSettings:ApiBaseUrl");

// 读取自定义配置文件
var config = new JsonFile("customSettings.json");
string key = config.GetValue("ThirdParty:ApiKey");

// 路径信息
string dir = App.Path;        // 应用程序基础目录
string cwd = App.SystemPath;  // 当前工作目录
```

---

## 依赖项

| 依赖包 | 用途 | 版本 |
|--------|------|------|
| Newtonsoft.Json | JSON 序列化/反序列化 | 13.0.4 |
| NPOI | Excel 文件操作 | 2.7.5 / 2.7.6 (net10.0) |
| Microsoft.Extensions.DependencyInjection | 依赖注入 | 9.0.11 / 10.0.5 (net10.0) |
| Microsoft.Extensions.Caching.Abstractions | 缓存抽象 | 9.0.11 / 10.0.5 (net10.0) |
| Swashbuckle.AspNetCore | Swagger 文档 | 9.0.6 / 10.1.5 (net10.0) |
| ZKWeb.System.Drawing | 绘图验证码 | 4.0.1 |

---

## 注意事项

- HTTP 请求失败返回 `"请求失败：{错误信息}"`，建议封装 `try-catch`
- `ConvHelper` 的 `ToInt`/`ToShort`/`ToDate` 等自带安全转换，失败返回默认值
- Cookie 默认启用 `HttpOnly` 和 `SameSite=Lax`
- Excel 的 `ExportExcel` 已内置路径遍历防护
- 大型 Excel 文件导入导出可能消耗较多内存
- `VerCode` 使用 `RandomNumberGenerator`（密码学安全），请勿回退为 `System.Random`

---

*本文档基于 Acme 版本 5.6.5。*

# Acme .NET 工具类库文档

## 概述

Acme 是一个功能丰富的 .NET 工具类库，提供了一系列常用的功能扩展和工具类，旨在简化日常开发工作。

**版本信息**
- 当前版本：5.6
- 支持框架：.NET 6.0、7.0、8.0、9.0、10.0
- 许可证：Apache License 2.0

## 快速开始

### 安装与配置

```csharp
// Program.cs 或 Startup.cs
using Acme;

var builder = WebApplication.CreateBuilder(args);

// 注册 Acme 服务
builder.Services.AddAcmeBuilderServer();

var app = builder.Build();
```

## 核心功能模块

### 1. JSON 处理

**所在文件：** `Json.cs`

```csharp
using Acme.Tools;

// 对象序列化
var user = new { Name = "张三", Age = 25, Email = "zhangsan@example.com" };
string json = user.ToJson();
// 输出: {"Name":"张三","Age":25,"Email":"zhangsan@example.com"}

// JSON 反序列化
string jsonString = "{\"Name\":\"李四\",\"Age\":30}";
var person = jsonString.ToEntity<dynamic>();
Console.WriteLine(person.Name); // 输出: 李四

// 泛型反序列化
var specificPerson = jsonString.ToEntity<User>();
```

### 2. 数据类型转换

**所在文件：** `ConvHelper.cs`

```csharp
using Acme.Tools;

// 安全类型转换
string numStr = "123";
int number = numStr.ToInt();           // 返回 123
int safeNumber = "abc".ToInt(0);       // 返回默认值 0
double doubleNum = "45.67".ToDouble();

// 路径处理
string absolutePath = @"C:\project\files\image.jpg";
string webPath = absolutePath.ToUrlPath(); // 转换为 Web 可用路径

// 随机数据生成
string randomCode = "0,1,2,3,4,5,6,7,8,9".ToRandomCode(6); // 6位数字验证码
string customRandom = "A,B,C,D,E".ToRandomCode(8); // 8位字母码

// 时间处理
long timestamp = ConvHelper.GetTimeStamp(); // 13位时间戳
```

### 3. HTTP 请求封装

**所在文件：** `HttpHelper.cs`

```csharp
using Acme.Tools;

// GET 请求示例
string apiUrl = "https://api.example.com/users?id=1";
string response = HttpHelper.HttpClientGet(apiUrl);
Console.WriteLine(response);

// POST 请求示例
var postData = new { 
    UserId = 1, 
    Action = "update", 
    Data = new { Name = "新名称" }
};
string postResponse = HttpHelper.HttpClientPost(
    "https://api.example.com/update", 
    postData
);

// 建议的错误处理方式
try
{
    string result = HttpHelper.HttpClientGet(apiUrl);
    // 处理结果
}
catch (Exception ex)
{
    Console.WriteLine($"请求失败: {ex.Message}");
}
```

### 4. 加密工具

**所在文件：** `MD5Encryption.cs`

```csharp
using Acme.Tools;

// 基本加密
string password = "mypassword123";
string encrypted = MD5Encryption.Encrypt(password);        // 小写MD5
string encryptedUpper = MD5Encryption.Encrypt(password, true); // 大写MD5

// 密码验证
bool isValid = MD5Encryption.Compare(inputPassword, storedHash);

// 使用示例 - 用户密码处理
public class UserService
{
    public bool ValidateUser(string inputPassword, string storedHash)
    {
        return MD5Encryption.Compare(inputPassword, storedHash);
    }
    
    public string HashPassword(string password)
    {
        return MD5Encryption.Encrypt(password);
    }
}
```

### 5. Session 管理

**接口：** `IHttpSessionService`  
**实现：** `HttpSessionService`

```csharp
using Acme.Sessions;

// 在控制器中使用
public class AccountController : Controller
{
    private readonly IHttpSessionService _sessionService;
    
    public AccountController(IHttpSessionService sessionService)
    {
        _sessionService = sessionService;
    }
    
    public IActionResult Login(string username)
    {
        // 存储字符串
        _sessionService.SetSession("CurrentUser", username);
        
        // 存储对象
        var userInfo = new { 
            UserName = username, 
            LoginTime = DateTime.Now,
            Role = "Admin"
        };
        _sessionService.SetObjectAsJson("UserInfo", userInfo);
        
        return RedirectToAction("Index");
    }
    
    public IActionResult GetUserInfo()
    {
        // 读取数据
        string username = _sessionService.GetSession("CurrentUser");
        var userInfo = _sessionService.GetObjectFromJson<dynamic>("UserInfo");
        
        // 清理 Session
        _sessionService.RemoveSession("CurrentUser");
        
        return View(userInfo);
    }
}
```

### 6. Cookie 管理

**接口：** `IHttpCookieService`  
**实现：** `HttpCookieService`

```csharp
using Acme.Cookie;

public class HomeController : Controller
{
    private readonly IHttpCookieService _cookieService;
    
    public HomeController(IHttpCookieService cookieService)
    {
        _cookieService = cookieService;
    }
    
    public IActionResult SetCookie()
    {
        // 设置 Cookie（300分钟过期）
        _cookieService.SetCookies("UserPreferences", "theme=dark", 300);
        
        // 自定义过期时间（60分钟）
        _cookieService.SetCookies("AuthToken", "abc123xyz", 60);
        
        return View();
    }
    
    public IActionResult ReadCookie()
    {
        string token = _cookieService.GetCookies("AuthToken");
        string preferences = _cookieService.GetCookies("UserPreferences");
        
        ViewData["Token"] = token;
        return View();
    }
    
    public IActionResult DeleteCookie()
    {
        _cookieService.DeleteCookies("AuthToken");
        return RedirectToAction("Index");
    }
}
```

### 7. 拼音处理

**所在文件：** `Pinyin.cs`

```csharp
using Acme.Pinyins;

// 中文转拼音首字母
string chinese = "中文拼音处理";
string initials = Pinyin.GetInitials(chinese); // 输出: "ZWPYCL"

// 编码转换（如果可用）
string encodedText = Pinyin.ConvertEncoding("文本内容", "UTF-8");

// 使用场景示例
public class SearchService
{
    public string GenerateSearchKey(string chineseName)
    {
        return Pinyin.GetInitials(chineseName).ToLower();
    }
}
```

### 8. Excel 处理

**基于 NPOI 的 Excel 操作**

```csharp
using Acme.Excel;

// 定义数据模型
public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
}

public class ExcelService
{
    // 导入 Excel 数据
    public List<Product> ImportProducts(string filePath)
    {
        var excelService = new NpolExcelService<Product>();
        return excelService.ImportFromExcel(filePath);
    }
    
    // 导出数据到 Excel
    public void ExportProducts(List<Product> products, string outputPath)
    {
        var excelService = new NpolExcelService<Product>();
        excelService.ExportToExcel(products, outputPath);
    }
}

// 使用示例
var products = new List<Product>
{
    new Product { Name = "笔记本电脑", Price = 5999.99m, Stock = 50 },
    new Product { Name = "无线鼠标", Price = 89.90m, Stock = 100 }
};

excelService.ExportProducts(products, "products.xlsx");
```

### 9. 验证码生成

**所在文件：** `VerCode.cs`

```csharp
using Acme.Tools;

// 数字验证码
string numericCode = VerCode.GetCode(6); // 6位数字验证码
string fourDigitCode = VerCode.GetCode(4); // 4位数字验证码

// 算式验证码（如果可用）
// var equation = VerCode.GetEquationCode(); // 返回算式和答案

// 在注册流程中使用
public class VerificationService
{
    public string GeneratePhoneVerificationCode()
    {
        return VerCode.GetCode(6); // 生成6位手机验证码
    }
    
    public string GenerateEmailVerificationCode()
    {
        return VerCode.GetCode(8); // 生成8位邮箱验证码
    }
}
```

### 10. 验证工具

**所在文件：** `VerificationHelper.cs`

```csharp
using Acme.Tools;

// Guid 验证
Guid testGuid = Guid.NewGuid();
bool isDefaultGuid = testGuid.IsDefault(); // 检查是否为默认值

// 正则表达式验证
string email = "user@example.com";
bool isValidEmail = email.RegexVerification(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$");

string phone = "13800138000";
bool isValidPhone = phone.RegexVerification(@"^1[3-9]\d{9}$");

// 扩展方法使用
public class UserValidator
{
    public bool ValidateUserInput(User user)
    {
        if (user.Id.IsDefault())
            return false;
            
        if (!user.Email.RegexVerification(emailPattern))
            return false;
            
        return true;
    }
}
```

### 11. 配置管理

**配置文件读取工具**

```csharp
using Acme.Apps;
using Acme.JsonFiles;

// 使用 App 类读取配置
string dbConnection = App.GetValue("ConnectionStrings:DefaultConnection");
string appName = App.GetValue("AppSettings:ApplicationName");
string logLevel = App.GetValue("Logging:LogLevel:Default");

// 自定义配置文件
var customConfig = new JsonFile("customSettings.json");
string apiKey = customConfig.GetValue("ThirdParty:ApiKey");
string endpoint = customConfig.GetValue("ThirdParty:Endpoint");

// 路径信息
string basePath = App.Path;           // 应用程序域基础目录
string workingPath = App.SystemPath;  // 当前工作目录

// 配置类封装示例
public class AppConfig
{
    public static string DatabaseConnection => 
        App.GetValue("ConnectionStrings:DefaultConnection");
        
    public static string ApiBaseUrl => 
        App.GetValue("AppSettings:ApiBaseUrl");
        
    public static bool EnableCache => 
        bool.Parse(App.GetValue("AppSettings:EnableCache") ?? "false");
}
```

## 最佳实践

### 1. 依赖注入配置

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// 注册 Acme 服务
builder.Services.AddAcmeBuilderServer();

// 注册自定义服务
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<IExcelService, ExcelService>();

var app = builder.Build();
```

### 2. 错误处理策略

```csharp
public class SafeDataProcessor
{
    public int? SafeConvertToInt(string input)
    {
        try
        {
            return input.ToInt();
        }
        catch
        {
            return null;
        }
    }
    
    public T SafeJsonDeserialize<T>(string json) where T : new()
    {
        try
        {
            return json.ToEntity<T>();
        }
        catch
        {
            return new T();
        }
    }
}
```

### 3. 配置管理示例

```csharp
public static class GlobalSettings
{
    public static class Paths
    {
        public static string Temp => Path.Combine(App.Path, "temp");
        public static string Logs => Path.Combine(App.Path, "logs");
        public static string Uploads => Path.Combine(App.Path, "uploads");
    }
    
    public static class Api
    {
        public static string BaseUrl => App.GetValue("Api:BaseUrl");
        public static int Timeout => int.Parse(App.GetValue("Api:Timeout") ?? "30");
    }
}
```

## 依赖项说明

| 依赖包 | 用途 |
|--------|------|
| Newtonsoft.Json | JSON 序列化/反序列化 |
| NPOI | Excel 文件操作 |
| Microsoft.Extensions.DependencyInjection | 依赖注入容器 |
| Microsoft.AspNetCore.Http | HTTP 上下文访问 |

## 注意事项

1. **版本兼容性**：确保使用的 .NET 版本在 6.0 及以上
2. **错误处理**：HTTP 请求等方法返回字符串结果，建议封装异常处理
3. **性能考虑**：频繁的 Session/Cookie 操作可能影响性能，请合理使用
4. **安全建议**：敏感数据建议使用更安全的加密方式，MD5 适用于非敏感场景

## 技术支持

如在使用过程中遇到问题，请检查：
- 是否正确注册了依赖注入服务
- 配置文件中相关配置项是否存在
- 使用的 .NET 版本是否受支持
- 相关依赖包版本是否兼容

---

*本文档基于 Acme 类库版本 5.6 AI编写，具体实现可能因版本更新而有所变化。*
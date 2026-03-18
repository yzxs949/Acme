# Acme .NET 工具类库文档

## 目录

- [Acme .NET 工具类库文档](#acme-net-工具类库文档)
  - [目录](#目录)
  - [概述](#概述)
  - [快速开始](#快速开始)
    - [安装与配置](#安装与配置)
      - [1. 通过 NuGet 安装（推荐）](#1-通过-nuget-安装推荐)
      - [2. 手动引用](#2-手动引用)
      - [3. 服务注册](#3-服务注册)
      - [4. 基本使用示例](#4-基本使用示例)
  - [核心功能模块](#核心功能模块)
    - [1. JSON 处理](#1-json-处理)
      - [主要功能](#主要功能)
      - [使用示例](#使用示例)
    - [2. 数据类型转换](#2-数据类型转换)
      - [主要功能](#主要功能-1)
      - [使用示例](#使用示例-1)
    - [3. HTTP 请求封装](#3-http-请求封装)
      - [主要功能](#主要功能-2)
      - [使用示例](#使用示例-2)
    - [4. 加密工具](#4-加密工具)
      - [主要功能](#主要功能-3)
      - [使用示例](#使用示例-3)
    - [5. Session 管理](#5-session-管理)
      - [主要功能](#主要功能-4)
      - [使用示例](#使用示例-4)
    - [6. Cookie 管理](#6-cookie-管理)
      - [主要功能](#主要功能-5)
      - [使用示例](#使用示例-5)
    - [7. 拼音处理](#7-拼音处理)
      - [主要功能](#主要功能-6)
      - [使用示例](#使用示例-6)
    - [8. Excel 处理](#8-excel-处理)
      - [主要功能](#主要功能-7)
      - [使用示例](#使用示例-7)
    - [9. 验证码生成](#9-验证码生成)
      - [主要功能](#主要功能-8)
      - [使用示例](#使用示例-8)
    - [10. 验证工具](#10-验证工具)
      - [主要功能](#主要功能-9)
      - [使用示例](#使用示例-9)
    - [11. 配置管理](#11-配置管理)
      - [主要功能](#主要功能-10)
      - [使用示例](#使用示例-10)
  - [最佳实践](#最佳实践)
    - [1. 依赖注入配置](#1-依赖注入配置)
      - [基本配置](#基本配置)
      - [服务生命周期管理](#服务生命周期管理)
    - [2. 错误处理策略](#2-错误处理策略)
      - [安全数据处理](#安全数据处理)
      - [全局异常处理](#全局异常处理)
    - [3. 配置管理示例](#3-配置管理示例)
      - [分层配置结构](#分层配置结构)
      - [配置验证](#配置验证)
    - [4. 性能优化建议](#4-性能优化建议)
      - [缓存策略](#缓存策略)
      - [异步操作](#异步操作)
    - [5. 安全最佳实践](#5-安全最佳实践)
      - [密码处理](#密码处理)
      - [输入验证](#输入验证)
  - [依赖项说明](#依赖项说明)
    - [安装依赖项](#安装依赖项)
    - [版本兼容性](#版本兼容性)
  - [注意事项](#注意事项)
    - [1. 版本兼容性](#1-版本兼容性)
    - [2. 错误处理](#2-错误处理)
    - [3. 性能考虑](#3-性能考虑)
    - [4. 安全建议](#4-安全建议)
    - [5. 配置管理](#5-配置管理)
    - [6. 路径处理](#6-路径处理)
    - [7. 依赖注入](#7-依赖注入)
  - [技术支持](#技术支持)
    - [常见问题排查](#常见问题排查)
    - [联系支持](#联系支持)
    - [提交问题时请提供以下信息](#提交问题时请提供以下信息)
    - [故障排除示例](#故障排除示例)

## 概述

Acme 是一个功能丰富的 .NET 工具类库，提供了一系列常用的功能扩展和工具类，旨在简化日常开发工作。

**版本信息**
- 当前版本：5.6.4
- 支持框架：.NET 8.0、9.0、10.0

## 快速开始

### 安装与配置

#### 1. 通过 NuGet 安装（推荐）

在 Visual Studio 中打开 NuGet 包管理器，搜索并安装 `Acme` 包。

或者使用 .NET CLI 命令：

```bash
dotnet add package Acme
```

#### 2. 手动引用

将 Acme 项目添加到解决方案中，并在目标项目中添加对它的引用。

#### 3. 服务注册

在 `Program.cs` 或 `Startup.cs` 中注册 Acme 服务：

```csharp
// Program.cs
using Acme;

var builder = WebApplication.CreateBuilder(args);

// 注册 Acme 服务
builder.Services.AddAcmeBuilderServer();

var app = builder.Build();
```

#### 4. 基本使用示例

```csharp
// 在控制器或服务中使用
using Acme.Tools;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        // 使用 JSON 工具
        var user = new { Name = "张三", Age = 25 };
        string json = user.ToJson();
        
        // 使用类型转换工具
        int number = "123".ToInt();
        
        // 使用加密工具
        string hashedPassword = MD5Encryption.Encrypt("password123");
        
        return View();
    }
}
```

## 核心功能模块

### 1. JSON 处理

**所在文件：** `Json.cs`

JSON 处理模块提供了简洁的对象序列化和反序列化方法，基于 Newtonsoft.Json 库。

#### 主要功能
- 对象序列化：将 .NET 对象转换为 JSON 字符串
- JSON 反序列化：将 JSON 字符串转换为 .NET 对象
- 支持动态类型和泛型类型

#### 使用示例

```csharp
using Acme.Tools;

// 1. 对象序列化
var user = new { 
    Name = "张三", 
    Age = 25, 
    Email = "zhangsan@example.com",
    Address = new { 
        City = "北京", 
        District = "朝阳区"
    }
};
string json = user.ToJson();
// 输出: {"Name":"张三","Age":25,"Email":"zhangsan@example.com","Address":{"City":"北京","District":"朝阳区"}}

// 2. JSON 反序列化为动态类型
string jsonString = "{\"Name\":\"李四\",\"Age\":30}";
var person = jsonString.ToEntity<dynamic>();
Console.WriteLine(person.Name); // 输出: 李四

// 3. 反序列化为具体类型
public class User
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
}

var specificPerson = jsonString.ToEntity<User>();
Console.WriteLine(specificPerson.Age); // 输出: 30

// 4. 处理复杂对象
string complexJson = "{\"Users\":[{\"Name\":\"张三\",\"Age\":25},{\"Name\":\"李四\",\"Age\":30}]}";
var result = complexJson.ToEntity<dynamic>();
foreach (var userItem in result.Users)
{
    Console.WriteLine($"姓名: {userItem.Name}, 年龄: {userItem.Age}");
}
```

### 2. 数据类型转换

**所在文件：** `ConvHelper.cs`

数据类型转换模块提供了安全、便捷的类型转换方法和一些实用的工具函数。

#### 主要功能
- 安全类型转换：支持字符串到各种基本类型的安全转换
- 路径处理：文件路径和 Web 路径的相互转换
- 随机数据生成：基于指定字符集生成随机码
- 时间处理：时间戳生成和转换

#### 使用示例

```csharp
using Acme.Tools;

// 1. 安全类型转换
string numStr = "123";
int number = numStr.ToInt();           // 返回 123
int safeNumber = "abc".ToInt(0);       // 返回默认值 0
double doubleNum = "45.67".ToDouble(); // 返回 45.67
bool boolValue = "true".ToBool();      // 返回 true

// 2. 路径处理
string absolutePath = @"C:\project\files\image.jpg";
string webPath = absolutePath.ToUrlPath(); // 转换为 Web 可用路径 ("/project/files/image.jpg")

// 3. 随机数据生成
// 生成6位数字验证码
string randomCode = "0,1,2,3,4,5,6,7,8,9".ToRandomCode(6); 
// 生成8位字母码
string customRandom = "A,B,C,D,E".ToRandomCode(8); 
// 生成混合验证码
string mixedCode = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F".ToRandomCode(10);

// 4. 时间处理
long timestamp = ConvHelper.GetTimeStamp(); // 13位时间戳
DateTime date = ConvHelper.GetDateTimeFromTimestamp(timestamp); // 时间戳转日期

// 5. 其他实用方法
string trimmedStr = "  Hello World  ".TrimAll(); // 去除所有空白字符
string[] splitArr = "a,b,c".SplitToArray(); // 分割字符串为数组
```

### 3. HTTP 请求封装

**所在文件：** `HttpHelper.cs`

HTTP 请求封装模块提供了简洁的 HTTP 客户端操作方法，基于 .NET 的 HttpClient 类。

#### 主要功能
- GET 请求：发送 HTTP GET 请求并获取响应
- POST 请求：发送 HTTP POST 请求并获取响应
- 支持 JSON 数据格式
- 内置错误处理机制

#### 使用示例

```csharp
using Acme.Tools;

// 1. GET 请求示例
string apiUrl = "https://api.example.com/users?id=1";
string response = HttpHelper.HttpClientGet(apiUrl);
Console.WriteLine(response);

// 2. POST 请求示例
var postData = new { 
    UserId = 1, 
    Action = "update", 
    Data = new { Name = "新名称" }
};
string postResponse = HttpHelper.HttpClientPost(
    "https://api.example.com/update", 
    postData
);

// 3. 带自定义请求头的请求
var headers = new Dictionary<string, string>
{
    { "Authorization", "Bearer your-token-here" },
    { "Content-Type", "application/json" }
};

string authenticatedResponse = HttpHelper.HttpClientGet(
    "https://api.example.com/protected", 
    headers
);

// 4. 错误处理最佳实践
try
{
    string result = HttpHelper.HttpClientGet(apiUrl);
    // 处理结果
    var data = result.ToEntity<dynamic>();
    Console.WriteLine($"请求成功: {data.message}");
}
catch (Exception ex)
{
    Console.WriteLine($"请求失败: {ex.Message}");
    // 记录日志或执行其他错误处理
}

// 5. 异步请求（如果支持）
// 注意：实际实现可能因版本不同而有所差异
// var asyncResponse = await HttpHelper.HttpClientGetAsync(apiUrl);
```

### 4. 加密工具

**所在文件：** `MD5Encryption.cs`

加密工具模块提供了基于 MD5 算法的加密和验证功能，适用于非敏感数据的加密场景。

#### 主要功能
- MD5 加密：将字符串转换为 MD5 哈希值
- 密码验证：比较输入密码与存储的哈希值
- 支持大小写输出

#### 使用示例

```csharp
using Acme.Tools;

// 1. 基本加密
string password = "mypassword123";
string encrypted = MD5Encryption.Encrypt(password);        // 小写MD5
string encryptedUpper = MD5Encryption.Encrypt(password, true); // 大写MD5

Console.WriteLine($"小写MD5: {encrypted}");
Console.WriteLine($"大写MD5: {encryptedUpper}");

// 2. 密码验证
bool isValid = MD5Encryption.Compare(inputPassword, storedHash);

// 3. 使用示例 - 用户密码处理
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

// 4. 实际应用场景
public class LoginService
{
    private readonly IUserRepository _userRepository;
    
    public LoginService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<bool> LoginAsync(string username, string password)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);
        if (user == null)
            return false;
        
        return MD5Encryption.Compare(password, user.PasswordHash);
    }
    
    public async Task RegisterAsync(string username, string password)
    {
        var hashedPassword = MD5Encryption.Encrypt(password);
        await _userRepository.CreateUserAsync(username, hashedPassword);
    }
}

// 5. 注意：MD5 安全性
// MD5 算法适用于非敏感数据的加密场景
// 对于敏感数据（如用户密码），建议使用更安全的哈希算法
// 如 BCrypt、Argon2 等
```

### 5. Session 管理

**接口：** `IHttpSessionService`  
**实现：** `HttpSessionService`

Session 管理模块提供了便捷的 HTTP Session 操作方法，支持字符串和对象的存储与获取。

#### 主要功能
- 字符串存储：存储和获取字符串类型的 Session 数据
- 对象存储：将对象序列化为 JSON 存储到 Session
- 对象读取：从 Session 中读取 JSON 并反序列化为对象
- Session 清理：移除指定的 Session 数据

#### 使用示例

```csharp
using Acme.Sessions;

// 1. 在控制器中使用依赖注入
public class AccountController : Controller
{
    private readonly IHttpSessionService _sessionService;
    
    public AccountController(IHttpSessionService sessionService)
    {
        _sessionService = sessionService;
    }
    
    public IActionResult Login(string username, string password)
    {
        // 验证用户
        if (ValidateUser(username, password))
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
            
            // 存储用户权限
            _sessionService.SetSession("UserRole", "Admin");
            
            return RedirectToAction("Index");
        }
        
        return View("Login");
    }
    
    public IActionResult GetUserInfo()
    {
        // 读取数据
        string username = _sessionService.GetSession("CurrentUser");
        var userInfo = _sessionService.GetObjectFromJson<dynamic>("UserInfo");
        string userRole = _sessionService.GetSession("UserRole");
        
        ViewData["Username"] = username;
        ViewData["UserRole"] = userRole;
        
        return View(userInfo);
    }
    
    public IActionResult Logout()
    {
        // 清理 Session
        _sessionService.RemoveSession("CurrentUser");
        _sessionService.RemoveSession("UserInfo");
        _sessionService.RemoveSession("UserRole");
        
        return RedirectToAction("Login");
    }
    
    private bool ValidateUser(string username, string password)
    {
        // 实际验证逻辑
        return true;
    }
}

// 2. 在服务中使用
public class UserSessionService
{
    private readonly IHttpSessionService _sessionService;
    
    public UserSessionService(IHttpSessionService sessionService)
    {
        _sessionService = sessionService;
    }
    
    public void SetCurrentUser(User user)
    {
        _sessionService.SetSession("UserId", user.Id.ToString());
        _sessionService.SetObjectAsJson("CurrentUser", user);
    }
    
    public User GetCurrentUser()
    {
        return _sessionService.GetObjectFromJson<User>("CurrentUser");
    }
    
    public bool IsUserLoggedIn()
    {
        return !string.IsNullOrEmpty(_sessionService.GetSession("UserId"));
    }
}
```

### 6. Cookie 管理

**接口：** `IHttpCookieService`  
**实现：** `HttpCookieService`

Cookie 管理模块提供了便捷的 HTTP Cookie 操作方法，支持设置、获取和删除 Cookie。

#### 主要功能
- 设置 Cookie：创建带有过期时间的 Cookie
- 获取 Cookie：读取指定名称的 Cookie 值
- 删除 Cookie：移除指定名称的 Cookie
- 支持自定义过期时间

#### 使用示例

```csharp
using Acme.Cookie;

// 1. 在控制器中使用依赖注入
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
        _cookieService.SetCookies("UserPreferences", "theme=dark&language=zh-CN", 300);
        
        // 自定义过期时间（60分钟）
        _cookieService.SetCookies("AuthToken", "abc123xyz", 60);
        
        // 设置短期 Cookie（会话结束后过期）
        _cookieService.SetCookies("SessionId", "session123", 0);
        
        return View();
    }
    
    public IActionResult ReadCookie()
    {
        string token = _cookieService.GetCookies("AuthToken");
        string preferences = _cookieService.GetCookies("UserPreferences");
        string sessionId = _cookieService.GetCookies("SessionId");
        
        ViewData["Token"] = token;
        ViewData["Preferences"] = preferences;
        ViewData["SessionId"] = sessionId;
        
        return View();
    }
    
    public IActionResult DeleteCookie()
    {
        _cookieService.DeleteCookies("AuthToken");
        _cookieService.DeleteCookies("UserPreferences");
        return RedirectToAction("Index");
    }
}

// 2. 在服务中使用
public class CookieManagerService
{
    private readonly IHttpCookieService _cookieService;
    
    public CookieManagerService(IHttpCookieService cookieService)
    {
        _cookieService = cookieService;
    }
    
    public void SetAuthToken(string token, int expirationMinutes = 60)
    {
        _cookieService.SetCookies("AuthToken", token, expirationMinutes);
    }
    
    public string GetAuthToken()
    {
        return _cookieService.GetCookies("AuthToken");
    }
    
    public void ClearAuthToken()
    {
        _cookieService.DeleteCookies("AuthToken");
    }
    
    public void SetUserPreferences(UserPreferences preferences)
    {
        string preferencesJson = preferences.ToJson();
        _cookieService.SetCookies("UserPreferences", preferencesJson, 43200); // 30天
    }
    
    public UserPreferences GetUserPreferences()
    {
        string preferencesJson = _cookieService.GetCookies("UserPreferences");
        return string.IsNullOrEmpty(preferencesJson) 
            ? new UserPreferences() 
            : preferencesJson.ToEntity<UserPreferences>();
    }
}

public class UserPreferences
{
    public string Theme { get; set; } = "light";
    public string Language { get; set; } = "zh-CN";
    public bool RememberMe { get; set; } = false;
}
```

### 7. 拼音处理

**所在文件：** `Pinyin.cs`

拼音处理模块提供了中文转拼音的功能，支持获取汉字的拼音首字母。

#### 主要功能
- 中文转拼音首字母：将中文字符串转换为拼音首字母组合
- 编码转换：支持不同编码之间的转换（如果可用）

#### 使用示例

```csharp
using Acme.Pinyins;

// 1. 中文转拼音首字母
string chinese = "中文拼音处理";
string initials = Pinyin.GetInitials(chinese); // 输出: "ZWPYCL"

// 2. 处理包含特殊字符的文本
string mixedText = "中文123拼音";
string mixedInitials = Pinyin.GetInitials(mixedText); // 输出: "ZWPY"

// 3. 编码转换（如果可用）
string encodedText = Pinyin.ConvertEncoding("文本内容", "UTF-8");

// 4. 使用场景示例 - 搜索功能
public class SearchService
{
    public string GenerateSearchKey(string chineseName)
    {
        return Pinyin.GetInitials(chineseName).ToLower();
    }
    
    public List<User> SearchUsers(string keyword)
    {
        // 生成拼音首字母作为搜索键
        string pinyinKey = Pinyin.GetInitials(keyword).ToLower();
        
        // 在数据库中搜索
        return _userRepository.SearchByPinyinKey(pinyinKey);
    }
}

// 5. 使用场景示例 - 排序功能
public class SortService
{
    public List<Product> SortProductsByPinyin(List<Product> products)
    {
        return products.OrderBy(p => 
            Pinyin.GetInitials(p.Name)
        ).ToList();
    }
}

// 6. 使用场景示例 - 用户名生成
public class UserService
{
    public string GenerateUsername(string realName)
    {
        string pinyinInitials = Pinyin.GetInitials(realName);
        string timestamp = DateTime.Now.ToString("yyyyMMdd");
        return $"{pinyinInitials.ToLower()}_{timestamp}";
    }
}
```

### 8. Excel 处理

**基于 NPOI 的 Excel 操作**

Excel 处理模块提供了基于 NPOI 库的 Excel 文件操作功能，支持数据导入和导出。

#### 主要功能
- Excel 导入：从 Excel 文件中读取数据并转换为对象列表
- Excel 导出：将对象列表导出到 Excel 文件
- 支持不同数据类型的自动转换

#### 使用示例

```csharp
using Acme.Excel;

// 1. 定义数据模型
public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
}

// 2. Excel 服务封装
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
    
    // 导出带标题的数据
    public void ExportProductsWithHeader(List<Product> products, string outputPath, string sheetName = "Products")
    {
        var excelService = new NpolExcelService<Product>();
        // 自定义标题映射
        var headerMapping = new Dictionary<string, string>
        {
            { "Name", "产品名称" },
            { "Price", "价格" },
            { "Stock", "库存" },
            { "CreatedDate", "创建日期" },
            { "IsActive", "是否激活" }
        };
        excelService.ExportToExcel(products, outputPath, sheetName, headerMapping);
    }
}

// 3. 使用示例
public class ProductController : Controller
{
    private readonly ExcelService _excelService;
    private readonly IProductRepository _productRepository;
    
    public ProductController(ExcelService excelService, IProductRepository productRepository)
    {
        _excelService = excelService;
        _productRepository = productRepository;
    }
    
    // 导出产品数据
    public IActionResult ExportProducts()
    {
        var products = _productRepository.GetAll();
        string filePath = Path.Combine(App.Path, "exports", $"products_{DateTime.Now:yyyyMMdd}.xlsx");
        
        // 确保导出目录存在
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        
        _excelService.ExportProductsWithHeader(products, filePath);
        
        return PhysicalFile(filePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
            Path.GetFileName(filePath));
    }
    
    // 导入产品数据
    [HttpPost]
    public async Task<IActionResult> ImportProducts(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("请选择要导入的文件");
        
        string tempPath = Path.Combine(App.Path, "temp", Guid.NewGuid().ToString() + ".xlsx");
        
        // 确保临时目录存在
        Directory.CreateDirectory(Path.GetDirectoryName(tempPath));
        
        using (var stream = new FileStream(tempPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        
        try
        {
            var products = _excelService.ImportProducts(tempPath);
            foreach (var product in products)
            {
                await _productRepository.AddAsync(product);
            }
            return RedirectToAction("Index", new { success = true });
        }
        catch (Exception ex)
        {
            return BadRequest($"导入失败: {ex.Message}");
        }
        finally
        {
            // 清理临时文件
            if (System.IO.File.Exists(tempPath))
                System.IO.File.Delete(tempPath);
        }
    }
}

// 4. 实际使用示例
var products = new List<Product>
{
    new Product { Name = "笔记本电脑", Price = 5999.99m, Stock = 50, CreatedDate = DateTime.Now, IsActive = true },
    new Product { Name = "无线鼠标", Price = 89.90m, Stock = 100, CreatedDate = DateTime.Now, IsActive = true },
    new Product { Name = "键盘", Price = 199.99m, Stock = 75, CreatedDate = DateTime.Now, IsActive = false }
};

var excelService = new ExcelService();
excelService.ExportProductsWithHeader(products, "products.xlsx", "产品列表");
```

### 9. 验证码生成

**所在文件：** `VerCode.cs`

验证码生成模块提供了简洁的验证码生成功能，支持不同长度的数字验证码。

#### 主要功能
- 数字验证码生成：生成指定长度的数字验证码
- 适用于手机验证码、邮箱验证码等场景

#### 使用示例

```csharp
using Acme.Tools;

// 1. 基本使用
string sixDigitCode = VerCode.GetCode(6); // 6位数字验证码
string fourDigitCode = VerCode.GetCode(4); // 4位数字验证码
string eightDigitCode = VerCode.GetCode(8); // 8位数字验证码

// 2. 在注册流程中使用
public class VerificationService
{
    private readonly ICacheService _cacheService;
    
    public VerificationService(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }
    
    public string GeneratePhoneVerificationCode(string phoneNumber)
    {
        string code = VerCode.GetCode(6); // 生成6位手机验证码
        
        // 存储验证码到缓存，有效期5分钟
        _cacheService.Set($"PhoneVerification:{phoneNumber}", code, TimeSpan.FromMinutes(5));
        
        // 发送验证码到手机（实际项目中实现）
        // _smsService.SendSms(phoneNumber, $"您的验证码是：{code}，有效期5分钟");
        
        return code;
    }
    
    public string GenerateEmailVerificationCode(string email)
    {
        string code = VerCode.GetCode(8); // 生成8位邮箱验证码
        
        // 存储验证码到缓存，有效期10分钟
        _cacheService.Set($"EmailVerification:{email}", code, TimeSpan.FromMinutes(10));
        
        // 发送验证码到邮箱（实际项目中实现）
        // _emailService.SendEmail(email, "验证码", $"您的验证码是：{code}，有效期10分钟");
        
        return code;
    }
    
    public bool VerifyPhoneCode(string phoneNumber, string code)
    {
        string storedCode = _cacheService.Get<string>($"PhoneVerification:{phoneNumber}");
        return storedCode == code;
    }
    
    public bool VerifyEmailCode(string email, string code)
    {
        string storedCode = _cacheService.Get<string>($"EmailVerification:{email}");
        return storedCode == code;
    }
}

// 3. 在登录流程中使用
public class LoginController : Controller
{
    private readonly VerificationService _verificationService;
    
    public LoginController(VerificationService verificationService)
    {
        _verificationService = verificationService;
    }
    
    [HttpPost]
    public IActionResult SendPhoneCode(string phoneNumber)
    {
        _verificationService.GeneratePhoneVerificationCode(phoneNumber);
        return Json(new { success = true, message = "验证码已发送" });
    }
    
    [HttpPost]
    public IActionResult LoginWithPhone(string phoneNumber, string code)
    {
        if (_verificationService.VerifyPhoneCode(phoneNumber, code))
        {
            // 登录成功逻辑
            return RedirectToAction("Index");
        }
        
        return View("Login", new { error = "验证码错误" });
    }
}

// 4. 其他使用场景
public class SecurityService
{
    public string GenerateTransactionCode()
    {
        // 生成交易验证码
        return VerCode.GetCode(6);
    }
    
    public string GenerateResetPasswordCode()
    {
        // 生成重置密码验证码
        return VerCode.GetCode(8);
    }
}
```

### 10. 验证工具

**所在文件：** `VerificationHelper.cs`

验证工具模块提供了一系列实用的验证方法，包括 Guid 验证和正则表达式验证。

#### 主要功能
- Guid 验证：检查 Guid 是否为默认值
- 正则表达式验证：使用正则表达式验证字符串格式
- 适用于邮箱、手机号、身份证号等格式验证

#### 使用示例

```csharp
using Acme.Tools;

// 1. Guid 验证
Guid testGuid = Guid.NewGuid();
bool isDefaultGuid = testGuid.IsDefault(); // 检查是否为默认值

// 2. 正则表达式验证
string email = "user@example.com";
bool isValidEmail = email.RegexVerification(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$");

string phone = "13800138000";
bool isValidPhone = phone.RegexVerification(@"^1[3-9]\d{9}$");

// 3. 常用验证模式
public static class ValidationPatterns
{
    public const string EmailPattern = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
    public const string PhonePattern = @"^1[3-9]\d{9}$";
    public const string IdCardPattern = @"^[1-9]\d{5}(18|19|20)\d{2}(0[1-9]|1[0-2])(0[1-9]|[12]\d|3[01])\d{3}[\dXx]$";
    public const string UrlPattern = @"^https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)$";
}

// 4. 扩展方法使用
public class UserValidator
{
    public bool ValidateUserInput(User user)
    {
        if (user.Id.IsDefault())
            return false;
            
        if (!user.Email.RegexVerification(ValidationPatterns.EmailPattern))
            return false;
            
        if (!user.Phone.RegexVerification(ValidationPatterns.PhonePattern))
            return false;
            
        return true;
    }
}

// 5. 在模型验证中使用
public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string IdCard { get; set; }
}

public class RegistrationService
{
    public bool ValidateRegistration(User user)
    {
        // 验证 Guid
        if (user.Id.IsDefault())
        {
            Console.WriteLine("用户ID无效");
            return false;
        }
        
        // 验证邮箱
        if (!user.Email.RegexVerification(ValidationPatterns.EmailPattern))
        {
            Console.WriteLine("邮箱格式不正确");
            return false;
        }
        
        // 验证手机号
        if (!user.Phone.RegexVerification(ValidationPatterns.PhonePattern))
        {
            Console.WriteLine("手机号格式不正确");
            return false;
        }
        
        // 验证身份证号
        if (!string.IsNullOrEmpty(user.IdCard) && 
            !user.IdCard.RegexVerification(ValidationPatterns.IdCardPattern))
        {
            Console.WriteLine("身份证号格式不正确");
            return false;
        }
        
        return true;
    }
}

// 6. 在控制器中使用
public class UserController : Controller
{
    private readonly RegistrationService _registrationService;
    
    public UserController(RegistrationService registrationService)
    {
        _registrationService = registrationService;
    }
    
    [HttpPost]
    public IActionResult Register(User user)
    {
        if (!_registrationService.ValidateRegistration(user))
        {
            return BadRequest(ModelState);
        }
        
        // 注册逻辑
        return RedirectToAction("Success");
    }
}
```

### 11. 配置管理

**配置文件读取工具**

配置管理模块提供了便捷的配置文件读取功能，支持从 appsettings.json 和自定义 JSON 文件中读取配置。

#### 主要功能
- 配置读取：从 appsettings.json 中读取配置值
- 自定义配置：从指定的 JSON 文件中读取配置
- 路径信息：获取应用程序路径信息
- 支持嵌套配置项的读取

#### 使用示例

```csharp
using Acme.Apps;
using Acme.JsonFiles;

// 1. 使用 App 类读取配置
string dbConnection = App.GetValue("ConnectionStrings:DefaultConnection");
string appName = App.GetValue("AppSettings:ApplicationName");
string logLevel = App.GetValue("Logging:LogLevel:Default");

// 2. 自定义配置文件
var customConfig = new JsonFile("customSettings.json");
string apiKey = customConfig.GetValue("ThirdParty:ApiKey");
string endpoint = customConfig.GetValue("ThirdParty:Endpoint");

// 3. 路径信息
string basePath = App.Path;           // 应用程序域基础目录
string workingPath = App.SystemPath;  // 当前工作目录

// 4. 配置类封装示例
public class AppConfig
{
    public static string DatabaseConnection => 
        App.GetValue("ConnectionStrings:DefaultConnection");
        
    public static string ApiBaseUrl => 
        App.GetValue("AppSettings:ApiBaseUrl");
        
    public static bool EnableCache => 
        bool.Parse(App.GetValue("AppSettings:EnableCache") ?? "false");
        
    public static int Timeout => 
        int.Parse(App.GetValue("AppSettings:Timeout") ?? "30");
}

// 5. 分层配置管理
public static class ConfigurationManager
{
    public static class Database
    {
        public static string ConnectionString => 
            App.GetValue("ConnectionStrings:DefaultConnection");
        
        public static string Provider => 
            App.GetValue("ConnectionStrings:Provider");
    }
    
    public static class Api
    {
        public static string BaseUrl => 
            App.GetValue("Api:BaseUrl");
        
        public static string Key => 
            App.GetValue("Api:Key");
        
        public static int Timeout => 
            int.Parse(App.GetValue("Api:Timeout") ?? "30");
    }
    
    public static class Logging
    {
        public static string Level => 
            App.GetValue("Logging:LogLevel:Default");
        
        public static string FilePath => 
            App.GetValue("Logging:FilePath");
    }
}

// 6. 实际使用示例
public class UserService
{
    public async Task<List<User>> GetUsers()
    {
        using (var db = new DbContext(ConfigurationManager.Database.ConnectionString))
        {
            return await db.Users.ToListAsync();
        }
    }
    
    public async Task<string> CallApi(string endpoint)
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri(ConfigurationManager.Api.BaseUrl);
        client.Timeout = TimeSpan.FromSeconds(ConfigurationManager.Api.Timeout);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {ConfigurationManager.Api.Key}");
        
        var response = await client.GetAsync(endpoint);
        return await response.Content.ReadAsStringAsync();
    }
}

// 7. 配置变更监听（如果支持）
// 注意：实际实现可能因版本不同而有所差异
// public class ConfigChangeMonitor
// {
//     public event Action OnConfigChanged;
//     
//     public void StartMonitoring()
//     {
//         // 实现配置文件变更监听逻辑
//     }
// }
```

## 最佳实践

### 1. 依赖注入配置

#### 基本配置

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// 注册 Acme 服务
builder.Services.AddAcmeBuilderServer();

// 注册自定义服务
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<IExcelService, ExcelService>();
builder.Services.AddSingleton<ICacheService, CacheService>();

var app = builder.Build();
```

#### 服务生命周期管理

```csharp
// 不同生命周期的服务注册
// Scoped：每个请求创建一个实例
builder.Services.AddScoped<IUserService, UserService>();

// Transient：每次注入时创建一个新实例
builder.Services.AddTransient<IEmailService, EmailService>();

// Singleton：整个应用程序生命周期中只创建一个实例
builder.Services.AddSingleton<ICacheService, CacheService>();
```

### 2. 错误处理策略

#### 安全数据处理

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
    
    public async Task<T> SafeApiCall<T>(Func<Task<T>> apiCall, T defaultValue = default)
    {
        try
        {
            return await apiCall();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"API调用失败: {ex.Message}");
            return defaultValue;
        }
    }
}
```

#### 全局异常处理

```csharp
// 在 Program.cs 中配置全局异常处理
app.UseExceptionHandler(options =>
{
    options.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>();
        if (exception != null)
        {
            // 记录异常
            Console.WriteLine($"全局异常: {exception.Error.Message}");
            
            // 返回友好错误信息
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new
            {
                success = false,
                message = "服务器内部错误"
            });
        }
    });
});
```

### 3. 配置管理示例

#### 分层配置结构

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
    
    public static class Security
    {
        public static string JwtSecret => App.GetValue("Security:JwtSecret");
        public static int JwtExpirationHours => int.Parse(App.GetValue("Security:JwtExpirationHours") ?? "24");
    }
}
```

#### 配置验证

```csharp
public class ConfigurationValidator
{
    public static bool ValidateConfig()
    {
        var errors = new List<string>();
        
        // 验证数据库连接字符串
        if (string.IsNullOrEmpty(GlobalSettings.Database.ConnectionString))
        {
            errors.Add("数据库连接字符串未配置");
        }
        
        // 验证 API 配置
        if (string.IsNullOrEmpty(GlobalSettings.Api.BaseUrl))
        {
            errors.Add("API 基础 URL 未配置");
        }
        
        // 验证安全配置
        if (string.IsNullOrEmpty(GlobalSettings.Security.JwtSecret))
        {
            errors.Add("JWT 密钥未配置");
        }
        
        // 输出错误信息
        if (errors.Count > 0)
        {
            Console.WriteLine("配置验证失败:");
            foreach (var error in errors)
            {
                Console.WriteLine($"- {error}");
            }
            return false;
        }
        
        return true;
    }
}

// 在应用启动时验证配置
if (!ConfigurationValidator.ValidateConfig())
{
    Console.WriteLine("配置验证失败，应用无法启动");
    return;
}
```

### 4. 性能优化建议

#### 缓存策略

```csharp
public class CachedUserService : IUserService
{
    private readonly IUserService _userService;
    private readonly ICacheService _cacheService;
    
    public CachedUserService(IUserService userService, ICacheService cacheService)
    {
        _userService = userService;
        _cacheService = cacheService;
    }
    
    public async Task<User> GetUserById(int id)
    {
        string cacheKey = $"User:{id}";
        
        // 尝试从缓存获取
        var cachedUser = _cacheService.Get<User>(cacheKey);
        if (cachedUser != null)
        {
            return cachedUser;
        }
        
        // 从数据库获取
        var user = await _userService.GetUserById(id);
        
        // 存入缓存，有效期10分钟
        _cacheService.Set(cacheKey, user, TimeSpan.FromMinutes(10));
        
        return user;
    }
}
```

#### 异步操作

```csharp
public class UserController : Controller
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    // 使用异步方法提高性能
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        try
        {
            var user = await _userService.GetUserById(id);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
```

### 5. 安全最佳实践

#### 密码处理

```csharp
public class SecurityService
{
    // 注意：MD5 适用于非敏感场景，敏感数据建议使用更安全的算法
    public string HashPassword(string password)
    {
        // 可以添加盐值增强安全性
        string salt = GenerateSalt();
        return MD5Encryption.Encrypt(password + salt) + ":" + salt;
    }
    
    public bool VerifyPassword(string inputPassword, string storedHash)
    {
        // 提取盐值
        var parts = storedHash.Split(':');
        if (parts.Length != 2)
            return false;
        
        string hash = parts[0];
        string salt = parts[1];
        
        // 验证密码
        string inputHash = MD5Encryption.Encrypt(inputPassword + salt);
        return inputHash == hash;
    }
    
    private string GenerateSalt()
    {
        return Guid.NewGuid().ToString().Substring(0, 8);
    }
}
```

#### 输入验证

```csharp
public class InputValidator
{
    public static bool ValidateUserInput(UserInput input)
    {
        if (string.IsNullOrEmpty(input.Username))
            return false;
        
        if (!input.Email.RegexVerification(ValidationPatterns.EmailPattern))
            return false;
        
        if (!input.Phone.RegexVerification(ValidationPatterns.PhonePattern))
            return false;
        
        return true;
    }
}

// 在控制器中使用
[HttpPost]
public IActionResult Register(UserInput input)
{
    if (!InputValidator.ValidateUserInput(input))
    {
        return BadRequest(new { message = "输入数据无效" });
    }
    
    // 注册逻辑
    return Ok(new { message = "注册成功" });
}
```

## 依赖项说明

| 依赖包 | 用途 | 建议版本 |
|--------|------|----------|
| Newtonsoft.Json | JSON 序列化/反序列化 | 13.0.4 或更高 |
| NPOI | Excel 文件操作 | 2.7.5 或更高 |
| Microsoft.Extensions.DependencyInjection | 依赖注入容器 | 9.0.11 或更高（.NET 8.0/9.0），10.0.2 或更高（.NET 10.0） |
| Microsoft.Extensions.Caching.Abstractions | 缓存抽象 | 9.0.11 或更高（.NET 8.0/9.0），10.0.2 或更高（.NET 10.0） |
| Swashbuckle.AspNetCore | API 文档生成 | 9.0.6 或更高（.NET 8.0/9.0），10.1.0 或更高（.NET 10.0） |
| ZKWeb.System.Drawing | 绘图功能 | 4.0.1 或更高 |

### 安装依赖项

使用 NuGet 包管理器安装所需依赖：

```bash
# 安装 Newtonsoft.Json
dotnet add package Newtonsoft.Json

# 安装 NPOI
dotnet add package NPOI

# 安装依赖注入容器
dotnet add package Microsoft.Extensions.DependencyInjection

# 安装缓存抽象
dotnet add package Microsoft.Extensions.Caching.Abstractions

# 安装 API 文档生成（可选）
dotnet add package Swashbuckle.AspNetCore

# 安装绘图功能（可选）
dotnet add package ZKWeb.System.Drawing
```

### 版本兼容性

- .NET 8.0：所有依赖包均支持
- .NET 9.0：所有依赖包均支持
- .NET 10.0：所有依赖包均支持

## 注意事项

### 1. 版本兼容性

- 确保使用的 .NET 版本在 8.0 及以上
- 不同 .NET 版本可能需要不同版本的依赖包，请根据实际情况选择合适的版本

### 2. 错误处理

- HTTP 请求等方法返回字符串结果，建议封装异常处理
- 对于可能失败的操作，建议使用 try-catch 块捕获异常
- 考虑使用 `SafeDataProcessor` 类提供的安全处理方法

### 3. 性能考虑

- 频繁的 Session/Cookie 操作可能影响性能，请合理使用
- 对于频繁访问的数据，考虑使用缓存机制
- 大型 Excel 文件的导入导出可能消耗较多内存，建议在后台线程中执行
- 异步操作可以提高应用程序的响应速度，建议在适当场景下使用

### 4. 安全建议

- 敏感数据建议使用更安全的加密方式，MD5 适用于非敏感场景
- 对于用户密码等敏感信息，建议使用 BCrypt、Argon2 等更安全的哈希算法
- 输入验证非常重要，建议对所有用户输入进行验证
- 避免在配置文件中存储敏感信息，如数据库密码、API 密钥等

### 5. 配置管理

- 确保配置文件存在且包含必要的配置项
- 对于不同环境（开发、测试、生产），建议使用不同的配置文件
- 考虑使用环境变量覆盖配置文件中的敏感信息

### 6. 路径处理

- 不同操作系统的路径分隔符不同，建议使用 `Path` 类处理路径
- 确保应用程序具有对所需目录的读写权限
- 对于临时文件，使用 `Path.GetTempFileName()` 或类似方法

### 7. 依赖注入

- 正确注册服务的生命周期，避免内存泄漏
- 避免在 Singleton 服务中注入 Scoped 或 Transient 服务
- 考虑使用接口隔离原则，减少服务之间的耦合

## 技术支持

### 常见问题排查

如在使用过程中遇到问题，请按照以下步骤排查：

1. **依赖注入检查**
   - 是否正确注册了 Acme 服务：`builder.Services.AddAcmeBuilderServer();`
   - 是否正确注册了其他所需服务

2. **配置文件检查**
   - 配置文件是否存在且包含必要的配置项
   - 配置项的路径是否正确（如 `ConnectionStrings:DefaultConnection`）

3. **版本兼容性检查**
   - 使用的 .NET 版本是否在 8.0 及以上
   - 相关依赖包版本是否与 .NET 版本兼容

4. **权限检查**
   - 应用程序是否具有对所需目录的读写权限
   - 数据库连接是否具有正确的权限

5. **错误信息分析**
   - 检查应用程序日志中的错误信息
   - 检查控制台输出的错误信息

### 联系支持

如果上述步骤无法解决问题，请通过以下方式联系技术支持：

- **电子邮件**：yzxs949@163.com
- **文档**：参考项目文档和示例代码

### 提交问题时请提供以下信息

- Acme 库版本
- .NET 版本
- 操作系统版本
- 完整的错误信息
- 重现问题的步骤
- 相关代码片段

### 故障排除示例

**问题**：无法读取配置文件

**解决方案**：
1. 确保 appsettings.json 文件存在于应用程序根目录
2. 确保配置文件格式正确（有效的 JSON 格式）
3. 确保配置项路径正确

**问题**：Excel 导入失败

**解决方案**：
1. 确保安装了 NPOI 依赖包
2. 确保 Excel 文件格式正确
3. 确保应用程序具有对文件的读取权限
4. 检查数据模型与 Excel 列的映射是否正确

---

*本文档基于 Acme 类库版本 5.6.2 云中小生编写，具体实现可能因版本更新而有所变化。*
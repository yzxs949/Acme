Acme 类库文档
概述
Acme 是一个 .NET 工具类库，提供了一系列常用的功能扩展和工具类，旨在简化日常开发工作。当前版本为 5.5.2.1，支持 .NET 6.0、7.0、8.0 和 9.0。该类库包含 JSON 处理、数据类型转换、HTTP 请求、加密工具等功能模块。
核心功能模块与代码示例
1. JSON 处理 (Json.cs)
功能描述：提供 JSON 序列化和反序列化的扩展方法。
using Acme.Tools;

// 序列化示例
var obj = new { Name = "Test", Value = 123 };
string json = obj.ToJson();
Console.WriteLine(json); // 输出: {"Name":"Test","Value":123}

// 反序列化示例
string jsonString = "{\"Name\":\"Test\",\"Value\":123}";
var result = jsonString.ToEntity<dynamic>();
Console.WriteLine(result.Name); // 输出: Test

2. 数据类型转换 (ConvHelper.cs)
功能描述：提供各种数据类型转换的扩展方法。
using Acme.Tools;

// 字符串转数字
string numStr = "123";
int num = numStr.ToInt(); // 返回 123
string invalidStr = "abc";
int defaultNum = invalidStr.ToInt(0); // 返回 0（默认值）

// 路径转换
string absolutePath = @"C:\project\files\test.txt";
string relativePath = absolutePath.ToUrlPath(); // 转换为相对路径

// 随机码生成
string randomCode = "0,1,2,3,4,5".ToRandomCode(6); // 生成6位随机数字串

// 时间戳获取
long timestamp = ConvHelper.GetTimeStamp(); // 获取13位时间戳

3. HTTP 请求 (HttpHelper.cs)
功能描述：提供 HTTP 请求的简单封装。
using Acme.Tools;

// POST 请求示例
var postData = new { userId = 1, action = "query" };
string postResponse = HttpHelper.HttpClientPost(" https://api.example.com/post ", postData);
Console.WriteLine(postResponse);

// GET 请求示例
string getResponse = HttpHelper.HttpClientGet(" https://api.example.com/get?id=1 ");
Console.WriteLine(getResponse);

4. 依赖注入扩展 (AcmeServiceCollectionExtensions.cs)
功能描述：提供 DI 容器的扩展方法，用于注册 Acme 相关服务。
using Microsoft.Extensions.DependencyInjection;
using Acme;

// 在 Startup.cs 或 Program.cs 中配置服务
var services = new ServiceCollection();
services.AddAcmeBuilderServer(); // 注册 HTTP 上下文、Cookie 和 Session 服务

// 使用示例：在控制器中注入服务
public class HomeController
{
    private readonly IHttpCookieService _cookieService;
    private readonly IHttpSessionService _sessionService;
    
    public HomeController(IHttpCookieService cookieService, IHttpSessionService sessionService)
    {
        _cookieService = cookieService;
        _sessionService = sessionService;
    }
}

5. 加密工具 (MD5Encryption.cs)
功能描述：提供 MD5 加密功能。
using Acme.Tools;

string originalText = "password123";
string encrypted = MD5Encryption.Encrypt(originalText); // 默认小写
string encryptedUpper = MD5Encryption.Encrypt(originalText, true); // 大写输出

// 比较 MD5 值
bool isMatch = MD5Encryption.Compare("inputText", "storedHash");

6. Session 管理 (IHttpSessionService.cs, HttpSessionService.cs)
功能描述：提供 Session 操作的接口和实现。
using Acme.Sessions;

// 设置和获取字符串 Session
_sessionService.SetSession("user", "admin");
string user = _sessionService.GetSession("user"); // 返回 "admin"

// 设置和获取对象 Session
var userInfo = new { Name = "John", Age = 30 };
_sessionService.SetObjectAsJson("userInfo", userInfo);
var retrievedUser = _sessionService.GetObjectFromJson<dynamic>("userInfo");

// 移除 Session
_sessionService.RemoveSession("user");

7. Cookie 管理 (IHttpCookieService.cs, HttpCookieService.cs)
功能描述：提供 Cookie 操作的接口和实现。
using Acme.Cookie;

// 设置 Cookie（过期时间默认为300分钟）
_cookieService.SetCookies("authToken", "abc123", 60); // 60分钟后过期

// 获取 Cookie
string token = _cookieService.GetCookies("authToken");

// 删除 Cookie
_cookieService.DeleteCookies("authToken");

8. 拼音处理 (Pinyin.cs)
功能描述：提供中文转拼音的功能。
using Acme.Pinyins;

string chineseText = "中文拼音";
string initials = Pinyin.GetInitials(chineseText); // 获取首字母缩写，如 "ZWPY"

// 编码转换示例（如果方法可用）
string converted = Pinyin.ConvertEncoding("text", "UTF-8");

9. Excel 处理 (NpolExcelHelper.cs, NpolExcelService.cs)
功能描述：基于 NPOI 的 Excel 导入导出功能。
using Acme.Excel;

// 导入 Excel 数据
var excelService = new NpolExcelService<MyEntity>();
List<MyEntity> dataList = excelService.ImportFromExcel("path/to/file.xlsx");

// 导出数据到 Excel
List<MyEntity> entities = new List<MyEntity> { ... };
excelService.ExportToExcel(entities, "output.xlsx");

10. 验证码生成 (VerCode.cs)
功能描述：提供验证码生成功能。
using Acme.Tools;

// 生成数字验证码
string numericCode = VerCode.GetCode(6); // 6位数字验证码

// 生成算式验证码（如果方法可用）
var equationCode = VerCode.GetEquationCode(); // 返回算式和答案

11. 验证工具 (VerificationHelper.cs)
功能描述：提供验证相关的扩展方法。
using Acme.Tools;

// 检查 Guid 是否为默认值
Guid testGuid = Guid.NewGuid();
bool isDefault = testGuid.IsDefault(); // 返回 false

// 正则表达式验证（示例：验证邮箱）
string email = "test@example.com";
bool isValid = email.RegexVerification(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$");

12. 配置管理 (JsonFile.cs, App.cs)
功能描述：提供配置文件读取功能。
using Acme.Apps;
using Acme.JsonFiles;

// 使用 App 类读取配置
string databaseConnection = App.GetValue("ConnectionStrings", "DefaultConnection");
string appName = App.GetValue("AppSettings:Name");

// 直接使用 JsonFile 类
var jsonFile = new JsonFile("custom.json");
string value = jsonFile.GetValue("Section", "Key");

// 获取系统路径
string currentPath = App.Path; // 应用程序域基础目录
string systemPath = App.SystemPath; // 当前工作目录

注意事项

版本兼容性：当前版本为 5.5.2.1，支持 .NET 6.0 及以上版本。
代码问题：部分类中存在重复定义的方法或属性（如 Json.cs 中的方法重载冲突），使用时需参考具体代码实现。
错误处理：HTTP 请求等方法返回异常信息字符串，建议在实际使用中添加异常处理。

依赖项

Newtonsoft.Json（用于 JSON 处理）
NPOI（用于 Excel 操作）
Microsoft.Extensions.DependencyInjection（用于依赖注入）
Microsoft.AspNetCore.Http（用于 HTTP 上下文访问）

许可证
Apache License 2.0
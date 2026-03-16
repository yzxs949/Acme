# App 类文档

## 概述

`App` 类是 Acme 库中的一个静态类，用于管理系统配置参数和获取应用程序路径信息。它提供了便捷的方法来读取配置文件中的值，并获取应用程序的基本路径。

## 命名空间

```csharp
using Acme.Apps;
```

## 核心功能

### 1. 配置文件读取

`App` 类使用 `JsonFile` 类来读取 `appsettings.json` 文件中的配置值。

#### 方法

| 方法签名 | 描述 | 返回值 |
|---------|------|--------|
| `GetValue(string key)` | 根据键获取配置值 | `string` - 配置值 |
| `GetValue(string key, string name)` | 根据键和名称获取配置值 | `string` - 配置值 |

#### 使用示例

```csharp
// 读取简单配置
string appName = App.GetValue("AppSettings:ApplicationName");
string dbConnection = App.GetValue("ConnectionStrings:DefaultConnection");

// 读取嵌套配置
string logLevel = App.GetValue("Logging:LogLevel:Default");
string apiKey = App.GetValue("ThirdParty:ApiKey");
```

### 2. 路径信息获取

`App` 类提供了两个静态属性来获取应用程序的路径信息。

#### 属性

| 属性名 | 描述 | 返回值 |
|-------|------|--------|
| `SystemPath` | 获取当前工作目录 | `string` - 当前工作目录路径 |
| `Path` | 获取应用程序域基础目录 | `string` - 应用程序基础目录路径 |

#### 使用示例

```csharp
// 获取应用程序基础目录
string basePath = App.Path;

// 获取当前工作目录
string workingPath = App.SystemPath;

// 构建文件路径
string logPath = Path.Combine(App.Path, "logs");
string tempPath = Path.Combine(App.Path, "temp");
```

## 实现细节

### 静态构造函数

```csharp
static App()
{
    JsonFileVariable = new("appsettings.json");
}
```

静态构造函数在类首次被使用时初始化 `JsonFileVariable`，使其指向 `appsettings.json` 文件。

### 内部字段

```csharp
private static JsonFile JsonFileVariable { get; set; }
```

`JsonFileVariable` 是一个静态字段，用于存储 `JsonFile` 实例，用于读取配置文件。

## 最佳实践

### 1. 配置管理封装

建议创建一个专门的配置管理类来封装对 `App` 类的调用，使配置访问更加集中和类型安全。

```csharp
public static class AppConfig
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
```

### 2. 路径管理

使用 `App.Path` 和 `App.SystemPath` 来构建应用程序所需的路径，避免硬编码路径。

```csharp
public static class PathManager
{
    public static string Logs => Path.Combine(App.Path, "logs");
    public static string Temp => Path.Combine(App.Path, "temp");
    public static string Uploads => Path.Combine(App.Path, "uploads");
    public static string Config => Path.Combine(App.Path, "config");
}
```

### 3. 错误处理

在读取配置值时，建议添加错误处理，以处理配置项不存在的情况。

```csharp
public static class SafeConfig
{
    public static string GetString(string key, string defaultValue = "")
    {
        try
        {
            string value = App.GetValue(key);
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }
        catch
        {
            return defaultValue;
        }
    }
    
    public static int GetInt(string key, int defaultValue = 0)
    {
        try
        {
            string value = App.GetValue(key);
            return int.TryParse(value, out int result) ? result : defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }
    
    public static bool GetBool(string key, bool defaultValue = false)
    {
        try
        {
            string value = App.GetValue(key);
            return bool.TryParse(value, out bool result) ? result : defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }
}
```

## 示例应用

### 完整配置管理示例

```csharp
using Acme.Apps;

public class Program
{
    public static void Main(string[] args)
    {
        // 读取配置
        string appName = App.GetValue("AppSettings:ApplicationName");
        string dbConnection = App.GetValue("ConnectionStrings:DefaultConnection");
        
        Console.WriteLine($"应用程序名称: {appName}");
        Console.WriteLine($"数据库连接: {dbConnection}");
        
        // 获取路径
        string basePath = App.Path;
        string workingPath = App.SystemPath;
        
        Console.WriteLine($"应用程序基础目录: {basePath}");
        Console.WriteLine($"当前工作目录: {workingPath}");
        
        // 构建自定义路径
        string logPath = Path.Combine(App.Path, "logs");
        Console.WriteLine($"日志目录: {logPath}");
        
        // 确保目录存在
        Directory.CreateDirectory(logPath);
        
        Console.WriteLine("配置读取完成！");
    }
}
```

## 注意事项

1. **配置文件位置**：`App` 类默认读取应用程序根目录下的 `appsettings.json` 文件。
2. **配置文件格式**：配置文件必须是有效的 JSON 格式。
3. **路径分隔符**：使用 `Path.Combine` 来构建路径，以确保跨平台兼容性。
4. **错误处理**：读取配置时可能会抛出异常，建议添加适当的错误处理。
5. **默认值**：对于可能不存在的配置项，建议提供默认值。

## 版本兼容性

- .NET 6.0：支持
- .NET 7.0：支持
- .NET 8.0：支持
- .NET 9.0：支持
- .NET 10.0：支持
```
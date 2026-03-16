# Acme.Dapper 使用文档

## 项目介绍

Acme.Dapper 是一个基于 Dapper 的轻量级 ORM 框架，提供了简洁易用的数据库操作接口，支持异步操作、事务处理、分页查询等功能。

当前版本：5.6.3

支持 .NET 8、.NET 9 和 .NET 10 平台。

## 核心功能

- 基础 CRUD 操作（增删改查）
- 异步操作支持
- 分页查询
- 事务支持
- 存储过程调用
- SQL 语句拼接

## 安装方法

1. 通过 NuGet 安装 Acme.Dapper 包
2. 在项目中添加引用
3. 确保项目引用了 Acme 基础库

## 快速开始

### 1. 注册服务

在 Startup.cs 或 Program.cs 中注册 Acme.Dapper 服务：

```csharp
using Acme.Dapper;

// 注册服务
builder.Services.AddAcmeDapper("Your Connection String");
```

### 2. 创建实体类

创建与数据库表对应的实体类，例如：

```csharp
using Dapper.Contrib.Extensions;

[Table("Users")]
public class User
{
    [Key]
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public DateTime CreatedTime { get; set; }
}
```

### 3. 使用仓储模式

在需要使用数据库操作的类中注入 `IRepository<T>`：

```csharp
using Acme.Dapper.Repository;

public class UserService
{
    private readonly IRepository<User> _userRepository;

    public UserService(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    // 使用示例
    public async Task<User> GetUserById(int id)
    {
        return await _userRepository.GetInfoAsync(id);
    }
}
```

## API 文档

### 仓储接口 (IRepository<T>)

#### 新增操作

- `Task<int> AddReturnIdAsync(T t)` - 异步新增并返回 ID
- `int AddReturnId(T t)` - 新增并返回 ID
- `Task<int> AddAsync(T t)` - 异步新增
- `int Add(T t)` - 新增

#### 删除操作

- `Task<bool> DeleteAsync(T t)` - 异步删除实体
- `Task<bool> DeleteAsync(int Id)` - 根据 ID 异步删除
- `bool Delete(T t)` - 删除实体
- `bool Delete(int Id)` - 根据 ID 删除

#### 修改操作

- `Task<int> UpdateReturnIdAsync(T t)` - 异步修改并返回 ID
- `int UpdateReturnId(T t)` - 修改并返回 ID
- `Task<bool> UpdateAsync(T t)` - 异步修改
- `Task<int> UpdateByIdAsync(object UpdateContext)` - 根据 ID 异步修改
- `bool Update(T t)` - 修改

#### 查询操作

- `T GetInfo(int Id)` - 根据 ID 获取单条数据
- `T GetInfoOrDefault(int Id)` - 根据 ID 获取单条数据，无数据返回默认值
- `T GetInfo(object SqlWhere)` - 按条件获取单条数据
- `T GetInfoOrDefault(object SqlWhere)` - 按条件获取单条数据，无数据返回默认值
- `Task<T> GetInfoAsync(int Id)` - 异步根据 ID 获取单条数据
- `Task<T> GetInfoOrDefaultAsync(int Id)` - 异步根据 ID 获取单条数据，无数据返回默认值
- `Task<T> GetInfoAsync(object SqlWhere)` - 异步按条件获取单条数据
- `Task<T> GetInfoOrDefaultAsync(object SqlWhere)` - 异步按条件获取单条数据，无数据返回默认值

#### 列表查询

- `List<T> GetList(object SqlWhere)` - 获取列表
- `Task<List<T>> GetListAsync(object SqlWhere)` - 异步获取列表
- `Task<int> GetCountAsync(object SqlWhere)` - 异步获取条数

#### 分页查询

- `PageList GetPageListBySql(string sql, int pageIndex, int pageSize, string orderField = "", OrderType orderType = OrderType.ASC)` - 根据 SQL 获取分页数据
- `Task<PageList> GetPageListBySqlAsync(string sql, int pageIndex, int pageSize, string orderField = "", OrderType orderType = OrderType.ASC)` - 异步根据 SQL 获取分页数据
- `PageList<TEntity> GetPageListBySql<TEntity>(string sql, int pageIndex, int pageSize, string orderField = "", OrderType orderType = OrderType.ASC)` - 根据 SQL 获取泛型分页数据
- `Task<PageList<TEntity>> GetPageListBySqlAsync<TEntity>(string sql, int pageIndex, int pageSize, string orderField = "", OrderType orderType = OrderType.ASC)` - 异步根据 SQL 获取泛型分页数据

### Extensions 扩展类

#### 查询操作

- `List<T> Query<T>(string sql, object param)` - 查询列表
- `Task<List<T>> QueryAsync<T>(string sql, object param)` - 异步查询列表
- `T QueryFirst<T>(string sql, object param)` - 查询第一个数据
- `Task<T> QueryFirstAsync<T>(string sql, object param)` - 异步查询第一个数据
- `T QueryFirstOrDefault<T>(string sql, object param)` - 查询第一个数据，无数据返回默认值
- `Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param)` - 异步查询第一个数据，无数据返回默认值
- `T QuerySingle<T>(string sql, object param)` - 查询单条数据
- `Task<T> QuerySingleAsync<T>(string sql, object param)` - 异步查询单条数据
- `T QuerySingleOrDefault<T>(string sql, object param)` - 查询单条数据，无数据返回默认值
- `Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param)` - 异步查询单条数据，无数据返回默认值

#### 执行操作

- `Task<int> ExecuteAsync(string sql, object param)` - 异步执行增删改语句
- `int Execute(string sql, object param)` - 执行增删改语句

#### 实体操作

- `Task<int> InsertAsync<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null)` - 异步插入实体
- `int Insert<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null)` - 插入实体
- `Task<bool> UpdateAsync<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null)` - 异步更新实体
- `bool Update<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null)` - 更新实体
- `Task<bool> DeleteAsync<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null)` - 异步删除实体
- `bool Delete<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null)` - 删除实体

#### 存储过程

- `List<T> ExecutePro<T>(string proc, object param)` - 执行存储过程
- `Task<List<T>> ExecuteProAsync<T>(string proc, object param)` - 异步执行存储过程

#### 事务操作

- `int ExecuteTransaction(string[] sqlarr)` - 执行事务（无参数）
- `Task<int> ExecuteTransactionAsync(string[] sqlarr)` - 异步执行事务（无参数）
- `int ExecuteTransaction(Dictionary<string, object> dic)` - 执行事务（带参数）
- `Task<int> ExecuteTransactionAsync(Dictionary<string, object> dic)` - 异步执行事务（带参数）

## 使用示例

### 1. 新增数据

```csharp
// 同步新增
var user = new User { UserName = "张三", Email = "zhangsan@example.com", Address = "北京" };
int id = _userRepository.AddReturnId(user);

// 异步新增
var user = new User { UserName = "李四", Email = "lisi@example.com", Address = "上海" };
int id = await _userRepository.AddReturnIdAsync(user);
```

### 2. 查询数据

```csharp
// 根据 ID 查询
var user = _userRepository.GetInfo(1);

// 按条件查询
var user = _userRepository.GetInfo(new { UserName = "张三" });

// 异步查询
var user = await _userRepository.GetInfoAsync(1);
```

### 3. 修改数据

```csharp
// 同步修改
var user = _userRepository.GetInfo(1);
user.UserName = "张三修改";
_userRepository.Update(user);

// 异步修改
var user = await _userRepository.GetInfoAsync(1);
user.UserName = "张三修改";
await _userRepository.UpdateAsync(user);
```

### 4. 删除数据

```csharp
// 同步删除
_userRepository.Delete(1);

// 异步删除
await _userRepository.DeleteAsync(1);
```

### 5. 列表查询

```csharp
// 按条件查询列表
var users = _userRepository.GetList(new { Address = "北京" });

// 异步查询列表
var users = await _userRepository.GetListAsync(new { Address = "北京" });
```

### 6. 分页查询

```csharp
// 分页查询
string sql = "SELECT * FROM Users WHERE Address = '北京'";
var pageList = _userRepository.GetPageListBySql<User>(sql, 1, 10, "Id", OrderType.DESC);

// 异步分页查询
string sql = "SELECT * FROM Users WHERE Address = '北京'";
var pageList = await _userRepository.GetPageListBySqlAsync<User>(sql, 1, 10, "Id", OrderType.DESC);
```

### 7. 执行事务

```csharp
// 执行事务
var dic = new Dictionary<string, object>();
dic.Add("INSERT INTO Users (UserName, Email) VALUES (@UserName, @Email)", new { UserName = "王五", Email = "wangwu@example.com" });
dic.Add("UPDATE Users SET Address = @Address WHERE Id = @Id", new { Address = "广州", Id = 1 });

int result = _extensions.ExecuteTransaction(dic);
```

### 8. 执行存储过程

```csharp
// 执行存储过程
var param = new { UserId = 1 };
var users = _extensions.ExecutePro<User>("GetUserById", param);
```

## 注意事项

1. 实体类需要使用 `Dapper.Contrib.Extensions` 命名空间下的特性进行标记，如 `[Table]`、`[Key]` 等。
2. 数据库连接字符串需要在注册服务时提供。
3. 所有操作都包含同步和异步版本，推荐使用异步版本以提高性能。
4. 分页查询支持自定义排序字段和排序类型。
5. 事务操作支持多条 SQL 语句的原子执行。

## 依赖项

- Dapper (2.1.72)
- Dapper.Contrib (2.0.78)
- Microsoft.Data.SqlClient (6.1.4)
- Microsoft.Extensions.DependencyInjection (10.0.5)
- Acme 基础库

## 版本要求

- .NET 8 或更高版本
- .NET 9
- .NET 10

## 总结

Acme.Dapper 提供了一套简洁、高效的数据库操作接口，结合了 Dapper 的性能优势和仓储模式的易用性，适合各种规模的项目使用。通过本文档的介绍，您应该能够快速上手并在项目中使用 Acme.Dapper 进行数据库操作。
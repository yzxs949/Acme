# Acme.Dapper

轻量、面向对象的 SQL Server 数据访问层，构建在 Dapper 与 Dapper.Contrib 之上。

[![NuGet](https://img.shields.io/badge/nuget-5.7.0-informational?style=flat-square)](https://www.nuget.org/packages/Acme.Dapper)
[![.NET](https://img.shields.io/badge/.NET-8.0%20%7C%209.0%20%7C%2010.0-512BD4?style=flat-square)](https://dotnet.microsoft.com)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2012%2B-CC2927?style=flat-square)](#环境要求)
[![License](https://img.shields.io/badge/license-MIT-green?style=flat-square)](https://opensource.org/licenses/MIT)

## 目录

- [简介](#简介)
- [特性](#特性)
- [安装](#安装)
- [快速开始](#快速开始)
- [约定与限制](#约定与限制)
- [API 参考](#api-参考)
- [使用示例](#使用示例)
- [架构与实现要点](#架构与实现要点)
- [常见问题](#常见问题)
- [环境要求](#环境要求)
- [版本历史](#版本历史)
- [贡献](#贡献)
- [公众号](#公众号)
- [赞赏支持](#赞赏支持)
- [许可证](#许可证)

## 简介

Acme.Dapper 给 Dapper 补上一个"对象视角"：把散落在业务代码里的 SQL 拼接、`SqlConnection` 生命周期、分页统计、事务回滚收敛成三件事——注入 `IRepository<T>` 做 CRUD，注入 `IExtensions` 执行原生 SQL，注入 `ISplicing<T>` 组装动态条件。

它不隐藏 SQL，也不试图做全功能 ORM。SQL 仍然由你写，被省掉的只是重复的样板代码。

```csharp
var user  = await _userRepository.GetInfoAsync(id, ct);
var page  = await _userRepository.GetPageListBySqlAsync<User>("SELECT * FROM Users WHERE Status = 1", 1, 20, "Id", OrderType.DESC, ct);
var total = await _extensions.ExecuteScalarAsync<int>("SELECT COUNT(1) FROM Orders WHERE Amount > @Amount", new { Amount = 1000 }, ct);
```

## 特性

| 能力 | 说明 |
|---|---|
| 轻量 | 一个包，无配置文件，不接管事务边界，也不要求 `DbContext` |
| 仓储模式 | `IRepository<T>` 提供同步 / 异步双版本 CRUD |
| 泛型分页 | `PageList<T>` 强类型分页结果，自带 `TotalPage` / `HasPrev` / `HasNext` |
| 异步优先 | 全部异步 API 支持 `CancellationToken`，内部统一 `ConfigureAwait(false)` |
| 事务安全 | 失败自动回滚并重新抛出原始异常，不吞异常、不返回 0 |
| 动态条件 | `AddAndWhere` / `AddContainWhere` 等链式扩展，按非空字段自动拼条件 |
| 元数据缓存 | 实体表名、主键、列名反射结果按类型缓存一次，不随调用次数增长 |
| 标识符转义 | 表名、列名统一加 `[..]` 并转义内部方括号，保留字可直接使用 |
| 参数化 | 常规路径全部走 `@参数`；分页排序字段另有白名单正则校验 |

## 安装

```bash
dotnet add package Acme.Dapper
```

或直接在 `PackageReference` 中引用：

```xml
<PackageReference Include="Acme.Dapper" Version="5.7.0" />
```

包依赖 `Acme` 基础库、`Dapper`、`Dapper.Contrib`、`Microsoft.Data.SqlClient`，NuGet 会自动还原，无需额外配置。

## 快速开始

### 1. 注册服务

```csharp
using Acme.Dapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAcmeDapper(builder.Configuration.GetConnectionString("Default")!);

var app = builder.Build();
app.Run();
```

`AddAcmeDapper` 内部注册：

| 服务 | 生命周期 | 说明 |
|---|---|---|
| `IExtensions` | Singleton | 无状态，封装原生 SQL 执行，仅持有连接字符串 |
| `ISplicing<>` | Singleton | 无状态，负责 SQL 组装 |
| `IRepository<>` | Scoped | 仓储，只持有上述两个依赖，无可变状态 |
| Acme 基础服务 | — | 同时调用 `AddAcmeBuilderServer()`，注册 `HttpContextAccessor`、Session、Cookie 服务 |

说明：

- 方法返回 `void`，不支持链式调用；`services` 或 `connectionString` 为空时抛 `ArgumentNullException`。
- 全部使用 `TryAdd*`，重复调用不会覆盖已有注册。
- 不使用 Session / Cookie 的纯后端服务，可以忽略多注册的这部分服务。

### 2. 定义实体

```csharp
using Dapper.Contrib.Extensions;

[Table("Users")]
public class User
{
    [Key]                                  // 自增主键，插入时自动排除
    public int Id { get; set; }

    public string UserName { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public DateTime CreatedTime { get; set; }
}
```

映射特性的解析规则：

| 特性 | 作用 |
|---|---|
| `[Table("Users")]` | 表名，支持 `[Table("dbo.Users")]` 带 schema |
| `[Key]` | 自增主键：插入排除、更新排除，`OUTPUT INSERTED` 时作为返回列 |
| `[ExplicitKey]` | 业务主键：插入包含、更新排除 |
| `[Computed]` | 计算列：写入排除（SELECT 列清单仍包含） |
| `[Write(false)]` | 只读列：写入排除（SELECT 列清单仍包含） |

以上特性均来自 `Dapper.Contrib.Extensions` 命名空间。请勿使用 `System.ComponentModel.DataAnnotations.Schema` 下的同名特性，本库不识别它们。

### 3. 注入使用

```csharp
using Acme.Dapper.Repository;

public class UserService
{
    private readonly IRepository<User> _userRepository;

    public UserService(IRepository<User> userRepository)
        => _userRepository = userRepository;

    public Task<User> GetAsync(int id, CancellationToken ct)
        => _userRepository.GetInfoAsync(id, ct);
}
```

## 约定与限制

在动手写实体之前，先确认这几条规则，能省掉大部分"为什么没生效"。

### 实体与列名

- 实体必须是 `class` 且有无参构造函数。`AddAcmeDapper` 注册的实现类 `Repository<T>` 与 `Splicing<T>` 都带 `where T : class, new()` 约束。
- 属性名即列名，按原样加方括号，不做命名转换。**不支持 `[Column]` 重命名**，也不做下划线 / 蛇形映射。
- 表名默认取类型名；标注 `[Table]` 后取特性值，`dbo.Users` 形式的 schema 会被拆成 `[dbo].[Users]`。
- 主键识别顺序：`[Key]` → `[ExplicitKey]` → 名为 `Id`（不区分大小写）的属性；都没有则为"无主键"，`InsertSqlParam` 退回 `OUTPUT INSERTED.[Id]`。

### 主键硬编码的两个例外

`GetInfo(int Id)` / `GetInfoOrDefault(int Id)` / `Delete(int Id)` / `DeleteAsync(int Id)` 内部用 `new { Id }` 作为条件，生成的 SQL 固定是 `WHERE [Id] = @Id`，不参考元数据里的主键名。主键不叫 `Id` 时，请改用条件重载：

```csharp
await _repo.GetInfoOrDefaultAsync(new { UserId = 42 }, ct);
await _repo.DeleteAsync(new User { UserId = 42 }, ct);
```

### 分页 SQL 的写法

- 传入的 `sql` **不要带 `ORDER BY`**。分页 SQL 会被包成 `ROW_NUMBER() OVER (...)` 的子查询，子查询里的 `ORDER BY` 会直接报错；排序由 `orderField` 参数控制。
- 统计条数时 `GetCountSql` 把首个顶层 `SELECT ... FROM` 之间的列清单替换为 `COUNT(1)`，因此 `SELECT DISTINCT`、`GROUP BY`、`UNION` 等语义复杂的语句需要自行确认结果。
- 分页结果依赖 `ROW_NUMBER()` 的稳定顺序，排序字段不唯一时翻页可能出现重复或遗漏。

### 连接与事务

- 每个方法内部新建一个 `SqlConnection`，用完即释放，靠 ADO.NET 连接池复用，不持有长连接。
- `IExtensions` 的 `Insert / Update / Delete` 虽然有 `IDbTransaction` 参数，但连接是内部新建的，传入外部事务会因连接不匹配而抛异常。多语句事务请直接用 `ExecuteTransaction`。
- 事务方法失败时回滚并抛出原始异常，不返回 0。

## API 参考

### `IRepository<T>`

命名空间 `Acme.Dapper.Repository`。所有异步方法末位均可传 `CancellationToken`。

#### 新增

| 方法 | 返回 | 说明 |
|---|---|---|
| `int AddReturnId(T t)` | 新增主键 | 本库生成 `INSERT ... OUTPUT INSERTED.[Key]` |
| `Task<int> AddReturnIdAsync(T t, CancellationToken)` | 新增主键 | 异步版 |
| `int Add(T t)` | 新增主键 | 走 Dapper.Contrib `Insert` |
| `Task<int> AddAsync(T t, CancellationToken)` | 新增主键 | 异步版 |

#### 删除

| 方法 | 返回 | 说明 |
|---|---|---|
| `bool Delete(T t)` | 是否成功 | 按实体主键删除（Dapper.Contrib） |
| `bool Delete(int Id)` | 是否成功 | 单条 `DELETE`，记录不存在返回 `false`；列名固定为 `Id` |
| `Task<bool> DeleteAsync(T t, CancellationToken)` | 是否成功 | 异步版 |
| `Task<bool> DeleteAsync(int Id, CancellationToken)` | 是否成功 | 异步版 |

#### 修改

| 方法 | 返回 | 说明 |
|---|---|---|
| `bool Update(T t)` | 是否成功 | Dapper.Contrib 全字段更新 |
| `Task<bool> UpdateAsync(T t, CancellationToken)` | 是否成功 | 异步版 |
| `int UpdateReturnId(T t)` | 被更新的主键 | `UPDATE ... OUTPUT INSERTED.[Key]` |
| `Task<int> UpdateReturnIdAsync(T t, CancellationToken)` | 被更新的主键 | 异步版 |
| `Task<int> UpdateByIdAsync(object ctx, CancellationToken)` | 被更新的主键 | 只更新 `ctx` 中出现的字段，主键字段用于 `WHERE` |

`UpdateByIdAsync` 的 `ctx` 必须包含主键属性（名字取自元数据，`[Key]` / `[ExplicitKey]` / `Id`），主键之外的字段才会进入 `SET`。无匹配行时 `OUTPUT INSERTED` 没有返回行，不要用它的返回值判断是否命中，需要精确判定时先用 `IExtensions.Execute` 看影响行数：

```csharp
// 只更新 Email 与 Address，其余字段不动
await _userRepository.UpdateByIdAsync(new { Id = 1, Email = "new@example.com", Address = "杭州" }, ct);
```

#### 查询单条

| 方法 | 无数据时 |
|---|---|
| `T GetInfo(int Id)` / `Task<T> GetInfoAsync(int Id, CancellationToken)` | 抛异常 |
| `T GetInfo(object SqlWhere)` / `Task<T> GetInfoAsync(object SqlWhere, CancellationToken)` | 抛异常 |
| `T GetInfoOrDefault(int Id)` / `Task<T> GetInfoOrDefaultAsync(int Id, CancellationToken)` | 返回 `default` |
| `T GetInfoOrDefault(object SqlWhere)` / `Task<T> GetInfoOrDefaultAsync(object SqlWhere, CancellationToken)` | 返回 `default` |

条件对象的每个可读属性都会变成 `[属性名] = @属性名` 并用 `AND` 连接。

#### 列表与分页

| 方法 | 说明 |
|---|---|
| `List<T> GetList(object SqlWhere)` | 条件列表，`SqlWhere` 为 `null` 时查全表 |
| `Task<List<T>> GetListAsync(object SqlWhere, CancellationToken)` | 异步版 |
| `Task<int> GetCountAsync(object SqlWhere, CancellationToken)` | 条数统计（仅异步） |
| `PageList GetPageListBySql(...)` | 弱类型分页，元素为 Dapper 的 `DapperRow` |
| `PageList<TEntity> GetPageListBySql<TEntity>(...)` | 强类型分页 |
| `Task<PageList> GetPageListBySqlAsync(...)` | 弱类型分页（异步） |
| `Task<PageList<TEntity>> GetPageListBySqlAsync<TEntity>(...)` | 强类型分页（异步） |

分页签名：

```csharp
PageList<TEntity> GetPageListBySql<TEntity>(
    string sql,
    int pageIndex,          // 从 1 开始，小于 1 自动纠正为 1
    int pageSize,           // 小于 1 抛 ArgumentOutOfRangeException
    string orderField = "", // 空白时用 ORDER BY (SELECT 0)，即不排序
    OrderType orderType = OrderType.ASC);
```

`orderField` 会经过白名单校验：只放行"字母 / 数字 / 下划线组成的列名，可带一个点号前缀"，多列之间用逗号分隔（末位逗号会被去掉）。即只允许单列、`表别名.列名`、逗号分隔的多列。多列共用同一个 `orderType`。含分号、引号、注释符等内容时抛 `ArgumentException`。

分页结果 `PageList<T>`：

```csharp
public class PageList<T>
{
    public int Total { get; set; }        // 总条数
    public int PageIndex { get; set; }    // 当前页
    public int PageSize { get; set; }
    public List<T> Items { get; set; }    // 初始化为空集合，非 null
    public int TotalPage { get; }         // 总页数，PageSize <= 0 时为 0
    public bool HasPrev { get; }          // PageIndex > 1
    public bool HasNext { get; }          // PageIndex < TotalPage
}
```

弱类型版 `PageList` 结构相同，`Items` 为 `List<object>`：

```csharp
PageList page = await _repo.GetPageListBySqlAsync(sql, 1, 20, "Id", OrderType.DESC, ct);
dynamic first = page.Items[0];
Console.WriteLine(first.UserName);
```

### `IExtensions`

命名空间 `Acme.Dapper.Extension`，原生 SQL 执行入口，适合复杂查询、批量脚本、存储过程。只读属性 `ConnectionString` 暴露构造时传入的连接串。

#### 查询

```csharp
List<T> Query<T>(string sql, object param);
Task<List<T>> QueryAsync<T>(string sql, object param, CancellationToken ct = default);

T QueryFirst<T>(string sql, object param);              // 无数据抛异常
Task<T> QueryFirstAsync<T>(string sql, object param, CancellationToken ct = default);

T QueryFirstOrDefault<T>(string sql, object param);     // 无数据返回 default
Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param, CancellationToken ct = default);

T QuerySingle<T>(string sql, object param);             // 多于一条抛异常
Task<T> QuerySingleAsync<T>(string sql, object param, CancellationToken ct = default);

T QuerySingleOrDefault<T>(string sql, object param);    // 无数据返回 default，多条抛异常
Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param, CancellationToken ct = default);
```

#### 执行

```csharp
int Execute(string sql, object param);
Task<int> ExecuteAsync(string sql, object param, CancellationToken ct = default);

object ExecuteScalar(string sql, object param);
T ExecuteScalarForT<T>(string sql, object param);
Task<T> ExecuteScalarAsync<T>(string sql, object param, CancellationToken ct = default);
Task<T> ExecuteScalarForTAsync<T>(string sql, object param, CancellationToken ct = default); // 等价于上行
```

#### 实体操作（Dapper.Contrib）

```csharp
int Insert<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null) where T : class, new();
Task<int> InsertAsync<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null, CancellationToken ct = default);

bool Update<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null) where T : class, new();
Task<bool> UpdateAsync<T>(...);

bool Delete<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null) where T : class, new();
Task<bool> DeleteAsync<T>(...);
```

`Insert` 的返回值是自增主键，内部从 `long` 做 `checked` 转换为 `int`，超出范围会抛 `OverflowException`，不会静默溢出。

#### 存储过程

```csharp
List<T> ExecutePro<T>(string proc, object param);
Task<List<T>> ExecuteProAsync<T>(string proc, object param, CancellationToken ct = default);
```

内部以 `CommandType.StoredProcedure` 执行。

#### 事务

```csharp
int ExecuteTransaction(string[] sqlarr);
Task<int> ExecuteTransactionAsync(string[] sqlarr, CancellationToken ct = default);

int ExecuteTransaction(Dictionary<string, object> dic);
Task<int> ExecuteTransactionAsync(Dictionary<string, object> dic, CancellationToken ct = default);
```

返回每条语句受影响行数之和；任一条失败即回滚并抛出原始异常。`Dictionary` 版本按插入顺序逐条执行。

```csharp
var sqls = new Dictionary<string, object>
{
    ["INSERT INTO Users (UserName, Email) VALUES (@UserName, @Email)"] = new { UserName = "王五", Email = "w@example.com" },
    ["UPDATE Accounts SET Balance = Balance - @Amount WHERE Id = @Id"]  = new { Amount = 100, Id = 1 },
};

int affected = await _extensions.ExecuteTransactionAsync(sqls, ct);
```

#### Reader

```csharp
IDataReader ExecuteReader(string sql, object param);
Task<IDataReader> ExecuteReaderAsync(string sql, object param, CancellationToken ct = default);
```

- 参数由 `param` 的可读属性逐个建 `SqlParameter`（属性名即参数名，前缀 `@`），不走 Dapper 的 `DynamicParameters`。
- 返回的 Reader 通过 `CommandBehavior.CloseConnection` 绑定连接——关闭 Reader 即关闭连接，务必 `using`。

### `ISplicing<T>`

命名空间 `Acme.Dapper.SqlSplice`，需要自己拼 SQL 时使用，注入即可。`T` 同样要求 `class, new()`。

| 方法 | 说明 |
|---|---|
| `string InsertSql(T t, bool IsItKey = true)` | 值内联的 INSERT，仅调试 / 脚本用 |
| `string DeleteSql(object CondiEntity)` | 值内联的 DELETE |
| `string UpdateSql(T t, object CondiEntity, bool IsItKey = true)` | 值内联的 UPDATE |
| `string SelectSql(object CondiEntity)` | 值内联的 SELECT |
| `string SelectSql()` | 全表 SELECT |
| `string InsertSqlParam(T t, bool IsItKey = true)` | 参数化 INSERT，带 `OUTPUT INSERTED` |
| `string DeleteSqlParam(object CondiEntity)` | 参数化 DELETE |
| `string UpdateSqlParam(T t, object CondiEntity, bool IsItKey = true)` | 参数化 UPDATE + 条件 |
| `string UpdateSqlParam(T t)` | 参数化 UPDATE，按主键 |
| `string UpdateSqlParam(object UpdateContext)` | 只更新 `UpdateContext` 中出现的字段，按主键 |
| `string SelectSqlParam(object CondiEntity)` | 参数化 SELECT |
| `string SelectSqlParam()` | 参数化全表 SELECT |
| `string SelectCountParam(object CondiEntity)` | 参数化 COUNT |
| `string ExistenceSql(string sql)` | 生成 `IF EXISTS(sql) PRINT 1 ELSE PRINT 2` |
| `string SelectNoResultSql()` | 生成 `SELECT #Result FROM [表]`，供 `Select` / `Total` 替换 |
| `string GetPageSql(string sql, int pageIndex, int pageSize, string orderField = "", OrderType orderType = OrderType.ASC)` | `ROW_NUMBER()` 分页 |
| `string GetCountSql(string sql)` | 把查询改写为 `COUNT(1)` |

`IsItKey` 的含义是"是否排除主键"：`true`（默认）时 INSERT 排除自增主键、UPDATE 排除全部主键；传 `false` 则把主键也纳入写入列。

`Splicing<T>` 实现类另有两个公开方法（不在接口上）：

```csharp
string GetUpdateContent(object UpdateContent); // 生成 SET 子句，自动排除主键
string GetSqlCount(object CondiEntity);        // SELECT COUNT(1) FROM [表] WHERE ...
```

### `DBSqlParameter` 条件扩展

`Acme.Dapper.SqlSplice.DBSqlParameter` 上的一组 `string` 扩展方法，用于后台管理类的动态筛选。

```csharp
string sql = "SELECT * FROM Users WHERE 1 = 1";

sql = sql.AddAndWhere(new { Address = "北京" });                    // AND [Address] = N'北京'
sql = sql.AddContainWhere(new { UserName = "张" });                 // AND [UserName] LIKE N'%张%'
sql = sql.AddLeftContainWhere(new { Email = "qq" });                // AND [Email] LIKE N'qq%'
sql = sql.AddRightContainWhere(new { Phone = "138" });              // AND [Phone] LIKE N'%138'

// 需要 OR 时指定 andOrNot
sql = sql.AddAndWhere(new { Status = 1 }, andOrNot: AndOrNot.OR);   // OR [Status] = 1
```

| 方法 | 生成的运算符 | 取值规则 |
|---|---|---|
| `AddAndWhere` | `= / <> / > / <`，由 `symbol` 参数决定，默认 `=` | 遍历所有可读属性，跳过值为 `null` 的属性 |
| `AddContainWhere` | `LIKE '%值%'` | 只取条件对象的第一个可读属性 |
| `AddLeftContainWhere` | `LIKE '值%'` | 只取第一个属性 |
| `AddRightContainWhere` | `LIKE '%值'` | 只取第一个属性 |

四个方法都有 `isJudgment` 开关（`false` 时原样返回）和 `andOrNot` 拼接符（默认 `AND`）。值以 SQL 字面量内联，字符串做单引号转义，但不参与参数化——对外暴露的接口请优先使用 `@参数` 查询。

其余辅助方法：

```csharp
string sql = "SELECT #Result FROM Users AS T1 WHERE ...";

// 字符串值原样作为列表达式，并以属性名做别名；非字符串值格式化为 SQL 字面量
sql = sql.Select(new { Id = "T1.Id", UserName = "T1.UserName" });  // 替换 #Result 为列清单
sql = sql.Total();                                                // 替换 #Result 为 COUNT(1)

"SELECT #Result FROM Users".Join<Order>(
    JoinType.LEFT, 1);                      // AS T1 LEFT JOIN [Order] AS T2
"...#Result FROM Users AS T1 LEFT JOIN Orders AS T2".ON(
    new Dictionary<string, string> { ["T1.Id"] = "T2.UserId" });

string sql2 = baseSql.SplitJoin(hasCondition, " AND Status = 1");
```

| 方法 | 说明 |
|---|---|
| `Select(this string sql, object result)` | 替换 `#Result`，缺占位符抛 `ArgumentException` |
| `Total(this string sql)` | 替换 `#Result` 为 `COUNT(1)` |
| `Join<T>(this string sql, JoinType join = INNER, int index = 1)` | 追加 `AS T{index} {join} JOIN [表] AS T{index+1}`，缺 `#Result` 抛异常 |
| `ON(this string sql, Dictionary<string, string> keyValues)` | 追加 `ON a = b AND ...`，字典为空抛异常 |
| `SplitJoin(this string sql1, bool isJudgment, string sql2)` | `isJudgment` 为 `true` 时直接字符串拼接，不加分隔符 |

`ON` 的条件片段按传入文本原样拼接，不做标识符转义。

## 使用示例

### 新增并取回主键

```csharp
var user = new User
{
    UserName = "张三",
    Email = "zhangsan@example.com",
    Address = "北京",
    CreatedTime = DateTime.Now,
};

int id = await _userRepository.AddReturnIdAsync(user, ct);
```

### 条件查询

```csharp
// 按主键
var user = await _userRepository.GetInfoAsync(1, ct);

// 按任意条件（属性名即列名）
var user = await _userRepository.GetInfoOrDefaultAsync(new { Email = "a@b.com" }, ct);

// 列表
var users = await _userRepository.GetListAsync(new { Address = "北京" }, ct);

// 条数
int count = await _userRepository.GetCountAsync(new { Address = "北京" }, ct);
```

### 分页

```csharp
string sql = "SELECT * FROM Users WHERE Status = 1";   // 不要带 ORDER BY
var page = await _userRepository.GetPageListBySqlAsync<User>(sql, 1, 20, "Id", OrderType.DESC, ct);

Console.WriteLine($"{page.Total} 条 / {page.TotalPage} 页，当前 {page.PageIndex}");
foreach (var item in page.Items) { /* ... */ }
```

### 局部更新

```csharp
await _userRepository.UpdateByIdAsync(new { Id = 1, Address = "广州" }, ct);
```

### 存储过程

```csharp
var users = await _extensions.ExecuteProAsync<User>("GetUsersByCity", new { City = "北京" }, ct);
```

### 混合使用原生 SQL 与仓储

```csharp
// 复杂联查用原生 SQL
string sql = @"SELECT u.Id, u.UserName, COUNT(o.Id) AS OrderCount
               FROM Users u
               LEFT JOIN Orders o ON o.UserId = u.Id
               GROUP BY u.Id, u.UserName";
var rows = await _extensions.QueryAsync<dynamic>(sql, null, ct);

// 需要按业务规则回滚的多步操作走事务
await _extensions.ExecuteTransactionAsync(new[]
{
    "UPDATE Accounts SET Balance = Balance - 100 WHERE Id = 1",
    "UPDATE Accounts SET Balance = Balance + 100 WHERE Id = 2",
}, ct);
```

## 架构与实现要点

```text
┌──────────────────┐
│   业务代码        │
└────────┬─────────┘
         │
   ┌─────▼──────┐        ┌──────────────┐
   │IRepository │───────▶│  ISplicing<T>│  SQL 组装 + 元数据缓存
   │    <T>     │        └──────────────┘
   └─────┬──────┘
         │
   ┌─────▼──────┐
   │ IExtensions│  Dapper 调用 + 连接生命周期
   └─────┬──────┘
         │
   ┌─────▼──────┐
   │SqlConnection│
   └────────────┘
```

- `Splicing<T>` 的 `EntityMetadata` 是 `static` 字段，每个实体类型只反射一次；表名另有 `ConcurrentDictionary` 缓存。
- `EntityMetadata` 把属性分成三组：`WritableProperties`（排除 `[Computed]`、`[Write(false)]`）、`InsertableProperties`（再排除自增主键）、`UpdatableProperties`（排除全部主键）。
- `Extensions` 每次调用新建连接，依赖连接池复用，不持有长连接；`IExtensions` 与 `ISplicing<>` 无状态，注册为 Singleton。
- `Repository<T>` 只做编排，无可变状态；`GetPageListBySql` 是"一次统计 + 一次分页"两次数据库往返。
- 标识符统一经 `QuoteIdentifier` 加方括号，`]` 会被转义为 `]]`；`GetCountSql` 与 `GetPageSql` 的关键字扫描都能跳过括号、字符串与 `--` 注释。

## 常见问题

### 必须标注 `[Key]` 吗？

不必。未标注时会自动识别名为 `Id`（不区分大小写）的属性作为主键。若主键叫 `UserId` 或由业务赋值，请用 `[Key]` / `[ExplicitKey]` 明确标注；主键既不叫 `Id` 又没有特性时，`InsertSqlParam` 会退回 `OUTPUT INSERTED.[Id]`，此时插入会失败。

### 实体标了 `[Table("Users")]` 却报"对象名无效"？

确认引用的是 `Dapper.Contrib.Extensions.TableAttribute`，而不是 `System.ComponentModel.DataAnnotations.Schema.TableAttribute`。同理，`[Key]`、`[ExplicitKey]`、`[Computed]`、`[Write(false)]` 也都要用 Dapper.Contrib 版本。

### 属性名能映射到不同列名吗？

不能。本库只读属性名，`[Column]` 不生效。列名与属性名必须一致；如需别名，请在原生 SQL 中用 `AS` 对齐属性名。

### `GetInfo` 和 `GetInfoOrDefault` 有什么区别？

前者在结果为空时抛异常（适合"业务上必须存在"的场景，能尽早暴露数据问题），后者返回 `default`。

### `AddReturnId` 与 `Add` 有什么区别？

两者都返回新增主键。`AddReturnId` 用本库生成的 `INSERT ... OUTPUT INSERTED` 语句；`Add` 走 Dapper.Contrib 的 `Insert`，对 `[Computed]` 等特性的处理更贴近 Contrib 的既有语义。功能等价，任选其一。

### 主键叫 `UserId`，为什么 `GetInfoAsync(1)` 查不到？

因为 `GetInfo(int Id)` / `Delete(int Id)` 的条件固定是 `[Id] = @Id`，不参考元数据主键名。改用条件重载：`GetInfoAsync(new { UserId = 1 }, ct)`。

### 分页时报"ORDER BY 子句在视图、内联函数、派生表……中无效"？

分页 SQL 会被包进子查询，`sql` 里不能有 `ORDER BY`。去掉它，把排序字段交给 `orderField` 参数（并确保该字段在白名单内，如 `Id`、`u.Id`、`Id,Name`）。

### 支持 MySQL / PostgreSQL / SQLite 吗？

不支持，当前仅支持 SQL Server。连接层硬编码 `Microsoft.Data.SqlClient`，SQL 组装全部是 T-SQL 方言（`OUTPUT INSERTED`、`ROW_NUMBER()`、`[..]` 标识符转义等）。

### 分页数据量很大怎么优化？

本库用 `ROW_NUMBER() OVER (...)` 实现分页，深分页（如第 10000 页）性能会明显下降。大数据量场景建议直接用 `IExtensions.QueryAsync` 写 `OFFSET ... FETCH NEXT` 或键集分页（`WHERE Id > @LastId ORDER BY Id`），条数也可换成带缓存的估算值。

### 事务失败了怎么知道？

`ExecuteTransaction` / `ExecuteTransactionAsync` 在任一语句失败时回滚并抛出原始异常，不再返回 `0`。请用 `try/catch` 处理；回滚本身使用 `CancellationToken.None`，避免取消令牌已触发时回滚被跳过。

### `Insert(model, transaction)` 传了自己的事务为什么报错？

`IExtensions` 每次调用都新建连接，外部事务属于另一条连接，无法挂载。需要多语句原子性时用 `ExecuteTransaction`；需要跨仓储的大事务，请自行用 `SqlConnection` + `IExtensions` 写原生 SQL。

### 能在控制台 / Worker 项目里用吗？

可以。`AddAcmeDapper` 附带注册的 `AddHttpContextAccessor`、`AddSession` 等基础服务在非 Web 场景不会影响数据库功能。

## 环境要求

| 项目 | 要求 |
|---|---|
| .NET | `net8.0` / `net9.0` / `net10.0` |
| 数据库 | SQL Server 2012 及以上（依赖 `ROW_NUMBER`、`OUTPUT`） |
| 依赖 | Dapper 2.1.89、Dapper.Contrib 2.0.78、Microsoft.Data.SqlClient 7.1.1、Microsoft.Extensions.DependencyInjection 10.0.14 |
| 基础库 | Acme（随 NuGet 自动还原） |

## 版本历史

### 5.7.0

修复：

- 补上缺失的 `ISplicing<>` 注册，此前解析 `IRepository<T>` 会直接抛 `InvalidOperationException`
- `InsertSqlParam` 参数占位符索引错误，导致所有列绑定到同一参数
- `GetUpdateContent` 覆盖而非追加，多字段更新只剩最后一列
- `UpdateSqlParam(T)` 取值索引错误；`WHERE Id='{Id}'` 改为参数化
- `UpdateReturnId(Async)` 误调用 `InsertSqlParam`，同步"修改"实际执行了 INSERT
- `AddReturnId(Async)` 取的是受影响行数（恒为 1）而非主键
- `ExecuteReader(Async)` 返回前释放连接，导致 reader 不可用
- `DBSqlParameter.Select` 取值对象错误；`ON` 拼接缺少空格
- `GetInfoOrDefault(Async)` 用全表载入再取首条，改为 `QueryFirstOrDefault`
- `Delete(int Id)` 由"先查后删"改为单条 `DELETE`
- `GetCountSql` 依赖字符串截取定位 `SELECT` / `FROM`，改为括号、引号、注释感知的顶层关键字扫描
- 事务失败静默返回 `0`，改为回滚后抛出

变更：

- 全部异步 API 增加 `CancellationToken` 可选参数（源码兼容，二进制不兼容）
- 表名、列名统一 `[..]` 转义，支持 `[Table("dbo.Users")]`
- 主键识别改为 `[Key]` / `[ExplicitKey]` / 约定 `Id`，不再假设"第一个属性"
- 分页 `orderField` 增加白名单校验
- `Extensions.ConnectionString` 改为只读
- `PageList.Items` 初始化为空集合，新增 `TotalPage` / `HasPrev` / `HasNext`
- `IExtensions` / `IRepository<>` 生命周期统一（`IExtensions` 与 `ISplicing<>` 为 Singleton）

### 5.6.x 及更早

初始仓储模式实现、分页、事务、存储过程支持。

## 贡献

欢迎提交 Issue 与 Pull Request。

- 仓库：<https://gitee.com/yzxs949/acme>
- 提交前请确保 `dotnet build` 零警告零错误
- 涉及 SQL 拼接的改动，请补充覆盖该类场景的用例

## 公众号

扫码关注公众号「.NET修仙日记」，获取更新动态与实战笔记。

![作者公众号](https://raw.githubusercontent.com/yzxs949/FilePath/refs/heads/main/Image/%E4%BA%8C%E7%BB%B4%E7%A0%81.jpg)

## 赞赏支持

如果这个项目对您有帮助，欢迎请作者喝杯咖啡，您的支持是持续维护的动力。

| 支付宝 | 微信 |
| :---: | :---: |
| ![支付宝](https://raw.githubusercontent.com/yzxs949/FilePath/refs/heads/main/Image/zfb.jpg) | ![微信](https://raw.githubusercontent.com/yzxs949/FilePath/refs/heads/main/Image/wx.jpg) |

## 许可证

本项目基于 [MIT License](https://opensource.org/licenses/MIT) 发布。

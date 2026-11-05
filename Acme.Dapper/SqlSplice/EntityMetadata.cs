using Dapper.Contrib.Extensions;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace Acme.Dapper.SqlSplice;

/// <summary>
/// 实体元数据：表名、主键、列名与可写列。
/// 每个实体类型只反射一次，避免每次拼接 SQL 都调用 GetProperties。
/// </summary>
internal sealed class EntityMetadata
{
    private static readonly ConcurrentDictionary<Type, string> TableNameCache = new();

    /// <summary>
    /// 用于查询的全部属性（按声明顺序，已排除只写属性与索引器）。
    /// </summary>
    public PropertyInfo[] Properties { get; }

    /// <summary>
    /// 表名（已按 SqlServer 语法加方括号转义）。
    /// </summary>
    public string TableName { get; }

    /// <summary>
    /// 主键属性名（未转义）；无主键时为 null。
    /// </summary>
    public string KeyName { get; }

    /// <summary>
    /// 主键列名（已转义）；无主键时为 null。
    /// </summary>
    public string KeyColumn { get; }

    /// <summary>
    /// SELECT 使用的列清单，如 <c>[Id], [Name]</c>。
    /// </summary>
    public string SelectColumns { get; }

    /// <summary>
    /// 所有可写属性（排除 <c>[Computed]</c> 与 <c>[Write(false)]</c>）。
    /// </summary>
    public PropertyInfo[] WritableProperties { get; }

    /// <summary>
    /// 新增时可写的属性（排除自增主键）。
    /// </summary>
    public PropertyInfo[] InsertableProperties { get; }

    /// <summary>
    /// 更新时可写的属性（排除全部主键）。
    /// </summary>
    public PropertyInfo[] UpdatableProperties { get; }

    /// <summary>
    /// 解析实体元数据。
    /// </summary>
    /// <param name="type">实体类型</param>
    public EntityMetadata(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        Properties = type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
            .ToArray();

        TableName = BuildTableName(type);
        KeyName = ResolveKeyName(Properties);
        KeyColumn = KeyName == null ? null : QuoteIdentifier(KeyName);
        SelectColumns = string.Join(", ", Properties.Select(p => QuoteIdentifier(p.Name)));

        WritableProperties = Properties
            .Where(p => p.CanWrite)
            .Where(p => p.GetCustomAttribute<ComputedAttribute>() == null)
            .Where(p => p.GetCustomAttribute<WriteAttribute>() is not { Write: false })
            .ToArray();

        InsertableProperties = WritableProperties.Where(p => !IsAutoKey(p)).ToArray();
        UpdatableProperties = WritableProperties.Where(p => !IsAnyKey(p)).ToArray();
    }

    /// <summary>
    /// 取实体对应的表名（已转义），结果按类型缓存。
    /// </summary>
    /// <param name="type">实体类型</param>
    /// <returns>转义后的表名</returns>
    public static string TableNameOf(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return TableNameCache.GetOrAdd(type, static t => new EntityMetadata(t).TableName);
    }

    /// <summary>
    /// 把标识符按 SqlServer 语法加上方括号并转义内部方括号。
    /// </summary>
    /// <param name="identifier">标识符</param>
    /// <returns>转义后的标识符</returns>
    public static string QuoteIdentifier(string identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier))
            throw new ArgumentException("SQL 标识符不能为空", nameof(identifier));

        return "[" + identifier.Replace("]", "]]") + "]";
    }

    /// <summary>
    /// 判断是否自增主键：显式标注 <c>[Key]</c>，或约定名为 Id 且未标注任何主键特性。
    /// </summary>
    private static bool IsAutoKey(PropertyInfo property)
    {
        if (property.GetCustomAttribute<KeyAttribute>() != null)
            return true;

        return property.GetCustomAttribute<ExplicitKeyAttribute>() == null
            && string.Equals(property.Name, "Id", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 判断是否主键（含由业务赋值的主键）。
    /// </summary>
    private static bool IsAnyKey(PropertyInfo property)
        => IsAutoKey(property) || property.GetCustomAttribute<ExplicitKeyAttribute>() != null;

    /// <summary>
    /// 解析主键属性名，优先取特性标注，其次约定名为 Id 的属性。
    /// </summary>
    private static string ResolveKeyName(PropertyInfo[] properties)
    {
        PropertyInfo key = properties.FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null)
                           ?? properties.FirstOrDefault(p => p.GetCustomAttribute<ExplicitKeyAttribute>() != null)
                           ?? properties.FirstOrDefault(p => string.Equals(p.Name, "Id", StringComparison.OrdinalIgnoreCase));

        return key?.Name;
    }

    /// <summary>
    /// 解析表名：优先取 <c>[Table]</c> 特性，否则使用类型名。
    /// </summary>
    private static string BuildTableName(Type type)
    {
        TableAttribute attribute = type.GetCustomAttribute<TableAttribute>();
        string name = attribute?.Name;
        string schema = null;

        if (string.IsNullOrWhiteSpace(name))
            name = type.Name;
        else
            name = name.Trim();

        // 支持 [Table("dbo.Users")] 这类带 schema 的写法
        int dot = name.LastIndexOf('.');
        if (dot > 0)
        {
            schema = name[..dot];
            name = name[(dot + 1)..];
        }

        return string.IsNullOrWhiteSpace(schema)
            ? QuoteIdentifier(name)
            : QuoteIdentifier(schema.Trim()) + "." + QuoteIdentifier(name);
    }
}

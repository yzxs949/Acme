using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Acme.Dapper.SqlSplice
{
    /// <summary>
    /// 且或非枚举
    /// </summary>
    public enum AndOrNot
    {
        /// <summary>
        /// 且
        /// </summary>
        AND = 0,

        /// <summary>
        /// 或
        /// </summary>
        OR = 1,
    }

    /// <summary>
    /// 连接查询类型
    /// </summary>
    public enum JoinType
    {
        /// <summary>
        /// 内联接
        /// </summary>
        INNER = 0,

        /// <summary>
        /// 左连接
        /// </summary>
        LEFT = 1,

        /// <summary>
        /// 右连接
        /// </summary>
        RIGHT = 2
    }

    /// <summary>
    /// sql参数通用类
    /// </summary>
    public static class DBSqlParameter
    {
        /// <summary>
        /// 将 .NET 值格式化为 SQL 字面量（仅用于本就不支持参数化的拼接场景）
        /// </summary>
        /// <param name="value">原始值</param>
        /// <returns>SQL 字面量</returns>
        internal static string SqlLiteral(object value) => value switch
        {
            null or DBNull => "NULL",
            bool b => b ? "1" : "0",
            string s => "N'" + s.Replace("'", "''") + "'",
            char c => "N'" + c.ToString().Replace("'", "''") + "'",
            DateTime dt => "'" + dt.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + "'",
            DateTimeOffset dto => "'" + dto.ToString("yyyy-MM-dd HH:mm:ss.fff zzz", CultureInfo.InvariantCulture) + "'",
            Guid g => "'" + g.ToString() + "'",
            byte[] bytes => "0x" + Convert.ToHexString(bytes),
            Enum e => Convert.ToInt64(e, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture),
            IFormattable f => f.ToString(null, CultureInfo.InvariantCulture),
            _ => "N'" + value.ToString().Replace("'", "''") + "'",
        };

        /// <summary>
        /// 构建SqlServer数据库的sql参数
        /// </summary>
        /// <param name="ParameterEntity">实体(如：new{Id=1,Name='夜殇'})</param>
        /// <returns>SqlParameter类型的sql参数数组</returns>
        public static SqlParameter[] SqlParame(this object ParameterEntity)
        {
            if (ParameterEntity == null)
                return Array.Empty<SqlParameter>();

            List<SqlParameter> parameters = new();
            foreach (PropertyInfo p in GetReadableProperties(ParameterEntity))
            {
                parameters.Add(new SqlParameter("@" + p.Name, p.GetValue(ParameterEntity) ?? DBNull.Value));
            }

            return parameters.ToArray();
        }

        /// <summary>
        /// 构建条件参数
        /// </summary>
        /// <param name="CondiEntity">条件实体 如：new {Id=1}</param>
        /// <returns></returns>
        public static string GetSqlCondi(object CondiEntity)
        {
            ArgumentNullException.ThrowIfNull(CondiEntity);

            PropertyInfo[] properties = GetReadableProperties(CondiEntity);
            if (properties.Length == 0)
                throw new ArgumentException("条件实体不包含任何可读属性", nameof(CondiEntity));

            return string.Join(
                " AND ",
                properties.Select(p => $"{EntityMetadata.QuoteIdentifier(p.Name)} = @{p.Name}"));
        }

        /// <summary>
        /// 把属性值当作列表达式：字符串原样输出（如 <c>T1.Id</c>），其余按 SQL 字面量处理
        /// </summary>
        /// <param name="value">属性值</param>
        private static string AsColumnExpression(object value)
            => value is string text ? text : SqlLiteral(value);

        /// <summary>
        /// 取对象的可读实例属性
        /// </summary>
        /// <param name="entity">对象</param>
        private static PropertyInfo[] GetReadableProperties(object entity)
            => entity.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
                .ToArray();

        /// <summary>
        /// 拼接sql语句条件
        /// </summary>
        /// <param name="sql">原sql</param>
        /// <param name="condiEntity">字段名和值</param>
        /// <param name="isJudgment">是否验证通过（通过拼接，不通过返回原sql）</param>
        /// <param name="symbol">符号（默认=）</param>
        /// <param name="andOrNot">拼接关键字（AND或OR）</param>
        /// <returns>新的sql字符串</returns>
        public static string AddAndWhere(
            this string sql,
            object condiEntity,
            bool isJudgment = true,
            string symbol = "=",
            AndOrNot andOrNot = AndOrNot.AND)
        {
            if (!isJudgment || condiEntity == null)
                return sql;

            string logic = andOrNot == AndOrNot.OR ? "OR" : "AND";
            foreach (PropertyInfo item in GetReadableProperties(condiEntity))
            {
                object value = item.GetValue(condiEntity);
                if (value == null)
                    continue;

                sql += $" {logic} {EntityMetadata.QuoteIdentifier(item.Name)} {symbol} {SqlLiteral(value)}";
            }

            return sql;
        }

        /// <summary>
        /// 添加模糊查询（两侧通配）
        /// </summary>
        /// <param name="sql">原sql</param>
        /// <param name="condiEntity">条件</param>
        /// <param name="isJudgment">是否验证通过</param>
        /// <param name="andOrNot">是AND 还是or</param>
        /// <returns></returns>
        public static string AddContainWhere(
            this string sql,
            object condiEntity,
            bool isJudgment = true,
            AndOrNot andOrNot = AndOrNot.AND)
            => AddLikeWhere(sql, condiEntity, isJudgment, andOrNot, "%{0}%");

        /// <summary>
        /// 拼接后缀通配的模糊查询
        /// </summary>
        /// <param name="sql">原sql</param>
        /// <param name="condiEntity">字段名和值</param>
        /// <param name="isJudgment">是否验证通过（通过拼接，不通过返回原sql）</param>
        /// <param name="andOrNot">拼接关键字（AND或OR）</param>
        /// <returns>新的sql字符串</returns>
        public static string AddLeftContainWhere(
            this string sql,
            object condiEntity,
            bool isJudgment = true,
            AndOrNot andOrNot = AndOrNot.AND)
            => AddLikeWhere(sql, condiEntity, isJudgment, andOrNot, "{0}%");

        /// <summary>
        /// 拼接前缀通配的模糊查询
        /// </summary>
        /// <param name="sql">原sql</param>
        /// <param name="condiEntity">字段名和值</param>
        /// <param name="isJudgment">是否验证通过（通过拼接，不通过返回原sql）</param>
        /// <param name="andOrNot">拼接关键字（AND或OR）</param>
        /// <returns>新的sql字符串</returns>
        public static string AddRightContainWhere(
            this string sql,
            object condiEntity,
            bool isJudgment = true,
            AndOrNot andOrNot = AndOrNot.AND)
            => AddLikeWhere(sql, condiEntity, isJudgment, andOrNot, "%{0}");

        /// <summary>
        /// 拼接 LIKE 条件（取条件实体的第一个属性）
        /// </summary>
        /// <param name="sql">原sql</param>
        /// <param name="condiEntity">条件</param>
        /// <param name="isJudgment">是否验证通过</param>
        /// <param name="andOrNot">拼接关键字</param>
        /// <param name="pattern">通配模板，如 <c>%{0}%</c></param>
        private static string AddLikeWhere(
            string sql,
            object condiEntity,
            bool isJudgment,
            AndOrNot andOrNot,
            string pattern)
        {
            if (!isJudgment || condiEntity == null)
                return sql;

            PropertyInfo[] properties = GetReadableProperties(condiEntity);
            if (properties.Length == 0)
                return sql;

            object value = properties[0].GetValue(condiEntity);
            if (value == null)
                return sql;

            string logic = andOrNot == AndOrNot.OR ? "OR" : "AND";
            string wildcard = string.Format(pattern, value.ToString().Replace("'", "''"));
            return sql + $" {logic} {EntityMetadata.QuoteIdentifier(properties[0].Name)} LIKE N'{wildcard}'";
        }

        /// <summary>
        /// 获取的结果集
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string Select(this string sql, object result)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(sql);
            ArgumentNullException.ThrowIfNull(result);

            if (!sql.Contains("#Result", StringComparison.Ordinal))
                throw new ArgumentException("sql 中缺少 #Result 占位符", nameof(sql));

            string strResult = string.Join(
                ", ",
                GetReadableProperties(result).Select(
                    p => $"{AsColumnExpression(p.GetValue(result))} AS {EntityMetadata.QuoteIdentifier(p.Name)}"));

            return sql.Replace("#Result", strResult);
        }

        /// <summary>
        /// 获取数据条数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string Total(this string sql)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(sql);

            if (!sql.Contains("#Result", StringComparison.Ordinal))
                throw new ArgumentException("sql 中缺少 #Result 占位符", nameof(sql));

            return sql.Replace("#Result", "COUNT(1)");
        }

        /// <summary>
        /// 链接查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="join"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string Join<T>(
            this string sql,
            JoinType join = JoinType.INNER,
            int index = 1) where T : class, new()
        {
            if (sql == null || !sql.Contains("#Result", StringComparison.Ordinal))
                throw new ArgumentException("sql 中缺少 #Result 占位符", nameof(sql));

            return sql + $" AS T{index} {join} JOIN {EntityMetadata.TableNameOf(typeof(T))} AS T{index + 1}";
        }

        /// <summary>
        /// ON条件
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string ON(this string sql, Dictionary<string, string> keyValues)
        {
            if (keyValues == null || keyValues.Count == 0)
                throw new ArgumentException("ON 条件不能为空", nameof(keyValues));

            return sql + " ON " + string.Join(" AND ", keyValues.Select(kv => $"{kv.Key} = {kv.Value}"));
        }

        /// <summary>
        /// sql语句拼接
        /// </summary>
        /// <param name="sql1">原来的sql</param>
        /// <param name="isJudgment">是否通过</param>
        /// <param name="sql2">要拼接的sql</param>
        /// <returns></returns>
        public static string SplitJoin(this string sql1, bool isJudgment, string sql2)
        {
            if (isJudgment)
                sql1 += sql2;
            return sql1;
        }
    }
}


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Acme.Dapper.Enums;

namespace Acme.Dapper.SqlSplice
{
    /// <summary>
    /// 动态生成 SQL 语句
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public class Splicing<T> : ISplicing<T> where T : class, new()
    {
        /// <summary>
        /// 实体元数据，每个实体类型只反射一次
        /// </summary>
        private static readonly EntityMetadata Meta = new(typeof(T));

        /// <summary>
        /// 排序字段白名单校验（只允许列名、逗号与点号，杜绝注入）
        /// </summary>
        private static readonly Regex OrderFieldRegex = new(
            @"^[A-Za-z0-9_]+(\.[A-Za-z0-9_]+)?(\s*,\s*[A-Za-z0-9_]+(\.[A-Za-z0-9_]+)?)*$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        #region 生成不带参数的Sql语句

        /// <summary>
        /// 动态生成添加的sql语句（值内联，仅用于调试或批量脚本，业务代码请用 <see cref="InsertSqlParam"/>）
        /// </summary>
        /// <param name="t">待添加的实体</param>
        /// <param name="IsItKey">是否排除自增主键</param>
        /// <returns>新增的sql语句</returns>
        public string InsertSql(T t, bool IsItKey = true)
        {
            ArgumentNullException.ThrowIfNull(t);

            PropertyInfo[] properties = ResolveProperties(IsItKey, forUpdate: false);
            if (properties.Length == 0)
                return string.Empty;

            string columns = string.Join(", ", properties.Select(p => EntityMetadata.QuoteIdentifier(p.Name)));
            string values = string.Join(", ", properties.Select(p => DBSqlParameter.SqlLiteral(p.GetValue(t))));

            return $"INSERT INTO {Meta.TableName}({columns}) VALUES({values})";
        }

        /// <summary>
        /// 动态生成按条件删除的sql语句
        /// </summary>
        /// <param name="CondiEntity">条件实体 如：new {Id=1}</param>
        /// <returns>按条件删除的sql语句</returns>
        public string DeleteSql(object CondiEntity)
            => $"DELETE FROM {Meta.TableName} WHERE {DBSqlParameter.GetSqlCondi(CondiEntity)}";

        /// <summary>
        /// 动态生成按条件更新的sql语句（值内联，业务代码请用 <see cref="UpdateSqlParam(T, object, bool)"/>）
        /// </summary>
        /// <param name="t">待更新到数据库的实体</param>
        /// <param name="CondiEntity">更新条件（如：new {Id=1}）</param>
        /// <param name="IsItKey">是否排除自增主键</param>
        /// <returns>按条件更新的sql语句</returns>
        public string UpdateSql(T t, object CondiEntity, bool IsItKey = true)
        {
            ArgumentNullException.ThrowIfNull(t);

            string setClause = BuildSetClause(ResolveProperties(IsItKey, forUpdate: true), p => DBSqlParameter.SqlLiteral(p.GetValue(t)));
            return $"UPDATE {Meta.TableName} SET {setClause} WHERE {DBSqlParameter.GetSqlCondi(CondiEntity)}";
        }

        /// <summary>
        /// 生成查询的sql语句
        /// </summary>
        /// <param name="CondiEntity">条件实体</param>
        /// <returns>查询的sql语句</returns>
        public string SelectSql(object CondiEntity)
            => $"SELECT {Meta.SelectColumns} FROM {Meta.TableName} WHERE {DBSqlParameter.GetSqlCondi(CondiEntity)}";

        /// <summary>
        /// 动态生成按查询所有的sql语句
        /// </summary>
        /// <returns>查询所有的sql语句</returns>
        public string SelectSql() => $"SELECT {Meta.SelectColumns} FROM {Meta.TableName} WHERE 1=1";

        #endregion

        #region 生成带参数的sql语句

        /// <summary>
        /// 生成带参数新增语句
        /// </summary>
        /// <param name="t">实体</param>
        /// <param name="IsItKey">是否排除自增主键</param>
        /// <returns></returns>
        public string InsertSqlParam(T t, bool IsItKey = true)
        {
            ArgumentNullException.ThrowIfNull(t);

            PropertyInfo[] properties = ResolveProperties(IsItKey, forUpdate: false);
            if (properties.Length == 0)
                return string.Empty;

            string columns = string.Join(", ", properties.Select(p => EntityMetadata.QuoteIdentifier(p.Name)));
            string values = string.Join(", ", properties.Select(p => "@" + p.Name));

            return $"INSERT INTO {Meta.TableName}({columns}) OUTPUT INSERTED.{Meta.KeyColumn ?? "[Id]"} VALUES({values})";
        }

        /// <summary>
        /// 生成删除的sql语句
        /// </summary>
        /// <param name="CondiEntity"></param>
        /// <returns></returns>
        public string DeleteSqlParam(object CondiEntity)
            => $"DELETE FROM {Meta.TableName} WHERE {DBSqlParameter.GetSqlCondi(CondiEntity)}";

        /// <summary>
        /// 生成查询条数
        /// </summary>
        /// <param name="CondiEntity"></param>
        /// <returns></returns>
        public string GetSqlCount(object CondiEntity)
            => $"SELECT COUNT(1) FROM {Meta.TableName} WHERE {DBSqlParameter.GetSqlCondi(CondiEntity)}";

        /// <summary>
        /// 动态生成按条件更新的sql语句
        /// </summary>
        /// <param name="t">待更新到数据库的实体</param>
        /// <param name="CondiEntity">更新条件（如：new {Id=1}）</param>
        /// <param name="IsItKey">是否排除自增主键</param>
        /// <returns>按条件更新的sql语句</returns>
        public string UpdateSqlParam(T t, object CondiEntity, bool IsItKey = true)
        {
            ArgumentNullException.ThrowIfNull(t);

            string setClause = BuildSetClause(ResolveProperties(IsItKey, forUpdate: true), p => "@" + p.Name);
            return $"UPDATE {Meta.TableName} SET {setClause} OUTPUT INSERTED.{Meta.KeyColumn ?? "[Id]"} WHERE {DBSqlParameter.GetSqlCondi(CondiEntity)}";
        }

        /// <summary>
        /// 生成按主键更新的sql语句
        /// </summary>
        /// <param name="UpdateContent">更新内容（如：new {Id=1, Name="张三"}）</param>
        /// <returns>按条件更新的sql语句</returns>
        public string UpdateSqlParam(object UpdateContent)
        {
            ArgumentNullException.ThrowIfNull(UpdateContent);

            string key = Meta.KeyName ?? "Id";
            string setClause = GetUpdateContent(UpdateContent);
            if (string.IsNullOrEmpty(setClause))
                throw new ArgumentException("更新内容不能为空", nameof(UpdateContent));

            return $"UPDATE {Meta.TableName} SET {setClause} OUTPUT INSERTED.{EntityMetadata.QuoteIdentifier(key)} WHERE {EntityMetadata.QuoteIdentifier(key)}=@{key}";
        }

        /// <summary>
        /// 动态生成按主键更新的sql语句
        /// </summary>
        /// <param name="t">待更新到数据库的实体</param>
        /// <returns>按条件更新的sql语句</returns>
        public string UpdateSqlParam(T t)
        {
            ArgumentNullException.ThrowIfNull(t);

            string key = Meta.KeyName ?? "Id";
            PropertyInfo[] properties = Meta.UpdatableProperties;
            if (properties.Length == 0)
                throw new InvalidOperationException($"实体 {typeof(T).Name} 没有可更新的属性");

            string setClause = BuildSetClause(properties, p => "@" + p.Name);
            return $"UPDATE {Meta.TableName} SET {setClause} OUTPUT INSERTED.{EntityMetadata.QuoteIdentifier(key)} WHERE {EntityMetadata.QuoteIdentifier(key)}=@{key}";
        }

        /// <summary>
        /// 生成查询的sql语句
        /// </summary>
        /// <param name="CondiEntity">条件实体</param>
        /// <returns>查询的sql语句</returns>
        public string SelectSqlParam(object CondiEntity)
            => CondiEntity == null
                ? SelectSqlParam()
                : $"SELECT {Meta.SelectColumns} FROM {Meta.TableName} WHERE {DBSqlParameter.GetSqlCondi(CondiEntity)}";

        /// <summary>
        /// 生成查询条数语句
        /// </summary>
        /// <param name="CondiEntity">条件实体</param>
        /// <returns>查询的sql语句</returns>
        public string SelectCountParam(object CondiEntity)
            => CondiEntity == null
                ? $"SELECT COUNT(1) FROM {Meta.TableName} WHERE 1=1"
                : $"SELECT COUNT(1) FROM {Meta.TableName} WHERE {DBSqlParameter.GetSqlCondi(CondiEntity)}";

        /// <summary>
        /// 动态生成按查询所有的sql语句
        /// </summary>
        /// <returns>查询所有的sql语句</returns>
        public string SelectSqlParam() => $"SELECT {Meta.SelectColumns} FROM {Meta.TableName}";

        #endregion

        /// <summary>
        /// 构建更新内容（排除主键）
        /// </summary>
        /// <param name="UpdateContent">条件实体 如：new {Id=1, Name="张三"}</param>
        /// <returns>形如 <c>[Name] = @Name</c> 的 SET 子句</returns>
        public string GetUpdateContent(object UpdateContent)
        {
            ArgumentNullException.ThrowIfNull(UpdateContent);

            string key = Meta.KeyName ?? "Id";
            PropertyInfo[] properties = UpdateContent.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
                .Where(p => !string.Equals(p.Name, key, StringComparison.OrdinalIgnoreCase))
                .ToArray();

            return BuildSetClause(properties, p => "@" + p.Name);
        }

        /// <summary>
        /// 生成判断是否存在的Sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string ExistenceSql(string sql)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(sql);
            return $"IF EXISTS({sql}) PRINT 1 ELSE PRINT 2";
        }

        /// <summary>
        /// 生成未确定查询结果的sql语句
        /// </summary>
        /// <returns>查询的sql语句</returns>
        public string SelectNoResultSql() => $"SELECT #Result FROM {Meta.TableName}";

        /// <summary>
        /// 获取排序的sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderType">排序类型</param>
        /// <returns></returns>
        public string GetPageSql(
            string sql,
            int pageIndex,
            int pageSize,
            string orderField = "",
            OrderType orderType = OrderType.ASC)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(sql);

            if (pageIndex < 1)
                pageIndex = 1;
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "每页条数必须大于 0");

            string orderBy = "ORDER BY (SELECT 0)";
            if (!string.IsNullOrWhiteSpace(orderField))
            {
                string field = orderField.Trim().TrimEnd(',');
                if (!OrderFieldRegex.IsMatch(field))
                    throw new ArgumentException($"排序字段不合法：{orderField}", nameof(orderField));

                orderBy = $"ORDER BY {field} {(orderType == OrderType.DESC ? "DESC" : "ASC")}";
            }

            int start = (pageIndex - 1) * pageSize;
            int end = pageIndex * pageSize;

            StringBuilder strSql = new();
            strSql.Append("SELECT * FROM (SELECT ROW_NUMBER() OVER (").Append(orderBy).Append(')');
            strSql.Append(" AS rowNum, * FROM (").Append(sql).Append(") AS T ) AS N");
            strSql.Append(" WHERE rowNum > ").Append(start).Append(" AND rowNum <= ").Append(end);
            return strSql.ToString();
        }

        /// <summary>
        /// 查询语句转化为获取条数Sql
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public string GetCountSql(string sql)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(sql);

            string trimmed = sql.Trim().TrimEnd(';');
            int selectIndex = FindFirstKeyword(trimmed, "SELECT", 0);
            if (selectIndex < 0)
                return $"SELECT COUNT(1) FROM ({trimmed}) AS _t";

            // 跳过 "SELECT" 关键字本身，再定位同一层的 FROM
            int fromIndex = FindFirstKeyword(trimmed, "FROM", selectIndex + "SELECT".Length);
            if (fromIndex < 0)
                return $"SELECT COUNT(1) FROM ({trimmed}) AS _t";

            return string.Concat(trimmed.AsSpan(0, selectIndex), "SELECT COUNT(1) ", trimmed.AsSpan(fromIndex));
        }

        #region 内部辅助

        /// <summary>
        /// 按是否排除主键取目标属性集合
        /// </summary>
        /// <param name="excludeKey">是否排除主键</param>
        /// <param name="forUpdate">是否为更新场景</param>
        private static PropertyInfo[] ResolveProperties(bool excludeKey, bool forUpdate)
        {
            if (!excludeKey)
                return Meta.WritableProperties;

            return forUpdate ? Meta.UpdatableProperties : Meta.InsertableProperties;
        }

        /// <summary>
        /// 拼接 SET 子句
        /// </summary>
        /// <param name="properties">参与赋值的属性</param>
        /// <param name="valueFactory">每个属性对应的右侧表达式</param>
        private static string BuildSetClause(IEnumerable<PropertyInfo> properties, Func<PropertyInfo, string> valueFactory)
        {
            string clause = string.Join(
                ", ",
                properties.Select(p => $"{EntityMetadata.QuoteIdentifier(p.Name)} = {valueFactory(p)}"));

            if (string.IsNullOrEmpty(clause))
                throw new InvalidOperationException($"实体 {typeof(T).Name} 没有可参与 SET 的属性");

            return clause;
        }

        /// <summary>
        /// 在顶层（跳过括号、字符串、注释）定位指定关键字
        /// </summary>
        /// <param name="sql">sql 文本</param>
        /// <param name="keyword">关键字</param>
        /// <param name="startIndex">起始扫描位置</param>
        /// <returns>关键字下标，未找到返回 -1</returns>
        private static int FindFirstKeyword(string sql, string keyword, int startIndex)
        {
            int depth = 0;
            char quote = '\0';

            for (int i = startIndex; i < sql.Length; i++)
            {
                char c = sql[i];

                if (quote != '\0')
                {
                    if (c == quote)
                        quote = '\0';
                    continue;
                }

                switch (c)
                {
                    case '\'':
                    case '"':
                        quote = c;
                        continue;
                    case '[':
                        quote = ']';
                        continue;
                    case '(':
                        depth++;
                        continue;
                    case ')':
                        depth--;
                        continue;
                    case '-' when i + 1 < sql.Length && sql[i + 1] == '-':
                        int lineEnd = sql.IndexOf('\n', i);
                        if (lineEnd < 0)
                            return -1;
                        i = lineEnd;
                        continue;
                }

                if (depth != 0 || c != keyword[0])
                    continue;

                if (string.Compare(sql, i, keyword, 0, keyword.Length, StringComparison.OrdinalIgnoreCase) != 0)
                    continue;

                bool leftOk = i == 0 || !IsIdentifierChar(sql[i - 1]);
                int after = i + keyword.Length;
                bool rightOk = after >= sql.Length || !IsIdentifierChar(sql[after]);
                if (leftOk && rightOk)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// 判断字符是否属于标识符组成字符
        /// </summary>
        private static bool IsIdentifierChar(char c)
            => char.IsLetterOrDigit(c) || c == '_' || c == '@' || c == '#';

        #endregion
    }
}


using System;
using System.Reflection;
using System.Text;
using Acme.Dapper.Enums;
using Acme.Tools;

namespace Acme.Dapper.SqlSplice
{

    /// <summary>
    /// 类名:动态生成不带参数的sql语句
    /// </summary>
    public class Splicing<T> : ISplicing<T> where T : class, new()
    {

        #region 生成不带参数的Sql语句
        /// <summary>
        /// 动态生成添加的sql语句
        /// </summary>
        /// <param name="t">待添加的实体</param>
        /// <param name="IsItKey">第一个字段是否是自增的主键（true是(默认是) false不是）</param>
        /// <returns>新增的sql语句</returns>
        public string InsertSql(T t, bool IsItKey = true)
        {
            try
            {
                StringBuilder Attribute = new();

                StringBuilder AttributeValue = new();

                PropertyInfo[] propertyInfo = t.GetType().GetProperties();

                int Index;

                if (IsItKey) Index = 1;

                else Index = 0;

                for (int i = Index; i < propertyInfo.Length; i++)
                {
                    if (i == Index)
                    {
                        Attribute.Append(propertyInfo[Index].Name);

                        AttributeValue.Append("'" + propertyInfo[Index].GetValue(t, null) + "'");
                    }
                    else
                    {
                        Attribute.Append(", " + propertyInfo[i].Name);

                        AttributeValue.Append(", '" + propertyInfo[i].GetValue(t, null) + "'");
                    }
                }

                string InserSql = string.Format("INSERT INTO {0}({1}) VALUES({2})", typeof(T).Name, Attribute, AttributeValue);

                return InserSql;
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// 动态生成按条件删除的sql语句
        /// </summary>
        /// <param name="CondiEntity">条件实体 如：new {Id=1}</param>
        /// <returns>按条件删除的sql语句</returns>
        public string DeleteSql(object CondiEntity)
        {
            try
            {
                string DeleteSql = string.Format("DELETE {0} WHERE {1}", typeof(T).Name, DBSqlParameter.GetSqlCondi(CondiEntity));

                return DeleteSql;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 动态生成按条件更新的sql语句
        /// </summary>
        /// <param name="t">待更新到数据库的实体</param>
        /// <param name="CondiEntity">更新条件（如：new {Id=1}）</param>
        /// <param name="IsItKey">第一个字段是否是自增的主键（true是 false不是）</param>
        /// <returns>按条件更新的sql语句</returns>
        public string UpdateSql(T t, object CondiEntity, bool IsItKey = true)
        {
            try
            {
                StringBuilder SETAppend = new();
                PropertyInfo[] propertyInfo = t.GetType().GetProperties();
                int Index;
                if (IsItKey) Index = 1;
                else Index = 0;
                for (int i = Index; i < propertyInfo.Length; i++)
                {
                    if (i == Index) SETAppend.Append(propertyInfo[Index].Name + " = '" + propertyInfo[Index].GetValue(t, null) + "'");
                    else SETAppend.Append("," + propertyInfo[i].Name + " = " + "'" + propertyInfo[i].GetValue(t, null) + "'");
                }
                string UpdateSql = string.Format("UPDATE {0} SET {1} WHERE {2}", typeof(T).Name, SETAppend, DBSqlParameter.GetSqlCondi(CondiEntity));

                return UpdateSql;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 生成查询的sql语句
        /// </summary>
        /// <param name="CondiEntity">条件实体</param>
        /// <returns>查询的sql语句</returns>
        public string SelectSql(object CondiEntity)
        {
            try
            {
                T t = Activator.CreateInstance<T>();
                StringBuilder AttributeValue = new();
                PropertyInfo[] propertyInfo = t.GetType().GetProperties();
                for (int i = 0; i < propertyInfo.Length; i++)
                {
                    if (i == 0) AttributeValue.Append(propertyInfo[0].Name);
                    else AttributeValue.Append(", " + propertyInfo[i].Name);
                }
                string SelectSql = string.Format("SELECT {0} FROM {1} WHERE {2}", AttributeValue, typeof(T).Name, DBSqlParameter.GetSqlCondi(CondiEntity));

                return SelectSql;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 动态生成按查询所有的sql语句
        /// </summary>
        /// <returns>查询所有的sql语句</returns>
        public string SelectSql()
        {
            try
            {
                T t = Activator.CreateInstance<T>();
                StringBuilder AttributeValue = new();
                PropertyInfo[] propertyInfo = t.GetType().GetProperties();
                for (int i = 0; i < propertyInfo.Length; i++)
                {
                    if (i == 0) AttributeValue.Append(propertyInfo[0].Name);
                    else
                    {
                        if (propertyInfo[i].Name.ToLower() == "Key".ToLower())
                        {
                            AttributeValue.Append(", " + $"[{propertyInfo[i].Name}]");
                        }
                        else
                        {
                            AttributeValue.Append(", " + propertyInfo[i].Name);
                        }
                    }

                }
                return string.Format("SELECT {0} FROM {1} WHERE 1=1", AttributeValue, typeof(T).Name);

            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region 生成带参数的sql语句

        /// <summary>
        /// 生成带参数新增语句
        /// </summary>
        /// <param name="t">实体</param>
        /// <param name="IsItKey">第一个是否主键</param>
        /// <returns></returns>
        public string InsertSqlParam(
            T t,
            bool IsItKey = true)
        {
            try
            {
                StringBuilder Attribute = new();
                StringBuilder AttributeValue = new();
                PropertyInfo[] propertyInfo = t.GetType().GetProperties();
                int Index;
                if (IsItKey)
                    Index = 1;
                else
                    Index = 0;
                for (int i = Index; i < propertyInfo.Length; i++)
                {
                    if (i == Index)
                    {
                        Attribute.Append(propertyInfo[Index].Name);
                        AttributeValue.Append($"@{propertyInfo[Index].Name}");
                    }
                    else
                    {
                        Attribute.Append(", " + propertyInfo[i].Name);
                        AttributeValue.Append($", @{propertyInfo[Index].Name}");
                    }
                }
                string InserSql = $"INSERT INTO {typeof(T).Name}({Attribute}) OUTPUT INSERTED.Id VALUES({AttributeValue})";
                return InserSql;
            }
            catch
            {
                return "";
            }

        }

        /// <summary>
        /// 生成删除的sql语句
        /// </summary>
        /// <param name="CondiEntity"></param>
        /// <returns></returns>
        public string DeleteSqlParam(object CondiEntity)
        {
            try
            {
                string DeleteSql = string.Format("DELETE {0} WHERE {1}", typeof(T).Name, DBSqlParameter.GetSqlCondi(CondiEntity));
                return DeleteSql;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 生成查询条数
        /// </summary>
        /// <param name="CondiEntity"></param>
        /// <returns></returns>
        public string GetSqlCount(object CondiEntity)
        {
            try
            {
                string DeleteSql = string.Format("SELECT Count(*) FROM {0} WHERE {1}", typeof(T).Name, DBSqlParameter.GetSqlCondi(CondiEntity));
                return DeleteSql;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 动态生成按条件更新的sql语句
        /// </summary>
        /// <param name="t">待更新到数据库的实体</param>
        /// <param name="CondiEntity">更新条件（如：new {Id=1}）</param>
        /// <param name="IsItKey">第一个字段是否是自增的主键（true是 false不是）</param>
        /// <returns>按条件更新的sql语句</returns>
        public string UpdateSqlParam(T t, object CondiEntity, bool IsItKey = true)
        {
            try
            {

                StringBuilder SETAppend = new();
                PropertyInfo[] propertyInfo = t.GetType().GetProperties();
                int Index;
                if (IsItKey) Index = 1;
                else Index = 0;
                for (int i = Index; i < propertyInfo.Length; i++)
                {
                    if (i == Index)
                        SETAppend.Append($"{propertyInfo[Index].Name} =  @{propertyInfo[Index].Name}");
                    else
                        SETAppend.Append($", {propertyInfo[i].Name} = @{propertyInfo[i].Name}");
                }
                string UpdateSql = $"UPDATE {typeof(T).Name} SET {SETAppend} OUTPUT INSERTED.Id WHERE {DBSqlParameter.GetSqlCondi(CondiEntity)}";

                return UpdateSql;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 动态生成按条件更新的sql语句
        /// </summary>
        /// <param name="UpdateContent">更新内容（如：new {Id=1}）</param>
        /// <returns>按条件更新的sql语句</returns>
        public string UpdateSqlParam(object UpdateContent)
        {
            string UpdateSql = $"UPDATE {typeof(T).Name} SET {GetUpdateContent(UpdateContent)} OUTPUT INSERTED.Id WHERE Id=@Id";
            return UpdateSql;
        }

        /// <summary>
        /// 动态生成按条件更新的sql语句
        /// </summary>
        /// <param name="t">待更新到数据库的实体</param>
        /// <returns>按条件更新的sql语句</returns>
        public string UpdateSqlParam(T t)
        {
            try
            {
                StringBuilder SETAppend = new();
                object Id = default;
                PropertyInfo[] propertyInfo = t.GetType().GetProperties();
                for (int i = 0; i < propertyInfo.Length; i++)
                {
                    if (propertyInfo[i].Name.ToUpper() == "id".ToUpper())
                        Id = propertyInfo[i].GetValue(t);
                    else
                    {
                        if (SETAppend.Length <= 0)
                            SETAppend.Append($"{propertyInfo[0].Name} =  @{propertyInfo[0].Name}");
                        else
                            SETAppend.Append($", {propertyInfo[i].Name} = @{propertyInfo[i].Name}");
                    }
                }
                string UpdateSql = $"UPDATE {typeof(T).Name} SET {SETAppend} OUTPUT INSERTED.Id WHERE Id='{Id}'";
                return UpdateSql;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 生成查询的sql语句
        /// </summary>
        /// <param name="CondiEntity">条件实体</param>
        /// <returns>查询的sql语句</returns>
        public string SelectSqlParam(object CondiEntity)
        {
            try
            {
                T t = Activator.CreateInstance<T>();
                StringBuilder AttributeValue = new();
                PropertyInfo[] propertyInfo = t.GetType().GetProperties();
                for (int i = 0; i < propertyInfo.Length; i++)
                {
                    if (i == 0)
                        AttributeValue.Append(propertyInfo[0].Name);
                    else
                        AttributeValue.Append(", " + propertyInfo[i].Name);
                }
                string SelectSql;
                if (CondiEntity != null)
                    SelectSql = $"SELECT {AttributeValue} FROM {typeof(T).Name} WHERE {DBSqlParameter.GetSqlCondi(CondiEntity)}";
                else
                    SelectSql = $"SELECT {AttributeValue} FROM {typeof(T).Name}";
                return SelectSql;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 生成查询条数语句
        /// </summary>
        /// <param name="CondiEntity">条件实体</param>
        /// <returns>查询的sql语句</returns>
        public string SelectCountParam(object CondiEntity)
        {
            try
            {
                T t = Activator.CreateInstance<T>();
                PropertyInfo[] propertyInfo = t.GetType().GetProperties();
                string SelectSql;
                if (CondiEntity != null)
                    SelectSql = $"SELECT COUNT(*) FROM {typeof(T).Name} WHERE {DBSqlParameter.GetSqlCondi(CondiEntity)}";
                else
                    SelectSql = $"SELECT COUNT(*) FROM {typeof(T).Name} WHERE 1=1";
                return SelectSql;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 动态生成按查询所有的sql语句
        /// </summary>
        /// <returns>查询所有的sql语句</returns>
        public string SelectSqlParam()
        {
            try
            {
                T t = Activator.CreateInstance<T>();
                StringBuilder AttributeValue = new();
                PropertyInfo[] propertyInfo = t.GetType().GetProperties();
                for (int i = 0; i < propertyInfo.Length; i++)
                {
                    if (i == 0)
                        AttributeValue.Append(propertyInfo[0].Name);
                    else
                        AttributeValue.Append($", {propertyInfo[i].Name}");
                }
                return $"SELECT {AttributeValue} FROM {typeof(T).Name}";
            }
            catch
            {
                return "";
            }
        }
        #endregion

        /// <summary>
        /// 构建修改参数
        /// </summary>
        /// <param name="UpdateContent">条件实体 如：new {Id=1}</param>
        /// <returns></returns>
        public string GetUpdateContent(object UpdateContent)
        {
            string UpdateContentString = "";
            PropertyInfo[] propertyInfo = UpdateContent.GetType().GetProperties();
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                if (propertyInfo[i].Name.ToUpper() != "id".ToUpper())
                {
                    if (UpdateContentString.Length <= 0)
                        UpdateContentString = $"{propertyInfo[i].Name} = @{propertyInfo[i].Name}";
                    else
                        UpdateContentString = $",{propertyInfo[i].Name} = @{propertyInfo[i].Name}";
                }
            }
            return UpdateContentString;
        }

        /// <summary>
        /// 生成判断是否存在的Sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string ExistenceSql(string sql)
        {
            return $"IF EXISTS({sql})PRINT 1 ELSE PRINT 2";
        }

        /// <summary>
        /// 生成未确定查询结果的sql语句
        /// </summary>
        /// <returns>查询的sql语句</returns>
        public string SelectNoResultSql()
        {
            try
            {
                T t = Activator.CreateInstance<T>();
                string SelectSql = $"SELECT #Result FROM {typeof(T).Name}";
                return SelectSql;
            }
            catch
            {
                return string.Empty;
            }
        }

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
            StringBuilder strSql = new();
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            if (!string.IsNullOrEmpty(orderField))
            {
                orderField = orderField.Trim();
                int lo = orderField.LastIndexOf(",");
                if (lo == orderField.Length - 1)
                {
                    orderField = orderField[0..^1];
                }
            }
            int num = (pageIndex - 1) * pageSize;
            int num1 = pageIndex * pageSize;
            string OrderBy;
            if (!string.IsNullOrEmpty(orderField))
                OrderBy = "ORDER BY " + orderField + " " + orderType.ToString() + "";
            else
                OrderBy = "ORDER BY (SELECT 0)";
            strSql.Append("SELECT * FROM (SELECT ROW_NUMBER() OVER (" + OrderBy + ")");
            strSql.Append(" AS rowNum, * FROM (" + sql + ") AS T ) AS N WHERE rowNum > " + num + " AND rowNum <= " + num1 + "");
            return strSql.ToString();
        }

        /// <summary>
        /// 查询语句转化为获取条数Sql
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public string GetCountSql(string sql)
        {
            string sqlConten= sql.MidStrEx("SELECT ", "FROM ");
            return sql.Replace(sqlConten, " COUNT(1) ");
        }
    }
}


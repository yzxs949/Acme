using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
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
        /// 
        /// </summary>
        public const string 一 = "-";

        /// <summary>
        /// 构建SqlServer数据库的sql参数
        /// </summary>
        /// <param name="ParameterEntity">实体(如：new{Id=1,Name='夜殇'})</param>
        /// <returns>SqlParameter类型的sql参数数组</returns>
        public static SqlParameter[] SqlParame(this object ParameterEntity)
        {
            object values;
            List<SqlParameter> Listparameter = new();
            foreach (PropertyInfo p in ParameterEntity.GetType().GetProperties())
            {
                try
                {
                    //接收属性名和属性值
                    string attribute = "@" + p.Name;
                    values = p.GetValue(ParameterEntity, null);//参数值
                    SqlParameter parameter = new(attribute, values);
                    Listparameter.Add(parameter);
                }
                catch
                {
                    continue;
                }
            }
            SqlParameter[] Arrayparameter = Listparameter.ToArray();
            return Arrayparameter;
        }

        /// <summary>
        /// 构建条件参数
        /// </summary>
        /// <param name="CondiEntity">条件实体 如：new {Id=1}</param>
        /// <returns></returns>
        public static string GetSqlCondi(object CondiEntity)
        {
            string RetSqlCondi = "";
            try
            {
                PropertyInfo[] property = CondiEntity.GetType().GetProperties();
                foreach (var item in property)
                {
                    if (property.First() == item)
                        RetSqlCondi = $"{item.Name} = @{item.Name}";
                    else
                        RetSqlCondi += $" AND {item.Name} = @{item.Name}";
                }
            }
            catch
            {
                throw;
            }
            return RetSqlCondi;
        }

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
            if (isJudgment && condiEntity != null)
            {
                PropertyInfo[] property = condiEntity.GetType().GetProperties();
                string AndOrNotName = Enum.GetName(andOrNot.GetType(), andOrNot);
                foreach (var item in property)
                {
                    object Values = item.GetValue(condiEntity);
                    if (Values != null)
                    {
                        sql += $" " +
                            $"{AndOrNotName}" +
                            $" {item.Name} " +
                            $"{symbol} " +
                            $"'{item.GetValue(condiEntity)}'";
                    }
                }
            }
            return sql;
        }

        /// <summary>
        /// 添加模糊查询
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
        {
            try
            {
                if (isJudgment && condiEntity != null)
                {
                    PropertyInfo[] property = condiEntity.GetType().GetProperties();
                    string AndOrNotName = Enum.GetName(andOrNot.GetType(), andOrNot);
                    object Values = property[0].GetValue(condiEntity);
                    if (Values != null)
                    {
                        sql += $" {AndOrNotName} {property[0].Name} LIKE '%{Values}%'";
                    }
                }
                return sql;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 拼接左模糊查询
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
        {
            try
            {
                if (isJudgment && condiEntity != null)
                {
                    PropertyInfo[] property = condiEntity.GetType().GetProperties();
                    string AndOrNotName = Enum.GetName(andOrNot.GetType(), andOrNot);
                    object Values = property[0].GetValue(condiEntity);
                    if (Values != null)
                    {
                        sql += $" " +
                               $"{AndOrNotName}" +
                               $" {property[0].Name}" +
                               $" LIKE '{Values}%'";
                    }
                }
                return sql;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 拼接右模糊查询
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
        {
            try
            {
                if (isJudgment && condiEntity != null)
                {
                    PropertyInfo[] property = condiEntity.GetType().GetProperties();
                    object Values = property[0].GetValue(condiEntity);
                    if (Values != null)
                    {
                        string AndOrNotName = Enum.GetName(andOrNot.GetType(), andOrNot);
                        sql += $" " +
                               $"{AndOrNotName}" +
                               $" {property[0].Name}" +
                               $" LIKE '%{Values}'";
                    }
                }
                return sql;
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// 获取的结果集
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string Select(this string sql, object result)
        {
            if (sql.Contains("#Result"))
            {
                string strResult = "";
                PropertyInfo[] propertyInfo = result.GetType().GetProperties();
                for (int i = 0; i < propertyInfo.Length; i++)
                {
                    if (strResult.Length <= 0)
                        strResult = $"{propertyInfo[i].GetValue(propertyInfo)} as {propertyInfo[i].Name}";
                    else
                        strResult = $",{propertyInfo[i].GetValue(propertyInfo)}, {propertyInfo[i].Name}";
                }
                return sql.Replace("#Result", strResult);
            }
            else
            {
                throw new Exception("sql参数错误");
            }
        }

        /// <summary>
        /// 获取数据条数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string Total(this string sql)
        {
            if (sql.Contains("#Result"))
            {
                return sql.Replace("#Result", "Count(*)");
            }
            else
            {
                throw new Exception("sql参数错误");
            }
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
            if (sql.Contains("#Result"))
            {
                sql += $" AS T{index} {join} JOIN {typeof(T).Name} AS T{index + 1}";
                return sql;
            }
            else
            {
                throw new Exception("sql参数错误");
            }
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
            if (keyValues.Count == 0)
            {
                throw new Exception("sql参数错误");
            }
            else
            {
                sql += " ON";
                int row = 0;
                foreach (var item in keyValues)
                {
                    if (row == 0)
                    {
                        sql += $" {item.Key}={item.Value}";
                    }
                    else
                    {
                        sql += $"AND {item.Key}={item.Value}";
                    }
                    row++;
                }
                return sql;
            }
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


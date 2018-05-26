using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Data;


namespace GS.SQL.DataSource
{
    public class SQLEnityResolve
    {
        private SQLEnityResolve()
        {

        }
        private static SQLEnityResolve _enityResolve = new SQLEnityResolve();
        public static SQLEnityResolve Get
        {
            get
            {
                return _enityResolve;
            }
        }
        public string GetTableName(object obj)
        {
            return GetTableName(obj);
        }
        public string GetTableName(Type type)
        {
            var table = type.GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault();
            if (table != null)
                return ((TableAttribute)table).Name;
            return type.Name;
        }
        public IEnumerable<PropertyInfo> GetPropertyInfo(object obj)
        {
            return obj.GetType().GetProperties();
        }
        public SQLPage<T> ConvertPage<T>(object obj, DataSet ds)
        {
            var resutl = new SQLPage<T>();
            resutl.Result = Convert<T>(obj, ds.Tables[0]);
            resutl.Count = System.Convert.ToInt32(ds.Tables[1].Rows[0]["total"]);
            resutl.Pagination = System.Convert.ToInt32(ds.Tables[1].Rows[0]["rows"]);
            return resutl;
        }
        private object GetValue(object value, ColumnAttribute columnAttribute)
        {
            if (columnAttribute.Type == DbType.DateTime)
                return System.Convert.ToDateTime(value);
            else if (columnAttribute.Type == DbType.String)
                return value.ToString();
            else if (columnAttribute.Type == DbType.Int32)
                return System.Convert.ToInt32(value);
            else if (columnAttribute.Type == DbType.Int16)
                return System.Convert.ToInt16(value);
            else if (columnAttribute.Type == DbType.Int64)
                return System.Convert.ToInt64(value);
            else if (columnAttribute.Type == DbType.Double)
                return System.Convert.ToDouble(value);
            else if (columnAttribute.Type == DbType.Single)
                return System.Convert.ToSingle(value);
            else if (columnAttribute.Type == DbType.Boolean)
                return System.Convert.ToBoolean(value);
            return value.ToString();
        }
        public IEnumerable<T> Convert<T>(object obj, DataTable data)
        {
            Type type = obj.GetType();
            var colNames = new List<string>();
            foreach (DataColumn item in data.Columns)
                colNames.Add(item.ColumnName.ToLower());

            return data.AsEnumerable().Select(item =>
            {
                T model = (T)Activator.CreateInstance(type);
                GetPropertyInfo(model).ToList().ForEach(t =>
                {
                    var attribute = t.GetCustomAttributes(typeof(ColumnAttribute), false).FirstOrDefault();
                    if (attribute == null)
                        return;

                    var columnAttribute = (ColumnAttribute)attribute;
                    if (!colNames.Contains(columnAttribute.Name))
                        return;

                    var value = item[columnAttribute.Name];
                    if (value != DBNull.Value)
                    {
                        if (t.PropertyType.IsEnum)
                            t.SetValue(model, Enum.Parse(t.PropertyType, value.ToString()));
                        else
                            t.SetValue(model, GetValue(value, columnAttribute));
                    }
                });
                return model;
            });
        }
        public IEnumerable<SQLEnityFiled> GetSQLEnityFiled(object obj)
        {
            var result = new List<SQLEnityFiled>();
            var column = GetPropertyInfo(obj);
            foreach (var item in column)
            {
                var attribute = item.GetCustomAttributes(typeof(ColumnAttribute), false).FirstOrDefault();
                var value = item.GetValue(obj);
                if (attribute == null)
                {
                    result.Add(new SQLEnityFiled()
                    {
                        ColName = item.Name,
                        IsIdent = false,
                        Type = DbType.String,
                        Value = value == null ? null : value.ToString(),
                        EntName = item.Name,
                    });
                }
                var columnAttribute = (ColumnAttribute)attribute;
                if (!columnAttribute.IsDbOption)
                    continue;

                result.Add(new SQLEnityFiled()
                {
                    ColName = columnAttribute.Name,
                    IsIdent = columnAttribute.Ident,
                    Type = columnAttribute.Type,
                    Value = value == null ? null : value.ToString(),
                    EntName = item.Name,
                    Key = columnAttribute.Key
                });

            }
            return result;

        }
    }
}

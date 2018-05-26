using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace GS.SQL.DataSource
{
    public class SQLExecute
    {
        private readonly DBHelper _dbHelper;
        public SQLExecute(string connection)
        {
            _dbHelper = new DBHelper(connection);
        }
        private string GetParamIdentifying()
        {
            return Guid.NewGuid().ToString().Substring(0, 7);
        }
        private object[] GetUpdateSqlString(IEnumerable<SQLEnityFiled> values, string tblName)
        {
            var sql = new StringBuilder().AppendFormat(" update [{0}] set ", tblName);
            var sqlParam = new List<DbParameter>();
            var keyItem = values.Where(t => t.Key).FirstOrDefault();
            if (keyItem == null)
                throw new Exception("enitiy key is not fount");

            var keyName = string.Format("@{0}", GetParamIdentifying() + keyItem.ColName);

            sqlParam.Add(new SqlParameter
            {
                ParameterName = keyName,
                DbType = keyItem.Type,
                Value = keyItem.Value
            });

            var content = values.Select(item =>
            {
                if (item.Key)
                    return string.Empty;
                if (string.IsNullOrEmpty(item.Value))
                    return string.Empty;
                var valueName = string.Format("@{0}", GetParamIdentifying() + item.ColName);

                sqlParam.Add(new SqlParameter
                {
                    ParameterName = valueName,
                    DbType = item.Type,
                    Value = item.Value
                });
                return string.Format(" [{0}]={1}", item.ColName, valueName);
            });

            return new object[]{
                sql.Append(string.Join(",", content.Where(t=>!string.IsNullOrEmpty(t)))).AppendFormat(" where {0}={1}", keyItem.ColName, keyName).ToString(),
                sqlParam
        };

        }
        public int Add(IEnumerable<IEnumerable<SQLEnityFiled>> val, string tblName)
        {
            var sql = string.Empty;
            var sqlParam = new List<DbParameter>();
            foreach (var values in val)
            {
                var col = new List<string>();
                var value = new List<string>();
                foreach (var item in values)
                {
                    if (item.IsIdent)
                        continue;
                    if (string.IsNullOrEmpty(item.Value))
                        continue;
                    var valueName = string.Format("@{0}", GetParamIdentifying() + item.ColName);
                    sqlParam.Add(new SqlParameter
                    {
                        ParameterName = valueName,
                        DbType = item.Type,
                        Value = item.Value
                    });
                    col.Add(string.Format("[{0}]", item.ColName));
                    value.Add(valueName);
                }
                sql = sql + string.Format("insert into [{0}]({1}) values({2}); ", tblName,
                string.Join(",", col),
                string.Join(",", value));
            }
            sql = sql + "; SELECT SCOPE_IDENTITY() AS newid;";

            return Convert.ToInt32(Query(sql, sqlParam).Tables[0].Rows[0]["newid"]);
        }
        private StringBuilder GetDeleteParam(IEnumerable<SQLEnityFiled> values, List<DbParameter> sqlParam, string tblName)
        {
            var sql = new StringBuilder().AppendFormat("delete [{0}] where", tblName);
            var where = values.Select(item =>
            {
                if (!item.Key)
                    return string.Empty;

                if (string.IsNullOrEmpty(item.Value))
                    return string.Empty;

                var valueName = string.Format("@{0}", GetParamIdentifying() + item.ColName);

                sqlParam.Add(new SqlParameter
                {
                    ParameterName = valueName,
                    DbType = item.Type,
                    Value = item.Value
                });
                return string.Format(" {0}={1}", item.ColName, valueName);
            });
            return sql.Append(string.Join(" and ", where.Where(t => !string.IsNullOrEmpty(t))));
        }
        public int Delete(IEnumerable<IEnumerable<SQLEnityFiled>> values, string tblName)
        {
            var sqlParam = new List<DbParameter>();
            var sql = new StringBuilder();
            foreach (var item in values)
            {
                sql.Append(GetDeleteParam(item, sqlParam, tblName));
            }
            return Exec(sql.ToString(), sqlParam);
        }
        public int Delete(IEnumerable<SQLEnityFiled> values, string tblName)
        {
            var sqlParam = new List<DbParameter>();
            var sql = GetDeleteParam(values, sqlParam, tblName);
            return Exec(sql.ToString(), sqlParam);
        }
        public int Update(IEnumerable<SQLEnityFiled> values, string tblName)
        {
            var obj = GetUpdateSqlString(values, tblName);
            return Exec(obj[0].ToString(), (List<DbParameter>)obj[1]);
        }
        public int Update(IEnumerable<IEnumerable<SQLEnityFiled>> values, string tblName)
        {
            var sql = new StringBuilder();
            var sqlParam = new List<DbParameter>();

            foreach (var item in values)
            {
                var obj = GetUpdateSqlString(item, tblName);
                sql.Append(obj[0].ToString());
                sqlParam.AddRange((List<DbParameter>)obj[1]);
            }
            return Exec(sql.ToString(), sqlParam);
        }
        public DataSet Query(string sql, IEnumerable<DbParameter> param)
        {
            if (param == null)
                return _dbHelper.ExecuteDataSetTp(sql);
            return _dbHelper.ExecuteDataSetTp(sql, param.ToArray());
        }
        public DataTable Query(string tblName, SQLCondition condition,
            IEnumerable<DbParameter> param = null,
            string sqlStr = null)
        {
			if (string.IsNullOrEmpty(sqlStr))
				sqlStr = "select * from [{0}]";

			var sql = string.Format(sqlStr, tblName);
			if (condition != null)
				sql = string.Format("{0} {1}", sql, condition.VirtualExpression);
			return Query(sql, param).Tables[0];
		}
        public DataSet Query(string tblName, int current, int page,
            SQLCondition condition,
            IEnumerable<DbParameter> param = null,
            string sqlStr = null,
            string sqlTotal = null)
        {
            if (string.IsNullOrEmpty(sqlStr))
                sqlStr = "select* from (select row_number()over(order by tempColumn)rownumber, * from(select top {0} tempColumn = 0, * from {1})a )b where rownumber > {2}";

            if (string.IsNullOrEmpty(sqlTotal))
                sqlTotal = "select count(0) as total,case when (count(0)%{0}) = 0 then (count(0)/{0}) else (count(0)/{0})+1  end as rows from {1}";

            var exp = string.Empty;
            var order = string.Empty;
            if (condition != null)
            {
                exp = condition.VirtualExpression.ToLower();
                if (exp.IndexOf("order") != -1)
                {
                    order = exp.Substring(exp.IndexOf("order"));
                    exp = exp.Replace(order, string.Empty);
                }
            }

            var sql = new StringBuilder().AppendFormat(
                sqlStr
                , current * page + page
                , $"[{tblName}] {exp} {order}"
                , current * page
            );

            sql.AppendFormat(
                sqlTotal
                , page
                , $"[{tblName}] {exp}"
            );
            
            return Query(sql.ToString(), param);
        }
        public int Exec(string sql)
        {
            return _dbHelper.ExecuteNonQueryTp(sql);
        }
        public int Exec(string sql, IEnumerable<DbParameter> param)
        {
            return _dbHelper.ExecuteNonQueryTp(sql, param.ToArray());
        }
    }
}

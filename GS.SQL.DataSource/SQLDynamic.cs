using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using GS.Common.Util;
using System.Data.Common;
using System.Data.SqlClient;

namespace GS.SQL.DataSource
{
    public class SQLDynamic
    {
        private readonly DBHelper _dbHelper;
        public SQLDynamic()
        {

        }
        public SQLDynamic(string database)
        {
            Database = database;
            _dbHelper = new DBHelper(database);
        }
        public string Database { get; set; }
        private string GetDatabase(string db)
        {
            if (db.IsNullOrEmpty())
                return Database;
            return db;
        }
        public Boolean IsDBExist(string db = null)
        {
            db = GetDatabase(db);
            var createDbStr = " select * from master.dbo.sysdatabases where name " + "= '" + db + "'";
            var dt = _dbHelper.ExecuteDataSetTp(createDbStr).Tables[0];
            return dt.Rows.Count > 0;
        }
        public Boolean IsTableExist(string db = null, string tb = null)
        {
            db = GetDatabase(db);
            var createDbStr = "use " + db + " select 1 from sysobjects where id = object_id('" + tb + "') and type ='U'";
            var dt = _dbHelper.ExecuteDataSetTp(createDbStr).Tables[0];
            return dt.Rows.Count > 0;
        }
        public void CreateDataBase(string db = null)
        {
            db = GetDatabase(db);
            var flag = IsDBExist(db);
            if (flag)
            {
                throw new Exception("数据库已经存在！");
            }
            else
            {
                var createDbStr = "Create database " + db;
                _dbHelper.ExecuteNonQueryTp(createDbStr);
            }
        }
        public void CreateDataTable(string dt, Dictionary<string, string> dic, string db = null)
        {
            db = GetDatabase(db);
            if (!IsDBExist(db))
            {
                throw new Exception("数据库不存在！");
            }
            if (IsTableExist(db, dt))
            {
                throw new Exception("数据库表已经存在！");
            }
            else
            {
                string content = " Id int identity(1,1) primary key, ";
                content = content + " [Updatetime] [datetime] NULL ";

                List<string> test = new List<string>(dic.Keys);
                for (int i = 0; i < dic.Count(); i++)
                {
                    content = content + "," + test[i] + "" + dic[test[i]];
                }

                string createTableStr = "use " + db + " create table " + dt + " (" + content + ")";
                Common.Util.LogUtil.InfoFormat(createTableStr);
                _dbHelper.ExecuteNonQueryTp(createTableStr);
            }
        }
        public bool DropDataTable(string[] dt, string db = null)
        {
            db = GetDatabase(db);
            if (!IsDBExist(db))
            {
                throw new Exception("数据库不存在！");
            }

            for (int i = 0; i < dt.Count(); i++)
            {
                if (!IsTableExist(db, dt[i]))
                {
                    continue;
                }
                else
                {
                    string createTableStr = "use " + db + " drop table " + dt[i] + " ";
                    _dbHelper.ExecuteNonQueryTp(createTableStr);
                }
            }
            return true;
        }
        public bool DropDataBase(string db = null)
        {
            db = GetDatabase(db);
            Boolean flag = IsDBExist(db);

            if (!flag)
            {
                return false;
            }
            else
            {
                string createDbStr = "Drop database " + db;
                _dbHelper.ExecuteNonQueryTp(createDbStr);
                return true;
            }
        }
        public void CreateDataTable(string[] dt, List<Dictionary<string, string>> dic, string db = null)
        {
            db = GetDatabase(db);
            if (!IsDBExist(db))
            {
                throw new Exception("数据库不存在！");
            }

            for (int i = 0; i < dt.Count(); i++)
            {
                if (IsTableExist(db, dt[i]))
                {
                    continue;
                }
                else
                {
                    string createTableStr = JoinSql(dt[i], dic[i], db);
                    _dbHelper.ExecuteNonQueryTp(createTableStr);
                }
            }
        }
        public string JoinSql(string dt, Dictionary<string, string> dic, string db = null)
        {
            db = GetDatabase(db);
            string content = "Id int identity(1,1) primary key ";

            List<string> test = new List<string>(dic.Keys);
            for (int i = 0; i < dic.Count(); i++)
            {
                content = content + " , " + test[i] + " " + dic[test[i]];
            }
            string createTableStr = "use " + db + " create table " + dt + " (" + content + ")";
            return createTableStr;
        }
        public void RemoveFormTableById(int id, string name)
        {
            var sql = $" delete {name} where Id=@id;";
            var dbParameter = new List<DbParameter>
            {
               new SqlParameter { ParameterName = "@id", DbType = DbType.Int32, Value = id}
            };
            _dbHelper.ExecuteNonQueryTp(sql, dbParameter.ToArray());
        }
        public int ValidateDyncForm(string tblName, string filedName, string value, int id)
        {
            var sql = $"select * from [{tblName}] where {filedName}='{value}'";
            if (id != -1)
            {
                sql += $" and id<>{id}";
            }
            return _dbHelper.ExecuteDataSetTp(sql).Tables[0].Rows.Count;
        }
        public int UpdateDyncForm(SQLDynamicRow sQLDynamicRow, string tblName, int id)
        {
            var model = sQLDynamicRow.Row.ToList();
            model.Add(new SQLDynamicItem()
            {
                Name = "[Updatetime]",
                Value = DateTime.UtcNow.ToString("yyyy-MM-dd hh:ss")
            });
            var content = new StringBuilder();
            foreach (var item in model)
            {
                if (item.Type == "number")
                    content.Append("" + item.Name + "=" + item.Value + ",");
                else
                    content.Append("" + item.Name + "='" + item.Value + "',");
            }
            var sql = $"update [{tblName}] set {content.ToString().TrimEnd(',')} where id={id}";
            LogUtil.InfoFormat(sql);
            return _dbHelper.ExecuteNonQueryTp(sql);
        }
        public int AddDyncForm(SQLDynamicRow sQLDynamicRow, string tblName)
        {
            var model = sQLDynamicRow.Row.ToList();
            model.Add(new SQLDynamicItem()
            {
                Name = "[Updatetime]",
                Value = DateTime.UtcNow.ToString("yyyy-MM-dd hh:ss")
            });

            var key = new StringBuilder();
            var value = new StringBuilder();

            foreach (var item in model)
            {
                key.Append(item.Name + ",");
                if (item.Type == "number")
                    value.Append("" + item.Value + ",");
                else
                    value.Append("'" + item.Value + "',");
            }
            var sql = $"insert into [{tblName}]({key.ToString().TrimEnd(',')}) values ({value.ToString().TrimEnd(',')})";
            LogUtil.InfoFormat(sql);
            return _dbHelper.ExecuteNonQueryTp(sql);
        }
        public IEnumerable<SQLDynamicItem> GetDyncValueByDyncParam(string tblName, string filedNameName, SQLCondition param = null)
        {
            var where = param != null ? param.WhereExpression : string.Empty;
            var sql = $"select {filedNameName} from [{tblName}] {where}";
            var dataList = _dbHelper.ExecuteDataSetTp(sql).Tables[0];
            if (dataList.Rows.Count > 0)
            {
                return dataList.AsEnumerable().Select(t =>
                {
                    return new SQLDynamicItem()
                    {
                        Name = filedNameName,
                        Value = t[filedNameName] != DBNull.Value ? t[filedNameName].ToString() : string.Empty
                    };
                });
            }
            return null;
        }
        private string GetValue(object row)
        {
            var value = string.Empty;
            if (row != DBNull.Value)
            {
                var type = row.GetType();
                if (type == typeof(DateTime) || type == typeof(DateTime?))
                    value = Convert.ToDateTime(row).ToString("yyyy-MM-dd");
                else
                    value = row.ToString();
            }
            return value;
        }
        public SQLDynamicRow GetDyncDataById(int id, string tblName)
        {
            var sql = $"select * from [{tblName}] where id=@id";
            var dbParameter = new List<DbParameter>
            {
               new SqlParameter {
                   ParameterName = "@id",
                   DbType = DbType.Int32,
                   Value = id}
            };
            var dataList = _dbHelper.ExecuteDataSetTp(sql, dbParameter.ToArray()).Tables[0];
            var name = new List<string>();
            foreach (DataColumn item in dataList.Columns)
            {
                name.Add(item.ColumnName);
            }
            var row = dataList.AsEnumerable().SingleOrDefault();
            return new SQLDynamicRow
            {
                DataId = id,
                Row = name.Select(n =>
                {
                    return new SQLDynamicItem()
                    {
                        Name = n,
                        Value = GetValue(row[n])
                    };
                })
            };
        }
        public SQLPage<SQLDynamicRow> GetDyncGrid(int page, int current, string tblName, string where = "", string order = "")
        {
            var sqlStr = "select * from (select row_number()over(order by tempColumn)rownumber, * from(select top {0} tempColumn = 0, * from {1})a )b where rownumber > {2}";
            var sqlTotal = "select count(0) as total,case when (count(0)%{0}) = 0 then (count(0)/{0}) else (count(0)/{0})+1  end as rows from {1}";

            var sql = new StringBuilder().AppendFormat(
                sqlStr
                , current * page + page
                , $"[{tblName}] {where} {order}"
                , current * page
            );

            sql.AppendFormat(
                sqlTotal
                , page
                , $"[{tblName}] {where}"
            );
            var dataSet = _dbHelper.ExecuteDataSetTp(sql.ToString());
            var dataList = dataSet.Tables[0];
            var name = new List<string>();

            foreach (DataColumn item in dataList.Columns)
            {
                if (item.ColumnName == "tempColumn" || item.ColumnName == "rownumber")
                    continue;
                name.Add(item.ColumnName);
            }

            var sqlPage = new SQLPage<SQLDynamicRow>()
            {
                Result = dataList.AsEnumerable().Select(t =>
                {
                    return new SQLDynamicRow()
                    {
                        Row = name.Select(n =>
                        {
                            return new SQLDynamicItem()
                            {
                                Name = n,
                                Value = GetValue(t[n])
                            };
                        })
                    };
                }),
                Count = System.Convert.ToInt32(dataSet.Tables[1].Rows[0]["total"]),
                Pagination = System.Convert.ToInt32(dataSet.Tables[1].Rows[0]["rows"])
            };
            return sqlPage;
        }
    }
}


using System.Collections.Generic;
using System.Linq;
using System.Data.Common;


namespace GS.SQL.DataSource
{
    public interface ISQLDataEnity<T>
    {
        int Exec(string sql);
        int Exec(string sql, IEnumerable<DbParameter> param);
        int Add(T model);
        int Add(IEnumerable<T> model);
        int Delete(T model);
        int Delete(IEnumerable<T> model);
        int Update(T model);
        int Update(IEnumerable<T> model);
        IEnumerable<T> Query(SQLCondition condition = null, IEnumerable<DbParameter> param = null, string sql = null);
        SQLPage<T> Query(int pageSize, int currentPage, SQLCondition condition = null, IEnumerable<DbParameter> param = null, string sql = null, string sqlTotal = null);
    }
    public class SQLDataEnity<T> : ISQLDataEnity<T>
    {
        public readonly SQLExecute _sqlExecute;
        public SQLDataEnity(string connection)
        {
            _sqlExecute = new SQLExecute(connection);
        }
        private IEnumerable<SQLEnityFiled> GetSQLEnityFiled(object model)
        {
            return SQLEnityResolve.Get.GetSQLEnityFiled(model);
        }
        private string GetTableName
        {
            get
            {
                return SQLEnityResolve.Get.GetTableName(typeof(T));
            }
        }
        public int Add(T model)
        {
            var list = new List<IEnumerable<SQLEnityFiled>>();
            list.Add(GetSQLEnityFiled(model));
            return _sqlExecute.Add(list, GetTableName);
        }
        public int Add(IEnumerable<T> model)
        {
            return _sqlExecute.Add(model.Select(t => GetSQLEnityFiled(t)), GetTableName);
        }
        public int Exec(string sql)
        {
            return _sqlExecute.Exec(sql);
        }
        public int Exec(string sql, IEnumerable<DbParameter> param)
        {
            return _sqlExecute.Exec(sql, param);
        }
        public int Delete(T model)
        {
            return _sqlExecute.Delete(GetSQLEnityFiled(model), GetTableName);
        }
        public int Delete(IEnumerable<T> model)
        {
            return _sqlExecute.Delete(
              model.Select(t => GetSQLEnityFiled(t)), GetTableName
            );
        }
        public T QueryFirst(SQLCondition condition = null, IEnumerable<DbParameter> param = null, string sql = null)
        {
            return Query(condition, param, sql).FirstOrDefault();
        }
        public IEnumerable<T> Query(SQLCondition condition = null, IEnumerable<DbParameter> param = null, string sql = null)
        {
            return SQLEnityResolve.Get.Convert<T>(this, _sqlExecute.Query(GetTableName, condition, param, sql));
        }
        public SQLPage<T> Query(int pageSize, int currentPage, SQLCondition condition = null, IEnumerable<DbParameter> param = null, string sql = null, string sqlTotal = null)
        {
            var sqlPage = SQLEnityResolve.Get.ConvertPage<T>(this,
                _sqlExecute.Query(GetTableName, currentPage, pageSize, condition, param, sql, sqlTotal));
            sqlPage.IsNext = sqlPage.Pagination > currentPage;
            return sqlPage;
        }
        public int Update(IEnumerable<T> model)
        {
            return _sqlExecute.Update(
             model.Select(t => GetSQLEnityFiled(t)), GetTableName
            );
        }
        public int Update(T model)
        {
            return _sqlExecute.Update(GetSQLEnityFiled(model), GetTableName);
        }
        public void Mapping(T item, SQLCondition condition)
        {
            item = Query(condition).FirstOrDefault();
        }
        public void Mapping(IEnumerable<T> item, SQLCondition condition)
        {
            item = Query(condition);
        }
    }

}

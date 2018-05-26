using GS.SQL.DataSource;
using GS.SQLModel.Form;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.Services
{
    public interface IDynamicDataservices
    {
        void RemoveFormTableById(int id, string name);
        void UpdateForm(DyncForm dyncForm, IEnumerable<DyncField> dyncField);
        void CreateTable(DyncForm dyncForm, IEnumerable<DyncField> dyncField);
        int IsTableExist(string name);
        int Add(DyncForm item);
        int ValidateDyncForm(string tableName, string filedName, string value, int id);
        int AddDyncTableData(SQLDynamicRow sQLDynamicRow, string tableName);
        int UpdateDyncTableData(SQLDynamicRow sQLDynamicRow, string tableName, int id);
        int AddDyncField(IEnumerable<DyncField> dyncField);
        string RemoveForm(string code);
        IEnumerable<DyncField> GetDyncFieldByFormCode(string formCode);
        IEnumerable<DyncField> GetDyncField(int dyncForm);
        IEnumerable<DyncForm> GetDyncFormByName(string name);   
        IEnumerable<SQLDynamicItem> GetDyncValueByDyncParam(string tableName, string filedNameName, IDictionary<string, string> param = null);
        IEnumerable<DyncForm> GetDyncForm();
        SQLPage<SQLDynamicRow> GetDyncFormTableByCodeAndSearchValue(int pageSize, int currentPage, string value, string code);
        SQLPage<DyncForm> GetDyncFormInfo(int pageSize, int currentPage);
        SQLPage<DyncForm> GetDyncForm(int pageSize, int currentPage);  
        SQLPage<DyncForm> GetDyncFormByName(int pageSize, int currentPage, string name);
        SQLPage<SQLDynamicRow> GetDyncFormTableByCode(int pageSize, int currentPage, string code);
        DyncForm GetDyncFormByCode(string code);
        SQLDynamicRow GetDyncDataById(int id, string tableName);
    }
    public class DynamicDataservices : IDynamicDataservices
    {
        private readonly SQLDynamic _sQLDynamic;
        private readonly DyncForm _dyncForm;
        private readonly DyncField _dyncField;
        public DynamicDataservices()
        {
            _sQLDynamic = new SQLDynamic("DyncData");
            _dyncForm = new DyncForm();
            _dyncField = new DyncField();
        }
        public int IsTableExist(string name)
        {
            return _sQLDynamic.IsTableExist(null, name) ? 1 : 0;
        }
        public int Add(DyncForm item)
        {
            return _dyncForm.Add(item);
        }
        public string RemoveForm(string code)
        {
            var formName = GetDyncFormByCode(code).Name;
            if (_sQLDynamic.IsTableExist(tb: formName))
            {
                _sQLDynamic.DropDataTable(new string[] {
                    formName
                });
            }

            var sql = " delete DyncForm where Code=@code; delete DyncField where DyncForm=@code;";
            var dbParameter = new List<DbParameter>
            {
               new SqlParameter { ParameterName = "@code", DbType = DbType.String, Value = code}
            };
            _dyncForm.Exec(sql, dbParameter);

            return formName;
        }
        public IEnumerable<DyncField> GetDyncField(int dyncForm)
        {
            var condition = new SQLCondition();
            condition.Expression = "where DyncForm =@DyncForm";
            var dbParameter = new List<DbParameter>
            {
               new SqlParameter { ParameterName = "@DyncForm", DbType = DbType.String, Value = dyncForm}
            };
            return _dyncField.Query(condition: condition, param: dbParameter);
        }
        public int AddDyncField(IEnumerable<DyncField> dyncField)
        {
            return _dyncField.Add(dyncField);
        }
        public SQLPage<DyncForm> GetDyncFormInfo(int pageSize, int currentPage)
        {
            return _dyncForm.Query(pageSize, currentPage);
        }
        public IEnumerable<DyncForm> GetDyncForm()
        {
            var condition = new SQLCondition();
            condition.Expression = "where IsDelete=0 order by Updatetime desc";
            return _dyncForm.Query(condition:condition);
        }
        public SQLPage<DyncForm> GetDyncForm(int pageSize, int currentPage)
        {
            var condition = new SQLCondition();
            condition.Expression = "where IsDelete=0 order by Updatetime desc";

            return _dyncForm.Query(pageSize, currentPage, condition);
        }
        public IEnumerable<DyncField> GetDyncFieldByFormCode(string formCode)
        {
            var condition = new SQLCondition();
            condition.Expression = "where DyncForm =@formCode";
            var dbParameter = new List<DbParameter>
            {
               new SqlParameter { ParameterName = "@formCode", DbType = DbType.String, Value = formCode}
            };
            return _dyncField.Query(condition, dbParameter);
        }
        public DyncForm GetDyncFormByCode(string code)
        {
            var condition = new SQLCondition();
            condition.Expression = "where Code =@code and IsDelete=0";
            var dbParameter = new List<DbParameter>
            {
               new SqlParameter { ParameterName = "@code", DbType = DbType.String, Value = code}
            };
            return _dyncForm.Query(condition, dbParameter).FirstOrDefault();
        }
        public IEnumerable<DyncForm> GetDyncFormByName(string name)
        {
            var condition = new SQLCondition();
            condition.Expression = $"where name = '{name}' and IsDelete=0";
            return _dyncForm.Query(condition);
        }
        public SQLPage<DyncForm> GetDyncFormByName(int pageSize, int currentPage, string name)
        {
            var condition = new SQLCondition();
            condition.Expression = "where name like '%" + name + "%' and IsDelete=0 order by Updatetime desc";
            return _dyncForm.Query(pageSize, currentPage, condition);
        }
        public SQLPage<SQLDynamicRow> GetDyncFormTableByCodeAndSearchValue(int pageSize, int currentPage, string value, string code)
        {
            var dyncForm = GetDyncFormByCode(code);
            var filed = GetDyncField(dyncForm.Id);
            var where = $"where {string.Join(" or ", filed.Select(t => $"{t.Name} like '%{value}%'"))}";
            var sqlPage = _sQLDynamic.GetDyncGrid(pageSize, currentPage, dyncForm.Name, where: where, order: "order by updateTime desc");
            return sqlPage;
        }
        public SQLPage<SQLDynamicRow> GetDyncFormTableByCode(int pageSize, int currentPage, string code)
        {
            var dyncForm = GetDyncFormByCode(code);
            var sqlPage = _sQLDynamic.GetDyncGrid(pageSize, currentPage, dyncForm.Name, order: "order by updateTime desc");
            return sqlPage;
        }
        public SQLDynamicRow GetDyncDataById(int id, string tableName)
        {
            return _sQLDynamic.GetDyncDataById(id, tableName);
        }
        public void RemoveFormTableById(int id, string name)
        {
            _sQLDynamic.RemoveFormTableById(id, name);
        }
        public void UpdateForm(DyncForm dyncForm, IEnumerable<DyncField> dyncField)
        {
            RemoveForm(dyncForm.Code);
            CreateTable(dyncForm, dyncField);
        }
        public int ValidateDyncForm(string tableName, string filedName, string value, int id)
        {
            return _sQLDynamic.ValidateDyncForm(tableName, filedName, value, id);
        }
        public int AddDyncTableData(SQLDynamicRow sQLDynamicRow, string tableName)
        {
            return _sQLDynamic.AddDyncForm(sQLDynamicRow, tableName);
        }
        public int UpdateDyncTableData(SQLDynamicRow sQLDynamicRow, string tableName, int id)
        {
            return _sQLDynamic.UpdateDyncForm(sQLDynamicRow, tableName, id);
        }
        public IEnumerable<SQLDynamicItem> GetDyncValueByDyncParam(string tableName, string filedNameName, IDictionary<string, string> param = null)
        {
            var condition = new SQLCondition();
            if (param != null)
            {
                condition.Where = param.Select(t =>
                {
                    return new Where()
                    {
                        FiledName = t.Key,
                        Value = t.Value
                    };
                });
            }
            return _sQLDynamic.GetDyncValueByDyncParam(tableName, filedNameName, condition);
        }
        public void CreateTable(DyncForm dyncForm, IEnumerable<DyncField> dyncField)
        {
            Add(dyncForm);
            var dic = dyncField.ToDictionary(t => t.Name, x => x.Info);
            AddDyncField(dyncField);
            _sQLDynamic.CreateDataTable(dyncForm.Name, dic);
        }
    }
}

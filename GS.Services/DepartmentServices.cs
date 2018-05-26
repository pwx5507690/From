using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GS.SQLModel;
using GS.SQL.DataSource;

namespace GS.Services
{
    public interface IDepartmentServices {
        int AddDepartment(Department department);
        int DeleteDepartment(Department department);
        int DeleteDepartment(IEnumerable<Department> department);
        int ExistByCode(string code, int id);
        int ExistByName(string name, int id);
        int UpdateDepartment(Department department);
        IEnumerable<Department> GetDepartment();
        IEnumerable<Department> GetDepartmentById(int[] id);
        IEnumerable<Department> GetDepartmentByName(string name);
        SQLPage<Department> GetDepartment(int pageSize, int currentPage);
        SQLPage<Department> GetDepartmentByName(string name, int pageSize, int currentPage);
        Department GetDepartmentById(int id);     
    }
    public class DepartmentServices: IDepartmentServices
    {
		private readonly Department _department;
		public DepartmentServices()
		{
			_department = new Department();
		}
		public int AddDepartment(Department department)
		{
			department.Updatetime = DateTime.Now;
			return _department.Add(department);
		}
		public int DeleteDepartment(Department department)
		{
			return _department.Delete(department);
		}
		public int DeleteDepartment(IEnumerable<Department> department)
		{
			return _department.Delete(department);
		}
		public int ExistByCode(string code, int id)
		{
			var dept = _department.Query().Where(t => t.Code == code);
			if (dept.Any())
			{
				if (id != -1 && dept.Count() == 1 && dept.SingleOrDefault().Id == id)
				{
					return 1;
				}
				return 0;
			}
			return 1;
		}
		public int ExistByName(string name, int id)
		{
			var dept = _department.Query().Where(t => t.Name == name);
			if (dept.Any())
			{
				if (id != -1 && dept.Count() == 1 && dept.SingleOrDefault().Id == id)
                    return 1;
                return 0;
			}
			return 1;
		}
		public int UpdateDepartment(Department department)
		{
			department.Updatetime = DateTime.Now;
			return _department.Update(department);
		}
		public IEnumerable<Department> GetDepartment()
		{
			return _department.Query(sql: "select a.*,b.Name as ParentName from [{0}] as a left join [{0}] as b on a.Parent = b.Id ");
		}
		public SQLPage<Department> GetDepartment(int pageSize, int currentPage)
		{
			var sql = "select b.*,c.Name as ParentName from (select row_number()over(order by tempColumn)rownumber, * from(select top {0} tempColumn = 0, * from {1})a )b left join {1} as c on b.Parent = c.Id where rownumber > {2}";
			return _department.Query(pageSize, currentPage, sql: sql);
		}
		public IEnumerable<Department> GetDepartmentByName(string name)
		{
			return this.GetDepartment().Where(t => t.Name.Contains(name));
		}
		public SQLPage<Department> GetDepartmentByName(string name, int pageSize, int currentPage)
		{
			var condition = new SQLCondition();
			condition.Expression = "where Name like '%" + name + "%'";
			var sql = "select b.*,c.Name as ParentName from (select row_number()over(order by tempColumn)rownumber, * from(select top {0} tempColumn = 0, * from {1})a )b left join [Department] as c on b.Parent = c.Id where rownumber > {2}";
			return _department.Query(pageSize, currentPage, condition, sql: sql);
		}
		public Department GetDepartmentById(int id)
		{
			return this.GetDepartment().Where(t => id == t.Id).SingleOrDefault();
		}
		public IEnumerable<Department> GetDepartmentById(int[] id)
		{
			return this.GetDepartment().Where(t => id.Contains(t.Id));
		}
	}
}

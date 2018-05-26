using GS.Services;
using GS.SQLModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GS.Api.Controllers
{

	[RoutePrefix("api/department")]
	public class DepartmentController : BaseController
	{
		public readonly IDepartmentServices _departmentServices;
		public DepartmentController(IDepartmentServices departmentServices)
		{
			_departmentServices = departmentServices;
		}
		[HttpPost]
		[Route("add")]
		public int Add(Department department)
		{
			return _departmentServices.AddDepartment(department);
		}
		[HttpDelete]
		[Route("delete/{id}")]
		public int Delete(int id)
		{
			var department = new Department();
			department.Id = id;
			return _departmentServices.DeleteDepartment(department);
		}
		[HttpPut]
		[Route("update")]
		public int Update(Department department)
		{
			return _departmentServices.UpdateDepartment(department);
		}
		[HttpGet]
		[Route("page/{pageSize}/current/{currentPage}")]
		public SQL.DataSource.SQLPage<Department> GetDepartment(int pageSize, int currentPage)
		{
			return _departmentServices.GetDepartment(pageSize, --currentPage);
		}
		[HttpGet]
		[Route("")]
		public IEnumerable<Department> GetDepartment()
		{
			return _departmentServices.GetDepartment();
		}
		[HttpGet]
		[Route("{id}")]
		public Department Query(int id)
		{
			return _departmentServices.GetDepartmentById(id);
		}
		[HttpGet]
		[Route("page/{pageSize}/current/{currentPage}/name/{name}")]
		public SQL.DataSource.SQLPage<Department> GetDepartmentByName(int pageSize, int currentPage, string name)
		{
			return _departmentServices.GetDepartmentByName(name, pageSize, --currentPage);
		}
		[HttpGet]
		[Route("existCode/{code}/{id}")]
		public int ExistByCode(string code, int id)
		{
			return _departmentServices.ExistByCode(code, id);
		}
		[HttpGet]
		[Route("existName/{name}/{id}")]
		public int ExistByName(string name, int id)
		{
			return _departmentServices.ExistByName(name, id);
		}
	}
}

using GS.Services;
using GS.SQLModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GS.View.Model;

namespace GS.Api.Controllers
{
	[RoutePrefix("api/role")]
	public class RoleController : BaseController
	{
		private readonly IRoleServices _role;
		public RoleController(IRoleServices role)
		{
			_role = role;
		}
		[HttpDelete]
		[Route("delete/{id}")]
		public int Delete(int id)
		{
			var role = new Role();
			role.Id = id;
			return _role.DeleteRole(role);
		}
		[HttpPut]
		[Route("update")]
		public int Update(VRole vRole)
		{
			return _role.UpdateRole(vRole);
		}
		[HttpPost]
		[Route("add")]
		public int Add(VRole vRole)
		{
			return _role.AddRole(vRole);
		}
		[HttpGet]
		[Route("")]
		public IEnumerable<Role> GetRole()
		{
			return _role.GetRole();
		}
		[HttpGet]
		[Route("vRole/page/{pageSize}/current/{currentPage}/name/{name}")]
		public VRole GetVRole(int pageSize, int currentPage, string name)
		{
			return _role.GetVRole(pageSize, --currentPage, name: name);
		}
		[HttpGet]
		[Route("vRole/page/{pageSize}/current/{currentPage}")]
		public VRole GetVRole(int pageSize, int currentPage)
		{
			return _role.GetVRole(pageSize, --currentPage);
		}
		[HttpGet]
		[Route("{id}")]
		public VRole GetRoleById(int id)
		{
			return _role.GetVRoleByRoleId(id);
		}
		[HttpGet]
		[Route("userRole/{id}")]
		public IEnumerable<UserRole> GetUserRoleByUserId(int id) {
			return _role.GetUserRoleByUserId(id);
		}
		[HttpGet]
		[Route("userRole")]
		public IEnumerable<UserRole> GetUserRole()
		{
			return _role.GetUserRole();
		}
	}
}

using System;
using System.Collections.Generic;
using GS.Services;
using GS.SQLModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GS.View.Model;

namespace GS.Api.Controllers
{
    [RoutePrefix("api/menu")]
    public class MenuController : BaseController
    {
        private readonly IRoleServices _roleServices;
        private readonly IMenuServices _menuServices;
        public MenuController(IRoleServices roleServices, IMenuServices menuServices)
        {
            _roleServices = roleServices;
            _menuServices = menuServices;
        }
        [HttpGet]
        public IEnumerable<VMenu> GetMenu()
        {
            return _menuServices.GetVMenu();
        }
        [HttpGet]
        [Route("currentUser")]
        public IEnumerable<VMenu> GetMenuByUserRole()
        {
            var userRole = _roleServices.GetUserRoleByUserId(CurrentUser.Id).Select(t => t.Role).Distinct();
            var menuRole = _roleServices.GetMenuRole().Where(t => userRole.Contains(t.Role)).Select(t => t.Menu).Distinct();
            var menu = _menuServices.GetVMenu().ToList();
            foreach (var item in menu)
                item.Menu = item.Menu.Where(t => menuRole.Contains(t.Id));
            menu = menu.Where(t => t.Menu.Any()).ToList();
            return menu;
        }
        [HttpGet]
        [Route("page/{pageSize}/current/{currentPage}/name/{name}")]
        public SQL.DataSource.SQLPage<Menu> GetMenuByName(int pageSize, int currentPage, string name)
        {
            return _menuServices.GetMenuByName(name, pageSize, --currentPage);
        }
        [HttpGet]
        [Route("page/{pageSize}/current/{currentPage}")]
        public SQL.DataSource.SQLPage<Menu> GetMenu(int pageSize, int currentPage)
        {
            return _menuServices.GetMenu(pageSize, --currentPage);
        }
        [HttpGet]
        [Route("query")]
        public IEnumerable<Menu> GetMenuAll()
        {
            return _menuServices.GetMenu();
        }
        [HttpPost]
        [Route("add")]
        public int AddMenu(Menu item)
        {
            item.Updatetime = DateTime.UtcNow;
            return _menuServices.AddMenu(item);
        }
        [HttpPut]
        [Route("update")]
        public int Update(Menu item)
        {
            item.Updatetime = DateTime.UtcNow;
            return _menuServices.Update(item);
        }
        [HttpDelete]
        [Route("delete/{id}")]
        public int DeleteMenu(int id)
        {
            return _menuServices.Delete(new Menu() { Id = id });
        }
    }
}

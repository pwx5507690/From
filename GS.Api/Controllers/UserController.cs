using GS.Services;
using GS.SQLModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace GS.Api.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : BaseController
    {
        private readonly IUserServices _userServices;
        private readonly IRoleServices _roleServices;
        private readonly string _defaultPassword = "123456";
        public UserController(IUserServices userServices, IRoleServices roleServices)
        {
            _userServices = userServices;
            _roleServices = roleServices;
        }
        [HttpGet]
        [Route("page/{pageSize}/current/{currentPage}")]
        public SQL.DataSource.SQLPage<User> GetUser(int pageSize, int currentPage)
        {
            return _userServices.GetUser(pageSize, --currentPage);
        }
        [HttpGet]
        [Route("existPhone/{phone}/{id}")]
        public int ExistByPhone(string phone, int id)
        {
            return _userServices.ExistByPhone(phone, id);
        }
        [HttpGet]
        [Route("existEmail/{mail}/{id}")]
        public int ExistByEmail(string mail, int id)
        {
            return _userServices.ExistByEmail(mail, id);
        }
        [HttpGet]
        [Route("page/{pageSize}/current/{currentPage}/name/{name}")]
        public SQL.DataSource.SQLPage<User> GetUserByName(int pageSize, int currentPage, string name)
        {
            return _userServices.GetUserByName(name, pageSize, --currentPage);
        }
        [HttpDelete]
        [Route("delete/{id}")]
        public int Delete(int id)
        {
            return _userServices.Delete(new User { Id = id });
        }
        [HttpPut]
        [Route("update")]
        public int Update(JObject value)
        {
            var user = value.ToObject<User>();
            user.Updatetime = DateTime.UtcNow;

            _userServices.Update(user);
            BindUserRole(user.Id, value);
            return user.Id;

        }
        [HttpPost]
        [Route("add")]
        public int Add(JObject value)
        {
            var user = value.ToObject<User>();
            user.Updatetime = DateTime.UtcNow;
            user.Password = _defaultPassword;

            var r = _userServices.Add(user);
            BindUserRole(r, value);
            return r;
        }
        [HttpGet]
        [Route("{id}")]
        public User Query(int id)
        {
            return _userServices.GetUserById(id);
        }
        private void BindUserRole(int userId, JObject value)
        {
            if (userId > 0 && value["role"] != null)
            {
                var role = value["role"].ToObject<IEnumerable<int>>().Select(t => new UserRole() { User = userId, Role = t });
                _roleServices.BindUserRole(role);
            }
        }
    }
}

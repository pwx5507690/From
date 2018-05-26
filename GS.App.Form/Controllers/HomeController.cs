using GS.Cache.Identity;
using GS.SQLModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GS.Services;

namespace GS.App.Form.Controllers
{
    public class HomeController : BaseController
    {
        public readonly IRoleServices _iRoleServices;
        public HomeController(IRoleServices iRoleServices) {
            _iRoleServices = iRoleServices;
        }
        public ActionResult Index()
        {
            var user = BaseIdentity.GetUser<User>();
            var userRole = _iRoleServices.GetUserRoleByUserId(user.Model.Id).FirstOrDefault();
            var token = BaseIdentity.GetClientToken();
            ViewBag.Token = token;
            ViewBag.User = user.Model;
            ViewBag.UserRole = userRole;
            return View();
        }
    }
}
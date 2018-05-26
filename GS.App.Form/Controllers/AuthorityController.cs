using GS.Cache.Identity;
using GS.Common.Util;
using GS.SQLModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GS.App.Form.Controllers
{
    public class AuthorityController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return Redirect($"~/{IdentityConstant._identityTarget}");
        }
        [HttpGet]
        public ActionResult Logout()
        {
            BaseIdentity.Remove();
            return Index();
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult IsAuthority()
        {
            if (BaseIdentity.GetUser<User>().IsNull())
                return new ContentResult() { Content = "error" };
            return new ContentResult() { Content = "success" };
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string token, string action)
        {
            if (token.IsNotNullOrEmpty())
            {
                BaseIdentity.SetClientToken(token);
                if (BaseIdentity.GetUser<User>().IsNotNull())
                    Response.Redirect(action ?? "/DyncForm", true);
            }
            return Index();
        }
    }
}
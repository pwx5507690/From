using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GS.Services;
using GS.Cache.Identity;
using GS.View.Model;
using GS.SQLModel;
using GS.Common.Web;
using GS.Common.Util;
using System.Net;
using Login = GS.View.Model.Login;
using System.IO;
using GS.Api.Model.Config;

namespace GS.App.Identity.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly ISsoServices _ssoServices;
        public readonly IConfigParamServices _iConfigParamServices;
        private const string REMEMBER = "REMEMBER";
        [HttpPost]
        public string Validator(string token)
        {
            return BaseIdentity.GetIdentity<User>(token).SerializeObject();
        }
        public LoginController(IUserServices userServices, ISsoServices ssoServices, IConfigParamServices iConfigParamServices)
        {
            _userServices = userServices;
            _ssoServices = ssoServices;
            _iConfigParamServices = iConfigParamServices;
        }
        public ActionResult Param()
        {
            var method = Request.HttpMethod.ToLower();
            var uuid = Request.Headers["Authorization"];
            if (method == "get")
            {
                var result = _iConfigParamServices.Param(uuid);
                if (result.Item1 != 200)
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

                return new ContentResult() { Content = result.Item2.SerializeObject() };
            }
            else if (method == "post")
            {
                var appsetting = _iConfigParamServices.Param(uuid);
                var content = new StreamReader(Request.InputStream).ReadToEnd();
                var code = _iConfigParamServices.Param(uuid, content.DeserializeObject<AppSettings>());
                if (code != 200)
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                return new ContentResult() { Content = code.ToString() };
            }
            return new HttpStatusCodeResult(HttpStatusCode.MethodNotAllowed);

        }
        [HttpPost]
        public ActionResult Index(string loaction, string loginUrl)
        {
            if (loaction.IsNullOrEmpty())
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.Message = string.Empty;
            var login = GetRemember();

            login.Location = loaction;
            login.LoginUrl = loginUrl;
            return View(login);
        }
        [HttpPost]
        public ActionResult Sso(string key)
        {
            var sso = _ssoServices.GetSsoByKey(key);
            if (sso.IsNull())
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.Message = string.Empty;
            var login = GetRemember();
            login.Location = sso.Location;
            return View("Index", login);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserLogin(Login model)
        {
            var vLogin = _userServices.Login(model.Name, model.Password);
            if (vLogin.IsLogin)
            {
                SetRemember(model);
                var client = Client.GetAddress();
                _userServices.AddLogin(vLogin, client.Ipv4);
                vLogin.User.Updatetime = DateTime.UtcNow;
                var identityModel = new IdentityModel<User>()
                {
                    LoginTime = DateTime.UtcNow,
                    Model = vLogin.User
                };
                BaseIdentity.SetIdentity<User>(vLogin.SessionId, identityModel);
                model.SessionId = vLogin.SessionId;
                model.IsLogin = true;
            }
            if (vLogin.LoginStats == LoginStats.NAME)
            {
                ViewBag.Message = "邮箱或手机号错误.";
                model.IsLogin = false;
            }
            else if (vLogin.LoginStats == LoginStats.PASSOWRD)
            {
                ViewBag.Message = "密码错误.";
                model.IsLogin = false;
            }
            return View("Index", model);
        }
        private Login GetRemember()
        {
            var value = Client.GetCookieValue(REMEMBER);
            if (value.IsNotNullOrEmpty())
                return value.DeserializeObject<Login>();

            return new Login();
        }
        private void SetRemember(Login model)
        {
            var isRememberMe = Request.Form["RememberMe"];
            if (model.RememberMe == 1)
                Client.SetCookie(REMEMBER, model.SerializeObject());
        }
    }
}
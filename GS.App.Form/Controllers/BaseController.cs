using GS.Cache.Identity;
using GS.SQLModel;
using System.Web;
using System.Web.Mvc;
using System.IO;
using GS.Common.Util;
using System.Text;
using System.Web.Configuration;
using Microsoft.AspNet.SignalR.Client;
using System.Collections.Generic;

namespace GS.App.Form.Controllers
{
    public class BaseController : Controller
    {

        public const int _pageSize = 15;
        private const string _message = "message";
        
        public void SetMessage(string message)
        {
            Common.Web.Client.SetCookie(_message, HttpUtility.UrlEncode(message, Encoding.GetEncoding("GB2312")));
        }
        public string GetMessage()
        {
            var message = Common.Web.Client.GetCookieValue(_message);
            if (message.IsNotNullOrEmpty())
            {
                ViewBag.message = HttpUtility.UrlDecode(message, Encoding.GetEncoding("GB2312"));
                Common.Web.Client.ClearCookie(_message);
            }
            return message;
        }
        public string PostContent
        {
            get
            {
                return new StreamReader(Request.InputStream).ReadToEnd();
            }
        }
        public T GetPostContentModal<T>()
        {
            return PostContent.DeserializeObject<T>();
        }
        public new IdentityModel<User> User
        {
            get
            {
                return BaseIdentity.GetUser<User>();
            }
        }
    }
}
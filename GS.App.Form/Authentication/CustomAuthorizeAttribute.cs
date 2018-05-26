using GS.Cache.Identity;
using GS.SQLModel;
using GS.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GS.App.Form.Result;

namespace GS.App.Form.Authentication
{

	public class CustomAuthorizeAttribute : AuthorizeAttribute
	{
		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			var user = BaseIdentity.GetUser<User>();
			return user.IsNotNull();
		}

        protected virtual ActionResult GetActionResult()
		{
			return new RedirectResult("~/Authority/Index");
		}

		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			filterContext.Result = GetActionResult();
		}
	}
	public class AjaxAuthorizeAttribute : CustomAuthorizeAttribute
	{
		protected override ActionResult GetActionResult()
		{
			return new ExceptionResult("401");
		}
	}
}
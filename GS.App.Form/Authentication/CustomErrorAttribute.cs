using GS.App.Form.Result;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace GS.App.Form.Authentication
{
	public class CustomErrorAttribute : HandleErrorAttribute
	{
		public override void OnException(ExceptionContext filterContext)
		{
			var writeLog = true;
			if (filterContext.Exception.GetType().ToString() == typeof(FormException).ToString())
                writeLog = ((FormException)filterContext.Exception).WriteLog;

            var customErrors = ((CustomErrorsSection)WebConfigurationManager.GetSection("system.web/customErrors"));

			var exResult = new ExceptionResult
			(
				filterContext.Exception.Message,
				isWriteLog: writeLog,
				controller: filterContext.Controller.ToString(),
				stackTrace: filterContext.Exception.StackTrace
			);
			if (customErrors.Mode == CustomErrorsMode.On)
			{
				exResult.ExecuteResult(filterContext);
				filterContext.RequestContext.HttpContext.Response.End();
			}
			else {
				base.OnException(filterContext);
			}
		}
	}
}
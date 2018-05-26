using GS.App.Form.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GS.App.Form.App_Start
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new CustomErrorAttribute());
			filters.Add(new CustomAuthorizeAttribute());
		}
	}
}
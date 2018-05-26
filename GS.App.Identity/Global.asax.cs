using GS.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace GS.App.Identity
{
	public class MvcApplication : Application
	{
		protected override void Application_Start()
		{
            base.Application_Start();
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
		}
	}
}

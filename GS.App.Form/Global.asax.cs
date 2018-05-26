using GS.App.Form.App_Start;
using GS.Cache.Identity;
using GS.Common.Injection.Core;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace GS.App.Form
{
	public class MvcApplication : IdentityApplication
	{
		public override void Start()
		{
            BundleConfig.RegisterBundles(BundleTable.Bundles);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);	
		}

	}
}

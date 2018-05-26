using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace GS.App.Identity
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.UseCdn = true;
			bundles.Add(new ScriptBundle("~/js/login")
			   .Include("~/Assets/js/jquery.min.js")
			   .Include("~/Assets/js/amazeui.min.js")
			   .Include("~/Assets/js/app.js")
			   .Include("~/Assets/js/login.js"));

			bundles.Add(new StyleBundle("~/css/login")
	.Include("~/Assets/css/amazeui.min.css")
	//	.Include("~/Assets/css/amazeui.datatables.min.css")
	.Include("~/Assets/css/app.css"));

#if DEBUG
			BundleTable.EnableOptimizations = false;
#else
                BundleTable.EnableOptimizations = true;
#endif
		}
	}
}
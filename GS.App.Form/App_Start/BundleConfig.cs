using System.Web;
using System.Web.Optimization;

namespace GS.App.Form
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
          
            bundles.Add(new StyleBundle("~/formBulid/css").Include(
				"~/css/common.css",
				"~/css/jquery-ui-1.9.2.custom.css",
				"~/css/widgets.css",
				"~/css/jquery.mCustomScrollbar.min.css",
				"~/css/formbuild.css",
				"~/css/pagerstyles.css"));

			bundles.Add(new StyleBundle("~/formView/css").Include(
				"~/Assets/css/amazeui.min.css",
				"~/Assets/css/amazeui.datatables.min.css",
				"~/Assets/css/app.css"));
		}
	}
}

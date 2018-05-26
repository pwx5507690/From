using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using GS.Common.Util;

namespace GS.Common.Web
{
	public class Cors : Constant
	{
		private const string OriginHeaderName = "Origin";
		public static void AddCorsHeadersIfAllowed()
		{
			var appConfigValue = WebConfigurationManager.AppSettings["CorsDomains"];
			var domainsAllowed = appConfigValue.Split(',');
			if (Request.Headers[OriginHeaderName].IsNotNull())
			{
				var origin = Request.Headers[OriginHeaderName];
				var matchedDomain = domainsAllowed.FirstOrDefault(origin.StartsWith);
				if (!String.IsNullOrWhiteSpace(matchedDomain))
				{
					Response.AddHeader("Access-Control-Allow-Origin",
						matchedDomain);
					Response.AddHeader("Access-Control-Allow-Credentials",
						"true");
					Response.AddHeader("Access-Control-Allow-Methods", "POST,GET,PUT,DELETE,OPTIONS");
				}
				if (Request.HttpMethod.ToUpper() == "OPTIONS")
				{
					Response.AddHeader("Access-Control-Allow-Headers", "x-requested-with,content-type,Authorization");
					Response.StatusCode = 200;
					Response.End();
				}
			}
		}
	}
}

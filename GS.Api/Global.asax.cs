using GS.Api.Attributes;
using GS.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace GS.Api
{
	public class WebApiApplication : Application
	{
		protected override void Application_Start()
		{
            GlobalConfiguration.Configuration.Filters.Add(new CustomErrorAttribute());
            GlobalConfiguration.Configure(
                httpConfiguration => WebApiConfig.Register(httpConfiguration));
            base.Application_Start();
		}
		protected void Application_BeginRequest()
		{   
            Cors.AddCorsHeadersIfAllowed();			
		}
	}
}

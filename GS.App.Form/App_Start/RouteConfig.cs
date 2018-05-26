using GS.App.Form.Route;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace GS.App.Form
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.Add(new CustomRoute());
            //routes.MapCustomRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            
            //var route = new System.Web.Routing.Route("{controller}/{action}/{id}",
            // new RouteValueDictionary(new { controller = "Home", action = "Index", id = UrlParameter.Optional }),
            // new RouteValueDictionary(),
            // new RouteValueDictionary(),
            // new MvcRouteHandler());
            //routes.Insert(0, route);
        }
	}
}

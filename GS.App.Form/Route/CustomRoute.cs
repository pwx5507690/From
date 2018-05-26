using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcRoute = System.Web.Routing.Route;

namespace GS.App.Form.Route
{
    public static class RouteExtensions
    {
        public static CustomRoute InsertCustomRoute(this RouteCollection routes, string name, string url, object defaults)
        {
            var route = new CustomRoute(url,
                new RouteValueDictionary(defaults),
                new RouteValueDictionary(),
                new RouteValueDictionary(),
                new MvcRouteHandler());
            routes.Add(name, route);
            return route;
        }
    }
    public class CustomRoute : MvcRoute
    {
        public CustomRoute(string url, 
            RouteValueDictionary defaults, 
            RouteValueDictionary constraints, 
            RouteValueDictionary dataTokens, IRouteHandler routeHandler)
        : base(url, defaults, constraints, dataTokens, routeHandler) {
        }
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            return base.GetRouteData(httpContext);
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return base.GetVirtualPath(requestContext, values);
        }
    }
}
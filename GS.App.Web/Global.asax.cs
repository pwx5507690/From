using Autofac;
using GS.Services;
using GS.Common.Web;
using GS.SQLModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace GS.App.Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        void Application_BeginRequest(object sender, EventArgs e)
        {
            var iCmsServices = new CmsServices();
            var rawUrl = HttpContext.Current.Request.RawUrl;
            var value = rawUrl.Split('/').Where(t => !string.IsNullOrEmpty(t));

            if (!value.Any())
                return;

            var siteName = value.First();
            var site = iCmsServices.GetSiteByName(siteName).SingleOrDefault();
            if (site == null)
                return;
            if (!string.IsNullOrEmpty(site.IpFilter) && site.IpFilter.Split('|').Any(t => t == Client.GetAddress().Ipv4))
            {
                Response.StatusCode = 401;
                Response.End();
            }

            Action calcAccess = () =>
            {
                site.Access = site.Access + 1;
                iCmsServices.Update(site);
            };
            if (value.Count() == 1)
            {
                calcAccess();
                Response.Redirect($"~/{siteName}/{site.PageName}", true);
            }
            else
            {
                var name = value.ToList()[1];
                if (site.PageName == name)
                    calcAccess();
            }

        }
    }
}
using GS.App.Form.App_Start;
using GS.Common.Util;
using GS.SQLModel;
using MessageCilentProxy;
using Microsoft.Owin;
using Owin;
using System.Diagnostics;
using System.Web.Configuration;

[assembly: OwinStartupAttribute(typeof(GS.App.Form.Startup))]
namespace GS.App.Form
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var clientName = "formSite";
            app.RunConnectionMessageCilent(clientName, (new User() { Name = clientName, Id = -1 }).SerializeObject());
        }
    }
}

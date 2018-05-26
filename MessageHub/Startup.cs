using System.Diagnostics;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(MessageHub.Startup))]

namespace MessageHub
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {           
            app.Use<AuthMiddleware>();
            app.UseErrorPage();
            app.Map("/message", map =>
            {
                var config = new HubConfiguration
                {
                    EnableJSONP = true,
                    EnableJavaScriptProxies = true,                  
                };
                map.UseCors(CorsOptions.AllowAll)
                   .RunSignalR(config);
            });
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new UserIdProvider());
            GlobalHost.Configuration.DefaultMessageBufferSize = 2000;
            GlobalHost.TraceManager.Switch.Level = SourceLevels.Information;
        }
    }
}

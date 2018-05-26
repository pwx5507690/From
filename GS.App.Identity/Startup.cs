using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GS.App.Identity.Startup))]
namespace GS.App.Identity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
		}
    }
}

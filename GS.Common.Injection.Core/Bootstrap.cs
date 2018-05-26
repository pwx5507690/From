using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using GS.Common.Util;
using System.Web.Http;
using System.Web.Mvc;

namespace GS.Common.Injection.Core
{
    public static class Bootstrap
    {
        public static void ConfigureWebApi(IContainer container)
        {
            if (Config.ApiHost.IsNotNullOrEmpty())
                GlobalConfiguration.Configure(
                    httpConfiguration => 
                    httpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(container));
        }

        public static void ConfigureMvc(IContainer container)
        {
            if (Config.Controllers.IsNotNullOrEmpty())
                DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        public static void ConfigureApplication(IContainer container)
        {
            AssemblyUtil.ForceLoadAllReferencedAssemblies();
            AutofacConfig.Configure(container);
        }
    }
}

using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using GS.Common.Util;

namespace GS.Common.Injection.Core
{
    public class WebModuleRegistration : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            if (Config.IocAssembly.IsNotNullOrEmpty())
                Config.IocAssembly.Split(',').Each(t => builder
                .RegisterAssemblyTypes(AssemblyUtil.GetAllProjectAssemblies(t.ToString().Trim()))
                .AsImplementedInterfaces()
                .PropertiesAutowired());

            if (Config.ApiHost.IsNotNullOrEmpty())
            {
                var api = AssemblyUtil.GetAllProjectAssemblies(Config.ApiHost);
                if (api != null)
                    builder.RegisterApiControllers(api);
            }

            if (Config.Controllers.IsNotNullOrEmpty())
            {
                var mvc = AssemblyUtil.GetAllProjectAssemblies(Config.Controllers);
                if (mvc != null)
                    builder.RegisterControllers(mvc);
            }
            builder.RegisterFilterProvider();
        }
    }
}

using Autofac;
using GS.Common.Util;

namespace GS.Common.Injection.Core
{
    public class AutofacConfig
    {
        public static void Configure(IContainer container, string assemblyPrefix = "GS")
        {
            var builder = new ContainerBuilder();
            var assemblies = AssemblyUtil.GetAllProjectAssemblies(assemblyPrefix);
            builder.RegisterAssemblyModules(assemblies);
            builder.Update(container);
        }
    }
}

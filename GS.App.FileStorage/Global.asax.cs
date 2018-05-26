using Autofac;
using Autofac.Integration.Web;
using GS.App.FileStorage.Authentication;
using GS.Cache.Identity;
using GS.Common.Injection.Core;
using GS.Services;
namespace GS.App.FileStorage
{
	public class Global : IdentityApplication, IContainerProviderAccessor
	{
		private static IContainerProvider _containerProvider;
        private void Ioc()
		{
			var builder = new ContainerBuilder();
			builder.RegisterType<Account>();
            builder.Update(IoC.Container);

			_containerProvider = new ContainerProvider(IoC.Container);
		}
		public override void Start()
		{
			Ioc();
		}
		public IContainerProvider ContainerProvider
		{
			get { return _containerProvider; }
		}
		public override IdentitiyHandler GetIHttpHandler()
		{
			return _containerProvider.ApplicationContainer.Resolve<Account>();
		}

	}
}
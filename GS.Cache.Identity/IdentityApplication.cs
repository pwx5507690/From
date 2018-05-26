using GS.Cache.Factory;
using GS.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace GS.Cache.Identity
{
	public abstract class IdentityApplication : Application, IIdentityHandlerFactory
	{

		protected override void Application_Start()
		{
            base.Application_Start();
            RegisterIdentity();
            Start();           
        }

		public virtual void RegisterIdentity()
		{
			RouteTable.Routes.Add(new Route(IdentityConstant._identityTarget, new IdentityRouteHandler(this)));
		}

		public virtual void Start()
		{
			return;
		}

		public virtual IdentitiyHandler GetIHttpHandler()
		{
			return new IdentitiyHandler();
		}
	}
}

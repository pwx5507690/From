using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GS.Common.Util;
using System.Web.Configuration;
using GS.Common.Web;
using System.Collections.Specialized;
using System.Web.Routing;

namespace GS.Cache.Identity
{
	public interface IIdentityHandlerFactory
	{
		IdentitiyHandler GetIHttpHandler();
	}

	public class IdentityRouteHandler : IRouteHandler
	{
		public readonly IIdentityHandlerFactory _iIdentityHandlerFactory;
		public IdentityRouteHandler(IIdentityHandlerFactory iIdentityHandlerFactory)
		{
			_iIdentityHandlerFactory = iIdentityHandlerFactory;
		}
		public IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			return _iIdentityHandlerFactory.GetIHttpHandler();
		}
	}

	public class IdentitiyHandler : BaseIdentity, IHttpHandler
	{
		public virtual string Url
		{
			get
			{
				return _target;
			}
		}
		public string Token
		{
			get
			{
				return Request[_storageName];
			}
		}

		public virtual NameValueCollection FormParam
		{
			get
			{
				var loginUrl = Client.RootUrl + "/" + _identityTarget;
				return new NameValueCollection() {
				{ _loaction, Url},
				{ _loginUrl, loginUrl}
			};
			}
		}

		public virtual void Validate()
		{
			if (Token.IsNullOrEmpty())
                GoToLoginPage();

            SetClientToken(Token);

			Response.Redirect(Url);
			Response.End();
		}

		public virtual void GoToLoginPage()
		{
			Client.RedirectPost(_loginPath, FormParam);
		}

		public bool IsReusable
		{
			get { return false; }
		}

		public void ProcessRequest(HttpContext context)
		{
			Validate();
		}
	}
}

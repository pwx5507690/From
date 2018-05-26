using System;
using System.Web;
using GS.Common.Util;
using System.Runtime.Remoting.Messaging;

namespace GS.Common.Web
{
    public class Constant
	{
		public static HttpContext HttpContext
		{
			get
			{
				if (CallContext.HostContext.IsNull())
				{
					throw new Exception("HttpContext is not Init");
				}
				return (HttpContext)CallContext.HostContext;
			}
		}
		public static HttpResponse Response
		{
			get
			{
				return HttpContext.Response;
			}
		}
		public static HttpServerUtility Server
		{
			get
			{
				return HttpContext.Server;
			}
		}

		public static HttpRequest Request
		{
			get
			{
				return HttpContext.Request;
			}
		}
	}
}

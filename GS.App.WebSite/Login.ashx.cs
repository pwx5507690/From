using GS.Cache.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GS.App.WebSite
{
	/// <summary>
	/// Summary description for Handler1
	/// </summary>
	public class Login : IHttpHandler
	{
		private readonly string _locationTarget = "location";

		public void ProcessRequest(HttpContext context)
		{
			var request = context.Request;
			var response = context.Response;
			if (request.HttpMethod.ToUpper() != "POST")
			{
				response.Write("不支持此请求方式！！！");
				response.End();
				return;
			}
			var url = request[_locationTarget];
			var token = request[BaseIdentity._storageName];

			BaseIdentity.SetClientToken(token);
			context.Response.Redirect(url);
			context.Response.End();
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}
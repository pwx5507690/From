using GS.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GS.Common.Web
{
	public class Error : Constant, IHttpModule
	{
		public void Dispose()
		{

		}

		public void Init(HttpApplication context)
		{
			context.Error += (sender, e) =>
			{
				var ex = Server.GetLastError();
				var iex = ex.InnerException;

				LogUtil.ErrorFormat("{0} : {1}", DateTime.Now.ToString("yyyy-MM-dd"), iex.Message);
				Server.ClearError();
			};
		}
	}
}

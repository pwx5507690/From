using GS.Common.Injection.Core;
using GS.Common.Util;
using System;
using System.Diagnostics;
using System.Text;
using System.Web;

namespace GS.Common.Web
{
    public abstract class Application : HttpApplication
    {
        [Conditional("DEBUG")]
        private void WebLog(object sender, EventArgs e)
        {
            var url = Request.Url.ToString();
            if (url.ToLower().IndexOf("weblog") == -1)
                return;

            var name = System.IO.Path.GetFileNameWithoutExtension(url);
            var logInfo = LogUtil.WebLog(name);
            Response.ContentEncoding = Encoding.GetEncoding("gb2312");
            Response.Write(logInfo);
            Response.End();
        }
        protected virtual void Application_Start()
        {
            Bootstrap.ConfigureApplication(IoC.Container);
            Bootstrap.ConfigureMvc(IoC.Container);
            Bootstrap.ConfigureWebApi(IoC.Container);
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            WebLog(sender,e);
            BeforeRequest(sender, e);
        }
        public virtual void BeforeRequest(object sender, EventArgs e)
        {
            return;
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            var iex = ex.InnerException ?? ex;
            LogUtil.ErrorFormat("{0} : {1}--{2}", DateTime.Now.ToString("yyyy-MM-dd"), iex?.Message, iex?.StackTrace);
            Response.Write(iex?.Message + iex?.StackTrace);
            Response.End();
            Server.ClearError();
        }
    }
}

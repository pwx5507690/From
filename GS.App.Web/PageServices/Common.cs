using Autofac;
using GS.Common.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;

namespace GS.App.Web.PageServices
{
    public class Common
    {
        public static string SendGet(string name)
        {
            return ApiProxy.Send(GS.Common.Http.HttpMethod.GET, name);
        }
        public static string SendPost(string name, HttpContent content)
        {
            return ApiProxy.Send(GS.Common.Http.HttpMethod.POST, name, content);
        }
        public static string Send(GS.Common.Http.HttpMethod httpMethod, string name, HttpContent content = null)
        {
            return ApiProxy.Send(httpMethod, name, content);
        }
        public static string IncludeControl(string siteName, string controlName)
        {
            var html = string.Empty;
            var page = new Page();
            var output = new StringWriter();
            try
            {
                var ctrl = (UserControl)page.LoadControl($"~/{siteName}/control/{controlName}.ascx");
                using (var sw = new HtmlTextWriter(output))
                {
                    page.Controls.Add(ctrl);
                    page.RenderControl(sw);
                    html = sw.InnerWriter.ToString();
                }
            }
            catch
            {
                html = $"加载{controlName}控件出错";
            }
            output.Dispose();
            return html;
        }
    }
}
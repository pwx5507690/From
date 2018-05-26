using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using Autofac;
using System.Web.Services;
using System.Net.Http;

namespace GS.App.Web.PageServices
{
    public class App : Page
    {
        [WebMethod]
        public static string SendGet(string name)
        {
            return Common.SendGet(name);
        }
        [WebMethod]
        public static string SendPost(string name, string content)
        {
            return Common.SendPost(name,new StringContent(content));
        }
    }
}
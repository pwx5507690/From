using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace GS.Common.Injection.Core
{
    class Config
    {
        public readonly static string IocAssembly = WebConfigurationManager.AppSettings["IocAssembly"];
        public readonly static string ApiHost = WebConfigurationManager.AppSettings["ApiHost"];
        public readonly static string Controllers = WebConfigurationManager.AppSettings["Controllers"];
    }
}

using GS.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace GS.Cache.Identity
{

	public class IdentityConstant : Constant
	{

        public static readonly string _messageToken = "KEV1245EWQPOIUYT";

        public static readonly string _hashKey = "KEV1245EWQPOIUYT";

		public static readonly int _defaultInterval = 30;

		public static readonly string _storageName = "userCache";

		public static readonly string _loaction = "loaction";

		public static readonly string _identityTarget = "Auth";

		public static readonly string _loginUrl = "loginUrl";

		public static readonly string _target = WebConfigurationManager.AppSettings["Location"];

		public static readonly string _loginPath = WebConfigurationManager.AppSettings["LoginPath"];

		public static readonly string _interval = WebConfigurationManager.AppSettings["Interval"];
	}
}

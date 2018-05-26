using GS.Cache.Identity;
using GS.Common.Util;
using GS.SQLModel;
using GS.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Routing;

namespace GS.App.FileStorage.Authentication
{
	public class Account : IdentitiyHandler
	{
		public string LoginPage
		{
			get
			{
				return WebConfigurationManager.AppSettings["LoginUrl"];
			}
		}

		public IdentityModel<User> User
		{
			get
			{
				return GetUser<User>();
			}
		}

		public override string Url
		{
			get
			{
				var selectedType = Client.GetCookieValue(Model.Constant.SELECTED_TYPE_KEY);
				var type = selectedType.IsNotNullOrEmpty() ? $"?type={selectedType}" : string.Empty;
				return WebConfigurationManager.AppSettings["DefaultPage"] + type;
			}
		}
	}
}
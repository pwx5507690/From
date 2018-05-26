using GS.App.FileStorage.Authentication;
using GS.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.App.FileStorage
{
	public partial class Index : System.Web.UI.Page
	{
		public string SelectType { get; set; }
		public Account _account { get; set; }
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(Request["token"]))
			{
				Account.SetClientToken(Request["token"]);
			}
			if (!Page.IsPostBack)
			{
				if (_account.User == null)
				{
					Response.Redirect("~/Auth");
				}
			}
			SelectType = Request["type"];
			Client.SetCookie(Model.Constant.SELECTED_TYPE_KEY, SelectType);
		}
	}
}
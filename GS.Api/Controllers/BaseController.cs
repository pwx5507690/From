using GS.Api.Attributes;
using GS.Api.Constants;
using GS.Cache.Identity;
using GS.SQLModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;

namespace GS.Api.Controllers
{
	[Authentication]
	public class BaseController : ApiController
	{
	
		public User CurrentUser
		{
			get
			{
				var token = Request.Headers.Authorization.ToString().Replace(Config._authorizationTarget + " ", "");
				var user = (BaseIdentity.GetIdentity<User>(token)).Model;
				return user;
			}
		}
	}
}

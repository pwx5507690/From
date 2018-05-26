using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GS.Common.Util;
using System.Collections.Specialized;

namespace GS.Common.Web
{
	public class ClientInfo
	{
		public string Host { get; set; }
		public string Ipv4 { get; set; }
	}
	public class Client : Constant
	{
        public static byte[] GetData(string url)
		{
			return new WebClient().DownloadData(url);
		}
		public static ClientInfo GetAddress()
		{
			string ip = string.Empty;
			string ipv4 = string.Empty;
			var resuqest = Request;
			if (resuqest.ServerVariables["HTTP_VIA"].IsNotNullOrEmpty())
                ip = Convert.ToString(resuqest.ServerVariables["HTTP_X_FORWARDED_FOR"]);

            if (ip.IsNullOrEmpty())
                ip = resuqest.UserHostAddress;

            foreach (IPAddress ipAddr in Dns.GetHostEntry(ip).AddressList)
			{
				if (ipAddr.AddressFamily.ToString() == "InterNetwork")
                    ipv4 = ipAddr.ToString();
            }

			return new ClientInfo
			{
				Host = Dns.GetHostEntry(ip).HostName + " IP: " + ipv4,
				Ipv4 = ipv4
			};
		}
		public static void ClearCookie(string cookiename)
		{
			var cookie = Constant.Request.Cookies[cookiename];
			if (cookie.IsNotNull())
			{
				cookie.Expires = DateTime.Now.AddYears(-3);
				Response.Cookies.Add(cookie);
			}
		}
		public static string GetCookieValue(string cookiename)
		{
			var cookie = Request.Cookies[cookiename];
			string str = string.Empty;
			if (cookie.IsNotNull())
                str = cookie.Value;

            return str;
		}
		public static void SetCookie(string cookiename, string cookievalue)
		{
			SetCookie(cookiename, cookievalue, DateTime.Now.AddDays(1.0));
		}
		public static string RootUrl
		{
			get
			{
				string appPath = string.Empty;
				if (Request.IsNotNull())
				{
					string rurlAuthority = Request.Url.GetLeftPart(UriPartial.Authority);
					if (Request.ApplicationPath == null || Request.ApplicationPath == "/")
						appPath = rurlAuthority;
					else
						appPath = rurlAuthority + Request.ApplicationPath;
				}
				return appPath;
			}
		}
		public static void SetCookie(string cookiename, string cookievalue, DateTime expires, string domain = null)
		{
			var cookie = new HttpCookie(cookiename);
			cookie.Value = cookievalue;
			cookie.Expires = expires;

			if (domain.IsNotNullOrEmpty())
                cookie.Domain = domain;

            Response.Cookies.Add(cookie);
		}
		public static void RedirectPost(string url, NameValueCollection data)
		{
			Response.ContentType = "text/html";
			string formID = "PostForm";
			var strForm = new StringBuilder();
			strForm.Append($"<form id='{formID}' name='{formID}' action='{url}' method='POST'>");
			foreach (string key in data)
                strForm.Append($"<input type='hidden' name='{key}' value='{data[key]}'>");
            strForm.Append("</form>");
			var strScript = new StringBuilder();
			strScript.Append("<script language='javascript'>");
			strScript.Append($"var v{formID} = document.{formID};");
			strScript.Append($"v{formID}.submit();");
			strScript.Append("</script>");
			Response.Clear();
			Response.Write(strForm.ToString() + strScript.ToString());
			Response.End();
		}
	}
}

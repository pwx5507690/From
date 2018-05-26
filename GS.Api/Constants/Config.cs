using System.Web.Configuration;

namespace GS.Api.Constants
{
    public class Config
	{
		public static readonly string _authorizationTarget;
		public static readonly string _authorizationHeadKey;
		static Config()
		{
			_authorizationTarget = GetValue("AuthorizationTarget");
			_authorizationHeadKey = "Authorization";
		}
        public static string GetValue(string name)
		{
			return WebConfigurationManager.AppSettings[name];
		}
	}
}
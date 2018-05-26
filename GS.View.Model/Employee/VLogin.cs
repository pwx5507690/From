using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GS.SQLModel;
using Newtonsoft.Json;

namespace GS.View.Model
{
	public enum LoginStats {
		NAME,
		PASSOWRD,
		SUCCESS
	}
    public class Login
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }
        [JsonProperty(PropertyName = "rememberMe")]
        public int RememberMe { get; set; }
        [JsonProperty(PropertyName = "isLogin")]
        public bool IsLogin { get; set; }
        [JsonProperty(PropertyName = "sessionId")]
        public string SessionId { get; set; }
        [JsonProperty(PropertyName = "loginUrl")]
        public string LoginUrl { get; set; }
    }
    public class VLogin
	{
		public User User { get; set; }
		public bool IsLogin { get; set; }
		public string SessionId { get; set; }
		public LoginStats LoginStats { get; set; }
	}
}

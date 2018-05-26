using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GS.Api.Tool.Models
{
	public class ApiRequestParamModels
	{
		[JsonProperty("url")]
		public string Url { get; set; }

		[JsonProperty("heardParam")]
		public string HeardParam { get; set; }

		[JsonProperty("heardParamValue")]
		public string HeardParamValue { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("paramType")]
		public string ParamType { get; set; }

		[JsonProperty("paramValue")]
		public string ParamValue { get; set; }
	}
}
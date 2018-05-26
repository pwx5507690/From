using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.View.Model
{
	public class VResponseFileResult
	{
		[JsonProperty(PropertyName = "isSuccess")]
		public bool IsSuccess { get; set; }

		[JsonProperty(PropertyName = "saveUrl")]
		public string SaveUrl { get; set; }

		[JsonProperty(PropertyName = "message")]
		public string Message { get; set; }
	}
}

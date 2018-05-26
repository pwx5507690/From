using GS.SQLModel.Form;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GS.App.Form.Models
{
	public class FormViewModel
	{
		[JsonProperty(PropertyName = "formData")]
		public string FormData { get; set; }
		[JsonProperty(PropertyName = "parameterData")]
		public string DyncField { get; set; }
		[JsonProperty(PropertyName = "optionType")]
		public int OptionType { get; set; }
	}
}
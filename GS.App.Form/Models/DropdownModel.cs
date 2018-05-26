using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GS.App.Form.Models
{
    public class DropdownControlModel
    {
        public IEnumerable<SelectListItem> Item { get; set; }
        public IDictionary<string, string> HtmlAttributes { get; set; }
    }
	public class DropdownModel
	{
		[JsonProperty(PropertyName = "CHKED")]
		public string Checked { get; set; }
		[JsonProperty(PropertyName = "VAL")]
		public string value { get; set; }
        [JsonProperty(PropertyName = "ITMS")]
        public IEnumerable<Dictionary<string, string>> Itms { get; set; }
    }
}
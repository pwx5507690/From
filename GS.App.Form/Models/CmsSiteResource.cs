using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GS.App.Form.Models
{
    public class CmsSiteResource
    {
        [JsonProperty(PropertyName = "siteId")]
        public int SiteId { get; set; }
        [JsonProperty(PropertyName = "resourcePath")]
        public List<string> ResourcePath { get; set; }
    }
}
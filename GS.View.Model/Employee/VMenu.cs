using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GS.SQLModel;
namespace GS.View.Model
{
	public class VMenu
	{
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
		public string Name { get; set; }
		[JsonProperty(PropertyName = "url")]
		public string Url { get; set; }
		[JsonProperty(PropertyName = "type")]
		public string Type { get; set; }
		[JsonProperty(PropertyName = "eName")]
		public string EName { get; set; }
		[JsonProperty(PropertyName = "menu")]
		public IEnumerable<Menu> Menu { get; set; }
		
	}
}

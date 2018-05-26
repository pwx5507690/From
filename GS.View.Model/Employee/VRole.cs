using GS.SQLModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.View.Model
{

	public class VRole
	{
		[JsonProperty(PropertyName = "id")]
		public int Id { get; set; }
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }
        [JsonProperty(PropertyName = "menuId")]
		public IEnumerable<int> MenuId { get; set; }
		[JsonProperty(PropertyName = "role")]
		public SQL.DataSource.SQLPage<Role> Role { get; set; }
		[JsonProperty(PropertyName = "menuRole")]
		public IEnumerable<MenuRole> MenuRole { get; set; }
	}
}

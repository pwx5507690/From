using GS.SQL.DataSource;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.SQLModel
{
	public class Menu : Base<Menu>
	{
		[JsonProperty(PropertyName = "name")]
		[Column("Name", DbType.String)]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "url")]
		[Column("Url", DbType.String)]
		public string Url { get; set; }

		[JsonProperty(PropertyName = "parent")]
		[Column("Parent", DbType.Int32)]
		public int? Parent { get; set; }

		[JsonProperty(PropertyName = "icon")]
		[Column("Icon", DbType.String)]
		public string Icon { get; set; }

		[JsonProperty(PropertyName = "parentName")]
		[Column("ParentName", DbType.String)]
		public string ParentName { get; set; }

		[JsonProperty(PropertyName = "isGroup")]
		[Column("IsGroup", DbType.Boolean)]
		public bool IsGroup { get; set; }

		[JsonProperty(PropertyName = "sort")]
		[Column("Sort", DbType.Int32)]
		public int Sort { get; set; }

		[JsonProperty(PropertyName = "type")]
		[Column("Type", DbType.String)]
		public string Type { get; set; }
	}
}

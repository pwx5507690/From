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
	public class Folder : FileManager<Folder>
	{
		[JsonProperty(PropertyName = "parent")]
		[ColumnAttribute("Parent", DbType.Int32)]
		public int Parent { get; set; }

		[JsonProperty(PropertyName = "parentName")]
		[ColumnAttribute("ParentName", DbType.String)]
		public string ParentName { get; set; }

		[JsonProperty(PropertyName = "group")]
		[ColumnAttribute("Group", DbType.Int32)]
		public string Group { get; set; }
	}
}

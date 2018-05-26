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
	public abstract class FileManager<T> : Base<T>
	{
		public FileManager() : base("FileManager")
		{

		}
		[JsonProperty(PropertyName = "createtime")]
		[ColumnAttribute("Createtime", DbType.DateTime)]
		public DateTime? Createtime { get; set; }

		[JsonProperty(PropertyName = "name")]
		[ColumnAttribute("Name", DbType.String)]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "role")]
		[ColumnAttribute("Role", DbType.String)]
		public string Role { get; set; }

		[JsonProperty(PropertyName = "user")]
		[ColumnAttribute("User", DbType.Int32)]
		public int User { get; set; }

		[JsonProperty(PropertyName = "url")]
		[ColumnAttribute("Url", DbType.String)]
		public string Url { get; set; }

		[JsonProperty(PropertyName = "type")]
		[ColumnAttribute("Type", DbType.String)]
		public string Type { get; set; }
	}
}

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
	public class Department : Base<Department>
	{
		[JsonProperty(PropertyName = "code")]
		[Column("Code", DbType.String)]
		public string Code { get; set; }

		[JsonProperty(PropertyName = "name")]
		[Column("Name", DbType.String)]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "parent")]
		[Column("Parent", DbType.Int32)]
		public int Parent { get; set; }

		[JsonProperty(PropertyName = "parentName")]
		[Column("ParentName", DbType.String)]
		public string ParentName { get; set; }
	}
}

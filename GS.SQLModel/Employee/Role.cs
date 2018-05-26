using GS.SQL.DataSource;
using Newtonsoft.Json;
using System.Data;

namespace GS.SQLModel
{
    public class Role : Base<Role>
	{
		[JsonProperty(PropertyName = "name")]
		[Column("Name", DbType.String)]
		public string Name { get; set; }
		[JsonProperty(PropertyName = "code")]
		[Column("Code", DbType.String)]
		public string Code { get; set; }
	}
}

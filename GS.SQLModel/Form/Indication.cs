using GS.SQL.DataSource;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.SQLModel.Form
{
	public class Indication : DyncBaseForm<Indication>
	{
		[JsonProperty(PropertyName = "targetName")]
		[Column("TargetName", DbType.String)]
		public string TargetName { get; set; }

		[JsonProperty(PropertyName = "targetType")]
		[Column("TargetType", DbType.String)]
		public string TargetType { get; set; }
	}
}

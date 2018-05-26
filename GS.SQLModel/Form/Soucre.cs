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
	public class Soucre : DyncBaseForm<Soucre>
	{
		[JsonProperty(PropertyName = "targetType")]
		[Column("TargetType", DbType.String)]
		public SoucreType SoucreType { get; set; }

		[JsonProperty(PropertyName = "action")]
		[Column("Action", DbType.String)]
		public string Action { get; set; }
	}
}

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
	public class UserRole : Base<UserRole>
	{

		[JsonProperty(PropertyName = "role")]
		[Column("Role", DbType.Int32)]
		public int Role { get; set; }

		[JsonProperty(PropertyName = "roleName")]
		[Column("RoleName", DbType.String)]
		public string RoleName { get; set; }

		[JsonProperty(PropertyName = "user")]
		[Column("User", DbType.Int32)]
		public int User { get; set; }
	}
}

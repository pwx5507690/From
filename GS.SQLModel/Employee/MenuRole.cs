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
	public class MenuRole : Base<MenuRole>
	{
		[JsonProperty(PropertyName = "role")]
		[Column("Role", DbType.Int32)]
		public int Role { get; set; }
	
		[JsonProperty(PropertyName = "menu")]
		[Column("Menu", DbType.Int32)]
		public int Menu { get; set; }

		[JsonProperty(PropertyName = "menuName")]
		[Column("MenuName", DbType.String)]
		public string MenuName { get; set; }
	}
}

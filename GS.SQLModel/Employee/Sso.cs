using GS.SQL.DataSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.SQLModel
{
	public class Sso: Base<Sso>
	{
		[Column("Location", DbType.String)]
		public string Location { get; set; }
		[Column("Key", DbType.String)]
		public string Key { get; set; }
	}
}

using GS.SQL.DataSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.SQLModel
{
	public class Login : Base<Login>
	{
		[Column("IPAddress", DbType.String)]
		public string IPAddress { get; set; }
		[Column("LoginTime", DbType.DateTime)]
		public DateTime LoginTime { get; set; }
		[Column("SessionId", DbType.String)]
		public string SessionId { get; set; }
		[Column("UserName", DbType.String)]
		public string UserName { get; set; }
		[Column("UserId", DbType.Int32)]
		public int UserId { get; set; }
		[Column("IsDrop", DbType.Int32)]
		public int IsDrop { get; set; }
	}
}

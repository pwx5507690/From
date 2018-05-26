using System.Data;

namespace GS.SQL.DataSource
{
    public class SQLEnityFiled
	{
		public string ColName { get; set; }

		public string EntName { get; set; }

		public string Value { get; set; }

		public DbType Type { get; set; }

		public bool Key { get; set; }

		public bool IsIdent { get; set; }

	}
}

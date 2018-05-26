using System.Collections.Generic;

namespace GS.SQL.DataSource
{
    public class SQLDynamicRow
	{
		public IEnumerable<SQLDynamicItem> Row { get; set; }
		public string Code { get; set; }
		public int DataId { get; set; }
	}
	public class SQLDynamicItem
	{
		public string Name { get; set; }
		public string Value { get; set; }
		public string Type { get; set; }
	}
}

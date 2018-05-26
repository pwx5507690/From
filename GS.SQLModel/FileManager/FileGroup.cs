using GS.SQL.DataSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.SQLModel
{
	public class FileGroup : Base<FileGroup>
	{
		[ColumnAttribute("Name", DbType.String)]
		public string Name { get; set; }
	}
}

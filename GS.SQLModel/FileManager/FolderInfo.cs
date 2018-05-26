using GS.SQL.DataSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.SQLModel.FileManager
{
	public class Properties
	{
		public DateTime? DateCreated { get; set; }
		public DateTime? DateModified { get; set; }
		public int Height { get; set; }
		public int Width { get; set; }
		public int Size { get; set; }
	}
	public class FolderInfo : FileManager<FolderInfo>
	{
		[ColumnAttribute("Path", DbType.String)]
		public string Path { get; set; }
		[ColumnAttribute("Filename", DbType.String)]
		public string Filename { get; set; }
		[ColumnAttribute("FileType", DbType.String)]
		public string FileType { get; set; }
		[ColumnAttribute("Preview", DbType.String)]
		public string Preview { get; set; }
		[ColumnAttribute("Error", DbType.String)]
		public string Error { get; set; }
		[ColumnAttribute("Code", DbType.String)]
		public string Code { get; set; }
		public Properties Properties { get; set; }
		[ColumnAttribute("ParentId", DbType.Int32)]
		public int ParentId { get; set; }
		[ColumnAttribute("Size", DbType.Int32)]
		public int Size { get; set; }
	}
}

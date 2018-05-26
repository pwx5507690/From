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
	public class File:FileManager<File>
	{
		[JsonProperty(PropertyName = "mark")]
		[ColumnAttribute("Mark", DbType.String)]
		public string Mark { get; set; }

		[JsonProperty(PropertyName = "fileType")]
		[ColumnAttribute("FileType", DbType.String)]
		public string FileType { get; set; }

		[JsonProperty(PropertyName = "folder")]
		[ColumnAttribute("Folder", DbType.Int32)]
		public int Folder { get; set; }

		[JsonProperty(PropertyName = "size")]
		[ColumnAttribute("Size", DbType.Int32)]
		public long Size { get; set; }

		[JsonProperty(PropertyName = "thumbnail")]
		[ColumnAttribute("Thumbnail", DbType.String)]
		public string Thumbnail { get; set; }
	}
}

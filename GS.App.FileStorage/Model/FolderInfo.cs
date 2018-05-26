using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GS.App.FileStorage.Model
{
	public class FileManageResponse
	{
		[JsonProperty(PropertyName = "Error")]
		public string Error { get; set; }
		[JsonProperty(PropertyName = "Code")]
		public int Code { get; set; }
		[JsonProperty(PropertyName = "Old Path")]
		public string OldPath { get; set; }
		[JsonProperty(PropertyName = "Old Name")]
		public string OldName { get; set; }
		[JsonProperty(PropertyName = "New Name")]
		public string NewName { get; set; }
		[JsonProperty(PropertyName = "Path")]
		public string Path { get; set; }
		[JsonProperty(PropertyName = "Name")]
		public string Name { get; set; }
		[JsonProperty(PropertyName = "New Path")]
		public string NewPath { get; set; }
		[JsonProperty(PropertyName = "Parent")]
		public int Parent { get; set; }
		[JsonProperty(PropertyName = "Id")]
		public int Id { get; set; }
	}
	public class Properties
	{
		public Properties() {
			DateCreated = string.Empty;
			DateModified = string.Empty;
		}
		[JsonProperty(PropertyName = "Date Created")]
		public string DateCreated { get; set; }
		[JsonProperty(PropertyName = "Date Modified")]
		public string DateModified { get; set; }
		[JsonProperty(PropertyName = "Height")]
		public int Height { get; set; }
		[JsonProperty(PropertyName = "Width")]
		public int Width { get; set; }
		[JsonProperty(PropertyName = "Size")]
		public long Size { get; set; }

	}
	public class FolderInfo
	{
		public FolderInfo()
		{
			Filename = string.Empty;
			FileType = string.Empty;
			Preview = string.Empty;
			Error = string.Empty;
			Error = string.Empty;
			Path = "-1";
		}
		public static FolderInfo Empty()
		{
			return new FolderInfo()
			{
				Properties = new Properties()
			};
		}
		public string Parent { get; set; }
		[JsonProperty(PropertyName = "Properties")]
		public Properties Properties { get; set; }
		[JsonProperty(PropertyName = "Path")]
		public string Path { get; set; }
		[JsonProperty(PropertyName = "Filename")]
		public string Filename { get; set; }
		[JsonProperty(PropertyName = "File Type")]
		public string FileType { get; set; }
		[JsonProperty(PropertyName = "Preview")]
		public string Preview { get; set; }
		[JsonProperty(PropertyName = "Error")]
		public string Error { get; set; }
		[JsonProperty(PropertyName = "Code")]
		public int Code { get; set; }
	}
}
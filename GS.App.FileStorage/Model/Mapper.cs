using GS.Common.Util;
using GS.Common.Web;
using GS.SQLModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using MFile = GS.SQLModel.File;

namespace GS.App.FileStorage.Model
{
	public class Mapper
	{
		public static string IconDirectory = "./images/fileicons/";
		public static Properties GetFileProperties(FileInfo info)
		{
			var prop = new Properties();
			prop.Size = info.Length;
			prop.DateCreated = info.CreationTime.ToString();
			prop.DateModified = info.LastWriteTime.ToString();

			if (ImageUtil.IsImage(info))
			{
				using (System.Drawing.Image img = System.Drawing.Image.FromFile(info.FullName))
				{
					prop.Width = img.Width;
					prop.Height = img.Height;
				}
			}
			return prop;
		}

		public static string GetFileType(FileInfo info)
		{
			return info.Extension.Replace(".", "");
		}

		private static string GetPreview(bool IsFile, FileInfo info = null, MFile file = null)
		{
			if (!IsFile)
				return IconDirectory + "_Open.png";
			return ImageUtil.IsImage(info) ? file.Url : string.Format("{0}{1}.png", IconDirectory, GetFileType(info));
		}

		public static FolderInfo ConvertToFolderInfo(MFile file)
		{
			var fileInfo = new FileInfo(Constant.Server.MapPath(file.Url));
			var folder = new FolderInfo()
			{
				Path = file.Id.ToString(),
				Filename = fileInfo.Name,
				FileType = GetFileType(fileInfo),
				Properties = GetFileProperties(fileInfo),
				Preview = GetPreview(true, fileInfo, file)
			};
			return folder;
		}

		public static FolderInfo ConvertToFolderInfo(Folder folder)
		{
			return new FolderInfo()
			{
				Parent = folder.Parent.ToString(),
				Path = folder.Id.ToString(),
				Filename = folder.Name,
				FileType = "dir",
				Preview = GetPreview(false),
				Properties = new Properties()
				{
					DateCreated = folder.Createtime.ToString(),
					DateModified = folder.Updatetime.ToString()
				}
			};
		}
	}
}
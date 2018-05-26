using System.Collections.Generic;
using System;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using GS.Services;
using GS.SQLModel;
using GS.Common.Util;
using GS.App.FileStorage.Model;
using MFile = GS.SQLModel.File;
using SFile = System.IO.File;
using Constant = GS.App.FileStorage.Model.Constant;
using GS.App.FileStorage.Authentication;
using GS.Common.Web;

namespace GS.App.FileStorage.Server
{
	public class Filemanager : IHttpHandler
	{
		public IFileServices _fileServices { get; set; }
		public IRoleServices _roleServices { get; set; }
		public Account _account { get; set; }

		private readonly string _folderType = "DIR";

		private IEnumerable<UserRole> GetRoleByUser(int id)
		{
			return _roleServices.GetUserRoleByUserId(id);
		}

		private IEnumerable<Folder> GetFolderByUser(int id, int folder)
		{
			var role = GetRoleByUser(id).Select(t => t.Role);
			return _fileServices.GetFolderByParent(folder, string.Join(",", role));
		}

		private IEnumerable<MFile> GetFileByfolder(int id, int folder)
		{
			var role = GetRoleByUser(id).Select(t => t.Role);
			return _fileServices.GetFileByFolder(folder, string.Join(",", role));
		}

		private IEnumerable<MFile> GetFileByRole(int id)
		{
			var role = GetRoleByUser(id).Select(t => t.Role);
			return _fileServices.GetFileByRole(string.Join(",", role));
		}

		private IEnumerable<FolderInfo> GetFile(IEnumerable<MFile> file)
		{
			return file.Select(t =>
			{
				return Mapper.ConvertToFolderInfo(t);
			});
		}

		private IEnumerable<FolderInfo> GetFolderInfo(IEnumerable<Folder> folder)
		{
			return folder.Select(t =>
			{
				return Mapper.ConvertToFolderInfo(t);
			});
		}

		private string GetRole()
		{
			var role = GetRoleByUser(User).Select(t => t.Role);
			return role.IsNull() ? "-1" : string.Join(",", role);
		}

		public int User
		{
			get
			{
				return _account.User.Model.Id;
			}
		}

		public string RoolFolder
		{
			get
			{
				return _account.User.Model.Name + User.ToString();
			}
		}

		public int Folder
		{
			get
			{
				var path = Constant.Request["path"];
				if (path.IsNullOrEmpty())
				{
					return -1;
				}
				return Convert.ToInt32(path);
			}
		}

		public string FileType
		{
			get
			{
				return Constant.Request["fileType"];
			}
		}

		public string NewName
		{
			get
			{
				return Constant.Request["newName"];
			}
		}

		public string FolderName
		{
			get
			{
				return HttpUtility.UrlDecode(Constant.Request["name"]);
			}
		}

		public string Cmd
		{
			get
			{
				return Constant.Request["mode"];
			}
		}

		public string GetFolderInfo()
		{
			var selectedType = Constant.Request["selectedType"];
			if (selectedType.IsNullOrEmpty())
			{
				var folderInfo = GetFolderInfo(GetFolderByUser(User, Folder))
												.Concat(GetFile(GetFileByfolder(User, Folder))).ToList();
				return folderInfo.SerializeObject();
			}
			else
			{
				return GetFile(GetFileByRole(User)).SerializeObject();
			}

		}

		public string GetInfo()
		{
			var role = GetRoleByUser(User).Select(t => t.Role);
			if (role.IsNull())
			{
				Unauthorized();
				return null;
			}
			var strRole = string.Join(",", role);
			if (FileType == _folderType)
			{
				var folder = _fileServices.GetFolderById(Folder);
				if (folder.IsNull())
				{
					return FolderInfo.Empty().SerializeObject();
				}
				if (!folder.Role.Contains(strRole))
				{
					Unauthorized();
					return null;
				}

				return Mapper.ConvertToFolderInfo(folder).SerializeObject();
			}
			else
			{
				var file = _fileServices.GetFileById(Folder);
				if (file.IsNull())
				{
					return FolderInfo.Empty().SerializeObject();
				}
				if (!file.Role.Contains(strRole))
				{
					Unauthorized();
					return string.Empty;
				}
				return Mapper.ConvertToFolderInfo(file).SerializeObject();
			}

		}

		public void SetFile<T>(FileManager<T> fileManager)
		{
			fileManager.Name = NewName;
			var name = fileManager.Url.Split('\\');
			fileManager.Url = Path.GetDirectoryName(fileManager.Url) + "\\" + fileManager.Name;
			//fileManager.Url.Substring(0, 
			//(fileManager.Url.Length - fileManager.Url.LastIndexOf("\\")) - 1) + fileManager.Name;
		}

		public Folder UpdateFolder(ref string old)
		{
			var item = _fileServices.GetFolderById(Folder);
			old = item.Url;
			SetFile<Folder>(item);
			_fileServices.UpdateFolder(item);
			return item;
		}

		public MFile UpdateFile(ref string old)
		{
			var item = _fileServices.GetFileById(Folder);
			old = item.Url;
			SetFile<MFile>(item);

			_fileServices.UpdateFile(item);
			return item;
		}

		public string Rename()
		{
			var fileManageResponse = new FileManageResponse();

			fileManageResponse.NewName = NewName;
			var old = string.Empty;
			if (FileType == _folderType)
			{
				var count = _fileServices.GetFolderByName(Folder, NewName).Count();
				if (count > 0)
				{
					fileManageResponse.Code = 1;
					fileManageResponse.Error = NewName + " 已经存在";
					return fileManageResponse.SerializeObject();
				}
				var url = UpdateFolder(ref old).Url;
				fileManageResponse.NewPath = url;
				var dirInfo = new DirectoryInfo(Constant.Server.MapPath("~/client/" + old));
				Directory.Move(Constant.Server.MapPath("~/client/" + old), Path.Combine(dirInfo.Parent.FullName, NewName));
				var fileInfoNew = new DirectoryInfo(Path.Combine(dirInfo.Parent.FullName, NewName));
			}
			else
			{
				var count = _fileServices.GetFileByName(Folder, NewName).Count();
				if (count > 0)
				{
					fileManageResponse.Code = 1;
					fileManageResponse.Error = NewName + " 已经存在";
					return fileManageResponse.SerializeObject();
				}
				var url = UpdateFile(ref old).Url;
				fileManageResponse.NewPath = url;
				var oldPath = Constant.Server.MapPath(old);
				var fileInfo = new FileInfo(oldPath);
				SFile.Move(oldPath, Path.Combine(fileInfo.Directory.FullName, NewName));
				var fileInfoNew = new FileInfo(Path.Combine(fileInfo.Directory.FullName, NewName));
			}
			fileManageResponse.OldName = Path.GetFileName(old);
			fileManageResponse.OldPath = Folder.ToString();
			fileManageResponse.Error = "No error";
			return fileManageResponse.SerializeObject();
		}

		public Folder DeleteFolder(int id)
		{
			var folder = _fileServices.GetFolderById(id);
			Directory.Delete(Constant.Server.MapPath("~/client/" + folder.Url), true);
			_fileServices.DeleteFolder(id);
			return folder;
		}

		public MFile DeleteFile(int id)
		{
			var file = _fileServices.GetFileById(id);
			SFile.Delete(Constant.Server.MapPath(file.Url));
			_fileServices.DeleteFile(id);
			return file;
		}

		public string Delete()
		{
			FileManageResponse folderInfo = new FileManageResponse()
			{
				Error = "No error",
				Code = 0
			};
			if (FileType.ToUpper() == _folderType)
			{
				var folder = DeleteFolder(Folder);
				folderInfo.Parent = folder.Parent;
				folderInfo.Path = folder.Id.ToString();
			}
			else
			{
				var file = DeleteFile(Folder);
				folderInfo.Parent = file.Folder;
				folderInfo.Path = file.Id.ToString();
			}

			return folderInfo.SerializeObject();
		}

		public string AddFolder()
		{
			var count = _fileServices.GetFolderByName(Folder, FolderName).Count();
			if (count > 0)
			{
				return new FileManageResponse()
				{
					Error = FolderName + "已经存在",
					Code = 1,
				}.SerializeObject();
			}

			var folder = _fileServices.GetFolderById(Folder);
			var url = string.Empty;

			if (folder.IsNull())
			{
				var path = Constant.Server.MapPath("~/client/" + RoolFolder);
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				url = Path.Combine(RoolFolder, FolderName);
			}
			else
			{
				url = Path.Combine(folder.Url, FolderName);
			}

			var newfolder = new Folder()
			{
				Url = url,
				Createtime = DateTime.UtcNow,
				Updatetime = DateTime.UtcNow,
				User = User,
				Name = FolderName,
				Type = _folderType,
				Role = GetRole(),
				Parent = Folder
			};

			var id = _fileServices.AddFolder(newfolder);

			Directory.CreateDirectory(Constant.Server.MapPath("~/client/" + newfolder.Url));
			return new FileManageResponse()
			{
				Path = newfolder.Parent.ToString(),
				Name = FolderName,
				Error = "No error",

				Code = 0,
				Id = id,
				Parent = newfolder.Parent
			}.SerializeObject();
		}

		public void Add()
		{
			var file = Constant.Request.Files[0];
			string path = Constant.Request["currentpath"];
			var parant = -1;
			if (path.IsNotNullOrEmpty())
			{
				parant = Convert.ToInt32(path);
			}

			if (parant == -1)
			{
				path = "~/client/" + RoolFolder;

			}
			else
			{
				path = "~/client/" + _fileServices.GetFolderById(parant).Url;
			}
			var count = _fileServices.GetFileByName(Folder, file.FileName).Count();
			var name = file.FileName;
			if (count > 0)
			{
				name = name + "(" + (count + 1) + ")";
			}
			var saveUrl = Constant.Server.MapPath(path) + "\\" + name;
			file.SaveAs(saveUrl);

			var mFile = new MFile();

			mFile.Url = path + "\\" + name;
			mFile.Name = name;
			mFile.Createtime = DateTime.UtcNow;
			mFile.Updatetime = DateTime.UtcNow;
			mFile.Folder = parant;
			mFile.User = User;
			mFile.Role = GetRole();
			mFile.FileType = Path.GetExtension(name).Replace(".", "").ToLower();

			var info = new FileInfo(saveUrl);
			mFile.Size = info.Length;

			_fileServices.AddFile(mFile);

			var response = new FileManageResponse()
			{
				Error = "No error",
				Name = Path.GetFileName(name),
				Path = parant.ToString()
			}.SerializeObject();

			Constant.Response.ContentType = "text/html";
			Constant.Response.ContentEncoding = Encoding.UTF8;

			System.Web.UI.WebControls.TextBox txt = new System.Web.UI.WebControls.TextBox();
			txt.TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine;
			txt.Text = response;

			StringWriter sw = new StringWriter();
			System.Web.UI.HtmlTextWriter writer = new System.Web.UI.HtmlTextWriter(sw);
			txt.RenderControl(writer);

			Constant.Response.Write(sw.ToString()
			);
		}

		public void Download()
		{
			var fi = new FileInfo(Constant.Server.MapPath(Constant.Request["path"]));
			Constant.Response.AddHeader("Content-Disposition", "attachment; filename=" + Constant.Server.UrlPathEncode(fi.Name));
			Constant.Response.AddHeader("Content-Length", fi.Length.ToString());
			Constant.Response.ContentType = "application/octet-stream";
			Constant.Response.TransmitFile(fi.FullName);
		}

		public void Unauthorized()
		{
			Constant.Response.StatusCode = 401;
			Constant.Response.End();
		}

		public void ProcessRequest(HttpContext context)
		{
			if (_account.User.IsNull())
			{
				Unauthorized();
				return;
			}
			if (context.Request.HttpMethod.ToUpper() == "POST")
			{
				Add();
				return;
			}
			context.Response.ClearHeaders();
			context.Response.ClearContent();
			context.Response.Clear();
			context.Response.ContentType = "plain/text";
			context.Response.ContentEncoding = Encoding.UTF8;
			try
			{
				var obj = this.CallMethod(Cmd);
				if (obj.IsNotNull())
				{
					context.Response.Write(obj.ToString());
				}
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

	}
}
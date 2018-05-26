using GS.SQL.DataSource;
using GS.SQLModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.Services
{
    public interface IFileServices {
        IEnumerable<FileGroup> GetGroup();
        IEnumerable<File> GetFileByName(int folder, string name);
        IEnumerable<File> GetFileByRole(string roleId);
        IEnumerable<File> GetFolderByName(int folder, string name);
        IEnumerable<File> GetFileByFolder(int folder, string roleId);
        IEnumerable<Folder> GetFolderByParent(int parent, string roleId);
        IEnumerable<Folder> GetFolderByGroup(int group);
        File GetFileById(int id);
        Folder GetFolderById(int id);
        int AddFile(File file);
        int AddFolder(Folder folder);
        int UpdateFile(File file);
        int UpdateFolder(Folder folder);
        int DeleteFile(int id);
        int DeleteFolder(int id);
    }

    public class FileServices: IFileServices
    {
		private readonly Folder _folder;
		private readonly FileGroup _fileGroup;
		private readonly File _file;
		public FileServices()
		{
			_folder = new Folder();
			_file = new File();
			_fileGroup = new FileGroup();
		}
		public IEnumerable<FileGroup> GetGroup()
		{
			return _fileGroup.Query();
		}
		public Folder GetFolderById(int id)
		{
			var condition = new SQLCondition();
			condition.Expression = "where Id = @id";
			var dbParameter = new DbParameter[] {
				new SqlParameter() { DbType = DbType.Int32,Value = id,
					ParameterName = "@id"
				}
			};
			return _folder.Query(condition: condition, param: dbParameter.ToArray()).FirstOrDefault();
		}
		public IEnumerable<File> GetFileByName(int folder, string name) {
			var condition = new SQLCondition();
			condition.Expression = "where Folder = @folder and  [Name] like '%" + name + "%'";
			var dbParameter = new DbParameter[] {
				new SqlParameter() {
					DbType = DbType.Int32,Value = folder,ParameterName = "@folder"
				}
			};
			return _file.Query(condition: condition, param: dbParameter.ToArray());
		}
		public IEnumerable<File> GetFileByRole(string roleId)
		{
			var condition = new SQLCondition();
			condition.Expression = "where Role like '%" + roleId + "%'";
			return _file.Query(condition: condition);
		}
		public IEnumerable<File> GetFolderByName(int folder, string name)
		{
			var condition = new SQLCondition();
			condition.Expression = "where Folder = @folder and [Name] like '%" + name + "%'";
			var dbParameter = new DbParameter[] {
				new SqlParameter() {
					DbType = DbType.Int32,Value = folder,ParameterName = "@folder"
				}
			};
			return _file.Query(condition: condition, param: dbParameter.ToArray());
		}
		public IEnumerable<File> GetFileByFolder(int folder, string roleId)
		{
			var condition = new SQLCondition();
			condition.Expression = "where Folder = @folder and Role like '%" + roleId + "%'";
			var dbParameter = new DbParameter[] {
				new SqlParameter() {
					DbType = DbType.Int32,Value = folder,ParameterName = "@folder"
				}
			};
			return _file.Query(condition: condition, param: dbParameter.ToArray());
		}
		public IEnumerable<Folder> GetFolderByParent(int parent, string roleId)
		{
			var condition = new SQLCondition();
			condition.Expression = "where Parent = @parent and Role like '%" + roleId + "%'";
			var dbParameter = new DbParameter[] {
				new SqlParameter() {
					DbType = DbType.Int32,Value = parent,ParameterName = "@parent"
				}
			};
			return _folder.Query(condition: condition, param: dbParameter.ToArray());
		}
		public IEnumerable<Folder> GetFolderByGroup(int group)
		{
			var condition = new SQLCondition();
			condition.Expression = "where Group = @group";
			var dbParameter = new DbParameter[] { new SqlParameter() {
					DbType = DbType.Int32,
					Value = group,
					ParameterName = "@group"
				}
			};
			return _folder.Query(condition: condition, param: dbParameter.ToArray());
		}
		public File GetFileById(int id)
		{
			var condition = new SQLCondition();
			condition.Expression = "where Id = @Id";
			var dbParameter = new DbParameter[] { new SqlParameter() {
					DbType = DbType.Int32,
					Value = id,
					ParameterName = "@Id"
				}
			};
			return _file.Query(condition: condition, param: dbParameter.ToArray()).FirstOrDefault();
		}
		public int AddFile(File file)
		{
			file.Updatetime = DateTime.UtcNow;
			return _file.Add(file);
		}
		public int AddFolder(Folder folder)
		{
			folder.Updatetime = DateTime.UtcNow;
			return _folder.Add(folder);
		}
		public int UpdateFile(File file)
		{
			file.Updatetime = DateTime.UtcNow;
			return _file.Update(file);
		}
		public int UpdateFolder(Folder folder)
		{
			folder.Updatetime = DateTime.UtcNow;
			return _folder.Update(folder);
		}
		public int DeleteFile(int id)
		{
			var sql = "delete [File] where Id = " + id;
			return _folder.Exec(sql);
		}
		public int DeleteFolder(int id)
		{
			var sql = " delete [Folder] where Id  = " + id;
			sql += " ;delete [Folder] where Parent  =  " + id;
			sql += " ;delete [File] where Folder  =  " + id;
			return _folder.Exec(sql);
		}
	}
}

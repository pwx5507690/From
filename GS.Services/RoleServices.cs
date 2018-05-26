using System;
using System.Collections.Generic;
using System.Linq;
using GS.SQLModel;
using GS.SQL.DataSource;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using GS.View.Model;

namespace GS.Services
{
    public interface IRoleServices
    {
        int UBindUserRole(IEnumerable<UserRole> userRole);
        int UnBindUserRoleByUserId(IEnumerable<int> userId);
        int BindUserRole(IEnumerable<UserRole> userRole);
        int AddRole(VRole vRole);
        int UpdateRole(VRole vRole);
        int DeleteRole(Role value);
        int DeleteRole(IEnumerable<Role> value);
        IEnumerable<Role> GetRoleByCode(string code);
        IEnumerable<UserRole> GetUserRoleByUserId(int userId);
        IEnumerable<UserRole> GetUserRole();
        IEnumerable<Role> GetRoleById(int id);
        IEnumerable<MenuRole> GetMenuRole();
        IEnumerable<Role> GetRole();
        IEnumerable<VMenu> GetMenu(User user);
        VRole GetVRoleByRoleId(int id);
        VRole GetVRole(int pageSize, int currentPage, string name = null);
        SQLPage<Role> GetRole(int pageSize, int currentPage);
        SQLPage<Role> GetRole(int pageSize, int currentPage, string name);
    }

    public class RoleServices: IRoleServices
    {
        private readonly Role _role;
        private readonly UserRole _userRole;
        private readonly MenuRole _menuRole;
        private readonly MenuServices _menuServices;
        public RoleServices()
        {
            _role = new Role();
            _userRole = new UserRole();
            _menuRole = new MenuRole();
            _menuServices = new MenuServices();
        }
        public IEnumerable<Role> GetRoleByCode(string code)
        {
            var condition = new SQLCondition();
            var dbParameter = new DbParameter[] {
                new SqlParameter() { DbType = DbType.String, Value = code, ParameterName = "@code"}
            };
            condition.Expression = "where Code=@code";
            return _role.Query(condition, dbParameter);
        }
        public int UBindUserRole(IEnumerable<UserRole> userRole)
        {
            return _userRole.Delete(userRole);
        }
        public IEnumerable<UserRole> GetUserRoleByUserId(int userId)
        {
            var condition = new SQLCondition();
            var dbParameter = new DbParameter[] {
                new SqlParameter() {
                    DbType = DbType.Int32,
                    Value = userId,
                    ParameterName = "@user"
                }
            };
            condition.Expression = "where [User]=@user";
            return _userRole.Query(condition, dbParameter);
        }
        public IEnumerable<UserRole> GetUserRole()
        {
            var sql = " select a.*,b.Name as RoleName from userRole as a left join [Role] as b on a.Role = b.Id";
            return _userRole.Query(sql: sql);
        }
        public int UnBindUserRoleByUserId(IEnumerable<int> userId)
        {
            var sql = $"delete userRole where [User] in({string.Join(",", userId)})";
            return _userRole.Exec(sql);
        }
        public int BindUserRole(IEnumerable<UserRole> userRole)
        {
            UnBindUserRoleByUserId(userRole.Select(t => t.User));
            return _userRole.Add(userRole);
        }
        public IEnumerable<Role> GetRoleById(int id)
        {
            var condition = new SQLCondition();
            var dbParameter = new DbParameter[] {
                new SqlParameter() {
                    DbType = DbType.Int32,
                    Value = id,
                    ParameterName = "@id"
                }
            };
            condition.Expression = "where Id=@id";
            return _role.Query(condition, dbParameter);
        }
        public VRole GetVRoleByRoleId(int id)
        {
            return new VRole()
            {
                MenuRole = GetMenuRole().Where(r => r.Role == id),
                Role = new SQLPage<Role>()
                {
                    Result = GetRoleById(id)
                }
            };
        }
        public VRole GetVRole(int pageSize, int currentPage, string name = null)
        {
            return new VRole()
            { 
                MenuRole = GetMenuRole(),
                Role = string.IsNullOrEmpty(name) ?
                GetRole(pageSize, currentPage) :
                GetRole(pageSize, currentPage, name)
            };
        }
        public IEnumerable<MenuRole> GetMenuRole()
        {
            var sql = "SELECT a.*,b.Name as MenuName FROM [Employee].[dbo].[MenuRole] as a left join [Menu] as b on a.Menu=b.id";
            return _menuRole.Query(sql: sql);
        }
        public IEnumerable<Role> GetRole()
        {
            return _role.Query();
        }
        public SQLPage<Role> GetRole(int pageSize, int currentPage)
        {
            return _role.Query(pageSize, currentPage);
        }
        public SQLPage<Role> GetRole(int pageSize, int currentPage, string name)
        {
            var condition = new SQLCondition();
            condition.Expression = $"where name like '%{name}%'";
            return _role.Query(pageSize, currentPage, condition: condition);
        }
        public int AddRole(VRole vRole)
        {
            var role = new Role();
            role.Name = vRole.Name;
            role.Updatetime = DateTime.UtcNow;
            role.Code = Guid.NewGuid().ToString();
            var result = _role.Add(role);
            role = GetRoleByCode(role.Code).SingleOrDefault();
            if (result > 0)
            {
                if (vRole.MenuId == null || !vRole.MenuId.Any())
                {
                    return result;
                }

                var menuRole = vRole.MenuId.Select(t =>
                {
                    var value = new MenuRole();
                    value.Menu = t;
                    value.Role = role.Id;
                    value.Updatetime = DateTime.UtcNow;
                    return value;
                });
                return _menuRole.Add(menuRole);
            }
            return -1;
        }
        public int UpdateRole(VRole vRole)
        {
            var role = new Role();
            role.Name = vRole.Name;
            role.Updatetime = DateTime.UtcNow;
            role.Id = vRole.Id;
            role.Code = null;
            var result = _role.Update(role);
            if (result > 0)
            {
                if (vRole.MenuId == null || !vRole.MenuId.Any())
                {
                    return result;
                }
                var sql = $"delete MenuRole where Role in({role.Id});";
                _menuRole.Exec(sql);
                var menuRole = vRole.MenuId.Select(t =>
                {
                    var value = new MenuRole();
                    value.Menu = t;
                    value.Role = role.Id;
                    value.Updatetime = DateTime.UtcNow;
                    return value;
                });
                return _menuRole.Add(menuRole);
            }
            return -1;
        }
        public int DeleteRole(Role value)
        {
            return DeleteRole(new List<Role> { value });
        }
        public int DeleteRole(IEnumerable<Role> value)
        {
            var roleId = string.Join(",", value.Select(t => t.Id));
            var sql = "delete MenuRole where Role in(" + roleId + ");";
            sql += "delete UserRole where Role in(" + roleId + ")";
            sql += "delete Role where id in(" + roleId + ")";
            return _role.Exec(sql);
        }
        public IEnumerable<VMenu> GetMenu(User user)
        {
            var userId = user.Id;
            var userRole = _userRole.Query().Where(t => t.User == userId);
            var role = userRole.Select(t => t.Role);
            var menuRole = _menuRole.Query().Where(t => role.Contains(t.Role)).Select(t => t.Menu);
            return _menuServices.ConvertToVMenu(_menuServices.GetMenu().Where(t => menuRole.Contains(t.Id)));
        }
    }
}

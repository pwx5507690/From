using System;
using System.Collections.Generic;
using System.Linq;
using GS.SQLModel;
using GS.SQL.DataSource;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using GS.View.Model;
using GS.Common.Util;
using Login = GS.SQLModel.Login;

namespace GS.Services
{
    public interface IUserServices
    {
        int AddLogin(VLogin item, string ipAddress);
        int Add(User user);
        int ExistByEmail(string email, int id);
        int ExistByPhone(string phone, int id);
        int Delete(User user);
        int Delete(IEnumerable<User> user);
        int Update(User user);
        VLogin Login(string login, string password);

        User GetUserById(int id);
        SQLPage<User> GetUser(int pageSize, int currentPage);
        SQLPage<User> GetUserByName(string name, int pageSize, int currentPage);
        SQLPage<VUser> ConverToVUser(SQLPage<User> user);
        SQLPage<VUser> GetUserByName(int page, int current, string name);
        SQLPage<VUser> GetVUser(int page, int current);
        SQLPage<VUser> GetUser();
    }

    public class UserServices : IUserServices
    {
        private readonly User _user;
        private readonly Login _login;
        private readonly DepartmentServices _departmentServices;
        public UserServices()
        {
            _user = new User();
            _login = new Login();
            _departmentServices = new DepartmentServices();
        }
        public int AddLogin(VLogin item, string ipAddress)
        {
            var login = new Login();
            item.SessionId = Guid.NewGuid().ToString() + login.LoginTime.ToString("yyyyMMddHHmmss");
            login.SessionId = item.SessionId;
            login.UserId = item.User.Id;
            login.UserName = item.User.Name;
            login.IPAddress = ipAddress;
            login.LoginTime = DateTime.UtcNow;

            var sql = "update Login set IsDrop = 0  where UserId=@userId ";
            var dbParameter = new DbParameter[] {
                new SqlParameter() { DbType = DbType.Int32, Value = item.User.Id, ParameterName = "@userId"},
            };
            _login.Exec(sql, dbParameter.ToArray());
            return _login.Add(login);
        }
        public VLogin Login(string login, string password)
        {
            var condition = new SQLCondition();
            var dbParameter = new DbParameter[] {
                new SqlParameter() { DbType = DbType.String, Value = login, ParameterName = "@login"},
            };
            condition.Expression = "where name = @login or  Email = @login or Phone=@login";
            var user = _user.Query(condition, dbParameter);
            var vLogin = new VLogin();
            if (!user.Any())
            {
                vLogin.IsLogin = false;
                vLogin.LoginStats = LoginStats.NAME;
                return vLogin;
            }
            var item = user.Where(t => t.Password.Equals(password.GetMd5Str())).SingleOrDefault();
            if (item == null)
            {
                vLogin.IsLogin = false;
                vLogin.LoginStats = LoginStats.PASSOWRD;
                return vLogin;
            }
            vLogin.IsLogin = true;
            vLogin.LoginStats = LoginStats.SUCCESS;
            vLogin.User = item;
            return vLogin;
        }
        public SQLPage<User> GetUser(int pageSize, int currentPage)
        {
            var sql = "select b.*,c.Name as DepartmentName from (select row_number()over(order by tempColumn)rownumber, * from(select top {0} tempColumn = 0, * from {1})a )b left join [Department] as c on b.Department = c.Id where rownumber > {2}";
            return _user.Query(pageSize, currentPage, sql: sql);
        }
        public SQLPage<User> GetUserByName(string name, int pageSize, int currentPage)
        {
            var condition = new SQLCondition();
            condition.Expression = "where Name like '%" + name + "%'";
            var sql = "select b.*,c.Name as DepartmentName from (select row_number()over(order by tempColumn)rownumber, * from(select top {0} tempColumn = 0, * from {1})a )b left join [Department] as c on b.Department = c.Id where rownumber > {2}";
            return _user.Query(pageSize, currentPage, condition, sql: sql);
        }
        public int ExistByEmail(string email, int id)
        {
            var condition = new SQLCondition();
            condition.Expression = "where Email=@email";
            var dbParameter = new List<DbParameter>
            {
               new SqlParameter { ParameterName = "@email", DbType = DbType.String, Value = email}
            };

            var user = _user.Query(condition, dbParameter);
            return Exist(user, id);
        }
        public int ExistByPhone(string phone, int id)
        {
            var condition = new SQLCondition();
            condition.Expression = "where Phone=@phone";
            var dbParameter = new List<DbParameter>
            {
               new SqlParameter { ParameterName = "@phone", DbType = DbType.String, Value = phone}
            };

            var user = _user.Query(condition, dbParameter);
            return Exist(user, id);
        }
        private int Exist(IEnumerable<User> user, int id)
        {

            if (user.Any())
            {
                if (id != -1 && user.Count() == 1 && user.SingleOrDefault().Id == id)
                    return 1;
                return 0;
            }
            else
            {
                return 1;
            }
        }
        public User GetUserById(int id)
        {
            var condition = new SQLCondition();
            condition.Expression = "where Id = @Id";
            var dbParameter = new List<DbParameter>
            {
               new SqlParameter { ParameterName = "@Id", DbType = DbType.Int32, Value = id}
            };
            return _user.Query(condition, dbParameter).FirstOrDefault();
        }
        private void SetVUser(VUser vUser, User user, IEnumerable<Department> dept)
        {
            foreach (var item in dept)
            {
                if (item.Id == user.Department)
                    vUser.Department = item;
                vUser.User = user;
            }
        }
        public SQLPage<VUser> ConverToVUser(SQLPage<User> user)
        {
            var vUser = new SQLPage<VUser>();
            vUser.Count = user.Count;
            vUser.IsNext = user.IsNext;
            vUser.Pagination = user.Pagination;
            var dept = _departmentServices.GetDepartmentById(user.Result.Select(t => t.Department).Distinct().ToArray());
            vUser.Result = user.Result.Select(t =>
            {
                var value = new VUser();
                if (t.Department == -1)
                    value.User = t;
                else
                    SetVUser(value, t, dept);
                return value;
            });
            return vUser;
        }
        public int Delete(IEnumerable<User> user)
        {
            return _user.Delete(user);
        }
        public int Delete(User user)
        {
            return _user.Delete(user);
        }
        public int Update(User user)
        {
            if (user.Password.IsNotNullOrEmpty())
            {
                user.Password = user.Password.GetMd5Str();
            }
            return _user.Update(user);
        }
        public int Add(User user)
        {
            user.Password = user.Password.GetMd5Str();
            return _user.Add(user);
        }
        public SQLPage<VUser> GetUserByName(int page, int current, string name)
        {
            var condition = new SQLCondition();
            condition.Expression = "where name like '%" + name + "%'";
            return ConverToVUser(_user.Query(page, current, condition));
        }
        public SQLPage<VUser> GetUser()
        {
            var user = new SQLPage<User>()
            {
                Count = -1,
                IsNext = false,
                Result = _user.Query()
            };
            return ConverToVUser(user);
        }
        public SQLPage<VUser> GetVUser(int page, int current)
        {
            return ConverToVUser(_user.Query(page, current));
        }
    }
}

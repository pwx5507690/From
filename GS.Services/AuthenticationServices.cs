using GS.SQL.DataSource;
using GS.SQLModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.Services
{
    public interface IAuthenticationServices
    {
        int Add(AuthenticationSite authenticationSite);
        AuthenticationSite GetAuthenticationSiteByUuid(string uuid);
        int Remove(string uuid);
    }
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly AuthenticationSite _authenticationSite;
        public AuthenticationServices()
        {
            _authenticationSite = new AuthenticationSite();
        }
        public AuthenticationSite GetAuthenticationSiteByUuid(string uuid)
        {
            var condition = new SQLCondition();
            condition.Expression = $"where uuid = '{uuid}'";
            return _authenticationSite.Query(condition).FirstOrDefault();
        }
        public int Add(AuthenticationSite authenticationSite)
        {
            return _authenticationSite.Add(authenticationSite);
        }
        public int Remove(string uuid)
        {
            return _authenticationSite.Exec($"delete AuthenticationSite where uuid='{uuid}'");
        }
    }
}

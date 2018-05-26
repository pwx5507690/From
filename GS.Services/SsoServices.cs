using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GS.SQLModel;
using GS.SQL.DataSource;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace GS.Services
{
    public interface ISsoServices {
        Sso GetSsoByKey(string key);
    }
    public class SsoServices: ISsoServices
    {
		private readonly Sso _sso;
		public SsoServices() {
			_sso = new Sso();
		}
		public Sso GetSsoByKey(string key) {
			var condition = new SQLCondition();
			var dbParameter = new DbParameter[] {
				new SqlParameter() { DbType = DbType.String, Value = key, ParameterName = "@key"}
			};
			condition.Expression = "where Key=@key";
			return _sso.Query(condition, dbParameter).FirstOrDefault();
		}
	}
}

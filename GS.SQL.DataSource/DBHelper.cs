using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace GS.SQL.DataSource
{
    public class DBHelper
	{
		private readonly Database dbTp;

		public DBHelper(string connectionStr)
		{
			var factory = new DatabaseProviderFactory();
			dbTp = factory.Create(connectionStr);
		}

		private void Write(string message, params DbParameter[] parameters)
		{
            System.Diagnostics.Trace.WriteLine(message);
			if (parameters != null)
			{
				var str = new StringBuilder();
				foreach (var item in parameters)
                    str.AppendFormat("{0}={1}", item.ParameterName, item.Value);
                System.Diagnostics.Trace.WriteLine(str.ToString());
			}
		}

		public object ExecuteScalarTp(string sql)
		{
			Write(sql);
			var cm = dbTp.GetSqlStringCommand(sql);
			object ret = dbTp.ExecuteScalar(cm);
			return ret;
		}

		public object ExecuteScalarTp(string sql, params DbParameter[] parameters)
		{
			Write(sql, parameters);
			var cm = dbTp.GetSqlStringCommand(sql);
			PrepareCommand(cm, parameters, dbTp);
			object ret = dbTp.ExecuteScalar(cm);
			return ret;
		}

		public IDataReader ExecuteReaderTp(string sql)
		{
			Write(sql);
			var cm = dbTp.GetSqlStringCommand(sql);
			return dbTp.ExecuteReader(cm);
		}

		public IDataReader ExecuteReaderTp(string sql, params DbParameter[] parameters)
		{
			Write(sql, parameters);
			var cm = dbTp.GetSqlStringCommand(sql);
			PrepareCommand(cm, parameters, dbTp);
			return dbTp.ExecuteReader(cm);
		}

		public DataSet ExecuteDataSetTp(string sql)
		{
			Write(sql);
			DbCommand cm = dbTp.GetSqlStringCommand(sql);
			return dbTp.ExecuteDataSet(cm);
		}

		public DataSet ExecuteDataSetTp(string sql, params DbParameter[] parameters)
		{
			Write(sql, parameters);
			var cm = dbTp.GetSqlStringCommand(sql);
			PrepareCommand(cm, parameters, dbTp);
			return dbTp.ExecuteDataSet(cm);
		}

		public DataSet ExecuteDataSetStoreProcedureTp(string spName, params DbParameter[] parameters)
		{
			Write(spName, parameters);
			DbCommand cm = dbTp.GetStoredProcCommand(spName);
			if (parameters != null && parameters.Length > 0)
			{
				PrepareCommand(cm, parameters, dbTp);
			}
			return dbTp.ExecuteDataSet(cm);
		}

		public int ExecuteNonQueryTp(string sql)
		{
			try
			{
				Write(sql);
				DbCommand cm = dbTp.GetSqlStringCommand(sql);
				return dbTp.ExecuteNonQuery(cm);
			}
			catch (Exception e)
			{
				throw e;
			}

		}

		public int ExecuteNonQueryTp(string sql, params DbParameter[] parameters)
		{
			Write(sql, parameters);
			DbCommand cm = dbTp.GetSqlStringCommand(sql);
			PrepareCommand(cm, parameters, dbTp);
			return dbTp.ExecuteNonQuery(cm);
		}

        //public void ExecuteNonQueryTp(string sql, Action<> params DbParameter[] parameters) {
        //    var connection = dbTp.CreateConnection();
        //    DbTransaction Tran = connection.BeginTransaction();
             
        //    ExecuteNonQueryTp(sql, parameters);
        //    Tran.Commit();
        //}
           

		public object ExecuteStoreProcedureTp(string procName, string outPutColumnName, params DbParameter[] parameters)
		{
			DbCommand cm = dbTp.GetStoredProcCommand(procName);
			PrepareCommand(cm, parameters, dbTp);
			int result = dbTp.ExecuteNonQuery(cm);
			if (!string.IsNullOrEmpty(outPutColumnName))
			{
				object ret = cm.Parameters[outPutColumnName].Value;
				return ret;
			}
			else
			{
				return null;
			}
		}

		private void PrepareCommand(DbCommand cm, DbParameter[] parameters, Database database)
		{
			var listParameter = new List<DbParameter>();
			foreach (DbParameter p in parameters)
			{
				DbProviderFactory fac = database.DbProviderFactory;
				DbParameter param = fac.CreateParameter();
				param.DbType = p.DbType;
				param.ParameterName = p.ParameterName;
				param.Size = p.Size;
				param.Value = p.Value;
				param.Direction = p.Direction;
				listParameter.Add(param);
			}
			cm.Parameters.AddRange(listParameter.ToArray());
		}
	}
}

be509a2f1a05b3e777abb78332bf3502 test

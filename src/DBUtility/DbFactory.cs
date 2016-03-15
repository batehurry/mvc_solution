using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Configuration;

namespace YH_OA.DBUtility
{
    public class DbFactory
    {
        private readonly static string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public static IDbConnection DbInstanceFactory(string dbType)
        {
            IDbConnection dbInstance = null;

            switch (dbType.ToLower())
            {
                case "sqlserver":
                    dbInstance = new SqlConnection(connectionString);
                    break;
                case "oracle":
                    dbInstance = new OracleConnection(connectionString);
                    break;
                case "mysql":
                    dbInstance = new OdbcConnection(connectionString);
                    break;
                case "access":
                    dbInstance = new OleDbConnection(connectionString);
                    break;
                default:
                    dbInstance = new SqlConnection(connectionString);
                    break;
            }

            return dbInstance;
        }

        public static IDbCommand CmdInstanceFactory(string dbType)
        {
            IDbCommand cmdInstance = null;

            switch (dbType.ToLower())
            {
                case "sqlserver":
                    cmdInstance = new SqlCommand();
                    break;
                case "oracle":
                    cmdInstance = new OracleCommand();
                    break;
                case "mysql":
                    cmdInstance = new OdbcCommand();
                    break;
                case "access":
                    cmdInstance = new OleDbCommand();
                    break;
                default:
                    cmdInstance = new SqlCommand();
                    break;
            }

            return cmdInstance;
        }

        public static IDbDataAdapter AdapterInstanceFactory(string dbType)
        {
            IDbDataAdapter adapterInstance = null;

            switch (dbType.ToLower())
            {
                case "sqlserver":
                    adapterInstance = new SqlDataAdapter();
                    break;
                case "oracle":
                    adapterInstance = new OracleDataAdapter();
                    break;
                case "mysql":
                    adapterInstance = new OdbcDataAdapter();
                    break;
                case "access":
                    adapterInstance = new OleDbDataAdapter();
                    break;
                default:
                    adapterInstance = new SqlDataAdapter();
                    break;
            }

            return adapterInstance;
        }

        /// <summary>
        /// 准备执行一个命令
        /// </summary>
        /// <param name="cmd">sql命令</param>
        /// <param name="conn">Sql连接</param>
        /// <param name="trans">Sql事务</param>
        /// <param name="cmdType">命令类型例如 存储过程或者文本</param>
        /// <param name="cmdText">命令文本,例如：Select * from Products</param>
        /// <param name="cmdParms">执行命令的参数</param>
        public static void FillParameter(string dbType, IDbCommand cmd, params IDataParameter[] cmdParms)
        {
            IDataParameter dbParameter = null;

            if (cmdParms != null)
            {
                switch (dbType.ToLower())
                {
                    case "sqlserver":
                        dbParameter = new SqlParameter();
                        break;
                    case "oracle":
                        dbParameter = new OracleParameter();
                        break;
                    case "mysql":
                        dbParameter = new OdbcParameter();
                        break;
                    case "access":
                        dbParameter = new OleDbParameter();
                        break;
                    default:
                        dbParameter = new SqlParameter();
                        break;
                }

                foreach (IDataParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
    }
}

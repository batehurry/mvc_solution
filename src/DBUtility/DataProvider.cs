using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;

namespace YH_OA.DBUtility
{
    public class DataProvider : DataProviders.IDataProvider
    {
        public DataProvider()
        {

        }

        public DataProvider(IDbConnection connection)
        {
            this.Connection = connection;
            
        }

        private static string dbType = ConfigurationManager.AppSettings["DataType"];

        private static IDbConnection connection = null;

        public IDbConnection Connection
        {
            get
            {
                return DbFactory.DbInstanceFactory(dbType);
            }
            set
            {
                connection = value;
            }
        }

        public IDbCommand Command(string sql)
        {
            return Command(sql, CommandType.Text, null);
        }

        public IDbCommand Command(string sql, CommandType cmdType)
        {
            return Command(sql, cmdType, null);
        }

        public IDbCommand Command(string sql, params IDataParameter[] param)
        {
            return Command(sql, CommandType.Text, param);
        }

        public IDbCommand Command(string sql, CommandType cmdType, params IDataParameter[] param)
        {
            IDbCommand cmd = DbFactory.CmdInstanceFactory(dbType);
            DbFactory.FillParameter(dbType, cmd, param);
            cmd.CommandText = sql;
            cmd.CommandType = cmdType;
            cmd.Connection = this.Connection;
            return cmd;
        }

        public IDataReader DataReader(IDbCommand dbCmd)
        {
            IDbCommand cmd = dbCmd;
            cmd.Connection.Open();
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public IDataReader DataReader(string sql)
        {
            return DataReader(sql, CommandType.Text);
        }

        public IDataReader DataReader(string sql, CommandType cmdType)
        {
            return DataReader(sql, cmdType, null);
        }

        public IDataReader DataReader(string sql, params IDataParameter[] param)
        {
            return DataReader(sql, CommandType.Text, param);
        }

        public IDataReader DataReader(string sql, CommandType cmdType, params IDataParameter[] param)
        {
            IDbCommand cmd = Command(sql, cmdType, param);
            cmd.Connection.Open();
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public IDbDataAdapter DataAdapter(string sql)
        {
            return DataAdapter(sql, CommandType.Text);
        }

        public IDbDataAdapter DataAdapter(string sql, CommandType cmdType)
        {
            return DataAdapter(sql, cmdType, null);
        }

        public IDbDataAdapter DataAdapter(string sql, params IDataParameter[] param)
        {
            return DataAdapter(sql, CommandType.Text, param);
        }

        /// <summary>
        /// 只用于查询的适配器
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="param">查询参数列表</param>
        /// <returns>IDbDataAdapter</returns>
        public IDbDataAdapter DataAdapter(string sql, CommandType cmdType, params IDataParameter[] param)
        {
            IDbDataAdapter da = DbFactory.AdapterInstanceFactory(dbType);
            da.SelectCommand = Command(sql, cmdType, param);
            return da;
        }

        public DataSet GetResult(string sql)
        {
            return GetResult(sql, null);
        }

        public DataSet GetResult(string sql, CommandType cmdType)
        {
            return GetResult(sql, cmdType, null);
        }

        public DataSet GetResult(string sql, params IDataParameter[] param)
        {
            return GetResult(sql, CommandType.Text, param);
        }

        public DataSet GetResult(IDataAdapter adapter)
        {
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            return ds;
        }

        public DataSet GetResult(string sql, CommandType cmdType, params IDataParameter[] param)
        {
            try
            {
                DataSet ds = new DataSet();
                IDbDataAdapter adapter = DataAdapter(sql, cmdType, param);
                adapter.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Int32 ExecuteNonQuery(string sql)
        {
            return ExecuteNonQuery(sql, null);
        }

        public Int32 ExecuteNonQuery(IDbCommand command)
        {
            using (IDbCommand cmd = command)
            {
                cmd.Connection.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public Int32 ExecuteNonQuery(string sql, CommandType cmdType)
        {
            return ExecuteNonQuery(sql, cmdType, null);
        }

        public Int32 ExecuteNonQuery(string sql, params IDataParameter[] param)
        {
            return ExecuteNonQuery(sql, CommandType.Text, param);
        }

        public Int32 ExecuteNonQuery(string sql, CommandType cmdType, params IDataParameter[] param)
        {
            using (IDbCommand cmd = Command(sql, cmdType, param))
            {
                cmd.Connection.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public object ExecuteScalar(string sql)
        {
            return ExecuteScalar(sql, null);
        }

        public object ExecuteScalar(IDbCommand command)
        {
            using (IDbCommand cmd = command)
            {
                cmd.Connection.Open();
                return cmd.ExecuteScalar();
            }
        }

        public object ExecuteScalar(string sql, CommandType cmdType)
        {
            return ExecuteScalar(sql, cmdType, null);
        }

        public object ExecuteScalar(string sql, params IDataParameter[] param)
        {
            return ExecuteScalar(sql, CommandType.Text, param);
        }

        public object ExecuteScalar(string sql, CommandType cmdType, params IDataParameter[] param)
        {
            using (IDbCommand cmd = Command(sql, cmdType, param))
            {
                cmd.Connection.Open();
                return cmd.ExecuteScalar();
            }
        }

        public int ExecuteSql(IList<String> sql, IList<IDataParameter[]> param)
        {
            return this.ExecuteSql(sql, CommandType.Text, param);
        }

        /// <summary>
        /// 数据库备份
        /// </summary>
        /// <param name="path">保存路径</param>
        /// <param name="database">要备份的数据库名称</param>
        /// <returns></returns>
        public int BackupDataBase(string path, string database)
        {
            var count = 0;
            var backupFileName = System.IO.Path.Combine(path, DateTime.Today.ToString("yyyyMMdd.bak"));

            var sqlstr = string.Format("BACKUP DATABASE [" + database + "] TO DISK='{0}'", backupFileName);
            try
            {
                count = ExecuteNonQuery(sqlstr, CommandType.Text);
            }
            catch
            {

            }
            return count;
        }

        /// <summary>
        /// 通过事务执行多条Sql语句
        /// </summary>
        /// <param name="sql">Sql语句集合</param>
        /// <param name="cmdType"></param>
        /// <param name="param">参数集合</param>
        public int ExecuteSql(IList<String> sql, CommandType cmdType, IList<IDataParameter[]> param)
        {
            if (sql.Count != param.Count)
            {
                //抛出异常，参数列表和
                throw new Exception("参数列表和数据库语句条数不匹配!");
            }
            else
            {
                using (IDbConnection conn = this.Connection)
                {
                    conn.Open();
                    using (IDbTransaction tran = conn.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i < param.Count; i++)
                            {
                                using (IDbCommand cmd = Command(sql[i], cmdType, param[i]))
                                {
                                    cmd.Transaction = tran;
                                    cmd.Connection = conn;
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            tran.Commit();
                            return 1;
                        }
                        catch
                        {
                            tran.Rollback();
                            conn.Close();
                            conn.Dispose();
                            return -1;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 通过事务执行多条Sql语句不包含parameter
        /// </summary>
        /// <param name="sql">Sql语句集合</param>
        /// <param name="cmdType"></param>
        /// <param name="param">参数集合</param>
        public int ExecuteSql(IList<String> sql, CommandType cmdType)
        {
            if (sql.Count > 0)
            {
                using (IDbConnection conn = this.Connection)
                {
                    conn.Open();
                    using (IDbTransaction tran = conn.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i < sql.Count; i++)
                            {
                                using (IDbCommand cmd = Command(sql[i], cmdType))
                                {
                                    cmd.Transaction = tran;
                                    cmd.Connection = conn;
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            tran.Commit();
                            return 1;
                        }
                        catch
                        {
                            tran.Rollback();
                            conn.Close();
                            conn.Dispose();
                            return -1;
                        }
                    }
                }
            }
            else
            {
                return 1;
            }
        }

        public String GetIdByExecuteSql(IList<String> sql, CommandType cmdType, IList<IDataParameter[]> param, string _Sql)
        {
            if (this.ExecuteSql(sql, cmdType, param) == 1)
            {
                return this.ExecuteScalar(_Sql).ToString();
            }
            return null;
        }
    }
}

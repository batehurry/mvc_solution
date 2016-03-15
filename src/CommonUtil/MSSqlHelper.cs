using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CommonUtil
{
    public class MSSqlHelper
    {
        public static string connstr = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;

        #region Null值处理

        public static object ToDBValue(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            else
                return value;
        }

        public static object FromDBValue(object value)
        {
            if (value == DBNull.Value)
            {
                return null;
            }
            else
                return value;
        }

        #endregion

        #region 常用方法
        /// <summary>
        /// 返回受影响条数
        /// </summary>
        /// <param name="sql">执行内容</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql, params SqlParameter[] paras)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(paras);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 执行存储过程，返回受影响条数
        /// </summary>
        /// <param name="sql">执行内容</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public static int ExecuteNonQuerySP(string sql, params SqlParameter[] paras)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(paras);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 返回一个对象,执行结果集中的第一行第一列
        /// </summary>
        /// <param name="sql">执行内容</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public static object ExecuteScalar(string sql, params SqlParameter[] paras)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(paras);
                    return cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// 执行存储过程，返回一个对象,结果集中的第一行第一列
        /// </summary>
        /// <param name="sql">执行内容</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public static object ExecuteScalarSP(string sql, params SqlParameter[] paras)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(paras);
                    return cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// 返回一个DataSet
        /// </summary>
        /// <param name="sql">执行内容</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public static DataSet ExecuteSet(string sql, params SqlParameter[] paras)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(paras);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
            }
        }

        /// <summary>
        /// 执行存储过程返回一个DataSet
        /// </summary>
        /// <param name="sql">执行内容</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public static DataSet ExecuteSetSP(string sql, params SqlParameter[] paras)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(paras);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
            }
        }

        /// <summary>
        /// 返回一个Datatable
        /// </summary>
        /// <param name="sql">执行内容</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string sql, params SqlParameter[] paras)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(paras);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        return ds.Tables[0];
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 执行存储过程，返回Datatable
        /// </summary>
        /// <param name="sql">执行内容</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTableSP(string sql, params SqlParameter[] paras)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(paras);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        return ds.Tables[0];
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 返回一个SqlDataReader( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="sql">执行内容</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public static SqlDataReader ExecuteDataReader(string sql, params SqlParameter[] paras)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(paras);
                    SqlDataReader sqlreader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    return sqlreader;
                }
            }
        }

        /// <summary>
        /// 执行存储过程返回SqlDataReader( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="sql">执行内容</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public static SqlDataReader ExecuteDataReaderSP(string sql, params SqlParameter[] paras)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(paras);
                    //关闭DataReader同时关闭Connection
                    SqlDataReader sqlreader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    return sqlreader;
                }
            }
        }
        #endregion

    }
}

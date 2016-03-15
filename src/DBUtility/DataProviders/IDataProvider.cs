using System;
using System.Data;

namespace YH_OA.DBUtility.DataProviders
{
    /// <summary>
    /// 对数据库访问的通用接口
    /// </summary>
    public interface IDataProvider
    {
        IDbConnection Connection { get; set; }

        IDbCommand Command(String sql);
        IDbCommand Command(String sql, CommandType cmdType);
        IDbCommand Command(String sql, params IDataParameter[] param);
        IDbCommand Command(String sql, CommandType cmdType, params IDataParameter[] param);

        IDataReader DataReader(String sql);
        IDataReader DataReader(IDbCommand cmd);
        IDataReader DataReader(String sql, CommandType cmdType);
        IDataReader DataReader(String sql, params IDataParameter[] param);
        IDataReader DataReader(String sql, CommandType cmdType, params IDataParameter[] param);

        IDbDataAdapter DataAdapter(String sql);
        IDbDataAdapter DataAdapter(String sql, CommandType cmdType);
        IDbDataAdapter DataAdapter(String sql, params IDataParameter[] param);
        IDbDataAdapter DataAdapter(String sql, CommandType cmdType, params IDataParameter[] param);

        DataSet GetResult(String sql);
        DataSet GetResult(IDataAdapter adapter);
        DataSet GetResult(String sql, CommandType cmdType);
        DataSet GetResult(String sql, params IDataParameter[] param);
        DataSet GetResult(String sql, CommandType cmdType, params IDataParameter[] param);

        Int32 ExecuteNonQuery(String sql);
        Int32 ExecuteNonQuery(IDbCommand cmd);
        Int32 ExecuteNonQuery(String sql, CommandType cmdType);
        Int32 ExecuteNonQuery(String sql, params IDataParameter[] param);
        Int32 ExecuteNonQuery(String sql, CommandType cmdType, params IDataParameter[] param);

        Object ExecuteScalar(String sql);
        Object ExecuteScalar(IDbCommand cmd);
        Object ExecuteScalar(String sql, CommandType cmdType);
        Object ExecuteScalar(String sql, params IDataParameter[] param);
        Object ExecuteScalar(String sql, CommandType cmdType, params IDataParameter[] param);
    
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PTFQA_Common;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.OracleClient;

using System.Data;

namespace PTFQA_DBPool
{
    public class DBPool : Common
    {
        #region "Variables"

        private string _strLastError = string.Empty;
        private int intTimeOut = 300;
        bool _bOracle = false;
        DbConnection _adoConnection = null;
        DbTransaction _adoTransaction = null;
        Common _objCommon = null;
        public struct ErrorMessage
        {
            public const string DbConnectionNotOpened = "Database connection is not opened";
            public const string ErrorWhileClosingConnection = "Error while closing connection";
            public const string ErrorWhileEstablishingConnection = "Error while establishing connection";
        }

        #endregion

        #region "Properties"

        public string ConnectionString { get; set; }

        #endregion

        #region "Methods"

        /// <summary>
        /// Get Last Error Details
        /// </summary>
        /// <returns>string</returns>
        public string GetLastError()
        {
            return _strLastError;
        }

        /// <summary>
        /// Establish Connection to Database
        /// </summary>
        /// <returns>bool</returns>
        private bool GetConnection()
        {
            bool bReturn = false;

            try
            {
                if (_adoConnection != null)
                {
                    if (_adoConnection.State != ConnectionState.Open)
                    {
                        _adoConnection.Open();
                    }
                }
                else
                {
                    if (!_bOracle)
                    {
                        _adoConnection = new SqlConnection(ConnectionString);
                    }
                    else
                    {
                        _adoConnection = new OracleConnection(ConnectionString);
                    }

                    _adoConnection.Open();
                }

                bReturn = true;
            }
            catch (Exception ex)
            {
                _strLastError = ex.Message;
            }

            return bReturn;
        }

        /// <summary>
        /// Open Connection to Database
        /// </summary>
        /// <returns>bool</returns>
        private bool OpenConnection()
        {
            bool bReturn = false;

            try
            {
                if (GetConnection())
                {
                    bReturn = true;
                }
            }
            catch (Exception ex)
            {
                _objCommon = new Common();

                _objCommon.WriteToFile(_objCommon.GetLogPath(),
                                        string.Format("{0} DBPool-Catch - {1} - {2} - 000",
                                        Convert.ToString(DateTime.Now), ErrorMessage.ErrorWhileEstablishingConnection,
                                        System.Net.Dns.GetHostName()), true);

                _strLastError = ex.Message;
            }
            finally
            {
                _objCommon = null;
            }

            return bReturn;
        }

        /// <summary>
        /// Close database Connection
        /// </summary>
        private void CloseConnection()
        {
            try
            {
                if (_adoConnection != null)
                {
                    _adoConnection.Close();
                }
            }
            catch (Exception ex)
            {
                _objCommon = new Common();

                _objCommon.WriteToFile(_objCommon.GetLogPath(),
                                        string.Format("{0} DBPool-Catch - {1} - {2} - 000",
                                        Convert.ToString(DateTime.Now), ErrorMessage.ErrorWhileClosingConnection,
                                        System.Net.Dns.GetHostName()), true);

                _strLastError = ex.Message;
            }
            finally
            {
                if (_adoConnection != null)
                {
                    _adoConnection.Dispose();
                    _adoConnection = null;
                }

                _objCommon = null;
            }
        }

        /// <summary>
        /// Create Command Instance with respective SQLQuery or SPName
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="cmd"></param>
        private void CreateCmdInstance(string sqlQuery, ref DbCommand cmd)
        {
            if (!_bOracle)
            {
                cmd = _adoTransaction == null ? new SqlCommand(sqlQuery, (SqlConnection)_adoConnection) :
                                                new SqlCommand(sqlQuery, (SqlConnection)_adoConnection, (SqlTransaction)_adoTransaction);

            }
            else
            {
                cmd = _adoTransaction == null ? new OracleCommand(sqlQuery, (OracleConnection)_adoConnection) :
                                               new OracleCommand(sqlQuery, (OracleConnection)_adoConnection, (OracleTransaction)_adoTransaction);
            }
        }

        /// <summary>
        /// Create Command Instance with respective SQLQuery or SPName for SQL
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="cmd"></param>
        private void CreateCmdInstanceSql(string sqlQuery, ref SqlCommand cmd)
        {

            cmd = _adoTransaction == null ? new SqlCommand(sqlQuery, (SqlConnection)_adoConnection) :
                                                 new SqlCommand(sqlQuery, (SqlConnection)_adoConnection, (SqlTransaction)_adoTransaction);
        }

        /// <summary>
        /// Execute Query with specific timeout and resultset as output
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="resultSet"></param>
        /// <param name="timeOut"></param>
        /// <returns>bool</returns>
        public bool RsQuery(string sqlQuery, ref DataSet resultSet, int timeOut)
        {
            DbCommand cmd = null;
            DbDataAdapter da = null;
            bool bReturn = false;

            try
            {
                da = default(DbDataAdapter);

                if (_adoConnection == null)
                {
                    _strLastError = ErrorMessage.DbConnectionNotOpened;
                    return false;
                }

                //Create Command Instance with respective SQLQuery or SPName
                CreateCmdInstance(sqlQuery, ref cmd);

                if (timeOut > 0) cmd.CommandTimeout = timeOut;

                if (!_bOracle)
                {
                    da = new SqlDataAdapter((SqlCommand)cmd);
                }
                else
                {
                    da = new OracleDataAdapter((OracleCommand)cmd);
                }

                resultSet = new DataSet();
                da.Fill(resultSet);
                bReturn = true;

            }
            catch (Exception ex)
            {
                _objCommon = new Common();

                _objCommon.WriteToFile(_objCommon.GetLogPath(),
                                        string.Format("{0} DBPool-Catch - {1} - {2} - 000",
                                        Convert.ToString(DateTime.Now), ex.Message,
                                        System.Net.Dns.GetHostName()), true);

                _strLastError = ex.Message;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
                da = null;
                _objCommon = null;
            }

            return bReturn;
        }

        /// <summary>
        /// Execute Query with specific timeout and nRowsAffected as output
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="nRowsAffected"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public bool ExecuteDml(string sqlQuery, ref int nRowsAffected, int timeOut)
        {
            DbCommand cmd = null;
            bool bReturn = false;

            try
            {
                if (_adoConnection == null)
                {
                    _strLastError = ErrorMessage.DbConnectionNotOpened;
                    return false;
                }

                //Create Command Instance with respective SQLQuery or SPName
                CreateCmdInstance(sqlQuery, ref cmd);

                if (timeOut > 0)
                {
                    cmd.CommandTimeout = timeOut;
                }

                nRowsAffected = cmd.ExecuteNonQuery();
                bReturn = true;
            }
            catch (Exception ex)
            {
                _objCommon = new Common();

                _objCommon.WriteToFile(_objCommon.GetLogPath(),
                                        string.Format("{0} DBPool-Catch - {1} - {2} - 000",
                                        Convert.ToString(DateTime.Now), ex.Message,
                                        System.Net.Dns.GetHostName()), true);

                _strLastError = ex.Message;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
                _objCommon = null;
            }

            return bReturn;
        }

        /// <summary>
        /// Execute Scalar Query and Get Scalar object as Output
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="retObject"></param>
        /// <returns>bool</returns>
        public bool ExecuteScalarQuery(string sqlQuery, ref object retObject)
        {
            DbCommand cmd = null;
            bool bReturn = false;

            try
            {
                if (_adoConnection == null)
                {
                    _strLastError = ErrorMessage.DbConnectionNotOpened;
                    return false;
                }

                //Create Command Instance with respective SQLQuery or SPName
                CreateCmdInstance(sqlQuery, ref cmd);

                retObject = cmd.ExecuteScalar();
                bReturn = true;
            }
            catch (Exception ex)
            {
                _objCommon = new Common();

                _objCommon.WriteToFile(_objCommon.GetLogPath(),
                                        string.Format("{0} DBPool-Catch - {1} - {2} - 000",
                                        Convert.ToString(DateTime.Now), ex.Message,
                                        System.Net.Dns.GetHostName()), true);

                _strLastError = ex.Message;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
                _objCommon = null;
            }

            return bReturn;
        }

        /// <summary>
        /// Execury Stored Procedure by passing parameters
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="inParams"></param>
        /// <returns>bool</returns>
        public bool SpQueryExecuteNonQuery(string spName, SqlParameter[] inParams)
        {
            try
            {
                if (OpenConnection())
                {
                    bool bReturn = false;
                    SqlCommand cmd = null;
                    SqlParameter objParam = null;

                    try
                    {
                        objParam = default(SqlParameter);

                        if (_adoConnection == null)
                        {
                            _strLastError = ErrorMessage.DbConnectionNotOpened;
                            return false;
                        }

                        //Create Command Instance with respective SQLQuery or SPName
                        CreateCmdInstanceSql(spName, ref cmd);

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = intTimeOut; // increased the timeout for SQL Queries

                        if (inParams != null)
                        {
                            foreach (DbParameter dbPar in inParams)
                            {
                                objParam = ((SqlCommand)cmd).Parameters.Add((SqlParameter)dbPar);
                                objParam.Direction = ParameterDirection.Input;
                            }
                        }

                        cmd.ExecuteNonQuery();

                        bReturn = true;
                    }
                    catch (Exception ex)
                    {
                        _objCommon = new Common();

                        _objCommon.WriteToFile(_objCommon.GetLogPath(),
                                                string.Format("{0} DBPool-Catch - {1} - {2} - {3} - 000",
                                                Convert.ToString(DateTime.Now), ex.Message, spName,
                                                System.Net.Dns.GetHostName()), true);

                        _strLastError = ex.Message;
                    }
                    finally
                    {
                        cmd = null;
                        _objCommon = null;
                    }

                    return bReturn;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Execute Stored Procedure by getting resultSet as output 
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="resultSet"></param>
        /// <returns>bool</returns>
        public bool SpQueryDataset(string spName, ref DataSet resultSet)
        {
            try
            {
                if (OpenConnection())
                {
                    bool bReturn = false;
                    SqlCommand cmd = null;
                    SqlDataAdapter objDA = null;

                    try
                    {
                        objDA = default(SqlDataAdapter);

                        if (_adoConnection == null)
                        {
                            _strLastError = ErrorMessage.DbConnectionNotOpened;
                            return false;
                        }

                        //Create Command Instance with respective SQLQuery or SPName
                        CreateCmdInstanceSql(spName, ref cmd);

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = intTimeOut; // increased the timeout for SQL Queries

                        resultSet = new DataSet();
                        objDA = new SqlDataAdapter((SqlCommand)cmd);
                        objDA.Fill(resultSet);
                        bReturn = true;
                    }
                    catch (Exception ex)
                    {
                        _objCommon = new Common();

                        _objCommon.WriteToFile(_objCommon.GetLogPath(),
                                               string.Format("{0} DBPool-Catch - {1} - {2} - {3} - 000",
                                                             Convert.ToString(DateTime.Now), ex.Message, spName,
                                                             System.Net.Dns.GetHostName()), true);
                        _strLastError = ex.Message;
                    }
                    finally
                    {
                        objDA = null;
                        cmd = null;
                        _objCommon = null;
                    }

                    return bReturn;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Execury Stored Procedure by passing parameters and getting resultSet as output
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="inParams"></param>
        /// <param name="resultSet"></param>
        /// <returns>bool</returns>
        public bool SpQueryDataset(string spName, SqlParameter[] inParams, ref DataSet resultSet)
        {
            try
            {
                if (OpenConnection())
                {
                    bool bReturn = false;
                    SqlCommand cmd = null;
                    SqlParameter objParam = null;
                    SqlDataAdapter objDA = null;

                    try
                    {
                        objParam = default(SqlParameter);
                        objDA = default(SqlDataAdapter);

                        if (_adoConnection == null)
                        {
                            _strLastError = ErrorMessage.DbConnectionNotOpened;
                            return false;
                        }

                        //Create Command Instance with respective SQLQuery or SPName
                        CreateCmdInstanceSql(spName, ref cmd);

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = intTimeOut; // increased the timeout for SQL Queries

                        if (inParams != null)
                        {
                            foreach (DbParameter dbPar in inParams)
                            {
                                objParam = ((SqlCommand)cmd).Parameters.Add((SqlParameter)dbPar);
                                objParam.Direction = ParameterDirection.Input;
                            }

                        }

                        resultSet = new DataSet();
                        objDA = new SqlDataAdapter((SqlCommand)cmd);
                        objDA.Fill(resultSet);
                        bReturn = true;
                    }
                    catch (Exception ex)
                    {
                        _objCommon = new Common();

                        _objCommon.WriteToFile(_objCommon.GetLogPath(),
                                               string.Format("{0} DBPool-Catch - {1} - {2} - {3} - 000",
                                                             Convert.ToString(DateTime.Now), ex.Message, spName,
                                                             System.Net.Dns.GetHostName()), true);
                        _strLastError = ex.Message;
                    }
                    finally
                    {
                        objDA = null;
                        cmd = null;
                        _objCommon = null;
                    }

                    return bReturn;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Execury Stored Procedure by passing parameters and getting output parameter values
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="inParams"></param>
        /// <param name="outParams"></param>
        /// <param name="isNonQuery"></param>
        /// <returns>bool</returns>
        public bool SpQueryOutputParam(string spName, SqlParameter[] inParams, ref SqlParameter[] outParams, bool isNonQuery)
        {
            try
            {
                if (OpenConnection())
                {
                    bool bReturn = false;
                    SqlCommand cmd = null;
                    SqlParameter objParam = null;

                    try
                    {
                        objParam = default(SqlParameter);

                        if (_adoConnection == null)
                        {
                            _strLastError = ErrorMessage.DbConnectionNotOpened;
                            return false;
                        }

                        //Create Command Instance with respective SQLQuery or SPName
                        CreateCmdInstanceSql(spName, ref cmd);

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = intTimeOut; // increased the timeout for SQL Queries

                        if (inParams != null)
                        {
                            foreach (DbParameter dbPar in inParams)
                            {
                                objParam = ((SqlCommand)cmd).Parameters.Add((SqlParameter)dbPar);
                                objParam.Direction = ParameterDirection.Input;
                            }
                        }

                        if (outParams != null)
                        {
                            foreach (DbParameter dbPar in outParams)
                            {
                                objParam = ((SqlCommand)cmd).Parameters.Add((SqlParameter)dbPar);
                                objParam.Direction = ParameterDirection.Output;
                            }
                        }

                        if (isNonQuery)
                        {
                            cmd.ExecuteNonQuery();
                        }

                        bReturn = true;
                    }
                    catch (Exception ex)
                    {
                        _objCommon = new Common();

                        _objCommon.WriteToFile(_objCommon.GetLogPath(),
                                                string.Format("{0} DBPool-Catch - {1} - {2} - {3} - 000",
                                                Convert.ToString(DateTime.Now), ex.Message, spName,
                                                System.Net.Dns.GetHostName()), true);

                        _strLastError = ex.Message;
                    }
                    finally
                    {
                        cmd = null;
                        _objCommon = null;
                    }

                    return bReturn;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Execury Stored Procedure by passing parameters and getting output parameter values, resultset
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="inParams"></param>
        /// <param name="outParams"></param>
        /// <param name="isNonQuery"></param>
        /// <param name="resultSet"></param>
        /// <returns>bool</returns>
        public bool SpQueryOutputParam(string spName, SqlParameter[] inParams, ref SqlParameter[] outParams, bool isNonQuery, ref DataSet resultSet)
        {
            bool bReturn = false;
            SqlCommand cmd = null;
            SqlParameter objParam = null;
            SqlDataAdapter objDA = null;

            try
            {
                if (OpenConnection())
                {

                objParam = default(SqlParameter);
                objDA = default(SqlDataAdapter);

                if (_adoConnection == null)
                {
                    _strLastError = ErrorMessage.DbConnectionNotOpened;
                    return false;
                }

                //Create Command Instance with respective SQLQuery or SPName
                CreateCmdInstanceSql(spName, ref cmd);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = intTimeOut; // increased the timeout for SQL Queries

                if (inParams != null)
                {
                    foreach (DbParameter dbPar in inParams)
                    {
                        objParam = ((SqlCommand)cmd).Parameters.Add((SqlParameter)dbPar);
                        objParam.Direction = ParameterDirection.Input;
                    }
                }

                if (outParams != null)
                {
                    foreach (DbParameter dbPar in outParams)
                    {
                        objParam = ((SqlCommand)cmd).Parameters.Add((SqlParameter)dbPar);
                        objParam.Direction = ParameterDirection.Output;
                    }
                }
                if (isNonQuery)
                {
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    resultSet = new DataSet();
                    objDA = new SqlDataAdapter((SqlCommand)cmd);
                    objDA.Fill(resultSet);
                }

                bReturn = true;
            }
        }
            catch (Exception ex)
            {
                _objCommon = new Common();

                _objCommon.WriteToFile(_objCommon.GetLogPath(),
                                        string.Format("{0} DBPool-Catch - {1} - {2} - {3} - 000",
                                        Convert.ToString(DateTime.Now), ex.Message, spName,
                                        System.Net.Dns.GetHostName()), true);

                _strLastError = ex.Message;
            }
            finally
            {
                objDA = null;
                cmd = null;
                _objCommon = null;
                CloseConnection();
            }

            return bReturn;
        }

        /// <summary>
        /// Execury Stored Procedure by passing parameters and getting return values as output
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="inParams"></param>
        /// <param name="retValues"></param>
        /// <param name="isNonQuery"></param>
        /// <returns>bool</returns>
        public bool SpQueryReturnValue(string spName, SqlParameter[] inParams, ref SqlParameter[] retValues, bool isNonQuery)
        {
            try
            {
                if (OpenConnection())
                {
                    bool bReturn = false;
                    SqlCommand cmd = null;
                    SqlParameter objParam = null;

                    try
                    {
                        objParam = default(SqlParameter);

                        if (_adoConnection == null)
                        {
                            _strLastError = ErrorMessage.DbConnectionNotOpened;
                            return false;
                        }

                        //Create Command Instance with respective SQLQuery or SPName
                        CreateCmdInstanceSql(spName, ref cmd);

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = intTimeOut; // increased the timeout for SQL Queries

                        if (inParams != null)
                        {
                            foreach (DbParameter dbPar in inParams)
                            {
                                objParam = ((SqlCommand)cmd).Parameters.Add((SqlParameter)dbPar);
                                objParam.Direction = ParameterDirection.Input;
                            }
                        }

                        if (retValues != null)
                        {
                            foreach (DbParameter dbPar in retValues)
                            {
                                objParam = ((SqlCommand)cmd).Parameters.Add((SqlParameter)dbPar);
                                objParam.Direction = ParameterDirection.ReturnValue;
                            }
                        }

                        if (isNonQuery)
                        {
                            cmd.ExecuteNonQuery();
                        }

                        bReturn = true;
                    }
                    catch (Exception ex)
                    {
                        _objCommon = new Common();

                        _objCommon.WriteToFile(_objCommon.GetLogPath(),
                                                string.Format("{0} DBPool-Catch - {1} - {2} - {3} - 000",
                                                Convert.ToString(DateTime.Now), ex.Message, spName,
                                                System.Net.Dns.GetHostName()), true);

                        _strLastError = ex.Message;
                    }
                    finally
                    {
                        cmd = null;
                        _objCommon = null;
                    }

                    return bReturn;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        #endregion

        
    }
}

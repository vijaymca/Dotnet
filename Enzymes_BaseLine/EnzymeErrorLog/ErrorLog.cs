using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Data;


namespace EnzymeErrorLog
{
    public class ErrorLog
    {
        /// <summary>
        ///  Type of Errors
        /// </summary>
        public static string m_strLastError;

        public static string strSqlCon = ConfigurationSettings.AppSettings["dbConnString"].ToString();

        public enum LogMessageType
        {
            LogError,
            LogInformation,
        }

        public static string m_strErrorMessage;

        public static bool SaveErrorLog(string strModuleName, string strLogType, string strErrorMsg, string strSPName, string strHostName, string strUsername, LogMessageType strLogMessageType)
        {
            bool bReturn = false;            
            int intErrorLevel = 1;
            try
            {
                intErrorLevel = Convert.ToInt32(ExecuteScalar("USP_CBD_Get_ErrorLevel"));
            }
            catch (Exception) { }           

            if (intErrorLevel == 0)
                return bReturn;
            if (intErrorLevel == 1 && strLogMessageType == LogMessageType.LogInformation)
                return bReturn;
            if (intErrorLevel == 1 && strLogMessageType == LogMessageType.LogError)
                bReturn = LogIt(strModuleName, strLogType, strErrorMsg, strSPName, strHostName, strUsername, strLogMessageType);
            if (intErrorLevel == 2)
                bReturn = LogIt(strModuleName, strLogType, strErrorMsg, strSPName, strHostName, strUsername, strLogMessageType);

            //Write to text log here based on the return type.
            if (bReturn)
            {
                //
                string strFilePath = GetLogPath();
                WriteToFile(strFilePath, strErrorMsg, true);
            }
            return bReturn;
        }

        private static bool LogIt(string strModuleName, string strLogType, string strErrorMsg, string strSPName, string strHostName, string strUsername, LogMessageType strLogMessageType)
        {            
            SqlParameter[] paramIn = new SqlParameter[6];
            bool Count = false;
            try
            {
                paramIn[0] = new SqlParameter("@p_Module_NM", strModuleName);
                paramIn[1] = new SqlParameter("@p_LogType_CD", strLogType);
                paramIn[2] = new SqlParameter("@p_Error_TXT", strErrorMsg);
                paramIn[3] = new SqlParameter("@p_Procedure_NM", strSPName);
                paramIn[4] = new SqlParameter("@p_Host_NM", strHostName);
                paramIn[5] = new SqlParameter("@p_User_NM", strUsername);

                Count = ExecuteNonQuery("USP_CBD_Ins_Error_Log", paramIn);
              
            }
            catch (Exception ex)
            {
                m_strErrorMessage = ex.StackTrace;
            }
            finally
            {
                paramIn = null;                
            }
            return Count;
        }

        public static string GetLogPath()
        {
            string strFileName = string.Empty;
            try
            {
                StringBuilder sbFileName = new StringBuilder();
                sbFileName.Append(DateTime.Now.Date.Month.ToString());
                sbFileName.Append("-");
                sbFileName.Append(DateTime.Now.Date.Day.ToString());
                sbFileName.Append("-");
                sbFileName.Append(DateTime.Now.Date.Year.ToString());
                strFileName = HttpContext.Current.Server.MapPath("~");
                strFileName = strFileName + "/" + sbFileName.ToString() + ".txt";
            }
            catch (Exception) { }
            return strFileName;
        }

        public static bool WriteToFile(string strFilePath, string strFileData, bool bAppend)
        {
            bool bReturn = false;
            StreamWriter objSWriter = default(StreamWriter);
            FileInfo objFile = null;
            try
            {
                objFile = new FileInfo(strFilePath);
                if (!objFile.Exists) objFile.Create();
                objSWriter = new StreamWriter(strFilePath, bAppend);
                objSWriter.Write(System.Environment.NewLine + strFileData);
                bReturn = true;
            }
            catch (Exception ex)
            {
                bReturn = false;
                m_strLastError = ex.Message;
            }
            finally
            {
                if ((objSWriter != null))
                {
                    objSWriter.Close();
                }
                objSWriter = null;
                objFile = null;
            }
            return bReturn;
        }

        public static string GetLastError()
        {
            //Return last error message here
            return m_strLastError;
        }

        private static bool ExecuteNonQuery(string spName, SqlParameter[] @params)
        {
            SqlConnection sqlCon = new SqlConnection(strSqlCon);
            int retval = 0;
            SqlCommand cmd = new SqlCommand(spName, sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;

            for (int i = 0; i <= @params.Length - 1; i++)
            {
                cmd.Parameters.Add(@params[i]);
            }

            try
            {
                sqlCon.Open();
                retval = cmd.ExecuteNonQuery();
                sqlCon.Close();
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }
            finally
            {
                if ((sqlCon.State == ConnectionState.Open))
                {
                    sqlCon.Close();
                }
                sqlCon.Dispose();
            }
            if (retval != 0)
                return true;
            else
                return false;
        }

        public static object ExecuteScalar(string spName)
        {
            object retval = null;
            SqlConnection sqlCon = new SqlConnection(strSqlCon);
            SqlCommand cmd = new SqlCommand(spName, sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 240;
            try
            {
                sqlCon.Open();
                retval = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }
            finally
            {
                if ((sqlCon.State == ConnectionState.Open))
                {
                    sqlCon.Close();
                }
                sqlCon.Dispose();
            }
            return retval;
        }
    }
}

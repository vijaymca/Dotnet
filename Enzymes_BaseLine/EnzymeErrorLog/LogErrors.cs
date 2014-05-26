using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace EnzymeErrorLog
{
    public class LogErrors
    {
        /*
         
        public static string strSqlCon = ConfigurationSettings.AppSettings["dbConnString"].ToString();

        public static void SaveError(string strUserName,string strSourceName, string strFunctionName, string strErrorCode, string strErrorMessage)
        {
            int intErrorLevel = 1;
            try
            {
                intErrorLevel= Convert.ToInt32(ExecuteScalar("USP_CBD_Get_ErrorLevel"));               
            }
            catch (Exception) { }
            if (intErrorLevel != 0)
            {
                try
                {
                    SqlParameter[] param = new SqlParameter[5];
                    param[0] = new SqlParameter("@SourceName", SqlDbType.VarChar);
                    param[0].Value = strSourceName;
                    param[1] = new SqlParameter("@FunctionName", SqlDbType.VarChar);
                    param[1].Value = strFunctionName;
                    param[2] = new SqlParameter("@ErrorCode", SqlDbType.VarChar);
                    param[2].Value = strErrorCode;
                    param[3] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar);
                    param[3].Value = strErrorMessage;
                    param[4] = new SqlParameter("@UserName", SqlDbType.VarChar);
                    param[4].Value = strUserName;
                    ExecuteNonQuery("USP_CBD_Ins_Erros", param);
                }
                catch (Exception ed)
                {
                    string s = ed.Message;
                }
            }
        }

        public static void SaveErrorLog(string strModuleName, string strLogType, string strErrorMsg, string strSPName, string strHostName, string strUsername)
        {
            int intErrorLevel = Convert.ToInt16(ConfigurationSettings.AppSettings["ErrorLevel"]);

            if (intErrorLevel == 0) 
            {
                //return bReturn;
            }
                

        }
        
        public static void LogIt(string strModuleName, string strLogType, string strErrorMsg, string strSPName, string strHostName, string strUsername)
        {
            bool bReturn = false;
            SqlParameter[] paramIn = new SqlParameter[6];            
            try
            {
                paramIn[0] = new SqlParameter("@p_Module_NM", strModuleName);
                paramIn[1] = new SqlParameter("@p_LogType_CD", strLogType);
                paramIn[2] = new SqlParameter("@p_Error_TXT", strErrorMsg);
                paramIn[3] = new SqlParameter("@p_Procedure_NM", strSPName);
                paramIn[4] = new SqlParameter("@p_Host_NM", strHostName);
                paramIn[5] = new SqlParameter("@p_User_NM", strUsername);
                ExecuteNonQuery("USP_CBD_Ins_Error_Log", paramIn);                                                  
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
               
            }            
        }

        private static void ExecuteNonQuery(string spName, SqlParameter[] @params)
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
                LogErrors.SaveError(Convert.ToString(System.Threading.Thread.CurrentPrincipal.Identity.Name),"DataBaseHelper.cs", "ExecuteScalar", "", ex.Message.ToString());
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

        */
    }
}

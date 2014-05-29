using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PTFQA_DBPool;
using System.Data.SqlClient;
using System.Data;

namespace PTFQA_DAL
{
    public class clsPTFQA_DAL
    {
        String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PTFQADB"].ConnectionString;

        DBPool objDBPool = null;
        SqlParameter[] paramIn = null;
        SqlParameter[] paramOut = null;

        /// <summary>
        /// Constructor to initialize the DB Connection
        /// </summary>
        public clsPTFQA_DAL()
        {
            objDBPool = new DBPool();
            objDBPool.ConnectionString = ConnectionString;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="regID"></param>
        /// <param name="objDS"></param>
        /// <returns></returns>
        public bool getCountryDetailsDAL(int regID, ref DataSet objDS)
        {
            try
            {
                paramIn = new SqlParameter[1];
                paramIn[0] = new SqlParameter("@p_RegionID", SqlDbType.Int);
                paramIn[0].Value = regID;

                if (objDBPool.SpQueryDataset("USP_GET_RegionCountry", paramIn, ref objDS))
                    return true;
                else return false;
            }
            finally
            {
                objDBPool = null;
            }
        }
    }
}

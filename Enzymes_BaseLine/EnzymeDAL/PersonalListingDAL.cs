//-----------------------------------------------------------------------
// <copyright file="PersonalListingDAL.cs" company="P&G">
// <author>Archana Priya B</author>  
// </copyright>
//-----------------------------------------------------------------------
#region Change Log
/// <changelog>
///   <item who="Archana" when="05-22-2014">Personnel Listing DAL class</item>
/// </changelog> 
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using EnzymeDBPool;
using System.Configuration;


namespace EnzymeDAL
{
    public class PersonalListingDAL
    {
		public string strConn = ConfigurationManager . ConnectionStrings [ "EnzymeConn" ] . ConnectionString;

        #region Global Declarations
		DBPool objDBPool = null;
		string strLastError;
       // DBPool SqlHelper = new DBPool();
        
        #endregion

		public PersonalListingDAL ( )
        {
            objDBPool = new DBPool();
			objDBPool . ConnectionString = strConn;
        }

        #region send data to RTCIS
         
        /// <summary>
        ///  to get Personnel Listing details.
        /// </summary>        
        /// <param name="strUser"></param>
        /// <returns></returns>
        public DataSet GetPersonnelListings(string strUser)
        {
            SqlParameter[] paramIn = new SqlParameter[0];
            DataSet dsPersonnelListingInfo = new DataSet();
            try
            {
               // paramIn[0] = new SqlParameter("@P_PurchaseorderId", strPOMaterialID);
               // paramIn[1] = new SqlParameter("@P_UserName", strUser);


				if ( objDBPool . SPQueryDatasetNoParams ( strConn , "USP_GET_lst_HSUPersons_All_Data" , ref dsPersonnelListingInfo ) )
                {
                    return dsPersonnelListingInfo;
                }
                else
                {
					strLastError = objDBPool . GetLastError ( );
                }

            }
            catch (Exception ex)
            { 
                //Set the error to this variable
                strLastError = ex.StackTrace;
            }
            finally
            {
                paramIn = null;
				objDBPool = null;
            }

            //Return status here
            return dsPersonnelListingInfo;
        }
		/// <summary>
		///  to get Personnel Listing details.
		/// </summary>        
		/// <param name="strUser"></param>
		/// <returns></returns>
		public DataSet GetSelectedPersonsData ( string strUser )
		{
			SqlParameter[] paramIn = new SqlParameter [ 1 ];
			DataSet dsPersonnelListingInfo = new DataSet ( );
			try
			{
				paramIn [ 0 ] = new SqlParameter ( "@p_HSUPersons_ID" , strUser );
				//paramIn [ 1 ] = new SqlParameter ( "@P_UserName" , strUser );


				if ( objDBPool .SpQueryDataset( "USP_GET_lst_HSUPersons_BasedOnID" ,paramIn, ref dsPersonnelListingInfo ) )
				{
					return dsPersonnelListingInfo;
				}
				else
				{
					strLastError = objDBPool . GetLastError ( );
				}

			}
			catch ( Exception ex )
			{
				//Set the error to this variable
				strLastError = ex . StackTrace;
			}
			finally
			{
				paramIn = null;
				objDBPool = null;
			}

			//Return status here
			return dsPersonnelListingInfo;
		}           
        #endregion
    }
}

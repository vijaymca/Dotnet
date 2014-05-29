//-----------------------------------------------------------------------
// <copyright file="PersonnelListingsBL.cs" company="P&G">
// <author>Archana Priya B</author>  
// </copyright>
//-----------------------------------------------------------------------
#region Change Log
/// <changelog>
///   <item who="Archana" when="10-28-2013">PersonnelListings BL class</item>
/// </changelog> 
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using EnzymeDAL;

namespace EnzymeBAL
{
   public class PersonnelListingsBL
    {
        #region Global Decalration

        PersonalListingDAL objrtcisdata = new PersonalListingDAL();

        #endregion

        #region GetLastError

        string m_strLastError = string.Empty ;

        public string GetLastError()
        {
            //Return last error message here
            return m_strLastError;
        }

        #endregion

        #region send data to RTCIS

        /// <summary>
        ///  to get Personnel Listing details.
        /// </summary>
        /// <param name="strUser"></param>
        /// <returns></returns>
        public DataSet GetPersonnelListings(string strUser)
        {
            DataSet dsMatCalledPOInfo = new DataSet();
            dsMatCalledPOInfo = objrtcisdata.GetPersonnelListings(strUser);
            return dsMatCalledPOInfo;
        }

		/// <summary>
		///  to get Personnel Listing details.
		/// </summary>
		/// <param name="strUser"></param>
		/// <returns></returns>
		public DataSet GetSelectedPersonsData ( string strUser )
		{
			DataSet dsMatCalledPOInfo = new DataSet ( );
			dsMatCalledPOInfo = objrtcisdata . GetSelectedPersonsData ( strUser );
			return dsMatCalledPOInfo;
		}

       
        #endregion
    }
}

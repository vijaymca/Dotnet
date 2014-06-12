#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: StaffDetails.cs 
#endregion
using System;

namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// Gets/Sets the properties of Team Staff
    /// </summary>
    public class StaffDetails
    {
        #region DECLARATION
        //Team_ID
        //User_Name
        //User_ID
        //User_Rank
        //ID
        //Discipline       
        string strUserName = string.Empty;
        string strUserID = string.Empty;
        string strDiscipline = string.Empty;
        string strTeamID = string.Empty;     
        string strUserRank = string.Empty;
        string rowId = string.Empty;
        string strPrivilege = string.Empty;
        #endregion DECLARATION

        #region PROPERTIES

        /// <summary>
        /// Gets/Sets Staff ID in team
        /// </summary>
        public string RowID
        {
            get { return rowId; }
            set { rowId = value; }
        }
        /// <summary>
        /// Gets/Sets User Name.
        /// </summary>
        public string UserName
        {
            get { return strUserName; }
            set { strUserName = value; }
        }
        /// <summary>
        /// Gets/Sets User ID.
        /// </summary>
        public string UserID
        {
            get { return strUserID; }
            set { strUserID = value; }
        }
        /// <summary>
        /// Gets/Sets Team ID.
        /// </summary>
        public string TeamID
        {
            get { return strTeamID; }
            set { strTeamID = value; }
        }       
        /// <summary>
        /// Gets/Sets User Rank with in a team and discipline.
        /// </summary>
        public string UserRank
        {
            get { return strUserRank; }
            set { strUserRank = value; }
        }

        /// <summary>
        /// Gets/Sets User discipline.
        /// </summary>
        public string Discipline
        {
            get { return strDiscipline; }
            set { strDiscipline = value; }
        }

        /// <summary>
        /// Gets/Sets Privileges of the Staff
        /// </summary>
        public string PRIVILEGE
        {
            get
            {
                return strPrivilege;
            }
            set
            {
                strPrivilege = value;
            }
        }
        #endregion PROPERTIES
    }
}

#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: SystemPrivileges.cs 
#endregion
using System;



namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// Gets/Sets Privileges of a DWB User
    /// </summary>
    [Serializable]
    public class SystemPrivileges
    {
        #region DECLARATION

        private bool blnAdminPrivilege;
        private bool blnBookOwner;
        private bool blnPageOwner;
        private bool blnDWBUser;
        private int intRowId;
        private string strDisciplineID = string.Empty;
        private string strDiscipline = string.Empty;
        #endregion

        #region PROPERTIES

        /// <summary>
        /// Gets or sets the user record ID.
        /// </summary>
        /// <value>The user record ID.</value>
        public int UserRecordID
        {
            get { return intRowId; }
            set { intRowId = value; }
        }

        /// <summary>
        /// Gets or sets the discipline ID.
        /// </summary>
        /// <value>The discipline ID.</value>
        public string DisciplineID
        {
            get { return strDisciplineID; }
            set { strDisciplineID = value; }
        }

        /// <summary>
        /// Gets or sets the discipline.
        /// </summary>
        /// <value>The discipline.</value>
        public string Discipline
        {
            get { return strDiscipline; }
            set { strDiscipline = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [admin privilege].
        /// </summary>
        /// <value><c>true</c> if [admin privilege]; otherwise, <c>false</c>.</value>
        public bool AdminPrivilege
        {
            get { return blnAdminPrivilege; }
            set { blnAdminPrivilege = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [page owner].
        /// </summary>
        /// <value><c>true</c> if [page owner]; otherwise, <c>false</c>.</value>
        public bool PageOwner
        {
            get { return blnPageOwner; }
            set { blnPageOwner = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [book owner].
        /// </summary>
        /// <value><c>true</c> if [book owner]; otherwise, <c>false</c>.</value>
        public bool BookOwner
        {
            get { return blnBookOwner; }
            set { blnBookOwner = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [DWB user].
        /// </summary>
        /// <value><c>true</c> if [DWB user]; otherwise, <c>false</c>.</value>
        public bool DWBUser
        {
            get { return blnDWBUser; }
            set { blnDWBUser = value; }
        }
        #endregion
    }
}

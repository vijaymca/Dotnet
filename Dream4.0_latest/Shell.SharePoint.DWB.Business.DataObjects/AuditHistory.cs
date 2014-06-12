#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: AuditHistory.cs 
#endregion

using System;

namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// This class contains the audit history information of the listitem.
    /// </summary>
    [Serializable]
    public class AuditHistory
    {
        #region DECLARATION
        string strUserName = string.Empty;
        DateTime dtmCurrentDate = DateTime.Now;
        string strRecordNumber = string.Empty;
        #endregion

        #region PROPERTIES
        /// <summary>
        /// Gets or Sets the user
        /// </summary>
        public string User
        {
            get { return strUserName; }
            set { strUserName = value; }
        }

        /// <summary>
        /// Gets or Sets the Date
        /// </summary>
        public DateTime Date
        {
            get { return dtmCurrentDate; }
            set { dtmCurrentDate = value; }
        }

        /// <summary>
        /// Gets or Sets the RecordNumber
        /// </summary>
        public string RecordNumber
        {
            get { return strRecordNumber; }
            set { strRecordNumber = value; }
        }
        #endregion
    }
}

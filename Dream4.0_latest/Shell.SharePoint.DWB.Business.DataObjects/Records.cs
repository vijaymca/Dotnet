#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: Records.cs
#endregion

using System.Collections;

namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// Records class
    /// </summary>
    public class Records
    {
        #region DECLARATION
        string strName = string.Empty;
        ArrayList arlRecord;
        #endregion
        #region PROPERTIES
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return strName; }
            set { strName = value; }
        }
        /// <summary>
        /// Gets or sets the record.
        /// </summary>
        /// <value>The record.</value>
        public ArrayList Record
        {
            get { return arlRecord; }
            set { arlRecord = value; }
        }
        #endregion
    }
}

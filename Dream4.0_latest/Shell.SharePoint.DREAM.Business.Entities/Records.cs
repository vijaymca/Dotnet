#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: Records.cs 
#endregion
using System;
using System.Collections;

namespace Shell.SharePoint.DREAM.Business.Entities
{
    /// <summary>
    /// The Records class for storing the Record items
    /// </summary>
    public class Records
    {
        #region DECLARATION
        ArrayList arlRecords = null;
        string strName = string.Empty;
        #endregion


        #region METHODS


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
        public ArrayList RecordCollection
        {
            get { return arlRecords; }
            set { arlRecords = value; }
        }

        #endregion

    }
}

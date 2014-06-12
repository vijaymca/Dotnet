#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: BatchImport.cs 
#endregion
using System;
using System.Collections;

namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// Batch Import class which holds the Page Name details.
    /// </summary>
    [Serializable]
    public class BatchImport
    {
        #region DECLARATION
        ArrayList arlPageName;
        string strBookID;
        string strUserName;
        string strDefaultSharedPath;
        #endregion
        #region PROPERTIES
        /// <summary>
        /// Gets or sets the batch import.
        /// </summary>
        /// <value>The batch import.</value>
        public ArrayList PageName
        {
            get { return arlPageName; }
            set { arlPageName = value; }
        }
        /// <summary>
        /// Gets or sets the book ID.
        /// </summary>
        /// <value>The book ID.</value>
        public string BookID
        {
            get { return strBookID; }
            set { strBookID = value; }
        }
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName
        {
            get { return strUserName; }
            set { strUserName = value; }
        }
        /// <summary>
        /// Gets or sets the default shared path.
        /// </summary>
        /// <value>The default shared path.</value>
        public string DefaultSharedPath
        {
            get { return strDefaultSharedPath; }
            set { strDefaultSharedPath = value; }
        }
        #endregion
    }
}

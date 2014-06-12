#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: BatchImports.cs 
#endregion
using System;
using System.Collections;

namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// BatchImports class to maintain the configurations.
    /// </summary>
    [Serializable]
    public class BatchImports
    {
        #region DECLARATION
        ArrayList arlBatchImport;        
        #endregion
        #region PROPERTIES

        /// <summary>
        /// Gets or sets the batch import.
        /// </summary>
        /// <value>The batch import.</value>
        public ArrayList BatchImport
        {
            get { return arlBatchImport; }
            set { arlBatchImport = value; }
        }
        #endregion
    }
}
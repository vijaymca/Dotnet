#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: FileType.cs 
#endregion
using System;

namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// File Type
    /// </summary>
    [Serializable]
    public class FileType
    {
        #region DECLARATION
        string strType;
        #endregion
        #region PROPERTIES

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type
        {
            get { return strType; }
            set { strType = value; }
        }
        #endregion
    }
}

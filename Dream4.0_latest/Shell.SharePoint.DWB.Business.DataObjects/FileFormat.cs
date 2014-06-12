#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: FileFormat.cs 
#endregion
using System;

namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// File format to keep the format.
    /// </summary>
    [Serializable]
    public class FileFormat
    {
        #region DECLARATION
        string strFormat;
        string strActualFormat;
        #endregion
        #region PROPERTIES

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>The format.</value>
        public string Format
        {
            get { return strFormat; }
            set { strFormat = value; }
        }

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>The format.</value>
        public string ActualFormat
        {
            get { return strActualFormat; }
            set { strActualFormat = value; }
        }
        #endregion
    }
}

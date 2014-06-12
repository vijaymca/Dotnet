#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: PageName.cs 
#endregion
using System;

namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// Page Name class to hold details about each page name.
    /// </summary>
    [Serializable]
    public class PageName
    {
        #region DECLARATION
        string strName;
        string strPageCount;
        SharedPath objSharePath;
        FileFormat objFileFormat;
        FileType objFileType;
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
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string PageCount
        {
            get { return strPageCount; }
            set { strPageCount = value; }
        }
        /// <summary>
        /// Gets or sets the shared path.
        /// </summary>
        /// <value>The shared path.</value>
        public SharedPath SharedPath
        {
            get { return objSharePath; }
            set { objSharePath = value; }
        }
        /// <summary>
        /// Gets or sets the file format.
        /// </summary>
        /// <value>The file format.</value>
        public FileFormat FileFormat
        {
            get { return objFileFormat; }
            set { objFileFormat = value; }
        }
        /// <summary>
        /// Gets or sets the type of the file.
        /// </summary>
        /// <value>The type of the file.</value>
        public FileType FileType
        {
            get { return objFileType; }
            set { objFileType = value; }
        }
        #endregion
    }
}

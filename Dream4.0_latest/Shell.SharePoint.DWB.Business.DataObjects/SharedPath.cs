#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: SharedPath.cs 
#endregion
using System;

namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// Shared Path class to save the path for each page name.
    /// </summary>
    [Serializable]
    public class SharedPath
    {
        #region DECLARATION
        string strPath;
        #endregion
        #region PROPERTIES
        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path
        {
            get { return strPath; }
            set { strPath = value; }
        }
        #endregion
    }
}

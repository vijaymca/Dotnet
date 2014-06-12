#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: URL.cs 
#endregion

/// <summary> 
/// This class has get/set methods to handle user defined links and cannot be inherited.
/// </summary> 
using System;

namespace Shell.SharePoint.DREAM.Business.Entities
{
    /// <summary>
    /// The URL class
    /// </summary>
    [Serializable]
    public class URL
    {
        #region DECLARATION
        private string strURLTitle = string.Empty;
        private string strURLValue = string.Empty;
        #endregion
        #region PROPERTIES
        /// <summary>
        /// Gets or sets the URL title.
        /// </summary>
        /// <value>The URL title.</value>
        public string URLTitle
        {
            get
            {
                return strURLTitle;
            }
            set
            {
                strURLTitle = value;
            }
        }

        /// <summary>
        /// Gets or sets the URL text value.
        /// </summary>
        /// <value>The URL value.</value>
        public string URLValue
        {
            get
            {
                return strURLValue;
            }
            set
            {
                strURLValue = value;
            }
        }
        #endregion
    }
}

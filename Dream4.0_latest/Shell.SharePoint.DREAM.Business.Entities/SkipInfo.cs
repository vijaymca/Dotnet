#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: SkipInfo.cs 
#endregion

/// <summary> 
/// This is used to get/set SkipInfo.
/// </summary>

namespace Shell.SharePoint.DREAM.Business.Entities
{
    /// <summary>
    /// Value Class.
    /// </summary>
    public class SkipInfo
    {
        #region DECLARATION        
        string strSkipRecordCount = string.Empty;
        string strMaxRecordfetch = string.Empty;
        #endregion
        #region PROPERTIES        
        /// <summary>
        /// Gets or sets the inner text.
        /// </summary>
        /// <value>The inner text.</value>
        public string SkipRecord
        {
            get
            {
                return strSkipRecordCount;
            }
            set
            {
                strSkipRecordCount = value;
            }
        }
        /// <summary>
        /// Gets or sets the inner text.
        /// </summary>
        /// <value>The inner text.</value>
        public string MaxFetch
        {
            get
            {
                return strMaxRecordfetch;
            }
            set
            {
                strMaxRecordfetch = value;
            }
        }
        #endregion
    }
}

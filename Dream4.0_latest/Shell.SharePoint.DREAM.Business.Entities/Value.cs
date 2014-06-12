#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: Value.cs 
#endregion

/// <summary> 
/// This is used to get/set values for the text.
/// </summary>

namespace Shell.SharePoint.DREAM.Business.Entities
{
    /// <summary>
    /// Value Class.
    /// </summary>
    public class Value
    {
        #region DECLARATION
        string strInnerText = string.Empty;
        #endregion
        #region PROPERTIES
        /// <summary>
        /// Gets or sets the inner text.
        /// </summary>
        /// <value>The inner text.</value>
        public string InnerText
        {
            get
            {
                return strInnerText;
            }
            set
            {
                strInnerText = value;
            }
        }
        #endregion
    }
}

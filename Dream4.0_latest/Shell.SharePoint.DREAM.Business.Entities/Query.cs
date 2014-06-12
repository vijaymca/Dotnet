#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: Query.cs 
#endregion
/// <summary> 
/// This is Query class. This class is used to create Query object for generating request info object
/// </summary>

namespace Shell.SharePoint.DREAM.Business.Entities
{
    /// <summary>
    /// Query Class.
    /// </summary>
    public class Query
    {
        #region DECLARATION
        private string strInnerText = string.Empty;
        private string strType = string.Empty;
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

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type
        {
            get
            {
                return strType;
            }
            set
            {
                strType = value;
            }
        }
        #endregion
    }
}

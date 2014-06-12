#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: Parameters.cs 
#endregion

/// <summary> 
/// This class is used to get/set parameter values while generating request info object
/// </summary>

namespace Shell.SharePoint.DREAM.Business.Entities
{
    /// <summary>
    /// Parameter Class.
    /// </summary>
    public class Parameters
    {
        #region DECLARATION
        private string strName =string.Empty;
        private string strValue = string.Empty;
        private string strLabel = string.Empty;
        #endregion
        #region PROPERTIES
        /// <summary>
        /// Gets or sets the parameter name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return strName;
            }
            set
            {
                strName = value;
            }
        }

        /// <summary>
        /// Gets or sets the parameter Label.
        /// </summary>
        /// <value>The Label.</value>
        public string Label
        {
            get
            {
                return strLabel;
            }
            set
            {
                strLabel = value;
            }
        }

        /// <summary>
        /// Gets or sets the parameter value.
        /// </summary>
        /// <value>The value.</value>
        public string Value
        {
            get
            {
                return strValue;
            }
            set
            {
                strValue = value;
            }
        }
        #endregion
    }
}

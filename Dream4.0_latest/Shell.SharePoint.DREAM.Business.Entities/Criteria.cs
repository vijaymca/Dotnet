#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: Criteria.cs 
#endregion

/// <summary> 
/// This class is used to create search criteria as an object for generating request info object
/// </summary> 

namespace Shell.SharePoint.DREAM.Business.Entities
{
    /// <summary>
    /// Criteria Class.
    /// </summary>
    public class Criteria
    {
        #region DECLARATION
        private string strValue = string.Empty;
        private string strOperator = string.Empty;
        private string strName = string.Empty;
        //Added for DWB Chapter
        private string strDisplayField = string.Empty;
        #endregion
        #region PROPERTIES
        /// <summary>
        /// Gets or sets the Display Field.
        /// </summary>
        /// <value>The value.</value>
        public string DisplayField
        {
            get
            {
                return strDisplayField;
            }
            set
            {
                strDisplayField = value;
            }
        }

        /// <summary>
        /// Gets or sets the value.
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

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
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
        /// Gets or sets the operator.
        /// </summary>
        /// <value>The operator.</value>
        public string Operator
        {
            get
            {
                return strOperator; 
            }
            set
            {
                strOperator = value; 
            }
        }
       
        #endregion
    }
}

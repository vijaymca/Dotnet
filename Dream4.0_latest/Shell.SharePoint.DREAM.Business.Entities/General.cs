#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: General.cs 
#endregion

/// <summary> 
/// This class is used to get/set general attributes for generating request info object
/// </summary>
using System.Collections;

namespace Shell.SharePoint.DREAM.Business.Entities
{
    /// <summary>
    /// General Class.
    /// </summary>
    public class General
    {
        #region DECLARATION
        private ArrayList arlAttribute = null;
        private ArrayList arlCriteria = null;
        #endregion
        #region PROPERTIES
        /// <summary>
        /// Gets or sets the attribute.
        /// </summary>
        /// <value>The attribute.</value>
        public ArrayList Attribute
        {
            get 
            {
                return arlAttribute; 
            }
            set
            {
                arlAttribute = value; 
            }
        }

        /// <summary>
        /// Gets or sets the criteria.
        /// </summary>
        /// <value>The criteria.</value>
        public ArrayList Criteria
        {
            get
            {
                return arlCriteria; 
            }
            set
            {
                arlCriteria = value; 
            }
        }
        #endregion
    }
}

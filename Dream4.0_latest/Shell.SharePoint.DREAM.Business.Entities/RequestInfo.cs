#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: RequestInfo.cs 
#endregion

/// <summary> 
/// This class is used to get/set request info object for search
/// </summary>

namespace Shell.SharePoint.DREAM.Business.Entities
{
    /// <summary>
    /// RequestInfo Class 
    /// </summary>
    public class RequestInfo
    {
        #region DECLARATION
        private Entity objEntity = null;
        #endregion
        #region PROPERTIES
        /// <summary>
        /// Gets or sets the report object value.
        /// </summary>
        /// <value>The report.</value>
        public Entity Entity
        {
            get
            {
                return objEntity;
            }
            set
            {
                objEntity = value;
            }
        }
        #endregion
    }
}

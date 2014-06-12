#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: SaveSearchRequest.cs 
#endregion
/// <summary> 
/// This entity is used for holding the attribute, criteria and attributegroup entities used in Save Search functionality
/// </summary>
using System;
using System.Collections;

namespace Shell.SharePoint.DREAM.Business.Entities
{
    /// <summary>
    /// The SaveSearchRequest class
    /// </summary>
    public class SaveSearchRequest
    {
        # region DECLARATION
        string strName = string.Empty;
        string strOrder = string.Empty;
        //For Save Search Type(Shared)
        protected Boolean blnSaveTypeShared = false;
        private RequestInfo objRequestInfo = null;
        string strType = string.Empty;
        #endregion
        
        #region PROPERTIES
        /// <summary>
        /// Gets or sets the SaveSearchRequest Name.
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
        /// Gets or sets the SaveSearchRequest Order.
        /// </summary>
        /// <value>The Order.</value>
        public string Order
        {
            get
            {
                return strOrder;
            }
            set
            {
                strOrder = value;
            }
        }
        /// <summary>
        /// Gets or sets the shared save search type.
        /// </summary>
        /// <value>The entity.</value>
        public Boolean SaveTypeShared
        {
            get
            {
                return blnSaveTypeShared;
            }
            set
            {
                blnSaveTypeShared = value;
            }
        }

        /// <summary>
        /// Gets or sets the report object value./
        /// </summary>
        /// <value>The report.</value>
        public RequestInfo RequestInfo
        {
            get
            {
                return objRequestInfo;
            }
            set
            {
                objRequestInfo = value;
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

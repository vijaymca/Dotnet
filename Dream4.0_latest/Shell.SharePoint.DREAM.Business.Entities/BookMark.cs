#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: BookMark.cs 
#endregion
/// <summary> 
/// This is BookMark class. This class is used to create BookMark  object for generating request info object
/// </summary>
using System.Collections;

namespace Shell.SharePoint.DREAM.Business.Entities
{
    /// <summary>
    /// BookMark entity class
    /// </summary>
    public class BookMark
    {
        #region DECLARATION
        private string strIdName = string.Empty;
        private string strType = string.Empty;
        private ArrayList arlValue = null;
        #endregion
        #region PROPERTIES
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string IdentifierName
        {
            get
            {
                return strIdName;
            }
            set
            {
                strIdName = value;
            }
        }

        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        /// <value>The name.</value>
        public string BookMarkType
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

        /// <summary>
        /// Gets or sets the Value.
        /// </summary>
        /// <value>The attribute.</value>
        public ArrayList Value
        {
            get
            {
                return arlValue;
            }
            set
            {
                arlValue = value;
            }
        }
               
        #endregion
    }
}

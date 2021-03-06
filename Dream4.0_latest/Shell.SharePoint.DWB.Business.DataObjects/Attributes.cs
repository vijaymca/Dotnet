#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: Attributes.cs 
#endregion
using System;
/// <summary> 
/// This is Attributes class. This class is used to create attribute object for 
/// generating request info object
/// </summary> 
namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// Attributes Class
    /// </summary>
    [Serializable]
    public class Attributes
    {
        #region DECLARATION
        string strName = string.Empty;
        string strValue = string.Empty;
        string strDisplay = string.Empty;
        string strDataType = string.Empty;
        #endregion
        #region PROPERTIES
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return strName; }
            set { strName = value; }
        }
     
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value
        {
            get { return strValue; }
            set { strValue = value; }
        }

        /// <summary>
        /// Gets or Sets the Display
        /// </summary>
        public string Display
        {
            get { return strDisplay; }
            set { strDisplay = value; }
        }
        
        /// <summary>
        /// Gets/Sets the Data Type.
        /// </summary>
        public string DataType
        {
            get { return strDataType; }
            set { strDataType = value; }
        }
        #endregion
    }
}

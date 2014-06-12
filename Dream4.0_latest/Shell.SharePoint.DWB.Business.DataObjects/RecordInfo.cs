#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: RecordInfo.cs 
#endregion

using System.Collections;

namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// Record Info Class.
    /// </summary>
    public class RecordInfo
    {
        #region DECLARATION
        ArrayList arlAttributes;
        #endregion
        #region PROPERTIES
        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public ArrayList Attributes 
        {
            get { return arlAttributes; }
            set { arlAttributes = value; }
        }
        #endregion
    }
}

#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: RecordHistory.cs 
#endregion

using System.Collections;

namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// Gets/Sets the Audit Trail information for any record.
    /// </summary>
    public class RecordHistory
    {
        #region DECLARATION
        string strOrder = string.Empty;
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

        /// <summary>
        /// Gets/Sets the RecordHistory Order.
        /// </summary>
        public string Order
        {
            get { return strOrder; }
            set { strOrder = value; }
        }
        #endregion
    }
}

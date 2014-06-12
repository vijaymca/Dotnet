#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: RecordHistories.cs 
#endregion

using System.Collections;

namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// Gets/Sets the Collection of Record History
    /// </summary>
    public class RecordHistories
    {
        #region DECLARATION
        ArrayList arlHistory;
        #endregion
        #region PROPERTIES
        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public ArrayList History
        {
            get { return arlHistory; }
            set { arlHistory = value; }
        }
        #endregion
    }
}

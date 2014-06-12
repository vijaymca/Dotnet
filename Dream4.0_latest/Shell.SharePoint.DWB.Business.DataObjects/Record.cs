#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: Record.cs 
#endregion

namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// Gets/Sets the Record to generate XML
    /// </summary>
    public class Record
    {
        #region DECLARATION
        string strRecordNumber = string.Empty;
        string strOrder = string.Empty;
        RecordInfo objRecordInfo;
        RecordHistories objRecordHistories;
        #endregion
        #region PROPERTIES

        /// <summary>
        /// Gets/Sets Record Number.
        /// </summary>
        public string RecordNumber
        {
            get { return strRecordNumber; }
            set { strRecordNumber = value; }
        }

        /// <summary>
        /// Gets/Sets Record Order.
        /// </summary>
        public string Order
        {
            get { return strOrder; }
            set { strOrder = value; }
        }

        /// <summary>
        /// Gets/Sets RecordInfo object.
        /// </summary>
        public RecordInfo RecordInfo
        {
            get { return objRecordInfo; }
            set { objRecordInfo = value; }
        }

        /// <summary>
        /// Gets/Sets RecordHistories object.
        /// </summary>
        public RecordHistories RecordHistories
        {
            get { return objRecordHistories; }
            set { objRecordHistories = value; }
        }
        #endregion
    }
}

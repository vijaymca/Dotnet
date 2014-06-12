#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: Record.cs 
#endregion
using System;
using System.Collections;


namespace Shell.SharePoint.DREAM.Business.Entities
{
    /// <summary>
    /// The Record class for storing the values of ListItems from a SharePoint List
    /// </summary>
    public class Record
    {
        #region DECLARATION
        string strRecordNumber = string.Empty;
        string strOrder = string.Empty;
        ArrayList arlAttributes = null;
        #endregion


        #region PROPERTIES
        /// <summary>
        /// Gets or sets the record number.
        /// </summary>
        /// <value>The record number.</value>
        public string RecordNumber
        {
            get { return strRecordNumber; }
            set { strRecordNumber = value; }
        }
        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        public string Order
        {
            get { return strOrder; }
            set { strOrder = value; }
        }

        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public ArrayList RecordAttributes
        {
            get { return arlAttributes; }
            set { arlAttributes = value; }
        }

        #endregion
    }
}

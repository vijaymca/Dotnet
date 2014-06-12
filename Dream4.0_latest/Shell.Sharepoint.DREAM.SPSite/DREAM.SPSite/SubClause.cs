#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: SubCluase.cs 
#endregion

using System;

namespace Shell.SharePoint.DREAM.Site.UI
{
    /// <summary>
    /// SubClause class
    /// </summary>
    public class SubClause
    {
        #region DECLARATIONS
        private string strFieldName;
        private string strComparisonOperator;
        private object strValue;
        #endregion

        /// <summary>
        /// Gets /Sets Fieldname
        /// </summary>
        public string FieldName
        {
            get { return strFieldName; }
            set { strFieldName = value; }
        }

        /// <summary>
        /// Gets/sets the comparison method
        /// </summary>
        public string ComparisonOperator
        {
            get { return strComparisonOperator; }
            set { strComparisonOperator = value; }
        }

        /// <summary>
        /// Gets/sets the value that was set for comparison
        /// </summary>
        public object CompareValue
        {
            get { return strValue; }
            set { strValue = value; }
        }

        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="compareOperator"></param>
        /// <param name="compareValue"></param>
        public SubClause(string fieldName, string compareOperator, object compareValue)
        {
            FieldName = fieldName;
            ComparisonOperator = compareOperator;
            CompareValue = compareValue;
        }
    }
}

#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: WhereClause.cs 
#endregion

using System;
using System.Collections.Generic;

namespace Shell.SharePoint.DREAM.Site.UI
{
   
    /// <summary>
    /// Enum of Logic Operators 
    /// </summary>
    public enum LogicOperator
    {
        AND,
        OR
    }

    /// <summary>
    /// WhereClause class
    /// </summary>
    public class WhereClause
    {
        #region DECLARATION
        private List<SubClause> arlSubClauses;	// Array of SubClause
        private string strWhereStatement = "WHERE ";
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public WhereClause()
        {
            arlSubClauses = new List<SubClause>();
        }

        /// <summary>
        /// Adds a SubClause object to the Collection
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="compareOperator"></param>
        /// <param name="compareValue"></param>
        public void AddClause(string fieldName, string compareOperator, object compareValue)
        {
            SubClause objSubClause = new SubClause(fieldName, compareOperator, compareValue);
            arlSubClauses.Add(objSubClause);
        }

        /// <summary>
        /// Returns the Where Statement section of the Constructed Query
        /// </summary>
        public string WhereStatement
        {
            get
            {
                int intCount = 0;
                foreach (SubClause objSubClause in arlSubClauses)
                {
                    strWhereStatement += CreateComparisonClause(objSubClause.FieldName, objSubClause.ComparisonOperator, objSubClause.CompareValue) + " ";
                    intCount+= 1;
                    if (intCount< arlSubClauses.Count)
                    {
                        strWhereStatement += LogicOperator.AND + " ";
                    }
                }

                if (arlSubClauses.Count == 0)
                    strWhereStatement = string.Empty;

                return strWhereStatement;
            }
        }

        /// <summary>
        /// Creates a Comparison clause using the operator and values suppplied
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="comparisonOperator"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static string CreateComparisonClause(string fieldName, string comparisonOperator, object value)
        {
            string strOutput = string.Empty;
            if (value != null && value != System.DBNull.Value)
            {
                strOutput = fieldName + " " + comparisonOperator + " " + FormatSQLValue(value); 
            }
            return strOutput;
        }
        /// <summary>
        /// Returns the formatted string value of the object 
        /// </summary>
        /// <param name="someValue"></param>
        /// <returns></returns>
        internal static string FormatSQLValue(object someValue)
        {
            string strFormattedValue = string.Empty;
            if (someValue == null)
            {
                strFormattedValue = "NULL";
            }
            else
            {
                switch (someValue.GetType().Name)
                {
                    case "String": strFormattedValue = "'" + ((string)someValue).Replace("'", "''") + "'"; break;
                    case "DateTime": strFormattedValue = "'" + ((DateTime)someValue).ToString("yyyy/MM/dd hh:mm:ss") + "'"; break;
                    case "DBNull": strFormattedValue = "NULL"; break;
                    case "Boolean": strFormattedValue = (bool)someValue ? "1" : "0"; break;
                    default: strFormattedValue = someValue.ToString(); break;
                }
            }
            return strFormattedValue;
        }
    }
}

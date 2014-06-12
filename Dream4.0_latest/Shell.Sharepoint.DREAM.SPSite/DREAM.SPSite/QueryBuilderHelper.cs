#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: QueryBuilderHelper.ascx.cs 
#endregion

using System;
using System.Collections.Generic;

/// <summary>
/// QueryBuilderHelper class
/// </summary>
namespace Shell.SharePoint.DREAM.Site.UI
{
    /// <summary>
    /// Constructs the Query for Query Builder Module
    /// </summary>
    public class QueryBuilderHelper
    {
        #region DECLARATION
        protected List<string> arlColumns = new List<string>();	// array of string
        protected string strTableName;	
        protected WhereClause objWhereClause = null;
        #endregion

        #region METHODS
        /// <summary>
        /// Constructor
        /// </summary>
        public QueryBuilderHelper() {
        objWhereClause = new WhereClause();
        }

        /// <summary>
        /// Builds the Query 
        /// </summary>
        /// <returns></returns>
        public string BuildQuery()
        {
            string strQuery = "SELECT ";

            foreach (string ColumnName in arlColumns)
            {
                strQuery += ColumnName + ',';
            }
            strQuery = strQuery.TrimEnd(','); // Trim de last comma inserted by foreach loop
            strQuery += ' ';

            strQuery += " FROM " + TableName;
           
            strQuery += ' ';

            if (objWhereClause != null)
                strQuery += objWhereClause.WhereStatement;

            return strQuery;
        }

        /// <summary>
        /// Adds a clause to the Where Statement of the Query
        /// </summary>
        /// <param name="colname"></param>
        /// <param name="optor"></param>
        /// <param name="compareVal"></param>
        public void AddSubClause(string colname, string optor, string compareVal)
        {
            objWhereClause.AddClause(colname, optor, compareVal);
        }

        /// <summary>
        /// Gets /Sets the Table Name
        /// </summary>
        public string TableName
        {
            get
            {
                return strTableName;
            }
            set
            {
                strTableName = value;
            }
        }

        /// <summary>
        /// Gets/ Sets the list of Column Names of the table
        /// </summary>
        public List<string> ColumnNames
        {
            get
            {
                return arlColumns;
            }
            set
            {
                arlColumns = value;
            }
        }
        #endregion
    }
}

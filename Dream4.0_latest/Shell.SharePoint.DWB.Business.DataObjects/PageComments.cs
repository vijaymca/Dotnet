#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: PageCommentsDetails.cs 
#endregion

namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// Data Object class for Page Comments
    /// </summary>
    public class PageCommentsDetails
    {
        #region DECLARATION
        string strPageID = string.Empty;
        string strDiscipline = string.Empty;
        string strDisciplineID = string.Empty;
        string strUserName = string.Empty;
        string strShareComments = "No";
        string strComments = string.Empty;
        int intRowId;
        #endregion
        #region Properties

        /// <summary>
        ///  Gets or sets the page ID.
        /// </summary>
        public string PageID
        {
            get { return strPageID; }
            set { strPageID = value; }
        }

        /// <summary>
        /// Gets or sets the page ID.
        /// </summary>
        public int RowId
        {
            get { return intRowId; }
            set { intRowId = value; }
        }

        /// <summary>
        /// Gets or sets the Discipline.
        /// </summary>
        public string Discipline
        {
            get { return strDiscipline; }
            set { strDiscipline = value; }
        }

        /// <summary>
        /// Gets or sets the Discipline ID.
        /// </summary>
        public string DisciplineID
        {
            get { return strDisciplineID; }
            set { strDisciplineID = value; }
        }

        /// <summary>
        /// Gets or Sets the Comments for the page
        /// </summary>
        public string Comments
        {
            get { return strComments; }
            set { strComments = value; }
        }

        /// <summary>
        /// Gets or Sets the UserName for the Comment
        /// </summary>
        public string UserName
        {
            get { return strUserName; }
            set { strUserName = value; }
        }

        /// <summary>
        /// Gets or Sets to Share the Comments or not
        /// </summary>
        public string ShareComments
        {
            get { return strShareComments; }
            set { strShareComments = value; }
        }
        #endregion
    }
}

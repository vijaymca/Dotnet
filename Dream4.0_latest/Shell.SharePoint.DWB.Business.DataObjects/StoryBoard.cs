#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: StoryBoard.cs 
#endregion
using System;

namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// Data Object class for Story Board.
    /// </summary>
    /// 
    [Serializable]
    public class StoryBoard
    {
        #region DECLARATION
        string strPageType = string.Empty;
        string strPageTitle = string.Empty;
        string strConnectionType = string.Empty;
        string strSource = string.Empty;
        string strDiscipline = string.Empty;
        string strMasterPageName = string.Empty;
        string strApplicationPage = string.Empty;
        string strApplicationTemplate = string.Empty;
        string strSOP = string.Empty;
        string strCreatedBy = string.Empty;
        string strCreationDate = string.Empty;
        string strPageOwner = string.Empty;
        int intPageId;
        int intRowId;
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the type of the page.
        /// </summary>
        /// <value>The type of the page.</value>
        public string PageType
        {
            get { return strPageType; }
            set { strPageType = value; }
        }
       /// <summary>
        /// Gets or sets the page ID.
       /// </summary>
        public int PageId
        {
            get { return intPageId; }
            set { intPageId = value; }
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
        /// Gets or sets the page title.
        /// </summary>
        /// <value>The page title.</value>
        public string PageTitle
        {
            get { return strPageTitle; }
            set { strPageTitle = value; }
        }
        /// <summary>
        /// Gets or sets the ConnectionType.
        /// </summary>
        /// <value>The ConnectionType.</value>
        public string ConnectionType
        {
            get { return strConnectionType ; }
            set { strConnectionType = value; }
        }
        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        public string Source
        {
            get { return strSource; }
            set { strSource = value; }
        }
        /// <summary>
        /// Gets or sets the discipline.
        /// </summary>
        /// <value>The discipline.</value>
        public string Discipline
        {
            get { return strDiscipline; }
            set { strDiscipline = value; }
        }
        /// <summary>
        /// Gets or sets the name of the master page.
        /// </summary>
        /// <value>The name of the master page.</value>
        public string MasterPageName
        {
            get { return strMasterPageName; }
            set { strMasterPageName = value; }
        }
        /// <summary>
        /// Gets or sets the application page.
        /// </summary>
        /// <value>The application page.</value>
        public string ApplicationPage
        {
            get { return strApplicationPage; }
            set { strApplicationPage = value; }
        }
        /// <summary>
        /// Gets or sets the application template.
        /// </summary>
        /// <value>The application template.</value>
        public string ApplicationTemplate
        {
            get { return strApplicationTemplate; }
            set { strApplicationTemplate = value; }
        }
        /// <summary>
        /// Gets or sets the SOP.
        /// </summary>
        /// <value>The SOP.</value>
        public string SOP
        {
            get { return strSOP; }
            set { strSOP = value; }
        }
        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public string CreatedBy
        {
            get { return strCreatedBy; }
            set { strCreatedBy = value; }
        }
        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        /// <value>The creation date.</value>
        public string CreationDate
        {
            get { return strCreationDate; }
            set { strCreationDate = value; }
        }
        /// <summary>
        /// Gets or sets the page owner.
        /// </summary>
        /// <value>The page owner.</value>
        public string PageOwner
        {
            get { return strPageOwner; }
            set { strPageOwner = value; }
        }
        #endregion
    }
}
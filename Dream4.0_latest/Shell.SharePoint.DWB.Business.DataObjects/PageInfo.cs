#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: PageInfo.cs 
#endregion

using System;

namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// Gets/Sets the Page information for Digital Well Book.
    /// </summary>
    [Serializable]
    public class PageInfo
    {
        #region DECLARATION
        string strPageTitle;
        string strPageActualName = string.Empty;
        string strPageID;
        string strPageURL;
        string strPageOwner = string.Empty;
        string strSignOffStatus = string.Empty;
        int intConnectionType;
        string strAssetType = string.Empty;
        string strActualAssetValue = string.Empty;
        string strColumnName = string.Empty;
        string strReportName = string.Empty;
        string strLastUpdatedDate = string.Empty; //Added by Praveena for module "Add Last Updated date"
        bool blnIncludeStoryBoard;
        bool blnIncludeComments;
        bool blnIncludeNarrative;

        #region DREAM 4.0 - eWB 2.0
        bool blnIsEmpty = true;
        #endregion 
        #endregion

        #region Properties

        /// <summary>
        /// Gets/Sets the Page Title.
        /// </summary>
        public string PageTitle
        {
            get { return strPageTitle; }
            set { strPageTitle = value; }
        }

        /// <summary>
        /// Gets/Sets the Page ID.
        /// </summary>
        public string PageID
        {
            get { return strPageID; }
            set { strPageID = value; }
        }

        /// <summary>
        /// Gets/Sets the Page URL.
        /// </summary>
        public string PageURL
        {
            get { return strPageURL; }
            set { strPageURL = value; }
        }

        /// <summary>
        /// Gets/Sets the Page Owner.
        /// </summary>
        public string PageOwner
        {
            get { return strPageOwner; }
            set { strPageOwner = value; }
        }

        /// <summary>
        /// Gets/Sets the Page SignOff Status.
        /// </summary>
        public string SignOffStatus
        {
            get { return strSignOffStatus; }
            set { strSignOffStatus = value; }
        }

        /// <summary>
        /// Gets/Sets the Page Connection Type.
        /// </summary>
        public int ConnectionType
        {
            get { return intConnectionType; }
            set { intConnectionType = value; }
        }

        /// <summary>
        /// Gets/Sets the Page Asset Type.
        /// </summary>
        public string AssetType
        {
            get { return strAssetType; }
            set { strAssetType = value; }
        }

        /// <summary>
        /// Gets/Sets the Actual Asset Value of the Chapter.
        /// </summary>
        public string ActualAssetValue
        {
            get { return strActualAssetValue; }
            set { strActualAssetValue = value; }
        }

        /// <summary>
        /// Gets/Sets the Column Name of the Chapter.
        /// </summary>
        public string ColumnName
        {
            get { return strColumnName; }
            set { strColumnName = value; }
        }

        /// <summary>
        /// Gets/Sets the Page Report Name for Type I pages.
        /// </summary>
        public string ReportName
        {
            get { return strReportName; }
            set { strReportName = value; }
        }

        /// <summary>
        /// Gets/Sets the Page actual name.
        /// </summary>
        public string PageActualName
        {
            get { return strPageActualName; }
            set { strPageActualName = value; }
        }

        /// <summary>
        /// Gets/Sets the Last Updated Date.
        /// Added by Praveena for module "Add Last Updated date"
        /// </summary>
        public string LastUpdatedDate
        {
            get { return strLastUpdatedDate; }
            set { strLastUpdatedDate = value; }
        }


        /// <summary>
        /// Gets/Sets to include story board or not.
        /// </summary>
        public bool IncludeStoryBoard
        {
            get { return blnIncludeStoryBoard; }
            set { blnIncludeStoryBoard = value; }
        }
        /// <summary>
        /// Gets/Sets to include comments or not.
        /// </summary>
        public bool IncludeComments
        {
            get { return blnIncludeComments; }
            set { blnIncludeComments = value; }
        }

        /// <summary>
        /// Gets/Sets to include narrative or not.
        /// </summary>
        public bool IncludeNarrative
        {
            get { return blnIncludeNarrative; }
            set { blnIncludeNarrative = value; }
        }

        #region DREAM 4.0 - eWB 2.0
        public bool IsEmpty
        {
            get { return blnIsEmpty; }
            set { blnIsEmpty = value; }
        }
        #endregion 
        #endregion
    }
}

#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : TreeNodeSelection.cs
#endregion
using System;

namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// Get/Set the selected Well Book Treenode properties.
    /// </summary>
    [Serializable]
    public class TreeNodeSelection
    {
        bool blnTypeI;
        bool blnTypeII;
        bool blnTypeIII;
        bool blnBookSummary;
        bool blnChapterSummary;
        string strPageID = string.Empty;
        string strChapterID = string.Empty;
        string strReportName = string.Empty;
        string strBookID = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is type I selected.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is type I selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsTypeISelected
        {
            get { return blnTypeI; }
            set { blnTypeI = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is type II selected.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is type II selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsTypeIISelected
        {
            get { return blnTypeII; }
            set { blnTypeII = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is type III selected.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is type III selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsTypeIIISelected
        {
            get { return blnTypeIII; }
            set { blnTypeIII = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is book selected.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is book selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsBookSelected
        {
            get { return blnBookSummary; }
            set { blnBookSummary = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is chapter selected.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is chapter selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsChapterSelected
        {
            get { return blnChapterSummary; }
            set { blnChapterSummary = value; }
        }

        /// <summary>
        /// Gets or sets the page ID.
        /// </summary>
        /// <value>The page ID.</value>
        public string PageID
        {
            get { return strPageID; }
            set { strPageID = value; }
        }

        /// <summary>
        /// Gets or sets the name of the report.
        /// </summary>
        /// <value>The name of the report.</value>
        public string ReportName
        {
            get { return strReportName; }
            set { strReportName = value; }
        }

        /// <summary>
        /// Gets or sets the chapter ID.
        /// </summary>
        /// <value>The chapter ID.</value>
        public string ChapterID
        {
            get { return strChapterID; }
            set { strChapterID = value; }
        }

        /// <summary>
        /// Gets or sets the book ID.
        /// </summary>
        /// <value>The book ID.</value>
        public string BookID
        {
            get { return strBookID; }
            set { strBookID = value; }
        }
    }
}

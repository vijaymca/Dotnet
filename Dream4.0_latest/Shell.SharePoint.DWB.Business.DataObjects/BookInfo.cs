#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: BookInfo.cs 
#endregion
using System;
using System.Collections;


namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// Book Info Class
    /// </summary>
    [Serializable]
    public class BookInfo
    {
        #region DECLARATION
        private string strBookName = string.Empty;
        private string strBookID = string.Empty;
        private string strBookOwner = string.Empty;
        private string strBookTeamID = string.Empty;
        private string strAction = string.Empty;
        private int intPageCount;
        private ArrayList arlChapters;
        private bool blnIsPrintable ;
        private bool blnIsTOCApplicable ;
        private bool blnIsStoryBoardApplicable ;
        private bool blnIsTitlePageApplicable;
        private bool blnIsBookTitleApplicable;
        #endregion

        #region PROPERTIES
        /// <summary>
        /// Gets or Sets the BookName
        /// </summary>
        public string BookName
        {
            get { return strBookName; }
            set { strBookName = value; }
        }

        /// <summary>
        /// Gets or Sets the BookID
        /// </summary>
        public string BookID
        {
            get { return strBookID; }
            set { strBookID = value; }
        }

        /// <summary>
        /// Gets or Sets the BookOwner
        /// </summary>
        public string BookOwner
        {
            get { return strBookOwner; }
            set { strBookOwner = value; }
        }

        /// <summary>
        /// Gets/Sets the TeamID of the Book.
        /// </summary>
        public string BookTeamID
        {
            get { return strBookTeamID; }
            set { strBookTeamID = value; }
        }
        /// <summary>
        /// Gets/Sets the no of Pages in a Book
        /// </summary>
        public int PageCount
        {
            get { return intPageCount; }
            set { intPageCount = value; }
        }

        /// <summary>
        /// Gets or Sets the Chapters
        /// </summary>
        public ArrayList Chapters
        {
            get { return arlChapters; }
            set { arlChapters = value; }
        }

        /// <summary>
        /// Gets/Sets the Action(Print/Pdf/TOC).
        /// </summary>
        public string Action
        {
            get { return strAction; }
            set { strAction = value; }
        }

        /// <summary>
        /// Gets/Sets Printable or not.
        /// </summary>
        public bool IsPrintable
        {
            get { return blnIsPrintable; }
            set { blnIsPrintable = value; }
        }

        /// <summary>
        /// Gets/Sets Table of Content applicable or not.
        /// </summary>
        public bool IsTOCApplicatble
        {
            get { return blnIsTOCApplicable; }
            set { blnIsTOCApplicable = value; }
        }

        /// <summary>
        /// Gets/Sets Story Board applicable or not.
        /// </summary>
        public bool IsStoryBoardApplicable
        {
            get { return blnIsStoryBoardApplicable; }
            set { blnIsStoryBoardApplicable = value; }
        }

        /// <summary>
        /// Gets/Sets Page Title is included in print or not.
        /// </summary>
        public bool IsPageTitleApplicable
        {
            get { return blnIsTitlePageApplicable; }
            set { blnIsTitlePageApplicable = value; }
        }

        /// <summary>
        /// Gets/Sets Book Title is included in print or not.
        /// </summary>
        public bool IsBookTitleApplicable
        {
            get { return blnIsBookTitleApplicable; }
            set { blnIsBookTitleApplicable = value; }
        }
        #endregion
    }
}

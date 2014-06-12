#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: UIHelper.cs
#endregion

namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// Get/Set the print options user selects while printing.
    /// </summary>
    public class PrintOptions
    {
        #region Variables
        bool blnIncludeStoryBoard;
        bool blnIncludePageTitle;
        bool blnIncludeTOC;
        bool blnIncludeBookTitle;
        
        //Added By Gopinath
        //Reason: Added filter properties
        //Date: 11/11/2010
        private bool blnPrintMyPages;
        private bool blnIncludeFilter;
        private string strSignedOff = string.Empty;
        private string strEmptyPages = string.Empty;
        private string strPageType = string.Empty;
        private string strPageName = string.Empty;
        private string strDiscipline = string.Empty;
        
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether [include story board].
        /// </summary>
        /// <value><c>true</c> if [include story board]; otherwise, <c>false</c>.</value>
        public bool IncludeStoryBoard
        {
            get { return blnIncludeStoryBoard; }
            set { blnIncludeStoryBoard = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [include page title].
        /// </summary>
        /// <value><c>true</c> if [include page title]; otherwise, <c>false</c>.</value>
        public bool IncludePageTitle
        {
            get { return blnIncludePageTitle; }
            set { blnIncludePageTitle = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether include Table of Contents.
        /// </summary>
        /// <value><c>true</c> if [include TOC]; otherwise, <c>false</c>.</value>
        public bool IncludeTOC
        {
            get { return blnIncludeTOC; }
            set { blnIncludeTOC = value; }
        }

        public bool IncludeBookTitle
        {
            get { return blnIncludeBookTitle; }
            set { blnIncludeBookTitle = value; }
        }

       //Added filter properties by Gopinath

        /// <summary>
        /// PrintMyPages
        /// </summary>
        public bool PrintMyPages
        {
            get { return blnPrintMyPages; }
            set { blnPrintMyPages = value; }
        }

        /// <summary>
        /// IncludeFilter
        /// </summary>
        public bool IncludeFilter
        {
            get { return blnIncludeFilter; }
            set { blnIncludeFilter = value; }
        }

        public string SignedOff
        {
            get { return strSignedOff; }
            set { strSignedOff = value; }
        }

        public string EmptyPages
        {
            get { return strEmptyPages; }
            set { strEmptyPages = value; }
        }

        public string PageType
        {
            get { return strPageType; }
            set { strPageType = value; }
        }

        public string PageName
        {
            get { return strPageName; }
            set { strPageName = value; }
        }

        public string Discipline
        {
            get { return strDiscipline; }
            set { strDiscipline = value; }
        }
        #endregion
    }
}

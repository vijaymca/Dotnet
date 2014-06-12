#region Shell Copyright.2011
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: Pagination.cs 
#endregion

using System;
using System.Xml;
/// <summary> 
/// This class is used to create an object for pagination
/// </summary> 
namespace Shell.SharePoint.DREAM.Business.Entities
{
    /// <summary>
    /// Pagination Class.
    /// </summary>
    public class Pagination
    {
        //****************Information about  used variables *******************
        //1. intNoOfPageLinkToDisplay - this comes from hardcoded value or from settings
        //2. intRecordPerPage - this comes from userpreferences
        //3. currentPageNumber - comes as parameter
        //4. intSkipCount -skip count is calculated on the basis of 2 & 1 **in some cases always set to 0
        //5. intRecordCount - it is calculated on the basis of 3 and itself(current record count in response xml),so it depends on response xml also
        //6. intPageCount - depends on  4 & 1
        //7. intStartPageNumber -depends on 2,5 & intNoOfPageLinkToDisplay
        //8. intEndPageNumber -depends on 5,6 & intNoOfPageLinkToDisplay
        //9. intRecordStarIndex - equals 1 or (currentPageNumber * intNodePerPage - intNodePerPage + 1);
        //10. intRecordEndIndex - euqals intNodePerPage or currentPageNumber * intNodePerPage;

        #region DECLARATION

        int intNoOfPageLinkToDisplay = 5;

        int intRecordPerPage = 0;

        int intCurrentPageNumber = 0;

        int intSkipCount = 0;

        int intRecordCount = 0;

        int intPageCount = 0;

        int intStartPageNumber = 0;

        int intEndPageNumber = 0;

        int intRecordStarIndex = 0;

        int intRecordEndIndex = 0;

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Gets the number of page link to display.
        /// </summary>
        /// <value>The number of page link to display.</value>
        public int NumberOfPageLinkToDisplay
        {
            get { return intNoOfPageLinkToDisplay; }
        }

        /// <summary>
        /// Gets the record per page.
        /// </summary>
        /// <value>The record per page.</value>
        public int RecordPerPage
        {
            get { return intRecordPerPage; }
        }

        /// <summary>
        /// Gets the current page number.
        /// </summary>
        /// <value>The current page number.</value>
        public int CurrentPageNumber
        {
            get { return intCurrentPageNumber; }
        }

        /// <summary>
        /// Gets the skip count.
        /// </summary>
        /// <value>The skip count.</value>
        public int SkipCount
        {
            get { return intSkipCount; }
        }

        /// <summary>
        /// Gets the record count.
        /// </summary>
        /// <value>The record count.</value>
        public int RecordCount
        {
            get { return intRecordCount; }
        }

        /// <summary>
        /// Gets the page count.
        /// </summary>
        /// <value>The page count.</value>
        public int PageCount
        {
            get { return intPageCount; }
        }

        /// <summary>
        /// Gets the start page number.
        /// </summary>
        /// <value>The start page number.</value>
        public int StartPageNumber
        {
            get { return intStartPageNumber; }
        }

        /// <summary>
        /// Gets the end page number.
        /// </summary>
        /// <value>The end page number.</value>
        public int EndPageNumber
        {
            get { return intEndPageNumber; }
        }

        /// <summary>
        /// Gets the index of the record star.
        /// </summary>
        /// <value>The index of the record star.</value>
        public int RecordStarIndex
        {
            get { return intRecordStarIndex; }
        }

        /// <summary>
        /// Gets the end index of the record.
        /// </summary>
        /// <value>The end index of the record.</value>
        public int RecordEndIndex
        {
            get { return intRecordEndIndex; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Pagination"/> class.
        /// </summary>
        /// <param name="currentPageNumber">The current page number.</param>
        /// <param name="recordCount">The record count.</param>
        /// <param name="recordPerPage">The record per page.</param>
        /// <param name="isSkipCountEnabled">if set to <c>true</c> [is skip count enabled].</param>
        public Pagination(int currentPageNumber, int recordCount, int recordPerPage, bool isSkipCountEnabled)
        {
            intCurrentPageNumber = currentPageNumber;
            intRecordCount = recordCount;
            intRecordPerPage = recordPerPage;
            if (isSkipCountEnabled)
            {
                SetProperitesWithSkipCount();
            }
            else
            {
                SetProperitesWithoutSkipCount();
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Sets the properites with skip count.
        /// </summary>
        private void SetProperitesWithSkipCount()
        {

            intSkipCount = GetSkipCount(intCurrentPageNumber, intRecordPerPage);

            intRecordCount = intSkipCount + intRecordCount;

            intPageCount = GetPageCount(intRecordCount, intRecordPerPage);

            intStartPageNumber = GetStartPageNumber(intCurrentPageNumber, intNoOfPageLinkToDisplay, intPageCount);

            intEndPageNumber = GetEndPageNumber(intStartPageNumber, intNoOfPageLinkToDisplay, intPageCount);

            intRecordStarIndex = 1;

            intRecordEndIndex = intRecordPerPage;
        }
        /// <summary>
        /// Sets the properites without skip count.
        /// </summary>
        private void SetProperitesWithoutSkipCount()
        {

            intSkipCount = 0;

            intPageCount = GetPageCount(intRecordCount, intRecordPerPage);

            intStartPageNumber = GetStartPageNumber(intCurrentPageNumber, intNoOfPageLinkToDisplay, intPageCount);

            intEndPageNumber = GetEndPageNumber(intStartPageNumber, intNoOfPageLinkToDisplay, intPageCount);

            intRecordStarIndex = GetRecordStarIndex(intCurrentPageNumber, intRecordPerPage);

            intRecordEndIndex = GetRecordEndIndex(intCurrentPageNumber, intRecordPerPage);
        }
        /// <summary>
        /// Gets the skip count.
        /// </summary>
        /// <param name="currentPageNumber">The current page number.</param>
        /// <param name="recordPerPage">The record per page.</param>
        /// <returns>Returns skip count as integer</returns>
        private int GetSkipCount(int currentPageNumber, int recordPerPage)
        {
            int intSkipCount = 0;
            intSkipCount = (currentPageNumber - 1) * recordPerPage;
            return intSkipCount;
        }

        /// <summary>
        /// Gets the page count.
        /// </summary>
        /// <param name="nodeCount">The node count.</param>
        /// <param name="recordPerPage">The record per page.</param>
        /// <returns>Returns page count as integer</returns>
        private int GetPageCount(int nodeCount, int recordPerPage)
        {
            int intPageCount = 0;
            intPageCount = (int)Math.Ceiling((double)nodeCount / (double)recordPerPage);
            return intPageCount;
        }

        /// <summary>
        /// Gets the start page number.
        /// </summary>
        /// <param name="currentPageNumber">The current page number.</param>
        /// <param name="noOfPageLinkToDisplay">The no of page link to display.</param>
        /// <param name="pageCount">The page count.</param>
        /// <returns>Returns start page number as integer</returns>
        private int GetStartPageNumber(int currentPageNumber, int noOfPageLinkToDisplay, int pageCount)
        {
            int intStartPageNumber = 0;
            intStartPageNumber = Math.Min(Math.Max(0, currentPageNumber - (noOfPageLinkToDisplay / 2)), Math.Max(0, pageCount - noOfPageLinkToDisplay + 1));
            return intStartPageNumber;
        }

        /// <summary>
        /// Gets the end page number.
        /// </summary>
        /// <param name="startPageNumber">The start page number.</param>
        /// <param name="noOfPageLinkToDisplay">The no of page link to display.</param>
        /// <param name="pageCount">The page count.</param>
        /// <returns>Returns end page number as integer</returns>
        private int GetEndPageNumber(int startPageNumber, int noOfPageLinkToDisplay, int pageCount)
        {
            int intEndPageNumber = 0;
            intEndPageNumber = Math.Min(pageCount, startPageNumber + noOfPageLinkToDisplay);
            return intEndPageNumber;
        }

        /// <summary>
        /// Gets the index of the record star.
        /// </summary>
        /// <param name="currentPageNumber">The current page number.</param>
        /// <param name="nodePerPage">The node per page.</param>
        /// <returns>Returns recorde start index as integer</returns>
        private int GetRecordStarIndex(int currentPageNumber, int nodePerPage)
        {
            int intRecordStarIndex = 0;
            intRecordStarIndex = currentPageNumber * nodePerPage - nodePerPage + 1;
            return intRecordStarIndex;
        }

        /// <summary>
        /// Gets the index of the record star.
        /// </summary>
        /// <param name="currentPageNumber">The current page number.</param>
        /// <param name="nodePerPage">The node per page.</param>
        /// <returns>Returns record end index as integer</returns>
        private int GetRecordEndIndex(int currentPageNumber, int nodePerPage)
        {
            int intRecordEndIndex = 0;
            intRecordEndIndex = currentPageNumber * nodePerPage;
            return intRecordEndIndex;
        }
        #endregion
    }
}

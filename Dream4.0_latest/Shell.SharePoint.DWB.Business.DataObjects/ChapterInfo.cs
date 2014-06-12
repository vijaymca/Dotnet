#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: ChapterInfo.cs 
#endregion

using System;
using System.Collections;

namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// Contains the chapter info.
    /// </summary>
    [Serializable]
    public class ChapterInfo
    {
        #region DECLARATION
        string strChapterTitle = string.Empty;
        string strChapterID = string.Empty;
        string strAssetType = string.Empty;
        string strAssetValue = string.Empty;
        string strActualAssetValue = string.Empty;
        string strColumnName = string.Empty;
        ArrayList arlPageInfo;
        bool blnIsPrintable;

        #region DREAM 4.0 - eWB2.0 - Customise Chapters
        bool blnDisplay;
        #endregion

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Gets/Sets Chapter Title
        /// </summary>
        public string ChapterTitle
        {
            get { return strChapterTitle; }
            set { strChapterTitle = value; }
        }
        /// <summary>
        /// Gets/Sets Chapter ID.
        /// </summary>
        public string ChapterID
        {
            get { return strChapterID; }
            set { strChapterID = value; }
        }
        /// <summary>
        /// Gets/Sets Chapter Asset Type.
        /// </summary>
        public string AssetType
        {
            get { return strAssetType; }
            set { strAssetType = value; }
        }
        /// <summary>
        /// Gets/Sets Chapter Asset Value.
        /// </summary>
        public string AssetValue
        {
            get { return strAssetValue; }
            set { strAssetValue = value; }
        }
        /// <summary>
        /// Gets/Sets Chapter Pages collection.
        /// </summary>
        public ArrayList PageInfo
        {
            get { return arlPageInfo; }
            set { arlPageInfo = value; }
        }
        /// <summary>
        /// Gets/Sets Chapter Actual Asset Value.
        /// </summary>
        public string ActualAssetValue
        {
            get { return strActualAssetValue; }
            set { strActualAssetValue = value; }
        }
        /// <summary>
        /// Gets/Sets Chapter Column Name.
        /// </summary>
        public string ColumnName
        {
            get { return strColumnName; }
            set { strColumnName = value; }
        }
        /// <summary>
        /// Gets/Sets Chapter is printable or not.
        /// </summary>
        public bool IsPrintable
        {
            get { return blnIsPrintable; }
            set { blnIsPrintable = value; }
        }
        #region DREAM 4.0 - eWB2.0 - Customise Chapters
        public bool Display
        {
            get { return blnDisplay; }
            set { blnDisplay = value; }
        }
        #endregion
        #endregion
    }
}

#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: ChapterDetails.cs 
#endregion
using System;
namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// This class handles the details of each Chapter
    /// </summary>
    [Serializable]
   public  class ChapterDetails
   {
       #region DECLARATION
       private string strAssetType = string.Empty;
       int intTemplateID;
       int intBookID;
       string strCountry = string.Empty;
       string strColumnName = string.Empty;
       string strCriteria = string.Empty;
       string strAssetValue = string.Empty;
       string strChapterTitle = string.Empty;
       string strChapterDescription = string.Empty;
       string strActualAssetValue = string.Empty;
       int intChapterSequence;
       string strTerminated = string.Empty;
       int intRowID;
       #endregion

       #region PROPERTIES
       /// <summary>
       /// Gets or Sets the RowID of the Chapter
       /// </summary>
       public int RowID
       {
           get { return intRowID; }
           set { intRowID = value; }
       }

       /// <summary>
       /// Gets or Sets the TemplateID of the Chapter
       /// </summary>
       public int TemplateID
        {
            get { return intTemplateID; }
            set { intTemplateID = value; }
        }

       /// <summary>
       /// Gets or Sets the BookID of the Chapter
       /// </summary>
       public int BookID
       {
           get { return intBookID; }
           set { intBookID = value; }
       }

       /// <summary>
       /// Gets or Sets the AssetType of the Chapter
       /// </summary>
        public string AssetType
        {
            get { return strAssetType; }
            set { strAssetType = value; }
        }

       /// <summary>
       /// Gets or Sets the AssetValue of the Chapter
       /// </summary>
       public string AssetValue
       {
           get { return strAssetValue; }
           set { strAssetValue = value; }
       }

       /// <summary>
       /// Gets or Sets the ChapterTitle of the Chapter
       /// </summary>
       public string ChapterTitle
       {
           get { return strChapterTitle; }
           set { strChapterTitle = value; }
       }
       
       /// <summary>
       /// Gets or Sets the ChapterDescription of the Chapter
       /// </summary>
       public string ChapterDescription
       {
           get { return strChapterDescription; }
           set { strChapterDescription = value; }
       }

       /// <summary>
       /// Gets or Sets the ChapterSequence of the Chapter
       /// </summary>
       public int ChapterSequence
       {
           get { return intChapterSequence; }
           set { intChapterSequence = value; }
       }

       /// <summary>
       /// Gets or Sets the Terminated Status of the Chapter
       /// </summary>
       public string Terminated
       {
           get { return strTerminated; }
           set { strTerminated = value; }
       }

       /// <summary>
       /// Gets or Sets the AssetType of the Chapter
       /// </summary>
       public string Country
       {
           get { return strCountry; }
           set { strCountry = value; }
       }
       /// <summary>
       /// Gets or Sets the AssetType of the Chapter
       /// </summary>
       public string ColumnName
       {
           get { return strColumnName; }
           set { strColumnName = value; }
       }
       /// <summary>
       /// Gets or Sets the AssetType of the Chapter
       /// </summary>
       public string Criteria
       {
           get { return strCriteria; }
           set { strCriteria = value; }
       }
       /// <summary>
       /// Gets or Sets the AssetType of the Chapter
       /// </summary>
       public string ActualAssetValue
       {
           get { return strActualAssetValue; }
           set { strActualAssetValue = value; }
       }

#endregion
    }
}

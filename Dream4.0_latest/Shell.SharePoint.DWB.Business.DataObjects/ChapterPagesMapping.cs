#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: ChapterPagesMapping.cs 
#endregion

using System;


namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// ChapterPagesMapping.cs stores the mapping of the chapters to the pages.
    /// </summary>
   public  class ChapterPagesMapping
   {
       #region DECLARATION

       int chapterID;
       string strPageName = string.Empty;
       string strPageActualName = string.Empty;
       string strTerminated = string.Empty;
       string strDiscipline = string.Empty;
       string strPageOwner = string.Empty;
       string strSignOffStatus = string.Empty;
       string strStandardOperatingProc = string.Empty;
       string strPublished = string.Empty;
       string strEmpty = string.Empty;
       int intMasterPageID;
       string strConnectionType = string.Empty;
       string strAssetType = string.Empty;
       string strPageURL = string.Empty;
       int intPageSequence;
       int intRowId;
       string strCreationDate = string.Empty;
       string strCreatedBy = string.Empty;
       #endregion

       #region PROPERTIES

       /// <summary>
       /// Gets/Sets Chapter ID
       /// </summary>
       public int ChapterID
       {
           get { return chapterID; }
           set { chapterID = value; }
       }
       /// <summary>
       /// Gets/Sets Row ID
       /// </summary>
       public int RowId
       {
           get { return intRowId; }
           set { intRowId = value; }
       }
       /// <summary>
       /// Gets/Sets Page Sequence
       /// </summary>
       public int PageSequence
       {
           get { return intPageSequence; }
           set { intPageSequence = value; }
       }

       /// <summary>
       /// Gets/Sets Page Name
       /// </summary>
       public string  PageName
       {
           get { return strPageName; }
           set { strPageName = value; }
       }

       /// <summary>
       /// Gets/Sets Standard Operating Proc
       /// </summary>
       public string StandardOperatingProc
       {
           get { return strStandardOperatingProc; }
           set { strStandardOperatingProc = value; }
       }

       /// <summary>
       /// Gets/Sets Page URL
       /// </summary>
       public string PageURL
       {
           get { return strPageURL; }
           set { strPageURL = value; }
       }

       /// <summary>
       /// Gets/Sets Asset Type
       /// </summary>
       public string AssetType
       {
           get { return strAssetType; }
           set { strAssetType = value; }
       }

       /// <summary>
       /// Gets/Sets Connection Type
       /// </summary>
       public string ConnectionType
       {
           get { return strConnectionType; }
           set { strConnectionType = value; }
       }

       /// <summary>
       /// Gets/Sets Page Actual Name
       /// </summary>
       public string PageActualName
       {
           get { return strPageActualName; }
           set { strPageActualName = value; }
       }

       /// <summary>
       /// Gets/Sets Terminated Status
       /// </summary>
       public string Terminated
       {
           get { return strTerminated; }
           set { strTerminated = value; }
       }
       
       /// <summary>
       /// Gets/Sets Discipline
       /// </summary>
       public string Discipline
       {
           get { return strDiscipline; }
           set { strDiscipline = value; }
       }

       /// <summary>
       /// Gets/Sets Page Owner
       /// </summary>
       public string PageOwner
       {
           get { return strPageOwner; }
           set { strPageOwner = value; }
       }

       /// <summary>
       /// Gets/Sets SignOff Status
       /// </summary>
       public string SignOffStatus
       {
           get { return strSignOffStatus; }
           set { strSignOffStatus = value; }
       }

       /// <summary>
       /// Gets/Sets Published Status
       /// </summary>
       public string Published
       {
           get { return strPublished; }
           set { strPublished = value; }
       }

       /// <summary>
       /// Gets/Sets Empty Status
       /// </summary>
       public string Empty
       {
           get { return strEmpty; }
           set { strEmpty = value; }
       }

       /// <summary>
       /// Gets/Sets MasterPage ID 
       /// </summary>
       public int MasterPageID
       {
           get { return intMasterPageID; }
           set { intMasterPageID = value; }
       }

       /// <summary>
       /// Gets/Sets Creation Date of Master Page 
       /// </summary>
       public string Created_Date
       {
           get { return strCreationDate; }
           set { strCreationDate = value; }
       }

       public string Created_By
       {
           get { return strCreatedBy; }
           set { strCreatedBy = value; }
       }
       #endregion
   }
}

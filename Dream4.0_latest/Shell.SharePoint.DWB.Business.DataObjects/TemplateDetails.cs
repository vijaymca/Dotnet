#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: TemplateDetails.cs 
#endregion

using System;
namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// Template Details
    /// </summary>
    /// 
    [Serializable]
   public  class TemplateDetails
   {
       #region DECLARATION

       string strTitle = string.Empty;
       string strAssetType = string.Empty;
       string strTerminated = string.Empty;
       string strAssetID = string.Empty;
       int intRowId;         

       #endregion

       #region PROPERTIES

       /// <summary>
       /// Gets/Sets Template ID.
       /// </summary>
       public int RowId
       {
           get { return intRowId; }
           set { intRowId = value; }
       }

       /// <summary>
       /// Gets/Sets Template terminated or not.
       /// </summary>
       public string Terminated
       {
           get { return strTerminated; }
           set { strTerminated = value; }
       }

       /// <summary>
       /// Gets/Sets Template Title.
       /// </summary>
       public string Title
       {
           get { return strTitle; }
           set { strTitle = value; }
       }

       /// <summary>
       /// Gets/Sets Template asset type.
       /// </summary>
       public string AssetType
       {
           get { return strAssetType; }
           set { strAssetType = value; }
       }

       /// <summary>
       /// Gets/Sets Template asset type ID.
       /// </summary>
       public string AssetID
       {
           get { return strAssetID; }
           set { strAssetID = value; }
       }
       #endregion 
   }
}

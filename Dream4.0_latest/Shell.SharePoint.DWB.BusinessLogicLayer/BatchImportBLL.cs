#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: BatchImportBLL.cs
#endregion
using System;
using System.Collections.Generic;
using System.Xml;
using Shell.SharePoint.DWB.DataAccessLayer;

namespace Shell.SharePoint.DWB.BusinessLogicLayer
{
    /// <summary>
    /// Batch Import Business logic class
    /// </summary>
    public class BatchImportBLL
    {
        #region DECLARATION
        BatchImportDAL objBatchImportDAL = null;
        #endregion
        #region PUBLIC METHODS
        public XmlDocument GetOnlyBatchImportXML(string bookID)
        {
            objBatchImportDAL = new BatchImportDAL();
            return objBatchImportDAL.GetOnlyBatchImportXML(bookID);
        }
        public void UploadToDocumentLib(string bookID, XmlDocument finalDocument)
        {
            objBatchImportDAL = new BatchImportDAL();
            objBatchImportDAL.UploadToDocumentLib(bookID,finalDocument);
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.Publishing.Internal;
using System.Web;
using System.IO;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DWB.BusinessLogicLayer;

namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// This class is used for Bulk Upload of Well Book Pages using SPLongRunningOperationJob.
    /// </summary>
    public class BatchImportLongRunningProcess : LongRunningOperationJob
    {
        #region VARIBLE DECLARATION
        SPWeb Web;
        const string USERDEFINEDDOCUMENTLIST = "DWB UserDefined Documents";
        DataTable dtBatchUpload = null;
        WellBookBLL objWellBookBLL;
        const string BATCHIMPORTLOGLISTNAME = "DWB Batch Import Log Report";
        #endregion VARIBLE DECLARATION

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="BatchImportLongRunningProcess"/> class.
        /// </summary>
        /// <param name="web">The web.</param>
        /// <param name="batchUpload">The batch upload.</param>
        public BatchImportLongRunningProcess(SPWeb web, DataTable batchUpload)
        {
            Web = web;
            if (batchUpload != null)
                dtBatchUpload = batchUpload;

        }
        #endregion Constructor

        #region Overridden Methods
        /// <summary>
        /// DoWork is virtual method of LongRunningOperationJob base class and overrided into BatchImportLongRunningProcess.
        /// </summary>
        public override void DoWork()
        {
            this.StatusDescription = "Batch Import started...";
            this.UpdateStatus();
            string strError = string.Empty;
            this.Title = "Batch Import";
            this.WaitMessage = "Please wait while all the documents are uploaded...";
            this.RedirectWhenFinished = false;
            this.UserCanCancel = true;

            this.MillisecondsToWaitForFinish = 100;
            this.TotalOperationsToBePerformed = 100;
            string strCompletePath = string.Empty;
            string strFileName = string.Empty;
            int intCounter = 1;
            DataTable dtBatchLog = new DataTable();
            dtBatchLog.Columns.Add("PageName");
            dtBatchLog.Columns.Add("ChapterName");
            dtBatchLog.Columns.Add("DateAndTime");
            dtBatchLog.Columns.Add("Status");
            dtBatchLog.Columns.Add("Detail");

            foreach (DataRow eachRow in dtBatchUpload.Rows)
            {
                strError = string.Empty;
                this.StatusDescription = "Uploading Document " + intCounter.ToString();
                this.UpdateStatus();
                try
                {
                    #region Call UploadFile to document library
                    if (this.UserRequestedCancel)
                    {
                        break;
                    }
                    else
                    {
                        //Fetch the complete path from data table
                        strCompletePath = eachRow["CompleteSharedPath"].ToString();
                        if (!string.IsNullOrEmpty(strCompletePath))
                        {
                            strFileName = strCompletePath.Substring(strCompletePath.LastIndexOf("\\") + 1);
                            UploadFileToDocumentLibrary(strCompletePath, strFileName, eachRow["PageId"].ToString());
                        }

                    }
                    #endregion Call UploadFile to document library
                }
                catch (FileNotFoundException fnfException)
                {
                    strError = fnfException.Message;
                    dtBatchLog.Rows.Add(eachRow["PageName"].ToString(), eachRow["ChapterName"].ToString(), DateTime.Now, "Fail", strError);
                    continue;
                }
                catch (Exception Ex)
                {
                    strError = Ex.Message;
                    dtBatchLog.Rows.Add(eachRow["PageName"].ToString(), eachRow["ChapterName"].ToString(), DateTime.Now, "Fail", strError);
                    continue;
                }

                if (string.IsNullOrEmpty(strError))
                {
                    dtBatchLog.Rows.Add(eachRow["PageName"].ToString(), eachRow["ChapterName"].ToString(), DateTime.Now, "Success", "");
                }

                this.OperationsPerformed++;
            }

            //Add BatchLog data table to sharepoint list.
            objWellBookBLL = new WellBookBLL();
            objWellBookBLL.SaveBatchImportLog(Web.Url, BATCHIMPORTLOGLISTNAME, dtBatchLog);

            if (this.UserRequestedCancel)
            {
                this.StatusDescription = "Batch Import Could not complete as you have canceled the process in between.";
            }
            else
            {
                this.StatusDescription = "Batch Import Process completed successfully, Please check the logs to know more details.";
            }
            this.NavigateWhenDoneUrl = "/Pages/BatchImportStatus.aspx?status=" + this.StatusDescription;
            this.UpdateStatus();

        }

        #endregion Overridden Methods

        #region Private Methods

        /// <summary>
        /// Uploads the file to document library.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="pageID">The page ID.</param>
        private void UploadFileToDocumentLibrary(string path, string fileName, string pageID)
        {
            FileStream fStream = null;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                     fStream = File.OpenRead(path);
                });
            }
            catch (Exception)
            {
                throw;
            }
            try
            {
                if (fStream != null)
                {
                    byte[] contents = new byte[fStream.Length];
                    fStream.Read(contents, 0, (int)fStream.Length);
                    fStream.Close();
                    fStream.Dispose();
                    UploadFiletoDocLibrary(USERDEFINEDDOCUMENTLIST, pageID, fileName, contents);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (fStream != null)
                {
                    fStream.Close();
                    fStream.Dispose();
                }
            }
        }

        /// <summary>
        /// Uploads the fileto doc library.
        /// </summary>
        /// <param name="docLibName">Name of the doc lib.</param>
        /// <param name="pageId">The page id.</param>
        /// <param name="postedFileName">Name of the posted file.</param>
        /// <param name="postedFile">The posted file.</param>
        private void UploadFiletoDocLibrary(string docLibName, string pageId, string postedFileName, byte[] postedFile)
        {
            CommonBLL objCommonBLL = new CommonBLL();
            objCommonBLL.UploadFileToDocumentLibrary(Web.Url, docLibName, pageId, Web.CurrentUser.Name, postedFileName, postedFile);
        }

        #endregion Private Methods
    }
}

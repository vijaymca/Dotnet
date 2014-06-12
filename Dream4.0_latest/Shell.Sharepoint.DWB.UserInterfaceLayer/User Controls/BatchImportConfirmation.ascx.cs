#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: BatchImportConfirmation.ascx.cs
//Modified By:Gopinath
//Date: 172-11-2010
//Reason: To implement the Batch Import Confirmation.
#endregion

using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Shell.SharePoint.DWB.BusinessLogicLayer;
using System.Net;
using System.Xml;
using Shell.SharePoint.DREAM.Utilities;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;


namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// This class is used for Bulk Upload of Well Book Pages.
    /// </summary>
    public partial class BatchImportConfirmation : UIHelper
    {
        #region VARIBLE DECLARATION

        const string QUERYSTRINGBOOKID = "BookID";
        const string REPORTXPATH = "response/report";
        const string RECORDXPATH = "/response/report/record";
        const string ASSETVALUE = "%Asset_Value%";
        const string CHAPTERNAME = "%Title%";
        const string ACTUALASSETVALUE = "%Actual_Asset_Value%";
        const string SPACE = " %";
        const string UNDERSCORE = "_%";
        const string BATCHIMPORTLOGLISTNAME = "DWB Batch Import Log Report";
        const string DWBWELLBOOKAUDITLIST = "DWB Books Audit Trail";
        const string AUDIT_ACTION_BATCHIMPORT = "13";
        string strWellBookId;
        WellBookBLL objWellBookBLL;

        #endregion VARIBLE DECLARATION

        #region EventHandler Methods

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (HttpContext.Current.Request.QueryString[QUERYSTRINGBOOKID]!=null)
                {
                    strWellBookId = HttpContext.Current.Request.QueryString[QUERYSTRINGBOOKID];
                    lblBookName.Text += GetBookNameByBookId(strWellBookId);

                    GetBatchImportXML(strWellBookId);
                    btnContinue.OnClientClick = "return CheckAtleastOneRecordSelected();";
                }
            }
            #region Exception Block
            catch (WebException webEx)
            {
                lblException.Text = webEx.Message;
                lblException.Visible = true;
                ExceptionBlock.Visible = true;
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strParentSiteURL, ex);
            }
            #endregion Exception Block
        }

        /// <summary>
        /// Handles the Click event of the btnContinue control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnContinue_Click(object sender, EventArgs e)
        {

            CheckBox chkAll;
            List<string> PageNamesList = new List<string>();
            List<string> SharedPathList = new List<string>();
            List<string> FileFormatList = new List<string>();
            List<string> FileActualFormatList = new List<string>();
            List<string> FileTypeList = new List<string>();

            try
            {
                foreach (GridViewRow row in gvBatchImportConfirmation.Rows)
                {
                    chkAll = (CheckBox)row.Cells[0].FindControl("chbSelectID");
                    if (chkAll != null && chkAll.Checked)
                    {
                        PageNamesList.Add(HttpUtility.HtmlDecode(row.Cells[1].Text.Trim()));
                        FileFormatList.Add(HttpUtility.HtmlDecode(row.Cells[4].Text.Trim()));
                        FileActualFormatList.Add(HttpUtility.HtmlDecode(row.Cells[5].Text.Trim()));
                        FileTypeList.Add(HttpUtility.HtmlDecode(row.Cells[6].Text.Trim()));
                        SharedPathList.Add(HttpUtility.HtmlDecode(row.Cells[7].Text.Trim()));
                    }
                }                           
            }
            #region Exception Block
            catch (WebException webEx)
            {
                lblException.Text = webEx.Message;
                lblException.Visible = true;
                ExceptionBlock.Visible = true;
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strParentSiteURL, ex);
            }
            #endregion Exception Block
            ProcessUpload(PageNamesList, SharedPathList, FileFormatList, FileActualFormatList, FileTypeList);
        }

        /// <summary>
        /// Handles the RowDataBound event of the gvBatchImportConfirmation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gvBatchImportConfirmation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk = (CheckBox)e.Row.FindControl("chbSelectID");
                chk.Attributes.Add("onclick", "return SelectAllWithHeader();");
            }
        }

        #endregion EventHandler Methods

        #region Private Methods

        /// <summary>
        /// Processes the upload.
        /// </summary>
        /// <param name="PageNamesList">The page names list.</param>
        /// <param name="SharedPathList">The shared path list.</param>
        /// <param name="FileFormatList">The file format list.</param>
        /// <param name="FileActualFormatList">The file actual format list.</param>
        /// <param name="FileTypeList">The file type list.</param>
        private void ProcessUpload(List<string> PageNamesList, List<string> SharedPathList, List<string> FileFormatList, List<string> FileActualFormatList, List<string> FileTypeList)
        {
            DataTable dtPageDetails = null;
            DataTable dtChapterDetails = null;
            string strPageName = string.Empty;
            string strSharedPath = string.Empty;
            string strCompletePath = string.Empty;            
            DataTable dtBatchUpload = new DataTable();
            dtBatchUpload.Columns.Add("PageId");
            dtBatchUpload.Columns.Add("CompleteSharedPath");
            dtBatchUpload.Columns.Add("PageName");
            dtBatchUpload.Columns.Add("ChapterName");

            //Get PageIds and ChapterIds
            #region Each Page Name
            for (int index=0; index<PageNamesList.Count; index++)
            {
                //NOTE : Same index going to used for all List types.
                //dtPageDetails contains PageId and ChapterId.
                dtPageDetails = new DataTable();
                dtPageDetails = GetPageIdsByPageName(PageNamesList[index]);

                if (dtPageDetails != null && dtPageDetails.Rows.Count > 0)
                {
                    //Loop through page Ids, under each page id frame the file path by appending chapter names, asset types.
                    foreach (DataRow rowPageInfo in dtPageDetails.Rows)//By PageIds
                    {
                        dtChapterDetails = new DataTable();
                        //dtChapterDetails contains ChapterNames, Actual Asset Value and Asset Value.
                        dtChapterDetails = GetChapterNameByChapterId(rowPageInfo[1].ToString()); //Index 1 gives the chapter Ids                                

                        //only we get one record at time
                        if (dtChapterDetails != null && dtChapterDetails.Rows.Count > 0)
                        {                           
                            foreach (DataRow rowChapInfo in dtChapterDetails.Rows)
                            {
                                //rowChapInfo[0] --> ChapterName; rowChapInfo[1] --> Asset Type
                                strCompletePath = FrameCompleteFileUploadPath(SharedPathList[index], FileFormatList[index], FileActualFormatList[index], FileTypeList[index], PageNamesList[index], rowChapInfo[0].ToString(), rowChapInfo[1].ToString(), rowChapInfo[2].ToString());
                               
                                //Upload bulk pages                                
                                dtBatchUpload.Rows.Add(rowPageInfo[0].ToString(), strCompletePath, PageNamesList[index], rowChapInfo[0].ToString());
                            }                       
                            
                        }
                    }
                }
            }
            #endregion Each Page Name

            //Batch upload
            UploadEachPageToDocLib(dtBatchUpload);

        }

        /// <summary>
        /// Uploads the each page to doc lib.
        /// </summary>
        /// <param name="dtBatchUpload">The dt batch upload.</param>
        private void UploadEachPageToDocLib(DataTable dtBatchUpload)
        {
            try
            {
                //SPWEB used for Current Batch import log running process.
                SPWeb Web = SPContext.Current.Web;

                //Clear the Batch Import log List before starting the Batch new import process.
                objWellBookBLL = new WellBookBLL();
                objWellBookBLL.ClearBatchImportLog(Web.Url, BATCHIMPORTLOGLISTNAME);

                
                BatchImportLongRunningProcess objBatchImportLong = new BatchImportLongRunningProcess(Web, dtBatchUpload);
                UpdateAuditTrailForBatchImport();
                Web.AllowUnsafeUpdates = true;
                Web.Update();
                objBatchImportLong.Start(Web);                
                string url = string.Format("{0}{1}?JobId={2}", Web.Url, "/_layouts/LongRunningOperationProgress.aspx", objBatchImportLong.JobId);
                SPUtility.Redirect(url, SPRedirectFlags.Default, this.Context);
            }
            catch (Exception)
            {
                throw;
            }            
        }

        /// <summary>
        /// To Update Batch Import Audit History 
        /// </summary>
        private void UpdateAuditTrailForBatchImport()
        {           
            objWellBookBLL=new WellBookBLL();
            objWellBookBLL.UpdateBatchImportAuditHistory(strParentSiteURL,strWellBookId, AUDIT_ACTION_BATCHIMPORT);
        }

        /// <summary>
        /// Frames the complete file upload path.
        /// </summary>
        /// <param name="sharedPath">The shared path.</param>
        /// <param name="fileFormat">The file format.</param>
        /// <param name="fileActualFormat">The file actual format.</param>
        /// <param name="fileType">Type of the file.</param>
        /// <param name="pageName">Name of the page.</param>
        /// <param name="chapterName">Name of the chapter.</param>
        /// <param name="assetValue">The asset value.</param>
        /// <param name="actualAssetValue">The actual asset value.</param>
        /// <returns></returns>
        private string FrameCompleteFileUploadPath(string sharedPath, string fileFormat, string fileActualFormat, string fileType, string pageName, string chapterName, string assetValue, string actualAssetValue)
        {
            int intMiddleChar;
            string strCompletePath = string.Empty;
            string strActualValue = string.Empty;

            #region Frame File Path
            //AssetName or ChapterName with Space
            if (fileActualFormat.Contains(SPACE))//Space
            {
                intMiddleChar = fileActualFormat.IndexOf(SPACE);
                strActualValue = fileActualFormat.Substring(0, intMiddleChar);
                if (strActualValue.CompareTo(ASSETVALUE) == 0)
                {
                    strActualValue = assetValue;
                }
                else if (strActualValue.CompareTo(CHAPTERNAME) == 0)
                {
                    strActualValue = chapterName;
                }
                else if (strActualValue.CompareTo(ACTUALASSETVALUE) == 0)
                {
                    strActualValue = actualAssetValue;
                }
                //File Name : Append Actual value, space with page name and file type
                strActualValue = strActualValue + " " + pageName;
            }
            //AssetName or ChapterName with Underscore
            else if (fileActualFormat.Contains(UNDERSCORE))//Underscore
            {
                intMiddleChar = fileActualFormat.IndexOf(UNDERSCORE);
                strActualValue = fileActualFormat.Substring(0, intMiddleChar);

                if (strActualValue.CompareTo(ASSETVALUE) == 0)
                {
                    strActualValue = assetValue;
                }
                else if (strActualValue.CompareTo(CHAPTERNAME) == 0)
                {
                    strActualValue = chapterName;
                }
                else if (strActualValue.CompareTo(ACTUALASSETVALUE) == 0)
                {
                    strActualValue = actualAssetValue;
                }
                //File Name : Append Actual value, underscore with page name and file type
                // Ex: (Asset_Type or ChapterName)+ (" "/_)+PageName
                strActualValue = strActualValue + "_" + pageName; 
            }

            if (!string.IsNullOrEmpty(strActualValue))
            {
                //Ex: //SharePath/(Asset_Type or ChapterName)+(" "/_)+PageName+FileType
                strCompletePath = CheckSharedPathWithSlash(sharedPath) + strActualValue + fileType;
            }

            #endregion Frame File Path
            return strCompletePath;
        }

        /// <summary>
        /// Fetch Book Name by passing book Id
        /// <remarks>Added By Gopinath</remarks>
        /// <remarks>Date : 18-11-2010</remarks>
        /// </summary>
        /// <param name="BookId">string</param>
        /// <returns>string</returns>
        private string GetBookNameByBookId(string BookId)
        {
            DataTable dtResults = null;
            string strBookName = string.Empty;
            string strCamlQuery = @"<Where><Eq><FieldRef Name='ID'/><Value Type='Counter'>" + BookId + "</Value></Eq></Where>";
            string strViewFields = "<FieldRef Name='Title'/>";

            dtResults = GetListItems(DWBBOOKLIST, strCamlQuery, strViewFields);
            if (dtResults != null && dtResults.Rows.Count > 0)
            {
                strBookName = dtResults.Rows[0][0].ToString();
            }

            return strBookName;
        }

        /// <summary>
        /// Gets the name of the page ids by page.
        /// </summary>
        /// <param name="PageName">Name of the page.</param>
        /// <returns></returns>
        private DataTable GetPageIdsByPageName(string PageName)
        {
            DataTable dtPageDetails = null;
            DataView dvPageDetails = null;
             
            string strCamlQuery = @"<Where><And><Eq><FieldRef Name='Page_Name' /><Value Type='Text'>" + PageName + "</Value></Eq><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq></And></Where>";
            string strViewFields = "<FieldRef Name='ID'/><FieldRef Name='Chapter_ID'/>";
            dtPageDetails = GetListItems(CHAPTERPAGESMAPPINGLIST, strCamlQuery, strViewFields);
            if (dtPageDetails != null && dtPageDetails.Rows.Count > 0)
            {
                dvPageDetails = dtPageDetails.DefaultView;
                dtPageDetails = dvPageDetails.ToTable(true, "ID", "Chapter_ID");
            } 
            return dtPageDetails;
        }

        /// <summary>
        /// Gets the chapter name by chapter id.
        /// </summary>
        /// <param name="ChapterId">The chapter id.</param>
        /// <returns></returns>
        private DataTable GetChapterNameByChapterId(string ChapterId)
        {
            DataTable dtChapterDetails = null;
            DataView dvChapterDetails = null;

            string strCamlQuery = @"<Where><And><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + ChapterId + "</Value></Eq><Eq><FieldRef Name='Book_ID' /><Value Type='Number'>" + strWellBookId + "</Value></Eq></And></Where>";
            string strViewFields = "<FieldRef Name='Title' /><FieldRef Name='Asset_Value' /><FieldRef Name='Actual_Asset_Value' />";

            dtChapterDetails = GetListItems(DWBCHAPTERLIST, strCamlQuery, strViewFields);

            if (dtChapterDetails != null && dtChapterDetails.Rows.Count > 0)
            {
                dvChapterDetails = dtChapterDetails.DefaultView;
                dtChapterDetails = dvChapterDetails.ToTable(true, "Title", "Asset_Value", "Actual_Asset_Value");
            } 

            return dtChapterDetails;
        }

        /// <summary>
        /// Gets the batch import XML.
        /// </summary>
        /// <param name="bookID">The book ID.</param>
        private void GetBatchImportXML(string bookID)
        {
            XmlDocument batchImportDoc = null;
            BatchImportBLL ObjBatchImportBLL = new BatchImportBLL();
            batchImportDoc = ObjBatchImportBLL.GetOnlyBatchImportXML(bookID);

            //Bind the gridview
            if (batchImportDoc != null)
            {
                DataTable dtXMLResults = GetXMLFileData(batchImportDoc, bookID);

                if (dtXMLResults != null && dtXMLResults.Rows.Count > 0)
                {
                    ExceptionBlock.Visible = false;
                    lblException.Visible = false;
                    btnContinue.Visible = true;

                    gvBatchImportConfirmation.DataSource = dtXMLResults;
                    gvBatchImportConfirmation.DataBind();
                }
                else
                {
                    ExceptionBlock.Visible = true;
                    lblException.Visible = true;
                    btnContinue.Visible = false; 
                    lblException.Text = "There are currently no details to be displayed.";
                }
            }

        }

        /// <summary>
        /// Gets the XML file data.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="bookID">The book ID.</param>
        /// <returns></returns>
        private DataTable GetXMLFileData(XmlDocument document, string bookID)
        {
            string strXpath = "/batchImport[@BookID='" + bookID + "']/pageName";
            DataTable dtBatchImportConfirm = new DataTable();
            dtBatchImportConfirm.Columns.Add("PageName");
            dtBatchImportConfirm.Columns.Add("CompleteSharedPath");
            dtBatchImportConfirm.Columns.Add("ActualFileFormat");
            dtBatchImportConfirm.Columns.Add("FileFormat");
            dtBatchImportConfirm.Columns.Add("FileType");
            dtBatchImportConfirm.Columns.Add("NoOfPages");
            dtBatchImportConfirm.Columns.Add("SharedPath");          

            XmlNodeList xmlMainNodeList = null;
            //Declared
            string strPageName = string.Empty;
            string strPageCount = string.Empty;
            string strSharedPath = string.Empty;
            string strFileFormat = string.Empty;
            string strFileType = string.Empty;
            string strCompletePath = string.Empty;
            string strActualFileFormat = string.Empty;

            try
            {
                //Check all the required objects are NULL
                if (document != null)
                {
                    //All PageName Nodes List
                    xmlMainNodeList = document.SelectNodes(strXpath);

                    if (xmlMainNodeList != null)
                    {
                        foreach (XmlNode xmlMainNode in xmlMainNodeList)
                        {
                            #region Fetch values from XML and add record to the Data table
                            //Check Current Field Exists
                            if (xmlMainNode.Attributes["name"] != null)
                                strPageName = xmlMainNode.Attributes["name"].Value;

                            if (xmlMainNode.Attributes["PageCount"] != null)
                                strPageCount = xmlMainNode.Attributes["PageCount"].Value;

                            //Check path end with "/" PENDING
                            if (xmlMainNode.SelectSingleNode("sharedPath") != null && xmlMainNode.SelectSingleNode("sharedPath").Attributes["path"] != null)
                                strSharedPath = xmlMainNode.SelectSingleNode("sharedPath").Attributes["path"].Value;

                            if (xmlMainNode.SelectSingleNode("fileFormat") != null && xmlMainNode.SelectSingleNode("fileFormat").Attributes["format"] != null)
                                strFileFormat = xmlMainNode.SelectSingleNode("fileFormat").Attributes["format"].Value;

                            if (xmlMainNode.SelectSingleNode("fileType") != null && xmlMainNode.SelectSingleNode("fileType").Attributes["type"] != null)
                                strFileType = xmlMainNode.SelectSingleNode("fileType").Attributes["type"].Value;
                            if (xmlMainNode.SelectSingleNode("fileFormat") != null && xmlMainNode.SelectSingleNode("fileFormat").Attributes["actualFormat"] != null)
                                strActualFileFormat = xmlMainNode.SelectSingleNode("fileFormat").Attributes["actualFormat"].Value;

                            //Append strSharedPath+strFileFormat+strFileType
                            strCompletePath = CheckSharedPathWithSlash(strSharedPath) + strFileFormat + strFileType;

                            dtBatchImportConfirm.Rows.Add(strPageName, strCompletePath, strActualFileFormat, strFileFormat, strFileType, strPageCount, strSharedPath);
                            #endregion Fetch values from XML and add record to the Data table

                        }
                    }
                }

            }
            #region Exception Block
            catch (WebException webEx)
            {
                lblException.Text = webEx.Message;
                lblException.Visible = true;
                ExceptionBlock.Visible = true;
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strParentSiteURL, ex);
            }
            #endregion Exception Block

            return dtBatchImportConfirm;
        }

        /// <summary>
        /// Checks the shared path with slash.
        /// </summary>
        /// <param name="SharedPath">The shared path.</param>
        /// <returns></returns>
        private string CheckSharedPathWithSlash(string SharedPath)
        {
            if (!string.IsNullOrEmpty(SharedPath))
            {
                bool blnCheckBackSlash = SharedPath.EndsWith("\\");
                if (!blnCheckBackSlash)
                {
                    SharedPath = SharedPath + "\\";
                }
            }
            return SharedPath;
        }

        #endregion Private Methods

        
    }
}
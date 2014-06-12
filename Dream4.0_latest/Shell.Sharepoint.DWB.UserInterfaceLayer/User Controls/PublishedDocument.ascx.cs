#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: PublishedDocument.ascx.cs 
#endregion

/// <summary> 
/// This is UserUploaded user control class. This class is used for handling the 
/// Type III reports 
/// </summary> 

using System;
using System.Data;
using System.Net;
using System.Web;
using System.Web.UI;
using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DWB.BusinessLogicLayer;
using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// UserUpload class for enabling the user to view/upload  Type III documents
    /// </summary>
    public partial class PublishedDocument : UIHelper
    {
        #region Declarations
        const string EPCATALOGUE = "EPFilterScreen.aspx";
      

        string strPageID = string.Empty;
        CommonBLL objCommon;                     
        string strMode = string.Empty;
        string strChapterID = string.Empty;
        TreeNodeSelection objTreeNode;
        const string DATEFORMAT = "Date Format";

        /// <summary>
        /// Gets or sets the book ID.
        /// </summary>
        /// <value>The book ID.</value>
        string PageID
        {
            get { return strPageID; }
            set { strPageID = value; }
        }
        /// <summary>
        /// Gets or sets the book ID.
        /// </summary>
        /// <value>The book ID.</value>
        string ChapterID
        {
            get { return strChapterID; }
            set { strChapterID = value; }
        }
        #endregion Declarations

        #region Protected Methods
        /// <summary>
        /// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"></see> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the server control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            try
            {
                btnPrint.Attributes.Add(ONCLICKEVENTNAME, base.RegisterJavaScript());
                if (HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES] != null &&
                    ((TreeNodeSelection)HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES]).IsTypeIISelected)
                {
                    AdvancedSearchContent.Visible = true;
                    objTreeNode = (TreeNodeSelection)HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES];
                    PageID = objTreeNode.PageID;
                    ChapterID = objTreeNode.ChapterID;
                    LoadMetaData();
                    btnUpdate.Attributes.Add(ONCLICKEVENTNAME, "javascript:UpdatePageContents('" + PageID + "','2');");
                }
                else if (HttpContext.Current.Request.QueryString[MODEQUERYSTRING] != null)
                {
                    AdvancedSearchContent.Visible = true;
                    PageID = HttpContext.Current.Request.QueryString[PAGEIDQUERYSTRING];
                    ChapterID = HttpContext.Current.Request.QueryString[CHAPTERIDQUERYSTRING];
                    LoadMetaData();
                    btnUpdate.Attributes.Add(ONCLICKEVENTNAME, "javascript:UpdatePageContents('" + PageID + "','2');");
                }
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(strParentSiteURL, webEx);
                RenderException(webEx.Message);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strParentSiteURL, ex);
            }
            base.Render(writer);
        }

        /// <summary>
        /// Updates whether the document has been signed off or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSignOff_ServerClick(object sender, EventArgs e)
        {
            try
            {
                objCommon = new CommonBLL();
                if (HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES] != null &&
                    ((TreeNodeSelection)HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES]).IsTypeIISelected)
                {
                    objTreeNode = (TreeNodeSelection)HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES];
                    PageID = objTreeNode.PageID;
                    ChapterID = objTreeNode.ChapterID;
                }
                else if (HttpContext.Current.Request.QueryString[MODEQUERYSTRING] != null)
                {
                    PageID = HttpContext.Current.Request.QueryString[PAGEIDQUERYSTRING];
                    ChapterID = HttpContext.Current.Request.QueryString[CHAPTERIDQUERYSTRING];
                }
                LoadMetaData();
                if (string.Equals(btnSignOff.Text, CANCELSIGNOFF))
                {
                    objCommon.SignOffPage(strParentSiteURL, CHAPTERPAGESMAPPINGLIST, hidPageId.Value, CHAPTERPAGESMAPPINGAUDITLIST, false);
                }
                else
                {
                    objCommon.SignOffPage(strParentSiteURL, CHAPTERPAGESMAPPINGLIST, hidPageId.Value, CHAPTERPAGESMAPPINGAUDITLIST, true);
                }
                LoadMetaData();               
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(strParentSiteURL, webEx);
                RenderException(webEx.Message);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strParentSiteURL, ex);
            }
        }

        /// <summary>
        /// Handles the ServerClick event of the btnPrint control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnPrint_ServerClick(object sender, EventArgs e)
        {
            int intPrintedDocID = 0;
            string[] strPrintedDocURL = null;
            CommonBLL objCommonBLL = null;
            string strCamlQuery = string.Empty;

            PrintOptions objPrintOptions = new PrintOptions();
          

            try
            {
                PageID = HttpContext.Current.Request.QueryString[PAGEIDQUERYSTRING];
                if (string.Compare(hdnIncludeStoryBoard.Value.ToLowerInvariant(), "true") == 0)
                {
                    objPrintOptions.IncludeStoryBoard = true;
                }
                if (string.Compare(hdnIncludePageTitle.Value.ToLowerInvariant(), "true") == 0)
                {
                    objPrintOptions.IncludePageTitle = true;
                }
              
                ///  Generate XML to Print PDF and returns the PDF id stored in document library
                intPrintedDocID = Print(objPrintOptions, WELLBOOKVIEWERCONTROLPAGE, HttpContext.Current.Request.QueryString[MODEQUERYSTRING]);

                /// Query the Printed Document library to get the recently printed document and show in IE.
                if (intPrintedDocID > 0)
                {
                    objCommonBLL = new CommonBLL();
                    strCamlQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" +
                   intPrintedDocID.ToString() + "</Value></Eq></Where>";
                    strPrintedDocURL = objCommonBLL.GetPrintedDocumentUrl(strParentSiteURL, PRINTEDDOCUMNETLIBRARY, strCamlQuery);
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "documentprinted", "window.open('" + @strDocumentURL + "', 'PDFViewer', 'scrollbars,resizable,status,toolbar,menubar');", true);
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "documentnotprinted", ALERTDOCUMENTNOTPRINTED, true);
                }
            }
            catch (System.Web.Services.Protocols.SoapException soapEx)
            {
                CommonUtility.HandleException(strParentSiteURL, soapEx);
                RenderException(soapEx.Message);
                PdfBLL objPdfBLL = new PdfBLL();
                objPdfBLL.PrintLog(strParentSiteURL, soapEx.StackTrace, soapEx.Message, "Type2.btnPrint_Click");

            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(strParentSiteURL, webEx);
                RenderException(webEx.Message);
                PdfBLL objPdfBLL = new PdfBLL();
                objPdfBLL.PrintLog(strParentSiteURL, webEx.StackTrace, webEx.Message, "Type2.btnPrint_Click");
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strParentSiteURL, ex);
                PdfBLL objPdfBLL = new PdfBLL();
                objPdfBLL.PrintLog(strParentSiteURL, ex.StackTrace, ex.Message, "Type2.btnPrint_Click");
            }
                  
        }
    
        /// <summary>
        /// Handles the Click event of the btnFirePostBack control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnFirePostBack_Click(object sender, EventArgs e)
        {
            /// This method is called when document is uploaded successfully.
        }
        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Sets the book detail data object.
        /// </summary>
        private void LoadMetaData()
        {

            string strCamlQuery = string.Empty;
            string strPageOwner = string.Empty;
            string strPageDiscipline = string.Empty;
            string strBookOwner = string.Empty;
            string strBookTeamID = string.Empty;
            string strViewFields = string.Empty;
            DataTable dtBookPage = null;
            DataTable dtPublishedDocDetails = null;
            btnSignOff.Visible = false;
            btnUpdate.Visible = false;
            linkEPCatalogFilter.Visible = false;
            bool blnShowUpdate = false;
            lblModifiedDate.Visible = false;

            if (HttpContext.Current.Request.QueryString[MODEQUERYSTRING] != null)
                strMode = HttpContext.Current.Request.QueryString[MODEQUERYSTRING];
            hidPageId.Value = PageID;
            try
            {
                objCommon = new CommonBLL();

                strCamlQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Number'>" +
                    PageID + "</Value></Eq></Where>";
                dtBookPage = objCommon.ReadList(strParentSiteURL, CHAPTERPAGESMAPPINGLIST, strCamlQuery);
                if (dtBookPage.Rows.Count > 0)
                {
                    lblOwner.Text = dtBookPage.Rows[0]["Owner"].ToString();
                    lblSignedOff.Text = dtBookPage.Rows[0]["Sign_Off_Status"].ToString();
                    lblTitleTemplate.Text = dtBookPage.Rows[0]["Page_Actual_Name"].ToString();
                    strPageDiscipline = dtBookPage.Rows[0]["Discipline"].ToString();
                    strPageOwner = dtBookPage.Rows[0]["Owner"].ToString();
                    string strAssetValue = objCommon.GetAssetValue(ChapterID);
                    /// If $ is present in Page Title replace with Asset Value
                    /// Else prefix with Asset Value.
                    if (lblTitleTemplate.Text.IndexOf("$") == 0)
                    {
                        lblTitleTemplate.Text = lblTitleTemplate.Text.Replace("$", strAssetValue);
                    }
                    else
                    {
                        lblTitleTemplate.Text = string.Concat(strAssetValue, "-", lblTitleTemplate.Text);
                    }
                }

                if (string.Compare(lblSignedOff.Text.ToLowerInvariant(), STATUSSIGNEDOFF) == 0)
                {
                    blnShowUpdate = false;
                    btnSignOff.Text = CANCELSIGNOFF;
                }
                else
                {
                    blnShowUpdate = true;
                    btnSignOff.Text = SIGNOFF;
                }

                strCamlQuery = @"<Where><Eq><FieldRef Name='PageID' /><Value Type='Number'>" +
                  PageID + "</Value></Eq></Where>";

                string strObjectInnerHtml = objCommon.GetUploadedDocumentUrl(strParentSiteURL, PUBLISHEDDOCUMENTLIST, strCamlQuery);
                ///loads the embedded image/document/application 
                docviewerdiv.InnerHtml = strObjectInnerHtml;

                //Added by Praveena for module "Add Last Updated date"
                strViewFields = @"<FieldRef Name='PageID' /><FieldRef Name='Modified' />";
                dtPublishedDocDetails = objCommon.ReadList(strParentSiteURL, PUBLISHEDDOCUMENTLIST, strCamlQuery, strViewFields);
                if (dtPublishedDocDetails != null && dtPublishedDocDetails.Rows.Count > 0)
                {
                    string dtModifiedDate = GetDateTime(dtPublishedDocDetails.Rows[0]["Modified"].ToString());
                    lblModifiedDate.Visible = true;
                    lblModifiedDate.Text = "Last Updated Date:" + dtModifiedDate;
                }

                /// EPCatalog Issue Fix
                BookInfo objBookInfo = null;
                if (HttpContext.Current.Session[SESSIONTREEVIEWDATAOBJECT] != null)
                {
                    string strAssetType = string.Empty;
                    objBookInfo = ((BookInfo)HttpContext.Current.Session[SESSIONTREEVIEWDATAOBJECT]);
                    if (objBookInfo != null)
                    {
                        strBookOwner = objBookInfo.BookOwner;
                        strBookTeamID = objBookInfo.BookTeamID;
                        foreach (ChapterInfo objChapterInfo in objBookInfo.Chapters)
                        {
                            if (string.Equals(objChapterInfo.ChapterID, ChapterID))
                            {
                                hidSelectedRows.Value = objChapterInfo.ActualAssetValue;
                                /// To send as query string to EPCatalog screen
                                hidAssetType.Value = objChapterInfo.AssetType;
                                if (string.Equals(objChapterInfo.AssetType, ASSETTYPEWELL))
                                    hidSelectedCriteriaName.Value =UWI;
                                else if (string.Equals(objChapterInfo.AssetType, ASSETTYPEFIELD))
                                    hidSelectedCriteriaName.Value = FIELDNAME;
                                else
                                    hidSelectedCriteriaName.Value = UWBI;
                            }
                        }
                    }
                }
                if (ShowButton(WELLBOOKVIEWERCONTROLPAGE, strBookOwner, strBookTeamID, strPageOwner, strPageDiscipline))
                {
                    btnSignOff.Visible = true;
                }
                if (ShowButton(WELLBOOKVIEWERCONTROLPAGEUPDATE, strBookOwner, strBookTeamID, strPageOwner, strPageDiscipline))
                {
                    if (blnShowUpdate)
                    {
                        btnUpdate.Visible = true;
                        linkEPCatalogFilter.Visible = true;
                    }
                }

                if (!string.IsNullOrEmpty(strMode) && strMode.Equals(VIEW))
                {
                    btnUpdate.Visible = false;
                    btnSignOff.Visible = false;
                    linkEPCatalogFilter.Visible = false;
                }

            }
            catch
            {
                throw;
            }
            finally
            {
                if (dtBookPage != null)
                    dtBookPage.Dispose();
            }
        }
        
        /// <summary>
        /// Renders the exception 
        /// </summary>
        /// <param name="p"></param>
        private void RenderException(string exception)
        {
            if (lblErrorMessage != null)
            {
                lblErrorMessage.Text = exception;
                lblErrorMessage.Visible = true;
            }
        }

        /// <summary>
        /// Converts and Returns the Culture Formatted Date Time object of the date in string 
        /// added by Praveena for module Add "last updated" date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private string GetDateTime(string date)
        {
            string strDateFormat = string.Empty;
            strDateFormat = PortalConfiguration.GetInstance().GetKey(DATEFORMAT).ToString();
            DateTime dtmValue = DateTime.Parse(date);
            return dtmValue.ToString(strDateFormat);
        }

        #endregion Private Methods
    }
}
#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
//Filename: UserUploaded.ascx.cs
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
using System.Xml;
using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DWB.BusinessLogicLayer;
using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// UserUpload class for enabling the user to view/upload  Type III documents
    /// </summary>
    public partial class UserUploaded : UIHelper
    {

        #region Declarations
        string strPageID = string.Empty;
        string strChapterID = string.Empty;
        string strMode = string.Empty;
        CommonBLL objCommon;
        TreeNodeSelection objTreeNode;
        const string DATEFORMAT = "Date Format";

        /// <summary>
        /// Gets or sets the Page ID.
        /// </summary>
        /// <value>The page ID.</value>
        string PageID
        {
            get { return strPageID; }
            set { strPageID = value; }
        }

        /// <summary>
        /// Gets or sets the chapter ID.
        /// </summary>
        /// <value>The chapter ID.</value>
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
                        ((TreeNodeSelection)HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES]).IsTypeIIISelected)
                {
                    AdvancedSearchContent.Visible = true;
                    objTreeNode = (TreeNodeSelection)HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES];
                    PageID = objTreeNode.PageID;
                    ChapterID = objTreeNode.ChapterID;
                    hidtype3uploaded.Value = string.Empty;
                    LoadMetaData();
                    btnUpdate.Attributes.Add(ONCLICKEVENTNAME, "javascript:UpdatePageContents('" + PageID + "','3');");
                }
                else if (HttpContext.Current.Request.QueryString[MODEQUERYSTRING] != null)
                {
                    AdvancedSearchContent.Visible = true;
                    PageID = HttpContext.Current.Request.QueryString[PAGEIDQUERYSTRING];
                    LoadMetaData();
                    btnUpdate.Attributes.Add("onclick", "javascript:UpdatePageContents('" + PageID + "','3');");
                }
                base.Render(writer);
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(strParentSiteURL, webEx);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strParentSiteURL, ex);
            }
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
                    ((TreeNodeSelection)HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES]).IsTypeIIISelected)
                {
                    objTreeNode = (TreeNodeSelection)HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES];
                    PageID = objTreeNode.PageID;
                    ChapterID = objTreeNode.ChapterID;
                }
                else if (HttpContext.Current.Request.QueryString[MODEQUERYSTRING] != null)
                {
                    PageID = HttpContext.Current.Request.QueryString[PAGEIDQUERYSTRING];
                }
                LoadMetaData();
                if (string.Equals(btnSignOff.Value, CANCELSIGNOFF))
                {
                    objCommon.SignOffPage(strParentSiteURL, CHAPTERPAGESMAPPINGLIST, hidPageId.Value, CHAPTERPAGESMAPPINGAUDITLIST, false);
                }
                else
                {
                    objCommon.SignOffPage(strParentSiteURL, CHAPTERPAGESMAPPINGLIST, hidPageId.Value, CHAPTERPAGESMAPPINGAUDITLIST, true);
                }

            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(strParentSiteURL, webEx, 1);
                RenderException(webEx.Message);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strParentSiteURL, ex);
            }
        }

        /// <summary>
        /// Handles the ServerClick event of the btnPrintPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnPrintPage_ServerClick(object sender, EventArgs e)
        {
            try
            {
                int intPrintedDocID = 0;
                //string[] strPrintedDocURL = null;
                CommonBLL objCommonBLL = null;
                string strCamlQuery = string.Empty;
                PrintOptions objPrintOptions = new PrintOptions();
                if (string.Compare(hdnIncludeStoryBoard.Value.ToLowerInvariant(), "true") == 0)
                {
                    objPrintOptions.IncludeStoryBoard = true;
                }
                if (string.Compare(hdnIncludePageTitle.Value.ToLowerInvariant(), "true") == 0)
                {
                    objPrintOptions.IncludePageTitle = true;
                }

                intPrintedDocID = Print(objPrintOptions, WELLBOOKVIEWERCONTROLPAGE, HttpContext.Current.Request.QueryString[MODEQUERYSTRING]);

                /// Open the generated document to show in IE                
                if (intPrintedDocID > 0)
                {
                    objCommonBLL = new CommonBLL();
                    strCamlQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" +
                   intPrintedDocID.ToString() + "</Value></Eq></Where>";
                    //strPrintedDocURL = objCommonBLL.GetPrintedDocumentUrl(strParentSiteURL, PRINTEDDOCUMNETLIBRARY, strCamlQuery);
                    //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "documentprinted", "openDWBPrintDoc('" + strPrintedDocURL[0] + "');", true);
                    ///Modified by Manoj for Print
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "documentprinted", "window.open('" + @strDocumentURL + "', 'PDFViewer', 'scrollbars,resizable,status,toolbar,menubar');", true);
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "documentnotprinted", ALERTDOCUMENTNOTPRINTED, true);
                }
            }
            catch (System.Web.Services.Protocols.SoapException soapEx)
            {
                CommonUtility.HandleException(strParentSiteURL, soapEx);//, 1);
                RenderException(soapEx.StackTrace);
                PdfBLL objPdfBLL = new PdfBLL();
                objPdfBLL.PrintLog(strParentSiteURL, soapEx.StackTrace, soapEx.Message, "Type3.btnPrint_Click");
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(strParentSiteURL, webEx);//, 1);
                RenderException(webEx.StackTrace);
                PdfBLL objPdfBLL = new PdfBLL();
                objPdfBLL.PrintLog(strParentSiteURL, webEx.StackTrace, webEx.Message, "Type3.btnPrint_Click");
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strParentSiteURL, ex);
                RenderException(ex.StackTrace);
                PdfBLL objPdfBLL = new PdfBLL();
                objPdfBLL.PrintLog(strParentSiteURL, ex.StackTrace, ex.Message, "Type3.btnPrint_Click");
            }
        }

        /// <summary>
        /// Handles the Click event of the btnFirePOstBack control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnFirePOstBack_Click(object sender, EventArgs e)
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
            string strViewFields = string.Empty;
            string strBookOwner = string.Empty;
            string strBookTeamID = string.Empty;
            string strPageOwner = string.Empty;
            string strPageDiscipline = string.Empty;
            DataTable dtBookPage = null;
            DataTable dtUserDefinedDocDetails=null;
            btnSignOff.Visible = false;
            btnUpdate.Visible = false;
            bool blnShowUpdate = false;
            lblModifiedDate.Visible = false;
            if (HttpContext.Current.Request.QueryString[MODEQUERYSTRING] != null)
                strMode = HttpContext.Current.Request.QueryString[MODEQUERYSTRING];
            hidPageId.Value = PageID;

            if (HttpContext.Current.Session[SESSIONTYPE3UPLOADED] != null)
                hidtype3uploaded.Value = HttpContext.Current.Session[SESSIONTYPE3UPLOADED].ToString().ToLowerInvariant();

            objCommon = new CommonBLL();
            strCamlQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Number'>" +
                PageID + "</Value></Eq></Where>";
            dtBookPage = objCommon.ReadList(strParentSiteURL, CHAPTERPAGESMAPPINGLIST, strCamlQuery);
            if (dtBookPage.Rows.Count > 0)
            {
                lblOwner.Text = dtBookPage.Rows[0]["Owner"].ToString();
                lblSignedOff.Text = dtBookPage.Rows[0]["Sign_Off_Status"].ToString();
                lblTitleTemplate.Text = dtBookPage.Rows[0]["Page_Actual_Name"].ToString();

                /// CHAPTERIDQUERYSTRING is not applicable. Get ChapterID from session object.
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
                strPageDiscipline = dtBookPage.Rows[0]["Discipline"].ToString();
                strPageOwner = dtBookPage.Rows[0]["Owner"].ToString();
            }
            if (string.Compare(lblSignedOff.Text.ToLowerInvariant(),STATUSSIGNEDOFF) == 0)
            {
                blnShowUpdate = false;
                btnSignOff.Value = CANCELSIGNOFF;
            }
            else
            {
                blnShowUpdate = true;
                btnSignOff.Value = SIGNOFF;
            }
            strCamlQuery = @"<Where><Eq><FieldRef Name='PageID' /><Value Type='Number'>" +
              PageID + "</Value></Eq></Where>";
            string strObjectInnerHtml = objCommon.GetUploadedDocumentUrl(strParentSiteURL, USERDEFINEDDOCUMENTLIST, strCamlQuery);
            /// Loads the embedded image/document/application 
            docviewerdiv.InnerHtml = strObjectInnerHtml;

            //Added by Praveena for module "Add Last Updated date"
            strViewFields = @"<FieldRef Name='PageID' /><FieldRef Name='Modified' />";
            dtUserDefinedDocDetails = objCommon.ReadList(strParentSiteURL, USERDEFINEDDOCUMENTLIST, strCamlQuery,strViewFields);
            if (dtUserDefinedDocDetails != null && dtUserDefinedDocDetails.Rows.Count > 0)
            {
                string dtModifiedDate = GetDateTime(dtUserDefinedDocDetails.Rows[0]["Modified"].ToString());
                lblModifiedDate.Visible = true;
                lblModifiedDate.Text = "Last Updated Date:" + dtModifiedDate;
            }

            if (HttpContext.Current.Session[SESSIONTREEVIEWDATAOBJECT] != null)
            {
                BookInfo objBookInfo = ((BookInfo)HttpContext.Current.Session[SESSIONTREEVIEWDATAOBJECT]);
                if (objBookInfo != null)
                {
                    strBookOwner = objBookInfo.BookOwner;
                    strBookTeamID = objBookInfo.BookTeamID;
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
                }
            }

            if (!string.IsNullOrEmpty(strMode) && strMode.Equals(VIEW))
            {
                btnUpdate.Visible = false;
                btnSignOff.Visible = false;
            }
            if (dtBookPage != null)
            {
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
        /// Added by Praveena for module "Add Last Updated date"
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
#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: ChapterMaintenance.cs
#endregion

using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DREAM.Utilities;
using System.Collections.Generic;
using Shell.SharePoint.DWB.BusinessLogicLayer;
using System.Net;

namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// Displays the Chapter details on Book Viewer screen.
    /// </summary>
    public partial class ChapterMaintenance : UIHelper
    {
        #region DECLARATION
    
        const string CHANGEMASTERPAGE = "change";
        const string PAGETITLEEXISTSMSG = "Chapter Title already exist. Please enter a different title.";
        const string MAINTAINWELLBOOKURL = "BookMaintenance.aspx";          

        int intBookID;
        ListEntry objListEntry;
        string strSelectedID = string.Empty;
        TreeNodeSelection objTreeNodeSelection;

        #endregion
      
        /// <summary>
        /// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"></see> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the server control content.</param>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            btnPrint.OnClientClick = base.RegisterJavaScript();
            if (HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES] != null &&
                    ((TreeNodeSelection)HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES]).IsChapterSelected)
            {                
                objTreeNodeSelection = (TreeNodeSelection)HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES];
                strSelectedID = objTreeNodeSelection.ChapterID; 
                int.TryParse(objTreeNodeSelection.BookID, out intBookID);
                try
                {
                    objListEntry = GetDetailsForSelectedID(strSelectedID, DWBCHAPTERLIST, CHAPTER);
                    if (objListEntry != null)
                    {
                        BindUIControls();
                    }                    
                }
                catch (WebException webEx)
                {
                    CommonUtility.HandleException(strParentSiteURL, webEx);
                    lblException.Text = webEx.Message;
                    lblException.Visible = true;

                }
                catch (Exception ex)
                {

                    CommonUtility.HandleException(strParentSiteURL, ex);

                }
                base.Render(writer);
            }
        }


        /// <summary>
        /// Handles the Click event of the cmdPrintChapter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmdPrintChapter_Click(object sender, EventArgs e)
        {
            string strAlertMessage = string.Empty;
            int intPrintedDocID = 0;
            string[] strPrintedDocURL = null;
            CommonBLL objCommonBLL = null;
            string strCamlQuery = string.Empty;
            PrintOptions objPrintOptions = new PrintOptions();

            try
            {
                /// If Hidden field containse "true" story board should be included in Print document
                if (string.Compare(hdnIncludeStoryBoard.Value.ToLowerInvariant(), "true", true) == 0)
                {
                    objPrintOptions.IncludeStoryBoard = true;
                }
                else if (string.Compare(hdnIncludeStoryBoard.Value.ToLowerInvariant(), "false", true) == 0)/// If Hidden field containse "false" story board shouldn't be included in Print document
                {
                    objPrintOptions.IncludeStoryBoard = false;
                }
                else  /// If Hidden field otherthan contains "true/false" story board shouldn't be included in Print document
                {
                    objPrintOptions.IncludeStoryBoard = false;
                }
                /// If Hidden field containse "true" Page Title should be included in Print document
                if (string.Compare(hdnIncludePageTitle.Value.ToLowerInvariant(), "true", true) == 0)
                {
                    objPrintOptions.IncludePageTitle = true;
                }
                else if (string.Compare(hdnIncludePageTitle.Value.ToLowerInvariant(), "false", true) == 0)/// If Hidden field containse "false" Page Title shouldn't be included in Print document
                {
                    objPrintOptions.IncludePageTitle = false;
                }
                else  /// If Hidden field otherthan contains "true/false" Page Title shouldn't be included in Print document
                {
                    objPrintOptions.IncludePageTitle = false;
                }

                //Added few more filter options as per DWBv2.0 requirement
                //Modified by Gopinath
                //Date:11/11/2010

                //PrintMyPages
                if (!string.IsNullOrEmpty(hdnPrintMyPages.Value.Trim()))
                    objPrintOptions.PrintMyPages = Convert.ToBoolean(hdnPrintMyPages.Value.Trim().ToString());


                //Include Filter
                if (!string.IsNullOrEmpty(hdnIncludeFilter.Value.Trim()))
                    objPrintOptions.IncludeFilter = Convert.ToBoolean(hdnIncludeFilter.Value.Trim().ToString());

                //SignedOff
                if (!string.IsNullOrEmpty(hdnSignedOffPages.Value.Trim()))
                    objPrintOptions.SignedOff = hdnSignedOffPages.Value.Trim().ToString();

                //EmptyPages
                if (!string.IsNullOrEmpty(hdnEmptyPages.Value.Trim()))
                    objPrintOptions.EmptyPages = hdnEmptyPages.Value.Trim().ToString();

                //PageType
                if (!string.IsNullOrEmpty(hdnPageType.Value.Trim()))
                    objPrintOptions.PageType = hdnPageType.Value.Trim().ToString();

                //PageName
                if (!string.IsNullOrEmpty(hdnPageName.Value.Trim()))
                    objPrintOptions.PageName = hdnPageName.Value.Trim().ToString();

                //Discipline
                if (!string.IsNullOrEmpty(hdnDiscipline.Value.Trim()))
                    objPrintOptions.Discipline = hdnDiscipline.Value.Trim().ToString();
                
                intPrintedDocID = Print(objPrintOptions, WELLBOOKVIEWERCONTROLCHAPTER, HttpContext.Current.Request.QueryString[MODEQUERYSTRING]);
             
                /// Open the generated document to show in IE                
                if (intPrintedDocID > 0)
                {
                    objCommonBLL = new CommonBLL();
                    strCamlQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" +
                   intPrintedDocID.ToString() + "</Value></Eq></Where>";
                    strPrintedDocURL = objCommonBLL.GetPrintedDocumentUrl(strParentSiteURL, PRINTEDDOCUMNETLIBRARY, strCamlQuery);
                    strAlertMessage = objCommonBLL.GetAlertMessage(strParentSiteURL, "Chapter");
                    if (strPrintedDocURL != null && strPrintedDocURL.Length > 0)
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "documentprinted", "alert('"+ strAlertMessage +"');", true);
                    }
                }
                else if (intPrintedDocID == -1)
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), HIDETABJSKEY, HIDETABJSPAGE);
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "NoDocumentsFound", @"<Script language='javaScript'>alert('Please change the filter criteria');</Script>");
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), HIDETABJSKEY, HIDETABJSPAGE);
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "documentnotprinted", "alert(' "+ ALERTDOCUMENTNOTPRINTED + " ')", true);
                }
            }            
            catch (System.Web.Services.Protocols.SoapException soapEx)
            {
                CommonUtility.HandleException(strParentSiteURL, soapEx,1);
                lblException.Text = ALERTDOCUMENTNOTPRINTED;
                lblException.Visible = true;
                PdfBLL objPdfBLL = new PdfBLL();
                objPdfBLL.PrintLog(strParentSiteURL, soapEx.StackTrace, soapEx.Message, "Chapter.btnPrint_Click");

            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(strParentSiteURL, webEx,1);
                lblException.Text = ALERTDOCUMENTNOTPRINTED;
                lblException.Visible = true;
                PdfBLL objPdfBLL = new PdfBLL();
                objPdfBLL.PrintLog(strParentSiteURL, webEx.StackTrace, webEx.Message, "Chapter.btnPrint_Click");

            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strParentSiteURL, ex,1);
                lblException.Text = ALERTDOCUMENTNOTPRINTED;
                lblException.Visible = true;
                PdfBLL objPdfBLL = new PdfBLL();
                objPdfBLL.PrintLog(strParentSiteURL, ex.StackTrace, ex.Message, "Chapter.btnPrint_Click");

            }
            finally
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), HIDETABJSKEY, HIDETABJSPAGE);
            }
        }


        /// <summary>
        /// Binds the UI controls.
        /// </summary>
        /// <param name="objMasterData">The obj well book data.</param>
        private void BindUIControls()
        {

            txtDescription.Text = objListEntry.ChapterDetails.ChapterDescription;

            //To:Do for the edit mode asset Value is set to default need to call webservice
            txtAssetValue.Text = objListEntry.ChapterDetails.AssetValue;

            txtAssetType.Text = objListEntry.ChapterDetails.AssetType;

            ListEntry objTemplate = GetDetailsForSelectedID(Convert.ToString(objListEntry.ChapterDetails.TemplateID), TEMPLATELIST, TEMPLATE);

            if (objTemplate != null && objTemplate.TemplateDetails != null)
            {
                txtTemplateTitle.Text = objTemplate.TemplateDetails.Title;
            }

            hdnListitemStatus.Value = Convert.ToString(objListEntry.ChapterDetails.BookID);
        }
    }
}
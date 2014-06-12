#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: WellBookSummary.cs
#endregion

using System;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DWB.BusinessLogicLayer;
using Shell.SharePoint.DREAM.Utilities;


namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// This user control displays the summary of the well book pages.
    /// The pages are displayed as per the page owner.
    /// </summary>
    public partial class WellBookSummary : UIHelper
    {
        #region DECLARATION

        string strWellBookId = string.Empty;
        ListEntry objListEntry;
        WellBookBLL objWellBookBLL;
        HiddenField hdnSelectedStatus;
        bool blnShowWellSummaryTable;
        TreeNodeSelection objTreeNodeSelection;

        #region DREAM 4.0-eWB2.0-Customise Chapters
        string strWellBookName = string.Empty;
        #endregion
        const string BORDERCOLOR = "bordercolor";
        const string BORDERCOLORVALUE = "#9b9797";//modified for eWB2 rebranding
        const string FIRST = "First";
        const string LAST = "Last";
        const string OVERFLOW = "overflow:auto;";
        const string EVENROWSTYLECSS = "evenRowStyle";
        const string ODDROWSTYLECSS = "oddRowStyle";
        const string WELLBOOKSUMMARYCSS = "DWBWellBookSummaryGridViewCSS";
        const string FIXEDHEADERCSS = "ResultFixedHeader";
        #endregion

        #region Protected Methods
        /// <summary>
        /// page load event triggered by asp.net engine. Used to read query string 
        /// and populate the UI controls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region DREAM 4.0-eWB2.0-Customise Chapters

            if (radLstChapters != null && chkSelectDeselectAll != null)
            {
                btnCustomiseChapters.OnClientClick = "return OpeneWBChpaterReorderPopUp('diveWBCustomiseChapters','"+ radLstChapters.ClientID+"','"+ chkSelectDeselectAll.ClientID +"');";
                chkSelectDeselectAll.Attributes.Add("onclick", "return SelectDeselectAll(this,'" + radLstChapters.ClientID + "');");
                btnApply.OnClientClick = "return ReOrderItemsInSession(this,'" + radLstChapters.ClientID + "',true);";
                btnApplyAndSave.OnClientClick = "return ReOrderItemsInSession(this,'" + radLstChapters.ClientID + "',false);";
            }
            #endregion
            hdnSelectedStatus = new HiddenField();
            hdnSelectedStatus.ID = "hdnSelectedStatus";
            this.Controls.Add(hdnSelectedStatus);


            #region DREAM 4.0 - eWB 2.0 - Customise Chapters
            /// Populating the RadListBox in OnLoad.
            if (!Page.IsPostBack)
            {
                strWellBookId = HttpContext.Current.Request.QueryString["BookID"];
                string strCamlQuery = @"<Where><Eq><FieldRef Name='Book_ID'/><Value Type='Counter'>" + strWellBookId + "</Value></Eq></Where>";
                /// Get the Book Details for the selected Book Id and populate the controls with data
                objListEntry = GetDetailsForSelectedID(strWellBookId, DWBBOOKLIST, WELLBOOK);
                if (objListEntry != null && objListEntry.WellBookDetails != null)
                {
                    strWellBookName = objListEntry.WellBookDetails.Title;
                    if (ShowButton(CUSTOMISECHAPTERS, objListEntry.WellBookDetails.BookOwner, objListEntry.WellBookDetails.TeamID, string.Empty, string.Empty))
                    {
                        btnCustomiseChapters.Visible = true;
                        radLstChapters.Items.Clear();
                        LoadChapterRadListBox(strWellBookId, radLstChapters);
                    }
                }
            }
            /// EventTarget value is set and __doPostBack is called in case of Apply and Apply & Save click of Customise Chapter.
            /// __doPostBack is used instead of server click event since it should be notified to TreeViewControl.cs as well for repopulating
            /// the treeview in OnPreRender.
            if (this.Page.Request.Params[EVENTTARGET] != null)
            {
                strWellBookId = HttpContext.Current.Request.QueryString["BookID"];
                string strCamlQuery = @"<Where><Eq><FieldRef Name='Book_ID'/><Value Type='Counter'>" + strWellBookId + "</Value></Eq></Where>";
                /// Get the Book Details for the selected Book Id and populate the controls with data
                objListEntry = GetDetailsForSelectedID(strWellBookId, DWBBOOKLIST, WELLBOOK);
                if (objListEntry != null && objListEntry.WellBookDetails != null)
                {
                    strWellBookName = objListEntry.WellBookDetails.Title;
                }

                /// If Apply button is clicked.
                if (this.Page.Request.Params[EVENTTARGET].ToLowerInvariant().Equals("customisechaptersinsession"))
                {

                    SaveCustomiseChapterPreferenceToSession(strWellBookId, strWellBookName, radLstChapters);

                }
                else if (this.Page.Request.Params[EVENTTARGET].ToLowerInvariant().Equals("customisechaptersforfuture"))
                { /// If Apply and Save button is clicked the changes are saved to document library as well as to Session                   
                    SaveCustomiseChapterPreferenceToSession(strWellBookId, strWellBookName, radLstChapters);
                    SaveReorderXml((string)HttpContext.Current.Session[CHAPTERPREFERENCEXML], strWellBookId);
                }
            }
            #endregion
        }

        #region DREAM 4.0 - eWB 2.0 - Customise Chapters
        /// Overriding Render is removed since RadListBox creates error in case of Render overriding.
        /// Functionlities achieved in Render method in earlier releases are moved to OnPreRender

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            btnPrint.OnClientClick = base.RegisterJavaScript();
            if (HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES] != null &&
                ((TreeNodeSelection)HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES]).IsBookSelected)
            {
                pnlTemplate.Visible = true;
                objTreeNodeSelection = (TreeNodeSelection)HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES];
                strWellBookId = objTreeNodeSelection.BookID;
                btnBatchImport.OnClientClick = "return openBatchImport('" + strWellBookId + "');";
                LoadWellBookData();
            }
            else
            {
                pnlTemplate.Visible = false;
            }
        }

        /////// <summary>
        /////// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"></see> object, which writes the content to be rendered on the client.
        /////// </summary>
        /////// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the server control content.</param>
        //protected override void Render(System.Web.UI.HtmlTextWriter writer)
        //{
        //    try
        //    {
        //        btnPrint.OnClientClick = base.RegisterJavaScript();
        //        if (HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES] != null &&
        //            ((TreeNodeSelection)HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES]).IsBookSelected)
        //        {
        //            pnlTemplate.Visible = true;
        //            objTreeNodeSelection = (TreeNodeSelection)HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES];
        //            strWellBookId = objTreeNodeSelection.BookID;
        //            btnBatchImport.OnClientClick = "return openBatchImport('" + strWellBookId + "');";
        //            LoadWellBookData();
        //        }

        //        System.IO.StringWriter stringWriter = new System.IO.StringWriter();
        //        HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
        //        base.Render(htmlWriter);
        //        string strHtml = stringWriter.ToString();
        //        string[] aspnet_formelems = new string[5];
        //        aspnet_formelems[0] = "__EVENTTARGET";
        //        aspnet_formelems[1] = "__EVENTARGUMENT";
        //        aspnet_formelems[2] = "__VIEWSTATE";
        //        aspnet_formelems[3] = "__EVENTVALIDATION";
        //        aspnet_formelems[4] = "__VIEWSTATEENCRYPTED";
        //        foreach (string strElement in aspnet_formelems)
        //        {

        //            int intStartPoint = strHtml.IndexOf("<input type=\"hidden\" name=\"" +
        //              strElement.ToString() + "\"");
        //            if (intStartPoint >= 0)
        //            {
        //                int intEndPoint = strHtml.IndexOf("/>", intStartPoint) + 2;
        //                string strViewStateInput = strHtml.Substring(intStartPoint, intEndPoint - intStartPoint);
        //                strHtml = strHtml.Remove(intStartPoint, intEndPoint - intStartPoint);
        //                int intFormStart = strHtml.IndexOf("<form");
        //                int intEndForm = strHtml.IndexOf(">", intFormStart) + 1;
        //                if (intEndForm >= 0)
        //                    strHtml = strHtml.Insert(intEndForm, strViewStateInput);
        //            }
        //        }
        //        writer.Write(strHtml);
        //    }
        //    catch (WebException webEx)
        //    {

        //        lblException.Text = webEx.Message;
        //        lblException.Visible = true;
        //        ExceptionBlock.Visible = true;
        //    }
        //    catch (Exception ex)
        //    {

        //        CommonUtility.HandleException(strParentSiteURL, ex);

        //    }
        //}
        #endregion

        /// <summary>
        /// Grid View Row data bound.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdWellBookSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            LinkButton linkbtnOpenSummary = null;
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[0].Text = PAGEOWNERCOLUMN;
                    e.Row.Cells[1].Text = TOTALCOLUMN;
                    e.Row.Cells[2].Text = SIGNEDOFFCOLUMN;
                    e.Row.Cells[3].Text = UNSIGNEDOFFCOLUMN;
                    e.Row.Cells[4].Text = EMPTYCOLUMN;
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.Cells[0].Text.Equals(TOTALCOLUMN))
                    {
                        linkbtnOpenSummary = (LinkButton)e.Row.Cells[1].Controls[1];
                        linkbtnOpenSummary.OnClientClick = "javascript:OpenWellBookSummaryDescription('All','Total','" + strWellBookId + "');";

                        linkbtnOpenSummary = (LinkButton)e.Row.Cells[2].Controls[1];
                        linkbtnOpenSummary.OnClientClick = "javascript:OpenWellBookSummaryDescription('All','SignedOff','" + strWellBookId + "');";
                        linkbtnOpenSummary = (LinkButton)e.Row.Cells[3].Controls[1];
                        linkbtnOpenSummary.OnClientClick = "javascript:OpenWellBookSummaryDescription('All','NotSignedOff','" + strWellBookId + "');";
                        linkbtnOpenSummary = (LinkButton)e.Row.Cells[4].Controls[1];
                        linkbtnOpenSummary.OnClientClick = "javascript:OpenWellBookSummaryDescription('All','Empty','" + strWellBookId + "');";
                    }
                    else
                    {
                        linkbtnOpenSummary = (LinkButton)e.Row.Cells[1].Controls[1];
                        linkbtnOpenSummary.OnClientClick = "javascript:OpenWellBookSummaryDescription('" + e.Row.Cells[0].Text + "','Total','" + strWellBookId + "');";

                        linkbtnOpenSummary = (LinkButton)e.Row.Cells[2].Controls[1];
                        linkbtnOpenSummary.OnClientClick = "javascript:OpenWellBookSummaryDescription('" + e.Row.Cells[0].Text + "','SignedOff','" + strWellBookId + "');";
                        linkbtnOpenSummary = (LinkButton)e.Row.Cells[3].Controls[1];
                        linkbtnOpenSummary.OnClientClick = "javascript:OpenWellBookSummaryDescription('" + e.Row.Cells[0].Text + "','NotSignedOff','" + strWellBookId + "');";
                        linkbtnOpenSummary = (LinkButton)e.Row.Cells[4].Controls[1];
                        linkbtnOpenSummary.OnClientClick = "javascript:OpenWellBookSummaryDescription('" + e.Row.Cells[0].Text + "','Empty','" + strWellBookId + "');";
                    }


                }
            }
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

        }

        /// <summary>
        /// Signs Off book or Cancel Signs off the book.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSignOff_Click(object sender, EventArgs e)
        {
            objWellBookBLL = new WellBookBLL();
            try
            {
                if (HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES] != null &&
                   ((TreeNodeSelection)HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES]).IsBookSelected)
                {
                    objTreeNodeSelection = (TreeNodeSelection)HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES];
                    strWellBookId = objTreeNodeSelection.BookID;
                }
                string strCamlQuery = @"<Where><Eq><FieldRef Name='Book_ID'/><Value Type='Counter'>" + strWellBookId + "</Value></Eq></Where>";
                /// Get the Book Details for the selected Book Id and populate the controls with data
                objListEntry = GetDetailsForSelectedID(strWellBookId, DWBBOOKLIST, WELLBOOK);
                if (objListEntry != null && objListEntry.WellBookDetails != null)
                {
                    #region DREAM 4.0 - eWB 2.0 - Customise Chapters
                    strWellBookName = objListEntry.WellBookDetails.Title;
                    #endregion
                    if (objListEntry.WellBookDetails.SignOffStatus.ToLowerInvariant().CompareTo(STATUSSIGNEDOFF) == 0)
                    {
                        objWellBookBLL.ChangeSignOffStatus(strParentSiteURL, STATUSACTIVE, DWBBOOKLIST, GetUserName(), strWellBookId, AUDITACTIONUNSIGNEDOFF);
                    }
                    else
                    {
                        objWellBookBLL.ChangeSignOffStatus(strParentSiteURL, STATUSTERMINATED, DWBBOOKLIST, GetUserName(), strWellBookId, AUDITACTIONSIGNEDOFF);
                    }
                }
            }
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
            finally
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), HIDETABJSKEY, HIDETABJSPAGE);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnPrint control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            string strAlertMessage = string.Empty;
            try
            {
                int intPrintedDocID = 0;
                string[] strPrintedDocURL = null;
                CommonBLL objCommonBLL = null;
                string strCamlQuery = string.Empty;
                PrintOptions objPrintOptions = new PrintOptions();

                //Set PrintOptions object properties through hidden fields
                objPrintOptions = SetPrintOptionsProperties(objPrintOptions);

                intPrintedDocID = Print(objPrintOptions, WELLBOOKVIEWERCONTROLBOOK, HttpContext.Current.Request.QueryString[MODEQUERYSTRING]);

                /// Open the generated document to show in IE     
                if (intPrintedDocID > 0)
                {
                    objCommonBLL = new CommonBLL();
                    strCamlQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" +
                   intPrintedDocID.ToString() + "</Value></Eq></Where>";
                    strPrintedDocURL = objCommonBLL.GetPrintedDocumentUrl(strParentSiteURL, PRINTEDDOCUMNETLIBRARY, strCamlQuery);
                    strAlertMessage = objCommonBLL.GetAlertMessage(strParentSiteURL, "Book");
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "documentprinted", "alert('" + strAlertMessage + "');", true);
                }
                else if (intPrintedDocID == -1)
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), HIDETABJSKEY, HIDETABJSPAGE);
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "NoDocumentsFound", @"<Script language='javaScript'>alert('Please change the filter criteria');</Script>");
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), HIDETABJSKEY, HIDETABJSPAGE);
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "documentnotprinted", ALERTDOCUMENTNOTPRINTED, true);
                }
            }
            catch (System.Web.Services.Protocols.SoapException soapEx)
            {
                CommonUtility.HandleException(strParentSiteURL, soapEx, 1);
                lblException.Text = ALERTDOCUMENTNOTPRINTED;
                lblException.Visible = true;
                ExceptionBlock.Visible = true;
                PdfBLL objPdfBLL = new PdfBLL();
                objPdfBLL.PrintLog(strParentSiteURL, soapEx.StackTrace, soapEx.Message, "WellBookSummary.btnPrint_Click");

            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(strParentSiteURL, webEx, 1);
                lblException.Text = ALERTDOCUMENTNOTPRINTED;
                lblException.Visible = true;
                ExceptionBlock.Visible = true;
                PdfBLL objPdfBLL = new PdfBLL();
                objPdfBLL.PrintLog(strParentSiteURL, webEx.StackTrace, webEx.Message, "WellBookSummary.btnPrint_Click");

            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strParentSiteURL, ex, 1);
                lblException.Text = ALERTDOCUMENTNOTPRINTED;
                lblException.Visible = true;
                ExceptionBlock.Visible = true;
                PdfBLL objPdfBLL = new PdfBLL();
                objPdfBLL.PrintLog(strParentSiteURL, ex.StackTrace, ex.Message, "WellBookSummary.btnPrint_Click");

            }
            finally
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), HIDETABJSKEY, HIDETABJSPAGE);
            }
        }



        #endregion Protected Methods

        #region Private Methods

        private PrintOptions SetPrintOptionsProperties(PrintOptions printOptions)
        {
            #region Fetch Hidden field values
            /// If Hidden field containse "true" story board should be included in Print document
            if (string.Compare(hdnIncludeStoryBoard.Value.ToLowerInvariant(), "true") == 0)
            {
                printOptions.IncludeStoryBoard = true;
            }
            else if (string.Compare(hdnIncludeStoryBoard.Value.ToLowerInvariant(), "false") == 0)/// If Hidden field containse "false" story board shouldn't be included in Print document
            {
                printOptions.IncludeStoryBoard = false;
            }
            else  /// If Hidden field otherthan contains "true/false" story board shouldn't be included in Print document
            {
                printOptions.IncludeStoryBoard = false;
            }
            /// If Hidden field containse "true" Page Title should be included in Print document
            if (string.Compare(hdnIncludePageTitle.Value.ToLowerInvariant(), "true") == 0)
            {
                printOptions.IncludePageTitle = true;
            }
            else if (string.Compare(hdnIncludePageTitle.Value.ToLowerInvariant(), "false") == 0)/// If Hidden field containse "false" Page Title shouldn't be included in Print document
            {
                printOptions.IncludePageTitle = false;
            }
            else  /// If Hidden field otherthan contains "true/false" Page Title shouldn't be included in Print document
            {
                printOptions.IncludePageTitle = false;
            }

            //Added few more filter options as per DWBv2.0 requirement
            //Modified by Gopinath
            //Date:11/11/2010

            //PrintMyPages
            if (!string.IsNullOrEmpty(hdnPrintMyPages.Value.Trim()))
                printOptions.PrintMyPages = Convert.ToBoolean(hdnPrintMyPages.Value.Trim().ToString());


            //Include Filter
            if (!string.IsNullOrEmpty(hdnIncludeFilter.Value.Trim()))
                printOptions.IncludeFilter = Convert.ToBoolean(hdnIncludeFilter.Value.Trim().ToString());

            //SignedOff
            if (!string.IsNullOrEmpty(hdnSignedOffPages.Value.Trim()))
                printOptions.SignedOff = hdnSignedOffPages.Value.Trim().ToString();

            //EmptyPages
            if (!string.IsNullOrEmpty(hdnEmptyPages.Value.Trim()))
                printOptions.EmptyPages = hdnEmptyPages.Value.Trim().ToString();

            //PageType
            if (!string.IsNullOrEmpty(hdnPageType.Value.Trim()))
                printOptions.PageType = hdnPageType.Value.Trim().ToString();

            //PageName
            if (!string.IsNullOrEmpty(hdnPageName.Value.Trim()))
                printOptions.PageName = hdnPageName.Value.Trim().ToString();

            //Discipline
            if (!string.IsNullOrEmpty(hdnDiscipline.Value.Trim()))
                printOptions.Discipline = hdnDiscipline.Value.Trim().ToString();

            #endregion Fetch Hidden field values

            printOptions.IncludeBookTitle = true;
            printOptions.IncludeTOC = true;

            return printOptions;
        }

        /// <summary>
        /// Loads the Well Book Pages data in the UI controls.
        /// Based on the book id passed in the query string.
        /// </summary>
        private void LoadWellBookData()
        {
            DataTable dtBookSummary = null;
            StringBuilder strChapterId = new StringBuilder();
            string strCamlQuery = @"<Where><Eq><FieldRef Name='Book_ID'/><Value Type='Counter'>" + strWellBookId + "</Value></Eq></Where>";
            /// Get the Book Details for the selected Book Id and populate the controls with data
            objListEntry = GetDetailsForSelectedID(strWellBookId, DWBBOOKLIST, WELLBOOK);
            txtOwner.Text = objListEntry.WellBookDetails.BookOwner;
            txtOwner.Enabled = false;
            txtTeam.Text = objListEntry.WellBookDetails.Team;
            txtTeam.Enabled = false;
            txtSignOffStatus.Text = objListEntry.WellBookDetails.SignOffStatus;
            txtSignOffStatus.Enabled = false;
            #region DREAM 4.0 - eWB 2.0 - Customise Chapters
            strWellBookName = objListEntry.WellBookDetails.Title;
            #endregion
            if (objListEntry.WellBookDetails.SignOffStatus.ToLowerInvariant().Equals(STATUSSIGNEDOFF))
            {
                hdnSelectedStatus.Value = STATUSTERMINATED;
                btnSignOff.Text = CANCELSIGNOFF;
            }
            else
            {
                hdnSelectedStatus.Value = STATUSACTIVE;
                btnSignOff.Text = SIGNOFF;
            }

            btnSignOff.Visible = false;
            btnBatchImport.Visible = false;
            blnShowWellSummaryTable = false;
            btnPrint.Visible = true;
            /// Show/Hide the SignOff,Pring Button and Book Summary
            if (ShowButton(WELLBOOKVIEWERCONTROLBOOK, objListEntry.WellBookDetails.BookOwner, objListEntry.WellBookDetails.TeamID, string.Empty, string.Empty))
            {
                blnShowWellSummaryTable = true;
                //Modefied By: Gopinath
                //Date : 04-11-2010
                //Below line of code commented due to the  V.2 requirement as Page Owner also can do the print.
                //btnPrint.Visible = true; 
                btnSignOff.Visible = true;
                //Added By Gopinath
                //Date : 17/11/2010
                btnBatchImport.Visible = true;
            }

            //Modefied By: Gopinath
            //Date : 04-11-2010
            //Below line of code commented due to the  V.2 requirement as Page Owner also can do the print
            //But not DWB User. If Book owner is normal user or DWB User then it returns true
            if (IsUserAsDWBUserAndPageOwner(strWellBookId, objListEntry.WellBookDetails.BookOwner))
            {
                btnPrint.Visible = false;
            }
            /// Show/Hide the Book Summary 
            if (blnShowWellSummaryTable)
            {
                /// Get the well book summary for the selected book and bid to the gridview
                dtBookSummary = GetWellBookSummary(strWellBookId);
                grdWellBookSummary.CssClass = WELLBOOKSUMMARYCSS;
                grdWellBookSummary.EnableViewState = true;
                grdWellBookSummary.HeaderStyle.CssClass = FIXEDHEADERCSS;
                grdWellBookSummary.HeaderStyle.Height = new Unit(20, UnitType.Pixel);
                grdWellBookSummary.RowStyle.CssClass = EVENROWSTYLECSS;
                grdWellBookSummary.AlternatingRowStyle.CssClass = ODDROWSTYLECSS;
                grdWellBookSummary.Attributes.Add(BORDERCOLOR, BORDERCOLORVALUE);//modified for eWB2 rebranding
                grdWellBookSummary.DataSource = dtBookSummary;
                grdWellBookSummary.DataBind();
            }
            else /// For users who are != BO/AD shows the summary of pages owned by the logged in user. [PageOwner summary CR implementation]
            {
                dtBookSummary = GetWellBookSummary(strWellBookId);
                if (dtBookSummary != null && dtBookSummary.Rows.Count > 0)
                {
                    DataView dtPageOwnerView = dtBookSummary.DefaultView;
                    /// Filter the summary table for the logged in user.
                    dtPageOwnerView.RowFilter = "Page_Owner ='" + GetUserName() + "'";

                    /// Convert the Dataview to DataTable
                    if (dtPageOwnerView != null && dtPageOwnerView.Count > 0)
                    {
                        dtBookSummary = null;
                        dtBookSummary = dtPageOwnerView.ToTable();
                    }
                    /// If the Summary table contains Rows > 0 bind to to datagrid.
                    if (dtBookSummary != null && dtBookSummary.Rows.Count > 0)
                    {
                        grdWellBookSummary.CssClass = WELLBOOKSUMMARYCSS;
                        grdWellBookSummary.EnableViewState = true;
                        grdWellBookSummary.HeaderStyle.CssClass = FIXEDHEADERCSS;
                        grdWellBookSummary.HeaderStyle.Height = new Unit(20, UnitType.Pixel);
                        grdWellBookSummary.RowStyle.CssClass = EVENROWSTYLECSS;
                        grdWellBookSummary.AlternatingRowStyle.CssClass = ODDROWSTYLECSS;
                        grdWellBookSummary.Attributes.Add(BORDERCOLOR, BORDERCOLORVALUE);//modified for eWB2 rebranding
                        grdWellBookSummary.DataSource = dtBookSummary;
                        grdWellBookSummary.DataBind();
                    }
                }

            }

            if (dtBookSummary != null)
                dtBookSummary.Dispose();
        }
        #endregion Private Methods

    }
}
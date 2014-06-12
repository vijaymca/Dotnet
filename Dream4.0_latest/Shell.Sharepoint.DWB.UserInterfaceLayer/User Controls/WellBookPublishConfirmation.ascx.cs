#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: WellBookPublishConfirmation.ascx.cs
#endregion

using System;
using System.Data;
using System.Web;
using System.Net;
using System.Web.UI.WebControls;
using System.Xml;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DWB.BusinessLogicLayer;

namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// Displays the information about Book before publishing.
    /// </summary>
    public partial class WellBookPublishConfirmation : UIHelper
    {
        #region Declarations
        const string QUERYSTRINGBOOKID = "BookID";
        const string BOOKPUBLISHEDMESSAGE = "The published book should now be treated as a business record and uploaded into your corporate repository.Please consult your GRM Fileplan and/or contact your publishing focal point.";
        const string NOACTIVECHAPTERORPAGEMESSAGE = "There are no active chapters/pages for the selected book. You can't publish the book now. Please contact Administrator.";
        const string BOOKNOTPUBLSIHEDMESSAGE = "eWB2 is not published.Please contact Administrator.";//for consistent usage of term eWB2 instead of Digital Well Book, DWB, eWell Book II.
        const string BOOKTITLEEXISTSMESSAGE = "Well book with this title already exists. Please give different name.";

        const string EVENROWSTYLECSS = "evenRowStyle";
        const string ODDROWSTYLECSS = "oddRowStyle";
        const string WELLBOOKSUMMARYCSS = "DWBWellBookSummaryGridViewCSS";
        const string FIXEDHEADERCSS = "ResultFixedHeader";
       

        string strBookID = string.Empty;
        ListEntry objListEntry ;

        #endregion

        #region Prrotected Methods
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            /// Based on the Book ID, retrieve the details and display
            /// Book ID can be sent in Query String
            try
            {
                strBookID = HttpContext.Current.Request.QueryString[QUERYSTRINGBOOKID];

                if (!Page.IsPostBack)
                {
                    DisplayBookDetails();
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
        /// Handles the RowDataBound event of the grdWellBookSummary control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void grdWellBookSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[0].Text = PAGEOWNERCOLUMN;
                    e.Row.Cells[1].Text = TOTALCOLUMN;
                    e.Row.Cells[2].Text =SIGNEDOFFCOLUMN;
                    e.Row.Cells[3].Text = UNSIGNEDOFFCOLUMN;
                    e.Row.Cells[4].Text = EMPTYCOLUMN;
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
        /// Handles the Click event of the cmdContinuePublish control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmdContinuePublish_Click(object sender, EventArgs e)
        {
            try
            {
                int intBookID = 0;
                int.TryParse(strBookID, out intBookID);
                Publish(intBookID);
            }
            catch (WebException webEx)
            {

                lblException.Text = webEx.Message;
                lblException.Visible = true;
                ExceptionBlock.Visible = true;
            }
            catch (System.Web.Services.Protocols.SoapException webEx)
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
        #endregion Prrotected Methods

        #region Private Methods
        /// <summary>
        /// Displays the book details.
        /// </summary>
        private void DisplayBookDetails()
        {
            string strCamlQuery = string.Empty;
            DataTable dtBookSummary = null;
            if (!string.IsNullOrEmpty(strBookID))
            {
                strCamlQuery = @"<Where><Eq><FieldRef Name='Book_ID'/><Value Type='Counter'>" + strBookID + "</Value></Eq></Where>";
                objListEntry = GetDetailsForSelectedID(strBookID, DWBBOOKLIST, WELLBOOK);
                if (objListEntry != null && objListEntry.WellBookDetails != null)
                {
                    lblBookName.Text = objListEntry.WellBookDetails.Title;
                    txtNewBookName.Text = objListEntry.WellBookDetails.Title;
                    txtNewBookName.Attributes.Add(ONFOCUSEVENTNAME, "ClearDefaultText(this,'" + objListEntry.WellBookDetails.Title + "');");
                    txtNewBookName.Attributes.Add(ONBLUREEEVNTNAME, "ResetToDefaultText(this,'" + objListEntry.WellBookDetails.Title + "');");

                    if (objListEntry.WellBookDetails.NoOfActiveChapters > 0)
                    {
                        dtBookSummary = GetWellBookSummary(strBookID);
                        ///   Last Row of dtBookSummary datatable column["Total"]contains the total no of pages in a Book;
                        ///   if dtBookSummary.Rows[dtBookSummary.Rows.Count -1]["Total"] == 0, then display only Cancel Button.
                        if (dtBookSummary != null && dtBookSummary.Rows.Count > 0)
                        {
                            int intNoOfBookPages = 0;
                            if (dtBookSummary.Rows[dtBookSummary.Rows.Count - 1][TOTALCOLUMN] != DBNull.Value)
                            {
                                int.TryParse(Convert.ToString(dtBookSummary.Rows[dtBookSummary.Rows.Count - 1][TOTALCOLUMN]), out intNoOfBookPages);
                            }
                            // Response.Write(intNoOfBookPages.ToString());
                            if (intNoOfBookPages > 0)
                            {
                                grdWellBookSummary.CssClass = WELLBOOKSUMMARYCSS;
                                grdWellBookSummary.EnableViewState = true;
                                grdWellBookSummary.HeaderStyle.CssClass = FIXEDHEADERCSS;
                                grdWellBookSummary.HeaderStyle.Height = new Unit(20, UnitType.Pixel);
                                grdWellBookSummary.RowStyle.CssClass = EVENROWSTYLECSS;
                                grdWellBookSummary.AlternatingRowStyle.CssClass = ODDROWSTYLECSS;
                                grdWellBookSummary.DataSource = dtBookSummary;
                                grdWellBookSummary.DataBind();
                                grdWellBookSummary.Visible = true;
                                cmdContinuePublish.Visible = true;
                            }
                            else
                            {
                                cmdContinuePublish.Visible = false;
                                grdWellBookSummary.Visible = false;
                                lblPublish.Text = NOACTIVECHAPTERORPAGEMESSAGE;
                                lblPublish.Visible = true;
                            }
                        }
                        else
                        {
                            cmdContinuePublish.Visible = false;
                            grdWellBookSummary.Visible = false;
                            lblPublish.Text = NOACTIVECHAPTERORPAGEMESSAGE;
                            lblPublish.Visible = true;
                        }
                    }
                    else
                    {
                        cmdContinuePublish.Visible = false;
                        grdWellBookSummary.Visible = false;
                        lblPublish.Text = NOACTIVECHAPTERORPAGEMESSAGE;
                        lblPublish.Visible = true;
                    }
                }
            }
            if (dtBookSummary != null)
            {
                dtBookSummary.Dispose();
            }
        }

        /// <summary>
        /// Publishes the Well book.
        /// </summary>
        /// <param name="rowId">The book id.</param>
        private void Publish(int rowId)
        {
            WellBookBLL objWellBook = new WellBookBLL();
            CommonBLL objCommonBLL = new CommonBLL();
            string strActionPerformed = string.Empty;
            string strAlertMessage = string.Empty;

            /// Validate the new book name is not already exists in DWB Books list
            if (!CheckDuplicateName(txtNewBookName.Text.Trim(), DWBTITLECOLUMN, DWBBOOKLIST))
            {
                PrintOptions objPrintOptions = new PrintOptions();
                objPrintOptions.IncludeStoryBoard = false;
                objPrintOptions.IncludePageTitle = true;
                objPrintOptions.IncludeBookTitle = true;
                objPrintOptions.IncludeTOC = true;
                System.Xml.XmlDocument objDWBBookXML = objWellBook.GenerateDWBBookXML(strParentSiteURL, rowId, BOOKACTIONPUBLISH, true,objPrintOptions);

                string strSiteURL = strParentSiteURL;
                SslRequiredWebPart objSslRequired = new SslRequiredWebPart();

                strSiteURL = objSslRequired.GetSslURL(strSiteURL);

                string strBookName = objDWBBookXML.DocumentElement.Attributes["BookName"].Value.ToString();
                string strRequestID = Guid.NewGuid().ToString();
                string strCurrentUser = GetUserName();
                string strLiveBookName = txtNewBookName.Text.Trim();
                UpdateDWBBookPrintDetails(strCurrentUser, strRequestID, strBookName, strLiveBookName, objDWBBookXML, rowId);
                TerminateLiveBook(rowId, DWBBOOKLIST);
                pnlPublishBook.Visible = false;
                strAlertMessage = objCommonBLL.GetAlertMessage(strParentSiteURL, "Publish");
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "documentprinted", "alert(' " + strAlertMessage + "');window.close();", true);
            }
            else
            {
                ExceptionBlock.Visible = true;
                lblException.Visible = true;
                lblException.Text = BOOKTITLEEXISTSMESSAGE;
            }
        }
        /// <summary>
        /// Updates the DWB book print details.
        /// </summary>
        /// <param name="currentUser">The current user.</param>
        /// <param name="requestID">The request ID.</param>
        /// <param name="bookName">Name of the book.</param>
        /// <param name="liveBookName">Name of the live book.</param>
        /// <param name="xmlDoc">The XML doc.</param>
        /// <param name="bookID">The book ID.</param>
        private void UpdateDWBBookPrintDetails(string currentUser, string requestID, string bookName, string liveBookName, XmlDocument xmlDoc, int bookID)
        {
            string strDocumentURL = PortalConfiguration.GetInstance().GetKey("DWBPublishNetworkPath") + bookName + ".pdf";
            //Call BLL method to update the list named "DWB Chapter Print Details" with the above details.
            WellBookBLL objBookBLL = new WellBookBLL();
            objBookBLL.UpdateBookPublishDetails(requestID, strDocumentURL, strParentSiteURL, currentUser, true, liveBookName, xmlDoc, bookID);
        }
        #endregion Private Methods
    }
}
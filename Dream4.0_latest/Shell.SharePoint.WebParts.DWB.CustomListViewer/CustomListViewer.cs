#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: CustomListViewer.cs
#endregion

using System;
using System.Web;
using System.Net;
using System.Data;
using System.Text;
using System.Xml;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DWB.BusinessLogicLayer;
using Shell.SharePoint.DREAM.Utilities;
using System.ComponentModel;
using Microsoft.SharePoint;

namespace Shell.SharePoint.WebParts.DWB.CustomListViewer
{
    /// <summary>
    /// This webpart used to display the data specific to
    /// a List like Master Page,Template or Book
    /// </summary>
    public class CustomListViewer : CustomListViewerHelper
    {
        #region Declaration
        public enum ReportName
        {
            MasterPage,
            Template,
            WellBook,
            BookPages,
            Chapters,
            ChapterPages,
            TemplatePages,
            UserRegistration,
            TeamRegistration,
            StaffRegistration,
            DWBHome,
            WellBookPageView,
            AuditTrail
        }
        ReportName objReportName;
        string strTitle = string.Empty;
        string strParentTitle = string.Empty;
        //added by Praveena        
        string strPageName;
        string strChapterNames;
        string strPageTypes;

        int intNoOfRecords;
        bool blnTerminatedStatus;
        const string STATUSRADIOGROUP = "$rblStatus";
        const string TERMINATE = "Terminated";
        const string ACTIVE = "Active";
        const string TERMINATEDTEMPLATES = "Terminated Templates";
        const string ACTIVETEMPLATES = "Active Templates";
        const string TERMINATEDBOOKS = "Terminated Books";
        const string ACTIVEBOOKS = "Active Books";
        const string ALLBOOKS = "All";
        const string FAVORITESONLY = "Favourites";
        const string ADDTOFAVORITES = "Add To Favourites";
        const string REMOVEFROMFAVORITES = "Remove From Favourites";
        const string PAGESIGNOFF = "PageSignOff";
        const string PAGECANCELSIGNOFF = "CancelPageSignOff";
        const string ACTIVEEXTENSION = " - Active";
        const string TERMINATEEXTENSION = " - Terminated";
        const string STATUS = "status";
        const string TERMINATESTATUS = "TerminateStatus";
        const string TRUE = "true";
        const string FALSE = "false";
        const string NODETAILMESSAGE = "There are currently no details to be displayed.";
        const string SPACE3 = "&nbsp; &nbsp; &nbsp;";
        const string TYPEIIIUPLOADED = "Type3Uploaded";
        //Added by Praveena
        const string SIGNEDOFF = "Signed Off";
        const string UNSIGNEDOFF = "Unsigned Off";
        const string SIGNOFF = "Sign Off";
        const string UNSIGNOFF = "Unsign Off";



        HiddenField hdnSelectedOptions;
        HiddenField hdnReportType;
        RadioButtonList rblStatus;
        Panel pnlRadioButtonList;
        Panel pnlTopLevelContainer;
        Panel pnlFilterOptions;
        Button btnAddMasterPage;
        Button btnAddChapter;
        Button btnAddChapterSuquence;
        Button btnAddTemplate;
        Button btnAddRemoveTemplatePages;
        Button btnAddBook;
        Label lblWellBookPageViewFilter;
        Button btnAlterPageSequence;
        Button btnChapterPage;
        Button btnAlterChapterPageSequence;
        /// <summary>
        /// Button to add new Registered user
        /// Appears in User Registration Screen
        /// </summary>
        Button btnAddNewUser;
        /// <summary>
        /// Button to add new Team
        /// Appears in Team Registration Screen
        /// </summary>
        Button btnAddNewTeam;
        /// <summary>
        /// Button to add/remove Staff to/from Team
        /// Appears in Staff Registration Screen
        /// </summary>
        Button btnAddRemoveStaff;
        /// <summary>
        /// Button to change the Rank of the Staff within Discipline in a Team
        /// Appears in Staff Registration Screen
        /// </summary>
        Button btnRankStaff;

        Button btnFirePostBack;

        /// <summary>
        /// Radio Button list for Favorite
        /// </summary>
        RadioButtonList rdoFavourites;

        /// <summary>
        /// Button to add to favorites selection
        /// </summary>
        Button btnAddToFavorites;
        HiddenField hdnSelectedAsFavorite;
        HiddenField hdnUserFavorites;
        HiddenField hidtype3uploaded;

        //Added by Praveena
        RadioButtonList rblSignOffStatus;
        Button btnFilter;
        Button btnReset;
        Label lblPageName;
        Label lblPageType;
        Label lblChapterName;
        DropDownList cboPageName;
        ListBox lstChapterNames;
        CheckBoxList cblPageTypes;
        Button btnSignOff;
        HiddenField hdnSignOffPageIds;
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the type of the custom list.
        /// </summary>
        /// <value>The type of the custom list.</value>
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DWB Properties")]
        [WebDisplayNameAttribute("Report Name")]
        [WebDescription("Select the Report Name.")]
        public ReportName CustomListType
        {
            get
            {
                return objReportName;
            }
            set
            {
                objReportName = value;
            }
        }

        /// <summary>
        /// Gets or sets the terminate list item.
        /// </summary>
        /// <value>The terminate list item.</value>
        public object TerminateListItem
        {
            get
            {
                return ViewState["TerminateListItem"];
            }
            set
            {
                ViewState["TerminateListItem"] = value;
            }
        }

        //Added by Praveena 
        //to hold Page Name Filter Value
        public string PageName
        {
            get { return strPageName; }
            set { strPageName = value; }
        }

        //to hold Chapter Name Filter Value
        public string ChapterNames
        {
            get { return strChapterNames; }
            set { strChapterNames = value; }
        }

        ////to hold Page Type Filter Value
        public string PageTypes
        {
            get { return strPageTypes; }
            set { strPageTypes = value; }
        }


        #endregion
        #region Overridden Methods

        /// <summary>
        /// On load event triggered by ASP.net when page is loaded.
        /// This method is used to read the event arguement and eventtargert
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {

            base.OnLoad(e);
            int intRowId = 0;
            String strEventArguments = this.Page.Request.Params[EVENTARG];
            String strEventTarget = this.Page.Request.Params[EVENTTARGET];
            if (!string.IsNullOrEmpty(strEventTarget) && (strEventTarget.Equals(STATUSACTIVATE) || strEventTarget.Equals(STATUSTERMINATE) || strEventTarget.Equals(STATUSREMOVE)))
            {
                if (!string.IsNullOrEmpty(strEventArguments))
                {
                    if (int.TryParse(strEventArguments, out intRowId))
                    {
                        UpdateListItemStatus(intRowId, strEventTarget);
                    }
                }
            }
            if (!string.IsNullOrEmpty(strEventTarget) && (strEventTarget.Equals(PAGESIGNOFF) || strEventTarget.Equals(PAGECANCELSIGNOFF)))
            {
                if (!string.IsNullOrEmpty(strEventArguments))
                {
                    int.TryParse(strEventArguments, out intRowId);
                    if (intRowId != 0)
                    {
                        UpdatePageSignOffStatus(intRowId, strEventTarget);
                    }
                }
            }
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls 
        /// that use composition-based implementation to create any child controls they contain  
        /// in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            strSiteURL = SPContext.Current.Site.Url.ToString();
            try
            {
                #region DREAM 4.0 - eWB 2.0 - AJAX Implemenation
                EnsurePanelFix(typeof(CustomListViewer));
                #endregion
                CreateViewerControls();

            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
        }

        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            DataTable dtRecords = null;

            try
            {
                /// Gets the Title of the Parent
                /// Eg : Template Title in case of Maintain Template Pages screen
                /// Should get Book title in case of Chapters Screen
                strParentTitle = GetParentTitle();

                if (string.Compare(CustomListType.ToString(), WELLBOOKPAGEVIEW, true) == 0)
                {
                    #region setting radiobutton status
                    if (rblSignOffStatus.SelectedIndex == -1)
                    {
                        string strPageStatus = HttpContext.Current.Request.QueryString["pageStatus"];
                        string strPageOwner = HttpContext.Current.Request.QueryString["pageOwner"];
                        if (string.Equals(strPageStatus, "Total") && string.Equals(strPageOwner, "All"))
                        {
                            rblSignOffStatus.SelectedIndex = 1;
                            rblSignOffStatus.Enabled = true;
                        }
                        else if (string.Equals(strPageOwner, "All"))
                        {
                            if (string.Equals(strPageStatus, "SignedOff"))
                            {
                                rblSignOffStatus.SelectedIndex = 0;
                                rblSignOffStatus.Enabled = false;
                            }
                            else if (string.Equals(strPageStatus, "NotSignedOff"))
                            {
                                rblSignOffStatus.SelectedIndex = 1;
                                rblSignOffStatus.Enabled = false;
                            }
                            else
                            {
                                rblSignOffStatus.SelectedIndex = 1;
                                rblSignOffStatus.Enabled = true;
                            }
                        }
                        else
                        {
                            if (string.Equals(strPageStatus, "SignedOff"))
                            {
                                rblSignOffStatus.SelectedIndex = 0;
                                rblSignOffStatus.Enabled = false;
                            }
                            else if (string.Equals(strPageStatus, "NotSignedOff"))
                            {
                                rblSignOffStatus.SelectedIndex = 1;
                                rblSignOffStatus.Enabled = false;
                            }
                            else if (string.Equals(strPageStatus, "Total"))
                            {
                                rblSignOffStatus.SelectedIndex = 1;
                                rblSignOffStatus.Enabled = true;
                            }
                            else
                            {
                                rblSignOffStatus.SelectedIndex = 1;
                                rblSignOffStatus.Enabled = true;
                            }
                        }
                    }
                    #endregion setting  radiobutton status
                    if (rblSignOffStatus.SelectedIndex == 0)
                    {
                        SignedOffStatus = "Yes";
                        btnSignOff.Text = UNSIGNOFF;
                    }
                    else
                    {
                        SignedOffStatus = "No";
                        btnSignOff.Text = SIGNOFF;
                    }
                }

                /// Get all the records of the List Type
                /// Eg: All Pages in Records in case of Maintain Template Pages screen

                /// This method call retrieves the DataTable consists of the List Items to be rendered
                dtRecords = GetRecords();

                if (dtRecords != null && dtRecords.Rows != null)
                {
                    intNoOfRecords = dtRecords.Rows.Count;
                }

                RenderParentTable(writer, string.Format(strTitle, strParentTitle));
                hdnReportType.Value = string.Format(strTitle, strParentTitle);

                if (pnlRadioButtonList != null && pnlRadioButtonList.Visible)
                {
                    if (ActiveStatus)
                    {
                        hdnReportType.Value = hdnReportType.Value + ACTIVEEXTENSION;
                    }
                    else
                    {
                        hdnReportType.Value = hdnReportType.Value + TERMINATEEXTENSION;
                    }
                }

                writer.Write(" <tr><td>");
                hdnReportType.RenderControl(writer);
                hdnSelectedOptions.RenderControl(writer);
                writer.Write("    ");
                /// No of records - This variable is used to show/hide the Alter Page Sequence Button 
                if (lblWellBookPageViewFilter != null)
                {
                    lblWellBookPageViewFilter.RenderControl(writer);
                }
                if (btnFirePostBack != null)
                {
                    btnFirePostBack.RenderControl(writer);
                }
                SetViewerButtonProperties(writer, intNoOfRecords);

                writer.Write("</td>");

                if (dtRecords == null || dtRecords.Rows.Count == 0)
                {
                    if (!string.Equals(ListReportName, "Audit Trail"))
                    {
                        writer.Write("<td  class=\"ListViewerOptionsCSS\" align=\"right\" colspan=\"2\">");
                        if (string.Compare(CustomListType.ToString(), DWBHOME, true) == 0)
                        {
                            writer.Write("<table width=\"100%\"><tr><td width=\"50%\"></td><td width=\"40%\">");
                            pnlTopLevelContainer.RenderControl(writer);
                            writer.Write("</td><td align=\"left\" width=\"10%\">");
                            if (btnAddToFavorites != null)
                                btnAddToFavorites.RenderControl(writer);
                            writer.Write("</td></tr></table>");
                        }
                        else if (string.Compare(CustomListType.ToString(), WELLBOOKPAGEVIEW, true) == 0)
                        {
                            writer.Write("<table width=\"100%\"><tr><td width=\"50%\"></td><td width=\"40%\">");
                            pnlTopLevelContainer.RenderControl(writer);
                            writer.Write("</td><td align=\"left\" width=\"10%\"></td></tr></table>");
                        }
                        else
                        {
                            pnlTopLevelContainer.RenderControl(writer);
                        }


                    }
                }
                else
                {
                    if (!string.Equals(ListReportName, "Audit Trail"))
                    {
                        writer.Write("<td  class=\"ListViewerOptionsCSS\" align=\"right\" nowrap=\"nowrap\">");

                        if (string.Compare(CustomListType.ToString(), DWBHOME, true) == 0)
                        {
                            writer.Write("<table width=\"100%\"><tr><td width=\"50%\"></td><td width=\"40%\">");
                            pnlTopLevelContainer.RenderControl(writer);
                            writer.Write("</td><td align=\"left\" width=\"10%\">");
                            if (btnAddToFavorites != null)
                                btnAddToFavorites.RenderControl(writer);
                            writer.Write("</td></tr></table>");
                        }
                        else if (string.Compare(CustomListType.ToString(), WELLBOOKPAGEVIEW, true) == 0)
                        {
                            writer.Write("<table width=\"100%\"><tr><td width=\"50%\"></td><td width=\"40%\">");
                            pnlTopLevelContainer.RenderControl(writer);
                            writer.Write("</td><td align=\"left\" width=\"10%\"></td></tr></table>");
                        }
                        else
                        {
                            pnlTopLevelContainer.RenderControl(writer);
                        }
                        writer.Write(" </td><td width=\"5%\" align=\"right\">");
                        linkPrint.RenderControl(writer);
                        linkExcel.RenderControl(writer);
                        #region DREAM 4.- - eWB 2.0
                        writer.Write(GetArchiveOptionsDiv());
                        #endregion

                    }
                    else
                    {
                        writer.Write("<table width=\"100%\"><tr><td width=\"100%\" align=\"right\">");
                        linkPrint.RenderControl(writer);
                        linkExcel.RenderControl(writer);
                    }
                }
                writer.Write(" </td></tr>");

                //Added by Praveena for applying filter to datatable
                DataView dvResultDataView = new DataView();
                if (string.Compare(CustomListType.ToString(), WELLBOOKPAGEVIEW, true) == 0)
                {
                    //to add  Filter Panel
                    writer.Write("<tr><td align=\"right\" colspan=\"3\">");
                    pnlFilterOptions.RenderControl(writer);
                    writer.Write(" </td></tr>");

                    //to add Filter to DataTable
                    dvResultDataView = dtRecords.DefaultView;
                    SetFilterProperties();
                    StringBuilder strRowFilters = new StringBuilder();
                    if (!string.IsNullOrEmpty(ChapterNames))
                    {
                        strRowFilters.Append("ChapterTitle in (" + ChapterNames + ") ");
                    }
                    if (!string.IsNullOrEmpty(PageTypes))
                    {
                        if (string.IsNullOrEmpty(strRowFilters.ToString()))
                        {
                            strRowFilters.Append("Connection_Type in (" + PageTypes + ") ");
                        }
                        else
                        {
                            strRowFilters.Append("AND Connection_Type in (" + PageTypes + ") ");
                        }
                    }
                    if (!string.IsNullOrEmpty(PageName) && !PageName.Equals("--Select All--"))
                    {
                        if (string.IsNullOrEmpty(strRowFilters.ToString()))
                        {
                            strRowFilters.Append("Page_Name='" + PageName + "' ");
                        }
                        else
                        {
                            strRowFilters.Append(" AND Page_Name='" + PageName + "' ");
                        }
                    }

                    //add SortOrder to DataView
                    dvResultDataView = SetSortOrder(dvResultDataView);
                    dvResultDataView.RowFilter = strRowFilters.ToString();
                    dtRecords = dvResultDataView.ToTable();
                }

                /// This method call generates the XML to be transformed along with XSL and renders the output                
                GetDetailsFromList(dtRecords, writer);
                if (string.Compare(CustomListType.ToString(), WELLBOOKPAGEVIEW, true) == 0)
                {
                    if (dtRecords.Rows.Count != 0)
                    {
                        writer.Write("<tr><td align=\"right\" colspan=\"3\">");
                        if (btnSignOff != null)
                            btnSignOff.RenderControl(writer);
                        writer.Write("</td></tr>");
                    }
                }
                writer.Write("</table>");
                writer.Write(@"<Script language='javascript'> setWindowTitle(" + strTitle + "'); </Script>");
                if (string.Compare(CustomListType.ToString(), WELLBOOKPAGEVIEW, true) == 0)
                {
                    writer.Write(@"<Script language='javascript'> DisablePageType();</Script>");
                }
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), webEx);
                writer.Write(webEx.Message);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
            finally
            {
                if (dtRecords != null)
                    dtRecords.Dispose();
            }
        }

        #endregion

        #region Event Hanlder Methods

        /// <summary>
        /// Event handler for Favourite Index changed/
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        void rdoFavourites_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdoFavourites.SelectedIndex == 0)
            {
                btnAddToFavorites.Text = ADDTOFAVORITES;
            }
            else if (rdoFavourites.SelectedIndex == 1)
            {
                btnAddToFavorites.Text = REMOVEFROMFAVORITES;
            }
            else
            {
                btnAddToFavorites.Visible = false;
            }
        }

        /// <summary>
        /// Rank the staff.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        void btnRankStaff_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("RankStaff.aspx?listType=" + STAFFRANK + "&idValue=" + HttpContext.Current.Request.QueryString[IDVALUEQUERYSTRING], false);
        }

        /// <summary>
        /// Handles the Click event of the Add/Remove Staff control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnAddRemoveStaff_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("AddRemoveStaff.aspx?listType=" + STAFFREGISTRATION + "&idValue=" + HttpContext.Current.Request.QueryString[IDVALUEQUERYSTRING], false);
        }

        /// <summary>
        /// Handles the Click event of the ChapterPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnChapterPage_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("ChapterPage.aspx?ChapterID=" + HttpContext.Current.Request.QueryString["ChapterID"], false);
        }

        /// <summary>
        /// Handles the Click event of the Alter Chapter PageSequence control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnAlterChapterPageSequence_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("AlterChapterPageSequence.aspx?listtype=ChapterPageMapping&parentId=" + HttpContext.Current.Request.QueryString["ChapterID"], false);
        }

        /// <summary>
        /// Handles the Click event of the Add Chapter Sequence control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnAddChapterSuquence_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("AlterChapterSequence.aspx?listtype=Chapter&parentId=" + HttpContext.Current.Request.QueryString["BookId"], false);
        }

        /// <summary>
        /// Handles the Click event of the Add Chapter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnAddChapter_Click(object sender, EventArgs e)
        {
            string strBookId = HttpContext.Current.Request.QueryString["BookId"];
            Page.Response.Redirect("WellBookChapter.aspx?mode=add&BookId=" + strBookId, false);
        }

        /// <summary>
        /// Handles the Click event of the Alter Page Sequence control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnAlterPageSequence_Click(object sender, EventArgs e)
        {
            switch (CustomListType.ToString())
            {
                case MASTERPAGEREPORT:
                    {
                        Page.Response.Redirect("AlterMasterPages.aspx?listtype=MasterPage", false);
                        break;
                    }
                case TEMPLATEPAGESREPORT:
                    {
                        Page.Response.Redirect("AlterTemplatePageSequence.aspx?parentId=" + HttpContext.Current.Request.QueryString[IDVALUEQUERYSTRING] + "&listtype=" + TEMPLATEPAGESREPORT, false);
                        break;
                    }
                default:
                    break;


            }
        }

        /// <summary>
        /// Handles the Click event of the Add To Favorites control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void BtnAddToFavorites_Click(object sender, EventArgs e)
        {
            string strFavroites = hdnSelectedAsFavorite.Value;
            strSiteURL = SPContext.Current.Site.Url.ToString();
            /// Update the DWB User list with the favorites selected. 
            strUserName = GetUserName();
            objWellBook = new WellBookBLL();
            if (string.Compare(btnAddToFavorites.Text, ADDTOFAVORITES, true) == 0)
            {
                objWellBook.AddToFavorites(strSiteURL, strFavroites, USERLIST, strUserName, true);
            }
            else if (string.Compare(btnAddToFavorites.Text, REMOVEFROMFAVORITES, true) == 0)
            {
                objWellBook.AddToFavorites(strSiteURL, strFavroites, USERLIST, strUserName, false);
            }
        }


        /// <summary>
        /// Handles the SelectedIndexChanged event of the rblStatus control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        void rblStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblStatus.SelectedIndex == 0)
            {
                TerminateListItem = false;
                ActiveStatus = true;
            }
            else
            {
                TerminateListItem = true;
                ActiveStatus = false;

            }
            blnInitializePageNumber = true;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the rblSignOffStatus control.
        /// Added By: Praveena 
        /// Date:10/09/2010
        /// Reason: SelectedIndexChanged event of the rblSignOffStatus control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void rblSignOffStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            //clearing all the filters when SignOff/UnsignedOff pages are selected.
            if ((!string.IsNullOrEmpty(this.Page.Request.Params[EVENTTARGET])) && (this.Page.Request.Params[EVENTTARGET].Contains(rblSignOffStatus.ID)))
            {
                //Setting PageName dropdown selected index to zero
                cboPageName.SelectedIndex = 0;
                // deselecting all ChapterNames selected
                lstChapterNames.SelectedIndex = -1;
                //deselecting all PageTypes selected.
                cblPageTypes.Items[0].Selected = false;
                cblPageTypes.Items[1].Selected = false;
                cblPageTypes.Items[2].Selected = false;
                //Setting the Filter properties to empty
                strPageName = cboPageName.SelectedItem.Text;
                strChapterNames = string.Empty;
                strPageTypes = string.Empty;
            }
        }

        /// <summary>
        /// Handles the Button click  event of the Filter Button
        /// Added By: Praveena 
        /// Date:11/09/2010
        /// Reason: Button click  event of the Filter Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnFilter_Click(object sender, EventArgs e)
        {
            SetFilterProperties();
        }

        /// <summary>
        /// Handles the Button click  event of the Reset Button
        /// Added By: Praveena 
        /// Date:11/09/2010
        /// Reason: Button click  event of the Reset Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnReset_Click(object sender, EventArgs e)
        {
            cboPageName.SelectedIndex = 0;
            lstChapterNames.SelectedIndex = -1;
            cblPageTypes.Items[0].Selected = false;
            cblPageTypes.Items[1].Selected = false;
            cblPageTypes.Items[2].Selected = false;
            strPageName = cboPageName.SelectedItem.Text;
            strChapterNames = string.Empty;
            strPageTypes = string.Empty;
        }

        /// <summary>
        /// Handles the Button click  event of the SignOff Button
        /// Added By: Praveena 
        /// Date:11/09/2010
        /// Reason: Button click  event of the SignOff Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSignOff_Click(object sender, EventArgs e)
        {
            try
            {
                if (rblSignOffStatus.SelectedIndex == -1)
                {
                    string strPageStatus = HttpContext.Current.Request.QueryString["pageStatus"];
                    if (string.Equals(strPageStatus, "SignedOff"))
                    {
                        btnSignOff.Text = UNSIGNOFF;
                    }
                    else if (string.Equals(strPageStatus, "NotSignedOff"))
                    {
                        btnSignOff.Text = SIGNOFF;
                    }
                }
                else if (rblSignOffStatus.SelectedIndex == 0)
                {
                    btnSignOff.Text = UNSIGNOFF;
                }
                else
                {
                    btnSignOff.Text = SIGNOFF;
                }
                string strSignOffPages = string.Empty;
                strSignOffPages = hdnSignOffPageIds.Value;
                string strAction = string.Empty;

                /// Update the Chapter Pages Mapping List and Audit List

                objWellBook = new WellBookBLL();
                if (string.Equals(btnSignOff.Text, SIGNOFF))
                {
                    strAction = "PageSignOff";
                    UpdateBulkPageSignOffStatus(strSignOffPages, strAction);
                }
                else if (string.Equals(btnSignOff.Text, UNSIGNOFF))
                {
                    strAction = "PageUnsignOff";
                    UpdateBulkPageSignOffStatus(strSignOffPages, strAction);
                }
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), webEx);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
            finally
            {
                hdnSignOffPageIds.Value = string.Empty;
            }
        }


        #endregion Event Hanlder Methods

        #region Private Methods
        /// <summary>
        /// Creates the viewer controls.
        /// </summary>
        private void CreateViewerControls()
        {
            pnlTopLevelContainer = new Panel();
            pnlTopLevelContainer.ID = "pnlToplevelContainerID";
            pnlTopLevelContainer.CssClass = "DWBToplevelReportPanelCSS";

            pnlFilterOptions = new Panel();
            pnlFilterOptions.ID = "pnlFilterOptionsID";
            pnlFilterOptions.CssClass = "DWBToplevelReportPanelCSS";
            pnlFilterOptions.Visible = false;

            pnlRadioButtonList = new Panel();
            pnlRadioButtonList.ID = "pnlRadiobuttonListID";
            pnlRadioButtonList.CssClass = "DWBRadioBtnPanelCSS";
            pnlRadioButtonList.GroupingText = "Show";
            pnlRadioButtonList.HorizontalAlign = HorizontalAlign.Left;

            rblStatus = new RadioButtonList();
            rblStatus.CssClass = "ReportRadioBtnCSS";
            hdnSelectedOptions = new HiddenField();
            hdnSelectedOptions.ID = "hdnSelectedOptions";
            hdnReportType = new HiddenField();
            hdnReportType.ID = "hdnReportType";

            rblStatus.AutoPostBack = true;
            rblStatus.ID = "rblStatus";
            rblStatus.RepeatDirection = RepeatDirection.Horizontal;
            rblStatus.RepeatLayout = RepeatLayout.Flow;
            rblStatus.SelectedIndexChanged += new EventHandler(rblStatus_SelectedIndexChanged);
            pnlRadioButtonList.Controls.Add(rblStatus);
            pnlTopLevelContainer.Controls.Add(pnlRadioButtonList);
            rblStatus.Items.Add(ACTIVE);
            rblStatus.Items.Add(TERMINATE);


            /// Print and excel options 
            linkPrint = new HyperLink();
            linkPrint.ID = "linkPrint";
            linkPrint.CssClass = "resultHyperLink";
            linkPrint.NavigateUrl = "javascript:PrintContent();";

            linkPrint.ImageUrl = "/_layouts/DREAM/images/icon_print.gif";
            linkExcel.ID = "linkExcel";
            linkExcel.CssClass = "resultHyperLink";
            if (string.Equals(CustomListType.ToString(), WELLBOOKPAGEVIEW))
                linkExcel.NavigateUrl = "javascript:DWBExportToExcelWithCheckBoxes();";
            else
                linkExcel.NavigateUrl = "javascript:DWBExportToExcel();";
            linkExcel.ImageUrl = "/_layouts/DREAM/images/icon_Excel.gif";

            switch (CustomListType.ToString())
            {
                case MASTERPAGEREPORT:
                    {
                        //strTitle = "List of Master Pages";
                        strTitle = "Maintain Master Pages";

                        btnAddMasterPage = new Button();
                        btnAddMasterPage.ID = "btnAddMasterPage";
                        btnAddMasterPage.CssClass = "DWBbuttonAdvSrch";
                        btnAddMasterPage.Text = "New Master Page";
                        btnAddMasterPage.OnClientClick = "return RedirectTo('" + "/Pages/MasterPage.aspx?mode=add" + "');";
                        this.Controls.Add(btnAddMasterPage);
                        btnAlterPageSequence = new Button();
                        btnAlterPageSequence.ID = "btnAlterPageSequenceID";
                        btnAlterPageSequence.CssClass = "DWBbuttonAdvSrch";
                        btnAlterPageSequence.Text = "Alter Page Sequence";
                        btnAlterPageSequence.Click += new EventHandler(btnAlterPageSequence_Click);
                        this.Controls.Add(btnAlterPageSequence);
                        ListName = MASTERPAGELIST;
                        AuditListName = MASTERPAGEAUDITTRAIL;

                        break;
                    }
                case WELLBOOKPAGEVIEW:
                    {
                        hidtype3uploaded = new HiddenField();
                        hidtype3uploaded.ID = "hidtype3uploaded";
                        if (HttpContext.Current.Session[TYPEIIIUPLOADED] != null)
                            hidtype3uploaded.Value = HttpContext.Current.Session[TYPEIIIUPLOADED].ToString().ToLowerInvariant();
                        pnlTopLevelContainer.Controls.Add(hidtype3uploaded);
                        btnFirePostBack = new Button();
                        btnFirePostBack.ID = "btnFirePostBack";
                        btnFirePostBack.Attributes.Add("style", "display:none;");
                        btnFirePostBack.OnClientClick = "Javascript:return FirePostBack('true');";
                        btnFirePostBack.Click += new EventHandler(btnFirePostBack_Click);
                        pnlTopLevelContainer.Controls.Add(btnFirePostBack);
                        strTitle = "WellBook: {0}";
                        lblWellBookPageViewFilter = new Label();
                        lblWellBookPageViewFilter.ID = "WellBookPageViewFilterID";
                        lblWellBookPageViewFilter.CssClass = "DWItemText";

                        pnlRadioButtonList.Visible = true;
                        pnlTopLevelContainer.HorizontalAlign = HorizontalAlign.Left;
                        pnlTopLevelContainer.Direction = ContentDirection.LeftToRight;
                        pnlTopLevelContainer.Wrap = false;
                        pnlTopLevelContainer.GroupingText = "Sign Off Status";
                        pnlTopLevelContainer.CssClass = "DWBHomeToplevelReportPanelCSS";
                        rblSignOffStatus = new RadioButtonList();
                        rblSignOffStatus.CssClass = "ReportRadioBtnCSS";
                        rblSignOffStatus.AutoPostBack = true;
                        rblSignOffStatus.ID = "rblSignOffStatus";
                        rblSignOffStatus.Items.Add(SIGNEDOFF);
                        rblSignOffStatus.Items.Add(UNSIGNEDOFF);
                        rblSignOffStatus.RepeatDirection = RepeatDirection.Horizontal;
                        rblSignOffStatus.RepeatLayout = RepeatLayout.Flow;
                        rblSignOffStatus.SelectedIndexChanged += new EventHandler(rblSignOffStatus_SelectedIndexChanged);
                        pnlRadioButtonList.Controls.Add(rblSignOffStatus);
                        pnlRadioButtonList.GroupingText = string.Empty;
                        pnlTopLevelContainer.Controls.Add(pnlRadioButtonList);


                        //for Filter
                        CreateFilterPanelControls();
                        CreateFilterPanel();

                        #region setting  labeltext and radiobutton status
                        if (rblSignOffStatus.SelectedIndex == -1)
                        {
                            string strPageStatus = HttpContext.Current.Request.QueryString["pageStatus"];
                            string strPageOwner = HttpContext.Current.Request.QueryString["pageOwner"];
                            if (string.Equals(strPageStatus, "Total") && string.Equals(strPageOwner, "All"))
                            {
                                lblWellBookPageViewFilter.Text = "Page Selection : All pages in the book";
                            }
                            else if (string.Equals(strPageOwner, "All"))
                            {
                                if (string.Equals(strPageStatus, "SignedOff"))
                                {
                                    lblWellBookPageViewFilter.Text = "Page Selection : Signed Off = Y";
                                }
                                else if (string.Equals(strPageStatus, "NotSignedOff"))
                                {
                                    lblWellBookPageViewFilter.Text = "Page Selection : Signed Off = N";
                                }
                                else
                                {
                                    lblWellBookPageViewFilter.Text = "Page Selection : Empty = Y";
                                }
                            }
                            else
                            {
                                if (string.Equals(strPageStatus, "SignedOff"))
                                {
                                    lblWellBookPageViewFilter.Text = "Page Selection : Owner = " + strPageOwner + " + Signed Off = Y";
                                }
                                else if (string.Equals(strPageStatus, "NotSignedOff"))
                                {
                                    lblWellBookPageViewFilter.Text = "Page Selection : Owner = " + strPageOwner + " + Signed Off = N";
                                }
                                else if (string.Equals(strPageStatus, "Total"))
                                {
                                    lblWellBookPageViewFilter.Text = "Page Selection : Owner = " + strPageOwner + " + All Pages in Book";
                                }
                                else
                                {
                                    lblWellBookPageViewFilter.Text = "Page Selection : Owner = " + strPageOwner + " + Empty = Y";
                                }
                            }
                        }
                        #endregion setting  labeltext and radiobutton status

                        this.Controls.Add(lblWellBookPageViewFilter);
                        this.Controls.Add(pnlFilterOptions);
                        this.Controls.Add(btnSignOff);
                    }
                    break;
                case TEMPLATEREPORT:
                    {
                        //strTitle = "List of Templates";
                        strTitle = "Maintain Templates";
                        btnAddTemplate = new Button();
                        btnAddTemplate.ID = "btnAddTemplate";
                        btnAddTemplate.CssClass = "DWBbuttonAdvSrch";
                        btnAddTemplate.Text = "New Template";
                        btnAddTemplate.OnClientClick = "return RedirectTo('" + "/Pages/Template.aspx?mode=add" + "');";
                        this.Controls.Add(btnAddTemplate);
                        break;
                    }

                case TEMPLATEPAGESREPORT:
                    {
                        pnlRadioButtonList.Visible = false;
                        //strTitle = "List of Template Pages : {0}";
                        strTitle = "Maintain Template Pages : {0}";
                        btnAddRemoveTemplatePages = new Button();
                        btnAddRemoveTemplatePages.ID = "btnAddRemoveTemplatePages";
                        btnAddRemoveTemplatePages.CssClass = "DWBbuttonAdvSrch";
                        btnAddRemoveTemplatePages.Text = "Add/Remove Page(s)";
                        /// Javascript Method = openTemplatePages(templateid,listType)
                        btnAddRemoveTemplatePages.OnClientClick = "return openPages('" + HttpContext.Current.Request.QueryString[IDVALUEQUERYSTRING] + "','" + ADDREMOVETEMPLATEPAGES + "');";
                        this.Controls.Add(btnAddRemoveTemplatePages);

                        btnAlterPageSequence = new Button();
                        btnAlterPageSequence.ID = "btnAlterPageSequenceID";
                        btnAlterPageSequence.CssClass = "DWBbuttonAdvSrch";
                        btnAlterPageSequence.Text = "Alter Page Sequence";
                        btnAlterPageSequence.Click += new EventHandler(btnAlterPageSequence_Click);
                        this.Controls.Add(btnAlterPageSequence);
                        break;
                    }
                case WELLBOOKREPORT:
                    {
                        //strTitle = "List of Well Books";
                        strTitle = "Maintain Well Books";
                        btnAddBook = new Button();
                        btnAddBook.ID = "btnAddBook";
                        btnAddBook.CssClass = "DWBbuttonAdvSrch";
                        btnAddBook.Text = "New Book";
                        btnAddBook.OnClientClick = "return RedirectTo('" + "/Pages/NewBook.aspx?mode=add" + "');";
                        this.Controls.Add(btnAddBook);
                        break;
                    }
                case CHAPTERREPORT:
                    {
                        strTitle = "Maintain Chapters: {0}";
                        btnAddChapter = new Button();
                        btnAddChapter.ID = "btnAddChapter";
                        btnAddChapter.CssClass = "DWBbuttonAdvSrch";
                        btnAddChapter.Text = "New Chapter";
                        btnAddChapter.Click += new EventHandler(btnAddChapter_Click);
                        this.Controls.Add(btnAddChapter);
                        btnAddChapterSuquence = new Button();

                        btnAddChapterSuquence.ID = "btnAlterChapterSequenceID";
                        btnAddChapterSuquence.CssClass = "DWBbuttonAdvSrch";
                        btnAddChapterSuquence.Text = "Alter Chapter Sequence";
                        btnAddChapterSuquence.Click += new EventHandler(btnAddChapterSuquence_Click);
                        this.Controls.Add(btnAddChapterSuquence);
                        ListName = CHAPTERLIST;
                        AuditListName = CHAPTERLISTAUDITTRAIL;
                        break;
                    }
                case CHAPTERPAGEREPORT:
                    {
                        strTitle = "Maintain Chapter Pages: {0}";
                        btnChapterPage = new Button();
                        btnChapterPage.ID = "btnAddChapterPage";
                        btnChapterPage.CssClass = "DWBbuttonAdvSrch";
                        btnChapterPage.Text = "Add Page";
                        btnChapterPage.Click += new EventHandler(btnChapterPage_Click);
                        this.Controls.Add(btnChapterPage);
                        btnAlterChapterPageSequence = new Button();

                        btnAlterChapterPageSequence.ID = "btnAlterChapterPageSequenceID";
                        btnAlterChapterPageSequence.CssClass = "DWBbuttonAdvSrch";
                        btnAlterChapterPageSequence.Text = "Alter Page Sequence";
                        btnAlterChapterPageSequence.Click += new EventHandler(btnAlterChapterPageSequence_Click);
                        this.Controls.Add(btnAlterChapterPageSequence);
                        break;
                    }

                case CHAPTERPAGEMAPPINGREPORT:
                    {
                        strTitle = "Maintain Book Pages: {0}";
                        break;
                    }
                case USERREGISTRATION:
                    {
                        strTitle = "User Registration";
                        /// New User Button
                        btnAddNewUser = new Button();
                        btnAddNewUser.ID = "btnAddNewUser";
                        btnAddNewUser.CssClass = "DWBbuttonAdvSrch";
                        btnAddNewUser.Text = "New User";
                        btnAddNewUser.OnClientClick = "return RedirectTo('" + "/Pages/AddUser.aspx?mode=add" + "');";
                        this.Controls.Add(btnAddNewUser);
                        break;

                    }
                case TEAMREGISTRATION:
                    {
                        strTitle = "Team Registration";
                        /// New Team Button
                        btnAddNewTeam = new Button();
                        btnAddNewTeam.ID = "btnAddNewTeam";
                        btnAddNewTeam.CssClass = "DWBbuttonAdvSrch";
                        btnAddNewTeam.Text = "New Team";
                        btnAddNewTeam.OnClientClick = "return RedirectTo('" + "/Pages/AddTeam.aspx?mode=add" + "');";
                        this.Controls.Add(btnAddNewTeam);
                        break;
                    }
                case STAFFREGISTRATION:
                    {
                        pnlRadioButtonList.Visible = false;
                        strTitle = "List of Staff : {0}";

                        /// Add/Remove Staff Button
                        btnAddRemoveStaff = new Button();
                        btnAddRemoveStaff.ID = "btnAddRemoveStaff";
                        btnAddRemoveStaff.CssClass = "DWBbuttonAdvSrch";
                        btnAddRemoveStaff.Text = "Add / Remove Staff ";
                        btnAddRemoveStaff.Click += new EventHandler(btnAddRemoveStaff_Click);

                        this.Controls.Add(btnAddRemoveStaff);

                        btnRankStaff = new Button();
                        btnRankStaff.ID = "btnRankStaff";
                        btnRankStaff.CssClass = "DWBbuttonAdvSrch";
                        btnRankStaff.Text = "Rank Staff";
                        btnRankStaff.Click += new EventHandler(btnRankStaff_Click);
                        this.Controls.Add(btnRankStaff);
                        break;
                    }
                case DWBHOME:
                    {
                        pnlRadioButtonList.Visible = true;
                        pnlTopLevelContainer.HorizontalAlign = HorizontalAlign.Left;
                        pnlTopLevelContainer.Direction = ContentDirection.LeftToRight;
                        pnlTopLevelContainer.Wrap = false;
                        pnlTopLevelContainer.GroupingText = "Show Favourites";
                        pnlTopLevelContainer.CssClass = "DWBHomeToplevelReportPanelCSS";
                        rdoFavourites = new RadioButtonList();
                        rdoFavourites.CssClass = "ReportRadioBtnCSS";
                        rdoFavourites.AutoPostBack = true;
                        rdoFavourites.ID = "rdoFavourites";
                        rdoFavourites.RepeatDirection = RepeatDirection.Horizontal;
                        rdoFavourites.RepeatLayout = RepeatLayout.Flow;
                        rdoFavourites.SelectedIndexChanged += new EventHandler(rdoFavourites_SelectedIndexChanged);
                        pnlRadioButtonList.Controls.Add(rdoFavourites);
                        pnlRadioButtonList.GroupingText = string.Empty;
                        pnlTopLevelContainer.Controls.Add(pnlRadioButtonList);
                        rdoFavourites.Items.Add(ALLBOOKS);
                        rdoFavourites.Items.Add(FAVORITESONLY);
                        rdoFavourites.SelectedIndex = 0;
                        strTitle = "List of Books";
                        /// Add To Favorites
                        btnAddToFavorites = new Button();
                        btnAddToFavorites.ID = "btnAddToFavorites";
                        btnAddToFavorites.CssClass = "DWBbuttonAdvSrch";
                        btnAddToFavorites.Text = ADDTOFAVORITES;
                        btnAddToFavorites.OnClientClick = "javascript:return AddToFavorites();";
                        btnAddToFavorites.Click += new EventHandler(BtnAddToFavorites_Click);
                        this.Controls.Add(btnAddToFavorites);

                        hdnSelectedAsFavorite = new HiddenField();
                        hdnSelectedAsFavorite.ID = "hdnSelectedAsFavorite";
                        pnlTopLevelContainer.Controls.Add(hdnSelectedAsFavorite);

                        hdnUserFavorites = new HiddenField();
                        hdnUserFavorites.ID = "hdnUserFavorites";
                        pnlTopLevelContainer.Controls.Add(hdnUserFavorites);

                        break;
                    }
                case AUDITTRAIL:
                    {
                        pnlRadioButtonList.Visible = false;
                        strTitle = "Audit Trail - {0}";
                        break;
                    }
            }
            if (!Page.IsPostBack)
            {
                rblStatus.SelectedIndex = 0;
            }

            //pnlTopLevelContainer.Controls.Add(hdnReportType);
            this.Controls.Add(hdnReportType);
            SetListProperty();
            SetActiveStatus();

            this.Controls.Add(pnlTopLevelContainer);
        }

        void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        void btnFirePostBack_Click(object sender, EventArgs e)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Updates the list item status.
        /// </summary>
        /// <param name="listItemId">The list item id.</param>
        /// <param name="action">The action.</param>
        private void UpdateListItemStatus(int listItemId, string action)
        {
            try
            {
                switch (action)
                {
                    case STATUSTERMINATE:
                        TerminateListElement(ListName, listItemId, AuditListName);
                        break;
                    case STATUSACTIVATE:
                        ActivateList(ListName, listItemId, AuditListName);
                        break;
                    #region DREAM 4.0 - eWB 2.0 - Deletion
                    case STATUSREMOVE:
                        DeleteListItem(listItemId, CustomListType.ToString(), ListName, AuditListName);
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), webEx);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
        }

        /// <summary>
        /// Show or Hide the Buttons on the screen Based on ListType
        /// </summary>
        /// <param name="writer">HtmlTextWriter</param>
        /// <param name="noOfRecords">No of Records for the screen. Used to show/hide the Alter Page Sequence button</param>
        private void SetViewerButtonProperties(HtmlTextWriter writer, int noOfRecords)
        {
            switch (CustomListType.ToString())
            {
                case MASTERPAGEREPORT:
                    {
                        if (ActiveStatus)
                        {
                            btnAddMasterPage.RenderControl(writer);
                            writer.Write(SPACE3);
                            btnAlterPageSequence.RenderControl(writer);
                        }
                        break;
                    }
                case TEMPLATEREPORT:
                    {
                        if (ActiveStatus)
                        {
                            btnAddTemplate.RenderControl(writer);
                        }
                        break;
                    }
                case WELLBOOKREPORT:
                    {
                        object objStoredPrivilege = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPrivileges.ToString());
                        if (objStoredPrivilege != null)
                        {
                            Privileges objPrivileges = (Privileges)objStoredPrivilege;
                            if (objPrivileges != null)
                            {
                                if (objPrivileges.SystemPrivileges != null && (objPrivileges.SystemPrivileges.BookOwner || objPrivileges.SystemPrivileges.AdminPrivilege))
                                {
                                    if (ActiveStatus)
                                    {
                                        btnAddBook.RenderControl(writer);
                                    }
                                }
                            }
                        }

                        break;
                    }
                case CHAPTERREPORT:
                    {
                        if (ActiveStatus)
                        {
                            btnAddChapter.RenderControl(writer);
                            writer.Write(SPACE3);
                        }
                        if (noOfRecords >= 2)
                        {
                            btnAddChapterSuquence.RenderControl(writer);
                        }
                        break;
                    }
                case CHAPTERPAGEREPORT:
                    {
                        if (ActiveStatus)
                        {
                            btnChapterPage.RenderControl(writer);
                            writer.Write(SPACE3);
                        }
                        if (noOfRecords >= 2)
                        {
                            btnAlterChapterPageSequence.RenderControl(writer);
                        }
                        break;
                    }
                case TEMPLATEPAGESREPORT:
                    {
                        btnAddRemoveTemplatePages.RenderControl(writer);
                        writer.Write(SPACE3);
                        if (noOfRecords >= 2)
                        {
                            btnAlterPageSequence.RenderControl(writer);
                            writer.Write(SPACE3);
                        }
                        break;
                    }
                case USERREGISTRATION:
                    {
                        if (ActiveStatus)
                        {
                            btnAddNewUser.RenderControl(writer);
                            writer.Write(SPACE3);
                        }
                        break;
                    }
                case TEAMREGISTRATION:
                    {
                        object objStoredPrivilege = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPrivileges.ToString());
                        if (objStoredPrivilege != null)
                        {
                            Privileges objPrivileges = (Privileges)objStoredPrivilege;
                            if (objPrivileges != null)
                            {
                                if (objPrivileges.SystemPrivileges != null && (objPrivileges.SystemPrivileges.AdminPrivilege))
                                {

                                    if (ActiveStatus)
                                    {
                                        btnAddNewTeam.RenderControl(writer);
                                        writer.Write(SPACE3);
                                    }
                                }
                            }
                        }
                        break;
                    }
                case STAFFREGISTRATION:
                    {
                        btnAddRemoveStaff.RenderControl(writer);
                        writer.Write(SPACE3);
                        btnRankStaff.RenderControl(writer);
                        writer.Write(SPACE3);
                        break;
                    }
                case DWBHOME:
                    {
                        object objStoredPrivilege = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPrivileges.ToString());
                        if (objStoredPrivilege != null)
                        {
                            Privileges objPrivileges = (Privileges)objStoredPrivilege;
                            if (objPrivileges != null && objPrivileges.IsNonDWBUser)
                            {
                                pnlTopLevelContainer.Visible = false;
                                btnAddToFavorites.Visible = false;
                            }
                        }
                        else
                        {
                            pnlTopLevelContainer.Visible = false;
                            btnAddToFavorites.Visible = false;
                        }
                        pnlRadioButtonList.Controls.Remove(rblStatus);
                        break;
                    }
                case WELLBOOKPAGEVIEW:
                    {
                        //pnlTopLevelContainer.Controls.Remove(pnlRadioButtonList);                        
                        pnlRadioButtonList.Controls.Remove(rblStatus);
                        break;
                    }
            }

        }

        /// <summary>
        /// This method calls the "TransformListDetail" method to transform and XML using XSL
        /// </summary>
        /// <param name="dtRecords">DataTable</param>
        /// <param name="writer">HtmlTextWriter</param>
        private void GetDetailsFromList(DataTable dtRecords, HtmlTextWriter writer)
        {
            /// Sets the xmlListDocument(Global Variable) value
            GetListXml(dtRecords);
            if (xmlListDocument != null)
            {
                writer.Write("<tr><td colspan=\"3\" >");
                if (dtRecords != null)
                {
                    switch (CustomListType.ToString())
                    {
                        case AUDITTRAIL:
                            {
                                TransformListDetail(dtRecords.Rows.Count);
                                writer.Write(strResultTable.ToString());
                                break;
                            }
                        case TEMPLATEREPORT:
                        case TEMPLATEPAGESREPORT:
                        case MASTERPAGEREPORT:
                        case USERREGISTRATION:
                        case TEAMREGISTRATION:
                        case STAFFREGISTRATION:
                            {
                                TransformListDetail(dtRecords.Rows.Count);
                                writer.Write(strResultTable.ToString());
                                break;
                            }
                        case DWBHOME:
                            {
                                TransformListDetailDWBHome(dtRecords.Rows.Count);

                                break;
                            }
                        default:
                            {
                                TransformListDetail(dtRecords.Rows.Count);
                                writer.Write(strResultTable.ToString());
                                break;
                            }
                    }
                }
                writer.Write("</td></tr>");
            }
            else
            {
                if (string.Equals(CustomListType.ToString(), DWBHOME))
                {
                    StringBuilder strTable = new StringBuilder();
                    strTable.Append("<tr><td colspan=\"3\" ><div id=\"tableContainer\" class=\"tableContainer\" width=\"100%\">" +
                         "<table style=\"border-right:1px solid #336699;\"  cellpadding=\"4\" cellspacing=\"0\"" +
                          "class=\"scrollTable\" id=\"tblSearchResults\">" +
                          "<thead class=\"fixedHeader\" id=\"fixedHeader\"><tr style=\"height: 20px;\">" +
                          "<th style=\"width:60%\">Books</th><th style=\"width:28%\">Team</th><th style=\"width:2%\">Published<br/>Books</th>" +
                          "<th style=\"width:10%\"><br/>");
                    if (rdoFavourites != null)
                    {
                        if (rdoFavourites.SelectedIndex == 0)
                        {
                            strTable.Append("<input type=\"checkbox\" checked=\"checked\" name=\"chbHeader\" id=\"chbHeader\" onclick=\"javascript:SelectUnSelectAll(true);\"></input>");
                        }
                        else if (rdoFavourites.SelectedIndex == 1)
                        {
                            strTable.Append("<input type=\"checkbox\" name=\"chbHeader\" id=\"chbHeader\" onclick=\"javascript:SelectUnSelectAll(true);\"></input>");
                        }
                    }

                    strTable.Append("</th></tr></thead>");
                    strTable.Append("<tbody border =\"1\" class=\"scrollContent\">");

                    writer.Write(strTable.ToString());
                    writer.Write("<tr height=\"20px\" style=\"background-color:#E8E8E8;display:block\"><td colspan = \"4\">");
                    writer.Write(NODETAILMESSAGE);
                    writer.Write("</td></tr></tbody></table></div></td></tr>");
                }
                else
                {
                    writer.Write("<tr><td>");
                    writer.Write(NODETAILMESSAGE);
                    writer.Write("</td></tr>");
                }
            }
        }

        /// <summary>
        /// Set the list property.
        /// </summary>
        private void SetListProperty()
        {
            if (TerminateListItem == null)
            {
                string strListItemStatus =
                 HttpContext.Current.Request.QueryString[TERMINATESTATUS];

                if (!string.IsNullOrEmpty(strListItemStatus))
                {

                    bool.TryParse(strListItemStatus, out blnTerminatedStatus);
                    TerminateListItem = blnTerminatedStatus;
                    if (blnTerminatedStatus)
                    {
                        rblStatus.SelectedIndex = 1;
                    }
                }
            }
            switch (CustomListType.ToString())
            {
                case MASTERPAGEREPORT:
                    ListName = MASTERPAGELIST;
                    AuditListName = MASTERPAGEAUDITTRAIL;
                    ActiveStatus = !(Convert.ToBoolean(TerminateListItem));
                    base.ListReportName = MASTERPAGEREPORT;
                    break;
                case AUDITTRAIL:
                    ListName = HttpContext.Current.Request.QueryString[AUDITTRAIL];
                    //AuditListName = MASTERPAGEAUDITTRAIL;
                    //ActiveStatus = !(Convert.ToBoolean(TerminateListItem));
                    base.ListReportName = "Audit Trail";
                    break;
                case WELLBOOKREPORT:
                    ListName = WELLBOOKLIST;
                    AuditListName = WELLBOOKAUDITTRAIL;
                    ActiveStatus = !(Convert.ToBoolean(TerminateListItem));
                    base.ListReportName = WELLBOOKREPORT;
                    break;
                case CHAPTERPAGEMAPPINGREPORT:
                    ListName = CHAPTERPAGEMAPPINGLIST;
                    AuditListName = CHAPTERPAGEMAPPINGLISTAUDITTRAIL;
                    ActiveStatus = !(Convert.ToBoolean(TerminateListItem));
                    base.ListReportName = CHAPTERPAGEMAPPINGREPORT;
                    break;
                case CHAPTERREPORT:
                    ListName = CHAPTERLIST;
                    AuditListName = CHAPTERLISTAUDITTRAIL;
                    ActiveStatus = !(Convert.ToBoolean(TerminateListItem));
                    base.ListReportName = CHAPTERREPORT;
                    break;
                case CHAPTERPAGEREPORT:
                    ListName = CHAPTERPAGEMAPPINGLIST;
                    AuditListName = CHAPTERPAGEMAPPINGLISTAUDITTRAIL;
                    ActiveStatus = !(Convert.ToBoolean(TerminateListItem));
                    base.ListReportName = CHAPTERPAGEREPORT;
                    break;
                case TEMPLATEREPORT:
                    ListName = TEMPLATELIST;
                    AuditListName = TEMPLATEAUDITTRAIL;
                    ActiveStatus = !(Convert.ToBoolean(TerminateListItem));
                    base.ListReportName = TEMPLATEREPORT;
                    break;
                case TEMPLATEPAGESREPORT:
                    ListName = TEMPLATEPAGEMAPPINGLIST;
                    AuditListName = TEMPLATEPAGEMAPPINGAUDITTRIALLIST;
                    ActiveStatus = !(Convert.ToBoolean(TerminateListItem));
                    base.ListReportName = TEMPLATEPAGESREPORT;
                    break;
                case USERREGISTRATION:
                    ListName = USERLIST;
                    AuditListName = string.Empty;
                    ActiveStatus = !(Convert.ToBoolean(TerminateListItem));
                    base.ListReportName = USERREGISTRATION;
                    break;
                case TEAMREGISTRATION:
                    ListName = TEAMLIST;
                    AuditListName = string.Empty;
                    ActiveStatus = !(Convert.ToBoolean(TerminateListItem));
                    base.ListReportName = TEAMREGISTRATION;
                    break;
                case STAFFREGISTRATION:
                    ListName = TEAMSTAFFLIST;
                    AuditListName = string.Empty;
                    ActiveStatus = !(Convert.ToBoolean(TerminateListItem));
                    base.ListReportName = STAFFREGISTRATION;
                    break;
                case WELLBOOKPAGEVIEW:
                    ListName = CHAPTERPAGEMAPPINGLIST;
                    AuditListName = CHAPTERPAGEMAPPINGLISTAUDITTRAIL;
                    ActiveStatus = !(Convert.ToBoolean(TerminateListItem));
                    base.ListReportName = WELLBOOKPAGEVIEW;
                    break;
                case DWBHOME:
                    ListName = WELLBOOKLIST;
                    AuditListName = WELLBOOKAUDITTRAIL;
                    base.ListReportName = DWBHOME;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Sets the status of the list item i.e. active or terminated.
        /// In case user is navigated back to maintainence screen from a data entry
        /// then set it to what was set previously.
        /// </summary>
        private void SetActiveStatus()
        {
            String sEventArguments = this.Page.Request.Params[EVENTARG];
            String sEventTarget = this.Page.Request.Params[EVENTTARGET];
            if (HttpContext.Current.Request.QueryString[STATUS] != null)
            {
                /// If Query String "status" = "false" "Terminate" window will be displayed. This applicable in
                /// Paging and Sorting 
                if (string.Equals(HttpContext.Current.Request.QueryString[STATUS], FALSE))
                {
                    if (!string.IsNullOrEmpty(sEventTarget))
                    {
                        /// If Event Target ="Activate" set Active window else terminate window.
                        if (sEventTarget.Equals(STATUSACTIVATE))
                        {
                            ActiveStatus = false;
                            rblStatus.SelectedIndex = 1;
                        }
                        else if (sEventTarget.Equals(STATUSTERMINATE))
                        {
                            ActiveStatus = true;
                            rblStatus.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        ActiveStatus = false;
                        rblStatus.SelectedIndex = 1;
                    }
                }

                else
                {
                    if (!string.IsNullOrEmpty(sEventTarget))
                    {
                        /// If Event Target ="Activate" set Active window else terminate window.
                        if (sEventTarget.Equals(STATUSACTIVATE))
                        {
                            ActiveStatus = false;
                            rblStatus.SelectedIndex = 1;
                        }
                        else if (sEventTarget.Equals(STATUSTERMINATE))
                        {
                            ActiveStatus = true;
                            rblStatus.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        ActiveStatus = true;
                        rblStatus.SelectedIndex = 0;
                    }
                }
            }
            else if (!string.IsNullOrEmpty(sEventTarget))/// If Event Target ="Activate" set Active window else terminate window.
            {
                if (sEventTarget.Equals(STATUSACTIVATE))
                {
                    ActiveStatus = false;
                    rblStatus.SelectedIndex = 1;
                }

                else if (sEventTarget.Equals(STATUSTERMINATE))
                {
                    ActiveStatus = true;
                    rblStatus.SelectedIndex = 0;
                }
            } /// Else, QueryString "TerinateStatus" = "true" set Terminate window else Active window.
            else if (HttpContext.Current.Request.QueryString[TERMINATESTATUS] != null && string.Equals(HttpContext.Current.Request.QueryString[TERMINATESTATUS], TRUE))
            {

                ActiveStatus = false;
                rblStatus.SelectedIndex = 1;
            }
            else
            {
                ActiveStatus = true;
                rblStatus.SelectedIndex = 0;
            }

        }

        /// <summary>
        /// Creates Filter Panel
        /// Added By: Praveena 
        /// Date:09/09/2010
        /// Reason: For Module Simplify SignOff
        /// </summary>


        private void CreateFilterPanelControls()
        {
            pnlFilterOptions.Visible = true;
            pnlFilterOptions.HorizontalAlign = HorizontalAlign.Center;
            pnlFilterOptions.Direction = ContentDirection.LeftToRight;
            pnlFilterOptions.Wrap = false;
            pnlFilterOptions.GroupingText = "Filter Options";
            pnlFilterOptions.CssClass = "DWBHomeToplevelReportPanelCSS";

            lblPageName = new Label();
            lblPageName.ID = "lblPageNameID";
            lblPageName.CssClass = "DWItemText";
            lblPageName.Text = "Page Name";

            lblPageType = new Label();
            lblPageType.ID = "lblPageTypeID";
            lblPageName.CssClass = "DWItemText";
            lblPageType.Text = "Page Type";

            lblChapterName = new Label();
            lblChapterName.ID = "lblChapterNameID";
            lblChapterName.CssClass = "DWItemText";
            lblChapterName.Text = "Chapter Name";

            ////pnlFilterOptions.Controls.Add(lblPageName);
            cboPageName = new DropDownList();
            cboPageName.ID = "cboPageNameID";
            cboPageName.CssClass = "DWBdropdownAdvSrch";
            cboPageName.Width = Unit.Pixel(250);
            cboPageName.AutoPostBack = true;
            cboPageName.Attributes.Add("onChange", "javascript:return DisablePageType();");

            //to bind items to Page Name DropDown
            BindPageNameDropDown(cboPageName, CHAPTERLIST);
            cboPageName.Items.Insert(0, DROPDOWNDEFAULTTEXTALL);

            lstChapterNames = new ListBox();
            lstChapterNames.ID = "lstChapterNamesID";
            lstChapterNames.SelectionMode = ListSelectionMode.Multiple;
            lstChapterNames.Width = Unit.Pixel(200);
            BindChapterNameListBox(lstChapterNames, CHAPTERLIST);

            cblPageTypes = new CheckBoxList();
            cblPageTypes.ID = "cblPageTypeID";
            cblPageTypes.Items.Insert(0, "Type I");
            cblPageTypes.Items.Insert(1, "Type II");
            cblPageTypes.Items.Insert(2, "Type III");
            cblPageTypes.Items[0].Value = "1 - Automated";
            cblPageTypes.Items[1].Value = "2 - Published Document";
            cblPageTypes.Items[2].Value = "3 - User Defined Document";
            cblPageTypes.Items[0].Selected = false;
            cblPageTypes.Items[1].Selected = false;
            cblPageTypes.Items[2].Selected = false;


            btnFilter = new Button();
            btnFilter.ID = "btnFilterID";
            btnFilter.CssClass = "DWBbuttonAdvSrch";
            btnFilter.Text = "Filter";
            btnFilter.Click += new EventHandler(btnFilter_Click);

            btnReset = new Button();
            btnReset.ID = "btnResetID";
            btnReset.CssClass = "DWBbuttonAdvSrch";
            btnReset.Text = "Reset";
            btnReset.Click += new EventHandler(btnReset_Click);

            btnSignOff = new Button();
            btnSignOff.ID = "btnSignOffID";
            btnSignOff.CssClass = "DWBbuttonAdvSrch";
            btnSignOff.OnClientClick = "javascript:return BulkSignOff();";
            btnSignOff.Click += new EventHandler(btnSignOff_Click);

            hdnSignOffPageIds = new HiddenField();
            hdnSignOffPageIds.ID = "hdnSignOffPageId";
            pnlFilterOptions.Controls.Add(hdnSignOffPageIds);

        }

        private void CreateFilterPanel()
        {
            LiteralControl ltCtl = new LiteralControl("<table style='width:100%;'><tr><td class='DWItemText' style='width: 15%; height: 26px;' align='left'>");
            pnlFilterOptions.Controls.Add(ltCtl);
            pnlFilterOptions.Controls.Add(lblPageName);
            LiteralControl ltCtlPageNameDropdown = new LiteralControl("</td><td class='DWBtdAdvSrchItem' style='height: 26px; width: 20%;' align='left'>");
            pnlFilterOptions.Controls.Add(ltCtlPageNameDropdown);
            pnlFilterOptions.Controls.Add(cboPageName);
            LiteralControl ltCtlPageTypeLabel = new LiteralControl("</td><td class='DWItemText' style='width: 15%; height: 26px; vertical-align:text-top;' align='left'>");
            pnlFilterOptions.Controls.Add(ltCtlPageTypeLabel);
            pnlFilterOptions.Controls.Add(lblPageType);
            LiteralControl ltCtlPageTypeCheckBox = new LiteralControl("</td><td id =\"tdPageTypes\" class='DWBtdAdvSrchItem' style='height: 26px; width: 30%; vertical-align:text-top;' align='left'>");
            pnlFilterOptions.Controls.Add(ltCtlPageTypeCheckBox);
            pnlFilterOptions.Controls.Add(cblPageTypes);
            LiteralControl ltCtlChapterNameLabel = new LiteralControl("</td></tr><tr><td class='DWItemText' style='width: 15%; height: 26px; vertical-align:text-top;' align='right'>");
            pnlFilterOptions.Controls.Add(ltCtlChapterNameLabel);
            pnlFilterOptions.Controls.Add(lblChapterName);
            LiteralControl ltCtlChapterNameDropdown = new LiteralControl("</td><td class='DWBtdAdvSrchItem' style='height: 26px; width: 40%;' align='left'>");
            pnlFilterOptions.Controls.Add(ltCtlChapterNameDropdown);
            pnlFilterOptions.Controls.Add(lstChapterNames);
            LiteralControl ltCtlButtons = new LiteralControl("</td><td></td><td align='center' style='height: 28px'>");
            pnlFilterOptions.Controls.Add(ltCtlButtons);
            pnlFilterOptions.Controls.Add(btnFilter);
            LiteralControl ltCtlSpace = new LiteralControl("&nbsp;&nbsp;");
            pnlFilterOptions.Controls.Add(ltCtlSpace);
            pnlFilterOptions.Controls.Add(btnReset);
            LiteralControl ltCtl1 = new LiteralControl("</td></tr></table>");
            pnlFilterOptions.Controls.Add(ltCtl1);
        }

        private void SetFilterProperties()
        {
            //fill filter properties
            strPageName = cboPageName.SelectedItem.Text;
            StringBuilder strSelectedChapters = new StringBuilder();
            for (int intRowIndex = 0; intRowIndex < lstChapterNames.Items.Count; intRowIndex++)
            {
                if (lstChapterNames.Items[intRowIndex].Selected == true)
                {
                    if (!string.IsNullOrEmpty(strSelectedChapters.ToString()))
                    {
                        strSelectedChapters.Append(",");
                    }

                    strSelectedChapters.Append("'" + lstChapterNames.Items[intRowIndex].Text + "'");

                }
            }
            strChapterNames = strSelectedChapters.ToString();
            StringBuilder strSelectedPageTypes = new StringBuilder();
            if (cboPageName.SelectedIndex == 0)
            {
                for (int intRowIndex = 0; intRowIndex < cblPageTypes.Items.Count; intRowIndex++)
                {
                    if (cblPageTypes.Items[intRowIndex].Selected == true)
                    {
                        if (!string.IsNullOrEmpty(strSelectedPageTypes.ToString()))
                        {
                            strSelectedPageTypes.Append(",");
                        }

                        strSelectedPageTypes.Append("'" + cblPageTypes.Items[intRowIndex].Value + "'");
                    }
                }
            }
            strPageTypes = strSelectedPageTypes.ToString();
        }

        private DataView SetSortOrder(DataView dvResultDataView)
        {
            /** code for applying sort*/
            if (this.Page.Request[EVENTTARGET] != null)
            {
                #region Sort by using explicit postback
                String strMixedValues = this.Page.Request[EVENTTARGET];
                String strArg = this.Page.Request[EVENTARG];
                if (strMixedValues.Contains(";"))
                {
                    if (!string.IsNullOrEmpty(strMixedValues))
                    {
                        char[] chSplitterComma = { ';' };
                        string[] strSortValues = strMixedValues.Split(chSplitterComma);
                        if (strSortValues != null && strSortValues.Length > 0)
                        {
                            SortBy = strSortValues[3]; //Column Name
                            SortType = strSortValues[5];//Sort Order
                            PageNumber = strSortValues[1];

                            SortType = string.Equals(SortType, "ascending") ? "ASC" : "DESC";
                        }
                        switch (SortBy)
                        {
                            case "Page Title":
                                {
                                    dvResultDataView.Sort = "Page_Name " + SortType;
                                    break;
                                }
                            case "Chapter Name":
                                {
                                    dvResultDataView.Sort = "ChapterTitle " + SortType;
                                    break;
                                }
                            case "Type":
                                {
                                    dvResultDataView.Sort = "Connection_Type " + SortType;
                                    break;
                                }
                            case "Discipline":
                                {
                                    dvResultDataView.Sort = "Discipline " + SortType;
                                    break;
                                }
                            case "Owner":
                                {
                                    dvResultDataView.Sort = "Owner " + SortType;
                                    break;
                                }
                        }

                    }
                }
                #endregion Sort by using explicit postback
            }

            return dvResultDataView;
        }

        #region DREAM 4.0 - eWB 2.0 - Deletion
        /// <summary>
        /// Gets the archive options div.
        /// </summary>
        /// <returns></returns>
        private string GetArchiveOptionsDiv()
        {

            StringBuilder sbExportOptionsDivHTML = new StringBuilder();

            sbExportOptionsDivHTML.Append("<table cellpadding=\"0\" cellspacing=\"0\">");
            sbExportOptionsDivHTML.Append("<tr>");
            sbExportOptionsDivHTML.Append("<td>");
            sbExportOptionsDivHTML.Append("<div class=\"DeleteOptions\" id=\"diveWBArchiveOptions\">");
            sbExportOptionsDivHTML.Append("<table width=\"100%\">");
            sbExportOptionsDivHTML.Append("<tr>");
            sbExportOptionsDivHTML.Append("<td class=\"DeleteDivHeader\" colspan=\"2\" style=\"width: 100%\">"); //colspan=\"1\
            sbExportOptionsDivHTML.Append("<b>Confirmation</b></td>");
            sbExportOptionsDivHTML.Append(" </tr>");
            sbExportOptionsDivHTML.Append("<tr>");
            sbExportOptionsDivHTML.Append("<td style=\"padding: 5px 5px 5px 5px\">");
           

            if (rblStatus.SelectedIndex == 0)
            {
                sbExportOptionsDivHTML.Append("<input type=\"radio\" name=\"rdblArchiveOptions\" id=\"rdblArchiveForFuture\" checked=\"checked\" value=\"Archive for future use\" title=\"Archive for future use\" /><label>Archive for future use</label>");
            }
            else if (rblStatus.SelectedIndex == 1)
            {
                sbExportOptionsDivHTML.Append("<input type=\"radio\" name=\"rdblArchiveOptions\" id=\"rdblActivateRecord\" checked=\"checked\" value=\"Activate\" title=\"Activate\" /><label>Activate</label>");
            }
           
            sbExportOptionsDivHTML.Append("<input type=\"radio\" name=\"rdblArchiveOptions\" id=\"rdblPermanentRemove\" value=\"Permanently delete\" title=\"Permanently delete\" />");
            sbExportOptionsDivHTML.Append("<label>Permanently delete</label>");
          
            sbExportOptionsDivHTML.Append("</td>");

            sbExportOptionsDivHTML.Append("</tr>");
            sbExportOptionsDivHTML.Append("<tr>");

            sbExportOptionsDivHTML.Append("<td  align=\"center\" colspan=\"2\">"); //colspan=\"1\"
            sbExportOptionsDivHTML.Append("<input class=\"buttonAdvSrch\" type=\"button\" value=\"OK\" id=\"btneWBArchiveOK\"  />"); //onclick=\"return hide('diveWBArchiveOptions');\"
            sbExportOptionsDivHTML.Append("&nbsp;&nbsp;&nbsp;<input class=\"buttonAdvSrch\" type=\"button\" value=\"Cancel\" id=\"btnSavenShow\" onclick=\"return hide('diveWBArchiveOptions')\" />");
            sbExportOptionsDivHTML.Append("</td>");

            sbExportOptionsDivHTML.Append("</tr>");
            sbExportOptionsDivHTML.Append("</table>");
            sbExportOptionsDivHTML.Append("</div>");
            sbExportOptionsDivHTML.Append("</td>");
            sbExportOptionsDivHTML.Append(" </tr>");
            sbExportOptionsDivHTML.Append("</table>");

            return sbExportOptionsDivHTML.ToString();
        }
        #endregion
        #endregion Private Methods
    }
}

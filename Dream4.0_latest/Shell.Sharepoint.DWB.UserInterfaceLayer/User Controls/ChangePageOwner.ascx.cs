#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: ChangePageOwner.cs
#endregion

using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DREAM.CustomDataGrid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Net;
using System.Web;
using System.Web.UI.WebControls;
using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DWB.BusinessLogicLayer;

namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// This class is used to change the page owner
    /// </summary>
    public partial class ChangePageOwner : UIHelper
    {

        #region DECLARATION

        const string BORDERCOLOR = "bordercolor";
        const string BORDERCOLORVALUE = "#9b9797";//modified for rebranding
        const string FIRST = "First";
        const string LAST = "Last";
        const string OVERFLOW = "overflow:auto;";
        const string CHANGEMASTERPAGE = "change";
        const string PAGETITLEEXISTSMSG = "Book Title already exist. Please enter a different title.";
        const string MAINTAINWELLBOOKURL = "BookMaintenance.aspx";
        const string PAGESELECTIONVISIBLE = "PageSelectionVisible";
        const string PAGEOWNERGRIDVIEWID = "PageOwnerGridViewID";
        const string WIDTH = "width";
        const string MAPGRIDROW = "MapGridRow";
        const string SEARCHRESULTFIXEDHEADER = "SearchResultFixedHeader";
        const string WARNING = "The Well book does not have a chapter.";
        const string COLOR = "#BDBDBD";//modified for rebranding
        const string EVENROWSTYLE = "evenRowStyle";
        const string ODDROWSTYLE = "oddRowStyle";
        const string NONE = "--None--";
        const string TYPECOLUMNHEADER = "Type";
        const string PAGETITLECOLUMNHEADER = "Page Title";
        const string GRIDVIEWWIDTH = "98%";
        const string TERMINATED = "Yes";
        const string NOTTERMINATED = "No";
        ListEntry objListEntry;
        CustomGridView pageOwnerGridview;
        WellBookBLL objWellBook;
        string strSelectedID = string.Empty;
        string strTerminatedStatus = "No";
        Label lblNoDetails;

        /// <summary>
        /// Gets or sets a value indicating whether [page selection visible].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [page selection visible]; otherwise, <c>false</c>.
        /// </value>
        public bool PageSelectionVisible
        {
            get
            {
                if (ViewState[PAGESELECTIONVISIBLE] == null)
                {
                    return false;
                }
                else
                {
                    return Convert.ToBoolean(ViewState[PAGESELECTIONVISIBLE]);
                }
            }
            set
            {
                ViewState[PAGESELECTIONVISIBLE] = value;
            }
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            strSelectedID = HttpContext.Current.Request.QueryString[IDVALUEQUERYSTRING];

            try
            {
                if (!IsPostBack)
                {
                    /// Set Book Title.
                    SetBookTitle();
                    /// Load the controls with data.
                    LoadControlsOnPageLoad();
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
        /// Raises the <see cref="E:System.Web.UI.Control.Init"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            /// Assign the Grid View properties
            pageOwnerGridview = new CustomGridView();
            pageOwnerGridview.ID = PAGEOWNERGRIDVIEWID;
            pageOwnerGridview.Attributes.Add(BORDERCOLOR, BORDERCOLORVALUE);
            pageOwnerGridview.Attributes.Add(WIDTH, GRIDVIEWWIDTH);
            pageOwnerGridview.Attributes.Add(STYLE, OVERFLOW);
            pageOwnerGridview.EnableViewState = true;


            pageOwnerGridview.AutoGenerateCheckBoxColumn = true;
            pageOwnerGridview.RowStyle.CssClass = MAPGRIDROW;
            ///<TODO>
            /// Javascript error on page after selecting page owner is caused by this style class.
            /// It tries to access the table "tableContainer" property but the object is null, no null check done.
            /// Check with Manoj about the significance of this style and fix the issue
            /// </TODO>
           /// pageOwnerGridview.HeaderStyle.CssClass = SEARCHRESULTFIXEDHEADER;
            pageOwnerGridview.HeaderStyle.Height = new Unit(20, UnitType.Pixel);
            pageOwnerGridview.HeaderStyle.BackColor = ColorTranslator.FromHtml(COLOR);
            pageOwnerGridview.RowStyle.CssClass = EVENROWSTYLE;
            pageOwnerGridview.AlternatingRowStyle.CssClass = ODDROWSTYLE;
            pageOwnerGridview.RowDataBound += new GridViewRowEventHandler(PageOwnerGridview_RowDataBound);
            lblNoDetails = new Label();
            lblNoDetails.ID = "lblNoDetailsID";
            lblNoDetails.CssClass = "DWItemText";
            lblNoDetails.Text = "There are currently no details to be displayed.";
            lblNoDetails.Visible = false;
            PageOwnerGridViewPanelID.Controls.Add(pageOwnerGridview);
            PageOwnerGridViewPanelID.Controls.Add(lblNoDetails);

            /// Added by:Praveena to Set Selected Index Changed event to RadioButton list
            rblStatus.SelectedIndexChanged += new EventHandler(rblStatus_SelectedIndexChanged);


            PageOwnerGridViewPanelID.Visible = false;
            /// Save button client click registration for validation
            if (btnSave != null)
            {
                btnSave.OnClientClick = "return IsPageSelected('" + pageOwnerGridview.ID + "');";
            }
            /// SaveAndChangeMore button client click registration for validation
            if (btnSaveAndChangeMore != null)
            {
                btnSaveAndChangeMore.OnClientClick = "return IsPageSelected('" + pageOwnerGridview.ID + "');";
            }
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            /// If owner is selected PageSelectionVisible = true, then only show the buttons.
            if (!PageSelectionVisible)
            {
                PageOwnerGridViewPanelID.Visible = false;
                cboNewOwner.Visible = false;
                btnCancel.Visible = false;
                btnSaveAndChangeMore.Visible = false;
                btnSave.Visible = false;
                lblNewOwner.Visible = false;
            }
        }
        #endregion Overridden Methods

        #region EventHandler Methods
        /// <summary>
        /// Event handler triggered for each row bounded in the grid view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PageOwnerGridview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[2].Text = TYPECOLUMNHEADER;
                e.Row.Cells[3].Text = PAGETITLECOLUMNHEADER;
                e.Row.Cells[7].Text = SIGNEDOFFCOLUMN;
            }

            /// Hide the ID column from the generated columns 
            e.Row.Cells[8].Visible = false; //Modified by:Praveena 
        }

        /// <summary>
        /// Saves the changes and redirects to the user maintanence screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                /// Set the selected data to data objects.
                SetListEntry();
                /// Update the changes to SharePoint list.
                UpdatePageOwner(objListEntry, CHAPTERPAGESMAPPINGLIST, CHAPTERPAGESMAPPINGAUDITLIST, AUDITACTIONUPDATION, cboNewOwner.SelectedItem.Text);
                /// Hide the Save,Save and Change more buttons
                PageSelectionVisible = false;
                /// Hide the GridView
                PageOwnerGridViewPanelID.Visible = false;
                /// Hide the new owner dropdown
                cboNewOwner.Visible = false;
                /// Hide the Save,Save and Change more buttons
                btnCancel.Visible = false;
                btnSaveAndChangeMore.Visible = false;
                btnSave.Visible = false;
                lblNewOwner.Visible = false;
                /// Redirect to Maintain Books page.
                Page.Response.Redirect(MAINTAINWELLBOOKURL, false);
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
        /// Cancels the user actions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(MAINTAINWELLBOOKURL, false);
        }

        /// <summary>
        /// Event handler if user changes once and wants to change more
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSaveAndChangeMore_Click(object sender, EventArgs e)
        {
            try
            {
                /// Set the selected data to data objects.
                SetListEntry();
                /// Update the changes to SharePoint list.
                UpdatePageOwner(objListEntry, CHAPTERPAGESMAPPINGLIST, CHAPTERPAGESMAPPINGAUDITLIST, AUDITACTIONUPDATION, cboNewOwner.SelectedItem.Text);
                /// Load the controls.
                LoadControlsOnPageLoad();
                /// Hide the Save,Save and Change more buttons
                PageSelectionVisible = false;
                /// Hide the GridView
                PageOwnerGridViewPanelID.Visible = false;
                /// Hide the new owner dropdown
                cboNewOwner.Visible = false;
                /// Hide the Save,Save and Change more buttons
                btnCancel.Visible = false;
                btnSaveAndChangeMore.Visible = false;
                btnSave.Visible = false;
                lblNewOwner.Visible = false;

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
        /// Will Filter the pages according to the filters applied
        /// Added By: Praveena  
        /// Date:04/09/2010
        /// Reason: For module Add additional attributes to change page owner table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                //setting the strTerminatedStatus value according to value selected in RadioButton List
                if (rblStatus.SelectedIndex == 0)
                {
                    strTerminatedStatus = NOTTERMINATED;
                }
                else
                {
                    strTerminatedStatus = TERMINATED;
                }

                //Adding all the selected Chapters in ListBox to string builder.
                System.Text.StringBuilder strSelectedChapters = new System.Text.StringBuilder();

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
                //passing the values selected in controls to filter the datatable based on them
                GetOwnerPageDetails(cboOwner.SelectedItem.Text, cboPageTitle.SelectedItem.Text, strTerminatedStatus, strSelectedChapters);
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
        /// Will Reset the filter options values to default
        /// Added By: Praveena  
        /// Date:08/09/2010
        /// Reason: For module Add additional attributes to change page owner table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnReset_Click(object sender, EventArgs e)
        {
            ResetFilterValues();
        }

        /// <summary>
        /// Populates the pages for the selected page owner.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CboOwner_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                /// If a owner is selected, show the grid view and buttons
                if (cboOwner.SelectedIndex > 0)
                {
                    PageOwnerGridViewPanelID.Visible = true;
                    cboNewOwner.Visible = true;
                    btnCancel.Visible = true;
                    btnSaveAndChangeMore.Visible = true;
                    btnSave.Visible = true;
                    PageSelectionVisible = true;
                    lblNewOwner.Visible = true;

                    //added by Praveena for module Add additional attributes to change page owner table
                    cboPageTitle.Enabled = true;
                    lstChapterNames.Enabled = true;
                    if (rblStatus.SelectedIndex == 0)
                    {
                        strTerminatedStatus = NOTTERMINATED;
                    }
                    else
                    {
                        strTerminatedStatus = TERMINATED;
                    }
                    System.Text.StringBuilder strSelectedChapters = new System.Text.StringBuilder();
                    cboPageTitle.SelectedIndex = 0;
                    lstChapterNames.SelectedIndex = -1;

                    GetOwnerPageDetails(cboOwner.SelectedItem.Text, cboPageTitle.SelectedItem.Text, strTerminatedStatus, strSelectedChapters);
                }
                else /// Reset the page to original state.
                {
                    PageSelectionVisible = false;
                    PageOwnerGridViewPanelID.Visible = false;
                    cboNewOwner.Visible = false;
                    btnCancel.Visible = false;
                    btnSaveAndChangeMore.Visible = false;
                    btnSave.Visible = false;
                    lblNewOwner.Visible = false;
                    //added by Praveena for module Add additional attributes to change page owner table
                    cboPageTitle.Enabled = false;
                    lstChapterNames.Enabled = false;
                    cboPageTitle.SelectedIndex = 0;
                    lstChapterNames.SelectedIndex = -1;

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
        /// Populate the pages according to status selected
        /// Added By: Praveena  
        /// Date:08/09/2010
        /// Reason: For module Add additional attributes to change page owner table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetFilterValues();
        }

        #endregion EventHandler Methods

        #region Private Methods
        /// <summary>
        /// Sets the list entry Data object.
        /// </summary>
        private void SetListEntry()
        {
            try
            {
                objListEntry = new ListEntry();
                int intSelectedRowId = 0;
                int[] selectedIndices = pageOwnerGridview.GetSelectedIndices();
                List<ChapterPagesMapping> lstChapterPagemapping = new List<ChapterPagesMapping>();
                ChapterPagesMapping objChapterPagemapping = null;
                GridViewRow gridviewRow = null;
                if (selectedIndices != null)
                {
                    for (int index = 0; index < selectedIndices.Length; index++)
                    {
                        gridviewRow = null;
                        gridviewRow = pageOwnerGridview.Rows[selectedIndices[index]];
                        objChapterPagemapping = new ChapterPagesMapping();
                        int.TryParse(gridviewRow.Cells[8].Text, out intSelectedRowId);
                        if (intSelectedRowId > 0)
                        {
                            objChapterPagemapping.RowId = intSelectedRowId;
                        }
                        lstChapterPagemapping.Add(objChapterPagemapping);
                    }
                }
                objListEntry.ChapterPagesMapping = lstChapterPagemapping;
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
        /// Sets the book title.
        /// </summary>
        private void SetBookTitle()
        {
            try
            {
                string strCamlQuery = @"<Where><Eq><FieldRef Name='ID' />
                 <Value Type='Counter'>" + strSelectedID + "</Value></Eq></Where>";
                string strViewFields = @"<FieldRef Name='ID' /><FieldRef Name='Title' />";

                CommonBLL objCommon = new CommonBLL();
                DataTable dtBook = objCommon.ReadList(strParentSiteURL, DWBBOOKLIST, strCamlQuery, strViewFields);
                if (dtBook != null && dtBook.Rows.Count > 0)
                {
                    lblBookTitle.Text = Convert.ToString(dtBook.Rows[0][DWBTITLECOLUMN]);
                    dtBook.Dispose();
                }
            }
            catch (Exception ex)
            {

                CommonUtility.HandleException(strParentSiteURL, ex);

            }
        }

        /// <summary>
        /// Loads the controls on page load.
        /// </summary>
        private void LoadControlsOnPageLoad()
        {
            try
            {
                string strCamlQuery = @"<Where><Eq><FieldRef Name='Book_ID' />
                 <Value Type='Number'>" + strSelectedID + "</Value></Eq></Where>";

                DataTable dtresultTable = GetListItems(DWBCHAPTERLIST, strCamlQuery, string.Empty);
                if (dtresultTable == null || dtresultTable.Rows.Count == 0)
                {
                    lblException.Text = WARNING;
                    lblException.Visible = true;
                    ExceptionBlock.Visible = true;
                    lblSelectCurrentOwner.Visible = false;
                    cboOwner.Visible = false;
                    return;
                }

                BindDataToControls(cboOwner, DWBCHAPTERLIST, CHANGEPAGEOWNER, strCamlQuery);
                cboOwner.Items.Insert(0, DROPDOWNDEFAULTTEXT);

                //added by Praveena for module Add additional attributes to change page owner table
                BindPageNames(cboPageTitle, DWBCHAPTERLIST, strCamlQuery);
                BindChapterNameListBox(lstChapterNames, DWBCHAPTERLIST, strCamlQuery);
                cboPageTitle.Items.Insert(0, DROPDOWNDEFAULTTEXTALL);
                cboPageTitle.Enabled = false;
                lstChapterNames.Enabled = false;


                if (cboOwner.Items.Count == 1)
                {
                    cboOwner.Items.Insert(1, NONE);
                }
                if (dtresultTable != null)
                {
                    dtresultTable.Dispose();
                }
            }
            catch (Exception ex)
            {

                CommonUtility.HandleException(strParentSiteURL, ex);

            }
        }

        /// <summary>
        /// Gets the list of page owners.
        /// Modified By: Praveena  
        /// Date:03/09/2010
        /// Reason: For module Add additional attributes to change page owner table
        /// </summary>
        /// <param name="username">User Name.</param>
        void GetOwnerPageDetails(string username, string pageName, string terminatedStatus, System.Text.StringBuilder chapterNames)
        {
            try
            {
                DataTable dtResulttable = null;
                DataTable dtTabletoBind = null;
                PageOwnerGridViewPanelID.Visible = true;
                lblNoDetails.Visible = false;
                string strCamlQuery = @"<Where><Eq><FieldRef Name='Book_ID' />
                 <Value Type='Number'>" + strSelectedID + "</Value></Eq></Where>";
                objWellBook = new WellBookBLL();
                dtResulttable = objWellBook.GetPagesForOwner(strParentSiteURL, DWBCHAPTERLIST, strCamlQuery, username, pageName, terminatedStatus, chapterNames);
                if (dtResulttable != null && dtResulttable.Rows.Count > 0)
                {
                    dtTabletoBind = dtResulttable.DefaultView.ToTable("BindTable", true, "ChapterName", CONNECTIONTYPECOLUMN, PAGENAMECOLUMN, DISCIPLINECOLUMN, OWNERCOLUMN,
                         EMPTYCOLUMN, SIGNOFFSTATUSCOLUMN, DWBIDCOLUMN);
                    pageOwnerGridview.DataSource = dtTabletoBind;
                    pageOwnerGridview.DataBind();
                    if (dtTabletoBind.Rows.Count == 0)
                    {
                        lblNoDetails.Visible = true;
                        PageSelectionVisible = false;
                        cboNewOwner.Visible = false;
                        btnCancel.Visible = false;
                        btnSaveAndChangeMore.Visible = false;
                        btnSave.Visible = false;
                        lblNewOwner.Visible = false;
                    }
                    else
                    {
                        lblNoDetails.Visible = false;
                        PageSelectionVisible = true;
                        cboNewOwner.Visible = true;
                        btnCancel.Visible = true;
                        btnSaveAndChangeMore.Visible = true;
                        btnSave.Visible = true;
                        lblNewOwner.Visible = true;

                    }
                }
                GetTeamUsers();
                if (dtResulttable != null)
                {
                    dtResulttable.Dispose();
                }
                if (dtTabletoBind != null)
                {
                    dtTabletoBind.Dispose();
                }
                /// Calling Javascript method DWBFixCheckBoxAlignment(gridID) to set the CheckBox column alignment as "center"
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "FixGridWidth", "DWBFixCheckBoxAlignment('" + pageOwnerGridview.ClientID + "');", true);
            }
            catch (Exception ex)
            {

                CommonUtility.HandleException(strParentSiteURL, ex);

            }

        }

        /// <summary>
        /// Gets the teams for the user.
        /// </summary>
        private void GetTeamUsers()
        {
            try
            {
                string strCamlQuery = @"<Where><Eq><FieldRef Name='ID' />
                 <Value Type='Counter'>" + strSelectedID + "</Value></Eq></Where>";
                base.GetTeamUsers(cboNewOwner, DWBBOOKLIST, strCamlQuery);
            }
            catch (Exception ex)
            {

                CommonUtility.HandleException(strParentSiteURL, ex);

            }
        }

        /// <summary>
        /// This method will set the filter options values to default values
        /// Added By: Praveena  
        /// Date:09/09/2010
        /// Reason: For module Add additional attributes to change page owner table
        /// </summary>
        private void ResetFilterValues()
        {
            try
            {
                if (rblStatus.SelectedIndex == 0)
                {
                    strTerminatedStatus = NOTTERMINATED;
                }
                else
                {
                    strTerminatedStatus = TERMINATED;
                }
                System.Text.StringBuilder strSelectedChapters = new System.Text.StringBuilder();
                cboPageTitle.SelectedIndex = 0;
                lstChapterNames.SelectedIndex = -1;
                GetOwnerPageDetails(cboOwner.SelectedItem.Text, cboPageTitle.SelectedItem.Text, strTerminatedStatus, strSelectedChapters);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strParentSiteURL, ex);
            }

        }

        #endregion Private Methods
    }
}
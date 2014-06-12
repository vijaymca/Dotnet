#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: WellBook.cs
#endregion

using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DREAM.Utilities;
using System.Net;

namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// Class is used to add and edit well book.
    /// </summary>
    public partial class WellBook : UIHelper
    {
        #region DECLARATION
     
        const string CHANGEMASTERPAGE = "change";
        const string BOOKTITLEEXISTSMSG = "This book title already exists. Please enter a different title.";
        const string MAINTAINWELLBOOKURL = "BookMaintenance.aspx";
        const string EDITBOOK = "Edit Book";
        
        ListEntry objListEntry ;
        string strSelectedID = string.Empty;
        string strMode = string.Empty;
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
            ListEntry objWellBookData = null;
            strMode = HttpContext.Current.Request.QueryString[MODEQUERYSTRING];;
            strSelectedID = HttpContext.Current.Request.QueryString[IDVALUEQUERYSTRING];
            try
            {
                if (!IsPostBack)
                {
                    LoadControlsOnPageLoad();
                    if (string.Equals(strMode, EDIT))
                    {
                        objWellBookData = GetDetailsForSelectedID(strSelectedID, DWBBOOKLIST, WELLBOOK);
                        if (objWellBookData != null)
                        {
                            BindUIControls(objWellBookData);
                        }
                    }

                    Page.ClientScript.RegisterStartupScript(this.Page.GetType(), SETWINDOWTITLEJSKEY, string.Format(SETWINDOWTITLEVALUE,lblWellBookHeading.Text), true);
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
        /// Handler triggered when user cancels his action.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            if (string.Compare(hdnListitemStatus.Value, STATUSTERMINATED, true) == 0)
            {
                Page.Response.Redirect(MAINTAINWELLBOOKURL + TERMINATESTATUSQUERYSTRING, false);
            }
            else
            {
                Page.Response.Redirect(MAINTAINWELLBOOKURL, false);
            }

        }

        /// <summary>
        /// Event handler for the event when user click ok  during adding new well
        /// book or editing the well book.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdOK_Click(object sender, EventArgs e)
        {
            string strActionPerformed = string.Empty;
            bool blnBookNameExists = true;

            try
            {
                /// Check book with same title exists already or not
                if (string.Equals(strMode, ADD))
                {
                    strActionPerformed = AUDITACTIONCREATION;
                    blnBookNameExists = CheckDuplicateName(txtWellBookTitle.Text.Trim(), DWBTITLECOLUMN, DWBBOOKLIST);
                }
                else if (string.Equals(strMode, EDIT))
                {
                    strActionPerformed = AUDITACTIONUPDATION;
                    blnBookNameExists = CheckDuplicateName(txtWellBookTitle.Text.Trim(), DWBTITLECOLUMN, DWBBOOKLIST, strSelectedID);
                }

                if (!blnBookNameExists)
                {
                    SetListEntry();
                    UpdateListEntry(objListEntry, DWBBOOKLIST, WELLBOOKAUDITLIST, WELLBOOK,
                        strActionPerformed);
                    if (string.Compare(hdnListitemStatus.Value, STATUSTERMINATED, true) == 0)
                    {
                        Page.Response.Redirect(MAINTAINWELLBOOKURL + TERMINATESTATUSQUERYSTRING, false);
                    }
                    else
                    {
                        Page.Response.Redirect(MAINTAINWELLBOOKURL, false);
                    }
                }
                else
                {
                    lblException.Text = BOOKTITLEEXISTSMSG;
                    lblException.Visible = true;
                    ExceptionBlock.Visible = true;
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
        /// Gets the team owner for a selected team.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cboTeam_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                cboOwner.Items.Clear();
                string camlQuery = @"<Where><Eq><FieldRef Name='Team_ID' />
                    <Value Type='Counter'>" + cboTeam.SelectedItem.Value + "</Value></Eq></Where><OrderBy><FieldRef Name='Title' Ascending='True'/></OrderBy>";
                BindDataToControls(cboOwner, TEAMSTAFFLIST, WELLBOOK, camlQuery);
                cboOwner.Items.Insert(0, DROPDOWNDEFAULTTEXTNONE);
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

        #endregion

        #region Private Methods
        /// <summary>
        /// Binds the UI controls.
        /// </summary>
        /// <param name="objMasterData">The WellBookData object.</param>
        private void BindUIControls(ListEntry objWellBookData)
        {
            txtWellBookTitle.Text = objWellBookData.WellBookDetails.Title;
            cboTeam.ClearSelection();

            BindDropDownList(cboTeam, objWellBookData.WellBookDetails.TeamID);
            cboTeam.Enabled = false;
            if (string.Equals(strMode, EDIT))
            {
                if (!cboTeam.SelectedItem.Value.Contains(DROPDOWNDEFAULTTEXTNONE))
                {
                    string strCAMLQuery = @"<Where><Eq><FieldRef Name='Team_ID' />
                    <Value Type='Number'>" + cboTeam.SelectedItem.Value + "</Value></Eq></Where><OrderBy><FieldRef Name='Title' Ascending='True'/></OrderBy>";
                    BindDataToControls(cboOwner, TEAMSTAFFLIST, WELLBOOK, strCAMLQuery);
                    cboOwner.Items.Insert(0,DROPDOWNDEFAULTTEXTNONE);
                    BindDropDownList(cboOwner, objWellBookData.WellBookDetails.BookOwnerID);
                }
                lblWellBookHeading.Text = lblWellBookHeading.Text + objWellBookData.WellBookDetails.Title;
            }
            hdnListitemStatus.Value = objWellBookData.WellBookDetails.Terminated;
        }

        /// <summary>
        /// Binds the drop down list.
        /// </summary>
        /// <param name="dropDown">The drop down.</param>
        /// <param name="value">The value.</param>
        private void BindDropDownList(DropDownList ddlControl, string value)
        {           
            if (ddlControl != null && ddlControl.Items.FindByValue(value) != null)
            {
                ddlControl.Items.FindByValue(value).Selected = true;
            }
        }

        /// <summary>
        /// Loads the controls on page load.
        /// </summary>
        private void LoadControlsOnPageLoad()
        {
            string strfieldsToView = "<FieldRef Name='Title' /><FieldRef Name='ID' /><FieldRef Name='Terminate_Status' />";
            string strCAMLQuery = string.Empty;          
            /// If user is AD - Display all teams
            /// If user is BO - Display only teams which user is member       
            object objPrivileges = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPrivileges.ToString());
            Privileges objStoredPrivileges = null;
            if (objPrivileges != null)
            {
                objStoredPrivileges = (Privileges)objPrivileges;
            }
            if (objStoredPrivileges != null && objStoredPrivileges.SystemPrivileges != null)
            {
                if (objStoredPrivileges.SystemPrivileges.AdminPrivilege)
                {
                    strCAMLQuery = @"<Where><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq></Where>";
                }
                else if (objStoredPrivileges.SystemPrivileges.BookOwner)
                {
                    strCAMLQuery = GetCAMLQueryForBOUsers(strParentSiteURL, DWBIDCOLUMN, IDCOLUMNTYPE, STATUSACTIVE,false);
                }
            }
            DataTable dtResultTable = GetListItems(TEAMLIST, strCAMLQuery, strfieldsToView);
            if (dtResultTable != null && dtResultTable.Rows.Count > 0)
            {
                cboTeam.DataSource = dtResultTable;
                cboTeam.DataTextField = DWBTITLECOLUMN;
                cboTeam.DataValueField = DWBIDCOLUMN;
                cboTeam.DataBind();
                cboTeam.Items.Insert(0, DROPDOWNDEFAULTTEXTNONE);
            }
            cboOwner.Items.Add(DROPDOWNDEFAULTTEXTNONE);
            SetUIControlForMode();
            if (dtResultTable != null)
                dtResultTable.Dispose();
        }
        
        /// <summary>
        /// Hides controls based on the mode.
        /// </summary>
        private void SetUIControlForMode()
        {
            switch (strMode)
            {
                case EDIT:
                    {
                        lblWellBookHeading.Text = EDITBOOK +" : ";
                        this.Page.Title = EDITBOOK;
                        break;
                    }
                default :
                    break;
            }
        }             

        /// <summary>
        /// Sets the list entry Data object.
        /// </summary>
        private void SetListEntry()
        {
            objListEntry = new ListEntry();
            objListEntry.WellBookDetails = SetWellBookDetails();

        }

        /// <summary>
        /// Sets the Well Book details data object from the UI.
        /// </summary>
        /// <returns></returns>
        private WellBookDetails SetWellBookDetails()
        {
            WellBookDetails objWellBookDetails = new WellBookDetails();
            CommonUtility objCommonUtilities = new CommonUtility();
            int intRowId = 0;
            objWellBookDetails.Title = txtWellBookTitle.Text.Trim();

            if (cboTeam.SelectedItem != null && !cboTeam.SelectedItem.Value.Contains(DROPDOWNDEFAULTTEXTNONE))
            {
                objWellBookDetails.TeamID = cboTeam.SelectedItem.Value;
            }
            if (cboOwner.SelectedItem != null && !cboOwner.SelectedItem.Value.Contains(DROPDOWNDEFAULTTEXTNONE))
            {
                objWellBookDetails.BookOwnerID = cboOwner.SelectedItem.Value;
            }
            else
            {               
                /// If no user is selected as Book Owner, assign the logged in user as Book Owner
                /// Read the value from Session object (UserRecordID)               
                object objPrivileges = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPrivileges.ToString());
                Privileges objStoredPrivileges = null;
                if (objPrivileges != null)
                {
                    objStoredPrivileges = (Privileges)objPrivileges;
                }
                if (objStoredPrivileges != null && objStoredPrivileges.SystemPrivileges != null)
                {
                    objWellBookDetails.BookOwnerID = objStoredPrivileges.SystemPrivileges.UserRecordID.ToString();
                }
            }
            
            if (string.Compare(strMode,EDIT) == 0)
            {
               if (int.TryParse(strSelectedID, out intRowId))
                {
                    objWellBookDetails.RowId = intRowId;
                }
            }
            else
            {
                objWellBookDetails.Terminated = STATUSACTIVE;
            }
            return objWellBookDetails;
        }
        #endregion
    }
}
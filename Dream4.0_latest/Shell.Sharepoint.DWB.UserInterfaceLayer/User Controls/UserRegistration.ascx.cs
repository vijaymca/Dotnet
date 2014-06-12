#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: UserRegistration.cs
#endregion

using System;
using System.Collections;
using System.Web;
using System.Web.UI.WebControls;

using Shell.SharePoint.DWB.Business.DataObjects;
using System.Net;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DWB.BusinessLogicLayer;
using System.Data;

namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// Code behind class for UserRegistration.ascx
    /// Add/Edit the user details.
    /// </summary>
    public partial class UserRegistration : UIHelper
    {
        #region Declarations
        string strSelectedID = string.Empty;
        string strMode = string.Empty;        
        ListEntry objListEntry;
        CommonBLL objCommonBLL;

        const string EDITUSER = "Edit User:";
        const string USERNAMEXISTS = "User Name or User Id already exists.Please change the name or id.";
        const string USERREGISTRATIONNOTSUCCESS = "User registration is not successful.";
        #endregion Declarations

        #region Protected Methods
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            strMode = HttpContext.Current.Request.QueryString[MODEQUERYSTRING];
            strSelectedID = HttpContext.Current.Request.QueryString[IDVALUEQUERYSTRING];

            try
            {
                if (!Page.IsPostBack)
                {
                    SetControlsForMode();
                }
                //Added By Gopinath.
                cboDiscipline.Attributes.Add("onChange", "javascript:return OnUserDisciplineIsAdmin();");

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
        /// Handles the Click event of the cmdOK control.
        /// Save the new user/Update the existing user details
        /// and returns to User Registration screen after successfull addition
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmdOK_Click(object sender, EventArgs e)
        {
            try
            {
                objListEntry = new ListEntry();
                objListEntry.UserDetails = GetUserDetails();
                objListEntry.Teams = GetTeams();
                bool blnUpdateSuccess = false;
                bool blnUserIdExists = true;
                if(string.Compare(strMode, ADD) == 0)
                {
                    blnUserIdExists = CheckDuplicateName(cboUserId.SelectedItem.Text, DWBUSERIDCOLUMN, USERLIST);
                    if(!blnUserIdExists)
                    {
                        blnUpdateSuccess = UpdateListEntry(objListEntry, USERLIST, USERREGISTRATION, AUDITACTIONCREATION);
                    }
                }
                else if(string.Compare(strMode, EDIT) == 0)
                {
                    blnUserIdExists = CheckDuplicateName(cboUserId.SelectedItem.Text, DWBUSERIDCOLUMN, USERLIST, HttpContext.Current.Request.QueryString[IDVALUEQUERYSTRING]);
                    if(!blnUserIdExists)
                    {
                        blnUpdateSuccess = UpdateListEntry(objListEntry, USERLIST, USERREGISTRATION, AUDITACTIONUPDATION);
                    }
                }
                if(blnUpdateSuccess)
                {
                    if(hdnUserTerminated != null && !string.IsNullOrEmpty(hdnUserTerminated.Value) && string.Equals(hdnUserTerminated.Value, STATUSTERMINATED))
                    {
                        Page.Response.Redirect(USERREGISTRATIONURL + TERMINATESTATUSQUERYSTRING, false);
                    }
                    else
                    {
                        Page.Response.Redirect(USERREGISTRATIONURL, false);
                    }
                }
                else if(blnUserIdExists)
                {
                    lblException.Text = USERNAMEXISTS;
                    lblException.Visible = true;
                    ExceptionBlock.Visible = true;
                }

                else if(!blnUpdateSuccess)
                {
                    lblException.Text = USERREGISTRATIONNOTSUCCESS;
                    lblException.Visible = true;
                    ExceptionBlock.Visible = true;
                }
            }
            catch(WebException webEx)
            {

                lblException.Text = webEx.Message;
                lblException.Visible = true;
                ExceptionBlock.Visible = true;
            }
            catch(Exception ex)
            {

                CommonUtility.HandleException(strParentSiteURL, ex);

            }
        }

        /// <summary>
        /// Handles the Click event of the cmdCancel control.        
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            if (hdnUserTerminated != null && !string.IsNullOrEmpty(hdnUserTerminated.Value) && string.Equals(hdnUserTerminated.Value, "Yes"))
            {
                Page.Response.Redirect(USERREGISTRATIONURL+TERMINATESTATUSQUERYSTRING, false);
            }
            else
            {
                Page.Response.Redirect(USERREGISTRATIONURL, false);
            }
        }

        /// <summary>
        /// Method To change the privileges to Admin if the Checkbox is checked
        /// And Change the privileges to DWB User if the Checkbox is unchecked
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cbIsAdmin_CheckedChanged(object sender, EventArgs e)
        {
            UserDetails objUserDetails = new UserDetails();
            if(cbIsAdmin.Checked)
            {
                objUserDetails.PrivilegeText = "AD - Admin Privilege";
                objUserDetails.PrivilegeCode = "AD";

            }
            else
            {
                objUserDetails.PrivilegeText = "US - DWB User";
                objUserDetails.PrivilegeCode = "US";
            }

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the user details from screen.
        /// </summary>
        /// <returns></returns>
        private UserDetails GetUserDetails()
        {

            /// Assign the values to the object of UserDetails class
            UserDetails objUserDetails = new UserDetails();
           
            objUserDetails.UserName = cboUserId.SelectedItem.Text;
            objUserDetails.WindowUserID = cboUserId.SelectedItem.Text;

            if(string.Compare(strMode, ADD) == 0)
            {
                objUserDetails.Terminated = STATUSACTIVE;
            }
            else if(string.Compare(strMode, EDIT) == 0)
            {
                objUserDetails.Terminated = STATUSTERMINATED;
                objUserDetails.RowId = Int32.Parse(HttpContext.Current.Request.QueryString[IDVALUEQUERYSTRING]);
            }

            objUserDetails.Discipline = cboDiscipline.SelectedItem.Text;
            objUserDetails.DisciplineID = cboDiscipline.SelectedItem.Value;
            
            if(cbIsAdmin.Checked)
            {
                objUserDetails.PrivilegeText = "AD - Admin Privilege";
                objUserDetails.PrivilegeCode = "AD";
                
            }
            else
            {
                objUserDetails.PrivilegeText = "US - DWB User";
                objUserDetails.PrivilegeCode = "US";
            }

            string strTeams = string.Empty;
            /// Selected team ids separated by ";"
            foreach(ListItem item in lstTeams.Items)
            {
                if(item.Selected)
                {
                    strTeams += item.Text + ";";
                }
            }
            objUserDetails.Team = strTeams;

            return objUserDetails;
        }

        /// <summary>
        /// Gets the selected teams while creating new user.
        /// </summary>
        /// <returns></returns>
        private ArrayList GetTeams()
        {
            ArrayList arlTeams = new ArrayList();
            TeamDetails objTeamDetails = null;
            foreach (ListItem item in lstTeams.Items)
            {
                if (item.Selected)
                {
                    objTeamDetails = new TeamDetails();
                    objTeamDetails.RowId = Int32.Parse(item.Value);
                    objTeamDetails.TeamName = item.Text;
                    arlTeams.Add(objTeamDetails);
                }
            }

            return arlTeams;
        }

        /// <summary>
        /// Sets the controls for mode(add/edit).
        /// </summary>
        private void SetControlsForMode()
        {
            string strCAMLQuery = string.Empty;
            string strViewFields = string.Empty;
            objCommonBLL = new CommonBLL();
            DataTable dtDwbUserList = null;
            DataTable dtDreamUserList = null;
            DataTable dtDreamUserOnlyList = null;
            switch (strMode)
            {
                case ADD:
                    {   
                        strCAMLQuery = @"<Where><Eq><FieldRef Name='Access_x0020_Approval_x0020_Stat' /><Value Type='Choice'>Approved</Value></Eq></Where><OrderBy><FieldRef Name='Title' Ascending='True' /></OrderBy>";
                        strViewFields = @"<FieldRef Name='Title'/>";
                        dtDreamUserList = objCommonBLL.ReadList(strParentSiteURL, DREAMUSERLIST, strCAMLQuery, strViewFields);
                        strCAMLQuery = @"<OrderBy><FieldRef Name='Windows_User_ID' Ascending='True' /></OrderBy>";
                        strViewFields = @"<FieldRef Name='Windows_User_ID' />";
                        dtDwbUserList = objCommonBLL.ReadList(strParentSiteURL, USERLIST, strCAMLQuery, strViewFields);
                        dtDreamUserOnlyList = FilterOnlyDreamUsers(dtDreamUserList, dtDwbUserList);
                        ///Populate Windows User Id dropdown with only dreamusers 
                        SetListValuesForDWBUser(cboUserId, dtDreamUserOnlyList);
                        /// Populate Discipline dropdown from SharePoint list
                        SetListValues(cboDiscipline, DISCIPLINELIST);

                        /// Populate Team list box from DWB Application (alread registered teams) not from CDS (confirmed)                        
                        strCAMLQuery = @"<Where><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq></Where>";
                        strViewFields = @"<FieldRef Name='Terminate_Status' /><FieldRef Name='ID' /><FieldRef Name='Title' />";
                        SetListBoxValues(lstTeams, TEAMLIST, strCAMLQuery, strViewFields);
                        break;
                    }
                case EDIT:
                    {
                        /// Retrieve the details from DWB User List and 
                        lblAddUser.Text = EDITUSER;
                        lblTeam.Visible = false;
                        lstTeams.Visible = false;
                        spnTeamMandotory.Visible = false;
                        SetListValues(cboDiscipline, DISCIPLINELIST);
                        objListEntry = GetDetailsForSelectedID(strSelectedID, USERLIST, USERREGISTRATION);

                        if(objListEntry != null && objListEntry.UserDetails != null)
                        {                           
                           lblUser.Text = objListEntry.UserDetails.UserName;                           
                           hdnUserTerminated.Value = objListEntry.UserDetails.Terminated;
                           SetListValues(cboUserId, USERLIST, "Windows_User_ID", "ID", objListEntry.UserDetails.WindowUserID);
                           if(objListEntry.UserDetails.PrivilegeCode == "AD")
                           {
                               cbIsAdmin.Checked = true;
                           }
                           else
                           {
                               cbIsAdmin.Checked = false;
                           }


                           if(cboUserId.Items.FindByText(objListEntry.UserDetails.WindowUserID)!= null)
                           {
                               cboUserId.ClearSelection();
                               cboUserId.Items.FindByText(objListEntry.UserDetails.WindowUserID).Selected = true;

                           }
                           cboUserId.Enabled = false;
                           

                            if(cboDiscipline.Items.FindByValue(objListEntry.UserDetails.DisciplineID) != null)
                            {
                                cboDiscipline.ClearSelection();
                                cboDiscipline.Items.FindByValue(objListEntry.UserDetails.DisciplineID).Selected = true;
                            }
                            
                        }
                        else
                        {
                            lblUser.Text = string.Format(EDITUSER, string.Empty);
                        }
                      break;
                    }
        }
        }

        /// <summary>
        /// Method To filter out only Dream users
        /// </summary>
        /// <param name="dt1">Datatable</param>
        /// <param name="dt2">Datatable</param>
        /// <returns></returns>
        private DataTable FilterOnlyDreamUsers(DataTable dt1, DataTable dt2)
        {

            try
            {
                for(int i = 0; i < dt1.Rows.Count; i++)
                {
                    bool blnPresent = false;
                    for(int j = 0; j < dt2.Rows.Count; j++)
                    {
                        if(dt1.Rows[i][0].ToString() == dt2.Rows[j][0].ToString())
                        {

                            DataRow row = dt1.Rows[i];
                            dt1.Rows.Remove(row);
                            blnPresent = true;
                        }
                        if(blnPresent)
                        {
                            break;
                        }

                    }

                }
            }
            finally
            {
                if(dt1 != null)
                    dt1.Dispose();
                if(dt2 != null)
                    dt1.Dispose();
            }

            return dt1;

        }
        /// <summary>
        /// Binds the UserId's from List to dropdown 
        /// </summary>
        /// <param name="dropDownList">DropdownList</param>
        /// <param name="dt">Datatable</param>
        private void SetListValuesForDWBUser(DropDownList dropDownList, DataTable dt)
        {
           
            DataRow drListData;
            ListItem lstItem;
            try
            {
               
                if(dt != null && dt.Rows.Count > 0)
                {
                    /// Loop through the values in Country List and finds the index of country user preference in List.
                    dropDownList.Items.Clear();
                    dropDownList.Items.Add(DROPDOWNDEFAULTTEXT);
                    for(int intRowIndex = 0; intRowIndex < dt.Rows.Count; intRowIndex++)
                    {
                        drListData = dt.Rows[intRowIndex];
                        lstItem = new ListItem();
                        lstItem.Text = drListData[DWBTITLECOLUMN].ToString();
                        lstItem.Value = drListData[DWBIDCOLUMN].ToString();
                        dropDownList.Items.Add(lstItem);

                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if(dt != null)
                    dt.Dispose();
            }

        }

        #endregion Private Methods

        
    }
}
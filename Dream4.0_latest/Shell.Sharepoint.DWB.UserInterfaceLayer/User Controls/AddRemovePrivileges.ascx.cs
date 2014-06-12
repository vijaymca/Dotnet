#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: AddRemovePrivileges.cs 
#endregion

using System;
using System.Collections;
using System.Net;
using System.Web;
using System.Web.UI.WebControls;

using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DWB.DualListControl;

namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// Code Behind for AddRemovePrivileges User Control 
    /// It provides UI to add/remove Privileges to User, Team Staff
    /// And to add/remove staff from team
    /// </summary>
    public partial class AddRemovePrivileges : UIHelper
    {
        #region DECLARATION
        ListEntry objListEntry;
        string strParentID = string.Empty;
        string strListType = string.Empty;
       
        const string EDITUSERPRIVILEGESHEADING = "Edit System Privileges: {0}";
        const string EDITUSERPRIVILEGESPAGEHEADING = "Edit System Privileges";
        const string ADDREMOVESTAFFSHEADING = "Add / Remove Staff: {0}";
        const string ADDREMOVESTAFFSPAGEHEADING = "Add / Remove Staff";
        const string EDITSTAFFPRIVILEGESHEADING = "Edit Staff Privileges: {0} ({1})";
        const string EDITSTAFFPRIVILEGESPAGEHEADING = "Edit Staff Privileges";
        const string LEFTTITLEPRIVILEGES = "Available";
        const string RIGHTTITLEPRIVILEGES = "Granted";
        const string LEFTTITLESTAFFS = "User List";
        const string RIGHTTITLESTAFFS = "Staff in Team";
        #endregion DECLARATION

        #region EVENT HANDLERS

        /// <summary>
        /// Page Load Event Handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            strParentID = HttpContext.Current.Request.QueryString[IDVALUEQUERYSTRING];
            strListType = HttpContext.Current.Request.QueryString[LISTTYPEQUERYSTRING];
            try
            {
                if (!Page.IsPostBack)
                {
                    /// Set the UI Title
                    SetTitles();
                    /// Based on the listType Bind the Dual List box
                    BindDualListControl();
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
        /// Cancel Handler.
        /// Redirects to Maintenance Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdCancel_Click(object sender, EventArgs e)
        {            
            Response.Redirect(GetRedirectUrl(), false);
        }

        /// <summary>
        /// Save Handler.
        /// Saves the changes to SharePoint list.
        /// Redirectes to Maintenance Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdSave_Click(object sender, EventArgs e)
        {
            /// Update the Privileges column for the new Privileges
            string strPrivileges = string.Empty;
            bool blnUpdateSuccess = false;
            objListEntry = new ListEntry();
            try
            {
                if (!string.IsNullOrEmpty(strListType))
                {
                    switch (strListType)
                    {
                        case USERREGISTRATION: /// Case to add/remove privileges to Users
                            {
                                objListEntry.UserDetails = new UserDetails();
                                objListEntry.UserDetails.RowId = int.Parse(HttpContext.Current.Request.QueryString[IDVALUEQUERYSTRING]);
                                foreach (ListItem lstItemSelectedPrivilege in dualListPrivileges.RightItems)
                                {
                                    if (lstItemSelectedPrivilege != null)
                                    {
                                        strPrivileges += lstItemSelectedPrivilege.Value + SPLITTER_STRING;
                                    }
                                }
                                objListEntry.UserDetails.Privileges = strPrivileges;

                                blnUpdateSuccess = UpdateListEntry(objListEntry, USERLIST, USERPRIVILEGES, AUDITACTIONUPDATION);
                                break;
                            }
                        case STAFFREGISTRATION:/// Case to add/remove Staffs from team
                            {
                                objListEntry.Staffs = new ArrayList();
                                objListEntry.TeamDetails = new TeamDetails();
                                objListEntry.TeamDetails.RowId = Int32.Parse(strParentID);
                                objListEntry.TeamDetails.TeamName = hdnParentName.Value;
                                StaffDetails objStaffDetails = null;
                                int index = 0;
                                foreach (ListItem lstItemSelectedStaff in dualListPrivileges.RightItems)
                                {
                                    if (lstItemSelectedStaff != null)
                                    {
                                        objStaffDetails = new StaffDetails();
                                        objStaffDetails.RowID = lstItemSelectedStaff.Value;
                                        objStaffDetails.UserName = lstItemSelectedStaff.Text;
                                        objStaffDetails.TeamID = strParentID;
                                        objStaffDetails.UserRank = index.ToString();
                                        objListEntry.Staffs.Add(objStaffDetails);
                                        index++;
                                    }
                                }
                                blnUpdateSuccess = UpdateListEntry(objListEntry, TEAMSTAFFLIST, STAFFREGISTRATION, AUDITACTIONUPDATION);
                                break;
                            }
                    }
                }

                if (blnUpdateSuccess)
                {
                    Page.Response.Redirect(GetRedirectUrl(), false);
                }
                else
                {
                    lblException.Text = GetErrorMessage();
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
        #endregion EVENT HANDLERS

        #region PRIVATE METHODS

        /// <summary>
        /// Binds the Dual List Control with Data.
        /// </summary>
        private void BindDualListControl()
        {

            if (!string.IsNullOrEmpty(strListType))
            {
                switch (strListType)
                {
                    case USERREGISTRATION:/// Case to add/remove privileges to Users
                        {
                            SetDualListLeftBox(dualListPrivileges, SYSTEMPRIVILEGESLIST, USERPRIVILEGES, string.Empty);
                            string strPrivileges = string.Empty;
                            if (objListEntry != null && objListEntry.UserDetails != null)
                            {
                                strPrivileges = objListEntry.UserDetails.Privileges;
                            }
                            SetDualListRightBox(dualListPrivileges, SYSTEMPRIVILEGESLIST, strPrivileges, USERPRIVILEGES);
                            break;
                        }
                  
                    case STAFFREGISTRATION:/// Case to add/remove privileges to Staff
                        {
                            SetDualListLeftBox(dualListPrivileges, USERLIST, STAFFREGISTRATION, string.Empty);
                            dualListPrivileges.ExcludeItemInRightBox = true;
                            dualListPrivileges.ExcludeBasedOnText = true;
                            /// ID Column and User_ID column values would be concatenated and set for Value field
                            SetDualListRightBox(dualListPrivileges, TEAMSTAFFLIST, strParentID, STAFFREGISTRATION);
                            break;
                        }
                }
            }

        }

        /// <summary>
        /// Sets the Title,Left and Right text of Dual list
        /// Also populates the ListEntry global object based on ListType
        /// </summary>
        private void SetTitles()
        {
            if (!string.IsNullOrEmpty(strListType))
            {

                switch (strListType)
                {
                    case USERREGISTRATION:/// Case to add/remove privileges to Users
                        {
                            objListEntry = GetDetailsForSelectedID(strParentID, USERLIST, USERREGISTRATION);

                            dualListPrivileges.LeftListLabelText = LEFTTITLEPRIVILEGES;
                            dualListPrivileges.RightListLabelText = RIGHTTITLEPRIVILEGES;
                            this.Parent.Page.Title = EDITUSERPRIVILEGESPAGEHEADING;

                            if (objListEntry != null && objListEntry.UserDetails != null)
                            {
                                lblHeading.Text = string.Format(EDITUSERPRIVILEGESHEADING, objListEntry.UserDetails.UserName);
                                hdnParentName.Value = objListEntry.UserDetails.UserName;
                            }
                            else
                            {
                                lblHeading.Text = string.Format(EDITUSERPRIVILEGESHEADING, string.Empty);
                            }
                            break;
                        }
                    case STAFFREGISTRATION:/// Case to add/remove Staffs from team
                        {
                            objListEntry = GetDetailsForSelectedID(strParentID, TEAMLIST, STAFFREGISTRATION);

                            dualListPrivileges.LeftListLabelText = LEFTTITLESTAFFS;
                            dualListPrivileges.RightListLabelText = RIGHTTITLESTAFFS;
                            this.Parent.Page.Title = ADDREMOVESTAFFSPAGEHEADING;
                            if (objListEntry != null && objListEntry.TeamDetails != null)
                            {
                                lblHeading.Text = string.Format(ADDREMOVESTAFFSHEADING, objListEntry.TeamDetails.TeamName);
                                hdnParentName.Value = objListEntry.TeamDetails.TeamName;
                            }
                            else
                            {
                                lblHeading.Text = string.Format(ADDREMOVESTAFFSHEADING, string.Empty);
                            }
                            break;
                        }
                   
                }
            }
        }

        /// <summary>
        /// Returns the Redirect Url based on the listType Query String value
        /// </summary>
        /// <returns></returns>
        private string GetRedirectUrl()
        {
            string strRedirectUrl = string.Empty;
            if (!string.IsNullOrEmpty(strListType))
            {

                switch (strListType)
                {
                    case USERREGISTRATION:/// Case to add/remove privileges to Users
                        {
                            strRedirectUrl = USERREGISTRATIONURL;
                            break;
                        }
                    case STAFFREGISTRATION:/// Case to add/remove Staffs from team
                        {
                            strRedirectUrl = STAFFLISTURL + "?" + IDVALUEQUERYSTRING + "=" + strParentID + "&" + LISTTYPEQUERYSTRING + "=" + strListType;
                            break;
                        }
                    default: /// Redirect to Home Page
                        {
                            strRedirectUrl = DWBHOMEURL;
                            break;
                        }
                }
            }
            else
            {
                strRedirectUrl = DWBHOMEURL;
            }
            return strRedirectUrl;
        }

        /// <summary>
        /// Returns the error message based on ListType value.
        /// </summary>
        /// <returns>string.</returns>
        private string GetErrorMessage()
        {
            string strErrorMessage = "Data is not saved.";
            if (!string.IsNullOrEmpty(strListType))
            {

                switch (strListType)
                {
                    case USERREGISTRATION:/// Case to add/remove privileges to Users
                        {
                            strErrorMessage = "Privileges are not saved.";
                            break;
                        }
                    case STAFFREGISTRATION:/// Case to add/remove Staffs from team
                        {
                            strErrorMessage = "Staff are not saved.";
                            break;
                        }
                    
                }
            }
            return strErrorMessage;
        }
         #endregion PRIVATE METHODS
    }
}
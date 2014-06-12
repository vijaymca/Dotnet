#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: ManageStaff.cs
#endregion
using System;
using System.Collections.Generic;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;
using System.Web.Services.Protocols;
using System.Net;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web;


namespace Shell.SharePoint.DREAM.Site.UI
{
    /// <summary>
    /// Manage Staff user control handles the add /remove of staff to a particular team.
    /// </summary>
    public partial class ManageStaff : UIHelper
    {


        #region DECLARATION
        const string LEFTLISTTITLE = "User List";
        const string RIGHTLISTTITLE = "Staff In Team";
        #endregion

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"></see> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnLoad(System.EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    if (Request.QueryString["idValue"] != null)
                    {
                        //reads the team name from the sharepoint list for the particular teamid.
                        objMossController = objFactory.GetServiceManager("MossService");
                        string strQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + Request.QueryString["idValue"].ToString() + "</Value></Eq></Where>";
                        string strFields = @"<FieldRef Name='Title' />";

                        //binds the team details to the UI controls .
                        DataTable dtTeamName = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, TEAMREGISTRATIONLIST, strQuery, strFields);
                        lblManageStaffTitle.Text = dtTeamName.Rows[0].ItemArray[0].ToString();
                        dualManageStaff.LeftListLabelText = LEFTLISTTITLE;
                        dualManageStaff.RightListLabelText = RIGHTLISTTITLE;
                        if (dtTeamName != null)
                            dtTeamName.Dispose();

                        //binds the dual list controls with values. 
                        BindDualListControl();
                    }
                }
            }
            catch (WebException webEx)
            {
                ShowLableMessage(webEx.Message);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }

        /// <summary>
        /// Shows the lable message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void ShowLableMessage(string message)
        {
            lblErrorMessage.Visible = true;
            lblErrorMessage.Text = message;
        }
        /// <summary>
        /// Binds the dual list control.
        /// </summary>
        private void BindDualListControl()
        {
            objMossController = objFactory.GetServiceManager("MossService");

            string strQuerystring = @"<OrderBy><FieldRef Name='DisplayName' /></OrderBy><Where><Eq><FieldRef Name='Active' /><Value Type='Text'>Yes</Value></Eq></Where>";
            string strViewFields = @"<FieldRef Name='DisplayName' /><FieldRef Name='ID' /><FieldRef Name='TeamID' /><FieldRef Name='IsTeamOwner' />";

            DataTable dtManageStaff = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, USERACCESSREQUESTLIST, strQuerystring, strViewFields);

            //get the teamowner 
            DataRow[] drTeamOwnerDetail = dtManageStaff.Select("IsTeamOwner ='Yes' AND TeamID='" + Request.QueryString["idValue"].ToString() + "'");

            /// Multi Team Owner Implementation
            /// Changed By: Yasotha
            /// Date : 13-Jan-2010

            if (drTeamOwnerDetail != null && drTeamOwnerDetail.Length > 0)
            {
                for (int intRowIndex = 0; intRowIndex < drTeamOwnerDetail.Length; intRowIndex++)
                {
                    hidTeamOwnerUserId.Value += drTeamOwnerDetail[intRowIndex]["ID"].ToString() + ";";
                }
                if (!string.IsNullOrEmpty(hidTeamOwnerUserId.Value))
                {
                    if (hidTeamOwnerUserId.Value.LastIndexOf(";") != -1)
                    {
                        hidTeamOwnerUserId.Value = hidTeamOwnerUserId.Value.Remove(hidTeamOwnerUserId.Value.LastIndexOf(";"));
                    }
                }
            }
            /// Multi Team Owner implementation
            /// End

            if (dtManageStaff != null && dtManageStaff.Rows.Count > 0)
            {
                dualManageStaff.LeftItems.Clear();

                DataRow[] arRegUsers = dtManageStaff.Select("TeamID = '0'");
                ListItem lstItem;
                for (int intCounter = 0; intCounter < arRegUsers.Length; intCounter++)
                {
                    DataRow drRegUsers = (DataRow)arRegUsers.GetValue(intCounter);

                    lstItem = new ListItem();
                    lstItem.Text = drRegUsers["DisplayName"].ToString();
                    lstItem.Value = drRegUsers["ID"].ToString();
                    dualManageStaff.LeftItems.Add(lstItem);
                }

                DataRow[] arStaffUsers = dtManageStaff.Select("TeamID='" + Request.QueryString["idValue"].ToString() + "'");

                for (int intCounter = 0; intCounter < arStaffUsers.Length; intCounter++)
                {
                    DataRow drStaffUsers = (DataRow)arStaffUsers.GetValue(intCounter);

                    lstItem = new ListItem();
                    lstItem.Text = drStaffUsers["DisplayName"].ToString();
                    lstItem.Value = drStaffUsers["ID"].ToString();
                    dualManageStaff.RightItems.Add(lstItem);
                }
                if (dtManageStaff != null)
                    dtManageStaff.Dispose();
            }

        }

        /// <summary>
        /// Handles the Click event of the CmdCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void CmdCancel_Click(object sender, EventArgs e)
        {
            objUtility = new CommonUtility();
            string strPermission = objUtility.GetTeamPermission(strCurrSiteUrl);
            objMossController = objFactory.GetServiceManager("MossService");
            if ((!((MOSSServiceManager)objMossController).IsAdmin(strCurrSiteUrl, HttpContext.Current.User.Identity.Name)) && (string.Equals(strPermission, TEAMOWNERPERMISSION)))
                Response.Redirect("/Pages/MyTeam.aspx", false);
            else
                if (((MOSSServiceManager)objMossController).IsAdmin(strCurrSiteUrl, HttpContext.Current.User.Identity.Name))
                    Response.Redirect("/Pages/TeamManagement.aspx", false);

        }

        /// <summary>
        /// Handles the Click event of the CmdSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void CmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                objUtility = new CommonUtility();
                objMossController = objFactory.GetServiceManager("MossService");
                Dictionary<string, string> dicListValues = null;

                //read from the USERRegistration list the staff list for the particular team. 
                string strStaffQuery = "<Where><And><And><Eq><FieldRef Name=\"TeamID\" /><Value Type=\"Text\">" + Request.QueryString["idValue"].ToString()
                + "</Value></Eq><Eq><FieldRef Name=\"IsTeamOwner\"/><Value Type=\"Text\">No</Value></Eq></And><Eq><FieldRef Name=\"Active\"/><Value Type=\"Text\">Yes</Value></Eq></And></Where>";
                DataTable dtTeamStaff = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, USERACCESSREQUESTLIST, strStaffQuery);
                ArrayList arlTeamStaff = new ArrayList();

                foreach (DataRow objRow in dtTeamStaff.Rows)
                {
                    arlTeamStaff.Add(objRow["ID"].ToString());
                }


                /// Multi Team Owner Implementation
                /// Changed By: Yasotha
                /// Date : 20-Jan-2010

                foreach (ListItem objItem in dualManageStaff.RightItems)
                {
                    if (arlTeamStaff.Count > 0)
                    {
                        if (!arlTeamStaff.Contains(objItem.Value) && (!hidTeamOwnerUserId.Value.Contains(objItem.Value)))
                        /// Multi Team Owner Implementation
                        /// End
                        {
                            dicListValues = new Dictionary<string, string>();
                            dicListValues.Add("ID", objItem.Value);
                            dicListValues.Add("TeamID", Request.QueryString["idValue"].ToString());

                            ((MOSSServiceManager)objMossController).UpdateListItem(dicListValues, USERACCESSREQUESTLIST);

                            objUtility.SendEmailForTeamAccessStatus(objItem.Value, "Approved", Request.QueryString["idValue"].ToString());
                        }
                    }
                    else
                    {
                        /// Multi Team Owner Implementation
                        /// Changed By: Yasotha
                        /// Date : 20-Jan-2010
                        if (!hidTeamOwnerUserId.Value.Contains(objItem.Value))
                        {
                            /// Multi Team Owner Implementation
                            /// End
                            dicListValues = new Dictionary<string, string>();
                            dicListValues.Add("ID", objItem.Value);
                            dicListValues.Add("TeamID", Request.QueryString["idValue"].ToString());

                            ((MOSSServiceManager)objMossController).UpdateListItem(dicListValues, USERACCESSREQUESTLIST);

                            objUtility.SendEmailForTeamAccessStatus(objItem.Value, "Approved", Request.QueryString["idValue"].ToString());
                        }
                    }

                }

                foreach (ListItem objItem in dualManageStaff.LeftItems)
                {
                    if (arlTeamStaff.Count > 0)
                    {
                        if (arlTeamStaff.Contains(objItem.Value))
                        {
                            dicListValues = new Dictionary<string, string>();
                            dicListValues.Add("ID", objItem.Value);
                            dicListValues.Add("TeamID", "0");

                            ((MOSSServiceManager)objMossController).UpdateListItem(dicListValues, USERACCESSREQUESTLIST);

                            objUtility.SendEmailForTeamAccessStatus(objItem.Value, "Rejected", Request.QueryString["idValue"].ToString());
                        }
                    }
                }

                string strPermission = objUtility.GetTeamPermission(strCurrSiteUrl);
                objMossController = objFactory.GetServiceManager("MossService");
                if ((!((MOSSServiceManager)objMossController).IsAdmin(strCurrSiteUrl, HttpContext.Current.User.Identity.Name)) && (string.Equals(strPermission, TEAMOWNERPERMISSION)))
                    Response.Redirect("/Pages/MyTeam.aspx", false);
                else
                    if (((MOSSServiceManager)objMossController).IsAdmin(strCurrSiteUrl, HttpContext.Current.User.Identity.Name))
                        Response.Redirect("/Pages/TeamManagement.aspx", false);
            }
            catch (WebException webEx)
            {
                ShowLableMessage(webEx.Message);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }


    }
}
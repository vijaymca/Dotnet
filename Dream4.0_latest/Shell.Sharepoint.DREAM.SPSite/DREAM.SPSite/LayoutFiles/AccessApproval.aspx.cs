#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: AccessApproval.aspx.cs
#endregion

using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DREAM.Controller;
using System.Text;
using System.Net;

using Microsoft.SharePoint;

namespace Shell.SharePoint.DREAM.Site.UI
{
    /// <summary>
    /// This is used when Non-Authorized userd try getting access to the Portal.
    /// </summary>
    public partial class AccessApproval :System.Web.UI.Page
    {
        CommonUtility objUtility;
        ActiveDirectoryService objADS;
        const string USERACCESSREQUESTLIST = "User Access Request";
        const string TEAMACCESSREQUESTLIST = "Team Access Request";
        const string TEAMREGISTRATIONLIST = "Team Registration";
        const string STATUSINPROGRESS = "In Progress";
        const string STATUSREJECTED = "Rejected";
        const string STATUSAPPROVED = "Approved";
        AbstractController objMossController;
        ServiceProvider objFactory = new ServiceProvider();


        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                trSiteMaintenance.Visible = false;
                if(!Page.IsPostBack)
                {
                    /// Set Region Title
                    SetRegionTitle();
                    objUtility = new CommonUtility();
                    txtUserAcc.Text = objUtility.GetUserName();
                    if(Request.QueryString["IsMaintenance"] != null)
                    {
                        if(Request.QueryString["IsMaintenance"].ToLowerInvariant().Equals("true"))
                        {
                            /// Display the Site Maintenace message
                            trAccessApproval.Visible = false;
                            trSiteMaintenance.Visible = true;
                            lblSiteMaintenanceMessage.Text = PortalConfiguration.GetInstance().GetKey("SiteMaintenanceMessage");
                            this.Page.Title = lblRegionTitle.Text;
                        }
                    }
                    else if(Request.QueryString["teamId"] != null)
                    {
                        lblTitle.Text = "Request Access for ";
                        DataTable dtTeamDetail = new DataTable();
                        objMossController = objFactory.GetServiceManager("MossService");
                        dtTeamDetail =
                            ((MOSSServiceManager)objMossController).ReadList(
                                HttpContext.Current.Request.Url.ToString(), TEAMREGISTRATIONLIST,
                                "<Where><Eq><FieldRef Name='ID'/><Value Type=\"Counter\">" +
                                Request.QueryString["teamId"].ToString() + "</Value></Eq></Where>");

                        if(dtTeamDetail != null)
                        {
                            lblTeamName.Text = dtTeamDetail.Rows[0]["Title"].ToString();
                            hidTeamOwner.Value = dtTeamDetail.Rows[0]["TeamOwner"].ToString();

                            if(dtTeamDetail != null)
                                dtTeamDetail.Dispose();
                        }

                    }
                    else
                    {
                        lblTitle.Text = "Access Approval Login";
                        lblTeamName.Visible = false;
                    }
                    txtRegion.Text = objUtility.GetUserDomain();
                }
            }
            catch(WebException webEx)
            {
                //ExceptionPanel.Visible = true;
                //lblException.Visible = true;
                //lblException.Text = webEx.Message;
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
        }

        /// <summary>
        /// Handles the Click event of the cmdSubmit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmdSubmit_Click(object sender, EventArgs e)
        {
            DataTable dtListValue = null;
            try
            {

                if(Page.IsPostBack)
                {

                    string strToMailID = string.Empty;
                    string strCAMLQuery = string.Empty;
                    string strStatus = string.Empty;
                    string strActive = string.Empty;
                    bool blnSendMail = false;
                    objUtility = new CommonUtility();
                    objADS = new ActiveDirectoryService();

                    string strParentSiteURL = HttpContext.Current.Request.Url.ToString();
                    string strUserName = txtUserAcc.Text.ToString();

                    StringBuilder strMessage = new StringBuilder();

                    ServiceProvider objFactory = new ServiceProvider();
                    objMossController = objFactory.GetServiceManager("MossService");

                    dtListValue = new DataTable();
                    DataRow listRow;


                    strMessage.Append("User ID : " + strUserName);
                    strMessage.Append("<BR>Region: " + txtRegion.Text);
                    strMessage.Append("<BR>Purpose : " + txtPurpose.Text);
                    strToMailID = objADS.GetEmailID(strUserName);


                    if(Request.QueryString["teamId"] == null)
                    {
                        strCAMLQuery = "<OrderBy><FieldRef Name=\"LinkTitleNoMenu\" /></OrderBy><Where><Eq><FieldRef Name=\"LinkTitle\" /><Value Type=\"Computed\">" + strUserName + "</Value></Eq></Where>";
                        dtListValue = ((MOSSServiceManager)objMossController).ReadList(strParentSiteURL, USERACCESSREQUESTLIST, strCAMLQuery);

                        if(dtListValue.Rows.Count > 0)
                        {
                            listRow = dtListValue.Rows[0];
                            strStatus = listRow["Access_x0020_Approval_x0020_Stat"].ToString();
                            strActive = listRow["Active"].ToString();
                        }

                        if(string.Compare(strStatus, STATUSINPROGRESS, true) == 0)
                        {
                            lblMessage.Text = "You have already requested for access. Please contact administrator.";
                        }
                        else
                        {

                            if((string.IsNullOrEmpty(strStatus)) || ((string.Compare(strStatus, STATUSAPPROVED, true) == 0) && (string.Compare(strActive, "No", true) == 0)))
                            {
                                ((MOSSServiceManager)objMossController).CreateAccessRequest(strUserName, txtRegion.Text, txtPurpose.Text, string.Empty, USERACCESSREQUESTLIST);
                                blnSendMail = true;
                            }
                            else if(string.Compare(strStatus, STATUSREJECTED, true) == 0)
                            {
                                strMessage.Append("<BR><BR><font color='red'><b>Please note that the user's request has been rejected earlier</b></font>");
                                blnSendMail = true;
                            }
                        }

                        if(blnSendMail)
                        {
                            objUtility.SendMailforUserAccessRequest(strToMailID, strMessage.ToString());
                            lblMessage.Text = "Thank you for your access request. You will receive an email shortly in response.";
                        }

                    }
                    else
                    {
                        objUtility.SendTeamAccessRequestEmail(strToMailID, strMessage.ToString(), lblTeamName.Text, hidTeamOwner.Value);
                        lblMessage.Text = "Thank you for your access request. You will receive an email shortly in response.";
                    }

                    cmdSubmit.Visible = false;
                    cmdReset.Visible = false;
                }
            }
            catch(WebException webEx)
            {
                //  ExceptionPanel.Visible = true;
                // lblException.Visible = true;
                //  lblException.Text = webEx.Message;
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
            finally
            {
                if(dtListValue != null)
                    dtListValue.Dispose();
            }
        }

        /// <summary>
        /// Sets the region title.
        /// </summary>
        private void SetRegionTitle()
        {
            lblRegionTitle.Text = "Data Retrieval EPICURE Application";
            string strUrl = string.Empty;
            try
            {
                strUrl = SPContext.Current.Site.Url;
                if(lblRegionTitle != null)
                {
                    if(SPContext.Current != null)
                    {
                        using(SPSite spSite = new SPSite(strUrl))
                        {
                            using(SPWeb spWeb = spSite.OpenWeb())
                            {
                                if(spWeb != null)
                                {
                                    lblRegionTitle.Text = spWeb.Title;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                lblRegionTitle.Text = "Data Retrieval EPICURE Application";
            }
        }
    }
}

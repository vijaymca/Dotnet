#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: TeamRegistration.cs
#endregion

using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DREAM.Utilities;
using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Net;

namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// Code Behind class for TeamRegistration user control
    /// Provides UI for Add/Edit Team
    /// </summary>
    public partial class TeamRegistration : UIHelper
    {
        #region Declarations
        string strSelectedID = string.Empty;
        string strMode = string.Empty;        
        const string EDITTEAM = "Edit Team :";
        const string TEAMNAMEEXISTSMESSAGE = "Team Name already exists. Please change the Team Name.";
        const string TEAMNOTCREATEMESSAGE = "Team registration is not successful.";
        const string ASSETNODESELECTPATH = "response/report/record/attribute[@name='Class']/@value";
        ListEntry objListEntry;
        #endregion

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
            }
            catch (System.Web.Services.Protocols.SoapException soapEx)
            {
                CommonUtility.HandleException(strParentSiteURL, soapEx,1);
                lblException.Text =  soapEx.Message;
                lblException.Visible = true;
                ExceptionBlock.Visible = true;
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(strParentSiteURL, webEx,1);
                lblException.Text = webEx.Message;
                lblException.Visible = true;
                ExceptionBlock.Visible = true;
            }
            catch (Exception Ex)
            {
                CommonUtility.HandleException(strParentSiteURL, Ex);
            }
        }

        /// <summary>
        /// Save the new team and returns to Team Registration screen after successfull addition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdOK_Click(object sender, EventArgs e)
        {
            try
            {
                objListEntry = new ListEntry();
                objListEntry.TeamDetails = GetTeamDetails();
                bool blnUpdateSuccess = false;
                bool blnNameExists = true;

                if (string.Compare(strMode, ADD) == 0)
                {
                    blnNameExists = CheckDuplicateName(txtTeamName.Text.Trim(),DWBTITLECOLUMN, TEAMLIST);
                    if (!blnNameExists)
                    {
                        blnUpdateSuccess = UpdateListEntry(objListEntry, TEAMLIST, TEAMREGISTRATION, AUDITACTIONCREATION);
                    }                    
                }
                else if (string.Compare(strMode, EDIT) == 0)
                {                   
                    blnNameExists = CheckDuplicateName(txtTeamName.Text.Trim(), DWBTITLECOLUMN, TEAMLIST, HttpContext.Current.Request.QueryString[IDVALUEQUERYSTRING]);
                    if (!blnNameExists)
                    {
                        blnUpdateSuccess = UpdateListEntry(objListEntry, TEAMLIST, TEAMREGISTRATION, AUDITACTIONUPDATION);
                    }
                }

                if (blnUpdateSuccess)
                {
                    if (hdnTeamTerminated != null && !string.IsNullOrEmpty(hdnTeamTerminated.Value) && string.Equals(hdnTeamTerminated.Value, STATUSTERMINATED))
                    {
                        Page.Response.Redirect(TEAMREGISTRATIONURL +TERMINATESTATUSQUERYSTRING, false);
                    }
                    else
                    {
                        Page.Response.Redirect(TEAMREGISTRATIONURL, false);
                    }
                }
                else if (blnNameExists)
                {
                    lblException.Text = TEAMNAMEEXISTSMESSAGE;
                    lblException.Visible = true;
                    ExceptionBlock.Visible = true;
                }
                else
                {
                    lblException.Text = TEAMNOTCREATEMESSAGE;
                    lblException.Visible = true;
                    ExceptionBlock.Visible = true;
                }
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(strParentSiteURL, webEx,1);
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
        /// Handles the Click event of the cmdCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            if (hdnTeamTerminated != null && !string.IsNullOrEmpty(hdnTeamTerminated.Value) && string.Equals(hdnTeamTerminated.Value, STATUSTERMINATED))
            {
                Page.Response.Redirect(TEAMREGISTRATIONURL +TERMINATESTATUSQUERYSTRING, false);
            }
            else
            {
                Page.Response.Redirect(TEAMREGISTRATIONURL, false);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Sets the controls for mode.
        /// </summary>
        private void SetControlsForMode()
        {
            string strCAMLQuery = string.Empty;
            string strViewFields = string.Empty;
            switch (strMode)
            {
                case ADD:
                    {
                        BindDropDownList();

                        break;
                    }
                case EDIT:
                    {
                        lblAddTeam.Text = EDITTEAM;
                        
                        objListEntry = GetDetailsForSelectedID(strSelectedID, TEAMLIST, TEAMREGISTRATION);
                        if (objListEntry != null && objListEntry.TeamDetails != null)
                        {
                            txtTeamName.Text = objListEntry.TeamDetails.TeamName;
                            lblTeam.Text = objListEntry.TeamDetails.TeamName;
                            hdnTeamTerminated.Value = objListEntry.TeamDetails.Terminated;
                            BindDropDownList();
                            if (cboAssetOwner.Items.FindByText(objListEntry.TeamDetails.AssetOwner) != null)
                            {
                                cboAssetOwner.ClearSelection();

                                cboAssetOwner.Items.FindByText(objListEntry.TeamDetails.AssetOwner).Selected = true;
                            }
                        }
                        else
                        {
                            lblTeam.Text = string.Format(EDITTEAM, string.Empty);
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// Binds the drop down list.
        /// </summary>
        private void BindDropDownList()
        {
            Shell.SharePoint.DWB.BusinessLogicLayer.TeamStaffRegistrationBLL objTeamStaffRegistrationBLL = new Shell.SharePoint.DWB.BusinessLogicLayer.TeamStaffRegistrationBLL();
            System.Xml.XmlDocument assetOwners = objTeamStaffRegistrationBLL.GetAssetOwnersFromService();
            if (assetOwners != null)
            {
                cboAssetOwner.DataSource = assetOwners.SelectNodes(ASSETNODESELECTPATH);
                cboAssetOwner.DataTextField = VALUE;
                cboAssetOwner.DataBind();

                ListItem lstSelect = new ListItem(DROPDOWNDEFAULTTEXT, "0");
                cboAssetOwner.Items.Insert(0, lstSelect);
               
            }
        }
      
        /// <summary>
        /// Gets the team details.
        /// </summary>
        /// <returns></returns>
        private TeamDetails GetTeamDetails()
        {

            /// Assign the values to the object of UserDetails class
            TeamDetails objTeamDetails = new TeamDetails();
            objTeamDetails.TeamName = txtTeamName.Text.Trim();

            if (string.Compare(strMode, ADD) == 0)
            {
                objTeamDetails.Terminated = STATUSACTIVE;
            }
            else if (string.Compare(strMode, EDIT) == 0)
            {
                objTeamDetails.Terminated = STATUSTERMINATED;
                objTeamDetails.RowId = Int32.Parse(HttpContext.Current.Request.QueryString[IDVALUEQUERYSTRING]);
            }
            if (cboAssetOwner.SelectedItem != null)
            {
                objTeamDetails.AssetOwner = cboAssetOwner.SelectedItem.Text;
                objTeamDetails.AssetOwnerID = cboAssetOwner.SelectedItem.Value;
            }

            return objTeamDetails;
        }
        #endregion
    }
}
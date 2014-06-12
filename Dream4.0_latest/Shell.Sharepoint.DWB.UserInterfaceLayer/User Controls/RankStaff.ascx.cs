#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: RankStaff.cs
#endregion

using System;
using System.Collections;
using System.Web;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DWB.BusinessLogicLayer;
using System.Data;
using System.Web.UI.WebControls;

namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// Code Behind class for RankSTaff user control
    /// Provides UI to Rank the Staff in a Team
    /// </summary>
    public partial class RankStaff : UIHelper
    {
        #region Declarations
        string strSelectedID = string.Empty;
        const string RANKSNOTUPDATEDMESSAGE = "Ranks are not saved successfully. Please try again.";
        #endregion

        #region Protected Methods
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
                if (!Page.IsPostBack)
                {
                    SetUIControls();
                }
            }
            catch (Exception Ex)
            {
                CommonUtility.HandleException(strParentSiteURL, Ex);
            }
        }

        /// <summary>
        /// Handles the Click event of the cmdSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void CmdSave_Click(object sender, EventArgs e)
        {
            /// Collect the items with new order
            try
            {
                bool blnUpdateSuccess = SaveNewRanks();
                if (blnUpdateSuccess)
                {
                    Page.Response.Redirect(STAFFLISTURL + "?" + IDVALUEQUERYSTRING + "=" + strSelectedID + "&" + LISTTYPEQUERYSTRING + "=" + TEAMREGISTRATION, false);
                }
                else
                {
                    lblException.Text = RANKSNOTUPDATEDMESSAGE;
                    lblException.Visible = true;
                    ExceptionBlock.Visible = true;
                }
            }
            catch (Exception Ex)
            {
                CommonUtility.HandleException(strParentSiteURL, Ex);
            }
        }

        /// <summary>
        /// Handles the Click event of the cmdCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void CmdCancel_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(STAFFLISTURL + "?" + IDVALUEQUERYSTRING + "=" + strSelectedID + "&" + LISTTYPEQUERYSTRING + "=" + TEAMREGISTRATION, false);
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cboDiscipline control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void CboDiscipline_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindStaffs();
            }
            catch (Exception Ex)
            {
                CommonUtility.HandleException(strParentSiteURL, Ex);
            }
        }

        /// <summary>
        /// Handles the Click event of the CmdSaveContinue control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void CmdSaveContinue_Click(object sender, EventArgs e)
        {
            bool blnUpdateSuccess = false;
            try
            {
                blnUpdateSuccess = SaveNewRanks();
                if (blnUpdateSuccess)
                {
                    BindStaffs();
                }
                else
                {
                    lblException.Text = RANKSNOTUPDATEDMESSAGE;
                    lblException.Visible = true;
                    ExceptionBlock.Visible = true;

                }
            }
            catch (Exception Ex)
            {
                CommonUtility.HandleException(strParentSiteURL, Ex);
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Sets the UI controls.
        /// </summary>
        private void SetUIControls()
        {
            /// Call the GetTeam methods From the Team Details assign the Team Name to the heading
            /// Populate the available Disciplines in the selected Team
            ListEntry objListEntry = GetDetailsForSelectedID(strSelectedID, TEAMLIST, TEAMREGISTRATION);
            if (objListEntry != null && objListEntry.TeamDetails != null)
            {
                hdnParentID.Value = objListEntry.TeamDetails.RowId.ToString();
                hdnParentName.Value = objListEntry.TeamDetails.TeamName;

                lblParentName.Text = objListEntry.TeamDetails.TeamName;
            }
            TeamStaffRegistrationBLL objTeamStaffRegistrationBLL = new TeamStaffRegistrationBLL();
            DataTable dtDiscipline = objTeamStaffRegistrationBLL.GetDisciplinesInTeam(strParentSiteURL, strSelectedID, TEAMSTAFFLIST);
            cboDiscipline.DataSource = dtDiscipline;
            cboDiscipline.DataTextField = DISCIPLINECOLUMN;
            cboDiscipline.DataBind();

            ListItem lstSelect = new ListItem(DROPDOWNDEFAULTTEXTNONE, "0");
            cboDiscipline.Items.Insert(0, lstSelect);
            if (dtDiscipline != null)
            {
                dtDiscipline.Dispose();
            }
        }

        /// <summary>
        /// Binds the staffs.
        /// </summary>
        private void BindStaffs()
        {
            dualListPrivileges.RightItems.Clear();

            TeamStaffRegistrationBLL objTeamStaffRegistrationBLL = new TeamStaffRegistrationBLL();
            DataTable dtStaffs = objTeamStaffRegistrationBLL.GetStaffsForDiscipline(strParentSiteURL, strSelectedID, cboDiscipline.SelectedItem.Text, TEAMSTAFFLIST);


            if (dtStaffs != null && dtStaffs.Rows.Count > 0)
            {
                foreach (DataRow dtRow in dtStaffs.Rows)
                {
                    ListItem lstItem = new ListItem();
                    lstItem.Text = dtRow["User_Rank"].ToString() + "-" + dtRow[DWBTITLECOLUMN].ToString();
                    lstItem.Value = dtRow[DWBIDCOLUMN].ToString();
                    dualListPrivileges.RightItems.Add(lstItem);
                }
            }
            if (dtStaffs != null)
            {
                dtStaffs.Dispose();
            }
        }

        /// <summary>
        /// Saves the new ranks.
        /// </summary>
        /// <returns>bool</returns>
        private bool SaveNewRanks()
        {
            ListEntry objListEntry = new ListEntry();
            objListEntry.TeamDetails = new TeamDetails();
            objListEntry.TeamDetails.RowId = Int32.Parse(strSelectedID);
            objListEntry.Staffs = GetNewStaffRanks();

            bool blnUpdateSuccess = UpdateListEntry(objListEntry, TEAMSTAFFLIST, STAFFRANK, AUDITACTIONUPDATION);

            return blnUpdateSuccess;
        }

        /// <summary>
        /// Gets the new staff ranks.
        /// </summary>
        /// <returns></returns>
        private ArrayList GetNewStaffRanks()
        {
            ArrayList arlStaffs = new ArrayList();
            StaffDetails objStaffDetails = null;
            for (int intIndex = 0; intIndex < dualListPrivileges.RightItems.Count; intIndex++)
            {
                objStaffDetails = new StaffDetails();
                objStaffDetails.RowID = dualListPrivileges.RightItems[intIndex].Value;
                objStaffDetails.UserRank = Convert.ToString(intIndex + 1);
                arlStaffs.Add(objStaffDetails);
            }
            return arlStaffs;
        }
        #endregion

    }
}
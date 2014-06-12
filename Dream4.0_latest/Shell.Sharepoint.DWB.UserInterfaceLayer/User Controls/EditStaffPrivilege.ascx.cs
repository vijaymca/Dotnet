using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;
using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DWB.BusinessLogicLayer;


namespace Shell.SharePoint.DWB.UserInterfaceLayer.User_Controls
{
    public partial class EditStaffPrivilege :UIHelper
    {
        //EditStaffPrivilegeBLL objEditStaffPrivilegeBLL;
        TeamStaffRegistrationBLL objTeamStaffRegistrationBLL;
        string strSelectedID = string.Empty;
        string strMode = string.Empty;
        StaffDetails objStaffData = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            strMode = HttpContext.Current.Request.QueryString[MODEQUERYSTRING];
            strSelectedID = HttpContext.Current.Request.QueryString[IDVALUEQUERYSTRING];
            string strCAMLQuery = string.Empty;
            string strViewFields = string.Empty;
            try
            {
                if(!IsPostBack)
                {
                    objTeamStaffRegistrationBLL = new TeamStaffRegistrationBLL();
                    strViewFields = "<FieldRef Name='Title'/>";
                    objStaffData = objTeamStaffRegistrationBLL.GetSelectedStaffDetails(strParentSiteURL, strSelectedID, TEAMSTAFFLIST);
                    
                        if(objStaffData != null)
                        {
                            SetListValues(cboEditPrivileges, SYSTEMPRIVILEGESLIST, strCAMLQuery, strViewFields);
                            //cboEditPrivileges.Items.FindByValue(objStaffData.RowID).Selected = true;
                            if(!string.IsNullOrEmpty(objStaffData.PRIVILEGE))
                            cboEditPrivileges.Items.FindByText(objStaffData.PRIVILEGE).Selected = true;
                                                   
                        }
                   
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
        /// Saving the updated privilege for the selected Staff.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdOK_Click(object sender, EventArgs e)
        {
            string strRedirectUrl = string.Empty;
            try
            {
                objTeamStaffRegistrationBLL = new TeamStaffRegistrationBLL();

                objStaffData = objTeamStaffRegistrationBLL.GetSelectedStaffDetails(strParentSiteURL, strSelectedID, TEAMSTAFFLIST);
                objStaffData.PRIVILEGE = cboEditPrivileges.SelectedItem.Text;
                

                objTeamStaffRegistrationBLL.UpdateStaffPrivilege(strParentSiteURL, objStaffData, TEAMSTAFFLIST);
                string strTeamId = objStaffData.TeamID;

                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ClosePopupAndRefreshParent", "javascript:ClosePopupAndRefreshParent('+ strTeamId + ',' + TEAMSTAFFLIST + ');", true);

            
                
            }
            catch(WebException webEx)
            {
                CommonUtility.HandleException(strParentSiteURL, webEx, 1);
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            //Page.Response.Redirect(EDITSTAFFPRIVILEGEURL);
        }

        
    }
}
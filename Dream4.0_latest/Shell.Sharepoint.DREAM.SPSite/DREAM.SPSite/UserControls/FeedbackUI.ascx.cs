#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : FeedbackUI.cs
#endregion
using System;
using System.Collections;
using System.Web;
using System.Data;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Utilities;
using Microsoft.SharePoint;
using System.Net;


namespace Shell.SharePoint.DREAM.Site.UI.UserControls
{
    /// <summary>
    /// Code behind for Feedback. 
    /// </summary>
    public partial class FeedbackUI : UIHelper
    {
        #region Declaration
        Feedback objFeedback = new Feedback();
        AbstractController objController;
        const string PAGELEVELFEEDBACK = "Page Level Feedback";
        const string GENERALFEEDBACK = "General Feedback";
        const string SUCCESSMESSEGE = "Thank you for your feedback.Your feedback has been submitted successfully.";
        const string ERRORMESSEGE = "There was a problem in submitting your feedback.";
        const string FEEDBACKPAGENAMESLIST = "FeedBack Page Names";
        #endregion
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable objListData = null;
            DataRow objListRow;
            string strParentSiteURL = string.Empty;
            try
            {
                
                if (!IsPostBack)
                {
                    //The Attached file is placed in a session object.
                    if (Session["FileAttached"] != null)
                    {
                        Session["FileAttached"] = null;
                    }
                    objController = objFactory.GetServiceManager("MossService");
                    strParentSiteURL = SPContext.Current.Site.Url.ToString();
                    objListData = ((MOSSServiceManager)objController).ReadList(strParentSiteURL, FEEDBACKPAGENAMESLIST, string.Empty);
                    if (objListData.Rows.Count > 0)
                    {
                        //Loop through the values in the feedback page name List.
                        for (int index = 0; index < objListData.Rows.Count; index++)
                        {
                            objListRow = objListData.Rows[index];
                            ddlPageName.Items.Add(objListRow["Title"].ToString());
                        }
                    }
                    //this will check for page name query string and assigns the dropdown value.
                    if (this.Page.Request.QueryString["pagename"] != null)
                    {
                        if (ddlPageName.Items.FindByText(this.Page.Request.QueryString["pagename"]) != null)
                            ddlPageName.Items.FindByText(this.Page.Request.QueryString["pagename"]).Selected = true;
                        else
                            ddlPageName.SelectedIndex = 0;
                    }

                    rdoFeedback.Items[0].Selected = true;
                   
                }
            }
            catch (WebException webEx)
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = webEx.Message;
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
        }
        /// <summary>
        /// Handles the Click event of the cmdSubmit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void BtnSubmit_Click(object sender, EventArgs e)
        {            
            objController = objFactory.GetServiceManager("MossService");
            objUtility = new CommonUtility();
            try
            {
                bool blnIsSaved = false;
                string strUserId = objUtility.GetUserName();
                string strMessage = string.Empty;
                //set the values selected by the user.
                if (rdoRating.SelectedIndex != -1)
                {
                    objFeedback.Rating = rdoRating.SelectedItem.Text;
                }
                objFeedback.Reason = txtReasonForRating.Text.Trim();
                objFeedback.AdditionalInformation = txtAdditionalInformation.Text.Trim();

                objFeedback.PageName = ddlPageName.SelectedItem.Text;
                objFeedback.Comment = txtPageLevelComment.Text.Trim();
                //check the type of feedback has selected.
                if (string.Compare(rdoFeedback.SelectedItem.Text, PAGELEVELFEEDBACK) == 0)
                {
                    objFeedback.TypeofFeedback = PAGELEVELFEEDBACK;
                }
                else
                {
                    objFeedback.TypeofFeedback = GENERALFEEDBACK;
                }

                ///Check if the Session object for the uploaded file is not null.
                if (Session["FileAttached"] != null)
                {
                    objFeedback.FileAttached = (byte[])Session["FileAttached"];
                    objFeedback.FileName = hidFileName.Value;
                }
                //Enters the Feedback details into sharepoint list. 
                blnIsSaved = ((MOSSServiceManager)objController).UpdateFeedback(strUserId, objFeedback);
                if (blnIsSaved)
                {
                    lblMessage.Text = SUCCESSMESSEGE;
                    objUtility.SendAlertMailforNewFeedback(strMessage);
                }
                else
                {
                    lblMessage.Text = ERRORMESSEGE;
                }

                pnlConfirmFeedback.Visible = true;
                pnlFeedback.Visible = false;
            }
            catch (WebException webEx)
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = webEx.Message;
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ClearAttachedFile(object sender, EventArgs e)
        {
            Session.Remove("FileAttached");
            hidFileName.Value = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                Session.Remove("FileAttached");

                rdoRating.SelectedIndex = -1;
                txtReasonForRating.Text = string.Empty;
                txtAdditionalInformation.Text = string.Empty;

                hidFileName.Value = string.Empty;
                ddlPageName.SelectedIndex = -1;
                if (ddlPageName.Items.FindByText(this.Page.Request.QueryString["pagename"]) != null)
                    ddlPageName.Items.FindByText(this.Page.Request.QueryString["pagename"]).Selected = true;
                else
                    ddlPageName.SelectedIndex = 0;

                txtPageLevelComment.Text = string.Empty;

                if (rdoFeedback.SelectedIndex == 1)
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "loadOption", "showPageGeneralTable('TR1','TR2','TR3','TR4','TR5','TR6','TR7','TR8','TR9');", true);
                }
            }
            catch (WebException webEx)
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = webEx.Message;
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }

        }
     
    }
}
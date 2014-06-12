#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: PageComments.cs
#endregion

using System;
using System.Data;
using System.Web;
using System.Web.UI;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DWB.BusinessLogicLayer;
using Shell.SharePoint.DWB.Business.DataObjects;
using System.Net;

namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    public partial class PageComments : UIHelper
    {
        #region Declarations
        string strPageID = string.Empty;
        string strUserName = string.Empty;
        string strDiscipline = string.Empty;
        string strDisciplineID = string.Empty;
        const string strAddCommentsPageURL = "/Pages/AddComments.aspx";
#endregion 

        #region Protected Methods

        /// <summary>
        /// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"></see> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the server control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            pnlComment.Visible = false;
            if (HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES] != null &&
                    !((TreeNodeSelection)HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES]).IsChapterSelected &&
                    !((TreeNodeSelection)HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES]).IsBookSelected)
            {
                TreeNodeSelection objTreeNodeSelection = null;
                objTreeNodeSelection = (TreeNodeSelection)HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES];
                if (objTreeNodeSelection != null)
                {
                    strPageID = objTreeNodeSelection.PageID;
                }
                try
                {
                    strUserName = GetUserName();
                    UserRegistrationBLL objUserRegistrationBLL = new UserRegistrationBLL();
                    UserDetails objUserDetails = objUserRegistrationBLL.GetUserDesicipline(strParentSiteURL, strUserName, USERLIST);
                    if (objUserDetails != null)
                    {
                        strDiscipline = objUserDetails.Discipline;
                        strDisciplineID = objUserDetails.DisciplineID;
                    }

                    if (ShowButton(WELLBOOKVIEWERCONTROLCOMMENTS, string.Empty, string.Empty, string.Empty, string.Empty))
                    {
                        pnlComment.Visible = true;
                        /// Get the Comments for selected Page, User Discipline
                        GetPageComments(strPageID, strDiscipline, strUserName);
                    }
                }
                catch (WebException webEx)
                {
                    CommonUtility.HandleException(strParentSiteURL, webEx);
                    lblException.Text = webEx.Message;
                    lblException.Visible = true;
                    ExceptionBlock.Visible = true;
                }
                catch (Exception ex)
                {

                    CommonUtility.HandleException(strParentSiteURL, ex);

                }
                base.Render(writer);
            }
          
        }

        /// <summary>
        /// Save click Handler. 
        /// Saves the comment to SharePoint list and close the popup window.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">EventArg</param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            TreeNodeSelection objTreeNodeSelection = null;

            UserRegistrationBLL objUserRegistrationBLL = new UserRegistrationBLL();
            UserDetails objUserDetails = null;
            bool blnUpdateSuccess = false;
            try
            {
                objTreeNodeSelection = (TreeNodeSelection)HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES];
                if (objTreeNodeSelection != null)
                {
                    strPageID = objTreeNodeSelection.PageID;
                }
                strUserName = GetUserName();
                objUserDetails = objUserRegistrationBLL.GetUserDesicipline(strParentSiteURL, strUserName, USERLIST);
                if (objUserDetails != null)
                {
                    strDiscipline = objUserDetails.Discipline;
                    strDisciplineID = objUserDetails.DisciplineID;
                }
                /// Save the Comments to the DWB Comment list and refresh the parent window
                PageCommentsDetails objPageCommentsDetails = new PageCommentsDetails();
                objPageCommentsDetails.Comments = txtPageComments.Text.Trim();
                objPageCommentsDetails.Discipline = strDiscipline;
                objPageCommentsDetails.DisciplineID = strDisciplineID;
                objPageCommentsDetails.PageID = strPageID;
                objPageCommentsDetails.UserName = strUserName;
                if (chkShareComments.Checked)
                {
                    objPageCommentsDetails.ShareComments = STATUSTERMINATED;
                }
                else
                {
                    objPageCommentsDetails.ShareComments = STATUSACTIVE;
                }

                ListEntry objListEntry = new ListEntry();
                objListEntry.PageComments = objPageCommentsDetails;
                blnUpdateSuccess = UpdateListEntry(objListEntry, PAGESCOMMENTSLIST, CHAPTERPAGESMAPPINGAUDITLIST, PAGECOMMENTS, AUDITACTIONCOMMENTADDED);
                if (blnUpdateSuccess)
                {
                    txtPageComments.Text = string.Empty;
                }
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(strParentSiteURL, webEx);

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
        /// Gets the page comments.
        /// </summary>
        /// <param name="pageID">The page ID.</param>
        /// <param name="discipline">The discipline.</param>
        /// <param name="userName">Name of the user.</param>
        private void GetPageComments(string pageID,string discipline,string userName)
        {
            DataTable dtPageComments = new DataTable();
            string strCAMLQuery = string.Empty;

            try
            {

                strCAMLQuery = "<OrderBy>" +
        "<FieldRef Name='Created' Ascending='False' />" +
      "</OrderBy>" +
      "<Where>" +
        "<Or>" +
         " <And>" +
          "<And>" +
           "<And>" +
         " <Neq>" +
          "  <FieldRef Name='UserName' />" +
           " <Value Type='Text'>" + userName + "</Value>" +
         " </Neq>" +
          "  <Eq>" +
          "    <FieldRef Name='Discipline' />" +
          "    <Value Type='Lookup'>" + discipline + "</Value>" +
          "  </Eq>" +
         " </And>" +
          "  <Eq>" +
          "    <FieldRef Name='Shared' />" +
          "    <Value Type='Choice'>Yes</Value>" +
         "   </Eq>" +
        " </And> " +
        "    <Eq> " +
         "     <FieldRef Name='Page_ID' />" +
          "    <Value Type='Number'>" + pageID + "</Value> " +
          "  </Eq>" +
         " </And>" +
         " <And>" +
         "   <And>" +
          "    <Eq>" +
           "     <FieldRef Name='UserName' />" +
             "   <Value Type='Text'>" + userName + "</Value>" +
            "  </Eq>" +
            "  <Eq>" +
             "   <FieldRef Name='Page_ID' />" +
             "   <Value Type='Number'>" + pageID + "</Value>" +
            "  </Eq>" +
           " </And>" +
           " <Eq>" +
            "  <FieldRef Name='Discipline' />" +
             " <Value Type='Lookup'>" + discipline + "</Value>" +
           " </Eq>" +
         " </And>" +
       " </Or>    " +
     " </Where>";
                string strViewFields = string.Empty;
                strViewFields = @"<FieldRef Name='Page_ID'/><FieldRef Name='UserName'/><FieldRef Name='Shared'/><FieldRef Name='Discipline'/><FieldRef Name='Comment'/>";

                dtPageComments = GetListItems(PAGESCOMMENTSLIST, strCAMLQuery, strViewFields);

                grdAuditTrail.DataSource = dtPageComments;
                grdAuditTrail.DataBind();
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(strParentSiteURL, webEx);
                lblException.Text = webEx.Message;
                lblException.Visible = true;
                ExceptionBlock.Visible = true;
            }
            catch (Exception ex)
            {

                CommonUtility.HandleException(strParentSiteURL, ex);

            }
            finally
            {
                if(dtPageComments != null)
                    dtPageComments.Dispose();
            }
        }
        #endregion
    }
}
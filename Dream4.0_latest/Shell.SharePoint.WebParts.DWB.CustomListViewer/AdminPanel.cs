#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: AdminPanel.cs
#endregion
using System;
using System.Runtime.InteropServices;
using System.Web;
using System.Net;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Shell.SharePoint.DWB.BusinessLogicLayer;
using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DREAM.Utilities;


namespace Shell.SharePoint.WebParts.DWB.CustomListViewer
{
    /// <summary>
    /// This webpart used to display the list of Links 
    /// avilable to logged in user based on his privileges
    /// </summary>
    [Guid("43aefa45-c5f3-4978-a672-317e60282d88")]
    public class AdminPanel : System.Web.UI.WebControls.WebParts.WebPart
    {
        #region Declaration

        const string ASSETTEAMMAPPINGLIST = "Asset Team Mapping";
        const string PAGETITLE = "Manage eWB2";
        const string SYSTEMPRIVILEGES = "System Privileges";
        const string STAFFPRIVILEGES = "Staff Privileges";
        /// Initiliazing the controls 
        HyperLink linkMasterPageMaintenance;
        HyperLink linkTemplateMaintenance;
        HyperLink linkBookMaintenance;
        HyperLink linkUserRegistration;
        HyperLink linkTeamRegistration;

        System.Web.UI.WebControls.Image imgUserPermissionBullet;
        System.Web.UI.WebControls.Image imgMastePageMaintainenceBullet;
        System.Web.UI.WebControls.Image imgWellBookMaintainenceBullet;
        System.Web.UI.WebControls.Image imgTemplateMaintainenceBullet;
        System.Web.UI.WebControls.Image imgTeamMaintainenceBullet;

        /// Initialiazing the objects
        AdminBLL objAdminBLL;
        CommonUtility objUtility = new CommonUtility();
        /// Intializing variables
        bool blnMasterPageMaintenance;
        bool blnTemplateMaintenance;
        bool blnBookMaintenance;
        bool blnUserRegistration;
        bool blnTeamRegistration;
        bool blnPublishBook;
        string strUserId;
        Privileges objPrivileges;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the MasterPageMaintenance.
        /// </summary>
        /// <value>The name.</value>
        private bool MasterPageMaintenance
        {
            get
            {
                return blnMasterPageMaintenance;
            }
            set
            {
                blnMasterPageMaintenance = value;
            }
        }

        /// <summary>
        /// Gets or sets the TeamRegistration.
        /// </summary>
        /// <value>The name.</value>
        private bool TeamRegistration
        {
            get
            {
                return blnTeamRegistration;
            }
            set
            {
                blnTeamRegistration = value;
            }
        }

        /// <summary>
        /// Gets or sets the TemplateMaintenance.
        /// </summary>
        /// <value>The name.</value>
        private bool TemplateMaintenance
        {
            get
            {
                return blnTemplateMaintenance;
            }
            set
            {
                blnTemplateMaintenance = value;
            }
        }

        /// <summary>
        /// Gets/Sets user Maintain Book permission.
        /// </summary>
        /// <value>The name.</value>
        private bool BookMaintenance
        {
            get
            {
                return blnBookMaintenance;
            }
            set
            {
                blnBookMaintenance = value;
            }
        }

        /// <summary>
        /// Gets or sets the TemplateMaintenance.
        /// </summary>
        /// <value>The name.</value>
        private bool UserRegistration
        {
            get
            {
                return blnUserRegistration;
            }
            set
            {
                blnUserRegistration = value;
            }
        }

        /// <summary>
        /// Gets/Sets user Publish Book permission.
        /// </summary>
        private bool PublishBook
        {
            get
            {
                return blnPublishBook;
            }
            set
            {
                blnPublishBook = value;
            }
        }

        #endregion

        #region Overridden Methods
        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls 
        /// that use composition-based implementation to create any child controls they contain  
        /// in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            try
            {
                object objStoredPrivilege = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPrivileges.ToString());

                if (objStoredPrivilege != null)
                {
                    CommonUtility.RemoveSessionVariable(Page, enumSessionVariable.UserPrivileges.ToString());
                    GetUserPrivileges();
                    CommonUtility.SetSessionVariable(Page, enumSessionVariable.UserPrivileges.ToString(), objPrivileges);
                }
                else
                {
                    GetUserPrivileges();
                    CommonUtility.SetSessionVariable(Page, enumSessionVariable.UserPrivileges.ToString(), objPrivileges);
                }

                if (objPrivileges != null)
                {
                    SetControlPanelProperties();
                }

                CreateControlPanelLinks();
            }
            catch (WebException ex1)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex1);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
        }

        /// <summary>
        /// Gets the user priviledges from the list.
        /// </summary>
        private void GetUserPrivileges()
        {
            objAdminBLL = new AdminBLL();
            string strParentSiteUrl = HttpContext.Current.Request.Url.ToString();
            strUserId = objUtility.GetUserName();
            DataTable dtSystemPrivileges = objAdminBLL.GetDWBPrivileges(strParentSiteUrl, strUserId, SYSTEMPRIVILEGES);

            if (dtSystemPrivileges != null)
            {
                objPrivileges = objAdminBLL.SetPrivilegesObjects(strParentSiteUrl, dtSystemPrivileges);
            }
            else
            {
                objPrivileges = new Privileges();
                objPrivileges.IsNonDWBUser = true;
            }

            if (dtSystemPrivileges != null)
            {
                dtSystemPrivileges.Dispose();
            }
        }

        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            try
            {
                if (BookMaintenance || MasterPageMaintenance || TemplateMaintenance || UserRegistration || TeamRegistration || PublishBook)
                {

                    writer.Write("<link rel=\"stylesheet\" type=\"text/css\" href=\"/_layouts/DREAM/styles/DWBStyleSheetRel2_0.css\" />");
                    writer.Write("<SCRIPT type=\"text/javascript\" SRC=\"/_layouts/dream/Javascript/DWBJavascriptFunctionRel2_0.js\"></SCRIPT>");
                    RenderParentTable(writer, PAGETITLE);
                    writer.Write("<tr><td>");
                    writer.Write("</td></tr>");

                    if (BookMaintenance || PublishBook)
                    {
                        writer.Write("<tr><td>");
                        imgWellBookMaintainenceBullet.RenderControl(writer);
                        writer.Write("&nbsp;&nbsp;");
                        linkBookMaintenance.RenderControl(writer);
                        writer.Write("</td></tr>");
                    }

                    if (MasterPageMaintenance)
                    {
                        writer.Write("<tr><td>");
                        imgMastePageMaintainenceBullet.RenderControl(writer);
                        writer.Write("&nbsp;&nbsp;");
                        linkMasterPageMaintenance.RenderControl(writer);
                        writer.Write("</td></tr>");
                    }

                    if (TemplateMaintenance)
                    {
                        writer.Write("<tr><td>");
                        imgTemplateMaintainenceBullet.RenderControl(writer);
                        writer.Write("&nbsp;&nbsp;");
                        linkTemplateMaintenance.RenderControl(writer);
                        writer.Write("</td></tr>");
                    }

                    if (UserRegistration)
                    {
                        writer.Write("<tr><td>");
                        imgUserPermissionBullet.RenderControl(writer);
                        writer.Write("&nbsp;&nbsp;");
                        linkUserRegistration.RenderControl(writer);
                        writer.Write("</td></tr>");
                    }
                    if (TeamRegistration)
                    {
                        writer.Write("<tr><td>");
                        imgTeamMaintainenceBullet.RenderControl(writer);
                        writer.Write("&nbsp;&nbsp;");
                        linkTeamRegistration.RenderControl(writer);
                        writer.Write("</td></tr>");
                    }
                    writer.Write("</table>");
                }
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), webEx);
                writer.Write(webEx.Message);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }

        }


        #endregion

        #region Private Methods

        /// <summary>
        /// Renders the parent table.
        /// </summary>
        /// <param name="writer">The writer.</param>
        protected void RenderParentTable(HtmlTextWriter writer, string headerName)
        {
            writer.Write("<table class=\"DWBtableAdvSrchBorder\" cellspacing=\"0\" cellpadding=\"4\" border=\"0\" width=\"100%\">");
            //modified for eWB2 rebranding
            writer.Write("<tr><td class=\"DWBtdAdvSrchHeader\" colspan=\"4\" valign=\"top\"><B>" + headerName + "</b></td></tr>");
        }

        /// <summary>
        /// Sets the Properties for Showing the Links for the user
        /// </summary>
        /// <param name="objUserPriviliges">String array with the user privileges</param>
        private void SetControlPanelProperties()
        {

            if (objPrivileges != null)
            {
                if (objPrivileges.SystemPrivileges != null)
                {
                    if (objPrivileges.SystemPrivileges.AdminPrivilege)
                    {
                        UserRegistration = true;
                        TeamRegistration = true;
                        BookMaintenance = true;
                        MasterPageMaintenance = true;
                        TemplateMaintenance = true;
                        PublishBook = true;
                    }
                    else if (objPrivileges.SystemPrivileges.BookOwner)
                    {
                        TeamRegistration = true;
                        BookMaintenance = true;
                        PublishBook = true;

                        UserRegistration = false;
                        TemplateMaintenance = false;
                        MasterPageMaintenance = false;
                    }
                    else if (objPrivileges.SystemPrivileges.PageOwner || objPrivileges.SystemPrivileges.DWBUser)
                    {
                        if (objPrivileges.FocalPoint != null)
                        {
                            if (!string.IsNullOrEmpty(objPrivileges.FocalPoint.BookIDs))
                            {
                                BookMaintenance = true;
                            }
                            else
                            {
                                BookMaintenance = false;
                            }
                        }
                        UserRegistration = false;
                        TeamRegistration = false;
                        MasterPageMaintenance = false;
                        TemplateMaintenance = false;
                        PublishBook = false;
                    }
                }
                else if (objPrivileges.IsNonDWBUser)
                {
                    UserRegistration = false;
                    TeamRegistration = false;
                    BookMaintenance = false;
                    MasterPageMaintenance = false;
                    TemplateMaintenance = false;
                    PublishBook = false;
                }
            }
        }

        /// <summary>
        /// Creates the Control Panel Links
        /// </summary>
        private void CreateControlPanelLinks()
        {

            if (UserRegistration)
            {
                imgUserPermissionBullet = new System.Web.UI.WebControls.Image();
                imgUserPermissionBullet.ImageUrl = "/_layouts/DREAM/images/DWBAdminbullets.gif";
                imgUserPermissionBullet.ID = "ImageUserRegistrationBulletID";
                imgUserPermissionBullet.CssClass = "ImageUserRegistrationBulletCSS";
                this.Controls.Add(imgUserPermissionBullet);
                linkUserRegistration = new HyperLink();
                linkUserRegistration.ID = "linkUserRegistration";
                linkUserRegistration.CssClass = "DWBresultHyperLink";
              //  linkUserRegistration.NavigateUrl = "/Pages/UserRegistration.aspx";
                linkUserRegistration.NavigateUrl = "javascript:OpenPageInContentWindow('/Pages/UserRegistration.aspx');";
                linkUserRegistration.Target = "_self";
                linkUserRegistration.Text = "User Registration";
                this.Controls.Add(linkUserRegistration);
            }

            if (MasterPageMaintenance)
            {
                imgMastePageMaintainenceBullet = new System.Web.UI.WebControls.Image();
                imgMastePageMaintainenceBullet.ImageUrl = "/_layouts/DREAM/images/DWBAdminbullets.gif";
                imgMastePageMaintainenceBullet.ID = "ImageMasterPageMaintenanceID";
                imgMastePageMaintainenceBullet.CssClass = "ImageMasterPageMaintenanceCSS";
                this.Controls.Add(imgMastePageMaintainenceBullet);
                linkMasterPageMaintenance = new HyperLink();
                linkMasterPageMaintenance.ID = "linkMasterPageMaintenance";
                linkMasterPageMaintenance.CssClass = "DWBresultHyperLink";
                //linkMasterPageMaintenance.NavigateUrl = "/Pages/MaintainMasterPage.aspx";
                linkMasterPageMaintenance.NavigateUrl = "javascript:OpenPageInContentWindow('/Pages/MaintainMasterPage.aspx');";
                linkMasterPageMaintenance.Target = "_self";
                linkMasterPageMaintenance.Text = "Maintain Master Pages";
                this.Controls.Add(linkMasterPageMaintenance);
            }

            if (TemplateMaintenance)
            {
                imgTemplateMaintainenceBullet = new System.Web.UI.WebControls.Image();
                imgTemplateMaintainenceBullet.ImageUrl = "/_layouts/DREAM/images/DWBAdminbullets.gif";
                imgTemplateMaintainenceBullet.ID = "ImageTemplateMaintenanceID";
                imgTemplateMaintainenceBullet.CssClass = "ImageTemplateMaintenanceCSS";
                this.Controls.Add(imgTemplateMaintainenceBullet);
                linkTemplateMaintenance = new HyperLink();
                linkTemplateMaintenance.ID = "linkTemplateMaintenance";
                linkTemplateMaintenance.CssClass = "DWBresultHyperLink";
              //  linkTemplateMaintenance.NavigateUrl = "/Pages/TemplateMaintenance.aspx";
                linkTemplateMaintenance.NavigateUrl = "javascript:OpenPageInContentWindow('/Pages/TemplateMaintenance.aspx');";
                linkTemplateMaintenance.Target = "_self";
                linkTemplateMaintenance.Text = "Maintain Templates";
                this.Controls.Add(linkTemplateMaintenance);
            }

            if (BookMaintenance || PublishBook)
            {
                imgWellBookMaintainenceBullet = new System.Web.UI.WebControls.Image();
                imgWellBookMaintainenceBullet.ImageUrl = "/_layouts/DREAM/images/DWBAdminbullets.gif";
                imgWellBookMaintainenceBullet.ID = "ImageWellBookMaintenanceID";
                imgWellBookMaintainenceBullet.CssClass = "ImageWellBookMaintenanceCSS";
                this.Controls.Add(imgWellBookMaintainenceBullet);
                linkBookMaintenance = new HyperLink();
                linkBookMaintenance.ID = "linkBookMaintenance";
                linkBookMaintenance.CssClass = "DWBresultHyperLink";
              //  linkBookMaintenance.NavigateUrl = "/Pages/BookMaintenance.aspx";
                linkBookMaintenance.NavigateUrl = "javascript:OpenPageInContentWindow('/Pages/BookMaintenance.aspx');";
                linkBookMaintenance.Target = "_self";
                linkBookMaintenance.Text = "Maintain Books";
                this.Controls.Add(linkBookMaintenance);
            }
            if (TeamRegistration)
            {
                imgTeamMaintainenceBullet = new System.Web.UI.WebControls.Image();
                imgTeamMaintainenceBullet.ImageUrl = "/_layouts/DREAM/images/DWBAdminbullets.gif";
                imgTeamMaintainenceBullet.ID = "ImageTeamRegistrationBulletID";
                imgTeamMaintainenceBullet.CssClass = "ImageTeamRegistrationBulletCSS";
                this.Controls.Add(imgTeamMaintainenceBullet);
                linkTeamRegistration = new HyperLink();
                linkTeamRegistration.ID = "linkTeamRegistrationID";
                linkTeamRegistration.CssClass = "DWBresultHyperLink";
               //linkTeamRegistration.NavigateUrl = "/Pages/TeamRegistration.aspx";
                linkTeamRegistration.NavigateUrl = "javascript:OpenPageInContentWindow('/Pages/TeamRegistration.aspx');";
                linkTeamRegistration.Target = "_self";
                linkTeamRegistration.Text = "Team Registration";
                this.Controls.Add(linkTeamRegistration);
            }
        }
        #endregion
    }
}

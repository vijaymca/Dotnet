#region Shell Copyright.2011
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: SuspendAccess.aspx.cs
#endregion
using System;
using Microsoft.SharePoint;
using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.DREAM.Site.UI
{
    /// <summary>
    /// This is used to redirect user when site is under maintainance
    /// </summary>
    public partial class SuspendAccess :System.Web.UI.Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                /// Set Region Title
                lblRegionTitle.Text = GetRegionTitle();
                lblMessage.Text = GetSiteMaintainanceMessage();
                this.Page.Title = lblRegionTitle.Text;
            }
            catch
            {
                lblRegionTitle.Text = "Data Retrieval EPICURE Application";
                lblMessage.Text = "Site is under maintainance. Please try later.";
                this.Page.Title = lblRegionTitle.Text;
            }
        }

        /// <summary>
        /// Gets the region title.
        /// </summary>
        /// <returns></returns>
        private string GetRegionTitle()
        {
            string strRegionTitle = "Data Retrieval EPICURE Application";
            string strUrl = string.Empty;
            if(SPContext.Current != null)
            {
                strUrl = SPContext.Current.Site.Url;
                using(SPSite spSite = new SPSite(strUrl))
                {
                    using(SPWeb spWeb = spSite.OpenWeb())
                    {
                        if(spWeb != null)
                        {
                            strRegionTitle = spWeb.Title;
                        }
                    }
                }
            }
            return strRegionTitle;
        }
        /// <summary>
        /// Gets the site maintainance message.
        /// </summary>
        /// <returns></returns>
        private string GetSiteMaintainanceMessage()
        {
            string strMessage = PortalConfiguration.GetInstance().GetKey("SiteMaintenanceMessage");
            return strMessage;
        }
    }
}

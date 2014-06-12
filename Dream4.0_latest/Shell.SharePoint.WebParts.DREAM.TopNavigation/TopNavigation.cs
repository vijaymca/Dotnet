#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : Topnavigation.cs
#endregion
using System;
using System.Runtime.InteropServices;
using System.Web;
using System.Collections;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing.Navigation;
using Microsoft.SharePoint.WebControls;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.MOSSProcess;

namespace Shell.SharePoint.WebParts.DREAM.TopNavigation
{
    /// <summary>
    ///     This common webpart[TopNavigation] will be used on the 
    ///     top part across the portal. This section contains Welcome note, 
    ///     links to following sections:
    ///     Admin, All-Dreams, Display, Preferences and Help links.
    /// </summary>
    [Guid("39e1c4c6-9ddb-49d8-91eb-1f6dcd6a0063")]
    public class TopNavigation : System.Web.UI.WebControls.WebParts.WebPart
    {
        #region Declaration
        const string HELPTOOLTIP = "User Help";
        const string HELPLINK = "/pages/HelpScreen.aspx";
        const string ADMINAPPROVEDREQLINK = "/Pages/ViewApprovedRequest.aspx";
        const string ADMINREJECTEDREQLINK = "/Pages/ViewRejectedRequest.aspx";
        const string ADMINPENDINGREQLINK = "/Pages/ViewPendingRequest.aspx";
        const string FEEDBACKLIST = "Feedback";
        const string ALLDREAMSLIST = "All Dreams";
        const string SYSTEMLINKSLIST = "System Defined Links";
        const string USERDEFINEDLINKSLIST = "User Defined Links";
        UserPreferences objUserPreferences;
        #endregion

        #region Protected Methods
        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            #region Method Variables
            AbstractController objMossController = null;
            String strUserID = string.Empty;
            String strParentSiteURL = string.Empty;
            #endregion
            try
            {
                SetUserPreference();
                ServiceProvider objFactory = new ServiceProvider();
                objMossController = objFactory.GetServiceManager("MossService");

                strUserID = HttpContext.Current.User.Identity.Name.ToString();
                strParentSiteURL = SPContext.Current.Site.Url.ToString();
                writer.Write("<table cellspacing=\"0\" cellpadding=\"0\">");
                writer.Write("<tr><td class=\"ms-globallinks\">");
                writer.Write("<span style='padding-left:3px'></span>");

                //display Welcome Note                
                writer.Write("Welcome " + strUserID);
                writer.Write(GetTopNavtext());

                //display Admin link to Administrator alone..                
                if (((MOSSServiceManager)objMossController).IsAdmin(strParentSiteURL,strUserID))
                {
                    writer.Write("<a href=\"#\" id=\"adminLink\" onmouseover=\"showAllDREAMMenu('divAdminMenu','visible','block','adminLink');\"  onmouseout=\"showAllDREAMMenu('divAdminMenu','hidden','none','adminLink');\" title=\"Admin\">Admin</a>");
                    writer.Write("<span style='padding-left:4px;padding-right:3px'>|</span>");
                }
                //Displaying the Admin Menu.
                DisplayAdminMenu(writer);
                writer.Write("</td><td class=\"ms-globallinks\"><span style='padding-left:3px'></span>");

                /*Displaying utility link*/
                writer.Write("<a href=\"#\" id=\"utilityLink\" onmouseover=\"showAllDREAMMenu('divUtilityLinksMenu','visible','block','utilityLink');\"  onmouseout=\"showAllDREAMMenu('divUtilityLinksMenu','hidden','none','utilityLink');\" title=\"Utilities\">Utilities</a>");
                writer.Write("<span style='padding-left:4px;padding-right:3px'>|</span>");
                DisplayUtilityLinksMenu(writer);
                writer.Write("</td><td class=\"ms-globallinks\"><span style='padding-left:3px'></span>");
                writer.Write("<a href=\"#\" id=\"userLink\" onmouseover=\"showAllDREAMMenu('divUserLinksMenu','visible','block','userLink');\"  onmouseout=\"showAllDREAMMenu('divUserLinksMenu','hidden','none','userLink');\" title=\"My Links\">My Links</a>");
                writer.Write("<span style='padding-left:4px;padding-right:3px'>|</span>");
                DisplayUserLinksMenu(writer, strParentSiteURL);
                writer.Write("</td><td class=\"ms-globallinks\"><span style='padding-left:3px'></span>");
                //** Commented in DREAM 3.1 as per requirement
                //writer.Write("<a href=\"#\" id=\"SystemLink\" onmouseover=\"showAllDREAMMenu('divSystemLinksMenu','visible','block','SystemLink');\"  onmouseout=\"showAllDREAMMenu('divSystemLinksMenu','hidden','none','SystemLink');\" title=\"Shared Links\">Shared Links</a>");
              //  writer.Write("<span style='padding-left:4px;padding-right:3px'>|</span>");
               // DisplaySystemLinksMenu(writer, strParentSiteURL);
               // writer.Write("</td><td class=\"ms-globallinks\"><span style='padding-left:3px'></span>");
                writer.Write("<a href=\"#\" id=\"allDreamsLink\" onmouseover=\"showAllDREAMMenu('divAllDreamMenu','visible','block','allDreamsLink');\"  onmouseout=\"showAllDREAMMenu('divAllDreamMenu','hidden','none','allDreamsLink');\">All DREAMs</a>");
                writer.Write("<span style='padding-left:4px;padding-right:3px'>|</span>");
                //Displays All DREAMs menu.
                DisplayAllDreamsMenu(writer, strParentSiteURL);
                writer.Write("</td><td class=\"ms-globallinks\"><span style='padding-left:3px'></span>");
                writer.Write("<a title='" + HELPTOOLTIP + "' href=\"javascript:OpenPopup('" + HELPLINK + "','Help','width=1024, height=650,scrollbars=yes,resizable');\"><img src=\"/_layouts/DREAM/images/help.gif\" align=\"absmiddle\" border=\"0\"/></a>");
                writer.Write("</td></tr></table>");
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the top navtext. This is used to render the span text whereever required,
        /// </summary>
        /// <returns></returns>
        private string GetTopNavtext()
        {
            String topNavText = "<span style='padding-left:4px;padding-right:3px'>|</span>"
                               + "</td><td class=\"ms-globallinks\">"
                               + "<span style='padding-left:3px'></span>";
            return topNavText;
        }
        /// <summary>
        /// Gets the Admin Menu. This is used to render the Admin on mouse over Menu.
        /// </summary>
        /// <returns></returns>
        private void DisplayAdminMenu(HtmlTextWriter writer)
        {
            #region DREAM 4.0 AJAX Modifications
            writer.Write("<DIV id=\"divAdminMenu\" style=\"position:absolute; visibility:hidden;display:none;z-index: 101; width: 150px;text:align:left;\" onmouseover=\"showAllDREAMMenu('divAdminMenu','visible','block','adminLink');\"  onmouseout=\"showAllDREAMMenu('divAdminMenu','hidden','none','adminLink');\">");
            writer.Write("<table id=\"DREAMTopNav\" cellpadding=\"3\" cellspacing=\"1\" border=\"0\" style=\"background-color:#7E9DAD;width:120px;margin-top:0px;\">");
            writer.Write("<tr><td nowrap style=\"text-align:left;background-color:#FAFAFA\"><a href=\"" + ADMINAPPROVEDREQLINK + "\" >View Approved Request</a></td></tr>");
            writer.Write("<tr><td style=\"text-align:left;background-color:#FAFAFA\"><a href=\"" + ADMINREJECTEDREQLINK + "\" >View Rejected Request</a></td></tr>");
            writer.Write("<tr><td style=\"text-align:left;background-color:#FAFAFA\"><a href=\"" + ADMINPENDINGREQLINK + "\" >View Pending Request</a></td></tr>"); 
            #endregion
            //this will generate the Feedback List URL.
            string strSiteURL = SPContext.Current.Site.Url.ToString();
            string strFeedbackListURL = strSiteURL + "/Lists/" + FEEDBACKLIST + "/AllItems.aspx";
            writer.Write("<tr><td style=\"text-align:left;background-color:#FAFAFA\"><a href=\"" + strFeedbackListURL + "\">View Feedback</a></td></tr>");
            writer.Write("<tr><td style=\"text-align:left;background-color:#FAFAFA\"><a href=\"#\" onclick=\"javascript:OpenPopup('/Pages/SyncBasinData.aspx','SyncBasinList','width=600, height=350, left=200, top=100, screenX=100, screenY=100, menubar=no, status=yes, toolbar=no')\">Sync.PowerHub</a></td></tr>");
            writer.Write("<tr><td style=\"text-align:left;background-color:#FAFAFA\"><a href=\"#\" onclick=\"javascript:OpenPopup('/_layouts/DREAM/RegionFormatSettings.aspx','RegionSettings')\">Regional Date Format</a></td></tr>");
            //Added in Dream 2.1
            //Start
            writer.Write("<tr><td style=\"text-align:left;background-color:#FAFAFA\"><a href=\"#\" onclick=\"javascript:OpenPopup('/lists/PVT Report/allitems.aspx','PVTReport')\">EP Catalog Searches</a></td></tr>");
            writer.Write("<tr><td style=\"text-align:left;background-color:#FAFAFA\"><a href=\"#\" onclick=\"javascript:OpenPopup('/Pages/FunctionalityUsage.aspx?type=simple','ReportUsageAnalysis','width=800, height=600, left=100, top=100, screenX=100, screenY=100, menubar=no, status=yes, toolbar=no,resizable=yes')\">Report Usage Analysis</a></td></tr>");
            //End
            #region DREAM 4.0 AJAX Modifications
            writer.Write("<tr><td style=\"text-align:left;background-color:#FAFAFA\"><a href=\"#\" onclick=\"javascript:OpenPageInContentWindow('/Pages/TeamManagement.aspx');\">Team Management</a></td></tr>");
            writer.Write("<tr><td style=\"text-align:left;background-color:#FAFAFA\"><a href=\"#\" onclick=\"javascript:OpenPopup('/Pages/rolesprofiles.aspx','rolesprofiles','')\">Roles And Profiles</a></td></tr>");
            #endregion 
            writer.Write("</table>");
            writer.Write("</DIV>");
        }
        /// <summary>
        /// Gets the All Dreams Menu. This is used to render the All DREAMS Section.
        /// </summary>
        /// <returns></returns>
        private void DisplayAllDreamsMenu(HtmlTextWriter writer, string parentSiteURL)
        {
            writer.Write("<DIV id=\"divAllDreamMenu\" style=\"position:absolute; visibility:hidden;display:none;z-index: 101; width: 150px;text:align:left;\" onmouseover=\"showAllDREAMMenu('divAllDreamMenu','visible','block','allDreamsLink');\"  onmouseout=\"showAllDREAMMenu('divAllDreamMenu','hidden','none','allDreamsLink');\">");
            writer.Write("<table id=\"DREAMTopNav\" cellpadding=\"2\" cellspacing=\"1\" border=\"0\" style=\"background-color:#7E9DAD;width:90px;margin-top:0px;\">");
            writer.Write(GetCachedTopNavigationItems(parentSiteURL, ALLDREAMSLIST, string.Empty));
            writer.Write("</table>");
            writer.Write("</DIV>");
        }
        /// <summary>
        /// Gets the User Links Menu. This is used to render the User Links Section.
        /// </summary>
        /// <returns></returns>
        private void DisplayUserLinksMenu(HtmlTextWriter writer, string parentSiteURL)
        {
            writer.Write("<DIV id=\"divUserLinksMenu\" style=\"position:absolute; visibility:hidden;display:none;z-index: 101; width: 150px;text:align:left;\" onmouseover=\"showAllDREAMMenu('divUserLinksMenu','visible','block','userLink');\"  onmouseout=\"showAllDREAMMenu('divUserLinksMenu','hidden','none','userLink');\">");
            writer.Write("<table id=\"DREAMTopNav\" cellpadding=\"2\" cellspacing=\"1\" border=\"0\" style=\"background-color:#7E9DAD;width:90px;margin-top:0px;\">");
            writer.Write(GetCachedUserLinkItems(parentSiteURL, USERDEFINEDLINKSLIST, string.Empty));
            writer.Write("</table>");
            writer.Write("</DIV>");
        }
        /// <summary>
        /// Displays the utility links menu.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void DisplayUtilityLinksMenu(HtmlTextWriter writer)
        {
            string[] arrTitle = { "Resource Status", "Data Owner", };
            string[] arrURL = { "/Pages/ResourceStatus.aspx", "/Pages/DataOwner.aspx" };

            writer.Write("<DIV id=\"divUtilityLinksMenu\" style=\"position:absolute; visibility:hidden;display:none;z-index: 101; width: 150px;text:align:left;\" onmouseover=\"showAllDREAMMenu('divUtilityLinksMenu','visible','block','utilityLink');\"  onmouseout=\"showAllDREAMMenu('divUtilityLinksMenu','hidden','none','utilityLink');\">");
            writer.Write("<table id=\"DREAMTopNav\" cellpadding=\"2\" cellspacing=\"1\" border=\"0\" style=\"background-color:#7E9DAD;width:90px;margin-top:0px;\">");

            for (int count = 0; count < arrTitle.Length; count++)
            {
                writer.Write("<tr><td nowrap style=\"text-align:left;background-color:#FAFAFA\" onmouseover=\"this.style.background-color:#8CCCD9\" onmouseout=\"this.style.background-color:#FFFFFF\">");

                writer.Write("<a class='listLink' href=\"javascript:OpenPopup('" + arrURL[count] + "','AllDreams','width=800, height=600,scrollbars=yes,resizable');\">" + arrTitle[count] + "</a>");
                writer.Write("</td></tr>");
            }
            writer.Write("</table>");
            writer.Write("</DIV>");
        }

        ///// <summary>
        ///// Gets the System Links Menu. This is used to render the System Links Section.
        ///// </summary>
        ///// <returns></returns>
        //private void DisplaySystemLinksMenu(HtmlTextWriter writer, string parentSiteURL)
        //{
        //    writer.Write("<DIV id=\"divSystemLinksMenu\" style=\"position:absolute; visibility:hidden;display:none;z-index: 101; width: 150px;text:align:left;\" onmouseover=\"showAllDREAMMenu('divSystemLinksMenu','visible','block','SystemLink');\"  onmouseout=\"showAllDREAMMenu('divSystemLinksMenu','hidden','none','SystemLink');\">");
        //    writer.Write("<table id=\"DREAMTopNav\" cellpadding=\"2\" cellspacing=\"1\" border=\"0\" style=\"background-color:#7E9DAD;width:90px;margin-top:0px;\">");
        //    writer.Write(GetCachedTopNavigationItems(parentSiteURL, SYSTEMLINKSLIST, string.Empty));
        //    writer.Write("</table>");
        //    writer.Write("</DIV>");
        //}
        /// <summary>
        /// Gets the cached left nav items.
        /// </summary>
        /// <param name="strParentSiteURL">The parent site URL.</param>
        /// <param name="strListName">Name of the list.</param>
        /// <param name="strCAMLQuery">CAML query.</param>
        /// <returns>Leftnav Links</returns>
        private string GetCachedTopNavigationItems(string parentSiteURL, string listName, string camlQuery)
        {
            StringBuilder strCachedData = new StringBuilder();
            string strTitleInSPList = string.Empty;
            string strURLInSPList = string.Empty;
            PortalSiteMapProvider objPortalSite = PortalSiteMapProvider.WebSiteMapProvider;
            SPSecurity.RunWithElevatedPrivileges(delegate()
               {
                   using (SPSite site = new SPSite(parentSiteURL))
                   {
                       using (SPWeb web = site.OpenWeb())
                       {
                           SPQuery query = new SPQuery();
                           query.Query = camlQuery;
                           PortalWebSiteMapNode pNode = (PortalWebSiteMapNode)objPortalSite.FindSiteMapNode(web.ServerRelativeUrl);
                           SiteMapNodeCollection listItems = objPortalSite.GetCachedListItemsByQuery(pNode, listName, query, web);
                           foreach (PortalListItemSiteMapNode listItem in listItems)
                           {
                               if (listItem.Title.Length > 0)
                               {
                                   strCachedData.Append("<tr><td style=\"text-align:left;background-color:#FAFAFA\" onmouseover=\"this.style.background-color:#8CCCD9\" onmouseout=\"this.style.background-color:#FFFFFF\">");
                                   strTitleInSPList = listItem["Title"].ToString();
                                   if (listItem["URL"] != null)
                                   {
                                       strURLInSPList = listItem["URL"].ToString();
                                   }
                                   else
                                   {
                                       strURLInSPList = " ";
                                   }
                                   if (!string.IsNullOrEmpty(strURLInSPList.Trim()))
                                   {
                                       strCachedData.Append("<a class='listLink' href=\"javascript:OpenPopup('" + strURLInSPList + "','AllDreams','');\">" + strTitleInSPList + "</a>");
                                   }
                                   else
                                   {
                                       strCachedData.Append("<span style='color:#333333;'>" + strTitleInSPList + "</span>");
                                   }
                               }
                               else
                               {
                                   strCachedData.Append(string.Empty);
                               }
                           }
                       }
                   }
               });
            return strCachedData.ToString();
        }
        /// <summary>
        /// Gets the cached left nav items.
        /// </summary>
        /// <param name="strParentSiteURL">The parent site URL.</param>
        /// <param name="strListName">Name of the list.</param>
        /// <param name="strCAMLQuery">CAML query.</param>
        /// <returns>Leftnav Links</returns>
        private string GetCachedUserLinkItems(string parentSiteURL, string listName, string camlQuery)
        {
            StringBuilder strCachedData = new StringBuilder();
            string strTitleInSPList = string.Empty;
            string strURLInSPList = string.Empty;
            PortalSiteMapProvider objPortalSite = PortalSiteMapProvider.WebSiteMapProvider;

            string strUserName = HttpContext.Current.User.Identity.Name.ToString();
            string[] arrUserName = new string[3];
            strUserName = strUserName.Replace("\\", "|");
            arrUserName = strUserName.Split('|');

            SPSecurity.RunWithElevatedPrivileges(delegate()
               {
                   using (SPSite site = new SPSite(parentSiteURL))
                   {
                       using (SPWeb web = site.OpenWeb())
                       {
                           SPQuery query = new SPQuery();
                           query.Query = camlQuery;
                           PortalWebSiteMapNode pNode = (PortalWebSiteMapNode)objPortalSite.FindSiteMapNode(web.ServerRelativeUrl);
                           SiteMapNodeCollection listItems = objPortalSite.GetCachedListItemsByQuery(pNode, listName, query, web);
                           foreach (PortalListItemSiteMapNode listItem in listItems)
                           {
                               if (listItem.Title.Length > 0)
                               {
                                   if (string.Equals(listItem["UserID"].ToString(), arrUserName[arrUserName.Length - 1].ToString()))
                                   {
                                       strCachedData.Append("<tr><td style=\"text-align:left;background-color:#FAFAFA\" onmouseover=\"this.style.background-color:#8CCCD9\" onmouseout=\"this.style.background-color:#FFFFFF\">");
                                       strTitleInSPList = listItem["Title"].ToString();
                                       if (listItem["URL"] != null)
                                       {
                                           strURLInSPList = listItem["URL"].ToString();
                                       }
                                       else
                                       {
                                           strURLInSPList = " ";
                                       }
                                       if (!string.IsNullOrEmpty(strURLInSPList.Trim()))
                                       {
                                           strCachedData.Append("<a class='listLink' href=\"javascript:OpenPopup('" + strURLInSPList + "','AllDreams','width=1024, height=650,scrollbars=yes,resizable');\">" + strTitleInSPList + "</a>");
                                       }
                                       else
                                       {
                                           strCachedData.Append("<span style='color:#333333;'>" + strTitleInSPList + "</span>");
                                       }
                                   }
                               }
                               else
                               {
                                   strCachedData.Append(string.Empty);
                               }
                           }
                       }
                   }
               });
            return strCachedData.ToString();
        }
        /// <summary>
        /// Sets the user preference.
        /// </summary>
        private void SetUserPreference()
        {
            object objSessionUserPreference = null;
            objUserPreferences = new UserPreferences();

            objSessionUserPreference = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString());
            //sets the user preferences object from session.
            if (objSessionUserPreference == null)
            {
                InitializeUserPreference();
                objSessionUserPreference = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString());
                objUserPreferences = (UserPreferences)objSessionUserPreference;
            }
            else
            {
                objUserPreferences = (UserPreferences)objSessionUserPreference;
            }

        }
        /// <summary>
        /// Initializes the user preference.
        /// </summary>
        protected void InitializeUserPreference()
        {
            CommonUtility objCommonUtility = new CommonUtility();
            AbstractController objMossController = null;
            ServiceProvider objFactory = new ServiceProvider();
            string strCurrSiteUrl = SPContext.Current.Site.Url.ToString();
            try
            {
                objUserPreferences = new UserPreferences();
                string strUserId = objCommonUtility.GetUserName();
                objMossController = objFactory.GetServiceManager("MossService");
                //get the user prefrences.
                objUserPreferences = ((MOSSServiceManager)objMossController).GetUserPreferencesValue(strUserId, strCurrSiteUrl);
                CommonUtility.SetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString(), objUserPreferences);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
        }
        #endregion

    }
}

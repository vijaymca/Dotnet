#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: Leftnavigation.cs
#endregion
using System;
using System.Runtime.InteropServices;
using System.Web;
using System.Data;
using System.Text;
using System.Xml;
using System.Web.UI;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing.Navigation;
using Microsoft.SharePoint.WebControls;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DREAM.Business.Entities;

namespace Shell.SharePoint.WebParts.DREAM.LeftNavigation
{
    /// <summary>
    ///     This common webpart[LeftNavigation] will be used on the 
    ///     lefthand side across the portal. This section contains Quick
    ///     Search UI, link to Advanced Search, System Defined & 
    ///     User Defined links.
    /// </summary>
    /// /// /
    [Guid("43aefa45-c5f3-4978-a672-317e60282d59")]
    public class LeftNavigation :System.Web.UI.WebControls.WebParts.WebPart
    {
        #region Declaration
        const string ACCESSDENIEDPAGE = "/_layouts/dream/AccessApproval.aspx";
        const string SUSPENDACCESSPAGE = "/_layouts/dream/SuspendAccess.aspx";
        const string ASSETTYPELIST = "Asset Type";
        const string COUNTRYLIST = "Country";
        const string SYSTEMLINKSLIST = "System Defined Links";
        const string SEARCHCONTEXTMENU = "Search Context Menu";
        const string USERDEFINEDLINKSLIST = "User Defined Links";
        const string SEARCHCONTEXTTITLE = "Context Search";
        const string WELLITEMVAL = "Well";
        const string WELLBOREITEMVAL = "Wellbore";
        const string PARSITEMVAL = "Project Archives";
        const string BASINITEMVAL = "Basin";
        const string BASINADVITEMVAL = "BasinSearch";
        const string FIELDITEMVAL = "Field";
        const string LOGSBYWELLWELLBORE = "Logs by Well Wellbore";
        const string REPORTSERVICECOLUMNLIST = "Report Service Layer Columns";
        const string WELLWELLBOREVAL = "Well Wellbore";
        const string PICKSVAL = "Picks";
        const string PARSVAL = "PARSADVSEARCH";
        const string FIELDVAL = "Field Advance Search";
        const string MAPWELLLAYER = "WELL";
        const string MAPWELLBORELAYER = "WELLBORE";
        const string MAPBASINLAYER = "BASIN";
        const string MAPFIELDLAYER = "FIELD";
        const string MAPPARSLAYER = "PROJECT ARCHIVES";
        const string USERPREFERENCES = "UserPreferences";
        const string SEARCHTYPE = "SearchType";
        const string ASSETQUERYSTRING = "asset";
        const string COUNTRYQUERYSTRING = "country";
        const string COLUMNQUERYSTRING = "column";
        const string WELLADVANCESEARCH = "Well Advance Search";
        const string WELLBOREADVANCESEARCH = "Wellbore Advance Search";
        const string BASINADVANCESEARCH = "Basin Advance Search";
        const string ANYCOUNTRYTEXT = "--Any Country--";
        const string ANYCOUNTRYVALUE = "0";
        const string ALLCOLUMNSTEXT = "--All Columns--";
        const string SELECTCOLUMNTEXT = "--Select Column--";
        const string QUICKSEARCHRESULTSPAGE = "QuickSearchResults.aspx";
        const string LISTOFWELLSITEM = "listofwells";
        const string LISTOFWELLBORESITEM = "listofwellbores";
        const string IWELLFILE = "iwellfile";
        const string IWELLFILEURL = "iWellFileURL";
        const string PVTREPORTLIST = "EP Catalog Report";

        #region SRP Changes
        const string RESERVOIRITEMVAL = "Reservoir";
        const string RESERVOIRADVSEARCH = "Reservoir Advance Search";
        const string LISTOFRESERVOIRSITEM = "listofreservoirs";
        const string LISTOFFIELDSITEM = "listoffields";
        const string CONTEXTSEARCHDOCLIB = "Context Search XML";
        #endregion
        /// <summary>
        /// Initializing the controls     
        /// </summary>    
        DropDownList cboQuickCountry = new DropDownList();
        DropDownList cboQuickAsset = new DropDownList();
        DropDownList cboQuickColumn;
        TextBox txtSearchCriteria = new TextBox();
        Button btnGo = new Button();

        /// <summary>
        ///  Initializing the Variables and Objects
        /// </summary>

        CommonUtility objUtility = new CommonUtility();
        UserPreferences objUserPreferences;
        AbstractController objController;
        string strSelectedCountry = string.Empty;
        string strCamlQuery = string.Empty;
        string strParentSiteURL = string.Empty;
        int intCountryIndex;
        int intAssetIndex;
        int intIWellIndex;
        //Added in Dream 4.0 Ajax
        UpdatePanel updtPanelLeftNaV;
        #endregion

        #region Protected Methods

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            //Dream 4.0 Ajax change starts
            //Fix for the UpdatePanel postback behaviour.
            EnsurePanelFix();
            RenderBusyBox();
            //Dream 4.0 Ajax change ends
            ServiceProvider objFactory = new ServiceProvider();
            objController = objFactory.GetServiceManager("MossService");
            strParentSiteURL = SPContext.Current.Site.Url.ToString();
            //Initializing Variables
            DataTable objListData = null;
            String strCAMLQuery = string.Empty;
            try
            {
                CreateUpdatePanel();
                CommonUtility.SetSessionVariable(Page, enumSessionVariable.IsDisplayContextSearch.ToString(), false);
                if(!UserValidation.ValidateUser())
                {
                    this.Context.Response.Redirect(ACCESSDENIEDPAGE, false);
                }

                if(UserValidation.IsSiteUnderMaintenance())
                {
                    /// Redirect to static page which should display the maintenance message
                    this.Context.Response.Redirect(SUSPENDACCESSPAGE, false);
                }
                //Initialize or get user preference values
                InitializeUserPreference();
                object objUserSessionPreferences = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString());
                if(objUserSessionPreferences != null)
                {
                    objUserPreferences = (UserPreferences)objUserSessionPreferences;
                }
                //Populating Country Drop Down
                strCAMLQuery = "<OrderBy><FieldRef Name=\"Title\" /></OrderBy><Where><Eq><FieldRef Name=\"Active\" /><Value Type=\"Choice\">Yes</Value></Eq></Where>";
                RenderQuickSearchCountry(objUserPreferences, strParentSiteURL, COUNTRYLIST, strCAMLQuery);
                //Populating Asset Drop Down
                strCAMLQuery = "<Where><Eq><FieldRef Name=\"IsActiveReportService\" /><Value Type=\"Boolean\">1</Value></Eq></Where>";
                RenderQuickSearchAsset(objUserPreferences, strParentSiteURL, ASSETTYPELIST, strCAMLQuery);
                //Creating Column Drop Down
                CreateQuickSearchColumnDDL();
                //Populating Column Drop Down
                PopulateQuickSearchColumnDDL();
                //Render Text Criteria Text Box
                RenderQuickSearchCriteriaField();
                RenderQuickSearchGoButton();
                this.Controls.Add(updtPanelLeftNaV);
                RenderLeftNav();
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
            finally
            {
                if(objListData != null)
                    objListData.Dispose();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            try
            {
                RenderAdvancedSearchLayout();
                updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("</table></td></tr></table>"));
                //sets the selected country value.
                strSelectedCountry = cboQuickCountry.SelectedValue;
                if(ScriptManager.GetCurrent(this.Page).IsInAsyncPostBack)
                {
                    if(GetAsyncPostBackControlID().Contains(btnGo.ID))
                        RenderLeftNavigationLinks(cboQuickAsset.SelectedItem.Text);
                    else if(GetAsyncPostBackControlID().Contains(updtPanelLeftNaV.ID))
                    {
                        string strArgument = this.Page.Request.Params.Get("__EVENTARGUMENT");
                        string strAsset = GetAssetForListSearch(strArgument);
                        if(!string.IsNullOrEmpty(strAsset))
                        {
                            RenderLeftNavigationLinks(strAsset);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Renders the left navigation to the specified HTML writer.
        /// </summary>
        /// 
        private void RenderLeftNav()
        {
            try
            {
                // Left Nav HTML rendering starts here.                
                GenerateInitialContent();
                RenderQuickSearchControls();
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
            finally
            {
                updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("<script language=\"javascript\">RegisterPageRequestManagerEventsForLeftNav();</script>"));
            }
        }
        /// <summary>
        /// Initializes the user preference.
        /// </summary>
        private void InitializeUserPreference()
        {
            objUserPreferences = new UserPreferences();
            string strUserId = objUtility.GetUserName();
            //get the user preferences.
            objUserPreferences = ((MOSSServiceManager)objController).GetUserPreferencesValue(strUserId, SPContext.Current.Site.Url.ToString());
            CommonUtility.SetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString(), objUserPreferences);
        }

        /// <summary>
        /// Gets the initial content.
        /// </summary>
        /// 
        private void GenerateInitialContent()
        {
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("<table id=\"tblLeftNavContainer\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" height=\"auto\">"));
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("<tr ><td class=\"LetNav\" valign=\"top\" bgcolor=\"#FCFEDF\">"));
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl(" <table width=\"100%\" border=\"0\" height=\"auto\" cellpadding=\"0\" cellspacing=\"0\" >"));
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("<tr><td style=\"height: 19px\" class=\"Shell_CompHeader\">Quick Search</td></tr>"));
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("<tr class=\"SubHeader\"><td style=\"padding-left:5px;\" valign=\"top\">"));
        }
        /// <summary>
        /// Renders the quick search controls.
        /// </summary>
        /// 
        private void RenderQuickSearchControls()
        {
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"padding-top:5px;\">"));
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("<tr  class=\"SubHeader\"><td colspan=\"2\">"));
            //Renders the Quick Search Controls like Country,Asset,Column and Criteria.
            cboQuickCountry.SelectedIndex = intCountryIndex;
            //Added in Dream 3.0 for Document asset
            //start
            if(cboQuickAsset.SelectedItem.Text.ToLowerInvariant().Equals("document"))
            {
                cboQuickCountry.SelectedIndex = 0;
                cboQuickCountry.Enabled = false;
            }
            //end
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(cboQuickCountry);
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("</td></tr><tr class=\"SubHeader\"><td colspan=\"2\">"));
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(cboQuickAsset);
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("</td></tr><tr class=\"SubHeader\"><td colspan=\"2\">"));
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(cboQuickColumn);
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("</td></tr><tr class=\"SubHeader\"><td>"));
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(txtSearchCriteria);
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("</td><td align=\"left\">"));
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(btnGo);
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("</td></tr></table></td></tr>"));
        }
        /// <summary>
        /// Renders the advanced search layout.
        /// </summary>
        /// 
        private void RenderAdvancedSearchLayout()
        {
            //Renders Advance Search and Map Search links.
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("<tr class=\"SubHeader\"><td style=\"padding-left:5px; height:25px;\">"));
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("<A class=\"LeftNavLinkTxt\" href=\"javascript:openAdvSearch('" + cboQuickAsset.SelectedValue + "');\">Adv. Search</A>&nbsp;"));
            string strIsMapApplicable = PortalConfiguration.GetInstance().GetKey("MapDisplay").ToUpper();
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("<A class=\"LeftNavLinkTxt\" href=\"javascript:openMapSearch('" + strIsMapApplicable + "');\">Map Search</A></td></tr>"));
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("<tr class=\"SubHeader\"><td  align=\"center\" style=\"padding-left:5px;padding-bottom:5px;\">"));
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("<A class=\"LeftNavLinkTxt\" href=\"javascript:openSpecialSearch('/pages/assettree.aspx');\">Asset Tree</A></td></tr>"));

            //Renders the Special Searches Standard Search links
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("<tr><td><DIV class=\"seperator\"></DIV>"));
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("<TABLE class=\"vertNav\" style=\"WIDTH: 100%; BORDER-COLLAPSE: collapse\" cellSpacing=0 cellPadding=0 border=0>"));
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("<TBODY><TR><TD class=inactive onclick=\"showMenuTable('tblAdvSearchmenu');\">"));
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("<DIV id=\"divSplSearch\" class=\"parent\" style=\"background-color:#ffeaa0;\">Special Searches</DIV>"));
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("<DIV id=\"tblAdvSearchmenu\" style=\"DISPLAY: none; background-color:#FCFEDF; margin-top:0px; z-index: 101; width:100%; height:auto;padding-right:0px; visibility:hidden;\"><TABLE id=\"tblSplSearchmenu\" style=\"WIDTH: 100%; height:auto; background-color:#FCFEDF; vertical-align:top; margin-top:0px;\" cellSpacing=\"0\" cellPadding=\"0\" border=\"0\"><TBODY>"));

            if(DisplayLogByFieldSearch())
            {
                updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("<TR class=\"lvl1\" id=\"logsFieldTR\" onmouseover=\"javascript:ChangeClassName(this,'lvl1over');\" onmouseout=\"javascript:ChangeClassName(this,'lvl1');\"><TD id=\"linkFieldDepth\"><DIV class=\"notparent\"><A href=\"#\" onclick=\"javascript:openLogsField('" + cboQuickAsset.SelectedItem.Value + "');\">Logs by Field / Depth</A></DIV></TD></TR>"));
            }
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("</TBODY></TABLE></DIV></TD></TR>"));
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("<TR class=\"lvl1\" id=\"querySearchTR\" onmouseover=\"javascript:ChangeClassName(this,'lvl1over');\" onmouseout=\"javascript:ChangeClassName(this,'lvl1');\"><TD style=\"background-color:#ffeaa0;\" id=\"linkQuerySearch\"><DIV class=\"notparent\" onclick=\"javascript:openQuerySearch();\">Query Search</DIV></TD></TR>"));
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("<TR class=\"lvl1\" id=\"standardSearchTR\" onmouseover=\"javascript:ChangeClassName(this,'lvl1over');\" onmouseout=\"javascript:ChangeClassName(this,'lvl1');\"><TD style=\"background-color:#ffeaa0;\" ><DIV class=\"notparent\" onclick=\"javascript:openStandardSearches('Save Search');\">Shared Searches</DIV></TD></TR>"));
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("<TR class=\"lvl1\" id=\"manageSearchTR\" onmouseover=\"javascript:ChangeClassName(this,'lvl1over');\" onmouseout=\"javascript:ChangeClassName(this,'lvl1');\"><TD style=\"background-color:#ffeaa0;\" ><DIV class=\"notparent\" onclick=\"javascript:openStandardSearches('Manage Search');\">My Searches</DIV></TD></TR>"));
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("</TBODY></TABLE></td></tr>"));
            updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("<tr><td height=\"10px\" bgcolor=\"#FCFEDF\"></td></tr>"));
        }
        /// <summary>
        /// Displays the log by field search.
        /// </summary>
        /// <returns></returns>
        private bool DisplayLogByFieldSearch()
        {
            bool blnStatus = false;
            string strLogByFieldStatus = PortalConfiguration.GetInstance().GetKey("ActivateLogsByFieldSearch");
            if((!string.IsNullOrEmpty(strLogByFieldStatus)) && (strLogByFieldStatus.ToLower().Equals("yes")))
            {
                blnStatus = true;
            }
            return blnStatus;
        }

        /// <summary>
        /// Renders the context search from list.
        /// </summary>
        /// <param name="assetType">Type of the asset.</param>
        /// <returns></returns>
        private StringBuilder RenderContextSearchFromList(string assetType)
        {
            StringBuilder strCachedData = new StringBuilder();
            SiteMapNodeCollection contextSearchItemCol = null;
            ArrayList arlGroupHeaders = null;
            int intHeaderIndex = 0;
            string strLinkName = string.Empty;
            string strLinkValue = string.Empty;
            string strToolTip = string.Empty;
            string strPageUrl = string.Empty;

            if(string.Equals(assetType, WELLBOREITEMVAL))
            {
                intIWellIndex = 4;
            }
            else if(string.Equals(assetType, BASINITEMVAL) || string.Equals(assetType, FIELDITEMVAL))
            {
                intIWellIndex = 1;
            }

            arlGroupHeaders = GetGroupHeaders(assetType);
            if(arlGroupHeaders != null && arlGroupHeaders.Count > 0)
            {
                strCachedData.Append("<table id=\"tblContextSearch\" class=\"vertNav\" style=\"display:none;WIDTH: 100%; BORDER-COLLAPSE: collapse;background-color:#ffeaa0;\" cellSpacing=0 cellPadding=0 border=0><TBODY>");

                foreach(string strGroupHeader in arlGroupHeaders)
                {
                    #region DREAM 4.0 Context search - ability to specify order of links
                    /// ====================================
                    /// Module Name:Context search - ability to specify order of links
                    /// Description:Context search - ability to specify order of links
                    /// Date:24-Jan-2011
                    /// Modified Lines:547-553
                    /// ====================================
                    string strCamlQuery = "<Where><And><Eq><FieldRef Name=\"Asset_x0020_Type\" /><Value Type=\"Lookup\">" + assetType + "</Value></Eq><And><Eq><FieldRef Name=\"Group_x0020_Header\" /><Value Type=\"Choice\">" + strGroupHeader + "</Value></Eq><Eq><FieldRef Name=\"Active\" /><Value Type=\"Boolean\">1</Value></Eq></And></And></Where><OrderBy><FieldRef Name=\"LinkOrder\" Ascending=\"True\" /><FieldRef Name=\"Title\" Ascending=\"True\" /></OrderBy>";
                    #endregion

                    contextSearchItemCol = GetContextSearchItemCollection(SEARCHCONTEXTTITLE, strCamlQuery);
                    if((contextSearchItemCol != null) && (contextSearchItemCol.Count > 0))
                    {
                        strCachedData.Append("<TR><TD class=\"inactive\">");
                        strCachedData.Append("<DIV id=\"div_" + intHeaderIndex + "\" class=\"parent\" onclick=\"HideExpandCntxtSrch('" + intHeaderIndex + "')\" style=\"font-size:11px\">" + strGroupHeader + "&nbsp;</DIV>");

                        strCachedData.Append("<DIV id=\"subMenu_" + intHeaderIndex + "\" style=\"font-size:10px;DISPLAY: none; background-color:#FCFEDF;margin-top:0px; z-index: 101; height:auto;width:100%; visibility:hidden;\"><TABLE style=\"WIDTH: 100%; height:auto; background-color:#FCFEDF; vertical-align:top;\" cellSpacing=0 cellPadding=0 border=0><TBODY>");
                        foreach(PortalListItemSiteMapNode listItem in contextSearchItemCol)
                        {
                            strCachedData.Append(CreateContextSearchLink(assetType, listItem));
                        }
                        //adding EPCatalog report options in context search
                        if(strGroupHeader.ToLower().Equals("documents"))
                        {
                            strCachedData.Append(RenderPVTReportContextSearch(assetType).ToString());
                        }
                        //
                        strCachedData.Append("</TBODY></TABLE></DIV>");

                        strCachedData.Append("</TD></TR>");

                    }
                    intHeaderIndex += 1;

                }
                strCachedData.Append("</TBODY></table>");

            }
            return strCachedData;
        }

        /// <summary>
        /// Renders the EPCatalog report context search.
        /// </summary>
        /// <param name="assetType">Type of the asset.</param>
        /// <returns></returns>
        private StringBuilder RenderPVTReportContextSearch(string assetType)
        {
            StringBuilder cachedData = new StringBuilder();
            SiteMapNodeCollection conTextSearchItemCol = null;
            string strLinkName = string.Empty;
            /// OrderBy added for DREAM 4.0 Context Search Re-order link requirement
            string strCamlQuery = "<OrderBy><FieldRef Name=\"Title\" /></OrderBy><Where><Eq><FieldRef Name=\"Active\" /><Value Type=\"Boolean\">1</Value></Eq></Where>";
            conTextSearchItemCol = GetContextSearchItemCollection(PVTREPORTLIST, strCamlQuery);
            if((conTextSearchItemCol != null) && (conTextSearchItemCol.Count > 0))
            {
                foreach(PortalListItemSiteMapNode listItem in conTextSearchItemCol)
                {
                    strLinkName = string.Empty;
                    if(listItem["PVT"] != null)
                    {
                        strLinkName = listItem["PVT"].ToString();
                    }
                    cachedData.Append("<TR  class=\"lvl1\" Height=\"auto\" onmouseover=\"javascript:ChangeClassName(this,'lvl1over');\" onmouseout=\"javascript:ChangeClassName(this,'lvl1');\"><TD title=\"" + strLinkName + "\"><DIV><a style=\"white-space:normal\" href=\"javascript:EPSearchContextLink('" + strLinkName + "','" + assetType + "'," + intIWellIndex + ",'PVTReport');\">" + strLinkName + "</a></DIV></TD></TR>");
                }
            }
            return cachedData;
        }
        /// <summary>
        /// Gets the group headers.,added for dynamic context search
        /// </summary>
        /// <param name="assetType">Type of the asset.</param>
        /// <returns></returns>
        private ArrayList GetGroupHeaders(string assetType)
        {
            #region DREAM 4.0 Context search - ability to specify order of links
            /// ====================================
            /// Module Name:Context search - ability to specify order of links
            /// Description:Context search - ability to specify order of links
            /// Date:24-Jan-2011
            /// Modified Lines:675-686
            /// ====================================
            PortalSiteMapProvider objPortalSiteMapProvider = PortalSiteMapProvider.WebSiteMapProvider;
            ArrayList arlGroupHeaders = new ArrayList();
            string strCamlQuery = "<Where><And><Eq><FieldRef Name=\"Asset_x0020_Type\" /><Value Type=\"Lookup\">" + assetType + "</Value></Eq><Eq><FieldRef Name=\"Active\" /><Value Type=\"Boolean\">1</Value></Eq></And></Where><OrderBy><FieldRef Name='HeaderOrder' Ascending='True' /><FieldRef Name='Group_x0020_Header' Ascending='True' /></OrderBy>";
            #endregion

            SPSecurity.RunWithElevatedPrivileges(delegate()
             {
                 using(SPSite site = new SPSite(SPContext.Current.Site.Url.ToString()))
                 {
                     using(SPWeb web = site.OpenWeb())
                     {
                         SPQuery query = new SPQuery();
                         query.Query = strCamlQuery;
                         PortalWebSiteMapNode pNode = (PortalWebSiteMapNode)objPortalSiteMapProvider.FindSiteMapNode(web.ServerRelativeUrl);
                         //Reads the sharepoint List based on CamlQuery. 
                         SiteMapNodeCollection listItems = objPortalSiteMapProvider.GetCachedListItemsByQuery(pNode, SEARCHCONTEXTTITLE, query, web);

                         //Loop through the items in List.
                         foreach(PortalListItemSiteMapNode listItem in listItems)
                         {
                             if(!arlGroupHeaders.Contains(listItem["Group_x0020_Header"]))
                             {
                                 arlGroupHeaders.Add(listItem["Group_x0020_Header"]);
                             }
                         }
                     }
                 }
             });
            return arlGroupHeaders;
        }
        /// <summary>
        /// Get the Active Cached context search items.
        /// </summary>
        /// <param name="camlQuery">The caml query.</param>
        /// <returns>Context Search Items</returns>
        private SiteMapNodeCollection GetContextSearchItemCollection(string listName, string camlQuery)
        {
            /// Initializing variables            
            string strParentSiteURL = string.Empty;
            SiteMapNodeCollection listItems = null;
            PortalSiteMapProvider objPortalSiteMapProvider = PortalSiteMapProvider.WebSiteMapProvider;
            strParentSiteURL = SPContext.Current.Site.Url.ToString();
            using(SPSite site = new SPSite(strParentSiteURL))
            {
                using(SPWeb web = site.OpenWeb())
                {
                    SPQuery query = new SPQuery();
                    query.Query = camlQuery;
                    PortalWebSiteMapNode pNode = (PortalWebSiteMapNode)objPortalSiteMapProvider.FindSiteMapNode(web.ServerRelativeUrl);
                    /// Reads the sharepoint List based on CamlQuery. 
                    listItems = objPortalSiteMapProvider.GetCachedListItemsByQuery(pNode, listName, query, web);
                }
            }
            return listItems;
        }
        /// <summary>
        /// Gets the asset for list search.
        /// </summary>
        /// <param name="listSearch">The list search.</param>
        /// <returns></returns>
        private string GetAssetForListSearch(string listSearch)
        {
            string strAsset = string.Empty;
            switch(listSearch.ToLowerInvariant())
            {
                case LISTOFWELLSITEM:
                    strAsset = WELLITEMVAL;
                    break;
                case LISTOFWELLBORESITEM:
                    strAsset = WELLBOREITEMVAL;
                    break;
                case LISTOFFIELDSITEM:
                    strAsset = FIELDITEMVAL;
                    break;
                case LISTOFRESERVOIRSITEM:
                    strAsset = RESERVOIRITEMVAL;
                    break;
                default:
                    strAsset = listSearch;
                    break;
            }
            return strAsset;
        }
        /// <summary>
        /// Renders the leftnav links.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="parentSiteURL">The parent site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="camlQuery">The caml query.</param>
        private void RenderLeftNavigationLinks(string asset)
        {
            StringBuilder strLeftNavLiks = new StringBuilder();
            strLeftNavLiks = LoadContextSearchProfile(asset);
            if(strLeftNavLiks.Length > 0)
            {

                updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("<tr><td style=\"Height:22px;\" bgcolor=\"#FCFEDF\">"));
                updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("<DIV ID=\"divContextSearch\" class=\"LeftNavHeader1\">" + SEARCHCONTEXTTITLE + "</DIV>"));
                ;
                updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl(strLeftNavLiks.ToString()));
                updtPanelLeftNaV.ContentTemplateContainer.Controls.Add(CreateLiteralControl("</td></tr>"));
                ;
            }
        }
        /// <summary>
        /// Renders the quick search country.
        /// </summary>
        /// <param name="preferencesSetByUser">The preferences set by user.</param>
        /// <param name="parentSiteURL">The parent site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="camlQuery">The caml query.</param>
        private void RenderQuickSearchCountry(UserPreferences preferencesSetByUser, string parentSiteURL, string listName, string camlQuery)
        {
            DataTable objListData = null;
            DataRow objListRow;
            String strCountryName = string.Empty;
            String strCountryDesc = string.Empty;
            String strCompareCountry = string.Empty;
            String strQueryAsset = string.Empty;

            try
            {
                cboQuickCountry.ID = "cboQuickCountry";
                cboQuickCountry.CssClass = "dropdownAdvSrch";
                cboQuickCountry.Width = Unit.Pixel(140);
                cboQuickCountry.Items.Add(ANYCOUNTRYTEXT);
                cboQuickCountry.Items[0].Value = ANYCOUNTRYVALUE;

                if(Page.Request.QueryString[ASSETQUERYSTRING] != null)
                {
                    strQueryAsset = Page.Request.QueryString[ASSETQUERYSTRING].Trim();
                }
                /// Reads the values from Country Sharepoint List.
                objListData = ((MOSSServiceManager)objController).ReadList(parentSiteURL, listName, camlQuery);
                if(objListData != null && objListData.Rows.Count > 0)
                {
                    /// Loop through the values in Country List and finds the index of country user preference in List.
                    for(int intIndex = 0; intIndex < objListData.Rows.Count; intIndex++)
                    {
                        objListRow = objListData.Rows[intIndex];
                        strCountryName = objListRow["Title"].ToString();
                        strCountryDesc = objListRow["Country_x0020_Code"].ToString();
                        if(preferencesSetByUser.Country != null)
                        {
                            if(Page.Request.QueryString[COUNTRYQUERYSTRING] != null)
                            {
                                if(string.Compare(strQueryAsset, PARSITEMVAL, true) == 0)
                                    strCompareCountry = strCountryName;
                                else
                                    strCompareCountry = strCountryDesc;


                                if(string.Equals(strCompareCountry.Trim(), Page.Request.QueryString[COUNTRYQUERYSTRING].Trim()))
                                {
                                    intCountryIndex = intIndex + 1;
                                }
                            }
                            else if(string.Equals(strCountryDesc.Trim(), preferencesSetByUser.Country.Trim()))
                            {
                                intCountryIndex = intIndex + 1;
                            }
                        }
                        else if(Page.Request.QueryString[COUNTRYQUERYSTRING] != null)
                        {
                            if(string.Compare(strQueryAsset, PARSITEMVAL, true) == 0)
                                strCompareCountry = strCountryName;
                            else
                                strCompareCountry = strCountryDesc;


                            if(string.Equals(strCompareCountry.Trim(), Page.Request.QueryString[COUNTRYQUERYSTRING].Trim()))
                            {
                                intCountryIndex = intIndex + 1;
                            }
                        }
                        cboQuickCountry.Items.Add(strCountryName);
                        cboQuickCountry.Items[intIndex + 1].Value = strCountryDesc;
                    }
                }
                objListData.Clear();
                /// set the country default value from user preference.
                cboQuickCountry.SelectedIndex = intCountryIndex;
            }
            finally
            {
                if(objListData != null)
                    objListData.Dispose();
            }
        }

        /// <summary>
        /// Renders the quick search asset.
        /// </summary>
        /// <param name="preferencesSetByUser">The preferences set by user.</param>
        /// <param name="parentSiteURL">The parent site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="camlQuery">The caml query.</param>
        private void RenderQuickSearchAsset(UserPreferences preferencesSetByUser, string parentSiteURL, string listName, string camlQuery)
        {

            DataTable objListData = null;
            DataRow objListRow;
            string strAssetName = string.Empty;
            try
            {
                cboQuickAsset.ID = "cboQuickAsset";
                cboQuickAsset.CssClass = "dropdownAdvSrch";
                cboQuickAsset.Width = Unit.Pixel(140);
                cboQuickAsset.AutoPostBack = true;
                cboQuickAsset.SelectedIndexChanged += cboQuickAsset_SelectedIndexChanged;
                cboQuickAsset.Attributes.Add("onchange", "javascript:QuickSearchOnChange();");
                /// Reads the values from Asset Sharepoint List.
                objListData = ((MOSSServiceManager)objController).ReadList(parentSiteURL, listName, camlQuery);
                if(objListData != null && objListData.Rows.Count > 0)
                {
                    /// Loop through the values in Asset List.
                    for(int intIndex = 0; intIndex < objListData.Rows.Count; intIndex++)
                    {
                        objListRow = objListData.Rows[intIndex];
                        strAssetName = objListRow["Title"].ToString();
                        if(preferencesSetByUser.Asset.Length > 0)
                        {
                            if(Page.Request.QueryString[ASSETQUERYSTRING] != null)
                            {
                                if(string.Equals(strAssetName.Trim(), Page.Request.QueryString[ASSETQUERYSTRING].Trim()))
                                {
                                    intAssetIndex = intIndex;
                                }
                            }
                            else if(string.Equals(strAssetName.Trim(), preferencesSetByUser.Asset.Trim()))
                            {
                                intAssetIndex = intIndex;
                            }
                        }
                        else if(Page.Request.QueryString[ASSETQUERYSTRING] != null)
                        {
                            if(string.Equals(strAssetName.Trim(), Page.Request.QueryString[ASSETQUERYSTRING].Trim()))
                            {
                                intAssetIndex = intIndex;
                            }
                        }
                        cboQuickAsset.Items.Add(strAssetName);
                    }
                }
                objListData.Clear();
                /// this will set the default value for the asset type from user preferences.
                cboQuickAsset.SelectedIndex = intAssetIndex;
            }
            finally
            {
                if(objListData != null)
                    objListData.Dispose();
            }
        }
        /// <summary>
        /// Handles the SelectedIndexChanged event of the cboQuickAsset control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cboQuickAsset_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //txtSearchCriteria.Text = string.Empty;//DREAM4.0 Code changes done for revise quick search
                this.Context.Items.Add("LeftNavSelected", "true");
                PopulateQuickSearchColumnDDL();
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }

        }

        /// <summary>
        /// Gets the asset columns.
        /// </summary>
        /// 
        private void PopulateQuickSearchColumnDDL()
        {
            DataTable objListData = null;
            try
            {

                string strColumnName = string.Empty;
                string strDisplayName = string.Empty;
                string strCamlQueryAssetColumns = string.Empty;
                strCamlQueryAssetColumns = "<Where><And><Eq><FieldRef Name=\"Active\" /><Value Type=\"Boolean\">1</Value></Eq><Eq><FieldRef Name=\"Asset_x0020_Type\" /><Value Type=\"Lookup\">" + cboQuickAsset.SelectedItem.Text + "</Value></Eq></And></Where><OrderBy><FieldRef Name=\"Display_x0020_Name\" Ascending=\"True\" /></OrderBy>";
                objListData = ((MOSSServiceManager)objController).ReadList(strParentSiteURL, REPORTSERVICECOLUMNLIST, strCamlQueryAssetColumns);
                if(objListData != null && objListData.Rows.Count > 0)
                {
                    cboQuickColumn.Visible = true;
                    cboQuickColumn.DataTextField = objListData.Columns["Display_x0020_Name"].ColumnName;
                    cboQuickColumn.DataValueField = objListData.Columns["Title"].ColumnName;
                    cboQuickColumn.DataSource = objListData;
                    cboQuickColumn.DataBind();
                    cboQuickColumn.Items.Insert(0, new ListItem(ALLCOLUMNSTEXT, string.Empty));
                    objListData.Clear();
                }
                else
                {
                    /// If Asset Type is Basin or hides the Quick Column Field. 
                    cboQuickColumn.Visible = false;
                }
            }
            finally
            {
                if(objListData != null)
                    objListData.Dispose();
            }
        }
        /// <summary>
        /// Creates the quick search column DDL.
        /// </summary>
        private void CreateQuickSearchColumnDDL()
        {
            cboQuickColumn = new DropDownList();
            cboQuickColumn.ID = "cboQuickColumn";
            cboQuickColumn.CssClass = "dropdownAdvSrch";
            cboQuickColumn.Width = Unit.Pixel(140);
        }

        /// <summary>
        /// Gets the caml query column list.
        /// </summary>
        /// 
        /// <returns></returns>
        private string GetCamlQueryColumnList()
        {
            string strCamlQuery = string.Empty;
            strCamlQuery = "<Where><And><Eq><FieldRef Name=\"Active\" /><Value Type=\"Boolean\">1</Value></Eq><Eq><FieldRef Name=\"Asset_x0020_Type\" /><Value Type=\"Lookup\">" + cboQuickAsset.SelectedItem.Text + "</Value></Eq></And></Where><OrderBy><FieldRef Name=\"Display_x0020_Name\" Ascending=\"True\" /></OrderBy>";
            return strCamlQuery;
        }
        /// <summary>
        /// Renders the quick search criteria field.
        /// </summary>
        private void RenderQuickSearchCriteriaField()
        {
            ///initializes the criteria object.
            txtSearchCriteria = new TextBox();
            txtSearchCriteria.ID = "txtSearchCriteria";
            txtSearchCriteria.Width = Unit.Pixel(105);
            txtSearchCriteria.CssClass = "queryfieldmini";
        }
        /// <summary>
        /// Renders the quick search go button.
        /// </summary>
        private void RenderQuickSearchGoButton()
        {
            /// renders the Quick Search Go button.
            btnGo.ID = "cmdGo";
            btnGo.CssClass = "buttonAdvSrch";
            btnGo.Text = "Go";
            btnGo.OnClientClick = "javascript:return ValidateQuickUI();";
        }
        /// <summary>
        /// Sets the country value.
        /// </summary>
        /// <param name="selectedCountry">The selected country.</param>
        private void SetCountryValue(string selectedCountry)
        {
            /// Loop through the items in Country list.
            for(int intIndex = 0; intIndex < cboQuickCountry.Items.Count; intIndex++)
            {
                if(string.Equals(cboQuickCountry.Items[intIndex].Value, selectedCountry))
                {
                    intCountryIndex = intIndex;
                    break;
                }
            }
        }
        #region Dream 4.0 code

        /// <summary>
        /// Creates the literal control.
        /// </summary>
        /// <param name="literalText">The literal text.</param>
        /// <returns></returns>
        private LiteralControl CreateLiteralControl(string literalText)
        {
            LiteralControl objLiteralControl = new LiteralControl();
            objLiteralControl.Text = literalText;
            return objLiteralControl;
        }
        /// <summary>
        /// Creates the update panel.
        /// </summary>
        private void CreateUpdatePanel()
        {
            updtPanelLeftNaV = new UpdatePanel();
            updtPanelLeftNaV.ID = "updtPanelLeftNaV";
            updtPanelLeftNaV.UpdateMode = UpdatePanelUpdateMode.Conditional;
            updtPanelLeftNaV.ChildrenAsTriggers = true;
        }

        #region FixingFormAction

        /// <summary>
        /// Ensures the panel fix.
        /// </summary>
        private void EnsurePanelFix()
        {
            if(this.Page.Form != null)
            {
                string formOnSubmitAtt = this.Page.Form.Attributes["onsubmit"];
                if(formOnSubmitAtt == "return _spFormOnSubmitWrapper();")
                {
                    this.Page.Form.Attributes["onsubmit"] = "_spFormOnSubmitWrapper();";
                }
            }
            ScriptManager.RegisterStartupScript(this, typeof(LeftNavigation), "UpdatePanelFixup", "_spOriginalFormAction = document.forms[0].action; _spSuppressFormOnSubmitWrapper=true;", true);
        }
        #endregion FixingFormAction
        /// <summary>
        /// Gets the async post back control ID.
        /// </summary>
        /// <returns></returns>
        public string GetAsyncPostBackControlID()
        {
            string smUniqueId = ScriptManager.GetCurrent(this.Page).UniqueID;
            string smFieldValue = this.Page.Request.Form[smUniqueId];

            if(!String.IsNullOrEmpty(smFieldValue) && smFieldValue.Contains("|"))
            {
                return smFieldValue.Split('|')[1];
            }
            return ScriptManager.GetCurrent(this.Page).AsyncPostBackSourceElementID;
        }
        /// <summary>
        /// Renders the busy box.
        /// </summary>
        private void RenderBusyBox()
        {
            System.Text.StringBuilder strBustBox = new System.Text.StringBuilder();
            strBustBox.Append("<iframe id=\"BusyBoxIFrame\" style=\"border:3px double #D2D2D2\" name=\"BusyBoxIFrame\" frameBorder=\"0\" scrolling=\"no\" ondrop=\"return false;\" src=\"/_layouts/dream/BasinSyncBusyBox.htm\"></iframe>");
            strBustBox.Append("<iframe id=\"SearchBusyBoxIFrame\" style=\"border:3px double #D2D2D2\" name=\"SearchBusyBoxIFrame\" frameBorder=\"0\" scrolling=\"no\" ondrop=\"return false;\" src=\"/_layouts/dream/BusyBox.htm\"></iframe>");
            strBustBox.Append("<script>");
            strBustBox.Append("var busyBox = new BusyBox(\"BusyBoxIFrame\", \"busyBox\", 1, \"processing\", \".gif\", 125, 147, 207,\"/_layouts/dream/BasinSyncBusyBox.htm\");");
            strBustBox.Append("var searchBusyBox = new BusyBox(\"SearchBusyBoxIFrame\", \"searchBusyBox\", 1, \"processing\", \".gif\", 125, 147, 207,\"/_layouts/dream/BusyBox.htm\");");
            strBustBox.Append("</script>");
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LeftNavBusyBoxScript", strBustBox.ToString());
        }


        #region Roles & Profiles Integration code

        /// <summary>
        /// Loads the context search profile.
        /// </summary>
        /// <param name="asset">The asset.</param>
        /// <returns></returns>
        private StringBuilder LoadContextSearchProfile(string asset)
        {
            XmlDocument xmlDocContextSearch = GetContextSearchProfile();
            StringBuilder strContextSearchProfile = new StringBuilder();
            if(xmlDocContextSearch != null)//role context search xml does exist
            {
                if(xmlDocContextSearch.SelectSingleNode("contextsearches/groupheaders[@asset = '" + asset + "']") != null)//Context search profile saved for current asset
                {
                    strContextSearchProfile = RenderContextSearchFromUserProfile(xmlDocContextSearch, asset);
                }
                else//Context search profile not saved for current asset
                {
                    //get default order for all group headers and context links fron context search list
                    strContextSearchProfile = RenderContextSearchFromList(asset);
                }
            }
            else//user role context search xml does not exist
            {
                //get default order for all group headers and context links fron context search list
                strContextSearchProfile = RenderContextSearchFromList(asset);
            }
            return strContextSearchProfile;
        }

        /// <summary>
        /// Renders the context search from user profile.
        /// </summary>
        /// <param name="contextSearchProfile">The context search profile.</param>
        /// <param name="assetType">Type of the asset.</param>
        /// <returns></returns>
        private StringBuilder RenderContextSearchFromUserProfile(XmlDocument contextSearchProfile, string assetType)
        {
            //All group headers and their order is taken fron user role contextsearch profile
            //loop for group header starts
            //if (true)//context search links are saved for current group header
            //{
            //    //only populate context search links saved in profile as display true in their saved order
            //}
            //else//context search links are not saved for current group header
            //{
            //    //get all the context search list for current asset and current group header from context search list
            //}
            //ends
            StringBuilder strCachedData = new StringBuilder();
            SiteMapNodeCollection contextSearchItemCol = null;
            ArrayList arlGroupHeaders = null;
            int intHeaderIndex = 0;
            string strLinkName = string.Empty;
            string strLinkValue = string.Empty;
            string strToolTip = string.Empty;
            string strPageUrl = string.Empty;

            if(string.Equals(assetType, WELLBOREITEMVAL))
            {
                intIWellIndex = 4;
            }
            else if(string.Equals(assetType, BASINITEMVAL) || string.Equals(assetType, FIELDITEMVAL))
            {
                intIWellIndex = 1;
            }
            arlGroupHeaders = GetProfileGroupHeaders(contextSearchProfile, assetType);
            XmlNode xmlNodeGroupHeaders = contextSearchProfile.SelectSingleNode("contextsearches/groupheaders[@asset = '" + assetType + "']");
            if(arlGroupHeaders != null && arlGroupHeaders.Count > 0)
            {
                strCachedData.Append("<table id=\"tblContextSearch\" class=\"vertNav\" style=\"display:none;WIDTH: 100%; BORDER-COLLAPSE: collapse;background-color:#ffeaa0;\" cellSpacing=0 cellPadding=0 border=0><TBODY>");

                foreach(string strGroupHeader in arlGroupHeaders)
                {
                    XmlNodeList xmlNodeLstContextSearch = xmlNodeGroupHeaders.SelectNodes("groupheader[@name = '" + strGroupHeader + "']/contextsearch[@display = 'true']");
                    if(xmlNodeLstContextSearch != null && xmlNodeLstContextSearch.Count > 0)//context search links are saved for current group header
                    {
                        strCachedData.Append("<TR><TD class=\"inactive\">");
                        strCachedData.Append("<DIV id=\"div_" + intHeaderIndex + "\" class=\"parent\" onclick=\"HideExpandCntxtSrch('" + intHeaderIndex + "')\" style=\"font-size:11px\">" + strGroupHeader + "&nbsp;</DIV>");

                        strCachedData.Append("<DIV id=\"subMenu_" + intHeaderIndex + "\" style=\"font-size:10px;DISPLAY: none; background-color:#FCFEDF;margin-top:0px; z-index: 101; height:auto;width:100%; visibility:hidden;\"><TABLE style=\"WIDTH: 100%; height:auto; background-color:#FCFEDF; vertical-align:top;\" cellSpacing=0 cellPadding=0 border=0><TBODY>");
                        //only populate context search links saved in profile as display true in their saved order
                        foreach(XmlNode node in xmlNodeLstContextSearch)
                        {
                            string strCamlQuery = "<Where><And><Eq><FieldRef Name=\"Asset_x0020_Type\" /><Value Type=\"Lookup\">" + assetType + "</Value></Eq><And><Eq><FieldRef Name=\"Active\" /><Value Type=\"Boolean\">1</Value></Eq><And><Eq><FieldRef Name=\"Group_x0020_Header\" /><Value Type=\"Choice\">" + strGroupHeader + "</Value></Eq><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">" + node.Attributes["name"].Value + "</Value></Eq></And></And></And></Where>";

                            contextSearchItemCol = GetContextSearchItemCollection(SEARCHCONTEXTTITLE, strCamlQuery);
                            if(contextSearchItemCol != null && contextSearchItemCol.Count > 0)
                            {
                                strCachedData.Append(CreateContextSearchLink(assetType, (PortalListItemSiteMapNode)contextSearchItemCol[0]));
                            }
                        }
                        //adding EPCatalog  report options in context search
                        if(strGroupHeader.ToLower().Equals("documents"))
                        {
                            strCachedData.Append(RenderPVTReportContextSearch(assetType).ToString());
                        }
                        //
                        strCachedData.Append("</TBODY></TABLE></DIV>");
                        strCachedData.Append("</TD></TR>");
                    }
                    else
                    {
                        string strCamlQuery = "<Where><And><Eq><FieldRef Name=\"Asset_x0020_Type\" /><Value Type=\"Lookup\">" + assetType + "</Value></Eq><And><Eq><FieldRef Name=\"Group_x0020_Header\" /><Value Type=\"Choice\">" + strGroupHeader + "</Value></Eq><Eq><FieldRef Name=\"Active\" /><Value Type=\"Boolean\">1</Value></Eq></And></And></Where><OrderBy><FieldRef Name=\"LinkOrder\" Ascending=\"True\" /><FieldRef Name=\"Title\" Ascending=\"True\" /></OrderBy>";

                        contextSearchItemCol = GetContextSearchItemCollection(SEARCHCONTEXTTITLE, strCamlQuery);
                        if((contextSearchItemCol != null) && (contextSearchItemCol.Count > 0))
                        {
                            strCachedData.Append("<TR><TD class=\"inactive\">");
                            strCachedData.Append("<DIV id=\"div_" + intHeaderIndex + "\" class=\"parent\" onclick=\"HideExpandCntxtSrch('" + intHeaderIndex + "')\" style=\"font-size:11px\">" + strGroupHeader + "&nbsp;</DIV>");

                            strCachedData.Append("<DIV id=\"subMenu_" + intHeaderIndex + "\" style=\"font-size:10px;DISPLAY: none; background-color:#FCFEDF;margin-top:0px; z-index: 101; height:auto;width:100%; visibility:hidden;\"><TABLE style=\"WIDTH: 100%; height:auto; background-color:#FCFEDF; vertical-align:top;\" cellSpacing=0 cellPadding=0 border=0><TBODY>");
                            foreach(PortalListItemSiteMapNode listItem in contextSearchItemCol)
                            {
                                strCachedData.Append(CreateContextSearchLink(assetType, listItem));
                            }
                            //adding EPCatalog report options in context search
                            if(strGroupHeader.ToLower().Equals("documents"))
                            {
                                strCachedData.Append(RenderPVTReportContextSearch(assetType).ToString());
                            }
                            //
                            strCachedData.Append("</TBODY></TABLE></DIV>");
                            strCachedData.Append("</TD></TR>");
                        }
                    }
                    intHeaderIndex++;
                }
                strCachedData.Append("</TBODY></table>");
            }
            return strCachedData;
        }

        /// <summary>
        /// Gets the profile group headers.
        /// </summary>
        /// <param name="contextSearchProfile">The context search profile.</param>
        /// <param name="asset">The asset.</param>
        /// <returns></returns>
        private ArrayList GetProfileGroupHeaders(XmlDocument contextSearchProfile, string asset)
        {
            XmlNode xmlNodeGroupHeaders = contextSearchProfile.SelectSingleNode("contextsearches/groupheaders[@asset = '" + asset + "']");
            ArrayList arrGroupHeaders = new ArrayList();
            for(int intIndex = 0; intIndex < xmlNodeGroupHeaders.ChildNodes.Count; intIndex++)
            {
                XmlNode xmlNodeGroupHeader = xmlNodeGroupHeaders.SelectSingleNode("groupheader[@order = '" + (intIndex + 1) + "']");
                if(xmlNodeGroupHeader != null)
                {
                    arrGroupHeaders.Add(xmlNodeGroupHeader.Attributes["name"].Value);
                }
            }
            return arrGroupHeaders;
        }

        /// <summary>
        /// Gets the context search profile.
        /// </summary>
        /// <returns></returns>
        private XmlDocument GetContextSearchProfile()
        {
            XmlDocument xmlDocContextSearch = null;
            string strUserRole = objUtility.GetUserRole();
            if(((MOSSServiceManager)objController).IsDocLibFileExist(CONTEXTSEARCHDOCLIB, strUserRole))
            {
                xmlDocContextSearch = ((MOSSServiceManager)objController).GetDocLibXMLFile(CONTEXTSEARCHDOCLIB, strUserRole);
            }
            return xmlDocContextSearch;
        }

        /// <summary>
        /// Creates the context search link.
        /// </summary>
        /// <param name="asset">The asset.</param>
        /// <param name="listItem">The list item.</param>
        /// <returns></returns>
        private string CreateContextSearchLink(string asset, PortalListItemSiteMapNode listItem)
        {
            StringBuilder strContextSearchLinkHTML = new StringBuilder();
            string strLinkName = string.Empty;
            string strLinkValue = string.Empty;
            string strToolTip = string.Empty;
            string strPageUrl = string.Empty;
            if(listItem["Title"] != null)
            {
                strLinkName = listItem["Title"].ToString();
            }
            if(listItem["ToolTip"] != null)
            {
                strToolTip = listItem["ToolTip"].ToString();
            }
            if(listItem["Value"] != null)
            {
                strLinkValue = listItem["Value"].ToString();
            }
            if(listItem["Page URL"] != null)
            {
                strPageUrl = listItem["Page URL"].ToString();
            }
            if(string.Equals(strLinkName, IWELLFILE))
            {
                string striWellFileURL = PortalConfiguration.GetInstance().GetKey(IWELLFILEURL);
                strContextSearchLinkHTML.Append("<TR class=\"lvl1\" onmouseover=\"javascript:ChangeClassName(this,'lvl1over');\" onmouseout=\"javascript:ChangeClassName(this,'lvl1');\"><TD title=\"" + strToolTip + "\"><DIV><a href=\"javascript:iWellFile('" + striWellFileURL + "','" + asset + "'," + intIWellIndex + ",'SearchResults');\">" + strLinkName + "</a></DIV></TD></TR>");
            }
            else if(strLinkName.Contains("EP Catalog"))
            {
                strContextSearchLinkHTML.Append("<TR class=\"lvl1\" onmouseover=\"javascript:ChangeClassName(this,'lvl1over');\" onmouseout=\"javascript:ChangeClassName(this,'lvl1');\"><TD title=\"" + strToolTip + "\"><DIV><a href=\"javascript:EPSearchContextLink('" + strLinkValue + "','" + asset + "'," + intIWellIndex + ",'SearchResults');\">" + strLinkName + "</a></DIV></TD></TR>");
            }
            else if(string.Equals(strLinkName.ToLowerInvariant(), "eWB2".ToLowerInvariant()))
            {
                strContextSearchLinkHTML.Append("<TR class=\"lvl1\" onmouseover=\"javascript:ChangeClassName(this,'lvl1over');\" onmouseout=\"javascript:ChangeClassName(this,'lvl1');\"><TD title=\"" + strToolTip + "\"><DIV><a href=\"javascript:OpenDWBContextSearchLink('eWB2','" + asset + "'," + intIWellIndex + ");\">" + strLinkName + "</a></DIV></TD></TR>");
            }
            else
            {
                strContextSearchLinkHTML.Append("<TR  class=\"lvl1\" Height=\"auto\" onmouseover=\"javascript:ChangeClassName(this,'lvl1over');\" onmouseout=\"javascript:ChangeClassName(this,'lvl1');\"><TD title=\"" + strToolTip + "\"><DIV><a style=\"white-space:normal\" href=\"javascript:OpenContextReports('" + strLinkValue + "','" + strPageUrl + "');\">" + strLinkName + "</a></DIV></TD></TR>");
            }
            return strContextSearchLinkHTML.ToString();
        }

        #endregion
        #endregion
        #endregion


    }
}


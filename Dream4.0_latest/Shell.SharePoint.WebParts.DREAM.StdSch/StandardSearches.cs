#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: StandardSearches.cs
#endregion
using System;
using System.Web;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.WebParts.DREAM.StandardSearches
{
    /// <summary>
    /// This is the Shared Searches class to display all the Admin saved searches.
    /// </summary>
    public class StandardSearches :System.Web.UI.WebControls.WebParts.WebPart
    {
        #region Declaration
        AbstractController objMossController;
        CommonUtility objUtility = new CommonUtility();
        ServiceProvider objFactory = new ServiceProvider();
        LinkButton linkDeleteButton = new LinkButton();
        HiddenField hidDeleteSearchName = new HiddenField();
        HiddenField hidSearchType = new HiddenField();
        Label lblNoRecords = new Label();
        ArrayList arlAdvSaveSearchNames;
        ArrayList arlWellAdvSaveSearchNames;
        ArrayList arlWellboreAdvSaveSearchNames;
        ArrayList arlPARSSaveSearchNames;
        ArrayList arlLOGSSaveSearchNames;
        ArrayList arlFieldSaveSearchNames;
        ArrayList arlBasinSaveSearchNames;
        ArrayList arlQuerySaveSearchNames;

        /// <summary>
        ///  Added for SRP Reservoir Advance Search
        /// </summary>
        ArrayList arlReservoirAdvSaveSearchNames;
        bool blnSRPEnabled;
        const string ADMIN = "Administrator";
        const string ADVANCEDSEARCH = "Advanced Search";
        const string PARSADVSEARCH = "PARS";
        const string LOGSBYFIELD = "Logs By Field Depth";
        const string FIELDADVSEARCH = "Field Advance Search";
        const string BASINADVSEARCH = "Basin Advance Search";
        const string QUERYSEARCH = "Query Search";
        const string ADVSITEURL = "/pages/AdvancedSearchResults.aspx";
        const string WELLWELLBOREADVSEARCHURL = "/pages/AdvSearchWellWellbore.aspx";
        const string PARSADVSEARCHURL = "/pages/AdvSearchPARS.aspx";
        const string LOGBYFIELDADVSEARCHURL = "/pages/AdvSearchlogsbyfielddepth.aspx";
        const string FIELDADVSEARCHURL = "/pages/AdvSearchField.aspx";
        const string BASINADVSEARCHURL = "/pages/AdvSearchBasin.aspx";
        const string QUERYSEARCHURL = "/pages/QuerySearch.aspx";
        /// <summary>
        ///  Added for SRP
        /// </summary>
        const string RESERVOIRADVSEARCHURL = "/pages/AdvSearchReservoir.aspx";
        const string DETAILLINKTEXT = "View Details";
        const string MODIFYLINKTEXT = "Modify";
        const string SAVESEARCH = "Save Search";
        const string MANAGESEARCH = "Manage Search";
        const string GENERALQUERYSRCH = "General Query Search";
        const string SQLQUERYSRCH = "Sql Query Search";
        //Added by Dev
        const string WELLADVSEARCH = "Well Advance Search";
        const string WELLBOREADVSEARCH = "Wellbore Advance Search";
        const string WELL = "Well";
        const string WELLBORE = "Wellbore";
        //End

        /// Added for SRP
        const string RESERVOIRSEARCHTYPE = "Reservoir Advance Search";
        const string RESERVOIR = "Reservoir";
        const string ENABLESRPSPECIFICCONTROLS = "EnableSRPSpecificControls";

        #region Added for DREAM 4.0
        /// <summary>
        /// Added for DREAM 4.0
        /// To separate Well and Wellbore advance search screens
        /// </summary>
        const string WELLADVSEARCHURL = "/pages/AdvSearchWell.aspx";
        #endregion

        #endregion

        #region Overridden Methods
        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            //Fix for the UpdatePanel postback behaviour.
            //Dream 4.0 code start
            objUtility.EnsurePanelFix(this.Page, typeof(StandardSearches));
            hidDeleteSearchName.ID = "hidDeleteSearchName";
            this.Controls.Add(hidDeleteSearchName);

            hidSearchType.ID = "hidSearchType";
            this.Controls.Add(hidSearchType);

            linkDeleteButton.ID = "linkDeleteButton";
            linkDeleteButton.Attributes.Add("runat", "server");
            linkDeleteButton.Text = "Delete";
            linkDeleteButton.Click += new EventHandler(linkDeleteButton_Click);
            this.Controls.Add(linkDeleteButton);

            lblNoRecords.ID = "lblNoRecords";
            lblNoRecords.CssClass = "labelMessage";
            lblNoRecords.Visible = false;
            this.Controls.Add(lblNoRecords);
        }

        /// <summary>
        /// Handles the Click event of the linkDeleteButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void linkDeleteButton_Click(object sender, EventArgs e)
        {
            string strUserID = string.Empty;
            if(hidDeleteSearchName.Value != null)
            {
                try
                {
                    strUserID = objUtility.GetSaveSearchUserName();
                    objMossController = objFactory.GetServiceManager("MossService");
                    if(hidDeleteSearchName.Value.ToString().Length > 0)
                        ((MOSSServiceManager)objMossController).DeleteSaveSearch(hidSearchType.Value.ToString(), strUserID, hidDeleteSearchName.Value.ToString());
                }
                catch(Exception ex)
                {
                    throw ex;
                }
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
                objMossController = objFactory.GetServiceManager("MOSSService");
                // arlAdvSaveSearchNames = new ArrayList();
                arlWellAdvSaveSearchNames = new ArrayList();
                arlWellboreAdvSaveSearchNames = new ArrayList();
                arlPARSSaveSearchNames = new ArrayList();
                arlLOGSSaveSearchNames = new ArrayList();
                arlFieldSaveSearchNames = new ArrayList();
                arlBasinSaveSearchNames = new ArrayList();
                arlQuerySaveSearchNames = new ArrayList();
                arlReservoirAdvSaveSearchNames = new ArrayList();
                if(Page.Request.QueryString["Type"] != null)
                {
                    if(string.Equals(Page.Request.QueryString["Type"].ToString(), MANAGESEARCH))
                    {
                        hidDeleteSearchName.RenderControl(writer);
                        hidSearchType.RenderControl(writer);
                        objUtility = new CommonUtility();
                        #region DeleteSearches

                        string strUserID = objUtility.GetSaveSearchUserName();
                        GetSaveSearchNames(strUserID, true);
                        //validates the Saved Searches and displays information if there are no saved searches.
                        if((arlWellAdvSaveSearchNames.Count == 0) && (arlWellboreAdvSaveSearchNames.Count == 0) && (arlPARSSaveSearchNames.Count == 0) && (arlLOGSSaveSearchNames.Count == 0) && (arlFieldSaveSearchNames.Count == 0) && (arlBasinSaveSearchNames.Count == 0) && (arlQuerySaveSearchNames.Count == 0) && (arlReservoirAdvSaveSearchNames.Count == 0))
                        {
                            writer.Write("<div id=\"divStandardSearch\"><table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tr><td align=\"left\" valign=\"middle\" class=\"breadcrumbRow\"><b>My Searches</b></td></tr>");
                            writer.Write("<tr><td height=\"10\" align=\"left\" valign=\"top\"></td></tr>");
                            writer.Write("<tr><td align=\"left\" valign=\"top\">");
                            writer.Write("<div class=\"iframeBorderException\"><table width=\"98%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                            lblNoRecords.Text = "There are no saved searches defined.";
                            lblNoRecords.Visible = true;
                            writer.Write("<table><tr><td><BR/>");
                            lblNoRecords.RenderControl(writer);
                            writer.Write("</td></tr></table>");
                            writer.Write("</table></div></td></tr></table></div>");
                        }
                        else
                        {
                            writer.Write("<div id=\"divStandardSearch\"><table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tr><td align=\"left\" valign=\"middle\" class=\"breadcrumbRow\"><b>My Searches</b></td></tr>");
                            RenderSaveSearchLayout(writer);
                        }
                        #endregion
                        writer.Write("<Script language=\"javascript\">setWindowTitle('My Searches');</Script>");
                    }
                    else
                    {
                        #region StandardSearches
                        GetSaveSearchNames(ADMIN, false);
                        //validates the Saved Searches and displays information if there are no saved searches.
                        if((arlWellAdvSaveSearchNames.Count == 0) && (arlWellboreAdvSaveSearchNames.Count == 0) && (arlPARSSaveSearchNames.Count == 0) && (arlLOGSSaveSearchNames.Count == 0) && (arlFieldSaveSearchNames.Count == 0) && (arlBasinSaveSearchNames.Count == 0) && (arlQuerySaveSearchNames.Count == 0) && (arlReservoirAdvSaveSearchNames.Count == 0))
                        {
                            writer.Write("<div id=\"divStandardSearch\"><table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tr><td align=\"left\" valign=\"middle\" class=\"breadcrumbRow\"><b>Shared Searches</b></td></tr>");
                            writer.Write("<tr><td height=\"10\" align=\"left\" valign=\"top\"></td></tr>");
                            writer.Write("<tr><td align=\"left\" valign=\"top\">");
                            writer.Write("<div class=\"iframeBorderException\"><table width=\"98%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                            lblNoRecords.Text = "There are no shared searches defined by the administrator.";
                            lblNoRecords.Visible = true;
                            writer.Write("<table><tr><td><BR/>");
                            lblNoRecords.RenderControl(writer);
                            writer.Write("</td></tr></table>");
                            writer.Write("</table></div></td></tr></table></div>");
                        }
                        else
                        {
                            writer.Write("<div id=\"divStandardSearch\"><table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tr><td align=\"left\" valign=\"middle\" class=\"breadcrumbRow\"><b>Shared Searches</b></td></tr>");
                            RenderSaveSearchLayout(writer);
                        }
                        #endregion
                        writer.Write("<Script language=\"javascript\">setWindowTitle('Shared Searches');</Script>");
                    }
                }
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
        }

        /// <summary>
        /// Gets the save search names.
        /// </summary>
        /// <param name="strUserID">The STR user ID.</param>
        private void GetSaveSearchNames(string userID, bool fetchAll)
        {
            try
            {
                arlWellAdvSaveSearchNames = ((MOSSServiceManager)objMossController).GetSharedSaveSearchName(WELLADVSEARCH, userID, fetchAll);
                arlWellboreAdvSaveSearchNames = ((MOSSServiceManager)objMossController).GetSharedSaveSearchName(WELLBOREADVSEARCH, userID, fetchAll);
                arlPARSSaveSearchNames = ((MOSSServiceManager)objMossController).GetSharedSaveSearchName(PARSADVSEARCH, userID, fetchAll);
                arlLOGSSaveSearchNames = ((MOSSServiceManager)objMossController).GetSharedSaveSearchName(LOGSBYFIELD, userID, fetchAll);
                arlFieldSaveSearchNames = ((MOSSServiceManager)objMossController).GetSharedSaveSearchName(FIELDADVSEARCH, userID, fetchAll);
                arlBasinSaveSearchNames = ((MOSSServiceManager)objMossController).GetSharedSaveSearchName(BASINADVSEARCH, userID, fetchAll);
                arlQuerySaveSearchNames = ((MOSSServiceManager)objMossController).GetSharedSaveSearchName(QUERYSEARCH, userID, fetchAll);

                string strSrpControlsEnable = PortalConfiguration.GetInstance().GetKey(ENABLESRPSPECIFICCONTROLS);
                if(string.Equals(strSrpControlsEnable.ToLowerInvariant(), "yes"))
                {
                    blnSRPEnabled = true;

                }
                if(blnSRPEnabled)
                {

                    arlReservoirAdvSaveSearchNames = ((MOSSServiceManager)objMossController).GetSharedSaveSearchName(RESERVOIRSEARCHTYPE, userID, fetchAll);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Renders the save search layout.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderSaveSearchLayout(HtmlTextWriter writer)
        {
            writer.Write("<tr><td height=\"10\" align=\"left\" valign=\"top\"></td></tr>");
            writer.Write("<tr><td align=\"left\" valign=\"top\">");
            writer.Write("<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"tableBorder\"><tr><td width=\"80%\" valign=\"middle\" class=\"savedsearchHeaderBlue\"><b>Title</b></td></tr></table>");
            writer.Write("</td></tr><tr><td align=\"left\" valign=\"top\">");
            writer.Write("<div class=\"iframeBorder\"><table width=\"98%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
            RenderAdvancedSaveSearches(writer, WELL);
            RenderAdvancedSaveSearches(writer, WELLBORE);
            RenderPARSSaveSearches(writer);
            RenderLOGSSaveSearches(writer);
            RenderFieldSaveSearches(writer);
            RenderBasinSaveSearches(writer);
            RenderQuerySearches(writer);
            RenderReservoirSaveSearches(writer);
            writer.Write("</table></div></td></tr></table></div>");
        }


        /// <summary>
        /// Renders the Saved Query Searches 
        /// </summary>
        /// <param name="writer"></param>
        private void RenderQuerySearches(HtmlTextWriter writer)
        {
            //validates the Savedsercahes.
            if(arlQuerySaveSearchNames.Count > 0)
            {
                writer.Write("<tr><td valign=\"middle\" class=\"savedsearchHeaderGray\">");
                writer.Write("<div id=\"dvi_col_exp_6\" style=\"float:left\"><img src=\"/_layouts/DREAM/images/plus.gif\" alt=\"open\" width=\"11\" height=\"11\" border=\"0\" hspace=\"0\" onclick=\"hideExpand(6)\" /></div>");
                writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;<b>Query Advanced Search</b></td></tr>");
                writer.Write("<tr id=\"dvi_col_6\" style=\"display:none;\"><td valign=\"middle\">");
                writer.Write("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                //Loop through the Saved Searches.
                if(string.Equals(Page.Request.QueryString["Type"].ToString(), MANAGESEARCH))
                {
                    RenderManageQuerySearch(writer, arlQuerySaveSearchNames);
                }
                else
                {
                    RenderStandardQuerySearch(writer, arlQuerySaveSearchNames);
                }
                writer.Write("</table></td></tr>");
            }
        }

        /// <summary>
        /// Renders the Manage Query Search Results of both type GENERAL and SQL
        /// </summary>
        /// <param name="writer"></param>
        private void RenderManageQuerySearch(HtmlTextWriter writer, ArrayList querySaveSearchNames)
        {
            bool blnGen = false;
            bool blnSql = false;

            foreach(string strSaveSearchName in querySaveSearchNames)
            {
                if(strSaveSearchName == GENERALQUERYSRCH)
                {
                    blnGen = true;
                    writer.Write("<tr><td valign=\"middle\" class=\"savedsearchSubHeaderGray\" style=\"font-size:10px\">");
                    writer.Write("<div id=\"dvi_col_exp_7\" style=\"float:left\"><img src=\"/_layouts/DREAM/images/plus.gif\" alt=\"open\" width=\"11\" height=\"11\" border=\"0\" hspace=\"0\" onclick=\"hideExpand(7)\" /></div>");
                    writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;<b>" + GENERALQUERYSRCH + "</b></td></tr>");
                    writer.Write("<tr id=\"dvi_col_7\" style=\"display:none;\"><td valign=\"middle\">");
                    writer.Write("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                }
                else
                {
                    if(strSaveSearchName == SQLQUERYSRCH)
                    {
                        blnSql = true;
                        if(blnGen)
                        {
                            blnGen = false;
                            writer.Write("</table></td></tr>");
                        }
                        writer.Write("<tr><td valign=\"middle\" class=\"savedsearchSubHeaderGray\" style=\"font-size:10px\">");
                        writer.Write("<div id=\"dvi_col_exp_8\" style=\"float:left\"><img src=\"/_layouts/DREAM/images/plus.gif\" alt=\"open\" width=\"11\" height=\"11\" border=\"0\" hspace=\"0\" onclick=\"hideExpand(8)\" /></div>");
                        writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;<b>" + SQLQUERYSRCH + "</b></td></tr>");
                        writer.Write("<tr id=\"dvi_col_8\" style=\"display:none;\"><td valign=\"middle\">");
                        writer.Write("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                    }
                    else
                    {

                        writer.Write("<tr><td width=\"5%\">&nbsp;</td><td width=\"40%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:ManageSearchResults('" + ADVSITEURL + "', 'Query Search','" + strSaveSearchName + "','" + "Query" + "')\">" + strSaveSearchName + "</a></td><td width=\"15%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:ManageSearchCriteriaDetail('" + QUERYSEARCHURL + "','" + strSaveSearchName + "')\">" + DETAILLINKTEXT + "</a></td><td width=\"15%\" class=\"savedSerchTxt\">");
                        linkDeleteButton.OnClientClick = "javascript:return DeleteSaveSearch('Query Search','" + strSaveSearchName + "');";
                        linkDeleteButton.RenderControl(writer);
                        writer.Write("</td><td width=\"15%\"><a href=\"Javascript:ModifySaveSearch('" + QUERYSEARCHURL + "','" + strSaveSearchName + "')\">" + MODIFYLINKTEXT + "</a></td></tr>");
                    }
                }
            }
            if(blnSql)
            {
                blnSql = false;
                writer.Write("</table></td></tr>");
            }
            if(blnGen)
            {
                writer.Write("</table></td></tr>");
            }
        }

        /// <summary>
        /// Renders the Standard Query Search Results of both type GENERAL and SQL
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="querySaveSearchNames"></param>
        private void RenderStandardQuerySearch(HtmlTextWriter writer, ArrayList querySaveSearchNames)
        {
            bool blnGen = false;
            bool blnSql = false;

            foreach(string strSaveSearchName in querySaveSearchNames)
            {

                if(strSaveSearchName == GENERALQUERYSRCH)
                {
                    blnGen = true;
                    writer.Write("<tr><td valign=\"middle\" class=\"savedsearchSubHeaderGray\" style=\"font-size:10px\">");
                    writer.Write("<div id=\"dvi_col_exp_9\" style=\"float:left\"><img src=\"/_layouts/DREAM/images/plus.gif\" alt=\"open\" width=\"11\" height=\"11\" border=\"0\" hspace=\"0\" onclick=\"hideExpand(9)\" /></div>");
                    writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;<b>" + GENERALQUERYSRCH + "</b></td></tr>");
                    writer.Write("<tr id=\"dvi_col_9\" style=\"display:none;\"><td valign=\"middle\">");
                    writer.Write("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                }
                else
                {
                    if(strSaveSearchName == SQLQUERYSRCH)
                    {
                        blnSql = true;
                        if(blnGen)
                        {
                            blnGen = false;
                            writer.Write("</table></td></tr>");
                        }
                        writer.Write("<tr><td valign=\"middle\" class=\"savedsearchSubHeaderGray\" style=\"font-size:10px\">");
                        writer.Write("<div id=\"dvi_col_exp_10\" style=\"float:left\"><img src=\"/_layouts/DREAM/images/plus.gif\" alt=\"open\" width=\"11\" height=\"11\" border=\"0\" hspace=\"0\" onclick=\"hideExpand(10)\" /></div>");
                        writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;<b>" + SQLQUERYSRCH + "</b></td></tr>");
                        writer.Write("<tr id=\"dvi_col_10\" style=\"display:none;\"><td valign=\"middle\">");
                        writer.Write("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                    }
                    else
                    {

                        writer.Write("<tr><td width=\"5%\">&nbsp;</td><td width=\"55%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:StandardSearchResults('" + ADVSITEURL + "', 'Query Search','" + strSaveSearchName + "','Query" + "')\">" + strSaveSearchName + "</a></td><td width=\"40%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:StandardSearchCriteriaDetail('" + QUERYSEARCHURL + "','" + strSaveSearchName + "')\">" + DETAILLINKTEXT + "</a></td></tr>");
                    }

                }

            }
            if(blnSql)
            {
                blnSql = false;
                writer.Write("</table></td></tr>");
            }
            if(blnGen)
            {
                writer.Write("</table></td></tr>");
            }
        }

        #endregion
        #region Private Methods
        /// <summary>
        /// Renders the advanced save searches.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderAdvancedSaveSearches(HtmlTextWriter writer, string assetType)
        {
            //validates the Savedsercahes.
            string strSearchType = string.Empty;
            string strDiv = string.Empty;
            if(assetType.ToLower().Equals("well"))
            {
                strSearchType = WELLADVSEARCH;
                arlAdvSaveSearchNames = arlWellAdvSaveSearchNames;
                strDiv = "<div id=\"dvi_col_exp_11\" style=\"float:left\"><img src=\"/_layouts/DREAM/images/plus.gif\" alt=\"open\" width=\"11\" height=\"11\" border=\"0\" hspace=\"0\" onclick=\"hideExpand(11)\" /></div>";
                strDiv += "&nbsp;&nbsp;&nbsp;&nbsp;<b>" + strSearchType + "</b></td></tr>";
                strDiv += "<tr id=\"dvi_col_11\" style=\"display:none;\"><td valign=\"middle\">";
            }

            else
            {
                strSearchType = WELLBOREADVSEARCH;
                arlAdvSaveSearchNames = arlWellboreAdvSaveSearchNames;
                strDiv = "<div id=\"dvi_col_exp_1\" style=\"float:left\"><img src=\"/_layouts/DREAM/images/plus.gif\" alt=\"open\" width=\"11\" height=\"11\" border=\"0\" hspace=\"0\" onclick=\"hideExpand(1)\" /></div>";
                strDiv += "&nbsp;&nbsp;&nbsp;&nbsp;<b>" + strSearchType + "</b></td></tr>";
                strDiv += "<tr id=\"dvi_col_1\" style=\"display:none;\"><td valign=\"middle\">";
            }
            if(arlAdvSaveSearchNames.Count > 0)
            {
                writer.Write("<tr><td valign=\"middle\" class=\"savedsearchHeaderGray\">");
                writer.Write(strDiv);
                writer.Write("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                //Loop through the Saved Searches.
                if(string.Equals(Page.Request.QueryString["Type"].ToString(), MANAGESEARCH))
                {
                    foreach(string strSaveSearchName in arlAdvSaveSearchNames)
                    {
                        #region modified for DREAM 4.0
                        /// <summary>
                        /// Added for separation of Well and Wellbore adv search screen requirment
                        /// </summary>

                        if(assetType.ToLowerInvariant().Equals("well"))
                        {
                            writer.Write("<tr><td width=\"5%\">&nbsp;</td><td width=\"40%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:ManageSearchResults('" + ADVSITEURL + "', '" + strSearchType + "','" + strSaveSearchName + "','" + assetType + "')\">" + strSaveSearchName + "</a></td><td width=\"15%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:ManageWellWellboreSearchCriteriaDetail('" + WELLADVSEARCHURL + "','" + strSaveSearchName + "','" + assetType + "')\">" + DETAILLINKTEXT + "</a></td><td width=\"15%\" class=\"savedSerchTxt\">");
                            linkDeleteButton.OnClientClick = "javascript:return DeleteSaveSearch('" + strSearchType + "','" + strSaveSearchName + "');";
                            linkDeleteButton.RenderControl(writer);
                            writer.Write("</td><td width=\"15%\"><a href=\"Javascript:ModifyWellWellboreSaveSearch('" + WELLADVSEARCHURL + "','" + strSaveSearchName + "','" + assetType + "')\">" + MODIFYLINKTEXT + "</a></td></tr>");
                        }
                        else
                        {
                            writer.Write("<tr><td width=\"5%\">&nbsp;</td><td width=\"40%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:ManageSearchResults('" + ADVSITEURL + "', '" + strSearchType + "','" + strSaveSearchName + "','" + assetType + "')\">" + strSaveSearchName + "</a></td><td width=\"15%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:ManageWellWellboreSearchCriteriaDetail('" + WELLWELLBOREADVSEARCHURL + "','" + strSaveSearchName + "','" + assetType + "')\">" + DETAILLINKTEXT + "</a></td><td width=\"15%\" class=\"savedSerchTxt\">");
                            linkDeleteButton.OnClientClick = "javascript:return DeleteSaveSearch('" + strSearchType + "','" + strSaveSearchName + "');";
                            linkDeleteButton.RenderControl(writer);
                            writer.Write("</td><td width=\"15%\"><a href=\"Javascript:ModifyWellWellboreSaveSearch('" + WELLWELLBOREADVSEARCHURL + "','" + strSaveSearchName + "','" + assetType + "')\">" + MODIFYLINKTEXT + "</a></td></tr>");
                        }
                        #endregion
                    }
                }
                else
                {
                    foreach(string strSaveSearchName in arlAdvSaveSearchNames)
                    {
                        if(assetType.ToLowerInvariant().Equals("well"))
                        {
                            writer.Write("<tr><td width=\"5%\">&nbsp;</td><td width=\"55%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:StandardSearchResults('" + ADVSITEURL + "', '" + strSearchType + "','" + strSaveSearchName + "','" + assetType + "')\">" + strSaveSearchName + "</a></td><td width=\"40%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:WellWellboreStandardSearchCriteriaDetail('" + WELLADVSEARCHURL + "','" + strSaveSearchName + "','" + assetType + "')\">" + DETAILLINKTEXT + "</a></td></tr>");
                        }
                        else
                        {
                            writer.Write("<tr><td width=\"5%\">&nbsp;</td><td width=\"55%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:StandardSearchResults('" + ADVSITEURL + "', '" + strSearchType + "','" + strSaveSearchName + "','" + assetType + "')\">" + strSaveSearchName + "</a></td><td width=\"40%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:WellWellboreStandardSearchCriteriaDetail('" + WELLWELLBOREADVSEARCHURL + "','" + strSaveSearchName + "','" + assetType + "')\">" + DETAILLINKTEXT + "</a></td></tr>");
                        }
                    }
                }
                writer.Write("</table></td></tr>");
            }

        }

        /// <summary>
        /// Renders the PARS save searches.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderPARSSaveSearches(HtmlTextWriter writer)
        {
            //validates the Savedsercahes.
            if(arlPARSSaveSearchNames.Count > 0)
            {
                writer.Write("<tr><td valign=\"middle\" class=\"savedsearchHeaderGray\">");
                writer.Write("<div id=\"dvi_col_exp_2\" style=\"float:left\"><img src=\"/_layouts/DREAM/images/plus.gif\" alt=\"open\" width=\"11\" height=\"11\" border=\"0\" hspace=\"0\" onclick=\"hideExpand(2)\" /></div>");
                writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;<b>Project Archives Advanced Search</b></td></tr>");
                writer.Write("<tr id=\"dvi_col_2\" style=\"display:none;\"><td valign=\"middle\">");
                writer.Write("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                //Loop through the Saved Searches.
                if(string.Equals(Page.Request.QueryString["Type"].ToString(), MANAGESEARCH))
                {
                    foreach(string strSaveSearchName in arlPARSSaveSearchNames)
                    {
                        writer.Write("<tr><td width=\"5%\">&nbsp;</td><td width=\"40%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:ManageSearchResults('" + ADVSITEURL + "', 'PARSADVSEARCH','" + strSaveSearchName + "','" + "PARS" + "')\">" + strSaveSearchName + "</a></td><td width=\"15%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:ManageSearchCriteriaDetail('" + PARSADVSEARCHURL + "','" + strSaveSearchName + "')\">" + DETAILLINKTEXT + "</a></td><td width=\"15%\" class=\"savedSerchTxt\">");
                        linkDeleteButton.OnClientClick = "javascript:return DeleteSaveSearch('PARS','" + strSaveSearchName + "');";
                        linkDeleteButton.RenderControl(writer);
                        writer.Write("</td><td width=\"15%\"><a href=\"Javascript:ModifySaveSearch('" + PARSADVSEARCHURL + "','" + strSaveSearchName + "')\">" + MODIFYLINKTEXT + "</a></td></tr>");
                    }
                }
                else
                {
                    foreach(string strSaveSearchName in arlPARSSaveSearchNames)
                    {
                        writer.Write("<tr><td width=\"5%\">&nbsp;</td><td width=\"55%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:StandardSearchResults('" + ADVSITEURL + "', 'PARSADVSEARCH','" + strSaveSearchName + "','" + "PARS" + "')\">" + strSaveSearchName + "</a></td><td width=\"40%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:StandardSearchCriteriaDetail('" + PARSADVSEARCHURL + "','" + strSaveSearchName + "')\">" + DETAILLINKTEXT + "</a></td></tr>");
                    }
                }
                writer.Write("</table></td></tr>");
            }
        }

        /// <summary>
        /// Renders the LOGS save searches.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderLOGSSaveSearches(HtmlTextWriter writer)
        {
            //validates the Savedsercahes.
            if(arlLOGSSaveSearchNames.Count > 0)
            {
                writer.Write("<tr><td valign=\"middle\" class=\"savedsearchHeaderGray\">");
                writer.Write("<div id=\"dvi_col_exp_3\" style=\"float:left\"><img src=\"/_layouts/DREAM/images/plus.gif\" alt=\"open\" width=\"11\" height=\"11\" border=\"0\" hspace=\"0\" onclick=\"hideExpand(3)\" /></div>");
                writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;<b>" + LOGSBYFIELD + "</b></td></tr>");
                writer.Write("<tr id=\"dvi_col_3\" style=\"display:none;\"><td valign=\"middle\">");
                writer.Write("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                //Loop through the Saved Searches.
                if(string.Equals(Page.Request.QueryString["Type"].ToString(), MANAGESEARCH))
                {
                    foreach(string strSaveSearchName in arlLOGSSaveSearchNames)
                    {
                        writer.Write("<tr><td width=\"5%\">&nbsp;</td><td width=\"40%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:ManageSearchResults('" + ADVSITEURL + "', 'Logs By Field Depth','" + strSaveSearchName + "','" + "Logs By Field Depth" + "')\">" + strSaveSearchName + "</a></td><td width=\"15%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:ManageSearchCriteriaDetail('" + LOGBYFIELDADVSEARCHURL + "','" + strSaveSearchName + "')\">" + DETAILLINKTEXT + "</a></td><td width=\"15%\" class=\"savedSerchTxt\">");
                        linkDeleteButton.OnClientClick = "javascript:return DeleteSaveSearch('Logs By Field Depth','" + strSaveSearchName + "');";
                        linkDeleteButton.RenderControl(writer);
                        writer.Write("</td><td width=\"15%\"><a href=\"Javascript:ModifySaveSearch('" + LOGBYFIELDADVSEARCHURL + "','" + strSaveSearchName + "')\">" + MODIFYLINKTEXT + "</a></td></tr>");
                    }
                }
                else
                {
                    foreach(string strSaveSearchName in arlLOGSSaveSearchNames)
                    {
                        writer.Write("<tr><td width=\"5%\">&nbsp;</td><td width=\"55%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:StandardSearchResults('" + ADVSITEURL + "', 'Logs By Field Depth','" + strSaveSearchName + "','" + "Logs By Field Depth" + "')\">" + strSaveSearchName + "</a></td><td width=\"40%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:StandardSearchCriteriaDetail('" + LOGBYFIELDADVSEARCHURL + "','" + strSaveSearchName + "')\">" + DETAILLINKTEXT + "</a></td></tr>");
                    }
                }
                writer.Write("</table></td></tr>");
            }
        }
        /// <summary>
        /// Renders the Field save searches.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderFieldSaveSearches(HtmlTextWriter writer)
        {
            //validates the Savedsercahes.
            if(arlFieldSaveSearchNames.Count > 0)
            {
                writer.Write("<tr><td valign=\"middle\" class=\"savedsearchHeaderGray\">");
                writer.Write("<div id=\"dvi_col_exp_4\" style=\"float:left\"><img src=\"/_layouts/DREAM/images/plus.gif\" alt=\"open\" width=\"11\" height=\"11\" border=\"0\" hspace=\"0\" onclick=\"hideExpand(4)\" /></div>");
                writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;<b>Field Advanced Search</b></td></tr>");
                writer.Write("<tr id=\"dvi_col_4\" style=\"display:none;\"><td valign=\"middle\">");
                writer.Write("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                //Loop through the Saved Searches.
                if(string.Equals(Page.Request.QueryString["Type"].ToString(), MANAGESEARCH))
                {
                    foreach(string strSaveSearchName in arlFieldSaveSearchNames)
                    {
                        writer.Write("<tr><td width=\"5%\">&nbsp;</td><td width=\"40%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:ManageSearchResults('" + ADVSITEURL + "', 'Field Advance Search','" + strSaveSearchName + "','" + "Field" + "')\">" + strSaveSearchName + "</a></td><td width=\"15%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:ManageSearchCriteriaDetail('" + FIELDADVSEARCHURL + "','" + strSaveSearchName + "')\">" + DETAILLINKTEXT + "</a></td><td width=\"15%\" class=\"savedSerchTxt\">");
                        linkDeleteButton.OnClientClick = "javascript:return DeleteSaveSearch('Field Advance Search','" + strSaveSearchName + "');";
                        linkDeleteButton.RenderControl(writer);
                        writer.Write("</td><td width=\"15%\"><a href=\"Javascript:ModifySaveSearch('" + FIELDADVSEARCHURL + "','" + strSaveSearchName + "')\">" + MODIFYLINKTEXT + "</a></td></tr>");
                    }
                }
                else
                {
                    foreach(string strSaveSearchName in arlFieldSaveSearchNames)
                    {
                        writer.Write("<tr><td width=\"5%\">&nbsp;</td><td width=\"55%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:StandardSearchResults('" + ADVSITEURL + "', 'Field Advance Search','" + strSaveSearchName + "','" + "Field" + "')\">" + strSaveSearchName + "</a></td><td width=\"40%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:StandardSearchCriteriaDetail('" + FIELDADVSEARCHURL + "','" + strSaveSearchName + "')\">" + DETAILLINKTEXT + "</a></td></tr>");
                    }
                }
                writer.Write("</table></td></tr>");
            }
        }


        /// <summary>
        /// Renders the Field save searches.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderReservoirSaveSearches(HtmlTextWriter writer)
        {
            //validates the Savedsercahes.
            if(blnSRPEnabled && arlReservoirAdvSaveSearchNames.Count > 0)
            {
                writer.Write("<tr><td valign=\"middle\" class=\"savedsearchHeaderGray\">");
                writer.Write("<div id=\"dvi_col_exp_12\" style=\"float:left\"><img src=\"/_layouts/DREAM/images/plus.gif\" alt=\"open\" width=\"11\" height=\"11\" border=\"0\" hspace=\"0\" onclick=\"hideExpand(12)\" /></div>");
                writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;<b>" + RESERVOIRSEARCHTYPE + "</b></td></tr>");
                writer.Write("<tr id=\"dvi_col_12\" style=\"display:none;\"><td valign=\"middle\">");
                writer.Write("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                //Loop through the Saved Searches.
                if(string.Equals(Page.Request.QueryString["Type"].ToString(), MANAGESEARCH))
                {
                    foreach(string strSaveSearchName in arlReservoirAdvSaveSearchNames)
                    {
                        writer.Write("<tr><td width=\"5%\">&nbsp;</td><td width=\"40%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:ManageSearchResults('" + ADVSITEURL + "', '" + RESERVOIRSEARCHTYPE + "','" + strSaveSearchName + "','" + RESERVOIR + "')\">" + strSaveSearchName + "</a></td><td width=\"15%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:ManageSearchCriteriaDetail('" + RESERVOIRADVSEARCHURL + "','" + strSaveSearchName + "')\">" + DETAILLINKTEXT + "</a></td><td width=\"15%\" class=\"savedSerchTxt\">");
                        linkDeleteButton.OnClientClick = "javascript:return DeleteSaveSearch('" + RESERVOIRSEARCHTYPE + "','" + strSaveSearchName + "');";
                        linkDeleteButton.RenderControl(writer);
                        writer.Write("</td><td width=\"15%\"><a href=\"Javascript:ModifySaveSearch('" + RESERVOIRADVSEARCHURL + "','" + strSaveSearchName + "')\">" + MODIFYLINKTEXT + "</a></td></tr>");
                    }
                }
                else
                {
                    foreach(string strSaveSearchName in arlReservoirAdvSaveSearchNames)
                    {
                        writer.Write("<tr><td width=\"5%\">&nbsp;</td><td width=\"55%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:StandardSearchResults('" + ADVSITEURL + "', '" + RESERVOIRSEARCHTYPE + "','" + strSaveSearchName + "','" + RESERVOIR + "')\">" + strSaveSearchName + "</a></td><td width=\"40%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:StandardSearchCriteriaDetail('" + RESERVOIRADVSEARCHURL + "','" + strSaveSearchName + "')\">" + DETAILLINKTEXT + "</a></td></tr>");
                    }
                }
                writer.Write("</table></td></tr>");
            }
        }


        /// <summary>
        /// Renders the Field save searches.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderBasinSaveSearches(HtmlTextWriter writer)
        {
            //validates the Savedsercahes.
            if(arlBasinSaveSearchNames.Count > 0)
            {
                writer.Write("<tr><td valign=\"middle\" class=\"savedsearchHeaderGray\">");
                writer.Write("<div id=\"dvi_col_exp_5\" style=\"float:left\"><img src=\"/_layouts/DREAM/images/plus.gif\" alt=\"open\" width=\"11\" height=\"11\" border=\"0\" hspace=\"0\" onclick=\"hideExpand(5)\" /></div>");
                writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;<b>Basin Advanced Search</b></td></tr>");
                writer.Write("<tr id=\"dvi_col_5\" style=\"display:none;\"><td valign=\"middle\">");
                writer.Write("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                //Loop through the Saved Searches.
                if(string.Equals(Page.Request.QueryString["Type"].ToString(), MANAGESEARCH))
                {
                    foreach(string strSaveSearchName in arlBasinSaveSearchNames)
                    {
                        writer.Write("<tr><td width=\"5%\">&nbsp;</td><td width=\"40%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:ManageSearchResults('" + ADVSITEURL + "', 'Basin Advance Search','" + strSaveSearchName + "','" + "Basin" + "')\">" + strSaveSearchName + "</a></td><td width=\"15%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:ManageSearchCriteriaDetail('" + BASINADVSEARCHURL + "','" + strSaveSearchName + "')\">" + DETAILLINKTEXT + "</a></td><td width=\"15%\" class=\"savedSerchTxt\">");
                        linkDeleteButton.OnClientClick = "javascript:return DeleteSaveSearch('Basin Advance Search','" + strSaveSearchName + "');";
                        linkDeleteButton.RenderControl(writer);
                        writer.Write("</td><td width=\"15%\"><a href=\"Javascript:ModifySaveSearch('" + BASINADVSEARCHURL + "','" + strSaveSearchName + "')\">" + MODIFYLINKTEXT + "</a></td></tr>");
                    }
                }
                else
                {
                    foreach(string strSaveSearchName in arlBasinSaveSearchNames)
                    {
                        writer.Write("<tr><td width=\"5%\">&nbsp;</td><td width=\"55%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:StandardSearchResults('" + ADVSITEURL + "', 'Basin Advance Search','" + strSaveSearchName + "','" + "Basin" + "')\">" + strSaveSearchName + "</a></td><td width=\"40%\" class=\"savedSerchTxt\">&nbsp;<a href=\"Javascript:StandardSearchCriteriaDetail('" + BASINADVSEARCHURL + "','" + strSaveSearchName + "')\">" + DETAILLINKTEXT + "</a></td></tr>");
                    }
                }
                writer.Write("</table></td></tr>");
            }
        }
        #endregion
    }
}

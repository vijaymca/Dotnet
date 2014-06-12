#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: SearchResults.cs 
#endregion
/// <summary> 
/// This is Search results webpart class
/// </summary>
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Web;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.WebParts.DREAM.SearchResults
{
    /// <summary>
    /// This class is to display QuickSearch, Advanced Search, Map Search results.
    /// </summary>
    public class SearchResults :SearchResultsHelper
    {
        #region Declaration
        #region Constants
        const string VALIDDOCUMENTEXCEPTION = "File must be text only.";
        const string RESERVOIRADVANCESEARCH = "Reservoir Advance Search";
        const string ANYCOUNTRYQUERYSTRING = "ANYCOUNTRY";
        #endregion

        #region Variables
        string strSaveSearchName = string.Empty;
        string strSearchName = string.Empty;
        string strWindowTitleName = string.Empty;
        bool blnReorderColumn = true;
        QuickSearchHelper objQuickSearchHelper = new QuickSearchHelper();
        #endregion

        #region Controls
        HiddenField hidListSearch = new HiddenField();
        HiddenField hidListClick = new HiddenField();
        HyperLink linkMyAssets = new HyperLink();
        HyperLink linkMyTeam = new HyperLink();
        #endregion
        #endregion

        #region Webpart Properties
        /// <summary>
        ///  DREAM 3.0 Changes
        /// </summary>
        #region ReorderColumn
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDisplayNameAttribute("Reorder Column")]
        [WebDescription("Check if Reordering of column is applicable.")]
        public bool ReorderColumn
        {
            get
            {
                return blnReorderColumn;
            }
            set
            {
                blnReorderColumn = value;
            }
        }
        #endregion
        #endregion

        #region Protected Methods
        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            //Fix for the UpdatePanel postback behaviour.
            //Dream 4.0 code start
            objCommonUtility.EnsurePanelFix(this.Page, typeof(SearchResults));
            //Dream 4.0 code end
            strCurrSiteUrl = HttpContext.Current.Request.Url.ToString();
            string strDetailReportName = string.Empty;
            try
            {
                intMaxRecords = Convert.ToInt32(PortalConfiguration.GetInstance().GetKey(MAXRECORDS));
                objReportController = objFactory.GetServiceManager(REPORTSERVICE);
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                objQueryController = objFactory.GetServiceManager(QUERYSERVICE);

                #region CreatingControls
                CreateMessageLabel();
                CreateHiddenControl();
                CreateRadioControls();
                CreateHyperLinks();
                CreateShowOnMap();
                CreateMyAssetLinks();
                #endregion
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
        }


        /// <summary>
        /// Creates my asset links.
        /// </summary>
        private void CreateMyAssetLinks()
        {
            linkMyAssets.ID = "linkMyAssets";
            linkMyAssets.CssClass = "resultHyperLink";
            linkMyAssets.ImageUrl = "/_layouts/DREAM/images/addtocart.gif";
            linkMyAssets.NavigateUrl = "javascript:openMyAssetWindow();";
            this.Controls.Add(linkMyAssets);

            linkMyTeam.ID = "linkMyTeam";
            linkMyTeam.CssClass = "resultHyperLink";
            linkMyTeam.ImageUrl = "/_layouts/DREAM/images/addtocart.gif";
            linkMyTeam.NavigateUrl = "javascript:openTeamWindow();";
            this.Controls.Add(linkMyTeam);
        }
        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            hidSearchName.RenderControl(writer);
            try
            {
                if((Page.Request.Params.Get(EVENTTARGET) != null) && (Page.Request.Params.Get(EVENTTARGET).ToLower().Contains(UPDATEPANELID)))
                {
                    hstblParams = objCommonUtility.GetPagingSortingParams(Page.Request.Params.Get(EVENTARGUMENT));
                }
                SetSearchName();
                hidMapIdentified.RenderControl(writer);
                hidListSearch.ID = "hidListSearch";
                hidListSearch.RenderControl(writer);
                hidListClick.ID = "hidListClick";
                hidListClick.RenderControl(writer);

                RenderPage(writer);
            }
            #region Exception Handling
            catch(XmlException xmlEx)
            {
                CommonUtility.HandleException(strCurrSiteUrl, xmlEx);
            }
            catch(SoapException soapEx)
            {
                if(!string.Equals(soapEx.Message.ToString(), NORECORDSFOUND))
                {
                    CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                }
                if(soapEx.Message.Contains("Illegal character entity"))
                {
                    CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                    RenderException(writer, VALIDDOCUMENTEXCEPTION, strWindowTitleName, true);
                }
                else
                {
                    RenderException(writer, soapEx.Message, strWindowTitleName, true);
                }
            }
            catch(WebException webEx)
            {
                CommonUtility.HandleException(strCurrSiteUrl, webEx, 1);
                RenderException(writer, webEx.Message, strWindowTitleName, true);
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
            finally
            {
                RegisterClientSideScript();
            }
            #endregion

        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Renders the page.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderPage(HtmlTextWriter writer)
        {
            base.ResponseType = TABULAR;
            hidQuickSearch.Value = "QuickSearch";
            objRequestInfo = new RequestInfo();
            object objSessionUserPreference = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString());
            if(objSessionUserPreference != null)
                objUserPreferences = (UserPreferences)objSessionUserPreference;
            //Checks whether user has done a QuickSearch or advanced search.
            #region QuickSearchResultXML
            if(strSearchType.Length == 0)
            {
                //creates the SearchRequestXML for Quick Search
                CreateQuickSearchRequestXML();
                hidSearchType.Value = strAsset;
            }
            #endregion
            #region SearchResultResultXML
            else
            {
                //creates the SearchRequestXML for Advanced Search
                CreateAdvancedSearchRequestXML();
                hidSearchType.Value = strSearchType;
            }
            if(Page.Request.QueryString["asset"] != null)//used in reorder column
            {
                hidAssetName.Value = Page.Request.QueryString["asset"].Trim();
            }
            #endregion
            if(xmlDocSearchResult != null)
            {
                //Dream 4.0 code start
                objReorder = new Reorder();
                //** ReorderColumn 
                if((ReorderColumn) && (!strSearchType.Equals("Query Search")))
                {
                    //function for reordering of column
                    objReorder.ManageReorder(strAsset, strAsset, xmlDocSearchResult, hidReorderColValue, hidReorderSourceId);
                    hidReorderColValue.RenderControl(writer);
                    hidReorderSourceId.RenderControl(writer);
                    xmlNdLstReorderColumn = objReorder.GetColumnNodeList(strAsset, strAsset, xmlDocSearchResult);
                    strReorderDivHTML = objReorder.GetReorderDivHTML(xmlNdLstReorderColumn);
                    objReorder.SetColNameDisplayStatus(out strColNames, out strColDisplayStatus, xmlNdLstReorderColumn);

                }
                //Dream 4.0 
                SetColumnNamesHiddenField();
                //** End

                hidMaxRecord.Value = intMaxRecords.ToString();
                GetRequestIDFromResults();
                //sets the MapZoomidentifiedColumn.
                SetMapUseColumnName(xmlDocSearchResult);
                hidMapUseColumnName.RenderControl(writer);
                SetHiddenFieldValues();
                //sets the user preferences. 
                SetUserPreference();
                DisplaySearchResults(writer);

            }
            else
            {
                RenderParentTable(writer, strWindowTitleName);
                writer.Write("<tr><td><br/>");
                lblMessage.Visible = true;
                lblMessage.Text = ((MOSSServiceManager)objMossController).GetCustomExceptionMessage(strCurrSiteUrl, "1");
                //this will render the custom error messege.
                lblMessage.RenderControl(writer);
                writer.Write("</td></tr></table>");
            }
        }
        /// <summary>
        /// Displays the search results.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void DisplaySearchResults(HtmlTextWriter writer)
        {
            string strPerformanceRecordLimit = string.Empty;
            RenderParentTable(writer, strWindowTitleName);
            RenderPrintExport(writer);
            //Dispalying performance warning message for selection of more records
            if(!string.IsNullOrEmpty(strPerformanceRecordLimit = PortalConfiguration.GetInstance().GetKey("PerformanceRecordLimit")) && (GetRecordCountFromResults(xmlDocSearchResult) > Convert.ToInt32(strPerformanceRecordLimit)) && (!string.IsNullOrEmpty(PortalConfiguration.GetInstance().GetKey("PerformanceWarning"))))
            {
                writer.Write("<tr><td valign=\"bottom\"><span class=\"warningstyle\">* ");
                writer.Write(PortalConfiguration.GetInstance().GetKey("PerformanceWarning"));
                writer.Write("</span></td><td colspan=\"3\" align=\"right\">");
            }
            else
            {
                writer.Write("<tr><td colspan=\"4\" align=\"right\">");
            }

            #region Render 'Add to My Team's Assets' & 'Add to My Assets' links
            if(!strSearchType.Equals("Query Search"))
            {

                if(((MOSSServiceManager)objMossController).IsCurrentUserTeamOwner(strCurrSiteUrl, objCommonUtility.GetUserName()))
                {
                    //this will render the 'Add to My Team's Assets' link control
                    writer.Write("Add to My Team's Assets");
                    writer.Write("&nbsp;");
                    linkMyTeam.RenderControl(writer);
                    writer.Write("&nbsp;&nbsp;&nbsp;");
                }
                //this will render the 'Add to My Assets' link control

                writer.Write("Add to My Assets");
                writer.Write("&nbsp;");
                linkMyAssets.RenderControl(writer);
                writer.Write("&nbsp;&nbsp;&nbsp;");

            }
            #endregion

            #region ZoomRendering
            //this will render the XSLZoom control.
            /// NOTE: strSearchType is empty for Quick Search. Condition fails for Reservoir Quick Search.
            /// So comparing with strAsset is also added for Hiding "Show on Map" for "Reservoir" quick search.
            if(PortalConfiguration.GetInstance().GetKey("MapDisplay").ToLower().Equals("on") && (!strSearchType.Equals("Query Search")) && (!strSearchType.Equals("Logs By Field Depth")) && (!strSearchType.ToLowerInvariant().Equals(RESERVOIRADVANCESEARCH.ToLowerInvariant())) && (!strSearchType.ToLowerInvariant().Equals("reservoir")) && (!strSearchType.ToLowerInvariant().Equals("listofreservoirs")) && (!strAsset.ToLowerInvariant().Equals("reservoir")))
            {
                btnShowOnMap.RenderControl(writer);
            }
            hidCheckedColumns.RenderControl(writer);
            #endregion

            writer.Write("</td></tr>");
            RenderHiddenControls(writer);
            RenderResult(writer, blnDisplayFeetMeter, hidSearchType.Value);

            //Added ni DREAM 4.0 for checkbox state management across postback
            objCommonUtility.RegisterOnLoadClientScript(this.Page, "SelectSelectAllCheckBox('chkbxRowSelectAll','tblSearchResults','tbody','chkbxRow');SelectSelectAllCheckBox('chkbxColumnSelectAll','tblSearchResults','thead','chkbxColumn');");
        }
        /// <summary>
        /// Sets the hidden field values.
        /// </summary>
        private void SetHiddenFieldValues()
        {
            #region SearchResultsHiddenFieldValue
            if(!string.Equals(strSearchName, "Quick Search"))
            {
                //below switch case sets the hidden report type value based on search type.
                switch(strSearchType)
                {
                    case "Well Wellbore":
                        hidReportType.Value = strSearchType;
                        break;
                    case "PARSADVSEARCH":
                        hidReportType.Value = strSearchType;
                        break;
                    case "Logs By Field Depth":
                        hidReportType.Value = strSearchType;
                        blnDisplayFeetMeter = true;
                        break;
                    default:
                        hidReportType.Value = strSearchType;
                        break;
                }
            }
            #endregion
        }
        /// <summary>
        /// Sets the type of the search.
        /// </summary>
        /// <param name="strSearchType">Type of the STR search.</param>
        /// <returns></returns>
        private void SetSearchName()
        {
            //this condition will check for searchtype.
            if(Page.Request.QueryString["SearchType"] != null)
            {
                strSearchType = Page.Request.QueryString["SearchType"];
                //Below switch case assign the value for hidden SearchName field based on SearchType.
                switch(strSearchType.ToUpper())
                {
                    case "WELL ADVANCE SEARCH":
                        strWindowTitleName = "Well Advanced Search Results";
                        strSearchName = strSearchType;
                        hidSearchName.Value = strSearchType;
                        break;
                    case "WELLBORE ADVANCE SEARCH":
                        strWindowTitleName = "Wellbore Advanced Search Results";
                        strSearchName = strSearchType;
                        hidSearchName.Value = strSearchType;
                        break;
                    case "PARSADVSEARCH":
                        strWindowTitleName = "Project Archives Search Results";
                        strSearchName = strSearchType;
                        hidSearchName.Value = "Project Archive Search";
                        blnDisplayiRequest = true;
                        break;
                    case "LOGS BY FIELD DEPTH":
                        strWindowTitleName = "Logs By Field Depth Search Results";
                        strSearchName = strSearchType;
                        hidSearchName.Value = strSearchType + " Search";
                        break;
                    case "FIELD ADVANCE SEARCH":
                        strWindowTitleName = "Field Advanced Search Results";
                        strSearchName = strSearchType;
                        hidSearchName.Value = strSearchType;
                        break;
                    case "BASINSEARCH":
                        strWindowTitleName = "Basin Advanced Search Results";
                        strSearchName = strSearchType;
                        hidSearchName.Value = strSearchType;
                        break;
                    case "BASIN ADVANCE SEARCH":
                        strWindowTitleName = "Basin Advanced Search Results";
                        strSearchName = strSearchType;
                        hidSearchName.Value = strSearchType;
                        break;
                    case "MapSearch":
                        strWindowTitleName = "Map Search Results";
                        strSearchName = strSearchType;
                        hidSearchName.Value = strSearchType;
                        break;
                    case "QUERY SEARCH":
                        strWindowTitleName = "Query Search Results";
                        strSearchName = strSearchType;
                        hidSearchName.Value = strSearchType;
                        break;
                    case "RESERVOIR ADVANCE SEARCH":
                        strWindowTitleName = "Reservoir Advanced Search Results";
                        strSearchName = strSearchType;
                        hidSearchName.Value = strSearchType;
                        break;
                    default:
                        strWindowTitleName = "Advanced Search";
                        strSearchName = strSearchType;
                        hidSearchName.Value = strSearchType;
                        break;
                }
            }
            else
            {
                strSearchName = "Quick Search";
                //this condition valiates the asset querystring value.
                if(Page.Request.QueryString["asset"] != null)
                {
                    strWindowTitleName = "Quick Search - " + Page.Request.QueryString["asset"] + " Results";
                }
                else if(Page.Request.QueryString["listSearchType"] != null)
                {
                    if(Page.Request.QueryString["listSearchType"].ToUpper().Equals("LISTOFWELLS"))
                        strWindowTitleName = "List of Wells Result";
                    else if(Page.Request.QueryString["listSearchType"].ToUpper().Equals("LISTOFRESERVOIRS")) /// Add for SRP "List of Reservoir Search"
                        strWindowTitleName = "List of Reservoir Result";
                    else if(Page.Request.QueryString["listSearchType"].ToUpper().Equals("LISTOFFIELDS")) /// Add for SRP "List of Reservoir Search"
                        strWindowTitleName = "List of Field Result";
                    else
                        strWindowTitleName = "List of Wellbore Results";
                }
            }
        }
        /// <summary>
        /// Creates the quick search request XML.
        /// </summary>
        private void CreateQuickSearchRequestXML()
        {
            if(Page.Request.QueryString.Count > 0)
            {
                SetQuickSearchRequestInfo();
                //Dream 4.0
                //saving request xml for export all
                SaveRequestXmlForExportAll(objRequestInfo);

                //Dream 4.0 paging requirement starts
                SetSkipInfo(objRequestInfo.Entity);
                //Dream 4.0 paging requirement ends

                //call for the GetSearchResults() method to fetch the search results from webservice.
                xmlDocSearchResult = objReportController.GetSearchResults(objRequestInfo, intMaxRecords, strAsset, strSortColumn, intSortOrder);

            }
        }

        /// <summary>
        /// Gets the quick search request info.
        /// </summary>
        private void SetQuickSearchRequestInfo()
        {
            //this will validates the country value selected and set the value to strCountry variable.
            if(Page.Request.QueryString["country"] != null)
            {
                if(string.Equals(Page.Request.QueryString["country"].Trim(), "0"))
                {
                    objQuickSearchHelper.Country = ANYCOUNTRYQUERYSTRING;
                }
                else
                {
                    objQuickSearchHelper.Country = Page.Request.QueryString["country"].Trim();
                }
            }
            if(Page.Request.QueryString["criteria"] != null)
            {
                objQuickSearchHelper.Criteria = this.Page.Server.UrlDecode(Page.Request.QueryString["criteria"]);
            }
            //this will validates the asset value selected and set the value to strAsset variable.
            if(Page.Request.QueryString["asset"] != null)
            {
                strAsset = Page.Request.QueryString["asset"].Trim();
                objQuickSearchHelper.Asset = strAsset;
                if(strAsset.Equals("Project Archives"))
                {
                    blnDisplayiRequest = true;
                }
            }
            //this will validates the column value selected and set the value to strColumn variable if the asset is not Basin and Field.
            //R5k changes
            // if (!(string.Equals(strAsset, "Basin")) && !(string.Equals(strAsset, "Field")))
            // {
            if(Page.Request.QueryString["column"] != null)
            {
                objQuickSearchHelper.Column = Page.Request.QueryString["column"].Trim();
                strSortColumn = objQuickSearchHelper.Column;
            }
            // }
            //the below condition check for whether the listSearch option is selected.
            if(Page.Request.QueryString["listSearchType"] != null)
            {
                SetListSearchProperties();
            }
            objQuickSearchHelper.CurrSiteUrl = strCurrSiteUrl;
            objRequestInfo = objQuickSearchHelper.SetQuickDataObjects();
            /// SRP Changes
            /// Add the Entity Name for the "Reservoir" Quick Search
            if(string.Equals(strAsset.ToLowerInvariant(), "reservoir"))
            {
                if(objRequestInfo != null && objRequestInfo.Entity != null)
                {
                    objRequestInfo.Entity.Name = RESERVOIRADVANCESEARCH;
                }
            }
        }

        /// <summary>
        /// Sets the list search properties.
        /// </summary>
        private void SetListSearchProperties()
        {
            string strListSearch = Page.Request.QueryString["listSearchType"];
            strSearchType = strListSearch;
            objQuickSearchHelper.List = strListSearch;
            if(strListSearch.ToLowerInvariant().Equals("listofwells"))
            {
                objQuickSearchHelper.Asset = "Well";
                strAsset = "Well";
            }
            else if(strListSearch.ToLowerInvariant().Equals("listofreservoirs")) /// Added for SRP "List of Reservoir" Search
            {
                objQuickSearchHelper.Asset = "Reservoir";
                strAsset = "Reservoir";
            }
            else if(strListSearch.ToLowerInvariant().Equals("listoffields")) /// Added for SRP "List of Reservoir" Search
            {
                objQuickSearchHelper.Asset = "Field";
                strAsset = "Field";
            }
            else
            {
                objQuickSearchHelper.Asset = "wellbore";
                strAsset = "Wellbore";
            }
        }
        /// <summary>
        /// Creates the advanced search request XML.
        /// </summary>
        private void CreateAdvancedSearchRequestXML()
        {
            object objAdvancedRequestXML = null;
            if(Page.Request.QueryString.Count > 0)
            {
                //the below condition validates the SearchType querystring value and assigns value for searchtype variable.
                if(Page.Request.QueryString["SearchType"] != null)
                {
                    strSearchType = Page.Request.QueryString["SearchType"];
                }
                objAdvancedRequestXML = CommonUtility.GetSessionVariable(Page, strSearchType);
                if(Page.Request.QueryString["asset"] != null)
                {
                    strAsset = Page.Request.QueryString["asset"].Trim();
                }
                if(Page.Request.QueryString["savesearchname"] != null)
                {
                    string strUserID = string.Empty;
                    if(Page.Request.QueryString["manage"] != null)
                    {
                        if(string.Equals(Page.Request.QueryString["manage"], "true"))
                            strUserID = GetUserName();
                        else
                            strUserID = "Administrator";
                    }

                    XmlDocument xmlDocSearchRequest = new XmlDocument();
                    strSaveSearchName = Page.Request.QueryString["savesearchname"];
                    if(string.Equals(strSearchType, "PARSADVSEARCH"))
                    {
                        xmlDocSearchRequest = ((MOSSServiceManager)objMossController).GetAdminSaveSearchXML("PARS", strUserID, strSaveSearchName);
                    }
                    else
                    {
                        xmlDocSearchRequest = ((MOSSServiceManager)objMossController).GetAdminSaveSearchXML(strSearchType, strUserID, strSaveSearchName);
                    }
                    /// Called from My Search and Shared Searches.
                    xmlDocSearchRequest = GetSRPSearchType(xmlDocSearchRequest, true);
                    /// Changes for SRP Ends here                    
                    //call for the GetSearchResults() method to fetch the search results from webservice.
                    //Dream 4.0 paging impplementation starts
                    //saving request xml for export all
                    SaveRequestXmlForExportAll(xmlDocSearchRequest);
                    SetSkipInfo(xmlDocSearchRequest);
                    //Dream 4.0 paging impplementation ends
                    if(strSearchType.Equals("Query Search"))
                    {
                        RemoveTypeAttribute(xmlDocSearchRequest);
                        xmlDocSearchResult = objQueryController.GetSearchResults(xmlDocSearchRequest, intMaxRecords, strSearchType, null, intSortOrder);
                    }
                    else
                    {
                        //call for the GetSearchResults() method to fetch the search results from webservice.
                        xmlDocSearchResult = objReportController.GetSearchResults(xmlDocSearchRequest, intMaxRecords, strSearchType, null, intSortOrder);
                    }
                }
                else
                {
                    if(objAdvancedRequestXML != null)
                    {
                        if(!string.IsNullOrEmpty((string)objAdvancedRequestXML))
                        {
                            xmlSearchRequest.LoadXml((string)objAdvancedRequestXML);
                            RemoveTypeAttribute(xmlSearchRequest);
                            /// Called when "Search" click from Adv Search Screen
                            xmlSearchRequest = GetSRPSearchType(xmlSearchRequest, false);
                            //Dream 4.0 paging impplementation starts
                            //saving request xml for export all
                            SaveRequestXmlForExportAll(xmlSearchRequest);
                            SetSkipInfo(xmlSearchRequest);
                            //Dream 4.0 paging impplementation ends
                            if(strSearchType.Equals("Query Search"))
                            {
                                if(Page.Request.QueryString["column"] != null)
                                {
                                    xmlDocSearchResult = objQueryController.GetSearchResults(xmlSearchRequest, intMaxRecords, strSearchType, Page.Request.QueryString["column"], intSortOrder);
                                }
                                else
                                {
                                    xmlDocSearchResult = objQueryController.GetSearchResults(xmlSearchRequest, intMaxRecords, strSearchType, null, intSortOrder);
                                }
                            }
                            else
                            {
                                //call for the GetSearchResults() method to fetch the search results from webservice.
                                xmlDocSearchResult = objReportController.GetSearchResults(xmlSearchRequest, intMaxRecords, strSearchType, strSortColumn, intSortOrder);
                            }
                        }
                    }
                }
            }
            else
            {
                HttpContext.Current.Response.Redirect("/", false);
            }
        }

        /// <summary>
        /// Change strSearchType if it is SRP Field Advance Search.
        /// </summary>
        /// <param name="xmlDocSearchRequest">The XML doc search request.</param>
        private XmlDocument GetSRPSearchType(XmlDocument xmlDocSearchRequest, bool addAttributeGroup)
        {
            /// Changes for SRP Starts here
            if(Page.Request.QueryString["SearchType"] != null && string.Equals(Page.Request.QueryString["SearchType"].ToLowerInvariant(), "Field Advance Search".ToLowerInvariant()))
            {
                if(xmlDocSearchRequest != null && xmlDocSearchRequest.SelectSingleNode("requestinfo/entity").Attributes["name"] != null)
                {
                    if(string.Equals(xmlDocSearchRequest.SelectSingleNode("requestinfo/entity").Attributes["name"].Value.ToLowerInvariant(), "SRP Field Advance Search".ToLowerInvariant()))
                    {
                        strSearchType = xmlDocSearchRequest.SelectSingleNode("requestinfo/entity").Attributes["name"].Value;
                        xmlDocSearchRequest.SelectSingleNode("requestinfo/entity").Attributes["name"].Value = "Field Advance Search";
                        /// Commented since "Checked = "Exclude" is added to avoid attributes to be sent to webservice
                        /*
                        /// Remove the "Tectonic Setting" Radio Button list selection since there is no equivalent attribute in webservice
                        if (xmlDocSearchRequest.SelectSingleNode("requestinfo/entity/attributegroup/attribute[@name='lTectonicClassification']") != null)
                        {
                            XmlNode xmlNodeTectonicClassification = xmlDocSearchRequest.SelectSingleNode("requestinfo/entity/attributegroup/attribute[@name='lTectonicClassification']");
                            xmlDocSearchRequest.SelectSingleNode("requestinfo/entity/attributegroup").RemoveChild(xmlNodeTectonicClassification);
                        }*/
                        /// Intercept the request xml to accomodate the attribute group for ranges
                        if(addAttributeGroup)
                        {
                            xmlDocSearchRequest = GetSRPAdvanceSearchRequestXML(xmlDocSearchRequest);
                        }
                    }
                }
            }
            else if(Page.Request.QueryString["SearchType"] != null && string.Equals(Page.Request.QueryString["SearchType"].ToLowerInvariant(), RESERVOIRADVANCESEARCH.ToLowerInvariant()))
            {
                /// Intercept the request xml to accomodate the attribute group for ranges
                if(addAttributeGroup)
                {
                    xmlDocSearchRequest = GetSRPAdvanceSearchRequestXML(xmlDocSearchRequest);
                }
            }
            return xmlDocSearchRequest;
        }

        /// <summary>
        /// Gets the SRP advance search request XML.
        /// </summary>
        /// <param name="xmlDocSearchRequest">The XML doc search request.</param>
        /// <returns></returns>
        private XmlDocument GetSRPAdvanceSearchRequestXML(XmlDocument xmlDocSearchRequest)
        {
            RequestInfo objRequestInfo = new RequestInfo();
            ArrayList arlBasicAttribute = new ArrayList();
            ArrayList arlParentAttributeGroup = new ArrayList();
            ArrayList arlChildAttributeGroup = new ArrayList();
            ArrayList arlChildAttributes = new ArrayList();
            string strSearchPercentageVAlue = PortalConfiguration.GetInstance().GetKey("SearchPercentageValue");
            AttributeGroup objBasicAttributeGroup = null;
            AttributeGroup objParentAttributeGroup = null;
            foreach(XmlNode requestInfoNode in xmlDocSearchRequest.ChildNodes)
            {
                foreach(XmlNode entityNode in requestInfoNode.ChildNodes)
                {
                    objRequestInfo.Entity = new Entity();
                    objRequestInfo.Entity.Name = entityNode.Attributes["name"].Value;
                    if(entityNode.SelectNodes("attributegroup").Count > 0)
                    {
                        #region Attribute Group
                        foreach(XmlNode attributeGroupNode in entityNode.ChildNodes)
                        {
                            foreach(XmlNode attributeNode in attributeGroupNode.ChildNodes)
                            {
                                if(attributeNode.Attributes["IsRangeApplicable"] != null)
                                {
                                    if(attributeNode.Attributes["IsRangeApplicable"].Value.Equals("False"))
                                    {
                                        /// Add to direct AttributeGroup.Attributes Collection
                                        if(attributeNode.Attributes["checked"] != null)
                                        {
                                            if(!attributeNode.Attributes["checked"].Value.Equals("Exclude"))
                                            {
                                                arlBasicAttribute = SetUITextControls(attributeNode.Attributes["name"].Value, attributeNode.Attributes["label"].Value, attributeNode.Attributes["operator"].Value, arlBasicAttribute, false, strSearchPercentageVAlue, attributeNode.Attributes["checked"].Value, attributeNode.ChildNodes);
                                            }
                                        }
                                        else
                                        {
                                            arlBasicAttribute = SetUITextControls(attributeNode.Attributes["name"].Value, attributeNode.Attributes["label"].Value, attributeNode.Attributes["operator"].Value, arlBasicAttribute, false, strSearchPercentageVAlue, string.Empty, attributeNode.ChildNodes);
                                        }

                                    }
                                    else if(attributeNode.Attributes["IsRangeApplicable"].Value.Equals("True"))
                                    {
                                        /// Create an Attribute Group
                                        /// Create Attribute Collection
                                        if(attributeNode.Attributes["checked"] != null)
                                        {
                                            if(!attributeNode.Attributes["checked"].Value.Equals("Exclude"))
                                            {
                                                arlChildAttributeGroup = SetUITextControls(attributeNode.Attributes["name"].Value, attributeNode.Attributes["label"].Value, attributeNode.Attributes["operator"].Value, arlChildAttributeGroup, true, strSearchPercentageVAlue, attributeNode.Attributes["checked"].Value, attributeNode.ChildNodes);
                                            }
                                        }
                                        else
                                        {
                                            arlChildAttributeGroup = SetUITextControls(attributeNode.Attributes["name"].Value, attributeNode.Attributes["label"].Value, attributeNode.Attributes["operator"].Value, arlChildAttributeGroup, true, strSearchPercentageVAlue, string.Empty, attributeNode.ChildNodes);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        foreach(XmlNode attributeNode in entityNode.ChildNodes)
                        {
                            if(attributeNode.Attributes["IsRangeApplicable"] != null)
                            {
                                if(attributeNode.Attributes["IsRangeApplicable"].Value.Equals("False"))
                                {
                                    /// Add to direct AttributeGroup.Attributes Collection
                                    if(attributeNode.Attributes["checked"] != null)
                                    {
                                        if(!attributeNode.Attributes["checked"].Value.Equals("Exclude"))
                                        {
                                            arlBasicAttribute = SetUITextControls(attributeNode.Attributes["name"].Value, attributeNode.Attributes["label"].Value, attributeNode.Attributes["operator"].Value, arlBasicAttribute, false, strSearchPercentageVAlue, attributeNode.Attributes["checked"].Value, attributeNode.ChildNodes);
                                        }
                                    }
                                    else
                                    {
                                        arlBasicAttribute = SetUITextControls(attributeNode.Attributes["name"].Value, attributeNode.Attributes["label"].Value, attributeNode.Attributes["operator"].Value, arlBasicAttribute, false, strSearchPercentageVAlue, string.Empty, attributeNode.ChildNodes);
                                    }

                                }
                                else if(attributeNode.Attributes["IsRangeApplicable"].Value.Equals("True"))
                                {
                                    /// Create an Attribute Group
                                    /// Create Attribute Collection
                                    if(attributeNode.Attributes["checked"] != null)
                                    {
                                        if(!attributeNode.Attributes["checked"].Value.Equals("Exclude"))
                                        {
                                            arlChildAttributeGroup = SetUITextControls(attributeNode.Attributes["name"].Value, attributeNode.Attributes["label"].Value, attributeNode.Attributes["operator"].Value, arlChildAttributeGroup, true, strSearchPercentageVAlue, attributeNode.Attributes["checked"].Value, attributeNode.ChildNodes);
                                        }
                                    }
                                    else
                                    {
                                        arlChildAttributeGroup = SetUITextControls(attributeNode.Attributes["name"].Value, attributeNode.Attributes["label"].Value, attributeNode.Attributes["operator"].Value, arlChildAttributeGroup, true, strSearchPercentageVAlue, string.Empty, attributeNode.ChildNodes);
                                    }
                                }
                            }

                        }
                    }
                }
            }
            if(arlBasicAttribute.Count > 1)
            {
                objBasicAttributeGroup = new AttributeGroup();
                objBasicAttributeGroup.Operator = xmlDocSearchRequest.SelectSingleNode("requestinfo/entity/attributegroup").Attributes["operator"].Value;
                objBasicAttributeGroup.Attribute = arlBasicAttribute;

                if(arlChildAttributeGroup.Count > 0)
                {
                    arlChildAttributeGroup.Add(objBasicAttributeGroup);
                    objParentAttributeGroup = new AttributeGroup();
                    objParentAttributeGroup.Operator = xmlDocSearchRequest.SelectSingleNode("requestinfo/entity/attributegroup").Attributes["operator"].Value;
                    objParentAttributeGroup.AttributeGroups = arlChildAttributeGroup;
                    arlParentAttributeGroup = new ArrayList();
                    arlParentAttributeGroup.Add(objParentAttributeGroup);
                    objRequestInfo.Entity.AttributeGroups = arlParentAttributeGroup;
                }
                else
                {
                    arlParentAttributeGroup = new ArrayList();
                    arlParentAttributeGroup.Add(objBasicAttributeGroup);
                    objRequestInfo.Entity.AttributeGroups = arlParentAttributeGroup;
                }
            }
            else
            {
                objRequestInfo.Entity.Attribute = arlBasicAttribute;
            }
            xmlDocSearchRequest = objReportController.CreateSearchRequest(objRequestInfo);
            return xmlDocSearchRequest;
        }


        /// <summary>
        /// Sets the UI text controls.
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="attributeValue">The attribute value.</param>
        /// <param name="labelName">Name of the label.</param>
        /// <param name="operatorName">Name of the operator.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="calculateRange">if set to <c>true</c> [calculate range].</param>
        /// <param name="rangeValue">The range value.</param>
        /// <param name="attributeChecked">The attribute checked.</param>
        /// <returns></returns>
        private ArrayList SetUITextControls(string attributeName, string labelName, string operatorName, ArrayList attribute, bool calculateRange, string rangeValue, string attributeChecked, XmlNodeList childNodes)
        {
            string attributeValue = string.Empty;
            Attributes objAttribute = new Attributes();
            Attributes objAttributeMin = new Attributes();
            ArrayList arlValue = new ArrayList();
            ArrayList arlValueMin = new ArrayList();
            AttributeGroup objBasicAttributeGroup = new AttributeGroup();
            ArrayList objInnerAttribute = new ArrayList();
            double dblRange = Convert.ToDouble(rangeValue);
            string strNewAttributeValue = string.Empty;

            if(attributeName.Length > 0)
            {
                if(calculateRange && dblRange > 0)
                {
                    objAttribute.Name = attributeName;
                    string strValue;
                    foreach(XmlNode node in childNodes)
                    {
                        strValue = string.Empty;
                        attributeValue = string.Empty;
                        attributeValue = node.InnerText;
                        strValue = GetMaxRange(attributeValue, dblRange).ToString();
                        arlValue.Add(SetValue(strValue));
                    }
                    objAttribute.Value = arlValue;
                    objAttribute.Operator = LESSTHANEQUALS;
                    objAttribute.Label = labelName;
                    objAttribute.IsRangeApplicable = true;
                    objAttribute.Checked = attributeChecked;
                    objInnerAttribute.Add(objAttribute);

                    objAttributeMin.Name = attributeName;

                    foreach(XmlNode node in childNodes)
                    {
                        strValue = string.Empty;
                        attributeValue = string.Empty;
                        attributeValue = node.InnerText;
                        strValue = GetMinRange(attributeValue, dblRange).ToString();
                        arlValueMin.Add(SetValue(strValue));
                    }
                    objAttributeMin.Value = arlValueMin;
                    objAttributeMin.Operator = GREATERTHANEQUALS;
                    objAttributeMin.Label = labelName;
                    objAttributeMin.IsRangeApplicable = true;
                    objAttributeMin.Checked = attributeChecked;
                    objInnerAttribute.Add(objAttributeMin);
                    objBasicAttributeGroup.Operator = GetLogicalOperator();
                    objBasicAttributeGroup.Attribute = objInnerAttribute;
                    attribute.Add(objBasicAttributeGroup);
                }
                else
                {
                    objAttribute.Name = attributeName;
                    /// If Label Name = "cboReserveMagOil - Value column" | "cboReserveMagGas - Value column" | "cboHydrocarbonMain - HydrocarbonCode column"
                    /// Get Value field from SharePoint list and Assign
                    if(labelName.Equals("cboReserveMagOil")) /// Field Adv Search
                    {
                        foreach(XmlNode node in childNodes)
                        {
                            attributeValue = string.Empty;
                            attributeValue = node.InnerText;

                            strNewAttributeValue = string.Empty;
                            strNewAttributeValue = GetSRPValueFields("Reserve Magnitude Oil", "Value", "Title", attributeValue);
                            if(!string.IsNullOrEmpty(strNewAttributeValue))
                            {
                                arlValue.Add(SetValue(strNewAttributeValue));
                            }
                        }
                    }
                    else if(labelName.Equals("cboReserveMagGas")) /// Field Adv Search
                    {
                        strNewAttributeValue = string.Empty;
                        foreach(XmlNode node in childNodes)
                        {
                            attributeValue = string.Empty;
                            attributeValue = node.InnerText;

                            strNewAttributeValue = string.Empty;
                            strNewAttributeValue = GetSRPValueFields("Reserve Magnitude Gas", "Value", "Title", attributeValue);
                            if(!string.IsNullOrEmpty(strNewAttributeValue))
                            {
                                arlValue.Add(SetValue(strNewAttributeValue));
                            }
                        }
                    }
                    else if(labelName.Equals("cboHydrocarbonMain")) /// Reservoir Adv Search
                    {
                        strNewAttributeValue = string.Empty;
                        foreach(XmlNode node in childNodes)
                        {
                            attributeValue = string.Empty;
                            attributeValue = node.InnerText;

                            strNewAttributeValue = string.Empty;
                            strNewAttributeValue = GetSRPValueFields("HydroCarbon Main", "HydrocarbonCode", "Title", attributeValue);
                            if(!string.IsNullOrEmpty(strNewAttributeValue))
                            {
                                arlValue.Add(SetValue(strNewAttributeValue));
                            }
                        }
                    }
                    /// If Label Name = "cboTectonicSetting" | "cboTectonicSettingKle" 
                    /// Get Basin Values from sharepoint list and assign to Value fields
                    else if(labelName.Equals("cboTectonicSetting") || labelName.Equals("cboTectonicSettingKle")) /// Field Adv Search
                    {
                        foreach(XmlNode node in childNodes)
                        {
                            attributeValue = string.Empty;
                            attributeValue = node.InnerText;

                            arlValue = GetTectonicSettingsBasinNames("Tectonic Setting", attributeValue);
                        }

                        if(arlValue.Count > 1)
                        {
                            operatorName = INOPERATOR;
                        }
                    }
                    else
                    {
                        foreach(XmlNode node in childNodes)
                        {
                            attributeValue = node.InnerText;
                            arlValue.Add(SetValue(attributeValue));
                        }
                    }
                    objAttribute.Value = arlValue;
                    objAttribute.Operator = operatorName;
                    objAttribute.Label = labelName;
                    objAttribute.Checked = attributeChecked;
                    if(calculateRange)
                    {
                        objAttribute.IsRangeApplicable = true;
                    }
                    /// If Attribute doesn't contain value collection
                    /// Don't add to attribute collection
                    if(arlValue.Count > 0)
                    {
                        attribute.Add(objAttribute);
                    }
                }
            }
            return attribute;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="strField">The STR field.</param>
        /// <returns>Value object</returns>        
        private Value SetValue(string value)
        {
            Value objValue = new Value();
            objValue.InnerText = value;
            return objValue;
        }

        /// <summary>
        /// Gets the max range of user input.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="range">The range.</param>
        /// <returns></returns>
        private double GetMaxRange(string value, double range)
        {
            double dblValue = 0;
            double dblMaxValue = 0;
            if(!string.IsNullOrEmpty(value))
            {
                double.TryParse(value, out dblValue);
                dblMaxValue = dblValue + ((dblValue * range) / 100);
            }

            return dblMaxValue;
        }

        /// <summary>
        /// Gets the min range of user input.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="range">The range.</param>
        /// <returns></returns>
        private double GetMinRange(string value, double range)
        {
            double dblValue = 0;
            double dblMinValue = 0;
            if(!string.IsNullOrEmpty(value))
            {
                double.TryParse(value, out dblValue);
                dblMinValue = dblValue - ((dblValue * range) / 100);
            }

            return dblMinValue;
        }

        private string GetSRPValueFields(string listName, string columnName, string filterColumnName, string filterValue)
        {
            DataTable dtListValues = null;
            string strValue = string.Empty;
            objMossController = objFactory.GetServiceManager(MOSSSERVICE);

            string strCamlQuery = "<Where><Eq><FieldRef Name='" + filterColumnName + "' /><Value Type='Text'>" + filterValue + "</Value></Eq></Where>";
            string strViewField = "<FieldRef Name='" + columnName + "' /><FieldRef Name='" + filterColumnName + "' />";
            try
            {
                dtListValues = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, listName, strCamlQuery, strViewField);
                if(dtListValues != null && dtListValues.Rows.Count > 0)
                {
                    strValue = Convert.ToString(dtListValues.Rows[0][columnName]);
                }
            }
            finally
            {
                if(dtListValues != null)
                    dtListValues.Dispose();
            }
            return strValue;
        }

        protected ArrayList GetTectonicSettingsBasinNames(string listName, string filterValue)
        {
            ArrayList arrTechoSettingsBasinNames = new ArrayList();
            string strCamlQuery = string.Empty;
            DataTable dtListValues;
            if(!string.IsNullOrEmpty(filterValue))
            {
                strCamlQuery = "<Where><And><Contains><FieldRef Name='Title' /> <Value Type='Text'>" + filterValue + "</Value></Contains><Eq><FieldRef Name='Active' /> <Value Type='Choice'>yes</Value></Eq></And></Where><OrderBy><FieldRef Name='Title' Ascending='False' />   </OrderBy>";
            }

            MOSSServiceManager objMossController = (MOSSServiceManager)objFactory.GetServiceManager(MOSSSERVICE);

            dtListValues = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, listName, strCamlQuery);
            if(dtListValues != null)
            {
                foreach(DataRow row in dtListValues.Rows)
                {
                    if(row["BasinName"] != null && !string.IsNullOrEmpty(row["BasinName"].ToString()))
                    {
                        arrTechoSettingsBasinNames.Add(SetValue(row["BasinName"].ToString()));
                    }

                }
            }
            return arrTechoSettingsBasinNames;

        }

        /// <summary>
        /// Removes the type attribute.
        /// </summary>
        /// <param name="requestXml">The request XML.</param>
        private void RemoveTypeAttribute(XmlDocument requestXml)
        {
            if(requestXml != null)
            {
                if(requestXml.SelectSingleNode(ENTITYXPATH).Attributes.GetNamedItem("type") != null)
                {
                    requestXml.SelectSingleNode(ENTITYXPATH).Attributes.RemoveNamedItem("type");
                }
            }
        }

        #endregion

    }
}

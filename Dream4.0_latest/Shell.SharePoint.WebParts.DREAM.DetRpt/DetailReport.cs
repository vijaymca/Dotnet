#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : DetailReport.cs
#endregion
using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.Services.Protocols;
using System.Xml;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DREAM.SearchHelper;

namespace Shell.SharePoint.WebParts.DREAM.DetailReport
{
    /// <summary>
    /// This web part is used as the container for storing detail report for 
    /// all search types in the site.//
    /// </summary>
    [Guid("97a1aa6c-e9fa-4931-8ac9-4bb161f8163a")]
    public class DetailReport : SearchHelper
    {
        #region Declaration
        #region CONTROLS
        Label lblMessage = new Label();
        HyperLink linkPrint = new HyperLink();
        HyperLink linkExcel = new HyperLink();
        HyperLink linkiRequest = new HyperLink();
        HyperLink linkExportAll = new HyperLink();        
        RadioButton rbFeet = new RadioButton();
        RadioButton rbMeters = new RadioButton();
        RadioButtonList rdoViewMode = new RadioButtonList();
        DropDownList cboFilter = new DropDownList();
        #endregion
        #region VARIABLES
        string strSearchName = string.Empty;
        string strDisplayType = string.Empty;
        string strSearchType = string.Empty;
        string strPicksFilter = string.Empty;
        string strPageNumber = string.Empty;
        string strSortColumn = string.Empty;
        string strSortOrder = string.Empty;
        string[] arrIdentifierValue = new string[1];
        string strIdentifiedItem = string.Empty;
        string strIdentifierName = string.Empty;
        bool blnDisplayFeetMeter = false;
        bool blnDisplayFilterDropDown = false;
        bool blnViewDatasheet = false;
        bool blnClicked = false;
        bool blnTabular = false;
        bool blnDisplayiRequest = false;
        int intMaxRecords = -1;
        int intSortOrder = 0;
        int intRecordcount = 0;
        #endregion

        #region CONSTANTS
        const string PICKSSELECTEDVAL = "SelectAll";
        const string SELECTALL = "--SelectAll--";
        const string NULL = "--BLANK--";
        const string FILTERDROPDOWN = "$cboFilter";
        const string VIEWMODERADIOGROUP = "$rdoViewMode";
        const string SEARCHTYPE = "SearchType";
        const string WELLBORE = "WELLBOREHEADER";        
        const string PICKS = "PICKSDETAIL";
        const string RECALLLOG = "RECALLLOGDETAIL";        
        const string PARS = "PARSDETAIL";
        const string TABULARREPORT = "Tabular";
        const string DATASHEETREPORT = "Datasheet";
        const string TABULAR = "TABULAR";
        const string DATASHEET = "DATA SHEET";
        const string INITIALDATASHEET = "DATASHEET";
        const string USERPREFERENCES = "UserPreferences";
        const string FEET = "Feet";
        const string GEOLOGICFEATURE = "Geologic Feature";
        const string REQUESTID = "hidRequestID";
        const string PAGENUMBERQS = "pagenumber";
        const string REPORTSERVICE = "ReportService";
        const string SORTBY = "sortby";
        const string SORTTYPE = "sorttype";
        const string RECORDCOUNTQS = "RecordCount";
        const string REQUESTIDQS = "RequestId";
        const string DESCENDING = "descending";
        const string TIMEDEPTHREPORT = "TimeDepthDetail Tabular";
        const string DIRECTIONALREPORT = "DirectionalSurveyDetail Tabular";
        const string TABULARREPORTXSL = "Tabular Results";
        const string GEOLOGICFEATUREXPATH = "/response/report/record/attribute[@name = 'Geologic Feature']";
        const string VALUE = "value";
        #endregion
        #region HIDDENFIELD
        HiddenField hidDisplayType = new HiddenField();
        HiddenField hidReportSelectRow = new HiddenField();
        HiddenField hidSelectedCriteriaName = new HiddenField();
        HiddenField hidDetailSelectedColumns = new HiddenField();
        HiddenField hidDetailSearchType = new HiddenField();
        HiddenField hidRowsCount = new HiddenField();
        HiddenField hidRequestID = new HiddenField();
        #endregion        
        #endregion
        #region Overridden Methods
        /// <summary>
        /// this pre-render event is used to store the Report Type, Selected Item, Criteria, Filter details
        /// in the global variables before the render event is called.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            try
            {
                if (HttpContext.Current.Request.Form[REQUESTID] != null)
                    hidRequestID.Value = HttpContext.Current.Request.Form[REQUESTID].ToString();
                //Gets the filter dropdown selected Value.
                if (HttpContext.Current.Request.Form[this.UniqueID + FILTERDROPDOWN] != null)
                {
                    strPicksFilter = HttpContext.Current.Request.Form[this.UniqueID + FILTERDROPDOWN].ToString();
                }
                //Gets the ViewMode Selected value.
                if (HttpContext.Current.Request.Form[this.UniqueID + VIEWMODERADIOGROUP] != null)
                {

                    strDisplayType = HttpContext.Current.Request.Form[this.UniqueID + VIEWMODERADIOGROUP].ToString();
                    blnTabular = true;
                }
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }
        /// <summary>
        /// Called by the ASP.NET page framework to initialize the page object.
        /// </summary>
        protected override void OnInit(EventArgs e)
        {
            //If page is already opened then the Busybox.htm will be shown.
            if (HttpContext.Current.Request.Form[this.UniqueID + VIEWMODERADIOGROUP] != null || HttpContext.Current.Request.Form[this.UniqueID + FILTERDROPDOWN] != null)
            {
                this.Page.Response.Write("<script language=\"javascript\" src=\""+SPContext.Current.Site.Url
                    +"/_Layouts/DREAM/Javascript/CastleBusyBox.js\"></script>");
                this.Page.Response.Write("<iframe id=\"BusyBoxIFrame\" style=\"border:3px double #D2D2D2\" name=\"BusyBoxIFrame\" frameBorder=\"0\" scrolling=\"no\" ondrop=\"return false;\" src=\"/_layouts/dream/Busybox.htm\"></iframe>");
                this.Page.Response.Write("<SCRIPT>");
                this.Page.Response.Write("var busyBox = new BusyBox(\"BusyBoxIFrame\", \"busyBox\", 1, \"processing\", \".gif\", 125, 147, 207,\"" + SPContext.Current.Site.Url + "/_layouts/dream/Busybox.htm\");");
                this.Page.Response.Write("</SCRIPT>");
                this.Page.Response.Write("<script>busyBox.Show();</script>");
                this.Page.Response.Flush();
            }
            else if (Page.Request.QueryString[PAGENUMBERQS] != null)
            {

                this.Page.Response.Write("<script language=\"javascript\" src=\"" + SPContext.Current.Site.Url
                   + "/_Layouts/DREAM/Javascript/CastleBusyBox.js\"></script>");
                this.Page.Response.Write("<iframe id=\"BusyBoxIFrame\" style=\"border:3px double #D2D2D2\" name=\"BusyBoxIFrame\" frameBorder=\"0\" scrolling=\"no\" ondrop=\"return false;\" src=\"/_layouts/dream/Busybox.htm\"></iframe>");
                this.Page.Response.Write("<SCRIPT>");
                this.Page.Response.Write("var busyBox = new BusyBox(\"BusyBoxIFrame\", \"busyBox\", 1, \"processing\", \".gif\", 125, 147, 207,\"" + SPContext.Current.Site.Url + "/_layouts/dream/Busybox.htm\");");
                this.Page.Response.Write("</SCRIPT>");
                this.Page.Response.Write("<script>busyBox.Show();</script>");
                this.Page.Response.Flush();
            }
            //If page is opening for the first time then the InitialBusybox.htm will be shown.
            else
            {
                this.Page.Response.Write("<script language=\"javascript\" src=\"" + SPContext.Current.Site.Url
                    + "/_Layouts/DREAM/Javascript/CastleBusyBox.js\"></script>");
                this.Page.Response.Write("<iframe id=\"BusyBoxIFrame1\" style=\"border:3px double #D2D2D2\" name=\"BusyBoxIFrame1\" frameBorder=\"0\" scrolling=\"no\" ondrop=\"return false;\" src=\"/_layouts/dream/Busybox.htm\"></iframe>");
                this.Page.Response.Write("<SCRIPT>");
                this.Page.Response.Write("var busyBox1 = new BusyBox(\"BusyBoxIFrame1\", \"busyBox1\", 1, \"processing\", \".gif\", 125, 147, 207,\"" + SPContext.Current.Site.Url + "/_layouts/dream/Busybox.htm\");");
                this.Page.Response.Write("</SCRIPT>");
                this.Page.Response.Write("<script>busyBox1.Show();</script>");
                this.Page.Response.Flush();
            }
            base.OnInit(e);
        }
        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {   
            objCommonUtility = new CommonUtility();
            base.CreateChildControls();
            try
            {                                
                SetReportProperties();
                CreateMessageLabel();
                CreateHyperLinks();
                CreateHiddenControl();
                CreateRadioControls();

                this.Controls.Add(lblMessage);
                this.Controls.Add(linkExcel);
                this.Controls.Add(linkPrint);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
            finally
            {
                this.Page.Response.Write("<script>try{busyBox1.Hide();}catch(Ex){}</script>");
                this.Page.Response.Write("<script>try{busyBox.Hide();}catch(Ex){}</script>");
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
                ServiceProvider objFactory = new ServiceProvider();
                objReportController = objFactory.GetServiceManager(REPORTSERVICE);
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                                
                RenderPage(writer);
                //Intializes the user preferences.
                InitializeUserPreference();
            }
            catch (SoapException soapEx)
            {
                if (!string.Equals(soapEx.Message.ToString(), NORECORDSFOUND))
                {
                    CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                }
                RenderExceptionMessage(writer, soapEx.Message.ToString());
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), webEx, 1);
                RenderExceptionMessage(writer, webEx.Message.ToString());
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
            finally
            {
                this.Page.Response.Write("<script>try{busyBox1.Hide();}catch(Ex){}</script>");
                this.Page.Response.Write("<script>try{busyBox.Hide();}catch(Ex){}</script>");
            }
        }

        #endregion
        #region Private Methods
        /// <summary>
        /// Renders the exception message.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="message">The message.</param>
        private void RenderExceptionMessage(HtmlTextWriter writer, string message)
        {
            RenderParentTable(writer, strSearchName);
            writer.Write("<tr><td style=\"white-space:normal;\"><br/>");
            lblMessage.Visible = true;
            lblMessage.Text = message;
            lblMessage.RenderControl(writer);
            writer.Write("</td></tr></table>");
            //sets the window title based on search name.
            writer.Write("<Script language=\"javascript\">setWindowTitle('" + strSearchName + "');</Script>");
        }
        /// <summary>
        /// Sets the report properties.
        /// </summary>
        private void SetReportProperties()
        {
            if (Page.Request.QueryString[SEARCHTYPE] != null)
                strSearchType = Page.Request.QueryString[SEARCHTYPE].ToString();
            #region ReportDisplayProperties
            //below switch case set the flag values based on search type.
            switch (strSearchType.ToUpper())
            {
                case WELLBORE:
                    strSearchName = "Well/Wellbore Header Report";
                    blnDisplayFeetMeter = true;
                    blnViewDatasheet = true;
                    break;
                case TIMEDEPTH:
                    strSearchName = "Time Depth Array Report";
                    blnDisplayFeetMeter = true;
                    break;
                case DIRECTIONAL:
                    strSearchName = "Directional Survey Array Report";
                    blnDisplayFeetMeter = true;
                    break;
                case PICKS:
                    strSearchName = "Picks Detail Report";
                    blnDisplayFilterDropDown = true;
                    blnDisplayFeetMeter = true;
                    break;
                case RECALLLOG:
                    strSearchName = "Recall Curve Report";
                    blnDisplayFeetMeter = true;
                    break;
                case PARS:
                    strSearchName = "Project Archives Detail Report";
                    blnViewDatasheet = true;
                    blnDisplayiRequest = true;
                    break;
                default:
                    break;
            }
            #endregion
        }
        /// <summary>
        /// Renders the page.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderPage(HtmlTextWriter writer)
        {
            objRequestInfo = new RequestInfo();            
            try
            {                
                writer.Write("<link rel=\"stylesheet\" type=\"text/css\" href=\"/_layouts/DREAM/styles/DetailReport.css\" />");                
                #region GETRESULTXMLDOCUMENT
                if (Page.Request.QueryString.Count > 0 && strPicksFilter.Length == 0 && strDisplayType.Length == 0)
                {
                    strSearchType = Page.Request.QueryString[SEARCHTYPE];                    
                    #region RequestXML Creation
                    if (Page.Request.QueryString[PAGENUMBERQS] != null)
                    {
                        //Gets the detail report result for paging and sorting.
                        GetDetailReportResultPaging();
                    }
                    else
                    {
                        if (!string.Equals(strSearchType.ToUpper(), RECALLLOG))
                        {
                            //Gets the detail report result for other reports except Recall Logs.
                            GetDetailReportResult();                            
                        }
                        else
                        {
                            //Gets the detail report result for Recall Logs.
                            GetRecallLogsReport();                            
                        }
                    #endregion
                    }
                }
                else
                {
                    if (Page.Request.QueryString.Count > 0)
                    {
                        strSearchType = Page.Request.QueryString[SEARCHTYPE];
                        base.RequestID = hidRequestID.Value.ToString().Trim();
                        if (blnViewDatasheet)
                        {                            
                            if (strDisplayType.Length == 0)
                                SetUserPreference();
                            if (!string.Equals(strDisplayType.ToUpper(), DATASHEET) && !string.Equals(strDisplayType.ToUpper(), INITIALDATASHEET))
                            {
                                base.ResponseType = TABULARREPORT;
                            }
                            else
                            {
                                base.ResponseType = DATASHEETREPORT;
                            }
                            //Creating the Request dataobject.
                            objRequestInfo = SetBasicDataObjects(strSearchType, string.Empty, null, true,false,intMaxRecords);                                                       
                        }
                            //For filter implementation
                        else if (strPicksFilter.Length > 0)
                        {                            
                            base.ResponseType = TABULARREPORT;
                            arrIdentifierValue[0] = strPicksFilter;
                            //Creating the Request dataobject.
                            objRequestInfo = SetBasicDataObjects(strSearchType, GEOLOGICFEATURE, arrIdentifierValue, true,true,intMaxRecords);                            
                        }
                        //Calling the Controller method to get the Search results.
                        xmlDocSearchResult = objReportController.GetSearchResults(objRequestInfo, intMaxRecords, strSearchType, null, intSortOrder);
                    }                    
                }
                #region UserPreference
                SetUserPreference();
                #endregion
                #endregion
                if (xmlDocSearchResult != null)
                {
                    //Gets the request ID from the result for caching.
                    GetRequestIDFromResult();
                    hidDetailSearchType.Value = strSearchType;
                    #region RenderingXMLResults
                    //Checks Whether Record count exceeds the maxRecord.
                    if (objCommonUtility.IsMaxRecordExceeds(xmlDocSearchResult, blnClicked))
                    {                        
                        blnClicked = true;                        
                    }

                    RenderParentTable(writer, strSearchName);
                    RenderPrintExport(writer);
                    RenderHiddenControls(writer);
                    if (Page.Request.QueryString[PAGENUMBERQS] != null && strPicksFilter.Length == 0 && strDisplayType.Length == 0)
                    {
                        strPageNumber = Page.Request.QueryString[PAGENUMBERQS].ToString();
                        strSortColumn = Page.Request.QueryString[SORTBY].ToString();
                        strSortOrder = Page.Request.QueryString[SORTTYPE].ToString();
                        if (!blnTabular)
                            strDisplayType = TABULARREPORT;
                    }
                    RenderDetailReport(writer);
                    #endregion
                }
                else
                {
                    RenderExceptionMessage(writer, ((MOSSServiceManager)objMossController).GetCustomExceptionMessage(strCurrSiteUrl, "1"));
                }
            }
            catch (SoapException)
            {
                throw;
            }
            catch (WebException) { throw; }
            catch (Exception)
            { throw; }
        }

        /// <summary>
        /// Gets the request ID from result.
        /// </summary>
        private void GetRequestIDFromResult()
        {
            try
            {
                XmlNodeList objXmlNodeList = null;
                objXmlNodeList = xmlDocSearchResult.SelectNodes("response");
                if (objXmlNodeList != null)
                {
                    foreach (XmlNode xmlNode in objXmlNodeList)
                    {
                        hidRequestID.Value = xmlNode.Attributes["requestid"].Value.ToString();
                    }
                }
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>
        /// Gets the detail report result paging.
        /// </summary>
        private void GetDetailReportResultPaging()
        {
            try
            { 
                //base.SearchType = strSearchType;
                //Sets the required attributes for fetching the result from report service.
                strPageNumber = Page.Request.QueryString[PAGENUMBERQS].ToString();
                if (Page.Request.QueryString[SORTBY].ToString().Length > 0)
                    strSortColumn = Page.Request.QueryString[SORTBY].ToString();
                else
                    strSortColumn = null;                
                strSortOrder = Page.Request.QueryString[SORTTYPE].ToString();
                intRecordcount = Convert.ToInt32(Page.Request.QueryString[RECORDCOUNTQS].ToString());
                if (strSortOrder == DESCENDING)
                {
                    intSortOrder = 1;
                }
                strCurrPageNumber = Page.Request.QueryString[PAGENUMBERQS].ToString();
                intOldRecordCount = Convert.ToInt32(Page.Request.QueryString[RECORDCOUNTQS].ToString());
                intRecordcount = Convert.ToInt32(Page.Request.QueryString[RECORDCOUNTQS].ToString());
                //Sets the RequestID from the Query string.
                base.RequestID = Page.Request.QueryString[REQUESTIDQS].ToString();
                if (!string.Equals(strDisplayType.ToUpper(), DATASHEET) && !string.Equals(strDisplayType.ToUpper(), INITIALDATASHEET))
                    base.ResponseType = TABULARREPORT;
                else
                    base.ResponseType = DATASHEETREPORT;
                //Creating the Request dataobject.
                objRequestInfo = SetBasicDataObjects(strSearchType, string.Empty, null, false, false,intMaxRecords);
                //Calling the Controller method to get the Search results.
                xmlDocSearchResult = objReportController.GetSearchResults(objRequestInfo, intMaxRecords, strSearchType, strSortColumn, intSortOrder); 
            }
            catch (SoapException)
            {
                throw;
            }
            catch (WebException) { throw; }
            catch (Exception)
            { throw; }
        }

        /// <summary>
        /// Gets the detail report result.
        /// </summary>
        private void GetDetailReportResult()
        {
            try
            {
                if (strDisplayType.Length == 0)
                    SetUserPreference();
                //Gets the Selected Identifier value from the results page.
                if (HttpContext.Current.Request.Form["hidSelectedRows"] != null)
                {
                    strIdentifiedItem = HttpContext.Current.Request.Form["hidSelectedRows"].ToString();
                }
                //Gets the Selected Identifier Column name from the results page.
                if (HttpContext.Current.Request.Form["hidSelectedCriteriaName"] != null)
                {
                    strIdentifierName = HttpContext.Current.Request.Form["hidSelectedCriteriaName"].ToString();
                }
                hidSelectedCriteriaName.Value = strIdentifierName;
                hidReportSelectRow.Value = strIdentifiedItem;
                arrIdentifierValue = strIdentifiedItem.Split('|');
                if (blnViewDatasheet)
                {
                    if (!string.Equals(strDisplayType.ToUpper(), DATASHEET) && !string.Equals(strDisplayType.ToUpper(), INITIALDATASHEET))
                    {
                        base.ResponseType = TABULARREPORT;
                    }
                    else
                    {
                        base.ResponseType = DATASHEETREPORT;
                    }
                }
                else
                {
                    base.ResponseType = TABULARREPORT;
                }
                //Creating the Request dataobject.
                objRequestInfo = SetBasicDataObjects(strSearchType, hidSelectedCriteriaName.Value.Trim(), arrIdentifierValue, false, false,intMaxRecords);
                //Calling the Controller method to get the Search results.
                xmlDocSearchResult = objReportController.GetSearchResults(objRequestInfo, intMaxRecords, strSearchType, null, intSortOrder);
            }
            catch (SoapException)
            { throw; }
            catch (WebException)
            { throw; }
            catch (Exception)
            { throw; }
        }

        /// <summary>
        /// Renders the recall logs report.
        /// </summary>
        private void GetRecallLogsReport()
        {
            try
            {
                base.ResponseType = TABULARREPORT;
                //Setting the searchHelper class properties to create the Recall Log criteria values.
                base.UWBI = HttpContext.Current.Request.Form["hidUWBI"].ToString().Split('|');
                base.LogService = HttpContext.Current.Request.Form["hidLogService"].ToString().Split('|');
                base.LogType = HttpContext.Current.Request.Form["hidLogType"].ToString().Split('|');
                base.LogSource = HttpContext.Current.Request.Form["hidLogSource"].ToString().Split('|');
                base.LogName = HttpContext.Current.Request.Form["hidLogName"].ToString().Split('|');
                base.LogActivity = HttpContext.Current.Request.Form["hidLogActivity"].ToString().Split('|');
                base.Logrun = HttpContext.Current.Request.Form["hidLogrun"].ToString().Split('|');
                base.LogVersion = HttpContext.Current.Request.Form["hidLogVersion"].ToString().Split('|');
                base.Recall_Project_Name = HttpContext.Current.Request.Form["hidProjectName"].ToString().Split('|');
                //Creating the Request dataobject.
                objRequestInfo = SetBasicDataObjects(strSearchType, string.Empty, null, false, false,intMaxRecords);
                //Calling the Controller method to get the Search results.
                xmlDocSearchResult = objReportController.GetSearchResults(objRequestInfo, intMaxRecords, strSearchType, null, intSortOrder);
            }
            catch (SoapException)
            { throw; }
            catch (WebException)
            { throw; }
            catch (Exception)
            { throw; }
        }
       
        /// <summary>
        /// Renders the detail report.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="blnDisplayFeetMeter">if set to <c>true</c> [BLN display feet meter].</param>
        /// <param name="depthUnit">The depth unit.</param>
        /// <param name="searchType">Type of the search.</param>
        private void RenderDetailReport(HtmlTextWriter writer)
        {
            try
            {
                string strDepthUnit = string.Empty;
                XmlTextReader xmlTextReader = null;

                //Gets the depth unit from userpreferences.
                strDepthUnit = objUserPreferences.DepthUnits;
                RenderFeetMeter(writer, strDepthUnit, blnViewDatasheet, blnDisplayFeetMeter);

                if (blnDisplayFilterDropDown)
                {
                    if (Page.Request.QueryString[PAGENUMBERQS] != null || strPicksFilter.Length > 0)
                    {
                        //Rendering the Picks dropdown.
                        writer.Write("<tr><td colspan=\"4\">");
                        //LoadPicksDropDown(cboFilter, xmlDocSearchResult);
                        LoadPicksDropDownFromSession(cboFilter);
                        cboFilter.RenderControl(writer);
                        writer.Write("</td></tr>");
                    }
                    else
                    {
                        //Rendering the Picks dropdown.
                        writer.Write("<tr><td colspan=\"4\">");
                        LoadPicksDropDown(cboFilter, xmlDocSearchResult);
                        cboFilter.RenderControl(writer);
                        writer.Write("</td></tr>");
                    }
                }
                if (string.Equals(strSearchType.ToString().ToUpper(), TIMEDEPTH) || string.Equals(strSearchType.ToString().ToUpper(), DIRECTIONAL))
                {
                    if (string.Equals(strSearchType.ToString().ToUpper(), TIMEDEPTH))
                        xmlTextReader = ((MOSSServiceManager)objMossController).GetXSLTemplate(TIMEDEPTHREPORT, strCurrSiteUrl);
                    else
                        xmlTextReader = ((MOSSServiceManager)objMossController).GetXSLTemplate(DIRECTIONALREPORT, strCurrSiteUrl);
                }
                else
                {
                    if (blnViewDatasheet)
                    {
                        //Gets the XSL template from the sharepoint list.
                        if (string.Equals(strDisplayType.ToUpper(), TABULAR))
                            xmlTextReader = ((MOSSServiceManager)objMossController).GetXSLTemplate(TABULARREPORTXSL, strCurrSiteUrl);
                        else
                            xmlTextReader = ((MOSSServiceManager)objMossController).GetXSLTemplate(strSearchType + " Datasheet", strCurrSiteUrl);
                    }
                    else
                        xmlTextReader = ((MOSSServiceManager)objMossController).GetXSLTemplate(TABULARREPORTXSL, strCurrSiteUrl);
                }
                writer.Write("<tr><td colspan=\"4\">");
                object objSessionUserPreference = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString());
                if (objSessionUserPreference != null)
                {
                    objPreferences = (UserPreferences)objSessionUserPreference;                    
                }
                TransformSearchResultsToXSL(xmlDocSearchResult, xmlTextReader, strPageNumber, strSortColumn, strSortOrder, strSearchType, intMaxRecords, strSearchName, intRecordcount,string.Empty);                
                if (intDetailRecordCount != 0)
                {
                    hidRowsCount.Value = intDetailRecordCount.ToString();
                    hidRowsCount.RenderControl(writer);
                }
                writer.Write("</td></tr></table>");                
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Loads the picks drop down from session.
        /// </summary>
        /// <param name="cboFilter">The cbo filter.</param>
        private void LoadPicksDropDownFromSession(DropDownList filter)
        {   
            filter.Items.Clear();
            //Gets the filter values from session.
            object objFilterValues = CommonUtility.GetSessionVariable(Page, enumSessionVariable.picksFilterValues.ToString());
            if (objFilterValues != null)
            {
                //Assign the values to the Picks filter dropdown.
                foreach (string strPicksValue in (string[])objFilterValues)
                {
                    filter.Items.Add(strPicksValue);
                }
            }
            if (strPicksFilter.Length > 0)
            {
                filter.SelectedValue = strPicksFilter;
            }
        }
        /// <summary>
        /// Renders the feet meter.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderFeetMeter(HtmlTextWriter writer, string depthUnit, bool displayViewMode, bool depthUnitDisplay)
        {
            //checks whether to display Report View mode.
            if (displayViewMode)
            {
                if (depthUnitDisplay)
                {
                    writer.Write("<tr><td colspan=\"1\" align=\"left\" class=\"tdAdvSrchItem\"><b>Depth Units: </b>");
                    //Checks whether the depth unit is set to Feet ot Metre.
                    if (string.Equals(depthUnit , FEET))
                    {
                        rbFeet.Checked = true;
                        rbMeters.Checked = false;
                    }
                    else
                    {
                        rbMeters.Checked = true;
                        rbFeet.Checked = false;
                    }
                    rbFeet.RenderControl(writer);
                    rbMeters.RenderControl(writer);
                    writer.Write("</td><td colspan=\"1\" align=\"right\" class=\"tdAdvSrchItem\"><b>View: </b>");
                    AssignValuesToViewMode(writer);
                }
                else
                {
                    writer.Write("<tr><td colspan=\"2\" align=\"right\" class=\"tdAdvSrchItem\"><b>View: </b>");
                    AssignValuesToViewMode(writer);
                }
            }
            else
            {
                writer.Write("<tr><td colspan=\"2\" align=\"left\" class=\"tdAdvSrchItem\"><b>Depth Units: </b>");
                if (string.Equals(depthUnit,FEET))
                {
                    rbFeet.Checked = true;
                }
                else
                {
                    rbMeters.Checked = true;
                }
                rbFeet.RenderControl(writer);
                rbMeters.RenderControl(writer);
            }
            writer.Write("</td></tr>");
        }
        /// <summary>
        /// Renders the hidden controls.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderHiddenControls(HtmlTextWriter writer)
        {
            //renders all the hidden fields.
            hidSelectedCriteriaName.RenderControl(writer);
            hidDisplayType.RenderControl(writer);
            hidDetailSearchType.RenderControl(writer);
            hidDetailSelectedColumns.RenderControl(writer);
            hidRequestID.RenderControl(writer);
        }
        /// <summary>
        /// Renders the print and export.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="recordExceeds">if set to <c>true</c> [record exceeds].</param>
        private void RenderPrintExport(HtmlTextWriter writer)
        {
            linkPrint.Visible = true;            
            //check for record count exceeds Maxrecord count based on that renders the controls.            
            if (blnViewDatasheet)
            {
                if (!string.Equals(strDisplayType.ToUpper(), DATASHEET) && !string.Equals(strDisplayType.ToUpper(), INITIALDATASHEET))
                {
                    RenderTabularPrintExport(writer);
                }
                else
                {
                    writer.Write("<tr><td class=\"tdAdvSrchItem\" colspan=\"4\" align=\"right\" width=\"100%\">Print");
                    writer.Write("&nbsp;");
                    linkPrint.RenderControl(writer);
                    writer.Write("&nbsp;");
                    if (blnDisplayiRequest)
                    {
                        linkiRequest.Visible = true;
                        writer.Write("&nbsp;");
                        writer.Write("Archive Restore");
                        writer.Write("&nbsp;");
                        linkiRequest.RenderControl(writer);
                    }
                    writer.Write("</td></tr>");
                }
            }
            else
            {
                RenderTabularPrintExport(writer);
            }            
        }

        /// <summary>
        /// Renders the tabular print export.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="recordExceeds">if set to <c>true</c> [record exceeds].</param>
        private void RenderTabularPrintExport(HtmlTextWriter writer)
        {
            writer.Write("<tr><td class=\"tdAdvSrchItem\" colspan=\"4\" align=\"right\" width=\"100%\">Print");
            writer.Write("&nbsp;");
            linkPrint.RenderControl(writer);
            writer.Write("&nbsp;");
            writer.Write("Export Page");
            writer.Write("&nbsp;");
            linkExcel.RenderControl(writer);
            writer.Write("&nbsp;");
            writer.Write("Export All");
            writer.Write("&nbsp;");
            linkExportAll.RenderControl(writer);
            if (blnDisplayiRequest)
            {
                linkiRequest.Visible = true;
                writer.Write("&nbsp;");
                writer.Write("Archive Restore");
                writer.Write("&nbsp;");
                linkiRequest.RenderControl(writer);
            }
            writer.Write("</td></tr>");
        }
        /// <summary>
        /// Renders the parent table.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderParentTable(HtmlTextWriter writer, string searchName)
        {
            //Renders the Parent table.
            writer.Write("<SCRIPT type=\"text/javascript\" SRC=\"/_layouts/dream/Javascript/sortTable.js\"></SCRIPT>");
            writer.Write("<table class=\"tableAdvSrchBorder\" cellspacing=\"0\" cellpadding=\"4\" border=\"0\" width=\"100%\">");
            writer.Write("<tr><td class=\"tdAdvSrchSubHeader\" colspan=\"4\" style=\"background-image: url(/_layouts/DREAM/images/hdrbg_p3.gif)\" valign=\"top\"><B>" + searchName + "</b></td></tr>");
        }
        /// <summary>
        /// Sets the user preference.
        /// </summary>
        private void SetUserPreference()
        {
            object objBeforeSessionUserPreference = null;
            object objSessionUserPreference = null;
            //validates the user preferences session value.
            objBeforeSessionUserPreference = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString());
            if (objBeforeSessionUserPreference == null)
            {
                InitializeUserPreference();
                objSessionUserPreference = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString());
                objUserPreferences = (UserPreferences)objSessionUserPreference;
            }
            else
            {
                objUserPreferences = (UserPreferences)objBeforeSessionUserPreference;
            }
            if (objUserPreferences != null)
            {
                if (strDisplayType.Length==0)
                    strDisplayType = objUserPreferences.Display;
            }
        }
        /// <summary>
        /// Initializes the user preference.
        /// </summary>
        protected void InitializeUserPreference()
        {
            try
            {                
                string strUserId = objCommonUtility.GetUserName();
                //reads the user preferences.
                objUserPreferences = ((MOSSServiceManager)objMossController).GetUserPreferencesValue(strUserId, strCurrSiteUrl);
                CommonUtility.SetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString(), objUserPreferences);                
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
        }
        /// <summary>
        /// Used to set the values for Tabular / DataSheet fieldsets based on the User Preferences set.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void AssignValuesToViewMode(HtmlTextWriter writer)
        {
            //set the viewmode value based on user preferences.
            if (string.Equals(strDisplayType.ToUpper() , TABULAR))
            {
                rdoViewMode.SelectedIndex = 1;
            }
            else
            {
                rdoViewMode.SelectedIndex = 0;
            }
            rdoViewMode.RenderControl(writer);
        }
        #endregion
        #region REQUIREDMETHODS
        /// <summary>
        /// Creates labels to display message.
        /// </summary>
        private void CreateMessageLabel()
        {
            lblMessage.ID = "lblMessage";
            lblMessage.CssClass = "labelMessage";
            //sets the default visibility of messege label to false.
            lblMessage.Visible = false;
        }
        /// <summary>
        /// Creates the hyper links.
        /// </summary>
        private void CreateHyperLinks()
        {
            //creates the print,excel and continue search links. 
            linkPrint.ID = "linkPrint";
            linkPrint.CssClass = "resultHyperLink";
            linkPrint.NavigateUrl = "javascript:printContent('tblSearchResults');";
            linkPrint.ImageUrl = "/_layouts/DREAM/images/icon_print.gif";
            this.Controls.Add(linkPrint);
            
            linkExcel.ID = "linkExcel";
            linkExcel.CssClass = "resultHyperLink";
            linkExcel.NavigateUrl = "javascript:ExportToExcel('tblSearchResults');";
            linkExcel.ImageUrl = "/_layouts/DREAM/images/icon_Excel.gif";
            this.Controls.Add(linkExcel);

            linkExportAll.ID = "linkExportAll";
            linkExportAll.CssClass = "resultHyperLink";
            linkExportAll.NavigateUrl = "javascript:ExportToExcelAll('/Pages/ExportToExcel.aspx?ReportType=Detail','DetailReport');";
            linkExportAll.ImageUrl = "/_layouts/DREAM/images/icon_Excel.gif";
            this.Controls.Add(linkExportAll);

            //Creates the iRequest Link for Project Archives.
            linkiRequest.ID = "linkiRequest";
            linkiRequest.CssClass = "resultHyperLink";
            linkiRequest.NavigateUrl = "javascript:OpeniRequest();";
            linkiRequest.ImageUrl = "/_layouts/DREAM/images/iRequest.gif";
            linkiRequest.Visible = false;

        }
        /// <summary>
        /// Creates the hidden controls.
        /// </summary>
        private void CreateHiddenControl()
        {
            hidSelectedCriteriaName.ID = "hidSelectedCriteriaName";
            hidRowsCount.ID = "hidRowsCount";
            hidRequestID.ID = REQUESTID;
            hidDetailSearchType.ID = "hidDetailSearchType";
            hidDetailSelectedColumns.ID = "hidDetailSelectedColumns";
        }
        /// <summary>
        /// Renders the radio controls.
        /// </summary>
        private void CreateRadioControls()
        {
            //Creating the Feet radio button.
            rbFeet.ID = "rbFeet";
            rbFeet.GroupName = "FeetMeters";
            rbFeet.Text = "Feet";            
            rbFeet.Attributes.Add("OnClick", "javascript:ConvertFeetMetresDetail(this.value,'" + strSearchType + "');");
            this.Controls.Add(rbFeet);

            //Creating the Metres radio button.
            rbMeters.ID = "rbMeters";
            rbMeters.GroupName = "FeetMeters";
            rbMeters.Text = "Metres";            
            rbMeters.Attributes.Add("OnClick", "javascript:ConvertFeetMetresDetail(this.value,'" + strSearchType + "');");
            this.Controls.Add(rbMeters);

            //Creating the View mode radio button group.
            rdoViewMode.AutoPostBack = true;
            rdoViewMode.ID = "rdoViewMode";
            //rdoViewMode.CssClass = "radioStyle";
            rdoViewMode.RepeatDirection = RepeatDirection.Horizontal;
            rdoViewMode.RepeatLayout = RepeatLayout.Flow;
            rdoViewMode.Items.Add("Data Sheet");
            rdoViewMode.Items.Add(TABULARREPORT);
            rdoViewMode.SelectedIndexChanged += new EventHandler(rdoViewMode_SelectedIndexChanged);
            this.Controls.Add(rdoViewMode);

            //Creating the Picks Filter dropdown box.
            cboFilter.ID = "cboFilter";
            cboFilter.EnableViewState = true;
            cboFilter.AutoPostBack = true;
            cboFilter.SelectedIndexChanged += new EventHandler(cboFilter_SelectedIndexChanged);
            this.Controls.Add(cboFilter);
        }
        /// <summary>
        /// Handles the SelectedIndexChanged event of the cboFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cboFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (HttpContext.Current.Request.Form[this.UniqueID + FILTERDROPDOWN] != null)
                {
                    strPicksFilter = HttpContext.Current.Request.Form[this.UniqueID + FILTERDROPDOWN].ToString();
                }
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }
        /// <summary>
        /// Loads the picks drop down.
        /// </summary>
        /// <param name="filter">The filter.</param>
        private void LoadPicksDropDown(DropDownList filter, XmlDocument searchResult)
        {
            string strValue = string.Empty;
            XmlNodeList xmlValues = searchResult.SelectNodes(GEOLOGICFEATUREXPATH);

            filter.Items.Clear();
            filter.Items.Add(SELECTALL);
            filter.Items[0].Value = PICKSSELECTEDVAL;
            //Loop through the Geological Features and adds them to filter field.
            foreach (XmlNode xmlnodeValue in xmlValues)
            {
                strValue = xmlnodeValue.Attributes.GetNamedItem(VALUE).Value.ToString();
                if (strValue.Length == 0)
                    strValue = NULL;
                int intCounter = 0;
                foreach (ListItem ltItem in filter.Items)
                {
                    if (ltItem.Text.ToString() == strValue)
                    {
                        intCounter++;
                    }
                }
                if (intCounter == 0)
                {
                    filter.Items.Add(strValue);
                }
            }
            if (strPicksFilter.Length > 0)
            {
                filter.SelectedValue = strPicksFilter;
            }
            int intStringArrayLength = filter.Items.Count;
            string[] arrPicksFilter = new string[intStringArrayLength];
            int intListCount = 0;
            foreach (ListItem lstItem in filter.Items)
            {
                arrPicksFilter[intListCount] = lstItem.Text.ToString();
                intListCount++;
            }
            CommonUtility.SetSessionVariable(Page, enumSessionVariable.picksFilterValues.ToString(), arrPicksFilter);
        }
        /// <summary>
        /// Handles the SelectedIndexChanged event of the rdoViewMode control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void rdoViewMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Gets the ViewMode Selected value.
            if (HttpContext.Current.Request.Form[this.UniqueID + VIEWMODERADIOGROUP] != null)
            {
                strDisplayType = HttpContext.Current.Request.Form[this.UniqueID + VIEWMODERADIOGROUP].ToString();
            }
        }
        #endregion
    }
}
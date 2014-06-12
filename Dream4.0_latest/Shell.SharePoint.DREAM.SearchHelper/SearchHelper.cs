#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: SearchHelper.cs
#endregion
using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Xml;
using System.Data;
using System.Xml.Xsl;
using System.Xml.XPath;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.DREAM.SearchHelper
{
    /// <summary>
    /// This is the Search Helper Base Class for all search results.
    /// </summary>
    public class SearchHelper :System.Web.UI.WebControls.WebParts.WebPart
    {
        #region DECLARATION
        #region CONSTANTS

        const string SELECTALL = "--SelectAll--";
        protected const string SORTDEFAULTORDER = "ascending";
        protected const string XPATH = "/response/report";
        protected const string ENTITYXPATH = "requestinfo/entity";
        protected const string EPXPATH = "ResultSet";
        protected const string RECORDSPERPAGE = "recordsPerPage";
        protected const string PAGENUMBER = "pageNumber";
        protected const string RECORDCOUNT = "recordCount";
        protected const string CURRENTPAGENAME = "CurrentPageName";
        protected const string CURRENTPAGE = "CurrentPage";
        protected const string MAXPAGES = "MaxPages";
        protected const int MAXPAGEVALUE = 5;
        private const string WINDOWTITLE = "windowTitle";
        private const string REQUESTID = "requestId";//Added for cashing
        protected const string SORTCOLUMN = "sortBy";
        protected const string SORTORDER = "sortType";
        protected const string ACTIVEDIV = "activeDiv";
        protected const string ASSETNAME = "asset";
        protected const string CRITERIANAME = "criteria";
        protected const string MAXRECORDS = "MaxRecords";
        protected const string ADMINISTRATOR = "Administrator";
        //DREAM 3.0 code
        //start
        protected const string COLUMNNAMES = "arrColNames";
        protected const string COLUMNDISPLAYSTATUS = "arrColDisplayStatus";
        protected const string PRESSUREUNIT = "pressureUnit";
        protected const string TEMPERATUREUNIT = "temperatureUnit";
        protected const string REORDERDIVHTML = "reorderDivHTML";
        //end
        protected int intDetailRecordCount;
        const string FILTERVALUE = "filterValue";
        const string FEETMETERARG = "userPreference";
        const string FORMULAVALUETITLE = "formulaValue";
        const string COLUMNSTOLOCK = "ColumnsToLock";
        const string PICKS = "Picks";
        const string RESULTRECORDLOCKXPATH = "/response/report/record[@recordno=1]/attribute";
        protected const string SEARCHTYPEPARAMETER = "searchType";
        protected const string ASSETTYPEPARAMETER = "AssetType";
        const string USERPREFERENCELABEL = "userPreference";
        protected const string UWBINAME = "UWBI";
        protected const string PREFERREDFLAG = "Preferred Flag";
        protected const string PICKSINTERPRETER = "Pick Interpreter";
        const string LOGSERVICENAME = "Log Service";
        const string LOGTYPENAME = "Log Type";
        const string LOGSOURCENAME = "Log Source";
        const string LOGATTRIBUTENAME = "Log Name";
        const string LOGACTIVITYNAME = "Log Activity";
        const string LOGRUNNAME = "Log Run";
        const string LOGVERSIONNAME = "Log Version";
        const string RECALLPROJECTNAME = "Recall Project Name";
        const string IQMCONFIGURATION = "IQM Configurations";
        const double PIVALUE = 3.28084;
        protected const string EQUALSOPERATOR = "EQUALS";
        protected const string LIKEOPERATOR = "LIKE";
        protected const string INOPERATOR = "IN";
        protected const string ANDOPERATOR = "AND";
        protected const string STAROPERATOR = "*";
        protected const string AMPERSAND = "%";
        protected const string LESSTHANEQUALS = "LTEQ";
        protected const string GREATERTHANEQUALS = "GTEQ";
        protected const string TIMEDEPTHDETAIL = "timedepthdetail";
        protected const string DIRECTIONALSURVEYDETAIL = "directionalsurveydetail";
        protected const string NORECORDSFOUND = "No records were found that matched your search parameters.Please modify your parameters and run the search again.";
        protected const string CONNECTIVITYERROR = "Connectivity to this database is currently not available";
        protected const string DATEFORMAT = "Date Format";
        protected const string SEARCHNAMEFORUBIWRKSHEET = "wellboreheader|picks|picksdetail|directionalsurvey|directionalsurveydetail|timedepth|timedepthdetail|geopressure|recalllogs|recallcurve|zoneproperties";
        protected const string REORDERDOCLIB = "Reorder XML";
        protected const string MOSSSERVICE = "MossService";
        protected const string EVENTTARGET = "__EVENTTARGET";
        protected const string EVENTARGUMENT = "__EVENTARGUMENT";
        protected const string PRESSURESURVEYDATA = "pressuresurveydata";
        protected const string UPDATEPANELID = "updatepanelcontentpage";
        //Dream 4.0 code start
        #region Dream 4.0 variable & constants Declarartion
        protected const string PAGENUMBERQS = "pagenumber";
        protected const string SORTBY = "sortby";
        protected const string SORTTYPE = "sorttype";
        protected const string SKIPRECORDS = "skiprecords";
        protected const string MAXPAGERECORDS = "maxpagerecords";
        protected const string DESCENDING = "descending";
        protected string strPageNumber = "1";
        protected const string REPORTSERVICE = "ReportService";
        protected const string RESOURCEMANAGER = "ResourceManager";
        protected const string QUERYSERVICE = "QueryService";
        protected const string EVENTSERVICE = "EVENTSERVICE";
        protected string strSortColumn = string.Empty;
        protected string strSortOrder = SORTDEFAULTORDER;
        protected Hashtable hstblParams;
        protected bool blnSkipCountEnabled = true;
        protected int intSortOrder;
        protected int intMaxRecords;
        protected const string TABULAR = "Tabular";
        protected string strActiveDiv = "tab-0";
        protected const string TABULARRESULTSXSL = "Tabular Results";
        protected const string PALEOMARKERSREPORT = "paleomarkers";
        protected const string RECALLLOGS = "RecallLogs";
        protected const string RECALLCURVE = "recallcurve";
        protected const string EDMREPORT = "edmreport";
        protected XmlNodeList xmlNdLstReorderColumn;
        protected Reorder objReorder;
        protected HiddenField hidSelectedCriteriaName;
        protected HiddenField hidSelectedRows;
        //Added in DREAM 4.0
        protected HiddenField hidSelectedAssetNames;
        protected HiddenField hidRowSelectedCheckBoxes;
        protected HiddenField hidColSelectedCheckBoxes;
        protected string[] arrIdentifierValue = new string[1];
        protected const string TIMEDEPTH = "timedepth";
        protected const string DIRECTIONALSURVEY = "directionalsurvey";
        protected const string EXPORTALLREQUESTXML = "EXPORTALLREQUESTXML";
        protected const string RECALLLOGSURL = "RecallLogsURL";
        protected const string PARAMROWSELECTEDCHECKBOXES = "arrRowSelectedCheckboxes";
        protected const string PARAMCOLSELECTEDCHECKBOXES = "arrColSelectedCheckboxes";
        protected const string ATTRIBUTEXPATHWITHDISPLAYTRUE = "response/report/record[1]/attribute[@display = 'true']";
        protected const string ASSETCOLNAME = "assetColName";
        protected string strChkbxColNames = string.Empty;
        protected string strChkbxColDisplayStatus = string.Empty;
        protected const string TRUEVERTICAL = "true vertical";
        protected const string RESPONSETYPEXPATH = "requestinfo/entity/@responsetype";
        #endregion
        //Dream 4.0 code Ends
        #endregion

        #region OBJECTS
        protected CommonUtility objCommonUtility = new CommonUtility();
        protected UserPreferences objPreferences;
        protected RequestInfo objRequestInfo;
        protected SkipInfo objSkipInfo;
        #endregion

        #region VARIABLES
        protected string strCurrSiteUrl = string.Empty;
        string strSearchType = string.Empty;
        protected string strRequestId = string.Empty;
        protected string strCurrPageNumber = string.Empty;
        /// DREAM 3.0 code
        /// start
        protected string strColNames = string.Empty;
        protected string strColDisplayStatus = string.Empty;
        protected string strPressureUnit = string.Empty;
        protected string strTemperatureUnit = string.Empty;
        protected string strReorderDivHTML = string.Empty;
        //end
        protected int intOldRecordCount;
        protected string strDepthUnit = string.Empty;

        string strCriteriaName = string.Empty;
        string[] arrUWBI = new string[1];
        string[] arrLogService = new string[1];
        string[] arrLogType = new string[1];
        string[] arrLogSource = new string[1];
        string[] arrLogName = new string[1];
        string[] arrLogActivity = new string[1];
        string[] arrLogRun = new string[1];
        string[] arrLogVersion = new string[1];
        string[] arrRecallProjectName = new string[1];
        string[] arrCriteriaValue = new string[1];
        string strRangeName = string.Empty;
        string strRangeValue = string.Empty;
        string strColorName = string.Empty;
        string strColorValue = string.Empty;
        string strResponseType = string.Empty;
        string strEntityName = string.Empty;
        protected int intRecordsPerPage = 100;
        protected HiddenField hidDateFormat;
        /// DREAM 3.0 code
        /// start
        protected HiddenField hidReorderColValue;
        protected HiddenField hidAssetName;
        /// end
        protected HiddenField hidReorderSourceId;
        protected HiddenField hidSearchType;
        protected XmlDocument userColumnReferenceXml;
        protected ServiceProvider objFactory = new ServiceProvider();
        protected UserPreferences objUserPreferences = new UserPreferences();
        protected AbstractController objMossController;
        protected AbstractController objReportController;
        protected XmlDocument xmlDocSearchResult;
        protected StringBuilder strResultTable;
        #endregion

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the type of the response.
        /// </summary>
        /// <value>The type of the response.</value>
        protected string ResponseType
        {
            get
            {
                return strResponseType;
            }
            set
            {
                strResponseType = value;
            }
        }
        /// <summary>
        /// Gets or sets the name of the entity.
        /// </summary>
        /// <value>The name of the entity.</value>
        protected string EntityName
        {
            get
            {
                return strEntityName;
            }
            set
            {
                strEntityName = value;
            }
        }

        /// <summary>
        /// Gets or sets the SEARCHTYPE.
        /// </summary>
        /// <value>The SEARCHTYPE.</value>
        string SearchType
        {
            get
            {
                return strSearchType;
            }
            set
            {
                strSearchType = value;
            }
        }
        /// <summary>
        /// Gets or sets the CRITERIA.
        /// </summary>
        /// <value>The CRITERIA.</value>
        string Criteria
        {
            get
            {
                return strCriteriaName;
            }
            set
            {
                strCriteriaName = value;
            }
        }
        /// <summary>
        /// Gets or sets the CRITERIAVALUE.
        /// </summary>
        /// <value>The CRITERIAVALUE.</value>
        string[] CriteriaValue
        {
            get
            {
                return arrCriteriaValue;
            }
            set
            {
                arrCriteriaValue = value;
            }
        }
        /// <summary>
        /// Gets or sets the UWBI.
        /// </summary>
        /// <value>The UWBI.</value>
        protected string[] UWBI
        {
            get
            {
                return arrUWBI;
            }
            set
            {
                arrUWBI = value;
            }
        }
        /// <summary>
        /// Gets or sets the log service.
        /// </summary>
        /// <value>The log service.</value>
        protected string[] LogService
        {
            get
            {
                return arrLogService;
            }
            set
            {
                arrLogService = value;
            }
        }
        /// <summary>
        /// Gets or sets the type of the log.
        /// </summary>
        /// <value>The type of the log.</value>
        protected string[] LogType
        {
            get
            {
                return arrLogType;
            }
            set
            {
                arrLogType = value;
            }
        }
        /// <summary>
        /// Gets or sets the log source.
        /// </summary>
        /// <value>The log source.</value>
        protected string[] LogSource
        {
            get
            {
                return arrLogSource;
            }
            set
            {
                arrLogSource = value;
            }
        }
        /// <summary>
        /// Gets or sets the name of the log.
        /// </summary>
        /// <value>The name of the log.</value>
        protected string[] LogName
        {
            get
            {
                return arrLogName;
            }
            set
            {
                arrLogName = value;
            }
        }
        /// <summary>
        /// Gets or sets the log activity.
        /// </summary>
        /// <value>The log activity.</value>
        protected string[] LogActivity
        {
            get
            {
                return arrLogActivity;
            }
            set
            {
                arrLogActivity = value;
            }
        }
        /// <summary>
        /// Gets or sets the logrun.
        /// </summary>
        /// <value>The logrun.</value>
        protected string[] Logrun
        {
            get
            {
                return arrLogRun;
            }
            set
            {
                arrLogRun = value;
            }
        }
        /// <summary>
        /// Gets or sets the log version.
        /// </summary>
        /// <value>The log version.</value>
        protected string[] LogVersion
        {
            get
            {
                return arrLogVersion;
            }
            set
            {
                arrLogVersion = value;
            }
        }
        /// <summary>
        /// Gets or sets the name of the recall_ project_.
        /// </summary>
        /// <value>The name of the recall_ project_.</value>
        protected string[] Recall_Project_Name
        {
            get
            {
                return arrRecallProjectName;
            }
            set
            {
                arrRecallProjectName = value;
            }
        }
        /// <summary>
        /// Gets or sets the request ID.
        /// </summary>
        /// <value>The request ID.</value>
        protected string RequestID
        {
            get
            {
                return strRequestId;
            }
            set
            {
                strRequestId = value;
            }
        }
        #endregion

        #region SEARCH RESULT Rendering


        /// <summary>
        /// Transforms the search results to XSL.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="textReader">The text reader.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="sortByColumn">The sort by column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="windowTitle">The window title.</param>
        /// <param name="recordCount">The record count.</param>
        /// <param name="activeDiv">The active div.</param>
        protected void TransformSearchResultsToXSL(XmlDocument document, XmlTextReader textReader,
                                                string pageNumber, string sortByColumn, string sortOrder,
                                                string searchType, int maxRecords, string windowTitle, int recordCount, string activeDiv)
        {
            XslTransform xslTransform = null;
            XmlDocument objXmlDocForXSL = null;
            XsltArgumentList xsltArgsList = null;
            XmlNodeList objXmlNodeList = null;
            XPathDocument objXPathDocument = null;
            MemoryStream objMemoryStream = null;
            DateTimeConvertor objDateTimeConvertor = null;

            ServiceProvider objFactory = new ServiceProvider();
            //calls the appropriate factory class from the controller layer.
            objMossController = objFactory.GetServiceManager(MOSSSERVICE);

            int intPageNumber = 0;

            int intRecordCount = 0;
            double dblCurrentPage = 0D;
            string strSortOrder = SORTDEFAULTORDER;
            string strRequestID = string.Empty;
            object objSessionUserPreference = null;

            try
            {
                if(document != null && textReader != null)
                {
                    /// Inititlize the system and custom objects
                    objCommonUtility = new CommonUtility();
                    xslTransform = new XslTransform();
                    objXmlDocForXSL = new XmlDocument();
                    xsltArgsList = new XsltArgumentList();
                    objPreferences = new UserPreferences();
                    objMemoryStream = new MemoryStream();
                    document.Save(objMemoryStream);
                    objMemoryStream.Position = 0;
                    objXPathDocument = new XPathDocument(objMemoryStream);


                    /// Inititlize the XSL
                    objXmlDocForXSL.Load(textReader);
                    xslTransform.Load(objXmlDocForXSL);
                    /// the below condition validates the pageNumber parameter value.
                    if(pageNumber.Length > 0)
                    {
                        intPageNumber = Convert.ToInt32(pageNumber);
                    }
                    /// the below condition validates the sortOrder parameter value.
                    if(sortOrder.Length > 0)
                    {
                        strSortOrder = sortOrder;
                    }
                    objSessionUserPreference = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString());
                    /// get count of records per page from User's Preference if set
                    if(objSessionUserPreference != null)
                    {
                        /// read the User Preferences from the session.
                        objPreferences = (UserPreferences)objSessionUserPreference;
                        if(string.IsNullOrEmpty(strDepthUnit))
                        {
                            strDepthUnit = objPreferences.DepthUnits;
                        }
                        intRecordsPerPage = Convert.ToInt16(objPreferences.RecordsPerPage);
                    }
                    /// Get the Total Record Count
                    objXmlNodeList = document.SelectNodes(XPATH);
                    if(objXmlNodeList != null)
                    {
                        foreach(XmlNode xmlNode in objXmlNodeList)
                        {
                            intRecordCount = Convert.ToInt32(xmlNode.SelectSingleNode("recordcount").InnerXml.ToString());
                            intDetailRecordCount = intRecordCount;
                        }
                    }
                    /// the below condition validates the pageNumber parameter value.
                    if(pageNumber.Length > 0)
                    {
                        dblCurrentPage = Double.Parse(pageNumber);
                        intRecordCount = recordCount; //Added for cashing
                    }
                    /// Added for cashing
                    /// Get the Response ID
                    objXmlNodeList = document.SelectNodes("response");
                    if(objXmlNodeList != null)
                    {
                        foreach(XmlNode xmlNode in objXmlNodeList)
                        {
                            if(xmlNode.Attributes["requestid"] != null)
                            {
                                strRequestID = xmlNode.Attributes["requestid"].Value.ToString();
                            }
                        }
                    }
                    if(sortByColumn == null)
                        sortByColumn = string.Empty;
                    int intColumnsToLock = GetNumberofRecordsToLock(document, RESULTRECORDLOCKXPATH);
                    /// Add the required parameters to the XsltArgumentList object
                    GetDataQualityRange(ref xsltArgsList);

                    objDateTimeConvertor = new DateTimeConvertor();
                    #region DREAM 4.0 - Paging functionality in My Team, My Team Assets and Team Management
                    /// Multi Team Owner Implementation
                    /// Changed By: Yasotha
                    /// Date : 24-Jan-2010
                    /// Modified Lines: 435-438
                    #endregion
                    /// passing parameters to XSL
                    xsltArgsList.AddExtensionObject("urn:DATE", objDateTimeConvertor);
                    xsltArgsList.AddParam(RECORDSPERPAGE, string.Empty, intRecordsPerPage);
                    xsltArgsList.AddParam(PAGENUMBER, string.Empty, intPageNumber);
                    xsltArgsList.AddParam(ACTIVEDIV, string.Empty, activeDiv);
                    xsltArgsList.AddParam(RECORDCOUNT, string.Empty, intRecordCount);
                    xsltArgsList.AddParam(CURRENTPAGENAME, string.Empty, objCommonUtility.GetCurrentPageName(true));
                    xsltArgsList.AddParam(CURRENTPAGE, string.Empty, dblCurrentPage + 1);
                    xsltArgsList.AddParam(MAXPAGES, string.Empty, MAXPAGEVALUE);
                    xsltArgsList.AddParam(SORTCOLUMN, string.Empty, sortByColumn);
                    xsltArgsList.AddParam(SORTORDER, string.Empty, strSortOrder);
                    xsltArgsList.AddParam(COLUMNSTOLOCK, string.Empty, intColumnsToLock);
                    xsltArgsList.AddParam(SEARCHTYPEPARAMETER, string.Empty, searchType);
                    xsltArgsList.AddParam(USERPREFERENCELABEL, string.Empty, strDepthUnit.ToLower());
                    xsltArgsList.AddParam(FORMULAVALUETITLE, string.Empty, PIVALUE);
                    xsltArgsList.AddParam(MAXRECORDS, string.Empty, maxRecords);
                    /// DREAM 3.0 code
                    /// start
                    xsltArgsList.AddParam(COLUMNNAMES, string.Empty, strColNames);
                    xsltArgsList.AddParam(COLUMNDISPLAYSTATUS, string.Empty, strColDisplayStatus);
                    xsltArgsList.AddParam(PRESSUREUNIT, string.Empty, strPressureUnit.ToLowerInvariant());
                    xsltArgsList.AddParam(TEMPERATUREUNIT, string.Empty, strTemperatureUnit);//.ToLowerInvariant());
                    xsltArgsList.AddParam(REORDERDIVHTML, string.Empty, strReorderDivHTML);
                    /// end
                    if(!windowTitle.Equals("My Team Assets"))
                    {
                        xsltArgsList.AddParam(WINDOWTITLE, string.Empty, windowTitle);
                    }
                    else
                    {
                        xsltArgsList.AddParam(WINDOWTITLE, string.Empty, string.Empty);
                    }
                    xsltArgsList.AddParam(REQUESTID, string.Empty, strRequestID);
                    StringBuilder builder = new StringBuilder();
                    StringWriter stringWriter = new StringWriter(builder);
                    xslTransform.Transform(objXPathDocument, xsltArgsList, stringWriter);
                    strResultTable = stringWriter.GetStringBuilder();
                }
                else
                {
                    throw new Exception("Invalid Arguments");
                }
            }
            finally
            {
                if(objMemoryStream != null)
                {
                    objMemoryStream.Close();
                    objMemoryStream.Dispose();
                }
            }
        }

        /// <summary>
        /// Transforms the EP catalog results to XSL.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="textReader">The text reader.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="windowTitle">The window title.</param>
        /// <param name="searchType">Type of the search.</param>
        protected void TransformEPCatalogResultsToXSL(XmlDocument document, XmlTextReader textReader, string pageNumber, string sortColumn, string sortOrder, int maxRecords, string windowTitle, string searchType)
        {
            XslTransform xslTransform = null;
            XmlDocument objXmlDocForXSL = null;
            XsltArgumentList xsltArgsList = null;
            XmlNodeList objXmlNodeList = null;
            XPathDocument objXPathDocument = null;
            DateTimeConvertor objDateTimeConvertor = null;
            int intPageNumber = 0;
            int intRecordCount = 0;
            double dblCurrentPage = 0D;
            string strSortXPATH = string.Empty;
            string strRequestID = string.Empty;
            object objSessionUserPreference = null;
            MemoryStream objMemoryStream = null;

            try
            {
                if(document != null && textReader != null)
                {
                    xslTransform = new XslTransform();
                    objXmlDocForXSL = new XmlDocument();
                    xsltArgsList = new XsltArgumentList();
                    objPreferences = new UserPreferences();

                    objSessionUserPreference = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString());
                    if(objSessionUserPreference != null)
                    {
                        objPreferences = (UserPreferences)objSessionUserPreference;
                        intRecordsPerPage = Convert.ToInt16(objPreferences.RecordsPerPage);
                    }

                    objMemoryStream = new MemoryStream();
                    document.Save(objMemoryStream);
                    objMemoryStream.Position = 0;
                    objXPathDocument = new XPathDocument(objMemoryStream);

                    /// Inititlize the XSL
                    objXmlDocForXSL.Load(textReader);
                    xslTransform.Load(objXmlDocForXSL);

                    /// Get the Total Record Count
                    objXmlNodeList = document.SelectNodes(EPXPATH);
                    if(objXmlNodeList != null)
                    {
                        foreach(XmlNode xmlNode in objXmlNodeList)
                        {
                            intRecordCount = Convert.ToInt32(xmlNode.SelectSingleNode("Count").InnerXml.ToString());
                        }
                    }
                    if(pageNumber.Length > 0)
                    {
                        intPageNumber = Convert.ToInt32(pageNumber);
                        /// the below condition validates the pageNumber parameter value.
                        dblCurrentPage = Double.Parse(pageNumber);
                    }
                    /// Added for cashing
                    /// Get the Response ID
                    strRequestID = (document.SelectSingleNode("ResultSet/requestid") != null ? document.SelectSingleNode("ResultSet/requestid").InnerText : string.Empty);

                    if(sortColumn == null)
                        sortColumn = string.Empty;
                    strSortXPATH = GetSortXPath(sortColumn);

                    /// Add the required parameters to the XsltArgumentList object
                    objDateTimeConvertor = new DateTimeConvertor();
                    xsltArgsList.AddExtensionObject("urn:DATE", objDateTimeConvertor);
                    xsltArgsList.AddParam("recordsPerPage", string.Empty, intRecordsPerPage);
                    xsltArgsList.AddParam(PAGENUMBER, string.Empty, intPageNumber);
                    xsltArgsList.AddParam(RECORDCOUNT, string.Empty, intRecordCount);
                    xsltArgsList.AddParam(CURRENTPAGENAME, string.Empty, HttpContext.Current.Request.Url.AbsolutePath + "?");
                    xsltArgsList.AddParam(CURRENTPAGE, string.Empty, dblCurrentPage + 1);
                    xsltArgsList.AddParam(MAXPAGES, string.Empty, 5);
                    xsltArgsList.AddParam("searchType", string.Empty, searchType);
                    xsltArgsList.AddParam(SORTCOLUMN, string.Empty, sortColumn);
                    xsltArgsList.AddParam(SORTORDER, string.Empty, sortOrder);
                    xsltArgsList.AddParam(WINDOWTITLE, string.Empty, windowTitle);
                    xsltArgsList.AddParam("SortXPath", string.Empty, strSortXPATH);
                    xsltArgsList.AddParam(MAXRECORDS, string.Empty, maxRecords);
                    xsltArgsList.AddParam(REQUESTID, string.Empty, strRequestID);
                    StringBuilder builder = new StringBuilder();
                    StringWriter stringWriter = new StringWriter(builder);
                    xslTransform.Transform(objXPathDocument, xsltArgsList, stringWriter);
                    strResultTable = stringWriter.GetStringBuilder();
                }
                else
                {
                    throw new Exception("Invalid Arguments");
                }
            }
            finally
            {
                if(objMemoryStream != null)
                    objMemoryStream.Dispose();
            }
        }




        /// <summary>
        /// Get the Quality Range values
        /// </summary>
        /// <param name="xsltArgsList">create xslt argument</param>
        private void GetDataQualityRange(ref XsltArgumentList xsltArgsList)
        {
            DataTable objListData = null;
            DataRow objListRow;
            try
            {
                objListData = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, IQMCONFIGURATION, string.Empty);
                if(objListData.Rows.Count > 0)
                {
                    /// Adds the values as Parameters to XSL. 
                    for(int intIndex = 0; intIndex < objListData.Rows.Count; intIndex++)
                    {
                        objListRow = objListData.Rows[intIndex];
                        strRangeName = objListRow["Title"].ToString();
                        strRangeValue = objListRow["Range_x0020_Value"].ToString();
                        strColorName = objListRow["Color_x0020_Name"].ToString();
                        strColorValue = objListRow["Color_x0020_Value"].ToString();
                        xsltArgsList.AddParam(strRangeName, string.Empty, strRangeValue);
                        xsltArgsList.AddParam(strColorName, string.Empty, strColorValue);
                    }
                }
            }
            finally
            {
                if(objListData != null)
                    objListData.Dispose();
            }
        }

        /// <summary>
        /// Gets the sort X path.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns></returns>
        private string GetSortXPath(string sortColumn)
        {
            string strXPathForSort = string.Empty;

            switch(sortColumn.ToUpper())
            {
                case "STATUS":
                    strXPathForSort = "PublishedStatus";
                    break;
                case "PUBLISH DATE":
                    strXPathForSort = "PublishDate";
                    break;
                case "ATTACHMENT":
                    strXPathForSort = "Usage/LogicalFormat";
                    break;
                case "TITLE":
                    strXPathForSort = "Title";
                    break;
                case "ASSET NAME":
                    strXPathForSort = "Asset";
                    break;
                case "AUTHOR":
                    strXPathForSort = "Author/Name";
                    break;
                default:
                    strXPathForSort = string.Empty;
                    break;

            }
            return strXPathForSort;
        }
        /// <summary>
        /// Gets the numberof records to lock.
        /// </summary>
        /// <param name="xmldocument">The xmldocument.</param>
        /// <returns></returns>
        private int GetNumberofRecordsToLock(XmlDocument xmldocument, string lockXPATH)
        {
            int intColumnsToLock = 0;
            if(xmldocument != null)
            {
                XmlNodeList objXmlNodeList = xmldocument.SelectNodes(lockXPATH);
                /// loops through the items in the list to get the total number of columns to lock
                foreach(XmlNode xmlNodeValue in objXmlNodeList)
                {
                    if(xmlNodeValue.Attributes.GetNamedItem("title") != null)
                    {
                        if(string.Equals(xmlNodeValue.Attributes.GetNamedItem("title").Value.ToString(), "true"))
                        {
                            intColumnsToLock++;
                        }
                    }
                }
            }
            return intColumnsToLock;
        }
        #endregion

        #region DataObjectCreation
        /// <summary>
        /// Sets the basic data objects to create XML document
        /// </summary>
        /// <param name="strRequestInfo">The requestinfo search type.</param>
        /// <returns></returns>
        protected RequestInfo SetBasicDataObjects(string searchType, string criteriaName, string[] selectedCriteriaValues, bool isRequestIDExists, bool isFetchAll, int maxRecords)
        {
            objRequestInfo = new RequestInfo();
            /// set the dataobject values.
            SearchType = searchType;
            Criteria = criteriaName;
            if(selectedCriteriaValues != null)
            {
                if((!string.IsNullOrEmpty(selectedCriteriaValues[0])) && (selectedCriteriaValues[0].Equals(SELECTALL)))
                {
                    selectedCriteriaValues[0] = STAROPERATOR;
                    CriteriaValue = selectedCriteriaValues;
                }
                else
                    CriteriaValue = selectedCriteriaValues;
            }

            if(strCurrPageNumber.Length > 0 || isRequestIDExists)
            {
                /// Creating and Adding the Entity object. 
                Entity objEntity = new Entity();
                objEntity.Property = true;
                objEntity.ResponseType = ResponseType;
                objEntity.SkipInfo = SetSkipInfo(isFetchAll, maxRecords);
                objEntity.RequestID = RequestID;
                objEntity.Name = EntityName;
                if(criteriaName.Length > 0)
                {
                    ArrayList arlAttribute = new ArrayList();
                    arlAttribute = SetAtribute();
                    objEntity.Attribute = arlAttribute;
                }
                objRequestInfo.Entity = objEntity;
            }
            else
            {
                objRequestInfo.Entity = SetEntity();
            }
            return objRequestInfo;
        }
        /// <summary>
        /// set the skip record count, request id and maxfetch record
        /// </summary>
        /// <returns></returns>
        private SkipInfo SetSkipInfo(bool isFetchAll, int maxRecords)
        {

            int intMaxRecordPage = 0;
            /// Get the value from Userpreferences
            objSkipInfo = new SkipInfo();
            objPreferences = new UserPreferences();
            objPreferences = (UserPreferences)CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString());
            if(objPreferences != null)
                intMaxRecordPage = Convert.ToInt32(objPreferences.RecordsPerPage);

            if(string.Equals(SearchType.ToString().ToLowerInvariant(), TIMEDEPTHDETAIL) || string.Equals(SearchType.ToString().ToLowerInvariant(), DIRECTIONALSURVEYDETAIL))
            {
                objSkipInfo.MaxFetch = maxRecords.ToString();
                objSkipInfo.SkipRecord = "0";
            }
            else
            {
                if(strCurrPageNumber.Length > 0)
                    objSkipInfo.SkipRecord = Convert.ToString(Convert.ToInt32(strCurrPageNumber) * intMaxRecordPage);
                else
                    objSkipInfo.SkipRecord = "0";
                if(isFetchAll)
                {
                    objSkipInfo.MaxFetch = maxRecords.ToString();
                }
                else
                    objSkipInfo.MaxFetch = objPreferences.RecordsPerPage;
            }
            return objSkipInfo;
        }
        /// <summary>
        /// This function will set the Entity object.
        /// </summary>
        /// <returns></returns>
        private Entity SetEntity()
        {
            Entity objEntity = new Entity();
            objEntity.Property = true;
            objEntity.ResponseType = ResponseType;
            objEntity.Name = EntityName;
            ArrayList arlAttribute = new ArrayList();
            ArrayList arlAttributeGroup = new ArrayList();
            /// the below condition set the attribute group objects based on search type.
            if(SearchType.ToLowerInvariant().Equals(RECALLCURVE))
            {
                arlAttributeGroup = SetLogsAttributeGroup();
                objEntity.AttributeGroups = arlAttributeGroup;
            }
            else
            {
                arlAttribute = SetAtribute();
                objEntity.Attribute = arlAttribute;
            }
            return objEntity;
        }
        /// <summary>
        /// Sets the atribute.
        /// </summary>
        /// <returns></returns>
        private ArrayList SetAtribute()
        {
            ArrayList arlAttribute = new ArrayList();
            ArrayList arlValue = new ArrayList();
            Attributes objDetail = new Attributes();
            objDetail.Name = Criteria;
            /// Loop through all the values in CriteriaValue object.
            foreach(string strAttributeValue in CriteriaValue)
            {
                if(!string.IsNullOrEmpty(strAttributeValue.Trim()))
                {
                    if(!arlValue.Contains(strAttributeValue.Trim()))
                    {
                        arlValue.Add(SetValue(strAttributeValue.Trim()));
                    }
                }
            }
            objDetail.Value = arlValue;
            objDetail.Operator = GetOperator(objDetail.Value);
            arlAttribute.Add(objDetail);
            return arlAttribute;
        }
        /// <summary>
        /// Sets the logs attribute group.
        /// </summary>
        /// <returns></returns>
        private ArrayList SetLogsAttributeGroup()
        {
            ArrayList arlAttributeGroup = new ArrayList();
            ArrayList arlAttribute = new ArrayList();
            AttributeGroup objAttributeGroup = new AttributeGroup();

            /// initializes the attribute objects and set the values
            Attributes objUWBIAttribute = new Attributes();
            objUWBIAttribute.Name = UWBINAME;
            objUWBIAttribute.Value = SetLogAttributeValue(UWBI);
            objUWBIAttribute.Operator = GetOperator(objUWBIAttribute.Value);
            /// adds the attribute object to the attribute Group.
            arlAttribute.Add(objUWBIAttribute);

            Attributes objLogServiceAttribute = new Attributes();
            objLogServiceAttribute.Name = LOGSERVICENAME;
            objLogServiceAttribute.Value = SetLogAttributeValue(LogService);
            objLogServiceAttribute.Operator = GetOperator(objLogServiceAttribute.Value);
            /// adds the attribute object to the attribute Group.
            arlAttribute.Add(objLogServiceAttribute);

            Attributes objLogTypeAttribute = new Attributes();
            objLogTypeAttribute.Name = LOGTYPENAME;
            objLogTypeAttribute.Value = SetLogAttributeValue(LogType);
            objLogTypeAttribute.Operator = GetOperator(objLogTypeAttribute.Value);
            /// adds the attribute object to the attribute Group.
            arlAttribute.Add(objLogTypeAttribute);

            Attributes objLogSourceAttribute = new Attributes();
            objLogSourceAttribute.Name = LOGSOURCENAME;
            objLogSourceAttribute.Value = SetLogAttributeValue(LogSource);
            objLogSourceAttribute.Operator = GetOperator(objLogSourceAttribute.Value);
            /// adds the attribute object to the attribute Group.
            arlAttribute.Add(objLogSourceAttribute);

            Attributes objLogNameAttribute = new Attributes();
            objLogNameAttribute.Name = LOGATTRIBUTENAME;
            /// set the attribute value.
            objLogNameAttribute.Value = SetLogAttributeValue(LogName);
            objLogNameAttribute.Operator = GetOperator(objLogNameAttribute.Value);
            /// adds the attribute object to the attribute Group.
            arlAttribute.Add(objLogNameAttribute);

            Attributes objLogActivityAttribute = new Attributes();
            objLogActivityAttribute.Name = LOGACTIVITYNAME;
            /// set the attribute value.
            objLogActivityAttribute.Value = SetLogAttributeValue(LogActivity);
            objLogActivityAttribute.Operator = GetOperator(objLogActivityAttribute.Value);
            /// adds the attribute object to the attribute Group.
            arlAttribute.Add(objLogActivityAttribute);

            Attributes objLogRunAttribute = new Attributes();
            objLogRunAttribute.Name = LOGRUNNAME;
            objLogRunAttribute.Value = SetLogAttributeValue(Logrun);
            objLogRunAttribute.Operator = GetOperator(objLogRunAttribute.Value);
            arlAttribute.Add(objLogRunAttribute);

            Attributes objLogVersionAttribute = new Attributes();
            objLogVersionAttribute.Name = LOGVERSIONNAME;
            /// set the attribute value.
            objLogVersionAttribute.Value = SetLogAttributeValue(LogVersion);
            objLogVersionAttribute.Operator = GetOperator(objLogVersionAttribute.Value);
            /// adds the attribute object to the attribute Group.
            arlAttribute.Add(objLogVersionAttribute);

            Attributes objLogProjectNameAttribute = new Attributes();
            objLogProjectNameAttribute.Name = RECALLPROJECTNAME;
            /// set the attribute value.
            objLogProjectNameAttribute.Value = SetLogAttributeValue(Recall_Project_Name);
            objLogProjectNameAttribute.Operator = GetOperator(objLogProjectNameAttribute.Value);
            /// adds the attribute object to the attribute Group.
            arlAttribute.Add(objLogProjectNameAttribute);

            objAttributeGroup.Operator = ANDOPERATOR;
            objAttributeGroup.Attribute = arlAttribute;

            arlAttributeGroup.Add(objAttributeGroup);
            return arlAttributeGroup;
        }
        /// <summary>
        /// Sets the log attribute value.
        /// </summary>
        /// <param name="attributeValue">The attribute value.</param>
        /// <returns></returns>
        private ArrayList SetLogAttributeValue(string[] attributeValue)
        {
            int intCheckDuplicate;
            ArrayList arlValue = new ArrayList();
            //Loop through all the value objects in Attribute object.
            foreach(string strAttributeValue in attributeValue)
            {
                intCheckDuplicate = 0;
                //the below condition validates the attribute value.
                if(strAttributeValue.Trim().Length > 0)
                {
                    foreach(Value objValue in arlValue)
                    {
                        if(string.Equals(strAttributeValue.Trim(), objValue.InnerText.ToString()))
                        {
                            intCheckDuplicate++;
                        }
                    }
                    if(intCheckDuplicate == 0)
                    {
                        arlValue.Add(SetValue(strAttributeValue.Trim()));
                    }
                }
            }
            //Dream 4.0
            if(arlValue.Count <= 0)
            {
                arlValue.Add(SetValue(string.Empty));
            }
            return arlValue;
        }
        /// <summary>
        /// Sets the string field as a Value object.
        /// </summary>
        /// <param name="strField">field.</param>
        /// <returns></returns>
        private Value SetValue(string field)
        {
            Value objValue = new Value();
            objValue.InnerText = field;
            //returns the value object.
            return objValue;
        }
        /// <summary>
        /// Gets the query operator for request xml
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected string GetOperator(ArrayList value)
        {
            string strOperator = string.Empty;
            //the below condition validates the value parameter.
            if(value.Count > 1)
            {
                strOperator = INOPERATOR;
            }
            else
            {
                //Loop through the values in ArrayList.
                foreach(Value objValue in value)
                {
                    //the below condition check for the innerText value.
                    if((objValue.InnerText.Contains(STAROPERATOR)) || (objValue.InnerText.Contains(AMPERSAND)))
                        strOperator = LIKEOPERATOR;
                    else
                        strOperator = EQUALSOPERATOR;
                }
            }
            return strOperator;
        }
        /// <summary>
        /// Gets the operator.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected string GetOperator(string value)
        {
            string strOperator;
            //the below condition check for the innerText value.
            if((value.Contains(STAROPERATOR)) || (value.Contains(AMPERSAND)))
            {
                strOperator = LIKEOPERATOR;
            }
            else
            {
                strOperator = EQUALSOPERATOR;
            }
            return strOperator;
        }
        /// <summary>
        /// Gets the logical operator.
        /// </summary>
        /// <returns></returns>
        protected string GetLogicalOperator()
        {
            string strOperator;
            strOperator = ANDOPERATOR;
            //returns the logical operator
            return strOperator;
        }
        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <returns></returns>
        protected string GetUserName()
        {
            string strUserName = string.Empty;
            objCommonUtility = new CommonUtility();
            strUserName = objCommonUtility.GetSaveSearchUserName();
            return strUserName;
        }
        /// <summary>
        /// Determines whether [is name exist] [the specified search names].
        /// </summary>
        /// <param name="searchNames">The search names.</param>
        /// <param name="searchType">Type of the search.</param>
        /// <returns>
        /// 	<c>true</c> if [is name exist] [the specified search names]; otherwise, <c>false</c>.
        /// </returns>
        protected bool IsNameExist(string searchNames, string searchType)
        {
            string[] arrName = searchNames.Split("|".ToCharArray());
            bool blnIsNameExist = false;
            foreach(string name in arrName)
            {
                if(name.Equals(searchType.ToLower()))
                {
                    blnIsNameExist = true;
                    break;
                }
            }
            return blnIsNameExist;
        }
        #endregion

        #region Dream 4.0 code

        #region Ajax Functions
        /// <summary>
        /// Gets the async post back control ID.
        /// </summary>
        /// <returns></returns>
        protected string GetAsyncPostBackControlID()
        {
            string smUniqueId = ScriptManager.GetCurrent(this.Page).UniqueID;
            string smFieldValue = this.Page.Request.Form[smUniqueId];

            if(!String.IsNullOrEmpty(smFieldValue) && smFieldValue.Contains("|"))
            {
                return smFieldValue.Split('|')[1];
            }
            return String.Empty;
        }
        #endregion

        #region Export Option Floating Div Implementation

        /// <summary>
        /// Gets the export options div HTML.
        /// </summary>
        /// <param name="reportType">Type of the report.</param>
        /// <param name="exportPage">if set to <c>true</c> [export page].</param>
        /// <param name="exportAll">if set to <c>true</c> [export all].</param>
        /// <param name="exportExcel">if set to <c>true</c> [export excel].</param>
        /// <param name="exportCSV">if set to <c>true</c> [export CSV].</param>
        /// <returns></returns>
        protected string GetExportOptionsDivHTML(string reportType, bool exportPage, bool exportAll, bool exportExcel, bool exportCSV)
        {
            StringBuilder sbExportOptionsDivHTML = new StringBuilder();
            sbExportOptionsDivHTML.Append("<table cellpadding=\"0\" cellspacing=\"0\">");
            sbExportOptionsDivHTML.Append("<tr>");
            sbExportOptionsDivHTML.Append("<td>");
            sbExportOptionsDivHTML.Append("<div class=\"popupExportOptions\" id=\"divExportOptions\">");
            sbExportOptionsDivHTML.Append("<table>");
            sbExportOptionsDivHTML.Append("<tr>");
            sbExportOptionsDivHTML.Append("<td class=\"ExportDivHeader\" colspan=\"2\">");
            sbExportOptionsDivHTML.Append("<b>Export Options</b></td>");
            sbExportOptionsDivHTML.Append(" </tr>");
            sbExportOptionsDivHTML.Append("<tr>");
            sbExportOptionsDivHTML.Append("<td style=\"padding: 5px 5px 5px 5px\">");
            sbExportOptionsDivHTML.Append("<div id=\"divExportSelection\" title=\"Export Selection\">");
            sbExportOptionsDivHTML.Append("<fieldset>");
            sbExportOptionsDivHTML.Append("<legend class=\"ExportDivLegend\">Export Selection </legend>");
            if(exportPage && exportAll)
            {
                sbExportOptionsDivHTML.Append("<input type=\"radio\" name=\"rdblExportSelection\" id=\"rdblExportSelectionCurrentPage\" checked=\"checked\" value=\"Current Page Only\" title=\"Current Page Only\" /><label>Current Page Only</label>");
                sbExportOptionsDivHTML.Append("<br />");
                sbExportOptionsDivHTML.Append("<input type=\"radio\" name=\"rdblExportSelection\" id=\"rdblExportSelectionAll\" value=\"All\" title=\"All\" />");
                sbExportOptionsDivHTML.Append("<label>All</label>");
                sbExportOptionsDivHTML.Append("</fieldset>");
                sbExportOptionsDivHTML.Append("</div>");
                sbExportOptionsDivHTML.Append("</td>");
                sbExportOptionsDivHTML.Append("<td>");
                sbExportOptionsDivHTML.Append("<div id=\"divExportFormat\" title=\"Export Format\">");
                sbExportOptionsDivHTML.Append("<fieldset>");
                sbExportOptionsDivHTML.Append("<legend class=\"ExportDivLegend\">Export Format </legend>");
                sbExportOptionsDivHTML.Append("<input type=\"radio\" name=\"rdblExportFormat\" id=\"rdblExportFormatExcel\" checked=\"checked\"  value=\"Excel\" title=\"Excel\" /><label>Excel</label>");
                sbExportOptionsDivHTML.Append("<br />");
                sbExportOptionsDivHTML.Append("<input type=\"radio\" name=\"rdblExportFormat\" id=\"rdblExportFormatCSV\" value=\"CSV\" title=\"CSV\" /><label>CSV</label>");
            }
            else
            {
                if(exportPage)
                {
                    /// Enable only Current Page option and set as default selected.
                    /// Enable only Excel option and set as default selected
                    sbExportOptionsDivHTML.Append("<input type=\"radio\" name=\"rdblExportSelection\" id=\"rdblExportSelectionCurrentPage\" checked=\"checked\" value=\"Current Page Only\" title=\"Current Page Only\" /><label>Current Page Only</label>");
                    sbExportOptionsDivHTML.Append("<br />");
                    sbExportOptionsDivHTML.Append("<input type=\"radio\" name=\"rdblExportSelection\" id=\"rdblExportSelectionAll\" value=\"All\" title=\"All\" disabled=\"disabled\"/>");
                    sbExportOptionsDivHTML.Append("<label>All</label>");
                    sbExportOptionsDivHTML.Append("</fieldset>");
                    sbExportOptionsDivHTML.Append("</div>");
                    sbExportOptionsDivHTML.Append("</td>");
                    sbExportOptionsDivHTML.Append("<td>");
                    sbExportOptionsDivHTML.Append("<div id=\"divExportFormat\" title=\"Export Format\">");
                    sbExportOptionsDivHTML.Append("<fieldset>");
                    sbExportOptionsDivHTML.Append("<legend class=\"ExportDivLegend\">Export Selection </legend>");
                    sbExportOptionsDivHTML.Append("<input type=\"radio\" name=\"rdblExportFormat\" id=\"rdblExportFormatExcel\" checked=\"checked\"  value=\"Excel\" title=\"Excel\" /><label>Excel</label>");
                    sbExportOptionsDivHTML.Append("<br />");
                    sbExportOptionsDivHTML.Append("<input type=\"radio\" name=\"rdblExportFormat\" id=\"rdblExportFormatCSV\" value=\"CSV\" title=\"CSV\" disabled=\"disabled\"/><label>CSV</label>");
                }
                if(exportAll)
                {
                    /// Enable only All option and set as default selected.
                    /// Enable only Excel option and set as default selected
                    sbExportOptionsDivHTML.Append("<input type=\"radio\" name=\"rdblExportSelection\" id=\"rdblExportSelectionCurrentPage\"  value=\"Current Page Only\" title=\"Current Page Only\" disabled=\"disabled\"/><label>Current Page Only</label>");
                    sbExportOptionsDivHTML.Append("<br />");
                    sbExportOptionsDivHTML.Append("<input type=\"radio\" name=\"rdblExportSelection\" id=\"rdblExportSelectionAll\" value=\"All\" title=\"All\" checked=\"checked\"/>");
                    sbExportOptionsDivHTML.Append("<label>All</label>");
                    sbExportOptionsDivHTML.Append("</fieldset>");
                    sbExportOptionsDivHTML.Append("</div>");
                    sbExportOptionsDivHTML.Append("</td>");
                    sbExportOptionsDivHTML.Append("<td>");
                    sbExportOptionsDivHTML.Append("<div id=\"divExportFormat\" title=\"Export Format\">");
                    sbExportOptionsDivHTML.Append("<fieldset>");
                    sbExportOptionsDivHTML.Append("<legend class=\"ExportDivLegend\">Export Selection </legend>");
                    sbExportOptionsDivHTML.Append("<input type=\"radio\" name=\"rdblExportFormat\" id=\"rdblExportFormatExcel\" checked=\"checked\"  value=\"Excel\" title=\"Excel\" /><label>Excel</label>");
                    sbExportOptionsDivHTML.Append("<br />");
                    sbExportOptionsDivHTML.Append("<input type=\"radio\" name=\"rdblExportFormat\" id=\"rdblExportFormatCSV\" value=\"CSV\" title=\"CSV\" disabled=\"disabled\"/><label>CSV</label>");
                }
            }
            sbExportOptionsDivHTML.Append("</fieldset>");
            sbExportOptionsDivHTML.Append("</div>");
            sbExportOptionsDivHTML.Append("</td>");
            sbExportOptionsDivHTML.Append("</tr>");
            sbExportOptionsDivHTML.Append("<tr>");

            sbExportOptionsDivHTML.Append("<td colspan=\"2\" align=\"center\">");
            sbExportOptionsDivHTML.Append("<input class=\"buttonAdvSrch\" type=\"button\" value=\"Continue\" id=\" btnExportContinue\" onclick=\"javascript:return ContinueExportOption('" + reportType + "');\" />");

            sbExportOptionsDivHTML.Append("&nbsp;&nbsp;&nbsp;<input class=\"buttonAdvSrch\" type=\"button\" value=\"Cancel\" id=\"btnExportCancel\" onclick=\"javascript:return hide('divExportOptions');\" />");
            sbExportOptionsDivHTML.Append("</td>");

            sbExportOptionsDivHTML.Append("</tr>");
            sbExportOptionsDivHTML.Append("</table>");
            sbExportOptionsDivHTML.Append("</div>");
            sbExportOptionsDivHTML.Append("</td>");
            sbExportOptionsDivHTML.Append(" </tr>");
            sbExportOptionsDivHTML.Append("</table>");

            return sbExportOptionsDivHTML.ToString();
        }

        #endregion

        #region Dream 4.0 New Pagination related methods
        /// <summary>
        /// Transforms the XML to result table.
        /// </summary>
        /// <param name="responseXml">The response XML.</param>
        /// <param name="textReader">The text reader.</param>
        /// <param name="currentPageNumber">The current page number.</param>
        /// <param name="sortByColumn">The sort by column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="activeDiv">The active div.</param>
        protected void TransformXmlToResultTable(XmlDocument responseXml, XmlTextReader textReader,
                                              string currentPageNumber, string sortByColumn, string sortOrder,
                                              string searchType, string activeDiv)
        {

            XslCompiledTransform xslTransform = null;
            XmlDocument objXmlDocForXSL = null;
            XsltArgumentList xsltArgsList = null;
            DateTimeConvertor objDateTimeConvertor = null;
            object objSessionUserPreference = null;
            int intRecordCount = 0;
            Pagination objPagination = null;

            if(responseXml != null && textReader != null)
            {
                xslTransform = new XslCompiledTransform();
                objXmlDocForXSL = new XmlDocument();
                //Inititlize the XSL
                objXmlDocForXSL.Load(textReader);
                xslTransform.Load(objXmlDocForXSL);
                xsltArgsList = new XsltArgumentList();
                objSessionUserPreference = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString());
                /// get count of records per page from User's Preference if set
                if(objSessionUserPreference != null)
                {
                    /// read the User Preferences from the session.
                    objPreferences = (UserPreferences)objSessionUserPreference;
                    if(string.IsNullOrEmpty(strDepthUnit))
                    {
                        strDepthUnit = objPreferences.DepthUnits;
                    }
                    intRecordsPerPage = Convert.ToInt16(objPreferences.RecordsPerPage);
                }
                //setting record count 
                intRecordCount = GetRecordCountFromResult(responseXml, searchType);

                /// the below condition validates the sortOrder parameter value.
                if(string.IsNullOrEmpty(sortOrder))
                {
                    sortOrder = SORTDEFAULTORDER;
                }
                if(string.IsNullOrEmpty(sortByColumn))
                    sortByColumn = string.Empty;

                /// Add the required parameters to the XsltArgumentList object
                /// passing parameters to XSL
                ///creating Pagination related parameter
                objPagination = new Pagination(Convert.ToInt32(currentPageNumber), intRecordCount, intRecordsPerPage, blnSkipCountEnabled);

                CreatePaginationXslParameter(ref xsltArgsList, objPagination);
                GetDataQualityRange(ref xsltArgsList);
                objDateTimeConvertor = new DateTimeConvertor();
                xsltArgsList.AddExtensionObject("urn:DATE", objDateTimeConvertor);
                xsltArgsList.AddParam(ACTIVEDIV, string.Empty, activeDiv);
                xsltArgsList.AddParam(SORTCOLUMN, string.Empty, sortByColumn);
                xsltArgsList.AddParam(SORTORDER, string.Empty, sortOrder);
                xsltArgsList.AddParam(SEARCHTYPEPARAMETER, string.Empty, searchType.ToLowerInvariant());
                xsltArgsList.AddParam(USERPREFERENCELABEL, string.Empty, strDepthUnit.ToLowerInvariant());
                xsltArgsList.AddParam(FORMULAVALUETITLE, string.Empty, PIVALUE);
                xsltArgsList.AddParam(COLUMNNAMES, string.Empty, strColNames);
                xsltArgsList.AddParam(COLUMNDISPLAYSTATUS, string.Empty, strColDisplayStatus);
                xsltArgsList.AddParam(PRESSUREUNIT, string.Empty, strPressureUnit.ToLowerInvariant());
                xsltArgsList.AddParam(TEMPERATUREUNIT, string.Empty, strTemperatureUnit);
                xsltArgsList.AddParam(REORDERDIVHTML, string.Empty, strReorderDivHTML);
                if(hidRowSelectedCheckBoxes != null)
                    xsltArgsList.AddParam(PARAMROWSELECTEDCHECKBOXES, string.Empty, hidRowSelectedCheckBoxes.Value);
                if(hidColSelectedCheckBoxes != null)
                    xsltArgsList.AddParam(PARAMCOLSELECTEDCHECKBOXES, string.Empty, hidColSelectedCheckBoxes.Value);
                xsltArgsList.AddParam(ASSETCOLNAME, string.Empty, GetAssetNameCol(hidAssetName.Value));
                StringBuilder builder = new StringBuilder();
                StringWriter stringWriter = new StringWriter(builder);
                xslTransform.Transform(responseXml, xsltArgsList, stringWriter);
                strResultTable = stringWriter.GetStringBuilder();
            }
            else
            {
                throw new Exception("Invalid Arguments");
            }

        }


        /// <summary>
        /// Transforms the EP catalog XML to result table.
        /// </summary>
        /// <param name="responseXml">The response XML.</param>
        /// <param name="textReader">The text reader.</param>
        /// <param name="currentPageNumber">The current page number.</param>
        /// <param name="sortByColumn">The sort by column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="searchType">Type of the search.</param>
        protected void TransformEPCatalogXmlToResultTable(XmlDocument responseXml, XmlTextReader textReader,
                                              string currentPageNumber, string sortByColumn, string sortOrder,
                                              string searchType)
        {
            XslCompiledTransform xslTransform = null;
            XmlDocument objXmlDocForXSL = null;
            XsltArgumentList xsltArgsList = null;
            DateTimeConvertor objDateTimeConvertor = null;
            object objSessionUserPreference = null;
            int intRecordCount = 0;
            Pagination objPagination = null;
            string strSortXPATH = string.Empty;

            if(responseXml != null && textReader != null)
            {
                xslTransform = new XslCompiledTransform();
                objXmlDocForXSL = new XmlDocument();
                //Inititlize the XSL
                objXmlDocForXSL.Load(textReader);
                xslTransform.Load(objXmlDocForXSL);
                xsltArgsList = new XsltArgumentList();
                objSessionUserPreference = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString());
                /// get count of records per page from User's Preference if set
                if(objSessionUserPreference != null)
                {
                    /// read the User Preferences from the session.
                    objPreferences = (UserPreferences)objSessionUserPreference;
                    if(string.IsNullOrEmpty(strDepthUnit))
                    {
                        strDepthUnit = objPreferences.DepthUnits;
                    }
                    intRecordsPerPage = Convert.ToInt16(objPreferences.RecordsPerPage);
                }
                //setting record count 
                intRecordCount = GetRecordCountFromResult(responseXml, searchType);

                /// the below condition validates the sortOrder parameter value.
                if(string.IsNullOrEmpty(sortOrder))
                {
                    sortOrder = SORTDEFAULTORDER;
                }
                if(string.IsNullOrEmpty(sortByColumn))
                    sortByColumn = string.Empty;
                strSortXPATH = GetSortXPath(sortByColumn);
                /// Add the required parameters to the XsltArgumentList object
                /// passing parameters to XSL
                ///creating Pagination related parameter
                objPagination = new Pagination(Convert.ToInt32(currentPageNumber), intRecordCount, intRecordsPerPage, false);
                CreatePaginationXslParameter(ref xsltArgsList, objPagination);
                objDateTimeConvertor = new DateTimeConvertor();
                xsltArgsList.AddExtensionObject("urn:DATE", objDateTimeConvertor);
                xsltArgsList.AddParam("SortXPath", string.Empty, strSortXPATH);
                xsltArgsList.AddParam(SORTCOLUMN, string.Empty, sortByColumn);
                xsltArgsList.AddParam(SORTORDER, string.Empty, sortOrder);
                xsltArgsList.AddParam(SEARCHTYPEPARAMETER, string.Empty, searchType.ToLowerInvariant());
                StringBuilder builder = new StringBuilder();
                StringWriter stringWriter = new StringWriter(builder);
                xslTransform.Transform(responseXml, xsltArgsList, stringWriter);
                strResultTable = stringWriter.GetStringBuilder();
            }
            else
            {
                throw new Exception("Invalid Arguments");
            }

        }
        /// <summary>
        /// Creates the pagination XSL parameter.
        /// </summary>
        /// <param name="xsltArgsList">The XSLT args list.</param>
        /// <param name="objPagination">The obj pagination.</param>
        private void CreatePaginationXslParameter(ref XsltArgumentList xsltArgsList, Pagination objPagination)
        {
            xsltArgsList.AddParam("recordsPerPage", string.Empty, objPagination.RecordPerPage);
            xsltArgsList.AddParam("maxPageLinkToDisplay", string.Empty, objPagination.NumberOfPageLinkToDisplay);
            xsltArgsList.AddParam("pageCount", string.Empty, objPagination.PageCount);
            xsltArgsList.AddParam("recordCount", string.Empty, objPagination.RecordCount);
            xsltArgsList.AddParam("startPageNumber", string.Empty, objPagination.StartPageNumber);
            xsltArgsList.AddParam("endPageNumber", string.Empty, objPagination.EndPageNumber);
            xsltArgsList.AddParam("currentPageNumber", string.Empty, objPagination.CurrentPageNumber);
            xsltArgsList.AddParam("recordStarIndex", string.Empty, objPagination.RecordStarIndex);
            xsltArgsList.AddParam("recordEndIndex", string.Empty, objPagination.RecordEndIndex);
        }

        /// <summary>
        /// Gets the record count from result.
        /// </summary>
        /// <param name="responseXml">The response XML.</param>
        /// <param name="searchType">Type of the search.</param>
        /// <returns></returns>
        protected int GetRecordCountFromResult(XmlDocument responseXml, string searchType)
        {
            const string PVTREPORT = "pvtreport";
            const string EPCATALOG = "epcatalog";
            int intRecordCount = 0;

            if(responseXml != null)
            {
                if(searchType.ToLowerInvariant().Equals(EPCATALOG) || searchType.ToLowerInvariant().Equals(PVTREPORT))
                {
                    intRecordCount = Convert.ToInt32(responseXml.SelectSingleNode("ResultSet/Count") != null ? responseXml.SelectSingleNode("ResultSet/Count").InnerText : "0");
                }
                else
                {
                    intRecordCount = Convert.ToInt32(responseXml.SelectSingleNode("/response/report/recordcount") != null ? responseXml.SelectSingleNode("/response/report/recordcount").InnerText : "0");
                }
            }
            return intRecordCount;
        }

        /// <summary>
        /// Gets the record count from results.
        /// </summary>
        /// <param name="responseXml">The response XML.</param>
        /// <returns></returns>
        protected int GetRecordCountFromResults(XmlDocument responseXml)
        {
            int intRecordCount = 0;
            if(responseXml != null)
            {
                XmlNode xmlNodeRecordCount = responseXml.SelectSingleNode("response/report/recordcount");
                if(xmlNodeRecordCount != null)
                {
                    intRecordCount = Convert.ToInt32(xmlNodeRecordCount.InnerXml);
                }
            }
            return intRecordCount;
        }

        /// <summary>
        /// Initializes the user preference.
        /// </summary>
        protected void InitializeUserPreference()
        {
            string strUserId = objCommonUtility.GetUserName();
            //get the user prefrences.
            objUserPreferences = ((MOSSServiceManager)objMossController).GetUserPreferencesValue(strUserId, strCurrSiteUrl);
            CommonUtility.SetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString(), objUserPreferences);
        }
        /// <summary>
        /// Sets the user preference.
        /// </summary>
        protected void SetUserPreference()
        {
            object objSessionUserPreference = null;

            objSessionUserPreference = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString());
            //sets the user preferences object from session.
            if(objSessionUserPreference == null)
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
        /// Sets the skip info.
        /// </summary>
        /// <param name="objEntity">The obj entity.</param>
        protected void SetSkipInfo(Entity objEntity)
        {
            //Dream 4.0 
            //start
            SetPagingParameter();
            SkipInfo objSkipInfo = new SkipInfo();
            objSkipInfo.SkipRecord = Convert.ToString(((Convert.ToInt32(strPageNumber) - 1) * intRecordsPerPage));
            objSkipInfo.MaxFetch = Convert.ToString(intRecordsPerPage * Convert.ToInt32(MAXPAGEVALUE));
            objEntity.SkipInfo = objSkipInfo;
        }
        /// <summary>
        /// Sets the skip info.
        /// </summary>
        /// <param name="responseXml">The response XML.</param>
        protected void SetSkipInfo(XmlDocument responseXml)
        {
            //Dream 4.0 
            //start
            SetPagingParameter();
            XmlElement EntityElement = (XmlElement)responseXml.SelectSingleNode(ENTITYXPATH);
            //Creating Skip recourd count
            XmlElement SkipRecords = responseXml.CreateElement(SKIPRECORDS);
            EntityElement.AppendChild(SkipRecords);
            SkipRecords.InnerText = Convert.ToString(((Convert.ToInt32(strPageNumber) - 1) * intRecordsPerPage));
            //Creating Max record fetch
            XmlElement MaxPageRecords = responseXml.CreateElement(MAXPAGERECORDS);
            EntityElement.AppendChild(MaxPageRecords);
            MaxPageRecords.InnerText = Convert.ToString(intRecordsPerPage * Convert.ToInt32(MAXPAGEVALUE));
        }

        /// <summary>
        /// Sets the paging parameter.
        /// </summary>
        protected void SetPagingParameter()
        {
            if(hstblParams != null)
            {
                if(hstblParams[PAGENUMBERQS] != null)
                {
                    strPageNumber = hstblParams[PAGENUMBERQS].ToString();
                }
                if(hstblParams[SORTBY] != null)
                {
                    strSortColumn = hstblParams[SORTBY].ToString();
                }
                if(hstblParams[SORTTYPE] != null)
                {
                    strSortOrder = hstblParams[SORTTYPE].ToString();
                }
                if(hstblParams[ACTIVEDIV] != null)
                {
                    strActiveDiv = hstblParams[ACTIVEDIV].ToString();
                }
            }
            if(strSortOrder.Equals(DESCENDING))
            {
                intSortOrder = 1;
            }
            else
            {
                intSortOrder = 0;
            }
            SetUserPreference();
            intRecordsPerPage = Convert.ToInt32(objUserPreferences.RecordsPerPage);
            intMaxRecords = intRecordsPerPage * Convert.ToInt32(MAXPAGEVALUE);
        }
        #endregion
        /// <summary>
        /// Adds the paleo markers attribute.
        /// </summary>
        protected void AddPaleoMarkersAttribute()
        {
            ArrayList arlAttributeGroup = new ArrayList();
            ArrayList arlAttribute = new ArrayList();
            AttributeGroup objAttributeGroup = new AttributeGroup();
            /// Initializes the attribute objects and set the values
            Attributes objPaleoMarkersAttribute = new Attributes();
            objPaleoMarkersAttribute.Name = PICKSINTERPRETER;
            objPaleoMarkersAttribute.Value = new ArrayList();
            Value pamValue = new Value();
            Value palValue = new Value();
            pamValue.InnerText = "PAM";
            palValue.InnerText = "PAL";
            objPaleoMarkersAttribute.Value.Add(pamValue);
            objPaleoMarkersAttribute.Value.Add(palValue);
            objPaleoMarkersAttribute.Operator = GetOperator(objPaleoMarkersAttribute.Value);
            /// Adds the attribute object to the attribute Group.
            arlAttribute.Add(objRequestInfo.Entity.Attribute[0]);
            objRequestInfo.Entity.Attribute = null;
            arlAttribute.Add(objPaleoMarkersAttribute);
            objAttributeGroup.Operator = ANDOPERATOR;
            objAttributeGroup.Attribute = arlAttribute;
            arlAttributeGroup.Add(objAttributeGroup);
            objRequestInfo.Entity.AttributeGroups = arlAttributeGroup;
        }
        /// <summary>
        /// Gets the recall curve request info.
        /// </summary>
        /// <param name="searchName">Name of the search.</param>
        /// <returns></returns>
        protected RequestInfo GetRecallCurveRequestInfo(string searchName)
        {
            RequestInfo objRecallRequestInfo = null;
            ResponseType = TABULAR;
            /// Setting the searchHelper class properties to create the Recall Log criteria values.
            UWBI = objCommonUtility.GetFormControlValue("hidUWBI").Split('|');
            LogService = objCommonUtility.GetFormControlValue("hidLogService").Split('|');
            LogType = objCommonUtility.GetFormControlValue("hidLogType").Split('|');
            LogSource = objCommonUtility.GetFormControlValue("hidLogSource").Split('|');
            LogName = objCommonUtility.GetFormControlValue("hidLogName").Split('|');
            LogActivity = objCommonUtility.GetFormControlValue("hidLogActivity").Split('|');
            Logrun = objCommonUtility.GetFormControlValue("hidLogrun").Split('|');
            LogVersion = objCommonUtility.GetFormControlValue("hidLogVersion").Split('|');
            Recall_Project_Name = objCommonUtility.GetFormControlValue("hidProjectName").Split('|');
            /// Creating the Request dataobject.
            objRecallRequestInfo = SetBasicDataObjects(searchName, string.Empty, null, false, false, intMaxRecords);
            return objRecallRequestInfo;
        }

        /// <summary>
        /// Saves the request XML for export all.
        /// </summary>
        /// <param name="requestXml">The request XML.</param>
        protected void SaveRequestXmlForExportAll(XmlDocument requestXml)
        {
            //saving request xml to session variable
            CommonUtility.SetSessionVariable(this.Page, EXPORTALLREQUESTXML, requestXml.OuterXml);
        }
        /// <summary>
        /// Saves the request XML for export all.
        /// </summary>
        /// <param name="requestInfo">The request info.</param>
        protected void SaveRequestXmlForExportAll(RequestInfo requestInfo)
        {
            XmlDocument xmlDocRequest = null;
            xmlDocRequest = objReportController.CreateSearchRequest(requestInfo);
            //saving request xml to session variable
            CommonUtility.SetSessionVariable(this.Page, EXPORTALLREQUESTXML, xmlDocRequest.OuterXml);
        }
        /// <summary>
        /// Adds the attribute.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="operation">The operation.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        protected Attributes AddAttribute(string name, string operation, string[] values)
        {
            Attributes objAttribute = new Attributes();

            objAttribute.Value = new ArrayList();

            Value objValue = null;

            objAttribute.Name = name;

            objAttribute.Operator = operation;
            foreach(string strValue in values)
            {
                if(!string.IsNullOrEmpty(strValue.Trim()))
                {
                    objValue = new Value();
                    objValue.InnerText = strValue.Trim();
                    objAttribute.Value.Add(objValue);
                }
            }
            return objAttribute;
        }
        /// <summary>
        /// Sets the column names hidden field.
        /// </summary>
        protected void SetColumnNamesHiddenField()
        {
            if(hidColSelectedCheckBoxes != null && string.IsNullOrEmpty(hidColSelectedCheckBoxes.Value))
            {
                objReorder.SetColNameDisplayStatus(out strChkbxColNames, out strChkbxColDisplayStatus, xmlDocSearchResult.SelectNodes(ATTRIBUTEXPATHWITHDISPLAYTRUE));
                hidColSelectedCheckBoxes.Value = strChkbxColNames;
            }
        }

        /// <summary>
        /// Gets the asset name col.
        /// </summary>
        /// <param name="assetType">Type of the asset.</param>
        /// <returns></returns>
        protected string GetAssetNameCol(string assetType)
        {
            const string ASSETTYPELIST = "Asset Type";
            const string ASSETNAMECOL = "AssetNameColumn";
            string strCamlQuery = "<Where><And><Eq><FieldRef Name=\"IsActiveReportService\" /><Value Type=\"Boolean\">1</Value></Eq><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">" + assetType + "</Value></Eq></And></Where>";
            string strViewFields = @"<FieldRef Name='Title'/><FieldRef Name='" + ASSETNAMECOL + "'/>";
            string strAssetNameCol = string.Empty;
            DataTable dtAssetType = null;
            try
            {
                dtAssetType = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, ASSETTYPELIST, strCamlQuery, strViewFields);
                if(dtAssetType != null && dtAssetType.Rows.Count > 0)
                {
                    strAssetNameCol = (string)dtAssetType.Rows[0][ASSETNAMECOL];
                }
            }
            finally
            {
                if(dtAssetType != null)
                {
                    dtAssetType.Dispose();
                }
            }
            return strAssetNameCol;
        }
        #endregion
    }
}

#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: DWBTypeIReports.cs
#endregion

using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DWB.BusinessLogicLayer;
using Shell.SharePoint.DREAM.SearchHelper;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using Telerik.Web.UI;

namespace Shell.SharePoint.WebParts.DWB.TypeIReports
{
    /// <summary>
    /// Renders the WellHistory/WellBoreHeader Reporte/Pre-Prod RFT Report.
    /// </summary>
    public class DWBTypeIReports : SearchHelper
    {
        #region WebPartPropertiesDeclaration
        BookInfo objBookInfo;
        CommonBLL objCommonBLL;
        string strParentSiteURL = string.Empty;
        string strPageID = string.Empty;
        string strChapterID = string.Empty;
        bool blnSignOff;

        /// <summary>
        /// Gets or Sets the Page ID.
        /// </summary>
        /// <value>The Page ID.</value>
        string PageID
        {
            get { return strPageID; }
            set { strPageID = value; }
        }

        /// <summary>
        /// Gets or sets the Chapter ID.
        /// </summary>
        /// <value>The Chapter ID.</value>
        string ChapterID
        {
            get { return strChapterID; }
            set { strChapterID = value; }
        }

        /// <summary>
        /// Enum for Service Name.
        /// </summary>
        public enum ServiceNameEnum
        {
            /// <summary>
            /// Report Service.
            /// </summary>
            ReportService,
            /// <summary>
            /// Event Service.
            /// </summary>
            EventService
        }
        ServiceNameEnum objServiceName;

        /// <summary>
        /// Enum for XSL file name.
        /// </summary>
        public enum XSLFileEnum
        {
            /// <summary>
            /// Tabular Results
            /// </summary>
            [Description("BookViewer")]
            TabularResults,
            /// <summary>
            /// TimeDepthDetail Tabular
            /// </summary>
            [Description("TimeDepthDetail Tabular")]
            TimeDepthDetail,
            /// <summary>
            /// DirectionalSurveyDetail Tabular
            /// </summary>
            [Description("DirectionalSurveyDetail Tabular")]
            DirectionalSurveyDetail,
            /// <summary>
            /// WellboreHeader Datasheet
            /// </summary>
            [Description("WellboreHeader Datasheet")]
            WellboreHeader_Datasheet,
            /// <summary>
            /// EPCatalog Results
            /// </summary>
            [Description("EPCatalog Results")]
            EPCatalog_Results,
            /// <summary>
            /// PARSDetail Datasheet
            /// </summary>
            [Description("PARSDetail Datasheet")]
            PARSDetail_Datasheet,
            /// <summary>
            /// WellHistory Datasheet
            /// </summary>
            [Description("WellHistory Datasheet")]
            WellHistory,
            /// <summary>
            /// WellSummary Datasheet
            /// </summary>
            [Description("WellSummary")]
            WellSummary
        }
        XSLFileEnum objXSLFileName;
        bool blnShowAll;

        /// <summary>
        /// Enum for View Mode.
        /// </summary>
        public enum ViewModeEnum
        {
            /// <summary>
            /// Tabular.
            /// </summary>
            Tabular,
            /// <summary>
            /// DataSheet
            /// </summary>
            DataSheet,
            /// <summary>
            /// TabularDataSheet
            /// </summary>
            TabularDataSheet
        }
        ViewModeEnum objViewMode;

        /// <summary>
        /// Enum for Report Level.
        /// </summary>
        public enum ReportLevelEnum
        {
            /// <summary>
            /// First
            /// </summary>
            First,
            /// <summary>
            /// Second
            /// </summary>
            Second
        }
        ReportLevelEnum objReportLevel;
        bool blnIsPrintApplicable;
        bool blnIsExportPage;
        bool blnIsExportAll;
        bool blnIsDetailReportAvailable;

        /// <summary>
        /// Enum for Search Name.
        /// </summary>
        public enum SearchNameEnum
        {
            [Description("Well/Wellbore Header Report")]
            WellboreHeader,
            [Description("Picks Search Report")]
            Picks,
            [Description("Picks Detail Report")]
            PicksDetail,
            [Description("Recall Logs Report")]
            RecallLogs,
            [Description("Recall Curve Report")]
            RecallCurve,
            [Description("Directional Survey Report")]
            DirectionalSurvey,
            [Description("Directional Survey Detail Report")]
            DirectionalSurveyDetail,
            [Description("Time Depth Report")]
            TimeDepth,
            [Description("Time Depth Detail Report")]
            TimeDepthDetail,
            [Description("Document Search Results")]
            EPCatalog,
            [Description("EP WellFile Report")]
            iWellFile,
            [Description("Well History Report")]
            WellHistory,
            [Description("Project Archives Detail Report")]
            PARSDetail,
            [Description("Pre production RFT Data")]
            PreProdRFT,
            [Description("Well Summary Report")]
            WellSummary
        }
        SearchNameEnum objSearchName;
        bool blnIsUnitConversion;
        #endregion

        #region WebPart Properties
        #region PrintOption
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DWBTypeIReports"/> is print.
        /// </summary>
        /// <value><c>true</c> if print; otherwise, <c>false</c>.</value>
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDescription("Check if print option is applicable.")]
        [WebDisplayNameAttribute("Print Option")]
        public bool Print
        {
            get
            {
                return blnIsPrintApplicable;
            }
            set
            {
                blnIsPrintApplicable = value;
            }
        }
        #endregion
        #region ExportPage
        /// <summary>
        /// Gets or sets a value indicating whether [export page].
        /// </summary>
        /// <value><c>true</c> if [export page]; otherwise, <c>false</c>.</value>
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDisplayNameAttribute(EXPORT_PAGE_TEXT)]
        [WebDescription("Check if export page option is applicable.")]
        public bool ExportPage
        {
            get
            {
                return blnIsExportPage;
            }
            set
            {
                blnIsExportPage = value;
            }
        }
        #endregion
        #region UnitConversion
        /// <summary>
        /// Gets or sets a value indicating whether [unit conversion].
        /// </summary>
        /// <value><c>true</c> if [unit conversion]; otherwise, <c>false</c>.</value>
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDisplayNameAttribute("Unit Conversion")]
        [WebDescription("Check if unit conversion is applicable.")]
        public bool UnitConversion
        {
            get
            {
                return blnIsUnitConversion;
            }
            set
            {
                blnIsUnitConversion = value;
            }
        }
        #endregion
        #region ExportAll
        /// <summary>
        /// Gets or sets a value indicating whether [export all].
        /// </summary>
        /// <value><c>true</c> if [export all]; otherwise, <c>false</c>.</value>
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDisplayNameAttribute("Export All Records")]
        [WebDescription("check if export all option is applicable.")]
        public bool ExportAll
        {
            get
            {
                return blnIsExportAll;
            }
            set
            {
                blnIsExportAll = value;
            }
        }
        #endregion
        #region ShowAllResults
        /// <summary>
        /// Gets or sets a value indicating whether [show all results].
        /// </summary>
        /// <value><c>true</c> if [show all results]; otherwise, <c>false</c>.</value>
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDisplayNameAttribute("Show All Records")]
        [WebDescription("check/uncheck to show all/limited results.")]
        public bool ShowAllResults
        {
            get
            {
                return blnShowAll;
            }
            set
            {
                blnShowAll = value;
            }
        }
        #endregion
        #region DetailReport
        /// <summary>
        /// Gets or sets a value indicating whether [detail report].
        /// </summary>
        /// <value><c>true</c> if [detail report]; otherwise, <c>false</c>.</value>
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDescription("Check if detail report is available.")]
        [WebDisplayNameAttribute("Detail Report")]
        public bool DetailReport
        {
            get
            {
                return blnIsDetailReportAvailable;
            }
            set
            {
                blnIsDetailReportAvailable = value;
            }
        }
        #endregion

        #region ReportLevel
        /// <summary>
        /// Gets or sets the report level.
        /// </summary>
        /// <value>The report level.</value>
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDisplayNameAttribute("Report Level")]
        [WebDescription("Select the report level.")]
        public ReportLevelEnum ReportLevel
        {
            get
            {
                return objReportLevel;
            }
            set
            {
                objReportLevel = value;
            }
        }
        #endregion
        #region ServiceName
        /// <summary>
        /// Gets or sets the name of the service.
        /// </summary>
        /// <value>The name of the service.</value>
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDisplayNameAttribute("Service Name")]
        [WebDescription("Select the service name.")]
        public ServiceNameEnum ServiceName
        {
            get
            {
                return objServiceName;
            }
            set
            {
                objServiceName = value;
            }
        }
        #endregion
        #region SearchName
        /// <summary>
        /// Gets or sets the name of the search.
        /// </summary>
        /// <value>The name of the search.</value>
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDisplayNameAttribute("Search Name")]
        [WebDescription("Select the search name.")]
        public SearchNameEnum SearchName
        {
            get
            {
                return objSearchName;
            }
            set
            {
                objSearchName = value;
            }
        }
        #endregion
        #region ViewMode
        /// <summary>
        /// Gets or sets the view mode.
        /// </summary>
        /// <value>The view mode.</value>
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDisplayNameAttribute("View Mode")]
        [WebDescription("Select the view mode.")]
        public ViewModeEnum ViewMode
        {
            get
            {
                return objViewMode;
            }
            set
            {
                objViewMode = value;
            }
        }
        #endregion
        #region XSLFileName
        /// <summary>
        /// Gets or sets the name of the XSL file.
        /// </summary>
        /// <value>The name of the XSL file.</value>
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDisplayNameAttribute("XSL File Name")]
        [WebDescription("Select the XSL file name to be used.")]
        public XSLFileEnum XSLFileName
        {
            get
            {
                return objXSLFileName;
            }
            set
            {
                objXSLFileName = value;
            }
        }
        #endregion
        #endregion

        #region DECLARATION
        #region CONSTANTS
        const string REQUESTID_HIDDENFIELD = "hidRequestID";
        const string FILTERDROPDOWN = "$cboFilter";
        const string VIEWMODERADIOGROUP = "$rdoViewMode";
        const string PAGENUMBERQS = "pagenumber";
        const string TABULARREPORT = "Tabular";
        const string DATASHEETREPORT = "Datasheet";
        const string DATASHEET = "DATA SHEET";
        const string INITIALDATASHEET = "DATASHEET";
        const string SORTBY = "sortby";
        const string SORTTYPE = "sorttype";
        const string RECORDCOUNTQS = "RecordCount";
        const string REQUESTIDQS = "RequestId";
        const string DESCENDING = "descending";
        const string MOSSSERVICE = "MossService";
        const string GEOLOGICFEATURE = "Geologic Feature";
        const string DWBPUBLISHEDDOCLIB = "DWB Published Documents";
        const string FEET = "Feet";
        const string METRES = "Metres";
        const string FEET_RDB_GROUP_NAME = "FeetMeters";
        const string GEOLOGICFEATUREXPATH = "/response/report/record/attribute[@name = 'Geologic Feature']";
        const string SELECTALL = "--SelectAll--";
        const string NULL = "--BLANK--";
        const string VALUE = "value";
        const string PICKSSELECTEDVAL = "SelectAll";

        const string ASSETTYPE = "assetType";
        const string ASSETTYPE_FIELD = "Field";
        const string ASSETTYPE_WELL = "Well";
        const string UWBI = "UWBI";
        const string UWI = "UWI";
        //const string FIELD_NAME = "Field Name";
        const string FIELD_IDENTIFIER = "Field Identifier";

        const string VIEW = "view";
        const string BOOKACTION_PUBLISH = "pdf";
        const string BOOKACTION_PRINT = "print";
        const string BOOKACTION_WELLBOOKTOC = "treeview";
        const string PRINTEDDOCUMNETLIBRARY = "DWB Printed Documents";
        const string DWBCHAPTERPAGESMAPPINGLIST = "DWB Chapter Pages Mapping";
        const string DWBCHAPTERPAGESMAPPINGAUDITLIST = "DWB Chapter Pages Mapping Audit Trail";

        const string EVENTARGUMENT = "__EVENTARGUMENT";
        const string EVENTTARGET = "__EVENTTARGET";
        const string EVENTARG_DOWNLOAD = "DownLoad";

        const string MAXRECORDS_KEY = "MaxRecords";
        const string SEARCH_NAME_PICKSDETAIL = "PicksDetail";
        const string SEARCH_NAME_EPCATALOG = "EPCatalog";
        const string SEARCH_NAME_RECALLCURVE = "RecallCurve";
        const string SEARCH_NAME_WELLBOREHEADER = "WellboreHeader";
        const string SEARCH_NAME_PARS_DETAIL = "PARSDetail";
        const string SEARCH_NAME_RECALL_LOGS = "RecallLogs";

        const string VIEWMODE_TABULARDATA_SHEET = "TabularDataSheet";
        const string VIEWMODE_DATA_SHEET = "DataSheet";
        const string DISPLAY_TYPE_TABULAR = "TABULAR";
        const string QUERYSTRING_PAGEID = "pageID";
        const string QUERYSTRING_CHAPTERID = "ChapterID";
        const string QUERYSTRING_MODE = "mode";
        const string SESSION_WEBPARTPROPERTIES = "WEBPARTPROPERTIES";
        const string SESSION_TREEVIEWDATAOBJECT = "TreeViewDataObject";
        const string SESSION_TREEVIEWXML = "TreeViewXML";

        const string CANCEL_SIGNOFF_TEXT = "Cancel Sign Off";
        const string SIGNOFF_TEXT = "Sign Off";
        const string EXPORT_PAGE_TEXT = "Export Page";
        const string EXPORT_ALL_TEXT = "Export All";
        const string ARCHIVE_RESTORE_TEXT = "Archive Restore";
        const string LABEL_CSS_CLASSNAME = "DWBqueryfieldmini";
        const string ONCLICK_EVENT_NAME = "OnClick";

        const string RESPONSE_NODE = "response";
        const string REQUEST_ID = "requestid";

        const string CONNECTION_TYPE3_PAGES = "3 - UserDefinedDocument";
        const string CONNECTION_TYPE2_PAGES = "2 - PublishedDocument";
        const string TYPE1_REPORT_NAME_WELLHISTORY = "1 - Automated (Well History)";
        const string TYPE1_REPORT_NAME_WELLBOREHEADER = "1 - Automated (Wellbore Header)";
        const string TYPE1_REPORT_NAME_PREPROD_RFT = "1 - Automated (PreProd-RFT)";
        const string TYPE1_WELLHISTORY_PAGE_URL = "DWBWellHistoryReport.aspx";
        const string TYPE1_WELLBORE_HEADER_PAGE_URL = "DWBWellboreHeader.aspx";
        const string TYPE1_PREPROD_RFT_PAGE_URL = "DWBPreProductionRFT.aspx";
        const string CONNECTION_TYPE_COLUMN_NAME = "ConnectionType";
        const string PAGE_URL_COLUMN_NAME = "PageURL";
        const string TYPE1WELLSUMMARYPAGEURL = "WellSummary.aspx";
        const string TYPE1_REPORT_NAME_WELLSUMMARY = "1 - Automated (WellSummary)";
        #endregion

        #region Variables
        string strPicksFilter = string.Empty;
        string strDisplayType = string.Empty;
        string strPageNumber = string.Empty;
        string strSortColumn = string.Empty;
        string strSortOrder = string.Empty;
        //string strCurrSiteUrl = HttpContext.Current.Request.Url.ToString();
        string strPageOwner = string.Empty;
        string strDiscipline = string.Empty;
        string strBookOwner = string.Empty;
        string strBookTeamID = string.Empty;
        bool blnClicked;
        string[] strIdentifierValue = new string[1];
        int intMaxRecords;
        int intSortOrder;
        int intRecordcount;
        string strDocumentURL = string.Empty;
        #endregion

        #region CONTROLS
        HyperLink lnkPrint;
        HyperLink lnkExportPage;
        HyperLink lnkExportAll;
        HyperLink lnkiRequest;
        HyperLink linkViewDetailReport;
        LinkButton linkContinueSearch;
        Label lblMessage = new Label();
        HiddenField hidRequestID;
        HiddenField hidReportSelectRow;
        HiddenField hidSelectedCriteriaName;
        HiddenField hidSelectedRows;
        HiddenField hidRowsCount;
        HiddenField hidMaxRecord;
        HiddenField hidSearchType;
        HiddenField hidSelectedColumns;

        HiddenField hidUWBI;
        HiddenField hidLogService;
        HiddenField hidLogType;
        HiddenField hidLogSource;
        HiddenField hidLogName;
        HiddenField hidLogActivity;
        HiddenField hidLogrun;
        HiddenField hidLogVersion;
        HiddenField hidProjectName;
        TextBox hdnIncludeStoryBoard;
        TextBox hdnIncludePageTitle;

        RadioButton rbFeet;
        RadioButton rbMeters;
        RadioButtonList rdoViewMode;
        DropDownList cboFilter;

        /*DWB Control*/
        Label lblOwner;
        Label lblSignedOff;
        Label lblTitleTemplate;
        Button btnSignOff;
        Button btnPrintPage;

        #endregion
        UserPreferences objUserPreferences = new UserPreferences();
        AbstractController objMossController ;
        AbstractController objReportController ;
        XmlDocument xmlDocSearchResult ;
        string strWebServiceUserName = string.Empty;
        string strWebServicePassword = string.Empty;
        string strWebServiceDomain = string.Empty;
        #endregion

        #region WebPartMethods
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"></see> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // this.EnsureChildControls();
            strCurrSiteUrl = GetWebAppRoot();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            if (HttpContext.Current.Request.Form[REQUESTID_HIDDENFIELD] != null)
                hidRequestID.Value = HttpContext.Current.Request.Form[REQUESTID_HIDDENFIELD].ToString();
            if (string.Equals(SearchName.ToString(), SEARCH_NAME_PICKSDETAIL))
            {
                /// Gets the filter dropdown selected Value.
                if (HttpContext.Current.Request.Form[this.UniqueID + FILTERDROPDOWN] != null)
                {
                    strPicksFilter = HttpContext.Current.Request.Form[this.UniqueID + FILTERDROPDOWN].ToString();
                }
            }
            if (string.Equals(ViewMode.ToString(), VIEWMODE_TABULARDATA_SHEET))
            {
                /// Gets the ViewMode Selected value.
                if (HttpContext.Current.Request.Form[this.UniqueID + VIEWMODERADIOGROUP] != null)
                {
                    strDisplayType = HttpContext.Current.Request.Form[this.UniqueID + VIEWMODERADIOGROUP].ToString();
                }
            }

        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            try
            {
                if (HttpContext.Current.Request.QueryString[QUERYSTRING_MODE] == null)
                {
                    Print = false;
                    ExportPage = false;
                    UnitConversion = false;
                    ExportAll = false;
                    ShowAllResults = true;
                    DetailReport = false;
                    ReportLevel = ReportLevelEnum.Second;
                    ViewMode = ViewModeEnum.DataSheet;
                }

                objCommonUtility = new CommonUtility();
                if (!ShowAllResults)
                {
                    intMaxRecords = Convert.ToInt32(PortalConfiguration.GetInstance().GetKey(MAXRECORDS_KEY).ToString());
                }
                else
                {
                    intMaxRecords = -1;
                }
                CreateMetadataControls();
                CreateMessageLabel();
                CreateHyperLinks();
                CreateHiddenControl();
                CreateRadioControls();
                CreatePrintPage();
                CreateSignOff();
            }
            catch (Exception Ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), Ex);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnPrintPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnPrintPage_Click(object sender, EventArgs e)
        {
            try
            {
                int intPrintedDocID = 0;
                string[] strPrintedDocURL = null;
                string strCamlQuery = string.Empty;
                PrintOptions objPrintOptions = new PrintOptions();
                /// If Hidden field containse "true" story board should be included in Print document              
                if (string.Compare(hdnIncludeStoryBoard.Text.ToLowerInvariant(), "true") == 0)
                {
                    objPrintOptions.IncludeStoryBoard = true;
                }
                /// If Hidden field containse "true" Page Title should be included in Print document
                if (string.Compare(hdnIncludePageTitle.Text.ToLowerInvariant(), "true") == 0)
                {
                    objPrintOptions.IncludePageTitle = true;
                }
                intPrintedDocID = PrintPage(objPrintOptions);

                /// Open the generated document to show in IE              
                if (intPrintedDocID > 0)
                {
                    objCommonBLL = new CommonBLL();
                    strCamlQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" +
                    intPrintedDocID.ToString() + "</Value></Eq></Where>";
                    strParentSiteURL = SPContext.Current.Site.Url.ToString();
                    
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "documentprinted", "window.open('" + @strDocumentURL + "', 'PDFViewer', 'scrollbars,resizable,status,toolbar,menubar');", true);
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "documentnotprinted", "alert('We are not able to Print the document. Please contact the administrator');", true);
                }

            }
            catch (SoapException soapEx)
            {
                if (!string.Equals(soapEx.Message.ToString(), NORECORDSFOUND))
                {
                    CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                }
                if (lblMessage != null)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = soapEx.Message;
                }
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), webEx, 1);
                if (lblMessage != null)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = webEx.Message;
                }
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
            finally
            {
                StringBuilder renderedOutput = new StringBuilder();
                System.IO.StringWriter strWriter = new System.IO.StringWriter(renderedOutput);
                HtmlTextWriter tWriter = new HtmlTextWriter(strWriter);
                tWriter.Write("<Script language=\"javascript\">setWindowTitle('" + GetEnumDescription(SearchName) + "');</Script>");
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSignOff control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnSignOff_Click(object sender, EventArgs e)
        {
            objCommonBLL = new CommonBLL();
            strParentSiteURL = SPContext.Current.Site.Url.ToString();
            TreeNodeSelection objTreeNodeSelection = null;
            objTreeNodeSelection = (TreeNodeSelection)HttpContext.Current.Session[SESSION_WEBPARTPROPERTIES];
            PageID = objTreeNodeSelection.PageID;

            if (string.Equals(btnSignOff.Text, CANCEL_SIGNOFF_TEXT))
            {
                objCommonBLL.SignOffPage(strParentSiteURL, DWBCHAPTERPAGESMAPPINGLIST, PageID, DWBCHAPTERPAGESMAPPINGAUDITLIST, false);
                LoadMetaDataControls();
            }
            else
            {
                objCommonBLL.SignOffPage(strParentSiteURL, DWBCHAPTERPAGESMAPPINGLIST, PageID, DWBCHAPTERPAGESMAPPINGAUDITLIST, true);
                LoadMetaDataControls();
            }
        }

        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            ServiceProvider objFactory = null;
            try
            {
                btnPrintPage.OnClientClick = this.RegisterJavaScript();
                if (HttpContext.Current.Session[SESSION_WEBPARTPROPERTIES] != null &&
                    ((TreeNodeSelection)HttpContext.Current.Session[SESSION_WEBPARTPROPERTIES]).IsTypeISelected)
                {
                    TreeNodeSelection objTreeNodeSelection = null;
                    objTreeNodeSelection = (TreeNodeSelection)HttpContext.Current.Session[SESSION_WEBPARTPROPERTIES];

                    if (string.Equals(objTreeNodeSelection.ReportName, TYPE1_REPORT_NAME_WELLHISTORY))
                    {
                        ServiceName = ServiceNameEnum.EventService;
                        SearchName = SearchNameEnum.WellHistory;
                        XSLFileName = XSLFileEnum.WellHistory;
                        ViewMode = ViewModeEnum.DataSheet;
                    }
                    else if (string.Equals(objTreeNodeSelection.ReportName, TYPE1_REPORT_NAME_WELLBOREHEADER))
                    {
                        ServiceName = ServiceNameEnum.ReportService;
                        SearchName = SearchNameEnum.WellboreHeader;
                        XSLFileName = XSLFileEnum.WellboreHeader_Datasheet;
                        ViewMode = ViewModeEnum.DataSheet;
                    }
                    else if (string.Equals(objTreeNodeSelection.ReportName, TYPE1_REPORT_NAME_PREPROD_RFT))
                    {
                        ServiceName = ServiceNameEnum.ReportService;
                        SearchName = SearchNameEnum.PreProdRFT;
                        XSLFileName = XSLFileEnum.TabularResults;
                        ViewMode = ViewModeEnum.Tabular;
                        ExportPage = true;
                    }
                    else if (string.Equals(objTreeNodeSelection.ReportName, TYPE1_REPORT_NAME_WELLSUMMARY))
                    {
                        ServiceName = ServiceNameEnum.ReportService;
                        SearchName = SearchNameEnum.WellSummary;
                        XSLFileName = XSLFileEnum.WellSummary;
                        ViewMode = ViewModeEnum.DataSheet;
                    }

                    PageID = objTreeNodeSelection.PageID;
                    ChapterID = objTreeNodeSelection.ChapterID;

                    LoadMetaDataControls();

                    objFactory = new ServiceProvider();
                    objReportController = objFactory.GetServiceManager(ServiceName.ToString());
                    objMossController = objFactory.GetServiceManager(MOSSSERVICE);

                    RenderPage(writer);
                    InitializeUserPreference();
                }
                else if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString[QUERYSTRING_MODE]))
                {
                    PageID = HttpContext.Current.Request.QueryString[QUERYSTRING_PAGEID];
                    ChapterID = HttpContext.Current.Request.QueryString[QUERYSTRING_CHAPTERID];

                    LoadMetaDataControls();

                    objFactory = new ServiceProvider();
                    objReportController = objFactory.GetServiceManager(ServiceName.ToString());
                    objMossController = objFactory.GetServiceManager(MOSSSERVICE);

                    RenderPage(writer);
                    InitializeUserPreference();
                }
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
                writer.Write("<Script language=\"javascript\">setWindowTitle('Well Book Viewer');</Script>");
            }
        }

        /// <summary>
        /// Gets the enum description.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo objFieldInfo = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])objFieldInfo.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }


        #endregion

        #region PrivateMethods
        /// <summary>
        /// Renders the exception message.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="message">The message.</param>
        private void RenderExceptionMessage(HtmlTextWriter writer, string message)
        {
            RenderParentTable(writer, false);
            writer.Write("<tr><td colspan=\"4\" style=\"white-space:normal;\"><br/>");
            lblMessage.Visible = true;
            lblMessage.Text = message;
            lblMessage.RenderControl(writer);
            writer.Write("</td></tr></table>");
            /// Sets the window title based on search name.
            writer.Write("<Script language=\"javascript\">setWindowTitle('" + GetEnumDescription(SearchName) + "');</Script>");
            if (btnPrintPage != null)
            {
                btnPrintPage.Visible = false;
                btnPrintPage.Style.Add(HtmlTextWriterStyle.Display, "none");
            }
        }

        /// <summary>
        /// Renders the parent table.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderParentTable(HtmlTextWriter writer, bool renderPrintButton)
        {
            /// Renders the Parent table.
            writer.Write("<SCRIPT type=\"text/javascript\" SRC=\"/_layouts/dream/Javascript/sortTableRel2_1.js\"></SCRIPT>");
            writer.Write("<SCRIPT type=\"text/javascript\" SRC=\"/_layouts/dream/Javascript/DWBJavascriptFunctionRel2_0.js\"></SCRIPT>");
            writer.Write("<link rel=\"stylesheet\" type=\"text/css\" href=\"/_layouts/DREAM/styles/DWBStyleSheetRel2_0.css\" />");
            writer.Write("<table class=\"tableAdvSrchBorder\" cellspacing=\"0\" cellpadding=\"4\" border=\"0\" height=\"100%\" width=\"100%\">");

            if (!string.Equals(SearchName.ToString(), SEARCH_NAME_EPCATALOG))
            {
                writer.Write("<tr bgcolor=\"#dcdcdc\"><td valign=\"top\" align=\"left\" style=\"word-wrap: break-word; \">");
                if (lblTitleTemplate != null)
                    lblTitleTemplate.RenderControl(writer);
                if (lblOwner != null)
                {
                    writer.Write("</td><td valign=\"top\" align=\"left\" style=\"word-wrap: break-word;\">Owner&nbsp;");
                    lblOwner.RenderControl(writer);
                }
                if (lblSignedOff != null)
                {
                    writer.Write("</td><td valign=\"top\" align=\"left\">Signed Off&nbsp;");
                    lblSignedOff.RenderControl(writer);
                }

                if (btnSignOff != null)
                {
                    if (blnSignOff)
                    {
                        string strMode = HttpContext.Current.Request.QueryString[QUERYSTRING_MODE];
                        if (string.IsNullOrEmpty(strMode) || !string.Equals(strMode, VIEW))
                        {
                            writer.Write("</td></tr><tr><td valign=\"top\" colspan=\"3\" align=\"right\">");
                            btnSignOff.RenderControl(writer);
                            writer.Write("&nbsp;");
                            if (renderPrintButton)
                            {
                                btnPrintPage.RenderControl(writer);
                            }
                        }
                    }
                    else
                    {
                        writer.Write("</td></tr><tr><td valign=\"top\" colspan=\"3\" align=\"right\">");
                        if (renderPrintButton)
                        {
                            btnPrintPage.RenderControl(writer);
                        }
                    }
                }
                else
                {
                    writer.Write("</td></tr><tr><td valign=\"top\" align=\"right\">");
                    if (renderPrintButton)
                    {
                        btnPrintPage.RenderControl(writer);
                    }
                }

                writer.Write("</td></tr>");
            }
        }

        /// <summary>
        /// Renders the page.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderPage(HtmlTextWriter writer)
        {
            try
            {
                string strRequestXml = string.Empty;
                XmlDocument xmlRequest = new XmlDocument();
                objRequestInfo = new RequestInfo();
                #region GETRESULTXML
                base.ResponseType = ViewMode.ToString();
                //writer.Write("<link rel=\"stylesheet\" type=\"text/css\" href=\"/_layouts/DREAM/styles/DetailReport.css\" />");
                /// DetailReportRel2_1.css
                writer.Write("<link rel=\"stylesheet\" type=\"text/css\" href=\"/_layouts/DREAM/styles/DetailReportRel2_1.css\" />");
                if (Page.Request.QueryString.Count > 0 && strPicksFilter.Length == 0 && strDisplayType.Length == 0)
                {
                    if (Page.Request.QueryString[PAGENUMBERQS] != null)
                    {
                        /// Gets the report result for paging and sorting.
                        GetResultForPaging();
                    }
                    else
                    {
                        if (string.Equals(SearchName.ToString(), SEARCH_NAME_EPCATALOG))
                        {
                            strRequestXml = (string)HttpContext.Current.Session[SEARCH_NAME_EPCATALOG];
                            xmlRequest.LoadXml(strRequestXml);
                            xmlDocSearchResult = objReportController.GetSearchResults(xmlRequest, intMaxRecords, SearchName.ToString(), null, intSortOrder);
                        }
                        else if (!string.Equals(SearchName.ToString(), SEARCH_NAME_RECALLCURVE))
                        {
                            /// Gets the report result for other reports except Recall Curve.
                            GetResponseXML();
                        }

                        else
                        {
                            /// Gets the detail report result for Recall Curve.
                            GetRecallCurveReport();
                        }
                    }
                }
                else
                {
                    if (Page.Request.QueryString.Count > 0)
                    {
                        if (string.Equals(ViewMode.ToString(), VIEWMODE_TABULARDATA_SHEET))
                        {
                            base.RequestID = hidRequestID.Value.ToString().Trim();
                            SetViewMode(true);
                            objRequestInfo = SetBasicDataObjects(SearchName.ToString(), string.Empty, null, true, false, intMaxRecords);
                        }
                        else if (strPicksFilter.Length > 0)
                        {
                            base.RequestID = hidRequestID.Value.ToString().Trim();
                            base.ResponseType = TABULARREPORT;
                            strIdentifierValue[0] = strPicksFilter;
                            /// Creating the Request dataobject.
                            objRequestInfo = SetBasicDataObjects(SearchName.ToString(), GEOLOGICFEATURE, strIdentifierValue, true, true, intMaxRecords);
                        }
                        /// Calling the Controller method to get the Search results.
                        xmlDocSearchResult = objReportController.GetSearchResults(objRequestInfo, intMaxRecords, SearchName.ToString(), null, intSortOrder);
                    }
                }
                #endregion

                #region RenderResults
                if (xmlDocSearchResult != null)
                {
                    hidMaxRecord.Value = intMaxRecords.ToString();
                    hidSearchType.Value = SearchName.ToString();
                    GetRequestIDFromResult();
                    RenderParentTable(writer, true);
                    RenderPrintExport(writer);
                    RenderHiddenControls(writer);
                    if (Page.Request.QueryString[PAGENUMBERQS] != null && strPicksFilter.Length == 0 && strDisplayType.Length == 0)
                    {
                        strPageNumber = Page.Request.QueryString[PAGENUMBERQS].ToString();
                        strSortColumn = Page.Request.QueryString[SORTBY].ToString();
                        strSortOrder = Page.Request.QueryString[SORTTYPE].ToString();
                    }
                    RenderResults(writer);
                }
                else
                {
                    RenderExceptionMessage(writer, ((MOSSServiceManager)objMossController).GetCustomExceptionMessage(strCurrSiteUrl, "1"));
                }
                #endregion
            }
            catch (SoapException)
            {
                throw;
            }
            catch (WebException)
            {
                throw;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the recall curve report.
        /// </summary>
        private void GetRecallCurveReport()
        {
            base.ResponseType = TABULARREPORT;
            /// Setting the searchHelper class properties to create the Recall Log criteria values.
            base.UWBI = HttpContext.Current.Request.Form["hidUWBI"].ToString().Split('|');
            base.LogService = HttpContext.Current.Request.Form["hidLogService"].ToString().Split('|');
            base.LogType = HttpContext.Current.Request.Form["hidLogType"].ToString().Split('|');
            base.LogSource = HttpContext.Current.Request.Form["hidLogSource"].ToString().Split('|');
            base.LogName = HttpContext.Current.Request.Form["hidLogName"].ToString().Split('|');
            base.LogActivity = HttpContext.Current.Request.Form["hidLogActivity"].ToString().Split('|');
            base.Logrun = HttpContext.Current.Request.Form["hidLogrun"].ToString().Split('|');
            base.LogVersion = HttpContext.Current.Request.Form["hidLogVersion"].ToString().Split('|');
            base.Recall_Project_Name = HttpContext.Current.Request.Form["hidProjectName"].ToString().Split('|');
            /// Creating the Request dataobject.
            objRequestInfo = SetBasicDataObjects(SearchName.ToString(), string.Empty, null, false, false, intMaxRecords);
            /// Calling the Controller method to get the Search results.
            xmlDocSearchResult = objReportController.GetSearchResults(objRequestInfo, intMaxRecords, SearchName.ToString(), null, intSortOrder);
        }

        /// <summary>
        /// Renders the results.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderResults(HtmlTextWriter writer)
        {
            object objSessionUserPreference = null;
            XmlTextReader xmlTextReader = null;
            string strDepthUnit = string.Empty;

            strDepthUnit = objUserPreferences.DepthUnits;
            RenderFeetMeter(writer, strDepthUnit);
            #region PICKS FILTER
            if (string.Equals(SearchName.ToString(), SEARCH_NAME_PICKSDETAIL))
            {
                if (Page.Request.QueryString[PAGENUMBERQS] != null || strPicksFilter.Length > 0)
                {
                    /// Rendering the Picks dropdown.
                    writer.Write("<tr><td colspan=\"4\">");
                    LoadPicksDropDownFromSession(cboFilter);
                    cboFilter.RenderControl(writer);
                    writer.Write("</td></tr>");
                }
                else
                {
                    /// Rendering the Picks dropdown.
                    writer.Write("<tr><td colspan=\"4\">");
                    LoadPicksDropDown(cboFilter, xmlDocSearchResult);
                    cboFilter.RenderControl(writer);
                    writer.Write("</td></tr>");
                }
            }
            #endregion
            if (!string.Equals(ViewMode.ToString(), VIEWMODE_TABULARDATA_SHEET))
            {
                xmlTextReader = ((MOSSServiceManager)objMossController).GetXSLTemplate(GetEnumDescription(XSLFileName), strCurrSiteUrl);
            }
            else
            {
                if (string.Equals(strDisplayType.ToUpper(), DISPLAY_TYPE_TABULAR))
                {
                    xmlTextReader = ((MOSSServiceManager)objMossController).GetXSLTemplate(GetEnumDescription(XSLFileName), strCurrSiteUrl);
                }
                else
                    if (string.Equals(SearchName.ToString(), SEARCH_NAME_WELLBOREHEADER))
                        xmlTextReader = ((MOSSServiceManager)objMossController).GetXSLTemplate(GetEnumDescription(XSLFileEnum.WellboreHeader_Datasheet), strCurrSiteUrl);
                    else if (string.Equals(SearchName.ToString(), SEARCH_NAME_PARS_DETAIL))
                        xmlTextReader = ((MOSSServiceManager)objMossController).GetXSLTemplate(GetEnumDescription(XSLFileEnum.PARSDetail_Datasheet), strCurrSiteUrl);
                    else
                        xmlTextReader = ((MOSSServiceManager)objMossController).GetXSLTemplate(GetEnumDescription(XSLFileName), strCurrSiteUrl);
            }

            objSessionUserPreference = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString());
            if (objSessionUserPreference != null)
            {
                objPreferences = (UserPreferences)objSessionUserPreference;
            }
            writer.Write("<tr><td colspan=\"4\">");

            TransformSearchResultsToXSL(xmlDocSearchResult, xmlTextReader, strPageNumber, strSortColumn, strSortOrder, SearchName.ToString(), intMaxRecords, GetEnumDescription(SearchName), intRecordcount, string.Empty);
            writer.Write(strResultTable.ToString());//DREAM 4.0
            if (intDetailRecordCount != 0)
            {
                hidRowsCount.Value = intDetailRecordCount.ToString();
                hidRowsCount.RenderControl(writer);
            }
            writer.Write("</td></tr></table>");
        }

        /// <summary>
        /// Renders the feet meter.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="depthUnit">The depth unit.</param>
        private void RenderFeetMeter(HtmlTextWriter writer, string depthUnit)
        {
            if (string.Equals(ViewMode.ToString(), VIEWMODE_TABULARDATA_SHEET))
            {
                if (UnitConversion)
                {
                    writer.Write("<tr><td colspan=\"1\" align=\"left\" class=\"tdAdvSrchItem\"><b>Depth Units: </b>");
                    if (string.Equals(depthUnit, FEET))
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
                    RenderViewMode(writer);
                }
                else
                {
                    writer.Write("<tr><td colspan=\"2\" align=\"right\" class=\"tdAdvSrchItem\"><b>View: </b>");
                    RenderViewMode(writer);
                }
            }
            else
            {
                if (UnitConversion)
                {
                    if (DetailReport)
                    {
                        writer.Write("<tr><td colspan=\"1\" align=\"left\" class=\"tdAdvSrchItem\"><b>Depth Units: </b>");
                        if (string.Equals(depthUnit, FEET))
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

                        writer.Write("</td><td colspan=\"3\" align=\"right\" class=\"tdAdvSrchItem\">");
                        linkViewDetailReport.RenderControl(writer);
                        writer.Write("</td></tr>");
                    }
                    else
                    {
                        writer.Write("<tr><td colspan=\"4\" align=\"left\" class=\"tdAdvSrchItem\"><b>Depth Units: </b>");
                        if (string.Equals(depthUnit, FEET))
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
                    }
                }
            }
            writer.Write("</td></tr>");
        }

        /// <summary>
        /// Loads the picks drop down.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="searchResult">The search result.</param>
        private void LoadPicksDropDown(DropDownList filter, XmlDocument searchResult)
        {
            string strValue = string.Empty;
            XmlNodeList xmlValues = searchResult.SelectNodes(GEOLOGICFEATUREXPATH);

            filter.Items.Clear();
            filter.Items.Add(SELECTALL);
            filter.Items[0].Value = PICKSSELECTEDVAL;
            /// Loop through the Geological Features and adds them to filter field.
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
        /// Loads the picks drop down from session.
        /// </summary>
        /// <param name="filter">The filter.</param>
        private void LoadPicksDropDownFromSession(DropDownList filter)
        {
            filter.Items.Clear();
            /// Gets the filter values from session.
            object objFilterValues = CommonUtility.GetSessionVariable(Page, enumSessionVariable.picksFilterValues.ToString());
            if (objFilterValues != null)
            {
                /// Assign the values to the Picks filter dropdown.
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
        /// Renders the view mode rabio button list.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderViewMode(HtmlTextWriter writer)
        {
            /// Set the viewmode value based on user preferences.
            if (string.Equals(strDisplayType.ToUpper(), DISPLAY_TYPE_TABULAR))
            {
                rdoViewMode.SelectedIndex = 1;
            }
            else
            {
                rdoViewMode.SelectedIndex = 0;
            }
            rdoViewMode.RenderControl(writer);
        }

        /// <summary>
        /// Renders the hidden controls.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderHiddenControls(HtmlTextWriter writer)
        {
            hidSelectedCriteriaName.RenderControl(writer);
            hidRequestID.RenderControl(writer);
            hidReportSelectRow.RenderControl(writer);
            hidSelectedRows.RenderControl(writer);
            hidMaxRecord.RenderControl(writer);
            hidSelectedColumns.RenderControl(writer);
            hidSearchType.RenderControl(writer);
            hdnIncludePageTitle.RenderControl(writer);
            hdnIncludeStoryBoard.RenderControl(writer);
            if (string.Equals(SearchName.ToString(), SEARCH_NAME_RECALL_LOGS))
            {
                hidUWBI.RenderControl(writer);
                hidLogService.RenderControl(writer);
                hidLogType.RenderControl(writer);
                hidLogSource.RenderControl(writer);
                hidLogName.RenderControl(writer);
                hidLogActivity.RenderControl(writer);
                hidLogrun.RenderControl(writer);
                hidLogVersion.RenderControl(writer);
                hidProjectName.RenderControl(writer);
            }
        }

        /// <summary>
        /// Renders the print export.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderPrintExport(HtmlTextWriter writer)
        {
            #region AllApplicable
            if (Print && ExportPage && ExportAll)
            {
                if (!ShowAllResults)
                {
                    if (objCommonUtility.IsMaxRecordExceeds(xmlDocSearchResult, blnClicked))
                    {
                        string strMaxRecord = PortalConfiguration.GetInstance().GetKey(MAXRECORDS_KEY);
                        writer.Write("<tr><td ID=\"continueSearch\" class=\"tdAdvSrchItem\" width=\"60%\">Record count exceeds ");
                        writer.Write(strMaxRecord);
                        writer.Write(". To fetch all records");
                        writer.Write("&nbsp;");
                        linkContinueSearch.RenderControl(writer);
                        writer.Write("</td><td class=\"tdAdvSrchItem\" width=\"40%\" align=\"right\">Print");
                    }
                    else
                    {
                        writer.Write("<tr><td class=\"tdAdvSrchItem\" colspan=\"4\" align=\"right\" width=\"100%\">Print");
                    }
                }
                else
                {
                    writer.Write("<tr><td class=\"tdAdvSrchItem\" colspan=\"4\" align=\"right\" width=\"100%\">Print");
                }
                writer.Write("&nbsp;");
                lnkPrint.RenderControl(writer);
                if (string.Equals(ViewMode.ToString(), VIEWMODE_TABULARDATA_SHEET))
                {
                    if (!string.Equals(strDisplayType.ToUpper(), DATASHEET) && !string.Equals(strDisplayType.ToUpper(), INITIALDATASHEET))
                    {
                        writer.Write("&nbsp;");
                        writer.Write(EXPORT_PAGE_TEXT);
                        writer.Write("&nbsp;");
                        lnkExportPage.RenderControl(writer);
                        writer.Write("&nbsp;");
                        writer.Write(EXPORT_ALL_TEXT);
                        writer.Write("&nbsp;");
                        lnkExportAll.RenderControl(writer);
                    }
                }
                else
                {
                    writer.Write("&nbsp;");
                    writer.Write(EXPORT_PAGE_TEXT);
                    writer.Write("&nbsp;");
                    lnkExportPage.RenderControl(writer);
                    writer.Write("&nbsp;");
                    writer.Write(EXPORT_ALL_TEXT);
                    writer.Write("&nbsp;");
                    lnkExportAll.RenderControl(writer);
                }
                if (string.Equals(SearchName.ToString(), SEARCH_NAME_PARS_DETAIL))
                {
                    lnkiRequest.Visible = true;
                    writer.Write("&nbsp;");
                    writer.Write(ARCHIVE_RESTORE_TEXT);
                    writer.Write("&nbsp;");
                    lnkiRequest.RenderControl(writer);
                }
                writer.Write("</td></tr>");
            }
            #endregion
            #region PrintExportPage
            if (Print && ExportPage && !ExportAll)
            {
                if (!ShowAllResults)
                {
                    if (objCommonUtility.IsMaxRecordExceeds(xmlDocSearchResult, blnClicked))
                    {
                        string strMaxRecord = PortalConfiguration.GetInstance().GetKey(MAXRECORDS_KEY);
                        writer.Write("<tr><td ID=\"continueSearch\" class=\"tdAdvSrchItem\" width=\"60%\">Record count exceeds ");
                        writer.Write(strMaxRecord);
                        writer.Write(". To fetch all records");
                        writer.Write("&nbsp;");
                        linkContinueSearch.RenderControl(writer);
                        writer.Write("</td><td class=\"tdAdvSrchItem\" width=\"40%\" align=\"right\">Print");
                    }
                    else
                    {
                        writer.Write("<tr><td class=\"tdAdvSrchItem\" colspan=\"3\" align=\"right\" width=\"100%\">Print");
                    }
                }
                else
                {
                    writer.Write("<tr><td class=\"tdAdvSrchItem\" colspan=\"3\" align=\"right\" width=\"100%\">Print");
                }
                writer.Write("&nbsp;");
                lnkPrint.RenderControl(writer);
                writer.Write("&nbsp;");
                writer.Write(EXPORT_PAGE_TEXT);
                writer.Write("&nbsp;");
                lnkExportPage.RenderControl(writer);
                if (string.Equals(SearchName.ToString(), SEARCH_NAME_PARS_DETAIL))
                {
                    lnkiRequest.Visible = true;
                    writer.Write("&nbsp;");
                    writer.Write(ARCHIVE_RESTORE_TEXT);
                    writer.Write("&nbsp;");
                    lnkiRequest.RenderControl(writer);
                }
                writer.Write("</td></tr>");
            }
            #endregion
            #region PrintExportAll
            if (Print && !ExportPage && ExportAll)
            {
                if (!ShowAllResults)
                {
                    if (objCommonUtility.IsMaxRecordExceeds(xmlDocSearchResult, blnClicked))
                    {
                        string strMaxRecord = PortalConfiguration.GetInstance().GetKey(MAXRECORDS_KEY);
                        writer.Write("<tr><td ID=\"continueSearch\" class=\"tdAdvSrchItem\" width=\"60%\">Record count exceeds ");
                        writer.Write(strMaxRecord);
                        writer.Write(". To fetch all records");
                        writer.Write("&nbsp;");
                        linkContinueSearch.RenderControl(writer);
                        writer.Write("</td><td class=\"tdAdvSrchItem\" width=\"40%\" align=\"right\">Print");
                    }
                    else
                    {
                        writer.Write("<tr><td class=\"tdAdvSrchItem\" colspan=\"3\" align=\"right\" width=\"100%\">Print");
                    }
                }
                else
                {
                    writer.Write("<tr><td class=\"tdAdvSrchItem\" colspan=\"3\" align=\"right\" width=\"100%\">Print");
                }
                writer.Write("&nbsp;");
                lnkPrint.RenderControl(writer);
                writer.Write("&nbsp;");
                writer.Write(EXPORT_ALL_TEXT);
                writer.Write("&nbsp;");
                lnkExportAll.RenderControl(writer);
                if (string.Equals(SearchName.ToString(), SEARCH_NAME_PARS_DETAIL))
                {
                    lnkiRequest.Visible = true;
                    writer.Write("&nbsp;");
                    writer.Write(ARCHIVE_RESTORE_TEXT);
                    writer.Write("&nbsp;");
                    lnkiRequest.RenderControl(writer);
                }
                writer.Write("</td></tr>");
            }
            #endregion
            #region ExportAllExportPage
            if (!Print && ExportPage && ExportAll)
            {
                if (!ShowAllResults)
                {
                    if (objCommonUtility.IsMaxRecordExceeds(xmlDocSearchResult, blnClicked))
                    {
                        string strMaxRecord = PortalConfiguration.GetInstance().GetKey(MAXRECORDS_KEY);
                        writer.Write("<tr><td ID=\"continueSearch\" class=\"tdAdvSrchItem\" width=\"60%\">Record count exceeds ");
                        writer.Write(strMaxRecord);
                        writer.Write(". To fetch all records");
                        writer.Write("&nbsp;");
                        linkContinueSearch.RenderControl(writer);
                        writer.Write("</td><td class=\"tdAdvSrchItem\" width=\"40%\" align=\"right\">Print");
                    }
                    else
                    {
                        writer.Write("<tr><td class=\"tdAdvSrchItem\" colspan=\"3\" align=\"right\" width=\"100%\">Print");
                    }
                }
                else
                {
                    writer.Write("<tr><td class=\"tdAdvSrchItem\" colspan=\"3\" align=\"right\" width=\"100%\">Print");
                }
                writer.Write("&nbsp;");
                writer.Write(EXPORT_PAGE_TEXT);
                writer.Write("&nbsp;");
                lnkExportPage.RenderControl(writer);
                writer.Write("&nbsp;");
                writer.Write(EXPORT_ALL_TEXT);
                writer.Write("&nbsp;");
                lnkExportAll.RenderControl(writer);
                if (string.Equals(SearchName.ToString(), SEARCH_NAME_PARS_DETAIL))
                {
                    lnkiRequest.Visible = true;
                    writer.Write("&nbsp;");
                    writer.Write(ARCHIVE_RESTORE_TEXT);
                    writer.Write("&nbsp;");
                    lnkiRequest.RenderControl(writer);
                }
                writer.Write("</td></tr>");
            }
            #endregion
            #region PrintExportAll
            if (Print && !ExportPage && !ExportAll)
            {
                if (!ShowAllResults)
                {
                    if (objCommonUtility.IsMaxRecordExceeds(xmlDocSearchResult, blnClicked))
                    {
                        string strMaxRecord = PortalConfiguration.GetInstance().GetKey(MAXRECORDS_KEY);
                        writer.Write("<tr><td ID=\"continueSearch\" class=\"tdAdvSrchItem\" width=\"60%\">Record count exceeds ");
                        writer.Write(strMaxRecord);
                        writer.Write(". To fetch all records");
                        writer.Write("&nbsp;");
                        linkContinueSearch.RenderControl(writer);
                        writer.Write("</td><td class=\"tdAdvSrchItem\" width=\"40%\" align=\"right\">Print");
                    }
                    else
                    {
                        writer.Write("<tr><td class=\"tdAdvSrchItem\" colspan=\"2\" align=\"right\" width=\"100%\">Print");
                    }
                }
                else
                {
                    writer.Write("<tr><td class=\"tdAdvSrchItem\" colspan=\"2\" align=\"right\" width=\"100%\">Print");
                }
                writer.Write("&nbsp;");
                lnkPrint.RenderControl(writer);
                writer.Write("&nbsp;");
                if (string.Equals(SearchName.ToString(), SEARCH_NAME_PARS_DETAIL))
                {
                    lnkiRequest.Visible = true;
                    writer.Write("&nbsp;");
                    writer.Write(ARCHIVE_RESTORE_TEXT);
                    writer.Write("&nbsp;");
                    lnkiRequest.RenderControl(writer);
                }
                writer.Write("</td></tr>");
            }
            #endregion
            #region PrintExportAll
            if (!Print && ExportPage && !ExportAll)
            {
                if (!ShowAllResults)
                {
                    if (objCommonUtility.IsMaxRecordExceeds(xmlDocSearchResult, blnClicked))
                    {
                        string strMaxRecord = PortalConfiguration.GetInstance().GetKey(MAXRECORDS_KEY);
                        writer.Write("<tr><td ID=\"continueSearch\" class=\"tdAdvSrchItem\" width=\"60%\">Record count exceeds ");
                        writer.Write(strMaxRecord);
                        writer.Write(". To fetch all records");
                        writer.Write("&nbsp;");
                        linkContinueSearch.RenderControl(writer);
                        writer.Write("</td><td class=\"tdAdvSrchItem\" width=\"40%\" align=\"right\">" + EXPORT_PAGE_TEXT);
                    }
                    else
                    {
                        writer.Write("<tr><td class=\"tdAdvSrchItem\" colspan=\"3\" align=\"right\" width=\"100%\">" + EXPORT_PAGE_TEXT);
                    }
                }
                else
                {
                    writer.Write("<tr><td class=\"tdAdvSrchItem\" colspan=\"3\" align=\"right\" width=\"100%\">" + EXPORT_PAGE_TEXT);
                }
                writer.Write("&nbsp;");
                lnkExportPage.RenderControl(writer);
                writer.Write("&nbsp;");
                writer.Write("</td></tr>");
            }
            #endregion
        }

        /// <summary>
        /// Gets the request ID from result.
        /// </summary>
        private void GetRequestIDFromResult()
        {
            try
            {
                XmlNodeList objXmlNodeList = null;
                objXmlNodeList = xmlDocSearchResult.SelectNodes(RESPONSE_NODE);
                if (objXmlNodeList != null)
                {
                    foreach (XmlNode xmlNode in objXmlNodeList)
                    {
                        hidRequestID.Value = xmlNode.Attributes[REQUEST_ID].Value.ToString();
                    }
                }
            }
            catch
            { throw; }
        }

        /// <summary>
        /// Gets the result for paging.
        /// </summary>
        private void GetResultForPaging()
        {
            try
            {
                if (Page.Request.QueryString[MAXRECORDS_KEY] != null)
                {
                    intMaxRecords = Convert.ToInt32(Page.Request.QueryString[MAXRECORDS_KEY].ToString());
                }
                if (Page.Request.QueryString[PAGENUMBERQS].Length > 0)
                {
                    /// Sets the required attributes for fetching the result from report service.
                    strPageNumber = Page.Request.QueryString[PAGENUMBERQS].ToString();
                }
                if (Page.Request.QueryString[SORTBY].ToString().Length > 0)
                {
                    strSortColumn = Page.Request.QueryString[SORTBY].ToString();
                    strSortOrder = Page.Request.QueryString[SORTTYPE].ToString();
                }
                else
                    strSortColumn = null;
                intRecordcount = Convert.ToInt32(Page.Request.QueryString[RECORDCOUNTQS].ToString());
                if (strSortOrder == DESCENDING)
                {
                    intSortOrder = 1;
                }
                strCurrPageNumber = strPageNumber;
                /// Sets the RequestID from the Query string.
                base.RequestID = Page.Request.QueryString[REQUESTIDQS].ToString();
                base.ResponseType = TABULARREPORT;
                strDisplayType = TABULARREPORT;
                /// Creating the Request dataobject.
                objRequestInfo = SetBasicDataObjects(SearchName.ToString(), string.Empty, null, false, false, intMaxRecords);
                /// Calling the Controller method to get the Search results.
                xmlDocSearchResult = objReportController.GetSearchResults(objRequestInfo, intMaxRecords, SearchName.ToString(), strSortColumn, intSortOrder);
                blnClicked = true;
            }
            catch (SoapException)
            {
                throw;
            }
            catch (WebException)
            {
                throw;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the response XML.
        /// </summary>
        private void GetResponseXML()
        {
            try
            {
                if (strDisplayType.Length == 0)
                    SetUserPreference();
                #region ResponseType
                SetViewMode(false);
                #endregion
                if (!ShowAllResults)
                {
                    if (intMaxRecords == -1)
                    {
                        /// Sets the RequestID property of the base class from the hiddenfield.
                        base.RequestID = hidRequestID.Value.ToString().Trim();
                        base.strCurrPageNumber = "0";
                        /// creates the requestInfo object to fetch all the records from report service.
                        objRequestInfo = SetBasicDataObjects(SearchName.ToString(), string.Empty, strIdentifierValue, false, true, intMaxRecords);
                    }
                    else
                    {
                        CreateDataObject();
                    }
                }
                else
                {
                    CreateDataObject();
                }
                if (objRequestInfo != null)
                {
                    /// call for the GetSearchResults() method to fetch the search results from webservice.
                    xmlDocSearchResult = objReportController.GetSearchResults(objRequestInfo, intMaxRecords, SearchName.ToString(), null, intSortOrder);
                }
            }
            catch (SoapException)
            {
                throw;
            }
            catch (WebException)
            {
                throw;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Sets the view mode.
        /// </summary>
        private void SetViewMode(bool setUserPreference)
        {
            if (string.Equals(ViewMode.ToString(), VIEWMODE_TABULARDATA_SHEET))
            {
                if (setUserPreference)
                {
                    if (strDisplayType.Length == 0)
                        SetUserPreference();
                }
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
                if (string.Equals(ViewMode.ToString(), VIEWMODE_DATA_SHEET))
                    base.ResponseType = DATASHEETREPORT;
                else
                    base.ResponseType = TABULARREPORT;
            }
        }

        /// <summary>
        /// Creates the data object.
        /// </summary>
        private void CreateDataObject()
        {
            if (HttpContext.Current.Session[SESSION_TREEVIEWDATAOBJECT] != null)
            {
                SetRequestInfoObject();
            }
            else if (HttpContext.Current.Request.QueryString[QUERYSTRING_MODE] != null)
                SetRequestInfoObject();
            else
                objRequestInfo = null;
        }

        /// <summary>
        /// Sets the request info object.
        /// </summary>
        private void SetRequestInfoObject()
        {
            string strChapterID = string.Empty;
            string strAssetType = string.Empty;
            if (HttpContext.Current.Request.QueryString[QUERYSTRING_CHAPTERID] != null)
                strChapterID = Convert.ToString(HttpContext.Current.Request.QueryString[QUERYSTRING_CHAPTERID]);
            objBookInfo = ((BookInfo)HttpContext.Current.Session[SESSION_TREEVIEWDATAOBJECT]);
            if (objBookInfo != null)
            {
                strBookOwner = objBookInfo.BookOwner;
                strBookTeamID = objBookInfo.BookTeamID;
                foreach (ChapterInfo objChapterInfo in objBookInfo.Chapters)
                {
                    if (string.Equals(objChapterInfo.ChapterID, ChapterID))
                    {
                        strIdentifierValue[0] = objChapterInfo.ActualAssetValue;
                        strAssetType = objChapterInfo.ColumnName;
                        if (string.Equals(objChapterInfo.AssetType, ASSETTYPE_WELL))
                            strAssetType = UWI;
                        else if (string.Equals(objChapterInfo.AssetType, ASSETTYPE_FIELD))
                            strAssetType = FIELD_IDENTIFIER;
                        else
                            strAssetType = UWBI;
                        /// If $ is present in Page Title replace with Asset Value
                        /// Else prefix with Asset Value.
                        if (lblTitleTemplate.Text.IndexOf("$") == 0)
                        {
                            lblTitleTemplate.Text = lblTitleTemplate.Text.Replace("$", objChapterInfo.AssetValue);
                        }
                        else
                        {
                            lblTitleTemplate.Text = string.Concat(objChapterInfo.AssetValue, "-", lblTitleTemplate.Text);
                        }
                    }
                }
                objRequestInfo = SetBasicDataObjects(SearchName.ToString(), strAssetType, strIdentifierValue, false, false, intMaxRecords);
            }
        }

        /// <summary>
        /// Initializes the user preference.
        /// </summary>
        private void InitializeUserPreference()
        {
            try
            {
                string strUserId = objCommonUtility.GetUserName();
                /// Reads the user preferences.
                objUserPreferences = ((MOSSServiceManager)objMossController).GetUserPreferencesValue(strUserId, strCurrSiteUrl);
                CommonUtility.SetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString(), objUserPreferences);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
        }

        /// <summary>
        /// Sets the user preference.
        /// </summary>
        private void SetUserPreference()
        {
            object objBeforeSessionUserPreference = null;
            object objSessionUserPreference = null;
            /// Validates the user preferences session value.
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
                if (strDisplayType.Length == 0)
                    strDisplayType = objUserPreferences.Display;
            }
        }

        /// <summary>
        /// Creates the message label.
        /// </summary>
        private void CreateMessageLabel()
        {
            lblMessage.ID = "lblMessage";
            lblMessage.CssClass = "labelMessage";
            /// Sets the default visibility of messege label to false.
            lblMessage.Visible = false;
            this.Controls.Add(lblMessage);
        }

        /// <summary>
        /// Creates the hyper links.
        /// </summary>
        private void CreateHyperLinks()
        {
            if (Print)
            {
                lnkPrint = new HyperLink();
                lnkPrint.ID = "linkPrint";
                lnkPrint.CssClass = "resultHyperLink";
                lnkPrint.NavigateUrl = "javascript:printContent('tblSearchResults','" + SearchName.ToString() + "');";
                lnkPrint.ImageUrl = "/_layouts/DREAM/images/icon_print.gif";
                this.Controls.Add(lnkPrint);
            }
            //if (ExportPage)
            //{
            lnkExportPage = new HyperLink();
            lnkExportPage.ID = "linkExcel";
            lnkExportPage.CssClass = "resultHyperLink";
            lnkExportPage.NavigateUrl = "javascript:DWBExportToExcelPreProd('tblSearchResults');";
            lnkExportPage.ImageUrl = "/_layouts/DREAM/images/icon_Excel.gif";
            this.Controls.Add(lnkExportPage);
            //}
            if (ExportAll)
            {
                lnkExportAll = new HyperLink();
                lnkExportAll.ID = "linkExportAll";
                lnkExportAll.CssClass = "resultHyperLink";
                lnkExportAll.NavigateUrl = "javascript:ExportToExcelAll('/Pages/ExportToExcel.aspx','ContextSearchResults');";
                lnkExportAll.ImageUrl = "/_layouts/DREAM/images/icon_Excel.gif";
                this.Controls.Add(lnkExportAll);
            }
            if (string.Equals(SearchName.ToString(), SEARCH_NAME_PARS_DETAIL))
            {
                lnkiRequest = new HyperLink();
                lnkiRequest.ID = "linkiRequest";
                lnkiRequest.CssClass = "resultHyperLink";
                lnkiRequest.NavigateUrl = "javascript:OpeniRequest();";
                lnkiRequest.ImageUrl = "/_layouts/DREAM/images/iRequest.gif";
                this.Controls.Add(lnkiRequest);
            }
            if (!ShowAllResults)
            {
                linkContinueSearch = new LinkButton();
                linkContinueSearch.ID = "linkContinueSearch";
                linkContinueSearch.Attributes.Add("style", "cursor:hand;text-decoration:underline;color:Blue;");
                linkContinueSearch.Attributes.Add("runat", "server");
                linkContinueSearch.Text = "click here";
                linkContinueSearch.OnClientClick = "javascript:OpenBusyBox();";
                linkContinueSearch.Click += new EventHandler(linkContinueSearch_Click);
                this.Controls.Add(linkContinueSearch);
            }
            if (DetailReport)
            {
                linkViewDetailReport = new HyperLink();
                linkViewDetailReport.ID = "linkViewDetailReport";
                linkViewDetailReport.CssClass = "resultHyperLink";
                linkViewDetailReport.Attributes.Add("text-decoration", "underline");
                linkViewDetailReport.NavigateUrl = "javascript:OpenDetailReport('" + GetEnumDescription(SearchName) + "');";
                linkViewDetailReport.Text = "View Detail Report";
                this.Controls.Add(linkViewDetailReport);
            }
        }

        /// <summary>
        /// Handles the Click event of the linkContinueSearch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void linkContinueSearch_Click(object sender, EventArgs e)
        {
            intMaxRecords = -1;
            blnClicked = true;
        }

        /// <summary>
        /// Creates the hidden control.
        /// </summary>
        private void CreateHiddenControl()
        {
            hidSelectedRows = new HiddenField();
            hidSelectedRows.ID = "hidSelectedRows";
            hidRequestID = new HiddenField();
            hidRequestID.ID = "hidRequestID";
            hidReportSelectRow = new HiddenField();
            hidReportSelectRow.ID = "hidReportSelectRow";
            hidMaxRecord = new HiddenField();
            hidMaxRecord.ID = "hidMaxRecord";
            hidSelectedColumns = new HiddenField();
            hidSelectedColumns.ID = "hidSelectedColumns";
            hidSearchType = new HiddenField();
            hidSearchType.ID = "hidSearchType";
            hidSelectedCriteriaName = new HiddenField();
            hidSelectedCriteriaName.ID = "hidSelectedCriteriaName";
            hidRowsCount = new HiddenField();
            hidRowsCount.ID = "hidRowsCount";

            /// Hidden fields loses value on Click event.
            /// Hence TextBox is used to get the selected Print Options.
            /// style=display:none is added so control will be added to DOM 
            /// for JS to assign values at client side meanwhile TextBox will not be visible                
            hdnIncludePageTitle = new TextBox();
            hdnIncludePageTitle.Attributes.Add("style", "display:none;");
            hdnIncludePageTitle.ID = "hdnIncludePageTitle";
            hdnIncludePageTitle.EnableViewState = true;
            hdnIncludeStoryBoard = new TextBox();
            hdnIncludeStoryBoard.EnableViewState = true;
            hdnIncludeStoryBoard.ID = "hdnIncludeStoryBoard";
            hdnIncludeStoryBoard.Attributes.Add("style", "display:none;");
            this.Controls.Add(hdnIncludePageTitle);
            this.Controls.Add(hdnIncludeStoryBoard);

            if (string.Equals(SearchName.ToString(), SEARCH_NAME_RECALL_LOGS))
            {
                hidUWBI = new HiddenField();
                hidUWBI.ID = "hidUWBI";
                hidLogService = new HiddenField();
                hidLogService.ID = "hidLogService";
                hidLogType = new HiddenField();
                hidLogType.ID = "hidLogType";
                hidLogSource = new HiddenField();
                hidLogSource.ID = "hidLogSource";
                hidLogName = new HiddenField();
                hidLogName.ID = "hidLogName";
                hidLogActivity = new HiddenField();
                hidLogActivity.ID = "hidLogActivity";
                hidLogrun = new HiddenField();
                hidLogrun.ID = "hidLogrun";
                hidLogVersion = new HiddenField();
                hidLogVersion.ID = "hidLogVersion";
                hidProjectName = new HiddenField();
                hidProjectName.ID = "hidProjectName";
            }
        }

        /// <summary>
        /// Creates the radio controls.
        /// </summary>
        private void CreateRadioControls()
        {
            if (UnitConversion)
            {
                /// This will create option for feet/meter selection
                rbFeet = new RadioButton();
                rbFeet.ID = "rbFeet";
                rbFeet.GroupName = FEET_RDB_GROUP_NAME;
                rbFeet.Text = FEET;
                if (string.Equals(ViewMode.ToString(), VIEWMODE_DATA_SHEET))
                {
                    rbFeet.Attributes.Add(ONCLICK_EVENT_NAME, "javascript:ConvertFeetMetresDetail(this.value,'" + GetEnumDescription(SearchName) + "','Data Sheet');");
                }
                else
                {
                    rbFeet.Attributes.Add(ONCLICK_EVENT_NAME, "javascript:ConvertFeetMetresDetail(this.value,'" + GetEnumDescription(SearchName) + "');");
                }
                this.Controls.Add(rbFeet);

                rbMeters = new RadioButton();
                rbMeters.ID = "rbMeters";
                rbMeters.GroupName = FEET_RDB_GROUP_NAME;
                rbMeters.Text = METRES;
                if (string.Equals(ViewMode.ToString(), VIEWMODE_DATA_SHEET))
                {
                    rbMeters.Attributes.Add(ONCLICK_EVENT_NAME, "javascript:ConvertFeetMetresDetail(this.value,'" + GetEnumDescription(SearchName) + "','Data Sheet');");
                }
                else
                {
                    rbMeters.Attributes.Add(ONCLICK_EVENT_NAME, "javascript:ConvertFeetMetresDetail(this.value,'" + GetEnumDescription(SearchName) + "');");
                }
                this.Controls.Add(rbMeters);
            }
            if (string.Equals(ViewMode.ToString(), VIEWMODE_TABULARDATA_SHEET))
            {
                rdoViewMode = new RadioButtonList();
                rdoViewMode.AutoPostBack = true;
                rdoViewMode.ID = "rdoViewMode";
                rdoViewMode.RepeatDirection = RepeatDirection.Horizontal;
                rdoViewMode.RepeatLayout = RepeatLayout.Flow;
                rdoViewMode.Items.Add("Data Sheet");
                rdoViewMode.Items.Add(TABULARREPORT);
                rdoViewMode.SelectedIndexChanged += new EventHandler(rdoViewMode_SelectedIndexChanged);
                this.Controls.Add(rdoViewMode);
            }
            if (string.Equals(SearchName.ToString(), SEARCH_NAME_PICKSDETAIL))
            {
                cboFilter = new DropDownList();
                cboFilter.ID = "cboFilter";
                cboFilter.EnableViewState = true;
                cboFilter.AutoPostBack = true;
                cboFilter.SelectedIndexChanged += new EventHandler(cboFilter_SelectedIndexChanged);
                this.Controls.Add(cboFilter);
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cboFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void cboFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.Form[this.UniqueID + FILTERDROPDOWN] != null)
            {
                strPicksFilter = HttpContext.Current.Request.Form[this.UniqueID + FILTERDROPDOWN].ToString();
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the rdoViewMode control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void rdoViewMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.Form[this.UniqueID + VIEWMODERADIOGROUP] != null)
            {
                strDisplayType = HttpContext.Current.Request.Form[this.UniqueID + VIEWMODERADIOGROUP].ToString();
            }
        }

        /// <summary>
        /// Print the current Page/Selected Pages
        /// </summary>
        private int PrintPage(PrintOptions objPrintOptions)
        {
            bool blnUserControlFound = false;
            RadTreeView objTreeViewWellBook = null;
            WellBookBLL objWellBookBLL = null;
            StringBuilder strPageIds = null;
            ArrayList arlPageDetails = null;
            ArrayList arlChapterCollection = null;
            string strSplitter = ";";
            BookInfo objBookInfo = null;
            ChapterInfo objChapterInfo = null;
            XmlDocument xmlWellBookDetails = null;
            string intPrintedDocID = string.Empty;
            string strChapterIDForPage = string.Empty;
            string strSelectedPageID = string.Empty;
            string strBookID = string.Empty;
            objWellBookBLL = new WellBookBLL();
            objCommonBLL = new CommonBLL();
            arlChapterCollection = new ArrayList();
            /// If in View Mode, take page ID and chapter ID from Query string
            if (HttpContext.Current.Request.QueryString[QUERYSTRING_MODE] == null)
            {
                if (this.Parent.GetType().Equals(typeof(SPWebPartManager)))
                {
                    foreach (Control ctrlWebPart in this.Parent.Controls)
                    {
                        if (ctrlWebPart.GetType().Equals(typeof(TreeViewControl.TreeViewControl)))
                        {

                            Control ctrlTreeView = ctrlWebPart.FindControl("RadTreeView1");
                            if (ctrlTreeView != null && ctrlTreeView.GetType().Equals(typeof(RadTreeView)))
                            {
                                objTreeViewWellBook = (RadTreeView)ctrlWebPart.FindControl("RadTreeView1");
                                blnUserControlFound = true;
                                break;
                            }
                            if (blnUserControlFound)
                            {
                                break;
                            }
                        }
                    }
                }

                if (objTreeViewWellBook != null)
                {
                    /// If selected node is book node, loop into each chapter node
                    /// If chapter node is checked, loop into each page node and create book info object
                    /// Else if selected node is chapter node, loop into each page node
                    /// Add only selected page details to chapter object.
                    /// Else if selected node is page node, create bookinfo which includes only selected page.

                    if (objTreeViewWellBook.SelectedNode.Level == 2)
                    {
                        strBookID = objTreeViewWellBook.SelectedNode.ParentNode.ParentNode.Value;
                        strChapterIDForPage = objTreeViewWellBook.SelectedNode.ParentNode.Value;
                        strSelectedPageID = objTreeViewWellBook.SelectedNode.Value;
                    }
                }
            }
            else
            {
                if (string.Compare(HttpContext.Current.Request.QueryString[QUERYSTRING_MODE], VIEW) == 0)
                {
                    strSelectedPageID = HttpContext.Current.Request.QueryString[QUERYSTRING_PAGEID];
                    strChapterIDForPage = HttpContext.Current.Request.QueryString[QUERYSTRING_CHAPTERID];
                    BookInfo objBookInfoSession = ((BookInfo)HttpContext.Current.Session[SESSION_TREEVIEWDATAOBJECT]);
                    if (objBookInfoSession != null)
                    {
                        strBookID = objBookInfoSession.BookID;
                    }
                }
            }
            strParentSiteURL = SPContext.Current.Site.Url.ToString();
            objBookInfo = objWellBookBLL.SetBookDetailDataObject(strParentSiteURL, strBookID, BOOKACTION_PRINT, false,objPrintOptions);
            objChapterInfo = objWellBookBLL.SetChapterDetails(strParentSiteURL, strChapterIDForPage, false);
            if (objBookInfo != null)
            {
                if (objChapterInfo != null)
                {
                    strPageIds = new StringBuilder();
                    strPageIds.Append(strSelectedPageID);
                    strPageIds.Append(strSplitter);
                    arlPageDetails = objWellBookBLL.SetSelectedPageInfo(strParentSiteURL, strPageIds.ToString(), objChapterInfo.ActualAssetValue, objChapterInfo.ColumnName);
                    if (arlPageDetails != null && arlPageDetails.Count > 0)
                    {
                        objChapterInfo.PageInfo = arlPageDetails;
                        objBookInfo.PageCount = arlPageDetails.Count;
                    }
                    arlChapterCollection.Add(objChapterInfo);
                }
                if (arlChapterCollection != null && arlChapterCollection.Count > 0)
                {
                    objBookInfo.Chapters = arlChapterCollection;
                }
                xmlWellBookDetails = objCommonBLL.CreateWellBookDetailXML(objBookInfo);
            }
            string strSiteURL = strParentSiteURL;

            strSiteURL = GetSslURL(strSiteURL);

            PDFServiceSPProxy objPDFService;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                objPDFService = new PDFServiceSPProxy();
                objPDFService.PreAuthenticate = true;
                SetUserNameDetailsforWebService();
                objPDFService.Credentials = new NetworkCredential(strWebServiceUserName, strWebServicePassword, strWebServiceDomain);
                PdfBLL objPdfBLL = new PdfBLL();
                objPdfBLL.PrintLog(strSiteURL, string.Format("WebService URL : {0}", objPDFService.Url), "Type I Page", "WebService URL");
                if (xmlWellBookDetails != null)
                {
                    string strBookName = xmlWellBookDetails.DocumentElement.Attributes["BookName"].Value.ToString();
                    string strRequestID = Guid.NewGuid().ToString();
                    string strDocumentURLTemp = PortalConfiguration.GetInstance().GetKey("DWBPrintNetworkPath") + string.Format("{0}_{1}", strBookName, strRequestID) + ".pdf";
                    UpdatePagePrintDetails(strRequestID, strDocumentURLTemp, strSiteURL, "temp");
                    intPrintedDocID = objPDFService.GeneratePDFDocument(xmlWellBookDetails.DocumentElement, strParentSiteURL, strRequestID);
                    strDocumentURL = strSiteURL + "/Pages/eWBPDFViewer.aspx?mode=page&requestID=" + strRequestID;
                }
            });
            return 1;
        }

        /// <summary>
        /// Updates the page print details.
        /// </summary>
        /// <param name="requestID">The request ID.</param>
        /// <param name="documentURL">The document URL.</param>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="userName">Name of the user.</param>
        private void UpdatePagePrintDetails(string requestID, string documentURL, string siteURL, string userName)
        {
            objCommonBLL = new CommonBLL();
            objCommonBLL.UpdateChapterPrintDetails(requestID, documentURL, siteURL, userName, "DWB Page Print Details");
        }

        private void SetUserNameDetailsforWebService()
        {
            strWebServiceUserName = PortalConfiguration.GetInstance().GetKey("DWBWebServiceUserName");
            strWebServicePassword = PortalConfiguration.GetInstance().GetKey("DWBWebServicePassword");
            strWebServiceDomain = PortalConfiguration.GetInstance().GetKey("DWBWebServiceDomain");
        }

        private bool IsHttps()
        {
            HttpContext context = HttpContext.Current;

            if (context != null)
            {
                if (context.Request.IsSecureConnection)
                {
                    return true;
                }
            }
            return false;

        }
        /// <summary>
        /// Determines if an SSL redirect is required.
        /// </summary>
        /// <param name="context">Context information representing an individual
        /// HTTP request.</param>
        /// <returns><c>true</c> if a redirect from HTTP to HTTPS is required,
        /// otherwise <c>false</c>.</returns>
        private string GetSslURL(string url)
        {
            string strSslUrl = SPContext.Current.Site.Url;
            if (IsHttps())
            {
                if (!string.IsNullOrEmpty(url))
                {
                    if (!url.Contains("https") || !url.Contains("HTTPS"))
                    {

                        if (url.Contains("http"))
                        {
                            strSslUrl = url.Replace("http", "https");
                        }
                        else if (url.Contains("HTTP"))
                        {
                            strSslUrl = url.Replace("HTTP", "HTTPS");
                        }
                    }
                    else
                    {
                        strSslUrl = url;
                    }
                }
                else
                {
                    HttpRequest request = HttpContext.Current.Request;

                    if (request != null)
                    {
                        strSslUrl = request.Url.AbsoluteUri.Substring(0, request.Url.AbsoluteUri.IndexOf("/Pages"));
                    }
                    if (strSslUrl.Contains("http"))
                    {
                        strSslUrl = strSslUrl.Replace("http", "https");
                    }
                    else if (strSslUrl.Contains("HTTP"))
                    {
                        strSslUrl = strSslUrl.Replace("HTTP", "HTTPS");
                    }

                }
            }
            else
            {
                strSslUrl = url;
            }
            return strSslUrl;
        }

        /// <summary>
        /// Gets the web app root.
        /// </summary>
        /// <returns></returns>
        private static string GetWebAppRoot()
        {
            string strHost = (HttpContext.Current.Request.Url.IsDefaultPort) ? HttpContext.Current.Request.Url.Host : HttpContext.Current.Request.Url.Authority;
            strHost = String.Format("{0}://{1}", HttpContext.Current.Request.Url.Scheme, strHost);
            if (HttpContext.Current.Request.ApplicationPath == "/")
                return strHost;
            else
                return strHost + HttpContext.Current.Request.ApplicationPath;
        }

        /// <summary>
        /// Loads the meta data controls.
        /// </summary>
        private void LoadMetaDataControls()
        {
            string strCamlQuery = string.Empty;
            DataTable dtBooks = null;
            strParentSiteURL = SPContext.Current.Site.Url.ToString();
            try
            {
                objCommonBLL = new CommonBLL();

                strCamlQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Number'>" +
                    PageID + "</Value></Eq></Where>";
                dtBooks = objCommonBLL.ReadList(strParentSiteURL, DWBCHAPTERPAGESMAPPINGLIST, strCamlQuery);
                if (dtBooks.Rows.Count > 0)
                {
                    CreateMetadataControls();
                    lblOwner.Text = dtBooks.Rows[0]["Owner"].ToString();
                    lblSignedOff.Text = dtBooks.Rows[0]["Sign_Off_Status"].ToString();
                    lblTitleTemplate.Text = dtBooks.Rows[0]["Page_Actual_Name"].ToString();
                    strPageOwner = dtBooks.Rows[0]["Owner"].ToString();
                    strDiscipline = dtBooks.Rows[0]["Discipline"].ToString();
                }
                ShowSignOff();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (dtBooks != null)
                {
                    dtBooks.Dispose();
                }
            }
        }

        /// <summary>
        /// Creates the metadata controls.
        /// </summary>
        private void CreateMetadataControls()
        {
            if (lblOwner == null)
            {
                lblOwner = new Label();
                lblOwner.ID = "lblOwner";
                lblOwner.CssClass = LABEL_CSS_CLASSNAME;
                lblOwner.Visible = true;
                this.Controls.Add(lblOwner);
            }
            if (lblSignedOff == null)
            {
                lblSignedOff = new Label();
                lblSignedOff.ID = "lblSignedOff";
                lblSignedOff.CssClass = LABEL_CSS_CLASSNAME;
                lblSignedOff.Visible = true;
                this.Controls.Add(lblSignedOff);
            }
            if (lblTitleTemplate == null)
            {
                lblTitleTemplate = new Label();
                lblTitleTemplate.ID = "lblTitleTemplate";
                lblTitleTemplate.CssClass = LABEL_CSS_CLASSNAME;
                lblTitleTemplate.Visible = true;
                this.Controls.Add(lblTitleTemplate);
            }

        }

        /// <summary>
        /// Creates the sign off.
        /// </summary>
        private void CreateSignOff()
        {
            if (btnSignOff == null)
            {
                btnSignOff = new Button();
                btnSignOff.ID = "btnSignOff";
                btnSignOff.CssClass = "button";
                if (string.Equals(lblSignedOff.Text, "No"))
                    btnSignOff.Text = SIGNOFF_TEXT;
                else
                    btnSignOff.Text = CANCEL_SIGNOFF_TEXT;
                btnSignOff.Click += new EventHandler(btnSignOff_Click);
                this.Controls.Add(btnSignOff);
            }
        }

        /// <summary>
        /// Shows the sign off.
        /// </summary>
        private void ShowSignOff()
        {
            string strMode = string.Empty;
            if (HttpContext.Current.Session[SESSION_TREEVIEWDATAOBJECT] != null)
            {
                objBookInfo = ((BookInfo)HttpContext.Current.Session[SESSION_TREEVIEWDATAOBJECT]);
                if (objBookInfo != null)
                {
                    strBookOwner = objBookInfo.BookOwner;
                    strBookTeamID = objBookInfo.BookTeamID;
                }
            }
            strMode = HttpContext.Current.Request.QueryString[QUERYSTRING_MODE];
            if (!string.IsNullOrEmpty(strMode) && strMode.Equals(VIEW))
            {
                blnSignOff = false;
            }
            else if (ShowButton(strBookOwner, strBookTeamID, strPageOwner, strDiscipline))
            {
                blnSignOff = true;
            }
            if (blnSignOff)
            {
                if (btnSignOff != null)
                {
                    if (string.Equals(lblSignedOff.Text, "No"))
                        btnSignOff.Text = SIGNOFF_TEXT;
                    else
                        btnSignOff.Text = CANCEL_SIGNOFF_TEXT;
                }
            }
        }

        /// <summary>
        /// Returns bool to indicate whether to show sign off button or not.
        /// </summary>
        /// <param name="bookOwner">The book owner.</param>
        /// <param name="bookTeamID">The book team ID.</param>
        /// <param name="pageOwner">The page owner.</param>
        /// <param name="pageDiscipline">The page discipline.</param>
        /// <returns>bool</returns>
        private bool ShowButton(string bookOwner, string bookTeamID, string pageOwner, string pageDiscipline)
        {
            bool blnShowButton = false;
            object objPrivileges = CommonUtility.GetSessionVariable(this.Page, enumSessionVariable.UserPrivileges.ToString());
            Shell.SharePoint.DWB.Business.DataObjects.Privileges objStoredPriviledges = null;
            string strUserName = objCommonUtility.GetUserName();
            if (objPrivileges != null)
            {
                objStoredPriviledges = (Shell.SharePoint.DWB.Business.DataObjects.Privileges)objPrivileges;
            }
            if (objStoredPriviledges != null)
            {
                if (objStoredPriviledges.IsNonDWBUser)
                {
                    blnShowButton = false;
                }
                else if (objStoredPriviledges.SystemPrivileges != null)
                {
                    if (objStoredPriviledges.SystemPrivileges.AdminPrivilege)
                    {
                        blnShowButton = true;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(pageOwner))
                        {
                            if (string.Compare(pageOwner, strUserName, true) == 0)
                            {
                                blnShowButton = true;
                            }
                        }
                        else if (!string.IsNullOrEmpty(bookOwner))
                        {
                            if (string.Compare(bookOwner, strUserName, true) == 0)
                            {
                                blnShowButton = true;
                            }
                        }
                        else
                        {
                            if (objStoredPriviledges.SystemPrivileges.BookOwner)
                            {
                                /// If user is member of the Team the Book belongs 
                                /// show signoff else hide                        
                                if (objStoredPriviledges.FocalPoint != null && !string.IsNullOrEmpty(bookTeamID))
                                {
                                    blnShowButton = IDExists(objStoredPriviledges.FocalPoint.TeamIDs, bookTeamID);
                                }
                            }
                            else if (objStoredPriviledges.SystemPrivileges.PageOwner)
                            {
                                /// If user is member of the Team the Book belongs 
                                /// and Page Discipline = User Discipline show signoff else hide       
                                bool blnTeamIDExists = false;
                                if (objStoredPriviledges.FocalPoint != null && !string.IsNullOrEmpty(bookTeamID))
                                {
                                    blnTeamIDExists = IDExists(objStoredPriviledges.FocalPoint.TeamIDs, bookTeamID);
                                    if (blnTeamIDExists)
                                    {
                                        /// If Page Discipline = User Discipline show signoff else hide 
                                        if (string.Compare(objStoredPriviledges.SystemPrivileges.Discipline, pageDiscipline, true) == 0)
                                        {
                                            blnShowButton = true;
                                        }
                                    }
                                }
                            }
                            else if (objStoredPriviledges.SystemPrivileges.DWBUser)
                            {
                                /// Show the Signoff button if user is owner of book
                                /// Or Page Owner
                                if (!string.IsNullOrEmpty(bookOwner))
                                {
                                    if (string.Compare(bookOwner, strUserName, true) == 0)
                                    {
                                        blnShowButton = true;
                                    }
                                }
                            }                            
                        }

                    }
                }
                else
                {
                    blnShowButton = false;
                }
            }

            return blnShowButton;
        }

        /// <summary>
        /// Returns Bool indicating team id or book id exists or not
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="IdToBeFound">The id to be found.</param>
        /// <returns>bool</returns>
        private static bool IDExists(string source, string IdToBeFound)
        {
            bool blnExists = false;
            List<string> strTeamIds = new List<string>();
            if (!string.IsNullOrEmpty(source))
            {
                string[] strTempTeamIds = source.Split("|".ToCharArray());
                for (int intIndex = 0; intIndex < strTempTeamIds.Length; intIndex++)
                {
                    strTeamIds.Add(strTempTeamIds[intIndex]);
                }
            }
            if (strTeamIds.Contains(IdToBeFound))
            {
                blnExists = true;
            }
            return blnExists;
        }

        /// <summary>
        /// Creates the print page button.
        /// </summary>
        private void CreatePrintPage()
        {
            btnPrintPage = new Button();
            btnPrintPage.ID = "btnPrint";
            btnPrintPage.CssClass = "button";
            btnPrintPage.Text = "Print this Page";
            btnPrintPage.OnClientClick = this.RegisterJavaScript();
            btnPrintPage.Click += new EventHandler(btnPrintPage_Click);            
            this.Controls.Add(btnPrintPage);
        }

        /// <summary>
        /// Returns the java script to open Print Option Page.
        /// </summary>
        /// <returns></returns>
        private string RegisterJavaScript()
        {
            string strJavaScriptMethod = "return openStoryBoardConfirmation(null);";
            bool blnUserControlFound = false;
            Telerik.Web.UI.RadTreeView trvWellBook = null;
            if (this.Parent.GetType().Equals(typeof(SPWebPartManager)))
            {
                foreach (Control ctrlWebPart in this.Parent.Controls)
                {
                    if (ctrlWebPart.GetType().Equals(typeof(TreeViewControl.TreeViewControl)))
                    {

                        Control ctrlTreeView = ctrlWebPart.FindControl("RadTreeView1");
                        if (ctrlTreeView != null && ctrlTreeView.GetType().Equals(typeof(RadTreeView)))
                        {
                            trvWellBook = (RadTreeView)ctrlWebPart.FindControl("RadTreeView1");
                            blnUserControlFound = true;
                            break;
                        }
                        if (blnUserControlFound)
                        {
                            break;
                        }
                    }
                }
                if (blnUserControlFound)
                {
                    strJavaScriptMethod = "return openStoryBoardConfirmation('" + trvWellBook.ClientID + "');";
                }
            }            
            return strJavaScriptMethod;
        }

#endregion
    }
}
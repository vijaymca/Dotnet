#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: ContextSearchResults.cs
#endregion
/// <summary> 
/// This is ContextSearchResults webpart class for all context searches
/// </summary>
using System;
using System.ComponentModel;
using System.Web;
using System.Net;
using System.Xml;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DREAM.SearchHelper;
using Shell.SharePoint.DREAM.Site.UI;
using Shell.SharePoint.DREAM.Business.Entities;


namespace Shell.SharePoint.WebParts.DREAM.ContextSearch
{
    /// <summary> 
    /// This is ContextSearchResults webpart class for all context searches,by configuring different 
    /// properties we can use same webpart for all context searches
    /// </summary>
    public partial class ContextSearchResults :SearchHelper
    {

        #region Enum Classes
        /// <summary>
        /// webservice name
        /// </summary>
        public enum ServiceNameEnum
        {
            ReportService,
            EventService
        }
        /// <summary>
        /// xsl filenames
        /// </summary>
        public enum XSLFileEnum
        {
            [Description("Tabular Results")]
            TabularResults,

            [Description("TimeDepthDetail Tabular")]
            TimeDepthDetail,

            [Description("DirectionalSurveyDetail Tabular")]
            DirectionalSurveyDetail,

            [Description("WellboreHeader Datasheet")]
            WellboreHeader_Datasheet,

            [Description("EPCatalog Results")]
            EPCatalog_Results,

            [Description("Dream EPCatalog Results")]
            DreamEPCatalog_Results,

            [Description("PARSDetail Datasheet")]
            PARSDetail_Datasheet,

            [Description("WellHistory Datasheet")]
            WellHistory,

            [Description("RecallLogs Results")]
            RecallLogsResults,

            [Description("EDMTabular Results")]
            EDMTabularResults,

            [Description("EDMHierarchical Results")]
            EDMHierarchicalResults,

            [Description("MechanicalData Tabular")]
            MechanicalData_Tabular,

            [Description("MechanicalData Datasheet")]
            MechanicalData_Datasheet,

            [Description("Tab Tabular Results")]
            TabTabularResults,

            [Description("WellTestData Tabular")]
            WellTestReport_Tabular,

            [Description("WellTestData Datasheet")]
            WellTestReport_DataSheet,

            [Description("PressureSurvey Tabular")]
            PressureSurvey_Tabular,
            //Dream 4.0 
            //start
            /// <summary>
            /// WellSummary Datasheet
            /// </summary>
            [Description("Dream WellSummary")]
            WellSummary,
            [Description("Significant Well Events")]
            SignificantWellEvents
            //end
        }
        /// <summary>
        /// ViewModeEnum
        /// </summary>
        public enum ViewModeEnum
        {
            Tabular,
            DataSheet,
            TabularDataSheet
        }
        /// <summary>
        /// ReportLevelEnum
        /// </summary>
        public enum ReportLevelEnum
        {
            First,
            Second
        }
        /// <summary>
        /// Context search names
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

            [Description("EP Catalog Search Results")]
            EPCatalog,

            [Description("EP WellFile Report")]
            iWellFile,

            [Description("Well History Report")]
            WellHistory,

            [Description("Project Archives Detail Report")]
            PARSDetail,

            [Description("Pre production RFT Data")]
            PreProdRFT,

            [Description("Daily Wells Reporting")]
            EDMReport,

            [Description("Mechanical Data Report")]
            MechanicalData,

            [Description("Paleo/Markers")]
            PaleoMarkers,
            #region DREAM 3.0

            [Description("Perforations (Sandface Completion) Report")]
            Perforations,

            [Description("Well Test Report")]
            WellTestReport,

            /// ====================================
            /// Module Name:Zone Properties
            /// Description: Zone Properties Search Type
            /// Date:21-July-2010
            /// Modified Lines:264-275
            /// ====================================
            [Description("Zone Properties")]
            ZoneProperties,

            [Description("EP Catalog Report")]
            PVTReport,
            [Description("Pressure Survey Data")]
            PressureSurveyData,
            [Description("Geopressure")]
            Geopressure,
            #endregion DREAM 3.0

            #region SRP
            /// ====================================
            /// Module Name:Field Header
            /// Description: Field Header Search Type
            /// Date:3-Sep-2010
            /// Modified Lines:276-
            /// ====================================
            [Description("Field Header")]
            FieldHeader,

            [Description("Reservoir Header")]
            ReservoirHeader,
            #endregion SRP

            #region Dream 4.0
            /// ====================================
            /// Module Name:Well Summary Report
            /// Description: Well Summary Report Search Type
            /// Date:12-Mar-2011
            /// Modified Lines:12
            /// ====================================
            [Description("Well Summary Report")]
            WellSummary,
            [Description("Significant Well Events")]
            /// ====================================
            /// Module Name:Well Reviews Report
            /// Description: Well Reviews Report Search Type
            /// Date:15-Mar-2011
            /// Modified Lines:12
            /// ====================================
            SignificantWellEvents,
            [Description("Well Reviews")]
            WellReviews,
            [Description("Interpolated Positional Logs")]
            InterpolatedLogs,
            [Description("Position Log Data")]
            PositionLogData,

            #endregion
        }
        #endregion

        #region WebPartProperties varaible declaration

        ServiceNameEnum objServiceName;
        XSLFileEnum objXSLFileName;
        ViewModeEnum objViewMode;
        ReportLevelEnum objReportLevel;
        SearchNameEnum objSearchName;
        bool blnIsPrintApplicable;
        bool blnIsExportPage;
        bool blnIsExportAll;
        bool blnIsDetailReportAvailable;
        bool blnIsDataSourceSelectionAvailable;
        bool bldDepthReference;
        /// <summary>
        ///  DREAM 3.0 Changes
        /// </summary>
        bool blnReorderColumn = true;
        bool blnIsUnitConversion;
        bool blnTemperatureUnit;
        bool blnPressureUnit;
        bool blnEnableSkipCount = true;
        #region DREAM 4.0
        string strUserControl = string.Empty;
        UIControlHandler objFilterControl = null;
        bool blnSearchWithFilterCriteria;
        bool blnDisplaySingleAsset;
        #endregion
        #endregion

        #region WebPart Properties
        #region PrintOption
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDescription("Check if Print Option is applicable.")]
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
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDisplayNameAttribute("Export Page")]
        [WebDescription("Check if Export Page option is applicable.")]
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
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDisplayNameAttribute("Unit Conversion")]
        [WebDescription("Check if Unit Conversion is applicable.")]
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

        #region DataSourceSelection
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDisplayNameAttribute("Data Source Selection")]
        [WebDescription("Check if Data Source selection is applicable.")]
        public bool DataSourceSelection
        {
            get
            {
                return blnIsDataSourceSelectionAvailable;
            }
            set
            {
                blnIsDataSourceSelectionAvailable = value;
            }
        }
        #endregion
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

        #region DepthReference
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDisplayNameAttribute("Depth Reference")]
        [WebDescription("Check if Depth Reference calculation is applicable.")]
        public bool DepthReference
        {
            get
            {
                return bldDepthReference;
            }
            set
            {
                bldDepthReference = value;
            }
        }
        #endregion

        #region ExportAll
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDisplayNameAttribute("Export All Records")]
        [WebDescription("check if Export All option is applicable.")]
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

        #region DetailReport
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDescription("Check if Detail Report is available.")]
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
        //Dream 3.0 code
        #region Temperature Unit
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDisplayNameAttribute("Temperature Unit")]
        [WebDescription("check/uncheck to allow conversion between temperature units.")]
        public bool TemperatureUnit
        {
            get
            {
                return blnTemperatureUnit;
            }
            set
            {
                blnTemperatureUnit = value;
            }
        }
        #endregion

        #region Pressure Unit
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDisplayNameAttribute("Pressure Unit")]
        [WebDescription("check/uncheck to allow conversion between Pressure units.")]
        public bool PressureUnit
        {
            get
            {
                return blnPressureUnit;
            }
            set
            {
                blnPressureUnit = value;
            }
        }
        #endregion

        #region Enable Skip Count
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDisplayNameAttribute("Enable Skip Count")]
        [WebDescription("check/uncheck to enable/disable skip count for the report.")]
        public bool EnableSkipCount
        {
            get
            {
                return blnEnableSkipCount;
            }
            set
            {
                blnEnableSkipCount = value;
            }
        }
        #endregion

        #region ReportLevel
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDisplayNameAttribute("Report Level")]
        [WebDescription("Select the Report Level.")]
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
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDisplayNameAttribute("Service Name")]
        [WebDescription("Select the Service Name.")]
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
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDisplayNameAttribute("View Mode")]
        [WebDescription("Select the View Mode.")]
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

        #region DREAM 4.0

        #region SearchWithFilterCriteria
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDisplayNameAttribute("Search With Filter Criteria")]
        [WebDescription("Check if search is having filter criteria otherwise uncheck it.")]
        [DefaultValue(false)]
        public bool SearchWithFilterCriteria
        {
            get
            {
                return blnSearchWithFilterCriteria;
            }
            set
            {
                blnSearchWithFilterCriteria = value;
            }
        }
        #endregion

        #region Filter criteria usercontrol
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDisplayNameAttribute("User Control (.ascx)")]
        [WebDescription("Path to the User Control (.ascx)")]
        [DefaultValue("")]
        public string UserControl
        {
            get
            {
                return strUserControl;
            }
            set
            {
                strUserControl = value;
            }
        }
        #endregion

        #region DisplaySingleAsset
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDisplayNameAttribute("Display Single Asset")]
        [WebDescription("Check if search dispalys single asset at a time otherwise uncheck it.")]
        [DefaultValue(false)]
        public bool DisplaySingleAsset
        {
            get
            {
                return blnDisplaySingleAsset;
            }
            set
            {
                blnDisplaySingleAsset = value;
            }
        }
        #endregion
        #endregion
        #endregion

        #region Protected Methods
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            objCommonUtility.RenderBusyBox(this.Page); //rendering busy box.
        }
        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            try
            {
                base.CreateChildControls();
                //Fix for the UpdatePanel postback behaviour.
                objCommonUtility.EnsurePanelFix(this.Page, typeof(ContextSearchResults));
                strCurrSiteUrl = HttpContext.Current.Request.Url.ToString();
                CreateBaseClassObjects();
                CreateMessageLabel();
                CreateHyperLinks(GetEnumDescription(SearchName));
                CreateHiddenControl();
                CreateRadioControls(ViewMode.ToString());
                //Calling method to preccess external request if any(request from enternal URL)
                if(IsExternalReuest())
                {
                    blnSuccessFull = ProcessExternalRequest();
                }
                else
                {
                    //When its external request dont set hidden field,i,e in case ofurl based integration
                    SetHiddenFieldValues();
                }
                //This hidden field should be set always
                hidSearchType.Value = SearchName.ToString().ToLowerInvariant();
                #region DREAM 4.0
                if(SearchWithFilterCriteria)
                {
                    if(!string.IsNullOrEmpty(UserControl))
                    {

                        objFilterControl = (UIControlHandler)this.Page.LoadControl(UserControl);
                        objFilterControl.SearchName = SearchName.ToString();
                        objFilterControl.SelectedRows = hidSelectedRows.Value;
                        objFilterControl.SelectedCriteriaName = hidSelectedCriteriaName.Value;
                        objFilterControl.SelectedAssetNames = hidSelectedAssetNames.Value;
                        if(objFilterControl != null)
                        {
                            // Add to the Controls collection to support postback
                            this.Controls.Add(objFilterControl);
                        }
                    }
                }
                else if(DisplaySingleAsset)
                {
                    LoadAssets();
                }

                #endregion
            }
            catch(Exception Ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, Ex);
            }
        }
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            string strControlID = string.Empty;
            if(string.Equals(SearchName.ToString(), PICKSDETAIL))
            {
                strControlID = string.Format("{0}{1}", this.UniqueID, FILTERDROPDOWN);
                /// Gets the filter dropdown selected Value.
                if(HttpContext.Current.Request.Form[strControlID] != null)
                {
                    strPicksFilter = HttpContext.Current.Request.Form[strControlID];
                }
            }
        }
        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            base.EnsureChildControls();

            if((Page.Request.Params.Get(EVENTTARGET) != null) && (Page.Request.Params.Get(EVENTTARGET).ToLowerInvariant().Contains(UPDATEPANELID)))
            {
                hstblParams = objCommonUtility.GetPagingSortingParams(Page.Request.Params.Get(EVENTARGUMENT));
            }
            try
            {

                #region DREAM 4.0
                RenderParentTable(writer, GetEnumDescription(SearchName));
                //Rendering filter criteria usercontrol
                RenderFilterUserControl(writer);
                #endregion
                /// If external request is invalid; then throw exception
                if(IsExternalReuest() && !blnSuccessFull)
                {
                    throw new Exception("Invalid Request");
                }
                InitializeUserPreference();
                //Dream 3.1 code start
                SetControlValuesToUSerPreferences();
                AttachDepthRefOnChangeEventHndlr();
                //Dream 3.1 code ends
                RenderPage(writer);
            }
            catch(SoapException soapEx)
            {
                if(!string.Equals(soapEx.Message.ToString(), NORECORDSFOUND))
                {
                    CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                }
                ///* Rendering hiddenfield for reorder in  case of saop exception to main user preference
                if(ReorderColumn)
                {
                    hidReorderColValue.RenderControl(writer);
                    hidReorderSourceId.RenderControl(writer);
                }
                RenderExceptionMessage(writer, soapEx.Message);
            }
            catch(WebException webEx)
            {
                CommonUtility.HandleException(strCurrSiteUrl, webEx, 1);
                RenderExceptionMessage(writer, webEx.Message);
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
                RenderExceptionMessage(writer, ex.Message);
            }
            finally
            {
                objCommonUtility.CloseBusyBox(this.Page);
                writer.Write("<script language=\"javascript\">setWindowTitle('" + GetEnumDescription(SearchName) + "');</script>");
                RegisterClientSideScript();
            }
        }

        #endregion

        #region Private methods

        #region Render Method
        /// <summary>
        /// Renders the page.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderPage(HtmlTextWriter writer)
        {
            if((ReorderColumn) && (string.Equals(ViewMode.ToString(), TABULARDATASHEET)))
            {
                if(string.Equals(strDisplayType.ToUpperInvariant(), DATASHEET))//in datasheet mode reorder should be disabled 
                {
                    ReorderColumn = false;
                    if(hidReorderColValue != null)
                        hidReorderColValue.RenderControl(writer);
                    if(hidReorderSourceId != null)
                        hidReorderSourceId.RenderControl(writer);
                }
                else
                {
                    ReorderColumn = true;
                }
            }
            hidMaxRecord.Value = intMaxRecords.ToString();
            hidDateFormat.Value = PortalConfiguration.GetInstance().GetKey(DATEFORMAT);
            objRequestInfo = new RequestInfo();
            EnableDisableExport(SearchName.ToString());
            if((DepthReference) && (!SearchName.ToString().ToLowerInvariant().Equals(MECHANICALDATA)))
            {
                LoadDepthRef();
            }
            else if(SearchName.ToString().ToLowerInvariant().Equals(MECHANICALDATA))
            {
                PopulateDepthRef();
            }
            RenderHiddenControls(writer);
            SetActiveDiv();

            #region GET RESULT XML
            base.ResponseType = ViewMode.ToString();
            /// set entity name
            base.EntityName = SearchName.ToString();
            #region DREAM 4.0 Pagination related changes
            //Start Pagination related changes
            if(Page.Request.QueryString.Count <= 0)//opening page directly to add webpart
            {
                xmlDocSearchResult = null;
            }
            else if(SearchWithFilterCriteria)//Added in DREAM 4.0 for search filter criteria
            {
                GetSearchWithFilterCriteriaResponse();
            }
            else if(DisplaySingleAsset)
            {
                GetSingleAssetReportReponse();
            }
            else
            {
                #region Getting response in general scenario
                //In following cases:
                //*1.First time load from quick search 
                //*2.Datapreferences switch
                //*3.Datasource source switch
                //*4.AH/TVD switch
                //*5.Tabular/datasheet switch
                //*6.Paging sorting postback &&//Reorder save and apply
                GetGeneralResponse();
                #endregion
            }
            //End of Pagination related changes
            #endregion
            #endregion

            #region RenderResults
            if(xmlDocSearchResult != null)
            {
                RenderResults(writer);
            }
            else
            {
                RenderExceptionMessage(writer, ((MOSSServiceManager)objMossController).GetCustomExceptionMessage(strCurrSiteUrl, "1"));
            }
            #endregion
        }

        /// <summary>
        /// Renders the results.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderResults(HtmlTextWriter writer)
        {
            //Dream 4.0 code start
            objReorder = new Reorder();
            ///* reorder column
            if(ReorderColumn)
            {
                /// function for reordering of column
                //Dream 4.0 Pagination changes on reorder
                #region Dream 4.0
                objReorder.ManageReorder(SearchName.ToString(), hidAssetName.Value, xmlDocSearchResult, hidReorderColValue, hidReorderSourceId);
                hidReorderColValue.RenderControl(writer);
                hidReorderSourceId.RenderControl(writer);
                xmlNdLstReorderColumn = objReorder.GetColumnNodeList(SearchName.ToString(), hidAssetName.Value, xmlDocSearchResult);
                strReorderDivHTML = objReorder.GetReorderDivHTML(xmlNdLstReorderColumn);
                objReorder.SetColNameDisplayStatus(out strColNames, out strColDisplayStatus, xmlNdLstReorderColumn);
                #endregion
            }
            //Added in DREAM 4.0 for Checkbox state management
            //this case is for wellbore header report when you switch from datasheet to tabular
            if(string.Equals(ViewMode.ToString(), TABULARDATASHEET))
            {
                if(!strDisplayType.ToUpperInvariant().Equals(DATASHEET) && string.IsNullOrEmpty(hidColSelectedCheckBoxes.Value))
                {
                    objReorder.SetColNameDisplayStatus(out strChkbxColNames, out strChkbxColDisplayStatus, xmlDocSearchResult.SelectNodes(ATTRIBUTEXPATHWITHDISPLAYTRUE));
                    hidColSelectedCheckBoxes.Value = strChkbxColNames;
                }
            }
            else if(!SearchName.ToString().ToLowerInvariant().Equals(EPCATALOG) && !SearchName.ToString().ToLowerInvariant().Equals(PVTREPORT))
            {
                SetColumnNamesHiddenField();

            }
            hidColSelectedCheckBoxes.RenderControl(writer);
            //end
            //** End
            int intTotalRecordCount = 0;
            GetRequestIDFromResult();
            hidRequestID.RenderControl(writer);
            //DREAM4.0 RenderParentTable(writer, GetEnumDescription(SearchName));
            /// Added new condition, If there is no result then the print option will not show
            intTotalRecordCount = GetRecordCountFromResult(xmlDocSearchResult, SearchName.ToString());
            /// in case of well history render print export since its reponse does not contain record count
            if((intTotalRecordCount > 0) || (SearchName.ToString().ToLowerInvariant().Equals(WELLHISTORY)))
            {
                RenderPrintExport(writer);
            }
            RenderResultTable(writer);
            //Added ni DREAM 4.0 for checkbox state management across postback
            objCommonUtility.RegisterOnLoadClientScript(this.Page, "SelectSelectAllCheckBox('chkbxRowSelectAll','tblSearchResults','tbody','chkbxRow');SelectSelectAllCheckBox('chkbxColumnSelectAll','tblSearchResults','thead','chkbxColumn');");
        }
        /// <summary>
        /// Renders the results.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderResultTable(HtmlTextWriter writer)
        {
            XmlTextReader xmlTextReader = null;
            blnSkipCountEnabled = EnableSkipCount;
            //**Commented in Dream 3.1
            // SetControlValuesToUSerPreferences();
            RenderFeetMeter(writer, strDepthUnit);
            #region PICKS FILTER
            if(string.Equals(SearchName.ToString(), PICKSDETAIL))
            {
                /// Rendering the Picks dropdown.
                writer.Write("<tr><td colspan=\"4\" class=\"tdAdvSrchItem\"><b>Geologic Feature: <b/>");
                if((string.IsNullOrEmpty(Page.Request.Params.Get(EVENTTARGET))) || (IsPostBackByControl(rdoDataSource)) || (IsPostBackByControl(rdoLengthType)))
                {
                    LoadPicksDropDown(cboFilter, xmlDocSearchResult);
                }
                else
                {
                    LoadPicksDropDownFromSession(cboFilter);
                }
                if(hstblParams != null || (IsPostBackByControl(cboFilter)))
                {
                    cboFilter.SelectedValue = strPicksFilter;
                }
                cboFilter.RenderControl(writer);
                writer.Write("</td></tr>");
            }
            /// Rendering data preference dropdown
            RenderDatapreferenceDDL(writer, false);
            #endregion
            if(!string.Equals(ViewMode.ToString(), TABULARDATASHEET))
            {
                xmlTextReader = ((MOSSServiceManager)objMossController).GetXSLTemplate(GetEnumDescription(XSLFileName), strCurrSiteUrl);
            }
            else
            {
                if(strDisplayType.ToLowerInvariant().Equals(TABULAR.ToLowerInvariant()))
                {
                    xmlTextReader = ((MOSSServiceManager)objMossController).GetXSLTemplate(GetEnumDescription(XSLFileName), strCurrSiteUrl);
                }
                else
                    if(string.Equals(SearchName.ToString(), WELLBOREHEADER) || string.Equals(SearchName.ToString(), FIELDHEADER) || string.Equals(SearchName.ToString(), RESERVOIRHEADER))
                        xmlTextReader = ((MOSSServiceManager)objMossController).GetXSLTemplate(GetEnumDescription(XSLFileEnum.WellboreHeader_Datasheet), strCurrSiteUrl);
                    else if(string.Equals(SearchName.ToString(), PARSDETAIL))
                        xmlTextReader = ((MOSSServiceManager)objMossController).GetXSLTemplate(GetEnumDescription(XSLFileEnum.PARSDetail_Datasheet), strCurrSiteUrl);

                    else if(string.Equals(SearchName.ToString().ToLowerInvariant(), MECHANICALDATA))
                    {
                        xmlTextReader = ((MOSSServiceManager)objMossController).GetXSLTemplate(GetEnumDescription(XSLFileEnum.MechanicalData_Datasheet), strCurrSiteUrl);
                    }
                    /// ====================================
                    /// Module Name:Well Test Data
                    /// Description: Get Well Test Data XSL Reader
                    /// Date:16-July-2010
                    /// Modified Lines:1269-1272
                    /// ====================================
                    else if(string.Equals(SearchName.ToString().ToLowerInvariant(), WELLTESTREPORT))
                    {
                        xmlTextReader = ((MOSSServiceManager)objMossController).GetXSLTemplate(GetEnumDescription(XSLFileEnum.WellTestReport_DataSheet), strCurrSiteUrl);
                    }
                    else
                        xmlTextReader = ((MOSSServiceManager)objMossController).GetXSLTemplate(GetEnumDescription(XSLFileName), strCurrSiteUrl);
            }
            writer.Write("<tr><td colspan=\"4\">");

            #region Dream 4.0 code
            //Dream 4.0 pagination and ajax changes
            SetPagingParameter();
            if(string.Equals(SearchName.ToString().ToLowerInvariant(), EPCATALOG) || string.Equals(SearchName.ToString().ToLowerInvariant(), PVTREPORT))
            {
                TransformEPCatalogXmlToResultTable(xmlDocSearchResult, xmlTextReader, strPageNumber, strSortColumn, strSortOrder, SearchName.ToString());
            }
            else
            {
                TransformXmlToResultTable(xmlDocSearchResult, xmlTextReader, strPageNumber, strSortColumn, strSortOrder, SearchName.ToString(), strActiveDiv);
                //end
            }
            writer.Write(strResultTable.ToString());
            #endregion
            if(intDetailRecordCount != 0)
            {
                hidRowsCount.Value = intDetailRecordCount.ToString();
                hidRowsCount.RenderControl(writer);
            }
            writer.Write("</td></tr></table>");

            if(xmlTextReader != null)
                xmlTextReader.Close();

        }
        /// <summary>
        /// Renders the feet meter.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="depthUnit">The depth unit.</param>
        private void RenderFeetMeter(HtmlTextWriter writer, string depthUnit)
        {
            ///Added for Feet Metre Fix By Manoj.
            hidDepthUnit.Value = depthUnit;
            hidDepthUnit.RenderControl(writer);
            if(string.Equals(ViewMode.ToString(), TABULARDATASHEET))
            {
                if(UnitConversion)
                {
                    writer.Write("<tr><td colspan=\"2\" align=\"left\" class=\"tdAdvSrchItem\"><b>Depth Units: </b>");
                    if(string.Equals(depthUnit, FEET))
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
                    writer.Write("</td><td colspan=\"2\" align=\"right\" class=\"tdAdvSrchItem\"><b>View: </b>");
                    RenderViewMode(writer);
                }
                else
                {
                    writer.Write("<tr><td colspan=\"4\" align=\"right\" class=\"tdAdvSrchItem\"><b>View: </b>");
                    RenderViewMode(writer);
                }

            }
            else
            {
                RenderFeetMeterInTabularMode(writer, depthUnit);
            }
            /// Rendering Mechanical data report psecific criteria   
            if(DepthReference)
            {
                writer.Write("</td></tr>");
                writer.Write("<tr><td colspan=\"2\" align=\"left\" class=\"tdAdvSrchItem\"><b>Depth Type: </b>");
                rdoLengthType.RenderControl(writer);
                writer.Write("</td><td colspan=\"2\" align=\"right\" class=\"tdAdvSrchItem\"><b>Depth Reference: </b>");
                cboDepthRef.RenderControl(writer);
            }
            writer.Write("</td></tr>");
        }

        /// <summary>
        /// Renders the feet meter in tabular mode.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="depthUnit">The depth unit.</param>
        private void RenderFeetMeterInTabularMode(HtmlTextWriter writer, string depthUnit)
        {
            if(UnitConversion)
            {
                #region Selecting Depth unit
                if(string.Equals(depthUnit, FEET))
                {
                    rbFeet.Checked = true;
                    rbMeters.Checked = false;
                }
                else
                {
                    rbMeters.Checked = true;
                    rbFeet.Checked = false;
                }
                #endregion
                if(DetailReport)
                {
                    writer.Write("<tr><td colspan=\"4\" align=\"left\" class=\"tdAdvSrchItem\">");
                    writer.Write("<table><tr><td><b>Depth Units: </b></td><td>");
                    rbFeet.RenderControl(writer);
                    rbMeters.RenderControl(writer);
                    writer.Write("</td></tr>");
                    writer.Write("</table>");
                    // writer.Write("</td><td colspan=\"3\" align=\"right\" valign=\"top\" class=\"tdAdvSrchItem\">");//DREAM4.0 Code changes for Relocate detail report links
                    /// added for external recall log report
                    if(SearchName.ToString().ToLowerInvariant().Equals("recalllogs"))
                    {
                        CreateExternalRecallLogLink(GetEnumDescription(SearchName));
                        //linkExternalRecallLogReport.RenderControl(writer);//DREAM4.0 Code changes
                        writer.Write("<tr><td colspan=\"4\" align=\"left\" valign=\"top\" class=\"tdAdvSrchItem\">");//DREAM 4.0 Code added for Relocate detail report links
                        linkViewDetailReport.RenderControl(writer);//DREAM 4.0 Code added for Relocate detail report links
                        writer.Write("&nbsp;&nbsp;");
                    }
                    //linkViewDetailReport.RenderControl(writer);//DREAM4.0 Code changes for Relocate detail report links
                }
                else
                {
                    if(SearchName.ToString().ToLowerInvariant().Equals(RECALLCURVE))
                    {
                        writer.Write("<tr><td colspan=\"4\" align=\"left\" class=\"tdAdvSrchItem\"><b>Depth Units: </b>");
                        rbFeet.RenderControl(writer);
                        rbMeters.RenderControl(writer);
                       // writer.Write("</td><td colspan=\"3\" align=\"right\" class=\"tdAdvSrchItem\">");
                        /// added for external recall log report                          
                        CreateExternalRecallLogLink(GetEnumDescription(SearchName));
                        //linkExternalRecallLogReport.RenderControl(writer);//DREAM4.0 Code changes for Relocate detail report links
                    }
                    else if(TemperatureUnit || PressureUnit)
                    {
                        writer.Write("<tr><td colspan=\"4\" align=\"left\" class=\"tdAdvSrchItem\">");
                        writer.Write("<table width=\"100%\"><tr><td><fieldset class=\"classFieldset\"><legend class=\"classLegend\">Depth Units:</legend>");
                        rbFeet.RenderControl(writer);
                        rbMeters.RenderControl(writer);
                        writer.Write("</fieldset></td>");
                        //Added for pressure survey data and Geopressure report
                        if(TemperatureUnit)
                        {
                            writer.Write("<td><fieldset class=\"classFieldset\"><legend class=\"classLegend\">Temperature Units:</legend>");
                            rdoTemperatureUnit.RenderControl(writer);
                            writer.Write("</fieldset></td>");
                        }
                        if(PressureUnit)
                        {
                            rdoPressureUnit.Attributes.Add("previousSelectedValue", rdoPressureUnit.SelectedItem.Value);
                            writer.Write("<td><fieldset class=\"classFieldset\"><legend class=\"classLegend\">Pressure Units:</legend>");
                            rdoPressureUnit.RenderControl(writer);
                            writer.Write("</fieldset></td></tr>");
                        }
                        writer.Write("</table>");

                    }
                    else
                    {
                        writer.Write("<tr><td colspan=\"4\" align=\"left\" class=\"tdAdvSrchItem\">");
                        writer.Write("<table><tr><td><b>Depth Units: </b></td><td>");
                        rbFeet.RenderControl(writer);
                        rbMeters.RenderControl(writer);
                        writer.Write("</td></tr>");
                        writer.Write("</table>");
                    }
                }
                writer.Write("</td></tr>");
            }
        }

        /// <summary>
        /// Renders the print export.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderPrintExport(HtmlTextWriter writer)
        {
            if(Print || ExportPage || ExportAll || DataSourceSelection || string.Equals(SearchName.ToString(), PARSDETAIL))
            {
                if((DataSourceSelection) && (!objCommonUtility.GetUserTeamID().Equals("0")))
                {
                    writer.Write("<tr><td class=\"tdAdvSrchItem\" colspan=\"2\">");
                    writer.Write("<b>Data Source: </b>");
                    rdoDataSource.RenderControl(writer);
                    writer.Write("</td><td class=\"tdAdvSrchItem\" colspan=\"2\" align=\"right\" >");
                }
                //Added in DREAM 4.0 for single selection restriction
                else if(DisplaySingleAsset && !SearchWithFilterCriteria)
                {
                    writer.Write("<tr><td class=\"tdAdvSrchItem\" colspan=\"2\">");
                    writer.Write("<b>Wellbore: </b>");
                    ddlAssets.RenderControl(writer);
                    writer.Write("</td><td class=\"tdAdvSrchItem\" colspan=\"2\" align=\"right\" >");
                }
                else
                {
                    writer.Write("<tr><td class=\"tdAdvSrchItem\" colspan=\"4\" align=\"right\" >");
                }
                if(Print)
                {
                    writer.Write("Print");
                    writer.Write("&nbsp;");
                    lnkPrint.RenderControl(writer);
                }
                #region DREAM 4.0 Export Options
                if(ExportPage || ExportAll)
                {
                    writer.Write("&nbsp;");
                    writer.Write("Export");
                    writer.Write("&nbsp;");
                    writer.Write("<input type=\"image\" class=\"buttonAdvSrch\" src=\"/_layouts/DREAM/images/icon_Excel.gif\"  id=\"btnShowExportOptionDiv\" onclick=\"SetExportOptionDefaults();return pop('divExportOptions')\" />");
                    writer.Write(GetExportOptionsDivHTML(SearchName.ToString(), ExportPage, ExportAll, true, ExportAll));
                    //Export csv is false when export all is flase
                }
                #endregion DREAM 4.0 Export Options
                if(string.Equals(SearchName.ToString(), PARSDETAIL))
                {
                    lnkiRequest.Visible = true;
                    writer.Write("&nbsp;");
                    writer.Write("Archive Restore");
                    writer.Write("&nbsp;");
                    lnkiRequest.RenderControl(writer);
                }
                writer.Write("&nbsp;</td></tr>");
            }

        }

        /// <summary>
        /// Renders the exception message.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="message">The message.</param>
        private void RenderExceptionMessage(HtmlTextWriter writer, string message)
        {
            //**DREAM4.0RenderParentTable(writer, GetEnumDescription(SearchName));
            /// rendering data preferences dropdown
            RenderDatapreferenceDDL(writer, true);

            writer.Write("<tr><td colspan=\"4\" style=\"white-space:normal;\"><br/>");
            lblMessage.Visible = true;
            lblMessage.Text = message;
            lblMessage.RenderControl(writer);
            writer.Write("</td></tr></table>");
            /// sets the window title based on search name.
            writer.Write("<Script language=\"javascript\">setWindowTitle('" + GetEnumDescription(SearchName) + "');</Script>");
        }

        /// <summary>
        /// Renders the parent table.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="searchName">Name of the search.</param>
        private void RenderParentTable(HtmlTextWriter writer, string searchName)
        {
            /// Renders the Parent table.
            writer.Write("<table class=\"tableAdvSrchBorder\" cellspacing=\"0\" cellpadding=\"4\" border=\"0\" width=\"100%\">");
            writer.Write("<tr><td class=\"tdAdvSrchHeader\" colspan=\"4\" valign=\"top\"><B>" + searchName + "</b></td></tr>");
        }

        /// <summary>
        /// Renders the hidden controls.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderHiddenControls(HtmlTextWriter writer)
        {
            hidSelectedCriteriaName.RenderControl(writer);
            hidReportSelectRow.RenderControl(writer);
            hidSelectedRows.RenderControl(writer);
            hidMaxRecord.RenderControl(writer);
            hidSelectedColumns.RenderControl(writer);
            hidSearchType.RenderControl(writer);
            hidDrpValue.RenderControl(writer);
            hidDateFormat.RenderControl(writer);
            hidAssetName.RenderControl(writer);

            #region Rendering Recallogs HiddenFields
            if(string.Equals(SearchName.ToString(), RECALLLOGS) || string.Equals(SearchName.ToString().ToLowerInvariant(), RECALLCURVE))
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
            #endregion


            #region DREAM 4.0 Export and Export All options
            hidFileType.RenderControl(writer);
            hidSelectedAssetNames.RenderControl(writer);
            hidRowSelectedCheckBoxes.RenderControl(writer);
            #endregion
        }

        /// <summary>
        /// Renders the datapreference DDL.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="isErrorOccured">if set to <c>true</c> [is error occured].</param>
        private void RenderDatapreferenceDDL(HtmlTextWriter writer, bool isErrorOccured)
        {

            if(!isErrorOccured)
            {
                if((string.Equals(SearchName.ToString().ToLowerInvariant(), TIMEDEPTH)) || (string.Equals(SearchName.ToString().ToLowerInvariant(), DIRECTIONALSURVEY)))
                {
                    writer.Write("<tr><td colspan=\"2\" align=\"left\" valign=\"top\" class=\"tdAdvSrchItem\">");//DREAM 4.0 Code added for Relocate detail report links
                    linkViewDetailReport.RenderControl(writer);//DREAM 4.0 Code added for Relocate detail report links
                    writer.Write("</td><td colspan=\"2\" align=\"right\" valign=\"top\" class=\"tdAdvSrchItem\"><b>Data Preference: </b>");
                    cboDataPreference.RenderControl(writer);

                    writer.Write("</td></tr>");
                }
                if((string.Equals(SearchName.ToString(), "Picks")))//DREAM 4.0 Code added for Relocate detail report links
                {
                    writer.Write("<tr><td colspan=\"4\" align=\"left\" valign=\"top\" class=\"tdAdvSrchItem\">");//DREAM 4.0 Code added for Relocate detail report links
                    linkViewDetailReport.RenderControl(writer);//DREAM 4.0 Code added for Relocate detail report links
                }

            }

            else
            {
                if((string.Equals(SearchName.ToString().ToLowerInvariant(), TIMEDEPTH)) || (string.Equals(SearchName.ToString().ToLowerInvariant(), DIRECTIONALSURVEY)))
                {
                    if((DataSourceSelection) && (!objCommonUtility.GetUserTeamID().Equals("0")))
                    {
                        writer.Write("<tr><td class=\"tdAdvSrchItem\" colspan=\"2\"><b>Data Source: </b>");
                        rdoDataSource.RenderControl(writer);
                        /// Rendering the data preference dropdown.
                        writer.Write("</td><td colspan=\"2\" align=\"right\" valign=\"top\" class=\"tdAdvSrchItem\"><b>Data Preference: </b>");
                        cboDataPreference.RenderControl(writer);

                    }
                    else
                    {
                        /// Rendering the data preference dropdown.
                        writer.Write("<tr><td colspan=\"4\" align=\"right\" valign=\"top\" class=\"tdAdvSrchItem\"><b>Data Preference: </b>");
                        cboDataPreference.RenderControl(writer);

                    }
                    writer.Write("</td></tr>");
                }
                else if((DataSourceSelection) && (!objCommonUtility.GetUserTeamID().Equals("0")))
                {
                    writer.Write("<tr><td class=\"tdAdvSrchItem\" valign=\"top\" colspan=\"4\"><b>Data Source: </b>");
                    rdoDataSource.RenderControl(writer);
                    writer.Write("</td></tr>");
                }
            }
        }

        /// <summary>
        /// Renders the view mode.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderViewMode(HtmlTextWriter writer)
        {
            rdoViewMode.RenderControl(writer);
        }
        #endregion

        #region Populating control dynamically
        /// <summary>
        /// Populates the depth ref.//in case of mechanical data where we are calling webservice to populate ddl
        /// </summary>
        private void PopulateDepthRef()
        {
            CreateDataObject();
            if(objRequestInfo != null)
            {
                objRequestInfo.Entity.ResponseType = string.Empty;
                xmlDocSearchResult = objReportController.GetSearchResults(objRequestInfo, intMaxRecords, MECHANICALDATADEPTHREF, null, intSortOrder);
                PopulatecboDepthRefValue(xmlDocSearchResult);
            }
        }

        /// <summary>
        /// Loads the depth ref.//in case of other report where we are populating ddl values staticalllly
        /// </summary>
        private void LoadDepthRef()
        {
            cboDepthRef.Items.Clear();
            string[] arrDepthRef ={ "--Select--", "BF", "DF", "GL", "KB", "PDL", "RT" };
            foreach(string str in arrDepthRef)
            {
                cboDepthRef.Items.Add(str);
            }
        }

        /// <summary>
        /// Populatecboes the depth ref value.
        /// </summary>
        /// <param name="xmlResponse">The XML response.</param>
        private void PopulatecboDepthRefValue(XmlDocument xmlResponse)
        {
            cboDepthRef.Items.Clear();
            if(xmlResponse != null)
            {
                XmlNodeList xmlNodeList = xmlResponse.SelectNodes("response/report/record");
                foreach(XmlNode node in xmlNodeList)
                {
                    ListItem item = new ListItem(node.SelectNodes("attribute[@name='depth_reference_point']/@value")[0].Value, node.SelectNodes("attribute[@name='depth_reference_elevation']/@value")[0].Value);
                    cboDepthRef.Items.Add(item);
                }
                ListItem pdlItem = new ListItem("PDL", "0.00");
                cboDepthRef.Items.Add(pdlItem);

                /// Selecting default value in dropdown
                XmlNode objxmlNode = xmlResponse.SelectSingleNode("response/report/record[1]/attribute[@name='Default']/@value");
                if((objxmlNode != null) && (cboDepthRef.Items.FindByText(objxmlNode.Value) != null))
                {
                    cboDepthRef.Items.FindByText(objxmlNode.Value).Selected = true;
                }

            }
        }
        //Added in DREAM 4.0
        /// <summary>
        /// Renders the filter user control.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderFilterUserControl(HtmlTextWriter writer)
        {

            if(SearchWithFilterCriteria && objFilterControl != null)
            {
                writer.Write("<tr><td colspan=\"4\" valign=\"top\">");
                objFilterControl.RenderControl(writer);
                writer.Write("</td></tr>");
            }
        }
        #endregion

        /// <summary>
        /// Gets the enum description.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static string GetEnumDescription(Enum value)
        {
            FieldInfo objFieldInfo = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])objFieldInfo.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if(attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        #endregion

    }
}

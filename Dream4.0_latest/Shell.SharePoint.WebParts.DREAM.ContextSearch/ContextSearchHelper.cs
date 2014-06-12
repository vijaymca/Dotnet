#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: ContextSearchHelper.cs
#endregion
/// <summary> 
/// This is ContextSearchHelper class which contains methods used in context search
/// </summary>
using System;
using System.ComponentModel;
using System.Web;
using System.Net;
using System.Data;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Services.Protocols;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using Shell.SharePoint.DREAM.SearchHelper;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Controller;

namespace Shell.SharePoint.WebParts.DREAM.ContextSearch
{
    /// <summary> 
    /// This is ContextSearchHelper class which contains methods used in context search
    /// </summary>
    public partial class ContextSearchResults :SearchHelper
    {

        #region DECLARATION
        #region Constants
        const string EPASSETTYPE = "AssetType";
        const string ASSET = "Asset";
        const string PRODUCTTYPE = "ProductType";
        const string HIDREQUESTID = "hidRequestID";
        const string FILTERDROPDOWN = "$cboFilter";
        const string VIEWMODERADIOGROUP = "$rdoViewMode";
        const string LENGTHTYPERADIOGROUP = "$rdoLengthType";
        const string DATASOURCERADIOGROUP = "$rdoDataSource";
        const string DATASHEETREPORT = "Datasheet";
        const string DATASHEET = "DATASHEET";
        const string GEOLOGICFEATURE = "Geologic Feature";
        const string FEET = "Feet";
        const string METER = "Metres";
        const string GEOLOGICFEATUREXPATH = "response/report/record[not(attribute[@name='Geologic Feature']/@value=preceding-sibling::record/attribute[@name='Geologic Feature']/@value)]/attribute[@name='Geologic Feature']/@value";
        const string SELECTALL = "--SelectAll--";
        const string NULL = "--BLANK--";
        const string PICKSDETAIL = "PicksDetail";
        const string TABULARDATASHEET = "TabularDataSheet";
        const string EPCATALOG = "epcatalog";
        const string WELLBOREHEADER = "WellboreHeader";
        const string PARSDETAIL = "PARSDetail";
        const string MECHANICALDATA = "mechanicaldata";
        const string WELLSUMMARY = "wellsummary";
        const string RECALLLOGSLVL1QS = "RecallLogsLvl1QueryString";
        const string RECALLLOGSLVL2QS = "RecallLogsLvl2QueryString";
        const string RECALLLOGSLVL3QS = "RecallLogsLvl3QueryString";
        const string RECALLLOGSLVL4QS = "RecallLogsLvl4QueryString";
        const string EDMREPORTREQUESTXML = "EDMREPORTREQUESTXML";
        const string SIGNIFICANTWELLEVENTSREQUESTXML = "SIGNIFICANTWELLEVENTSREQUESTXML";
        const string DEPTHREFVALUES = "BF|DF|GL|KB|PDL|RT";
        const string MECHANICALDATADEPTHREF = "MECHANICALDATADEPTHREF";
        const string WELLHISTORY = "wellhistory";
        const string UWI = "UWI";
        const string OPENWORKS = "OpenWorks";
        const string CDS = "CDS";
        const string DATAPREFERENCELIST = "Data Preference";
        const string TVDSS = "tvdss";
        /// ====================================
        /// Module Name:Well Test Data
        /// Description: Well Test Data Session and Report name string
        /// Date:16-July-2010
        /// Modified Lines:582-584
        /// ====================================
        const string WELLTESTREPORTREQUESTXML = "WELLTESTREPORTREQUESTXML";
        const string WELLTESTREPORT = "welltestreport";
        /// ====================================
        /// Module Name:Zone Properties
        /// Description: Zone Properties string
        /// Date:21-July-2010
        /// Modified Lines:596
        /// ====================================
        const string ZONEPROPERTIES = "zoneproperties";
        const string PVTREPORT = "pvtreport";
        const string ONCLICK = "OnClick";
        const string ONCHANGE = "onChange";
        const string RESULTHYPERLINKCSS = "resultHyperLink";
        const string PRESSURESURVEYREQUESTXML = "PRESSURESURVEYREQUESTXML";
        const string GEOPRESSURE = "geopressure";
        const string GEOPRESSUREREQUESTXML = "GEOPRESSUREREQUESTXML";
        const string FIELDHEADER = "FieldHeader";
        const string RESERVOIRHEADER = "ReservoirHeader";
        //Dream 4.0 Constants declartion starts
        const string SIGNIFICANTWELLEVENTS = "significantwellevents";
        const string WELLREVIEWS = "wellreviews";
        //Dream 4.0 Constants declartion ends
        #endregion

        #region Controls
        HyperLink lnkPrint;
        HyperLink lnkExportPage;
        HyperLink lnkExportAll;
        HyperLink lnkiRequest;
        HyperLink linkViewDetailReport;
        HyperLink linkExternalRecallLogReport;
        HiddenField hidRequestID;
        HiddenField hidReportSelectRow;
        HiddenField hidRowsCount;
        HiddenField hidMaxRecord;
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
        HiddenField hidDrpValue;
        HiddenField hidDepthUnit;
        RadioButton rbFeet;
        RadioButton rbMeters;
        RadioButtonList rdoViewMode;
        /// <summary>
        ///  Mechanical data specific controls
        /// </summary>
        RadioButtonList rdoLengthType;
        RadioButtonList rdoDataSource;
        //Dream 3.0 code
        //start
        RadioButtonList rdoTemperatureUnit;
        RadioButtonList rdoPressureUnit;
        //end
        DropDownList cboFilter;
        DropDownList cboDepthRef;
        /// <summary>
        /// Data preference dropdown
        /// </summary>
        DropDownList cboDataPreference;
        Label lblMessage;

        #region DREAM 4.0

        protected HiddenField hidFileType = new HiddenField();
        DropDownList ddlAssets;
        #endregion
        #endregion

        #region Variables
        string strPicksFilter = string.Empty;
        string strDisplayType = string.Empty;
        bool blnSuccessFull;
        #endregion
        #endregion

        #region Instantiating class objects
        /// <summary>
        /// Initializes a new instance of the <see cref="ContextSearchHelper"/> class.
        /// </summary>
        private void CreateBaseClassObjects()
        {
            objCommonUtility = new CommonUtility();
            objReportController = objFactory.GetServiceManager(ServiceName.ToString());
            objMossController = objFactory.GetServiceManager(MOSSSERVICE);
        }
        #endregion

        #region Creating controls
        /// <summary>
        /// Creates the external recall log link.
        /// </summary>
        /// <param name="searchDescription">The search description.</param>
        private void CreateExternalRecallLogLink(string searchDescription)
        {
            linkExternalRecallLogReport = new HyperLink();
            linkExternalRecallLogReport.ID = "linkExternalRecallLogReport";
            linkExternalRecallLogReport.CssClass = RESULTHYPERLINKCSS;
            linkExternalRecallLogReport.Attributes.Add("text-decoration", "underline");
            linkExternalRecallLogReport.NavigateUrl = "javascript:OpenRecallLogsReport('" + GetRecallLogsURL() + "','" + searchDescription + "');";
            linkExternalRecallLogReport.Text = "View Details in Recall Browser";
            this.Controls.Add(linkExternalRecallLogReport);
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
            hidDrpValue = new HiddenField();
            hidDrpValue.ID = "hidDrpValue";
            hidDateFormat = new HiddenField();
            hidDateFormat.ID = "hidDateFormat";
            hidReorderColValue = new HiddenField();
            hidReorderColValue.ID = "hidReorderColValue";
            this.Controls.Add(hidReorderColValue);
            hidReorderSourceId = new HiddenField();
            hidReorderSourceId.ID = "hidReorderSourceId";
            this.Controls.Add(hidReorderSourceId);
            hidAssetName = new HiddenField();
            hidAssetName.ID = "hidAssetName";
            ///Added for Feet Metre fix. Manoj
            hidDepthUnit = new HiddenField();
            hidDepthUnit.ID = "hidDepthUnit";
            this.Controls.Add(hidDepthUnit);
            #region Recall logs related hidden fields
            if(string.Equals(SearchName.ToString(), RECALLLOGS) || string.Equals(SearchName.ToString().ToLowerInvariant(), RECALLCURVE))
            {
                hidUWBI = new HiddenField();
                hidUWBI.ID = "hidUWBI";
                this.Controls.Add(hidUWBI);
                hidLogService = new HiddenField();
                hidLogService.ID = "hidLogService";
                this.Controls.Add(hidLogService);
                hidLogType = new HiddenField();
                hidLogType.ID = "hidLogType";
                this.Controls.Add(hidLogType);
                hidLogSource = new HiddenField();
                hidLogSource.ID = "hidLogSource";
                this.Controls.Add(hidLogSource);
                hidLogName = new HiddenField();
                hidLogName.ID = "hidLogName";
                this.Controls.Add(hidLogName);
                hidLogActivity = new HiddenField();
                hidLogActivity.ID = "hidLogActivity";
                this.Controls.Add(hidLogActivity);
                hidLogrun = new HiddenField();
                hidLogrun.ID = "hidLogrun";
                this.Controls.Add(hidLogrun);
                hidLogVersion = new HiddenField();
                hidLogVersion.ID = "hidLogVersion";
                this.Controls.Add(hidLogVersion);
                hidProjectName = new HiddenField();
                hidProjectName.ID = "hidProjectName";
                this.Controls.Add(hidProjectName);
            }
            #endregion

            #region DREAM 4.0
            hidFileType = new HiddenField();
            hidFileType.ID = "hidFileType";
            this.Controls.Add(hidFileType);
            hidSelectedAssetNames = new HiddenField();
            hidSelectedAssetNames.ID = "hidSelectedAssetNames";
            this.Controls.Add(hidSelectedAssetNames);
            hidRowSelectedCheckBoxes = new HiddenField();
            hidRowSelectedCheckBoxes.ID = "hidRowSelectedCheckBoxes";
            this.Controls.Add(hidRowSelectedCheckBoxes);
            hidColSelectedCheckBoxes = new HiddenField();
            hidColSelectedCheckBoxes.ID = "hidColSelectedCheckBoxes";
            this.Controls.Add(hidColSelectedCheckBoxes);
            #endregion

        }
        /// <summary>
        /// Creates the radio controls.
        /// </summary>
        /// <param name="viewMode">The view mode.</param>
        private void CreateRadioControls(string viewMode)
        {
            if(UnitConversion)
            {
                CreateUnitConvertionControls();
            }
            /// Rendering Mechanical data report specific criteria          
            if(DepthReference)
            {
                CreateDepthRefControls();
            }
            if(string.Equals(viewMode.ToString(), TABULARDATASHEET))
            {
                CreateViewModeControl();
            }
            if(string.Equals(SearchName.ToString(), PICKSDETAIL))
            {
                CreatePicksFilterControl();
            }
            if((string.Equals(SearchName.ToString().ToLowerInvariant(), TIMEDEPTH)) || (string.Equals(SearchName.ToString().ToLowerInvariant(), DIRECTIONALSURVEY)))
            {
                InitilizeDataPreferenceDDL();
            }
            if(DataSourceSelection)
            {
                CreateDataSourceControls();
            }
            if(TemperatureUnit)
            {
                CreateTemperatureUnitControl();
            }
            if(PressureUnit)
            {
                CreatePressureUnitControl();
            }
            //Added in DREAM 4.0 for single selection restriction
            if(DisplaySingleAsset && !SearchWithFilterCriteria)
            {
                CreateDDLAssets();
            }
        }

        /// <summary>
        /// Creates the pressure unit control.
        /// </summary>
        private void CreatePressureUnitControl()
        {
            rdoPressureUnit = new RadioButtonList();
            rdoPressureUnit.ID = "rdoPressureUnit";
            rdoPressureUnit.EnableViewState = true;
            rdoPressureUnit.RepeatDirection = RepeatDirection.Horizontal;
            rdoPressureUnit.RepeatLayout = RepeatLayout.Flow;
            rdoPressureUnit.Items.Add("barA");
            rdoPressureUnit.Items.Add("kPa");
            rdoPressureUnit.Items.Add("psiA");
            if(SearchName.ToString().ToLowerInvariant().Equals(PRESSURESURVEYDATA))
            {
                AddOnClickEventForRadioButtonList(rdoPressureUnit, "PressureUnitConvertorInTabReport('tblPressureSurvey','tblReservoir')");
            }
            else if(SearchName.ToString().ToLowerInvariant().Equals(WELLSUMMARY))
            {
                AddOnClickEventForRadioButtonList(rdoPressureUnit, "WellSummaryPressureUnitConvertor(this)");
            }
            else
            {
                AddOnClickEventForRadioButtonList(rdoPressureUnit, "PressureUnitConvertor(this)");
            }
            this.Controls.Add(rdoPressureUnit);
        }

        /// <summary>
        /// Creates the temperature unit control.
        /// </summary>
        private void CreateTemperatureUnitControl()
        {
            rdoTemperatureUnit = new RadioButtonList();
            rdoTemperatureUnit.ID = "rdoTemperatureUnit";
            rdoTemperatureUnit.EnableViewState = true;
            rdoTemperatureUnit.RepeatDirection = RepeatDirection.Horizontal;
            rdoTemperatureUnit.RepeatLayout = RepeatLayout.Flow;
            rdoTemperatureUnit.Items.Add("degC");
            rdoTemperatureUnit.Items.Add("degF");
            if(SearchName.ToString().ToLowerInvariant().Equals(PRESSURESURVEYDATA))
            {
                AddOnClickEventForRadioButtonList(rdoTemperatureUnit, "TemperatureUnitConvertorInTabReport('tblPressureSurvey','tblReservoir')");
            }
            else if(SearchName.ToString().ToLowerInvariant().Equals(WELLSUMMARY))
            {
                AddOnClickEventForRadioButtonList(rdoTemperatureUnit, "WellSummaryTemperatureUnitConvertor(this)");
            }
            else
            {
                AddOnClickEventForRadioButtonList(rdoTemperatureUnit, "TemperatureUnitConvertor(this)");
            }
            this.Controls.Add(rdoTemperatureUnit);
        }
        /// <summary>
        /// Adds the on click event for radio button list.
        /// </summary>
        /// <param name="objRadioButtonList">The obj radio button list.</param>
        /// <param name="jsFuctionName">Name of the js fuction.</param>
        private void AddOnClickEventForRadioButtonList(RadioButtonList objRadioButtonList, string jsFuctionName)
        {
            if(objRadioButtonList != null)
            {
                foreach(ListItem item in objRadioButtonList.Items)
                {
                    item.Attributes.Add(ONCLICK, jsFuctionName + ";");
                }
            }
        }
        /// <summary>
        /// Creates the data source controls.
        /// </summary>
        private void CreateDataSourceControls()
        {
            rdoDataSource = new RadioButtonList();
            rdoDataSource.AutoPostBack = true;
            rdoDataSource.EnableViewState = true;
            rdoDataSource.ID = "rdoDataSource";
            rdoDataSource.RepeatDirection = RepeatDirection.Horizontal;
            rdoDataSource.RepeatLayout = RepeatLayout.Flow;
            rdoDataSource.Items.Add(CDS);
            rdoDataSource.Items.Add(OPENWORKS);

            if(HttpContext.Current.Request.Form[this.UniqueID + DATASOURCERADIOGROUP] != null)
            {
                rdoDataSource.Items.FindByText(HttpContext.Current.Request.Form[this.UniqueID + DATASOURCERADIOGROUP].ToString()).Selected = true;

            }
            else
            {
                rdoDataSource.SelectedIndex = 0;
            }

            this.Controls.Add(rdoDataSource);
        }

        /// <summary>
        /// Creates the picks filter control.
        /// </summary>
        private void CreatePicksFilterControl()
        {
            cboFilter = new DropDownList();
            cboFilter.ID = "cboFilter";
            cboFilter.EnableViewState = true;
            cboFilter.AutoPostBack = true;
            cboFilter.SelectedIndexChanged += new EventHandler(cboFilter_SelectedIndexChanged);
            this.Controls.Add(cboFilter);
        }

        /// <summary>
        /// Creates the view mode controls.
        /// </summary>
        private void CreateViewModeControl()
        {
            rdoViewMode = new RadioButtonList();
            rdoViewMode.AutoPostBack = true;
            rdoViewMode.EnableViewState = true;
            rdoViewMode.ID = "rdoViewMode";
            rdoViewMode.RepeatDirection = RepeatDirection.Horizontal;
            rdoViewMode.RepeatLayout = RepeatLayout.Flow;
            //rdoViewMode.Items.Add("Data Sheet");
            //Added in Dream 3.1
            rdoViewMode.Items.Add(new ListItem("Data Sheet", "DataSheet"));
            rdoViewMode.Items.Add(new ListItem(TABULAR, TABULAR));

            //Commented in Dream 3.1
            //if (HttpContext.Current.Request.Form[this.UniqueID + VIEWMODERADIOGROUP] != null)
            //{
            //    rdoViewMode.Items.FindByText(HttpContext.Current.Request.Form[this.UniqueID + VIEWMODERADIOGROUP].ToString()).Selected = true;
            //    strDisplayType = HttpContext.Current.Request.Form[this.UniqueID + VIEWMODERADIOGROUP].ToString();
            //}
            //else
            //    rdoViewMode.SelectedIndex = 1;

            this.Controls.Add(rdoViewMode);
        }

        /// <summary>
        /// Creates the depth ref controls.
        /// </summary>
        private void CreateDepthRefControls()
        {
            rdoLengthType = new RadioButtonList();
            rdoLengthType.AutoPostBack = true;
            rdoLengthType.EnableViewState = true;
            rdoLengthType.ID = "rdoLengthType";
            rdoLengthType.RepeatDirection = RepeatDirection.Horizontal;
            rdoLengthType.RepeatLayout = RepeatLayout.Flow;
            rdoLengthType.Items.Add("Along Hole");
            rdoLengthType.Items.Add("True Vertical");

            if(HttpContext.Current.Request.Form[this.UniqueID + LENGTHTYPERADIOGROUP] != null)
            {
                rdoLengthType.Items.FindByText(HttpContext.Current.Request.Form[this.UniqueID + LENGTHTYPERADIOGROUP].ToString()).Selected = true;
            }
            else
            {
                rdoLengthType.SelectedIndex = 0;
            }

            this.Controls.Add(rdoLengthType);

            cboDepthRef = new DropDownList();
            cboDepthRef.ID = "cboDepthRef";
            cboDepthRef.EnableViewState = true;
            cboDepthRef.AutoPostBack = false;
            this.Controls.Add(cboDepthRef);
            /// Populating depth refrence dropdown values
            //if(SearchName.ToString().ToLowerInvariant().Equals(MECHANICALDATA))
            //{
            //    cboDepthRef.Attributes.Add(ONCHANGE, "javascript:MachanicalDataConversion(this)");
            //}
            ///// ====================================
            ///// Module Name:Well Test Data
            ///// Description: Depth reference conversion method registration
            ///// Date:16-July-2010
            ///// Modified Lines:2225-2229
            ///// ====================================
            //else if(SearchName.ToString().ToLowerInvariant().Equals(WELLTESTREPORT))
            //{
            //    cboDepthRef.Attributes.Add(ONCHANGE, "javascript:MachanicalDataConversion(this)");
            //}
            //else
            //{
            //    //**Commented in Dream 3.1
            //    //if (HttpContext.Current.Request.Form[this.UniqueID + VIEWMODERADIOGROUP] != null)
            //    //    strDisplayType = HttpContext.Current.Request.Form[this.UniqueID + VIEWMODERADIOGROUP].ToString();


            //    if((SearchName.ToString().ToLowerInvariant().Equals("wellboreheader")) && (string.Equals(strDisplayType.ToUpperInvariant(), DATASHEET)))
            //    {
            //        cboDepthRef.Attributes.Add(ONCHANGE, "javascript:DepthRefDTSTCalculator()");
            //    }
            //    else
            //    {
            //        cboDepthRef.Attributes.Add(ONCHANGE, "javascript:DepthRefCalculator('tblSearchResults')");
            //    }
            //}
        }
        /// <summary>
        /// Attaches the depth ref on change event HNDLR.
        /// </summary>
        private void AttachDepthRefOnChangeEventHndlr()
        {
            if(cboDepthRef != null)
            {
                if(SearchName.ToString().ToLowerInvariant().Equals(MECHANICALDATA))
                {
                    cboDepthRef.Attributes.Add(ONCHANGE, "javascript:MachanicalDataConversion(this)");
                }
                /// ====================================
                /// Module Name:Well Test Data
                /// Description: Depth reference conversion method registration
                /// Date:16-July-2010
                /// Modified Lines:2225-2229
                /// ====================================
                else if(SearchName.ToString().ToLowerInvariant().Equals(WELLTESTREPORT))
                {
                    cboDepthRef.Attributes.Add(ONCHANGE, "javascript:MachanicalDataConversion(this)");
                }
                else
                {
                    if((SearchName.ToString().ToLowerInvariant().Equals("wellboreheader")) && (string.Equals(strDisplayType.ToUpperInvariant(), DATASHEET)))
                    {
                        cboDepthRef.Attributes.Add(ONCHANGE, "javascript:DepthRefDTSTCalculator()");
                    }
                    else
                    {
                        cboDepthRef.Attributes.Add(ONCHANGE, "javascript:DepthRefCalculator('tblSearchResults')");
                    }
                }
            }
        }

        /// <summary>
        /// Creates the unit convertion controls.
        /// </summary>
        private void CreateUnitConvertionControls()
        {
            /// This will create option for feet/meter selection
            rbFeet = new RadioButton();
            rbFeet.ID = "rbFeet";
            rbFeet.GroupName = "FeetMeters";
            rbFeet.Text = FEET;

            rbMeters = new RadioButton();
            rbMeters.ID = "rbMeters";
            rbMeters.GroupName = "FeetMeters";
            rbMeters.Text = METER;
            /// ====================================
            /// Module Name:Well Test Data
            /// Description: Feet Meter conversion method registration
            /// Date:16-July-2010
            /// Modified Lines:2167-2168
            /// ====================================
            if((SearchName.ToString().ToLowerInvariant().Equals(MECHANICALDATA)) || (SearchName.ToString().ToLowerInvariant().Equals(PALEOMARKERSREPORT)) || SearchName.ToString().ToLowerInvariant().Equals(WELLTESTREPORT) || SearchName.ToString().ToLowerInvariant().Equals(PRESSURESURVEYDATA))
            {
                rbFeet.Attributes.Add(ONCLICK, "javascript:FeetMetreConversion('Feet');");
                rbMeters.Attributes.Add(ONCLICK, "javascript:FeetMetreConversion('Metres');");
            }
            else if(SearchName.ToString().ToLowerInvariant().Equals(WELLSUMMARY))
            {
                rbFeet.Attributes.Add(ONCLICK, "javascript:WellSummaryFeetMetreConversion('Feet');");
                rbMeters.Attributes.Add(ONCLICK, "javascript:WellSummaryFeetMetreConversion('Metres');");
            }
            else
            {
                rbFeet.Attributes.Add(ONCLICK, "javascript:FeetMetreConversionNew('tblSearchResults', 'Feet');");
                rbMeters.Attributes.Add(ONCLICK, "javascript:FeetMetreConversionNew('tblSearchResults', 'Metres');");
            }
            if(SearchName.ToString().ToLowerInvariant().Equals("interpolatedlogs") || SearchName.ToString().ToLowerInvariant().Equals("positionlogdata"))
            {
                rbFeet.Attributes.Add(ONCLICK, rbFeet.Attributes[ONCLICK] + "ChangeInterpolatedDepthLabel('Feet');");
                rbMeters.Attributes.Add(ONCLICK, rbMeters.Attributes[ONCLICK] + "ChangeInterpolatedDepthLabel('Metres');");
            }
            this.Controls.Add(rbFeet);
            this.Controls.Add(rbMeters);
        }
        /// <summary>
        /// Creates the message label.
        /// </summary>
        private void CreateMessageLabel()
        {
            lblMessage = new Label();
            lblMessage.ID = "lblMessage";
            lblMessage.CssClass = "labelMessage";
            /// Sets the default visibility of messege label to false.
            lblMessage.Visible = false;
            this.Controls.Add(lblMessage);
        }
        /// <summary>
        /// Creates the hyper links.
        /// </summary>
        /// <param name="searchDescription">The search description.</param>
        private void CreateHyperLinks(string searchDescription)
        {
            if(Print)
            {
                CreatePrintLink();
            }
            if(ExportPage)
            {
                CreateExportPageLink();
            }
            if(ExportAll)
            {
                CreateExportAllLink();
            }
            if(string.Equals(SearchName.ToString(), PARSDETAIL))
            {
                CreatePARSDetailLink();
            }
            if(DetailReport)
            {
                CreateDetailReportLink(searchDescription);
            }
        }

        /// <summary>
        /// Creates the PARS detail link.
        /// </summary>
        private void CreatePARSDetailLink()
        {
            lnkiRequest = new HyperLink();
            lnkiRequest.ID = "linkiRequest";
            lnkiRequest.CssClass = RESULTHYPERLINKCSS;
            lnkiRequest.NavigateUrl = "javascript:OpeniRequest();";
            lnkiRequest.ImageUrl = "/_layouts/DREAM/images/iRequest.gif";
            this.Controls.Add(lnkiRequest);
        }

        /// <summary>
        /// Creates the print link.
        /// </summary>
        private void CreatePrintLink()
        {
            lnkPrint = new HyperLink();
            lnkPrint.ID = "linkPrint";
            lnkPrint.CssClass = RESULTHYPERLINKCSS;
            lnkPrint.NavigateUrl = "javascript:printContent('tblSearchResults','" + SearchName.ToString() + "');";
            lnkPrint.ImageUrl = "/_layouts/DREAM/images/icon_print.gif";
            this.Controls.Add(lnkPrint);
        }

        /// <summary>
        /// Creates the detail report link.
        /// </summary>
        /// <param name="searchDescription">The search description.</param>
        private void CreateDetailReportLink(string searchDescription)
        {
            linkViewDetailReport = new HyperLink();
            linkViewDetailReport.ID = "linkViewDetailReport";
            linkViewDetailReport.CssClass = RESULTHYPERLINKCSS;
            linkViewDetailReport.ToolTip = "View Detail Report";//DREAM4.0Code added for relocate detail report
            linkViewDetailReport.Attributes.Add("text-decoration", "underline");
            linkViewDetailReport.NavigateUrl = "javascript:OpenDetailReport('" + searchDescription + "');";
            linkViewDetailReport.ImageUrl = "/_layouts/DREAM/images/ApViewProps.gif";//DREAM4.0Code changes done for relocate detail report
            this.Controls.Add(linkViewDetailReport);
        }

        /// <summary>
        /// Creates the export all link.
        /// </summary>
        private void CreateExportAllLink()
        {
            lnkExportAll = new HyperLink();
            lnkExportAll.ID = "linkExportAll";
            lnkExportAll.CssClass = RESULTHYPERLINKCSS;
            if(string.Equals(SearchName.ToString().ToLowerInvariant(), PALEOMARKERSREPORT))
            {
                lnkExportAll.NavigateUrl = "javascript:TabTabularExportAll ('tblCDSPAL','tblMMSPAM');";
            }
            else if(string.Equals(SearchName.ToString().ToLowerInvariant(), PRESSURESURVEYDATA))
            {
                lnkExportAll.NavigateUrl = "javascript:TabTabularExportAll ('tblPressureSurvey','tblReservoir');";
            }
            else
            {
                lnkExportAll.NavigateUrl = "javascript:ExportToExcelAll('/Pages/ExportToExcel.aspx','ContextSearchResults');";
            }
            lnkExportAll.ImageUrl = "/_layouts/DREAM/images/icon_Excel.gif";
            this.Controls.Add(lnkExportAll);
        }

        /// <summary>
        /// Creates the export page link.
        /// </summary>
        private void CreateExportPageLink()
        {
            lnkExportPage = new HyperLink();
            lnkExportPage.ID = "linkExcel";
            lnkExportPage.CssClass = RESULTHYPERLINKCSS;
            if(SearchName.ToString().ToLowerInvariant().Equals(EPCATALOG) || SearchName.ToString().ToLowerInvariant().Equals(PVTREPORT))
            {
                lnkExportPage.NavigateUrl = "javascript:EPExportToExcel('tblSearchResults');";
            }
            else if(SEARCHNAMEFORUBIWRKSHEET.Contains(SearchName.ToString().ToLowerInvariant()))
            {
                lnkExportPage.NavigateUrl = "javascript:ExportUBIToWorkSheet();";
            }
            else if(string.Equals(SearchName.ToString().ToLowerInvariant(), EDMREPORT))
            {
                lnkExportPage.NavigateUrl = "javascript:ExportEDMSearchResults('tblSearchResults');";
            }
            else if(string.Equals(SearchName.ToString().ToLowerInvariant(), MECHANICALDATA))
            {
                lnkExportPage.NavigateUrl = "javascript:ExportMDSearchResults('tblHole','tblCasings','tblLiners','tblMechanicalcontent','tblFluidscements','tblGrossperforations','tblWellhead');";
            }
            else if(string.Equals(SearchName.ToString().ToLowerInvariant(), WELLTESTREPORT))
            {
                lnkExportPage.NavigateUrl = "javascript:ExportWellTestSearchResults('tblgeneraltestdata','tbltestanalysisdata','tbltestformationdata','tbltestflowdata','tbltestintervaldata');";
            }
            else if(string.Equals(SearchName.ToString().ToLowerInvariant(), PALEOMARKERSREPORT))
            {
                lnkExportPage.NavigateUrl = "javascript:ExportTabTabularSearchResults('tblCDSPAL','tblMMSPAM');";
            }
            else if(string.Equals(SearchName.ToString().ToLowerInvariant(), PRESSURESURVEYDATA))
            {
                lnkExportPage.NavigateUrl = "javascript:ExportTabTabularSearchResults('tblPressureSurvey','tblReservoir');";
            }
            else
            {
                lnkExportPage.NavigateUrl = "javascript:ExportToExcel('tblSearchResults');";
            }
            lnkExportPage.ImageUrl = "/_layouts/DREAM/images/icon_Excel.gif";
            this.Controls.Add(lnkExportPage);
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
            /// Loop through the Geological Features and adds them to filter field.
            foreach(XmlNode xmlnodeValue in xmlValues)
            {
                strValue = xmlnodeValue.Value;
                if(string.IsNullOrEmpty(xmlnodeValue.Value))
                {
                    strValue = NULL;
                }
                filter.Items.Add(xmlnodeValue.Value);
            }
            string[] arrPicksFilter = new string[filter.Items.Count];
            int intListCount = 0;
            foreach(ListItem lstItem in filter.Items)
            {
                arrPicksFilter[intListCount] = lstItem.Text;
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
            //Gets the filter values from session.
            object objFilterValues = CommonUtility.GetSessionVariable(Page, enumSessionVariable.picksFilterValues.ToString());
            if(objFilterValues != null)
            {
                //Assign the values to the Picks filter dropdown.
                foreach(string strPicksValue in (string[])objFilterValues)
                {
                    filter.Items.Add(strPicksValue);
                }
            }
            if(strPicksFilter.Length > 0)
            {
                filter.SelectedValue = strPicksFilter;
            }
        }
        /// <summary>
        /// Initilizes the data preference DDL.
        /// </summary>
        private void InitilizeDataPreferenceDDL()
        {
            cboDataPreference = new DropDownList();
            cboDataPreference.ID = "cboDataPreference";
            DataTable dtDataPreference = null;
            try
            {
                dtDataPreference = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, DATAPREFERENCELIST, string.Empty);
                foreach(DataRow dtRow in dtDataPreference.Rows)
                {
                    if(dtRow[0] != null)
                    {
                        cboDataPreference.Items.Add((string)dtRow[0]);
                    }
                }
                if(dtDataPreference.Select("IsDefault='1'")[0][0] != null)
                {
                    cboDataPreference.SelectedValue = (string)dtDataPreference.Select("IsDefault='1'")[0][0];
                }
                cboDataPreference.EnableViewState = true;
                cboDataPreference.AutoPostBack = true;
                this.Controls.Add(cboDataPreference);
            }
            finally
            {
                if(dtDataPreference != null)
                {
                    dtDataPreference.Dispose();
                }
            }
        }
        #endregion

        #region Creating Request
        /// <summary>
        /// Adds the data preference attribute.
        /// </summary>
        private void AddDataPreferenceAttribute()
        {
            if(cboDataPreference != null)
            {
                if(!cboDataPreference.SelectedValue.ToLowerInvariant().Equals("show all"))
                {
                    ArrayList arlAttributeGroup = new ArrayList();
                    ArrayList arlAttribute = new ArrayList();
                    AttributeGroup objAttributeGroup = new AttributeGroup();
                    /// Initializes the attribute objects and set the values
                    Attributes objDataPrefAttribute = new Attributes();
                    objDataPrefAttribute.Name = PREFERREDFLAG;
                    objDataPrefAttribute.Value = new ArrayList();
                    Value preferredValue = new Value();

                    if(cboDataPreference.SelectedValue.ToLowerInvariant().Equals("preferred data"))
                    {
                        preferredValue.InnerText = "true";
                    }
                    else
                    {
                        preferredValue.InnerText = "false";
                    }
                    objDataPrefAttribute.Value.Add(preferredValue);
                    objDataPrefAttribute.Operator = GetOperator(objDataPrefAttribute.Value);
                    /// Adds the attribute object to the attribute Group.
                    arlAttribute.Add(objRequestInfo.Entity.Attribute[0]);
                    objRequestInfo.Entity.Attribute = null;
                    arlAttribute.Add(objDataPrefAttribute);

                    objAttributeGroup.Operator = ANDOPERATOR;
                    objAttributeGroup.Attribute = arlAttribute;

                    arlAttributeGroup.Add(objAttributeGroup);
                    objRequestInfo.Entity.AttributeGroups = arlAttributeGroup;
                }

            }
        }
        /// <summary>
        /// Adds the entity attributes.
        /// </summary>
        private void AddEntityAttributes()
        {

            if(DepthReference)
            {
                SetEntityTVDSS();
            }
            if(DataSourceSelection)
            {
                SetEntityDataSource();
            }

        }
        /// <summary>
        /// Creates the data object.
        /// </summary>
        private void CreateDataObject()
        {
            string strIdentifiedItem = string.Empty;
            if(!string.IsNullOrEmpty(hidSelectedRows.Value))
            {
                /// Gets the Selected Identifier value from the results page.
                strIdentifiedItem = hidSelectedRows.Value;

                /// Gets the Selected Identifier Column name from the results page.
                hidReportSelectRow.Value = strIdentifiedItem;
                arrIdentifierValue = strIdentifiedItem.Split('|');

                /// Creates the requestInfo object to fetch result from report service.
                objRequestInfo = SetBasicDataObjects(SearchName.ToString(), hidSelectedCriteriaName.Value.Trim(), arrIdentifierValue, false, false, intMaxRecords);
            }
            else
                objRequestInfo = null;
        }
        /// <summary>
        /// Sets the entity TVDSS.
        /// </summary>
        private void SetEntityTVDSS()
        {
            if(rdoLengthType.SelectedValue.ToLowerInvariant().Equals(TRUEVERTICAL))
            {
                objRequestInfo.Entity.TVDSS = true;
            }
            else
            {
                objRequestInfo.Entity.TVDSS = false;
            }
        }
        /// <summary>
        /// Sets the entity data source.
        /// </summary>
        private void SetEntityDataSource()
        {

            if(rdoDataSource.SelectedValue.ToLowerInvariant().Equals(OPENWORKS.ToLowerInvariant()))
            {
                objRequestInfo.Entity.DataSource = OPENWORKS;
                objRequestInfo.Entity.ProjectName = objCommonUtility.GetCurrentUserProjectName();
            }
            else
            {
                objRequestInfo.Entity.DataSource = string.Empty;
            }

        }

        /// <summary>
        /// Creates the document quick search request info.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        private RequestInfo CreateDocumentQuickSearchRequestInfo(string criteria)
        {
            Entity objEntity;
            RequestInfo objRequestInfo;
            objRequestInfo = new RequestInfo();
            objEntity = new Entity();
            objEntity.ResponseType = TABULAR;
            objEntity.Criteria = SetCriteria(criteria, "string");
            objRequestInfo.Entity = objEntity;
            objRequestInfo.Entity.Attribute = new ArrayList();
            //Adding country attribute
            //DREAM4.0
            //Start
            AddCountryAttribute(objRequestInfo.Entity.Attribute);
            //end
            return objRequestInfo;
        }
        /// <summary>
        /// Creates the EP request info.
        /// </summary>
        /// <param name="assetType">Type of the asset.</param>
        /// <param name="identifiers">The identifiers.</param>
        /// <returns></returns>
        private RequestInfo CreateEPRequestInfo(string assetType, string identifiers)
        {
            Entity objEntity;
            RequestInfo objRequestInfo;
            objRequestInfo = new RequestInfo();
            objEntity = new Entity();
            objEntity.ResponseType = TABULAR;
            objEntity.AttributeGroups = new ArrayList();

            AttributeGroup objAttributeGrp = new AttributeGroup();
            objAttributeGrp.Operator = ANDOPERATOR;
            objAttributeGrp.Attribute = new ArrayList();
            /// Adding asset value               
            if(!string.IsNullOrEmpty(identifiers))
            {
                string strPattern = @"\r\n";
                Regex fixMe = new Regex(strPattern);
                string strTrimmedMyAssetValues = fixMe.Replace(identifiers, string.Empty);

                string[] arrAssetVal = strTrimmedMyAssetValues.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                Attributes objAssetValue = new Attributes();
                objAssetValue.Name = ASSET;
                objAssetValue.Value = new ArrayList();

                foreach(string strAssetVal in arrAssetVal)
                {
                    Value objVal = new Value();
                    objVal.InnerText = strAssetVal.Trim();
                    objAssetValue.Value.Add(objVal);
                }
                objAssetValue.Operator = GetOperator(objAssetValue.Value);
                objAttributeGrp.Attribute.Add(objAssetValue);
            }
            //Adding country attribute
            //DREAM4.0
            //Start
            AddCountryAttribute(objAttributeGrp.Attribute);
            //end
            objEntity.AttributeGroups.Add(objAttributeGrp);

            objRequestInfo.Entity = objEntity;
            return objRequestInfo;
        }
        /// <summary>
        /// Adds the country attribute.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        private void AddCountryAttribute(ArrayList attributes)
        {
            if(!string.IsNullOrEmpty(Page.Request.QueryString["country"]) && !Page.Request.QueryString["country"].Trim().Equals("0"))
            {
                attributes.Add(AddAttribute("Country", "EQUALS", new string[] { Page.Request.QueryString["country"].Trim() }));
            }
        }
        /// <summary>
        /// Creates the PVT request info.
        /// </summary>
        /// <param name="PVTReport">The EPCatalog report.</param>
        /// <param name="assetType">Type of the asset.</param>
        /// <param name="identifiers">The identifiers.</param>
        /// <returns></returns>
        private RequestInfo CreatePVTRequestInfo(string PVTReport, string assetType, string identifiers)
        {
            RequestInfo objPVTRequestInfo = CreateEPRequestInfo(assetType, identifiers);
            ArrayList arrlstProductType = GetPVTProductTypes(PVTReport);
            if(arrlstProductType != null)
            {
                // ** code for adding group of product types to added**//
                Attributes objProductType = new Attributes();
                objProductType.Name = PRODUCTTYPE;
                objProductType.Value = new ArrayList();
                foreach(string strValue in arrlstProductType)
                {
                    Value objProductTypeVal = new Value();
                    objProductTypeVal.InnerText = strValue;
                    objProductType.Value.Add(objProductTypeVal);
                }
                objProductType.Operator = GetOperator(objProductType.Value);
                ((AttributeGroup)objPVTRequestInfo.Entity.AttributeGroups[0]).Attribute.Add(objProductType);
            }
            return objPVTRequestInfo;
        }
        #endregion

        #region Getting Response
        /// <summary>
        /// Gets the general response.
        /// </summary>
        private void GetGeneralResponse()
        {
            #region ResponseType
            SetViewMode();
            #endregion
            switch(SearchName.ToString().ToLowerInvariant())
            {
                case RECALLCURVE:
                    {
                        /// Gets the detail report result for Recall Curve.
                        GetRecallCurveReport();
                        break;
                    }
                case EPCATALOG:
                    {
                        /// Gets the detail report result for EPCATALOG
                        GetEPCatalogResult();
                        break;
                    }
                case PVTREPORT:
                    {
                        GetPVTReportResult();
                        break;
                    }
                default:
                    {
                        /// Gets the report result for other reports except Recall Curve.
                        GetResponseXML();
                        break;
                    }
            }
        }
        /// <summary>
        /// Gets the recall curve report.
        /// </summary>
        private void GetRecallCurveReport()
        {
            objRequestInfo = GetRecallCurveRequestInfo(SearchName.ToString());
            AddEntityAttributes();
            /// Calling the Controller method to get the Search results.
            //Dream 4.0 pagination realted changes start
            if(EnableSkipCount)
            {
                SetSkipInfo(objRequestInfo.Entity);
            }
            //Dream 4.0 pagination realted changes ends
            xmlDocSearchResult = objReportController.GetSearchResults(objRequestInfo, intMaxRecords, SearchName.ToString(), null, intSortOrder);
        }

        /// <summary>
        /// Gets the EP catalog result.
        /// </summary>
        private void GetEPCatalogResult()
        {
            XmlDocument xmlSearchRequest = null;
            object objAdvancedRequestXML = null;
            //Document search from Quick search page
            if((Page.Request.QueryString["assettype"] != null) && (Page.Request.QueryString["assettype"].ToLowerInvariant().Equals("document")))
            {
                objRequestInfo = CreateDocumentQuickSearchRequestInfo(Page.Server.UrlDecode(Page.Request.QueryString["SearchType"]));
                xmlDocSearchResult = objReportController.GetSearchResults(objRequestInfo, intMaxRecords, SearchName.ToString().ToUpperInvariant(), null, intSortOrder);
            }
            //EP Catalog with filter
            ///pages/EPCatalog.aspx?SearchType=EPCATALOG&assetType=Wellbore
            else if((Page.Request.QueryString["searchtype"] != null) && (Page.Request.QueryString["searchtype"].ToLowerInvariant().Equals("epcatalog")))
            {
                xmlSearchRequest = new XmlDocument();
                objAdvancedRequestXML = null;
                objAdvancedRequestXML = CommonUtility.GetSessionVariable(Page, SearchName.ToString().ToUpperInvariant());
                CommonUtility.RemoveSessionVariable(Page, SearchName.ToString().ToUpperInvariant());
                /// Gets the result from the EP-Catalog service.
                xmlSearchRequest.LoadXml((string)objAdvancedRequestXML);
                xmlDocSearchResult = objReportController.GetSearchResults(xmlSearchRequest, intMaxRecords, SearchName.ToString().ToUpperInvariant(), null, intSortOrder);
            }
            //EP Catalog without filter
            //pages/EPCatalog.aspx?SearchType=EPCatalogWithoutFilter&assetType=Wellbore
            else if((Page.Request.QueryString["searchtype"] != null) && (Page.Request.QueryString["searchtype"].ToLowerInvariant().Equals("epcatalogwithoutfilter")))
            {
                objRequestInfo = CreateEPRequestInfo(Page.Request.QueryString["assettype"], hidSelectedRows.Value);
                xmlDocSearchResult = objReportController.GetSearchResults(objRequestInfo, intMaxRecords, SearchName.ToString().ToUpperInvariant(), null, intSortOrder);
            }

        }
        /// <summary>
        /// Gets the EPCtalog report result.
        /// </summary>
        private void GetPVTReportResult()
        {
            objRequestInfo = CreatePVTRequestInfo(Page.Server.UrlDecode(Page.Request.QueryString["SearchType"]), this.Page.Request.QueryString["assettype"], hidSelectedRows.Value);
            xmlDocSearchResult = objReportController.GetSearchResults(objRequestInfo, intMaxRecords, SearchName.ToString().ToUpperInvariant(), null, intSortOrder);
        }

        /// <summary>
        /// Gets the response XML.
        /// </summary>
        /// 
        private void GetResponseXML()
        {
            CreateDataObject();
            if(objRequestInfo != null)
            {
                //Adding data preference attribute
                if((string.Equals(SearchName.ToString().ToLowerInvariant(), TIMEDEPTH)) || (string.Equals(SearchName.ToString().ToLowerInvariant(), DIRECTIONALSURVEY)))
                {
                    AddDataPreferenceAttribute();
                }
                if(string.Equals(SearchName.ToString().ToLowerInvariant(), PALEOMARKERSREPORT))
                {
                    AddPaleoMarkersAttribute();
                }
                AddEntityAttributes();
                //Dream 4.0 pagination realted changes start
                #region Dream 4.0
                if(EnableSkipCount)
                {
                    SetSkipInfo(objRequestInfo.Entity);
                }
                if(string.Equals(SearchName.ToString(), PICKSDETAIL) && ((IsPostBackByControl(cboFilter)) || ((Page.Request.Params.Get(EVENTTARGET) != null) && (Page.Request.Params.Get(EVENTTARGET).ToLowerInvariant().Contains(UPDATEPANELID)))))
                {
                    AddPicksFilterCriteria();
                }
                #endregion
                //Dream 4.0 pagination realted changes ends
                /// Call for the GetSearchResults() method to fetch the search results from webservice.
                xmlDocSearchResult = objReportController.GetSearchResults(objRequestInfo, intMaxRecords, SearchName.ToString(), strSortColumn, intSortOrder);
            }
        }

        /// <summary>
        /// Adds the picks filter criteria.
        /// </summary>
        private void AddPicksFilterCriteria()
        {
            ArrayList arlAttributeGroup = new ArrayList();
            ArrayList arlAttribute = new ArrayList();
            AttributeGroup objAttributeGroup = new AttributeGroup();
            /// Initializes the attribute objects and set the values
            Attributes objPicksFilterAttribute = new Attributes();
            objPicksFilterAttribute.Name = GEOLOGICFEATURE;
            objPicksFilterAttribute.Value = new ArrayList();
            Value objValue = new Value();
            if(strPicksFilter.Equals(SELECTALL))
            {
                objValue.InnerText = STAROPERATOR;
            }
            else
            {
                objValue.InnerText = strPicksFilter;
            }
            objPicksFilterAttribute.Value.Add(objValue);
            objPicksFilterAttribute.Operator = GetOperator(objPicksFilterAttribute.Value);
            /// Adds the attribute object to the attribute Group.
            arlAttribute.Add(objRequestInfo.Entity.Attribute[0]);
            objRequestInfo.Entity.Attribute = null;
            arlAttribute.Add(objPicksFilterAttribute);
            objAttributeGroup.Operator = ANDOPERATOR;
            objAttributeGroup.Attribute = arlAttribute;
            arlAttributeGroup.Add(objAttributeGroup);
            objRequestInfo.Entity.AttributeGroups = arlAttributeGroup;
        }
        /// <summary>
        /// Gets the mechanical data response.
        /// </summary>
        /// 
        private void GetMechanicalDataResponse()
        {
            /// To set activdiv as hole section when it loads first time.
            strActiveDiv = "holesection";
            objRequestInfo = new RequestInfo();
            Entity objEntity = new Entity();
            objRequestInfo.Entity = objEntity;
            objEntity.ResponseType = ResponseType;
            if(rdoLengthType.SelectedValue.ToLowerInvariant().Equals(TRUEVERTICAL))
            {
                objEntity.Name = "CommonData_tvdss";
            }
            else
            {
                objEntity.Name = "CommonData";
            }
            objEntity.Attribute = new ArrayList();
            objEntity.Attribute.Add(AddAttribute(hidSelectedCriteriaName.Value, EQUALSOPERATOR, new string[] { ddlAssets.SelectedValue }));
            xmlDocSearchResult = objReportController.GetSearchResults(objRequestInfo, intMaxRecords, SearchName.ToString(), null, intSortOrder);
        }
        /// <summary>
        /// Gets the EDM report result.
        /// </summary>
        private void GetEDMReportResult()
        {
            XmlDocument xmlDocSearchRequest = null;
            if(objFilterControl != null && objFilterControl.RequestXML != null)
            {
                xmlDocSearchRequest = objFilterControl.RequestXML;//Geting request xml generated by filter usercontrol 
            }
            if(xmlDocSearchRequest != null)
            {
                XmlNode objXmlNode = null;
                if(xmlDocSearchRequest.SelectSingleNode(RESPONSETYPEXPATH) != null)
                {
                    objXmlNode = xmlDocSearchRequest.SelectSingleNode(RESPONSETYPEXPATH);
                }
                if((objXmlNode != null) && (objXmlNode.Value.ToLowerInvariant().Equals(DATASHEETREPORT.ToLowerInvariant())))
                {
                    this.Print = false;
                    this.ExportPage = false;
                    this.ExportAll = false;
                    this.XSLFileName = XSLFileEnum.EDMHierarchicalResults;
                }
                else
                {
                    this.Print = true;
                    this.ExportPage = true;
                    this.ExportAll = true;
                    this.XSLFileName = XSLFileEnum.EDMTabularResults;
                }
                if(EnableSkipCount)
                {
                    SetSkipInfo(xmlDocSearchRequest);
                }
                xmlDocSearchResult = objReportController.GetSearchResults(xmlDocSearchRequest, intMaxRecords, SearchName.ToString(), strSortColumn, intSortOrder);
            }
        }

        /// <summary>
        /// Gets the well test report response.
        /// </summary>
        /// ====================================
        /// Module Name:Well Test Data
        /// Description: Get Well Test Data Response XML
        /// Date:16-July-2010
        /// Modified Lines:1833-1877
        /// ====================================
        private void GetWellTestReportResponse()
        {
            strActiveDiv = "generaltestdata";
            XmlDocument xmlDocSearchRequest = null;
            if(objFilterControl != null && objFilterControl.RequestXML != null)
            {
                xmlDocSearchRequest = objFilterControl.RequestXML;//Geting request xml generated by filter usercontrol 
            }
            if(xmlDocSearchRequest != null)
            {
                XmlNode objXmlNode = null;
                if(xmlDocSearchRequest.SelectSingleNode(RESPONSETYPEXPATH) != null)
                {
                    objXmlNode = xmlDocSearchRequest.SelectSingleNode(RESPONSETYPEXPATH);
                }
                if((objXmlNode != null) && (objXmlNode.Value.ToLowerInvariant().Equals(DATASHEETREPORT.ToLowerInvariant())))
                {
                    this.Print = false;
                    this.ExportPage = false;
                    this.ViewMode = ViewModeEnum.DataSheet;
                    this.XSLFileName = XSLFileEnum.WellTestReport_DataSheet;
                }
                else
                {
                    this.Print = true;
                    this.ExportPage = true;
                    this.ViewMode = ViewModeEnum.Tabular;
                    this.XSLFileName = XSLFileEnum.WellTestReport_Tabular;
                }
                if(EnableSkipCount)
                {
                    SetSkipInfo(xmlDocSearchRequest);
                }
                xmlDocSearchResult = objReportController.GetSearchResults(xmlDocSearchRequest, intMaxRecords, SearchName.ToString(), strSortColumn, intSortOrder);
            }
        }

        ///// <summary>
        ///// Gets the pressure survey response.
        ///// </summary>
        //private void GetPressureSurveyResponse()
        //{
        //    string strRequestXml = string.Empty;
        //    XmlDocument xmlDocSearchRequest = new XmlDocument();
        //    if(CommonUtility.GetSessionVariable(this.Page, PRESSURESURVEYREQUESTXML) != null)
        //    {
        //        strRequestXml = (string)CommonUtility.GetSessionVariable(this.Page, PRESSURESURVEYREQUESTXML);
        //        xmlDocSearchRequest.LoadXml(strRequestXml);
        //        AddEntityAttributes(xmlDocSearchRequest);
        //        xmlDocSearchResult = objReportController.GetSearchResults(xmlDocSearchRequest, intMaxRecords, SearchName.ToString(), strSortColumn, intSortOrder);
        //    }
        //}
        /// <summary>
        /// Adds the entity attributes.
        /// </summary>
        /// <param name="requestXml">The request XML.</param>
        private void AddEntityAttributes(XmlDocument requestXml)
        {
            if(DepthReference)
            {
                XmlNode xmlNodeEnity = requestXml.SelectSingleNode(ENTITYXPATH);
                if((xmlNodeEnity != null) && (rdoLengthType.SelectedValue.ToLowerInvariant().Equals(TRUEVERTICAL)))
                {
                    XmlAttribute xmlAttributeLengthType = requestXml.CreateAttribute(TVDSS);
                    xmlNodeEnity.Attributes.Append(xmlAttributeLengthType);
                    xmlAttributeLengthType.Value = true.ToString().ToLowerInvariant();
                }
            }
        }
        #endregion


        #region Recall logs
        /// <summary>
        /// Gets the recall logs URL.
        /// </summary>
        /// <returns>string</returns>
        private string GetRecallLogsURL()
        {
            string strURL = PortalConfiguration.GetInstance().GetKey(RECALLLOGSURL);
            strURL += PortalConfiguration.GetInstance().GetKey(RECALLLOGSLVL1QS);
            strURL += PortalConfiguration.GetInstance().GetKey(RECALLLOGSLVL2QS);
            if(SearchName.ToString().ToLowerInvariant().Equals(RECALLCURVE))
            {
                strURL += PortalConfiguration.GetInstance().GetKey(RECALLLOGSLVL3QS);
            }
            strURL += PortalConfiguration.GetInstance().GetKey(RECALLLOGSLVL4QS);

            return strURL;
        }
        #endregion

        #region External URL integeration
        /// <summary>
        /// Processes the external request.
        /// </summary>
        /// <returns>true/false</returns>
        private bool ProcessExternalRequest()
        {
            bool blnSuccess = false;
            if(this.Page.Request.QueryString[UWBINAME] != null)
            {
                blnSuccess = AssignValueToHidnFld(UWBINAME);
            }
            else if(this.Page.Request.QueryString[UWI] != null)
            {
                blnSuccess = AssignValueToHidnFld(UWI);
            }
            return blnSuccess;
        }
        /// <summary>
        /// Determines whether [is external reuest].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is external reuest]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsExternalReuest()
        {
            return ((this.Page.Request.QueryString[UWBINAME] != null) || (this.Page.Request.QueryString[UWI] != null));
        }
        /// <summary>
        /// Assigns the value to hidn FLD.
        /// </summary>
        /// <param name="queryStringName">Name of the query string.</param>
        /// <returns>true/false</returns>
        private bool AssignValueToHidnFld(string queryStringName)
        {
            string strIds = string.Empty;
            bool blnSucces = true;
            strIds = this.Page.Request.QueryString[queryStringName];
            if(!string.IsNullOrEmpty(strIds))
            {
                strIds = this.Page.Server.UrlDecode(strIds).Replace(';', '|');
                if(IsValidRequest(strIds))
                {
                    hidSelectedRows.Value = strIds;
                    hidSelectedCriteriaName.Value = queryStringName.ToUpperInvariant();
                    //Set Asset name here added in DREAM 4.0
                    if(DisplaySingleAsset)
                        hidSelectedAssetNames.Value = GetAssetNames(strIds, queryStringName);
                }
                else
                {
                    blnSucces = false;
                }

            }
            else
            {
                blnSucces = false;
            }
            return blnSucces;
        }
        /// <summary>
        /// Determines whether [is valid request] [the specified ids].
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>
        /// 	<c>true</c> if [is valid request] [the specified ids]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidRequest(string ids)
        {
            bool blnValidRequest = true;
            string[] arrIds = null;
            if(ids.Contains("|"))
            {
                arrIds = ids.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                arrIds = new string[] { ids };
            }
            foreach(string strId in arrIds)
            {
                try
                {
                    Int64.Parse(strId);
                }
                catch
                {
                    blnValidRequest = false;
                    break;
                }
            }
            //if((arrIds.Length > 1) && (IsMultipleIdsValid()))
            //{
            //    blnValidRequest = false;
            // }
            return blnValidRequest;
        }
        /// <summary>
        /// Determines whether [is multiple ids valid].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is multiple ids valid]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsMultipleIdsValid()
        {
            return (SearchName.ToString().ToLowerInvariant().Equals(MECHANICALDATA) || SearchName.ToString().ToLowerInvariant().Equals(EDMREPORT) || SearchName.ToString().ToLowerInvariant().Equals(SIGNIFICANTWELLEVENTS) || SearchName.ToString().ToLowerInvariant().Equals(WELLREVIEWS));
        }
        /// <summary>
        /// Handles the SelectedIndexChanged event of the cboFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void cboFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(HttpContext.Current.Request.Form[this.UniqueID + FILTERDROPDOWN] != null)
            {
                strPicksFilter = HttpContext.Current.Request.Form[this.UniqueID + FILTERDROPDOWN].ToString();
            }
        }

        #endregion

        #region Supporting Method
        /// <summary>
        /// Gets the request ID from result.
        /// </summary>
        private void GetRequestIDFromResult()
        {
            if(SearchName.ToString().ToLowerInvariant().Equals(EPCATALOG) || SearchName.ToString().ToLowerInvariant().Equals(PVTREPORT))
            {
                hidRequestID.Value = (xmlDocSearchResult.SelectSingleNode("ResultSet/requestid") != null ? xmlDocSearchResult.SelectSingleNode("ResultSet/requestid").InnerText : string.Empty);
            }
            else
            {
                XmlNode xmlNode = xmlDocSearchResult.SelectSingleNode("response/@requestid");
                if(xmlNode != null)
                {
                    hidRequestID.Value = xmlNode.Value;
                }
            }
        }

        /// <summary>
        /// Sets the view mode.
        /// </summary>
        /// 
        /// 
        private void SetViewMode()
        {
            if(string.Equals(ViewMode.ToString(), TABULARDATASHEET))
            {
                if(!string.Equals(strDisplayType.ToUpperInvariant(), DATASHEET))
                {
                    base.ResponseType = TABULAR;
                }
                else
                {
                    base.ResponseType = DATASHEETREPORT;
                }
            }
            else
            {
                if(ViewMode.ToString().ToLowerInvariant().Equals(DATASHEETREPORT.ToLowerInvariant()))
                    base.ResponseType = DATASHEETREPORT;
                else
                    base.ResponseType = TABULAR;
            }
        }
        /// <summary>
        /// Determines whether [is post back by control] [the specified control id].
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns>
        /// 	<c>true</c> if [is post back by control] [the specified control id]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsPostBackByControl(Control control)
        {
            bool blnPostBackByControl = false;
            if((control != null) && (objCommonUtility.GetPostBackControl(this.Page) != null) && (objCommonUtility.GetPostBackControl(this.Page).ID.Equals(control.ID)))
            {
                blnPostBackByControl = true;
            }
            return blnPostBackByControl;
        }
        /// <summary>
        /// Gets the PVT product types.
        /// </summary>
        /// <param name="PVTReport">The EPCatalog report.</param>
        /// <returns>ArrayList of ProductTypes</returns>
        private ArrayList GetPVTProductTypes(string PVTReport)
        {
            ArrayList arrlstProductType = null;
            DataTable dtProductTypes = null;
            string strProductType = string.Empty;
            string strListName = "EP Catalog Report";
            string strCamlQuery = "<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>" + PVTReport + "</Value></Eq></Where>";
            string strViewFields = @"<FieldRef Name='Title'/><FieldRef Name='Product_x0020_Type'/>";

            try
            {
                dtProductTypes = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, strListName, strCamlQuery, strViewFields);
                if(dtProductTypes != null && dtProductTypes.Rows[0]["Product_x0020_Type"] != null)
                {
                    strProductType = (string)dtProductTypes.Rows[0]["Product_x0020_Type"];
                }
                if(!string.IsNullOrEmpty(strProductType))
                {
                    string[] arrProductType = strProductType.Split(";#".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    arrlstProductType = new ArrayList();
                    for(int intProductTypeCounter = 0; intProductTypeCounter < arrProductType.Length; intProductTypeCounter += 2)
                    {
                        if(!string.IsNullOrEmpty(arrProductType[intProductTypeCounter]))
                        {
                            arrlstProductType.Add(arrProductType[intProductTypeCounter]);
                        }
                    }
                }
            }
            finally
            {
                if(dtProductTypes != null)
                {
                    dtProductTypes.Dispose();
                }
            }
            return arrlstProductType;
        }
        /// <summary>
        /// Sets the active div.
        /// </summary>
        private void SetActiveDiv()
        {
            switch(SearchName.ToString().ToLowerInvariant())
            {
                case PALEOMARKERSREPORT:
                    strActiveDiv = "CDSPAL";
                    break;
                case MECHANICALDATA:
                    strActiveDiv = "holesection";
                    break;
                case EDMREPORT:
                    strActiveDiv = "tab-0";
                    break;
                /// ====================================
                /// Module Name:Well Test Data
                /// Description: Set active Div control
                /// Date:16-July-2010
                /// Modified Lines:2740-2742
                /// ====================================
                case WELLTESTREPORT:
                    strActiveDiv = "generaltestdata";
                    break;
                case PRESSURESURVEYDATA:
                    strActiveDiv = "PressureSurvey";
                    break;
            }
        }
        /// <summary>
        /// Enables the disable export.
        /// </summary>
        /// <param name="searchName">Name of the search.</param>
        private void EnableDisableExport(string searchName)
        {
            if(string.Equals(SearchName.ToString(), WELLBOREHEADER) || string.Equals(SearchName.ToString(), FIELDHEADER) || string.Equals(SearchName.ToString(), RESERVOIRHEADER) || string.Equals(SearchName.ToString(), PARSDETAIL) || (SearchName.ToString().ToLowerInvariant().Equals(MECHANICALDATA)) || (SearchName.ToString().ToLowerInvariant().Equals(WELLTESTREPORT)))
            {
                if((searchName.ToLowerInvariant().Equals(SearchName.ToString().ToLowerInvariant())) && (strDisplayType.ToUpperInvariant().Equals(DATASHEET)))
                {
                    this.ExportPage = false;
                    this.ExportAll = false;
                }
                else
                {
                    this.ExportPage = true;
                    this.ExportAll = true;
                }
            }
        }

        /// <summary>
        /// Sets the hidden field values.
        /// </summary>
        private void SetHiddenFieldValues()
        {
            if(HttpContext.Current.Request.Form["hidSelectedRows"] != null)
            {
                hidSelectedRows.Value = HttpContext.Current.Request.Form["hidSelectedRows"].ToString();
            }
            if(HttpContext.Current.Request.Form["hidSelectedCriteriaName"] != null)
            {
                hidSelectedCriteriaName.Value = HttpContext.Current.Request.Form["hidSelectedCriteriaName"].ToString();
            }
            //Added in DREAM 4.0
            //if(!objCommonUtility.IsPostBack(this.Page))
            {
                hidSelectedAssetNames.Value = objCommonUtility.GetFormControlValue("hidSelectedAssetNames");
            }
            if(HttpContext.Current.Request.Form[HIDREQUESTID] != null)
            {
                hidRequestID.Value = HttpContext.Current.Request.Form[HIDREQUESTID].ToString();
            }
            if(SearchName.ToString().ToLowerInvariant().Equals(RECALLCURVE) && !objCommonUtility.IsPostBack(this.Page))
            {
                hidUWBI.Value = objCommonUtility.GetFormControlValue("hidUWBI");
                hidLogService.Value = objCommonUtility.GetFormControlValue("hidLogService");
                hidLogType.Value = objCommonUtility.GetFormControlValue("hidLogType");
                hidLogSource.Value = objCommonUtility.GetFormControlValue("hidLogSource");
                hidLogName.Value = objCommonUtility.GetFormControlValue("hidLogName");
                hidLogActivity.Value = objCommonUtility.GetFormControlValue("hidLogActivity");
                hidLogrun.Value = objCommonUtility.GetFormControlValue("hidLogrun");
                hidLogVersion.Value = objCommonUtility.GetFormControlValue("hidLogVersion");
                hidProjectName.Value = objCommonUtility.GetFormControlValue("hidProjectName");
            }
            if(HttpContext.Current.Request.Form["hidAssetName"] != null)
            {
                hidAssetName.Value = HttpContext.Current.Request.Form["hidAssetName"];
            }
        }
        /// <summary>
        /// Determines whether [is report need fresh request].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is report need fresh request]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsReportNeedFreshRequest()
        {
            string strSearchName = SearchName.ToString().ToLowerInvariant();
            if(strSearchName.Equals(RECALLLOGS.ToLowerInvariant()) ||
                strSearchName.Equals(RECALLCURVE.ToLowerInvariant()) ||
                strSearchName.Equals(EDMREPORT.ToLowerInvariant())
                || strSearchName.Equals(PALEOMARKERSREPORT.ToLowerInvariant()) || strSearchName.Equals(PRESSURESURVEYDATA.ToLowerInvariant()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Sets the criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        private Criteria SetCriteria(string criteria, string column)
        {
            Criteria objCriteria = new Criteria();
            if(column.Length > 0)
                objCriteria.Name = column;
            objCriteria.Value = criteria;
            objCriteria.Operator = GetOperator(objCriteria.Value);
            //returns the search criteria object.
            return objCriteria;
        }


        #region Dream 3.1 code
        /// <summary>
        /// Sets the control values to U ser preferences.
        /// </summary>
        private void SetControlValuesToUSerPreferences()
        {
            if(!objCommonUtility.IsPostBack(this.Page))
            {
                strDepthUnit = objUserPreferences.DepthUnits;
                //Dream 3.1 fix start
                strDisplayType = objUserPreferences.Display;
                if(string.Equals(ViewMode.ToString(), TABULARDATASHEET))
                {
                    if(rdoViewMode != null)
                    {
                        if(string.Equals(strDisplayType.ToUpperInvariant(), DATASHEET))
                            rdoViewMode.SelectedIndex = 0;
                        else
                            rdoViewMode.SelectedIndex = 1;
                    }
                }
                //Dream 3.1 fix end
                if(TemperatureUnit)
                {
                    if(!string.IsNullOrEmpty(objUserPreferences.TemperatureUnits))
                    {
                        rdoTemperatureUnit.SelectedValue = objUserPreferences.TemperatureUnits;
                    }
                    else
                    {
                        rdoTemperatureUnit.SelectedIndex = 0;
                    }
                    strTemperatureUnit = rdoTemperatureUnit.SelectedItem.Value;
                }
                if(PressureUnit)
                {
                    if(!string.IsNullOrEmpty(objUserPreferences.PressureUnits))
                    {
                        rdoPressureUnit.SelectedValue = objUserPreferences.PressureUnits;
                    }
                    else
                    {
                        rdoPressureUnit.SelectedIndex = 0;
                    }
                    strPressureUnit = rdoPressureUnit.SelectedItem.Value;
                }
            }
            else
            {
                //Dream 3.1 fix start
                if(string.Equals(ViewMode.ToString(), TABULARDATASHEET))
                {
                    if(rdoViewMode != null)
                    {
                        strDisplayType = rdoViewMode.SelectedValue;
                    }
                }
                //Dream 3.1 fix end
                if(UnitConversion)
                {

                    if(rbFeet.Checked)
                    {
                        strDepthUnit = FEET;
                    }
                    else
                    {
                        strDepthUnit = METER;
                    }
                }
                if(TemperatureUnit)
                {
                    strTemperatureUnit = rdoTemperatureUnit.SelectedValue;
                }
                if(PressureUnit)
                {
                    strPressureUnit = rdoPressureUnit.SelectedValue;
                }
            }
        }
        #endregion

        #region Dream 4.0 code
        /// <summary>
        /// Registers the client side script.
        /// </summary>
        private void RegisterClientSideScript()
        {

            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language=\"javascript\">try{");
            switch(SearchName.ToString().ToLowerInvariant())
            {
                case EDMREPORT:
                    strScript.Append("tabs('" + strActiveDiv + "');");
                    break;
                case WELLTESTREPORT:
                case MECHANICALDATA:
                    strScript.Append("MDRTabClick('" + strActiveDiv + "');AddLastSelectedValue();");
                    break;
                case PRESSURESURVEYDATA:
                    strScript.Append("MDRTabClick('" + strActiveDiv + "');");
                    break;
                case SIGNIFICANTWELLEVENTS:
                    strScript.Append("FixColWidth('tblSearchResults');CarriageReturnHandling('Remarks ','tblSearchResults');");
                    break;
                default:
                    strScript.Append("ApplyReorder();");
                    break;
            }
            strScript.Append("}catch(Ex){}</script>");
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ContextSearchResultsOnLoad", strScript.ToString(), false);
        }
        /// <summary>
        /// Gets the significant well events result.
        /// </summary>
        //private void GetSignificantWellEventsResult()
        //{
        //    XmlDocument xmlSearchRequest = new XmlDocument();
        //    if(CommonUtility.GetSessionVariable(Page, SIGNIFICANTWELLEVENTSREQUESTXML) != null)
        //    {
        //        xmlSearchRequest.LoadXml((string)CommonUtility.GetSessionVariable(Page, SIGNIFICANTWELLEVENTSREQUESTXML));
        //        xmlDocSearchResult = objReportController.GetSearchResults(xmlSearchRequest, intMaxRecords, SearchName.ToString().ToUpperInvariant(), null, intSortOrder);
        //    }
        //}
        /// <summary>
        /// Gets the well review result.
        /// </summary>
        //private void GetWellReviewResult()
        //{
        //    XmlDocument xmlSearchRequest = new XmlDocument();
        //    xmlSearchRequest.LoadXml((string)CommonUtility.GetSessionVariable(Page, EDMREPORTREQUESTXML));
        //    if(xmlSearchRequest != null)
        //    {
        //        xmlDocSearchResult = objReportController.GetSearchResults(xmlSearchRequest, intMaxRecords, SearchName.ToString().ToUpperInvariant(), null, intSortOrder);
        //    }
        //}

        /// <summary>
        /// Gets the response with filter criteria.
        /// </summary>
        private void GetSearchWithFilterCriteriaResponse()
        {
            switch(SearchName.ToString().ToLowerInvariant())
            {
                case WELLTESTREPORT:
                    {
                        GetWellTestReportResponse();
                        break;
                    }
                case EDMREPORT:
                    {
                        /// Gets the result for EDM Report
                        GetEDMReportResult();
                        break;
                    }
                default:
                    {
                        //SignificantWellEvents
                        //PressureSurveyData
                        GetGenericWithFilterCriteriaResponse();
                        break;
                    }
            }
        }
        /// <summary>
        /// Gets the generic with filter criteria response.
        /// </summary>
        private void GetGenericWithFilterCriteriaResponse()
        {
            XmlDocument xmlDocSearchRequest = null;
            if(objFilterControl != null && objFilterControl.RequestXML != null)
            {
                xmlDocSearchRequest = objFilterControl.RequestXML;//Geting request xml generated by filter usercontrol 
            }
            if(xmlDocSearchRequest != null)
            {
                AddEntityAttributes(xmlDocSearchRequest);
                if(EnableSkipCount)
                {
                    SetSkipInfo(xmlDocSearchRequest);
                }
                xmlDocSearchResult = objReportController.GetSearchResults(xmlDocSearchRequest, intMaxRecords, SearchName.ToString(), strSortColumn, intSortOrder);
            }
        }
        /// <summary>
        /// Loads the assets.
        /// </summary>
        private void LoadAssets()
        {
            objCommonUtility.PopulateAssetListControl(ddlAssets, objCommonUtility.GetPipeSeperatedStrAsArray(hidSelectedAssetNames.Value), objCommonUtility.GetPipeSeperatedStrAsArray(hidSelectedRows.Value));
        }
        /// <summary>
        /// Initializes the DDL assets.
        /// </summary>
        private void CreateDDLAssets()
        {
            ddlAssets = new DropDownList();
            ddlAssets.ID = "ddlAssets";
            ddlAssets.EnableViewState = true;
            ddlAssets.AutoPostBack = true;
            this.Controls.Add(ddlAssets);
        }


        /// <summary>
        /// Gets the asset names.
        /// </summary>
        /// <param name="uniqueIds">The unique ids.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        private string GetAssetNames(string uniqueIds, string criteria)
        {
            #region Decleration
            XmlDocument xmlDocAssetNameResponse = null;
            XmlNode xmlNodeAssetName = null;
            string[] arrIds = null;
            string strAssetNames = string.Empty;
            string strAssetType = string.Empty;
            string strAssetNameXPath = string.Empty;
            string strAssetColName = string.Empty;
            #endregion

            arrIds = uniqueIds.Contains("|") ? (uniqueIds.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)) : (new string[] { uniqueIds });

            strAssetType = criteria.ToLowerInvariant().Equals(UWI.ToLowerInvariant()) ? ("Well") : ("Wellbore");

            strAssetColName = GetAssetNameCol(strAssetType);

            strAssetNameXPath = "response/report/record[(attribute/@name='" + criteria + "') and (attribute/@value='%ID%')]/attribute[@name='" + strAssetColName + "']/@value";
            xmlDocAssetNameResponse = GetAssetNameResponse(arrIds, strAssetType);
            xmlNodeAssetName = null;
            if(xmlDocAssetNameResponse != null)
            {
                foreach(string strId in arrIds)
                {
                    xmlNodeAssetName = xmlDocAssetNameResponse.SelectSingleNode(strAssetNameXPath.Replace("%ID%", strId));
                    if(xmlNodeAssetName != null)
                    {
                        strAssetNames += xmlNodeAssetName.Value + "|";
                    }
                }
            }
            return strAssetNames;
        }
        /// <summary>
        /// Gets the asset name response.
        /// </summary>
        /// <param name="uniqueIds">The unique ids.</param>
        /// <returns></returns>
        private XmlDocument GetAssetNameResponse(string[] uniqueIds, string assetType)
        {
            XmlDocument xmlDocAssetNameResponse = null;
            string strOperator = string.Empty;
            //Creating local variable of reportsevice manage because it always has to be a reportservice manager
            AbstractController objReportController = objFactory.GetServiceManager(REPORTSERVICE);
            objRequestInfo = new RequestInfo();
            Entity objEntity = new Entity();
            objRequestInfo.Entity = objEntity;
            objEntity.ResponseType = TABULAR;
            objEntity.Attribute = new ArrayList();
            strOperator = objCommonUtility.GetOperator(uniqueIds);
            objEntity.Attribute.Add(AddAttribute(hidSelectedCriteriaName.Value, strOperator, uniqueIds));
            xmlDocAssetNameResponse = objReportController.GetSearchResults(objRequestInfo, -1, assetType, null, intSortOrder);
            return xmlDocAssetNameResponse;
        }
        /// <summary>
        /// Gets the reponse for single asset report.
        /// </summary>
        private void GetSingleAssetReportReponse()
        {
            switch(SearchName.ToString().ToLowerInvariant())
            {
                case MECHANICALDATA:
                    {
                        GetMechanicalDataResponse();
                        break;
                    }
                default:
                    {
                        //WellSummary
                        //WellReviews
                        //WellHistory
                        GetGenericSingleAssetReportReponse();
                        break;
                    }
            }
        }
        /// <summary>
        /// Gets the generic single asset report reponse.
        /// </summary>
        private void GetGenericSingleAssetReportReponse()
        {
            objRequestInfo = new RequestInfo();
            Entity objEntity = new Entity();
            objRequestInfo.Entity = objEntity;
            objEntity.Name = EntityName;
            objEntity.ResponseType = ResponseType;
            objEntity.Attribute = new ArrayList();
            objEntity.Attribute.Add(AddAttribute(hidSelectedCriteriaName.Value, EQUALSOPERATOR, new string[] { ddlAssets.SelectedValue }));
            if(EnableSkipCount)
            {
                SetSkipInfo(objRequestInfo.Entity);
            }
            xmlDocSearchResult = objReportController.GetSearchResults(objRequestInfo, intMaxRecords, SearchName.ToString(), null, intSortOrder);
        }
        #endregion
        #endregion

    }
}

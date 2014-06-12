#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: UIHelper.cs
#endregion

using System;
using System.Data;
using System.Collections;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web;
using System.Xml;
using System.Text.RegularExpressions;
using Microsoft.SharePoint;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Utilities;
using System.IO;
using System.Text;
using Telerik.Web.UI;

namespace Shell.SharePoint.DREAM.Site.UI
{

    /// <summary>
    /// The UIHelper Class, helper class for the UI Search Pages.
    /// </summary>
    public class UIHelper :System.Web.UI.UserControl
    {
        #region Declaration
        protected const string FIELDSEARCHTYPE = "Field Advance Search";
        protected const string SRPFIELDSEARCHTYPE = "SRP Field Advance Search";
        protected const string SEARCHTYPE = "PARSADVSEARCH";
        //SRP
        protected const string RESERVOIRSEARCHTYPE = "Reservoir Advance Search";
        protected const string STARTDATENODE = "STARTDATE";
        protected const string ENDDATENODE = "ENDDATE";
        protected const string WEEKNODE = "week";
        protected const string MONTHNODE = "month";
        protected const string YEARNODE = "year";
        protected const string SURFACE = "Surface";
        protected const string BOTTOM = "Bottom";
        protected const string TIMESTAMPNODE = "TIMESTAMP";
        protected const string XMMINLATNODE = "XMMINLAT";
        protected const string XMMAXLATNODE = "XMMAXLAT";
        protected const string XMMINLONNODE = "XMMINLON";
        protected const string XMMAXLONNODE = "XMMAXLON";
        protected const string SOAPEXCEPTIONMESSAGE = "No records were found that matched your search parameters.Please modify your parameters and run the search again.";
        protected const string UNEXPECTEDEXCEPTIONMESSAGE = "Unexpected error";
        protected const string NORECORDFOUNDSEXCEPTIONMESSAGE = "No Records Found";
        protected const string COUNTRYLIST = "Country";
        protected const string COUNTRY = "Country";
        protected const string BASINLIST = "Basin";
        protected const string SPUDDATE = "spud_date";
        protected const string LISTVALUES = "LISTVALUES";
        protected const string ADVANCESEARCH = "ADVANCESEARCH";
        protected const string QUERYSEARCH = "Query Search";
        protected const string ADVANCEDSEARCH = "Advanced Search";
        //Added for Dream 2.1
        protected const string WELLADVANCEDSEARCH = "Well Advance Search";
        protected const string WELLBOREADVANCEDSEARCH = "Wellbore Advance Search";
        protected string strSiteURL = string.Empty;
        //end
        //SRP start
        protected const string GRAINSIZEMEANLIST = "Grain Size Mean";
        protected const string LITHOLOGYMAINLIST = "Lithology Main";
        protected const string LITHOLOGYSECONDARYLIST = "Lithology Secondary";
        protected const string LITHOSTRATGROUPLIST = "Reservoir Lithostrat Group";
        protected const string LITHOSTRATFORMATION = "Reservoir Lithostrat Formation";
        protected const string LITHOSTRATMEMBER = "Reservoir Lithostrat Member";
        protected const string PRODUCTIONSTATUSLIST = "Production Status";
        protected const string DRIVEMECHANISMLIST = "Drive Mechanism";
        protected const string HYDROCARBONMAINLIST = "HydroCarbon Main";
        protected const string HYDROCARBONSECONDARYLIST = "HydroCarbon Secondary";
        protected const string OPERATIONALENVLIST = "Operational Environment";
        protected const string RESERVEMAGNITUDEOILLIST = "Reserve Magnitude Oil";
        protected const string RESERVEMAGNITUDEGASLIST = "Reserve Magnitude Gas";
        protected const string TECHONICSETTINNGSLIST = "Tectonic Setting";
        protected const string SEARCHPERCENTAGEVALUE = "SearchPercentageValue";
        protected const string ENABLESRPSPECIFICCONTROLS = "EnableSRPSpecificControls";
        protected const string LITHOSTRATGROUPDATAFIELD = "LithostratGroup";
        protected const string LITHOSTRATFORMATIONDATAFIELD = "LithostratFormation";
        protected const string LITHOSTRATMEMBERDATAFIELD = "LithostratMember";
        //<TODO> Change the XSLFILE Name 
        protected const string SRPXSLFILENAME = "RadTransformer";
        protected const string JAVASCRIPTONCLICK = "onclick";
        protected const string SRPTEXTBOXALLOWEDDECIMALS = "noOfDecimals";
        protected const string SRPTEXTBOXISRANGEALLOWED = "isRange";
        protected const string SRPTEXTBOXMINIMUMVALUE = "minimumValue";
        protected const string SRPTEXTBOXMAXIMUMVALUE = "maximumValue";
        protected const string SRPTEXTBOXLABELNAME = "lableName";
        protected const string SRPTEXTBOXMEASURMENTS = "Measuments";
        protected const string SRPTEXTBOXDEPENDENTID = "dependentId";
        protected const string SRPADVPOPUPID = "ControlId";
        protected const string SRPADVPOPUPBASIN = "Basin";
        protected const string SRPADVPOPUPOPERATOR = "Operator";

        //end
        protected const string SEARCHLEVEL = "1";
        protected const string FROM = "From";
        protected const string TO = "To";
        protected const string KICKOFFDATE = "Kickoff_Date";
        protected const string DATETYPE = "Date_Type";
        protected const string COMPLETIONDATE = "completion_date";
        protected const string ABANDONEDDATE = "abandonment_date";
        protected const string GREATERTHANEQUALS = "GTEQ";
        protected const string LESSTHANEQUALS = "LTEQ";
        protected const string BOTTOMLATITUDE = "Preferred_bh_latitude";
        protected const string BOTTOMLONGITUDE = "Preferred_bh_longitude";
        protected const string SURFACELATITUDE = "Preferred_latitude";
        protected const string SURFACELONGITUDE = "Preferred_longitude";
        protected const string MINLATITUDE = "MinLatitude";
        protected const string MAXLATITUDE = "MaxLatitude";
        protected const string MINLONGITUDE = "MinLongitude";
        protected const string MAXLONGITUDE = "MaxLongitude";
        protected const string STAROPERATOR = "*";
        protected const string AMPERSAND = "%";
        protected const string QUESTIONMARKOPERATOR = "?";
        protected const string EQUALSOPERATOR = "EQUALS";
        protected const string DATEVALUE = "Spud";
        protected const string INOPERATOR = "IN";
        protected const string LIKEOPERATOR = "LIKE";
        protected const string ANDOPERATOR = "AND";
        //SRP
        protected const string OROPERATOR = "OR";
        protected const string ALLCOLUMNS = "*";
        //end
        protected const string TRUE = "true";
        protected const string FALSE = "false";
        protected const string CHECKED = "checked";
        protected const string SAVESEARCHXSLPATH = "/saveSearchRequests/saveSearchRequest/requestinfo/entity";
        protected const string SAVESHAREDSEARCHXSLPATH = "/saveSearchRequests/saveSearchRequest";
        protected const string ENTITYPATH = "requestinfo/entity";
        protected const string VALUEPATH = "requestinfo/entity/display/value";
        protected const string QUERYPATH = "requestinfo/entity/query";
        protected const string ATTRIBSHARED = "shared";
        protected const string ATTRITYPE = "type";
        protected const string ATTRIBNAME = "name";
        protected const string PARSSEARCHTYPE = "PARS";
        protected const string LOGSFIELDSEARCHTYPE = "Logs By Field Depth";
        protected const string LABEL = "label";
        protected const string ATTRIBUTEGROUP = "attributegroup";
        protected const string ATTRIBUTE = "attribute";
        protected const string VALUE = "value";
        protected const string PARAMETER = "parameter";
        protected const string PARAMETERXPATH = "/parameter";
        protected const string ATTRIBUTEGROUPXPATH = "/attributegroup/attributegroup";
        protected const string ATTRIBUTEGROUPPATH = "requestinfo/entity/attributegroup";
        protected const string ATTRIBUTEXPATH = "/attribute";
        protected const string FIELD = "Fields";
        protected const string CHECKREFRESH = "CheckRefresh";
        //srp
        protected const string FIELDOPERATOR = "Field Operator";
        protected const string FIELDNAME = "FieldName";
        protected const string FIELDVALUE = "FieldValue";
        protected const string TOOLTIPPOPUPFUNCTION = "OpenToolTipPopup('{0}');";
        //end
        protected const string STATE = "State or Province";
        /// <summary>
        /// Added for WDA Requirement Enhancement on 31-May-2010
        /// </summary>
        protected const string APIIDENTIFIER = "APIIdentifiers";
        /// <summary>
        /// Added for WDA Requirement Enhancement on 31-May-2010
        /// </summary>
        protected const string BLOCK = "Block";
        protected const string COUNTY = "County";
        protected const string FORMATION = "Formation";
        protected const string ONSHOREOFFSHORE = "Onshore Offshore";
        protected const string KIND = "Wellbore Kind";
        protected const string WELLBORESTATUS = "Wellbore Status";
        protected const string SINGLEATTRIBUTEGROUPXPATH = "/attributegroup";
        protected const string DATASOURCE = "DataSource";
        protected const string DATASOURCENAME = "DataSourceName";
        protected const string DATAPROVIDER = "DataProvider";
        protected const string TABLENAME = "TableName";
        protected const string MODIFYSRCH = "Modify Search";
        protected const string MODIFYSQL = "Modify Sql Query";
        protected const string SAVESRCH = "Save Search";
        protected const string SAVESQL = "Save Sql Query";
        protected const string EDITCRITERIA = "Edit Criteria";
        protected const string EDITSQL = "Edit Sql";
        protected const string MOSSSERVICE = "MossService";
        protected const string REPORTSERVICE = "ReportService";
        protected const string QUERYSERVICE = "QueryService";
        protected const string RESOURCEMANAGER = "ResourceManager";
        protected const string ASSETTYPE = "assetType";
        protected const string DEFAULTDROPDOWNTEXT = "---Select---";
        protected const string TABULAR = "Tabular";
        protected const string DATASHEET = "datasheet";
        protected const string HIERARCHICAL = "Hierarchical";
        protected const string REPORTSERVICECOLUMNLIST = "Report Service Layer Columns";
        protected const string ALLCOLUMNSTEXT = "--All Columns--";
        protected const string WELLITEMVAL = "Well";
        protected const string WELLBOREITEMVAL = "Wellbore";
        protected const string PARSITEMVAL = "Project Archives";
        protected const string BASINITEMVAL = "Basin";
        protected const string BASINADVITEMVAL = "BasinSearch";
        protected const string FIELDITEMVAL = "Field";
        protected const string RESERVOIRITEMVAL = "Reservoir";
        protected const string PARSTITLE = "Title";
        protected const string PARSDESCRIPTION = "Description";
        protected const string PARSPROJECTNAME = "Project Name";
        protected const string UWI = "Unique Well Identifier";
        protected const string UWBI = "Unique Wellbore Identifier";
        protected const string USERREGISTRATIONLIST = "User Registration";
        protected const string USERACCESSREQUESTLIST = "User Access Request";
        protected const string PROJECTLIST = "Project";
        protected const string TEAMREGISTRATIONLIST = "Team Registration";
        protected const string MAPBOOKMARKSLIST = "MapBookmarks";
        protected const string DREAMADMINPERMISSION = "DreamAdmin";
        protected const string TEAMOWNERPERMISSION = "TeamOwner";
        protected const string NONREGUSERPERMISSION = "NonRegUser";
        protected const string STAFFPERMISSION = "StaffUser";
        protected const string ADMINUSER = "Administrator";
        protected const string DEPOSITIONALENVLIST = "Depositional Environment";


        protected int intListValMaxRecord = 65000;
        protected string strCurrSiteUrl = HttpContext.Current.Request.Url.ToString();
        protected string MYASSETPATTERN = @"\r\n";
        protected const string BLANKFILEMESSAGE = "The file you have selected is empty. ";
        /// <summary>
        /// Added for WDA Requirement Enhancement on 31-May-2010
        /// </summary>
        protected const string APIAREANOTFOUNDMESSAGE = "No API Area has been found for the selected country.";
        /// <summary>
        /// Added for WDA Requirement Enhancement on 31-May-2010
        /// </summary>
        protected const string BLOCKNOTFOUNDMESSAGE = "No Block has been found for the selected API Identifier.";
        protected RequestInfo objRequestInfo = new RequestInfo();
        protected UserPreferences objUserPreferences;
        protected AbstractController objMossController;
        protected AbstractController objReportController;
        protected ResourceServiceManager objResourceController;
        protected ServiceProvider objFactory = new ServiceProvider();
        protected UIUtilities objUIUtilities;
        protected CommonUtility objUtility = new CommonUtility();
        protected DataTable dtListValues = new DataTable();
        //Dream 4.0 
        //start
        protected AbstractController objEventServiceController;
        protected DateTimeConvertorService objDateTimeConvertorService = new DateTimeConvertorService();
        protected const string WEBSERVICEDATEFORMAT = "yyyy-MM-dd";
        protected const string EVENTSERVICE = "EVENTSERVICE";
        protected const string EVENTTARGET = "__EVENTTARGET";
        protected const string REDIRECTATTRIBUTE = "redirect";
        protected const string REDIRECTATTRIBUTEVALUE = "true";
        protected const string RESIZEWINDOWATTRIBUTE = "resizeWindow";
        protected const string BASINXPATH = "response/report/record/attribute[@name = 'Basin Name' and @value != '']/@value";
        protected const string BASINNAME = "Basin Name";
        protected const string BASINIDENTIFIER = "Basin Identifier";
        protected const string SEARCHRESULTSPAGE = "/pages/AdvancedSearchResults.aspx";
        //end

        //For List Values
        public int intMaxRecords = -1;
        #endregion

        #region Public methods
        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            string strUserName = string.Empty;
            string strParentSiteURL = string.Empty;
            strUserName = objUtility.GetSaveSearchUserName();
            return strUserName;
        }

        /// <summary>
        /// Gets the email  of the Administrator.
        /// </summary>
        /// <returns></returns>
        public string GetAdminEmailID()
        {
            string strAdminEmail = string.Empty;
            strAdminEmail = objUtility.GetAdminEmailID();
            return strAdminEmail;
        }

        #endregion

        #region Protected Methods
        /// <summary>
        /// Sets the textbox default.
        /// </summary>
        /// <param name="txt">The TXT.</param>
        protected void SetTextboxDefault(TextBox txt)
        {
            txt.Text = "0";
        }
        /// <summary>
        /// Gets the asset columns.
        /// </summary>
        protected void GetAssetColumns(string listName, ListControl cboAssetColumn, string asset)//Dream4.0 changes replaced listbox control type to listcontrol
        {
            DataTable objListData = null;
            DataRow objListRow = null;
            objReportController = objFactory.GetServiceManager(MOSSSERVICE);
            string strColumnName = string.Empty;
            string strDisplayName = string.Empty;
            string strParentSiteURL = SPContext.Current.Site.Url.ToString();
            string strCamlQueryAssetColumns = string.Empty;
            try
            {
                //Modification for list tunning dated - 09/feb/2009
                strCamlQueryAssetColumns = GetCamlQueryColumnList(strCamlQueryAssetColumns, asset);
                objListData = ((MOSSServiceManager)objReportController).ReadList(strParentSiteURL, listName, strCamlQueryAssetColumns);
                if(objListData.Rows.Count > 0)
                {
                    cboAssetColumn.Items.Clear();
                    cboAssetColumn.Items.Add(DEFAULTDROPDOWNTEXT);
                    cboAssetColumn.Items[0].Value = string.Empty;
                    for(int index = 0; index < objListData.Rows.Count; index++)
                    {
                        objListRow = objListData.Rows[index];
                        strColumnName = objListRow["Title"].ToString();
                        strDisplayName = objListRow["Display_x0020_Name"].ToString();
                        cboAssetColumn.Items.Add(strDisplayName);
                        cboAssetColumn.Items[index + 1].Value = strColumnName;
                    }
                }
                objListData.Clear();
            }
            finally
            {
                if(objListData != null)
                    objListData.Dispose();
            }
        }


        /// <summary>
        /// Sets the date value.
        /// </summary>
        /// <param name="selectedDate">The selected date.</param>
        /// <param name="fromValue">From value.</param>
        /// <param name="toValue">To value.</param>
        /// <returns></returns>
        protected ArrayList SetDateValue(string selectedDate, string fromValue, string toValue)
        {
            ArrayList arrDate = new ArrayList();
            Value objDate = new Value();
            if(string.Equals(selectedDate, FROM))
            {
                objDate.InnerText = objDateTimeConvertorService.GetDateInFormat(fromValue, WEBSERVICEDATEFORMAT);
            }
            else
            {
                objDate.InnerText = objDateTimeConvertorService.GetDateInFormat(toValue, WEBSERVICEDATEFORMAT);
            }
            arrDate.Add(objDate);
            return arrDate;
        }

        /// <summary>
        /// Determines whether [is duplicate name exist] [the specified list of save search].
        /// </summary>
        /// <param name="listOfSaveSearch">The list of save search.</param>
        /// <param name="saveSearchName">Name of the save search.</param>
        /// <returns>
        /// 	<c>true</c> if [is duplicate name exist] [the specified list of save search]; otherwise, <c>false</c>.
        /// </returns>
        protected bool IsDuplicateNameExist(ArrayList listOfSaveSearch, string saveSearchName)
        {
            bool blnIsDuplicateNameExist = false;
            foreach(string strSaveSearchName in listOfSaveSearch)
            {
                if(string.Compare(strSaveSearchName, saveSearchName, true) == 0)
                {
                    blnIsDuplicateNameExist = true;
                    break;
                }
            }
            return blnIsDuplicateNameExist;
        }

        /// <summary>
        /// Gets the logical operator.
        /// </summary>
        /// <returns></returns>
        protected string GetLogicalOperator()
        {
            string strOperator;
            strOperator = ANDOPERATOR;
            return strOperator;
        }

        /// <summary>
        /// Gets the query operator for request xml
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected string GetOperator(ArrayList value)
        {
            string strOperator = string.Empty;
            if(value.Count > 1)
            {
                strOperator = INOPERATOR;
            }
            else
            {
                foreach(Value objValue in value)
                {
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

        //SRP code
        /// <summary>
        /// GetLogicalOperator.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected string GetLogicalOperator(string value)
        {
            string strOperator = string.Empty;

            if(string.Equals(value.ToUpper(), ANDOPERATOR))
            {
                strOperator = ANDOPERATOR;
            }
            else
            {
                strOperator = OROPERATOR;
            }
            return strOperator;
        }

        /// <summary>
        /// GetQueryTextOperator.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected string GetQueryTextOperator(ArrayList value)
        {
            string strOperator = string.Empty;
            if(value.Count > 1)
            {
                strOperator = INOPERATOR;
            }
            else
            {
                foreach(Value objValue in value)
                {
                    if((objValue.InnerText.Contains(STAROPERATOR)) || (objValue.InnerText.Contains(AMPERSAND)) || (objValue.InnerText.Contains(QUESTIONMARKOPERATOR)))
                        strOperator = LIKEOPERATOR;
                    else
                        strOperator = EQUALSOPERATOR;
                }
            }
            return strOperator;
        }

        /// <summary>
        /// Gets the date operator.
        /// </summary>
        /// <param name="selectedDate">The selected date.</param>
        /// <returns></returns>
        protected string GetDateOperator(string selectedDate)
        {
            string strOperator;
            if(string.Equals(selectedDate, FROM))
            {
                strOperator = GREATERTHANEQUALS;
            }
            else
            {
                strOperator = LESSTHANEQUALS;
            }
            return strOperator;
        }

        /// <summary>
        /// Gets the lat lon ID.
        /// </summary>
        /// <param name="controlID">The control ID.</param>
        /// <returns></returns>
        protected string GetLatLonID(string controlID)
        {
            return controlID.Substring(controlID.Length - 3, 3);
        }

        /// <summary>
        /// Gets the control ID.
        /// </summary>
        /// <param name="strControlID">The STR control ID.</param>
        /// <returns></returns>
        protected string GetControlID(string controlID)
        {
            return controlID.Remove(0, 3).ToLower();
        }

        /// <summary>
        /// Gets the control ID.
        /// </summary>
        /// <param name="strControlID">The STR control ID.</param>
        /// <returns></returns>
        protected string GetRadControlID(string controlID)
        {
            return controlID.Remove(0, 6).ToLower();
        }

        /// <summary>
        /// Gets the nodeID from control.
        /// </summary>
        /// <param name="strControlID">The STR control ID.</param>
        /// <returns></returns>
        protected string GetNodeIDFromControl(string controlID)
        {
            return controlID.Substring(3, controlID.Length - 3);
        }

        /// <summary>
        /// Determines whether [is option selected] [the specified LST control].
        /// </summary>
        /// <param name="lstControl">The LST control.</param>
        /// <returns>
        /// 	<c>true</c> if [is option selected] [the specified LST control]; otherwise, <c>false</c>.
        /// </returns>
        protected bool IsOptionSelected(ListControl listBoxControl)//DREAM4.0 Changes replaced Listbox control type to List control
        {
            bool blnSelected = false;
            if(listBoxControl.SelectedIndex >= 0)
                blnSelected = true;
            return blnSelected;
        }

        /// <summary>
        /// Loads the date format.
        /// </summary>
        protected void LoadRegionFormat(DropDownList dateList)
        {
            LoadDateFormat(dateList, "Date Format");
        }

        /// <summary>
        /// Loads the country basin data.
        /// </summary>
        /// <param name="strListName">Name of the STR list.</param>
        /// <param name="strEntityName">Name of the STR entity.</param>
        /// <param name="lstControl">The LST control.</param>
        protected void LoadCountryBasinData(string listName, string entityName, ListControl listBoxControl)//DREAM4.0 Changes replaced Listbox control type to List control
        {
            try
            {
                DataRow dtRow;

                objUserPreferences = new UserPreferences();
                int intControlIndex = -1;
                string strEntityName = string.Empty;
                string strColumnName = string.Empty;
                string strPreferredCountryBasin = string.Empty;
                string strCamlQuery = "<OrderBy><FieldRef Name=\"Title\"/></OrderBy><Where><Eq>" + "<FieldRef Name=\"Active\" /><Value Type=\"Choice\">Yes</Value></Eq></Where>";

                string strUserId = objUtility.GetUserName();
                objUserPreferences = ((MOSSServiceManager)objMossController).GetUserPreferencesFromSession(Page, strUserId, objUtility.GetParentSiteUrl(strCurrSiteUrl));

                dtListValues.Reset();
                dtListValues = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, listName, strCamlQuery);
                listBoxControl.Items.Clear();

                if(string.Equals(entityName, BASINLIST))
                {
                    if(objUserPreferences.Basin.Length > 0)
                        strPreferredCountryBasin = objUserPreferences.Basin.ToLower();
                    strColumnName = "Title";
                }
                else if(string.Equals(entityName, COUNTRYLIST))
                {
                    if(objUserPreferences.Country.Length > 0)
                        strPreferredCountryBasin = objUserPreferences.Country.ToLower();
                    strColumnName = "Country_x0020_Code";
                }

                if(dtListValues != null)
                {
                    for(int intIndex = 0; intIndex < dtListValues.Rows.Count; intIndex++)
                    {
                        dtRow = dtListValues.Rows[intIndex];
                        if((dtRow["Title"] != null) && (!string.IsNullOrEmpty(dtRow["Title"].ToString())))
                        {
                            strEntityName = dtRow[strColumnName].ToString();

                            if(string.Equals(strEntityName.ToLower(), strPreferredCountryBasin))
                            {
                                intControlIndex = intIndex;
                            }
                            listBoxControl.Items.Add(dtRow["Title"].ToString());
                            if(string.Compare(entityName, COUNTRYLIST, true) == 0)
                            {
                                listBoxControl.Items[intIndex].Value = Convert.ToString(dtRow["Country_x0020_Code"]);
                            }
                        }
                    }
                }
                //DREAM4.0 code added for adv search country dropdown
                if(string.Equals(entityName, BASINLIST))
                {
                    listBoxControl.SelectedIndex = intControlIndex;
                }
                else if(string.Equals(entityName, COUNTRYLIST) && intControlIndex != -1)
                {
                    listBoxControl.SelectedIndex = intControlIndex;
                }
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
            finally
            {
                if(dtListValues != null)
                    dtListValues.Dispose();
            }
        }
        #region SRP Code
        /// <summary>
        /// Gets the names of the basin.
        /// </summary>
        /// <param name="basinname">The basinname.</param>
        /// <returns></returns>
        protected DataTable GetBasinFromSPList(string basinname)
        {

            string strCamlQuery = string.Empty;
            if(!string.IsNullOrEmpty(basinname))
            {
                strCamlQuery = "<Where><And><BeginsWith><FieldRef Name='Title' /> <Value Type='Text'>" + basinname + "</Value></BeginsWith><Eq><FieldRef Name='Active' /> <Value Type='Choice'>yes</Value></Eq></And></Where><OrderBy><FieldRef Name='Title' Ascending='False' />   </OrderBy>";
            }

            MOSSServiceManager objMossController = (MOSSServiceManager)objFactory.GetServiceManager(MOSSSERVICE);
            dtListValues.Reset();
            dtListValues = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, BASINLIST, strCamlQuery);
            return dtListValues;

        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="listName">Name of the list.</param>
        /// <param name="filterValue">The filter value.</param>
        /// <returns></returns>
        protected DataTable GetList(string listName, string filterValue)
        {

            string strCamlQuery = string.Empty;
            if(!string.IsNullOrEmpty(filterValue))
            {
                strCamlQuery = "<Where><And><Contains><FieldRef Name='Title' /> <Value Type='Text'>" + filterValue + "</Value></Contains><Eq><FieldRef Name='Active' /> <Value Type='Choice'>yes</Value></Eq></And></Where><OrderBy><FieldRef Name='Title' Ascending='False' />   </OrderBy>";
            }
            else if(string.Equals(filterValue, DEPOSITIONALENVLIST))
            {

                strCamlQuery = "<Where><Eq><FieldRef Name='Active' /><Value Type='Choice'>Yes</Value></Eq></Where><OrderBy><FieldRef Name='Rank' Ascending='True' /></OrderBy>";
            }
            else
            {
                strCamlQuery = "<OrderBy><FieldRef Name=\"Title\"/></OrderBy><Where><Eq><FieldRef Name=\"Active\" /><Value Type=\"Choice\">Yes</Value></Eq></Where>";
            }

            MOSSServiceManager objMossController = (MOSSServiceManager)objFactory.GetServiceManager(MOSSSERVICE);
            dtListValues.Reset();
            dtListValues = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, listName, strCamlQuery);
            return dtListValues;

        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="listName">Name of the list.</param>
        /// <param name="filterValue">The filter value.</param>
        /// <returns></returns>
        protected ArrayList GetTectonicSettingsBasinNames(string listName, string filterValue)
        {
            ArrayList arrTechoSettingsBasinNames = new ArrayList();
            string strCamlQuery = string.Empty;
            if(!string.IsNullOrEmpty(filterValue))
            {
                strCamlQuery = "<Where><And><Contains><FieldRef Name='Title' /> <Value Type='Text'>" + filterValue + "</Value></Contains><Eq><FieldRef Name='Active' /> <Value Type='Choice'>yes</Value></Eq></And></Where><OrderBy><FieldRef Name='Title' Ascending='False' />   </OrderBy>";
            }

            MOSSServiceManager objMossController = (MOSSServiceManager)objFactory.GetServiceManager(MOSSSERVICE);
            dtListValues.Reset();
            dtListValues = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, listName, strCamlQuery);
            foreach(DataRow row in dtListValues.Rows)
            {
                if(row["BasinName"] != null && !string.IsNullOrEmpty(row["BasinName"].ToString()))
                {
                    arrTechoSettingsBasinNames.Add(SetValue(row["BasinName"].ToString()));
                }

            }
            return arrTechoSettingsBasinNames;

        }

        /// <summary>
        /// Loads the country basin data.
        /// </summary>
        /// <param name="strListName">Name of the STR list.</param>
        /// <param name="dropDownControl">The DropDown control.</param>
        protected void LoadDropdownData(string listName, DropDownList dropDownControl)
        {
            try
            {
                DataRow dtRow;
                string strEntityName = string.Empty;
                string strColumnName = string.Empty;
                string strPreferredCountryBasin = string.Empty;
                string strCamlQuery = "<OrderBy><FieldRef Name=\"Title\"/></OrderBy><Where><Eq><FieldRef Name=\"Active\" /><Value Type=\"Choice\">Yes</Value></Eq></Where>";
                if(string.Compare(listName, RESERVEMAGNITUDEOILLIST) == 0 || string.Compare(listName, RESERVEMAGNITUDEGASLIST) == 0)
                {
                    strCamlQuery = "<OrderBy><FieldRef Name=\"SortOrder\"/></OrderBy><Where><Eq><FieldRef Name=\"Active\" /><Value Type=\"Choice\">Yes</Value></Eq></Where>";
                }
                else if(string.Compare(listName, TECHONICSETTINNGSLIST) == 0)
                {
                    string[] strUniqueColumn = { "Title" };
                    if(string.Equals(dropDownControl.ID, "cboTectonicSettingKle"))
                    {
                        strCamlQuery = "<OrderBy><FieldRef Name=\"Title\"/></OrderBy><Where><And><Eq><FieldRef Name=\"Active\" /><Value Type=\"Choice\">Yes</Value></Eq><Eq><FieldRef Name='TectonicType' /><Value Type='Choice'>" + "Klemme" + "</Value></Eq></And></Where>";
                    }
                    else if(string.Equals(dropDownControl.ID, "cboTectonicSetting"))
                    {
                        strCamlQuery = "<OrderBy><FieldRef Name=\"Title\"/></OrderBy><Where><And><Eq><FieldRef Name=\"Active\" /><Value Type=\"Choice\">Yes</Value></Eq><Eq><FieldRef Name='TectonicType' /><Value Type='Choice'>" + "Bally" + "</Value></Eq></And></Where>";
                    }
                }
                dtListValues.Reset();
                dtListValues = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, listName, strCamlQuery);

                string strDefaultvalue = dropDownControl.Items.Count > 0 ? dropDownControl.Items[0].ToString() : string.Empty;
                dropDownControl.Items.Clear();
                dropDownControl.Items.Add(strDefaultvalue);
                ListItem lstItem;
                if(dtListValues != null && dtListValues.Rows.Count > 0)
                {
                    if(string.Compare(listName, TECHONICSETTINNGSLIST) == 0)
                    {
                        string[] strUniqueColumn = { "Title" };
                        dtListValues = dtListValues.DefaultView.ToTable(true, strUniqueColumn);
                    }

                    for(int intIndex = 0; intIndex < dtListValues.Rows.Count; intIndex++)
                    {
                        dtRow = dtListValues.Rows[intIndex];
                        if((dtRow["Title"] != null) && (!string.IsNullOrEmpty(dtRow["Title"].ToString())))
                        {
                            lstItem = new ListItem(dtRow["Title"].ToString());

                            if(string.Compare(listName, RESERVEMAGNITUDEOILLIST) == 0 || string.Compare(listName, RESERVEMAGNITUDEGASLIST) == 0)
                            {
                                if((dtRow["Value"] != null) && (!string.IsNullOrEmpty(dtRow["Value"].ToString())))
                                {

                                    lstItem = new ListItem(dtRow["Title"].ToString(), dtRow["Value"].ToString());
                                }
                            }
                            else if(string.Compare(listName, HYDROCARBONMAINLIST) == 0)
                            {
                                if((dtRow["HydrocarbonCode"] != null) && (!string.IsNullOrEmpty(dtRow["HydrocarbonCode"].ToString())))
                                {

                                    lstItem = new ListItem(dtRow["Title"].ToString(), dtRow["HydrocarbonCode"].ToString());
                                }
                            }
                            else
                            {
                                lstItem = new ListItem(dtRow["Title"].ToString());
                            }
                            dropDownControl.Items.Add(lstItem);
                        }

                    }
                }
                dropDownControl.SelectedIndex = 0;
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
            finally
            {
                if(dtListValues != null)
                    dtListValues.Dispose();
            }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="strField">The STR field.</param>
        /// <returns>Value object</returns>        
        protected Value SetValue(string value)
        {
            Value objValue = new Value();
            objValue.InnerText = value;
            return objValue;
        }
        #endregion
        /// <summary>
        /// Sets the basin country attribute.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="listBoxControl">The list box control.</param>
        /// <returns>Arraylist containing Basin object or Country object.</returns>
        protected ArrayList SetBasinCountryAttribute(ArrayList attribute, ListControl listBoxControl)//DREAM4.0 Changes replaced Listbox control type to List control
        {
            Attributes objAttribute = new Attributes();
            ArrayList arrAttributeValue = new ArrayList();
            int intCounter = 0;
            foreach(ListItem lstItem in listBoxControl.Items)
            {
                if(lstItem.Selected)
                {
                    intCounter++;
                    arrAttributeValue.Add(SetValue(lstItem.Value));
                    objAttribute.Operator = GetOperator(lstItem.Value);
                }
            }
            if(intCounter == 0)
            {
                if(string.Equals(listBoxControl.ID, "lstCountry"))
                {
                    if(listBoxControl.Items.Count > 0)
                    {
                        foreach(ListItem lstItem in listBoxControl.Items)
                        {
                            arrAttributeValue.Add(SetValue(lstItem.Value));
                        }
                        objAttribute.Name = GetControlID(listBoxControl.ID);
                        objAttribute.Value = arrAttributeValue;
                        objAttribute.Label = listBoxControl.ID;
                        objAttribute.Checked = FALSE;
                        objAttribute.Operator = GetOperator(objAttribute.Value);
                        attribute.Add(objAttribute);
                    }
                }
            }
            else if(intCounter == 1)
            {
                objAttribute.Name = GetControlID(listBoxControl.ID);
                objAttribute.Label = listBoxControl.ID;
                objAttribute.Value = arrAttributeValue;
                objAttribute.Checked = TRUE;
                attribute.Add(objAttribute);
            }
            else
            {
                objAttribute.Name = GetControlID(listBoxControl.ID);
                objAttribute.Value = arrAttributeValue;
                objAttribute.Label = listBoxControl.ID;
                objAttribute.Operator = GetOperator(objAttribute.Value);
                objAttribute.Checked = TRUE;
                attribute.Add(objAttribute);
            }
            return attribute;
        }

        /// <summary>
        /// Sets the basic list attribute.
        /// </summary>
        /// <param name="arrAttribute">The arr attribute.</param>
        /// <param name="txtControl">The TextBox Control.</param>
        /// <returns></returns>
        protected ArrayList SetBasicListAttribute(ArrayList attribute, TextBox textBoxControl)
        {
            Attributes objAttribute = new Attributes();
            ArrayList arrAttributeValue = new ArrayList();
            if(textBoxControl.Text.Length > 0)
            {
                objAttribute.Name = GetControlID(textBoxControl.ID);
                arrAttributeValue.Add(SetValue(textBoxControl.Text.Trim()));
                objAttribute.Value = arrAttributeValue;
                objAttribute.Operator = GetOperator(textBoxControl.Text);
                objAttribute.Label = textBoxControl.ID;
                attribute.Add(objAttribute);
            }
            return attribute;
        }

        /// <summary>
        /// Constructs an attribute object with the search criteria and values selected
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="textBoxControl"></param>
        /// <param name="arlCriteria"></param>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        protected ArrayList SetSearchCriteriaAttribute(ArrayList attribute, TextBox textBoxControl, ArrayList arlCriteria, ListControl searchCriteria)
        {
            Attributes objAttribute = new Attributes();
            ArrayList arrAttributeValue = new ArrayList();
            if(arlCriteria.Count > 0)
            {
                objAttribute.Name = searchCriteria.SelectedItem.Value;
                foreach(string strCriteria in arlCriteria)
                {
                    arrAttributeValue.Add(SetValue(strCriteria));
                }
                objAttribute.Value = arrAttributeValue;
                objAttribute.Operator = GetOperator(arrAttributeValue);
                objAttribute.Label = "FileSearch";
                attribute.Add(objAttribute);
            }
            else
            {
                if(string.Equals(searchCriteria.SelectedItem.Text, UWI) || string.Equals(searchCriteria.SelectedItem.Text, UWBI))
                {
                    if(textBoxControl.Text.Length > 0)
                    {
                        objAttribute.Name = GetControlID(textBoxControl.ID);
                        arrAttributeValue.Add(SetValue(textBoxControl.Text.Trim()));
                        objAttribute.Value = arrAttributeValue;
                        objAttribute.Operator = GetOperator(textBoxControl.Text);
                        objAttribute.Label = textBoxControl.ID;
                        attribute.Add(objAttribute);
                    }
                }
            }
            return attribute;

        }

        /// <summary>
        /// overlaoded method constructs an attribute object with the search criteria and values selected
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="listBoxControl"></param>
        /// <param name="arlCriteria"></param>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        protected ArrayList SetBasinCountryAttribute(ArrayList attribute, ListBox listBoxControl, ArrayList arlCriteria, ListControl searchCriteria)
        {

            Attributes objAttribute = new Attributes();
            ArrayList arrAttributeValue = new ArrayList();
            if(arlCriteria.Count > 0)
            {

                objAttribute.Name = searchCriteria.SelectedItem.Value;
                foreach(string strCriteria in arlCriteria)
                {
                    arrAttributeValue.Add(SetValue(strCriteria));
                }
                objAttribute.Value = arrAttributeValue;
                objAttribute.Operator = GetOperator(arrAttributeValue);
                objAttribute.Label = "FileSearch";
                attribute.Add(objAttribute);
            }
            else
            {
                attribute = SetBasinCountryAttribute(attribute, listBoxControl);
            }
            return attribute;
        }


        /// <summary>
        /// Sets the basic list attribute.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="listBoxControl">The list box control.</param>
        /// <returns></returns>
        protected ArrayList SetBasicListAttribute(ArrayList attribute, ListBox listBoxControl)
        {
            Attributes objAttribute = new Attributes();
            ArrayList arrAttributeValue = new ArrayList();
            int intCounter = 0;
            foreach(ListItem item in listBoxControl.Items)
            {
                if(item.Selected)
                {
                    intCounter++;
                    arrAttributeValue.Add(SetValue(item.Text));
                }
            }
            if(intCounter != 0)
            {
                objAttribute.Name = GetControlID(listBoxControl.ID);
                objAttribute.Value = arrAttributeValue;
                objAttribute.Label = listBoxControl.ID;
                objAttribute.Operator = GetOperator(objAttribute.Value);
                attribute.Add(objAttribute);
            }
            return attribute;
        }

        #region DREAM 2.1 Fix
        /// <summary>
        /// Sets the basic list attribute.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="listBoxControl">The list box control.</param>
        /// <returns></returns>
        protected ArrayList SetBasicListAttribute(ArrayList attribute, ListBox listBoxControl, bool useValueField)
        {
            Attributes objAttribute = new Attributes();
            ArrayList arrAttributeValue = new ArrayList();
            int intCounter = 0;
            foreach(ListItem item in listBoxControl.Items)
            {
                if(item.Selected)
                {
                    intCounter++;
                    if(useValueField)
                    {
                        arrAttributeValue.Add(SetValue(item.Value));
                    }
                    else
                    {
                        arrAttributeValue.Add(SetValue(item.Text));
                    }
                }
            }
            if(intCounter != 0)
            {
                objAttribute.Name = GetControlID(listBoxControl.ID);
                objAttribute.Value = arrAttributeValue;
                objAttribute.Label = listBoxControl.ID;
                objAttribute.Operator = GetOperator(objAttribute.Value);
                attribute.Add(objAttribute);
            }
            return attribute;
        }
        #endregion DREAM 2.1 Fix
        /// <summary>
        /// Sets the lat lon parameter.
        /// </summary>
        /// <param name="textBoxControl">The text box control.</param>
        /// <param name="parameterArray">The parameter array.</param>
        /// <returns></returns>
        protected ArrayList SetLatLonParameter(TextBox textBoxControl, ArrayList parameterArray)
        {
            Parameters objLatLongDeg = new Parameters();
            objLatLongDeg.Name = GetLatLonID(textBoxControl.ID);
            objLatLongDeg.Value = textBoxControl.Text;
            objLatLongDeg.Label = textBoxControl.ID;
            parameterArray.Add(objLatLongDeg);

            return parameterArray;
        }


        /// <summary>
        /// Sets the drop down controls.
        /// </summary>
        /// <param name="dropdownlist">The dropdownlist.</param>
        /// <param name="attributes">The attributes object.</param>
        /// <returns>The arraylist containing attributes object</returns>
        protected ArrayList SetUITextControls(DropDownList dropdownlist, ArrayList attribute)
        {
            Attributes objAttribute = new Attributes();
            ArrayList arlValue = new ArrayList();
            if(dropdownlist.SelectedIndex != -1)
            {
                objAttribute.Name = GetNodeIDFromControl(dropdownlist.ID);
                arlValue.Add(SetValue(dropdownlist.SelectedItem.Value.Trim()));
                objAttribute.Value = arlValue;
                objAttribute.Operator = GetOperator(objAttribute.Value);
                objAttribute.Label = dropdownlist.ID;
                attribute.Add(objAttribute);
            }
            return attribute;
        }

        protected ArrayList SetUITextControls(DropDownList dropdownlist, ArrayList attribute, bool useTextField)
        {
            Attributes objAttribute = new Attributes();
            ArrayList arlValue = new ArrayList();
            if(dropdownlist.SelectedIndex != -1)
            {
                objAttribute.Name = GetNodeIDFromControl(dropdownlist.ID);
                if(useTextField)
                {
                    arlValue.Add(SetValue(dropdownlist.SelectedItem.Text.Trim()));
                }
                else
                {
                    arlValue.Add(SetValue(dropdownlist.SelectedItem.Value.Trim()));
                }
                objAttribute.Value = arlValue;
                objAttribute.Operator = GetOperator(objAttribute.Value);
                objAttribute.Label = dropdownlist.ID;
                attribute.Add(objAttribute);
            }
            return attribute;
        }
        /// <summary>
        /// Sets the drop down controls.
        /// </summary>
        /// <param name="dropdownlist">The dropdownlist.</param>
        /// <param name="attributes">The attributes object.</param>
        /// <returns>The arraylist containing attributes object</returns>
        protected ArrayList SetUITextControls(DropDownList dropdownlist, ArrayList attribute, ArrayList arlValue)
        {
            Attributes objAttribute = new Attributes();

            if(dropdownlist.SelectedIndex != -1 && arlValue.Count > 0)
            {
                objAttribute.Name = GetNodeIDFromControl(dropdownlist.ID);
                objAttribute.Value = arlValue;
                objAttribute.Operator = GetOperator(objAttribute.Value);
                objAttribute.Label = dropdownlist.ID;
                attribute.Add(objAttribute);
            }
            return attribute;
        }

        #region SRP Code
        /// <summary>
        /// Sets the  radio button list controls.
        /// </summary>
        /// <param name="RadioButtonList">The radiobuttonlist.</param>
        /// <param name="attributes">The attributes object.</param>
        /// <returns>The arraylist containing attributes object</returns>
        protected ArrayList SetUITextControls(RadioButtonList radiobuttonlist, ArrayList attribute, bool includeInRequest)
        {
            Attributes objAttribute = new Attributes();
            ArrayList arlValue = new ArrayList();
            if(radiobuttonlist.SelectedIndex != -1)
            {
                objAttribute.Name = GetNodeIDFromControl(radiobuttonlist.ID);
                arlValue.Add(SetValue(radiobuttonlist.SelectedValue.Trim()));
                objAttribute.Value = arlValue;
                objAttribute.Operator = GetOperator(objAttribute.Value);
                objAttribute.Label = radiobuttonlist.ID;
                if(!includeInRequest)
                {
                    objAttribute.Checked = "Exclude";
                }
                attribute.Add(objAttribute);
            }
            return attribute;
        }

        /// <summary>
        /// Sets the Rad Combo Box controls.
        /// </summary>
        /// <param name="dropdownlist">The Rad Combox box.</param>
        /// <param name="attributes">The attributes object.</param>
        /// <returns>The arraylist containing attributes object</returns>
        protected ArrayList SetUITextControls(RadComboBox dropdownlist, ArrayList attribute, string values)
        {
            Attributes objAttribute = new Attributes();
            ArrayList arlValue = new ArrayList();
            if(!string.IsNullOrEmpty(dropdownlist.SelectedValue))
            {
                objAttribute.Name = GetRadControlID(dropdownlist.ID);
                if(values.Contains(";"))
                {
                    string[] strTempValues = values.Split(";".ToCharArray());
                    if(strTempValues != null && strTempValues.Length >= 1)
                    {
                        values = string.Empty;
                        values = strTempValues[0];
                    }
                }
                arlValue.Add(SetValue(values));
                objAttribute.Value = arlValue;
                objAttribute.Operator = GetOperator(objAttribute.Value);
                objAttribute.Label = dropdownlist.ID;
                attribute.Add(objAttribute);
            }
            else
            {
                if((!string.IsNullOrEmpty(values)) && (!values.Equals(";")) && (!values.Equals(DEFAULTDROPDOWNTEXT + ";")))
                {
                    objAttribute.Name = GetRadControlID(dropdownlist.ID);
                    if(values.Contains(";"))
                    {
                        string[] strTempValues = values.Split(";".ToCharArray());
                        if(strTempValues != null && strTempValues.Length >= 1)
                        {
                            values = string.Empty;
                            values = strTempValues[0];
                        }
                    }
                    arlValue.Add(SetValue(values));
                    objAttribute.Value = arlValue;
                    objAttribute.Operator = GetOperator(objAttribute.Value);
                    objAttribute.Label = dropdownlist.ID;
                    attribute.Add(objAttribute);
                }
            }
            return attribute;
        }

        /// <summary>
        /// Sets the  text controls with out range.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        /// <param name="attributes">The attributes object.</param>
        /// <returns>The arraylist containing attributes object</returns>
        protected ArrayList SetUITextControls(TextBox textBox, ArrayList attribute)
        {
            Attributes objAttribute = new Attributes();
            ArrayList arlValue = new ArrayList();
            if(textBox.Text.Trim().Length > 0)
            {

                objAttribute.Name = GetNodeIDFromControl(textBox.ID);
                arlValue.Add(SetValue(textBox.Text.Trim()));
                objAttribute.Value = arlValue;
                objAttribute.Operator = GetOperator(objAttribute.Value);
                objAttribute.Label = textBox.ID;
                attribute.Add(objAttribute);
            }
            return attribute;
        }

        /// <summary>
        /// Sets the text controls with Range.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        /// <param name="attributes">The attributes object.</param>
        /// <returns>The arraylist containing attributes object</returns>
        protected ArrayList SetUITextControls(TextBox textBox, ArrayList attribute, bool calculateRange, string rangeValue, bool searchClick)
        {
            Attributes objAttribute = new Attributes();
            Attributes objAttributeMin = new Attributes();
            ArrayList arlValue = new ArrayList();
            ArrayList arlValueMin = new ArrayList();
            AttributeGroup objBasicAttributeGroup = new AttributeGroup();
            ArrayList objInnerAttribute = new ArrayList();
            double dblRange = Convert.ToDouble(rangeValue);


            if(textBox.Text.Trim().Length > 0)
            {
                if((searchClick && calculateRange) && dblRange > 0)
                {
                    objAttribute.Name = GetNodeIDFromControl(textBox.ID);
                    string strValue = GetMaxRange(textBox.Text.Trim(), dblRange).ToString();
                    arlValue.Add(SetValue(strValue));
                    objAttribute.Value = arlValue;
                    objAttribute.Operator = LESSTHANEQUALS;
                    objAttribute.Label = textBox.ID;
                    objAttribute.IsRangeApplicable = true;
                    objInnerAttribute.Add(objAttribute);

                    objAttributeMin.Name = GetNodeIDFromControl(textBox.ID);
                    strValue = GetMinRange(textBox.Text.Trim(), dblRange).ToString();
                    arlValueMin.Add(SetValue(strValue));
                    objAttributeMin.Value = arlValueMin;
                    objAttributeMin.Operator = GREATERTHANEQUALS;
                    objAttributeMin.Label = textBox.ID;
                    objAttributeMin.IsRangeApplicable = true;
                    objInnerAttribute.Add(objAttributeMin);
                    objBasicAttributeGroup.Operator = GetLogicalOperator();
                    objBasicAttributeGroup.Attribute = objInnerAttribute;
                    attribute.Add(objBasicAttributeGroup);
                }
                else
                {
                    objAttribute.Name = GetNodeIDFromControl(textBox.ID);
                    arlValue.Add(SetValue(textBox.Text.Trim()));
                    objAttribute.Value = arlValue;
                    objAttribute.Operator = GetOperator(objAttribute.Value);
                    objAttribute.Label = textBox.ID;
                    if(calculateRange)
                    {
                        objAttribute.IsRangeApplicable = true;
                    }
                    attribute.Add(objAttribute);
                }
            }
            return attribute;
        }


        /// <summary>
        /// Sets the UI text controls with Search Criteria.
        /// </summary>
        /// <param name="textBoxControl">The text box control.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="arlCriteria">The arl criteria.</param>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        protected ArrayList SetUITextControls(TextBox textBoxControl, ArrayList attribute, ArrayList arlCriteria, ListControl searchCriteria)
        {
            Attributes objAttribute = new Attributes();
            ArrayList arlValue = new ArrayList();
            if(arlCriteria.Count > 0)
            {

                objAttribute.Name = searchCriteria.SelectedItem.Value;
                foreach(string strCriteria in arlCriteria)
                {
                    arlValue.Add(SetValue(strCriteria));
                }
                objAttribute.Value = arlValue;
                objAttribute.Operator = GetOperator(arlValue);
                objAttribute.Label = "FileSearch";
                attribute.Add(objAttribute);
            }
            else
            {
                if(textBoxControl.Text.Length > 0)
                {
                    objAttribute.Name = GetNodeIDFromControl(textBoxControl.ID);
                    arlValue.Add(SetValue(textBoxControl.Text.Trim()));
                    objAttribute.Value = arlValue;
                    objAttribute.Operator = GetOperator(objAttribute.Value);
                    objAttribute.Label = textBoxControl.ID;
                    attribute.Add(objAttribute);
                }
            }
            return attribute;
        }

        /// <summary>
        /// Sets the UI text controls with depth Units.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        /// <param name="attributes">The attributes object.</param>
        /// <returns>The arraylist containing attributes object</returns>
        protected ArrayList SetUITextControls(TextBox textBox, ArrayList attribute, string depthUnits)
        {
            Attributes objAttribute = new Attributes();
            ArrayList arlValue = new ArrayList();
            if(textBox.Text.Trim().Length > 0)
            {
                objAttribute.Name = GetNodeIDFromControl(textBox.ID) + depthUnits;
                arlValue.Add(SetValue(textBox.Text.Trim()));
                objAttribute.Value = arlValue;
                if(string.Equals(textBox.ID.ToString(), "txtCurveTopDepth") || string.Equals(textBox.ID.ToString(), "txtCurveBottomDepth"))
                    objAttribute.Operator = GetLogsByFieldOperator(textBox.ID.ToString());
                else
                    objAttribute.Operator = GetOperator(objAttribute.Value);
                objAttribute.Label = textBox.ID;
                attribute.Add(objAttribute);
            }
            return attribute;
        }
        #endregion
        /// <summary>
        /// Gets the operator.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected string GetLogsByFieldOperator(string textBoxID)
        {
            string strOperator;

            if(string.Equals(textBoxID, "txtCurveTopDepth"))
            {
                strOperator = GREATERTHANEQUALS;
            }
            else
            {
                strOperator = LESSTHANEQUALS;
            }
            return strOperator;
        }
        /// <summary>
        /// Clears the UI controls.
        /// </summary>
        protected void ClearUIControls()
        {
            foreach(Control objControl in this.Controls)
            {
                foreach(Control objChildControl in objControl.Controls)
                {
                    RadioButtonList objRadioButtonList = new RadioButtonList();
                    RadioButton objRadioButton = new RadioButton();
                    TextBox objTextBox = new TextBox();
                    ListBox objListBox = new ListBox();
                    CheckBox objCheckBox = new CheckBox();
                    RadComboBox objRadComboBox = new RadComboBox();
                    DropDownList objDropDownList = new DropDownList();
                    if(string.Equals(objChildControl.GetType().ToString(), objRadioButtonList.GetType().ToString()))
                    {
                        foreach(ListItem lstRadioItem in ((RadioButtonList)(objChildControl)).Items)
                        {
                            lstRadioItem.Selected = false;
                        }
                    }
                    if(string.Equals(objChildControl.GetType().ToString(), objRadioButton.GetType().ToString()))
                    {
                        ((RadioButton)(objChildControl)).Checked = false;
                    }
                    if(string.Equals(objChildControl.GetType().ToString(), objTextBox.GetType().ToString()))
                    {
                        ((TextBox)(objChildControl)).Text = string.Empty;
                    }
                    if(string.Equals(objChildControl.GetType().ToString(), objListBox.GetType().ToString()))
                    {
                        ((ListBox)(objChildControl)).Items.Clear();
                    }
                    if(string.Equals(objChildControl.GetType().ToString(), objCheckBox.GetType().ToString()))
                    {
                        ((CheckBox)(objChildControl)).Checked = false;
                    }

                }
            }
        }
        /// <summary>
        /// Checks the radio button group.
        /// </summary>
        /// <param name="controlID">The control ID.</param>
        /// <param name="saveSearchDoc">The save search doc.</param>
        /// <param name="strXPATH">The STR XPATH.</param>
        /// <returns></returns>
        protected int CheckRadioButtonGroup(string controlID, XmlDocument saveSearchDoc, string radioGroupXPath)
        {
            string strRdoBtnListName = string.Empty;
        SELECTRADIOGROUP:
            XmlNodeList xmlnodelistRdoButton = saveSearchDoc.SelectNodes(radioGroupXPath);
            int intCounter = 0;
            foreach(XmlNode xmlnodeRdoButton in xmlnodelistRdoButton)
            {
                if((xmlnodeRdoButton.Attributes.GetNamedItem("label") != null) && (string.Equals(xmlnodeRdoButton.Attributes.GetNamedItem("label").Value.ToString(), controlID)))
                {
                    strRdoBtnListName = xmlnodeRdoButton.Attributes.GetNamedItem("name").Value.ToString();
                    intCounter++;
                }
            }
            if(intCounter == 0)
            {
                radioGroupXPath = "/attributegroup/attributegroup/attributegroup";
                goto SELECTRADIOGROUP;
            }
            int intIndex = 0;
            switch(strRdoBtnListName)
            {
                case "Well Wellbore":
                    intIndex = 0;
                    break;
                case "Picks":
                    intIndex = 1;
                    break;
                case "Logs by Well Wellbore":
                    intIndex = 2;
                    break;
                case "SpudKickoff":
                    intIndex = 0;
                    break;
                case "Completion":
                    intIndex = 1;
                    break;
                case "Surface":
                    intIndex = 0;
                    break;
                case "Bottom":
                    intIndex = 1;
                    break;
                default:
                    intIndex = 0;
                    break;
            }
            return intIndex;
        }
        /// <summary>
        /// Assign tooltip to the control
        /// </summary>
        /// <param name="searchType"></param>
        protected DataTable AssignToolTip()
        {
            DataTable dtToolTip = new DataTable();
            dtToolTip = TooltipManager.GetInstance().GetTooltip();
            return dtToolTip;
        }
        /// <summary>
        /// Filter tool tip data table based on searchType
        /// </summary>
        /// <param name="filterDataTable"></param>
        /// <param name="searchType"></param>
        /// <returns></returns>
        protected DataTable GetFilterDataTable(DataTable filterDataTable, string searchType)
        {
            string strFilterColumn = string.Empty;
            strFilterColumn = GetFilterColumn(searchType);
            return TooltipManager.GetInstance().FilterDataTable(filterDataTable, strFilterColumn);
        }
        /// <summary>
        /// Reads from text file.
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        /// <returns></returns>
        protected ArrayList ReadFromTextFile(Stream fileStream)
        {
            ArrayList objSrchValue = new ArrayList();
            StreamReader objReader = new StreamReader(fileStream);

            string strCriteriaValue = objReader.ReadLine();

            while(strCriteriaValue != null)
            {
                if(!string.IsNullOrEmpty(strCriteriaValue))
                    objSrchValue.Add(strCriteriaValue);
                strCriteriaValue = objReader.ReadLine();
            }

            if(objReader != null)
            {
                objReader.Close();
                objReader.Dispose();
            }
            if(fileStream != null)
            {
                fileStream.Close();
                fileStream.Dispose();
            }
            return objSrchValue;
        }
        /// <summary>
        /// Based on the type of file, this method calls the appropriate file reader code and returns
        /// an arraylist of search criteria
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected ArrayList ReadFileSearch(HttpPostedFile content, string documentContent)
        {
            switch(content.FileName.Substring(content.FileName.LastIndexOf(".") + 1))
            {
                case "txt":
                    {
                        return ReadFromTextFile(content.InputStream);
                    }
                case "xls":
                    {
                        return ReadFromOfficeFile(documentContent);
                    }
                case "doc":
                    {
                        return ReadFromOfficeFile(documentContent);
                    }
                case "xlsx":
                    {
                        return ReadFromOfficeFile(documentContent);
                    }
                case "docx":
                    {
                        return ReadFromOfficeFile(documentContent);
                    }
            }
            return null;
        }
        /// <summary>
        /// Gets the tooltip.
        /// </summary>
        /// <param name="dtTooltip">The dt tooltip.</param>
        /// <param name="controlName">Name of the control.</param>
        /// <returns></returns>
        protected string GetTooltip(DataTable dtTooltip, string controlName)
        {
            string strToolTip = string.Empty;
            DataRow[] arrDataRow = null;

            if(dtTooltip != null)
            {
                arrDataRow = dtTooltip.Select("Title ='" + controlName + "'");
                dtTooltip.Dispose();
            }
            if(arrDataRow != null && arrDataRow.Length > 0)
            {
                strToolTip = arrDataRow[0]["Tooltip_x0020_Text"].ToString();
            }
            return strToolTip;
        }
        #region SRP Code
        /// <summary>
        /// Gets the max range of user input.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="range">The range.</param>
        /// <returns></returns>
        protected double GetMaxRange(string value, double range)
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
        protected double GetMinRange(string value, double range)
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
        #endregion
        /// <summary>
        /// Redirects the page.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="asset">The asset.</param>
        protected void RedirectPage(string url, string asset)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "AdvSrchResultsPageRedirect", "<script language=\"javascript\">try{OpenAdvSearchResultsPage('" + url + "','" + asset + "');}catch(Ex){}</script>", false);
        }
        /// <summary>
        /// Gets the id from hidden field.
        /// </summary>
        /// <param name="hdnSelectedRows">The HDN selected rows.</param>
        /// <returns></returns>
        protected string[] GetIdFromHiddenField(HiddenField hdnSelectedRows)
        {
            string strSelectedRows = hdnSelectedRows.Value;
            string[] arrIDs = null;
            Regex fixMe = new Regex(MYASSETPATTERN);
            string strTrimmedMyAssetValues = string.Empty;

            if(!string.IsNullOrEmpty(strSelectedRows))
            {
                strTrimmedMyAssetValues = fixMe.Replace(strSelectedRows, "");
                arrIDs = strTrimmedMyAssetValues.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }
            return arrIDs;
        }
        /// <summary>
        /// Create an attributes object and assign its properties
        /// </summary>
        /// <param name="name">Name of attribute</param>
        /// <param name="operation">Operation</param>
        /// <param name="value">array of value</param>
        /// <returns>attributes object</returns>
        protected Attributes AddAttribute(string name, string operation, string[] values)
        {
            Attributes objAttribute = new Attributes();

            objAttribute.Value = new ArrayList();

            objAttribute.Name = name;

            objAttribute.Operator = operation;
            foreach(string value in values)
            {
                if(!string.IsNullOrEmpty(value.Trim()))
                {
                    Value objValue = new Value();
                    objValue.InnerText = value.Trim();
                    objAttribute.Value.Add(objValue);
                }
            }
            return objAttribute;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /////Modification for list tunning dated - 09/feb/2009
        /// Get caml query based on asset type
        /// </summary>
        private string GetCamlQueryColumnList(string camlQuery, string asset)
        {
            camlQuery = "<Where><Eq><FieldRef Name=\"Asset_x0020_Type\" /><Value Type=\"Lookup\">" + asset + "</Value></Eq></Where>";
            return camlQuery;
        }
        /// <summary>
        /// Loads the date format.
        /// </summary>
        /// <param name="dropdownList">The dropdown list.</param>
        /// <param name="listName">Name of the list.</param>
        private void LoadDateFormat(DropDownList dropdownList, string listName)
        {
            DataRow dtRow;

            string strCamlQuery = "<OrderBy><FieldRef Name=\"Title\"/></OrderBy>";
            dtListValues.Reset();
            dtListValues = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, listName, strCamlQuery);
            dropdownList.Items.Clear();
            if(dtListValues != null)
            {
                for(int intIndex = 0; intIndex < dtListValues.Rows.Count; intIndex++)
                {
                    dtRow = dtListValues.Rows[intIndex];
                    dropdownList.Items.Add(dtRow["Title"].ToString());
                }
            }
            if(string.IsNullOrEmpty(PortalConfiguration.GetInstance().GetKey(listName).ToString()))
            {
                strCamlQuery = "<OrderBy><FieldRef Name=\"Title\"/></OrderBy><Where><Eq>" + "<FieldRef Name=\"Default_x0020_Format\" /><Value Type=\"Choice\">TRUE</Value></Eq></Where>";
                dtListValues.Reset();
                dtListValues = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, listName, strCamlQuery);
                if(dtListValues != null)
                {
                    for(int intIndex = 0; intIndex < dtListValues.Rows.Count; intIndex++)
                    {
                        dtRow = dtListValues.Rows[intIndex];
                        for(int intListDate = 0; intListDate < dropdownList.Items.Count; intListDate++)
                        {
                            if(string.Equals(dropdownList.Items[intListDate].Text.ToString(), dtRow["Title"].ToString()))
                            {
                                dropdownList.Items[intListDate].Selected = true;
                            }
                        }
                    }
                }
            }
            else
            {
                for(int intListDate = 0; intListDate < dropdownList.Items.Count; intListDate++)
                {
                    if(string.Equals(dropdownList.Items[intListDate].Text.ToString(), PortalConfiguration.GetInstance().GetKey(listName).ToString()))
                    {
                        dropdownList.Items[intListDate].Selected = true;
                    }
                }
            }
        }
        /// <summary>
        /// Get Filter Column
        /// </summary>
        /// <param name="filterColumn"></param>
        /// <returns></returns>
        private string GetFilterColumn(string filterColumn)
        {
            string strFilterColumn = string.Empty;
            switch(filterColumn)
            {
                case "Wellbore":
                    strFilterColumn = "Is_x0020_Applicable_x0020_For_x0";
                    break;
                case "Project Archives":
                    strFilterColumn = "Is_x0020_Applicable_x0020_For_x00";
                    break;
                case "Field":
                    strFilterColumn = "Is_x0020_Applicable_x0020_For_x01";
                    break;
                case "Basin":
                    strFilterColumn = "Is_x0020_Applicable_x0020_For_x02";
                    break;
                case "Logs By Field or Depth":
                    strFilterColumn = "Is_x0020_Applicable_x0020_For_x03";
                    break;
                case "Reservoir":
                    strFilterColumn = "Is_x0020_Applicable_x0020_For_x04";
                    break;
                default:
                    strFilterColumn = "Is_x0020_Applicable_x0020_For_x0";
                    break;
            }
            return strFilterColumn;
        }
        /// <summary>
        /// Reads from office file.
        /// </summary>
        /// <param name="documentContent">Content of the document.</param>
        /// <returns></returns>
        private ArrayList ReadFromOfficeFile(string documentContent)
        {
            ArrayList objSrchValue = new ArrayList();
            if(!string.IsNullOrEmpty(documentContent))
            {
                string[] arlContent = documentContent.Split(MYASSETPATTERN.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach(string strKey in arlContent)
                {
                    objSrchValue.Add(strKey);
                }
            }
            return objSrchValue;
        }

        #region R5k changes for DREAM 4.0
        /// <summary>
        /// Populates the list control.
        /// </summary>
        /// <param name="listcontrol">The listcontrol.</param>
        /// <param name="xmlDocResponse">The XML doc response.</param>
        /// <param name="xpath">The xpath.</param>
        /// <param name="selectedItem">The selected item.</param>
        protected void PopulateListControl(ListControl listcontrol, XmlDocument responseXml, string xpath, string selectedItem)//Dream4.0 changes replaced list box control to list control
        {
            XmlNodeList objXmlNodeList = null;
            if(responseXml != null)
            {
                objXmlNodeList = responseXml.SelectNodes(xpath);
                if(objXmlNodeList != null)
                {
                    listcontrol.DataSource = objXmlNodeList;
                    listcontrol.DataTextField = "value";
                    listcontrol.DataValueField = "value";
                    listcontrol.DataBind();
                    if(listcontrol.Items.FindByText(selectedItem) != null)
                    {
                        listcontrol.Items.FindByText(selectedItem).Selected = true;
                    }
                }
            }
        }
        /// <summary>
        /// Gets the basins.
        /// </summary>
        /// <returns>XmlDocument</returns>
        protected XmlDocument GetBasinFromWebService()
        {
            objReportController = objFactory.GetServiceManager(REPORTSERVICE);
            RequestInfo objRequestInfo = new RequestInfo();
            Entity objEntity = new Entity();
            Criteria objCriteria = new Criteria();
            objCriteria.Value = STAROPERATOR;
            objCriteria.Operator = LIKEOPERATOR;
            objEntity.Criteria = objCriteria;
            objRequestInfo.Entity = objEntity;
            XmlDocument xmlDocResponse = objReportController.GetSearchResults(objRequestInfo, intListValMaxRecord, BASINITEMVAL, null, 0);
            return xmlDocResponse;
        }
        /// <summary>
        /// Sets the user preferences.
        /// </summary>
        protected void SetUserPreferences()
        {
            objUserPreferences = new UserPreferences();
            string strUserId = objUtility.GetUserName();
            objUserPreferences = ((MOSSServiceManager)objMossController).GetUserPreferencesFromSession(Page, strUserId, objUtility.GetParentSiteUrl(strCurrSiteUrl));
        }
        #endregion
        #endregion
    }
}
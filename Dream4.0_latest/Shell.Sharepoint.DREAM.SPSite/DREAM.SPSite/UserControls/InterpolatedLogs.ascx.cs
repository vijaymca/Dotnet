#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename:InterpolatedLogs.ascx.cs
#endregion

#region namespace
using System;
using System.Collections;
using System.Data;
using System.Net;
using System.Web;
using System.Xml;
using System.Text.RegularExpressions;
using System.Web.Services.Protocols;
using System.Web.UI.WebControls;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;
using Microsoft.SharePoint;
using Shell.SharePoint.DREAM.Business.Entities;
#endregion namespace


namespace Shell.SharePoint.DREAM.Site.UI.UserControls
{
    public partial class InterpolatedLogs :UIControlHandler
    {
        #region Declarations
        const string HIDSELECTEDROWS = "hidSelectedRows";
        const string INTERPOLATEDLOGSENTITYNAME = "InterpolatedLogs";
        const string HIDSELECTEDCRITERIA = "hidSelectedCriteriaName";
        const string WELLBORENAME = "wellbore_name";
        const string PROJECTCOORDINATESYSTEM = "output_projected_coordinate_system";
        const string DEPTHINTERVAL = "depth_interval";
        const string INTERPOLATEDPOSITIONLOGS = "Interpolated Position Logs";
        const string TESTTYPE = "InterpolatedLogsTestType";
        const string TITLE = "Title";
        const string PROJECTSYSTEMLIST = "Project Coordinate System";
        const string DEPTHINTERVALLIST = "Depth Interval";
        const string SEARCH = "SearchType";
        const string INTERPOLATEDLOGS = "interpolatedlogs";
        const string POSITIONLOGDATA = "positionlogdata";
        const string POSITIONLOGDATAENTITY = "Position Log Data";
        const string DATAPREFERENCELIST = "Data Preference";
        const string PREFERREDDATA = "preferred data";
        const string NONPREFERREDDATA = "non-preferred data";
        const string PREFERREDFLAG = "Preferred Flag";
        const string INPUTDEPTHUNIT = "input_depth_unit";
        const string OUTPUTDEPTHUNIT = "output_depth_unit";
        const string MODE = "input_depth_mode";
        const string FEET = "feet";
        const string METER = "meter";
        const string ENTITYMETER = "m";
        const string FEETTEXT = "(ft)";
        const string METERTEXT = "(m)";
        string strCurrentDepthUnit = string.Empty;

        #endregion

        #region Protected Methods
        /// <summary>
        /// Page_s the init.
        /// </summary>
        protected void Page_Init()
        {
            try
            {
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                objReportController = objFactory.GetServiceManager(REPORTSERVICE);
                LoadWellbore();
                LoadProjectCoordinateSystem();
                LoadDepthInterval();
                LoadMode();
                LoadDataPreference();
            }
            catch(SoapException soapEx)
            {
                if(!string.Equals(soapEx.Message.ToString(), SOAPEXCEPTIONMESSAGE))
                {
                    CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                }
            }
            catch(WebException webEx)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), webEx, 1);
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);

            }
        }
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                strCurrentDepthUnit = GetUserSelectedUnit();
                if(!objUtility.IsPostBack(this.Page))
                {
                    if(Request.QueryString[SEARCH] != null)//to get the query string
                    {
                        if(Request.QueryString[SEARCH].ToLower().Equals(INTERPOLATEDLOGS))
                        {
                            trDepthInterval.Visible = true;
                            trMode.Visible = false;
                            lblChnage.Visible = false;
                            trDataPreferences.Visible = false;
                        }
                        else if(Request.QueryString[SEARCH].ToLower().Equals(POSITIONLOGDATA))
                        {
                            trMode.Visible = true;
                            trDepthInterval.Visible = false;
                            lblChange1.Visible = false;
                        }
                    }
                    txtbxFrom.Text = "0";
                    btnSearch_Click(null, EventArgs.Empty);
                    ddlMode.SelectedValue = "AHBDF";

                }
                if(strCurrentDepthUnit.Equals(FEET))
                {
                    lblFrom.Text = FEETTEXT;
                    lblTo.Text = FEETTEXT;
                    lblDepthInterval.Text = FEETTEXT;
                }
                else
                {
                    lblFrom.Text = METERTEXT;
                    lblTo.Text = METERTEXT;
                    lblDepthInterval.Text = METERTEXT;
                }
            }

            catch(SoapException soapEx)
            {
                if(!string.Equals(soapEx.Message.ToString(), SOAPEXCEPTIONMESSAGE))
                {
                    CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                }
            }
            catch(WebException webEx)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), webEx, 1);
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSearch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                XmlDocument xmlWellTestReportRequest = null;
                if(Request.QueryString[SEARCH].ToLower().Equals(INTERPOLATEDLOGS))
                {
                    xmlWellTestReportRequest = objReportController.CreateSearchRequest(GetRequestInfo(INTERPOLATEDPOSITIONLOGS));
                }
                else if(Request.QueryString[SEARCH].ToLower().Equals(POSITIONLOGDATA))
                {
                    xmlWellTestReportRequest = objReportController.CreateSearchRequest(GetRequestInfo(POSITIONLOGDATAENTITY));
                }
                if(xmlWellTestReportRequest != null)
                {
                    xmlDocRequest = xmlWellTestReportRequest;
                }
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }
        #endregion
        #region Private Methods

        /// <summary>
        /// Loads the wellbore.
        /// </summary> 
        private void LoadWellbore()
        {
            objUtility.PopulateAssetListControl(ddlWellBore, objUtility.GetPipeSeperatedStrAsArray(strSelectedAssetNames), objUtility.GetPipeSeperatedStrAsArray(strSelectedRows));
        }
        /// <summary>
        /// Loads the project coordinate system.
        /// </summary>
        private void LoadProjectCoordinateSystem()
        {
            string strCamlQuery = " <OrderBy><FieldRef Name=\"Title\" Ascending=\"True\" /></OrderBy>";
            string strViewFields = @"<FieldRef Name='Title'/>";
            LoadDropdownLstBxFromSPList(ddlProjectionSystem, PROJECTSYSTEMLIST, TITLE, TITLE, strCamlQuery, strViewFields);

        }
        /// <summary>
        /// Loads the depth interval.
        /// </summary>
        private void LoadDepthInterval()
        {
            string strCamlQuery = "<OrderBy><FieldRef Name=\"Title\" Ascending=\"True\" /></OrderBy>";
            string strViewFields = @"<FieldRef Name='Title'/>";
            LoadDropdownLstBxFromSPList(ddlDepthInterval, DEPTHINTERVALLIST, TITLE, TITLE, strCamlQuery, strViewFields);
        }
        /// <summary>
        /// Loads the mode.
        /// </summary>
        private void LoadMode()
        {
            ArrayList mode = new ArrayList();
            mode.Add("AHBDF");
            mode.Add("TVD");
            mode.Add("TVDSS");
            ddlMode.DataSource = mode;
            ddlMode.DataBind();
        }
        /// <summary>
        /// Loads the data preference.
        /// </summary>
        private void LoadDataPreference()
        {
            DataTable dtDataPreference = null;
            try
            {
                dtDataPreference = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, DATAPREFERENCELIST, string.Empty);
                foreach(DataRow dtRow in dtDataPreference.Rows)
                {
                    if(dtRow[0] != null)
                    {
                        ddlDataPreference.Items.Add((string)dtRow[0]);
                    }
                }
                if(dtDataPreference.Select("IsDefault='1'")[0][0] != null)
                {
                    ddlDataPreference.SelectedValue = (string)dtDataPreference.Select("IsDefault='1'")[0][0];
                }
            }
            finally
            {
                if(dtDataPreference != null)
                {
                    dtDataPreference.Dispose();
                }
            }
        }
        /// <summary>
        /// Loads the dropdown LST bx from SP list.
        /// </summary>
        /// <param name="dropdownLstBx">The dropdown LST bx.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="displayColName">Display name of the col.</param>
        /// <param name="valueColName">Name of the value col.</param>
        /// <param name="camlQuery">The caml query.</param>
        /// <param name="viewFields">The view fields.</param>
        private void LoadDropdownLstBxFromSPList(DropDownList dropdownLstBx, string listName, string displayColName, string valueColName, string camlQuery, string viewFields)
        {
            DataTable objDataTable = null;
            try
            {
                objDataTable = ((MOSSServiceManager)objMossController).ReadList(SPContext.Current.Site.Url, listName, camlQuery, viewFields);
                dropdownLstBx.DataSource = objDataTable;
                dropdownLstBx.DataTextField = displayColName;
                dropdownLstBx.DataValueField = valueColName;
                dropdownLstBx.DataBind();
            }
            finally
            {
                if(objDataTable != null)
                {
                    objDataTable.Dispose();
                }
            }
        }
        /// <summary>
        /// Gets the request info.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <returns></returns>
        private RequestInfo GetRequestInfo(string entityName)
        {
            objRequestInfo = new RequestInfo();
            objRequestInfo.Entity = new Entity();
            objRequestInfo.Entity.Property = true;
            objRequestInfo.Entity.Name = entityName;
            objRequestInfo.Entity.ResponseType = TABULAR;
            ArrayList arlAttributeGroup = new ArrayList();
            AttributeGroup objAttributeGroup = new AttributeGroup();
            objAttributeGroup.Operator = ANDOPERATOR;
            objAttributeGroup.Attribute = GetAttributes(entityName);
            arlAttributeGroup.Add(objAttributeGroup);
            objRequestInfo.Entity.AttributeGroups = arlAttributeGroup;
            return objRequestInfo;
        }
        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <returns></returns>
        private ArrayList GetAttributes(string entityName)
        {
            ArrayList arlAttribute = new ArrayList();
            if(ddlWellBore.Items.Count > 0)
            {
                arlAttribute.Add(AddAttribute(strSelectedCriteriaName, EQUALSOPERATOR, new string[] { ddlWellBore.SelectedValue }));
            }
            if(ddlProjectionSystem.Items.Count > 0)
            {
                arlAttribute.Add(AddAttribute(PROJECTCOORDINATESYSTEM, EQUALSOPERATOR, new string[] { ddlProjectionSystem.SelectedValue }));
            }
            if(Request.QueryString[SEARCH].ToLower().Equals(INTERPOLATEDLOGS))
            {
                if(ddlDepthInterval.Items.Count > 0)
                {
                    arlAttribute.Add(AddAttribute(DEPTHINTERVALLIST, EQUALSOPERATOR, new string[] { ddlDepthInterval.SelectedValue }));
                }
            }
            else if(Request.QueryString[SEARCH].ToLower().Equals(POSITIONLOGDATA))
            {
                if(ddlDataPreference.SelectedValue.ToLowerInvariant().Equals(PREFERREDDATA))
                {
                    arlAttribute.Add(AddAttribute(PREFERREDFLAG, EQUALSOPERATOR, new string[] { "true" }));

                }
                else if(ddlDataPreference.SelectedValue.ToLowerInvariant().Equals(NONPREFERREDDATA))
                {
                    arlAttribute.Add(AddAttribute(PREFERREDFLAG, EQUALSOPERATOR, new string[] { "false" }));
                }
                arlAttribute.Add(AddAttribute(MODE, EQUALSOPERATOR, new string[] { ddlMode.SelectedValue }));

            }

            if(!string.IsNullOrEmpty(txtbxFrom.Text))
            {
                arlAttribute.Add(AddAttribute(FROM, GREATERTHANEQUALS, new string[] { txtbxFrom.Text }));
            }
            if(!string.IsNullOrEmpty(txtbxTo.Text))
            {
                arlAttribute.Add(AddAttribute(TO, LESSTHANEQUALS, new string[] { txtbxTo.Text }));
            }
            if(strCurrentDepthUnit.Equals(FEET))
            {
                arlAttribute.Add(AddAttribute(INPUTDEPTHUNIT, EQUALSOPERATOR, new string[] { FEET }));
                arlAttribute.Add(AddAttribute(OUTPUTDEPTHUNIT, EQUALSOPERATOR, new string[] { FEET }));
            }
            else
            {
                arlAttribute.Add(AddAttribute(INPUTDEPTHUNIT, EQUALSOPERATOR, new string[] { ENTITYMETER }));
                arlAttribute.Add(AddAttribute(OUTPUTDEPTHUNIT, EQUALSOPERATOR, new string[] { ENTITYMETER }));
            }
            return arlAttribute;
        }
        private string GetUserSelectedUnit()
        {
            string strUnit = string.Empty;
            const string FEETMETERRADIOBUTTONID = "FeetMeters";
            SetUserPreferences();
            if(!string.IsNullOrEmpty(strUnit = GetFormControlValue(FEETMETERRADIOBUTTONID)))
            {
                if(strUnit.ToLower().Equals("rbfeet"))
                {
                    strUnit = "feet";
                }
                else
                {
                    strUnit = "metres";
                }
            }
            else if(objUserPreferences != null)
            {
                strUnit = objUserPreferences.DepthUnits.ToLowerInvariant();
            }
            return strUnit;
        }
        /// <summary>
        /// Gets the form control value.
        /// </summary>
        /// <param name="controlId">The control id.</param>
        /// <returns></returns>
        private string GetFormControlValue(string controlId)
        {
            #region Decaleration
            string strControlId = string.Empty;
            string strValue = string.Empty;
            string strFirstPart = string.Empty;
            string strSecondPart = string.Empty;
            string strIds = string.Empty;
            string[] arrControlIds = null;
            #endregion
            arrControlIds = HttpContext.Current.Request.Form.AllKeys;
            if(arrControlIds.Length > 0)
            {
                strIds = string.Join("|", arrControlIds);
                if(arrControlIds.Length == 1)
                {
                    strControlId = strIds;
                }
                else if(strIds.IndexOf(controlId) != -1)
                {
                    strFirstPart = strIds.Substring(0, strIds.IndexOf(controlId));
                    strSecondPart = strIds.Substring(strIds.IndexOf(controlId));
                    strControlId = strFirstPart.Substring(strFirstPart.LastIndexOf("|") + 1) + (strSecondPart.Contains("|") ? strSecondPart.Substring(0, strSecondPart.IndexOf("|")) : strSecondPart);
                }
                if(HttpContext.Current.Request.Form[strControlId] != null)
                {
                    strValue = HttpContext.Current.Request.Form[strControlId];
                }
            }
            return strValue;
        }
    }
        #endregion
}


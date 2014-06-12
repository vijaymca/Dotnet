#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : Geopressure.ascx.cs
#endregion

#region namespace
using System;
using System.Collections;
using System.Net;
using System.Web;
using System.Xml;
using System.Text.RegularExpressions;
using System.Web.Services.Protocols;
using System.Web.UI.WebControls;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DREAM.Business.Entities;
#endregion namespace

namespace Shell.SharePoint.DREAM.Site.UI.UserControls
{
    public partial class GeoPressure :UIControlHandler
    {
        #region Declarations
        const string GEOPRESSUREREQUESTXML = "GEOPRESSUREREQUESTXML";
        const string TESTTYPE = "GeoPresTestType";
        const string TESTTYPEATTRIBUTE = "test_type";
        const string DATASOURCEATTRIBUTE = "data_source";
        const string GEOPRESSUREENTITYNAME = "Geopressure";
        const string WELLTESTTYPE = "WellTestData";
        const string TESTTYPEATTRIBUTEXPATH = "response/report/record/attribute[@name='Test Type']/@value";
        const string DATASOURCEATTRIBUTEXPATH = "response/report/record/attribute[@name='Data Source']/@value";
        const string DROPDOWNDEFAULTTEXT = "--All--";
        const string DATASOURCEENTITYNAME = "GeoPressureDs";
        #endregion

        #region Protected Methods
        /// <summary>
        /// Page_s the init.
        /// </summary>
        protected void Page_Init()
        {
            try
            {
                objReportController = objFactory.GetServiceManager(REPORTSERVICE);
                LoadTestTypes();
                LoadDataSource();
                HideFilterOptions();
            }
            catch(SoapException soapEx)
            {
                if(!string.Equals(soapEx.Message.ToString(), SOAPEXCEPTIONMESSAGE))
                {
                    CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                }
                RenderExceptionMessage(soapEx.Message);
            }
            catch(WebException webEx)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), webEx, 1);
                RenderExceptionMessage(webEx.Message);
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
                lblException.Visible = false;
                if(!objUtility.IsPostBack(this.Page))
                {
                    btnSubmit_Click(null, EventArgs.Empty);
                }
            }
            catch(SoapException soapEx)
            {
                if(!string.Equals(soapEx.Message.ToString(), SOAPEXCEPTIONMESSAGE))
                {
                    CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                }
                RenderExceptionMessage(soapEx.Message);
            }
            catch(WebException webEx)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), webEx, 1);
                RenderExceptionMessage(webEx.Message);
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);

            }

        }

        /// <summary>
        /// Handles the Click event of the btnSubmit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                /// Create the request xml and save to Session Variable
                XmlDocument xmlWellTestReportRequest = null;
                xmlWellTestReportRequest = objReportController.CreateSearchRequest(GetRequestInfo(GEOPRESSUREENTITYNAME));
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
        /// Loads the test types.
        /// </summary>
        private void LoadTestTypes()
        {
            XmlDocument xmlWellTestTypesResp = null;
            //Handling soap exception here inorder to load other control though it fails here
            try
            {
                xmlWellTestTypesResp = objReportController.GetSearchResults(GetRequestInfo(TESTTYPE), -1, GEOPRESSUREENTITYNAME.ToUpperInvariant(), string.Empty, -1);
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
                RenderExceptionMessage(webEx.Message);
            }
            if(xmlWellTestTypesResp != null && xmlWellTestTypesResp.ChildNodes.Count > 0)
            {
                cboTestType.DataSource = xmlWellTestTypesResp.SelectNodes(TESTTYPEATTRIBUTEXPATH);
                cboTestType.DataTextField = VALUE;
                cboTestType.DataValueField = VALUE;
                cboTestType.DataBind();
                ListItem lstSelect = new ListItem(DROPDOWNDEFAULTTEXT, "0");
                cboTestType.Items.Insert(0, lstSelect);
            }
        }
        /// <summary>
        /// Loads the data source.
        /// </summary>
        private void LoadDataSource()
        {
            XmlDocument xmlDocGeopressureDatasource = null;
            //Handling soap exception here inorder to load other control though it fails here
            try
            {
                xmlDocGeopressureDatasource = objReportController.GetSearchResults(GetRequestInfo(DATASOURCEENTITYNAME), -1, GEOPRESSUREENTITYNAME.ToUpperInvariant(), string.Empty, -1);
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
                RenderExceptionMessage(webEx.Message);
            }
            if(xmlDocGeopressureDatasource != null && xmlDocGeopressureDatasource.ChildNodes.Count > 0)
            {
                cboDataSource.DataSource = xmlDocGeopressureDatasource.SelectNodes(DATASOURCEATTRIBUTEXPATH);
                cboDataSource.DataTextField = VALUE;
                cboDataSource.DataValueField = VALUE;
                cboDataSource.DataBind();
                ListItem lstSelect = new ListItem(DROPDOWNDEFAULTTEXT, "0");
                cboDataSource.Items.Insert(0, lstSelect);
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
            objRequestInfo.Entity.SessionID = this.Page.Session.SessionID;
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
            string[] arrWellWellboreID = null;
            ArrayList arlAttribute = new ArrayList();
            arrWellWellboreID = objUtility.GetPipeSeperatedStrAsArray(strSelectedRows);

            if((arrWellWellboreID != null) && (arrWellWellboreID.Length > 0))
            {
                arlAttribute.Add(objUtility.AddAttribute(strSelectedCriteriaName, objUtility.GetOperator(arrWellWellboreID), arrWellWellboreID));
            }
            if(entityName.ToLowerInvariant().Equals(GEOPRESSUREENTITYNAME.ToLowerInvariant()))
            {
                if(cboTestType.SelectedIndex > 0)
                {
                    arlAttribute.Add(objUtility.AddAttribute(TESTTYPEATTRIBUTE, EQUALSOPERATOR, new string[] { cboTestType.SelectedValue }));
                }
                if(cboDataSource.SelectedIndex > 0)
                {
                    arlAttribute.Add(objUtility.AddAttribute(DATASOURCEATTRIBUTE, EQUALSOPERATOR, new string[] { cboDataSource.SelectedValue }));
                }
            }
            return arlAttribute;
        }
        #region RenderExceptionMessage
        /// <summary>
        /// Renders the exception message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void RenderExceptionMessage(string message)
        {
            ExceptionBlock.Visible = true;
            lblException.Visible = true;
            lblException.Text = message;
        }
        /// <summary>
        /// Hides the filter options.
        /// </summary>
        private void HideFilterOptions()
        {
            if((cboDataSource.Items.Count <= 0) && (cboTestType.Items.Count <= 0))
            {
                GeoPressureFilterContent.Visible = false;
            }
            else
            {
                GeoPressureFilterContent.Visible = true;
            }
        }
        #endregion

        #endregion Private Methods
    }
}
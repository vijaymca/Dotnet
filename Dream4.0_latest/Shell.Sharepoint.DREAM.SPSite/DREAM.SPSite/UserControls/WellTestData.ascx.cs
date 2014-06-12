#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : WellTestData.ascx.cs
#endregion

#region namespace
using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Net;
using System.Web;
using System.Web.Services.Protocols;
using System.Web.UI.WebControls;
using System.Xml;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DREAM.Business.Entities;
#endregion namespace

namespace Shell.SharePoint.DREAM.Site.UI.UserControls
{
    public partial class WellTestData :UIControlHandler
    {
        #region Declarations

        const string STARTDATE = "test_date";
        const string ENDDATE = "test_date";
        const string RECORDXPATH = "response/report/record";
        const string WELLTESTTYPE = "WellTestData";
        const string TESTTYPE = "TestType";
        const string TESTTYPEATTRIBUTE = "test_type";
        const string TESTTYPEATTRIBUTEXPATH = "attribute[@name='Test Type']";
        const string PRESSURESURVEYDATA = "pressuresurveydata";
        const string PRESSURESURVEYENTITYNAME = "PressureSurvey";
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
                /// Load the Test Type list box from WebService
                LoadTestTypes();
                CreateDDLAssets(tdDDLAssets);//DREAM 4.0
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
                if(!this.Page.EnableViewState)
                    this.Page.EnableViewState = true;
                lblException.Visible = false;
                if(!objUtility.IsPostBack(this.Page))
                {
                    btnSubmit_Click(null, EventArgs.Empty);
                }
                if(strSearchName.ToLowerInvariant().Equals(PRESSURESURVEYDATA))
                {
                    trViewMode.Style.Add("display", "none");
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
                xmlWellTestReportRequest = objReportController.CreateSearchRequest(GetRequestInfo(false));
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
        #endregion Protected Methods

        #region Private Methods
        /// <summary>
        /// Loads the test types.
        /// </summary>
        private void LoadTestTypes()
        {
            XmlDocument xmlWellTestTypesResp = new XmlDocument();
            xmlWellTestTypesResp = objReportController.GetSearchResults(GetRequestInfo(true), -1, WELLTESTTYPE.ToUpperInvariant(), string.Empty, -1);
            lstTest_Type.Items.Clear();

            if(xmlWellTestTypesResp != null && xmlWellTestTypesResp.ChildNodes.Count > 0)
            {
                ListItem lstItem = null;
                foreach(XmlNode xmlNodeTestType in xmlWellTestTypesResp.SelectNodes(RECORDXPATH))
                {
                    lstItem = new ListItem();
                    if(xmlNodeTestType.SelectSingleNode(TESTTYPEATTRIBUTEXPATH) != null && xmlNodeTestType.SelectSingleNode(TESTTYPEATTRIBUTEXPATH).Attributes[VALUE] != null)
                    {
                        lstItem.Value = xmlNodeTestType.SelectSingleNode(TESTTYPEATTRIBUTEXPATH).Attributes[VALUE].Value;
                        lstItem.Text = xmlNodeTestType.SelectSingleNode(TESTTYPEATTRIBUTEXPATH).Attributes[VALUE].Value;
                        lstItem.Selected = true;
                        lstTest_Type.Items.Add(lstItem);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the request info.
        /// </summary>
        /// <returns></returns>
        private RequestInfo GetRequestInfo(bool wellTestTypeReq)
        {
            objRequestInfo = new RequestInfo();
            objRequestInfo.Entity = new Entity();
            objRequestInfo.Entity.SessionID = this.Page.Session.SessionID;
            objRequestInfo.Entity.Property = true;

            if(wellTestTypeReq)
            {
                objRequestInfo.Entity.Name = TESTTYPE;
                objRequestInfo.Entity.ResponseType = TABULAR.ToLowerInvariant();
                objRequestInfo.Entity.Attribute = new ArrayList();
                objRequestInfo.Entity.Attribute.Add(objUtility.AddAttribute(TESTTYPEATTRIBUTE, LIKEOPERATOR, new string[] { STAROPERATOR }));
            }
            else
            {
                if(strSearchName.ToLowerInvariant().Equals(PRESSURESURVEYDATA))
                {
                    objRequestInfo.Entity.Name = PRESSURESURVEYENTITYNAME;
                }
                else
                {
                    objRequestInfo.Entity.Name = WELLTESTTYPE;
                }
                if(rbLstDisplayFormat.SelectedItem != null)
                {
                    if(rbLstDisplayFormat.SelectedItem.Text.ToLowerInvariant().Equals(TABULAR.ToLowerInvariant()))
                    {
                        objRequestInfo.Entity.ResponseType = TABULAR;
                    }
                    else if(rbLstDisplayFormat.SelectedItem.Text.ToLowerInvariant().Equals(DATASHEET))
                    {
                        objRequestInfo.Entity.ResponseType = DATASHEET;
                    }
                }
                ArrayList arlAttributeGroup = new ArrayList();
                AttributeGroup objAttributeGroup = new AttributeGroup();
                objAttributeGroup.Operator = ANDOPERATOR;
                objAttributeGroup.Attribute = GetAttributes();
                arlAttributeGroup.Add(objAttributeGroup);
                objRequestInfo.Entity.AttributeGroups = arlAttributeGroup;
            }
            return objRequestInfo;
        }


        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <returns></returns>
        private ArrayList GetAttributes()
        {
            string[] arrWellWellboreID = null;
            ArrayList arlAttribute = new ArrayList();

            AddIdentifierAttribute(arlAttribute);//DREAM 4.0

            if(!string.IsNullOrEmpty(txtStartDate.Text))
            {
                arlAttribute.Add(objUtility.AddAttribute(STARTDATE, GREATERTHANEQUALS, new string[] { txtStartDate.Text.Trim() }));
            }
            if(!string.IsNullOrEmpty(txtEndDate.Text))
            {
                arlAttribute.Add(objUtility.AddAttribute(ENDDATE, LESSTHANEQUALS, new string[] { txtEndDate.Text.Trim() }));
            }
            SetBasinCountryAttribute(arlAttribute, lstTest_Type);
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
        #endregion

        #endregion Private Methods
    }
}
#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: FunctionalityUsageAdvReport.ascx.cs 
#endregion

/// <summary> 
/// This is Functionality Usage Advanced Report class
/// </summary> 
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Net;
using System.Web.Services.Protocols;
using System.Xml;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;
using Microsoft.SharePoint;

namespace Shell.SharePoint.DREAM.Site.UI.UserControls
{
    /// <summary>
    /// The Functionality Usage Advanced Report UI Class
    /// </summary>
    public partial class FunctionalityUsageAdvReport : UIControlHandler
    {
        #region Declaration
        const string SEARCHNAME = "Search Name";
        const string USERNAME = "User Name";
        const string OPERATOR = "Like";
        const string VALUE = "*";
        const string VALUEATTRIBUTE = "value";
        const string USERNAMESREPORT = "usernames";
        const string SEARCNAMESREPORT = "searchnames";
        const string NODEPATH = "/response/report/record/attribute";
        const string ENTITYNAME = "Functionality Usage";
        const string ADVANCE = "Advance";
        const string STARTDATE = "Start Date";
        const string ENDDATE = "End Date";
        const string GTEQ = "GTEQ";
        const string LTEQ = "LTEQ";
        const string AND = "AND";
        const string INEQUALS = "IN";
        const string ADVREPORTREQUESTXML = "ADVREPORTREQUESTXML";
        string REDIRECTIONURL = SPContext.Current.Site.Url + "/Pages/FunctionalityUsage.aspx?type=advanced";

        #endregion

        /// <summary>
        /// Handles the initialization event of the Page control.
        /// </summary>
        protected void Page_Init()
        {
            try
            {

                objReportController = objFactory.GetServiceManager(REPORTSERVICE);
                objResourceController = (ResourceServiceManager)objFactory.GetServiceManager(RESOURCEMANAGER);
                RenderListBoxValues();
                BindTooltipTextToControls();
            }
            catch (Exception ex)
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
                cmdSearch.Attributes.Add("onclick", "return ValidateFURSearchCritetia();");
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }
        /// <summary>
        /// Event handler for the Search Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                XmlDocument xmlAdvReportRequest = null;
                xmlAdvReportRequest = objReportController.CreateSearchRequest(getAdvRequestInfo());
                CommonUtility.SetSessionVariable(this.Page, ADVREPORTREQUESTXML, xmlAdvReportRequest.OuterXml);
                Response.Redirect(REDIRECTIONURL, false);
            }
            catch (XmlException xmlEx)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), xmlEx);
            }
            catch (SoapException soapEx)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), soapEx);
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), webEx);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
        }


        #region Private Method
        /// <summary>
        /// Render listbox values
        /// </summary>       
        private void RenderListBoxValues()
        {



            XmlDocument xmlUserNameRequest = null;

            XmlDocument xmlSearchNameResponse = null;

            XmlDocument xmlUserNameResponse = null;
            try
            {
                xmlUserNameRequest = objReportController.CreateSearchRequest(GetRequestInfo());
                //call two webmethod here
                xmlSearchNameResponse = objResourceController.GetFURReport(null, SEARCNAMESREPORT, -1, null, 0);

                xmlUserNameResponse = objResourceController.GetFURReport(xmlUserNameRequest, USERNAMESREPORT, -1, null, 0);

                PopulateListBoxValue(lstFURSearchName, xmlSearchNameResponse);

                PopulateListBoxValue(lstUserName, xmlUserNameResponse);
            }
            catch (XmlException)
            {
                throw;
            }
            catch (SoapException)
            {
                throw;
            }
            catch (WebException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Get requestinfo object for Username and searchname requests'
        /// </summary>
        /// <param name="requestType">Type of request,Username or Searchname</param>
        /// <returns>RequestInfo object</returns>
        private RequestInfo GetRequestInfo()
        {
            objRequestInfo = new RequestInfo();

            objRequestInfo.Entity = new Entity();

            objRequestInfo.Entity.Criteria = new Criteria();

            try
            {
                objRequestInfo.Entity.Criteria.Operator = OPERATOR;

                objRequestInfo.Entity.Criteria.Value = VALUE;
                objRequestInfo.Entity.Criteria.Name = USERNAME;
            }
            catch (Exception)
            {
                throw;
            }
            return objRequestInfo;
        }
        /// <summary>
        /// Binds values of response xml to listbox 
        /// </summary>
        /// <param name="listBoxControl">Listbox to populate</param>
        /// <param name="xmlResponse">Response XML</param>
        private void PopulateListBoxValue(ListBox listBoxControl, XmlDocument xmlResponse)
        {

            if (xmlResponse != null)
            {
                listBoxControl.Items.Clear();

                XmlNodeList xmlNodeList = xmlResponse.SelectNodes(NODEPATH);

                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    listBoxControl.Items.Add(xmlNode.Attributes[VALUEATTRIBUTE].Value);
                }
            }

        }
        /// <summary>
        /// Create RequestInfo object for Advanced functionality usage report
        /// </summary>
        /// <returns>RequestInfo object</returns>
        private RequestInfo getAdvRequestInfo()
        {
            objRequestInfo = new RequestInfo();

            objRequestInfo.Entity = new Entity();

            try
            {
                objRequestInfo.Entity.Name = ENTITYNAME;

                objRequestInfo.Entity.Type = ADVANCE;

                ArrayList arlAttributeGroup = new ArrayList();

                AttributeGroup objAttributeGroup = new AttributeGroup();

                objAttributeGroup.Operator = AND;

                objAttributeGroup.Attribute = GetAttributes();

                arlAttributeGroup.Add(objAttributeGroup);

                objRequestInfo.Entity.AttributeGroups = arlAttributeGroup;

            }
            catch (Exception)
            {
                throw;
            }
            return objRequestInfo;
        }
        /// <summary>
        /// Gets arraylist of attributes for Advanced functionality usage request xml
        /// </summary>
        /// <returns>ArrayList</returns>
        private ArrayList GetAttributes()
        {
            string strStartDate = string.Empty;
            string strEndDate = string.Empty;
            string[] arrSearchName = null;
            string[] arrUsername = null;
            ArrayList arlAttribute = new ArrayList();

            try
            {
                SetSelectedDates(ref strStartDate, ref strEndDate);

                arlAttribute.Add(AddAttribute(STARTDATE, GTEQ, new string[] { strStartDate }));

                arlAttribute.Add(AddAttribute(ENDDATE, LTEQ, new string[] { strEndDate }));

                arrSearchName = GetSelectedLstBxValues(lstFURSearchName);

                if ((arrSearchName != null) && (arrSearchName.Length > 0))
                {
                    arlAttribute.Add(AddAttribute(SEARCHNAME, INEQUALS, arrSearchName));
                }
                arrUsername = GetSelectedLstBxValues(lstUserName);

                if ((arrUsername != null) && (arrUsername.Length > 0))
                {
                    arlAttribute.Add(AddAttribute(USERNAME, INEQUALS, GetSelectedLstBxValues(lstUserName)));
                }
            }
            catch (Exception)
            {
                throw;
            }

            return arlAttribute;

        }
        /// <summary>
        /// Create an attributes object and assign its properties
        /// </summary>
        /// <param name="name">Name of attribute</param>
        /// <param name="operation">Operation</param>
        /// <param name="value">array of value</param>
        /// <returns>attributes object</returns>
        private Attributes AddAttribute(string name, string operation, string[] values)
        {
            Attributes objAttribute = new Attributes();

            objAttribute.Value = new ArrayList();

            objAttribute.Name = name;

            objAttribute.Operator = operation;
            foreach (string value in values)
            {
                Value objValue = new Value();
                objValue.InnerText = value;
                objAttribute.Value.Add(objValue);
            }
            return objAttribute;
        }
        /// <summary>
        /// Gets list of all selected items of a listbox
        /// </summary>
        /// <param name="listBox">Listbox control</param>
        /// <returns>Array of selected items</returns>
        private string[] GetSelectedLstBxValues(ListBox listBox)
        {

            int[] arrSelectedIndex = listBox.GetSelectedIndices();
            string[] arrValues = new string[arrSelectedIndex.Length];
            int counter = 0;
            foreach (int index in arrSelectedIndex)
            {
                arrValues[counter] = listBox.Items[index].Text;
                counter++;
            }
            return arrValues;
        }

        /// <summary>
        /// Set StartDate and EndDate attributes of request xml
        /// </summary>
        /// <param name="startDate">Ref of startdate string</param>
        /// <param name="endDate">Ref of end date string</param>
        private void SetSelectedDates(ref string startDate, ref string endDate)
        {

            try
            {
                if (rbLast6Month.Checked)
                {
                    startDate = DateTime.Today.AddMonths(-5).ToShortDateString();

                    endDate = DateTime.Today.Date.ToShortDateString();
                }
                else if (rbCurrentYear.Checked)
                {
                    startDate = new DateTime(DateTime.Today.Year, 1, 1).ToShortDateString();

                    endDate = DateTime.Today.Date.ToShortDateString();
                }
                else if (rbLast1Year.Checked)
                {
                    startDate = new DateTime(DateTime.Today.Year - 1, 1, 1).ToShortDateString();

                    endDate = new DateTime(DateTime.Today.Year - 1, 12, 31).ToShortDateString();
                }
                else if (rbLast2Year.Checked)
                {
                    startDate = new DateTime(DateTime.Today.Year - 2, 1, 1).ToShortDateString();

                    endDate = new DateTime(DateTime.Today.Year - 1, 12, 31).ToShortDateString();
                }
                else if (rbSelectDates.Checked)
                {
                    startDate = GetDateSelectedByCalender(objDateTimeConvertorService.ParseDateTime(txtStartDate.Text)).ToShortDateString();
                    endDate = GetDateSelectedByCalender(objDateTimeConvertorService.ParseDateTime(txtEndDate.Text)).ToShortDateString();
                }
                else
                {
                    startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).ToShortDateString();

                    endDate = DateTime.Today.Date.ToShortDateString();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Bind tool tip to controls
        /// </summary>
        private void BindTooltipTextToControls()
        {
            DataTable dtFilterTooltip = new DataTable();

            dtFilterTooltip = AssignToolTip();

            imgSearchNam.ToolTip = GetTooltip(dtFilterTooltip, GetControlID(lstFURSearchName.ID));

            imgUserName.ToolTip = GetTooltip(dtFilterTooltip, GetControlID(lstUserName.ID));

            if (dtFilterTooltip != null)
                dtFilterTooltip.Dispose();

        }

        #endregion

    }
}
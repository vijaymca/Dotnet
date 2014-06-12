#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : EDMReport.ascx.cs
#endregion

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.IO;
using System.Xml;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.DREAM.Site.UI.UserControls
{
    public partial class EDMReport :UIControlHandler
    {
        #region Declaration
        const string STARTDATE = "event_date";
        const string ENDDATE = "event_date";
        const string GTEQ = "GT";
        const string LTEQ = "LT";
        const string EVENTSOURCE = "eventsource";
        const string EVENTCLOSED = "eventclosed";
        const string USERNAME = "username";
        #endregion

        #region Protectted Methods
        /// <summary>
        /// Handles the initialization event of the Page control.
        /// </summary>
        protected void Page_Init()
        {
            try
            {
                PopulateDDLTimePeriod();
                CreateDDLAssets(tdDDLAssets);//DREAM 4.0
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
                cboTimePeriod.Attributes.Add("onchange", "javascript:DateFiltersOptionsChange()");
                btnSubmit.Attributes.Add("onclick", "return ValidateEDMReportSearchCriteria();");
                objReportController = objFactory.GetServiceManager(REPORTSERVICE);
                if(!objUtility.IsPostBack(this.Page))
                {
                    SetDefaultCriteria();
                }
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
        }
        /// <summary>
        /// Handles the Click event of the BtnSubmit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            XmlDocument xmlEDMReportRequest = null;
            xmlEDMReportRequest = objReportController.CreateSearchRequest(GetRequestInfo());
            if(xmlEDMReportRequest != null)
            {
                xmlEDMReportRequest = AddIncludeEndDateAttribute(xmlEDMReportRequest);
                xmlEDMReportRequest = AddEventSourceNode(xmlEDMReportRequest);
                xmlDocRequest = xmlEDMReportRequest;
            }
        }
        #endregion

        #region Private Method

        /// <summary>
        /// Sets the default criteria.
        /// </summary>
        private void SetDefaultCriteria()
        {
            //setting default option as last 7 days               
            cboTimePeriod.SelectedIndex = 1;
            //setting start date and end date values
            string[] strDateRange = AddDateOptions("Last 7 Days").Split(";".ToCharArray());
            txtStartDate.Text = objDateTimeConvertorService.GetDateTime(strDateRange[0]);//Dream 4.0
            txtEndDate.Text = objDateTimeConvertorService.GetDateTime(strDateRange[1]);//Dream 4.0
            //selecting yes option'
            rbLstYesNo.SelectedIndex = 0;
            BtnSubmit_Click(null, EventArgs.Empty);
        }


        /// <summary>
        /// Populates the DDL time period.
        /// </summary>
        private void PopulateDDLTimePeriod()
        {
            for(int intCounter = 1; intCounter < cboTimePeriod.Items.Count; intCounter++)
            {
                cboTimePeriod.Items[intCounter].Value = AddDateOptions(cboTimePeriod.Items[intCounter].Text);
            }
        }

        /// <summary>
        /// Adds the date options.
        /// </summary>
        /// <param name="dropDownText">The drop down text.</param>
        /// <returns></returns>
        private string AddDateOptions(string dropDownText)
        {
            DateTime objStartDate = new DateTime();
            DateTime objEndDate = new DateTime();
            DateTime objTempDate = new DateTime();
            switch(dropDownText)
            {
                case "Last 7 Days":
                    objStartDate = DateTime.Now.AddDays(-6);
                    objEndDate = DateTime.Now;
                    break;
                case "Last 31 Days":
                    objStartDate = DateTime.Now.AddDays(-30);
                    objEndDate = DateTime.Now;
                    break;
                case "Current Week":
                    objStartDate = DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek));
                    objEndDate = DateTime.Now;
                    break;
                case "Current Month":
                    objStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    objEndDate = DateTime.Now;
                    break;
                case "Current Year":
                    objStartDate = new DateTime(DateTime.Now.Year, 1, 1);
                    objEndDate = DateTime.Now;
                    break;
                case "Last Week":
                    objStartDate = DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek + 7));
                    objEndDate = DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek + 1));
                    break;
                case "Last Month":
                    objTempDate = DateTime.Now.AddMonths(-1);
                    objStartDate = new DateTime(objTempDate.Year, objTempDate.Month, 1);
                    objEndDate = objStartDate.AddMonths(1).AddDays(-1);
                    break;
                case "Last year":
                    objTempDate = new DateTime(DateTime.Now.Year, 1, 1);
                    objStartDate = objTempDate.AddYears(-1);
                    objEndDate = objTempDate.AddDays(-1);
                    break;

            }
            return objStartDate.Year.ToString() + "-" + (objStartDate.Month > 9 ? objStartDate.Month.ToString() : "0" + objStartDate.Month.ToString()) + "-" + (objStartDate.Day > 9 ? objStartDate.Day.ToString() : "0" + objStartDate.Day.ToString()) + ";" + objEndDate.Year.ToString() + "-" + (objEndDate.Month > 9 ? objEndDate.Month.ToString() : "0" + objEndDate.Month.ToString()) + "-" + (objEndDate.Day > 9 ? objEndDate.Day.ToString() : "0" + objEndDate.Day.ToString());
        }
        /// <summary>
        /// Gets the request info.
        /// </summary>
        /// <returns></returns>
        private RequestInfo GetRequestInfo()
        {
            objRequestInfo = new RequestInfo();
            Entity objEntity = new Entity();
            objRequestInfo.Entity = objEntity;
            objEntity.Property = true;
            if(rbLstDisplayFormat.SelectedValue.ToLower().Equals(HIERARCHICAL.ToLowerInvariant()))
            {
                objEntity.ResponseType = DATASHEET;
            }
            else
            {
                objEntity.ResponseType = TABULAR;
            }
            objRequestInfo.Entity.AttributeGroups = new ArrayList();
            AttributeGroup objAttributeGroup = new AttributeGroup();
            objAttributeGroup.Operator = ANDOPERATOR;
            objAttributeGroup.Attribute = GetAttributes();
            objRequestInfo.Entity.AttributeGroups.Add(objAttributeGroup);
            return objRequestInfo;
        }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <returns></returns>
        private ArrayList GetAttributes()
        {
            ArrayList arlAttribute = new ArrayList();

            AddIdentifierAttribute(arlAttribute);

            arlAttribute.Add(objUtility.AddAttribute(STARTDATE, GTEQ, new string[] { objDateTimeConvertorService.ParseDateTime(txtStartDate.Text.Trim()) }));

            arlAttribute.Add(objUtility.AddAttribute(ENDDATE, LTEQ, new string[] { objDateTimeConvertorService.ParseDateTime(txtEndDate.Text.Trim()) }));

            return arlAttribute;
        }
        /// <summary>
        /// Adds the event source node.
        /// </summary>
        /// <param name="requestXML">The request XML.</param>
        /// <returns></returns>
        private XmlDocument AddEventSourceNode(XmlDocument requestXML)
        {
            XmlNode eventSourceNode = requestXML.CreateElement(EVENTSOURCE);
            XmlNode xmlNodeValue = null;
            foreach(ListItem item in chbLstReportLevel.Items)
            {
                if((rbLstDisplayFormat.SelectedValue.ToLowerInvariant().Equals(HIERARCHICAL.ToLowerInvariant())) || (item.Selected == true))
                {
                    xmlNodeValue = requestXML.CreateElement(VALUE);
                    xmlNodeValue.InnerText = item.Value;
                    eventSourceNode.AppendChild(xmlNodeValue);
                }
            }
            requestXML.SelectSingleNode(ENTITYPATH).AppendChild(eventSourceNode);
            return requestXML;
        }
        /// <summary>
        /// Adds the include end date attribute.
        /// </summary>
        /// <param name="requestXML">The request XML.</param>
        /// <returns></returns>
        private XmlDocument AddIncludeEndDateAttribute(XmlDocument requestXML)
        {
            XmlNode entityNode = requestXML.SelectSingleNode(ENTITYPATH);
            const string USERNAME = "UserName";
            if(entityNode != null)
            {
                XmlAttribute eventClosed = requestXML.CreateAttribute(EVENTCLOSED);
                entityNode.Attributes.Append(eventClosed);
                if(rbLstYesNo.SelectedIndex == 0)
                {
                    eventClosed.Value = TRUE;
                }
                else
                {
                    eventClosed.Value = FALSE;
                }
                //converting username attribute to lower case
                XmlAttribute EntityUserName = requestXML.CreateAttribute(USERNAME);
                entityNode.Attributes.Append(EntityUserName);
                EntityUserName.Value = entityNode.Attributes[USERNAME].Value;
                entityNode.Attributes.Remove(entityNode.Attributes[USERNAME]);
            }
            return requestXML;
        }
        #endregion
    }
}


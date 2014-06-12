#region Shell Copyright.2011
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : SignificantWellEvents.ascx.cs
#endregion
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services.Protocols;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;
using System.Text.RegularExpressions;
using System.Xml;

namespace Shell.SharePoint.DREAM.Site.UI.UserControls
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SignificantWellEvents :UIControlHandler
    {

        #region Declaration
        const string EVENTGROUP = "EventGroupCode";
        const string EVENTTYPE = "EventName";
        const string PRIORITY = "Priority";
        const string CREATEDBY = "CreatedBy";
        const string UPDATEDBY = "UpdatedBy";
        const string OWNER = "Owner";
        const string SEARCHOWNER = "owner";
        const string SEARCHRESOURCE = "resourcetype";
        const string SEARCHEVENTTYPE = "eventname";
        const string SEARCHEVENTGROUPCODE = "eventgroupcode";
        const string XPATHRESPONSE = "/response/report/record";
        const string ATTOWNERID = "Owner ID";
        const string ATTOWNERNAME = "Owner Name";
        const string ATTRESOURCETYPE = "ResourceType";
        const string ATTEVENTCODE = "EventCode";
        const string ATTGROUPNAME = "EventGroupName";
        const string ATTGROUPCODE = "EventGroupCode";
        const string ATTEVENTNAME = "EventTypeName";
        const string SWEDURL = "SWED URL";
        const string DDLCLASS = "ddlSignificantWellEvents";
        #endregion

        #region Methods

        #region Protected methods
        /// <summary>
        /// Handles the initialization event of the Page control.
        /// </summary>
        protected void Page_Init()
        {
            try
            {
                objEventServiceController = objFactory.GetServiceManager(EVENTSERVICE);
                objReportController = objFactory.GetServiceManager(REPORTSERVICE);
                if(!objUtility.IsPostBack(this.Page))
                {
                    objUtility = new CommonUtility();
                    objRequestInfo = new RequestInfo();
                    objRequestInfo = SetSearchRequestObject(EVENTGROUP);
                    cblEventsGroup.Items.Clear();
                    cblEventsGroup.Items.Add(DEFAULTDROPDOWNTEXT);
                    LoadSearch(objRequestInfo, EVENTGROUP, cblEventsGroup);
                    objRequestInfo = SetSearchRequestObject(EVENTTYPE);
                    cblEventsType.Items.Clear();
                    cblEventsType.Items.Add(DEFAULTDROPDOWNTEXT);
                    LoadSearch(objRequestInfo, EVENTTYPE, cblEventsType);
                    string strCurrUserName = objUtility.GetUserName();
                    txtCREATEDBY.Text = strCurrUserName;
                    txtUPDATEDBY.Text = strCurrUserName;
                }
                //DREAM 4.0
                CreateDDLAssets(tdDDLAssets);
                ddlAssets.CssClass = DDLCLASS;
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
        }
        /// <summary>
        /// Loads the control and 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if(!objUtility.IsPostBack(this.Page))
                {
                    BtnFilter_Click(null, EventArgs.Empty);
                    hdnSwedUrl.Value = PortalConfiguration.GetInstance().GetKey(SWEDURL);
                }

                btnFilter.Attributes.Add("onclick", "return CheckFilterValidation();");
                Page.ClientScript.RegisterStartupScript(typeof(string), "ShowFilter", "HideExpandFilter('expImage','FilterDiv');", true);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "AsyncShowFilter", "<script language=\"javascript\">try{HideExpandFilter('expImage','FilterDiv');MakeBoldCheckBoxSelected();}catch(Ex){}</script>", false);
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex, 1);
                lblErrorMessage.Text = ex.Message;
            }
            lblErrorMessage.Visible = false;
        }

        /// <summary>
        /// Filters the results and save the options in Session
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnFilter_Click(object sender, EventArgs e)
        {
            ArrayList arlFilterOption = new ArrayList();
            XmlDocument xmlWellEventsRequest = null;
            if(cboEventGroup.Checked)
                arlFilterOption.Add(EVENTGROUP + "#" + cblEventsGroup.SelectedValue);
            if(cboEventType.Checked)
                arlFilterOption.Add(EVENTTYPE + "#" + cblEventsType.Items[cblEventsType.SelectedIndex].Text);

            string strPriorty = string.Empty;

            if(cboEventPriority.Checked)
            {
                if(chbHigh.Checked)
                    strPriorty += ";H";

                if(chbMedium.Checked)
                    strPriorty += ";M";

                if(chbLow.Checked)
                    strPriorty += ";L";
            }

            if(!string.IsNullOrEmpty(strPriorty))
                arlFilterOption.Add(PRIORITY + "#" + strPriorty);
            if(cboCreatedBy.Checked)
                arlFilterOption.Add(CREATEDBY + "#" + txtCREATEDBY.Text);
            if(cboUpdatedBy.Checked)
                arlFilterOption.Add(UPDATEDBY + "#" + txtUPDATEDBY.Text);
            if(cboOwnedBy.Checked)
                arlFilterOption.Add(OWNER + "#" + txtOWNEDBY.Text);

            if(arlFilterOption.Count == 0)
            {
                xmlWellEventsRequest = objReportController.CreateSearchRequest(GetWellEventRequestObject(arlFilterOption, false));
            }
            else
            {
                xmlWellEventsRequest = objReportController.CreateSearchRequest(GetWellEventRequestObject(arlFilterOption, true));
            }
            //DREAM 4.0
            if(xmlWellEventsRequest != null)
            {
                xmlDocRequest = xmlWellEventsRequest;
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Sets the search request object.
        /// </summary>
        /// <param name="srchType">Type of the SRCH.</param>
        /// <returns></returns>
        private RequestInfo SetSearchRequestObject(string srchType)
        {
            objRequestInfo = new RequestInfo();
            Entity objEntity = new Entity();
            objEntity.Property = true;
            Criteria objCriteria = new Criteria();
            objCriteria.Name = srchType;
            objCriteria.Operator = LIKEOPERATOR;
            objCriteria.Value = STAROPERATOR;
            objEntity.Criteria = objCriteria;
            objRequestInfo.Entity = objEntity;
            return objRequestInfo;
        }

        /// <summary>
        /// Loads the search.
        /// </summary>
        /// <param name="requestInfo">The request info.</param>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="comboDropDown">The combo drop down.</param>
        private void LoadSearch(RequestInfo requestInfo, string searchType, ListControl comboDropDown)
        {
            XmlDocument xmlDocResponse = null;
            XmlNodeList xmlNodeLstRecords = null;
            try
            {
                xmlDocResponse = objEventServiceController.GetSearchResults(requestInfo, 150, searchType, null, 0);
                xmlNodeLstRecords = xmlDocResponse.SelectNodes(XPATHRESPONSE);

                //Loads the list control with the values based on the SearchType parameter
                switch(searchType.ToLowerInvariant())
                {
                    case SEARCHEVENTTYPE:
                        {
                            foreach(XmlNode reportNode in xmlNodeLstRecords)
                            {
                                comboDropDown.Items.Add(new ListItem(reportNode.SelectSingleNode("attribute[@name='" + ATTEVENTNAME + "']").Attributes["value"].Value, reportNode.SelectSingleNode("attribute[@name='" + ATTEVENTCODE + "']").Attributes["value"].Value));
                            }
                            break;
                        }
                    case SEARCHEVENTGROUPCODE:
                        {
                            foreach(XmlNode reportNode in xmlNodeLstRecords)
                            {
                                comboDropDown.Items.Add(new ListItem(reportNode.SelectSingleNode("attribute[@name='" + ATTGROUPNAME + "']").Attributes["value"].Value, reportNode.SelectSingleNode("attribute[@name='" + ATTGROUPCODE + "']").Attributes["value"].Value));
                            }
                            break;
                        }
                }
            }
            catch(SoapException soapEx)
            {
                if(!string.Equals(soapEx.Message.ToString(), SOAPEXCEPTIONMESSAGE))
                {
                    CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                }
                lblErrorMessage.Text = SOAPEXCEPTIONMESSAGE;
            }
            catch(WebException webEx)
            {
                lblErrorMessage.Text = webEx.Message;
            }
        }

        /// <summary>
        /// Gets the well event request object.
        /// </summary>
        /// <param name="filterCriterias">The filter criterias.</param>
        /// <param name="includeFilterOptions">if set to <c>true</c> [include filter options].</param>
        /// <returns></returns>
        private RequestInfo GetWellEventRequestObject(ArrayList filterCriterias, bool includeFilterOptions)
        {
            Attributes objAttribute = null;
            objRequestInfo = new RequestInfo();
            Entity objEntity = new Entity();
            objEntity.Property = true;
            objEntity.ResponseType = TABULAR;
            ArrayList arlAttribute = new ArrayList();

            arlAttribute = objEntity.Attribute;
            if(arlAttribute == null)
                arlAttribute = new ArrayList();

            AddIdentifierAttribute(arlAttribute);//DREAM 4.0
            if(includeFilterOptions)
            {
                AttributeGroup objAttributeGroup = new AttributeGroup();
                objAttributeGroup.Operator = ANDOPERATOR;
                objAttributeGroup.Attribute = new ArrayList();

                string strFilter;
                string strFilterVal;
                foreach(string strVal in filterCriterias)
                {
                    strFilter = strVal.Split("#".ToCharArray())[0];
                    strFilterVal = strVal.Split("#".ToCharArray())[1];

                    objAttribute = new Attributes();
                    objAttribute.Name = strFilter;

                    if(string.Equals(strFilter, PRIORITY))
                    {
                        if(strFilterVal.Split(";".ToCharArray()).Length > 1)
                        {
                            objAttribute.Operator = INOPERATOR;
                            foreach(string strFilVal in strFilterVal.Split(";".ToCharArray()))
                            {
                                if(!string.IsNullOrEmpty(strFilVal))
                                {
                                    objAttribute = AddValue(objAttribute, strFilVal);
                                }
                            }
                        }
                        else
                        {
                            objAttribute.Operator = LIKEOPERATOR;
                            objAttribute = AddValue(objAttribute, strFilterVal);
                        }

                    }
                    else
                    {
                        objAttribute.Operator = LIKEOPERATOR;
                        objAttribute = AddValue(objAttribute, strFilterVal);
                    }

                    if(!arlAttribute.Contains(objAttribute))
                    {
                        arlAttribute.Add(objAttribute);
                    }
                }

                objAttributeGroup.Attribute = arlAttribute;
                objEntity.AttributeGroups = new ArrayList();
                if(!objEntity.AttributeGroups.Contains(objAttributeGroup))
                {
                    objEntity.AttributeGroups.Add(objAttributeGroup);
                }

            }
            else
            {
                objEntity.Attribute = arlAttribute;
            }

            objRequestInfo.Entity = objEntity;
            return objRequestInfo;
        }

        /// <summary>
        /// Adds the value.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private Attributes AddValue(Attributes attribute, string value)
        {
            Value objValue = new Value();
            ArrayList arlValue = attribute.Value;
            if(arlValue == null)
                arlValue = new ArrayList();

            objValue.InnerText = value;

            if(!arlValue.Contains(objValue))
                arlValue.Add(objValue);

            attribute.Value = arlValue;
            return attribute;
        }
        #endregion

        #endregion
    }
}
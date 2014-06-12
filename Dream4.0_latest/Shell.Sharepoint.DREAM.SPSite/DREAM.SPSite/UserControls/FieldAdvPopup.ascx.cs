#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: FieldAdvPopup.ascx.cs 
#endregion
using System;
using System.Data;
using System.Xml;
using System.IO;
using System.Xml.Xsl;
using System.Text;
using System.Collections;
using System.Web.Services.Protocols;
using System.Net;
using System.Web.UI;

using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DREAM.Controller;


namespace Shell.SharePoint.DREAM.Site.UI.UserControls
{

    /// <summary>
    /// This is a Field Selection Pop Up class for Field Adv Search screen
    /// </summary>
    public partial class FieldAdvPopup :UIControlHandler
    {
        #region Declarations
        string strAttributeName = string.Empty;
        string strEntityName = string.Empty;
        #endregion

        #region PageLoad
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string strControlId = string.Empty;
                objUIUtilities = new UIUtilities();
                objReportController = objFactory.GetServiceManager(REPORTSERVICE);
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                SetUserPreferences();
                if(Request.QueryString[SRPADVPOPUPID] != null)
                {
                    if(Request.QueryString[SRPADVPOPUPID].Length > 0)
                    {
                        strControlId = Request.QueryString[SRPADVPOPUPID];
                    }
                }

                if(Request.QueryString[SRPADVPOPUPBASIN] != null)
                {
                    if(string.Equals(Request.QueryString[SRPADVPOPUPBASIN].ToLowerInvariant(), "true"))
                    {
                        lblName.Text = BASINIDENTIFIER;
                        strAttributeName = BASINITEMVAL;
                        strEntityName = BASINITEMVAL;

                        //**R5k changes for Dream 4.0
                        //Starts
                        // SetDataSource(SRPADVPOPUPBASIN, "Title", "Title");
                        PopulateListControl(lstFieldNames, GetBasinFromWebService(), BASINXPATH, objUserPreferences.Basin);
                        //Ends
                    }

                }
                else if(Request.QueryString[SRPADVPOPUPOPERATOR] != null)
                {
                    if(string.Equals(Request.QueryString[SRPADVPOPUPOPERATOR].ToLowerInvariant(), "true"))
                    {
                        lblName.Text = SRPADVPOPUPOPERATOR;
                        strAttributeName = SRPADVPOPUPOPERATOR;//.ToLowerInvariant();
                        strEntityName = FIELDOPERATOR;
                        SetDataSource(SRPADVPOPUPOPERATOR, "Operator", string.Empty);
                    }
                }

                cmdClose.Attributes.Add(JAVASCRIPTONCLICK, "CloseWindow();");
                StringBuilder strSelectJavaScript = new StringBuilder();
                strSelectJavaScript.Append("GetSeletectedItem('");
                strSelectJavaScript.Append(lstFieldNames.ClientID);
                strSelectJavaScript.Append("','");
                strSelectJavaScript.Append(strControlId);
                strSelectJavaScript.Append("','");
                strSelectJavaScript.Append(lblName.Text);
                strSelectJavaScript.Append("');");
                cmdSelect.Attributes.Add(JAVASCRIPTONCLICK, strSelectJavaScript.ToString());
            }
            catch(SoapException soapEx)
            {
                if(soapEx.Message.Equals(SOAPEXCEPTIONMESSAGE))
                {
                    lblMessage.Text = NORECORDFOUNDSEXCEPTIONMESSAGE;
                    lblMessage.Visible = true;
                }
                else
                {
                    lblMessage.Text = UNEXPECTEDEXCEPTIONMESSAGE;
                    lblMessage.Visible = true;
                }
                CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
            }
            catch(WebException webEx)
            {
                lblMessage.Text = UNEXPECTEDEXCEPTIONMESSAGE;
                lblMessage.Visible = true;
                CommonUtility.HandleException(strCurrSiteUrl, webEx, 1);
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }
        #endregion

        #region Data Source Binding
        /// <summary>
        /// Sets the data source.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="dataTextName">Name of the data text.</param>
        /// <param name="dataValueName">Name of the data value.</param>
        private void SetDataSource(string type, string dataTextName, string dataValueName)
        {
            DataTable dtOperator = null;
            try
            {
                dtOperator = GetDataForListBox(type);
                if(dtOperator != null && dtOperator.Rows.Count > 0)
                {
                    lstFieldNames.DataSource = dtOperator;

                    lstFieldNames.DataTextField = dataTextName;
                    if(!string.IsNullOrEmpty(dataValueName))
                    {
                        lstFieldNames.DataValueField = dataValueName;
                    }
                    lstFieldNames.DataBind();
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = NORECORDFOUNDSEXCEPTIONMESSAGE;
                }
            }
            finally
            {
                if(dtOperator != null)
                {
                    if(dtOperator.DataSet != null)
                        dtOperator.DataSet.Dispose();
                }
            }
        }
        /// <summary>
        /// Query the names from webservice
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private DataTable GetDataForListBox(string type)
        {
            if(string.Equals(type.ToLowerInvariant(), BASINITEMVAL.ToLowerInvariant()))
            {
                return GetBasinFromSPList(string.Empty);
            }
            else
            {
                ArrayList arrListValue = new ArrayList();
                XmlDocument xmlDoc = new XmlDocument();
                arrListValue.Add(lstFieldNames);
                SetListValues(arrListValue, "*");
                if(string.Equals(type.ToLowerInvariant(), FIELDITEMVAL.ToLowerInvariant()))
                {
                    xmlDoc = objReportController.GetSearchResults(objRequestInfo, intListValMaxRecord, FIELD, null, 0);
                    if(xmlDoc != null)
                    {
                        return GetData(xmlDoc, GetXslForRadResponseXml(), FIELD).Tables[3];
                    }
                }
                else
                {
                    xmlDoc = objReportController.GetSearchResults(objRequestInfo, intListValMaxRecord, FIELDOPERATOR, null, 0);
                    if(xmlDoc != null)
                    {
                        return GetData(xmlDoc, GetXslForRadResponseXml(), FIELDOPERATOR).Tables[3];
                    }
                }
                return null;
            }
        }
        #endregion

        #region XmlTransforming

        /// <summary>
        /// Query the Fields names from webservice
        /// </summary>
        /// <param name="objResponseXml">The obj response XML.</param>
        /// <param name="objXmlTextReader">The obj XML text reader.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        private DataSet GetData(XmlDocument objResponseXml, XmlTextReader objXmlTextReader, string fieldName)
        {
            XmlDocument xmlDocForXSL = new XmlDocument();
            StringWriter objStringWriter = new StringWriter();
            XslCompiledTransform objCompiledTransform = new XslCompiledTransform();
            XsltArgumentList xsltArgsList = new XsltArgumentList();
            DataSet dsOutput = new DataSet();

            if(objResponseXml != null)
            {
                switch(fieldName)
                {
                    case FIELD:
                        {
                            xsltArgsList.AddParam("PARAM1", string.Empty, "Field Name");
                            xsltArgsList.AddParam("PARAM2", string.Empty, "Field Identifier");
                            break;
                        }
                    case FIELDOPERATOR:
                        {
                            xsltArgsList.AddParam("PARAM1", string.Empty, "Operator");
                            break;
                        }

                }
                xmlDocForXSL.Load(objXmlTextReader);
                objCompiledTransform.Load(xmlDocForXSL);
                objCompiledTransform.Transform(new XmlNodeReader(objResponseXml), xsltArgsList, objStringWriter);
                objResponseXml.LoadXml(objStringWriter.ToString());
                objStringWriter.Flush();
                objStringWriter.Close();
                objStringWriter.Dispose();
                dsOutput.ReadXml(new XmlNodeReader(objResponseXml));
                objXmlTextReader.Close();
            }
            return dsOutput;
        }
        #endregion

        #region Request Xml  Genration
        /// <summary>
        /// Sets the list values.
        /// </summary>
        /// <param name="fieldsGroup">The fields group.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private RequestInfo SetListValues(ArrayList fieldsGroup, string value)
        {
            objRequestInfo.Entity = SetEntity(fieldsGroup, value, strAttributeName, strEntityName);
            return objRequestInfo;
        }

        /// <summary>
        /// Sets the entity.
        /// </summary>
        /// <param name="fieldsGroup">The fields group.</param>
        /// <param name="userEnteredValue">The User Entered Value</param>
        /// <param name="attributename">The attributename.</param>
        /// <param name="entityname">The entityname.</param>
        /// <returns></returns>
        private Entity SetEntity(ArrayList fieldsGroup, string userEnteredValue, string attributename, string entityname)
        {
            Entity objEntity = new Entity();

            if(fieldsGroup.Count == 1 && fieldsGroup[0] != null)
            {
                Control radControl = (Control)fieldsGroup[0];
                ArrayList arlAttribute = new ArrayList();
                Attributes objAttribute = new Attributes();
                objAttribute.Name = attributename;
                ArrayList arlValue = new ArrayList();
                Value objValue = new Value();
                objValue.InnerText = userEnteredValue;
                arlValue.Add(objValue);
                objAttribute.Value = arlValue;
                objAttribute.Operator = GetOperator(objAttribute.Value);
                arlAttribute.Add(objAttribute);
                objEntity.Attribute = arlAttribute;
                objEntity.Criteria = SetCriteria();
                objEntity.Name = entityname;
            }
            else
            {
                objEntity.Criteria = SetCriteria();
            }
            return objEntity;
        }

        /// <summary>
        /// Sets the criteria.
        /// </summary>
        /// <returns></returns>
        private Criteria SetCriteria()
        {
            Criteria objCriteria = new Criteria();
            objCriteria.Value = STAROPERATOR;
            objCriteria.Operator = GetOperator(objCriteria.Value);
            return objCriteria;
        }
        #endregion
    }
}
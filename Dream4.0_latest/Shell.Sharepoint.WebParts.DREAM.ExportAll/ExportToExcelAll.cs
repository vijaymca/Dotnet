#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : ExportToExcelAll.cs
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Drawing;
using System.Xml;
using System.Web.Services.Protocols;
using System.Runtime.InteropServices;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DREAM.SearchHelper;
using SpreadsheetGear;

namespace Shell.Sharepoint.WebParts.DREAM.ExportAll
{
    /// <summary>
    /// Export To Excel Class.
    /// </summary>
    [Guid("cd72d294-ea07-4638-8bd1-38b6bc95f7d5")]
    public class ExportToExcelAll : SearchHelper
    {

        #region Declaration

        #region Constants
        const string SELECTALLCOLUMNS = "ALLCOLUMNS";
        const string REPORTHEADER = "Export to Excel All";
        const string NORECORDS = "No records found.";
        const string NODOCUMENTS = "No documents found.";
        const string ASSETTYPE = "assetType";
        const string EPCATALOGFILENAME = "Document Results";
        const string SEARCHTYPEQS = "Searchtype";
        const string EDMREPORTREQUESTXML = "EDMREPORTREQUESTXML";
        const string DIRECTIONALSURVEYCHARTRESULTS = "directionalsurveydetailchartresults";
        const string PICKSCHARTRESULTS = "picksdetailchartresults";
        const string PALEOMARKERS = "paleomarkers";
        const double FEETMETERCONSTANT = 3.28084;
        const string FEETMETERRADIOBUTTONID = "FeetMeters";
        #endregion

        #region Variables
        string strSearchType = string.Empty;
        string strColIdentifierItem = string.Empty;
        string strReportType = string.Empty;
        string strFileType = string.Empty;
        Boolean blnChkExported;
        AbstractController objQueryBuildController;
        //Dream 4.0 
        string strSelectedRows = string.Empty;
        string strSelectedCriteriaName = string.Empty;
        string strSearchName = string.Empty;
        string strUserSelectedUnit = string.Empty;
        string strQuickSearch = string.Empty;
        #endregion

        #region Controls
        Label lblMessage;
        #endregion
        #endregion

        #region Protected Methods
        ///// <summary>
        ///// Create required controls for webpart
        ///// </summary>
        protected override void CreateChildControls()
        {
            strCurrSiteUrl = HttpContext.Current.Request.Url.ToString();
            objFactory = new ServiceProvider();
            objReportController = objFactory.GetServiceManager(REPORTSERVICE);
            base.CreateChildControls();
            lblMessage = new Label();
            lblMessage.Text = string.Empty;
            lblMessage.Visible = false;
            this.Controls.Add(lblMessage);
        }
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            try
            {
                if (Page.Request.QueryString["Requestid"] != null)
                    RequestID = Page.Request.QueryString["Requestid"].ToString();
                if (Page.Request.QueryString[SEARCHTYPEQS] != null)
                    strSearchType = Page.Request.QueryString[SEARCHTYPEQS].ToString();
                if (HttpContext.Current.Request.Form["hidSelectedColumns"] != null)
                    strColIdentifierItem = HttpContext.Current.Request.Form["hidSelectedColumns"].ToString();
                if (HttpContext.Current.Request.Form["hidDetailSelectedColumns"] != null)
                    strColIdentifierItem = HttpContext.Current.Request.Form["hidDetailSelectedColumns"].ToString();
                if (Page.Request.QueryString["ReportType"] != null)
                    strReportType = Page.Request.QueryString["ReportType"].ToString();
                if (Page.Request.QueryString["FileType"] != null)
                    strFileType = Page.Request.QueryString["FileType"].ToString();
                //Dream 4.0
                if (HttpContext.Current.Request.Form["hidSelectedRows"] != null)
                {
                    strSelectedRows = HttpContext.Current.Request.Form["hidSelectedRows"].ToString();
                }
                if (HttpContext.Current.Request.Form["hidSelectedCriteriaName"] != null)
                {
                    strSelectedCriteriaName = HttpContext.Current.Request.Form["hidSelectedCriteriaName"].ToString();
                }
                if (HttpContext.Current.Request.Form["hidQuickSearch"] != null)
                {
                    strQuickSearch = HttpContext.Current.Request.Form["hidQuickSearch"].ToString();
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);
            }
        }
        /// <summary>
        /// Method use to render the created controls on page.
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            XmlDocument XMLDocResponse = null;
            string strSearchType = string.Empty;
            strUserSelectedUnit = GetUserSelectedUnit();
            try
            {
                XMLDocResponse = new XmlDocument();
                if (Page.Request.QueryString[SEARCHTYPEQS] != null)
                {
                    strSearchType = Page.Request.QueryString[SEARCHTYPEQS];
                }

                //Gets the Report service result.
                #region Getting Response xml
                //if strQuickSearch is not empty then its a qucik search or advance search page export all
                //in that case get export all request xml from webservice
                if (!string.IsNullOrEmpty(strQuickSearch))
                {
                    //Gets the EDMReport result.
                    XMLDocResponse = GetResponseFromSessionRequest(EXPORTALLREQUESTXML, REPORTSERVICE);
                }
                else if (EDMREPORT.Equals(strSearchType.ToLowerInvariant()))
                {
                    //Gets the EDMReport result.
                    XMLDocResponse = GetResponseFromSessionRequest(EDMREPORTREQUESTXML,EVENTSERVICE);
                }
                else if (strSearchType.ToLowerInvariant().Equals(RECALLCURVE))
                {
                    XMLDocResponse = GetRecallCurveResponseXml(strSearchType);
                }
                else
                {
                    XMLDocResponse = GetResponseXML(strSearchType);
                }
                #endregion

                #region Exporting response xml to xlsheet
                if (XMLDocResponse != null)
                {
                    if (EDMREPORT.Equals(strSearchType.ToLowerInvariant()))
                    {
                        //Exports the EDMReport result to Excel.
                        ExportEDMResultsToExcel(XMLDocResponse);
                        blnChkExported = true;
                    }
                    else if (strReportType.ToLowerInvariant().Equals(DIRECTIONALSURVEYCHARTRESULTS) ||
           strReportType.ToLowerInvariant().Equals(PICKSCHARTRESULTS))
                    {
                        ExportChartDataToExcel(XMLDocResponse, strReportType);
                    }
                    //Exports the report service result to Excel.
                    else if (PALEOMARKERS.Equals(strSearchType.ToLowerInvariant()))
                    {
                        ExportPaleoMarkersReport(XMLDocResponse);
                    }
                    else if (PRESSURESURVEYDATA.Equals(strSearchType.ToLowerInvariant()))
                    {
                        ExportPressureSurveyReport(XMLDocResponse);
                    }
                    else if (IsNameExist(SEARCHNAMEFORUBIWRKSHEET, strSearchType))
                    {
                        ExportUBIToWrkSheet(XMLDocResponse);
                    }
                    else
                    {
                        ExportXMLResultsTOExcel(XMLDocResponse);
                    }
                    blnChkExported = true;
                }
                else
                {
                    //When xml document is null renders error message.
                    RenderException(writer, NORECORDS);
                }
                #endregion

            }
            catch (SoapException soapEx)
            {
                if (!string.Equals(soapEx.Message, NORECORDSFOUND))
                {
                    CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                }
                RenderException(writer, soapEx.Message);
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(strCurrSiteUrl, webEx, 1);
                RenderException(writer, webEx.Message);
            }
            catch (Exception Ex)
            {
                RenderException(writer, Ex.Message);
            }
            if (blnChkExported)
                HttpContext.Current.Response.End();
        }
        #endregion

        #region Private Methods

        #region Render Methods
        /// <summary>
        /// Renders the parent table.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="searchName">Search Name.</param>
        private void RenderParentTable(HtmlTextWriter writer, string searchName)
        {
            writer.Write("<table class=\"tableAdvSrchBorder\" cellspacing=\"0\" cellpadding=\"4\" border=\"0\" width=\"100%\">");
            writer.Write("<tr><td class=\"tdAdvSrchHeader\" colspan=\"4\" valign=\"top\"><B>" + searchName + " Results</b></td></tr>");
        }
        /// <summary>
        /// Renders the exception.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="message">The message.</param>
        private void RenderException(HtmlTextWriter writer, string message)
        {
            RenderParentTable(writer, REPORTHEADER);
            writer.Write("<tr><td><br/>");
            lblMessage.Visible = true;
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Text = message;
            lblMessage.RenderControl(writer);
            writer.Write("</td></tr></table>");
        }
        /// <summary>
        /// Renders the XL sheet.
        /// </summary>
        /// <param name="workbook">The workbook.</param>
        private void RenderXLSheet(IWorkbook workbook)
        {
            // Stream the Excel spreadsheet to the client in a format
            // compatible with Excel 97/2000/XP/2003/2007.
            HttpContext.Current.Response.Clear();

            if (!string.IsNullOrEmpty(strFileType))
            {
                if (strFileType.ToLowerInvariant().Equals("csv"))
                {
                    //text/csv
                    //string attachment = "attachment; filename=SearchResults.csv";
                    //HttpContext.Current.Response.ContentType = "text/csv";
                    //HttpContext.Current.Response.AddHeader("Content-Disposition", attachment);
                    //workbook.SaveToStream(HttpContext.Current.Response.OutputStream, FileFormat.CSV);

                    string attachment = "filename=SearchResults.csv";
                    HttpContext.Current.Response.ContentType = "application/force-download";
                    HttpContext.Current.Response.AddHeader("Content-Disposition", attachment);
                    workbook.SaveToStream(HttpContext.Current.Response.OutputStream, FileFormat.CSV);
                }
                else if (strFileType.ToLowerInvariant().Equals("excel"))
                {
                    HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=SearchResults.xls");
                    workbook.SaveToStream(HttpContext.Current.Response.OutputStream, FileFormat.Excel8);
                    HttpContext.Current.Response.End();
                }
                else
                {
                    HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=SearchResults.xls");
                    workbook.SaveToStream(HttpContext.Current.Response.OutputStream, FileFormat.Excel8);
                    HttpContext.Current.Response.End();
                }
            }
            else
            {
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=SearchResults.xls");
                workbook.SaveToStream(HttpContext.Current.Response.OutputStream, FileFormat.Excel8);
                HttpContext.Current.Response.End();
            }
            

        }
        #endregion

        #region Get Response XML
        /// <summary>
        /// Gets the result XML doument.
        /// </summary>
        /// <returns></returns>
        private XmlDocument GetResponseXml()
        {
            int intMaxRecords = 0;
            XmlDocument xmlDocResponse;
            objQueryBuildController = objFactory.GetServiceManager(QUERYSERVICE);
            objRequestInfo = new RequestInfo();
            xmlDocResponse = new XmlDocument();

            intMaxRecords = Convert.ToInt32(PortalConfiguration.GetInstance().GetKey(MAXRECORDS));
            base.ResponseType = "Tabular";
            objRequestInfo = SetBasicDataObjects(string.Empty, string.Empty, null, true, true, intMaxRecords);
            if (strSearchType.Equals("Query Search"))
            {
                xmlDocResponse = objQueryBuildController.GetSearchResults(objRequestInfo, intMaxRecords, strSearchType, null, 0);
            }
            else
            {
                xmlDocResponse = objReportController.GetSearchResults(objRequestInfo, intMaxRecords, strSearchType, null, 0);
            }
            return xmlDocResponse;
        }
        /// <summary>
        /// Gets the EDM report results.
        /// </summary>
        /// <returns></returns>
        private XmlDocument GetResponseFromSessionRequest(string sessionVariableName,string reportName)
        {
            XmlDocument xmlDocResponse = null;
            XmlDocument xmlDocRequestXml = new XmlDocument();
            int intMaxRecords = 0;
            xmlDocRequestXml.LoadXml((string)CommonUtility.GetSessionVariable(Page, sessionVariableName));
            if (xmlDocRequestXml != null)
            {
                if (Page.Request.QueryString["MaxRecord"] != null)
                {
                    intMaxRecords = Convert.ToInt32(Page.Request.QueryString["MaxRecord"].ToString());
                }
                objReportController = objFactory.GetServiceManager(reportName);
                xmlDocResponse = objReportController.GetSearchResults(xmlDocRequestXml, intMaxRecords, strSearchType.ToUpper(), null, 0); ;
            }
            return xmlDocResponse;
        }
        #endregion

        #region Supporting Methods
        /// <summary>
        /// Creates the column query.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        /// <param name="nameOperator">The name operator.</param>
        /// <param name="colOperator">The col operator.</param>
        /// <returns></returns>
        private string CreateColumnQuery(string[] columnNames, string nameOperator, string colOperator)
        {
            string strQuery = string.Empty;
            foreach (string str in columnNames)
            {
                if (!string.IsNullOrEmpty(str.Trim()))
                {
                    strQuery += "@name" + nameOperator + "'" + str.Trim() + "' " + colOperator + " ";
                }
            }
            return strQuery.Substring(0, strQuery.LastIndexOf('\'') + 1);
        }

        /// <summary>
        /// Gets the measurement unit.
        /// </summary>
        /// <param name="currentNode">The current node.</param>
        /// <returns></returns>
        private string GetMeasurementUnit(XmlNode currentNode)
        {
            string strUnit = string.Empty;
            string strRefColName = string.Empty;
            if ((currentNode != null) && (currentNode.Attributes["referencecolumn"] != null) && (!string.IsNullOrEmpty((strRefColName = currentNode.Attributes["referencecolumn"].Value))))
            {
                XmlNode objXmlNode = currentNode.ParentNode.SelectSingleNode("attribute[@name ='" + strRefColName + "']");
                if (objXmlNode != null)
                {
                    strUnit = objXmlNode.Attributes["value"].Value;
                }

            }
            return strUnit;
        }
        /// <summary>
        /// Gets the feet meter label.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        private string GetFeetMeterLabel(string unit)
        {
            string strLabel = string.Empty;
            if (unit.ToLowerInvariant().Equals("metres"))
            {
                strLabel = " (m) ";
            }
            else
            {
                strLabel = " (ft) ";
            }
            return strLabel;
        }
        /// <summary>
        /// Converts the feet metre.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="currentUnit">The current unit.</param>
        /// <returns></returns>
        private double ConvertFeetMetre(double value, string currentUnit)
        {
            double dblConvertedValue = 0.00;
            if (strUserSelectedUnit.ToLowerInvariant().Equals(currentUnit.ToLowerInvariant()))
            {
                dblConvertedValue = value;
            }
            else if (strUserSelectedUnit.ToLowerInvariant().Equals("metres"))
            {
                dblConvertedValue = (value / FEETMETERCONSTANT);
            }
            else if (strUserSelectedUnit.ToLowerInvariant().Equals("feet"))
            {
                dblConvertedValue = (value * FEETMETERCONSTANT);
            }
            return dblConvertedValue;
        }

        /// <summary>
        /// Converts the feet metre.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="currentUnit">The current unit.</param>
        /// <returns></returns>
        private double ConvertFeetMetre(double value, string currentUnit, string strUserSelectedUnit)
        {
            double dblConvertedValue = 0.00;
            if (strUserSelectedUnit.ToLowerInvariant().Equals(currentUnit.ToLowerInvariant()))
            {
                dblConvertedValue = value;
            }
            else if (strUserSelectedUnit.ToLowerInvariant().Equals("metres"))
            {
                dblConvertedValue = (value / FEETMETERCONSTANT);
            }
            else if (strUserSelectedUnit.ToLowerInvariant().Equals("feet"))
            {
                dblConvertedValue = (value * FEETMETERCONSTANT);
            }
            return dblConvertedValue;
        }

        /// <summary>
        /// Gets the user selected unit.
        /// </summary>
        /// <returns></returns>
        private string GetUserSelectedUnit()
        {
            string strUnit = string.Empty;
            UserPreferences objUserPreferences = new UserPreferences();
            objMossController = objFactory.GetServiceManager("MossService");
            string strUserId = objCommonUtility.GetUserName();
            //get the user prefrences.
            objUserPreferences = ((MOSSServiceManager)objMossController).GetUserPreferencesValue(strUserId, strCurrSiteUrl);
            if(!string.IsNullOrEmpty(strUnit = objCommonUtility.GetFormControlValue(FEETMETERRADIOBUTTONID)))
            {
                if (strUnit.ToLower().Equals("rbfeet"))
                {
                    strUnit = "feet";
                }
                else
                {
                    strUnit = "metres";
                }
            }
            else if (objUserPreferences != null)
            {
                strUnit = objUserPreferences.DepthUnits.ToLowerInvariant();
            }
            return strUnit;
        }

        /// <summary>
        /// Gets the user selected unit.
        /// </summary>
        /// <returns></returns>
        private string GetUserSelectedUnitForChart()
        {
            string strUnit = string.Empty;
            UserPreferences objUserPreferences = new UserPreferences();
            objMossController = objFactory.GetServiceManager("MossService");
            string strUserId = objCommonUtility.GetUserName();
            //get the user prefrences.
            objUserPreferences = ((MOSSServiceManager)objMossController).GetUserPreferencesValue(strUserId, strCurrSiteUrl);
            if(!string.IsNullOrEmpty(strUnit = objCommonUtility.GetFormControlValue(FEETMETERRADIOBUTTONID)))
            {
                if (strUnit.ToLower().Equals("feet"))
                {
                    strUnit = "feet";
                }
                else
                {
                    strUnit = "metres";
                }
            }
            else if (objUserPreferences != null)
            {
                strUnit = objUserPreferences.DepthUnits.ToLowerInvariant();
            }
            return strUnit;
        }
        #endregion

        #region Export Methods
        /// <summary>
        /// Exports the EDM report results with tab to excel.
        /// </summary>
        /// <param name="responseXML">The response XML.</param>
        private void ExportEDMResultsToExcel(XmlDocument responseXML)
        {
            string strXpath = "attribute[@display!='false']";
            IWorkbook objWorkbook = Factory.GetWorkbook();
            string[] arrworksheetName = { "Events", "Daily Summary", "Reported Activities" };
            if (responseXML != null)
            {
                XmlNodeList xmlNodeListTabs = responseXML.SelectNodes("response/report/record[not(attribute[@name='Level']/@value=preceding-sibling::record/attribute[@name='Level']/@value)]/attribute[@name='Level']/@value");
                #region Looping all tabs
                for (int intTabCounter = 0; intTabCounter < xmlNodeListTabs.Count; intTabCounter++)
                {
                    XmlNodeList objNodeList = responseXML.SelectNodes("response/report/record[(attribute/@name='Level') and (attribute/@value='" + xmlNodeListTabs[intTabCounter].Value + "')]");
                    if (intTabCounter != 0)
                    {
                        objWorkbook.Worksheets.Add();
                    }
                    IWorksheet objWorksheet = objWorkbook.Worksheets[intTabCounter];
                    IRange objCells = objWorksheet.Cells;
                    objWorksheet.Name = arrworksheetName[intTabCounter];
                    #region Looping all rows
                    for (int intRowCounter = 0; intRowCounter < objNodeList.Count; intRowCounter++)
                    {
                        XmlNodeList xmlNodeListColumns = objNodeList[intRowCounter].SelectNodes(strXpath);
                        #region Looping All Columns
                        //writes all the values to the excel.
                        for (int intColumnCounter = 0; intColumnCounter < xmlNodeListColumns.Count; intColumnCounter++)
                        {

                            if (intRowCounter == 0)
                            {
                                objCells[intRowCounter, intColumnCounter].Font.Bold = true;
                                objCells[intRowCounter, intColumnCounter].Interior.ColorIndex = 15;
                                objCells[intRowCounter, intColumnCounter].Columns.AutoFit();
                                if (!string.IsNullOrEmpty(xmlNodeListColumns[intColumnCounter].Attributes["referencecolumn"].Value))
                                {
                                    objCells[intRowCounter, intColumnCounter].Value = xmlNodeListColumns[intColumnCounter].Attributes["name"].Value;
                                }
                                else
                                {
                                    objCells[intRowCounter, intColumnCounter].Value = xmlNodeListColumns[intColumnCounter].Attributes["name"].Value;
                                }
                            }

                            XmlAttribute objXmlAttribute = xmlNodeListColumns[intColumnCounter].Attributes["type"];

                            if ((objXmlAttribute != null) && (objXmlAttribute.Value.ToLowerInvariant().Equals("number")))
                            {
                                objCells[intRowCounter + 1, intColumnCounter].NumberFormat = "0.00";
                                objCells[intRowCounter + 1, intColumnCounter].HorizontalAlignment = HAlign.Right;
                                if (!string.IsNullOrEmpty(xmlNodeListColumns[intColumnCounter].Attributes["referencecolumn"].Value))
                                {
                                    objCells[intRowCounter + 1, intColumnCounter].Value = ConvertFeetMetre(double.Parse(xmlNodeListColumns[intColumnCounter].Attributes["value"].Value), GetMeasurementUnit(xmlNodeListColumns[intColumnCounter]));
                                }
                                else
                                {
                                    objCells[intRowCounter + 1, intColumnCounter].Value = xmlNodeListColumns[intColumnCounter].Attributes["value"].Value;
                                }

                            }
                            else if ((objXmlAttribute != null) && (objXmlAttribute.Value.ToLowerInvariant().Equals("date")))
                            {

                                objCells[intRowCounter + 1, intColumnCounter].Value = xmlNodeListColumns[intColumnCounter].Attributes["value"].Value;
                                objCells[intRowCounter + 1, intColumnCounter].NumberFormat = PortalConfiguration.GetInstance().GetKey(DATEFORMAT).ToString();

                                objCells[intRowCounter + 1, intColumnCounter].HorizontalAlignment = HAlign.Left;
                            }
                            else
                            {
                                objCells[intRowCounter + 1, intColumnCounter].NumberFormat = "@";
                                objCells[intRowCounter + 1, intColumnCounter].HorizontalAlignment = HAlign.Left;
                                objCells[intRowCounter + 1, intColumnCounter].Value = xmlNodeListColumns[intColumnCounter].Attributes["value"].Value;

                            }
                            objCells[intRowCounter + 1, intColumnCounter].Columns.AutoFit();

                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
                objWorkbook.Worksheets[0].Select();
                RenderXLSheet(objWorkbook);
            }
        }

        /// <summary>
        /// Exports the table with tabs to excel.
        /// </summary>
        /// <param name="responseXML">The response XML.</param>
        /// <param name="tabXpath">The tab xpath.</param>
        /// <param name="rowXpath">The row xpath.</param>
        /// <param name="columnXpath">The column xpath.</param>
        /// <param name="worksheetNames">The worksheet names.</param>
        private void ExportTableWithTabsToExcel(XmlDocument responseXML, string[] tabXpath, string rowXpath, string[] columnXpath, string[] worksheetNames)
        {
            string[] strXpath = columnXpath;
            IWorkbook objWorkbook = Factory.GetWorkbook();
            string[] arrworksheetName = worksheetNames;
            string strRowXpath = rowXpath;
            string strDepthUnitLabel = GetFeetMeterLabel(strUserSelectedUnit);
            if (responseXML != null)
            {
                #region Looping all tabs
                for (int intTabCounter = 0; intTabCounter < tabXpath.Length; intTabCounter++)
                {
                    XmlNodeList objNodeList = responseXML.SelectNodes(strRowXpath.Replace("%tabfilter%", tabXpath[intTabCounter]));
                    if (intTabCounter != 0)
                    {
                        objWorkbook.Worksheets.Add();
                    }
                    IWorksheet objWorksheet = objWorkbook.Worksheets[intTabCounter];
                    IRange objCells = objWorksheet.Cells;
                    objWorksheet.Name = arrworksheetName[intTabCounter];
                    #region Looping all rows
                    for (int intRowCounter = 0; intRowCounter < objNodeList.Count; intRowCounter++)
                    {
                        XmlNodeList xmlNodeListColumn = objNodeList[intRowCounter].SelectNodes(strXpath[intTabCounter]);
                        #region Looping All Column
                        //writes all the values to the excel.
                        for (int intColumnCounter = 0; intColumnCounter < xmlNodeListColumn.Count; intColumnCounter++)
                        {

                            if (intRowCounter == 0)
                            {
                                objCells[intRowCounter, intColumnCounter].Font.Bold = true;
                                objCells[intRowCounter, intColumnCounter].Interior.ColorIndex = 15;
                                objCells[intRowCounter, intColumnCounter].Columns.AutoFit();
                                if (!string.IsNullOrEmpty(xmlNodeListColumn[intColumnCounter].Attributes["referencecolumn"].Value))
                                {
                                    objCells[intRowCounter, intColumnCounter].Value = xmlNodeListColumn[intColumnCounter].Attributes["name"].Value + strDepthUnitLabel;
                                }
                                else
                                {
                                    objCells[intRowCounter, intColumnCounter].Value = xmlNodeListColumn[intColumnCounter].Attributes["name"].Value;
                                }
                            }

                            XmlAttribute objXmlAttribute = xmlNodeListColumn[intColumnCounter].Attributes["type"];

                            if ((objXmlAttribute != null) && (objXmlAttribute.Value.ToLowerInvariant().Equals("number")))
                            {
                                objCells[intRowCounter + 1, intColumnCounter].NumberFormat = "0.00";
                                objCells[intRowCounter + 1, intColumnCounter].HorizontalAlignment = HAlign.Right;
                                if (!string.IsNullOrEmpty(xmlNodeListColumn[intColumnCounter].Attributes["referencecolumn"].Value))
                                {
                                    objCells[intRowCounter + 1, intColumnCounter].Value = ConvertFeetMetre(double.Parse(xmlNodeListColumn[intColumnCounter].Attributes["value"].Value), GetMeasurementUnit(xmlNodeListColumn[intColumnCounter]));
                                }
                                else
                                {
                                    objCells[intRowCounter + 1, intColumnCounter].Value = xmlNodeListColumn[intColumnCounter].Attributes["value"].Value;
                                }

                            }
                            else if ((objXmlAttribute != null) && (objXmlAttribute.Value.ToLowerInvariant().Equals("date")))
                            {

                                objCells[intRowCounter + 1, intColumnCounter].Value = xmlNodeListColumn[intColumnCounter].Attributes["value"].Value;
                                objCells[intRowCounter + 1, intColumnCounter].NumberFormat = PortalConfiguration.GetInstance().GetKey(DATEFORMAT).ToString();

                                objCells[intRowCounter + 1, intColumnCounter].HorizontalAlignment = HAlign.Left;
                            }
                            else
                            {
                                objCells[intRowCounter + 1, intColumnCounter].NumberFormat = "@";
                                objCells[intRowCounter + 1, intColumnCounter].HorizontalAlignment = HAlign.Left;
                                objCells[intRowCounter + 1, intColumnCounter].Value = xmlNodeListColumn[intColumnCounter].Attributes["value"].Value;

                            }
                            objCells[intRowCounter + 1, intColumnCounter].Columns.AutoFit();
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
                objWorkbook.Worksheets[0].Select();
                RenderXLSheet(objWorkbook);
            }
        }
        /// <summary>
        /// Exports the XML results TO excel.
        /// </summary>
        /// <param name="responseXML">The response XML.</param>
        private void ExportXMLResultsTOExcel(XmlDocument responseXML)
        {
            // Create a new workbook.             
            IWorkbook objWorkbook = Factory.GetWorkbook();
            IWorksheet objWorksheet = objWorkbook.Worksheets["Sheet1"];
            IRange objCells = objWorksheet.Cells;
            // Set the worksheet name.
            objWorksheet.Name = "SearchResults";
            if (responseXML != null)
            {
                XmlNodeList xmlNodeListRows = responseXML.SelectNodes("response/report/record");
                AddWorkSheet(xmlNodeListRows, objCells);
                RenderXLSheet(objWorkbook);
            }
        }
        /// <summary>
        /// Exports the UBI to WRK sheet.
        /// </summary>
        /// <param name="responseXML">The response XML.</param>
        private void ExportUBIToWrkSheet(XmlDocument responseXML)
        {
            // Create a new workbook.             
            IWorkbook objWorkbook = Factory.GetWorkbook();
            string strIdentifier = string.Empty;
            if (responseXML != null)
            {
                if (responseXML.SelectSingleNode("response/report/record/attribute[@name='UWI']") != null)
                {
                    strIdentifier = "UWI";
                }
                else if (responseXML.SelectSingleNode("response/report/record/attribute[@name='UWBI']") != null)
                {
                    strIdentifier = "UWBI";
                }
                else
                {
                    strIdentifier = "Unique Wellbore Identifier";
                }
                XmlNodeList xmlNodeListUWIs = responseXML.SelectNodes("response/report/record[not(attribute[@name='" + strIdentifier + "']/@value=preceding-sibling::record/attribute[@name='" + strIdentifier + "']/@value)]/attribute[@name='" + strIdentifier + "']/@value");
                for (int intUWICounter = 0; intUWICounter < xmlNodeListUWIs.Count; intUWICounter++)
                {
                    string strUWI = xmlNodeListUWIs[intUWICounter].Value;
                    XmlNodeList objNodeList = responseXML.SelectNodes("response/report/record[attribute[@name='" + strIdentifier + "']/@value='" + strUWI + "']");
                    if (intUWICounter != 0)
                    {
                        objWorkbook.Worksheets.Add();
                    }
                    IWorksheet objWorksheet = objWorkbook.Worksheets[intUWICounter];
                    IRange objCells = objWorksheet.Cells;
                    // Set the worksheet name.
                    objWorksheet.Name = strUWI;
                    AddWorkSheet(objNodeList, objCells);
                }
                objWorkbook.Worksheets[0].Select();
                RenderXLSheet(objWorkbook);
            }
        }

        /// <summary>
        /// Export list of rows(nodelist) to a worksheet.
        /// </summary>
        /// <param name="recordNodeList">The record node list.</param>
        /// <param name="cells">The cells.</param>
        private void AddWorkSheet(XmlNodeList recordNodeList, IRange cells)
        {
            strColIdentifierItem = strColIdentifierItem.Replace("\r\n", string.Empty);
            string[] arrColNames = strColIdentifierItem.Split("|".ToCharArray());
            string strDepthUnitLabel = GetFeetMeterLabel(strUserSelectedUnit);
            #region Exporting Results
            if (recordNodeList != null)
            {
                for (int intRowCounter = 0; intRowCounter < recordNodeList.Count; intRowCounter++)
                {
                    if (arrColNames != null)
                    {
                        for (int intColumnCounter = 0; intColumnCounter < arrColNames.Length; intColumnCounter++)
                        {
                            if (!string.IsNullOrEmpty(arrColNames[intColumnCounter].Trim()))
                            {
                                XmlNode xmlNodeColumn = recordNodeList[intRowCounter].SelectSingleNode("attribute[@name = '" + arrColNames[intColumnCounter].Trim() + "']");

                                if (xmlNodeColumn == null)
                                {
                                    continue;
                                }
                                if (intRowCounter == 0)
                                {
                                    cells[intRowCounter, intColumnCounter].Font.Bold = true;
                                    cells[intRowCounter, intColumnCounter].Interior.ColorIndex = 15;
                                    cells[intRowCounter, intColumnCounter].Columns.AutoFit();
                                    if (!string.IsNullOrEmpty(xmlNodeColumn.Attributes["referencecolumn"].Value))
                                    {
                                        cells[intRowCounter, intColumnCounter].Value = xmlNodeColumn.Attributes["name"].Value + strDepthUnitLabel;
                                    }
                                    else
                                    {
                                        cells[intRowCounter, intColumnCounter].Value = xmlNodeColumn.Attributes["name"].Value;
                                    }
                                }

                                XmlAttribute objXmlAttribute = xmlNodeColumn.Attributes["type"];

                                if ((objXmlAttribute != null) && (objXmlAttribute.Value.ToLowerInvariant().Equals("number")))
                                {
                                    if (!xmlNodeColumn.Attributes["name"].Value.ToLowerInvariant().Equals("quality"))//for quality column to be displayed as whole number
                                    {
                                        cells[intRowCounter + 1, intColumnCounter].NumberFormat = "0.00";
                                    }

                                    cells[intRowCounter + 1, intColumnCounter].HorizontalAlignment = HAlign.Right;
                                    if (!string.IsNullOrEmpty(xmlNodeColumn.Attributes["referencecolumn"].Value))
                                    {
                                        double unitValue = 0.00;
                                        if (double.TryParse(xmlNodeColumn.Attributes["value"].Value, out unitValue))
                                        {
                                            cells[intRowCounter + 1, intColumnCounter].Value = ConvertFeetMetre(unitValue, GetMeasurementUnit(xmlNodeColumn));
                                        }
                                        else
                                        {
                                            cells[intRowCounter + 1, intColumnCounter].Value = xmlNodeColumn.Attributes["value"].Value;
                                        }

                                    }
                                    else
                                    {
                                        cells[intRowCounter + 1, intColumnCounter].Value = xmlNodeColumn.Attributes["value"].Value;
                                    }

                                }
                                else if ((objXmlAttribute != null) && (objXmlAttribute.Value.ToLowerInvariant().Equals("date")))
                                {

                                    cells[intRowCounter + 1, intColumnCounter].Value = xmlNodeColumn.Attributes["value"].Value;
                                    cells[intRowCounter + 1, intColumnCounter].NumberFormat = PortalConfiguration.GetInstance().GetKey(DATEFORMAT).ToString();
                                    cells[intRowCounter + 1, intColumnCounter].HorizontalAlignment = HAlign.Left;
                                }
                                else
                                {
                                    cells[intRowCounter + 1, intColumnCounter].NumberFormat = "@";
                                    cells[intRowCounter + 1, intColumnCounter].HorizontalAlignment = HAlign.Left;
                                    cells[intRowCounter + 1, intColumnCounter].Value = xmlNodeColumn.Attributes["value"].Value;

                                }
                                cells[intRowCounter + 1, intColumnCounter].Columns.AutoFit();
                            }
                        }
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// Exports the paleo markers report.
        /// </summary>
        /// <param name="responseXML">The response XML.</param>
        private void ExportPaleoMarkersReport(XmlDocument responseXML)
        {
            strColIdentifierItem = strColIdentifierItem.Replace("\r\n", string.Empty);
            string[] strColNamePerTab = strColIdentifierItem.Split(new string[] { "TAB" }, StringSplitOptions.RemoveEmptyEntries);
            string[] arrColXpath = new string[strColNamePerTab.Length];
            for (int intTabCounter = 0; intTabCounter < strColNamePerTab.Length; intTabCounter++)
            {
                arrColXpath[intTabCounter] = "attribute[(@display!='false') and (" + CreateColumnQuery(strColNamePerTab[intTabCounter].Split("|".ToCharArray()), "=", "or") + ")]";
            }
            string strColXpath = "attribute[(@display!='false') and (" + CreateColumnQuery(strColIdentifierItem.Split("|".ToCharArray()), "=", "or") + ")]";
            string[] strTabXpath = { "PAL", "PAM" };
            string strRowXpath = "response/report/record[(attribute/@name='Pick Interpreter') and (attribute/@value='%tabfilter%')]";
            string[] arrTabName ={ "CDS Paleo with Ages", "MMS Paleo with Ages" };
            ExportTableWithTabsToExcel(responseXML, strTabXpath, strRowXpath, arrColXpath, arrTabName);
        }

        /// <summary>
        /// Exports the paleo markers report.
        /// </summary>
        /// <param name="responseXML">The response XML.</param>
        private void ExportPressureSurveyReport(XmlDocument responseXML)
        {
            strColIdentifierItem = strColIdentifierItem.Replace("\r\n", string.Empty);
            string[] strColNamePerTab = strColIdentifierItem.Split(new string[] { "TAB" }, StringSplitOptions.RemoveEmptyEntries);
            string[] arrColXpath = new string[strColNamePerTab.Length];
            for (int intTabCounter = 0; intTabCounter < strColNamePerTab.Length; intTabCounter++)
            {
                arrColXpath[intTabCounter] = "attribute[(@display!='false') and (" + CreateColumnQuery(strColNamePerTab[intTabCounter].Split("|".ToCharArray()), "=", "or") + ")]";
            }
            string strColXpath = "attribute[(@display!='false') and (" + CreateColumnQuery(strColIdentifierItem.Split("|".ToCharArray()), "=", "or") + ")]";
            string[] strTabXpath = { "PressureSurvey", "Reservoir" };

            string strRowXpath = "response/report[@name='%tabfilter%']/record";

            string[] arrTabName ={ "Pressure Survey Data", "Reservoir Data" };
            ExportTableWithTabsToExcel(responseXML, strTabXpath, strRowXpath, arrColXpath, arrTabName);
        }

        /// <summary>
        /// Exports the chart to excel.
        /// </summary>
        /// <param name="responseXML">The response XML.</param>
        /// <param name="reportType">Type of the report.</param>
        private void ExportChartDataToExcel(XmlDocument responseXML, string reportType)
        {

            IWorkbook objWorkbook = Factory.GetWorkbook();
            IWorksheet objWorksheet = objWorkbook.Worksheets["Sheet1"];

            if (responseXML != null)
            {
                XmlNodeList xmlNodeListRows = responseXML.SelectNodes("response/report/record");
                IRange objCells = objWorksheet.Cells;
                objWorksheet.Name = "SearchResults";

                AddChartWorkSheet(xmlNodeListRows, objCells);

                objWorkbook.Worksheets[0].Select();
                InsertChartToExcel(reportType, objWorksheet);
                /// Render the chart
                RenderXLSheet(objWorkbook);
            }
        }

        /// <summary>
        /// Inserts the chart to excel.
        /// </summary>
        /// <param name="reportType">Type of the report.</param>
        /// <param name="objWorksheet">The obj worksheet.</param>
        private static void InsertChartToExcel(string reportType, IWorksheet objWorksheet)
        {
            /*  // Add a chart to the worksheet's shape collection.
              // NOTE: Calculate the coordinates of the chart by converting row
              //       and column coordinates to points.  Use fractional row 
              //       and colum values to get coordinates anywhere in between 
              //       row and column boundaries.
              SpreadsheetGear.IWorksheetWindowInfo windowInfo = objWorksheet.WindowInfo;

              double left = windowInfo.ColumnToPoints(2.0);
              double top = windowInfo.RowToPoints(1.0);
              double right = windowInfo.ColumnToPoints(9.0);
              double bottom = windowInfo.RowToPoints(16.0);
           
              SpreadsheetGear.Charts.IChart chart = objWorksheet.Shapes.AddChart(left, top, right - left, bottom - top).Chart; */
            SpreadsheetGear.Charts.IChart chart = objWorksheet.Shapes.AddChart(500, 50, 500, 500).Chart;
            /// Holds the XAxis Range
            string strXAxisRange = string.Empty;
            /// Holds the YAxis Range
            string strRange = string.Empty;
            string strXAxisTitle = string.Empty;
            string strYAxisTitle = string.Empty;
            Dictionary<string, string> dicChartSeries = new Dictionary<string, string>();
            /// Choose XAxis Data and Range
            /// Choose YAxis Data and Range
            switch (reportType.ToLowerInvariant())
            {
                case DIRECTIONALSURVEYCHARTRESULTS:
                    {
                        for (int intColumnIndex = 0; intColumnIndex < objWorksheet.Cells.CurrentRegion.ColumnCount; intColumnIndex++)
                        {
                            strRange = string.Empty;
                            /// Choose XAxis Data and Range
                            if (objWorksheet.Cells.CurrentRegion.Columns[0, intColumnIndex].Entry.ToLowerInvariant().Contains("depth"))
                            {
                                strXAxisRange = objWorksheet.Cells.CurrentRegion.Columns[1, intColumnIndex].Address;
                                strXAxisRange = string.Format("{0}:{1}", strXAxisRange, objWorksheet.Cells.CurrentRegion.Columns[0, intColumnIndex].EndDown.Address);
                                strXAxisTitle = objWorksheet.Cells.CurrentRegion.Columns[0, intColumnIndex].Entry;
                            }
                            /// Choose YAxis Data and Range
                            if (objWorksheet.Cells.CurrentRegion.Columns[0, intColumnIndex].Entry.ToLowerInvariant().Contains("azimuth") || objWorksheet.Cells.CurrentRegion.Columns[0, intColumnIndex].Entry.ToLowerInvariant().Contains("inclination"))
                            {
                                strRange = objWorksheet.Cells.CurrentRegion.Columns[1, intColumnIndex].Address;
                                strRange = string.Format("{0}:{1}", strRange, objWorksheet.Cells.CurrentRegion.Columns[0, intColumnIndex].EndDown.Address);
                                dicChartSeries.Add(objWorksheet.Cells.CurrentRegion.Columns[0, intColumnIndex].Entry.ToLowerInvariant(), strRange);
                                strYAxisTitle = "Degrees";// objWorksheet.Cells.CurrentRegion.Columns[0, intColumnIndex].Entry;
                            }
                        }
                        break;
                    }
                case PICKSCHARTRESULTS:
                    {
                        for (int intColumnIndex = 0; intColumnIndex < objWorksheet.Cells.CurrentRegion.ColumnCount; intColumnIndex++)
                        {
                            strRange = string.Empty;
                            /// Choose XAxis Data and Range
                            if (objWorksheet.Cells.CurrentRegion.Columns[0, intColumnIndex].Entry.ToLowerInvariant().Contains("pick name"))
                            {
                                strXAxisRange = objWorksheet.Cells.CurrentRegion.Columns[1, intColumnIndex].Address;
                                strXAxisRange = string.Format("{0}:{1}", strXAxisRange, objWorksheet.Cells.CurrentRegion.Columns[0, intColumnIndex].EndDown.Address);
                                strXAxisTitle = objWorksheet.Cells.CurrentRegion.Columns[0, intColumnIndex].Entry;
                            }
                            /// Choose YAxis Data and Range
                            if (objWorksheet.Cells.CurrentRegion.Columns[0, intColumnIndex].Entry.ToLowerInvariant().Contains("ahbdf ah"))
                            {
                                strRange = objWorksheet.Cells.CurrentRegion.Columns[1, intColumnIndex].Address;
                                strRange = string.Format("{0}:{1}", strRange, objWorksheet.Cells.CurrentRegion.Columns[0, intColumnIndex].EndDown.Address);
                                dicChartSeries.Add(objWorksheet.Cells.CurrentRegion.Columns[0, intColumnIndex].Entry.ToLowerInvariant(), strRange);
                                strYAxisTitle = string.Format("Depth {0}", objWorksheet.Cells.CurrentRegion.Columns[0, intColumnIndex].Entry.Substring(objWorksheet.Cells.CurrentRegion.Columns[0, intColumnIndex].Entry.IndexOf("("))); ;
                            }

                        }
                        break;
                    }
            }
            /// Add Chart Series and assign XAxis and YAxis
            SpreadsheetGear.Charts.ISeries chartSeries;
            foreach (string strSeriesName in dicChartSeries.Keys)
            {
                chartSeries = chart.SeriesCollection.Add();
                chartSeries.MarkerSize = 10;
                chartSeries.MarkerStyle = SpreadsheetGear.Charts.MarkerStyle.Diamond;
                chartSeries.Values = objWorksheet.Cells[dicChartSeries[strSeriesName]];

                chartSeries.XValues = objWorksheet.Cells[strXAxisRange];
                /// Sets the Legend Name and Chart Title
                switch (reportType.ToLowerInvariant())
                {
                    case DIRECTIONALSURVEYCHARTRESULTS:
                        {
                            chartSeries.Name = strSeriesName;
                            break;
                        }
                    case PICKSCHARTRESULTS:
                        {
                            chartSeries.Name = strYAxisTitle;
                            break;
                        }
                }

                if (strSeriesName.Equals("inclination"))
                {
                    chartSeries.AxisGroup = SpreadsheetGear.Charts.AxisGroup.Primary;
                }
            }
            /// Set the chart type.
            chart.ChartType = SpreadsheetGear.Charts.ChartType.Line;

            /// Sets the X-Axis Title
            if (chart.Axes[SpreadsheetGear.Charts.AxisType.Category] != null)
            {
                chart.Axes[SpreadsheetGear.Charts.AxisType.Category].HasTitle = true;
                chart.Axes[SpreadsheetGear.Charts.AxisType.Category].AxisTitle.Text = strXAxisTitle;
                chart.Axes[SpreadsheetGear.Charts.AxisType.Category].AxisTitle.Font.Bold = false;
            }
            /// Sets the Y-Axis Title
            if (chart.Axes[SpreadsheetGear.Charts.AxisType.Value] != null)
            {
                chart.Axes[SpreadsheetGear.Charts.AxisType.Value].HasTitle = true;
                chart.Axes[SpreadsheetGear.Charts.AxisType.Value].AxisTitle.Text = strYAxisTitle;
                chart.Axes[SpreadsheetGear.Charts.AxisType.Value].AxisTitle.Font.Bold = false;
            }

        }

        /// <summary>
        /// Adds the chart work sheet.
        /// </summary>
        /// <param name="recordNodeList">The record node list.</param>
        /// <param name="cells">The cells.</param>
        private void AddChartWorkSheet(XmlNodeList recordNodeList, IRange cells)
        {
            string strXpath = "attribute[@display!='false']";
            double unitValue = 0.00;

            if (recordNodeList != null)
            {
                for (int intRowCounter = 0; intRowCounter < recordNodeList.Count; intRowCounter++)
                {
                    XmlNodeList xmlNodeListColumns = recordNodeList[intRowCounter].SelectNodes(strXpath);
                    #region Looping for All Column
                    /// Writes all the values to the excel.
                    for (int intColumnCounter = 0; intColumnCounter < xmlNodeListColumns.Count; intColumnCounter++)
                    {
                        if (!string.IsNullOrEmpty(xmlNodeListColumns[intColumnCounter].Attributes["name"].Value))
                        {
                            XmlNode xmlNodeColumn = recordNodeList[intRowCounter].SelectSingleNode("attribute[@name = '" + xmlNodeListColumns[intColumnCounter].Attributes["name"].Value + "']");

                            if (xmlNodeColumn == null)
                            {
                                continue;
                            }
                            if (intRowCounter == 0)
                            {
                                cells[intRowCounter, intColumnCounter].Font.Bold = true;
                                cells[intRowCounter, intColumnCounter].Interior.ColorIndex = 15;
                                cells[intRowCounter, intColumnCounter].Columns.AutoFit();
                                if (!string.IsNullOrEmpty(xmlNodeColumn.Attributes["referencecolumn"].Value))
                                {
                                    cells[intRowCounter, intColumnCounter].Value = xmlNodeColumn.Attributes["name"].Value + GetFeetMeterLabel(GetUserSelectedUnitForChart());
                                }
                                else
                                {
                                    cells[intRowCounter, intColumnCounter].Value = xmlNodeColumn.Attributes["name"].Value;
                                }
                            }

                            XmlAttribute objXmlAttribute = xmlNodeColumn.Attributes["type"];

                            if ((objXmlAttribute != null) && (objXmlAttribute.Value.ToLowerInvariant().Equals("number")))
                            {
                                if (!xmlNodeColumn.Attributes["name"].Value.ToLowerInvariant().Equals("quality"))/// for quality column to be displayed as whole number
                                {
                                    cells[intRowCounter + 1, intColumnCounter].NumberFormat = "0.00";
                                }

                                cells[intRowCounter + 1, intColumnCounter].HorizontalAlignment = HAlign.Right;
                                if (!string.IsNullOrEmpty(xmlNodeColumn.Attributes["referencecolumn"].Value))
                                {

                                    if (double.TryParse(xmlNodeColumn.Attributes["value"].Value, out unitValue))
                                    {
                                        cells[intRowCounter + 1, intColumnCounter].Value = ConvertFeetMetre(unitValue, GetMeasurementUnit(xmlNodeColumn), GetUserSelectedUnitForChart());
                                    }
                                    else
                                    {
                                        cells[intRowCounter + 1, intColumnCounter].Value = xmlNodeColumn.Attributes["value"].Value;
                                    }

                                }
                                else
                                {
                                    cells[intRowCounter + 1, intColumnCounter].Value = xmlNodeColumn.Attributes["value"].Value;
                                }

                            }
                            else if ((objXmlAttribute != null) && (objXmlAttribute.Value.ToLowerInvariant().Equals("date")))
                            {

                                cells[intRowCounter + 1, intColumnCounter].Value = xmlNodeColumn.Attributes["value"].Value;
                                cells[intRowCounter + 1, intColumnCounter].NumberFormat = PortalConfiguration.GetInstance().GetKey(DATEFORMAT).ToString();
                                cells[intRowCounter + 1, intColumnCounter].HorizontalAlignment = HAlign.Left;
                            }
                            else
                            {
                                cells[intRowCounter + 1, intColumnCounter].NumberFormat = "@";
                                cells[intRowCounter + 1, intColumnCounter].HorizontalAlignment = HAlign.Left;
                                cells[intRowCounter + 1, intColumnCounter].Value = xmlNodeColumn.Attributes["value"].Value;
                            }
                            cells[intRowCounter + 1, intColumnCounter].Columns.AutoFit();
                        }
                    }
                    #endregion
                }
            }

        }
        #endregion
        #endregion

        #region Export All For new paging
        /// <summary>
        /// Gets the response XML.
        /// </summary>
        /// 
        private XmlDocument GetResponseXML(string searchName)
        {
            XmlDocument xmlDocResponse = null;
            objRequestInfo = new RequestInfo();
            int intMaxRecords = Convert.ToInt32(PortalConfiguration.GetInstance().GetKey(MAXRECORDS));
            string strIdentifiedItem = string.Empty;
            base.ResponseType = TABULAR;
            base.EntityName = searchName;
            if (!string.IsNullOrEmpty(strSelectedRows))
            {
                /// Gets the Selected Identifier value from the results page.
                strIdentifiedItem = strSelectedRows;

                /// Gets the Selected Identifier Column name from the results page.
                arrIdentifierValue = strIdentifiedItem.Split('|');

                /// Creates the requestInfo object to fetch result from report service.
                objRequestInfo = SetBasicDataObjects(searchName, strSelectedCriteriaName, arrIdentifierValue, false, false, intMaxRecords);
            }
            else
            {
                objRequestInfo = null;
            }

            if (objRequestInfo != null)
            {
                if (string.Equals(searchName.ToLowerInvariant(), PALEOMARKERSREPORT))
                {
                    AddPaleoMarkersAttribute();
                }
                string strLengthType = string.Empty;
                if (!string.IsNullOrEmpty(strLengthType = objCommonUtility.GetFormControlValue("rdoLengthType")))
                {
                    if (strLengthType.Equals(TRUEVERTICAL))
                    {
                        objRequestInfo.Entity.TVDSS = true;
                    }
                    else
                    {
                        objRequestInfo.Entity.TVDSS = false;
                    }
                }
                //end
                /// Call for the GetSearchResults() method to fetch the search results from webservice.
                if (strSearchType.Equals("Query Search"))
                {
                    objQueryBuildController = objFactory.GetServiceManager(QUERYSERVICE);
                    xmlDocResponse = objQueryBuildController.GetSearchResults(objRequestInfo, intMaxRecords, searchName, null, 0);
                }
                else
                {
                    xmlDocResponse = objReportController.GetSearchResults(objRequestInfo, intMaxRecords, searchName, null, 0);
                }
            }
            return xmlDocResponse;
        }

        /// <summary>
        /// Gets the recall curve response XML.
        /// </summary>
        /// <param name="searchName">Name of the search.</param>
        /// <returns></returns>
        private XmlDocument GetRecallCurveResponseXml(string searchName)
        {
            XmlDocument xmlDocResponse = null;
            int intMaxRecords = Convert.ToInt32(PortalConfiguration.GetInstance().GetKey(MAXRECORDS));
            objRequestInfo = GetRecallCurveRequestInfo(searchName);
            if (objRequestInfo != null)
            {
                xmlDocResponse = objReportController.GetSearchResults(objRequestInfo, intMaxRecords, searchName, null, 0);
            }
            return xmlDocResponse;
        }
        #endregion
    }
}

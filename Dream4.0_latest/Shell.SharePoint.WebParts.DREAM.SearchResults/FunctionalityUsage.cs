#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : FunctionalityUsage.cs
#endregion
using System;
using System.Data;
using System.Xml;
using System.Xml.Xsl;
using System.Runtime.InteropServices;
using System.Web;
using System.Net;
using System.Web.Services.Protocols;
using System.Collections;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Controller;


namespace Shell.SharePoint.WebParts.DREAM.SearchResults
{
    /// <summary>
    /// This web part is used to diaplay functinality usgae report to admin.
    /// </summary>
    [Guid("97a1aa6c-e9fa-6789-8ac9-4bb161f8163a")]
    public class FunctionalityUsage : SearchResultsHelper
    {
        #region Declaration
        const string ENTITYNAME = "Functionality Usage";
        const string SIMPLE = "Simple";
        const string ADVANCE = "Advanced";
        const string SIMPLEREPORT = "simple";
        const string ADVANCEDREPORT = "advanced";
        const string STARTDATE = "Start Date";
        const string ENDDATE = "End Date";
        const string GTEQ = "GTEQ";
        const string LTEQ = "LTEQ";
        const string AND = "AND";
        const string XSLPARAM = "windowTitle";
        const string XSLPARAMVALUE = "Functionality Usage Report";
        const string ADVREPORTREQUESTXML = "ADVREPORTREQUESTXML";
        string strWindowTitleName = "Functionality Usage Report";
        #endregion

        #region Overridden Method

        /// <summary>
        /// Called by the ASP.NET page framework to initialize the page object.
        /// </summary>
        protected override void OnInit(EventArgs e)
        {
            objCommonUtility.RenderBusyBox(this.Page);
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            try
            {
                objReportController = objFactory.GetServiceManager(REPORTSERVICE);
                objResourceController = (ResourceServiceManager)objFactory.GetServiceManager(RESOURCEMANAGER);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
        }
        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            try
            {
                if (Page.Request.QueryString["type"] != null)
                {
                    if (Page.Request.QueryString["type"].ToLower() == SIMPLE.ToLower())
                    {
                        RenderReport(writer, SIMPLE);
                    }
                    else if (Page.Request.QueryString["type"].ToLower() == ADVANCE.ToLower())
                    {
                        RenderReport(writer, ADVANCE);
                    }
                    else
                    {
                        writer.Write("QueryString not found");
                    }

                }

            }
            catch (XmlException xmlEx)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), xmlEx);
                if (Page.Request.QueryString["type"].ToLower() == ADVANCE.ToLower())
                    writer.Write("<a href='/Pages/FunctionalityUsageFilter.aspx'>Go Back to Advanced Report</a>");
                RenderException(writer, xmlEx.Message.ToString(), strWindowTitleName, true);

            }
            catch (SoapException soapEx)
            {
                if (!string.Equals(soapEx.Message.ToString(), NORECORDSFOUND))
                {
                    CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), soapEx, 1);
                }
                if (Page.Request.QueryString["type"].ToLower() == ADVANCE.ToLower())
                    writer.Write("<a href='/Pages/FunctionalityUsageFilter.aspx'>Go Back to Advanced Report</a>");
                RenderException(writer, soapEx.Message.ToString(), strWindowTitleName, true);

            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), webEx, 1);
                if (Page.Request.QueryString["type"].ToLower() == ADVANCE.ToLower())
                    writer.Write("<a href='/Pages/FunctionalityUsageFilter.aspx'>Go Back to Advanced Report</a>");
                RenderException(writer, webEx.Message.ToString(), strWindowTitleName, true);

            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
            finally
            {
                objCommonUtility.CloseBusyBox(this.Page);
            }
        }
        #endregion

        #region Private Method

        /// <summary>
        /// Render Functionality usage report
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        private void RenderReport(HtmlTextWriter writer, string reportType)
        {
            xmlDocSearchResult = new XmlDocument();
            try
            {

                if (reportType.Equals(SIMPLE))
                {
                    objRequestInfo = GetSimpleRequestInfo();
                    xmlSearchRequest = objReportController.CreateSearchRequest(objRequestInfo);
                    //Calling webmethod
                    xmlDocSearchResult = objResourceController.GetFURReport(xmlSearchRequest, SIMPLEREPORT, -1, null, 0);
                    RenderTable(writer);
                    RenderPrintExport(writer);
                    writer.Write("<a href='/Pages/FunctionalityUsageFilter.aspx'>View Advanced Report</a></td></tr>");
                    writer.Write("<tr><td align=\"left\" width=\"100%\" style=\"font-weight:bold\">");
                    writer.Write("Report for the month of " + DateTime.Today.ToString("MMMM") + ".");

                }
                else
                {
                    xmlSearchRequest = GetAdvRequestXML();
                    //Calling webmethod
                    xmlDocSearchResult = objResourceController.GetFURReport(xmlSearchRequest, ADVANCEDREPORT, -1, null, 0);
                    RenderTable(writer);
                    RenderPrintExport(writer);
                    writer.Write("<a href='/Pages/FunctionalityUsageFilter.aspx'>Go Back to Advanced Report</a>");
                }



                writer.Write("</td></tr><tr><td width=\"100%\" style=\"height:10px\"></td></tr></table>");

                objMossController = objFactory.GetServiceManager("MossService");

                XmlTextReader objXmlTextReader = ((MOSSServiceManager)objMossController).GetXSLTemplate("FunctionalityUsage", SPContext.Current.Site.Url);

                DisplayReport(xmlDocSearchResult, objXmlTextReader);
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
        /// Get Advanced report request xml from session variable
        /// </summary>
        /// <returns>Requestxml object</returns>
        private XmlDocument GetAdvRequestXML()
        {
            XmlDocument objXmlDocument = new XmlDocument();
            try
            {
                objXmlDocument.LoadXml((string)CommonUtility.GetSessionVariable(Page, ADVREPORTREQUESTXML));
            }
            catch (Exception)
            {
                throw;
            }
            return objXmlDocument;
        }

        /// <summary>
        /// Creates RequestInfo object of simple funtionality usage report
        /// </summary>
        /// <returns>RequestInfo object</returns>
        private RequestInfo GetSimpleRequestInfo()
        {
            objRequestInfo = new RequestInfo();

            objRequestInfo.Entity = new Entity();

            try
            {
                objRequestInfo.Entity.Name = ENTITYNAME;

                objRequestInfo.Entity.Type = SIMPLE;

                ArrayList arlAttribute = new ArrayList();

                ArrayList arlAttributeGroup = new ArrayList();

                arlAttribute.Add(AddAttribute(STARTDATE, GTEQ, new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).ToShortDateString()));

                arlAttribute.Add(AddAttribute(ENDDATE, LTEQ, DateTime.Today.Date.ToShortDateString()));

                AttributeGroup objAttributeGroup = new AttributeGroup();

                objAttributeGroup.Operator = AND;

                objAttributeGroup.Attribute = arlAttribute;

                arlAttributeGroup.Add(objAttributeGroup);

                objRequestInfo.Entity.AttributeGroups = arlAttributeGroup;
            }
            catch (XmlException)
            {
                throw;
            }
            catch (Exception)
            {

                throw;
            }

            return objRequestInfo;
        }

        /// <summary>
        /// Create an attributes object and assign its properties
        /// </summary>
        /// <param name="name">Name of attribute</param>
        /// <param name="operation">Operation</param>
        /// <param name="value">value</param>
        /// <returns>attributes object</returns>
        private Attributes AddAttribute(string name, string operation, string value)
        {
            Attributes objAttribute = new Attributes();

            objAttribute.Value = new ArrayList();

            Value objValue = new Value();

            try
            {
                objAttribute.Name = name;

                objAttribute.Operator = operation;

                objValue.InnerText = value;

                objAttribute.Value.Add(objValue);

            }
            catch (Exception)
            {
                throw;
            }
            return objAttribute;
        }

        /// <summary>
        /// This method display Functionality usage response xml through xsl template
        /// </summary>
        /// <param name="responseXML">response xml</param>
        /// <param name="textReader">text reader of XSL template</param>
        /// <returns></returns>
        private void DisplayReport(XmlDocument responseXML, XmlTextReader textReader)
        {
            XslCompiledTransform xslTransform = null;

            XmlDocument objXmlDocForXSL = null;
            try
            {
                xslTransform = new XslCompiledTransform();

                objXmlDocForXSL = new XmlDocument();

                XsltArgumentList xsltArgumentList = new XsltArgumentList();

                xsltArgumentList.AddParam(XSLPARAM, string.Empty, XSLPARAMVALUE);

                //Inititlize the XSL
                objXmlDocForXSL.Load(textReader);

                xslTransform.Load(objXmlDocForXSL);

                xslTransform.Transform(responseXML, xsltArgumentList, HttpContext.Current.Response.Output);
            }
            catch (XmlException)
            {
                throw;
            }

            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Render print option on page
        /// </summary>
        /// <param name="writer">HtmlTextWriter</param>
        /// <returns></returns>
        new private void RenderPrintExport(HtmlTextWriter writer)
        {
            try
            {
                //initializing print link
                linkPrint.ID = "linkPrint";
                linkPrint.CssClass = "resultHyperLink";
                linkPrint.NavigateUrl = "javascript:printContent('tblSearchResults');";
                linkPrint.ImageUrl = "/_layouts/DREAM/images/icon_print.gif";
                linkPrint.Visible = true;
                //initializing export link
                linkExcel.ID = "linkExcel";
                linkExcel.CssClass = "resultHyperLink";
                linkExcel.NavigateUrl = "javascript:FURExportToExcel('tblSearchResults');";
                linkExcel.ImageUrl = "/_layouts/DREAM/images/icon_Excel.gif";
                linkExcel.Visible = true;

                writer.Write("Print&nbsp;");
                linkPrint.RenderControl(writer);
                writer.Write("&nbsp;");
                #region DREAM 4.0 Export Options

                writer.Write("&nbsp;");
                writer.Write("Export");
                writer.Write("&nbsp;");
                writer.Write("<input type=\"image\" class=\"buttonAdvSrch\" src=\"/_layouts/DREAM/images/icon_Excel.gif\"  id=\"btnShowExportOptionDiv\" onclick=\"SetExportOptionDefaults();return pop('divExportOptions')\" />");
                string strExportOptionsDiv = GetExportOptionsDivHTML(strWindowTitleName, true, false, true, false);
                writer.Write(strExportOptionsDiv);
                #endregion DREAM 4.0 Export Options
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
        /// Renders the table.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderTable(HtmlTextWriter writer)
        {
            writer.Write("<table cellspacing=\"0\" cellpadding=\"4\" border=\"0\" width=\"100%\"  class=\"tableAdvSrchBorder\" >");
            writer.Write("<tr><td width=\"100%\"  class=\"tdAdvSrchHeader\" valign=\"top\"><B>Functionality Usage Report</b></td></tr>");
            writer.Write("<tr><td align=\"right\" width=\"100%\" class=\"tdAdvSrchItem\">");
        }


        #endregion
    }

}

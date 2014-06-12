#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: ReportServiceManager.cs
#endregion

using System;
using System.Net;
using System.Xml;
using System.Web.Services.Protocols;
using Shell.SharePoint.DREAM.Business.Entities;


namespace Shell.SharePoint.DREAM.Controller
{
    /// <summary>
    /// This class will be used to handle the report 
    /// service request based on search type
    /// </summary>
    class ReportServiceManager :AbstractController
    {
        #region DECLARATION
        #region Constant
        const string XPATH = "/requestinfo/entity";
        #endregion

        #region Variables
        ReportServiceProxy objWebService;
        XmlNode[] objNodeParem;
        XmlNode[] objNode;
        XmlDocument xmlSearhRequestDoc;
        SearchRequestHandler objSearchRequestHandler;
        #endregion
        #endregion

        #region CONSTRUCTOR
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportServiceManager"/> class.
        /// </summary>
        public ReportServiceManager(string siteURL)
        {
            objWebService = new ReportServiceProxy(siteURL);
            objNodeParem = new XmlNode[1];
            xmlSearhRequestDoc = new XmlDocument();
            objSearchRequestHandler = new SearchRequestHandler();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportServiceManager"/> class.
        /// </summary>
        public ReportServiceManager()
        {
            objWebService = new ReportServiceProxy();
            objNodeParem = new XmlNode[1];
            xmlSearhRequestDoc = new XmlDocument();
            objSearchRequestHandler = new SearchRequestHandler();
            ;
        }
        #endregion

        #region CALLING WEBMETHODS
        /// <summary>
        /// Fetches the results.
        /// </summary>
        /// <param name="xmlSearchRequest">The XML search request.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns></returns>
        private XmlDocument FetchReportServiceResults(XmlDocument xmlSearchRequest,
                                        int maxRecords,
                                        string searchType,
                                        string sortColumn, int sortOrder)
        {
            #region Request XML Logger added in DREAM 4.0
            objSearchRequestHandler.XmlLogger(xmlSearchRequest, XMLTYPEREQUEST, XMLLOGDOCLIB);
            #endregion
            XmlDocument objXmlSearhResultsDoc = null;
            //***Code used when webservice is down or when we have to test reports with hardcoded xml*****.
            //***********start*******************
           // objXmlSearhResultsDoc = new XmlDocument();
           //objXmlSearhResultsDoc.Load(@"C:\Sonia\DREAM4.0\ResponseXml\Response.xml");
            //objXmlSearhResultsDoc.Load(@"C:\Sonia\DREAM4.0\Modules\WellSummary\XSL Test\WellsummaryResponse.xml");
            //if(objXmlSearhResultsDoc != null)
               // return objXmlSearhResultsDoc;
            //***********end*************
            if(sortColumn != null)
                if(sortColumn.ToString().Trim().Length == 0)
                    sortColumn = null;
            objNodeParem[0] = xmlSearchRequest.SelectSingleNode(XPATH);
            /// This switch case to get the search results from webservice based on search type.
            switch(searchType.ToUpper())
            {
                case "WELLSUMMARY":
                    objNode = (XmlNode[])objWebService.getWellSummary(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "WELL ADVANCE SEARCH":
                    objNode = (XmlNode[])objWebService.getWells(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "PREPRODRFT":
                    objNode = (XmlNode[])objWebService.getPreProdRFT(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "ADVANCED SEARCH":
                    objNode = (XmlNode[])objWebService.getWellbore(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "ASSET OWNER":
                    objNode = (XmlNode[])objWebService.getAssetOwner(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "WELLBORE ADVANCE SEARCH":
                    objNode = (XmlNode[])objWebService.getWellbore(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "BASIN":
                    objNode = (XmlNode[])objWebService.getBasins(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "BASINSEARCH":
                    objNode = (XmlNode[])objWebService.getBasins(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "BASIN ADVANCE SEARCH":
                    objNode = (XmlNode[])objWebService.getBasins(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "COUNTY":
                    objNode = (XmlNode[])objWebService.getCounty(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "DIRECTIONALSURVEY":
                    objNode = (XmlNode[])objWebService.getDirectionalSurvey(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "DIRECTIONALSURVEYDETAIL":
                    objNode = (XmlNode[])objWebService.getDirectionalSurveyArray(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "PVTREPORT":
                case "EPCATALOG":
                    objNode = (XmlNode[])objWebService.getDocuments(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "FIELD":
                    objNode = (XmlNode[])objWebService.getFields(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "FIELDS":
                    objNode = (XmlNode[])objWebService.getFields(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "FORMATION":
                    objNode = (XmlNode[])objWebService.getFormation(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "FIELD ADVANCE SEARCH":
                    objNode = (XmlNode[])objWebService.getFields(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "LOGS BY FIELD DEPTH":
                    objNode = (XmlNode[])objWebService.getFieldLogs(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "LISTOFWELLS":
                    objNode = (XmlNode[])objWebService.getWells(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "LISTOFWELLBORES":
                    objNode = (XmlNode[])objWebService.getWellbore(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "ONSHORE OFFSHORE":
                    objNode = (XmlNode[])objWebService.getOnshoreOffshore(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "PICKS":
                    objNode = (XmlNode[])objWebService.getPicks(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "PICKSDETAIL":
                    objNode = (XmlNode[])objWebService.getPicksDetail(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "PARSADVSEARCH":
                    objNode = (XmlNode[])objWebService.getProjectArchive(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "PROJECT ARCHIVES":
                    objNode = (XmlNode[])objWebService.getProjectArchive(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "PARSDETAIL":
                    objNode = (XmlNode[])objWebService.getProjectArchiveDetails(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "RECALLLOG":
                    objNode = (XmlNode[])objWebService.getWellboreLogs(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "RECALLLOGS":
                    objNode = (XmlNode[])objWebService.getWellboreLogs(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "RECALLLOGDETAIL":
                    objNode = (XmlNode[])objWebService.getCurveLogs(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "RECALLCURVE":
                    objNode = (XmlNode[])objWebService.getCurveLogs(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "STATE OR PROVINCE":
                    objNode = (XmlNode[])objWebService.getStateOrProvince(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "TIMEDEPTH":
                    objNode = (XmlNode[])objWebService.getTimeDepth(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "TIMEDEPTHDETAIL":
                    objNode = (XmlNode[])objWebService.getTimeDepthArray(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "WELLBORE":
                    objNode = (XmlNode[])objWebService.getWellbore(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "WELLBOREHEADER":
                    objNode = (XmlNode[])objWebService.getWellboreHeader(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "WELLBORE KIND":
                    objNode = (XmlNode[])objWebService.getWellboreKind(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "WELLBORE STATUS":
                    objNode = (XmlNode[])objWebService.getWellboreStatus(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "WELL":
                    objNode = (XmlNode[])objWebService.getWells(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "MECHANICALDATA":
                    objNode = (XmlNode[])objWebService.getMechanicalWellData(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "MECHANICALDATADEPTHREF":
                    objNode = (XmlNode[])objWebService.getDepthReference(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "AHTVCALCULATOR":
                    objNode = (XmlNode[])objWebService.getWellborePath(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "AHTVCONVERTEDPATH":
                    objNode = (XmlNode[])objWebService.getConvertedDepth(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "PALEOMARKERS":
                    objNode = (XmlNode[])objWebService.getPaleo(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "GEOPRESSURE":
                case "PRESSURESURVEYDATA":
                case "PERFORATIONS": // PerforationLogs
                case "APIIDENTIFIERS"://APIIdentifiers
                case "BLOCK"://Block
                case "WELLTESTREPORT": //WellTestReport
                case "WELLTESTDATA": //WellTestData
                case "ZONEPROPERTIES": //ZoneProperties
                case "OWPROJECTS": //ListOfProjectsOW
                case "SRP FIELD ADVANCE SEARCH": //SRP Field Advance Search
                case "FIELD OPERATOR": // Field Operator
                case "RESERVOIR": // Reservoir
                case "RESERVOIRHEADER": //ReservoirHeader
                case "RESERVOIR ADVANCE SEARCH": //Reservoir Advance Search
                case "RESERVOIR LITHOSTRAT GROUP": //Reservoir Lithostrat Group
                case "RESERVOIR LITHOSTRAT FORMATION": //Reservoir Lithostrat Formation
                case "RESERVOIR LITHOSTRAT MEMBER": //Reservoir Lithostrat Member
                case "INTERPOLATEDLOGS"://Interpolated Logs
                case "POSITIONLOGDATA"://Positional Log Data
                    {
                        objNode = (XmlNode[])objWebService.getSearchResults(objNodeParem, maxRecords, sortColumn, sortOrder);
                        break;
                    }
                case "FIELDHEADER": // FieldHeader
                    {
                        objNode = (XmlNode[])objWebService.getSearchResults(objNodeParem, maxRecords, sortColumn, sortOrder);
                        /// Process the XML to replace "Tectonic Settings" with proper values
                        XmlDocument objXmlTempSearchResults = new XmlDocument();
                        objXmlTempSearchResults.LoadXml(objNode[0].OuterXml.ToString());
                        ResponseXMLInterceptor objResponseXMLInterceptor = new ResponseXMLInterceptor();
                        string strResponseType = string.Empty;
                        if(xmlSearchRequest != null)
                        {
                            strResponseType = xmlSearchRequest.SelectSingleNode("requestinfo/entity").Attributes["responsetype"].Value;
                        }
                        objXmlTempSearchResults = objResponseXMLInterceptor.InsertTectonicSettings(objXmlTempSearchResults, strResponseType);
                        objNode[0] = objXmlTempSearchResults.DocumentElement.ParentNode;
                        break;
                    }
                case "COUNTRY":
                    {
                        objNode = (XmlNode[])objWebService.getCountry(objNodeParem, maxRecords, sortColumn, sortOrder);
                        break;
                    }
            }
            if(objNode != null)
            {
                objXmlSearhResultsDoc = new XmlDocument();
                /// Loads the search results 
                objXmlSearhResultsDoc.LoadXml(objNode[0].OuterXml.ToString());
                #region Response XML Logger added in DREAM 4.0
                objSearchRequestHandler.XmlLogger(objXmlSearhResultsDoc, XMLTYPERESPONSE, XMLLOGDOCLIB);
                #endregion
            }
            return objXmlSearhResultsDoc;
        }
        #endregion

        #region GETSEARCHRESULTS METHODS

        /// <summary>
        /// Gets the search results.
        /// </summary>
        /// <returns></returns>
        public override XmlDocument GetSearchResults()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// Gets the search results.
        /// </summary>
        /// <param name="searchRequest">The search request.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns></returns>
        public override XmlDocument GetSearchResults(XmlDocument searchRequest,
                                            int maxRecords, string searchType, string sortColumn, int sortOrder)
        {
            base.MaxRecord = maxRecords;
            base.SearchType = searchType;
            base.SortColumn = sortColumn;
            base.SortOrder = sortOrder;
            return base.GetSearchResults(searchRequest);
        }
        /// <summary>
        /// Gets the search results.
        /// </summary>
        /// <param name="requestInfo">The request info.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns></returns>
        public override XmlDocument GetSearchResults(RequestInfo requestInfo, int maxRecords,
                                           string searchType, string sortColumn, int sortOrder)
        {
            xmlSearhRequestDoc = objSearchRequestHandler.CreateRequestXML(requestInfo);
            base.MaxRecord = maxRecords;
            base.SearchType = searchType;
            base.SortColumn = sortColumn;
            base.SortOrder = sortOrder;
            return base.GetSearchResults(xmlSearhRequestDoc);
        }
        /// <summary>
        /// Creates the search request.
        /// </summary>
        /// <param name="requestInformation">The request information.</param>
        /// <returns></returns>
        public override XmlDocument CreateSearchRequest(object requestInformation)
        {
            SearchRequestHandler objSearchRequestHandler = null;
            XmlDocument objXmlSearhRequestDoc = null;
            objXmlSearhRequestDoc = new XmlDocument();
            objSearchRequestHandler = new SearchRequestHandler();
            RequestInfo objRequestInfo = (RequestInfo)requestInformation;
            //Creates the request XML.
            objXmlSearhRequestDoc = objSearchRequestHandler.CreateRequestXML(objRequestInfo);
            return objXmlSearhRequestDoc;
        }
        /// <summary>
        /// Fetches the results.
        /// </summary>
        /// <param name="searchRequest">The search request.</param>
        /// <returns></returns>
        public override XmlDocument FetchResults(XmlDocument searchRequest)
        {
            return FetchReportServiceResults(searchRequest, base.MaxRecord, base.SearchType, base.SortColumn, base.SortOrder);

        }
        #endregion
    }
}
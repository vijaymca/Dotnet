#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: ResourceServiceManager.cs
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Net;
using System.Web.Services.Protocols;
using Shell.SharePoint.DREAM.Business.Entities;

namespace Shell.SharePoint.DREAM.Controller
{
    class EventServiceManager :AbstractController
    {
        EventServicesProxy objEventService;
        XmlNode[] objNodeParem;
        XmlNode[] objNode;
        XmlDocument xmlSearhRequestDoc;
        SearchRequestHandler objSearchRequestHandler;
        const string XPATH = "/requestinfo/entity";

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
        /// <param name="request">The request.</param>
        /// <returns></returns>
        //public override XmlDocument GetSearchResults(EPRequest request)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}
        #region CONSTRUCTOR
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportServiceManager"/> class.
        /// </summary>
        public EventServiceManager(string siteURL)
        {
            try
            {
                objEventService = new EventServicesProxy(siteURL);
                objNodeParem = new XmlNode[1];
                xmlSearhRequestDoc = new XmlDocument();
                objSearchRequestHandler = new SearchRequestHandler();
                ;
            }
            catch(Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportServiceManager"/> class.
        /// </summary>
        public EventServiceManager()
        {
            try
            {
                objEventService = new EventServicesProxy();
                objNodeParem = new XmlNode[1];
                xmlSearhRequestDoc = new XmlDocument();
                objSearchRequestHandler = new SearchRequestHandler();
                ;
            }
            catch(Exception)
            {
                throw;
            }
        }
        #endregion

        /// <summary>
        /// Creates the search request.
        /// </summary>
        /// <param name="requestInfo">The request info.</param>
        /// <returns></returns>
        public override XmlDocument CreateSearchRequest(object requestInformation)
        {
            SearchRequestHandler objSearchRequestHandler = null;
            XmlDocument objXmlSearhRequestDoc = null;
            try
            {
                objXmlSearhRequestDoc = new XmlDocument();
                objSearchRequestHandler = new SearchRequestHandler();
                RequestInfo objRequestInfo = (RequestInfo)requestInformation;
                //Creates the request XML.
                objXmlSearhRequestDoc = objSearchRequestHandler.CreateRequestXML(objRequestInfo);
            }
            catch(Exception)
            {
                throw;
            }
            return objXmlSearhRequestDoc;
        }
        /// <summary>
        /// Fetches the results.
        /// </summary>
        /// <param name="searchRequest">The search request.</param>
        /// <returns></returns>
        public override XmlDocument FetchResults(XmlDocument searchRequest)
        {
            try
            {
                return FetchReportServiceResults(searchRequest, base.SearchType, base.SortColumn);
            }
            catch(SoapException)
            {
                throw;
            }
            catch(WebException)
            {
                throw;
            }
            catch(Exception)
            {
                throw;
            }
        }
        #region CALLING WEBMETHODS
        /// <summary>
        /// Fetches the results.
        /// </summary>
        /// <param name="xmlSearchRequest">The XML search request.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns></returns>
        private XmlDocument FetchReportServiceResults(XmlDocument xmlSearchRequest, string searchType,
                                        string sortColumn)
        {
            #region Request XML Logger added in DREAM 4.0
            objSearchRequestHandler.XmlLogger(xmlSearchRequest, XMLTYPEREQUEST, XMLLOGDOCLIB);
            #endregion
            XmlDocument objXmlSearhResultsDoc = null;
            //Test code needs to revoed later
            //start
            //objXmlSearhResultsDoc = new XmlDocument();
            ////objXmlSearhResultsDoc.Load(@"C:\Dev Folder\DREAM4.0\ResponseXml\Response.xml");
            //objXmlSearhResultsDoc.Load(@"C:\Dev Folder\DREAM4.0\Modules\SignificantWellEvent\SignificantWellEventsResponse.xml");
            //if(objXmlSearhResultsDoc != null)
            //    return objXmlSearhResultsDoc;
            //end
            if(sortColumn != null)
                if(sortColumn.ToString().Trim().Length == 0)
                    sortColumn = null;
            try
            {

                objNodeParem[0] = xmlSearchRequest.SelectSingleNode(XPATH);
                //This switch case to get the search results from webservice based on search type.
                switch(searchType.ToUpper())
                {
                    case "WELLHISTORY":
                        objNode = (XmlNode[])objEventService.getWellHistory(objNodeParem);
                        break;
                    case "EDMREPORT":
                        objNode = (XmlNode[])objEventService.getEdmReport(objNodeParem);
                        break;
                    case "EVENTGROUPCODE":
                        {
                            objNode = (XmlNode[])objEventService.getEventGroups(objNodeParem, base.MaxRecord, base.SortColumn, base.SortOrder);
                            break;
                        }
                    case "EVENTNAME":
                        {
                            objNode = (XmlNode[])objEventService.getEventTypes(objNodeParem, base.MaxRecord, base.SortColumn, base.SortOrder);
                            break;
                        }
                    case "SIGNIFICANTWELLEVENTS":
                        {
                            objNode = (XmlNode[])objEventService.getSwedEvents(objNodeParem, base.MaxRecord, base.SortColumn, base.SortOrder);
                            break;
                        }
                    case "WELLREVIEWS":
                        {
                            objNode = (XmlNode[])objEventService.getSwedEvents(objNodeParem, base.MaxRecord, base.SortColumn, base.SortOrder);
                            break;
                        }
                }
                if(objNode != null)
                {
                    objXmlSearhResultsDoc = new XmlDocument();
                    //Loads the search results 
                    objXmlSearhResultsDoc.LoadXml(objNode[0].OuterXml.ToString());
                    #region Response XML Logger added in DREAM 4.0
                    objSearchRequestHandler.XmlLogger(objXmlSearhResultsDoc, XMLTYPERESPONSE, XMLLOGDOCLIB);
                    #endregion
                }
            }
            catch(SoapException)
            {
                throw;
            }
            catch(WebException)
            {
                throw;
            }
            catch(Exception)
            {
                throw;
            }
            return objXmlSearhResultsDoc;
        }
        #endregion
        #region GETSEARCHRESULTS METHODS
        /// <summary>
        /// Gets the search results.
        /// </summary>
        /// <param name="xmlSearchRequest">The XML search request.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns></returns>
        public override XmlDocument GetSearchResults(XmlDocument searchRequest,
                                            int maxRecords, string searchType, string sortColumn, int sortOrder)
        {
            try
            {
                base.MaxRecord = maxRecords;
                base.SearchType = searchType;
                base.SortColumn = sortColumn;
                base.SortOrder = sortOrder;
                return base.GetSearchResults(searchRequest);
            }
            catch(SoapException)
            {
                throw;
            }
            catch(WebException)
            {
                throw;
            }
            catch(Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Gets the search results.
        /// </summary>
        /// <param name="requestInfo">The request info.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns></returns>
        public override XmlDocument GetSearchResults(RequestInfo requestInfo, int maxRecords,
                                           string searchType, string sortColumn, int sortOrder)
        {
            try
            {
                xmlSearhRequestDoc = objSearchRequestHandler.CreateRequestXML(requestInfo);
                base.MaxRecord = maxRecords;
                base.SearchType = searchType;
                base.SortColumn = sortColumn;
                base.SortOrder = sortOrder;
                return base.GetSearchResults(xmlSearhRequestDoc);
            }
            catch(SoapException)
            {
                throw;
            }
            catch(WebException)
            {
                throw;
            }
            catch(Exception)
            {
                throw;
            }
        }
        #endregion
    }
}

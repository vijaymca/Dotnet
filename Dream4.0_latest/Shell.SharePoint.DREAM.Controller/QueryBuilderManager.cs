#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: QueryBuilderManager.cs
#endregion

using System;
using System.Net;
using System.Xml;
using System.Web;
using System.Web.Services.Protocols;
using System.Web.UI.WebControls;
using Shell.SharePoint.DREAM.Business.Entities;

namespace Shell.SharePoint.DREAM.Controller
{
    public class QueryBuilderManager :AbstractController
    {
        #region DECLARATION
        QueryBuilder.QueryManagerService objWebService;
        XmlNode[] objNodeParem;
        XmlNode[] objNode;
        XmlDocument xmlSearhRequestDoc;
        SearchRequestHandler objSearchRequestHandler;
        const string XPATH = "/requestinfo/entity";
        #endregion

        #region CONSTRUCTOR
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportServiceManager"/> class.
        /// </summary>
        public QueryBuilderManager()
        {
            try
            {
                objWebService = new QueryBuilder.QueryManagerService();
                objNodeParem = new XmlNode[1];
                xmlSearhRequestDoc = new XmlDocument();
                objSearchRequestHandler = new SearchRequestHandler();
            }
            catch(Exception)
            {
                throw;
            }
        }
        #endregion

        #region Overridden method
        /// <summary>
        /// Gets the search results.
        /// </summary>
        /// <returns></returns>
        public override XmlDocument GetSearchResults()
        {
            throw new Exception("The method or operation is not implemented.");
        }
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
            XmlDocument objXmlQueryResultsDoc = null;
            try
            {
                #region Request XML Logger added in DREAM 4.0
                objSearchRequestHandler.XmlLogger(searchRequest, XMLTYPEREQUEST, XMLLOGDOCLIB);
                #endregion
                objNodeParem[0] = searchRequest.SelectSingleNode(XPATH);
                objNode = (XmlNode[])objWebService.getQueryResults(objNodeParem, base.MaxRecord, base.SortColumn, base.SortOrder);
                if(objNode != null)
                {
                    objXmlQueryResultsDoc = new XmlDocument();
                    //Loads the Resource results 
                    objXmlQueryResultsDoc.LoadXml(objNode[0].OuterXml.ToString());
                    #region Response XML Logger added in DREAM 4.0
                    objSearchRequestHandler.XmlLogger(objXmlQueryResultsDoc, XMLTYPERESPONSE, XMLLOGDOCLIB);
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
            return objXmlQueryResultsDoc;
        }
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
        #endregion
    }
}

#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: ResourceServiceManager.cs
#endregion

using System;
using System.Net;
using System.Xml;
using System.Web.Services.Protocols;
using Shell.SharePoint.DREAM.Business.Entities;

namespace Shell.SharePoint.DREAM.Controller
{
    /// <summary>
    /// Service Manager class for Resource Service.
    /// </summary>
    public class ResourceServiceManager :AbstractController
    {
        #region DECLARATION
        #region Constants
        const string SIMPLE = "simple";
        const string ADVANCED = "advanced";
        const string USERNAMES = "usernames";
        const string SEARCNAMES = "searchnames";
        const string XPATH = "/requestinfo/entity";
        #endregion

        #region Variables
        ResourceServiceProxy objWebService;
        XmlNode[] objNodeParem;
        XmlNode[] objNode;
        SearchRequestHandler objSearchRequestHandler;
        #endregion
        #endregion
        #region CONSTRUCTOR
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceServiceManager"/> class.
        /// </summary>
        public ResourceServiceManager()
        {
            objWebService = new ResourceServiceProxy();
            objNodeParem = new XmlNode[1];
            objSearchRequestHandler = new SearchRequestHandler();
        }
        #endregion

        #region ABSTRACTMETHOD IMPLEMENTATION
        public override XmlDocument CreateSearchRequest(object requestObject)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override XmlDocument GetSearchResults(RequestInfo requestInfo, int maxRecords, string searchType, string sortColumn, int sortOrder)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override XmlDocument GetSearchResults(XmlDocument searchRequest, int maxRecords, string searchType, string sortColumn, int sortOrder)
        {
            throw new Exception("The method or operation is not implemented.");
        }


        #endregion
        #region CALLING WEBMETHODS
        /// <summary>
        /// Fetches the results.
        /// </summary>
        /// <returns></returns>
        public override XmlDocument GetSearchResults()
        {
            XmlDocument xmlDoc = null;
            return base.GetSearchResults(xmlDoc);
        }
        /// <summary>
        /// Fetches the results.
        /// </summary>
        /// <param name="searchRequest">The search request.</param>
        /// <returns></returns>
        public override XmlDocument FetchResults(XmlDocument searchRequest)
        {
            #region Request XML Logger added in DREAM 4.0
            objSearchRequestHandler.XmlLogger(searchRequest, XMLTYPEREQUEST, XMLLOGDOCLIB);
            #endregion
            XmlDocument objXmlResourceResultsDoc = null;
            objNode = (XmlNode[])objWebService.getResourceStatus();
            if(objNode != null)
            {
                objXmlResourceResultsDoc = new XmlDocument();
                //Loads the Resource results 
                objXmlResourceResultsDoc.LoadXml(objNode[0].OuterXml.ToString());
                #region Response XML Logger added in DREAM 4.0
                objSearchRequestHandler.XmlLogger(objXmlResourceResultsDoc, XMLTYPERESPONSE, XMLLOGDOCLIB);
                #endregion
            }
            return objXmlResourceResultsDoc;
        }
        //Added by Dev For Functionality usage report
        //start
        /// <summary>
        /// Gets the FUR report based on report type
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <returns></returns>
        public XmlDocument GetFURReport(XmlDocument searchRequest, string searchType,
                                        int maxRecords,
                                        string sortColumn, int sortOrder)
        {
            #region Request XML Logger added in DREAM 4.0
            objSearchRequestHandler.XmlLogger(searchRequest, XMLTYPEREQUEST, XMLLOGDOCLIB);
            #endregion
            XmlDocument objXMLResultsDoc = null;
            if(searchRequest != null)
            {
                objNodeParem[0] = searchRequest.SelectSingleNode(XPATH);
            }

            //This switch case to get the FUR search results from webservice based on search type.
            switch(searchType.ToLower())
            {
                case "simple":
                case "advanced":
                    objNode = (XmlNode[])objWebService.getFunctionalityUsage(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
                case "searchnames":
                    objNode = (XmlNode[])objWebService.getSearchNames();
                    break;
                case "usernames":
                    objNode = (XmlNode[])objWebService.getUserNames(objNodeParem, maxRecords, sortColumn, sortOrder);
                    break;
            }
            if(objNode != null)
            {
                objXMLResultsDoc = new XmlDocument();
                //Loads the Resource results 
                objXMLResultsDoc.LoadXml(objNode[0].OuterXml.ToString());
                #region Response XML Logger added in DREAM 4.0
                objSearchRequestHandler.XmlLogger(objXMLResultsDoc, XMLTYPERESPONSE, XMLLOGDOCLIB);
                #endregion
            }
            return objXMLResultsDoc;
        }

        //end
        #endregion
    }
}

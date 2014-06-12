#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: AbstractController.cs
#endregion

/// <summary> 
/// This class is the abstract controller.
/// </summary> 
using System;
using System.Xml;
using System.Web.Services.Protocols;
using Shell.SharePoint.DREAM.Business.Entities;
using System.Net;

namespace Shell.SharePoint.DREAM.Controller
{
    /// <summary>
    /// Abstract Controller class
    /// </summary>
    public abstract class AbstractController
    {
        #region DECLARATION
        //Added in DREAM 4.0 for request xml logger
        protected const string XMLLOGDOCLIB = "XmlLog";
        protected const string XMLTYPERESPONSE = "response";
        protected const string XMLTYPEREQUEST = "request";
        int intMaxRecord;
        int intSortOrder;
        string strSortColumn;
        string strSearchType;
        XmlDocument xmlResultDoc;
        #endregion
        #region PROPERTIES
        /// <summary>
        /// Gets or sets the type of the search.
        /// </summary>
        /// <value>The type of the search.</value>
        protected string SearchType
        {
            get
            {
                return strSearchType;
            }
            set
            {
                strSearchType = value;
            }
        }
        /// <summary>
        /// Gets or sets the max record.
        /// </summary>
        /// <value>The max record.</value>
        protected int MaxRecord
        {
            get
            {
                return intMaxRecord;
            }
            set
            {
                intMaxRecord = value;
            }
        }
        /// <summary>
        /// Gets or sets the sort column.
        /// </summary>
        /// <value>The sort column.</value>
        protected string SortColumn
        {
            get
            {
                return strSortColumn;
            }
            set
            {
                strSortColumn = value;
            }
        }
        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        /// <value>The sort order.</value>
        protected int SortOrder
        {
            get
            {
                return intSortOrder;
            }
            set
            {
                intSortOrder = value;
            }
        }
        #endregion
        #region ABSTRACT METHODS
        /// <summary>
        /// Gets the search results.
        /// </summary>
        /// <param name="searchRequest">The search request.</param>
        /// <returns></returns>
        public XmlDocument GetSearchResults(XmlDocument searchRequest)
        {
            try
            {
                xmlResultDoc = FetchResults(searchRequest);
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
            return xmlResultDoc;
        }
        /// <summary>
        /// Creates the search request.
        /// </summary>
        /// <param name="requestObject">The request object.</param>
        /// <returns></returns>
        public abstract XmlDocument CreateSearchRequest(Object requestObject);
        /// <summary>
        /// Gets the search results.
        /// </summary>
        /// <param name="searchRequest">The search request.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns></returns>
        public abstract XmlDocument GetSearchResults(XmlDocument searchRequest, int maxRecords, string searchType, string sortColumn, int sortOrder);
        /// <summary>
        /// Gets the search results.
        /// </summary>
        /// <param name="requestInfo">The request info.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns></returns>
        public abstract XmlDocument GetSearchResults(RequestInfo requestInfo, int maxRecords, string searchType, string sortColumn, int sortOrder);
        /// <summary>
        /// Gets the search results.
        /// </summary>
        /// <returns></returns>
        public abstract XmlDocument GetSearchResults();
        /// <summary>
        /// Fetches the results.
        /// </summary>
        /// <param name="searchRequest">The search request.</param>
        /// <returns></returns>
        public abstract XmlDocument FetchResults(XmlDocument searchRequest);
        #endregion
    }
}

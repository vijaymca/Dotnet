#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: PdfBLL.cs
#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.XPath;
using System.Xml;
using System.Xml.Xsl;
using Microsoft.SharePoint;
using Winnovative.PdfCreator;
using Shell.SharePoint.DWB.DataAccessLayer;
using Shell.SharePoint.DREAM.SearchHelper;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DREAM.Business.Entities;


namespace Shell.SharePoint.DWB.BusinessLogicLayer
{
    /// <summary>
    /// BLL class for PDF Service.
    /// </summary>
    public class PdfBLL
    {
        #region Variables
        string strRangeName = string.Empty;
        string strRangeValue = string.Empty;
        string strColorName = string.Empty;
        string strColorValue = string.Empty;
        string strCAMLNarrative;
        string strFieldNarrative;
        private CommonDAL objDAL;
        private Stream memStream;
        protected AbstractController objMOSSController;
        protected CommonUtility objCommonUtility;
        protected UserPreferences objPreferences;
        private StringWriter strwr;
        int intRecordsPerPage = 100;
        #endregion

        #region Constants
        const string IQMCONFIGURATION = "IQM Configurations";
        const string SORTDEFAULTORDER = "descending";
        const string SORTORDER = "sortType";
        const string COLUMNSTOLOCK = "ColumnsToLock";
        const string SEARCHTYPEPARAMETER = "SearchType";
        const string USERPREFERENCELABEL = "userPreference";
        const string FORMULAVALUETITLE = "formulaValue";
        const string MAXRECORDS = "MaxRecords";
        const double PIVALUE = 3.28084;
        const string XPATH = "/response/report";
        const string RESULTRECORDLOCKXPATH = "/response/report/record[@recordno=1]/attribute";
        const string RECORDSPERPAGE = "recordsPerPage";
        const string PAGENUMBER = "pageNumber";
        const string RECORDCOUNT = "recordCount";
        const string CURRENTPAGENAME = "CurrentPageName";
        const string CURRENTPAGE = "CurrentPage";
        const string MAXPAGES = "MaxPages";
        const int MAXPAGEVALUE = 5;
        const string WINDOWTITLE = "windowTitle";
        const string REQUESTID = "requestId";//Added for cashing
        const string SORTCOLUMN = "sortBy";
        const string ISSORTAPPLICABLE = "IsSortApplicable";
        const string FALSE = "false";
        const string EXTENSIONOBJECT = "urn:DATE";
        const string MOSSSERVICE = "MossService";
        const string YES = "Yes";
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the STRing fields narrative.
        /// </summary>
        /// <value>The STR field narrative.</value>
        private String FieldNarrative
        {
            get { return strFieldNarrative; }
            set { strFieldNarrative = value; }
        }
        /// <summary>
        /// Gets or sets the STRing CAML Query FOR narrative.
        /// </summary>
        /// <value>The STR CAML narrative.</value>
        private String CAMLNarrative
        {
            get { return strCAMLNarrative; }
            set { strCAMLNarrative = value; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the narrative FOR pAGE.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="context">The context.</param>
        /// <param name="listName">Name of the list.</param>
        /// <returns></returns>
        public string GetNarrative(int pageId, string context, string listName)
        {
            string strNarrative = string.Empty;
            objDAL = new CommonDAL();
            CAMLNarrative = @"<Where>
                              <Eq>
                                 <FieldRef Name='Page_ID' />
                                 <Value Type='Number'>" + pageId + @"</Value>
                              </Eq>
                           </Where>";
            FieldNarrative = @"<FieldRef Name='Narrative'/>";
            DataTable dtNarrative = new DataTable();
            dtNarrative = objDAL.ReadList(context, listName, CAMLNarrative, FieldNarrative);

            if (dtNarrative != null && dtNarrative.Rows.Count > 0)
            {
                strNarrative = Convert.ToString(dtNarrative.Rows[0][0]);
                dtNarrative.Dispose();
            }
            return strNarrative;
        }

        /// <summary>
        /// Downloads the file from document library.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="docLibName">Name of the doc lib.</param>
        /// <param name="pageId">The page id.</param>
        /// <returns>SPFile</returns>
        public SPFile DownloadFileFromDocumentLibrary(string siteURL, string docLibName, string pageId)
        {
            CommonDAL objDAL = new CommonDAL();
            return objDAL.DownloadFileFromDocumentLibrary(siteURL, docLibName, pageId);
        }

        /// <summary>
        /// Gets the book title.
        /// </summary>
        /// <param name="xmlDocumentCriteria">The XML document criteria.</param>
        /// <returns></returns>
        public string GetAttributeBookInfo(XPathDocument xmlDocumentCriteria, String attributeName)
        {
            try
            {
                string strAttributeBookInfo = string.Empty;
                XPathNavigator xnav = xmlDocumentCriteria.CreateNavigator();
                /// Set up namespace manager for XPath  
                XmlNamespaceManager ns = new XmlNamespaceManager(xnav.NameTable);
                ns.AddNamespace("PDF", "http://www.shell.com/");

                XPathNodeIterator nodes = xnav.Select("/BookInfo");
                if (nodes != null)
                {
                    nodes.MoveNext();

                    XPathNavigator node = nodes.Current;
                    strAttributeBookInfo = node.GetAttribute(attributeName, ns.DefaultNamespace);
                }
                return strAttributeBookInfo;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Uploads the published document.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="bookID">The book ID.</param>
        /// <param name="docPDF">The Winnovative document PDF.</param>
        /// <param name="bookName">Name of the book.</param>
        /// <param name="listPublished">The Name of the published list.</param>
        /// <returns>Int Id</returns>
        public int UploadPublishedDocument(string context, string bookID, Document docPDF, string bookName,
                                            string listPublished)
        {
            objDAL = new CommonDAL();
            memStream = new MemoryStream();

            string strSaveToNetworkPath = PortalConfiguration.GetInstance().FindWebServiceKey("SaveToNetworkPath", context, true);
            string strNetworkPath = string.Empty;
            byte[] fileBuffer;//= new byte[1024];
            FileStream fsDWBBook = null;
            int intResult = 0;
            objCommonUtility = new CommonUtility();
            string strbookName = string.Empty;
            if (!string.Equals(listPublished, "DWB Published Library"))
            {
                string strBookGuid = Guid.NewGuid().ToString();
                strbookName = string.Format("{0}_{1}", bookName, strBookGuid);
            }
            else
            {
                strbookName = bookName;
            }
            if (!string.IsNullOrEmpty(strSaveToNetworkPath) && string.Equals(strSaveToNetworkPath.ToLowerInvariant(), YES.ToLowerInvariant()))
            {
                /// Save to the path mentioned in Portal Configuration
                strNetworkPath = PortalConfiguration.GetInstance().FindWebServiceKey("DWBNetworkPath", context, true);
                string strBookPath = string.Format(@"{0}{1}.pdf", strNetworkPath, strbookName);
                docPDF.Save(strBookPath);
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    fsDWBBook = File.Open(strBookPath, FileMode.Open, FileAccess.Read);
                });
                if (fsDWBBook != null)
                {
                    fileBuffer = new byte[fsDWBBook.Length];
                    fsDWBBook.Read(fileBuffer, 0, fileBuffer.Length);

                    if (!string.IsNullOrEmpty(strbookName))
                    {
                        intResult = objDAL.UploadPDFFileToDocumentLibrary(context, bookID, fsDWBBook, strbookName, listPublished);
                    }
                    else
                    {
                        intResult = objDAL.UploadPDFFileToDocumentLibrary(context, bookID, fsDWBBook, bookName, listPublished);
                    }
                    fsDWBBook.Close();
                }
            }
            else
            {
                ///  Save to doc library
                docPDF.Save(memStream);

                memStream.Position = 0;
                if (!string.IsNullOrEmpty(strbookName))
                {
                    intResult = objDAL.UploadPDFFileToDocumentLibrary(context, bookID, memStream, strbookName,
                                                      listPublished);
                }
                else
                {
                    intResult = objDAL.UploadPDFFileToDocumentLibrary(context, bookID, memStream, bookName,
                                                      listPublished);
                }
                memStream.Close();
            }
            return intResult;
        }

        /// <summary>
        /// Gets the attribute page info.
        /// </summary>
        /// <param name="xmlDocumentCriteria">The XML document criteria.</param>
        /// <param name="strAttributeName">Name of the STR attribute.</param>
        /// <returns>string</returns>
        public string GetAttributePageInfo(XPathNavigator xnav, String attributeName)
        {
            try
            {
                // Set up namespace manager for XPath  
                XmlNamespaceManager ns = new XmlNamespaceManager(xnav.NameTable);
                ns.AddNamespace("PDF", "http://www.shell.com/");

                XPathNodeIterator nodes = xnav.Select("/PageInfo");
                nodes.MoveNext();

                XPathNavigator node = nodes.Current;
                return (node.GetAttribute(attributeName, ns.DefaultNamespace));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Transforms the form search results to XSL.
        /// </summary>
        /// <param name="document">The XML document.</param>
        /// <param name="textReader">The XML text reader.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="sortByColumn">The sortby column.</param>
        /// <param name="sortOrder">The sortorder.</param>
        /// <param name="searchType">Type of the search.</param>
        public StringWriter TransformSearchResultsToXSL(XmlDocument document, XmlTextReader textReader,
                                                string pageNumber, string sortByColumn, string sortOrder,
                                                string searchType, int maxRecords, string windowTitle, int recordCount, string context)
        {
            //XslCompiledTransform xslTransform = null;
            XslTransform xslTransform = null;
            XmlDocument objXmlDocForXSL = null;
            XsltArgumentList xsltArgsList = null;
            XmlNodeList objXmlNodeList = null;
            XPathDocument objXPathDocument = null;
            MemoryStream objMemoryStream = null;
            DateTimeConvertor objDateTimeConvertor = null;

            ServiceProvider objFactory = new ServiceProvider();
            objMOSSController = objFactory.GetServiceManager(MOSSSERVICE);
            strwr = new StringWriter();
            int intPageNumber = 0;

            int intRecordCount = 0;
            double dblCurrentPage = 0D;
            string strSortOrder = SORTDEFAULTORDER;
            //string strDepthUnit = string.Empty;
            string strDepthUnit = "metres";
            string strRequestID = string.Empty;


            try
            {
                if (document != null && textReader != null)
                {
                    //Inititlize the system and custom objects
                    objCommonUtility = new CommonUtility();
                    xslTransform = new XslTransform();
                    objXmlDocForXSL = new XmlDocument();
                    xsltArgsList = new XsltArgumentList();

                    objMemoryStream = new MemoryStream();
                    document.Save(objMemoryStream);
                    objMemoryStream.Position = 0;
                    objXPathDocument = new XPathDocument(objMemoryStream);

                    //Inititlize the XSL
                    objXmlDocForXSL.Load(textReader);
                    xslTransform.Load(objXmlDocForXSL);
                    //the below condition validates the pageNumber parameter value.
                    if (pageNumber.Length > 0)
                    {
                        intPageNumber = Convert.ToInt32(pageNumber);
                    }
                    //the below condition validates the sortOrder parameter value.
                    if (sortOrder.Length > 0)
                    {
                        strSortOrder = sortOrder;
                    }

                    //Get the Total Record Count
                    objXmlNodeList = document.SelectNodes(XPATH);
                    if (objXmlNodeList != null)
                    {
                        foreach (XmlNode xmlNode in objXmlNodeList)
                        {
                            intRecordCount = Convert.ToInt32(xmlNode.SelectSingleNode("recordcount").InnerXml.ToString());
                        }
                    }
                    //the below condition validates the pageNumber parameter value.
                    if (pageNumber.Length > 0)
                    {
                        dblCurrentPage = Double.Parse(pageNumber);
                        intRecordCount = recordCount; //Added for cashing
                    }
                    //Added for cashing
                    //Get the Response ID
                    objXmlNodeList = document.SelectNodes("response");
                    if (objXmlNodeList != null)
                    {
                        foreach (XmlNode xmlNode in objXmlNodeList)
                        {
                            strRequestID = xmlNode.Attributes["requestid"].Value.ToString();
                        }
                    }
                    if (sortByColumn == null)
                        sortByColumn = string.Empty;
                    int intColumnsToLock = GetNumberofRecordsToLock(document, RESULTRECORDLOCKXPATH);
                    //Add the required parameters to the XsltArgumentList object
                    GetDataQualityRange(ref xsltArgsList, context);

                    objDateTimeConvertor = new DateTimeConvertor();
                    //xsltArgsList.AddExtensionObject(EXTENSIONOBJECT, objDateTimeConvertor);
                    //xsltArgsList.AddParam(RECORDSPERPAGE, string.Empty, intRecordsPerPage);
                    //xsltArgsList.AddParam(PAGENUMBER, string.Empty, intPageNumber);
                    //xsltArgsList.AddParam(RECORDCOUNT, string.Empty, intRecordCount);
                    //xsltArgsList.AddParam(CURRENTPAGENAME, string.Empty, objCommonUtility.GetCurrentPageName(true));
                    //xsltArgsList.AddParam(CURRENTPAGE, string.Empty, dblCurrentPage + 1);
                    //xsltArgsList.AddParam(MAXPAGES, string.Empty, MAXPAGEVALUE);
                    //xsltArgsList.AddParam(SORTCOLUMN, string.Empty, sortByColumn);
                    //xsltArgsList.AddParam(SORTORDER, string.Empty, strSortOrder);
                    //xsltArgsList.AddParam(COLUMNSTOLOCK, string.Empty, intColumnsToLock);
                    //xsltArgsList.AddParam(SEARCHTYPEPARAMETER, string.Empty, searchType);
                    //xsltArgsList.AddParam(USERPREFERENCELABEL, string.Empty, strDepthUnit.ToLower());
                    //xsltArgsList.AddParam(FORMULAVALUETITLE, string.Empty, PIVALUE);
                    //xsltArgsList.AddParam(MAXRECORDS, string.Empty, maxRecords);
                    //xsltArgsList.AddParam(WINDOWTITLE, string.Empty, windowTitle);
                    //xsltArgsList.AddParam(REQUESTID, string.Empty, strRequestID);
                    //xsltArgsList.AddParam(ISSORTAPPLICABLE, string.Empty, FALSE);

                    xsltArgsList.AddExtensionObject("urn:DATE", objDateTimeConvertor);
                    xsltArgsList.AddParam(RECORDSPERPAGE, string.Empty, intRecordsPerPage);
                    xsltArgsList.AddParam(PAGENUMBER, string.Empty, intPageNumber);
                    xsltArgsList.AddParam(RECORDCOUNT, string.Empty, intRecordCount);
                    xsltArgsList.AddParam(CURRENTPAGENAME, string.Empty, objCommonUtility.GetCurrentPageName(true));
                    xsltArgsList.AddParam(CURRENTPAGE, string.Empty, dblCurrentPage + 1);
                    xsltArgsList.AddParam(MAXPAGES, string.Empty, MAXPAGEVALUE);
                    xsltArgsList.AddParam(SORTCOLUMN, string.Empty, sortByColumn);
                    xsltArgsList.AddParam(SORTORDER, string.Empty, strSortOrder);
                    xsltArgsList.AddParam(COLUMNSTOLOCK, string.Empty, intColumnsToLock);
                    xsltArgsList.AddParam(SEARCHTYPEPARAMETER, string.Empty, searchType);
                    xsltArgsList.AddParam(USERPREFERENCELABEL, string.Empty, strDepthUnit.ToLower());
                    xsltArgsList.AddParam(FORMULAVALUETITLE, string.Empty, PIVALUE);
                    xsltArgsList.AddParam(MAXRECORDS, string.Empty, maxRecords);
                    xsltArgsList.AddParam(WINDOWTITLE, string.Empty, windowTitle);
                    xsltArgsList.AddParam(REQUESTID, string.Empty, strRequestID);

                    xslTransform.Transform(objXPathDocument, xsltArgsList, strwr);
                }
                else
                {
                    throw new Exception("Invalid Arguments");
                }
                return strwr;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (objMemoryStream != null)
                {
                    objMemoryStream.Close();
                    objMemoryStream.Dispose();
                }
            }
        }


        public void PrintLog(string parentSiteUrl, string message, string location, string title)
        {
            objDAL = new CommonDAL();
            objDAL.PrintLog(parentSiteUrl, message, location, title);
        }


        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the numberof records to lock.
        /// </summary>
        /// <param name="xmldocument">The xmldocument.</param>
        /// <returns></returns>
        private int GetNumberofRecordsToLock(XmlDocument xmldocument, string lockXPATH)
        {
            int intColumnsToLock = 0;
            if (xmldocument != null)
            {
                XmlNodeList objXmlNodeList = xmldocument.SelectNodes(lockXPATH);
                foreach (XmlNode xmlNodeValue in objXmlNodeList)
                {
                    if (string.Equals(xmlNodeValue.Attributes.GetNamedItem("title").Value.ToString(), "true"))
                    {
                        intColumnsToLock++;
                    }
                }
            }
            return intColumnsToLock;
        }

        /// <summary>
        /// Get the Quality Range values
        /// </summary>
        /// <param name="xsltArgsList">create xslt argument</param>
        private void GetDataQualityRange(ref XsltArgumentList xsltArgsList, string context)
        {
            DataTable dtListData = null;
            DataRow drListData;
            try
            {
                dtListData = ((MOSSServiceManager)objMOSSController).ReadList(context, IQMCONFIGURATION, string.Empty);
                if (dtListData.Rows.Count > 0)
                {
                    //Loop through the items in List
                    for (int index = 0; index < dtListData.Rows.Count; index++)
                    {
                        drListData = dtListData.Rows[index];
                        strRangeName = Convert.ToString(drListData["Title"]);
                        strRangeValue = Convert.ToString(drListData["Range_x0020_Value"]);
                        strColorName = Convert.ToString(drListData["Color_x0020_Name"]);
                        strColorValue = Convert.ToString(drListData["Color_x0020_Value"]);
                        xsltArgsList.AddParam(strRangeName, string.Empty, strRangeValue);
                        xsltArgsList.AddParam(strColorName, string.Empty, strColorValue);
                    }
                }
            }
            catch (Exception)
            { throw; }
            finally { if (dtListData != null) dtListData.Dispose(); }
        }
        #endregion
    }
}

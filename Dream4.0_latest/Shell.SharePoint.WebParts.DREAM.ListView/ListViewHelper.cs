#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: ListViewHelper.cs
#endregion

using System;
using System.Runtime.InteropServices;
using System.Web;
using System.Data;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Web.UI;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;

using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Business.Entities;
using System.Collections.Generic;

namespace Shell.SharePoint.WebParts.DREAM.ListView
{
    /// <summary>
    /// ListViewHelper class
    /// </summary>
    [Guid("43aefa45-c5f3-4978-a672-317e60282d87")]
    public class ListViewHelper : Microsoft.SharePoint.WebPartPages.WebPart
    {

        #region DECLARATION
        string strReportName = string.Empty;
        string strListName = string.Empty;
        string strPermission = string.Empty;
        ListViewerXMLGenerator objListViewer;
        protected XmlDocument xmlListDocument;
        Records objRecords;
        protected const string SORTDEFAULTORDER = "ascending";
        protected string strSiteURL = SPContext.Current.Site.Url.ToString();
        protected AbstractController objMOSSController ;
        protected ServiceProvider objFactory = new ServiceProvider();
        protected CommonUtility objCommonUtility ;
        protected bool blnInitializePageNumber ;
        protected HyperLink linkExcel = new HyperLink();
        protected HyperLink linkPrint = new HyperLink();
        protected const string TEAMREGISTRATION = "TeamRegistration";
        protected const string TEAMREGISTRATIONLIST = "Team Registration";
        protected const string USERACCESSREQUESTLIST = "User Access Request";
        protected const string NOTEAMSMESSAGE = "There are no teams to display.";
        protected string strUserName = string.Empty;
     
        #endregion


        #region PROPERTIES
        /// <summary>
        /// Gets or sets the name of the report.
        /// </summary>
        /// <value>The name of the report.</value>
        protected string ListReportName
        {
            get { return strReportName; }
            set { strReportName = value; }
        }
        /// <summary>
        /// Property to store the List Name
        /// </summary>
        /// <value>The name of the list.</value>
        protected string ListName
        {
            get
            {
                return strListName;
            }
            set
            {
                strListName = value;
            }
        }


        /// <summary>
        /// Gets or sets the permission.
        /// </summary>
        /// <value>The permission.</value>
        protected  string Permission
        {
            get
            {
                return strPermission;
            }
            set
            {
                strPermission = value;
            }
        }

        #endregion

        #region Public Methods
       
        /// <summary>
        /// Create the XML to be transformed with XSL and sets the "xmlListDocument" Global variable value
        /// </summary>
        /// <param name="dtListDetails">DataTable consists of records to be displayed</param>
        protected void GetListXml(DataTable dtListDetails)
        {
            string strCamlQuery = string.Empty;
            string strTerminated = string.Empty;
            try
            {
                if (dtListDetails != null && dtListDetails.Rows.Count > 0)
                {
                    /// Sets the properties of Records object
                    SetRecordsDataObject(dtListDetails);
                    objListViewer = new ListViewerXMLGenerator();
                    xmlListDocument = objListViewer.CreateListViewerXML(objRecords);
                }
            }
            catch
            { throw; }

        }

      
        /// <summary>
        /// This method retrieves the List Items to be rendered on the page
        /// The CAML is formed based on the CustomListType value
        /// </summary>
        /// <returns>DataTable</returns>
        protected DataTable GetRecords()
        {
            DataTable dtListDetails = null;
            string strCamlQuery = string.Empty;

            objMOSSController = objFactory.GetServiceManager("MossService");
                          
            strCamlQuery = GetsCAMLQuery();

            if (!string.IsNullOrEmpty(strCamlQuery))
            {
                dtListDetails = ((MOSSServiceManager)objMOSSController).ReadList(strSiteURL, ListName.ToString(), strCamlQuery);
            }
            return dtListDetails;
        }


        /// <summary>
        /// Deletes the record.
        /// </summary>
        /// <param name="listName">Name of the list.</param>
        /// <param name="itemId">The item id.</param>
        /// <returns></returns>
        protected void DeleteRecord(string listName,string itemId)
        {
            objMOSSController = objFactory.GetServiceManager("MossService");
            ((MOSSServiceManager)objMOSSController).DeleteListItem(listName, itemId);
            //releases the staff belonging to this team
            string strQuery = @"<Where><Eq><FieldRef Name='TeamID' /><Value Type='Text'>" + itemId + "</Value></Eq></Where>";

            DataTable dtStaff = ((MOSSServiceManager)objMOSSController).ReadList(strSiteURL, USERACCESSREQUESTLIST, strQuery);
            Dictionary<string, string> dicListValues = null;

            foreach (DataRow drStaff in dtStaff.Rows)
            {
                dicListValues = new Dictionary<string, string>();
                dicListValues.Add("ID", drStaff["ID"].ToString());
                dicListValues.Add("TeamID", "0");
                dicListValues.Add("IsTeamOwner", "No");
                ((MOSSServiceManager)objMOSSController).UpdateListItem(dicListValues, USERACCESSREQUESTLIST);
            }
        }

        /// <summary>
        /// Gets the camlQuery based on the report type
        /// </summary>
        /// <returns></returns>
        private string GetsCAMLQuery()
        {
            string strCamlQuery = string.Empty;
           
            switch (ListReportName)
            {
                case TEAMREGISTRATION:
                    {
                        strCamlQuery = @"<OrderBy><FieldRef Name='Title' /></OrderBy><Where><IsNotNull><FieldRef Name='Title' /></IsNotNull></Where>";
                        break;
                    }
                default:
                    strCamlQuery = @"<Where><IsNotNull><FieldRef Name='Title' /></IsNotNull></Where>";
                    break;

            }

            return strCamlQuery;
        }

               
        /// <summary>
        /// Sets the records data object.
        /// </summary>
        /// <param name="dtListDetails">The dt list details.</param>
        private void SetRecordsDataObject(DataTable listDetails)
        {
            objRecords = new Records();
            objRecords.Name = ListReportName;
            objRecords.RecordCollection = SetListDetail(listDetails);
        }

        /// <summary>
        /// Set the Record and Attribute properties for each record in DataTable
        /// </summary>
        /// <param name="listDetails">DataTable</param>
        /// <returns>ArrayList</returns>
        private ArrayList SetListDetail(DataTable listDetails)
        {
            DataRow objDataRow;
            Record objRecord = null;
            ArrayList arlRecord = null;
            try
            {
                arlRecord = new ArrayList();
                for (int intIndex = 0; intIndex < listDetails.Rows.Count; intIndex++)
                {
                    objDataRow = listDetails.Rows[intIndex];
                    objRecord = new Record();
                    objRecord.Order = Convert.ToString(intIndex + 1);
                    objRecord.RecordNumber = objDataRow["ID"].ToString();
                    objRecord.RecordAttributes = SetTeamRecordAttributes(objDataRow);
                    arlRecord.Add(objRecord);
                }
                return arlRecord;
            }
            catch
            {
                throw;
            }
        }



        /// <summary>
        /// Sets the team record attributes.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <returns></returns>
        private ArrayList SetTeamRecordAttributes(DataRow dataRow)
        {
            ArrayList arlAttributes = null;

            try
            {
                arlAttributes = new ArrayList();
                arlAttributes.Add(CreateAttribute("Name", Convert.ToString(dataRow["Title"]), "true"));
                arlAttributes.Add(CreateAttribute("Owner", dataRow["TeamOwner"].ToString(), "true"));
                arlAttributes.Add(CreateAttribute("Project Name (OpenWorks)", dataRow["ProjectTitle"].ToString(), "true"));
                return arlAttributes;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Creates the attribute.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="display">The display.</param>
        /// <returns></returns>
        private RecordAttribute CreateAttribute(string name, string value, string display)
        {
            int intNum;
            RecordAttribute objRecAttribute = new RecordAttribute();
            objRecAttribute.Name = name;
            objRecAttribute.Value = value;
            objRecAttribute.Display = display;
            bool blnIsNumeric = int.TryParse(value, out intNum);

            if (blnIsNumeric)
            {
                objRecAttribute.DataType = "number";
            }
            else if (string.Equals(value.GetType().Name , "String"))
            {
                objRecAttribute.DataType = "text";
            }
            return objRecAttribute;
        }





        /// <summary>
        /// Transforms the list detail.
        /// </summary>
        /// <param name="noOfRecords">The no of records.</param>
        protected void TransformListDetail(int noOfRecords)
        {
            XmlTextReader xmlTextReader = null;
            XslCompiledTransform xslTransform = null;
            XmlDocument objXmlDocForXSL = null;
            XsltArgumentList xsltArgsList = null;
            XPathDocument objXPathDocument = null;
            MemoryStream objMemoryStream = null;
            objCommonUtility = new CommonUtility();
            objMOSSController = objFactory.GetServiceManager("MossService");

            /// For Paging and Sorting
            int intrecordsPerPage = 0;
            int intPageNumber = 0;
            int intMaxPages = 5;
            int intCurrentPage = 1;
            string strSortBy = string.Empty;
            string strSortType = string.Empty;
            object objSessionUserPreference = null;

            strSortType = SORTDEFAULTORDER;

            Shell.SharePoint.DREAM.Business.Entities.UserPreferences objPreferences = null;
            try
            {

                xmlTextReader = ((MOSSServiceManager)objMOSSController).GetXSLTemplate("Dream List View", strSiteURL);
                if (xmlListDocument != null && xmlTextReader != null)
                {
                    xslTransform = new XslCompiledTransform();
                    XslCompiledTransform xslTransformTest = new XslCompiledTransform();
                    objXmlDocForXSL = new XmlDocument();
                    xsltArgsList = new XsltArgumentList();
                    objMemoryStream = new MemoryStream();
                    xmlListDocument.Save(objMemoryStream);
                    objMemoryStream.Position = 0;
                    objXPathDocument = new XPathDocument(objMemoryStream);

                    //Inititlize the XSL
                    objXmlDocForXSL.Load(xmlTextReader);
                    xslTransform.Load(objXmlDocForXSL);
                    xsltArgsList.AddParam("listType", string.Empty, ListReportName);

                    /// Parameters for Paging/Sorting
                    objSessionUserPreference = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString());
                    if (objSessionUserPreference != null)
                    {
                        objPreferences = (Shell.SharePoint.DREAM.Business.Entities.UserPreferences)objSessionUserPreference;
                        intrecordsPerPage = Convert.ToInt32(objPreferences.RecordsPerPage);                          
                    }
                    xsltArgsList.AddParam("recordsPerPage", string.Empty, intrecordsPerPage);

                    //<!-- Page Number field -->
                    if (HttpContext.Current.Request.QueryString["pageNumber"] != null)
                    {
                        intPageNumber = Int32.Parse(HttpContext.Current.Request.QueryString["pageNumber"]);
                        if (blnInitializePageNumber)
                            intPageNumber = 0;
                    }
                    if (HttpContext.Current.Request.QueryString["sortBy"] != null)
                    {
                        strSortBy = HttpContext.Current.Request.QueryString["sortBy"];
                    }
                   
                    if (HttpContext.Current.Request.QueryString["sortType"] != null)
                    {
                        strSortType = HttpContext.Current.Request.QueryString["sortType"];
                    }
                    
                    if (intPageNumber > (noOfRecords / intrecordsPerPage))
                    {
                        intPageNumber--;
                    }
                    xsltArgsList.AddParam("pageNumber", string.Empty, intPageNumber);

                    xsltArgsList.AddParam("recordCount", string.Empty, noOfRecords);
                    intMaxPages = 5;
                    xsltArgsList.AddParam("MaxPages", string.Empty, intMaxPages);
                    intCurrentPage = intPageNumber;
                    xsltArgsList.AddParam("CurrentPage", string.Empty, intPageNumber);
                    if (!string.IsNullOrEmpty(HttpContext.Current.Request.Url.Query))
                    {
                        
                        string strURL = GetCurrentPageName(true);
                        xsltArgsList.AddParam("CurrentPageName", string.Empty, strURL);// + "&");
                    }
                    else
                    {
                        xsltArgsList.AddParam("CurrentPageName", string.Empty, HttpContext.Current.Request.Url.AbsolutePath + "?");
                    }
                    xsltArgsList.AddParam("sortBy", string.Empty, strSortBy);
                    xsltArgsList.AddParam("sortType", string.Empty, strSortType);
                    // 
                    XmlNode xmlNode = xmlListDocument.SelectSingleNode("records/record/recordInfo/attribute[@name='" + strSortBy + "']");
                    if (xmlNode != null)
                    {
                        xsltArgsList.AddParam("sortDataType", string.Empty, xmlNode.Attributes["datatype"].Value);
                    }
                    if (((MOSSServiceManager)objMOSSController).IsAdmin(strSiteURL, HttpContext.Current.User.Identity.Name.ToString()))
                    {
                        xsltArgsList.AddParam("TeamPermission", string.Empty, "DreamAdmin");
                    }
                    else
                        xsltArgsList.AddParam("TeamPermission", string.Empty, Permission);
                     
                    xslTransform.Transform(objXPathDocument, xsltArgsList, HttpContext.Current.Response.Output);
                }
                else
                {
                    throw new Exception("Invalid Arguments");
                }
            }
            catch
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
        /// <summary>
        /// Renders the parent table.
        /// </summary>
        /// <param name="writer">The writer.</param>
        protected void RenderParentTable(HtmlTextWriter writer, string title)
        {
            writer.Write("<table class=\"tableAdvSrchBorder\" cellspacing=\"0\" cellpadding=\"4\" border=\"0\" width=\"100%\">");
            writer.Write("<tr><td class=\"tdAdvSrchHeader\" colspan=\"3\" valign=\"top\"><B>" + title + "</b></td></tr>");
        }



        /// <summary>
        /// Gets the name of the current page.
        /// </summary>
        /// <param name="query">if set to <c>true</c> [query].</param>
        /// <returns></returns>
        public String GetCurrentPageName(bool query)
        {
            string strFullString = string.Empty;
            string strCurrentPath = System.Web.HttpContext.Current.Request.Url.PathAndQuery;
            string[] arrTempPageName = new string[3];
            string[] arrPageName = new string[3];
            StringBuilder strResultString = new StringBuilder();
            arrTempPageName = strCurrentPath.Split('/');
            strFullString = arrTempPageName[arrTempPageName.Length - 1].ToString();
            try
            {
                if (query)
                {
                    if (strFullString.IndexOf('&') > 0)
                    {
                        arrPageName = strFullString.Split('&');
                        for (int intIndex = 0; intIndex < arrPageName.Length; intIndex++)
                        {
                            if (arrPageName[intIndex].ToLower().IndexOf("pagenumber") >= 0)
                            {
                                strResultString.Append(arrPageName[intIndex].Substring(0, arrPageName[intIndex].ToLower().IndexOf("pagenumber")));
                                break;
                            }
                            strResultString.Append(arrPageName[intIndex] + "&");
                        }
                    }
                    else
                    {
                        strResultString.Append(strFullString + "&");
                    }
                }
            }
            catch
            {
                throw;
            }
            return strResultString.ToString();
        }

        #endregion
    }

}

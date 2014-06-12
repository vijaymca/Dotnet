#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: ChapterBLL.cs
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using Shell.SharePoint.DWB.DataAccessLayer;
using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.DWB.BusinessLogicLayer
{
    /// <summary>
    /// The BLL class for the Chapter and Chapter pages functionality
    /// </summary>
    public class ChapterBLL
    {
        #region DECLARATION
        const string TEMPLATELIST = "DWB Templates";
        const string DWBUSER = "DWB User";
        const string DWBSTORYBOARD = "DWB StoryBoard";
        const string CHAPTERPAGESMAPPINGAUDITLIST = "DWB Chapter Pages Mapping Audit Trail";
        const string CHAPTERPAGESMAPPINGLIST = "DWB Chapter Pages Mapping";
        const string DWBMASTERPAGESLIST = "DWB Master Pages";
        const string DWBTEMPLATEPAGESLIST = "DWB Template Page Mapping";
        CommonDAL objCommonDAL;
        WellBookChapterDAL objWellBookChapterDAL ;

        private const string AUDIT_ACTION_CREATION = "1";

        #region DREAM 4.0 - eWB 2.0
        const string CHAPTERPREFERENCEDOCLIB = "eWB Customise Chapter";
        const string MOSSSERVICE = "MossService";
        #endregion 

        #endregion

        /// <summary>
        /// Gets the List items which can be binded to the UI controls
        /// </summary>
        /// <param name="strSiteURL">SiteURL.</param>
        /// <param name="listName">ListName.</param>
        /// <param name="strCAMLQuery">CAML Query.</param>
        /// <returns>Dictionary object.</returns>
        public Dictionary<string, string> GetListItems(string siteURL, string listName, string CAMLQuery)
        {
            Dictionary<string, string> listItemCollection = null;
            switch (listName)
            {
                case TEMPLATELIST:
                    listItemCollection = this.GetTemplatesForAsset(siteURL, listName, CAMLQuery);
                    break;
                default:
                    break;
            }

            return listItemCollection;

        }

        /// <summary>
        /// Gets the collection of the Templates based on the asset.
        /// </summary>
        /// <param name="strSiteURL">Site URL.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="strCAMLQuery">CAML Query.</param>
        /// <returns>Dictionary object.</returns>
        private Dictionary<string, string> GetTemplatesForAsset(string siteURL, string listName, string CAMLQuery)
        {
            Dictionary<string, string> listItemCollection = null;
            DataTable dtResultDatatable = null;
            CommonDAL objCommonDAL = new CommonDAL();
            dtResultDatatable = objCommonDAL.ReadList(siteURL, listName, CAMLQuery);
            if (dtResultDatatable != null && (dtResultDatatable.Rows.Count > 0))
            {
                listItemCollection = new Dictionary<string, string>();
                for (int rowIndex = 0; rowIndex < dtResultDatatable.Rows.Count; rowIndex++)
                {
                    if (!listItemCollection.ContainsKey(Convert.ToString(dtResultDatatable.Rows[rowIndex]["ID"])))
                    {
                        listItemCollection.Add(Convert.ToString(dtResultDatatable.Rows[rowIndex]["ID"]), Convert.ToString(dtResultDatatable.Rows[rowIndex]["Title"]));
                    }
                }
            }
            if (dtResultDatatable != null)
            {
                dtResultDatatable.Dispose();
            }
            return listItemCollection;

        }

        /// <summary>
        /// Updates the list entry.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listEntry">The list entry.</param>
        /// <param name="auditListName">Name of the audit list.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="actionPerformed">The action performed.</param>
        public void UpdateListEntry(string siteURL, ListEntry listEntry, string auditListName, string listName, string userName, string actionPerformed)
        {
            objWellBookChapterDAL = new WellBookChapterDAL();
            objCommonDAL = new CommonDAL();

            string strChapterID = objWellBookChapterDAL.UpdateListEntry(siteURL, listEntry, listName, auditListName, userName, actionPerformed);
            if (listEntry != null && listEntry.ChapterPagesMapping != null)
            {
                if (listEntry.ChapterPagesMapping.Count > 0 && actionPerformed.Contains(AUDIT_ACTION_CREATION))
                {
                    objWellBookChapterDAL.AddChapterMasterPageMapping(siteURL, listEntry, CHAPTERPAGESMAPPINGLIST, CHAPTERPAGESMAPPINGAUDITLIST, userName, actionPerformed);

                     string strCAMLQuery = string.Empty;
                     DataTable dtChapterPages = null;
                     DataTable dtMasterPage = null;
                     DataTable dtTemplate = null;
                     string strViewFields = string.Empty;
                     int intMasterPageID;
                     if (!string.IsNullOrEmpty(strChapterID))
                     {
                         strCAMLQuery = @"<Where><Eq><FieldRef Name='Chapter_ID'/><Value Type='Number'>" + strChapterID + "</Value></Eq></Where>";
                         dtChapterPages = objCommonDAL.ReadList(siteURL, CHAPTERPAGESMAPPINGLIST, strCAMLQuery);
                     }
                    if (dtChapterPages != null && dtChapterPages.Rows.Count > 0)
                     {
                         foreach (DataRow dtRow in dtChapterPages.Rows)
                         {
                             StoryBoard objStoryBoard = new StoryBoard();
                             objStoryBoard.PageId = Int32.Parse(dtRow["ID"].ToString());
                             objStoryBoard.PageOwner = dtRow["Owner"].ToString();
                             objStoryBoard.PageType = dtRow["Asset_Type"].ToString();
                             objStoryBoard.SOP = dtRow["Standard_Operating_Procedure"].ToString();
                             objStoryBoard.PageTitle = dtRow["Page_Actual_Name"].ToString();
                             objStoryBoard.Discipline = dtRow["Discipline"].ToString();
                             objStoryBoard.ConnectionType = dtRow["Connection_Type"].ToString();

                             intMasterPageID = Int32.Parse(dtRow["Master_Page_ID"].ToString());
                             strCAMLQuery = string.Empty;

                             strCAMLQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + intMasterPageID.ToString() + "</Value></Eq></Where>";
                             strViewFields = @"<FieldRef Name='ID'/><FieldRef Name='Master_Page_ID'/>";
                             dtTemplate = objCommonDAL.ReadList(siteURL, DWBTEMPLATEPAGESLIST, strCAMLQuery, strViewFields);
                             if (dtTemplate != null && dtTemplate.Rows.Count > 0)
                             {
                                 intMasterPageID = Int32.Parse(dtTemplate.Rows[0]["Master_Page_ID"].ToString());
                             }

                             strCAMLQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + intMasterPageID.ToString() + "</Value></Eq></Where>";
                             strViewFields = @"<FieldRef Name='ID'/><FieldRef Name='Page_Owner'/><FieldRef Name='Created'/><FieldRef Name='Title'/>";
                             dtMasterPage = objCommonDAL.ReadList(siteURL, DWBMASTERPAGESLIST, strCAMLQuery, strViewFields);

                             if (dtMasterPage != null && dtMasterPage.Rows.Count > 0)
                             {
                                 objStoryBoard.MasterPageName = dtMasterPage.Rows[0]["Title"].ToString();
                                 objStoryBoard.CreatedBy = dtMasterPage.Rows[0]["Page_Owner"].ToString();
                                 objStoryBoard.CreationDate = dtMasterPage.Rows[0]["Created"].ToString();
                             }
                             strCAMLQuery = string.Empty;
                             strCAMLQuery = @"<Where><Eq><FieldRef Name='Page_ID' /><Value Type='Number'>" + objStoryBoard.PageId.ToString() + "</Value></Eq></Where>";
                             objWellBookChapterDAL.UpdateStoryBoard(siteURL, DWBSTORYBOARD, CHAPTERPAGESMAPPINGAUDITLIST, strCAMLQuery, objStoryBoard.PageId.ToString(), objStoryBoard, actionPerformed, userName);
                         }
                     }
                     if (dtChapterPages != null)
                     {
                         dtChapterPages.Dispose();
                     }
                     if (dtMasterPage != null)
                     {
                         dtMasterPage.Dispose();
                     }
                }
            }

        }

        /// <summary>
        /// Set ChapterDetails.
        /// </summary>
        /// <param name="siteURL">Site URL.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="queryString">CAML Query.</param>
        /// <returns>ListEntry object.</returns>
        public ListEntry SetChapterDetail(string siteURL, string listName, string CAMLQuery)
        {
            objWellBookChapterDAL = new WellBookChapterDAL();
            return (objWellBookChapterDAL.GetChapterDetails(siteURL, listName, CAMLQuery));
        }

        /// <summary>
        /// Gets the active master pages for the chapters
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="chapterId">Chapter ID.</param>
        /// <param name="masterPageListName">Name of the master page list.</param>
        /// <param name="chapterpageListName">Name of the chapterpage list.</param>
        /// <param name="chapterListName">Name of the chapter list.</param>
        /// <returns>DataTable.</returns>
        public DataTable GetMasterPagesForChapter(string siteURL, string chapterId, string masterPageListName, string chapterpageListName, string chapterListName)
        {
            DataTable dtResultTable = null;
            StringBuilder strMasterPageId = new StringBuilder();
            objCommonDAL = new CommonDAL();
            string strAssetType = string.Empty;
            string strCamlQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + chapterId + "</Value></Eq></Where>";
            dtResultTable = objCommonDAL.ReadList(siteURL, chapterListName, strCamlQuery);
            if (dtResultTable == null && dtResultTable.Rows.Count < 1)
            {
                return dtResultTable;
            }
            else
            {
                strAssetType = Convert.ToString(dtResultTable.Rows[0]["Asset_Type"]);
                strCamlQuery = @"<Where><And><Eq><FieldRef Name='Chapter_ID' /><Value Type='Number'>" + chapterId + "</Value></Eq><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq></And></Where>";
                dtResultTable = objCommonDAL.ReadList(siteURL, chapterpageListName, strCamlQuery);
                if (dtResultTable == null && dtResultTable.Rows.Count < 1)
                {
                    return dtResultTable;
                }
                else
                {
                    for (int i = 0; i < dtResultTable.Rows.Count; i++)
                    {
                        strMasterPageId.Append(Convert.ToString(dtResultTable.Rows[i]["Master_Page_ID"]));
                        strMasterPageId.Append(";");
                    }

                    strCamlQuery = this.CreateCAMLQueryForNot(strMasterPageId.ToString(), "ID", "Counter");
                    strMasterPageId.Remove(0, strMasterPageId.Length);
                    strMasterPageId.Append(strCamlQuery);
                    if (string.IsNullOrEmpty(strCamlQuery))
                    {
                        strMasterPageId.Append("<Eq><FieldRef Name='Asset_Type' /><Value Type='Lookup'>" + strAssetType + "</Value></Eq>");
                        strMasterPageId.Append("<Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq></And></Where>");
                        strMasterPageId.Insert(0, "<OrderBy><FieldRef Name='Page_Sequence' Ascending='True'/></OrderBy><Where><And>");
                    }
                    else
                    {
                        strMasterPageId.Append("<Eq><FieldRef Name='Asset_Type' /><Value Type='Lookup'>" + strAssetType + "</Value></Eq></And>");
                        strMasterPageId.Append("<Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq></And></Where>");
                        strMasterPageId.Insert(0, "<OrderBy><FieldRef Name='Page_Sequence' Ascending='True'/></OrderBy><Where><And><And>");
                    }
                    dtResultTable = objCommonDAL.ReadList(siteURL, masterPageListName, strMasterPageId.ToString());
                }
            }
            return dtResultTable;
        }

        /// <summary>
        /// Get the Owners for the chapter page
        /// for the team.
        /// Retrieves the users with Discipline form DWB Team Staff list.
        /// User belong to the team which owns the book are retrieved.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="chapterId">Chapter ID.</param>
        /// <param name="chapterListName">Name of the chapter list.</param>
        /// <param name="bookListName">Name of the book list.</param>
        /// <param name="teamStaffListName">Name of the team staff list.</param>
        /// <returns>Data Table.</returns>
        public DataTable GetOwnersForChapterPage(string siteURL, string chapterId, string chapterListName, string bookListName, string teamStaffListName)
        {
            /// This method retrives the list of users with Discipline from DWB Team Staff list using below logic.
            /// Get all users with Team ID = ID of the team which owns the Book to which the pages are getting added.
            DataTable dtResultTable = null;
            string strBookId = string.Empty;
            string strTeamId = string.Empty;

            string strCamlQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + chapterId + "</Value></Eq></Where>";
            objCommonDAL = new CommonDAL();
            dtResultTable = objCommonDAL.ReadList(siteURL, chapterListName, strCamlQuery);
            if (dtResultTable == null && dtResultTable.Rows.Count < 1)
            {
                return dtResultTable;
            }
            else
            {
                strBookId = Convert.ToString(dtResultTable.Rows[0]["Book_ID"]);
                strCamlQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + strBookId + "</Value></Eq></Where>";

                dtResultTable = objCommonDAL.ReadList(siteURL, bookListName, strCamlQuery);
                if (dtResultTable == null && dtResultTable.Rows.Count < 1)
                {
                    return dtResultTable;
                }
                else
                {
                    strTeamId = Convert.ToString(dtResultTable.Rows[0]["Team_ID"]);
                    strCamlQuery = @"<Where><Eq><FieldRef Name='Team_ID' /><Value Type='Number'>" + strTeamId + "</Value></Eq></Where>";
                    dtResultTable = objCommonDAL.ReadList(siteURL, teamStaffListName, strCamlQuery);
                }
            }
            return dtResultTable;
        }

        /// <summary>
        /// Add page to the chapter
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="objListentry">ListEntry object.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="auditListName">Name of the audit list.</param>
        /// <param name="username">Windows User ID.</param>
        /// <param name="actionPerformed">Audit Action ID</param>
        public void AddPageToChapters(string siteURL, ListEntry objListentry, string listName, string auditListName, string username, string actionPerformed)
        {
            objWellBookChapterDAL = new WellBookChapterDAL();
            string strPageID = objWellBookChapterDAL.AddPageToChapter(siteURL, objListentry, listName, auditListName, username, actionPerformed);

            if (!string.IsNullOrEmpty(strPageID) && objListentry != null && objListentry.ChapterPagesMapping != null)
            {
                StoryBoard objStoryBoard = new StoryBoard();
                objStoryBoard.PageId = Int32.Parse(strPageID);
                objStoryBoard.PageOwner = objListentry.ChapterPagesMapping[0].PageOwner;
                objStoryBoard.PageType = objListentry.ChapterPagesMapping[0].AssetType;
                objStoryBoard.SOP = objListentry.ChapterPagesMapping[0].StandardOperatingProc;
                objStoryBoard.PageTitle = objListentry.ChapterPagesMapping[0].PageName;
                objStoryBoard.Discipline = objListentry.ChapterPagesMapping[0].Discipline;
                objStoryBoard.ConnectionType = objListentry.ChapterPagesMapping[0].ConnectionType;
                objStoryBoard.MasterPageName = objListentry.ChapterPagesMapping[0].PageActualName;
                objStoryBoard.CreatedBy = objListentry.ChapterPagesMapping[0].Created_By;
                objStoryBoard.CreationDate = objListentry.ChapterPagesMapping[0].Created_Date;

                string strCAMLQuery = @"<Where><Eq><FieldRef Name='Page_ID' /><Value Type='Number'>" + strPageID + "</Value></Eq></Where>";
                objWellBookChapterDAL.UpdateStoryBoard(siteURL, DWBSTORYBOARD, CHAPTERPAGESMAPPINGAUDITLIST, strCAMLQuery, strPageID, objStoryBoard, actionPerformed, username);
            } 
        }


        /// <summary>
        /// Updates the Page sequence
        /// </summary>
        /// <param name="parentSiteUrl">Site URL.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="auditListName">Name of the audit list.</param>
        /// <param name="dvUpdateListitems">DataView.</param>
        /// <param name="actionPerformed">Audit Action ID.</param>
        /// <param name="userName">Name of the user.</param>
        public void UpdateChapterSequence(string parentSiteUrl, string listName, string auditListName,
            DataView dvUpdateListitems, string actionPerformed, string userName)
        {
            objCommonDAL = new CommonDAL();

            objCommonDAL.UpdateSequence(parentSiteUrl, listName, dvUpdateListitems, "Chapter_Sequence");
            if (dvUpdateListitems != null && dvUpdateListitems.Count > 0)
            {
                for (int rowIndex = 0; rowIndex < dvUpdateListitems.Count; rowIndex++)
                {
                    objCommonDAL.UpdateListAuditHistory(parentSiteUrl, auditListName,
                        int.Parse(dvUpdateListitems[rowIndex]["ID"].ToString()), userName, actionPerformed);
                }
            }
            if (dvUpdateListitems != null)
                dvUpdateListitems.Dispose();
        }

        /// <summary>
        /// Get the Chapter Pages
        /// </summary>
        /// <param name="siteUrl">The site URL.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="CAMLQuery">The CAML query.</param>
        /// <returns>ListEntry object.</returns>
        public ListEntry GetChapterPages(string siteUrl, string listName, string CAMLQuery)
        {
            objWellBookChapterDAL = new WellBookChapterDAL();
            return objWellBookChapterDAL.SetChapterPage(siteUrl, listName, CAMLQuery);
        }

        /// <summary>
        /// Creates a CAML query from the selected ID
        /// </summary>
        /// <param name="selectedID">The selected ID.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <returns>CAMLquery</returns>
        private string CreateCAMLQueryForNot(string selectedID, string fieldName, string fieldType)
        {
            StringBuilder strCamlQueryBuilder = new StringBuilder();
            string[] strSelectedIDs = null;
            if (string.IsNullOrEmpty(selectedID))
            {
                // return if the selected ID is empty
                return string.Empty;
            }
            strSelectedIDs = selectedID.Split(';');
            if (strSelectedIDs.Length == 2)
            {
                strCamlQueryBuilder.Append(@"<Neq><FieldRef Name='" + fieldName + "' /><Value Type='" + fieldType + "'>" +
                    strSelectedIDs[0] + "</Value></Neq>");
                return strCamlQueryBuilder.ToString();
            }
            if (strSelectedIDs.Length > 2)
            {
                for (int i = 0; i < strSelectedIDs.Length - 1; i++)
                {
                    if (i != 0)
                        strCamlQueryBuilder.Insert(0, "<And>");
                    strCamlQueryBuilder.Append(@"<Neq><FieldRef Name='" + fieldName + "' /><Value Type='" + fieldType + "'>" +
                        strSelectedIDs[i] + "</Value></Neq>");
                    if (i != 0)
                        strCamlQueryBuilder.Append("</And>");
                }
            }
            return strCamlQueryBuilder.ToString();
        }

        /// <summary>
        /// Updates the Narrative field
        /// </summary>
        /// <param name="parentSiteURL">SiteURL.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="auditListName">Audit List Name.</param>
        /// <param name="camlQuery">CAML Query.</param>
        /// <param name="pageID">Page ID.</param>
        /// <param name="narrative">Narrative.</param>
        /// <param name="userName">Windows User ID.</param>
        public void UpdateNarrative(string parentSiteURL, string listName, string auditListName, string camlQuery, string pageID, string narrative, string userName)
        {

            objWellBookChapterDAL = new WellBookChapterDAL();
            objWellBookChapterDAL.UpdateNarrative(parentSiteURL, listName, auditListName, camlQuery, pageID, narrative, userName);

        }


        /// <summary>
        /// Updates the Story Board
        /// </summary>
        /// <param name="parentSiteURL">Site URL.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="auditListName">Audit List Name.</param>
        /// <param name="camlQuery">CAML Query.</param>
        /// <param name="pageID">Page ID.</param>
        /// <param name="pageStoryBoard">StoryBoard object.</param>
        /// <param name="userName">Windows User ID.</param>
        /// <param name="actionPerformed">Audit Action ID.</param>
        public void UpdateStoryBoard(string parentSiteURL, string listName, string auditListName, string camlQuery, string pageID, StoryBoard pageStoryBoard, string userName, string actionPerformed)
        {

            objWellBookChapterDAL = new WellBookChapterDAL();
            objWellBookChapterDAL.UpdateStoryBoard(parentSiteURL, listName, auditListName, camlQuery, pageID, pageStoryBoard, actionPerformed, userName);


        }

        /// <summary>
        /// Gets the asset value for the asset type
        /// </summary>
        /// <param name="requestInfo">The request info.</param>
        /// <param name="assetType">Type of the asset.</param>
        /// <returns></returns>
        public XmlDocument GetAssetValueForAssetType(RequestInfo requestInfo, string assetType)
        {

            ServiceProvider objFactory = new ServiceProvider();
            XmlDocument responseXmlDoc = null;
            AbstractController objAbstractController = null;

            objAbstractController = objFactory.GetServiceManager("ReportService");
            responseXmlDoc = objAbstractController.GetSearchResults(requestInfo, -1, assetType, null, 0);

            return responseXmlDoc;
        }
       
        /// <summary>
        /// Gets the teams for the Users
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public DataTable GetAssetTeams(string siteURL, string username)
        {
            string strViewFields = "<FieldRef Name='Team' />";
            string strCamlQuery = @"<Where><Eq><FieldRef Name='Windows_User_ID' /> <Value Type='Text'>" + username + " </Value></Eq></Where>";
            objCommonDAL = new CommonDAL();
            return objCommonDAL.ReadList(siteURL, DWBUSER, strCamlQuery, strViewFields);

        }

        #region DREAM 4.0 - eWB 2.0 - Customise Chapters

        /// <summary>
        /// Saves the reorder XML to document library.
        /// </summary>
        /// <param name="chapterPreference">The chapter preference.</param>
        /// <param name="bookId">The book id.</param>
        /// <returns>XmlDocument</returns>
        public XmlDocument SaveReorderXml(string chapterPreference, string bookId)
        {
            XmlDocument userReorderXml = null;


            CommonUtility objCommonUtility = new CommonUtility();
            AbstractController objMOSSController;
            ServiceProvider objFactory;

            objFactory = new ServiceProvider();
            objMOSSController = objFactory.GetServiceManager(MOSSSERVICE);

            XmlDocument chapterPreferenceXml = new XmlDocument();
            chapterPreferenceXml.LoadXml(chapterPreference);
          
            string strUserId = objCommonUtility.GetUserName();
            /// If the file for the logged in user is available in doc lib, load to XmlDocument
            if (((MOSSServiceManager)objMOSSController).IsDocLibFileExist(CHAPTERPREFERENCEDOCLIB, strUserId))
            {
                userReorderXml = ((MOSSServiceManager)objMOSSController).GetDocLibXMLFile(CHAPTERPREFERENCEDOCLIB, strUserId);
            }
            else
            { /// Else create new XmlDocument
                userReorderXml = new XmlDocument();
                XmlElement rootElement = userReorderXml.CreateElement("Books");
                userReorderXml.AppendChild(rootElement);
            }

            /// If the preference Xml is available in document read into XmlNode
            /// Else create a new node, add all details and add the node to XmlDocument.
            XmlNode bookNode = userReorderXml.SelectSingleNode("Books/BookInfo[@BookID='" + bookId + "']");
            if (bookNode == null)
            {
                XmlNode ndSearchType = userReorderXml.CreateElement("BookInfo");
                XmlAttribute atBookId = userReorderXml.CreateAttribute("BookID");
                atBookId.Value = bookId;
                ndSearchType.Attributes.Append(atBookId);
                XmlAttribute atBookName = userReorderXml.CreateAttribute("BookName");
                atBookName.Value = chapterPreferenceXml.SelectSingleNode("BookInfo").Attributes["BookName"].Value;
                ndSearchType.Attributes.Append(atBookName);
                userReorderXml.SelectSingleNode("Books").AppendChild(ndSearchType);
                ndSearchType.InnerXml = chapterPreferenceXml.SelectSingleNode("BookInfo").InnerXml;

            }
            else
            {/// If the preference Xml is available in document modify only the inner xml of the BookInfo node.
                bookNode.InnerXml = chapterPreferenceXml.SelectSingleNode("BookInfo").InnerXml;
            }
            ((MOSSServiceManager)objMOSSController).UploadToDocumentLib(CHAPTERPREFERENCEDOCLIB, strUserId, userReorderXml);
            return userReorderXml;
        }


        #endregion 

    }
}

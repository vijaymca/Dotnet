#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: WellBookChapterDAL.cs
#endregion
using System;
using System.Data;
using System.Text;
using Shell.SharePoint.DWB.Business.DataObjects;
using Microsoft.SharePoint;

namespace Shell.SharePoint.DWB.DataAccessLayer
{
    /// <summary>
    /// Data access layer class for Well Book Chapter
    /// Get/Set the Well Book Chapter details from/to SharePoint list
    /// </summary>
    public class WellBookChapterDAL
    {
        CommonDAL objCommonDAL;
        private const string AUDIT_ACTION_COMMENT_ADDED = "10";
        private const string AUDIT_ACTION_STORYBOARD_UPDATED = "11";
        private const string AUDIT_ACTION_CREATION = "1";
        private const string AUDIT_ACTION_UPDATION = "2";
        private const string AUDIT_ACTION_ACTIVATE = "3";
        private const string AUDIT_ACTION_TERMINATE = "4";

        private const string DWBBOOKLIST = "DWB Books";
        private const string DWBTEAMSTAFFLIST = "DWB Team Staff";
        const string DWBUSERLIST = "DWB User";
        /// <summary>
        /// Updates the list entry.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listEntry">The list entry.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="auditListName">Audit List Name.</param>
        /// <param name="userName">User Name.</param>
        /// <param name="actionPerformed">Audit Action.</param>
        /// <returns>ID of the Chapter Created/Updated.</returns>
        /// <exception cref="">Handled in calling method.</exception>
        internal string UpdateListEntry(string siteURL, ListEntry listEntry, string listName, string auditListName, string userName, string actionPerformed)
        {

            SPList list;
            SPQuery query;
            SPListItem objListItem;
            SPListItemCollection objListItemCollection;
            SPFieldLookupValue lookupField;
            int intPageSequence = 10;
            int intListItemId = 0;
            string strListGuid = string.Empty;
            StringBuilder sbMethodBuilder = new StringBuilder();

            string strChapterID = string.Empty;
            string strBatch = string.Empty;
            string strBatchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
  "<ows:Batch OnError=\"Return\">{0}</ows:Batch>";

            string strMethodFormat = "<Method ID=\"{0}\">" +
             "<SetList>{1}</SetList>" +
             "<SetVar Name=\"Cmd\">Save</SetVar>" +
             "<SetVar Name=\"ID\">{2}</SetVar>" +
             "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Chapter_Sequence\">{3}</SetVar>" +
            "</Method>";

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        list = web.Lists[listName];

                        query = new SPQuery();
                        strListGuid = list.ID.ToString();
                        objListItem = list.Items.Add();
                        if (string.Equals(actionPerformed, AUDIT_ACTION_UPDATION))
                        {
                            objListItem = list.GetItemById(listEntry.ChapterDetails.RowID);
                        }
                        objListItem["Title"] = listEntry.ChapterDetails.ChapterTitle;
                        objListItem["Asset_Value"] = listEntry.ChapterDetails.AssetValue;
                        objListItem["Actual_Asset_Value"] = listEntry.ChapterDetails.ActualAssetValue;
                        objListItem["Country"] = listEntry.ChapterDetails.Country;
                        objListItem["Criteria"] = listEntry.ChapterDetails.Criteria;
                        objListItem["Column_Name"] = listEntry.ChapterDetails.ColumnName;
                        objListItem["Book_ID"] = listEntry.ChapterDetails.BookID;
                        if (!string.IsNullOrEmpty(listEntry.ChapterDetails.AssetType))
                        {
                            lookupField = new SPFieldLookupValue(Convert.ToInt32(listEntry.ChapterDetails.AssetType), "");
                            objListItem["Asset_Type"] = lookupField;
                        }
                       
                        objListItem["Chapter_Description"] = listEntry.ChapterDetails.ChapterDescription;
                        objListItem["Template_ID"] = listEntry.ChapterDetails.TemplateID;

                        if (!string.IsNullOrEmpty(listEntry.ChapterDetails.Terminated))
                        {
                            objListItem["Terminate_Status"] = listEntry.ChapterDetails.Terminated;
                        }

                        objListItem.Update();
                        strChapterID = objListItem["ID"].ToString();
                        listEntry.ChapterDetails.RowID = int.Parse(objListItem["ID"].ToString());
                        string strNoOfActiveChapters = string.Empty;
                        if (string.Equals(actionPerformed, AUDIT_ACTION_CREATION))
                        {
                            query.Query = @"<OrderBy><FieldRef Name='Chapter_Sequence' /></OrderBy><Where><And><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq><Eq><FieldRef Name='Book_ID' /><Value Type='Number'>" + listEntry.ChapterDetails.BookID.ToString() + "</Value></Eq></And></Where>";
                            objListItemCollection = list.GetItems(query);
                                                   
                            if (objListItemCollection != null && objListItemCollection.Count > 0)
                            {
                                strNoOfActiveChapters = objListItemCollection.Count.ToString();

                                for (int intIndex = 0; intIndex < objListItemCollection.Count; intIndex++)
                                {
                                    int.TryParse(Convert.ToString(objListItemCollection[intIndex]["ID"]), out intListItemId);
                                    if (intListItemId != listEntry.ChapterDetails.RowID)
                                    {
                                        intPageSequence = intPageSequence + 10;
                                        sbMethodBuilder.AppendFormat(strMethodFormat, intListItemId, strListGuid, intListItemId, intPageSequence);
                                    }
                                    else
                                    {
                                        sbMethodBuilder.AppendFormat(strMethodFormat, intListItemId, strListGuid, intListItemId, 10);

                                    }
                                }
                                strBatch = string.Format(strBatchFormat, sbMethodBuilder.ToString());
                                web.ProcessBatchData(strBatch);                                                                
                            }
                            objCommonDAL = new CommonDAL();
                            /// Update the DWB Books list with no of active chapters
                            objCommonDAL.UpdateNoOfActiveChapters(siteURL, DWBBOOKLIST, listEntry.ChapterDetails.BookID.ToString(), strNoOfActiveChapters.ToString());
                        }

                        web.AllowUnsafeUpdates = false;
                        objCommonDAL = new CommonDAL();
                       
                        objCommonDAL.UpdateListAuditHistory(siteURL, auditListName, listEntry.ChapterDetails.RowID, userName, actionPerformed);


                    }
                }
            });
            return strChapterID;
        }

        /// <summary>
        /// Updates the list entry.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listEntry">The list entry.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="auditListName">Audit List Name.</param>
        /// <param name="userName">User Name.</param>
        /// <param name="actionPerformed">Audit Action.</param>
        /// <returns>ID of the Chapter Page Created.</returns>
        /// <exception cref="">Handled in calling method.</exception>
        internal string AddChapterMasterPageMapping(string siteURL, ListEntry listEntry, string listName, string auditListtname, string username, string actionPerformed)
        {

            SPList list;
            SPListItem objListItem;
            int intRowId = 0;
            StringBuilder strTemplatePageMappingRowId = new StringBuilder();
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        list = web.Lists[listName];
                        objCommonDAL = new CommonDAL();
                        
                        string strCAMLQuery = string.Empty; 
                        string strViewFields = string.Empty; 
                        string strBookTeamID = string.Empty;
                        string strPageOwner = string.Empty;
                        string strUserID = string.Empty;
                        DataTable dtTeamStaff = null;
                        DataTable dtBook = null;
                        DataTable dtUser = null;
                        if (listEntry != null)
                        {
                            /// Get the Team of the book
                            if (listEntry.ChapterDetails != null)
                            {
                                strCAMLQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + listEntry.ChapterDetails.BookID.ToString() + "</Value></Eq></Where>";
                                strViewFields =@"<FieldRef Name='ID' /><FieldRef Name='Team' /><FieldRef Name='Team_ID' />";
                                dtBook = objCommonDAL.ReadList(siteURL, DWBBOOKLIST, strCAMLQuery, strViewFields);
                                if (dtBook != null && dtBook.Rows.Count > 0)
                                {
                                    strBookTeamID = Convert.ToString(dtBook.Rows[0]["Team_ID"]);
                                }
                            }
                            
                            for (int i = 0; i < listEntry.ChapterPagesMapping.Count; i++)
                            {
                                strPageOwner = string.Empty;
                                objListItem = list.Items.Add();
                                objListItem["Master_Page_ID"] = listEntry.ChapterPagesMapping[i].MasterPageID;
                                objListItem["Page_Actual_Name"] = listEntry.ChapterPagesMapping[i].PageActualName;
                                objListItem["Page_Name"] = listEntry.ChapterPagesMapping[i].PageName;

                                objListItem["Discipline"] = listEntry.ChapterPagesMapping[i].Discipline;
                                objListItem["Chapter_ID"] = listEntry.ChapterDetails.RowID;
                                objListItem["Asset_Type"] = listEntry.ChapterPagesMapping[i].AssetType;
                                if (!string.IsNullOrEmpty(listEntry.ChapterPagesMapping[i].Empty))
                                    objListItem["Empty"] = listEntry.ChapterPagesMapping[i].Empty;
                                objListItem["Page_Sequence"] = listEntry.ChapterPagesMapping[i].PageSequence;
                                if (!string.IsNullOrEmpty(listEntry.ChapterPagesMapping[i].SignOffStatus))
                                {
                                    objListItem["Sign_Off_Status"] = listEntry.ChapterPagesMapping[i].SignOffStatus;
                                }
                                objListItem["Standard_Operating_Procedure"] = listEntry.ChapterPagesMapping[i].StandardOperatingProc;
                                objListItem["Connection_Type"] = listEntry.ChapterPagesMapping[i].ConnectionType;
                                if (!string.IsNullOrEmpty(listEntry.ChapterPagesMapping[i].PageURL))
                                {
                                    objListItem["Page_URL"] = listEntry.ChapterPagesMapping[i].PageURL;
                                }                                                           
                                strCAMLQuery = string.Empty;
                                strViewFields = string.Empty;
                                /// Retrieve the User with Rank =1 for the selected Team(Book) and Discipline(MasterPage) , "DWB Team Staff" and  assign to "Owner" column 
                                if (!string.IsNullOrEmpty(strBookTeamID) && !string.IsNullOrEmpty(listEntry.ChapterPagesMapping[i].Discipline))
                                {
                                    strCAMLQuery = @"<OrderBy><FieldRef Name='User_Rank' Ascending='TRUE' /></OrderBy><Where><And><And><Eq><FieldRef Name='Team_ID' /><Value Type='Number'>" + strBookTeamID + "</Value></Eq><Eq><FieldRef Name='Discipline' /><Value Type='Text'>" + listEntry.ChapterPagesMapping[i].Discipline + "</Value></Eq></And><Eq><FieldRef Name='User_Rank' /><Value Type='Number'>" + "1" + "</Value></Eq></And></Where>";
                                    strViewFields = @"<FieldRef Name='ID' /><FieldRef Name='Team_ID' /><FieldRef Name='Discipline' /><FieldRef Name='User_Rank' /><FieldRef Name='User_ID' /><FieldRef Name='Title' />";
                                    dtTeamStaff = objCommonDAL.ReadList(siteURL, DWBTEAMSTAFFLIST, strCAMLQuery, strViewFields);
                                }
                                if (dtTeamStaff != null && dtTeamStaff.Rows.Count > 0)
                                {
                                    strUserID = Convert.ToString(dtTeamStaff.Rows[0]["User_ID"]);
                                    strCAMLQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + strUserID + "</Value></Eq></Where>";
                                    strViewFields = @"<FieldRef Name='ID' /><FieldRef Name='Windows_User_ID' />";
                                    dtUser = objCommonDAL.ReadList(siteURL, DWBUSERLIST, strCAMLQuery, strViewFields);
                                    if (dtUser != null && dtUser.Rows.Count > 0)
                                    {
                                        strPageOwner = dtUser.Rows[0]["Windows_User_ID"].ToString();
                                    }
                                }
                                if (!string.IsNullOrEmpty(strPageOwner))
                                {
                                    objListItem["Owner"] = strPageOwner;
                                }
                                else /// If no user with Rank =1 or no user for the selected discipline available, assign the Master Page Owner to "Owner" column
                                {
                                    objListItem["Owner"] = listEntry.ChapterPagesMapping[i].PageOwner;
                                }
                                objListItem.Update();
                                int.TryParse(Convert.ToString(objListItem["ID"]), out intRowId);
                                listEntry.ChapterPagesMapping[i].RowId = intRowId;
                                objCommonDAL.UpdateListAuditHistory(siteURL, auditListtname, intRowId, username, actionPerformed);

                            }
                        }

                        web.AllowUnsafeUpdates = false;
                        if (dtBook != null)
                        {
                            dtBook.Dispose();
                        }
                        if (dtTeamStaff != null)
                        {
                            dtTeamStaff.Dispose();
                        }
                        if (dtUser != null)
                        {
                            dtUser.Dispose();
                        }
                    }                    
                }
            });
            
            return strTemplatePageMappingRowId.ToString();
        }

        /// <summary>
        /// Adds the Pages to the Chapter Directly.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listEntry">The list entry.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="auditListName">Audit List Name.</param>
        /// <param name="userName">User Name.</param>
        /// <param name="actionPerformed">Audit Action.</param>
        /// <returns>ID of the Chapter Page Created.</returns>
        /// <exception cref="">Handled in calling method.</exception>
        internal string AddPageToChapter(string siteURL, ListEntry listEntry, string listName, string auditListtname, string username, string actionPerformed)
        {
            string strPageID = string.Empty;
            SPList list;
            SPListItem objListItem;
            int intRowId = 0;
            int intPageSequence = 0;
            DataTable dtlistItem = null;
            int intMappingRowId = 0;
            DataView dtDataView = null;
            SPQuery query;
            string strCamlQuery = string.Empty;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteURL))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;
                            list = web.Lists[listName];
                            objCommonDAL = new CommonDAL();
                            for (int i = 0; i < listEntry.ChapterPagesMapping.Count; i++)
                            {
                                objListItem = list.Items.Add();
                                objListItem["Master_Page_ID"] = listEntry.ChapterPagesMapping[i].MasterPageID;
                                objListItem["Page_Actual_Name"] = listEntry.ChapterPagesMapping[i].PageActualName;
                                objListItem["Page_Name"] = listEntry.ChapterPagesMapping[i].PageName;
                                objListItem["Owner"] = listEntry.ChapterPagesMapping[i].PageOwner;
                                objListItem["Discipline"] = listEntry.ChapterPagesMapping[i].Discipline;
                                objListItem["Chapter_ID"] = listEntry.ChapterDetails.RowID;
                                objListItem["Asset_Type"] = listEntry.ChapterPagesMapping[i].AssetType;
                                if (!string.IsNullOrEmpty(listEntry.ChapterPagesMapping[i].Empty))
                                    objListItem["Empty"] = listEntry.ChapterPagesMapping[i].Empty;
                                objListItem["Page_Sequence"] = listEntry.ChapterPagesMapping[i].PageSequence;
                                if (!string.IsNullOrEmpty(listEntry.ChapterPagesMapping[i].SignOffStatus))
                                {
                                    objListItem["Sign_Off_Status"] = listEntry.ChapterPagesMapping[i].SignOffStatus;
                                }
                                objListItem["Connection_Type"] = listEntry.ChapterPagesMapping[i].ConnectionType;
                                objListItem["Standard_Operating_Procedure"] = listEntry.ChapterPagesMapping[i].StandardOperatingProc;
                                if (!string.IsNullOrEmpty(listEntry.ChapterPagesMapping[i].PageURL))
                                {
                                    objListItem["Page_URL"] = listEntry.ChapterPagesMapping[i].PageURL;
                                }
                                objListItem.Update();
                                /// Assing the Item ID to return 
                                strPageID = Convert.ToString(objListItem["ID"]);

                                int.TryParse(Convert.ToString(objListItem["ID"]), out intRowId);
                                listEntry.ChapterPagesMapping[i].RowId = intRowId;
                                strCamlQuery = @"<Where><Eq><FieldRef Name='Chapter_ID' /><Value Type='Number'>" + listEntry.ChapterDetails.RowID + "</Value></Eq></Where>";
                                query = new SPQuery();
                                query.Query = strCamlQuery;
                                query.ViewFields = @"<FieldRef Name='Chapter_ID' /><FieldRef Name='ID' /><FieldRef Name='Page_Sequence' />";
                                dtlistItem = list.GetItems(query).GetDataTable();
                                if (dtlistItem != null && dtlistItem.Rows.Count > 0)
                                {
                                    dtDataView = dtlistItem.DefaultView;
                                    dtDataView.Sort = "Page_Sequence asc";
                                    for (int j = 0; j < dtDataView.Count; j++)
                                    {
                                        intPageSequence = intPageSequence + 10;
                                        intMappingRowId = (int)dtDataView[j]["ID"];
                                        if (intMappingRowId != intRowId)
                                        {
                                            objListItem = list.GetItemById(intMappingRowId);
                                            objListItem["Page_Sequence"] = intPageSequence;

                                            objListItem.Update();
                                        }
                                    }
                                }
                                objCommonDAL.UpdateListAuditHistory(siteURL, auditListtname, intRowId, username, actionPerformed);

                            }

                            web.AllowUnsafeUpdates = false;
                        }



                    }
                });
            }
            finally
            {
                if (dtlistItem != null) dtlistItem.Dispose();
            }
            return strPageID;
        }

        /// <summary>
        /// Gets the Chapter Details.
        /// </summary>
        /// <param name="parentSiteUrl">Site URL.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="queryString">CAML Query.</param>
        /// <returns>ListEntry Object.</returns>
        /// <exception cref="">Handled in calling method.</exception>
        internal ListEntry GetChapterDetails(string siteURL, string listName, string queryString)
        {
            ListEntry objListEntry = null;
            DataTable objListItems = null;
            DataRow objListRow;
            int intTemplateId = 0;
            int intBookId = 0;
            ChapterDetails objChapterDetails = null;
            try
            {
                objCommonDAL = new CommonDAL();
                objListItems = objCommonDAL.ReadList(siteURL, listName, queryString);

                if (objListItems != null)
                {
                    objListEntry = new ListEntry();
                    objChapterDetails = new ChapterDetails();
                    for (int index = 0; index < objListItems.Rows.Count; index++)
                    {
                        objListRow = objListItems.Rows[index];
                        objChapterDetails.ChapterTitle = objListRow["Title"].ToString();
                        objChapterDetails.AssetValue = Convert.ToString(objListRow["Asset_Value"]);
                        objChapterDetails.Country = Convert.ToString(objListRow["Country"]);
                        objChapterDetails.Criteria = Convert.ToString(objListRow["Criteria"]);
                        objChapterDetails.ColumnName = Convert.ToString(objListRow["Column_Name"]);
                        if (objListRow["Chapter_Sequence"] != DBNull.Value)
                        {
                            objChapterDetails.ChapterSequence = Convert.ToInt32(objListRow["Chapter_Sequence"]);
                        }
                        int.TryParse(Convert.ToString(objListRow["Book_ID"]), out intBookId);
                        objChapterDetails.BookID = intBookId;
                        objChapterDetails.AssetType = Convert.ToString(objListRow["Asset_Type"]);
                        objChapterDetails.ChapterDescription = Convert.ToString(objListRow["Chapter_Description"]);                     
                        int.TryParse(Convert.ToString(objListRow["Template_ID"]), out intTemplateId);
                        objChapterDetails.TemplateID = intTemplateId;
                        objChapterDetails.Terminated = Convert.ToString(objListRow["Terminate_Status"]);

                        objListEntry.ChapterDetails = objChapterDetails;
                    }
                }
                return objListEntry;
            }
            finally
            {
                if (objListItems != null)
                    objListItems.Dispose();
            }
        }

        /// <summary>
        /// Gets the Master Page Information from ChapterPage mapping List based on the CAML Query.
        /// </summary>
        /// <param name="parentSiteUrl">Site URL.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="queryString">CAML Query.</param>
        /// <returns>ListEntry object.</returns>
        /// <exception cref="">Handled in calling method.</exception>
        internal ListEntry SetChapterPage(string siteURL, string listName, string queryString)
        {
            ListEntry objListEntry = null;
            DataTable objListItems = null;
            DataRow objListRow;
            int intRowId = 0;
            MasterPageDetails objMasterpage = null;
            ChapterDetails objChapterDetails = null;
            try
            {
                objCommonDAL = new CommonDAL();
                objListItems = objCommonDAL.ReadList(siteURL, listName, queryString);

                if (objListItems != null)
                {
                    objListEntry = new ListEntry();
                    objMasterpage = new MasterPageDetails();
                    for (int index = 0; index < objListItems.Rows.Count; index++)
                    {
                        objListRow = objListItems.Rows[index];
                        objMasterpage.Name = Convert.ToString(objListRow["Page_Name"]);
                        objMasterpage.TemplateTitle = Convert.ToString(objListRow["Page_Actual_Name"]);
                        if (objListRow["Page_Sequence"] != DBNull.Value)
                        {
                            objMasterpage.PageSequence = Convert.ToInt32(objListRow["Page_Sequence"]);
                        }
                        objMasterpage.SOP = Convert.ToString(objListRow["Standard_Operating_Procedure"]);
                        objMasterpage.AssetType = Convert.ToString(objListRow["Asset_Type"]);
                        objMasterpage.ConnectionType = Convert.ToString(objListRow["Connection_Type"]);
                        objMasterpage.SignOffDiscipline = Convert.ToString(objListRow["Discipline"]);
                        objChapterDetails = new ChapterDetails();
                        int.TryParse(Convert.ToString(objListRow["Chapter_ID"]), out  intRowId);
                        objChapterDetails.RowID = intRowId;
                        int.TryParse(Convert.ToString(objListRow["ID"]), out  intRowId);
                        objMasterpage.RowId = intRowId;
                        objListEntry.ChapterDetails = objChapterDetails;
                        objListEntry.MasterPage = objMasterpage;
                    }
                }
                return objListEntry;
            }
            finally
            {
                if (objListItems != null)
                    objListItems.Dispose();
            }
        }

        /// <summary>
        /// Updates the Narrative for Book Page.
        /// </summary>
        /// <param name="parentSiteUrl">Site URL.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="auditListName"></param>
        /// <param name="camlQuery">CAML Query.</param>
        /// <param name="pageID">Page ID.</param>
        /// <param name="narrative">Narrative.</param>
        /// <param name="userName">User Name.</param>
        /// <exception cref="">Handled in calling method.</exception>
        internal void UpdateNarrative(string siteURL, string listName, string auditListName, string camlQuery, string pageID, string narrative, string userName)
        {
            SPList list;
            SPQuery query;
            SPListItemCollection objListItems;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        list = web.Lists[listName];
                        query = new SPQuery();
                        query.Query = camlQuery;
                        objListItems = list.GetItems(query);

                        if (objListItems.Count > 1)
                        {
                            foreach (SPListItem objListItem in objListItems)
                            {
                                objListItem["Narrative"] = narrative;
                                objListItem["Page_ID"] = Convert.ToInt32(pageID);
                                objListItem.Update();
                            }
                        }
                        else
                        {
                            SPListItem objListItem = list.Items.Add();
                            objListItem["Narrative"] = narrative;
                            objListItem["Page_ID"] = Convert.ToInt32(pageID);
                            objListItem.Update();
                        }

                        objCommonDAL = new CommonDAL();
                        objCommonDAL.UpdateListAuditHistory(siteURL, auditListName, Convert.ToInt32(pageID), userName, "9");
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });
        }


        /// <summary>
        /// Updates the Story board.
        /// </summary>
        /// <param name="parentSiteURL">The site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="auditListName">Audit List Name.</param>
        /// <param name="camlQuery">CAML Query.</param>
        /// <param name="pageID">Page ID.</param>
        /// <param name="pageStoryBoard">StoryBoard object.</param>
        /// <param name="actionPerformed">Audit action.</param>
        /// <param name="userName">User Name.</param>
        /// <exception cref="">Handled in calling method.</exception>
        internal void UpdateStoryBoard(string siteURL, string listName, string auditListName, string camlQuery, string pageID, StoryBoard pageStoryBoard, string actionPerformed, string userName)
        {

            SPList list;
            SPListItem objListItem;
            SPQuery spQuery = null;
            SPListItemCollection objListItemCollection = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        list = web.Lists[listName];
                        spQuery = new SPQuery();
                        spQuery.Query = camlQuery;

                        objListItemCollection = list.GetItems(spQuery);
                        if (objListItemCollection.Count > 0)
                        {
                            objListItem = objListItemCollection[0];
                        }
                        else
                        {
                            objListItem = list.Items.Add();
                        }
                        if (pageStoryBoard.PageId > 0)
                            objListItem["Page_ID"] = pageStoryBoard.PageId;
                        if (!string.IsNullOrEmpty(pageStoryBoard.PageTitle))
                            objListItem["Page_Title"] = pageStoryBoard.PageTitle;
                        if (!string.IsNullOrEmpty(pageStoryBoard.ConnectionType))
                            objListItem["Connection_Type"] = pageStoryBoard.ConnectionType;
                        if (!string.IsNullOrEmpty(pageStoryBoard.Source))
                            objListItem["Source"] = pageStoryBoard.Source;
                        if (!string.IsNullOrEmpty(pageStoryBoard.Discipline))
                            objListItem["Discipline"] = pageStoryBoard.Discipline;
                        if (!string.IsNullOrEmpty(pageStoryBoard.MasterPageName))
                            objListItem["Master_Page"] = pageStoryBoard.MasterPageName;
                        if (!string.IsNullOrEmpty(pageStoryBoard.ApplicationTemplate))
                            objListItem["Application_Template"] = pageStoryBoard.ApplicationTemplate;
                        if (!string.IsNullOrEmpty(pageStoryBoard.ApplicationPage))
                            objListItem["Application_Page"] = pageStoryBoard.ApplicationPage;
                        if (!string.IsNullOrEmpty(pageStoryBoard.SOP))
                            objListItem["SOP"] = pageStoryBoard.SOP;
                        if (!string.IsNullOrEmpty(pageStoryBoard.CreatedBy))
                            objListItem["Created_By"] = pageStoryBoard.CreatedBy;
                        if (!string.IsNullOrEmpty(pageStoryBoard.CreationDate))
                            objListItem["Creation_Date"] = Convert.ToDateTime(pageStoryBoard.CreationDate).ToString("yyyy-MM-ddTHH:mm:ssZ");
                        if (!string.IsNullOrEmpty(pageStoryBoard.PageOwner))
                            objListItem["Page_Owner"] = pageStoryBoard.PageOwner;
                        if (!string.IsNullOrEmpty(pageStoryBoard.PageType))
                            objListItem["Page_Type"] = pageStoryBoard.PageType;
                        objListItem.Update();
                        web.AllowUnsafeUpdates = false;
                        if (string.Compare(actionPerformed, AUDIT_ACTION_STORYBOARD_UPDATED, true) == 0)
                        {
                            objCommonDAL = new CommonDAL();
                            objCommonDAL.UpdateListAuditHistory(siteURL, auditListName, Convert.ToInt32(pageID), userName, actionPerformed);
                        }
                    }
                }
            });

        }
       
        /// <summary>
        /// Updates comments for Well Book Page.
        /// </summary>
        /// <param name="parentSiteURL">The site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="auditListName">Audit List Name.</param>
        /// <param name="listEntry">ListEntry object contains the Comments.</param>
        /// <param name="actionPerformed">Audit action.</param>
        /// <returns>True/False</returns>
        /// <exception cref="">Handled in calling method.</exception>
        internal bool UpdateComments(string siteURL, string listName, string auditListName, ListEntry listEntry, string actionPerformed)
        {
              SPList list;
            SPListItem objListItem;
            SPFieldLookupValue lookupField;
            bool blnUpdateSuccess = false;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        list = web.Lists[listName];
                        if (string.Compare(AUDIT_ACTION_COMMENT_ADDED, actionPerformed, true) == 0)
                        {
                            if (listEntry != null && listEntry.PageComments != null)
                            {
                                objListItem = list.Items.Add();
                                objListItem["Page_ID"] = Convert.ToInt32(listEntry.PageComments.PageID);
                                objListItem["UserName"] = listEntry.PageComments.UserName;
                                objListItem["Comment"] = listEntry.PageComments.Comments;
                                /// Lookup column
                                if (!string.IsNullOrEmpty(listEntry.PageComments.DisciplineID))
                                {
                                    lookupField = new SPFieldLookupValue(listEntry.PageComments.DisciplineID);
                                    objListItem["Discipline"] = lookupField;
                                }                                
                                objListItem["Shared"] = listEntry.PageComments.ShareComments;
                                objListItem.Update();

                                objCommonDAL = new CommonDAL();
                                objCommonDAL.UpdateListAuditHistory(siteURL, auditListName, Int32.Parse(listEntry.PageComments.PageID), listEntry.PageComments.UserName, actionPerformed);
                                blnUpdateSuccess = true;
                            }
                        }
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });
           return blnUpdateSuccess;
        }
    }
}

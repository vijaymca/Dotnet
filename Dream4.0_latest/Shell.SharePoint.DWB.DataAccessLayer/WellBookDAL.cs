#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: WellBookDAL.cs
#endregion
using System;
using System.Collections.Generic;
using System.Data;
using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DREAM.Utilities;
using Microsoft.SharePoint;
using System.Text;

namespace Shell.SharePoint.DWB.DataAccessLayer
{
    /// <summary>
    /// Data access layer class for Well Book 
    /// Get/Set the Well Book details from/to SharePoint list
    /// </summary>
    public class WellBookDAL
    {
        CommonDAL objCommonDAL;
        const string AUDIT_ACTION_UPDATION = "2";
        const string AUDIT_ACTION_BOOK_TITLE_UPDATED = "12";
        const string DWBSTORYBOARD = "DWB StoryBoard";
        const string CHAPTERPAGESMAPPINGAUDITLIST = "DWB Chapter Pages Mapping Audit Trail";
        const string DATEFORMAT = "Date Format";
        const string CHAPTERPAGESMAPPINGLIST = "DWB Chapter Pages Mapping";
        #region Methods

        /// <summary>
        /// Updates the list entry.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listEntry">The list entry.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="auditListName">Audit List Name.</param>
        /// <param name="userName">User Name.</param>
        /// <param name="actionPerformed">Audit Action.</param>
        /// <exception cref="">Handled in calling method.</exception>
        internal void UpdateListEntry(string siteURL, ListEntry listEntry, string listName, string auditListName, string userName, string actionPerformed)
        {
            SPList list;
            SPQuery query;
            SPListItem objListItem;
            SPFieldLookupValue lookupField;
            int intRowId = 0;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        list = web.Lists[listName];
                        query = new SPQuery();
                        objListItem = list.Items.Add();
                        if (string.Compare(actionPerformed, AUDIT_ACTION_BOOK_TITLE_UPDATED, true) == 0 || string.Compare(actionPerformed, AUDIT_ACTION_UPDATION, true) == 0)
                        {
                            objListItem = list.GetItemById(listEntry.WellBookDetails.RowId);
                        }
                        objListItem["Title"] = listEntry.WellBookDetails.Title;
                        if (!string.IsNullOrEmpty(listEntry.WellBookDetails.TeamID))
                        {
                            lookupField = new SPFieldLookupValue(listEntry.WellBookDetails.
                                TeamID);
                            objListItem["Team"] = lookupField;
                            int.TryParse(listEntry.WellBookDetails.TeamID, out intRowId);
                            objListItem["Team_ID"] = intRowId;
                        }

                        if (!string.IsNullOrEmpty(listEntry.WellBookDetails.BookOwnerID))
                        {
                            lookupField = new SPFieldLookupValue(listEntry.WellBookDetails.BookOwnerID);
                            objListItem["Owner"] = lookupField;
                        }

                        objListItem.Update();
                        listEntry.WellBookDetails.RowId = int.Parse(objListItem["ID"].ToString());

                        web.AllowUnsafeUpdates = false;
                        objCommonDAL = new CommonDAL();
                        CommonUtility objCommonUtility = new CommonUtility();
                        userName = objCommonUtility.GetUserName();
                        objCommonDAL.UpdateListAuditHistory(siteURL, auditListName, listEntry.WellBookDetails.RowId, userName, actionPerformed);

                    }
                }
            });
        }

        /// <summary>
        /// Gets Well Book Details.
        /// </summary>
        /// <param name="parentSiteUrl">Site URL.</param>
        /// <param name="listName">Book List Name.</param>
        /// <param name="queryString">CAML Query.</param>
        /// <returns>Object of ListEntry.</returns>
        /// <exception cref="">Handled in calling method.</exception>
        internal ListEntry GetWellBookDetail(string parentSiteUrl, string listName, string queryString)
        {
            ListEntry objListEntry = null;
            string[] strSplitter = { ";#" };
            WellBookDetails objWellBookDetails = null;

            SPSecurity.RunWithElevatedPrivileges(delegate()
             {
                 using (SPSite site = new SPSite(parentSiteUrl))
                 {
                     using (SPWeb web = site.OpenWeb())
                     {
                         SPList list = web.Lists[listName];
                         SPQuery query = new SPQuery();
                         string strViewFields = @"<FieldRef Name='ID'/><FieldRef Name='Title'/><FieldRef Name='Team'/><FieldRef Name='Sign_Off_Status'/><FieldRef Name='Owner'/><FieldRef Name='Terminate_Status'/><FieldRef Name='NoOfActiveChapters'/>";
                         query.Query = queryString;
                         query.ViewFields = strViewFields;
                         SPListItemCollection itemCollection = list.GetItems(query);

                         if (itemCollection != null && itemCollection.Count > 0)
                         {
                             objListEntry = new ListEntry();
                             objWellBookDetails = new WellBookDetails();

                             SPListItem objListItem = itemCollection[0];
                             objWellBookDetails.Title = Convert.ToString(objListItem["Title"]);
                             int intRowId = 0;
                             int.TryParse(objListItem["ID"].ToString(), out intRowId);
                             objWellBookDetails.RowId = intRowId;
                             string strTeam = Convert.ToString(objListItem["Team"]);
                             string[] strTeamDetails = strTeam.Split(strSplitter, StringSplitOptions.None);
                             if (strTeamDetails != null && strTeamDetails.Length == 2)
                             {
                                 objWellBookDetails.TeamID = strTeamDetails[0];
                                 objWellBookDetails.Team = strTeamDetails[1];
                             }
                             objWellBookDetails.SignOffStatus = Convert.ToString(objListItem["Sign_Off_Status"]);

                             string strOwner = Convert.ToString(objListItem["Owner"]);
                             string[] strOutput = strOwner.Split(strSplitter, StringSplitOptions.None);
                             if (strOutput != null && strOutput.Length == 2)
                             {
                                 objWellBookDetails.BookOwnerID = strOutput[0];
                                 objWellBookDetails.BookOwner = strOutput[1];
                             }
                             objWellBookDetails.Terminated = Convert.ToString(objListItem["Terminate_Status"]);
                             int intNoOfActiveChapters = 0;
                             /// Assign the no of active chapters in a book to objWellBookDetails.NoOfActiveChapters
                             int.TryParse(Convert.ToString(objListItem["NoOfActiveChapters"]), out intNoOfActiveChapters);
                             objWellBookDetails.NoOfActiveChapters = intNoOfActiveChapters;

                             objListEntry.WellBookDetails = objWellBookDetails;
                         }
                     }
                 }
             });
            return objListEntry;
        }

        #region DREAM 4.0 - eWB2.0 - Change discipline when page owner is changed
        /// Updating the Discipline column is added.
        /// discipline parameter is added for the same.

        /// <summary>
        /// Updates the Page Owner details.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listEntry">The list entry.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="auditListName">Audit List Name.</param>
        /// <param name="userName">User Name.</param>
        /// <param name="actionPerformed">Audit Action.</param>
        /// <exception cref="">Handled in calling method.</exception>
        internal void UpdatePageOwner(string siteURL, ListEntry listEntry, string listName, string auditListName, string userName, string actionPerformed,string discipline)
        {
            SPList list;
            SPQuery query;
            SPListItem objListItem;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        list = web.Lists[listName];
                        query = new SPQuery();
                        for (int intIndex = 0; intIndex < listEntry.ChapterPagesMapping.Count; intIndex++)
                        {
                            objListItem = list.GetItemById(listEntry.ChapterPagesMapping[intIndex].RowId);
                            objListItem["Owner"] = userName;
                            #region DREAM 4.0 - eWB 2.0 - Change discipline when page owner changed
                            objListItem["Discipline"] = discipline;
                            #endregion 
                            objListItem.Update();
                        }
                        web.AllowUnsafeUpdates = false;
                        objCommonDAL = new CommonDAL();
                        for (int intIndex = 0; intIndex < listEntry.ChapterPagesMapping.Count; intIndex++)
                        {
                            /// userName contains the value of new page owner selected.
                            /// Instead of new page owner, current login user name should be used for audit history.
                            /// Fix is applied.
                            userName = string.Empty;
                            CommonUtility objCommonUtility = new CommonUtility();
                            userName = objCommonUtility.GetUserName();
                            objCommonDAL.UpdateListAuditHistory(siteURL, auditListName, listEntry.ChapterPagesMapping[intIndex].RowId, userName, actionPerformed);
                        }

                    }
                }
            });
        }

        #endregion

        /// <summary>
        /// Updates the Page Owner details in Story Board list.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="camlQuery">The caml query.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="userName">User Name.</param>
        /// <exception cref="">Handled in calling method.</exception>
        internal void UpdatePageOwner(string siteURL, string camlQuery, string listName, string userName)
        {
            SPList list;
            SPQuery query;

            SPListItemCollection objListItemCollection = null;
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
                        objListItemCollection = list.GetItems(query);
                        if (objListItemCollection != null)
                        {
                            foreach (SPListItem listItem in objListItemCollection)
                            {

                                listItem["Page_Owner"] = userName;
                                listItem.Update();

                            }
                        }
                        web.AllowUnsafeUpdates = false;

                    }
                }
            });
        }
        /// <summary>
        /// Updates the Pages details in a Well Book.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listEntry">The list entry.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="auditListName">Audit List Name.</param>
        /// <param name="userName">User Name.</param>
        /// <param name="actionPerformed">Audit Action.</param>
        /// <exception cref="">Handled in calling method.</exception>
        internal void UpdateBookPage(string siteURL, ListEntry listEntry, string listName, string auditListName, string userName, string actionPerformed)
        {
            SPList list;
            SPListItem objListItem;
            DataTable dtlistItem = null;
            int intId = 0;
            int intRowId = 0;
            int intPageSequence = 0;
            DataView dvResultView;
            SPQuery query = null;
            string strCAMLQuery = string.Empty;
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
                            objListItem = list.Items.Add();
                            if (string.Equals(actionPerformed, AUDIT_ACTION_UPDATION))
                            {
                                objListItem = list.GetItemById(listEntry.MasterPage.RowId);
                            }
                            objListItem["Page_Name"] = listEntry.MasterPage.Name;
                            objListItem["Page_Actual_Name"] = listEntry.MasterPage.TemplateTitle;
                            objListItem["Page_Sequence"] = listEntry.MasterPage.PageSequence;
                            objListItem["Standard_Operating_Procedure"] = listEntry.MasterPage.SOP;
                            objListItem["Discipline"] = listEntry.MasterPage.SignOffDiscipline;
                            objListItem.Update();
                            int.TryParse((objListItem["ID"].ToString()), out intId);
                            listEntry.MasterPage.RowId = intId;
                            /// start
                            query = new SPQuery();
                            query.Query = @"<Where><And><Eq><FieldRef Name='Terminate_Status' />
              <Value Type='Choice'>No</Value></Eq><Eq><FieldRef Name='Chapter_ID' />
              <Value Type='Number'>" + Convert.ToString(objListItem["Chapter_ID"]) + "</Value></Eq></And></Where>";
                            dtlistItem = list.GetItems(query).GetDataTable();
                            if (dtlistItem != null && dtlistItem.Rows.Count > 0)
                            {
                                dvResultView = dtlistItem.DefaultView;
                                dvResultView.Sort = "Page_Sequence asc";
                                for (int intIndex = 0; intIndex < dvResultView.Count; intIndex++)
                                {
                                    intPageSequence = intPageSequence + 10;
                                    intRowId = (int)dvResultView[intIndex]["ID"];
                                    objListItem = list.GetItemById(intRowId);
                                    objListItem["Page_Sequence"] = intPageSequence;

                                    objListItem.Update();
                                }
                            }
                            /// end
                            web.AllowUnsafeUpdates = false;
                            objCommonDAL = new CommonDAL();
                            objCommonDAL.UpdateListAuditHistory(siteURL, auditListName, listEntry.MasterPage.RowId, userName, actionPerformed);
                            /// Update the values to StoryBoard list
                            WellBookChapterDAL objWellBookChapterDAL = new WellBookChapterDAL();
                            StoryBoard objStoryBoard = new StoryBoard();
                            objListItem = list.GetItemById(listEntry.MasterPage.RowId);

                            if (objListItem != null)
                            {
                                objStoryBoard.PageId = Int32.Parse(Convert.ToString(objListItem["ID"]));
                                objStoryBoard.SOP = Convert.ToString(objListItem["Standard_Operating_Procedure"]);
                                objStoryBoard.PageTitle = Convert.ToString(objListItem["Page_Actual_Name"]);
                                objStoryBoard.Discipline = Convert.ToString(objListItem["Discipline"]);

                                strCAMLQuery = string.Empty;
                                strCAMLQuery = @"<Where><Eq><FieldRef Name='Page_ID' /><Value Type='Number'>" + objStoryBoard.PageId.ToString() + "</Value></Eq></Where>";
                                objWellBookChapterDAL.UpdateStoryBoard(siteURL, DWBSTORYBOARD, CHAPTERPAGESMAPPINGAUDITLIST, strCAMLQuery, objStoryBoard.PageId.ToString(), objStoryBoard, actionPerformed, userName);
                            }
                        }
                    }
                });
            }
            finally
            {
                if (dtlistItem != null) dtlistItem.Dispose();
            }
        }

        /// <summary>
        /// Updates the list item value.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="rowId">The row id.</param>
        /// <param name="listItemCollection">The list item collection.</param>
        /// <exception cref="">Handled in calling method.</exception>
        internal void UpdateListItemValue(string siteURL, string listName, int rowId, Dictionary<string, string> listItemCollection)
        {
            SPList list;
            SPListItem objListItem;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        list = web.Lists[listName];
                        objListItem = list.GetItemById(rowId);
                        if (objListItem != null)
                        {
                            foreach (string listItemname in listItemCollection.Keys)
                            {
                                objListItem[listItemname] = listItemCollection[listItemname];

                            }
                            objListItem.Update();

                        }
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });

        }

        /// <summary>
        /// Updates the list item collection values.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="camlQuery">The caml query.</param>
        /// <param name="listItemCollection">The list item collection.</param>
        /// <exception cref="">Handled in calling method.</exception>
        internal static void UpdateListItemCollectionValues(string siteURL, string listName, string camlQuery, Dictionary<string, string> listItemCollection)
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

                        if (objListItems != null)
                        {
                            foreach (SPListItem objListItem in objListItems)
                            {
                                foreach (string listItemname in listItemCollection.Keys)
                                {
                                    objListItem[listItemname] = listItemCollection[listItemname];

                                    //Condition Added by Gopinath due to the breaking of Sign off status
                                    //Praveena modified earlier.
                                    if (string.Compare(listName, CHAPTERPAGESMAPPINGLIST, true) == 0)
                                    {
                                        if (listItemname == "Sign_Off_Status" && objListItem[listItemname].ToString() == "Yes")
                                        {
                                            objListItem["Last_SO_Date"] = GetDateTime(DateTime.Now.ToString());
                                        }
                                    }
                                    
                                }
                                objListItem.Update();
                            }

                        }
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });

        }


        /// <summary>
        /// Converts and Returns the Culture Formatted Date Time object of the date in string 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private static string GetDateTime(string date)
        {
            string strDateFormat = string.Empty;
            strDateFormat = PortalConfiguration.GetInstance().GetKey(DATEFORMAT).ToString();
            DateTime dtmValue = DateTime.Parse(date);
            return dtmValue.ToString(strDateFormat);
        }

        /// <summary>
        /// Updates the SignOff/UnsignOff Status
        /// Added By: Praveena  
        /// Date:11/09/2010
        /// Reason: For module Simplify Sign Off
        /// </summary>
        /// <param name="siteURL"></param>
        /// <param name="listName"></param>
        /// <param name="camlQuery"></param>
        /// <param name="signOffStatus"></param>
        internal static void UpdateBulkSignOffSatus(string siteURL, string listName, string camlQuery, string signOffStatus)
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

                        if (objListItems != null)
                        {
                            foreach (SPListItem objListItem in objListItems)
                            {
                                objListItem["Sign_Off_Status"] = signOffStatus;
                                if (signOffStatus == "Yes")
                                {                                  
                                    objListItem["Last_SO_Date"] = GetDateTime(DateTime.Now.ToString());
                                }
                                objListItem.Update();
                            }
                        }
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });

        }

        /// <summary>
        /// <remarks>Added By Gopinath</remarks>
        /// Date : 22-11-2010
        /// Reason : To update the batch import log at once.
        /// </summary>
        /// <param name="siteURL">string</param>
        /// <param name="listName">string</param>
        /// <param name="batchImportLog">DataTable</param>
        internal void SaveBatchImportLog(string siteURL, string listName, DataTable batchImportLog)
        {
            SPList list;
            SPListItem listItem;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        list = web.Lists[listName];

                        //SPListItem listItem = list.Items.Add();

                        if (batchImportLog != null && batchImportLog.Rows.Count > 0)
                        {
                            foreach (DataRow eachRow in batchImportLog.Rows)
                            {
                                listItem = list.Items.Add();
                                listItem["Page_Name"] = eachRow["PageName"].ToString();
                                listItem["Chapter_Name"] = eachRow["ChapterName"].ToString();
                                listItem["DateAndTime"] = eachRow["DateAndTime"].ToString();
                                listItem["Status"] = eachRow["Status"].ToString();
                                listItem["Detail"] = eachRow["Detail"].ToString();
                                listItem.Update();
                            }
                        }

                        web.AllowUnsafeUpdates = false;
                    }
                }
            });

        }

        /// <summary>
        /// <remarks>Added By Gopinath</remarks>
        /// Date : 22-11-2010
        /// Reason : To update the batch import log at once.
        /// Deletes all the items from the specifid list. Uses batch processing for performance.
        /// </summary>
        /// <param name="site">The site that hosts the list.</param>
        /// <param name="list">The name of the list that has the items to be deleted.</param>
        internal void ClearBatchImportLog(string siteURL, string listName)
        {
            try
            {
                SPList list;
                int intItemsCount;
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteURL))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;
                            list = web.Lists[listName];
                            intItemsCount = list.ItemCount;

                            if (intItemsCount > 0)
                            {
                                StringBuilder sbDelete = BuildBatchDeleteCommand(list);

                                site.RootWeb.ProcessBatchData(sbDelete.ToString());
                            }
                            web.AllowUnsafeUpdates = false;
                        }
                    }
                });
            }
            catch (SPException ex)
            {
                string str = ex.InnerException.ToString();
            }
        }

        /// <summary>
        /// Builds a batch string with a list of all the items that are to be deleted.
        /// </summary>
        /// <param name="spList"></param>
        /// <returns></returns>
        private static StringBuilder BuildBatchDeleteCommand(SPList list)
        {
            StringBuilder sbDelete = new StringBuilder();
            sbDelete.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?><Batch>");
            string command = "<Method><SetList Scope=\"Request\">" + list.ID +
                "</SetList><SetVar Name=\"ID\">{0}</SetVar><SetVar Name=\"Cmd\">Delete</SetVar></Method>";

            foreach (SPListItem item in list.Items)
            {
                sbDelete.Append(string.Format(command, item.ID.ToString()));
            }
            sbDelete.Append("</Batch>");
            return sbDelete;
        }
        #endregion Methods
    }
}
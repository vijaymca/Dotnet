#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: CommonDAL.cs
#endregion

using System;
using System.Data;
using System.IO;
using System.Xml;
using System.Text;
using System.Web;
using Microsoft.SharePoint;
using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.DWB.DataAccessLayer
{
    /// <summary>
    /// Data Access Layer for common methods used across DWB
    /// </summary>
    public class CommonDAL
    {
        #region DECLARATIONS
        private const string XSLTEMPLATES = "XSLTemplates";
        private const string CHAPTERLIST = "DWB Chapter";
        private const string BOOKLIST = "DWB Books";
        private const string CHAPTERPAGEMAPPING = "DWB Chapter Pages Mapping";
        private const string EDIT = "edit";
        private const string VIEW = "view";
        private const string ADD = "add";
        private const string DATEFORMAT = "Date Format";
        #endregion DECLARATIONS

        /// <summary>
        /// Reads the list.
        /// </summary>
        /// <param name="parentSiteUrl">The parent site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns></returns>
        internal DataTable ReadList(string parentSiteUrl, string listName, string queryString)
        {
            DataTable dtObjListItems = new DataTable();
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(parentSiteUrl))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPList list = web.Lists[listName];
                            SPQuery query = new SPQuery();

                            query.Query = queryString;
                            SPListItemCollection listItemCollection = list.GetItems(query);

                            if (listItemCollection != null && listItemCollection.Count > 0)
                            {
                                /// Reads the values from sharepoint list.
                                dtObjListItems = list.GetItems(query).GetDataTable();
                            }
                        }
                    }
                });
            }
            finally
            {
                if (dtObjListItems != null)
                    dtObjListItems.Dispose();
            }
            return dtObjListItems;
        }
        /// <summary>
        /// Reads the library.
        /// </summary>
        /// <param name="parentSiteUrl">The parent site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns>DataTable</returns>
        internal DataTable ReadLibrary(string parentSiteUrl, string libraryName, int bookId)
        {
            DataTable dtFiles = new DataTable();
            SPQuery query;
            string strFileName = string.Empty;
            string strFileURL = string.Empty;
            string strPublishedDate = string.Empty;
            string newURL = string.Empty;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(parentSiteUrl))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPList library = web.Lists[libraryName];
                            query = new SPQuery();
                            query.Query = "<Where><And><And><Eq><FieldRef Name=\"Book_ID\" /><Value Type=\"Text\">" + bookId + "</Value></Eq><Eq><FieldRef Name=\"IsPublish\" /><Value Type=\"Text\">True</Value></Eq></And><Eq><FieldRef Name=\"PrintStatus\" /><Value Type=\"Choice\">Completed</Value></Eq></And></Where>";
                            SPListItemCollection libraryItemCollection = library.GetItems(query);
                            dtFiles.Columns.Add("FileName");
                            dtFiles.Columns.Add("FileURL");
                            dtFiles.Columns.Add("PublishedDate");
                            if (libraryItemCollection != null && libraryItemCollection.Count > 0)
                            {
                                newURL = parentSiteUrl.Substring(0, parentSiteUrl.IndexOf("/Pages"));
                                /// Reads the Files from sharepoint library.
                                foreach (SPListItem spListItem in libraryItemCollection)
                                {
                                    strFileName = spListItem["DocumentURL"].ToString().Substring(spListItem["DocumentURL"].ToString().LastIndexOf("/") + 1);
                                    strFileURL = newURL + "/Pages/eWBPDFViewer.aspx?mode=book&requestID=" + spListItem["RequestID"].ToString();
                                    strPublishedDate = spListItem["Modified"].ToString();
                                    DateTime dtmValue = DateTime.Parse(strPublishedDate);
                                    dtFiles.Rows.Add(strFileName, strFileURL, dtmValue.ToString(Convert.ToString(PortalConfiguration.GetInstance().GetKey(DATEFORMAT))));
                                }
                            }
                        }
                    }
                });
            }
            finally
            {
                if (dtFiles != null)
                    dtFiles.Dispose();
            }
            return dtFiles;
        }
        /// <summary>
        /// Reads the list.
        /// </summary>
        /// <param name="parentSiteUrl">The parent site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns></returns>
        internal DataTable ReadList(string parentSiteUrl, string listName, string queryString, string fieldstoView)
        {
            DataTable dtListItems = null;
            SPList list;
            SPQuery query;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(parentSiteUrl))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            list = web.Lists[listName];
                            query = new SPQuery();

                            query.Query = queryString;
                            query.ViewFields = fieldstoView;

                            if (list.GetItems(query).Count > 0)
                            {
                                /// Reads the values from sharepoint list.
                                dtListItems = list.GetItems(query).GetDataTable();
                            }
                        }
                    }
                });
            }
            finally
            {
                if (dtListItems != null)
                    dtListItems.Dispose();
            }
            return dtListItems;
        }

        /// <summary>
        /// Updates Sequence.
        /// </summary>
        /// <param name="parentSiteUrl">The parent site URL.</param>
        /// <param name="listName">Name of the list.</param>
        ///<param name="dvlistItems">DataView object.</param>
        /// <param name="fieldName">Field Name.</param>
        /// <returns></returns>
        internal void UpdateSequence(string parentSiteUrl, string listName, DataView dvlistItems, string fieldName)
        {
            string strlistGuid = string.Empty;
            StringBuilder methodBuilder = new StringBuilder();
            string strBatch = string.Empty;
            string strBatchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
  "<ows:Batch OnError=\"Return\">{0}</ows:Batch>";

            string strMethodFormat = "<Method ID=\"{0}\">" +
             "<SetList>{1}</SetList>" +
             "<SetVar Name=\"Cmd\">Save</SetVar>" +
             "<SetVar Name=\"ID\">{2}</SetVar>" +
             "<SetVar Name=\"urn:schemas-microsoft-com:office:office#" + fieldName + "\">{3}</SetVar>" +
            "</Method>";
            SPList list;
            int intRowId = 0;


            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(parentSiteUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        list = web.Lists[listName];
                        strlistGuid = list.ID.ToString();
                        if (dvlistItems != null && dvlistItems.Count > 0)
                        {
                            for (int i = 0; i < dvlistItems.Count; i++)
                            {
                                intRowId = int.Parse(dvlistItems[i]["ID"].ToString());
                                methodBuilder.AppendFormat(strMethodFormat, intRowId, strlistGuid, intRowId, dvlistItems[i][fieldName]);
                            }
                            strBatch = string.Format(strBatchFormat, methodBuilder.ToString());
                            web.ProcessBatchData(strBatch);
                        }
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });

        }

        /// <summary>
        /// Delete the audit list entry
        /// </summary>
        /// <param name="parentSiteUrl">The parent site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns></returns>
        internal void DeleteAuditTrail(string parentSiteUrl, string listName, string queryString)
        {

            SPListItemCollection objlistItemCollection;
            SPList list;
            SPQuery query;
            int intIndex = 0;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(parentSiteUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        list = web.Lists[listName];
                        web.AllowUnsafeUpdates = true;
                        query = new SPQuery();
                        query.Query = queryString;
                        objlistItemCollection = list.GetItems(query);
                        if (objlistItemCollection != null)
                        {
                            while (objlistItemCollection.Count != 0)
                            {
                                objlistItemCollection.Delete(intIndex);

                            }
                        }
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });

        }
        /// <summary>
        /// Activate the listitem.
        /// </summary>
        /// <param name="parentSiteUrl">The parent site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="rowId">ID of the Item.</param>
        /// <param name="itemStatus">Status of the Item.</param>
        /// <param name="updatePageSequence">Update Page Sequence or not (true/false).</param>
        /// <returns></returns>
        internal void ListStatusUpdate(string parentSiteUrl, string listName, int rowId, string itemStatus, bool updatePageSequence)
        {
            SPListItem objListItem = null;
            SPList list;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(parentSiteUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        list = web.Lists[listName];
                        objListItem = list.GetItemById(rowId);
                        if (objListItem != null)
                        {
                            objListItem["Terminate_Status"] = itemStatus;
                            if (updatePageSequence)
                            {
                                objListItem["Page_Sequence"] = 1;
                            }
                            objListItem.Update();
                        }

                        web.AllowUnsafeUpdates = false;

                    }
                }
            });
        }
        /// <summary>
        /// Activate the listitem.
        /// </summary>
        /// <param name="parentSiteUrl">The parent site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="rowId">The row id.</param>
        /// <param name="itemStatus">The item status.</param>
        /// <param name="auditListName">Name of the audit list.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="actionPerformed">The action performed.</param>
        /// <param name="seqFieldName">Name of the seq field.</param>
        internal void ListItemStatusUpdate(string parentSiteUrl, string listName, int rowId, string itemStatus, string auditListName, string userName,
                string actionPerformed, string seqFieldName)
        {
            SPListItem objListItem = null;
            SPQuery objQuery = null;
            DataTable dtResultTable = null;
            int intSequence = 10;
            int intListItemId = 0;
            SPList list;
            string strListGuid = string.Empty;
            StringBuilder methodBuilder = new StringBuilder();

            string strBatch = string.Empty;
            string strBatchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
  "<ows:Batch OnError=\"Return\">{0}</ows:Batch>";

            string strMethodFormat = "<Method ID=\"{0}\">" +
             "<SetList>{1}</SetList>" +
             "<SetVar Name=\"Cmd\">Save</SetVar>" +
             "<SetVar Name=\"ID\">{2}</SetVar>" +
             "<SetVar Name=\"urn:schemas-microsoft-com:office:office#" + seqFieldName + "\">{3}</SetVar>" +
            "</Method>";
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(parentSiteUrl))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;
                            list = web.Lists[listName];
                            strListGuid = list.ID.ToString();
                            objListItem = list.GetItemById(rowId);
                            string strNoOfActiveChapters = string.Empty;
                            string strBookID = string.Empty;
                            if (objListItem != null)
                            {
                                objListItem["Terminate_Status"] = itemStatus;
                                if (!string.IsNullOrEmpty(seqFieldName) && itemStatus.Equals("Yes", StringComparison.OrdinalIgnoreCase))
                                {
                                    objListItem[seqFieldName] = 1;
                                    if (CHAPTERLIST == listName)
                                    {
                                        strBookID = Convert.ToString(objListItem["Book_ID"]);
                                    }
                                }
                                objListItem.Update();
                                if (!string.IsNullOrEmpty(seqFieldName) && itemStatus.Equals("No", StringComparison.OrdinalIgnoreCase))
                                {
                                    methodBuilder.AppendFormat(strMethodFormat, rowId, strListGuid, rowId, 10);
                                    objQuery = new SPQuery();
                                    if (CHAPTERLIST == listName)
                                    {
                                        strBookID = Convert.ToString(objListItem["Book_ID"]);
                                        objQuery.Query = @"<OrderBy><FieldRef Name='" + seqFieldName + "' /></OrderBy><Where><And><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq><Eq><FieldRef Name='Book_ID' /><Value Type='Number'>" + strBookID + "</Value></Eq></And></Where>";

                                    }
                                    else if (CHAPTERPAGEMAPPING == listName)
                                    {
                                        objQuery.Query = @"<OrderBy><FieldRef Name='" + seqFieldName + "' /></OrderBy><Where><And><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq><Eq><FieldRef Name='Chapter_ID' /><Value Type='Number'>" + Convert.ToString(objListItem["Chapter_ID"]) + "</Value></Eq></And></Where>";
                                    }
                                    else
                                    {
                                        objQuery.Query = @"<OrderBy><FieldRef Name='" + seqFieldName + "' /></OrderBy><Where><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq></Where>";
                                    }

                                    dtResultTable = list.GetItems(objQuery).GetDataTable();
                                    if (dtResultTable != null && dtResultTable.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dtResultTable.Rows.Count; i++)
                                        {

                                            int.TryParse(Convert.ToString(dtResultTable.Rows[i]["ID"]), out intListItemId);
                                            if (intListItemId != rowId)
                                            {
                                                intSequence = intSequence + 10;
                                                methodBuilder.AppendFormat(strMethodFormat, intListItemId, strListGuid, intListItemId, intSequence);

                                            }
                                        }
                                        strNoOfActiveChapters = dtResultTable.Rows.Count.ToString();
                                    }
                                    strBatch = string.Format(strBatchFormat, methodBuilder.ToString());
                                    web.ProcessBatchData(strBatch);
                                }
                                /// When Chapter is activated/terminated this method would be called
                                /// noOfActiveChapters would be set to count when terminated. On Activation the query would be run in UpdateNoOfActiveChapters method
                                if (string.Compare(listName, CHAPTERLIST, true) == 0)
                                {
                                    UpdateNoOfActiveChapters(parentSiteUrl, BOOKLIST, strBookID, strNoOfActiveChapters);
                                }
                                UpdateListAuditHistory(parentSiteUrl, auditListName, rowId, userName, actionPerformed);
                            }

                            web.AllowUnsafeUpdates = false;

                        }
                    }
                });
            }
            finally
            {
                if (dtResultTable != null) dtResultTable.Dispose();
            }

        }

        /// <summary>
        /// Updates the SignOff status of Chapter Pages.
        /// </summary>
        /// <param name="parentSiteUrl">The parent site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="rowId">The row id.</param>
        /// <param name="itemStatus">The item status.</param>
        internal void ListItemStatusUpdateForChapterPages(string parentSiteUrl, string listName, int rowId, string itemStatus)
        {
            SPQuery objQuery = null;
            DataTable dtResultTable = null;
            int listItemId = 0;
            SPList list;
            string strListGuid = string.Empty;
            StringBuilder sbMethodBuilder = new StringBuilder();

            string strBatch = string.Empty;
            string strBatchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
  "<ows:Batch OnError=\"Return\">{0}</ows:Batch>";

            string strMethodFormat = "<Method ID=\"{0}\">" +
             "<SetList>{1}</SetList>" +
             "<SetVar Name=\"Cmd\">Save</SetVar>" +
             "<SetVar Name=\"ID\">{2}</SetVar>" +
             "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Terminate_Status\">{3}</SetVar>" +
            "</Method>";
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(parentSiteUrl))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;
                            list = web.Lists[listName];
                            strListGuid = list.ID.ToString();

                            objQuery = new SPQuery();

                            if (string.Compare(itemStatus, "Yes", true) == 0)
                            {
                                objQuery.Query = @"<Where><And><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq><Eq><FieldRef Name='Chapter_ID' /><Value Type='Number'>" + Convert.ToString(rowId) + "</Value></Eq></And></Where>";
                            }
                            else if (string.Compare(itemStatus, "No", true) == 0)
                            {
                                objQuery.Query = @"<Where><And><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>Yes</Value></Eq><Eq><FieldRef Name='Chapter_ID' /><Value Type='Number'>" + Convert.ToString(rowId) + "</Value></Eq></And></Where>";
                            }
                            dtResultTable = list.GetItems(objQuery).GetDataTable();
                            if (dtResultTable != null && dtResultTable.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtResultTable.Rows.Count; i++)
                                {

                                    int.TryParse(Convert.ToString(dtResultTable.Rows[i]["ID"]), out listItemId);
                                    if (listItemId != rowId)
                                    {
                                        sbMethodBuilder.AppendFormat(strMethodFormat, listItemId, strListGuid, listItemId, itemStatus);
                                    }
                                }
                            }
                            strBatch = string.Format(strBatchFormat, sbMethodBuilder.ToString());
                            web.ProcessBatchData(strBatch);

                            web.AllowUnsafeUpdates = false;

                        }
                    }
                });
            }
            finally
            {
                if (dtResultTable != null) dtResultTable.Dispose();
            }
        }

        /// <summary>
        /// Checks the duplicate.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="value">The value.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="listName">Name of the list.</param>
        /// <returns>bool</returns>
        internal bool CheckDuplicate(string siteURL, string value, string columnName, string listName)
        {
            bool blnIsValueExist = false;
            SPList list;
            SPQuery query;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        list = web.Lists[listName];
                        query = new SPQuery();
                        query.Query = @"<Where><Eq><FieldRef Name='" + columnName + "' /><Value Type='Text'>" + value + "</Value></Eq></Where>";
                        if (list.GetItems(query).Count > 0)
                        {
                            blnIsValueExist = true;
                        }
                    }
                }
            });

            return blnIsValueExist;
        }

        /// <summary>
        /// Checks the duplicate excluding current item.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="value">The value.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="rowID">The row ID.</param>
        /// <returns>bool</returns>
        internal bool CheckDuplicate(string siteURL, string value, string columnName, string listName, string rowID)
        {
            bool blnIsValueExist = false;
            SPList list;
            SPQuery query;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        list = web.Lists[listName];
                        query = new SPQuery();
                        query.Query = @"<Where><Eq><FieldRef Name='" + columnName + "' /><Value Type='Text'>" + value + "</Value></Eq></Where>";
                        SPListItemCollection listItemCollection = list.GetItems(query);
                        if (listItemCollection != null && listItemCollection.Count > 0)
                        {
                            SPListItem listItem = listItemCollection[0];
                            if (string.Compare(listItem["ID"].ToString(), rowID) != 0)
                            {
                                blnIsValueExist = true;
                            }
                        }
                    }
                }
            });
            return blnIsValueExist;
        }

        /// <summary>
        /// Checks for the duplicate Chapter Title for the selected Book
        /// </summary>
        /// <param name="siteURL">Site URL.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="chapterTitle">Chapter Title</param>
        /// <param name="chapterID">Chapter ID(string.Empty in ADD mode)</param>
        /// <param name="bookID">Book ID</param>
        /// <param name="mode">add/edit</param>
        /// <returns>bool</returns>
        internal bool CheckDuplicateChapter(string siteURL, string listName, string chapterTitle, string chapterID, string bookID, string mode)
        {
            bool blnIsValueExist = false;
            SPList list;
            SPQuery query;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        list = web.Lists[listName];
                        query = new SPQuery();
                        if (string.Compare(mode, ADD, true) == 0)
                        {
                            query.Query = @"<Where><And><Eq><FieldRef Name='Book_ID' /><Value Type='Number'>" + bookID + "</Value>         </Eq><Eq><FieldRef Name='Title' /><Value Type='Text'>" + chapterTitle + "</Value></Eq></And></Where>";
                            query.ViewFields = @"<FieldRef Name='Book_ID' /><FieldRef Name='Title' /><FieldRef Name='ID' />";
                            SPListItemCollection listItemCollection = list.GetItems(query);
                            if (listItemCollection != null && listItemCollection.Count > 0)
                            {
                                blnIsValueExist = true;
                            }
                        }
                        else if (string.Compare(mode, EDIT, true) == 0)
                        {
                            query.Query = @"<Where><And><Eq><FieldRef Name='Book_ID' /><Value Type='Number'>" + bookID + "</Value>         </Eq><Eq><FieldRef Name='Title' /><Value Type='Text'>" + chapterTitle + "</Value></Eq></And></Where>";
                            query.ViewFields = @"<FieldRef Name='Book_ID' /><FieldRef Name='Title' /><FieldRef Name='ID' />";
                            SPListItemCollection listItemCollection = list.GetItems(query);
                            if (listItemCollection != null && listItemCollection.Count > 0)
                            {
                                foreach (SPListItem listItem in listItemCollection)
                                {
                                    if (string.Compare(listItem["ID"].ToString(), chapterID) != 0)
                                    {
                                        blnIsValueExist = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            });

            return blnIsValueExist;
        }


        /// <summary>
        /// Gets the XSL template.
        /// </summary>
        /// <param name="strType">Type of the STR.</param>
        /// <param name="strParentSiteUrl">The STR parent site URL.</param>
        /// <returns>XmlTextReader</returns>
        internal XmlTextReader GetXSLTemplate(string xslFileName, string parentSiteUrl)
        {
            SPFile XSLFile = null;
            MemoryStream objMemoryStream = null;
            XmlTextReader xmlTextReader = null;

            SPSecurity.RunWithElevatedPrivileges(delegate()
                   {
                       using (SPSite site = new SPSite(parentSiteUrl))
                       {
                           using (SPWeb web = site.OpenWeb())
                           {
                               /// Reads the Xsl template files from XSLTemplate list.
                               XSLFile = web.Folders[XSLTEMPLATES].Files[xslFileName + ".xsl"];
                               if (XSLFile != null)
                               {
                                   objMemoryStream = new MemoryStream(XSLFile.OpenBinary());
                                   xmlTextReader = new XmlTextReader(objMemoryStream);
                               }
                               else
                               {
                                   throw new Exception(xslFileName + ".xsl" + " XML Template not Found");
                               }

                           }
                       }
                   });
            return xmlTextReader;
        }


        /// <summary>
        /// Updates the audit history for list item.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="auditListName">Name of the audit list.</param>
        /// <param name="rowid">The rowid.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="actionPerformed">The action performed.</param>
        internal void UpdateListAuditHistory(string siteURL, string auditListName, int rowid, string userName, string actionPerformed)
        {
            string strCamlQuery = string.Empty;
            string strSelectedID = string.Empty;
            SPList list;
            SPQuery query;
            SPListItem objListItem;
            SPFieldLookupValue lookupField = null;


            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        list = web.Lists[auditListName];
                        query = new SPQuery();

                        objListItem = list.Items.Add();
                        objListItem["User"] = userName;
                        objListItem["Title"] = userName;
                        if (!string.IsNullOrEmpty(actionPerformed))
                        {
                            lookupField = new SPFieldLookupValue(actionPerformed);
                            objListItem["Audit_Action"] = lookupField;
                        }

                        objListItem["Master_ID"] = rowid;

                        objListItem["Date"] = DateTime.Now;
                        objListItem.Update();
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });
        }


        /// <summary>
        /// Updates the template mapping lite item audit history.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="auditListName">Name of the audit list.</param>
        /// <param name="rowId">The row id.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="actionPerformed">The action performed.</param>
        internal void UpdateListTemplateMappingAuditHistory(string siteURL, string auditListName, int rowId, string userName, string actionPerformed)
        {
            SPList list;
            SPListItem objListItem;
            SPFieldLookupValue lookupField;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        list = web.Lists[auditListName];
                        objListItem = list.Items.Add();
                        objListItem["User"] = userName;
                        if (!string.IsNullOrEmpty(actionPerformed))
                        {
                            lookupField = new SPFieldLookupValue(actionPerformed);
                            objListItem["Audit_Action"] = lookupField;
                        }
                        objListItem["Master_ID"] = rowId;
                        objListItem["Date"] = DateTime.Now;
                        objListItem.Update();
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        /// <summary>
        /// Uploads a file to SharePoint Document Library
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="docLibName">Name of the doc lib.</param>
        /// <param name="pageId">The page id.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="postedFileName">Name of the posted file.</param>
        /// <param name="postedFile">The posted file.</param>
        /// <returns></returns>
        internal bool UploadFileToDocumentLibrary(string siteURL, string docLibName, string pageId, string userName, string postedFileName, byte[] postedFile)
        {
            bool result = false;
            SPFolder documentLibrary;
            SPList docLibList;
            SPFile documentLibraryItem;
            SPListItemCollection spDocLibListCollection = null;
            SPQuery spQuery = new SPQuery();
            int intIndex = 0;
            spQuery.Query = "<Where><Eq><FieldRef Name='PageID' /><Value Type='Number'>" + pageId + "</Value></Eq></Where>";

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite newSite = new SPSite(siteURL))
                {
                    using (SPWeb web = newSite.OpenWeb())
                    {
                        if (web.Folders[docLibName] != null)
                        {
                            documentLibrary = web.Folders[docLibName];
                            web.AllowUnsafeUpdates = true;
                            docLibList = web.Lists[docLibName];
                            spDocLibListCollection = docLibList.GetItems(spQuery);
                            {
                                if (spDocLibListCollection != null &&
                                    spDocLibListCollection.Count > 0)
                                {
                                    while (spDocLibListCollection.Count != 0)
                                    {
                                        spDocLibListCollection.Delete(intIndex);

                                    }

                                }
                            }
                            documentLibraryItem = documentLibrary.Files.Add(pageId + "-" + postedFileName, postedFile, true);
                            documentLibraryItem.Item["User"] = userName;
                            int intPageNumber = 0;
                            int.TryParse(pageId, out intPageNumber);
                            documentLibraryItem.Item["PageID"] = intPageNumber;
                            documentLibraryItem.Item.Update();
                            result = true;
                            ///If the file upload is success update "Empty = No"  in "DWB Chapter Pages Mapping"                           
                            UpdateDocumentStatus(siteURL, CHAPTERPAGEMAPPING, pageId);
                            web.AllowUnsafeUpdates = false;
                        }

                    }
                }
            });
            return result;
        }

        /// <summary>
        /// Downloads the file from document library.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="docLibName">Name of the doc lib.</param>
        /// <param name="pageId">The page id.</param>
        /// <returns>SPFile</returns>
        internal SPFile DownloadFileFromDocumentLibrary(string siteURL, string docLibName, string pageId)
        {
            Stream strmStream = new MemoryStream();
            SPFile file = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
             {
                 using (SPSite site = new SPSite(siteURL))
                 {
                     using (SPWeb web = site.OpenWeb())
                     {
                         SPList docLib = web.Lists[docLibName];
                         SPQuery query = new SPQuery();
                         query.Query = @"<Where><Eq><FieldRef Name='PageID' /><Value Type='Number'>" + pageId + "</Value></Eq></Where>";
                         SPListItemCollection items = docLib.GetItems(query);
                         if (items.Count > 0)
                         {
                             file = items[0].File;
                         }
                     }
                 }
             });
            return file;
        }

        /// <summary>
        /// Returns the url and type of the uploaded document
        /// </summary>
        /// <param name="parentSiteUrl">The parent site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns></returns>
        internal string[] GetUploadedDocumentUrl(string parentSiteUrl, string listName, string queryString)
        {
            string strDocUrl = string.Empty;
            string strType = string.Empty;
            SPList list;
            SPQuery query;
            string[] strDetail = new string[2];

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(parentSiteUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        list = web.Lists[listName];
                        query = new SPQuery();
                        query.Query = queryString;

                        if (list.GetItems(query).Count > 0)
                        {
                            /// Reads the values from sharepoint list.
                            strDocUrl = "/" + list.GetItems(query)[0].Url;
                            strType = strDocUrl.Substring(strDocUrl.LastIndexOf(".") + 1).ToLowerInvariant();

                            strDetail[0] = strDocUrl;
                            strDetail[1] = strType;

                        }
                    }
                }
            });
            return strDetail;
        }

        /// <summary>
        /// Activate the listitem.
        /// </summary>
        /// <param name="parentSiteUrl">The parent site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="rowId">The row id.</param>
        /// <param name="itemStatus">The item status.</param>
        /// <param name="auditListName">Name of the audit list.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="actionPerformed">The action performed.</param>
        internal void SignOffStatusUpdate(string parentSiteUrl, string listName, int rowId, string itemStatus, string auditListName, string userName, string actionPerformed)
        {
            SPListItem objListItem = null;
            SPList list;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(parentSiteUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        list = web.Lists[listName];
                        objListItem = list.GetItemById(rowId);
                        if (objListItem != null)
                        {
                            objListItem["Sign_Off_Status"] = itemStatus;
                            objListItem.Update();
                            UpdateListAuditHistory(parentSiteUrl, auditListName, rowId, userName, actionPerformed);
                        }

                        web.AllowUnsafeUpdates = false;

                    }
                }
            });
        }

        /// <summary>
        /// Updates the no of active chapters.
        /// </summary>
        /// <param name="siteUrl">The site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="bookID">The book ID.</param>
        /// <param name="noActiveChapters">The no active chapters.</param>
        internal void UpdateNoOfActiveChapters(string siteUrl, string listName, string bookID, string noActiveChapters)
        {
            SPList list = null;
            SPList bookList = null;

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;

                            /// If noActiveChapters is null or empty query the DWB Chapter list and update based on the count
                            if (string.IsNullOrEmpty(noActiveChapters))
                            {
                                list = web.Lists[CHAPTERLIST];

                                SPQuery query = new SPQuery();
                                query.Query = @"<OrderBy><FieldRef Name='Chapter_Sequence' /></OrderBy><Where><And><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq><Eq><FieldRef Name='Book_ID' /><Value Type='Number'>" + bookID + "</Value></Eq></And></Where>";
                                SPListItemCollection objListItemCollection = list.GetItems(query);
                                if (objListItemCollection != null && objListItemCollection.Count > 0)
                                {
                                    noActiveChapters = objListItemCollection.Count.ToString();
                                }
                            }
                            bookList = web.Lists[listName];
                            SPListItem bookItem = null;
                            if (!string.IsNullOrEmpty(bookID))
                            {
                                bookItem = bookList.GetItemById(Int32.Parse(bookID));

                                if (!string.IsNullOrEmpty(noActiveChapters))
                                {
                                    bookItem["NoOfActiveChapters"] = Int32.Parse(noActiveChapters);
                                }
                                else
                                {
                                    bookItem["NoOfActiveChapters"] = 0;
                                }
                            }
                            bookItem.Update();
                            web.AllowUnsafeUpdates = false;
                        }
                    }
                });
            }
            finally
            {

            }
        }

        /// <summary>
        /// Updates the document status.
        /// </summary>
        /// <param name="siteUrl">The site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="pageID">The page ID.</param>
        internal void UpdateDocumentStatus(string siteUrl, string listName, string pageID)
        {
            SPList list = null;
            SPListItem listItem = null;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;
                            list = web.Lists[listName];
                            if (!string.IsNullOrEmpty(pageID))
                            {
                                listItem = list.GetItemById(Int32.Parse(pageID));

                                if (listItem != null)
                                {
                                    listItem["Empty"] = "No";
                                    listItem.Update();
                                }
                            }
                            web.AllowUnsafeUpdates = false;
                        }
                    }
                });
            }
            finally
            {
            }
        }
        /// <summary>
        /// Uploads the PDF file to document library.
        /// </summary>
        /// <param name="strContext">The STR context.</param>
        /// <param name="strBookID">The STR book ID.</param>
        /// <param name="memStream">The mem stream.</param>
        /// <param name="strBookName">Name of the STR book.</param>
        /// <param name="strListPublished">The STR list published.</param>
        internal int UploadPDFFileToDocumentLibrary(string siteURL, string bookID,
                                                     Stream memStream, string bookName,
                                                     string listPublished)
        {
            int intItemID = 0;
            SPFolder documentLibrary;
            SPList docLibList;
            SPFile documentLibraryItem;

            SPSecurity.RunWithElevatedPrivileges(delegate()
                  {
                      SPUserToken systoken = null;

                      using (SPSite tempSite = new SPSite(siteURL))
                      {
                          systoken = tempSite.SystemAccount.UserToken;
                      }
                      using (SPSite site = new SPSite(siteURL, systoken))
                      {
                          using (SPWeb web = site.OpenWeb())//strContext))
                          {
                              if (web.Folders[listPublished] != null)
                              {
                                  documentLibrary = web.Folders[listPublished];
                                  web.AllowUnsafeUpdates = true;
                                  docLibList = web.Lists[listPublished];
                                  char[] specialChars = { '#', '%', '&', '*', ':', '<', '>', '?', '/', '{', '|', '}' };

                                  while (bookName.IndexOfAny(specialChars) != -1)
                                  {
                                      bookName = bookName.Replace(bookName[bookName.IndexOfAny(specialChars)], '_');
                                  }
                                  documentLibraryItem = documentLibrary.Files.Add(bookName + ".pdf", memStream, true);
                                  int.TryParse(bookID, out intItemID);
                                  documentLibraryItem.Item["BookId"] = intItemID;
                                  if (HttpContext.Current != null)
                                  {
                                      if (HttpContext.Current.Items["HttpHandlerSPWeb"] == null)
                                      {
                                          HttpContext.Current.Items["HttpHandlerSPWeb"] = web;
                                      }
                                  }
                                  documentLibraryItem.Item.Update();
                                  intItemID = documentLibraryItem.Item.ID;
                                  web.AllowUnsafeUpdates = false;
                              }
                          }
                      }
                  });

            return intItemID;
        }

        /// <summary>
        /// Updates AuditHistory for SignOff/UnsignOff Status
        /// Added By: Praveena  
        /// Date:11/09/2010
        /// Reason: For module Simplify Sign Off
        /// </summary>
        /// <param name="siteURL"></param>
        /// <param name="auditListName"></param>
        /// <param name="rowIDs"></param>
        /// <param name="userName"></param>
        /// <param name="actionPerformed"></param>
        internal void UpdateBulkListAuditHistory(string siteURL, string auditListName, string rowIDs, string userName, string actionPerformed)
        {
            string strCamlQuery = string.Empty;
            string strSelectedID = string.Empty;
            SPList list;
            SPQuery query;
            SPListItem objListItem;
            SPFieldLookupValue lookupField = null;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        list = web.Lists[auditListName];
                        query = new SPQuery();
                        string[] strRowIDs = rowIDs.Split(';');

                        foreach (string rowid in strRowIDs)
                        {
                            if (!string.IsNullOrEmpty(rowid) && !rowid.Equals("on"))
                            {
                                objListItem = list.Items.Add();
                                objListItem["User"] = userName;
                                objListItem["Title"] = userName;
                                if (!string.IsNullOrEmpty(actionPerformed))
                                {
                                    lookupField = new SPFieldLookupValue(actionPerformed);
                                    objListItem["Audit_Action"] = lookupField;
                                }

                                objListItem["Master_ID"] = rowid;

                                objListItem["Date"] = DateTime.Now;
                                objListItem.Update();
                            }
                        }
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        /// <summary>
        /// To Update Batch Import Audit History
        /// </summary>
        /// <param name="siteURL"></param>
        /// <param name="auditListName"></param>
        /// <param name="bookID"></param>
        /// <param name="userName"></param>
        /// <param name="actionPerformed"></param>
        internal void UpdateBatchImportAuditHistory(string siteURL, string auditListName, string bookID, string userName, string actionPerformed)
        {
            string strCamlQuery = string.Empty;
            string strSelectedID = string.Empty;
            SPList list;
            SPQuery query;
            SPListItem objListItem;
            SPFieldLookupValue lookupField = null;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        list = web.Lists[auditListName];
                        query = new SPQuery();

                        objListItem = list.Items.Add();
                        objListItem["User"] = userName;
                        objListItem["Title"] = userName;
                        if (!string.IsNullOrEmpty(actionPerformed))
                        {
                            lookupField = new SPFieldLookupValue(actionPerformed);
                            objListItem["Audit_Action"] = lookupField;
                        }
                        objListItem["Master_ID"] = bookID;
                        objListItem["Date"] = DateTime.Now;
                        objListItem.Update();
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });

        }

        /// <summary>
        /// Prints the log.
        /// </summary>
        /// <param name="parentSiteUrl">The parent site URL.</param>
        /// <param name="message">The message.</param>
        /// <param name="location">The location.</param>
        /// <param name="title">The title.</param>
        internal void PrintLog(string parentSiteUrl, string message, string location, string title)
        {
            SPListItem objListItem = null;
            SPList list;
            try
            {
                string strEnableDWBPrintLog = PortalConfiguration.GetInstance().FindWebServiceKey("EnableDWBPrintLog", parentSiteUrl, true);

                if (!string.IsNullOrEmpty(strEnableDWBPrintLog) && string.Equals(strEnableDWBPrintLog.ToLowerInvariant(), "yes"))
                {

                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        SPUserToken systoken = null;

                        using (SPSite tempSite = new SPSite(parentSiteUrl))
                        {
                            systoken = tempSite.SystemAccount.UserToken;
                        }
                        using (SPSite site = new SPSite(parentSiteUrl, systoken))
                        {
                            using (SPWeb web = site.OpenWeb())
                            {
                                web.AllowUnsafeUpdates = true;
                                list = web.Lists["DWB Print Log"];
                                objListItem = list.Items.Add();
                                if (objListItem != null)
                                {
                                    objListItem["Title"] = title;
                                    objListItem["Location"] = location;
                                    objListItem["Message"] = message;
                                    if (System.Web.HttpContext.Current != null)
                                    {
                                        if (System.Web.HttpContext.Current.Items["HttpHandlerSPWeb"] == null)
                                        {
                                            System.Web.HttpContext.Current.Items["HttpHandlerSPWeb"] = web;
                                        }
                                    }
                                    objListItem.Update();
                                }

                                web.AllowUnsafeUpdates = false;

                            }
                        }
                    });
                }
            }
            catch
            {
                throw;
            }
        }
        #region Print Updates
        /// <summary>
        /// Updates the book print details.
        /// </summary>
        /// <param name="requestID">The request ID.</param>
        /// <param name="documentURL">The document URL.</param>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="IsPublish">if set to <c>true</c> [is publish].</param>
        /// <param name="liveBookName">Name of the live book.</param>
        /// <param name="PrintCriteriaDocument">The print criteria document.</param>
        /// <param name="eMailID">The e mail ID.</param>
        internal void UpdateBookPrintDetails(string requestID, string documentURL, string siteURL, string userName, string listName, bool IsPublish, string liveBookName, XmlDocument PrintCriteriaDocument, string eMailID)
        {
            SPFolder documentLibrary;
            SPList docLibList;
            SPFile documentLibraryItem;

            SPSecurity.RunWithElevatedPrivileges(delegate()
              {
                  SPUserToken systoken = null;

                  using (SPSite tempSite = new SPSite(siteURL))
                  {
                      systoken = tempSite.SystemAccount.UserToken;
                  }
                  using (SPSite site = new SPSite(siteURL, systoken))
                  {
                      using (SPWeb web = site.OpenWeb())//strContext))
                      {
                          if (web.Folders[listName] != null)
                          {
                              documentLibrary = web.Folders[listName];
                              web.AllowUnsafeUpdates = true;
                              docLibList = web.Lists[listName];
                              ASCIIEncoding encoding = new ASCIIEncoding();
                              documentLibraryItem = documentLibrary.Files.Add(requestID + ".xml", (encoding.GetBytes(PrintCriteriaDocument.OuterXml)), true);
                              documentLibraryItem.Item["UserName"] = userName;
                              documentLibraryItem.Item["Title"] = requestID;
                              documentLibraryItem.Item["RequestID"] = requestID;
                              documentLibraryItem.Item["DocumentURL"] = documentURL;
                              documentLibraryItem.Item["IsPublish"] = IsPublish.ToString();
                              documentLibraryItem.Item["LiveBookName"] = liveBookName.ToString();
                              documentLibraryItem.Item["PrintStatus"] = "Pending";
                              documentLibraryItem.Item["EmailID"] = eMailID;
                              if (HttpContext.Current != null)
                              {
                                  if (HttpContext.Current.Items["HttpHandlerSPWeb"] == null)
                                  {
                                      HttpContext.Current.Items["HttpHandlerSPWeb"] = web;
                                  }
                              }
                              documentLibraryItem.Item.Update();
                              web.AllowUnsafeUpdates = false;
                          }
                      }
                  }
              });
        }

        /// <summary>
        /// Deletes the chapter print details.
        /// </summary>
        /// <param name="camlQuery">The caml query.</param>
        /// <param name="parentSiteURL">The parent site URL.</param>
        /// <param name="listName">Name of the list.</param>
        internal void DeleteChapterPrintDetails(string camlQuery, string parentSiteURL, string listName)
        {
            SPListItemCollection objlistItemCollection;
            SPList list;
            SPQuery query;
            int intIndex = 0;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(parentSiteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        list = web.Lists[listName];
                        web.AllowUnsafeUpdates = true;
                        query = new SPQuery();
                        query.Query = camlQuery;
                        objlistItemCollection = list.GetItems(query);
                        if (objlistItemCollection != null)
                        {
                            while (objlistItemCollection.Count != 0)
                            {
                                objlistItemCollection.Delete(intIndex);
                            }
                        }
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        /// <summary>
        /// Updates the chapter print details.
        /// </summary>
        /// <param name="requestID">The request ID.</param>
        /// <param name="documentURL">The document URL.</param>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="listName">Name of the list.</param>
        internal void UpdateChapterPrintDetails(string requestID, string documentURL, string siteURL, string userName, string listName)
        {
            SPList list = null;
            SPListItem objListItem = null;
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
                            objListItem["UserName"] = userName;
                            objListItem["Title"] = requestID;
                            objListItem["RequestID"] = requestID;
                            objListItem["DocumentURL"] = documentURL;
                            objListItem.Update();
                            web.AllowUnsafeUpdates = false;
                        }
                    }
                });
            }
            finally
            {
            }
        }

        /// <summary>
        /// Gets the chapter print details.
        /// </summary>
        /// <param name="queryString">The query string.</param>
        /// <param name="parentSiteURL">The parent site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <returns></returns>
        internal DataTable GetChapterPrintDetails(string queryString, string parentSiteURL, string listName)
        {
            DataTable dtObjListItems = new DataTable();
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(parentSiteURL))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPList list = web.Lists[listName];
                            SPQuery query = new SPQuery();

                            query.Query = queryString;
                            SPListItemCollection listItemCollection = list.GetItems(query);

                            if (listItemCollection != null && listItemCollection.Count > 0)
                            {
                                /// Reads the values from sharepoint list.
                                dtObjListItems = list.GetItems(query).GetDataTable();
                            }
                        }
                    }
                });
            }
            finally
            {
                if (dtObjListItems != null)
                    dtObjListItems.Dispose();
            }
            return dtObjListItems;
        }

        #region DREAM 4.0 - eWB@.0

        #region Change Page Discipline when page owner changed
        /// <summary>
        /// Updates the list item.
        /// Dynamically generates the CAML query based on field name, type and value passed.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <param name="fieldValue">The field value. [In case of Choice field type, ensure the correct choice text is sent to avoid exception]</param>
        /// <param name="camlQuery">The caml query.</param>
        /// <param name="viewFields">The view fields.</param>
        internal void UpdateListItem(string siteURL, string listName, string fieldName, SPFieldType fieldType, string fieldValue, string camlQuery, string viewFields)
        {
            SPListItemCollection objListItemCollection = null;
           // SPListItem objListItem = null;
            SPList list;
            SPQuery query;
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
                        query.ViewFields = viewFields;

                        objListItemCollection = list.GetItems(query);
                        
                        if (objListItemCollection != null && objListItemCollection.Count > 0)
                        {
                            foreach (SPListItem objListItem in objListItemCollection)
                            {                               
                                if (objListItem.Fields[fieldName] != null)
                                {
                                    if (objListItem.Fields[fieldName].Type == SPFieldType.Text)
                                        objListItem[fieldName] = fieldValue;
                                    else if (objListItem.Fields[fieldName].Type == SPFieldType.Boolean)
                                    {
                                        objListItem[fieldName] = Convert.ToBoolean(fieldValue);
                                    }
                                    else if (objListItem.Fields[fieldName].Type == SPFieldType.Number)
                                    {
                                        objListItem[fieldName] = Convert.ToInt32(fieldValue);
                                    }
                                    else if (objListItem.Fields[fieldName].Type == SPFieldType.Choice)
                                    {
                                        objListItem[fieldName] = fieldValue;
                                    }
                                    objListItem.Update();
                                }
                            }
                            
                        } 
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        #endregion 

        #region Change Page Discipline when page owner changed

        /// <summary>
        /// Deletes the list item by id.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="itemID">The item ID.</param>
        internal void DeleteListItemById(string siteURL, string listName, int itemID)
        {
            SPListItem objListItem = null;           
            SPList list;
            
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        list = web.Lists[listName];
                        objListItem = list.GetItemById(itemID);
                        if (objListItem != null)
                        {
                            objListItem.Delete();
                            list.Update();
                        }
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        /// <summary>
        /// Deletes the list item by id.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="itemID">The item ID.</param>
        internal void DeleteListItemById(string siteURL, string listName, int[] itemID)
        {
            SPListItem objListItem = null;            
            SPList list;
            
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        list = web.Lists[listName];
                                                
                        for (int index = 0; index < itemID.Length; index++)
                        {
                            objListItem = list.GetItemById(itemID[index]);
                            if (objListItem != null)
                            {
                                objListItem.Delete();
                            }
                        }
                        list.Update();
                       web.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        /// <summary>
        /// Deletes the list item by id.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="itemID">The item ID.</param>
        internal void DeleteListItem(string siteURL, string listName, string camlQuery)
        {
            SPListItemCollection objlistItemCollection;
            SPList list;
            SPQuery query;
            int intIndex = 0;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        list = web.Lists[listName];
                        web.AllowUnsafeUpdates = true;
                        query = new SPQuery();
                        query.Query = camlQuery;
                        objlistItemCollection = list.GetItems(query);
                        if (objlistItemCollection != null)
                        {
                            while (objlistItemCollection.Count != 0)
                            {
                                objlistItemCollection.Delete(intIndex);

                            }
                        }
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        /// <summary>
        /// Deletes the document.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="documentLibraryName">Name of the document library.</param>
        /// <param name="camlQuery">The caml query.</param>
        internal void DeleteDocument(string siteURL, string documentLibraryName, string camlQuery)
        {
            
            SPList docLibList;            
            SPListItemCollection docLibItemCollection = null;
            SPQuery query = new SPQuery();
            int intIndex = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        docLibList = web.Lists[documentLibraryName];
                        query.Query = camlQuery;
                        docLibItemCollection = docLibList.GetItems(query);
                        {
                            if (docLibItemCollection != null &&
                                docLibItemCollection.Count > 0)
                            {
                                while (docLibItemCollection.Count != 0)
                                {
                                    docLibItemCollection.Delete(intIndex);

                                }

                            }
                        }
                    }
                }
            });
        }
        #endregion
        #endregion
        
        /// <summary>
        /// Terminates the book.
        /// </summary>
        /// <param name="strParentSiteURL">The STR parent site URL.</param>
        /// <param name="bookID">The book ID.</param>
        /// <param name="listName">Name of the list.</param>
        internal void TerminateBook(string strParentSiteURL, int bookID, string listName)
        {
            SPList list = null;
            SPListItem objListItem = null;
            string strCamlQuery = string.Empty;
            SPQuery query;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(strParentSiteURL))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;
                            list = web.Lists[listName];
                            objListItem = list.GetItemById(bookID);
                            if (objListItem != null)
                            {
                                objListItem["Terminate_Status"] = "Yes";
                                objListItem.Update();
                            }
                            web.AllowUnsafeUpdates = false;
                        }
                    }
                });
            }
            finally
            {
            }
        }

        /// <summary>
        /// Updates the book publish details.
        /// </summary>
        /// <param name="requestID">The request ID.</param>
        /// <param name="documentURL">The document URL.</param>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="IsPublish">if set to <c>true</c> [is publish].</param>
        /// <param name="liveBookName">Name of the live book.</param>
        /// <param name="PrintCriteriaDocument">The print criteria document.</param>
        /// <param name="eMailID">The e mail ID.</param>
        /// <param name="bookID">The book ID.</param>
        internal void UpdateBookPublishDetails(string requestID, string documentURL, string siteURL, string userName, string listName, bool IsPublish, string liveBookName, XmlDocument PrintCriteriaDocument, string eMailID, int bookID)
        {
            SPFolder documentLibrary;
            SPList docLibList;
            SPFile documentLibraryItem;

            SPSecurity.RunWithElevatedPrivileges(delegate()
              {
                  SPUserToken systoken = null;

                  using (SPSite tempSite = new SPSite(siteURL))
                  {
                      systoken = tempSite.SystemAccount.UserToken;
                  }
                  using (SPSite site = new SPSite(siteURL, systoken))
                  {
                      using (SPWeb web = site.OpenWeb())//strContext))
                      {
                          if (web.Folders[listName] != null)
                          {
                              documentLibrary = web.Folders[listName];
                              web.AllowUnsafeUpdates = true;
                              docLibList = web.Lists[listName];
                              ASCIIEncoding encoding = new ASCIIEncoding();
                              documentLibraryItem = documentLibrary.Files.Add(requestID + ".xml", (encoding.GetBytes(PrintCriteriaDocument.OuterXml)), true);
                              documentLibraryItem.Item["UserName"] = userName;
                              documentLibraryItem.Item["Title"] = requestID;
                              documentLibraryItem.Item["RequestID"] = requestID;
                              documentLibraryItem.Item["DocumentURL"] = documentURL;
                              documentLibraryItem.Item["IsPublish"] = IsPublish.ToString();
                              documentLibraryItem.Item["LiveBookName"] = liveBookName.ToString();
                              documentLibraryItem.Item["PrintStatus"] = "Pending";
                              documentLibraryItem.Item["Book_ID"] = bookID.ToString();
                              documentLibraryItem.Item["EmailID"] = eMailID;
                              if (HttpContext.Current != null)
                              {
                                  if (HttpContext.Current.Items["HttpHandlerSPWeb"] == null)
                                  {
                                      HttpContext.Current.Items["HttpHandlerSPWeb"] = web;
                                  }
                              }
                              documentLibraryItem.Item.Update();
                              web.AllowUnsafeUpdates = false;
                          }
                      }
                  }
              });
        }
        #endregion

        /// <summary>
        /// Gets the alert message.
        /// </summary>
        /// <param name="parentSiteURL">The parent site URL.</param>
        /// <param name="queryString">The query string.</param>
        /// <param name="listName">Name of the list.</param>
        /// <returns></returns>
        internal string GetAlertMessage(string parentSiteURL, string queryString, string listName)
        {
            string strMessage = string.Empty;
            SPListItemCollection objListItemCollection = null;
            SPList list;
            SPQuery query;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(parentSiteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {                        
                        list = web.Lists[listName];
                        query = new SPQuery();
                        query.Query = queryString;
                        objListItemCollection = list.GetItems(query);

                        if (objListItemCollection != null && objListItemCollection.Count > 0)
                        {
                            foreach (SPListItem objListItem in objListItemCollection)
                            {
                                strMessage = objListItem["AlertMessage"].ToString();
                            }
                        }
                    }
                }
            });
            return strMessage;
        }
    }
}

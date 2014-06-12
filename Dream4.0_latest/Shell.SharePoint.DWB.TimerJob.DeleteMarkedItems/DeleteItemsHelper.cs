#region Shell Copyright.2011
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: DeleteItemsHelper.cs
#endregion


using System;
using System.Data;
using System.Text;

using Microsoft.SharePoint;

namespace Shell.SharePoint.DWB.TimerJob.DeleteMarkedItems
{
    /// <summary>
    /// Helper class to delete the Books/Chapters/Chapter Pages/Comments/Narratives/Story Board/Audit Trail in timerjob.
    /// </summary>
    public class DeleteItemsHelper
    {
        #region Constants
        const string DWBBOOKSLIST = "DWB Books";
        const string DWBBOOKAUDITLIST = "DWB Books Audit Trail";
        const string DWBCHAPTERLIST = "DWB Chapter";
        const string DWBCHAPTERAUDITLIST = "DWB Chapter Audit Trail";
        const string DWBCHAPTERPAGESMAPPINGLIST = "DWB Chapter Pages Mapping";
        const string DWBCHAPTERPAGESMAPPINGAUDITLIST = "DWB Chapter Pages Mapping Audit Trail";
        const string DWBNARRATIVES = "DWB Narratives";
        const string DWBSTORYBOARD = "DWB StoryBoard";
        const string DWBCOMMENT = "DWB Comment";

        const string DWBTEMPLATELIST = "DWB Templates";
        const string DWBTEMPLATEAUDITLIST = "DWB Templates Audit Trail";

        const string DWBTEMPLATEPAGESLIST = "DWB Template Page Mapping";
        const string DWBTEMPLATECONFIGURATIONAUDIT = "DWB Template Page Mapping Audit Trail";


        const string DWBMASTERPAGESLIST = "DWB Master Pages";
        const string DWBMASTERPAGEAUDITLIST = "DWB Master Pages Audit Trail";

        const string DWBUSER = "DWB User";
        const string DWBTEAMLIST = "DWB Team";
        const string DWBTEAMSTAFFLIST = "DWB Team Staff";

        const string USERDEFINEDDOCUMENTLIST = "DWB UserDefined Documents";
        const string PUBLISHEDDOCUMENTLIST = "DWB Published Documents";
        const string PRINTEDDOCUMNETLIBRARY = "DWB Printed Documents";

        const string DELETEQUERY = @"<Where><Eq><FieldRef Name='ToBeDeleted' /><Value Type='Choice'>Yes</Value></Eq></Where>";
        const string GETCHAPTERS = @"<Where><Eq><FieldRef Name='Book_ID' /><Value Type='Number'>{0}</Value></Eq></Where>";
        const string GETCHAPTERPAGES = @"<Where><Eq><FieldRef Name='Chapter_ID' /><Value Type='Number'>{0}</Value></Eq></Where>";
        const string CHAPTERPAGEDOCUMNETCAMEQUERY = @"<Where><Eq><FieldRef Name='PageID' /><Value Type='Number'>{0}</Value></Eq></Where>";
        #endregion

        #region Variables
        string strSiteURL = string.Empty;
        #endregion


        #region Public Methods
        /// <summary>
        /// Deletes the items.
        /// </summary>
        /// <param name="siteCollectionURL">The site collection URL.</param>
        public void DeleteItems(string siteCollectionURL)
        {
            try
            {
                strSiteURL = siteCollectionURL;
                DeleteBooks();
                DeleteChapters();
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Deletes all books marked to be deleted.
        /// </summary>
        private void DeleteBooks()
        {
            try
            {
                string strAuditCamlQuery = string.Empty;
                /// Delete Books marked as "ToBeDeleted = Yes"
                /// Steps to delete Books:
                /// Step 1: Find all chapters belong to the book           
                /// Step 2: For each chapter, find all pages belong to the chapter.
                /// Step 3: For all pages in a chapter, delete audit, storyboard, comment, narrative, documents.
                /// Step 4: Delete all pages.
                /// Step 5: Delete audit for all chapter
                /// Step 6: Delete chapter
                /// Step 7: Delete audit for book
                /// Step 8: Delete Book
                /// Step 9: What about the publised copy of the book?

                int[] intBookID = GetIDs(strSiteURL, DWBBOOKSLIST, DELETEQUERY, @"<FieldRef Name='ID' /><FieldRef Name='ToBeDeleted' />");
                if (intBookID != null && intBookID.Length > 0)
                {
                    for (int intIndex = 0; intIndex < intBookID.Length; intIndex++)
                    {
                        DeleteChapters(intBookID[intIndex]);
                    }
                    /// Delete Book Audits
                    strAuditCamlQuery = CreateCAMLQuery(intBookID, "Master_ID", "Number");
                    DeleteItems(strSiteURL, DWBBOOKAUDITLIST, strAuditCamlQuery);

                    /// Delete Books
                    DeleteItems(strSiteURL, DWBBOOKSLIST, DELETEQUERY);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes all chapters marked tobedeleted in DWB Chapter List.
        /// </summary>
        private void DeleteChapters()
        {
            /// Delete Chapters marked as "ToBeDeleted = Yes"
            /// Steps to delete Chapters:
            /// Step 1: Find all chapters ToBeDeleted           
            /// Step 2: For each chapter, find all pages belong to the chapter.
            /// Step 3: For all pages in a chapter, delete audit, storyboard, comment, narrative, documents.
            /// Step 4: Delete all pages.
            /// Step 5: Delete audit for all chapter
            /// Step 6: Delete chapter
            /// Step 7: Delete audit for book
            /// Step 8: Delete Book
            /// Step 9: What about the publised copy of the book?
            try
            {
                int[] intChapterIDs = GetIDs(strSiteURL, DWBCHAPTERLIST, DELETEQUERY, @"<FieldRef Name='ID' /><FieldRef Name='ToBeDeleted' />");
                string strAuditCamlQuery = string.Empty;

                if (intChapterIDs != null && intChapterIDs.Length > 0)
                {
                    /// Delete all page related items in all chapters
                    DeleteChapterPages(intChapterIDs);

                    /// Delete Chapter Audits
                    strAuditCamlQuery = CreateCAMLQuery(intChapterIDs, "Master_ID", "Number");
                    DeleteItems(strSiteURL, DWBCHAPTERAUDITLIST, strAuditCamlQuery);

                    /// Delete All chapters ToBeDeleted
                    DeleteItems(strSiteURL, DWBCHAPTERLIST, DELETEQUERY);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes the chapters belong to a particular book.
        /// </summary>
        /// <param name="bookID">The book ID.</param>
        private void DeleteChapters(int bookID)
        {
            try
            {
                int[] intChapterID = GetIDs(strSiteURL, DWBCHAPTERLIST, string.Format(GETCHAPTERS, bookID.ToString()), @"<FieldRef Name='ID' /><FieldRef Name='ToBeDeleted' />");
                string strAuditCamlQuery = string.Empty;

                if (intChapterID != null && intChapterID.Length > 0)
                {
                    /// Delete Page and related stuffs in the Book
                    DeleteChapterPages(intChapterID);

                    /// Delete Chapter Audits
                    strAuditCamlQuery = CreateCAMLQuery(intChapterID, "Master_ID", "Number");
                    DeleteItems(strSiteURL, DWBCHAPTERAUDITLIST, strAuditCamlQuery);

                    /// Delete all chapters in the Book
                    DeleteItems(strSiteURL, DWBCHAPTERLIST, string.Format(GETCHAPTERS, bookID.ToString()));
                }
            }             
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes the chapter pages.
        /// </summary>
        /// <param name="chapterIds">The chapter ids.</param>
        private void DeleteChapterPages(int[] chapterIds)
        {
            int[] intChapterPageID;
            string strCamlQuery;
            string strDocumentQuery;
            try
            {
                foreach (int intIndex in chapterIds)
                {
                    /// Retrive pages for each chapter
                    intChapterPageID = GetIDs(strSiteURL, DWBCHAPTERPAGESMAPPINGLIST, string.Format(GETCHAPTERPAGES, intIndex.ToString()), @" <FieldRef Name='Chapter_ID' /> <FieldRef Name='ID' />");

                    if (intChapterPageID != null && intChapterPageID.Length > 0)
                    {
                        /// Create CAML query to delete Audit for all pages in a chapter
                        strCamlQuery = CreateCAMLQuery(intChapterPageID, "Master_ID", "Number");

                        /// Delete Audit for all pages in a chapter.
                        DeleteItems(strSiteURL, DWBCHAPTERPAGESMAPPINGAUDITLIST, strCamlQuery);

                        /// Create CAML query to delete Comment, Narrative, StoryBoard for all pages in a chapter
                        strCamlQuery = CreateCAMLQuery(intChapterPageID, "Page_ID", "Number");

                        /// Delete Comments for all pages in a Chapter
                        DeleteItems(strSiteURL, DWBCOMMENT, strCamlQuery);

                        /// Delete Narrative for all pages in a Chapter
                        DeleteItems(strSiteURL, DWBNARRATIVES, strCamlQuery);

                        /// Delete StoryBoard for all pages in a Chapter
                        DeleteItems(strSiteURL, DWBSTORYBOARD, strCamlQuery);

                        /// Create CAML query to delete document for all pages in a chapter
                        strDocumentQuery = CreateCAMLQuery(intChapterPageID, "PageID", "Number");
                        /// Delete Type 3 Documents for all pages in a Chapter
                        DeleteDocument(strSiteURL, USERDEFINEDDOCUMENTLIST, strDocumentQuery);
                        /// Delete Type 2 Documents for all pages in a Chapter
                        DeleteDocument(strSiteURL, PUBLISHEDDOCUMENTLIST, strDocumentQuery);

                        /// Delete Pages in a chapter
                        DeleteItems(strSiteURL, DWBCHAPTERPAGESMAPPINGLIST, string.Format(GETCHAPTERPAGES, intIndex.ToString()));
                    }
                }

            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Deletes the items.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="camlQuery">The caml query.</param>
        private void DeleteItems(string siteURL, string listName, string camlQuery)
        {
            SPListItemCollection objlistItemCollection;
            SPList list;
            SPQuery query;
            int intIndex = 0;
            try
            {
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
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes the document.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="docLibraryName">Name of the doc library.</param>
        /// <param name="camlQuery">The caml query.</param>
        private void DeleteDocument(string siteURL, string docLibraryName, string camlQuery)
        {
            SPList docLibList;
            SPListItemCollection docLibItemCollection = null;
            SPQuery query = new SPQuery();
            int intIndex = 0;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteURL))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            docLibList = web.Lists[docLibraryName];
                            query.Query = camlQuery;
                            docLibItemCollection = docLibList.GetItems(query);

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
                });
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Creates the caml query without where.
        /// </summary>
        /// <param name="selectedID">The selected ID.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <returns></returns>
        private string CreateCamlQueryWithoutWhere(string selectedID, string fieldName, string fieldType)
        {
            StringBuilder strCamlQueryBuilder = new StringBuilder();
            string[] strSelectedIDs = null;

            try
            {
                if (string.IsNullOrEmpty(selectedID))
                {
                    /// return if the selected ID is empty
                    return string.Empty;
                }
                strSelectedIDs = selectedID.Split(';');
                if (strSelectedIDs.Length == 2)
                {
                    strCamlQueryBuilder.Append(@"<Eq><FieldRef Name='" + fieldName + "' /><Value Type='" + fieldType + "'>" +
                        strSelectedIDs[0] + "</Value></Eq>");
                    return strCamlQueryBuilder.ToString();
                }
                if (strSelectedIDs.Length > 2)
                {
                    for (int intIndex = 0; intIndex < strSelectedIDs.Length - 1; intIndex++)
                    {
                        if (intIndex != 0)
                            strCamlQueryBuilder.Insert(0, "<Or>");
                        strCamlQueryBuilder.Append(@"<Eq><FieldRef Name='" + fieldName + "' /><Value Type='" + fieldType + "'>" +
                            strSelectedIDs[intIndex] + "</Value></Eq>");
                        if (intIndex != 0)
                            strCamlQueryBuilder.Append("</Or>");
                    }
                }

                return strCamlQueryBuilder.ToString();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Creates the CAML query.
        /// </summary>
        /// <param name="selectedID">The selected ID.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <returns></returns>
        private string CreateCAMLQuery(int[] selectedID, string fieldName, string fieldType)
        {
            StringBuilder strCamlQueryBuilder = new StringBuilder();
            try
            {
                if (selectedID == null || selectedID.Length == 0)
                {
                    // return if the selected ID is empty
                    return string.Empty;
                }
                //else
                //{
                //    strSelectedIDs = selectedID.Split(';');
                //}
                if (selectedID.Length == 1)
                {
                    strCamlQueryBuilder.Append(@"<Where><Eq><FieldRef Name='" + fieldName + "' /><Value Type='" + fieldType + "'>" +
                        selectedID[0].ToString() + "</Value></Eq></Where>");
                    return strCamlQueryBuilder.ToString();
                }
                if (selectedID.Length > 1)
                {
                    for (int intIndex = 0; intIndex < selectedID.Length; intIndex++)
                    {
                        if (intIndex != 0)
                            strCamlQueryBuilder.Insert(0, "<Or>");
                        strCamlQueryBuilder.Append(@"<Eq><FieldRef Name='" + fieldName + "' /><Value Type='" + fieldType + "'>" +
                            selectedID[intIndex].ToString() + "</Value></Eq>");
                        if (intIndex != 0)
                            strCamlQueryBuilder.Append("</Or>");
                    }
                    strCamlQueryBuilder.Insert(0, @"<Where>");
                    strCamlQueryBuilder.Append(@"</Where>");

                }
                return strCamlQueryBuilder.ToString();
            }
            catch
            {
                throw;
            }
          
        }
        #region Data Retrival/Operations methods

        /// <summary>
        /// Reads the list.
        /// </summary>
        /// <param name="parentSiteUrl">The parent site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="queryString">The query string.</param>
        /// <param name="fieldstoView">The fieldsto view.</param>
        /// <returns></returns>
        private DataTable ReadList(string parentSiteUrl, string listName, string queryString, string fieldstoView)
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
        /// Reads the list.
        /// </summary>
        /// <param name="parentSiteUrl">The parent site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="queryString">The query string.</param>
        /// <param name="fieldstoView">The fieldsto view.</param>
        /// <returns></returns>
        private int[] GetIDs(string parentSiteUrl, string listName, string queryString, string fieldstoView)
        {
            DataTable dtListItems = null;
            SPList list;
            SPQuery query;
            int[] intIDs = null;
            int intRowCount;
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
                                intRowCount = dtListItems.Rows.Count;
                                if (dtListItems != null && intRowCount > 0)
                                {
                                    intIDs = new int[intRowCount];
                                    for (int intRowIndex = 0; intRowIndex < intRowCount; intRowIndex++)
                                    {
                                        intIDs[intRowIndex] = Convert.ToInt32(dtListItems.Rows[intRowIndex]["ID"]);
                                    }
                                }
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
            return intIDs;
        }

        #endregion

        #endregion
    }
}

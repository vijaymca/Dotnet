#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: WellBookBLL.cs
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DWB.DataAccessLayer;
using System.Data;
using System.Collections;
using Shell.SharePoint.DREAM.Utilities;
using Microsoft.SharePoint;
using System.Xml;

namespace Shell.SharePoint.DWB.BusinessLogicLayer
{
    /// <summary>
    /// BLL Class for Well Book
    /// </summary>
    public class WellBookBLL
    {

        #region Declarations
        WellBookDAL objWellBookDAL;
        CommonBLL objCommonBLL;
        WellBookChapterDAL objWellBookChapterDAL;
        CommonDAL objCommonDAL;
        const string DWBBOOKLIST = "DWB Books";
        const string DWBWELLBOOKAUDITLIST = "DWB Books Audit Trail";
        const string DWBCHAPTERLIST = "DWB Chapter";
        const string CHAPTERAUDITLIST = "DWB Chapter Audit Trail";
        const string CHAPTERPAGESMAPPINGLIST = "DWB Chapter Pages Mapping";
        const string CHAPTERPAGESMAPPINGAUDITLIST = "DWB Chapter Pages Mapping Audit Trail";
        const string DWBNARRATIVES = "DWB Narratives";
        const string DWBSTORYBOARD = "DWB StoryBoard";
        const string DWBCOMMENT = "DWB Comment";
        const string CONNECTIONLIST = "DWB Source";
        const string ASSETTEAMMAPPING = "DWB User";
        const string DWBTEAMSTAFF = "DWB Team Staff";
        const string USERDEFINEDDOCUMENTLIST = "DWB UserDefined Documents";
        const string PUBLISHEDDOCUMENTLIST = "DWB Published Documents";

        const string BOOKACTION_PUBLISH = "pdf";
        const string BOOKACTION_PRINT = "print";
        const string BOOKACTION_WELLBOOKTOC = "treeview";

        const string IDCOLUMN = "ID";
        const string PAGENAMECOLUMN = "Page_Name";
        const string CHAPTERIDCOLUMN = "Chapter_ID";
        const string CHAPTERNAMECOLUMN = "ChapterName";
        const string TITLECOLUMN = "Title";
        const string DISCIPLINECOLUMN = "Discipline";
        const string IDCOLUMNTYPE = "Counter";

        const string WELLBOREHEADERREPORTNAME = "WellboreHeader";
        const string WELLHISTORYREPORTNAME = "WellHistory";
        const string PREPRODRFTREPORTNAME = "PreProdRFT";
        const string WELLSUMMARYRREPORTNAME = "WellSummary";

        const string AUDITACTIONSIGNEDOFF = "5";
        const string AUDITACTIONUNSIGNEDOFF = "6";

        const string STATUSSIGNEDOFF = "yes";
        const string STATUSUNSIGNEDOFF = "no";
		const string DATEFORMAT = "Date Format";
        #region DREAM 4.0 - eWB2.0
        const string DWBUSER = "DWB User";
       
        #endregion
        #endregion

        #region Public Methods
        /// <summary>
        /// Updates the list entry.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listEntry">The list entry.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="type">The type.</param>
        public void UpdateListEntry(string siteURL, ListEntry listEntry, string auditListName, string listName,
            string userName, string actionPerformed)
        {
            objWellBookDAL = new WellBookDAL();
            objWellBookDAL.UpdateListEntry(siteURL, listEntry, listName, auditListName, userName,
                actionPerformed);
        }

        /// <summary>
        /// Added By Gopinath
        /// Date : 22-11-2010
        /// To save the bacth import log in sharepoint list.
        /// </summary>
        /// <param name="siteURL">string</param>
        /// <param name="listName">string</param>
        /// <param name="batchImportLog">DataTable</param>
        public void SaveBatchImportLog(string siteURL, string listName, DataTable batchImportLog)
        {
            objWellBookDAL = new WellBookDAL();
            objWellBookDAL.SaveBatchImportLog(siteURL, listName, batchImportLog);
        }

        /// <summary>
        /// Added By Gopinath
        /// Date : 22-11-2010
        /// Clear the Batch Import sharepoint list at start of batch import long running process.
        /// </summary>
        /// <param name="siteURL"></param>
        /// <param name="listName"></param>
        public void ClearBatchImportLog(string siteURL, string listName)
        {
            objWellBookDAL = new WellBookDAL();
            objWellBookDAL.ClearBatchImportLog(siteURL, listName);
        }

        /// <summary>
        /// Gets the Well Book Details
        /// </summary>
        /// <param name="siteURL"></param>
        /// <param name="listName"></param>
        /// <param name="queryString"></param>
        /// <returns>ListEntry</returns>
        public ListEntry GetWellBookDetail(string siteURL, string listName, string CAMLQuery)
        {
            objWellBookDAL = new WellBookDAL();
            return (objWellBookDAL.GetWellBookDetail(siteURL, listName, CAMLQuery));
        }

        /// <summary>
        /// Get List Details
        /// </summary>
        /// <param name="siteURL">Site URL.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="queryString">CAML Query.</param>
        /// <returns>DataTable</returns>
        public DataTable GetListDetails(string siteURL, string listName, string CAMLQuery)
        {
            objCommonDAL = new CommonDAL();
            return objCommonDAL.ReadList(siteURL, listName, CAMLQuery);
        }

        /// <summary>
        /// Gets the Collections of list item which can be directly binded
        /// to control.
        /// </summary>
        /// <param name="strSiteURL">Site URL.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="strCAMLQuery">CAML Query.</param>     
        /// <returns>Dictionary Object.</returns>        
        public Dictionary<string, string> GetListItems(string strSiteURL, string listName, string CAMLQuery)
        {
            Dictionary<string, string> listItemCollection = null;
            switch (listName)
            {
                case DWBTEAMSTAFF:
                    listItemCollection = GetAssetTeamMappping(strSiteURL, listName, CAMLQuery);
                    break;
                default:
                    break;
            }

            return listItemCollection;

        }

        /// <summary>
        /// Gets the owners list
        /// </summary>
        /// <param name="siteURL">Site URL.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="queryString">CAML Query.</param>     
        /// <returns>Dictionary Object.</returns>
        public Dictionary<string, string> GetPageOwnerList(string siteURL, string listName, string CAMLQuery)
        {
            Dictionary<string, string> listItemCollection = null;
            DataTable dtResultDatatable = null;
            string strCAMLQuery = string.Empty;
            StringBuilder strChapterId = new StringBuilder();
            objCommonDAL = new CommonDAL();
            string strViewFields = string.Empty;
            strViewFields = @"<FieldRef Name='ID' />";
            dtResultDatatable = objCommonDAL.ReadList(siteURL, listName, CAMLQuery, strViewFields);

            if (dtResultDatatable != null && dtResultDatatable.Rows.Count > 0)
            {
                for (int intRowIndex = 0; intRowIndex < dtResultDatatable.Rows.Count; intRowIndex++)
                {
                    strChapterId.Append(Convert.ToString(dtResultDatatable.Rows[intRowIndex][IDCOLUMN]));
                    strChapterId.Append(";");
                }
                objCommonBLL = new CommonBLL();
                strCAMLQuery = objCommonBLL.CreateCAMLQuery(strChapterId.ToString(), "Chapter_ID", "Number", "Owner");
                if (string.IsNullOrEmpty(CAMLQuery))
                    return listItemCollection;
                dtResultDatatable = objCommonDAL.ReadList(siteURL, CHAPTERPAGESMAPPINGLIST, strCAMLQuery);
                if (dtResultDatatable != null && dtResultDatatable.Rows.Count > 0)
                {
                    listItemCollection = new Dictionary<string, string>();

                    for (int intRowIndex = 0; intRowIndex < dtResultDatatable.Rows.Count; intRowIndex++)
                    {

                        if ((!listItemCollection.ContainsValue(Convert.ToString(dtResultDatatable.Rows[intRowIndex]["Owner"]))
                            && (!string.IsNullOrEmpty(Convert.ToString(dtResultDatatable.Rows[intRowIndex]["Owner"])))))
                        {
                            listItemCollection.Add(Convert.ToString(dtResultDatatable.Rows[intRowIndex][IDCOLUMN]), Convert.ToString(dtResultDatatable.Rows[intRowIndex]["Owner"]));
                        }
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
        /// Gets the PageNames List
        /// Added By: Praveena  
        /// Date:03/09/2010
        /// Reason: For module Add additional attributes to change page owner table
        /// </summary>
        /// <param name="siteURL">Site URL.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="queryString">CAML Query.</param>     
        /// <returns>Dictionary Object.</returns>
        public Dictionary<string, string> GetPageNamesList(string siteURL, string listName, string CAMLQuery)
        {

            Dictionary<string, string> listItemCollection = null;
            DataTable dtResultDatatable = null;
            string strCAMLQuery = string.Empty;
            StringBuilder strChapterId = new StringBuilder();
            objCommonDAL = new CommonDAL();
            string strViewFields = string.Empty;
            strViewFields = @"<FieldRef Name='ID' />";

            //Get the chapter details
            dtResultDatatable = objCommonDAL.ReadList(siteURL, listName, CAMLQuery, strViewFields);
            if (dtResultDatatable != null && dtResultDatatable.Rows.Count > 0)
            {
                for (int intRowIndex = 0; intRowIndex < dtResultDatatable.Rows.Count; intRowIndex++)
                {
                    strChapterId.Append(Convert.ToString(dtResultDatatable.Rows[intRowIndex][IDCOLUMN]));
                    strChapterId.Append(";");
                }
                objCommonBLL = new CommonBLL();
                strCAMLQuery = objCommonBLL.CreateCAMLQuery(strChapterId.ToString(), "Chapter_ID", "Number");
                if (string.IsNullOrEmpty(CAMLQuery))
                    return listItemCollection;

                strViewFields = @"<FieldRef Name='Page_Name' /><FieldRef Name='Discipline' />";

                //Get the Page details by passing chapter Ids
                dtResultDatatable = objCommonDAL.ReadList(siteURL, CHAPTERPAGESMAPPINGLIST, strCAMLQuery, strViewFields);
                if (dtResultDatatable != null && dtResultDatatable.Rows.Count > 0)
                {
                    listItemCollection = new Dictionary<string, string>();
                    DataView dvResult = dtResultDatatable.DefaultView;

                    //PageName and Discipline are store into the dictionary collection.
                    dtResultDatatable = dvResult.ToTable(true, "Page_Name", "Discipline");
                    for (int intRowIndex = 0; intRowIndex < dtResultDatatable.Rows.Count; intRowIndex++)
                    {
                        if ((!listItemCollection.ContainsKey(Convert.ToString(dtResultDatatable.Rows[intRowIndex][PAGENAMECOLUMN]))) &&
                             (!string.IsNullOrEmpty(Convert.ToString(dtResultDatatable.Rows[intRowIndex][PAGENAMECOLUMN]))) &&
                             (!string.IsNullOrEmpty(Convert.ToString(dtResultDatatable.Rows[intRowIndex][DISCIPLINECOLUMN]))))
                        {
                            listItemCollection.Add(Convert.ToString(dtResultDatatable.Rows[intRowIndex][PAGENAMECOLUMN]), Convert.ToString(dtResultDatatable.Rows[intRowIndex][DISCIPLINECOLUMN]));
                        }
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
        /// Gets the Disciplines List
        /// Added By: Gopinath  
        /// Date:3/09/2010
        /// Reason: To get Disciplines List
        /// </summary>
        /// <param name="siteURL">Site URL.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="queryString">CAML Query.</param>     
        /// <returns>Dictionary Object.</returns>
        public Dictionary<string, string> GetDisciplinesList(string siteURL, string listName, string CAMLQuery)
        {
            Dictionary<string, string> listItemCollection = null;
            DataTable dtResultDatatable = null;
            string strCAMLQuery = string.Empty;
            StringBuilder strChapterId = new StringBuilder();
            objCommonDAL = new CommonDAL();
            string strViewFields = string.Empty;
            strViewFields = @"<FieldRef Name='ID' />";
            dtResultDatatable = objCommonDAL.ReadList(siteURL, listName, CAMLQuery, strViewFields);

            if (dtResultDatatable != null && dtResultDatatable.Rows.Count > 0)
            {
                for (int intRowIndex = 0; intRowIndex < dtResultDatatable.Rows.Count; intRowIndex++)
                {
                    strChapterId.Append(Convert.ToString(dtResultDatatable.Rows[intRowIndex][IDCOLUMN]));
                    strChapterId.Append(";");
                }
                objCommonBLL = new CommonBLL();
                strCAMLQuery = objCommonBLL.CreateCAMLQuery(strChapterId.ToString(), "Chapter_ID", "Number");
                if (string.IsNullOrEmpty(CAMLQuery))
                    return listItemCollection;
                dtResultDatatable = objCommonDAL.ReadList(siteURL, CHAPTERPAGESMAPPINGLIST, strCAMLQuery);

                if (dtResultDatatable != null && dtResultDatatable.Rows.Count > 0)
                {
                    listItemCollection = new Dictionary<string, string>();

                    for (int intRowIndex = 0; intRowIndex < dtResultDatatable.Rows.Count; intRowIndex++)
                    {
                        if ((!listItemCollection.ContainsKey(Convert.ToString(dtResultDatatable.Rows[intRowIndex][IDCOLUMN])) &&
                            (!listItemCollection.ContainsValue(Convert.ToString(dtResultDatatable.Rows[intRowIndex][DISCIPLINECOLUMN])))
                            && (!string.IsNullOrEmpty(Convert.ToString(dtResultDatatable.Rows[intRowIndex][DISCIPLINECOLUMN])))))
                        {
                            listItemCollection.Add(Convert.ToString(dtResultDatatable.Rows[intRowIndex][IDCOLUMN]), Convert.ToString(dtResultDatatable.Rows[intRowIndex][DISCIPLINECOLUMN]));
                        }

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
        /// Get Pages for the selected Owner
        /// Modified By: Praveena  
        /// Date:03/09/2010
        /// Reason: For module Add additional attributes to change page owner table
        /// </summary>
        /// <param name="siteURL">Site URL.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="queryString">CAML Query.</param>     
        /// <param name="username">Windows User ID of the selected Owner</param>
        /// <returns>DataTable</returns>
        public DataTable GetPagesForOwner(string siteURL, string listName, string CAMLQuery, string username, string pageName, string terminatedStatus, System.Text.StringBuilder chapterNames)
        {
            string strViewFields = string.Empty;
            strViewFields = @"<FieldRef Name='ID' /><FieldRef Name='Title' />";
            DataTable dtResultDatatable = null;
            DataView dvresultdataview = null;
            string strCAMLQuery = string.Empty;
            StringBuilder strChapterId = new StringBuilder();
            Dictionary<string, string> listItemCollection = null;
            objCommonDAL = new CommonDAL();
            dtResultDatatable = objCommonDAL.ReadList(siteURL, listName, CAMLQuery, strViewFields);
            if (dtResultDatatable != null && dtResultDatatable.Rows.Count > 0)
            {
                listItemCollection = new Dictionary<string, string>();
                for (int intRowIndex = 0; intRowIndex < dtResultDatatable.Rows.Count; intRowIndex++)
                {
                    strChapterId.Append(Convert.ToString(dtResultDatatable.Rows[intRowIndex][IDCOLUMN]));
                    strChapterId.Append(";");
                    if ((!listItemCollection.ContainsValue(Convert.ToString(dtResultDatatable.Rows[intRowIndex][TITLECOLUMN]))
                        && (!string.IsNullOrEmpty(Convert.ToString(dtResultDatatable.Rows[intRowIndex][TITLECOLUMN])))))
                    {
                        listItemCollection.Add(Convert.ToString(dtResultDatatable.Rows[intRowIndex][IDCOLUMN]), Convert.ToString(dtResultDatatable.Rows[intRowIndex][TITLECOLUMN]));
                    }
                }
                objCommonBLL = new CommonBLL();
                strCAMLQuery = objCommonBLL.CreateCAMLQuery(strChapterId.ToString(), CHAPTERIDCOLUMN, "Number");
                strViewFields = "<FieldRef Name='Connection_Type' /><FieldRef Name='Page_Name' /><FieldRef Name='Discipline' /><FieldRef Name='Owner' /><FieldRef Name='Empty' /><FieldRef Name='Sign_Off_Status' /><FieldRef Name='Chapter_ID' /><FieldRef Name='Terminate_Status' />";
                dtResultDatatable = objCommonDAL.ReadList(siteURL, CHAPTERPAGESMAPPINGLIST,
                   strCAMLQuery, strViewFields);

                if (dtResultDatatable != null && dtResultDatatable.Rows.Count > 0)
                {
                    Dictionary<string, string> ChapterDetails = listItemCollection;
                    DataColumn dcChapterName = dtResultDatatable.Columns.Add(CHAPTERNAMECOLUMN, typeof(string));
                    string str = string.Empty;
                    foreach (DataRow dtRow in dtResultDatatable.Rows)
                    {
                        if (ChapterDetails.ContainsKey(dtRow[CHAPTERIDCOLUMN].ToString()))
                        {
                            ChapterDetails.TryGetValue(dtRow[CHAPTERIDCOLUMN].ToString(), out str);
                            dtRow[CHAPTERNAMECOLUMN] = str;
                        }
                    }
                    dvresultdataview = dtResultDatatable.DefaultView;
                    //to apply filters to dataview
                    dvresultdataview = ApplyFilters(dvresultdataview, username, pageName, chapterNames, terminatedStatus);
                }
            }
            if (dvresultdataview != null)
            {
                return dvresultdataview.Table;
            }
            else
            {
                return dtResultDatatable;
            }
        }

        /// <summary>
        /// Gets the onwers of the team.
        /// </summary>
        /// <param name="siteURL">Site URL.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="queryString">CAML Query.</param>     
        /// <returns>Dictionary object</returns>
        public Dictionary<string, string> GetOwnerForTeam(string siteURL, string listName, string CAMLQuery)
        {
            Dictionary<string, string> listItemCollection = null;
            DataTable dtResultDatatable = null;
            DataTable dtUserDetails = null;
            string strcamlQuery = string.Empty;
            StringBuilder strTeamId = new StringBuilder();
            objCommonDAL = new CommonDAL();
            dtResultDatatable = objCommonDAL.ReadList(siteURL, listName, CAMLQuery);
            if (dtResultDatatable != null && dtResultDatatable.Rows.Count > 0)
            {
                strcamlQuery = @"<Where><Eq><FieldRef Name='Team_ID' />
                 <Value Type='Number'>" + (Convert.ToString(dtResultDatatable.Rows[0]["Team_ID"])) + "</Value></Eq></Where>";
                dtResultDatatable = objCommonDAL.ReadList(siteURL, DWBTEAMSTAFF,
                   strcamlQuery);

                if (dtResultDatatable != null && dtResultDatatable.Rows.Count > 0)
                {
                    for (int intRowIndex = 0; intRowIndex < dtResultDatatable.Rows.Count; intRowIndex++)
                    {
                        strTeamId.Append(Convert.ToString(dtResultDatatable.Rows[intRowIndex]["User_ID"]));
                        strTeamId.Append(";");
                    }
                    objCommonBLL = new CommonBLL();
                    strcamlQuery = objCommonBLL.CreateCAMLQuery(strTeamId.ToString(), IDCOLUMN, IDCOLUMNTYPE, "DWBUserName");
                    dtUserDetails = objCommonDAL.ReadList(siteURL, DWBUSER, strcamlQuery);
                    listItemCollection = new Dictionary<string, string>();
                    for (int intRowIndex = 0; intRowIndex < dtUserDetails.Rows.Count; intRowIndex++)
                    {
                        if (!listItemCollection.ContainsKey(Convert.ToString(dtResultDatatable.Rows[intRowIndex]["ID"])))
                        {
                            listItemCollection.Add(Convert.ToString(dtUserDetails.Rows[intRowIndex][IDCOLUMN]), Convert.ToString(dtUserDetails.Rows[intRowIndex]["Windows_User_ID"]));
                        }
                    }
                }

            }
            if (dtResultDatatable != null)
            {
                dtResultDatatable.Dispose();
            }
            if (dtUserDetails != null)
            {
                dtUserDetails.Dispose();
            }
            return listItemCollection;
        }

        /// <summary>
        /// Updates the page owners
        /// </summary>
        /// <param name="siteURL">Site URL.</param>
        /// <param name="listEntry">ListEntry object.</param>
        /// <param name="auditListName">Name of the audit list.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="actionPerformed">The action performed.</param>
        public void UpdatePageOwner(string siteURL, ListEntry listEntry, string auditListName, string listName,
            string userName, string actionPerformed)
        {
            StringBuilder strPageId = new StringBuilder();
            objWellBookDAL = new WellBookDAL();
            objCommonBLL = new CommonBLL();
            objCommonDAL = new CommonDAL();
            string strCAMLQuery = string.Empty;

            #region DREAM 4.0 - eWB2.0 - Change discipline when page owner is changed
            strCAMLQuery = string.Format(@"<Where><Eq><FieldRef Name='Windows_User_ID' /><Value Type='Text'>{0}</Value></Eq></Where>", userName); 
            string strViewFields = @"<FieldRef Name='Windows_User_ID' /><FieldRef Name='ID' /><FieldRef Name='Discipline' /><FieldRef Name='Privileges' /><FieldRef Name='DWBUserName' />";
            DataTable dtUserDetails = null;
            string strDiscipline = string.Empty;
            dtUserDetails = objCommonDAL.ReadList(siteURL, DWBUSER, strCAMLQuery, strViewFields);

            if (dtUserDetails != null && dtUserDetails.Rows.Count > 0)
            {
                strDiscipline = Convert.ToString(dtUserDetails.Rows[0][DISCIPLINECOLUMN]);
            }
          
            #endregion
            objWellBookDAL.UpdatePageOwner(siteURL, listEntry, listName, auditListName, userName, actionPerformed,strDiscipline);
            for (int intRowIndex = 0; intRowIndex < listEntry.ChapterPagesMapping.Count; intRowIndex++)
            {
                strPageId.Append(listEntry.ChapterPagesMapping[intRowIndex].RowId);
                strPageId.Append(";");

            }

            strCAMLQuery = objCommonBLL.CreateCAMLQuery(strPageId.ToString(), "Page_ID", "Number");
            objWellBookDAL.UpdatePageOwner(siteURL, strCAMLQuery, DWBSTORYBOARD, userName);

            #region DREAM 4.0 - eWB2.0 - Change discipline when page owner is changed
            /// Update the "Disciplie" column for selected Page_IDs in DWB Story Board list
            strViewFields = @"<FieldRef Name='ID' /><FieldRef Name='Discipline' /><FieldRef Name='Page_ID' />";
            objCommonDAL.UpdateListItem(siteURL, DWBSTORYBOARD,DISCIPLINECOLUMN, Microsoft.SharePoint.SPFieldType.Text, strDiscipline, strCAMLQuery, strViewFields);
            /// Update the "Disciplie" column for selected Page_IDs in DWB Comments list
            objCommonDAL.UpdateListItem(siteURL, DWBCOMMENT, DISCIPLINECOLUMN, Microsoft.SharePoint.SPFieldType.Text, strDiscipline, strCAMLQuery, strViewFields);
            #endregion 
        }

        /// <summary>
        /// Get Pages for the Selected Book
        /// </summary>
        /// <param name="strParentSiteUrl">Site URL.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="strCamlQuery">CAML Query.</param>     
        /// <returns>ListEntry object</returns>
        public ListEntry GetBookPages(string siteURL, string listName, string CAMLQuery)
        {
            ListEntry objListEntry = null;
            DataTable dtResultTable = null;
            objWellBookChapterDAL = new WellBookChapterDAL();
            objCommonDAL = new CommonDAL();
            string strcamlQuery = string.Empty;
            int intRowId = 0;
            objListEntry = objWellBookChapterDAL.SetChapterPage(siteURL, listName, CAMLQuery);
            strcamlQuery = @"<Where><Eq><FieldRef Name='ID' />
                 <Value Type='Counter'>" + Convert.ToString(objListEntry.ChapterDetails.RowID) + "</Value></Eq></Where>";
            dtResultTable = objCommonDAL.ReadList(siteURL, DWBCHAPTERLIST,
               strcamlQuery);
            if (dtResultTable != null && dtResultTable.Rows.Count > 0)
            {
                int.TryParse(Convert.ToString(dtResultTable.Rows[0]["Book_ID"]), out intRowId);
                objListEntry.ChapterDetails.BookID = intRowId;
                objListEntry.ChapterDetails.ChapterTitle = Convert.ToString(dtResultTable.Rows[0][TITLECOLUMN]);
            }
            if (dtResultTable != null)
            {
                dtResultTable.Dispose();
            }
            return objListEntry;
        }

        /// <summary>
        /// Updates the Book Page details.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listEntry">The list entry.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="auditListName">Name of the audit list.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="actionPerformed">The action performed.</param>
        public void UpdateBookPage(string siteURL, ListEntry listEntry, string listName, string auditListName,
            string userName, string actionPerformed)
        {
            objWellBookDAL = new WellBookDAL();
            objWellBookDAL.UpdateBookPage(siteURL, listEntry, listName, auditListName, userName,
                actionPerformed);
        }

        /// <summary>
        /// Publishe the Well book and updates the list item.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="rowId">The row id.</param>
        public void PublishWellBook(string siteURL, string listName, int rowId)
        {
            objWellBookDAL = new WellBookDAL();
            objCommonDAL = new CommonDAL();
            objCommonBLL = new CommonBLL();
            DataTable dtResultTable = null;
            int intListItemRowId = 0;
            StringBuilder strID = new StringBuilder();
            string strCAMlQueryforPageId = string.Empty;
            Dictionary<string, string> listItemCollection = null;
            string strCamlQuery = string.Empty;

            listItemCollection = new Dictionary<string, string>();
            listItemCollection.Add("Sign_Off_Status", "No");
            /// Updating Sign_Off_Status column value to "No" in DWB Books list.
            objWellBookDAL.UpdateListItemValue(siteURL, listName, rowId, listItemCollection);

            strCamlQuery = "<Where><Eq><FieldRef Name='Book_ID' /><Value Type='Number'>" +
                  rowId.ToString() + "</Value></Eq></Where>";
            dtResultTable = objCommonDAL.ReadList(siteURL, DWBCHAPTERLIST, strCamlQuery);
            if (dtResultTable != null && dtResultTable.Rows.Count > 0)
            {
                for (int i = 0; i < dtResultTable.Rows.Count; i++)
                {
                    strID.Append(Convert.ToString(dtResultTable.Rows[i][IDCOLUMN]));
                    strID.Append(";");
                }
            }
            else
            {
                //the book does not have any chapters
                return;
            }

            /// Updating Sign_Off_Status column value to "No" in DWB Chapter Pages Mapping list.

            ///Build CAML Query to get all pages in the selected Book
            strCamlQuery = objCommonBLL.CreateCAMLQuery(strID.ToString(), "Chapter_ID", "Number");
            dtResultTable = objCommonDAL.ReadList(siteURL, CHAPTERPAGESMAPPINGLIST, strCamlQuery);
            strID.Remove(0, strID.Length);
            if (dtResultTable != null && dtResultTable.Rows.Count > 0)
            {
                listItemCollection.Clear();
                listItemCollection.Add("Sign_Off_Status", "No");
                for (int intRowIndex = 0; intRowIndex < dtResultTable.Rows.Count; intRowIndex++)
                {
                    int.TryParse(Convert.ToString(dtResultTable.Rows[intRowIndex][IDCOLUMN]), out intListItemRowId);
                    /// Updating Sign_Off_Status column value to "No" in DWB Chapter Pages Mapping list.
                    objWellBookDAL.UpdateListItemValue(siteURL, CHAPTERPAGESMAPPINGLIST, intListItemRowId, listItemCollection);
                    strID.Append(intListItemRowId.ToString());
                    strID.Append(";");

                }
            }


            /// Delete Comments for All Books Pages
            strCAMlQueryforPageId = objCommonBLL.CreateCAMLQuery(strID.ToString(), "Page_ID", "Number");
            objCommonDAL.DeleteAuditTrail(siteURL, DWBCOMMENT, strCAMlQueryforPageId);

            /// Delete audit trail of the Book.
            strCamlQuery = @"<Where><Eq><FieldRef Name='Master_ID' /><Value Type='Number'>" + rowId.ToString() + "</Value></Eq></Where>";
            objCommonDAL.DeleteAuditTrail(siteURL, DWBWELLBOOKAUDITLIST, strCamlQuery);


            if (dtResultTable != null)
            {
                dtResultTable.Dispose();
            }
        }

        /// <summary>
        /// Add Books as favorites         
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>                   
        /// <param name="favBookIds">Books IDs separated by ;</param>        
        /// <param name="listName">List Name.</param>  
        /// <param name="username">User ID</param>
        /// <param name="addToFavourite">True if Add to Favourites. False if Remove from Favorites.</param>
        public void AddToFavorites(string siteURL, string favBookIds, string listName, string username, bool addToFavourite)
        {
            objWellBookDAL = new WellBookDAL();

            Dictionary<string, string> listItemCollection = null;
            if (favBookIds.Length != 0)
            {

                listItemCollection = new Dictionary<string, string>();

                string strCAMLQuery = "<Where><Eq><FieldRef Name='Windows_User_ID' /><Value Type='Text'>" + username + "</Value></Eq></Where>";
                if (addToFavourite)
                {
                    listItemCollection.Add("FavoriteBooks", favBookIds);

                    WellBookDAL.UpdateListItemCollectionValues(siteURL, listName, strCAMLQuery, listItemCollection);
                }
                else
                {
                    /// Read the FavoriteBooks value from DWB User list
                    /// Remove the Book Ids in favBookIds
                    /// Update the FavoriteBooks in DWB User list with the new value
                    objCommonDAL = new CommonDAL();
                    DataTable dtUser = objCommonDAL.ReadList(siteURL, listName, strCAMLQuery);
                    if (dtUser != null && dtUser.Rows.Count > 0)
                    {
                        string strExistingFavouriteBooks = Convert.ToString(dtUser.Rows[0]["FavoriteBooks"]);
                        if (!string.IsNullOrEmpty(strExistingFavouriteBooks))
                        {
                            string[] strBookIds = favBookIds.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            for (int intIndex = 0; intIndex < strBookIds.Length; intIndex++)
                            {
                                if (strExistingFavouriteBooks.Contains(";" + strBookIds[intIndex] + ";"))
                                {
                                    strExistingFavouriteBooks = strExistingFavouriteBooks.Remove(strExistingFavouriteBooks.IndexOf(";" + strBookIds[intIndex] + ";"), strBookIds[intIndex].Length + 1);
                                    if (strExistingFavouriteBooks.Length == 1 && (string.Compare(strExistingFavouriteBooks, ";")) == 0)
                                    {
                                        strExistingFavouriteBooks = string.Empty;
                                    }
                                }
                            }

                            listItemCollection.Add("FavoriteBooks", strExistingFavouriteBooks);
                            WellBookDAL.UpdateListItemCollectionValues(siteURL, listName, strCAMLQuery, listItemCollection);
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Changes the Sign of status and the Chapter Page mapping list.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="signOffStatus">The sign off status.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="username">The username.</param>
        /// <param name="bookId">The book id.</param>
        /// <param name="actionPerformed">The action performed.</param>
        public void ChangeSignOffStatus(string siteURL, string signOffStatus, string listName, string username, string bookId, string actionPerformed)
        {
            objWellBookDAL = new WellBookDAL();
            objCommonDAL = new CommonDAL();
            objCommonBLL = new CommonBLL();
            StringBuilder strChapterPageId = new StringBuilder();
            Dictionary<string, string> listItemCollection =
                new Dictionary<string, string>();
            listItemCollection.Add("Sign_Off_Status", signOffStatus);
            string strCAMLQuery = "<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + bookId + "</Value></Eq></Where>";
            WellBookDAL.UpdateListItemCollectionValues(siteURL, listName,
                strCAMLQuery, listItemCollection);
            int intBookId = 0;
            if (!string.IsNullOrEmpty(bookId))
            {
                intBookId = int.Parse(bookId);
            }
            objCommonDAL.UpdateListAuditHistory(siteURL, DWBWELLBOOKAUDITLIST, intBookId, username, actionPerformed);

            #region Code Commented as per confirmation of UAT Feedback
            /// Update Signoff Status for all pages in Book
            //dtResult = GetPagesForBook(siteURL, bookId, "No");
            //if (dtResult != null && dtResult.Rows.Count > 0)
            //{
            //    for (int intRowIndex = 0; intRowIndex < dtResult.Rows.Count; intRowIndex++)
            //    {
            //        strChapterPageId.Append(Convert.ToString(dtResult.Rows[intRowIndex][IDCOLUMN]));
            //        strChapterPageId.Append(";");
            //    }
            //    strCAMLQuery = objCommonBLL.CreateCamlQueryWithoutWhere(strChapterPageId.ToString(), IDCOLUMN, IDCOLUMNTYPE);
            //    listItemCollection.Clear();
            //    listItemCollection.Add("Sign_Off_Status", signOffStatus);
            //    WellBookDAL.UpdateListItemCollectionValues(siteURL, chapterpageList, strCAMLQuery, listItemCollection);
            //}
            //if (dtResult != null)
            //{
            //    dtResult.Dispose();
            //}
            #endregion Code Commented as per UAT Feedback
        }

        /// <summary>
        /// Gets the Well Book Pages
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="bookId">The book id.</param>
        /// <param name="terminated">The terminated.</param>
        /// <returns></returns>
        public DataTable GetPagesForBook(string siteURL, string bookId, string terminated)
        {
            DataTable dtResult = null;
            objCommonDAL = new CommonDAL();
            objCommonBLL = new CommonBLL();
            StringBuilder strChapterId = new StringBuilder();
            string strCamlQuery = @"<Where><And><Eq><FieldRef Name='Book_ID' />
            <Value Type='Number'> " + bookId + "</Value></Eq><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>" + terminated + "</Value></Eq></And></Where>";
            dtResult = objCommonDAL.ReadList(siteURL, DWBCHAPTERLIST, strCamlQuery);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                for (int intRowIndex = 0; intRowIndex < dtResult.Rows.Count; intRowIndex++)
                {
                    strChapterId.Append(Convert.ToString(dtResult.Rows[intRowIndex][IDCOLUMN]));
                    strChapterId.Append(";");
                }
                strCamlQuery = objCommonBLL.CreateCamlQueryWithoutWhere(strChapterId.ToString(), "Chapter_ID", "Number");
                strChapterId.Remove(0, strChapterId.Length);
                strChapterId.Append(strCamlQuery);
                strChapterId.Append("<Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>" + terminated + "</Value></Eq></And></Where>");
                strChapterId.Insert(0, "<Where><And>");
                dtResult = objCommonDAL.ReadList(siteURL, CHAPTERPAGESMAPPINGLIST, strChapterId.ToString());

            }

            return dtResult;
        }

        /// <summary>
        /// Updates the Comments for the Selected Page
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="listName">List Name.</param>                
        /// <param name="auditListName">Name of Audit List.</param>
        /// <param name="listEntry">ListEntry object.</param>
        /// <param name="actionPerformed">ID of the Audit Action</param>
        /// <returns>bool</returns>
        public bool UpdatePageComments(string siteURL, string listName, string auditListName, ListEntry listEntry, string actionPerformed)
        {
            objWellBookChapterDAL = new WellBookChapterDAL();
            return objWellBookChapterDAL.UpdateComments(siteURL, listName, auditListName, listEntry, actionPerformed);
        }

        /// <summary>
        /// Updates the Sign of Status
        /// </summary>        
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="listName">List Name.</param>    
        /// <param name="signOffStatus">Yes/No.</param>
        /// <param name="pageId">Page ID.</param>
        /// <param name="actionperformed">ID of Audit Action.</param>
        public void UpdateWellBookPageSignOffStatus(string siteURL, string listName, string signOffStatus, int pageId, string actionPerformed)
        {

            objWellBookDAL = new WellBookDAL();
            Dictionary<string, string> listItemCollection =
              new Dictionary<string, string>();
            listItemCollection.Add("Sign_Off_Status", signOffStatus);
            string strCAMLQuery = "<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + pageId + "</Value></Eq></Where>";
            WellBookDAL.UpdateListItemCollectionValues(siteURL, listName, strCAMLQuery, listItemCollection);
            objCommonDAL = new CommonDAL();

            Shell.SharePoint.DREAM.Utilities.CommonUtility objCommonUtility = new Shell.SharePoint.DREAM.Utilities.CommonUtility();
            string strUserName = objCommonUtility.GetUserName();
            objCommonDAL.UpdateListAuditHistory(siteURL, CHAPTERPAGESMAPPINGAUDITLIST, pageId, strUserName, actionPerformed);
        }

        /// <summary>
        /// Generates the DWB book XML.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="rowId">The row id.</param>
        /// <param name="action">The action.</param>
        /// <param name="bookSelected">if set to <c>true</c> [book selected].</param>
        /// <param name="includeTOC">if set to <c>true</c> [include TOC].</param>
        /// <param name="includeStoryBoard">if set to <c>true</c> [include story board].</param>
        /// <returns></returns>
        public System.Xml.XmlDocument GenerateDWBBookXML(string siteURL, int rowId, string action, bool bookSelected, PrintOptions objPrintOptions)
        {
            BookInfo objBookInfo = null;
            System.Xml.XmlDocument xmlWellBookDetails = null;
            if (rowId > 0)
            {
                objBookInfo = SetBookDetailDataObject(siteURL, rowId.ToString(), action, bookSelected, objPrintOptions);
                objCommonBLL = new CommonBLL();
                xmlWellBookDetails = objCommonBLL.CreateWellBookDetailXML(objBookInfo);
            }
            return xmlWellBookDetails;
        }

        /// <summary>
        /// Sets the book detail data object.
        /// </summary>
        public BookInfo SetBookDetailDataObject(string siteURL, string bookID, string action, bool bookSelected, PrintOptions objPrintOptions)
        {
            string strCamlQuery = string.Empty;
            string strViewFields = string.Empty;
            DataTable dtBooks = null;
            DataRow objDataRow;
            BookInfo objBookInfo = null;
            try
            {
                objCommonDAL = new CommonDAL();
                objBookInfo = new BookInfo();
                objBookInfo.BookID = bookID;
                strCamlQuery = @"<Where><And><Eq><FieldRef Name='ID' /><Value Type='Number'>" +
                    bookID + "</Value></Eq><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq></And></Where>";
                strViewFields = @"<FieldRef Name='ID' /><FieldRef Name='Title' /><FieldRef Name='Owner' /><FieldRef Name='Team_ID' />";
                dtBooks = objCommonDAL.ReadList(siteURL, DWBBOOKLIST, strCamlQuery, strViewFields);
                for (int intIndex = 0; intIndex < dtBooks.Rows.Count; intIndex++)
                {
                    objDataRow = dtBooks.Rows[intIndex];
                    objBookInfo.BookName = objDataRow[TITLECOLUMN].ToString();
                    objBookInfo.BookOwner = objDataRow["Owner"].ToString();
                    objBookInfo.BookTeamID = objDataRow["Team_ID"].ToString();
                    objBookInfo.Action = action;
                    objBookInfo.IsPrintable = true;
                    objBookInfo.IsTOCApplicatble = objPrintOptions.IncludeTOC;
                    if (bookSelected)
                    {
                        objBookInfo.Chapters = SetChapterDetail(siteURL, bookID, action);
                        int intPageCount = 0;
                        if (objBookInfo.Chapters != null && objBookInfo.Chapters.Count > 0)
                        {
                            foreach (ChapterInfo objChapterInfo in objBookInfo.Chapters)
                            {
                                intPageCount += objChapterInfo.PageInfo.Count;
                            }
                            intPageCount += objBookInfo.Chapters.Count;
                        }
                        objBookInfo.PageCount = intPageCount;
                    }

                    objBookInfo.IsStoryBoardApplicable = objPrintOptions.IncludeStoryBoard;
                    objBookInfo.IsPageTitleApplicable = objPrintOptions.IncludePageTitle;
                    objBookInfo.IsBookTitleApplicable = objPrintOptions.IncludeBookTitle;
                }
            }
            catch (Exception)
            {
                throw;
            }
            if (dtBooks != null)
            {
                dtBooks.Dispose();
            }
            return objBookInfo;
        }

        /// <summary>
        /// Sets the chapter details.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="ChapterID">The chapter ID.</param>
        /// <param name="chapterSelected">if set to <c>true</c> [chapter selected].</param>
        /// <returns></returns>
        public ChapterInfo SetChapterDetails(string siteURL, string ChapterID, bool chapterSelected)
        {
            ChapterInfo objChapterInfo = null;
            string strCamlQuery = string.Empty;
            DataTable dtChapters = null;
            DataRow objDataRow;
            try
            {
                objCommonBLL = new CommonBLL();
                strCamlQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + ChapterID + "</Value></Eq></Where>";
                dtChapters = objCommonBLL.ReadList(siteURL, DWBCHAPTERLIST, strCamlQuery);

                for (int intIndex = 0; intIndex < dtChapters.Rows.Count; intIndex++)
                {
                    objChapterInfo = new ChapterInfo();
                    objDataRow = dtChapters.Rows[intIndex];
                    objChapterInfo.AssetType = objDataRow["Asset_Type"].ToString();
                    objChapterInfo.AssetValue = objDataRow["Asset_Value"].ToString();
                    objChapterInfo.ChapterID = objDataRow[IDCOLUMN].ToString();
                    objChapterInfo.ChapterTitle = objDataRow[TITLECOLUMN].ToString();
                    objChapterInfo.ActualAssetValue = objDataRow["Actual_Asset_Value"].ToString();
                    objChapterInfo.ColumnName = objDataRow["Column_Name"].ToString();
                    if (chapterSelected)
                    {
                        /// Call SetPageInfo method and assign the Page Collection                    
                        objChapterInfo.PageInfo = SetChapterPageInfo(siteURL, objChapterInfo.ChapterID, objChapterInfo.ActualAssetValue, objChapterInfo.ColumnName);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            if (dtChapters != null)
            {
                dtChapters.Dispose();
            }
            return objChapterInfo;
        }

        /// <summary>
        /// Returns the array of selected Page Info objects.
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="pageIds">PageIDs concatenated with "|".</param>
        /// <param name="chapterActualAssetValue">Actual Asset Value of Chapter.</param>
        /// <param name="columnName">Column Name of the Chapter.</param>
        /// <returns>ArrayList of PageInfo objects.</returns>
        public ArrayList SetSelectedPageInfo(string siteURL, string pageIds, string chapterActualAssetValue, string columnName)
        {
            /// Split the pageIds with "|" as splitter
            /// Format the CAML Query for the selected Pages
            /// Assign values to PageInfo object and add to Array list
            ArrayList arlPageInfo = null;
            PageInfo objPageInfo = null;
            string strCAMLQuery = string.Empty;
            string strViewFields = string.Empty;
            DataTable dtChapterPages = null;
            DataTable dtDocumentDetails = null;

            string strConnectionType = string.Empty;
            string strPageURL = string.Empty;
            string[] strConnectionTypeSplit = null;
            int intConnectionType = 0;
            if (!string.IsNullOrEmpty(pageIds) && pageIds.Length > 0)
            {
                objCommonBLL = new CommonBLL();
                strCAMLQuery = objCommonBLL.CreateCAMLQuery(pageIds, IDCOLUMN, IDCOLUMNTYPE);
                if (!string.IsNullOrEmpty(strCAMLQuery))
                {
                    strCAMLQuery = strCAMLQuery.Insert(0, "<OrderBy><FieldRef Name='Page_Sequence' Ascending='True'/></OrderBy>");
                }
                strViewFields = @"<FieldRef Name='Page_Sequence' /><FieldRef Name='ID' /><FieldRef Name='Title' /><FieldRef Name='Terminate_Status' /><FieldRef Name='Chapter_ID' /><FieldRef Name='Page_Name' /><FieldRef Name='Page_Actual_Name' /><FieldRef Name='Page_URL' /><FieldRef Name='Owner' /><FieldRef Name='Connection_Type' /><FieldRef Name='Sign_Off_Status' /><FieldRef Name='Asset_Type' />";
                objCommonDAL = new CommonDAL();
                dtChapterPages = objCommonDAL.ReadList(siteURL, CHAPTERPAGESMAPPINGLIST, strCAMLQuery, strViewFields);
                if (dtChapterPages != null && dtChapterPages.Rows.Count > 0)
                {
                    arlPageInfo = new ArrayList();
                    foreach (DataRow objDataRow in dtChapterPages.Rows)
                    {
                        objPageInfo = new PageInfo();
                        objPageInfo.PageID = objDataRow[IDCOLUMN].ToString();
                        objPageInfo.PageTitle = objDataRow["Page_Name"].ToString();
                        objPageInfo.PageActualName = objDataRow["Page_Actual_Name"].ToString();
                        objPageInfo.PageURL = objDataRow["Page_URL"].ToString();
                        if (objDataRow["Owner"] != null || objDataRow["Owner"] != DBNull.Value)
                        {
                            objPageInfo.PageOwner = objDataRow["Owner"].ToString();
                        }
                        strConnectionType = string.Empty;
                        strConnectionTypeSplit = null;
                        strConnectionType = objDataRow["Connection_Type"].ToString();
                        strConnectionTypeSplit = strConnectionType.Split("-".ToCharArray());
                        intConnectionType = 0;
                        if (strConnectionTypeSplit != null && strConnectionTypeSplit.Length > 0)
                        {
                            int.TryParse(strConnectionTypeSplit[0], out intConnectionType);
                        }
                        objPageInfo.ConnectionType = intConnectionType;
                        objPageInfo.SignOffStatus = objDataRow["Sign_Off_Status"].ToString();
                        objPageInfo.AssetType = objDataRow["Asset_Type"].ToString();
                        /// If connection type == 1, assign below properties
                        if (intConnectionType == 1)
                        {
                            objPageInfo.ActualAssetValue = chapterActualAssetValue;
                            objPageInfo.ColumnName = columnName;
                            /// Based on page URL assign "WellHistory/WellBoreHeader/Pre-Prod RFT" 
                            /// DWBWellHistoryReport.aspx
                            /// DWBPreProductionRFT.aspx
                            /// DWBWellboreHeader.aspx
                            strPageURL = string.Empty;
                            strPageURL = objDataRow["Page_URL"].ToString();
                            if (string.Compare(strPageURL, "DWBWellHistoryReport.aspx", true) == 0)
                            {
                                objPageInfo.ReportName = WELLHISTORYREPORTNAME;
                            }
                            else if (string.Compare(strPageURL, "DWBPreProductionRFT.aspx", true) == 0)
                            {
                                objPageInfo.ReportName = PREPRODRFTREPORTNAME;
                            }
                            else if (string.Compare(strPageURL, "DWBWellboreHeader.aspx", true) == 0)
                            {
                                objPageInfo.ReportName = WELLBOREHEADERREPORTNAME;
                            }
                            else if (string.Compare(strPageURL, "WellSummary.aspx", true) == 0)
                            {
                                objPageInfo.ReportName = WELLSUMMARYRREPORTNAME;
                            }
                        }

                        //Added by Praveena for module "Add Last Updated date"
                        strCAMLQuery = objCommonBLL.CreateCAMLQuery(pageIds, "PageID", "Number");
                        strViewFields = @"<FieldRef Name='PageID' /><FieldRef Name='Modified' />";
                        if (intConnectionType == 3)
                        {
                            dtDocumentDetails = objCommonBLL.ReadList(siteURL, USERDEFINEDDOCUMENTLIST, strCAMLQuery, strViewFields);
                        }
                        else if (intConnectionType == 2)
                        {
                            dtDocumentDetails = objCommonBLL.ReadList(siteURL, PUBLISHEDDOCUMENTLIST, strCAMLQuery, strViewFields);
                        }
                        if (dtDocumentDetails != null && dtDocumentDetails.Rows.Count > 0)
                        {
                            foreach (DataRow dtRow in dtDocumentDetails.Rows)
                            {
                                objPageInfo.LastUpdatedDate = GetDateTime(dtRow["Modified"].ToString());
                            }
                        }

                        arlPageInfo.Add(objPageInfo);
                    }
                }
            }
            if (dtChapterPages != null)
            {
                dtChapterPages.Dispose();
            }
            return arlPageInfo;
        }

        /// <summary>
        /// Update Sign Off Status
        /// Added By: Praveena  
        /// Date:11/09/2010
        /// Reason: For module Simplify Sign Off
        /// </summary>
        /// <param name="siteURL"></param>
        /// <param name="listName"></param>
        /// <param name="signOffStatus"></param>
        /// <param name="pageIDs"></param>
        /// <param name="actionPerformed"></param>
        public void UpdateBulkSignOffStatus(string siteURL, string listName, string signOffStatus, string pageIDs, string actionPerformed)
        {

            objWellBookDAL = new WellBookDAL();
            objCommonBLL = new CommonBLL();
            string strCAMLQuery = objCommonBLL.CreateCAMLQuery(pageIDs, "ID", "Number");
            WellBookDAL.UpdateBulkSignOffSatus(siteURL, listName, strCAMLQuery, signOffStatus);
            objCommonDAL = new CommonDAL();
            Shell.SharePoint.DREAM.Utilities.CommonUtility objCommonUtility = new Shell.SharePoint.DREAM.Utilities.CommonUtility();
            string strUserName = objCommonUtility.GetUserName();
            objCommonDAL.UpdateBulkListAuditHistory(siteURL, CHAPTERPAGESMAPPINGAUDITLIST, pageIDs, strUserName, actionPerformed);
        }

        /// <summary>
        /// To Get TypeIII Pages for selected Book
        /// Added By: Praveena  
        /// Date:16/09/2010
        /// Reason: For module Add additional attributes to change page owner table
        /// </summary>
        /// <param name="siteURL"></param>
        /// <param name="bookId"></param>
        /// <param name="terminated"></param>
        /// <returns></returns>
        public DataTable GetTypeIIIPagesForBook(string siteURL, string bookId, string terminated)
        {
            DataTable dtResult = null;
            objCommonDAL = new CommonDAL();
            objCommonBLL = new CommonBLL();
            StringBuilder strChapterId = new StringBuilder();
            string strCamlQuery = @"<Where><And><Eq><FieldRef Name='Book_ID' />
            <Value Type='Number'> " + bookId + "</Value></Eq><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>" + terminated + "</Value></Eq></And></Where>";
            dtResult = objCommonDAL.ReadList(siteURL, DWBCHAPTERLIST, strCamlQuery);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                for (int intRowIndex = 0; intRowIndex < dtResult.Rows.Count; intRowIndex++)
                {
                    strChapterId.Append(Convert.ToString(dtResult.Rows[intRowIndex][IDCOLUMN]));
                    strChapterId.Append(";");
                }
                strCamlQuery = objCommonBLL.CreateCamlQueryWithoutWhere(strChapterId.ToString(), "Chapter_ID", "Number");
                strChapterId.Remove(0, strChapterId.Length);
                strChapterId.Append(strCamlQuery);
                strChapterId.Append("<Eq><FieldRef Name='Connection_Type' /><Value Type='Text'>3 - User Defined Document</Value></Eq></And>");
                strChapterId.Append("<Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>" + terminated + "</Value></Eq></And></Where>");
                strChapterId.Insert(0, "<Where><And><And>");
                dtResult = objCommonDAL.ReadList(siteURL, CHAPTERPAGESMAPPINGLIST, strChapterId.ToString());

            }

            return dtResult;

        }

        /// <summary>
        /// To Update Batch Import Audit History
        /// </summary>
        /// <param name="siteURL"></param>
        /// <param name="bookID"></param>
        /// <param name="actionPerformed"></param>
        public void UpdateBatchImportAuditHistory(string siteURL, string bookID, string actionPerformed)
        {
            objCommonDAL = new CommonDAL();
            Shell.SharePoint.DREAM.Utilities.CommonUtility objCommonUtility = new Shell.SharePoint.DREAM.Utilities.CommonUtility();
            string strUserName = objCommonUtility.GetUserName();
            objCommonDAL.UpdateBatchImportAuditHistory(siteURL, DWBWELLBOOKAUDITLIST, bookID, strUserName, actionPerformed);
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Sets the chapter page info.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="chapterID">The chapter ID.</param>
        /// <param name="chapterActualAssetValue">The chapter actual asset value.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>ArrayList of Chapter Pages.</returns>
        private ArrayList SetChapterPageInfo(string siteURL, string chapterID, string chapterActualAssetValue, string columnName)
        {
            string strCamlQuery = string.Empty;
            string strViewFields = string.Empty;
            DataTable dtSelectedPages = null;
            DataRow objDataRow;
            PageInfo objPageInfo = null;
            ArrayList arlPageDetails = null;
            string strConnectionType = string.Empty;
            string[] strConnectionTypeSplit = null;
            int intConnectionType = 0;
            string strPageURL = string.Empty;
            try
            {
                objCommonDAL = new CommonDAL();
                strCamlQuery = @" <OrderBy><FieldRef Name='Page_Sequence' Ascending='True'/></OrderBy> <Where><And><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq><Eq><FieldRef Name='Chapter_ID' /><Value Type='Number'>" + chapterID + "</Value>" + "</Eq></And></Where>";
                strViewFields = @"<FieldRef Name='Page_Sequence' /><FieldRef Name='ID' /><FieldRef Name='Title' /><FieldRef Name='Terminate_Status' /><FieldRef Name='Chapter_ID' /><FieldRef Name='Page_Name' /><FieldRef Name='Page_Actual_Name' /><FieldRef Name='Page_URL' /><FieldRef Name='Owner' /><FieldRef Name='Connection_Type' /><FieldRef Name='Sign_Off_Status' /><FieldRef Name='Asset_Type' />";
                dtSelectedPages = objCommonDAL.ReadList(siteURL, CHAPTERPAGESMAPPINGLIST, strCamlQuery);

                arlPageDetails = new ArrayList();
                for (int intIndex = 0; intIndex < dtSelectedPages.Rows.Count; intIndex++)
                {
                    objPageInfo = new PageInfo();
                    objDataRow = dtSelectedPages.Rows[intIndex];
                    objPageInfo.PageID = objDataRow[IDCOLUMN].ToString();
                    objPageInfo.PageTitle = objDataRow["Page_Name"].ToString();
                    objPageInfo.PageActualName = objDataRow["Page_Actual_Name"].ToString();
                    objPageInfo.PageURL = objDataRow["Page_URL"].ToString();
                    if (objDataRow["Owner"] != null || objDataRow["Owner"] != DBNull.Value)
                    {
                        objPageInfo.PageOwner = objDataRow["Owner"].ToString();
                    }
                    strConnectionType = string.Empty;
                    strConnectionType = objDataRow["Connection_Type"].ToString();
                    strConnectionTypeSplit = null;
                    strConnectionTypeSplit = strConnectionType.Split("-".ToCharArray());
                    intConnectionType = 0;
                    if (strConnectionTypeSplit != null && strConnectionTypeSplit.Length > 0)
                    {
                        int.TryParse(strConnectionTypeSplit[0], out intConnectionType);
                    }
                    objPageInfo.ConnectionType = intConnectionType;
                    objPageInfo.SignOffStatus = objDataRow["Sign_Off_Status"].ToString();
                    objPageInfo.AssetType = objDataRow["Asset_Type"].ToString();
                    /// If connection type == 1, assign below properties
                    if (intConnectionType == 1)
                    {
                        objPageInfo.ActualAssetValue = chapterActualAssetValue;
                        objPageInfo.ColumnName = columnName;
                        /// Based on page URL assign "WellHistory/WellBoreHeader/Pre-Prod RFT" 
                        /// DWBWellHistoryReport.aspx
                        /// DWBPreProductionRFT.aspx
                        /// DWBWellboreHeader.aspx
                        strPageURL = string.Empty;
                        strPageURL = objDataRow["Page_URL"].ToString();
                        if (string.Compare(strPageURL, "DWBWellHistoryReport.aspx", true) == 0)
                        {
                            objPageInfo.ReportName = WELLHISTORYREPORTNAME;
                        }
                        else if (string.Compare(strPageURL, "DWBPreProductionRFT.aspx", true) == 0)
                        {
                            objPageInfo.ReportName = PREPRODRFTREPORTNAME;
                        }
                        else if (string.Compare(strPageURL, "DWBWellboreHeader.aspx", true) == 0)
                        {
                            objPageInfo.ReportName = WELLBOREHEADERREPORTNAME;
                        }
                        else if (string.Compare(strPageURL, "WellSummary.aspx", true) == 0)
                        {
                            objPageInfo.ReportName = WELLSUMMARYRREPORTNAME;
                        }
                    }
                    #region DREAM 4.0 - eWB 2.0
                    if (objDataRow["Empty"] != null)
                    {
                        if (objDataRow["Empty"].ToString().ToLowerInvariant().Equals("no"))
                        {
                            objPageInfo.IsEmpty = false;
                        }
                        else if (objDataRow["Empty"].ToString().ToLowerInvariant().Equals("yes"))
                        {
                            objPageInfo.IsEmpty = true;
                        }

                    }
                    #endregion 
                    arlPageDetails.Add(objPageInfo);
                }

                if (dtSelectedPages != null)
                {
                    dtSelectedPages.Dispose();
                }
                return arlPageDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the asset team mapping
        /// </summary>
        /// <param name="strSiteURL">Site URL.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="strCAMLQuery">CAML Query.</param>     
        /// <returns>Dictionary object.</returns>
        private Dictionary<string, string> GetAssetTeamMappping(string siteURL, string listName, string CAMLQuery)
        {
            Dictionary<string, string> listItemCollection = null;
            DataTable dtResultDatatable = null;
            objCommonDAL = new CommonDAL();
            dtResultDatatable = objCommonDAL.ReadList(siteURL, listName, CAMLQuery);
            if (dtResultDatatable != null && (dtResultDatatable.Rows.Count > 0))
            {
                listItemCollection = new Dictionary<string, string>();

                for (int intIndex = 0; intIndex < dtResultDatatable.Rows.Count; intIndex++)
                {
                    if (!listItemCollection.ContainsKey(dtResultDatatable.Rows[intIndex]["User_ID"].ToString()))
                    {
                        listItemCollection.Add(Convert.ToString(dtResultDatatable.Rows[intIndex]["User_ID"]), Convert.ToString(dtResultDatatable.Rows[intIndex][TITLECOLUMN]));
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
        /// Sets the collection of chapter detail.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="bookID">The book ID.</param>
        /// <param name="action">The action.</param>
        /// <returns>ArrayList</returns>
        private ArrayList SetChapterDetail(string siteURL, string bookID, string action)
        {
            string strCamlQuery = string.Empty;
            string strViewFields = string.Empty;
            DataTable dtChapters = null;
            DataRow objDataRow;
            ChapterInfo objChapterInfo = null;
            ArrayList arlChapterDetails = null;
            try
            {
                objCommonDAL = new CommonDAL();
                //strCamlQuery = @"<OrderBy><FieldRef Name='Chapter_Sequence' /></OrderBy><Where><And><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq><Eq><FieldRef Name='Book_ID' /><Value Type='Number'>" + bookID + "</Value>" + "</Eq></And></Where>";
                #region DREAM 4.0 - eWB2.0 - Deletion module
                /// Modify CAML query to exclude the chapters marked ad ToBeDeleted = Yes displaying from TreeView.
                strCamlQuery = @"<OrderBy><FieldRef Name='Chapter_Sequence' /></OrderBy><Where><And><And><Eq><FieldRef Name='Terminate_Status' /> <Value Type='Choice'>No</Value></Eq><Or><IsNull><FieldRef Name='ToBeDeleted' /></IsNull><Eq><FieldRef Name='ToBeDeleted' /><Value Type='Choice'>No</Value></Eq></Or></And><Eq><FieldRef Name='Book_ID' /><Value Type='Number'>" + bookID + "</Value>" + "</Eq></And></Where>";
                #endregion 
                strViewFields = @"<FieldRef Name='Chapter_Sequence' /><FieldRef Name='ID' /><FieldRef Name='Title' /><FieldRef Name='Terminate_Status' /><FieldRef Name='Book_ID' /><FieldRef Name='Actual_Asset_Value' /><FieldRef Name='Column_Name' /><FieldRef Name='Asset_Type' /><FieldRef Name='Asset_Value' />";
                dtChapters = objCommonDAL.ReadList(siteURL, DWBCHAPTERLIST, strCamlQuery, strViewFields);

                arlChapterDetails = new ArrayList();
                for (int intIndex = 0; intIndex < dtChapters.Rows.Count; intIndex++)
                {
                    objChapterInfo = new ChapterInfo();
                    objDataRow = dtChapters.Rows[intIndex];
                    objChapterInfo.AssetType = objDataRow["Asset_Type"].ToString();
                    objChapterInfo.AssetValue = objDataRow["Asset_Value"].ToString();
                    objChapterInfo.ChapterID = objDataRow[IDCOLUMN].ToString();
                    objChapterInfo.ChapterTitle = objDataRow[TITLECOLUMN].ToString();
                    objChapterInfo.ActualAssetValue = objDataRow["Actual_Asset_Value"].ToString();
                    objChapterInfo.ColumnName = objDataRow["Column_Name"].ToString();

                    objChapterInfo.PageInfo = SetChapterPageInfo(siteURL, objChapterInfo.ChapterID, objChapterInfo.ActualAssetValue, objChapterInfo.ColumnName);
                    if (objChapterInfo.PageInfo.Count > 0)
                    {
                        /// Add chapter to collection only if the Chapter contains >=1 pages.
                        arlChapterDetails.Add(objChapterInfo);
                        if (string.Compare(action, "print", true) == 0 || string.Compare(action, "pdf", true) == 0)
                        {
                            /// Set IsPrintable to true 
                            /// This method is called for Publish/Print complete book
                            objChapterInfo.IsPrintable = true;
                        }
                    }
                }
                if (dtChapters != null)
                {
                    dtChapters.Dispose();
                }
                return arlChapterDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// To Apply Filters to DataView
        /// Added By: Praveena  
        /// Date:26/09/2010
        /// Reason: For module Add additional attributes to change page owner table
        /// </summary>
        /// <param name="resultDataView">DataView</param>
        /// <param name="username">Owner Name</param>
        /// <param name="pageName">PageName</param>
        /// <param name="chapterNames">Chapter Name</param>
        /// <param name="terminatedStatus">Terminated Status</param>
        /// <returns></returns>
        private DataView ApplyFilters(DataView resultDataView, string username, string pageName, System.Text.StringBuilder chapterNames, string terminatedStatus)
        {
            StringBuilder strRowFilter = new StringBuilder();
            strRowFilter.Append("Owner = '" + username + "' AND Terminate_Status='" + terminatedStatus + "' ");
            if (!pageName.Equals("--Select All--"))
            {
                strRowFilter.Append(" AND Page_Name='" + pageName + "' ");
            }
            if (!string.IsNullOrEmpty(chapterNames.ToString()))
            {
                strRowFilter.Append(" AND ChapterName in (" + chapterNames + ") ");
            }
            strRowFilter.Append(" OR Isnull(Owner,'Null Column') = 'Null Column'");
            resultDataView.RowFilter = strRowFilter.ToString();
            resultDataView.Sort = "ChapterName ASC";
            return resultDataView;
        }

        /// <summary>
        /// Converts and Returns the Culture Formatted Date Time object of the date in string 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private string GetDateTime(string date)
        {
            string strDateFormat = string.Empty;
            strDateFormat = PortalConfiguration.GetInstance().GetKey(DATEFORMAT).ToString();
            DateTime dtmValue = DateTime.Parse(date);
            return dtmValue.ToString(strDateFormat);
        }
        #endregion
        #region PrintUpdates
        /// <summary>
        /// Updates the book print details.
        /// </summary>
        /// <param name="requestID">The request ID.</param>
        /// <param name="documentURL">The document URL.</param>
        /// <param name="parentSiteURL">The parent site URL.</param>
        /// <param name="currentUser">The current user.</param>
        /// <param name="isPublish">if set to <c>true</c> [is publish].</param>
        /// <param name="liveBookName">Name of the live book.</param>
        /// <param name="xmlDoc">The XML doc.</param>
        public void UpdateBookPrintDetails(string requestID, string documentURL, string parentSiteURL, string currentUser, bool isPublish, string liveBookName, XmlDocument xmlDoc)
        {
            ActiveDirectoryService objAds = new ActiveDirectoryService();
            string strEmailID = objAds.GetEmailID(currentUser.ToString());

            objCommonDAL = new CommonDAL();
            objCommonDAL.UpdateBookPrintDetails(requestID, documentURL, parentSiteURL, currentUser, "DWB Book Print details Library", isPublish, liveBookName, xmlDoc, strEmailID);
        }

        /// <summary>
        /// Updates the chapter print details.
        /// </summary>
        /// <param name="requestID">The request ID.</param>
        /// <param name="documentURL">The document URL.</param>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="userName">Name of the user.</param>
        public void UpdateChapterPrintDetails(string requestID, string documentURL, string siteURL, string userName)
        {
            objCommonDAL = new CommonDAL();
            objCommonDAL.UpdateChapterPrintDetails(requestID, documentURL, siteURL, userName, "DWB Chapter Print Details");
        }

        /// <summary>
        /// Sends the email to user on print.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <param name="parentSiteURL">The parent site URL.</param>
        /// <param name="strAddress">The STR address.</param>
        /// <param name="contextForMail">The context for mail.</param>
        public void SendEmailToUserOnPrint(string results, string parentSiteURL, string strAddress, SPContext contextForMail)
        {
            DataTable dtPrintChapterDetails = null;
            ActiveDirectoryService objADS = null;
            try
            {
                try
                {
                    objADS = new ActiveDirectoryService();
                }
                catch (Exception)
                {
                    objADS = new ActiveDirectoryService(contextForMail);
                }
                dtPrintChapterDetails = new DataTable();
                CommonUtility objUtility = new CommonUtility();
                objCommonDAL = new CommonDAL();
                string strToMailID = string.Empty;
                string strAccessLink = string.Empty;
                string strMessage = string.Empty;

                string strCamlQuery = @"<Where><Eq><FieldRef Name='RequestID' /><Value Type='Text'>" + results + "</Value></Eq></Where>";
                dtPrintChapterDetails = objCommonDAL.GetChapterPrintDetails(strCamlQuery, parentSiteURL, "DWB Chapter Print Details");

                if (dtPrintChapterDetails != null && dtPrintChapterDetails.Rows.Count > 0)
                {
                    /// Loop through the values in Chapter Print Details list.
                    foreach (DataRow dtRow in dtPrintChapterDetails.Rows)
                    {                        
                        try
                        {
                            strToMailID = objADS.GetEmailID(dtRow["UserName"].ToString());
                        }
                        catch (Exception)
                        {                            
                        }                        
                        strAccessLink = parentSiteURL + "/Pages/eWBPDFViewer.aspx?mode=chapter&requestID=" + results;

                        objUtility.SendMailforPrintUpdate(strToMailID, strAccessLink, parentSiteURL, strAddress, contextForMail);
                    }
                }                
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dtPrintChapterDetails != null)
                    dtPrintChapterDetails.Dispose();
            }
        }

        /// <summary>
        /// Gets the print document URL.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <param name="parentSiteURL">The parent site URL.</param>
        /// <param name="strAddress">The STR address.</param>
        /// <param name="contextForMail">The context for mail.</param>
        /// <param name="listName">Name of the list.</param>
        /// <returns></returns>
        public string GetPrintDocumentURL(string results, string parentSiteURL, string strAddress, SPContext contextForMail, string listName)
        {
            string strDocumentURL = string.Empty;
            DataTable dtPrintChapterDetails = null;
            try
            {
                string strCamlQuery = @"<Where><Eq><FieldRef Name='RequestID' /><Value Type='Text'>" + results + "</Value></Eq></Where>";
                dtPrintChapterDetails = objCommonDAL.GetChapterPrintDetails(strCamlQuery, parentSiteURL, listName);

                if (dtPrintChapterDetails != null && dtPrintChapterDetails.Rows.Count > 0)
                {
                    /// Loop through the values in Chapter Print Details list.
                    foreach (DataRow dtRow in dtPrintChapterDetails.Rows)
                    {                        
                        strDocumentURL = dtRow["DocumentURL"].ToString();                                                
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dtPrintChapterDetails != null)
                    dtPrintChapterDetails.Dispose();
            }
            return strDocumentURL;
        }        

        /// <summary>
        /// Updates the book publish details.
        /// </summary>
        /// <param name="requestID">The request ID.</param>
        /// <param name="documentURL">The document URL.</param>
        /// <param name="parentSiteURL">The parent site URL.</param>
        /// <param name="currentUser">The current user.</param>
        /// <param name="isPublish">if set to <c>true</c> [is publish].</param>
        /// <param name="liveBookName">Name of the live book.</param>
        /// <param name="xmlDoc">The XML doc.</param>
        /// <param name="bookID">The book ID.</param>
        public void UpdateBookPublishDetails(string requestID, string documentURL, string parentSiteURL, string currentUser, bool isPublish, string liveBookName, XmlDocument xmlDoc, int bookID)
        {
            ActiveDirectoryService objAds = new ActiveDirectoryService();
            string strEmailID = objAds.GetEmailID(currentUser.ToString());

            objCommonDAL = new CommonDAL();
            objCommonDAL.UpdateBookPublishDetails(requestID, documentURL, parentSiteURL, currentUser, "DWB Book Print details Library", isPublish, liveBookName, xmlDoc, strEmailID, bookID);
        }
        #endregion


    }
}

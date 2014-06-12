#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: CommonBLL.cs
#endregion

using System;
using System.Web;
using System.Text;
using System.Xml;
using System.Data;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DWB.DataAccessLayer;
using Shell.SharePoint.DWB.Business.DataObjects;
using System.IO;

namespace Shell.SharePoint.DWB.BusinessLogicLayer
{
    /// <summary>
    /// BLL class for Common Methods
    /// </summary>
    public class CommonBLL
    {
        #region Declarations
        CommonDAL objCommonDAL;
        CommonUtility objCommonUtility;
        private const string AUDIT_ACTION_ACTIVATE = "3";
        private const string AUDIT_ACTION_TERMINATE = "4";
        private const string AUDIT_ACTION_SIGNEDOFF = "5";
        private const string AUDIT_ACTION_UNSIGNEDOFF = "6";
        private const string USERLIST = "DWB User";
        private const string TEAMLIST = "DWB Team";
        private const string TEAMSTAFFLIST = "DWB Team Staff";
        private const string SESSIONTREEVIEWDATAOBJECT = "TreeViewDataObject";
        private const string YES = "Yes";
        private const string NO = "No";

        #region DREAM 4.0- eWB 2.0
        private const string DWBNARRATIVES = "DWB Narratives";
        private const string DWBSTORYBOARD = "DWB StoryBoard";
        private const string DWBCOMMENT = "DWB Comment";
        private const string USERDEFINEDDOCUMENTLIST = "DWB UserDefined Documents";
        private const string PUBLISHEDDOCUMENTLIST = "DWB Published Documents";
        private const string DWBTEMPLATEPAGESLIST = "DWB Template Page Mapping";
        private const string DWBTEMPLATECONFIGURATIONAUDIT = "DWB Template Page Mapping Audit Trail";
        private const string AUDITHISTORYCAMLQUERY = @"<Where><Eq><FieldRef Name='Master_ID' /><Value Type='Number'>{0}</Value></Eq></Where>";
        private const string CHAPTERPAGEDOCUMNETCAMEQUERY = @"<Where><Eq><FieldRef Name='PageID' /><Value Type='Number'>{0}</Value></Eq></Where>";
        private const string DWBTYPE1PAGE = "1 - Automated";
        private const string DWBTYPE2PAGE = "2 - Published Document";
        private const string DWBTYPE3PAGE = "3 - User Defined Document";

        #endregion 
        #endregion

        #region Public Methods
        /// <summary>
        /// Reads the list.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="CAMLQuery">The CAML query.</param>
        /// <returns>DataTable</returns>
        public DataTable ReadList(string siteURL, string listName, string CAMLQuery)
        {
            objCommonDAL = new CommonDAL();
            return objCommonDAL.ReadList(siteURL, listName, CAMLQuery);
        }

        /// <summary>
        /// Reads the list and returns the requested Fields.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="CAMLQuery">The CAML query.</param>
        /// <param name="viewFields">The view fields.</param>
        /// <returns>DataTable</returns>
        public DataTable ReadList(string siteURL, string listName, string CAMLQuery, string viewFields)
        {
            objCommonDAL = new CommonDAL();
            return objCommonDAL.ReadList(siteURL, listName, CAMLQuery, viewFields);
        }

        /// <summary>
        /// Checks the duplicate.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="value">The value.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="listName">Name of the list.</param>
        /// <returns>bool</returns>
        public bool CheckDuplicate(string siteURL, string value, string columnName, string listName)
        {
            objCommonDAL = new CommonDAL();
            return objCommonDAL.CheckDuplicate(siteURL, value, columnName, listName);
        }

        /// <summary>
        /// Checks for the Duplicate Name/title in Edit screens.
        /// </summary>
        /// <param name="siteURL">Site URL.</param>
        /// <param name="value">Value to be checked for duplicate.</param>
        /// <param name="columnName">Column Name in sharepoint list to look for duplicate.</param>
        /// <param name="listName">SharePoint list name.</param>
        /// <param name="rowID">ID of the Item.</param>
        /// <returns>bool</returns>
        public bool CheckDuplicate(string siteURL, string value, string columnName, string listName, string rowID)
        {
            objCommonDAL = new CommonDAL();
            return objCommonDAL.CheckDuplicate(siteURL, value, columnName, listName, rowID);
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
        public bool CheckDuplicateChapter(string siteURL, string listName, string chapterTitle, string chapterID, string bookID, string mode)
        {
            objCommonDAL = new CommonDAL();
            return objCommonDAL.CheckDuplicateChapter(siteURL, listName, chapterTitle, chapterID, bookID, mode);
        }

        /// <summary>
        /// Gets the XSL template.
        /// </summary>
        /// <param name="strType">type</param>
        /// <param name="strParentSiteUrl">parent site url.</param>
        /// <returns>XmlTextReader</returns>
        public XmlTextReader GetXSLTemplate(string type, string siteURL)
        {
            XmlTextReader objXmlTextReader = null;

            objCommonDAL = new CommonDAL();
            /// Get the XSLTemplate from sharepoint list
            objXmlTextReader = objCommonDAL.GetXSLTemplate(type, siteURL);

            return objXmlTextReader;
        }

        /// <summary>
        /// Activates the List items
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="rowID">The row ID.</param>
        /// <param name="auditlistName">Name of the auditlist.</param>
        /// <param name="updatePageSequence">if set to <c>true</c> [update page sequence].</param>
        /// <param name="updateAuditHistory">if set to <c>true</c> [update audit history].</param>
        /// <returns>bool</returns>
        public bool ActivateListValues(string siteURL, string listName, int rowID, string auditlistName, bool updatePageSequence, bool updateAuditHistory)
        {

            objCommonDAL = new CommonDAL();
            objCommonUtility = new CommonUtility();
            objCommonDAL.ListStatusUpdate(siteURL, listName, rowID, NO, updatePageSequence);
            if (updateAuditHistory)
            {
                objCommonDAL.UpdateListAuditHistory(siteURL, auditlistName, rowID, objCommonUtility.GetUserName(), AUDIT_ACTION_ACTIVATE);
            }
            return true;
        }

        /// <summary>
        /// Activates the List items
        /// </summary>
        /// <param name="siteUrl">The site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="rowID">The row ID.</param>
        /// <param name="auditlistName">Name of the auditlist.</param>
        /// <param name="actionPeformed">The action peformed.</param>
        /// <param name="itemStatus">The item status.</param>
        /// <param name="sequenceField">The sequence field.</param>
        public void UpdateListItemStatus(string siteUrl, string listName, int rowID, string auditlistName, string actionPeformed, string itemStatus, string sequenceField)
        {
            objCommonDAL = new CommonDAL();
            objCommonUtility = new CommonUtility();
            objCommonDAL.ListItemStatusUpdate(siteUrl, listName, rowID, itemStatus, auditlistName, objCommonUtility.GetUserName(), actionPeformed, sequenceField);

        }

        /// <summary>
        /// Updates the Terminate Status of the List Item
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="rowID">RowID</param>
        /// <param name="actionPeformed">Audit Action ID.</param>
        /// <param name="itemStatus">Status of the Item(Terminated/Activated)</param>
        public void ListItemStatusUpdateForChapterPages(string siteUrl, string listName, int rowID, string itemStatus)
        {
            objCommonDAL = new CommonDAL();
            objCommonUtility = new CommonUtility();
            objCommonDAL.ListItemStatusUpdateForChapterPages(siteUrl, listName, rowID, itemStatus);
        }

        /// <summary>
        /// Terminates the List items
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="rowID">The row ID.</param>
        /// <param name="auditlistName">Name of the auditlist.</param>
        /// <param name="updatepageSequence">if set to <c>true</c> [updatepage sequence].</param>
        /// <param name="updateAuditListHistory">if set to <c>true</c> [update audit list history].</param>
        /// <returns></returns>
        public bool TerminateListValues(string siteURL, string listName, int rowID, string auditlistName, bool updatepageSequence, bool updateAuditListHistory)
        {

            objCommonDAL = new CommonDAL();
            objCommonUtility = new CommonUtility();
            objCommonDAL.ListStatusUpdate(siteURL, listName, rowID, YES, updatepageSequence);
            if (updateAuditListHistory)
            {
                objCommonDAL.UpdateListAuditHistory(siteURL, auditlistName, rowID, objCommonUtility.GetUserName(), AUDIT_ACTION_TERMINATE);
            }

            return true;
        }

        /// <summary>
        /// Gets the Array of Master Page Id
        /// </summary>
        /// <param name="strSiteUrl">Site URL.</param>
        /// <param name="strListName">List Name.</param>
        /// <param name="strCAMLquery">CAML Query.</param>
        /// <returns>int[]</returns>
        public int[] GetMasterPageID(string siteURL, string listName, string CAMLquery)
        {
            DataTable dtResultTable = null;
            int[] intMasterPageId = null;

            objCommonDAL = new CommonDAL();
            dtResultTable = objCommonDAL.ReadList(siteURL, listName, CAMLquery);
            if (dtResultTable != null && dtResultTable.Rows.Count > 0)
            {
                intMasterPageId = new int[dtResultTable.Rows.Count];

                for (int intRowIndex = 0; intRowIndex < dtResultTable.Rows.Count; intRowIndex++)
                {

                    int.TryParse(Convert.ToString(dtResultTable.Rows[intRowIndex]["Master_Page_ID"]), out intMasterPageId[intRowIndex]);
                }
            }

            if (dtResultTable != null)
                dtResultTable.Dispose();

            return intMasterPageId;
        }

        /// <summary>
        /// Creates a CAML query from the selected ID
        /// </summary>
        /// <param name="selectedID">The selected ID.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <returns>CAMLquery</returns>
        public string CreateCAMLQuery(string selectedID, string fieldName, string fieldType)
        {
            StringBuilder strCamlQueryBuilder = new StringBuilder();
            string[] strSelectedIDs = null;
            if (string.IsNullOrEmpty(selectedID))
            {
                // return if the selected ID is empty
                return string.Empty;
            }
            else
            {
                strSelectedIDs = selectedID.Split(';');
            }
            if (strSelectedIDs.Length == 2)
            {
                strCamlQueryBuilder.Append(@"<Where><Eq><FieldRef Name='" + fieldName + "' /><Value Type='" + fieldType + "'>" +
                    strSelectedIDs[0] + "</Value></Eq></Where>");
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
                strCamlQueryBuilder.Insert(0, @"<Where>");
                strCamlQueryBuilder.Append(@"</Where>");

            }

            return strCamlQueryBuilder.ToString();

        }

        /// <summary>
        /// Overloaded method to create CamlQuery 
        /// Which has parameter for ordering the Results accordingly.
        /// </summary>
        /// <param name="selectedID">The selected Id</param>
        /// <param name="fieldName">Field Name for where clause</param>
        /// <param name="fieldType">Type Of the Field</param>
        /// <param name="orderbyField">OrderbyField for ordering the end result</param>
        /// <returns></returns>
        public string CreateCAMLQuery(string selectedID, string fieldName, string fieldType, string orderbyField)
        {
            StringBuilder strCamlQueryBuilder = new StringBuilder();
            string[] strSelectedIDs = null;
            if(string.IsNullOrEmpty(selectedID))
            {
                // return if the selected ID is empty
                return string.Empty;
            }
            else
            {
                strSelectedIDs = selectedID.Split(';');
            }
            if(strSelectedIDs.Length == 2)
            {
                strCamlQueryBuilder.Append(@"<Where><Eq><FieldRef Name='" + fieldName + "' /><Value Type='" + fieldType + "'>" +
                    strSelectedIDs[0] + "</Value></Eq></Where>");
                return strCamlQueryBuilder.ToString();
            }
            if(strSelectedIDs.Length > 2)
            {
                for(int intIndex = 0; intIndex < strSelectedIDs.Length - 1; intIndex++)
                {
                    if(intIndex != 0)
                        strCamlQueryBuilder.Insert(0, "<Or>");
                    strCamlQueryBuilder.Append(@"<Eq><FieldRef Name='" + fieldName + "' /><Value Type='" + fieldType + "'>" +
                        strSelectedIDs[intIndex] + "</Value></Eq>");
                    if(intIndex != 0)
                        strCamlQueryBuilder.Append("</Or>");
                }
                strCamlQueryBuilder.Insert(0, @"<Where>");
                strCamlQueryBuilder.Append(@"</Where>");
                strCamlQueryBuilder.Append(@"<OrderBy><FieldRef Name='" + orderbyField + "' Ascending='True' /></OrderBy>");

            }

            return strCamlQueryBuilder.ToString();

        }

        /// <summary>
        /// Creates a CAML query from the selected ID without the where clause so
        /// that the caller program can add more criteria.
        /// </summary>
        /// <param name="selectedID">The selected ID.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <returns>CAMLquery</returns>
        public string CreateCamlQueryWithoutWhere(string selectedID, string fieldName, string fieldType)
        {
            StringBuilder strCamlQueryBuilder = new StringBuilder();
            string[] strSelectedIDs = null;
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

        /// <summary>
        /// Creates the Well Book Details XML.
        /// </summary>
        /// <param name="bookInfo">BookInfo object.</param>
        /// <returns>XmlDocument.</returns>
        public XmlDocument CreateWellBookDetailXML(BookInfo bookInfo)
        {
            WellBookTreeXMLGeneratorBLL objXmlGenerator = new WellBookTreeXMLGeneratorBLL();
            return (objXmlGenerator.CreateWellBookTreeXML(bookInfo));
        }

        /// <summary>
        /// Uploads the file to the document library
        /// </summary>
        /// <param name="siteUrl">The site URL.</param>
        /// <param name="docLibName">Name of the doc lib.</param>
        /// <param name="pageId">The page id.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="postedFileName">Name of the posted file.</param>
        /// <param name="postedFile">The posted file.</param>
        public void UploadFileToDocumentLibrary(string siteUrl, string docLibName, string pageId, string userName, string postedFileName, byte[] postedFile)
        {
            objCommonDAL = new CommonDAL();
            objCommonDAL.UploadFileToDocumentLibrary(siteUrl, docLibName, pageId, userName, postedFileName, postedFile);
        }

        /// <summary>
        /// Returns the uploaded document url
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="CAMLQuery">The CAML query.</param>
        /// <returns>string.</returns>
        public string GetUploadedDocumentUrl(string siteURL, string listName, string CAMLQuery)
        {
            objCommonDAL = new CommonDAL();
            string strObjectInnerHtml = string.Empty;
            string[] strDetail = objCommonDAL.GetUploadedDocumentUrl(siteURL, listName, CAMLQuery);

            string strDocUrl = strDetail[0];
            string strType = strDetail[1];

            if (!string.IsNullOrEmpty(strDocUrl) && !string.IsNullOrEmpty(strType))
            {
                if (string.Equals(strType, "pdf"))
                {
                    strDocUrl = strDocUrl + "#toolbar=0";
                    strType = "application/pdf";
                }

                if (string.Equals(strType, "bmp") || string.Equals(strType, "gif") || string.Equals(strType, "png"))
                {
                    strType = "image/" + strType;
                }

                if (string.Equals(strType, "jpg") || string.Equals(strType, "jpeg") || string.Equals(strType, "jpe"))
                {
                    strType = "image/jpeg";
                }

                if (string.Equals(strType, "tif"))
                {
                    strType = "image/tiff";
                }

                if (string.Equals(strType, "wmf"))
                {
                    strType = "application/x-msmetafile";
                }
                if (string.Equals(strType, "eps") || string.Equals(strType, "ai"))
                {
                    strType = "application/postscript";
                }
                if (string.Equals(strType, "svg"))
                {
                    strType = "image/svg+xml";
                }

                strObjectInnerHtml = "<object autostart=\"false\" data=\"" + strDocUrl + "\" type=\"" + strType + "\" width=\"100%\" height=\"100%\"></object>";
            }

            return strObjectInnerHtml;
        }

        /// <summary>
        /// Gets the printed document URL.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="CAMLQuery">The CAML query.</param>
        /// <returns>string[]</returns>
        public string[] GetPrintedDocumentUrl(string siteURL, string listName, string CAMLQuery)
        {
            objCommonDAL = new CommonDAL();
            string[] strDetail = objCommonDAL.GetUploadedDocumentUrl(siteURL, listName, CAMLQuery);

            return strDetail;
        }

        /// <summary>
        /// Updates the Audit Trail
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="pageId">The page id.</param>
        /// <param name="auditListName">Name of the audit list.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="actionPerformed">The action performed.</param>
        public void UpdateAuditTrail(string siteURL, string pageId, string auditListName, string userName, string actionPerformed)
        {

            objCommonDAL = new CommonDAL();
            objCommonDAL.UpdateListAuditHistory(siteURL, auditListName, Convert.ToInt32(pageId), userName, actionPerformed);

        }

        /// <summary>
        /// Signs the off page.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="id">The id.</param>
        /// <param name="auditListName">Name of the audit list.</param>
        /// <param name="signOffStatus">if set to <c>true</c> [sign off status].</param>
        public void SignOffPage(string siteURL, string listName, string rowId, string auditListName, bool signOffStatus)
        {

            objCommonDAL = new CommonDAL();
            objCommonUtility = new CommonUtility();
            if (signOffStatus)
                objCommonDAL.SignOffStatusUpdate(siteURL, listName, Convert.ToInt32(rowId), YES, auditListName, objCommonUtility.GetUserName(), AUDIT_ACTION_SIGNEDOFF);
            else
                objCommonDAL.SignOffStatusUpdate(siteURL, listName, Convert.ToInt32(rowId), NO, auditListName, objCommonUtility.GetUserName(), AUDIT_ACTION_UNSIGNEDOFF);

        }

        /// <summary>
        /// returns the AssetValue for a specific chapter
        /// </summary>
        /// <param name="chapterId"></param>
        /// <returns>string</returns>
        public string GetAssetValue(string chapterId)
        {
            BookInfo objBookInfo;
            if (HttpContext.Current.Session[SESSIONTREEVIEWDATAOBJECT] != null)
            {
                objBookInfo = ((BookInfo)HttpContext.Current.Session[SESSIONTREEVIEWDATAOBJECT]);
                foreach (ChapterInfo objChapterInfo in objBookInfo.Chapters)
                {
                    if (string.Compare(objChapterInfo.ChapterID, chapterId) == 0)
                        return objChapterInfo.AssetValue;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the CAML query for BO users.
        /// </summary>
        /// <param name="strSiteURL">The STR site URL.</param>
        /// <param name="columName">Name of the column.</param>
        /// <param name="columnType">Type of the column.</param>
        /// <param name="terminated">The terminated status.</param>
        /// <returns>CAML query for BO users.</returns>
        public string GetCAMLQueryForBOUsers(string strSiteURL, string columName, string columnType, string terminated,bool maintainBooks)
        {
            string strCamlQuery = string.Empty;

            objCommonUtility = new CommonUtility();
            string strUserId = objCommonUtility.GetUserName();
            DataTable dtUser = null;

            strCamlQuery = "<Where><Eq> <FieldRef Name='Windows_User_ID' /> <Value Type='Text'>"
    + strUserId + " </Value></Eq></Where>";
            string strViewFields = @"<FieldRef Name='Windows_User_ID' /><FieldRef Name='ID' />";
            dtUser = ReadList(strSiteURL, USERLIST, strCamlQuery, strViewFields);

            /// Get all the Team_IDs where User_ID = log in user id and Privileges Contains "RS"/"SY" from "DWB Team Staff" list
            /// From all the Team_IDs get the Teams from "DWB Team" list                               
            strCamlQuery = string.Empty;
            strViewFields = string.Empty;
            if (dtUser != null && dtUser.Rows.Count > 0)
            {
                strCamlQuery = @"<OrderBy><FieldRef Name='Team_ID' /></OrderBy>
                                                    <Where>                                                    
                                                        <Eq>
                                                            <FieldRef Name='User_ID' />
                                                            <Value Type='Number'>" + dtUser.Rows[0]["ID"].ToString() + "</Value>" +
                                    "</Eq>" +
                                 "</Where>";
                strViewFields = @"<FieldRef Name='Team_ID' /><FieldRef Name='User_ID' /><FieldRef Name='ID' />";
                dtUser.Dispose();
            }

            DataTable dtTeams = ReadList(strSiteURL, TEAMSTAFFLIST, strCamlQuery, strViewFields);
            strCamlQuery = string.Empty;
            StringBuilder sbTeamId = new StringBuilder();

            #region DREAM 4.0- eWB 2.0
            if (maintainBooks)
            {
                strCamlQuery = @" <OrderBy><FieldRef Name='Title' /></OrderBy><Where><And><Eq><FieldRef Name='Terminate_Status' />          <Value Type='Choice'>"+terminated +"</Value></Eq><Or><IsNull><FieldRef Name='ToBeDeleted' /></IsNull><Eq><FieldRef Name='ToBeDeleted' /><Value Type='Choice'>No</Value></Eq></Or></And></Where>";
            }
            else
            {
                strCamlQuery = @"<OrderBy><FieldRef Name='Title' /></OrderBy><Where><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>"+ terminated+"</Value></Eq></Where>";
            }
            #endregion
            if (dtTeams != null && dtTeams.Rows.Count > 0)
            {
                for (int intRowIndex = 0; intRowIndex < dtTeams.Rows.Count; intRowIndex++)
                {
                    sbTeamId.Append(Convert.ToString(dtTeams.Rows[intRowIndex]["Team_ID"]));
                    sbTeamId.Append(";");
                }
                strCamlQuery = CreateCamlQueryWithoutWhere(sbTeamId.ToString(), columName, columnType);
                sbTeamId.Remove(0, sbTeamId.Length);
                sbTeamId.Append(strCamlQuery);

                #region DREAM 4.0- eWB 2.0
                if (maintainBooks)
                {
                    //sbTeamId.Append("<Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>" + terminated + "</Value></Eq></And></Where>");
                    sbTeamId.Append("<And><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>"+ terminated+"</Value></Eq><Or><IsNull><FieldRef Name='ToBeDeleted' /> </IsNull><Eq><FieldRef Name='ToBeDeleted' /><Value Type='Choice'>No</Value></Eq></Or></And></And></Where>");
                    sbTeamId.Insert(0, "<Where><And>");
                }
                else
                {
                    sbTeamId.Append("<Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>" + terminated + "</Value></Eq></And></Where>");
                    sbTeamId.Insert(0, "<Where><And>");
                }
                #endregion
                strCamlQuery = sbTeamId.ToString();
                dtTeams.Dispose();
            }
            return strCamlQuery;
        }

        #region DREAM 4.0- eWB 2.0

        #region Delete List Item

        public void MarkItemToDelete(string siteURL, string listName, int itemID)
        {
            string strCamlQuery = string.Format(@"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>{0}</Value></Eq></Where> ", itemID.ToString());
            string strViewFields = @"<FieldRef Name='ID' /><FieldRef Name='ToBeDeleted' />";
            UpdateListItem(siteURL, listName, "ToBeDeleted", Microsoft.SharePoint.SPFieldType.Choice, YES, strCamlQuery, strViewFields);
        }
        /// <summary>
        /// Deletes the list item by id.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="itemID">The item ID.</param>
        public void DeleteListItemById(string siteURL, int itemID, string listName, string auditListName)
        {
            objCommonDAL = new CommonDAL();
            objCommonDAL.DeleteListItemById(siteURL, listName, itemID);

            if(!string.IsNullOrEmpty(auditListName))
            {
                /// Get the CAML Query to delete the Audit History records for the selected records.
                string strCamlQuery = string.Format(AUDITHISTORYCAMLQUERY, itemID.ToString());

                objCommonDAL.DeleteAuditTrail(siteURL, auditListName, strCamlQuery);
            }
        }

        /// <summary>
        /// Deletes the list items by id.
        /// Pass the array of list item IDs.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="itemID">The item ID.</param>
        public void DeleteListItemById(string siteURL,  int[] itemID,string listName,string auditListName)
        {
            objCommonDAL = new CommonDAL();
            objCommonDAL.DeleteListItemById(siteURL, listName, itemID);
        }

        /// <summary>
        /// Deletes the chapter pages.
        /// Deletes Audit Trail, Story Board, Comments, Narrative and document
        /// for the selected page.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="chapterPageID">The chapter page ID.</param>
        /// <param name="connectionType">Type of the connection.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="auditListName">Name of the audit list.</param>
        public void DeleteChapterPages(string siteURL, int chapterPageID, string connectionType, string listName, string auditListName)
        {
            string strCamlQuery = string.Empty;
            string strPageType = string.Empty;
            DataTable dtResultTable;
            objCommonDAL = new CommonDAL();
            
            /// Get the Page type from "DWB Chapter Pages Mapping" list
            /// Based on the connection type delete the document.
            strCamlQuery = string.Format(@" <Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>{0}</Value></Eq></Where>", chapterPageID.ToString());
            dtResultTable = objCommonDAL.ReadList(siteURL, listName, strCamlQuery, "<FieldRef Name='ID' /><FieldRef Name='Connection_Type' />");
           if (dtResultTable != null && dtResultTable.Rows.Count > 0)
           {
               strPageType = Convert.ToString(dtResultTable.Rows[0]["Connection_Type"]);
               dtResultTable.Dispose();
           }
           /// Delete documents for the selected page
           if (!string.IsNullOrEmpty(strPageType))
           {
               switch (strPageType)
               {
                   case DWBTYPE2PAGE:
                       {
                           objCommonDAL.DeleteDocument(siteURL,PUBLISHEDDOCUMENTLIST,string.Format(CHAPTERPAGEDOCUMNETCAMEQUERY,chapterPageID.ToString()));
                           break;
                       }
                   case DWBTYPE3PAGE:
                       {
                           objCommonDAL.DeleteDocument(siteURL, USERDEFINEDDOCUMENTLIST, string.Format(CHAPTERPAGEDOCUMNETCAMEQUERY, chapterPageID.ToString()));
                           break;
                       }
                   default:
                       break;
               }
           }
           strCamlQuery = string.Empty;
            /// CAML query for DWB Comments.
           strCamlQuery = string.Format(@"<Where><Eq><FieldRef Name='Page_ID' /><Value Type='Number'>{0}</Value></Eq></Where> ", chapterPageID.ToString());
           /// Delete Comments
           objCommonDAL.DeleteListItem(siteURL, DWBCOMMENT, strCamlQuery);
          
           /// Delete Narrative
           objCommonDAL.DeleteListItem(siteURL, DWBNARRATIVES, strCamlQuery);
            /// Delete StoryBoard
            objCommonDAL.DeleteListItem(siteURL, DWBSTORYBOARD, strCamlQuery);
            /// Delete Audit Trail
            objCommonDAL.DeleteAuditTrail(siteURL, auditListName, string.Format(AUDITHISTORYCAMLQUERY, chapterPageID.ToString()));
            /// Delete Chapter Page
            objCommonDAL.DeleteListItemById(siteURL, listName, chapterPageID);

        }

        /// <summary>
        /// Deletes the template.
        /// Deletes the Pages added to the template and removes the entry 
        /// in Template_ID column of Master Pages list.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="templateID">The template ID.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="auditListName">Name of the audit list.</param>
        public void DeleteTemplate(string siteURL, int templateID, string listName, string auditListName)
        {
            DataTable dtResultsTable;
            StringBuilder strTemplateID = new StringBuilder(); ;
            objCommonDAL = new CommonDAL();
            string strCamlQuery = string.Empty;
           
            /// Retrieve ID values for of Template Pages Mapping list.
            dtResultsTable = objCommonDAL.ReadList(siteURL, DWBTEMPLATEPAGESLIST, string.Format(@"<Where><Eq><FieldRef Name='Template_ID' /><Value Type='Number'>{0}</Value></Eq></Where>", templateID.ToString()), "<FieldRef Name='Template_ID' /><FieldRef Name='ID' />");

            if (dtResultsTable != null && dtResultsTable.Rows.Count > 0)
            {
               
                for (int intRowIndex = 0; intRowIndex < dtResultsTable.Rows.Count;intRowIndex++ )
                {
                    strTemplateID.Append(Convert.ToString(dtResultsTable.Rows[intRowIndex]["ID"]));
                    strTemplateID.Append(";");
                }
            }
            /// Delete the Template Pages
            objCommonDAL.DeleteListItem(siteURL, DWBTEMPLATEPAGESLIST, string.Format(@"<Where><Eq><FieldRef Name='Template_ID' /><Value Type='Number'>{0}</Value></Eq></Where>", templateID.ToString()));

            if (strTemplateID.Length > 0)
            {
                strCamlQuery = CreateCAMLQuery(strTemplateID.ToString(), "Master_ID", "Number");
            }
            /// Delete the Template Pages Audit
            objCommonDAL.DeleteAuditTrail(siteURL, DWBTEMPLATECONFIGURATIONAUDIT, strCamlQuery);

            /// Delete Template
            objCommonDAL.DeleteListItemById(siteURL, listName, templateID);
            /// Delete Template Audit
            objCommonDAL.DeleteAuditTrail(siteURL, auditListName, string.Format(AUDITHISTORYCAMLQUERY, templateID.ToString()));
        }

        /// <summary>
        /// Deletes the user.
        /// Deletes the user from the team if added to any team.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="ID">The ID.</param>
        /// <param name="listName">Name of the list.</param>
        public void DeleteUser(string siteURL, int ID, string listName)
        {
            objCommonDAL = new CommonDAL();
            /// Delete User from the team
            string strCamlQuery = string.Format(@"<Where><Eq><FieldRef Name='User_ID' /><Value Type='Number'>{0}</Value></Eq></Where>", ID.ToString());
            objCommonDAL.DeleteListItem(siteURL, TEAMSTAFFLIST, strCamlQuery);
            /// Delete User
            objCommonDAL.DeleteListItemById(siteURL, listName, ID);
            
        }

        /// <summary>
        /// Deletes the team.
        /// Deletes staffs added to the team and then the team
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="ID">The ID.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="stafflistName">Name of the stafflist.</param>
        public void DeleteTeam(string siteURL, int ID, string listName,string stafflistName)
        {
            objCommonDAL = new CommonDAL();
            /// Delete User from the team
            string strCamlQuery = string.Format(@"<Where><Eq><FieldRef Name='Team_ID' /><Value Type='Number'>{0}</Value></Eq></Where>", ID.ToString());
            objCommonDAL.DeleteListItem(siteURL, stafflistName, strCamlQuery);
            /// Delete User
            objCommonDAL.DeleteListItemById(siteURL, listName, ID);
            
        }

        public void UpdateListItem(string siteURL, string listName, string fieldName, Microsoft.SharePoint.SPFieldType fieldType, string fieldValue, string camlQuery, string viewFields)
        {
            objCommonDAL = new CommonDAL();
            objCommonDAL.UpdateListItem(siteURL, listName, fieldName, fieldType, fieldValue, camlQuery, viewFields);
        }
        #endregion

        #endregion

        #endregion
        
         #region Print Update
        /// <summary>
        /// Gets the request ID.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="camlQuery">The caml query.</param>
        /// <returns></returns>
        public string GetRequestID(string siteURL, string listName, string camlQuery)
        {
            DataTable dtListValues = null;
            string strURL = string.Empty;
            try
            {
                objCommonDAL = new CommonDAL();
                dtListValues = objCommonDAL.ReadList(siteURL, listName, camlQuery);

                if (dtListValues != null && dtListValues.Rows.Count > 0)
                {
                    for (int intRowIndex = 0; intRowIndex < dtListValues.Rows.Count; intRowIndex++)
                    {
                        strURL = Convert.ToString(dtListValues.Rows[intRowIndex]["DocumentURL"]);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dtListValues != null)
                    dtListValues.Dispose();
            }
            return strURL;
        }

        /// <summary>
        /// Updates the chapter print details.
        /// </summary>
        /// <param name="requestID">The request ID.</param>
        /// <param name="documentURL">The document URL.</param>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="p">The p.</param>
        public void UpdateChapterPrintDetails(string requestID, string documentURL, string siteURL, string userName, string p)
        {
            objCommonDAL = new CommonDAL();
            objCommonDAL.UpdateChapterPrintDetails(requestID, documentURL, siteURL, userName, "DWB Page Print Details");
        }

        /// <summary>
        /// Terminates the book.
        /// </summary>
        /// <param name="strParentSiteURL">The STR parent site URL.</param>
        /// <param name="bookID">The book ID.</param>
        /// <param name="listName">Name of the list.</param>
        public void TerminateBook(string strParentSiteURL, int bookID, string listName)
        {
            objCommonDAL = new CommonDAL();
            objCommonDAL.TerminateBook(strParentSiteURL, bookID, listName);
        }
        #endregion

        /// <summary>
        /// Gets the alert message.
        /// </summary>
        /// <param name="parentSiteURL">The parent site URL.</param>
        /// <param name="level">The level.</param>
        /// <returns></returns>
        public string GetAlertMessage(string parentSiteURL, string level)
        {
            objCommonDAL = new CommonDAL();
            string strAlertMessage = string.Empty;
            string strCamlQuery = @"<Where><Eq><FieldRef Name='PrintLevel' /><Value Type='Choice'>" + level + "</Value></Eq></Where>";
            strAlertMessage = objCommonDAL.GetAlertMessage(parentSiteURL, strCamlQuery, "DWB Print Alert Messages");
            return strAlertMessage;
        }

    }
}

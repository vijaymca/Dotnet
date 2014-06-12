#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: CustomListViewerHelper.cs
#endregion

using System;
using System.Runtime.InteropServices;
using System.Web;
using System.Data;
using System.Text;
using System.Drawing;
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
using Shell.SharePoint.DWB.BusinessLogicLayer;
using Shell.SharePoint.DWB.Business.DataObjects;

namespace Shell.SharePoint.WebParts.DWB.CustomListViewer
{
    /// <summary>
    /// CustomListViewerHelper class
    /// </summary>
    [Guid("43aefa45-c5f3-4978-a672-317e60282d87")]
    public class CustomListViewerHelper : System.Web.UI.WebControls.WebParts.WebPart
    {
        #region DECLARATION
        string strAuditListName = string.Empty;
        string strListName = string.Empty;
        bool blnTerminatedStatus;
        protected bool blnInitializePageNumber;
        string strIsCheckBoxApplicable = "false";
        string strIsAddMasterApplicable = "false";
        string strIsViewMasterApplicable = "false";
        string strIsExpandCollapseApplicable = "false";
        string strIsEditApplicable = "false";
        string strSignedOffStatus = string.Empty;
        protected string strSiteURL = string.Empty;
        string strReportName = string.Empty;
        protected XmlDocument xmlListDocument;

        protected const string WELLBOOKPAGEVIEW = "WellBookPageView";
        protected const string MASTERPAGEREPORT = "MasterPage";
        protected const string AUDITTRAIL = "AuditTrail";
        protected const string TEMPLATEREPORT = "Template";
        protected const string TEMPLATEPAGESREPORT = "TemplatePages";
        protected const string ADDREMOVETEMPLATEPAGES = "AddRemoveTemplatePages";
        protected const string ALTERTEMPLATEPAGESEQUENCE = "AlterTemplatePageSequence";
        protected const string WELLBOOKREPORT = "WellBook";
        protected const string CHAPTERPAGEMAPPINGREPORT = "BookPages";
        protected const string CHAPTERPAGEREPORT = "ChapterPages";
        protected const string CHAPTERREPORT = "Chapters";
        protected const string USERREGISTRATION = "UserRegistration";
        protected const string USERPRIVILEGES = "UserPrivileges";
        protected const string TEAMREGISTRATION = "TeamRegistration";
        protected const string STAFFREGISTRATION = "StaffRegistration";
        protected const string STAFFRANK = "StaffRank";
        //protected const string STATUSTERMINATE = "Terminate";
        #region DREAM 4.0 - eWB 2.0
        protected const string STATUSTERMINATE = "Archive";
        #endregion
        protected const string STATUSACTIVATE = "Activate";
        protected const string DWBHOME = "DWBHome";

        protected const string EVENTARG = "__EVENTARGUMENT";
        protected const string EVENTTARGET = "__EVENTTARGET";

        protected const string MASTERPAGELIST = "DWB Master Pages";
        protected const string MASTERPAGEAUDITTRAIL = "DWB Master Pages Audit Trail";
        protected const string WELLBOOKLIST = "DWB Books";
        protected const string WELLBOOKAUDITTRAIL = "DWB Books Audit Trail";
        protected const string CHAPTERPAGEMAPPINGLIST = "DWB Chapter Pages Mapping";
        protected const string CHAPTERLIST = "DWB Chapter";
        protected const string CHAPTERLISTAUDITTRAIL = "DWB Chapter Audit Trail";
        protected const string CHAPTERPAGEMAPPINGLISTAUDITTRAIL = "DWB Chapter Pages Mapping Audit Trail";

        protected const string TEMPLATELIST = "DWB Templates";
        protected const string TEMPLATEAUDITTRAIL = "DWB Templates Audit Trail";
        protected const string TEMPLATEPAGEMAPPINGLIST = "DWB Template Page Mapping";
        protected const string TEMPLATEPAGEMAPPINGAUDITTRIALLIST = "DWB Template Page Mapping Audit Trail";
        protected const string USERLIST = "DWB User";
        protected const string TEAMLIST = "DWB Team";
        protected const string TEAMSTAFFLIST = "DWB Team Staff";
        protected const string SYSTEMPRIVILEGESLIST = "DWB System Privileges";
        protected const string IDVALUEQUERYSTRING = "idValue";
        protected const string MODEQUERYSTRING = "mode";

        protected const string BOOKACTION_PUBLISH = "pdf";
        protected const string BOOKACTION_PRINT = "print";

        protected const string AUDIT_ACTION_ACTIVATE = "3";
        protected const string AUDIT_ACTION_TERMINATE = "4";
        protected const string AUDIT_ACTION_SIGNEDOFF = "5";
        protected const string AUDIT_ACTION_UNSIGNEDOFF = "6";

        protected const string STATUS_TERMINATED = "Yes";
        protected const string STATUS_ACTIVE = "No";
        protected const string STATUS_SIGNEDOFF = "Yes";
        protected const string STATUS_UNSIGNEDOFF = "No";

        protected const string CHAPTERTITLE_COLUMNNAME = "ChapterTitle";
        protected const string DATATABLE_SORTEXP = "{0} {1}";
        protected const string SORTDIR_ASC = "ASC";

        //added by Praveena
        protected const string DWBTITLECOLUMN = "Title";
        protected const string DWBIDCOLUMN = "ID";
        protected const string DROPDOWNDEFAULTTEXTALL = "--Select All--";

        #region DREAM 4.0 - eWB 2.0
        protected const string STATUSREMOVE = "Remove";
        protected StringBuilder strResultTable;
        #endregion

        Records objRecords;
        DataTable dtHistoryDetails;

        ListViewerXMLGeneratorBLL objListViewer;
        CommonBLL objCommonBLL;

        protected WellBookBLL objWellBook;
        protected CommonUtility objCommonUtility;
        protected string strUserName = string.Empty;

        protected HyperLink linkExcel = new HyperLink();
        protected HyperLink linkPrint = new HyperLink();
        protected System.Collections.Generic.Dictionary<string, string> dicChatperDetail = new System.Collections.Generic.Dictionary<string, string>();
        private string sortType = string.Empty;
        private string sortBy = string.Empty;
        private string pageNumber = string.Empty;


        #endregion

        #region PROPERTIES
        /// <summary>
        /// Gets or sets the name of the report.
        /// </summary>
        /// <value>The name of the report.</value>
        public string ListReportName
        {
            get { return strReportName; }
            set { strReportName = value; }
        }

        /// <summary>
        /// Property to Store the Audit List Name
        /// </summary>
        public string AuditListName
        {
            get
            {
                return strAuditListName;
            }
            set
            {
                strAuditListName = value;
            }
        }

        /// <summary>
        /// Property to store the List Name
        /// </summary>
        public string ListName
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
        /// Property to store the Page Status
        /// </summary>
        public bool ActiveStatus
        {
            get
            {
                return blnTerminatedStatus;
            }
            set
            {
                blnTerminatedStatus = value;
            }
        }

        /// <summary>
        /// Gets or sets the is edit applicable.
        /// </summary>
        /// <value>The is edit applicable.</value>
        public string IsEditApplicable
        {
            get { return strIsEditApplicable; }
            set { strIsEditApplicable = value; }
        }

        /// <summary>
        /// Gets or sets the is check box applicable.
        /// </summary>
        /// <value>The is check box applicable.</value>
        public string IsCheckBoxApplicable
        {
            get { return strIsCheckBoxApplicable; }
            set { strIsCheckBoxApplicable = value; }
        }

        /// <summary>
        /// Gets or sets the is add master applicable.
        /// </summary>
        /// <value>The is add master applicable.</value>
        public string IsAddMasterApplicable
        {
            get { return strIsAddMasterApplicable; }
            set { strIsAddMasterApplicable = value; }
        }

        /// <summary>
        /// Gets or sets the is view master applicable.
        /// </summary>
        /// <value>The is view master applicable.</value>
        public string IsViewMasterApplicable
        {
            get { return strIsViewMasterApplicable; }
            set { strIsViewMasterApplicable = value; }
        }

        /// <summary>
        /// Gets or sets the is expand collapse applicable.
        /// </summary>
        /// <value>The is expand collapse applicable.</value>
        public string IsExpandCollapseApplicable
        {
            get { return strIsExpandCollapseApplicable; }
            set { strIsExpandCollapseApplicable = value; }
        }

        /// <summary>
        /// Property to store the Signed Off Status
        /// </summary>
        public string SignedOffStatus
        {
            get
            {
                return strSignedOffStatus;
            }
            set
            {
                strSignedOffStatus = value;
            }
        }

        public string SortType
        {
            get { return sortType; }
            set { sortType = value; }
        }

        public string SortBy
        {
            get { return sortBy; }
            set { sortBy = value; }
        }


        public string PageNumber
        {
            get { return pageNumber; }
            set { pageNumber = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the list XML.
        /// </summary>
        /// <returns></returns>
        protected DataTable GetListXml()
        {
            string strCamlQuery = string.Empty;
            string strTerminated = string.Empty;
            DataTable dtListDetails = null;
            StringBuilder strRecordID = new StringBuilder();
            try
            {
                if (!ActiveStatus)
                    strTerminated = STATUS_TERMINATED;
                else
                    strTerminated = STATUS_ACTIVE;
                strCamlQuery = @"<Where><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>" +
                    strTerminated + "</Value></Eq></Where>";
                strCamlQuery = GetsCAMLQuery();
                objCommonBLL = new CommonBLL();
                strSiteURL = SPContext.Current.Site.Url.ToString();
                if (!string.IsNullOrEmpty(strCamlQuery))
                {
                    dtListDetails = objCommonBLL.ReadList(strSiteURL, ListName.ToString(), strCamlQuery);
                }
                if (dtListDetails != null && dtListDetails.Rows.Count > 0)
                {
                    //dtHistoryDetails = objCommonBLL.ReadList(strSiteURL, AuditListName,
                    //    "<OrderBy><FieldRef Name='Created' Ascending='False' /></OrderBy>");
                    SetRecordsDataObject(dtListDetails);
                    objListViewer = new ListViewerXMLGeneratorBLL();
                    xmlListDocument = objListViewer.CreateListViewerXML(objRecords);
                }
            }
            catch
            { throw; }
            finally
            {
                if (dtListDetails != null)
                {
                    dtListDetails.Dispose();
                }
            }
            return dtListDetails;
        }

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
                    //if (!string.IsNullOrEmpty(strSiteURL) && !string.IsNullOrEmpty(AuditListName))
                    //{
                    //    dtHistoryDetails = objCommonBLL.ReadList(strSiteURL, AuditListName,
                    //        "<OrderBy><FieldRef Name='Created' Ascending='False' /></OrderBy>");
                    //}
                    /// Sets the properties of Records object
                    SetRecordsDataObject(dtListDetails);
                    objListViewer = new ListViewerXMLGeneratorBLL();
                    xmlListDocument = objListViewer.CreateListViewerXML(objRecords);
                }
            }
            catch
            { throw; }

        }

        /// <summary>
        /// Gets the parent title for the selected item.
        /// </summary>
        /// <returns>Title of the selected item.</returns>
        protected string GetParentTitle()
        {
            string strTitle = string.Empty;
            string strCAMLQuery = string.Empty;
            string strFieldsToView = string.Empty;
            DataTable dtResultTable = null;
            switch (strReportName)
            {
                case TEMPLATEPAGESREPORT:
                    {
                        TemplateDetailBLL objTemplateDetail = new TemplateDetailBLL();
                        strCAMLQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + HttpContext.Current.Request.QueryString[IDVALUEQUERYSTRING] + "</Value></Eq></Where>";
                        ListEntry objTemplateListEntry = objTemplateDetail.GetTemplateDetail(strSiteURL, TEMPLATELIST, strCAMLQuery);
                        if (objTemplateListEntry != null && objTemplateListEntry.TemplateDetails != null)
                        {
                            strTitle = objTemplateListEntry.TemplateDetails.Title;
                        }
                        break;
                    }
                case STAFFREGISTRATION:
                    {
                        TeamStaffRegistrationBLL objTeamStaffRegistrationBLL = new TeamStaffRegistrationBLL();
                        ListEntry objTeamListEntry = objTeamStaffRegistrationBLL.GetTeamDetails(strSiteURL, HttpContext.Current.Request.QueryString[IDVALUEQUERYSTRING], TEAMLIST);
                        if (objTeamListEntry != null && objTeamListEntry.TeamDetails != null)
                        {
                            strTitle = objTeamListEntry.TeamDetails.TeamName;
                        }
                        break;
                    }
                case "Audit Trail":
                    {
                        if (HttpContext.Current.Request.QueryString["auditFor"] != null)
                            strTitle = HttpContext.Current.Request.QueryString["auditFor"];
                        else
                            strTitle = string.Empty;
                        break;
                    }
                case CHAPTERREPORT:
                case WELLBOOKPAGEVIEW:
                case CHAPTERPAGEMAPPINGREPORT:
                    {
                        strCAMLQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + HttpContext.Current.Request.QueryString["BookId"] + "</Value></Eq></Where>";
                        objCommonBLL = new CommonBLL();
                        strFieldsToView = "<FieldRef Name='ID'/><FieldRef Name='Title'/>";
                        dtResultTable = objCommonBLL.ReadList(strSiteURL, WELLBOOKLIST, strCAMLQuery, strFieldsToView);
                        if (dtResultTable != null && dtResultTable.Rows.Count > 0)
                        {
                            strTitle = Convert.ToString(dtResultTable.Rows[0]["Title"]);
                        }
                        break;
                    }

                case CHAPTERPAGEREPORT:
                    {

                        string strBookId = string.Empty;
                        strCAMLQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + HttpContext.Current.Request.QueryString["ChapterID"] + "</Value></Eq></Where>";
                        objCommonBLL = new CommonBLL();
                        strFieldsToView = "<FieldRef Name='Title'/><FieldRef Name='Book_ID'/>";
                        dtResultTable = objCommonBLL.ReadList(strSiteURL, CHAPTERLIST, strCAMLQuery, strFieldsToView);
                        if (dtResultTable != null && dtResultTable.Rows.Count > 0)
                        {
                            strTitle = Convert.ToString(dtResultTable.Rows[0]["Title"]);
                            strBookId = Convert.ToString(dtResultTable.Rows[0]["Book_ID"]);
                        }
                        if (!string.IsNullOrEmpty(strBookId))
                        {
                            strCAMLQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + strBookId + "</Value></Eq></Where>";

                            strFieldsToView = "<FieldRef Name='Title'/>";
                            dtResultTable = objCommonBLL.ReadList(strSiteURL, WELLBOOKLIST, strCAMLQuery, strFieldsToView);
                            if (dtResultTable != null && dtResultTable.Rows.Count > 0)
                            {
                                strTitle = strTitle + " ( " + Convert.ToString(dtResultTable.Rows[0]["Title"]) + " )";

                            }
                        }
                        break;
                    }
            }
            if (dtResultTable != null)
            {
                dtResultTable.Dispose();
            }
            return strTitle;
        }

        /// <summary>
        /// This method retrieves the List Items to be rendered on the page
        /// The CAML is formed based on the CustomListType value
        /// </summary>
        /// <returns>DataTable</returns>
        protected DataTable GetRecords()
        {
            string strTerminated = string.Empty;
            DataTable dtListDetails = null;



            if (!ActiveStatus)
                strTerminated = STATUS_TERMINATED;
            else
                strTerminated = STATUS_ACTIVE;

            string strCamlQuery = @"<Where><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>" +
                 strTerminated + "</Value></Eq></Where>";
           
            strCamlQuery = GetsCAMLQuery();
            objCommonBLL = new CommonBLL();
            if (!string.IsNullOrEmpty(strCamlQuery))
            {
                dtListDetails = objCommonBLL.ReadList(strSiteURL, ListName.ToString(), strCamlQuery);
            }
            if (string.Compare(ListReportName, CHAPTERPAGEMAPPINGREPORT, true) == 0 || string.Compare(ListReportName, WELLBOOKPAGEVIEW, true) == 0)
            {
                dtListDetails.Columns.Add(CHAPTERTITLE_COLUMNNAME);
                for (int rowIndex = 0; rowIndex < dtListDetails.Rows.Count; rowIndex++)
                {
                    string strChapterTitle = string.Empty;
                    if (dicChatperDetail != null)
                    {
                        dicChatperDetail.TryGetValue(Convert.ToString(dtListDetails.Rows[rowIndex]["Chapter_ID"]), out strChapterTitle);
                    }
                    dtListDetails.Rows[rowIndex][CHAPTERTITLE_COLUMNNAME] = strChapterTitle;
                }

                dtListDetails.DefaultView.Sort = string.Format(DATATABLE_SORTEXP, CHAPTERTITLE_COLUMNNAME, SORTDIR_ASC);
                dtListDetails = dtListDetails.DefaultView.ToTable();
            }
            return dtListDetails;
        }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            string strUserName = string.Empty;
            objCommonUtility = new CommonUtility();
            strUserName = objCommonUtility.GetUserName();
            return strUserName;
        }

        /// <summary>
        /// Gets the camlQuery based on the report type
        /// </summary>
        /// <returns></returns>
        private string GetsCAMLQuery()
        {
            string strTerminated = string.Empty;
            string strCamlQuery = string.Empty;
            string strMasterID = string.Empty;
            if (!ActiveStatus)
                strTerminated = STATUS_TERMINATED;
            else
                strTerminated = STATUS_ACTIVE;
            switch (ListReportName)
            {
                case "Audit Trail":
                    {
                        strMasterID = HttpContext.Current.Request.QueryString["auditID"];
                        strCamlQuery = @"<OrderBy><FieldRef Name='Created' Ascending='False' /></OrderBy><Where><Eq><FieldRef Name='Master_ID' /><Value Type='Number'>" + strMasterID + "</Value></Eq></Where>";
                        break;
                    }
                case MASTERPAGEREPORT:
                    {
                        strCamlQuery = @"<OrderBy><FieldRef Name='Page_Sequence' /></OrderBy><Where><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>" +
                   strTerminated + "</Value></Eq></Where>";
                        break;
                    }
                case CHAPTERPAGEMAPPINGREPORT:
                    {
                        strCamlQuery = GetCAMLQueryForWellBookPages();
                        if (strCamlQuery.Length == 0)
                        {
                            strCamlQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>-9999</Value></Eq></Where>";
                        }
                        break;
                    }
                case WELLBOOKPAGEVIEW:
                    {
                        strCamlQuery = GetCAMLQueryForWellBookSummaryPages();
                        if (strCamlQuery.Length == 0)
                        {
                            strCamlQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>-9999</Value></Eq></Where>";
                        }
                        break;
                    }
                case CHAPTERREPORT:
                    {
                        strCamlQuery = GetCAMLQueryForWellBookChapters();
                        if (strCamlQuery.Length == 0)
                        {
                            #region DREAM 4.0 - eWB2.0 - Deletion Module
                            /// <Eq><FieldRef Name='ToBeDeleted' /><Value Type='Choice'>No</Value></Eq></And> is added.
                            /// And condition added to the CAML query
                            //strCamlQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>-9999</Value></Eq></Where>";
                            strCamlQuery = @"<Where><And><Eq><FieldRef Name='ID' /><Value Type='Counter'>-9999</Value></Eq>   <Or>
         <IsNull>
            <FieldRef Name='ToBeDeleted' />
         </IsNull><Eq>
            <FieldRef Name='ToBeDeleted' />
            <Value Type='Choice'>No</Value>
         </Eq>      </Or>
      </And></Where>";
                            #endregion 
                        }
                        break;
                    }
                case CHAPTERPAGEREPORT:
                    {
                        strCamlQuery = @"<OrderBy><FieldRef Name='Page_Sequence' /></OrderBy><Where><And><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>" +
                   strTerminated + "</Value></Eq><Eq><FieldRef Name='Chapter_ID'/><Value Type='Number'>" + HttpContext.Current.Request.QueryString["ChapterID"] + "</Value></Eq></And></Where>";
                        break;
                    }
                case TEMPLATEREPORT:
                    {
                        /// Title column in DWB Templates list is renamed to "Template_Name". the Internal name remains as "Title";
                        strCamlQuery = @"<OrderBy><FieldRef Name='Title' /></OrderBy><Where><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>" + strTerminated + "</Value></Eq></Where>";
                        break;
                    }
                case TEMPLATEPAGESREPORT:
                    {
                        strCamlQuery = GetCAMLQueryForTemplatePages();
                        break;
                    }
                case USERREGISTRATION:
                    {
                        strCamlQuery = @"<OrderBy><FieldRef Name='DWBUserName' /></OrderBy><Where><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>" + strTerminated + "</Value></Eq></Where>";
                        break;
                    }
                case TEAMREGISTRATION:
                    {
                        object objStoredPrivilege = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPrivileges.ToString());
                        if (objStoredPrivilege != null)
                        {
                            Privileges objPrivileges = (Privileges)objStoredPrivilege;
                            if (objPrivileges != null)
                            {

                                if (objPrivileges.SystemPrivileges != null)
                                {
                                    if (objPrivileges.SystemPrivileges.AdminPrivilege)
                                    {
                                        /// Get all the teams from "DWB Team"
                                        strCamlQuery = @"<OrderBy><FieldRef Name='Title' /></OrderBy><Where><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>" + strTerminated + "</Value></Eq></Where>";
                                    }
                                    else if (objPrivileges.SystemPrivileges.BookOwner)
                                    {
                                        /// Get the teams where loggedin user is member
                                        objCommonBLL = new CommonBLL();
                                        strCamlQuery = objCommonBLL.GetCAMLQueryForBOUsers(strSiteURL, "ID", "Counter", strTerminated,false);
                                    }
                                }

                            }
                        }

                        break;
                    }
                case STAFFREGISTRATION:
                    {
                        strCamlQuery = @"<OrderBy><FieldRef Name='Discipline' /><FieldRef Name='User_Rank' /></OrderBy><Where><Eq><FieldRef Name='Team_ID' /><Value Type='Number'>" + HttpContext.Current.Request.QueryString[IDVALUEQUERYSTRING] + "</Value></Eq></Where>";
                        break;
                    }
                case WELLBOOKREPORT:
                    {
                        strCamlQuery = string.Empty;
                        object objStoredPrivilege = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPrivileges.ToString());
                        if (objStoredPrivilege != null)
                        {
                            Privileges objPrivileges = (Privileges)objStoredPrivilege;
                            if (objPrivileges != null && objPrivileges.SystemPrivileges != null)
                            {
                                if (objPrivileges.SystemPrivileges.AdminPrivilege)
                                {
                                    /// <Eq><FieldRef Name='ToBeDeleted' /><Value Type='Choice'>No</Value></Eq></And> is added.
                                    /// And condition added to the CAML query
                                   
           //                         strCamlQuery = @"<OrderBy><FieldRef Name='Title' /></OrderBy><Where><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>" +
                                    //strTerminated + "</Value></Eq></Where>";
                                    #region DREAM 4.0 - eWB2.0 - Deletion Module
                                    strCamlQuery = @"<OrderBy><FieldRef Name='Title' /></OrderBy><Where><And><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>" +
           strTerminated + "</Value></Eq><Or><IsNull><FieldRef Name='ToBeDeleted' /></IsNull><Eq><FieldRef Name='ToBeDeleted' /><Value Type='Choice'>No</Value></Eq></Or></And></Where>";
                                    #endregion 
                                }
                                else if (objPrivileges.SystemPrivileges.BookOwner)
                                {
                                    /// Find the Team_IDs where user is member "DWB Team Staff" list
                                    /// and Get Only the Books where Team (DWB Books) == Team_Name(Title) in "DWB Team" list
                                    /// <TODO>
                                    /// Add the <Eq><FieldRef Name='ToBeDeleted' /><Value Type='Choice'>No</Value></Eq> at proper location
                                    /// </TODO>
                                    objCommonBLL = new CommonBLL();
                                    strCamlQuery = objCommonBLL.GetCAMLQueryForBOUsers(strSiteURL, "Team_ID", "Number", strTerminated,true);
                                }
                                else if (objPrivileges.SystemPrivileges.PageOwner || objPrivileges.SystemPrivileges.DWBUser)
                                {
                                    if (objPrivileges.FocalPoint != null && !string.IsNullOrEmpty(objPrivileges.FocalPoint.BookIDs))
                                    {
                                        objCommonUtility = new CommonUtility();
                                        /// Get only the Books where "Owner" = logged in user
                                        //strCamlQuery = @" <Where> <And><Eq><FieldRef Name='Owner' /><Value Type='Lookup'>" + objCommonUtility.GetUserName() + "</Value></Eq><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq></And></Where>";
                                        #region DREAM 4.0 - eWB2.0 - Deletion Module
                                        /// <Eq><FieldRef Name='ToBeDeleted' /><Value Type='Choice'>No</Value></Eq></And> is added.
                                        /// Another And condition added to the CAML query
                                        strCamlQuery = @" <Where><And> <Eq><FieldRef Name='Owner' /><Value Type='Lookup'>" + objCommonUtility.GetUserName() + "</Value></Eq><And><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq> <Or><IsNull><FieldRef Name='ToBeDeleted' /></IsNull><Eq><FieldRef Name='ToBeDeleted' /><Value Type='Choice'>No</Value></Eq></Or></And></And></Where>";
                                        #endregion
                                    }
                                }
                            }
                        }
                        break;
                    }
                case DWBHOME:
                    {
                        objCommonBLL = new CommonBLL();
                        strCamlQuery = @"<Where><Eq><FieldRef Name='Windows_User_ID' /><Value Type='Text'>" +
                         GetUserName() + "</Value></Eq></Where>";

                        DataTable dtListDetails = objCommonBLL.ReadList(strSiteURL, USERLIST, strCamlQuery);

                        string strFavoriteBooksIds = string.Empty;
                        if (dtListDetails.Rows.Count > 0)
                            strFavoriteBooksIds = Convert.ToString(dtListDetails.Rows[0]["FavoriteBooks"]);

                        ((HiddenField)FindControl("hdnUserFavorites")).Value = strFavoriteBooksIds;

                        RadioButtonList rdblFavorites = (RadioButtonList)FindControl("rdoFavourites");
                        if (rdblFavorites != null)
                        {
                            if (rdblFavorites.SelectedIndex == 0)
                            {
                                /// Get all the Books
                                /// strCamlQuery = @"<OrderBy><FieldRef Name='Title' /></OrderBy><Where><And><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq><Gt><FieldRef Name='NoOfActiveChapters' /><Value Type='Number'>0</Value></Gt></And></Where>";
                                #region DREAM 4.0 - eWB2.0 - Deletion Module
                                strCamlQuery = @"<OrderBy><FieldRef Name='Title' /></OrderBy><Where><And><And><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq>  <Or>
         <IsNull>
            <FieldRef Name='ToBeDeleted' />
         </IsNull>

            <Eq>
               <FieldRef Name='ToBeDeleted' />
               <Value Type='Choice'>No</Value>
            </Eq>
                </Or>
         </And><Gt><FieldRef Name='NoOfActiveChapters' /><Value Type='Number'>0</Value></Gt></And></Where>";
                                #endregion
                            }
                            else if (rdblFavorites.SelectedIndex == 1)
                            {
                                /// Get only Favourite Books
                                string[] strBookIds = strFavoriteBooksIds.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                strCamlQuery = CreateCAMLQuery(strBookIds);
                            }
                        }
                        if (dtListDetails != null)
                        {
                            dtListDetails.Dispose();
                        }
                        break;
                    }
                default:
                    strCamlQuery = @"<OrderBy><FieldRef Name='Title' /></OrderBy><Where><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>" +
             strTerminated + "</Value></Eq></Where>";
                    break;

            }

            return strCamlQuery;
        }

        /// <summary>
        /// Gets the CAML query for well book chapters.
        /// </summary>
        /// <returns>CAML query for well book chapters.</returns>
        private string GetCAMLQueryForWellBookChapters()
        {
            StringBuilder sbQuery = new StringBuilder();
            string strBookId = HttpContext.Current.Request.QueryString["BookId"];
            if (string.IsNullOrEmpty(strBookId))
                return string.Empty;
            #region DREAM 4.- eWB2.0-Deletion module
            /// <Eq><FieldRef Name='ToBeDeleted' /><Value Type='Choice'>No</Value></Eq></And> is added.
            /// Another And condition added to the CAML query
            sbQuery.Append("<OrderBy><FieldRef Name='Chapter_Sequence' /></OrderBy><Where><And><And><Eq><FieldRef Name='Book_ID' /><Value Type='Number'>" +
             strBookId + "</Value></Eq>");
            if (ActiveStatus)
            {
                sbQuery.Append("<Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No" +
                      "</Value></Eq></And> <Or><IsNull><FieldRef Name='ToBeDeleted' /></IsNull><Eq><FieldRef Name='ToBeDeleted' /><Value Type='Choice'>No</Value></Eq></Or></And></Where>");
            }
            else
            {
                sbQuery.Append("<Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>Yes" +
                      "</Value></Eq></And><Or><IsNull><FieldRef Name='ToBeDeleted' /></IsNull><Eq><FieldRef Name='ToBeDeleted' /><Value Type='Choice'>No</Value></Eq></Or></And></Where>");
            }
            #endregion 
            return sbQuery.ToString();

        }

        /// <summary>
        /// Gets the CAML query for well book pages.
        /// </summary>
        /// <returns>CAML query for well book pages.</returns>
        private string GetCAMLQueryForWellBookPages()
        {
            string strCamlQuery = string.Empty;
            StringBuilder sbChapterId = new StringBuilder();
            DataTable dtListDetails = null;
            string strTerminated = string.Empty;
            if (!ActiveStatus)
                strTerminated = STATUS_TERMINATED;
            else
                strTerminated = STATUS_ACTIVE;
            string strBookId = HttpContext.Current.Request.QueryString["BookId"];
            if (string.IsNullOrEmpty(strBookId))
                return string.Empty;
            objCommonBLL = new CommonBLL();
            strCamlQuery = @"<Where><Eq><FieldRef Name='Book_ID' /><Value Type='Number'>" +
             strBookId + "</Value></Eq></Where>";
            dtListDetails = objCommonBLL.ReadList(strSiteURL, CHAPTERLIST, strCamlQuery);
            if (dtListDetails != null && dtListDetails.Rows.Count > 0)
            {
                dicChatperDetail = new System.Collections.Generic.Dictionary<string, string>();
                for (int intRowIndex = 0; intRowIndex < dtListDetails.Rows.Count; intRowIndex++)
                {
                    if (!dicChatperDetail.ContainsKey(Convert.ToString(dtListDetails.Rows[intRowIndex]["ID"])))
                    {
                        dicChatperDetail.Add(Convert.ToString(dtListDetails.Rows[intRowIndex]["ID"]), Convert.ToString(dtListDetails.Rows[intRowIndex]["Title"]));
                        sbChapterId.Append(Convert.ToString(dtListDetails.Rows[intRowIndex]["ID"]));
                        sbChapterId.Append(";");
                    }
                }
                strCamlQuery = CreateCAMLQueryForSetOfCondtion(sbChapterId.ToString(), "Chapter_ID", "Number");
                sbChapterId.Remove(0, sbChapterId.Length);
                sbChapterId.Append(strCamlQuery);
                sbChapterId.Append("<Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>" + strTerminated + "</Value></Eq></And></Where>");
                sbChapterId.Insert(0, "<Where><And>");
                sbChapterId.Insert(0, "<OrderBy><FieldRef Name='Page_Name' /></OrderBy>");
            }
            if (dtListDetails != null)
            {
                dtListDetails.Dispose();
            }
            return sbChapterId.ToString();

        }

        /// <summary>
        /// Gets the CAML query for well book summary pages.
        /// </summary>
        /// <returns> CAML query for well book summary pages.</returns>
        private string GetCAMLQueryForWellBookSummaryPages()
        {
            string strCamlQuery = string.Empty;
            StringBuilder sbChapterId = new StringBuilder();
            DataTable dtListDetails = null;
            string strTerminated = string.Empty;
            if (!ActiveStatus)
                strTerminated = STATUS_TERMINATED;
            else
                strTerminated = STATUS_ACTIVE;

            string strbookId = HttpContext.Current.Request.QueryString["BookId"];
            string wellbookStatus = HttpContext.Current.Request.QueryString["pageStatus"];
            string strPageOnwer = HttpContext.Current.Request.QueryString["pageOwner"];
            if (string.IsNullOrEmpty(strbookId))
                return string.Empty;
            objCommonBLL = new CommonBLL();
            strCamlQuery = @"<Where><Eq><FieldRef Name='Book_ID' /><Value Type='Number'>" +
             strbookId + "</Value></Eq></Where>";
            dtListDetails = objCommonBLL.ReadList(strSiteURL, CHAPTERLIST, strCamlQuery);
            if (dtListDetails != null && dtListDetails.Rows.Count > 0)
            {
                dicChatperDetail = new System.Collections.Generic.Dictionary<string, string>();

                for (int intRowIndex = 0; intRowIndex < dtListDetails.Rows.Count; intRowIndex++)
                {
                    if (!dicChatperDetail.ContainsKey(Convert.ToString(dtListDetails.Rows[intRowIndex]["ID"])))
                    {
                        dicChatperDetail.Add(Convert.ToString(dtListDetails.Rows[intRowIndex]["ID"]), Convert.ToString(dtListDetails.Rows[intRowIndex]["Title"]));
                        sbChapterId.Append(Convert.ToString(dtListDetails.Rows[intRowIndex]["ID"]));
                        sbChapterId.Append(";");
                    }
                }
                strCamlQuery = CreateCAMLQueryForSetOfCondtion(sbChapterId.ToString(), "Chapter_ID", "Number");
                sbChapterId.Remove(0, sbChapterId.Length);
                sbChapterId.Append(strCamlQuery);
                if (strPageOnwer.Equals("All") && wellbookStatus.Equals("Total"))
                {
                    sbChapterId.Append("<Eq><FieldRef Name='Sign_Off_Status' /><Value Type='Choice'>" + strSignedOffStatus + "</Value></Eq></And>");
                    sbChapterId.Append("<Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>" + strTerminated + "</Value></Eq></And></Where>");
                    sbChapterId.Insert(0, "<Where><And><And>");
                }
                else if (!strPageOnwer.Equals("All") && wellbookStatus.Equals("Total"))
                {
                    sbChapterId.Append("<Eq><FieldRef Name='Sign_Off_Status' /><Value Type='Choice'>" + strSignedOffStatus + "</Value></Eq></And>");
                    sbChapterId.Append("<Eq><FieldRef Name='Owner' /><Value Type='Text'>" + strPageOnwer + "</Value></Eq></And>");
                    sbChapterId.Append("<Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>" + strTerminated + "</Value></Eq></And></Where>");
                    sbChapterId.Insert(0, "<Where><And><And><And>");
                }
                else if (strPageOnwer.Equals("All") && !wellbookStatus.Equals("Total"))
                {
                    if (wellbookStatus.Equals("SignedOff"))
                    {
                        sbChapterId.Append("<Eq><FieldRef Name='Sign_Off_Status' /><Value Type='Choice'>Yes</Value></Eq></And>");
                    }
                    else if (wellbookStatus.Equals("NotSignedOff"))
                    {
                        sbChapterId.Append("<Eq><FieldRef Name='Sign_Off_Status' /><Value Type='Choice'>No</Value></Eq></And>");
                    }
                    else if (wellbookStatus.Equals("Empty"))
                    {
                        sbChapterId.Append("<Eq><FieldRef Name='Sign_Off_Status' /><Value Type='Choice'>" + strSignedOffStatus + "</Value></Eq></And>");
                        //need to add here also?
                        sbChapterId.Append("<Eq><FieldRef Name='Empty' /><Value Type='Choice'>Yes</Value></Eq></And>");
                    }

                    sbChapterId.Append("<Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>" + strTerminated + "</Value></Eq></And></Where>");
                    if (wellbookStatus.Equals("Empty"))
                        sbChapterId.Insert(0, "<Where><And><And><And>");
                    else
                        sbChapterId.Insert(0, "<Where><And><And>");
                }
                else if (!strPageOnwer.Equals("All") && !wellbookStatus.Equals("Total"))
                {

                    if (wellbookStatus.Equals("SignedOff"))
                    {
                        sbChapterId.Append("<Eq><FieldRef Name='Sign_Off_Status' /><Value Type='Choice'>Yes</Value></Eq></And>");
                    }
                    else if (wellbookStatus.Equals("NotSignedOff"))
                    {
                        sbChapterId.Append("<Eq><FieldRef Name='Sign_Off_Status' /><Value Type='Choice'>No</Value></Eq></And>");
                    }
                    else if (wellbookStatus.Equals("Empty"))
                    {
                        sbChapterId.Append("<Eq><FieldRef Name='Sign_Off_Status' /><Value Type='Choice'>" + strSignedOffStatus + "</Value></Eq></And>");
                        sbChapterId.Append("<Eq><FieldRef Name='Empty' /><Value Type='Choice'>Yes</Value></Eq></And>");
                    }

                    sbChapterId.Append("<Eq><FieldRef Name='Owner' /><Value Type='Text'>" + strPageOnwer + "</Value></Eq></And>");

                    sbChapterId.Append("<Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>" + strTerminated + "</Value></Eq></And></Where>");
                    if (wellbookStatus.Equals("Empty"))
                        sbChapterId.Insert(0, "<Where><And><And><And><And>");
                    else
                        sbChapterId.Insert(0, "<Where><And><And><And>");
                }
                sbChapterId.Insert(0, "<OrderBy><FieldRef Name='Connection_Type' /></OrderBy>");
            }
            if (dtListDetails != null)
            {
                dtListDetails.Dispose();
            }
            return sbChapterId.ToString();

        }

        /// <summary>
        /// Method gives the CAML query to retrieve the Pages mapped to selected Template 
        /// Template ID is retrieved from Query String
        /// </summary>
        /// <returns></returns>
        private string GetCAMLQueryForTemplatePages()
        {
            string strCamlQuery = string.Empty;

            string strTemplateId = HttpContext.Current.Request.QueryString[IDVALUEQUERYSTRING];
            if (string.IsNullOrEmpty(strTemplateId))
                return string.Empty;

            strCamlQuery = @"<OrderBy><FieldRef Name='Page_Sequence' /></OrderBy><Where><Eq><FieldRef Name='Template_ID' /><Value Type='Number'>" +
             strTemplateId + "</Value></Eq></Where>";

            return strCamlQuery;
        }
        /// <summary>
        /// Creates a CAML query from the selected ID
        /// </summary>
        /// <param name="selectedID">The selected ID.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <returns>CAMLquery</returns>
        protected string CreateCAMLQueryForSetOfCondtion(string selectedID, string fieldName, string fieldType)
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
                strCamlQueryBuilder.Append(@"<Eq><FieldRef Name='" + fieldName + "' /><Value Type='" + fieldType + "'>" +
                    strSelectedIDs[0] + "</Value></Eq>");
                return strCamlQueryBuilder.ToString();
            }
            if (strSelectedIDs.Length > 2)
            {
                for (int i = 0; i < strSelectedIDs.Length - 1; i++)
                {
                    if (i != 0)
                        strCamlQueryBuilder.Insert(0, "<Or>");
                    strCamlQueryBuilder.Append(@"<Eq><FieldRef Name='" + fieldName + "' /><Value Type='" + fieldType + "'>" +
                        strSelectedIDs[i] + "</Value></Eq>");
                    if (i != 0)
                        strCamlQueryBuilder.Append("</Or>");
                }

            }

            return strCamlQueryBuilder.ToString();

        }

        /// <summary>
        /// Updates the page sign off status
        /// </summary>
        /// <param name="rowid"></param>
        /// <param name="signOffStatus"></param>
        protected void UpdatePageSignOffStatus(int rowid, string signOffStatus)
        {
            objWellBook = new WellBookBLL();
            strSiteURL = SPContext.Current.Site.Url.ToString();
            if (signOffStatus.Equals("PageSignOff"))
            {
                objWellBook.UpdateWellBookPageSignOffStatus(strSiteURL, CHAPTERPAGEMAPPINGLIST, STATUS_SIGNEDOFF, rowid, AUDIT_ACTION_SIGNEDOFF);
            }
            else
            {
                objWellBook.UpdateWellBookPageSignOffStatus(strSiteURL, CHAPTERPAGEMAPPINGLIST, STATUS_UNSIGNEDOFF, rowid, AUDIT_ACTION_UNSIGNEDOFF);
            }
        }

        /// <summary>
        /// Updates the page sign off status on SignOff button click
        /// Added By: Praveena 
        /// Date:11/09/2010
        /// Reason: For Module Simplify SignOff
        /// </summary>
        /// <param name="rowIDs"></param>
        /// <param name="signOffStatus"></param>
        protected void UpdateBulkPageSignOffStatus(string rowIDs, string signOffStatus)
        {
            objWellBook = new WellBookBLL();
            strSiteURL = SPContext.Current.Site.Url.ToString();
            if (signOffStatus.Equals("PageSignOff"))
            {
                objWellBook.UpdateBulkSignOffStatus(strSiteURL, CHAPTERPAGEMAPPINGLIST, STATUS_SIGNEDOFF, rowIDs, AUDIT_ACTION_SIGNEDOFF);
            }
            else
            {
                objWellBook.UpdateBulkSignOffStatus(strSiteURL, CHAPTERPAGEMAPPINGLIST, STATUS_UNSIGNEDOFF, rowIDs, AUDIT_ACTION_UNSIGNEDOFF);
            }
        }

        /// <summary>
        /// Activates the list item based on the listName
        /// </summary>
        /// <param name="strListName"></param>
        /// <param name="strCamelQuery"></param>
        protected void ActivateList(string listName, int rowId, string auditListName)
        {
            objCommonBLL = new CommonBLL();
            string strSequenceField = string.Empty;
            if (string.Equals(ListName, CHAPTERLIST))
            {
                strSequenceField = "Chapter_Sequence";
            }
            else
            {
                strSequenceField = "Page_Sequence";
            }
            if (string.Equals(ListName, CHAPTERLIST))
            {
                objCommonBLL.UpdateListItemStatus(strSiteURL, ListName, rowId, AuditListName, AUDIT_ACTION_ACTIVATE, STATUS_ACTIVE, strSequenceField);
                objCommonBLL.ListItemStatusUpdateForChapterPages(strSiteURL, CHAPTERPAGEMAPPINGLIST, rowId, STATUS_ACTIVE);
            }
            else if (string.Equals(ListName, MASTERPAGELIST) || string.Equals(ListName, CHAPTERPAGEMAPPINGLIST))
            {
                objCommonBLL.UpdateListItemStatus(strSiteURL, ListName, rowId, AuditListName, AUDIT_ACTION_ACTIVATE, STATUS_ACTIVE, strSequenceField);
            }
            else if (string.Equals(ListName, TEMPLATELIST) || string.Equals(ListName, WELLBOOKLIST))
            {
                objCommonBLL.ActivateListValues(strSiteURL, listName, rowId, auditListName, false, true);
            }
            else if (string.Equals(ListName, USERLIST) || string.Equals(ListName, TEAMLIST) || string.Equals(ListName, TEAMSTAFFLIST))
            {
                objCommonBLL.ActivateListValues(strSiteURL, listName, rowId, auditListName, false, false);
            }
            else
            {
                objCommonBLL.ActivateListValues(strSiteURL, listName, rowId, auditListName, true, true);
            }
        }

        /// <summary>
        /// Terminates the list element.
        /// </summary>
        /// <param name="strListName">Name of the STR list.</param>
        /// <param name="rowId">The row id.</param>
        /// <param name="strAuditListName">Name of the STR audit list.</param>
        protected void TerminateListElement(string listName, int rowId, string auditListName)
        {
            objCommonBLL = new CommonBLL();
            string strSequenceField = string.Empty;
            if (string.Equals(ListName, CHAPTERLIST))
            {
                strSequenceField = "Chapter_Sequence";
            }
            else
            {
                strSequenceField = "Page_Sequence";
            }
            if (string.Equals(ListName, CHAPTERLIST))
            {
                objCommonBLL.UpdateListItemStatus(strSiteURL, ListName, rowId, AuditListName, AUDIT_ACTION_TERMINATE, STATUS_TERMINATED, strSequenceField);
                objCommonBLL.ListItemStatusUpdateForChapterPages(strSiteURL, CHAPTERPAGEMAPPINGLIST, rowId, STATUS_TERMINATED);

            }
            else if (string.Equals(ListName, MASTERPAGELIST) || string.Equals(ListName, CHAPTERPAGEMAPPINGLIST))
            {
                objCommonBLL.UpdateListItemStatus(strSiteURL, ListName, rowId, AuditListName, AUDIT_ACTION_TERMINATE, STATUS_TERMINATED, strSequenceField);
            }
            else if (string.Equals(ListName, WELLBOOKLIST) || string.Equals(ListName, TEMPLATELIST))
            {
                objCommonBLL.TerminateListValues(strSiteURL, listName, rowId, auditListName, false, true);
            }
            else if (string.Equals(ListName, USERLIST) || string.Equals(ListName, TEAMLIST) || string.Equals(ListName, TEAMSTAFFLIST))
            {
                objCommonBLL.TerminateListValues(strSiteURL, listName, rowId, auditListName, false, false);
            }
            else
            {
                objCommonBLL.TerminateListValues(strSiteURL, listName, rowId, auditListName, true, true);
            }
        }

        /// <summary>
        /// Sets the records data object.
        /// </summary>
        /// <param name="dtListDetails">The dt list details.</param>
        private void SetRecordsDataObject(DataTable listDetails)
        {
            objRecords = new Records();
            objRecords.Name = ListReportName;
            objRecords.Record = SetListDetail(listDetails);
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
                    if (objDataRow["ID"] != null)
                        objRecord.RecordNumber = objDataRow["ID"].ToString();
                    objRecord.RecordInfo = SetRecordInfo(objDataRow);
                    //objRecord.RecordHistories = SetRecordHistories(objDataRow["ID"].ToString());
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
        /// Sets the record histories.
        /// </summary>
        /// <param name="selectedID">The selected ID.</param>
        /// <returns>RecordHistories.</returns>
        private RecordHistories SetRecordHistories(string selectedID)
        {
            string strCamlQuery = string.Empty;
            string strTerminated = string.Empty;
            DataRow[] objSelectedIDHistory = null;
            RecordHistories objRecordHistories = null;
            ArrayList arlHistory = new ArrayList();
            int intIndex = 0;
            try
            {
                if (dtHistoryDetails != null && dtHistoryDetails.Rows.Count > 0)
                {
                    objRecordHistories = new RecordHistories();
                    objSelectedIDHistory = dtHistoryDetails.Select("Master_ID =" + selectedID);
                    foreach (DataRow objDataRow in objSelectedIDHistory)
                    {
                        arlHistory.Add(SetRecordHistory(objDataRow, intIndex + 1));
                        intIndex++;
                    }
                    objRecordHistories.History = arlHistory;
                }

                return objRecordHistories;
            }
            catch
            { throw; }
        }

        /// <summary>
        /// Sets the record history.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <param name="order">The order.</param>
        /// <returns>RecordHistory.</returns>
        private RecordHistory SetRecordHistory(DataRow dataRow, int order)
        {
            RecordHistory objRecordHistory = null;
            try
            {
                objRecordHistory = new RecordHistory();
                objRecordHistory.Order = order.ToString();
                objRecordHistory.Attributes = SetAuditHistory(dataRow);
                return objRecordHistory;
            }
            catch { throw; }
        }

        /// <summary>
        /// Sets the audit history.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <returns>ArrayList</returns>
        private ArrayList SetAuditHistory(DataRow dataRow)
        {
            ArrayList arlAttributes = null;
            DateTime dtTime = DateTime.MinValue;
            try
            {
                arlAttributes = new ArrayList();
                dtTime = (DateTime)dataRow["Date"];
                arlAttributes.Add(CreateAttribute("Date / Time", dtTime.ToString("dd-MMM-yy HH:mm "), "true"));
                arlAttributes.Add(CreateAttribute("User", dataRow["Title"].ToString(), "true"));
                arlAttributes.Add(CreateAttribute("Comments", dataRow["Audit_Action"].ToString(), "true"));
                return arlAttributes;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Set the attributes for each record and returns the RecordInfo object
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>RecordInfo</returns>
        private RecordInfo SetRecordInfo(DataRow dataRow)
        {
            RecordInfo objRecordInfo = null;
            try
            {
                objRecordInfo = new RecordInfo();
                switch (ListReportName.ToString())
                {
                    case "Audit Trail":
                        {
                            objRecordInfo.Attributes = SetAuditHistory(dataRow);
                            break;
                        }
                    case MASTERPAGEREPORT:
                        {
                            objRecordInfo.Attributes = SetMasterAttributes(dataRow);
                            break;
                        }
                    case TEMPLATEREPORT:
                        {
                            objRecordInfo.Attributes = SetTemplateAttributes(dataRow);
                            break;
                        }
                    case TEMPLATEPAGESREPORT:
                        {
                            objRecordInfo.Attributes = SetTemplatePagesAttributes(dataRow);
                            break;
                        }
                    case WELLBOOKREPORT:
                        {
                            objRecordInfo.Attributes = SetBookAttributes(dataRow);
                            break;
                        }
                    case CHAPTERPAGEMAPPINGREPORT:
                        {
                            objRecordInfo.Attributes = SetBookPagesAttributes(dataRow);
                            break;
                        }
                    case CHAPTERPAGEREPORT:
                        {
                            objRecordInfo.Attributes = SetChapterPagesAttributes(dataRow);
                            break;
                        }
                    case CHAPTERREPORT:
                        {
                            objRecordInfo.Attributes = SetBookChaptersAttributes(dataRow);
                            break;
                        }
                    case USERREGISTRATION:
                        {
                            objRecordInfo.Attributes = SetUserAttributes(dataRow);
                            break;
                        }
                    case TEAMREGISTRATION:
                        {
                            objRecordInfo.Attributes = SetTeamAttributes(dataRow);
                            break;
                        }
                    case STAFFREGISTRATION:
                        {
                            objRecordInfo.Attributes = SetStaffAttributes(dataRow);
                            break;
                        }
                    case DWBHOME:
                        {
                            objRecordInfo.Attributes = SetDWBHomeAttributes(dataRow);
                            break;
                        }
                    case WELLBOOKPAGEVIEW:
                        {
                            objRecordInfo.Attributes = SetWellBookPageSummaryAttributes(dataRow);
                            break;
                        }
                }
                return objRecordInfo;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Sets the master attributes.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <returns>ArrayList.</returns>
        private ArrayList SetMasterAttributes(DataRow dataRow)
        {
            ArrayList arlAttributes = null;

            try
            {
                arlAttributes = new ArrayList();
                arlAttributes.Add(CreateAttribute("Seq", Convert.ToString(dataRow["Page_Sequence"]), "true"));
                arlAttributes.Add(CreateAttribute("Name", dataRow["Title"].ToString(), "true"));
                arlAttributes.Add(CreateAttribute("Asset Type", dataRow["Asset_Type"].ToString(), "true"));
                arlAttributes.Add(CreateAttribute("Sign Off Discipline", dataRow["Sign_Off_Discipline"].ToString(), "true"));
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
        /// <returns>Attributes.</returns>
        private Attributes CreateAttribute(string name, string value, string display)
        {
            int intOutput;
            Attributes objAttribute = new Attributes();
            objAttribute.Name = name;
            objAttribute.Value = value;
            objAttribute.Display = display;
            bool isNumeric = int.TryParse(value, out intOutput);

            if (isNumeric)
            {
                objAttribute.DataType = "number";
            }
            else if (string.Equals(value.GetType().Name, "String"))
            {
                objAttribute.DataType = "text";
            }
            return objAttribute;
        }

        /// <summary>
        /// Sets Template attributes
        /// Name,Asset Type
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private ArrayList SetTemplateAttributes(DataRow dataRow)
        {
            ArrayList arlAttributes = null;
            try
            {
                arlAttributes = new ArrayList();
                arlAttributes.Add(CreateAttribute("Name", dataRow["Title"].ToString(), "true"));
                arlAttributes.Add(CreateAttribute("Asset Type", dataRow["Asset_Type"].ToString(), "true"));

                return arlAttributes;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Sets Template attributes
        /// Name,Asset Type
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private ArrayList SetWellBookPageSummaryAttributes(DataRow dataRow)
        {
            ArrayList arlAttributes = null;
            try
            {
                arlAttributes = new ArrayList();

                arlAttributes.Add(CreateAttribute("Chapter Name", dataRow[CHAPTERTITLE_COLUMNNAME].ToString(), "true"));
                if (dataRow["Connection_Type"].ToString().IndexOf("1") == 0)
                    arlAttributes.Add(CreateAttribute("Type", "1", "true"));
                else if (dataRow["Connection_Type"].ToString().IndexOf("2") == 0)
                    arlAttributes.Add(CreateAttribute("Type", "2", "true"));
                else
                    arlAttributes.Add(CreateAttribute("Type", "3", "true"));
                arlAttributes.Add(CreateAttribute("Page Title", dataRow["Page_Name"].ToString(), "true"));
                arlAttributes.Add(CreateAttribute("Discipline", dataRow["Discipline"].ToString(), "true"));
                arlAttributes.Add(CreateAttribute("Owner", dataRow["Owner"].ToString(), "true"));
                arlAttributes.Add(CreateAttribute("Empty", dataRow["Empty"].ToString(), "true"));
                arlAttributes.Add(CreateAttribute("Signed Off", dataRow["Sign_Off_Status"].ToString(), "false"));
                arlAttributes.Add(CreateAttribute("Page_URL", dataRow["Page_URL"].ToString(), "false"));
                arlAttributes.Add(CreateAttribute("Chapter_ID", dataRow["Chapter_ID"].ToString(), "false"));
                if (dataRow["Sign_Off_Status"].ToString() == "Yes")
                {
                    arlAttributes.Add(CreateAttribute("Last_SO_Date", dataRow["Last_SO_Date"].ToString(), "true"));
                }
                else
                {
                    arlAttributes.Add(CreateAttribute("Last_SO_Date", dataRow["Last_SO_Date"].ToString(), "false"));
                }
                return arlAttributes;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Sets the TemplatePages attributes
        /// Seq,Master Page Name
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>ArrayList</returns>
        private ArrayList SetTemplatePagesAttributes(DataRow dataRow)
        {
            ArrayList arlAttributes = null;
            try
            {
                arlAttributes = new ArrayList();
                arlAttributes.Add(CreateAttribute("Seq", dataRow["Page_Sequence"].ToString(), "true"));
                arlAttributes.Add(CreateAttribute("Master Page Name", dataRow["Master_Page_Name"].ToString(), "true"));

                return arlAttributes;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Sets the book attributes.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <returns>ArrayList.</returns>
        private ArrayList SetBookAttributes(DataRow dataRow)
        {
            ArrayList arlAttributes = null;
            string strCamlQuery = string.Empty;
            string strViewFields = string.Empty;
            string strPageOwner = string.Empty;
            DataTable dtUser = null;
            try
            {
                arlAttributes = new ArrayList();
                arlAttributes.Add(CreateAttribute("Name", Convert.ToString(dataRow["Title"]), "true"));
                arlAttributes.Add(CreateAttribute("Team", dataRow["Team"].ToString(), "true"));

                strCamlQuery = @"<Where><Eq><FieldRef Name='Windows_User_ID' /><Value Type='Text'>" + dataRow["Owner"].ToString() + "</Value></Eq></Where>";
                strViewFields = @"<FieldRef Name='ID' /><FieldRef Name='DWBUserName' />";
                objCommonBLL = new CommonBLL();
                dtUser = objCommonBLL.ReadList(strSiteURL, USERLIST, strCamlQuery, strViewFields);
                if (dtUser != null && dtUser.Rows.Count > 0)
                {
                    strPageOwner = dtUser.Rows[0]["DWBUserName"].ToString();
                }

                arlAttributes.Add(CreateAttribute("Well Book Owner", strPageOwner, "true"));
                return arlAttributes;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Sets the DWB home attributes.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <returns>ArrayList</returns>
        private ArrayList SetDWBHomeAttributes(DataRow dataRow)
        {
            ArrayList arlAttributes = null;
            try
            {
                arlAttributes = new ArrayList();

                arlAttributes.Add(CreateAttribute("Name", Convert.ToString(dataRow["Title"]), "true"));
                arlAttributes.Add(CreateAttribute("Team", dataRow["Team"].ToString(), "true"));

                RadioButtonList rdblFavourites = (RadioButtonList)FindControl("rdoFavourites");
                /// Set IsFavorite to Yes/No only if All radio button is selected
                /// Set IsFavorite always to No if Favourites is selected
                if (rdblFavourites != null)
                {
                    if (rdblFavourites.SelectedIndex == 0)
                    {

                        if (((HiddenField)FindControl("hdnUserFavorites")).Value.Contains(";" + dataRow["ID"] + ";"))
                            arlAttributes.Add(CreateAttribute("IsFavorite", "Yes", "true"));
                        else
                            arlAttributes.Add(CreateAttribute("IsFavorite", "No", "true"));
                    }
                    else if (rdblFavourites.SelectedIndex == 1)
                    {
                        arlAttributes.Add(CreateAttribute("IsFavorite", "No", "true"));
                    }
                }
                return arlAttributes;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Sets the book chapters attributes.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <returns>ArrayList</returns>
        private ArrayList SetBookChaptersAttributes(DataRow dataRow)
        {
            ArrayList arlAttributes = null;
            try
            {
                arlAttributes = new ArrayList();

                arlAttributes.Add(CreateAttribute("Seq", Convert.ToString(dataRow["Chapter_Sequence"]), "true"));
                arlAttributes.Add(CreateAttribute("Chapter Name", Convert.ToString(dataRow["Title"]), "true"));
                arlAttributes.Add(CreateAttribute("Asset Type", Convert.ToString(dataRow["Asset_Type"]), "true"));
                string strCAMLQuery = @"<Where><Eq><FieldRef Name='ID'/><Value Type='ID'>" + dataRow["Template_ID"].ToString() + "</Value></Eq></Where>";
                string strViewFields = @"<FieldRef Name='ID'/><FieldRef Name='Title'/>";
                objCommonBLL = new CommonBLL();
                DataTable dtTemplate = objCommonBLL.ReadList(strSiteURL, TEMPLATELIST, strCAMLQuery, strViewFields);
                if (dtTemplate != null && dtTemplate.Rows.Count > 0)
                {
                    arlAttributes.Add(CreateAttribute("Template", Convert.ToString(dtTemplate.Rows[0]["Title"]), "true"));
                }
                else
                {
                    arlAttributes.Add(CreateAttribute("Template", string.Empty, "true"));
                }
                if (dtTemplate != null)
                {
                    dtTemplate.Dispose();
                }
                return arlAttributes;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Sets the book pages attributes.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <returns>ArrayList</returns>
        private ArrayList SetBookPagesAttributes(DataRow dataRow)
        {
            ArrayList arlAttributes = null;
            try
            {
                arlAttributes = new ArrayList();
                arlAttributes.Add(CreateAttribute("Chapter Name", dataRow[CHAPTERTITLE_COLUMNNAME].ToString(), "true"));
                arlAttributes.Add(CreateAttribute("Page Title ", dataRow["Page_Name"].ToString(), "true"));
                arlAttributes.Add(CreateAttribute("Discipline", dataRow["Discipline"].ToString(), "true"));
                arlAttributes.Add(CreateAttribute("Owner", dataRow["Owner"].ToString(), "true"));
                return arlAttributes;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Sets the chapter pages attributes.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <returns>ArrayList.</returns>
        private ArrayList SetChapterPagesAttributes(DataRow dataRow)
        {
            ArrayList arlAttributes = null;
            try
            {
                arlAttributes = new ArrayList();

                arlAttributes.Add(CreateAttribute("Seq", Convert.ToString(dataRow["Page_Sequence"]), "true"));
                arlAttributes.Add(CreateAttribute("Page Name", Convert.ToString(dataRow["Page_Name"]), "true"));
                return arlAttributes;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Sets the User Attributes
        /// User Name,Windows User Id,Discipline,Privileges
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>ArrayList</returns>
        private ArrayList SetUserAttributes(DataRow dataRow)
        {
            ArrayList arlAttributes = null;
            try
            {
                arlAttributes = new ArrayList();
                arlAttributes.Add(CreateAttribute("User Name", dataRow["DWBUserName"].ToString(), "false"));
                arlAttributes.Add(CreateAttribute("Windows User Id", dataRow["Windows_User_ID"].ToString(), "true"));
                arlAttributes.Add(CreateAttribute("Discipline", dataRow["Discipline"].ToString(), "true"));
                arlAttributes.Add(CreateAttribute("Privileges", dataRow["Privileges"].ToString(), "true"));

                return arlAttributes;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Set Team Attributes
        /// Name,Asset Owner
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>ArrayList</returns>
        private ArrayList SetTeamAttributes(DataRow dataRow)
        {
            ArrayList arlAttributes = null;
            try
            {
                arlAttributes = new ArrayList();
                arlAttributes.Add(CreateAttribute("Name", dataRow["Title"].ToString(), "true"));
                arlAttributes.Add(CreateAttribute("Asset Owner", dataRow["Asset_Owner"].ToString(), "true"));

                return arlAttributes;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Set Staff attributes
        /// Discipline,Rank,User Name,Privileges
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>ArrayList</returns>
        private ArrayList SetStaffAttributes(DataRow dataRow)
        {
            ArrayList arlAttributes = null;
            try
            {
                arlAttributes = new ArrayList();
                arlAttributes.Add(CreateAttribute("Discipline", dataRow["Discipline"].ToString(), "true"));
                arlAttributes.Add(CreateAttribute("Rank", dataRow["User_Rank"].ToString(), "true"));
                arlAttributes.Add(CreateAttribute("User Name", dataRow["Title"].ToString(), "true"));
                arlAttributes.Add(CreateAttribute("Privilege", dataRow["Privilege"].ToString(), "true"));

                return arlAttributes;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Transforms the list detail.
        /// </summary>
        /// <param name="listType">Type of the list.</param>
        /// <param name="noOfRecords">The no of records.</param>
        protected void TransformListDetail(int noOfRecords)
        {
            XmlTextReader xmlTextReader = null;
            XslTransform xslTransform = null;
            XmlDocument objXmlDocForXSL = null;
            XsltArgumentList xsltArgsList = null;
            XPathDocument objXPathDocument = null;
            MemoryStream objMemoryStream = null;
            int intrecordsPerPage = 0;
            int intPageNumber = 0;
            int intMaxPages = 5;
            int intCurrentPage = 1;
            string strSortBy = string.Empty;
            string strSortType = string.Empty;
            object objSessionUserPreference = null;
            Shell.SharePoint.DREAM.Business.Entities.UserPreferences objPreferences = null;
            try
            {
                objCommonBLL = new CommonBLL();
                xmlTextReader = objCommonBLL.GetXSLTemplate("List Viewer", strSiteURL);
                if (xmlListDocument != null && xmlTextReader != null)
                {
                    xslTransform = new XslTransform();
                    XslCompiledTransform xslTransformTest = new XslCompiledTransform();
                    objXmlDocForXSL = new XmlDocument();
                    xsltArgsList = new XsltArgumentList();
                    objMemoryStream = new MemoryStream();
                    xmlListDocument.Save(objMemoryStream);
                    objMemoryStream.Position = 0;
                    objXPathDocument = new XPathDocument(objMemoryStream);

                    /// Inititlize the XSL
                    objXmlDocForXSL.Load(xmlTextReader);
                    xslTransform.Load(objXmlDocForXSL);

                    xsltArgsList.AddParam("historyColumn", string.Empty, IsExpandCollapseApplicable.ToString());
                    xsltArgsList.AddParam("editColumn", string.Empty, IsEditApplicable.ToString());
                    if (ActiveStatus)
                    {
                        xsltArgsList.AddParam("listItemAction", string.Empty, STATUSTERMINATE);
                    }
                    else
                    {
                        xsltArgsList.AddParam("listItemAction", string.Empty, STATUSACTIVATE);
                    }
                    xsltArgsList.AddParam("addMasterLinkColumn", string.Empty,
                        IsAddMasterApplicable.ToString());
                    xsltArgsList.AddParam("viewMasterLinkColumn", string.Empty,
                        IsViewMasterApplicable.ToString());
                    xsltArgsList.AddParam("listType", string.Empty, ListReportName);
                    xsltArgsList.AddParam("activeStatus", string.Empty, ActiveStatus.ToString().ToLower());
                    /// <!-- Set the number of records per page-->
                    /// <xsl:param name="recordsPerPage" select="0" />                    
                    objSessionUserPreference = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString());
                    if (objSessionUserPreference != null)
                    {
                        objPreferences = (Shell.SharePoint.DREAM.Business.Entities.UserPreferences)objSessionUserPreference;
                        intrecordsPerPage = Convert.ToInt32(objPreferences.RecordsPerPage);
                    }
                    xsltArgsList.AddParam("recordsPerPage", string.Empty, intrecordsPerPage);
                    xsltArgsList.AddParam("AuditListName", string.Empty, AuditListName);


                    if (!(string.Compare(ListReportName, WELLBOOKPAGEVIEW, true) == 0))
                    {
                        /// <!-- Page Number field -->
                        /// <xsl:param name="pageNumber" select="1" />
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
                        else
                        {
                            strSortType = "descending";
                        }

                    }
                    else
                    {
                        intPageNumber = string.IsNullOrEmpty(pageNumber) ? 0 : Convert.ToInt32(pageNumber);
                        sortType = string.Equals(sortType, "ASC") ? "ascending" : "descending";
                        xsltArgsList.AddParam("sortBy", string.Empty, sortBy);
                        xsltArgsList.AddParam("sortType", string.Empty, sortType);
                        if (!(string.IsNullOrEmpty(sortBy)) && !(string.IsNullOrEmpty(sortBy)))
                        {
                            strSortBy = sortBy;
                        }

                    }

                    int intNoofPages = 0;
                    if (intrecordsPerPage > 0)
                    {
                        intNoofPages = noOfRecords / intrecordsPerPage;
                    }
                    if (intPageNumber > intNoofPages)
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
                        objCommonUtility = new CommonUtility();
                        string strURL = GetCurrentPageName(true);
                        xsltArgsList.AddParam("CurrentPageName", string.Empty, strURL);
                    }
                    else
                    {
                        xsltArgsList.AddParam("CurrentPageName", string.Empty, HttpContext.Current.Request.Url.AbsolutePath + "?");
                    }
                    if (!(string.Compare(ListReportName, WELLBOOKPAGEVIEW, true) == 0))
                    {
                        xsltArgsList.AddParam("sortBy", string.Empty, strSortBy);
                        xsltArgsList.AddParam("sortType", string.Empty, strSortType);
                    }

                    XmlNode xmlNode = xmlListDocument.SelectSingleNode("records/record/recordInfo/attribute[@name='" + strSortBy + "']");
                    if (xmlNode != null)
                    {
                        xsltArgsList.AddParam("sortDataType", string.Empty, xmlNode.Attributes["datatype"].Value);
                    }
                    #region DREAM 4.0 - eWB 2.0 - AJAX Implemenation
                    //  xslTransform.Transform(objXPathDocument, xsltArgsList, HttpContext.Current.Response.Output);

                    StringBuilder builder = new StringBuilder();
                    StringWriter stringWriter = new StringWriter(builder);
                    xslTransform.Transform(objXPathDocument, xsltArgsList, stringWriter);
                    strResultTable = stringWriter.GetStringBuilder();
                    #endregion 
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

        protected void TransformAuditListDetail()
        {
            XmlTextReader xmlTextReader = null;
            XslTransform xslTransform = null;
            XmlDocument objXmlDocForXSL = null;
            XsltArgumentList xsltArgsList = null;
            XPathDocument objXPathDocument = null;
            MemoryStream objMemoryStream = null;
            string strSortBy = string.Empty;
            string strSortType = string.Empty;
            try
            {
                objCommonBLL = new CommonBLL();
                xmlTextReader = objCommonBLL.GetXSLTemplate("List Viewer", strSiteURL);
                if (xmlListDocument != null && xmlTextReader != null)
                {
                    xslTransform = new XslTransform();
                    XslCompiledTransform xslTransformTest = new XslCompiledTransform();
                    objXmlDocForXSL = new XmlDocument();
                    xsltArgsList = new XsltArgumentList();
                    objMemoryStream = new MemoryStream();
                    xmlListDocument.Save(objMemoryStream);
                    objMemoryStream.Position = 0;
                    objXPathDocument = new XPathDocument(objMemoryStream);

                    /// Inititlize the XSL
                    objXmlDocForXSL.Load(xmlTextReader);
                    xslTransform.Load(objXmlDocForXSL);

                    /// <!-- Set the number of records per page-->
                    /// <xsl:param name="recordsPerPage" select="0" />                    

                    if (!string.IsNullOrEmpty(HttpContext.Current.Request.Url.Query))
                    {
                        objCommonUtility = new CommonUtility();
                        string strURL = GetCurrentPageName(true);
                        xsltArgsList.AddParam("CurrentPageName", string.Empty, strURL);
                    }
                    else
                    {
                        xsltArgsList.AddParam("CurrentPageName", string.Empty, HttpContext.Current.Request.Url.AbsolutePath + "?");
                    }

                    xslTransform.Transform(objXPathDocument, xsltArgsList,
                        HttpContext.Current.Response.Output);

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
        /// Transforms the list detail DWB home.
        /// </summary>
        /// <param name="noOfRecords">The no of records.</param>
        protected void TransformListDetailDWBHome(int noOfRecords)
        {
            XmlTextReader xmlTextReader = null;
            XslCompiledTransform xslTransform = null;
            XmlDocument objXmlDocForXSL = null;
            XsltArgumentList xsltArgsList = null;
            XPathDocument objXPathDocument = null;
            MemoryStream objMemoryStream = null;

            /// For Paging and Sorting
            int intrecordsPerPage = 0;
            int intPageNumber = 0;
            int intMaxPages = 5;
            int intCurrentPage = 1;
            string strSortBy = string.Empty;
            string strSortType = string.Empty;
            object objSessionUserPreference = null;
            Shell.SharePoint.DREAM.Business.Entities.UserPreferences objPreferences = null;
            try
            {
                objCommonBLL = new CommonBLL();
                xmlTextReader = objCommonBLL.GetXSLTemplate("List Viewer DWBHome", strSiteURL);
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

                    /// Inititlize the XSL
                    objXmlDocForXSL.Load(xmlTextReader);
                    xslTransform.Load(objXmlDocForXSL);
                    xsltArgsList.AddParam("listType", string.Empty, ListReportName);

                    /// Parameters for Paging/Sorting
                    xsltArgsList.AddParam("activeStatus", string.Empty, ActiveStatus.ToString().ToLower());
                    /// <!-- Set the number of records per page-->
                    /// <xsl:param name="recordsPerPage" select="0" />                   
                    objSessionUserPreference = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString());
                    if (objSessionUserPreference != null)
                    {
                        objPreferences = (Shell.SharePoint.DREAM.Business.Entities.UserPreferences)objSessionUserPreference;
                        intrecordsPerPage = Convert.ToInt32(objPreferences.RecordsPerPage);
                    }
                    xsltArgsList.AddParam("recordsPerPage", string.Empty, intrecordsPerPage);

                    /// <!-- Page Number field -->
                    /// <xsl:param name="pageNumber" select="1" />
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
                    else
                    {
                        strSortType = "descending";
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
                        objCommonUtility = new CommonUtility();
                        string strURL = GetCurrentPageName(true);
                        xsltArgsList.AddParam("CurrentPageName", string.Empty, strURL);
                    }
                    else
                    {
                        xsltArgsList.AddParam("CurrentPageName", string.Empty, HttpContext.Current.Request.Url.AbsolutePath + "?");
                    }
                    xsltArgsList.AddParam("sortBy", string.Empty, strSortBy);
                    xsltArgsList.AddParam("sortType", string.Empty, strSortType);

                    XmlNode xmlNode = xmlListDocument.SelectSingleNode("records/record/recordInfo/attribute[@name='" + strSortBy + "']");
                    if (xmlNode != null)
                    {
                        xsltArgsList.AddParam("sortDataType", string.Empty, xmlNode.Attributes["datatype"].Value);
                    }
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
            writer.Write("<link href=\"/_Layouts/DREAM/Styles/DWBHistoryReport.css\" rel=\"stylesheet\" type=\"text/css\" />");
            writer.Write("<link href=\"/_Layouts/DREAM/Styles/DWBReportLayout.css\" rel=\"stylesheet\" type=\"text/css\" />");
            writer.Write("<link href=\"/_Layouts/DREAM/Styles/DWBStyleSheetRel2_0.css\" rel=\"stylesheet\" type=\"text/css\" />");
            writer.Write("<SCRIPT type=\"text/javascript\" SRC=\"/_layouts/dream/Javascript/DREAMJavaScriptFunctionsRel2_1.js\"></SCRIPT>");
            writer.Write("<SCRIPT type=\"text/javascript\" SRC=\"/_layouts/dream/Javascript/DWBJavascriptFunctionRel2_0.js\"></SCRIPT>");
            writer.Write("<table class=\"tableAdvSrchBorder\" cellspacing=\"0\" cellpadding=\"4\" border=\"0\" width=\"100%\">");
            writer.Write("<tr><td class=\"DWBtdAdvSrchHeader\" colspan=\"3\" valign=\"top\"><B>" + title + "</b></td></tr>");
        }

        /// <summary>
        /// Creates the CAML query.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>string.</returns>
        private static string CreateCAMLQuery(string[] parameters)
        {
            StringBuilder sbCAMLQuery = new StringBuilder();
            // "AND" each parameter to the query 
            for (int intLength = 0; intLength < parameters.Length; intLength++)
            {
                AppendEQ(sbCAMLQuery, parameters[intLength], "ID", "Counter");

                if (intLength > 0)
                {
                    sbCAMLQuery.Insert(0, "<Or>");
                    sbCAMLQuery.Append("</Or>");
                }
            }
            if (parameters.Length == 0)
            {
                sbCAMLQuery.Insert(0, "<Where><IsNull><FieldRef Name='ID' /></IsNull>");
                sbCAMLQuery.Append("</Where>");
            }
            else
            {
                //sbCAMLQuery.Insert(0, "<OrderBy><FieldRef Name='Title' /></OrderBy><Where><And><And><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq><Gt><FieldRef Name='NoOfActiveChapters' /><Value Type='Number'>0</Value></Gt></And>");
                //sbCAMLQuery.Append("</And></Where>");
                #region DREAM 4.0 - eWB2.0 - Deletion Module
                sbCAMLQuery.Insert(0, "<OrderBy><FieldRef Name='Title' /></OrderBy><Where><And><And><And><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq><Or><IsNull><FieldRef Name='ToBeDeleted' /></IsNull><Eq><FieldRef Name='ToBeDeleted' /><Value Type='Choice'>No</Value></Eq></Or></And><Gt><FieldRef Name='NoOfActiveChapters' /><Value Type='Number'>0</Value></Gt></And>");
                sbCAMLQuery.Append("</And></Where>");
                #endregion 
            }


            return sbCAMLQuery.ToString();
        }

        /// <summary>
        /// Appends the EQ.
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="value">The value.</param>
        /// <param name="internalname">The internalname.</param>
        /// <param name="internaltype">The internaltype.</param>
        private static void AppendEQ(StringBuilder sbCAMLQuery, string value, string internalname, string internaltype)
        {
            sbCAMLQuery.Append("<Eq>");
            sbCAMLQuery.Append("<FieldRef Name='" + internalname + "'/>");
            sbCAMLQuery.AppendFormat("<Value Type='" + internaltype + "'>{0}</Value>", value);
            sbCAMLQuery.Append("</Eq>");
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

        /// <summary>
        /// Binds the Data to PageName Dropdown
        /// Added By: Praveena 
        /// Date:10/09/2010
        /// Reason: To Populate Values in PageName Dropdown
        /// </summary>
        /// <param name="cboList"></param>
        /// <param name="strListname"></param>
        /// <param name="strCAMLQuery"></param>
        protected void BindPageNameDropDown(DropDownList cboList, string listName)
        {
            string strBookId = HttpContext.Current.Request.QueryString["BookId"];
            string strCamlQuery = @"<Where><Eq><FieldRef Name='Book_ID' />
                 <Value Type='Number'>" + strBookId + "</Value></Eq></Where>";
            objWellBook = new WellBookBLL();
            cboList.Items.Clear();
            cboList.DataSource = objWellBook.GetPageNamesList(strSiteURL, listName, strCamlQuery);
            cboList.DataTextField = "key";
            cboList.DataBind();

        }

        /// <summary>
        /// Binds the Data to ChapterName Dropdown        
        /// Added By: Praveena 
        /// Date:3/09/2010
        /// Reason: To Populate Values in ChapterName ListBox
        /// </summary>
        /// <param name="cboList"></param>
        /// <param name="strListname"></param>
        /// <param name="strCAMLQuery"></param>
        protected void BindChapterNameListBox(ListBox listBox, string strListname)
        {
            string strBookId = HttpContext.Current.Request.QueryString["BookId"];
            string strCamlQuery = @"<Where><And><Eq><FieldRef Name='Book_ID' />
                 <Value Type='Number'>" + strBookId + "</Value></Eq><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq></And></Where>";
            DataTable dtListData = null;
            DataRow drListData;
            ListItem lstItem;
            string strViewFields = string.Empty;
            strViewFields = @"<FieldRef Name='ID' /><FieldRef Name='Title' />";
            objCommonBLL = new CommonBLL();
            dtListData = objCommonBLL.ReadList(strSiteURL, strListname, strCamlQuery, strViewFields);
            if (dtListData != null && dtListData.Rows.Count > 0)
            {
                /// Loop through the list and add items
                listBox.Items.Clear();
                for (int intRowIndex = 0; intRowIndex < dtListData.Rows.Count; intRowIndex++)
                {
                    drListData = dtListData.Rows[intRowIndex];
                    lstItem = new ListItem();
                    lstItem.Text = drListData[DWBTITLECOLUMN].ToString();
                    lstItem.Value = drListData[DWBIDCOLUMN].ToString();
                    listBox.Items.Add(lstItem);
                }
            }

            if (dtListData != null)
                dtListData.Dispose();
        }


        #region FixingFormAction


        /// <summary>
        /// Ensures the panel fix.
        /// </summary>
        /// <param name="type">The type.</param>
        protected void EnsurePanelFix(Type type)
        {
            if (this.Page.Form != null)
            {
                string formOnSubmitAtt = this.Page.Form.Attributes["onsubmit"];
                if (formOnSubmitAtt == "return _spFormOnSubmitWrapper();")
                {
                    this.Page.Form.Attributes["onsubmit"] = "_spFormOnSubmitWrapper();";
                }
            }
            ScriptManager.RegisterStartupScript(this, type, "UpdatePanelFixup", "_spOriginalFormAction = document.forms[0].action; _spSuppressFormOnSubmitWrapper=true;", true);
        }
        #endregion FixingFormAction

        #region DREAM 4.0- eWB 2.0

        #region Delete List Item
        protected void DeleteListItem(int itemID,string reportName, string listName, string auditListName)
        {
            objCommonBLL = new CommonBLL();

            switch (reportName)
            {
                case WELLBOOKREPORT:  
                case CHAPTERREPORT:
                    {
                        objCommonBLL.MarkItemToDelete(strSiteURL, listName, itemID);
                        break;
                    }
                case CHAPTERPAGEREPORT:
                    {
                        objCommonBLL.DeleteChapterPages(strSiteURL, itemID, string.Empty, listName, auditListName);
                        break;
                    }
                case TEMPLATEREPORT:
                    {
                        objCommonBLL.DeleteTemplate(strSiteURL, itemID, listName, auditListName);
                        break;
                    }
                case TEMPLATEPAGESREPORT:
                case MASTERPAGEREPORT:                    
                    {
                        objCommonBLL.DeleteListItemById(strSiteURL,itemID,listName,auditListName);
                        break;
                    }
                case USERREGISTRATION:
                    {
                        objCommonBLL.DeleteUser(strSiteURL,itemID,listName);
                        break;
                    }
                case TEAMREGISTRATION:
                    {
                        objCommonBLL.DeleteTeam(strSiteURL, itemID, listName, TEAMSTAFFLIST);
                        break;
                    }
            }

        }
        #endregion
        #endregion

        #endregion
    }

}

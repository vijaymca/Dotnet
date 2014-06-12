#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: UIHelper.cs
#endregion

using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.XPath;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;

using Telerik.Web.UI;

using System.Xml;
using Shell.SharePoint.DWB.BusinessLogicLayer;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DWB.DualListControl;
using DWBDataObjects = Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.WebParts.DWB.TreeViewControl;
using System.Web.Services.Protocols;
using System.Net;

namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// Base class for all user controls in DWB.
    /// </summary>
    public class UIHelper : System.Web.UI.UserControl
    {

        #region Declaration

        #region Variables
        MasterPageBLL objMasterBLL;
        TemplateDetailBLL objTemplateBLL;
        WellBookBLL objWellBookBLL;
        CommonBLL objCommonBLL;
        ChapterBLL objChapterBLL;
        CommonUtility objCommonUtility;
        protected string strParentSiteURL = SPContext.Current.Site.Url;
        string strUserName = string.Empty;
        List<String> strPageIdList;
        /// <summary>
        /// Added for Print updates
        /// </summary>
        string strWebServiceUserName = string.Empty;
        string strWebServicePassword = string.Empty;
        string strWebServiceDomain = string.Empty;
        PDFServiceSPProxy objPDFService;
        string strAddress = SPContext.Current.Site.WebApplication.OutboundMailServiceInstance.Server.Address;
        SPContext contextForEmails = SPContext.GetContext(HttpContext.Current);
        public string strDocumentURL = string.Empty;

        #endregion

        #region Constants
        protected const string MASTERPAGE = "MASTERPAGE";
        protected const string TEMPLATE = "TEMPLATE";
        protected const string TEMPLATEPAGEMAPPING = "TEMPLATECONFIGURATION";
        protected const string MASTERPAGETEMPLATEMAPPING = "MASTERPAGETEMPLATEMAPPING";
        protected const string WELLBOOK = "WELLBOOK";
        protected const string CHANGEPAGEOWNER = "CHANGEPAGEOWNER";
        protected const string CHAPTER = "CHAPTER";
        protected const string CHAPTERPAGEMAPPING = "CHAPTERPAGEMAPPING";
        protected const string TEMPLATEPAGESSEQUENCE = "TEMPLATEPAGES";
        protected const string TEMPLATEMASTERPAGES = "TemplateMasterPages";
        protected const string USERREGISTRATION = "UserRegistration";
        protected const string USERPRIVILEGES = "UserPrivileges";
        protected const string TEAMREGISTRATION = "TeamRegistration";
        protected const string STAFFREGISTRATION = "StaffRegistration";
        protected const string STAFFRANK = "StaffRank";
        protected const string PAGECOMMENTS = "PageComments";
        protected const string USERDEFINEDDOCUMENTLIST = "DWB UserDefined Documents";
        protected const string PUBLISHEDDOCUMENTLIST = "DWB Published Documents";
        protected const string PRINTEDDOCUMNETLIBRARY = "DWB Printed Documents";

        protected const string TEMPLATELIST = "DWB Templates";
        protected const string TEMPLATEPAGESMAPPINGLIST = "DWB Template Page Mapping";
        protected const string TEMPLATECONFIGURATIONAUDIT = "DWB Template Page Mapping Audit Trail";
        protected const string TEMPLATEAUDITLIST = "DWB Templates Audit Trail";
        protected const string ASSETTYPELIST = "DWB Asset Type";
        protected const string MASTERPAGELIST = "DWB Master Pages";
        protected const string MASTERPAGEAUDITLIST = "DWB Master Pages Audit Trail";
        protected const string DISCIPLINELIST = "DWB Discipline";
        protected const string TEAMLIST = "DWB Team";
        protected const string TEAMSTAFFLIST = "DWB Team Staff";
        protected const string USERLIST = "DWB User";
        protected const string SYSTEMPRIVILEGESLIST = "DWB System Privileges";
        protected const string PAGESCOMMENTSLIST = "DWB Comment";
        protected const string DWBBOOKLIST = "DWB Books";
        protected const string WELLBOOKAUDITLIST = "DWB Books Audit Trail";
        protected const string DWBCHAPTERLIST = "DWB Chapter";
        protected const string CHAPTERAUDITLIST = "DWB Chapter Audit Trail";
        protected const string CHAPTERPAGESMAPPINGLIST = "DWB Chapter Pages Mapping";
        protected const string CHAPTERPAGESMAPPINGAUDITLIST = "DWB Chapter Pages Mapping Audit Trail";
        protected const string DWBNARRATIVES = "DWB Narratives";
        protected const string DWBSTORYBOARD = "DWB StoryBoard";
        protected const string CONNECTIONLIST = "DWB Source";
        protected const string DREAMUSERLIST = "User Access Request";

        /// <summary>
        /// Column Names
        /// </summary>
        protected const string DWBUSERNAMECOLUMN = "DWBUserName";
        protected const string DWBUSERIDCOLUMN = "Windows_User_ID";
        protected const string DWBTITLECOLUMN = "Title";
        protected const string DWBIDCOLUMN = "ID";
        protected const string IDCOLUMNTYPE = "Counter";
        protected const string PRIVILEGEDESCRIPTIONCOLUMN = "Privilege_Description";
        protected const string PAGEOWNERCOLUMN = "Page Owner";
        protected const string TOTALCOLUMN = "Total";
        protected const string SIGNEDOFFCOLUMN = "Signed Off";
        protected const string UNSIGNEDOFFCOLUMN = "Not Signed Off";
        protected const string EMPTYCOLUMN = "Empty";
        protected const string CONNECTIONTYPECOLUMN = "Connection_Type";
        protected const string PAGENAMECOLUMN = "Page_Name";
        protected const string DISCIPLINECOLUMN = "Discipline";
        protected const string OWNERCOLUMN = "Owner";
        protected const string SIGNOFFSTATUSCOLUMN = "Sign_Off_Status";
        protected const string USERIDCOLUMN = "User_ID";
        /// <summary>
        /// Audit Action ID
        /// </summary>
        protected const string AUDITACTIONCREATION = "1";
        protected const string AUDITACTIONUPDATION = "2";
        protected const string AUDITACTIONACTIVATE = "3";
        protected const string AUDITACTIONTERMINATE = "4";
        protected const string AUDITACTIONCOMMENTADDED = "10";
        protected const string AUDITACTIONSIGNEDOFF = "5";
        protected const string AUDITACTIONUNSIGNEDOFF = "6";
        protected const string AUDITACTIONBOOKPUBLISHED = "13";
        protected const string AUDITACTIONSTORYBOARDUPDATED = "11";
        protected const string AUDITACTIONBOOKTITLEUPDATED = "12";

        /// <summary>
        /// Privileges Code
        /// </summary>
        protected const string DWBUSERPRIVILEGECODE = "US";

        /// <summary>
        /// Status
        /// </summary>
        protected const string STATUSTERMINATED = "Yes";
        protected const string STATUSACTIVE = "No";

        protected const string STATUSSIGNEDOFF = "yes";

        /// <summary>
        /// Mode
        /// </summary>
        protected const string EDIT = "edit";
        protected const string VIEW = "view";
        protected const string ADD = "add";

        /// <summary>
        /// Query String
        /// </summary>
        protected const string IDVALUEQUERYSTRING = "idValue";
        protected const string MODEQUERYSTRING = "mode";
        protected const string LISTTYPEQUERYSTRING = "listType";
        protected const string CHAPTERIDQUERYSTRING = "ChapterID";
        protected const string PAGEIDQUERYSTRING = "pageID";
        protected const string BOOKIDQUERYSTRING = "BookId";

        protected string SPLITTER_STRING = ";";
        protected char[] SPLITTER = { ';' };
        const string EQUALSOPERATOR = "EQUALS";
        const string LIKEOPERATOR = "LIKE";
        const string INOPERATOR = "IN";
        const string STAROPERATOR = "*";
        const string AMPERSANDOPERATOR = "%";

        const string USERSESSIONSYSTEMPRIVILEGES = "System Privileges";
        protected const string SESSIONWEBPARTPROPERTIES = "WEBPARTPROPERTIES";
        protected const string SESSIONTYPE3UPLOADED = "Type3Uploaded";
        protected const string SESSIONTREEVIEWDATAOBJECT = "TreeViewDataObject";
        protected const string BOOKACTIONPUBLISH = "pdf";
        protected const string BOOKACTIONPRINT = "print";
        protected const string BOOKACTION_WELLBOOKTOC = "treeview";

        protected const string WELLBOOKVIEWERCONTROLBOOK = "book";
        protected const string WELLBOOKVIEWERCONTROLCHAPTER = "chapter";
        protected const string WELLBOOKVIEWERCONTROLPAGE = "page";
        protected const string WELLBOOKVIEWERCONTROLNARRATIVE = "narrative";
        protected const string WELLBOOKVIEWERCONTROLSTORYBOARD = "storyboard";
        protected const string WELLBOOKVIEWERCONTROLCOMMENTS = "comments";
        protected const string WELLBOOKVIEWERCONTROLPAGEUPDATE = "pageupdate";

        protected const string DROPDOWNDEFAULTTEXT = "--Select--";
        protected const string DROPDOWNDEFAULTTEXTALL = "--Select All--"; //Added By Gopinath for to make constant as "Select All".
        protected const string DROPDOWNDEFAULTTEXTNONE = "--None--";
        protected const string ONCLICKEVENTNAME = "onclick";
        protected const string ONFOCUSEVENTNAME = "onfocus";
        protected const string ONBLUREEEVNTNAME = "onblur";
        protected const string ONKEYUPEVENTNAME = "onkeyup";
        protected const string STYLE = "style";
        protected const string VALUE = "value";
        protected const string TEXTALIGNCENTER = "text-align:center;";
        protected const string CANCELSIGNOFF = "Cancel Sign Off";
        protected const string SIGNOFF = "Sign Off";
        protected const string ALERTDOCUMENTNOTPRINTED = "We are not able to Print the document. Please contact the administrator.";
        protected const string ALERTNOCHAPTERNODECHECKED = "alert('Please select atleast one Chapter to Print.');";
        protected const string ALERTNOPAGENODECHECKED = "alert('Please select atleast one Page to Print.');";
        protected const string HIDETABJSKEY = "Hide Tab";
        protected const string HIDETABJSPAGE = @"<Script language='javaScript'>HideTabs('RadTabStrip1', 'none');</Script>";
        protected const string SETWINDOWTITLEJSKEY = "WINDOWTITLE";
        protected const string SETWINDOWTITLEVALUE = "setWindowTitle('{0}');";

        /// <summary>
        /// Asset Type
        /// </summary>
        protected const string ASSETTYPEFIELD = "Field";
        protected const string ASSETTYPEWELL = "Well";
        protected const string ASSETTYPEWELLBORE = "Wellbore";
        protected const string UWBI = "UWBI";
        protected const string UWI = "UWI";
        protected const string FIELDNAME = "Field Name";

        /// <summary>
        /// Redirection URLs
        /// </summary>
        protected const string USERREGISTRATIONURL = "/Pages/UserRegistration.aspx";
        protected const string STAFFLISTURL = "/Pages/StaffList.aspx";
        protected const string DWBHOMEURL = "/Pages/DWBHomePage.aspx";
        protected const string MASTERPAGEURL = "/Pages/MaintainMasterPage.aspx";
        protected const string MAINTAINCHAPTERURL = "/Pages/MaintainChapters.aspx";
        protected const string MAINTAINCHAPTERPAGESURL = "/Pages/MaintainChaptersPages.aspx";
        protected const string MAINTAINTEMPLATEPAGESURL = "/Pages/MaintainTemplatePages.aspx";
        protected const string MAINTAINBOOKPAGESURSL = "MaintainBookPages.aspx";
        protected const string TEAMREGISTRATIONURL = "/Pages/TeamRegistration.aspx";
        protected const string TERMINATESTATUSQUERYSTRING = "?TerminateStatus=true";
        protected const string EDITSTAFFPRIVILEGEURL = "/Pages/EditStaffPrivilege.aspx";

        #region DREAM 4.0 - eWB2.0-Customise Chapters
        protected const string CUSTOMISECHAPTERS = "customisechapters";
        protected const string CHAPTERPREFERENCEXML = "ewbreorderchapterxml";
        protected const string TRUE = "true";
        protected const string FALSE = "true";
        protected const string EVENTARG = "__EVENTARGUMENT";
        protected const string EVENTTARGET = "__EVENTTARGET";
        protected const string SESSION_TREEVIEWXML = "TreeViewXML";
        protected const string CHAPTERPREFERENCEDOCLIB = "eWB Customise Chapter";
        protected const string MOSSSERVICE = "MossService";
        #endregion
        #endregion

        #endregion Declaration

        #region Protected Methods
        /// <summary>
        /// Sets the list values.
        /// </summary>
        /// <param name="dropDownList">The drop down list.</param>
        /// <param name="listName">Name of the list.</param>
        protected void SetListValues(DropDownList dropDownList, string listName)
        {
            DataTable dtListData = null;
            DataRow drListData;
            ListItem lstItem;
            try
            {
                objCommonBLL = new CommonBLL();
                dtListData = objCommonBLL.ReadList(strParentSiteURL, listName, string.Empty);
                if (dtListData != null && dtListData.Rows.Count > 0)
                {
                    /// Loop through the values in Country List and finds the index of country user preference in List.
                    dropDownList.Items.Clear();
                    dropDownList.Items.Add(DROPDOWNDEFAULTTEXT);
                    for (int intRowIndex = 0; intRowIndex < dtListData.Rows.Count; intRowIndex++)
                    {
                        drListData = dtListData.Rows[intRowIndex];
                        lstItem = new ListItem();
                        lstItem.Text = drListData[DWBTITLECOLUMN].ToString();
                        lstItem.Value = drListData[DWBIDCOLUMN].ToString();
                        dropDownList.Items.Add(lstItem);

                    }
                }
            }
            catch
            { throw; }
            finally
            {
                if (dtListData != null)
                    dtListData.Dispose();
            }
        }

        /// <summary>
        /// Sets the list values.
        /// </summary>
        /// <param name="dropDownList">The drop down list.</param>
        /// <param name="listName">Name of the list.</param>
        protected void SetListValues(DropDownList dropDownList, string listName, string textField, string valueField, string selectedValue)
        {
            DataTable dtListData = null;
            DataRow drListData;
            ListItem lstItem;
            try
            {
                objCommonBLL = new CommonBLL();
                dtListData = objCommonBLL.ReadList(strParentSiteURL, listName, string.Empty);
                if (dtListData != null && dtListData.Rows.Count > 0)
                {
                    /// Loop through the values in Country List and finds the index of country user preference in List.
                    dropDownList.Items.Clear();
                    dropDownList.Items.Add(DROPDOWNDEFAULTTEXT);
                    for (int intRowIndex = 0; intRowIndex < dtListData.Rows.Count; intRowIndex++)
                    {
                        drListData = dtListData.Rows[intRowIndex];
                        lstItem = new ListItem();
                        lstItem.Text = drListData[textField].ToString();
                        lstItem.Value = drListData[valueField].ToString();
                        dropDownList.Items.Add(lstItem);
                    }
                    if (dropDownList.Items.FindByValue(selectedValue) != null)
                    {
                        dropDownList.Items.FindByValue(selectedValue).Selected = true;
                    }
                }
            }
            catch
            { throw; }
            finally
            {
                if (dtListData != null)
                    dtListData.Dispose();
            }
        }

        /// <summary>
        /// Sets the list box values.
        /// </summary>
        /// <param name="listBox">The list box.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="CAMLQuery">The CAML query.</param>
        /// <param name="viewFields">The view fields.</param>
        protected void SetListBoxValues(ListBox listBox, string listName, string CAMLQuery, string viewFields)
        {
            DataTable dtListData = null;
            DataRow drListData;
            ListItem lstItem;
            try
            {
                objCommonBLL = new CommonBLL();
                dtListData = objCommonBLL.ReadList(strParentSiteURL, listName, CAMLQuery, viewFields);
                if (dtListData != null && dtListData.Rows.Count > 0)
                {
                    /// Loop through the Team list and add items
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
            }
            catch
            { throw; }
            finally
            {
                if (dtListData != null)
                    dtListData.Dispose();
            }
        }

        /// <summary>
        /// Sets the list box values based on the CAML query
        /// </summary>
        /// <param name="lstListBox"></param>
        /// <param name="listName"></param>
        /// <param name="strCAMLQuery"></param>
        protected void SetListValues(ListBox lstListBox, string listName, string strCAMLQuery, DWBDataObjects.ListEntry objListEntry)
        {
            DataTable dtListData = null;
            DataRow drListData;
            ListItem lstItem;
            try
            {
                objCommonBLL = new CommonBLL();
                dtListData = objCommonBLL.ReadList(strParentSiteURL, listName, strCAMLQuery);
                if (dtListData != null && dtListData.Rows.Count > 0)
                {
                    lstListBox.Items.Clear();
                    for (int intRowIndex = 0; intRowIndex < dtListData.Rows.Count; intRowIndex++)
                    {
                        drListData = dtListData.Rows[intRowIndex];
                        lstItem = new ListItem();
                        lstItem.Text = Convert.ToString(drListData[DWBTITLECOLUMN]);
                        lstItem.Value = Convert.ToString(drListData[DWBIDCOLUMN]);
                        if (objListEntry != null)
                        {
                            if (objListEntry.MasterPage.Templates.Contains(lstItem.Value))
                                lstListBox.Items.Add(lstItem);
                        }
                        else
                        {
                            lstListBox.Items.Add(lstItem);
                        }
                    }
                }
            }
            catch
            { throw; }
            finally
            {
                if (dtListData != null)
                    dtListData.Dispose();
            }
        }

        /// <summary>
        /// Sets the list values.
        /// </summary>
        /// <param name="dropDownList">The drop down list.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="strCAMLQuery">The STR CAML query.</param>
        /// <param name="strViewFields">The STR view fields.</param>
        protected void SetListValues(DropDownList dropDownList, string listName, string strCAMLQuery, string strViewFields)
        {
            DataTable dtListData = null;
            DataRow drListData;
            ListItem lstItem = new ListItem();
            try
            {
                objCommonBLL = new CommonBLL();
                dtListData = objCommonBLL.ReadList(strParentSiteURL, listName, strCAMLQuery, strViewFields);
                if (dtListData != null && dtListData.Rows.Count > 0)
                {
                    /// Loop through the values in Country List and finds the index of country user preference in List.
                    dropDownList.Items.Clear();
                    dropDownList.Items.Add(DROPDOWNDEFAULTTEXT);
                    for (int intRowIndex = 0; intRowIndex < dtListData.Rows.Count; intRowIndex++)
                    {
                        drListData = dtListData.Rows[intRowIndex];
                        lstItem = new ListItem();
                        lstItem.Text = drListData[DWBTITLECOLUMN].ToString();
                        lstItem.Value = drListData[DWBIDCOLUMN].ToString();
                        dropDownList.Items.Add(lstItem);
                    }
                }
            }
            catch
            { throw; }
            finally
            {
                if (dtListData != null)
                    dtListData.Dispose();
            }
        }

        /// <summary>
        /// Sets the Well Book Owners from Asset Team Mapping list
        /// </summary>
        /// <param name="lstWellbookOwners"></param>
        /// <param name="strSourcelistName"></param>
        /// <param name="strAddedListvalues"></param>
        /// <param name="blnEdit"></param>
        /// <param name="strSeletectedID"></param>
        protected void SetWellBookOwners(ListBox lstWellbookOwners, string strSourcelistName, string
            strAddedListvalues, bool blnEdit, string strSeletectedID)
        {
            DataTable objListData = null;
            DataTable objExistingEntries = null;
            DataRow objListRow;
            string strCurrentUsername = string.Empty;
            string strQuerystring = string.Empty;
            ListItem lstItem;
            try
            {
                strCurrentUsername = GetUserName();
                objWellBookBLL = new WellBookBLL();
                strQuerystring = @"<Where>
                 <Contains>
                    <FieldRef Name='Privileges' />
                    <Value Type='Text'>WO</Value>
                 </Contains>
                 </Where>";
                objListData = objWellBookBLL.GetListDetails(strParentSiteURL, strSourcelistName,
                    strQuerystring);
                if (objListData != null && objListData.Rows.Count > 0)
                {
                    /// Loop through the values in Asset team mapping based 
                    lstWellbookOwners.Items.Clear();

                    for (int intRowIndex = 0; intRowIndex < objListData.Rows.Count; intRowIndex++)
                    {
                        objListRow = objListData.Rows[intRowIndex];
                        lstItem = new ListItem();
                        lstItem.Text = objListRow[DWBTITLECOLUMN].ToString();
                        lstItem.Value = objListRow[DWBTITLECOLUMN].ToString();
                        lstWellbookOwners.Items.Add(lstItem);

                    }

                    if (blnEdit)
                    {
                        strQuerystring = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" +
                            strSeletectedID + "</Value></Eq></Where>";

                        objExistingEntries = objWellBookBLL.GetListDetails(strParentSiteURL, strAddedListvalues,
                            strQuerystring);
                        string strexistingWellbookOnwer = string.Empty;
                        if (objExistingEntries != null && objExistingEntries.Rows.Count > 0)
                        {
                            for (int intRowIndex = 0; intRowIndex < objExistingEntries.Rows.Count; intRowIndex++)
                            {
                                objListRow = objExistingEntries.Rows[intRowIndex];
                                strexistingWellbookOnwer = objListRow["Well_Book_Owner"].ToString();
                            }
                            foreach (ListItem objListItem in lstWellbookOwners.Items)
                            {
                                if (strexistingWellbookOnwer.Contains(objListItem.Value))
                                {
                                    objListItem.Selected = true;
                                }
                            }
                        }
                    }
                }
            }
            catch
            { throw; }
            finally
            {
                if (objListData != null)
                    objListData.Dispose();
                if (objExistingEntries != null)
                    objExistingEntries.Dispose();
            }
        }

        /// <summary>
        /// Gets the items which are not included
        /// </summary>
        /// <param name="dualLstMasterPages">DualList object</param>
        /// <param name="strListName">List name from where the Right box to populated</param>
        /// <param name="strPageType">Page where the Dual list control is used (Template/User Privileges/Staff Privileges)</param>
        /// <param name="assetType">Asset Type if the Dual list box is used in Templates pages else string.Empty</param>
        protected void SetDualListLeftBox(DualList dualListBox, string strListName, string strPageType, string assetType)
        {
            DataTable objListData = null;
            DataRow objListRow;
            string strQuerystring = string.Empty;
            string strViewFields = string.Empty;
            ListItem lstItem;
            try
            {
                switch (strPageType)
                {

                    case TEMPLATEMASTERPAGES:
                        {
                            objTemplateBLL = new TemplateDetailBLL();
                            strQuerystring = @"<OrderBy><FieldRef Name='Page_Sequence' /></OrderBy><Where><And><Eq><FieldRef Name='Asset_Type' /><Value Type='Lookup'>" + assetType + "</Value></Eq><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq></And></Where>";
                            strViewFields = @"<FieldRef Name='Page_Sequence' /><FieldRef Name='ID' /><FieldRef Name='Title' /><FieldRef Name='Terminate_Status' />";

                            objListData = objTemplateBLL.GetMasterPageList(strParentSiteURL, strListName, strQuerystring, strViewFields);
                            if (objListData != null && objListData.Rows.Count > 0)
                            {
                                //Loop through the values in Country List and finds the index of country user preference in List.
                                dualListBox.LeftItems.Clear();

                                for (int intRowIndex = 0; intRowIndex < objListData.Rows.Count; intRowIndex++)
                                {

                                    objListRow = objListData.Rows[intRowIndex];
                                    lstItem = new ListItem();
                                    lstItem.Text = string.Format("{0:0000}", objListRow["Page_Sequence"]) + "-" +
                                         objListRow[DWBTITLECOLUMN].ToString();

                                    lstItem.Value = string.Format("New{0}", objListRow[DWBIDCOLUMN].ToString());
                                    dualListBox.LeftItems.Add(lstItem);
                                }
                            }
                            break;
                        }
                    case USERPRIVILEGES:
                        {
                            /// Call the Team Privileges / System Privileges List to get the details
                            objCommonBLL = new CommonBLL();
                            strQuerystring = string.Empty;
                            /// Privilege_Abbr - Title column is used
                            strViewFields = @"<FieldRef Name='Title'/><FieldRef Name='Privilege_Description'/><FieldRef Name='ID'/>";
                            objListData = objCommonBLL.ReadList(strParentSiteURL, strListName, strQuerystring, strViewFields);
                            if (objListData != null && objListData.Rows.Count > 0)
                            {
                                dualListBox.LeftItems.Clear();

                                for (int intRowIndex = 0; intRowIndex < objListData.Rows.Count; intRowIndex++)
                                {

                                    objListRow = objListData.Rows[intRowIndex];
                                    lstItem = new ListItem();
                                    lstItem.Text = objListRow[PRIVILEGEDESCRIPTIONCOLUMN].ToString();
                                    lstItem.Value = objListRow[DWBTITLECOLUMN].ToString();
                                    dualListBox.LeftItems.Add(lstItem);
                                }
                            }
                            break;
                        }
                    case STAFFREGISTRATION:
                        {
                            /// Call the DWB User List and populate with Active Users
                            objCommonBLL = new CommonBLL();
                            strQuerystring = string.Empty;
                            strViewFields = string.Empty;

                            //strQuerystring = @"<Where><Eq><FieldRef Name='Terminate_Status'/><Value Type='Choice'>" + STATUSACTIVE + "</Value></Eq></Where>";
                            strQuerystring = @"<Where><And><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>" + STATUSACTIVE + "</Value></Eq><Neq><FieldRef Name='Discipline' /><Value Type='Lookup'>Administrator</Value></Neq></And></Where><OrderBy><FieldRef Name='DWBUserName' Ascending='True'/></OrderBy>";
                            strViewFields = @"<FieldRef Name='Terminate_Status'/><FieldRef Name='ID'/><FieldRef Name='DWBUserName'/>";

                            objListData = objCommonBLL.ReadList(strParentSiteURL, strListName, strQuerystring, strViewFields);

                            if (objListData != null && objListData.Rows.Count > 0)
                            {
                                dualListBox.LeftItems.Clear();
                                for (int intRowIndex = 0; intRowIndex < objListData.Rows.Count; intRowIndex++)
                                {
                                    objListRow = objListData.Rows[intRowIndex];
                                    lstItem = new ListItem();
                                    lstItem.Text = Convert.ToString(objListRow["DWBUserName"]);
                                    lstItem.Value = Convert.ToString(objListRow[DWBIDCOLUMN]);
                                    dualListBox.LeftItems.Add(lstItem);
                                }
                            }
                            break;
                        }
                }

            }
            catch
            { throw; }
            finally
            {
                if (objListData != null)
                    objListData.Dispose();
            }
        }

        /// <summary>
        /// Gets the items already included 
        /// </summary>
        /// <param name="lbxMasterPageList">DualList object</param>
        /// <param name="strListName">List name from where the Right box to populated</param>
        /// <param name="strPageType">Page where the Dual list control is used (Template/User Privileges/Staff Privileges)</param>
        protected void SetDualListRightBox(DualList dualLstMasterPages, string strListName, string strSelectedID, string strPageType)
        {
            DataTable objListData = null;
            DataRow objListRow;
            ListItem lstItem;
            DataView objlistSortedView;
            string strCAMLQuery = string.Empty;
            string strViewFields = string.Empty;
            try
            {

                switch (strPageType)
                {
                    case TEMPLATEMASTERPAGES:
                        {
                            objTemplateBLL = new TemplateDetailBLL();
                            strCAMLQuery = @"<OrderBy><FieldRef Name='Page_Sequence' /></OrderBy><Where><Eq><FieldRef Name='Template_ID' /><Value Type='Number'>" + strSelectedID + "</Value></Eq></Where>";
                            strViewFields = @"<FieldRef Name='Page_Sequence' /><FieldRef Name='ID' /><FieldRef Name='Master_Page_Name' /><FieldRef Name='Master_Page_ID' />";
                            objListData = objTemplateBLL.GetMasterPageList(strParentSiteURL, strListName,
                                strCAMLQuery, strViewFields);
                            if (objListData != null && objListData.Rows.Count > 0)
                            {
                                objlistSortedView = objListData.DefaultView;
                                dualLstMasterPages.RightItems.Clear();

                                for (int intRowIndex = 0; intRowIndex < objListData.Rows.Count; intRowIndex++)
                                {
                                    objListRow = objListData.Rows[intRowIndex];
                                    lstItem = new ListItem();
                                    lstItem.Text = string.Format("{0:0000}",
                                        objListRow["Page_Sequence"]) + "-" +
                                        objListRow["Master_Page_Name"].ToString();
                                    lstItem.Value = objListRow[DWBIDCOLUMN].ToString();
                                    dualLstMasterPages.RightItems.Add(lstItem);
                                }
                            }
                            break;
                        }
                    case USERPRIVILEGES:
                        {
                            /// Get the Privileges string from DWB User List - strSelectedID string contains the Privileges from User List / Team Staff List
                            /// Split the String with splitter ";"
                            /// For each Privilege in User Record get Privilege from DWB System Privileges list / DWB Team Privileges List
                            string[] privileges = strSelectedID.Split(SPLITTER, StringSplitOptions.None);
                            /// Privilege_Abbr - Title column is used
                            strViewFields = @"<FieldRef Name='Title'/><FieldRef Name='Privilege_Description'/><FieldRef Name='ID'/>";
                            if (privileges != null && privileges.Length > 0)
                            {
                                string strTempCAML = string.Empty;
                                strCAMLQuery = string.Empty;

                                if (privileges.Length == 1)
                                {
                                    strCAMLQuery = @"<Eq><FieldRef Name='Title'/><Value Type='Text'>" + privileges[0] + "</Value></Eq>";
                                }
                                else
                                {
                                    for (int intIndex = privileges.Length; intIndex > 0; intIndex--)
                                    {
                                        if (intIndex == privileges.Length)
                                        {
                                            strTempCAML = string.Empty;
                                            strTempCAML = @"<Eq><FieldRef Name='Title'/><Value Type='Text'>" + privileges[intIndex - 1] + "</Value></Eq>";
                                            strCAMLQuery = strTempCAML;
                                        }

                                        else if (intIndex == privileges.Length - 1)
                                        {
                                            strTempCAML = string.Empty;
                                            strTempCAML = @"<Or>" + strCAMLQuery + "<Eq><FieldRef Name='Title'/><Value Type='Text'>" + privileges[intIndex - 1] + "</Value></Eq>" + "</Or>";
                                            strCAMLQuery = strTempCAML;
                                        }
                                        else
                                        {
                                            strTempCAML = string.Empty;
                                            strTempCAML = @"<Or>" + strCAMLQuery + "<Eq><FieldRef Name='Title'/><Value Type='Text'>" + privileges[intIndex - 1] + "</Value></Eq>" + "</Or>";
                                            strCAMLQuery = strTempCAML;
                                        }

                                    }
                                }

                                strCAMLQuery = @"<Where>" + strCAMLQuery + "</Where>";

                                objCommonBLL = new CommonBLL();
                                objListData = objCommonBLL.ReadList(strParentSiteURL, strListName, strCAMLQuery, strViewFields);
                                if (objListData != null && objListData.Rows.Count > 0)
                                {
                                    /// Loop through the values in Country List and finds the index of country user preference in List.
                                    dualLstMasterPages.RightItems.Clear();

                                    for (int intRowIndex = 0; intRowIndex < objListData.Rows.Count; intRowIndex++)
                                    {

                                        objListRow = objListData.Rows[intRowIndex];
                                        lstItem = new ListItem();
                                        lstItem.Text = objListRow[PRIVILEGEDESCRIPTIONCOLUMN].ToString();
                                        lstItem.Value = objListRow[DWBTITLECOLUMN].ToString();
                                        dualLstMasterPages.RightItems.Add(lstItem);
                                    }
                                }
                            }
                            break;
                        }

                    case STAFFREGISTRATION:
                        {

                            TeamStaffRegistrationBLL objTeamStaffRegistrationBLL = new TeamStaffRegistrationBLL();
                            objListData = objTeamStaffRegistrationBLL.GetStaffs(strParentSiteURL, strSelectedID, strListName);

                            if (objListData != null && objListData.Rows.Count > 0)
                            {
                                /// Title column is renamed as User_Name
                                dualLstMasterPages.RightItems.Clear();

                                for (int intRowIndex = 0; intRowIndex < objListData.Rows.Count; intRowIndex++)
                                {
                                    objListRow = objListData.Rows[intRowIndex];
                                    lstItem = new ListItem();
                                    lstItem.Text = objListRow[DWBTITLECOLUMN].ToString();
                                    lstItem.Value = objListRow[DWBIDCOLUMN].ToString() + SPLITTER_STRING + objListRow["User_ID"].ToString();
                                    dualLstMasterPages.RightItems.Add(lstItem);
                                }
                            }
                            break;
                        }
                }

            }
            catch
            { throw; }
            finally
            {
                if (objListData != null)
                    objListData.Dispose();
            }
        }

        /// <summary>
        /// Checks the name of the duplicate.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="listName">Name of the list.</param>
        /// <returns></returns>
        protected bool CheckDuplicateName(string value, string columnName, string listName)
        {
            objCommonBLL = new CommonBLL();
            return objCommonBLL.CheckDuplicate(strParentSiteURL, value, columnName, listName);
        }

        /// <summary>
        /// Checks the name of the duplicate for the selected ID.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="rowID">The row ID.</param>
        /// <returns></returns>
        protected bool CheckDuplicateName(string value, string columnName, string listName, string rowID)
        {
            objCommonBLL = new CommonBLL();
            return objCommonBLL.CheckDuplicate(strParentSiteURL, value, columnName, listName, rowID);
        }

        /// <summary>
        /// Checks the duplicate chapter in a book.
        /// </summary>
        /// <param name="listName">Name of the list.</param>
        /// <param name="chapterTitle">The chapter title.</param>
        /// <param name="chapterID">The chapter ID.</param>
        /// <param name="bookID">The book ID.</param>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        protected bool CheckDuplicateChapter(string listName, string chapterTitle, string chapterID, string bookID, string mode)
        {
            objCommonBLL = new CommonBLL();
            return objCommonBLL.CheckDuplicateChapter(strParentSiteURL, listName, chapterTitle, chapterID, bookID, mode);
        }

        /// <summary>
        /// Returns bool to indicate whether to show sign off button or not.
        /// </summary>
        /// <param name="controltype">The controltype.</param>
        /// <param name="bookOwner">The book owner.</param>
        /// <param name="bookTeamID">The book team ID.</param>
        /// <param name="pageOwner">The page owner.</param>
        /// <param name="pageDiscipline">The page discipline.</param>
        /// <returns>Bool</returns>        
        protected bool ShowButton(string controltype, string bookOwner, string bookTeamID, string pageOwner, string pageDiscipline)
        {
            bool blnShowButton = false;
            object ObjPrivileges = CommonUtility.GetSessionVariable(this.Page, enumSessionVariable.UserPrivileges.ToString());
            DWBDataObjects.Privileges objStoredPriviledges = null;
            strUserName = GetUserName();
            if (ObjPrivileges != null)
            {
                objStoredPriviledges = (DWBDataObjects.Privileges)ObjPrivileges;
            }
            if (objStoredPriviledges != null)
            {
                if (objStoredPriviledges.IsNonDWBUser)
                {
                    blnShowButton = false;
                }
                else if (objStoredPriviledges.SystemPrivileges != null)
                {
                    if (objStoredPriviledges.SystemPrivileges.AdminPrivilege)
                    {
                        blnShowButton = true;
                    }
                    else
                    {
                        switch (controltype)
                        {
                            case WELLBOOKVIEWERCONTROLBOOK:
                                {
                                    #region BOOK SUMMARY
                                    if (!string.IsNullOrEmpty(bookOwner) && string.Compare(bookOwner, strUserName, true) == 0)
                                    {
                                        blnShowButton = true;
                                        break;
                                    }
                                    else if (objStoredPriviledges.SystemPrivileges.BookOwner)
                                    {
                                        /// If user is member of the Team the Book belongs 
                                        /// show signoff else hide                                        
                                        if (objStoredPriviledges.FocalPoint != null && !string.IsNullOrEmpty(bookTeamID))
                                        {
                                            blnShowButton = IDExists(objStoredPriviledges.FocalPoint.TeamIDs, bookTeamID);
                                        }
                                    }
                                    else if (objStoredPriviledges.SystemPrivileges.PageOwner || objStoredPriviledges.SystemPrivileges.DWBUser)
                                    {
                                        /// Show the Signoff button if user is owner of book
                                        if (!string.IsNullOrEmpty(bookOwner) && string.Compare(bookOwner, strUserName, true) == 0)
                                        {
                                            blnShowButton = true;
                                            break;
                                        }
                                    }
                                    #endregion
                                    break;
                                }
                            case WELLBOOKVIEWERCONTROLPAGE:
                            case WELLBOOKVIEWERCONTROLSTORYBOARD:
                            case WELLBOOKVIEWERCONTROLNARRATIVE:
                                {
                                    #region BOOK PAGES
                                    if (!string.IsNullOrEmpty(pageOwner) && string.Compare(pageOwner, strUserName, true) == 0)
                                    {
                                        blnShowButton = true;
                                    }
                                    else if (!string.IsNullOrEmpty(bookOwner) && string.Compare(bookOwner, strUserName, true) == 0)
                                    {
                                        blnShowButton = true;
                                        break;
                                    }
                                    else
                                    {
                                        if (objStoredPriviledges.SystemPrivileges.BookOwner)
                                        {
                                            /// If user is member of the Team the Book belongs 
                                            /// show signoff else hide                        
                                            if (objStoredPriviledges.FocalPoint != null && !string.IsNullOrEmpty(bookTeamID))
                                            {
                                                blnShowButton = IDExists(objStoredPriviledges.FocalPoint.TeamIDs, bookTeamID);
                                            }
                                        }
                                        else if (objStoredPriviledges.SystemPrivileges.PageOwner)
                                        {
                                            /// If user is member of the Team the Book belongs 
                                            /// and Page Discipline = User Discipline show signoff else hide       
                                            bool blnTeamIDExists = false;
                                            if (objStoredPriviledges.FocalPoint != null && !string.IsNullOrEmpty(bookTeamID))
                                            {
                                                blnTeamIDExists = IDExists(objStoredPriviledges.FocalPoint.TeamIDs, bookTeamID);
                                                if (blnTeamIDExists)
                                                {
                                                    /// If Page Discipline = User Discipline show signoff else hide 
                                                    if (string.Compare(objStoredPriviledges.SystemPrivileges.Discipline, pageDiscipline, true) == 0)
                                                    {
                                                        blnShowButton = true;
                                                    }
                                                }
                                            }
                                        }
                                        else if (objStoredPriviledges.SystemPrivileges.DWBUser)
                                        {
                                            /// Show the Signoff button if user is owner of book
                                            /// Or Page Owner
                                            if (!string.IsNullOrEmpty(bookOwner) && string.Compare(bookOwner, strUserName, true) == 0)
                                            {
                                                blnShowButton = true;
                                                break;
                                            }
                                        }

                                    }
                                    #endregion
                                    break;
                                }
                            case WELLBOOKVIEWERCONTROLCOMMENTS:
                                {
                                    #region PAGE COMMENTS
                                    /// Show Update button if user is DWB User
                                    if (objStoredPriviledges.SystemPrivileges.BookOwner || objStoredPriviledges.SystemPrivileges.DWBUser || objStoredPriviledges.SystemPrivileges.PageOwner)
                                    {
                                        blnShowButton = true;
                                    }
                                    #endregion
                                    break;
                                }
                            case WELLBOOKVIEWERCONTROLPAGEUPDATE:
                                {
                                    #region UPDATE BUTTON ON PAGES
                                    /// Show Update button if user is DWB User
                                    if (objStoredPriviledges.SystemPrivileges.BookOwner)
                                    {
                                        blnShowButton = true;
                                        break;
                                    }
                                    else if (objStoredPriviledges.SystemPrivileges.DWBUser || objStoredPriviledges.SystemPrivileges.PageOwner)
                                    {
                                        bool blnTeamIDExists = false;
                                        /// If user is DWB User (US) or PO privileged, show update button if user is part 
                                        /// of team which Book is created & Page Discipline == User Discipline.
                                        if (objStoredPriviledges.FocalPoint != null && !string.IsNullOrEmpty(bookTeamID))
                                        {
                                            blnTeamIDExists = IDExists(objStoredPriviledges.FocalPoint.TeamIDs, bookTeamID);
                                            if (blnTeamIDExists && (string.Compare(objStoredPriviledges.SystemPrivileges.Discipline, pageDiscipline, true) == 0))
                                            {
                                                /// If Page Discipline = User Discipline show signoff else hide 
                                                blnShowButton = true;
                                                break;
                                            }
                                            else if (!string.IsNullOrEmpty(bookOwner) && string.Compare(bookOwner, strUserName, true) == 0)
                                            {
                                                blnShowButton = true;
                                                break;
                                            }
                                            else if (!string.IsNullOrEmpty(pageOwner) && string.Compare(pageOwner, strUserName, true) == 0)
                                            {
                                                blnShowButton = true;
                                                break;
                                            }

                                        }
                                    }
                                    #endregion
                                    break;

                                }
                            #region DREAM 4.0 - eWB2.0- Customise Chapters
                            case CUSTOMISECHAPTERS:
                                {
                                    if (objStoredPriviledges.FocalPoint != null && !string.IsNullOrEmpty(bookTeamID))
                                    {
                                        blnShowButton = IDExists(objStoredPriviledges.FocalPoint.TeamIDs, bookTeamID);
                                    }

                                    break;
                                }
                            #endregion
                            default:
                                {
                                    blnShowButton = false;
                                    break;
                                }
                        }
                    }
                }
                else
                {
                    blnShowButton = false;
                }
            }

            return blnShowButton;
        }

        /// <summary>
        /// Returns Bool indicating team id or book id exists or not
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="IdToBeFound">The id to be found.</param>
        /// <returns>bool</returns>
        protected bool IDExists(string source, string IdToBeFound)
        {
            bool blnExists = false;
            List<string> strTeamIds = new List<string>();
            if (!string.IsNullOrEmpty(source))
            {
                string[] strTempTeamIds = source.Split("|".ToCharArray());
                for (int intIndex = 0; intIndex < strTempTeamIds.Length; intIndex++)
                {
                    strTeamIds.Add(strTempTeamIds[intIndex]);
                }
            }
            if (strTeamIds.Contains(IdToBeFound))
            {
                blnExists = true;
            }
            return blnExists;
        }

        /// <summary>
        /// Updates the list entry.
        /// </summary>
        /// <param name="listEntry">The list entry.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="pageType">Type of the page.</param>
        protected bool UpdateListEntry(DWBDataObjects.ListEntry listEntry, string listName, string auditListName, string pageType, string actionPerformed)
        {
            bool blnUpdateSuccess = false;
            strUserName = GetUserName();
            switch (pageType)
            {
                case MASTERPAGE:
                    {
                        objMasterBLL = new MasterPageBLL();
                        listEntry.MasterPage.PageOwner = strUserName;
                        objMasterBLL.UpdateListEntry(strParentSiteURL, listEntry, auditListName, listName, strUserName, actionPerformed);
                        break;
                    }
                case TEMPLATE:
                    {
                        objTemplateBLL = new TemplateDetailBLL();
                        objTemplateBLL.UpdateListEntry(strParentSiteURL, listEntry, auditListName, listName, strUserName, actionPerformed);
                        break;
                    }
                case TEMPLATEPAGEMAPPING:
                    {
                        objTemplateBLL = new TemplateDetailBLL();
                        objTemplateBLL.UpdateTemplatePageMapping(strParentSiteURL, listEntry, listName, auditListName, strUserName, actionPerformed, listEntry.TemplateDetails.RowId.ToString());
                        break;
                    }
                case MASTERPAGETEMPLATEMAPPING:
                    {
                        objTemplateBLL = new TemplateDetailBLL();
                        objTemplateBLL.UpdateTemplateIDinMasterPageList(strParentSiteURL, listEntry, listName, auditListName, strUserName, actionPerformed);
                        break;
                    }
                case WELLBOOK:
                    {
                        objWellBookBLL = new WellBookBLL();
                        objWellBookBLL.UpdateListEntry(strParentSiteURL, listEntry, auditListName, listName,
                            strUserName, actionPerformed);
                        break;
                    }
                case CHAPTER:
                    {
                        objChapterBLL = new ChapterBLL();
                        objChapterBLL.UpdateListEntry(strParentSiteURL, listEntry, auditListName, listName, strUserName, actionPerformed);
                        break;
                    }
                case CHANGEPAGEOWNER:
                    {
                        objWellBookBLL = new WellBookBLL();
                        objWellBookBLL.UpdatePageOwner(strParentSiteURL, listEntry, auditListName, listName, strUserName, actionPerformed);
                        break;
                    }
                case PAGECOMMENTS:
                    {
                        objWellBookBLL = new WellBookBLL();
                        blnUpdateSuccess = objWellBookBLL.UpdatePageComments(strParentSiteURL, listName, auditListName, listEntry, actionPerformed);
                        break;
                    }
            }

            return blnUpdateSuccess;

        }

        /// <summary>
        /// Updates the list entry.
        /// </summary>
        /// <param name="listEntry">The list entry.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="pageType">Type of the page.</param>
        /// <param name="actionPerformed">The action performed.</param>
        /// <returns></returns>
        protected bool UpdateListEntry(DWBDataObjects.ListEntry listEntry, string listName, string pageType, string actionPerformed)
        {
            bool blnUpdateSuccess = false;
            strUserName = GetUserName();
            switch (pageType)
            {
                case USERREGISTRATION:
                    {
                        UserRegistrationBLL objUserRegistrationBLL = new UserRegistrationBLL();
                        blnUpdateSuccess = objUserRegistrationBLL.UpdateListEntry(strParentSiteURL, listEntry, listName, actionPerformed);
                        break;
                    }
                case USERPRIVILEGES:
                    {
                        UserRegistrationBLL objUserRegistrationBLL = new UserRegistrationBLL();
                        blnUpdateSuccess = objUserRegistrationBLL.UpdatePrivileges(strParentSiteURL, listEntry, listName, actionPerformed);
                        break;
                    }
                case TEAMREGISTRATION:
                    {
                        TeamStaffRegistrationBLL objTeamStaffRegistrationBLL = new TeamStaffRegistrationBLL();
                        blnUpdateSuccess = objTeamStaffRegistrationBLL.UpdateTeamListEntry(strParentSiteURL, listEntry, listName, actionPerformed);
                        break;
                    }
                case STAFFREGISTRATION:
                    {
                        TeamStaffRegistrationBLL objTeamStaffRegistrationBLL = new TeamStaffRegistrationBLL();
                        objTeamStaffRegistrationBLL.UpdateStaffsInTeam(strParentSiteURL, listEntry, listName);
                        blnUpdateSuccess = true;
                        break;
                    }
                case STAFFRANK:
                    {
                        TeamStaffRegistrationBLL objTeamStaffRegistrationBLL = new TeamStaffRegistrationBLL();
                        blnUpdateSuccess = objTeamStaffRegistrationBLL.UpdateStaffsRank(strParentSiteURL, listEntry, listName);
                        break;
                    }

            }

            return blnUpdateSuccess;

        }

        /// <summary>
        /// Updates the list entry.
        /// </summary>
        /// <param name="listEntry">The list entry.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="pageType">Type of the page.</param>
        protected void UpdatePageOwner(DWBDataObjects.ListEntry listEntry, string listName, string auditListName, string actionPerformed, string username)
        {
            objWellBookBLL = new WellBookBLL();
            objWellBookBLL.UpdatePageOwner(strParentSiteURL, listEntry, auditListName, listName, username, actionPerformed);
        }

        /// <summary>
        /// Updates the Audit Trail Information for a particular List
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="auditListName">Name of the audit list.</param>
        /// <param name="actionPerformed">The action performed.</param>
        protected void UpdateAuditHistory(string pageId, string auditListName, string actionPerformed)
        {
            objCommonBLL = new CommonBLL();
            strUserName = GetUserName();
            objCommonBLL.UpdateAuditTrail(strParentSiteURL, pageId, auditListName, strUserName, actionPerformed);

        }

        /// <summary>
        /// Uploads the file to SharePoint Document Library
        /// </summary>
        /// <param name="docLibName"></param>
        /// <param name="postedFile"></param>
        protected void UploadFiletoDocLibrary(string docLibName, string pageId, string postedFileName, byte[] postedFile)
        {
            objCommonBLL = new CommonBLL();
            strUserName = GetUserName();
            objCommonBLL.UploadFileToDocumentLibrary(strParentSiteURL, docLibName, pageId, strUserName, postedFileName, postedFile);
        }

        /// <summary>
        /// Gets the details for selected ID.
        /// </summary>
        /// <param name="selectedID">The selected ID.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="pageType">Type of the page.</param>
        /// <returns></returns>
        protected DWBDataObjects.ListEntry GetDetailsForSelectedID(string selectedID, string listName, string pageType)
        {
            string strQueryString;
            DWBDataObjects.ListEntry objListEntry = null;
            try
            {
                strQueryString = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + selectedID + "</Value></Eq></Where>";
                switch (pageType)
                {
                    case MASTERPAGE:
                        {
                            objMasterBLL = new MasterPageBLL();
                            objListEntry = objMasterBLL.SetMasterPageDetail(strParentSiteURL, listName,
                                strQueryString);
                            break;
                        }
                    case TEMPLATE:
                        {
                            objTemplateBLL = new TemplateDetailBLL();
                            objListEntry = objTemplateBLL.GetTemplateDetail(strParentSiteURL, listName,
                                strQueryString);
                            break;
                        }
                    case WELLBOOK:
                        {
                            objWellBookBLL = new WellBookBLL();
                            objListEntry = objWellBookBLL.GetWellBookDetail(strParentSiteURL, listName,
                                strQueryString);
                            break;
                        }
                    case CHAPTER:
                        {
                            objChapterBLL = new ChapterBLL();
                            objListEntry = objChapterBLL.SetChapterDetail(strParentSiteURL, listName, strQueryString);
                            break;
                        }

                    case USERREGISTRATION:
                        {
                            UserRegistrationBLL objUserRegistrationBLL = new UserRegistrationBLL();
                            objListEntry = objUserRegistrationBLL.GetUserDetails(strParentSiteURL, selectedID, listName);
                            break;
                        }

                    case TEAMREGISTRATION:
                    case STAFFREGISTRATION:
                        {
                            TeamStaffRegistrationBLL objTeamStaffRegistrationBLL = new TeamStaffRegistrationBLL();
                            objListEntry = objTeamStaffRegistrationBLL.GetTeamDetails(strParentSiteURL, selectedID, listName);
                            break;
                        }
                }
                return objListEntry;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the list of items based on the CAML query and the fields to view. 
        /// </summary>
        /// <param name="strListName"></param>
        /// <param name="strCamlQuery"></param>
        /// <param name="strFieldsView"></param>
        /// <returns></returns>
        protected DataTable GetListItems(string listName, string camlQuery, string fieldsView)
        {

            DataTable dtListDetails;
            try
            {

                objCommonBLL = new CommonBLL();
                dtListDetails = objCommonBLL.ReadList(strParentSiteURL, listName, camlQuery,
                    fieldsView);

            }
            catch
            { throw; }

            return dtListDetails;

        }

        /// <summary>
        /// Gets the list of items based on the CAML query and the fields to view. 
        /// </summary>
        /// <param name="strListName"></param>
        /// <param name="strCamlQuery"></param>
        /// <param name="strFieldsView"></param>
        /// <returns></returns>
        protected void UpdateListItemSequence(string strListName, DataView dvUpdatedListItem,
            string strAuditListName, string strActionPerformed, string pageType)
        {
            CommonUtility objCommonUtility; ;
            try
            {
                switch (pageType)
                {
                    case MASTERPAGE:
                    case CHAPTERPAGEMAPPING:
                        objMasterBLL = new MasterPageBLL();
                        objMasterBLL.UpdatepageSequence(strParentSiteURL, strListName, strAuditListName,
                            dvUpdatedListItem, strActionPerformed);

                        break;
                    case CHAPTER:
                        objChapterBLL = new ChapterBLL();
                        objCommonUtility = new CommonUtility();
                        objChapterBLL.UpdateChapterSequence(strParentSiteURL, strListName, strAuditListName,
                            dvUpdatedListItem, strActionPerformed, objCommonUtility.GetUserName());
                        break;
                    case TEMPLATEPAGESSEQUENCE:
                        {
                            objTemplateBLL = new TemplateDetailBLL();
                            objCommonUtility = new CommonUtility();
                            objTemplateBLL.UpdatePageSequence(strParentSiteURL, strListName, strAuditListName, strActionPerformed, objCommonUtility.GetUserName(), dvUpdatedListItem);
                            break;
                        }

                }

            }
            catch
            { throw; }
        }

        /// <summary>
        /// Binds the data to controls.
        /// </summary>
        /// <param name="cboList">The cbo list.</param>
        /// <param name="listname">The listname.</param>
        /// <param name="pageType">Type of the page.</param>
        /// <param name="strCAMLQuery">The STR CAML query.</param>
        protected void BindDataToControls(DropDownList cboList, string listname, string pageType, string strCAMLQuery)
        {

            switch (pageType)
            {
                case WELLBOOK:
                    objWellBookBLL = new WellBookBLL();
                    cboList.Items.Clear();
                    cboList.DataSource = objWellBookBLL.GetListItems(strParentSiteURL, listname, strCAMLQuery);
                    cboList.DataTextField = "value";
                    cboList.DataValueField = "key";
                    cboList.DataBind();
                    break;
                case CHAPTER:
                    objChapterBLL = new ChapterBLL();
                    cboList.Items.Clear();
                    cboList.DataSource = objChapterBLL.GetListItems(strParentSiteURL, listname, strCAMLQuery);
                    cboList.DataTextField = "value";
                    cboList.DataValueField = "key";
                    cboList.DataBind();
                    break;
                case CHANGEPAGEOWNER:
                    objWellBookBLL = new WellBookBLL();
                    cboList.Items.Clear();
                    cboList.DataSource = objWellBookBLL.GetPageOwnerList(strParentSiteURL, listname, strCAMLQuery);
                    cboList.DataTextField = "value";
                    cboList.DataValueField = "key";
                    cboList.DataBind();
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// Binds the Data to PageName Dropdown in StoryBoard Confirmation Page
        /// Added By: Praveena 
        /// Date:3/09/2010
        /// Reason: To Populate Values in PageName Dropdown for module Print By Page Type
        /// </summary>
        /// <param name="cboList"></param>
        /// <param name="strListname"></param>
        /// <param name="strCAMLQuery"></param>
        protected void BindPageNameDropDown(DropDownList cboList, string listName, string CAMLQuery)
        {
            objWellBookBLL = new WellBookBLL();
            cboList.Items.Clear();
            cboList.DataSource = objWellBookBLL.GetPageNamesList(strParentSiteURL, listName, CAMLQuery);
            cboList.DataTextField = "key";
            cboList.DataValueField = "value";
            cboList.DataBind();

        }

        /// <summary>
        /// Binds the Data to PageName Dropdown in Change Page Owner Page
        /// Added By: Praveena 
        /// Date:3/09/2010
        /// Reason: To Populate Values in PageName Dropdown for module Add addtional attribute to ChnagePageOwner
        /// </summary>
        /// <param name="cboList"></param>
        /// <param name="strListname"></param>
        /// <param name="strCAMLQuery"></param>
        protected void BindPageNames(DropDownList cboList, string listName, string CAMLQuery)
        {
            objWellBookBLL = new WellBookBLL();
            cboList.Items.Clear();
            cboList.DataSource = objWellBookBLL.GetPageNamesList(strParentSiteURL, listName, CAMLQuery);
            cboList.DataTextField = "key";
            cboList.DataBind();

        }

        /// <summary>
        /// Binds the Data to Discipline Dropdown
        /// Added By: Gopinath 
        /// Date:3/09/2010
        /// Reason: To Populate Values in Discipline Dropdown
        /// </summary>
        /// <param name="cboList">DropDownList</param>
        /// <param name="strListname">string</param>
        /// <param name="strCAMLQuery">string</param>
        protected void BindDisciplineDropDown(DropDownList cboList, string listName, string CAMLQuery)
        {
            objWellBookBLL = new WellBookBLL();
            cboList.Items.Clear();
            cboList.DataSource = objWellBookBLL.GetDisciplinesList(strParentSiteURL, listName, CAMLQuery);
            cboList.DataTextField = "value";
            cboList.DataValueField = "key";
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
        protected void BindChapterNameListBox(ListBox listBox, string listName, string CAMLQuery)
        {
            DataTable dtListData = null;
            DataRow drListData;
            ListItem lstItem;
            try
            {
                string strViewFields = string.Empty;
                strViewFields = @"<FieldRef Name='ID' /><FieldRef Name='Title' />";
                objCommonBLL = new CommonBLL();
                dtListData = objCommonBLL.ReadList(strParentSiteURL, listName, CAMLQuery, strViewFields);
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
            }
            catch
            { throw; }
            finally
            {
                if (dtListData != null)
                    dtListData.Dispose();
            }

        }


        /// <summary>
        /// Check the User Permission
        /// Added By: Gopinath 
        /// Date:3/11/2010
        /// Reason: To check the User permission such as Book Owner, Administrator and Page Owner
        /// </summary>
        /// <param name="controltype">string</param>
        /// <param name="bookOwner">string</param>
        /// <param name="bookTeamID">string</param>
        /// <param name="pageOwner">string</param>
        /// <param name="pageDiscipline">string</param>
        /// <returns>bool</returns>
        protected bool IsUserAsBookOwnerOrAdmin(string controltype, string bookOwner, string bookTeamID)
        {
            bool blnEnablePrintMyPagesOption = false;
            string strUserName = string.Empty;

            object ObjPrivileges = CommonUtility.GetSessionVariable(this.Page, enumSessionVariable.UserPrivileges.ToString());
            DWBDataObjects.Privileges objStoredPriviledges = null;
            strUserName = GetUserName();
            if (ObjPrivileges != null)
            {
                objStoredPriviledges = (DWBDataObjects.Privileges)ObjPrivileges;
            }
            if (objStoredPriviledges != null)
            {
                if (objStoredPriviledges.IsNonDWBUser)
                {
                    blnEnablePrintMyPagesOption = false;
                }
                else if (objStoredPriviledges.SystemPrivileges != null)
                {
                    if (objStoredPriviledges.SystemPrivileges.AdminPrivilege)
                    {
                        blnEnablePrintMyPagesOption = true;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(controltype) && string.Compare(controltype, WELLBOOKVIEWERCONTROLBOOK, true) == 0)
                        {
                            #region BOOK SUMMARY
                            if (!string.IsNullOrEmpty(bookOwner) && string.Compare(bookOwner, strUserName, true) == 0)
                            {
                                blnEnablePrintMyPagesOption = true;
                            }
                            else if (objStoredPriviledges.SystemPrivileges.BookOwner)
                            {
                                /// If user is member of the Team the Book belongs  
                                if (objStoredPriviledges.FocalPoint != null && !string.IsNullOrEmpty(bookTeamID))
                                {
                                    blnEnablePrintMyPagesOption = true;
                                }
                            }
                            else if (objStoredPriviledges.SystemPrivileges.PageOwner)
                            {
                                blnEnablePrintMyPagesOption = false;
                            }
                            #endregion
                        }
                    }
                }
                else
                {
                    blnEnablePrintMyPagesOption = false;
                }
            }

            return blnEnablePrintMyPagesOption;
        }

        /// <summary>
        /// <remarks>Check the Current User Permission</remarks>
        /// <remarks>Added By: Gopinath </remarks>
        /// <remarks>Date:3/11/2010</remarks>  
        /// <para>Reason: Check the Current User Permission. If Book owner is normal user or DWB User then it returns true. </para> 
        /// </summary>
        /// <param name="controltype">string</param>
        /// <param name="bookOwner">string</param>
        /// <param name="bookTeamID">string</param>
        /// <param name="pageOwner">string</param>
        /// <param name="pageDiscipline">string</param>
        /// <returns>bool</returns>
        protected bool IsUserAsDWBUserAndPageOwner(string BookId, string bookOwner)
        {
            bool blnDisablePrintMyPagesOption = false;
            string strUserName = string.Empty;

            object ObjPrivileges = CommonUtility.GetSessionVariable(this.Page, enumSessionVariable.UserPrivileges.ToString());
            DWBDataObjects.Privileges objStoredPriviledges = null;
            strUserName = GetUserName();
            if (ObjPrivileges != null)
            {
                objStoredPriviledges = (DWBDataObjects.Privileges)ObjPrivileges;
            }
            if (objStoredPriviledges != null)
            {
                if (objStoredPriviledges.IsNonDWBUser)
                {
                    blnDisablePrintMyPagesOption = true;
                }
                else if (objStoredPriviledges.SystemPrivileges != null)
                {
                    //Fix issue on 24/11/2010
                    //Issue : When normal DWB user becomes book owner Print button not visible
                    //Resolution & Resolved : Checking the book owner and current user. if both were same Print button visible.
                    if (!string.IsNullOrEmpty(bookOwner) && string.Compare(bookOwner, strUserName, true) == 0)
                    {
                        blnDisablePrintMyPagesOption = false;
                    }
                    //Block of code end for issue fix
                    else if (objStoredPriviledges.SystemPrivileges.AdminPrivilege || objStoredPriviledges.SystemPrivileges.BookOwner)
                    {
                        blnDisablePrintMyPagesOption = false;
                    }
                    else if (objStoredPriviledges.SystemPrivileges.PageOwner)
                    {
                        if (CheckPageOwnerInBookList(BookId))
                        {
                            blnDisablePrintMyPagesOption = false;
                        }
                        else
                        {
                            blnDisablePrintMyPagesOption = true;
                        }

                    }
                    else if (objStoredPriviledges.SystemPrivileges.DWBUser)
                    {
                        if (CheckPageOwnerInBookList(BookId))
                        {
                            blnDisablePrintMyPagesOption = false;
                        }
                        else
                        {
                            blnDisablePrintMyPagesOption = true;
                        }
                    }
                }
                else
                {
                    blnDisablePrintMyPagesOption = false;
                }
            }

            return blnDisablePrintMyPagesOption;
        }


        /// <summary>
        /// Determines whether [is user as DWB user] [the specified book owner].
        /// </summary>
        /// <param name="bookOwner">The book owner.</param>
        /// <returns>
        /// 	<c>true</c> if [is user as DWB user] [the specified book owner]; otherwise, <c>false</c>.
        /// </returns>
        protected bool IsUserAsDWBUser(string bookOwner)
        {
            bool blnDisablePrintMyPagesOption = false;
            string strUserName = string.Empty;

            object ObjPrivileges = CommonUtility.GetSessionVariable(this.Page, enumSessionVariable.UserPrivileges.ToString());
            DWBDataObjects.Privileges objStoredPriviledges = null;
            strUserName = GetUserName();
            if (ObjPrivileges != null)
            {
                objStoredPriviledges = (DWBDataObjects.Privileges)ObjPrivileges;
            }
            if (objStoredPriviledges != null)
            {
                if (objStoredPriviledges.IsNonDWBUser)
                {
                    blnDisablePrintMyPagesOption = true;
                }
                else if (objStoredPriviledges.SystemPrivileges != null)
                {
                    //Fix issue on 24/11/2010
                    //Issue : When normal DWB user becomes book owner Print button not visible
                    //Resolution & Resolved : Checking the book owner and current user. if both were same Print button visible.
                    if (!string.IsNullOrEmpty(bookOwner) && string.Compare(bookOwner, strUserName, true) == 0)
                    {
                        blnDisablePrintMyPagesOption = false;
                    }
                    //Block of code end for issue fix
                    else if (objStoredPriviledges.SystemPrivileges.AdminPrivilege || objStoredPriviledges.SystemPrivileges.BookOwner || objStoredPriviledges.SystemPrivileges.PageOwner)
                    {
                        blnDisablePrintMyPagesOption = false;
                    }
                    else if (objStoredPriviledges.SystemPrivileges.DWBUser)
                    {
                        blnDisablePrintMyPagesOption = true;
                    }
                }
                else
                {
                    blnDisablePrintMyPagesOption = false;
                }
            }

            return blnDisablePrintMyPagesOption;
        }

        /// <summary>
        /// Gets
        /// </summary>
        /// <param name="cboList"></param>
        /// <param name="listname"></param>
        /// <param name="strCAMLQuery"></param>
        protected void GetTeamUsers(DropDownList cboList, string listname, string strCAMLQuery)
        {
            objWellBookBLL = new WellBookBLL();
            cboList.Items.Clear();
            cboList.DataSource = objWellBookBLL.GetOwnerForTeam(strParentSiteURL, listname, strCAMLQuery);
            cboList.DataTextField = "value";
            cboList.DataValueField = "key";
            cboList.DataBind();
        }

        /// <summary>
        /// Gets the master page ID for template.
        /// </summary>
        /// <param name="templateId">The template id.</param>
        /// <param name="templatePageMappingList">The template page mapping list.</param>
        /// <returns></returns>
        protected int[] GetMasterPageIDForTemplate(int templateId, string templatePageMappingList)
        {
            int[] intMasterpageId;
            string strCAMLQuery = @"<Where><Eq><FieldRef Name='Template_ID' /><Value Type='Number'>" + templateId + "</Value></Eq></Where>";
            try
            {
                objCommonBLL = new CommonBLL();
                intMasterpageId = objCommonBLL.GetMasterPageID(strParentSiteURL, templatePageMappingList, strCAMLQuery);
            }

            catch
            {
                throw;
            }
            return intMasterpageId;
        }

        /// <summary>
        /// Updates the narrative.
        /// </summary>
        /// <param name="listName">Name of the list.</param>
        /// <param name="auditListName">Name of the audit list.</param>
        /// <param name="pageID">The page ID.</param>
        /// <param name="narrative">The narrative.</param>
        protected void UpdateNarrative(string listName, string auditListName, string pageID, string narrative)
        {
            string strCamlQuery = string.Empty;
            try
            {
                strCamlQuery = @"<Where><Eq><FieldRef Name='Page_ID' /><Value Type='Number'>" + pageID + "</Value></Eq></Where>";
                objChapterBLL = new ChapterBLL();
                strUserName = GetUserName();
                objChapterBLL.UpdateNarrative(strParentSiteURL, listName, auditListName, strCamlQuery, pageID, narrative, strUserName);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the Story Board
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="pageStoryBoard"></param>
        /// <param name="actionPerformed"></param>
        protected void UpdateStoryBoard(string listName, string auditListName, string pageId, DWBDataObjects.StoryBoard pageStoryBoard, string actionPerformed)
        {
            string strCamlQuery = string.Empty;
            try
            {
                objChapterBLL = new ChapterBLL();
                strCamlQuery = @"<Where><Eq><FieldRef Name='Page_ID' /><Value Type='Number'>" + pageId + "</Value></Eq></Where>";
                strUserName = GetUserName();
                objChapterBLL.UpdateStoryBoard(strParentSiteURL, listName, auditListName, strCamlQuery, pageId, pageStoryBoard, strUserName, actionPerformed);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <returns></returns>
        protected string GetUserName()
        {
            string strUserName = string.Empty;
            objCommonUtility = new CommonUtility();
            strUserName = objCommonUtility.GetUserName();
            return strUserName;
        }

        /// <summary>
        /// Gets the email  of the Administrator.
        /// </summary>
        /// <returns></returns>
        protected string GetAdminEmailID()
        {
            string strAdminEmail = string.Empty;
            objCommonUtility = new CommonUtility();
            strAdminEmail = objCommonUtility.GetAdminEmailID();
            return strAdminEmail;
        }

        /// <summary>
        /// Sets the quick Search data objects.
        /// </summary>
        /// <returns></returns>
        protected RequestInfo SetQuickDataObjects(string country, string columnName, string criteria)
        {
            RequestInfo objRequestInfo = new RequestInfo();
            try
            {
                objRequestInfo.Entity = SetQuickEntity(country, columnName, criteria);
            }
            catch
            { throw; }
            return objRequestInfo;
        }

        /// <summary>
        /// Gets the user priviledges from the list.
        /// </summary>
        protected DWBDataObjects.Privileges GetUserPrivileges()
        {
            AdminBLL objAdminBLL = new AdminBLL();
            DWBDataObjects.Privileges objPrivileges;

            CommonUtility objUtility = new CommonUtility();
            string strUserId = objUtility.GetUserName();
            DataTable dtSystemPrivileges = objAdminBLL.GetDWBPrivileges(strParentSiteURL, strUserId, USERSESSIONSYSTEMPRIVILEGES);

            if (dtSystemPrivileges != null)
            {
                objPrivileges = objAdminBLL.SetPrivilegesObjects(strParentSiteURL, dtSystemPrivileges);
            }
            else
            {
                objPrivileges = new DWBDataObjects.Privileges();
                objPrivileges.IsNonDWBUser = true;
            }

            if (dtSystemPrivileges != null)
            {
                dtSystemPrivileges.Dispose();
            }

            return objPrivileges;
        }

        /// <summary>
        /// Gets the summary table.
        /// </summary>
        /// <returns></returns>
        protected DataTable GetSummaryTable()
        {
            DataTable dtBookSummary = new DataTable();
            DataColumn dtCol;
            dtCol = new DataColumn("Page_Owner");

            dtBookSummary.Columns.Add(dtCol);

            dtCol = new DataColumn(TOTALCOLUMN);
            dtCol.DataType = System.Type.GetType("System.Int32");
            dtBookSummary.Columns.Add(dtCol);
            dtCol = new DataColumn("Signed_Off");
            dtCol.DataType = System.Type.GetType("System.Int32");
            dtBookSummary.Columns.Add(dtCol);
            dtCol = new DataColumn("NotSigned_Off");
            dtCol.DataType = System.Type.GetType("System.Int32");
            dtBookSummary.Columns.Add(dtCol);
            dtCol = new DataColumn(EMPTYCOLUMN);
            dtCol.DataType = System.Type.GetType("System.Int32");
            dtBookSummary.Columns.Add(dtCol);
            return dtBookSummary;
        }

        /// <summary>
        /// Gets the well book summary.
        /// </summary>
        /// <param name="BookID">The book ID.</param>
        /// <returns></returns>
        protected DataTable GetWellBookSummary(string BookID)
        {
            DataView dvResultView = null;
            DataTable dtPageOwner = null;
            int intTotal = 0;
            int intSignOff = 0;
            int intNotSignOff = 0;
            int intEmpty = 0;
            int intFooterTotal = 0;
            int intFooterSignOff = 0;
            int intFooterNotSignOff = 0;
            int intFooterEmpty = 0;
            string strValue = string.Empty;
            DataTable dtBookSummary = null;
            StringBuilder strChapterId = new StringBuilder();
            DataRow drSummaryRow;
            string strCamlQuery = string.Empty;
            string strViewFields = string.Empty;
            if (!string.IsNullOrEmpty(BookID))
            {
                strCamlQuery = @"<Where><Eq><FieldRef Name='Book_ID'/><Value Type='Counter'>" + BookID + "</Value></Eq></Where>";
                strViewFields = @"<FieldRef Name='ID'/>";
                objCommonBLL = new CommonBLL();
                DataTable dtresult = objCommonBLL.ReadList(strParentSiteURL, DWBCHAPTERLIST, strCamlQuery);
                if (dtresult != null && dtresult.Rows.Count > 0)
                {
                    intFooterEmpty = 0;
                    intFooterNotSignOff = 0;
                    intFooterSignOff = 0;
                    intFooterTotal = 0;

                    objWellBookBLL = new WellBookBLL();
                    dtresult = objWellBookBLL.GetPagesForBook(strParentSiteURL, BookID, "No");
                    if (dtresult != null && dtresult.Rows.Count > 0)
                    {
                        dvResultView = dtresult.DefaultView;
                        if (dvResultView != null && dvResultView.Count > 0)
                        {
                            dtPageOwner = dvResultView.ToTable(true, "Owner");
                        }
                    }
                    if (dtPageOwner != null && dtPageOwner.Rows.Count > 0)
                    {
                        dtBookSummary = GetSummaryTable();
                        for (int intRowIndex = 0; intRowIndex < dtPageOwner.Rows.Count; intRowIndex++)
                        {
                            intTotal = 0;
                            intSignOff = 0;
                            intNotSignOff = 0;
                            intEmpty = 0;
                            dvResultView.RowFilter = "Owner ='" + Convert.ToString(dtPageOwner.Rows[intRowIndex]["Owner"]) + "'";
                            for (int intViewIndex = 0; intViewIndex < dvResultView.Count; intViewIndex++)
                            {
                                intTotal = intTotal + 1;
                                strValue = Convert.ToString(dvResultView[intViewIndex][EMPTYCOLUMN]);
                                if (strValue.Equals("Yes"))
                                {
                                    intEmpty = intEmpty + 1;

                                }
                                strValue = Convert.ToString(dvResultView[intViewIndex]["Sign_Off_Status"]);
                                if (strValue.Equals("Yes"))
                                {
                                    intSignOff = intSignOff + 1;

                                }
                                else
                                {
                                    intNotSignOff = intNotSignOff + 1;
                                }

                            }
                            drSummaryRow = dtBookSummary.NewRow();
                            drSummaryRow["Page_Owner"] = Convert.ToString(dtPageOwner.Rows[intRowIndex]["Owner"]);
                            drSummaryRow[TOTALCOLUMN] = intTotal;
                            drSummaryRow["Signed_Off"] = intSignOff;
                            drSummaryRow["NotSigned_Off"] = intNotSignOff;
                            drSummaryRow[EMPTYCOLUMN] = intEmpty;
                            dtBookSummary.Rows.Add(drSummaryRow);
                            intFooterTotal = intFooterTotal + intTotal;
                            intFooterSignOff = intFooterSignOff + intSignOff;
                            intFooterNotSignOff = intFooterNotSignOff + intNotSignOff;
                            intFooterEmpty = intFooterEmpty + intEmpty;
                        }
                        drSummaryRow = dtBookSummary.NewRow();
                        drSummaryRow["Page_Owner"] = TOTALCOLUMN;
                        drSummaryRow[TOTALCOLUMN] = intFooterTotal;
                        drSummaryRow["Signed_Off"] = intFooterSignOff;
                        drSummaryRow["NotSigned_Off"] = intFooterNotSignOff;
                        drSummaryRow[EMPTYCOLUMN] = intFooterEmpty;
                        dtBookSummary.Rows.Add(drSummaryRow);


                    }
                }
                if (dtPageOwner != null)
                {
                    dtPageOwner.Dispose();
                }
                if (dtresult != null)
                {
                    dtresult.Dispose();
                }
            }
            return dtBookSummary;
        }

        /// <summary>
        /// Added By Gopinath
        /// Date : 27/11/2010
        /// Reason : PageOwner exists any of chapter list and pages print button should dispaly.
        /// </summary>
        /// <param name="BookID"></param>
        /// <returns></returns>
        protected bool CheckPageOwnerInBookList(string BookID)
        {
            DataView dvResultView = null;
            DataTable dtPageOwner = null;
            DataTable dtBookSummary = null;
            StringBuilder strChapterId = new StringBuilder();
            DataRow drSummaryRow;
            string strCamlQuery = string.Empty;
            string strViewFields = string.Empty;
            string strCurrentUserName = string.Empty;
            bool blnCurrentPOExists = false;

            if (!string.IsNullOrEmpty(BookID))
            {
                strCamlQuery = @"<Where><Eq><FieldRef Name='Book_ID'/><Value Type='Counter'>" + BookID + "</Value></Eq></Where>";
                strViewFields = @"<FieldRef Name='ID'/>";
                objCommonBLL = new CommonBLL();
                DataTable dtresult = objCommonBLL.ReadList(strParentSiteURL, DWBCHAPTERLIST, strCamlQuery);

                //Check for chapterIDs
                if (dtresult != null && dtresult.Rows.Count > 0)
                {
                    objWellBookBLL = new WellBookBLL();
                    dtresult = objWellBookBLL.GetPagesForBook(strParentSiteURL, BookID, "No");

                    if (dtresult != null && dtresult.Rows.Count > 0)
                    {
                        dvResultView = dtresult.DefaultView;
                        if (dvResultView != null && dvResultView.Count > 0)
                        {
                            dtPageOwner = dvResultView.ToTable(true, "Owner");
                        }
                    }
                    if (dtPageOwner != null && dtPageOwner.Rows.Count > 0)
                    {
                        strCurrentUserName = GetUserName();

                        foreach (DataRow drRow in dtPageOwner.Rows)
                        {
                            //if(drRow["Owner"].ToString()
                            if (string.Compare(drRow["Owner"].ToString(), strCurrentUserName, true)==0)
                            {
                                blnCurrentPOExists = true;
                                break;
                            }
                        }
                    }
                }
                if (dtPageOwner != null)
                {
                    dtPageOwner.Dispose();
                }
                if (dtresult != null)
                {
                    dtresult.Dispose();
                }
            }

            return blnCurrentPOExists;
        }


        /// <summary>
        /// Gets the CAML query for BO users.
        /// </summary>
        /// <param name="strSiteURL">The STR site URL.</param>
        /// <param name="columName">Name of the colum.</param>
        /// <param name="columnType">Type of the column.</param>
        /// <param name="terminated">The terminated.</param>
        /// <returns></returns>
        protected string GetCAMLQueryForBOUsers(string strSiteURL, string columName, string columnType, string terminated, bool bookMaintenance)
        {
            objCommonBLL = new CommonBLL();
            return objCommonBLL.GetCAMLQueryForBOUsers(strSiteURL, columName, columnType, terminated, bookMaintenance);
        }

        /// <summary>
        /// Prints the Book/Chapter/Page.
        /// </summary>
        /// <param name="objPrintOptions">The PrintOptions object.</param>
        /// <param name="controlType">Type of the control.</param>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        protected int Print(DWBDataObjects.PrintOptions objPrintOptions, string controlType, string mode)
        {
            bool blnUserControlFound = false;
            Telerik.Web.UI.RadTreeView trvWellBook = null;
            WellBookBLL objWellBookBLL;
            StringBuilder strPageIds;
            ArrayList arlPageDetails;
            ArrayList arlChapterCollection;
            string strSplitter = ";";
            DWBDataObjects.BookInfo objBookInfo;
            DWBDataObjects.ChapterInfo objChapterInfo;
            XmlDocument xmlWellBookDetails = null;
            string intPrintedDocID = string.Empty;
            int intNoOfPrintedPages = 0;
            bool blnIsPagePrint = false;
            bool blnIsChapterPrint = false;
            bool blnIsBookPrint = false;

            SetUserNameDetailsforWebService();
            if (string.Compare(mode, VIEW, true) != 0)
            {
                if (this.Parent.Parent.GetType().Equals(typeof(SPWebPartManager)))
                {
                    foreach (Control ctrlWebPart in this.Parent.Parent.Controls)
                    {
                        if (ctrlWebPart.GetType().Equals(typeof(TreeViewControl)))
                        {
                            Control ctrlTreeView = ctrlWebPart.FindControl("RadTreeView1");
                            if (ctrlTreeView != null && ctrlTreeView.GetType().Equals(typeof(RadTreeView)))
                            {
                                trvWellBook = (RadTreeView)ctrlWebPart.FindControl("RadTreeView1");
                                blnUserControlFound = true;
                                break;
                            }
                            if (blnUserControlFound)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            objWellBookBLL = new WellBookBLL();
            objCommonBLL = new CommonBLL();
            arlChapterCollection = new ArrayList();
            /// If selected node is book node, loop into each chapter node
            /// If chapter node is checked, loop into each page node and create book info object
            /// Else if selected node is chapter node, loop into each page node
            /// Add only selected page details to chapter object.
            /// Else if selected node is page node, create bookinfo which includes only selected page.

            //Added By Gopinath  
            //Date : 10/11/2010          
            //Description : Making Filter CAML query for Filter options which are loaded from PrintByPageType

            #region Building CAML Query using Print Options
            StringBuilder sbFilterCAMLQuery = new StringBuilder();

            //Check for Current User as Page Owner
            string strCurrentUser = string.Empty;
            object ObjPrivileges = CommonUtility.GetSessionVariable(this.Page, enumSessionVariable.UserPrivileges.ToString());
            DWBDataObjects.Privileges objStoredPriviledges = null;
            if (ObjPrivileges != null)
            {
                objStoredPriviledges = (DWBDataObjects.Privileges)ObjPrivileges;
            }
            strCurrentUser = GetUserName();

            #region CAML Query
            if (objStoredPriviledges != null && objStoredPriviledges.SystemPrivileges != null)
            {
                if (!objStoredPriviledges.SystemPrivileges.PageOwner)
                {
                    //Only for BO/AD
                    if ((!objPrintOptions.PrintMyPages && objPrintOptions.IncludeFilter) || (objPrintOptions.PrintMyPages && objPrintOptions.IncludeFilter))
                    {
                        //Page Name

                        if ((!string.Equals(objPrintOptions.PageName, "all")) && (!string.IsNullOrEmpty(objPrintOptions.PageName)))
                            sbFilterCAMLQuery.Append(@"<Eq><FieldRef Name='Page_Name' /><Value Type='Text'>" + objPrintOptions.PageName + "</Value></Eq>");
                        else
                            sbFilterCAMLQuery.Append(@"<IsNotNull><FieldRef Name='Page_Name' /></IsNotNull>");

                        if (string.Equals(objPrintOptions.PageName, "all")) //If page name selected then no need to consider discipline and page type.
                        {
                            //Discipline
                            if ((!string.Equals(objPrintOptions.Discipline, "all")) && (!string.IsNullOrEmpty(objPrintOptions.Discipline)))
                                sbFilterCAMLQuery.Append(@"<Eq><FieldRef Name='Discipline' /><Value Type='Text'>" + objPrintOptions.Discipline + "</Value></Eq>");
                            else
                                sbFilterCAMLQuery.Append(@"<IsNotNull><FieldRef Name='Discipline' /></IsNotNull>");
                        }
                        else
                        {
                            sbFilterCAMLQuery.Append(@"<IsNotNull><FieldRef Name='Discipline' /></IsNotNull>");
                        }
                        sbFilterCAMLQuery.Append("</And>");
                        sbFilterCAMLQuery.Insert(0, "<And>");

                        //Page Type
                        if (string.Equals(objPrintOptions.PageName, "all")) //If page name selected then no need to consider discipline and page type.
                        {
                            #region Page Type
                            //PageType contain values 0,1,2
                            if (!string.Equals(objPrintOptions.PageType, "none"))
                            {
                                char[] chSplitterComma = { ',' };
                                string[] strPageType = objPrintOptions.PageType.Split(chSplitterComma);
                                if (strPageType != null && strPageType.Length > 0)
                                {
                                    for (int index = 0; index < strPageType.Length; index++)
                                    {
                                        switch (strPageType[index])
                                        {
                                            case "0":
                                                {
                                                    sbFilterCAMLQuery.Append(@"<Eq><FieldRef Name='Connection_Type' /> <Value Type='Text'>1 - Automated</Value></Eq></And>");
                                                    sbFilterCAMLQuery.Insert(0, "<And>");
                                                }
                                                break;
                                            case "1":
                                                {
                                                    sbFilterCAMLQuery.Append(@"<Eq><FieldRef Name='Connection_Type' /> <Value Type='Text'>2 - Published Document</Value></Eq></And>");
                                                    sbFilterCAMLQuery.Insert(0, "<And>");
                                                }
                                                break;
                                            case "2":
                                                {
                                                    sbFilterCAMLQuery.Append(@"<Eq><FieldRef Name='Connection_Type' /> <Value Type='Text'>3 - User Defined Document</Value></Eq></And>");
                                                    sbFilterCAMLQuery.Insert(0, "<And>");
                                                }
                                                break;
                                        }
                                    }
                                }
                            }
                            #endregion Page Type
                        }

                        //Signed Off
                        if ((!string.Equals(objPrintOptions.SignedOff, "both")) && (!string.IsNullOrEmpty(objPrintOptions.SignedOff)))
                        {
                            sbFilterCAMLQuery.Append(@"<Eq><FieldRef Name='Sign_Off_Status' /> <Value Type='Choice'>" + objPrintOptions.SignedOff + "</Value></Eq> </And>");
                            sbFilterCAMLQuery.Insert(0, "<And>");
                        }

                        //Empty Pages
                        if ((!string.Equals(objPrintOptions.EmptyPages, "both")) && (!string.IsNullOrEmpty(objPrintOptions.EmptyPages)))
                        {
                            sbFilterCAMLQuery.Append(@"<Eq><FieldRef Name='Empty' /> <Value Type='Choice'>" + objPrintOptions.EmptyPages + "</Value></Eq> </And>");
                            sbFilterCAMLQuery.Insert(0, "<And>");
                        }


                        if (objPrintOptions.PrintMyPages)
                        {
                            sbFilterCAMLQuery.Append(@"<Eq><FieldRef Name='Owner' /><Value Type='Text'>" + strCurrentUser + "</Value></Eq>");
                            sbFilterCAMLQuery.Append("</And>");
                            sbFilterCAMLQuery.Insert(0, "<And>");
                        }
                        //Open & Close <Where></Where>
                        sbFilterCAMLQuery.Append(@"</Where>");
                        sbFilterCAMLQuery.Insert(0, "<Where>");
                    }
                    else if (objPrintOptions.PrintMyPages && !objPrintOptions.IncludeFilter)
                    {
                        sbFilterCAMLQuery.Append(@"<Eq><FieldRef Name='Owner' /><Value Type='Text'>" + strCurrentUser + "</Value></Eq>");
                        //Open & Close <Where></Where>
                        sbFilterCAMLQuery.Append(@"</Where>");
                        sbFilterCAMLQuery.Insert(0, "<Where>");
                    }
                }
                // For PageOwner
                else if (objPrintOptions.PrintMyPages && objPrintOptions.IncludeFilter)
                {
                    sbFilterCAMLQuery.Append(@"<Eq><FieldRef Name='Owner' /><Value Type='Text'>" + strCurrentUser + "</Value></Eq>");
                    //Open & Close <Where></Where>
                    sbFilterCAMLQuery.Append(@"</Where>");
                    sbFilterCAMLQuery.Insert(0, "<Where>");
                }
            }
            #endregion CAML Query
            //Retrive only Page Id
            string strViewFields = "<FieldRef Name='ID' />";

            #endregion Building CAML Query using Print Options
            if (sbFilterCAMLQuery != null && sbFilterCAMLQuery.Length > 0)
            {
                strPageIdList = new List<string>(); //Declared Globally
                DataTable dtPageIds = objCommonBLL.ReadList(strParentSiteURL, CHAPTERPAGESMAPPINGLIST, sbFilterCAMLQuery.ToString(), strViewFields);
                if (dtPageIds != null && dtPageIds.Rows.Count > 0)
                {
                    foreach (DataRow drPageId in dtPageIds.Rows)
                    {
                        strPageIdList.Add(drPageId[0].ToString());
                    }
                }
            }

            ///End Gopinath code.

            switch (controlType)
            {
                case WELLBOOKVIEWERCONTROLBOOK:
                    {
                        #region BOOK PRINT
                        blnIsBookPrint = true;
                        if (trvWellBook != null)
                        {
                            if (trvWellBook.SelectedNode == null)
                            {
                                trvWellBook.Nodes[0].Selected = true;
                            }
                            if (trvWellBook.SelectedNode.Level == 0)
                            {
                                objBookInfo = objWellBookBLL.SetBookDetailDataObject(strParentSiteURL, trvWellBook.SelectedNode.Value, BOOKACTIONPRINT, false, objPrintOptions);
                                if (trvWellBook.CheckedNodes.Count > 0)
                                {
                                    foreach (RadTreeNode chapterNode in trvWellBook.SelectedNode.Nodes)
                                    {
                                        if (chapterNode.Checked)
                                        {
                                            objChapterInfo = CreateChapterInfo(chapterNode);
                                            if (objChapterInfo != null)
                                                arlChapterCollection.Add(objChapterInfo);
                                            if (objChapterInfo != null && objChapterInfo.PageInfo != null)
                                            {
                                                intNoOfPrintedPages += objChapterInfo.PageInfo.Count;
                                            }
                                        }
                                    }
                                    objBookInfo.Chapters = arlChapterCollection;
                                    objBookInfo.PageCount = intNoOfPrintedPages;
                                    if (objBookInfo.Chapters.Count == 0)
                                        objBookInfo = null;
                                    xmlWellBookDetails = objCommonBLL.CreateWellBookDetailXML(objBookInfo);
                                }
                                else
                                {
                                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "documentnotprinted", ALERTNOCHAPTERNODECHECKED, true);
                                }

                            }
                        }
                        #endregion BOOK PRINT
                        break;
                    }
                case WELLBOOKVIEWERCONTROLCHAPTER:
                    {
                        #region CHAPTER PRINT
                        blnIsChapterPrint = true;
                        if (trvWellBook != null)
                        {
                            objBookInfo = objWellBookBLL.SetBookDetailDataObject(strParentSiteURL, trvWellBook.SelectedNode.ParentNode.Value, BOOKACTIONPRINT, false, objPrintOptions);
                            if (trvWellBook.SelectedNode.Level == 1)
                            {
                                if (trvWellBook.SelectedNode.Checked)
                                {
                                    objChapterInfo = CreateChapterInfo(trvWellBook.SelectedNode);
                                    if (objChapterInfo != null)
                                        arlChapterCollection.Add(objChapterInfo);

                                    objBookInfo.Chapters = arlChapterCollection;
                                    if (objBookInfo.Chapters.Count == 0)
                                        objBookInfo = null;
                                    xmlWellBookDetails = objCommonBLL.CreateWellBookDetailXML(objBookInfo);
                                }
                                else
                                {
                                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "documentnotprinted", ALERTNOPAGENODECHECKED, true);
                                }
                            }
                        }
                        #endregion CHAPTER PRINT
                        break;
                    }
                case WELLBOOKVIEWERCONTROLPAGE:
                    {
                        #region PAGE PRINT
                        blnIsPagePrint = true;
                        string strPageID = string.Empty;
                        string strChapterID = string.Empty;
                        string strBookID = string.Empty;
                        if (trvWellBook != null)
                        {
                            if (trvWellBook.SelectedNode.Level == 2)
                            {
                                strBookID = trvWellBook.SelectedNode.ParentNode.ParentNode.Value;
                                strChapterID = trvWellBook.SelectedNode.ParentNode.Value;
                                strPageID = trvWellBook.SelectedNode.Value;
                            }
                        }
                        else if (string.Compare(mode, VIEW, true) == 0)
                        {
                            strPageID = HttpContext.Current.Request.QueryString[PAGEIDQUERYSTRING];
                            strChapterID = HttpContext.Current.Request.QueryString[CHAPTERIDQUERYSTRING];
                            DWBDataObjects.BookInfo objBookInfoSession = ((DWBDataObjects.BookInfo)HttpContext.Current.Session[SESSIONTREEVIEWDATAOBJECT]);
                            if (objBookInfoSession != null)
                            {
                                strBookID = objBookInfoSession.BookID;
                            }
                        }
                        objBookInfo = objWellBookBLL.SetBookDetailDataObject(strParentSiteURL, strBookID, BOOKACTIONPRINT, false, objPrintOptions);
                        objChapterInfo = objWellBookBLL.SetChapterDetails(strParentSiteURL, strChapterID, false);
                        if (objChapterInfo != null)
                        {
                            strPageIds = new StringBuilder();
                            strPageIds.Append(strPageID);
                            strPageIds.Append(strSplitter);
                            arlPageDetails = objWellBookBLL.SetSelectedPageInfo(strParentSiteURL, strPageIds.ToString(), objChapterInfo.ActualAssetValue, objChapterInfo.ColumnName);
                            if (arlPageDetails != null && arlPageDetails.Count > 0)
                            {
                                objChapterInfo.PageInfo = arlPageDetails;
                            }
                            arlChapterCollection.Add(objChapterInfo);
                        }
                        if (arlChapterCollection != null && arlChapterCollection.Count > 0)
                        {
                            objBookInfo.Chapters = arlChapterCollection;
                        }
                        xmlWellBookDetails = objCommonBLL.CreateWellBookDetailXML(objBookInfo);
                        #endregion PAGE PRINT
                        break;
                    }
                default:
                    break;
            }
            if (xmlWellBookDetails == null)
                {
                    return -1;
                }
            string strSiteURL = strParentSiteURL;
            SslRequiredWebPart objSslRequired = new SslRequiredWebPart();

            strSiteURL = objSslRequired.GetSslURL(strSiteURL);

            PDFServiceSPProxy objPDFService;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                objPDFService = new PDFServiceSPProxy();
                objPDFService.PreAuthenticate = true;
                objPDFService.Credentials = new NetworkCredential(strWebServiceUserName, strWebServicePassword, strWebServiceDomain);
                PdfBLL objPdfBLL = new PdfBLL();

                if (xmlWellBookDetails != null)
                {
                    try
                    {
                        string strBookName = xmlWellBookDetails.DocumentElement.Attributes["BookName"].Value.ToString();
                        string strRequestID = Guid.NewGuid().ToString();
                        if (blnIsPagePrint)
                        {
                            string strDocumentURLTemp = PortalConfiguration.GetInstance().GetKey("DWBPrintNetworkPath") + string.Format("{0}_{1}", strBookName, strRequestID) + ".pdf";
                            UpdatePagePrintDetails(strRequestID, strDocumentURLTemp, strSiteURL, "temp");
                            intPrintedDocID = objPDFService.GeneratePDFDocument(xmlWellBookDetails.DocumentElement, strParentSiteURL, strRequestID);
                            strDocumentURL = strSiteURL + "/Pages/eWBPDFViewer.aspx?mode=page&requestID=" + strRequestID;
                        }
                        else if (blnIsChapterPrint)
                        {
                            UpdateDWBChapterPrintDetails(strCurrentUser, strRequestID, strBookName);
                            //strCurrentUser, strRequestID, e-MailID, document URL
                            //intPrintedDocID = objPDFService.GeneratePDFDocument(xmlWellBookDetails.DocumentElement, strParentSiteURL, strRequestID);
                            AsyncCallback asyncCall = new AsyncCallback(CallbackMethod);
                            objPDFService.Timeout = System.Threading.Timeout.Infinite;
                            objPDFService.BeginGeneratePDFDocument(xmlWellBookDetails.DocumentElement, strParentSiteURL, strRequestID, asyncCall, objPDFService);
                        }
                        else if (blnIsBookPrint)
                        {
                            UpdateDWBBookPrintDetails(strCurrentUser, strRequestID, strBookName, xmlWellBookDetails);                            
                        }
                    }
                    catch (SoapException)
                    {
                        throw;
                    }
                }
            });
            return 1;
        }

 /// <summary>
        /// Updates the page print details.
        /// </summary>
        /// <param name="requestID">The request ID.</param>
        /// <param name="documentURLTemp">The document URL temp.</param>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="username">The username.</param>
        private void UpdatePagePrintDetails(string requestID, string documentURLTemp, string siteURL, string username)
        {
            objCommonBLL = new CommonBLL();
            objCommonBLL.UpdateChapterPrintDetails(requestID, documentURLTemp, siteURL, username, "DWB Page Print Details");
        }

        /// <summary>
        /// Updates the DWB book print details.
        /// </summary>
        /// <param name="currentUser">The current user.</param>
        /// <param name="requestID">The request ID.</param>
        /// <param name="bookName">Name of the book.</param>
        /// <param name="xmlDoc">The XML doc.</param>
        private void UpdateDWBBookPrintDetails(string currentUser, string requestID, string bookName, XmlDocument xmlDoc)
        {
            strDocumentURL = PortalConfiguration.GetInstance().GetKey("DWBPrintNetworkPath") + string.Format("{0}_{1}", bookName, requestID) + ".pdf";
            //Call BLL method to update the list named "DWB Chapter Print Details" with the above details.
            WellBookBLL objBookBLL = new WellBookBLL();
            objBookBLL.UpdateBookPrintDetails(requestID, strDocumentURL, strParentSiteURL, currentUser, false, string.Empty, xmlDoc);
        }

        /// <summary>
        /// Callbacks the method.
        /// </summary>
        /// <param name="asyncResult">The async result.</param>
        private void CallbackMethod(IAsyncResult asyncResult)
        {
            WellBookBLL objBookBLL = null;
            try
            {
                string strResults = string.Empty;
                // Create an instance of the WebService
                objPDFService = (PDFServiceSPProxy)asyncResult.AsyncState;
                strResults = objPDFService.EndGeneratePDFDocument(asyncResult);
                objBookBLL = new WellBookBLL();
                //strDocumentURL = objBookBLL.GetPrintDocumentURL(strResults, strParentSiteURL, strAddress, contextForEmails, "DWB Chapter Print Details");
                objBookBLL.SendEmailToUserOnPrint(strResults, strParentSiteURL, strAddress, contextForEmails);
            }
            catch (SoapException)
            {
                throw;
            }
        }

        /// <summary>
        /// Sets the user name detailsfor web service.
        /// </summary>
        private void SetUserNameDetailsforWebService()
        {
            strWebServiceUserName = PortalConfiguration.GetInstance().GetKey("DWBWebServiceUserName");
            strWebServicePassword = PortalConfiguration.GetInstance().GetKey("DWBWebServicePassword");
            strWebServiceDomain = PortalConfiguration.GetInstance().GetKey("DWBWebServiceDomain");
        }

        private void UpdateDWBChapterPrintDetails(string currentUser, string requestID, string bookName)
        {
            strDocumentURL = String.Empty;
            strDocumentURL = PortalConfiguration.GetInstance().GetKey("DWBPrintNetworkPath") + string.Format("{0}_{1}", bookName, requestID) + ".pdf";
            //Call BLL method to update the list named "DWB Chapter Print Details" with the above details.
            WellBookBLL objBookBLL = new WellBookBLL();
            objBookBLL.UpdateChapterPrintDetails(requestID, strDocumentURL, strParentSiteURL, currentUser);
        }
        /// <summary>
        /// Terminates the live book.
        /// </summary>
        /// <param name="bookID">The book ID.</param>
        /// <param name="listName">Name of the list.</param>
        protected void TerminateLiveBook(int bookID, string listName)
        {
            objCommonBLL = new CommonBLL();
            objCommonBLL.TerminateBook(strParentSiteURL, bookID, listName);
        }


        #region DREAM 4.0 - eWB2.0 - Customise Chapters

        /// <summary>
        /// Loads the chapter RAD list box.
        /// </summary>
        /// <param name="bookID">The book ID.</param>
        /// <param name="radListBox">The RAD list box.</param>
        protected void LoadChapterRadListBox(string bookID, RadListBox radListBox)
        {
            if (!string.IsNullOrEmpty(bookID) && radListBox != null)
            {
                if (HttpContext.Current.Session[CHAPTERPREFERENCEXML] != null)
                {
                    /// Load the XML to XmlDocument or XmlNodeList
                    /// Load the chapterpreference session value to string object
                    /// then load the string to XmlDocument object and populate the RadListBox
                    string strChapterXml = (string)HttpContext.Current.Session[CHAPTERPREFERENCEXML];
                    XmlDocument xmlChapterPreference = new XmlDocument();
                    xmlChapterPreference.LoadXml(strChapterXml);
                    XmlNodeList xmlChapterNodes = xmlChapterPreference.SelectNodes("/BookInfo/Chapter");
                    RadListBoxItem objListItem = new RadListBoxItem();
                    foreach (XmlNode xmlNodeChapter in xmlChapterNodes)
                    {
                        objListItem = new RadListBoxItem();
                        objListItem.Text = xmlNodeChapter.Attributes["ChapterTitle"].Value;
                        objListItem.Value = xmlNodeChapter.Attributes["ChapterID"].Value;

                        if (xmlNodeChapter.Attributes["Display"].Value.ToLowerInvariant().Equals(TRUE))
                        {
                            objListItem.Checked = true;
                        }
                        radListBox.Items.Add(objListItem);
                    }
                }
            }
        }

        /// <summary>
        /// Saves the customise chapter preference to session object.
        /// Add the ChapterOrder attribute to SESSION_TREEVIEWXML object.
        /// </summary>
        /// <param name="bookID">The book ID.</param>
        /// <param name="bookName">Name of the book.</param>
        /// <param name="radListBox">The RAD list box.</param>
        protected void SaveCustomiseChapterPreferenceToSession(string bookID, string bookName, RadListBox radListBox)
        {
            XmlDocument chapterListXml = new XmlDocument();
            CommonBLL objCommonBLL = new CommonBLL();

            DWBDataObjects.ChapterInfo objChapterInfo;
            DWBDataObjects.BookInfo objBookInfo = new DWBDataObjects.BookInfo();
            objBookInfo.BookName = bookName;
            objBookInfo.BookID = bookID;
            ArrayList arlChapters = new ArrayList();
            foreach (RadListBoxItem listItem in radListBox.Items)
            {
                objChapterInfo = new DWBDataObjects.ChapterInfo();
                objChapterInfo.ChapterTitle = listItem.Text;
                objChapterInfo.ChapterID = listItem.Value;
                if (listItem.Checked)
                {
                    objChapterInfo.Display = true;
                }
                arlChapters.Add(objChapterInfo);
            }

            objBookInfo.Chapters = arlChapters;

            chapterListXml = objCommonBLL.CreateWellBookDetailXML(objBookInfo);
            if (HttpContext.Current.Session[CHAPTERPREFERENCEXML] != null)
            {
                HttpContext.Current.Session.Remove(CHAPTERPREFERENCEXML);
            }
            /// Chapter preference XML created and stored to session.
            HttpContext.Current.Session[CHAPTERPREFERENCEXML] = chapterListXml.OuterXml;

            ///Adding ChapterOrder attribute to SESSION_TREEVIEWXML object.
            XmlDocument xmlDocWellBookDetails = new XmlDocument();
            xmlDocWellBookDetails.LoadXml((string)HttpContext.Current.Session[SESSION_TREEVIEWXML]);

            ReOrderChapterNode(xmlDocWellBookDetails, chapterListXml);
        }

        /// This method can be removed.

        /// <summary>
        /// Saves the customise chapter preference to session.
        /// Reorderxml is received from HiddenField value. 
        /// </summary>
        /// <param name="bookID">The book ID.</param>
        /// <param name="bookName">Name of the book.</param>
        /// <param name="reorderXml">The reorder XML.</param>
        protected void SaveCustomiseChapterPreferenceToSession(string bookID, string bookName, string reorderXml)
        {
            XmlDocument chapterListXml = new XmlDocument();
            CommonBLL objCommonBLL = new CommonBLL();

            DWBDataObjects.ChapterInfo objChapterInfo;
            DWBDataObjects.BookInfo objBookInfo = new DWBDataObjects.BookInfo();
            objBookInfo.BookName = bookName;
            objBookInfo.BookID = bookID;
            ArrayList arlChapters = new ArrayList();
            //foreach (RadListBoxItem listItem in radListBox.Items)
            //{
            //    objChapterInfo = new DWBDataObjects.ChapterInfo();
            //    objChapterInfo.ChapterTitle = listItem.Text;
            //    objChapterInfo.ChapterID = listItem.Value;
            //    if (listItem.Checked)
            //    {
            //        objChapterInfo.Display = true;
            //    }
            //    arlChapters.Add(objChapterInfo);
            //}

            //objBookInfo.Chapters = arlChapters;

            chapterListXml = objCommonBLL.CreateWellBookDetailXML(objBookInfo);
            if (chapterListXml != null && !string.IsNullOrEmpty(reorderXml))
            {
                reorderXml = reorderXml.Insert(0, chapterListXml.OuterXml.Substring(0, chapterListXml.OuterXml.LastIndexOf("/>")) + ">");
                reorderXml = reorderXml.Insert(reorderXml.Length - 1, "</BookInfo>");
                
            }
            if (HttpContext.Current.Session[CHAPTERPREFERENCEXML] != null)
            {
                HttpContext.Current.Session.Remove(CHAPTERPREFERENCEXML);
            }

            HttpContext.Current.Session[CHAPTERPREFERENCEXML] = reorderXml;
        }
        
        /// <summary>
        /// Saves the reorder XML to document library.
        /// </summary>
        /// <param name="chapterPreference">The chapter preference.</param>
        /// <param name="bookId">The book id.</param>
        /// <returns></returns>
        protected XmlDocument SaveReorderXml(string chapterPreference, string bookId)
        {
            XmlDocument userReorderXml = null;

            ChapterBLL objChapterBLL = new ChapterBLL();
            userReorderXml = objChapterBLL.SaveReorderXml(chapterPreference, bookId);
            return userReorderXml;
        }

        /// <summary>
        /// Reorder chapter node.
        /// Add the ChapterOrder attribute to SESSION_TREEVIEWXML object.
        /// </summary>
        /// <param name="wellBookDetailsXml">The well book details XML.</param>
        /// <param name="chapterPreferenceXml">The chapter preference XML.</param>
        /// <returns></returns>
        private XmlDocument ReOrderChapterNode(XmlDocument wellBookDetailsXml, XmlDocument chapterPreferenceXml)
        {
            XmlDocument xmlWellBookDetailsCopy = new XmlDocument();
            
            if (chapterPreferenceXml != null && wellBookDetailsXml != null)
            {
                xmlWellBookDetailsCopy.LoadXml(wellBookDetailsXml.OuterXml);
                int intNodeIndex = 1;
                XmlNodeList chapterNodesXml = chapterPreferenceXml.SelectNodes("/BookInfo/Chapter");
                /// For each Chapter node in chapter preference xml add the ChapterOrder attribute in wellBookDetailsXml
                foreach (XmlNode chapterNode in chapterNodesXml)
                {
                    XmlAttribute orderAttribute = wellBookDetailsXml.CreateAttribute("ChapterOrder");
                    orderAttribute.Value = intNodeIndex.ToString();
                    wellBookDetailsXml.SelectSingleNode("/BookInfo/Chapter[@ChapterID='" + chapterNode.Attributes["ChapterID"].Value + "']").Attributes.Append(orderAttribute);

                    intNodeIndex++;
                }
            }                     
            return wellBookDetailsXml;
        }

        /// <summary>
        /// Ensures the panel fix.
        /// </summary>
        /// <param name="type">The type.</param>
        protected void EnsurePanelFix(Type type, string scriptKey)
        {
            if (this.Page.Form != null)
            {
                string formOnSubmitAtt = this.Page.Form.Attributes["onsubmit"];
                if (formOnSubmitAtt == "return _spFormOnSubmitWrapper();")
                {
                    this.Page.Form.Attributes["onsubmit"] = "_spFormOnSubmitWrapper();";
                }
            }
            ScriptManager.RegisterStartupScript(this, type, scriptKey, "_spOriginalFormAction = document.forms[0].action; _spSuppressFormOnSubmitWrapper=true;", true);
        }

        #endregion

        #endregion

        #region Private Methods
        /// <summary>
        /// This function will set the Entity object.
        /// </summary>
        /// <returns></returns>
        private Entity SetQuickEntity(string country, string columnName, string criteria)
        {
            Entity objEntity;
            /// initializes the EntityList object.            
            try
            {
                objEntity = new Entity();
                objEntity.ResponseType = "Tabular";
                objEntity.Property = true;
                Criteria objCriteria = new Criteria();
                ArrayList arlAttribute = new ArrayList();
                arlAttribute = SetQuickAttribute(country);
                if (arlAttribute.Count > 1)
                {
                    objEntity.AttributeGroups = SetQuickAttributeGroup(arlAttribute);
                }
                else
                {
                    objEntity.Attribute = arlAttribute;
                }
                objCriteria = SetQuickCriteria(columnName, criteria);
                objEntity.Criteria = objCriteria;
            }
            catch
            { throw; }
            return objEntity;
        }

        /// <summary>
        /// Sets the quick attribute group.
        /// </summary>
        /// <returns></returns>
        private ArrayList SetQuickAttributeGroup(ArrayList quickAttribute)
        {
            ArrayList arlQuickAttributeGroup = new ArrayList();
            try
            {
                /// initializes the attribute group object.
                AttributeGroup objQuickAttributeGroup = new AttributeGroup();
                objQuickAttributeGroup.Operator = GetLogicalOperator();
                objQuickAttributeGroup.Attribute = quickAttribute;
                arlQuickAttributeGroup.Add(objQuickAttributeGroup);
            }
            catch
            { throw; }
            return arlQuickAttributeGroup;
        }

        /// <summary>
        /// Gets the logical operator.
        /// </summary>
        /// <returns></returns>
        private string GetLogicalOperator()
        {
            string strOperator;
            strOperator = "AND";
            /// returns the logical operator
            return strOperator;
        }

        /// <summary>
        /// Sets the string field as a Value object.
        /// </summary>
        /// <param name="strField">field.</param>
        /// <returns></returns>
        private Value SetValue(string field)
        {
            Value objValue = new Value();
            objValue.InnerText = field;
            /// returns the value object.
            return objValue;
        }

        /// <summary>
        /// Gets the query operator for request xml
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private string GetOperator(ArrayList value)
        {
            string strOperator = string.Empty;
            try
            {
                /// the below condition validates the value parameter.
                if (value.Count > 1)
                {
                    strOperator = INOPERATOR;
                }
                else
                {
                    /// Loop through the values in ArrayList.
                    foreach (Value objValue in value)
                    {
                        /// the below condition check for the innerText value.
                        if ((objValue.InnerText.Contains(STAROPERATOR)) || (objValue.InnerText.Contains(AMPERSANDOPERATOR)))
                            strOperator = LIKEOPERATOR;
                        else
                            strOperator = EQUALSOPERATOR;
                    }
                }
            }
            catch
            { throw; }
            return strOperator;
        }

        /// <summary>
        /// Sets the attribute node for request xml.
        /// </summary>
        /// <returns></returns>
        private ArrayList SetQuickAttribute(string country)
        {
            ArrayList arlAttribute = new ArrayList();
            try
            {
                ArrayList arlValue = new ArrayList();
                Attributes objCountry = new Attributes();
                /// the below condition check the selected option for country.
                if (string.Equals(country.ToUpper().Trim(), "--ANY COUNTRY--"))
                {
                    objCountry.Name = "country";
                    arlValue = ReadActiveCountriesList();   //for 'ANYCOUNTRY' read all active countries from SPList
                    objCountry.Value = arlValue;
                    objCountry.Operator = GetOperator(objCountry.Value);
                }
                else
                {
                    objCountry.Name = "country";
                    arlValue.Add(SetValue(country));
                    objCountry.Value = arlValue;
                    objCountry.Operator = GetOperator(objCountry.Value);
                }
                arlAttribute.Add(objCountry);
            }
            catch
            { throw; }
            return arlAttribute;
        }

        /// <summary>
        /// Read active countries from 'Country' SPList and add to arraylist
        /// </summary>
        private ArrayList ReadActiveCountriesList()
        {
            ArrayList arlActiveCountries = new ArrayList();
            DataTable objDtCountryList = new DataTable();
            try
            {
                objCommonBLL = new CommonBLL();
                string strCamlQuery = "<OrderBy><FieldRef Name=\"Title\"/></OrderBy><Where><Eq>" + "<FieldRef Name=\"Active\" /><Value Type=\"Choice\">Yes</Value></Eq></Where>";
                /// this will read values from Country Sharepoint list.
                objDtCountryList = objCommonBLL.ReadList(strParentSiteURL, "Country", strCamlQuery);
                if (objDtCountryList != null && objDtCountryList.Rows.Count > 0)
                {
                    /// Loop through the values in country list.
                    foreach (DataRow dtRow in objDtCountryList.Rows)
                    {
                        arlActiveCountries.Add(SetValue(dtRow["Country_x0020_Code"].ToString()));
                    }
                }

            }
            catch
            {
                throw;
            }
            finally
            {
                if (objDtCountryList != null) objDtCountryList.Dispose();
            }
            return arlActiveCountries;
        }

        /// <summary>
        /// Gets the query operator for request xml
        /// </summary>
        /// <param name="strValue">value.</param>
        /// <returns></returns>
        private string GetOperator(string value)
        {
            string strOperator;
            try
            {
                /// the below condition check for the innerText value.
                if ((value.Contains(STAROPERATOR)) || (value.Contains(AMPERSANDOPERATOR)))
                    strOperator = LIKEOPERATOR;
                else
                    strOperator = EQUALSOPERATOR;
            }
            catch
            { throw; }
            return strOperator;
        }

        /// <summary>
        /// This function will set search criteria to Criteria object.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        private Criteria SetQuickCriteria(string columnName, string criteria)
        {
            Criteria objCriteria = new Criteria();
            if (columnName.Length > 0)
                objCriteria.Name = columnName;
            objCriteria.Value = criteria;
            objCriteria.DisplayField = "true";
            objCriteria.Operator = GetOperator(objCriteria.Value);
            /// returns the search criteria object.
            return objCriteria;
        }

        /// <summary>
        /// Creates the chapter info.
        /// </summary>
        /// <param name="chapterNode">The chapter node.</param>
        /// <returns></returns>
        private DWBDataObjects.ChapterInfo CreateChapterInfo(RadTreeNode chapterNode)
        {
            string strSplitter = ";";
            StringBuilder strPageIds = new StringBuilder();
            ArrayList arlPageDetails = new ArrayList();
            //bool blnAddChapter = false;
            DWBDataObjects.ChapterInfo objChapterInfo = null;
            WellBookBLL objWellBookBLL = new WellBookBLL();
            DataTable dtPageIds = null;

            /// Loop for Pages node and print only selected Pages.
            if (chapterNode != null)
            {
                if (chapterNode.Nodes != null && chapterNode.Nodes.Count > 0)
                {
                    foreach (RadTreeNode pageNode in chapterNode.Nodes)
                    {
                        if (pageNode.Checked)
                        {
                            strPageIds.Append(pageNode.Value);
                            strPageIds.Append(strSplitter);
                        }
                    }

                    //Get Pages in each Chapter
                    strPageIds = GetPagesInEachChapter(strPageIds.ToString());

                }
                else
                {
                    string strViewFields = "<FieldRef Name='ID' />";
                    #region Fix to avoid printing the terminated pages when chapter is not expanded
                    /// CAML Query is updated to add "<Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq>" condition
                    string strCamlQuery = @"<Where><And><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq><Eq><FieldRef Name='Chapter_ID' /><Value Type='Counter'>" + chapterNode.Value + "</Value></Eq></And></Where>";
                    #endregion
                    dtPageIds = objCommonBLL.ReadList(strParentSiteURL, CHAPTERPAGESMAPPINGLIST, strCamlQuery, strViewFields);
                    if (dtPageIds != null && dtPageIds.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtPageIds.Rows)
                        {
                            strPageIds.Append(row[0].ToString() + strSplitter);
                        }
                    }
                    //Get Pages in each Chapter
                    strPageIds = GetPagesInEachChapter(strPageIds.ToString());

                    /// Get all the pages for the chapter and print
                    //objChapterInfo = objWellBookBLL.SetChapterDetails(strParentSiteURL, chapterNode.Value, true);
                }
                if (strPageIds != null && strPageIds.Length > 0)
                {
                    /// Create Chapter Object
                    objChapterInfo = objWellBookBLL.SetChapterDetails(strParentSiteURL, chapterNode.Value, false);
                    /// Create Pages Collection
                    if (objChapterInfo != null)
                    {
                        arlPageDetails = objWellBookBLL.SetSelectedPageInfo(strParentSiteURL, strPageIds.ToString(), objChapterInfo.ActualAssetValue, objChapterInfo.ColumnName);

                        if (arlPageDetails.Count > 0)
                        {
                            objChapterInfo.PageInfo = arlPageDetails;
                        }
                        if (objChapterInfo.PageInfo == null)
                        {
                            objChapterInfo = null;
                        }
                    }
                }

            }

            //Added IF condition to check the object null or not.
            /// Indicate wheter Chapter Title is Printable or  not.
            /// Added for Print Options CR Implementation
            if (objChapterInfo != null)
                objChapterInfo.IsPrintable = true;

            return objChapterInfo;
        }

        /// <summary>
        /// Get the PageIds based on Chapter
        /// <remarks>Added By Gopinath</remarks>
        /// <remarks>Date : 14/11/2010</remarks>
        /// </summary>
        /// <param name="pageIdList">string</param>
        /// <returns></returns>
        private StringBuilder GetPagesInEachChapter(string pageIdList)
        {
            string strSplitter = ";";
            StringBuilder strPageIds = null;
            List<string> FilteredPageIdsList;
            //Compare PageIds
            //Modified By Gopinath
            //Date : 12/11/2010
            //Description : Filter the Tree view selected page ids with Filter Options
            #region Filter Page Ids
            //Check if filtered CAML query returns any PageIds
            //Global declared "strPageIdList"
            if (strPageIdList != null && strPageIdList.Count > 0)
            {
                int intPageCount = strPageIdList.Count;
                if (pageIdList != null && pageIdList.Length > 0)
                {
                    string strPageIdsWithSplitter = pageIdList.ToString();
                    char[] chSplitterSemicolon = { ';' };
                    string[] strTreeViewPageArray = strPageIdsWithSplitter.Split(chSplitterSemicolon);
                    FilteredPageIdsList = new List<string>();

                    if (strTreeViewPageArray != null && strTreeViewPageArray.Length > 0)
                    {
                        for (int count = 0; count < strTreeViewPageArray.Length; count++)
                        {
                            //Compare TreeView selected PageId list and Filtered PageIds list.
                            //If both PageIds are match add into new collection bag.
                            if (strPageIdList.Contains(strTreeViewPageArray[count]))
                            {
                                FilteredPageIdsList.Add(strTreeViewPageArray[count]);
                            }
                        }
                    }

                    strPageIds = new StringBuilder(); //Clean the PageIds object

                    //Build FilteredPageIdsList string builder
                    if (FilteredPageIdsList != null && FilteredPageIdsList.Count > 0)
                    {
                        foreach (string strPageId in FilteredPageIdsList)
                        {
                            strPageIds.Append(strPageId);
                            strPageIds.Append(strSplitter);
                        }
                    }
                }
            }
            return strPageIds;
            #endregion Filter Page Ids
            ///End Gopinath Code           
        }

        /// <summary>
        /// Returns the java script to open Print Option Page.
        /// </summary>
        /// <returns></returns>
        protected string RegisterJavaScript()
        {
            string strJavaScriptMethod = "return openStoryBoardConfirmation(null);";
            bool blnUserControlFound = false;
            Telerik.Web.UI.RadTreeView trvWellBook = null;
            if (this.Parent.Parent.GetType().Equals(typeof(SPWebPartManager)))
            {
                foreach (Control ctrlWebPart in this.Parent.Parent.Controls)
                {
                    if (ctrlWebPart.GetType().Equals(typeof(TreeViewControl)))
                    {

                        Control ctrlTreeView = ctrlWebPart.FindControl("RadTreeView1");
                        if (ctrlTreeView != null && ctrlTreeView.GetType().Equals(typeof(RadTreeView)))
                        {
                            trvWellBook = (RadTreeView)ctrlWebPart.FindControl("RadTreeView1");
                            blnUserControlFound = true;
                            break;
                        }
                        if (blnUserControlFound)
                        {
                            break;
                        }
                    }
                }
                if (blnUserControlFound)
                {
                    strJavaScriptMethod = "return openStoryBoardConfirmation('" + trvWellBook.ClientID + "');";
                }
            }
            return strJavaScriptMethod;
        }

        #endregion
    }

    /// <summary>
    /// Class to get secure communicatio URL [HTTPS]
    /// </summary>
    public class SslRequiredWebPart
    {
        /// <summary>
        /// Redirect from HTTP to HTTPS (if required).
        /// </summary>
        public SslRequiredWebPart()
        {
        }
        /// <summary>
        /// Determines whether this instance is HTTPS.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is HTTPS; otherwise, <c>false</c>.
        /// </returns>
        private bool IsHttps()
        {
            HttpContext context = HttpContext.Current;

            if (context != null)
            {
                if (context.Request.IsSecureConnection)
                {
                    return true;
                }
            }
            return false;

        }

        /// <summary>
        /// Determines if an SSL redirect is required.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>
        /// 	<c>true</c> if a redirect from HTTP to HTTPS is required,
        /// otherwise <c>false</c>.
        /// </returns>
        public string GetSslURL(string url)
        {
            string strSslUrl = SPContext.Current.Site.Url;
            if (IsHttps())
            {
                if (!string.IsNullOrEmpty(url))
                {
                    if (!url.Contains("https") || !url.Contains("HTTPS"))
                    {

                        if (url.Contains("http"))
                        {
                            strSslUrl = url.Replace("http", "https");
                        }
                        else if (url.Contains("HTTP"))
                        {
                            strSslUrl = url.Replace("HTTP", "HTTPS");
                        }
                    }
                    else
                    {
                        strSslUrl = url;
                    }
                }
                else
                {
                    HttpRequest request = HttpContext.Current.Request;

                    if (request != null)
                    {
                        strSslUrl = request.Url.AbsoluteUri.Substring(0, request.Url.AbsoluteUri.IndexOf("/Pages"));
                    }
                    if (strSslUrl.Contains("http"))
                    {
                        strSslUrl = strSslUrl.Replace("http", "https");
                    }
                    else if (strSslUrl.Contains("HTTP"))
                    {
                        strSslUrl = strSslUrl.Replace("HTTP", "HTTPS");
                    }

                }
            }
            else
            {
                strSslUrl = url;
            }
            return strSslUrl;
        }
    }

}


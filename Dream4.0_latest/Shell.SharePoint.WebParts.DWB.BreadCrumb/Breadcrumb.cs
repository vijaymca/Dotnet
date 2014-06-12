#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : Breadcrumb.cs
#endregion

using System;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Net;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.SharePoint;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DWB.BusinessLogicLayer;
using Shell.SharePoint.DWB.Business.DataObjects;

namespace Shell.SharePoint.WebParts.DWB.BreadCrumb
{
    /// <summary>
    /// This class displays the navigation links in DWB
    /// </summary>
        
    [DefaultProperty("Text"),
       ToolboxData("<{0}:Breadcrumb runat=server></{0}:Breadcrumb>"),
     XmlRoot(Namespace = "Shell.SharePoint.WebParts.DWB.BreadCrumb")]    
    public class Breadcrumb : WebPart
    {
        #region Declaration
        private const string strDefaultText = "Digital Well Book Home";
        const string USERLIST = "DWB User";
       /// page urls 
        const string MAINTAINBOOKSURL = "/Pages/BookMaintenance.aspx";
        const string MAINTAINTEMPLATESURL = "/Pages/TemplateMaintenance.aspx";        
        const string MAINTAINMASTERPAGEURL = "/Pages/MaintainMasterPage.aspx";
        const string USERREGISTRATIONURL = "/Pages/UserRegistration.aspx";
        const string TEAMREGISTRATIONURL = "/Pages/TeamRegistration.aspx";
        const string VIEWBOOKURL = "/Pages/WellBookViewer.aspx";
        const string VIEWPAGESELECTIONURL = "/Pages/BookPageSummary.aspx";
        const string NEWBOOKURL = null;
        const string EDITBOOKURL = null;
        const string MAINTAINCHAPTERSURL = "/Pages/MaintainChapters.aspx";
        const string MAINTAINBOOKPAGESURL = "/Pages/MaintainBookPages.aspx";
        const string EDITBOOKPAGESURL = null;
        const string CHANGEPAGEOWNERURL = null;
        const string NEWCHAPTERURL = null;
        const string ALTERCHAPTERSEQUENCEURL = null;
        const string ALTERCHAPTERPAGESEQUENCEURL = null;
        const string EDITCHAPTERURL = null;
        const string MAINTAINCHAPTERPAGESURL = "/Pages/MaintainChaptersPages.aspx";
        const string NEWCHAPTERPAGEURL = null;
        const string EDITCHAPTERPAGEURL = null;

        const string ALTERSEQUENCEURL = null;
        const string NEWTEMPLATEURL = null;
        const string EDITTEMPLATEURL = null;
        const string EDITTEMPLATEPAGEURL = null;
        const string MAINTAINTEMPLATEPAGEURL = "/Pages/MaintainTemplatePages.aspx";
        const string ADREMOVEPAGESTEMPLATEURL = null;
        const string ALTERTEMPLATEPAGESSEQUENCEURL = null;
        const string EDITTEMPLATEMASTERPAGEURL = null;
        const string NEWMASTERPAGEURL = null;
        const string ALTERPAGESEQUENCEURL = null;
        const string EDITMASTERPAGEURL = null;
        const string NEWUSERURL = null;
        const string EDITUSERURL = null;
        const string EDITSYSTEMPRIVILEGESURL = null;
        const string NEWTEAMURL = null;
        const string EDITTEAMURL = null;
        const string STAFFLISTURL = "/Pages/StaffList.aspx";
        const string ADDREMOVESTAFFURL = null;
        const string RANKSTAFFURL = null;
        const string EDITSTAFFPRIVILEGESURL = null;
        const string HOMEURL = "/Pages/DWBHomePage.aspx";

        /// display names
        const string MAINTAINBOOKSDISPLAYNAME = "Maintain Books";
        const string MAINTAINTEMPLATESDISPLAYNAME = "Maintain Templates";
        const string MAINTAINMASTERPAGESDISPLAYNAME = "Maintain Master Pages";
        const string USERREGISTRATIONDISPLAYNAME = "User Registration";
        const string TEAMREGISTRATIONDISPLAYNAME = "Team Registration";
        const string VIEWBOOKDISPLAYNAME = "View Book";
        const string VIEWPAGESELECTIONDISPLAYNAME = "View Page Selection";
        const string NEWBOOKDISPLAYNAME = "New Book";
        const string EDITBOOKDISPLAYNAME = "Edit Book";
        const string MAINTAINCHAPTERSDISPLAYNAME = "Maintain Chapters";
        const string MAINTAINBOOKPAGESDISPLAYNAME = "Maintain Book Pages";
        const string EDITBOOKPAGESDISPLAYNAME = "Edit Book Page";
        const string CHANGEPAGEOWNERDISPLAYNAME = "Change Page Owner";
        const string NEWTEMPLATEDISPLAYNAME = "New Template";
        const string EDITTEMPLATEDISPLAYNAME = "Edit Template";
        const string EDITTEMPLATEPAGEDISPLAYNAME = "Edit Template Page";
        const string MAINTAINTEMPLATEPAGEDISPLAYNAME = "Maintain Template Pages";
        const string ADDREMOVETEMPLATEPAGESDISPLAYNAME = "Add / Remove Pages";
        const string ALTERTEMPLATEPAGESEQUENCE = "Alter Page Sequence";
        const string EDITTEMPLATEMASTERPAGE = "Edit Master Page";
        const string NEWMASTERPAGEDISPLAYNAME = "New Master Page";
        const string EDITMASTERPAGEDISPLAYNAME = "Edit Master Page";
        const string ALTERPAGESEQUENCEDISPLAYNAME = "Alter Page Sequence";
        const string NEWUSERDISPLAYNAME = "New User";
        const string EDITUSERDISPLAYNAME = "Edit User";       
        const string EDITSYSTEMPRIVILEGESDISPLAYNAME = "System Privileges";
        const string NEWTEAMDISPLAYNAME = "New Team";
        const string EDITTEAMDISPLAYNAME = "Edit Team";
        const string STAFFLISTDISPLAYNAME = "Staff List";
        const string ADDREMOVESTAFFDISPLAYNAME = "Add / Remove Staff";
        const string RANKSTAFFDISPLAYNAME = "Rank Staff";
        const string EDITSTAFFPRIVILEGES = "Edit Staff Privileges";
        const string NEWCHAPTERDISPLAYNAME = "New Chapter";
        const string ALTERCHAPTERSEQUENCEDISPLAYNAME = "Alter Chapter Sequence";
        const string ALTERCHAPTERPAGESEQUENCEDISPLAYNAME = "Alter Page Sequence";
        const string EDITCHAPTERDISPLAYNAME = "Edit Chapter";
        const string MAINTAINCHAPTERPAGESDISPLAYNAME = "Maintain Chapter Pages";
        const string NEWCHAPTERPAGEDISPLAYNAME = "Add Page";
        const string EDITCHAPTERPAGEDISPLAYNAME = "Edit Page";
        const string ALTERSEQUENCEDISPLAYNAME = "Alter Sequence";
        #region DREAM 4.0 - Consistent use of eWB2 requirement changes
        /// <summary>
        /// eWell Book II is replaced with eWB2.
        /// </summary>
        const string HOME = "eWB2";//"eWell Book II";//"Digital Well Book Home";
        #endregion 
        const string ROOTELEMENT = "Home";

        public XmlDocument objXmlDocument;
        string strParentSiteURL = SPContext.Current.Site.Url.ToString();

        const string SYSTEMPRIVILEGES = "System Privileges";
        const string STAFFPRIVILEGES = "Staff Privileges";
        #endregion

        #region Methods and Properties
        /// <summary>
        /// Enum for ScreenName
        /// </summary>
        public enum ScreenNameEnum
        {
            [Description(HOME)]
            Home,
            [Description(MAINTAINBOOKSDISPLAYNAME)]
            MaintainBooks,
            [Description(MAINTAINTEMPLATESDISPLAYNAME)]
            MaintainTemplates,
            [Description(MAINTAINMASTERPAGESDISPLAYNAME)]
            MaintainMasterPages,
            [Description(USERREGISTRATIONDISPLAYNAME)]
            UserRegistration,
            [Description(TEAMREGISTRATIONDISPLAYNAME)]
            TeamRegistration,
            [Description(VIEWBOOKDISPLAYNAME)]
            ViewBook,
            [Description(VIEWPAGESELECTIONDISPLAYNAME)]
            ViewPageSelection,
            [Description(NEWBOOKDISPLAYNAME)]
            NewBook,
            [Description(EDITBOOKDISPLAYNAME)]
            EditBook,
            [Description(MAINTAINCHAPTERSDISPLAYNAME)]
            MaintainChapters,
            [Description(MAINTAINBOOKPAGESDISPLAYNAME)]
            MaintainBookPages,
            [Description(EDITBOOKPAGESDISPLAYNAME)]
            EditBookPage,
            [Description(CHANGEPAGEOWNERDISPLAYNAME)]
            ChangePageOwner,
            [Description(NEWTEMPLATEDISPLAYNAME)]
            NewTemplate,
            [Description(EDITTEMPLATEDISPLAYNAME)]
            EditTemplate,
            [Description(MAINTAINTEMPLATEPAGEDISPLAYNAME)]
            MaintainTemplatePages,
            [Description(ADDREMOVETEMPLATEPAGESDISPLAYNAME)]
            AddRemoveTemplatePages,
            [Description(ALTERTEMPLATEPAGESEQUENCE)]
            AlterTemplatePageSequence,
            [Description(EDITTEMPLATEMASTERPAGE)]
            EditTemplateMasterPage,
            [Description(NEWMASTERPAGEDISPLAYNAME)]
            NewMasterPage,
            [Description(EDITMASTERPAGEDISPLAYNAME)]
            EditMasterPage,
             [Description(ALTERPAGESEQUENCEDISPLAYNAME)]
            AlterPageSequence,
            [Description(NEWUSERDISPLAYNAME)]
            NewUser,
            [Description(EDITUSERDISPLAYNAME)]
            EditUser,
            [Description(EDITSYSTEMPRIVILEGESDISPLAYNAME)]
            EditSystemPrivileges,
            [Description(NEWTEAMDISPLAYNAME)]
            NewTeam,
            [Description(EDITTEAMDISPLAYNAME)]
            EditTeam,
            [Description(STAFFLISTDISPLAYNAME)]
            StaffList,
            [Description(ADDREMOVESTAFFDISPLAYNAME)]
            AddRemoveStaff,
            [Description(RANKSTAFFDISPLAYNAME)]
            RankStaff,
            [Description(EDITSTAFFPRIVILEGES)]
            EditStaffPrivileges,
            [Description(NEWCHAPTERDISPLAYNAME)]
            NewChapter,
            [Description(ALTERCHAPTERSEQUENCEDISPLAYNAME)]
            AlterChapterSequence,
            [Description(EDITCHAPTERDISPLAYNAME)]
            EditChapter,
            [Description(MAINTAINCHAPTERPAGESDISPLAYNAME)]
            MaintainChapterPages,
            [Description(NEWCHAPTERPAGEDISPLAYNAME)]
            NewChapterPage,
            [Description(EDITCHAPTERPAGEDISPLAYNAME)]
            EditChapterPage,
            [Description(ALTERCHAPTERPAGESEQUENCEDISPLAYNAME)]
            AlterChapterPageSequence
          
        }

        /// <summary>
        /// Property for Reading and Setting the Screen Name
        /// </summary>
        ScreenNameEnum objScreenName;       
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("Screen Name")]
        [WebDisplayNameAttribute("Screen Name")]
        [WebDescription("Select the Current Screen Name ")]
        public ScreenNameEnum ScreenName
        {
            get
            {
                return objScreenName;
            }
            set
            {
                objScreenName = value;
            }
        }

        /// <summary>
        /// Property to Allow Non DWB Users to view the page or not
        /// </summary>
        bool blnAllowNonRegisteredUsers;
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("Allow Non DWB Users")]
        [WebDisplayNameAttribute("Allow Non DWB Users")]
        [WebDescription("Check if Non DWB Users allowed to view the page")]
        public bool AllowNonRegisteredUsers
        {
            get
            {
                return blnAllowNonRegisteredUsers;
            }
            set
            {
                blnAllowNonRegisteredUsers = value;
            }

        }

        /// <summary>
        /// Renders the bread crumb trail on to the page 
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            try
            {
                base.Render(writer);
                string strBreadCrumbTrail = string.Empty;
                XmlNode objCurrNode = objXmlDocument.DocumentElement.SelectSingleNode("//attribute[@internalName='" + ScreenName + "']");
                /// Construct the hyperlink
                string strCurrentScreenName = string.Empty;
                string[] arBreadCrumb = new string[6];
               
                int intIndex = 0;
                
                if (objCurrNode != null)
                {
                    strCurrentScreenName = objCurrNode.Attributes["displayName"].Value;

                    /// Session Time out issue
                    /// Read the Session value
                    /// Check for the Privilege based on Screen Name and redirect

                    if (CheckPrivileges(ScreenName))
                    {
                        while (objCurrNode.ParentNode != null && objCurrNode.ParentNode.NodeType != XmlNodeType.Document)
                        {

                            if (!string.Equals(objCurrNode.Attributes["internalName"].Value, ScreenNameEnum.ViewBook.ToString()))
                            {
                                #region DREAM 4.0 - AJAX Modification issue fix
                                /// To avoid opening the DWBHomePage.aspx inside the RadSplitter again, only for DWB Home Pages set the target property as "_parent" i.e target="_parent".
                                /// For rest of the windows simply set only the href value
                                if (string.Equals(objCurrNode.ParentNode.Attributes["internalName"].Value,HOME))
                                {
                               
                                    arBreadCrumb[intIndex] = "<a href=\"" + objCurrNode.ParentNode.Attributes["pageUrl"].Value + "\""
                                        + "target=\"_parent\">" + objCurrNode.ParentNode.Attributes["displayName"].Value + "</a>";
                                }
                                else
                                {
                                arBreadCrumb[intIndex] = "<a href=\"" + objCurrNode.ParentNode.Attributes["pageUrl"].Value + "\">" + objCurrNode.ParentNode.Attributes["displayName"].Value + "</a>";
                            }
                                #endregion
                        }
                            intIndex += 1;
                            objCurrNode = objCurrNode.ParentNode;
                        }
                        for (int intLength = arBreadCrumb.Length - 1; intLength >= 0; intLength--)
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(arBreadCrumb.GetValue(intLength))))
                            {
                                strBreadCrumbTrail += arBreadCrumb.GetValue(intLength) + " - ";
                            }
                        }
                        strBreadCrumbTrail += strCurrentScreenName;
                    }
                    else
                    {
                        HttpContext.Current.Response.Redirect("/Pages/DWBExceptionPage.aspx?mode=3",false);
                    }
                }
                else
                    strBreadCrumbTrail = HOME;

                writer.Write("<span style=\"font-style:verdana;font-size:11px;\">" + strBreadCrumbTrail + "</span>");
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(strParentSiteURL, webEx, 1);
                writer.Write("<span style=\"font-style:verdana;font-size:11px;\">" + webEx.Message + "</span>");
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strParentSiteURL, ex);
            }
        }

        /// <summary>
        /// Adds a node with the properties set to the XML Object
        /// </summary>
        /// <param name="attributeDisplayName"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeUrl"></param>
        /// <param name="parentNode"></param>
        private void AddAttributeNode(string attributeDisplayName,string attributeName, string attributeUrl, XmlNode parentNode)
        {
            XmlAttribute displayName = objXmlDocument.CreateAttribute("displayName");
            XmlAttribute internalName = objXmlDocument.CreateAttribute("internalName");
            XmlAttribute pageUrl = objXmlDocument.CreateAttribute("pageUrl");

            XmlElement objAttributeNode = objXmlDocument.CreateElement("attribute");
            
            displayName.InnerText = attributeDisplayName;
            objAttributeNode.Attributes.Append(displayName);
           
            internalName.InnerText = attributeName;
            objAttributeNode.Attributes.Append(internalName);
          
            pageUrl.InnerText = attributeUrl;
            objAttributeNode.Attributes.Append(pageUrl);
                     
            parentNode.AppendChild(objAttributeNode);
            
        }

        /// <summary>
        /// Generates the Bread Crumb trail
        /// </summary>
        private void CreateBreadCrumbTrail()
        {
            try
            {
                #region Home breadcrumb trail

                XmlAttribute displayName = objXmlDocument.CreateAttribute("displayName");
                XmlAttribute internalName = objXmlDocument.CreateAttribute("internalName");
                XmlAttribute pageUrl = objXmlDocument.CreateAttribute("pageUrl");

                XmlElement RootElement = objXmlDocument.CreateElement(ROOTELEMENT);
                XmlNode RootNode = objXmlDocument.AppendChild(RootElement);

                displayName.InnerText = HOME;
                RootNode.Attributes.Append(displayName);

                internalName.InnerText = HOME;
                RootNode.Attributes.Append(internalName);

                pageUrl.InnerText = HOMEURL;
                RootNode.Attributes.Append(pageUrl);

                //Maintain Book BreadCrumb
                AddAttributeNode(MAINTAINBOOKSDISPLAYNAME, ScreenNameEnum.MaintainBooks.ToString(), MAINTAINBOOKSURL, RootNode);

                //Maintain Templates Breadcrumb
                AddAttributeNode(MAINTAINTEMPLATESDISPLAYNAME, ScreenNameEnum.MaintainTemplates.ToString(), MAINTAINTEMPLATESURL, RootNode);

                //Maintain Master Pages Breadcrumb
                AddAttributeNode(MAINTAINMASTERPAGESDISPLAYNAME, ScreenNameEnum.MaintainMasterPages.ToString(), MAINTAINMASTERPAGEURL, RootNode);

                //User Registration Breadcrumb
                AddAttributeNode(USERREGISTRATIONDISPLAYNAME, ScreenNameEnum.UserRegistration.ToString(), USERREGISTRATIONURL, RootNode);

                //Team Registration Breadcrumb
                AddAttributeNode(TEAMREGISTRATIONDISPLAYNAME, ScreenNameEnum.TeamRegistration.ToString(), TEAMREGISTRATIONURL, RootNode);

                //View Book Breadcrumb
                AddAttributeNode(VIEWBOOKDISPLAYNAME, ScreenNameEnum.ViewBook.ToString(), VIEWBOOKURL, RootNode);


                #region Maintain Book breadcrumb trail
                XmlNode MainTainBookNode = RootNode.SelectSingleNode("//attribute[@internalName='MaintainBooks']");

                //New Book Breadcrumb
                AddAttributeNode(NEWBOOKDISPLAYNAME, ScreenNameEnum.NewBook.ToString(), NEWBOOKURL, MainTainBookNode);

                //Edit Book Breadcrumb
                AddAttributeNode(EDITBOOKDISPLAYNAME, ScreenNameEnum.EditBook.ToString(), EDITBOOKURL, MainTainBookNode);

                //MaintainChapters Breadcrumb
                AddAttributeNode(MAINTAINCHAPTERSDISPLAYNAME, ScreenNameEnum.MaintainChapters.ToString(), MAINTAINCHAPTERSURL, MainTainBookNode);

                //MaintainBookPages Breadcrumb(child here too)
                AddAttributeNode(MAINTAINBOOKPAGESDISPLAYNAME, ScreenNameEnum.MaintainBookPages.ToString(), MAINTAINBOOKPAGESURL, MainTainBookNode);

                #region Maintain book pages trail
                XmlNode MaintainBookPagesNode = RootNode.SelectSingleNode("//attribute[@internalName='MaintainBookPages']");

                //EditBookPages Breadcrumb(child here too)
                AddAttributeNode(EDITBOOKPAGESDISPLAYNAME, ScreenNameEnum.EditBookPage.ToString(), EDITBOOKPAGESURL, MaintainBookPagesNode);

                #endregion
                //ChangePageOwner Breadcrumb
                AddAttributeNode(CHANGEPAGEOWNERDISPLAYNAME, ScreenNameEnum.ChangePageOwner.ToString(), CHANGEPAGEOWNERURL, MainTainBookNode);

                #region  Maintain Chapters breadcrumb trail
                XmlNode MaintainChapterNode = RootNode.SelectSingleNode("//attribute[@internalName='MaintainChapters']");

                //NewChapter Breadcrumb(child here too)
                AddAttributeNode(NEWCHAPTERDISPLAYNAME, ScreenNameEnum.NewChapter.ToString(), NEWCHAPTERURL, MaintainChapterNode);


                //AlterChapterSequence Breadcrumb
                AddAttributeNode(ALTERCHAPTERSEQUENCEDISPLAYNAME, ScreenNameEnum.AlterChapterSequence.ToString(), ALTERCHAPTERSEQUENCEURL, MaintainChapterNode);

                //EditChapter Breadcrumb
                AddAttributeNode(EDITCHAPTERDISPLAYNAME, ScreenNameEnum.EditChapter.ToString(), EDITCHAPTERURL, MaintainChapterNode);


                //Maintain Chapter Pages Breadcrumb
                AddAttributeNode(MAINTAINCHAPTERPAGESDISPLAYNAME, ScreenNameEnum.MaintainChapterPages.ToString(), MAINTAINCHAPTERPAGESURL, MaintainChapterNode);

                #region Maintain Chapter Pages Breadcrumb trail
                XmlNode MaintainChapterPagesNode = RootNode.SelectSingleNode("//attribute[@internalName='MaintainChapterPages']");

                //AddChapterPage Breadcrumb
                AddAttributeNode(NEWCHAPTERPAGEDISPLAYNAME, ScreenNameEnum.NewChapterPage.ToString(), NEWCHAPTERPAGEURL, MaintainChapterPagesNode);

                //EditChapterPage Breadcrumb
                AddAttributeNode(EDITCHAPTERPAGEDISPLAYNAME, ScreenNameEnum.EditChapterPage.ToString(), EDITCHAPTERPAGEURL, MaintainChapterPagesNode);


                //AlterSequence Breadcrumb
                AddAttributeNode(ALTERCHAPTERPAGESEQUENCEDISPLAYNAME, ScreenNameEnum.AlterChapterPageSequence.ToString(), ALTERCHAPTERPAGESEQUENCEURL, MaintainChapterPagesNode);

                #endregion

                #endregion

                #endregion

                #region Maintain Template breadcrumb trail
                XmlNode MainTainTemplateNode = RootNode.SelectSingleNode("//attribute[@internalName='MaintainTemplates']");

                //New Template Breadcrumb
                AddAttributeNode(NEWTEMPLATEDISPLAYNAME, ScreenNameEnum.NewTemplate.ToString(), NEWTEMPLATEURL, MainTainTemplateNode);

                //Edit Template Breadcrumb
                AddAttributeNode(EDITTEMPLATEDISPLAYNAME, ScreenNameEnum.EditTemplate.ToString(), EDITTEMPLATEURL, MainTainTemplateNode);

                //MaintainTemplatePages Template Breadcrumb(child here)
                AddAttributeNode(MAINTAINTEMPLATEPAGEDISPLAYNAME, ScreenNameEnum.MaintainTemplatePages.ToString(), MAINTAINTEMPLATEPAGEURL, MainTainTemplateNode);

                #region Maintain Template Page breadcrumbtrail
                XmlNode MaintainTemplatePageNode = RootNode.SelectSingleNode("//attribute[@internalName='MaintainTemplatePages']");

                //AddRemPage Breadcrumb
                AddAttributeNode(ADDREMOVETEMPLATEPAGESDISPLAYNAME, ScreenNameEnum.AddRemoveTemplatePages.ToString(), ADREMOVEPAGESTEMPLATEURL, MaintainTemplatePageNode);


                //AlterPageSequence Breadcrumb
                AddAttributeNode(ALTERTEMPLATEPAGESEQUENCE, ScreenNameEnum.AlterTemplatePageSequence.ToString(), ALTERTEMPLATEPAGESSEQUENCEURL, MaintainTemplatePageNode);


                //EditMasterPage Breadcrumb
                AddAttributeNode(EDITTEMPLATEMASTERPAGE, ScreenNameEnum.EditTemplateMasterPage.ToString(), EDITTEMPLATEMASTERPAGEURL, MaintainTemplatePageNode);
                #endregion

                #endregion

                #region Maintain Master Pages breadcrumb trail
                XmlNode MaintainMasterPageNode = RootNode.SelectSingleNode("//attribute[@internalName='MaintainMasterPages']");

                //NewMasterPage  Breadcrumb
                AddAttributeNode(NEWMASTERPAGEDISPLAYNAME, ScreenNameEnum.NewMasterPage.ToString(), NEWMASTERPAGEURL, MaintainMasterPageNode);


                //AlterPageSequence  Breadcrumb
                AddAttributeNode(ALTERPAGESEQUENCEDISPLAYNAME, ScreenNameEnum.AlterPageSequence.ToString(), ALTERPAGESEQUENCEURL, MaintainMasterPageNode);

                //EditMasterPage  Breadcrumb
                AddAttributeNode(EDITMASTERPAGEDISPLAYNAME, ScreenNameEnum.EditMasterPage.ToString(), EDITMASTERPAGEURL, MaintainMasterPageNode);

                #endregion

                #region User Registration breadcrumb trail
                XmlNode UserRegistrationNode = RootNode.SelectSingleNode("//attribute[@internalName='UserRegistration']");

                //NewUser  Breadcrumb
                AddAttributeNode(NEWUSERDISPLAYNAME, ScreenNameEnum.NewUser.ToString(), NEWUSERURL, UserRegistrationNode);

                //EditUser  Breadcrumb
                AddAttributeNode(EDITUSERDISPLAYNAME, ScreenNameEnum.EditUser.ToString(), EDITUSERURL, UserRegistrationNode);

                //EditSystemPrivileges  Breadcrumb
                AddAttributeNode(EDITSYSTEMPRIVILEGESDISPLAYNAME, ScreenNameEnum.EditSystemPrivileges.ToString(), EDITSYSTEMPRIVILEGESURL, UserRegistrationNode);


                #endregion

                #region Team Registration breadcrumb trail
                XmlNode TeamRegistrationNode = RootNode.SelectSingleNode("//attribute[@internalName='TeamRegistration']");

                //NewTeam  Breadcrumb
                AddAttributeNode(NEWTEAMDISPLAYNAME, ScreenNameEnum.NewTeam.ToString(), NEWTEAMURL, TeamRegistrationNode);

                //EditTeam  Breadcrumb
                AddAttributeNode(EDITTEAMDISPLAYNAME, ScreenNameEnum.EditTeam.ToString(), EDITTEAMURL, TeamRegistrationNode);

                //StaffList  Breadcrumb
                AddAttributeNode(STAFFLISTDISPLAYNAME, ScreenNameEnum.StaffList.ToString(), STAFFLISTURL, TeamRegistrationNode);

                #region StaffList Breadcrumb trail
                XmlNode StaffListNode = RootNode.SelectSingleNode("//attribute[@internalName='StaffList']");

                //StaffList  Breadcrumb
                AddAttributeNode(ADDREMOVESTAFFDISPLAYNAME, ScreenNameEnum.AddRemoveStaff.ToString(), ADDREMOVESTAFFURL, StaffListNode);

                //RankStaff  Breadcrumb
                AddAttributeNode(RANKSTAFFDISPLAYNAME, ScreenNameEnum.RankStaff.ToString(), RANKSTAFFURL, StaffListNode);


                //Edit Staff Privileges  Breadcrumb
                AddAttributeNode(EDITSTAFFPRIVILEGES, ScreenNameEnum.EditStaffPrivileges.ToString(), EDITSTAFFPRIVILEGESURL, StaffListNode);

                #endregion

                #endregion

                #region ViewBook breadcrumb trail 
                XmlNode ViewBookNode = RootNode.SelectSingleNode("//attribute[@internalName='ViewBook']");

                //NewTeam  Breadcrumb
                AddAttributeNode(VIEWPAGESELECTIONDISPLAYNAME, ScreenNameEnum.ViewPageSelection.ToString(), VIEWPAGESELECTIONURL, ViewBookNode);
                #endregion

                #endregion
                CommonUtility.SetSessionVariable(Page, "BreadCrumbXML", objXmlDocument.OuterXml);
                //HttpContext.Current.Session["BreadCrumbXML"] = objXmlDocument;
            }
            catch 
            {
                throw;
            }
        }

        /// <summary>
        /// Initialises controls and Creates Breadcrumb Trail if not present already in Session 
        /// </summary>
        protected override void CreateChildControls()
        {
            try
            {
                base.CreateChildControls();
                objXmlDocument = new XmlDocument();
                object objSessionXML = CommonUtility.GetSessionVariable(Page, "BreadCrumbXML");

                if (objSessionXML != null)
                {
                    objXmlDocument.LoadXml((string)objSessionXML);

                    if (HttpContext.Current.Request.Url.Query != null)
                    {
                        UpdateProperty();
                    }
                    CommonUtility.SetSessionVariable(Page, "BreadCrumbXML", objXmlDocument.OuterXml);
                }
                else
                    CreateBreadCrumbTrail();
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(strParentSiteURL, webEx, 1);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strParentSiteURL, ex);
            }
        }

        /// <summary>
        /// Updates the url of the current screen name node.
        /// </summary>
        protected void UpdateProperty()
        {
            XmlNode objCurrentNode = objXmlDocument.DocumentElement.SelectSingleNode("//attribute[@internalName='" + ScreenName.ToString() + "']");

            /// To avoid the Null Exception on Home click below checks are added            
            if (objCurrentNode != null)
            {
                if (!string.IsNullOrEmpty(objCurrentNode.Attributes["pageUrl"].Value) && objCurrentNode.Attributes["pageUrl"].Value.Contains("?"))
                    objCurrentNode.Attributes["pageUrl"].Value = objCurrentNode.Attributes["pageUrl"].Value.Remove(objCurrentNode.Attributes["pageUrl"].Value.IndexOf("?"));

                objCurrentNode.Attributes["pageUrl"].Value += HttpContext.Current.Request.Url.Query;
            }
        }
              
        /// <summary>
        /// Check the current login user has permission for the screen.
        /// </summary>
        /// <param name="enmScreenName">ScreenNameEnum.</param>
        /// <returns>true/false.</returns>
        protected bool CheckPrivileges(ScreenNameEnum enmScreenName)
        {
            string strRequiredPrivileges = string.Empty;
            bool blnUserPermittedToVisitPage = false;
            Privileges objPrivileges = null;
            object objStoredPrivilege = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPrivileges.ToString());
            
            /// Reload the session again if Privileges is null           
            if (objStoredPrivilege != null)
            {
                objPrivileges = (Privileges)objStoredPrivilege;
            }
            else
            {
                objPrivileges = GetUserPrivileges();
                CommonUtility.SetSessionVariable(Page, enumSessionVariable.UserPrivileges.ToString(), objPrivileges);                
            }

            if (objPrivileges != null)
            {                
                if (objPrivileges.SystemPrivileges != null && objPrivileges.SystemPrivileges.AdminPrivilege)
                {
                    blnUserPermittedToVisitPage = true;
                    return blnUserPermittedToVisitPage;
                }

                switch (enmScreenName)
                {
                    case ScreenNameEnum.Home:
                    case ScreenNameEnum.ViewBook:
                        {                            
                            if (objPrivileges.SystemPrivileges != null)
                            {
                                blnUserPermittedToVisitPage = true;
                            }
                            else if (objPrivileges.IsNonDWBUser && AllowNonRegisteredUsers)
                            {
                                blnUserPermittedToVisitPage = true;
                            }
                            break;
                        }                    
                    case ScreenNameEnum.MaintainBooks:
                    case ScreenNameEnum.MaintainBookPages:
                    case ScreenNameEnum.MaintainChapters:
                    case ScreenNameEnum.MaintainChapterPages:                    
                    case ScreenNameEnum.EditBook:
                    case ScreenNameEnum.NewChapter:
                    case ScreenNameEnum.EditChapter:
                    case ScreenNameEnum.NewChapterPage:
                    case ScreenNameEnum.EditChapterPage:
                    case ScreenNameEnum.EditBookPage:
                    case ScreenNameEnum.ChangePageOwner:
                    case ScreenNameEnum.AlterChapterSequence:
                    case ScreenNameEnum.AlterChapterPageSequence:
                        {

                            if (objPrivileges.SystemPrivileges != null)
                            {
                                if (objPrivileges.SystemPrivileges.BookOwner)
                                {
                                    blnUserPermittedToVisitPage = true;
                                }
                                else if ((objPrivileges.SystemPrivileges.DWBUser || objPrivileges.SystemPrivileges.PageOwner) && (!string.IsNullOrEmpty(objPrivileges.FocalPoint.BookIDs)))
                                {
                                    blnUserPermittedToVisitPage = true;
                                }
                            }                           
                            break;
                        }
                    case ScreenNameEnum.NewBook:
                        {
                            if (objPrivileges.SystemPrivileges != null)
                            {
                                if (objPrivileges.SystemPrivileges.BookOwner)
                                {
                                    blnUserPermittedToVisitPage = true;
                                }                               
                            }        
                            break;
                        }
                    case ScreenNameEnum.MaintainMasterPages:
                    case ScreenNameEnum.NewMasterPage:
                    case ScreenNameEnum.EditMasterPage:
                    case ScreenNameEnum.AlterPageSequence:
                        {

                            if (objPrivileges.SystemPrivileges != null)
                            {
                                if (objPrivileges.SystemPrivileges.AdminPrivilege)
                                {
                                    blnUserPermittedToVisitPage = true;
                                }
                            }

                            break;
                        }
                    case ScreenNameEnum.MaintainTemplates:
                    case ScreenNameEnum.MaintainTemplatePages:
                    case ScreenNameEnum.NewTemplate:
                    case ScreenNameEnum.EditTemplate:
                    case ScreenNameEnum.EditTemplateMasterPage:
                    case ScreenNameEnum.AlterTemplatePageSequence:
                    case ScreenNameEnum.AddRemoveTemplatePages:
                        {
                            if (objPrivileges.SystemPrivileges != null)
                            {
                                if (objPrivileges.SystemPrivileges.AdminPrivilege)
                                {
                                    blnUserPermittedToVisitPage = true;
                                }
                            }

                            break;
                        }
                    case ScreenNameEnum.UserRegistration:
                    case ScreenNameEnum.NewUser:
                    case ScreenNameEnum.EditUser:
                    case ScreenNameEnum.EditSystemPrivileges:
                        {
                            if (objPrivileges.SystemPrivileges != null)
                            {
                                if (objPrivileges.SystemPrivileges.AdminPrivilege)
                                {
                                    blnUserPermittedToVisitPage = true;
                                }
                            }
                            break;
                        }
                    case ScreenNameEnum.TeamRegistration:
                    case ScreenNameEnum.NewTeam:
                    case ScreenNameEnum.EditTeam:
                    case ScreenNameEnum.StaffList:
                    case ScreenNameEnum.AddRemoveStaff:
                    case ScreenNameEnum.RankStaff:
                    case ScreenNameEnum.EditStaffPrivileges:
                        {
                            if (objPrivileges.SystemPrivileges != null)
                            {
                                if (objPrivileges.SystemPrivileges.AdminPrivilege || objPrivileges.SystemPrivileges.BookOwner)
                                {
                                    blnUserPermittedToVisitPage = true;
                                }
                            }
                            break;
                        }
                    case ScreenNameEnum.ViewPageSelection:
                        {                           
                            if (objPrivileges.SystemPrivileges != null)
                            {
                                blnUserPermittedToVisitPage = true;
                            }
                            else if (!objPrivileges.IsNonDWBUser )
                            {
                                blnUserPermittedToVisitPage = true;
                            }
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            return blnUserPermittedToVisitPage;
        }

        /// <summary>
        /// Gets the user priviledges from the list.
        /// </summary>
        private Privileges GetUserPrivileges()
        {
            AdminBLL objMOSSService = new AdminBLL();
            Privileges objPrivileges = null;
        
            CommonUtility objUtility = new CommonUtility();
           string userId = objUtility.GetUserName();
           DataTable dtSystemPrivileges = objMOSSService.GetDWBPrivileges(strParentSiteURL, userId, SYSTEMPRIVILEGES);
          
            if (dtSystemPrivileges != null )
            {
                objPrivileges = objMOSSService.SetPrivilegesObjects(strParentSiteURL, dtSystemPrivileges);
            }
            else
            {
                objPrivileges = new Privileges();
                objPrivileges.IsNonDWBUser = true;
            }

            if (dtSystemPrivileges != null)
            {
                dtSystemPrivileges.Dispose();
            }            
            return objPrivileges;
        }
        #endregion
    }
}

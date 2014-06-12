#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: TreeViewControl.cs
#endregion

using System;
using System.Net;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DWB.BusinessLogicLayer;
using Shell.SharePoint.DWB.Business.DataObjects;
using Microsoft.SharePoint;
using Telerik.Web.UI;


namespace Shell.SharePoint.WebParts.DWB.TreeViewControl
{
    /// <summary>
    /// Well Book Viewer TreeView Control.
    /// Shows the Chapters and Pages in terms of nodes in a treeview.
    /// </summary>
    public class TreeViewControl : WebPart
    {
        #region Declarations

        #region Constants
        const string HIDETAB_JS_KEY = "Hide Tab";
        const string HIDETAB_JS = @"<Script language='javaScript'>HideTabs('RadTabStrip1', 'none');</Script>";
        const string QUERYSTRING_BOOKID = "BookID";
        const string QUERYSTRING_CHAPTERID = "ChapterID";
        const string QUERYSTRING_IDVALUE = "idValue";
        const string SESSION_WEBPARTPROPERTIES = "WEBPARTPROPERTIES";
        const string SESSION_TREEVIEWDATAOBJECT = "TreeViewDataObject";
        const string SESSION_TREEVIEWXML = "TreeViewXML";
        const string WELLBOOK_VIEWER_TITLE = "Well Book Viewer";
        const string CONNECTION_TYPE3_PAGES = "3 - UserDefinedDocument";
        const string CONNECTION_TYPE2_PAGES = "2 - PublishedDocument";
        const string TYPE1_REPORT_NAME_WELLHISTORY = "1 - Automated (Well History)";
        const string TYPE1_REPORT_NAME_WELLBOREHEADER = "1 - Automated (Wellbore Header)";
        const string TYPE1_REPORT_NAME_PREPROD_RFT = "1 - Automated (PreProd-RFT)";
        const string TYPE1_REPORT_NAME_WELLSUMMARY = "1 - Automated (WellSummary)";
        const string TYPE1_WELLHISTORY_PAGE_URL = "DWBWellHistoryReport.aspx";
        const string TYPE1_WELLBORE_HEADER_PAGE_URL = "DWBWellboreHeader.aspx";
        const string TYPE1_PREPROD_RFT_PAGE_URL = "DWBPreProductionRFT.aspx";
        const string TYPE1_WELLSUMMARY_PAGE_URL = "WellSummary.aspx";
        const string CONNECTION_TYPE_COLUMN_NAME = "ConnectionType";
        const string PAGE_URL_COLUMN_NAME = "PageURL";

        const string QUERYSTRINGSEARCHTYPE = "SearchType";
        const string DWBSEARCHTYPE = "DWBContextSearch";

        const string HIDETABJSKEY = "Hide Tab";
        const string HIDETABJSPAGE = @"<Script language='javaScript'>HideTabs('RadTabStrip1', 'none');</Script>";

        #region DREAM 4.0 - eWB 2.0
        const string CHAPTERPREFERENCEDOCLIB = "eWB Customise Chapter";
        const string CHAPTERPREFERENCEXML = "ewbreorderchapterxml";
        const string MOSSSERVICE = "MossService";
        #endregion
        #endregion

        Panel pnlWellBookTreeView;
        RadTreeView trvWellBookViewer;
        HiddenField hdnAssetValue;
        string strParentSiteURL = SPContext.Current.Site.Url;
        TreeNodeSelection objTreeNodeValues;

        #region DREAM 4.0 - eWB2.0        
        #region Hide/Reveal Empty Pages
        HiddenField hdnShowEmptyPages;
        CheckBox chkBoxHideEmptyPages;
        #endregion 

        #region Customise Chapters
        const string EVENTARG = "__EVENTARGUMENT";
        const string EVENTTARGET = "__EVENTTARGET";
        #endregion 

        #endregion
        #endregion

        #region Event Handlers
        /// <summary>
        /// Raises the webpart OnInit event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                /// Javascript fix for RadTreeView control.
                Page.ClientScript.RegisterStartupScript(typeof(TreeViewControl), this.ID, "_spOriginalFormAction = document.forms[0].action;_spSuppressFormOnSubmitWrapper=true;", true);
                if (this.Page.Form != null)
                {
                    string strFormOnSubmitAtt = this.Page.Form.Attributes["onsubmit"];
                    if (!string.IsNullOrEmpty(strFormOnSubmitAtt) && string.Compare(strFormOnSubmitAtt, "return _spFormOnSubmitWrapper();") == 0)
                    {
                        this.Page.Form.Attributes["onsubmit"] = "_spFormOnSubmitWrapper();";
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strParentSiteURL, ex);
            }
        }

        /// <summary>
        /// Raises the webpart Onload event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            //  base.OnLoad(e);
            EnsureChildControls();
            try
            {
                pnlWellBookTreeView = new Panel();
                pnlWellBookTreeView.ID = "Panel1";
                this.Controls.Add(pnlWellBookTreeView);

                trvWellBookViewer = new RadTreeView();
                trvWellBookViewer.ID = "RadTreeView1";
                trvWellBookViewer.CheckBoxes = true;
              //  if (this.Page.Request.Params[EVENTTARGET] == null || (!this.Page.Request.Params[EVENTTARGET].ToLowerInvariant().Equals("customisechaptersinsession")))
         //       {
                    LoadRootNodes(trvWellBookViewer, TreeNodeExpandMode.ServerSideCallBack);
               // }
                    
                    trvWellBookViewer.NodeExpand += new RadTreeViewEventHandler(treeView_NodeExpand);
                    trvWellBookViewer.NodeClick += new RadTreeViewEventHandler(treeView_NodeClick);
                    trvWellBookViewer.CheckAllNodes();
                    trvWellBookViewer.TriStateCheckBoxes = true;
                    trvWellBookViewer.CheckChildNodes = true;
                
                pnlWellBookTreeView.Controls.Add(trvWellBookViewer);
                this.Controls.Add(pnlWellBookTreeView);
                if (!Page.IsPostBack)
                {
                    objTreeNodeValues = new TreeNodeSelection();
                    if (trvWellBookViewer != null)
                    {
                        if (trvWellBookViewer.FindNodeByValue(HttpContext.Current.Request.QueryString[QUERYSTRING_BOOKID]) != null)
                        {
                            trvWellBookViewer.FindNodeByValue(HttpContext.Current.Request.QueryString[QUERYSTRING_BOOKID]).Selected = true;
                        }
                        if (trvWellBookViewer.SelectedNode != null && (trvWellBookViewer.SelectedNode.Level == 0 || trvWellBookViewer.SelectedNode.Level == 1))
                        {
                            Page.ClientScript.RegisterStartupScript(this.GetType(), HIDETAB_JS_KEY, HIDETAB_JS);
                        }
                    }
                    objTreeNodeValues.IsBookSelected = true;
                    objTreeNodeValues.BookID = HttpContext.Current.Request.QueryString[QUERYSTRING_BOOKID];
                    HttpContext.Current.Session[SESSION_WEBPARTPROPERTIES] = objTreeNodeValues;
                }
                ExpandChapterNode();        
                this.Page.Title = WELLBOOK_VIEWER_TITLE;
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(strParentSiteURL, webEx);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strParentSiteURL, ex);
            }
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            try
            {
                #region DREAM 4.0 - eWB2.0

                chkBoxHideEmptyPages = new CheckBox();
                chkBoxHideEmptyPages.ID = "chkBoxHideEmptyPages";
                chkBoxHideEmptyPages.Text = "Hide Empty Pages";
                chkBoxHideEmptyPages.AutoPostBack = true;
                chkBoxHideEmptyPages.CheckedChanged += new EventHandler(chkBoxHideEmptyPages_CheckedChanged);

                this.Controls.Add(chkBoxHideEmptyPages);

                hdnShowEmptyPages = new HiddenField();
                hdnShowEmptyPages.ID = "hdnShowEmptyPages";
                this.Controls.Add(hdnShowEmptyPages);
                #endregion
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(strParentSiteURL, webEx);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strParentSiteURL, ex);
            }
        }


        /// <summary>
        /// Handles the CheckedChanged event of the chkBoxHideEmptyPages control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void chkBoxHideEmptyPages_CheckedChanged(object sender, EventArgs e)
        {

            try
            {

                if (chkBoxHideEmptyPages.Checked)
                {
                    hdnShowEmptyPages.Value = "false";
                }
                else
                {
                    hdnShowEmptyPages.Value = "true";
                }

                trvWellBookViewer.Nodes.Clear();
                LoadRootNodes(trvWellBookViewer, TreeNodeExpandMode.ServerSideCallBack);
                trvWellBookViewer.CheckAllNodes();
                trvWellBookViewer.TriStateCheckBoxes = true;
                trvWellBookViewer.CheckChildNodes = true;
                if (trvWellBookViewer.Nodes.Count > 0)
                {
                    trvWellBookViewer.ClearSelectedNodes();
                    trvWellBookViewer.Nodes[0].Selected = true;
                }
                SetBodyWebPartProperty(trvWellBookViewer.SelectedNode);
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(strParentSiteURL, webEx);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strParentSiteURL, ex);
            }
            finally
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), HIDETABJSKEY, HIDETABJSPAGE);
            }
        }

        
        #region DREAM 4.0 - eWB 2.0 - Cusomise Chapter
        /// To avoid the error due to RadListBox, instead of overriding Render method
        /// OnPreRender is overridden.
        /// Treenode is re-populated incase of Apply Click or Apply & Save click in Customise Chapter
        
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            if (this.Page.Request.Params[EVENTTARGET] != null)
            {
                if (this.Page.Request.Params[EVENTTARGET].ToLowerInvariant().Equals("customisechaptersinsession"))
                {
                    trvWellBookViewer.Nodes.Clear();
                    LoadRootNodes(trvWellBookViewer, TreeNodeExpandMode.ServerSideCallBack);
                    trvWellBookViewer.CheckAllNodes();
                    trvWellBookViewer.TriStateCheckBoxes = true;
                    trvWellBookViewer.CheckChildNodes = true;
                    if (trvWellBookViewer.Nodes.Count > 0)
                    {
                        trvWellBookViewer.ClearSelectedNodes();
                        trvWellBookViewer.Nodes[0].Selected = true;
                    }
                    SetBodyWebPartProperty(trvWellBookViewer.SelectedNode);
                }
                else if (this.Page.Request.Params[EVENTTARGET].ToLowerInvariant().Equals("customisechaptersforfuture"))
                {
                    trvWellBookViewer.Nodes.Clear();
                    LoadRootNodes(trvWellBookViewer, TreeNodeExpandMode.ServerSideCallBack);
                    trvWellBookViewer.CheckAllNodes();
                    trvWellBookViewer.TriStateCheckBoxes = true;
                    trvWellBookViewer.CheckChildNodes = true;
                    if (trvWellBookViewer.Nodes.Count > 0)
                    {
                        trvWellBookViewer.ClearSelectedNodes();
                        trvWellBookViewer.Nodes[0].Selected = true;
                    }
                    SetBodyWebPartProperty(trvWellBookViewer.SelectedNode);
                }
            }
            base.OnPreRender(e);
        }
        #endregion 
        ///// <summary>
        ///// Renders the control to the specified HTML writer.
        ///// </summary>
        ///// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the control content.</param>
        //protected override void Render(System.Web.UI.HtmlTextWriter writer)
        //{
        //  //  base.Render(writer);
        //    if (this.Page.Request.Params[EVENTTARGET] != null && this.Page.Request.Params[EVENTTARGET].ToLowerInvariant().Equals("customisechaptersinsession"))
        //    {
        //        trvWellBookViewer.Nodes.Clear();
        //        LoadRootNodes(trvWellBookViewer, TreeNodeExpandMode.ServerSideCallBack);
        //        trvWellBookViewer.CheckAllNodes();
        //        trvWellBookViewer.TriStateCheckBoxes = true;
        //        trvWellBookViewer.CheckChildNodes = true;
        //        if (trvWellBookViewer.Nodes.Count > 0)
        //        {
        //            trvWellBookViewer.ClearSelectedNodes();
        //            trvWellBookViewer.Nodes[0].Selected = true;
        //        }
        //        SetBodyWebPartProperty(trvWellBookViewer.SelectedNode);
        //    }

        //    hdnAssetValue.RenderControl(writer);
        //    hdnShowEmptyPages.RenderControl(writer);
        //    chkBoxHideEmptyPages.RenderControl(writer);
        //    pnlWellBookTreeView.RenderControl(writer);
        //}

        /// <summary>
        /// Handles the NodeExpand event of the treeView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.RadTreeNodeEventArgs"/> instance containing the event data.</param>
        void treeView_NodeExpand(object sender, RadTreeNodeEventArgs e)
        {
            try
            {
                PopulateNodeOnDemand(e);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strParentSiteURL, ex);
            }
        }

        /// <summary>
        /// Handles the NodeClick event of the treeView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.RadTreeNodeEventArgs"/> instance containing the event data.</param>
        void treeView_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            try
            {
                SetBodyWebPartProperty(e.Node);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strParentSiteURL, ex);
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the well book detail XML.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        private XmlDocument GetWellBookDetailXML(string action)
        {

            XmlDocument xmlWellBookDetails = null;
            string strBookID = string.Empty;
            string strChapterID = string.Empty;
            if (HttpContext.Current.Request.QueryString[QUERYSTRING_BOOKID] != null)
            {
                strBookID = HttpContext.Current.Request.QueryString[QUERYSTRING_BOOKID];
            }
            if (strBookID.Length > 0)
            {
                WellBookBLL objWellBookBLL = new WellBookBLL();
                PrintOptions objPrintOptions = new PrintOptions();
                objPrintOptions.IncludeBookTitle = false;
                objPrintOptions.IncludePageTitle = false;
                objPrintOptions.IncludeStoryBoard = false;

                BookInfo objBookInfo = objWellBookBLL.SetBookDetailDataObject(strParentSiteURL, strBookID, action, true, objPrintOptions);
                CommonBLL objCommonBLL = new CommonBLL();
                HttpContext.Current.Session[SESSION_TREEVIEWDATAOBJECT] = objBookInfo;
                if (HttpContext.Current.Request.QueryString[QUERYSTRING_IDVALUE] != null)
                {
                    strChapterID = HttpContext.Current.Request.QueryString[QUERYSTRING_IDVALUE];
                }
                if (HttpContext.Current.Request.QueryString[QUERYSTRING_CHAPTERID] != null)
                {
                    strChapterID = HttpContext.Current.Request.QueryString[QUERYSTRING_CHAPTERID];
                }
                xmlWellBookDetails = objCommonBLL.CreateWellBookDetailXML(objBookInfo);
            }
            return xmlWellBookDetails;
        }

        /// <summary>
        /// Loads the root nodes.
        /// </summary>
        /// <param name="objTreeView">The obj tree view.</param>
        /// <param name="objTreeNodeExpandMode">The obj tree node expand mode.</param>
        private void LoadRootNodes(RadTreeView objTreeView, TreeNodeExpandMode objTreeNodeExpandMode)
        {
            XmlDocument xmlWellBookDetails = new XmlDocument();
            if (Page.IsPostBack && HttpContext.Current.Session[SESSION_TREEVIEWXML] != null)
            {
                xmlWellBookDetails.LoadXml((string)HttpContext.Current.Session[SESSION_TREEVIEWXML]);
                if (HttpContext.Current.Request.QueryString[QUERYSTRINGSEARCHTYPE] != null)
                {
                    if (string.Equals(DWBSEARCHTYPE.ToLowerInvariant(), HttpContext.Current.Request.QueryString[QUERYSTRINGSEARCHTYPE].ToLowerInvariant()))
                    {
                        XmlNode xmlParentNode = xmlWellBookDetails.SelectSingleNode("/BookInfo");
                        if (Convert.ToInt32(xmlParentNode.Attributes[QUERYSTRING_BOOKID].Value) != Convert.ToInt32(HttpContext.Current.Request.QueryString[QUERYSTRING_BOOKID]))
                        {
                            xmlWellBookDetails = GetWellBookDetailXML("treeview");
                            HttpContext.Current.Session[SESSION_TREEVIEWXML] = xmlWellBookDetails.OuterXml;
                        }
                    }
                }
            }
            else
            {
                xmlWellBookDetails = GetWellBookDetailXML("treeview");
                HttpContext.Current.Session[SESSION_TREEVIEWXML] = xmlWellBookDetails.OuterXml;
            }
            if (xmlWellBookDetails != null)
            {
                XmlDocument xmlChapterPreference = LoadChapterPreference(xmlWellBookDetails, HttpContext.Current.Request.QueryString[QUERYSTRING_BOOKID]);

                XmlNode xmlParentNode = xmlWellBookDetails.SelectSingleNode("/BookInfo");

                RadTreeNode parentNode = new RadTreeNode();
                if (xmlParentNode != null)
                {
                    parentNode.Text = xmlParentNode.Attributes["BookName"].Value;
                    parentNode.Value = xmlParentNode.Attributes[QUERYSTRING_BOOKID].Value;
                }
                parentNode.ExpandMode = TreeNodeExpandMode.ClientSide;
                #region DREAM 4.1 - eWB 2.0 - Customise Chapters
                //objTreeView.Nodes.Clear();
                trvWellBookViewer.Nodes.Clear();
                #endregion
               // objTreeView.Nodes.Add(parentNode);
                trvWellBookViewer.Nodes.Add(parentNode);


                if (xmlChapterPreference != null)
                {
                    XmlNodeList nodeList = xmlChapterPreference.SelectNodes("/BookInfo/Chapter");
                    XmlNode chapterNode;
                    RadTreeNode childNode = null;
                    foreach (XmlNode node in nodeList)
                    {
                        if ( node.Attributes[QUERYSTRING_CHAPTERID].Value  != null && node.Attributes["Display"].Value.ToLowerInvariant().Equals("true"))
                        {
                            chapterNode = xmlWellBookDetails.SelectSingleNode("/BookInfo/Chapter[@ChapterID='" + node.Attributes[QUERYSTRING_CHAPTERID].Value+"']");
                            childNode = new RadTreeNode();
                            childNode.Text = chapterNode.Attributes["ChapterTitle"].Value;
                            childNode.Value = chapterNode.Attributes[QUERYSTRING_CHAPTERID].Value;
                            childNode.ExpandMode = objTreeNodeExpandMode;

                            parentNode.Nodes.Add(childNode);
                        }
                    }
                }
                else
                {

                    XmlNodeList nodeList = xmlWellBookDetails.SelectNodes("/BookInfo/Chapter");

                    RadTreeNode childNode = null;
                    foreach (XmlNode node in nodeList)
                    {
                        if (xmlChapterPreference != null && (xmlChapterPreference.SelectSingleNode("/BookInfo/Chapter[@ChapterID='" + node.Attributes[QUERYSTRING_CHAPTERID].Value + "']") != null) && (xmlChapterPreference.SelectSingleNode("/BookInfo/Chapter[@ChapterID='" + node.Attributes[QUERYSTRING_CHAPTERID].Value + "']").Attributes["Display"].Value.ToLowerInvariant().Equals("true")))
                        {
                            childNode = new RadTreeNode();
                            childNode.Text = node.Attributes["ChapterTitle"].Value;
                            childNode.Value = node.Attributes[QUERYSTRING_CHAPTERID].Value;
                            childNode.ExpandMode = objTreeNodeExpandMode;

                            parentNode.Nodes.Add(childNode);
                        }
                    }
                }
                parentNode.Expanded = true;
            }
        }

        /// <summary>
        /// Sets the body web part property.
        /// </summary>
        /// <param name="objRadTreeNode">The obj RAD tree node.</param>
        private void SetBodyWebPartProperty(RadTreeNode objRadTreeNode)
        {
            objTreeNodeValues = new TreeNodeSelection();
            if (objRadTreeNode != null)
            {
                if (objRadTreeNode.Level == 0)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), HIDETAB_JS_KEY, HIDETAB_JS);
                    objTreeNodeValues.IsBookSelected = true;
                    objTreeNodeValues.BookID = objRadTreeNode.Value;
                }
                else if (objRadTreeNode.Level == 1)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), HIDETAB_JS_KEY, HIDETAB_JS);
                    objTreeNodeValues.IsChapterSelected = true;
                    objTreeNodeValues.ChapterID = objRadTreeNode.Value;
                    objTreeNodeValues.BookID = objRadTreeNode.ParentNode.Value;
                }
                else
                {
                    if (string.Equals(objRadTreeNode.ToolTip, CONNECTION_TYPE3_PAGES))
                    {
                        objTreeNodeValues.IsTypeIIISelected = true;
                        objTreeNodeValues.ChapterID = objRadTreeNode.ParentNode.Value;
                        objTreeNodeValues.BookID = objRadTreeNode.ParentNode.ParentNode.Value;
                        objTreeNodeValues.PageID = objRadTreeNode.Value;
                    }
                    else if (string.Equals(objRadTreeNode.ToolTip, CONNECTION_TYPE2_PAGES))
                    {
                        objTreeNodeValues.IsTypeIISelected = true;
                        objTreeNodeValues.ChapterID = objRadTreeNode.ParentNode.Value;
                        objTreeNodeValues.BookID = objRadTreeNode.ParentNode.ParentNode.Value;
                        objTreeNodeValues.PageID = objRadTreeNode.Value;
                    }
                    else
                    {
                        objTreeNodeValues.IsTypeISelected = true;
                        objTreeNodeValues.ChapterID = objRadTreeNode.ParentNode.Value;
                        objTreeNodeValues.BookID = objRadTreeNode.ParentNode.ParentNode.Value;
                        objTreeNodeValues.PageID = objRadTreeNode.Value;
                        objTreeNodeValues.ReportName = objRadTreeNode.ToolTip;
                    }
                }
            }
            HttpContext.Current.Session[SESSION_WEBPARTPROPERTIES] = objTreeNodeValues;
        }

        /// <summary>
        /// Populates the node on demand.
        /// </summary>
        /// <param name="e">The <see cref="Telerik.Web.UI.RadTreeNodeEventArgs"/> instance containing the event data.</param>
        private void PopulateNodeOnDemand(RadTreeNodeEventArgs e)
        {
            XmlDocument xmlDocWellBookDetails = new XmlDocument();
            if (HttpContext.Current.Session[SESSION_TREEVIEWXML] != null)
            {
                xmlDocWellBookDetails.LoadXml((string)HttpContext.Current.Session[SESSION_TREEVIEWXML]);
            }
            else
            {
                xmlDocWellBookDetails = GetWellBookDetailXML("treeview");
            }

            if (xmlDocWellBookDetails != null)
            {
                #region DREAM 4.0 - eWB 2.0 - Hide/Reveal Empty Pages
                /// XPath query modified.
                XmlNodeList nodeList;// = xmlDocWellBookDetails.SelectNodes("/BookInfo/Chapter[@ChapterID='" + e.Node.Value + "']/PageInfo");
                if (!string.IsNullOrEmpty(hdnShowEmptyPages.Value))
                {
                    if (hdnShowEmptyPages.Value.ToLowerInvariant().Equals("true"))
                    {
                        nodeList = xmlDocWellBookDetails.SelectNodes("/BookInfo/Chapter[@ChapterID='" + e.Node.Value + "']/PageInfo");
                    }
                    else
                    {
                        nodeList = xmlDocWellBookDetails.SelectNodes("/BookInfo/Chapter[@ChapterID='" + e.Node.Value + "']/PageInfo[@IsEmpty='False']");
                    }
                }
                else
                {
                    nodeList = xmlDocWellBookDetails.SelectNodes("/BookInfo/Chapter[@ChapterID='" + e.Node.Value + "']/PageInfo");
                }
                #endregion

                RadTreeNode childNode = null;
                foreach (XmlNode node in nodeList)
                {
                    childNode = new RadTreeNode();
                    childNode.Text = node.Attributes["PageTitle"].Value;
                    childNode.Value = node.Attributes["PageID"].Value;
                    if (node.Attributes[CONNECTION_TYPE_COLUMN_NAME] != null)
                    {
                        /// Compare with Connection type instead of Page URL property
                        if (string.Equals(node.Attributes[CONNECTION_TYPE_COLUMN_NAME].Value, "3"))
                            childNode.ToolTip = CONNECTION_TYPE3_PAGES;
                        else if (string.Equals(node.Attributes[CONNECTION_TYPE_COLUMN_NAME].Value, "2"))
                            childNode.ToolTip = CONNECTION_TYPE2_PAGES;
                        else if (string.Equals(node.Attributes[CONNECTION_TYPE_COLUMN_NAME].Value, "1"))
                        {
                            if (node.Attributes[PAGE_URL_COLUMN_NAME] != null)
                            {
                                if (string.Equals(node.Attributes[PAGE_URL_COLUMN_NAME].Value, TYPE1_WELLHISTORY_PAGE_URL))
                                    childNode.ToolTip = TYPE1_REPORT_NAME_WELLHISTORY;
                                else if (string.Equals(node.Attributes[PAGE_URL_COLUMN_NAME].Value, TYPE1_WELLBORE_HEADER_PAGE_URL))
                                    childNode.ToolTip = TYPE1_REPORT_NAME_WELLBOREHEADER;
                                else if (string.Equals(node.Attributes[PAGE_URL_COLUMN_NAME].Value, TYPE1_PREPROD_RFT_PAGE_URL))
                                    childNode.ToolTip = TYPE1_REPORT_NAME_PREPROD_RFT;
                                else if (string.Equals(node.Attributes[PAGE_URL_COLUMN_NAME].Value, TYPE1_WELLSUMMARY_PAGE_URL))
                                    childNode.ToolTip = TYPE1_REPORT_NAME_WELLSUMMARY;
                            }
                        }
                    }

                    childNode.Checked = true;
                    e.Node.Nodes.Add(childNode);
                }
            }
            e.Node.Expanded = true;
        }

        /// <summary>
        /// Expands the chapter node.
        /// </summary>
        private void ExpandChapterNode()
        {
            string strChapterTitle = string.Empty;
            string strAssetValue = string.Empty;

            if (Page.IsPostBack && HttpContext.Current.Request.QueryString[QUERYSTRINGSEARCHTYPE] != null)
            {
                if (string.Equals(DWBSEARCHTYPE.ToLowerInvariant(), HttpContext.Current.Request.QueryString[QUERYSTRINGSEARCHTYPE].ToLowerInvariant()))
                {
                    hdnAssetValue = new HiddenField();
                    hdnAssetValue.ID = "hdnAssetValue";
                    this.Controls.Add(hdnAssetValue);

                    if (HttpContext.Current.Request.Form["hidAssetValue"] != null)
                        hdnAssetValue.Value = HttpContext.Current.Request.Form["hidAssetValue"].ToString();

                    if (hdnAssetValue != null && !string.IsNullOrEmpty(hdnAssetValue.Value))
                    {
                        strAssetValue = hdnAssetValue.Value;

                        /// Xpath to get ChapterID
                        XmlDocument xmlDocWellBook = new XmlDocument();
                        if (HttpContext.Current.Session[SESSION_TREEVIEWXML] != null)
                        {
                            xmlDocWellBook.LoadXml((string)HttpContext.Current.Session[SESSION_TREEVIEWXML]);

                            if (xmlDocWellBook != null)
                            {
                                XmlNodeList xmlChaptersNode = xmlDocWellBook.SelectNodes("/BookInfo/Chapter[@ActualAssetValue='" + strAssetValue + "']");
                                if (xmlChaptersNode != null && xmlChaptersNode.Count > 0)
                                {
                                    strChapterTitle = xmlChaptersNode[0].Attributes["ChapterTitle"].Value;

                                    if (trvWellBookViewer != null && trvWellBookViewer.FindNodeByText(strChapterTitle) != null)
                                    {
                                        trvWellBookViewer.ClearSelectedNodes();
                                        trvWellBookViewer.FindNodeByText(strChapterTitle).Selected = true;
                                        trvWellBookViewer.FindNodeByText(strChapterTitle).Expanded = true;
                                        trvWellBookViewer.FindNodeByText(strChapterTitle).ExpandMode = TreeNodeExpandMode.ClientSide;

                                        SetBodyWebPartProperty(trvWellBookViewer.FindNodeByText(strChapterTitle));
                                        PopulateNodeOnDemand(new RadTreeNodeEventArgs(trvWellBookViewer.FindNodeByText(strChapterTitle)));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        #region DREAM 4.0 - eWB 2.0 - Custmise Chaptres

        /// <summary>
        /// Loads the chapter preference.
        /// If the preference Xml for the logged in user and selected book is available in document library it loads from Doc List.
        /// Else create the fresh xml for the selected book and stores in Session.
        /// </summary>
        /// <param name="wellBookDetails">The well book details.</param>
        /// <param name="bookID">The book ID.</param>
        /// <returns>XmlDocument</returns>
        private XmlDocument LoadChapterPreference(XmlDocument wellBookDetails, string bookID)
        {
            XmlDocument chapterPreferenceXml=null;
            CommonUtility objCommonUtility = new CommonUtility();
            AbstractController objMOSSController;
            ServiceProvider objFactory;

            objFactory = new ServiceProvider();
            objMOSSController = objFactory.GetServiceManager(MOSSSERVICE);
            /// If any postback happens, the preference will be loaded from Session
            if (Page.IsPostBack && HttpContext.Current.Session[CHAPTERPREFERENCEXML] != null)
            {
                 string strChapterXml = (string)HttpContext.Current.Session[CHAPTERPREFERENCEXML];
                 if (!string.IsNullOrEmpty(strChapterXml))
                 {
                     chapterPreferenceXml = new XmlDocument();
                     chapterPreferenceXml.LoadXml(strChapterXml);
                 }
            }
            else 
            { /// Otherwise preferences will be loaded from Doc Lib or created newly
                if (((MOSSServiceManager)objMOSSController).IsDocLibFileExist(CHAPTERPREFERENCEDOCLIB, objCommonUtility.GetUserName()))
                {
                   XmlDocument  savedChapterPreferenceXml = ((MOSSServiceManager)objMOSSController).GetDocLibXMLFile(CHAPTERPREFERENCEDOCLIB, objCommonUtility.GetUserName());
                    string strXPath = "/Books/BookInfo[@BookID='" + HttpContext.Current.Request.QueryString[QUERYSTRING_BOOKID] +"']";

                    XmlNode bookInfoXml = savedChapterPreferenceXml.SelectSingleNode(strXPath);

                    if (bookInfoXml != null && bookInfoXml.ChildNodes.Count > 0)
                    {
                        chapterPreferenceXml = new XmlDocument();
                        chapterPreferenceXml.LoadXml(bookInfoXml.OuterXml);

                        /// Compare the User Preference with wellbookXML and add/remove the chapter nodes to the xml based on
                        /// If a chapter node available in preference xml but not in wellbookxml means either the chapter is deleted/terminated/contains no page so, not visible in treeview, remove such chapters from preference xml and save it back to Doc Lib.
                        /// If a chapter node not available in preference xml but available in wellbookxml means either the chapter is activated/added pages/newly added. Add such chapters with display=true to preference xml and the begining and save to Doc Lib.
                        AddRemoveChapterNodes(wellBookDetails, chapterPreferenceXml);
                    }
                    else
                    {
                        chapterPreferenceXml = CreateChapterListXml(wellBookDetails);
                    }
                }
                else
                {
                    chapterPreferenceXml = CreateChapterListXml(wellBookDetails);
                }
                if (chapterPreferenceXml != null)
                {
                    HttpContext.Current.Session.Remove(CHAPTERPREFERENCEXML);
                    HttpContext.Current.Session[CHAPTERPREFERENCEXML] = chapterPreferenceXml.OuterXml;
                }
            }
                     
            return chapterPreferenceXml;
        }

        /// <summary>
        /// Creates the chapter list XML incase of the preference xml 
        /// for the logged in user and selected book is not available in Doc Lib.
        /// </summary>
        /// <param name="wellBookDetails">The well book details.</param>
        /// <returns></returns>
        private XmlDocument CreateChapterListXml(XmlDocument wellBookDetails)
        {
            XmlDocument chapterListXml = new XmlDocument();
            CommonBLL objCommonBLL = new CommonBLL();
            if (wellBookDetails != null)
            {
                XmlNodeList xmlChapterNodes = wellBookDetails.SelectNodes("/BookInfo/Chapter");
                ChapterInfo objChapterInfo;
                BookInfo objBookInfo = new BookInfo();
                objBookInfo.BookName = wellBookDetails.SelectSingleNode("/BookInfo").Attributes["BookName"].Value;
                objBookInfo.BookID = wellBookDetails.SelectSingleNode("/BookInfo").Attributes["BookID"].Value;
                System.Collections.ArrayList arlChapters = new System.Collections.ArrayList();
                foreach (XmlNode xmlChapterNode in xmlChapterNodes)
                {
                    objChapterInfo = new ChapterInfo();
                    objChapterInfo.ChapterTitle = xmlChapterNode.Attributes["ChapterTitle"].Value;
                    objChapterInfo.ChapterID = xmlChapterNode.Attributes["ChapterID"].Value;
                    objChapterInfo.Display = true;
                    arlChapters.Add(objChapterInfo);
                }

                objBookInfo.Chapters = arlChapters;

                chapterListXml = objCommonBLL.CreateWellBookDetailXML(objBookInfo);
            }
            return chapterListXml;
        }

        /// <summary>
        /// Adds the remove chapter nodes to the preference xml saved in document library.
        /// </summary>
        /// <param name="wellBookDetails">The well book details.</param>
        /// <param name="chapterPreference">The chapter preference.</param>
        /// <returns>XmlDocument</returns>
        private XmlDocument AddRemoveChapterNodes(XmlDocument wellBookDetails, XmlDocument chapterPreference)
        {
           
            if (wellBookDetails != null && wellBookDetails.ChildNodes.Count > 0 && chapterPreference != null && chapterPreference.ChildNodes.Count > 0)
            {
                /// add/remove chapters present in wellbookdetails but not in chapterpreference.
                XmlNodeList chapterNodesInTree = wellBookDetails.SelectNodes("/BookInfo/Chapter");
                XmlNode chapterNodeInPreference = null;
                int intIndex = 1;
                foreach (XmlNode chapterNodeInTree in chapterNodesInTree)
                {
                   chapterNodeInPreference = chapterPreference.SelectSingleNode("/BookInfo/Chapter[@ChapterID = '" + chapterNodeInTree.Attributes["ChapterID"].Value + "']");

                   if (chapterNodeInPreference == null)
                   {
                       /// Insert at the beginning
                       XmlNode refChapterNode = chapterPreference.SelectSingleNode("/BookInfo/Chapter");
                       XmlElement newChapterElement = chapterPreference.CreateElement("Chapter");
                       XmlAttribute chapterIDAttribute = chapterPreference.CreateAttribute("ChapterID");
                       chapterIDAttribute.Value = chapterNodeInTree.Attributes["ChapterID"].Value;
                       XmlAttribute chapterTitle = chapterPreference.CreateAttribute("ChapterTitle");
                       chapterTitle.Value = chapterNodeInTree.Attributes["ChapterTitle"].Value;
                       XmlAttribute display = chapterPreference.CreateAttribute("Display");
                       display.Value = "True";
                       XmlAttribute chapterOrder = chapterPreference.CreateAttribute("ChapterOrder");
                       chapterOrder.Value = intIndex.ToString();
                       intIndex++;
                       newChapterElement.Attributes.Append(chapterIDAttribute);
                       newChapterElement.Attributes.Append(chapterTitle);
                       newChapterElement.Attributes.Append(display);
                       newChapterElement.Attributes.Append(chapterOrder);
                       chapterPreference.SelectSingleNode("/BookInfo").InsertBefore(newChapterElement, refChapterNode);
                   }                   
                }
                /// remove chapters in chapterpreference but not in wellbookdetails.
                XmlNodeList chapterNodesInPreference = chapterPreference.SelectNodes("/BookInfo/Chapter");
                XmlNode newChapterNodeInTree = null;
                foreach (XmlNode chapterNode in chapterNodesInPreference)
                {
                    newChapterNodeInTree = wellBookDetails.SelectSingleNode("/BookInfo/Chapter[@ChapterID = '" + chapterNode.Attributes["ChapterID"].Value + "']");
                    if (newChapterNodeInTree == null)
                    {
                        /// Remove the node from preferencexml.
                        chapterPreference.SelectSingleNode("/BookInfo").RemoveChild(chapterNode);                        
                    }
                }

            }

            ChapterBLL objChapterBLL = new ChapterBLL();
            objChapterBLL.SaveReorderXml(chapterPreference.OuterXml, HttpContext.Current.Request.QueryString[QUERYSTRING_BOOKID]);
            return chapterPreference;
        }
        #endregion
        #endregion
    }
}

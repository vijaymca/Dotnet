#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: AssetTreeHelper
#endregion
/// <summary> 
/// This is asset tree helper class.
/// </summary>
using System;
using System.Collections;
using System.Data;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Web.Services.Protocols;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing.Navigation;
using Telerik.Web.UI;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DREAM.Business.Entities;

namespace Shell.SharePoint.DREAM.Site.UI
{
    /// <summary>
    /// AssetTreeHelper class contains all method releated to asset tree functionality.
    /// </summary>
    public class AssetTreeHelper :UIControlHandler
    {
        #region DECLARATION
        const string CONTEXTSEARCHDOCLIB = "Context Search XML";
        #region Variable
        int intNoOfPageLinkToDisplay = 5;
        int intNodePerPage = 5;
        int intSkipCount;
        int intMaxRecord = 100;
        XmlDocument xmlDocAssetTreeConfig;
        XmlNode xmlNodeCurrentLevel;
        RadTreeView trvAssetTree;
        #endregion
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetTreeHelper"/> class.
        /// </summary>
        /// <param name="assetTree">The asset tree.</param>
        public AssetTreeHelper(RadTreeView assetTree)
        {
            strSiteURL = SPContext.Current.Site.Url;
            objMossController = objFactory.GetServiceManager(MOSSSERVICE);
            trvAssetTree = assetTree;
            xmlDocAssetTreeConfig = GetAssetTreeConfiguration();
        }
        #endregion

        #region Public method

        /// <summary>
        /// Gets the site URL.
        /// </summary>
        /// <returns></returns>
        public string GetSiteURL()
        {
            return strSiteURL;
        }
        /// <summary>
        /// Populates the nodes.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="currentPageNumber">The current page number.</param>
        /// <returns></returns>
        public int PopulateRootNodes(string searchText, int currentPageNumber)
        {
            int intNodeCount = 0;
            xmlNodeCurrentLevel = GetCurrentLevelNode(1);
            //adding root searchbox
            AddRootSearchBox(trvAssetTree.Nodes);
            //
            if(!string.IsNullOrEmpty(GetNodeAttributeValue(xmlNodeCurrentLevel, AssetTreeConstants.LISTNAME)))
            {
                intNodeCount = PopulateNodesFromSPList(true, null, searchText, currentPageNumber);
            }
            else
            {
                intNodeCount = PopulateNodesFromWebservice(true, null, searchText, currentPageNumber);
            }
            return intNodeCount;
        }
        /// <summary>
        /// Populates the child nodes.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="searchText">The search text.</param>
        /// <param name="currentPageNumber">The current page number.</param>
        /// <returns></returns>
        public int PopulateChildNodes(RadTreeNode node, string searchText, int currentPageNumber)
        {
            int intNodeCount = 0;
            xmlNodeCurrentLevel = GetCurrentLevelNode(node.Level + 2);
            if(!string.IsNullOrEmpty(GetNodeAttributeValue(xmlNodeCurrentLevel, AssetTreeConstants.LISTNAME)))
            {
                intNodeCount = PopulateNodesFromSPList(false, node, searchText, currentPageNumber);
            }
            else
            {
                intNodeCount = PopulateNodesFromWebservice(false, node, searchText, currentPageNumber);
            }
            return intNodeCount;
        }

        /// <summary>
        /// Gets the page count.
        /// </summary>
        /// <param name="nodeCount">The node count.</param>
        /// <param name="nodeLevel">The node level.</param>
        /// <returns></returns>
        public int GetPageCount(int nodeCount, int nodeLevel)
        {
            xmlNodeCurrentLevel = GetCurrentLevelNode(nodeLevel);
            intNodePerPage = Convert.ToInt32(GetNodeAttributeValue(xmlNodeCurrentLevel, AssetTreeConstants.NODESPERPAGE));
            int intPageCount = 0;
            intPageCount = (int)Math.Ceiling((double)nodeCount / (double)intNodePerPage);
            return intPageCount;
        }
        /// <summary>
        /// Gets the page count.
        /// </summary>
        /// <param name="nodeCount">The node count.</param>
        /// <returns></returns>
        private int GetPageCount(int nodeCount)
        {
            intNodePerPage = Convert.ToInt32(GetNodeAttributeValue(xmlNodeCurrentLevel, AssetTreeConstants.NODESPERPAGE));
            int intPageCount = 0;
            intPageCount = (int)Math.Ceiling((double)nodeCount / (double)intNodePerPage);
            return intPageCount;
        }

        #region ContextSearch creation

        /// <summary>
        /// Creates the context menu on the basis of asset type
        /// </summary>
        public void CreateContextMenu()
        {
            XmlNode xmlNodeAssetTree = GetCurrentLevelNode(0);
            XmlNode xmlNodeFirstChild = xmlNodeAssetTree.FirstChild.FirstChild;
            string strAsset = string.Empty;
            RadTreeViewContextMenu contextMenu = null;
            while(xmlNodeFirstChild != null)
            {
                strAsset = GetNodeAttributeValue(xmlNodeFirstChild, AssetTreeConstants.ASSETNAME);
                contextMenu = new RadTreeViewContextMenu();
                contextMenu.ID = AssetTreeConstants.CONTEXTMENUIDPREFIX + strAsset;
                LoadContextSearchProfile(strAsset, contextMenu);
                trvAssetTree.ContextMenus.Add(contextMenu);
                xmlNodeFirstChild = xmlNodeFirstChild.FirstChild;
            }
        }

        /// <summary>
        /// Creates the context search menu from SP list.
        /// </summary>
        /// <param name="strAsset">The STR asset.</param>
        /// <param name="contextMenu">The context menu.</param>
        private void CreateContextSearchMenuFromSPList(string strAsset, RadTreeViewContextMenu contextMenu)
        {
            ArrayList arrDistinctGrpHdr = new ArrayList();
            string strCamlQueryGroupHdr = string.Empty;
            string strCamlQueryContextItem = string.Empty;
            string strGrpHdr = string.Empty;
            RadMenuItem menuItemGrpHdr = null;
            SiteMapNodeCollection siteMapNodeColContextLink = null;
            strCamlQueryGroupHdr = "<Where><And><Eq><FieldRef Name=\"Asset_x0020_Type\" /><Value Type=\"Lookup\">" + strAsset + "</Value></Eq><Eq><FieldRef Name=\"Active\" /><Value Type=\"Boolean\">1</Value></Eq></And></Where><OrderBy><FieldRef Name='HeaderOrder' Ascending='True' /><FieldRef Name='Group_x0020_Header' Ascending='True' /></OrderBy>";
            SiteMapNodeCollection siteMapNodeColGroupHeader = ((MOSSServiceManager)objMossController).GetChachedListItems(strSiteURL, AssetTreeConstants.CONTEXTSEARCHLIST, strCamlQueryGroupHdr);
            foreach(PortalListItemSiteMapNode siteMapNodeGroupHeader in siteMapNodeColGroupHeader)
            {
                strGrpHdr = (string)siteMapNodeGroupHeader[AssetTreeConstants.GROUPHEADERCOLNAME];
                if(arrDistinctGrpHdr.Contains(strGrpHdr))
                {
                    continue;
                }
                else
                {
                    arrDistinctGrpHdr.Add(strGrpHdr);
                }
                menuItemGrpHdr = new RadMenuItem();
                menuItemGrpHdr.Text = strGrpHdr;
                menuItemGrpHdr.Value = AssetTreeConstants.MENUHEADERVALUE;
                contextMenu.Items.Add(menuItemGrpHdr);
                strCamlQueryContextItem = "<Where><And><Eq><FieldRef Name=\"Asset_x0020_Type\" /><Value Type=\"Lookup\">" + strAsset + "</Value></Eq><And><Eq><FieldRef Name=\"Group_x0020_Header\" /><Value Type=\"Choice\">" + strGrpHdr + "</Value></Eq><Eq><FieldRef Name=\"Active\" /><Value Type=\"Boolean\">1</Value></Eq></And></And></Where><OrderBy><FieldRef Name=\"LinkOrder\" Ascending=\"True\" /><FieldRef Name=\"Title\" Ascending=\"True\" /></OrderBy>";
                siteMapNodeColContextLink = ((MOSSServiceManager)objMossController).GetChachedListItems(strSiteURL, AssetTreeConstants.CONTEXTSEARCHLIST, strCamlQueryContextItem);
                foreach(PortalListItemSiteMapNode siteMapNodeContextLink in siteMapNodeColContextLink)
                {
                    CreateContextSearchMenuItem(strAsset, menuItemGrpHdr, siteMapNodeContextLink);
                }
                //adding EPCatalog report link
                if(strGrpHdr.ToLowerInvariant().Equals("documents"))
                {
                    CreatePVTReportMenuItem(strAsset, menuItemGrpHdr);
                }
            }
        }

        /// <summary>
        /// Creates the PVT report menu item.
        /// </summary>
        /// <param name="strAsset">The STR asset.</param>
        /// <param name="menuItemGrpHdr">The menu item GRP HDR.</param>
        private void CreatePVTReportMenuItem(string strAsset, RadMenuItem menuItemGrpHdr)
        {
            string strCamlQueryPVTItem = "<OrderBy><FieldRef Name=\"Title\" /></OrderBy><Where><Eq><FieldRef Name=\"Active\" /><Value Type=\"Boolean\">1</Value></Eq></Where>";
            SiteMapNodeCollection siteMapNodeColPVTLink = ((MOSSServiceManager)objMossController).GetChachedListItems(strSiteURL, AssetTreeConstants.PVTREPORTLIST, strCamlQueryPVTItem);
            RadMenuItem menuItemContextLink = null;
            foreach(PortalListItemSiteMapNode siteMapNodePVTtLink in siteMapNodeColPVTLink)
            {
                menuItemContextLink = new RadMenuItem();
                menuItemContextLink.Text = (string)siteMapNodePVTtLink["PVT"];
                menuItemContextLink.Value = "/pages/PVTReport.aspx";
                menuItemContextLink.Attributes.Add("asset", strAsset);
                menuItemContextLink.Attributes.Add("linkValue", "PVTReport");
                menuItemGrpHdr.Items.Add(menuItemContextLink);
            }
        }

        /// <summary>
        /// Creates the context search menu item.
        /// </summary>
        /// <param name="strAsset">The STR asset.</param>
        /// <param name="menuItemGrpHdr">The menu item GRP HDR.</param>
        /// <param name="siteMapNodeContextLink">The site map node context link.</param>
        private void CreateContextSearchMenuItem(string strAsset, RadMenuItem menuItemGrpHdr, PortalListItemSiteMapNode siteMapNodeContextLink)
        {
            RadMenuItem menuItemContextLink = new RadMenuItem();
            menuItemContextLink.Text = (string)siteMapNodeContextLink[AssetTreeConstants.TITLE];
            menuItemContextLink.Value = ((string)siteMapNodeContextLink[AssetTreeConstants.PAGEURLCOLNAME]);
            menuItemContextLink.Attributes.Add("asset", strAsset);
            menuItemContextLink.Attributes.Add("linkValue", (string)siteMapNodeContextLink["Value"]);
            menuItemGrpHdr.Items.Add(menuItemContextLink);
        }
        #region Context Search from User Profile
        /// <summary>
        /// Loads the context search profile.
        /// </summary>
        /// <param name="asset">The asset.</param>
        /// <param name="contextMenu">The context menu.</param>
        private void LoadContextSearchProfile(string asset, RadTreeViewContextMenu contextMenu)
        {
            XmlDocument xmlDocContextSearch = GetContextSearchProfile();
            if(xmlDocContextSearch != null)//role context search xml does exist
            {
                if(xmlDocContextSearch.SelectSingleNode("contextsearches/groupheaders[@asset = '" + asset + "']") != null)//Context search profile saved for current asset
                {
                    RenderContextSearchFromUserProfile(xmlDocContextSearch, asset, contextMenu);
                }
                else//Context search profile not saved for current asset
                {
                    //get default order for all group headers and context links fron context search list
                    CreateContextSearchMenuFromSPList(asset, contextMenu);
                }
            }
            else//user role context search xml does not exist
            {
                //get default order for all group headers and context links fron context search list
                CreateContextSearchMenuFromSPList(asset, contextMenu);
            }
        }
        /// <summary>
        /// Gets the profile group headers.
        /// </summary>
        /// <param name="contextSearchProfile">The context search profile.</param>
        /// <param name="asset">The asset.</param>
        /// <returns></returns>
        private ArrayList GetProfileGroupHeaders(XmlDocument contextSearchProfile, string asset)
        {
            XmlNode xmlNodeGroupHeaders = contextSearchProfile.SelectSingleNode("contextsearches/groupheaders[@asset = '" + asset + "']");
            ArrayList arrGroupHeaders = new ArrayList();
            XmlNode xmlNodeGroupHeader = null;
            for(int intIndex = 0; intIndex < xmlNodeGroupHeaders.ChildNodes.Count; intIndex++)
            {
                xmlNodeGroupHeader = xmlNodeGroupHeaders.SelectSingleNode("groupheader[@order = '" + (intIndex + 1) + "']");
                if(xmlNodeGroupHeader != null)
                {
                    arrGroupHeaders.Add(xmlNodeGroupHeader.Attributes["name"].Value);
                }
            }
            return arrGroupHeaders;
        }

        /// <summary>
        /// Renders the context search from user profile.
        /// </summary>
        /// <param name="contextSearchProfile">The context search profile.</param>
        /// <param name="assetType">Type of the asset.</param>
        /// <param name="contextMenu">The context menu.</param>
        private void RenderContextSearchFromUserProfile(XmlDocument contextSearchProfile, string assetType, RadTreeViewContextMenu contextMenu)
        {
            //All group headers and their order is taken fron user role contextsearch profile
            //loop for group header starts
            //if (true)//context search links are saved for current group header
            //{
            //    //only populate context search links saved in profile as display true in their saved order
            //}
            //else//context search links are not saved for current group header
            //{
            //    //get all the context search list for current asset and current group header from context search list
            //}
            //ends
            SiteMapNodeCollection contextSearchItemCol = null;
            ArrayList arlGroupHeaders = null;
            RadMenuItem menuItemGrpHdr = null;
            XmlNodeList xmlNodeLstContextSearch = null;
            string strCamlQuery = string.Empty;
            arlGroupHeaders = GetProfileGroupHeaders(contextSearchProfile, assetType);
            XmlNode xmlNodeGroupHeaders = contextSearchProfile.SelectSingleNode("contextsearches/groupheaders[@asset = '" + assetType + "']");
            if(arlGroupHeaders != null && arlGroupHeaders.Count > 0)
            {
                foreach(string strGroupHeader in arlGroupHeaders)
                {
                    menuItemGrpHdr = new RadMenuItem();
                    menuItemGrpHdr.Text = strGroupHeader;
                    menuItemGrpHdr.Value = AssetTreeConstants.MENUHEADERVALUE;
                    contextMenu.Items.Add(menuItemGrpHdr);
                    xmlNodeLstContextSearch = xmlNodeGroupHeaders.SelectNodes("groupheader[@name = '" + strGroupHeader + "']/contextsearch[@display = 'true']");
                    if(xmlNodeLstContextSearch != null && xmlNodeLstContextSearch.Count > 0)//context search links are saved for current group header
                    {

                        //only populate context search links saved in profile as display true in their saved order
                        foreach(XmlNode node in xmlNodeLstContextSearch)
                        {
                            strCamlQuery = "<Where><And><Eq><FieldRef Name=\"Asset_x0020_Type\" /><Value Type=\"Lookup\">" + assetType + "</Value></Eq><And><Eq><FieldRef Name=\"Active\" /><Value Type=\"Boolean\">1</Value></Eq><And><Eq><FieldRef Name=\"Group_x0020_Header\" /><Value Type=\"Choice\">" + strGroupHeader + "</Value></Eq><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">" + node.Attributes["name"].Value + "</Value></Eq></And></And></And></Where>";

                            contextSearchItemCol = ((MOSSServiceManager)objMossController).GetChachedListItems(strSiteURL, AssetTreeConstants.CONTEXTSEARCHLIST, strCamlQuery);
                            if(contextSearchItemCol != null && contextSearchItemCol.Count > 0)
                            {
                                CreateContextSearchMenuItem(assetType, menuItemGrpHdr, (PortalListItemSiteMapNode)contextSearchItemCol[0]);
                            }
                        }
                        //adding EPCatalog  report options in context search
                        if(strGroupHeader.ToLower().Equals("documents"))
                        {
                            CreatePVTReportMenuItem(assetType, menuItemGrpHdr);
                        }
                        //
                    }
                    else
                    {
                        strCamlQuery = "<Where><And><Eq><FieldRef Name=\"Asset_x0020_Type\" /><Value Type=\"Lookup\">" + assetType + "</Value></Eq><And><Eq><FieldRef Name=\"Group_x0020_Header\" /><Value Type=\"Choice\">" + strGroupHeader + "</Value></Eq><Eq><FieldRef Name=\"Active\" /><Value Type=\"Boolean\">1</Value></Eq></And></And></Where><OrderBy><FieldRef Name=\"LinkOrder\" Ascending=\"True\" /><FieldRef Name=\"Title\" Ascending=\"True\" /></OrderBy>";
                        contextSearchItemCol = ((MOSSServiceManager)objMossController).GetChachedListItems(strSiteURL, AssetTreeConstants.CONTEXTSEARCHLIST, strCamlQuery);
                        if((contextSearchItemCol != null) && (contextSearchItemCol.Count > 0))
                        {
                            foreach(PortalListItemSiteMapNode listItem in contextSearchItemCol)
                            {
                                CreateContextSearchMenuItem(assetType, menuItemGrpHdr, listItem);
                            }
                            //adding EPCatalog report options in context search
                            if(strGroupHeader.ToLower().Equals("documents"))
                            {
                                CreatePVTReportMenuItem(assetType, menuItemGrpHdr);
                            }
                            //
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Gets the context search profile.
        /// </summary>
        /// <returns></returns>
        private XmlDocument GetContextSearchProfile()
        {
            XmlDocument xmlDocContextSearch = null;
            string strUserRole = objUtility.GetUserRole();
            if(((MOSSServiceManager)objMossController).IsDocLibFileExist(CONTEXTSEARCHDOCLIB, strUserRole))
            {
                xmlDocContextSearch = ((MOSSServiceManager)objMossController).GetDocLibXMLFile(CONTEXTSEARCHDOCLIB, strUserRole);
            }
            return xmlDocContextSearch;
        }
        #endregion
        #endregion

        #endregion

        #region Private Method

        #region Node population method

        /// <summary>
        /// Populates the nodes from SP list.
        /// </summary>
        /// <param name="isRootNode">if set to <c>true</c> [is root node].</param>
        /// <param name="node">The node.</param>
        /// <param name="searchText">The search text.</param>
        /// <param name="currentPageNumber">The current page number.</param>
        /// <returns></returns>
        private int PopulateNodesFromSPList(bool isRootNode, RadTreeNode node, string searchText, int currentPageNumber)
        {
            DataRow dtRow = null;
            int intRecordStarIndex = 0;
            int intRecordEndIndex = 0;
            int intStartPageNumber = 0;
            int intEndPageNumber = 0;
            int intNodeCount = 0;
            int intPageCount = 0;
            string strListName = GetNodeAttributeValue(xmlNodeCurrentLevel, AssetTreeConstants.LISTNAME);
            string strTextColName = GetNodeAttributeValue(xmlNodeCurrentLevel, AssetTreeConstants.TEXTATTRIBUTE);
            string strValueColName = GetNodeAttributeValue(xmlNodeCurrentLevel, AssetTreeConstants.VALUEATTRIBUTE);
            string strCamlQuery = "<OrderBy><FieldRef Name=\"" + strTextColName + "\"/></OrderBy><Where><Eq>" + "<FieldRef Name=\"Active\" /><Value Type=\"Choice\">Yes</Value></Eq></Where>";
            try
            {
                dtListValues.Reset();
                dtListValues = ((MOSSServiceManager)objMossController).ReadList(strSiteURL, strListName, strCamlQuery);
                if(dtListValues != null && dtListValues.Rows.Count > 0)
                {
                    DataRow[] dataRow = null;
                    if(!string.IsNullOrEmpty(searchText))
                    {
                        try
                        {
                            dataRow = dtListValues.Select(strTextColName + " LIKE '" + searchText + "'");
                        }
                        catch(EvaluateException)
                        {
                            dataRow = null; //catching invalid search expression.
                        }
                    }
                    else
                    {
                        dataRow = dtListValues.Select();
                    }
                    if(dataRow != null && dataRow.Length > 0)
                    {
                        //paging
                        intNodePerPage = Convert.ToInt32(GetNodeAttributeValue(xmlNodeCurrentLevel, AssetTreeConstants.NODESPERPAGE));

                        intNodeCount = dataRow.Length;
                        ;
                        intPageCount = GetPageCount(intNodeCount);
                        intStartPageNumber = Math.Min(Math.Max(0, currentPageNumber - (intNoOfPageLinkToDisplay / 2)), Math.Max(0, intPageCount - intNoOfPageLinkToDisplay + 1));
                        intEndPageNumber = Math.Min(intPageCount, intStartPageNumber + intNoOfPageLinkToDisplay);
                        intRecordStarIndex = currentPageNumber * intNodePerPage - intNodePerPage;
                        intRecordEndIndex = currentPageNumber * intNodePerPage;
                        //
                        for(int intIndex = intRecordStarIndex; intIndex < intRecordEndIndex; intIndex++)
                        {

                            if(intIndex < intNodeCount)
                            {

                                dtRow = dataRow[intIndex];
                                if(dtRow == null)
                                {
                                    continue;
                                }
                                string strNodeText = (string)dtRow[strTextColName];
                                string strNodeValue = (string)dtRow[strValueColName];
                                if((!string.IsNullOrEmpty(strNodeText)) && (!string.IsNullOrEmpty(strNodeValue)))
                                {
                                    RadTreeNode treeNodeNew = new RadTreeNode();
                                    treeNodeNew.Text = GetSearchBoxHTML(strNodeText, strNodeValue, GetNodeAttributeValue(xmlNodeCurrentLevel.FirstChild, AssetTreeConstants.ASSETNAME));
                                    treeNodeNew.Value = strNodeValue;
                                    treeNodeNew.Attributes.Add(AssetTreeConstants.SEARCHTYPE, GetNodeAttributeValue(xmlNodeCurrentLevel, AssetTreeConstants.SEARCHNAME));
                                    treeNodeNew.Attributes.Add(AssetTreeConstants.SELECTEDCRITERIA, GetNodeAttributeValue(xmlNodeCurrentLevel, AssetTreeConstants.VALUEATTRIBUTE));
                                    treeNodeNew.ExpandMode = TreeNodeExpandMode.ServerSide;
                                    treeNodeNew.EnableContextMenu = false;
                                    treeNodeNew.Checkable = false;
                                    treeNodeNew.ContextMenuID = GetContextMenuID(GetNodeAttributeValue(xmlNodeCurrentLevel, AssetTreeConstants.ASSETNAME));
                                    if(isRootNode)
                                    {
                                        trvAssetTree.Nodes.Add(treeNodeNew);
                                    }
                                    else
                                    {
                                        node.Nodes.Add(treeNodeNew);
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        if(intPageCount > 1)
                        {
                            if(isRootNode)
                            {
                                AddLastNode(trvAssetTree.Nodes, GetPagingNodeText(intStartPageNumber, intEndPageNumber, currentPageNumber, intPageCount), AssetTreeConstants.TREEVIEWPAGING);
                            }
                            else
                            {
                                AddLastNode(node, GetPagingNodeText(intStartPageNumber, intEndPageNumber, currentPageNumber, intPageCount), AssetTreeConstants.TREEVIEWPAGING);
                            }
                        }
                    }
                    else
                    {
                        if(isRootNode)
                        {
                            AddLastNode(trvAssetTree.Nodes, AssetTreeConstants.NORECORDSMESSAGE, AssetTreeConstants.ERROR);
                        }
                        else
                        {
                            AddLastNode(node, AssetTreeConstants.NORECORDSMESSAGE, AssetTreeConstants.ERROR);
                        }
                    }
                }
            }
            finally
            {
                if(dtListValues != null)
                {
                    dtListValues.Dispose();
                }
            }
            return intNodeCount;
        }
        /// <summary>
        /// Populates the nodes from webservice.
        /// </summary>
        /// <param name="isRootNode">if set to <c>true</c> [is root node].</param>
        /// <param name="node">The node.</param>
        /// <param name="searchText">The search text.</param>
        /// <param name="currentPageNumber">The current page number.</param>
        /// <returns></returns>
        private int PopulateNodesFromWebservice(bool isRootNode, RadTreeNode node, string searchText, int currentPageNumber)
        {
            #region Declearation
            string strValueAttributeXPath = string.Empty;
            string strTextAttributeXPath = string.Empty;
            string strChildNodesXPath = string.Empty;
            string strNodeValue = string.Empty;
            int intNodeCount = 0;
            int intRecordStarIndex = 0;
            int intRecordEndIndex = 0;
            int intNodeDepth = 0;
            int intStartPageNumber = 0;
            int intEndPageNumber = 0;
            int intPageCount = 0;
            int intNodeLevel = 0;
            XmlNode xmlNodeReport = null;
            XmlDocument xmlDocResponse = null;
            #endregion
            try
            {
                //seetings values for root or child nodes
                if(isRootNode)
                {
                    intNodeLevel = 0;
                    strNodeValue = string.Empty;
                }
                else
                {
                    intNodeLevel = node.Level;
                    strNodeValue = node.Value;
                }
                //
                intNodePerPage = Convert.ToInt32(GetNodeAttributeValue(xmlNodeCurrentLevel, AssetTreeConstants.NODESPERPAGE));
                intSkipCount = (currentPageNumber - 1) * intNodePerPage;
                xmlDocResponse = GetResponseXML(strNodeValue, searchText);
                strValueAttributeXPath = "child::*[@name = '" + GetNodeAttributeValue(xmlNodeCurrentLevel, AssetTreeConstants.VALUEATTRIBUTE) + "']";
                strTextAttributeXPath = "child::*[@name = '" + GetNodeAttributeValue(xmlNodeCurrentLevel, AssetTreeConstants.TEXTATTRIBUTE) + "']";
                #region Populating nodes
                if(xmlDocResponse != null)
                {
                    if(Int32.TryParse(xmlDocResponse.SelectSingleNode(AssetTreeConstants.RECORDCOUNTNODEXPATH).InnerXml, out intNodeCount))
                    {
                        intNodeCount = intSkipCount + intNodeCount;
                        intPageCount = GetPageCount(intNodeCount);
                    }
                    else
                    {
                        intPageCount = 0;
                    }
                    intStartPageNumber = Math.Min(Math.Max(0, currentPageNumber - (intNoOfPageLinkToDisplay / 2)), Math.Max(0, intPageCount - intNoOfPageLinkToDisplay + 1));
                    intEndPageNumber = Math.Min(intPageCount, intStartPageNumber + intNoOfPageLinkToDisplay);
                    xmlNodeReport = xmlDocResponse.SelectSingleNode(AssetTreeConstants.REPORTNODEXPATH);
                    if(xmlNodeReport != null)
                    {
                        // intRecordStarIndex = currentPageNumber * intNodePerPage - intNodePerPage + 1;
                        // intRecordEndIndex = currentPageNumber * intNodePerPage;
                        intRecordStarIndex = 1;
                        intRecordEndIndex = intNodePerPage;
                        strChildNodesXPath = "child::record[(position() >= " + intRecordStarIndex + ") and (position() <= " + intRecordEndIndex + ")]";
                        intNodeDepth = GetAssetTreeDepth();
                        XmlNodeList ndList = xmlNodeReport.SelectNodes(strChildNodesXPath);
                        for(int intNodeCounter = 0; intNodeCounter < ndList.Count; intNodeCounter++)
                        {
                            RadTreeNode newNode = new RadTreeNode();
                            if((intNodeDepth - 2) == intNodeLevel)
                            {
                                newNode.Text = ndList[intNodeCounter].SelectSingleNode(strTextAttributeXPath).Attributes[AssetTreeConstants.VALUE].Value;
                                newNode.ExpandMode = TreeNodeExpandMode.ClientSide;
                                newNode.Attributes.Add(AssetTreeConstants.LEAFNODE, AssetTreeConstants.LEAFNODE);
                            }
                            else
                            {
                                newNode.Text = GetSearchBoxHTML(ndList[intNodeCounter].SelectSingleNode(strTextAttributeXPath).Attributes[AssetTreeConstants.VALUE].Value,
                                    ndList[intNodeCounter].SelectSingleNode(strValueAttributeXPath).Attributes[AssetTreeConstants.VALUE].Value,
                                    GetNodeAttributeValue(xmlNodeCurrentLevel.FirstChild, AssetTreeConstants.ASSETNAME));
                                newNode.ExpandMode = TreeNodeExpandMode.ServerSide;
                            }
                            newNode.Value = ndList[intNodeCounter].SelectSingleNode(strValueAttributeXPath).Attributes[AssetTreeConstants.VALUE].Value;
                            newNode.Attributes.Add(AssetTreeConstants.SEARCHTYPE, GetNodeAttributeValue(xmlNodeCurrentLevel, AssetTreeConstants.SEARCHNAME));
                            newNode.Attributes.Add(AssetTreeConstants.SELECTEDCRITERIA, GetNodeAttributeValue(xmlNodeCurrentLevel, AssetTreeConstants.VALUEATTRIBUTE));
                            newNode.ContextMenuID = GetContextMenuID(GetNodeAttributeValue(xmlNodeCurrentLevel, AssetTreeConstants.ASSETNAME));

                            if(isRootNode)
                            {
                                trvAssetTree.Nodes.Add(newNode);
                            }
                            else
                            {
                                node.Nodes.Add(newNode);
                            }
                        }
                    }
                    if(intPageCount > 1)
                    {
                        if(isRootNode)
                        {
                            AddLastNode(trvAssetTree.Nodes, GetPagingNodeText(intStartPageNumber, intEndPageNumber, currentPageNumber, intPageCount), AssetTreeConstants.TREEVIEWPAGING);
                        }
                        else
                        {
                            AddLastNode(node, GetPagingNodeText(intStartPageNumber, intEndPageNumber, currentPageNumber, intPageCount), AssetTreeConstants.TREEVIEWPAGING);
                        }
                    }
                }
                #endregion
            }
            catch(SoapException soapEx)
            {
                HandleSoapException(soapEx, isRootNode, node);
            }
            catch(WebException webEx)
            {
                HandleWebException(webEx, isRootNode, node);
            }
            return intNodeCount;
        }

        #endregion

        #region Request Generation Methods
        /// <summary>
        /// Gets the response XML.
        /// </summary>
        /// <param name="nodeValue">The node value.</param>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        private XmlDocument GetResponseXML(string nodeValue, string searchText)
        {
            XmlDocument xmlDocSearchResult = null;
            string strSearchType = GetNodeAttributeValue(xmlNodeCurrentLevel, "searchName");
            string strSortColumn = GetNodeAttributeValue(xmlNodeCurrentLevel, "searchTextColName");
            objReportController = objFactory.GetServiceManager(REPORTSERVICE);
            objRequestInfo = CreateRequestInfo(nodeValue, searchText);
            xmlDocSearchResult = objReportController.GetSearchResults(objRequestInfo, intMaxRecord, strSearchType, strSortColumn, 0);
            return xmlDocSearchResult;
        }
        /// <summary>
        /// Creates the request info.
        /// </summary>
        /// <param name="nodeValue">The node value.</param>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        private RequestInfo CreateRequestInfo(string nodeValue, string searchText)
        {
            RequestInfo objRequestInfo = new RequestInfo();
            objRequestInfo.Entity = SetEntity(nodeValue, searchText);
            return objRequestInfo;
        }
        /// <summary>
        /// Sets the entity.
        /// </summary>
        /// <param name="nodeValue">The node value.</param>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        private Entity SetEntity(string nodeValue, string searchText)
        {
            Entity objEntity = new Entity();
            //set entity
            objEntity.ResponseType = TABULAR;
            objEntity.Property = false;
            objEntity.SkipInfo = SetSkipInfo();//commented for the time being,when webservice method will support skip count will uncomment
            //add attribute
            ArrayList arlValue = new ArrayList();
            arlValue.Add(SetValue(nodeValue));
            objEntity.Attribute = SetAttribute(arlValue, GetNodeAttributeValue(xmlNodeCurrentLevel, "parentColName"));
            if(!string.IsNullOrEmpty(searchText))
            {
                objEntity.Criteria = GetCriteria(GetNodeAttributeValue(xmlNodeCurrentLevel, "searchTextColName"), searchText);
            }
            return objEntity;
        }
        /// <summary>
        /// Sets the skip info.
        /// </summary>
        /// <returns></returns>
        private SkipInfo SetSkipInfo()
        {
            SkipInfo objSkipInfo = new SkipInfo();
            objSkipInfo.SkipRecord = intSkipCount.ToString();
            objSkipInfo.MaxFetch = intMaxRecord.ToString();
            return objSkipInfo;
        }
        /// <summary>
        /// Sets the attribute.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns></returns>
        private ArrayList SetAttribute(ArrayList value, string attributeName)
        {
            ArrayList arlAttribute = new ArrayList();
            Attributes objAttribute = new Attributes();
            objAttribute.Name = attributeName;
            objAttribute.Value = value;
            objAttribute.Operator = GetOperator(objAttribute.Value);
            arlAttribute.Add(objAttribute);
            return arlAttribute;
        }
        /// <summary>
        /// Gets the criteria.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        private Criteria GetCriteria(string column, string criteria)
        {
            string strCriteria = string.Empty;
            if(string.IsNullOrEmpty(criteria))
            {
                strCriteria = "*";
            }
            else
            {
                strCriteria = criteria;
            }
            Criteria objCriteria = new Criteria();
            objCriteria.Name = column;
            objCriteria.Value = strCriteria;
            objCriteria.Operator = GetOperator(strCriteria);
            return objCriteria;
        }
        #endregion

        #region Supporting Methods

        /// <summary>
        /// Gets the asset tree configuration.
        /// </summary>
        /// <returns></returns>
        private XmlDocument GetAssetTreeConfiguration()
        {
            XmlDocument xmlDoc = null;
            string strQuery = "<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>AssetTreeConfiguration.xml</Value></Eq></Where>";
            xmlDoc = ((MOSSServiceManager)objMossController).ReadXmlFileFromSharePoint(strSiteURL, AssetTreeConstants.ASSETTREECONFIGLIST, strQuery);
            return xmlDoc;
        }
        /// <summary>
        /// Gets the current level node.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns></returns>
        private XmlNode GetCurrentLevelNode(int level)
        {
            XmlNode xmlNodeCurrentLevel = null;
            if(xmlDocAssetTreeConfig != null)
            {
                for(int intLevelCounter = 0; intLevelCounter <= level; intLevelCounter++)
                {
                    if(intLevelCounter == 0)
                    {
                        xmlNodeCurrentLevel = xmlDocAssetTreeConfig.DocumentElement;
                    }
                    else
                    {
                        xmlNodeCurrentLevel = xmlNodeCurrentLevel.FirstChild;
                    }
                }
            }
            return xmlNodeCurrentLevel;
        }
        /// <summary>
        /// Gets the node attribute value.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns></returns>
        private string GetNodeAttributeValue(XmlNode node, string attributeName)
        {
            string strValue = string.Empty;
            if(node == null)
            {
                strValue = string.Empty;
            }
            else if(node.Attributes[attributeName] != null)
            {
                strValue = node.Attributes[attributeName].Value;
            }
            return strValue;
        }
        /// <summary>
        /// Gets the document depth.
        /// </summary>
        /// <returns></returns>
        private int GetAssetTreeDepth()
        {
            int intDepth = 0;
            XmlNode xmlNodeFirstChild = xmlDocAssetTreeConfig.DocumentElement.FirstChild;
            while(xmlNodeFirstChild != null)
            {
                intDepth++;
                xmlNodeFirstChild = xmlNodeFirstChild.FirstChild;
            }
            return intDepth;
        }
        /// <summary>
        /// Gets the paging node text.
        /// </summary>
        /// <param name="startPageNumber">The start page number.</param>
        /// <param name="endPageNumber">The end page number.</param>
        /// <param name="selectedPage">The selected page.</param>
        /// <param name="pageCount">The page count.</param>
        /// <returns></returns>
        private string GetPagingNodeText(int startPageNumber, int endPageNumber, int selectedPage, int pageCount)
        {
            string strNodeText = string.Empty;
            strNodeText += "<span onclick=\"AllowEventBubbling('activeLink')\" ondblclick=\"AllowEventBubbling()\">";
            if(selectedPage != 1)
            {
                //commented for the time being
                //strNodeText += CreatePagingLink(AssetTreeConstants.FIRST, true);
                strNodeText += AssetTreeConstants.LINKSPACE;
                strNodeText += CreatePagingLink(AssetTreeConstants.PREVIOUS, true);
            }
            for(int intPageCounter = startPageNumber; intPageCounter < endPageNumber; intPageCounter++)
            {
                if((intPageCounter + 1) == selectedPage)
                {
                    strNodeText += AssetTreeConstants.LINKSPACE;
                    strNodeText += CreatePagingLink((intPageCounter + 1).ToString(), false);
                }
                else
                {
                    strNodeText += AssetTreeConstants.LINKSPACE;
                    strNodeText += CreatePagingLink((intPageCounter + 1).ToString(), true);
                }
            }
            if(selectedPage != pageCount)
            {
                strNodeText += AssetTreeConstants.LINKSPACE;
                strNodeText += CreatePagingLink(AssetTreeConstants.NEXT, true);
                //commented for the time being
                //strNodeText += AssetTreeConstants.LINKSPACE;
                //strNodeText += CreatePagingLink(AssetTreeConstants.LAST, true);
            }
            strNodeText += "</span>";
            return strNodeText;
            #region Commented Code
            //can be used later commented for time being
            //if (min > 0)
            //{
            //    Literal spacer = new Literal();
            //    spacer.Text = "&hellip;";
            //    this.Controls.Add(spacer);
            //}

            //if (max < PageCount)
            //{
            //    Literal spacer = new Literal();
            //    spacer.Text = "&hellip;";
            //    this.Controls.Add(spacer);
            //} 
            #endregion

        }
        /// <summary>
        /// Gets the context menu ID.
        /// </summary>
        /// <param name="asset">The asset.</param>
        /// <returns></returns>
        private string GetContextMenuID(string asset)
        {
            string strId = AssetTreeConstants.CONTEXTMENUIDPREFIX + asset;
            return strId;
        }
        /// <summary>
        /// Gets the search box HTML.
        /// </summary>
        /// <param name="nodeText">The node text.</param>
        /// <param name="nodeValue">The node value.</param>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        private string GetSearchBoxHTML(string nodeText, string nodeValue, string searchText)
        {
            StringBuilder strSearchBoxHTML = new StringBuilder();
            strSearchBoxHTML.Append("<span onclick=\"AllowEventBubbling('imgGo')\" ondblclick=\"AllowEventBubbling('nodeLabel')\">");
            strSearchBoxHTML.Append("<label id=\"nodeLabel\">");
            strSearchBoxHTML.Append(nodeText);
            strSearchBoxHTML.Append("&nbsp;&nbsp;</label><img src=\"/_Layouts/DREAM/Images/assettree_search.png\" onclick=\"javascript:ShowHideSearchBox(this);\"/>");
            strSearchBoxHTML.Append("<br/><span id=\"");
            strSearchBoxHTML.Append(nodeValue);
            strSearchBoxHTML.Append("\"class=\"hide\">");
            strSearchBoxHTML.Append("<input id=\"txtSearchBox\" type=\"text\" value=\"%");
            strSearchBoxHTML.Append(searchText + " Name");
            strSearchBoxHTML.Append("%\" />");
            strSearchBoxHTML.Append("&nbsp;<img id=\"imgGo\" src=\"/_Layouts/DREAM/Images/assettree_go.jpg\"  onclick=\"javascript:onSearchClick(this);\"/></span></span>");
            return strSearchBoxHTML.ToString();
        }
        /// <summary>
        /// Adds the root search box.
        /// </summary>
        /// <param name="nodeCollection">The node collection.</param>
        /// 
        private void AddRootSearchBox(RadTreeNodeCollection nodeCollection)
        {
            RadTreeNode firstNode = new RadTreeNode();
            firstNode.Text = GetSearchBoxHTML(string.Empty, "Asset Tree", GetNodeAttributeValue(xmlNodeCurrentLevel, AssetTreeConstants.ASSETNAME));
            firstNode.Value = AssetTreeConstants.ROOTSEARCHBOXNODE;
            firstNode.ExpandMode = TreeNodeExpandMode.ClientSide;
            firstNode.EnableContextMenu = false;
            firstNode.Checkable = false;
            nodeCollection.Add(firstNode);
        }
        /// <summary>
        /// Creates the paging link.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="isClickable">if set to <c>true</c> [is clickable].</param>
        /// <returns></returns>
        private string CreatePagingLink(string text, bool isClickable)
        {
            string strLinkHTML = string.Empty;
            if(isClickable)
            {
                strLinkHTML = "<span  name=\"activeLink\" class=\"pagelink\" style=\"color:blue\" onclick=\"javascript:onPageLinkClick(this);\" >";
            }
            else
            {
                strLinkHTML = "<span id=\"currentLink\" class=\"pagelink\" style=\"color:black\" onclick=\"javascript:onPageLinkClick(this);\" >";
            }
            strLinkHTML += text;
            strLinkHTML += "</span>";
            return strLinkHTML;
        }
        /// <summary>
        /// Adds the last node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="nodeText">The node text.</param>
        /// <param name="nodeValue">The node value.</param>
        private void AddLastNode(RadTreeNode node, string nodeText, string nodeValue)
        {
            RadTreeNode lastNode = new RadTreeNode();
            lastNode.Text = nodeText;
            lastNode.Value = nodeValue;
            lastNode.ExpandMode = TreeNodeExpandMode.ClientSide;
            lastNode.EnableContextMenu = false;
            lastNode.Checkable = false;
            if(nodeValue.Equals(AssetTreeConstants.ERROR))
            {
                lastNode.ForeColor = System.Drawing.Color.Red;
            }
            node.Nodes.Add(lastNode);
        }
        /// <summary>
        /// Adds the last node.
        /// </summary>
        /// <param name="nodeCollection">The node collection.</param>
        /// <param name="nodeText">The node text.</param>
        /// <param name="nodeValue">The node value.</param>
        private void AddLastNode(RadTreeNodeCollection nodeCollection, string nodeText, string nodeValue)
        {
            RadTreeNode lastNode = new RadTreeNode();
            lastNode.Text = nodeText;
            lastNode.Value = nodeValue;
            lastNode.ExpandMode = TreeNodeExpandMode.ClientSide;
            lastNode.EnableContextMenu = false;
            lastNode.Checkable = false;
            if(nodeValue.Equals(AssetTreeConstants.ERROR))
            {
                lastNode.ForeColor = System.Drawing.Color.Red;
            }
            nodeCollection.Add(lastNode);
        }
        #endregion

        #region Exception Handling Method
        /// <summary>
        /// Handles the SOAP exception.
        /// </summary>
        /// <param name="soapEx">The SOAP ex.</param>
        /// <param name="isRootNode">if set to <c>true</c> [is root node].</param>
        /// <param name="node">The node.</param>
        private void HandleSoapException(SoapException soapEx, bool isRootNode, RadTreeNode node)
        {
            string strMessage = string.Empty;
            if(!string.Equals(soapEx.Message.ToString(), SOAPEXCEPTIONMESSAGE))
            {
                CommonUtility.HandleException(strSiteURL, soapEx, 1);
                strMessage = soapEx.Message;
            }
            else
            {
                strMessage = AssetTreeConstants.NORECORDSMESSAGE;
            }
            if(isRootNode)
            {
                AddLastNode(trvAssetTree.Nodes, strMessage, AssetTreeConstants.ERROR);
            }
            else
            {
                AddLastNode(node, strMessage, AssetTreeConstants.ERROR);
            }
        }
        /// <summary>
        /// Handles the web exception.
        /// </summary>
        /// <param name="webEx">The web ex.</param>
        /// <param name="isRootNode">if set to <c>true</c> [is root node].</param>
        /// <param name="node">The node.</param>
        private void HandleWebException(WebException webEx, bool isRootNode, RadTreeNode node)
        {
            CommonUtility.HandleException(strSiteURL, webEx, 1);
            if(isRootNode)
            {
                AddLastNode(trvAssetTree.Nodes, webEx.Message, AssetTreeConstants.ERROR);
            }
            else
            {
                AddLastNode(node, webEx.Message, AssetTreeConstants.ERROR);
            }
        }
        #endregion
        #endregion

    }
}

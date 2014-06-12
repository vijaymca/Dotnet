#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: AssetTree.ascx.cs
#endregion
/// <summary> 
/// This is asset tree usercontrol cs class.
/// </summary>
using System;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.DREAM.Site.UI.UserControls
{
    /// <summary>
    /// This is asset tree usercontrol code behind class.It is calling asset tree helper 
    /// class functions to provide asset tree functionality
    /// </summary>
    public partial class AssetTree : System.Web.UI.UserControl
    {

        #region DECLARATION
        AssetTreeHelper objAssetTreeHelper;
        string strSiteURL = string.Empty;
        int intRecordCount;
        #endregion

        #region Protected Method
        
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
           
            try
            {
                base.EnsureChildControls();
                
                //instantiating object
                objAssetTreeHelper = new AssetTreeHelper(trvAssetTree);
                strSiteURL = objAssetTreeHelper.GetSiteURL();

                if (!IsPostBack)
                {
                    objAssetTreeHelper.CreateContextMenu();
                    intRecordCount = objAssetTreeHelper.PopulateRootNodes(string.Empty, 1);
                    //hidRecordCount.Value = Convert.ToString(intRecordCount);
                    if (trvAssetTree.Nodes.Count > 0)
                    {
                        trvAssetTree.Nodes[0].Attributes.Add(AssetTreeConstants.RECORDCOUNT, Convert.ToString(intRecordCount));
                    }
                    hidPageNumber.Value = 1.ToString();
                    hidClickedPage.Value = 1.ToString();
                    this.Page.ClientScript.RegisterClientScriptBlock(GetType(), "IsPostBack", "var isPostBack = true;", true);
                }
                //attributes is added to resize window on load
                trvAssetTree.Attributes.Add(AssetTreeConstants.RESIZEWINDOWATTRIBUTE, AssetTreeConstants.RESIZEWINDOWATTRIBUTEVALUE);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strSiteURL, ex);
            }
        }
        /// <summary>
        /// Handles the NodeExpand event of the trvAssetTree control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.RadTreeNodeEventArgs"/> instance containing the event data.</param>
        protected void trvAssetTree_NodeExpand(object sender, RadTreeNodeEventArgs e)
        {
            try
            {
                if ((e.Node.Nodes.Count == 0) || (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Value.Equals(AssetTreeConstants.ERROR)))
                {
                    SetHiddenFieldValue(e.Node.Level + 2);
                    if (e.Node.Nodes.Count > 0)
                    DeleteNodes(e.Node.Nodes); 
                    intRecordCount = objAssetTreeHelper.PopulateChildNodes(e.Node, string.Empty, 1);
                    e.Node.Attributes.Add(AssetTreeConstants.RECORDCOUNT, Convert.ToString(intRecordCount));
                }
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strSiteURL, ex);
            }
        }
        /// <summary>
        /// Handles the NodeClick event of the trvAssetTree control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.RadTreeNodeEventArgs"/> instance containing the event data.</param>
        protected void trvAssetTree_NodeClick(object sender, RadTreeNodeEventArgs e)
        {

            try
            {
                RadTreeNode treeNode = null;
                RadTreeNodeCollection objTreeNodeValues = null;
                bool blnRootSearchClick = false;
                if (e.Node.Value.Equals(AssetTreeConstants.TREEVIEWPAGING))
                {
                    treeNode = e.Node.ParentNode;
                }
                else if (e.Node.Value.Equals(AssetTreeConstants.ROOTSEARCHBOXNODE))
                {
                    treeNode = e.Node.ParentNode;
                    blnRootSearchClick = true;
                }
                else
                {
                    treeNode = e.Node;
                    treeNode.Attributes.Add(AssetTreeConstants.SEARCHTEXT, hidSearchText.Value);
                }

                if (treeNode == null)//treenode is null means level is 1
                {
                    SetHiddenFieldValue(1);
                    objTreeNodeValues = trvAssetTree.Nodes;
                    DeleteNodes(objTreeNodeValues);
                    intRecordCount = objAssetTreeHelper.PopulateRootNodes(hidSearchText.Value, Convert.ToInt32(hidPageNumber.Value));
                    if (trvAssetTree.Nodes.Count > 0)
                    {
                        trvAssetTree.Nodes[0].Attributes.Add(AssetTreeConstants.RECORDCOUNT, Convert.ToString(intRecordCount));
                    }
                    if (blnRootSearchClick)
                    {
                        trvAssetTree.Nodes[0].Attributes.Add(AssetTreeConstants.SEARCHTEXT, hidSearchText.Value);
                    }
                }
                else
                {
                    SetHiddenFieldValue(treeNode.Level + 2);
                    objTreeNodeValues = treeNode.Nodes;
                    DeleteNodes(objTreeNodeValues);
                    intRecordCount = objAssetTreeHelper.PopulateChildNodes(treeNode, hidSearchText.Value, Convert.ToInt32(hidPageNumber.Value));
                    e.Node.Attributes.Add(AssetTreeConstants.RECORDCOUNT, Convert.ToString(intRecordCount));
                    e.Node.Expanded = true;
                }
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strSiteURL, ex);
            }

        }
        /// <summary>
        /// Handles the NodeCollapse event of the trvAssetTree control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.RadTreeNodeEventArgs"/> instance containing the event data.</param>
        protected void trvAssetTree_NodeCollapse(object sender, RadTreeNodeEventArgs e)
        {
            
            //currently doing nothing,may require later to add some functionality.
        }
        /// <summary>
        /// Handles the Unload event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Unload(object sender, System.EventArgs e)
        {
            if (trvAssetTree != null)
            {
                trvAssetTree.Dispose();
            }
        }
        #endregion

        #region Private method
        /// <summary>
        /// Sets the hidden field value of current page munber.
        /// </summary>
        /// <param name="level">The level.</param>
        private void SetHiddenFieldValue(int level)
        {
            string strClickedPage = hidClickedPage.Value;
            int intResult = 0;
            if (int.TryParse(strClickedPage, out intResult))
            {
                hidPageNumber.Value = strClickedPage;
            }
            else
            {
                if (strClickedPage.ToLower().Equals(AssetTreeConstants.FIRST.ToLower()))
                {
                    hidPageNumber.Value = Convert.ToString(1);
                }
                else if (strClickedPage.ToLower().Equals(AssetTreeConstants.PREVIOUS.ToLower()))
                {
                    hidPageNumber.Value = Convert.ToString(Convert.ToInt16(hidPageNumber.Value) - 1);
                }
                else if (strClickedPage.ToLower().Equals(AssetTreeConstants.NEXT.ToLower()))
                {
                    hidPageNumber.Value = Convert.ToString((Convert.ToInt16(hidPageNumber.Value) + 1));
                }
                else if (strClickedPage.ToLower().Equals(AssetTreeConstants.LAST.ToLower()))
                {
                    hidPageNumber.Value = Convert.ToString(objAssetTreeHelper.GetPageCount(Convert.ToInt16(hidRecordCount.Value), level));
                }
            }
        }
        /// <summary>
        /// Deletes the nodes.
        /// </summary>
        /// <param name="nodeCollection">The node collection.</param>
        private void DeleteNodes(RadTreeNodeCollection nodeCollection)
        {
            if (nodeCollection != null)
            {
                for (int intNodeCounter = nodeCollection.Count - 1; intNodeCounter >= 0; intNodeCounter--)
                {
                    nodeCollection.RemoveAt(intNodeCounter);
                }
            }
        }

        #endregion

    }
}
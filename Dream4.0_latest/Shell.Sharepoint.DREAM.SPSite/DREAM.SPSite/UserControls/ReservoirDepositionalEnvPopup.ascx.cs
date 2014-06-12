#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: ReservoirDepositionalEnvPopup.ascx.cs 
#endregion


using System;
using System.Data;
using System.Net;

using Telerik.Web.UI;

using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DREAM.Controller;

namespace Shell.SharePoint.DREAM.Site.UI.UserControls
{
    /// <summary>
    /// Reservoir Depositional UI Screen for Reservoir Adv Search 
    /// </summary>
    public partial class ReservoirDepositionalEnvPopup : UIControlHandler
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString[SRPADVPOPUPID] != null)
                {
                    if (Request.QueryString[SRPADVPOPUPID].ToString().Length > 0)
                    {
                        hidFldDepositional.Value = Request.QueryString[SRPADVPOPUPID].ToString();
                    }
                }
                if (!Page.IsPostBack)
                {
                    LoadNodes();
                }
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(strCurrSiteUrl, webEx, 1);
                
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }

        /// <summary>
        /// Handles the Unload event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (radTreeViewDepositinalEnv != null)
                radTreeViewDepositinalEnv.Dispose();
        }

        #region databinding to Rad Tree view control
        /// <summary>
        /// Loads the nodes.
        /// </summary>
        private void LoadNodes()
        {
            DataTable dtDepositionalEnv = null;
           
            try
            {
                dtDepositionalEnv = GetDataSource(DEPOSITIONALENVLIST, string.Empty);
                
                /// Bind RootNode
                string strCamlQuery = @"<Where>
      <And>
         <Eq>
            <FieldRef Name='Hierarchy' />
            <Value Type='Choice'>1</Value>
         </Eq>
         <IsNull>
            <FieldRef Name='ParentNode' />
         </IsNull>
      </And>
   </Where>
   <OrderBy>
      <FieldRef Name='Rank' />
   </OrderBy>";

                dtDepositionalEnv = GetDataSource(DEPOSITIONALENVLIST, strCamlQuery);
                radTreeViewDepositinalEnv.DataTextField = "Title";             
                radTreeViewDepositinalEnv.DataValueField = "Index";
                radTreeViewDepositinalEnv.DataSource = dtDepositionalEnv;
                radTreeViewDepositinalEnv.DataBind();
                BindChildNodes();
            }
            finally
            {
                if (dtDepositionalEnv != null)
                {
                    dtDepositionalEnv.Dispose();
                }
            }
         }

         /// <summary>
         /// Binds the child nodes when tree loaded on page load.
         /// </summary>
        private void BindChildNodes()
        {
            int level = 0;
            foreach (RadTreeNode node in radTreeViewDepositinalEnv.Nodes)
            {
                level = 0;
                level = node.Level + 2;                
                BindChildNodes(node.Value, level.ToString(), node);
                /// Append the ID with "-1" for identifying the hierarchy when node is clicked.
                /// node.Value = string.Format("{0}-{1}", node.Value, "1");
                /// "-" is used in values. So for nodes having "-" in data, is failing. To Avoid "#" is uses instead
                node.Value = string.Format("{0}#{1}", node.Value, "1");
                
                node.ExpandMode = TreeNodeExpandMode.ClientSide;
            }
            
        }
         /// <summary>
         /// Gets the data source.
         /// </summary>
         /// <param name="listName">Name of the list.</param>
         /// <param name="value">The value.</param>
         /// <returns></returns>
        private  DataTable GetDataSource(string listName,string camlQuery)
        {
            MOSSServiceManager objMossController = (MOSSServiceManager)objFactory.GetServiceManager(MOSSSERVICE);
            dtListValues.Reset();
            dtListValues = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, listName, camlQuery);
            return dtListValues;
        }


        /// <summary>
        /// Binds the child nodes when treenode expanded.
        /// </summary>
        /// <param name="parentID">The parent ID.</param>
        /// <param name="nodeDepth">The node depth.</param>
        /// <param name="radNode">The RAD node.</param>
        private void BindChildNodes(string parentID, string nodeDepth, RadTreeNode radNode)
        {           
            string strCamlQuery = @"<Where><And><Eq><FieldRef Name='ParentNode' /><Value Type='Lookup'>" + parentID + "</Value></Eq><Eq><FieldRef Name='Hierarchy' /><Value Type='Choice'>" + nodeDepth + "</Value></Eq></And></Where><OrderBy><FieldRef Name='Rank' /></OrderBy>";

            DataTable dtNodes = new DataTable();
            RadTreeNode childNode;
            dtNodes = GetDataSource(DEPOSITIONALENVLIST, strCamlQuery);
            string strValue = string.Empty;
            try
            {
                if (dtNodes != null && dtNodes.Rows.Count > 0)
                {
                    foreach (DataRow dtRow in dtNodes.Rows)
                    {
                        /// Concate "ID" and "Hierarchy" field values to Value field of TreeNode.   
                        /// node.Value = string.Format("{0}-{1}", node.Value, "Hierarchy");
                        /// "-" is used in values. So for nodes having "-" in data, is failing. To Avoid "#" is uses instead
                        //strValue = string.Format("{0}-{1}", Convert.ToString(dtRow["Index"]), Convert.ToString(dtRow["Hierarchy"]));
                        strValue = string.Format("{0}#{1}", Convert.ToString(dtRow["Index"]), Convert.ToString(dtRow["Hierarchy"]));

                        childNode = new RadTreeNode(Convert.ToString(dtRow["Title"]), strValue);
                        /// If the data is for last level of Hierary, don't show + symbol for expansion.
                        if (nodeDepth != "5")
                        {
                            childNode.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
                        }
                        radNode.Nodes.Add(childNode);
                    }
                }
            }
            finally
            {
                if (dtNodes != null)
                    dtNodes.Dispose();
            }
        }
        #endregion
        #region OnDemand Approach

        /// <summary>
         /// Populates the node on demand.
         /// </summary>
         /// <param name="e">The <see cref="Telerik.Web.UI.RadTreeNodeEventArgs"/> instance containing the event data.</param>
         /// <param name="expandMode">The expand mode.</param>
        protected void PopulateNodeOnDemand(object sender,RadTreeNodeEventArgs e)
        {
            /// "-" is used in values. So for nodes having "-" in data, is failing. To Avoid "#" is uses instead
            string[] strValue = e.Node.Value.Split("#".ToCharArray());
            int intHierarchy = 0;
            int.TryParse(strValue[1], out intHierarchy);
            intHierarchy++;
            BindChildNodes(strValue[0],intHierarchy.ToString() , e.Node);  
               
        }
        
        #endregion
    }
}
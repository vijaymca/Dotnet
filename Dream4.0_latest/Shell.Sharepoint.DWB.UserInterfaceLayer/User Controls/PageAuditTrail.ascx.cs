#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: PageAuditTrail.ascx 
#endregion

using System;
using System.Web;
using System.Data;
using System.Net;
using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// Displays the Audit Trail information of each page 
    /// </summary>
    public partial class PageAuditTrail : UIHelper
    {

        #region DECLARATION
        string strPageId=string.Empty;
        #endregion
        
        #region Methods
        
        /// <summary>
        /// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"></see> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the server control content.</param>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (HttpContext.Current.Session["WEBPARTPROPERTIES"] != null &&
                    !((TreeNodeSelection)HttpContext.Current.Session["WEBPARTPROPERTIES"]).IsChapterSelected &&
                    !((TreeNodeSelection)HttpContext.Current.Session["WEBPARTPROPERTIES"]).IsBookSelected)
            {
                TreeNodeSelection objTreeNodeSelection = null;
                objTreeNodeSelection = (TreeNodeSelection)HttpContext.Current.Session["WEBPARTPROPERTIES"];
                strPageId = objTreeNodeSelection.PageID;
                LoadPageAuditTrail();
                base.Render(writer);
            }
        }

        /// <summary>
        /// Retrieves the Audit Information and binds the grid view
        /// </summary>
        private void LoadPageAuditTrail()
        {
            DataTable dtAuditTrail = null;
            
            string strCAMLQuery = @"<OrderBy><FieldRef Name='Created' Ascending='False' /></OrderBy><Where><Eq><FieldRef Name='Master_ID' /><Value Type='Number'>" + strPageId + "</Value></Eq></Where>";
            dtAuditTrail = GetListItems(CHAPTERPAGESMAPPINGAUDITLIST, strCAMLQuery, string.Empty);
            grdAuditTrail.DataSource = dtAuditTrail;
            grdAuditTrail.DataBind();
        }
        #endregion
    }
}
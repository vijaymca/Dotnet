#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : PageNarrative.cs
#endregion
using System;
using System.Web;
using System.Data;
using System.Net;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DWB.BusinessLogicLayer;
using Shell.SharePoint.DWB.Business.DataObjects;

namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// Displays and updates the Narrative of a Page 
    /// </summary>
    public partial class PageNarrative : UIHelper
    {
        #region Declarations
        DataTable dtResultTable;
        string strPageID = string.Empty;
             
        /// <summary>
        /// Gets or sets the page ID.
        /// </summary>
        /// <value>The page ID.</value>
        string PageID
        {
            get
            {
                return strPageID;
            }
            set
            {
                strPageID = value;
            }
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"></see> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the server control content.</param>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES] != null &&
                    !((TreeNodeSelection)HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES]).IsChapterSelected &&
                    !((TreeNodeSelection)HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES]).IsBookSelected)
            {
                DataRow drNarrative;
                string strBookOwner = string.Empty;
                string strPageOwner = string.Empty;
                string strUserName = string.Empty;
                string strBookTeamID = string.Empty;
                string strPageDiscipline = string.Empty;
                TreeNodeSelection objTreeNodeSelection = null;
                try
                {
                    objTreeNodeSelection = (TreeNodeSelection)HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES];
                    if (objTreeNodeSelection != null)
                    {
                        PageID = objTreeNodeSelection.PageID;
                    }
                    GetPageNarrative();
                    txtNarrative.Text = string.Empty;
                    strUserName = GetUserName();
                    if (dtResultTable != null)
                    {
                        drNarrative = dtResultTable.Rows[0];
                        txtNarrative.Text = drNarrative["Narrative"].ToString();
                    }

                    if (HttpContext.Current.Session[SESSIONTREEVIEWDATAOBJECT] != null)
                    {
                        BookInfo objBookInfo = null;
                        objBookInfo = ((BookInfo)HttpContext.Current.Session[SESSIONTREEVIEWDATAOBJECT]);

                        if (objBookInfo != null)
                        {
                            strBookOwner = objBookInfo.BookOwner;
                            strBookTeamID = objBookInfo.BookTeamID;
                        }
                        string strCamlQuery = @"<Where><Eq><FieldRef Name='ID' />
                 <Value Type='Counter'>" + strPageID + "</Value></Eq></Where>";
                        string strViewFields = @"<FieldRef Name='ID' /><FieldRef Name='Owner' /><FieldRef Name='Discipline' />";
                        CommonBLL objCommonBLL = new CommonBLL();
                        DataTable dtBookPage = objCommonBLL.ReadList(strParentSiteURL, CHAPTERPAGESMAPPINGLIST, strCamlQuery, strViewFields);

                        if (dtBookPage != null && dtBookPage.Rows.Count > 0)
                        {
                            strPageOwner = Convert.ToString(dtBookPage.Rows[0]["Owner"]);
                            strPageDiscipline = Convert.ToString(dtBookPage.Rows[0]["Discipline"]);
                            dtBookPage.Dispose();
                        }
                    }
                    if (ShowButton(WELLBOOKVIEWERCONTROLNARRATIVE, strBookOwner, strBookTeamID, strPageOwner, strPageDiscipline))
                    {
                        txtNarrative.Enabled = true;
                        btnAdd.Visible = true;
                    }
                }
                catch (WebException webEx)
                {
                    CommonUtility.HandleException(strParentSiteURL, webEx);
                }
                catch (Exception Ex)
                {
                    CommonUtility.HandleException(strParentSiteURL, Ex);
                }
                base.Render(writer);
            }
        }
              
        /// <summary>
        /// Updates the narrative edited by the user to the Sharepoint List
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNodeSelection objTreeNodeSelection = null;

                objTreeNodeSelection = (TreeNodeSelection)HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES];
                if (objTreeNodeSelection != null)
                {
                    PageID = objTreeNodeSelection.PageID;
                }
                UpdateNarrative(DWBNARRATIVES, CHAPTERPAGESMAPPINGAUDITLIST, PageID, txtNarrative.Text.Trim());               
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(strParentSiteURL, webEx);
            }
            catch (Exception Ex)
            {
                CommonUtility.HandleException(strParentSiteURL, Ex);
            }
        }
        #endregion 
    
        #region Private Methods
        /// <summary>
        /// Gets the page narrative.
        /// </summary>
        private void GetPageNarrative()
        {
            string strCAMLQuery = string.Empty;
            try
            {
                strCAMLQuery = @"<Where><Eq><FieldRef Name='Page_ID' /><Value Type='Number'>" + PageID + "</Value></Eq></Where>";
                dtResultTable = GetListItems(DWBNARRATIVES, strCAMLQuery, string.Empty);
            }
            catch (Exception Ex)
            {
                CommonUtility.HandleException(strParentSiteURL, Ex);
            }
        }
          #endregion 
    }
}
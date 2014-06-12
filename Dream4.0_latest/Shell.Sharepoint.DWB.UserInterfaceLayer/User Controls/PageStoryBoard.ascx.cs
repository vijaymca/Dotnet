#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : PageStoryBoard.cs
#endregion

using System;
using System.Data;
using System.Net;
using System.Web;

using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// Displays and updates the story Board Details of the page 
    /// </summary>
    public partial class PageStoryBoard : UIHelper
    {
        #region Declarations
        DataTable dtResultTable = new DataTable();

        string strPageID = string.Empty;
        string strBookOwner = string.Empty;
        string strPageOwner = string.Empty;
        string strBookTeamID = string.Empty;
        string strPageDiscipline = string.Empty;

        
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
                try
                {
                TreeNodeSelection objTreeNodeSelection = null;
                objTreeNodeSelection = (TreeNodeSelection)HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES];
                if (objTreeNodeSelection != null)
                {
                    PageID = objTreeNodeSelection.PageID;
                }
                    pnlStoryBoard.Visible = true;
                    GetStoryBoardInformation();

                    hidUserName.Value = GetUserName();
                    if (HttpContext.Current.Session[SESSIONTREEVIEWDATAOBJECT] != null)
                    {
                        BookInfo objBookInfo = null;
                        objBookInfo = ((BookInfo)HttpContext.Current.Session[SESSIONTREEVIEWDATAOBJECT]);
                        hidBookOwner.Value = objBookInfo.BookOwner;
                        strBookOwner = objBookInfo.BookOwner;
                        strBookTeamID = objBookInfo.BookTeamID;
                    }

                    BindDataToTable();

                    if (ShowButton(WELLBOOKVIEWERCONTROLSTORYBOARD, strBookOwner, strBookTeamID, strPageOwner, strPageDiscipline))
                    {
                        txtSource.Enabled = true;
                        txtApplicationGeneratingPage.Enabled = true;
                        txtApplicationTemplate.Enabled = true;
                        btnSave.Visible = true;
                    }
                }
                catch (WebException webEx)
                {
                    CommonUtility.HandleException(strParentSiteURL, webEx);
                }
                catch (Exception Ex)
                {
                    Shell.SharePoint.DREAM.Utilities.CommonUtility.HandleException(strParentSiteURL, Ex);
                }
                base.Render(writer);
            }
            
        }
        
        /// <summary>
        /// Updates the story board information of the page into the the sharepoint list
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            string strSource = string.Empty;
            string strApplicationPage = string.Empty;
            string strApplicationTemplate = string.Empty;

            try
            {
                TreeNodeSelection objTreeNodeSelection = null;
                objTreeNodeSelection = (TreeNodeSelection)HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES];
                if (objTreeNodeSelection != null)
                {
                    PageID = objTreeNodeSelection.PageID;
                }
                StoryBoard objPageStoryBoard = new StoryBoard();
                objPageStoryBoard.PageId = Convert.ToInt32(PageID);
                objPageStoryBoard.ApplicationPage = txtApplicationGeneratingPage.Text.Trim();
                objPageStoryBoard.ApplicationTemplate = txtApplicationTemplate.Text.Trim();
                objPageStoryBoard.CreatedBy = txtCreatedBy.Text.Trim();
                objPageStoryBoard.CreationDate = txtCreationDate.Text.Trim();
                objPageStoryBoard.Discipline = txtDiscipline.Text.Trim();
                objPageStoryBoard.MasterPageName = txtMasterPageName.Text.Trim();
                objPageStoryBoard.PageOwner = txtPageOwner.Text.Trim();
                objPageStoryBoard.PageTitle = txtPageTitle.Text.Trim();
                objPageStoryBoard.PageType = txtPageType.Text.Trim();
                objPageStoryBoard.SOP = txtSOP.Text.Trim();
                objPageStoryBoard.Source = txtSource.Text.Trim();

                UpdateStoryBoard(DWBSTORYBOARD, CHAPTERPAGESMAPPINGAUDITLIST, PageID, objPageStoryBoard, AUDITACTIONSTORYBOARDUPDATED);
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
        /// Gets the story board information.
        /// </summary>
        private void GetStoryBoardInformation()
        {
            string strCAMLQuery = string.Empty;
            string strfieldsToView = string.Empty;

            strCAMLQuery = @"<Where><Eq><FieldRef Name='Page_ID' /><Value Type='Number'>" + PageID + "</Value></Eq></Where>";
            dtResultTable = GetListItems(DWBSTORYBOARD, strCAMLQuery, strfieldsToView);
        }

        /// <summary>
        /// Binds the table.
        /// </summary>
        private void BindDataToTable()
        {
            DataRow objListRow;
            if (dtResultTable != null)
            {
                if (dtResultTable.Rows.Count > 0)
                {
                    objListRow = dtResultTable.Rows[0];
                    txtPageType.Text = objListRow["Page_Type"].ToString();
                    txtPageTitle.Text = objListRow["Page_Title"].ToString();
                    txtConnectionType.Text = objListRow["Connection_Type"].ToString();
                    txtSource.Text = objListRow["Source"].ToString();
                    txtDiscipline.Text = objListRow["Discipline"].ToString();
                    txtMasterPageName.Text = objListRow["Master_Page"].ToString();
                    txtApplicationGeneratingPage.Text = objListRow["Application_Page"].ToString();
                    txtApplicationTemplate.Text = objListRow["Application_Template"].ToString();
                    txtSOP.Text = objListRow["SOP"].ToString();
                    txtCreatedBy.Text = objListRow["Created_By"].ToString();
                    txtCreationDate.Text = objListRow["Creation_Date"].ToString();
                    txtPageOwner.Text = objListRow["Page_Owner"].ToString();
                    strPageOwner = objListRow["Page_Owner"].ToString();
                    strPageDiscipline = objListRow["Discipline"].ToString();
                }
                dtResultTable.Dispose();
            }
        }
        #endregion

    }
}
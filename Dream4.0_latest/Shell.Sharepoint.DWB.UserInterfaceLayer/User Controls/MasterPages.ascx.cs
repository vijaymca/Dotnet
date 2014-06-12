#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: MasterPages.cs
#endregion
using System;
using System.Data;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DWB.BusinessLogicLayer;


namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// Master Pages user Interface
    /// </summary>
    public partial class MasterPages : UIHelper
    {
        #region DECLARATION

        const string DWBTYPE2PAGEURL = "DWBPublishedDocuments.aspx";
        const string DWBTYPE3PAGEURL = "DWBUserDefinedDocument.aspx";
        const string CHAPTERPAGES = "ChapterPages";
        const string BOOKPAGES = "BookPages";
        const string PAGETITLEEXISTSMSG = "Page Title already exist. Please enter a different title.";
        const string MAINTAINMASTERURL = "MaintainMasterPage.aspx";

        WellBookBLL objWellBook;
        ChapterBLL objChapterBll;
        ListEntry objListEntry;
        CommonUtility objUtility;
        TemplateDetailBLL objTemplateBLL;
        string strSelectedID = string.Empty;
        string strMode = string.Empty;
        string strListType = string.Empty;
        string strEditMasterPageHeading = "Edit Master Page: {0}";
        /// <summary>
        /// Gets or sets the parent id.
        /// </summary>
        /// <value>The parent id.</value>
        public string ParentId
        {
            get
            {
                return Convert.ToString(ViewState["ParentId"]);
            }
            set
            {
                ViewState["ParentId"] = value;
            }
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            strMode = HttpContext.Current.Request.QueryString[MODEQUERYSTRING];
            strSelectedID = HttpContext.Current.Request.QueryString[IDVALUEQUERYSTRING];
            strListType = HttpContext.Current.Request.QueryString[LISTTYPEQUERYSTRING];

            try
            {
                if (!IsPostBack)
                {
                    LoadControlsOnPageLoad();
                    if (string.Equals(strMode, EDIT))
                    {

                        GetMasterPageDetails();
                        if (objListEntry != null)
                        {
                            SetUIControlForMode();
                            SetUIControls(objListEntry);
                        }
                    }
                }
            }
            catch (WebException webEx)
            {

                lblException.Text = webEx.Message;
                lblException.Visible = true;
                ExceptionBlock.Visible = true;
            }
            catch (Exception ex)
            {

                CommonUtility.HandleException(strParentSiteURL, ex);

            }
        }

        /// <summary>
        /// Handles the Click event of the cmdCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(GetUrlToRedirect(), false);
        }

        /// <summary>
        /// Handles the Click event of the cmdOK control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmdOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.Equals(HttpContext.Current.Request.QueryString[MODEQUERYSTRING], EDIT))
                {
                    if (!CheckDuplicateName(txtPageTitle.Text.Trim(), DWBTITLECOLUMN, MASTERPAGELIST))
                    {
                        SetListEntry();
                        UpdateMasterPageDetails();

                    }
                    else
                    {
                        lblException.Text = PAGETITLEEXISTSMSG;
                        lblException.Visible = true;
                        ExceptionBlock.Visible = true;
                        return;
                    }
                }
                else
                {
                    SetListEntry();
                    UpdateMasterPageDetails();

                }
                Page.Response.Redirect(GetUrlToRedirect(), false);
            }
            catch (WebException webEx)
            {

                lblException.Text = webEx.Message;
                lblException.Visible = true;
                ExceptionBlock.Visible = true;
            }
            catch (Exception ex)
            {

                CommonUtility.HandleException(strParentSiteURL, ex);

            }
        }

        /// <summary>
        /// Selected index changed
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cboAssetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strSelectedAsset = cboAssetType.SelectedItem.Text;

                string strcamlQuery = @"<OrderBy><FieldRef Name='Title' /></OrderBy><Where><Eq><FieldRef Name='Asset_Type' />
                    <Value Type='Lookup'>" + strSelectedAsset + "</Value></Eq></Where>";
                lstTemplates.Items.Clear();
                SetListValues(lstTemplates, TEMPLATELIST, strcamlQuery, null);
            }
            catch (WebException webEx)
            {

                lblException.Text = webEx.Message;
                lblException.Visible = true;
                ExceptionBlock.Visible = true;
            }
            catch (Exception ex)
            {

                CommonUtility.HandleException(strParentSiteURL, ex);

            }

        }
        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Sets the master page details.
        /// </summary>
        private void GetMasterPageDetails()
        {
            switch (strListType)
            {
                case CHAPTERPAGES:
                    objChapterBll = new ChapterBLL();
                    objListEntry = objChapterBll.GetChapterPages(strParentSiteURL, CHAPTERPAGESMAPPINGLIST, @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + strSelectedID + "</Value></Eq></Where>");
                    break;
                case BOOKPAGES:
                    objWellBook = new WellBookBLL();
                    objListEntry = objWellBook.GetBookPages(strParentSiteURL, CHAPTERPAGESMAPPINGLIST, @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + strSelectedID + "</Value></Eq></Where>");
                    break;
                case TEMPLATEMASTERPAGES:
                    {
                        objTemplateBLL = new TemplateDetailBLL();
                        string strCAMLQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + strSelectedID + "</Value></Eq></Where>";

                        string strViewFields = @"<FieldRef Name='ID' /><FieldRef Name='Asset_Type' /><FieldRef Name='Connection_Type' /><FieldRef Name='Page_Owner' /><FieldRef Name='Page_Sequence' /><FieldRef Name='Page_URL' /><FieldRef Name='Discipline' /><FieldRef Name='Standard_Operating_Procedure' /><FieldRef Name='Page_Title_Template' /><FieldRef Name='Template_ID' /><FieldRef Name='Master_Page_Name' /><FieldRef Name='Master_Page_ID' />";
                        objListEntry = objTemplateBLL.GetMasterPageDetails(strParentSiteURL, TEMPLATEPAGESMAPPINGLIST, strCAMLQuery, strViewFields);
                        break;
                    }
                default:
                    objListEntry = GetDetailsForSelectedID(strSelectedID, MASTERPAGELIST, MASTERPAGE);
                    break;
            }
        }

        /// <summary>
        /// Binds the UI controls.
        /// </summary>
        /// <param name="objMasterData">The obj master data.</param>
        private void SetUIControls(ListEntry objMasterData)
        {
            if (objMasterData != null && objMasterData.MasterPage != null)
            {
                txtPageTitle.Text = objMasterData.MasterPage.Name;

                int intIndexof = 0;
                if (!string.IsNullOrEmpty(objMasterData.MasterPage.TemplateTitle))
                {
                    intIndexof = objMasterData.MasterPage.TemplateTitle.IndexOf("$ -");
                }
                if (intIndexof == -1)
                {
                    txtTitleTemplate.Text = "$ - " + objMasterData.MasterPage.TemplateTitle;
                }
                else
                {
                    txtTitleTemplate.Text = objMasterData.MasterPage.TemplateTitle;
                }

                BindDropDownList(cboAssetType, objMasterData.MasterPage.AssetType);
                cboAssetType.Enabled = false;

                BindDropDownList(cboDiscipline, objMasterData.MasterPage.SignOffDiscipline);

                txtSOP.Text = objMasterData.MasterPage.SOP;

                BindDropDownList(cboConnectionType, objMasterData.MasterPage.ConnectionType);
                cboConnectionType.Enabled = false;
                hdnListItemStatus.Value = objMasterData.MasterPage.Terminated;
                if (string.Compare(strListType,BOOKPAGES, true) == 0)
                {
                    lstTemplates.Visible = false;
                    lblTemplates.Visible = false;
                    lblMasterPage.Text = string.Format("Edit Book Page: {0}", objListEntry.MasterPage.Name);
                    if (objListEntry.ChapterDetails != null)
                        ParentId = Convert.ToString(objListEntry.ChapterDetails.BookID);
                }
                else if (string.Compare(strListType,CHAPTERPAGES, true) == 0)
                {
                    lstTemplates.Visible = false;
                    lblTemplates.Visible = false;
                    lblMasterPage.Text = string.Format("Edit Chapter Page: {0}", objListEntry.MasterPage.Name);
                    if (objListEntry.ChapterDetails != null)
                        ParentId = Convert.ToString(objMasterData.ChapterDetails.RowID);
                }
                else if (string.Compare(strListType,TEMPLATEMASTERPAGES, true) == 0)
                {
                    lstTemplates.Visible = false;
                    lblTemplates.Visible = false;
                    if (objListEntry.MasterPage != null)
                    {
                        ParentId = objListEntry.MasterPage.Templates;
                    }
                }
                else
                {
                    string strCamlQuery = @"<Where><Eq><FieldRef Name='Asset_Type' />
              <Value Type='Lookup'>" + objMasterData.MasterPage.AssetType + "</Value></Eq></Where>";
                    SetListValues(lstTemplates, TEMPLATELIST, strCamlQuery, objMasterData);
                    lstTemplates.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Binds the drop down list.
        /// </summary>
        /// <param name="dropDown">The drop down.</param>
        /// <param name="value">The value.</param>
        private void BindDropDownList(DropDownList dropDown, string value)
        {
            if (dropDown != null)
            {                
                if (dropDown.Items.FindByText(value) != null)
                {
                    dropDown.ClearSelection();
                    dropDown.Items.FindByText(value).Selected = true;
                }
            }            
        }

        /// <summary>
        /// Loads the controls on page load.
        /// </summary>
        private void LoadControlsOnPageLoad()
        {
            string strSelectedAsset = string.Empty;
            SetListValues(cboAssetType, ASSETTYPELIST);
            /// Remove the --Select-- option
            /// Set the Wellbore as default value
            if (cboAssetType != null && cboAssetType.Items.FindByText("Wellbore") != null)
            {
                cboAssetType.Items.RemoveAt(0);
                cboAssetType.ClearSelection();
                cboAssetType.Items.FindByText("Wellbore").Selected = true;
                strSelectedAsset = cboAssetType.SelectedItem.Text;
            }
            SetListValues(cboDiscipline, DISCIPLINELIST);
            SetListValues(cboConnectionType, CONNECTIONLIST);
            /// Generic method used in UI helper adds select but not required in connection type
            cboConnectionType.Items.Remove(DROPDOWNDEFAULTTEXT);
            cboConnectionType.SelectedIndex = 2;
            SetUIControlForMode();
            if (!string.IsNullOrEmpty(strSelectedAsset))
            {
                string strcamlQuery = @"<OrderBy><FieldRef Name='Title' /></OrderBy><Where><Eq><FieldRef Name='Asset_Type' />
                    <Value Type='Lookup'>" + strSelectedAsset + "</Value></Eq></Where>";
                lstTemplates.Items.Clear();
                SetListValues(lstTemplates, TEMPLATELIST, strcamlQuery, null);
            }
        }

        /// <summary>
        /// Hides controls based on the mode.
        /// </summary>
        private void SetUIControlForMode()
        {
            switch (strMode)
            {
                case EDIT:
                    {
                        if (objListEntry != null && objListEntry.MasterPage != null)
                        {
                            lblMasterPage.Text = string.Format(strEditMasterPageHeading, objListEntry.MasterPage.Name);
                        }
                        else
                        {
                            lblMasterPage.Text = string.Format(strEditMasterPageHeading, string.Empty);
                        }
                        lblTemplates.Text = "This page is used in the following Template(s):";
                        this.Parent.Page.Title = "Edit Master Page";

                        break;
                    }
                default:
                    break;
            }
        }

        /// <summary>
        /// Sets the list entry Data object.
        /// </summary>
        private void SetListEntry()
        {
            objListEntry = new ListEntry();
            objListEntry.MasterPage = SetMasterPageDetails();

        }

        /// <summary>
        /// Sets the master page details data object from the UI.
        /// </summary>
        /// <returns></returns>
        private MasterPageDetails SetMasterPageDetails()
        {
            MasterPageDetails objMasterPage = new MasterPageDetails();
            int intRowId = 0;
            objMasterPage.Name = txtPageTitle.Text.Trim();
            objMasterPage.TemplateTitle = txtTitleTemplate.Text.Trim();

            if (cboDiscipline.SelectedIndex > 0)
            {
                if ((string.Compare(strListType, BOOKPAGES, true) == 0) || (string.Compare(strListType,CHAPTERPAGES, true) == 0) ||
                  (string.Compare(strListType, TEMPLATEMASTERPAGES, true) == 0))
                {
                    objMasterPage.SignOffDiscipline = cboDiscipline.SelectedItem.Text;
                    /// This property is used to update DWB Template Pages Mapping list details
                    objMasterPage.SignOffDisciplineText = cboDiscipline.SelectedItem.Text;
                }
                else
                {
                    objMasterPage.SignOffDiscipline = cboDiscipline.SelectedItem.Value;
                    /// This property is used to update DWB Template Pages Mapping list details
                    objMasterPage.SignOffDisciplineText = cboDiscipline.SelectedItem.Text;
                }
            }
            objMasterPage.SOP = txtSOP.Text.Trim();

            if (string.Compare(strListType, TEMPLATEMASTERPAGES) == 0)
            {
                objMasterPage.ConnectionType = cboConnectionType.SelectedItem.Text;
                objMasterPage.AssetType = cboAssetType.SelectedItem.Text;
                /// This property is used to update DWB Template Pages Mapping list details
                objMasterPage.ConnectionTypeText = cboConnectionType.SelectedItem.Text;
                /// This property is used to update DWB Template Pages Mapping list details
                objMasterPage.AssetTypeText = cboAssetType.SelectedItem.Text;
            }
            else
            {
                objMasterPage.ConnectionType = cboConnectionType.SelectedItem.Value;
                objMasterPage.AssetType = cboAssetType.SelectedItem.Value;
                /// This property is used to update DWB Template Pages Mapping list details
                objMasterPage.ConnectionTypeText = cboConnectionType.SelectedItem.Text;
                /// This property is used to update DWB Template Pages Mapping list details
                objMasterPage.AssetTypeText = cboAssetType.SelectedItem.Text;
            }           
            if (string.Equals(strMode, ADD))
            {
                objMasterPage.Terminated = "No";
                if (cboConnectionType.SelectedItem.Value.Equals("2"))
                {
                    objMasterPage.PageURL = DWBTYPE2PAGEURL;
                }
                else if (cboConnectionType.SelectedItem.Value.Equals("3"))
                {
                    objMasterPage.PageURL = DWBTYPE3PAGEURL;
                }
                ListItemCollection templateList = lstTemplates.Items;
                foreach (ListItem templateItem in templateList)
                {
                    if (templateItem.Selected)
                    {
                        objMasterPage.Templates = objMasterPage.Templates + templateItem.Value + ";";
                    }
                }

            }
            else
            {
                if (int.TryParse(strSelectedID, out intRowId))
                {
                    objMasterPage.RowId = intRowId;
                }

            }

            return objMasterPage;
        }

        /// <summary>
        /// Updates the master page details.
        /// </summary>
        private void UpdateMasterPageDetails()
        {
            string strActionPerformed = AUDITACTIONCREATION;
            if (string.Compare(strMode,EDIT) == 0)
            {
                strActionPerformed = AUDITACTIONUPDATION;
            }
            objUtility = new CommonUtility();
            string strUserName = objUtility.GetUserName();
            switch (strListType)
            {
                case BOOKPAGES:
                    objWellBook = new WellBookBLL();
                    objWellBook.UpdateBookPage(strParentSiteURL, objListEntry, CHAPTERPAGESMAPPINGLIST, CHAPTERPAGESMAPPINGAUDITLIST, strUserName, strActionPerformed);
                    break;
                case CHAPTERPAGES:
                    objWellBook = new WellBookBLL();
                    objWellBook.UpdateBookPage(strParentSiteURL, objListEntry, CHAPTERPAGESMAPPINGLIST, CHAPTERPAGESMAPPINGAUDITLIST, strUserName, strActionPerformed);
                    break;
                case TEMPLATEMASTERPAGES:
                    objTemplateBLL = new TemplateDetailBLL();
                    objTemplateBLL.UpdateMasterPageDetail(strParentSiteURL, TEMPLATEPAGESMAPPINGLIST, TEMPLATECONFIGURATIONAUDIT, objListEntry, strActionPerformed, strUserName);
                    break;
                default:
                    UpdateListEntry(objListEntry, MASTERPAGELIST, MASTERPAGEAUDITLIST, MASTERPAGE, strActionPerformed);
                    break;
            }
        }

        /// <summary>
        /// Gets the URL to redirect.
        /// </summary>
        /// <returns></returns>
        private string GetUrlToRedirect()
        {
            string strUrlToRedirect = string.Empty;
            switch (strListType)
            {
                case BOOKPAGES:
                    strUrlToRedirect = "MaintainBookPages.aspx?BookId=" + ParentId;
                    break;
                case CHAPTERPAGES:
                    strUrlToRedirect = "MaintainChaptersPages.aspx?ChapterID=" + ParentId;
                    break;
                case TEMPLATEMASTERPAGES:
                    strUrlToRedirect = "MaintainTemplatePages.aspx?idValue=" + ParentId;
                    break;
                default:
                    if (string.Compare(hdnListItemStatus.Value,STATUSTERMINATED, true) == 0)
                    {
                        strUrlToRedirect = MAINTAINMASTERURL + TERMINATESTATUSQUERYSTRING;
                    }
                    else
                    {
                        strUrlToRedirect = MAINTAINMASTERURL;
                    }
                    break;
            }
            return strUrlToRedirect;

        }
        #endregion Private Methods
    }
}
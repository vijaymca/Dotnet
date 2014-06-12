#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: AddChapter.cs 
#endregion

using System;
using System.Data;
using System.Collections.Generic;
using System.Net;
using System.Web.Services.Protocols;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;

using Microsoft.SharePoint;

using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DWB.BusinessLogicLayer;
using DREAMDataObjects = Shell.SharePoint.DREAM.Business.Entities;

namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// Control used to add and edit chapter
    /// </summary>
    public partial class AddChapter : UIHelper
    {
        #region DECLARATION

        const string CHANGEMASTERPAGE = "change";
        const string PAGETITLEEXISTSMSG = "Chapter Title already exist. Please enter a different title.";
        const string MAINTAINWELLBOOKURL = "BookMaintenance.aspx";      
        const string COUNTRYLIST = "Country";
        const string REPORTSERVICECOLUMN = "DWB Report Service Columns";
        const string REPORTXPATH = "response/report";
        const string RECORDXPATH = "/response/report/record";
        const string SEARCHBUTTONRESETTEXT = "Reset";
        const string SEARCHBUTTONDEFAULTTEXT = "Find Assets";
        const string COUNTRYDROPDOWNDEFAULTTEXT = "--Any Country--";
        const string CONNECTIONTYPEI = "Automated";
        const string DROPDONWDEFAULT = "Select";        
        const string EDITCHAPTER = "Edit  Chapter";
        
        string strBookID = string.Empty;
        string strMode = string.Empty;
        string strSelectedID = string.Empty;
        ListEntry objListEntry;
        ChapterBLL objChapter;
        CommonBLL objCommonBLL;
        
        #endregion

        #region Protected Methods - Event Handlers
        /// <summary>
        /// page load event triggered by asp.net engine. Used to read query string 
        /// and populate the UI controls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                strMode = HttpContext.Current.Request.QueryString[MODEQUERYSTRING];
                strSelectedID = HttpContext.Current.Request.QueryString[IDVALUEQUERYSTRING];
                lblException.Visible = false;
                strBookID = HttpContext.Current.Request.QueryString[BOOKIDQUERYSTRING];
                if (!IsPostBack)
                {
                    LoadControlsOnPageLoad();
                    if (string.Equals(strMode, EDIT))
                    {
                        objListEntry = GetDetailsForSelectedID(strSelectedID, DWBCHAPTERLIST, CHAPTER);
                        if (objListEntry != null)
                        {
                            BindUIControls();
                        }
                    }

                    Page.ClientScript.RegisterStartupScript(this.Page.GetType(), SETWINDOWTITLEJSKEY, string.Format(SETWINDOWTITLEVALUE, lblChapter.Text), true);
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
        /// Handles the Click event of the btnSearch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DREAMDataObjects.RequestInfo objRequestInfo = null;
            try
            {
                if (string.Equals(btnSearch.Text, SEARCHBUTTONRESETTEXT))
                {
                    cboDWBCountry.Enabled = true;
                    cboColumnName.Enabled = true;
                    cboAssetType.Enabled = true;
                    txtCriteria.Enabled = true;
                    btnSearch.Text = SEARCHBUTTONDEFAULTTEXT;
                    lstAssetValues.Items.Clear();
                }
                else
                {
                    string strXpath = string.Empty;
                    string strSelectedAsset = cboAssetType.SelectedItem.Text;
                    objChapter = new ChapterBLL();
                    //objRequestInfo = SetQuickDataObjects(cboDWBCountry.SelectedItem.Text, cboColumnName.SelectedItem.Value, txtCriteria.Text);
                    objRequestInfo = SetQuickDataObjects(cboDWBCountry.SelectedItem.Value, cboColumnName.SelectedItem.Value, txtCriteria.Text);
                    XmlDocument assetNodeList = objChapter.GetAssetValueForAssetType(objRequestInfo, strSelectedAsset);
                    lblAssetValue.Text = strSelectedAsset;
                    AssignValuesToDropDown(assetNodeList, lstAssetValues);
                    cboDWBCountry.Enabled = false;
                    cboColumnName.Enabled = false;
                    cboAssetType.Enabled = false;
                    txtCriteria.Enabled = false;
                    btnSearch.Text = SEARCHBUTTONRESETTEXT;
                }
            }
            catch (XmlException xmlEx)
            {
                CommonUtility.HandleException(strParentSiteURL, xmlEx);
            }
            catch (SoapException soapEx)
            {
                lblException.Text = soapEx.Message;
                lblException.Visible = true;
                ExceptionBlock.Visible = true;
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
        /// Event handler for click of cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.Compare(strMode,EDIT, true) == 0)
                {
                    Page.Response.Redirect(MAINTAINCHAPTERURL + "?" + BOOKIDQUERYSTRING + "=" + hdnListitemStatus.Value, false);
                }
                else
                {
                    Page.Response.Redirect(MAINTAINCHAPTERURL + "?" + BOOKIDQUERYSTRING + "=" + HttpContext.Current.Request.QueryString[BOOKIDQUERYSTRING], false);
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
        /// Event handler for click of Ok.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdOK_Click(object sender, EventArgs e)
        {

            string strActionPerformed = string.Empty;
            bool blnChapterTitleExists = true;

            try
            {
                if (string.Equals(strMode, ADD))
                {
                    strActionPerformed = AUDITACTIONCREATION;
                    blnChapterTitleExists = CheckDuplicateChapter(DWBCHAPTERLIST, txtChapterTitle.Text.Trim(), string.Empty, strBookID, ADD);
                }
                else if (string.Equals(strMode, EDIT))
                {
                    strActionPerformed = AUDITACTIONUPDATION;
                    blnChapterTitleExists = CheckDuplicateChapter(DWBCHAPTERLIST, txtChapterTitle.Text.Trim(), strSelectedID, hdnListitemStatus.Value, EDIT);
                }

                if (!blnChapterTitleExists)
                {
                    SetListEntry();
                    UpdateListEntry(objListEntry, DWBCHAPTERLIST, CHAPTERAUDITLIST, CHAPTER,
                        strActionPerformed);
                    if (string.Compare(strMode,EDIT, true) == 0)
                    {
                        Page.Response.Redirect(MAINTAINCHAPTERURL + "?BookId=" + hdnListitemStatus.Value, false);
                    }
                    else
                    {
                        Page.Response.Redirect(MAINTAINCHAPTERURL + "?BookId=" + HttpContext.Current.Request.QueryString[BOOKIDQUERYSTRING], false);
                    }
                }
                else
                {
                    lblException.Text = PAGETITLEEXISTSMSG;
                    lblException.Visible = true;
                    ExceptionBlock.Visible = true;
                    return;
                }
            }
            catch (XmlException xmlEx)
            {

                CommonUtility.HandleException(strParentSiteURL, xmlEx);

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
        /// Event fired when selected Asset type is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cboAssetType_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                lstAssetValues.Items.Clear();
                string strSelectedAsset = cboAssetType.SelectedItem.Text;
                GetAssetColumns(REPORTSERVICECOLUMN);
                cboColumnName.Visible = true;
                string strcamlQuery = @"<Where><And><And><Eq><FieldRef Name='Asset_Type' />
                    <Value Type='Lookup'>" + strSelectedAsset + "</Value></Eq><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq></And> <Eq>" +
             "<FieldRef Name='Has_MasterPage' /><Value Type='Choice'>Yes</Value></Eq></And></Where>";
                BindDataToControls(cboTemplate, TEMPLATELIST, CHAPTER, strcamlQuery);
                cboTemplate.Items.Insert(0, DROPDOWNDEFAULTTEXT);
            }            
            catch (WebException webEx)
            {

                lblException.Text = webEx.Message;
                lblException.Visible = true;
                ExceptionBlock.Visible = true;

            }
            catch (SoapException soapEx)
            {

                lblException.Text = soapEx.Message;
                lblException.Visible = true;
                ExceptionBlock.Visible = true;

            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strParentSiteURL, ex);
            }
        }

        #endregion Protected Methods - Event Handlers

        #region Private Methods
        /// <summary>
        /// Binds the UI controls.
        /// </summary>
        /// <param name="objMasterData">The obj well book data.</param>
        private void BindUIControls()
        {
            txtChapterTitle.Text = objListEntry.ChapterDetails.ChapterTitle;

            txtDescription.Text = objListEntry.ChapterDetails.ChapterDescription;
            lblChapter.Text = lblChapter.Text + " : " + objListEntry.ChapterDetails.ChapterTitle;          
            cboAssetType.ClearSelection();

            BindDropDownList(cboAssetType, objListEntry.ChapterDetails.AssetType);
            cboAssetType.Enabled = false;
            cboTemplate.ClearSelection();
            txtCriteria.Text = objListEntry.ChapterDetails.Criteria;

            if (string.Equals(strMode, EDIT) || string.Equals(strMode, VIEW))
            {
                string strSelectedAsset = cboAssetType.SelectedItem.Text;
                string strCamlQuery = @"<Where><Eq><FieldRef Name='Asset_Type' />
                    <Value Type='Lookup'>" + strSelectedAsset + "</Value></Eq></Where>";
                BindDataToControls(cboTemplate, TEMPLATELIST, CHAPTER, strCamlQuery);
                GetAssetColumns(REPORTSERVICECOLUMN);
                BindCountryDropDownList(cboDWBCountry, objListEntry.ChapterDetails.Country);
                BindDropDownList(cboColumnName, objListEntry.ChapterDetails.ColumnName);
                LoadAssetValue();

                if (cboTemplate != null && cboTemplate.Items.FindByValue(objListEntry.ChapterDetails.TemplateID.ToString()) != null)
                {
                    cboTemplate.ClearSelection();
                    cboTemplate.Items.FindByValue(objListEntry.ChapterDetails.TemplateID.ToString()).Selected = true;
                }
                cboDWBCountry.Enabled = false;
                cboColumnName.Enabled = false;
                txtCriteria.Enabled = false;
                btnSearch.Text = SEARCHBUTTONRESETTEXT;
                btnSearch.Visible = false;
                lstAssetValues.Enabled = false;
            }

            cboTemplate.Enabled = false;

            hdnListitemStatus.Value = Convert.ToString(objListEntry.ChapterDetails.BookID);
        }

        /// <summary>
        /// Loads the asset value.
        /// </summary>
        private void LoadAssetValue()
        {
            Shell.SharePoint.DREAM.Business.Entities.RequestInfo objRequestInfo = null;
            try
            {
                string strXpath = string.Empty;
                string strSelectedAsset = objListEntry.ChapterDetails.AssetType;
                objChapter = new ChapterBLL();
                //objRequestInfo = SetQuickDataObjects(cboDWBCountry.SelectedItem.Text, cboColumnName.SelectedItem.Value, txtCriteria.Text);
                objRequestInfo = SetQuickDataObjects(cboDWBCountry.SelectedItem.Value, cboColumnName.SelectedItem.Value, txtCriteria.Text);
                XmlDocument assetNodeList = objChapter.GetAssetValueForAssetType(objRequestInfo, strSelectedAsset);
                lblAssetValue.Text = strSelectedAsset;
                AssignValuesToDropDown(assetNodeList, lstAssetValues);
                BindListBoxValue(lstAssetValues, objListEntry.ChapterDetails.AssetValue);
            }
            catch (XmlException xmlEx)
            {
                CommonUtility.HandleException(strParentSiteURL, xmlEx);
            }
            catch (SoapException soapEx)
            {
                lblException.Text = soapEx.Message;
                lblException.Visible = true;
                ExceptionBlock.Visible = true;
            }
            catch (WebException webEx)
            {
                lblException.Text = webEx.Message;
                lblException.Visible = true;
                ExceptionBlock.Visible = true;
            }
        }

        /// <summary>
        /// Binds the list box value.
        /// </summary>
        /// <param name="lstAssetValues">ListBox.</param>
        /// <param name="assetValue">The asset value.</param>
        private void BindListBoxValue(ListBox lstAssetValues, string assetValue)
        {
            for (int intIndex = 0; intIndex < lstAssetValues.Items.Count; intIndex++)
            {
                if (string.Equals(lstAssetValues.Items[intIndex].Text, assetValue))
                {
                    lstAssetValues.ClearSelection();
                    lstAssetValues.Items[intIndex].Selected = true;
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
            for (int intIndex = 0; intIndex < dropDown.Items.Count; intIndex++)
            {
                if (string.Equals(dropDown.Items[intIndex].Text, value))
                {
                    dropDown.ClearSelection();
                    dropDown.Items[intIndex].Selected = true;
                }
            }
        }

        /// <summary>
        /// Binds the drop down list.
        /// </summary>
        /// <param name="dropDown">The drop down.</param>
        /// <param name="value">The value.</param>
        private void BindCountryDropDownList(DropDownList dropDown, string value)
        {
            //for (int intIndex = 0; intIndex < dropDown.Items.Count; intIndex++)
            //{
            //    if (string.Equals(dropDown.Items[intIndex].Text, value))
            //    {
            //        dropDown.Items[intIndex].Selected = true;
            //    }
            //}
            if (dropDown != null)
            {
                if (dropDown.Items.FindByValue(value) != null)
                {
                    dropDown.ClearSelection();
                    dropDown.Items.FindByValue(value).Selected = true;
                }
                else if (dropDown.Items.FindByText(value) != null)
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
            object objUserPreference = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString());
            string strUserPreferenceCountry = string.Empty;
            string strUserPreferenceAsset = string.Empty;
            DREAMDataObjects.UserPreferences objDreamUserPreference = null;
            if (objUserPreference != null)
            {
                objDreamUserPreference = (DREAMDataObjects.UserPreferences)objUserPreference;
                if (objDreamUserPreference != null)
                {
                    strUserPreferenceCountry = objDreamUserPreference.Country;
                    strUserPreferenceAsset = objDreamUserPreference.Asset;
                }
            }
            SetCountryList(strUserPreferenceCountry);            
            SetListValues(cboAssetType, ASSETTYPELIST);
            if (!string.IsNullOrEmpty(strUserPreferenceAsset))
            {
                if (cboAssetType != null && cboAssetType.Items.FindByText(strUserPreferenceAsset) != null)
                {
                    cboAssetType.ClearSelection();
                    cboAssetType.Items.FindByText(strUserPreferenceAsset).Selected = true;
                }
            }

            /// Populate Template list based on asset type           
            if (cboAssetType.SelectedIndex > 0)
            {               
                string strCamlQuery = @"<Where><And><And><Eq><FieldRef Name='Asset_Type' />
                    <Value Type='Lookup'>" + strUserPreferenceAsset + "</Value></Eq><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq></And> <Eq>" +
             "<FieldRef Name='Has_MasterPage' /><Value Type='Choice'>Yes</Value></Eq></And></Where>";
                BindDataToControls(cboTemplate, TEMPLATELIST, CHAPTER, strCamlQuery);
                cboTemplate.Items.Insert(0, DROPDOWNDEFAULTTEXT);
            }            
            /// Populate Column list based on asset type            
            GetAssetColumns(REPORTSERVICECOLUMN);
            SetUIControlForMode();
        }

        /// <summary>
        /// Sets the country list.
        /// </summary>
        /// <param name="userPreferenceCountry">The user preference country.</param>
        private void SetCountryList(string userPreferenceCountry)
        {
            DataTable dtListData = null;
            DataRow drListData;
            ListItem lstItem = null;
            try
            {                              
                string strCamlQuery = "<OrderBy><FieldRef Name=\"Title\" /></OrderBy><Where><Eq><FieldRef Name=\"Active\" /><Value Type=\"Choice\">Yes</Value></Eq></Where>";
             
                objCommonBLL = new CommonBLL();
                dtListData = objCommonBLL.ReadList(strParentSiteURL, COUNTRYLIST, strCamlQuery);
                if (dtListData.Rows.Count > 0)
                {
                    /// Loop through the values in Country List and finds the index of country user preference in List.
                    cboDWBCountry.Items.Clear();
                    cboDWBCountry.Items.Add(COUNTRYDROPDOWNDEFAULTTEXT);
                    for (int index = 0; index < dtListData.Rows.Count; index++)
                    {
                        drListData = dtListData.Rows[index];
                        lstItem = new ListItem();
                        lstItem.Text = drListData[DWBTITLECOLUMN].ToString();
                        lstItem.Value = drListData["Country_x0020_Code"].ToString();
                        cboDWBCountry.Items.Add(lstItem);
                    }

                    if (!string.IsNullOrEmpty(userPreferenceCountry))
                    {
                        if (cboDWBCountry.Items.FindByValue(userPreferenceCountry) != null)
                        {
                            cboDWBCountry.ClearSelection();
                            cboDWBCountry.Items.FindByValue(userPreferenceCountry).Selected = true;
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
        /// Hides controls based on the mode.
        /// </summary>
        private void SetUIControlForMode()
        {
            switch (strMode)
            {
                case EDIT:
                    {
                        lblChapter.Text = EDITCHAPTER;
                        this.Page.Title = EDITCHAPTER;
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
            objListEntry.ChapterDetails = SetChapterDetails();
            SetDWBPageDetails();
        }

        /// <summary>
        /// Sets the Chapter details data object from the UI.
        /// </summary>
        /// <returns>ChapterDetails object.</returns>
        private ChapterDetails SetChapterDetails()
        {
            ChapterDetails objChapterDetails = new ChapterDetails();
            CommonUtility objCommonUtilities = new CommonUtility();

            int intTemplateID = 0;
            objChapterDetails.ChapterTitle = txtChapterTitle.Text.Trim();
            objChapterDetails.ChapterDescription = txtDescription.Text.Trim();
            int intBookID = 0;
            int.TryParse(strBookID, out intBookID);
            objChapterDetails.BookID = intBookID;
            if (cboDWBCountry.SelectedItem != null)
            {
                //objChapterDetails.Country = cboDWBCountry.SelectedItem.Text;
                objChapterDetails.Country = cboDWBCountry.SelectedItem.Value;
            }
            if (cboColumnName.SelectedItem != null)
            {
                objChapterDetails.ColumnName = cboColumnName.SelectedItem.Text;
            }
            if (txtCriteria.Text.Length > 0)
            {
                objChapterDetails.Criteria = txtCriteria.Text.Trim();
            }
            if (cboAssetType.SelectedItem != null && !cboAssetType.SelectedItem.Value.Contains(DROPDONWDEFAULT))
            {
                objChapterDetails.AssetType = cboAssetType.SelectedValue;
            }
            if (lstAssetValues.SelectedItem != null)
            {
                objChapterDetails.AssetValue = lstAssetValues.SelectedItem.Text;
                objChapterDetails.ActualAssetValue = lstAssetValues.SelectedItem.Value;
            }
            if (cboTemplate.SelectedItem != null && !cboTemplate.SelectedItem.Value.Contains(DROPDONWDEFAULT))
            {
                int.TryParse(cboTemplate.SelectedValue, out intTemplateID);
                objChapterDetails.TemplateID = intTemplateID;
            }
            int intRowId = 0;
            if (string.Compare(strMode,EDIT, true) == 0)
            {
                if (int.TryParse(strSelectedID, out intRowId))
                {
                    objChapterDetails.RowID = intRowId;
                    objChapterDetails.BookID = int.Parse(hdnListitemStatus.Value);
                }
            }
            else
            {
                objChapterDetails.Terminated = STATUSACTIVE;
            }
            return objChapterDetails;
        }

        /// <summary>
        /// Sets the ChapterPagesMapping collection based on the selected Book and Chapter.
        /// </summary>
        /// <returns></returns>
        private void SetDWBPageDetails()
        {
            int intTemplateID = 0;
            string strCAMLQuery = string.Empty;
            ChapterPagesMapping objChapterPageMapping = null;
            DataTable dtresult = null;
            int intRowId = 0;
            string strTitleTemplate = string.Empty;
            string strConnectionType = string.Empty;

            List<ChapterPagesMapping> listChapterPageMapping = new List<ChapterPagesMapping>();
            List<StoryBoard> listStoryBoard = new List<StoryBoard>();
            if (cboTemplate.SelectedItem != null && !cboTemplate.SelectedItem.Value.Contains(DROPDONWDEFAULT))
            {
                int.TryParse(cboTemplate.SelectedValue, out intTemplateID);
                strCAMLQuery = @"<Where><Eq><FieldRef Name='Template_ID' /><Value Type='Number'>" + Convert.ToString(intTemplateID) + "</Value></Eq></Where>";
                dtresult = GetListItems(TEMPLATEPAGESMAPPINGLIST, strCAMLQuery, string.Empty);
                if (dtresult != null && dtresult.Rows.Count > 0)
                {
                    for (int intRowIndex = 0; intRowIndex < dtresult.Rows.Count; intRowIndex++)
                    {

                        objChapterPageMapping = new ChapterPagesMapping();
                        objChapterPageMapping.PageName = Convert.ToString(dtresult.Rows[intRowIndex]["Master_Page_Name"]);

                        strTitleTemplate = Convert.ToString(dtresult.Rows[intRowIndex]["Page_Title_Template"]);
                        if (!string.IsNullOrEmpty(strTitleTemplate))
                        {
                            objChapterPageMapping.PageActualName = strTitleTemplate;
                        }
                        objChapterPageMapping.AssetType = Convert.ToString(dtresult.Rows[intRowIndex]["Asset_Type"]);
                        objChapterPageMapping.Discipline = Convert.ToString(dtresult.Rows[intRowIndex]["Discipline"]);
                        strConnectionType = Convert.ToString(dtresult.Rows[intRowIndex]["Connection_Type"]);
                        objChapterPageMapping.ConnectionType = strConnectionType;
                        if (!strConnectionType.Contains(CONNECTIONTYPEI))
                        {
                            objChapterPageMapping.Empty = STATUSTERMINATED;
                        }
                        objChapterPageMapping.PageURL = Convert.ToString(dtresult.Rows[intRowIndex]["Page_URL"]);
                        objChapterPageMapping.StandardOperatingProc = Convert.ToString(dtresult.Rows[intRowIndex]["Standard_Operating_Procedure"]);
                        objChapterPageMapping.PageOwner = Convert.ToString(dtresult.Rows[intRowIndex]["Page_Owner"]);
                        int.TryParse(Convert.ToString(dtresult.Rows[intRowIndex][DWBIDCOLUMN]), out intRowId);
                        objChapterPageMapping.MasterPageID = intRowId;
                        objChapterPageMapping.RowId = intRowId;
                        int.TryParse(Convert.ToString(dtresult.Rows[intRowIndex]["Page_Sequence"]), out intRowId);
                        objChapterPageMapping.PageSequence = intRowId;
                        listChapterPageMapping.Add(objChapterPageMapping);

                    }
                }


            }
            if (objListEntry != null)
            {
                objListEntry.ChapterPagesMapping = listChapterPageMapping;
            }
            if (dtresult != null)
            {
                dtresult.Dispose();
            }
        }

        /// <summary>
        /// Gets the asset columns.
        /// </summary>
        /// <param name="list name">The list name.</param>
        private void GetAssetColumns(string listName)
        {
            DataTable dtListData = null;
            DataRow drListRData;
            try
            {
                objCommonBLL = new CommonBLL();
                string strColumnName = string.Empty;
                string strDisplayName = string.Empty;
                string strParentSiteURL = SPContext.Current.Site.Url.ToString();
                string strCamlQueryAssetColumns = string.Empty;
                strCamlQueryAssetColumns = GetCamlQueryColumnList(strCamlQueryAssetColumns);
                dtListData = objCommonBLL.ReadList(strParentSiteURL, listName, strCamlQueryAssetColumns);
                if (dtListData.Rows.Count > 0)
                {
                    cboColumnName.Items.Clear();
                    for (int intRowIndex = 0; intRowIndex < dtListData.Rows.Count; intRowIndex++)
                    {
                        drListRData = dtListData.Rows[intRowIndex];
                        strColumnName = drListRData[DWBTITLECOLUMN].ToString().Trim();
                        strDisplayName = drListRData["Display_x0020_Name"].ToString().Trim();
                        cboColumnName.Items.Add(strDisplayName);
                        cboColumnName.Items[intRowIndex].Value = strColumnName;
                    }
                }
                dtListData.Clear();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (dtListData != null)
                    dtListData.Dispose();
            }
        }

        /// <summary>
        /// Get caml query based on asset type
        /// </summary>
        /// <param name="camlQuery">The caml query.</param>
        /// <returns></returns>
        private string GetCamlQueryColumnList(string camlQuery)
        {
            if (string.Equals(cboAssetType.SelectedItem.Text.ToLowerInvariant(), ASSETTYPEWELL.ToLowerInvariant()))
            {
                camlQuery = "<Where><Eq><FieldRef Name='Asset_x0020_Type' /><Value Type='Text'>Well</Value></Eq></Where>";
            }
            else if (string.Equals(cboAssetType.SelectedItem.Text.ToLowerInvariant(), ASSETTYPEWELLBORE.ToLowerInvariant()))
            {
                camlQuery = "<Where><Eq><FieldRef Name='Asset_x0020_Type' /><Value Type='Text'>Wellbore</Value></Eq></Where>";
            }
            else if (string.Equals(cboAssetType.SelectedItem.Text.ToLowerInvariant(), ASSETTYPEFIELD.ToLowerInvariant()))
            {
                camlQuery = "<Where><Eq><FieldRef Name='Asset_x0020_Type' /><Value Type='Text'>Field</Value></Eq></Where>";
            }
            return camlQuery;
        }

        /// <summary>
        /// Assigns the values to drop down.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="document">The XML document.</param>
        /// <param name="listControl">The list control for which data needs to be populated.</param>
        private void AssignValuesToDropDown(XmlDocument document, ListBox listControl)
        {
            listControl.Items.Clear();
            XmlNodeList xmlMainNodeList = null;
            XmlNodeList xmlSubNodeList = null;
            ListItem lstItem = null;
            /// Check all the required objects are NULL
            if (listControl != null && document != null)
            {
                xmlMainNodeList = document.SelectNodes(REPORTXPATH);
                if (xmlMainNodeList != null)
                {
                    foreach (XmlNode xmlMainNode in xmlMainNodeList)
                    {
                        /// Check Current Field Exists
                        xmlSubNodeList = document.SelectNodes(RECORDXPATH);

                        if (xmlSubNodeList != null)
                        {
                            /// If sub node has value then added it to DropDown Box
                            foreach (XmlNode xmlSubNode in xmlSubNodeList)
                            {
                                lstItem = new ListItem();
                                lstItem.Text = xmlSubNode.ChildNodes[0].Attributes.GetNamedItem(VALUE).InnerText.ToString();
                                lstItem.Value = xmlSubNode.ChildNodes[1].Attributes.GetNamedItem(VALUE).InnerText.ToString();
                                listControl.Items.Add(lstItem);
                            }
                        }/// Sub Node List Not Null                       
                    }/// FOR EACH
                }/// Main Node List Not Null
                else
                {
                    throw new Exception("XML Nodes not Found");
                }
            }/// Main IF
            else
            {
                throw new Exception("Invalid Arguments");
            }
        }
        #endregion Private Methods
    }
}
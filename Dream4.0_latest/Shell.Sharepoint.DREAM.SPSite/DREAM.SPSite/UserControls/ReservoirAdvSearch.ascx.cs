#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: ReservoirAdvSearch.ascx.cs 
#endregion

using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Web.Services.Protocols;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Xsl;

using ShellEntities = Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;
using Telerik.Web.UI;

namespace Shell.SharePoint.DREAM.Site.UI.UserControls
{
    /// <summary>
    /// Reservoir The  UI Class for advance search criteria screen
    /// </summary>
    public partial class ReservoirAdvSearch : UIControlHandler
    {


        #region Declaration
        const string RESERVOIRPAGEURL = "AdvSearchReservoir.aspx";
        const string RESERVOIRCHECKREFRESH = "ReservoirCheckRefresh";
        ShellEntities.Entity objEntity = new ShellEntities.Entity();
        string strCurrSiteURL = string.Empty;
        bool blnSearchClick;
        string strLithologyMainValue = string.Empty;
        string strLithologySecondaryValue = string.Empty;
        string strLithostratGroupValue = string.Empty;
        string strLithostratFormationValue = string.Empty;
        string strLithostratMemberValue = string.Empty;
        string strProductionStatusValue = string.Empty;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ReservoirAdvSearch"/> class.
        /// </summary>
        public ReservoirAdvSearch()
        {
            strCurrSiteURL = HttpContext.Current.Request.Url.ToString();
        }

        #region Event Handlers
        #region PageLoad
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            blnSearchClick = false;
            cmdSearch.Attributes.Add(REDIRECTATTRIBUTE, REDIRECTATTRIBUTEVALUE);
            cmdReset.Attributes.Add(REDIRECTATTRIBUTE, REDIRECTATTRIBUTEVALUE);
            cmdSearchButton.Attributes.Add(REDIRECTATTRIBUTE, REDIRECTATTRIBUTEVALUE);
            cmdResetButton.Attributes.Add(REDIRECTATTRIBUTE, REDIRECTATTRIBUTEVALUE);
            if (!Page.IsPostBack)
            {


                try
                {
                    AttatchJavascriptToControls();
                    /// Sets the Reservoir Check refresh session variable.
                    CommonUtility.SetSessionVariable(Page, RESERVOIRCHECKREFRESH, DateTime.Now.ToString());

                    base.SearchType = RESERVOIRSEARCHTYPE;
                    lblException.Text = string.Empty;
                    LoadControls(chbShared, cboSavedSearch, cboGrainSizeMean, cboDriveMechanism, cboHydrocarbonMain, cboHydrocarbonSecondary, rdblSearchCond);
                    GetAssetColumns(REPORTSERVICECOLUMNLIST, cboSearchCriteria, RESERVOIRITEMVAL);
                    PopulateRadComboControls(true);

                    if (Request.QueryString[SAVESEARCHNAMEQUERYSTRING] != null)
                    {
                        if (Request.QueryString[OPERATIONQUERYSTRING] != null)
                        {
                            if (string.Equals(Request.QueryString[OPERATIONQUERYSTRING].ToString().ToLowerInvariant(), "modify".ToLowerInvariant()))
                            {
                                cmdSaveSearch.Value = MODIFYSRCH;
                                txtSaveSearch.Text = Request.QueryString[SAVESEARCHNAMEQUERYSTRING].ToString();
                                txtSaveSearch.Enabled = false;
                            }
                        }
                    }

                    BindTooltipTextToControls();
                }
                catch (WebException webEx)
                {
                    lblException.Visible = true;
                    lblException.Text = webEx.Message;
                }
                catch (SoapException soapEx)
                {
                    if (!string.Equals(soapEx.Message.ToString().ToLowerInvariant(), SOAPEXCEPTIONMESSAGE.ToLowerInvariant()))
                    {
                        CommonUtility.HandleException(strCurrSiteURL, soapEx, 1);
                    }
                    lblException.Visible = true;
                    lblException.Text = soapEx.Message.ToString();
                }
                catch (Exception ex)
                {
                    CommonUtility.HandleException(strCurrSiteURL, ex);
                }
            }


        }

        /// <summary>
        /// Handles the Unload event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (radCboLithologyMain != null)
                radCboLithologyMain.Dispose();
            if (radCboLithologySecondary != null)
                radCboLithologySecondary.Dispose();
            if (radCboLithostratFormation != null)
                radCboLithostratFormation.Dispose();
            if (radCboLithostratGroup != null)
                radCboLithostratGroup.Dispose();
            if (radCboLithostratMember != null)
                radCboLithostratMember.Dispose();
        }
        #endregion

        /// <summary>
        /// Handles the PreRender event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_PreRender(object sender, EventArgs e)
        {

            object objCheckRefresh = CommonUtility.GetSessionVariable(Page, RESERVOIRCHECKREFRESH);
            /// If the session has the CheckRefresh value then assign to ViewState.
            if (objCheckRefresh != null)
                ViewState[CHECKREFRESH] = (string)objCheckRefresh;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cboSavedSearch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cboSavedSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            int intSelectedSaveSearchIndex = 0;
            string strSelectedSaveSearchName = string.Empty;
            try
            {
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                string strUserID = GetUserName();
                XmlDocument xmldoc = new XmlDocument();
                xmldoc = ((MOSSServiceManager)objMossController).GetDocLibXMLFile(RESERVOIRSEARCHTYPE, strUserID);
                intSelectedSaveSearchIndex = cboSavedSearch.SelectedIndex;
                if (cboSavedSearch.SelectedIndex != 0)
                {
                    strSelectedSaveSearchName = cboSavedSearch.SelectedItem.Text;
                }
                ClearUIControls();
                lblException.Visible = false;
                if ((intSelectedSaveSearchIndex != 0) || (string.Equals(cmdSaveSearch.Value.ToString().ToLowerInvariant(), MODIFYSRCH.ToLowerInvariant())))
                {
                    BindSearchConditionOperator(xmldoc, strSelectedSaveSearchName, rdblSearchCond);
                    BindUIControls(xmldoc, strSelectedSaveSearchName, chbShared);
                    PopulateRadComboControls(true);
                    cboSavedSearch.SelectedIndex = intSelectedSaveSearchIndex;
                    if (string.Equals(cmdSaveSearch.Value.ToString().ToLowerInvariant(), MODIFYSRCH.ToLowerInvariant()))
                    {
                        txtSaveSearch.Text = cboSavedSearch.Text;
                    }
                }
            }
            catch (WebException webEx)
            {
                lblException.Visible = true;
                lblException.Text = webEx.Message;
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteURL, ex);
            }
        }

        /// <summary>
        /// Handles the Click event of the cmdSearch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmdSearch_Click(object sender, EventArgs e)
        {

            try
            {
                AssignRadComboValues();
                blnSearchClick = true;
                if (Page.IsPostBack)
                {
                    DisplaySearchResults();
                }
            }
            catch (WebException webEx)
            {
                lblException.Visible = true;
                lblException.Text = webEx.Message;
            }
            catch (SoapException soapEx)
            {
                if (!string.Equals(soapEx.Message.ToString().ToLowerInvariant(), SOAPEXCEPTIONMESSAGE.ToLowerInvariant()))
                {
                    CommonUtility.HandleException(strCurrSiteURL, soapEx, 1);
                }
                lblException.Visible = true;
                lblException.Text = soapEx.Message.ToString();
            }
            catch (Exception ex)
            {
                if (string.Equals(ex.Message.ToLowerInvariant(), BLANKFILEMESSAGE.ToLowerInvariant()))
                {
                    lblException.Visible = true;
                    lblException.Text = BLANKFILEMESSAGE;
                }
                else
                    CommonUtility.HandleException(strCurrSiteUrl, ex);
            }

        }

        /// <summary>
        /// Handles the Click event of the cmdReset control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmdReset_Click(object sender, EventArgs e)
        {
            StringBuilder strUrl = new StringBuilder();
            if (string.Equals(cmdSaveSearch.Value.ToString().ToLowerInvariant(), MODIFYSRCH.ToLowerInvariant()))
            {
                if (Request.QueryString[SAVESEARCHNAMEQUERYSTRING] != null)
                {
                    strUrl.Append(RESERVOIRPAGEURL);
                    strUrl.Append("?asset=Field&manage=true&savesearchname=");
                    strUrl.Append(Request.QueryString[SAVESEARCHNAMEQUERYSTRING].ToString());
                    strUrl.Append("&operation=modify");
                }
                else
                {
                    strUrl.Append(RESERVOIRPAGEURL);
                    strUrl.Append("?asset=Field");
                }
            }
            else
            {
                strUrl.Append(RESERVOIRPAGEURL);
                strUrl.Append("?asset=Field");
            }
            RedirectPage(strUrl.ToString(), string.Empty);
        }
        #region Rad Combo Items Requested Event
        /// <summary>
        /// Handles the ItemsRequested event of the cmdRadCboLithologyMainComboBox control.
        /// </summary>
        /// <param name="o">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs"/> instance containing the event data.</param>
        protected void cmdRadCboLithologyMainComboBox_ItemsRequested(object o, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
        {

            try
            {
                PopulateRadComboBox(e, radCboLithologyMain, LITHOLOGYMAINLIST, "Title", "Title", true);
            }
            catch (SoapException soapEx)
            {
                if (soapEx.Message.Equals(SOAPEXCEPTIONMESSAGE))
                {

                    e.Message = NORECORDFOUNDSEXCEPTIONMESSAGE;
                }
                else
                {
                    e.Message = UNEXPECTEDEXCEPTIONMESSAGE;
                    CommonUtility.HandleException(strCurrSiteURL, soapEx, 1);
                }
            }

            catch (WebException webEx)
            {
                e.Message = UNEXPECTEDEXCEPTIONMESSAGE;
                CommonUtility.HandleException(strCurrSiteURL, webEx, 1);
            }
            catch (Exception ex)
            {
                e.Message = UNEXPECTEDEXCEPTIONMESSAGE;
                CommonUtility.HandleException(strCurrSiteURL, ex);
            }


        }

        /// <summary>
        /// Handles the ItemsRequested event of the cmdRadLithostratGroupComboBox control.
        /// </summary>
        /// <param name="o">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs"/> instance containing the event data.</param>
        protected void cmdRadLithostratGroupComboBox_ItemsRequested(object o, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
        {

            try
            {
                PopulateRadComboBox(e, radCboLithostratGroup, LITHOSTRATGROUPLIST, FIELDVALUE, FIELDNAME, true);
            }
            catch (SoapException soapEx)
            {
                if (soapEx.Message.Equals(SOAPEXCEPTIONMESSAGE))
                {

                    e.Message = NORECORDFOUNDSEXCEPTIONMESSAGE;
                }
                else
                {
                    e.Message = UNEXPECTEDEXCEPTIONMESSAGE;
                    CommonUtility.HandleException(strCurrSiteURL, soapEx, 1);
                }
            }

            catch (WebException webEx)
            {
                e.Message = UNEXPECTEDEXCEPTIONMESSAGE;
                CommonUtility.HandleException(strCurrSiteURL, webEx, 1);
            }
            catch (Exception ex)
            {
                e.Message = UNEXPECTEDEXCEPTIONMESSAGE;
                CommonUtility.HandleException(strCurrSiteURL, ex);
            }
        }

        /// <summary>
        /// Handles the ItemsRequested event of the cmdRadLithostratFormationComboBox control.
        /// </summary>
        /// <param name="o">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs"/> instance containing the event data.</param>
        protected void cmdRadLithostratFormationComboBox_ItemsRequested(object o, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
        {
            radCboLithostratFormation.Items.Clear();
            try
            {
                PopulateRadComboBox(e, radCboLithostratFormation, LITHOSTRATFORMATION, LITHOSTRATFORMATIONDATAFIELD, LITHOSTRATFORMATIONDATAFIELD, true);
            }
            catch (SoapException soapEx)
            {
                if (soapEx.Message.Equals(SOAPEXCEPTIONMESSAGE))
                {

                    e.Message = NORECORDFOUNDSEXCEPTIONMESSAGE;
                    e.EndOfItems = true;
                    e.NumberOfItems = 0;
                    if (radCboLithostratFormation.FindItemByText(DEFAULTDROPDOWNTEXT) == null)
                    {
                        RadComboBoxItem radComboItem = new RadComboBoxItem(DEFAULTDROPDOWNTEXT);
                        radCboLithostratFormation.Items.Insert(0, radComboItem);
                    }
                    radCboLithostratFormation.SelectedIndex = 0;
                }
                else
                {
                    RadComboBoxItem radComboItem = new RadComboBoxItem(DEFAULTDROPDOWNTEXT);
                    radCboLithostratFormation.Items.Insert(0, radComboItem);
                    e.Message = UNEXPECTEDEXCEPTIONMESSAGE;
                    e.EndOfItems = true;
                    e.NumberOfItems = 0;
                    radCboLithostratFormation.SelectedIndex = 0;
                    CommonUtility.HandleException(strCurrSiteURL, soapEx, 1);
                }
            }

            catch (WebException webEx)
            {
                RadComboBoxItem radComboItem = new RadComboBoxItem(DEFAULTDROPDOWNTEXT);
                radCboLithostratFormation.Items.Insert(0, radComboItem);
                e.Message = UNEXPECTEDEXCEPTIONMESSAGE;
                radCboLithostratFormation.SelectedIndex = 0;
                CommonUtility.HandleException(strCurrSiteURL, webEx, 1);
            }
            catch (Exception ex)
            {
                RadComboBoxItem radComboItem = new RadComboBoxItem(DEFAULTDROPDOWNTEXT);
                radCboLithostratFormation.Items.Insert(0, radComboItem);
                e.Message = UNEXPECTEDEXCEPTIONMESSAGE;
                e.EndOfItems = true;
                e.NumberOfItems = 0;
                radCboLithostratFormation.SelectedIndex = 0;
                CommonUtility.HandleException(strCurrSiteURL, ex);
            }

        }

        /// <summary>
        /// Handles the ItemsRequested event of the cmdRadLithostratMemberComboBox control.
        /// </summary>
        /// <param name="o">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs"/> instance containing the event data.</param>
        protected void cmdRadLithostratMemberComboBox_ItemsRequested(object o, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
        {

            radCboLithostratMember.Items.Clear();
            try
            {
                PopulateRadComboBox(e, radCboLithostratMember, LITHOSTRATMEMBER, LITHOSTRATMEMBERDATAFIELD, LITHOSTRATMEMBERDATAFIELD, true);
            }
            catch (SoapException soapEx)
            {
                if (soapEx.Message.Equals(SOAPEXCEPTIONMESSAGE))
                {

                    e.Message = NORECORDFOUNDSEXCEPTIONMESSAGE;
                    e.EndOfItems = true;
                    e.NumberOfItems = 0;
                    RadComboBoxItem radComboItem = new RadComboBoxItem(DEFAULTDROPDOWNTEXT);
                    radCboLithostratMember.Items.Insert(0, radComboItem);
                    radCboLithostratMember.SelectedIndex = 0;
                }
                else
                {
                    e.Message = UNEXPECTEDEXCEPTIONMESSAGE;
                    e.EndOfItems = true;
                    e.NumberOfItems = 0;
                    CommonUtility.HandleException(strCurrSiteURL, soapEx, 1);
                }
            }
            catch (WebException webEx)
            {
                e.Message = UNEXPECTEDEXCEPTIONMESSAGE;
                e.EndOfItems = true;
                e.NumberOfItems = 0;
                CommonUtility.HandleException(strCurrSiteURL, webEx, 1);
            }
            catch (Exception ex)
            {
                e.Message = UNEXPECTEDEXCEPTIONMESSAGE;
                e.EndOfItems = true;
                e.NumberOfItems = 0;
                CommonUtility.HandleException(strCurrSiteURL, ex);
            }

        }

        /// <summary>
        /// Handles the ItemsRequested event of the cmdRadLithologySecondaryComboBox control.
        /// </summary>
        /// <param name="o">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs"/> instance containing the event data.</param>
        protected void cmdRadLithologySecondaryComboBox_ItemsRequested(object o, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                PopulateRadComboBox(e, radCboLithologySecondary, LITHOLOGYSECONDARYLIST, "Secondary", "Secondary", true);
            }
            catch (SoapException soapEx)
            {
                if (soapEx.Message.Equals(SOAPEXCEPTIONMESSAGE))
                {

                    e.Message = NORECORDFOUNDSEXCEPTIONMESSAGE;
                    e.EndOfItems = true;
                    e.NumberOfItems = 0;
                }
                else
                {
                    e.Message = UNEXPECTEDEXCEPTIONMESSAGE;
                    e.EndOfItems = true;
                    e.NumberOfItems = 0;
                    CommonUtility.HandleException(strCurrSiteURL, soapEx, 1);
                }
            }

            catch (WebException webEx)
            {
                e.Message = UNEXPECTEDEXCEPTIONMESSAGE;
                e.EndOfItems = true;
                e.NumberOfItems = 0;
                CommonUtility.HandleException(strCurrSiteURL, webEx, 1);
            }
            catch (Exception ex)
            {
                e.Message = UNEXPECTEDEXCEPTIONMESSAGE;
                e.EndOfItems = true;
                e.NumberOfItems = 0;
                CommonUtility.HandleException(strCurrSiteURL, ex);
            }
        }

        #endregion

        #region Save Request Xml
        /// <summary>
        /// Events fires on Save search button click.
        /// </summary>
        protected void cmdSaveSearch_Click(object sender, EventArgs e)
        {
            bool blnIsNameExist = false;
            try
            {
                AssignRadComboValues();

                if (string.Equals(cmdSaveSearch.Value.ToString().ToLowerInvariant(), MODIFYSRCH.ToLowerInvariant()))
                {
                    lblException.Visible = false;
                    objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                    object objCheckRefresh = CommonUtility.GetSessionVariable(Page, RESERVOIRCHECKREFRESH);
                    //Checks whether the event is fired or page has been refreshed.
                    if (string.Equals(ViewState[CHECKREFRESH].ToString(), (string)objCheckRefresh))
                    {
                        CommonUtility.SetSessionVariable(Page, RESERVOIRCHECKREFRESH, DateTime.Now.ToString());
                        string strSaveSearchName = txtSaveSearch.Text.Trim();
                        XmlDocument xmlDocSearchRequest = null;
                        xmlDocSearchRequest = CreateSaveSearchXML();
                        UISaveSearchHandler objUISaveSearchHandler = null;
                        try
                        {
                            objUISaveSearchHandler = new UISaveSearchHandler();
                            objUISaveSearchHandler.ModifySaveSearchXML(RESERVOIRSEARCHTYPE, strSaveSearchName, chbShared.Checked, cboSavedSearch, xmlDocSearchRequest);
                            if (cboSavedSearch.Items.FindByText(DEFAULTDROPDOWNTEXT) == null)
                            {
                                cboSavedSearch.Items.Insert(0, DEFAULTDROPDOWNTEXT);
                            }
                        }
                        catch (WebException webEx)
                        {
                            lblException.Visible = true;
                            lblException.Text = webEx.Message;
                        }
                        catch (Exception ex)
                        {
                            lblException.Visible = true;
                            lblException.Text = ex.Message;
                            CommonUtility.HandleException(strCurrSiteURL, ex);
                        }
                    }
                    else
                    {
                        cboSavedSearch.Items.Clear();
                        cboSavedSearch.Items.Add(DEFAULTDROPDOWNTEXT);
                        ((MOSSServiceManager)objMossController).LoadSaveSearch(RESERVOIRSEARCHTYPE, cboSavedSearch);
                    }
                    cboSavedSearch.Text = txtSaveSearch.Text.ToString().Trim();
                    txtSaveSearch.Text = string.Empty;
                    txtSaveSearch.Enabled = true;
                    cmdSaveSearch.Value = SAVESRCH;
                }
                else
                {
                    lblException.Visible = false;
                    ArrayList arlSavedSearch = new ArrayList();
                    objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                    object objCheckRefresh = CommonUtility.GetSessionVariable(Page, RESERVOIRCHECKREFRESH);
                    /// Checks whether the event is fired or page has been refreshed.
                    if (string.Equals(ViewState[CHECKREFRESH].ToString(), (string)objCheckRefresh))
                    {
                        CommonUtility.SetSessionVariable(Page, RESERVOIRCHECKREFRESH, DateTime.Now.ToString());
                        string strSaveSearchName = txtSaveSearch.Text.Trim();
                        /// Check for Duplicate Name Exist
                        arlSavedSearch = ((MOSSServiceManager)objMossController).GetSaveSearchName(RESERVOIRSEARCHTYPE, GetUserName());
                        if (IsDuplicateNameExist(arlSavedSearch, strSaveSearchName))
                        {
                            lblException.Visible = true;
                            lblException.Text = ((MOSSServiceManager)objMossController).GetCustomExceptionMessage(strCurrSiteURL, "12");
                            blnIsNameExist = true;
                        }
                        else
                        {
                            /// Create Save Search.
                            XmlDocument xmlDocSearchRequest = null;
                            xmlDocSearchRequest = CreateSaveSearchXML();
                            UISaveSearchHandler objUISaveSearchHandler = null;
                            try
                            {
                                objUISaveSearchHandler = new UISaveSearchHandler();
                                objUISaveSearchHandler.SaveSearchXMLToLibrary(RESERVOIRSEARCHTYPE, strSaveSearchName, chbShared.Checked, cboSavedSearch, xmlDocSearchRequest);
                            }
                            catch (WebException webEx)
                            {
                                lblException.Visible = true;
                                lblException.Text = webEx.Message;
                            }
                            catch (Exception ex)
                            {
                                lblException.Visible = true;
                                lblException.Text = ex.Message;
                                CommonUtility.HandleException(strCurrSiteURL, ex);
                            }
                        }
                    }
                    else
                    {
                        cboSavedSearch.Items.Clear();
                        cboSavedSearch.Items.Add(DEFAULTDROPDOWNTEXT);
                        ((MOSSServiceManager)objMossController).LoadSaveSearch(RESERVOIRSEARCHTYPE, cboSavedSearch);
                    }
                    if (!blnIsNameExist)
                        cboSavedSearch.Text = txtSaveSearch.Text.ToString().Trim();
                    txtSaveSearch.Text = string.Empty;
                    PopulateRadComboControls(false);
                }
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteURL, ex);
            }

        }
        #endregion

        #endregion

        #region Private Methods

        #region Search Results
        /// <summary>
        /// Displays the search results from response xml to ui.
        /// </summary>
        private void DisplaySearchResults()
        {
            objRequestInfo = new ShellEntities.RequestInfo();
            objRequestInfo = SetBasicDataObjects();
            UISaveSearchHandler objUISaveSearch = new UISaveSearchHandler();
            objUISaveSearch.DisplayResults(Page, objRequestInfo, RESERVOIRSEARCHTYPE);
            StringBuilder strSearchResultsPage = new StringBuilder();
            strSearchResultsPage.Append(SEARCHRESULTSPAGE);
            strSearchResultsPage.Append("?SearchType=");
            strSearchResultsPage.Append(RESERVOIRSEARCHTYPE);
            strSearchResultsPage.Append("&asset=");
            strSearchResultsPage.Append(RESERVOIRITEMVAL);
            RedirectPage(strSearchResultsPage.ToString(), RESERVOIRITEMVAL);
        }
        #endregion

        #region ToolTips Binding to Controls
        /// <summary>
        /// Method is use for Bind tool tips to the controls
        /// </summary>
        private void BindTooltipTextToControls()
        {
            DataTable dtFilterTooltip = null;
            string strControlName = string.Empty;
            string strTooltipText = string.Empty;
            string strAsset = RESERVOIRITEMVAL;

            try
            {
                dtFilterTooltip = new DataTable();
                dtFilterTooltip = AssignToolTip();
                dtFilterTooltip = GetFilterDataTable(dtFilterTooltip, strAsset);
                if (dtFilterTooltip.Rows.Count > 0)
                {
                    foreach (DataRow drFilter in dtFilterTooltip.Rows)
                    {
                        strControlName = drFilter["Title"].ToString();
                        strTooltipText = drFilter["Tooltip_X0020_Text"].ToString();
                        if (string.Equals(imgSavedSearch.ID.Remove(0, 3).ToString(), strControlName))
                        {
                            imgSavedSearch.ToolTip = strTooltipText;
                        }
                        else if (imgGrainSizeMean.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgGrainSizeMean.ToolTip = strTooltipText;
                            imgGrainSizeMean.Attributes.Add(JAVASCRIPTONCLICK, string.Format(TOOLTIPPOPUPFUNCTION, imgGrainSizeMean.ID.Remove(0, 3)));
                        }
                        else if (imgIdentifier.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgIdentifier.ToolTip = strTooltipText;
                        }
                        else if (imgLithologyMain.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgLithologyMain.ToolTip = strTooltipText;
                        }
                        else if (imgLithologySecondary.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgLithologySecondary.ToolTip = strTooltipText;
                        }
                        else if (imgLithostratGroup.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgLithostratGroup.ToolTip = strTooltipText;
                            imgLithostratGroup.Attributes.Add(JAVASCRIPTONCLICK, string.Format(TOOLTIPPOPUPFUNCTION, imgLithostratGroup.ID.Remove(0, 3)));
                        }
                        else if (imgLithostratFormation.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgLithostratFormation.ToolTip = strTooltipText;
                            imgLithostratFormation.Attributes.Add(JAVASCRIPTONCLICK, string.Format(TOOLTIPPOPUPFUNCTION, imgLithostratFormation.ID.Remove(0, 3)));
                        }
                        else if (imgLithostratMember.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgLithostratMember.ToolTip = strTooltipText;
                            imgLithostratMember.Attributes.Add(JAVASCRIPTONCLICK, string.Format(TOOLTIPPOPUPFUNCTION, imgLithostratMember.ID.Remove(0, 3)));
                        }
                        else if (imgGasInitiallyInPlace.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgGasInitiallyInPlace.ToolTip = strTooltipText;
                            imgGasInitiallyInPlace.Attributes.Add(JAVASCRIPTONCLICK, string.Format(TOOLTIPPOPUPFUNCTION, imgGasInitiallyInPlace.ID.Remove(0, 3)));
                        }
                        else if (imgStockTankOilInitially.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgStockTankOilInitially.ToolTip = strTooltipText;
                            imgStockTankOilInitially.Attributes.Add(JAVASCRIPTONCLICK, string.Format(TOOLTIPPOPUPFUNCTION, imgStockTankOilInitially.ID.Remove(0, 3)));
                        }
                        else if (imgProductionStatus.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgProductionStatus.ToolTip = strTooltipText;
                        }
                        else if (imgSearchName.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgSearchName.ToolTip = strTooltipText;
                        }
                        else if (imgDriveMechanism.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgDriveMechanism.ToolTip = strTooltipText;
                        }
                        else if (imgHydrocarbonMain.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgHydrocarbonMain.ToolTip = strTooltipText;
                        }
                        else if (imgHydrocarbonSecondary.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgHydrocarbonSecondary.ToolTip = strTooltipText;
                        }
                        else if (imgOilViscosity.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgOilViscosity.ToolTip = strTooltipText;
                        }
                        else if (imgPressureReservoirInitial.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgPressureReservoirInitial.ToolTip = strTooltipText;
                        }
                        else if (imgOilGravity.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgOilGravity.ToolTip = strTooltipText;
                            imgOilGravity.Attributes.Add(JAVASCRIPTONCLICK, string.Format(TOOLTIPPOPUPFUNCTION, imgOilGravity.ID.Remove(0, 3)));
                        }
                    }
                }

                if (radCboProductionStatus != null && radCboProductionStatus.SelectedItem != null)
                {
                    radCboProductionStatus.ToolTip = radCboProductionStatus.SelectedItem.Text;
                }
            }
            finally { if (dtFilterTooltip != null)dtFilterTooltip.Dispose(); }
        }

        #endregion

        #region Respose xml Generation and Transformation
        /// <summary>
        /// Query the names from webservice
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private DataTable GetDataSource(string type, string value)
        {
            if (string.Equals(type.ToLowerInvariant(), LITHOLOGYMAINLIST.ToLowerInvariant()))
            {
                return GetList(type, value);

            }
            else if (string.Equals(type.ToLowerInvariant(), LITHOLOGYSECONDARYLIST.ToLowerInvariant()))
            {
                return GetList(type, value);
            }
            else if (string.Equals(type.ToLowerInvariant(), PRODUCTIONSTATUSLIST.ToLowerInvariant()))
            {
                return GetList(type, value);
            }
            else
            {
                ArrayList arrListValue = new ArrayList();
                objUIUtilities = new UIUtilities();
                objReportController = objFactory.GetServiceManager(REPORTSERVICE);
                XmlDocument xmlDoc = new XmlDocument();

                if (string.Equals(type.ToLowerInvariant(), LITHOSTRATGROUPLIST.ToLowerInvariant()))
                {
                    arrListValue.Add(radCboLithostratGroup);
                    SetListValues(arrListValue, value, LITHOSTRATGROUPLIST);
                    xmlDoc = objReportController.GetSearchResults(objRequestInfo, intListValMaxRecord, LITHOSTRATGROUPLIST, null, 0);
                    return GetRadDataSource(xmlDoc, GetXslForRadResponseXml(), LITHOSTRATGROUPLIST).Tables[3];

                }
                else if (string.Equals(type.ToLowerInvariant(), LITHOSTRATFORMATION.ToLowerInvariant()))
                {
                    arrListValue.Add(radCboLithostratFormation);
                    SetListValues(arrListValue, value, LITHOSTRATFORMATION);
                    xmlDoc = objReportController.GetSearchResults(objRequestInfo, intListValMaxRecord, LITHOSTRATFORMATION, null, 0);

                    return GetRadDataSource(xmlDoc, GetXslForRadResponseXml(), LITHOSTRATFORMATION).Tables[3];
                }
                else if (string.Equals(type.ToLowerInvariant(), LITHOSTRATMEMBER.ToLowerInvariant()))
                {
                    arrListValue.Add(radCboLithostratMember);
                    SetListValues(arrListValue, value, LITHOSTRATMEMBER);
                    xmlDoc = objReportController.GetSearchResults(objRequestInfo, intListValMaxRecord, LITHOSTRATMEMBER, null, 0);

                    return GetRadDataSource(xmlDoc, GetXslForRadResponseXml(), LITHOSTRATMEMBER).Tables[3];
                }


            }

            return null;
        }



        /// <summary>
        ///  Query the Basin names from webservice
        /// </summary>
        /// <param name="XmlDocument">The Search Respose xml for Rad Control</param>
        /// <param name="XmlTextReader">The TextReader object which contains XSl file  </param>
        private DataSet GetRadDataSource(XmlDocument objResponseXml, XmlTextReader objXmlTextReader, string fieldName)
        {
            XmlDocument xmlDocRadTransformerXSL = new XmlDocument();
            StringWriter objStringWriter = new StringWriter();
            XslCompiledTransform objCompiledTransform = new XslCompiledTransform();
            XsltArgumentList xsltArgsList = new XsltArgumentList();
            DataSet dsOutput = new DataSet();

            switch (fieldName)
            {
                case FIELD:
                    {
                        xsltArgsList.AddParam("PARAM1", string.Empty, "Field Name");
                        xsltArgsList.AddParam("PARAM2", string.Empty, "Field Identifier");
                        break;
                    }
                case FIELDOPERATOR:
                    {
                        xsltArgsList.AddParam("PARAM1", string.Empty, "Field Name");
                        xsltArgsList.AddParam("PARAM2", string.Empty, "Field Identifier");
                        break;
                    }
                case LITHOSTRATGROUPLIST:
                    {
                        xsltArgsList.AddParam("PARAM1", string.Empty, "Lithostrat Group");
                        break;
                    }
                case LITHOSTRATFORMATION:
                    {
                        xsltArgsList.AddParam("PARAM1", string.Empty, "Lithostrat Formation");
                        break;
                    }
                case LITHOSTRATMEMBER:
                    {
                        xsltArgsList.AddParam("PARAM1", string.Empty, "Lithostrat Member");
                        break;
                    }

            }
            xmlDocRadTransformerXSL.Load(objXmlTextReader);
            objCompiledTransform.Load(xmlDocRadTransformerXSL);
            objCompiledTransform.Transform(new XmlNodeReader(objResponseXml), xsltArgsList, objStringWriter);
            objResponseXml.LoadXml(objStringWriter.ToString());
            objStringWriter.Flush();
            objStringWriter.Close();
            objStringWriter.Dispose();
            dsOutput.ReadXml(new XmlNodeReader(objResponseXml));
            objXmlTextReader.Close();
            return dsOutput;
        }

        /// <summary>
        /// Loads the lithostrat group controls.
        /// </summary>
        private void LoadLithostratGroup(string listName, string filteredValue, RadComboBox radCboLithostratGroup, string dataTextField, string dataValueField)
        {
            DataTable dtLithostratGroup = null;
            try
            {
                dtLithostratGroup = GetDataSource(listName, filteredValue);
                if (dtLithostratGroup != null && dtLithostratGroup.Rows.Count > 0)
                {
                    radCboLithostratGroup.DataSource = dtLithostratGroup;
                    radCboLithostratGroup.DataTextField = dataTextField;
                    radCboLithostratGroup.DataValueField = dataValueField;
                    radCboLithostratGroup.DataBind();
                    radCboLithostratGroup.Items.Insert(0, new RadComboBoxItem(DEFAULTDROPDOWNTEXT));
                    radCboLithostratGroup.SelectedIndex = 0;
                }
                else
                {
                    radCboLithostratGroup.ErrorMessage = NORECORDFOUNDSEXCEPTIONMESSAGE;
                }
            }
            catch (SoapException soapEx)
            {
                if (soapEx.Message.Equals(SOAPEXCEPTIONMESSAGE))
                {
                    radCboLithostratGroup.ErrorMessage = NORECORDFOUNDSEXCEPTIONMESSAGE;
                }
                else
                {
                    radCboLithostratGroup.ErrorMessage = UNEXPECTEDEXCEPTIONMESSAGE;
                    CommonUtility.HandleException(strCurrSiteURL, soapEx, 1);
                }
            }

            catch (WebException webEx)
            {
                radCboLithostratGroup.ErrorMessage = UNEXPECTEDEXCEPTIONMESSAGE;
                CommonUtility.HandleException(strCurrSiteURL, webEx, 1);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteURL, ex);
            }
            finally
            {
                if (dtLithostratGroup != null)
                {
                    if (dtLithostratGroup.DataSet != null)
                        dtLithostratGroup.DataSet.Dispose();
                    dtLithostratGroup.Dispose();
                }
            }
        }

        /// <summary>
        /// Populates the RAD combo box.
        /// </summary>
        /// <param name="e">The <see cref="Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs"/> instance containing the event data.</param>
        /// <param name="sourceListName">Name of the source list.</param>
        /// <param name="valueFieldName">Name of the value field.</param>
        /// <param name="textFieldName">Name of the text field.</param>
        protected void PopulateRadComboBox(RadComboBoxItemsRequestedEventArgs e, RadComboBox objRadComboBox, string sourceListName, string valueFieldName, string textFieldName, bool populateFromSharePointList)
        {
            DataTable dtFieldName = null;
            StringBuilder strUserEnteredValue = new StringBuilder();
            try
            {
                if (!string.IsNullOrEmpty(e.Text))
                {
                    if (e.Text == "empty")
                    {
                        e.Text = string.Empty;
                    }
                    strUserEnteredValue.Append(e.Text);
                    if (!populateFromSharePointList)
                    {
                        strUserEnteredValue.Append("*");
                    }
                    dtFieldName = GetDataSource(sourceListName, strUserEnteredValue.ToString());

                    if (dtFieldName != null && dtFieldName.Rows.Count > 0)
                    {
                        int intItemsPerRequest = 111;
                        int intItemOffset = e.NumberOfItems;
                        int intEndOffset = Math.Min(intItemOffset + intItemsPerRequest, dtFieldName.Rows.Count);
                        e.EndOfItems = intEndOffset == dtFieldName.Rows.Count;
                        objRadComboBox.DataSource = dtFieldName;
                        objRadComboBox.DataTextField = textFieldName;
                        objRadComboBox.DataValueField = valueFieldName;
                        objRadComboBox.DataBind();
                        e.Message = dtFieldName.Rows.Count.ToString();
                        RadComboBoxItem radComboItem = new RadComboBoxItem(DEFAULTDROPDOWNTEXT);
                        objRadComboBox.Items.Insert(0, radComboItem);
                    }
                    else
                    {
                        e.Message = dtFieldName.Rows.Count.ToString();
                        RadComboBoxItem radComboItem = new RadComboBoxItem(DEFAULTDROPDOWNTEXT);
                        objRadComboBox.Items.Clear();
                        objRadComboBox.Items.Insert(0, radComboItem);
                        e.Message = NORECORDFOUNDSEXCEPTIONMESSAGE;
                    }
                }
            }
            finally
            {
                if (dtFieldName != null)
                    dtFieldName.Dispose();
            }
        }
        #endregion

        #region Request XML Generation
        /// <summary>
        /// Sets the list values.
        /// </summary>
        /// <param name="arrFields">The arr fields.</param>
        /// <param name="lbControl">The lb control.</param>
        /// <returns></returns>
        private ShellEntities.RequestInfo SetListValues(ArrayList fieldsGroup, string value, string entityName)
        {
            objRequestInfo.Entity = SetEntity(fieldsGroup, value, entityName);
            return objRequestInfo;
        }

        /// <summary>
        /// Sets the entity.
        /// </summary>
        /// <param name="arrFields">The arr fields.</param>
        /// <param name="userEnteredValue">The User Entered Value</param>
        /// <returns></returns>
        private ShellEntities.Entity SetEntity(ArrayList fieldsGroup, string userEnteredValue, string entityName)
        {
            ShellEntities.Entity objEntity = new ShellEntities.Entity();

            if (fieldsGroup.Count == 1 && fieldsGroup[0] != null)
            {
                Control radControl = (Control)fieldsGroup[0];
                ArrayList arlAttribute = new ArrayList();
                ShellEntities.Attributes objAttribute = new ShellEntities.Attributes();
                objAttribute.Name = GetRadControlID(radControl.ID);
                ArrayList arlValue = new ArrayList();
                ShellEntities.Value objValue = new ShellEntities.Value();
                objValue.InnerText = userEnteredValue;
                arlValue.Add(objValue);
                objAttribute.Value = arlValue;
                objAttribute.Operator = GetOperator(objAttribute.Value);
                arlAttribute.Add(objAttribute);
                objEntity.Attribute = arlAttribute;
                objEntity.Criteria = SetCriteria();
                objEntity.Name = entityName;
            }
            else
            {
                objEntity.Criteria = SetCriteria();
            }
            return objEntity;
        }

        /// <summary>
        /// Sets the criteria.
        /// </summary>
        /// <returns></returns>
        private ShellEntities.Criteria SetCriteria()
        {
            ShellEntities.Criteria objCriteria = new ShellEntities.Criteria();
            objCriteria.Value = STAROPERATOR;
            objCriteria.Operator = GetOperator(objCriteria.Value);
            return objCriteria;
        }

        /// <summary>
        /// Sets the basic data objects to create XML document
        /// </summary>
        /// <param name="strRequestInfo">The requestinfo search type.</param>
        /// <returns></returns>
        private ShellEntities.RequestInfo SetBasicDataObjects()
        {
            objRequestInfo = new ShellEntities.RequestInfo();

            objRequestInfo.Entity = SetEntity();
            return objRequestInfo;
        }

        /// <summary>
        /// Sets the entity.
        /// </summary>
        /// <returns></returns>
        private ShellEntities.Entity SetEntity()
        {

            objEntity = new ShellEntities.Entity();
            objEntity.Name = RESERVOIRSEARCHTYPE;
            ArrayList arlBasicAttributeGroup = new ArrayList();
            ArrayList arlAttribute = new ArrayList();
            ArrayList arlAttributeGroup = new ArrayList();
            string strSearchPercentageValue = PortalConfiguration.GetInstance().GetKey(SEARCHPERCENTAGEVALUE);

            arlAttribute = SetBasicAttribute();
            if (blnSearchClick)
            {
                arlAttributeGroup = SetSRPAttributeGroup(strSearchPercentageValue, arlAttributeGroup);
            }
            if (arlAttribute.Count > 1)
            {
                arlBasicAttributeGroup = SetBasicAttributeGroup(arlAttribute);
                if (arlAttributeGroup.Count > 0)
                {
                    arlAttributeGroup.Add(arlBasicAttributeGroup[0]);
                    ShellEntities.AttributeGroup objFinalAttributeGroup = new ShellEntities.AttributeGroup();
                    objFinalAttributeGroup.Operator = GetLogicalOperator(rdblSearchCond.SelectedValue);
                    objFinalAttributeGroup.AttributeGroups = arlAttributeGroup;
                    ArrayList newArrayList = new ArrayList();
                    newArrayList.Add(objFinalAttributeGroup);
                    objEntity.AttributeGroups = newArrayList;
                }
                else
                {
                    objEntity.AttributeGroups = arlBasicAttributeGroup;
                }
            }
            else
            {
                objEntity.Attribute = arlAttribute;
                if (arlAttributeGroup.Count > 0)
                {
                    ShellEntities.AttributeGroup objFinalAttributeGroup = new ShellEntities.AttributeGroup();
                    objFinalAttributeGroup.Operator = GetLogicalOperator(rdblSearchCond.SelectedValue);
                    objFinalAttributeGroup.AttributeGroups = arlAttributeGroup;
                    ArrayList newArrayList = new ArrayList();
                    newArrayList.Add(objFinalAttributeGroup);
                    objEntity.AttributeGroups = newArrayList;
                }
            }


            return objEntity;
        }

        /// <summary>
        /// Sets the basic attribute group.
        /// </summary>
        /// <returns></returns>
        private ArrayList SetBasicAttributeGroup(ArrayList arlAttribute)
        {
            ArrayList arrBasicAttributeGroup = new ArrayList();

            ShellEntities.AttributeGroup objBasicAttributeGroup = new ShellEntities.AttributeGroup();
            objBasicAttributeGroup.Operator = GetLogicalOperator(rdblSearchCond.SelectedValue);
            objBasicAttributeGroup.Attribute = arlAttribute;
            arrBasicAttributeGroup.Add(objBasicAttributeGroup);

            return arrBasicAttributeGroup;
        }

        /// <summary>
        /// Sets the attribute node for request xml.
        /// </summary>
        /// <returns></returns>
        private ArrayList SetBasicAttribute()
        {
            string strSearchPercentageVAlue = PortalConfiguration.GetInstance().GetKey(SEARCHPERCENTAGEVALUE);
            ArrayList arlAttribute = new ArrayList();

            /// checks if file based search criteria is selected or not. 
            if (cboSearchCriteria.SelectedIndex != 0)
            {
                /// overloaded method to search a file for identifier values. Returns an arraylist of Attributes objects
                arlAttribute = SetUITextControls(txtReservoir_Identifier, arlAttribute, ReadFileSearch(fileUploader.PostedFile, hidWordContent.Value), cboSearchCriteria);
                if (arlAttribute.Count == 0)
                {
                    throw new Exception(BLANKFILEMESSAGE);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(txtReservoir_Identifier.Text.Trim()))
                {
                    arlAttribute = SetUITextControls(txtReservoir_Identifier, arlAttribute);
                }
            }

            if (cboGrainSizeMean.SelectedIndex != 0)
            {
                arlAttribute = SetUITextControls(cboGrainSizeMean, arlAttribute);
            }
            if (radCboProductionStatus.SelectedIndex != 0 && radCboProductionStatus.SelectedItem != null)
            {
                arlAttribute = SetUITextControls(radCboProductionStatus, arlAttribute, radCboProductionStatus.SelectedItem.Text);
            }
            if (cboDriveMechanism.SelectedIndex != 0)
            {
                arlAttribute = SetUITextControls(cboDriveMechanism, arlAttribute);
            }
            if (cboHydrocarbonMain.SelectedIndex != 0)
            {
                /// If Search click add the "Value" field to Request XML
                if (blnSearchClick)
                {
                    arlAttribute = SetUITextControls(cboHydrocarbonMain, arlAttribute);
                }
                else /// Else for Save Search add "Text" field to Request XML since needed to rebind the values
                {
                    arlAttribute = SetUITextControls(cboHydrocarbonMain, arlAttribute, true);
                }
            }
            if (cboHydrocarbonSecondary.SelectedIndex != 0)
            {
                arlAttribute = SetUITextControls(cboHydrocarbonSecondary, arlAttribute);
            }
            /// strLithologyMainValue contains Select; as default value.
            /// Set for selected index before setting this.
            if (!string.IsNullOrEmpty(strLithologyMainValue))
            {
                arlAttribute = SetUITextControls(radCboLithologyMain, arlAttribute, strLithologyMainValue);
            }
            if (!string.IsNullOrEmpty(strLithologySecondaryValue))
            {
                arlAttribute = SetUITextControls(radCboLithologySecondary, arlAttribute, strLithologySecondaryValue);
            }
            if (!string.IsNullOrEmpty(strLithostratGroupValue))
            {
                arlAttribute = SetUITextControls(radCboLithostratGroup, arlAttribute, strLithostratGroupValue);
            }
            if (!string.IsNullOrEmpty(strLithostratFormationValue))
            {
                arlAttribute = SetUITextControls(radCboLithostratFormation, arlAttribute, strLithostratFormationValue);
            }
            if (!string.IsNullOrEmpty(strLithostratMemberValue))
            {
                arlAttribute = SetUITextControls(radCboLithostratMember, arlAttribute, strLithostratMemberValue);
            }
            if (!string.IsNullOrEmpty(txtChronostratigraphy.Text.Trim()))
            {
                arlAttribute = SetUITextControls(txtChronostratigraphy, arlAttribute);
            }

            arlAttribute = GetDepositionalAttribute(arlAttribute);


            if (!blnSearchClick)
            {
                if (!string.IsNullOrEmpty(txtGasInitiallyInPlace.Text.Trim()))
                {
                    arlAttribute = SetUITextControls(txtGasInitiallyInPlace, arlAttribute, true, strSearchPercentageVAlue, blnSearchClick);
                }
                if (!string.IsNullOrEmpty(txtStockTankOilInitially.Text.Trim()))
                {
                    arlAttribute = SetUITextControls(txtStockTankOilInitially, arlAttribute, true, strSearchPercentageVAlue, blnSearchClick);
                }
                if (!string.IsNullOrEmpty(txtRecoveryFactorOil.Text.Trim()))
                {
                    arlAttribute = SetUITextControls(txtRecoveryFactorOil, arlAttribute, true, strSearchPercentageVAlue, blnSearchClick);
                }
                if (!string.IsNullOrEmpty(txtRecoveryFactorGas.Text.Trim()))
                {
                    arlAttribute = SetUITextControls(txtRecoveryFactorGas, arlAttribute, true, strSearchPercentageVAlue, blnSearchClick);
                }
                if (!string.IsNullOrEmpty(txtRecoverableOil.Text.Trim()))
                {
                    arlAttribute = SetUITextControls(txtRecoverableOil, arlAttribute, true, strSearchPercentageVAlue, blnSearchClick);
                }
                if (!string.IsNullOrEmpty(txtRecoverableGas.Text.Trim()))
                {
                    arlAttribute = SetUITextControls(txtRecoverableGas, arlAttribute, true, strSearchPercentageVAlue, blnSearchClick);
                }

                if (!string.IsNullOrEmpty(txtOilViscosity.Text.Trim()))
                {
                    arlAttribute = SetUITextControls(txtOilViscosity, arlAttribute, true, strSearchPercentageVAlue, blnSearchClick);
                }
                if (!string.IsNullOrEmpty(txtPressureReservoirInitial.Text.Trim()))
                {
                    arlAttribute = SetUITextControls(txtPressureReservoirInitial, arlAttribute, true, strSearchPercentageVAlue, blnSearchClick);
                }
                if (!string.IsNullOrEmpty(txtOilGravity.Text.Trim()))
                {
                    arlAttribute = SetUITextControls(txtOilGravity, arlAttribute, true, strSearchPercentageVAlue, blnSearchClick);
                }
                if (!string.IsNullOrEmpty(txtPermeability_Max.Text.Trim()))
                {
                    arlAttribute = SetUITextControls(txtPermeability_Max, arlAttribute, true, strSearchPercentageVAlue, blnSearchClick);
                }
                if (!string.IsNullOrEmpty(txtPermeability_Min.Text.Trim()))
                {
                    arlAttribute = SetUITextControls(txtPermeability_Min, arlAttribute, true, strSearchPercentageVAlue, blnSearchClick);
                }
                if (!string.IsNullOrEmpty(txtPorosityMax.Text.Trim()))
                {
                    arlAttribute = SetUITextControls(txtPorosityMax, arlAttribute, true, strSearchPercentageVAlue, blnSearchClick);
                }
                if (!string.IsNullOrEmpty(txtPorosityMin.Text.Trim()))
                {
                    arlAttribute = SetUITextControls(txtPorosityMin, arlAttribute, true, strSearchPercentageVAlue, blnSearchClick);
                }
                if (!string.IsNullOrEmpty(txtSandReservoir_Net_Gross_avg.Text.Trim()))
                {
                    arlAttribute = SetUITextControls(txtSandReservoir_Net_Gross_avg, arlAttribute, true, strSearchPercentageVAlue, blnSearchClick);
                }
                if (!string.IsNullOrEmpty(txtSandReservoir_Net_Gross_max.Text.Trim()))
                {
                    arlAttribute = SetUITextControls(txtSandReservoir_Net_Gross_max, arlAttribute, true, strSearchPercentageVAlue, blnSearchClick);
                }
                if (!string.IsNullOrEmpty(txtSandReservoir_Net_Gross_min.Text.Trim()))
                {
                    arlAttribute = SetUITextControls(txtSandReservoir_Net_Gross_min, arlAttribute, true, strSearchPercentageVAlue, blnSearchClick);
                }
                if (!string.IsNullOrEmpty(txtWaterSaturation_max.Text.Trim()))
                {
                    arlAttribute = SetUITextControls(txtWaterSaturation_max, arlAttribute, true, strSearchPercentageVAlue, blnSearchClick);
                }
            }


            return arlAttribute;
        }

        private ArrayList GetDepositionalAttribute(ArrayList arlAttribute)
        {
            ShellEntities.Attributes objAttribute = new ShellEntities.Attributes();
            ArrayList arlValue = new ArrayList();
            /// Change the Attribute.Name property
            /// Name will vary based on the selected Depositional level
            /// This value is return from Depositional Popup along with selected value and stored in hidDepositionalColumn control
            if (!string.IsNullOrEmpty(hidDepositionalColumn.Value) && !string.IsNullOrEmpty(txtDepositionalEnv.Text.Trim()) && !string.IsNullOrEmpty(hidDepositionalValue.Value))
            {
                /// If Search Click, include attribute only for the selected Depositional environment level and value
                /// Read value from hidDepositionalValue Hidden field, not from txtDepositionalEnv since it shows only display value
                if (txtDepositionalEnv.Text.Trim().Length > 0)
                {

                    objAttribute.Name = hidDepositionalColumn.Value;
                    arlValue.Add(SetValue(hidDepositionalValue.Value.Trim()));
                    objAttribute.Value = arlValue;
                    objAttribute.Operator = GetOperator(objAttribute.Value);
                    /// Fixed by Yasotha
                    /// Without lable name, gives "object reference" error in SearchResults.cs - GetSRPAdvanceSearchRequestXML 
                    objAttribute.Label = "none";
                    arlAttribute.Add(objAttribute);
                }

                if (!blnSearchClick)
                {

                    if (hidDepositionalValue.Value.Trim().Length > 0)
                    {
                        objAttribute = new ShellEntities.Attributes();
                        arlValue = new ArrayList();
                        /// Selected Depositional Selected Value attribute
                        objAttribute.Name = GetNodeIDFromControl(hidDepositionalValue.ID);
                        arlValue.Add(SetValue(hidDepositionalValue.Value.Trim()));
                        objAttribute.Value = arlValue;
                        objAttribute.Operator = GetOperator(objAttribute.Value);
                        objAttribute.Label = hidDepositionalValue.ID;
                        objAttribute.Checked = "Exclude";
                        arlAttribute.Add(objAttribute);

                    }
                    if (hidDepositionalColumn.Value.Trim().Length > 0)
                    {
                        objAttribute = new ShellEntities.Attributes();
                        arlValue = new ArrayList();
                        /// Selected Depositional Selected Column attribute
                        objAttribute.Name = GetNodeIDFromControl(hidDepositionalColumn.ID);
                        arlValue.Add(SetValue(hidDepositionalColumn.Value.Trim()));
                        objAttribute.Value = arlValue;
                        objAttribute.Operator = GetOperator(objAttribute.Value);
                        objAttribute.Label = hidDepositionalColumn.ID;
                        objAttribute.Checked = "Exclude";
                        arlAttribute.Add(objAttribute);

                    }
                    if (txtDepositionalEnv.Text.Trim().Length > 0)
                    {
                        /// Selected Depositional Selected Display attribute
                        arlAttribute = SetUITextControls(txtDepositionalEnv, arlAttribute);
                        if (arlAttribute != null && arlAttribute.Count > 0)
                        {
                            ((ShellEntities.Attributes)arlAttribute[arlAttribute.Count - 1]).Checked = "Exclude";
                        }
                    }
                }
            }
            return arlAttribute;
        }

        /// <summary>
        /// Creates the save search XML.
        /// </summary>
        private XmlDocument CreateSaveSearchXML()
        {
            XmlDocument xmlDocSearchRequest = null;
            objRequestInfo = new ShellEntities.RequestInfo();
            objReportController = objFactory.GetServiceManager(REPORTSERVICE);

            objRequestInfo = SetBasicDataObjects();
            xmlDocSearchRequest = objReportController.CreateSearchRequest(objRequestInfo);
            return xmlDocSearchRequest;
        }

        /// <summary>
        /// Sets the SRP attribute group.
        /// </summary>
        /// <param name="strSearchPercentageValue">The search percentage value.</param>
        /// <param name="arlAttribute">The arraylist of Attribute.</param>
        /// <returns></returns>
        private ArrayList SetSRPAttributeGroup(string searchPercentageValue, ArrayList attribute)
        {

            if (!string.IsNullOrEmpty(txtGasInitiallyInPlace.Text.Trim()))
            {
                attribute = SetUITextControls(txtGasInitiallyInPlace, attribute, true, searchPercentageValue, blnSearchClick);
            }
            if (!string.IsNullOrEmpty(txtStockTankOilInitially.Text.Trim()))
            {
                attribute = SetUITextControls(txtStockTankOilInitially, attribute, true, searchPercentageValue, blnSearchClick);
            }
            if (!string.IsNullOrEmpty(txtRecoveryFactorOil.Text.Trim()))
            {
                attribute = SetUITextControls(txtRecoveryFactorOil, attribute, true, searchPercentageValue, blnSearchClick);
            }
            if (!string.IsNullOrEmpty(txtRecoveryFactorGas.Text.Trim()))
            {
                attribute = SetUITextControls(txtRecoveryFactorGas, attribute, true, searchPercentageValue, blnSearchClick);
            }
            if (!string.IsNullOrEmpty(txtRecoverableOil.Text.Trim()))
            {
                attribute = SetUITextControls(txtRecoverableOil, attribute, true, searchPercentageValue, blnSearchClick);
            }
            if (!string.IsNullOrEmpty(txtRecoverableGas.Text.Trim()))
            {
                attribute = SetUITextControls(txtRecoverableGas, attribute, true, searchPercentageValue, blnSearchClick);
            }

            if (!string.IsNullOrEmpty(txtOilViscosity.Text.Trim()))
            {
                attribute = SetUITextControls(txtOilViscosity, attribute, true, searchPercentageValue, blnSearchClick);
            }
            if (!string.IsNullOrEmpty(txtPressureReservoirInitial.Text.Trim()))
            {
                attribute = SetUITextControls(txtPressureReservoirInitial, attribute, true, searchPercentageValue, blnSearchClick);
            }
            if (!string.IsNullOrEmpty(txtOilGravity.Text.Trim()))
            {
                attribute = SetUITextControls(txtOilGravity, attribute, true, searchPercentageValue, blnSearchClick);
            }
            if (!string.IsNullOrEmpty(txtPermeability_Max.Text.Trim()))
            {
                attribute = SetUITextControls(txtPermeability_Max, attribute, true, searchPercentageValue, blnSearchClick);
            }
            if (!string.IsNullOrEmpty(txtPermeability_Min.Text.Trim()))
            {
                attribute = SetUITextControls(txtPermeability_Min, attribute, true, searchPercentageValue, blnSearchClick);
            }
            if (!string.IsNullOrEmpty(txtPorosityMax.Text.Trim()))
            {
                attribute = SetUITextControls(txtPorosityMax, attribute, true, searchPercentageValue, blnSearchClick);
            }
            if (!string.IsNullOrEmpty(txtPorosityMin.Text.Trim()))
            {
                attribute = SetUITextControls(txtPorosityMin, attribute, true, searchPercentageValue, blnSearchClick);
            }
            if (!string.IsNullOrEmpty(txtSandReservoir_Net_Gross_avg.Text.Trim()))
            {
                attribute = SetUITextControls(txtSandReservoir_Net_Gross_avg, attribute, true, searchPercentageValue, blnSearchClick);
            }
            if (!string.IsNullOrEmpty(txtSandReservoir_Net_Gross_max.Text.Trim()))
            {
                attribute = SetUITextControls(txtSandReservoir_Net_Gross_max, attribute, true, searchPercentageValue, blnSearchClick);
            }
            if (!string.IsNullOrEmpty(txtSandReservoir_Net_Gross_min.Text.Trim()))
            {
                attribute = SetUITextControls(txtSandReservoir_Net_Gross_min, attribute, true, searchPercentageValue, blnSearchClick);
            }
            if (!string.IsNullOrEmpty(txtWaterSaturation_max.Text.Trim()))
            {
                attribute = SetUITextControls(txtWaterSaturation_max, attribute, true, searchPercentageValue, blnSearchClick);
            }
            return attribute;
        }

        #endregion

        #region Javascript binding to controls and Reset controls
        /// <summary>
        /// Assigns the RAD combo values.
        /// </summary>
        private void AssignRadComboValues()
        {
            strLithologyMainValue = string.Empty;
            strLithologySecondaryValue = string.Empty;
            strLithostratGroupValue = string.Empty;
            strLithostratFormationValue = string.Empty;
            strLithostratMemberValue = string.Empty;
            strProductionStatusValue = string.Empty;
            /// Check for selected index of RadCombox boxes before assigning values.
            strLithologyMainValue = GetRadComboValues(strLithologyMainValue, radCboLithologyMain);
            strLithologySecondaryValue = GetRadComboValues(strLithologySecondaryValue, radCboLithologySecondary);
            strLithostratGroupValue = GetRadComboValues(strLithostratGroupValue, radCboLithostratGroup);
            strLithostratFormationValue = GetRadComboValues(strLithostratFormationValue, radCboLithostratFormation);
            strLithostratMemberValue = GetRadComboValues(strLithostratMemberValue, radCboLithostratMember);
            /// This value is used in PopulateRadCombo method to re-assign the selected Production Status values after populating.
            strProductionStatusValue = GetRadComboValues(strProductionStatusValue, radCboProductionStatus);
            /*  if (string.IsNullOrEmpty(strLithologyMainValue.ToLowerInvariant()))
              {
                  StringBuilder strLithologyMain = new StringBuilder();

                  strLithologyMain.Append(radCboLithologySecondary.Text);
                  strLithologyMain.Append(";");
                  strLithologyMain.Append(radCboLithologySecondary.SelectedValue);
                  if (!string.IsNullOrEmpty(radCboLithologySecondary.Text))
                      strLithologyMainValue = strLithologyMain.ToString();

              }
              if (string.IsNullOrEmpty(strLithologySecondaryValue.ToLowerInvariant()))
              {
                  StringBuilder strLithologySecondary = new StringBuilder();            

                      strLithologySecondary.Append(radCboLithologySecondary.Text);
                      strLithologySecondary.Append(";");
                      strLithologySecondary.Append(radCboLithologySecondary.SelectedValue);
                      if (!string.IsNullOrEmpty(radCboLithologySecondary.Text))
                          strLithologySecondaryValue = strLithologySecondary.ToString();
               
              }
              if (string.IsNullOrEmpty(strLithostratGroupValue.ToLowerInvariant()))
              {
                  StringBuilder strLithostratGroup = new StringBuilder();
              
                      strLithostratGroup.Append(radCboLithostratGroup.Text);
                      strLithostratGroup.Append(";");
                      strLithostratGroup.Append(radCboLithostratGroup.SelectedValue);
                      if (!string.IsNullOrEmpty(radCboLithostratGroup.Text))
                          strLithostratGroupValue = strLithostratGroup.ToString();
               
              }
              if (string.IsNullOrEmpty(strLithostratFormationValue.ToLowerInvariant()))
              {
                  StringBuilder strLithostratFormation = new StringBuilder();
             
                      strLithostratFormation.Append(radCboLithostratFormation.Text);
                      strLithostratFormation.Append(";");
                      strLithostratFormation.Append(radCboLithostratFormation.SelectedValue);
                      if (!string.IsNullOrEmpty(radCboLithostratFormation.Text))
                          strLithostratFormationValue = strLithostratFormation.ToString();
             
              }
              if (string.IsNullOrEmpty(strLithostratMemberValue.ToLowerInvariant()))
              {
                  StringBuilder strLithostratMember = new StringBuilder();
                  strLithostratMember.Append(radCboLithostratMember.Text);
                  strLithostratMember.Append(";");
                  strLithostratMember.Append(radCboLithostratMember.SelectedValue);
                  if (!string.IsNullOrEmpty(radCboLithostratMember.Text))
                      strLithostratMemberValue = strLithostratMember.ToString();
              }*/
        }

        /// <summary>
        /// Gets the RAD combo values.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="objRadComboBox">The obj RAD combo box.</param>
        /// <returns></returns>
        private string GetRadComboValues(string value, RadComboBox objRadComboBox)
        {
            string strRadComboValue = string.Empty;
            if (string.IsNullOrEmpty(value.ToLowerInvariant()))
            {
                StringBuilder strRadComboSelectedValue = new StringBuilder();

                strRadComboSelectedValue.Append(objRadComboBox.Text);
                strRadComboSelectedValue.Append(";");
                strRadComboSelectedValue.Append(objRadComboBox.SelectedValue);
                if (!string.IsNullOrEmpty(objRadComboBox.Text))
                    strRadComboValue = strRadComboSelectedValue.ToString();

            }
            return strRadComboValue;
        }

        /// <summary>
        /// Populates the RAD combo controls.
        /// </summary>
        /// <param name="isPageLoad">if set to <c>true</c> [is page load].</param>
        private void PopulateRadComboControls(bool isPageLoad)
        {
            AssignRadComboValues();
            if (isPageLoad)
            {
                LoadLithostratGroup(LITHOLOGYMAINLIST, string.Empty, radCboLithologyMain, "Title", "Title");

                if (strLithologyMainValue.Contains(";"))
                    radCboLithologyMain.SelectedValue = strLithologyMainValue.Split(";".ToCharArray())[0];

                LoadLithostratGroup(LITHOSTRATGROUPLIST, ALLCOLUMNS, radCboLithostratGroup, LITHOSTRATGROUPDATAFIELD, LITHOSTRATGROUPDATAFIELD);
                LoadLithostratGroup(LITHOSTRATFORMATION, ALLCOLUMNS, radCboLithostratFormation, LITHOSTRATFORMATIONDATAFIELD, LITHOSTRATFORMATIONDATAFIELD);
                LoadLithostratGroup(LITHOSTRATMEMBER, ALLCOLUMNS, radCboLithostratMember, LITHOSTRATMEMBERDATAFIELD, LITHOSTRATMEMBERDATAFIELD);
                LoadLithostratGroup(PRODUCTIONSTATUSLIST, string.Empty, radCboProductionStatus, "Title", "Title");

                if (strProductionStatusValue.Contains(";"))
                {
                    radCboProductionStatus.SelectedValue = strProductionStatusValue.Split(";".ToCharArray())[0];
                }
                else
                {
                    if (radCboProductionStatus.FindItemByText(strProductionStatusValue) != null)
                        radCboProductionStatus.FindItemByText(strProductionStatusValue).Selected = true;
                }
            }
            /// Initially load all data and later filter based on selected Lithology Main value;
            //if (!string.IsNullOrEmpty(strLithologyMainValue) && strLithologyMainValue.Contains(";"))
            //{
            LoadLithostratGroup(LITHOLOGYSECONDARYLIST, strLithologyMainValue.Split(";".ToCharArray())[0], radCboLithologySecondary, "Secondary", "Secondary");
            if (strLithologySecondaryValue.Contains(";"))
                radCboLithologySecondary.SelectedValue = strLithologySecondaryValue.Split(";".ToCharArray())[0];
            // }

            if (!string.IsNullOrEmpty(strLithostratGroupValue) && strLithostratGroupValue.Contains(";"))
            {
                LoadLithostratGroup(LITHOSTRATFORMATION, strLithostratGroupValue.Split(";".ToCharArray())[0], radCboLithostratFormation, FIELDNAME, FIELDVALUE);
                if (strLithostratFormationValue.Contains(";"))
                    radCboLithostratFormation.SelectedValue = strLithostratFormationValue.Split(";".ToCharArray())[0];

                if (!string.IsNullOrEmpty(strLithostratFormationValue) && strLithostratFormationValue.Contains(";"))
                {
                    LoadLithostratGroup(LITHOSTRATMEMBER, strLithostratFormationValue.Split(";".ToCharArray())[0], radCboLithostratMember, FIELDNAME, FIELDVALUE);
                    if (strLithostratMemberValue.Contains(";"))
                        radCboLithostratMember.SelectedValue = strLithostratMemberValue.Split(";".ToCharArray())[0];
                }
            }
        }

        /// <summary>
        /// Clears the UI controls.
        /// </summary>
        new protected void ClearUIControls()
        {
            foreach (Control objControl in this.Controls)
            {
                foreach (Control objChildControl in objControl.Controls)
                {
                    RadioButtonList objRadioButtonList = new RadioButtonList();
                    RadioButton objRadioButton = new RadioButton();
                    TextBox objTextBox = new TextBox();
                    ListBox objListBox = new ListBox();
                    CheckBox objCheckBox = new CheckBox();
                    RadComboBox objRadCombo = new RadComboBox();
                    DropDownList objDropDownList = new DropDownList();
                    if (string.Equals(objChildControl.GetType().ToString(), objRadioButtonList.GetType().ToString()))
                    {
                        ((RadioButtonList)(objChildControl)).SelectedIndex = 0;
                    }
                    if (string.Equals(objChildControl.GetType().ToString(), objRadioButton.GetType().ToString()))
                    {
                        ((RadioButton)(objChildControl)).Checked = false;
                    }
                    if (string.Equals(objChildControl.GetType().ToString(), objTextBox.GetType().ToString()))
                    {
                        ((TextBox)(objChildControl)).Text = string.Empty;
                    }
                    if (string.Equals(objChildControl.GetType().ToString(), objListBox.GetType().ToString()))
                    {
                        ((ListBox)(objChildControl)).Items.Clear();
                    }
                    if (string.Equals(objChildControl.GetType().ToString(), objRadCombo.GetType().ToString()))
                    {
                        //    On changing the selected Save Search, all RadCombo items are removed. Only Reset them to selected index = 0. 
                        ((RadComboBox)(objChildControl)).SelectedIndex = 0;
                        // Clear only Lithology Secondary RadCombo if Lithology Main selected index = 0;
                        if (((RadComboBox)(objChildControl)).ID.Equals(radCboLithologySecondary.ID))
                        {
                            if (radCboLithologyMain.SelectedIndex == 0)
                            {
                                radCboLithologySecondary.Items.Clear();
                            }
                        }
                    }
                    if (string.Equals(objChildControl.GetType().ToString(), objCheckBox.GetType().ToString()))
                    {
                        ((CheckBox)(objChildControl)).Checked = false;
                    }
                    /// All DropDown Controls set only selected index = 0, should not clear items
                    if (string.Equals(objChildControl.GetType().ToString(), objDropDownList.GetType().ToString()))
                    {
                        ((DropDownList)(objChildControl)).SelectedIndex = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Attatches the javascript to controls.
        /// </summary>
        private void AttatchJavascriptToControls()
        {
            txtChronostratigraphy.Attributes.Add("readonly", "readonly");
            txtDepositionalEnv.Attributes.Add("readonly", "readonly");
            StringBuilder strChronostraticOnclickJavascript = new StringBuilder();
            strChronostraticOnclickJavascript.Append("OpenChronostraticPopup('");
            strChronostraticOnclickJavascript.Append(txtChronostratigraphy.ClientID);
            strChronostraticOnclickJavascript.Append("');");

            StringBuilder strDepositionalEnvOnclickJavascript = new StringBuilder();
            strDepositionalEnvOnclickJavascript.Append("OpenDepositionalPopup('");
            strDepositionalEnvOnclickJavascript.Append(txtDepositionalEnv.ClientID);
            strDepositionalEnvOnclickJavascript.Append("','");
            strDepositionalEnvOnclickJavascript.Append(hidDepositionalColumn.ClientID);
            strDepositionalEnvOnclickJavascript.Append("');");

            cmdChronostraticName.Attributes.Add(JAVASCRIPTONCLICK, strChronostraticOnclickJavascript.ToString());
            cmdDepositionalEnv.Attributes.Add(JAVASCRIPTONCLICK, strDepositionalEnvOnclickJavascript.ToString());

            StringBuilder strChronostraticClearOnclickJavascript = new StringBuilder();
            strChronostraticClearOnclickJavascript.Append("ClearChronostraticValue('");
            strChronostraticClearOnclickJavascript.Append(txtChronostratigraphy.ClientID);
            strChronostraticClearOnclickJavascript.Append("');");

            StringBuilder strDepositionalEnvClearOnclickJavascript = new StringBuilder();
            strDepositionalEnvClearOnclickJavascript.Append("ClearChronostraticValue('");
            strDepositionalEnvClearOnclickJavascript.Append(txtDepositionalEnv.ClientID);
            strDepositionalEnvClearOnclickJavascript.Append("');");

            cmdChronostraticClear.Attributes.Add(JAVASCRIPTONCLICK, strChronostraticClearOnclickJavascript.ToString());
            cmdDepositionalEnvClear.Attributes.Add(JAVASCRIPTONCLICK, strDepositionalEnvClearOnclickJavascript.ToString());

            StringBuilder strSearchCriteria = new StringBuilder();
            strSearchCriteria.Append("javascript:FileSearchTypeSelectedIndexChange(this,'");
            strSearchCriteria.Append(txtReservoir_Identifier.ID);
            strSearchCriteria.Append("|input')");
            cboSearchCriteria.Attributes.Add("onchange", strSearchCriteria.ToString());

            /// Append IDs of the TextBox controls needs validation on Search/Save Search click
            StringBuilder strTextControlIds = AppendControlIDs();

            StringBuilder strSaveSearchJavaScript = new StringBuilder();
            strSaveSearchJavaScript.Append("if(!NumericValidation('");
            strSaveSearchJavaScript.Append(strTextControlIds.ToString());
            strSaveSearchJavaScript.Append("') )return false;if(!ValidateSaveSrchFIELD())return false;");
            StringBuilder strSearchJavaScript = new StringBuilder();
            strSearchJavaScript.Append("if(!NumericValidation('");
            strSearchJavaScript.Append(strTextControlIds.ToString());
            strSearchJavaScript.Append("') )return false;if(!ValidateField(false))return false;");

            cmdSaveSearch.Attributes.Add(JAVASCRIPTONCLICK, strSaveSearchJavaScript.ToString());
            cmdSearch.Attributes.Add(JAVASCRIPTONCLICK, strSearchJavaScript.ToString());

            /// Add attributes to TextBox controls.
            /// These attributes will be used for Validation on Search/Save Search click
            AddAttributesToTextBoxControls();
        }

        /// <summary>
        /// Add attributes to TextBox controls.
        /// These attributes will be used for Validation on Search/Save Search click
        /// </summary>
        private void AddAttributesToTextBoxControls()
        {
            txtPorosityMin.Attributes.Add(SRPTEXTBOXALLOWEDDECIMALS, "2");
            txtPorosityMin.Attributes.Add(SRPTEXTBOXISRANGEALLOWED, "true");
            txtPorosityMin.Attributes.Add(SRPTEXTBOXMINIMUMVALUE, "0");
            txtPorosityMin.Attributes.Add(SRPTEXTBOXMAXIMUMVALUE, "50");
            txtPorosityMin.Attributes.Add(SRPTEXTBOXLABELNAME, "Porosity (Min)");
            txtPorosityMin.Attributes.Add(SRPTEXTBOXMEASURMENTS, "%");

            txtPorosityMax.Attributes.Add(SRPTEXTBOXALLOWEDDECIMALS, "2");
            txtPorosityMax.Attributes.Add(SRPTEXTBOXISRANGEALLOWED, "true");
            txtPorosityMax.Attributes.Add(SRPTEXTBOXMINIMUMVALUE, "0");
            txtPorosityMax.Attributes.Add(SRPTEXTBOXMAXIMUMVALUE, "50");
            txtPorosityMax.Attributes.Add(SRPTEXTBOXLABELNAME, "Porosity (Max)");
            txtPorosityMax.Attributes.Add(SRPTEXTBOXMEASURMENTS, "%");
            txtPorosityMax.Attributes.Add(SRPTEXTBOXDEPENDENTID, txtPorosityMin.ClientID.ToString());

            txtPermeability_Min.Attributes.Add(SRPTEXTBOXALLOWEDDECIMALS, "4");
            txtPermeability_Min.Attributes.Add(SRPTEXTBOXISRANGEALLOWED, "true");
            txtPermeability_Min.Attributes.Add(SRPTEXTBOXMINIMUMVALUE, "0.0001");
            txtPermeability_Min.Attributes.Add(SRPTEXTBOXMAXIMUMVALUE, "20000");
            txtPermeability_Min.Attributes.Add(SRPTEXTBOXLABELNAME, "Permeabilty (Min)");
            txtPermeability_Min.Attributes.Add(SRPTEXTBOXMEASURMENTS, "md");

            txtPermeability_Max.Attributes.Add(SRPTEXTBOXALLOWEDDECIMALS, "4");
            txtPermeability_Max.Attributes.Add(SRPTEXTBOXISRANGEALLOWED, "true");
            txtPermeability_Max.Attributes.Add(SRPTEXTBOXMINIMUMVALUE, "0.0001");
            txtPermeability_Max.Attributes.Add(SRPTEXTBOXMAXIMUMVALUE, "20000");
            txtPermeability_Max.Attributes.Add(SRPTEXTBOXLABELNAME, "Permeabilty (Max)");
            txtPermeability_Max.Attributes.Add(SRPTEXTBOXMEASURMENTS, "md");
            txtPermeability_Max.Attributes.Add(SRPTEXTBOXDEPENDENTID, txtPermeability_Min.ClientID.ToString());

            txtSandReservoir_Net_Gross_avg.Attributes.Add(SRPTEXTBOXALLOWEDDECIMALS, "2");
            txtSandReservoir_Net_Gross_avg.Attributes.Add(SRPTEXTBOXISRANGEALLOWED, "true");
            txtSandReservoir_Net_Gross_avg.Attributes.Add(SRPTEXTBOXMINIMUMVALUE, "0");
            txtSandReservoir_Net_Gross_avg.Attributes.Add(SRPTEXTBOXMAXIMUMVALUE, "infinite");
            txtSandReservoir_Net_Gross_avg.Attributes.Add(SRPTEXTBOXLABELNAME, "Sand Reservoir Net/Gross(Avg)");
            txtSandReservoir_Net_Gross_avg.Attributes.Add(SRPTEXTBOXMEASURMENTS, "Ratio(Decimals)");

            txtSandReservoir_Net_Gross_max.Attributes.Add(SRPTEXTBOXALLOWEDDECIMALS, "2");
            txtSandReservoir_Net_Gross_max.Attributes.Add(SRPTEXTBOXISRANGEALLOWED, "true");
            txtSandReservoir_Net_Gross_max.Attributes.Add(SRPTEXTBOXMINIMUMVALUE, "0");
            txtSandReservoir_Net_Gross_max.Attributes.Add(SRPTEXTBOXMAXIMUMVALUE, "infinite");
            txtSandReservoir_Net_Gross_max.Attributes.Add(SRPTEXTBOXLABELNAME, "Sand Reservoir Net/Gross(Max)");
            txtSandReservoir_Net_Gross_max.Attributes.Add(SRPTEXTBOXMEASURMENTS, "Ratio(Decimals)");

            txtSandReservoir_Net_Gross_min.Attributes.Add(SRPTEXTBOXALLOWEDDECIMALS, "2");
            txtSandReservoir_Net_Gross_min.Attributes.Add(SRPTEXTBOXISRANGEALLOWED, "true");
            txtSandReservoir_Net_Gross_min.Attributes.Add(SRPTEXTBOXMINIMUMVALUE, "0");
            txtSandReservoir_Net_Gross_min.Attributes.Add(SRPTEXTBOXMAXIMUMVALUE, "infinite");
            txtSandReservoir_Net_Gross_min.Attributes.Add(SRPTEXTBOXLABELNAME, "Sand Reservoir Net/Gross(Min)");
            txtSandReservoir_Net_Gross_min.Attributes.Add(SRPTEXTBOXMEASURMENTS, "Ratio(Decimals)");

            txtWaterSaturation_max.Attributes.Add(SRPTEXTBOXALLOWEDDECIMALS, "2");
            txtWaterSaturation_max.Attributes.Add(SRPTEXTBOXISRANGEALLOWED, "true");
            txtWaterSaturation_max.Attributes.Add(SRPTEXTBOXMINIMUMVALUE, "0");
            txtWaterSaturation_max.Attributes.Add(SRPTEXTBOXMAXIMUMVALUE, "infinite");
            txtWaterSaturation_max.Attributes.Add(SRPTEXTBOXLABELNAME, "Water Saturation(Max)");
            txtWaterSaturation_max.Attributes.Add(SRPTEXTBOXMEASURMENTS, "Ratio(Decimals)");

            txtOilViscosity.Attributes.Add(SRPTEXTBOXALLOWEDDECIMALS, "2");
            txtOilViscosity.Attributes.Add(SRPTEXTBOXISRANGEALLOWED, "true");
            txtOilViscosity.Attributes.Add(SRPTEXTBOXMINIMUMVALUE, "0");
            txtOilViscosity.Attributes.Add(SRPTEXTBOXMAXIMUMVALUE, "infinite");
            txtOilViscosity.Attributes.Add(SRPTEXTBOXLABELNAME, "Oil Viscosity");
            txtOilViscosity.Attributes.Add(SRPTEXTBOXMEASURMENTS, "");

            txtPressureReservoirInitial.Attributes.Add(SRPTEXTBOXALLOWEDDECIMALS, "2");
            txtPressureReservoirInitial.Attributes.Add(SRPTEXTBOXISRANGEALLOWED, "true");
            txtPressureReservoirInitial.Attributes.Add(SRPTEXTBOXMINIMUMVALUE, "0");
            txtPressureReservoirInitial.Attributes.Add(SRPTEXTBOXMAXIMUMVALUE, "infinite");
            txtPressureReservoirInitial.Attributes.Add(SRPTEXTBOXLABELNAME, "Pressure Reservoir(Initial)");
            txtPressureReservoirInitial.Attributes.Add(SRPTEXTBOXMEASURMENTS, "");

            txtOilGravity.Attributes.Add(SRPTEXTBOXALLOWEDDECIMALS, "2");
            txtOilGravity.Attributes.Add(SRPTEXTBOXISRANGEALLOWED, "true");
            txtOilGravity.Attributes.Add(SRPTEXTBOXMINIMUMVALUE, "0");
            txtOilGravity.Attributes.Add(SRPTEXTBOXMAXIMUMVALUE, "infinite");
            txtOilGravity.Attributes.Add(SRPTEXTBOXLABELNAME, "Oil Gravity");
            txtOilGravity.Attributes.Add(SRPTEXTBOXMEASURMENTS, "");

            txtGasInitiallyInPlace.Attributes.Add(SRPTEXTBOXALLOWEDDECIMALS, "0");
            txtGasInitiallyInPlace.Attributes.Add(SRPTEXTBOXISRANGEALLOWED, "true");
            txtGasInitiallyInPlace.Attributes.Add(SRPTEXTBOXMINIMUMVALUE, "0");
            txtGasInitiallyInPlace.Attributes.Add(SRPTEXTBOXMAXIMUMVALUE, "infinite");
            txtGasInitiallyInPlace.Attributes.Add(SRPTEXTBOXLABELNAME, "Gas Initially In Place");
            txtGasInitiallyInPlace.Attributes.Add(SRPTEXTBOXMEASURMENTS, "");

            txtStockTankOilInitially.Attributes.Add(SRPTEXTBOXALLOWEDDECIMALS, "0");
            txtStockTankOilInitially.Attributes.Add(SRPTEXTBOXISRANGEALLOWED, "true");
            txtStockTankOilInitially.Attributes.Add(SRPTEXTBOXMINIMUMVALUE, "0");
            txtStockTankOilInitially.Attributes.Add(SRPTEXTBOXMAXIMUMVALUE, "infinite");
            txtStockTankOilInitially.Attributes.Add(SRPTEXTBOXLABELNAME, "Stock Tank Oil Initially");
            txtStockTankOilInitially.Attributes.Add(SRPTEXTBOXMEASURMENTS, "");

            txtRecoveryFactorOil.Attributes.Add(SRPTEXTBOXALLOWEDDECIMALS, "2");
            txtRecoveryFactorOil.Attributes.Add(SRPTEXTBOXISRANGEALLOWED, "true");
            txtRecoveryFactorOil.Attributes.Add(SRPTEXTBOXMINIMUMVALUE, "0");
            txtRecoveryFactorOil.Attributes.Add(SRPTEXTBOXMAXIMUMVALUE, "100");
            txtRecoveryFactorOil.Attributes.Add(SRPTEXTBOXLABELNAME, "Recovery Factor Oil");
            txtRecoveryFactorOil.Attributes.Add(SRPTEXTBOXMEASURMENTS, "%");

            txtRecoveryFactorGas.Attributes.Add(SRPTEXTBOXALLOWEDDECIMALS, "2");
            txtRecoveryFactorGas.Attributes.Add(SRPTEXTBOXISRANGEALLOWED, "true");
            txtRecoveryFactorGas.Attributes.Add(SRPTEXTBOXMINIMUMVALUE, "0");
            txtRecoveryFactorGas.Attributes.Add(SRPTEXTBOXMAXIMUMVALUE, "100");
            txtRecoveryFactorGas.Attributes.Add(SRPTEXTBOXLABELNAME, "Recovery Factor Gas");
            txtRecoveryFactorGas.Attributes.Add(SRPTEXTBOXMEASURMENTS, "%");

            txtRecoverableOil.Attributes.Add(SRPTEXTBOXALLOWEDDECIMALS, "2");
            txtRecoverableOil.Attributes.Add(SRPTEXTBOXISRANGEALLOWED, "true");
            txtRecoverableOil.Attributes.Add(SRPTEXTBOXMINIMUMVALUE, "0");
            txtRecoverableOil.Attributes.Add(SRPTEXTBOXMAXIMUMVALUE, "infinite");
            txtRecoverableOil.Attributes.Add(SRPTEXTBOXLABELNAME, "Recoverable Oil");
            txtRecoverableOil.Attributes.Add(SRPTEXTBOXMEASURMENTS, "Mmbbl");

            txtRecoverableGas.Attributes.Add(SRPTEXTBOXALLOWEDDECIMALS, "2");
            txtRecoverableGas.Attributes.Add(SRPTEXTBOXISRANGEALLOWED, "true");
            txtRecoverableGas.Attributes.Add(SRPTEXTBOXMINIMUMVALUE, "0");
            txtRecoverableGas.Attributes.Add(SRPTEXTBOXMAXIMUMVALUE, "infinite");
            txtRecoverableGas.Attributes.Add(SRPTEXTBOXLABELNAME, "Recoverable Gas");
            txtRecoverableGas.Attributes.Add(SRPTEXTBOXMEASURMENTS, "MMSCF");
        }

        /// <summary>
        /// Append IDs of the TextBox controls needs validation on Search/Save Search click
        /// </summary>
        /// <returns></returns>
        private StringBuilder AppendControlIDs()
        {
            StringBuilder strTextControlIds = new StringBuilder();
            strTextControlIds.Append(txtPorosityMin.ClientID);
            strTextControlIds.Append(";");
            strTextControlIds.Append(txtPorosityMax.ClientID);
            strTextControlIds.Append(";");
            strTextControlIds.Append(txtPermeability_Min.ClientID);
            strTextControlIds.Append(";");
            strTextControlIds.Append(txtPermeability_Max.ClientID);
            strTextControlIds.Append(";");
            strTextControlIds.Append(txtSandReservoir_Net_Gross_avg.ClientID);
            strTextControlIds.Append(";");
            strTextControlIds.Append(txtSandReservoir_Net_Gross_min.ClientID);
            strTextControlIds.Append(";");
            strTextControlIds.Append(txtSandReservoir_Net_Gross_max.ClientID);
            strTextControlIds.Append(";");
            strTextControlIds.Append(txtWaterSaturation_max.ClientID);
            strTextControlIds.Append(";");
            strTextControlIds.Append(txtOilViscosity.ClientID);
            strTextControlIds.Append(";");
            strTextControlIds.Append(txtPressureReservoirInitial.ClientID);
            strTextControlIds.Append(";");
            strTextControlIds.Append(txtOilGravity.ClientID);
            strTextControlIds.Append(";");
            strTextControlIds.Append(txtGasInitiallyInPlace.ClientID);
            strTextControlIds.Append(";");
            strTextControlIds.Append(txtStockTankOilInitially.ClientID);
            strTextControlIds.Append(";");
            strTextControlIds.Append(txtRecoveryFactorOil.ClientID);
            strTextControlIds.Append(";");
            strTextControlIds.Append(txtRecoveryFactorGas.ClientID);
            strTextControlIds.Append(";");
            strTextControlIds.Append(txtRecoverableOil.ClientID);
            strTextControlIds.Append(";");
            strTextControlIds.Append(txtRecoverableGas.ClientID);
            return strTextControlIds;
        }

        #endregion
        #endregion
    }
}
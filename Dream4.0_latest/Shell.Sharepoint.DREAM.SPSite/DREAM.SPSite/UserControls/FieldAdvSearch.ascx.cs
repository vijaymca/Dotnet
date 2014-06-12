#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: FieldAdvSearch.ascx.cs 
#endregion

/// <summary> 
/// This is Field Search class
/// </summary> 
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Xsl;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DREAM.Business.Entities;
using Telerik.Web.UI;
using System.Text;


namespace Shell.SharePoint.DREAM.Site.UI.UserControls
{
    /// <summary>
    /// The Field UI Class for advance search criteria screen
    /// </summary>
    public partial class FieldAdvSearch :UIControlHandler
    {
        #region Declaration
        const string FIELDPAGEURL = "AdvSearchField.aspx";
        const string FIELDCHECKREFRESH = "FieldCheckRefresh";
        Entity objEntity = new Entity();
        string strCurrSiteURL = HttpContext.Current.Request.Url.ToString();
        string strFieldNameValue = string.Empty;
        string strBasinNameValue = string.Empty;
        string strOperatorNameValue = string.Empty;
        bool blnSearchClick;
        bool blnSrpControlsEnable;
        #endregion

        #region Protected Methods
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
            cmdSavedSearchButton.Attributes.Add(REDIRECTATTRIBUTE, REDIRECTATTRIBUTEVALUE);
            btnResetButton.Attributes.Add(REDIRECTATTRIBUTE, REDIRECTATTRIBUTEVALUE);
            if(!Page.IsPostBack)
            {

                try
                {

                    AttatchJavascriptToControls();
                    /// Sets the Field Check refresh session variable.
                    CommonUtility.SetSessionVariable(Page, FIELDCHECKREFRESH, DateTime.Now.ToString());
                    base.ListName = COUNTRYLIST;
                    base.EntityName = COUNTRYLIST;
                    base.SearchType = FIELDSEARCHTYPE;
                    lblException.Text = string.Empty;

                    if(blnSrpControlsEnable)
                    {
                        LoadControls(chbShared, cboSavedSearch, lstCountry, cboOperationalEnv, cboReserveMagOil, cboReserveMagGas, cboTectonicSetting, cboTectonicSettingKle, rdblSearchCond);
                        lstCountry.Items.Insert(0, DEFAULTDROPDOWNTEXT);
                       
                    }
                    else
                    {
                        //R5k changes in Dream 4.0
                        LoadCountryBasinData(ListName, EntityName, lstCountry);
                        lstCountry.Items.Insert(0, DEFAULTDROPDOWNTEXT);
                        LoadControls(chbShared, cboSavedSearch);
                    }
                    GetAssetColumns(REPORTSERVICECOLUMNLIST, cboSearchCriteria, FIELDITEMVAL);
                    if(Request.QueryString[SAVESEARCHNAMEQUERYSTRING] != null)
                    {
                        if(Request.QueryString[OPERATIONQUERYSTRING] != null)
                        {
                            if(string.Equals(Request.QueryString[OPERATIONQUERYSTRING].ToString().ToLowerInvariant(), "modify".ToLowerInvariant()))
                            {
                                cmdSaveSearch.Value = MODIFYSRCH;
                                txtSaveSearch.Text = Request.QueryString[SAVESEARCHNAMEQUERYSTRING].ToString();
                                txtSaveSearch.Enabled = false;
                            }
                        }
                    }
                    BindTooltipTextToControls();

                    SetTectonicSettings();
                }
                catch(WebException webEx)
                {
                    lblException.Visible = true;
                    lblException.Text = webEx.Message;
                }
                catch(SoapException soapEx)
                {
                    if(!string.Equals(soapEx.Message.ToString().ToLowerInvariant(), SOAPEXCEPTIONMESSAGE.ToLowerInvariant()))
                    {
                        CommonUtility.HandleException(strCurrSiteURL, soapEx, 1);
                    }
                    lblException.Visible = true;
                    lblException.Text = soapEx.Message.ToString();
                }
                catch(Exception ex)
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
            if(radCboOperator != null)
                radCboOperator.Dispose();
            if(radCboFieldName != null)
                radCboFieldName.Dispose();
            if(radCboBasinName != null)
                radCboBasinName.Dispose();
        }
        #region SRP Code

        /// <summary>cmdRadCboOperatorComboBox_ItemsRequested
        /// Handles the key pressed event of the radCboBasinName control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs"/> instance containing the event data.</param>
        protected void cmdRadCboBasinNameComboBox_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {

            try
            {
                //PopulateRadComboBox(e, radCboBasinName, BASINLIST, "Title", "Title");
                PopulateRadComboBox(e, radCboBasinName, BASINLIST, BASINITEMVAL, BASINITEMVAL);
            }

            catch(WebException webEx)
            {
                e.Message = UNEXPECTEDEXCEPTIONMESSAGE;
                CommonUtility.HandleException(strCurrSiteURL, webEx, 1);
            }
            catch(Exception ex)
            {
                e.Message = UNEXPECTEDEXCEPTIONMESSAGE;
                CommonUtility.HandleException(strCurrSiteURL, ex);
            }
        }

        /// <summary>
        /// Handles the key pressed event of the radCboFieldName control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs"/> instance containing the event data.</param>
        protected void cmdRadCboFieldNameComboBox_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {

            try
            {

                PopulateRadComboBox(e, radCboFieldName, FIELD, FIELDVALUE, FIELDNAME);

            }

            catch(SoapException soapEx)
            {
                if(soapEx.Message.Equals(SOAPEXCEPTIONMESSAGE))
                {
                    e.Message = NORECORDFOUNDSEXCEPTIONMESSAGE;
                }
                else
                {
                    e.Message = UNEXPECTEDEXCEPTIONMESSAGE;
                    CommonUtility.HandleException(strCurrSiteURL, soapEx, 1);
                }
            }
            catch(WebException webEx)
            {

                e.Message = UNEXPECTEDEXCEPTIONMESSAGE;
                CommonUtility.HandleException(strCurrSiteURL, webEx, 1);
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteURL, ex);
            }

        }



        /// <summary>
        /// Handles the key pressed event of the radCbooperational control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs"/> instance containing the event data.</param>
        protected void cmdRadCboOperatorComboBox_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {

            try
            {
                PopulateRadComboBox(e, radCboOperator, FIELDOPERATOR, string.Empty, SRPADVPOPUPOPERATOR);
            }

            catch(SoapException soapEx)
            {
                if(soapEx.Message.Equals(SOAPEXCEPTIONMESSAGE))
                {
                    /// Add the message as item to Rad DropDown
                    e.Message = NORECORDFOUNDSEXCEPTIONMESSAGE;
                }
                else
                {
                    e.Message = UNEXPECTEDEXCEPTIONMESSAGE;
                    CommonUtility.HandleException(strCurrSiteURL, soapEx, 1);
                }
            }
            catch(WebException webEx)
            {
                /// Add the message as item to Rad DropDown
                e.Message = UNEXPECTEDEXCEPTIONMESSAGE;
                CommonUtility.HandleException(strCurrSiteURL, webEx, 1);
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteURL, ex);
            }

        }

        #endregion

        /// <summary>
        /// Handles the Click event of the cmdSaveSearch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmdSaveSearch_Click(object sender, EventArgs e)
        {

            bool blnIsNameExist = false;
            try
            {
                AssignRadValueFromHiddenVariable(hidRadContent.Value);
                if(string.Equals(cmdSaveSearch.Value.ToString().ToLowerInvariant(), MODIFYSRCH.ToLowerInvariant()))
                {
                    lblException.Visible = false;
                    objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                    object objCheckRefresh = CommonUtility.GetSessionVariable(Page, FIELDCHECKREFRESH);
                    //Checks whether the event is fired or page has been refreshed.
                    if(string.Equals(ViewState[CHECKREFRESH].ToString(), (string)objCheckRefresh))
                    {
                        CommonUtility.SetSessionVariable(Page, FIELDCHECKREFRESH, DateTime.Now.ToString());
                        string strSaveSearchName = txtSaveSearch.Text.Trim();
                        XmlDocument xmlDocSearchRequest = null;
                        xmlDocSearchRequest = CreateSaveSearchXML();
                        UISaveSearchHandler objUISaveSearchHandler = null;
                        try
                        {
                            objUISaveSearchHandler = new UISaveSearchHandler();
                            objUISaveSearchHandler.ModifySaveSearchXML(FIELDSEARCHTYPE, strSaveSearchName, chbShared.Checked, cboSavedSearch, xmlDocSearchRequest);
                            if(cboSavedSearch.Items.FindByText(DEFAULTDROPDOWNTEXT) == null)
                            {
                                cboSavedSearch.Items.Insert(0, DEFAULTDROPDOWNTEXT);
                            }
                        }
                        catch(WebException webEx)
                        {
                            lblException.Visible = true;
                            lblException.Text = webEx.Message;
                        }
                        catch(Exception ex)
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
                        ((MOSSServiceManager)objMossController).LoadSaveSearch(FIELDSEARCHTYPE, cboSavedSearch);
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
                    object objCheckRefresh = CommonUtility.GetSessionVariable(Page, FIELDCHECKREFRESH);
                    /// Checks whether the event is fired or page has been refreshed.
                    if(string.Equals(ViewState[CHECKREFRESH].ToString(), (string)objCheckRefresh))
                    {
                        CommonUtility.SetSessionVariable(Page, FIELDCHECKREFRESH, DateTime.Now.ToString());
                        string strSaveSearchName = txtSaveSearch.Text.Trim();
                        /// Check for Duplicate Name Exist
                        arlSavedSearch = ((MOSSServiceManager)objMossController).GetSaveSearchName(FIELDSEARCHTYPE, GetUserName());
                        if(IsDuplicateNameExist(arlSavedSearch, strSaveSearchName))
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
                                objUISaveSearchHandler.SaveSearchXMLToLibrary(FIELDSEARCHTYPE, strSaveSearchName, chbShared.Checked, cboSavedSearch, xmlDocSearchRequest);
                            }
                            catch(WebException webEx)
                            {
                                lblException.Visible = true;
                                lblException.Text = webEx.Message;
                            }
                            catch(Exception ex)
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
                        ((MOSSServiceManager)objMossController).LoadSaveSearch(FIELDSEARCHTYPE, cboSavedSearch);
                    }
                    if(!blnIsNameExist)
                        cboSavedSearch.Text = txtSaveSearch.Text.ToString().Trim();
                    txtSaveSearch.Text = string.Empty;
                }
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteURL, ex);
            }
        }

        /// <summary>
        /// Handles the PreRender event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_PreRender(object sender, EventArgs e)
        {

            object objCheckRefresh = CommonUtility.GetSessionVariable(Page, FIELDCHECKREFRESH);
            //if the session has the CheckRefresh value then assign to ViewState.
            if(objCheckRefresh != null)
                ViewState[CHECKREFRESH] = (string)objCheckRefresh;
        }


        /// <summary>
        /// Event handler for the Search Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            AssignRadValueFromHiddenVariable(hidRadContent.Value);
            try
            {
                blnSearchClick = true;
                if(Page.IsPostBack)
                {
                    DisplaySearchResults();
                }
            }
            catch(WebException webEx)
            {
                lblException.Visible = true;
                lblException.Text = webEx.Message;
            }
            catch(SoapException soapEx)
            {
                if(!string.Equals(soapEx.Message.ToString().ToLowerInvariant(), SOAPEXCEPTIONMESSAGE.ToLowerInvariant()))
                {
                    CommonUtility.HandleException(strCurrSiteURL, soapEx, 1);
                }
                lblException.Visible = true;
                lblException.Text = soapEx.Message.ToString();
            }
            catch(Exception ex)
            {
                if(string.Equals(ex.Message.ToLowerInvariant(), BLANKFILEMESSAGE.ToLowerInvariant()))
                {
                    lblException.Visible = true;
                    lblException.Text = BLANKFILEMESSAGE;
                }
                else
                    CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cboSavedSearch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cboSavedSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                string strUserID = GetUserName();
                XmlDocument xmldoc = new XmlDocument();
                xmldoc = ((MOSSServiceManager)objMossController).GetDocLibXMLFile(FIELDSEARCHTYPE, strUserID);
                string strSrpControlsEnable = PortalConfiguration.GetInstance().GetKey(ENABLESRPSPECIFICCONTROLS);
                if(string.Equals(strSrpControlsEnable.ToLowerInvariant(), "yes"))
                {
                    blnSrpControlsEnable = true;

                }
                ClearUIControls();
                ResetSRPControls();
                LoadCountryBasinData(COUNTRYLIST, COUNTRY, lstCountry);
                lstCountry.ClearSelection();
                lblException.Visible = false;
                if((cboSavedSearch.SelectedIndex != 0) || (string.Equals(cmdSaveSearch.Value.ToString().ToLowerInvariant(), MODIFYSRCH.ToLowerInvariant())))
                {
                    BindSearchConditionOperator(xmldoc, cboSavedSearch.SelectedItem.Text.ToString(), rdblSearchCond);
                    BindUIControls(xmldoc, cboSavedSearch.SelectedItem.Text.ToString(), chbShared);

                    if(string.Equals(cmdSaveSearch.Value.ToString().ToLowerInvariant(), MODIFYSRCH.ToLowerInvariant()))
                    {
                        txtSaveSearch.Text = cboSavedSearch.Text;
                    }
                }
            }
            catch(WebException webEx)
            {
                lblException.Visible = true;
                lblException.Text = webEx.Message;
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteURL, ex);
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
            if(string.Equals(cmdSaveSearch.Value.ToString().ToLowerInvariant(), MODIFYSRCH.ToLowerInvariant()))
            {
                if(Request.QueryString[SAVESEARCHNAMEQUERYSTRING] != null)
                {
                    strUrl.Append(FIELDPAGEURL);
                    strUrl.Append("?asset=Field&manage=true&savesearchname=");
                    strUrl.Append(Request.QueryString[SAVESEARCHNAMEQUERYSTRING].ToString());
                    strUrl.Append("&operation=modify");
                }
                else
                {
                    strUrl.Append(FIELDPAGEURL);
                    strUrl.Append("?asset=Field");
                }
            }
            else
            {
                strUrl.Append(FIELDPAGEURL);
                strUrl.Append("?asset=Field");
            }
            RedirectPage(strUrl.ToString(), string.Empty);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Creates the save search XML.
        /// </summary>
        private XmlDocument CreateSaveSearchXML()
        {
            XmlDocument xmlDocSearchRequest = null;
            objRequestInfo = new RequestInfo();
            objReportController = objFactory.GetServiceManager(REPORTSERVICE);

            objRequestInfo = SetBasicDataObjects();
            xmlDocSearchRequest = objReportController.CreateSearchRequest(objRequestInfo);
            return xmlDocSearchRequest;
        }

        /// <summary>
        /// Displays the search results from response xml to ui.
        /// </summary>
        private void DisplaySearchResults()
        {
            objRequestInfo = new RequestInfo();
            objRequestInfo = SetBasicDataObjects();
            UISaveSearchHandler objUISaveSearch = new UISaveSearchHandler();
            objUISaveSearch.DisplayResults(Page, objRequestInfo, FIELDSEARCHTYPE);
            StringBuilder strSearchResultsPage = new StringBuilder();
            strSearchResultsPage.Append(SEARCHRESULTSPAGE);
            strSearchResultsPage.Append("?SearchType=");
            strSearchResultsPage.Append(FIELDSEARCHTYPE);
            strSearchResultsPage.Append("&asset=Field");
            RedirectPage(strSearchResultsPage.ToString(), "Field");
        }

        /// <summary>
        /// Method is use for Bind tool tips to the controls
        /// </summary>
        private void BindTooltipTextToControls()
        {
            DataTable dtFilterTooltip = null;
            string strControlName = string.Empty;
            string strTooltipText = string.Empty;
            string strAsset = FIELDITEMVAL;


            try
            {
                dtFilterTooltip = new DataTable();
                dtFilterTooltip = AssignToolTip();
                dtFilterTooltip = GetFilterDataTable(dtFilterTooltip, strAsset);
                if(dtFilterTooltip.Rows.Count > 0)
                {
                    foreach(DataRow drFilter in dtFilterTooltip.Rows)
                    {
                        strControlName = drFilter["Title"].ToString();
                        strTooltipText = drFilter["Tooltip_X0020_Text"].ToString();
                        if(string.Equals(imgSavedSearch.ID.Remove(0, 3).ToString(), strControlName))
                        {
                            imgSavedSearch.ToolTip = strTooltipText;
                        }
                        else if(imgIdentifier.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgIdentifier.ToolTip = strTooltipText;
                        }
                        else if(imgDescription.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgDescription.ToolTip = strTooltipText;
                        }
                        else if(imgCountry.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgCountry.ToolTip = strTooltipText;
                            imgCountry.Attributes.Add(JAVASCRIPTONCLICK, string.Format(TOOLTIPPOPUPFUNCTION, imgCountry.ID.Remove(0, 3)));
                        }
                        else if(imgOwner.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgOwner.ToolTip = strTooltipText;
                        }
                        else if(imgFieldName.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgFieldName.ToolTip = strTooltipText;
                            imgFieldName.Attributes.Add(JAVASCRIPTONCLICK, string.Format(TOOLTIPPOPUPFUNCTION, imgFieldName.ID.Remove(0, 3)));
                        }
                        else if(imgTypeofField.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgTypeofField.ToolTip = strTooltipText;
                        }
                        else if(imgOperator.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgOperator.ToolTip = strTooltipText;
                            imgOperator.Attributes.Add(JAVASCRIPTONCLICK, string.Format(TOOLTIPPOPUPFUNCTION, imgOperator.ID.Remove(0, 3)));
                        }
                        else if(imgCurrentStatus.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgCurrentStatus.ToolTip = strTooltipText;
                        }
                        else if(imgSearchName.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgSearchName.ToolTip = strTooltipText;
                        }
                        else if(imgOperationalEnv.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgOperationalEnv.ToolTip = strTooltipText;
                            imgOperationalEnv.Attributes.Add(JAVASCRIPTONCLICK, string.Format(TOOLTIPPOPUPFUNCTION, imgOperationalEnv.ID.Remove(0, 3)));
                        }
                        else if(imgTectonicSetting.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgTectonicSetting.ToolTip = strTooltipText;
                            imgTectonicSetting.Attributes.Add(JAVASCRIPTONCLICK, string.Format(TOOLTIPPOPUPFUNCTION, imgTectonicSetting.ID.Remove(0, 3)));
                        }
                        else if(imgNoOfWells.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgNoOfWells.ToolTip = strTooltipText;
                        }
                        else if(imgNoOfProducers.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgNoOfProducers.ToolTip = strTooltipText;
                        }
                        else if(imgNoOfInjector.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgNoOfInjector.ToolTip = strTooltipText;
                        }
                        else if(imgBasinName.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgBasinName.ToolTip = strTooltipText;
                            imgBasinName.Attributes.Add(JAVASCRIPTONCLICK, string.Format(TOOLTIPPOPUPFUNCTION, imgBasinName.ID.Remove(0, 3)));
                        }
                        else if(imgReserveMagnitudeOil.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgReserveMagnitudeOil.ToolTip = strTooltipText;
                            imgReserveMagnitudeOil.Attributes.Add(JAVASCRIPTONCLICK, string.Format(TOOLTIPPOPUPFUNCTION, imgReserveMagnitudeOil.ID.Remove(0, 3)));
                        }
                        else if(imgPorosityMax.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgPorosityMax.ToolTip = strTooltipText;
                            imgPorosityMax.Attributes.Add(JAVASCRIPTONCLICK, string.Format(TOOLTIPPOPUPFUNCTION, imgPorosityMax.ID.Remove(0, 3)));
                        }
                        else if(imgPermeabilityMax.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgPermeabilityMax.ToolTip = strTooltipText;
                            imgPermeabilityMax.Attributes.Add(JAVASCRIPTONCLICK, string.Format(TOOLTIPPOPUPFUNCTION, imgPermeabilityMax.ID.Remove(0, 3)));
                        }
                        else if(imgOilInPlace.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgOilInPlace.ToolTip = strTooltipText;
                            imgOilInPlace.Attributes.Add(JAVASCRIPTONCLICK, string.Format(TOOLTIPPOPUPFUNCTION, imgOilInPlace.ID.Remove(0, 3)));
                        }
                        else if(imgCondensateInPlace.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgCondensateInPlace.ToolTip = strTooltipText;
                            imgCondensateInPlace.Attributes.Add(JAVASCRIPTONCLICK, string.Format(TOOLTIPPOPUPFUNCTION, imgCondensateInPlace.ID.Remove(0, 3)));
                        }
                        else if(imgCondensateRecoveryFactor.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgCondensateRecoveryFactor.ToolTip = strTooltipText;
                            imgCondensateRecoveryFactor.Attributes.Add(JAVASCRIPTONCLICK, string.Format(TOOLTIPPOPUPFUNCTION, imgCondensateRecoveryFactor.ID.Remove(0, 3)));
                        }
                        else if(imgGasInitiallyInPlace.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgGasInitiallyInPlace.ToolTip = strTooltipText;
                            imgGasInitiallyInPlace.Attributes.Add(JAVASCRIPTONCLICK, string.Format(TOOLTIPPOPUPFUNCTION, imgGasInitiallyInPlace.ID.Remove(0, 3)));
                        }
                    }
                }
            }
            finally
            {
                if(dtFilterTooltip != null)
                    dtFilterTooltip.Dispose();
            }
        }

        /// <summary>
        /// Sets the list values.
        /// </summary>
        /// <param name="arrFields">The arr fields.</param>
        /// <param name="lbControl">The lb control.</param>
        /// <returns></returns>
        private RequestInfo SetListValues(ArrayList fieldsGroup, string value,string attributeName)
        {
            objRequestInfo.Entity = SetEntity(fieldsGroup, value, attributeName);
            return objRequestInfo;
        }

        /// <summary>
        /// Sets the entity.
        /// </summary>
        /// <param name="arrFields">The arr fields.</param>
        /// <param name="userEnteredValue">The User Entered Value</param>
        /// <returns></returns>
        private Entity SetEntity(ArrayList fieldsGroup, string userEnteredValue, string attributeName)
        {
            Entity objEntity = new Entity();

            if(fieldsGroup.Count == 1 && fieldsGroup[0] != null)
            {
                Control radControl = (Control)fieldsGroup[0];
                ArrayList arlAttribute = new ArrayList();
                Attributes objAttribute = new Attributes();
                //Commented in DREAM 4.0 for R5K Changes
                //objAttribute.Name = GetRadControlID(radControl.ID);
                //Added in DREAM 4.0 for R5K Changes
                //Start
                objAttribute.Name = attributeName;
                //End
                ArrayList arlValue = new ArrayList();
                Value objValue = new Value();
                objValue.InnerText = userEnteredValue;
                arlValue.Add(objValue);
                objAttribute.Value = arlValue;
                objAttribute.Operator = GetOperator(objAttribute.Value);
                arlAttribute.Add(objAttribute);
                objEntity.Attribute = arlAttribute;
                objEntity.Criteria = SetCriteria();

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
        private Criteria SetCriteria()
        {
            Criteria objCriteria = new Criteria();
            objCriteria.Value = STAROPERATOR;
            objCriteria.Operator = GetOperator(objCriteria.Value);
            return objCriteria;
        }

        /// <summary>
        /// Sets the basic data objects to create XML document
        /// </summary>
        /// <param name="strRequestInfo">The requestinfo search type.</param>
        /// <returns></returns>
        private RequestInfo SetBasicDataObjects()
        {
            objRequestInfo = new RequestInfo();

            objRequestInfo.Entity = SetEntity();
            if(blnSrpControlsEnable)
                objRequestInfo.Entity.Name = SRPFIELDSEARCHTYPE;
            else
                objRequestInfo.Entity.Name = FIELDSEARCHTYPE;


            return objRequestInfo;
        }

        /// <summary>
        /// Sets the entity.
        /// </summary>
        /// <returns></returns>
        private Entity SetEntity()
        {

            objEntity = new Entity();
            ArrayList arlBasicAttributeGroup = new ArrayList();
            ArrayList arlAttribute = new ArrayList();
            ArrayList arlSRPAttribute = new ArrayList();
            string strSearchPercentageVAlue = PortalConfiguration.GetInstance().GetKey(SEARCHPERCENTAGEVALUE);

            arlAttribute = SetBasicAttribute();


            if(blnSrpControlsEnable && blnSearchClick)
            {
                arlSRPAttribute = SetSRPAttributes(arlSRPAttribute);
                if(arlSRPAttribute.Count >= 1)
                {
                    arlAttribute.AddRange(arlSRPAttribute);

                }
                if(arlAttribute.Count > 1)
                {

                    ArrayList arlFinalAttributeGroup = new ArrayList();

                    arlFinalAttributeGroup = SetSRPAttributeGroup(strSearchPercentageVAlue, arlFinalAttributeGroup);
                    if(arlFinalAttributeGroup.Count > 1)
                    {
                        arlBasicAttributeGroup = SetBasicAttributeGroup(arlAttribute);
                        arlFinalAttributeGroup.Add(arlBasicAttributeGroup[0]);

                        AttributeGroup objFinalAttributeGroup = new AttributeGroup();
                        objFinalAttributeGroup.Operator = GetLogicalOperator(rdblSearchCond.SelectedValue);
                        objFinalAttributeGroup.AttributeGroups = arlFinalAttributeGroup;
                        ArrayList newArrayList = new ArrayList();
                        newArrayList.Add(objFinalAttributeGroup);
                        objEntity.AttributeGroups = newArrayList;
                    }

                    else
                    {
                        arlBasicAttributeGroup = SetBasicAttributeGroup(arlAttribute);
                        objEntity.AttributeGroups = arlBasicAttributeGroup;

                    }
                }
                else
                {

                    objEntity.Attribute = arlAttribute;
                }
            }
            else
            {
                if(arlAttribute.Count > 1)
                {
                    arlBasicAttributeGroup = SetBasicAttributeGroup(arlAttribute);
                    objEntity.AttributeGroups = arlBasicAttributeGroup;
                }
                else
                {
                    objEntity.Attribute = arlAttribute;
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

            AttributeGroup objBasicAttributeGroup = new AttributeGroup();
            /// SRP Changes
            if(blnSrpControlsEnable)
            {
                objBasicAttributeGroup.Operator = GetLogicalOperator(rdblSearchCond.SelectedValue);
            }
            else
            {
                objBasicAttributeGroup.Operator = GetLogicalOperator();
            }

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
            string strSrpControlsEnable = PortalConfiguration.GetInstance().GetKey(ENABLESRPSPECIFICCONTROLS);
            if(string.Equals(strSrpControlsEnable.ToLowerInvariant(), "yes"))
            {
                blnSrpControlsEnable = true;

            }
            ArrayList arlAttribute = new ArrayList();
            ArrayList arlAttributeGroup = new ArrayList();
            ArrayList arlAttributeGroups = new ArrayList();

            /// checks if file based search criteria is selected or not. 
            if(cboSearchCriteria.SelectedIndex != 0)
            {
                /// overloaded method to search a file for identifier values. Returns an arraylist of Attributes objects
                arlAttribute = SetUITextControls(txtField_Identifier, arlAttribute, ReadFileSearch(fileUploader.PostedFile, hidWordContent.Value), cboSearchCriteria);
                if(arlAttribute.Count == 0)
                {
                    throw new Exception(BLANKFILEMESSAGE);
                }
            }
            else
            {
                if(!string.IsNullOrEmpty(txtField_Identifier.Text.Trim()))
                {
                    arlAttribute = SetUITextControls(txtField_Identifier, arlAttribute);
                }
            }
            if(!string.IsNullOrEmpty(txtDescription.Text.Trim()))
            {
                arlAttribute = SetUITextControls(txtDescription, arlAttribute);
            }

            if(!string.IsNullOrEmpty(txtFieldType.Text.Trim()))
            {
                arlAttribute = SetUITextControls(txtFieldType, arlAttribute);
            }
            if(!string.IsNullOrEmpty(txtOwner.Text.Trim()))
            {
                arlAttribute = SetUITextControls(txtOwner, arlAttribute);
            }

            if(!string.IsNullOrEmpty(txtCurrentStatus.Text.Trim()))
            {
                arlAttribute = SetUITextControls(txtCurrentStatus, arlAttribute);
            }
            SetBasinCountryAttribute(arlAttribute, lstCountry);
            #region SRP Controls
            if(blnSrpControlsEnable)
            {
                if(!blnSearchClick)
                {
                    #region Save Search Click
                    if(!string.IsNullOrEmpty(txtOilInPlace.Text.Trim()))
                    {
                        arlAttribute = SetUITextControls(txtOilInPlace, arlAttribute, true, strSearchPercentageVAlue, blnSearchClick);
                    }
                    if(!string.IsNullOrEmpty(txtGasInitiallyInPlace.Text.Trim()))
                    {
                        arlAttribute = SetUITextControls(txtGasInitiallyInPlace, arlAttribute, true, strSearchPercentageVAlue, blnSearchClick);
                    }
                    if(!string.IsNullOrEmpty(txtCondensateInPlace.Text.Trim()))
                    {
                        arlAttribute = SetUITextControls(txtCondensateInPlace, arlAttribute, true, strSearchPercentageVAlue, blnSearchClick);
                    }
                    if(!string.IsNullOrEmpty(txtCondensateRecovery.Text.Trim()))
                    {
                        arlAttribute = SetUITextControls(txtCondensateRecovery, arlAttribute, true, strSearchPercentageVAlue, blnSearchClick);
                    }
                    if(!string.IsNullOrEmpty(txtPermeability.Text.Trim()))
                    {
                        arlAttribute = SetUITextControls(txtPermeability, arlAttribute, true, strSearchPercentageVAlue, blnSearchClick);
                    }
                    if(!string.IsNullOrEmpty(txtPorosity.Text.Trim()))
                    {
                        arlAttribute = SetUITextControls(txtPorosity, arlAttribute, true, strSearchPercentageVAlue, blnSearchClick);
                    }
                    if(!string.IsNullOrEmpty(txtNoOfWells.Text.Trim()))
                    {
                        arlAttribute = SetUITextControls(txtNoOfWells, arlAttribute, false, strSearchPercentageVAlue, blnSearchClick);
                    }
                    if(!string.IsNullOrEmpty(txtNoOfProducers.Text.Trim()))
                    {
                        arlAttribute = SetUITextControls(txtNoOfProducers, arlAttribute, false, strSearchPercentageVAlue, blnSearchClick);
                    }
                    if(!string.IsNullOrEmpty(txtNoOfInjectors.Text.Trim()))
                    {
                        arlAttribute = SetUITextControls(txtNoOfInjectors, arlAttribute, false, strSearchPercentageVAlue, blnSearchClick);
                    }
                    if(cboOperationalEnv.SelectedIndex != 0)
                    {
                        arlAttribute = SetUITextControls(cboOperationalEnv, arlAttribute);
                    }
                    /// For Save Search use "Text" field values in Request XML since needed to bind back
                    if(cboReserveMagOil.SelectedIndex != 0)
                    {
                        arlAttribute = SetUITextControls(cboReserveMagOil, arlAttribute, true);
                    }
                    /// For Save Search use "Text" field values in Request XML since needed to bind back
                    if(cboReserveMagGas.SelectedIndex != 0)
                    {
                        arlAttribute = SetUITextControls(cboReserveMagGas, arlAttribute, true);
                    }
                    /// For Save Search use "Text" field values in Request XML since needed to bind back
                    if(cboTectonicSetting.SelectedIndex != 0)
                    {
                        arlAttribute = SetUITextControls(cboTectonicSetting, arlAttribute, true);
                    }
                    /// For Save Search use "Text" field values in Request XML since needed to bind back
                    if(cboTectonicSettingKle.SelectedIndex != 0)
                    {
                        arlAttribute = SetUITextControls(cboTectonicSettingKle, arlAttribute, true);
                        if(arlAttribute != null && arlAttribute.Count > 0)
                        {
                            ((Attributes)arlAttribute[arlAttribute.Count - 1]).Name = GetControlID(cboTectonicSetting.ID);
                        }
                    }
                    if(rdblTectonicClassification.SelectedIndex != -1)
                    {
                        arlAttribute = SetUITextControls(rdblTectonicClassification, arlAttribute, false);
                    }
                    if(!string.IsNullOrEmpty(strBasinNameValue))
                    {
                        arlAttribute = SetUITextControls(radCboBasinName, arlAttribute, strBasinNameValue);
                    }
                    #endregion
                }
            }
            if(!string.IsNullOrEmpty(strFieldNameValue))
            {
                arlAttribute = SetUITextControls(radCboFieldName, arlAttribute, strFieldNameValue);
            }
            if(!string.IsNullOrEmpty(strOperatorNameValue))
            {
                arlAttribute = SetUITextControls(radCboOperator, arlAttribute, strOperatorNameValue);
            }
            #endregion

            return arlAttribute;
        }

        #region SRP Code
        /// <summary>
        /// Sets the SRP attribute group.
        /// </summary>
        /// <param name="strSearchPercentageVAlue">The STR search percentage V alue.</param>
        /// <param name="arlAttribute">The arraylist of attributegroup.</param>
        /// <returns></returns>
        private ArrayList SetSRPAttributeGroup(string strSearchPercentageVAlue, ArrayList arlAttributeGroup)
        {
            if(!string.IsNullOrEmpty(txtOilInPlace.Text.Trim()))
            {
                arlAttributeGroup = SetUITextControls(txtOilInPlace, arlAttributeGroup, true, strSearchPercentageVAlue, blnSearchClick);
            }
            if(!string.IsNullOrEmpty(txtGasInitiallyInPlace.Text.Trim()))
            {
                arlAttributeGroup = SetUITextControls(txtGasInitiallyInPlace, arlAttributeGroup, true, strSearchPercentageVAlue, blnSearchClick);
            }
            if(!string.IsNullOrEmpty(txtCondensateInPlace.Text.Trim()))
            {
                arlAttributeGroup = SetUITextControls(txtCondensateInPlace, arlAttributeGroup, true, strSearchPercentageVAlue, blnSearchClick);
            }
            if(!string.IsNullOrEmpty(txtCondensateRecovery.Text.Trim()))
            {
                arlAttributeGroup = SetUITextControls(txtCondensateRecovery, arlAttributeGroup, true, strSearchPercentageVAlue, blnSearchClick);
            }
            if(!string.IsNullOrEmpty(txtPermeability.Text.Trim()))
            {
                arlAttributeGroup = SetUITextControls(txtPermeability, arlAttributeGroup, true, strSearchPercentageVAlue, blnSearchClick);
            }
            if(!string.IsNullOrEmpty(txtPorosity.Text.Trim()))
            {
                arlAttributeGroup = SetUITextControls(txtPorosity, arlAttributeGroup, true, strSearchPercentageVAlue, blnSearchClick);
            }
            if(!string.IsNullOrEmpty(txtNoOfWells.Text.Trim()))
            {
                arlAttributeGroup = SetUITextControls(txtNoOfWells, arlAttributeGroup, false, strSearchPercentageVAlue, blnSearchClick);
            }
            if(!string.IsNullOrEmpty(txtNoOfProducers.Text.Trim()))
            {
                arlAttributeGroup = SetUITextControls(txtNoOfProducers, arlAttributeGroup, false, strSearchPercentageVAlue, blnSearchClick);
            }
            if(!string.IsNullOrEmpty(txtNoOfInjectors.Text.Trim()))
            {
                arlAttributeGroup = SetUITextControls(txtNoOfInjectors, arlAttributeGroup, false, strSearchPercentageVAlue, blnSearchClick);
            }
            return arlAttributeGroup;
        }

        /// <summary>
        /// Sets the SRP attributes.
        /// </summary>
        /// <param name="arlAttribute">The array list of attributes.</param>
        /// <returns></returns>
        private ArrayList SetSRPAttributes(ArrayList arlAttribute)
        {
            if(cboOperationalEnv.SelectedIndex != 0)
            {
                arlAttribute = SetUITextControls(cboOperationalEnv, arlAttribute);
            }
            if(cboReserveMagOil.SelectedIndex != 0)
            {
                arlAttribute = SetUITextControls(cboReserveMagOil, arlAttribute);
            }
            if(cboReserveMagGas.SelectedIndex != 0)
            {
                arlAttribute = SetUITextControls(cboReserveMagGas, arlAttribute);
            }
            if(cboTectonicSetting.SelectedIndex != 0)
            {
                arlAttribute = SetUITextControls(cboTectonicSetting, arlAttribute, GetTectonicSettingsBasinNames("Tectonic Setting", cboTectonicSetting.SelectedValue));
            }
            if(cboTectonicSettingKle.SelectedIndex != 0)
            {
                arlAttribute = SetUITextControls(cboTectonicSettingKle, arlAttribute, GetTectonicSettingsBasinNames("Tectonic Setting", cboTectonicSettingKle.SelectedValue));
                if(arlAttribute != null && arlAttribute.Count > 0)
                {
                    ((Attributes)arlAttribute[arlAttribute.Count - 1]).Name = GetControlID(cboTectonicSetting.ID);
                }
            }
            if(!string.IsNullOrEmpty(strBasinNameValue))
            {
                arlAttribute = SetUITextControls(radCboBasinName, arlAttribute, strBasinNameValue);
            }
            return arlAttribute;
        }


        /// <summary>
        /// Sets the tectonic settings.
        /// </summary>
        private void SetTectonicSettings()
        {
            if(string.Compare(rdblTectonicClassification.SelectedItem.Text, "Bally") == 0)
            {
                cboTectonicSettingKle.Attributes.Add("style", "Display:none");
            }
            else if(string.Compare(rdblTectonicClassification.SelectedItem.Text, "Klemme") == 0)
            {
                cboTectonicSetting.Attributes.Add("style", "Display:none");
            }
        }

        /// <summary>
        /// Resets the SRP controls.
        /// </summary>
        private void ResetSRPControls()
        {
            radCboFieldName.Items.Clear();
            radCboOperator.Items.Clear();
            if(blnSrpControlsEnable)
            {
                cboOperationalEnv.ClearSelection();
                cboReserveMagGas.ClearSelection();
                cboReserveMagOil.ClearSelection();
                cboTectonicSetting.ClearSelection();
                cboTectonicSettingKle.ClearSelection();
                rdblTectonicClassification.SelectedIndex = 0;
                radCboBasinName.Items.Clear();
                rdblSearchCond.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Attatches the javascript to controls.
        /// </summary>
        private void AttatchJavascriptToControls()
        {
            string strSrpControlsEnable = PortalConfiguration.GetInstance().GetKey(ENABLESRPSPECIFICCONTROLS);
            if(string.Equals(strSrpControlsEnable.ToLowerInvariant(), "yes"))
            {
                blnSrpControlsEnable = true;

            }
            hidSrpControlSectionId.Value = strSrpControlsEnable.ToLowerInvariant();
            StringBuilder strBasinOnclickJavascript = new StringBuilder();
            strBasinOnclickJavascript.Append("OpenSRPPopup('");
            strBasinOnclickJavascript.Append(radCboBasinName.ClientID);
            strBasinOnclickJavascript.Append("','Basin');");


            StringBuilder strOperatorOnclickJavascript = new StringBuilder();
            strOperatorOnclickJavascript.Append("OpenSRPPopup('");
            strOperatorOnclickJavascript.Append(radCboOperator.ClientID);
            strOperatorOnclickJavascript.Append("','Operator');");

            cmdBasinName.Attributes.Add(JAVASCRIPTONCLICK, strBasinOnclickJavascript.ToString());
            cmdOperator.Attributes.Add(JAVASCRIPTONCLICK, strOperatorOnclickJavascript.ToString());
            StringBuilder strSearchCriteria = new StringBuilder();
            strSearchCriteria.Append("javascript:FileSearchTypeSelectedIndexChange(this,'");
            strSearchCriteria.Append(txtField_Identifier.ID);
            strSearchCriteria.Append("|input')");
            cboSearchCriteria.Attributes.Add("onchange", strSearchCriteria.ToString());

            foreach(ListItem item in rdblTectonicClassification.Items)
            {
                StringBuilder strTeconicClassificationItem = new StringBuilder();
                strTeconicClassificationItem.Append("TechnoSettingsOnChangeVisible(this,'");
                strTeconicClassificationItem.Append(cboTectonicSetting.ClientID);
                strTeconicClassificationItem.Append("','");
                strTeconicClassificationItem.Append(cboTectonicSettingKle.ClientID);
                strTeconicClassificationItem.Append("');");
                item.Attributes.Add(JAVASCRIPTONCLICK, strTeconicClassificationItem.ToString());
            }

            StringBuilder strTextControlIds = new StringBuilder();
            strTextControlIds.Append(txtPorosity.ClientID);
            strTextControlIds.Append(";");
            strTextControlIds.Append(txtPermeability.ClientID);
            strTextControlIds.Append(";");
            strTextControlIds.Append(txtOilInPlace.ClientID);
            strTextControlIds.Append(";");
            strTextControlIds.Append(txtGasInitiallyInPlace.ClientID);
            strTextControlIds.Append(";");
            strTextControlIds.Append(txtCondensateInPlace.ClientID);
            strTextControlIds.Append(";");
            strTextControlIds.Append(txtCondensateRecovery.ClientID);
            strTextControlIds.Append(";");
            strTextControlIds.Append(txtNoOfWells.ClientID);
            strTextControlIds.Append(";");
            strTextControlIds.Append(txtNoOfProducers.ClientID);
            strTextControlIds.Append(";");
            strTextControlIds.Append(txtNoOfInjectors.ClientID);

            StringBuilder strSaveSearchJavaScript = new StringBuilder();
            strSaveSearchJavaScript.Append("if(!NumericValidation('");
            strSaveSearchJavaScript.Append(strTextControlIds.ToString());
            strSaveSearchJavaScript.Append("') )return false;if(!ValidateSaveSrchFIELD())return false;");
            StringBuilder strSearchJavaScript = new StringBuilder();
            strSearchJavaScript.Append("if(!NumericValidation('");
            strSearchJavaScript.Append(strTextControlIds.ToString());
            //strSearchJavaScript.Append("') )return false;if(!ValidateField(false))return false;");
            strSearchJavaScript.Append("') )return false;return ValidateFieldSearch();");//Dream 3.1 fix

            cmdSaveSearch.Attributes.Add(JAVASCRIPTONCLICK, strSaveSearchJavaScript.ToString());
            cmdSearch.Attributes.Add(JAVASCRIPTONCLICK, strSearchJavaScript.ToString());
            cmdSavedSearchButton.Attributes.Add(JAVASCRIPTONCLICK, strSearchJavaScript.ToString());//Dream 3.1 fix
            txtPorosity.Attributes.Add(SRPTEXTBOXALLOWEDDECIMALS, "2");
            txtPorosity.Attributes.Add(SRPTEXTBOXISRANGEALLOWED, "true");
            txtPorosity.Attributes.Add(SRPTEXTBOXMINIMUMVALUE, "0");
            txtPorosity.Attributes.Add(SRPTEXTBOXMAXIMUMVALUE, "50");
            txtPorosity.Attributes.Add(SRPTEXTBOXLABELNAME, "Porosity (Max)");
            txtPorosity.Attributes.Add(SRPTEXTBOXMEASURMENTS, "%");

            txtPermeability.Attributes.Add(SRPTEXTBOXALLOWEDDECIMALS, "4");
            txtPermeability.Attributes.Add(SRPTEXTBOXISRANGEALLOWED, "true");
            txtPermeability.Attributes.Add(SRPTEXTBOXMINIMUMVALUE, "0.0001");
            txtPermeability.Attributes.Add(SRPTEXTBOXMAXIMUMVALUE, "20000");
            txtPermeability.Attributes.Add(SRPTEXTBOXLABELNAME, "Permeabilty (Max)");
            txtPermeability.Attributes.Add(SRPTEXTBOXMEASURMENTS, "md");

            txtOilInPlace.Attributes.Add(SRPTEXTBOXALLOWEDDECIMALS, "2");
            txtOilInPlace.Attributes.Add(SRPTEXTBOXISRANGEALLOWED, "true");
            txtOilInPlace.Attributes.Add(SRPTEXTBOXMINIMUMVALUE, "0");
            txtOilInPlace.Attributes.Add(SRPTEXTBOXMAXIMUMVALUE, "infinite");
            txtOilInPlace.Attributes.Add(SRPTEXTBOXLABELNAME, "Oil In Place");
            txtOilInPlace.Attributes.Add(SRPTEXTBOXMEASURMENTS, "mmbbl");

            txtGasInitiallyInPlace.Attributes.Add(SRPTEXTBOXALLOWEDDECIMALS, "2");
            txtGasInitiallyInPlace.Attributes.Add(SRPTEXTBOXISRANGEALLOWED, "true");
            txtGasInitiallyInPlace.Attributes.Add(SRPTEXTBOXMINIMUMVALUE, "0");
            txtGasInitiallyInPlace.Attributes.Add(SRPTEXTBOXMAXIMUMVALUE, "infinite");
            txtGasInitiallyInPlace.Attributes.Add(SRPTEXTBOXLABELNAME, "Gas Initially In Place(GIIP)");
            txtGasInitiallyInPlace.Attributes.Add(SRPTEXTBOXMEASURMENTS, "MMScf");

            txtCondensateInPlace.Attributes.Add(SRPTEXTBOXALLOWEDDECIMALS, "2");
            txtCondensateInPlace.Attributes.Add(SRPTEXTBOXISRANGEALLOWED, "true");
            txtCondensateInPlace.Attributes.Add(SRPTEXTBOXMINIMUMVALUE, "0");
            txtCondensateInPlace.Attributes.Add(SRPTEXTBOXMAXIMUMVALUE, "infinite");
            txtCondensateInPlace.Attributes.Add(SRPTEXTBOXLABELNAME, "Condensate In Place");
            txtCondensateInPlace.Attributes.Add(SRPTEXTBOXMEASURMENTS, "MMbbl");

            txtCondensateRecovery.Attributes.Add(SRPTEXTBOXALLOWEDDECIMALS, "2");
            txtCondensateRecovery.Attributes.Add(SRPTEXTBOXISRANGEALLOWED, "true");
            txtCondensateRecovery.Attributes.Add(SRPTEXTBOXMINIMUMVALUE, "0");
            txtCondensateRecovery.Attributes.Add(SRPTEXTBOXMAXIMUMVALUE, "100");
            txtCondensateRecovery.Attributes.Add(SRPTEXTBOXLABELNAME, "Condensate Recovery Factor");
            txtCondensateRecovery.Attributes.Add(SRPTEXTBOXMEASURMENTS, "%");


            txtNoOfWells.Attributes.Add(SRPTEXTBOXALLOWEDDECIMALS, "0");
            txtNoOfWells.Attributes.Add(SRPTEXTBOXISRANGEALLOWED, "true");
            txtNoOfWells.Attributes.Add(SRPTEXTBOXMINIMUMVALUE, "0");
            txtNoOfWells.Attributes.Add(SRPTEXTBOXMAXIMUMVALUE, "99999");
            txtNoOfWells.Attributes.Add(SRPTEXTBOXLABELNAME, "No.of Wells");
            txtNoOfWells.Attributes.Add(SRPTEXTBOXMEASURMENTS, "");

            txtNoOfProducers.Attributes.Add(SRPTEXTBOXALLOWEDDECIMALS, "0");
            txtNoOfProducers.Attributes.Add(SRPTEXTBOXISRANGEALLOWED, "true");
            txtNoOfProducers.Attributes.Add(SRPTEXTBOXMINIMUMVALUE, "0");
            txtNoOfProducers.Attributes.Add(SRPTEXTBOXMAXIMUMVALUE, "99999");
            txtNoOfProducers.Attributes.Add(SRPTEXTBOXLABELNAME, "No.of Producers");
            txtNoOfProducers.Attributes.Add(SRPTEXTBOXMEASURMENTS, "");

            txtNoOfInjectors.Attributes.Add(SRPTEXTBOXALLOWEDDECIMALS, "0");
            txtNoOfInjectors.Attributes.Add(SRPTEXTBOXISRANGEALLOWED, "true");
            txtNoOfInjectors.Attributes.Add(SRPTEXTBOXMINIMUMVALUE, "0");
            txtNoOfInjectors.Attributes.Add(SRPTEXTBOXMAXIMUMVALUE, "99999");
            txtNoOfInjectors.Attributes.Add(SRPTEXTBOXLABELNAME, "No.of Injectors");
            txtNoOfInjectors.Attributes.Add(SRPTEXTBOXMEASURMENTS, "");


        }

        /// <summary>
        /// Method is used  to get radCombo boxes Selected Values from Hidden variable 
        /// <param name="value">The hidden variable value.</param>
        /// </summary>
        private void AssignRadValueFromHiddenVariable(string value)
        {
            if(value.Length > 0)
            {
                if(value.Contains("|"))
                {
                    foreach(string item in value.Split("|".ToCharArray()))
                    {

                        if(item.Contains(radCboBasinName.ID))
                        {
                            strBasinNameValue = item.Substring(radCboBasinName.ID.Length + 1);
                        }
                        else if(item.Contains(radCboFieldName.ID))
                        {
                            strFieldNameValue = item.Substring(radCboFieldName.ID.Length + 1);
                        }
                        else if(item.Contains(radCboOperator.ID))
                        {
                            strOperatorNameValue = item.Substring(radCboOperator.ID.Length + 1);
                        }

                    }

                }
            }

            if(string.IsNullOrEmpty(strBasinNameValue))
            {
                StringBuilder strBasinName = new StringBuilder();
                strBasinName.Append(radCboBasinName.Text);
                strBasinName.Append(";");
                strBasinName.Append(radCboBasinName.SelectedValue);
                strBasinNameValue = strBasinName.ToString();
            }
            if(string.IsNullOrEmpty(strOperatorNameValue))
            {
                StringBuilder strOperatorName = new StringBuilder();
                strOperatorName.Append(radCboOperator.Text);
                strOperatorName.Append(";");
                strOperatorName.Append(radCboOperator.SelectedValue);
                strOperatorNameValue = strOperatorName.ToString();
            }
            if(string.IsNullOrEmpty(strFieldNameValue))
            {
                StringBuilder strFieldName = new StringBuilder();
                strFieldName.Append(radCboFieldName.Text);
                strFieldName.Append(";");
                strFieldName.Append(radCboFieldName.SelectedValue);
                strFieldNameValue = strFieldName.ToString();
            }


        }

        /// <summary>
        ///  Query the names from webservice
        /// </summary>
        /// <param name="name">basin</param>
        private DataTable GetDataForComboBox(string userEnteredValue, string type)
        {
            //Commented in DREAM 4.0 for R5K changes
            /* if (string.Equals(type, BASINLIST))
             {
                 return GetBasinFromSPList(userEnteredValue);

             }
             else*/
            {
                ArrayList arrListValue = new ArrayList();
                objUIUtilities = new UIUtilities();
                objReportController = objFactory.GetServiceManager(REPORTSERVICE);
                XmlDocument xmlDoc = null;
                //Added in DREAM 4.0 for R5K changes
                //starts
                if(string.Equals(type, BASINLIST))
                {
                    arrListValue.Add(radCboBasinName);
                    SetListValues(arrListValue, userEnteredValue, BASINNAME);
                    xmlDoc = objReportController.GetSearchResults(objRequestInfo, intListValMaxRecord, BASINLIST, null, 0);
                    if(xmlDoc != null)
                        return GetRadDataSource(xmlDoc, GetXslForRadResponseXml(), BASINLIST).Tables[3];
                }
                //ends
                else if(string.Equals(type, FIELD))
                {
                    arrListValue.Add(radCboFieldName);
                    SetListValues(arrListValue, userEnteredValue,GetRadControlID(radCboFieldName.ID));
                    xmlDoc = objReportController.GetSearchResults(objRequestInfo, intListValMaxRecord, FIELD, null, 0);
                    if(xmlDoc != null)
                        return GetRadDataSource(xmlDoc, GetXslForRadResponseXml(), FIELD).Tables[3];
                }
                else
                {
                    arrListValue.Add(radCboOperator);
                    SetListValues(arrListValue, userEnteredValue, GetRadControlID(radCboOperator.ID));
                    objRequestInfo.Entity.Name = FIELDOPERATOR;
                    xmlDoc = objReportController.GetSearchResults(objRequestInfo, intListValMaxRecord, FIELDOPERATOR, null, 0);
                    if(xmlDoc != null)
                        return GetRadDataSource(xmlDoc, GetXslForRadResponseXml(), FIELDOPERATOR).Tables[3];

                }

            }
            return null;
        }

        /// <summary>
        ///  Transform the resposexml into formated xml.
        /// </summary>
        /// <param name="XmlDocument">The Search Respose xml for Rad Control</param>
        /// <param name="XmlTextReader">The TextReader object which contains XSl file  </param>
        private DataSet GetRadDataSource(XmlDocument objResponseXml, XmlTextReader objXmlTextReader, string fieldName)
        {
            XmlDocument xmlDocForXSL = new XmlDocument();
            StringWriter objStringWriter = new StringWriter();
            XslCompiledTransform objCompiledTransform = new XslCompiledTransform();
            XsltArgumentList xsltArgsList = new XsltArgumentList();
            DataSet dsOutput = new DataSet();
            XmlNodeReader xmlNodeReader = null;
            switch(fieldName)
            {
                case FIELD:
                    {
                        xsltArgsList.AddParam("PARAM1", string.Empty, "Field Name");
                        xsltArgsList.AddParam("PARAM2", string.Empty, "Field Identifier");
                        break;
                    }
                case FIELDOPERATOR:
                    {
                        xsltArgsList.AddParam("PARAM1", string.Empty, "Operator");
                        break;
                    }
                //Added in DREAM 4.0 for R5K changes
                //starts
                case BASINLIST:
                    {
                        xsltArgsList.AddParam("PARAM1", string.Empty, BASINNAME);
                        break;
                    }
                //Ends
            }
            //Commented in DREAM 4.0 for R5K Changes
            xmlDocForXSL.Load(objXmlTextReader);
            objCompiledTransform.Load(xmlDocForXSL);
            objCompiledTransform.Transform(objResponseXml, xsltArgsList, objStringWriter);
            objResponseXml.LoadXml(objStringWriter.ToString());
            xmlNodeReader = new XmlNodeReader(objResponseXml);
            dsOutput.ReadXml(xmlNodeReader);

            //Closing and disposing objects
            objXmlTextReader.Close();
            xmlNodeReader.Close();
            objStringWriter.Flush();
            objStringWriter.Close();
            objStringWriter.Dispose();
            return dsOutput;
        }

        /// <summary>
        /// Populates the RAD combo box.
        /// </summary>
        /// <param name="e">The <see cref="Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs"/> instance containing the event data.</param>
        /// <param name="sourceListName">Name of the source list.</param>
        /// <param name="valueFieldName">Name of the value field.</param>
        /// <param name="textFieldName">Name of the text field.</param>
        private void PopulateRadComboBox(RadComboBoxItemsRequestedEventArgs e, RadComboBox objRadComboBox, string sourceListName, string valueFieldName, string textFieldName)
        {
            DataTable dtFieldName = null;
            StringBuilder strUserEnteredValue = new StringBuilder();
            try
            {
                if(!string.IsNullOrEmpty(e.Text))
                {
                    strUserEnteredValue.Append(e.Text);
                    //Commented in DREAM 4.0 For R%K Changes
                    /*if(!sourceListName.Equals(BASINLIST))
                    {
                        if(!strUserEnteredValue.ToString().Contains("*"))
                        {
                            strUserEnteredValue.Append("*");
                        }
                    }
                    else
                    {
                        char[] charSpecialCharacter = { '*', '%' };
                        if(strUserEnteredValue.ToString().Contains("*") || strUserEnteredValue.ToString().Contains("%"))
                        {
                            strUserEnteredValue.Remove(strUserEnteredValue.ToString().IndexOfAny(charSpecialCharacter), (strUserEnteredValue.ToString().Length - strUserEnteredValue.ToString().IndexOfAny(charSpecialCharacter)));
                        }
                    }*/
                    //Added in DREAM 4.0 For R%K Changes
                    //Starts
                    if(!strUserEnteredValue.ToString().Contains("*"))
                    {
                        strUserEnteredValue.Append("*");
                    }
                    //Ends
                    dtFieldName = GetDataForComboBox(strUserEnteredValue.ToString(), sourceListName);
                    if(dtFieldName != null && dtFieldName.Rows.Count > 0)
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
                    }
                    else
                    {
                        e.Message = NORECORDFOUNDSEXCEPTIONMESSAGE;
                    }
                }
            }
            finally
            {
                if(dtFieldName != null)
                    dtFieldName.Dispose();
            }

        }

        #endregion
        #endregion
    }
}
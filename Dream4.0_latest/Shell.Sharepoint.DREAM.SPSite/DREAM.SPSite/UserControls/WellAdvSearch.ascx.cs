#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : WellAdvSearch.ascx.cs
#endregion

using System;
using System.Data;
using System.Collections;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Web.Services.Protocols;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.DREAM.Site.UI.UserControls
{
    /// <summary>
    /// Code behind for Advanced Search. This is  for Well  
    /// Advanced search types.
    /// </summary>
    public partial class WellAdvSearch :UIHelper
    {

        #region Declaration
        const string PAGENAME = "AdvSearchWell.aspx";
        const string WELLREPORT = "Well";
        const string WELLTYPESTATUS = "WellExistenceKind";
        const string WELLCHECKREFRESH = "WellCheckRefresh";
        const string SAVESEARCHNAMEQUERYSTRING = "savesearchname";
        const string ASSET = "asset";
        const string WELLASSET = "well";

        #endregion

        #region Event Handler Methods

        /// <summary>
        /// Handles the PreRender event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            object objCheckRefresh = CommonUtility.GetSessionVariable(Page, WELLCHECKREFRESH);
            //if the session has the CheckRefresh value then assign to ViewState.
            if(objCheckRefresh != null)
                ViewState[CHECKREFRESH] = (string)objCheckRefresh;
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Dream 4.0 
            lblCountryInfo.Text = "  Selecting the \"Country\" will activate the \"State or Province\" and  <br/> \"Field Name\" checkboxes.";
            lblStateInfo.Text = "  Selecting one or more \"State or Province\" entries will activate the <br/> \"County\" checkbox.";

            string strOnChange = string.Format("javascript:FileSearchTypeSelectedIndexChange(this,'{0}|input','{1}|input','{2}|input','{3}|input')", txtUnique_Well_Identifier.ID, txtWell_Name.ID, txtCommon_Name.ID, txtAlias_Name.ID);

            cboSearchCriteria.Attributes.Add("onchange", strOnChange);
            cmdSaveSearch.Attributes.Add("onclick", "return ValidateWellboreSaveSearch('" + WELLREPORT + "');");
            objUIUtilities = new UIUtilities();
            objMossController = objFactory.GetServiceManager(MOSSSERVICE);
            objReportController = objFactory.GetServiceManager(REPORTSERVICE);
            SetUserPreferences();
            //SetTextboxDefault(txtMaxLatDeg);//DREAM4.0 Auto default to zeros in adv search
            //SetTextboxDefault(txtMaxLatMin);
            //SetTextboxDefault(txtMaxLatSec);
            //SetTextboxDefault(txtMaxLonDeg);
            //SetTextboxDefault(txtMaxLonMin);
            //SetTextboxDefault(txtMaxLonSec);
            //SetTextboxDefault(txtMinLatDeg);
            //SetTextboxDefault(txtMinLatMin);
            //SetTextboxDefault(txtMinLatSec);
            //SetTextboxDefault(txtMinLonDeg);
            //SetTextboxDefault(txtMinLonMin);
            //SetTextboxDefault(txtMinLonSec);
            lblException.Text = string.Empty;
            lblMessage.Text = string.Empty;
            lblStateMessage.Visible = false;
            lblCountyMessage.Visible = false;
            lblField_Identifier.Visible = false;
            lblFormation.Visible = false;
            cmdSearch.Attributes.Add(REDIRECTATTRIBUTE, REDIRECTATTRIBUTEVALUE);
            cmdReset.Attributes.Add(REDIRECTATTRIBUTE, REDIRECTATTRIBUTEVALUE);
            cmdTopSearch.Attributes.Add(REDIRECTATTRIBUTE, REDIRECTATTRIBUTEVALUE);
            cmdTopReset.Attributes.Add(REDIRECTATTRIBUTE, REDIRECTATTRIBUTEVALUE);
            if(!Page.IsPostBack)
            {
                try
                {
                    //Sets the Well Check refresh session variable.
                    CommonUtility.SetSessionVariable(Page, WELLCHECKREFRESH, DateTime.Now.ToString());
                    if(Request.QueryString[ASSET] != null)
                    {
                        if(Request.QueryString[ASSET].ToLower().Equals(WELLASSET))
                        {
                            rdoRbDate.Items[1].Text = "Abandonment";
                            rdoRbDate.Items[1].Value = "Abandoned";
                            ((MOSSServiceManager)objMossController).LoadSaveSearch(WELLADVANCEDSEARCH, cboSavedSearch);
                        }
                        else
                        {
                            RenderException("No items to display in this view.");
                        }

                        LoadOtherPageControls();
                        GetAssetColumns(REPORTSERVICECOLUMNLIST, cboSearchCriteria, Request.QueryString[ASSET].ToString());
                    }
                    else
                    {
                        RenderException("No items to display in this view.");
                    }
                }
                catch(WebException webEx)
                {
                    RenderException(webEx.Message.ToString());
                }
                catch(Exception ex)
                {
                    CommonUtility.HandleException(strCurrSiteUrl, ex);
                }
            }
            //DREAM 4.0 added for adv search country dropdown
            objUtility.RegisterOnLoadClientScript(this.Page, "if(document.getElementById(GetObjectID('chbGeographicalSearch', 'input')).checked == true) {showLatLongTable('TR1','TR2','TR3','TR4','TR5','TR6','TR7','TR8');}");
        }

        /// <summary>
        /// Handles the Click event of the cmdSearch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            hidSearchName.Value = ADVANCEDSEARCH;
            try
            {
                if(Page.IsPostBack)
                {
                    DisplaySearchResults();
                }
            }
            catch(SoapException soapEx)
            {
                if(!string.Equals(soapEx.Message.ToString(), SOAPEXCEPTIONMESSAGE))
                {
                    CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                }
                RenderException(soapEx.Message.ToString());
            }
            catch(WebException webEx)
            {
                RenderException(webEx.Message.ToString());
            }
            catch(Exception ex)
            {
                if(string.Equals(ex.Message, BLANKFILEMESSAGE))
                {
                    GeneralExceptionBlock.Visible = true;
                    lblException.Visible = true;
                    lblException.Text = BLANKFILEMESSAGE;
                }
                else
                    CommonUtility.HandleException(strCurrSiteUrl, ex);
            }

        }

        /// <summary>
        /// Handles the Change event of the chbState control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void chbState_Change(object sender, EventArgs e)
        {
            ArrayList arrListValue = new ArrayList();
            XmlDocument xmlDoc = new XmlDocument();
            lblStateMessage.Visible = false;
            lblCountyMessage.Visible = false;
            lblField_Identifier.Visible = false;
            lblFormation.Visible = false;

            try
            {
                if(chbState.Checked)
                {
                    arrListValue.Add(lstCountry);
                    lstState_Or_Province.Items.Clear();

                    SetListValues(arrListValue); //parameter for which data needs to be populated.          
                    xmlDoc = objReportController.GetSearchResults(objRequestInfo, intListValMaxRecord, STATE, null, 0);
                    if(xmlDoc != null)
                    {
                        objUIUtilities.AssignValuesToDropDown(xmlDoc, lstState_Or_Province);
                    }
                    if(lstState_Or_Province.Items.Count < 1)
                    {
                        lblStateMessage.Visible = true;
                        lblStateMessage.Text = "No state or province has been found for the selected country.";
                        lblStateMessage.ForeColor = System.Drawing.Color.Red;
                    }

                }
                else
                {
                    lstState_Or_Province.Items.Clear();
                    lstCounty.Items.Clear();
                    chbCounty.Checked = false;
                    chbCounty.Enabled = false;
                }
            }
            catch(SoapException soapEx)
            {
                if(!string.Equals(soapEx.Message.ToString(), SOAPEXCEPTIONMESSAGE))
                {
                    CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                }
                lblStateMessage.Visible = true;
                if(string.Equals(soapEx.Message.ToString(), SOAPEXCEPTIONMESSAGE))
                {
                    lblStateMessage.Text = "No state or province has been found for the selected country.";
                }
                else
                {
                    lblStateMessage.Text = "No state or province has been found for the selected country.";
                }
                lblStateMessage.ForeColor = System.Drawing.Color.Red;
            }
            catch(WebException webEx)
            {
                RenderException(webEx.Message.ToString());
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }

        /// <summary>
        /// Handles the Change event of the chbCounty control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void chbCounty_Change(object sender, EventArgs e)
        {
            try
            {
                lblStateMessage.Visible = false;
                lblCountyMessage.Visible = false;
                lblField_Identifier.Visible = false;
                lblFormation.Visible = false;

                //Based on Country check box loads the state values.
                if(chbCounty.Checked)
                {
                    lstCounty.Enabled = true;
                    ArrayList arrListValue = new ArrayList();
                    XmlDocument xmlDoc = new XmlDocument();

                    arrListValue.Add(lstCountry);
                    arrListValue.Add(lstState_Or_Province);
                    SetListValues(arrListValue);
                    xmlDoc = objReportController.GetSearchResults(objRequestInfo, intListValMaxRecord, COUNTY, null, 0);

                    lstCounty.Items.Clear();
                    if(xmlDoc != null)
                    {
                        objUIUtilities.AssignValuesToDropDown(xmlDoc, lstCounty);
                    }
                    if(lstCounty.Items.Count < 1)
                    {
                        lblCountyMessage.Visible = true;
                        lblCountyMessage.Text = "No County has been found for the selected State or Province.";
                        lblCountyMessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
                else
                {
                    lstCounty.Items.Clear();
                }
            }
            catch(WebException webEx)
            {
                RenderException(webEx.Message.ToString());
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }

        /// <summary>
        /// Handles the Change event of the chbField control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void chbField_Change(object sender, EventArgs e)
        {
            ArrayList arrListValue = new ArrayList();
            XmlDocument xmlDoc = new XmlDocument();
            lblStateMessage.Visible = false;
            lblCountyMessage.Visible = false;
            lblField_Identifier.Visible = false;
            lblFormation.Visible = false;
            //check for the Field_Identified value based on which values will be loaded.
            if(chbField_Identifier.Checked)
            {
                arrListValue.Add(lstCountry);
                lstField_Identifier.Items.Clear();
                SetListValues(arrListValue); //parameter for which data needs to be populated.            
                try
                {
                    xmlDoc = objReportController.GetSearchResults(objRequestInfo, intListValMaxRecord, FIELD, null, 0);
                    if(xmlDoc != null)
                    {
                        objUIUtilities.AssignFieldValuesToDropDown(xmlDoc, lstField_Identifier);
                    }
                    if(lstField_Identifier.Items.Count < 1)
                    {
                        lblField_Identifier.Visible = true;
                        lblField_Identifier.Text = "No Field has been found for the selected country.";
                        lblField_Identifier.ForeColor = System.Drawing.Color.Red;
                    }
                }
                catch(SoapException soapEx)
                {
                    if(!string.Equals(soapEx.Message.ToString(), SOAPEXCEPTIONMESSAGE))
                    {
                        CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                    }
                    lblField_Identifier.Visible = true;
                    if(string.Equals(soapEx.Message.ToString(), SOAPEXCEPTIONMESSAGE))
                    {
                        lblField_Identifier.Text = "No Field has been found for the selected country.";
                    }
                    else
                    {
                        lblField_Identifier.Text = "No Field has been found for the selected country.";
                    }
                    lblField_Identifier.ForeColor = System.Drawing.Color.Red;
                }
                catch(WebException webEx)
                {
                    RenderException(webEx.Message.ToString());
                }
                catch(Exception ex)
                {
                    CommonUtility.HandleException(strCurrSiteUrl, ex);
                }
            }
            else
            {
                lstField_Identifier.Items.Clear();
            }
        }

        /// <summary>
        /// Handles the Change event of the chbFormation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void chbFormation_Change(object sender, EventArgs e)
        {
            try
            {
                lblStateMessage.Visible = false;
                lblCountyMessage.Visible = false;
                lblField_Identifier.Visible = false;
                lblFormation.Visible = false;

                if(chbFormation.Checked)
                {
                    lstPick.Items.Clear();

                    PopulateFieldData(lstPick, FORMATION);
                }
                else
                {
                    lstPick.Items.Clear();
                }
            }
            catch(WebException webEx)
            {
                RenderException(webEx.Message.ToString());
            }
            catch(Exception ex)
            {
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
            string strUrl = string.Empty;
            if(string.Equals(cmdSaveSearch.Text.ToString(), MODIFYSRCH))
            {
                if(Request.QueryString[SAVESEARCHNAMEQUERYSTRING] != null)
                {

                    ///<TODO>
                    /// Check how to avoid hard coding the "asset" query string
                    /// </TODO>
                    strUrl = PAGENAME + "?asset=Well&manage=true&savesearchname=" + Request.QueryString[SAVESEARCHNAMEQUERYSTRING].ToString() + "&operation=modify";
                }
                else
                {
                    ///<TODO>
                    /// Check how to avoid hard coding the "asset" query string
                    /// </TODO>
                    strUrl = PAGENAME + "?asset=Well";
                }
            }
            else
            {
                ///<TODO>
                /// Check how to avoid hard coding the "asset" query string
                /// </TODO>
                strUrl = PAGENAME + "?asset=Well";
            }
            RedirectPage(strUrl, "Well");
        }

        /// <summary>
        /// Handles the Click event of the cmdSearch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmdSaveSearch_Click(object sender, EventArgs e)
        {
            bool blnIsNameExist = false;
            try
            {
                lblException.Visible = false;
                object objCheckRefresh = CommonUtility.GetSessionVariable(Page, WELLCHECKREFRESH);
                if(string.Equals(cmdSaveSearch.Text.ToString(), MODIFYSRCH))
                {
                    //Checks whether the event is fired or page has been refreshed.
                    if(string.Equals(ViewState[CHECKREFRESH].ToString(), (string)objCheckRefresh))
                    {
                        CommonUtility.SetSessionVariable(Page, WELLCHECKREFRESH, DateTime.Now.ToString());
                        string strSaveSearchName = txtSaveSearch.Text.Trim();
                        XmlDocument xmlDocSearchRequest = null;
                        xmlDocSearchRequest = CreateSaveSearchXML();
                        UISaveSearchHandler objUISaveSearchHandler = null;

                        objUISaveSearchHandler = new UISaveSearchHandler();

                        objUISaveSearchHandler.ModifySaveSearchXML(WELLADVANCEDSEARCH, strSaveSearchName, chbShared.Checked, cboSavedSearch, xmlDocSearchRequest);


                        if(cboSavedSearch.Items.FindByText(DEFAULTDROPDOWNTEXT) == null)
                        {
                            cboSavedSearch.Items.Insert(0, DEFAULTDROPDOWNTEXT);
                        }

                    }
                    else
                    {
                        cboSavedSearch.Items.Clear();
                        cboSavedSearch.Items.Add(DEFAULTDROPDOWNTEXT);

                        ((MOSSServiceManager)objMossController).LoadSaveSearch(WELLADVANCEDSEARCH, cboSavedSearch);


                    }
                    cboSavedSearch.Text = txtSaveSearch.Text.ToString().Trim();
                    txtSaveSearch.Text = string.Empty;
                    txtSaveSearch.Enabled = true;
                    cmdSaveSearch.Text = "Save Search";
                }
                else /// Save Search
                {
                    ArrayList arlSavedSearch = new ArrayList();
                    //Checks whether the event is fired or page has been refreshed.
                    if(string.Equals(ViewState[CHECKREFRESH].ToString(), (string)objCheckRefresh))
                    {
                        CommonUtility.SetSessionVariable(Page, WELLCHECKREFRESH, DateTime.Now.ToString());
                        string strSaveSearchName = txtSaveSearch.Text.Trim();

                        arlSavedSearch = ((MOSSServiceManager)objMossController).GetSaveSearchName(WELLADVANCEDSEARCH, GetUserName());


                        if(IsDuplicateNameExist(arlSavedSearch, strSaveSearchName))
                        {
                            GeneralExceptionBlock.Visible = true;
                            lblException.Visible = true;
                            lblException.Text = ((MOSSServiceManager)objMossController).GetCustomExceptionMessage(strCurrSiteUrl, "12");
                            blnIsNameExist = true;
                        }
                        else
                        {
                            XmlDocument xmlDocSearchRequest = null;
                            xmlDocSearchRequest = CreateSaveSearchXML();
                            UISaveSearchHandler objUISaveSearchHandler = null;

                            objUISaveSearchHandler = new UISaveSearchHandler();


                            objUISaveSearchHandler.SaveSearchXMLToLibrary(WELLADVANCEDSEARCH, strSaveSearchName, chbShared.Checked, cboSavedSearch, xmlDocSearchRequest);


                        }
                    }
                    else
                    {
                        cboSavedSearch.Items.Clear();
                        cboSavedSearch.Items.Add(DEFAULTDROPDOWNTEXT);


                        ((MOSSServiceManager)objMossController).LoadSaveSearch(WELLADVANCEDSEARCH, cboSavedSearch);


                    }
                    if(!blnIsNameExist)
                    {
                        if(cboSavedSearch.Items.FindByText(txtSaveSearch.Text.ToString().Trim()) != null)
                        {
                            cboSavedSearch.Items.FindByText(txtSaveSearch.Text.ToString().Trim()).Selected = true;
                        }
                    }
                    txtSaveSearch.Text = string.Empty;
                }
            }
            catch(WebException webEx)
            {
                RenderException(webEx.Message.ToString());
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }

        /*********************************************************************************
       *   Data Object Process code Ends here...
       * ******************************************************************************/


        /// <summary>
        /// Handles the SelectedIndexChanged event of the cboSavedSearch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>       
        protected void cboSavedSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strUserID = GetUserName();
                XmlDocument xmldoc = new XmlDocument();

                xmldoc = ((MOSSServiceManager)objMossController).GetDocLibXMLFile(WELLADVANCEDSEARCH, strUserID);

                ClearUIControls();

                LoadControlsOnPageLoad();
                if((cboSavedSearch.SelectedIndex != 0) || (string.Equals(cmdSaveSearch.Text.ToString(), MODIFYSRCH)))
                {
                    BindUIControls(xmldoc, cboSavedSearch.SelectedItem.Text.ToString());

                    if(string.Equals(cmdSaveSearch.Text.ToString(), MODIFYSRCH))
                    {
                        txtSaveSearch.Text = cboSavedSearch.Text;
                    }
                }

            }
            catch(WebException webEx)
            {
                RenderException(webEx.Message.ToString());
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the lstCountry control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lstCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblStateMessage.Visible = false;
                lblCountyMessage.Visible = false;
                lblField_Identifier.Visible = false;
                lblFormation.Visible = false;

                lstState_Or_Province.Items.Clear();
                lstState_Or_Province.Enabled = false;
                lstField_Identifier.Items.Clear();
                lstField_Identifier.Enabled = false;

                lstCounty.Items.Clear();
                lstCounty.Enabled = false;

                chbState.Checked = false;
                chbState.Enabled = false;

                chbField_Identifier.Checked = false;
                chbField_Identifier.Enabled = false;

                chbCounty.Checked = false;
                chbCounty.Enabled = false;


                if(lstCountry.SelectedIndex == 0)
                {
                    lstState_Or_Province.Enabled = false;
                    lstField_Identifier.Enabled = false;
                }
                else
                {
                    lstState_Or_Province.Enabled = true;
                    lstField_Identifier.Enabled = true;

                    chbState.Enabled = true;
                    chbField_Identifier.Enabled = true;
                }
            }
            catch(WebException webEx)
            {
                RenderException(webEx.Message.ToString());
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the lstState_Or_Province control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lstState_Or_Province_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblStateMessage.Visible = false;
            lblCountyMessage.Visible = false;
            lblField_Identifier.Visible = false;
            lblFormation.Visible = false;
            try
            {
                lblCountyMessage.Visible = false;
                lstCounty.Items.Clear();
                lstCounty.Enabled = false;
                chbCounty.Checked = false;
                chbCounty.Enabled = false;

                if(IsOptionSelected(lstState_Or_Province))
                {
                    chbCounty.Enabled = true;
                }
            }
            catch(WebException webEx)
            {
                RenderException(webEx.Message.ToString());
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }


        #endregion Event Handler Methods

        #region Private Methods
        /// <summary>
        /// Loads the other page controls.
        /// </summary>
        private void LoadOtherPageControls()
        {
            string strUserID = string.Empty;
            string strSaveSearchName = string.Empty;
            string strAdvSearchName = string.Empty;
            XmlDocument xmldoc = new XmlDocument();
            chbFormation.Enabled = true;
            LoadControlsOnPageLoad();
            BindTooltipTextToControls();
            if(Request.QueryString[SAVESEARCHNAMEQUERYSTRING] != null)
            {

                if(Request.QueryString["manage"] != null)
                {
                    strSaveSearchName = Request.QueryString[SAVESEARCHNAMEQUERYSTRING].ToString();
                    strAdvSearchName = WELLADVANCEDSEARCH;

                    if(Request.QueryString["manage"].ToLower().Equals("true"))
                    {
                        strUserID = GetUserName();
                        xmldoc = ((MOSSServiceManager)objMossController).GetDocLibXMLFile(strAdvSearchName, strUserID);
                        if(IsValidSaveSearchName(strSaveSearchName, xmldoc, false))
                        {

                            BindUIControls(xmldoc, strSaveSearchName);
                            cboSavedSearch.Text = strSaveSearchName;
                            if(Request.QueryString["operation"] != null)
                            {
                                if(string.Equals(Request.QueryString["operation"].ToString(), "modify"))
                                {

                                    cmdSaveSearch.Text = MODIFYSRCH;
                                    txtSaveSearch.Text = strSaveSearchName;
                                    txtSaveSearch.Enabled = false;
                                    cboSavedSearch.Items.RemoveAt(0);//removing select option in modify mode
                                }
                            }

                        }
                        else
                        {
                            RenderException("Save Search name doesnot exist.");
                        }


                    }
                    else if(Request.QueryString["manage"].ToLower().Equals("false"))
                    {
                        strUserID = ADMINUSER;
                        xmldoc = ((MOSSServiceManager)objMossController).GetDocLibXMLFile(strAdvSearchName, strUserID);
                        if(IsValidSaveSearchName(strSaveSearchName, xmldoc, true))
                        {
                            BindUIControls(xmldoc, strSaveSearchName);
                        }
                        else
                        {
                            RenderException("Save Search name doesnot exist.");
                        }
                    }
                    else
                    {
                        RenderException("No items to display in this view.");
                    }

                }

            }
            strUserID = GetUserName();
            //this will check the condition for logged in user and based on that Shared Save Search check box will be visible.
            if(string.Equals(strUserID, ADMINUSER))
            {
                chbShared.Visible = true;
            }
            else
            {
                chbShared.Visible = false;
            }
        }

        /// <summary>
        /// Determines whether [is valid save search name] [the specified save search name].
        /// </summary>
        /// <param name="saveSearchName">Name of the save search.</param>
        /// <param name="saveSearchDoc">The save search doc.</param>
        /// <param name="isManage">if set to <c>true</c> [is manage].</param>
        /// <returns>
        /// 	<c>true</c> if [is valid save search name] [the specified save search name]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidSaveSearchName(string saveSearchName, XmlDocument saveSearchDoc, bool isManage)
        {
            bool blnValidSaveSearchName = false;

            string strNodeSearchPath = SAVESHAREDSEARCHXSLPATH + "[(@" + ATTRIBNAME + "=\"" + saveSearchName + "\")";
            if(isManage)
            {
                strNodeSearchPath += "and(@" + ATTRIBSHARED + "=\"True\")]";
            }
            else
            {
                strNodeSearchPath += "]";
            }
            XmlNodeList xmlnodelistSharedSaveSearch = saveSearchDoc.SelectNodes(strNodeSearchPath);
            if(xmlnodelistSharedSaveSearch.Count > 0)
            {
                blnValidSaveSearchName = true;
            }
            else
            {
                blnValidSaveSearchName = false;
            }
            return blnValidSaveSearchName;
        }

        /// <summary>
        /// Method is use to bind the tooltips to the controls
        /// </summary>
        private void BindTooltipTextToControls()
        {
            DataTable dtFilterTooltip = null;
            string strControlName = string.Empty;
            string strTooltipText = string.Empty;
            string strAsset = WELLREPORT;
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
                        if(string.Equals(imgUWI.ID.Remove(0, 3).ToString(), strControlName))
                        {
                            imgUWI.ToolTip = strTooltipText;
                        }

                        if(string.Equals(imgBasin.ID.Remove(0, 3).ToString(), strControlName))
                        {
                            imgBasin.ToolTip = strTooltipText;
                        }
                        if(string.Equals(imgCountry.ID.Remove(0, 3).ToString(), strControlName))
                        {
                            imgCountry.ToolTip = strTooltipText;
                        }
                        else if(imgStateOrProvince.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgStateOrProvince.ToolTip = strTooltipText;
                        }
                        else if(imgCounty.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgCounty.ToolTip = strTooltipText;
                        }
                        else if(imgFieldName.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgFieldName.ToolTip = strTooltipText;
                        }
                        else if(imgOnshoreOrOffshore.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgOnshoreOrOffshore.ToolTip = strTooltipText;
                        }
                        else if(imgWellType.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgWellType.ToolTip = strTooltipText;
                        }
                        else if(imgFormation.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgFormation.ToolTip = strTooltipText;
                        }
                        else if(imgOperator.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgOperator.ToolTip = strTooltipText;
                        }
                        else if(imgWellStatus.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgWellStatus.ToolTip = strTooltipText;
                        }
                        else if(imgSearchName.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgSearchName.ToolTip = strTooltipText;
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
        /// Loads the controls on page load.
        /// </summary>
        private void LoadControlsOnPageLoad()
        {
            try
            {
                //Initializing Country & Basin controls.
                //**R5k changes for Dream 4.0
                //Starts
                //LoadCountryBasinData(BASINLIST, BASINITEMVAL, lstBasin);
                PopulateListControl(lstBasin, GetBasinFromWebService(), BASINXPATH, objUserPreferences.Basin);
                //Ends
                LoadCountryBasinData(COUNTRYLIST, COUNTRYLIST, lstCountry);
                //Dream4.0code added for adv search country dropdown
                lstCountry.Items.Insert(0, DEFAULTDROPDOWNTEXT);
                if(lstCountry.SelectedIndex >= 0)
                {
                    lstCountry_SelectedIndexChanged(null, EventArgs.Empty);
                }
                /// Populate Picks list box
                PopulateFormationData(lstPick);
                /// Populate Onshore/Offshore list box
                PopulateFieldData(lstOnshore_Or_Offshore, ONSHOREOFFSHORE);
                /// Populate Kind list box
                PopulateFieldData(lstKind, KIND);
                /// Populate Well/WellBore Status list box
                PopulateFieldData(lstCurrent_Status, WELLBORESTATUS);

            }
            catch(SoapException soapEx)
            {
                if(!string.Equals(soapEx.Message.ToString(), SOAPEXCEPTIONMESSAGE))
                {
                    CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                }
                RenderException(soapEx.Message.ToString());
            }
            catch(WebException webEx)
            {
                RenderException(webEx.Message.ToString());
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }

        /// <summary>
        /// Renders the exception.
        /// </summary>
        /// <param name="message">The message.</param>
        private void RenderException(string message)
        {
            lblSOAPException.Text = message;
            lblSOAPException.Visible = true;
            AdvancedSearchContent.Visible = false;
            ExceptionBlock.Visible = true;
        }

        /// <summary>
        /// Creates the save search XML.
        /// </summary>
        private XmlDocument CreateSaveSearchXML()
        {
            XmlDocument xmlDocSearchRequest = null;
            objRequestInfo = new RequestInfo();
            objRequestInfo = SetBasicDataObjects();
            /// Creates the search request XML.
            xmlDocSearchRequest = objReportController.CreateSearchRequest(objRequestInfo);
            return xmlDocSearchRequest;
        }

        /*********************************************************************************
        *   Data Object Process code starts here...
        * ******************************************************************************/

        /// <summary>
        /// Populates the field data.
        /// </summary>
        /// <param name="listBoxControl">The list box control.</param>
        private void PopulateFieldData(ListBox listBoxControl, string fieldName)
        {
            ArrayList arlListValue = new ArrayList();
            XmlDocument objXmlDoc = new XmlDocument();
            /// Loads the values for the list.
            SetListValues(arlListValue);
            if(fieldName.Equals(WELLBORESTATUS))
            {
                objRequestInfo.Entity.Name = WELLTYPESTATUS;
            }
            objXmlDoc = objReportController.GetSearchResults(objRequestInfo, intListValMaxRecord, fieldName, null, 0);
            if(objXmlDoc != null)
            {
                objUIUtilities.AssignValuesToDropDown(objXmlDoc, listBoxControl);
            }
        }

        /// <summary>
        /// Populates the formation data.
        /// </summary>
        /// <param name="listBoxControl">The list box control.</param>
        private void PopulateFormationData(ListBox listBoxControl)
        {
            ArrayList arrListValue = new ArrayList();
            XmlDocument xmlDoc = new XmlDocument();
            if(chbFormation.Checked)
            {

                SetListValues(arrListValue); //parameter for which data needs to be populated.            
                try
                {
                    xmlDoc = objReportController.GetSearchResults(objRequestInfo, intListValMaxRecord, FORMATION, null, 0);
                    if(xmlDoc != null)
                    {
                        objUIUtilities.AssignValuesToDropDown(xmlDoc, listBoxControl);
                    }
                    if(lstPick.Items.Count < 1)
                    {
                        lblFormation.Visible = true;
                        lblFormation.Text = "No picks formation has been found.";
                        lblFormation.ForeColor = System.Drawing.Color.Red;
                    }
                }
                catch(SoapException soapEx)
                {
                    if(!string.Equals(soapEx.Message.ToString(), SOAPEXCEPTIONMESSAGE))
                    {
                        CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                    }
                    lblFormation.Visible = true;
                    lblFormation.Text = "Not able to fecth data,please try again.";
                    lblFormation.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        /// <summary>
        /// Sets the basic data objects.
        /// </summary>
        /// <param name="strRequestInfo">The STR request info.</param>
        /// <returns></returns>
        private RequestInfo SetBasicDataObjects()
        {
            objRequestInfo.Entity = SetEntity();
            return objRequestInfo;
        }

        /// <summary>
        /// Sets the list values.
        /// </summary>
        /// <param name="arrFields">The arr fields.</param>
        /// <param name="lbControl">The lb control.</param>
        /// <returns></returns>
        private RequestInfo SetListValues(ArrayList fieldsGroup)
        {
            objRequestInfo.Entity = SetEntity(fieldsGroup);
            return objRequestInfo;
        }

        /// <summary>
        /// Sets the entity.
        /// </summary>
        /// <param name="arrFields">The arr fields.</param>
        /// <param name="lbControl">The lb control.</param>
        /// <returns></returns>
        private Entity SetEntity(ArrayList fieldsGroup)
        {
            Entity objEntity = new Entity();
            AttributeGroup objBasicAttributeGroup = new AttributeGroup();
            if(fieldsGroup.Count > 1)
            {
                ArrayList arlBasicAttributeGroup = new ArrayList();
                ArrayList arlAttribute = new ArrayList();
                objBasicAttributeGroup.Operator = GetLogicalOperator();
                //Loop through the items in the FieldsGroup.
                foreach(ListBox lbItem in fieldsGroup)
                {
                    Attributes objAttribute = new Attributes();
                    objAttribute.Name = GetControlID(lbItem.ID);
                    ArrayList arrValue = new ArrayList();

                    for(int intCounter = 0; intCounter < lbItem.Items.Count; intCounter++)
                    {
                        if(lbItem.Items[intCounter].Selected)
                        {
                            Value objValue = new Value();
                            objValue.InnerText = lbItem.Items[intCounter].Value;
                            arrValue.Add(objValue);
                        }
                    }
                    if(arrValue.Count == 0)
                    {
                        Value objValue = new Value();
                        objValue.InnerText = AMPERSAND;
                        arrValue.Add(objValue);
                    }
                    objAttribute.Value = arrValue;
                    objAttribute.Operator = GetOperator(objAttribute.Value);
                    arlAttribute.Add(objAttribute);
                }
                objBasicAttributeGroup.Attribute = arlAttribute;
                arlBasicAttributeGroup.Add(objBasicAttributeGroup);
                objEntity.AttributeGroups = arlBasicAttributeGroup;

                objEntity.Criteria = SetCriteria();
            }
            else if(fieldsGroup.Count == 1)
            {
                ArrayList arlBasicAttributeGroup = new ArrayList();
                ArrayList arlAttribute = new ArrayList();
                Attributes objAttribute = new Attributes();
                objBasicAttributeGroup.Operator = GetLogicalOperator();
                objAttribute.Name = GetControlID(lstCountry.ID);
                ArrayList arlValue = new ArrayList();
                for(int intCounter = 0; intCounter < lstCountry.Items.Count; intCounter++)
                {
                    if(lstCountry.Items[intCounter].Selected)
                    {
                        Value objValue = new Value();
                        objValue.InnerText = lstCountry.Items[intCounter].Value;
                        arlValue.Add(objValue);
                    }
                }
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
        /// Sets the entity.
        /// </summary>
        /// <returns></returns>
        private Entity SetEntity()
        {
            Entity objEntity = new Entity();
            objEntity.Property = true;
            objEntity.Type = WELLREPORT;
            ArrayList arlParentAttributeGroup = new ArrayList();
            ArrayList arrBasicAttributeGroup = new ArrayList();
            arrBasicAttributeGroup = SetBasicAttributeGroup();
            AttributeGroup objDateGroup = new AttributeGroup();
            for(int intDate = 0; intDate < rdoRbDate.Items.Count; intDate++)
            {
                if(rdoRbDate.Items[intDate].Selected)
                {
                    objDateGroup = SetDateAttributeGroup();
                }
            }
            if(objDateGroup.AttributeGroups != null)
            {
                arrBasicAttributeGroup.Add(objDateGroup);
            }
            AttributeGroup objGeologicalGroup = new AttributeGroup();
            if(rdoLatLong.Checked)
            {
                objGeologicalGroup = SetGeographicalAttributeGroup();
            }
            if(objGeologicalGroup.AttributeGroups != null)
            {
                arrBasicAttributeGroup.Add(objGeologicalGroup);
            }
            if(arrBasicAttributeGroup.Count > 1)
            {
                AttributeGroup objAttributeGroup = new AttributeGroup();
                objAttributeGroup.Operator = GetLogicalOperator();
                objAttributeGroup.AttributeGroups = arrBasicAttributeGroup;
                arlParentAttributeGroup.Add(objAttributeGroup);
                objEntity.AttributeGroups = arlParentAttributeGroup;
            }
            else
            {
                objEntity.AttributeGroups = arrBasicAttributeGroup;
            }
            return objEntity;
        }

        /// <summary>
        /// Sets the basic attribute group.
        /// </summary>
        /// <returns></returns>
        private ArrayList SetBasicAttributeGroup()
        {
            AttributeGroup objBasicAttributeGroup = new AttributeGroup();
            objBasicAttributeGroup.Operator = GetLogicalOperator();

            objBasicAttributeGroup.Name = WELLREPORT;

            objBasicAttributeGroup.Checked = TRUE;
            objBasicAttributeGroup.Attribute = SetBasicAttribute();

            ArrayList arrBasicAttributeGroup = new ArrayList();
            arrBasicAttributeGroup.Add(objBasicAttributeGroup);

            return arrBasicAttributeGroup;
        }

        /// <summary>
        /// Sets the date attribute group.
        /// </summary>
        /// <returns></returns>
        private AttributeGroup SetDateAttributeGroup()
        {
            AttributeGroup objParentAttributeGroup = new AttributeGroup();
            if(rdoRbDate.Items[0].Selected)
            {
                objParentAttributeGroup.Operator = GetLogicalOperator();
                objParentAttributeGroup.Name = rdoRbDate.Items[0].Value.ToString();
                objParentAttributeGroup.Label = rdoRbDate.ID;
                objParentAttributeGroup.Checked = TRUE;
                objParentAttributeGroup.AttributeGroups = SetDateAttribute();
            }
            else
            {
                objParentAttributeGroup.Operator = GetLogicalOperator();
                objParentAttributeGroup.Name = rdoRbDate.Items[1].Value.ToString();
                objParentAttributeGroup.Label = rdoRbDate.ID;
                objParentAttributeGroup.Checked = TRUE;
                objParentAttributeGroup.AttributeGroups = SetDateAttribute();
            }
            return objParentAttributeGroup;
        }

        /// <summary>
        /// Sets the date attribute.
        /// </summary>
        /// <returns></returns>
        private ArrayList SetDateAttribute()
        {
            ArrayList arrDate = new ArrayList();
            if(rdoRbDate.Items[0].Selected)
            {
                ArrayList arrSpudDate = new ArrayList();
                AttributeGroup objFirstChildAttributeGroup = new AttributeGroup();
                objFirstChildAttributeGroup.Operator = GetLogicalOperator();

                Attributes objFromSpudDate = new Attributes();
                objFromSpudDate.Name = SPUDDATE;
                objFromSpudDate.Label = txtFrom.ID;
                objFromSpudDate.Operator = GetDateOperator(FROM);
                objFromSpudDate.Value = SetDateValue(FROM, txtFrom.Text, txtTo.Text);
                arrSpudDate.Add(objFromSpudDate);

                Attributes objToSpudDate = new Attributes();
                objToSpudDate.Name = SPUDDATE;
                objToSpudDate.Label = txtTo.ID;
                objToSpudDate.Operator = GetDateOperator(TO);
                objToSpudDate.Value = SetDateValue(TO, txtFrom.Text, txtTo.Text);
                arrSpudDate.Add(objToSpudDate);

                objFirstChildAttributeGroup.Attribute = arrSpudDate;

                arrDate.Add(objFirstChildAttributeGroup);

                ArrayList arrKickOffDate = new ArrayList();
                AttributeGroup objSecondChildAttributeGroup = new AttributeGroup();
                objSecondChildAttributeGroup.Operator = GetLogicalOperator();

                Attributes objFromKickOffDate = new Attributes();
                objFromKickOffDate.Name = KICKOFFDATE;
                objFromKickOffDate.Operator = GetDateOperator(FROM);
                objFromKickOffDate.Value = SetDateValue(FROM, txtFrom.Text, txtTo.Text);
                arrKickOffDate.Add(objFromKickOffDate);

                Attributes objToKickOffDate = new Attributes();
                objToKickOffDate.Name = KICKOFFDATE;
                objToKickOffDate.Operator = GetDateOperator(TO);
                objToKickOffDate.Value = SetDateValue(TO, txtFrom.Text, txtTo.Text);
                arrKickOffDate.Add(objToKickOffDate);

                objSecondChildAttributeGroup.Attribute = arrKickOffDate;

                arrDate.Add(objSecondChildAttributeGroup);
            }
            else
            {
                AttributeGroup objFirstChildAttributeGroup = new AttributeGroup();
                ArrayList arrCompletionDate = new ArrayList();

                Attributes objFromCompletionDate = new Attributes();
                objFromCompletionDate.Name = ABANDONEDDATE;

                objFromCompletionDate.Label = txtFrom.ID;
                objFromCompletionDate.Operator = GREATERTHANEQUALS;
                objFromCompletionDate.Value = SetDateValue(FROM, txtFrom.Text, txtTo.Text);
                arrCompletionDate.Add(objFromCompletionDate);

                Attributes objToCompletionDate = new Attributes();
                objToCompletionDate.Name = ABANDONEDDATE;

                objToCompletionDate.Label = txtTo.ID;
                objToCompletionDate.Operator = LESSTHANEQUALS;
                objToCompletionDate.Value = SetDateValue(TO, txtFrom.Text, txtTo.Text);
                arrCompletionDate.Add(objToCompletionDate);

                objFirstChildAttributeGroup.Attribute = arrCompletionDate;
                objFirstChildAttributeGroup.Operator = GetLogicalOperator();
                arrDate.Add(objFirstChildAttributeGroup);
            }
            return arrDate;
        }

        /// <summary>
        /// Sets the geographical attribute group.
        /// </summary>
        /// <returns></returns>
        private AttributeGroup SetGeographicalAttributeGroup()
        {
            AttributeGroup objParentAttributeGroup = new AttributeGroup();

            if(rdoLatLong.Checked)
            {
                objParentAttributeGroup.Operator = GetLogicalOperator();
                objParentAttributeGroup.Label = rdoLatLong.ID;
                objParentAttributeGroup.Name = GetControlID(rdoLatLong.ID);
                objParentAttributeGroup.Checked = TRUE;
                objParentAttributeGroup.AttributeGroups = SetGeographicalAttribute();
            }
            return objParentAttributeGroup;
        }

        /// <summary>
        /// Sets the geographical attribute.
        /// </summary>
        /// <returns></returns>
        private ArrayList SetGeographicalAttribute()
        {
            ArrayList arrLatLon = new ArrayList();
            if(rdoRbLatLon.Items[1].Selected)
            {
                ArrayList arrLatitude = new ArrayList();

                AttributeGroup objFirstChildAttributeGroup = new AttributeGroup();
                objFirstChildAttributeGroup.Operator = GetLogicalOperator();
                objFirstChildAttributeGroup.Checked = TRUE;
                objFirstChildAttributeGroup.Label = rdoRbLatLon.ID;
                objFirstChildAttributeGroup.Name = rdoRbLatLon.Items[1].Value.ToString();

                Attributes objMinLat = new Attributes();
                objMinLat.Name = BOTTOMLATITUDE;
                objMinLat.Operator = GREATERTHANEQUALS;
                objMinLat.Parameter = SetParameter(MINLATITUDE);
                arrLatitude.Add(objMinLat);

                Attributes objMaxLat = new Attributes();
                objMaxLat.Name = BOTTOMLATITUDE;
                objMaxLat.Operator = LESSTHANEQUALS;
                objMaxLat.Parameter = SetParameter(MAXLATITUDE);
                arrLatitude.Add(objMaxLat);

                objFirstChildAttributeGroup.Attribute = arrLatitude;

                arrLatLon.Add(objFirstChildAttributeGroup);

                ArrayList arrLongitude = new ArrayList();
                AttributeGroup objSecondChildAttributeGroup = new AttributeGroup();
                objSecondChildAttributeGroup.Operator = GetLogicalOperator();

                Attributes objMinLon = new Attributes();
                objMinLon.Name = BOTTOMLONGITUDE;
                objMinLon.Operator = GREATERTHANEQUALS;
                objMinLon.Parameter = SetParameter(MINLONGITUDE);
                arrLongitude.Add(objMinLon);

                Attributes objMaxLon = new Attributes();
                objMaxLon.Name = BOTTOMLONGITUDE;
                objMaxLon.Operator = LESSTHANEQUALS;
                objMaxLon.Parameter = SetParameter(MAXLONGITUDE);
                arrLongitude.Add(objMaxLon);

                objSecondChildAttributeGroup.Attribute = arrLongitude;

                arrLatLon.Add(objSecondChildAttributeGroup);
            }
            else
            {
                ArrayList arrLatitude = new ArrayList();
                AttributeGroup objFirstChildAttributeGroup = new AttributeGroup();
                objFirstChildAttributeGroup.Operator = GetLogicalOperator();
                objFirstChildAttributeGroup.Checked = TRUE;
                objFirstChildAttributeGroup.Label = rdoRbLatLon.ID;
                objFirstChildAttributeGroup.Name = rdoRbLatLon.Items[0].Value.ToString();

                Attributes objMinLat = new Attributes();
                objMinLat.Name = SURFACELATITUDE;
                objMinLat.Operator = GREATERTHANEQUALS;
                objMinLat.Parameter = SetParameter(MINLATITUDE);
                arrLatitude.Add(objMinLat);

                Attributes objMaxLat = new Attributes();
                objMaxLat.Name = SURFACELATITUDE;
                objMaxLat.Operator = LESSTHANEQUALS;
                objMaxLat.Parameter = SetParameter(MAXLATITUDE);
                arrLatitude.Add(objMaxLat);

                objFirstChildAttributeGroup.Attribute = arrLatitude;

                arrLatLon.Add(objFirstChildAttributeGroup);

                ArrayList arrLongitude = new ArrayList();
                AttributeGroup objSecondChildAttributeGroup = new AttributeGroup();
                objSecondChildAttributeGroup.Operator = GetLogicalOperator();

                Attributes objMinLon = new Attributes();
                objMinLon.Name = SURFACELONGITUDE;
                objMinLon.Operator = GREATERTHANEQUALS;
                objMinLon.Parameter = SetParameter(MINLONGITUDE);
                arrLongitude.Add(objMinLon);

                Attributes objMaxLon = new Attributes();
                objMaxLon.Name = SURFACELONGITUDE;
                objMaxLon.Operator = LESSTHANEQUALS;
                objMaxLon.Parameter = SetParameter(MAXLONGITUDE);
                arrLongitude.Add(objMaxLon);

                objSecondChildAttributeGroup.Attribute = arrLongitude;

                arrLatLon.Add(objSecondChildAttributeGroup);
            }

            return arrLatLon;
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
        /// Sets the basic attribute.
        /// </summary>
        /// <returns></returns>
        private ArrayList SetBasicAttribute()
        {
            ArrayList arrAttribute = new ArrayList();

            //checks if file based search criteria is selected or not. 
            //method to search a file for identifier values. Returns an arraylist of Attributes objects
            if(cboSearchCriteria.SelectedIndex != 0)
            {
                arrAttribute = SetSearchCriteriaAttribute(arrAttribute, txtUnique_Well_Identifier, ReadFileSearch(fileUploader.PostedFile, hidWordContent.Value), cboSearchCriteria);
                if(arrAttribute.Count == 0)
                {
                    throw new Exception(BLANKFILEMESSAGE);
                }
            }

            if(!string.Equals(cboSearchCriteria.SelectedItem.Text, UWI))
            {
                arrAttribute = SetBasicListAttribute(arrAttribute, txtUnique_Well_Identifier);
            }

            arrAttribute = SetBasinCountryAttribute(arrAttribute, lstBasin);

            arrAttribute = SetBasinCountryAttribute(arrAttribute, lstCountry);

            arrAttribute = SetBasicListAttribute(arrAttribute, lstState_Or_Province);

            arrAttribute = SetBasicListAttribute(arrAttribute, lstCounty);

            arrAttribute = SetBasicListAttribute(arrAttribute, lstField_Identifier, true);

            //This is currently blocked as web services are not supporting 
            arrAttribute = SetBasicListAttribute(arrAttribute, lstOnshore_Or_Offshore);

            arrAttribute = SetBasicListAttribute(arrAttribute, lstKind);

            arrAttribute = SetBasicListAttribute(arrAttribute, lstPick);

            if(!string.IsNullOrEmpty(txtCurrent_Operator.Text.Trim()))
                arrAttribute = SetBasicListAttribute(arrAttribute, txtCurrent_Operator);

            arrAttribute = SetBasicListAttribute(arrAttribute, lstCurrent_Status);

            /// Start
            /// Modified By: Yasotha
            /// Modified On: 24-Jan-2010
            /// Added for DREAM 4.0 requirment

            if(!string.IsNullOrEmpty(txtWell_Name.Text.Trim()))
                arrAttribute = SetBasicListAttribute(arrAttribute, txtWell_Name);
            if(!string.IsNullOrEmpty(txtCommon_Name.Text.Trim()))
                arrAttribute = SetBasicListAttribute(arrAttribute, txtCommon_Name);
            if(!string.IsNullOrEmpty(txtAlias_Name.Text.Trim()))
                arrAttribute = SetBasicListAttribute(arrAttribute, txtAlias_Name);
            /// End

            return arrAttribute;
        }

        /// <summary>
        /// Sets the parameter.
        /// </summary>
        /// <param name="strCriteria">The STR criteria.</param>
        /// <returns></returns>
        private ArrayList SetParameter(string criteria)
        {
            ArrayList arlParameter = new ArrayList();

            if(string.Equals(criteria, MINLATITUDE))
            {
                //Set the parameter for Degree, Minute and Second
                arlParameter = SetLatLonParameter(txtMinLatDeg, arlParameter);
                arlParameter = SetLatLonParameter(txtMinLatMin, arlParameter);
                arlParameter = SetLatLonParameter(txtMinLatSec, arlParameter);
                //Dream 4.0 
                arlParameter = SetLatLonParameter(txtMinLatNS, arlParameter);
            }
            else if(string.Equals(criteria, MAXLATITUDE))
            {
                //Set the parameter for Degree, Minute and Second
                arlParameter = SetLatLonParameter(txtMaxLatDeg, arlParameter);
                arlParameter = SetLatLonParameter(txtMaxLatMin, arlParameter);
                arlParameter = SetLatLonParameter(txtMaxLatSec, arlParameter);
                //Dream 4.0 
                arlParameter = SetLatLonParameter(txtMaxLatNS, arlParameter);
            }
            else if(string.Equals(criteria, MINLONGITUDE))
            {
                //Set the parameter for Degree,Minute,Second and E/W.
                arlParameter = SetLatLonParameter(txtMinLonDeg, arlParameter);
                arlParameter = SetLatLonParameter(txtMinLonMin, arlParameter);
                arlParameter = SetLatLonParameter(txtMinLonSec, arlParameter);
                //Dream 4.0 
                arlParameter = SetLatLonParameter(txtMinLonEW, arlParameter);
            }
            else if(string.Equals(criteria, MAXLONGITUDE))
            {
                //Set the parameter for Degree,Minute,Second and E/W.
                arlParameter = SetLatLonParameter(txtMaxLonDeg, arlParameter);
                arlParameter = SetLatLonParameter(txtMaxLonMin, arlParameter);
                arlParameter = SetLatLonParameter(txtMaxLonSec, arlParameter);
                //Dream 4.0 
                arlParameter = SetLatLonParameter(txtMaxLonEW, arlParameter);
            }
            return arlParameter;
        }

        /// <summary>
        /// Displays the search results.
        /// </summary>
        protected void DisplaySearchResults()
        {
            RequestInfo objReqInfo = null;
            objReqInfo = new RequestInfo();
            objReqInfo = SetBasicDataObjects();
            string strAssetType = "Well";
            UISaveSearchHandler objUISaveSearchHandler = new UISaveSearchHandler();
            objUISaveSearchHandler.DisplayResults(Page, objReqInfo, WELLADVANCEDSEARCH);
            RedirectPage(SEARCHRESULTSPAGE + "?SearchType=" + WELLADVANCEDSEARCH + "&asset=" + strAssetType, strAssetType);
        }

        /*********************************************************************************
               UI Control Binding code starts here...
       * ******************************************************************************/

        /// <summary>
        /// Binds the UI controls.
        /// </summary>
        /// <param name="saveSearchDoc">The savesearch Xmldocument.</param>
        /// <param name="saveSearchName">Name of the save search selected by user.</param>
        private void BindUIControls(XmlDocument saveSearchDoc, string saveSearchName)
        {
            if(saveSearchDoc != null)
            {
                //reload the shared checkbox value based on selected saved search.
                XmlNodeList xmlnodelistSharedSaveSearch = saveSearchDoc.SelectNodes(SAVESHAREDSEARCHXSLPATH + "[@" + ATTRIBNAME + "=\"" + saveSearchName + "\"]");
                XmlNode xmlnodeSharedSaveSearch = xmlnodelistSharedSaveSearch.Item(0);
                if(xmlnodeSharedSaveSearch.Attributes.GetNamedItem(ATTRIBSHARED).Value.Equals("True"))
                {
                    chbShared.Checked = true;
                }
                else
                {
                    chbShared.Checked = false;
                }

                XmlNodeList xmlnodelistSaveSearch = saveSearchDoc.SelectNodes(SAVESEARCHXSLPATH);
                foreach(XmlNode xmlnodeSaveSearch in xmlnodelistSaveSearch)
                {
                    if(xmlnodeSaveSearch.ParentNode.ParentNode.Attributes.GetNamedItem(ATTRIBNAME).Value.Equals(saveSearchName))
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        //Get the XmlDocument for the Selected Search Name.
                        xmlDoc.LoadXml(xmlnodeSaveSearch.InnerXml);
                        BindAttributeGroup(xmlDoc, SINGLEATTRIBUTEGROUPXPATH);
                    }
                }
            }
        }

        /// <summary>
        /// Binds the attribute group.
        /// </summary>
        /// <param name="saveSearchDoc">The save search doc.</param>
        /// <param name="SaveSearchXPATH">The save search XPATH.</param>
        private void BindAttributeGroup(XmlDocument saveSearchDoc, string saveSearchXPATH)
        {
            XmlNodeList xmlnodelistAttrGrp = saveSearchDoc.SelectNodes(saveSearchXPATH);
            foreach(XmlNode xmlnodeAttrGrp in xmlnodelistAttrGrp)
            {
                string strLabel;
                try
                {
                    strLabel = xmlnodeAttrGrp.Attributes.GetNamedItem(LABEL).Value.ToString();
                }
                catch(Exception)
                {
                    strLabel = string.Empty;
                }

                if(strLabel.Length > 0)
                {
                    foreach(Control objControl in this.Controls)
                    {
                        foreach(Control objChildControl in objControl.Controls)
                        {
                            RadioButtonList objRadioButtonList = new RadioButtonList();
                            RadioButton objRadioButton = new RadioButton();
                            TextBox objTextBox = new TextBox();
                            ListBox objListBox = new ListBox();
                            //Checks for Control of RadioButton Type.
                            if(string.Equals(objChildControl.GetType().ToString(), objRadioButton.GetType().ToString()))
                            {
                                //Checks whether Control ID is same as label in XML.
                                if(string.Equals(objChildControl.ID, strLabel))
                                {
                                    if((string.Equals(objChildControl.ID, "rdoLatLong")) || (string.Equals(objChildControl.ID, "rdoCRS")))
                                    {
                                        chbGeographicalSearch.Checked = true;
                                    }
                                    ((RadioButton)(objChildControl)).Checked = true;
                                }
                            }
                            //Checks for Control of TextBox Type.
                            if(string.Equals(objChildControl.GetType().ToString(), objTextBox.GetType().ToString()))
                            {
                                if(string.Equals(objChildControl.ID, strLabel))
                                {
                                    if(string.Equals(xmlnodeAttrGrp.FirstChild.Name, VALUE))
                                    {
                                        ((TextBox)(objChildControl)).Text = xmlnodeAttrGrp.FirstChild.InnerText.ToString();
                                    }
                                    else
                                    {
                                        ((TextBox)(objChildControl)).Text = xmlnodeAttrGrp.Attributes.GetNamedItem(VALUE).Value.ToString();
                                    }
                                }
                            }
                            //Checks for Control of ListBox Type.
                            if(string.Equals(objChildControl.GetType().ToString(), objListBox.GetType().ToString()))
                            {
                                if(string.Equals(objChildControl.ID, strLabel))
                                {
                                    /// Set the respective check box checked and populate related listbox
                                    BindCheckBoxControl(objChildControl.ID);

                                    ((ListBox)(objChildControl)).ClearSelection();
                                    XmlNodeList xmlnodelistValue = saveSearchDoc.SelectNodes(saveSearchXPATH + "/value");
                                    //ListBox selection based on the save search XML.
                                    foreach(ListItem lstItems in ((ListBox)(objChildControl)).Items)
                                    {
                                        foreach(XmlNode xmlnodeListBoxItem in xmlnodelistValue)
                                        {
                                            if(string.Equals(xmlnodeListBoxItem.ParentNode.Attributes.GetNamedItem(LABEL).Value.ToString(), strLabel))
                                            {
                                                if(string.Equals(lstItems.Value, xmlnodeListBoxItem.InnerText))
                                                {
                                                    lstItems.Selected = true;
                                                }
                                            }
                                        }
                                    }
                                    ////if any country is selected, respective state / field needs to be loaded.
                                    //if(string.Equals(((ListBox)(objChildControl)).ID, "lstCountry"))
                                    //{
                                    //    lstCountry_SelectedIndexChanged(null, EventArgs.Empty);
                                    //}

                                    //if any state is selected, respective county needs to be loaded.
                                    if(string.Equals(((ListBox)(objChildControl)).ID, "lstState_Or_Province"))
                                    {
                                        lstState_Or_Province_SelectedIndexChanged(null, EventArgs.Empty);
                                    }

                                }
                            }
                            //DREAM 4.0 coutry listbox change
                            //Checks for Control of dropdownlistbox Type.
                            if(string.Equals(objChildControl.GetType().ToString(), typeof(DropDownList).ToString()))
                            {
                                if(string.Equals(objChildControl.ID, strLabel))
                                {
                                    /// Set the respective check box checked and populate related listbox
                                    BindCheckBoxControl(objChildControl.ID);

                                    ((DropDownList)(objChildControl)).ClearSelection();
                                    XmlNodeList xmlnodelistValue = saveSearchDoc.SelectNodes(saveSearchXPATH + "/value");
                                    //ListBox selection based on the save search XML.
                                    foreach(ListItem lstItems in ((DropDownList)(objChildControl)).Items)
                                    {
                                        foreach(XmlNode xmlnodeListBoxItem in xmlnodelistValue)
                                        {
                                            if(string.Equals(xmlnodeListBoxItem.ParentNode.Attributes.GetNamedItem(LABEL).Value.ToString(), strLabel))
                                            {
                                                if(string.Equals(lstItems.Value, xmlnodeListBoxItem.InnerText))
                                                {
                                                    lstItems.Selected = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    //if any country is selected, respective state / field needs to be loaded.
                                    if(string.Equals(((DropDownList)(objChildControl)).ID, "lstCountry"))
                                    {
                                        lstCountry_SelectedIndexChanged(null, EventArgs.Empty);
                                    }
                                }
                            }
                            //Checks for Control of RadioButtonList Type.
                            if(string.Equals(objChildControl.GetType().ToString(), objRadioButtonList.GetType().ToString()))
                            {
                                if(string.Equals(objChildControl.ID, strLabel))
                                {
                                    //Select the LatLon RadioButton List.
                                    if(string.Equals(objChildControl.ID, rdoRbLatLon.ID))
                                    {
                                        int intIndex = CheckRadioButtonGroup(((RadioButtonList)(objChildControl)).ID.ToString(), saveSearchDoc, saveSearchXPATH);
                                        ((RadioButtonList)(objChildControl)).Items[intIndex].Selected = true;
                                    }
                                    else
                                    {
                                        int intIndex = CheckRadioButtonGroup(((RadioButtonList)(objChildControl)).ID.ToString(), saveSearchDoc, saveSearchXPATH);
                                        ((RadioButtonList)(objChildControl)).Items[intIndex].Selected = true;
                                    }
                                }
                            }
                        }
                    }
                }
                //Checks whether the Xml Attribute node has more Child nodes.
                if(xmlnodeAttrGrp.HasChildNodes)
                {
                    //Sets the XPATH by appending with Attributegroup.
                    if(string.Equals(xmlnodeAttrGrp.FirstChild.Name.ToString(), ATTRIBUTEGROUP))
                    {
                        saveSearchXPATH = saveSearchXPATH.ToString() + SINGLEATTRIBUTEGROUPXPATH;
                        BindAttributeGroup(saveSearchDoc, saveSearchXPATH);
                    }
                    //Sets the XPATH by appending with Attribute.
                    else if(string.Equals(xmlnodeAttrGrp.FirstChild.Name.ToString(), ATTRIBUTE))
                    {
                        if(string.Equals(xmlnodeAttrGrp.FirstChild.FirstChild.Name.ToString(), VALUE))
                        {
                            saveSearchXPATH = saveSearchXPATH.ToString() + ATTRIBUTEXPATH;
                            BindInnerAttributeGroup(saveSearchDoc, saveSearchXPATH);
                            saveSearchXPATH = ATTRIBUTEGROUPXPATH;
                        }
                        else if(string.Equals(xmlnodeAttrGrp.FirstChild.FirstChild.Name.ToString(), PARAMETER))
                        {
                            saveSearchXPATH = saveSearchXPATH.ToString() + ATTRIBUTEXPATH;
                            BindInnerAttributeGroup(saveSearchDoc, saveSearchXPATH);
                            saveSearchXPATH = ATTRIBUTEGROUPXPATH;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Binds the check box control and fires Change event 
        /// to populate the cascading list boxes.
        /// </summary>
        /// <param name="childControlID">The child control ID.</param>
        private void BindCheckBoxControl(string childControlID)
        {

            switch(childControlID)
            {
                //if any state is selected, respective county needs to be loaded.
                case "lstPick":
                    {
                        chbFormation.Checked = true;
                        chbFormation_Change(null, EventArgs.Empty);
                        break;
                    }
                //Whether the ListBox is same as state then check the required CheckBoxes.
                case "lstState_Or_Province":
                    {
                        chbState.Checked = true;
                        chbState_Change(null, EventArgs.Empty);
                        break;
                    }
                //Whether the ListBox is same as County then check the required CheckBoxes.
                case "lstCounty":
                    {
                        chbCounty.Checked = true;
                        chbCounty_Change(null, EventArgs.Empty);
                        break;
                    }
                //Whether the ListBox is same as Field then check the required CheckBoxes.
                case "lstField_Identifier":
                    {
                        chbField_Identifier.Checked = true;
                        chbField_Change(null, EventArgs.Empty);
                        break;
                    }

                default:
                    break;
            }

        }

        /// <summary>
        /// Binds the inner attribute group.
        /// </summary>
        /// <param name="saveSearchDoc">The save search doc.</param>
        /// <param name="SaveSearchXPATH">The save search XPATH.</param>
        private void BindInnerAttributeGroup(XmlDocument saveSearchDoc, string saveSearchXPATH)
        {
            RadioButtonList objRadioButtonList = new RadioButtonList();
            RadioButton objRadioButton = new RadioButton();
            TextBox objTextBox = new TextBox();
            ListBox objListBox = new ListBox();
            XmlNodeList xmlnodelistAttrGrp = saveSearchDoc.SelectNodes(saveSearchXPATH);
            foreach(XmlNode xmlnodeAttrGrp in xmlnodelistAttrGrp)
            {
                string strLabel;
                try
                {
                    strLabel = xmlnodeAttrGrp.Attributes.GetNamedItem(LABEL).Value.ToString();
                }
                catch(Exception)
                {
                    strLabel = string.Empty;
                }

                if(strLabel.Length > 0)
                {
                    //Loop through the page controls.
                    foreach(Control objControl in this.Controls)
                    {
                        //Loop through the page's panel control.
                        foreach(Control objChildControl in objControl.Controls)
                        {
                            //Checks for Control of RadioButton Type.
                            if(string.Equals(objChildControl.GetType().ToString(), objRadioButton.GetType().ToString()))
                            {
                                if(string.Equals(objChildControl.ID, strLabel))
                                {
                                    if((string.Equals(objChildControl.ID, "rdoLatLong")) || (string.Equals(objChildControl.ID, "rdoCRS")))
                                    {
                                        chbGeographicalSearch.Checked = true;
                                    }
                                    ((RadioButton)(objChildControl)).Checked = true;
                                    break;
                                }
                            }
                            //Checks for Control of TextBox Type.
                            if(string.Equals(objChildControl.GetType().ToString(), objTextBox.GetType().ToString()))
                            {
                                if(string.Equals(objChildControl.ID, strLabel))
                                {
                                    if(xmlnodeAttrGrp.HasChildNodes)
                                    {
                                        if(string.Equals(xmlnodeAttrGrp.FirstChild.Name, VALUE))
                                        {
                                            ((TextBox)(objChildControl)).Text = xmlnodeAttrGrp.FirstChild.InnerText.ToString();
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        ((TextBox)(objChildControl)).Text = xmlnodeAttrGrp.Attributes.GetNamedItem(VALUE).Value.ToString();
                                        break;
                                    }
                                }
                            }
                            //Checks for Control of ListBox Type.
                            if(string.Equals(objChildControl.GetType().ToString(), objListBox.GetType().ToString()))
                            {
                                if(string.Equals(objChildControl.ID, strLabel))
                                {
                                    /// Set the respective check box checked and populate related listbox
                                    BindCheckBoxControl(objChildControl.ID);

                                    ((ListBox)(objChildControl)).ClearSelection();
                                    XmlNodeList xmlnodelistValue = saveSearchDoc.SelectNodes(saveSearchXPATH + "/value");
                                    //ListBox selection based on the save search XML.
                                    foreach(ListItem lstItems in ((ListBox)(objChildControl)).Items)
                                    {
                                        foreach(XmlNode xmlnodeListBoxItem in xmlnodelistValue)
                                        {
                                            if(string.Equals(xmlnodeListBoxItem.ParentNode.Attributes.GetNamedItem(LABEL).Value.ToString(), strLabel))
                                            {
                                                if(string.Equals(lstItems.Value, xmlnodeListBoxItem.InnerText))
                                                {
                                                    lstItems.Selected = true;
                                                }
                                            }
                                        }
                                    }
                                    ////if any country is selected, respective state / field needs to be loaded.
                                    //if(string.Equals(((ListBox)(objChildControl)).ID, "lstCountry"))
                                    //{
                                    //    lstCountry_SelectedIndexChanged(null, EventArgs.Empty);
                                    //    break;
                                    //}
                                    //if any state is selected, respective county needs to be loaded.
                                    if(string.Equals(((ListBox)(objChildControl)).ID, "lstState_Or_Province"))
                                    {
                                        lstState_Or_Province_SelectedIndexChanged(null, EventArgs.Empty);
                                        break;
                                    }

                                }
                            }
                            //DREAM 4.0 coutry listbox change
                            //Checks for Control of dropdownlistbox Type.
                            if(string.Equals(objChildControl.GetType().ToString(), typeof(DropDownList).ToString()))
                            {
                                if(string.Equals(objChildControl.ID, strLabel))
                                {
                                    /// Set the respective check box checked and populate related listbox
                                    BindCheckBoxControl(objChildControl.ID);

                                    ((DropDownList)(objChildControl)).ClearSelection();
                                    XmlNodeList xmlnodelistValue = saveSearchDoc.SelectNodes(saveSearchXPATH + "/value");
                                    //ListBox selection based on the save search XML.
                                    foreach(ListItem lstItems in ((DropDownList)(objChildControl)).Items)
                                    {
                                        foreach(XmlNode xmlnodeListBoxItem in xmlnodelistValue)
                                        {
                                            if(string.Equals(xmlnodeListBoxItem.ParentNode.Attributes.GetNamedItem(LABEL).Value.ToString(), strLabel))
                                            {
                                                if(string.Equals(lstItems.Value, xmlnodeListBoxItem.InnerText))
                                                {
                                                    lstItems.Selected = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    //if any country is selected, respective state / field needs to be loaded.
                                    if(string.Equals(((DropDownList)(objChildControl)).ID, "lstCountry"))
                                    {
                                        lstCountry_SelectedIndexChanged(null, EventArgs.Empty);
                                    }
                                }
                            }
                            //Checks for Control of RadioButtonList Type.
                            if(string.Equals(objChildControl.GetType().ToString(), objRadioButtonList.GetType().ToString()))
                            {
                                if(string.Equals(objChildControl.ID, strLabel))
                                {
                                    int intIndex = CheckRadioButtonGroup(((RadioButtonList)(objChildControl)).ID.ToString(), saveSearchDoc, saveSearchXPATH);
                                    ((RadioButtonList)(objChildControl)).Items[intIndex].Selected = true;
                                    break;
                                }
                            }
                        }
                    }

                }
                if(xmlnodeAttrGrp.HasChildNodes)
                {
                    if(string.Equals(xmlnodeAttrGrp.FirstChild.Name.ToString(), "parameter"))
                    {
                        saveSearchXPATH = saveSearchXPATH.ToString() + "/parameter";
                        BindInnerAttributeGroup(saveSearchDoc, saveSearchXPATH);
                    }
                }
            }
        }

        #endregion Private Methods


    }
}

#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: PARSAdvSearch.ascx.cs 
#endregion

/// <summary> 
/// This is Project Archive Search results class
/// </summary> 
using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;
using System.Net;
using System.Web.Services.Protocols;

namespace Shell.SharePoint.DREAM.Site.UI.UserControls
{
    /// <summary>
    /// The PARS UI Class
    /// </summary>
    public partial class PARSAdvSearch : UIControlHandler
    {
        #region Declaration
        Entity objEntity = new Entity();
        string strCurrSiteURL = string.Empty;
        const string SORT_DEFAULT_ORDER = "descending";
        const string PARSPAGEURL = "AdvSearchPARS.aspx";
        const string CHECKREFRESH = "PARSCheckRefresh";
        HiddenField hidSearchName = new HiddenField();
        #endregion
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            strCurrSiteURL = HttpContext.Current.Request.Url.ToString();
            objMossController = objFactory.GetServiceManager("MossService");
            cboSearchCriteria.Attributes.Add("onchange", "javascript:FileSearchTypeSelectedIndexChange(this,'" + txtXMPROJECTNAME.ID + "|input','" + txtXMTITLE.ID + "|input','" + txtXMDESCRIPTION.ID + "|input" + "')");
            cmdSearch.Attributes.Add(REDIRECTATTRIBUTE, REDIRECTATTRIBUTEVALUE);
            cmdReset.Attributes.Add(REDIRECTATTRIBUTE, REDIRECTATTRIBUTEVALUE);
            btnSearch.Attributes.Add(REDIRECTATTRIBUTE, REDIRECTATTRIBUTEVALUE);
            btnReset.Attributes.Add(REDIRECTATTRIBUTE, REDIRECTATTRIBUTEVALUE);
        
            if (!Page.IsPostBack)
            {
                //Sets the PARS Check refresh session variable.
                CommonUtility.SetSessionVariable(Page, CHECKREFRESH, DateTime.Now.ToString());
                try
                {
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
                    base.SearchType = PARSSEARCHTYPE;
                    lblException.Text = string.Empty;
                    LoadControls(chbShared, cboSavedSearch);
                    GetAssetColumns(REPORTSERVICECOLUMNLIST, cboSearchCriteria, PARSITEMVAL);
                    if (Request.QueryString["savesearchname"] != null)
                    {
                        if (Request.QueryString["operation"] != null)
                        {
                            if (string.Equals(Request.QueryString["operation"].ToString(), "modify"))
                            {
                                cmdSaveSearch.Value = "Modify Search";
                                txtSaveSearch.Text = Request.QueryString["savesearchname"].ToString();
                                txtSaveSearch.Enabled = false;
                            }
                        }
                    }
                    BindTooltipTextToControls();
                    if (CheckChildControlID())
                    { 
                      chbGeographicalSearch.Checked = true; 
                    }
                }
                catch (WebException webEx)
                {
                    ShowLableMessage(webEx.Message);
                }
                catch (Exception ex)
                {
                    CommonUtility.HandleException(strCurrSiteUrl, ex);
                }
            }
            //DREAM 4.0 added for adv search country dropdown
            objUtility.RegisterOnLoadClientScript(this.Page, "if(document.getElementById(GetObjectID('chbGeographicalSearch', 'input')).checked == true){showPARSLatLongTable('TR1','TR2','TR3','TR4','TR5','TR6');}  ");
        }
        /// <summary>
        /// Method is use to Bind tooltips to controls
        /// </summary>
        private void BindTooltipTextToControls()
        {
            DataTable dtFilterTooltip = null;
            string strControlName = string.Empty;
            string strTooltipText = string.Empty;
            string strAsset = "Project Archives";
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
                        if (string.Equals(imgTitle.ID.Remove(0, 3).ToString(), strControlName))
                        {
                            imgTitle.ToolTip = strTooltipText;
                        }
                        if (string.Equals(imgDescription.ID.Remove(0, 3).ToString(), strControlName))
                        {
                            imgDescription.ToolTip = strTooltipText;
                        }
                        if (string.Equals(imgProjectName.ID.Remove(0, 3).ToString(), strControlName))
                        {
                            imgProjectName.ToolTip = strTooltipText;
                        }
                        if (string.Equals(imgMilestone.ID.Remove(0, 3).ToString(), strControlName))
                        {
                            imgMilestone.ToolTip = strTooltipText;
                        }
                        if (string.Equals(imgBasin.ID.Remove(0, 3).ToString(), strControlName))
                        {
                            imgBasin.ToolTip = strTooltipText;
                        }
                        if (string.Equals(imgField.ID.Remove(0, 3).ToString(), strControlName))
                        {
                            imgField.ToolTip = strTooltipText;
                        }
                        if (string.Equals(imgAsset.ID.Remove(0, 3).ToString(), strControlName))
                        {
                            imgAsset.ToolTip = strTooltipText;
                        }
                        if (string.Equals(imgCountry.ID.Remove(0, 3).ToString(), strControlName))
                        {
                            imgCountry.ToolTip = strTooltipText;
                        }
                        if (string.Equals(imgDataOwner.ID.Remove(0, 3).ToString(), strControlName))
                        {
                            imgDataOwner.ToolTip = strTooltipText;
                        }
                        if (string.Equals(imgOwnerOrganisation.ID.Remove(0, 3).ToString(), strControlName))
                        {
                            imgOwnerOrganisation.ToolTip = strTooltipText;
                        }
                        else if (imgSearchName.ID.Remove(0, 3).ToString().Equals(strControlName))
                        {
                            imgSearchName.ToolTip = strTooltipText;
                        }
                    }
                }
            }
            catch
            { throw; }
            finally { if (dtFilterTooltip != null) dtFilterTooltip.Dispose(); }
        }
        /// <summary>
        /// Handles the Click event of the cmdReset control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmdReset_Click(object sender, EventArgs e)
        {
            string strUrl = string.Empty;
            if (string.Equals(cmdSaveSearch.Value.ToString(), "Modify Search"))
            {
                if (Request.QueryString["savesearchname"] != null)
                {
                    strUrl = PARSPAGEURL + "?asset=Project Archives&manage=true&savesearchname=" + Request.QueryString["savesearchname"].ToString() + "&operation=modify";
                }
                else
                {
                    strUrl = PARSPAGEURL + "?asset=Project Archives";
                }
            }
            else
            {
                strUrl = PARSPAGEURL + "?asset=Project Archives";
            }
            RedirectPage(strUrl, string.Empty);
        }

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
                if (string.Equals(cmdSaveSearch.Value.ToString(), "Modify Search"))
                {
                    lblException.Visible = false;
                    objMossController = objFactory.GetServiceManager("MossService");
                    object objCheckRefresh = CommonUtility.GetSessionVariable(Page, CHECKREFRESH);
                    //Checks whether the event is fired or page has been refreshed.
                    if (string.Equals(ViewState["CheckRefresh"].ToString(), (string)objCheckRefresh))
                    {
                        CommonUtility.SetSessionVariable(Page, CHECKREFRESH, DateTime.Now.ToString());
                        string strSaveSearchName = txtSaveSearch.Text.Trim();
                        XmlDocument xmlDocSearchRequest = null;
                        xmlDocSearchRequest = CreateSaveSearchXML();
                        UISaveSearchHandler objUISaveSearchHandler = null;
                        try
                        {
                            objUISaveSearchHandler = new UISaveSearchHandler();
                            objUISaveSearchHandler.ModifySaveSearchXML(PARSSEARCHTYPE, strSaveSearchName, chbShared.Checked, cboSavedSearch, xmlDocSearchRequest);
                            if (cboSavedSearch.Items.FindByText("---Select---") == null)
                            {
                                cboSavedSearch.Items.Insert(0, "---Select---");
                            }
                        }
                        catch (WebException webEx)
                        {
                            ShowLableMessage(webEx.Message);
                        }
                        catch (Exception ex)
                        {
                            CommonUtility.HandleException(strCurrSiteUrl, ex);
                        }
                    }
                    else
                    {
                        cboSavedSearch.Items.Clear();
                        cboSavedSearch.Items.Add("---Select---");
                        ((MOSSServiceManager)objMossController).LoadSaveSearch(PARSSEARCHTYPE, cboSavedSearch);
                    }
                    cboSavedSearch.Text = txtSaveSearch.Text.ToString().Trim();
                    txtSaveSearch.Text = string.Empty;
                    txtSaveSearch.Enabled = true;
                    cmdSaveSearch.Value = "Save Search";
                }
                else
                {
                    lblException.Visible = false;
                    ArrayList arlSavedSearch = new ArrayList();
                    objMossController = objFactory.GetServiceManager("MOSSService");
                    object objCheckRefresh = CommonUtility.GetSessionVariable(Page, CHECKREFRESH);
                    //Checks whether the event is fired or page has been refreshed.
                    if (string.Equals(ViewState["CheckRefresh"].ToString(), (string)objCheckRefresh))
                    {
                        CommonUtility.SetSessionVariable(Page, CHECKREFRESH, DateTime.Now.ToString());
                        string strSaveSearchName = txtSaveSearch.Text.Trim();
                        //Check for Duplicate Name Exist
                        arlSavedSearch = ((MOSSServiceManager)objMossController).GetSaveSearchName(PARSSEARCHTYPE, GetUserName());
                        if (IsDuplicateNameExist(arlSavedSearch, strSaveSearchName))
                        {
                            ShowLableMessage(((MOSSServiceManager)objMossController).GetCustomExceptionMessage(strCurrSiteURL, "12"));
                            blnIsNameExist = true;
                        }
                        else
                        {
                            XmlDocument xmlDocSearchRequest = null;
                            xmlDocSearchRequest = CreateSaveSearchXML();

                            UISaveSearchHandler objUISaveSearchHandler = null;
                            try
                            {
                                objUISaveSearchHandler = new UISaveSearchHandler();
                                objUISaveSearchHandler.SaveSearchXMLToLibrary(PARSSEARCHTYPE, strSaveSearchName, chbShared.Checked, cboSavedSearch, xmlDocSearchRequest);
                            }
                            catch (WebException webEx)
                            {
                                ShowLableMessage(webEx.Message);
                            }
                            catch (Exception ex)
                            {
                                CommonUtility.HandleException(strCurrSiteUrl, ex);
                            }
                        }
                    }
                    else
                    {
                        cboSavedSearch.Items.Clear();
                        cboSavedSearch.Items.Add("---Select---");
                        ((MOSSServiceManager)objMossController).LoadSaveSearch(PARSSEARCHTYPE, cboSavedSearch);
                    }
                    if (!blnIsNameExist)
                        cboSavedSearch.Text = txtSaveSearch.Text.ToString().Trim();
                    txtSaveSearch.Text = string.Empty;
                }
            }
            catch (WebException webEx)
            {
                ShowLableMessage(webEx.Message);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }

        /// <summary>
        /// Handles the PreRender event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            object objCheckRefresh = CommonUtility.GetSessionVariable(Page, CHECKREFRESH);
            //if the session has the CheckRefresh value then assign to ViewState.
            if (objCheckRefresh != null)
                ViewState["CheckRefresh"] = (string)objCheckRefresh;
        }

        /// <summary>
        /// Creates the save search XML.
        /// </summary>
        private XmlDocument CreateSaveSearchXML()
        {
            XmlDocument xmlDocSearchRequest = null;
            objRequestInfo = new RequestInfo();
            objReportController = objFactory.GetServiceManager("ReportService");

            objRequestInfo = SetBasicDataObjects();
            xmlDocSearchRequest = objReportController.CreateSearchRequest(objRequestInfo);
            return xmlDocSearchRequest;
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
                objMossController = objFactory.GetServiceManager("MossService");
                string strUserID = GetUserName();
                XmlDocument xmldoc = new XmlDocument();
                xmldoc = ((MOSSServiceManager)objMossController).GetDocLibXMLFile(PARSSEARCHTYPE, strUserID);
                ClearUIControls();
                txtEndDate.Text = string.Empty;
                txtStartDate.Text = string.Empty;
                chbGeographicalSearch.Checked = false;


                if ((cboSavedSearch.SelectedIndex != 0) || (string.Equals(cmdSaveSearch.Value.ToString(), "Modify Search")))
                {
                    BindUIControls(xmldoc, cboSavedSearch.SelectedItem.Text.ToString(), chbShared);
                    if (CheckChildControlID())
                    {
                        chbGeographicalSearch.Checked = true;
                    }
                    if (rbSelectDates.Checked)
                    {
                        trDates.Style.Add("display", "block");
                    }

                    if (string.Equals(cmdSaveSearch.Value.ToString(), "Modify Search"))
                    {
                        txtSaveSearch.Text = cboSavedSearch.Text;
                    }
                }
            }
            catch (WebException webEx)
            {
                ShowLableMessage(webEx.Message);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }

        /// <summary>
        /// Enables the radio button.
        /// </summary>
        protected void EnableDateField()
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "EnableTable", "<Script language='javascript'>EnableDisableDates(rbSelectDates);</Script>");
        }
        /// <summary>
        /// Displays the search results from response xml to ui.
        /// </summary>
        private void DisplaySearchResults()
        {
            try
            {
                objRequestInfo = new RequestInfo();
                objRequestInfo = SetBasicDataObjects();
                UISaveSearchHandler objUISaveSearch = new UISaveSearchHandler();
                objUISaveSearch.DisplayResults(Page, objRequestInfo, SEARCHTYPE);
                string strUrl = SEARCHRESULTSPAGE + "?SearchType=" + SEARCHTYPE + "&asset=Project Archives";
                RedirectPage(strUrl, "Project Archives");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Sets the basic data objects to create XML document
        /// </summary>
        /// <param name="strRequestInfo">The requestinfo search type.</param>
        /// <returns></returns>
        private RequestInfo SetBasicDataObjects()
        {
            objRequestInfo = new RequestInfo();
            try
            {
                objRequestInfo.Entity = SetEntity();
            }
            catch
            { throw; }
            return objRequestInfo;
        }

        /// <summary>
        /// Sets the entity.
        /// </summary>
        /// <returns></returns>
        private Entity SetEntity()
        {
            try
            {
                objEntity = new Entity();
                ArrayList arlParentAttributeGroup = new ArrayList();
                ArrayList arlBasicAttributeGroup = new ArrayList();
                if (HasBasicAttributeSelected())
                {
                    arlBasicAttributeGroup = SetBasicAttributeGroup();
                }

                if (rbLastWeek.Checked == true || rbLastMonth.Checked == true || rbLastYear.Checked == true || rbSelectDates.Checked == true)
                {
                    AttributeGroup objDateGroup = new AttributeGroup();
                    objDateGroup = SetDateAttributeGroup();
                    if (objDateGroup.Attribute != null)
                    {
                        arlBasicAttributeGroup.Add(objDateGroup);
                    }
                }
                if (txtMinLatDeg.Text.Length > 0)
                {
                    AttributeGroup objGeologicalGroup = new AttributeGroup();
                    objGeologicalGroup = SetGeographicalAttributeGroup();
                    if (objGeologicalGroup.AttributeGroups != null)
                    {
                        arlBasicAttributeGroup.Add(objGeologicalGroup);
                    }
                }
                if (arlBasicAttributeGroup.Count > 1)
                {
                    AttributeGroup objAttributeGroup = new AttributeGroup();
                    objAttributeGroup.Operator = GetLogicalOperator();
                    objAttributeGroup.AttributeGroups = arlBasicAttributeGroup;
                    arlParentAttributeGroup.Add(objAttributeGroup);
                    objEntity.AttributeGroups = arlParentAttributeGroup;
                }
                else
                {
                    objEntity.AttributeGroups = arlBasicAttributeGroup;
                }
            }
            catch
            { throw; }
            return objEntity;
        }

        /// <summary>
        /// Sets the basic attribute group.
        /// </summary>
        /// <returns></returns>
        private ArrayList SetBasicAttributeGroup()
        {
            ArrayList arrBasicAttributeGroup = new ArrayList();
            try
            {
                AttributeGroup objBasicAttributeGroup = new AttributeGroup();
                objBasicAttributeGroup.Operator = GetLogicalOperator();
                objBasicAttributeGroup.Attribute = SetBasicAttribute();
                arrBasicAttributeGroup.Add(objBasicAttributeGroup);
            }
            catch
            { throw; }
            return arrBasicAttributeGroup;
        }

        /// <summary>
        /// Sets the attribute node for request xml.
        /// </summary>
        /// <returns></returns>
        private ArrayList SetBasicAttribute()
        {
            ArrayList arlAttribute = new ArrayList();
            try
            {
                //checks if file based search criteria is selected or not. 
                if (cboSearchCriteria.SelectedIndex != 0)
                {
                    //overloaded method to search a file for identifier values. Returns an arraylist of Attributes objects
                    arlAttribute = SetUITextControls(txtXMTITLE, arlAttribute, ReadFileSearch(fileUploader.PostedFile, hidWordContent.Value), cboSearchCriteria);

                    if (arlAttribute.Count == 0)
                    {
                        throw new Exception(BLANKFILEMESSAGE);
                    }
                    //check to ensure that the file based search criteria precede in importance when compared to the 
                    //normal search parameters of the advance search UI screen 
                    if (!string.Equals(cboSearchCriteria.SelectedItem.Text, PARSTITLE))
                    {
                        if (!string.IsNullOrEmpty(txtXMTITLE.Text.Trim()))
                            arlAttribute = SetUITextControls(txtXMTITLE, arlAttribute);
                    }
                    if (!string.Equals(cboSearchCriteria.SelectedItem.Text, PARSDESCRIPTION))
                    {
                        if (!string.IsNullOrEmpty(txtXMDESCRIPTION.Text.Trim()))
                            arlAttribute = SetUITextControls(txtXMDESCRIPTION, arlAttribute);
                    }
                    if (!string.Equals(cboSearchCriteria.SelectedItem.Text, PARSPROJECTNAME))
                    {
                        if (!string.IsNullOrEmpty(txtXMPROJECTNAME.Text.Trim()))
                            arlAttribute = SetUITextControls(txtXMPROJECTNAME, arlAttribute);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(txtXMTITLE.Text.Trim()))
                        arlAttribute = SetUITextControls(txtXMTITLE, arlAttribute);
                    if (!string.IsNullOrEmpty(txtXMDESCRIPTION.Text.Trim()))
                        arlAttribute = SetUITextControls(txtXMDESCRIPTION, arlAttribute);
                    if (!string.IsNullOrEmpty(txtXMPROJECTNAME.Text.Trim()))
                        arlAttribute = SetUITextControls(txtXMPROJECTNAME, arlAttribute);
                }

                if (!string.IsNullOrEmpty(txtXMMILESTONE.Text.Trim()))
                    arlAttribute = SetUITextControls(txtXMMILESTONE, arlAttribute);
                if (!string.IsNullOrEmpty(txtXMBASIN.Text.Trim()))
                    arlAttribute = SetUITextControls(txtXMBASIN, arlAttribute);
                if (!string.IsNullOrEmpty(txtXMFIELD.Text.Trim()))
                    arlAttribute = SetUITextControls(txtXMFIELD, arlAttribute);
                if (!string.IsNullOrEmpty(txtXMCOUNTRY.Text.Trim()))
                    arlAttribute = SetUITextControls(txtXMCOUNTRY, arlAttribute);
                if (!string.IsNullOrEmpty(txtXMASSET.Text.Trim()))
                    arlAttribute = SetUITextControls(txtXMASSET, arlAttribute);
                if (!string.IsNullOrEmpty(txtXMOWNER.Text.Trim()))
                    arlAttribute = SetUITextControls(txtXMOWNER, arlAttribute);
                if (!string.IsNullOrEmpty(txtXMOWNERORGANISATION.Text.Trim()))
                    arlAttribute = SetUITextControls(txtXMOWNERORGANISATION, arlAttribute);
            }
            catch
            { throw; }
            return arlAttribute;
        }

        /// <summary>
        /// Sets the date attribute group.
        /// </summary>
        /// <returns></returns>
        private AttributeGroup SetDateAttributeGroup()
        {
            AttributeGroup objParentAttributeGroup = new AttributeGroup();
            try
            {
                if (rbLastWeek.Checked == true)
                {
                    SetDateAtributeGroupProperties(objParentAttributeGroup, rbLastWeek);
                }
                else if (rbLastMonth.Checked == true)
                {
                    SetDateAtributeGroupProperties(objParentAttributeGroup, rbLastMonth);
                }
                else if (rbLastYear.Checked == true)
                {
                    SetDateAtributeGroupProperties(objParentAttributeGroup, rbLastYear);
                }
                else if (rbSelectDates.Checked == true)
                {
                    SetDateAtributeGroupProperties(objParentAttributeGroup, rbSelectDates);
                }
            }
            catch
            { throw; }
            return objParentAttributeGroup;
        }

        /// <summary>
        /// Sets the date atribute group properties.
        /// </summary>
        /// <param name="parentAttributeGroup">The parent attribute group.</param>
        /// <param name="dateRadioButton">The date radio button.</param>
        /// <returns></returns>
        private AttributeGroup SetDateAtributeGroupProperties(AttributeGroup parentAttributeGroup, RadioButton dateRadioButton)
        {
            parentAttributeGroup.Operator = GetLogicalOperator();
            parentAttributeGroup.Name = dateRadioButton.ID.Remove(0, 2).ToLower();
            parentAttributeGroup.Checked = TRUE;
            parentAttributeGroup.Label = dateRadioButton.ID;
            parentAttributeGroup.Attribute = SetDateAttribute();

            return parentAttributeGroup;
        }

        /// <summary>
        /// Sets the date attribute.
        /// </summary>
        /// <returns></returns>
        private ArrayList SetDateAttribute()
        {
            ArrayList arlDate = new ArrayList();
            try
            {
                if (rbLastWeek.Checked || rbLastMonth.Checked || rbLastYear.Checked)
                {
                    Attributes objStartDate = new Attributes();
                    objStartDate.Name = TIMESTAMPNODE;
                    objStartDate.Operator = GREATERTHANEQUALS;
                    objStartDate.Value = SetDateValue(STARTDATENODE);
                    arlDate.Add(objStartDate);

                    Attributes objEndDate = new Attributes();
                    objEndDate.Name = TIMESTAMPNODE;
                    objEndDate.Operator = LESSTHANEQUALS;
                    objEndDate.Value = SetDateValue(ENDDATENODE);
                    arlDate.Add(objEndDate);
                }
                else if (rbSelectDates.Checked)
                {
                    Attributes objStartDate = new Attributes();
                    objStartDate.Name = TIMESTAMPNODE;
                    objStartDate.Label = txtStartDate.ID;
                    objStartDate.Operator = GREATERTHANEQUALS;
                    objStartDate.Value = SetDateValue(STARTDATENODE);
                    arlDate.Add(objStartDate);

                    Attributes objEndDate = new Attributes();
                    objEndDate.Name = TIMESTAMPNODE;
                    objEndDate.Label = txtEndDate.ID;
                    objEndDate.Operator = LESSTHANEQUALS;
                    objEndDate.Value = SetDateValue(ENDDATENODE);
                    arlDate.Add(objEndDate);
                }
            }
            catch
            { throw; }
            return arlDate;
        }

        /// <summary>
        /// Sets the date value.
        /// </summary>
        /// <param name="strDate">date.</param>
        /// <returns></returns>
        private ArrayList SetDateValue(string date)
        {
            ArrayList arlDate = new ArrayList();
            try
            {
                Value objDate = new Value();
                if (rbLastWeek.Checked)
                {
                    if (string.Equals(date, STARTDATENODE))
                    {
                        objDate.InnerText = DateTime.Today.AddDays(-7).ToString(WEBSERVICEDATEFORMAT);
                    }
                    else if (string.Equals(date, ENDDATENODE))
                    {
                        objDate.InnerText = DateTime.Today.ToString(WEBSERVICEDATEFORMAT);
                    }
                }
                else if (rbLastMonth.Checked)
                {
                    if (string.Equals(date, STARTDATENODE))
                    {
                        objDate.InnerText = DateTime.Today.AddDays(-31).ToString(WEBSERVICEDATEFORMAT);
                    }
                    else if (string.Equals(date, ENDDATENODE))
                    {
                        objDate.InnerText = DateTime.Today.ToString(WEBSERVICEDATEFORMAT);
                    }
                }
                else if (rbLastYear.Checked)
                {
                    if (string.Equals(date, STARTDATENODE))
                    {
                        objDate.InnerText = DateTime.Today.AddDays(-365).ToString(WEBSERVICEDATEFORMAT);
                    }
                    else if (string.Equals(date, ENDDATENODE))
                    {
                        objDate.InnerText = DateTime.Today.ToString(WEBSERVICEDATEFORMAT);
                    }
                }
                else if (rbSelectDates.Checked)
                {
                    if (string.Equals(date, STARTDATENODE))
                    {
                        objDate.InnerText = objDateTimeConvertorService.GetDateInFormat(txtStartDate.Text, WEBSERVICEDATEFORMAT);
                    }
                    else if (string.Equals(date, ENDDATENODE))
                    {
                        objDate.InnerText = objDateTimeConvertorService.GetDateInFormat(txtEndDate.Text, WEBSERVICEDATEFORMAT);
                    }
                }
                arlDate.Add(objDate);
            }
            catch
            { throw; }
            return arlDate;
        }

        /// <summary>
        /// Sets the geographical attribute group.
        /// </summary>
        /// <returns></returns>
        private AttributeGroup SetGeographicalAttributeGroup()
        {
            AttributeGroup objParentAttributeGroup = new AttributeGroup();
            try
            {
                if (txtMinLatDeg.Text.Length > 0)
                {
                    objParentAttributeGroup.Operator = GetLogicalOperator();
                    objParentAttributeGroup.AttributeGroups = SetGeographicalAttribute();
                }
            }
            catch
            { throw; }
            return objParentAttributeGroup;
        }

        /// <summary>
        /// Sets the geographical attribute.
        /// </summary>
        /// <returns></returns>
        private ArrayList SetGeographicalAttribute()
        {
            ArrayList arlLatLon = new ArrayList();
            try
            {
                ArrayList arlLatitude = new ArrayList();
                AttributeGroup objFirstChildAttributeGroup = new AttributeGroup();
                objFirstChildAttributeGroup.Operator = GetLogicalOperator();

                Attributes objMinLat = new Attributes();
                objMinLat.Name = XMMINLATNODE;
                objMinLat.Operator = GREATERTHANEQUALS;
                objMinLat.Parameter = SetParameter(MINLATITUDE);
                arlLatitude.Add(objMinLat);

                Attributes objMaxLat = new Attributes();
                objMaxLat.Name = XMMAXLATNODE;
                objMaxLat.Operator = LESSTHANEQUALS;
                objMaxLat.Parameter = SetParameter(MAXLATITUDE);
                arlLatitude.Add(objMaxLat);

                objFirstChildAttributeGroup.Attribute = arlLatitude;
                arlLatLon.Add(objFirstChildAttributeGroup);

                ArrayList arlLongitude = new ArrayList();
                AttributeGroup objSecondChildAttributeGroup = new AttributeGroup();
                objSecondChildAttributeGroup.Operator = GetLogicalOperator();

                Attributes objMinLon = new Attributes();
                objMinLon.Name = XMMINLONNODE;
                objMinLon.Operator = GREATERTHANEQUALS;
                objMinLon.Parameter = SetParameter(MINLONGITUDE);
                arlLongitude.Add(objMinLon);

                Attributes objMaxLon = new Attributes();
                objMaxLon.Name = XMMAXLONNODE;
                objMaxLon.Operator = LESSTHANEQUALS;
                objMaxLon.Parameter = SetParameter(MAXLONGITUDE);
                arlLongitude.Add(objMaxLon);

                objSecondChildAttributeGroup.Attribute = arlLongitude;
                arlLatLon.Add(objSecondChildAttributeGroup);
            }
            catch
            { throw; }
            return arlLatLon;
        }

        /// <summary>
        /// Sets the parameter.
        /// </summary>
        /// <param name="strCriteria">criteria.</param>
        /// <returns></returns>
        private ArrayList SetParameter(string criteria)
        {
            ArrayList arlParameter = new ArrayList();
            try
            {
                if (txtMinLatDeg.Text.Length > 0)
                {
                    if (string.Equals(criteria, MINLATITUDE))
                    {
                        arlParameter.Add(SetParameterProperties(txtMinLatDeg));
                        arlParameter.Add(SetParameterProperties(txtMinLatMin));
                        arlParameter.Add(SetParameterProperties(txtMinLatSec));
                        //Dream 4.0 
                        arlParameter = SetLatLonParameter(txtMinLatNS, arlParameter);
                    }
                    else if (string.Equals(criteria, MAXLATITUDE))
                    {
                        arlParameter.Add(SetParameterProperties(txtMaxLatDeg));
                        arlParameter.Add(SetParameterProperties(txtMaxLatMin));
                        arlParameter.Add(SetParameterProperties(txtMaxLatSec));
                        //Dream 4.0 
                        arlParameter = SetLatLonParameter(txtMaxLatNS, arlParameter);
                    }
                    else if (string.Equals(criteria, MINLONGITUDE))
                    {
                        //Set the parameter for Degree,Minute,Second and E/W.
                        arlParameter.Add(SetParameterProperties(txtMinLonDeg));
                        arlParameter.Add(SetParameterProperties(txtMinLonMin));
                        arlParameter.Add(SetParameterProperties(txtMinLonSec));
                        arlParameter.Add(SetParameterProperties(txtMinLonEW));
                    }
                    else if (string.Equals(criteria, MAXLONGITUDE))
                    {
                        //Set the parameter for Degree,Minute,Second and E/W.
                        arlParameter.Add(SetParameterProperties(txtMaxLonDeg));
                        arlParameter.Add(SetParameterProperties(txtMaxLonMin));
                        arlParameter.Add(SetParameterProperties(txtMaxLonSec));
                        arlParameter.Add(SetParameterProperties(txtMaxLonEW));
                    }
                }
            }
            catch
            { throw; }
            return arlParameter;
        }

        /// <summary>
        /// Sets the parameter properties.
        /// </summary>
        /// <param name="geologicalFieldValue">The geological field value.</param>
        /// <returns></returns>
        private Parameters SetParameterProperties(TextBox geologicalFieldValue)
        {
            Parameters objLatLon = new Parameters();
            objLatLon.Name = GetLatLonID(geologicalFieldValue.ID);
            objLatLon.Label = geologicalFieldValue.ID;
            objLatLon.Value = geologicalFieldValue.Text;
            return objLatLon;
        }

        /// <summary>
        /// Determines whether [has basic attribute selected].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [has basic attribute selected]; otherwise, <c>false</c>.
        /// </returns>
        private bool HasBasicAttributeSelected()
        {
            if (txtXMTITLE.Text.Length > 0 ||
                txtXMDESCRIPTION.Text.Length > 0 ||
                txtXMPROJECTNAME.Text.Length > 0 ||
                txtXMMILESTONE.Text.Length > 0 ||
                txtXMBASIN.Text.Length > 0 ||
                txtXMFIELD.Text.Length > 0 ||
                txtXMCOUNTRY.Text.Length > 0 ||
                txtXMASSET.Text.Length > 0 ||
                txtXMOWNER.Text.Length > 0 ||
                txtXMOWNERORGANISATION.Text.Length > 0 || cboSearchCriteria.SelectedIndex > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Checks the child control ID.
        /// </summary>
        /// <param name="childControl">The child control.</param>
        /// <returns></returns>
        private bool CheckChildControlID()
        {
            if ((txtMinLatDeg.Text.ToString().Length > 0) || (txtMinLatMin.Text.ToString().Length > 0) || (txtMinLatSec.Text.ToString().Length > 0) || (txtMinLonDeg.Text.ToString().Length > 0) || (txtMinLonMin.Text.ToString().Length > 0) || (txtMinLonSec.Text.ToString().Length > 0) || (txtMaxLatDeg.Text.ToString().Length > 0) || (txtMaxLatMin.Text.ToString().Length > 0) || (txtMaxLatSec.Text.ToString().Length > 0) || (txtMaxLonDeg.Text.ToString().Length > 0) || (txtMaxLonMin.Text.ToString().Length > 0) || (txtMaxLonSec.Text.ToString().Length > 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Event handler for the Search Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            hidSearchName.Value = "Project Archive";
            try
            {
                if (Page.IsPostBack)
                {
                    DisplaySearchResults();
                }
            }
            catch (WebException webEx)
            {
                ShowLableMessage(webEx.Message);
            }
            catch (SoapException soapEx)
            {
                CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                ShowLableMessage(soapEx.Message);
            }
            catch (Exception ex)
            {
                if (string.Equals(ex.Message, BLANKFILEMESSAGE))
                {
                    ExceptionBlock.Visible = true;
                    lblException.Visible = true;
                    lblException.Text = BLANKFILEMESSAGE;
                }
                else
                    CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }

        /// <summary>
        /// Shows the lable message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void ShowLableMessage(string message)
        {
            ExceptionBlock.Visible = true;
            lblException.Visible = true;
            lblException.Text = message;
        }
    }
}
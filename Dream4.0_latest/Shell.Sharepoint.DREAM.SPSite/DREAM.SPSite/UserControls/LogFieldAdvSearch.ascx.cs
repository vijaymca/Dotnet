#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: LogFieldAdvSearch.ascx.cs 
#endregion

/// <summary> 
/// This is Logs by field Search results class, to make search request and bind the response xml to the page.
/// </summary> 
using System;
using System.Collections;
using System.Web;
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
    /// This is Logs by field Search results class
    /// </summary>
    public partial class LogFieldAdvSearch : UIControlHandler
    {
        #region Declaration
        Entity objEntity = new Entity();
        string strCurrSiteURL = string.Empty;
        const string LOGSFIELDFILENAME = "AdvSearchlogsbyfielddepth.aspx";
        const string NAME = "name";
        const string METRES = "Metres";
        const string FEET = "Feet";
        HiddenField hidSearchName = new HiddenField();
        const string ADMINUSER = "Administrator";
        const string CHECKREFRESH = "LogsCheckRefresh";
        #endregion
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            strCurrSiteURL = HttpContext.Current.Request.Url.ToString();
            objUserPreferences = new UserPreferences();
            string strDepthUnit = string.Empty;
            string strUser = string.Empty;
            try
            {
                lblException.Text = string.Empty;
                cmdSearch.Attributes.Add(REDIRECTATTRIBUTE, REDIRECTATTRIBUTEVALUE);
                cmdReset.Attributes.Add(REDIRECTATTRIBUTE, REDIRECTATTRIBUTEVALUE);
                if (!Page.IsPostBack)
                {
                    //Sets the Logs Check refresh session variable.
                    CommonUtility.SetSessionVariable(Page, CHECKREFRESH, DateTime.Now.ToString());
                    object objSessionUserPreference = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString());
                    if (objSessionUserPreference != null)
                        objUserPreferences = (UserPreferences)objSessionUserPreference;
                    strDepthUnit = objUserPreferences.DepthUnits;
                    base.SearchType = LOGSFIELDSEARCHTYPE;
                    LoadControls(chbShared, cboSavedSearch);
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

                    if (Request.QueryString[SAVESEARCHNAMEQUERYSTRING] != null)
                    {
                        if (Request.QueryString["manage"] != null)
                        {
                            if (string.Equals(Request.QueryString["manage"].ToString(), "true"))
                                strUser = GetUserName();
                            else
                                strUser = "Administrator";
                        }
                        XmlDocument xmldoc = new XmlDocument();
                        //loads the saved search XML Document of administrator 
                        xmldoc = ((MOSSServiceManager)objMossController).GetDocLibXMLFile(SearchType, strUser);
                        SetFeetMetre(xmldoc, cboSavedSearch.SelectedItem.Text.ToString());
                    }

                    if (rdoDepthUnitsMetres.Checked != true && rdoDepthUnitsFeet.Checked != true)
                    {
                        if (string.Equals(strDepthUnit, METRES))
                        {
                            rdoDepthUnitsMetres.Checked = true;
                        }
                        else if (string.Equals(strDepthUnit, FEET))
                        {
                            rdoDepthUnitsFeet.Checked = true;
                        }
                    }
                }
            }
            catch (WebException webEx)
            {
                ShowLableMessage(webEx.Message);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteURL, ex);
            }
        }
        /// <summary>
        /// Binds the tooltip text to controls.
        /// </summary>
        private void BindTooltipTextToControls()
        {
            DataTable dtFilterTooltip = null;
            string strControlName = string.Empty;
            string strTooltipText = string.Empty;
            string strAsset = "Logs By Field or Depth";
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
                        if (string.Equals(imgFieldName.ID.Remove(0, 3).ToString(), strControlName))
                        {
                            imgFieldName.ToolTip = strTooltipText;
                        }
                        if (string.Equals(imgCurveTopDepth.ID.Remove(0, 3).ToString(), strControlName))
                        {
                            imgCurveTopDepth.ToolTip = strTooltipText;
                        }
                        if (string.Equals(imgCurveBottomDepth.ID.Remove(0, 3).ToString(), strControlName))
                        {
                            imgCurveBottomDepth.ToolTip = strTooltipText;
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
        /// Displays the search results from response xml to ui.
        /// </summary>
        private void DisplaySearchResults()
        {

            try
            {
                objRequestInfo = new RequestInfo();
                objRequestInfo = SetBasicDataObjects();
                UISaveSearchHandler objUISaveSearch = new UISaveSearchHandler();
                objUISaveSearch.DisplayResults(Page, objRequestInfo, LOGSFIELDSEARCHTYPE);
                string strUrl = SEARCHRESULTSPAGE + "?SearchType=" + LOGSFIELDSEARCHTYPE + "&asset="+LOGSFIELDSEARCHTYPE;
                RedirectPage(SEARCHRESULTSPAGE + "?SearchType=" + LOGSFIELDSEARCHTYPE, LOGSFIELDSEARCHTYPE);
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
                ArrayList arlBasicAttributeGroup = new ArrayList();
                arlBasicAttributeGroup = SetBasicAttributeGroup();
                objEntity.AttributeGroups = arlBasicAttributeGroup;
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
                if (!string.IsNullOrEmpty(txtFieldName.Text.Trim()))
                    arlAttribute = SetUITextControls(txtFieldName, arlAttribute);
                if (rdoDepthUnitsMetres.Checked)
                {
                    if (!string.IsNullOrEmpty(txtCurveTopDepth.Text.Trim()))
                        arlAttribute = SetUITextControls(txtCurveTopDepth, arlAttribute, METRES);
                    if (!string.IsNullOrEmpty(txtCurveBottomDepth.Text.Trim()))
                        arlAttribute = SetUITextControls(txtCurveBottomDepth, arlAttribute, METRES);
                }
                else if (rdoDepthUnitsFeet.Checked)
                {
                    if (!string.IsNullOrEmpty(txtCurveTopDepth.Text.Trim()))
                        arlAttribute = SetUITextControls(txtCurveTopDepth, arlAttribute, FEET);
                    if (!string.IsNullOrEmpty(txtCurveBottomDepth.Text.Trim()))
                        arlAttribute = SetUITextControls(txtCurveBottomDepth, arlAttribute, FEET);
                }
            }
            catch
            { throw; }
            return arlAttribute;
        }
        /// <summary>
        /// Handles the Click event of the cmdSearch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            hidSearchName.Value = "Logs By Field or Depth";
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
                if (!string.Equals(soapEx.Message.ToString(), SOAPEXCEPTIONMESSAGE))
                {
                    CommonUtility.HandleException(strCurrSiteURL, soapEx, 1);
                }
                ShowLableMessage(soapEx.Message);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteURL, ex);
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
        /// <summary>
        /// Handles the Click event of the cmdReset control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmdReset_Click(object sender, EventArgs e)
        {
            string strUrl = string.Empty;
            if (Request.QueryString["savesearchname"] != null)
            {
                if (Request.QueryString["operation"] != null)
                {
                    if (string.Equals(Request.QueryString["operation"], "modify"))
                    {
                        strUrl = LOGSFIELDFILENAME + "?savesearchname=" + Request.QueryString["savesearchname"] + "&operation=modify";
                    }
                    else
                    {
                        strUrl = LOGSFIELDFILENAME;
                    }
                }
                else
                {
                    strUrl = LOGSFIELDFILENAME;
                }
            }
            else
            {
                strUrl = LOGSFIELDFILENAME;
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
                            objUISaveSearchHandler.ModifySaveSearchXML(LOGSFIELDSEARCHTYPE, strSaveSearchName, chbShared.Checked, cboSavedSearch, xmlDocSearchRequest);
                        }
                        catch (WebException webEx)
                        {
                            ShowLableMessage(webEx.Message);
                        }
                        catch (Exception ex)
                        {
                            ShowLableMessage(ex.Message);
                            CommonUtility.HandleException(strCurrSiteURL, ex);
                        }
                    }
                    else
                    {
                        cboSavedSearch.Items.Clear();
                        cboSavedSearch.Items.Add("---Select---");
                        ((MOSSServiceManager)objMossController).LoadSaveSearch(LOGSFIELDSEARCHTYPE, cboSavedSearch);
                    }
                    cboSavedSearch.Text = txtSaveSearch.Text.ToString().Trim();
                    txtSaveSearch.Text = string.Empty;
                    txtSaveSearch.Enabled = true;
                    cmdSaveSearch.Value = "Save Search";
                }
                else
                {
                    ArrayList arlSavedSearch = new ArrayList();
                    objMossController = objFactory.GetServiceManager("MossService");
                    object objCheckRefresh = CommonUtility.GetSessionVariable(Page, CHECKREFRESH);
                    //Checks whether the event is fired or page has been refreshed.
                    if (string.Equals(ViewState["CheckRefresh"].ToString(), (string)objCheckRefresh))
                    {
                        CommonUtility.SetSessionVariable(Page, CHECKREFRESH, DateTime.Now.ToString());
                        string strSaveSearchName = txtSaveSearch.Text.Trim();
                        //Check for Duplicate Name Exist
                        arlSavedSearch = ((MOSSServiceManager)objMossController).GetSaveSearchName(LOGSFIELDSEARCHTYPE, GetUserName());
                        if (IsDuplicateNameExist(arlSavedSearch, strSaveSearchName))
                        {
                            ExceptionBlock.Visible = true;
                            lblException.Visible = true;
                            lblException.Text = ((MOSSServiceManager)objMossController).GetCustomExceptionMessage(strCurrSiteURL, "12");
                            blnIsNameExist = true;
                        }
                        else
                        {
                            //Create Save Search.
                            XmlDocument xmlDocSearchRequest = null;
                            xmlDocSearchRequest = CreateSaveSearchXML();
                            UISaveSearchHandler objUISaveSearchHandler = null;
                            try
                            {
                                objUISaveSearchHandler = new UISaveSearchHandler();
                                objUISaveSearchHandler.SaveSearchXMLToLibrary(LOGSFIELDSEARCHTYPE, strSaveSearchName, chbShared.Checked, cboSavedSearch, xmlDocSearchRequest);
                            }
                            catch (WebException webEx)
                            {
                                ShowLableMessage(webEx.Message);
                            }
                            catch (Exception ex)
                            {
                                ShowLableMessage(ex.Message);
                                CommonUtility.HandleException(strCurrSiteURL, ex);
                            }
                        }
                    }
                    else
                    {
                        cboSavedSearch.Items.Clear();
                        cboSavedSearch.Items.Add("---Select---");
                        ((MOSSServiceManager)objMossController).LoadSaveSearch(LOGSFIELDSEARCHTYPE, cboSavedSearch);
                    }
                    if (!blnIsNameExist)
                        cboSavedSearch.Text = txtSaveSearch.Text.ToString().Trim();
                    txtSaveSearch.Text = string.Empty;
                }
            }
            catch (Exception ex)
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
                XmlDocument objXmldoc = new XmlDocument();
                objXmldoc = ((MOSSServiceManager)objMossController).GetDocLibXMLFile(LOGSFIELDSEARCHTYPE, strUserID);
                if (cboSavedSearch.SelectedIndex != 0)
                {
                    ClearUIControls();
                    BindUIControls(objXmldoc, cboSavedSearch.SelectedItem.Text.ToString(), chbShared);
                    SetFeetMetre(objXmldoc, cboSavedSearch.SelectedItem.Text.ToString());
                }
                else
                {
                    if (Request.QueryString["savesearchname"] != null)
                    {
                        if (Request.QueryString["operation"] != null)
                        {
                            if (string.Equals(Request.QueryString["operation"].ToString(), "modify"))
                            {
                                ClearUIControls();
                                BindUIControls(objXmldoc, cboSavedSearch.SelectedItem.Text.ToString(), chbShared);
                                SetFeetMetre(objXmldoc, cboSavedSearch.SelectedItem.Text.ToString());
                            }
                            else
                            {
                                Response.Redirect(LOGSFIELDFILENAME, false);
                            }
                        }
                    }
                    else
                    {
                        Response.Redirect(LOGSFIELDFILENAME, false);
                    }
                }
                if (Request.QueryString["savesearchname"] != null)
                {
                    if (Request.QueryString["operation"] != null)
                    {
                        if (string.Equals(Request.QueryString["operation"].ToString(), "modify"))
                        {
                            txtSaveSearch.Text = cboSavedSearch.Text;
                        }
                    }
                }
            }
            catch (WebException webEx)
            {
                ShowLableMessage(webEx.Message);
            }
            catch (Exception ex)
            {
                ShowLableMessage(ex.Message);
                CommonUtility.HandleException(strCurrSiteURL, ex);
            }
        }
        /// <summary>
        /// Sets the feet metre.
        /// </summary>
        /// <param name="saveSearchDoc">The save search doc.</param>
        /// <param name="saveSearchName">Name of the save search.</param>
        private void SetFeetMetre(XmlDocument saveSearchDoc, string saveSearchName)
        {
            XmlNodeList xmlnodelistSaveSearch = saveSearchDoc.SelectNodes(SAVESEARCHXSLPATH);
            XmlDocument xmlDoc;
            foreach (XmlNode xmlnodeSaveSearch in xmlnodelistSaveSearch)
            {
                if (string.Compare(xmlnodeSaveSearch.ParentNode.ParentNode.Attributes.GetNamedItem(ATTRIBNAME).Value.ToString(), saveSearchName) == 0)
                {
                    xmlDoc = new XmlDocument();
                    //Get the XmlDocument for the Selected Search Name.
                    xmlDoc.LoadXml(xmlnodeSaveSearch.InnerXml);
                    XmlNodeList xmlnodelistAttrGrp = xmlDoc.SelectNodes("/attributegroup/attribute");
                    foreach (XmlNode xmlnodeAttrGrp in xmlnodelistAttrGrp)
                    {
                        string strName;
                        try
                        {
                            strName = xmlnodeAttrGrp.Attributes.GetNamedItem(NAME).Value.ToString();
                        }
                        catch (Exception)
                        {
                            strName = string.Empty;
                        }
                        if (strName.Contains(METRES))
                        {
                            rdoDepthUnitsMetres.Checked = true;
                        }
                        else if (strName.Contains(FEET))
                        {
                            rdoDepthUnitsFeet.Checked = true;
                        }
                    }
                }
            }
        }
    }
}
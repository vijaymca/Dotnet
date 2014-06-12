#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: BasinAdvSearch.ascx.cs 
#endregion
using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Xml;
using System.Web.Services.Protocols;
using System.Data;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DREAM.Business.Entities;
using System.Net;

namespace Shell.SharePoint.DREAM.Site.UI.UserControls
{
    /// <summary>
    /// Basin Advance Search.
    /// </summary>
    public partial class BasinAdvSearch : UIControlHandler
    {
        #region Declaration
        const string BASINPAGEURL = "AdvSearchBasin.aspx";
        const string ADMINUSER = "Administrator";
        const string BASINSEARCHTYPE = "BasinSearch";
        const string BASINSAVESEARCHTYPE = "Basin Advance Search";
        const string CHECKREFRESH = "basinCheckRefresh";
        const string BASIN = "Basin";

        const string REPORTSERVICE = "ReportService";
        const string CHECKREFRESHVS = "CheckRefresh";

        Entity objEntity = new Entity();
        #endregion
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            cboSearchCriteria.Attributes.Add("onchange", "javascript:FileSearchTypeSelectedIndexChange(this,'" + lstBasin.ID + "|select" + "')");
            cmdSearch.Attributes.Add(REDIRECTATTRIBUTE, REDIRECTATTRIBUTEVALUE);
            cmdReset.Attributes.Add(REDIRECTATTRIBUTE, REDIRECTATTRIBUTEVALUE);
            objMossController = objFactory.GetServiceManager(MOSSSERVICE);
            objReportController = objFactory.GetServiceManager(REPORTSERVICE);
            SetUserPreferences();
            if (!Page.IsPostBack)
            {
                //Set the Basin CheckRefresh variable in session
                CommonUtility.SetSessionVariable(Page, CHECKREFRESH, DateTime.Now.ToString());
                try
                {
                    base.SearchType = BASINSAVESEARCHTYPE;
                    lblException.Text = string.Empty;
                    ListName = BASINLIST;
                    EntityName = BASINITEMVAL;
                    //**R5k changes for Dream 4.0
                    PopulateListControl(lstBasin, GetBasinFromWebService(), BASINXPATH, objUserPreferences.Basin);
                    LoadControls(chbShared, cboSavedSearch);
                    GetAssetColumns(REPORTSERVICECOLUMNLIST, cboSearchCriteria, BASINITEMVAL);
                    if (Request.QueryString[SAVESEARCHNAMEQUERYSTRING] != null)
                    {
                        if (Request.QueryString["operation"] != null)
                        {
                            if (string.Equals(Request.QueryString["operation"].ToString(), "modify"))
                            {
                                cmdSaveSearch.Value = MODIFYSRCH;
                                txtSaveSearch.Text = Request.QueryString[SAVESEARCHNAMEQUERYSTRING].ToString();
                                txtSaveSearch.Enabled = false;
                            }
                        }
                    }
                    BindTooltipTextToControls();
                }
                catch (SoapException soapEx)
                {
                    if (!string.Equals(soapEx.Message.ToString(), SOAPEXCEPTIONMESSAGE))
                    {
                        CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                    }
                    ShowLableMessage(soapEx.Message.ToString());
                }
                catch (Exception ex)
                {
                    CommonUtility.HandleException(strCurrSiteUrl, ex);
                }
            }
        }
        /// <summary>
        /// Method is use for binding tooltips
        /// </summary>
        private void BindTooltipTextToControls()
        {
            DataTable dtFilterTooltip = null;
            string strControlName = string.Empty;
            string strTooltipText = string.Empty;
            string strAsset = BASIN;
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

                        if (string.Equals(imgBasin.ID.Remove(0, 3).ToString(), strControlName))
                        {
                            imgBasin.ToolTip = strTooltipText;
                        }
                        if (string.Equals(imgSavedSearch.ID.Remove(0, 3).ToString(), strControlName))
                        {
                            imgSavedSearch.ToolTip = strTooltipText;
                        }
                        if (string.Equals(imgDescription.ID.Remove(0, 3).ToString(), strControlName))
                        {
                            imgDescription.ToolTip = strTooltipText;
                        }
                        if (string.Equals(imgStatus.ID.Remove(0, 3).ToString(), strControlName))
                        {
                            imgStatus.ToolTip = strTooltipText;
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
            finally { if (dtFilterTooltip != null)dtFilterTooltip.Dispose(); }
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
                if (string.Equals(cmdSaveSearch.Value.ToString(), MODIFYSRCH))
                {
                    lblException.Visible = false;
                    object objCheckRefresh = CommonUtility.GetSessionVariable(Page, CHECKREFRESH);
                    //Checks whether the event is fired or page has been refreshed.
                    if (string.Equals(ViewState["CheckRefresh"].ToString(), (string)objCheckRefresh))
                    {
                        CommonUtility.SetSessionVariable(Page, CHECKREFRESH, DateTime.Now.ToString());
                        string strSaveSearchName = txtSaveSearch.Text.Trim();
                        XmlDocument xmlDocSearchRequest = null;
                        xmlDocSearchRequest = CreateSaveSearchXML();
                        UISaveSearchHandler objUISaveSearchHandler = null;

                        objUISaveSearchHandler = new UISaveSearchHandler();
                        objUISaveSearchHandler.ModifySaveSearchXML(BASINSAVESEARCHTYPE, strSaveSearchName, chbShared.Checked, cboSavedSearch, xmlDocSearchRequest);
                        if (cboSavedSearch.Items.FindByText(DEFAULTDROPDOWNTEXT) == null)
                        {
                            cboSavedSearch.Items.Insert(0, DEFAULTDROPDOWNTEXT);
                        }

                    }
                    else
                    {
                        cboSavedSearch.Items.Clear();
                        cboSavedSearch.Items.Add(DEFAULTDROPDOWNTEXT);
                        ((MOSSServiceManager)objMossController).LoadSaveSearch(BASINSAVESEARCHTYPE, cboSavedSearch);
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
                    object objCheckRefresh = CommonUtility.GetSessionVariable(Page, CHECKREFRESH);
                    //Checks whether the event is fired or page has been refreshed.
                    if (string.Equals(ViewState[CHECKREFRESHVS].ToString(), (string)objCheckRefresh))
                    {
                        CommonUtility.SetSessionVariable(Page, CHECKREFRESH, DateTime.Now.ToString());
                        string strSaveSearchName = txtSaveSearch.Text.Trim();
                        //Check for Duplicate Name Exist
                        arlSavedSearch = ((MOSSServiceManager)objMossController).GetSaveSearchName(BASINSAVESEARCHTYPE, GetUserName());
                        if (IsDuplicateNameExist(arlSavedSearch, strSaveSearchName))
                        {
                            ShowLableMessage(((MOSSServiceManager)objMossController).GetCustomExceptionMessage(strCurrSiteUrl, "12").ToString());
                            blnIsNameExist = true;
                        }
                        else
                        {
                            //Create Save Search.
                            XmlDocument xmlDocSearchRequest = null;
                            xmlDocSearchRequest = CreateSaveSearchXML();
                            UISaveSearchHandler objUISaveSearchHandler = null;

                            objUISaveSearchHandler = new UISaveSearchHandler();
                            objUISaveSearchHandler.SaveSearchXMLToLibrary(BASINSAVESEARCHTYPE, strSaveSearchName, chbShared.Checked, cboSavedSearch, xmlDocSearchRequest);

                        }
                    }
                    else
                    {
                        cboSavedSearch.Items.Clear();
                        cboSavedSearch.Items.Add(DEFAULTDROPDOWNTEXT);
                        ((MOSSServiceManager)objMossController).LoadSaveSearch(BASINSAVESEARCHTYPE, cboSavedSearch);
                    }
                    if (!blnIsNameExist)
                        cboSavedSearch.Text = txtSaveSearch.Text.ToString().Trim();
                    txtSaveSearch.Text = string.Empty;
                }
            }
            catch (WebException webEx)
            {
                ShowLableMessage(webEx.Message.ToString());
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);

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
                ViewState[CHECKREFRESHVS] = (string)objCheckRefresh;
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
        /// Creates the save search XML.
        /// </summary>
        private XmlDocument CreateSaveSearchXML()
        {
            XmlDocument xmlDocSearchRequest = null;
            objRequestInfo = new RequestInfo();
            objRequestInfo = SetBasicDataObjects();
            xmlDocSearchRequest = objReportController.CreateSearchRequest(objRequestInfo);
            return xmlDocSearchRequest;
        }
        /// <summary>
        /// Event handler for the Search Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsPostBack)
                {
                    DisplaySearchResults();
                }
            }
            catch (WebException webEx)
            {
                ShowLableMessage(webEx.Message.ToString());
            }
            catch (SoapException soapEx)
            {
                if (!string.Equals(soapEx.Message.ToString(), SOAPEXCEPTIONMESSAGE))
                {
                    CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                }
                ShowLableMessage(soapEx.Message.ToString());
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
        /// Displays the search results from response xml to ui.
        /// </summary>
        private void DisplaySearchResults()
        {
            try
            {
                objRequestInfo = new RequestInfo();
                objRequestInfo = SetBasicDataObjects();
                UISaveSearchHandler objUISaveSearch = new UISaveSearchHandler();
                objUISaveSearch.DisplayResults(Page, objRequestInfo, BASINSEARCHTYPE);
                string strUrl = SEARCHRESULTSPAGE + "?SearchType=" + BASINSEARCHTYPE + "&asset=Basin";
                RedirectPage(strUrl, "Basin");
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
                ArrayList arlAttribute = new ArrayList();
                arlAttribute = SetBasicAttribute();
                if (arlAttribute.Count > 1)
                {
                    arlBasicAttributeGroup = SetBasicAttributeGroup(arlAttribute);
                    objEntity.AttributeGroups = arlBasicAttributeGroup;
                }
                else
                {
                    objEntity.Attribute = arlAttribute;
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
        private ArrayList SetBasicAttributeGroup(ArrayList arlAttribute)
        {
            ArrayList arrBasicAttributeGroup = new ArrayList();
            try
            {
                AttributeGroup objBasicAttributeGroup = new AttributeGroup();
                objBasicAttributeGroup.Operator = GetLogicalOperator();
                objBasicAttributeGroup.Attribute = arlAttribute;
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
                if (cboSearchCriteria.SelectedIndex != 0)
                {
                    //overloaded method to search a file for identifier values. Returns an arraylist of Attributes objects
                    arlAttribute = SetBasinCountryAttribute(arlAttribute, lstBasin, ReadFileSearch(fileUploader.PostedFile, hidWordContent.Value), cboSearchCriteria);
                    if (arlAttribute.Count == 0)
                    {
                        throw new Exception(BLANKFILEMESSAGE);
                    }
                }
                else
                {
                    //regular without file search
                    arlAttribute = SetBasinCountryAttribute(arlAttribute, lstBasin);
                }

                if (!string.IsNullOrEmpty(txtDescription.Text.Trim()))
                {
                    arlAttribute = SetUITextControls(txtDescription, arlAttribute);
                }
                if (!string.IsNullOrEmpty(txtStatus.Text.Trim()))
                {
                    arlAttribute = SetUITextControls(txtStatus, arlAttribute);
                }
            }
            catch
            { throw; }
            return arlAttribute;
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
                string strUserID = GetUserName();
                XmlDocument xmldoc = new XmlDocument();
                xmldoc = ((MOSSServiceManager)objMossController).GetDocLibXMLFile(BASINSAVESEARCHTYPE, strUserID);
                ClearUIControls();
              
                //**R5k changes for Dream 4.0
                //start
                // LoadCountryBasinData(BASINLIST, BASIN, lstBasin);
                PopulateListControl(lstBasin, GetBasinFromWebService(), BASINXPATH, objUserPreferences.Basin);
                //end
                lstBasin.ClearSelection();
                lblException.Visible = false;
                if ((cboSavedSearch.SelectedIndex != 0) || (string.Equals(cmdSaveSearch.Value.ToString(), MODIFYSRCH)))
                {
                    BindUIControls(xmldoc, cboSavedSearch.SelectedItem.Text.ToString(), chbShared);

                    if (string.Equals(cmdSaveSearch.Value.ToString(), MODIFYSRCH))
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
        /// Handles the Click event of the cmdReset control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmdReset_Click(object sender, EventArgs e)
        {
            string strUrl = string.Empty;
            if (string.Equals(cmdSaveSearch.Value.ToString(), MODIFYSRCH))
            {
                if (Request.QueryString[SAVESEARCHNAMEQUERYSTRING] != null)
                {
                    strUrl = BASINPAGEURL + "?asset=Basin&manage=true&savesearchname=" + Request.QueryString[SAVESEARCHNAMEQUERYSTRING].ToString() + "&operation=modify";
                }
                else
                {
                    strUrl = BASINPAGEURL + "?asset=Basin";
                }
            }
            else
            {
                strUrl = BASINPAGEURL + "?asset=Basin";
            }
            RedirectPage(strUrl, string.Empty);
        }
    }
}
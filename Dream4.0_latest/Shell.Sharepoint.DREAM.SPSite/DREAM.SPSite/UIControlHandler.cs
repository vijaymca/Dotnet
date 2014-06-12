#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: UIControlHandler.cs
#endregion
using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Web.Services.Protocols;
using Shell.SharePoint.DREAM.Controller;
using Telerik.Web.UI;


namespace Shell.SharePoint.DREAM.Site.UI
{
    /// <summary>
    /// This is the Handler class for UI Controls.
    /// </summary>
    public class UIControlHandler :UIHelper
    {
        #region DECLARATION

        string strListName = string.Empty;
        string strEntityName = string.Empty;
        string strSearchType = string.Empty;
        protected const string SAVESEARCHNAMEQUERYSTRING = "savesearchname";
        protected const string OPERATIONQUERYSTRING = "operation";
        protected const string MANAGEQUERYSTRING = "manage";
        #region DREAM4.0
        protected XmlDocument xmlDocRequest;
        protected string strSelectedRows;
        protected string strSelectedCriteriaName;
        protected string strSelectedAssetNames;
        protected string strSearchName;
        protected DropDownList ddlAssets;
        #endregion
        #endregion

        #region DREAM 4.0
        #region Properties

        /// <summary>
        /// Gets or sets the selected rows.
        /// </summary>
        /// <value>The selected rows.</value>
        public string SelectedRows
        {
            get
            {
                return strSelectedRows;
            }
            set
            {
                strSelectedRows = value;
            }
        }
        /// <summary>
        /// Gets or sets the name of the selected criteria.
        /// </summary>
        /// <value>The name of the selected criteria.</value>
        public string SelectedCriteriaName
        {
            get
            {
                return strSelectedCriteriaName;
            }
            set
            {
                strSelectedCriteriaName = value;
            }
        }
        /// <summary>
        /// Gets or sets the selected asset names.
        /// </summary>
        /// <value>The selected asset names.</value>
        public string SelectedAssetNames
        {
            get
            {
                return strSelectedAssetNames;
            }
            set
            {
                strSelectedAssetNames = value;
            }
        }
        /// <summary>
        /// Gets or sets the request XML.
        /// </summary>
        /// <value>The request XML.</value>
        public XmlDocument RequestXML
        {
            get
            {
                return xmlDocRequest;
            }
            set
            {
                xmlDocRequest = value;
            }
        }
        /// <summary>
        /// Gets or sets the name of the search.
        /// </summary>
        /// <value>The name of the search.</value>
        public string SearchName
        {
            get
            {
                return strSearchName;
            }
            set
            {
                strSearchName = value;
            }
        }
        #endregion
        #endregion
        /// <summary>
        /// Gets or sets the name of the list.
        /// </summary>
        /// <value>The name of the list.</value>
        protected string ListName
        {
            get
            {
                return strListName;
            }
            set
            {
                strListName = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the entity.
        /// </summary>
        /// <value>The name of the entity.</value>
        protected string EntityName
        {
            get
            {
                return strEntityName;
            }
            set
            {
                strEntityName = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the search.
        /// </summary>
        /// <value>The type of the search.</value>
        protected string SearchType
        {
            get
            {
                return strSearchType;
            }
            set
            {
                strSearchType = value;
            }
        }

        #region LoadControls
        /// <summary>
        /// Loads the controls from Adv Field Search With out SRP Controls.
        /// </summary>
        /// <param name="chbShared">The shared CheckBox.</param>
        /// <param name="cboSavedSearch">The saved search DropDown box.</param>
        /// <param name="countryOrBasin">The country or basin ListBox.</param>
        protected void LoadControls(CheckBox sharedCheckBox, DropDownList savedSearch)
        {
            string strUserID = string.Empty;
            try
            {
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                if(sharedCheckBox != null && savedSearch != null)
                {
                    //Commented in R5k changes in Dream 4.0
                    /* if (countryOrBasin != null)
                     {
                         LoadCountryBasinData(ListName, EntityName, countryOrBasin);
                     }*/
                    if(Request.QueryString.Count > 0)
                    {
                        //This will check the savesearchname query string which will come from standard search detail link and loads the controls.
                        if(Request.QueryString[SAVESEARCHNAMEQUERYSTRING] != null)
                        {
                            if(Request.QueryString[OPERATIONQUERYSTRING] != null)
                            {
                                if(string.Equals(Request.QueryString[OPERATIONQUERYSTRING], "modify"))
                                {
                                    savedSearch.Items.Clear();
                                }
                            }

                            if(Request.QueryString[MANAGEQUERYSTRING] != null)
                            {
                                if(string.Equals(Request.QueryString[MANAGEQUERYSTRING], TRUE))
                                    strUserID = GetUserName();
                                else
                                    strUserID = ADMINUSER;
                            }
                        }
                        else
                            strUserID = GetUserName();
                    }
                    ((MOSSServiceManager)objMossController).LoadSaveSearch(SearchType, savedSearch);
                    if(Request.QueryString.Count > 0)
                    {
                        //This will check the savesearchname query string which will come from standard search detail link and loads the controls.
                        if(Request.QueryString[SAVESEARCHNAMEQUERYSTRING] != null)
                        {
                            XmlDocument xmldoc = new XmlDocument();
                            //loads the saved search XML Document of administrator 
                            xmldoc = ((MOSSServiceManager)objMossController).GetDocLibXMLFile(SearchType, strUserID);
                            BindUIControls(xmldoc, Request.QueryString[SAVESEARCHNAMEQUERYSTRING].ToString(), sharedCheckBox);
                            if(Request.QueryString[MANAGEQUERYSTRING] != null)
                            {
                                if(string.Equals(Request.QueryString[MANAGEQUERYSTRING].ToString(), TRUE))
                                    savedSearch.Text = Request.QueryString[SAVESEARCHNAMEQUERYSTRING];
                            }
                        }
                    }
                    //this will check the condition for logged in user and based on that Shared Save Search check box will be visible.
                    strUserID = GetUserName();
                    if(string.Compare(strUserID, ADMINUSER) == 0)
                    {
                        sharedCheckBox.Visible = true;
                    }
                    else
                    {
                        sharedCheckBox.Visible = false;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        #region  SRP Code
        /// <summary>
        /// Loads the controls which are passed as parameters from Field ADV search With SRP Controls. 
        /// </summary>
        /// <param name="chbShared">The shared CheckBox.</param>
        /// <param name="cboSavedSearch">The saved search DropDown box.</param>
        /// <param name="countryOrBasin">The country or basin ListBox.</param>
        /// <param name="operationaEnv">The operationaEnvironment Dropdownlist.</param>
        /// <param name="reserveMagnitudeOil">The reserve Magnitude Oil Dropdownlist.</param>
        /// <param name="reserveMagnitudeGas">The reserve Magnitude Gas Dropdownlist.</param>
        /// <param name="tectonicSettings">The Tectonic Settings Dropdownlist.</param>
        protected void LoadControls(CheckBox sharedCheckBox, DropDownList savedSearch, ListControl countryOrBasin, DropDownList operationaEnv, DropDownList reserveMagnitudeOil, DropDownList reserveMagnitudeGas, DropDownList tectonicSettings, DropDownList cboTeconicSettingKle, RadioButtonList searchConditionOperator)//DREAM4.0 Code changes done for adv search country dropdown
        {
            string strUserID = string.Empty;
            try
            {
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                if(sharedCheckBox != null && savedSearch != null)
                {
                    if(countryOrBasin != null)
                    {
                        LoadCountryBasinData(ListName, EntityName, countryOrBasin);
                    }

                    if(operationaEnv != null)
                    {
                        LoadDropdownData(OPERATIONALENVLIST, operationaEnv);
                    }
                    if(reserveMagnitudeOil != null)
                    {
                        LoadDropdownData(RESERVEMAGNITUDEOILLIST, reserveMagnitudeOil);
                    }
                    if(reserveMagnitudeGas != null)
                    {
                        LoadDropdownData(RESERVEMAGNITUDEGASLIST, reserveMagnitudeGas);
                    }
                    if(tectonicSettings != null)
                    {
                        LoadDropdownData(TECHONICSETTINNGSLIST, tectonicSettings);
                    }
                    if(cboTeconicSettingKle != null)
                    {
                        LoadDropdownData(TECHONICSETTINNGSLIST, cboTeconicSettingKle);
                    }
                    if(Request.QueryString.Count > 0)
                    {
                        //This will check the savesearchname query string which will come from standard search detail link and loads the controls.
                        if(Request.QueryString[SAVESEARCHNAMEQUERYSTRING] != null)
                        {
                            if(Request.QueryString[OPERATIONQUERYSTRING] != null)
                            {
                                if(string.Equals(Request.QueryString[OPERATIONQUERYSTRING].ToString(), "modify"))
                                {
                                    savedSearch.Items.Clear();
                                }
                            }

                            if(Request.QueryString[MANAGEQUERYSTRING] != null)
                            {
                                if(string.Equals(Request.QueryString[MANAGEQUERYSTRING].ToString(), TRUE))
                                    strUserID = GetUserName();
                                else
                                    strUserID = ADMINUSER;
                            }
                        }
                        else
                            strUserID = GetUserName();
                    }
                    ((MOSSServiceManager)objMossController).LoadSaveSearch(SearchType, savedSearch);
                    if(Request.QueryString.Count > 0)
                    {
                        //This will check the savesearchname query string which will come from standard search detail link and loads the controls.
                        if(Request.QueryString[SAVESEARCHNAMEQUERYSTRING] != null)
                        {
                            XmlDocument xmldoc = new XmlDocument();
                            //loads the saved search XML Document of administrator 
                            xmldoc = ((MOSSServiceManager)objMossController).GetDocLibXMLFile(SearchType, strUserID);
                            BindSearchConditionOperator(xmldoc, Request.QueryString[SAVESEARCHNAMEQUERYSTRING], searchConditionOperator);
                            BindUIControls(xmldoc, Request.QueryString[SAVESEARCHNAMEQUERYSTRING].ToString(), sharedCheckBox);
                            if(Request.QueryString[MANAGEQUERYSTRING] != null)
                            {
                                if(string.Equals(Request.QueryString[MANAGEQUERYSTRING].ToString(), TRUE))
                                    savedSearch.Text = Request.QueryString[SAVESEARCHNAMEQUERYSTRING].ToString();
                            }
                        }
                    }
                    //this will check the condition for logged in user and based on that Shared Save Search check box will be visible.
                    strUserID = GetUserName();
                    if(string.Compare(strUserID, ADMINUSER) == 0)
                    {
                        sharedCheckBox.Visible = true;
                    }
                    else
                    {
                        sharedCheckBox.Visible = false;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion


        /// <summary>
        /// Loads the controls from Reservoir Adv Search.
        /// </summary>
        /// <param name="sharedCheckBox">The shared check box.</param>
        /// <param name="cboSavedSearch">The  saved search.</param>
        /// <param name="cboGrainSizeMean">The cbo grain size mean.</param>
        /// <param name="cboProductionStatus">The  production status.</param>
        /// <param name="cboDriveMechanism">The  drive mechanism.</param>
        /// <param name="cboHydrocarbonMain">The cbo hydrocarbon main.</param>
        /// <param name="cboHydrocarbonSecondary">The  hydrocarbon secondary.</param>
        /// <param name="searchConditionOperator">The search condition operator.</param>
        protected void LoadControls(CheckBox sharedCheckBox, DropDownList cboSavedSearch, DropDownList cboGrainSizeMean, DropDownList cboDriveMechanism, DropDownList cboHydrocarbonMain, DropDownList cboHydrocarbonSecondary, RadioButtonList searchConditionOperator)
        {  //DropDownList cboLithologyMain, DropDownList cboLithologySecondary
            string strUserID = string.Empty;
            try
            {
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                {

                    if(cboGrainSizeMean != null)
                    {
                        LoadDropdownData(GRAINSIZEMEANLIST, cboGrainSizeMean);
                    }

                    if(cboDriveMechanism != null)
                    {
                        LoadDropdownData(DRIVEMECHANISMLIST, cboDriveMechanism);
                    }
                    if(cboHydrocarbonMain != null)
                    {
                        LoadDropdownData(HYDROCARBONMAINLIST, cboHydrocarbonMain);
                    }
                    if(cboHydrocarbonSecondary != null)
                    {
                        LoadDropdownData(HYDROCARBONSECONDARYLIST, cboHydrocarbonSecondary);
                    }
                    if(Request.QueryString.Count > 0)
                    {
                        //This will check the savesearchname query string which will come from standard search detail link and loads the controls.
                        if(Request.QueryString[SAVESEARCHNAMEQUERYSTRING] != null)
                        {
                            if(Request.QueryString[OPERATIONQUERYSTRING] != null)
                            {
                                if(string.Equals(Request.QueryString[OPERATIONQUERYSTRING].ToString(), "modify"))
                                {
                                    cboSavedSearch.Items.Clear();
                                }
                            }

                            if(Request.QueryString[MANAGEQUERYSTRING] != null)
                            {
                                if(string.Equals(Request.QueryString[MANAGEQUERYSTRING].ToString(), TRUE))
                                    strUserID = GetUserName();
                                else
                                    strUserID = ADMINUSER;
                            }
                        }
                        else
                            strUserID = GetUserName();
                    }
                    ((MOSSServiceManager)objMossController).LoadSaveSearch(SearchType, cboSavedSearch);
                    if(Request.QueryString.Count > 0)
                    {
                        //This will check the savesearchname query string which will come from standard search detail link and loads the controls.
                        if(Request.QueryString[SAVESEARCHNAMEQUERYSTRING] != null)
                        {
                            XmlDocument xmldoc = new XmlDocument();
                            //loads the saved search XML Document of administrator 
                            xmldoc = ((MOSSServiceManager)objMossController).GetDocLibXMLFile(SearchType, strUserID);
                            BindSearchConditionOperator(xmldoc, Request.QueryString[SAVESEARCHNAMEQUERYSTRING], searchConditionOperator);
                            BindUIControls(xmldoc, Request.QueryString[SAVESEARCHNAMEQUERYSTRING].ToString(), sharedCheckBox);
                            if(Request.QueryString[MANAGEQUERYSTRING] != null)
                            {
                                if(string.Equals(Request.QueryString[MANAGEQUERYSTRING].ToString(), TRUE))
                                    cboSavedSearch.Text = Request.QueryString[SAVESEARCHNAMEQUERYSTRING].ToString();
                            }
                        }
                    }
                    //this will check the condition for logged in user and based on that Shared Save Search check box will be visible.
                    strUserID = GetUserName();
                    if(string.Compare(strUserID, ADMINUSER) == 0)
                    {
                        sharedCheckBox.Visible = true;
                    }
                    else
                    {
                        sharedCheckBox.Visible = false;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion LoadControls

        #region Bind Controls

        /// <summary>
        /// Binds the UI controls.
        /// </summary>
        /// <param name="saveSearchDoc">The savesearch Xmldocument.</param>
        /// <param name="saveSearchName">Name of the save search selected by user.</param>
        /// <param name="chbShared">The CHB shared.</param>
        protected void BindUIControls(XmlDocument saveSearchDoc, string saveSearchName, CheckBox chbShared)
        {
            XmlDocument xmlDoc = null;
            //load the shared checkbox value based on selected saved search.
            XmlNodeList xmlnodelistSharedSaveSearch = saveSearchDoc.SelectNodes(SAVESHAREDSEARCHXSLPATH + "[@" + ATTRIBNAME + "=\"" + saveSearchName + "\"]");
            XmlNode xmlnodeSharedSaveSearch = xmlnodelistSharedSaveSearch.Item(0);
            if(string.Compare(xmlnodeSharedSaveSearch.Attributes.GetNamedItem(ATTRIBSHARED).Value.ToString().ToLowerInvariant(), TRUE) == 0)
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
                //Checks whether the savesearch name matches the name in the saved document.
                if(string.Compare(xmlnodeSaveSearch.ParentNode.ParentNode.Attributes.GetNamedItem(ATTRIBNAME).Value.ToString(), saveSearchName) == 0)
                {
                    xmlDoc = new XmlDocument();
                    //Get the XmlDocument for the Selected Search Name.
                    xmlDoc.LoadXml(xmlnodeSaveSearch.InnerXml);
                    if(string.Equals(xmlDoc.FirstChild.Name, ATTRIBUTEGROUP))
                        BindAttributeGroup(xmlDoc, SINGLEATTRIBUTEGROUPXPATH);
                    else
                        BindAttributeGroup(xmlDoc, ATTRIBUTEXPATH);
                }
            }
        }

        /// <summary>
        /// Binds the search condition operator.
        /// </summary>
        /// <param name="saveSearchDoc">The save search doc.</param>
        /// <param name="saveSearchName">Name of the save search.</param>
        /// <param name="rdblSearchConditionList">The RDBL search condition list.</param>
        protected void BindSearchConditionOperator(XmlDocument saveSearchDoc, string saveSearchName, RadioButtonList rdblSearchConditionList)
        {
            if(saveSearchDoc != null)
            {
                XmlNode xmlNodeSaveSearch = saveSearchDoc.SelectSingleNode("saveSearchRequests/saveSearchRequest[@name='" + saveSearchName + "']");
                XmlNode xmlNodeOperator = null;
                if(xmlNodeSaveSearch != null)
                {
                    xmlNodeOperator = xmlNodeSaveSearch.SelectSingleNode("requestinfo/entity/attributegroup");
                }
                if(xmlNodeOperator != null && xmlNodeOperator.Attributes["operator"] != null)
                {
                    if(rdblSearchConditionList.Items.FindByText(xmlNodeOperator.Attributes["operator"].Value) != null)
                    {
                        rdblSearchConditionList.ClearSelection();
                        rdblSearchConditionList.Items.FindByText(xmlNodeOperator.Attributes["operator"].Value).Selected = true;
                    }
                    else if(rdblSearchConditionList.Items.FindByValue(xmlNodeOperator.Attributes["operator"].Value) != null)
                    {
                        rdblSearchConditionList.ClearSelection();
                        rdblSearchConditionList.Items.FindByValue(xmlNodeOperator.Attributes["operator"].Value).Selected = true;
                    }
                }
                else
                {
                    rdblSearchConditionList.SelectedIndex = 0;
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
            string strLabel;
            TextBox objTextBox = new TextBox();
            ListBox objListBox = new ListBox();
            RadioButton objRadioButton = new RadioButton();
            #region  SRP Code
            DropDownList objDropdownList = new DropDownList();
            RadComboBox objRadComboBox = new RadComboBox();
            RadioButtonList objRadioButtonList = new RadioButtonList();
            HiddenField objHiddenField = new HiddenField();
            #endregion
            XmlNodeList xmlnodelistAttrGrp = saveSearchDoc.SelectNodes(saveSearchXPATH);
            //loop through the save search XML.
            foreach(XmlNode xmlnodeAttrGrp in xmlnodelistAttrGrp)
            {
                try
                {
                    //Get the Control label.
                    strLabel = xmlnodeAttrGrp.Attributes.GetNamedItem(LABEL).Value.ToString();
                }
                catch(Exception)
                {
                    strLabel = string.Empty;
                }
                if(strLabel.Length > 0)
                {
                    //Loop through the controls in page.
                    foreach(Control objControl in this.Controls)
                    {
                        //Loop through the controls in the page's panel.
                        foreach(Control objChildControl in objControl.Controls)
                        {
                            #region RadioButton Control
                            //Checks whether the control is a radio button or not.
                            if(string.Compare(objChildControl.GetType().ToString(), objRadioButton.GetType().ToString()) == 0)
                            {
                                if(string.Equals(objChildControl.ID, strLabel))
                                {
                                    ((RadioButton)(objChildControl)).Checked = true;
                                }
                            }
                            #endregion RadioButton Control

                            #region TextBox Control
                            //Check for TextBox controls in the list.
                            if(string.Compare(objChildControl.GetType().ToString(), objTextBox.GetType().ToString()) == 0)
                            {
                                //if the Control ID matches then it will assign the value.
                                if(string.Compare(objChildControl.ID, strLabel) == 0)
                                {
                                    if(string.Compare(xmlnodeAttrGrp.FirstChild.Name, VALUE) == 0)
                                    {
                                        ((TextBox)(objChildControl)).Text = xmlnodeAttrGrp.FirstChild.InnerText.ToString();
                                    }
                                    else
                                    {
                                        ((TextBox)(objChildControl)).Text = xmlnodeAttrGrp.Attributes.GetNamedItem(VALUE).Value.ToString();
                                    }
                                }
                            }
                            #endregion TextBox Control

                            #region ListBox Control
                            //Checks whether the control is a List Box or not.
                            if(string.Compare(objChildControl.GetType().ToString(), objListBox.GetType().ToString()) == 0)
                            {
                                //Checks whether the Id matches with the Label.
                                if(string.Compare(objChildControl.ID, strLabel) == 0)
                                {
                                    ((ListBox)(objChildControl)).ClearSelection();
                                    XmlNodeList xmlnodelistValue = saveSearchDoc.SelectNodes(saveSearchXPATH + "/value");
                                    //Modification done for Field Issue.
                                    if(!string.Equals(xmlnodeAttrGrp.Attributes.GetNamedItem(CHECKED).Value.ToString(), FALSE))
                                    {
                                        //Loop through the items in the list.
                                        foreach(ListItem lstItems in ((ListBox)(objChildControl)).Items)
                                        {
                                            foreach(XmlNode xmlnodeListBoxItem in xmlnodelistValue)
                                            {
                                                if(string.Compare(xmlnodeListBoxItem.ParentNode.Attributes.GetNamedItem(LABEL).Value.ToString(), strLabel) == 0)
                                                {
                                                    //check for the item text and if the value matches then that item will be selected.
                                                    if(string.Compare(lstItems.Value, xmlnodeListBoxItem.InnerText) == 0)
                                                    {
                                                        lstItems.Selected = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion ListBox Control

                            #region DropDownList Control

                            //Check for Drop controls in the list.
                            if(string.Compare(objChildControl.GetType().ToString(), objDropdownList.GetType().ToString()) == 0)
                            {
                                //if the Control ID matches then it will assign the value.
                                if(string.Compare(objChildControl.ID, strLabel) == 0)
                                {
                                    string objvalue = string.Empty;

                                    if(string.Compare(xmlnodeAttrGrp.FirstChild.Name, VALUE) == 0)
                                    {
                                        objvalue = xmlnodeAttrGrp.FirstChild.InnerText.ToString();
                                    }
                                    else
                                    {
                                        objvalue = xmlnodeAttrGrp.Attributes.GetNamedItem(VALUE).Value.ToString();
                                    }
                                    //Loop through the items in the list.
                                    foreach(ListItem lstItem in ((DropDownList)(objChildControl)).Items)
                                    {
                                        if(string.Compare(lstItem.Value, objvalue) == 0)
                                        {
                                            ((DropDownList)(objChildControl)).SelectedValue = lstItem.Value;
                                        }

                                    }
                                }
                            }
                            #endregion DropDownList Control

                            #region RadioButtonList Control
                            //check for readio button list
                            if(string.Compare(objChildControl.GetType().ToString(), objRadioButtonList.GetType().ToString()) == 0)
                            {
                                //if the Control ID matches then it will assign the value.
                                if(string.Compare(objChildControl.ID, strLabel) == 0)
                                {
                                    string objvalue = string.Empty;

                                    if(string.Compare(xmlnodeAttrGrp.FirstChild.Name, VALUE) == 0)
                                    {
                                        objvalue = xmlnodeAttrGrp.FirstChild.InnerText.ToString();
                                    }
                                    else
                                    {
                                        objvalue = xmlnodeAttrGrp.Attributes.GetNamedItem(VALUE).Value.ToString();
                                    }
                                    //Loop through the items in the list.
                                    foreach(ListItem lstItem in ((RadioButtonList)(objChildControl)).Items)
                                    {
                                        if(string.Compare(lstItem.Value, objvalue) == 0)
                                        {
                                            ((RadioButtonList)(objChildControl)).SelectedValue = lstItem.Value;
                                        }

                                    }
                                }
                            }

                            #endregion RadioButtonList Control

                            #region RadComboBox Control
                            //Check for RadComboBox in the list.
                            if(string.Compare(objChildControl.GetType().ToString(), objRadComboBox.GetType().ToString()) == 0)
                            {
                                //if the Control ID matches then it will assign the value.
                                if(string.Compare(objChildControl.ID, strLabel) == 0)
                                {
                                    string objvalue = string.Empty;

                                    if(string.Compare(xmlnodeAttrGrp.FirstChild.Name, VALUE) == 0)
                                    {
                                        objvalue = xmlnodeAttrGrp.FirstChild.InnerText.ToString();
                                    }
                                    else
                                    {
                                        objvalue = xmlnodeAttrGrp.Attributes.GetNamedItem(VALUE).Value.ToString();
                                    }
                                    if(!string.IsNullOrEmpty(objvalue) && objvalue.Contains(";"))
                                    {
                                        RadComboBoxItem item = new RadComboBoxItem(objvalue.Split(";".ToCharArray())[0], objvalue.Split(";".ToCharArray())[1]);
                                        ((RadComboBox)(objChildControl)).Items.Add(item);
                                        ((RadComboBox)(objChildControl)).SelectedValue = item.Value;
                                        ((RadComboBox)(objChildControl)).Text = item.Text;
                                    }
                                }
                            }
                            #endregion RadComboBox Control

                            #region Hidden Control
                            //Check for TextBox controls in the list.
                            if(string.Compare(objChildControl.GetType().ToString(), objHiddenField.GetType().ToString()) == 0)
                            {
                                //if the Control ID matches then it will assign the value.
                                if(string.Compare(objChildControl.ID, strLabel) == 0)
                                {
                                    if(string.Compare(xmlnodeAttrGrp.FirstChild.Name, VALUE) == 0)
                                    {
                                        ((HiddenField)(objChildControl)).Value = xmlnodeAttrGrp.FirstChild.InnerText.ToString();
                                    }
                                    else
                                    {
                                        ((HiddenField)(objChildControl)).Value = xmlnodeAttrGrp.Attributes.GetNamedItem(VALUE).Value.ToString();
                                    }
                                }
                            }
                            #endregion Hidden Control
                        }
                    }
                }
                //If the xmlnode has childnodes then process repeats for childnodes also.
                if(xmlnodeAttrGrp.HasChildNodes)
                {
                    //This will change the XPATH by adding Attributegroup.
                    if(string.Equals(xmlnodeAttrGrp.FirstChild.Name.ToString(), ATTRIBUTEGROUP))
                    {
                        saveSearchXPATH = saveSearchXPATH.ToString() + SINGLEATTRIBUTEGROUPXPATH;
                        BindAttributeGroup(saveSearchDoc, saveSearchXPATH);
                    }
                    //This will change the XPATH by adding Attribute.
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
            //Checks whether the XmlnodeList do not have any xmlNode.
            if(xmlnodelistAttrGrp.Count == 0)
            {
                XmlNodeList xmlnodelistAttrGrps = saveSearchDoc.SelectNodes(ATTRIBUTEXPATH);
                XmlNode xmlnodeAttrGrp = xmlnodelistAttrGrps.Item(0);
                strLabel = xmlnodeAttrGrp.Attributes.GetNamedItem(LABEL).Value.ToString();
                if(strLabel.Length > 0)
                {
                    foreach(Control objControl in this.Controls)
                    {
                        foreach(Control objChildControl in objControl.Controls)
                        {
                            #region Radio Button
                            if(string.Compare(objChildControl.GetType().ToString(), objRadioButton.GetType().ToString()) == 0)
                            {
                                if(string.Equals(objChildControl.ID, strLabel))
                                {
                                    ((RadioButton)(objChildControl)).Checked = true;
                                }
                            }
                            #endregion Radio Button

                            #region TextBox Control
                            //Check for TextBox controls in the list.
                            if(string.Compare(objChildControl.GetType().ToString(), objTextBox.GetType().ToString()) == 0)
                            {
                                //if the Control ID matches then it will assign the value.
                                if(string.Compare(objChildControl.ID, strLabel) == 0)
                                {
                                    if(string.Compare(xmlnodeAttrGrp.FirstChild.Name, VALUE) == 0)
                                    {
                                        ((TextBox)(objChildControl)).Text = xmlnodeAttrGrp.FirstChild.InnerText.ToString();
                                    }
                                    else
                                    {
                                        ((TextBox)(objChildControl)).Text = xmlnodeAttrGrp.Attributes.GetNamedItem(VALUE).Value.ToString();
                                    }
                                }
                            }
                            #endregion TextBox Control

                            #region ListBox Control
                            //Check for ListBox type controls in the list.
                            if(string.Compare(objChildControl.GetType().ToString(), objListBox.GetType().ToString()) == 0)
                            {
                                if(string.Compare(objChildControl.ID, strLabel) == 0)
                                {
                                    ((ListBox)(objChildControl)).ClearSelection();
                                    XmlNodeList xmlnodelistValue = saveSearchDoc.SelectNodes(saveSearchXPATH + "/value");
                                    if(!string.Equals(xmlnodeAttrGrp.Attributes.GetNamedItem(CHECKED).Value.ToString(), FALSE))
                                    {
                                        //Loop through the items in the list.
                                        foreach(ListItem lstItems in ((ListBox)(objChildControl)).Items)
                                        {
                                            foreach(XmlNode xmlnodeListBoxItem in xmlnodelistValue)
                                            {
                                                //check for the item text and if the value matches then that item will be selected.
                                                if(string.Compare(xmlnodeListBoxItem.ParentNode.Attributes.GetNamedItem(LABEL).Value.ToString(), strLabel) == 0)
                                                {
                                                    if(string.Compare(lstItems.Value, xmlnodeListBoxItem.InnerText) == 0)
                                                    {
                                                        lstItems.Selected = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion ListBox Control

                            #region DropDownList Control

                            //Check for Drop controls in the list.
                            if(string.Compare(objChildControl.GetType().ToString(), objDropdownList.GetType().ToString()) == 0)
                            {
                                //if the Control ID matches then it will assign the value.
                                if(string.Compare(objChildControl.ID, strLabel) == 0)
                                {
                                    string objvalue = string.Empty;

                                    if(string.Compare(xmlnodeAttrGrp.FirstChild.Name, VALUE) == 0)
                                    {
                                        objvalue = xmlnodeAttrGrp.FirstChild.InnerText.ToString();
                                    }
                                    else
                                    {
                                        objvalue = xmlnodeAttrGrp.Attributes.GetNamedItem(VALUE).Value.ToString();
                                    }
                                    //Loop through the items in the list.
                                    foreach(ListItem lstItem in ((DropDownList)(objChildControl)).Items)
                                    {
                                        if(string.Compare(lstItem.Value, objvalue) == 0)
                                        {
                                            ((DropDownList)(objChildControl)).SelectedValue = lstItem.Value;
                                        }

                                    }
                                }
                            }
                            #endregion DropDownList Control

                            #region RadioButtonList Control
                            //check for readio button list
                            if(string.Compare(objChildControl.GetType().ToString(), objRadioButtonList.GetType().ToString()) == 0)
                            {
                                //if the Control ID matches then it will assign the value.
                                if(string.Compare(objChildControl.ID, strLabel) == 0)
                                {
                                    string objvalue = string.Empty;

                                    if(string.Compare(xmlnodeAttrGrp.FirstChild.Name, VALUE) == 0)
                                    {
                                        objvalue = xmlnodeAttrGrp.FirstChild.InnerText.ToString();
                                    }
                                    else
                                    {
                                        objvalue = xmlnodeAttrGrp.Attributes.GetNamedItem(VALUE).Value.ToString();
                                    }
                                    //Loop through the items in the list.
                                    foreach(ListItem lstItem in ((RadioButtonList)(objChildControl)).Items)
                                    {
                                        if(string.Compare(lstItem.Value, objvalue) == 0)
                                        {
                                            ((RadioButtonList)(objChildControl)).SelectedValue = lstItem.Value;
                                        }

                                    }
                                }
                            }
                            #endregion RadioButtonList Control

                            #region RadComboBox Control
                            //Check for RadComboBox in the list.
                            if(string.Compare(objChildControl.GetType().ToString(), objRadComboBox.GetType().ToString()) == 0)
                            {
                                //if the Control ID matches then it will assign the value.
                                if(string.Compare(objChildControl.ID, strLabel) == 0)
                                {
                                    string objvalue = string.Empty;

                                    if(string.Compare(xmlnodeAttrGrp.FirstChild.Name, VALUE) == 0)
                                    {
                                        objvalue = xmlnodeAttrGrp.FirstChild.InnerText.ToString();
                                    }
                                    else
                                    {
                                        objvalue = xmlnodeAttrGrp.Attributes.GetNamedItem(VALUE).Value.ToString();
                                    }
                                    if(!string.IsNullOrEmpty(objvalue) && objvalue.Contains(";"))
                                    {
                                        RadComboBoxItem item = new RadComboBoxItem(objvalue.Split(";".ToCharArray())[0], objvalue.Split(";".ToCharArray())[1]);
                                        ((RadComboBox)(objChildControl)).Items.Add(item);
                                        ((RadComboBox)(objChildControl)).SelectedValue = item.Value;
                                        ((RadComboBox)(objChildControl)).Text = item.Text;
                                    }
                                }
                            }

                            #endregion RadComboBox Control

                            #region Hidden Control
                            //Check for TextBox controls in the list.
                            if(string.Compare(objChildControl.GetType().ToString(), objHiddenField.GetType().ToString()) == 0)
                            {
                                //if the Control ID matches then it will assign the value.
                                if(string.Compare(objChildControl.ID, strLabel) == 0)
                                {
                                    if(string.Compare(xmlnodeAttrGrp.FirstChild.Name, VALUE) == 0)
                                    {
                                        ((HiddenField)(objChildControl)).Value = xmlnodeAttrGrp.FirstChild.InnerText.ToString();
                                    }
                                    else
                                    {
                                        ((HiddenField)(objChildControl)).Value = xmlnodeAttrGrp.Attributes.GetNamedItem(VALUE).Value.ToString();
                                    }
                                }
                            }
                            #endregion Hidden Control

                        }
                    }
                }
            }
        }
        /// <summary>
        /// Binds the inner attribute group.
        /// </summary>
        /// <param name="saveSearchDoc">The save search doc.</param>
        /// <param name="SaveSearchXPATH">The save search XPATH.</param>
        private void BindInnerAttributeGroup(XmlDocument saveSearchDoc, string saveSearchXPATH)
        {
            XmlNodeList xmlnodelistAttrGrp = saveSearchDoc.SelectNodes(saveSearchXPATH);
            foreach(XmlNode xmlnodeAttrGrp in xmlnodelistAttrGrp)
            {
                string strLabel = string.Empty;
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
                            RadioButton objRadioButton = new RadioButton();
                            TextBox objTextBox = new TextBox();
                            ListBox objListBox = new ListBox();
                            // SRP Code
                            DropDownList objDropdownList = new DropDownList();
                            RadComboBox objRadComboBox = new RadComboBox();
                            RadioButtonList objRadioButtonList = new RadioButtonList();
                            HiddenField objHiddenField = new HiddenField();
                            if(objChildControl.ID != null)
                            {
                                //Checks whether the ID is Equal to trDates row.
                                if(string.Equals(objChildControl.ID.ToString(), "trDates"))
                                {
                                    foreach(Control objDateControl in objChildControl.Controls)
                                    {
                                        foreach(Control objDatesControl in objDateControl.Controls)
                                        {
                                            #region TextBox Control
                                            //Checks for control type as TextBox.
                                            if(string.Equals(objDatesControl.GetType().ToString(), objTextBox.GetType().ToString()))
                                            {
                                                if(string.Equals(objDatesControl.ID, strLabel))
                                                {
                                                    if(xmlnodeAttrGrp.HasChildNodes)
                                                    {
                                                        if(string.Equals(xmlnodeAttrGrp.FirstChild.Name, VALUE))
                                                        {
                                                            ((TextBox)(objDatesControl)).Text = xmlnodeAttrGrp.FirstChild.InnerText.ToString();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ((TextBox)(objDatesControl)).Text = xmlnodeAttrGrp.Attributes.GetNamedItem(VALUE).Value.ToString();
                                                    }
                                                }
                                            }
                                            #endregion TextBox Control
                                        }
                                    }
                                }
                            }
                            #region RadioButton Control
                            //Checks for control type as RadioButton.
                            if(string.Equals(objChildControl.GetType().ToString(), objRadioButton.GetType().ToString()))
                            {
                                if(string.Equals(objChildControl.ID, strLabel))
                                {
                                    ((RadioButton)(objChildControl)).Checked = true;
                                }
                            }
                            #endregion RadioButton Control

                            #region ListBox Control
                            //Check for ListBox type controls in the list.
                            if(string.Compare(objChildControl.GetType().ToString(), objListBox.GetType().ToString()) == 0)
                            {
                                if(string.Compare(objChildControl.ID, strLabel) == 0)
                                {
                                    ((ListBox)(objChildControl)).ClearSelection();
                                    XmlNodeList xmlnodelistValue = saveSearchDoc.SelectNodes(saveSearchXPATH + "/value");
                                    if(!string.Equals(xmlnodeAttrGrp.Attributes.GetNamedItem(CHECKED).Value.ToString(), FALSE))
                                    {
                                        //Loop through the items in the list.
                                        foreach(ListItem lstItems in ((ListBox)(objChildControl)).Items)
                                        {
                                            foreach(XmlNode xmlnodeListBoxItem in xmlnodelistValue)
                                            {
                                                //check for the item text and if the value matches then that item will be selected.
                                                if(string.Compare(xmlnodeListBoxItem.ParentNode.Attributes.GetNamedItem(LABEL).Value.ToString(), strLabel) == 0)
                                                {
                                                    if(string.Compare(lstItems.Value, xmlnodeListBoxItem.InnerText) == 0)
                                                    {
                                                        lstItems.Selected = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion ListBox Control

                            #region TextBox Control
                            //Checks for control type as TextBox.
                            if(string.Equals(objChildControl.GetType().ToString(), objTextBox.GetType().ToString()))
                            {
                                if(string.Equals(objChildControl.ID, strLabel))
                                {
                                    if(xmlnodeAttrGrp.HasChildNodes)
                                    {
                                        if(string.Equals(xmlnodeAttrGrp.FirstChild.Name, VALUE))
                                        {
                                            ((TextBox)(objChildControl)).Text = xmlnodeAttrGrp.FirstChild.InnerText.ToString();
                                        }
                                    }
                                    else
                                    {
                                        ((TextBox)(objChildControl)).Text = xmlnodeAttrGrp.Attributes.GetNamedItem(VALUE).Value.ToString();
                                    }

                                }
                            }
                            #endregion TextBox Control

                            #region DropDownList Control

                            //Check for Drop controls in the list.
                            if(string.Compare(objChildControl.GetType().ToString(), objDropdownList.GetType().ToString()) == 0)
                            {
                                //if the Control ID matches then it will assign the value.
                                if(string.Compare(objChildControl.ID, strLabel) == 0)
                                {
                                    string objvalue = string.Empty;

                                    if(string.Compare(xmlnodeAttrGrp.FirstChild.Name, VALUE) == 0)
                                    {
                                        objvalue = xmlnodeAttrGrp.FirstChild.InnerText.ToString();
                                    }
                                    else
                                    {
                                        objvalue = xmlnodeAttrGrp.Attributes.GetNamedItem(VALUE).Value.ToString();
                                    }
                                    //Loop through the items in the list.
                                    foreach(ListItem lstItem in ((DropDownList)(objChildControl)).Items)
                                    {
                                        //Added in DREAM 4.0 for changing country listbox to  ddl
                                        if(objChildControl.ID.Equals("lstCountry"))
                                        {
                                            ((DropDownList)(objChildControl)).SelectedValue = objvalue;
                                        }
                                        if(string.Compare(lstItem.Text, objvalue) == 0)
                                        {
                                            ///<TODO>
                                            /// Change this code by modifying as below
                                            /// Use FindByText and FindByValue function to find and set selected item
                                            /// </TODO>

                                            /// This if Condition for SRP Controls. The below dropdowns are having different value for "Text" and "Value".
                                            /// Selected Text is saved in "Saved Search". Should be set based on Text
                                            if(strLabel.Equals("cboReserveMagOil") || strLabel.Equals("cboReserveMagGas") || strLabel.Equals("cboTectonicSetting") || strLabel.Equals("cboTectonicSettingKle") || strLabel.Equals("cboHydrocarbonMain"))
                                            {
                                                ((DropDownList)(objChildControl)).ClearSelection();
                                                ((DropDownList)(objChildControl)).Items.FindByText(lstItem.Text).Selected = true;
                                            }
                                            else
                                            {
                                                ((DropDownList)(objChildControl)).SelectedValue = lstItem.Text;
                                            }
                                        }

                                    }
                                }
                            }
                            #endregion DropDownList Control

                            #region RadioButtonList Control
                            //check for readio button list
                            if(string.Compare(objChildControl.GetType().ToString(), objRadioButtonList.GetType().ToString()) == 0)
                            {
                                //if the Control ID matches then it will assign the value.
                                if(string.Compare(objChildControl.ID, strLabel) == 0)
                                {
                                    string objvalue = string.Empty;

                                    if(string.Compare(xmlnodeAttrGrp.FirstChild.Name, VALUE) == 0)
                                    {
                                        objvalue = xmlnodeAttrGrp.FirstChild.InnerText.ToString();
                                    }
                                    else
                                    {
                                        objvalue = xmlnodeAttrGrp.Attributes.GetNamedItem(VALUE).Value.ToString();
                                    }
                                    //Loop through the items in the list.
                                    foreach(ListItem lstItem in ((RadioButtonList)(objChildControl)).Items)
                                    {
                                        if(string.Compare(lstItem.Value, objvalue) == 0)
                                        {
                                            ((RadioButtonList)(objChildControl)).SelectedValue = lstItem.Value;
                                        }

                                    }
                                }
                            }
                            #endregion RadioButtonList Control

                            #region RadComboBox Control
                            //Check for RadComboBox in the list.
                            if(string.Compare(objChildControl.GetType().ToString(), objRadComboBox.GetType().ToString()) == 0)
                            {
                                //if the Control ID matches then it will assign the value.
                                if(string.Compare(objChildControl.ID, strLabel) == 0)
                                {
                                    string objvalue = string.Empty;

                                    if(string.Compare(xmlnodeAttrGrp.FirstChild.Name, VALUE) == 0)
                                    {
                                        objvalue = xmlnodeAttrGrp.FirstChild.InnerText.ToString();
                                    }
                                    else
                                    {
                                        objvalue = xmlnodeAttrGrp.Attributes.GetNamedItem(VALUE).Value.ToString();
                                    }
                                    if(!string.IsNullOrEmpty(objvalue) && objvalue.Contains(";"))
                                    {

                                        RadComboBoxItem item = new RadComboBoxItem(objvalue.Split(";".ToCharArray())[0], objvalue.Split(";".ToCharArray())[1]);


                                        ((RadComboBox)(objChildControl)).Items.Add(item);

                                        ((RadComboBox)(objChildControl)).SelectedValue = item.Value;
                                        ((RadComboBox)(objChildControl)).Text = item.Text;

                                    }
                                    else
                                    {
                                        RadComboBoxItem item = new RadComboBoxItem(objvalue);


                                        ((RadComboBox)(objChildControl)).Items.Add(item);
                                        if(((RadComboBox)(objChildControl)).FindItemByText(objvalue) != null)
                                        {
                                            ((RadComboBox)(objChildControl)).FindItemByText(objvalue).Selected = true;
                                        }
                                        ((RadComboBox)(objChildControl)).Text = item.Text;
                                    }

                                }
                            }
                            #endregion RadComboBox Control

                            #region Hidden Control
                            //Checks for control type as TextBox.
                            if(string.Equals(objChildControl.GetType().ToString(), objHiddenField.GetType().ToString()))
                            {
                                if(string.Equals(objChildControl.ID, strLabel))
                                {
                                    if(xmlnodeAttrGrp.HasChildNodes)
                                    {
                                        if(string.Equals(xmlnodeAttrGrp.FirstChild.Name, VALUE))
                                        {
                                            ((HiddenField)(objChildControl)).Value = xmlnodeAttrGrp.FirstChild.InnerText.ToString();
                                        }
                                    }
                                    else
                                    {
                                        ((HiddenField)(objChildControl)).Value = xmlnodeAttrGrp.Attributes.GetNamedItem(VALUE).Value.ToString();
                                    }

                                }
                            }
                            #endregion Hidden Control
                        }
                    }
                }
                //Checks whether the Xml node has some more Child nodes or not.
                if(xmlnodeAttrGrp.HasChildNodes)
                {
                    if(string.Equals(xmlnodeAttrGrp.FirstChild.Name.ToString(), PARAMETER))
                    {
                        saveSearchXPATH = saveSearchXPATH.ToString() + PARAMETERXPATH;
                        BindInnerAttributeGroup(saveSearchDoc, saveSearchXPATH);
                    }
                }
            }
        }
        /// <summary>
        /// Gets date selected from calender control by user 
        /// </summary>
        /// <param name="textBox">textBox ID</param>
        /// <returns>DateTime object</returns>
        protected DateTime GetDateSelectedByCalender(string date)
        {
            string[] arrDate = null;
            DateTime dteSelected = DateTime.Today;
            if(!string.IsNullOrEmpty(date))
            {
                if(date.Contains("-"))
                {
                    arrDate = date.Split(new char[] { '-' });
                    dteSelected = new DateTime(int.Parse(arrDate[2]), int.Parse(arrDate[0]), int.Parse(arrDate[1]));
                }

            }
            return dteSelected;
        }

        /// <summary>
        ///  Query the XSL Template from the sharepoint library.
        /// </summary>
        protected XmlTextReader GetXslForRadResponseXml()
        {
            MOSSServiceManager objMOSSController = (MOSSServiceManager)objFactory.GetServiceManager(MOSSSERVICE);
            XmlTextReader objXmlTextReader = ((MOSSServiceManager)objMOSSController).GetXSLTemplate(SRPXSLFILENAME, strCurrSiteUrl);
            return objXmlTextReader;
        }

        #region SRP

        #endregion
        #endregion Bind Controls
        #region DREAM 4.0
        /// <summary>
        /// Creates the DDL assets.
        /// </summary>
        protected void CreateDDLAssets(Control ddlContainer)
        {
            ddlAssets = new DropDownList();
            ddlAssets.ID = "ddlAssets";
            ddlAssets.EnableViewState = true;
            //this.Controls.Add(ddlAssets);
            ddlContainer.Controls.Add(ddlAssets);
            objUtility.PopulateAssetListControl(ddlAssets, objUtility.GetPipeSeperatedStrAsArray(strSelectedAssetNames), objUtility.GetPipeSeperatedStrAsArray(strSelectedRows));
        }
        /// <summary>
        /// Adds the identifier attribute.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        protected void AddIdentifierAttribute(ArrayList attributes)
        {
            attributes.Add(AddAttribute(strSelectedCriteriaName, EQUALSOPERATOR, new string[] { ddlAssets.SelectedValue }));
        }
        #endregion
    }
}
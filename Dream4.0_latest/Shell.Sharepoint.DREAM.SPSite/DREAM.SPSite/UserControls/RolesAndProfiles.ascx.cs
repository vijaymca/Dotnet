#region Shell Copyright.2011
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: RolesAndProfiles.ascx.cs
#endregion
/// <summary> 
/// This is roles profiles usercontrol cs class.
/// </summary>
using System;
using System.Xml;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Services.Protocols;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;
using Microsoft.SharePoint;
using Telerik.Web.UI;
namespace Shell.SharePoint.DREAM.Site.UI.UserControls
{
    /// <summary>
    /// This is roles profiles usercontrol code behind class.It is calling roles profiles helper
    /// class functions to provide roles profiles functionality
    /// </summary>
    public partial class RolesAndProfiles :UIControlHandler
    {
        #region DECLARATION
        RolesProfilesHelper objRolesProfilesHelper;
        Reorder objReorder;
        const string REORDERDOCLIB = "Reorder XML";
        const string CONTEXTSEARCHDOCLIB = "Context Search XML";
        const string TITLE = "Title";
        new const string VALUE = "Value";
        const string ROLESLIST = "Roles";
        const string PROFILETYPELIST = "ProfileType";
        const string ASSETTYPELIST = "Asset Type";
        const string CONTEXTSEARCHLIST = "Context Search";
        const string SEARCHRESULTS = "SearchResults";
        const string CONTEXTSEARCH = "ContextSearch";
        
        #endregion

        #region Protected method
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                base.EnsureChildControls();
                objRolesProfilesHelper = new RolesProfilesHelper();
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                objReportController = objFactory.GetServiceManager(REPORTSERVICE);
                if(!IsPostBack)
                {
                    string strCamlQuery = string.Empty;
                    string strViewFields = string.Empty;
                    objUtility.RenderAjaxBusyBox(this.Page); //rendering busy box.

                    //Populating Roles dropdownlistbox
                    strCamlQuery = "<Query><Where><Eq><FieldRef Name=\"Active\" /><Value Type=\"Boolean\">1</Value></Eq></Where></Query><OrderBy><FieldRef Name='Title' Ascending='True' /></OrderBy>";
                    strViewFields = @"<FieldRef Name='Title'/><FieldRef Name='Value'/>";
                    objRolesProfilesHelper.LoadDropdownLstBxFromSPList(ddlRole, ROLESLIST, TITLE, VALUE, strCamlQuery, strViewFields);
                    //Populating Profile Type dropdownlistbox
                    strCamlQuery = "<Where><Eq><FieldRef Name=\"Active\" /><Value Type=\"Boolean\">1</Value></Eq></Where><OrderBy><FieldRef Name='Title' Ascending='True' /></OrderBy>";
                    strViewFields = @"<FieldRef Name='Title'/><FieldRef Name='Value'/>";
                    objRolesProfilesHelper.LoadDropdownLstBxFromSPList(ddlProfileType, PROFILETYPELIST, TITLE, VALUE, strCamlQuery, strViewFields);
                    //Populating AssetType dropdownlistbox
                    strCamlQuery = "<Where><Eq><FieldRef Name=\"IsActiveReportService\" /><Value Type=\"Boolean\">1</Value></Eq></Where><OrderBy><FieldRef Name='Title' Ascending='True' /></OrderBy>";
                    strViewFields = @"<FieldRef Name='Title'/>";
                    objRolesProfilesHelper.LoadDropdownLstBxFromSPList(ddlAssetType, ASSETTYPELIST, TITLE, TITLE, strCamlQuery, strViewFields);
                    trProfileType.Visible = false;
                    trAssetType.Visible = false;
                    trResultType.Visible = false;
                    trCategory.Visible = false;
                    trAddRemoveItems.Visible = false;
                    ShowHideAddRemove(false);
                }
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
            finally
            {
                objUtility.CloseAjaxBusyBox(this.Page); //rendering busy box.
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlRole control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(!ddlRole.SelectedValue.Contains(DEFAULTDROPDOWNTEXT))
                {
                    trProfileType.Visible = true;
                    btnDeleteProfile.Visible = false;
                    ddlProfileType.Text = DEFAULTDROPDOWNTEXT;
                    trAssetType.Visible = false;
                    trResultType.Visible = false;
                    trCategory.Visible = false;
                    trAddRemoveItems.Visible = false;
                    ShowHideAddRemove(false);
                }
                else
                {
                    trProfileType.Visible = false;
                    trAssetType.Visible = false;
                    trResultType.Visible = false;
                    trCategory.Visible = false;
                    trAddRemoveItems.Visible = false;
                    ShowHideAddRemove(false);
                }
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlProfileType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ddlProfileType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(!ddlProfileType.SelectedValue.Contains(DEFAULTDROPDOWNTEXT) && ((ddlProfileType.SelectedValue.Contains(SEARCHRESULTS))))
                {
                    trAssetType.Visible = true;
                    ddlAssetType.Text = DEFAULTDROPDOWNTEXT;
                    trResultType.Visible = false;
                    trCategory.Visible = false;
                    trAddRemoveItems.Visible = false;
                    ShowHideAddRemove(false);
                }
                else if(!ddlProfileType.SelectedValue.Contains(DEFAULTDROPDOWNTEXT) && ((ddlProfileType.SelectedValue.Contains(CONTEXTSEARCH))))
                {
                    trAssetType.Visible = true;
                    ddlAssetType.Text = DEFAULTDROPDOWNTEXT;
                    trResultType.Visible = false;
                    trCategory.Visible = false;
                    trAddRemoveItems.Visible = false;
                    ShowHideAddRemove(false);
                }
                else
                {
                    trAssetType.Visible = false;
                    trResultType.Visible = false;
                    trCategory.Visible = false;
                    trAddRemoveItems.Visible = false;
                    ShowHideAddRemove(false);
                }
                ShowHideDeleteButton();

            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }
        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlAssetType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ddlAssetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(!ddlAssetType.SelectedValue.Contains(DEFAULTDROPDOWNTEXT) && ((ddlProfileType.SelectedValue.Contains(SEARCHRESULTS))))
                {
                    objRolesProfilesHelper.LoadSearcheNames(ddlAssetType.SelectedValue, ddlSearchNames);
                    trResultType.Visible = true;
                    ddlSearchNames.Text = DEFAULTDROPDOWNTEXT;
                    trCategory.Visible = false;
                    trAddRemoveItems.Visible = false;
                    ShowHideAddRemove(false);
                }
                else if(!ddlAssetType.SelectedValue.Contains(DEFAULTDROPDOWNTEXT) && ((ddlProfileType.SelectedValue.Contains(CONTEXTSEARCH))))
                {
                    trResultType.Visible = false;
                    LoadCategory(radLstBxCategory, ddlAssetType.SelectedValue);
                    trCategory.Visible = true;
                    trAddRemoveItems.Visible = false;
                    ShowHideAddRemove(false);
                }
                else
                {
                    trCategory.Visible = false;
                    trResultType.Visible = false;
                    trAddRemoveItems.Visible = false;
                    ShowHideAddRemove(false);
                }
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }

        }
        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlResultType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ddlSearchNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(!ddlSearchNames.SelectedValue.Contains(DEFAULTDROPDOWNTEXT))
                {
                    XmlDocument xmlDocResponse = null;
                    xmlDocResponse = objReportController.GetSearchResults(objRolesProfilesHelper.CreateRequestInfo(ddlSearchNames.SelectedValue), -1, "RolesProfiles", string.Empty, 0);
                    xmlDocResponse = new XmlDocument();
                    //****Added for testing ,should be removed later when webservices are ready
                    // xmlDocResponse.Load(@"C:\Sonia \ColumnResponse.xml");
                    xmlDocResponse.Load(@"C:\Dev Folder\DREAM4.0\Modules\Roles & Profiles\XML\ColumnResponse.xml");
                    LoadListOfColumn(radLstBxSource, radLstBxDestination, xmlDocResponse);
                    trCategory.Visible = false;
                    trAddRemoveItems.Visible = false;
                    ShowHideAddRemove(true);
                }
                else
                {
                    trAddRemoveItemsHeader.Visible = false;
                    trAddRemoveItems.Visible = false;
                    ShowHideAddRemove(false);
                }
            }
            catch(SoapException soapEx)
            {
                if(!string.Equals(soapEx.Message.ToString(), SOAPEXCEPTIONMESSAGE))
                {
                    CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                }
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }
        /// <summary>
        /// Handles the SelectedIndexChanged event of the radLstBxCategory control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void radLstBxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadListOfContextSearch(radLstBxSource, radLstBxDestination);
                trResultType.Visible = false;
                trAddRemoveItems.Visible = true;
                ShowHideAddRemove(true);
                trAddRemoveItemsHeader.Visible = false;
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }
        /// <summary>
        /// Handles the btnclick event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnSave_btnclick(object sender, EventArgs e)
        {
            try
            {
                if(ddlProfileType.SelectedValue == SEARCHRESULTS)
                {
                    SaveColumnOrder();
                }
                else
                {
                    SaveContextSearch();
                }
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }
        /// <summary>
        /// Handles the btnclick event of the btnDeleteProfile control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnDeleteProfile_btnclick(object sender, EventArgs e)
        {
            try
            {
                string strDocLibName = string.Empty;
                if(ddlProfileType.SelectedValue.Contains(SEARCHRESULTS))
                {
                    strDocLibName = REORDERDOCLIB;
                }
                else if((ddlProfileType.SelectedValue.Contains(CONTEXTSEARCH)))
                {
                    strDocLibName = CONTEXTSEARCHDOCLIB;
                }
                if(((MOSSServiceManager)objMossController).IsDocLibFileExist(strDocLibName, ddlRole.SelectedValue))
                {
                    ((MOSSServiceManager)objMossController).DeleteXMLFile(SPContext.Current.Site.Url, strDocLibName, ddlRole.SelectedValue);
                }
                ddlProfileType_SelectedIndexChanged(null, EventArgs.Empty);
                btnDeleteProfile.Visible = false;
                //  ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "DeleteMessageScript", "<script language=\"javascript\">try{alert('Profile deleted successfully.')();}catch(Ex){}</script>", false);
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }
        #endregion

        #region Private method
        /// <summary>
        /// Shows the hide add remove.
        /// </summary>
        /// <param name="display">if set to <c>true</c> [display].</param>
        private void ShowHideAddRemove(bool display)
        {
            trAddRemoveItemsButton.Visible = display;
            trAddRemoveItemsHeader.Visible = display;
            radLstBxSource.Visible = display;
            radLstBxDestination.Visible = display;
        }
        /// <summary>
        /// Saves the column order.
        /// </summary>
        private void SaveColumnOrder()
        {
            string strColOrderStatus = string.Empty;
            strColOrderStatus = objRolesProfilesHelper.GetColumnOrder(radLstBxSource, radLstBxDestination, "column");
            objReorder = new Reorder();
            objReorder.SaveReorderXml(strColOrderStatus, ddlSearchNames.SelectedValue, ddlAssetType.SelectedValue, ddlRole.SelectedValue);
        }
        /// <summary>
        /// Saves the context search.
        /// </summary>
        private void SaveContextSearch()
        {
            string strColOrderStatus = string.Empty;
            strColOrderStatus = objRolesProfilesHelper.GetColumnOrder(radLstBxSource, radLstBxDestination, CONTEXTSEARCH.ToLowerInvariant());
            objReorder = new Reorder();
            objReorder.SaveGroupHeaderOrder(strColOrderStatus, radLstBxCategory.SelectedValue, GetItemAsStringArray(radLstBxCategory.Items), ddlAssetType.SelectedValue, ddlRole.SelectedValue);
        }
        /// <summary>
        /// Gets the item as string array.
        /// </summary>
        /// <param name="itemCollection">The item collection.</param>
        /// <returns></returns>
        private string[] GetItemAsStringArray(RadListBoxItemCollection itemCollection)
        {
            string[] arrStr = new string[itemCollection.Count];
            for(int intCounter = 0; intCounter < itemCollection.Count; intCounter++)
            {
                arrStr[intCounter] = itemCollection[intCounter].Value;
            }
            return arrStr;
        }
        /// <summary>
        /// Loads the list box.
        /// </summary>
        /// <param name="objRadLstBxSource">The obj RAD LST bx source.</param>
        /// <param name="objRadLstBxDestination">The obj RAD LST bx destination.</param>
        /// <param name="reponseXml">The reponse XML.</param>
        private void LoadListOfColumn(RadListBox objRadLstBxSource, RadListBox objRadLstBxDestination, XmlDocument reponseXml)
        {
            if(reponseXml != null)
            {
                Reorder objReorder = new Reorder();
                XmlNodeList xmlNdLstSourceCol = null;
                XmlNodeList xmlNdLstDestinationCol = null;
                RadListBoxItem radLstBxItem = null;
                objReorder.SetSourceNDestinationItemList(ddlRole.SelectedValue, ddlSearchNames.SelectedValue, ddlAssetType.SelectedValue, reponseXml, out xmlNdLstSourceCol, out xmlNdLstDestinationCol);

                objRadLstBxSource.Items.Clear();
                if(xmlNdLstSourceCol != null && xmlNdLstSourceCol.Count > 0)
                {
                    objRadLstBxSource.DataSource = xmlNdLstSourceCol;
                    objRadLstBxSource.DataTextField = "value";
                    objRadLstBxSource.DataValueField = "value";
                    objRadLstBxSource.DataBind();
                }

                objRadLstBxDestination.Items.Clear();
                if(xmlNdLstDestinationCol != null && xmlNdLstDestinationCol.Count > 0)
                {
                    foreach(XmlNode xmlNodeCol in xmlNdLstDestinationCol)
                    {
                        radLstBxItem = new RadListBoxItem(xmlNodeCol.Attributes["name"].Value);
                        objRadLstBxDestination.Items.Add(radLstBxItem);
                        if(xmlNodeCol.Attributes["mandatory"].Value.ToLowerInvariant().Equals("true"))
                        {
                            radLstBxItem.Enabled = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Loads the list box.
        /// </summary>
        /// <param name="objRadLstBxSource">The obj RAD LST bx source.</param>
        /// <param name="objRadLstBxDestination">The obj RAD LST bx destination.</param>
        private void LoadListOfContextSearch(RadListBox objRadLstBxSource, RadListBox objRadLstBxDestination)
        {
            Reorder objReorder = new Reorder();
            XmlNodeList xmlNdLstSourceCol = null;
            XmlNodeList xmlNdLstDestinationCol = null;
            objReorder.SetSrcNDestNdLstForContextSearch(ddlRole.SelectedValue, radLstBxCategory.SelectedValue, ddlAssetType.SelectedValue, out xmlNdLstSourceCol, out xmlNdLstDestinationCol);

            objRadLstBxSource.Items.Clear();
            if(xmlNdLstSourceCol != null && xmlNdLstSourceCol.Count > 0)
            {
                objRadLstBxSource.DataSource = xmlNdLstSourceCol;
                objRadLstBxSource.DataTextField = "value";
                objRadLstBxSource.DataValueField = "value";
                objRadLstBxSource.DataBind();
            }

            objRadLstBxDestination.Items.Clear();
            if(xmlNdLstDestinationCol != null && xmlNdLstDestinationCol.Count > 0)
            {
                objRadLstBxDestination.DataSource = xmlNdLstDestinationCol;
                objRadLstBxDestination.DataTextField = "value";
                objRadLstBxDestination.DataValueField = "value";
                objRadLstBxDestination.DataBind();
            }

        }

        /// <summary>
        /// Loads the category.
        /// </summary>
        /// <param name="radListBx">The RAD list bx.</param>
        /// <param name="assetType">Type of the asset.</param>
        private void LoadCategory(RadListBox radListBx, string assetType)
        {
            Reorder objReorder = new Reorder();
            XmlNodeList xmlNdLstSourceCol = null;
            xmlNdLstSourceCol = objReorder.GetNdLstForCategory(ddlRole.SelectedValue, assetType);
            radListBx.Items.Clear();
            if(xmlNdLstSourceCol != null && xmlNdLstSourceCol.Count > 0)
            {
                radListBx.DataSource = xmlNdLstSourceCol;
                radListBx.DataTextField = "value";
                radListBx.DataValueField = "value";
                radListBx.DataBind();
            }
        }
        /// <summary>
        /// Shows the hide delete button.
        /// </summary>
        private void ShowHideDeleteButton()
        {
            string strDocLibName = string.Empty;
            if(ddlProfileType.SelectedValue.Contains(SEARCHRESULTS))
            {
                strDocLibName = REORDERDOCLIB;
            }
            else if((ddlProfileType.SelectedValue.Contains(CONTEXTSEARCH)))
            {
                strDocLibName = CONTEXTSEARCHDOCLIB;
            }

            if(!ddlProfileType.SelectedValue.Contains(DEFAULTDROPDOWNTEXT) && (((MOSSServiceManager)objMossController).IsDocLibFileExist(strDocLibName, ddlRole.SelectedValue)))
            {
                btnDeleteProfile.Visible = true;

            }
            else
            {
                btnDeleteProfile.Visible = false;
            }
        }
        #endregion

    }
}
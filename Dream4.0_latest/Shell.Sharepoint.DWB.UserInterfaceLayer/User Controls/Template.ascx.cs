#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : Template.cs
#endregion

using System;
using System.Collections;
using System.Data;
using System.Net;
using System.Web;
using System.Web.UI.WebControls;

using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DWB.DualListControl;
using Shell.SharePoint.DREAM.Utilities;


namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// Code Behind class for Template user control
    /// Provides UI for Add/Edit Template
    /// </summary>
    public partial class Template : UIHelper
    {
        #region DECLARATION
        const string TEMPLATETITLEEXISTSMSG = "Template with this title already exist. Please enter a different title.";
        const string MAINTAINTEMPLATEURL = "/Pages/TemplateMaintenance.aspx";       
        const string NEWTEMPLATE = "New Template";
        const string EDITTEMPLATE = "Edit Template";

        ListEntry objListEntry ;
        string strSelectedID = string.Empty;
        string strMode = string.Empty;      
        #endregion

        #region Protected Methods
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ListEntry objTemplateData = null;
            strMode = HttpContext.Current.Request.QueryString[MODEQUERYSTRING];
            strSelectedID = HttpContext.Current.Request.QueryString[IDVALUEQUERYSTRING];
            try
            {
                if (!IsPostBack)
                {
                    LoadControlsOnPageLoad();

                    if (string.Equals(strMode, EDIT) || (string.Equals(strMode, VIEW)))
                    {
                        objTemplateData = GetDetailsForSelectedID(strSelectedID, TEMPLATELIST, TEMPLATE);

                        if (objTemplateData != null)
                        {
                            hdnTemplateID.Value = objTemplateData.TemplateDetails.RowId.ToString();
                            hdnTerminated.Value = objTemplateData.TemplateDetails.Terminated;
                            BindUIControls(objTemplateData);
                            SetUIControlForMode(objTemplateData.TemplateDetails);
                        }
                    }

                    Page.ClientScript.RegisterStartupScript(this.Page.GetType(), SETWINDOWTITLEJSKEY, string.Format(SETWINDOWTITLEVALUE, lblTemplateHeading.Text), true);
                }
            }
            catch (WebException webEx)
            {
                lblException.Text = webEx.Message;
                lblException.Visible = true;
                ExceptionBlock.Visible = true;
                CommonUtility.HandleException(strParentSiteURL, webEx);
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
            Response.Redirect(MAINTAINTEMPLATEURL, false);
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
                if (string.Equals(HttpContext.Current.Request.QueryString[MODEQUERYSTRING].ToString(), ADD))
                {
                    if (!CheckDuplicateName(txtTemplateTitle.Text.Trim(), DWBTITLECOLUMN, TEMPLATELIST))
                    {
                        SetListEntry();
                        UpdateListEntry(objListEntry, TEMPLATELIST, TEMPLATEAUDITLIST, TEMPLATE,
                            AUDITACTIONCREATION);
                        /// Update the MasterPages from the selected Template
                        if (cboTemplates != null && cboTemplates.SelectedIndex > 0)
                        {
                            int intselectedTemplateID = Int32.Parse(cboTemplates.SelectedValue);

                            ArrayList arLstmasterPages = SetTemplateConfiguration(intselectedTemplateID);

                            objListEntry.TemplateConfiguration = arLstmasterPages;
                            /// Update the DWB Template Pages Mapping List for the pages in choosen template
                            UpdateListEntry(objListEntry, TEMPLATEPAGESMAPPINGLIST, TEMPLATECONFIGURATIONAUDIT, TEMPLATEPAGEMAPPING, AUDITACTIONCREATION);

                            /// Update the DWB Master Pages list for the MasterPages referenced in the new template
                            UpdateListEntry(objListEntry, MASTERPAGELIST, MASTERPAGEAUDITLIST, MASTERPAGETEMPLATEMAPPING, AUDITACTIONUPDATION);
                        }


                        Response.Redirect(MAINTAINTEMPLATEURL, false);
                    }
                    else
                    {
                        lblException.Text = TEMPLATETITLEEXISTSMSG;
                        lblException.Visible = true;
                        ExceptionBlock.Visible = true;
                    }
                }

                else if (string.Equals(HttpContext.Current.Request.QueryString[MODEQUERYSTRING], EDIT))
                {
                    if (!CheckDuplicateName(txtTemplateTitle.Text.Trim(), DWBTITLECOLUMN, TEMPLATELIST, strSelectedID))
                    {
                        SetListEntry();
                        UpdateListEntry(objListEntry, TEMPLATELIST, TEMPLATEAUDITLIST, TEMPLATE, AUDITACTIONUPDATION);
                        Response.Redirect(MAINTAINTEMPLATEURL, false);
                    }
                    else
                    {
                        lblException.Text = TEMPLATETITLEEXISTSMSG;
                        lblException.Visible = true;
                        ExceptionBlock.Visible = true;
                    }
                }
            }
            catch (WebException webEx)
            {
                lblException.Text = webEx.Message;
                lblException.Visible = true;
                ExceptionBlock.Visible = true;
                CommonUtility.HandleException(strParentSiteURL, webEx);
            }
            catch (Exception ex)
            {

                CommonUtility.HandleException(strParentSiteURL, ex);

            }
        }

        /// <summary>
        /// On SelectedIndex change event of Asset Type dropdown,
        /// cboTemplates will be populated with the Templates for the selected Asset Type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cboAssetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strCAMLQuery = string.Empty;
            try
            {
                strCAMLQuery = @"<Where><And><Eq><FieldRef Name='Has_MasterPage' /><Value Type='Choice'>Yes</Value></Eq><Eq><FieldRef Name='Asset_Type' /><Value Type='Lookup'>" + cboAssetType.SelectedItem.Text + "</Value></Eq></And></Where>";
                string strViewFields = "<FieldRef Name='Title' /><FieldRef Name = 'ID'/><FieldRef Name = 'Asset_Type' /><FieldRef Name='Has_MasterPage' />";
                SetListValues(cboTemplates, TEMPLATELIST, strCAMLQuery, strViewFields);
            }
            catch (WebException webEx)
            {
                lblException.Text = webEx.Message;
                lblException.Visible = true;
                ExceptionBlock.Visible = true;
                CommonUtility.HandleException(strParentSiteURL, webEx);
            }
            catch (Exception ex)
            {

                CommonUtility.HandleException(strParentSiteURL, ex);

            }
        }
        #endregion Protected Methods

        #region Private Methods
        /// <summary>
        /// Binds the UI controls.
        /// </summary>
        /// <param name="objMasterData">The obj master data.</param>
        private void BindUIControls(ListEntry objTemplateData)
        {
            txtTemplateTitle.Text = objTemplateData.TemplateDetails.Title;

            BindDropDownList(cboAssetType, objTemplateData.TemplateDetails.AssetType);
            cboAssetType.Enabled = false;
            cboTemplates.Visible = false;
            lblCopyMasterPage.Visible = false;
        }

        /// <summary>
        /// Binds the drop down list.
        /// </summary>
        /// <param name="dropDown">The drop down.</param>
        /// <param name="value">The value.</param>
        private void BindDropDownList(DropDownList ddlControl, string value)
        {
            for (int intIndex = 0; intIndex < ddlControl.Items.Count; intIndex++)
            {
                if (string.Equals(ddlControl.Items[intIndex].Text.ToString(), value))
                {
                    ddlControl.ClearSelection();
                    ddlControl.Items[intIndex].Selected = true;
                    break;
                }
            }
        }

        /// <summary>
        /// Loads the controls on page load.
        /// </summary>
        private void LoadControlsOnPageLoad()
        {
            SetListValues(cboAssetType, ASSETTYPELIST);
            cboAssetType.SelectedIndex = 0;

            string strCAMLQuery = string.Empty;
            strCAMLQuery = "<Where><And><Eq><FieldRef Name='Has_MasterPage' /><Value Type='Choice'>Yes</Value></Eq><Eq><FieldRef Name='Asset_Type' /><Value Type='Lookup'>" + cboAssetType.SelectedItem.Text + "</Value></Eq></And></Where>";
            string strViewFields = "<FieldRef Name='Title' /><FieldRef Name = 'ID'/><FieldRef Name = 'Asset_Type' /><FieldRef Name='Has_MasterPage' />";
            SetListValues(cboTemplates, TEMPLATELIST, strCAMLQuery, strViewFields);
        }

        /// <summary>
        /// Hides controls based on the mode.
        /// </summary>
        private void SetUIControlForMode(TemplateDetails objTemplateDetails)
        {
            switch (strMode)
            {
                case ADD:
                    {
                        this.Page.Title = NEWTEMPLATE;
                        lblTemplateHeading.Text = NEWTEMPLATE;
                        break;
                    }
                case EDIT:
                    {
                        string strEditTemplate = EDITTEMPLATE + " :";

                        if (objTemplateDetails != null)
                        {
                            lblTemplateHeading.Text = strEditTemplate;
                            lblTemplateTitle.Text = objTemplateDetails.Title;
                        }
                        else
                        {
                            lblTemplateHeading.Text = strEditTemplate;
                        }
                        this.Page.Title = EDITTEMPLATE;
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
            try
            {
                objListEntry = new ListEntry();
                objListEntry.TemplateDetails = SetTemplateDetails();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Sets the Template details data object from the UI.
        /// </summary>
        /// <returns>TemplateDetails</returns>
        private TemplateDetails SetTemplateDetails()
        {
            TemplateDetails objTemplateDetails = new TemplateDetails();
            objTemplateDetails.Title = txtTemplateTitle.Text.Trim();
            objTemplateDetails.AssetType = cboAssetType.SelectedItem.Text;
            objTemplateDetails.AssetID = cboAssetType.SelectedItem.Value;
            if (string.Equals(ADD, strMode))
            {
                objTemplateDetails.Terminated = STATUSACTIVE;
            }
            else if (string.Equals(EDIT, strMode))
            {
                if (!string.IsNullOrEmpty(hdnTemplateID.Value))
                {
                    objTemplateDetails.RowId = Int32.Parse(hdnTemplateID.Value);
                }
                if (!string.IsNullOrEmpty(hdnTerminated.Value))
                {
                    objTemplateDetails.Terminated = hdnTerminated.Value;
                }
            }
            else
            {
                objTemplateDetails.Terminated =STATUSTERMINATED;
            }

            return objTemplateDetails;
        }

        /// <summary>
        /// To set the properties of the master page from the Selected template
        /// </summary>
        /// <param name="selectedtemplateID">ID of the template selected</param>
        /// <returns>Array list of master pages in the selected template</returns>
        private ArrayList SetTemplateConfiguration(int selectedtemplateID)
        {

            int[] masterPageID = null;

            ArrayList arlTemplateConfiguaration = null;
            DataTable dtListDetails = null;
            /// Title column is renamed to Template_Name - internal name remains as Title
            /// This query fetch ID of the new Tempalte created
            string strCAMLQuery = @"<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>" + txtTemplateTitle.Text.Trim() + "</Value></Eq></Where>";

            string strfieldsToView = "<FieldRef Name='ID' />";
            string strNewTemplateID = string.Empty;
            dtListDetails = GetListItems(TEMPLATELIST, strCAMLQuery, strfieldsToView);

            try
            {
                if (dtListDetails != null && dtListDetails.Rows.Count > 0)
                {
                    strNewTemplateID = dtListDetails.Rows[0][DWBIDCOLUMN].ToString();
                }
                if (!string.IsNullOrEmpty(strNewTemplateID))
                {
                    objListEntry.TemplateDetails.RowId = Int32.Parse(strNewTemplateID);
                }               
                masterPageID = GetMasterPageIDForTemplate(selectedtemplateID, TEMPLATEPAGESMAPPINGLIST);
                if (masterPageID != null && masterPageID.Length > 0)
                {
                    arlTemplateConfiguaration = new ArrayList();
                    for (int index = 0; index < masterPageID.Length; index++)
                    {
                        strCAMLQuery = string.Empty;
                        strCAMLQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + masterPageID[index].ToString() + "</Value></Eq></Where>";
                        strfieldsToView = string.Empty;
                        /// Title column is renamed to MasterPageName - internal name remains as Title
                        strfieldsToView = "<FieldRef Name='Title' /><FieldRef Name='Title_Template' /><FieldRef Name='Page_Owner' /><FieldRef Name='Sign_Off_Discipline' /><FieldRef Name='Connection_Type' /><FieldRef Name='ID' /><FieldRef Name='Page_Sequence' /><FieldRef Name='Asset_Type' /><FieldRef Name='Page_URL' /><FieldRef Name='Standard_Operating_Procedure' /><FieldRef Name='Page_Sequence' /><FieldRef Name='Page_Sequence' />";
                        DataTable dtresult = GetListItems(MASTERPAGELIST, strCAMLQuery, strfieldsToView);
                        if (dtresult != null && dtresult.Rows.Count > 0)
                        {
                            TemplateConfiguration objTemplateConfiguration = null;

                            foreach (DataRow objdataRow in dtresult.Rows)
                            {
                                 objTemplateConfiguration = new TemplateConfiguration();

                                objTemplateConfiguration.MasterPageTitle = objdataRow[DWBTITLECOLUMN].ToString();
                                objTemplateConfiguration.TemplateTitle = objdataRow["Title_Template"].ToString();
                                objTemplateConfiguration.PageOwner = objdataRow["Page_Owner"].ToString();
                                objTemplateConfiguration.Discipline = objdataRow["Sign_Off_Discipline"].ToString();
                                objTemplateConfiguration.ConnectionType = objdataRow["Connection_Type"].ToString();
                                objTemplateConfiguration.LinkedMasterPageId = objdataRow[DWBIDCOLUMN].ToString();
                                objTemplateConfiguration.PageSequence = Int32.Parse(objdataRow["Page_Sequence"].ToString());
                                objTemplateConfiguration.AssetType = objdataRow["Asset_Type"].ToString();
                                objTemplateConfiguration.PageURL = objdataRow["Page_URL"].ToString();
                                objTemplateConfiguration.StandardOperatingProcedure = objdataRow["Standard_Operating_Procedure"].ToString();
                                objTemplateConfiguration.TemplateID = strNewTemplateID;
                                arlTemplateConfiguaration.Add(objTemplateConfiguration);
                            }
                            dtresult.Dispose();
                        }
                    }
                }
            }
            finally
            {
                if (dtListDetails != null)
                {
                    dtListDetails.Dispose();
                }
            }
            return arlTemplateConfiguaration;
        }
        #endregion Private Methods
    }
}
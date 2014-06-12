#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : TemplatePages.cs
#endregion

using System;
using System.Collections;
using System.Net;
using System.Web.UI.WebControls;
using System.Web;
using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DWB.DualListControl;
using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// Code Behind class for TemplatePages user control
    /// Provides UI for Add/Remove Pages from Template
    /// </summary>
    public partial class TemplatePages : UIHelper
    {

        #region Declarations
        private ListEntry objTemplateData;
        #endregion 

        #region Protected Methods
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {

                    /// Bind the Dual List box
                    /// Left Box - Pages in Library - DWB Master Pages List
                    /// Right Box - Pages in Templat - DWB Template Pages Mapping List
                    
                    string strTemplateID = HttpContext.Current.Request.QueryString[IDVALUEQUERYSTRING];

                    objTemplateData = GetDetailsForSelectedID(strTemplateID, TEMPLATELIST, TEMPLATE);
                    if (objTemplateData != null && objTemplateData.TemplateDetails != null)
                    {
                        lblTemplateTitle.Text = objTemplateData.TemplateDetails.Title;
                        hdnTemplateType.Value = objTemplateData.TemplateDetails.AssetType;
                        SetDualListLeftBox(duallistTemplatePages, MASTERPAGELIST, TEMPLATEMASTERPAGES, objTemplateData.TemplateDetails.AssetType);
                    }
                    SetDualListRightBox(duallistTemplatePages, TEMPLATEPAGESMAPPINGLIST, strTemplateID, TEMPLATEMASTERPAGES);
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
        /// Handles the Click event of the cmdCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void CmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(MAINTAINTEMPLATEPAGESURL +"?" + IDVALUEQUERYSTRING +"=" + HttpContext.Current.Request.QueryString[IDVALUEQUERYSTRING],false);
        }
     
        /// <summary>
        /// Handles the Click event of the cmdSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void CmdSave_Click(object sender, EventArgs e)
        {
            /// Update the DWB Template Master Pages list for current list of Master Pages
            /// Update the DWB Master Page list for Template_ID column [update for new for addition and removal of a master page from Template
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString[IDVALUEQUERYSTRING]))
                {
                    int intSelectedTemplateID = Int32.Parse(HttpContext.Current.Request.QueryString[IDVALUEQUERYSTRING]);

                    ArrayList arlTemplateConfigurations = new ArrayList();
                    objTemplateData = new ListEntry();
                    TemplateDetails objTemplateDetails = new TemplateDetails();

                    objTemplateDetails.RowId = intSelectedTemplateID;
                    objTemplateDetails.Title = lblTemplateTitle.Text.Trim();
                    objTemplateDetails.AssetType = hdnTemplateType.Value;

                    objTemplateData.TemplateDetails = objTemplateDetails;

                    arlTemplateConfigurations = SetTemplateConfiguration(intSelectedTemplateID);
                    objTemplateData.TemplateConfiguration = arlTemplateConfigurations;                    
                    UpdateListEntry(objTemplateData, TEMPLATEPAGESMAPPINGLIST, TEMPLATECONFIGURATIONAUDIT, TEMPLATEPAGEMAPPING, AUDITACTIONUPDATION);

                    /// Update the DWB Master Pages list for the MasterPages referenced in the new template
                    UpdateListEntry(objTemplateData, MASTERPAGELIST, MASTERPAGEAUDITLIST, MASTERPAGETEMPLATEMAPPING, AUDITACTIONUPDATION);

                    Response.Redirect(MAINTAINTEMPLATEPAGESURL + "?"+ IDVALUEQUERYSTRING+"=" + HttpContext.Current.Request.QueryString[IDVALUEQUERYSTRING],false);
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

        #endregion

        #region Private Methods
        /// <summary>
        /// Sets the template configuration.
        /// </summary>
        /// <param name="selectedtemplateID">The selectedtemplate ID.</param>
        /// <returns>ArrayList</returns>
        private ArrayList SetTemplateConfiguration(int selectedtemplateID)
        {

            ArrayList arlTemplateConfiguaration = new ArrayList();
 
            /// Update the DWB Template Pages Mapping List with new set of pages
            ListItemCollection newMasterPages = duallistTemplatePages.RightItems;
            if (newMasterPages != null && newMasterPages.Count > 0)
            {
                TemplateConfiguration objTemplateConfiguration = null;
                for (int intIndex = 0; intIndex < newMasterPages.Count; intIndex++)
                {                   
                    objTemplateConfiguration = new TemplateConfiguration();

                    /// RowId and LinkedMasterPageId are assigned to Value field of List Box.
                    /// Incase of existing Master Page in Template, it is the ID of the Template Pages Mapping entry
                    /// Incase of new master page, it is the MasterPage ID from Master Page List and from where it has to copied
                                     
                    objTemplateConfiguration.LinkedMasterPageId = newMasterPages[intIndex].Value;
                    objTemplateConfiguration.MasterPageTitle = newMasterPages[intIndex].Text;
                    objTemplateConfiguration.TemplateID = selectedtemplateID.ToString();
                   
                    arlTemplateConfiguaration.Add(objTemplateConfiguration);                    
                }
            }
            return arlTemplateConfiguaration;
        }

        #endregion
    }
}
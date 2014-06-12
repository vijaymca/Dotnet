using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Shell.SharePoint.DREAM.Utilities;
using System.Net;

namespace Shell.SharePoint.DREAM.Site.UI.UserControls
{
    public partial class RegionFormatSettings : UIHelper
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMessage.Text = string.Empty;
                lblMessage.Visible = false;
                if (!Page.IsPostBack)
                {
                    objMossController = objFactory.GetServiceManager("MossService");
                    LoadRegionFormat(cboDateFormat);
                }
            }
            catch (WebException webEx)
            {
                lblMessage.Visible = true;
                lblMessage.Text = webEx.Message;
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }

        }        

        /// <summary>
        /// Handles the Click event of the cmdGoSelect control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmdGoSelect_Click(object sender, EventArgs e)
        {
           
            try
            {                
                PortalConfiguration.GetInstance().SetKey("Date Format", cboDateFormat.SelectedItem.Text.ToString());
                lblMessage.Visible = true;
                lblMessage.Text = "Regional Date Format settings has been updated successfully.";            
            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Regional Date Format settings could not save the changes.";
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }
    }
}
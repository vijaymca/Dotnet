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

namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// This class is used for Batch Import Status log link.
    /// </summary>
    public partial class BatchImportStatus : System.Web.UI.UserControl
    {
        #region DECLARATION
        string strBatchImportStatus = string.Empty;
        const string QUERYSTRINGSTATUS = "status";
        const string SHOWLINKBATCHIMPORTLOG = "Click here to see the Batch Import Log.";
        #endregion DECLARATION

        #region EventHandler Methods
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            strBatchImportStatus = HttpContext.Current.Request.QueryString[QUERYSTRINGSTATUS];

            if (!string.IsNullOrEmpty(strBatchImportStatus))
            {
                lblStatusMsg.Text = strBatchImportStatus;
                lnkShowBatchImportStatus.Text = SHOWLINKBATCHIMPORTLOG;
                lnkShowBatchImportStatus.Attributes.Add("onclick", "return openBatchImportStatus();");
            }
            else
            {
                lnkShowBatchImportStatus.Visible = false; 
            }
        }

        #endregion EventHandler Methods
    }
}
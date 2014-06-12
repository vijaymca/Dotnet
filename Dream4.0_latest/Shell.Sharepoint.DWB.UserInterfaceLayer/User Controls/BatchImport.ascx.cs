#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: BatchImport.ascx.cs
//Modified By:Gopinath
//Date: 172-11-2010
//Reason: To implement the Batch Import.
#endregion

using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Shell.SharePoint.DREAM.Utilities;
using System.Net;

namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// This class is used to fetch shared path from portal configuration to initiate Batch Import Process.
    /// </summary>
    public partial class BatchImport : UIHelper
    {
        #region VARIBALE DECLARATION
        const string BATCHIMPORTPATH = "BatchImportSharedPath";
        const string BATCHIMPORTPATHSESSION = "BatchImportPath";
        const string ENABLETEXTBOX = "document.getElementById(GetObjectID('txtSharedPath','input')).disabled = false;";
        const string ALERTPATHISEMPTY = "alert('Please enter a valid path.');";
        string strBookID;
        #endregion VARIBALE DECLARATION

        #region Events

        /// <summary>
        /// First hit
        /// </summary>
        protected void Page_Init()
        {
            try
            {
                txtSharedPath.Text = GetDefaultSharedPath();
                strBookID = HttpContext.Current.Request.QueryString["bookId"];

            }
            #region Exception Block
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
            #endregion Exception Block
        }


        /// <summary>
        /// When continue click the sharedpath stored into the session variable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnContinue_Click(object sender, EventArgs e)
        {
            try
            {
                string strSharedPath = txtSharedPath.Text.Trim();
                if (!string.IsNullOrEmpty(strSharedPath))
                {
                    HttpContext.Current.Session[BATCHIMPORTPATHSESSION] = strSharedPath;
                    if (!string.IsNullOrEmpty(strBookID))
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "BatchImportConfiguration", "<Script language='javascript'>openBatchImportConfiguration('" + strBookID + "');</Script>");
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "TextboxEnable", ENABLETEXTBOX, true);
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "pathempty", ALERTPATHISEMPTY, true);
                }

            }
            #region Exception Block
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
            #endregion Exception Block
        }

        #endregion Events

        #region Private Methods

        /// <summary>
        /// Gets the default shared path.
        /// </summary>
        /// <returns></returns>
        private string GetDefaultSharedPath()
        {
            string strDefaultSharedPath = string.Empty;
            strDefaultSharedPath = PortalConfiguration.GetInstance().GetKey(BATCHIMPORTPATH);
            return strDefaultSharedPath;
        }

        #endregion Private Methods


    }
}
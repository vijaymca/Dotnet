#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: UploadDocument.ascx 
#endregion

/// <summary> 
/// This is UploadDocument user control class. This class is used for handling the 
/// uploading documents for Type III report
/// </summary> 

using System;
using System.IO;
using System.Web;
using System.Net;
using System.Web.UI;
using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DREAM.Utilities;
using System.Text;
using Shell.SharePoint.DWB.BusinessLogicLayer;
using System.Data;

namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// Enables the user to update/upload a Type III document.
    /// </summary>
    public partial class UploadDocument : UIHelper
    {
        #region Declaration

        const string UPLOADSUCCESS = "Document uploaded successfully.";
        StringBuilder strFileTypes;
        const string DWBFILETYPE = "DWB File Types";
        CommonBLL objCommonBLL;
        #endregion

        #region Methods
        /// <summary>
        /// Loads default values 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    if (HttpContext.Current.Request.QueryString["PageId"] != null)
                        hidPageId.Value = HttpContext.Current.Request.QueryString["PageId"];//.ToString();
                }
                //Added By Praveena
                //To get all File Types from List and passing to JavaScript
                DataTable dtListData = null;
                string strCamlQuery = @" <Where><IsNotNull><FieldRef Name='File_Type' /></IsNotNull></Where>";
                string strViewFields = string.Empty;
                strViewFields = @"<FieldRef Name='File_Type' />";
                objCommonBLL = new CommonBLL();
                dtListData = objCommonBLL.ReadList(strParentSiteURL, DWBFILETYPE, strCamlQuery, strViewFields);
                strFileTypes = new StringBuilder();
                for (int intRowIndex = 0; intRowIndex < dtListData.Rows.Count; intRowIndex++)
                {
                    strFileTypes.Append(Convert.ToString(dtListData.Rows[intRowIndex]["File_Type"]));
                    strFileTypes.Append("|");
                }
                btnUpload.OnClientClick = "javascript:return eWBValidateUpload('" + strFileTypes.ToString() + "')";
            }
            catch (WebException webEx)
            {

                RenderException(webEx.Message);
            }
            catch (Exception ex)
            {

                CommonUtility.HandleException(strParentSiteURL, ex);

            }
        }

        /// <summary>
        /// Uploads the selected document to SharePoint Document Library
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            Stream objStream = null;
            try
            {
                if (fileUploader.PostedFile != null)
                {

                    objStream = fileUploader.PostedFile.InputStream;
                    byte[] fileContents = new byte[objStream.Length];
                    objStream.Read(fileContents, 0, (int)objStream.Length);
                    objStream.Close();
                    string strPageID = hidPageId.Value;
                    string fileName = fileUploader.PostedFile.FileName.Substring(fileUploader.PostedFile.FileName.LastIndexOf("\\") + 1);
                    if (string.Equals(HttpContext.Current.Request.QueryString["type"].ToString(), "3"))
                    {

                        UploadFiletoDocLibrary(USERDEFINEDDOCUMENTLIST, strPageID, fileName, fileContents);
                        //setting session object to tue indicating the document was successfully uploaded. 
                        HttpContext.Current.Session[SESSIONTYPE3UPLOADED] = true;

                        Page.ClientScript.RegisterStartupScript(typeof(string), "updateUploadFlag", "updateUploadFlag('" + HttpContext.Current.Session[SESSIONTYPE3UPLOADED].ToString().ToLowerInvariant() + "','hidtype3uploaded','input');", true);
                    }
                    else
                    {
                        UploadFiletoDocLibrary(PUBLISHEDDOCUMENTLIST, strPageID, fileName, fileContents);
                    }

                    UpdateAuditHistory(strPageID, CHAPTERPAGESMAPPINGAUDITLIST, AUDITACTIONUPDATION);

                    fileUploader.Visible = false;
                    btnUpload.Visible = false;
                    btnCancel.Visible = false;
                    RenderException(UPLOADSUCCESS);
                    btnClose.Visible = true;
                }
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(strParentSiteURL, webEx, 1);
                RenderException(webEx.Message);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strParentSiteURL, ex);
            }
            finally
            {
                if (objStream != null) objStream.Dispose();
            }
        }


        /// <summary>
        /// Renders the exception message
        /// </summary>
        /// <param name="p"></param>
        private void RenderException(string exception)
        {
            if (lblErrorMessage != null)
            {
                lblErrorMessage.Text = exception;
                lblErrorMessage.Visible = true;
            }
        }



        #endregion
    }
}
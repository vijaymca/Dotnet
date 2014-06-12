#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : FileUpload.cs
#endregion
using System;
using System.IO;
using System.Net;
using Shell.SharePoint.DREAM.Utilities;


namespace Shell.SharePoint.DREAM.Site.UI
{
    /// <summary>
    /// Handles the file upload control
    /// </summary>
    public partial class FileUpload : UIHelper
    {

        /// <summary>
        /// Handles the Click event of the BtnOK control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void BtnOK_Click(object sender, EventArgs e)
        {
            //check if the uploaded file is null or not
            try
            {
                if (fileUploader.PostedFile != null)
                {
                    Stream objStream = fileUploader.PostedFile.InputStream;
                    byte[] objFileContents = new byte[objStream.Length];
                    objStream.Read(objFileContents, 0, (int)objStream.Length);
                    if (objStream != null)
                    {
                        objStream.Close();
                        objStream.Dispose();
                    }

                    string strFileName = fileUploader.PostedFile.FileName.Substring(fileUploader.PostedFile.FileName.LastIndexOf("\\") + 1);
                    string strFilePath = fileUploader.PostedFile.FileName.Replace("\\", "/");
                    //store the uploaded file in Session object

                    Session["FileAttached"] = objFileContents;
                    Page.ClientScript.RegisterStartupScript(typeof(string), "update", "UpdateParentControl('lnkFile','A','" + strFileName + "','" + strFilePath + "');", true);
                }
            }
            catch (WebException webEx)
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = webEx.Message;
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }
    }
}
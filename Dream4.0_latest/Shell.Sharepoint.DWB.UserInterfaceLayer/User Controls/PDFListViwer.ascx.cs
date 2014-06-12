#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: PDFListViwer.ascx.cs
#endregion

using System;
using System.Data;
using System.Net;
using System.Web;
using System.Web.UI.WebControls;

using Shell.SharePoint.DWB.BusinessLogicLayer;
using Shell.SharePoint.DREAM.Utilities;


namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// Displays the list of Published Books link.
    /// </summary>
    public partial class PDFListViwer : System.Web.UI.UserControl
    {
        #region Variables
        PDFListVwrBLL objpdfLstVwr;
        DataTable dtPdfList;
        #endregion

        #region Constants
        const string DWBPUBLISHEDLIBRARY = "DWB Book Print details Library";
        const string PUBLISHEDVERSIONTEXT = "Published Version(s) of : {0}";
        const string NOPUBLSIEDVERSIONTEXT = "No published version(s) of : {0}";
        const string QUERYSTRINGBOOKID = "BookID";
        const string QUERYSTRINGBOOKNAME = "BookName";
        #endregion

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                objpdfLstVwr = new PDFListVwrBLL();
                dtPdfList = new DataTable();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString[QUERYSTRINGBOOKID]))
                {
                    dtPdfList = objpdfLstVwr.GetSPFiles(HttpContext.Current.Request.Url.ToString(), DWBPUBLISHEDLIBRARY, Convert.ToInt32(HttpContext.Current.Request.QueryString[QUERYSTRINGBOOKID]));
                    lblException.Text = string.Empty;
                    lblHeader.Text = string.Format(PUBLISHEDVERSIONTEXT, HttpContext.Current.Request.QueryString[QUERYSTRINGBOOKNAME]);
                    if (dtPdfList != null && dtPdfList.Rows.Count > 0)
                    {
                        rptrFileslist.DataSource = dtPdfList;
                        rptrFileslist.DataBind();
                    }
                    else
                    {
                        lblHeader.Text = string.Format(PUBLISHEDVERSIONTEXT, HttpContext.Current.Request.QueryString[QUERYSTRINGBOOKNAME]);
                        ExceptionBlock.Visible = true;
                        lblException.Visible = true;
                        lblException.Text = string.Format(NOPUBLSIEDVERSIONTEXT, HttpContext.Current.Request.QueryString[QUERYSTRINGBOOKNAME]); ;
                    }

                }
                else
                {
                    lblHeader.Text = string.Format(PUBLISHEDVERSIONTEXT, HttpContext.Current.Request.QueryString[QUERYSTRINGBOOKNAME]);
                    ExceptionBlock.Visible = true;
                    lblException.Visible = true;
                    lblException.Text = string.Format(NOPUBLSIEDVERSIONTEXT, HttpContext.Current.Request.QueryString[QUERYSTRINGBOOKNAME]); ;
                }
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), webEx);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
            finally
            {
                if (dtPdfList != null)
                {
                    dtPdfList.Dispose();
                }
            }
        }
    }
}
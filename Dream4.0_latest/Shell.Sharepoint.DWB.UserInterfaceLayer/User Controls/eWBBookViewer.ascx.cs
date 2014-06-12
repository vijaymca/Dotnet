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
using Microsoft.SharePoint;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Shell.SharePoint.DWB.BusinessLogicLayer;
using System.IO;

namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    public partial class eWBBookViewer : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strRequestID = string.Empty;
            string filepath = string.Empty;
            string strCAMLQuery = string.Empty;
            string strLibraryName = string.Empty;
            CommonBLL objCommon = new CommonBLL();

            WindowsImpersonationContext ctx = WindowsIdentity.Impersonate(IntPtr.Zero);
            try
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["requestID"]))
                    strRequestID = Page.Request.QueryString["requestID"].ToString();
                if (!string.IsNullOrEmpty(Page.Request.QueryString["mode"]))
                {
                    if (string.Equals(Page.Request.QueryString["mode"], "chapter"))
                        strLibraryName = "DWB Chapter Print Details";
                    else if (string.Equals(Page.Request.QueryString["mode"], "book"))
                        strLibraryName = "DWB Book Print details Library";
                    else
                        strLibraryName = "DWB Page Print details";
                }

                strCAMLQuery = @"<Where><Eq><FieldRef Name='RequestID' /><Value Type='Text'>" + strRequestID + "</Value></Eq></Where>";
                filepath = @objCommon.GetRequestID(SPContext.Current.Site.Url, strLibraryName, strCAMLQuery);
                FileInfo fInfo = new FileInfo(filepath);

                if (fInfo.Exists)
                {
                    HttpContext.Current.Response.ClearHeaders();
                    HttpContext.Current.Response.ClearContent();
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AppendHeader("Content-Disposition:", " attachment; filename=" + fInfo.Name);
                    HttpContext.Current.Response.AppendHeader("Content-Length", Convert.ToString(fInfo.Length));
                    HttpContext.Current.Response.Charset = "UTF-8";

                    HttpContext.Current.Response.TransmitFile(fInfo.FullName);
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.Close();
                }
            }
            finally
            {
                ctx.Undo();
            }
        }
    }
}
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

namespace Shell.SharePoint.DREAM.Site.UI
{
    public partial class CSVExport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;

            context.Response.ContentType = "application/force-download";
            context.Response.AddHeader("content-disposition", "filename=filename.csv");
            context.Response.Write(context.Request.Form["exportdata"]);
            context.Response.End();
        }
    }
}
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace Shell.SharePoint.DREAM.Site.UI
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CsvExport : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
           // context.Response.Write("Hello World");

            context.Response.ContentType = "application/force-download"; 
            context.Response.AddHeader("content-disposition", "filename=filename.csv"); 
            context.Response.Write(context.Request.Form["exportdata"]); 
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

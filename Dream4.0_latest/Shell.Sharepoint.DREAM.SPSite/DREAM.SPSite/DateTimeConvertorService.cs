using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Shell.SharePoint.DREAM.Utilities;

namespace DateTimeFormatter
{
    [System.Web.Script.Services.ScriptService]
    [WebService(Namespace = "http://xmlforasp.net")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class DateTimeConvertorService : WebService
    {
        private const string DATEFORMAT = "Date Format";

        /// <summary>
        /// Converts and Returns the Culture Formatted Date Time object of the date in string 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetDateTime(string date)
        {
            string strDateFormat = string.Empty;
            strDateFormat = PortalConfiguration.GetInstance().GetKey(DATEFORMAT).ToString();
            DateTime dtmValue = DateTime.Parse(date);
            return dtmValue.ToString(strDateFormat);
        }
    }
}

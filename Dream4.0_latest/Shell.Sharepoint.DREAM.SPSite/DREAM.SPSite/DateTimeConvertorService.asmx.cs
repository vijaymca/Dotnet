using System;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Script.Services;
using Shell.SharePoint.DREAM.Utilities;
using Microsoft.SharePoint;

namespace Shell.SharePoint.DREAM.Site.UI
{
    /// <summary>
    /// Summary description for DateTimeConvertorService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // The following attribute allows the service to 
    // be called from script using ASP.NET AJAX.
    [System.Web.Script.Services.ScriptService]
    public class DateTimeConvertorService :System.Web.Services.WebService
    {
        public DateTimeConvertorService()
        {
            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        /// <summary>
        /// Gets the date time.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        [WebMethod]
        public string GetDateTime(string date)
        {
            const string DATEFORMAT = "Date Format";
            string strDateFormat = string.Empty;
            strDateFormat = PortalConfiguration.GetInstance().GetKey(DATEFORMAT).ToString();
            DateTime dtmValue = DateTime.Parse(date);
            return dtmValue.ToString(strDateFormat);
        }

        /// <summary>
        /// Parses the date time.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        [WebMethod]
        public string ParseDateTime(string date)
        {
            const string DATEFORMAT = "MM-dd-yyyy";
            DateTime dtmValue;
            try
            {
                dtmValue = DateTime.Parse(date);
            }
            catch(FormatException ex)
            {
                return ex.Message;
            }
            return dtmValue.ToString(DATEFORMAT);
        }

        /// <summary>
        /// Parses the date time.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        [WebMethod]
        public string GetDateInFormat(string date, string format)
        {
            DateTime dtmValue;
            try
            {
                dtmValue = DateTime.Parse(date);
            }
            catch(FormatException ex)
            {
                return ex.Message;
            }
            return dtmValue.ToString(format);
        }
        /// <summary>
        /// Validates the date.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        [WebMethod]
        public string[] ValidateDate(string startDate, string endDate)
        {
            const string DATEFORMAT = "MM-dd-yyyy";
            string[] arrResult = new string[2];
            DateTime dtmStartDate;
            DateTime dtmEndDate;
            try
            {
                dtmStartDate = DateTime.Parse(startDate);
                dtmEndDate = DateTime.Parse(endDate);
                arrResult[0] = dtmStartDate.ToString(DATEFORMAT);
                arrResult[1] = dtmEndDate.ToString(DATEFORMAT);
            }
            catch(FormatException ex)
            {
                if(ex.Message.ToLowerInvariant().Contains("unknown word"))
                    arrResult[0] = "Invalid Date format";
                else
                    arrResult[0] = "Invalid Date";
                arrResult[1] = PortalConfiguration.GetInstance().GetKey(DATEFORMAT).ToString();
                return arrResult;
            }
            return arrResult;
        }
        /// <summary>
        /// Determines whether [is valid date] [the specified STR date].
        /// </summary>
        /// <param name="strDate">The STR date.</param>
        /// <returns>
        /// 	<c>true</c> if [is valid date] [the specified STR date]; otherwise, <c>false</c>.
        /// </returns>
        [WebMethod]
        public bool IsValidDate(string strDate)
        {
            bool blnValidDate = false;
            DateTime objDateTime;
            try
            {
                objDateTime = DateTime.Parse(strDate);
                blnValidDate = true;
            }
            catch(FormatException)
            {
                blnValidDate = false;
            }
            return blnValidDate;
        }
        /// <summary>
        /// Gets the search results.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        [WebMethod]
        public string[] GetSearchResults(string searchCriteria, string listName, string columnName)
        {
            DataRow[] dataRow = null;
            DataTable dtResults = null;
            string[] arrResult = null;
            string strCamlQuery = "<OrderBy><FieldRef Name=\"" + columnName + "\"/></OrderBy>";
            string strViewFields = @"<FieldRef Name='" + columnName + "'/>";
            if(string.IsNullOrEmpty(searchCriteria))
            {
                searchCriteria = "*";
            }
            else if(!searchCriteria.Contains("*"))
            {
                searchCriteria += "*";
            }
            try
            {
                using(SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using(SPWeb web = site.OpenWeb())
                    {
                        SPList objSPList = web.Lists[listName];
                        SPQuery query = new SPQuery();
                        query.ViewFields = strViewFields;
                        query.Query = strCamlQuery;
                        dtResults = objSPList.GetItems(query).GetDataTable();
                        try
                        {
                            dataRow = dtResults.Select(columnName + " LIKE '" + searchCriteria + "'");
                        }
                        catch(EvaluateException)
                        {
                            dataRow = null; //catching invalid search expression.
                        }
                    }
                }
            }
            catch(Exception objException)
            {
                arrResult = new string[1];
                arrResult[0] = objException.Message;
                return arrResult;
            }
            finally
            {
                if(dtResults != null)
                    dtResults.Dispose();

            }
            if(dataRow != null)
            {
                arrResult = new string[dataRow.Length];
                for(int intCounter = 0; intCounter < dataRow.Length; intCounter++)
                {
                    arrResult[intCounter] = (string)dataRow[intCounter][columnName];
                }
            }
            else
            {
                arrResult = new string[1];
                arrResult[0] = "Invalid search expression.";
            }
            return arrResult;
        }
    }
}

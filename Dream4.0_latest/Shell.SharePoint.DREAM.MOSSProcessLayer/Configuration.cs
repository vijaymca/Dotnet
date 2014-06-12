#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: Configuration.cs
#endregion


using Microsoft.SharePoint;  



namespace Shell.SharePoint.DREAM.MOSSProcess
{
    /// <summary>
    /// The Configuration Class to get the Custom Exception message from the SharePoint site.
    /// </summary>
    public class Configuration
    {
        #region Declaration
        const string CUSTOMEXCEPTIONLIST = "Custom Exception";
        #endregion
        #region CustomException Method
        /// <summary>
        /// Gets the custom exception message.
        /// </summary>
        /// <param name="parentSiteUrl">The parent site URL.</param>
        /// <param name="exceptionID">The exception ID.</param>
        /// <returns></returns>
        internal string GetCustomExceptionMessage(string parentSiteUrl, string exceptionID)
        {
            SPQuery query;
            SPList list;
            string strCustomExceptionMsg = string.Empty;
            string strQueryString = string.Empty;

            try
            {
                strQueryString = "<OrderBy><FieldRef Name=\"Title\" /></OrderBy><Where><Eq><FieldRef Name=\"LinkTitle\" /><Value Type=\"Text\">" + exceptionID + "</Value></Eq></Where>";
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(parentSiteUrl))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            list = web.Lists[CUSTOMEXCEPTIONLIST];
                            query = new SPQuery();
                            query.Query = strQueryString;
                            SPListItemCollection listItems = list.GetItems(query);

                            if (listItems.Count > 0)
                            {
                                SPListItem listItem = listItems[0];                                
                                strCustomExceptionMsg = listItem["Exception Message"].ToString();
                            }
                        }
                    }
                });
            }
            catch
            {
                throw;
            }
            return strCustomExceptionMsg;
        }
        #endregion
    }
}

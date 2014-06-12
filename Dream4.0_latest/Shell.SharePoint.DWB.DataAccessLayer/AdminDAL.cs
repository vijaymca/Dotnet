#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: AdminDAL.cs
#endregion

using System.Data;
using Microsoft.SharePoint;

namespace Shell.SharePoint.DWB.DataAccessLayer
{
    /// <summary>
    /// Data access layer class for Admin Panel 
    /// </summary>
    public class AdminDAL
    {
       
        #region Public Methods
        
        /// <summary>
        /// Reads the list & return the list items as Datatable.
        /// This function accepts CAML query using the paramenter querystring.
        /// </summary>
        /// <param name="parentSiteUrl">The parent site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="queryString">The CAML query string.</param>
        /// <returns></returns>
        internal DataTable ReadList(string parentSiteUrl, string listName, string queryString)
        {
            DataTable dtObjListItems = new DataTable();
            SPList list;
            SPQuery query;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(parentSiteUrl))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            list = web.Lists[listName];
                            query = new SPQuery();
                            query.Query = queryString;
                            if (list.GetItems(query).Count > 0)
                            {
                                /// Reads the values from sharepoint list.
                                dtObjListItems = list.GetItems(query).GetDataTable();
                            }
                        }
                    }
                });
            }
            catch 
            {
                throw;
            }
            finally
            {
                if (dtObjListItems != null)
                dtObjListItems.Dispose();
            }
            return dtObjListItems;
        }
        #endregion
    }
}

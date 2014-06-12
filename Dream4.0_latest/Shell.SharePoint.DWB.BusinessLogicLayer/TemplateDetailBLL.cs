#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: TemplateDetailBLL.cs
#endregion

using System.Data;
using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DWB.DataAccessLayer;


namespace Shell.SharePoint.DWB.BusinessLogicLayer
{
    /// <summary>
    /// Business Logic Layer for Add/Update Template, Template Pages, Terminate/Activate Templates
    /// </summary>
    public class TemplateDetailBLL
    {

        #region Declarations
        TemplateDAL objTemplateDAL;
        CommonDAL objCommonDAL;
        #endregion

        #region Public Methods
        /// <summary>
        /// Updates the list entry for Add/Edit template.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listEntry">The list entry.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="auditListName">Audit List Name.</param>
        /// <param name="userName">User Name.</param>
        /// <param name="actionPerformed">Audit Action.</param>
        /// <exception cref="">Handled in calling class.</exception>
        public void UpdateListEntry(string siteURL, ListEntry listEntry, string auditListName, string listName,
            string userName, string actionPerformed)
        {
            objTemplateDAL = new TemplateDAL();
            objTemplateDAL.UpdateListEntry(siteURL, listEntry, listName, auditListName, userName,
                actionPerformed);
        }

        /// <summary>
        /// Updates the Mapping between master page and template in DWB Template Pages Mapping list
        /// </summary>
        /// <param name="siteURL">URL of the Site where list available</param>
        /// <param name="listEntry">ListEntry object</param>
        /// <param name="listName">DWB Template Pages Mapping</param>
        /// <param name="auditListName">DWB Template Pages Mapping Audit Trial</param>
        /// <param name="userName">User Name</param>
        /// <param name="actionPerformed">Audit Action</param>
        /// <param name="selectedTemplateID">TemplateID</param>
        /// <exception cref="">Handled in calling class.</exception>
        public void UpdateTemplatePageMapping(string siteURL, ListEntry listEntry, string listName, string auditListName, string userName, string actionPerformed, string selectedTemplateID)
        {
            objTemplateDAL = new TemplateDAL();
            objTemplateDAL.UpdateTemplatePageMapping(siteURL, listEntry, listName, auditListName, userName, actionPerformed, selectedTemplateID);
        }

        /// <summary>
        /// Updates the Templates column in DWB Master Page list
        /// </summary>
        /// <param name="siteUrl">Site URL</param>
        /// <param name="listEntry">ListEntry object</param>
        /// <param name="listName">List Name(DWB Master Page)</param>
        /// <param name="auditListName">Audit List Name</param>
        /// <param name="userName">User Name</param>
        /// <param name="actionPerformed">Audit Action</param>
        /// <exception cref="">Handled in calling class.</exception>
        public void UpdateTemplateIDinMasterPageList(string siteURL, ListEntry listEntry, string listname, string auditListName, string userName, string actionPerformed)
        {
            objTemplateDAL = new TemplateDAL();
            objTemplateDAL.UpdateTemplateIDinMasterPageList(siteURL, listEntry, listname, auditListName, userName, actionPerformed);
        }

        /// <summary>
        /// Method to update the Page Sequence for Master Pages in a Template
        /// </summary>
        /// <param name="siteURL">Site URL.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="auditListName">Audit List Name.</param>
        /// <param name="actionPerformed">Audit Action.</param>
        /// <param name="userName">User Name.</param>
        /// <param name="dvUpdateListitems">DataView.</param>
        public void UpdatePageSequence(string siteURL, string listName, string auditListName, string actionPerformed, string userName, DataView dvUpdateListitems)
        {
            objCommonDAL = new CommonDAL();
            objCommonDAL.UpdateSequence(siteURL, listName, dvUpdateListitems, "Page_Sequence");

            if (dvUpdateListitems != null && dvUpdateListitems.Count > 0)
            {
                for (int i = 0; i < dvUpdateListitems.Count; i++)
                {
                    objCommonDAL.UpdateListAuditHistory(siteURL, auditListName,
                        int.Parse(dvUpdateListitems[i]["ID"].ToString()), userName, actionPerformed);
                }
            }
            dvUpdateListitems.Dispose();
        }

        /// <summary>
        /// Updates the details of the Master Page whiled edited
        /// Called from MasterPage.ascx
        /// </summary>
        /// <param name="siteUrl">Site URL</param>
        /// <param name="listName">List Name</param>
        /// <param name="auditListName">Audit List Name</param>
        /// <param name="objListEntry">List Entry object</param>
        /// <param name="actionPerformed">Audit Action</param>
        /// <param name="userName">User Name</param>
        /// <returns>returns the TemplateID associated with the master page</returns>
        public int UpdateMasterPageDetail(string siteUrl, string listName, string auditListName, ListEntry objListEntry, string actionPerformed, string userName)
        {
            objTemplateDAL = new TemplateDAL();
            return objTemplateDAL.UpdateMasterPageDetail(siteUrl, listName, auditListName, objListEntry, actionPerformed, userName);
        }

        /// <summary>
        /// Gets the details of the selected Template
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="queryString">CAML Query.</param>
        /// <returns>List Entry object with TemplateDetails property values set.</returns>
        public ListEntry GetTemplateDetail(string siteURL, string listName, string CAMLQuery)
        {
            objTemplateDAL = new TemplateDAL();
            return (objTemplateDAL.GetTemplateDetail(siteURL, listName, CAMLQuery));
        }

        /// <summary>
        /// Gets the Master Pages
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="queryString">CAML Query.</param>
        /// <param name="viewFields">View Fields.</param>
        /// <returns>DataTable.</returns>
        public DataTable GetMasterPageList(string siteURL, string listName, string CAMLQuery, string viewFields)
        {
            objCommonDAL = new CommonDAL();
            return objCommonDAL.ReadList(siteURL, listName, CAMLQuery, viewFields);
        }

        /// <summary>
        /// Gets the Master Pages for the selected Template
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="queryString">CAML Query.</param>
        /// <param name="viewFields">View Fields.</param>
        /// <returns>List Entry object with TemplateConfiguration values assigned.</returns>
        /// <exception cref="">Handled in calling method.</exception>
        public ListEntry GetMasterPageDetails(string siteURL, string listName, string CAMLQuery, string viewFields)
        {
            objTemplateDAL = new TemplateDAL();
            return objTemplateDAL.GetMasterPageDetail(siteURL, listName, CAMLQuery, viewFields);
        }

        #endregion
    }
}

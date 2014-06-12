#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: MasterDAL.cs
#endregion
using System;
using System.Data;
using System.Text;
using Shell.SharePoint.DWB.Business.DataObjects;
using Microsoft.SharePoint;


namespace Shell.SharePoint.DWB.DataAccessLayer
{
    /// <summary>
    /// Data access layer class for DWB Master Pages 
    /// Get/Set the DWB Master Page details from/to SharePoint list
    /// </summary>   
    public class MasterDAL
    {
        CommonDAL objCommonDAL;
        private const string AUDIT_ACTION_CREATION = "1";
        private const string AUDIT_ACTION_UPDATION = "2";
        private const string AUDIT_ACTION_ACTIVATE = "3";
        private const string AUDIT_ACTION_TERMINATE = "4";

        private const string MASTERPAGELIST = "DWB Master Pages";
        private const string TEMPLATEPAGESMAPPINGLIST = "DWB Template Page Mapping";
        private const string TEMPLATELIST = "DWB Templates";
        /// <summary>
        /// Updates the list entry.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listEntry">The list entry.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="actionPerformed">The action performed.</param>
        internal void UpdateListEntry(string siteURL, ListEntry listEntry, string listName, string actionPerformed)
        {

            SPList list;
            SPQuery query;
            SPListItem objListItem;
            SPFieldLookupValue lookupField;
            DataTable dtlistItem = null;
            int intPageSequence = 10;
            string strListGuid = string.Empty;
            StringBuilder sbMethodBuilder = new StringBuilder();
            string strBatch = string.Empty;
            int intRowId = 0;
            string strBatchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
  "<ows:Batch OnError=\"Return\">{0}</ows:Batch>";

            string strMethodFormat = "<Method ID=\"{0}\">" +
             "<SetList>{1}</SetList>" +
             "<SetVar Name=\"Cmd\">Save</SetVar>" +
             "<SetVar Name=\"ID\">{2}</SetVar>" +
             "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Page_Sequence\">{3}</SetVar>" +
            "</Method>";
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteURL))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;
                            list = web.Lists[listName];
                            strListGuid = list.ID.ToString();
                            query = new SPQuery();
                            objListItem = list.Items.Add();
                            if (string.Equals(actionPerformed, AUDIT_ACTION_UPDATION))
                            {
                                objListItem = list.GetItemById(listEntry.MasterPage.RowId);
                            }
                            if (!string.IsNullOrEmpty(listEntry.MasterPage.Name))
                            {
                                objListItem["MasterPageName"] = listEntry.MasterPage.Name;
                            }
                            if (!string.IsNullOrEmpty(listEntry.MasterPage.TemplateTitle))
                            {
                                objListItem["Title_Template"] = listEntry.MasterPage.TemplateTitle;
                            }
                            if (!string.IsNullOrEmpty(listEntry.MasterPage.AssetType))
                            {
                                lookupField = new SPFieldLookupValue(Convert.ToInt32(listEntry.MasterPage.AssetType), string.Empty);
                                objListItem["Asset_Type"] = lookupField;
                                listEntry.MasterPage.AssetType = lookupField.LookupValue;
                            }

                            if (!string.IsNullOrEmpty(listEntry.MasterPage.SignOffDiscipline))
                            {
                                lookupField = new SPFieldLookupValue(Convert.ToInt32(listEntry.MasterPage.SignOffDiscipline), string.Empty);
                                objListItem["Sign_Off_Discipline"] = lookupField;
                                listEntry.MasterPage.SignOffDiscipline = lookupField.LookupValue;
                            }
                            if (!string.IsNullOrEmpty(listEntry.MasterPage.SOP))
                            {
                                objListItem["Standard_Operating_Procedure"] = listEntry.MasterPage.SOP;
                            }
                            if (!string.IsNullOrEmpty(listEntry.MasterPage.ConnectionType))
                            {
                                lookupField = new SPFieldLookupValue(Convert.ToInt32(listEntry.MasterPage.ConnectionType), string.Empty);
                                objListItem["Connection_Type"] = lookupField;
                                listEntry.MasterPage.ConnectionType = lookupField.LookupValue;
                            }

                            if (!string.IsNullOrEmpty(listEntry.MasterPage.Terminated))
                            {
                                objListItem["Terminate_Status"] = listEntry.MasterPage.Terminated;
                            }
                            if (!string.IsNullOrEmpty(listEntry.MasterPage.PageURL))
                            {
                                objListItem["Page_URL"] = listEntry.MasterPage.PageURL;
                            }
                            if (!string.IsNullOrEmpty(listEntry.MasterPage.PageOwner))
                            {
                                objListItem["Page_Owner"] = listEntry.MasterPage.PageOwner;
                            }
                            if (!string.IsNullOrEmpty(listEntry.MasterPage.Templates))
                            {
                                objListItem["Template_ID"] = listEntry.MasterPage.Templates;
                            }
                            objListItem.Update();
                            listEntry.MasterPage.RowId = int.Parse(objListItem["ID"].ToString());
                            query.Query = @"<OrderBy><FieldRef Name='Page_Sequence' /></OrderBy><Where><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq></Where>";
                            if (string.Equals(actionPerformed, AUDIT_ACTION_CREATION))
                            {

                                dtlistItem = list.GetItems(query).GetDataTable();
                                if (dtlistItem != null && dtlistItem.Rows.Count > 0)
                                {
                                    listEntry.MasterPage.PageSequence = 10;
                                    sbMethodBuilder.AppendFormat(strMethodFormat, listEntry.MasterPage.RowId, strListGuid, listEntry.MasterPage.RowId, 10);
                                    for (int i = 0; i < dtlistItem.Rows.Count; i++)
                                    {

                                        intRowId = (int)dtlistItem.Rows[i]["ID"];

                                        if (listEntry.MasterPage.RowId != intRowId)
                                        {
                                            intPageSequence = intPageSequence + 10;
                                            sbMethodBuilder.AppendFormat(strMethodFormat, intRowId, strListGuid, intRowId, intPageSequence);
                                        }

                                    }
                                    strBatch = string.Format(strBatchFormat, sbMethodBuilder.ToString());
                                    web.ProcessBatchData(strBatch);
                                }
                            }
                            web.AllowUnsafeUpdates = false;

                        }
                    }
                });
            }
            finally
            {
                if (dtlistItem != null) dtlistItem.Dispose();
            }
        }


        /// <summary>
        /// Updates the list entry.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listEntry">The list entry.</param>
        /// <param name="listName">Name of the list.</param>
        /// <returns>string</returns>
        internal string AddTemplateMasterPageMapping(string siteURL, ListEntry listEntry, string listName)
        {

            SPList list;
            SPListItem objListItem;
            string[] strTemplateIds = null;
            StringBuilder strTemplatePageMappingRowId = new StringBuilder();

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        list = web.Lists[listName];

                        if (!string.IsNullOrEmpty(listEntry.MasterPage.Templates))
                        {
                            strTemplateIds = listEntry.MasterPage.Templates.Split(';');
                        }
                        if (strTemplateIds != null)
                        {
                            for (int i = 0; i < strTemplateIds.Length - 1; i++)
                            {
                                objListItem = list.Items.Add();
                                objListItem["Master_Page_ID"] = listEntry.MasterPage.RowId;
                                objListItem["Template_ID"] = int.Parse(strTemplateIds[i]);
                                objListItem["Page_Sequence"] = listEntry.MasterPage.PageSequence;
                                objListItem["Master_Page_Name"] = listEntry.MasterPage.Name;
                                objListItem["Page_Title_Template"] = listEntry.MasterPage.TemplateTitle;
                                objListItem["Asset_Type"] = listEntry.MasterPage.AssetTypeText;
                                objListItem["Discipline"] = listEntry.MasterPage.SignOffDisciplineText;
                                objListItem["Standard_Operating_Procedure"] = listEntry.MasterPage.SOP;
                                objListItem["Connection_Type"] = listEntry.MasterPage.ConnectionTypeText;
                                if (!string.IsNullOrEmpty(listEntry.MasterPage.PageURL))
                                {
                                    objListItem["Page_URL"] = listEntry.MasterPage.PageURL;
                                }
                                objListItem["Page_Owner"] = listEntry.MasterPage.PageOwner;
                                objListItem.Update();
                                /// Update the DWB Template list - Has_MasterPage column
                                TemplateDAL objTemplateDAL = new TemplateDAL();
                                objTemplateDAL.UpdateHasMasterPageColumn(siteURL, TEMPLATELIST, strTemplateIds[i], 1);
                                strTemplatePageMappingRowId.Append(objListItem["ID"].ToString() + ";");
                            }
                        }
                        web.AllowUnsafeUpdates = false;
                    }
               
                }
            });

            return strTemplatePageMappingRowId.ToString();
        }

        /// <summary>
        /// Update Audit History.
        /// </summary>
        /// <param name="siteURL">Site URL.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="auditListName">Audit List Name.</param>
        /// <param name="rowId">ID of the item which audit needs to be updated.</param>
        /// <param name="pageTitle">Master Page Title</param>
        /// <param name="userName">Name of the user updating.</param>
        /// <param name="actionPerformed">Action Performed.</param>
        internal void UpdateListAuditHistory(string siteURL, string listName, string auditListName, int rowId, string pageTitle, string userName, string actionPerformed)
        {
            DataTable dtList = null;
            DataRow objDataRow;
            string strCamlQuery = string.Empty;
            string strSelectedID = string.Empty;
            SPList list;
            SPQuery query;
            SPListItem objListItem;
            SPFieldLookupValue lookupField;
            try
            {
                if (rowId != 0)
                {
                    strCamlQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Number'>" + rowId + "</Value></Eq></Where>";
                }
                else
                {
                    strCamlQuery = @"<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>" + pageTitle + "</Value></Eq></Where>";
                }
                objCommonDAL = new CommonDAL();
                dtList = objCommonDAL.ReadList(siteURL, listName, strCamlQuery);
                for (int intIndex = 0; intIndex < dtList.Rows.Count; intIndex++)
                {
                    objDataRow = dtList.Rows[intIndex];
                    strSelectedID = objDataRow["ID"].ToString();
                }
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteURL))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;
                            list = web.Lists[auditListName];
                            query = new SPQuery();

                            objListItem = list.Items.Add();
                            objListItem["User"] = userName;
                            if (!string.IsNullOrEmpty(actionPerformed))
                            {
                                lookupField = new SPFieldLookupValue(actionPerformed);
                                objListItem["Audit_Action"] = lookupField;
                            }
                            objListItem["Master_ID"] = strSelectedID;
                            objListItem["Date"] = DateTime.Now;
                            objListItem.Update();
                            web.AllowUnsafeUpdates = false;
                        }
                    }
                });
            }
            finally
            {
                if (dtList != null) dtList.Dispose();
            }
        }

        /// <summary>
        /// Gets the Master Page Details.
        /// </summary>
        /// <param name="parentSiteUrl">Site URL.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="queryString">CAML Query.</param>
        /// <returns>ListEntry object.</returns>
        internal ListEntry GetMasterPageDetail(string parentSiteUrl, string listName, string queryString)
        {
            ListEntry objListEntry = null;
            DataTable objListItems = null;
            DataRow objListRow;
            MasterPageDetails objMasterPage = null;
            try
            {
                objCommonDAL = new CommonDAL();
                objListItems = objCommonDAL.ReadList(parentSiteUrl, listName, queryString);

                if (objListItems != null)
                {
                    objListEntry = new ListEntry();
                    objMasterPage = new MasterPageDetails();
                    for (int index = 0; index < objListItems.Rows.Count; index++)
                    {
                        objListRow = objListItems.Rows[index];
                        objMasterPage.Name = objListRow["Title"].ToString();
                        objMasterPage.TemplateTitle = objListRow["Title_Template"].ToString();
                        if (objListRow["Page_Sequence"] != DBNull.Value)
                        {
                            objMasterPage.PageSequence = Convert.ToInt32(objListRow["Page_Sequence"]);
                        }
                        objMasterPage.AssetType = Convert.ToString(objListRow["Asset_Type"]);
                        objMasterPage.SignOffDiscipline = Convert.ToString(objListRow["Sign_Off_Discipline"]);
                        objMasterPage.SOP = Convert.ToString(objListRow["Standard_Operating_Procedure"]);
                        objMasterPage.ConnectionType = Convert.ToString(objListRow["Connection_Type"]);
                        objMasterPage.Terminated = Convert.ToString(objListRow["Terminate_Status"]);
                        objMasterPage.Templates = Convert.ToString(objListRow["Template_ID"]);
                        objListEntry.MasterPage = objMasterPage;
                    }
                }
                return objListEntry;
            }
            finally
            {
                if (objListItems != null)
                    objListItems.Dispose();
            }
        }
    }
}

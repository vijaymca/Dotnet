#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: TemplateDAL.cs
#endregion
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Shell.SharePoint.DWB.Business.DataObjects;
using Microsoft.SharePoint;

namespace Shell.SharePoint.DWB.DataAccessLayer
{

    /// <summary>
    /// Data Access Layer for Add/Update Template, Template Pages, Terminate/Activate Templates
    /// </summary>
    public class TemplateDAL
    {
        #region Declarations
        CommonDAL objCommonDAL;
        private const string AUDIT_ACTION_CREATION = "1";
        private const string AUDIT_ACTION_UPDATION = "2";
        private const string AUDIT_ACTION_ACTIVATE = "3";
        private const string AUDIT_ACTION_TERMINATE = "4";

        private const string MASTERPAGELIST = "DWB Master Pages";
        private const string TEMPLATEPAGESMAPPINGLIST = "DWB Template Page Mapping";
        private const string TEMPLATELIST = "DWB Templates";
        #endregion Declarations

        #region Methods

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
        internal void UpdateListEntry(string siteUrl, ListEntry listEntry, string listName, string auditListName, string userName, string actionPerformed)
        {
            SPList list;
            SPQuery query;
            SPListItem objListItem;
            SPListItemCollection objListCollection;
            SPFieldLookupValue lookupField;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        list = web.Lists[listName];
                        query = new SPQuery();
                        objListItem = list.Items.Add();

                        if (string.Equals(actionPerformed, AUDIT_ACTION_UPDATION))
                        {
                            query.Query = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Number'>" +
                      listEntry.TemplateDetails.RowId + "</Value></Eq></Where>";
                            objListCollection = list.GetItems(query);
                            foreach (SPListItem objList in objListCollection)
                            {
                                objListItem = objList;
                            }
                        }
                        /// Default Title column is renamed as Template_Name. Internal Name remains same as "Title"
                        objListItem["Title"] = listEntry.TemplateDetails.Title;

                        lookupField = new SPFieldLookupValue(listEntry.TemplateDetails.AssetID);
                        /// Asset_Type
                        objListItem["Asset_Type"] = lookupField;
                        /// Terminate_Status
                        objListItem["Terminate_Status"] = listEntry.TemplateDetails.Terminated;

                        objListItem.Update();
                        web.AllowUnsafeUpdates = false;
                        objCommonDAL = new CommonDAL();
                        objCommonDAL.UpdateListAuditHistory(siteUrl,auditListName, Int32.Parse(objListItem["ID"].ToString()), userName, actionPerformed);
                    }
                }
            });
        }

        /// <summary>
        /// Updates the Mapping between master page and template in DWB Template Pages Mapping list
        /// </summary>
        /// <param name="siteURL">URL of the Site where list available.</param>
        /// <param name="listEntry">ListEntry object.</param>
        /// <param name="listName">Template List Name.(DWB Template Pages Mapping)</param>
        /// <param name="auditListName">Template Audit List Name.(DWB Template Pages Mapping Audit Trial)</param>
        /// <param name="userName">User Name.</param>
        /// <param name="actionPerformed">Audit Action.</param>
        /// <param name="selectedTemplateID">TemplateID.</param>
        /// <exception cref="">Handled in calling class.</exception>
        
        internal void UpdateTemplatePageMapping(string siteUrl, ListEntry listEntry, string listName, string auditListName, string userName, string actionPerformed, string selectedTemplateID)
        {
            /// Query the Template Mapping List for the selected Template ID 
            /// If no records exist then insert all the new records
            /// If records for the particular Template exists, update the existing records and insert if any new pages are added
            int intNoOfMasterPages = 0;
            DataTable dtListItemCollection = null;
            DataTable dtresult = null;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;
                            SPList list = web.Lists[listName];

                            /// Insert the pages entry for the new template
                            if (string.Compare(actionPerformed, AUDIT_ACTION_CREATION, true) == 0)
                            {
                                if (listEntry != null && listEntry.TemplateConfiguration != null)
                                {
                                    foreach (TemplateConfiguration objTemplateConfiguration in listEntry.TemplateConfiguration)
                                    {
                                        SPListItem objListItem = list.Items.Add();
                                        objListItem["Page_Title_Template"] = objTemplateConfiguration.TemplateTitle;
                                        objListItem["Page_Sequence"] = objTemplateConfiguration.PageSequence;
                                        objListItem["Title"] = objTemplateConfiguration.MasterPageTitle;
                                        objListItem["Master_Page_ID"] = objTemplateConfiguration.LinkedMasterPageId;
                                        objListItem["Master_Page_Name"] = objTemplateConfiguration.MasterPageTitle;
                                        objListItem["Asset_Type"] = objTemplateConfiguration.AssetType;
                                        objListItem["Discipline"] = objTemplateConfiguration.Discipline;
                                        objListItem["Connection_Type"] = objTemplateConfiguration.ConnectionType;
                                        objListItem["Page_Owner"] = objTemplateConfiguration.PageOwner;
                                        if (!string.IsNullOrEmpty(objTemplateConfiguration.PageURL))
                                        {
                                            objListItem["Page_URL"] = objTemplateConfiguration.PageURL;
                                        }
                                        objListItem["Standard_Operating_Procedure"] = objTemplateConfiguration.StandardOperatingProcedure;
                                        objListItem["Template_ID"] = objTemplateConfiguration.TemplateID;

                                        objListItem.Update();
                                        intNoOfMasterPages++;
                                        objCommonDAL = new CommonDAL();
                                        objCommonDAL.UpdateListAuditHistory(siteUrl, auditListName, Convert.ToInt32(objListItem["ID"]),
                                            userName, actionPerformed);
                                    }
                                    /// Update the Has_MasterPage column in DWB Templates list if no of master pages in a template >=1
                                    UpdateHasMasterPageColumn(siteUrl, TEMPLATELIST, selectedTemplateID, intNoOfMasterPages);
                                }
                            }
                            else if (string.Compare(actionPerformed, AUDIT_ACTION_UPDATION, true) == 0)/// UPdate the pages details for the template
                            {
                                SPQuery query = new SPQuery();
                                query.Query = @"<OrderBy><FieldRef Name='Master_Page_ID'/></OrderBy><Where><Eq><FieldRef Name='Template_ID' /><Value Type='Number'>" +
                                 selectedTemplateID + "</Value></Eq></Where>";
                                SPListItemCollection objListItemCollection = list.GetItems(query);

                                List<string> itemTobeAddedOrDeleted = new List<string>();
                                List<string> itemToAdded = new List<string>();
                                bool blnMasterPageFound = false;
                                
                                /// If the entry in List Box is not in SPListItemCollection, add it
                                /// If the entry in List Box is in SPListItemCollection, update it
                                /// If the entry is not in ListBox but present in SPListItemCollection delete it
                                if (objListItemCollection != null)
                                {
                                    string strMasterPageID = string.Empty;
                                    foreach (SPListItem item in objListItemCollection)
                                    {
                                        blnMasterPageFound = false;
                                        strMasterPageID = string.Empty;
                                        strMasterPageID = item["ID"].ToString();

                                        if (listEntry != null)
                                        {
                                            if (listEntry.TemplateConfiguration != null && listEntry.TemplateConfiguration.Count > 0)
                                            {
                                                foreach (TemplateConfiguration objTemplateConfiguration in listEntry.TemplateConfiguration)
                                                {
                                                    if (string.Compare(strMasterPageID, objTemplateConfiguration.LinkedMasterPageId) == 0)
                                                    {
                                                        blnMasterPageFound = true;
                                                        break;
                                                    }

                                                }

                                            }

                                            if (!blnMasterPageFound && !itemTobeAddedOrDeleted.Contains(item["ID"].ToString()))
                                            {
                                                itemTobeAddedOrDeleted.Add(item["ID"].ToString());
                                            }
                                        }
                                    }
                                }                              
                                for (int index = 0; index < itemTobeAddedOrDeleted.Count; index++)
                                {
                                    list.Items.DeleteItemById(Int32.Parse(itemTobeAddedOrDeleted[index]));
                                }
                                objListItemCollection = list.GetItems(query);
                                if (objListItemCollection != null)
                                {
                                    dtListItemCollection = objListItemCollection.GetDataTable();
                                }
                                if (listEntry != null && listEntry.TemplateConfiguration != null)
                                {
                                   
                                    SPListItem objListItem = null;
                                    string[] splitter = { "New" };
                                    string strRowID = string.Empty;
                                    string strMasterPageID = string.Empty;
                                    string[] strSplitted = null;
                                    DataRow[] dtRow = null;
                                    string strCAMLQuery = string.Empty;
                                    string strfieldsToView = string.Empty;
                                    foreach (TemplateConfiguration objTemplateConfiguration in listEntry.TemplateConfiguration)
                                    {

                                        objListItem = list.Items.Add();
                                         strRowID = string.Empty;
                                         strMasterPageID = string.Empty;
                                        /// Check the LinkedMasterPageId contains "New" string
                                        /// If contains split the string and assign the value at index 1
                                        /// Else the assign the LinkedMasterPageId value to 

                                        if (objTemplateConfiguration.LinkedMasterPageId.IndexOf("New") != -1)
                                        {
                                            strSplitted = null;
                                            strSplitted = objTemplateConfiguration.LinkedMasterPageId.Split(splitter, StringSplitOptions.None);
                                            if (strSplitted != null && strSplitted.Length >= 2)
                                            {
                                                strMasterPageID = strSplitted[1];
                                            }
                                        }
                                        else
                                        {
                                            strRowID = objTemplateConfiguration.LinkedMasterPageId;
                                            strMasterPageID = objTemplateConfiguration.LinkedMasterPageId;
                                        }
                                        dtRow = null;
                                        if (dtListItemCollection != null && !string.IsNullOrEmpty(strRowID))
                                        {
                                            dtRow = dtListItemCollection.Select("ID = " + strRowID);
                                        }
                                        if (dtRow != null && dtRow.Length > 0)
                                        {
                                            objListItem = list.Items.GetItemById(Convert.ToInt32(dtRow[0]["ID"]));
                                            objListItem.Update();
                                            intNoOfMasterPages++;

                                            objCommonDAL = new CommonDAL();
                                            objCommonDAL.UpdateListAuditHistory(siteUrl, auditListName, Convert.ToInt32(objListItem["ID"]),
                                                userName, actionPerformed);
                                        }
                                        else
                                        {
                                            strCAMLQuery = string.Empty;
                                            strCAMLQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + strMasterPageID + "</Value></Eq></Where>";
                                            strfieldsToView = string.Empty;
                                            /// Title column is renamed to MasterPageName - internal name remains as Title
                                            strfieldsToView = "<FieldRef Name='Title' /><FieldRef Name='Title_Template' /><FieldRef Name='Page_Owner' /><FieldRef Name='Sign_Off_Discipline' /><FieldRef Name='Connection_Type' /><FieldRef Name='ID' /><FieldRef Name='Page_Sequence' /><FieldRef Name='Asset_Type' /><FieldRef Name='Page_URL' /><FieldRef Name='Standard_Operating_Procedure' /><FieldRef Name='Page_Sequence' /><FieldRef Name='Page_Sequence' />";
                                            objCommonDAL = new CommonDAL();
                                            dtresult = objCommonDAL.ReadList(siteUrl, MASTERPAGELIST, strCAMLQuery, strfieldsToView);

                                            if (dtresult != null && dtresult.Rows.Count > 0)
                                            {
                                                foreach (DataRow objdataRow in dtresult.Rows)
                                                {
                                                    objListItem["Page_Title_Template"] = objdataRow["Title_Template"].ToString();
                                                    objListItem["Page_Sequence"] = Int32.Parse(objdataRow["Page_Sequence"].ToString());
                                                    objListItem["Title"] = objdataRow["Title_Template"].ToString();
                                                    objListItem["Master_Page_ID"] = Int32.Parse(objdataRow["ID"].ToString());
                                                    objListItem["Master_Page_Name"] = objdataRow["Title"].ToString();
                                                    objListItem["Asset_Type"] = objdataRow["Asset_Type"].ToString();
                                                    objListItem["Discipline"] = objdataRow["Sign_Off_Discipline"].ToString();
                                                    objListItem["Connection_Type"] = objdataRow["Connection_Type"].ToString();
                                                    objListItem["Page_Owner"] = objdataRow["Page_Owner"].ToString();
                                                    if (!string.IsNullOrEmpty(objdataRow["Page_URL"].ToString()))
                                                    {
                                                        objListItem["Page_URL"] = objdataRow["Page_URL"].ToString();
                                                    }
                                                    objListItem["Standard_Operating_Procedure"] = objdataRow["Standard_Operating_Procedure"].ToString();
                                                    objListItem["Template_ID"] = selectedTemplateID;

                                                    objListItem.Update();
                                                    intNoOfMasterPages++;
                                                    objCommonDAL = new CommonDAL();
                                                    objCommonDAL.UpdateListAuditHistory(siteUrl, auditListName, Convert.ToInt32(objListItem["ID"]),
                                                        userName, actionPerformed);
                                                }
                                            }
                                        }
                                    }                                   
                                    UpdateHasMasterPageColumn(siteUrl, TEMPLATELIST, selectedTemplateID, intNoOfMasterPages);
                                }
                            }
                            web.AllowUnsafeUpdates = false;
                        }
                    }
                });
            }
            finally
            {
                if (dtListItemCollection != null)
                    dtListItemCollection.Dispose();
                if (dtresult != null)
                    dtresult.Dispose();
            }
        }

        /// <summary>
        /// Updates the Templates column in DWB Master Page list
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="listEntry">ListEntry object.</param>
        /// <param name="listName">List Name(DWB Master Page).</param>
        /// <param name="auditListName">Audit List Name.</param>
        /// <param name="userName">User Name.</param>
        /// <param name="actionPerformed">Audit Action.</param>
        /// <exception cref="">Handled in calling class.</exception>
        internal void UpdateTemplateIDinMasterPageList(string siteUrl, ListEntry listEntry, string listName, string auditListName, string userName, string actionPerformed)
        {
            DataTable dtAddMasterPages = null;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;
                            SPList list = web.Lists[listName];
                            SPQuery query = new SPQuery();
                            string strTemplate_IDs = string.Empty;
                            string strTempIDs = string.Empty;
                            string strTemplateIDAdded = string.Empty;
                            if (listEntry != null)
                            {
                                /// Filter ListItems by AssetType and Terminated Status
                                query.Query = @"<Where><And><Eq><FieldRef Name='Asset_Type' /><Value Type='LookUp'>" + listEntry.TemplateDetails.AssetType + "</Value></Eq><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq></And></Where>";
                                string strSelectedTemplateID = listEntry.TemplateDetails.RowId.ToString();

                                SPListItemCollection objListItemCollection = list.GetItems(query);

                                /// LinkedMasterPageId property holds the item ID of the Template Page Mapping list or ID of the Master Page in Master Pages List prefixed with "New";
                                /// Get the MasterPages collection from DWB Template Master Page List for the selected TemplateID
                                /// Loop through each object in the collection and find the Master Page ID [objListItem["ID"].ToString()] is equal to 
                                /// LinkedMasterPageID [dtRow["Master_Page_ID"].
                                /// If Equals, the master page is mapped to template, check the Template_ID column contains the selected TemplateID or not
                                /// If doesn't contains, append else do nothing
                                /// If not Equals, the master page is not mapped to the selected template, remove the TemplateID from the Template_ID column

                                SPList mappingList = web.Lists[TEMPLATEPAGESMAPPINGLIST];
                                SPQuery mappingQuery = new SPQuery();
                                mappingQuery.Query = @"<OrderBy><FieldRef Name='Page_Sequence' /></OrderBy><Where><Eq><FieldRef Name='Template_ID' /><Value Type='Number'>" + strSelectedTemplateID + "</Value></Eq></Where>";
                                mappingQuery.ViewFields = @"<FieldRef Name='Page_Sequence' /><FieldRef Name='ID' /><FieldRef Name='Master_Page_Name' /><FieldRef Name='Master_Page_ID' />";
                                SPListItemCollection objAddedMasterPages = mappingList.GetItems(mappingQuery);

                                if (objAddedMasterPages != null)
                                {
                                    dtAddMasterPages = objAddedMasterPages.GetDataTable();
                                }
                                DataRow[] dtRow = null;
                                int index = -1;
                                foreach (SPListItem objListItem in objListItemCollection)
                                {
                                    strTemplate_IDs = string.Empty;
                                    strTempIDs = string.Empty;
                                    if (objListItem["Template_ID"] != null)
                                    {
                                        strTemplate_IDs = objListItem["Template_ID"].ToString();
                                    }
                                    dtRow = null;
                                    if (dtAddMasterPages != null)
                                    {
                                        dtRow = dtAddMasterPages.Select("Master_Page_ID = " + objListItem["ID"].ToString());
                                    }
                                    if (dtRow != null && dtRow.Length > 0)
                                    {
                                        /// The Master Pages exist in Mapping List for the selected Template
                                        /// Check whether the TemplateID present in Template_ID column or not
                                        /// If not exist, append else do nothing
                                        strTempIDs = ";" + strTemplate_IDs;
                                        index = strTempIDs.IndexOf(";" + strSelectedTemplateID + ";");
                                        if (index == -1)
                                        {
                                            strTemplate_IDs = strTemplate_IDs + strSelectedTemplateID + ";";
                                        }
                                    }
                                    else
                                    {
                                        /// The Master Page is not mapped to selected Template in Mapping List
                                        /// Check whether the TemplateID present in Template_ID column or not
                                        /// If exists remove and update the item else do nothing
                                        strTempIDs = ";" + strTemplate_IDs;
                                        index = strTempIDs.IndexOf(";" + strSelectedTemplateID + ";");
                                        if (index != -1)
                                        {
                                            strTemplate_IDs = strTemplate_IDs.Remove(index, strSelectedTemplateID.Length + 1);
                                        }
                                    }

                                    objListItem["Template_ID"] = strTemplate_IDs;
                                    objListItem.Update();
                                    objCommonDAL = new CommonDAL();
                                    objCommonDAL.UpdateListAuditHistory(siteUrl, auditListName, Convert.ToInt32(objListItem["ID"]),
                                        userName, actionPerformed);
                                }

                                web.AllowUnsafeUpdates = false;
                            }
                        }// Ends using SPWeb                       
                    }// Ends using SPSite
                }); // Ends using RunWithElevatedPrivileges
            }
            finally
            {
                if (dtAddMasterPages != null)
                    dtAddMasterPages.Dispose();
            }
        }

        /// <summary>
        /// Updates the Has_MasterPage column in DWB Template list
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="listName">List Name(DWB Template).</param>
        /// <param name="templateID">TemplateID.</param>
        /// <param name="noOfMasterPages">No of MasterPages in a template.</param>
        internal void UpdateHasMasterPageColumn(string siteUrl, string listName, string templateID, int noOfMasterPages)
        {

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        SPList list = web.Lists[listName];

                        SPListItem listItem = list.GetItemById(Int32.Parse(templateID));
                        if (noOfMasterPages > 0)
                        {
                            listItem["Has_MasterPage"] = "Yes";
                        }
                        else
                        {
                            listItem["Has_MasterPage"] = "No";
                        }
                        listItem.Update();
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        /// <summary>
        /// Updates the details of the Master Page whiled edited
        /// Called from MasterPage.ascx
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="auditListName">Audit List Name.</param>
        /// <param name="objListEntry">List Entry object.</param>
        /// <param name="actionPerformed">Audit Action.</param>
        /// <param name="userName">User Name.</param>
        /// <returns>returns the TemplateID associated with the master page.</returns>
        /// <exception cref="">Handled in calling method.</exception>
        internal int UpdateMasterPageDetail(string siteUrl, string listName, string auditListName, ListEntry objListEntry, string actionPerformed, string userName)
        {
            int intTemplateID = 0;

            SPSecurity.RunWithElevatedPrivileges(delegate()
               {
                   using (SPSite site = new SPSite(siteUrl))
                   {
                       using (SPWeb web = site.OpenWeb())
                       {
                           web.AllowUnsafeUpdates = true;
                           SPList list = web.Lists[listName];

                           if (objListEntry != null && objListEntry.MasterPage != null)
                           {
                               SPListItem listItem = list.GetItemById(objListEntry.MasterPage.RowId);
                               if (listItem != null)
                               {
                                   listItem["Discipline"] = objListEntry.MasterPage.SignOffDiscipline;//Discipline
                                   listItem["Standard_Operating_Procedure"] = objListEntry.MasterPage.SOP;//Standard_Operating_Procedure 
                                   listItem["Page_Title_Template"] = objListEntry.MasterPage.TemplateTitle;//Page_Title_Template 
                                   listItem["Master_Page_Name"] = objListEntry.MasterPage.Name;//Master_Page_Name 
                                   listItem.Update();
                                   intTemplateID = Convert.ToInt32(listItem["Template_ID"]);
                               }

                               web.AllowUnsafeUpdates = false;
                               objCommonDAL = new CommonDAL();
                               objCommonDAL.UpdateListAuditHistory(siteUrl, auditListName, objListEntry.MasterPage.RowId,
                                   userName, actionPerformed);
                           }
                       }
                   }
               });
            return intTemplateID;
        }

        /// <summary>
        /// Gets the details of the selected Template
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="listName">List Name (DWB Template).</param>
        /// <param name="queryString">CAML Query.</param>
        /// <returns>List Entry object with TemplateDetails property values set.</returns>
        internal ListEntry GetTemplateDetail(string siteUrl, string listName, string queryString)
        {
            ListEntry objListEntry = null;
            DataTable objListItems = null;
            DataRow objListRow;

            TemplateDetails objTemplateDetails = null;
            try
            {
                objCommonDAL = new CommonDAL();
                objListItems = objCommonDAL.ReadList(siteUrl, listName, queryString);

                if (objListItems != null)
                {
                    objListEntry = new ListEntry();
                    objTemplateDetails = new TemplateDetails();
                    for (int index = 0; index < objListItems.Rows.Count; index++)
                    {
                        objListRow = objListItems.Rows[index];
                        /// Title column renamed to "Template_Name". Internal name remains as "Title"
                        objTemplateDetails.Title = objListRow["Title"].ToString();
                        /// 
                        objTemplateDetails.AssetType = objListRow["Asset_Type"].ToString();
                        objTemplateDetails.Terminated = objListRow["Terminate_Status"].ToString();
                        objTemplateDetails.RowId = Int32.Parse(objListRow["ID"].ToString());
                        objListEntry.TemplateDetails = objTemplateDetails;
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

        /// <summary>
        /// Gets the Master Pages for the selected Template
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="listName">List Name (DWB Template).</param>
        /// <param name="queryString">CAML Query.</param>
        /// <param name="viewFields">View Fields.</param>
        /// <returns>List Entry object with TemplateConfiguration values assigned.</returns>
        /// <exception cref="">Handled in calling method.</exception>
        internal ListEntry GetMasterPageDetail(string siteUrl, string listName, string queryString, string viewFields)
        {
            ListEntry objListEntry = null;

            SPSecurity.RunWithElevatedPrivileges(delegate()
               {
                   using (SPSite site = new SPSite(siteUrl))
                   {
                       using (SPWeb web = site.OpenWeb())
                       {
                           web.AllowUnsafeUpdates = true;
                           SPList list = web.Lists[listName];
                           SPQuery query = new SPQuery();
                           query.Query = queryString;
                           query.ViewFields = viewFields;

                           SPListItemCollection listItems = list.GetItems(query);
                           if (listItems != null)
                           {
                               objListEntry = new ListEntry();
                               MasterPageDetails objMasterPageDetails = new MasterPageDetails();
                               foreach (SPListItem item in listItems)
                               {
                                   objMasterPageDetails.RowId = Convert.ToInt32(item["ID"]);//ID
                                   objMasterPageDetails.AssetType = Convert.ToString(item["Asset_Type"]);//Asset_Type 
                                   objMasterPageDetails.ConnectionType = Convert.ToString(item["Connection_Type"]);// Connection_Type 
                                   objMasterPageDetails.PageOwner = Convert.ToString(item["Page_Owner"]); //Page_Owner 
                                   objMasterPageDetails.PageSequence = Convert.ToInt32(item["Page_Sequence"]);//Page_Sequence
                                   objMasterPageDetails.PageURL = Convert.ToString(item["Page_URL"]);//Page_URL 
                                   objMasterPageDetails.SignOffDiscipline = Convert.ToString(item["Discipline"]);//Discipline
                                   objMasterPageDetails.SOP = Convert.ToString(item["Standard_Operating_Procedure"]);//Standard_Operating_Procedure 
                                   objMasterPageDetails.TemplateTitle = Convert.ToString(item["Page_Title_Template"]);//Page_Title_Template 
                                   objMasterPageDetails.Templates = Convert.ToString(item["Template_ID"]);//Template_ID 
                                   objMasterPageDetails.Name = Convert.ToString(item["Master_Page_Name"]);//Master_Page_Name 
                                   objMasterPageDetails.MasterPageID = Convert.ToString(item["Master_Page_ID"]);//Master_Page_ID
                                   objListEntry.MasterPage = objMasterPageDetails;
                               }
                           }
                           web.AllowUnsafeUpdates = false;
                       }
                   }
               });

            return objListEntry;
        }

        #endregion Methods
    }
}

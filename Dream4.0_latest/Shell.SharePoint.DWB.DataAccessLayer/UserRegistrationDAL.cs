#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: UserRegistrationDAL.cs
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Shell.SharePoint.DWB.Business.DataObjects;
using Microsoft.SharePoint;
using System.Web;

namespace Shell.SharePoint.DWB.DataAccessLayer
{
    /// <summary>
    /// Data Access Layer class for Maintain User, Add/Edit User, Privileges
    /// </summary>
    public class UserRegistrationDAL
    {
        #region Declartions
        private const string AUDIT_ACTION_CREATION = "1";
        private const string AUDIT_ACTION_UPDATION = "2";
        private const string AUDIT_ACTION_ACTIVATE = "3";
        private const string AUDIT_ACTION_TERMINATE = "4";

        private const string TEAMSTAFFLIST = "DWB Team Staff";

        #endregion Declartions

        #region  Internal Methods

        /// <summary>
        /// Get the details of the selected User
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="selectedID">selected User ID.</param>
        /// <param name="listName">List Name.</param>
        /// <returns>Object of ListEntry.</returns>
        /// <exception cref="">Handled in calling method.</exception>
        internal ListEntry GetUserDetails(string siteUrl, string selectedID, string listName)
        {
            ListEntry objListEntry = null;
            SPList list;
            SPListItem objListItem;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        list = web.Lists[listName];

                        string strCAMLQuery = @"<Where><Eq><FieldRef Name='ID'/><Value Type='Counter'>" + selectedID + "</Value></Eq></Where>";
                        string strViewFields = @"<FieldRef Name='DWBUserName'/><FieldRef Name='Windows_User_ID'/><FieldRef Name='Discipline'/><FieldRef Name='Terminate_Status'/><FieldRef Name='ID'/><FieldRef Name='Privileges'/>";

                        SPQuery query = new SPQuery();
                        query.Query = strCAMLQuery;
                        query.ViewFields = strViewFields;

                        SPListItemCollection listItemCollection = list.GetItems(query);

                        if (listItemCollection != null && listItemCollection.Count > 0)
                        {
                            objListItem = listItemCollection[0];

                            if (objListItem != null)
                            {
                                objListEntry = new ListEntry();
                                UserDetails objUserDetails = new UserDetails();
                                objUserDetails.UserName = Convert.ToString(objListItem["DWBUserName"]);
                                objUserDetails.WindowUserID = Convert.ToString(objListItem["Windows_User_ID"]);
                                objUserDetails.Terminated = Convert.ToString(objListItem["Terminate_Status"]);
                                /// objListItem["Discipline"] - value is stored in this format [ID;#Value - Eg: 2;#Reservoir Engineering]
                                /// If multiple selection allowed [2;#Reservoir Engineering;#1;#Production Technology]
                                string strDiscipline = Convert.ToString(objListItem["Discipline"]);
                                string[] splitter = { ";#" };
                                string[] output = strDiscipline.Split(splitter, StringSplitOptions.None);
                                if (output != null && output.Length == 2)
                                {
                                    objUserDetails.DisciplineID = output[0];
                                    objUserDetails.Discipline = output[1];
                                }
                                objUserDetails.Privileges = Convert.ToString(objListItem["Privileges"]);
                                objUserDetails.PrivilegeCode = Convert.ToString(objListItem["Privileges"]);
                                objUserDetails.RowId = Convert.ToInt32(objListItem["ID"]);

                                objListEntry.UserDetails = objUserDetails;
                            }
                        }
                    }
                }
            });
            return objListEntry;
        }

        /// <summary>
        /// Get the Discipline details of the Selected user
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="selectedID">Selected User ID.</param>
        /// <param name="listName">List Name.</param>
        /// <returns>Object of ListEntry.</returns>
        /// <exception cref="">Handled in calling method.</exception>
        internal UserDetails GetUserDescipline(string siteUrl, string windowsUserID, string listName)
        {
            UserDetails objUserDetails = null;
            SPList list;
            SPListItem objListItem;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        list = web.Lists[listName];

                        string strCAMLQuery = @"<Where><Eq><FieldRef Name='Windows_User_ID'/><Value Type='Text'>" + windowsUserID + "</Value></Eq></Where>";
                        string strViewFields = @"<FieldRef Name='DWBUserName'/><FieldRef Name='Windows_User_ID'/><FieldRef Name='Discipline'/><FieldRef Name='Terminate_Status'/><FieldRef Name='ID'/><FieldRef Name='Privileges'/>";

                        SPQuery query = new SPQuery();
                        query.Query = strCAMLQuery;
                        query.ViewFields = strViewFields;

                        SPListItemCollection listItemCollection = list.GetItems(query);

                        if (listItemCollection != null && listItemCollection.Count > 0)
                        {
                            objListItem = listItemCollection[0];

                            if (objListItem != null)
                            {                               
                                objUserDetails = new UserDetails();
                                objUserDetails.UserName = Convert.ToString(objListItem["DWBUserName"]);
                                objUserDetails.WindowUserID = Convert.ToString(objListItem["Windows_User_ID"]);
                                objUserDetails.Terminated = Convert.ToString(objListItem["Terminate_Status"]);
                                /// objListItem["Discipline"] - value is stored in this format [ID;#Value - Eg: 2;#Reservoir Engineering]
                                /// If multiple selection allowed [2;#Reservoir Engineering;#1;#Production Technology]
                                string strDiscipline = Convert.ToString(objListItem["Discipline"]);
                                string[] splitter = { ";#" };
                                string[] output = strDiscipline.Split(splitter, StringSplitOptions.None);
                                if (output != null && output.Length == 2)
                                {
                                    objUserDetails.DisciplineID = output[0];
                                    objUserDetails.Discipline = output[1];
                                }
                                objUserDetails.Privileges = Convert.ToString(objListItem["Privileges"]);
                                objUserDetails.RowId = Convert.ToInt32(objListItem["ID"]);                              
                            }
                        }

                    }
                }
            });
            return objUserDetails;
        }
      
        /// <summary>
        /// Add/Update user details in DWB User list
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="listEntry">ListEntry object.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="actionPerformed">Audit Action.</param>
        /// <returns>True/False.</returns>
        /// <exception cref="">Handled in calling method.</exception>
        internal bool UpdateListEntry(string siteUrl, ListEntry listEntry, string listName,  string actionPerformed)
        {
            bool blnUpdateSuccess = false;
            SPList list;
            SPListItem objListItem;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        list = web.Lists[listName];
                        objListItem = list.Items.Add();

                        if (string.Equals(actionPerformed, AUDIT_ACTION_CREATION))
                        {
                            /// DWBUserName
                            objListItem["DWBUserName"] = listEntry.UserDetails.UserName;

                            /// Windows_User_ID
                            objListItem["Windows_User_ID"] = listEntry.UserDetails.WindowUserID;
                            /// Discipline
                            SPFieldLookupValue lookupField = new SPFieldLookupValue(listEntry.UserDetails.DisciplineID);
                            objListItem["Discipline"] = lookupField;
                            /// Terminate_Status
                            objListItem["Terminate_Status"] = listEntry.UserDetails.Terminated;
                            /// Team
                            objListItem["Team"] = listEntry.UserDetails.Team;
                            objListItem["Privileges"] = listEntry.UserDetails.PrivilegeCode;
                            objListItem.Update();
                            /// Assign the ID of the list item to RowID of UserDetail object and call
                            listEntry.UserDetails.RowId = Int32.Parse(objListItem["ID"].ToString());                            
                            /// Update Rank
                            /// Rank = Max(Rank) of same discipline of new user + 1                            
                            UpdateTeamStaffList(siteUrl, listEntry, TEAMSTAFFLIST);
                        }
                        if (string.Equals(actionPerformed, AUDIT_ACTION_UPDATION))
                        {
                            SPListItem objList = list.GetItemById(listEntry.UserDetails.RowId);
                            objListItem = objList;

                            /// DWBUserName
                            objListItem["DWBUserName"] = listEntry.UserDetails.UserName;

                            /// Windows_User_ID
                            objListItem["Windows_User_ID"] = listEntry.UserDetails.WindowUserID;
                            /// Discipline
                            SPFieldLookupValue lookupField = new SPFieldLookupValue(listEntry.UserDetails.DisciplineID);
                            objListItem["Discipline"] = lookupField;
                            objListItem["Privileges"] = listEntry.UserDetails.PrivilegeCode;
                            objListItem.Update();                           
                            ///Update the DWB Team Staff List - User Name and Dicipline columns                            
                            blnUpdateSuccess = UpdateTeamStaffListUserDetails(siteUrl, listEntry, TEAMSTAFFLIST);
                        }

                        blnUpdateSuccess = true;
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });

            return blnUpdateSuccess;
        }

        /// <summary>
        /// Add/Update the User Privilieges in DWB User list
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="listEntry">ListEntry object.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="actionPerformed">Audit Action.</param>
        /// <returns>True/False.</returns>
        /// <exception cref="">Handled in calling method.</exception>
        internal bool UpdatePrivileges(string siteUrl, ListEntry listEntry, string listName, string actionPerformed)
        {
            bool blnUpdateSuccess = false;
            SPList list;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        list = web.Lists[listName];

                        if (string.Equals(actionPerformed, AUDIT_ACTION_UPDATION))
                        {
                            SPListItem objListItem = list.GetItemById(listEntry.UserDetails.RowId);

                            /// Privileges
                            objListItem["Privileges"] = listEntry.UserDetails.Privileges;
                            objListItem.Update();
                            blnUpdateSuccess = true;
                        }

                        web.AllowUnsafeUpdates = false;
                    }
                }
            });
            return blnUpdateSuccess;
        }

        /// <summary>
        /// Updates the DWB Team Staff list when one or more teams are selected while creating user
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="listEntry">ListEntry object.</param>
        /// <param name="listName">List Name.</param>
        /// <returns>True/False.</returns>
        /// <exception cref="">Handled in calling method.</exception>
        internal bool UpdateTeamStaffList(string siteUrl, ListEntry listEntry, string listName)
        {
            bool blnUpdateSuccess = false;
            SPList list;
            string strUserID = string.Empty;
            string strWindowsUserID = string.Empty;
            string strDiscipline = string.Empty;
            string strUserName = string.Empty;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        list = web.Lists[listName];
                        /// Foreach Team in Teams collection, update the DWB Team Staff list
                        if (listEntry != null)
                        {
                            if (listEntry.UserDetails != null)
                            {
                                strUserID = listEntry.UserDetails.RowId.ToString();
                                strDiscipline = listEntry.UserDetails.Discipline;
                                strWindowsUserID = listEntry.UserDetails.WindowUserID;
                                strUserName = listEntry.UserDetails.UserName;
                            }
                            if (listEntry.Teams != null && listEntry.Teams.Count > 0)
                            {
                                SPQuery query = new SPQuery();
                                SPListItemCollection listItemCollection = null;
                                SPListItem listItem = null;
                                string strCAMLQuery = string.Empty;
                                string strViewFields = string.Empty;
                                SPQuery rankQuery = new SPQuery();
                                SPListItemCollection itemCollectionRank = null;
                                foreach (TeamDetails objTeamDetails in listEntry.Teams)
                                {
                                     query = new SPQuery();                                  
                                    /// Query the "DWB Team Staff" list for the selected team and user discipline
                                    /// if a user for the selected discipline is not available, start rank = 1
                                    /// else append the rank                                    
                                    query.Query = @"<Where><And><Eq><FieldRef Name='Team_ID'/><Value Type='Number'>" + objTeamDetails.RowId + "</Value></Eq><Eq><FieldRef Name='User_ID'/><Value Type='Number'>" + strUserID + "</Value></Eq></And></Where>";

                                    listItemCollection = list.GetItems(query);
                                    
                                    if (listItemCollection != null && listItemCollection.Count > 0)
                                    {
                                        /// The Staff in this team alreay exist
                                        listItem = listItemCollection[0];
                                        listItem["Team_ID"] = objTeamDetails.RowId;
                                        listItem["User_ID"] = Int32.Parse(strUserID);
                                        listItem["Title"] = strUserName;
                                        listItem["Discipline"] = strDiscipline;
                                        listItem.Update();
                                        blnUpdateSuccess = true;
                                    }
                                    else
                                    {
                                        /// Add the entry to DWB Team Staff list
                                        listItem = list.Items.Add();
                                        listItem["Team_ID"] = objTeamDetails.RowId;
                                        listItem["User_ID"] = Int32.Parse(strUserID);
                                        listItem["Title"] = strUserName;
                                        listItem["Discipline"] = strDiscipline;
                                        /// To update the rank Rank = Max(Rank) of same discipline of new user + 1
                                        strCAMLQuery = string.Empty;
                                        strCAMLQuery = @"<OrderBy><FieldRef Name='User_Rank' Ascending='FALSE'/></OrderBy><Where><And><And><Eq><FieldRef Name='Team_ID'/><Value Type='Number'>" + objTeamDetails.RowId + "</Value></Eq><Eq><FieldRef Name='Discipline'/><Value Type='Text'>" + strDiscipline + "</Value></Eq></And><IsNotNull><FieldRef Name='User_Rank' /></IsNotNull></And></Where>";
                                        strViewFields = string.Empty;
                                        strViewFields = "<FieldRef Name='User_Rank'/><FieldRef Name='Discipline'/><FieldRef Name='ID'/>";
                                        rankQuery = new SPQuery();
                                        rankQuery.Query = strCAMLQuery;
                                        rankQuery.ViewFields = strViewFields;
                                        itemCollectionRank = list.GetItems(rankQuery);
                                        if (itemCollectionRank != null && itemCollectionRank.Count > 0)
                                        {
                                            if (itemCollectionRank[0]["User_Rank"] != null)
                                            {
                                                listItem["User_Rank"] = Int32.Parse(itemCollectionRank[0]["User_Rank"].ToString()) + 1;
                                            }
                                        }
                                        else  /// To update the rank Rank = 1 (no user for the selected discipline exist in selected team)
                                        {
                                            listItem["User_Rank"] = 1;
                                        }
                                        listItem.Update();
                                        blnUpdateSuccess = true;
                                    }
                                }
                            }
                        }

                    }
                }
            });
            return blnUpdateSuccess;
        }

        /// <summary>
        /// Updates the change done to User Name, Discipline in DWB User List to DWB Team Staff list
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="listEntry">ListEntry object with UserDetails assigned.</param>
        /// <param name="listName">List Name.(DWB Team Staff List).</param>
        /// <returns>bool</returns>
        /// <exception cref="">Handled in calling method.</exception>
        internal bool UpdateTeamStaffListUserDetails(string siteUrl, ListEntry listEntry, string listName)
        {
            bool blnUpdateSuccess = false;
            SPList list;
            string strListGuid = string.Empty;
            StringBuilder strMethodBuilder = new StringBuilder();
            string strBatchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
  "<ows:Batch OnError=\"Return\">{0}</ows:Batch>";

            string strMethodFormat = "<Method ID=\"{0}\">" + /// ID of the list item to be updated
             "<SetList>{1}</SetList>" + /// List Guid
             "<SetVar Name=\"Cmd\">Save</SetVar>" +
             "<SetVar Name=\"ID\">{2}</SetVar>" + /// ID of the list item to updated
             "<SetVar Name=\"urn:schemas-microsoft-com:office:office#User_ID\">{3}</SetVar>" + /// Column name and value to updated
             "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Title\">{4}</SetVar>" + /// Column name and value to updated
             "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Discipline\">{5}</SetVar>" + /// Column name and value to updated
            "</Method>";
            string strBatchXML = string.Empty;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;
                            list = web.Lists[listName];
                            strListGuid = list.ID.ToString();

                            if (listEntry != null)
                            {
                                string strUserID = string.Empty;

                                if (listEntry.UserDetails != null)
                                {
                                    strUserID = listEntry.UserDetails.RowId.ToString();
                                    /// Get all the Team Staffs where the User_ID = Row ID of the user
                                    SPQuery query = new SPQuery();
                                    query.Query = @"<Where><Eq><FieldRef Name='User_ID'/><Value Type='Number'>" + strUserID + "</Value></Eq></Where>";
                                    query.ViewFields = @"<FieldRef Name='User_ID'/><FieldRef Name='ID'/><FieldRef Name='Title'/><FieldRef Name='Discipline'/>";
                                    SPListItemCollection listItemCollection = list.GetItems(query);
                                    /// Update using Batch update method
                                    foreach (SPListItem listItem in listItemCollection)
                                    {
                                        strMethodBuilder.AppendFormat(strMethodFormat, listItem["ID"].ToString(), strListGuid, listItem["ID"].ToString(), listEntry.UserDetails.RowId.ToString(), listEntry.UserDetails.UserName, listEntry.UserDetails.Discipline);
                                    }

                                    /// Form the Batch process XML
                                    strBatchXML = string.Format(strBatchFormat, strMethodBuilder.ToString());
                                    web.ProcessBatchData(strBatchXML);
                                    blnUpdateSuccess = true;
                                }
                            }

                            web.AllowUnsafeUpdates = false;
                        }
                    }
                });
            }
            catch
            {
                throw;
            }
            return blnUpdateSuccess;
        }
        #endregion Internal Methods
    }
}

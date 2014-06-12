#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: TeamStaffRegistrationDAL.cs
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
    /// Data Access Layer class for Team Registration and Staff Registration moducles
    /// </summary>
    public class TeamStaffRegistrationDAL
    {
        #region Declartions
        private const string AUDIT_ACTION_CREATION = "1";
        private const string AUDIT_ACTION_UPDATION = "2";
        private const string AUDIT_ACTION_ACTIVATE = "3";
        private const string AUDIT_ACTION_TERMINATE = "4";
        protected char[] SPLITTER = { ';' };
        private const string DWBUSERLIST = "DWB User";
        private const string DWBTEAMLIST = "DWB Team";
        #endregion Declartions

        #region  Internal Methods
        /// <summary>
        /// Gets the details of the selected Team
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="selectedID">Team ID.</param>
        /// <param name="listName">List Name.</param>
        /// <exception>Handled in calling class</exception>
        /// <returns>List Entry object.</returns>
        internal ListEntry GetTeamDetails(string siteUrl, string selectedID, string listName)
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
                        list = web.Lists[listName];

                        string strCAMLQuery = @"<Where><Eq><FieldRef Name='ID'/><Value Type='Counter'>" + selectedID + "</Value></Eq></Where>";
                        string strViewFields = @"<FieldRef Name='Title'/><FieldRef Name='Asset_Owner'/><FieldRef Name='Terminate_Status'/><FieldRef Name='ID'/>";

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
                                TeamDetails objTeamDetails = new TeamDetails();
                                objTeamDetails.TeamName = Convert.ToString(objListItem["Title"]);
                                objTeamDetails.AssetOwner = Convert.ToString(objListItem["Asset_Owner"]);
                                objTeamDetails.Terminated = Convert.ToString(objListItem["Terminate_Status"]);
                                objTeamDetails.RowId = Convert.ToInt32(objListItem["ID"]);

                                objListEntry.TeamDetails = objTeamDetails;
                            }
                        }
                    }
                }
            });

            return objListEntry;
        }

        /// <summary>
        /// Gets the Staffs for the selected Team
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="teamID">TeamID.</param>
        /// <param name="listName">List Name to get the Staffs.</param>
        /// <returns>DataTable.</returns>
        internal DataTable GetStaffs(string siteUrl, string teamID, string listName)
        {
            DataTable dtStaffList = null;
            SPList list;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            list = web.Lists[listName];

                            string strCAMLQuery = @"<OrderBy><FieldRef Name='Title' Ascending='TRUE'/></OrderBy><Where><Eq><FieldRef Name='Team_ID'/><Value Type='Number'>" + teamID + "</Value></Eq></Where>";
                            /// Title column is renamed as User_Name
                            string strViewFields = @"<FieldRef Name='ID'/><FieldRef Name='Title'/><FieldRef Name='Team_ID'/><FieldRef Name='User_ID'/><FieldRef Name='User_Rank'/>";

                            SPQuery query = new SPQuery();
                            query.Query = strCAMLQuery;
                            query.ViewFields = strViewFields;

                            SPListItemCollection listItemCollection = list.GetItems(query);
                            if (listItemCollection != null && listItemCollection.Count > 0)
                            {
                                dtStaffList = listItemCollection.GetDataTable();

                            }
                        }
                    }
                });
            }
            finally
            {
                if (dtStaffList != null)
                {
                    dtStaffList.Dispose();
                }
            }
            return dtStaffList;
        }
       
        /// <summary>
        /// Get all the Unique Disciplines in a Team
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="teamID">TeamID.</param>
        /// <param name="listName">ListName.</param>
        /// <returns>DataTable.</returns>
        internal DataTable GetDisciplinesInTeam(string siteUrl, string teamID, string listName)
        {
            DataTable dtDiscipline = null;
            DataTable dtStaffs = null;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
               {
                   using (SPSite site = new SPSite(siteUrl))
                   {
                       using (SPWeb web = site.OpenWeb())
                       {
                           SPList list = web.Lists[listName];

                           string strCAMLQuery = string.Empty;
                           string strViewFields = string.Empty;
                           strCAMLQuery = @"<Where><Eq><FieldRef Name='Team_ID'/><Value Type='Number'>" + teamID + "</Value></Eq></Where>";
                           strViewFields = @"<FieldRef Name='ID'/><FieldRef Name='Discipline'/><FieldRef Name='Title'/>";

                           SPQuery query = new SPQuery();
                           query.Query = strCAMLQuery;
                           query.ViewFields = strViewFields;

                           SPListItemCollection listItems = list.GetItems(query);
                           if (listItems != null)
                           {
                               dtStaffs = listItems.GetDataTable();

                               string[] strColumns = { "Discipline" };
                               if (dtStaffs != null)
                               {
                                   dtDiscipline = dtStaffs.DefaultView.ToTable("Discipline", true, strColumns);                                  
                               }
                           }
                       }
                   }
               });
            }
            finally
            {
                if (dtDiscipline != null)
                {
                    dtDiscipline.Dispose();
                }
                if (dtStaffs != null)
                {
                    dtStaffs.Dispose();
                }
            }
            return dtDiscipline;
        }

        /// <summary>
        /// Get all the staffs in a team based on the selected Discipline
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="teamID">Team ID.</param>
        /// <param name="discipline">Discipline.</param>
        /// <param name="listName">List Name.</param>
        /// <returns>DataTable.</returns>
        internal DataTable GetStaffsForDiscipline(string siteUrl, string teamID, string discipline, string listName)
        {
            DataTable dtStaffs = null;

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
               {
                   using (SPSite site = new SPSite(siteUrl))
                   {
                       using (SPWeb web = site.OpenWeb())
                       {
                           SPList list = web.Lists[listName];

                           string strCAMLQuery = string.Empty;
                           string strViewFields = string.Empty;
                           strCAMLQuery = @"<OrderBy><FieldRef Name='User_Rank' Ascending='TRUE'/></OrderBy><Where><And><Eq><FieldRef Name='Team_ID'/><Value Type='Number'>" + teamID + "</Value></Eq><Eq><FieldRef Name='Discipline'/><Value Type='Text'>" + discipline + "</Value></Eq></And></Where>";
                           strViewFields = @"<FieldRef Name='ID'/><FieldRef Name='User_Rank'/><FieldRef Name='Title'/><FieldRef Name='Discipline'/>";

                           SPQuery query = new SPQuery();
                           query.Query = strCAMLQuery;
                           query.ViewFields = strViewFields;

                           SPListItemCollection listItems = list.GetItems(query);
                           if (listItems != null)
                           {
                               dtStaffs = listItems.GetDataTable();                               
                           }
                       }
                   }
               });
            }
            finally
            {
                if (dtStaffs != null)
                {
                    dtStaffs.Dispose();
                }
            }
            return dtStaffs;
        }

        /// <summary>
        /// Add/Update the Team details
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="listEntry">List Entry object.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="actionPerformed">Audit Action.</param>
        /// <returns>True/False.</returns>
        internal bool UpdateTeamListEntry(string siteUrl, ListEntry listEntry, string listName, string actionPerformed)
        {
            bool updateSuccess = false;
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
                            /// Team_Name [Internal name is Title]
                            objListItem["Title"] = listEntry.TeamDetails.TeamName;

                            /// Asset_Owner
                            objListItem["Asset_Owner"] = listEntry.TeamDetails.AssetOwner;
                            /// Terminate_Status
                            objListItem["Terminate_Status"] = listEntry.TeamDetails.Terminated;
                        }
                        else if (string.Equals(actionPerformed, AUDIT_ACTION_UPDATION))
                        {
                            SPListItem objList = list.GetItemById(listEntry.TeamDetails.RowId);
                            objListItem = objList;

                            /// Team_Name [Internal name is Title]
                            objListItem["Title"] = listEntry.TeamDetails.TeamName;

                            /// Asset_Owner
                            objListItem["Asset_Owner"] = listEntry.TeamDetails.AssetOwner;
                        }

                        objListItem.Update();
                        updateSuccess = true;
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });
            return updateSuccess;
        }

        /// <summary>
        /// Add/Remove the staffs in a team
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="listEntry">List Entry object.</param>
        /// <param name="listName">List Name.</param>       
        /// <returns>True/False.</returns>
        internal void UpdateStaffsInTeam(string siteUrl, ListEntry listEntry, string listName)
        {
            SPList list;
            DataTable dtUserCollection = null;
            string strTeamName = string.Empty;
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
                            /// Retrieve the existing staff for the Team and Remove the Staff which are not in ListEntry.Staff collection
                            SPQuery query = new SPQuery();
                            SPListItemCollection listItemCollection = null;
                            List<Int32> itemTobedeleted = new List<int>();

                            string strTeamID = string.Empty;
                            if (listEntry != null)
                            {
                                if (listEntry.TeamDetails != null)
                                {
                                    strTeamID = listEntry.TeamDetails.RowId.ToString();
                                    strTeamName = listEntry.TeamDetails.TeamName;

                                    query.Query = @"<Where><Eq><FieldRef Name='Team_ID'/><Value Type='Number'>" + strTeamID + "</Value></Eq></Where>";

                                    listItemCollection = list.GetItems(query);
                                }

                                if (listItemCollection != null)
                                {
                                    bool staffExists = false;
                                    string strItemID = string.Empty;
                                    string[] strUserID = null;
                                    foreach (SPListItem listItem in listItemCollection)
                                    {
                                        staffExists = false;
                                        strItemID = listItem["User_ID"].ToString();
                                        if (listEntry.Staffs != null && listEntry.Staffs.Count > 0)
                                        {
                                            foreach (StaffDetails objStaffDetails in listEntry.Staffs)
                                            {
                                                strUserID = null;
                                                strUserID = objStaffDetails.RowID.Split(SPLITTER, StringSplitOptions.None);
                                                if (strUserID.Length >= 2)
                                                {
                                                    if (Int32.Parse(strUserID[1]) == Int32.Parse(strItemID))
                                                    {
                                                        staffExists = true;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        if (!staffExists && !itemTobedeleted.Contains(Int32.Parse(listItem["ID"].ToString())))
                                        {
                                            itemTobedeleted.Add(Int32.Parse(listItem["ID"].ToString()));
                                        }
                                    }
                                }                               
                                for (int intIndex = 0; intIndex < itemTobedeleted.Count; intIndex++)
                                {
                                    list.Items.DeleteItemById(itemTobedeleted[intIndex]);
                                }
                                /// Add the listitems for the StaffDetails object which doesn't have concatenation in RowID property
                                if (listEntry.Staffs != null && listEntry.Staffs.Count > 0)
                                {                                   
                                    string strCAMLQuery = string.Empty;
                                    string strViewFields = string.Empty;
                                    string strTempCAML = string.Empty;
                                    SPList userList = web.Lists[DWBUSERLIST];
                                    SPListItem userItem = null;
                                    int intUserID = 0;
                                    string strDiscipline = string.Empty;
                                    string strRankCAMLQuery = string.Empty;
                                    string[] strSplitter = { ";#" };
                                    string[] strOutput = null;
                                    SPQuery rankQuery;
                                    SPListItemCollection itemCollectionRank;
                                    SPListItem listItem;
                                    foreach (StaffDetails objStaffDetails in listEntry.Staffs)
                                    {
                                        if (objStaffDetails.RowID.IndexOf(";") == -1)
                                        {                                           
                                            intUserID = 0;
                                            int.TryParse(objStaffDetails.RowID,out intUserID);
                                            if (intUserID > 0)
                                            {
                                                userItem = userList.GetItemById(intUserID);
                                                /// Query the "DWB Team Staff" list for the selected team and user discipline
                                                /// if a user for the selected discipline is not available, start rank = 1
                                                /// else append the rank                                                
                                                if (userItem != null)
                                                {                                                   
                                                    strDiscipline = Convert.ToString(userItem["Discipline"]);
                                                    strOutput = strDiscipline.Split(strSplitter, StringSplitOptions.None);

                                                    if (strOutput != null && strOutput.Length == 2)
                                                    {
                                                        strRankCAMLQuery = @" <OrderBy><FieldRef Name='User_Rank' Ascending='FALSE'/></OrderBy><Where><And><And><Eq><FieldRef Name='Team_ID' /><Value Type='Number'>" + strTeamID + "</Value></Eq><Eq><FieldRef Name='Discipline' /><Value Type='Text'>" + strOutput[1] + "</Value></Eq></And><IsNotNull><FieldRef Name='User_Rank' /></IsNotNull></And></Where>";
                                                    }
                                                    strViewFields = string.Empty;
                                                    strViewFields = "<FieldRef Name='User_Rank'/><FieldRef Name='Discipline'/><FieldRef Name='ID'/>";
                                                    rankQuery = new SPQuery();
                                                    rankQuery.Query = strRankCAMLQuery;
                                                    rankQuery.ViewFields = strViewFields;
                                                    itemCollectionRank = null;
                                                    itemCollectionRank = list.GetItems(rankQuery);

                                                    /// Staff Properties - Team_ID,User_ID,Title(User_Name),Discipline,User_Rank
                                                    listItem = list.Items.Add();
                                                    listItem["Team_ID"] = Int32.Parse(strTeamID);
                                                    listItem["User_ID"] = Int32.Parse(Convert.ToString(userItem["ID"]));
                                                    listItem["Title"] = Convert.ToString(userItem["DWBUserName"]);
                                                   
                                                    if (strOutput != null && strOutput.Length == 2)
                                                    {
                                                        listItem["Discipline"] = strOutput[1];
                                                    }
                                                    /// If Staff for the selected discipline available update with Max(Rank) + 1
                                                    if (itemCollectionRank != null && itemCollectionRank.Count > 0)
                                                    {
                                                        listItem["User_Rank"] = Int32.Parse(itemCollectionRank[0]["User_Rank"].ToString()) + 1;
                                                    }
                                                    else /// Assign default rank = 1
                                                    {
                                                        listItem["User_Rank"] = 1;
                                                    }
                                                    listItem.Update();
                                                }
                                            }
                                        }
                                    }
                                    /// Re-order the user rank based sort = User Rank
                                    strCAMLQuery = string.Empty;
                                    strCAMLQuery = @"<OrderBy><FieldRef Name='Discipline' /><FieldRef Name='User_Rank' Ascending='True' /></OrderBy><Where><Eq><FieldRef Name='Team_ID' /><Value Type='Number'>" + strTeamID + "</Value></Eq></Where>";
                                    strViewFields = string.Empty;
                                    strViewFields = @"<FieldRef Name='User_Rank'/><FieldRef Name='Discipline'/><FieldRef Name='ID'/><FieldRef Name='Team_ID' />";
                                    DataTable dtTeamStaff = null;
                                    DataTable dtDiscipline = null;
                                    SPQuery teamStaffQuery = new SPQuery();
                                    teamStaffQuery.Query = strCAMLQuery;
                                    teamStaffQuery.ViewFields = strViewFields;
                                    listItemCollection = null;
                                    string strFilterExpr = string.Empty;
                                    string strSortExrp = string.Empty;
                                    listItemCollection = list.GetItems(teamStaffQuery);

                                    if (listItemCollection != null && listItemCollection.Count > 0)
                                    {
                                        dtTeamStaff = listItemCollection.GetDataTable();
                                        string[] strColumns = {"Discipline"};
                                        if(dtTeamStaff != null)
                                            dtDiscipline = dtTeamStaff.DefaultView.ToTable(true, strColumns); 
                                    }
                                    if (dtDiscipline != null && dtDiscipline.Rows.Count > 0)
                                    {
                                        DataRow[] drTeamStaff = null;
                                        listItem = null;
                                        int intRank = 1;
                                       
                                        strSortExrp = "User_Rank ASC";
                                        foreach (DataRow dtRow in dtDiscipline.Rows)
                                        {
                                            strFilterExpr = "Discipline ='" + Convert.ToString(dtRow["Discipline"]) + "'";
                                            /// While converting the list item collection to DataTable, lookup fields holds only Text not ID;#Text 
                                            if (dtTeamStaff != null)
                                                drTeamStaff = dtTeamStaff.Select(strFilterExpr, strSortExrp);
                                            if (drTeamStaff != null)
                                            {
                                                intRank = 1;
                                                foreach (DataRow drStaff in drTeamStaff)
                                                {
                                                    listItem = list.GetItemById(int.Parse(drStaff["ID"].ToString()));
                                                    listItem["User_Rank"] = intRank;
                                                    listItem.Update();
                                                    intRank++;
                                                }
                                            }
                                        }
                                        dtDiscipline.Dispose();
                                        if (dtTeamStaff != null)
                                            dtTeamStaff.Dispose();
                                    }                                   
                                    UpdateUserListForTeam(siteUrl, listEntry, DWBUSERLIST);
                                }
                                else
                                {
                                    UpdateUserListForTeam(siteUrl, listEntry, DWBUSERLIST);
                                 
                                }
                            }// listEntry obj not null check
                            web.AllowUnsafeUpdates = false;
                        } // spWeb ends
                    } // spSite ends
                }); // Elevated Privilages ends
            }           
            finally
            {
                if (dtUserCollection != null)
                    dtUserCollection.Dispose();
            }
          
        }
        
        /// <summary>
        /// Add/Update the Rank of the Staff based in Discipline in a Team 
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="listEntry">List Entry object.</param>
        /// <param name="listName">List Name.</param>     
        /// <exception cref="">Handled in calling class.</exception>
        /// <returns>True/False.</returns>
        internal bool UpdateStaffRank(string siteUrl, ListEntry listEntry, string listName)
        {
            bool updateSuccess = false;
            SPList list;
            string strListGuid = string.Empty;
            StringBuilder strMethodBuilder = new StringBuilder();
            string strBatchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
  "<ows:Batch OnError=\"Return\">{0}</ows:Batch>";

            string strMethodFormat = "<Method ID=\"{0}\">" + /// ID of the list item to be updated
             "<SetList>{1}</SetList>" + /// List Guid
             "<SetVar Name=\"Cmd\">Save</SetVar>" +
             "<SetVar Name=\"ID\">{2}</SetVar>" + /// ID of the list item to updated
             "<SetVar Name=\"urn:schemas-microsoft-com:office:office#User_Rank\">{3}</SetVar>" + /// Column name and value to updated
            "</Method>";
            string strBatchXML = string.Empty;

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
                            string strTeamID = string.Empty;

                            if (listEntry.TeamDetails != null)
                            {
                                strTeamID = listEntry.TeamDetails.RowId.ToString();
                                /// Update using Batch update method
                                foreach (StaffDetails objStaffDetails in listEntry.Staffs)
                                {
                                    strMethodBuilder.AppendFormat(strMethodFormat, objStaffDetails.RowID, strListGuid, objStaffDetails.RowID, objStaffDetails.UserRank);
                                }

                                strBatchXML = string.Format(strBatchFormat, strMethodBuilder.ToString());
                                web.ProcessBatchData(strBatchXML);
                                updateSuccess = true;
                            }
                        }

                        web.AllowUnsafeUpdates = false;
                    }
                }
            });

            return updateSuccess;
        }

        /// <summary>
        /// Update Team column in DWB User list when a staff is added/removed from a team
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="listEntry">List Entry object.</param>
        /// <param name="listName">List Name.</param>        
        /// <exception cref="">Handled in calling method.</exception>
        internal void UpdateUserListForTeam(string siteUrl, ListEntry listEntry, string listName)
        {
            string strTeamName = string.Empty;
            bool staffExists = false;
            List<Int32> itemTobedeleted = new List<int>();

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        SPList list = web.Lists[listName];
                        SPQuery query = new SPQuery();
                        if (listEntry != null)
                        {
                            if (listEntry.TeamDetails != null)
                            {
                                strTeamName = listEntry.TeamDetails.TeamName;
                            }
                            query.Query = @"<Where><Contains><FieldRef Name='Team'/><Value Type='Text'>" + strTeamName + "</Value></Contains></Where>";
                            query.ViewFields = @"<FieldRef Name='ID'/><FieldRef Name='Team'/><FieldRef Name='Windows_User_ID'/>";

                            string[] strUserID = null;
                            SPListItemCollection listItemCollection = list.GetItems(query);
                            /// Identify the user names available in DWB User list for the selected Team but not available in Staffs collection
                            /// Mark the items to be deleted - i.e. the TeamName should be removed from Team column of DWB User list
                            foreach (SPListItem listItem in listItemCollection)
                            {
                                staffExists = false;
                                if (listEntry.Staffs != null)
                                {
                                    foreach (StaffDetails objStaffDetails in listEntry.Staffs)
                                    {
                                        strUserID = null;
                                        strUserID = objStaffDetails.RowID.Split(SPLITTER, StringSplitOptions.None);
                                        if (strUserID.Length >= 2)
                                        {
                                            if (Int32.Parse(strUserID[1]) == Int32.Parse(listItem["ID"].ToString()))
                                            {
                                                staffExists = true;
                                                break;
                                            }
                                        }
                                    }
                                    if (!staffExists && !itemTobedeleted.Contains(Int32.Parse(listItem["ID"].ToString())))
                                    {
                                        itemTobedeleted.Add(Int32.Parse(listItem["ID"].ToString()));
                                    }
                                }
                            }
                            /// Remove the TeamName from the Team column of the marked items
                            string strTeams = string.Empty;
                            int intTeamNameIndex = 0;
                            SPListItem listItemToBeDeleted;
                            for (int intIndex = 0; intIndex < itemTobedeleted.Count; intIndex++)
                            {
                                listItemToBeDeleted = list.GetItemById(itemTobedeleted[intIndex]);
                                strTeams = string.Empty;
                                strTeams = listItemToBeDeleted["Team"].ToString();
                                intTeamNameIndex = -1;
                                intTeamNameIndex = strTeams.IndexOf(strTeamName);
                                if (intTeamNameIndex != -1)
                                {
                                    strTeams = strTeams.Remove(intTeamNameIndex, strTeamName.Length + 1);
                                    listItemToBeDeleted["Team"] = strTeams;
                                    listItemToBeDeleted.Update();
                                }
                            }
                           
                            int intUserID = 0;                                                                                  
                            /// Update the DWB User list Team column for each staff in Staffs collection
                            foreach (StaffDetails objStaffDetails in listEntry.Staffs)
                            {
                                strUserID = null;
                                strUserID = objStaffDetails.RowID.Split(SPLITTER, StringSplitOptions.None);
                                intUserID = 0;
                                if (strUserID.Length >= 2)
                                {
                                    intUserID = Int32.Parse(strUserID[1]);
                                }
                                else
                                {
                                    intUserID = Int32.Parse(objStaffDetails.RowID);
                                }
                                SPListItem listItem = null;
                                strTeams = string.Empty;
                                if (intUserID != 0)
                                {
                                    listItem = list.GetItemById(intUserID);
                                    if (listItem != null)
                                    {
                                        if (listItem["Team"] != null)
                                        {
                                            strTeams = listItem["Team"].ToString();
                                            intTeamNameIndex = strTeams.IndexOf(strTeamName);
                                            if (intTeamNameIndex == -1)
                                            {
                                                strTeams = strTeams + strTeamName + ";";
                                                listItem["Team"] = strTeams;
                                                listItem.Update();
                                            }
                                        }
                                        else
                                        {
                                            strTeams = strTeamName + ";";
                                            listItem["Team"] = strTeams;
                                            listItem.Update();
                                        }
                                    }
                                }
                            }
                        }
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        /// <summary>
        /// Get The Details for the selected Staff Member
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <param name="selectedID"></param>
        /// <param name="listName"></param>
        /// <returns></returns>
        internal StaffDetails GetSelectedStaffDetails(string siteUrl, string selectedID, string listName)
        {
            StaffDetails objStaffData = null;
            SPList list;
            SPListItem objListItem;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using(SPSite site = new SPSite(siteUrl))
                {
                    using(SPWeb web = site.OpenWeb())
                    {
                        list = web.Lists[listName];

                        string strCAMLQuery = @"<Where><Eq><FieldRef Name='ID'/><Value Type='Counter'>" + selectedID + "</Value></Eq></Where>";
                        //string strViewFields = @"<FieldRef Name='Title'/><FieldRef Name='Asset_Owner'/><FieldRef Name='Terminate_Status'/><FieldRef Name='ID'/>";

                        SPQuery query = new SPQuery();
                        query.Query = strCAMLQuery;
                        //query.ViewFields = strViewFields;
                        
                        SPListItemCollection listItemCollection = list.GetItems(query);
                        if(listItemCollection != null && listItemCollection.Count > 0)
                        {
                            objListItem = listItemCollection[0];

                            if(objListItem != null)
                            {
                              
                                objStaffData = new StaffDetails();
                                objStaffData.UserName = Convert.ToString(objListItem["Title"]);
                                objStaffData.TeamID = Convert.ToString(objListItem["Team_ID"]);
                                objStaffData.UserID = Convert.ToString(objListItem["User_ID"]);
                                objStaffData.UserRank = Convert.ToString(objListItem["User_Rank"]);
                                objStaffData.Discipline = Convert.ToString(objListItem["Discipline"]);
                                objStaffData.PRIVILEGE = Convert.ToString(objListItem["Privilege"]);
                                objStaffData.RowID = Convert.ToString(objListItem["ID"]);
                            }
                        }
                    }
                }
            });

            return objStaffData;
        }

        /// <summary>
        /// Updating the Privilege For the selected Staff
        /// Member
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <param name="selectedId"></param>
        /// <param name="listName"></param>
        internal void UpdateStaffPrivilege(string siteUrl, StaffDetails objStaffDeatils, string listName)
        {
            SPList list;
           
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using(SPSite site = new SPSite(siteUrl))
                {
                    using(SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        list = web.Lists[listName];
                        int row = Convert.ToInt32(objStaffDeatils.RowID);
                        SPListItem objListItem = list.GetItemById(row);
                        /// Privileges
                        objListItem["Privilege"] = objStaffDeatils.PRIVILEGE;
                        objListItem.Update();
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
    
        #endregion Internal Methods
}
    }


#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: TeamStaffRegistrationBLL.cs
#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;

using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DWB.DataAccessLayer;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Controller;


namespace Shell.SharePoint.DWB.BusinessLogicLayer
{
    /// <summary>
    /// BLL class to Add/Remove Team,Staffs and Privileges
    /// </summary>
    public class TeamStaffRegistrationBLL
    {
        #region Declarations
        TeamStaffRegistrationDAL objTeamStaffRegistrationDAL;
        const string REPORTSERVICE = "ReportService";
        const string SEARCHNAMEASSETOWNER = "ASSET OWNER";
        const string LIKEOPERATOR = "LIKE";
        const string STAROPERATOR = "*";
        const string CRITERIANAME = "Class";
        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the details of the selected Team
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="selectedID">Team ID.</param>
        /// <param name="listName">List Name.</param>
        /// <exception>Handled in calling class</exception>
        /// <returns>List Entry object.</returns>
        public ListEntry GetTeamDetails(string siteUrl, string selectedID, string listName)
        {
            objTeamStaffRegistrationDAL = new TeamStaffRegistrationDAL();
            return objTeamStaffRegistrationDAL.GetTeamDetails(siteUrl, selectedID, listName);
        }

        /// <summary>
        /// Gets the Staffs for the selected Team
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="teamID">TeamID.</param>
        /// <param name="listName">List Name to get the Staffs.</param>
        /// <returns>DataTable.</returns>
        public DataTable GetStaffs(string siteUrl, string teamID, string listName)
        {
            objTeamStaffRegistrationDAL = new TeamStaffRegistrationDAL();
            return objTeamStaffRegistrationDAL.GetStaffs(siteUrl, teamID, listName);
        }

        /// <summary>
        /// Get all the Unique Disciplines in a Team
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="teamID">TeamID.</param>
        /// <param name="listName">ListName.</param>
        /// <returns>DataTable.</returns>
        public DataTable GetDisciplinesInTeam(string siteUrl, string teamID, string listName)
        {
            objTeamStaffRegistrationDAL = new TeamStaffRegistrationDAL();
            return objTeamStaffRegistrationDAL.GetDisciplinesInTeam(siteUrl, teamID, listName);
        }

        /// <summary>
        /// Get all the staffs in a team based on the selected Discipline
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="teamID">Team ID.</param>
        /// <param name="discipline">Discipline.</param>
        /// <param name="listName">List Name.</param>
        /// <returns>DataTable.</returns>
        public DataTable GetStaffsForDiscipline(string siteUrl, string teamID, string discipline, string listName)
        {
            objTeamStaffRegistrationDAL = new TeamStaffRegistrationDAL();
            return objTeamStaffRegistrationDAL.GetStaffsForDiscipline(siteUrl, teamID, discipline, listName);
        }

        /// <summary>
        /// Add/Update the Team details
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="listEntry">List Entry object.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="actionPerformed">Audit Action.</param>
        /// <returns>True/False.</returns>
        public bool UpdateTeamListEntry(string siteUrl, ListEntry listEntry, string listName, string actionPerformed)
        {
            objTeamStaffRegistrationDAL = new TeamStaffRegistrationDAL();
            return objTeamStaffRegistrationDAL.UpdateTeamListEntry(siteUrl, listEntry, listName, actionPerformed);
        }

        /// <summary>
        /// Add/Remove the staffs in a team
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="listEntry">List Entry object.</param>
        /// <param name="listName">List Name.</param>
        public void UpdateStaffsInTeam(string siteUrl, ListEntry listEntry, string listName)
        {
            objTeamStaffRegistrationDAL = new TeamStaffRegistrationDAL();
            objTeamStaffRegistrationDAL.UpdateStaffsInTeam(siteUrl, listEntry, listName);
        }


        /// <summary>
        /// Add/Update the Rank of the Staff based in Discipline in a Team
        /// </summary>
        /// <param name="siteUrl">Site URL.</param>
        /// <param name="listEntry">List Entry object.</param>
        /// <param name="listName">List Name.</param>
        /// <returns>True/False.</returns>
        public bool UpdateStaffsRank(string siteUrl, ListEntry listEntry, string listName)
        {
            objTeamStaffRegistrationDAL = new TeamStaffRegistrationDAL();
            return objTeamStaffRegistrationDAL.UpdateStaffRank(siteUrl, listEntry, listName);
        }

        /// <summary>
        /// Method to get the Asset Owners from DREAM webservice
        /// </summary>
        /// <returns></returns>
        public XmlDocument GetAssetOwnersFromService()
        {
            ServiceProvider objFactory = new ServiceProvider();
            XmlDocument responseXmlDoc;
            AbstractController objAbstractController;
            RequestInfo objRequestInfo = this.SetRequestInfoToAssetOwner();
            objAbstractController = objFactory.GetServiceManager(REPORTSERVICE);
            responseXmlDoc = objAbstractController.GetSearchResults(objRequestInfo, -1, SEARCHNAMEASSETOWNER, string.Empty, 0);
            return responseXmlDoc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <param name="selectedID"></param>
        /// <param name="listName"></param>
        /// <returns></returns>
        public StaffDetails GetSelectedStaffDetails(string siteUrl, string selectedID, string listName)
        {
            objTeamStaffRegistrationDAL = new TeamStaffRegistrationDAL();
            return objTeamStaffRegistrationDAL.GetSelectedStaffDetails(siteUrl, selectedID, listName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <param name="selectedId"></param>
        /// <param name="listName"></param>
        public void UpdateStaffPrivilege(string siteUrl, StaffDetails objStaffDetails, string listName)
        {
            objTeamStaffRegistrationDAL = new TeamStaffRegistrationDAL();
            objTeamStaffRegistrationDAL.UpdateStaffPrivilege(siteUrl, objStaffDetails, listName);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Method to generate the Request XML objext
        /// </summary>
        /// <returns></returns>
        private RequestInfo SetRequestInfoToAssetOwner()
        {
            /// Sample Request XML
            /// <requestinfo>
            /// <entity property="true">
            /// <criteria name="Class" value="*" operator="LIKE" />
            /// </entity>
            /// </requestinfo>

            RequestInfo objRequestInfo = new RequestInfo();
            objRequestInfo.Entity = new Entity();
            objRequestInfo.Entity.Property = true;
            objRequestInfo.Entity.Criteria = new Criteria();
            objRequestInfo.Entity.Criteria.Name = CRITERIANAME;
            objRequestInfo.Entity.Criteria.Operator = LIKEOPERATOR;
            objRequestInfo.Entity.Criteria.Value = STAROPERATOR;

            return objRequestInfo;
        }
        #endregion
    }
}

#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: AdminBLL.cs
#endregion

using System;
using System.Data;
using System.Text;
using Shell.SharePoint.DWB.DataAccessLayer;
using Shell.SharePoint.DWB.Business.DataObjects;


namespace Shell.SharePoint.DWB.BusinessLogicLayer
{
    /// <summary>
    ///  AdminBLL class.
    /// </summary>
    public class AdminBLL
    {
        #region Declartion

        AdminDAL objListAccessDAL;
        const string SYSTEMPRIVILEGES = "System Privileges";
        const string STAFFPRIVILEGES = "Staff Privileges";
        const string DWBUSERLIST = "DWB User";
        const string DWBTEAMSTAFFLIST = "DWB Team Staff";
        const string DWBBOOKLIST = "DWB Books";
        const string DWBADMIN = "AD";
        const string DWBPAGEOWNER = "PO";
        const string DWBBOOKOWNER = "BO";
        const string DWBUSER = "US";
        #endregion

        #region Public Methods


        /// <summary>
        /// Gets the user privileges based on the privilege type
        /// </summary>
        /// <param name="parentSiteUrl">Site URL.</param>
        /// <param name="strUserName">Windows User ID.</param>
        /// <param name="strpPrivilegeType">Privilege Type.</param>
        /// <returns>DataTable.</returns>
        public DataTable GetDWBPrivileges(string parentSiteUrl, string userid, string privilegeType)
        {
            string strQuery = string.Empty;
            int intUserId = 0;
            DataTable dtPrivileges = null;
            objListAccessDAL = new AdminDAL();
            switch (privilegeType)
            {
                case SYSTEMPRIVILEGES:
                    strQuery = "<Where><And><Eq><FieldRef Name='Windows_User_ID' /> <Value Type='Text'>" + userid + " </Value></Eq><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq></And></Where>";
                    dtPrivileges = objListAccessDAL.ReadList(parentSiteUrl, DWBUSERLIST, strQuery);
                    break;
                case STAFFPRIVILEGES:
                    int.TryParse(userid, out intUserId);
                    strQuery = "<Where><Eq><FieldRef Name='User_ID' /> <Value Type='Number'>" + intUserId + " </Value></Eq></Where>";
                    dtPrivileges = objListAccessDAL.ReadList(parentSiteUrl, DWBTEAMSTAFFLIST, strQuery);
                    break;
                default:
                    break;
            }

            return dtPrivileges;
        }

        /// <summary>
        /// Sets the System and Staff level privileges based on the entries in the datatable.
        /// </summary>
        /// <param name="strSiteURL">The STR site URL.</param>
        /// <param name="systemPrivileges">The system privileges.</param>
        /// <returns>Privilege collection.</returns>
        public Privileges SetPrivilegesObjects(string strSiteURL, DataTable systemPrivileges)
        {
            Privileges objPrivileges = new Privileges();
            string strPrivilege = string.Empty;
            SystemPrivileges objSystemPrivileges = null;
            if (systemPrivileges == null || systemPrivileges.Rows.Count == 0)
            {
                objPrivileges.IsNonDWBUser = true;
                return objPrivileges;
            }
            if (systemPrivileges != null && systemPrivileges.Rows.Count > 0)
            {
                if (systemPrivileges.Rows[0]["Privileges"] != DBNull.Value)
                    strPrivilege = (string)systemPrivileges.Rows[0]["Privileges"];
                if (!string.IsNullOrEmpty(strPrivilege))
                {
                    objSystemPrivileges = this.SetSytemPrivileges(strPrivilege);
                }
                if (objSystemPrivileges != null)
                {
                    objSystemPrivileges.UserRecordID = Convert.ToInt32(systemPrivileges.Rows[0]["ID"].ToString());
                    objSystemPrivileges.Discipline = systemPrivileges.Rows[0]["Discipline"].ToString();
                }

                /// Get the Teams the user is member of
                CommonBLL objCommonBLL = new CommonBLL();
                string strCAMLQuery = string.Empty;
                string strViewFields = string.Empty;

                strCAMLQuery = @"<OrderBy><FieldRef Name='Team_ID' /></OrderBy>
                                                    <Where>                                                    
                                                        <Eq>
                                                            <FieldRef Name='User_ID' />
                                                            <Value Type='Number'>" + systemPrivileges.Rows[0]["ID"].ToString() + "</Value>" +
                                        "</Eq>" +
                                     "</Where>";
                strViewFields = @"<FieldRef Name='Team_ID' /><FieldRef Name='User_ID' /><FieldRef Name='ID' />";
                DataTable dtResultTable = objCommonBLL.ReadList(strSiteURL, DWBTEAMSTAFFLIST, strCAMLQuery, strViewFields);
                StringBuilder sbTeamId = new StringBuilder();
                FocalPoint objFocalPoint = new FocalPoint();
                if (dtResultTable != null && dtResultTable.Rows.Count > 0)
                {
                    foreach (DataRow dtRow in dtResultTable.Rows)
                    {
                        sbTeamId.Append(Convert.ToString(dtRow["Team_ID"]));
                        sbTeamId.Append("|");
                    }
                    objFocalPoint.TeamIDs = sbTeamId.ToString();
                    dtResultTable.Dispose();
                }

                /// Get the Books owned by user
                dtResultTable = null;
                strCAMLQuery = string.Empty;
                strViewFields = string.Empty;
                strCAMLQuery = @"<Where><Eq><FieldRef Name='Owner' /><Value Type='Lookup'>" + systemPrivileges.Rows[0]["Windows_User_ID"].ToString() + "</Value></Eq></Where>";
                strViewFields = @"<FieldRef Name='Owner' /><FieldRef Name='ID' /><FieldRef Name='Title' /><FieldRef Name='Team_ID' />";
                dtResultTable = objCommonBLL.ReadList(strSiteURL, DWBBOOKLIST, strCAMLQuery, strViewFields);
                if (dtResultTable != null && dtResultTable.Rows.Count > 0)
                {
                    sbTeamId = new StringBuilder();
                    foreach (DataRow dtRow in dtResultTable.Rows)
                    {
                        sbTeamId.Append(Convert.ToString(dtRow["Team_ID"]));
                        sbTeamId.Append("|");
                    }

                    objFocalPoint.BookIDs = sbTeamId.ToString();
                    dtResultTable.Dispose();
                }
                objPrivileges.SystemPrivileges = objSystemPrivileges;
                objPrivileges.FocalPoint = objFocalPoint;
            }
            return objPrivileges;
        }

        /// <summary>
        /// Sets the system level privileges based on the privileges stored in the list.
        /// </summary>
        /// <param name="strSytemPrivilege">Containing the privilege values.</param>
        /// <returns>SystemPrivilege collection.</returns>
        private SystemPrivileges SetSytemPrivileges(string systemPrivileges)
        {
            SystemPrivileges objSystemPrivileges = null;
            string[] strSystemPrivileges = null;

            if (!string.IsNullOrEmpty(systemPrivileges))
            {
                strSystemPrivileges = systemPrivileges.Split(';');
            }
            objSystemPrivileges = new SystemPrivileges();
            if (strSystemPrivileges != null)
            {
                for (int intLength = 0; intLength < strSystemPrivileges.Length; intLength++)
                {
                    switch (strSystemPrivileges[intLength])
                    {
                        case DWBADMIN:
                            objSystemPrivileges.AdminPrivilege = true;
                            objSystemPrivileges.DWBUser = true;
                            break;
                        case DWBBOOKOWNER:
                            objSystemPrivileges.BookOwner = true;
                            objSystemPrivileges.DWBUser = true;
                            break;
                        case DWBPAGEOWNER:
                            objSystemPrivileges.PageOwner = true;
                            objSystemPrivileges.DWBUser = true;
                            break;
                        case DWBUSER:
                            objSystemPrivileges.DWBUser = true;
                            break;
                        default:
                            break;
                    }
                }
            }
            return objSystemPrivileges;
        }
        #endregion
    }
}

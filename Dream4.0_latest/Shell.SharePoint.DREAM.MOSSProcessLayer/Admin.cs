#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: Admin.cs
#endregion

using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.Data;
using System.Globalization;
using System.Web;
using Shell.SharePoint.DREAM.Utilities;
using System.IO;
namespace Shell.SharePoint.DREAM.MOSSProcess
{
    /// <summary>
    /// This class is initiated in the Controller.
    /// Moss Process Layer communicates to SharePoint.
    /// This is used for Authorizing the logged in User
    /// </summary>    
    public class Admin
    {
        #region Declaration
        const string ADMIN = "System Administrators";
        const string GLOBALADMIN = "Global Administrators";
        const string USERACCESSREQUESTLIST = "User Access Request";
        const string TEAMACCESSREQUESTLIST = "Team Access Request";
        const string COMPANYCODELIST = "Company Codes";
        const string COUNTRYCODELIST = "Country Codes";

        CommonUtility objUtility = new CommonUtility();
        ActiveDirectoryService objADService = new ActiveDirectoryService();
        #endregion
        #region Public MOSSProcess Methods
        /// <summary>
        /// Determines whether the logged in user is a valid user or not.
        /// </summary>
        /// <param name="parentSiteURL">The parent site URL.</param>
        /// <param name="strUserID">Logged in userID.</param>
        /// <returns>
        /// 	<c>true</c> if the logged in user is a valid User; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValidUser(string parentSiteURL, string userID)
        {
            #region Method Variables
            string strCompanyCode = string.Empty;
            string strCountryCode = string.Empty;
            string[] arrUserDetails = new string[2];
            string strCAMLQuery = string.Empty;
            string strCompanyCountryCode = string.Empty;
            bool blnStatus = false;

            DataTable objListData = null;
            DataTable objCompanyList = null;
            Common objCommon = null;
            #endregion
            try
            {
                objListData = new DataTable();
                objCompanyList = new DataTable();
                objCommon = new Common();

                userID = objUtility.GetUserName();
                try
                {
                    strCompanyCountryCode = objADService.GetCompanyCountryCode(userID).ToString();
                    if (strCompanyCountryCode.Length > 0)
                        arrUserDetails = strCompanyCountryCode.Split('|');
                    else
                        arrUserDetails = null;
                }
                catch (Exception Ex)
                {
                    CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), Ex, 5);
                }
                if (arrUserDetails != null)
                {
                    if (arrUserDetails[0] != null)
                        strCountryCode = arrUserDetails[0];
                    if (arrUserDetails[1] != null)
                        strCompanyCode = arrUserDetails[1];

                    strCAMLQuery = "<OrderBy><FieldRef Name=\"Title\" /></OrderBy><Where><Eq>"
                                      + "<FieldRef Name=\"Title\" /><Value Type=\"Text\">" + strCountryCode
                                      + "</Value></Eq></Where>";
                    objListData = objCommon.ReadList(parentSiteURL, COUNTRYCODELIST, strCAMLQuery);

                    strCAMLQuery = "<OrderBy><FieldRef Name=\"Title\" /></OrderBy><Where><Eq>"
                                      + "<FieldRef Name=\"Title\"/><Value Type=\"Text\">" + strCompanyCode
                                      + "</Value></Eq></Where>";
                    objCompanyList = objCommon.ReadList(parentSiteURL, COMPANYCODELIST, strCAMLQuery);

                    //if both the Country & Company code of logged in user's exist in SP list..
                    if ((objListData.Rows.Count > 0) && (objCompanyList.Rows.Count > 0))
                    {
                        blnStatus = true;
                    }
                    else
                    {
                        objListData.Clear();
                        strCAMLQuery = "<OrderBy><FieldRef Name=\"Title\" /></OrderBy><Where><And><And><Eq>"
                                          + "<FieldRef Name=\"Active\" /><Value Type=\"Text\">Yes</Value></Eq>"
                                          + "<Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">"
                                          + userID + "</Value></Eq></And><Eq><FieldRef Name=\"Access_x0020_Approval_x0020_Stat\" /><Value Type=\"Choice\">Approved</Value></Eq></And></Where>";

                        objListData = objCommon.ReadList(parentSiteURL, USERACCESSREQUESTLIST, strCAMLQuery);
                        if (objListData.Rows.Count > 0)
                        {
                            blnStatus = true;
                        }
                        else
                        {
                            blnStatus = false;
                        }
                    }
                }
                else
                {
                    objListData.Clear();
                    strCAMLQuery = "<OrderBy><FieldRef Name=\"Title\" /></OrderBy><Where><And><And><Eq>"
                                      + "<FieldRef Name=\"Active\" /><Value Type=\"Text\">Yes</Value></Eq>"
                                      + "<Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">"
                                      + userID + "</Value></Eq></And><Eq><FieldRef Name=\"Access_x0020_Approval_x0020_Stat\" /><Value Type=\"Choice\">Approved</Value></Eq></And></Where>";

                    objListData = objCommon.ReadList(parentSiteURL, USERACCESSREQUESTLIST, strCAMLQuery);
                    if (objListData.Rows.Count > 0)
                    {
                        blnStatus = true;
                    }
                    else
                    {
                        blnStatus = false;
                    }
                }
                return blnStatus;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (objListData != null)
                    objListData.Dispose();
                if (objCompanyList != null)
                    objCompanyList.Dispose();
            }
        }
        /// <summary>
        /// Determines whether the logged in user is a site admin or not.
        /// </summary>
        /// <param name="parentSiteURL">The parent site URL.</param>
        /// <returns>
        /// 	<c>true</c> if the logged in user is a site admin; otherwise, <c>false</c>.
        /// </returns>
        internal bool IsAdmin(string parentSiteURL, string userID)
        {
            try
            {
                bool blnIsAdmin = false;
                SPSecurity.RunWithElevatedPrivileges(delegate()
               {
                   using (SPSite site = new SPSite(parentSiteURL))
                   {
                       using (SPWeb spWeb = site.OpenWeb())
                       {
                           //Checking If user is a site collection admin
                           for (int intIndex = 0; intIndex < spWeb.SiteAdministrators.Count; intIndex++)
                           {
                               if (spWeb.SiteAdministrators[intIndex].LoginName.ToUpper() == userID.ToUpper())
                               {
                                   blnIsAdmin = true;
                                   break;
                               }
                           }
                           //Checking If user is a member of Site Administrator group
                           if (!blnIsAdmin)
                               for (int intIndex = 0; intIndex < spWeb.SiteGroups[ADMIN].Users.Count; intIndex++)
                               {
                                   if (spWeb.SiteGroups[ADMIN].Users[intIndex].LoginName.ToUpper() == userID.ToUpper())
                                   {
                                       blnIsAdmin = true;
                                       break;
                                   }
                               }
                       }
                   }
               });
                return blnIsAdmin;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Inserts the access request.
        /// </summary>
        /// <param name="regionID">The region ID.</param>
        /// <param name="purpose">The purpose.</param>
        internal void CreateAccessRequest(string userID, string regionID, string purpose, string teamId, string listName)
        {

            try
            {
                //  using(SPSite newSite = SPContext.Current.Site)
                string strSiteURL = SPContext.Current.Site.Url;
                SPSecurity.RunWithElevatedPrivileges(delegate()
                 {
                     using (SPSite site = new SPSite(strSiteURL))
                     {
                         using (SPWeb web = site.OpenWeb())
                         {
                             SPList list = web.Lists[listName];
                             web.AllowUnsafeUpdates = true;
                             SPListItem listItem = list.Items.Add();

                             listItem["Title"] = userID;
                             listItem["Region"] = regionID;
                             try
                             {
                                 listItem["Email"] = objADService.GetEmailID(userID);
                             }
                             catch
                             {
                                 //In case active directory is down,catch the exception and proceed
                             }
                             if(listName == TEAMACCESSREQUESTLIST)
                             {
                                 listItem["TeamID"] = teamId;
                             }
                             //Commented in DREAM 3.1 Release
                             try
                             {
                                 listItem["Request Date"] = DateTime.Now.ToString(DateTimeFormatInfo.InvariantInfo);
                             }
                             catch
                             {
                                 //In caseof date format exception,catch the exception and proceed
                             }
                             listItem["Purpose"] = purpose;
                             listItem["DisplayName"] = objADService.GetDisplayName(userID);
                             listItem.Update();
                             web.AllowUnsafeUpdates = false;
                         }
                     }
                 });
            }
            catch
            {
                throw;
            }

        }
        #endregion
    }
}

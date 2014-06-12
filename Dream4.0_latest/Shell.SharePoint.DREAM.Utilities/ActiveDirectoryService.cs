#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: ActiveDirectoryService.cs
#endregion

/// <summary> 
/// This class is used to read ActiveDirectory data
/// </summary> 
using System;
using System.DirectoryServices;
using Shell.SharePoint.DREAM.Business.Entities;
using System.Text;
using System.IO;
using Microsoft.SharePoint;

namespace Shell.SharePoint.DREAM.Utilities
{
    //modified by dev
    //start
    /// <summary> 
    /// This enum class is used to contain keys of details need to be fetched from active directory
    /// </summary> 
    internal enum ADPropertyKeys
    {
        //name of variable should map to exact key name of active directory detail which we want to fetch 
        samaccountname, //userID
        displayname,
        department,
        shellggddepartmentnumber, //department code
        company,
        telephonenumber,
        mobile,
        othertelephone,
        shellggdtelephoneextension, //extension no
        mail,
        title, //designation
        location,
        postalcode,
        co    //country
    }
    //end

    /// <summary>
    /// This class is used to read ActiveDirectory data
    /// </summary>
    public class ActiveDirectoryService
    {
        #region Declaration
        static string strADFullPathName = string.Empty;
        static string strADServerName = string.Empty;
        static string strADUserName = string.Empty;
        static string strADPassword = string.Empty;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveDirectoryService"/> class.
        /// </summary>
        public ActiveDirectoryService()
        {
            try
            {
                strADFullPathName = PortalConfiguration.GetInstance().GetKey("ADFullPathName");
                strADServerName = PortalConfiguration.GetInstance().GetKey("ADServerName");
                strADUserName = PortalConfiguration.GetInstance().GetKey("ADUserName");
                strADPassword = PortalConfiguration.GetInstance().GetKey("ADPassword");
            }
            catch(Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveDirectoryService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public ActiveDirectoryService(SPContext context)
        {
            try
            {
                strADFullPathName = PortalConfiguration.GetInstance().GetKey("ADFullPathName", context.Web.Url.ToString());
                strADServerName = PortalConfiguration.GetInstance().GetKey("ADServerName", context.Web.Url.ToString());
                strADUserName = PortalConfiguration.GetInstance().GetKey("ADUserName", context.Web.Url.ToString());
                strADPassword = PortalConfiguration.GetInstance().GetKey("ADPassword", context.Web.Url.ToString());
            }
            catch(Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Gets the company country code of given user.
        /// </summary>
        /// <param name="strUserID">userID.</param>
        /// <returns></returns>
        public String GetCompanyCountryCode(String userID)
        {
            string strCompanyCountryCode = string.Empty;
            try
            {
                strCompanyCountryCode = GetUserDetail(userID, false);
                return strCompanyCountryCode;
            }
            catch(Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <returns></returns>
        private string GetUserDetail(string userID, bool isEmailIDRequired)
        {
            string strResult = string.Empty;
            DirectorySearcher dirSearcher = null;
            DirectoryEntry dirEntry = null;
            SearchResult objResults = null;
            try
            {
                dirEntry = GetDirectoryObject("/" + GetLDAPDomain());
                dirSearcher = new DirectorySearcher(dirEntry);
                dirSearcher.ClientTimeout = TimeSpan.FromSeconds(30);
                dirSearcher.SearchScope = SearchScope.Subtree;
                dirSearcher.Filter = "(SAMAccountName=" + userID + ")";

                objResults = dirSearcher.FindOne();
                if(!(objResults == null))
                {
                    if(isEmailIDRequired)
                    {
                        strResult = objResults.Properties["mail"][0].ToString();
                    }
                    else
                    {
                        strResult = objResults.Properties["c"][0].ToString() + "|" + objResults.Properties["ou"][0].ToString();
                    }
                }
                return strResult;
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if(dirEntry != null)
                    dirEntry.Dispose();
                if(dirSearcher != null)
                    dirSearcher.Dispose();
            }
        }

        /// <summary>
        /// Gets the LDAP domain.
        /// </summary>
        /// <returns></returns>
        private static string GetLDAPDomain()
        {
            StringBuilder strBldrLDAPDomain = null;
            string strDomain = string.Empty;
            try
            {
                strBldrLDAPDomain = new StringBuilder();
                string[] LDAPDC = strADServerName.Split('.');
                for(int intCount = 0; intCount < LDAPDC.GetUpperBound(0) + 1; intCount++)
                {
                    strBldrLDAPDomain.Append("DC=" + LDAPDC[intCount]);
                    if(intCount < LDAPDC.GetUpperBound(0))
                    {
                        strBldrLDAPDomain.Append(",");
                    }
                }
                strDomain = strBldrLDAPDomain.ToString();
                return strDomain;
            }
            catch(Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the directory object.
        /// </summary>
        /// <param name="domainReference">The domain reference.</param>
        /// <returns></returns>
        private static DirectoryEntry GetDirectoryObject(string domainReference)
        {
            DirectoryEntry objDirectoryEntry = null;
            try
            {
                objDirectoryEntry = new DirectoryEntry(strADFullPathName + domainReference, strADUserName, strADPassword, AuthenticationTypes.Secure);
                return objDirectoryEntry;
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if(objDirectoryEntry != null)
                    objDirectoryEntry.Dispose();
            }
        }

        /// <summary>
        /// Gets the Email ID for the current logged in user.
        /// </summary>
        /// <param name="strUserId">logged in userId</param>
        /// <returns>Email id of logged in user</returns>
        public string GetEmailID(string userId)
        {
            string strEmailID = string.Empty;
            try
            {
                strEmailID = GetUserDetail(userId, true);
                return strEmailID;
            }
            catch(Exception)
            {
                throw;
            }
        }


        /// modified by dev for dataowner module
        /// <summary>
        /// This method is used to fetch specific details of  users from active directory
        /// </summary>
        /// <param name="userIDs"> array of userIDs</param>
        /// <returns>collection of users</returns>
        public Users GetUserDetails(string[] userIDs)
        {
            Users objUsers = new Users();
            User objUser;
            DirectorySearcher dirSearcher = null;
            DirectoryEntry dirEntry = null;
            SearchResult objResults = null;
            try
            {
                dirEntry = GetDirectoryObject("/" + GetLDAPDomain());
                dirSearcher = new DirectorySearcher(dirEntry);
                dirSearcher.ClientTimeout = TimeSpan.FromSeconds(30);
                dirSearcher.SearchScope = SearchScope.Subtree;
                for(int intCount = 0; intCount < userIDs.Length; intCount++)
                {
                    dirSearcher.Filter = "(SAMAccountName=" + userIDs[intCount] + ")";

                    objResults = dirSearcher.FindOne();
                    if(!(objResults == null))
                    {
                        objUser = new User();
                        ResultPropertyCollection propColl = objResults.Properties;
                        foreach(string strKey in Enum.GetNames(typeof(ADPropertyKeys)))
                        {
                            try
                            {
                                if((propColl[strKey] != null) && (propColl[strKey][0] != null))
                                {
                                    objUser.AddUserProperty(strKey.ToLower(), propColl[strKey][0].ToString());
                                }
                            }
                            //Added in DREAM 4.0 to handle indexout of bound error for unknown properties
                            catch
                            {
                                objUser.AddUserProperty(strKey.ToLower(), string.Empty);
                            }
                        }
                        objUsers.Add(objUser);
                    }
                }
                return objUsers;
            }
            finally
            {
                if(dirEntry != null)
                    dirEntry.Dispose();
                if(dirSearcher != null)
                    dirSearcher.Dispose();
            }

        }


        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public string GetDisplayName(string userId)
        {
            string strResult = string.Empty;
            DirectorySearcher dirSearcher = null;
            DirectoryEntry dirEntry = null;
            SearchResult objResults = null;
            try
            {
                dirEntry = GetDirectoryObject("/" + GetLDAPDomain());
                dirSearcher = new DirectorySearcher(dirEntry);
                dirSearcher.ClientTimeout = TimeSpan.FromSeconds(30);
                dirSearcher.SearchScope = SearchScope.Subtree;
                dirSearcher.Filter = "(SAMAccountName=" + userId + ")";

                objResults = dirSearcher.FindOne();
                if(!(objResults == null))
                {
                    strResult = objResults.Properties["givenName"][0].ToString() + " " + objResults.Properties["SN"][0].ToString();
                }
                return strResult;
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if(dirEntry != null)
                    dirEntry.Dispose();
                if(dirSearcher != null)
                    dirSearcher.Dispose();
            }

        }

    }
}


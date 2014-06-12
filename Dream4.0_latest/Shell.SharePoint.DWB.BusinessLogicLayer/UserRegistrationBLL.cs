#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: UserRegistrationBLL.cs
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DWB.DataAccessLayer;
using System.Data;

namespace Shell.SharePoint.DWB.BusinessLogicLayer
{
    /// <summary>
    /// Business Logic Layer class for Maintain User, Add/Edit User, Privileges
    /// </summary>
   public class UserRegistrationBLL
   {
       #region Declarations
       UserRegistrationDAL objUserDetailDAL;
       #endregion

       #region Public Methods
       /// <summary>
        /// Add/Update user details in DWB User list
        /// </summary>
        /// <param name="siteUrl">Site URL</param>
        /// <param name="listEntry">ListEntry object</param>
        /// <param name="listName">List Name</param>
        /// <param name="userName">User Name</param>
        /// <param name="actionPerformed">Audit Action</param>
        /// <returns>True/False</returns>
       public bool UpdateListEntry(string siteUrl, ListEntry listEntry, string listName, string actionPerformed)
       {
           objUserDetailDAL = new UserRegistrationDAL();
          return objUserDetailDAL.UpdateListEntry(siteUrl, listEntry, listName, actionPerformed);
       }

       /// <summary>
       /// Add/Update the User Privilieges in DWB User list
       /// </summary>
       /// <param name="siteUrl">Site URL</param>
       /// <param name="listEntry">ListEntry object</param>
       /// <param name="listName">List Name</param>
       /// <param name="actionPerformed">Audit Action</param>
       /// <returns>True/False</returns>
       public bool UpdatePrivileges(string siteUrl, ListEntry listEntry, string listName,  string actionPerformed)
       {
           objUserDetailDAL = new UserRegistrationDAL();
           return objUserDetailDAL.UpdatePrivileges(siteUrl, listEntry, listName, actionPerformed);
       }

       /// <summary>
       /// Get the details of the selected User
       /// </summary>
       /// <param name="siteUrl">Site URL</param>
       /// <param name="selectedID">selected User ID</param>
       /// <param name="listName">List Name</param>
       /// <returns>Object of ListEntry</returns>
       public ListEntry GetUserDetails(string siteUrl, string selectedID, string listName)
       {
           objUserDetailDAL = new UserRegistrationDAL();
           return objUserDetailDAL.GetUserDetails(siteUrl, selectedID, listName);
       }

       /// <summary>
       /// Get the Discipline details of the Selected user
       /// </summary>
       /// <param name="siteUrl">Site URL.</param>
       /// <param name="selectedID">selected User ID.</param>
       /// <param name="listName">List Name.</param>
       /// <returns>Object of ListEntry.</returns>
       public UserDetails GetUserDesicipline(string siteUrl, string windowsUserId, string listName)
       {
           objUserDetailDAL = new UserRegistrationDAL();
           return objUserDetailDAL.GetUserDescipline(siteUrl, windowsUserId, listName);
       }
       #endregion
   }
}

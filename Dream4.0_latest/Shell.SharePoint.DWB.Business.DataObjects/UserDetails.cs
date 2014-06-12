#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: UserDetails.cs 
#endregion

using System;
namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// Gets/Sets the user details.
    /// </summary>
    /// 
    [Serializable]
   public class UserDetails
   {
       #region DECLARATION
       string strUserName = string.Empty;
       string strWindowsUserID = string.Empty;
       string strDiscipline = string.Empty;
       string strDisciplineID = string.Empty;
       string strTeam = string.Empty;
       string strPrivileges = string.Empty;
       string strPrivilegeText = string.Empty;
       string strPrivilegeCode = string.Empty;
       string strTerminated = string.Empty;
       int intRowId;
       
       #endregion DECLARATION

       #region PROPERTIES

       /// <summary>
       /// Gets/Sets User ID.
       /// </summary>
       public int RowId
       {
           get { return intRowId; }
           set { intRowId = value; }
       }

       /// <summary>
       /// Gets/Sets User Terminated or not.
       /// </summary>
       public string Terminated
       {
           get { return strTerminated; }
           set { strTerminated = value; }
       }

       /// <summary>
       /// Gets/Sets User Name.
       /// </summary>
       public string UserName
       {
           get { return strUserName; }
           set { strUserName = value; }
       }

       /// <summary>
       /// Gets/Sets Windows ID of the user.
       /// </summary>
       public string WindowUserID
       {
           get { return strWindowsUserID; }
           set { strWindowsUserID = value; }
       }

       /// <summary>
       /// Gets/Sets Team Name of the User.
       /// </summary>
       public string Team
       {
           get { return strTeam; }
           set { strTeam = value; }
       }

       /// <summary>
       /// Gets/Sets User Privileges (short string separated by ;).
       /// </summary>
       public string Privileges
       {
           get { return strPrivileges; }
           set { strPrivileges = value; }
       }

       /// <summary>
       /// Gets/Sets User Privileges (short string separated by ;).
       /// </summary>
       public string PrivilegeCode
       {
           get { return strPrivilegeCode; }
           set { strPrivilegeCode = value; }
       }

       /// <summary>
       /// Gets/Sets User Privileges (short string separated by ;).
       /// </summary>
       public string PrivilegeText
       {
           get { return strPrivilegeText; }
           set { strPrivilegeText = value; }
       }
       /// <summary>
       /// Gets/Sets User Discipline Text.
       /// </summary>
       public string Discipline
       {
           get { return strDiscipline; }
           set { strDiscipline = value; }
       }

       /// <summary>
       /// Gets/Sets User Discipline ID.
       /// </summary>
       public string DisciplineID
       {
           get { return strDisciplineID; }
           set { strDisciplineID = value; }
       }
       #endregion PROPERTIES
   }
}

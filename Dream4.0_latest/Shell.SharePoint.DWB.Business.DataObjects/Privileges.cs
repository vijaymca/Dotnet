#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: Privileges.cs 
#endregion

using System;

namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// Gets/Sets the User and Staff Privileges.
    /// </summary>
    [Serializable]
   public class Privileges
   {
       #region DECLARATION     
       SystemPrivileges objSystemPrivileges;
       FocalPoint objFocalPoint;
       bool blnIsNonDWBUser;
       #endregion

       #region PROPERTIES

        /// <summary>
       /// Gets/Sets System Privileges.
        /// </summary>
       public SystemPrivileges SystemPrivileges
       {
           get { return objSystemPrivileges; }
           set { objSystemPrivileges = value; }
       }

       public FocalPoint FocalPoint
       {
           get { return objFocalPoint; }
           set { objFocalPoint = value; }
       }
        /// <summary>
        /// Gets/Sets DWB User or not.
        /// </summary>
       public bool IsNonDWBUser
       {
           get { return blnIsNonDWBUser; }
           set { blnIsNonDWBUser = value; }
       }
       #endregion
   }
}

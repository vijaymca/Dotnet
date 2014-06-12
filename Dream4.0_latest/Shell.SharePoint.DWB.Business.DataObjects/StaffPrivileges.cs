#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: StaffPrivileges.cs 
#endregion
using System;


namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// Gets/Sets Privileges of a Team Staff
    /// </summary>
    [Serializable]
   public class StaffPrivileges
   {
       #region DECLARATION
       bool blnRegisterStaff = false;
       bool blnPublishBook = false;
       bool blnSignOff = false;
       #endregion

       #region PROPERTIES

       public bool RegisterStaff
       {
           get { return blnRegisterStaff; }
           set { blnRegisterStaff = value; }
       }
       public bool PublishBook
       {
           get { return blnPublishBook; }
           set { blnPublishBook = value; }
       }
       public bool SignOff
       {
           get { return blnSignOff; }
           set { blnSignOff = value; }
       }
       #endregion
   }
}

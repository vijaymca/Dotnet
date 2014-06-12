#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: ExceptionControl.ascx.cs 
#endregion

using System;
using System.Web;
using System.Data;
using Shell.SharePoint.DREAM.Utilities;


namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// Class displays the appropriate message to the user regarding his permission to DWB
    /// </summary>
    public partial class ExceptionControl : UIHelper
    {

        #region Declarations
        const string ERRORMODESESSIONTIMEOUT = "1";
        const string ERRORMODENOPRIVILEGES = "3";
        #endregion 

        #region Protected Methods
        /// <summary>
        /// Renders the appropriate message based on the User Access 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (string.Equals(HttpContext.Current.Request.QueryString[MODEQUERYSTRING], ERRORMODESESSIONTIMEOUT))
                {
                    spnException.InnerHtml = "Sorry , you do not have enough privileges for this operation. Please log in again to continue.";
                }
                else if (string.Equals(HttpContext.Current.Request.QueryString[MODEQUERYSTRING], ERRORMODENOPRIVILEGES))
                {
                    spnException.InnerHtml = "Sorry , you do not have enough privileges for this operation. Please <a href=\"mailto:" + GetAdminEmailID() + "\">Email us</a>";
                }   
                else
                {
                    //for consistent usage of term eWB2 instead of Digital Well Book, DWB, eWell Book II.
                    spnException.InnerHtml = "Sorry, you are not a registered user of eWB2. Please <a href=\"mailto:" + GetAdminEmailID() + "\">Email us</a>";
                }

            }
        }
        #endregion
    }
}
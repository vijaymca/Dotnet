#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: ConfirmUserPreferences.aspx.cs
#endregion

/// <summary> 
/// This is the ui class to display user preferences saved confirmation message to user
/// </summary> 
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.UI.HtmlControls;
using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.DREAM.Site.UI
{
    /// <summary>
    /// This is the ui class to display user preferences saved confirmation message to user
    /// </summary>
    public partial class ConfirmUserPreferences : System.Web.UI.Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["saved"] != null)
                    {
                        if (string.Equals(Request.QueryString["saved"], "1"))
                            lblMessage.Text = "The user preferences you have selected are successfully saved. The saved preferences will be reflected when you initiate new search.";
                        else if (string.Equals(Request.QueryString["saved"], "0"))
                        {
                            lblMessage.CssClass = "labelMessage";
                            lblMessage.Text = "The user preferences are not saved.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
        }
    }
}

#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : ExceptionMessage.ascx.cs
#endregion

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Shell.SharePoint.DREAM.Site.UI.UserControls
{
    /// <summary>
    /// This is used to display exception message across the site.
    /// </summary>
    public partial class ExceptionMessage : System.Web.UI.UserControl
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string strMessage = Request.QueryString["ExceptionMessage"];
            lblMessage.Text = strMessage;   
        }
    }
}
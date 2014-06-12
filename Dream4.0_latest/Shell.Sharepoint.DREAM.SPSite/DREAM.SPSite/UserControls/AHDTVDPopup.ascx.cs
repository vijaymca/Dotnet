#region Shell Copyright.2010s
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : AHDTVDPopup.ascx.cs
#endregion
using System;
using System.Web.UI;
namespace Shell.SharePoint.DREAM.Site.UI.UserControls
{
    /// <summary>
    /// This user control is used to calculate the AH Depth values    
    /// </summary>
    public partial class AHDTVDPopup :System.Web.UI.UserControl
    {

        /// <summary>
        /// Handles the Click event of the btnPopulateDepth control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void BtnPopulateDepth_Click(object sender, EventArgs e)
        {
            try
            {
                Session["tblConvertsRowsCount"] = null;
                Session["DepthValues"] = null;
                string strDepthValues = txtAHTopDepth.Text + "|" + txtAHBottomDepth.Text + "|" + txtAHDepthInterval.Text + "|MD";
                Session["DepthValues"] = strDepthValues.ToString();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), string.Empty, "javascript:CloseChildAndRefreshParet('AHDTVDConverter.aspx','');", true);               
                // this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "CloseChildAndRefreshParet('AHDTVDConverter.aspx','');", true);

            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
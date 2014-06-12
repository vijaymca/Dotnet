#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: DREAMWindow.cs
#endregion

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;

using Telerik.Web.UI;

using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.WebParts.DREAM.Charts
{
    /// <summary>
    /// Class to render the RadWindow object.
    /// </summary>
    public class DREAMWindow : WebPart
    {
        #region Declaration
        RadWindow objRadWindow = new RadWindow();
        RadScriptManager objRadScriptManager = new RadScriptManager();
        RadWindowManager objRadWindowManager = new RadWindowManager();
        string strCurrentSiteUrl = string.Empty;
        #endregion

        #region Protected Methods
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            if (Page != null && RadScriptManager.GetCurrent(Page) == null)
            {
                objRadScriptManager = new RadScriptManager();
                objRadScriptManager.ID = "objScriptManager";
                Page.Form.Controls.AddAt(0, objRadScriptManager);
            }

            base.OnInit(e);
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            try
            {
                strCurrentSiteUrl = HttpContext.Current.Request.Url.ToString();
                objRadWindowManager = new RadWindowManager();
                objRadWindowManager.ID = "objRadWindowManager";


                objRadWindow = new RadWindow();
                objRadWindow.ID = "objRadWindow";
                objRadWindow.Behaviors = WindowBehaviors.Move | WindowBehaviors.Close | WindowBehaviors.Resize| WindowBehaviors.Maximize| WindowBehaviors.Minimize;
                
                objRadWindow.OnClientClose = "OnClientClose";
                objRadWindow.OnClientCommand = "OnClientCommand";
                objRadWindow.DestroyOnClose = false;
                objRadWindow.AutoSize = false;
                objRadWindow.VisibleTitlebar = true;
                objRadWindow.VisibleStatusbar = false;
                objRadWindow.Opacity = 100;                

                objRadWindowManager.Windows.Add(objRadWindow);
                this.Controls.Add(objRadWindowManager);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strCurrentSiteUrl,ex);
            }
                     
        }

        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            try
            {
                writer.Write("<table>");
                writer.Write("<tr><td>");
                objRadWindowManager.RenderControl(writer);
                writer.Write("</td></tr>");
                writer.Write("</table>");
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strCurrentSiteUrl, ex);
            }
            finally
            {
                if (objRadWindow != null)
                    objRadWindow.Dispose();
                if (objRadWindowManager != null)
                    objRadWindowManager.Dispose();
            }
        }

        #endregion Protected Methods
    }
}

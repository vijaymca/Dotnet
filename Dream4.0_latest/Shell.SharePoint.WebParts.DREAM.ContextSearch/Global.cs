#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : Global.cs
#endregion
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Shell.SharePoint.WebParts.DREAM.ContextSearch
{
    /// <summary>
    /// This Class finds the control which triggered the postback
    /// </summary>
    public class Global : System.Web.HttpApplication
    {
        private System.ComponentModel.IContainer components;

        /// <summary>
        /// Initialization
        /// </summary>
        public Global()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the post back control.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns>System.Web.UI.Control</returns>
        public static System.Web.UI.Control GetPostBackControl(System.Web.UI.Page page)
        {
            Control objControl = null;
            string strCtrlname = page.Request.Params["__EVENTTARGET"];
            if (!string.IsNullOrEmpty(strCtrlname))
            {
                objControl = page.FindControl(strCtrlname);
            }
            // if __EVENTTARGET is null, the control is a button type and we need to 
            // iterate over the form collection to find it
            else
            {
                string strCtrl = String.Empty;
                Control objCtrl = null;
                foreach (string strCtl in page.Request.Form)
                {
                    // handle ImageButton controls ...
                    if (strCtl.EndsWith(".x") || strCtl.EndsWith(".y"))
                    {
                        strCtrl = strCtl.Substring(0, strCtl.Length - 2);
                        objCtrl = page.FindControl(strCtrl);
                    }
                    else
                    {
                        objCtrl = page.FindControl(strCtl);
                    }
                    if (objCtrl is System.Web.UI.WebControls.Button ||
                             objCtrl is System.Web.UI.WebControls.ImageButton)
                    {
                        objControl = objCtrl;
                        break;
                    }
                }
            }
            return objControl;
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
        }

    }
}



#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : UserControlContainer.cs
#endregion
using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebPartPages;

namespace Shell.SharePoint.WebParts.DREAM.UserControlContainer
{
    /// <summary>
    /// Container control to host the ASP.NET user controls
    /// like All advanced search criteria forms,user perfernece and display options
    /// </summary>
    [DefaultProperty("Text"),
        ToolboxData("<{0}:UserControlContainer runat=server></{0}:UserControlContainer>"),
       XmlRoot(Namespace = "Shell.SharePoint.WebParts.DREAM")]
    public class UserControlContainer : Microsoft.SharePoint.WebPartPages.WebPart
    {
        private const string strDefaultText = "";
        private string strUserControl = strDefaultText;
        private Control objControl;

        [Browsable(true),
            Category("User Control"),
            DefaultValue(strDefaultText),
            WebPartStorage(Storage.Personal),
            FriendlyName("User Control (.ascx)"),
            Description("Path to the User Control (.ascx)")]
        public string UserControl
        {
            get
            {
                return strUserControl;
            }

            set
            {
                strUserControl = value;
            }
        }

        /// <summary>
        ///	This method gets the custom tool parts for this Web Part by overriding the
        ///	GetToolParts method of the WebPart base class. You must implement
        ///	custom tool parts in a separate class that derives from 
        ///	Microsoft.SharePoint.WebPartPages.ToolPart. 
        ///	</summary>
        ///<returns>An array of references to ToolPart objects.</returns>
        public override ToolPart[] GetToolParts()
        {
            ToolPart[] objToolParts = new ToolPart[3];
            WebPartToolPart objWPTP = new WebPartToolPart();
            CustomPropertyToolPart objCustomToolPart = new CustomPropertyToolPart();
            objToolParts[0] = objCustomToolPart;
            objToolParts[1] = objWPTP;
            objToolParts[2] = new CopyrightToolpart();

            objWPTP.Expand(Microsoft.SharePoint.WebPartPages.WebPartToolPart.Categories.Appearance);
            objCustomToolPart.Expand("User Control");

            return objToolParts;
        }

        /// <summary>
        /// Load the UserControl
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            try
            {
                if (!string.IsNullOrEmpty(strUserControl))
                {

                    objControl = this.Page.LoadControl(strUserControl);

                }
                else
                {
                    objControl = new LiteralControl(string.Format("To link to content, <a href=\"javascript:MSOTlPn_ShowToolPaneWrapper('{0}','{1}','{2}');\">open the tool pane</a> and then type a URL in the Link text box.", 1, 129, this.ID));
                }
            }
            catch (System.Exception ex)
            {
                objControl = new LiteralControl(string.Format("<b>Error:</b> unable to load {0}<br /><b>Details:</b> {1}", strUserControl, ex.Message));
            }

            if (objControl != null)
            {
                // Add to the Controls collection to support postback

                this.Controls.Add(objControl);

            }
        }


        /// <summary>
        /// Render this Web Part to the output parameter specified.
        /// </summary>
        /// <param name="output"> The HTML writer to write out to </param>
        protected override void RenderWebPart(HtmlTextWriter output)
        {
            base.EnsureChildControls();
            if (objControl != null)
            {
                objControl.RenderControl(output);
            }
        }
        /// <summary>
        /// Raises the webpart OnInit event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                /// Javascript fix for RadTreeView control.
                    Page.ClientScript.RegisterStartupScript(typeof(UserControlContainer), this.ID, "_spOriginalFormAction = document.forms[0].action;_spSuppressFormOnSubmitWrapper=true;", true);
                    if (this.Page.Form != null)
                    {
                        string strFormOnSubmitAtt = this.Page.Form.Attributes["onsubmit"];
                        if (!string.IsNullOrEmpty(strFormOnSubmitAtt) && string.Compare(strFormOnSubmitAtt, "return _spFormOnSubmitWrapper();") == 0)
                        {
                            this.Page.Form.Attributes["onsubmit"] = "_spFormOnSubmitWrapper();";
                        }
                    } 
            }
            catch (Exception ex)
            {
                this.Page.Response.Write(ex);
            }
        }
        /// <summary>
        /// Inherited from System.Web.UI.Control.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the server control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            //asset tree hiddenfield rendering
            if (UserControl.ToLower().Contains("assettree"))
            {
                CreateHiddenField(writer);
            }
        }
        /// <summary>
        /// Creates the hidden field.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void CreateHiddenField(HtmlTextWriter writer)
        {
            HiddenField hidSelectedRows = new HiddenField();
            hidSelectedRows.ID = "hidSelectedRows";
            hidSelectedRows.RenderControl(writer);
            HiddenField hidSelectedCriteriaName = new HiddenField();
            hidSelectedCriteriaName.ID = "hidSelectedCriteriaName";
            hidSelectedCriteriaName.RenderControl(writer);
            HiddenField hidSearchType = new HiddenField();
            hidSearchType.ID = "hidSearchType";
            hidSearchType.RenderControl(writer);
            //Added in DREAM 4.0 for Removing single asset restriction for context search
            HiddenField hidSelectedAssetNames = new HiddenField();
            hidSelectedAssetNames.ID = "hidSelectedAssetNames";
            hidSelectedAssetNames.RenderControl(writer);
            HiddenField hidAssetName = new HiddenField();
            hidAssetName.ID = "hidAssetName";
            hidAssetName.RenderControl(writer);
        }

    }
}

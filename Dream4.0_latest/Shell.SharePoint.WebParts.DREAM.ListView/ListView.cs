#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: ListView.cs
#endregion

using System;
using System.Runtime.InteropServices;
using System.Web;
using System.Net;
using System.Data;
using System.Text;
using System.Xml;
using System.Web.UI;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;
using System.ComponentModel;

namespace Shell.SharePoint.WebParts.DREAM.ListView
{
    /// <summary>
    /// This webpart used to display the data specific to
    /// a List like Master Page,Template or Book
    /// </summary>
    public  class ListView : ListViewHelper
    {
        public enum ReportName
        {
            TeamRegistration
        }
        ReportName objReportName;
        #region Declaration
        Panel pnlTopLevelContainer;
        HiddenField hdnSelectedOptions;
        HiddenField hdnReportType;
        string strParentTitle = string.Empty;
        string strTitle = string.Empty;
        Button btnAddNewTeam ;
        
        #endregion 
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("List Viewer Properties")]
        [WebDisplayNameAttribute("Report Name")]
        [WebDescription("Select the Report Name.")]
        public ReportName CustomListType
        {
            get
            {
                return objReportName;
            }
            set
            {
                objReportName = value;
            }
        }

   
       

        #region Overridden Methods
        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls 
        /// that use composition-based implementation to create any child controls they contain  
        /// in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            try
            {
                objCommonUtility = new CommonUtility();
                objMOSSController = objFactory.GetServiceManager("MossService");
                strUserName = objCommonUtility.GetUserName();
                Permission = objCommonUtility.GetTeamPermission(HttpContext.Current.Request.Url.ToString());
                CreateViewerControls();                
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
        }
        /// <summary>
        /// Creates the viewer controls.
        /// </summary>
        private void CreateViewerControls()
        {
            pnlTopLevelContainer = new Panel();
            pnlTopLevelContainer.ID = "pnlToplevelContainerID";
            pnlTopLevelContainer.ID = "DWBToplevelReportPanelCSS";
            
            hdnSelectedOptions = new HiddenField();
            hdnSelectedOptions.ID = "hdnSelectedOptions";
            hdnReportType = new HiddenField();
            hdnReportType.ID = "hdnReportType";


            //print and excel options 
            linkPrint = new HyperLink();
            linkPrint.ID = "linkPrint";
            linkPrint.CssClass = "resultHyperLink";
            linkPrint.NavigateUrl = "javascript:PrintContent();";

            linkPrint.ImageUrl = "/_layouts/DREAM/images/icon_print.gif";
            linkExcel.ID = "linkExcel";
            linkExcel.CssClass = "resultHyperLink";
            linkExcel.NavigateUrl = "javascript:ExportToExcelAdvanced();";
            linkExcel.ImageUrl = "/_layouts/DREAM/images/icon_Excel.gif";

            switch (CustomListType.ToString())
            {
                case TEAMREGISTRATION:
                    {
                        strTitle = "List Of Teams";

                        btnAddNewTeam = new Button();
                        btnAddNewTeam.ID = "btnAddNewTeam";
                        btnAddNewTeam.CssClass = "buttonAdvSrch";
                        btnAddNewTeam.Text = "New Team";
                        btnAddNewTeam.Click += new EventHandler(BtnAddNewTeam_Click);
                        Controls.Add(btnAddNewTeam);
                        ListName = TEAMREGISTRATIONLIST;
                        break;
                    }
             
            }
            pnlTopLevelContainer.Controls.Add(hdnReportType);
            SetListProperty();
            UpdateListRecords();
            this.Controls.Add(pnlTopLevelContainer);            
        }


        /// <summary>
        /// Handles the Click event of the BtnAddNewTeam control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void BtnAddNewTeam_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Redirect("/Pages/NewTeam.aspx", false);
        }

        /// <summary>
        /// Updates the list records.
        /// </summary>
        private void UpdateListRecords()
        {
            String strEventTarget = this.Page.Request.Params["__EVENTTARGET"];
            String strEventArgument = this.Page.Request.Params["__EVENTARGUMENT"];

            if (string.Equals(strEventTarget, "Delete"))
            {
                if (!string.IsNullOrEmpty(strEventArgument)) DeleteRecord(ListName, strEventArgument);
            }
        }

        /// <summary>
        /// Set the list property.
        /// </summary>
        private void SetListProperty()
        {
            switch (CustomListType.ToString())
            {
                case TEAMREGISTRATION:
                    ListName = TEAMREGISTRATIONLIST;
                    base.ListReportName = TEAMREGISTRATION;
                    break;
                default:
                    break;
            }
        }
       
        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {

            DataTable dtRecords = null;
            try
            {
                if (this.Context.Items["LeftNavSelected"] == null)
                {
                    dtRecords = GetRecords();

                    RenderParentTable(writer, string.Format(strTitle, strParentTitle));
                    hdnReportType.Value = string.Format(strTitle, strParentTitle);

                    writer.Write(" <tr><td>");
                    hdnSelectedOptions.RenderControl(writer);
                    writer.Write("    ");

                    SetViewerButtonProperties(writer);

                    writer.Write("</td>");

                    if (dtRecords == null || dtRecords.Rows.Count == 0)
                    {
                        writer.Write("<td  class=\"ListViewerOptionsCSS\" align=\"right\" colspan=\"2\">");
                        pnlTopLevelContainer.RenderControl(writer);
                    }
                    else
                    {
                        writer.Write("<td  class=\"ListViewerOptionsCSS\" align=\"right\">");
                        pnlTopLevelContainer.RenderControl(writer);
                        writer.Write(" </td><td class=\"ListViewerOptionsCSS\" align=\"right\" width=\"100%\">Print");
                        writer.Write("&nbsp;");
                        linkPrint.RenderControl(writer);
                        writer.Write("&nbsp;");
                        writer.Write("Export Page");
                        writer.Write("&nbsp;");
                        linkExcel.RenderControl(writer);
                    }
                    writer.Write(" </td><tr>");
                    /// This method call generates the XML to be transformed along with XSL and renders the output
                    GetDetailsFromList(dtRecords, writer);
                    writer.Write("</table>");
                    writer.Write(@"<Script language='javascript'> setWindowTitle(" + strTitle + "'); </Script>");
                }
            }
            catch (WebException ex1)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex1);
                writer.Write(ex1.Message);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
            finally
            {
                if (dtRecords != null)
                    dtRecords.Dispose();
            }
        }



        /// <summary>
        /// The event handler for the System.Web.UI.Control.PreRender event that occurs immediately before the Web Part is rendered to the Web Part Page it is contained on.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            base.ChromeType = PartChromeType.None;
        }


        /// <summary>
        /// Sets the viewer button properties.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void SetViewerButtonProperties(HtmlTextWriter writer)
        {                             
                switch (CustomListType.ToString())
                {
                    case TEAMREGISTRATION:
                        {
                            objMOSSController = objFactory.GetServiceManager("MossService");
                            if (((MOSSServiceManager)objMOSSController).IsAdmin(strSiteURL, HttpContext.Current.User.Identity.Name))
                            {
                                btnAddNewTeam.RenderControl(writer);
                            }
                            break;
                        }
                }
        }
       
        /// <summary>
        /// This method calls the "TransformListDetail" method to transform and XML using XSL
        /// </summary>
        /// <param name="dtRecords">DataTable</param>
        /// <param name="writer">HtmlTextWriter</param>
        private void GetDetailsFromList(DataTable dtRecords, HtmlTextWriter writer)
        {
            
            /// Sets the xmlListDocument(Global Variable) value
            GetListXml(dtRecords);
            if (xmlListDocument != null)
            {
                writer.Write("<tr><td colspan=\"3\" >");
                switch (CustomListType.ToString())
                {
                    case TEAMREGISTRATION:
                        if(dtRecords != null )
                        TransformListDetail(dtRecords.Rows.Count);
                        break;
                   
                }

                writer.Write("</td></tr>");
            }
            else
            {
                writer.Write("<tr><td colspan=\"3\" >");
                switch (CustomListType.ToString())
                {
                    case TEAMREGISTRATION:
                        writer.Write("<br/><br/><label class=\"labelMessage\">"+ NOTEAMSMESSAGE +"</label>");
                        break;
                }
                writer.Write("</td></tr>");
            }
        }       
        #endregion
    }
}

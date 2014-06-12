#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: DWBContextSearchResults.cs
#endregion
using System;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Controller;

namespace Shell.SharePoint.WebParts.DREAM.ContextSearch
{
    /// <summary>
    /// Context Search Results webpart for DWB Context Search
    /// </summary>
    public class DWBContextSearchResults : WebPart
    {

        #region WebPartPropertiesDeclaration
        bool blnIsPrintApplicable;
        bool blnIsExportPage ;
        /// <summary>
        /// Context search names
        /// </summary>
        public enum SearchNameEnum
        {
            /// <summary>
            /// DWB Search Results
            /// </summary>
            [Description("List of Books")]
            DWB
        }
        SearchNameEnum objSearchName;
        #endregion

        #region WebPart Properties
        #region PrintOption
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDescription("Check if Print Option is applicable.")]
        [WebDisplayNameAttribute("Print Option")]
        public bool Print
        {
            get
            {
                return blnIsPrintApplicable;
            }
            set
            {
                blnIsPrintApplicable = value;
            }
        }

        #region SearchName
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDisplayNameAttribute("Search Name")]
        [WebDescription("Select the search name.")]
        public SearchNameEnum SearchName
        {
            get
            {
                return objSearchName;
            }
            set
            {
                objSearchName = value;
            }
        }
        #endregion

        #endregion

        #region ExportPage
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Context Properties")]
        [WebDisplayNameAttribute("Export Page")]
        [WebDescription("Check if Export Page option is applicable.")]
        public bool ExportPage
        {
            get
            {
                return blnIsExportPage;
            }
            set
            {
                blnIsExportPage = value;
            }
        }
        #endregion

        #endregion

        #region DECLARATION
        #region CONSTANTS

        const string DWBCHAPTERLIST = "DWB Chapter";
        const string DWBBOOKSLIST = "DWB Books";
        const string CHAPTERCAMLQUERY = @"<Where><And><Eq><FieldRef Name='Asset_Type' /><Value Type='Lookup'>{0}</Value></Eq><Contains><FieldRef Name='Actual_Asset_Value' /><Value Type='Text'>{1}</Value></Contains></And></Where>";
        const string CHAPTERVIEWFIELDS = @"<FieldRef Name='ID' /><FieldRef Name='Book_ID' /><FieldRef Name='Asset_Type' /><FieldRef Name='Actual_Asset_Value' />";

        const string MOSSSERVICE = "MossService";

        const string QUERYSTRINGSEARCHTYPE = "SearchType";
        const string DWBSEARCHTYPE = "DWBContextSearch";
        //  const string NORESULTSFOUND = "No results found the for search criteria.";
        const string NORESULTSFOUND = "No records were found that matched your search parameters.Please modify your parameters and run the search again.";

        const string HIDSELECTEDROWS = "hidSelectedRows";
        const string BOOKID = "Book_ID";
        const string EVENROWSTYLE = "evenRowStyle";
        const string ODDROWSTYLE = "oddRowStyle";
        #endregion

        #region Variables

        string strCurrSiteUrl =string.Empty;
        DataTable dtBooks;
        CommonUtility objCommonUtility = new CommonUtility();
        #endregion

        #region CONTROLS
        HyperLink lnkPrint;
        HyperLink lnkExportPage ;
        Label lblMessage = new Label();

        HiddenField hidSelectedRows ;
        HiddenField hidAssetValue;
        HiddenField hidReportType ;
        HiddenField hidSearchType ;

        #endregion

        AbstractController objMossController;
        

        #endregion

        #region Protected Methods

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            try
            {
                strCurrSiteUrl = HttpContext.Current.Request.Url.ToString();
                CreateHiddenControls();
                CreateHyperLinks();
                /// Read the selected asset values from Quick Search results window and save to hidden variable.
                if (HttpContext.Current.Request.Form[HIDSELECTEDROWS] != null)
                {
                    hidSelectedRows.Value = HttpContext.Current.Request.Form[HIDSELECTEDROWS].ToString();
                    string[] arrIdentifierValue = hidSelectedRows.Value.Split('|');
                    if (arrIdentifierValue != null && arrIdentifierValue.Length > 0)
                    {
                        hidAssetValue.Value = arrIdentifierValue[0];
                    }
                }
            }

            catch (Exception Ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), Ex);

            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            objCommonUtility.RenderBusyBox(this.Page);
        }

        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            try
            {

                dtBooks = GetListofBooks();

                if (dtBooks != null && dtBooks.Rows.Count > 0)
                {
                    RenderHiddenControls(writer);
                    RenderParentTable(writer, GetEnumDescription(SearchName));
                    RenderPrintExport(writer);
                    RenderResults(writer);
                }
                else
                {
                    /// Display no books found                    
                    RenderExceptionMessage(writer, NORESULTSFOUND);
                }
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), webEx, 1);

            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
                RenderExceptionMessage(writer, ex.Message);
            }
            finally
            {
                objCommonUtility.CloseBusyBox(this.Page);
                /// Set Window Title
                writer.Write("<Script language=\"javascript\">setWindowTitle('" + GetEnumDescription(SearchName) + "');</Script>");

            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Creates the hidden controls.
        /// </summary>
        private void CreateHiddenControls()
        {
            hidSelectedRows = new HiddenField();
            hidSelectedRows.ID = HIDSELECTEDROWS;

            hidReportType = new HiddenField();
            hidReportType.ID = "hidReportType";
            hidReportType.Value = GetEnumDescription(SearchName);

            hidSearchType = new HiddenField();
            hidSearchType.ID = "hidSearchType";
            hidSearchType.Value = SearchName.ToString();

            hidAssetValue = new HiddenField();
            hidAssetValue.ID = "hidAssetValue";
        }

        /// <summary>
        /// Creates the hyper links.
        /// </summary>
        private void CreateHyperLinks()
        {
            if (Print)
            {
                lnkPrint = new HyperLink();
                lnkPrint.ID = "linkPrint";
                lnkPrint.CssClass = "resultHyperLink";
                lnkPrint.NavigateUrl = "javascript:printContent('tblSearchResults','" + SearchName.ToString() + "');";
                lnkPrint.ImageUrl = "/_layouts/DREAM/images/icon_print.gif";
                this.Controls.Add(lnkPrint);
            }
            if (ExportPage)
            {
                lnkExportPage = new HyperLink();
                lnkExportPage.ID = "linkExcel";
                lnkExportPage.CssClass = "resultHyperLink";
                if (string.Equals(SearchName.ToString().ToLowerInvariant(), "dwb"))
                {
                    lnkExportPage.NavigateUrl = "javascript:DWBExportToExcel();";
                }

                lnkExportPage.ImageUrl = "/_layouts/DREAM/images/icon_Excel.gif";
                this.Controls.Add(lnkExportPage);
            }

        }

        /// <summary>
        /// Creates a CAML query from the selected ID
        /// </summary>
        /// <param name="selectedID">The selected ID.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <returns>CAMLquery</returns>
        private string CreateCAMLQuery(string selectedID, string fieldName, string fieldType)
        {
            StringBuilder strCamlQueryBuilder = new StringBuilder();
            string[] strSelectedIDs = null;
            if (string.IsNullOrEmpty(selectedID))
            {
                // return if the selected ID is empty
                return string.Empty;
            }
            else
            {
                strSelectedIDs = selectedID.Split(';');
            }
            if (strSelectedIDs.Length == 2)
            {
                strCamlQueryBuilder.Append(@"<Where><Eq><FieldRef Name='" + fieldName + "' /><Value Type='" + fieldType + "'>" +
                    strSelectedIDs[0] + "</Value></Eq></Where>");
                return strCamlQueryBuilder.ToString();
            }
            if (strSelectedIDs.Length > 2)
            {
                for (int intIndex = 0; intIndex < strSelectedIDs.Length - 1; intIndex++)
                {
                    if (intIndex != 0)
                        strCamlQueryBuilder.Insert(0, "<Or>");
                    strCamlQueryBuilder.Append(@"<Eq><FieldRef Name='" + fieldName + "' /><Value Type='" + fieldType + "'>" +
                        strSelectedIDs[intIndex] + "</Value></Eq>");
                    if (intIndex != 0)
                        strCamlQueryBuilder.Append("</Or>");
                }
                strCamlQueryBuilder.Insert(0, @"<Where>");
                strCamlQueryBuilder.Append(@"</Where>");

            }

            return strCamlQueryBuilder.ToString();

        }

        /// <summary>
        /// Gets the enum description.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>EnumDescription</returns>
        private static string GetEnumDescription(Enum value)
        {
            FieldInfo objFieldInfo = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])objFieldInfo.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        /// <summary>
        /// Gets the listof books for the search criteria.
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable GetListofBooks()
        {
            ServiceProvider objFactory = new ServiceProvider();
            /// Column name to apply row filtering
            string[] strColumns = { BOOKID };
            StringBuilder strBookIds = new StringBuilder();
            string strCamlQuery = string.Empty;
            objMossController = objFactory.GetServiceManager(MOSSSERVICE);

            DataTable dtBooks = null;
            DataTable dtChapter = new DataTable();

            try
            {
                if (!string.IsNullOrEmpty(hidAssetValue.Value))
                {
                    dtChapter = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, DWBCHAPTERLIST, string.Format(CHAPTERCAMLQUERY, Page.Request.QueryString["assettype"], hidAssetValue.Value), CHAPTERVIEWFIELDS);
                }
                if (dtChapter != null && dtChapter.Rows.Count > 0)
                    /// Filter the Chapters table based on Unique Book_ID value
                    dtChapter = dtChapter.DefaultView.ToTable("Books", true, strColumns);
                if (dtChapter != null && dtChapter.Rows.Count > 0)
                {
                    foreach (DataRow drBookID in dtChapter.Rows)
                    {
                        strBookIds.Append(Convert.ToString(drBookID[BOOKID]));
                        strBookIds.Append(";");
                    }

                    strCamlQuery = CreateCAMLQuery(strBookIds.ToString(), "ID", "Counter");
                    string strOrderBy = @"<OrderBy><FieldRef Name='Title' Ascending='True' /></OrderBy>";
                    /// Insert OrderBy to CAML Query
                    strCamlQuery = strCamlQuery.Insert(0, strOrderBy);
                    dtBooks = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, DWBBOOKSLIST, strCamlQuery);
                }                
            }
            finally
            {
                if (dtChapter != null)
                    dtChapter.Dispose();
            }

            return dtBooks;
        }

        #region Render Methods
        /// <summary>
        /// Renders the hidden controls.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderHiddenControls(HtmlTextWriter writer)
        {
            if (hidSelectedRows != null)
                hidSelectedRows.RenderControl(writer);
            if (hidReportType != null)
                hidReportType.RenderControl(writer);
            if (hidSearchType != null)
                hidSearchType.RenderControl(writer);
            if (hidAssetValue != null)
                hidAssetValue.RenderControl(writer);
        }

        /// <summary>
        /// Renders the parent table.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="searchName">Name of the search.</param>
        private void RenderParentTable(HtmlTextWriter writer, string searchName)
        {
            /// Renders the Parent table.          
            writer.Write("<table class=\"tableAdvSrchBorder\" cellspacing=\"0\" cellpadding=\"4\" border=\"0\" width=\"100%\">");
            writer.Write("<tr><td id=\"hidePrintCol\" class=\"tdAdvSrchHeader\" colspan=\"4\" valign=\"top\"><B>" + searchName + "</B></td></tr>");
        }

        /// <summary>
        /// Renders the print export.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderPrintExport(HtmlTextWriter writer)
        {

            if (Print || ExportPage)
            {
                writer.Write("<tr><td class=\"tdAdvSrchItem\" colspan=\"4\" align=\"right\" >");

                if (Print)
                {
                    writer.Write("Print");
                    writer.Write("&nbsp;");
                    lnkPrint.RenderControl(writer);
                }
                if (ExportPage)
                {
                    writer.Write("&nbsp;");
                    writer.Write("Export Page");
                    writer.Write("&nbsp;");
                    lnkExportPage.RenderControl(writer);
                }

                writer.Write("&nbsp;</td></tr>");
                writer.Write("</table>");
                writer.Write("<br/>");
                writer.Write("<br/>");
            }

        }

        /// <summary>
        /// Renders the search results.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderResults(HtmlTextWriter writer)
        {
         //   DataTable dtBooks = new DataTable();
            try
            {
              //  dtBooks = GetListofBooks();
                if (dtBooks != null && dtBooks.Rows.Count > 0)
                {
                    DataRow drBook = null;
                    for (int intRowIndex = 0; intRowIndex < dtBooks.Rows.Count; intRowIndex++)
                    {
                        drBook = dtBooks.Rows[intRowIndex];
                        if (drBook != null)
                        {
                            /// Create Header Row
                            if (intRowIndex == 0)
                            {
                                writer.Write("<div id=\"tableContainer\" class=\"tableContainer\" width=\"100%\">");
                                writer.Write("<table style=\"border-right:1px solid #336699;\"  cellpadding=\"4\" cellspacing=\"0\" class=\"scrollTable\" id=\"tblSearchResults\" width=\"100%\">");
                                writer.Write("<thead class=\"fixedHeader\" id=\"fixedHeader\">");
                                writer.Write("<tr style=\"height: 20px;\">");
                                writer.Write("<th>Books");
                                writer.Write("<img style=\"cursor:hand\" class=\"hidePrintLink\" src=\"/_layouts/dream/images/UP_INACTIVE.GIF\" title=\"Click to Sort\" onclick=\"javascript:StringTableSort('tblSearchResults',this.parentNode);\"></img>");
                                writer.Write("</th>");
                                writer.Write("<th id=\"hidePrintCol\">Published Books</th>");
                                writer.Write("</tr>");
                                writer.Write("</thead>");
                                writer.Write("<tbody border =\"1\" class=\"scrollContent\">");
                            }
                            if (intRowIndex % 2 == 0) // Even Row
                            {
                                RenderTable(writer, drBook, EVENROWSTYLE);
                            }
                            else // Odd Row
                            {
                                RenderTable(writer, drBook, ODDROWSTYLE);
                            }
                        }
                    }

                }
                //else
                //{
                //    /// Display no books found
                //    writer.Write("<div id=\"tableContainer\" class=\"tableContainer\">");
                //    writer.Write("<table style=\"border-right:1px solid #336699;\"  cellpadding=\"4\" cellspacing=\"0\" class=\"scrollTable\" id=\"tblSearchResults\" width=\"99%\">");
                //    writer.Write("<thead class=\"fixedHeader\" id=\"fixedHeader\">");
                //    writer.Write("<tr style=\"height: 20px;\">");
                //    writer.Write("<th>Books</th>");
                //    writer.Write("<th>Published Books</th>");
                //    writer.Write("</tr>");
                //    writer.Write("</thead>");
                //    writer.Write("<tbody border =\"1\" class=\"scrollContent\">");
                //    writer.Write("<tr height=\"20px\" style=\"display:block\">");
                //    writer.Write("<td style=\"word-wrap:break-word;vertical-align:middle;text-align:left\"  width=\"300\" colspan=\"2\">");
                //    writer.Write(NORESULTSFOUND);
                //    writer.Write("</td>");
                //    writer.Write("</tr>");
                //}
                writer.Write("</tbody>");
                writer.Write("</table>");
                writer.Write("<Script language=\"javascript\">DWBFixColWidth(\"tblSearchResults\",'DWBHome');</Script> ");
                writer.Write("</div>");
            }
            finally
            {
                if (dtBooks != null)
                    dtBooks.Dispose();
            }
        }

        /// <summary>
        /// Renders the table format.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="drBook">The dr book.</param>
        /// <param name="rowStyle">The row style.</param>
        private static void RenderTable(HtmlTextWriter writer, DataRow drBook, string rowStyle)
        {
            writer.Write("<tr height=\"20px\" style=\"display:block\" class=" + rowStyle + ">");
            writer.Write("<td style=\"word-wrap:break-word;vertical-align:middle;text-align:left\"  width=\"100%\">");
            writer.Write("<a href=\"javascript:void(0)\" class=\"linkstyle\" onclick = \"javascript:OpenBookViewer('" + Convert.ToString(drBook["ID"]) + "','" + DWBSEARCHTYPE + "');\">" + Convert.ToString(drBook["Title"]) + "</a>");
            writer.Write("</td>");
            writer.Write("<td id=\"hidePrintCol\" style=\"vertical-align:middle;text-align:left\" width=\"50%\">");
            writer.Write("<a href=\"javascript:void(0)\" class=\"linkstyle\" onclick=\"javascript:openPublishedBookList('" + Convert.ToString(drBook["ID"]) + "','" + Convert.ToString(drBook["Title"]) + "');\"> List </a >");
            writer.Write("</td>");
            writer.Write("</tr>");
        }

        /// <summary>
        /// Renders the exception message.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="message">The message.</param>
        private void RenderExceptionMessage(HtmlTextWriter writer, string message)
        {
            RenderParentTable(writer, GetEnumDescription(SearchName));
            /// Rendering data preferences dropdown                  
            writer.Write("<tr><td colspan=\"4\" style=\"white-space:normal;\"><br/>");
            lblMessage.Visible = true;
            lblMessage.CssClass = "labelMessage";
            lblMessage.Text = message;
            lblMessage.RenderControl(writer);
            writer.Write("</td></tr></table>");
            /// sets the window title based on search name.
            writer.Write("<Script language=\"javascript\">setWindowTitle('" + GetEnumDescription(SearchName) + "');</Script>");
        }

        #endregion Render Methods
        #endregion
    }
}

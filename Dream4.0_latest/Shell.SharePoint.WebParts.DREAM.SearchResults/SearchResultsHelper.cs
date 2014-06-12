#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: SearchResultsHelper.cs
#endregion

using System;
using System.Web;
using System.Text;
using System.Web.UI;
using Microsoft.SharePoint;
using System.Web.UI.WebControls;
using System.Xml;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.WebParts.DREAM.SearchResults
{

    /// <summary>
    /// This is a helper class for Search Result and Context Search Result webPart.
    /// </summary>
    public class SearchResultsHelper :Shell.SharePoint.DREAM.SearchHelper.SearchHelper
    {
        #region DECLARATION
        /// <summary>
        /// Initializing the controls
        /// </summary>
        protected Label lblMessage = new Label();
        protected HyperLink linkExcel = new HyperLink();
        protected HyperLink linkPrint = new HyperLink();
        protected HyperLink linkiRequest = new HyperLink();
        protected HyperLink linkExportAll = new HyperLink();
        protected HyperLink linkViewDetailReport = new HyperLink();
        protected Button btnShowOnMap = new Button();
        protected RadioButton rbFeet = new RadioButton();
        protected RadioButton rbMeters = new RadioButton();
        protected HiddenField hidReportType;
        protected HiddenField hidDepthUnit;
        protected HiddenField hidDisplayType;
        protected HiddenField hidSearchName;
        protected HiddenField hidMaxRecordFlag;
        protected HiddenField hidCheckedColumns;
        protected HiddenField hidMapIdentified;
        protected HiddenField hidMapUseColumnName;
        protected HiddenField hidRequestID;
        protected HiddenField hidSelectedColumns;
        protected HiddenField hidMaxRecord;
        protected HiddenField hidQuickSearch;
        protected HiddenField hidRecallLogsURL;//Dream 4.0

        #region DREAM 4.0 Export and Export All options

        protected HiddenField hidFileType;

        #endregion


        //protected bool blnClicked;
        protected bool blnDisplayFeetMeter;
        protected int intRecordcount;//declared for casching implementation
        protected string strSearchType = string.Empty;
        //Declaration of Map Viewer Members.        
        protected string strMapIdentified = string.Empty;
        protected XmlDocument xmlSearchRequest = new XmlDocument();
        protected string strAsset = string.Empty;
        protected string strCriteria = string.Empty;
        //constants
        const string FEET = "Feet";
        protected const string MAPPAGEURL = "/pages/MapSearch.aspx?SearchType=Zoom";
        protected bool blnDisplayiRequest;
        protected ResourceServiceManager objResourceController;
        protected AbstractController objQueryController;
        private string[] arrSelectedIdentifiers;
        #endregion

        /// <summary>
        /// Gets or sets the selected identifiers.
        /// </summary>
        /// <value>The selected identifiers.</value>
        public string[] SelectedIdentifiers
        {
            get
            {
                return arrSelectedIdentifiers;
            }
            set
            {
                arrSelectedIdentifiers = value;
            }
        }

        #region CommonFunctionalities
        /// <summary>
        /// Creates the hyper links.
        /// </summary>
        protected void CreateHyperLinks()
        {
            //below condition check for whether Map/only data is selected 
            string strSearchTypeQS = string.Empty;
            if(Page.Request.QueryString["SearchType"] != null)
            {
                strSearchTypeQS = Page.Request.QueryString["SearchType"].ToUpper();
            }
            if(string.Equals(strSearchTypeQS, "EPCATALOG"))
            {
                linkPrint.ID = "linkPrint";
                linkPrint.CssClass = "resultHyperLink";
                linkPrint.NavigateUrl = "javascript:EPprintContent('tblSearchResults');";
                linkPrint.ImageUrl = "/_layouts/DREAM/images/icon_print.gif";

                linkExportAll.ID = "linkExportAll";
                linkExportAll.CssClass = "resultHyperLink";
                linkExportAll.NavigateUrl = "javascript:EPExportToExcelAll('/Pages/ExportToExcel.aspx','EPCatalog');";
                linkExportAll.ImageUrl = "/_layouts/DREAM/images/icon_Excel.gif";


            }
            else
            {

                linkExcel.ID = "linkExcel";
                linkExcel.CssClass = "resultHyperLink";
                linkExcel.NavigateUrl = "javascript:ExportToExcel('tblSearchResults');";
                linkExcel.ImageUrl = "/_layouts/DREAM/images/icon_Excel.gif";
                this.Controls.Add(linkExcel);

                linkPrint.ID = "linkPrint";
                linkPrint.CssClass = "resultHyperLink";
                linkPrint.NavigateUrl = "javascript:printContent('tblSearchResults');";
                linkPrint.ImageUrl = "/_layouts/DREAM/images/icon_print.gif";
                this.Controls.Add(linkPrint);

                linkExportAll.ID = "linkExportAll";
                linkExportAll.CssClass = "resultHyperLink";
                linkExportAll.ImageUrl = "/_layouts/DREAM/images/icon_Excel.gif";
                linkExportAll.NavigateUrl = "javascript:ExportToExcelAll('/Pages/ExportToExcel.aspx','SearchResults');";


                //Creates the iRequest Link for Project Archives.
                linkiRequest.ID = "linkiRequest";
                linkiRequest.CssClass = "resultHyperLink";
                linkiRequest.NavigateUrl = "javascript:OpeniRequest();";
                linkiRequest.ImageUrl = "/_layouts/DREAM/images/iRequest.gif";
                linkiRequest.Visible = false;
            }
        }

        /// <summary>
        /// Creates the hidden controls.
        /// </summary>
        protected void CreateHiddenControl()
        {
            hidSearchName = new HiddenField();
            hidSearchName.ID = "hidSearchName";
            hidMaxRecordFlag = new HiddenField();
            hidMaxRecordFlag.ID = "hidMaxRecordFlag";
            hidMaxRecord = new HiddenField();
            hidMaxRecord.ID = "hidMaxRecord";
            hidCheckedColumns = new HiddenField();
            hidCheckedColumns.ID = "hidCheckedColumns";
            hidRequestID = new HiddenField();
            hidRequestID.ID = "hidRequestID";
            this.Controls.Add(hidRequestID);
            hidMapIdentified = new HiddenField();
            hidMapIdentified.ID = "hidMapIdentified";
            hidMapUseColumnName = new HiddenField();
            hidMapUseColumnName.ID = "hidMapUseColumnName";

            hidSearchType = new HiddenField();
            hidSearchType.ID = "hidSearchType";
            hidDateFormat = new HiddenField();
            hidDateFormat.ID = "hidDateFormat";

            //DREAM 3.0 code
            //start
            hidReorderColValue = new HiddenField();
            hidReorderColValue.ID = "hidReorderColValue";
            this.Controls.Add(hidReorderColValue);
            hidReorderSourceId = new HiddenField();
            hidReorderSourceId.ID = "hidReorderSourceId";
            this.Controls.Add(hidReorderSourceId);
            hidSelectedColumns = new HiddenField();
            hidSelectedColumns.ID = "hidSelectedColumns";
            hidSelectedRows = new HiddenField();
            hidSelectedRows.ID = "hidSelectedRows";
            hidReportType = new HiddenField();
            hidReportType.ID = "hidReportType";
            hidSelectedCriteriaName = new HiddenField();
            hidSelectedCriteriaName.ID = "hidSelectedCriteriaName";
            hidDisplayType = new HiddenField();
            hidDisplayType.ID = "hidDisplayType";
            hidAssetName = new HiddenField();
            hidAssetName.ID = "hidAssetName";
            hidDepthUnit = new HiddenField();
            hidDepthUnit.ID = "hidDepthUnit";
            this.Controls.Add(hidDepthUnit);
            //end

            #region DREAM 4.0 Export and Export All options
            hidFileType = new HiddenField();
            hidFileType.ID = "hidFileType";
            this.Controls.Add(hidFileType);
            #endregion
            //Dream 4.0 code start
            hidQuickSearch = new HiddenField();
            hidQuickSearch.ID = "hidQuickSearch";
            hidSelectedAssetNames = new HiddenField();
            hidSelectedAssetNames.ID = "hidSelectedAssetNames";
            this.Controls.Add(hidSelectedAssetNames);
            hidRecallLogsURL = new HiddenField();
            hidRecallLogsURL.ID = "hidRecallLogsURL";
            hidRecallLogsURL.Value = PortalConfiguration.GetInstance().GetKey(RECALLLOGSURL);
            hidRowSelectedCheckBoxes = new HiddenField();
            hidRowSelectedCheckBoxes.ID = "hidRowSelectedCheckBoxes";
            this.Controls.Add(hidRowSelectedCheckBoxes);
            hidColSelectedCheckBoxes = new HiddenField();
            hidColSelectedCheckBoxes.ID = "hidColSelectedCheckBoxes";
            this.Controls.Add(hidColSelectedCheckBoxes);
            //Dream 4.0 code end
        }
        /// <summary>
        /// Creates labels to display message.
        /// </summary>
        protected void CreateMessageLabel()
        {
            //create the messege lable for displaying messege to user like Maxrecords exceeds or customerror messege information.
            lblMessage.ID = "lblMessage";
            lblMessage.CssClass = "labelMessage";
            lblMessage.Visible = false;
            this.Controls.Add(lblMessage);
        }
        /// <summary>
        /// Renders the radio controls.
        /// </summary>
        protected void CreateRadioControls()
        {
            //this will create option for feet/meter selection
            rbFeet.ID = "rbFeet";
            rbFeet.GroupName = "FeetMeters";
            rbFeet.Text = "Feet";
            rbFeet.Checked = true;
            rbFeet.Attributes.Add("OnClick", "javascript:FeetMetreConversionNew('tblSearchResults', 'Feet');");
            this.Controls.Add(rbFeet);

            rbMeters.ID = "rbMeters";
            rbMeters.GroupName = "FeetMeters";
            rbMeters.Text = "Metres";
            rbMeters.Attributes.Add("OnClick", "javascript:FeetMetreConversionNew('tblSearchResults', 'Metres');");
            this.Controls.Add(rbMeters);
        }
        /// <summary>
        /// Renders the parent table.
        /// </summary>
        /// <param name="writer">The writer.</param>
        protected void RenderParentTable(HtmlTextWriter writer, string searchName)
        {
            writer.Write("<table class=\"tableAdvSrchBorder\" cellspacing=\"0\" cellpadding=\"4\" border=\"0\" width=\"100%\">");
            writer.Write("<tr><td class=\"tdAdvSrchHeader\" colspan=\"4\" valign=\"top\"><b>" + searchName + "</b></td></tr>");
            writer.Write("<Script language=\"javascript\">setWindowTitle('" + searchName + "');</Script>");
        }

        /// <summary>
        /// Renders the exception message.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="message">The message.</param>
        /// <param name="windowTitle">The title of the window.</param>
        protected void RenderException(HtmlTextWriter writer, string message, string windowTitle, bool srch)
        {
            RenderParentTable(writer, windowTitle);
            writer.Write("<tr><td border=\"0\" class=\"labelMessage\">");
            writer.Write(message);
            writer.Write("</td></tr></table>");
        }
        /// <summary>
        /// Renders the inline exception.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="message">The message.</param>
        /// <param name="windowTitle">The window title.</param>
        /// <param name="srch">if set to <c>true</c> [SRCH].</param>
        protected void RenderInlineException(HtmlTextWriter writer, string message, string windowTitle, bool srch)
        {
            writer.Write("<div class=\"labelMessage\">");
            lblMessage.Visible = true;
            lblMessage.Text = message;
            lblMessage.RenderControl(writer);
            writer.Write("</div>");
            writer.Write("<Script language=\"javascript\">setWindowTitle('" + windowTitle + "');</Script>");
        }
        /// <summary>
        /// Renders the print and export.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// 
        protected void RenderPrintExport(HtmlTextWriter writer)
        {
            string strSearchTypeQS = string.Empty;
            if(Page.Request.QueryString["SearchType"] != null)
            {
                strSearchTypeQS = Page.Request.QueryString["SearchType"].ToString().ToUpper();
            }
            if(string.Equals(strSearchTypeQS, "EPCATALOG"))
            {
                linkPrint.Visible = true;
                linkExportAll.Visible = true;

                writer.Write("<tr><td class=\"tdAdvSrchItem\" colspan=\"2\" align=\"right\" width=\"100%\">Print");
                writer.Write("&nbsp;");
                linkPrint.RenderControl(writer);
                writer.Write("&nbsp;");
                writer.Write("Export All");
                writer.Write("&nbsp;");
                linkExportAll.RenderControl(writer);
                writer.Write("&nbsp;");

                writer.Write("</td></tr>");
            }
            else
            {
                linkPrint.Visible = true;

                #region DREAM 4.0 Export Options
                /// Visible set to false since Export Options div will be rendered
                linkExcel.Visible = false;
                linkExportAll.Visible = false;
                #endregion
                writer.Write("<tr><td class=\"tdAdvSrchItem\" colspan=\"4\" align=\"right\" width=\"100%\">Print");
                writer.Write("&nbsp;");
                linkPrint.RenderControl(writer);
                writer.Write("&nbsp;");
                #region DREAM 4.0 Export Options

                /// Commented since Export options div will be rendered
                writer.Write("&nbsp;");
                writer.Write("Export");
                writer.Write("&nbsp;");
                writer.Write("<input type=\"image\" class=\"buttonAdvSrch\" src=\"/_layouts/DREAM/images/icon_Excel.gif\"  id=\"btnShowExportOptionDiv\" onclick=\"SetExportOptionDefaults();return pop('divExportOptions')\" />");
                string strExportOptionsDiv = GetExportOptionsDivHTML("SearchResults", true, true, true, true);
                writer.Write(strExportOptionsDiv);
                #endregion
                if(blnDisplayiRequest)
                {
                    linkiRequest.Visible = true;
                    writer.Write("&nbsp;");
                    writer.Write("Archive Restore");
                    writer.Write("&nbsp;");
                    linkiRequest.RenderControl(writer);
                }
                writer.Write("</td></tr>");

            }
        }

        /// <summary>
        /// Renders the hidden controls.
        /// </summary>
        /// <param name="writer">The writer.</param>
        protected void RenderHiddenControls(HtmlTextWriter writer)
        {
            hidMaxRecordFlag.RenderControl(writer);
            hidSelectedColumns.RenderControl(writer);
            hidSelectedRows.RenderControl(writer);
            hidReportType.RenderControl(writer);
            hidSelectedCriteriaName.RenderControl(writer);
            hidDisplayType.RenderControl(writer);
            hidMaxRecord.RenderControl(writer);
            hidRequestID.RenderControl(writer);
            hidSearchType.RenderControl(writer);
            hidDateFormat.RenderControl(writer);
            hidAssetName.RenderControl(writer);

            #region DREAM 4.0 Export and Export All options
            hidFileType.RenderControl(writer);
            #endregion
            //Dream 4.0 code starts
            hidQuickSearch.RenderControl(writer);
            hidSelectedAssetNames.RenderControl(writer);
            hidRecallLogsURL.RenderControl(writer);
            hidRowSelectedCheckBoxes.RenderControl(writer);
            hidColSelectedCheckBoxes.RenderControl(writer);
            //Dream 4.0 code ends
        }
        /// <summary>
        /// Renders the feet meter.
        /// </summary>
        /// <param name="writer">The writer.</param>
        protected void RenderFeetMeter(HtmlTextWriter writer, string depthUnit, string detailReport)
        {
            //the below condition check for the detail report value based on that it will render the detail report link along with feet/meter options.
            if(detailReport.Length > 0)
            {
                writer.Write("<tr><td colspan=\"1\" align=\"left\" class=\"tdAdvSrchItem\"><b>Depth Units: </b>");
                if(string.Equals(depthUnit, FEET))
                {
                    rbFeet.Checked = true;
                }
                else
                {
                    rbMeters.Checked = true;
                }
                hidDepthUnit.Value = depthUnit;
                rbFeet.RenderControl(writer);
                rbMeters.RenderControl(writer);

                writer.Write("</td><td colspan=\"3\" align=\"right\" class=\"tdAdvSrchItem\">");
                linkViewDetailReport.RenderControl(writer);
                writer.Write("</td></tr>");
            }
            else
            {
                writer.Write("<tr><td colspan=\"4\" align=\"left\" class=\"tdAdvSrchItem\"><b>Depth Units: </b>");
                if(string.Equals(depthUnit, FEET))
                {
                    rbFeet.Checked = true;
                }
                else
                {
                    rbMeters.Checked = true;
                }
                hidDepthUnit.Value = depthUnit;
                rbFeet.RenderControl(writer);
                rbMeters.RenderControl(writer);
                writer.Write("</td></tr>");
            }
            hidDepthUnit.RenderControl(writer);
        }
        /// <summary>
        /// Renders the result.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="blnDisplayFeetMeter">if set to <c>true</c> [BLN display feet meter].</param>
        protected void RenderResult(HtmlTextWriter writer, bool displayFeetMeter, string searchType)
        {
            XmlTextReader objXmlTextReader = null;
            try
            {
                if(displayFeetMeter)
                {
                    //DREAM 3.0 code
                    //start
                    if(string.IsNullOrEmpty(this.Page.Request.Params.Get(EVENTTARGET)))
                    {
                        strDepthUnit = objUserPreferences.DepthUnits;
                    }
                    else
                    {
                        if(rbFeet.Checked)
                        {
                            strDepthUnit = "Feet";
                        }
                        else
                        {
                            strDepthUnit = "Metres";
                        }
                    }
                    //end
                    if(hidSearchName.Value.IndexOf("Logs By Field Depth Search") >= 0)
                        RenderFeetMeter(writer, strDepthUnit, string.Empty);
                    else
                        RenderFeetMeter(writer, strDepthUnit, "DetailReport");
                }

                //get the XSLTemplate based on the search type.
                //calling generic XSL.
                objXmlTextReader = ((MOSSServiceManager)objMossController).GetXSLTemplate(TABULARRESULTSXSL, strCurrSiteUrl);

                writer.Write("<tr><td colspan=\"4\">");
                //Transforms the searchresultXMl to XSL.
                //***Dream 4.0 
                //start
                SetPagingParameter();
                TransformXmlToResultTable(xmlDocSearchResult, objXmlTextReader, strPageNumber, strSortColumn, strSortOrder, searchType, string.Empty);
                writer.Write(strResultTable.ToString());//DREAM 4.0 code change
                writer.Write("</td></tr></table>");
                //end
            }
            finally
            {
                if(objXmlTextReader != null)
                {
                    objXmlTextReader.Close();
                }
            }
        }
        /// <summary>
        /// Sets the map zoom identified column.
        /// </summary>
        /// <param name="searchResult">The search result.</param>
        protected void SetMapUseColumnName(XmlDocument searchResult)
        {
            XmlNode ndMapUse = searchResult.SelectSingleNode("response/report/record[@recordno=1]/attribute[@mapuse='true'][1]/@name");
            if(ndMapUse != null)
            {
                hidMapUseColumnName.Value = ndMapUse.Value;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnShowOnMap control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnShowOnMap_Click(object sender, EventArgs e)
        {
            try
            {
                CommonUtility.CleanSessionVariable(true, Page);
                /*Getting the Hidden field containing all the Checked Columns Identified Value.*/
                if(this.Context.Request.Form["hidMapIdentified"] != null)
                {
                    strMapIdentified = this.Context.Request.Form["hidMapIdentified"].ToString();
                }
                if(this.Page.Request.QueryString["asset"] != null)
                {
                    strAsset = this.Page.Request.QueryString["asset"].Trim();
                }
                //added in dream2.1 for including wellbore in map
                if(strAsset.ToLower().Equals("wellbore"))
                {
                    strAsset = "Well";
                }
                //setting two session variable for map search webpart
                CommonUtility.SetSessionVariable(this.Page, enumSessionVariable.AssetType.ToString(), strAsset);
                CommonUtility.SetSessionVariable(this.Page, enumSessionVariable.ZoomListIds.ToString(), strMapIdentified);
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
            this.Page.Response.Redirect(MAPPAGEURL);
        }
        /// <summary>
        /// Creates the show on map button control.
        /// </summary>
        protected void CreateShowOnMap()
        {
            //this will create the Map related controls.                
            btnShowOnMap.ID = "btnShowOnMap";
            btnShowOnMap.Text = "Show on Map";
            btnShowOnMap.CssClass = "buttonAdvSrch";
            btnShowOnMap.OnClientClick = "javascript:return GetSelectedMapIdentifier('tblSearchResults','ON')";
            btnShowOnMap.Click += new EventHandler(btnShowOnMap_Click);
            this.Controls.Add(btnShowOnMap);
        }
        /// <summary>
        /// Registers the client side script.
        /// </summary>
        protected void RegisterClientSideScript()
        {

            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language=\"javascript\">try{");
            strScript.Append("ApplyReorder();");
            strScript.Append("}catch(Ex){}</script>");
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SearchResultsOnLoad", strScript.ToString(), false);
        }
        #endregion
        /// <summary>
        /// Gets the request ID from results.
        /// </summary>
        protected void GetRequestIDFromResults()
        {
            XmlNodeList objXmlNodeList = null;
            objXmlNodeList = xmlDocSearchResult.SelectNodes("response");
            if(objXmlNodeList != null)
            {
                foreach(XmlNode xmlNode in objXmlNodeList)
                {
                    hidRequestID.Value = xmlNode.Attributes["requestid"].Value.ToString();
                }
            }
        }
    }
}
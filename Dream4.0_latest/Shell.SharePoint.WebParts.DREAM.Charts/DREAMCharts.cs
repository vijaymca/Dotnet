#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: DREAMCharts.cs
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;

using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;

using Telerik.Web.UI;
using Telerik.Charting;

namespace Shell.SharePoint.WebParts.DREAM.Charts
{
    /// <summary>
    /// Class file for Chart webparts in DREAM
    /// </summary>
    public class DREAMCharts : ChartHelper
    {
        #region Enums
        public enum XSLFileEnum
        {

            /// <summary>
            /// DirectionalSurvey Chart
            /// </summary>
            [Description("RadTransformer")]
            DirectionalSurveyChart,

            /// <summary>
            /// TimeDepth Chart
            /// </summary>
            [Description("RadTransformer")]
            PicksChart
        }

        public enum ReportNameEnum
        {
            /// <summary>
            /// DirectionalSurvey Chart
            /// </summary>
            [Description("Directional Survey Chart")]
            DirectionalSurveyDetail,

            /// <summary>
            /// TimeDepth Chart
            /// </summary>
            [Description("Picks Chart")]
            PicksDetail
        }

        public enum XAxis
        {
            [Description("Directional Survey XAxis")]
            Depth,
            [Description("Pick Name")]
            PickName
        }
        public enum YAxis
        {
            [Description("Degrees")]
            Inclination,
            [Description("Degrees")]
            Azimuth,
            [Description("Depth")]
            AHBDFAH,


        }
        #endregion

        #region Webpart Properties

        ReportNameEnum objReportName;
        XSLFileEnum objXSLFileName;
        bool blnIsExportChart;

        #region ServiceName
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Chart Properties")]
        [WebDisplayNameAttribute("Report Name")]
        [WebDescription("Select the Report Name.")]
        public ReportNameEnum ReportName
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
        #endregion

        #region XSLFileName
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Chart Properties")]
        [WebDisplayNameAttribute("XSL File Name")]
        [WebDescription("Select the XSL File Name.")]
        public XSLFileEnum XSLFileName
        {
            get
            {
                return objXSLFileName;
            }
            set
            {
                objXSLFileName = value;
            }
        }
        #endregion

        #region ExportChart
        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("DREAM Chart Properties")]
        [WebDisplayNameAttribute("Export Chart")]
        [WebDescription("check if Export Chart option is applicable.")]
        public bool Export
        {
            get
            {
                return blnIsExportChart;
            }
            set
            {
                blnIsExportChart = value;
            }
        }
        #endregion
        #endregion

        #region Declarations

        #region Constants
        const string HIDREQUESTID = "hidRequestID";
        const string HIDSEARCHTYPE = "hidSearchType";
        #endregion

        #region Controls
        RadChart objRadChart ;
        RadScriptManager objRadScriptManager ;
        RadToolTipManager objRadToolTipManager ;



        /// <summary>
        /// Read the selected values in parent window.
        /// </summary>
        HiddenField hdnSelectedUWI;
        /// <summary>
        ///  Read the selected Depth Type from parent window. 
        /// </summary>
        HiddenField hdnSelectedDepthType;
        /// <summary>
        ///  Read the selected Depth Unit from parent window.
        /// </summary>
        HiddenField hdnSelectedDepthUnit;
        /// <summary>
        ///  Store the Search Type
        /// </summary>
        HiddenField hdnSearchType;
        /// <summary>
        ///  Store the Request ID, it will be used in Export All functionality.
        /// </summary>
        HiddenField hdnRequestID;
        /// <summary>
        ///  Store the Max Record details.
        /// </summary>
        HiddenField hdnMaxRecord;
        /// <summary>
        /// Store the selected column details.
        /// </summary>
        HiddenField hdnSelectedColumns;

        HiddenField hdnSelectedCriteriaName;
        /// <summary>
        /// Label renders the Exception/Error Mesaages.
        /// </summary>
        Label lblMessage = new Label();

        HyperLink lnkExportAll;        
        #endregion

        #region Variables
        string strDepthColumn = string.Empty;
        #endregion
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
                base.CreateChildControls();

                CreateHiddenControls();
                CreateFeatureControls();
                CreateChartControl();

            }

            catch (SoapException soapEx)
            {

                if (!string.Equals(soapEx.Message.ToString(), NORECORDSFOUND))
                {
                    CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                }
            }
            catch (WebException webEx)
            {

                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), webEx, 1);

            }
            catch (Exception ex)
            {

                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex,1);
            }
        }

        /// <summary>
        /// Handles the ItemDataBound event of the objRadChart control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Charting.ChartItemDataBoundEventArgs"/> instance containing the event data.</param>
        void objRadChart_ItemDataBound(object sender, ChartItemDataBoundEventArgs e)
        {
            DataRowView dr = ((DataRowView)e.DataItem);
            if (e.SeriesItem != null && e.SeriesItem.Parent != null)
            {
                switch (ReportName.ToString().ToLowerInvariant())
                {
                    case "directionalsurveydetail":
                        {
                            e.SeriesItem.ActiveRegion.Tooltip += e.SeriesItem.Parent.Name + ":" + e.SeriesItem.YValue + "\n" + e.SeriesItem.Parent.DataXColumn + ":" + Convert.ToString(dr[e.SeriesItem.Parent.DataXColumn]);
                            break;
                        }
                    case "picksdetail":
                        {
                            e.SeriesItem.ActiveRegion.Tooltip += e.SeriesItem.Parent.Name + ":" + e.SeriesItem.YValue + "\n" + "Pick Name:" + Convert.ToString(dr[XAxis.PickName.ToString()]);
                            break;
                        }
                }

            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            objCommonUtility.RenderBusyBox(this.Page);
            if (Page != null && RadScriptManager.GetCurrent(Page) == null)
            {
                objRadScriptManager = new RadScriptManager();
                objRadScriptManager.ID = "objScriptManager";
                Page.Form.Controls.AddAt(0, objRadScriptManager);
            }

            base.OnInit(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Unload"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains event data.</param>
        protected override void OnUnload(EventArgs e)
        {
            DisposeRadControls();
            base.OnUnload(e);
        }

        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            try
            {
                RenderHiddenControls(writer);
                RenderParentTable(writer);
                RenderChartControl(writer);
            }

            catch (SoapException soapEx)
            {
                if (!string.Equals(soapEx.Message.ToString(), NORECORDSFOUND))
                {
                    CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                }
                RenderExceptionMessage(writer, soapEx.Message);
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), webEx, 1);
                RenderExceptionMessage(writer, webEx.Message);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex,1);
                RenderExceptionMessage(writer, TECHNICALEXCEPTION);
            }
            finally
            {
                objCommonUtility.CloseBusyBox(this.Page);
                writer.Write("<Script language=\"javascript\">setWindowTitle('" + GetEnumDescription(ReportName) + "');</Script>");
                writer.Write("</table>");

            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Creates the feature controls.
        /// </summary>
        private void CreateFeatureControls()
        {
            if (Export)
            {
                lnkExportAll = new HyperLink();
                lnkExportAll.ID = "linkExportAll";
                lnkExportAll.CssClass = "resultHyperLink";
                lnkExportAll.ToolTip = "Export";
                lnkExportAll.NavigateUrl = "javascript:ExportToExcelAll('/Pages/ExportToExcel.aspx','" + ReportName.ToString() + "ChartResults');";

                lnkExportAll.ImageUrl = "/_layouts/DREAM/images/icon_Excel.gif";
               
            }
        }

        /// <summary>
        /// Creates the chart control.
        /// </summary>
        private void CreateChartControl()
        {


            objRadChart = new RadChart();
            objRadChart.ID = "obj" + this.ReportName.ToString() + "chart";
            this.Controls.Add(objRadChart);

            objRadToolTipManager = new RadToolTipManager();
            objRadToolTipManager.ID = "objRadToolTipDirectionalSurvey";
            objRadToolTipManager.Animation = ToolTipAnimation.Slide;
            objRadToolTipManager.Position = ToolTipPosition.TopCenter;
            objRadToolTipManager.AutoTooltipify = true;
            objRadToolTipManager.Skin = "Telerik";
            objRadToolTipManager.Width = Unit.Pixel(200);
            this.Controls.Add(objRadToolTipManager);

            /// Read the values sent from Parent Window
            ReadParentWindowValue();
            /// Create Chart Appearance Properties
            CreateChartAppearanceProperties();

            /// Bind data to chart
            BindChartData();
        }

        /// <summary>
        /// Creates the chart appearance properties.
        /// </summary>
        private void CreateChartAppearanceProperties()
        {
            if (objRadChart != null)
            {
                #region Chart Appearance

                objRadChart.Appearance.Dimensions.Paddings.Bottom = Telerik.Charting.Styles.Unit.Pixel(20);
                objRadChart.Appearance.Dimensions.Paddings.Top = Telerik.Charting.Styles.Unit.Pixel(20);
                objRadChart.Appearance.Dimensions.Paddings.Left = Telerik.Charting.Styles.Unit.Pixel(20);
                objRadChart.Appearance.Dimensions.Paddings.Right = Telerik.Charting.Styles.Unit.Pixel(20); //"20px";

                objRadChart.Width = Unit.Pixel(800);
                objRadChart.Height = Unit.Pixel(800);
                objRadChart.Appearance.Border.Visible = false;
                #endregion Chart Appearance

                #region Chart Title Appearance
                objRadChart.ChartTitle.TextBlock.Appearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Right;
                objRadChart.ChartTitle.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Hatch;
                objRadChart.ChartTitle.Appearance.Shadow.Color = System.Drawing.Color.YellowGreen;
                objRadChart.ChartTitle.TextBlock.Appearance.TextProperties.Font = new System.Drawing.Font("verdana", 9, System.Drawing.FontStyle.Bold);

                #endregion Chart Title Appearance

                #region Chart Legend Appearance
                objRadChart.Legend.Appearance.Location = Telerik.Charting.Styles.LabelLocation.OutsidePlotArea;

                objRadChart.Legend.Appearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.BottomRight;

                objRadChart.Legend.Appearance.ItemMarkerAppearance.Figure = "Rectangle";

                objRadChart.ItemDataBound += new EventHandler<ChartItemDataBoundEventArgs>(objRadChart_ItemDataBound);

                #endregion Chart Legend Appearance

                #region Chart PlotArea Appearance
                objRadChart.AutoLayout = true;
                objRadChart.PlotArea.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Solid;
                objRadChart.PlotArea.Appearance.FillStyle.MainColor = System.Drawing.Color.WhiteSmoke;
                objRadChart.PlotArea.Appearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Center;
                objRadChart.PlotArea.XAxis.IsZeroBased = true;
                objRadChart.PlotArea.XAxis.AutoScale = true;

                objRadChart.PlotArea.XAxis.LayoutMode = Telerik.Charting.Styles.ChartAxisLayoutMode.Normal;
                objRadChart.PlotArea.XAxis.Appearance.ValueFormat = Telerik.Charting.Styles.ChartValueFormat.Number;
                objRadChart.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 300;
                //objRadChart.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Font =
                // new System.Drawing.Font("Verdana", 6, System.Drawing.FontStyle.Bold);
                objRadChart.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Color =
                 System.Drawing.Color.Black;
                objRadChart.PlotArea.XAxis.AxisLabel.Visible = true;
                objRadChart.PlotArea.XAxis.AxisLabel.Appearance.Visible = true;
                objRadChart.PlotArea.XAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color = System.Drawing.Color.Black;
                objRadChart.PlotArea.YAxis.IsZeroBased = true;
                objRadChart.PlotArea.YAxis.Appearance.ValueFormat = Telerik.Charting.Styles.ChartValueFormat.Number;
                objRadChart.PlotArea.YAxis.AutoScale = true;
                objRadChart.PlotArea.YAxis.AxisLabel.Visible = true;
                objRadChart.PlotArea.YAxis.AxisLabel.Appearance.Visible = true;
                objRadChart.PlotArea.YAxis.AxisLabel.TextBlock.Text = string.Empty;
                objRadChart.PlotArea.YAxis.AxisLabel.ActiveRegion.Attributes = string.Empty;
                objRadChart.PlotArea.YAxis.AxisLabel.ActiveRegion.Tooltip = string.Empty;
                objRadChart.PlotArea.YAxis.Appearance.TextAppearance.TextProperties.Color =
                System.Drawing.Color.Black;
                objRadChart.PlotArea.YAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color = System.Drawing.Color.Black;

                objRadChart.PlotArea.YAxis2.IsZeroBased = true;
                objRadChart.PlotArea.YAxis2.Appearance.ValueFormat = Telerik.Charting.Styles.ChartValueFormat.Number;
                objRadChart.PlotArea.YAxis2.AutoScale = true;
                objRadChart.PlotArea.YAxis2.AxisLabel.Visible = true;
                objRadChart.PlotArea.YAxis2.AxisLabel.Appearance.Visible = true;
                objRadChart.PlotArea.YAxis2.AxisLabel.TextBlock.Text = string.Empty;
                objRadChart.PlotArea.YAxis2.AxisLabel.ActiveRegion.Attributes = string.Empty;
                objRadChart.PlotArea.YAxis2.AxisLabel.ActiveRegion.Tooltip = string.Empty;
                objRadChart.PlotArea.YAxis2.Appearance.TextAppearance.TextProperties.Color =
                System.Drawing.Color.Black;
                objRadChart.PlotArea.YAxis2.AxisLabel.TextBlock.Appearance.TextProperties.Color = System.Drawing.Color.Black;
                #endregion Chart PlotArea Appearance

                #region Chart Border
                objRadChart.Appearance.Border.Color = System.Drawing.Color.Maroon;
                #endregion

                #region Chart Series
                /// Create Chart series
                /// Set XAxis and YAxis details for each series
                switch (ReportName.ToString().ToLowerInvariant())
                {
                    #region directionalsurveydetail
                    case "directionalsurveydetail"://.ToLowerInvariant():
                        {
                            objRadChart.CreateSeries(YAxis.Azimuth.ToString(), System.Drawing.Color.Green, System.Drawing.Color.Green, ChartSeriesType.Line);
                            objRadChart.CreateSeries(YAxis.Inclination.ToString(), System.Drawing.Color.Maroon, System.Drawing.Color.Maroon, ChartSeriesType.Line);
                            strDepthColumn = XAxis.Depth.ToString();
                            foreach (ChartSeries objRadChartSeries in objRadChart.Series)
                            {
                                objRadChartSeries.Appearance.ShowLabels = false;
                                objRadChartSeries.Type = ChartSeriesType.Line;
                                if (objRadChartSeries.Name.ToLowerInvariant().Equals(YAxis.Azimuth.ToString().ToLowerInvariant()))
                                {
                                    /// Set Field name to bind Y Column
                                    objRadChartSeries.DataYColumn = YAxis.Azimuth.ToString();
                                    /// Set Field name to bind X Column
                                    objRadChartSeries.DataXColumn = XAxis.Depth.ToString();

                                    objRadChart.PlotArea.XAxis.AxisLabel.TextBlock.Text = string.Format("{0} ({1})", XAxis.Depth.ToString(), hdnSelectedDepthUnit.Value); ;
                                    objRadChart.PlotArea.YAxis.AxisLabel.TextBlock.Text = GetEnumDescription(YAxis.Azimuth);//.ToString();
                                    objRadChart.PlotArea.YAxis2.AxisLabel.TextBlock.Text = GetEnumDescription(YAxis.Inclination); //YAxis.Inclination.ToString();
                                    /// Set Azimuth as Secondary axis
                                    objRadChartSeries.YAxisType = ChartYAxisType.Secondary;
                                    objRadChartSeries.Appearance.FillStyle.MainColor = System.Drawing.Color.Green;
                                    objRadChartSeries.Appearance.PointMark.Visible = true;

                                }
                                else if (objRadChartSeries.Name.ToLowerInvariant().Equals(YAxis.Inclination.ToString().ToLowerInvariant()))
                                {
                                    /// Set Field name to bind Y Column
                                    objRadChartSeries.DataYColumn = YAxis.Inclination.ToString();
                                    /// Set Field name to bind X Column
                                    objRadChartSeries.DataXColumn = XAxis.Depth.ToString();
                                    objRadChartSeries.Appearance.FillStyle.MainColor = System.Drawing.Color.Maroon;
                                    objRadChartSeries.Appearance.PointMark.Visible = true;
                                }

                            }

                            break;
                        }
                    #endregion

                    #region picksdetail
                    case "picksdetail":
                        {
                            objRadChart.SeriesOrientation = ChartSeriesOrientation.Vertical;
                            objRadChart.CreateSeries(GetEnumDescription(YAxis.AHBDFAH), System.Drawing.Color.Green, System.Drawing.Color.Green, ChartSeriesType.Line);
                            strDepthColumn = YAxis.AHBDFAH.ToString();
                            foreach (ChartSeries objRadChartSeries in objRadChart.Series)
                            {
                                objRadChartSeries.Appearance.ShowLabels = false;
                                objRadChartSeries.Type = ChartSeriesType.Line;
                                if (objRadChartSeries.Name.ToLowerInvariant().Equals(GetEnumDescription(YAxis.AHBDFAH).ToLowerInvariant()))
                                {
                                    /// Set Field name to bind Y Column
                                    objRadChartSeries.DataYColumn = YAxis.AHBDFAH.ToString();
                                    /// Set Field name to bind X Column
                                    objRadChart.PlotArea.XAxis.DataLabelsColumn = XAxis.PickName.ToString();
                                    /// XAxis lable                                  

                                    objRadChart.PlotArea.XAxis.Appearance.LabelAppearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Bottom;

                                    objRadChart.PlotArea.XAxis.AxisLabel.TextBlock.Text = GetEnumDescription(XAxis.PickName);//.ToString();
                                    objRadChart.PlotArea.XAxis.AxisLabel.TextBlock.Appearance.Visible = true;
                                    objRadChart.PlotArea.XAxis.AxisLabel.TextBlock.Appearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.BottomRight;
                                    objRadChart.PlotArea.YAxis.AxisLabel.TextBlock.Text = string.Format("{0} ({1})", GetEnumDescription(YAxis.AHBDFAH), hdnSelectedDepthUnit.Value);
                                    objRadChartSeries.YAxisType = ChartYAxisType.Primary;
                                    objRadChartSeries.Appearance.FillStyle.MainColor = System.Drawing.Color.Blue;
                                    objRadChartSeries.Appearance.PointMark.Visible = true;

                                    /// Set Chart Width
                                    objRadChart.Width = Unit.Pixel(1500);
                                    /// Set Chart height
                                    objRadChart.Height = Unit.Pixel(1600);
                                }

                            }

                            break;
                        }
                    #endregion
                    default:
                        break;
                }
                #endregion Chart Series
            }
        }

        /// <summary>
        /// Binds the chart data.
        /// </summary>
        private void BindChartData()
        {
            DataSet dsChart = null;
            XmlTextReader objXmlTextReader = null;

            try
            {

                /// Get response XML from webservice
                objRespXmlDocument = GetResponseXML(hdnSelectedCriteriaName.Value, hdnSelectedUWI.Value);

                objFactory = new ServiceProvider();
                objMOSSController = objFactory.GetServiceManager(MOSSSERVICE);
                /// Get the XSL template to transform webservice response to RAdChart format.
                objXmlTextReader = ((MOSSServiceManager)objMOSSController).GetXSLTemplate(GetEnumDescription(XSLFileName), strCurrSiteUrl);
                if (objRespXmlDocument != null)
                {
                    dsChart = GetRadDataSource(objXmlTextReader);
                    dsChart = ConvertUnitValue(hdnSelectedDepthUnit.Value, dsChart, strDepthColumn);
                    /// Set Chart Title.                   
                    objRadChart.ChartTitle.TextBlock.Text = string.Format("UWBI : {0}", HttpContext.Current.Request.QueryString["hidReportSelectRow"]);
                    if (dsChart != null)
                    {
                        if (dsChart.Tables["attribute"] != null && dsChart.Tables["attribute"].Rows.Count > 0)
                        {
                            
                            /// Bind chart to DataTable "attribute"
                            objRadChart.DataSource = dsChart;
                            objRadChart.DataMember = "attribute";
                            objRadChart.DataBind();
                            if (dsChart.Tables["attribute"].Rows.Count == 1)
                            {
                                objRadChart.Series[0].Appearance.PointMark.Visible = true;
                                objRadChart.Series[0].Appearance.PointMark.Figure = "Star5";
                                objRadChart.Series[0].Appearance.PointMark.Dimensions.Height = Telerik.Charting.Styles.Unit.Pixel(25);
                                objRadChart.Series[0].Appearance.PointMark.Dimensions.Width = Telerik.Charting.Styles.Unit.Pixel(25);
                                objRadChart.Series[0].Appearance.PointMark.FillStyle.FillType = Telerik.Charting.Styles.FillType.Solid;
                                objRadChart.Series[0].Appearance.PointMark.FillStyle.MainColor = System.Drawing.Color.Blue;
                                objRadChart.Series[0].Appearance.PointMark.Shadow.Blur = 1;
                                objRadChart.Series[0].Appearance.PointMark.Shadow.Color = System.Drawing.Color.DimGray;
                                objRadChart.Series[0].Appearance.PointMark.Shadow.Distance = 2;

                                //Border.Color = 50, 0, 245, 100 
                                //Border.Width = 1 
                                //Figure = Star5 
                                //Visible = True 
                                //Dimensions.Height = 25px 
                                //Dimensions.Width = 25px 
                                //FillStyle.FillType = Solid 
                                //FillStyle.MainColor = 90, 100, 254, 100 
                                //Shadow.Blur = 1 
                                //Shadow.Color = DimGray 
                                //Shadow.Distance = 2 

                            }
                        }
                    }
                    /// Store the request ID
                    if (objRespXmlDocument.SelectSingleNode("response/@requestid") != null)
                    {
                        hdnRequestID.Value = hdnRequestID.Value = objRespXmlDocument.SelectSingleNode("response/@requestid").Value;
                        this.RequestID = hdnRequestID.Value = objRespXmlDocument.SelectSingleNode("response/@requestid").Value;
                    }
                }
            }
            finally
            {
                if (dsChart != null)
                    dsChart.Dispose();
                if (objXmlTextReader != null)
                    objXmlTextReader.Close();
            }
        }

        /// <summary>
        /// Creates the hidden controls.
        /// </summary>
        private void CreateHiddenControls()
        {
            hdnSearchType = new HiddenField();
            hdnSearchType.ID = "hidSearchType";

            hdnSelectedColumns = new HiddenField();
            hdnSelectedColumns.ID = "hidSelectedColumns";

            hdnSelectedDepthType = new HiddenField();
            hdnSelectedDepthType.ID = "hdnSelectedDepthType";

            hdnSelectedDepthUnit = new HiddenField();
            //hdnSelectedDepthUnit.ID = "hdnSelectedDepthUnit";
            hdnSelectedDepthUnit.ID = "hdnFeetMeters";

            hdnSelectedUWI = new HiddenField();
            hdnSelectedUWI.ID = "hdnSelectedUWI";

            hdnRequestID = new HiddenField();
            hdnRequestID.ID = "hidRequestID";

            hdnMaxRecord = new HiddenField();
            hdnMaxRecord.ID = "hidMaxRecord";

            hdnSelectedCriteriaName = new HiddenField();
            hdnSelectedCriteriaName.ID = "hdnSelectedCriteriaName";

            lblMessage = new Label();
            lblMessage.ID = "lblMessage";

            this.Controls.Add(hdnSearchType);
            this.Controls.Add(hdnSelectedColumns);
            this.Controls.Add(hdnSelectedDepthType);
            this.Controls.Add(hdnSelectedDepthUnit);
            this.Controls.Add(hdnSelectedUWI);
            this.Controls.Add(hdnRequestID);
            this.Controls.Add(hdnMaxRecord);
            this.Controls.Add(hdnSelectedCriteriaName);
            this.Controls.Add(lblMessage);
        }

        private void DisposeRadControls()
        {

            if (objRadChart != null)
            {
                objRadChart.Dispose();

            }
            if (objRadToolTipManager != null)
            {
                objRadToolTipManager.Dispose();
            }

        }

        /// <summary>
        /// Reads the parent window value.
        /// </summary>
        private void ReadParentWindowValue()
        {

            this.ResponseType = TABULARRESPONSETYPE;
            hdnSearchType.Value = ReportName.ToString();
            /// Search Type as selected ReportName property value
            this.SearchType = ReportName.ToString();
            this.EntityName = ReportName.ToString();

            /// Read the parent window selected values from query string
            if (HttpContext.Current.Request.QueryString.Count > 0)
            {
                if (HttpContext.Current.Request.QueryString["hidReportSelectRow"] != null && HttpContext.Current.Request.QueryString["hidReportSelectRow"].Length > 0)
                {
                    hdnSelectedUWI.Value = HttpContext.Current.Request.QueryString["hidReportSelectRow"];
                }
                if (HttpContext.Current.Request.QueryString["hidSelectedCriteriaName"] != null && HttpContext.Current.Request.QueryString["hidSelectedCriteriaName"].Length > 0)
                {
                    hdnSelectedCriteriaName.Value = HttpContext.Current.Request.QueryString["hidSelectedCriteriaName"];
                }
                if (HttpContext.Current.Request.QueryString["hdnSelectedDepthUnit"] != null && HttpContext.Current.Request.QueryString["hdnSelectedDepthUnit"].Length > 0)
                {
                    hdnSelectedDepthUnit.Value = HttpContext.Current.Request.QueryString["hdnSelectedDepthUnit"];
                }
            }

        }

        /// <summary>
        /// Renders the hidden controls.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderHiddenControls(HtmlTextWriter writer)
        {
            if (hdnSearchType != null)
                hdnSearchType.RenderControl(writer);
            if (hdnSelectedColumns != null)
                hdnSelectedColumns.RenderControl(writer);
            if (hdnSelectedDepthType != null)
                hdnSelectedDepthType.RenderControl(writer);
            if (hdnSelectedDepthUnit != null)
                hdnSelectedDepthUnit.RenderControl(writer);
            if (hdnSelectedUWI != null)
                hdnSelectedUWI.RenderControl(writer);
            if (hdnRequestID != null)
                hdnRequestID.RenderControl(writer);
            if (hdnMaxRecord != null)
                hdnMaxRecord.RenderControl(writer);
        }

        /// <summary>
        /// Renders the exception message.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="message">The message.</param>
        private void RenderExceptionMessage(HtmlTextWriter writer, string message)
        {
            RenderParentTable(writer);

            writer.Write("<tr><td colspan=\"1\" style=\"white-space:normal;\"><br/>");
            if (lblMessage != null)
            {
                lblMessage.Visible = true;
                lblMessage.Text = message;
                lblMessage.RenderControl(writer);
            }
            writer.Write("</td></tr></table>");
            /// sets the window title based on search name.
            writer.Write("<Script language=\"javascript\">setWindowTitle('" + GetEnumDescription(ReportName) + "');</Script>");

        }

        /// <summary>
        /// Renders the parent table.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderParentTable(HtmlTextWriter writer)
        {
            writer.Write("<table cellpadding=\"4\" cellspacing=\"0\" class=\"tableChartTable\" width=\"100%\"");

        }

        /// <summary>
        /// Renders the chart control.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderChartControl(HtmlTextWriter writer)
        {
            writer.Write("<tr><td valign=\"top\" class=\"tdChartTable\" width=\"50%\" >");
            if (objRadChart != null)
            {

                objRadChart.RenderControl(writer);
            }

            writer.Write("</td>");//</tr>");

            //writer.Write("<tr>
            writer.Write("<td  valign=\"top\" class=\"tdChartTable\" width=\"50%\" >");
            if (objRadToolTipManager != null)
            {
                objRadToolTipManager.ToolTipZoneID = objRadChart.ClientID;
                objRadToolTipManager.RenderControl(writer);

            }
            if (Export && lnkExportAll != null)
            {
                writer.Write("Export");
                writer.Write("&nbsp;");
                lnkExportAll.RenderControl(writer);
            }
            writer.Write("</td></tr>");
        }

        /// <summary>
        /// Gets the enum description.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
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
        #endregion
    }
}

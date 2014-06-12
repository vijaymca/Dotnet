#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : AHDTVDConverter.ascx.cs
#endregion

#region namespace
using System;
using System.Collections;
using System.Net;
using System.Web;
using System.Xml;
using System.Web.UI;
using System.Web.Services.Protocols;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Controller;
using System.Text;
#endregion

namespace Shell.SharePoint.DREAM.Site.UI.UserControls
{
    /// <summary>
    /// This user control is used to calculate the AH depth / TV depth values
    /// </summary>

    public partial class AHDTVDConverter : UIControlHandler
    {
        #region Variables
        protected string strIdentifiedItem = string.Empty;
        protected string strIdentifierName = string.Empty;
        protected string strInputDepthUnit = string.Empty;
        protected string stroutputDepthUnit = string.Empty;
        protected string strDepthname = string.Empty;
        protected string strDepthValues = string.Empty;
        protected const string strWELLBORE = "WELLBORE";
        protected const string strMECHANICALDATADEPTHREF = "MECHANICALDATADEPTHREF";
        protected const string strAHTVCALCULATOR = "AHTVCALCULATOR";
        protected const string strMD = "MD";
        protected const string strTVD = "TVD";
        protected const string strUWBI = "UWBI";
        protected const string Default = "Default";
        protected const string strAHTVDWeb = "AHTVDWeb";
        protected const string strtblWithHeaderRow = "tblWithHeaderRow";
        protected const string strtblWithOutHeaderRow = "tblWithOutHeaderRow";
        protected const string METRES = "Metres";
        protected const string FEET = "Feet";
        const string SORTCOLUMN = "WellBore Name";
        protected string strFeetMetre = string.Empty;
        string[] arrIdentifierValue = new string[1];
        string[] arrIdentifierValueFirst = new string[1];
        string[] arrDepthval = new string[5];
        string[] arrDepthValueConvertPath;
        private string strDepthUnit = string.Empty;
        int intMinRowLength;
        int intMaxRowlength = 12;
        protected const int intMaxRecord = 500;
        int intSortOrder;
        int intRowNumber;
        int introwCount;
        double dblTopDepth;
        double dblBottomDepth;
        int intDepthInterval;
        int inttblCount;

        #endregion

        #region CONTROLS

        XmlDocument xmlDocSearchResultWellBore;
        XmlDocument xmlDocSearchResultDepthRef;
        XmlDocument xmlDocSearchResultAHTVD;

        Table tblConvertRows = new Table();
        #endregion

        #region ServiceName

        public ServiceNameEnum ServiceName
        {
            get
            {
                return objServiceName;
            }
            set
            {
                objServiceName = value;
            }
        }
        #endregion

        #region WebPartPropertiesDeclaration
        public enum ServiceNameEnum
        {
            ReportService,
            EventService
        }
        ServiceNameEnum objServiceName;
        #endregion

        /// <summary>
        /// Page_s the init.
        /// </summary>

        protected void Page_Init()
        {
            #region  Page_init
            try
            {
                pnlSoapError.Visible = false;
                lblErrorMessage.Visible = false;
                pnlConverterContent.Visible = true;
                btn_goBack.Visible = false;
                pnlTable.Visible = true;
                pnlTableErrorMessage.Visible = false;
                lblTableErrorMessage.Visible = false;
                objUtility = new CommonUtility();

                objUtility.RenderBusyBox(this.Page);

                if (HttpContext.Current.Request.Form["hidSelectedRows"] != null)
                {
                    //This session values is assigned into AHDTVDPopup.ascx / TVDepthPopup.ascx
                    Session.Remove("DepthValues");
                    Session.Remove("tblConvertsRowsCount");
                    Session.Remove("xmlDocSearchResultWellBore");
                    Session.Remove("TopDepth");
                    Session.Remove("BottomDepth");
                    Session.Remove("xmlDocSearchResultAHTVD");
                    Session.Remove("xmlDocSearchResultDepthRef");
                    Session.Remove("hidDrpDepthRefValue");
                }

                tblConvertRows.EnableViewState = true;
                //**added of dev
                CommonUtility objCommonUtility = new CommonUtility();
                ServiceProvider objFactory = new ServiceProvider();
                objReportController = objFactory.GetServiceManager(ServiceName.ToString());
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);


                string strUserId = objCommonUtility.GetUserName();
                //reads the user preferences.
                objUserPreferences = ((MOSSServiceManager)objMossController).GetUserPreferencesValue(strUserId, strCurrSiteUrl);
                CommonUtility.SetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString(), objUserPreferences);
                object objSessionUserPreference = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString());
                if (objSessionUserPreference != null)
                    objUserPreferences = (UserPreferences)objSessionUserPreference;
                strDepthUnit = objUserPreferences.DepthUnits;

                if (rdoDepthUnitsFeet.Checked != true && rdoDepthUnitsMetres.Checked != true)
                {
                    if (string.Equals(strDepthUnit, METRES))
                    {
                        rdoDepthUnitsMetres.Checked = true;
                    }
                    else if (string.Equals(strDepthUnit, FEET))
                    {
                        rdoDepthUnitsFeet.Checked = true;
                    }
                }
                //**end

                //Used to set the tab index for AH and TV textbox only.
                if (hdnTabIndex != null)
                {
                    if (string.IsNullOrEmpty(hdnTabIndex.Value.ToString()))
                    {
                        hdnTabIndex.Value = "1";
                    }
                }

                if (Session["tblConvertsRowsCount"] == null)
                {
                    GenerateRows(intMinRowLength, intMaxRowlength, strtblWithHeaderRow);
                }
                else
                {
                    GenerateRows(intMinRowLength, intMaxRowlength, strtblWithHeaderRow);
                    GenerateRows(intMaxRowlength, Convert.ToInt32(Session["tblConvertsRowsCount"].ToString()), strtblWithOutHeaderRow);
                }

                objReportController = objFactory.GetServiceManager(ServiceName.ToString());

                //Fill Wellbore name into Wellbore Dropdown

                #region populate wellbore name
                if (Session["xmlDocSearchResultWellBore"] == null)
                {
                    objRequestInfo = SetDataObject(strUWBI);
                    xmlDocSearchResultWellBore = objReportController.GetSearchResults(objRequestInfo, intMaxRecord, strWELLBORE, SORTCOLUMN, intSortOrder);
                    FilldrpWellboreName(xmlDocSearchResultWellBore);
                    Session["xmlDocSearchResultWellBore"] = xmlDocSearchResultWellBore.OuterXml;
                    objRequestInfo = null;
                }
                else
                {
                    XmlDocument xmlResponse = new XmlDocument();
                    xmlResponse.LoadXml((string)Session["xmlDocSearchResultWellBore"]);
                    FilldrpWellboreName(xmlResponse);
                    objRequestInfo = null;
                }
                #endregion

                //Fill Wellbore depth reference when Wellbore name is selected.

                #region Populate Wellbore Depth Reference.

                if (Session["xmlDocSearchResultDepthRef"] == null)
                {
                    objRequestInfo = SetDataObject(Default);
                    xmlDocSearchResultDepthRef = objReportController.GetSearchResults(objRequestInfo, intMaxRecord, strMECHANICALDATADEPTHREF, null, intSortOrder);
                    hidDepthRefDefaultUnit.Value = GetMeasurementUnit(xmlDocSearchResultDepthRef.SelectNodes("response/report/record[1]/attribute[@name='depth_reference_elevation']")[0]);
                    FilldrpDepthReference(xmlDocSearchResultDepthRef);
                    Session["xmlDocSearchResultDepthRef"] = xmlDocSearchResultDepthRef.OuterXml;
                    objRequestInfo = null;
                }
                else
                {
                    XmlDocument xmlResponse = new XmlDocument();
                    xmlResponse.LoadXml((string)Session["xmlDocSearchResultDepthRef"]);
                    hidDepthRefDefaultUnit.Value = GetMeasurementUnit(xmlResponse.SelectNodes("response/report/record[1]/attribute[@name='depth_reference_elevation']")[0]);
                    FilldrpDepthReference(xmlResponse);
                }
                #endregion

                #region Display Country Details.
                if (Session["xmlDocSearchResultAHTVD"] == null)
                {
                    objRequestInfo = SetDataObject(Default);
                    xmlDocSearchResultAHTVD = objReportController.GetSearchResults(objRequestInfo, intMaxRecord, strAHTVCALCULATOR, null, intSortOrder);
                    FillCountryDetails(xmlDocSearchResultAHTVD);
                    Session["xmlDocSearchResultAHTVD"] = xmlDocSearchResultAHTVD.OuterXml;
                    objRequestInfo = null;
                }
                else
                {
                    XmlDocument xmlResponse = new XmlDocument();
                    xmlResponse.LoadXml((string)Session["xmlDocSearchResultAHTVD"]);
                    FillCountryDetails(xmlResponse);
                }
                #endregion

                inttblCount = tblConvertRows.Rows.Count;

                //Populate Top & Bottom Depths value from Popup Window

                # region Top & Bottom Depths values to Table
                if (Session["DepthValues"] != null)
                {
                    strDepthValues = Session["DepthValues"].ToString();
                    arrDepthval = strDepthValues.Split('|');
                    dblTopDepth = Convert.ToDouble(arrDepthval[0].ToString());
                    dblBottomDepth = Convert.ToDouble(arrDepthval[1].ToString());
                    intDepthInterval = Convert.ToInt32(arrDepthval[2].ToString());
                    strDepthname = arrDepthval[3].ToString();
                    //used to find the no of rows to be create in the HTML table.
                    int index = Convert.ToInt32(Math.Ceiling(Math.Round(((dblBottomDepth - dblTopDepth) / intDepthInterval), 2)));

                    index = index + 3;
                    if (index < inttblCount)
                    {
                        //if index count is <=10, then no need to create extra rows, by default 10 rows will be there.
                        FillDepthValues(dblTopDepth, dblBottomDepth, intDepthInterval, index, strDepthname, drpDepthReference.SelectedItem.Value);
                    }
                    else
                    {
                        // if index count is >=10, then need to add new rows into HTML table.
                        GenerateRows(intMaxRowlength, index, strtblWithOutHeaderRow);
                        FillDepthValues(dblTopDepth, dblBottomDepth, intDepthInterval, index, strDepthname, drpDepthReference.SelectedItem.Value);

                    }
                }
                #endregion

                SetCSSforEditableAHTVdepth();

            }
            catch (SoapException soapEx)
            {
                if (!string.Equals(soapEx.Message, SOAPEXCEPTIONMESSAGE))
                {
                    CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                }
                RenderExceptionMessage(soapEx.Message);
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), webEx, 1);
                RenderExceptionMessage(webEx.Message);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);

            }
            #endregion
        }


        #region Fill the Depth values from Populate Window.

        /// <summary>
        /// Fills the depth values.
        /// </summary>
        /// <param name="dblTopDepth">The DBL top depth.</param>
        /// <param name="dblBottomDepth">The DBL bottom depth.</param>
        /// <param name="intDepthInterval">The int depth interval.</param>
        /// <param name="length">The length.</param>
        /// <param name="strDepthname">The STR depthname.</param>
        /// <param name="Depthref">The depthref.</param>
        public void FillDepthValues(double dblTopDepth, double dblBottomDepth, int intDepthInterval, int length, string strDepthname, string Depthref)
        {
            try
            {
                intRowNumber = 1;
                TextBox txtDepth;
                TextBox txtlastDepth;
                TextBox txtlblDepth;
                double interval = Double.Parse(Depthref);
                Decimal declblAHDepthValue;
                Decimal decDrpDepthRefValue;
                string strdepth = string.Empty;
                string strDrpDepthRefValue = Session["hidDrpDepthRefValue"].ToString();
                decDrpDepthRefValue = Convert.ToDecimal(strDrpDepthRefValue.ToString());
                //*added by dev
                decDrpDepthRefValue = ConvertFeetMetre(decDrpDepthRefValue, hidDepthRefDefaultUnit.Value);
                //end
                int intLength = length - 3;

                for (double index = dblTopDepth; index <= dblBottomDepth; index = index + intDepthInterval)
                {
                    if (intRowNumber <= intLength)
                    {
                        // if Values coming from AHDepth popup or TVDepth Popup
                        if (strDepthname.Equals(strMD))
                        {
                            txtDepth = (TextBox)tblConvertRows.Rows[intRowNumber + 1].Cells[0].FindControl("txtAHDepth" + intRowNumber);
                            txtlblDepth = (TextBox)tblConvertRows.Rows[intRowNumber + 1].Cells[1].FindControl("lblAHDepth" + intRowNumber);
                        }
                        else
                        {
                            txtDepth = (TextBox)tblConvertRows.Rows[intRowNumber + 1].Cells[2].FindControl("txtTVDepth" + intRowNumber);
                            txtlblDepth = (TextBox)tblConvertRows.Rows[intRowNumber + 1].Cells[3].FindControl("lblTVDepth" + intRowNumber);
                        }
                        txtDepth.Text = index.ToString("#0.00");

                        declblAHDepthValue = Convert.ToDecimal(txtDepth.Text.ToString());
                        // This function called when user enter the depth value from popup window, then need to fill 2 column or 4 column with respective to the 1 and 3 textboxes.
                        //if 1 or 3 text box value is 100, then depth ref dropdown selected value is DF (1.23) then need to display 100-1.23 = 98.77.
                        txtlblDepth.Text = Convert.ToString(Math.Round((declblAHDepthValue - decDrpDepthRefValue), 2).ToString("#0.00"));
                        strdepth = txtDepth.Text;

                        intRowNumber = intRowNumber + 1;
                    }
                }


                //Use to display the bottom value , suppose top depth is 10.2, bottom depth is 21.5, interval  2, then it will divide up 20.2, then we need to 21.5 value to next textbox.
                int introwCnt = intRowNumber - 1;
                if (strDepthname.Equals(strMD))
                {
                    txtDepth = (TextBox)tblConvertRows.Rows[intRowNumber].Cells[0].FindControl("txtAHDepth" + intRowNumber);
                    txtlastDepth = (TextBox)tblConvertRows.Rows[introwCnt].Cells[0].FindControl("txtAHDepth" + introwCnt);

                    if (txtDepth.Text != dblBottomDepth.ToString() && string.IsNullOrEmpty(txtDepth.Text))
                        if (txtlastDepth.Text != dblBottomDepth.ToString())
                        {
                            txtDepth.Text = dblBottomDepth.ToString("#0.00");
                            txtlblDepth =
                                (TextBox)
                                tblConvertRows.Rows[intRowNumber].Cells[1].FindControl("lblAHDepth" + intRowNumber);
                            declblAHDepthValue = Convert.ToDecimal(txtDepth.Text.ToString());
                            // This function called when user enter the depth value from popup window, then need to fill 2 column or 4 column with respective to the 1 and 3 textboxes.
                            //if 1 or 3 text box value is 100, then depth ref dropdown selected value is DF (1.23) then need to display 100-1.23 = 98.77.
                            txtlblDepth.Text =
                                Convert.ToString(
                                    Math.Round((declblAHDepthValue - decDrpDepthRefValue), 2).ToString("#0.00")).
                                    ToString();
                        }


                }
                else
                {
                    txtDepth = (TextBox)tblConvertRows.Rows[intRowNumber].Cells[2].FindControl("txtTVDepth" + intRowNumber);
                    txtlastDepth = (TextBox)tblConvertRows.Rows[introwCnt].Cells[2].FindControl("txtTVDepth" + introwCnt);

                    if (txtDepth.Text != dblBottomDepth.ToString() && string.IsNullOrEmpty(txtDepth.Text))
                    {
                        if (txtlastDepth.Text != dblBottomDepth.ToString())
                        {
                            txtDepth.Text = dblBottomDepth.ToString("#0.00");
                            txtlblDepth =
                                (TextBox)
                                tblConvertRows.Rows[intRowNumber].Cells[3].FindControl("lblTVDepth" + intRowNumber);
                            declblAHDepthValue = Convert.ToDecimal(txtDepth.Text.ToString());
                            // This function called when user enter the depth value from popup window, then need to fill 2 column or 4 column with respective to the 1 and 3 textboxes.
                            //if 1 or 3 text box value is 100, then depth ref dropdown selected value is DF (1.23) then need to display 100-1.23 = 98.77.
                            txtlblDepth.Text =
                                Convert.ToString(
                                    Math.Round((declblAHDepthValue - decDrpDepthRefValue), 2).ToString("#0.00")).
                                    ToString();
                        }
                    }



                }
            }
            catch (Exception)
            {
                throw;
            }


        }
        #endregion

        #region Page Load()
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AbstractController objMossController = null;
                UserPreferences objUserPreferences = new UserPreferences();
                ////Some time user enter the values in between the textbox like row1 textbox and row 5 textbox, before populate the value to table, clean up the textbox value.
                drpWellbore.Attributes.Add("onChange", "javascript:ClearAllRows(this)");

                #region if Wellbore dropdown value changed, need to populate Depthrefece value country value to Dropdown and Label control

                CommonUtility objCommonUtility = new CommonUtility();
                ServiceProvider objFactory = new ServiceProvider();
                objReportController = objFactory.GetServiceManager(ServiceName.ToString());
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);


                string strUserId = objCommonUtility.GetUserName();
                //reads the user preferences.
                objUserPreferences = ((MOSSServiceManager)objMossController).GetUserPreferencesValue(strUserId, strCurrSiteUrl);
                CommonUtility.SetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString(), objUserPreferences);

                object objSessionUserPreference = CommonUtility.GetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString());
                if (objSessionUserPreference != null)
                    objUserPreferences = (UserPreferences)objSessionUserPreference;
                strDepthUnit = objUserPreferences.DepthUnits;

                if (rdoDepthUnitsFeet.Checked != true && rdoDepthUnitsMetres.Checked != true)
                {
                    if (string.Equals(strDepthUnit, METRES))
                    {
                        rdoDepthUnitsMetres.Checked = true;
                    }
                    else if (string.Equals(strDepthUnit, FEET))
                    {
                        rdoDepthUnitsFeet.Checked = true;
                    }
                }


                if (rdoDepthUnitsFeet.Checked)
                {
                    strFeetMetre = "(ft)";
                }
                if (rdoDepthUnitsMetres.Checked)
                {
                    strFeetMetre = "(m)";
                }

                //Used to display the selected value to drpdepthReference Dropdown.
                FillDepthrefSelectedValue();

                if((objUtility.GetPostBackControl(this.Page) != null) && (string.Equals(objUtility.GetPostBackControl(this.Page).ID, "drpWellbore")))
                {
                    try
                    {
                        rdoDepthUnitsFeet.Checked = true;
                        objRequestInfo = SetDataObject(Default);
                        xmlDocSearchResultAHTVD = objReportController.GetSearchResults(objRequestInfo, intMaxRecord, strAHTVCALCULATOR, null, intSortOrder);
                        FillCountryDetails(xmlDocSearchResultAHTVD);
                        Session["xmlDocSearchResultAHTVD"] = null;
                        Session["xmlDocSearchResultAHTVD"] = xmlDocSearchResultAHTVD.OuterXml;
                        objRequestInfo = null;

                        objRequestInfo = SetDataObject(Default);
                        xmlDocSearchResultDepthRef = objReportController.GetSearchResults(objRequestInfo, intMaxRecord, strMECHANICALDATADEPTHREF, null, intSortOrder);
                        Session["xmlDocSearchResultDepthRef"] = null;
                        Session["xmlDocSearchResultDepthRef"] = xmlDocSearchResultDepthRef.OuterXml;
                        FilldrpDepthReference(xmlDocSearchResultDepthRef);
                        FillDepthrefSelectedValue();
                    }
                    catch
                    {
                        pnlTable.Visible = false;
                        pnlTableErrorMessage.Visible = true;
                        lblTableErrorMessage.Visible = true;
                    }
                }

                #endregion



                int intCount = tblConvertRows.Rows.Count - 2;
                // Thease session variables are used to display the values into First row first col, last row first column of the HTML tables in non-editable mode.
                if (Session["TopDepth"] != null && Session["BottomDepth"] != null)
                {
                    //Assign always Topdepth values into First textbox, and Bottom values into last text box, thease values are coming webservice.
                    TextBox txtAHDepth0 = (TextBox)tblConvertRows.Rows[0].Cells[0].FindControl("txtAHDepth0");
                    txtAHDepth0.Text = Convert.ToDouble(Session["TopDepth"].ToString()).ToString(("#0.00"));

                    TextBox txtAHDepthMax = (TextBox)tblConvertRows.Rows[intCount].Cells[0].FindControl("txtAHDepth" + intCount);
                    txtAHDepthMax.Text = Convert.ToDouble(Session["BottomDepth"].ToString()).ToString(("#0.00"));

                    TextBox lblAHDepth0 = (TextBox)tblConvertRows.Rows[0].Cells[1].FindControl("lblAHDepth0");

                    Decimal decTxtAHDepth0 = Convert.ToDecimal(txtAHDepth0.Text.ToString());
                    Decimal decTxtAHDepthMax = Convert.ToDecimal(txtAHDepthMax.Text.ToString());
                    Decimal decHndDrpValue = Convert.ToDecimal(Session["hidDrpDepthRefValue"].ToString());
                    //*added by Dev
                    decHndDrpValue = ConvertFeetMetre(decHndDrpValue, hidDepthRefDefaultUnit.Value);

                    // used to display with 2 digit, eg : 52.00
                    //Bydefault need to display calculated values to 1 rows 2 columns and last row 2 columns.
                    // (0.00 - Depth Reference (DF,BF,etc...)
                    lblAHDepth0.Text = Convert.ToString(Math.Round(Convert.ToDecimal(Convert.ToDouble(decTxtAHDepth0) - Convert.ToDouble(decHndDrpValue)), 2).ToString("#0.00")).ToString();
                    TextBox lblAHDepthMax = (TextBox)tblConvertRows.Rows[intCount].Cells[1].FindControl("lblAHDepth" + intCount);
                    lblAHDepthMax.Text = Convert.ToString(Math.Round(Convert.ToDecimal(Convert.ToDouble(decTxtAHDepthMax) - Convert.ToDouble(decHndDrpValue)), 2).ToString("#0.00")).ToString();

                }
            }
            catch (SoapException soapEx)
            {
                if (!string.Equals(soapEx.Message, SOAPEXCEPTIONMESSAGE))
                {
                    CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                }
                RenderExceptionMessage(soapEx.Message);
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), webEx, 1);
                RenderExceptionMessage(webEx.Message);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
            finally
            {
                objUtility.CloseBusyBox(this.Page);
            }
        }
        #endregion

        #region SetDataObject()
        /// <summary>
        /// Sets the data object.
        /// </summary>
        /// <param name="agrVal">The agr val.</param>
        /// <returns></returns>
        private RequestInfo SetDataObject(string agrVal)
        {
            try
            {
                objRequestInfo = new RequestInfo();
                if (agrVal.Equals(strUWBI))
                {
                    objRequestInfo.Entity = SetEntity(strUWBI);
                }
                if (agrVal.Equals(Default))
                {
                    objRequestInfo.Entity = SetEntity(Default);
                }
                if (agrVal.Equals(strAHTVDWeb))
                {
                    objRequestInfo.Entity = SetEntityTables();
                }

            }
            catch (Exception)
            {
                throw;
            }

            return objRequestInfo;

        }
        #endregion

        #region Fill Depthrefernce Dropdown

        /// <summary>
        /// Filldrps the depth reference.
        /// </summary>
        /// <param name="xmlResponse">The XML response.</param>
        private void FilldrpDepthReference(XmlDocument xmlResponse)
        {
            try
            {
                drpDepthReference.Items.Clear();
                if (xmlResponse != null)
                {
                    XmlNodeList xmlNodeList = xmlResponse.SelectNodes("response/report/record");
                    foreach (XmlNode node in xmlNodeList)
                    {
                        ListItem lstItem = new ListItem();
                        lstItem.Text = node.SelectNodes("attribute[@name='depth_reference_point']/@value")[0].Value;
                        lstItem.Value = node.SelectNodes("attribute[@name='depth_reference_elevation']/@value")[0].Value;
                        drpDepthReference.Items.Add(lstItem);
                    }
                    //Add PDL item to drpDepthRef dropdown.
                    ListItem pdlItem = new ListItem("PDL", "0.00");
                    drpDepthReference.Items.Add(pdlItem);

                    XmlNode objxmlNode = xmlResponse.SelectSingleNode("response/report/record[1]/attribute[@name='Default']/@value");
                    if ((objxmlNode != null) && (drpDepthReference.Items.FindByText(objxmlNode.Value) != null))
                    {
                        //set default value as a selected item.
                        drpDepthReference.Items.FindByText(objxmlNode.Value).Selected = true;
                        hidDrpValue.Value = drpDepthReference.SelectedItem.Text;
                        Session["hidDrpDepthRefValue"] = null;
                        Session["hidDrpDepthRefText"] = null;
                        Session["hidDrpDepthRefValue"] = drpDepthReference.SelectedItem.Value;
                        Session["hidDrpDepthRefText"] = hidDrpValue.Value;
                    }
                    FillDepthrefSelectedValue();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region Fill Wellbore dropdown()
        /// <summary>
        /// Filldrps the name of the wellbore.
        /// </summary>
        /// <param name="xmlResponse">The XML response.</param>
        private void FilldrpWellboreName(XmlDocument xmlResponse)
        {
            try
            {
                if (xmlResponse != null)
                {
                    XmlNodeList xmlNodeList = xmlResponse.SelectNodes("response/report/record");
                    foreach (XmlNode node in xmlNodeList)
                    {
                        ListItem lstItem = new ListItem();
                        lstItem.Value = node.SelectNodes("attribute[@name='UWBI']/@value")[0].Value;
                        lstItem.Text = node.SelectNodes("attribute[@name='WellBore Name']/@value")[0].Value;
                        drpWellbore.Items.Add(lstItem);
                    }
                    //Set default value in dropdown               
                    XmlNode objxmlNode = xmlResponse.SelectSingleNode("response/report/record[1]/attribute[@name='WellBore Name']/@value");
                    if ((objxmlNode != null) && (drpWellbore.Items.FindByText(objxmlNode.Value) != null))
                    {
                        drpWellbore.Items.FindByText(objxmlNode.Value).Selected = true;
                    }

                }
            }
            catch (SoapException)
            {
                throw;
            }
            catch (WebException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region Fill country details()
        /// <summary>
        /// Fills the country details.
        /// </summary>
        /// <param name="xmlResponse">The XML response.</param>
        private void FillCountryDetails(XmlDocument xmlResponse)
        {
            try
            {
                string countryDetails = string.Empty;
                string ProjectDetails = string.Empty;
                string WellborePathDetails = string.Empty;
                string PDLDetails = string.Empty;
                Decimal decfirstTopDepth;
                Decimal declastBottomDepth;
                if (xmlResponse != null)
                {
                    XmlNodeList xmlNodeList = xmlResponse.SelectNodes("response/report/record");
                    foreach (XmlNode node in xmlNodeList)
                    {
                        lblResultCountry.Text = node.SelectNodes("attribute[@name='Country']/@value")[0].Value.ToString();
                        string CountryTableName = node.SelectNodes("attribute[@name='Country']/@tablename")[0].Value.ToString();
                        string Countrydbcolumnname = node.SelectNodes("attribute[@name='Country']/@dbcolumnname")[0].Value.ToString();
                        if (!string.IsNullOrEmpty(CountryTableName))
                            countryDetails = "Table Name: " + CountryTableName; // +"<br>Column Name: " + Countrydbcolumnname;
                        if (!string.IsNullOrEmpty(CountryTableName))
                            countryDetails += "\n" + "Column Name: " + Countrydbcolumnname;
                        lblCountry.ToolTip = countryDetails.ToString();

                        lblResultProject.Text = node.SelectNodes("attribute[@name='Projected Coordinate System']/@value")[0].Value.ToString();
                        string ProjectTableName = node.SelectNodes("attribute[@name='Projected Coordinate System']/@tablename")[0].Value.ToString();
                        string Projectdbcolumnname = node.SelectNodes("attribute[@name='Projected Coordinate System']/@dbcolumnname")[0].Value.ToString();
                        if (!string.IsNullOrEmpty(ProjectTableName))
                            ProjectDetails = "Table Name: " + ProjectTableName; // +"<br>Column Name: " + Projectdbcolumnname;
                        if (!string.IsNullOrEmpty(Projectdbcolumnname))
                            ProjectDetails += "\n" + "Column Name: " + Projectdbcolumnname;
                        lblProject.ToolTip = ProjectDetails.ToString();

                        lblResultWellborePath.Text = node.SelectNodes("attribute[@name='Wellbore Path']/@value")[0].Value.ToString();
                        string WellborePathTableName = node.SelectNodes("attribute[@name='Wellbore Path']/@tablename")[0].Value.ToString();
                        string WellborePathdbcolumnname = node.SelectNodes("attribute[@name='Wellbore Path']/@dbcolumnname")[0].Value.ToString();
                        if (!string.IsNullOrEmpty(WellborePathTableName))
                            WellborePathDetails = "Table Name: " + WellborePathTableName; // +"<br>Column Name: " + Projectdbcolumnname;
                        if (!string.IsNullOrEmpty(WellborePathdbcolumnname))
                            WellborePathDetails += "\n" + "Column Name: " + WellborePathdbcolumnname;
                        lblWellborePath.ToolTip = WellborePathDetails.ToString();

                        lblResultPDL.Text = node.SelectNodes("attribute[@name='Permanent Datum']/@value")[0].Value.ToString();
                        string PDLTableName = node.SelectNodes("attribute[@name='Permanent Datum']/@tablename")[0].Value.ToString();
                        string PDLdbcolumnname = node.SelectNodes("attribute[@name='Permanent Datum']/@dbcolumnname")[0].Value.ToString();
                        if (!string.IsNullOrEmpty(PDLTableName))
                            PDLDetails = "Table Name: " + PDLTableName; // +"<br>Column Name: " + Projectdbcolumnname;
                        if (!string.IsNullOrEmpty(WellborePathdbcolumnname))
                            PDLDetails += "\n" + "Column Name: " + PDLdbcolumnname;
                        lblPDL.ToolTip = PDLDetails.ToString();

                        Session["TopDepth"] = null;
                        decfirstTopDepth = Convert.ToDecimal(node.SelectNodes("attribute[@name='Top Depth']/@value")[0].Value.ToString());
                        //*addedby Dev
                        decfirstTopDepth = ConvertFeetMetre(decfirstTopDepth, GetMeasurementUnit(node.SelectNodes("attribute[@name='Top Depth']")[0]));
                        Session["TopDepth"] = Convert.ToString(Math.Round(decfirstTopDepth, 2)).ToString(); ;

                        Session["BottomDepth"] = null;
                        declastBottomDepth = Convert.ToDecimal(node.SelectNodes("attribute[@name='Bottom Depth']/@value")[0].Value.ToString());
                        //*added by Dev
                        declastBottomDepth = ConvertFeetMetre(declastBottomDepth, GetMeasurementUnit(node.SelectNodes("attribute[@name='Bottom Depth']")[0]));
                        Session["BottomDepth"] = Convert.ToString(Math.Round(declastBottomDepth, 2)).ToString();
                    }
                }
            }
            catch
            {
                pnlTable.Visible = false;
                pnlTableErrorMessage.Visible = true;
                lblTableErrorMessage.Visible = true;


            }

        }
        #endregion


        /// <summary>
        /// Adds the attribute.
        /// </summary>
        /// <param name="attName">Name of the att.</param>
        /// <param name="attOperator">The att operator.</param>
        /// <param name="valInnerText">The val inner text.</param>
        /// <returns></returns>
        private Attributes addAttribute(string attName, string attOperator, string valInnerText)
        {
            Attributes objAttribute = new Attributes();
            objAttribute.Name = attName.ToString();
            objAttribute.Operator = attOperator.ToString();
            objAttribute.Value = new ArrayList();
            Value objVal = new Value();
            objVal.InnerText = valInnerText.ToString();
            objAttribute.Value.Add(objVal);
            return objAttribute;

        }




        #region SetEntityTables()
        //This function is used to generate the Entity, When user clicked the Convert Button.
        /// <summary>
        /// Sets the entity tables.
        /// </summary>
        /// <returns></returns>
        private Entity SetEntityTables()
        {
            Entity objEntity = new Entity();

            //If User Entered values from Populate (Popup)
            if (Session["DepthValues"] != null)
            {
                strDepthValues = string.Empty;
                strDepthValues = Session["DepthValues"].ToString();
                arrDepthValueConvertPath = strDepthValues.Split('|');
                dblTopDepth = Convert.ToDouble(arrDepthValueConvertPath[0].ToString());
                dblBottomDepth = Convert.ToDouble(arrDepthValueConvertPath[1].ToString());
                intDepthInterval = Convert.ToInt32(arrDepthValueConvertPath[2].ToString());
                strDepthname = arrDepthValueConvertPath[3].ToString();
                //Used to find no.of rows need to create the HTML table.
                int index = Convert.ToInt32(Math.Round(((dblBottomDepth - dblTopDepth) / intDepthInterval), 0));
                int tblCount = tblConvertRows.Rows.Count;
            }
            objEntity.ResponseType = TABULAR;
            objEntity.AttributeGroups = new ArrayList();

            AttributeGroup objAttributeGrp = new AttributeGroup();
            objAttributeGrp.Operator = ANDOPERATOR;
            objAttributeGrp.Attribute = new ArrayList();

            objAttributeGrp.Attribute.Add(addAttribute("UWBI", "EQUALS", drpWellbore.SelectedItem.Value));

            introwCount = tblConvertRows.Rows.Count;

            Attributes objTopDepthAttribute = new Attributes();
            objTopDepthAttribute.Name = "Top Depth";
            objTopDepthAttribute.Value = new ArrayList();


            // This function is use to get the values from AH/TV textboxes.

            //this condition is used the get the values from AH depth textboxes.
            TextBox txtAHTopDepth0 = (TextBox)tblConvertRows.Rows[1].Cells[0].FindControl("txtAHDepth0");
            //Suppose, if User changed the drpDepthref value, then we need to covert the depth values from selected drpDepthref to Default Depth value.
            //Eg User entered value 100 into textbox, and user do not change the drpdepthref default value, then we can sent directly tgis value to webservice,
            //Suppose user entered 100 into textbox, and user changed the drpDepthRef default value(DF to GL,etc), then need to convert textbox value (100) to DF format : like (100 -GL) +DF
            if (hdnFirstAHDepthValue != null)
            {
                if (!string.IsNullOrEmpty(hdnFirstAHDepthValue.Value))
                {
                    txtAHTopDepth0.Text = hdnFirstAHDepthValue.Value;
                }
            }
            Decimal decDrpRefValue = Convert.ToDecimal(drpDepthReference.SelectedItem.Value);
            Decimal decHdnDrpRefValue = Convert.ToDecimal(Session["hidDrpDepthRefValue"].ToString());
            //*added by dev
            decDrpRefValue = ConvertFeetMetre(decDrpRefValue, hidDepthRefDefaultUnit.Value);
            decHdnDrpRefValue = ConvertFeetMetre(decHdnDrpRefValue, hidDepthRefDefaultUnit.Value);
            //end
            Decimal decResult;
            if (Session["hidDrpDepthRefText"].ToString().Equals(drpDepthReference.SelectedItem.Text))
            {
                Value objTopDepthVal = new Value();
                objTopDepthVal.InnerText = txtAHTopDepth0.Text;
                objTopDepthAttribute.Value.Add(objTopDepthVal);
            }
            else
            {
                Decimal decAHTopDepth0 = Convert.ToDecimal(txtAHTopDepth0.Text);
                decResult = (decAHTopDepth0 - decDrpRefValue) + decHdnDrpRefValue;
                Value objTopDepthVal = new Value();
                objTopDepthVal.InnerText = decResult.ToString();
                objTopDepthAttribute.Value.Add(objTopDepthVal);
            }

            // Convert all the textbox value into Default Depthref value, before send to Webservice.
            Decimal decAHDepth;
            TextBox txtTVDepth;
            Decimal decTVDepth;
            Decimal decDepth;
            //If user enter value from only populate popup. not in Add rows button clicked.
            if (!string.IsNullOrEmpty(strDepthname) && hidDepthMode.Value == null)
            {
                // Values created from AHDepth popup.
                if (strDepthname.Equals(strMD))
                {
                    for (int rowNumber = 1; rowNumber < introwCount - 2; rowNumber++)
                    {
                        TextBox txtAHDepth = (TextBox)tblConvertRows.Rows[rowNumber + 1].Cells[0].FindControl("txtAHDepth" + rowNumber);
                        if (txtAHDepth != null && !string.IsNullOrEmpty(txtAHDepth.Text))
                        {
                            //user is not change the default drpDepthRef
                            if (Session["hidDrpDepthRefText"].ToString().Equals(drpDepthReference.SelectedItem.Text))
                            {
                                Value objTopDepthVal = new Value();
                                objTopDepthVal.InnerText = txtAHDepth.Text;
                                objTopDepthAttribute.Value.Add(objTopDepthVal);
                            }
                            else
                            {
                                // if user changed the drpDepthRef, then first we need to calculate all the value to default drpref value, before sending the resquest xml to the webservice.
                                decAHDepth = Convert.ToDecimal(txtAHDepth.Text);
                                decResult = (decAHDepth - decDrpRefValue) + decHdnDrpRefValue;
                                Value objTopDepthVal = new Value();
                                objTopDepthVal.InnerText = decResult.ToString();
                                objTopDepthAttribute.Value.Add(objTopDepthVal);
                            }
                        }
                    }
                }
                else
                {
                    // Values created from TVDepth popup.
                    for (int rowNumber = 1; rowNumber < introwCount - 2; rowNumber++)
                    {
                        txtTVDepth = (TextBox)tblConvertRows.Rows[rowNumber + 1].Cells[0].FindControl("txtTVDepth" + rowNumber);
                        if (txtTVDepth != null && !string.IsNullOrEmpty(txtTVDepth.Text))
                        {
                            if (Session["hidDrpDepthRefText"].ToString().Equals(drpDepthReference.SelectedItem.Text))
                            {
                                Value objTopDepthVal = new Value();
                                objTopDepthVal.InnerText = txtTVDepth.Text;
                                objTopDepthAttribute.Value.Add(objTopDepthVal);
                            }
                            else
                            {
                                decTVDepth = Convert.ToDecimal(txtTVDepth.Text);
                                // if user changed the drpDepthRef, then first we need to calculate all the value to default drpref value, before sending the resquest xml to the webservice.
                                decResult = (decTVDepth - decDrpRefValue) + decHdnDrpRefValue;
                                Value objTopDepthVal = new Value();
                                objTopDepthVal.InnerText = decResult.ToString();
                                objTopDepthAttribute.Value.Add(objTopDepthVal);
                            }
                        }

                    }
                }
                //Suppose user directly enter the value into textbox, after click the Popup button, then need to clear below values.
                strDepthname = string.Empty;
            }
            else
            {
                // Used the get the values from Depth textboxes.

                TextBox txtDepth;
                if (hidDepthMode.Value != null)
                {
                    for (int rowNumber = 1; rowNumber < introwCount - 2; rowNumber++)
                    {
                        //If user directly entered into AH Depth textboxes.
                        if (hidDepthMode.Value.Equals(strMD))
                        {
                            txtDepth = (TextBox)tblConvertRows.Rows[rowNumber + 1].Cells[0].FindControl("txtAHDepth" + rowNumber);
                        }
                        else
                        {
                            //If user directly entered into TV Depth textboxes.
                            txtDepth = (TextBox)tblConvertRows.Rows[rowNumber + 1].Cells[2].FindControl("txtTVDepth" + rowNumber);

                        }
                        if (txtDepth != null && !string.IsNullOrEmpty(txtDepth.Text))
                        {
                            // Check whether user changed the drpDepthRef value or not, if changed then need to convert value to Default drpDepthref
                            if (Session["hidDrpDepthRefText"].ToString().Equals(drpDepthReference.SelectedItem.Text))
                            {
                                Value objTopDepthVal = new Value();
                                objTopDepthVal.InnerText = txtDepth.Text;
                                objTopDepthAttribute.Value.Add(objTopDepthVal);
                            }
                            else
                            {
                                decDepth = Convert.ToDecimal(txtDepth.Text);
                                // if user changed the drpDepthRef, then first we need to calculate all the value to default drpref value, before sending the resquest xml to the webservice.
                                decResult = (decDepth - decDrpRefValue) + decHdnDrpRefValue;
                                Value objTopDepthVal = new Value();
                                objTopDepthVal.InnerText = decResult.ToString();
                                objTopDepthAttribute.Value.Add(objTopDepthVal);
                            }
                        }

                    }
                }
            }

            inttblCount = tblConvertRows.Rows.Count - 2;

            //Get bottom depth value for gerentating the Request XML.
            TextBox txtAHBottomDepth = (TextBox)tblConvertRows.Rows[inttblCount].Cells[0].FindControl("txtAHDepth" + inttblCount);
            if (hdnLastAHDepthValue != null)
            {
                if (!string.IsNullOrEmpty(hdnLastAHDepthValue.Value))
                {
                    txtAHBottomDepth.Text = hdnLastAHDepthValue.Value;
                }
            }
            if (Session["hidDrpDepthRefText"].ToString().Equals(drpDepthReference.SelectedItem.Text))
            {
                Value objTopDepthVal = new Value();
                objTopDepthVal.InnerText = txtAHBottomDepth.Text;
                objTopDepthAttribute.Value.Add(objTopDepthVal);
            }
            else
            {
                Decimal decAHBottomDepth = Convert.ToDecimal(txtAHBottomDepth.Text);
                // if user changed the drpDepthRef, then first we need to calculate all the value to default drpref value, before sending the resquest xml to the webservice.
                decResult = (decAHBottomDepth - decDrpRefValue) + decHdnDrpRefValue;
                Value objTopDepthVal = new Value();
                objTopDepthVal.InnerText = decResult.ToString();
                objTopDepthAttribute.Value.Add(objTopDepthVal);
            }

            if (objTopDepthAttribute.Value.Count > 1)
            {
                objTopDepthAttribute.Operator = "IN";
                objAttributeGrp.Attribute.Add(objTopDepthAttribute);
            }
            if (rdoDepthUnitsMetres.Checked)
            {
                strInputDepthUnit = "m";
                stroutputDepthUnit = "m";
            }
            else
            {
                strInputDepthUnit = "feet";
                stroutputDepthUnit = "feet";
            }

            objAttributeGrp.Attribute.Add(addAttribute("Input Depth Unit", "EQUALS", strInputDepthUnit.ToString()));
            objAttributeGrp.Attribute.Add(addAttribute("Output Depth Unit", "EQUALS", stroutputDepthUnit.ToString()));
            objAttributeGrp.Attribute.Add(addAttribute("Projected Coordinated System", "EQUALS", lblResultProject.Text));
            objAttributeGrp.Attribute.Add(addAttribute("Wellbore Path", "EQUALS", lblResultWellborePath.Text));

            string strDepthModevalue = string.Empty;
            // if user enter the value only from Populate.
            //Some time user enter the value after populate the webservice response XML to table.
            if (!string.IsNullOrEmpty(strDepthname) && hidDepthMode.Value == null)
            {
                strDepthModevalue = strDepthname.ToString();
            }
            else
            {
                // after populate the webservice response in to table , then user may be enter the value AHdepth text box or TVDepth textbox.
                if (hidDepthMode.Value != null && !string.IsNullOrEmpty(hidDepthMode.Value.ToString()))
                {
                    strDepthModevalue = hidDepthMode.Value;
                }
                else
                {
                    strDepthModevalue = strMD;
                }
            }
            objAttributeGrp.Attribute.Add(addAttribute("Input Depth Mode", "EQUALS", strDepthModevalue.ToString()));
            tblConvertRows.EnableViewState = true;

            objEntity.AttributeGroups.Add(objAttributeGrp);



            return objEntity;


        }
        #endregion

        #region SetEntity()
        /// <summary>
        /// Sets the entity.
        /// </summary>
        /// <param name="agrVal">The agr val.</param>
        /// <returns></returns>
        private Entity SetEntity(string agrVal)
        {
            Entity objEntity = new Entity();
            if (agrVal.Equals(strUWBI))
            {
                if (HttpContext.Current.Request.Form["hidSelectedRows"] != null)
                {
                    Session["hidSelectedRows"] = null;
                    Session["hidSelectedRows"] = HttpContext.Current.Request.Form["hidSelectedRows"].ToString();
                }
                else
                {
                    strIdentifiedItem = Session["hidSelectedRows"].ToString();
                }
                if (HttpContext.Current.Request.Form["hidSelectedCriteriaName"] != null)
                {
                    Session["hidSelectedCriteriaName"] = null;
                    Session["hidSelectedCriteriaName"] = HttpContext.Current.Request.Form["hidSelectedCriteriaName"].ToString();
                    strIdentifierName = Session["hidSelectedCriteriaName"].ToString();
                }
                else
                {
                    strIdentifierName = Session["hidSelectedCriteriaName"].ToString();
                }
                hidSelectedCriteriaName.Value = Session["hidSelectedCriteriaName"].ToString();
                arrIdentifierValue = Session["hidSelectedRows"].ToString().Split('|');
                objEntity.Property = true;
                objEntity.ResponseType = TABULAR;
                if (hidSelectedCriteriaName.Value.Length > 0)
                {
                    ArrayList arlAttribute = new ArrayList();
                    arlAttribute = SetAtribute(arrIdentifierValue, strIdentifierName.ToString());
                    objEntity.Attribute = arlAttribute;
                }
            }
            else
            {
                objEntity.Property = true;
                objEntity.ResponseType = TABULAR;
                arrIdentifierValueFirst[0] = drpWellbore.SelectedItem.Value;
                ArrayList arlAttribute = new ArrayList();
                strIdentifierName = Session["hidSelectedCriteriaName"].ToString();
                arlAttribute = SetAtribute(arrIdentifierValueFirst, strIdentifierName.ToString());
                objEntity.Attribute = arlAttribute;
            }
            return objEntity;


        }
        #endregion

        #region SetAtribute()
        /// <summary>
        /// Sets the atribute.
        /// </summary>
        /// <param name="selectedCriteriaValues">The selected criteria values.</param>
        /// <param name="strIdentifierName">Name of the STR identifier.</param>
        /// <returns></returns>
        private ArrayList SetAtribute(string[] selectedCriteriaValues, string strIdentifierName)
        {
            ArrayList arlAttribute = new ArrayList();
            ArrayList arlValue = new ArrayList();
            Attributes objDetail = new Attributes();
            foreach (string strAttributeValue in selectedCriteriaValues)
            {
                int intCheckDuplicate = 0;
                if (strAttributeValue.Trim().Length > 0)
                {
                    //Loop through all the value object in ArrayList.
                    foreach (Value objValue in arlValue)
                    {
                        if (string.Equals(strAttributeValue.Trim(), objValue.InnerText.ToString()))
                        {
                            intCheckDuplicate++;
                        }
                    }
                    if (intCheckDuplicate == 0)
                    {
                        arlValue.Add(SetValue(strAttributeValue.Trim()));
                    }
                }
            }
            objDetail.Value = arlValue;
            objDetail.Operator = GetOperator(objDetail.Value);
            objDetail.Name = strIdentifierName.ToString();
            arlAttribute.Add(objDetail);
            return arlAttribute;



        }
        #endregion

        #region btnConvert_Clcik
        /// <summary>
        /// Handles the Click event of the btnConvert control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnConvert_Click(object sender, EventArgs e)
        {
            try
            {
                ServiceProvider objFactory = new ServiceProvider();
                objReportController = objFactory.GetServiceManager(ServiceName.ToString());
                objRequestInfo = SetDataObject(strAHTVDWeb);
                //Below methos is used to call the webservice, response XML will render into HTML table.
                xmlDocSearchResultAHTVD = objReportController.GetSearchResults(objRequestInfo, intMaxRecord,
                                                                               "AHTVCONVERTEDPATH", null, intSortOrder);
                clearAHTVDepthValues();
                PopulateDepthvalues(xmlDocSearchResultAHTVD);

            }
            catch (SoapException soapEx)
            {
                if (!string.Equals(soapEx.Message.ToString(), SOAPEXCEPTIONMESSAGE))
                {
                    CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                }
                RenderExceptionMessage(soapEx.Message.ToString());
            }
            catch (WebException webEx)
            {
                RenderExceptionMessage(webEx.Message.ToString());
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
        }
        #endregion

        #region PopulateDepthvalues()
        //Populate Depth values into HTML tables
        /// <summary>
        /// Populates the depthvalues.
        /// </summary>
        /// <param name="xmlResponse">The XML response.</param>
        private void PopulateDepthvalues(XmlDocument xmlResponse)
        {
            try
            {
                if (xmlResponse != null)
                {
                    int introwIndex = 0;
                    int intrecordCount = Convert.ToInt32(xmlResponse.SelectSingleNode("response/report/recordcount").InnerText);
                    int inttblLength = tblConvertRows.Rows.Count - 2;
                    XmlNodeList xmlNodeList = xmlResponse.SelectNodes("response/report/record");
                    string strDrpDepthRefValue = Session["hidDrpDepthRefValue"].ToString();

                    Decimal decDrpRefValue = Convert.ToDecimal(drpDepthReference.SelectedItem.Value);
                    Decimal decHdnDrpRefValue = Convert.ToDecimal(Session["hidDrpDepthRefValue"].ToString());
                    //*added by dev
                    decDrpRefValue = ConvertFeetMetre(decDrpRefValue, hidDepthRefDefaultUnit.Value);
                    decHdnDrpRefValue = ConvertFeetMetre(decHdnDrpRefValue, hidDepthRefDefaultUnit.Value);
                    //end
                    Decimal decResult;
                    Decimal decAHDepth;
                    TextBox txtAHDepth;
                    TextBox lblAHDepth;
                    Decimal declblAHDepthValue;
                    TextBox txtTVDepth;
                    TextBox lblTVDepth;
                    Decimal decTVDepth;
                    Decimal decTVresult;
                    TextBox lblXOffset;
                    Decimal declblTVDepthValue;
                    Decimal decXOffset;
                    TextBox lblYOffset;
                    Decimal decYoffset;
                    TextBox lblEasting;
                    Decimal decEasting;
                    TextBox lblNorthing;
                    Decimal decNorthing;

                    Decimal decDrpDepthRefValue = Convert.ToDecimal(strDrpDepthRefValue.ToString());

                    //Populate  the response XLL value to Depth textboxes.
                    foreach (XmlNode node in xmlNodeList)
                    {
                        //Populate  the response XLL value to Depth textboxes except last XML value.
                        if (intrecordCount != Convert.ToInt32(node.Attributes["recordno"].Value))
                        {

                            txtAHDepth = (TextBox)tblConvertRows.Rows[introwIndex + 1].Cells[0].FindControl("txtAHDepth" + introwIndex);
                            txtAHDepth.Text = Convert.ToString(Math.Round(Convert.ToDecimal(Convert.ToDouble(node.SelectNodes("attribute[@name='Along Hole Depth']/@value")[0].Value)), 2).ToString("#0.00"));

                            decAHDepth = Convert.ToDecimal(txtAHDepth.Text);
                            decDrpRefValue = Convert.ToDecimal(drpDepthReference.SelectedItem.Value);
                            decHdnDrpRefValue = Convert.ToDecimal(Session["hidDrpDepthRefValue"].ToString());
                            //*added by dev
                            decDrpRefValue = ConvertFeetMetre(decDrpRefValue, hidDepthRefDefaultUnit.Value);
                            decHdnDrpRefValue = ConvertFeetMetre(decHdnDrpRefValue, hidDepthRefDefaultUnit.Value);
                            //end
                            // if user changed the drpDepthRef, then first we need to calculate all the value to default drpref value, before sending the resquest xml to the webservice.
                            //after getting response from webservice, we need to calculated the values with respective to the selected drpdepthref dropdown values.
                            decResult = (decAHDepth - decHdnDrpRefValue) + decDrpRefValue;
                            txtAHDepth.Text = decResult.ToString("#0.00");

                            lblAHDepth = (TextBox)tblConvertRows.Rows[introwIndex + 1].Cells[1].FindControl("lblAHDepth" + introwIndex);
                            declblAHDepthValue = Convert.ToDecimal(Convert.ToDouble(node.SelectNodes("attribute[@name='Along Hole Depth']/@value")[0].Value));
                            //UAT fix on 9/12/2010
                            //start
                            declblAHDepthValue = ConvertFeetMetre(declblAHDepthValue, hidDepthRefDefaultUnit.Value);
                            decDrpDepthRefValue = ConvertFeetMetre(decDrpDepthRefValue, hidDepthRefDefaultUnit.Value);
                            //end
                            // if user changed the drpDepthRef, then first we need to calculate all the value to default drpref value, before sending the resquest xml to the webservice.
                            //after getting response from webservice, we need to calculated the values with respective to the selected drpdepthref dropdown values.

                            lblAHDepth.Text = Convert.ToString(Math.Round((declblAHDepthValue - decDrpDepthRefValue), 2).ToString("#0.00")).ToString();

                            txtTVDepth = (TextBox)tblConvertRows.Rows[introwIndex + 1].Cells[2].FindControl("txtTVDepth" + introwIndex);
                            txtTVDepth.Text = Convert.ToString(Math.Round(Convert.ToDecimal(Convert.ToDouble(node.SelectNodes("attribute[@name='True Vertical Depth']/@value")[0].Value)), 2).ToString("#0.00"));

                            decTVDepth = Convert.ToDecimal(txtTVDepth.Text);
                            // if user changed the drpDepthRef, then first we need to calculate all the value to default drpref value, before sending the resquest xml to the webservice.
                            //after getting response from webservice, we need to calculated the values with respective to the selected drpdepthref dropdown values.
                            decTVresult = (decTVDepth - decHdnDrpRefValue) + decDrpRefValue;
                            txtTVDepth.Text = decTVresult.ToString("#0.00");


                            lblTVDepth = (TextBox)tblConvertRows.Rows[introwIndex + 1].Cells[3].FindControl("lblTVDepth" + introwIndex);
                            declblTVDepthValue = Convert.ToDecimal(Convert.ToDouble(node.SelectNodes("attribute[@name='True Vertical Depth']/@value")[0].Value));
                            // if user changed the drpDepthRef, then first we need to calculate all the value to default drpref value, before sending the resquest xml to the webservice.
                            //after getting response from webservice, we need to calculated the values with respective to the selected drpdepthref dropdown values.
                            lblTVDepth.Text = Convert.ToString(Math.Round((declblTVDepthValue - decDrpDepthRefValue), 2).ToString("#0.00")).ToString();

                            lblXOffset = (TextBox)tblConvertRows.Rows[introwIndex + 1].Cells[4].FindControl("lblXOffset" + introwIndex);
                            decXOffset = Convert.ToDecimal(Convert.ToDouble(node.SelectNodes("attribute[@name='X Offset']/@value")[0].Value));
                            lblXOffset.Text = Convert.ToString(Math.Round(decXOffset, 2).ToString("#0.00")).ToString();


                            lblYOffset = (TextBox)tblConvertRows.Rows[introwIndex + 1].Cells[5].FindControl("lblYOffset" + introwIndex);
                            decYoffset = Convert.ToDecimal(Convert.ToDouble(node.SelectNodes("attribute[@name='Y Offset']/@value")[0].Value));
                            lblYOffset.Text = Convert.ToString(Math.Round(decYoffset, 2).ToString("#0.00")).ToString();

                            lblEasting = (TextBox)tblConvertRows.Rows[introwIndex + 1].Cells[6].FindControl("lblEasting" + introwIndex);
                            decEasting = Convert.ToDecimal(Convert.ToDouble(node.SelectNodes("attribute[@name='Easting']/@value")[0].Value));
                            lblEasting.Text = Convert.ToString(Math.Round(decEasting, 2).ToString("#0.00")).ToString();

                            lblNorthing = (TextBox)tblConvertRows.Rows[introwIndex + 1].Cells[7].FindControl("lblNorthing" + introwIndex);
                            decNorthing = Convert.ToDecimal(Convert.ToDouble(node.SelectNodes("attribute[@name='Northing']/@value")[0].Value));
                            lblNorthing.Text = Convert.ToString(Math.Round(decNorthing, 2).ToString("#0.00")).ToString();
                            introwIndex = introwIndex + 1;
                        }



                        else
                        {
                            //Display last response XML value in to Last row of the table.
                            txtAHDepth = (TextBox)tblConvertRows.Rows[inttblLength].Cells[0].FindControl("txtAHDepth" + inttblLength);
                            txtAHDepth.Text = Convert.ToString(Math.Round(Convert.ToDecimal(Convert.ToDouble(node.SelectNodes("attribute[@name='Along Hole Depth']/@value")[0].Value)), 2).ToString("#0.00"));

                            lblAHDepth = (TextBox)tblConvertRows.Rows[inttblLength].Cells[1].FindControl("lblAHDepth" + inttblLength);
                            declblAHDepthValue = Convert.ToDecimal(Convert.ToDouble(node.SelectNodes("attribute[@name='Along Hole Depth']/@value")[0].Value));
                            // if user changed the drpDepthRef, then first we need to calculate all the value to default drpref value, before sending the resquest xml to the webservice.
                            //after getting response from webservice, we need to calculated the values with respective to the selected drpdepthref dropdown values.
                            lblAHDepth.Text = Convert.ToString(Math.Round((declblAHDepthValue - decDrpDepthRefValue), 2).ToString("#0.00")).ToString();

                            txtTVDepth = (TextBox)tblConvertRows.Rows[inttblLength].Cells[2].FindControl("txtTVDepth" + inttblLength);
                            txtTVDepth.Text = Convert.ToString(Math.Round(Convert.ToDecimal(Convert.ToDouble(node.SelectNodes("attribute[@name='True Vertical Depth']/@value")[0].Value).ToString("#0.00")), 2));

                            lblTVDepth = (TextBox)tblConvertRows.Rows[inttblLength].Cells[3].FindControl("lblTVDepth" + inttblLength);
                            declblTVDepthValue = Convert.ToDecimal(Convert.ToDouble(node.SelectNodes("attribute[@name='True Vertical Depth']/@value")[0].Value));
                            // if user changed the drpDepthRef, then first we need to calculate all the value to default drpref value, before sending the resquest xml to the webservice.
                            //after getting response from webservice, we need to calculated the values with respective to the selected drpdepthref dropdown values.
                            lblTVDepth.Text = Convert.ToString(Math.Round((declblTVDepthValue - decDrpDepthRefValue), 2).ToString("#0.00")).ToString();

                            lblXOffset = (TextBox)tblConvertRows.Rows[inttblLength].Cells[4].FindControl("lblXOffset" + inttblLength);
                            decXOffset = Convert.ToDecimal(Convert.ToDouble(node.SelectNodes("attribute[@name='X Offset']/@value")[0].Value));
                            lblXOffset.Text = Convert.ToString(Math.Round(decXOffset, 2).ToString("#0.00")).ToString();


                            lblYOffset = (TextBox)tblConvertRows.Rows[inttblLength].Cells[5].FindControl("lblYOffset" + inttblLength);
                            decYoffset = Convert.ToDecimal(Convert.ToDouble(node.SelectNodes("attribute[@name='Y Offset']/@value")[0].Value));
                            lblYOffset.Text = Convert.ToString(Math.Round(decYoffset, 2).ToString("#0.00")).ToString();

                            lblEasting = (TextBox)tblConvertRows.Rows[inttblLength].Cells[6].FindControl("lblEasting" + inttblLength);
                            decEasting = Convert.ToDecimal(Convert.ToDouble(node.SelectNodes("attribute[@name='Easting']/@value")[0].Value));
                            lblEasting.Text = Convert.ToString(Math.Round(decEasting, 2).ToString("#0.00")).ToString();

                            lblNorthing = (TextBox)tblConvertRows.Rows[inttblLength].Cells[7].FindControl("lblNorthing" + inttblLength);
                            decNorthing = Convert.ToDecimal(Convert.ToDouble(node.SelectNodes("attribute[@name='Northing']/@value")[0].Value));
                            lblNorthing.Text = Convert.ToString(Math.Round(decNorthing, 2).ToString("#0.00")).ToString();

                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region CreateTDHeaderControl()
        /// <summary>
        /// Creates the TD header control.
        /// </summary>
        /// <param name="tdName">Name of the td.</param>
        /// <param name="tdCSS">The td CSS.</param>
        /// <param name="lblName">Name of the LBL.</param>
        /// <param name="lblText">The LBL text.</param>
        /// <param name="lblCSS">The LBL CSS.</param>
        /// <returns></returns>
        private TableHeaderCell CreateTDHeaderControl(string tdName, string tdCSS, string lblName, string lblText, string lblCSS)
        {

            TableHeaderCell tdHeader = new TableHeaderCell();


            tdHeader.ID = tdName.ToString();
            tdHeader.CssClass = tdCSS.ToString();
            Label lblDepth = new Label();
            lblDepth.ID = lblName.ToString();
            lblDepth.Text = lblText.ToString();
            lblDepth.CssClass = lblCSS.ToString();
            tdHeader.Controls.Add(lblDepth);
            return tdHeader;
        }
        #endregion

        #region CreateTDWithControls()
        /// <summary>
        /// Creates the TD with controls.
        /// </summary>
        /// <param name="tdName">Name of the td.</param>
        /// <param name="tdCSS">The td CSS.</param>
        /// <param name="txtBoxName">Name of the TXT box.</param>
        /// <param name="txtBoxCSS">The TXT box CSS.</param>
        /// <param name="i">The increment (i).</param>
        /// <param name="index">The index.</param>
        /// <param name="readonlyVal">The readonly val.</param>
        /// <returns></returns>
        private TableCell CreateTDWithControls(string tdName, string tdCSS, string txtBoxName, string txtBoxCSS, int intCol, int index, string readonlyVal)
        {
            TableCell td = new TableCell();
            td.ID = tdName.ToString() + intCol.ToString();
            TextBox txtBox = new TextBox();
            txtBox.ID = txtBoxName + intCol.ToString();
            txtBox.Style.Add("text-align", "right");
            //restrict to type some value to First and Last rows of the textboxes.

            if (intCol == 0 || intCol == index - 1)
            {
                txtBox.Attributes.Add("readonly", "readonly");
            }
            //restrict to user type values into no editable text boxes.
            if (!string.IsNullOrEmpty(readonlyVal.ToString()))
            {
                txtBox.Attributes.Add("readonly", "readonly");
            }
            else
            {
                //Set Tab index for editable text boxes.
                if (txtBoxName.Equals("txtAHDepth"))
                {
                    {
                        if (hdnTabIndex != null)
                        {
                            txtBox.TabIndex = (short)Convert.ToInt32(hdnTabIndex.Value);
                            hdnTabIndex.Value = Convert.ToString(Convert.ToInt32(hdnTabIndex.Value) + 1);
                        }
                    }
                }
                if (txtBoxName.Equals("txtTVDepth"))
                {
                    if (hdnTabIndex != null)
                    {
                        txtBox.TabIndex = (short)Convert.ToInt32(hdnTabIndex.Value);
                        hdnTabIndex.Value = Convert.ToString(Convert.ToInt32(hdnTabIndex.Value) + 1);
                    }

                }
            }

            txtBox.CssClass = txtBoxCSS.ToString();
            td.CssClass = tdCSS.ToString();
            td.Controls.Add(txtBox);
            return td;
        }
        #endregion

        #region CreateTDwithHeaderBtnControl()
        /// <summary>
        /// Creates the T dwith header BTN control.
        /// </summary>
        /// <param name="tdName">Name of the td.</param>
        /// <param name="tdCSS">The td CSS.</param>
        /// <param name="lblName">Name of the LBL.</param>
        /// <param name="lblText">The LBL text.</param>
        /// <param name="lblCSS">The LBL CSS.</param>
        /// <param name="btnName">Name of the BTN.</param>
        /// <param name="btnText">The BTN text.</param>
        /// <param name="btnCSS">The BTN CSS.</param>
        /// <returns></returns>
        private TableHeaderCell CreateTDwithHeaderBtnControl(string tdName, string tdCSS, string lblName, string lblText, string lblCSS, string btnName, string btnText, string btnCSS)
        {

            TableHeaderCell tdHeader = new TableHeaderCell();


            tdHeader.ID = tdName.ToString();
            tdHeader.CssClass = tdCSS.ToString();

            Label lblHead = new Label();
            lblHead.ID = lblName.ToString();
            lblHead.Text = lblText.ToString();
            lblHead.CssClass = lblCSS.ToString();


            Button btnHead = new Button();
            btnHead.ID = btnName.ToString();
            btnHead.Text = btnText.ToString();
            btnHead.CssClass = btnCSS.ToString();
            tdHeader.Controls.Add(lblHead);
            tdHeader.Controls.Add(btnHead);
            return tdHeader;
        }
        #endregion

        #region GenerateRows()
        /// <summary>
        /// Generates the rows.
        /// </summary>
        /// <param name="min">The min.</param>
        /// <param name="index">The index.</param>
        /// <param name="times">The times.</param>
        private void GenerateRows(int min, int index, string times)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<html>");
                sb.Append("<head>");
                sb.Append("</head>");
                sb.Append("</html>");
                this.Controls.Add(new LiteralControl(sb.ToString()));


                tblConvertRows.ID = "tblConvertRows";
                tblConvertRows.CssClass = "tblConvertRows";
                tblConvertRows.CellPadding = 0;
                tblConvertRows.CellSpacing = 0;

                if (times.Equals(strtblWithHeaderRow))
                {


                    #region header Control

                    TableHeaderRow trHeader = new TableHeaderRow();
                    trHeader.TableSection = TableRowSection.TableHeader;
                    trHeader.CssClass = "fixedHeader";

                    trHeader.Cells.Add(CreateTDwithHeaderBtnControl("tdAHDepthHead", "tdAHDepthBtn", "lblAHDepthHead", "AH Depth [DF] (ft)&nbsp;&nbsp;", "labelMessage2", "btnAHDepthHead", "Populate", "buttonAdvSrch"));

                    trHeader.Cells.Add(CreateTDHeaderControl("tdAHDepthHeade1", "tdAHDepthHeadSpan", "lblAHDepthHead1", "AH Depth [PDL] (ft)", "labelMessage2"));
                    trHeader.Cells.Add(CreateTDwithHeaderBtnControl("tdTVDepthHead", "tdAHDepthBtn", "lblTVDepthHead", "TV Depth &nbsp;&nbsp;", "labelMessage2", "btnTVDepthHead", "Populate", "buttonAdvSrch"));
                    trHeader.Cells.Add(CreateTDHeaderControl("tdTVDepthHead1", "tdAHDepthHeadSpan", "lblTVDepthHead1", "TV Depth [PDL] (ft)", "labelMessage2"));
                    trHeader.Cells.Add(CreateTDHeaderControl("tdXoffsetHead", "tdAMiscHead", "lblXOffsetHead", "X Offset (m)", "labelMessage2"));
                    trHeader.Cells.Add(CreateTDHeaderControl("tdYoffsetHead", "tdAMiscHead", "lblYOffsetHead", "Y Offset (m)", "labelMessage2"));
                    trHeader.Cells.Add(CreateTDHeaderControl("tdEastingHead", "tdAMiscHead", "lblEastingHead", "Easting (m)", "labelMessage2"));
                    trHeader.Cells.Add(CreateTDHeaderControl("tdNorthingHead", "tdAMiscHead", "lblNorthingHead", "Northing (m)", "labelMessage2"));



                    tblConvertRows.Rows.Add(trHeader);
                    #endregion
                }
                for (int intMin = min; intMin < index; intMin++)
                {
                    TableRow tr = new TableRow();
                    tr.ID = "tr" + intMin;
                    string strTxtCssName = string.Empty;
                    string strtdCssName = string.Empty;
                    string readonlyVal = string.Empty;
                    string readonlyVal1 = "false";
                    if (intMin == 0 || intMin == index - 1)
                    {
                        strTxtCssName = "AHDepthlbl";
                        strtdCssName = "tdAHDepthSpn";
                        readonlyVal = string.Empty;


                    }
                    else
                    {
                        strTxtCssName = "AHTVTextBox";
                        strtdCssName = "tdAHDepth";
                        readonlyVal = string.Empty;

                    }
                    tr.Cells.Add(CreateTDWithControls("tdAHDepth", strtdCssName.ToString(), "txtAHDepth", strTxtCssName.ToString(), intMin, index, readonlyVal));
                    tr.Cells.Add(CreateTDWithControls("tdAHDepthSpn", "tdAHDepthSpn", "lblAHDepth", "AHDepthlbl", intMin, index, readonlyVal1));

                    if (intMin == 0 || intMin == index - 1)
                    {
                        strTxtCssName = "AHDepthlbl";
                        strtdCssName = "tdAHDepthSpn";
                        readonlyVal = string.Empty;

                    }
                    else
                    {
                        strTxtCssName = "AHTVTextBox";
                        strtdCssName = "tdTVDepth";
                        readonlyVal = string.Empty;

                    }

                    tr.Cells.Add(CreateTDWithControls("tdTVDepth", strtdCssName.ToString(), "txtTVDepth", strTxtCssName.ToString(), intMin, index, readonlyVal));
                    tr.Cells.Add(CreateTDWithControls("tdTVDepthSpn", "tdTVDepthSpn", "lblTVDepth", "AHDepthlbl", intMin, index, readonlyVal1));
                    tr.Cells.Add(CreateTDWithControls("tdXOffset", "tdXOffset", "lblXOffset", "AHDepthlbl", intMin, index, readonlyVal1));
                    tr.Cells.Add(CreateTDWithControls("tdYOffset", "tdYOffset", "lblYOffset", "AHDepthlbl", intMin, index, readonlyVal1));
                    tr.Cells.Add(CreateTDWithControls("tdEasting", "tdEasting", "lblEasting", "AHDepthlbl", intMin, index, readonlyVal1));
                    tr.Cells.Add(CreateTDWithControls("tdNorthing", "tdNorthing", "lblNorthing", "AHDepthlbl", intMin, index, readonlyVal1));
                    tblConvertRows.Rows.Add(tr);
                    pnl.Controls.Add(tblConvertRows);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region btnAddRows_Click
        //This Add Row is use to add the number rows into the HTML talbes.
        /// <summary>
        /// Handles the Click event of the btnAddRows control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnAddRows_Click(object sender, EventArgs e)
        {
            try
            {
                //Assign last value into Last textboxes.
                int intcnt = tblConvertRows.Rows.Count - 2;
                TextBox txtAHDepth = (TextBox)tblConvertRows.Rows[intcnt].Cells[0].FindControl("txtAHDepth" + intcnt);
                TextBox lblAHDepth = (TextBox)tblConvertRows.Rows[intcnt].Cells[0].FindControl("lblAHDepth" + intcnt);
                TextBox txtTVDepth = (TextBox)tblConvertRows.Rows[intcnt].Cells[0].FindControl("txtTVDepth" + intcnt);
                TextBox lblTVDepth = (TextBox)tblConvertRows.Rows[intcnt].Cells[0].FindControl("lblTVDepth" + intcnt);
                TextBox lblXOffset = (TextBox)tblConvertRows.Rows[intcnt].Cells[0].FindControl("lblXOffset" + intcnt);
                TextBox lblYOffset = (TextBox)tblConvertRows.Rows[intcnt].Cells[0].FindControl("lblYOffset" + intcnt);
                TextBox lblEasting = (TextBox)tblConvertRows.Rows[intcnt].Cells[0].FindControl("lblEasting" + intcnt);
                TextBox lblNorthing = (TextBox)tblConvertRows.Rows[intcnt].Cells[0].FindControl("lblNorthing" + intcnt);
                int tbllength;
                if (Session["tblConvertsRowsCount"] != null)
                {
                    tbllength = Convert.ToInt32(Session["tblConvertsRowsCount"].ToString());
                }
                else
                {
                    tbllength = Convert.ToInt32(tblConvertRows.Rows.Count) - 1;
                }
                int inthid = Convert.ToInt32(hidRows.Value);
                int intMaxLength = tbllength + inthid + 1;

                Session["tblConvertsRowsCount"] = intMaxLength.ToString();
                GenerateRows(tbllength, intMaxLength, strtblWithOutHeaderRow);

                int intcnt1 = tblConvertRows.Rows.Count - 2;

                TextBox txtAHDepth1 = (TextBox)tblConvertRows.Rows[intcnt1].Cells[0].FindControl("txtAHDepth" + intcnt1);
                txtAHDepth1.Text = txtAHDepth.Text;
                txtAHDepth.Text = string.Empty;

                TextBox lblAHDepth1 = (TextBox)tblConvertRows.Rows[intcnt1].Cells[0].FindControl("lblAHDepth" + intcnt1);
                lblAHDepth1.Text = lblAHDepth.Text;
                lblAHDepth.Text = string.Empty;

                TextBox txtTVDepth1 = (TextBox)tblConvertRows.Rows[intcnt1].Cells[0].FindControl("txtTVDepth" + intcnt1);
                txtTVDepth1.Text = txtTVDepth.Text;
                txtTVDepth.Text = string.Empty;

                TextBox lblTVDepth1 = (TextBox)tblConvertRows.Rows[intcnt1].Cells[0].FindControl("lblTVDepth" + intcnt1);
                lblTVDepth1.Text = lblTVDepth.Text;
                lblTVDepth.Text = string.Empty;

                TextBox lblXOffset1 = (TextBox)tblConvertRows.Rows[intcnt1].Cells[0].FindControl("lblXOffset" + intcnt1);
                lblXOffset1.Text = lblXOffset.Text;
                lblXOffset.Text = string.Empty;

                TextBox lblYOffset1 = (TextBox)tblConvertRows.Rows[intcnt1].Cells[0].FindControl("lblYOffset" + intcnt1);
                lblYOffset1.Text = lblYOffset.Text;
                lblYOffset.Text = string.Empty;

                TextBox lblEasting1 = (TextBox)tblConvertRows.Rows[intcnt1].Cells[0].FindControl("lblEasting" + intcnt1);
                lblEasting1.Text = lblEasting.Text;
                lblEasting.Text = string.Empty;

                TextBox lblNorthing1 = (TextBox)tblConvertRows.Rows[intcnt1].Cells[0].FindControl("lblNorthing" + intcnt1);
                lblNorthing1.Text = lblNorthing.Text;
                lblNorthing.Text = string.Empty;

                SetCSSforEditableAHTVdepth();

            }
            catch (SoapException)
            {
                throw;
            }
            catch (WebException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region FillDepthrefSelectedValue()
        /// <summary>
        /// Fills the depthref selected value.
        /// </summary>
        private void FillDepthrefSelectedValue()
        {
            if (Session["hidDrpDepthRefText"] != null)
            {
                if ((drpDepthReference.SelectedItem != null) && (drpDepthReference.Items.FindByText(drpDepthReference.SelectedItem.Text) != null))
                    drpDepthReference.Items.FindByText(drpDepthReference.SelectedItem.Text).Selected = false;
                if (drpDepthReference.Items.FindByText(Session["hidDrpDepthRefText"].ToString()) != null)
                    drpDepthReference.Items.FindByText(Session["hidDrpDepthRefText"].ToString()).Selected = true;

                Label lblAHDepthHead = (Label)tblConvertRows.Rows[0].Cells[0].FindControl("lblAHDepthHead");
                lblAHDepthHead.Text = "AH Depth [" + Session["hidDrpDepthRefText"].ToString() + "]" + strFeetMetre + "&nbsp";

                Label lblAHDepthHead1 = (Label)tblConvertRows.Rows[0].Cells[1].FindControl("lblAHDepthHead1");
                lblAHDepthHead1.Text = "AH Depth [PDL]" + strFeetMetre + "&nbsp";


                Label lblTVDepthHead = (Label)tblConvertRows.Rows[0].Cells[2].FindControl("lblTVDepthHead");
                lblTVDepthHead.Text = "TV Depth [" + Session["hidDrpDepthRefText"].ToString() + "] " + strFeetMetre + "&nbsp";

                Label lblTVDepthHead1 = (Label)tblConvertRows.Rows[0].Cells[3].FindControl("lblTVDepthHead1");
                lblTVDepthHead1.Text = "TV Depth [PDL] " + strFeetMetre + "&nbsp";

                Label lblXOffsetHead = (Label)tblConvertRows.Rows[0].Cells[4].FindControl("lblXOffsetHead");
                lblXOffsetHead.Text = "X Offset (m)&nbsp";

                Label lblYOffsetHead = (Label)tblConvertRows.Rows[0].Cells[5].FindControl("lblYOffsetHead");
                lblYOffsetHead.Text = "Y Offset (m)&nbsp";

                Label lblEastingHead = (Label)tblConvertRows.Rows[0].Cells[6].FindControl("lblEastingHead");
                lblEastingHead.Text = "Easting (m)&nbsp";

                Label lblNorthingHead = (Label)tblConvertRows.Rows[0].Cells[7].FindControl("lblNorthingHead");
                lblNorthingHead.Text = "Northing (m)&nbsp";

            }
            if (hidDrpValue != null)
            {
                if (!string.IsNullOrEmpty(hidDrpValue.Value))
                {
                    if ((drpDepthReference.SelectedItem != null) && (drpDepthReference.Items.FindByText(drpDepthReference.SelectedItem.Text) != null))
                        drpDepthReference.Items.FindByText(drpDepthReference.SelectedItem.Text).Selected = false;
                    if (drpDepthReference.Items.FindByText(hidDrpValue.Value) != null)
                        drpDepthReference.Items.FindByText(hidDrpValue.Value).Selected = true;

                    Label lblAHDepthHead = (Label)tblConvertRows.Rows[0].Cells[0].FindControl("lblAHDepthHead");
                    lblAHDepthHead.Text = "AH Depth [" + hidDrpValue.Value + "] " + strFeetMetre + "&nbsp";

                    Label lblAHDepthHead1 = (Label)tblConvertRows.Rows[0].Cells[1].FindControl("lblAHDepthHead1");
                    lblAHDepthHead1.Text = "AH Depth [PDL] " + strFeetMetre + "&nbsp";

                    Label lblTVDepthHead = (Label)tblConvertRows.Rows[0].Cells[2].FindControl("lblTVDepthHead");
                    lblTVDepthHead.Text = "TV Depth [" + hidDrpValue.Value + "] " + strFeetMetre + "&nbsp";

                    Label lblTVDepthHead1 = (Label)tblConvertRows.Rows[0].Cells[3].FindControl("lblTVDepthHead1");
                    lblTVDepthHead1.Text = "TV Depth [PDL] " + strFeetMetre + "&nbsp";

                    Label lblXOffsetHead = (Label)tblConvertRows.Rows[0].Cells[4].FindControl("lblXOffsetHead");
                    lblXOffsetHead.Text = "X Offset (m)&nbsp";

                    Label lblYOffsetHead = (Label)tblConvertRows.Rows[0].Cells[5].FindControl("lblYOffsetHead");
                    lblYOffsetHead.Text = "Y Offset (m)&nbsp";

                    Label lblEastingHead = (Label)tblConvertRows.Rows[0].Cells[6].FindControl("lblEastingHead");
                    lblEastingHead.Text = "Easting (m)&nbsp";

                    Label lblNorthingHead = (Label)tblConvertRows.Rows[0].Cells[7].FindControl("lblNorthingHead");
                    lblNorthingHead.Text = "Northing (m)&nbsp";
                }
            }

        }
        #endregion

        #region SetCSSforEditableAHTVdepth()
        /// <summary>
        /// Sets the CSS for editable AH/ TV depth.
        /// </summary>
        private void SetCSSforEditableAHTVdepth()
        {
            int inttblCount = tblConvertRows.Rows.Count;
            for (int index = 1; index < inttblCount - 2; index++)
            {
                TextBox txtAHDepth = (TextBox)tblConvertRows.Rows[index + 1].Cells[0].FindControl("txtAHDepth" + index);
                txtAHDepth.CssClass = "AHTVTextBox";
                txtAHDepth.Attributes.Remove("readonly");

                TextBox txtTVDepth = (TextBox)tblConvertRows.Rows[index + 1].Cells[2].FindControl("txtTVDepth" + index);
                txtTVDepth.CssClass = "AHTVTextBox";
                txtTVDepth.Attributes.Remove("readonly");

            }
            btnConvert.Focus();
            Button btnAHDepthHead = (Button)tblConvertRows.Rows[0].Cells[0].FindControl("btnAHDepthHead");
            btnAHDepthHead.Attributes.Add("onClick", "return openAHDepthPopup();");

            Button btnTVDepthHead = (Button)tblConvertRows.Rows[0].Cells[2].FindControl("btnTVDepthHead");
            btnTVDepthHead.Attributes.Add("onClick", "return openTVDepthPopup();");
        }
        #endregion

        #region clearAHTVDepthValues()
        /// <summary>
        /// Clears the AH/TV depth values.
        /// </summary>
        private void clearAHTVDepthValues()
        {
            for (int index = 1; index < tblConvertRows.Rows.Count - 2; index++)
            {

                TextBox txtAHDepth = (TextBox)tblConvertRows.Rows[index].Cells[0].FindControl("txtAHDepth" + index);
                txtAHDepth.Text = string.Empty;

                TextBox lblAHDepth = (TextBox)tblConvertRows.Rows[index].Cells[1].FindControl("lblAHDepth" + index);
                lblAHDepth.Text = string.Empty;

                TextBox txtTVDepth = (TextBox)tblConvertRows.Rows[index].Cells[2].FindControl("txtTVDepth" + index);
                txtTVDepth.Text = string.Empty;

                TextBox lblTVDepth = (TextBox)tblConvertRows.Rows[index].Cells[3].FindControl("lblTVDepth" + index);
                lblTVDepth.Text = string.Empty;

                TextBox lblXOffset = (TextBox)tblConvertRows.Rows[index].Cells[4].FindControl("lblXOffset" + index);
                lblXOffset.Text = string.Empty;

                TextBox lblYOffset = (TextBox)tblConvertRows.Rows[index].Cells[5].FindControl("lblYOffset" + index);
                lblYOffset.Text = string.Empty;

                TextBox lblEasting = (TextBox)tblConvertRows.Rows[index].Cells[6].FindControl("lblEasting" + index);
                lblEasting.Text = string.Empty;

                TextBox lblNorthing = (TextBox)tblConvertRows.Rows[index].Cells[7].FindControl("lblNorthing" + index);
                lblNorthing.Text = string.Empty;

            }
        }
        #endregion

        #region RenderExceptionMessage()
        /// <summary>
        /// Renders the exception message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void RenderExceptionMessage(string message)
        {

            pnlSoapError.Visible = true;
            lblErrorMessage.Visible = true;
            pnlConverterContent.Visible = false;
            if (!string.Equals(message, SOAPEXCEPTIONMESSAGE))//In case of no records error back button will not be displayed
            {
                btn_goBack.Visible = true;
            }
            lblErrorMessage.Text = message;
            pnlTable.Visible = false;
            pnlTableErrorMessage.Visible = false;

        }
        #endregion
        #region Added For Feet meter convertion

        private string GetCurrentUnit()
        {
            string strUnit = string.Empty;
            if (rdoDepthUnitsFeet.Checked)
            {
                strUnit = "feet";
            }
            else
            {
                strUnit = "metres";
            }
            return strUnit;
        }
        /// <summary>
        /// Converts the feet metre.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="currentUnit">The current unit.</param>
        /// <returns></returns>
        private Decimal ConvertFeetMetre(Decimal value, string currentUnit)
        {
            string strUserSelectedUnit = GetCurrentUnit();
            Decimal formulaValue = (Decimal)3.28084;
            Decimal strConvertedValue = value;
            if (strUserSelectedUnit.ToLower().Equals(currentUnit.ToLower()))
            {
                strConvertedValue = value;
            }
            else if (strUserSelectedUnit.ToLower().Equals("metres"))
            {
                strConvertedValue = (value / formulaValue);
            }
            else if (strUserSelectedUnit.ToLower().Equals("feet"))
            {
                strConvertedValue = (value * formulaValue);
            }

            return strConvertedValue;
        }
        /// <summary>
        /// Gets the measurement unit.
        /// </summary>
        /// <param name="currentNode">The current node.</param>
        /// <returns></returns>
        private string GetMeasurementUnit(XmlNode currentNode)
        {
            string strUnit = string.Empty;
            string strRefColName = string.Empty;
            if ((currentNode != null) && (currentNode.Attributes["referencecolumn"] != null) && (!string.IsNullOrEmpty((strRefColName = currentNode.Attributes["referencecolumn"].Value))))
            {
                XmlNode objXmlNode = currentNode.ParentNode.SelectSingleNode("attribute[@name ='" + strRefColName + "']");
                if (objXmlNode != null)
                {
                    strUnit = objXmlNode.Attributes["value"].Value;
                }

            }
            return strUnit;
        }
        #endregion

    }
}
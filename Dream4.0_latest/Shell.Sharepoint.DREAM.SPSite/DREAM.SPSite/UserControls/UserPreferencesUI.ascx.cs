#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: UserPreferencesUI.ascx.cs
#endregion

/// <summary> 
/// This is the Userpreference ui class, to bind, display and fetch user values
/// </summary> 
using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using Microsoft.SharePoint;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Utilities;
using System.Net;

namespace Shell.SharePoint.DREAM.Site.UI.UserControls
{
    /// <summary>
    /// This is the Userpreference ui class, to bind, display and fetch user values
    /// </summary>

    public partial class UserPreferencesUI :UIHelper
    {
        #region Declaration
        const string DEFAULTPREFERENCESLIST = "Default Preferences";
        const string FIELDLIST = "Field";
        const string ASSETTYPELIST = "Asset Type";
        const string SELECT = "---Select---";
        const string CREATE = "Create";
        const string UPDATE = "Update";
        const string DISPLAY = "Display";
        const string DEPTHUNIT = "DepthUnits";
        const string RECORDSPERPAGE = "RecordsPerPage";
        //Dream 3.0 code
        //Start
        const string PRESSUREUNIT = "PressureUnits";
        const string TEMPERATUREUNIT = "TemperatureUnits";
        //End
        #endregion

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                objReportController = objFactory.GetServiceManager(REPORTSERVICE);
               
                if(!IsPostBack)
                {
                    SetUserPreferences();
                    ReadValuesToControls();
                    SetDefaultPreferences();
                }
            }
            catch(WebException webEx)
            {
                ShowLableMessage(webEx.Message);
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
        }

        /// <summary>
        /// Handles the Click event of the cmdSubmit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmdSubmit_Click(object sender, EventArgs e)
        {
            string strAction = string.Empty;
            try
            {
                string strUserId = objUtility.GetUserName();
                objUserPreferences = ((MOSSServiceManager)objMossController).GetUserPreferences(strUserId);
                if(objUserPreferences == null)
                {
                    objUserPreferences = new UserPreferences();
                    strAction = CREATE;
                }
                else
                {
                    strAction = UPDATE;
                }

                bool blnIsSaved = false;
                objUserPreferences.Display = cboDisplay.SelectedItem.Value;
                objUserPreferences.DepthUnits = cboDepthUnits.SelectedItem.Value;
                objUserPreferences.Country = cboCountry.SelectedItem.Value;
                objUserPreferences.Asset = cboAsset.SelectedItem.Value;
                objUserPreferences.RecordsPerPage = cboRecordsPerPage.SelectedItem.Value;
                objUserPreferences.Basin = cboBasin.SelectedItem.Value;
                //Dream 3.0 code
                //Start
                objUserPreferences.PressureUnits = cboPressureUnits.SelectedItem.Value;
                objUserPreferences.TemperatureUnits = cboTemperatureUnits.SelectedItem.Value;
                //End
                URL objURL = null;
                ArrayList arlURL = new ArrayList();
                objURL = new URL();
                objURL.URLTitle = txtLinkTitle1.Text.Trim();
                objURL.URLValue = txtLinkUrl1.Text.Trim();
                arlURL.Add(objURL);

                objURL = new URL();
                objURL.URLTitle = txtLinkTitle2.Text.Trim();
                objURL.URLValue = txtLinkUrl2.Text.Trim();
                arlURL.Add(objURL);

                objURL = new URL();
                objURL.URLTitle = txtLinkTitle3.Text.Trim();
                objURL.URLValue = txtLinkUrl3.Text.Trim();
                arlURL.Add(objURL);

                objURL = new URL();
                objURL.URLTitle = txtLinkTitle4.Text.Trim();
                objURL.URLValue = txtLinkUrl4.Text.Trim();
                arlURL.Add(objURL);

                objUserPreferences.URL = arlURL;
                blnIsSaved = ((MOSSServiceManager)objMossController).UpdateUserPreferences(strUserId, objUserPreferences, strAction);
                if(blnIsSaved)
                {
                    CommonUtility.SetSessionVariable(Page, enumSessionVariable.UserPreferences.ToString(), objUserPreferences);
                    Page.Response.Redirect("/_layouts/Dream/ConfirmUserPreferences.aspx?saved=1", false);
                }
                else
                {
                    Page.Response.Redirect("/_layouts/Dream/ConfirmUserPreferences.aspx?saved=0", false);
                }
            }
            catch(WebException webEx)
            {
                ShowLableMessage(webEx.Message);
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }

        /// <summary>
        /// Shows the lable message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void ShowLableMessage(string message)
        {
            ExceptionPanel.Visible = true;
            lblException.Visible = true;
            lblException.Text = message;
        }

        /// <summary>
        /// Fill all controls with values.
        /// </summary>
        private void ReadValuesToControls()
        {
            try
            {
                ReadDefaultPrefList();
                ReadCountryList();
                ReadBasinList();
                ReadAssetTypeList();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Read 'Asset type' list and set dropdown assettype.
        /// </summary>
        private void ReadAssetTypeList()
        {
            try
            {
                dtListValues.Reset();
                string strCAMLQuery = "<Where><Eq><FieldRef Name=\"IsActiveReportService\" /><Value Type=\"Boolean\">1</Value></Eq></Where>";
                dtListValues = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, ASSETTYPELIST, strCAMLQuery);
                cboAsset.Items.Clear();
                if(dtListValues != null)
                {
                    foreach(DataRow dtRow in dtListValues.Rows)
                    {
                        cboAsset.Items.Add(dtRow["Title"].ToString());
                    }
                }
                cboAsset.SelectedIndex = 0;
            }
            finally
            {
                if(dtListValues != null)
                    dtListValues.Dispose();
            }
        }

        /// <summary>
        /// Reads the DefaultPreferences List to fill values for dropdowns Display, DepthUnits, Recordsperpage.
        /// </summary>
        private void ReadDefaultPrefList()
        {
            string strDisplay = string.Empty;
            string strDepthUnits = string.Empty;
            string strRecordsPerpage = string.Empty;
            string strDisplayDefault = string.Empty;
            string strDepthUnitsDefault = string.Empty;
            string strRecordsPerpageDefault = string.Empty;
            //Dream 3.0 code
            //start
            string strPressureUnits = string.Empty;
            string strTemperatureUnits = string.Empty;
            string strPressureUnitDefault = string.Empty;
            string strTemperatureUnitDefault = string.Empty;
            string[] arrPressureUnits = new string[3];
            string[] arrTemperatureUnits = new string[2];
            //end
            string[] arrDisplayVals = new string[2];
            string[] arrDepthUnitVals = new string[2];
            string[] arrRecordsPerPageVals = new string[2];
            int index = 0;
            dtListValues = new DataTable();

            try
            {
                //Read 'Default Preferences' list and set the field values
                dtListValues.Reset();
                dtListValues = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, DEFAULTPREFERENCESLIST, string.Empty);
                if(dtListValues != null)
                {
                    foreach(DataRow dtRow in dtListValues.Rows)
                    {
                        if(string.Equals(dtRow["Title"].ToString(), DISPLAY))
                        {
                            strDisplayDefault = dtRow["Default_x0020_Value"].ToString();
                            strDisplay = dtRow["Values"].ToString();
                            arrDisplayVals = strDisplay.Split(',');
                        }
                        else if(string.Equals(dtRow["Title"].ToString(), DEPTHUNIT))
                        {
                            strDepthUnitsDefault = dtRow["Default_x0020_Value"].ToString();
                            strDepthUnits = dtRow["Values"].ToString();
                            arrDepthUnitVals = strDepthUnits.Split(',');
                        }
                        else if(string.Equals(dtRow["Title"].ToString(), RECORDSPERPAGE))
                        {
                            strRecordsPerpageDefault = dtRow["Default_x0020_Value"].ToString();
                            strRecordsPerpage = dtRow["Values"].ToString();
                            arrRecordsPerPageVals = strRecordsPerpage.Split(',');
                        }
                        //Dream 3.0 code
                        //start
                        else if(string.Equals(dtRow["Title"].ToString(), PRESSUREUNIT))
                        {
                            strPressureUnitDefault = dtRow["Default_x0020_Value"].ToString();
                            strPressureUnits = dtRow["Values"].ToString();
                            arrPressureUnits = strPressureUnits.Split(',');
                        }
                        else if(string.Equals(dtRow["Title"].ToString(), TEMPERATUREUNIT))
                        {
                            strTemperatureUnitDefault = dtRow["Default_x0020_Value"].ToString();
                            strTemperatureUnits = dtRow["Values"].ToString();
                            arrTemperatureUnits = strTemperatureUnits.Split(',');
                        }
                        //end
                    }
                    cboDisplay.Items.Clear();
                    for(index = 0; index < arrDisplayVals.Length; index++)
                    {
                        cboDisplay.Items.Add(arrDisplayVals[index]);
                    }
                    cboDisplay.SelectedValue = strDisplayDefault;
                    cboDepthUnits.Items.Clear();
                    for(index = 0; index < arrDepthUnitVals.Length; index++)
                    {
                        cboDepthUnits.Items.Add(arrDepthUnitVals[index]);
                    }
                    cboDepthUnits.SelectedValue = strDepthUnitsDefault;
                    cboRecordsPerPage.Items.Clear();
                    for(index = 0; index < arrRecordsPerPageVals.Length; index++)
                    {
                        cboRecordsPerPage.Items.Add(arrRecordsPerPageVals[index]);
                    }
                    cboRecordsPerPage.SelectedValue = strRecordsPerpageDefault;
                    //Dream 3.0 code
                    //start
                    cboPressureUnits.Items.Clear();
                    for(index = 0; index < arrPressureUnits.Length; index++)
                    {
                        cboPressureUnits.Items.Add(arrPressureUnits[index]);
                    }
                    cboPressureUnits.SelectedValue = strPressureUnitDefault;
                    cboTemperatureUnits.Items.Clear();
                    for(index = 0; index < arrTemperatureUnits.Length; index++)
                    {
                        cboTemperatureUnits.Items.Add(arrTemperatureUnits[index]);
                    }
                    cboTemperatureUnits.SelectedValue = strTemperatureUnitDefault;
                    //end
                }
            }
            finally
            {
                if(dtListValues != null)
                    dtListValues.Dispose();
            }
        }

        /// <summary>
        /// Read 'Country' list and set dropdown country
        /// </summary>
        private void ReadCountryList()
        {
            try
            {
                int intCounter = 1;
                string strCamlQuery = "<OrderBy><FieldRef Name=\"Title\"/></OrderBy><Where><Eq>" + "<FieldRef Name=\"Active\" /><Value Type=\"Choice\">Yes</Value></Eq></Where>";
                dtListValues.Reset();
                dtListValues = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, COUNTRYLIST, strCamlQuery);
                cboCountry.Items.Clear();
                cboCountry.Items.Add(SELECT);
                cboCountry.Items[0].Value = SELECT;

                if(dtListValues != null)
                {
                    foreach(DataRow dtRow in dtListValues.Rows)
                    {
                        cboCountry.Items.Add(dtRow["Title"].ToString());
                        cboCountry.Items[intCounter].Value = dtRow["Country_x0020_Code"].ToString();
                        intCounter++;
                    }
                }
                cboCountry.SelectedIndex = 0;
            }
            finally
            {
                if(dtListValues != null)
                    dtListValues.Dispose();
            }
        }

        /// <summary>
        /// Read 'Basin' list and set dropdown basin
        /// </summary>
        private void ReadBasinList()
        {
            //**R5k changes for Dream 4.0
            //Starts
            PopulateListControl(cboBasin, GetBasinFromWebService(), BASINXPATH, objUserPreferences.Basin);
            cboBasin.Items.Insert(0, SELECT);
            //Ends
        }

        /// <summary>
        /// Read UserPreferences to set users saved default value
        /// </summary>
        /// 
        private void SetDefaultPreferences()
        {
            if(objUserPreferences != null)
            {
                cboDisplay.SelectedValue = objUserPreferences.Display;
                cboDepthUnits.SelectedValue = objUserPreferences.DepthUnits;
                cboCountry.SelectedValue = objUserPreferences.Country;
                cboAsset.SelectedValue = objUserPreferences.Asset;
                cboRecordsPerPage.SelectedValue = objUserPreferences.RecordsPerPage;
                cboBasin.SelectedValue = objUserPreferences.Basin;
                //Dream 3.0 Code
                //Start
                cboPressureUnits.SelectedValue = objUserPreferences.PressureUnits;
                cboTemperatureUnits.SelectedValue = objUserPreferences.TemperatureUnits;
                //End

                //Read user defined links
                URL objURL = null;
                ArrayList arlUserDefLinks = new ArrayList();
                arlUserDefLinks = objUserPreferences.URL;
                for(int index = 0; index < arlUserDefLinks.Count; index++)
                {
                    objURL = new URL();
                    objURL = (URL)arlUserDefLinks[index];
                    switch(index)
                    {
                        case 0:
                            txtLinkTitle1.Text = objURL.URLTitle;
                            txtLinkUrl1.Text = objURL.URLValue;
                            break;
                        case 1:
                            txtLinkTitle2.Text = objURL.URLTitle;
                            txtLinkUrl2.Text = objURL.URLValue;
                            break;
                        case 2:
                            txtLinkTitle3.Text = objURL.URLTitle;
                            txtLinkUrl3.Text = objURL.URLValue;
                            break;
                        case 3:
                            txtLinkTitle4.Text = objURL.URLTitle;
                            txtLinkUrl4.Text = objURL.URLValue;
                            break;
                    }
                }
            }
        }
    }
}
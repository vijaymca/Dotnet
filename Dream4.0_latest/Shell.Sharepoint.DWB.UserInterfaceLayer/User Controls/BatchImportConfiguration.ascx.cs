using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Shell.SharePoint.DWB.BusinessLogicLayer;
using System.Net;
using Shell.SharePoint.DWB.Business.DataObjects;
using System.Xml;
using Shell.SharePoint.DREAM.Utilities;
using System.Drawing;
using System.Collections.Generic;

namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// This class is used for Batch Import Configuration
    /// </summary>
    public partial class BatchImportConfiguration : UIHelper
    {
        #region DECLARATION
        CommonBLL objCommonBLL;
        WellBookBLL objWellBookBLL;
        BatchImports objBatchImports;
        DWB.Business.DataObjects.BatchImport objBatchImport;
        PageName objPageName;
        SharedPath objSharedPath;
        FileType objFileType;
        FileFormat objFileFormat;
        ArrayList arlBatchImport;
        ArrayList arlPageName;
        const string DWBFILETYPE = "DWB File Types";
        const string DWBDOCUMENTNAMINGCONVENTION = "DWB Document Naming Convention";
        const string PAGENAMESCOLUMN = "Page Name";
        const string SHAREDAREAPATHCOLUMN = "Shared Area Path";
        const string FILETYPECOLUMN = "File Type";
        const string DOCUMENTNAMINGCONVCOLUMN = "Document Naming Convention";
        const string OVERFLOW = "overflow:auto;";
        const string EVENROWSTYLECSS = "evenRowStyle";
        const string ODDROWSTYLECSS = "oddRowStyle";
        const string WELLBOOKSUMMARYCSS = "DWBWellBookSummaryGridViewCSS";
        const string FIXEDHEADERCSS = "ResultFixedHeader";
        const string BATCHIMPORTPATHSESSION = "BatchImportPath";
        const string SAVEDSUCCESSFULLYMESSAGE = "alert('All configurations has been updated successfully.')";

        string strSharedAreaPath;
        string strBookID;
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            btnReset.OnClientClick = "javascript:return ConfirmReset()";

            if (HttpContext.Current.Request.QueryString["bookId"] != null)
            {
                if (!(string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["bookId"])))
                    strBookID = HttpContext.Current.Request.QueryString["bookId"];
                if (HttpContext.Current.Session[BATCHIMPORTPATHSESSION] != null)
                    strSharedAreaPath = HttpContext.Current.Session[BATCHIMPORTPATHSESSION].ToString();
                PopulateGrid();
            }
        }
        #endregion

        #region EventHandler Methods

        /// <summary>
        /// Saves the changes into BatchImportXML and closes the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveOrUpdateBatchImportXML();
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "SavedSuccessfully", SAVEDSUCCESSFULLYMESSAGE, true);
                HttpContext.Current.Session.Remove(BATCHIMPORTPATHSESSION);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CloseWindow", "<Script language='javascript'>CloseWithoutPrompt();</Script>");
            }
            catch (WebException webEx)
            {
                lblException.Text = webEx.Message;
                lblException.Visible = true;
                ExceptionBlock.Visible = true;
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strParentSiteURL, ex);
            }
        }

        /// <summary>
        /// Saves the changes to BatchImportXML and redirects to BatchImportConfirmation Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveAndContinue_Click(object sender, EventArgs e)
        {
            try
            {
                SaveOrUpdateBatchImportXML();
                string OPENBATCHIMPORTCONFIRMATION = "openBatchImportConfirmation('" + strBookID + "');";
                HttpContext.Current.Session.Remove(BATCHIMPORTPATHSESSION);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "BatchImportConfirmation", "<Script language='javascript'>openBatchImportConfirmation('" + strBookID + "');</Script>");
            }
            catch (WebException webEx)
            {
                lblException.Text = webEx.Message;
                lblException.Visible = true;
                ExceptionBlock.Visible = true;
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strParentSiteURL, ex);
            }

        }

        /// <summary>
        /// Resets the values to previously configured.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReset_Click(object sender, EventArgs e)
        {
            if (hdnResetStatus.Value == "true")
                PopulateGrid();
        }

        /// <summary>
        /// Closes the page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {            
            HttpContext.Current.Session.Remove(BATCHIMPORTPATHSESSION);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "BatchImportConfirmation", "<Script language='javascript'>CloseWithoutPrompt();</Script>");
        }

        /// <summary>
        /// Reset the Shared Area Path to the value configured in Batch Import Screen
        /// added by praveena for module Reset
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnResetToDefaultPath_Click(object sender, EventArgs e)
        {
            if (grdBatchImportConfiguration.Rows.Count != 0)
            {
                foreach (GridViewRow gdvRow in grdBatchImportConfiguration.Rows)
                {
                    TextBox txtSharedAreaPath = (TextBox)gdvRow.FindControl("txtSharedAreaPath");
                    txtSharedAreaPath.Text = strSharedAreaPath;
                }
            }
        }

        /// <summary>
        /// Event handler triggered for each row bounded in the grid view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdBatchImportConfiguration_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[0].Text = PAGENAMESCOLUMN;
                    e.Row.Cells[1].Text = SHAREDAREAPATHCOLUMN;
                    e.Row.Cells[2].Text = FILETYPECOLUMN;
                    e.Row.Cells[3].Text = DOCUMENTNAMINGCONVCOLUMN;
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    TextBox txtSharedAreaPath = (TextBox)e.Row.FindControl("txtSharedAreaPath");
                    txtSharedAreaPath.Text = strSharedAreaPath;
                    DropDownList cboFileType = (DropDownList)e.Row.FindControl("cboFileType");
                    BindDropDown(cboFileType);
                    DropDownList cboNamingConvention = (DropDownList)e.Row.FindControl("cboNamingConvention");
                    BindDropDown(cboNamingConvention);

                }
            }
            catch (WebException webEx)
            {
                lblException.Text = webEx.Message;
                lblException.Visible = true;
                ExceptionBlock.Visible = true;
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strParentSiteURL, ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method binds the values to DropDownLists
        /// </summary>
        /// <param name="cboList">DropDownListID</param>
        private void BindDropDown(DropDownList cboList)
        {
            DataTable dtListData = null;
            try
            {
                if (cboList.ID == "cboFileType")
                {
                    cboList.Items.Clear();
                    string strCamlQuery = @" <Where><IsNotNull><FieldRef Name='File_Type' /></IsNotNull></Where>";
                    string strViewFields = string.Empty;
                    strViewFields = @"<FieldRef Name='ID' /><FieldRef Name='File_Type' />";
                    objCommonBLL = new CommonBLL();
                    dtListData = objCommonBLL.ReadList(strParentSiteURL, DWBFILETYPE, strCamlQuery, strViewFields);
                    if (dtListData != null && dtListData.Rows.Count > 0)
                    {
                        cboList.DataSource = dtListData;
                        cboList.DataValueField = "ID";
                        cboList.DataTextField = "File_Type";
                        cboList.DataBind();
                    }
                }
                else
                {
                    cboList.Items.Clear();
                    string strCamlQuery = @" <Where><IsNotNull><FieldRef Name='Title' /></IsNotNull></Where>";
                    string strViewFields = string.Empty;
                    strViewFields = @"<FieldRef Name='Actual_Naming_Convention' /><FieldRef Name='Title' />";
                    objCommonBLL = new CommonBLL();
                    dtListData = objCommonBLL.ReadList(strParentSiteURL, DWBDOCUMENTNAMINGCONVENTION, strCamlQuery, strViewFields);
                    if (dtListData != null && dtListData.Rows.Count > 0)
                    {
                        cboList.DataSource = dtListData;
                        cboList.DataValueField = "Actual_Naming_Convention";
                        cboList.DataTextField = "Title";
                        cboList.DataBind();
                    }
                }
            }

            catch (WebException webEx)
            {
                lblException.Text = webEx.Message;
                lblException.Visible = true;
                ExceptionBlock.Visible = true;
            }
            finally
            {
                if (dtListData != null)
                    dtListData.Dispose();
            }
        }

        /// <summary>
        /// This method binds the values to grid on Page load
        /// </summary>
        private void PopulateGrid()
        {
            grdBatchImportConfiguration.CssClass = WELLBOOKSUMMARYCSS;
            grdBatchImportConfiguration.EnableViewState = true;
            grdBatchImportConfiguration.HeaderStyle.CssClass = FIXEDHEADERCSS;
            grdBatchImportConfiguration.HeaderStyle.Height = new Unit(20, UnitType.Pixel);
            grdBatchImportConfiguration.RowStyle.CssClass = EVENROWSTYLECSS;
            grdBatchImportConfiguration.AlternatingRowStyle.CssClass = ODDROWSTYLECSS;
            XmlDocument batchImportDoc = null;
            BatchImportBLL ObjBatchImportBLL = new BatchImportBLL();
            try
            {
                batchImportDoc = ObjBatchImportBLL.GetOnlyBatchImportXML(strBookID);
                //loading grid from values configured in XML
                if (batchImportDoc != null)
                {
                    objWellBookBLL = new WellBookBLL();
                    DataTable dtResult = objWellBookBLL.GetTypeIIIPagesForBook(strParentSiteURL, strBookID, "No");
                    DataView dvResultTable;
                    dvResultTable = dtResult.DefaultView;
                    dtResult = dvResultTable.ToTable(true, "Page_Name");
                    grdBatchImportConfiguration.DataSource = dtResult;
                    grdBatchImportConfiguration.DataBind();
                    foreach (GridViewRow gdvRow in grdBatchImportConfiguration.Rows)
                    {
                        XmlNode node = batchImportDoc.SelectSingleNode("/batchImport[@BookID='" + strBookID + "']/pageName[@name='" + HttpUtility.HtmlDecode(gdvRow.Cells[0].Text) + "']");
                        if (node != null)
                        {
                            TextBox txtSharedAreaPath = (TextBox)gdvRow.FindControl("txtSharedAreaPath");
                            txtSharedAreaPath.Text = node.SelectSingleNode("sharedPath").Attributes["path"].Value;
                            DropDownList cboFileType = (DropDownList)gdvRow.FindControl("cboFileType");
                            foreach (ListItem eachItem in cboFileType.Items)
                            {
                                if (eachItem.Text == node.SelectSingleNode("fileType").Attributes["type"].Value)
                                {
                                    eachItem.Selected = true;
                                }
                            }
                            DropDownList cboNamingConvention = (DropDownList)gdvRow.FindControl("cboNamingConvention");
                            foreach (ListItem eachItem in cboNamingConvention.Items)
                            {
                                if (eachItem.Text == node.SelectSingleNode("fileFormat").Attributes["format"].Value)
                                {
                                    eachItem.Selected = true;
                                }
                            }
                        }
                        else
                        {
                            gdvRow.Cells[0].ForeColor = Color.Red;
                        }
                    }
                }
                //loading grid with default values
                else
                {
                    objWellBookBLL = new WellBookBLL();
                    DataTable dtResult = objWellBookBLL.GetTypeIIIPagesForBook(strParentSiteURL, strBookID, "No");
                    DataView dvResultTable;
                    dvResultTable = dtResult.DefaultView;
                    dtResult = dvResultTable.ToTable(true, "Page_Name");
                    grdBatchImportConfiguration.DataSource = dtResult;
                    grdBatchImportConfiguration.DataBind();
                }
            }
            catch (WebException webEx)
            {
                lblException.Text = webEx.Message;
                lblException.Visible = true;
                ExceptionBlock.Visible = true;
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strParentSiteURL, ex);
            }
        }

        /// <summary>
        /// To get logged in User Name
        /// </summary>
        /// <returns></returns>
        private string GetUserName()
        {
            Shell.SharePoint.DREAM.Utilities.CommonUtility objCommonUtility = new Shell.SharePoint.DREAM.Utilities.CommonUtility();
            string strUserName = objCommonUtility.GetUserName();
            return strUserName;
        }

        /// <summary>
        /// This method Saves or Upadates the values into BatchImportXML
        /// </summary>
        private void SaveOrUpdateBatchImportXML()
        {
            objBatchImport = new Shell.SharePoint.DWB.Business.DataObjects.BatchImport();
            objBatchImport.BookID = strBookID;
            objBatchImport.DefaultSharedPath = strSharedAreaPath;
            objBatchImport.UserName = GetUserName();
            string strRowFilter;
            arlPageName = new ArrayList();
            foreach (GridViewRow gdvRow in grdBatchImportConfiguration.Rows)
            {
                objPageName = new PageName();
                objPageName.Name = HttpUtility.HtmlDecode(gdvRow.Cells[0].Text);
                DataTable dtResult = objWellBookBLL.GetTypeIIIPagesForBook(strParentSiteURL, strBookID, "No");
                DataView dvResultTable;
                dvResultTable = dtResult.DefaultView;
                strRowFilter = "Page_Name = '" + objPageName.Name + "'";
                dvResultTable.RowFilter = strRowFilter;
                dtResult = dvResultTable.ToTable();
                objPageName.PageCount = dtResult.Rows.Count.ToString();
                TextBox txtSharedAreaPath = (TextBox)gdvRow.FindControl("txtSharedAreaPath");
                objSharedPath = new SharedPath();
                objSharedPath.Path = txtSharedAreaPath.Text;
                objPageName.SharedPath = objSharedPath;
                DropDownList cboFileType = (DropDownList)gdvRow.FindControl("cboFileType");
                objFileType = new FileType();
                objFileType.Type = cboFileType.SelectedItem.Text;
                objPageName.FileType = objFileType;
                DropDownList cboNamingConvention = (DropDownList)gdvRow.FindControl("cboNamingConvention");
                objFileFormat = new FileFormat();
                objFileFormat.Format = cboNamingConvention.SelectedItem.Text;
                objFileFormat.ActualFormat = cboNamingConvention.SelectedItem.Value;
                objPageName.FileFormat = objFileFormat;
                arlPageName.Add(objPageName);
            }
            objBatchImport.PageName = arlPageName;
            objBatchImports = new BatchImports();
            arlBatchImport = new ArrayList();
            arlBatchImport.Add(objBatchImport);
            objBatchImports.BatchImport = arlBatchImport;
            BatchImportXmlGeneratotorBLL objBatchImportXmlGeneratotorBLL;
            objBatchImportXmlGeneratotorBLL = new BatchImportXmlGeneratotorBLL();
            XmlDocument xmlDoc = objBatchImportXmlGeneratotorBLL.CreateBatchImportXML(objBatchImports);
            //GetXMLFileData(xmlDoc, strBookID);
            BatchImportBLL objBatchImportBLL;
            objBatchImportBLL = new BatchImportBLL();
            objBatchImportBLL.UploadToDocumentLib(strBookID, xmlDoc);
        }

        #endregion

    }
}








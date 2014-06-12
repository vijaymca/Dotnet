#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: QueryBuilder.ascx.cs 
#endregion

/// <summary> 
/// This is Query Builder class
/// </summary> 
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Services.Protocols;
using System.Net;
using System.Xml;
using Microsoft.SharePoint;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DREAM.Business.Entities;
using Telerik.Web.UI;


namespace Shell.SharePoint.DREAM.Site.UI.UserControls
{
    /// <summary>
    /// Handles the Query Builder Module 
    /// </summary>
    public partial class QueryBuilder :UIControlHandler
    {
        #region Declaration

        static ArrayList arlOperators;
        const string DATAPROVIDERLIST = "Query Search Data Provider";
        const string DATASOURCELIST = "Query Search Data Source";
        const string DBTABLELIST = "Query Search Table";
        const string OPERATORLIST = "Query Search Operators";
        const string DBTABLECOLUMNLIST = "Query Search Table Columns";
        const string QUERYBUILDERURL = "/pages/QuerySearch.aspx";
        const string SELECTSTRING = "SELECT ";
        const string SIMPLE = "Simple";
        const string ALLCOLUMNS = "*";
        const string COLUMNNAMES = "Column Names";
        const string ALLCOLUMNSKEYWORD = "allcolumns";
        const string FROMSTRING = " FROM ";
        const string WHERECLAUSE = " WHERE ";
        const string COLUMNSXPATH = "Columns/columnName";
        const string SQLDEFAULTTEXT = "<<View SQL Query over here>>";
        const string SQL = "sql";
        const string GEN = "general";
        const string MODIFY = "modify";
        AbstractController objQueryServiceController;
        XmlDocument xmlDocSearchResult = new XmlDocument();
        bool blnIsAdmin;
        string type = string.Empty;
        string strUserID = string.Empty;
        #endregion

        /// <summary>
        /// Loads the Tree View with Data Source and Data Provider Information. Also Loads the search details when redirected from 
        /// Standard/Manage Search Links
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dtListData = null;
            ServiceProvider objFactory = new ServiceProvider();
            try
            {
                btnSearch.Attributes.Add(REDIRECTATTRIBUTE, REDIRECTATTRIBUTEVALUE);
                RunButton.Attributes.Add(REDIRECTATTRIBUTE, REDIRECTATTRIBUTEVALUE);

                //attributes is added to resize window on load
                querySearchTree.Attributes.Add(RESIZEWINDOWATTRIBUTE, REDIRECTATTRIBUTEVALUE);
                cboSavedSearch.Attributes.Add(RESIZEWINDOWATTRIBUTE, REDIRECTATTRIBUTEVALUE);
                ClearButton.Attributes.Add(RESIZEWINDOWATTRIBUTE, REDIRECTATTRIBUTEVALUE);
                btnReset.Attributes.Add(RESIZEWINDOWATTRIBUTE, REDIRECTATTRIBUTEVALUE);
                btnSaveSearch.Attributes.Add(RESIZEWINDOWATTRIBUTE, REDIRECTATTRIBUTEVALUE);
                saveButton.Attributes.Add(RESIZEWINDOWATTRIBUTE, REDIRECTATTRIBUTEVALUE);
                viewButton.Attributes.Add(RESIZEWINDOWATTRIBUTE, REDIRECTATTRIBUTEVALUE);
                editButton.Attributes.Add(RESIZEWINDOWATTRIBUTE, REDIRECTATTRIBUTEVALUE);
                //**
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                objReportController = objFactory.GetServiceManager(REPORTSERVICE);
                objQueryServiceController = objFactory.GetServiceManager(QUERYSERVICE);

                ExceptionPanel.Visible = false;
                lblException.Text = string.Empty;

                strUserID = HttpContext.Current.User.Identity.Name.ToString();
                blnIsAdmin = ((MOSSServiceManager)objMossController).IsAdmin(strCurrSiteUrl, strUserID);
                if(!txtSQLQuery.ReadOnly)
                {
                    type = SQL;
                }
                else
                {
                    type = GEN;
                }

                if(!IsPostBack)
                {
                    LoadTreeViewFromDataSet();
                    cboSavedSearch.Items.Clear();

                    cboSavedSearch.Items.Add(DEFAULTDROPDOWNTEXT);
                    ((MOSSServiceManager)objMossController).LoadSaveSearch(QUERYSEARCH, cboSavedSearch);

                    if(Page.Request.QueryString["savesearchname"] != null)
                    {
                        BindUIControls(Page.Request.QueryString["savesearchname"].ToString());
                        lblException.Visible = false;
                    }
                }

            }
            catch(WebException webEx)
            {
                CommonUtility.HandleException(strCurrSiteUrl, webEx, 1);
                ShowLableMessage(webEx.Message);
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
            finally
            {
                if(dtListData != null)
                    dtListData.Dispose();
            }
        }

        /// <summary>
        /// Gets the operator from list.
        /// </summary>
        private void GetOperatorFromList()
        {
            DataTable dtListOperators = null;

            try
            {
                DataRow dtOperatorsRow;
                arlOperators = new ArrayList();
                if(querySearchTree.SelectedNode != null)
                {
                    string strCamlQuery = "<Where><Eq><FieldRef Name='Data_x0020_Source' /><Value Type='LookupMulti'>" + querySearchTree.SelectedNode.ParentNode.ParentNode.Text.ToString() + "</Value></Eq></Where>";
                    string strListName = OPERATORLIST;

                    objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                    dtListOperators = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, strListName, strCamlQuery);
                    if(dtListOperators != null)
                    {
                        for(int intRowIndex = 0; intRowIndex < dtListOperators.Rows.Count; intRowIndex++)
                        {
                            dtOperatorsRow = dtListOperators.Rows[intRowIndex];
                            arlOperators.Add(dtOperatorsRow["Title"].ToString());
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if(dtListOperators != null)
                    dtListOperators.Dispose();
            }
        }

        /// <summary>
        /// Enables the criteria.
        /// </summary>
        private void EnableCriteria()
        {
            if(tblColumnNames.Enabled)
            {
                for(int intIndex = 0; intIndex < tblColumnNames.Rows.Count; intIndex++)
                {
                    TextBox txtCriteria = (TextBox)tblColumnNames.Rows[intIndex].Cells[3].FindControl("txtCriteria");
                    DropDownList cboOperator = (DropDownList)tblColumnNames.Rows[intIndex].Cells[2].FindControl("cboOperator");
                    if(cboOperator.SelectedIndex > 0)
                    {
                        txtCriteria.Enabled = true;
                        cboOperator.Enabled = true;
                    }
                }
            }
            if(!txtSQLQuery.ReadOnly)
            {
                editButton.ToolTip = EDITCRITERIA;
                type = SQL;
            }
            else
            {
                editButton.ToolTip = EDITSQL;
                type = GEN;
            }
        }

        /// <summary>
        /// Loads the tree view from data set.
        /// </summary>
        private void LoadTreeViewFromDataSet()
        {
            MemoryStream objMemoryStream = null;
            DataSet objDataSet = new DataSet();
            XmlDocument xmlTreeView = null;

            try
            {
                xmlTreeView = ((MOSSServiceManager)objMossController).GetTreeViewXML(strCurrSiteUrl);

                if(xmlTreeView != null)
                {

                    objMemoryStream = new MemoryStream();
                    xmlTreeView.Save(objMemoryStream);
                    objMemoryStream.Position = 0;
                    objDataSet.ReadXml(objMemoryStream);

                    // You can use this:
                    XmlDataSource objXmlDataSource = new XmlDataSource();
                    objXmlDataSource.EnableCaching = false;
                    objXmlDataSource.Data = objDataSet.GetXml();
                    //codeChange
                    //querySearchTree.PopulateNodesFromClient = true;
                    // Create the root node binding.

                    RadTreeNodeBinding RootBinding = new RadTreeNodeBinding();
                    RootBinding.DataMember = DATASOURCE;
                    RootBinding.TextField = "#name";
                    RootBinding.Expanded = true;
                    //RootBinding.SelectAction = TreeNodeSelectAction.Expand;

                    RadTreeNodeBinding RootBinding2 = new RadTreeNodeBinding();
                    RootBinding2.DataMember = DATASOURCENAME;
                    RootBinding2.TextField = ATTRIBNAME;
                    RootBinding2.Expanded = true;
                    //RootBinding2.SelectAction = TreeNodeSelectAction.Expand;

                    // Create the parent node binding.
                    RadTreeNodeBinding ParentBinding = new RadTreeNodeBinding();
                    ParentBinding.DataMember = DATAPROVIDER;
                    ParentBinding.TextField = ATTRIBNAME;
                    //ParentBinding.SelectAction = TreeNodeSelectAction.Expand;
                    //ParentBinding.Expanded = true;

                    // Create the leaf node binding.
                    RadTreeNodeBinding LeafBinding = new RadTreeNodeBinding();
                    LeafBinding.DataMember = TABLENAME;
                    LeafBinding.TextField = ATTRIBNAME;
                    LeafBinding.ValueField = "#InnerText";
                    //LeafBinding.SelectAction = TreeNodeSelectAction.Select;
                    LeafBinding.NavigateUrl = string.Empty;


                    querySearchTree.DataBindings.Add(RootBinding);
                    querySearchTree.DataBindings.Add(RootBinding2);
                    querySearchTree.DataBindings.Add(ParentBinding);
                    querySearchTree.DataBindings.Add(LeafBinding);

                    querySearchTree.DataSource = objXmlDataSource;

                    querySearchTree.DataBind();
                }
                else
                {
                    ShowLableMessage(((MOSSServiceManager)objMossController).GetCustomExceptionMessage(strCurrSiteUrl, "17").ToString());
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if(objMemoryStream != null)
                    objMemoryStream.Dispose();
                if(objDataSet != null)
                    objDataSet.Dispose();
            }
        }

        /// <summary>
        /// Handles the Click event of the edit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.ImageClickEventArgs"/> instance containing the event data.</param>
        protected void Edit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if(tblColumnNames.Enabled)
                {
                    ViewButton_Click(sender, e);
                    tblColumnNames.Enabled = false;
                    txtSQLQuery.ReadOnly = false;
                    if(!string.Equals(btnSaveSearch.Text.ToString(), MODIFYSRCH) && (
                        !string.Equals(btnSaveSearch.Text.ToString(), MODIFYSQL)))
                    {
                        btnSaveSearch.Text = SAVESQL;
                    }
                }
                else
                {
                    txtSQLQuery.Text = SQLDEFAULTTEXT;
                    tblColumnNames.Enabled = true;
                    txtSQLQuery.ReadOnly = true;
                    if(!string.Equals(btnSaveSearch.Text.ToString(), MODIFYSRCH) && (
                        !string.Equals(btnSaveSearch.Text.ToString(), MODIFYSQL)))
                    {
                        btnSaveSearch.Text = SAVESRCH;
                    }
                }
                EnableCriteria();
            }
            catch(WebException webEx)
            {
                CommonUtility.HandleException(strCurrSiteUrl, webEx, 1);
                ShowLableMessage(webEx.Message);
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }



        /// <summary>
        /// Handles the Click event of the btnReset control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void BtnReset_Click(object sender, EventArgs e)
        {
            if(string.Equals(btnSaveSearch.Text.ToString(), MODIFYSRCH) || (string.Equals(btnSaveSearch.Text.ToString(), MODIFYSQL)))
            {
                if(Page.Request.QueryString["savesearchname"] != null)
                {
                    BindUIControls(Page.Request.QueryString["savesearchname"]);
                    lblException.Visible = false;
                }
                else
                {
                    QuerySearchTree_SelectedNodeChanged(null, EventArgs.Empty);
                }
            }
            else
            {
                QuerySearchTree_SelectedNodeChanged(null, EventArgs.Empty);
            }
        }
        /// <summary>
        /// Handles the Click event of the viewButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.ImageClickEventArgs"/> instance containing the event data.</param>
        protected void ViewButton_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string strSQLQuery = string.Empty;
                string strColumnName = string.Empty;

                DropDownList cboOperator = null;
                TextBox txtCriteria = null;
                CheckBox chbColumn = null;
                CheckBox chbSelectAll = chbHeaderColumn;

                QueryBuilderHelper objQueryBuilder = new QueryBuilderHelper();
                #region sql query builder
                objQueryBuilder.TableName = querySearchTree.SelectedNode.Value;

                if(chbSelectAll.Checked)
                {
                    objQueryBuilder.ColumnNames.Add(ALLCOLUMNS);
                }
                else
                {

                    for(int intIndex = 0; intIndex < tblColumnNames.Rows.Count; intIndex++)
                    {
                        chbColumn = (CheckBox)(tblColumnNames.Rows[intIndex].Cells[0].FindControl("chbColumns"));
                        if(chbColumn != null)
                        {
                            if(chbColumn.Checked)
                            {
                                objQueryBuilder.ColumnNames.Add(tblColumnNames.Rows[intIndex].Cells[1].Text.ToString());
                            }
                        }
                    }
                }

                for(int intIndex = 0; intIndex < tblColumnNames.Rows.Count; intIndex++)
                {
                    strColumnName = tblColumnNames.Rows[intIndex].Cells[1].Text;
                    cboOperator = (DropDownList)tblColumnNames.Rows[intIndex].Cells[2].FindControl("cboOperator");
                    txtCriteria = (TextBox)tblColumnNames.Rows[intIndex].Cells[3].FindControl("txtCriteria");

                    if(txtCriteria.Text.ToString().Length > 0 && cboOperator.SelectedIndex != 0)
                    {
                        string strCriteria = txtCriteria.Text;
                        strCriteria = strCriteria.Replace("*", "%");
                        strCriteria = strCriteria.Replace("?", "_");
                        strCriteria = strCriteria.Replace("'", "''");

                        objQueryBuilder.AddSubClause(strColumnName, cboOperator.SelectedItem.Text, strCriteria);
                    }
                }

                strSQLQuery = objQueryBuilder.BuildQuery();
                #endregion

                txtSQLQuery.Text = strSQLQuery;
                tblColumnNames.Enabled = true;
                txtSQLQuery.ReadOnly = true;

                if(!string.Equals(btnSaveSearch.Text.ToString(), MODIFYSRCH) && (
                       !string.Equals(btnSaveSearch.Text.ToString(), MODIFYSQL)))
                {
                    btnSaveSearch.Text = SAVESRCH;
                }

                EnableCriteria();
            }
            catch(WebException webEx)
            {
                CommonUtility.HandleException(strCurrSiteUrl, webEx, 1);
                ShowLableMessage(webEx.Message);
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            tblColumnNames.RowCreated += new GridViewRowEventHandler(TblColumnNames_RowCreated);
        }

        /// <summary>
        /// Loads the dropdownlist with the operator values selected from SharePoint List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TblColumnNames_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if(querySearchTree.SelectedNode != null)
                {
                    GetOperatorFromList();
                }
                if(e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList cboOperator = (DropDownList)e.Row.FindControl("cboOperator");
                    foreach(string strOperator in arlOperators)
                    {
                        cboOperator.Items.Add(strOperator);
                    }
                }
            }
            catch(WebException webEx)
            {
                CommonUtility.HandleException(strCurrSiteUrl, webEx, 1);
                ShowLableMessage(webEx.Message);
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }

        /// <summary>
        /// Creates the columns table.
        /// </summary>
        /// <param name="columnsListDocument">The columns list document.</param>
        private void CreateColumnsTable(XmlDocument columnsListDocument)
        {
            MemoryStream objMemoryStream = null;
            DataSet dataSet = new DataSet();

            try
            {
                objMemoryStream = new MemoryStream();
                columnsListDocument.Save(objMemoryStream);
                objMemoryStream.Position = 0;
                dataSet.ReadXml(objMemoryStream);

                tblColumnNames.DataSource = dataSet.Tables[1];
                tblColumnNames.DataBind();
                chbHeaderColumn.Checked = true;
                EnableCriteria();
            }
            catch
            {
                throw;
            }
            finally
            {
                if(objMemoryStream != null)
                    objMemoryStream.Dispose();
                if(dataSet != null)
                    dataSet.Dispose();
            }
        }

        /// <summary>
        /// Handles the Click event of the saveButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.ImageClickEventArgs"/> instance containing the event data.</param>
        protected void SaveButton_Click(object sender, ImageClickEventArgs e)
        {
            BtnSaveSearch_Click(null, EventArgs.Empty);
        }

        /// <summary>
        /// Handles the Click event of the ClearButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.ImageClickEventArgs"/> instance containing the event data.</param>
        protected void ClearButton_Click(object sender, ImageClickEventArgs e)
        {
            if(string.Equals(btnSaveSearch.Text.ToString(), MODIFYSRCH) || (string.Equals(btnSaveSearch.Text.ToString(), MODIFYSQL)))
            {
                if(Page.Request.QueryString["savesearchname"] != null)
                {
                    BindUIControls(Page.Request.QueryString["savesearchname"]);
                    lblException.Visible = false;
                }
                else
                {
                    QuerySearchTree_SelectedNodeChanged(null, EventArgs.Empty);
                }
            }
            else
            {
                QuerySearchTree_SelectedNodeChanged(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Handles the Click event of the RunButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.ImageClickEventArgs"/> instance containing the event data.</param>
        protected void RunButton_Click(object sender, ImageClickEventArgs e)
        {
            BtnSearch_Click(null, EventArgs.Empty);
        }

        /// <summary>
        /// Handles the SelectedNodeChanged event of the querySearchTree control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void QuerySearchTree_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                objRequestInfo = SetDataObject(true);
                xmlDocSearchResult = objQueryServiceController.GetSearchResults(objRequestInfo, intMaxRecords, string.Empty, null, 0);
                CreateColumnsTable(xmlDocSearchResult);
                columnSearchPanel.Visible = true;
                tblColumnNames.Enabled = true;
                txtSQLQuery.Text = SQLDEFAULTTEXT;
                txtSQLQuery.ReadOnly = true;
                chbShared.Checked = false;
                cboSavedSearch.SelectedIndex = 0;
                LoadControlPanel();
            }
            catch(SoapException soapEx)
            {
                if(!string.Equals(soapEx.Message.ToString(), SOAPEXCEPTIONMESSAGE))
                {
                    CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                }
                ShowLableMessage(soapEx.Message);
            }
            catch(WebException webEx)
            {
                CommonUtility.HandleException(strCurrSiteUrl, webEx, 1);
                ShowLableMessage(webEx.Message);
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }

        /// <summary>
        /// Hides or Loads the Edit Button
        /// </summary>
        private void LoadControlPanel()
        {
            if(blnIsAdmin)
            {
                editButton.Visible = true;
                chbShared.Visible = true;

            }
            else
            {
                editButton.Visible = false;
                chbShared.Visible = false;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSaveSearch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void BtnSaveSearch_Click(object sender, EventArgs e)
        {
            bool blnIsNameExist = false;
            UISaveSearchHandler objUISaveSearchHandler = null;
            XmlDocument xmlDocSearchRequest = null;
            ArrayList arlSavedSearch = null;
            string strSaveSearchName = txtSaveSearchName.Text.Trim();

            try
            {
                if(string.Equals(btnSaveSearch.Text.ToString(), MODIFYSRCH) || (string.Equals(btnSaveSearch.Text.ToString(), MODIFYSQL)))
                {
                    lblException.Visible = false;

                    xmlDocSearchRequest = CreateSaveSearchXML();

                    try
                    {
                        objUISaveSearchHandler = new UISaveSearchHandler();
                        objUISaveSearchHandler.ModifySaveSearchXML(QUERYSEARCH, strSaveSearchName, chbShared.Checked, cboSavedSearch, xmlDocSearchRequest);
                    }
                    catch(WebException webEx)
                    {
                        CommonUtility.HandleException(strCurrSiteUrl, webEx, 1);
                        ShowLableMessage(webEx.Message);
                    }
                    catch(Exception ex)
                    {
                        CommonUtility.HandleException(strCurrSiteUrl, ex);
                    }


                    cboSavedSearch.Text = txtSaveSearchName.Text.ToString().Trim();
                    txtSaveSearchName.Enabled = true;
                    //added in dream 3.0
                    txtSaveSearchName.Text = string.Empty;
                    btnSaveSearch.Text = SAVESRCH;
                }
                else
                {
                    lblException.Visible = false;
                    arlSavedSearch = new ArrayList();

                    //Check for Duplicate Name Exist
                    arlSavedSearch = ((MOSSServiceManager)objMossController).GetSaveSearchName(QUERYSEARCH, GetUserName());
                    if(IsDuplicateNameExist(arlSavedSearch, strSaveSearchName))
                    {
                        ShowLableMessage(((MOSSServiceManager)objMossController).GetCustomExceptionMessage(strCurrSiteUrl, "12").ToString());
                        blnIsNameExist = true;
                    }
                    else
                    {
                        //Create Save Search.
                        xmlDocSearchRequest = CreateSaveSearchXML();

                        try
                        {
                            objUISaveSearchHandler = new UISaveSearchHandler();
                            objUISaveSearchHandler.SaveSearchXMLToLibrary(QUERYSEARCH, strSaveSearchName, chbShared.Checked, cboSavedSearch, xmlDocSearchRequest);
                        }
                        catch(WebException webEx)
                        {
                            CommonUtility.HandleException(strCurrSiteUrl, webEx, 1);
                            ShowLableMessage(webEx.Message);
                        }
                        catch(Exception ex)
                        {
                            CommonUtility.HandleException(strCurrSiteUrl, ex);
                        }
                    }
                    if(!blnIsNameExist)
                        cboSavedSearch.Text = txtSaveSearchName.Text.ToString().Trim();
                }
            }
            catch(WebException webEx)
            {
                CommonUtility.HandleException(strCurrSiteUrl, webEx, 1);
                ShowLableMessage(webEx.Message);
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
            finally
            {
                if(objUISaveSearchHandler != null)
                    objUISaveSearchHandler.Dispose();
            }

        }

        /// <summary>
        /// Displays the  Message to the user
        /// </summary>
        /// <param name="message"></param>
        private void ShowLableMessage(string message)
        {
            ExceptionPanel.Visible = true;
            lblException.Text = message;
        }

        /// <summary>
        /// Creates the Save Search Request XML .
        /// </summary>
        /// <returns></returns>
        private XmlDocument CreateSaveSearchXML()
        {
            XmlDocument xmlSearchRequest = null;

            try
            {
                objRequestInfo = SetDataObject(false);
                xmlSearchRequest = objReportController.CreateSearchRequest(objRequestInfo);
            }
            catch
            {
                throw;
            }
            return xmlSearchRequest;
        }


        /// <summary>
        /// Handles the Click event of the btnSearch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            DisplaySearchResults();
        }

        /// <summary>
        /// Redirects the user to AdvanceSearch Results to display the Search Report.
        /// </summary>
        protected void DisplaySearchResults()
        {
            UISaveSearchHandler objUISaveSearchHandler = null;

            try
            {
                objRequestInfo = SetDataObject(false);
                objRequestInfo.Entity.Type = string.Empty; //making type empty to fpr removing type attribute from request info
                objUISaveSearchHandler = new UISaveSearchHandler();
                objUISaveSearchHandler.DisplayResults(Page, objRequestInfo, QUERYSEARCH);
                string strUrl = SEARCHRESULTSPAGE + "?SearchType=" + QUERYSEARCH + "&asset=" + QUERYSEARCH;
                RedirectPage(SEARCHRESULTSPAGE + "?SearchType=" + QUERYSEARCH, QUERYSEARCH);
            }
            catch(WebException webEx)
            {
                CommonUtility.HandleException(strCurrSiteUrl, webEx, 1);
                ShowLableMessage(webEx.Message);
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
            finally
            {
                if(objUISaveSearchHandler != null)
                    objUISaveSearchHandler.Dispose();
            }
        }

        /// <summary>
        /// Instantiates the RequestInfo object with the values set by the User
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        private RequestInfo SetDataObject(bool blnNoType)
        {
            if(blnNoType == true)
                type = string.Empty;
            objRequestInfo.Entity = SetEntity();

            return objRequestInfo;
        }


        /// <summary>
        /// Instantiates the Entity Object
        /// </summary>
        /// <param name="objEntity"></param>
        /// <returns></returns>
        private Entity SetEntityAttributes(Entity entity)
        {
            try
            {
                ArrayList arlAttributes = new ArrayList();
                bool blnIsDisplayAllColumns = false;
                CheckBox chbSelectAll = chbHeaderColumn;
                CheckBox chbSelectColumn = null;
                TextBox txtCriteria = null;
                DropDownList cboOperator = null;

                if(chbSelectAll.Checked)
                {
                    blnIsDisplayAllColumns = true;
                }
                else
                {
                    Display objDisplay = new Display();
                    objDisplay.Value = new ArrayList();

                    entity.Display = objDisplay;

                    for(int intIndex = 0; intIndex < tblColumnNames.Rows.Count; intIndex++)
                    {
                        if(!blnIsDisplayAllColumns)
                        {
                            chbSelectColumn = (CheckBox)tblColumnNames.Rows[intIndex].Cells[0].FindControl("chbColumns");
                            if(chbSelectColumn.Checked)
                            {
                                entity.Display = CreateDisplayColumnNames(entity.Display, tblColumnNames.Rows[intIndex].Cells[1].Text.ToString());
                            }
                        }
                    }
                }
                for(int intIndex = 0; intIndex < tblColumnNames.Rows.Count; intIndex++)
                {
                    txtCriteria = (TextBox)tblColumnNames.Rows[intIndex].Cells[3].FindControl("txtCriteria");
                    cboOperator = (DropDownList)tblColumnNames.Rows[intIndex].Cells[2].FindControl("cboOperator");
                    if(cboOperator.SelectedIndex > 0 && txtCriteria.Text.Trim().Length > 0)
                    {
                        arlAttributes.Add(CreateAttribute(tblColumnNames.Rows[intIndex].Cells[1].Text.ToString(), cboOperator.SelectedItem.Text.ToString(), txtCriteria.Text.ToString()));
                    }
                }

                if(arlAttributes.Count > 1)
                {
                    entity.AttributeGroups = CreateAttributeGroup(arlAttributes);
                }
                else
                {
                    entity.Attribute = arlAttributes;
                }
            }
            catch
            {
                throw;
            }
            return entity;
        }

        /// <summary>
        /// Creates an Array List of  AttributeGroup Object
        /// </summary>
        /// <param name="attributeList"></param>
        /// <returns></returns>
        private ArrayList CreateAttributeGroup(ArrayList attributeList)
        {
            ArrayList arlAttributeGroup = new ArrayList();

            AttributeGroup objAttributeGroup = new AttributeGroup();
            objAttributeGroup.Operator = Convert.ToString(LogicOperator.AND);
            objAttributeGroup.Attribute = attributeList;
            arlAttributeGroup.Add(objAttributeGroup);

            return arlAttributeGroup;
        }

        /// <summary>
        /// Creates an Attribute Object
        /// </summary>
        /// <param name="criteriaName"></param>
        /// <param name="operatorValue"></param>
        /// <param name="criteriaValue"></param>
        /// <returns></returns>
        private Attributes CreateAttribute(string criteriaName, string operatorValue, string criteriaValue)
        {
            Attributes objAttribute = new Attributes();

            objAttribute.Name = criteriaName;
            objAttribute.Operator = operatorValue;
            objAttribute.Value = CreateValue(criteriaValue);

            return objAttribute;
        }

        /// <summary>
        /// Creates a Value Object
        /// </summary>
        /// <param name="criteriaValue"></param>
        /// <returns></returns>
        private ArrayList CreateValue(string criteriaValue)
        {
            ArrayList arlValues = new ArrayList();

            Value objValue = new Value();
            objValue.InnerText = criteriaValue;
            arlValues.Add(objValue);

            return arlValues;
        }

        /// <summary>
        /// Assigns the properties of the Value Object and adds Display Object
        /// </summary>
        /// <param name="objDisplay"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private Display CreateDisplayColumnNames(Display display, string strColumnName)
        {
            Value objValue = new Value();
            objValue.InnerText = strColumnName;

            if(!display.Value.Contains(objValue))
                display.Value.Add(objValue);
            return display;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cboSavedSearch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void CboSavedSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtSaveSearchName.Text = string.Empty;
                if(cboSavedSearch.SelectedIndex != 0)
                {
                    BindUIControls(cboSavedSearch.Text);
                    //added in dream 3.0
                    if(string.Equals(btnSaveSearch.Text.ToString(), MODIFYSRCH) || (string.Equals(btnSaveSearch.Text.ToString(), MODIFYSQL)))
                    {
                        txtSaveSearchName.Text = cboSavedSearch.Text;
                        txtSaveSearchName.Enabled = false;
                    }
                    //
                    lblException.Visible = false;
                }
                else
                {
                    QuerySearchTree_SelectedNodeChanged(null, EventArgs.Empty);
                }
            }
            catch(WebException webEx)
            {
                CommonUtility.HandleException(strCurrSiteUrl, webEx, 1);
                ShowLableMessage(webEx.Message);
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }

        /// <summary>
        /// Loads the controls with values populated on the basis of the search name selected in the drop down list
        /// </summary>
        /// <param name="srchName"></param>
        private void BindUIControls(string strSrchName)
        {
            try
            {


                string strColName = string.Empty;
                XmlDocument xmldoc = new XmlDocument();
                objUtility = new CommonUtility();
                strUserID = objUtility.GetSaveSearchUserName();
                xmldoc = ((MOSSServiceManager)objMossController).GetDocLibXMLFile(QUERYSEARCH, strUserID);
                ClearUIControls();
                if(xmldoc.DocumentElement != null)
                {
                    //set the node selected in tree view
                    //codeChange
                    querySearchTree.CollapseAllNodes();
                    XmlElement rootNode = xmldoc.DocumentElement;
                    XmlNode saveSearchRequestNode = rootNode.SelectSingleNode("saveSearchRequest[@name='" + strSrchName + "']");
                    XmlNode entityNode = saveSearchRequestNode.SelectSingleNode(ENTITYPATH);
                    string strDataSource = entityNode.Attributes[DATASOURCE.ToLowerInvariant()].Value;
                    string strDataProvider = entityNode.Attributes[DATAPROVIDER.ToLowerInvariant()].Value;
                    string strTableName = entityNode.Attributes[ATTRIBNAME].Value;

                    //newcode
                    RadTreeNode trvDataSourNode = querySearchTree.FindNodeByText(DATASOURCE);
                    trvDataSourNode.Expanded = true;
                    RadTreeNode trvDataSourceNode = trvDataSourNode.Nodes.FindNodeByText(strDataSource);
                    trvDataSourceNode.Expanded = true;
                    RadTreeNode trvDataProviderNode = trvDataSourceNode.Nodes.FindNodeByText(strDataProvider);
                    trvDataProviderNode.Expanded = true;
                    RadTreeNode selNode = trvDataProviderNode.Nodes.FindNodeByValue(strTableName);
                    selNode.Selected = true;
                    QuerySearchTree_SelectedNodeChanged(selNode, null);
                    //end of loading columns of the selected table node

                    string strType = entityNode.Attributes[ATTRITYPE].Value;
                    string strIsShared = saveSearchRequestNode.Attributes[ATTRIBSHARED].Value;

                    chbShared.Checked = Convert.ToBoolean(strIsShared);

                    if(cboSavedSearch.Text != strSrchName)
                    {
                        cboSavedSearch.SelectedIndex = -1;
                        cboSavedSearch.Items.FindByText(strSrchName).Selected = true;
                    }

                    //set the button text based on the query string values
                    if(!IsPostBack)
                    {
                        if(Request.QueryString["operation"] != null)
                        {
                            if(string.Equals(Request.QueryString["operation"].ToString(), MODIFY))
                            {
                                if(string.Equals(strType, GEN))
                                {
                                    btnSaveSearch.Text = MODIFYSRCH;
                                }
                                else
                                {
                                    btnSaveSearch.Text = MODIFYSQL;
                                }
                                //added in dream 3.0
                                txtSaveSearchName.Text = strSrchName;
                                //
                                txtSaveSearchName.Enabled = false;
                            }
                        }
                        else
                        {
                            if(string.Equals(strType, GEN))
                            {
                                btnSaveSearch.Text = SAVESRCH;
                            }
                            else
                            {
                                btnSaveSearch.Text = SAVESQL;
                            }
                            txtSaveSearchName.Enabled = true;
                        }
                    }
                    //end of setting the button text

                    //load the values in the grid and sql based on the type of search
                    if(string.Equals(strType, GEN))
                    {
                        CheckBox chbHeader = chbHeaderColumn;

                        txtSQLQuery.Text = string.Empty;
                        txtSQLQuery.ReadOnly = true;
                        tblColumnNames.Enabled = true;

                        ///Load Criteria 
                        for(int intIndex = 0; intIndex < tblColumnNames.Rows.Count; intIndex++)
                        {
                            strColName = tblColumnNames.Rows[intIndex].Cells[1].Text;
                            XmlNode attributeGrp = saveSearchRequestNode.SelectSingleNode(ATTRIBUTEGROUPPATH);
                            XmlNode attribute = null;

                            if(attributeGrp != null)
                            {
                                attribute = attributeGrp.SelectSingleNode("attribute[@name='" + strColName + "']");
                            }
                            else
                            {
                                attribute = saveSearchRequestNode.SelectSingleNode("requestinfo/entity/attribute[@name='" + strColName + "']");
                            }
                            ///for criteria selection 
                            if(attribute != null)
                            {
                                TextBox txtCriteria = (TextBox)tblColumnNames.Rows[intIndex].Cells[3].FindControl("txtCriteria");
                                txtCriteria.Text = attribute.FirstChild.InnerText;
                                DropDownList cboOperator = (DropDownList)tblColumnNames.Rows[intIndex].Cells[2].FindControl("cboOperator");
                                cboOperator.SelectedIndex = -1;
                                cboOperator.Items.FindByText(attribute.Attributes["operator"].Value).Selected = true;
                            }
                        }

                        ///check the display columns 
                        XmlNodeList ValueList = saveSearchRequestNode.SelectNodes(VALUEPATH);
                        if(ValueList.Count > 0)
                        {
                            for(int intIndex = 0; intIndex < tblColumnNames.Rows.Count; intIndex++)
                            {
                                strColName = tblColumnNames.Rows[intIndex].Cells[1].Text;
                                CheckBox chbAttribute = (CheckBox)tblColumnNames.Rows[intIndex].Cells[0].FindControl("chbColumns");
                                chbAttribute.Checked = false;

                                foreach(XmlNode valNode in ValueList)
                                {
                                    if(chbHeader.Checked)
                                        chbHeader.Checked = false;
                                    if(string.Equals(valNode.InnerText, strColName))
                                    {
                                        chbAttribute.Checked = true;
                                        break;
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        txtSQLQuery.Text = string.Empty;
                        txtSQLQuery.ReadOnly = false;
                        tblColumnNames.Enabled = false;
                        XmlNode query = saveSearchRequestNode.SelectSingleNode(QUERYPATH);
                        txtSQLQuery.Text = query.InnerText;
                    }
                }
                EnableCriteria();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Sets the Entity object
        /// </summary>
        /// <returns></returns>
        public Entity SetEntity()
        {
            Entity objEntity;
            try
            {
                objEntity = new Entity();
                objEntity.DataProvider = querySearchTree.SelectedNode.ParentNode.Text;
                objEntity.DataSource = querySearchTree.SelectedNode.ParentNode.ParentNode.Text;
                objEntity.Name = querySearchTree.SelectedNode.Value;

                objEntity.ResponseType = TABULAR;
                Criteria objCriteria = new Criteria();

                switch(type)
                {
                    case SQL:
                        {
                            objEntity.Type = SQL;
                            objEntity.Property = true;
                            Query objQuery = new Query();
                            objQuery.InnerText = txtSQLQuery.Text;
                            objQuery.Type = SIMPLE;
                            objEntity.Query = objQuery;
                            break;
                        }
                    case GEN:
                        {
                            objEntity = SetEntityAttributes(objEntity);
                            objEntity.Type = GEN;
                            objEntity.Property = true;
                            break;
                        }
                    default:
                        {
                            objEntity.Type = ALLCOLUMNSKEYWORD;
                            objEntity.Property = false;
                            objCriteria.Name = COLUMNNAMES;
                            objCriteria.Value = ALLCOLUMNS;
                            objEntity.Criteria = objCriteria;
                            break;
                        }
                }
            }
            catch
            {
                throw;
            }
            return objEntity;
        }

    }
}
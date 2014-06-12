#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: AlterPageSequence.cs
#endregion

using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Net;
using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// Alters the page Sequence
    /// </summary>
    public partial class AlterPageSequence : UIHelper
    {
        #region DECLARATION
        DataTable dtResultTable ;
        DataView dvResultdtView ;
        DataTable dtUpdatedTable ;

        string strListType = string.Empty;
        string strFieldToAlter = string.Empty;
        string strListName = string.Empty;
        string strAuditListName = string.Empty;
        string strParentId = string.Empty;
        string strSortExpression = "{0} {1}";
        const string SORT_ORDER_ASC = "asc";
        const string PAGE_SEQUENCE_COLUMN = "Page_Sequence";
        const string ASSET_TYPE_COLUMN = "Asset_Type";
        const string PAGE_NAME_COLUMN = "Page_Name";
        const string CHAPTER_SEQUENCE_COLUMN = "Chapter_Sequence";
        const string MASTER_PAGE_SEQUENCE_COLUMN = "Master_Page_Name";
        const string INT32_COLUMN_DATATYPE = "System.Int32";
        const string PARENTIDQUERYSTRING = "parentId";
        const string EVENROWSTYLECSS = "evenRowStyle";
        const string ODDROWSTYLECSS = "oddRowStyle";
        const string ALTERCHAPTERPAGESEQUENCE = "Alter Chapter Sequence";
        const string TEXTBOXID  = "TableCellTextboxID";
        const string HIDDENCONTROLID = "TableCellhiddenID";
        const string TABLEROWID = "TableRowID";
        const string TABLECELLID = "TableCellID";
        const string ASSETTYPECOLUMNTITLE = "Asset Type";
        const string MASTERPAGETITLECOLUMN = "Master Page Title";
        const string CHAPTERTITLECOLUMN = "Chapter Name";
        const string CHAPTERPAGETITLECOLUMN = "Chapter Page Title";
        const string TEMPLATEPAGETITLECOLUMN = "Template Page Title";
        #endregion

        #region Overridden Methods
        /// <summary>
        /// page load event triggered by asp.net engine. Used to read query string 
        /// and populate the UI controls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (dtResultTable != null && dtResultTable.Rows.Count > 0)
                {
                    if (!IsPostBack)
                        BindTable();
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
        /// page unload event triggered by asp.net engine. This event is used to
        /// dispose the datatable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnUnload(EventArgs e)
        {
            if (dtResultTable != null)
                dtResultTable.Dispose();
            if (dtUpdatedTable != null)
                dtUpdatedTable.Dispose();

            base.OnUnload(e);
        }

        /// <summary>
        /// page init event triggered by asp.net engine. This event is used to
        /// initialize the controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            try
            {
                /// "listtype" replaced with "listType"
                strListType = HttpContext.Current.Request.QueryString[LISTTYPEQUERYSTRING];
                strParentId = HttpContext.Current.Request.QueryString[PARENTIDQUERYSTRING];
                GetListItems();
                GenerateTable();
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

        #endregion Overridden Methods

        #region EventHandler Methods
        /// <summary>
        /// Renumbers the sequence of the items and populates in the user control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRenumber_Click(object sender, EventArgs e)
        {
            int intRowCount = Table1.Rows.Count;
            int intCellCount = 0;
            try
            {
                dtUpdatedTable = GetTable();
                dtUpdatedTable.Rows.Clear();                
                DataRow dtRow = null;
                TextBox txtPageSequence = null;
                HiddenField hdnRowId = null;
                int intPageSequence = 0;
                int intHiddenValue = 0;
                for (int intRowIndex = 0; intRowIndex < intRowCount - 1; intRowIndex++)
                {
                    intCellCount = Table1.Rows[0].Cells.Count;
                    dtRow = dtUpdatedTable.NewRow();
                    for (int intCellIndex = 0; intCellIndex < intCellCount; intCellIndex++)
                    {
                        if (intCellIndex == 0)
                        {
                            txtPageSequence = (TextBox)this.FindControl(TEXTBOXID + intRowIndex.ToString() + "0");

                            hdnRowId = (HiddenField)this.FindControl(HIDDENCONTROLID + intRowIndex.ToString() + "0");
                            int.TryParse(txtPageSequence.Text, out intPageSequence);
                            dtRow[strFieldToAlter] = intPageSequence;
                            int.TryParse(hdnRowId.Value, out intHiddenValue);
                            dtRow[DWBIDCOLUMN] = intHiddenValue;
                        }
                        else
                        {
                            dtRow[intCellIndex] = Table1.Rows[intRowIndex + 1].Cells[intCellIndex].Text;
                        }
                    }
                    dtUpdatedTable.Rows.Add(dtRow);
                }
                BindGridView();
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
        /// Gets the sequence number entered by the user and sort and then updates the
        /// sharepoint list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                GetUserEnteredValues();
                DataView dvNewView = dtUpdatedTable.DefaultView;              
                dvNewView.Sort = string.Format(strSortExpression, strFieldToAlter, SORT_ORDER_ASC);
                int intPageSequence = 10;
                DataTable dtSortedTable = dvNewView.ToTable();
                for (int intRowIndex = 0; intRowIndex < dtSortedTable.Rows.Count; intRowIndex++)
                {
                    dtSortedTable.Rows[intRowIndex][0] = intPageSequence;
                    intPageSequence += 10;

                }
                dvNewView = dtSortedTable.DefaultView;

                UpdateListItemSequence(strListName, dvNewView, strAuditListName, AUDITACTIONUPDATION, strListType.ToUpperInvariant());
                Response.Redirect(UrlToRedirect(), false);
                if (dtSortedTable != null)
                {
                    dtSortedTable.Dispose();
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
        /// Event handler is triggered when user cancels his action user is then  redirected 
        /// the previous maintainence sceen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(UrlToRedirect(), false);
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

        #endregion EventHandler Methods

        #region Private Methods

        /// <summary>
        /// Reads the sequence number entered by the user in the UI and
        /// constructs a table which can be further sorted.
        /// </summary>
        private void GetUserEnteredValues()
        {
            int intRowCount = Table1.Rows.Count;
            int intCellCount = 0;
            try
            {
                dtUpdatedTable = GetTable();               
                DataRow dtRow = null;
                TextBox txtPageSequence = null;
                HiddenField hdnRowId = null;
                int intPageSequence = 0;
                int intHiddenValue = 0;
                for (int intRowIndex = 0; intRowIndex < intRowCount - 1; intRowIndex++)
                {
                    intCellCount = Table1.Rows[0].Cells.Count;
                    dtRow = dtUpdatedTable.NewRow();
                    for (int intCellIndex = 0; intCellIndex < intCellCount; intCellIndex++)
                    {
                        if (intCellIndex == 0)
                        {
                            txtPageSequence = (TextBox)this.FindControl(TEXTBOXID + intRowIndex.ToString() + "0");

                            hdnRowId = (HiddenField)this.FindControl(HIDDENCONTROLID + intRowIndex.ToString() + "0");
                            int.TryParse(txtPageSequence.Text, out intPageSequence);
                            dtRow[strFieldToAlter] = intPageSequence;
                            int.TryParse(hdnRowId.Value, out intHiddenValue);
                            dtRow[DWBIDCOLUMN] = intHiddenValue;
                        }
                        else
                        {
                            dtRow[intCellIndex] = Table1.Rows[intRowIndex + 1].Cells[intCellIndex].Text;
                        }
                    }
                    dtUpdatedTable.Rows.Add(dtRow);
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
        /// Sorts the value entered by the user with the sequence starting from 10 and bind the 
        /// table to the UI table.
        /// </summary>
        private void BindGridView()
        {
            DataTable dtSortedTable = null;
            try
            {
                if (IsPostBack)
                {
                    dvResultdtView = dtUpdatedTable.DefaultView;
                }
                else
                {
                    dvResultdtView = dtResultTable.DefaultView;
                }             
                dvResultdtView.Sort = string.Format(strSortExpression, strFieldToAlter, SORT_ORDER_ASC);
                int intPageSequence = 10;
                dtSortedTable = dvResultdtView.ToTable();
                for (int intRowIndex = 0; intRowIndex < dtSortedTable.Rows.Count; intRowIndex++)
                {
                    dtSortedTable.Rows[intRowIndex][0] = intPageSequence;
                    intPageSequence += 10;

                }
                dvResultdtView = dtSortedTable.DefaultView;
                BindTable();
                if (dtSortedTable != null)
                {
                    dtSortedTable.Dispose();
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
        /// Creates the Table dynamically
        /// </summary>
        private void GenerateTable()
        {
            if (dtResultTable == null)
                return;
            int intRowNumber = dtResultTable.Rows.Count;
            TableRow trRow = new TableRow();
            TableCell tcCell = new TableCell();
            TextBox txtBox = new TextBox();

            int intColumnCount = 4;

            try
            {
                for (int intRowIndex = 0; intRowIndex < intRowNumber; intRowIndex++)
                {
                    trRow = new TableRow();
                    if ((intRowIndex % 2) == 0)
                    {
                        trRow.CssClass = EVENROWSTYLECSS;
                    }
                    else
                    {
                        trRow.CssClass = ODDROWSTYLECSS;
                    }
                    trRow.ID = TABLEROWID + intRowIndex.ToString();
                    for (int intCellIndex = 0; intCellIndex < intColumnCount - 1; intCellIndex++)
                    {
                        tcCell = new TableCell();
                        tcCell.ID = TABLECELLID + intRowIndex.ToString() + intCellIndex.ToString();
                        if (intCellIndex == 0)
                        {
                            txtBox = new TextBox();
                            txtBox.ID = TEXTBOXID + intRowIndex.ToString() + intCellIndex.ToString();
                            txtBox.MaxLength = 4;
                            txtBox.Attributes.Add(ONKEYUPEVENTNAME, "return validate('" + txtBox.ID + "');");

                            tcCell.Controls.Add(txtBox);
                            HiddenField hdnTableCell = new HiddenField();
                            hdnTableCell.ID = HIDDENCONTROLID + intRowIndex.ToString() + intCellIndex.ToString();

                            tcCell.Controls.Add(hdnTableCell);
                        }
                        trRow.Controls.Add(tcCell);
                    }

                    Table1.Rows.Add(trRow);
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
        /// Gets the List items from list.
        /// </summary>
        private void GetListItems()
        {
            string strCAMLQuery = string.Empty;
            string strFieldsToView = string.Empty;
            if (string.IsNullOrEmpty(strListType))
            {
                return;
            }
            try
            {
                switch (strListType.ToUpperInvariant())
                {
                    case MASTERPAGE:
                        strCAMLQuery = @"<OrderBy><FieldRef Name='Page_Sequence' /></OrderBy><Where><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq></Where>";
                        strFieldsToView = "<FieldRef Name='Page_Sequence' /><Value Type='Choice'/><FieldRef Name='Title' /><FieldRef Name='Asset_Type' /><FieldRef Name='ID' />";
                        dtResultTable = GetListItems(MASTERPAGELIST, strCAMLQuery, strFieldsToView);
                        strFieldToAlter = PAGE_SEQUENCE_COLUMN;
                        strListName = MASTERPAGELIST;
                        strAuditListName = MASTERPAGEAUDITLIST;
                        break;
                    case CHAPTER:
                        strCAMLQuery = @"<OrderBy><FieldRef Name='Chapter_Sequence' /></OrderBy><Where><And><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq><Eq><FieldRef Name='Book_ID' /><Value Type='Number'>" + strParentId + "</Value></Eq></And></Where>";
                        strFieldsToView = "<FieldRef Name='Chapter_Sequence' /><FieldRef Name='Title' /><FieldRef Name='Asset_Type' /><FieldRef Name='ID' />";
                        dtResultTable = GetListItems(DWBCHAPTERLIST, strCAMLQuery, strFieldsToView);
                        strFieldToAlter = CHAPTER_SEQUENCE_COLUMN;
                        strListName = DWBCHAPTERLIST;
                        strAuditListName = CHAPTERAUDITLIST;
                        lbPageSequence.Text = ALTERCHAPTERPAGESEQUENCE;
                        break;
                    case CHAPTERPAGEMAPPING:
                        strCAMLQuery = @"<OrderBy><FieldRef Name='Page_Sequence' /></OrderBy><Where><And><Eq><FieldRef Name='Terminate_Status' /><Value Type='Choice'>No</Value></Eq><Eq><FieldRef Name='Chapter_ID' /><Value Type='Number'>" + strParentId + "</Value></Eq></And></Where>";
                        strFieldsToView = "<FieldRef Name='Page_Sequence' /><FieldRef Name='Page_Name' /><FieldRef Name='Asset_Type' /><FieldRef Name='ID' />";
                        dtResultTable = GetListItems(CHAPTERPAGESMAPPINGLIST, strCAMLQuery, strFieldsToView);
                        strFieldToAlter = PAGE_SEQUENCE_COLUMN;
                        strListName = CHAPTERPAGESMAPPINGLIST;
                        strAuditListName = CHAPTERPAGESMAPPINGAUDITLIST;
                        break;
                    case TEMPLATEPAGESSEQUENCE:
                        {
                            strCAMLQuery = @"<OrderBy><FieldRef Name='Page_Sequence' /></OrderBy><Where><Eq><FieldRef Name='Template_ID' /><Value Type='Number'>" + strParentId + "</Value></Eq></Where>";
                            strFieldsToView = "<FieldRef Name='Page_Sequence' /><FieldRef Name='Master_Page_Name' /><FieldRef Name='Asset_Type' /><FieldRef Name='ID' />"; 
                            strFieldToAlter = PAGE_SEQUENCE_COLUMN;
                            strListName = TEMPLATEPAGESMAPPINGLIST;
                            strAuditListName = TEMPLATECONFIGURATIONAUDIT;
                            dtResultTable = GetListItems(strListName, strCAMLQuery, strFieldsToView);

                            break;
                        }
                }

                dvResultdtView = dtResultTable.DefaultView;
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
        /// Adds the Item  Id as the hidden variable to the Table row.
        /// </summary>
        private void BindTable()
        {
            int intRowcount = Table1.Rows.Count;
            TextBox txtPageSequene = null;

            HiddenField hdnRowId = null;
            int intColumnCount = Table1.Rows[0].Cells.Count;

            Table1.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            Table1.Rows[0].Cells[1].HorizontalAlign = HorizontalAlign.Center;
            Table1.Rows[0].Cells[2].HorizontalAlign = HorizontalAlign.Center;
            switch (strListType.ToUpperInvariant())
            {
                case MASTERPAGE:
                    Table1.Rows[0].Cells[1].Text = MASTERPAGETITLECOLUMN;
                    Table1.Rows[0].Cells[2].Text = ASSETTYPECOLUMNTITLE;
                    break;
                case CHAPTER:
                    Table1.Rows[0].Cells[1].Text = CHAPTERTITLECOLUMN;
                    Table1.Rows[0].Cells[2].Text = ASSETTYPECOLUMNTITLE;
                    break;
                case CHAPTERPAGEMAPPING:
                    Table1.Rows[0].Cells[1].Text =CHAPTERPAGETITLECOLUMN;
                    Table1.Rows[0].Cells[2].Text = ASSETTYPECOLUMNTITLE;
                    break;
                case TEMPLATEPAGESSEQUENCE:
                    {
                        Table1.Rows[0].Cells[1].Text = TEMPLATEPAGETITLECOLUMN;
                        Table1.Rows[0].Cells[2].Text = ASSETTYPECOLUMNTITLE;
                        break;
                    }
            }

            for (int intRowIndex = 0; intRowIndex < intRowcount - 1; intRowIndex++)
            {
                for (int intColumnIndex = 0; intColumnIndex < intColumnCount; intColumnIndex++)
                {
                    if (intColumnIndex == 0)
                    {
                        txtPageSequene = (TextBox)this.FindControl(TEXTBOXID + intRowIndex.ToString() + "0");
                        txtPageSequene.Text = dvResultdtView[intRowIndex][intColumnIndex].ToString();
                        txtPageSequene.Attributes.Add(STYLE, TEXTALIGNCENTER);

                        hdnRowId = (HiddenField)this.FindControl(HIDDENCONTROLID + intRowIndex.ToString() + "0");
                        hdnRowId.Value = dvResultdtView[intRowIndex][DWBIDCOLUMN].ToString();
                    }
                    else
                    {
                        Table1.Rows[intRowIndex + 1].Cells[intColumnIndex].Text = dvResultdtView[intRowIndex][intColumnIndex].ToString();
                    }
                }
            }
        }

        /// <summary>
        /// Returns the structure of the new table.
        /// </summary>
        /// <returns></returns>
        private DataTable GetTable()
        {            
            DataTable dtUpdatedDataTable = new DataTable();
            DataColumn dtCol = null;
            switch (strListType.ToUpperInvariant())
            {
                case MASTERPAGE:

                    dtCol = new DataColumn(PAGE_SEQUENCE_COLUMN);
                    dtCol.DataType = System.Type.GetType(INT32_COLUMN_DATATYPE);
                    dtUpdatedDataTable.Columns.Add(dtCol);
                    dtCol = new DataColumn(DWBTITLECOLUMN);
                    dtUpdatedDataTable.Columns.Add(dtCol);
                    dtCol = new DataColumn(ASSET_TYPE_COLUMN);
                    dtUpdatedDataTable.Columns.Add(dtCol);
                    dtCol = new DataColumn(DWBIDCOLUMN);
                    dtCol.DataType = System.Type.GetType(INT32_COLUMN_DATATYPE);
                    dtUpdatedDataTable.Columns.Add(dtCol);
                    break;
                case CHAPTERPAGEMAPPING:
                    dtCol = new DataColumn(PAGE_SEQUENCE_COLUMN);
                    dtCol.DataType = System.Type.GetType(INT32_COLUMN_DATATYPE);
                    dtUpdatedDataTable.Columns.Add(dtCol);
                    dtCol = new DataColumn(PAGE_NAME_COLUMN);
                    dtUpdatedDataTable.Columns.Add(dtCol);
                    dtCol = new DataColumn(ASSET_TYPE_COLUMN);
                    dtUpdatedDataTable.Columns.Add(dtCol);
                    dtCol = new DataColumn(DWBIDCOLUMN);
                    dtCol.DataType = System.Type.GetType(INT32_COLUMN_DATATYPE);
                    dtUpdatedDataTable.Columns.Add(dtCol);
                    break;
                case CHAPTER:
                    dtCol = new DataColumn(CHAPTER_SEQUENCE_COLUMN);
                    dtCol.DataType = System.Type.GetType(INT32_COLUMN_DATATYPE);
                    dtUpdatedDataTable.Columns.Add(dtCol);

                    dtCol = new DataColumn(DWBTITLECOLUMN);
                    dtUpdatedDataTable.Columns.Add(dtCol);
                    dtCol = new DataColumn(ASSET_TYPE_COLUMN);
                    dtUpdatedDataTable.Columns.Add(dtCol);
                    dtCol = new DataColumn(DWBIDCOLUMN);
                    dtCol.DataType = System.Type.GetType(INT32_COLUMN_DATATYPE);
                    dtUpdatedDataTable.Columns.Add(dtCol);
                    break;

                case TEMPLATEPAGESSEQUENCE:
                    {
                        dtCol = new DataColumn(PAGE_SEQUENCE_COLUMN);
                        dtCol.DataType = System.Type.GetType(INT32_COLUMN_DATATYPE);
                        dtUpdatedDataTable.Columns.Add(dtCol);

                        dtCol = new DataColumn(MASTER_PAGE_SEQUENCE_COLUMN);
                        dtUpdatedDataTable.Columns.Add(dtCol);
                        dtCol = new DataColumn(ASSET_TYPE_COLUMN);
                        dtUpdatedDataTable.Columns.Add(dtCol);
                        dtCol = new DataColumn(DWBIDCOLUMN);
                        dtCol.DataType = System.Type.GetType(INT32_COLUMN_DATATYPE);
                        dtUpdatedDataTable.Columns.Add(dtCol);
                        break;
                    }


            }
            return dtUpdatedDataTable;
        }

        /// <summary>
        /// Gets the URL to which user 
        /// </summary>
        /// <returns></returns>
        private string UrlToRedirect()
        {
            string strRedirectionUrl = string.Empty;
            switch (strListType.ToUpperInvariant())
            {
                case MASTERPAGE: strRedirectionUrl = MASTERPAGEURL;
                    break;
                case CHAPTER:
                    strRedirectionUrl = MAINTAINCHAPTERURL + "?" + BOOKIDQUERYSTRING + "=" + strParentId;
                    break;
                case CHAPTERPAGEMAPPING:
                    strRedirectionUrl = MAINTAINCHAPTERPAGESURL+ "?"+ CHAPTERIDQUERYSTRING +"=" + strParentId;
                    break;
                case TEMPLATEPAGESSEQUENCE:
                    strRedirectionUrl = MAINTAINTEMPLATEPAGESURL + "?" + IDVALUEQUERYSTRING + "=" + strParentId;
                    break;
                default:
                    strRedirectionUrl = DWBHOMEURL;
                    break;
            }
            return strRedirectionUrl;
        }
        #endregion Private Methods

    }
}
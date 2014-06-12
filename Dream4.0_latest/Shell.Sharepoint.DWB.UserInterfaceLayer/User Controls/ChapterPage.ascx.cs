#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: ChapterPage.cs
#endregion


using System;
using System.Web;
using System.Net;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;

using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DWB.BusinessLogicLayer;
namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// Class handles the Add/Edit Chapter page.
    /// </summary>
    public partial class ChapterPage : UIHelper
    {
        #region DECLARATION

        const string BOOKPAGES = "BookPages";
        const string PAGETITLEEXISTSMSG = "Page Title already exist. Please enter a different title.";
        const string MAINTAINMASTERURL = "MaintainMasterPage.aspx";
        const string MAINTAINCHAPTERPAGEURL = "MaintainChaptersPages.aspx";
        const string CONNECTIONTYPEI = "Automated";
        
        ListEntry objListEntry;
        string strChapterID = string.Empty;
        ChapterBLL objChapterBLL;

        #endregion

        #region Protected Methods
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            strChapterID = HttpContext.Current.Request.QueryString[CHAPTERIDQUERYSTRING];
            try
            {
                if (!IsPostBack)
                {
                    LoadControlsOnPageLoad();

                }
            }
            catch (WebException webEx)
            {
                lblException.Text = webEx.Message;
                lblException.Visible = true;

            }
            catch (Exception ex)
            {

                CommonUtility.HandleException(strParentSiteURL, ex);

            }
        }

        /// <summary>
        /// Handles the Click event of the cmdCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(MAINTAINCHAPTERPAGEURL + "?"+CHAPTERIDQUERYSTRING +"=" + strChapterID, false);
        }

        /// <summary>
        /// Saves the page to the chapter
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                SetChapterPageDetails();
                objChapterBLL = new ChapterBLL();
                if (objListEntry != null)
                    objChapterBLL.AddPageToChapters(strParentSiteURL, objListEntry, CHAPTERPAGESMAPPINGLIST, CHAPTERPAGESMAPPINGAUDITLIST, GetUserName(), AUDITACTIONCREATION);
                Page.Response.Redirect(MAINTAINCHAPTERPAGEURL + "?" + CHAPTERIDQUERYSTRING + "=" + strChapterID, false);
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
        /// Gets the sign off discipline for the selected master page.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cboMasterPages_SelectedIndexChanged(object sender, EventArgs e)
        {
           
                string strfieldsToView = "<FieldRef Name='Sign_Off_Discipline' />";
                string strCamlQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + cboMasterPages.SelectedValue + "</Value></Eq></Where>";
                try
                {
                    DataTable dtResultTable = GetListItems(MASTERPAGELIST, strCamlQuery, strfieldsToView);
                    if (dtResultTable != null && dtResultTable.Rows.Count > 0)
                    {
                        txtDiscipline.Text = Convert.ToString(dtResultTable.Rows[0]["Sign_Off_Discipline"]);
                    }
                    else
                    {
                        txtDiscipline.Text = String.Empty;
                    }
                    if (dtResultTable != null)
                        dtResultTable.Dispose();
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
        /// Sets the chapter details based on the selected master page.
        /// </summary>
        private void SetChapterPageDetails()
        {
            try
            {
                objChapterBLL = new ChapterBLL();
                ChapterDetails objChapterdetails = null;
                int intChapterId = 0;
                string strConnectionType = string.Empty;
                ChapterPagesMapping objChapterPageMapping = null;
                List<ChapterPagesMapping> lstChapterPageMapping = new List<ChapterPagesMapping>();
                string strCamlQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + cboMasterPages.SelectedValue + "</Value></Eq></Where>";
                DataTable dtresultTable = GetListItems(MASTERPAGELIST, strCamlQuery, string.Empty);
                if (dtresultTable != null && dtresultTable.Rows.Count > 0)
                {

                    objListEntry = new ListEntry();
                    objChapterdetails = new ChapterDetails();
                    int.TryParse(strChapterID, out intChapterId);
                    objChapterdetails.RowID = intChapterId;
                    for (int intRowIndex = 0; intRowIndex < dtresultTable.Rows.Count; intRowIndex++)
                    {

                        objChapterPageMapping = new ChapterPagesMapping();

                        objChapterPageMapping.Discipline = Convert.ToString(dtresultTable.Rows[intRowIndex]["Sign_Off_Discipline"]);
                        objChapterPageMapping.PageName = Convert.ToString(dtresultTable.Rows[intRowIndex][DWBTITLECOLUMN]);
                        objChapterPageMapping.PageActualName = Convert.ToString(dtresultTable.Rows[intRowIndex]["Title_Template"]);
                        objChapterPageMapping.PageSequence = Convert.ToInt32(dtresultTable.Rows[intRowIndex]["Page_Sequence"]);
                        objChapterPageMapping.AssetType = Convert.ToString(dtresultTable.Rows[intRowIndex]["Asset_Type"]);
                        objChapterPageMapping.StandardOperatingProc = Convert.ToString(dtresultTable.Rows[intRowIndex]["Standard_Operating_Procedure"]);
                        strConnectionType = Convert.ToString(dtresultTable.Rows[intRowIndex][CONNECTIONTYPECOLUMN]);
                        objChapterPageMapping.ConnectionType = strConnectionType;
                        objChapterPageMapping.PageURL = Convert.ToString(dtresultTable.Rows[intRowIndex]["Page_URL"]);
                        if (!strConnectionType.Contains(CONNECTIONTYPEI))
                        {
                            objChapterPageMapping.Empty = "Yes";
                        }
                        if (cboPageOwner.SelectedIndex == 0)
                        {
                            objChapterPageMapping.PageOwner = Convert.ToString(dtresultTable.Rows[intRowIndex]["Page_Owner"]);
                        }
                        else
                        {
                           
                            DataTable dtUser = new DataTable();
                            strCamlQuery = string.Empty;
                            string strViewFields = string.Empty;
                            strCamlQuery = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + cboPageOwner.SelectedValue + "</Value></Eq></Where>"; ;
                            strViewFields = @"<FieldRef Name='ID' /><FieldRef Name='Windows_User_ID' /><FieldRef Name='DWBUserName' />";
                            CommonBLL objCommonBLL = new CommonBLL();
                            dtUser = objCommonBLL.ReadList(strParentSiteURL, USERLIST, strCamlQuery, strViewFields);

                            if (dtUser != null && dtUser.Rows.Count > 0)
                            {
                                objChapterPageMapping.PageOwner = Convert.ToString(dtUser.Rows[0][DWBUSERIDCOLUMN]);
                                dtUser.Dispose();
                            }
                        }
                        objChapterPageMapping.MasterPageID = Convert.ToInt32(dtresultTable.Rows[intRowIndex][DWBIDCOLUMN]);
                        objChapterPageMapping.Created_Date = Convert.ToString(dtresultTable.Rows[intRowIndex]["Created"]);
                        objChapterPageMapping.Created_By = Convert.ToString(dtresultTable.Rows[intRowIndex]["Page_Owner"]);
                        lstChapterPageMapping.Add(objChapterPageMapping);
                    }
                    objListEntry.ChapterPagesMapping = lstChapterPageMapping;
                    objListEntry.ChapterDetails = objChapterdetails;
                }
                if (dtresultTable != null)
                    dtresultTable.Dispose();
            }
            catch (Exception ex)
            {

                CommonUtility.HandleException(strParentSiteURL, ex);

            }
        }

        /// <summary>
        /// Loads the controls on page load.
        /// </summary>
        private void LoadControlsOnPageLoad()
        {
            try
            {
                objChapterBLL = new ChapterBLL();
                ListItem objListItem = null;
                bool blnPageOwnerSet = false;
                DataTable dtresulttable = objChapterBLL.GetMasterPagesForChapter(strParentSiteURL, strChapterID, MASTERPAGELIST, CHAPTERPAGESMAPPINGLIST, DWBCHAPTERLIST);
                if (dtresulttable != null)
                {
                    cboMasterPages.Items.Clear();

                    cboMasterPages.DataSource = dtresulttable;
                    cboMasterPages.DataTextField = DWBTITLECOLUMN;
                    cboMasterPages.DataValueField = DWBIDCOLUMN;

                    cboMasterPages.DataBind();
                    cboMasterPages.Items.Insert(0, DROPDOWNDEFAULTTEXT);
                }
                dtresulttable = objChapterBLL.GetOwnersForChapterPage(strParentSiteURL, strChapterID, DWBCHAPTERLIST, DWBBOOKLIST, TEAMSTAFFLIST);
                if (dtresulttable != null)
                {
                    cboPageOwner.Items.Clear();

                    for (int intRowIndex = 0; intRowIndex < dtresulttable.Rows.Count; intRowIndex++)
                    {
                        objListItem = new ListItem();
                        objListItem.Value = Convert.ToString(dtresulttable.Rows[intRowIndex][USERIDCOLUMN]);
                        objListItem.Text = Convert.ToString(dtresulttable.Rows[intRowIndex][DISCIPLINECOLUMN]) + "-" +
                           Convert.ToString(dtresulttable.Rows[intRowIndex][DWBTITLECOLUMN]);
                        if (!blnPageOwnerSet)
                        {
                            objListItem.Selected = true;
                            blnPageOwnerSet = true;
                        }

                        cboPageOwner.Items.Add(objListItem);
                    }
                }
                cboPageOwner.Items.Insert(0, DROPDOWNDEFAULTTEXTNONE);
                cboPageOwner.ClearSelection();
                cboPageOwner.SelectedIndex = 0;
                if (dtresulttable != null)
                    dtresulttable.Dispose();
            }
            catch (Exception ex)
            {

                CommonUtility.HandleException(strParentSiteURL, ex);

            }
        }
        #endregion
    }
}
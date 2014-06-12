#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: StoryBoardConfirmation.ascx.cs
//Modified By:Gopinath
//Date: 02-11-2010
//Reason: To implement the DWB version two changes
#endregion

using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DWB.BusinessLogicLayer;
using Shell.SharePoint.DREAM.Utilities;
using DWBDataObjects = Shell.SharePoint.DWB.Business.DataObjects;
using System.Net;

namespace Shell.SharePoint.DWB.UserInterfaceLayer
{
    /// <summary>
    /// Displays Story Board options.
    /// </summary>
    public partial class StoryBoardConfirmation : UIHelper
    {
        #region VARIABLE DECLARATION
        public bool blnType = false;
        public bool blnIsBookOwnerOrAdmin = false;

        string strWellBookId = string.Empty;
        string strWellChapterId = string.Empty;
        ListEntry objListEntry = null;
        TreeNodeSelection objTreeNodeSelection;

        #endregion VARIABLE DECLARATION

        #region Page Events

        /// <summary>
        /// Added By Gopinath
        /// Date : 02-11-2010
        /// Reason : Loading the page with populated drodowns on Init method.
        /// </summary>        
        protected void Page_Init()
        {
            try
            {
                #region On First Post back
                if (!Page.IsPostBack)
                {
                    //Modified By: Gopinath
                    //Date : 04/11/2010
                    //Reason : Code modified to check the BookId and ChapterId from Webpart session Properties
                    //Fetch bookId from WebPart session when book selected
                    if (HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES] != null)
                    {
                        objTreeNodeSelection = (TreeNodeSelection)HttpContext.Current.Session[SESSIONWEBPARTPROPERTIES];

                        LoadStoryBoardInfo(objTreeNodeSelection);

                        chkPrintMyPagesOnly.Attributes.Add("onclick", "javascript:return checkPrintMyPages('" + chkPrintMyPagesOnly.ClientID + "','" + rblIncludeFilter.ClientID + "' );");
                        cboPageName.Attributes.Add("onChange", "javascript:return AutoSelectDisciplinePerPageName();");
                    }
                }
                #endregion On First Post back
            }
            #region Exception Block
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
            #endregion Exception Block
        }

        /// <summary>
        /// Added by Gopinath
        /// Date :25/11/2010
        /// Reason : For code review comment, split the code and extracted as method. 
        /// </summary>
        /// <param name="objTreeNodeSelection">TreeNodeSelection</param>
        private void LoadStoryBoardInfo(TreeNodeSelection objTreeNodeSelection)
        {
            #region Check for User selection either Book or Chapter
            //For Book
            if (objTreeNodeSelection.IsBookSelected)
            {
                strWellBookId = objTreeNodeSelection.BookID;

                if (string.IsNullOrEmpty(strWellBookId))
                    return;

                EnablePrintMyPagesForBook(strWellBookId);

                //Fill Page Name and Discipline dropdowns
                PopulateDropDowns(strWellBookId);

                //If book and chapter selected Type is true. If selects Page its false 
                blnType = true;
            }
            //For Chapter
            else if (objTreeNodeSelection.IsChapterSelected)
            {
                strWellBookId = objTreeNodeSelection.BookID;
                strWellChapterId = objTreeNodeSelection.ChapterID;

                if (string.IsNullOrEmpty(strWellChapterId))
                    return;

                EnablePrintMyPagesForChapter(strWellBookId);

                //Fill Page Name and Discipline dropdowns
                PopulateDropDowns(strWellBookId);

                //If book and chapter selected Type is true. If selects Page its false 
                blnType = true;
            }
            //For Page
            else
            {
                pnlPrintMyPages.Visible = false;
                pnlFilterOptions.Visible = false;
                //Type is Page.
                blnType = false;
            }
            #endregion Check for User selection either Book or Chapter
        }

        #endregion Page Events

        #region Private Methods

        /// <summary>
        /// Enable Print My Pages Option by checking the current user for book
        /// Added by Gopinath
        /// Date : 02-11-2010
        /// Reason : To check the current user permission as per enable the Print my pages option
        /// </summary>
        /// <param name="wellBookId">string</param>
        private void EnablePrintMyPagesForBook(string wellBookId)
        {
            // Get the Book Details for the selected Book Id and populate the controls with data
            objListEntry = GetDetailsForSelectedID(wellBookId, DWBBOOKLIST, WELLBOOK);

            if (IsUserAsBookOwnerOrAdmin(WELLBOOKVIEWERCONTROLBOOK, objListEntry.WellBookDetails.BookOwner, objListEntry.WellBookDetails.TeamID))
            {
                chkPrintMyPagesOnly.Checked = false;//If current user BO/AD uncheck the printmypages default.
                rblIncludeFilter.SelectedValue = "1"; //If current user as Page Owner then Include Filter Yes.
                blnIsBookOwnerOrAdmin = true;
            }
            else //Page Owner
            {
                chkPrintMyPagesOnly.Checked = true;
                rblIncludeFilter.SelectedValue = "1";
                blnIsBookOwnerOrAdmin = false;
            }

        }

        /// <summary>
        /// Enables the print my pages for chapter.
        /// Added by Gopinath
        /// Date : 02-11-2010
        /// Reason : To check the current user permission as per enable the Print my pages option
        /// </summary>
        /// <param name="strWellBookId">The STR well book id.</param>
        private void EnablePrintMyPagesForChapter(string wellBookId)
        {
            // Get the Chapter Details for the selected Chapter Id and populate the controls with data
            objListEntry = GetDetailsForSelectedID(wellBookId, DWBBOOKLIST, WELLBOOK);

            //If Book owner is normal user or DWB User then it returns true
            if (IsUserAsDWBUser(objListEntry.WellBookDetails.BookOwner))
                pnlPrintMyPages.Visible = false; //If current user as DWB User then not display PrintMyPages.

            if (IsUserAsBookOwnerOrAdmin(WELLBOOKVIEWERCONTROLBOOK, objListEntry.WellBookDetails.BookOwner, objListEntry.WellBookDetails.TeamID))
            {
                blnIsBookOwnerOrAdmin = true;
            }
            else
            {
                chkPrintMyPagesOnly.Checked = true;
                rblIncludeFilter.SelectedValue = "1";
                blnIsBookOwnerOrAdmin = false;
            }
        }

        /// <summary>
        /// Populate Page Name and Discipline drop downs
        /// Added by Gopinath
        /// Date : 02-11-2010
        /// Reason : To populate the Page Name and Discipline drop downs by using BookId
        /// </summary>
        /// <param name="strWellBookId">string</param>
        private void PopulateDropDowns(string wellBookId)
        {
            if (string.IsNullOrEmpty(wellBookId))
                return;

            string strCamlQuery = @"<Where><Eq><FieldRef Name='Book_ID' />
                 <Value Type='Number'>" + wellBookId + "</Value></Eq></Where>";

            BindPageNameDropDown(cboPageName, DWBCHAPTERLIST, strCamlQuery);
            cboPageName.Items.Insert(0, DROPDOWNDEFAULTTEXTALL);
            if (cboPageName.Items.Count == 1)
            {
                cboPageName.Items.Insert(1, DROPDOWNDEFAULTTEXTNONE);
            }

            BindDisciplineDropDown(cboDisciplineName, DWBCHAPTERLIST, strCamlQuery);
            cboDisciplineName.Items.Insert(0, DROPDOWNDEFAULTTEXTALL);
            if (cboDisciplineName.Items.Count == 1)
            {
                cboDisciplineName.Items.Insert(1, DROPDOWNDEFAULTTEXTNONE);
            }
        }

        #endregion Private Methods


    }
}
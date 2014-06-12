#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: CustomGridViewHelper.cs
#endregion
/// <summary> 
/// This is the Custom Grid view which is inherited from the GridView base Class
/// </summary>
using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace Shell.SharePoint.DREAM.CustomDataGrid
{
    /// <summary>
    /// The CustomGridView Helper Partial class
    /// </summary>
    public partial class CustomGridView : System.Web.UI.WebControls.GridView
    {
        #region Constants

        
        
        private const string CHECKBOXCOLUMNHEADERTEMPLATE = "<input type='checkbox' hidefocus='true' id='{0}' name='{0}' {1} onclick='CheckAll(this)'>";
        private const string CHECKBOXCOLUMNHEADERID = "HeaderButton";
        #endregion
        #region Methods
        // METHOD:: add a brand new checkbox column
        /// <summary>
        /// Adds the check box column.
        /// </summary>
        /// <param name="columnList">The column list.</param>
        /// <returns></returns>
        protected virtual ArrayList AddCheckBoxColumn(ICollection columnList)
        {
            ArrayList arlColumnlist=null;
            string strShouldCheck = string.Empty;
            string strCheckBoxID = string.Empty;
            try
            {
                arlColumnlist = new ArrayList(columnList);

                // Determine the check state for the header checkbox
                strCheckBoxID = String.Format(CHECKBOXCOLUMNHEADERID, ClientID);
                if (!DesignMode)
                {
                    object objCheckBoxID = Page.Request[strCheckBoxID];                    
                }
                // Create a new custom CheckBoxField object 
                InputCheckBoxField objCheckBoxfield = new InputCheckBoxField();
                objCheckBoxfield.HeaderText = String.Format(CHECKBOXCOLUMNHEADERTEMPLATE, strCheckBoxID, strShouldCheck);
                objCheckBoxfield.ReadOnly = true;                

                // Insert the checkbox field into the list at the specified position
                if (CheckBoxColumnIndex > arlColumnlist.Count)
                {
                    arlColumnlist.Add(objCheckBoxfield);
                    CheckBoxColumnIndex = arlColumnlist.Count - 1;
                }
                else
                    arlColumnlist.Insert(CheckBoxColumnIndex, objCheckBoxfield);
            }
            catch (Exception)
            {
                throw;
            }

            // Return the new list
            return arlColumnlist;
        }

        // METHOD:: retrieve the style object based on the row state
        /// <summary>
        /// Gets the state of the row style from.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        protected virtual TableItemStyle GetRowStyleFromState(DataControlRowState state)
        {
            //the below switch case returns the style object based on the row state.
            try
            {
                switch (state)
                {
                    case DataControlRowState.Alternate:
                        return AlternatingRowStyle;
                    case DataControlRowState.Edit:
                        return EditRowStyle;
                    case DataControlRowState.Selected:
                        return SelectedRowStyle;
                    default:
                        return RowStyle;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion
    }
}

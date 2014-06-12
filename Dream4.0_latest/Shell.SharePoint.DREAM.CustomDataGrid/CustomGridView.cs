#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: CustomGridView.cs
#endregion
/// <summary> 
/// This is the Custom Grid view which is inherited from the GridView base Class
/// </summary>
using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Shell.SharePoint.DREAM.CustomDataGrid
{
    /// <summary>
    /// The CustomGridView Class
    /// </summary>
    public partial class CustomGridView : System.Web.UI.WebControls.GridView
    {
        #region Declaration        
        private ArrayList arlCachedSelectedIndices;        
        #endregion

        #region Properties
        // PROPERTY:: AutoGenerateCheckBoxColumn
        [Category("Behavior")]
        [Description("Whether a checkbox column is generated automatically at runtime")]
        [DefaultValue(false)]
        //[DefaultValue(true)]
        public bool AutoGenerateCheckBoxColumn
        {
            get
            {
                object objAutoCheckBoxColumn = ViewState["AutoGenerateCheckBoxColumn"];
                if (objAutoCheckBoxColumn == null)
                    return false;
                return (bool)objAutoCheckBoxColumn;
            }
            set { ViewState["AutoGenerateCheckBoxColumn"] = value; }
        }
        // PROPERTY:: CheckBoxColumnIndex
        [Category("Behavior")]
        [Description("Indicates the 0-based position of the checkbox column")]
        [DefaultValue(0)]
        public int CheckBoxColumnIndex
        {
            get
            {
                object objCheckBoxColumnIndex = ViewState["CheckBoxColumnIndex"];
                //the below condition check the viewstate for CheckBoxColumnIndex
                if (objCheckBoxColumnIndex == null) return 0;
                return (int)objCheckBoxColumnIndex;
            }
            set
            {
                ViewState["CheckBoxColumnIndex"] = (value < 0 ? 0 : value);
            }
        }
        // PROPERTY:: SelectedIndices
        /// <summary>
        /// Gets the selected indices.
        /// </summary>
        /// <value>The selected indices.</value>
        internal virtual ArrayList SelectedIndices
        {
            get
            {
                try
                {
                    arlCachedSelectedIndices = new ArrayList();
                    for (int intIndex = 0; intIndex < Rows.Count; intIndex++)
                    {
                        // Retrieve the reference to the checkbox
                        CheckBox chbCheckBox = (CheckBox)Rows[intIndex].FindControl(InputCheckBoxField.CHECKBOXID);
                        if (chbCheckBox == null)
                            return arlCachedSelectedIndices;
                        if (chbCheckBox.Checked)
                            arlCachedSelectedIndices.Add(intIndex);
                    }
                    return arlCachedSelectedIndices;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        #endregion
        // METHOD:: GetSelectedIndices
        /// <summary>
        /// Gets the selected indices.
        /// </summary>
        /// <returns></returns>
        public virtual int[] GetSelectedIndices()
        {
            //This will return the array of selected indices.
            return (int[])SelectedIndices.ToArray(typeof(int));
        }       

        #region Members overrides
        // METHOD:: CreateColumns
        /// <summary>
        /// Creates the set of column fields used to build the control hierarchy.
        /// </summary>
        /// <param name="dataSource">A <see cref="T:System.Web.UI.WebControls.PagedDataSource"></see> that represents the data source.</param>
        /// <param name="useDataSource">true to use the data source specified by the dataSource parameter; otherwise, false.</param>
        /// <returns>
        /// A <see cref="T:System.Collections.ICollection"></see> that contains the fields used to build the control hierarchy.
        /// </returns>
        protected override ICollection CreateColumns(PagedDataSource dataSource, bool useDataSource)
        {
            try
            {
                ICollection columnList = base.CreateColumns(dataSource, useDataSource);
                if (!AutoGenerateCheckBoxColumn)
                    return columnList;

                // Add a checkbox column if required
                ArrayList arlExtendedColumnList = AddCheckBoxColumn(columnList);
                return arlExtendedColumnList;
            }
            catch (Exception)
            {
                throw;
            }
        }        
        // METHOD:: OnPreRender
        protected override void OnPreRender(EventArgs e)
        {
            // Do as usual
            base.OnPreRender(e);
        }
        #endregion
    }
}

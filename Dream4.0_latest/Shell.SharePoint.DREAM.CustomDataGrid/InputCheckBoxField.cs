#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: InputCheckBoxField.cs
#endregion
/// <summary> 
/// This is the CheckBox Field generator Class for GridView.
/// </summary>
using System;
using System.Web.UI.WebControls;

namespace Shell.SharePoint.DREAM.CustomDataGrid
{
    /// <summary>
    /// The InputCheckBoxField class, it cannot be inherited.
    /// </summary>
    internal sealed class InputCheckBoxField : CheckBoxField
    {
        #region Declaration
        public const string CHECKBOXID = "CheckBoxButton";
        #endregion
        #region OverriddenMethod
        /// <summary>
        /// Initializes the specified <see cref="T:System.Web.UI.WebControls.DataControlFieldCell"></see> object to the specified row state.
        /// </summary>
        /// <param name="cell">The <see cref="T:System.Web.UI.WebControls.DataControlFieldCell"></see> to initialize.</param>
        /// <param name="rowState">One of the <see cref="T:System.Web.UI.WebControls.DataControlRowState"></see> values.</param>
        protected override void InitializeDataCell(DataControlFieldCell cell, DataControlRowState rowState)
        {
            try
            {
                base.InitializeDataCell(cell, rowState);

                // Adds a checkbox anyway, if not done already
                if (cell.Controls.Count == 0)
                {
                    CheckBox chkBox = new CheckBox();
                    chkBox.ID = InputCheckBoxField.CHECKBOXID;
                    chkBox.Attributes.Add("onclick", "DeSelectCheck('HeaderButton')");
                    chkBox.EnableViewState = true;
                    cell.Controls.Add(chkBox);
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
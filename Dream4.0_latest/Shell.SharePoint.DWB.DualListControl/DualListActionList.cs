#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: DualListDesigner.cs
#endregion

using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.Design;
using System.ComponentModel.Design;


namespace Shell.SharePoint.DWB.DualListControl
{
    /// <summary>
    /// Partial Class for Dual List control Design details
    /// </summary>
    internal partial class DualListDesigner
    {
        /// <summary>
        /// Partial Class for Dual List control Design details
        /// </summary>
        private class DualListActionList : DesignerActionList
        {
            #region Ctor

            public DualListActionList(DualListDesigner designer)
                : base(designer.Component)
            {
                _designer = designer;
            }

            #endregion

            #region Action Backing Methods

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public void EditLeftItems()
            {
                this._designer.EditLeftItems();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public void EditRightItems()
            {
                this._designer.EditRightItems();
            }

            #endregion

            #region Action Backing Properties

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public Boolean EnableMoveAll
            {
                get
                {
                    return ((DualList)this._designer.Component).EnableMoveAll;
                }
                set
                {
                    PropertyDescriptor property = TypeDescriptor.GetProperties(this._designer.Component)["EnableMoveAll"];
                    property.SetValue(this._designer.Component, value);
                }
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public Boolean EnableMoveUpDown
            {
                get
                {
                    return ((DualList)this._designer.Component).EnableMoveUpDown;
                }
                set
                {
                    PropertyDescriptor property = TypeDescriptor.GetProperties(this._designer.Component)["EnableMoveUpDown"];
                    property.SetValue(this._designer.Component, value);
                }
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public Boolean ExcludeItemInRightBox
            {
                get
                {
                    return ((DualList)this._designer.Component).ExcludeItemInRightBox;
                }
                set
                {
                    PropertyDescriptor property = TypeDescriptor.GetProperties(this._designer.Component)["ExcludeItemInRightBox"];
                    property.SetValue(this._designer.Component, value);
                }
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public Boolean ExcludeBasedOnValue
            {
                get
                {
                    return ((DualList)this._designer.Component).ExcludeBasedOnValue;
                }
                set
                {
                    PropertyDescriptor property = TypeDescriptor.GetProperties(this._designer.Component)["ExcludeBasedOnValue"];
                    property.SetValue(this._designer.Component, value);
                }
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public Boolean ExcludeBasedOnText
            {
                get
                {
                    return ((DualList)this._designer.Component).ExcludeBasedOnText;
                }
                set
                {
                    PropertyDescriptor property = TypeDescriptor.GetProperties(this._designer.Component)["ExcludeBasedOnText"];
                    property.SetValue(this._designer.Component, value);
                }
            }

            #endregion

            public override DesignerActionItemCollection GetSortedActionItems()
            {
                DesignerActionItemCollection actions = new DesignerActionItemCollection();
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this._designer.Component);
                PropertyDescriptor actionableProperty;

                actionableProperty = properties["LeftItems"];
                if (actionableProperty != null && actionableProperty.IsBrowsable)
                {
                    DesignerActionMethodItem editLeftItemsAction = new DesignerActionMethodItem(this, "EditLeftItems", "Edit Left Items...", "LeftData", "Edit the items visible in the left list");
                    actions.Add(editLeftItemsAction);
                }

                actionableProperty = properties["RightItems"];
                if (actionableProperty != null && actionableProperty.IsBrowsable)
                {
                    actions.Add(new DesignerActionMethodItem(this, "EditRightItems", "Edit Right Items...", "RightData", "Edit the items visible in the right list"));
                }

                DesignerActionHeaderItem behaviorHeader = new DesignerActionHeaderItem("Behavior", "Behavior");
                actions.Add(behaviorHeader);

                actionableProperty = properties["EnableMoveAll"];
                if ((actionableProperty != null) && actionableProperty.IsBrowsable)
                {
                    actions.Add(new DesignerActionPropertyItem("EnableMoveAll", "Toggle EnableMoveAll", "Behavior", "Toggles the visibility of the buttons which move all items"));
                }

                actionableProperty = properties["EnableMoveUpDown"];
                if ((actionableProperty != null) && actionableProperty.IsBrowsable)
                {
                    actions.Add(new DesignerActionPropertyItem("EnableMoveUpDown", "Toggle EnableMoveUpDown", "Behavior", "Toggles the visibility of the buttons which move chosen items up and down"));
                }

                return actions;
            }

            DualListDesigner _designer;
        }
    }
}

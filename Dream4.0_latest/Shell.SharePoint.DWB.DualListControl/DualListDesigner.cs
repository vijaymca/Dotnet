#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: DualListDesigner.cs
#endregion

using System;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;
using System.Web.UI.WebControls;
using System.ComponentModel.Design;
using System.ComponentModel;



namespace Shell.SharePoint.DWB.DualListControl
{
    /// <summary>
    /// Partial Class for Dual List control Design details
    /// </summary>
    internal partial class DualListDesigner : ControlDesigner
    {
        //ResourceManager Resources1 = ResourceManager.CreateFileBasedResourceManager("Resources", @"C:\Documents and Settings\Administrator\My Documents\Visual Studio 2005\Projects\DynamicListBox\Design", null);
        #region Smart Tag
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                DesignerActionListCollection actions = new DesignerActionListCollection();
                actions.AddRange(base.ActionLists);
                actions.Add(new DualListActionList(this));
                return actions;
            }
        }

        internal void EditLeftItems()
        {
            PropertyDescriptor propertyToEdit = TypeDescriptor.GetProperties(this.Component)["LeftItems"];
            ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.EditItemsCallback), propertyToEdit, "Edit the items visible in the left list", propertyToEdit);
        }

        internal void EditRightItems()
        {
            PropertyDescriptor propertyToEdit = TypeDescriptor.GetProperties(this.Component)["RightItems"];
            ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.EditItemsCallback), propertyToEdit, "Edit the items visible in the right list", propertyToEdit);
        }

        private bool EditItemsCallback(object context)
        {
            ListItemsCollectionEditor itemsEditor = new ListItemsCollectionEditor(typeof(ListItemCollection));

            IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
            PropertyDescriptor editedProperty = (PropertyDescriptor)context;

            ITypeDescriptorContext typeDescriptorContext = new TypeDescriptorContext(designerHost, editedProperty, base.Component);


            IServiceProvider serviceProvider = new WindowsFormsEditorService(this);

            itemsEditor.EditValue(typeDescriptorContext, serviceProvider, editedProperty.GetValue(base.Component));
            return true;
        }

        #endregion

        public override string GetDesignTimeHtml()
        {
            this.CreateChildControls();
            return base.GetDesignTimeHtml();
        }

        protected virtual void CreateChildControls()
        {
            ICompositeControlDesignerAccessor designerAccessor = (ICompositeControlDesignerAccessor)base.ViewControl;
            designerAccessor.RecreateChildControls();

            this.ViewControlCreated = false;

            DataBindLists();
        }

        protected override bool UsePreviewControl
        {
            get
            {
                return true;
            }
        }

        private void DataBindLists()
        {
            DualList view = this.ViewControl as DualList;
            if (view != null)
            {
                ApplyListDataSource(view.leftBox);
                ApplyListDataSource(view.rightBox);
            }
        }

        private static void ApplyListDataSource(System.Web.UI.WebControls.ListBox viewList)
        {
            Boolean isDataBound = (viewList.DataSource != null || !String.IsNullOrEmpty(viewList.DataSourceID));
            if (viewList.Items.Count == 0 || isDataBound)
            {
                if (isDataBound)
                {
                    viewList.Items.Clear();
                    viewList.Items.Add("bound");
                }
                else
                {
                    viewList.Items.Add("Unbound");
                }
            }
        }

    }
}

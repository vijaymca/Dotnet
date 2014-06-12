#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: TypeDescriptorContext.cs
#endregion

using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace Shell.SharePoint.DWB.DualListControl
{
    /// <summary>
    /// DualListDesigner Partial Class.
    /// </summary>
    internal partial class DualListDesigner
    {

        /// <summary>
        /// The ListItemsCollectionEditor used in the DualListDesigner's Smart Tag requires an implementation of ITypeDescriptorContext.
        /// Unfortunately all the built in versions are marked internal, so one must be trivally implemented.
        /// </summary>
        protected internal sealed class TypeDescriptorContext : ITypeDescriptorContext, IServiceProvider
        {

            public TypeDescriptorContext(IDesignerHost designerHost, PropertyDescriptor propDesc, object instance)
            {
                this._designerHost = designerHost;
                this._propDesc = propDesc;
                this._instance = instance;
            }

            #region IServiceProvider

            public object GetService(Type serviceType)
            {
                return this._designerHost.GetService(serviceType);
            }

            #endregion

            #region ITypeDescriptorContext

            public void OnComponentChanged()
            {
                if (this.ComponentChangeService != null)
                {
                    this.ComponentChangeService.OnComponentChanged(this._instance, this._propDesc, null, null);
                }
            }

            public Boolean OnComponentChanging()
            {
                if (this.ComponentChangeService != null)
                {
                    try
                    {
                        this.ComponentChangeService.OnComponentChanging(this._instance, this._propDesc);
                    }
                    catch (CheckoutException checkoutException)
                    {
                        if (checkoutException != CheckoutException.Canceled)
                        {
                            throw;
                        }
                        return false;
                    }
                }
                return true;
            }

            public IContainer Container
            {
                get
                {
                    return (IContainer)this._designerHost.GetService(typeof(IContainer));
                }
            }

            public Object Instance
            {
                get
                {
                    return this._instance;
                }
            }

            public PropertyDescriptor PropertyDescriptor
            {
                get
                {
                    return this._propDesc;
                }
            }

            #endregion

            private IComponentChangeService ComponentChangeService
            {
                get
                {
                    return (IComponentChangeService)this._designerHost.GetService(typeof(IComponentChangeService));
                }
            }

            private IDesignerHost _designerHost;
            private Object _instance;
            private PropertyDescriptor _propDesc;

        }
    }
}
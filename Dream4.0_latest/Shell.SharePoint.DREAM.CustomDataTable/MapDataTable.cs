#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: MapCustomDataTable.cs 
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Shell.SharePoint.DREAM.CustomDataTable
{
    /// <summary>
    /// Holds properties for custom data table
    /// </summary>
    public class MapCustomDataTable : DataTable 
    {
        private bool blnIsMaximum;          // To check maximum value
        private string strRequiredField;    // To store required field value
        private string strAssetType;
        private int intContextKey;
        #region "DataTable Properties"
        /// <summary>
        /// Gets or sets a value indicating whether this instance is maximum.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is maximum; otherwise, <c>false</c>.
        /// </value>
        public bool IsMaximum
        {
            get
            {
                return blnIsMaximum;
            }
            set
            {
                blnIsMaximum = value;
            }
        }
        /// <summary>
        /// Gets or sets the required field.
        /// </summary>
        /// <value>The required field.</value>
        public string RequiredField
        {
            get
            {
                return strRequiredField;
            }
            set
            {
                strRequiredField = value;
            }
        }
        /// <summary>
        /// Gets or sets the type of the asset.
        /// </summary>
        /// <value>The type of the asset.</value>
        public string AssetType
        {
            get
            {
                return strAssetType;
            }
            set
            {
                strAssetType = value;
            }
        }

        /// <summary>
        /// Gets or sets the context key.
        /// </summary>
        /// <value>The context key.</value>
        public int ContextKey
        {
            get
            {
                return intContextKey;
            }
            set
            {
                intContextKey = value;
            }
        }
        #endregion
    }
}

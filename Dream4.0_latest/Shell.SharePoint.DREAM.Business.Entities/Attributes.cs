#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: Attributes.cs 
#endregion

/// <summary> 
/// This is Attributes class. This class is used to create attribute object for 
/// generating request info object
/// </summary> 
using System.Collections;


namespace Shell.SharePoint.DREAM.Business.Entities
{
    /// <summary>
    /// Attributes Class
    /// </summary>
    public class Attributes
    {
        #region DECLARATION
        private string strName = string.Empty;
        private string strOperator = string.Empty;
        private string strLabel = string.Empty;
        private string strChecked = string.Empty;
        private string strType = string.Empty;
        private bool blnIsRangeApplicable = false;
            
        //Added for DWB Chapter.
        private string strDisplayField = string.Empty;

        private ArrayList arlValue = null;
        private ArrayList arlParameter = null;
        #endregion
        #region PROPERTIES

        /// <summary>
        /// Gets or sets the Display Field.
        /// </summary>
        /// <value>The name.</value>
        public string DisplayField
        {
            get
            {
                return strDisplayField;
            }
            set
            {
                strDisplayField = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return strName; 
            }
            set
            {
                strName = value; 
            }
        }

        /// <summary>
        /// Gets or sets the checked value.
        /// </summary>
        /// <value>The checked value</value>
        public string Checked
        {
            get
            {
                return strChecked;
            }
            set
            {
                strChecked = value;
            }
        }

        /// <summary>
        /// Gets or sets the Range Applicable.
        /// </summary>
        /// <value>The checked value</value>
        public bool IsRangeApplicable
        {
            get
            {
                return blnIsRangeApplicable;
            }
            set
            {
                blnIsRangeApplicable = value;
            }

        }

        /// <summary>
        /// Gets or sets the Label.
        /// </summary>
        /// <value>The Label.</value>
        public string Label
        {
            get
            {
                return strLabel;
            }
            set
            {
                strLabel = value;
            }
        }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>The operator.</value>
        public string Operator
        {
            get
            {
                return strOperator; 
            }
            set
            {
                strOperator = value; 
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public ArrayList Value
        {
            get
            {
                return arlValue; 
            }
            set
            {
                arlValue = value; 
            }
        }

        /// <summary>
        /// Gets or sets the parameter.
        /// </summary>
        /// <value>The parameter.</value>
        public ArrayList Parameter
        {
            get
            {
                return arlParameter;
            }
            set
            {
                arlParameter = value;
            }
        }
        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        /// <value>The name.</value>
        public string Type
        {
            get
            {
                return strType;
            }
            set
            {
                strType = value;
            }
        }
        #endregion
    }
}

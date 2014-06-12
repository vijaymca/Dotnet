#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: AttributeGroup.cs 
#endregion
/// <summary> 
/// This is AttributeGroup class. This class is used to create attribute group object for 
/// generating request info object
/// </summary>
using System.Collections;


namespace Shell.SharePoint.DREAM.Business.Entities
{
    /// <summary>
    /// AttributeGroup Class
    /// </summary>
    public class AttributeGroup
    {
        #region Declaration
        private string strOperator = string.Empty;
        private string strName = string.Empty;
        private string strLabel = string.Empty;
        private string strChecked = string.Empty;
        private ArrayList arlAttribute = null;
        private ArrayList arlAttributeGroup = null;
        #endregion
        #region PROPERTIES
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
        /// Gets or sets the Name.
        /// </summary>
        /// <value>The Name.</value>
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
        /// Gets or sets the Checked Value.
        /// </summary>
        /// <value>The Checked Value.</value>
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
        /// Gets or sets the attribute.
        /// </summary>
        /// <value>The attribute.</value>
        public ArrayList Attribute
        {
            get
            {
                return arlAttribute;
            }
            set
            {
                arlAttribute = value;
            }
        }

        /// <summary>
        /// Gets or sets the attribute groups.
        /// </summary>
        /// <value>The attribute groups.</value>
        public ArrayList AttributeGroups
        {
            get
            {
                return arlAttributeGroup;
            }
            set
            {
                arlAttributeGroup = value;
            }
        }
        #endregion
    }
}

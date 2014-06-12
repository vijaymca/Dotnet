#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: Entity.cs 
#endregion

/// <summary> 
/// This class is used to create an entity object for generating request info object
/// </summary> 
using System.Collections;

namespace Shell.SharePoint.DREAM.Business.Entities
{
    /// <summary>
    /// Entity Class.
    /// </summary>
    public class Entity
    {
        #region DECLARATION
        string strRequestId = string.Empty;
        string strSessionId = string.Empty;
        private SkipInfo objSkipInfo = null;        
        private ArrayList arlAttribute = null;
        private ArrayList arlAttributeGroup = null;
        private Display objDisplay = null;
        private Query objQuery = null;
        private Criteria objCriteria = null;
        private string strResponseType = string.Empty;
        string strName = string.Empty;        
        string strDataSource = string.Empty;
        string strProjectName = string.Empty;
        string strDataProvider = string.Empty;
        string strType = string.Empty;
        private bool blnProperty = true;
        private bool blnTVDSS = false;
        #endregion
        #region PROPERTIES

        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        /// <value>The entities.</value>
        public SkipInfo SkipInfo
        {
            get
            {
                return objSkipInfo;
            }
            set
            {
                objSkipInfo = value;
            }
        }

        /// <summary>
        /// Gets or sets the report id.
        /// </summary>
        /// <value>The level.</value>
        public string ResponseType
        {
            get { return strResponseType; }
            set { strResponseType = value; }
        }


        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Entity"/> is property.
        /// </summary>
        /// <value><c>true</c> if property; otherwise, <c>false</c>.</value>
        public bool Property
        {
            get { return blnProperty; }
            set { blnProperty = value; }
        }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Entity"/> is TVDSS.
        /// </summary>
        /// <value><c>true</c> if TVDSS; otherwise, <c>false</c>.</value>
        public bool TVDSS
        {
            get { return blnTVDSS; }
            set { blnTVDSS = value; }
        }

        /// <summary>
        /// Gets or sets the report id.
        /// </summary>
        /// <value>The level.</value>
        public string RequestID
        {
            get { return strRequestId; }
            set { strRequestId = value; }
        }
        /// <summary>
        /// Gets or sets the report id.
        /// </summary>
        /// <value>The level.</value>
        public string SessionID
        {
            get { return strSessionId ; }
            set { strSessionId = value; }
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

        /// <summary>
        /// Gets or sets the criteria.
        /// </summary>
        /// <value>The criteria.</value>
        public Criteria Criteria
        {
            get
            { 
                return objCriteria; 
            }
            set 
            { 
                objCriteria = value; 
            }
        }
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        /// <value>The criteria.</value>
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
        /// Gets or sets the Data Source.
        /// </summary>
        /// <value>The criteria.</value>
        public string DataSource
        {
            get
            {
                return strDataSource;
            }
            set
            {
                strDataSource = value;
            }
        }
        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        /// <value>The name of the project.</value>
        public string ProjectName
        {
            get
            {
                return strProjectName;
            }
            set
            {
                strProjectName = value;
            }
        }
        /// <summary>
        /// Gets or sets the Data Provider.
        /// </summary>
        /// <value>The criteria.</value>
        public string DataProvider
        {
            get
            {
                return strDataProvider;
            }
            set
            {
                strDataProvider = value;
            }
        }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
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
        /// <summary>
        /// Gets or sets the display attributes.
        /// </summary>
        /// <value>The display attributes.</value>
        public Display Display
        {
            get
            {
                return objDisplay;
            }
            set
            {
                objDisplay = value;
            }
        }

        /// <summary>
        /// Gets or sets the Query used for searching in the Query Builder
        /// </summary>
        public Query Query
        {
            get
            {
                return objQuery;
            }
            set
            {
                objQuery = value;
            }
        }

        #endregion
    }
}
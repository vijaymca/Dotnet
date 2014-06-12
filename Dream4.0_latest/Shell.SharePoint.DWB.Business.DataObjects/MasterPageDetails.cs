#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: MasterPageDetails.cs 
#endregion


namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// Master Page Detail class
    /// </summary>
    public class MasterPageDetails
    {
        #region DECLARATION
        string strName = string.Empty;
        string strTemplateTitle = string.Empty;
        int intPageSequence;
        string strAssetType = string.Empty;
        string strAssetTypeText = string.Empty;
        string  strSignOffDiscipline = string.Empty;
        string strSignOffDisciplineText = string.Empty;
        string strSOP = string.Empty;
        string strConnectionType = string.Empty;
        string strConnectionTypeText = string.Empty;
        string strTerminated = string.Empty;
        int intRowId;
        string strPageURL = string.Empty;
        string strTemplates = string.Empty;
        string strPageOwner = string.Empty;
        string strMasterPageID = string.Empty;
        
        #endregion
        #region PROPERTIES

        /// <summary>
        /// Row Id.
        /// </summary>
        public int RowId
        {
            get { return intRowId; }
            set { intRowId = value; }
        }

        /// <summary>
        /// Master Page Name.
        /// </summary>
        public string Name
        {
            get { return strName;}
            set { strName = value; }
        }

        /// <summary>
        /// Master Page Template Title.
        /// </summary>
        public string TemplateTitle
        {
            get { return strTemplateTitle; }
            set { strTemplateTitle = value; }
        }

        /// <summary>
        /// Page Sequence.
        /// </summary>
        public int PageSequence
        {
            get { return intPageSequence; }
            set { intPageSequence = value; }
        }

        /// <summary>
        /// Page Owner.
        /// </summary>
        public string  PageOwner
        {
            get { return strPageOwner; }
            set { strPageOwner = value; }
        }
        
        /// <summary>
        /// Asset Type ID of the Master Page.
        /// </summary>
        public string AssetType
        {
            get { return strAssetType; }
            set { strAssetType = value; }
        }

        /// <summary>
        /// Asset Type Text of the Master Page.
        /// </summary>
        public string AssetTypeText
        {
            get { return strAssetTypeText; }
            set { strAssetTypeText = value; }
        }

        /// <summary>
        /// Sign Off Discipline ID.
        /// </summary>
        public string SignOffDiscipline
        {
            get { return strSignOffDiscipline; }
            set { strSignOffDiscipline = value; }
        }

        /// <summary>
        /// Sign Off Discipline Text.
        /// </summary>
        public string SignOffDisciplineText
        {
            get { return strSignOffDisciplineText; }
            set { strSignOffDisciplineText = value; }
        }

        /// <summary>
        /// Standard Operating Procedure.
        /// </summary>
        public string SOP
        {
            get { return strSOP; }
            set { strSOP = value; }
        }

        /// <summary>
        /// ConnectionType ID.
        /// </summary>
        public string ConnectionType
        {
            get { return strConnectionType; }
            set { strConnectionType = value; }
        }

        /// <summary>
        /// ConnectionType Text.
        /// </summary>
        public string ConnectionTypeText
        {
            get { return strConnectionTypeText; }
            set { strConnectionTypeText = value; }
        }

        /// <summary>
        /// Terminated.
        /// </summary>
        public string Terminated
        {
            get { return strTerminated; }
            set { strTerminated = value; }
        }

        /// <summary>
        /// Master Page URL.
        /// </summary>
        public string PageURL
        {
            get { return strPageURL; }
            set { strPageURL = value; }
        }

        /// <summary>
        /// Master Page Template IDs separated by semicolen(;).
        /// </summary>
        public string Templates
        {
            get { return strTemplates; }
            set { strTemplates = value; }
        }

        /// <summary>
        /// Gets or sets the master page ID.
        /// </summary>
        /// <value>The master page ID.</value>
        public string MasterPageID
        {
            get { return strMasterPageID; }
            set { strMasterPageID = value; }
        }
        #endregion
    }
}

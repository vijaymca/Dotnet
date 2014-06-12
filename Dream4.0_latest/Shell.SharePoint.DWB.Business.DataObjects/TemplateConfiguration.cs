#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: TemplateConfiguration.cs 
#endregion

using System;
namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// Gets/Sets the properties of Template Master Pages
    /// </summary>
    /// 
    [Serializable]
    public class TemplateConfiguration 
    {
        #region DECLARATION

        string strLinkedMasterPageId = string.Empty;
        string strmasterPageTitle = string.Empty;
        string strTemplateTitle = string.Empty;
        int intPageSequence;
        int intRowId;
        string strassetType = string.Empty;
        string strdiscipline = string.Empty;
        string strpageOwner = string.Empty;
        string strpageURL = string.Empty;
        string strStdOperatingProcedure = string.Empty;
        string strtemplateID = string.Empty;
        string strconnectionType = string.Empty;
        #endregion


        #region PROPERTIES

        /// <summary>
        /// Gets/Sets Template Master Page ID.
        /// </summary>
        public int RowId
        {
            get { return intRowId; }
            set { intRowId = value; }
        }

        /// <summary>
        /// Gets/Sets Template Master Page Sequence
        /// </summary>      
        public int PageSequence
        {
            get { return intPageSequence; }
            set { intPageSequence = value; }
        }

        /// <summary>
        /// Gets/Sets Template Title.
        /// </summary>
        public string TemplateTitle
        {
            get { return strTemplateTitle; }
            set { strTemplateTitle = value; }
        }

        /// <summary>
        /// Gets/Sets Template Master Page Title.
        /// </summary>
        public string MasterPageTitle
        {
            get { return strmasterPageTitle; }
            set { strmasterPageTitle = value; }
        }

        /// <summary>
        /// Gets/Sets Linked Master Page ID.
        /// </summary>
        public string LinkedMasterPageId
        {
            get { return strLinkedMasterPageId; }
            set { strLinkedMasterPageId = value; }
        }

        /// <summary>
        /// Gets/Sets Master Page Asset Type.
        /// </summary>
        public string AssetType
        {
            get { return strassetType; }
            set { strassetType = value; }
        }

        /// <summary>
        /// Gets/Sets Master Page Discipline.
        /// </summary>
        public string Discipline
        {
            get { return strdiscipline; }
            set { strdiscipline = value; }
        }

        /// <summary>
        /// Gets/Sets Master Page Owner.
        /// </summary>
        public string PageOwner
        {
            get { return strpageOwner; }
            set { strpageOwner = value; }
        }
        /// <summary>
        /// Gets/Sets  Master Page URL.
        /// </summary>
        public string PageURL
        {
            get { return strpageURL; }
            set { strpageURL = value; }
        }

        /// <summary>
        /// Gets/Sets  Master Page Standard Operating Procedure.
        /// </summary>
        public string StandardOperatingProcedure
        {
            get { return strStdOperatingProcedure; }
            set { strStdOperatingProcedure = value; }
        }

        /// <summary>
        /// Gets/Sets Template ID.
        /// </summary>
        public string TemplateID
        {
            get { return strtemplateID; }
            set { strtemplateID = value; }
        }

        /// <summary>
        /// Gets/Sets  Master Page Connection Type.
        /// </summary>
        public string ConnectionType
        {
            get { return strconnectionType; }
            set { strconnectionType = value; }
        }
       
        #endregion        
    }
}

#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: MasterPageBLL.cs
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using Shell.SharePoint.DWB.Business.DataObjects;
using Shell.SharePoint.DWB.DataAccessLayer;
using System.Data;

namespace Shell.SharePoint.DWB.BusinessLogicLayer
{
    /// <summary>
    /// BLL class for Master Page Screen methods
    /// </summary>
    public class MasterPageBLL
    {
        #region Declarations
        MasterDAL objMasterDAL;
        CommonDAL objCommonDAL;
        private const string AUDIT_ACTION_CREATION = "1";
        const string TEMPLATEPAGESMAPPINGLIST = "DWB Template Page Mapping";
        const string TEMPLATEPAGESMAPPINGAUDITLIST = "DWB Chapter Pages Mapping Audit Trail";

        #endregion

        #region Public Methods
        /// <summary>
        /// Updates the list entry.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <param name="listEntry">The list entry.</param>
        /// <param name="auditListName">Name of the audit list.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="actionPerformed">The action performed.</param>
        public void UpdateListEntry(string siteURL, ListEntry listEntry, string auditListName, string listName, string userName, string actionPerformed)
        {
            objMasterDAL = new MasterDAL();
            objCommonDAL = new CommonDAL();
            string[] strTemplateIds = null;
            string strTemplatemappingRowId = string.Empty;
            objMasterDAL.UpdateListEntry(siteURL, listEntry, listName, actionPerformed);
            if (listEntry.MasterPage.RowId > 0)
            {
                objMasterDAL.UpdateListAuditHistory(siteURL, listName, auditListName, listEntry.MasterPage.RowId, listEntry.MasterPage.Name,
                    userName, actionPerformed);
            }

            if (!string.IsNullOrEmpty(listEntry.MasterPage.Templates) && actionPerformed.Contains(AUDIT_ACTION_CREATION))
            {
                strTemplatemappingRowId = objMasterDAL.AddTemplateMasterPageMapping(siteURL, listEntry, TEMPLATEPAGESMAPPINGLIST);
                strTemplateIds = strTemplatemappingRowId.Split(';');
                if (strTemplateIds != null)
                {
                    for (int intIndex = 0; intIndex < strTemplateIds.Length - 1; intIndex++)
                    {
                        int intRowId = 0;
                        int.TryParse(strTemplateIds[intIndex], out intRowId);
                        objCommonDAL.UpdateListTemplateMappingAuditHistory(siteURL, TEMPLATEPAGESMAPPINGAUDITLIST, intRowId, userName, AUDIT_ACTION_CREATION);

                    }
                }
            }
        }

        /// <summary>
        /// Updates the Page sequence
        /// </summary>
        /// <param name="parentSiteUrl">The parent site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="strAuditListName">Name of the STR audit list.</param>
        /// <param name="dvUpdateListitems">The dv update listitems.</param>
        /// <param name="actionPerformed">The action performed.</param>
        public void UpdatepageSequence(string parentSiteUrl, string listName, string strAuditListName,
            DataView dvUpdateListitems, string actionPerformed)
        {
            objCommonDAL = new CommonDAL();
            objMasterDAL = new MasterDAL();
            objCommonDAL.UpdateSequence(parentSiteUrl, listName, dvUpdateListitems, "Page_Sequence");
            if (dvUpdateListitems != null && dvUpdateListitems.Count > 0)
            {
                for (int intIndex = 0; intIndex < dvUpdateListitems.Count; intIndex++)
                {
                    objMasterDAL.UpdateListAuditHistory(parentSiteUrl, listName, strAuditListName,
                        int.Parse(dvUpdateListitems[intIndex]["ID"].ToString()), string.Empty, Environment.UserName, actionPerformed);
                }
            }
        }

        /// <summary>
        /// Set the Master Page Details
        /// </summary>
        /// <param name="siteURL">Site URL.</param>
        /// <param name="listName">List Name.</param>
        /// <param name="queryString">CAML Query.</param>
        /// <returns>object of ListEntry class</returns>
        public ListEntry SetMasterPageDetail(string siteURL, string listName, string queryString)
        {
            objMasterDAL = new MasterDAL();
            return (objMasterDAL.GetMasterPageDetail(siteURL, listName, queryString));
        }
        #endregion
    }
}

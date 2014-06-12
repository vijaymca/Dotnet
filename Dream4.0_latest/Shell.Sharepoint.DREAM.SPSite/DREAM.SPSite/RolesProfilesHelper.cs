#region Shell Copyright.2011
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: RolesProfilesHelper
#endregion
/// <summary> 
/// This is roles profiles helper class.
/// </summary>
using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;
using Microsoft.SharePoint;
using Telerik.Web.UI;

namespace Shell.SharePoint.DREAM.Site.UI
{
    /// <summary>
    /// RolesProfilesHelper class contains all method releated to roles profiles functionality.
    /// </summary>
    public class RolesProfilesHelper
    {
        #region DECLARATION
        AbstractController objMossController;
        AbstractController objReportController;
        const string MOSSSERVICE = "MossService";
        const string REPORTSERVICE = "ReportService";
        ServiceProvider objFactory;
        const string TABULAR = "Tabular";
        const string ASSETSEARCHMAPPINGLIST = "Asset Search Name Mapping";
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetTreeHelper"/> class.
        /// </summary>
        /// <param name="assetTree">The roles profiles.</param>
        public RolesProfilesHelper()
        {
            objFactory = new ServiceProvider();
            objMossController = objFactory.GetServiceManager(MOSSSERVICE);
            objReportController = objFactory.GetServiceManager(REPORTSERVICE);
        }
        #endregion

        #region Public method

        /// <summary>
        /// Loads the dropdown LST bx from SP list.
        /// </summary>
        /// <param name="dropdownLstBx">The dropdown LST bx.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="displayColName">Display name of the col.</param>
        /// <param name="valueColName">Name of the value col.</param>
        /// <param name="camlQuery">The caml query.</param>
        /// <param name="viewFields">The view fields.</param>
        public void LoadDropdownLstBxFromSPList(DropDownList dropdownLstBx, string listName, string displayColName, string valueColName, string camlQuery, string viewFields)
        {
            DataTable objDataTable = null;
            try
            {
                objDataTable = ((MOSSServiceManager)objMossController).ReadList(SPContext.Current.Site.Url, listName, camlQuery, viewFields);
                dropdownLstBx.DataSource = objDataTable;
                dropdownLstBx.DataTextField = displayColName;
                dropdownLstBx.DataValueField = valueColName;
                dropdownLstBx.DataBind();
                dropdownLstBx.Items.Insert(0, "---Select---");
            }
            finally
            {
                if(objDataTable != null)
                {
                    objDataTable.Dispose();
                }
            }
        }

        /// <summary>
        /// Loads the searche names.
        /// </summary>
        /// <param name="asset">The asset.</param>
        /// <param name="dropdownLstBx">The dropdown LST bx.</param>
        public void LoadSearcheNames(string asset, DropDownList dropdownLstBx)
        {
            const string SEARCHES = "Searches";
            const string SPLITSTRING = ";#";
            const string VALUE = "Value";
            const string SEARCHNAMEEXP = "%SEARCHNAME%";
            string strCmlQryAsstSrchMap = "<Where><Eq><FieldRef Name=\"Asset\" /><Value Type=\"Lookup\">" + asset + "</Value></Eq></Where>";
            string strViewFields = @"<FieldRef Name='Title'/><FieldRef Name='" + SEARCHES + "'/>";
            string strCmlQrySrchNames = "<Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">" + SEARCHNAMEEXP + "</Value></Eq></Where>";
            string strSearcheNames = string.Empty;
            string strSrchName = string.Empty;
            string strSrchValue = string.Empty;
            DataTable objDataTable = null;
            SortedList sortedLstSrchNames = new SortedList();
            try
            {
                objDataTable = ((MOSSServiceManager)objMossController).ReadList(SPContext.Current.Site.Url, ASSETSEARCHMAPPINGLIST, strCmlQryAsstSrchMap, strViewFields);
                if(objDataTable != null && objDataTable.Rows.Count > 0 && objDataTable.Rows[0][SEARCHES] != null)
                {
                    strSearcheNames = (string)objDataTable.Rows[0][SEARCHES];
                }
                if(!string.IsNullOrEmpty(strSearcheNames))
                {
                    string[] arrSearches = strSearcheNames.Split(SPLITSTRING.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    for(int intSrchNamesCounter = 0; intSrchNamesCounter < arrSearches.Length; intSrchNamesCounter += 2)
                    {
                        if(!string.IsNullOrEmpty(arrSearches[intSrchNamesCounter]))
                        {
                            strSrchName = arrSearches[intSrchNamesCounter];
                            strSrchValue = GetColValueFromDtRow(GetLookUpColumnRow(SEARCHES, strCmlQrySrchNames.Replace(SEARCHNAMEEXP,arrSearches[intSrchNamesCounter])), VALUE);
                            sortedLstSrchNames.Add(strSrchValue, strSrchName);
                        }
                    }
                }
                sortedLstSrchNames.TrimToSize();
                dropdownLstBx.DataSource = sortedLstSrchNames;
                dropdownLstBx.DataTextField = "value";
                dropdownLstBx.DataValueField = "key";
                dropdownLstBx.DataBind();
                dropdownLstBx.Items.Insert(0, "---Select---");
            }
            finally
            {
                if(objDataTable != null)
                {
                    objDataTable.Dispose();
                }
            }
        }

        /// <summary>
        /// Gets the col value from dt row.
        /// </summary>
        /// <param name="dtRow">The dt row.</param>
        /// <param name="colName">Name of the col.</param>
        /// <returns></returns>
        private string GetColValueFromDtRow(DataRow dtRow, string colName)
        {
            string strColValue = string.Empty;
            if(dtRow != null && dtRow[colName] != null)
                strColValue = (string)dtRow[colName];
            return strColValue;
        }
        /// <summary>
        /// Gets the look up column row.
        /// </summary>
        /// <param name="listName">Name of the list.</param>
        /// <param name="camlQuery">The caml query.</param>
        /// <param name="viewFields">The view fields.</param>
        /// <returns></returns>
        public DataRow GetLookUpColumnRow(string listName, string camlQuery)
        {
            DataTable objDataTable = null;
            DataRow objDataRow = null;
            try
            {
                objDataTable = ((MOSSServiceManager)objMossController).ReadList(SPContext.Current.Site.Url, listName, camlQuery);
                if(objDataTable != null && objDataTable.Rows.Count > 0)
                {
                    objDataRow = objDataTable.Rows[0];
                }
            }
            finally
            {
                if(objDataTable != null)
                {
                    objDataTable.Dispose();
                }
            }
            return objDataRow;
        }


        /// <summary>
        /// Creates the request info.
        /// </summary>
        /// <returns></returns>
        public RequestInfo CreateRequestInfo(string searchName)
        {
            RequestInfo objRequestInfo = new RequestInfo();
            objRequestInfo.Entity = new Entity();
            objRequestInfo.Entity.Name = searchName;// result type dropdown selected value
            objRequestInfo.Entity.ResponseType = TABULAR;
            objRequestInfo.Entity.Type = "allcolumns";
            objRequestInfo.Entity.Property = false;
            objRequestInfo.Entity.Criteria = new Criteria();
            objRequestInfo.Entity.Criteria.Name = "*";
            return objRequestInfo;
        }
        /// <summary>
        /// Gets the column order.
        /// </summary>
        /// <param name="radLstBxSource">The RAD LST bx source.</param>
        /// <param name="radLstBxDestination">The RAD LST bx destination.</param>
        /// <param name="nodeName">Name of the node.</param>
        /// <returns></returns>
        public string GetColumnOrder(RadListBox radLstBxSource, RadListBox radLstBxDestination, string nodeName)
        {
            string strColOrderStatus = string.Empty;
            strColOrderStatus = CreateColumnXMLString(radLstBxDestination, nodeName, "true");
            strColOrderStatus += CreateColumnXMLString(radLstBxSource, nodeName, "false");
            return strColOrderStatus;
        }
        #endregion

        #region Private method
        /// <summary>
        /// Creates the column XML string.
        /// </summary>
        /// <param name="radLstBx">The RAD LST bx.</param>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="strDiplayStatus">The STR diplay status.</param>
        /// <returns></returns>
        private static string CreateColumnXMLString(RadListBox radLstBx, string nodeName, string strDiplayStatus)
        {
            string strXml = string.Empty;
            for(int intCounter = 0; intCounter < radLstBx.Items.Count; intCounter++)
            {
                strXml += "<" + nodeName + " name=\"" + radLstBx.Items[intCounter].Value + "\" display=\"" + strDiplayStatus + "\"/>";
            }
            return strXml;
        }
        #endregion
    }
}


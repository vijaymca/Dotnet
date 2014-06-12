#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: MapExportAll.cs 
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using Shell.SharePoint.DREAM.MapProcess;
using Shell.SharePoint.DREAM.CustomDataTable;
using ESRI.ArcGIS.ADF.ArcGISServer;

namespace Shell.Sharepoint.WebParts.DREAM.ExportAll
{
    /// <summary>
    /// This class is used to query Map Search Results from Map Service through proxy class
    /// </summary>
    class MapExportAll
    {
        /// <summary>
        /// Gets the map results.
        /// </summary>
        /// <param name="currentPage">The current page.</param>
        /// <returns></returns>
        public MapCustomDataTable GetMapResults(Page currentPage)
        {
            #region Local Variables
            //getting session variables
            string strLayerName = string.Empty;
            string strWhereClause = string.Empty;
            string strAssetType = string.Empty;
            string strSearchType = string.Empty;
            int intMaxRecords = 0;
            MapCustomDataTable objDataTable = null;
            ESRI.ArcGIS.ADF.Web.Geometry.Geometry objGeometry = null;
            MapProcessManager objMapController = null;
            ESRI.ArcGIS.ADF.ArcGISServer.MapServerProxy objMapServer = null;
            #endregion
            try
            {
                #region Get Records
                object objAssetType = MapUtility.GetSessionVariable(currentPage, enumSessionVariable.AssetType.ToString());
                object objGeom = MapUtility.GetSessionVariable(currentPage, enumSessionVariable.geometry.ToString());
                object objSearchType = MapUtility.GetSessionVariable(currentPage, enumSessionVariable.searchType.ToString());
                object objMaxRecords = MapUtility.GetSessionVariable(currentPage, enumSessionVariable.maxRecordsCount.ToString());
                object objWhereClause = MapUtility.GetSessionVariable(currentPage, enumSessionVariable.whereClause.ToString());
                if (objSearchType != null)
                {
                    objMapServer = GetMapServer();
                    //validating and assigning the variables
                    objMapController = new MapProcessManager();
                    strSearchType = (string)objSearchType;
                    if (objGeom != null) objGeometry = (ESRI.ArcGIS.ADF.Web.Geometry.Geometry)objGeom;
                    if (objMaxRecords != null) intMaxRecords = (int)objMaxRecords;
                    if (objWhereClause != null) strWhereClause = (string)objWhereClause;
                    if (objAssetType != null)
                    {
                        strAssetType = (string)objAssetType;
                        if (!string.IsNullOrEmpty(strAssetType))
                        {
                            strLayerName = objMapController.GetLayerName(strAssetType);
                        }
                    }
                    if (string.Equals(strSearchType, MapBusinessConstants.SEARCHTYPEOTHER))
                    {
                        if (objGeometry != null && !string.IsNullOrEmpty(strLayerName))
                        {
                            objDataTable = GatMapSpatialQueryData(objMapServer,objGeometry, strLayerName, intMaxRecords, false, strAssetType);
                        }
                    }
                    else if (string.Equals(strSearchType, MapBusinessConstants.SEARCHTYPEQUICK))
                    {
                        if (!string.IsNullOrEmpty(strWhereClause) && !string.IsNullOrEmpty(strLayerName))
                        {
                            objDataTable = GetMapQueryData(objMapServer,strLayerName, strWhereClause, intMaxRecords, strAssetType);
                        }
                    }
                }
                    #endregion
            }
            catch (Exception ex)
            {
                MapUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
            finally
            {
                if (objDataTable != null) objDataTable.Dispose();
            }
            return objDataTable;
        }
        /// <summary>
        /// Gets the map query data.
        /// </summary>
        /// <param name="mapServer">The map server.</param>
        /// <param name="layerName">Name of the layer.</param>
        /// <param name="whereClause">The where clause.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="assetType">Type of the asset.</param>
        /// <returns></returns>
        private MapCustomDataTable GetMapQueryData(MapServerProxy mapServer, string layerName, 
            string whereClause,int maxRecords, string assetType)
        {
            #region "Local Variables"
            DataTable dtQSrch = null;
            MapCustomDataTable dtCustomQSrch = null;
            int intLayerID = -1;
            QueryFilter objQSFilter = new QueryFilter();
            MapProcessManager objMapController = null;
            ESRI.ArcGIS.ADF.StringCollection objFieldsColl = null;
            string strFields = string.Empty;
            ESRI.ArcGIS.ADF.ArcGISServer.RecordSet objRS = null;
            #endregion
            try
            {
                if (!string.IsNullOrEmpty(layerName))
                {
                    intLayerID = GetLayerID(mapServer, layerName);
                    if (intLayerID != -1)
                    {
                        //assigning Query Functionality
                        //Declaring Query Filter and assigning properties
                        objMapController = new MapProcessManager();
                        objFieldsColl = new ESRI.ArcGIS.ADF.StringCollection();
                        objQSFilter.WhereClause = whereClause;
                        objFieldsColl = objMapController.GetSubFields(layerName);
                        foreach (string strColumnName in objFieldsColl)
                        {
                            if (!string.IsNullOrEmpty(strFields))
                                strFields = strFields + "," + strColumnName;
                            else
                                strFields = strColumnName;
                        }
                        if (objFieldsColl != null && objFieldsColl.Count > 0) objQSFilter.SubFields = strFields;
                        //Querying for data Table.
                        objRS = mapServer.QueryFeatureData(mapServer.GetMapName(0), intLayerID, objQSFilter);
                        dtQSrch = ESRI.ArcGIS.ADF.Web.DataSources.ArcGISServer.Converter.ToDataTable(objRS);
                        if (dtQSrch != null && dtQSrch.Rows.Count > 0)
                        {
                            dtCustomQSrch = FormatDataTable(dtQSrch,layerName,maxRecords, assetType);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (dtQSrch != null) dtQSrch.Dispose();
                if (dtCustomQSrch != null) dtCustomQSrch.Dispose();
            }
            return dtCustomQSrch;
        }
        /// <summary>
        /// Gats the map spatial query data.
        /// </summary>
        /// <param name="mapServer">The map server.</param>
        /// <param name="inputGeometry">The input geometry.</param>
        /// <param name="layerName">Name of the layer.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="returnGeometry">if set to <c>true</c> [return geometry].</param>
        /// <param name="assetType">Type of the asset.</param>
        /// <returns></returns>
        private MapCustomDataTable GatMapSpatialQueryData(MapServerProxy mapServer, 
        ESRI.ArcGIS.ADF.Web.Geometry.Geometry inputGeometry,
        string layerName, int maxRecords, bool returnGeometry, string assetType)
        {
            #region "Local Variables"
            DataTable objDataTable = null;
            MapCustomDataTable objCustomDataTable = null;
            int intLayerID = -1;
            SpatialFilter objSpatailFil;
            MapProcessManager objMapController = null;
            ESRI.ArcGIS.ADF.StringCollection objStrColl = null;
            string strFields = string.Empty;
            ESRI.ArcGIS.ADF.ArcGISServer.RecordSet objRS = null;
            #endregion
            try
            {
                if (!string.IsNullOrEmpty(layerName))
                {
                    intLayerID = GetLayerID(mapServer, layerName);
                    if (intLayerID != -1)
                    {
                        objMapController = new MapProcessManager();
                        objStrColl = new ESRI.ArcGIS.ADF.StringCollection();
                        //creating spatial filter
                        objSpatailFil = new SpatialFilter();
                        //setting geometry to spatial filter
                        objSpatailFil.FilterGeometry = ESRI.ArcGIS.ADF.Web.DataSources.ArcGISServer.Converter.FromAdfGeometry(inputGeometry);
                        objSpatailFil.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        objStrColl = objMapController.GetSubFields(layerName);
                        foreach (string strColumnName in objStrColl)
                        {
                            if (!string.IsNullOrEmpty(strFields))
                                strFields = strFields + "," + strColumnName;
                            else
                                strFields = strColumnName;
                        }
                        if (objStrColl != null && objStrColl.Count > 0) objSpatailFil.SubFields = strFields;
                        //obtaining queried data table
                        objRS = mapServer.QueryFeatureData(mapServer.GetMapName(0), intLayerID, objSpatailFil);
                        objDataTable = ESRI.ArcGIS.ADF.Web.DataSources.ArcGISServer.Converter.ToDataTable(objRS);
                        //formatting data table
                        if (objDataTable != null && objDataTable.Rows.Count > 0 && returnGeometry == false)
                        {
                            objCustomDataTable = FormatDataTable(objDataTable, layerName,
                                 maxRecords, assetType);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (objDataTable != null) objDataTable.Dispose();
                if (objCustomDataTable != null) objCustomDataTable.Dispose();
            }
            return objCustomDataTable;
        }
        /// <summary>
        /// Gets the map server.
        /// </summary>
        /// <returns></returns>
        private MapServerProxy GetMapServer()
        {
            #region Local Variables
            string strMapServiceURL = string.Empty;
            string strMapServiceName = string.Empty;
            string strIdentity = string.Empty;
            string strEndpoint = string.Empty;
            MapProcessManager objMapController = null;
            DataTable objDT = null;
            MapServerProxy objMapServer = null;
            #endregion
            try
            {
                #region "Assigning variables from configuration"
                objMapController = new MapProcessManager();
                objDT = objMapController.ReadList(MapBusinessConstants.SEARCHLISTPORTAL);
                if (objDT != null)
                {
                    foreach (DataRow objDR in objDT.Rows)
                    {
                        if (objDR["title"] != null)
                        {
                            if (string.Equals(objDR["title"].ToString(), MapUIConstants.MAPSERVICEURL))
                            {
                                if (objDR["value"] != null) strMapServiceURL = objDR["value"].ToString();
                            }
                            else if (string.Equals(objDR["title"].ToString(), MapUIConstants.MAPSERVICENAME))
                            {
                                if (objDR["value"] != null) strMapServiceName = objDR["value"].ToString();
                            }
                            else if (string.Equals(objDR["title"].ToString(), MapUIConstants.MAPIDENTITY))
                            {
                                if (objDR["value"] != null) strIdentity = objDR["value"].ToString();
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(strMapServiceURL) &&
                        !string.IsNullOrEmpty(strMapServiceName))
                    {
                        string[] strValues = strMapServiceName.Split('@');
                        if (strValues.Length >= 1)
                        {
                            strEndpoint = strMapServiceURL + "/" + strValues[1].ToString() + "/MapServer";
                            objMapServer = new ESRI.ArcGIS.ADF.ArcGISServer.MapServerProxy(strEndpoint);
                        }
                    }
                }
                #endregion
            }
            catch
            {
                throw;
            }
            return objMapServer;
        }
        /// <summary>
        /// Gets the layer ID.
        /// </summary>
        /// <param name="mapServer">The map server.</param>
        /// <param name="layerName">Name of the layer.</param>
        /// <returns></returns>
        private int GetLayerID(MapServerProxy mapServer, string layerName)
        {
            #region Local Variables
            int intLayerID = -1;
            MapServerInfo objMapServerInfo = null;
            MapLayerInfo[] objLayerInfoArray = null;
            MapLayerInfo objLayerInfo = null;
            #endregion
            try
            {
                objMapServerInfo = mapServer.GetServerInfo(mapServer.GetMapName(0));
                objLayerInfoArray = objMapServerInfo.MapLayerInfos;
                for (int intLayers = 1; intLayers < objLayerInfoArray.Length; intLayers++)
                {
                    objLayerInfo = objLayerInfoArray[intLayers];
                    if (objLayerInfo.Name == layerName)
                    {
                        intLayerID = objLayerInfo.LayerID;
                        break;
                    }
                }
            }
            catch
            {
                throw;
            }
            return intLayerID;
        }
        /// <summary>
        /// Formats the data table.
        /// </summary>
        /// <param name="resultsTable">The results table.</param>
        /// <param name="layerName">Name of the layer.</param>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="inputGeometry">The input geometry.</param>
        /// <param name="whereClause">The where clause.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="assetType">Type of the asset.</param>
        /// <returns></returns>
        private MapCustomDataTable FormatDataTable(DataTable resultsTable, string layerName,
             int maxRecords, string assetType)
        {
            #region Local Variables
            MapCustomDataTable objCustomDataTable = new MapCustomDataTable();
            List<string> columnNamesToRemove = new List<string>();
            MapProcessManager objMapController = new MapProcessManager();
            ArrayList arrFieldNames;
            string strCAMLQuery = string.Empty;
            string strRequiredField = string.Empty;
            string strContextkey = string.Empty;
            #endregion
            try
            {
                if (resultsTable != null)
                {
                    strRequiredField = objMapController.GetRequiredField(layerName, MapBusinessConstants.TYPEREQUIREDFIELD); // get from Configuration
                    strCAMLQuery = "<OrderBy><FieldRef Name='Order_x0020_Number' /></OrderBy><Where><And><Eq><FieldRef Name='Layer_x0020_Name' /><Value Type='Lookup'>" +
                        layerName + "</Value></Eq><Eq><FieldRef Name='Active' /><Value Type='Boolean'>1</Value></Eq></And></Where>";
                    arrFieldNames = objMapController.GetConfigList(MapBusinessConstants.SEARCHLISTMAPLAYERCOLUMN, strCAMLQuery, MapBusinessConstants.SEARCHLISTASSETFIELD);
                    //looping the columns in the data table
                    if (arrFieldNames != null && arrFieldNames.Count > 0)
                    {
                        #region Remove extra columns and set the required session variables for future use
                        foreach (DataColumn dcResults in resultsTable.Columns)
                        {
                            if (!arrFieldNames.Contains(dcResults.ColumnName))
                            {
                                columnNamesToRemove.Add(dcResults.ColumnName);
                            }
                        }
                        foreach (string strColName in columnNamesToRemove)
                        {
                            resultsTable.Columns.Remove(strColName);
                        }
                        //creating new custom table.                
                        objCustomDataTable.Merge(resultsTable);
                        if (objCustomDataTable.Rows.Count > maxRecords)
                        {
                            //assigning session variables
                            objCustomDataTable.IsMaximum = true;
                            if (objCustomDataTable.Rows.Count == maxRecords + 1)
                            {
                                objCustomDataTable.Rows.Remove(objCustomDataTable.Rows[maxRecords]);
                            }
                        }
                        #endregion
                        objCustomDataTable.RequiredField = strRequiredField;
                        objCustomDataTable.AssetType = assetType;
                        // Get context key
                        strContextkey = objMapController.GetRequiredField(layerName, MapBusinessConstants.TYPECONTEXT); // get from Configuration
                        // Caption values
                        arrFieldNames = objMapController.GetConfigList(MapBusinessConstants.SEARCHLISTMAPLAYERCOLUMN, strCAMLQuery, MapBusinessConstants.SEARCHCAPTION);
                        #region Reset Column names
                        if (arrFieldNames != null)
                        {
                            if (objCustomDataTable.Columns.Count == arrFieldNames.Count)
                            {
                                for (int intNames = 0; intNames < arrFieldNames.Count; intNames++)
                                {
                                    objCustomDataTable.Columns[intNames].ColumnName = arrFieldNames[intNames].ToString();
                                    if (string.Equals(arrFieldNames[intNames].ToString(), strContextkey))
                                    {
                                        objCustomDataTable.ContextKey = intNames;
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (objCustomDataTable != null) objCustomDataTable.Dispose();
            }
            return objCustomDataTable;
        }
    }
}
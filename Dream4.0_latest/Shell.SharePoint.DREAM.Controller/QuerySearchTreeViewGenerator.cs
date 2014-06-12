#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: QuerySearchTreeViewGenerator.cs
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using Shell.SharePoint.DREAM.MOSSProcess;

namespace Shell.SharePoint.DREAM.Controller
{
    /// <summary>
    /// This Class Generates the XMl fo create the treeview.
    /// </summary>
    class QuerySearchTreeViewGenerator : Constants
    {
        #region Declaration
        string strSiteURL = string.Empty;
        XmlDocument objXmlDocument;
        MOSSServiceManager objMossServiceManager;
        
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        private string URL
        {
            get
            {
                return strSiteURL;
            }
            set
            {
                strSiteURL = value;
            }
        } 
        #endregion

        #region Public Method
        /// <summary>
        /// Creates the tree view XML.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <returns></returns>
        public XmlDocument CreateTreeViewXml(string siteURL)
        {
            try
            {
                objXmlDocument = new XmlDocument();
                URL = siteURL;
                //Calling the CreateRootElement method.
                objXmlDocument = CreateRootElement();
                return objXmlDocument;
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        #endregion

        #region Private method
        /// <summary>
        /// Creates the root element.
        /// </summary>
        /// <returns></returns>
        private XmlDocument CreateRootElement()
        {
            try
            {
                //Creating the root Xml Element Data Source.
                XmlElement DataSourceParentElement = objXmlDocument.CreateElement(DATASOURCE);
                objXmlDocument.AppendChild(DataSourceParentElement);

                //Creating the RequestInfo node with Name attribute.
                XmlNode DataSourceParentNode = objXmlDocument.DocumentElement;
                //Method call to create DataSource Node.
                CreateDataSource(DataSourceParentNode);
            }
            catch (Exception)
            {
                throw;
            }
            //returns the final SearchRequest Xml.
            return objXmlDocument;
        }

        /// <summary>
        /// Creates the data source.
        /// </summary>
        /// <param name="DataSourceParentNode">The data source parent node.</param>
        private void CreateDataSource(XmlNode DataSourceParentNode)
        {
            DataTable dtDataSource = null;
            DataRow dtDataSourceRow;
            dtDataSource = new DataTable();
            objMossServiceManager = new MOSSServiceManager();
            dtDataSource = objMossServiceManager.ReadList(URL, DATASOURCELISTNAME, string.Empty);
            if (dtDataSource != null)
            {
                for (int intIndex = 0; intIndex < dtDataSource.Rows.Count; intIndex++)
                {
                    dtDataSourceRow = dtDataSource.Rows[intIndex];
                    XmlElement DataSourceElement = objXmlDocument.CreateElement(DATASOURCENAME);
                    DataSourceParentNode.AppendChild(DataSourceElement);

                    XmlAttribute NameAttribute = objXmlDocument.CreateAttribute(NAME);
                    DataSourceElement.Attributes.Append(NameAttribute);
                    NameAttribute.Value = dtDataSourceRow["Title"].ToString();

                    CreateDataProvider(DataSourceElement, dtDataSourceRow["Logical_x0020_Name"].ToString());
                }
            }
        }

        /// <summary>
        /// Creates the data provider.
        /// </summary>
        /// <param name="DataSourceElement">The data source element.</param>
        /// <param name="dataSourceName">Name of the data source.</param>
        private void CreateDataProvider(XmlElement DataSourceElement, string dataSourceName)
        {
            DataTable dtDataProvider = null;
            DataRow dtDataProviderRow;
            dtDataProvider = new DataTable();
            objMossServiceManager = new MOSSServiceManager();
            string strFolderName = dataSourceName;
            dtDataProvider = objMossServiceManager.ReadFolderList(URL, DATAPROVIDERLISTNAME, strFolderName);
            //dtDataProvider = FilterDataTable(dtDataProvider, "Table_x0020_Name", dataSourceName);
            if (dtDataProvider != null)
            {
                for (int intIndex = 0; intIndex < dtDataProvider.Rows.Count; intIndex++)
                {
                    dtDataProviderRow = dtDataProvider.Rows[intIndex];
                    XmlElement DataProviderElement = objXmlDocument.CreateElement(DATAPROVIDERNAME);
                    DataSourceElement.AppendChild(DataProviderElement);

                    XmlAttribute NameAttribute = objXmlDocument.CreateAttribute(NAME);
                    DataProviderElement.Attributes.Append(NameAttribute);
                    NameAttribute.Value = dtDataProviderRow["Title"].ToString();

                    CreateTableNames(DataProviderElement, dtDataProviderRow["Logical_x0020_Name"].ToString());
                }
            }
        }

        /// <summary>
        /// Creates the table names.
        /// </summary>
        /// <param name="DataProviderElement">The data provider element.</param>
        /// <param name="p">The p.</param>
        private void CreateTableNames(XmlElement DataProviderElement, string dataProviderName)
        {
            DataTable dtDataTable = null;
            DataRow dtDataTableRow;
            dtDataTable = new DataTable();
            objMossServiceManager = new MOSSServiceManager();
            string strFolderName = dataProviderName + " Tables";
            dtDataTable = objMossServiceManager.ReadFolderList(URL, "Query Search Table", strFolderName);
            if (dtDataTable != null)
            {
                for (int intIndex = 0; intIndex < dtDataTable.Rows.Count; intIndex++)
                {
                    dtDataTableRow = dtDataTable.Rows[intIndex];
                    XmlElement DataTableElement = objXmlDocument.CreateElement(DATATABLE);
                    DataProviderElement.AppendChild(DataTableElement);
                    DataTableElement.InnerText = dtDataTableRow["Title"].ToString();
                    XmlAttribute TableActualNameAttribute = objXmlDocument.CreateAttribute(NAME);
                    DataTableElement.Attributes.Append(TableActualNameAttribute);
                    TableActualNameAttribute.Value = dtDataTableRow["Logical_x0020_Name"].ToString();
                }
            }
        }         
        #endregion
    }
}

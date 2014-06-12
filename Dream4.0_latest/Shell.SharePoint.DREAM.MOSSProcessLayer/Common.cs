#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: Common.cs
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing.Navigation;
using System.Web;
using System.Data;
using System.IO;
using System.Xml;
using System.Net;




namespace Shell.SharePoint.DREAM.MOSSProcess
{
    /// <summary>
    /// This class contains methods that are commonly used in the project.
    /// </summary>
    public class Common
    {
        #region Declaration
        const string XSLTEMPLATES = "XSLTemplates";
        const string TEAMREGISTRATION = "Team Registration";
        const string USERACCESSREQUESTLIST = "User Access Request";
        const string TEAMADDERRORMESSAGE = "A team with the name you specified already exists. Specify a different team name.";
        // AccessRequest objAccessRequest;

        #endregion
        #region PUBLIC METHODS
        /// <summary>
        /// Reads the list & return the list items as Datatable.
        /// </summary>
        /// <param name="parentSiteUrl">The parent site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <returns></returns>
        internal DataTable ReadList(string parentSiteUrl, string listName)
        {
            DataTable objListItems = new DataTable();
            SPList list;
            try
            {
                /// Test whether RunWithElevatedPrivileges needed for "Read only" operation.

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using(SPSite site = new SPSite(parentSiteUrl))
                    {
                        using(SPWeb web = site.OpenWeb())
                        {
                            list = web.Lists[listName];
                            if(list.ItemCount > 0)
                            {
                                /// Reads the values from sharepoint list.
                                objListItems = list.Items.GetDataTable();
                            }
                        }
                    }
                });
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if(objListItems != null)
                    objListItems.Dispose();
            }
            return objListItems;
        }
        /// <summary>
        /// Reads the list & return the list items as Datatable.
        /// This function accepts CAML query using the paramenter querystring
        /// </summary>
        /// <param name="parentSiteUrl">The parent site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns></returns>
        internal DataTable ReadList(string parentSiteUrl, string listName, string queryString)
        {
            DataTable objListItems = new DataTable();
            SPList list;
            SPQuery query;
            try
            {
                /// Test whether RunWithElevatedPrivileges needed for "Read only" operation.

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using(SPSite site = new SPSite(parentSiteUrl))
                    {
                        using(SPWeb web = site.OpenWeb())
                        {
                            list = web.Lists[listName];
                            query = new SPQuery();
                            query.Query = queryString;
                            if(list.GetItems(query).Count > 0)
                            {
                                /// Reads the values from sharepoint list.
                                objListItems = list.GetItems(query).GetDataTable();
                            }
                        }
                    }
                });
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if(objListItems != null)
                    objListItems.Dispose();
            }
            return objListItems;
        }

        /// <summary>
        /// Reads the list.
        /// </summary>
        /// <param name="parentSiteUrl">The parent site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="queryString">The query string.</param>
        /// <param name="viewFields">The view fields.</param>
        /// <returns></returns>
        internal DataTable ReadList(string parentSiteUrl, string listName, string queryString, string viewFields)
        {
            DataTable objListItems = new DataTable();
            SPList list;
            SPQuery query;
            try
            {
                /// Test whether RunWithElevatedPrivileges needed for "Read only" operation.

                SPSecurity.RunWithElevatedPrivileges(delegate()
               {
                   using(SPSite site = new SPSite(parentSiteUrl))
                   {
                       using(SPWeb web = site.OpenWeb())
                       {
                           list = web.Lists[listName];
                           query = new SPQuery();
                           if(!string.IsNullOrEmpty(viewFields))
                           {
                               query.ViewFields = viewFields;
                           }
                           query.Query = queryString;
                           if(list.GetItems(query).Count > 0)
                           {
                               /// Reads the values from sharepoint list.
                               objListItems = list.GetItems(query).GetDataTable();
                           }
                       }
                   }
               });
            }
            catch
            {
                throw;
            }
            return objListItems;
        }

        /// <summary>
        /// Gets the XSL template.
        /// </summary>
        /// <param name="strType">Type of the STR.</param>
        /// <param name="strParentSiteUrl">The STR parent site URL.</param>
        /// <returns></returns>
        internal XmlTextReader GetXSLTemplate(string xslFileName, string parentSiteUrl)
        {
            SPFile XSLFile = null;
            MemoryStream objMemoryStream = null;
            XmlTextReader xmlTextReader = null;

            try
            {
                /// Test whether RunWithElevatedPrivileges needed for "Read only" operation.

                SPSecurity.RunWithElevatedPrivileges(delegate()
                   {
                       using(SPSite site = new SPSite(parentSiteUrl))
                       {
                           using(SPWeb web = site.OpenWeb())
                           {
                               /// Reads the Xsl template files from XSLTemplate list.
                               XSLFile = web.Folders[XSLTEMPLATES].Files[xslFileName + ".xsl"];
                               if(XSLFile != null)
                               {
                                   objMemoryStream = new MemoryStream(XSLFile.OpenBinary());
                                   xmlTextReader = new XmlTextReader(objMemoryStream);
                               }
                               else
                               {
                                   throw new Exception(xslFileName + ".xsl" + " XML Template not Found");
                               }

                           }
                       }
                   });
                return xmlTextReader;
            }
            catch(Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Read the Items from folders and return as datatable.
        /// This function accepts CAML query using the paramenter querystring
        /// </summary>
        /// <param name="parentSiteUrl">The parent site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns></returns>
        internal DataTable ReadFolderList(string parentSiteUrl, string listName, string foldername)
        {
            DataTable objListItems = new DataTable();
            SPList list;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        using(SPSite site = new SPSite(parentSiteUrl))
                        {
                            using(SPWeb web = site.OpenWeb())
                            {
                                list = web.Lists[listName];
                                string filename = string.Empty;
                                SPFolder spFolder = web.GetFolder("/Lists/" + listName + "/" + foldername);
                                if(spFolder.Exists)
                                {
                                    SPQuery qry = new SPQuery();
                                    qry.Folder = spFolder;
                                    objListItems = web.Lists[spFolder.ParentListId].GetItems(qry).GetDataTable();
                                }
                            }
                        }
                    });
            }
            catch(Exception)
            {
                throw;
            }
            return objListItems;
        }
        /// <summary>
        /// Saves the date format.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="ConfigItems">The config items.</param>
        internal void SaveDateFormat(string key, string value, Dictionary<string, string> ConfigItems)
        {
            SPListItemCollection preferencesItemsColl = null;
            SPList preferencesList;
            SPQuery query;
            string siteURL = string.Empty;
            try
            {
                siteURL = HttpContext.Current.Request.Url.ToString();
                if(ConfigItems.Count != 0)
                {
                    if(ConfigItems.ContainsKey(key))
                    {
                        ConfigItems.Remove(key);
                        ConfigItems.Add(key, value);
                    }
                    else
                    {
                        ConfigItems.Add(key, value);
                    }
                }
                /// SPSite with site can be created directly.
                using(SPSite newSite = new SPSite(siteURL))
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        using(SPSite site = new SPSite(siteURL))
                        {
                            using(SPWeb web = site.OpenWeb())
                            {
                                preferencesList = web.Lists["Portal Configurations"];
                                web.AllowUnsafeUpdates = true;
                                query = new SPQuery();
                                preferencesItemsColl = preferencesList.GetItems(query);
                                foreach(SPListItem preferencesItem in preferencesItemsColl)
                                {
                                    if(preferencesItem["Key"].ToString() == key)
                                    {
                                        preferencesItem["Value"] = value;
                                        preferencesItem.Update();
                                    }
                                }
                                web.AllowUnsafeUpdates = false;
                            }
                        }
                    });
                }
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Deletes the list item.
        /// </summary>
        /// <param name="listName">Name of the list.</param>
        /// <param name="itemId">The item id.</param>
        internal void DeleteListItem(string listName, string itemId)
        {
            string strSiteUrl = SPContext.Current.Site.Url;

            /// SPSite with site can be created directly.
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using(SPSite site = new SPSite(strSiteUrl))
                {
                    using(SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        SPList objList = web.Lists[listName];
                        objList.Items.DeleteItemById(Convert.ToInt32(itemId));
                        objList.Update();
                        web.AllowUnsafeUpdates = false;

                    }
                }
            });
        }



        /// <summary>
        /// Updates the list item.
        /// </summary>
        /// <param name="listValues">The list values.</param>
        /// <param name="listName">Name of the list.</param>
        internal string UpdateListItem(Dictionary<string, string> listValues, string listName)
        {
            string strSiteUrl = SPContext.Current.Site.Url;
            string strListItemId = "0";

            /// SPSite with site can be created directly.
            /// RunWithElevatedPrivileges only when listvalues.count > 0
            if(listValues.Keys.Count > 0)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                 {
                     using(SPSite site = new SPSite(strSiteUrl))
                     {
                         using(SPWeb web = site.OpenWeb())
                         {
                             SPList list = web.Lists[listName];
                             SPListItem listItem = null;

                             web.AllowUnsafeUpdates = true;

                             if(listValues.ContainsKey("ID"))
                             {
                                 listItem = list.Items.GetItemById(Convert.ToInt32(listValues["ID"]));
                                 /// Delete the existing item.
                                 listValues.Remove("ID");

                                 if(string.Equals(listName, TEAMREGISTRATION))
                                 {
                                     if(string.Equals(listItem["Title"].ToString().ToLowerInvariant(), listValues["Title"].ToString().ToLowerInvariant()))
                                     {
                                         listValues.Remove("Title");
                                     }
                                     else
                                     {
                                         SPQuery objQuery = new SPQuery();
                                         objQuery.Query = "<Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">" + listValues["Title"].ToString() + "</Value></Eq></Where>";

                                         if(list.GetItems(objQuery) != null &&
                                              list.GetItems(objQuery).Count > 0)
                                         {
                                             throw new WebException(TEAMADDERRORMESSAGE);

                                         }
                                     }
                                 }

                                 foreach(string strKey in listValues.Keys)
                                 {
                                     listItem[strKey] = listValues[strKey].ToString();
                                 }
                             }
                             else
                             {
                                 listItem = list.Items.Add();
                                 if(string.Equals(listName, TEAMREGISTRATION))
                                 {
                                     SPQuery objQuery = new SPQuery();
                                     objQuery.Query = "<Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">" + listValues["Title"].ToString() + "</Value></Eq></Where>";

                                     if(list.GetItems(objQuery) != null &&
                                          list.GetItems(objQuery).Count > 0)
                                     {
                                         throw new WebException(TEAMADDERRORMESSAGE);
                                     }
                                 }
                                 foreach(string strKey in listValues.Keys)
                                 {
                                     listItem[strKey] = listValues[strKey].ToString();
                                 }
                             }

                             listItem.Update();

                             strListItemId = listItem["ID"].ToString();
                             web.AllowUnsafeUpdates = false;
                         }
                     }
                 });
            }
            return strListItemId;
        }



        /// <summary>
        /// Reads the XML file from share point.
        /// </summary>
        /// <param name="currentSiteUrl">The current site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        internal XmlDocument ReadXmlFileFromSharePoint(string currentSiteUrl, string listName, string query)
        {
            XmlDocument objXmlDocument = new XmlDocument();
            /// Test whether RunWithElevatedPrivileges needed for "Read only" operation.

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using(SPSite site = new SPSite(currentSiteUrl))
                {
                    using(SPWeb web = site.OpenWeb())
                    {
                        SPList objSPList = web.Lists[listName];
                        SPQuery objQuery = new SPQuery();
                        objQuery.Query = query;
                        if(objSPList.GetItems(objQuery).Count > 0)
                        {
                            objXmlDocument.Load(objSPList.GetItems(objQuery)[0].File.OpenBinaryStream());
                        }

                    }
                }
            });
            return objXmlDocument;
        }
        /// <summary>
        /// Gets the chached list items.
        /// </summary>
        /// <param name="currentSiteUrl">The current site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        internal SiteMapNodeCollection GetChachedListItems(string currentSiteUrl, string listName, string query)
        {
            SiteMapNodeCollection listItemsCached = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
              {
                  using(SPSite site = new SPSite(currentSiteUrl))
                  {
                      using(SPWeb web = site.OpenWeb())
                      {
                          SPQuery objQuery = new SPQuery();
                          objQuery.Query = query;
                          PortalSiteMapProvider objPortalSiteMapProvider = PortalSiteMapProvider.WebSiteMapProvider;
                          PortalWebSiteMapNode pNode = (PortalWebSiteMapNode)objPortalSiteMapProvider.FindSiteMapNode(web.ServerRelativeUrl);
                          listItemsCached = objPortalSiteMapProvider.GetCachedListItemsByQuery(pNode, listName, objQuery, web);
                      }
                  }
              });
            return listItemsCached;
        }
        /// <summary>
        /// Gets the chached list items.
        /// </summary>
        /// <param name="currentSiteUrl">The current site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        internal SiteMapNodeCollection GetChachedListItems(string currentSiteUrl, string listName, SPQuery spQuery)
        {
            SiteMapNodeCollection listItemsCached = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
              {
                  using(SPSite site = new SPSite(currentSiteUrl))
                  {
                      using(SPWeb web = site.OpenWeb())
                      {
                          PortalSiteMapProvider objPortalSiteMapProvider = PortalSiteMapProvider.WebSiteMapProvider;
                          PortalWebSiteMapNode pNode = (PortalWebSiteMapNode)objPortalSiteMapProvider.FindSiteMapNode(web.ServerRelativeUrl);
                          listItemsCached = objPortalSiteMapProvider.GetCachedListItemsByQuery(pNode, listName, spQuery, web);
                      }
                  }
              });
            return listItemsCached;
        }
        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="currentSiteUrl">The current site URL.</param>
        /// <param name="docLibName">Name of the doc lib.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        internal bool DeleteXMLFile(string currentSiteUrl, string docLibName, string fileName)
        {
            /// Test whether RunWithElevatedPrivileges needed for "Read only" operation.
            bool blnSuccess = false;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using(SPSite site = new SPSite(currentSiteUrl))
                {
                    using(SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        SPFolder objFolder = web.GetFolder(docLibName);
                        if(objFolder.Files[fileName + ".xml"].Exists)
                        {
                            objFolder.Files[fileName + ".xml"].Delete();
                            objFolder.Update();
                            blnSuccess = true;
                        }
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });
            return blnSuccess;
        }
        #endregion
    }

}

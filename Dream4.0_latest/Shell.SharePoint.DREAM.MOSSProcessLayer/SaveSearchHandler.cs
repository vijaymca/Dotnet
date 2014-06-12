#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : SaveSearchHandler.cs
#endregion

using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.DREAM.MOSSProcess
{
    /// <summary>
    /// The methods used in this class are used to handle the Save Search Functionality.
    /// </summary>
    public class SaveSearchHandler
    {
        #region Declaration
        const string XSLPATH = "/saveSearchRequests/saveSearchRequest/requestinfo/report/constraint/entities/entity";
        const string PARENTXPATH = "/saveSearchRequests/saveSearchRequest";
        const string PARENTXPATHQSGEN = "/saveSearchRequests/saveSearchRequest[@type='general']";
        const string PARENTXPATHQSGENSHARED = "/saveSearchRequests/saveSearchRequest[@type='general' and @shared='True']";
        const string PARENTXPATHQSSQL = "/saveSearchRequests/saveSearchRequest[@type='sql']";
        const string PARENTXPATHQSSQLSHARED = "/saveSearchRequests/saveSearchRequest[@type='sql' and @shared='True']";
        const string ATTRIBNAME = "name";
        const string ATTRIBORDER = "order";
        string strCurrSiteURL = SPContext.Current.Site.Url.ToString();
        const string ATTRIBSHARED = "shared";
        const string QUERYSEARCH = "Query Search";

        const string GENQRYSRCH = "General Query Search";
        const string SQLQRYSRCH = "Sql Query Search";

        #endregion
        #region Methods
        /// <summary>
        /// Gets the doc lib XML file.
        /// </summary>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="userID">The user ID.</param>
        /// <returns></returns>
        internal XmlDocument GetDocLibXMLFile(string searchType, string userID)
        {
            #region Method Variables
            XmlDocument objXmlDoc = new XmlDocument();
            string strFileName = userID + ".xml";
            SPFile spXMLFile = null;
            MemoryStream objMemoryStream = null;
            XmlTextReader xmlTextReader = null;
            #endregion
            try
            {
                if(IsDocLibExist(searchType, strCurrSiteURL))
                {
                    //Get the XmlDocument from Document Library.
                    if (IsDocLibFileExist(searchType, userID))
                    {
                        //returns the XML File
                        SPSecurity.RunWithElevatedPrivileges(delegate()
                        {
                            using (SPSite site = new SPSite(strCurrSiteURL))
                            {
                                using (SPWeb web = site.OpenWeb())
                                {
                                    spXMLFile = web.Folders[searchType].Files[strFileName];
                                    if (spXMLFile != null)
                                    {
                                        objMemoryStream = new MemoryStream(spXMLFile.OpenBinary());
                                        xmlTextReader = new XmlTextReader(objMemoryStream);
                                        objXmlDoc.Load(xmlTextReader);
                                    }
                                }
                            }
                        });//end of SPSecurity
                    }
                }
                else
                {
                    //returns Null if the Document Library does not exist
                    throw new Exception(searchType + " - Document Library not Found.");
                }
            }            
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (objMemoryStream != null)
                {
                    objMemoryStream.Close();
                    objMemoryStream.Dispose();
                }
            }
            return objXmlDoc;
        }
        /// <summary>
        /// Gets the admin save search XML.
        /// </summary>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="userID">The user ID.</param>
        /// <param name="strSaveSearchName">Name of the STR save search.</param>
        /// <returns></returns>
        internal XmlDocument GetAdminSaveSearchXML(string searchType, string userID, string saveSearchName)
        {
            try
            {
                XmlDocument objXmlDocument = new XmlDocument();
                XmlDocument objXmlSaveSearchDoc = new XmlDocument();
                SPSecurity.RunWithElevatedPrivileges(delegate()
               {
                   objXmlDocument = GetDocLibXMLFile(searchType, userID);
                   if (objXmlDocument != null)
                   {
                       XmlNodeList objXmlNodeList = objXmlDocument.SelectNodes(PARENTXPATH);
                       //Loop through the nodes in XmlNodeList 
                       foreach (XmlNode objXmlNode in objXmlNodeList)
                       {
                           if (string.Equals(objXmlNode.Attributes.GetNamedItem(ATTRIBNAME).Value.ToString(), saveSearchName))
                           {
                               objXmlSaveSearchDoc.LoadXml(objXmlNode.InnerXml);
                           }
                       }
                   }
               });
                return objXmlSaveSearchDoc;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Determines whether the document library exist for the specified search type.
        /// </summary>
        /// <param name="searchType">Type of the search.</param>
        /// <returns>
        ///     <c>true</c> if [document library exist for the specified search type]; otherwise, <c>false</c>.
        /// </returns>
        internal bool IsDocLibExist(string docLibName, string currentSiteUrl)
        {
            bool blnIsDocLibExist = false;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using(SPSite site = new SPSite(currentSiteUrl))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            try
                            {
                                blnIsDocLibExist = web.Folders[docLibName].Exists;
                            }
                            catch
                            {
                                blnIsDocLibExist = false;
                            }
                        }
                    }
                });
            }
            catch (SPException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            return blnIsDocLibExist;
        }
        /// <summary>
        /// Determines whether the xml file exist for the logged in User.
        /// </summary>
        /// <param name="searchType">Type of the search.</param>
        /// <returns>
        ///     <c>true</c> if [document library exist for the specified search type]; otherwise, <c>false</c>.
        /// </returns>
        internal bool IsDocLibFileExist(string searchType, string userID)
        {
            bool blnIsDocLibFileExist = false;
            string strFileName = userID + ".xml";
            try
            {
                //check for the privileges.
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(strCurrSiteURL))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            try
                            {
                                blnIsDocLibFileExist = web.GetFile(searchType + "/" + strFileName).Exists;
                            }
                            catch
                            {
                                blnIsDocLibFileExist = false;
                            }
                        }
                    }
                });
            }
            catch (SPException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            return blnIsDocLibFileExist;
        }        
        /// <summary>
        /// Uploads to document lib.
        /// </summary>
        /// <param name="finalDocument">The final document.</param>
        internal void UploadToDocumentLib(string searchType, string userID, XmlDocument finalDocument)
        {            
            SPFile newFile = null;
            try
            {
                //check for the privileges.
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(strCurrSiteURL))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;
                            ASCIIEncoding encoding = new ASCIIEncoding();
                            newFile = web.Folders[searchType].Files.Add(userID + ".xml", (encoding.GetBytes(finalDocument.OuterXml)), true);
                            web.AllowUnsafeUpdates = false;
                        }
                    }
                });
            }
            catch (SPException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }            
        }
        internal void DeleteFromDocumentLib(string searchType, string userID)
        {           
            try
            {
                //check for the privileges.
                SPSecurity.RunWithElevatedPrivileges(delegate()
               {
                    using (SPSite site = new SPSite(strCurrSiteURL))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                           if(IsDocLibFileExist(searchType,userID))
                            {
                                web.AllowUnsafeUpdates = true;
                                web.Folders[searchType].Files[userID + ".xml"].Delete();
                                web.Folders[searchType].Update();
                                web.AllowUnsafeUpdates = false;
                            }                           
                        }
                    }
               });
            }
            catch (SPException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Gets the name of the save search.
        /// </summary>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="userID">The user ID.</param>
        /// <returns></returns>
        internal ArrayList GetSaveSearchName(string searchType, string userID)
        {
            ArrayList arlListSaveSearch = new ArrayList();
            XmlDocument objXmlDoc = new XmlDocument();
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
               {
                   //Read Document Library for the specific SearchType and UserID and load it into a XmlDocument
                   objXmlDoc = GetDocLibXMLFile(searchType, userID);
                   if (objXmlDoc != null)
                   {
                       if (searchType == QUERYSEARCH)
                       {
                           XmlNodeList xmlNodeListGen = objXmlDoc.SelectNodes(PARENTXPATHQSGEN);
                           XmlNodeList xmlNodeListSql = objXmlDoc.SelectNodes(PARENTXPATHQSSQL);

                           if (xmlNodeListGen.Count > 0)
                           {
                               arlListSaveSearch.Add(GENQRYSRCH);
                           }
                           foreach (XmlNode xmlNode in xmlNodeListGen)
                           {
                               arlListSaveSearch.Add(xmlNode.Attributes.GetNamedItem(ATTRIBNAME).Value.ToString());
                           }
                           if (xmlNodeListSql.Count > 0)
                           {
                               arlListSaveSearch.Add(SQLQRYSRCH);
                           }
                           foreach (XmlNode xmlNode in xmlNodeListSql)
                           {
                               arlListSaveSearch.Add(xmlNode.Attributes.GetNamedItem(ATTRIBNAME).Value.ToString());
                           }

                       }
                       else
                       {
                           XmlNodeList xmlNodeList = objXmlDoc.SelectNodes(PARENTXPATH);
                           foreach (XmlNode xmlNode in xmlNodeList)
                           {
                               arlListSaveSearch.Add(xmlNode.Attributes.GetNamedItem(ATTRIBNAME).Value.ToString());
                           }
                       }

                   }
               });
            }
            catch (SPException)
            {
                throw;
            }
            catch (IndexOutOfRangeException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            return arlListSaveSearch;
        }
        /// <summary>
        /// Gets the name of the shared save search.
        /// </summary>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="userID">The user ID.</param>
        /// <returns></returns>
        internal ArrayList GetSharedSaveSearchName(string searchType, string userID, bool fetchAll)
        {
            ArrayList arlListSaveSearch = new ArrayList();
            XmlDocument objXmlDoc = new XmlDocument();
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
               {
                   //Read Document Library for the specific SearchType and UserID and load it into a XmlDocument
                   objXmlDoc = GetDocLibXMLFile(searchType, userID);
                   if (objXmlDoc != null)
                   {

                       if (searchType == QUERYSEARCH)
                       {
                           XmlNodeList xmlNodeListGen;
                           XmlNodeList xmlNodeListSql;

                           if (fetchAll)
                           {
                               xmlNodeListGen = objXmlDoc.SelectNodes(PARENTXPATHQSGEN);
                               xmlNodeListSql = objXmlDoc.SelectNodes(PARENTXPATHQSSQL);

                           }
                           else
                           {
                               xmlNodeListGen = objXmlDoc.SelectNodes(PARENTXPATHQSGENSHARED);
                               xmlNodeListSql = objXmlDoc.SelectNodes(PARENTXPATHQSSQLSHARED);
                           }

                           if (xmlNodeListGen.Count > 0)
                           {
                               arlListSaveSearch.Add(GENQRYSRCH);
                           }
                           foreach (XmlNode xmlNode in xmlNodeListGen)
                           {
                               arlListSaveSearch.Add(xmlNode.Attributes.GetNamedItem(ATTRIBNAME).Value.ToString());
                           }
                           if (xmlNodeListSql.Count > 0)
                           {
                               arlListSaveSearch.Add(SQLQRYSRCH);
                           }
                           foreach (XmlNode xmlNode in xmlNodeListSql)
                           {
                               arlListSaveSearch.Add(xmlNode.Attributes.GetNamedItem(ATTRIBNAME).Value.ToString());
                           }

                       }
                       else
                       {
                           XmlNodeList xmlNodeList;
                           if (fetchAll)
                           {
                               xmlNodeList = objXmlDoc.SelectNodes(PARENTXPATH);
                           }
                           else
                           {
                               xmlNodeList = objXmlDoc.SelectNodes(PARENTXPATH + "[@" + ATTRIBSHARED + "=\"True\"]");
                           }
                           foreach (XmlNode xmlNode in xmlNodeList)
                           {
                               arlListSaveSearch.Add(xmlNode.Attributes.GetNamedItem(ATTRIBNAME).Value.ToString());
                           }
                       }

                   }
               });
            }
            catch (Exception)
            {
                throw;
            }
            return arlListSaveSearch;
        }

        /// <summary>
        /// Gets the Save Search Order Number.
        /// </summary>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="userID">The user ID.</param>
        /// <returns></returns>
        internal ArrayList GetSaveOrderNumber(string searchType, string userID)
        {
            ArrayList arlListSaveSearch = new ArrayList();
            XmlDocument objXmlDoc = new XmlDocument();
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
               {
                   //Read Document Library for the specific SearchType and UserID and load it into a XmlDocument
                   objXmlDoc = GetDocLibXMLFile(searchType, userID);
                   if (objXmlDoc != null)
                   {
                       XmlNodeList xmlNodeList = objXmlDoc.SelectNodes(PARENTXPATH);
                       foreach (XmlNode xmlNode in xmlNodeList)
                       {
                           arlListSaveSearch.Add(xmlNode.Attributes.GetNamedItem(ATTRIBORDER).Value.ToString());
                       }
                   }
               });
            }
            catch (Exception)
            {
                throw;
            }
            return arlListSaveSearch;
        }
        /// <summary>
        /// Loads the save search into the "Save Search" drop down box.  
        /// </summary>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="userID">The user ID.</param>
        /// <param name="comboBoxControl">The combo box control.</param>
        internal void LoadSaveSearch(string searchType, DropDownList comboBoxControl)
        {
            CommonUtility objCommonUtility = new CommonUtility();
            ArrayList arlListSaveSearch = new ArrayList();
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
               {
                   string strUserId = objCommonUtility.GetSaveSearchUserName();
                   arlListSaveSearch = GetSaveSearchName(searchType, strUserId);
                   //Loops through the values in Save Search List.

                   ListItem objItem;
                   foreach (string strSaveSearchName in arlListSaveSearch)
                   {
                       objItem = new ListItem();
                       objItem.Text = strSaveSearchName;
                       objItem.Value = strSaveSearchName;
                       comboBoxControl.Items.Add(objItem);
                   }
               });
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Checks whether duplicate search criteria exists..
        /// </summary>
        /// <param name="childXml">The child XML.</param>
        /// <param name="parentXml">The parent XML.</param>
        /// <returns></returns>
        internal bool IsDuplicateExists(XmlDocument childXml, XmlDocument parentXml)
        {
            try
            {
                bool blnReturnValue = false;
                SPSecurity.RunWithElevatedPrivileges(delegate()
               {
                   XmlNodeList objParentXmlNodeList = parentXml.SelectNodes(PARENTXPATH);
                   //Loops through the nodes in XmlNodeList.
                   foreach (XmlNode objParentXmlNode in objParentXmlNodeList)
                   {
                       if (string.Compare(objParentXmlNode.InnerXml, childXml.FirstChild.InnerXml, true) == 0)
                       {
                           blnReturnValue = true;
                           break;
                       }
                   }
               });
                return blnReturnValue;
            }
            catch (Exception)
            {
                throw;
            }
            
        }
        #endregion
    }
}
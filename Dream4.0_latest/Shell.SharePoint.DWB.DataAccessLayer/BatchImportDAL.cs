#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: BatchImportDAL.cs
#endregion
using System;
using System.Xml;
using System.IO;
using Microsoft.SharePoint;
using System.Text;

namespace Shell.SharePoint.DWB.DataAccessLayer
{
    /// <summary>
    /// Batch Import Data Access class
    /// </summary>
    public class BatchImportDAL
    {
        #region DECLARATION
        const string BATCHIMPORTFOLDER = "Batch Import Configuration";
        const string BOOKID = "BookID";
        const string XPATH = "/batchImports/batchImport";
        string strCurrSiteURL = SPContext.Current.Site.Url.ToString();
        #endregion
        /// <summary>
        /// Gets the only batch import XML.
        /// </summary>
        /// <param name="bookID">The book ID.</param>
        /// <returns></returns>
        internal XmlDocument GetOnlyBatchImportXML(string bookID)
        {
            XmlDocument objXmlDocument;
            XmlDocument objXmlBatchImportDoc = null;
            try
            {                
                SPSecurity.RunWithElevatedPrivileges(delegate()
               {
                   objXmlDocument = GetBatchImportXMLFile();
                   if (objXmlDocument != null)
                   {
                       XmlNodeList objXmlNodeList = objXmlDocument.SelectNodes(XPATH);
                       //Loop through the nodes in XmlNodeList 
                       foreach (XmlNode objXmlNode in objXmlNodeList)
                       {
                           if (string.Equals(objXmlNode.Attributes.GetNamedItem(BOOKID).Value.ToString(), bookID))
                           {
                               objXmlBatchImportDoc = new XmlDocument();
                               objXmlBatchImportDoc.LoadXml(objXmlNode.OuterXml);
                           }
                       }
                   }
               });
                return objXmlBatchImportDoc;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Uploads to document lib.
        /// </summary>
        /// <param name="bookID">The book ID.</param>
        /// <param name="finalDocument">The final document.</param>
        internal void UploadToDocumentLib(string bookID, XmlDocument finalDocument)
        {
            SPFile newFile = null;
            try
            {
                finalDocument = ModifyUpdatedXmlDocument(bookID, finalDocument);
                //check for the privileges.
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(strCurrSiteURL))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;
                            ASCIIEncoding encoding = new ASCIIEncoding();
                            newFile = web.Folders[BATCHIMPORTFOLDER].Files.Add("BatchImport.xml", (encoding.GetBytes(finalDocument.OuterXml)), true);
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
        #region PRIVATE METHODS
        /// <summary>
        /// Gets the updated XML document.
        /// </summary>
        /// <param name="bookID">The book ID.</param>
        /// <param name="finalDocument">The final document.</param>
        /// <returns></returns>
        private XmlDocument ModifyUpdatedXmlDocument(string bookID, XmlDocument finalDocument)
        {
            XmlDocument xmlBatchImportDoc = null;
            XmlDocument xmlUpdatedDocument = null;
            XmlNodeList xmlnodelist;
            XmlNode xmlParentNode;
            bool blnIsBookExists = false;
            try
            {
                //Gets the Xml Document for the book.
                xmlBatchImportDoc = GetBatchImportXMLFile();
                if (xmlBatchImportDoc != null)
                {
                    XmlNode xmlBatchImportNode = finalDocument.SelectSingleNode(XPATH);
                    //XmlNode xmlBatchImportNode = finalDocument.ChildNodes[0];
                    xmlnodelist = xmlBatchImportDoc.SelectNodes(XPATH);
                    foreach (XmlNode xmlNode in xmlnodelist)
                    {
                        if (string.Compare(xmlNode.Attributes.GetNamedItem(BOOKID).Value.ToString(), bookID) == 0)
                        {
                            xmlParentNode = xmlNode.ParentNode;
                            xmlParentNode.RemoveChild(xmlNode);                            
                            
                            XmlNode newXmlNode = xmlBatchImportDoc.ImportNode(xmlBatchImportNode,true);
                            xmlBatchImportDoc.DocumentElement.AppendChild(newXmlNode);
                            blnIsBookExists = true;
                        }                        
                    }
                    if (!blnIsBookExists)
                    {
                        XmlNode newXmlNode = xmlBatchImportDoc.ImportNode(xmlBatchImportNode, true);
                        xmlBatchImportDoc.DocumentElement.AppendChild(newXmlNode);
                    }
                    xmlUpdatedDocument = new XmlDocument();
                    xmlUpdatedDocument.LoadXml(xmlBatchImportDoc.OuterXml);
                }
                else
                {
                    xmlUpdatedDocument = new XmlDocument();
                    xmlUpdatedDocument.LoadXml(finalDocument.OuterXml);
                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return xmlUpdatedDocument;
        }
        
        /// <summary>
        /// Gets the doc lib XML file.
        /// </summary>
        /// <returns></returns>
        private XmlDocument GetBatchImportXMLFile()
        {
            #region Method Variables
            XmlDocument objXmlDoc = null;
            string strFileName = "BatchImport.xml";
            SPFile spXMLFile = null;
            MemoryStream objMemoryStream = null;
            XmlTextReader xmlTextReader = null;
            #endregion
            try
            {
                if (IsDocLibExist(BATCHIMPORTFOLDER))
                {
                    //Get the XmlDocument from Document Library.
                    if (IsDocLibFileExist(BATCHIMPORTFOLDER, strFileName))
                    {
                        //returns the XML File
                        SPSecurity.RunWithElevatedPrivileges(delegate()
                        {
                            using (SPSite site = new SPSite(strCurrSiteURL))
                            {
                                using (SPWeb web = site.OpenWeb())
                                {
                                    spXMLFile = web.Folders[BATCHIMPORTFOLDER].Files[strFileName];
                                    if (spXMLFile != null)
                                    {
                                        objMemoryStream = new MemoryStream(spXMLFile.OpenBinary());
                                        xmlTextReader = new XmlTextReader(objMemoryStream);
                                        objXmlDoc = new XmlDocument();
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
                    throw new Exception(BATCHIMPORTFOLDER + " - Document Library not Found.");
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
        /// Determines whether [is doc lib exist] [the specified library name].
        /// </summary>
        /// <param name="libraryName">Name of the library.</param>
        /// <returns>
        /// 	<c>true</c> if [is doc lib exist] [the specified library name]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsDocLibExist(string libraryName)
        {
            bool blnIsDocLibExist = false;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(strCurrSiteURL))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            try
                            {
                                blnIsDocLibExist = web.Folders[libraryName].Exists;
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
        /// Determines whether [is doc lib file exist] [the specified library name].
        /// </summary>
        /// <param name="libraryName">Name of the library.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>
        /// 	<c>true</c> if [is doc lib file exist] [the specified library name]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsDocLibFileExist(string libraryName, string fileName)
        {
            bool blnIsDocLibFileExist = false;            
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
                                blnIsDocLibFileExist = web.GetFile(libraryName + "/" + fileName).Exists;
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
        #endregion
    }
}

#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: SaveSearchXMLGenerator.cs
#endregion

/// <summary> 
/// This class is used to create SaveSearch xml for a search.
/// </summary> 
using System;
using System.Collections;
using System.Xml;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.MOSSProcess;
using Shell.SharePoint.DREAM.Utilities;
using Microsoft.SharePoint;

namespace Shell.SharePoint.DREAM.Controller
{
    /// <summary>
    /// SaveSearchXMLGenerator class to generate the SaveSearch Request XML.
    /// </summary>
    public class SaveSearchXMLGenerator : Constants
    {        
        #region Private Methods
        /// <summary>
        /// Creates the save search outer XML.
        /// </summary>
        /// <returns>Save Search Outer XMLDocument.</returns>
        private XmlDocument CreateSaveSearchOuterXML()
        {
            XmlDocument objOuterXmlDocument = new XmlDocument();
            try
            {
                //creates the XmlElement.
                XmlElement xmlelementSaveSearch = objOuterXmlDocument.CreateElement(SAVESEARCHREQUESTS);
                objOuterXmlDocument.AppendChild(xmlelementSaveSearch);                
            }
            catch(Exception)
            {
                throw;
            }
            return objOuterXmlDocument;
        }

        /// <summary>
        /// Appends the inner to outer XML.
        /// </summary>
        /// <param name="outerXml">The outer XML.</param>
        /// <param name="innerXml">The inner XML.</param>
        /// <returns>Final outer Xml Document.</returns>
        /// <param name="saveSearchRequestInformation">The save search request object.</param>
        private XmlDocument AppendInnerToOuterXML(XmlDocument outerXml, XmlDocument innerXml, string searchName, string searchOrder, SaveSearchRequest saveSearchRequestInformation)
        {
            XmlDocument objXmlInnerDoc = new XmlDocument();
            try
            {
                XmlElement xmlelementSaveSearchChild = objXmlInnerDoc.CreateElement(SAVESEARCHREQUEST);
                objXmlInnerDoc.AppendChild(xmlelementSaveSearchChild);

                XmlNode xmlnodeSaveSearchChild = objXmlInnerDoc.DocumentElement;

                XmlAttribute xmlattributeName = objXmlInnerDoc.CreateAttribute(NAME);
                xmlelementSaveSearchChild.Attributes.Append(xmlattributeName);
                xmlattributeName.Value = searchName;

                XmlAttribute xmlattributeShared = objXmlInnerDoc.CreateAttribute(SHARED);
                xmlelementSaveSearchChild.Attributes.Append(xmlattributeShared);
                xmlattributeShared.Value = saveSearchRequestInformation.SaveTypeShared.ToString();

                XmlAttribute xmlattributeOrder = objXmlInnerDoc.CreateAttribute(ORDER);
                xmlelementSaveSearchChild.Attributes.Append(xmlattributeOrder);
                xmlattributeOrder.Value = searchOrder;
               
                XmlAttribute xmlattributeType = objXmlInnerDoc.CreateAttribute(TYPE);
                xmlelementSaveSearchChild.Attributes.Append(xmlattributeType);
                xmlattributeType.Value = saveSearchRequestInformation.Type;
                
                XmlDocumentFragment xmlDocInnerFrag = objXmlInnerDoc.CreateDocumentFragment();
                xmlDocInnerFrag.InnerXml = innerXml.OuterXml.ToString();
                XmlNode innerChildNode = objXmlInnerDoc.DocumentElement;
                innerChildNode.InsertAfter(xmlDocInnerFrag, innerChildNode.LastChild);

                XmlDocumentFragment xmlDocFrag = outerXml.CreateDocumentFragment();
                xmlDocFrag.InnerXml = objXmlInnerDoc.OuterXml.ToString();
                XmlNode childNode = outerXml.DocumentElement;
                childNode.InsertAfter(xmlDocFrag, childNode.LastChild);
            }
            catch(Exception)
            {
                throw;
            }
            return outerXml;
        }
        
        /// <summary>
        /// Saves the search.
        /// </summary>
        /// <param name="searchName">Name of the search.</param>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="userID">The user ID.</param>
        /// <param name="SearchRequestsXML">The search requests XML.</param>
        /// <param name="saveSearchRequestInformation">The save search request object.</param>
        public void SaveSearch(string searchName, string searchType, string userID, XmlDocument SearchRequestsXML, string searchOrder, SaveSearchRequest saveSearchRequestInformation)
        {
            try
            {
                //The commented code will be removed in next release
                //SPSecurity.RunWithElevatedPrivileges(delegate()
                // {
                     SaveSearchHandler objSaveSearchHandler = new SaveSearchHandler();
                     //creates the xmlDocument for save search.
                     XmlDocument objXmldocumentFinal = CreateSaveSearch(searchName, searchType, userID, SearchRequestsXML, searchOrder, saveSearchRequestInformation);
                     if (objXmldocumentFinal != null)
                         objSaveSearchHandler.UploadToDocumentLib(searchType, userID, objXmldocumentFinal);
                // });
            }
            catch (Exception)
            {
                throw;
            }
        }        
        /// <summary>
        /// Creates the save search.
        /// </summary>
        /// <param name="searchName">Name of the search.</param>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="userID">The user ID.</param>
        /// <param name="searchRequestXML">The search request XML.</param>
        /// <param name="saveSearchRequestInformation">The save search request object.</param>
        /// <returns></returns>
        private XmlDocument CreateSaveSearch(string searchName, string searchType, string userID, XmlDocument searchRequestXML, string searchOrder, SaveSearchRequest saveSearchRequestInformation)
        {
            ArrayList arlSaveOrderNumber = new ArrayList();
            XmlDocument objXmlFinalDoc = new XmlDocument();
            XmlDocument objXmlRootDoc = new XmlDocument();
            string strSaveSearchLimit = string.Empty;  
            SaveSearchHandler objSaveSearchHandler = new SaveSearchHandler();
            try
            {
                
                     strSaveSearchLimit = PortalConfiguration.GetInstance().GetKey("SaveSearchLimit");
                     if (!objSaveSearchHandler.IsDocLibFileExist(searchType, userID))
                     {
                         objXmlRootDoc = CreateSaveSearchOuterXML();
                         objXmlFinalDoc = AppendInnerToOuterXML(objXmlRootDoc, searchRequestXML, searchName, searchOrder, saveSearchRequestInformation);
                     }
                     else
                     {
                         arlSaveOrderNumber = objSaveSearchHandler.GetSaveOrderNumber(searchType, userID);
                         if (arlSaveOrderNumber.Contains(strSaveSearchLimit))
                         {
                             //throw Exception message : Save Search Criteria Exceeds the limit..
                             throw new Exception("You have exceeded the Save Search Criteria Limit. Please contact administrator.");
                         }
                         else
                         {
                             objXmlRootDoc = objSaveSearchHandler.GetDocLibXMLFile(searchType, userID);
                             if (objSaveSearchHandler.IsDuplicateExists(searchRequestXML, objXmlRootDoc))
                             {
                                 //throw Exception message : Duplicate Criteria name Exist..
                                 throw new Exception("Save Search criteria already exist. Please choose a different criteria.");
                             }
                             else
                             {
                                 objXmlFinalDoc = AppendInnerToOuterXML(objXmlRootDoc, searchRequestXML, searchName, searchOrder, saveSearchRequestInformation);
                             }
                         }
                     }//end if 
               
            }//end Try	     

            catch (Exception)
            {
                throw;
            }
            return objXmlFinalDoc;
        }
        #endregion
    }
}

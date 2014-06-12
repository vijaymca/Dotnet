#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: MyAssetXMLGenerator.cs
#endregion

/// <summary> 
/// This class is used to create a user specific My Asset xml. 
///
/// </summary> 
using System;
using System.Xml;
using Shell.SharePoint.DREAM.MOSSProcess;
using Shell.SharePoint.DREAM.Business.Entities;

namespace Shell.SharePoint.DREAM.Controller
{
    /// <summary>
    /// MyAssetXMLGenerator class
    /// </summary>
    public class MyAssetXMLGenerator : Constants
    {
        #region DECLARATION
        private XmlDocument objXmlDocument;
        private SaveSearchHandler objSaveSearchHandler = new SaveSearchHandler();
        #endregion

        #region XMLCREATION METHODS

        /// <summary>
        /// Saves my assets.
        /// </summary>
        /// <param name="bookMarks">The book marks.</param>
        /// <param name="userID">The user ID.</param>
        /// <returns></returns>
        public XmlDocument SaveMyAssets(BookMarks bookMarks, string identifier,string listName)
        {
            try
            {
                objXmlDocument = new XmlDocument();
                objXmlDocument = ReadMyAssetXML(identifier, listName);
                
                if (objXmlDocument.DocumentElement != null)
                {
                    // Xml File saved earlier
                    objXmlDocument = UpdateMyAssetXML(bookMarks);

                }
                else
                {
                    //Calling the Create Xml File method.
                    objXmlDocument = CreateMyAssetXML(bookMarks);
                }
                objSaveSearchHandler.UploadToDocumentLib(listName, identifier, objXmlDocument);
            }
            catch (Exception)
            {
                throw;
            }
            return objXmlDocument;
        }


        

        /// <summary>
        /// Updates the existing My Asset XML
        /// </summary>
        /// <param name="objBookMarks"></param>
        /// <returns></returns>
        private XmlDocument UpdateMyAssetXML(BookMarks bookMarks)
        {
            try
            {
                XmlNode BookMarksNode = objXmlDocument.DocumentElement;
                XmlNode BookMarkNode = null;

                foreach (BookMark objBookMark in bookMarks.BookMark)
                {
                    switch (objBookMark.BookMarkType.ToLower())
                    {
                        case ASSETWELL:
                            {
                                BookMarkNode = BookMarksNode.SelectSingleNode("BookMark[@type='Well']");
                                break;
                            }
                        case ASSETWELLBORE:
                            {
                                BookMarkNode = BookMarksNode.SelectSingleNode("BookMark[@type='Wellbore']");
                                break;
                            }
                        case ASSETBASIN:
                            {
                                BookMarkNode = BookMarksNode.SelectSingleNode("BookMark[@type='Basin']");
                                break;
                            }
                        case ASSETFIELD:
                            {
                                BookMarkNode = BookMarksNode.SelectSingleNode("BookMark[@type='Field']");
                                break;
                            }
                        case ASSETPARS:
                            {
                                BookMarkNode = BookMarksNode.SelectSingleNode("BookMark[@type='Project Archives']");
                                break;
                            }
                        case ASSETLOGSBYFIELD:
                            {
                                BookMarkNode = BookMarksNode.SelectSingleNode("BookMark[@type='Logs By Field Depth']");
                                break;
                            }
                        case ASSETRESERVOIR:
                            {
                                BookMarkNode = BookMarksNode.SelectSingleNode("BookMark[@type='Reservoir']");
                                break;
                            }
                    }

                    if (objBookMark.Value != null)
                    {

                        if (BookMarkNode == null)
                        {
                            BookMarkNode = CreateBookMark(bookMarks, BookMarksNode);
                        }
                        else
                        {
                            CreateBookMarkValue(objBookMark, BookMarkNode);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return objXmlDocument;
        }

        /// <summary>
        /// Builds the My Asset XML
        /// </summary>
        /// <param name="objBookMarks"></param>
        /// <returns></returns>
        private XmlDocument CreateMyAssetXML(BookMarks bookMarks)
        {
            try
            {
                return CreateRootElement(bookMarks);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Builds the My Asset XML
        /// </summary>
        /// <param name="objbookMarks"></param>
        /// <returns></returns>
        private XmlDocument CreateRootElement(BookMarks bookMarks)
        {
            try
            {
                //Creating the root Xml Element BookMarks.
                XmlElement BookMarksElement = objXmlDocument.CreateElement(BOOKMARKS);
                objXmlDocument.AppendChild(BookMarksElement);

                XmlNode BookMarksNode = objXmlDocument.DocumentElement;

                if (bookMarks.BookMark != null)
                {
                    CreateBookMark(bookMarks, BookMarksNode);
                }
            }
            catch (Exception)
            {
                throw;
            }
            //returns the final My Asset Xml.
            return objXmlDocument;
        }

        /// <summary>
        /// Recursively creates Value nodes in the respective BookMark Node
        /// </summary>
        /// <param name="bookMark"></param>
        /// <param name="BookMarkNode"></param>
        private void CreateBookMarkValue(BookMark bookMark, XmlNode bookMarkNode)
        {
            try
            {
                if (bookMark.Value != null)
                {
                    XmlNode rootNode = objXmlDocument.DocumentElement;

                    foreach (Value objValue in bookMark.Value)
                    {
                        XmlNode ValueNode = rootNode.SelectSingleNode("BookMark[value='" + objValue.InnerText + "']");
                        if (ValueNode == null)
                        {
                            XmlElement BookMarkValueElement = objXmlDocument.CreateElement(VALUE);
                            XmlNode BookMarkValueNode = bookMarkNode.AppendChild(BookMarkValueElement);
                            BookMarkValueNode.InnerText = objValue.InnerText;
                        }
                    }
                }
            }
            catch 
            {
                throw;
            }
        }

        /// <summary>
        /// Creates BookMark node in the My Asset XML
        /// </summary>
        /// <param name="objbookMarks"></param>
        /// <param name="BookMarksNode"></param>
        /// <returns></returns>
        private XmlNode CreateBookMark(BookMarks bookMarks,XmlNode bookMarksNode)
        {
            XmlNode BookMarkNode = null;
            string strType = string.Empty;
            string strIdentifierName = string.Empty;

            try
            {
                foreach (BookMark objBookMark in bookMarks.BookMark)
                {
                    strType = objBookMark.BookMarkType;
                    strIdentifierName = objBookMark.IdentifierName;
                    //Method call to create BookMark Node.
                    XmlElement BookMarkElement = objXmlDocument.CreateElement(BOOKMARK);
                    ///create attribute for BookMark node
                    XmlAttribute BookMarkTypeAttribute = objXmlDocument.CreateAttribute(TYPE);
                    strType = strType.Replace("_", " ");
                    BookMarkTypeAttribute.InnerText = strType;
                    BookMarkElement.Attributes.Append(BookMarkTypeAttribute);
                    XmlAttribute BookMarkNameAttribute = objXmlDocument.CreateAttribute(IDENTIFIERNAME);
                    BookMarkNameAttribute.InnerText = strIdentifierName;
                    BookMarkElement.Attributes.Append(BookMarkNameAttribute);

                    BookMarkNode = SetNodeOrder(objBookMark.BookMarkType, BookMarkNode, bookMarksNode, BookMarkElement);
                    ///reset the child nodes.
                    CreateBookMarkValue(objBookMark, BookMarkNode);
                }
                return BookMarkNode;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Applies ordering to the BookMark nodes in My Asset XML - Well, Wellbore, Basin, Field, PARS
        /// </summary>
        /// <param name="strType"></param>
        /// <param name="BookMarkNode"></param>
        /// <param name="BookMarksNode"></param>
        /// <param name="BookMarkElement"></param>
        /// <returns></returns>
        private XmlNode SetNodeOrder(string type, XmlNode bookMarkNode, XmlNode bookMarksNode, XmlElement bookMarkElement)
        {
            try
            {
                XmlNode wellBookMark = bookMarksNode.SelectSingleNode("BookMark[@type='Well']");
                XmlNode wellboreBookMark = bookMarksNode.SelectSingleNode("BookMark[@type='Wellbore']");
                XmlNode basinBookMark = bookMarksNode.SelectSingleNode("BookMark[@type='Basin']");
                XmlNode fieldBookMark = bookMarksNode.SelectSingleNode("BookMark[@type='Field']");
                XmlNode parsBookMark = bookMarksNode.SelectSingleNode("BookMark[@type='Project Archives']");
                XmlNode logsBookMark = bookMarksNode.SelectSingleNode("BookMark[@type='Logs By Field']");


                //Order the elements in the XML
                switch (type.ToLower())
                {
                    case ASSETWELL:
                        {
                            if (wellboreBookMark != null)
                                bookMarkNode = bookMarksNode.InsertBefore(bookMarkElement, wellboreBookMark);
                            else
                                if (basinBookMark != null)
                                    bookMarkNode = bookMarksNode.InsertBefore(bookMarkElement, basinBookMark);
                                else
                                    if (fieldBookMark != null)
                                        bookMarkNode = bookMarksNode.InsertBefore(bookMarkElement, fieldBookMark);
                                    else
                                        if (parsBookMark != null)
                                            bookMarkNode = bookMarksNode.InsertBefore(bookMarkElement, parsBookMark);
                                        else
                                            if (logsBookMark != null)
                                                bookMarkNode = bookMarksNode.InsertBefore(bookMarkElement, logsBookMark);
                                            else
                                                bookMarkNode = bookMarksNode.AppendChild(bookMarkElement);
                            break;
                        }
                    case ASSETWELLBORE:
                        {
                            if (wellBookMark != null)
                                bookMarkNode = bookMarksNode.InsertAfter(bookMarkElement, wellBookMark);
                            else
                                if (basinBookMark != null)
                                    bookMarkNode = bookMarksNode.InsertBefore(bookMarkElement, basinBookMark);
                                else
                                    if (fieldBookMark != null)
                                        bookMarkNode = bookMarksNode.InsertBefore(bookMarkElement, fieldBookMark);
                                    else
                                        if (parsBookMark != null)
                                            bookMarkNode = bookMarksNode.InsertBefore(bookMarkElement, parsBookMark);
                                        else
                                            if (logsBookMark != null)
                                                bookMarkNode = bookMarksNode.InsertBefore(bookMarkElement, logsBookMark);
                                            else
                                                bookMarkNode = bookMarksNode.AppendChild(bookMarkElement);
                            break;
                        }
                    case ASSETBASIN:
                        {
                            if (fieldBookMark != null)
                                bookMarkNode = bookMarksNode.InsertBefore(bookMarkElement, fieldBookMark);
                            else
                                if (parsBookMark != null)
                                    bookMarkNode = bookMarksNode.InsertBefore(bookMarkElement, parsBookMark);
                                else
                                    if (logsBookMark != null)
                                        bookMarkNode = bookMarksNode.InsertBefore(bookMarkElement, logsBookMark);
                                    else
                                        bookMarkNode = bookMarksNode.AppendChild(bookMarkElement);
                            break;
                        }
                    case ASSETFIELD:
                        {
                            if (parsBookMark != null)
                                bookMarkNode = bookMarksNode.InsertBefore(bookMarkElement, parsBookMark);
                            else
                                if (logsBookMark != null)
                                    bookMarkNode = bookMarksNode.InsertBefore(bookMarkElement, logsBookMark);
                                else
                                    bookMarkNode = bookMarksNode.AppendChild(bookMarkElement);
                            break;
                        }
                    case ASSETPARS:
                        {
                            if (logsBookMark != null)
                                bookMarkNode = bookMarksNode.InsertBefore(bookMarkElement, logsBookMark);
                            else
                                bookMarkNode = bookMarksNode.AppendChild(bookMarkElement);
                            break;
                        }
                    default:
                        {
                            bookMarkNode = bookMarksNode.AppendChild(bookMarkElement);
                            break;
                        }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return bookMarkNode;
        }


        /// <summary>
        /// Reads my asset XML.
        /// </summary>
        /// <param name="strIdentifier">The STR identifier.</param>
        /// <param name="listName">Name of the list.</param>
        /// <returns></returns>
        public XmlDocument ReadMyAssetXML(string identifier, string listName)
        {
            XmlDocument objXmlDocument = null;
            try
            {
                objXmlDocument = objSaveSearchHandler.GetDocLibXMLFile(listName, identifier);
                return objXmlDocument;
            }
            catch
            {
                throw;
            }
        }



        /// <summary>
        /// Uploads my asset XML.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="myAssetXML">My asset XML.</param>
        /// <param name="listName">Name of the list.</param>
        public void UploadMyAssetXML(string identifier,XmlDocument myAssetXML, string listName)
        {          
            try
            {
                if (myAssetXML != null)
                {
                    objSaveSearchHandler.UploadToDocumentLib(listName, identifier, myAssetXML);
                }                
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Deletes my asset XML.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="listName">Name of the list.</param>
        public void DeleteMyAssetXML(string identifier,string listName)
        {
            try
            {
                objSaveSearchHandler.DeleteFromDocumentLib(listName, identifier);              
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}

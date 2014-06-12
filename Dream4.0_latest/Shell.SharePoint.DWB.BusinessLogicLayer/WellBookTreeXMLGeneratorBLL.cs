#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: WellBookTreeXMLGeneratorBLL.cs
#endregion

using System.Collections;
using System.Xml;
using Shell.SharePoint.DWB.Business.DataObjects;

namespace Shell.SharePoint.DWB.BusinessLogicLayer
{
    /// <summary>
    /// WellBookTreeXMLGeneratorBLL class.
    /// </summary>
    public class WellBookTreeXMLGeneratorBLL
    {
        #region DECLARATION
        XmlDocument objXmlDocument;
        #endregion

        #region XMLCREATION METHODS
        /// <summary>
        /// Creates the list viewer XML.
        /// </summary>
        /// <param name="records">The records.</param>
        /// <returns></returns>
        public XmlDocument CreateWellBookTreeXML(BookInfo bookInfo)
        {
            if (bookInfo != null)
            {
                objXmlDocument = new XmlDocument();
                objXmlDocument = CreateRootElement(bookInfo);
            }
            return objXmlDocument;
        }

        /// <summary>
        /// Creates the root element.
        /// </summary>
        /// <param name="objRequestInfo">The request info object.</param>
        /// <returns></returns>
        private XmlDocument CreateRootElement(BookInfo bookInfo)
        {

            //Creating the root Xml Element BookInfo.
            XmlElement BookElement = objXmlDocument.CreateElement("BookInfo");
            objXmlDocument.AppendChild(BookElement);

            //Creating the BookInfo node with Name attribute.
            XmlNode BookNode = objXmlDocument.DocumentElement;

            if (bookInfo.Chapters != null)
            {
                //Method call to create Chapters Node.
                CreateChapter(bookInfo, BookNode);
            }
            if (bookInfo.BookID.Length > 0)
            {
                XmlAttribute BookID = objXmlDocument.CreateAttribute("BookID");
                BookElement.Attributes.Append(BookID);
                BookID.Value = bookInfo.BookID;
            }
            if (bookInfo.BookName.Length > 0)
            {
                XmlAttribute BookName = objXmlDocument.CreateAttribute("BookName");
                BookElement.Attributes.Append(BookName);
                BookName.Value = bookInfo.BookName;
            }
            if (bookInfo.BookOwner.Length > 0)
            {
                XmlAttribute BookOwner = objXmlDocument.CreateAttribute("BookOwner");
                BookElement.Attributes.Append(BookOwner);
                BookOwner.Value = bookInfo.BookOwner;
            }
            if (bookInfo.PageCount > 0)
            {
                XmlAttribute PageCount = objXmlDocument.CreateAttribute("PageCount");
                BookElement.Attributes.Append(PageCount);
                PageCount.Value = bookInfo.PageCount.ToString();
            }
            if (bookInfo.Action.Length > 0)
            {
                XmlAttribute Action = objXmlDocument.CreateAttribute("Type");
                BookElement.Attributes.Append(Action);
                Action.Value = bookInfo.Action;
            }
            XmlAttribute IsPrintable = objXmlDocument.CreateAttribute("IsPrintable");
            XmlAttribute IsTOCApplicable = objXmlDocument.CreateAttribute("IsTOCApplicable");
            XmlAttribute IsStoryBoardApplicable = objXmlDocument.CreateAttribute("IsStoryBoardApplicable");
            /// Added for Print Options CR Implementation
            XmlAttribute IsPageTitleApplicable = objXmlDocument.CreateAttribute("IsPageTitleApplicable");
            XmlAttribute IsBookTitleApplicable = objXmlDocument.CreateAttribute("IsBookTitleApplicable");

            BookElement.Attributes.Append(IsPrintable);
            BookElement.Attributes.Append(IsTOCApplicable);
            BookElement.Attributes.Append(IsStoryBoardApplicable);
            /// Added for Print Options CR Implementation
            BookElement.Attributes.Append(IsPageTitleApplicable);
            BookElement.Attributes.Append(IsBookTitleApplicable);
            IsPrintable.Value = bookInfo.IsPrintable.ToString();
            IsTOCApplicable.Value = bookInfo.IsTOCApplicatble.ToString();
            IsStoryBoardApplicable.Value = bookInfo.IsStoryBoardApplicable.ToString();
            /// Added for Print Options CR Implementation
            IsPageTitleApplicable.Value = bookInfo.IsPageTitleApplicable.ToString();
            IsBookTitleApplicable.Value = bookInfo.IsBookTitleApplicable.ToString();
            //returns the final SearchRequest Xml.
            return objXmlDocument;
        }

        /// <summary>
        /// Creates the entity.
        /// </summary>
        /// <param name="objRequestInfo">The requestinfo object.</param>
        /// <param name="EntitiesElement">The entities element.</param>
        private void CreateChapter(BookInfo bookInfo, XmlNode bookNode)
        {
            XmlElement ChapterElement;

            foreach (ChapterInfo objChapter in bookInfo.Chapters)
            {
                ChapterElement = objXmlDocument.CreateElement("Chapter");
                bookNode.AppendChild(ChapterElement);
                if (objChapter.AssetType.Length > 0)
                {
                    XmlAttribute AssetAttribute = objXmlDocument.CreateAttribute("AssetType");
                    ChapterElement.Attributes.Append(AssetAttribute);
                    AssetAttribute.Value = objChapter.AssetType;
                }
                if (objChapter.AssetValue.Length > 0)
                {
                    XmlAttribute AssetValueAttribute = objXmlDocument.CreateAttribute("AssetValue");
                    ChapterElement.Attributes.Append(AssetValueAttribute);
                    AssetValueAttribute.Value = objChapter.AssetValue;
                }
                if (objChapter.ChapterID.Length > 0)
                {
                    XmlAttribute ChapterIDAttribute = objXmlDocument.CreateAttribute("ChapterID");
                    ChapterElement.Attributes.Append(ChapterIDAttribute);
                    ChapterIDAttribute.Value = objChapter.ChapterID;
                }
                if (objChapter.ChapterTitle.Length > 0)
                {
                    XmlAttribute ChapterTitleAttribute = objXmlDocument.CreateAttribute("ChapterTitle");
                    ChapterElement.Attributes.Append(ChapterTitleAttribute);
                    ChapterTitleAttribute.Value = objChapter.ChapterTitle;
                }
                if (objChapter.ActualAssetValue.Length > 0)
                {
                    XmlAttribute ActualAssetValue = objXmlDocument.CreateAttribute("ActualAssetValue");
                    ChapterElement.Attributes.Append(ActualAssetValue);
                    ActualAssetValue.Value = objChapter.ActualAssetValue;
                }
                XmlAttribute IsPrintable = objXmlDocument.CreateAttribute("IsPrintable");
                ChapterElement.Attributes.Append(IsPrintable);
                IsPrintable.Value = objChapter.IsPrintable.ToString();
                if (objChapter.PageInfo != null)
                {
                    // if(string.Equals(objChapter.ChapterID, chapterID))
                    CreatePageInfo(objChapter.PageInfo, ChapterElement);
                }
                #region DREAM 4.0 - eWB2.0 - Customise Chapters
                XmlAttribute Display = objXmlDocument.CreateAttribute("Display");
                ChapterElement.Attributes.Append(Display);
                Display.Value = objChapter.Display.ToString();
                #endregion
            }

        }

        /// <summary>
        /// Creates the PageInfo DataObject.
        /// </summary>
        /// <param name="pageInfo">ArrayList.</param>
        /// <param name="chapterElement">XmlElement.</param>
        private void CreatePageInfo(ArrayList pageInfo, XmlElement chapterElement)
        {
            foreach (PageInfo objPageInfo in pageInfo)
            {
                XmlElement PageInfoElement = objXmlDocument.CreateElement("PageInfo");
                chapterElement.AppendChild(PageInfoElement);

                //Creating attributes for Attribute Element.
                if (objPageInfo.PageID.Length > 0)
                {
                    XmlAttribute PageID = objXmlDocument.CreateAttribute("PageID");
                    PageInfoElement.Attributes.Append(PageID);
                    PageID.Value = objPageInfo.PageID;
                }

                if (objPageInfo.PageTitle.Length > 0)
                {
                    XmlAttribute PageTitle = objXmlDocument.CreateAttribute("PageTitle");
                    PageInfoElement.Attributes.Append(PageTitle);
                    PageTitle.Value = objPageInfo.PageTitle;
                }
                if (objPageInfo.PageActualName.Length > 0)
                {
                    XmlAttribute Title = objXmlDocument.CreateAttribute("Title");
                    PageInfoElement.Attributes.Append(Title);
                    Title.Value = objPageInfo.PageActualName;
                }
                if (objPageInfo.PageURL.Length > 0)
                {
                    XmlAttribute PageURL = objXmlDocument.CreateAttribute("PageURL");
                    PageInfoElement.Attributes.Append(PageURL);
                    PageURL.Value = objPageInfo.PageURL;
                }
                if (objPageInfo.PageOwner.Length > 0)
                {
                    XmlAttribute PageOwner = objXmlDocument.CreateAttribute("PageOwner");
                    PageInfoElement.Attributes.Append(PageOwner);
                    PageOwner.Value = objPageInfo.PageOwner;
                }
                if (objPageInfo.SignOffStatus.Length > 0)
                {
                    XmlAttribute SignOffStatus = objXmlDocument.CreateAttribute("SignOffStatus");
                    PageInfoElement.Attributes.Append(SignOffStatus);
                    SignOffStatus.Value = objPageInfo.SignOffStatus;
                }
                if (objPageInfo.ConnectionType != 0)
                {
                    XmlAttribute ConnectionType = objXmlDocument.CreateAttribute("ConnectionType");
                    PageInfoElement.Attributes.Append(ConnectionType);
                    ConnectionType.Value = objPageInfo.ConnectionType.ToString();
                }
                if (objPageInfo.AssetType.Length > 0)
                {
                    XmlAttribute AssetType = objXmlDocument.CreateAttribute("AssetType");
                    PageInfoElement.Attributes.Append(AssetType);
                    AssetType.Value = objPageInfo.AssetType;
                }
                if (objPageInfo.ActualAssetValue.Length > 0)
                {
                    XmlAttribute ActualAssetType = objXmlDocument.CreateAttribute("AssetValue");
                    PageInfoElement.Attributes.Append(ActualAssetType);
                    ActualAssetType.Value = objPageInfo.ActualAssetValue;
                }
                if (objPageInfo.ColumnName.Length > 0)
                {
                    XmlAttribute ColumnName = objXmlDocument.CreateAttribute("ActualColumnname");
                    PageInfoElement.Attributes.Append(ColumnName);
                    ColumnName.Value = objPageInfo.ColumnName;
                }
                if (objPageInfo.ReportName.Length > 0)
                {
                    XmlAttribute ReportName = objXmlDocument.CreateAttribute("ReportName");
                    PageInfoElement.Attributes.Append(ReportName);
                    ReportName.Value = objPageInfo.ReportName;
                }
                
                 //Added by Praveena for module "Add Last Updated date"
                if (objPageInfo.LastUpdatedDate.Length > 0)
                {
                    XmlAttribute LastUpdatedDate = objXmlDocument.CreateAttribute("LastUpdatedDate");
                    PageInfoElement.Attributes.Append(LastUpdatedDate);
                    LastUpdatedDate.Value = objPageInfo.LastUpdatedDate;
                }
                
                #region DREAM 4.0 - eWB2.0 - Hide/Reveal Empty Pages
                XmlAttribute IsEmpty = objXmlDocument.CreateAttribute("IsEmpty");
                PageInfoElement.Attributes.Append(IsEmpty);
                IsEmpty.Value = objPageInfo.IsEmpty.ToString();
                #endregion
            }
        }
        #endregion
    }
}

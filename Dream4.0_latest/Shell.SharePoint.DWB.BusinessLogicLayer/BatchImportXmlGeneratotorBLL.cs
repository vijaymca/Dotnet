#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: BatchImportXmlGeneratotorBLL.cs
#endregion
using System.Xml;
using Shell.SharePoint.DWB.Business.DataObjects;

namespace Shell.SharePoint.DWB.BusinessLogicLayer
{
    /// <summary>
    /// This class is used to generate the Batch Import configuration XML.
    /// </summary>
    public class BatchImportXmlGeneratotorBLL
    {
        #region DECLARATION
        XmlDocument objXmlDocument;
        const string BATCHIMPORTS = "batchImports";
        const string BATCHIMPORT = "batchImport";
        const string BOOKID = "BookID";
        const string DEFAULTPATH = "DefaultSharedPath";
        const string USERNAME = "UserName";
        const string PAGENAME = "pageName";
        const string NAME = "name";
        const string PAGECOUNT = "PageCount";
        const string FILETYPE = "fileType";
        const string TYPE = "type";
        const string FILEFORMAT = "fileFormat";
        const string FORMAT = "format";
        const string ACTUALFORMAT = "actualFormat";
        const string SHAREDPATH = "sharedPath";
        const string PATH = "path";
        #endregion
        /// <summary>
        /// Creates the batch import XML.
        /// </summary>
        /// <param name="batchImports">The batch imports.</param>
        /// <returns></returns>
        public XmlDocument CreateBatchImportXML(BatchImports batchImports)
        {
            if (batchImports != null)
            {
                objXmlDocument = new XmlDocument();
                objXmlDocument = CreateRootElement(batchImports);
            }
            return objXmlDocument;
        }

        /// <summary>
        /// Creates the root element.
        /// </summary>
        /// <param name="batchImports">The batch imports.</param>
        /// <returns></returns>
        private XmlDocument CreateRootElement(BatchImports batchImports)
        {
            //Creating the root Xml Element Batch Imports.
            XmlElement BatchImportsElement = objXmlDocument.CreateElement(BATCHIMPORTS);
            objXmlDocument.AppendChild(BatchImportsElement);

            //Creating the RequestInfo node with Name attribute.
            XmlNode BatchImportsNode = objXmlDocument.DocumentElement;
            if (batchImports.BatchImport != null)
            {
                //Method call to create Batch import.
                CreateBatchImport(batchImports, BatchImportsNode);
            }
            else
                objXmlDocument = null;

            //returns the final SearchRequest Xml.
            return objXmlDocument;
        }

        /// <summary>
        /// Creates the batch import.
        /// </summary>
        /// <param name="batchImports">The batch imports.</param>
        /// <param name="BatchImportsNode">The batch imports node.</param>
        private void CreateBatchImport(BatchImports batchImports, XmlNode BatchImportsNode)
        {
            XmlElement BatchImportElement;

            foreach (BatchImport objBatchImport in batchImports.BatchImport)
            {
                BatchImportElement = objXmlDocument.CreateElement(BATCHIMPORT);
                BatchImportsNode.AppendChild(BatchImportElement);
                if (objBatchImport.BookID.Length > 0)
                {
                    XmlAttribute BookIDAttribute = objXmlDocument.CreateAttribute(BOOKID);
                    BatchImportElement.Attributes.Append(BookIDAttribute);
                    BookIDAttribute.Value = objBatchImport.BookID;
                }
                if (objBatchImport.DefaultSharedPath.Length > 0)
                {
                    XmlAttribute DefaultPathAttribute = objXmlDocument.CreateAttribute(DEFAULTPATH);
                    BatchImportElement.Attributes.Append(DefaultPathAttribute);
                    DefaultPathAttribute.Value = objBatchImport.DefaultSharedPath;
                }
                if (objBatchImport.UserName.Length > 0)
                {
                    XmlAttribute UserNameAttribute = objXmlDocument.CreateAttribute(USERNAME);
                    BatchImportElement.Attributes.Append(UserNameAttribute);
                    UserNameAttribute.Value = objBatchImport.UserName;
                }
                foreach (PageName objPageName in objBatchImport.PageName)
                {
                    if (objPageName != null)
                    {
                        CreatePageName(objPageName, BatchImportElement);
                    }
                }
            }
        }

        /// <summary>
        /// Creates the name of the page.
        /// </summary>
        /// <param name="arrayList">The array list.</param>
        /// <param name="BatchImportElement">The batch import element.</param>
        private void CreatePageName(PageName pageName, XmlElement BatchImportElement)
        {
            XmlElement PageNameElement = objXmlDocument.CreateElement(PAGENAME);
            BatchImportElement.AppendChild(PageNameElement);

            //Creating name attribute for PageName node.
            if (pageName.Name.ToString().Length > 0)
            {
                XmlAttribute Name = objXmlDocument.CreateAttribute(NAME);
                PageNameElement.Attributes.Append(Name);
                Name.Value = pageName.Name;
            }

            //Creating name attribute for PageName node.
            if (pageName.PageCount.ToString().Length > 0)
            {
                XmlAttribute PageCount = objXmlDocument.CreateAttribute(PAGECOUNT);
                PageNameElement.Attributes.Append(PageCount);
                PageCount.Value = pageName.PageCount;
            }

            //Creating Shared Path element for PageName node.
            if (pageName.SharedPath != null)
            {
                CreateSharedPath(pageName.SharedPath, PageNameElement);
            }
            //Creating Shared Path element for PageName node.
            if (pageName.FileFormat != null)
            {
                CreateFileFormat(pageName.FileFormat, PageNameElement);
            }
            //Creating Shared Path element for PageName node.
            if (pageName.FileType != null)
            {
                CreateFileType(pageName.FileType, PageNameElement);
            }
        }

        /// <summary>
        /// Creates the type of the file.
        /// </summary>
        /// <param name="fileType">Type of the file.</param>
        /// <param name="PageNameElement">The page name element.</param>
        private void CreateFileType(FileType fileType, XmlElement PageNameElement)
        {
            XmlElement FileTypeElement = objXmlDocument.CreateElement(FILETYPE);
            PageNameElement.AppendChild(FileTypeElement);

            //Creating type attribute for filetype node.
            if (fileType.Type.ToString().Length > 0)
            {
                XmlAttribute Type = objXmlDocument.CreateAttribute(TYPE);
                FileTypeElement.Attributes.Append(Type);
                Type.Value = fileType.Type;
            }
        }

        /// <summary>
        /// Creates the file format.
        /// </summary>
        /// <param name="fileFormat">The file format.</param>
        /// <param name="PageNameElement">The page name element.</param>
        private void CreateFileFormat(FileFormat fileFormat, XmlElement PageNameElement)
        {
            XmlElement FileFormatElement = objXmlDocument.CreateElement(FILEFORMAT);
            PageNameElement.AppendChild(FileFormatElement);

            //Creating type attribute for filetype node.
            if (fileFormat.Format.ToString().Length > 0)
            {
                XmlAttribute Format = objXmlDocument.CreateAttribute(FORMAT);
                FileFormatElement.Attributes.Append(Format);
                Format.Value = fileFormat.Format;
            }
            //Creating actual format attribute for filetype node.
            if (fileFormat.ActualFormat.ToString().Length > 0)
            {
                XmlAttribute ActualFormat = objXmlDocument.CreateAttribute(ACTUALFORMAT);
                FileFormatElement.Attributes.Append(ActualFormat);
                ActualFormat.Value = fileFormat.ActualFormat;
            }
        }

        /// <summary>
        /// Creates the shared path.
        /// </summary>
        /// <param name="sharedPath">The shared path.</param>
        /// <param name="PageNameElement">The page name element.</param>
        private void CreateSharedPath(SharedPath sharedPath, XmlElement PageNameElement)
        {
            XmlElement SharedPathElement = objXmlDocument.CreateElement(SHAREDPATH);
            PageNameElement.AppendChild(SharedPathElement);

            //Creating type attribute for filetype node.
            if (sharedPath.Path.ToString().Length > 0)
            {
                XmlAttribute Path = objXmlDocument.CreateAttribute(PATH);
                SharedPathElement.Attributes.Append(Path);
                Path.Value = sharedPath.Path;
            }
        }
    }
}

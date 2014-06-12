#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: BookDetails.cs
#endregion
using System;
using Winnovative.PdfCreator;
using Shell.SharePoint.DWB.BusinessLogicLayer;
using System.Xml.XPath;

using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.WebService.DWB.PdfGenerate
{
    /// <summary>
    /// The BookDetails Class is used to Initaie the Different Aspect of PDF generation 
    /// Related to a Book
    /// </summary>
    public class BookDetails
    {
        #region Variables
        private int intResult;
        private String strtitle;
        private Document objPDF;
        private PdfBLL objPdfBLL;
        private Boolean blnIsTOCApplicable;
        private Boolean blnIsStoryBoardApplicable;
        bool blnIsPageTitleApplicable;
        private PDFHelper objPdfhelper;
        private WellBookTOC objBookTOC;
        private String strLibraryName;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the name of the library.
        /// </summary>
        /// <value>The name of the STR library.</value>
        public String LibraryName
        {
            get { return strLibraryName; }
            set { strLibraryName = value; }
        }
        /// <summary>
        /// Gets or sets the PDF document .
        /// </summary>
        /// <value>The doc PDF.</value>
        public Document PDFDocument
        {
            get { return objPDF; }
            set { objPDF = value; }
        }
        /// <summary>
        /// Gets or sets the booktitle.
        /// </summary>
        /// <value>The booktitle.</value>
        public String Bookitle
        {
            get { return strtitle; }
            set { strtitle = value; }
        }
        #endregion

        #region Public Method
        /// <summary>
        /// Generates the book.
        /// </summary>
        /// <param name="xmldocCriteria">The xmldoc criteria.</param>
        public int GenerateBook(XPathDocument xpDocument, String strContext)
        {           
            try
            {                
                LicensingManager.LicenseKey = PortalConfiguration.GetInstance().FindWebServiceKey("WinnovativeLicenseKey", strContext,true);
                objPdfhelper = new PDFHelper();
                objPdfBLL = new PdfBLL();
                objBookTOC = new WellBookTOC();
                blnIsTOCApplicable = new Boolean();
                ///Once Book Title is obtained get the First Page of PDF ready with Book Title and 
                ///Current Date and time Below it.
                PDFDocument = new Document();
                               
                LibraryName = objPdfBLL.GetAttributeBookInfo(xpDocument, "Type");
                if (LibraryName.Equals("pdf"))
                    LibraryName = Constant.LISTPUBLISHED;
                else if (LibraryName.Equals("print"))
                    LibraryName = Constant.LISTPRINTED;
                blnIsTOCApplicable = Convert.ToBoolean(objPdfBLL.GetAttributeBookInfo(xpDocument, "IsTOCApplicable"));
                blnIsStoryBoardApplicable = Convert.ToBoolean(objPdfBLL.GetAttributeBookInfo(xpDocument, "IsStoryBoardApplicable"));
                blnIsPageTitleApplicable = Convert.ToBoolean(objPdfBLL.GetAttributeBookInfo(xpDocument, "IsPageTitleApplicable"));
                objPdfhelper.PrintLog(strContext, "PDF generation for {" + objPdfBLL.GetAttributeBookInfo(xpDocument, "BookName") + " }is Started.Calling GenerateTOC to generate PDF", "BookDetails.GenerateBook", "BeforeUploading");

                objBookTOC.GenerateTOC(PDFDocument, xpDocument, strContext, blnIsTOCApplicable, blnIsStoryBoardApplicable, blnIsPageTitleApplicable);
                if ((Convert.ToBoolean(objPdfBLL.GetAttributeBookInfo(xpDocument, "IsBookTitleApplicable"))))
                {
                    objPdfhelper.AddBookTitle(PDFDocument, xpDocument, blnIsTOCApplicable, LibraryName);
                }

                PDFDocument.CompressionLevel = CompressionLevel.BestCompression;
                objPdfhelper.PrintLog(strContext, "PDF for {" + objPdfBLL.GetAttributeBookInfo(xpDocument, "BookName") + " }is generated.Calling UploadPublishedDocument to upload to document library", "BookDetails.GenerateBook", "BeforeUploading");

                intResult = objPdfBLL.UploadPublishedDocument(strContext, objPdfBLL.GetAttributeBookInfo(xpDocument, "BookID"), PDFDocument, objPdfBLL.GetAttributeBookInfo(xpDocument, "BookName"), LibraryName);
                if (intResult > 0)
                {
                    objPdfhelper.PrintLog(strContext, "PDF for {" + objPdfBLL.GetAttributeBookInfo(xpDocument, "BookName") + "} is uploaded successfully.", "BookDetails.GenerateBook", "AfterUploading");

                }
                else
                {
                    objPdfhelper.PrintLog(strContext, "PDF for {" + objPdfBLL.GetAttributeBookInfo(xpDocument, "BookName") + "} is NOT uploaded successfully.", "BookDetails.GenerateBook", "AfterUploading");

                }
                return intResult;
            }
            catch (Exception ex)
            {
                objPdfhelper.PrintLog(strContext, ex.Message, "BookDetails.GenerateBook", "Exception");
                objPdfhelper.PrintLog(strContext, ex.StackTrace, "BookDetails.GenerateBook", "Exception");
                throw;
            }
            finally
            {
                PDFDocument.Close();
            }
        }

        #endregion
    }
}

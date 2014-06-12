#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: PDFHelper.cs
#endregion

using System;
using System.Collections;
using System.IO;
using System.Web;
using System.Xml.XPath;
using System.Xml;

using Microsoft.SharePoint;

using Shell.SharePoint.DWB.BusinessLogicLayer;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DREAM.Controller;

using Winnovative.PdfCreator;

using System.Diagnostics;

namespace Shell.SharePoint.WebService.DWB.PdfGenerate
{
    /// <summary>
    /// PDF Helpper Class
    /// </summary>
    public class PDFHelper
    {
        #region Variables
        private Attributes objAttribute;
        private ArrayList arlAttribute;
        private ArrayList arrAttributeValue;
        private PdfPage pdfPage;
        private HtmlToPdfElement htmlToPdfElement;
        private String _strbookTitle;
        private String strbookHeader;
        private String _strTocHtml;
        private String strFileExtn;
        private String _strbaseURL;
        private const string DATEFORMAT = "MMM, dd yyyy";
        private PdfBLL pdfBLL;
        private RequestInfo objreqinf;
        private Entity objEntity;
        private AbstractController objQueryServiceController;
        private XmlDocument xmlDocSearchResult;
        private ServiceProvider objFactory;
        private StringWriter strWr;
        private PDFHelper pdfHelper;
        const string MOSSSERVICE = "MossService";
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the file extn.
        /// </summary>
        /// <value>The file extn.</value>
        public String FileExtn
        {
            get { return strFileExtn; }
            set { strFileExtn = value; }
        }
        /// <summary>
        /// Gets or sets the strbase URL.
        /// </summary>
        /// <value>The strbase URL.</value>
        public String strbaseURL
        {
            get { return _strbaseURL; }
            set { _strbaseURL = value; }
        }
        /// <summary>
        /// Gets or sets the STRing toc HTML.
        /// </summary>
        /// <value>The STR toc HTML.</value>
        public String strTocHtml
        {
            get { return _strTocHtml; }
            set { _strTocHtml = value; }
        }
        /// <summary>
        /// Gets or sets the string book title.
        /// </summary>
        /// <value>The strbook title.</value>
        public String strbookTitle
        {
            get { return _strbookTitle; }
            set { _strbookTitle = value; }
        }
        #endregion

        #region Internal Methods
        /// <summary>
        /// Adds the book title.
        /// </summary>
        /// <param name="docPDF">The doc PDF.</param>
        /// <param name="strBooktitle">The STR booktitle.</param>
        internal void AddBookTitle(Document docPDF, XPathDocument xmlDocumentCriteria, Boolean IsTOCApplicable, string type)
        {
            strbookHeader = string.Empty;
            strbookHeader = "DIGITAL WELL BOOK _____________________________________________________";
            try
            {
                pdfBLL = new PdfBLL();
                strbookTitle = pdfBLL.GetAttributeBookInfo(xmlDocumentCriteria, "BookName");


                if (IsTOCApplicable)
                {
                    pdfPage = docPDF.Pages[0];
                }
                else
                {
                    docPDF.InsertPage(0, PageSize.A4, new Margins(), PageOrientation.Portrait);
                    pdfPage = docPDF.Pages[0];
                }

                #region Header for Book Title Page
                //Generate Text
                // Create a Bold .NET font of 10 points
                System.Drawing.Font fntVerdanaHeadaer = new System.Drawing.Font("Verdana", 15,
                            System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                // Create the PDF fonts based on the .NET true type fonts
                PdfFont newfntVerdanaHeadaer = docPDF.AddFont(fntVerdanaHeadaer);

                // Add Title elements to the document
                TextElement PageTitleVerdanaHeadaer = new TextElement(3, 20, strbookHeader, newfntVerdanaHeadaer);
                PageTitleVerdanaHeadaer.TextAlign = HorizontalTextAlign.Left;
                pdfPage.AddElement(PageTitleVerdanaHeadaer);
                #endregion

                //Generate Text
                #region bOOK tITLE
                System.Drawing.Font ttfFontBold = new System.Drawing.Font("Verdana", 30,
                                System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
                // Create the PDF fonts based on the .NET true type fonts
                PdfFont newVerdanaFontBold = docPDF.AddFont(ttfFontBold);


                // Add Title elements to the document
                TextElement TypeTitle = new TextElement(3, 300, strbookTitle, newVerdanaFontBold);
                TypeTitle.TextAlign = HorizontalTextAlign.Center;
                AddElementResult addTitle = pdfPage.AddElement(TypeTitle);
                #endregion

                ///Generate Text for Book Owner 
                #region bOOK oWNER

                System.Drawing.Font fntVerdanaBold = new System.Drawing.Font("Verdana", 20,
                            System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
                // Create the PDF fonts based on the .NET true type fonts
                PdfFont newfntVerdanaBold = docPDF.AddFont(fntVerdanaBold);
                //Add Book Owner to Page
                TextElement TypeVerdanaBold = new TextElement(3, addTitle.EndPageBounds.Bottom + 5,
                                    "Book Owner - " + pdfBLL.GetAttributeBookInfo(xmlDocumentCriteria, "BookOwner"),
                                                        newfntVerdanaBold);
                TypeVerdanaBold.TextAlign = HorizontalTextAlign.Center;
                AddElementResult addOwner = pdfPage.AddElement(TypeVerdanaBold);
                #endregion

                #region Published Date
                System.Drawing.Font ttfFont = new System.Drawing.Font("Verdana", 10,
                                System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                // Create the PDF fonts based on the .NET true type fonts
                PdfFont newVerdanaFont = docPDF.AddFont(ttfFont);
                TextElement TypeVerdanaDate;
                if (type.Equals("pdf"))
                {
                    TypeVerdanaDate = new TextElement(3, addOwner.EndPageBounds.Bottom + 5, "Published date: " + DateTime.Now.ToString(DATEFORMAT), newVerdanaFont);
                }
                else
                {
                    TypeVerdanaDate = new TextElement(3, addOwner.EndPageBounds.Bottom + 5, "Date: " + DateTime.Now.ToString(DATEFORMAT), newVerdanaFont);
                }
                TypeVerdanaDate.TextAlign = HorizontalTextAlign.Center;
                pdfPage.AddElement(TypeVerdanaDate);
                #endregion
            }
            finally
            {
            }
        }

        /// <summary>
        /// Adds the HTM lto PDF.
        /// </summary>
        /// <param name="docPDF">The doc PDF.</param>
        /// <param name="strBTOC">The STR BTOC.</param>
        internal void AddHTMLtoPDF(Document docPDF, String strBTOC, string strContext, bool blnIsTOC)
        {
            if (blnIsTOC)
            {
                strbaseURL = strContext;
                htmlToPdfElement = new HtmlToPdfElement(Convert.ToString(strBTOC), strbaseURL);

                pdfPage = docPDF.InsertPage(1, PageSize.A4, new Margins(40), PageOrientation.Portrait);
                //docPDF.AddPage(PageSize.A4, new Margins(10, 10, 0, 0), PageOrientation.Portrait);
                pdfPage.AddElement(htmlToPdfElement);
                docPDF.Pages.Add(pdfPage);
            }
            else
            {
                strbaseURL = strContext;
                htmlToPdfElement = new HtmlToPdfElement(Convert.ToString(strBTOC), strbaseURL);
                pdfPage = docPDF.AddPage(PageSize.A4, new Margins(40), PageOrientation.Portrait);
                pdfPage.AddElement(htmlToPdfElement);
                docPDF.Pages.Add(pdfPage);
            }
        }

        /// <summary>
        /// Adds the chapter title page.
        /// </summary>
        /// <param name="docPDF">The doc PDF.</param>
        /// <param name="ChapterTitle">The chapter title.</param>
        internal void AddChapterTitlePage(Document docPDF, String ChapterTitle)
        {
            pdfPage = docPDF.AddPage(new Margins(40));

            #region Chapter Title
            /// Generate Text
            /// Create a Times New Roman Bold .NET font of 10 points
            System.Drawing.Font ttfFontBold = new System.Drawing.Font("Verdana", 40,
                        System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            /// Create the PDF fonts based on the .NET true type fonts
            PdfFont newVerdanaFontBold = docPDF.AddFont(ttfFontBold);

            /// Add Title elements to the document
            TextElement TypeTitle = new TextElement(3, 300, ChapterTitle, newVerdanaFontBold);
            TypeTitle.TextAlign = HorizontalTextAlign.Center;
            pdfPage.AddElement(TypeTitle);
            #endregion
        }

        /// <summary>
        /// Adds the file to PDF document.
        /// </summary>
        /// <param name="docPDF">The doc PDF.</param>
        /// <param name="file">The file.</param>
        internal void AddFiletoPDFDoc(Document docPDF, SPFile file)
        {
            pdfHelper = new PDFHelper();
            if (file != null)
            {
                FileExtn = file.Name.Substring(file.Name.LastIndexOf(".") + 1).ToLowerInvariant();
                if (FileExtn.Equals("pdf"))
                {

                    Document newDoc = new Document(file.OpenBinaryStream());
                    docPDF.AppendDocument(newDoc);
                }
                else if (FileExtn.Equals("bmp") || FileExtn.Equals("gif") || FileExtn.Equals("jpg") ||
                         FileExtn.Equals("tif") || FileExtn.Equals("png") || FileExtn.Equals("jpeg") ||
                         FileExtn.Equals("tiff") || FileExtn.Equals("png"))
                {
                    PdfPage pagePDF = docPDF.AddPage();
                    Stream ImageStream = file.OpenBinaryStream();
                    System.Drawing.Image Image = System.Drawing.Image.FromStream(ImageStream);
                    /// Display image in the available space in page and with a auto determined height to keep the aspect ratio
                    ImageElement imageElement = new ImageElement(0, 0, Image);
                    pagePDF.AddElement(imageElement);
                }
                else
                {
                    pdfHelper.AddWarningTitleToNewPDfPage(docPDF, "File Type is not Supported");
                }
            }
            else
            {
                pdfHelper.AddWarningTitleToNewPDfPage(docPDF, "File Does not Exist");
            }
        }

        /// <summary>
        /// Adds the type1 to PDF doc.
        /// </summary>
        /// <param name="docPDF">The doc PDF.</param>
        /// <param name="xpatpageNode">The xpatpage node.</param>
        internal string AddType1toPDFDoc(string[] strarrPageDetails, string context)
        {
            strWr = new StringWriter();
            objFactory = new ServiceProvider();
            objreqinf = new RequestInfo();
            xmlDocSearchResult = new XmlDocument();
            pdfBLL = new PdfBLL();
            XmlTextReader xmlTextReader = null;
            AbstractController objMossController = null;
            objMossController = objFactory.GetServiceManager(MOSSSERVICE);
            objreqinf.Entity = SetEntity(strarrPageDetails);
            if (!strarrPageDetails[3].ToUpper().Equals("WELLHISTORY"))
            {
                objQueryServiceController = objFactory.GetServiceManager(Constant.REPORTSERVICE, context);
                xmlDocSearchResult = objQueryServiceController.GetSearchResults(objreqinf, -1, strarrPageDetails[3], null, 0);
            }
            else
            {
                objQueryServiceController = objFactory.GetServiceManager(Constant.EVENTSERVICE, context);
                xmlDocSearchResult = objQueryServiceController.GetSearchResults(objreqinf, -1, strarrPageDetails[3], null, 0);
            }
            switch (strarrPageDetails[3].ToUpper())
            {
                case "PREPRODRFT":
                    xmlTextReader = ((MOSSServiceManager)objMossController).GetXSLTemplate("PDFTabular Results", context);
                    break;
                case "WELLBOREHEADER":
                    xmlTextReader = ((MOSSServiceManager)objMossController).GetXSLTemplate("WellboreHeader Datasheet", context);
                    break;
                case "WELLHISTORY":
                    xmlTextReader = ((MOSSServiceManager)objMossController).GetXSLTemplate("WellHistory DataSheet", context);
                    break;
                case "WELLSUMMARY":
                    xmlTextReader = ((MOSSServiceManager)objMossController).GetXSLTemplate("WellSummary", context);
                    break;
            }
            strWr = pdfBLL.TransformSearchResultsToXSL(xmlDocSearchResult, xmlTextReader, string.Empty, string.Empty,
              string.Empty, strarrPageDetails[3], 100, "Report", 100, context);

            return Convert.ToString(strWr.GetStringBuilder());
        }

        /// <summary>
        /// Sets the entity.
        /// </summary>
        /// <param name="xpatpageNode">The xpatpage node.</param>
        /// <returns>Entity</returns>
        private Entity SetEntity(string[] strarrPageDetails)
        {
            try
            {
                objEntity = new Entity();
                objQueryServiceController = null;
                objEntity.Property = true;
                switch (strarrPageDetails[3])
                {
                    case "PreProdRFT":
                        objEntity.ResponseType = Constant.TABULAR;
                        break;
                    case "WellboreHeader":
                        objEntity.ResponseType = Constant.DATASHEETREPORT;
                        break;
                    case "WellHistory":
                        objEntity.ResponseType = Constant.DATASHEETREPORT;
                        break;
                    case "WellSummary":
                        objEntity.ResponseType = Constant.DATASHEETREPORT;
                        break;
                }
                ArrayList arlAttribute = new ArrayList();
                arlAttribute = SetBasicAttribute(strarrPageDetails);
                objEntity.Attribute = arlAttribute;

            }
            catch
            { throw; }
            return objEntity;
        }

        /// <summary>
        /// Sets the basic attribute.
        /// </summary>
        /// <param name="xpatpageNode">The xpatpage node.</param>
        /// <returns>ArrayList</returns>
        private ArrayList SetBasicAttribute(string[] strarrPageDetails)
        {
            objAttribute = new Attributes();
            arlAttribute = new ArrayList();
            arrAttributeValue = new ArrayList();
            pdfBLL = new PdfBLL();

            switch (strarrPageDetails[4])
            {
                case "Well":
                    objAttribute.Name = "UWI";
                    break;
                case "Wellbore":
                    objAttribute.Name = "UWBI";
                    break;
                case "Field":
                    objAttribute.Name = "Field Name";
                    break;
            }
            arrAttributeValue.Add(SetValue(strarrPageDetails[2]));
            objAttribute.Value = arrAttributeValue;
            objAttribute.Operator = strarrPageDetails[1];
            arlAttribute.Add(objAttribute);
            return arlAttribute;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="strField">The STR field.</param>
        /// <returns>Value object</returns>        
        protected static Value SetValue(string value)
        {
            Value objValue = new Value();
            objValue.InnerText = value;
            return objValue;
        }

        /// <summary>
        /// Adds the warning title to new Pdf page.
        /// </summary>
        /// <param name="docPDF">The doc PDF.</param>
        /// <param name="strText">The STR text.</param>
        internal void AddWarningTitleToNewPDfPage(Document docPDF, String strText)
        {
            pdfPage = docPDF.AddPage();

            /// Generate Text
            /// Create a Bold .NET font of 10 points
            System.Drawing.Font ttfFontBold = new System.Drawing.Font("Verdana", 12,
                        System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            /// Create the PDF fonts based on the .NET true type fonts
            PdfFont newVerdanaFontBold = docPDF.AddFont(ttfFontBold);

            /// Add Title elements to the document
            TextElement WarningTitle = new TextElement(0, 200, strText, newVerdanaFontBold);
            WarningTitle.TextAlign = HorizontalTextAlign.Center;
            pdfPage.AddElement(WarningTitle);
        }
        #endregion

        #region Print Log Methods

        internal void PrintLog(string parentSiteUrl, string message, string location, string title)
        {
            SPListItem objListItem = null;
            SPList list;
            try
            {
                string strEnableDWBPrintLog = PortalConfiguration.GetInstance().FindWebServiceKey("EnableDWBPrintLog", parentSiteUrl,true);

                if (!string.IsNullOrEmpty(strEnableDWBPrintLog) && string.Equals(strEnableDWBPrintLog.ToLowerInvariant(), "yes"))
                {

                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        SPUserToken systoken = null;

                        using (SPSite tempSite = new SPSite(parentSiteUrl))
                        {
                            systoken = tempSite.SystemAccount.UserToken;
                        }
                        using (SPSite site = new SPSite(parentSiteUrl, systoken))
                        {
                            using (SPWeb web = site.OpenWeb())
                            {
                                web.AllowUnsafeUpdates = true;
                                list = web.Lists["DWB Print Log"];
                                objListItem = list.Items.Add();
                                if (objListItem != null)
                                {
                                    objListItem["Title"] = title;
                                    objListItem["Location"] = location;
                                    objListItem["Message"] = message;
                                    if (HttpContext.Current != null)
                                    {
                                        if (HttpContext.Current.Items["HttpHandlerSPWeb"] == null)
                                        {
                                            HttpContext.Current.Items["HttpHandlerSPWeb"] = web;
                                        }
                                    }
                                    objListItem.Update();
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

        #endregion Print Log Methods
    }
}

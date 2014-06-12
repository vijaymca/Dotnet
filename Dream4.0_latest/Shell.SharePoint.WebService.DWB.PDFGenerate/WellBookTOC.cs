#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: WellBookTOC.cs
#endregion
using System;
using System.Xml.XPath;
using Winnovative.PdfCreator;
using System.Xml;
using System.Text;
using Shell.SharePoint.DWB.BusinessLogicLayer;
namespace Shell.SharePoint.WebService.DWB.PdfGenerate
{
    /// <summary>
    /// WellBookTOC Class is used for the Generation of TOC  and all tasks Related to TOC.
    /// </summary>
    public class WellBookTOC
    {
        #region Variables
        private String strHyphn;
        //private StringBuilder strBHyphen;
        private PDFHelper objpdfHelper;
        private BookPage objbkpage;
        private PdfBLL objpdfBLL;
        private const string strTOCBeginHTML =
        @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
        <html xmlns=""http://www.w3.org/1999/xhtml"">
        <head>
<link rel=""stylesheet"" type=""text/css"" href=""/_Layouts/DREAM/Styles/DetailReportRel2_1.css"" />
      <link rel=""stylesheet"" type=""text/css"" href=""/_Layouts/DREAM/Styles/SearchResultsRel2_1.css"" />
          <title></title>
        </head>
        <body>
          <table width=""100%"" border=""0"" style=""font-family: verdana;font-size:medium"">
            <tr >
              <td style=""height: 23px;text-align: center;font-size:large"">
                <b>Table of Contents</b>
              </td>
            </tr>
            <tr>
              <td style=""height: 23px"">
              </td>
            </tr>
            <tr>
              <td >
                <table>";
        private StringBuilder strBTOC;
        private const string strTOCEndHTML = @"</table>
                                          </td>
                                        </tr>
                                      </table>
                                    </body>
                                    </html>";
        private String strPgTitle;
        private String strAssetVlue;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the STR hyphen.
        /// </summary>
        /// <value>The STR hyphen.</value>
        public String strHyphen
        {
            get { return strHyphn; }
            set { strHyphn = value; }
        }
        /// <summary>
        /// Gets or sets the STR page title.
        /// </summary>
        /// <value>The STR page title.</value>
        public String strPageTitle
        {
            get { return strPgTitle; }
            set { strPgTitle = value; }
        }
        /// <summary>
        /// Gets or sets the STR asset value.
        /// </summary>
        /// <value>The STR asset value.</value>
        public String strAssetValue
        {
            get { return strAssetVlue; }
            set { strAssetVlue = value; }
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Generates the TOC.
        /// </summary>
        /// <param name="docPDF">The doc PDF.</param>
        /// <param name="xmlPathDocumentCriteria">The XML path document criteria.</param>
        internal void GenerateTOC(Document docPDF, XPathDocument xmlPathDocumentCriteria, String context,Boolean isTOCApplicable, Boolean isStoryBoardApplicable, bool isPageTitleApplicable)
        {
            objpdfHelper = new PDFHelper();
            try
            {
                strBTOC = new StringBuilder();
                
                objpdfBLL = new PdfBLL();
                XPathNavigator objXPNavigator = xmlPathDocumentCriteria.CreateNavigator();
                // Set up namespace manager for XPath  
                XmlNamespaceManager objXMLnameSpaces = new XmlNamespaceManager(objXPNavigator.NameTable);
                objXMLnameSpaces.AddNamespace("PDF", "http://www.shell.com/");

                if (isTOCApplicable)
                {
                    Int32 intPgCount = Convert.ToInt32(objpdfBLL.GetAttributeBookInfo(xmlPathDocumentCriteria, "PageCount"));
                    Int32 TOCPageCount = intPgCount / 63;
                    if (!(intPgCount % 63 == 0))
                        TOCPageCount++;
                    for (int pageCounter = 0; pageCounter < TOCPageCount; pageCounter++)
                    {
                        docPDF.Pages.AddNewPage(PageSize.A4, new Margins(40), PageOrientation.Portrait);
                        
                    }
                    strBTOC.Append(strTOCBeginHTML);
                }
                XPathNodeIterator chapternodes = objXPNavigator.Select("/BookInfo/Chapter");
                while (chapternodes.MoveNext())//chapters
                {
                    if (isTOCApplicable)
                    {
                        strBTOC.AppendFormat(@"<tr><td colspan=""2"" >{0}</td><td ></td><td ></td></tr>",
                                chapternodes.Current.GetAttribute("ChapterTitle", objXMLnameSpaces.DefaultNamespace));
                    }
                    //Add page for Chapter Title if is Printable
                    if (Convert.ToBoolean(chapternodes.Current.GetAttribute("IsPrintable", objXMLnameSpaces.DefaultNamespace)))
                    {
                        objpdfHelper.AddChapterTitlePage(docPDF, chapternodes.Current.GetAttribute("ChapterTitle", objXMLnameSpaces.DefaultNamespace));
                    }
                    XPathNodeIterator pagenodes = chapternodes.Current.Select("PageInfo");
                    while (pagenodes.MoveNext())//pages in a chapter
                    {
                        if (isTOCApplicable)
                        {
                            //strBHyphen = new StringBuilder();
                            strPageTitle = pagenodes.Current.GetAttribute("PageTitle", objXMLnameSpaces.DefaultNamespace);
                            //for (int counter = 0; counter < (113 - strPageTitle.Length); counter++)
                            //{
                            //    strBHyphen.Append("-");
                            //}
                            strBTOC.AppendFormat(@"<tr>
                                        <td style=""width:5%""></td>
                                        <td style=""width:95%"" align=""right""><font style=""float:left"">{0}</font></td>
                                   </tr>",
                                                  strPageTitle);

                        }
                        objbkpage = new BookPage();
                        objbkpage.GeneratePDFPage(docPDF, pagenodes.Current, context, isPageTitleApplicable);
                        if (isStoryBoardApplicable)
                        {
                            objbkpage.GeneratePDFPageStoryBoard(docPDF, pagenodes.Current, context);
                        }
                    }
                    objpdfHelper.PrintLog(context, string.Format("Chapter {0} is created", chapternodes.Current.GetAttribute("ChapterTitle", objXMLnameSpaces.DefaultNamespace)), "WellBookTOC.GenerateTOC", "ChapterCreation");
                }
                if (isTOCApplicable)
                {
                    strBTOC.Append(strTOCEndHTML);
                    objpdfHelper.AddHTMLtoPDF(docPDF, Convert.ToString(strBTOC), context, true);
                }
            }
            catch (Exception ex)
            {
                objpdfHelper.PrintLog(context, ex.Message, "WellBookTOC.GenerateTOC", "Exception");
                objpdfHelper.PrintLog(context, ex.StackTrace, "WellBookTOC.GenerateTOC", "Exception");
                throw;
            }
        }
        #endregion
    }
}

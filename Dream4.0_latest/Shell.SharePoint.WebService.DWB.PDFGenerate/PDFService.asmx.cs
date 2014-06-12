#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: PDFService.asmx.cs
#endregion
using System;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Net;
using System.Web.Services;
using System.Web.Services.Protocols;

using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DREAM.CustomSOAPHeader;
namespace Shell.SharePoint.WebService.DWB.PdfGenerate
{
    /// <summary>
    /// PDFService is the Entry Point of the PDF WebService
    /// </summary>
    [WebService(Namespace = "http://shell.com/")]
    public class PDFService : System.Web.Services.WebService
    {
        #region Variables
        private BookDetails objBookDetails;
        private int intResult;     
        const string NORECORDSFOUND = "No records were found that matched your search parameters.Please modify your parameters and run the search again.";
        private PDFHelper objPdfhelper = new PDFHelper();
        #endregion

        #region Public Methods
        /// <summary>
        /// Generates the PDF.
        /// </summary>
        /// <param name="xmlDocCriteria">The XML doc criteria.</param>
        [WebMethod]
        public int GeneratePDF(XmlElement xmlElement, String strContext)
        {
            try
            {
              
                if (objPdfhelper != null)
                {
                    objPdfhelper.PrintLog(strContext, "GeneratePDF Called", "PDFService.GeneratePDF", "GeneratePDF");
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlElement.OuterXml);
                XPathDocument doc = new XPathDocument(new XmlNodeReader(xmlDoc.DocumentElement.ParentNode));
                objBookDetails = new BookDetails();
                intResult = objBookDetails.GenerateBook(doc, strContext);
                return intResult;
            }

            catch (WebException ex)
            {
                objPdfhelper.PrintLog(strContext, ex.Message, "PDFService.GeneratePDF", "Exception");
                objPdfhelper.PrintLog(strContext, ex.StackTrace, "PDFService.GeneratePDF", "Exception");
                throw;
            }
            catch (SoapException soapEx)
            {
                objPdfhelper.PrintLog(strContext, soapEx.Message, "PDFService.GeneratePDF", "Exception");
                objPdfhelper.PrintLog(strContext, soapEx.StackTrace, "PDFService.GeneratePDF", "Exception");
                /// If no records found exception comes, still print the doc
                if (string.Compare(soapEx.Message, "No records were found that matched your search parameters.Please modify your parameters and run the search again.", true) != 0)
                {
                    throw;
                }
                else if (string.Compare(soapEx.Message, "There is a technical problem in processing your last request.Please try again and/or inform the administrator via the link on the Help main page.", true) != 0)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                objPdfhelper.PrintLog(strContext, ex.Message, "PDFService.GeneratePDF", "Exception");
                objPdfhelper.PrintLog(strContext, ex.StackTrace, "PDFService.GeneratePDF", "Exception");
                throw;
            }
            return intResult;
        }

        [WebMethod]
        public string GetString()
        {
            return "Test Succeed";
        }

        [WebMethod]
        public string GetNewString(string name)
        {
            return string.Format("Your Name {0}",name);
        }
        #endregion
    }
}

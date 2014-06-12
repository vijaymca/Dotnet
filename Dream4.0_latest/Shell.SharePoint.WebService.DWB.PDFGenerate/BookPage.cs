#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: BookPage.cs
#endregion
using System;
using Winnovative.PdfCreator;
using System.Xml.XPath;
using System.Xml;
using Shell.SharePoint.DWB.BusinessLogicLayer;
using Shell.SharePoint.WebService.DWB.PdfGenerate;
using System.IO;
using Microsoft.SharePoint;
using System.Text;
using System.Web.Services.Protocols;
using System.Data;
using System.Web;
using System.Net;
namespace Shell.SharePoint.WebService.DWB.PdfGenerate
{
  public class BookPage
  {
    #region Variables
    private string[] strarrPageDetails;
    private DataRow dtrwRow;
    private string strType1Html;
    private Stream objStream ;
    private SPFile objSPfile;
    private String strConnectionType;
    private String strAssetValueXML;
    private String strPgTitle;
    private String strPgOwner;
    private String strSgnOffStatus;
    private String strNrrative;
    private PdfBLL pdfBLL;
    private Int32 intPgId;
    private PDFHelper objpdfHelper;
    private string strfieldsToView;
    private string strCAMLQuery;
    private CommonBLL objCommonBLL;
    private string strHtmlStoryBoard;
    private StringBuilder strbHtml;
    private StringBuilder strPageTitleHtml;
    #endregion
    #region Properties
    /// <summary>
    /// Gets or sets the integer page id.
    /// </summary>
    /// <value>The int page id.</value>
    public Int32 IntPageId
    {
      get { return intPgId; }
      set { intPgId = value; }
    }
    /// <summary>
    /// Gets or sets the STR narrative.
    /// </summary>
    /// <value>The STR narrative.</value>
    public String StrNarrative
    {
      get { return strNrrative; }
      set { strNrrative = value; }
    }
    /// <summary>
    /// Gets or sets the STRing sign off status.
    /// </summary>
    /// <value>The STR sign off status.</value>
    public String StrSignOffStatus
    {
      get { return strSgnOffStatus; }
      set { strSgnOffStatus = value; }
    }
    /// <summary>
    /// Gets or sets the STR page owner.
    /// </summary>
    /// <value>The STR page owner.</value>
    public String StrPageOwner
    {
      get { return strPgOwner; }
      set { strPgOwner = value; }
    }
    /// <summary>
    /// Gets or sets the STR page title.
    /// </summary>
    /// <value>The STR page title.</value>
    public String StrPageTitle
    {
      get { return strPgTitle; }
      set { strPgTitle = value; }
    }
    /// <summary>
    /// Gets or sets the type of the STRing asset.
    /// </summary>
    /// <value>The type of the STR asset.</value>
    public String StrAssetValue
    {
      get { return strAssetValueXML; }
      set { strAssetValueXML = value; }
    }
    /// <summary>
    /// Gets or sets the type of the STR connection Type.
    /// </summary>
    /// <value>The type of the STR connect.</value>
    public String StrConnectType
    {
      get { return strConnectionType; }
      set { strConnectionType = value; }
    }
    #endregion
    /// <summary>
    /// Generates the PDF page.
    /// </summary>
    /// <param name="docPDF">The doc PDF.</param>
    /// <param name="xPathNavigator">The x path navigator.</param>
    internal void GeneratePDFPage(Document docPDF, XPathNavigator xpatpageNode,String strContext, bool isPageTitleApplicable)
    {
        objpdfHelper = new PDFHelper();
      try
      {
        strarrPageDetails = new string[5];
        strbHtml = new StringBuilder();
        strPageTitleHtml = new StringBuilder();
        strType1Html = string.Empty;
        objSPfile = null;
        objStream = new MemoryStream();
        XmlNamespaceManager objXMLnameSpaces = new XmlNamespaceManager(xpatpageNode.NameTable);
        objXMLnameSpaces.AddNamespace("PDF", "http://www.shell.com/");
        StrConnectType = xpatpageNode.GetAttribute("ConnectionType", objXMLnameSpaces.DefaultNamespace);

        ///Add First Page
        pdfBLL = new PdfBLL();
        
        strarrPageDetails[0] = xpatpageNode.GetAttribute("ActualColumnname", objXMLnameSpaces.DefaultNamespace);
        strarrPageDetails[1] = Constant.EQUALS;
        strarrPageDetails[2] = xpatpageNode.GetAttribute("AssetValue", objXMLnameSpaces.DefaultNamespace);
        strarrPageDetails[3] = xpatpageNode.GetAttribute("ReportName", objXMLnameSpaces.DefaultNamespace);
        strarrPageDetails[4] = xpatpageNode.GetAttribute("AssetType", objXMLnameSpaces.DefaultNamespace);
        IntPageId = Convert.ToInt32(xpatpageNode.GetAttribute("PageID", objXMLnameSpaces.DefaultNamespace));
        if (isPageTitleApplicable)
        {
            StrNarrative = pdfBLL.GetNarrative(IntPageId, strContext, Constant.LISTNARRATIVE);
            if (string.IsNullOrEmpty(StrNarrative))
                StrNarrative = "N/A";
            StrSignOffStatus = xpatpageNode.GetAttribute("SignOffStatus", objXMLnameSpaces.DefaultNamespace);
            switch (StrSignOffStatus)
            {
                case "Yes":
                    StrSignOffStatus = "Signed Off";
                    break;
                case "No":
                    StrSignOffStatus = "Not Signed Off";
                    break;
            }
            StrPageOwner = xpatpageNode.GetAttribute("PageOwner", objXMLnameSpaces.DefaultNamespace);
            StrPageTitle = xpatpageNode.GetAttribute("Title", objXMLnameSpaces.DefaultNamespace);
            xpatpageNode.MoveToParent();
            StrAssetValue = xpatpageNode.GetAttribute("AssetValue", objXMLnameSpaces.DefaultNamespace);
            StrPageTitle = StrPageTitle.Replace("$", StrAssetValue);
            #region PageTitleHtml
            strPageTitleHtml.Append(@"<table width=""90%"" align=""center"" style="" font-family:Verdana;font-size:42px;font-weight:bold;"" >
                                      <tr >
                                        <td align=""center"" width=""40%"" style=""padding-right: 8px; height: 600px;"">
                                          <b>");
            strPageTitleHtml.Append(StrPageTitle);
            strPageTitleHtml.Append(@" </b></td>
                                      </tr>
                                      </table>
                                    <table width=""70%"" align=""center"" style="" font-family:Verdana;font-size:20px;
                                        background-color:Black;border:solid 1px Black;"" cellpadding=""2"" cellspacing=""2"" >
                                      <tr style=""background-color:White;"">
                                        <td align=""right"" width=""40%"" style=""padding-right: 8px; height: 21px;"">
                                          <b>Page Owner</b></td>
                                        <td align=""left"" width=""60%"" style=""height: 21px"">
                                          ");
            strPageTitleHtml.Append(StrPageOwner);
            strPageTitleHtml.Append(@" </td>
                                      </tr>
                                      <tr style=""background-color:White;"">
                                        <td align=""right"" style=""padding-right: 8px;"">
                                          <b>Sign Off Status</b></td>
                                        <td align=""left"" >
                                          ");
            strPageTitleHtml.Append(StrSignOffStatus);
            strPageTitleHtml.Append(@"</td>
                                      </tr>
                                      <tr style=""height:100px;background-color:White;"">
                                        <td align=""right"" style=""padding-right: 8px;"" valign=""top"">
                                          <b>Narrative</b></td>
                                        <td align=""left"" valign=""top"">
                                          ");
            strPageTitleHtml.Append(StrNarrative);
            strPageTitleHtml.Append(@"</td>
                                      </tr>
                                  </table>");
            #endregion
            objpdfHelper.AddHTMLtoPDF(docPDF, Convert.ToString(strPageTitleHtml), strContext, false);
        }
        switch (StrConnectType)
        {
          case "1":
              try
              {
                  strType1Html = objpdfHelper.AddType1toPDFDoc(strarrPageDetails, strContext);
                  strbHtml.Append(Constant.HEAD);
                  strbHtml.Append(strType1Html);
                  strbHtml.Append(Constant.END);
                  HtmlToPdfElement htmlToPdfElement;
                  htmlToPdfElement = new HtmlToPdfElement(0, 0, -1, -1, Convert.ToString(strbHtml), HttpContext.Current.Request.Url.ToString());
                  htmlToPdfElement.FitWidth = true;
                  htmlToPdfElement.EmbedFonts = true;
                  htmlToPdfElement.LiveUrlsEnabled = true;
                  htmlToPdfElement.ScriptsEnabled = true;
                  htmlToPdfElement.ActiveXEnabled = true;
                  htmlToPdfElement.PdfBookmarkOptions.TagNames = null;
                  //Add a first page to the document. The next pages will inherit the settings from this page 
                  PdfPage page = docPDF.Pages.AddNewPage(PageSize.A4, new Margins(10, 10, 0, 0), PageOrientation.Portrait);
                  page.AddElement(htmlToPdfElement);
              }
              catch (SoapException soapEx)
              {
                  objpdfHelper.AddWarningTitleToNewPDfPage(docPDF,soapEx.Message);
              }
              catch (WebException ex)
              {
                  objpdfHelper.AddWarningTitleToNewPDfPage(docPDF, ex.Message);
              }

            break;
          case "2":
            objSPfile = pdfBLL.DownloadFileFromDocumentLibrary(strContext, Constant.LISTPUBLISHEDDOCUMENTS,
                                                          Convert.ToString(IntPageId));
            objpdfHelper.AddFiletoPDFDoc(docPDF, objSPfile);
            break;
          case "3":
            objSPfile = pdfBLL.DownloadFileFromDocumentLibrary(strContext, Constant.LISTUSERDEFINEDDOCUMENTS,
                                                          Convert.ToString(IntPageId));
            objpdfHelper.AddFiletoPDFDoc(docPDF, objSPfile);
            break;
        }
      }
      catch (Exception ex)
      {
          objpdfHelper.PrintLog(strContext, ex.Message, "BookPage.GeneratePDFPage", "Exception");
          objpdfHelper.PrintLog(strContext, ex.StackTrace, "BookPage.GeneratePDFPage", "Exception");
        throw;
      }
      finally {
        objStream.Dispose();
      }
    }
    /// <summary>
    /// Generates the PDF page story board.
    /// </summary>
    /// <param name="docPDF">The doc PDF.</param>
    /// <param name="xPathNavigator">The xpath navigator.</param>
    /// <param name="strContext">The SiTe context.</param>
    internal void GeneratePDFPageStoryBoard(Document docPDF, XPathNavigator xPathNavigator, string strContext)
    {
      strCAMLQuery= string.Empty;
      strfieldsToView = string.Empty;
      XmlNamespaceManager objXMLnameSpaces = new XmlNamespaceManager(xPathNavigator.NameTable);
      objXMLnameSpaces.AddNamespace("PDF", "http://www.shell.com/");
      strCAMLQuery = @"<Where><Eq><FieldRef Name=""Page_ID"" /><Value Type=""Number"">" + IntPageId + @"</Value></Eq></Where>";
      objCommonBLL = new CommonBLL();
      DataTable dtResultTable = objCommonBLL.ReadList(strContext,Constant.DWBSTORYBOARD, strCAMLQuery, strfieldsToView);
      if (dtResultTable != null)
      {
        if (dtResultTable.Rows.Count > 0)
        {
          try
          {
            dtrwRow = dtResultTable.Rows[0];
            #region StoryBoard HTML
            strHtmlStoryBoard = @"
                              <table align=""center"">
                              <tr style=""background-color:white"">
                                <td style=""text-align:center"">
                                <font style=""font-size:large;font-weight:bold;font-family:Verdana;""> Story Board Info </font>
                                </td>
                              </tr>   
                              </table>    
                              <table align=""center"" width=""80%"" cellpadding=""2""cellspacing=""2""
                                style=""font-size:medium;font-family:Verdana;background-color:#000"">
                              <tr style=""background-color:white"">
                                <td width=""30%"" align=""right"">
                                Page Type  
                                </td>
                                <td width=""70%"">
                                  " + Convert.ToString(dtrwRow["Page_Type"]) + @"                              
                                </td>
                              </tr>
                              <tr style=""background-color:white"">
                                <td align=""right"">
                                Page Title  
                                </td>
                                <td>
                                  " + Convert.ToString(dtrwRow["Page_Title"]) + @"        
                                </td>
                              </tr>
                              <tr style=""background-color:white"">
                                <td align=""right"">
                                Connection Type 
                                </td>
                                <td>
                                  " + Convert.ToString(dtrwRow["Connection_Type"]) + @"        
                                </td>
                              </tr>
                              <tr style=""background-color:white"">
                                <td align=""right"">
                                Source  
                                </td>
                                <td>
                                  " + Convert.ToString(dtrwRow["Source"]) + @"        
                                </td>
                              </tr>
                              <tr style=""background-color:white"">
                                <td align=""right"">
                                Discipline  
                                </td>
                                <td>
                                  " + Convert.ToString(dtrwRow["Discipline"]) + @"        
                                </td>
                              </tr>
                              <tr style=""background-color:white"">
                                <td align=""right"">
                                Master Page Name  
                                </td>
                                <td>
                                  " + Convert.ToString(dtrwRow["Master_Page"]) + @"        
                                </td>
                              </tr>
                              <tr style=""background-color:white"">
                                <td align=""right"">
                                Application Generating Page  
                                </td>
                                <td>
                                  " + Convert.ToString(dtrwRow["Application_Page"]) + @"        
                                </td>
                              </tr>
                              <tr style=""background-color:white"">
                                <td align=""right"">
                                Application Template  
                                </td>
                                <td>
                                  " + Convert.ToString(dtrwRow["Application_Template"]) + @"        
                                </td>
                              </tr>
                              <tr style=""background-color:white"">
                                <td align=""right"">
                                SOP  
                                </td>
                                <td>
                                  " + Convert.ToString(dtrwRow["SOP"]) + @"        
                                </td>
                              </tr>
                              <tr style=""background-color:white"">
                                <td align=""right"">
                                Created By  
                                </td>
                                <td>
                                  " + Convert.ToString(dtrwRow["Created_By"]) + @"        
                                </td>
                              </tr>
                              <tr style=""background-color:white"">
                                <td align=""right"">
                                Creation Date  
                                </td>
                                <td>
                                  " + Convert.ToString(dtrwRow["Creation_Date"]) + @"        
                                </td>
                              </tr>
                              <tr style=""background-color:white"">
                                <td align=""right"">
                                Page Owner  
                                </td>
                                <td>
                                  " + Convert.ToString(dtrwRow["Page_Owner"]) + @"        
                                </td>
                              </tr>
                            </table>"
                              ;
            #endregion
            objpdfHelper.AddHTMLtoPDF(docPDF, strHtmlStoryBoard, strContext, false);

          }
          finally
          {
            dtResultTable.Dispose();
          }
        }
      }
    }
  }
}

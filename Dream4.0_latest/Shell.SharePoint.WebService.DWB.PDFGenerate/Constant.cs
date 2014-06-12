#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: Constant.cs
#endregion

using System;
using System.Data;
using System.Configuration;

namespace Shell.SharePoint.WebService.DWB.PdfGenerate
{
  /// <summary>
  /// Contains the Constants for Web Service.
  /// </summary>
  public static class Constant
  {
    internal static String LISTNARRATIVE = "DWB Narratives";
    internal static String LISTPUBLISHEDDOCUMENTS = "DWB Published Documents";
    internal static String LISTUSERDEFINEDDOCUMENTS = "DWB UserDefined Documents";
    internal static String LISTPUBLISHED = "DWB Published Library";
    internal static String LISTPRINTED = "DWB Printed Documents";
    internal static string DWBSTORYBOARD = "DWB StoryBoard";
    internal static string EQUALS = "EQUALS";
    internal static string TABULAR = "Tabular";
    internal static string DATASHEETREPORT = "Datasheet";
    internal static string REPORTSERVICE = "ReportService";
    internal static string EVENTSERVICE = "EVENTSERVICE";
    internal static string HEAD = @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
      <html xmlns=""http://www.w3.org/1999/xhtml"" >
      <head>
          <title>Untitled Page</title>
          <link rel=""stylesheet"" type=""text/css"" href=""/_Layouts/DREAM/Styles/DetailReportRel2_1.css"" />
        <link rel=""stylesheet"" type=""text/css"" href=""/_Layouts/DREAM/Styles/WellHistoryStyle.css"" />
        <link rel=""stylesheet"" type=""text/css"" href=""/_Layouts/DREAM/Styles/WellHistoryStyleRel2_1.css"" />
        <link rel=""stylesheet"" type=""text/css"" href=""/_Layouts/DREAM/Styles/DWBHistoryReport.css"" />
      </head>
      <body>";
    internal static string END = @"</body></html>";
  }
}

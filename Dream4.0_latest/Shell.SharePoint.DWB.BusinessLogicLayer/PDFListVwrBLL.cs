#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: PDFListVwrBLL.cs
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Shell.SharePoint.DWB.DataAccessLayer;

namespace Shell.SharePoint.DWB.BusinessLogicLayer
{
    /// <summary>
    /// BLL for PDF List Viewer.
    /// </summary>
  public class PDFListVwrBLL
  {
    #region Variables
    CommonDAL objCommonDAL; 
    #endregion

    /// <summary>
    /// Gets the SP files.
    /// </summary>
    /// <param name="strContext">The STR context.</param>
    /// <param name="strLibraryName">Name of the STR library.</param>
    /// <param name="intBookId">The int book id.</param>
    /// <returns>DataTable</returns>
    public DataTable GetSPFiles(string context, string libraryName,int bookId)
    {
      objCommonDAL = new CommonDAL();
      return objCommonDAL.ReadLibrary(context, libraryName, bookId);
    }
  }
}

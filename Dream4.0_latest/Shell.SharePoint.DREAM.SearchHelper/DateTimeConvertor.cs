#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : DateTimeConvertor.cs
#endregion

/// <summary>
/// DateTimeConvertor
/// </summary>
using System;
using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.DREAM.SearchHelper
{
    /// <summary>
    /// This class is used to return the Date Time in the format Specified in the PortalConfiguration List
    /// </summary>
  public  class DateTimeConvertor
    {
        private const string DATEFORMAT = "Date Format";

        /// <summary>
        /// Converts and Returns the Culture Formatted Date Time object of the date in string 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public string GetDateTime(string date)
        {
            string strDateFormat = string.Empty;
            strDateFormat = PortalConfiguration.GetInstance().GetKey(DATEFORMAT).ToString();
            DateTime dtmValue = DateTime.Parse(date);
            return dtmValue.ToString(strDateFormat);
        }
    }
}

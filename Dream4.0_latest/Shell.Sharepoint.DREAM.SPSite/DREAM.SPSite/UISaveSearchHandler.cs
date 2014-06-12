#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: UISaveSearchHandler.cs
#endregion
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Utilities;
using Microsoft.SharePoint;
using System.Xml;

namespace Shell.SharePoint.DREAM.Site.UI
{
    /// <summary>
    /// This Class is for Handling the save Search for UI Controls.
    /// </summary>
    public class UISaveSearchHandler :UIHelper
    {
        /// <summary>
        /// Saves the search XML to library.
        /// </summary>
        /// <param name="searchName">Name of the search.</param>
        /// <param name="saveSearchName">Name of the save search.</param>
        /// <param name="labelException">The label exception.</param>
        /// <param name="shared">if set to <c>true</c> [shared].</param>
        /// <param name="saveSearch">The save search.</param>
        /// <param name="searchRequest">The search request.</param>
        public void SaveSearchXMLToLibrary(string strSearchName, string strSaveSearchName, bool blnShared, DropDownList cboSaveSearch, XmlDocument xmlSearchRequest)
        {
            objMossController = objFactory.GetServiceManager(MOSSSERVICE);
            SaveSearchXMLGenerator objSaveSearchXMLGenerator = new SaveSearchXMLGenerator();
            SaveSearchRequest objSaveSearchRequest = new SaveSearchRequest();
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
               {
                   int intOrderCount = ((MOSSServiceManager)objMossController).GetSaveOrderNumber(strSearchName, GetUserName()).Count;
                   intOrderCount = intOrderCount + 1;
                   //check the condition for save search type shared is selected or not.
                   objSaveSearchRequest.SaveTypeShared = blnShared;

                   if(xmlSearchRequest.SelectSingleNode(ENTITYPATH).Attributes[ATTRITYPE] != null)
                   {
                       string strType = xmlSearchRequest.SelectSingleNode(ENTITYPATH).Attributes[ATTRITYPE].Value;
                       objSaveSearchRequest.Type = strType;
                   }


                   objSaveSearchXMLGenerator.SaveSearch(strSaveSearchName, strSearchName, GetUserName(), xmlSearchRequest, intOrderCount.ToString(), objSaveSearchRequest);
                   cboSaveSearch.Items.Clear();
                   cboSaveSearch.Items.Add(DEFAULTDROPDOWNTEXT);
                   ((MOSSServiceManager)objMossController).LoadSaveSearch(strSearchName, cboSaveSearch);
               });
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// Displays the results.
        /// </summary>
        /// <param name="requestInfo">The request info.</param>
        /// <param name="searchName">Name of the search.</param>
        /// <param name="pageURL">The page URL.</param>
        public void DisplayResults(Page page, RequestInfo requestInfo, string searchName)
        {
            XmlDocument xmlDocSearchRequest = null;
            if(string.Equals(searchName, QUERYSEARCH))
            {
                objReportController = objFactory.GetServiceManager(QUERYSERVICE);
                xmlDocSearchRequest = objReportController.CreateSearchRequest(requestInfo);
            }
            else
            {
                objReportController = objFactory.GetServiceManager(REPORTSERVICE);
                xmlDocSearchRequest = objReportController.CreateSearchRequest(requestInfo);
            }
            CommonUtility.SetSessionVariable(page, searchName, xmlDocSearchRequest.OuterXml);
        }
        /// <summary>
        /// Saves the search XML to library.
        /// </summary>
        /// <param name="searchName">Name of the search.</param>
        /// <param name="saveSearchName">Name of the save search.</param>
        /// <param name="labelException">The label exception.</param>
        /// <param name="shared">if set to <c>true</c> [shared].</param>
        /// <param name="saveSearch">The save search.</param>
        /// <param name="searchRequest">The search request.</param>
        public void ModifySaveSearchXML(string searchName, string saveSearchName, bool shared, DropDownList saveSearch, XmlDocument searchRequest)
        {
            objMossController = objFactory.GetServiceManager(MOSSSERVICE);
            SaveSearchXMLGenerator objSaveSearchXMLGenerator = new SaveSearchXMLGenerator();
            SaveSearchRequest objSaveSearchRequest = new SaveSearchRequest();
            CommonUtility objUtility = null;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    objUtility = new CommonUtility();
                    string strUserID = objUtility.GetSaveSearchUserName();
                    ((MOSSServiceManager)objMossController).ModifySaveSearch(searchName, strUserID, saveSearchName, searchRequest, shared.ToString());
                    saveSearch.Items.Clear();
                    ((MOSSServiceManager)objMossController).LoadSaveSearch(searchName, saveSearch);
                });
            }
            catch
            {
                throw;
            }

        }

    }
}

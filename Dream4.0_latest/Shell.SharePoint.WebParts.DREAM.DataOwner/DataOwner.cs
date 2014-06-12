#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : DataOwner.cs
#endregion
using System;
using System.Web.UI;
using System.Xml;
using System.IO;
using System.Xml.Xsl;
using System.Xml.XPath;
using Microsoft.SharePoint;
using System.Web;
using System.Data;
using System.Web.UI.WebControls.WebParts;
using System.Runtime.InteropServices;
using Shell.SharePoint.DREAM.MOSSProcess;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DREAM.Business.Entities;


namespace Shell.SharePoint.WebParts.DREAM.DataOwner
{
    /// <summary>
    /// This web part is used to diaplay contact details of Data Owners 
    /// of all databases of active region.
    /// </summary>
    [Guid("97a1aa6c-e9fa-4931-8ac9-4bb161f8163a")]
    public class DataOwner : WebPart
    {
        #region Declaration
        const string strDetailsRequired = "DisplayName|Department|DepartmentCode|Company|Adress|Phone|Email|WebSite";
        private AbstractController objMossController;
        private ServiceProvider objFactory = new ServiceProvider();
        private CommonUtility objCommonUtility = new CommonUtility();
        private ActiveDirectoryService objADservice;
        DataOwnerXMLGenerator objDataOwnerXMLGenerator = new DataOwnerXMLGenerator(SPContext.Current.Site.Url);
        string[] arrDataBaseName;
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Called by the ASP.NET page framework to initialize the page object.
        /// </summary>
        protected override void OnInit(EventArgs e)
        {
            objCommonUtility.RenderBusyBox(this.Page);
        }

        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            try
            {

                XmlDocument objDataOwnerXML;
                objDataOwnerXML = objDataOwnerXMLGenerator.GetDataOwnerXML(GetDataOwnerDetails(GetDataOwnerNames()), arrDataBaseName);
                objMossController = objFactory.GetServiceManager("MossService");
                XmlTextReader objXmlTextReader = ((MOSSServiceManager)objMossController).GetXSLTemplate("DataOwner", SPContext.Current.Site.Url);
                DisplayDetails(objDataOwnerXML, objXmlTextReader);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
            finally
            {
                objCommonUtility.CloseBusyBox(this.Page);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// This method is used to get names of dataowners of active region.
        /// </summary>
        /// <returns>array of names of dataowners</returns>
        private string[] GetDataOwnerNames()
        {
            string[] arrDataOwner = null;
            DataTable dtblDataOwnerNames = null;
            dtblDataOwnerNames = objDataOwnerXMLGenerator.GetActiveDataOwnerList();
            try
            {
                if ((dtblDataOwnerNames != null) && (dtblDataOwnerNames.Rows.Count > 0))
                {
                    arrDataOwner = new string[dtblDataOwnerNames.Rows.Count];
                    arrDataBaseName = new string[dtblDataOwnerNames.Rows.Count];
                    for (int intCount = 0; intCount < dtblDataOwnerNames.Rows.Count; intCount++)
                    {
                        if (dtblDataOwnerNames.Rows[intCount]["Data_x0020_Owner_x0020_Name"] != null)
                        {
                            arrDataOwner[intCount] = dtblDataOwnerNames.Rows[intCount]["Data_x0020_Owner_x0020_Name"].ToString();
                            arrDataBaseName[intCount] = dtblDataOwnerNames.Rows[intCount]["Database_x0020_Name"].ToString();
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (dtblDataOwnerNames != null)
                {
                    dtblDataOwnerNames.Dispose();
                }
            }
            return arrDataOwner;
        }
        /// <summary>
        /// This method is used get details of dataowner from active directory
        /// </summary>
        /// <param name="DataOwnerNames">array of dataowners' name</param>
        /// <returns>collection of users</returns>
        private Users GetDataOwnerDetails(string[] dataOwnerNames)
        {
            Users objUsers = null;
            try
            {
                objADservice = new ActiveDirectoryService();
                SPSecurity.RunWithElevatedPrivileges(delegate()
              {
                  objUsers = objADservice.GetUserDetails(dataOwnerNames);
              });
            }
            catch
            {
                throw;
            }
            return objUsers;
        }
        /// <summary>
        /// This method display dataowner xml through xsl template
        /// </summary>
        /// <param name="dataOwnerXML">Data owner XML</param>
        /// <param name="textReader">text reader of XSL template</param>
        /// <returns></returns>
        private void DisplayDetails(XmlDocument dataOwnerXML, XmlTextReader textReader)
        {
            XslCompiledTransform xslTransform = null;
            XmlDocument objXmlDocForXSL = null;
            try
            {
                xslTransform = new XslCompiledTransform();
                objXmlDocForXSL = new XmlDocument();
                //Inititlize the XSL
                objXmlDocForXSL.Load(textReader);
                xslTransform.Load(objXmlDocForXSL);
                xslTransform.Transform(dataOwnerXML, null, HttpContext.Current.Response.Output);
            }
            catch
            {
                throw;
            }

        }

        #endregion
    }
}





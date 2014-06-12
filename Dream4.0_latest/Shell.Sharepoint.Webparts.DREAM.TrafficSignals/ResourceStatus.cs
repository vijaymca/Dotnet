#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : ResourceStatus.cs
#endregion
using System;
using System.Xml;
using System.Web.UI;
using System.Drawing;
using Microsoft.SharePoint;
using System.Web.UI.WebControls;
using System.Web.Services.Protocols;
using System.Runtime.InteropServices;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;

namespace Shell.Sharepoint.Webparts.DREAM.TrafficSignals
{
    /// <summary>
    /// This web part is used to display the status of availability/unavailability for 
    /// all all the resources which is used in DREAM portal
    /// </summary>
    [Guid("c4897f3d-6cff-4a8c-b82e-d2cc5fcf776b")]
    public class ResourceStatus : System.Web.UI.WebControls.WebParts.WebPart
    {
        #region DECLARATION
         XmlDocument xmlResourceStatus;
        AbstractController objResourceController;
        string strCurrSiteUrl = SPContext.Current.Site.Url.ToString();
        const string RESOURCESTATUSHEADDER = "Resource Status";
        const string ERRORMESSAGE = "Resource status not available";
        const string NORECORDSFOUND = "No records were found that matched your search parameters. Please modify your paramaters and run the search again.";
        Label lblErrorMessege;
        CommonUtility objCommonUtility = new CommonUtility();
        #endregion

        #region PROTECTED METHODS
        /// <summary>
        /// Called by the ASP.NET page framework to initialize the page object.
        /// </summary>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            objCommonUtility.RenderAjaxBusyBox(this.Page);
        }

        /// <summary>
        /// Renders the parent table.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderParentTable(HtmlTextWriter writer)
        {
            writer.Write("<table class=\"tableAdvSrchBorder\" cellspacing=\"0\" cellpadding=\"4\" border=\"0\" width=\"100%\">");
            writer.Write("<tr><td class=\"tdAdvSrchHeader\" colspan=\"4\" valign=\"top\"><B>" + RESOURCESTATUSHEADDER + "</b></td></tr>");
        }
        /// <summary>
        /// Render method for rendering Resource Status XSL Results
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {            
            RenderParentTable(writer);
            writer.Write("<tr><td><br/>");            
            RenderResourceStatus(writer);
            writer.Write("</td></tr></table>");            
        }
        /// <summary>
        /// Child controls method for ensure childcontrols for page.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            lblErrorMessege = new Label();
            lblErrorMessege.Visible = false;
            this.Controls.Add(lblErrorMessege);
        }
        #endregion

        #region PRIVATE METHOD

        /// <summary>
        /// Method is use to get the staus XML and render as in HTML tables.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the control content.</param>
        private void RenderResourceStatus(HtmlTextWriter writer)
        {
            ServiceProvider objFactory = new ServiceProvider();
            objResourceController = objFactory.GetServiceManager("ResourceManager");
            XmlNodeList objXMLnodeList = null;
            Boolean blnCheckValue = false;
            xmlResourceStatus = new XmlDocument();
            try
            {
                //Gets the resource status result from the report service.
                xmlResourceStatus = objResourceController.GetSearchResults();
                if (xmlResourceStatus != null)
                {
                    writer.Write("<div id=\"tableContainer\" class=\"tableContainer\">");
                    writer.Write("<table style=\"border-right:1px solid #bdbdbd;\"  cellpadding=\"0\" cellspacing=\"0\" class=\"scrollTable\" id=\"tblSearchResults\"><thead class=\"fixedHeader\" id=\"fixedHeader\"><tr style=\"height: 20px;\" align=\"center\"><th valign=\"center\" align=\"center\" text-align=\"center\">");
                    //Checks whether any service is not working and need to show the Description column.
                    objXMLnodeList = xmlResourceStatus.SelectNodes("response/resources/resource");
                    foreach (XmlNode xmlNodeResource in objXMLnodeList)
                    {
                        if (xmlNodeResource.Attributes["value"].Value.ToString() == "false")
                        {
                            blnCheckValue = true;
                            break;
                        }
                    }
                    if (blnCheckValue)
                    {
                        writer.Write("Resource Name</th>");
                        writer.Write("<th>Status</th>");
                        writer.Write("<th>Description</th>");
                    }
                    else
                    {
                        writer.Write("Resource Name</th>");
                        writer.Write("<th>Status</th>");
                    }
                    writer.Write("</tr></thead><tbody boder =\"1\" class=\"scrollContent\">");
                    //render each service status.
                    foreach (XmlNode xmlNodeResource in objXMLnodeList)
                    {
                        writer.Write("<tr height=\"20px\"><td>&#160;");                        
                        writer.Write(xmlNodeResource.Attributes["name"].Value.ToString());
                        writer.Write("&#160;</td>");
                        if (xmlNodeResource.Attributes["value"].Value.ToString() == "true")
                        {
                            if (!blnCheckValue)
                            {
                                //if the service is available shows the resource available image.
                                writer.Write("<td align=\"center\"><img align='absmiddle' src='/_layouts/DREAM/images/Resource_Available.gif' alt='Available' />");
                                writer.Write("</td>");
                            }
                            else
                            {
                                writer.Write("<td align=\"center\"><img align='absmiddle' src='/_layouts/DREAM/images/Resource_Available.gif' alt='Available' />");
                                writer.Write("</td>");
                                writer.Write("<td>&#160;</td>");                                
                            }                            
                        }
                        else
                        {
                            if (!blnCheckValue)
                            {
                                //if the service is available shows the resource available image.
                                writer.Write("<td align=\"center\"><img align='absmiddle' src='/_layouts/DREAM/images/Resource_Unavailable.gif' alt='Unavailable' />");
                                writer.Write("</td>");
                            }
                            else
                            {
                                writer.Write("<td align=\"center\"><img align='absmiddle' src='/_layouts/DREAM/images/Resource_Unavailable.gif' alt='Unavailable' />");
                                writer.Write("</td><td>");
                                writer.Write(xmlNodeResource.Attributes["desc"].Value.ToString());
                                writer.Write("&#160;</td>");
                            }
                        }
                        writer.Write("</tr>");
                    }
                    writer.Write("</table></tbody></table></div>");
                }
                else
                {
                    //renders the exception message.
                    RenderException(ERRORMESSAGE, writer);
                }
            }
            catch (SoapException soapEx)
            {
                if (!string.Equals(soapEx.Message.ToString(), NORECORDSFOUND))
                {
                    CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                }
                RenderException(soapEx.Message.ToString(),writer);
            }
            catch (Exception Ex)
            {
                RenderException(Ex.Message.ToString(),writer);
            }
            finally
            {
                objCommonUtility.CloseAjaxBusyBox(this.Page);
            }
        }
        /// <summary>
        /// Renders the exception.
        /// </summary>
        /// <param name="message">The message.</param>
        private void RenderException(string message, HtmlTextWriter writer)
        {
            lblErrorMessege.Visible = true;
            lblErrorMessege.Text = message;
            lblErrorMessege.ForeColor = Color.Red;
            lblErrorMessege.RenderControl(writer);
        }
        #endregion
    }
}
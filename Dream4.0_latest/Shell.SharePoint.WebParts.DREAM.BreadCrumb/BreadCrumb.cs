#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: BreadCrumb.cs
#endregion

using System;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Xml.XPath;
using System.Web;
using System.ComponentModel;
using System.Net;
using System.IO;
using Shell.SharePoint.DREAM.Utilities;
using Shell.SharePoint.DREAM.Controller;


namespace Shell.SharePoint.WebParts.DREAM.BreadCrumb
{

    /// <summary>
    /// BreadCrumb for DREAM
    /// </summary>
    public class BreadCrumb : WebPart
    {

        #region DECLARATION
        public XmlDocument objXmlDocument;
        string strParentSiteURL = HttpContext.Current.Request.Url.ToString();

        string strCurrentPageName = string.Empty;
        const string SITEMAPLIST = "DREAM Site Map";
        const string HOME = "Dream Home";
        const string SITEMAPURL = "/" + SITEMAPLIST + "/sitemap.xml";
        MOSSServiceManager objMOSSServiceManager = new MOSSServiceManager();
        #endregion


        #region METHODS

        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the control content.</param>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            try
            {
                if (this.Context.Items["LeftNavSelected"] == null)
                {
                    string strBreadCrumbTrail = string.Empty;
                    XmlNode objCurrNode = objXmlDocument.DocumentElement.SelectSingleNode("//attribute[@internalName='" + strCurrentPageName + "']");
                    //construct the hyperlink
                    string strCurrentScreenName = string.Empty;
                    string[] arBreadCrumb = new string[6];
                    int intCounter = 0;

                    if (objCurrNode != null)
                    {
                        strCurrentScreenName = objCurrNode.Attributes["displayName"].Value;

                        while (objCurrNode.ParentNode != null && objCurrNode.ParentNode.NodeType != XmlNodeType.Document)
                        {
                            arBreadCrumb[intCounter] = "<a href=\"" + objCurrNode.ParentNode.Attributes["pageUrl"].Value + "\" ";
                            if (objCurrNode.ParentNode.Attributes["displayName"].Value.ToLowerInvariant().Equals("dream home"))
                                arBreadCrumb[intCounter] += "target=\"_parent\"";
                            arBreadCrumb[intCounter] += ">" + objCurrNode.ParentNode.Attributes["displayName"].Value + "</a>";
                            intCounter += 1;
                            objCurrNode = objCurrNode.ParentNode;
                        }

                        for (int intVariable = arBreadCrumb.Length - 1; intVariable >= 0; intVariable--)
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(arBreadCrumb.GetValue(intVariable))))
                            {
                                strBreadCrumbTrail += arBreadCrumb.GetValue(intVariable) + " >> ";
                            }
                        }
                        strBreadCrumbTrail += strCurrentScreenName;
                    }
                    else
                        strBreadCrumbTrail = HOME;

                    writer.Write("<span style=\"font-style:verdana;font-size:11px;\">" + strBreadCrumbTrail + "</span>");
                }
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(strParentSiteURL, webEx, 1);
                writer.Write("<span style=\"font-style:verdana;font-size:11px;\">" + webEx.Message + "</span>");
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strParentSiteURL, ex);
            }
        }



        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            base.ChromeType = PartChromeType.None;
        }
        /// <summary>
        /// Gets the name of the current page.
        /// </summary>
        /// <returns></returns>
        public string GetCurrentPageInternalName()
        {
            string strPath = HttpContext.Current.Request.Url.AbsolutePath;
            FileInfo objInfo = new FileInfo(strPath);
            string strRet = objInfo.Name.Split(".".ToCharArray())[0];
            return strRet;
        }


        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            try
            {
                base.CreateChildControls();
                objXmlDocument = new XmlDocument();
                strCurrentPageName = GetCurrentPageInternalName();

                if (HttpContext.Current.Session["DREAMBreadCrumbXML"] != null)
                {
                    objXmlDocument.LoadXml((string)HttpContext.Current.Session["DREAMBreadCrumbXML"]);
                }
                else
                {
                    objXmlDocument = objMOSSServiceManager.ReadXmlFileFromSharePoint(strParentSiteURL, SITEMAPLIST, "<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>sitemap.xml</Value></Eq></Where>");
                    HttpContext.Current.Session["DREAMBreadCrumbXML"] = objXmlDocument.OuterXml;
                }

                if (HttpContext.Current.Request.Url.Query != null)
                {
                    UpdateProperty();
                }

            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(strParentSiteURL, webEx, 1);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strParentSiteURL, ex);
            }
        }

        /// <summary>
        /// Updates the url of the current screen name node.
        /// </summary>
        protected void UpdateProperty()
        {
            XmlNode objCurrentNode = objXmlDocument.DocumentElement.SelectSingleNode("//attribute[@internalName='" + strCurrentPageName + "']");

            if (objCurrentNode != null)
            {
                if (!string.IsNullOrEmpty(objCurrentNode.Attributes["pageUrl"].Value) && objCurrentNode.Attributes["pageUrl"].Value.Contains("?"))
                    objCurrentNode.Attributes["pageUrl"].Value = objCurrentNode.Attributes["pageUrl"].Value.Remove(objCurrentNode.Attributes["pageUrl"].Value.IndexOf("?"));

                objCurrentNode.Attributes["pageUrl"].Value += HttpContext.Current.Request.Url.Query;
            }
        }


        #endregion


    }
}

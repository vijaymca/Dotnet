#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: MyAssetHolder.cs 
#endregion
/// <summary> 
/// This is MyAssetHolder webpart class
/// </summary>
using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Net;
using System.Web;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;
using Microsoft.SharePoint;

namespace Shell.SharePoint.WebParts.DREAM.SearchResults
{
    /// <summary>
    /// This class is to generate the My Asset XML and save it into SharePoint document library.
    /// </summary>
    public class MyAssetHolder : MyTeamHelper
    {
        #region DECLARATION
        private HtmlButton btnOk = new HtmlButton();
        private HiddenField hidMyAsset = new HiddenField();
        private MyAssetXMLGenerator objMyAssetCreator;
        private string strUserID = string.Empty;
        private string strAssetIdentifier = string.Empty;
        private string strWindowTitle = "My Assets";
        private XmlDocument objXmlDocument;
        #endregion

        #region Protected Methods
        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            try
            {

                #region DRAEM 4.0 Paging Functionality requirement
                if ((Page.Request.Params.Get(EVENTTARGET) != null) && (Page.Request.Params.Get(EVENTTARGET).ToLower().Contains(UPDATEPANELID)))
                {
                    hstblParams = objCommonUtility.GetPagingSortingParams(Page.Request.Params.Get(EVENTARGUMENT));
                }
                #endregion
                if (this.Context.Items["LeftNavSelected"] == null)
                {

                    objXmlDocument = objMyAssetCreator.ReadMyAssetXML(strUserID, MYASSETLIB);

                    RenderResults(writer);
                    //added for context search by dev
                    RenderHiddenControls(writer);
                    //end
                }
            }
            catch (XmlException xmlEx)
            {
                CommonUtility.HandleException(strCurrSiteUrl, xmlEx);
            }
            catch (SoapException soapEx)
            {
                string strErrorMsg = soapEx.Message.ToString();

                if (!string.Equals(strErrorMsg, NORECORDSFOUND))
                {
                    CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                }
                if (strErrorMsg.Contains(CONNECTIVITYERROR))
                {
                    strErrorMsg = CONNECTIVITYERROR;
                }

                RenderException(writer, strErrorMsg, strWindowTitle, true);
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(strCurrSiteUrl, webEx, 1);
                RenderException(writer, webEx.Message.ToString(), strWindowTitle, true);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
            finally
            {
                writer.Write("<Script language=\"javascript\">setWindowTitle('" + strWindowTitle + "');</Script>");
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                strCurrSiteUrl = HttpContext.Current.Request.Url.ToString();
                if (this.Context.Items["LeftNavSelected"] == null)
                {
                    if (Page.Request.QueryString["PopUp"] == null)
                    {
                        base.OnInit(e);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            try
            {
                objCommonUtility.EnsurePanelFix(this.Page, typeof(MyAssetHolder));
                objMyAssetCreator = new MyAssetXMLGenerator();
                objReportController = objFactory.GetServiceManager(REPORTSERVICE);
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                strUserID = objCommonUtility.GetUserName();
                CreateHiddenControl();
                CreateShowOnMap();
                if (string.Equals(Page.Request.QueryString["operation"], "add") && HttpContext.Current.Request.Form["hidSelectedRows"] != null)
                {
                    string strPattern = @"\r\n";
                    Regex fixMe = new Regex(strPattern);
                    string strTrimmedMyAssetValues = fixMe.Replace(HttpContext.Current.Request.Form["hidSelectedRows"], string.Empty);

                    SelectedIdentifiers = strTrimmedMyAssetValues.Trim().Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                }
                //Dream 4.0 code start
                strSearchType = GetAsset();
                //Dream 4.0 code ends
                hidMyAsset.ID = "hidMyAsset";
                this.Controls.Add(hidMyAsset);
                btnOk.ID = "btnOk";
                btnOk.Attributes.Add("class", "button");
                btnOk.Attributes.Add("runat", "server");
                btnOk.InnerText = "OK";
                btnOk.Attributes.Add("onclick", "javascript:window.close();");
                this.Controls.Add(btnOk);
                //adding delete button
                linkMyAssets.ID = "linkMyAssets";
                linkMyAssets.CssClass = "resultHyperLink";
                linkMyAssets.ImageUrl = "/_layouts/DREAM/images/MyAssetDelete.gif";
                linkMyAssets.NavigateUrl = "javascript:DeleteSelectedAsset('" + strSearchType + "');";
                this.Controls.Add(linkMyAssets);
                base.CreateChildControls();
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Renders the results.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderResults(HtmlTextWriter writer)
        {
            if ((Page.Request.QueryString["PopUp"] != null) && (string.Equals(Page.Request.QueryString["PopUp"], "yes")))
            {
                writer.Write("<table class=\"tableAdvSrchBorder\" cellspacing=\"0\" cellpadding=\"4\" border=\"0\" width=\"100%\">");
                writer.Write("<tr><td colspan=\"4\" class=\"tdAdvSrchSubHeader\">" + strWindowTitle + "</td></tr>");

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["hidSelectedRows"]))
                {
                    if (string.Equals(Page.Request.QueryString["operation"], "add"))
                    {
                        ////save and upload xml
                        if (CheckAssetLimit(strSearchType))
                        {
                            SaveMyAssets();
                            writer.Write("<tr><td colspan=\"3\" >" + ASSETDETAILSAVED + "</td>");
                        }
                        else
                        {
                            writer.Write("<tr><td colspan=\"3\"  style=\"word-wrap:break-word\" width=\"300\">" + ((MOSSServiceManager)objMossController).GetCustomExceptionMessage(strCurrSiteUrl, "18").ToString() + "</td>");
                        }
                    }
                    else if ((Page.Request.QueryString["operation"] != null) && (string.Equals(Page.Request.QueryString["operation"], "delete")))
                    {
                        objMyAssetCreator.UploadMyAssetXML(strUserID, DeleteSelectedAsset(strSearchType, objXmlDocument), MYASSETLIB)
;
                        writer.Write("<tr><td colspan=\"3\" >" + ASSETDETAILDELETED + "</td>");
                    }
                    else if (hstblParams != null && hstblParams["operation"] != null && hstblParams["operation"].ToString() == "delete")
                    {
                        objMyAssetCreator.UploadMyAssetXML(strUserID, DeleteSelectedAsset(strSearchType, objXmlDocument), MYASSETLIB);
                        if ((objXmlDocument != null) && (objXmlDocument.DocumentElement != null))
                        {
                            RenderBookMarks(writer, objXmlDocument, strWindowTitle, string.Empty, "openMyAssets");
                        }
                        else
                        {
                            RenderException(writer, ((MOSSServiceManager)objMossController).GetCustomExceptionMessage(strCurrSiteUrl, "20").ToString(), strWindowTitle, true);
                        }
                    }

                }
                writer.Write("<td align=\"right\">");
                hidMyAsset.RenderControl(writer);
                btnOk.RenderControl(writer);
                writer.Write("</td></tr></table>");
            }
            else
            {
                if (hstblParams != null && hstblParams["operation"] != null && hstblParams["operation"].ToString() == "delete")
                {
                    objMyAssetCreator.UploadMyAssetXML(strUserID, DeleteSelectedAsset(strSearchType, objXmlDocument), MYASSETLIB);
                }

                if ((objXmlDocument != null) && (objXmlDocument.DocumentElement != null))
                {
                    RenderBookMarks(writer, objXmlDocument, strWindowTitle, string.Empty, "openMyAssets");
                    if (hstblParams != null && hstblParams["operation"] != null && hstblParams["operation"].ToString() == "delete")
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "DeleteSuccess", "javascript:alert('" + ASSETDETAILDELETED + "');", true);
                    }
                }
                else
                {

                    RenderException(writer, ((MOSSServiceManager)objMossController).GetCustomExceptionMessage(strCurrSiteUrl, "20"), strWindowTitle, true);

                }
            }
        }

        /// <summary>
        /// Saves the My Asset Details to SharePoint
        /// </summary>
        /// 
        private void SaveMyAssets()
        {
            BookMarks bookMarksNode = SetQuickBookMarks(strSearchType);
            if (bookMarksNode != null)
            {
                objMyAssetCreator.SaveMyAssets(bookMarksNode, strUserID, MYASSETLIB);
            }
        }
        /// <summary>
        /// Creates a BOOKMARKS object
        /// </summary>
        /// <param name="searchType">Type of the search.</param>
        /// <returns></returns>
        private BookMarks SetQuickBookMarks(string searchType)
        {
            BookMarks objRootNode = new BookMarks();
            BookMark objBookMark = null;
            Value objValue;
            ArrayList arlBookMark = new ArrayList();

            if (HttpContext.Current.Request.Form["hidSelectedCriteriaName"] != null)
                strAssetIdentifier = HttpContext.Current.Request.Form["hidSelectedCriteriaName"].ToString();

            objBookMark = new BookMark();
            objBookMark.BookMarkType = searchType;
            objBookMark.IdentifierName = strAssetIdentifier;

            if (!arlBookMark.Contains(objBookMark))
                arlBookMark.Add(objBookMark);

            string[] arrMyAssetVal = SelectedIdentifiers;

            ArrayList arlValue = null;

            foreach (string strAssetVal in arrMyAssetVal)
            {
                if (strAssetVal.Trim().Length > 0)
                {
                    if (objBookMark.Value == null)
                    {
                        arlValue = new ArrayList();
                    }
                    else
                        arlValue = objBookMark.Value;

                    objValue = new Value();
                    objValue.InnerText = strAssetVal.Trim();

                    if (!arlValue.Contains(objValue))
                        arlValue.Add(objValue);

                    objBookMark.Value = arlValue;
                }
            }

            objRootNode.BookMark = arlBookMark;
            return objRootNode;
        }
        /// <summary>
        /// Checks the Max number of Assets saved against the limit set in PortalCOnfiguration List
        /// </summary>
        /// <param name="srchType"></param>
        /// <returns></returns>
        private bool CheckAssetLimit(string srchType)
        {
            string strMyAssetLimit = PortalConfiguration.GetInstance().GetKey(MYASSETLIMITKEY);

            if (strMyAssetLimit.Length == 0)
                strMyAssetLimit = MYASSETLIMIT;
            if ((objXmlDocument != null) && (objXmlDocument.DocumentElement != null))
            {
                XmlNode objXmlAssetNode = objXmlDocument.DocumentElement.SelectSingleNode("BookMark[@type='" + srchType + "']");
                if (objXmlAssetNode != null)
                {
                    if (objXmlAssetNode.ChildNodes.Count >= Convert.ToInt16(strMyAssetLimit))
                        return false;
                    else
                        return true;
                }
                else
                    return true;
            }
            else
            {
                if (SelectedIdentifiers.Length > Convert.ToInt16(strMyAssetLimit))
                    return false;
                else
                    return true;
            }
        }

        #region My Asset Delete Method
        /// <summary>
        /// Deletes the selected asset.
        /// </summary>
        /// <param name="assetName">Name of the asset.</param>
        /// <param name="myAssetXML">My asset XML.</param>
        /// <returns></returns>
        private XmlDocument DeleteSelectedAsset(string assetName, XmlDocument myAssetXML)
        {

            string strPattern = @"\r\n";
            Regex fixMe = new Regex(strPattern);
            XmlNode objXMLNodeToDelete = null;
            XmlNodeList objXmlNodeList = null;

            if (myAssetXML != null)
            {
                if (string.Equals(assetName, PROJECTARCHIVES))
                    assetName = "Project Archives";

                string strXPath = "BookMarks/BookMark[@type='" + assetName + "']";
                string strAssetValuesToDelete = HttpContext.Current.Request.Form["hidSelectedRows"];
                string strTrimmedMyAssetValues = fixMe.Replace(strAssetValuesToDelete, string.Empty);
                string[] straryAssetToDelete = strTrimmedMyAssetValues.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                foreach (string asset in straryAssetToDelete)
                {
                    objXMLNodeToDelete = myAssetXML.SelectSingleNode(strXPath + "/value[text()='" + asset.Trim() + "']");
                    if (objXMLNodeToDelete != null)
                    {
                        objXMLNodeToDelete.ParentNode.RemoveChild(objXMLNodeToDelete);
                    }
                }
                //if all values of a asset is deleted then deleteing asset node from xml
                objXmlNodeList = myAssetXML.SelectNodes(strXPath + "/value");
                if ((objXmlNodeList != null) && ((objXmlNodeList.Count == 0)))
                {
                    objXMLNodeToDelete = myAssetXML.SelectSingleNode(strXPath);
                    if (objXMLNodeToDelete != null)
                    {
                        objXMLNodeToDelete.ParentNode.RemoveChild(objXMLNodeToDelete);
                    }
                }
                //if all asset nodes are deleted then delete xml from savebookmarks document library.
                if (myAssetXML.SelectNodes("BookMarks/BookMark").Count == 0)
                {
                    objMyAssetCreator.DeleteMyAssetXML(strUserID, MYASSETLIB);
                    myAssetXML = null;
                }
            }
            return myAssetXML;
        }
        #endregion
        #endregion

    }
}

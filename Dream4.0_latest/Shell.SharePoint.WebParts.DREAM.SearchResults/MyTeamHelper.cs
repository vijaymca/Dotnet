#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: MyTeamHelper.cs 
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Services.Protocols;
using System.Xml;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.WebParts.DREAM.SearchResults
{
    /// <summary>
    /// Helper class for MyTeam and MyAsset modules
    /// </summary>
    public class MyTeamHelper :SearchResultsHelper
    {
        #region Controls
        protected HiddenField hidListSearch = new HiddenField();
        protected HiddenField hidListClick = new HiddenField();
        protected HyperLink linkMyAssets = new HyperLink();

        #endregion
        const string MYASSETTITLE = "My Assets";
        protected const string MYASSETLIB = "My Assets";
        protected const string MYTEAMLIB = "My Team Assets";
        protected const string MYASSETLIMITKEY = "My Asset Limit";
        protected const string MYASSETLIMIT = "250";
        protected const string PROJECTARCHIVES = "ProjectArchives";
        protected const string ASSETDETAILSAVED = "Your Asset Details have been added.";
        protected const string ASSETDETAILDELETED = "Selected record(s) have been deleted.";
        protected const string TEAMOWNERPERMISSION = "TeamOwner";
        protected const string NONREGUSERPERMISSION = "NonRegUser";
        string strPermission = string.Empty;
        #region Properties
        /// <summary>
        /// Gets or sets the permission.
        /// </summary>
        /// <value>The permission.</value>
        protected string Permission
        {
            get
            {
                return strPermission;
            }
            set
            {
                strPermission = value;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Renders the book marks.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="bookMarkNodeList">The book mark node list.</param>
        protected void RenderBookMarks(HtmlTextWriter writer, XmlDocument objXmlDocument, string windowTitle, string strTeamID, string jsFunctionName)
        {

            XmlNodeList bookMarkNodeList = null;
            if(objXmlDocument != null)
                bookMarkNodeList = objXmlDocument.DocumentElement.SelectNodes("BookMark");

            writer.Write("<div id=\"divStandardSearch\"><table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tr><td align=\"left\" valign=\"middle\" class=\"breadcrumbRow\"><b>" + windowTitle + "</b></td></tr>");

            if(string.Equals(Permission, TEAMOWNERPERMISSION))
            {
                writer.Write("<tr><td height=\"10\" align=\"left\" valign=\"top\"></td></tr>");
                writer.Write("<tr><td height=\"10\" align=\"left\" valign=\"top\"><input type=\"button\" id=\"btnManageStaff\" class=\"button\" value=\"Manage Staff\"  onclick=\"javascript:OpenSameWindow('/Pages/ManageStaff.aspx?idValue=" + strTeamID + "');\"/></td></tr>");
            }

            writer.Write("<tr><td height=\"10\" align=\"left\" valign=\"top\"></td></tr>");

            writer.Write("<tr><td align=\"left\" valign=\"top\">");
            writer.Write("<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"tableBorder\"><tr><td id=\"tdTitle\" width=\"80%\" valign=\"middle\" class=\"savedsearchHeaderBlue\"><b>Title</b></td></tr></table>");

            writer.Write("</td></tr><tr><td align=\"left\" valign=\"top\">");
            writer.Write("<div class=\"iframeMyAssetBorder\">");

            writer.Write("<table id=\"tblAssetContainer\" width=\"100%\" hieght=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");

            if(bookMarkNodeList != null && bookMarkNodeList.Count > 0)
            {
                foreach(XmlNode objBookMarkNode in bookMarkNodeList)
                {
                    writer.Write("<tr  style=\"height:auto\" align=\"center\"><td class=\"savedsearchHeaderGray\" valign=\"middle\">");
                    if(string.Equals(objBookMarkNode.Attributes["type"].Value, strSearchType))
                    {
                        writer.Write("<div  id=\"" + Guid.NewGuid() + "_minusDiv\" style=\"width:100%;float:left;display:block\">");
                        writer.Write("<br>");
                        writer.Write("<img id=\"" + Guid.NewGuid() + "_minusImg\" onclick=\"javascript:" + jsFunctionName + "('" + objBookMarkNode.Attributes["type"].Value + "',this);\" src=\"/_layouts/DREAM/images/minus.gif\" alt=\"open\" style=\"cursor:hand;\" width=\"11\" height=\"11\" border=\"0\" hspace=\"0\"/>");
                        writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;");
                        writer.Write("<b>" + objBookMarkNode.Attributes["type"].Value + "</b>");
                        writer.Write("<br>");
                        writer.Write("<div id=\"minusDetDiv\">");
                        XmlNode objBookMark = objXmlDocument.DocumentElement.SelectSingleNode("BookMark[@type='" + objBookMarkNode.Attributes["type"].Value + "']");
                        hidListSearch.ID = "hidListSearch";
                        hidListClick.ID = "hidListClick";
                        hidListSearch.RenderControl(writer);
                        hidListClick.RenderControl(writer);
                        DisplayMyAssetResults(writer, objBookMark, strSearchType, windowTitle);
                        writer.Write("<br>");
                        writer.Write("</div></div>");
                    }
                    else
                    {
                        writer.Write("<div id=\"" + Guid.NewGuid() + "_plusDiv\"   style=\"width:100%;float:left;display:block\">");
                        writer.Write("<img id=\"" + Guid.NewGuid() + "_plusImg\" onclick=\"javascript:" + jsFunctionName + "('" + objBookMarkNode.Attributes["type"].Value + "',this);\" style=\"cursor:hand;\" src=\"/_layouts/DREAM/images/plus.gif\" alt=\"open\" width=\"11\" height=\"11\" border=\"0\" hspace=\"0\"/>");
                        writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;");
                        writer.Write("<b>" + objBookMarkNode.Attributes["type"].Value + "</b>");
                        writer.Write("</div>");
                    }
                    writer.Write("</td></tr>");
                }
                writer.Write("</table></div></td></tr>");
                writer.Write("</table></div>");
            }
        }

        /// <summary>
        /// Displays the My Asset details
        /// </summary>
        /// <param name="objBookMark"></param>
        /// <param name="srchType"></param>
        private void DisplayMyAssetResults(HtmlTextWriter writer, XmlNode bookMark, string srchType, string windowTitle)
        {

            try
            {
                RequestInfo requestInfo = CreateRequestInfo(bookMark, srchType);
                //Dream 4.0 paging related changes starts
                SetSkipInfo(requestInfo.Entity);
                //Dream 4.0 paging related changes ends
                xmlDocSearchResult = objReportController.GetSearchResults(requestInfo, intMaxRecords, srchType, strSortColumn, intSortOrder);
                //Show on map
                //sets the Column name used in Map service.
                SetMapUseColumnName(xmlDocSearchResult);
                hidMapUseColumnName.RenderControl(writer);
                hidMapIdentified.RenderControl(writer);

                XmlNodeList objXmlNodeList = xmlDocSearchResult.SelectNodes(XPATH);
                intMaxRecords = Convert.ToInt32(PortalConfiguration.GetInstance().GetKey(MAXRECORDS).ToString());
                #region DREAM 4.0 Export All
                AssignHiddenFieldValues(xmlDocSearchResult);
                #endregion
                XmlTextReader objXmlTextReader = ((MOSSServiceManager)objMossController).GetXSLTemplate(TABULARRESULTSXSL, strCurrSiteUrl);
                //**********Added for My asset Delete option
                writer.Write("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\">");
                RenderMyAssetDelete(writer, windowTitle);
                writer.Write("<tr><td>");
                //*******
                //**Added for reordering of column
                //*start
                objReorder = new Reorder();
                xmlNdLstReorderColumn = objReorder.GetColumnNodeList(strSearchType, strSearchType, xmlDocSearchResult);
                objReorder.SetColNameDisplayStatus(out strColNames, out strColDisplayStatus, xmlNdLstReorderColumn);
                //*end
                //Added in DREAM 4.0 for Checkbox state management
                SetColumnNamesHiddenField();
                //** End
                //Dream 4.0 paging related changes starts
                SetPagingParameter();
                TransformXmlToResultTable(xmlDocSearchResult, objXmlTextReader, strPageNumber, strSortColumn, strSortOrder, strSearchType, string.Empty);
                writer.Write(strResultTable.ToString());
                //Dream 4.0 paging related changes ends
                writer.Write("</td></tr></table>");

                string strExportOptionsDiv = GetExportOptionsDivHTML("SearchResults", true, true, true, true);
                writer.Write(strExportOptionsDiv);
                //Added ni DREAM 4.0 for checkbox state management across postback
                objCommonUtility.RegisterOnLoadClientScript(this.Page, "SelectSelectAllCheckBox('chkbxRowSelectAll','tblSearchResults','tbody','chkbxRow');SelectSelectAllCheckBox('chkbxColumnSelectAll','tblSearchResults','thead','chkbxColumn');");
            }
            catch(SoapException soapEx)
            {
                string strErrorMsg = soapEx.Message;
                if(!string.Equals(strErrorMsg, NORECORDSFOUND))
                {
                    CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                }
                if(strErrorMsg.Contains(CONNECTIVITYERROR))
                {
                    strErrorMsg = CONNECTIVITYERROR;
                }
                RenderMyAssetException(writer, strErrorMsg, MYASSETTITLE);
            }
        }

        /// <summary>
        /// Renders my asset delete.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderMyAssetDelete(HtmlTextWriter writer, string windowTitle)
        {
            //rendering my asset delete option table
            writer.Write("<tr><td width=\"100%\" valign=\"bottom\" align=\"right\" border=\"0\" text-align=\"center\">");
            #region DRAEM 4.0 Export Options

            writer.Write("&nbsp;");
            writer.Write("Export");
            writer.Write("&nbsp;");
            writer.Write("<input type=\"image\" class=\"buttonAdvSrch\" src=\"/_layouts/DREAM/images/icon_Excel.gif\"  id=\"btnShowExportOptionDiv\" onclick=\"SetExportOptionDefaults();return pop('divExportOptions')\" />");
            writer.Write("&nbsp;&nbsp;&nbsp;");

            #endregion
            if(PortalConfiguration.GetInstance().GetKey("MapDisplay").ToLower().Equals("on") && (!strSearchType.ToLowerInvariant().Equals("query search")) && (!strSearchType.ToLowerInvariant().Equals("logs by field depth")) && (!strSearchType.ToLowerInvariant().Equals("reservoir")))
            {
                btnShowOnMap.RenderControl(writer);
                writer.Write("&nbsp;&nbsp;&nbsp;");
            }
            if(string.Equals(Permission, TEAMOWNERPERMISSION) || windowTitle.Equals(MYASSETTITLE))
            {
                writer.Write("Delete");
                writer.Write("&nbsp;");
                linkMyAssets.RenderControl(writer);
                writer.Write("&nbsp;&nbsp;&nbsp;");
            }

            writer.Write("<br/><br/></td></tr>");
        }

        /// <summary>
        /// Creates a RequestInfo object
        /// </summary>
        /// <param name="objBookMark"></param>
        /// <returns></returns>
        private RequestInfo CreateRequestInfo(XmlNode bookMark, string searchType)
        {
            RequestInfo objRequestInfo = new RequestInfo();
            Entity objEntity = new Entity();
            objEntity.ResponseType = TABULAR;
            objEntity.Property = true;
            ArrayList arlAttribute = new ArrayList();

            arlAttribute = objEntity.Attribute;
            if(arlAttribute == null)
                arlAttribute = new ArrayList();

            Attributes objAttribute = new Attributes();
            objAttribute.Name = bookMark.Attributes["IdentifierName"].Value;

            XmlNodeList xmlValueList = bookMark.SelectNodes("value");
            if(xmlValueList.Count > 1)
                objAttribute.Operator = INOPERATOR;
            else
                objAttribute.Operator = EQUALSOPERATOR;


            foreach(XmlNode objVal in xmlValueList)
            {
                objAttribute = AddValue(objAttribute, objVal);
            }
            if(!arlAttribute.Contains(objAttribute))
            {
                arlAttribute.Add(objAttribute);
            }
            objEntity.Attribute = arlAttribute;
            objRequestInfo.Entity = objEntity;
            if(!string.IsNullOrEmpty(searchType) && string.Equals(searchType.ToLowerInvariant(), "reservoir"))
            {
                objRequestInfo.Entity.Name = "Reservoir Advance Search";
            }

            return objRequestInfo;
        }


        /// <summary>
        /// Adds a new Value Object to the Attribute Object
        /// </summary>
        /// <param name="objAttribute"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        private Attributes AddValue(Attributes attribute, XmlNode valueNode)
        {
            Value objBookMarkValue = new Value();
            ArrayList arlValue = attribute.Value;
            if(arlValue == null)
                arlValue = new ArrayList();

            objBookMarkValue.InnerText = valueNode.InnerText;

            if(!arlValue.Contains(objBookMarkValue))
                arlValue.Add(objBookMarkValue);

            attribute.Value = arlValue;
            return attribute;
        }

        /// <summary>
        /// Renders my asset exception.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="message">The message.</param>
        /// <param name="windowTitle">The window title.</param>
        protected void RenderMyAssetException(HtmlTextWriter writer, string message, string windowTitle)
        {
            writer.Write("<table border=\"0\" cellspacing=\"0\" cellpadding=\"4\" width=\"100%\">");
            writer.Write("<tr><td border=\"0\" class=\"labelMessage\">");
            writer.Write(message);
            writer.Write("</td></tr></table>");
            //sets the window title based on search name.
            writer.Write("<Script language=\"javascript\">setWindowTitle('" + windowTitle + "');</Script>");
        }

        #region DREAM 4.0 Paging functionlity

        /// <summary>
        /// Gets the asset.
        /// </summary>
        /// <returns></returns>
        protected string GetAsset()
        {
            if(GetAsyncPostBackControlID().ToLowerInvariant().Contains(UPDATEPANELID))
            {
                if(!string.IsNullOrEmpty(this.Page.Request.Params.Get(EVENTARGUMENT)))
                {
                    string strParamList = this.Page.Request.Params.Get(EVENTARGUMENT);
                    if(strParamList.Contains("&"))
                    {
                        strSearchType = HttpContext.Current.Request.Form["hidAssetName"];
                        hidAssetName.Value = strSearchType;
                    }
                    else
                    {
                        strSearchType = this.Page.Request.Params.Get(EVENTARGUMENT);
                        hidAssetName.Value = strSearchType;
                    }
                }
            }
            else if(!string.IsNullOrEmpty(HttpContext.Current.Request.Form["hidAssetName"]))
            {
                strSearchType = HttpContext.Current.Request.Form["hidAssetName"];
                hidAssetName.Value = strSearchType;
            }
            return strSearchType;
        }
        #endregion

        #region DREAM 4.0 Export All

        private void AssignHiddenFieldValues(XmlDocument xmlDocSearchResult)
        {
            XmlNodeList objXmlNodeList = null;
            if(xmlDocSearchResult != null)
                objXmlNodeList = xmlDocSearchResult.SelectNodes("response");
            if(objXmlNodeList != null)
            {
                foreach(XmlNode xmlNode in objXmlNodeList)
                {
                    hidRequestID.Value = xmlNode.Attributes["requestid"].Value.ToString();
                }
            }
            hidSearchType.Value = strSearchType;
            hidMaxRecord.Value = intMaxRecords.ToString();
        }
        #endregion
        #endregion Methods
    }
}

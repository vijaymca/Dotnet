#region Shell Copyright.2011
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: Reorder.cs
#endregion
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Xml;
using System.IO;
using System.Data;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.DREAM.Utilities
{
    /// <summary>
    /// This is the Reorder Class which contains all the method related to reordering.
    /// </summary>
    public class Reorder
    {
        #region DECLARATION
        #region CONSTANTS
        const string MOSSSERVICE = "MossService";
        const string REORDERDOCLIB = "Reorder XML";
        const string CONTEXTSEARCHDOCLIB = "Context Search XML";
        const string EVENTTARGET = "__EVENTTARGET";
        const string BTNAPPLYSAVEID = "btnsavenshow";
        #endregion
        AbstractController objMOSSController;
        ServiceProvider objFactory;
        CommonUtility objCommonUtility;
        XmlDocument userColumnReferenceXml;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Reorder"/> class.
        /// </summary>
        public Reorder()
        {
            objFactory = new ServiceProvider();
            objCommonUtility = new CommonUtility();
            objMOSSController = objFactory.GetServiceManager(MOSSSERVICE);
        }
        #endregion


        /// <summary>
        /// Manages the reorder.
        /// </summary>
        /// <param name="searchName">Name of the search.</param>
        /// <param name="asset">The asset.</param>
        /// <param name="responseXml">The response XML.</param>
        /// <param name="hidReorderColValue">The hid reorder col value.</param>
        /// <param name="hidReorderSourceId">The hid reorder source id.</param>
        public void ManageReorder(string searchName, string asset, XmlDocument responseXml, HiddenField hidReorderColValue, HiddenField hidReorderSourceId)
        {
            Page objCurrentPage = hidReorderColValue.Page;
            if(string.IsNullOrEmpty(objCurrentPage.Request.Params.Get(EVENTTARGET)))
            {
                hidReorderColValue.Value = GetColumnNodeList(searchName, asset, responseXml).Item(0).ParentNode.InnerXml;
            }
            else if((objCurrentPage.Request.Params.Get(EVENTTARGET) != null) && (hidReorderSourceId.Value.ToLower().Equals(BTNAPPLYSAVEID)))
            {
                userColumnReferenceXml = SaveReorderXml(hidReorderColValue.Value, searchName, asset);
            }
            else if(!string.IsNullOrEmpty(objCurrentPage.Request.Params.Get(EVENTTARGET)))
            {
                userColumnReferenceXml = GetReorderXml(hidReorderColValue.Value, searchName, asset);
            }
        }

        /// <summary>
        /// Renders the Reorder Div
        /// </summary>
        /// <param name="nlColPreferences">The nl col preferences.</param>
        /// <returns></returns>
        public string GetReorderDivHTML(XmlNodeList nlColPreferences)
        {
            StringBuilder strReorderHTML = new StringBuilder();
            strReorderHTML.Append("<table cellpadding=\"0\" cellspacing=\"0\" ><tr><td><input type=\"image\" class=\"buttonAdvSrch\" src=\"/_layouts/DREAM/images/Reorder_Customize.jpg\" id=\"btnShowDiv\" onclick=\"return OpenReorderPopUp()\"/></td></tr><tr><td>");
            strReorderHTML.Append("<div class=\"popup\" id=\"showHideDiv\"><table><tr><td class=\"reorderHeader\"><b>List Of Columns</b></td><td align=\"right\"><img src=\"/_layouts/DREAM/images/CMSRemoveImage.GIF\" onclick=\"return HideReorderPouUp()\" alt=\"Close\" /></td></tr><tr><td style=\"padding:5px 5px 5px 5px\"><div title=\"Drag and drop column to reorder.\"><table id=\"tblShowHideColOption\">");
            //Dream 4.0 
            //Start
            strReorderHTML.Append("<tr NoDrop=\"NoDrop\" NoDrag=\"NoDrag\" NotSelectable=\"NotSelectable\"><td><input style=\"cursor:default\" id=\"chkbxCheckUncheckAll\" type=\"checkbox\" ");
            if(IsAllColumnSelected(nlColPreferences))
            {
                strReorderHTML.Append("checked=\"checked\"");
            }
            strReorderHTML.Append(" value=\"CheckUncheckAll\" onclick=\"javascript:CheckUncheckAllColumn(this,'tblShowHideColOption');\" /><b>(Select All)</b></td></tr>");
            //End
            bool blnMandatory;
            foreach(XmlNode objXmlNode in nlColPreferences)
            {
                blnMandatory = false;
                if(objXmlNode.Attributes["mandatory"] != null)
                {
                    blnMandatory = Convert.ToBoolean(objXmlNode.Attributes["mandatory"].Value);
                }
                if(!objXmlNode.Attributes["name"].Value.Equals("Depth References"))
                {
                    strReorderHTML.Append("<tr><td>" + GetChkBxHTML(objXmlNode.Attributes["name"].Value, Convert.ToBoolean(objXmlNode.Attributes["display"].Value), blnMandatory) + objXmlNode.Attributes["name"].Value + "</td></tr>");
                }
            }
            strReorderHTML.Append("</table></div></td><td>");
            strReorderHTML.Append(GetUpDownBtnHTML());
            strReorderHTML.Append("</td></tr><tr><td colspan=\"2\"><table>");
            strReorderHTML.Append("<tr><td><input class=\"buttonAdvSrch\" type=\"button\" value=\"Apply\" id=\"btnShow\" onclick=\"GetColumnOrder(this,'tblShowHideColOption')\"/>");
            strReorderHTML.Append("</td><td><input class=\"buttonAdvSrch\" type=\"button\" value=\"Apply & Save\" id=\"btnSavenShow\" onclick=\"GetColumnOrder(this,'tblShowHideColOption')\"/>");
            strReorderHTML.Append("</td></tr>");
            strReorderHTML.Append("</table></td></tr></table>");
            strReorderHTML.Append("</div></td></tr></table>");
            return strReorderHTML.ToString();
        }

        /// <summary>
        /// Determines whether [is all column selected] [the specified nl col preferences].
        /// </summary>
        /// <param name="nlColPreferences">The nl col preferences.</param>
        /// <returns>
        /// 	<c>true</c> if [is all column selected] [the specified nl col preferences]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsAllColumnSelected(XmlNodeList nlColPreferences)
        {
            bool blnAllColumnSelected = false;
            XmlNode objXmlNode = null;
            if((nlColPreferences != null) && (nlColPreferences.Count > 0 && ((objXmlNode = nlColPreferences[0].ParentNode) != null)))
            {
                int intHiddenColumn = objXmlNode.SelectNodes("child::*[(@display ='False') or (@display ='false')]").Count;
                if(intHiddenColumn == 0)
                {
                    blnAllColumnSelected = true;
                }
            }
            return blnAllColumnSelected;
        }

        /// <summary>
        /// Renders the move up down.
        /// </summary>
        /// <returns></returns>
        private string GetUpDownBtnHTML()
        {
            StringBuilder strUpDownBtnHTML = new StringBuilder();
            strUpDownBtnHTML.Append("<table>");
            strUpDownBtnHTML.Append("<tr><td><input class=\"btnUpDown\"  type=\"image\" title=\"Top\" src=\"/_layouts/DREAM/images/TPMIN1.GIF\" id=\"btnTop\" alt=\"Top\" onclick=\"javascript:return MoveRow(this);\" /></td></tr>");
            strUpDownBtnHTML.Append("<tr><td><input class=\"btnUpDown\"   type=\"image\" title=\"Up\" src=\"/_layouts/DREAM/images/ARRUPA.GIF\" id=\"btnUp\" alt=\"Up\"/ onclick=\"javascript:return MoveRow(this);\"></td></tr>");
            strUpDownBtnHTML.Append("<tr><td><input class=\"btnUpDown\"  type=\"image\" title=\"Down\" src=\"/_layouts/DREAM/images/ARRDOWNA.GIF\" id=\"btnDown\" alt=\"Down\" onclick=\"javascript:return MoveRow(this);\"/></td></tr>");
            strUpDownBtnHTML.Append("<tr><td><input  class=\"btnUpDown\"  type=\"image\" title=\"Bottom\" src=\"/_layouts/DREAM/images/TPMAX1.GIF\" id=\"btnBottom\" alt=\"Bottom\" onclick=\"javascript:return MoveRow(this);\"/></td></tr>");
            strUpDownBtnHTML.Append("</table>");
            return strUpDownBtnHTML.ToString();
        }

        /// <summary>
        /// Set ColName and DisplayStatus.
        /// </summary>
        /// <param name="colNames">The col names.</param>
        /// <param name="colDisplayStatus">The col display status.</param>
        /// <param name="nlColPreferences">The nl col preferences.</param>
        public void SetColNameDisplayStatus(out string colNames, out string colDisplayStatus, XmlNodeList nlColPreferences)
        {
            string strColName = string.Empty;
            string strColDisplayStatus = string.Empty;
            if(nlColPreferences != null)
            {
                foreach(XmlNode node in nlColPreferences)
                {
                    strColName = string.Concat(strColName, node.Attributes["name"].Value, "|");
                    if(node.Attributes["name"].Value.Equals("Depth References"))
                    {
                        strColDisplayStatus = string.Concat(strColDisplayStatus, "false|");
                    }
                    else
                    {
                        strColDisplayStatus = string.Concat(strColDisplayStatus, node.Attributes["display"].Value, "|");
                    }

                }
            }
            colNames = strColName;
            colDisplayStatus = strColDisplayStatus;
        }

        /// <summary>
        /// Gets the Get Column Node List.
        /// </summary>
        /// <param name="searchName">Name of the search.</param>
        /// <param name="asset">The asset.</param>
        /// <param name="xmlDocSearchResult">The XML doc search result.</param>
        /// <returns></returns>
        public XmlNodeList GetColumnNodeList(string searchName, string asset, XmlDocument xmlDocSearchResult)
        {
            XmlNode xmlNodeColToRender = null;
            XmlDocument responseXml = new XmlDocument();
            XmlDocument userReorderXml = new XmlDocument();
            string strUserRole = objCommonUtility.GetUserRole();//Dream 4.0
            bool blnUpdateRoleReorderXml = false;
            XmlNode xmlNodeToRemove = null;
            XmlNode xmlNodeNewElement = null;
            XmlNode xmlNodeColInResponsexml = null;
            XmlNode xmlNodeColPreferences = null;
            if(userColumnReferenceXml == null)
            {
                if(((MOSSServiceManager)objMOSSController).IsDocLibFileExist(REORDERDOCLIB, objCommonUtility.GetReorderUserName()))
                {
                    userReorderXml = ((MOSSServiceManager)objMOSSController).GetDocLibXMLFile(REORDERDOCLIB, objCommonUtility.GetReorderUserName());
                }
                //Dream 4.0
                //start
                else if(((MOSSServiceManager)objMOSSController).IsDocLibFileExist(REORDERDOCLIB, strUserRole))
                {
                    userReorderXml = ((MOSSServiceManager)objMOSSController).GetDocLibXMLFile(REORDERDOCLIB, strUserRole);
                    blnUpdateRoleReorderXml = true;
                }
                //end
                else
                {
                    return GetReorderXml(GetNodeListAsString(xmlDocSearchResult.SelectNodes("response/report/record[1]/attribute[@display !='false']")), searchName, asset).ChildNodes[0].ChildNodes[0].ChildNodes;
                }
            }
            else
            {
                userReorderXml = userColumnReferenceXml;
            }

            responseXml.LoadXml(xmlDocSearchResult.OuterXml);
            xmlNodeColInResponsexml = responseXml.SelectSingleNode("response/report/record[1]");
            xmlNodeColPreferences = userReorderXml.SelectSingleNode(string.Concat("searchtypes/searchtype[(@name='", searchName, "') and (@asset = '", asset, "')]"));

            bool blnUpdateSaveXml = false;
            bool blnMandatory;
            if(xmlNodeColPreferences != null)
            {
                xmlNodeColToRender = xmlNodeColPreferences;

                for(int intNodeCounter = xmlNodeColPreferences.ChildNodes.Count - 1; intNodeCounter >= 0; intNodeCounter--)
                {
                    xmlNodeToRemove = null;
                    if((xmlNodeToRemove = xmlNodeColInResponsexml.SelectSingleNode(string.Concat("attribute[@name = \"", xmlNodeColPreferences.ChildNodes[intNodeCounter].Attributes["name"].Value, "\"]"))) != null)
                    {
                        blnMandatory = false;
                        if(xmlNodeToRemove.Attributes["mandatory"] != null)
                        {
                            blnMandatory = Convert.ToBoolean(xmlNodeToRemove.Attributes["mandatory"].Value);
                        }
                        xmlNodeColPreferences.ChildNodes[intNodeCounter].Attributes.Append(CreateColAttribute(userReorderXml, "mandatory", blnMandatory.ToString().ToLowerInvariant()));
                        xmlNodeColInResponsexml.RemoveChild(xmlNodeToRemove);
                    }
                    else if((IsAHTVDColumn(xmlNodeColPreferences.ChildNodes[intNodeCounter].Attributes["name"].Value)) &&
                        ((xmlNodeToRemove = xmlNodeColInResponsexml.SelectSingleNode(string.Concat("attribute[@name = \"", GetAHTVColumnName(xmlNodeColPreferences.ChildNodes[intNodeCounter].Attributes["name"].Value), "\"]"))) != null)
                        )
                    {
                        blnMandatory = false;
                        if(xmlNodeToRemove.Attributes["mandatory"] != null)
                        {
                            blnMandatory = Convert.ToBoolean(xmlNodeToRemove.Attributes["mandatory"].Value);
                        }
                        xmlNodeColPreferences.ChildNodes[intNodeCounter].Attributes.Append(CreateColAttribute(userReorderXml, "mandatory", blnMandatory.ToString().ToLowerInvariant()));
                        xmlNodeColPreferences.ChildNodes[intNodeCounter].Attributes["name"].Value = GetAHTVColumnName(xmlNodeColPreferences.ChildNodes[intNodeCounter].Attributes["name"].Value);
                        xmlNodeColInResponsexml.RemoveChild(xmlNodeToRemove);
                    }
                    else
                    {
                        xmlNodeColToRender.RemoveChild(xmlNodeColPreferences.ChildNodes[intNodeCounter]);
                        blnUpdateSaveXml = true;
                    }
                }
            }
            else
            {
                return GetReorderXml(GetNodeListAsString(xmlDocSearchResult.SelectNodes("response/report/record[1]/attribute[@display !='false']")), searchName, asset).ChildNodes[0].ChildNodes[0].ChildNodes;
            }
            if((blnUpdateSaveXml) && (!blnUpdateRoleReorderXml) && (((MOSSServiceManager)objMOSSController).IsDocLibFileExist(REORDERDOCLIB, objCommonUtility.GetReorderUserName())))
            {
                ((MOSSServiceManager)objMOSSController).UploadToDocumentLib(REORDERDOCLIB, objCommonUtility.GetReorderUserName(), userReorderXml);
            }
            //Dream 4.0 start
            else if((blnUpdateSaveXml) && (blnUpdateRoleReorderXml) && (((MOSSServiceManager)objMOSSController).IsDocLibFileExist(REORDERDOCLIB, strUserRole)))
            {
                ((MOSSServiceManager)objMOSSController).UploadToDocumentLib(REORDERDOCLIB, strUserRole, userReorderXml);
            }
            //end
            foreach(XmlNode xmlNode in xmlNodeColInResponsexml.SelectNodes("attribute[@display !='false']"))
            {
                xmlNodeNewElement = CreateColumnNode(userReorderXml, xmlNode.Attributes["name"].Value, xmlNode.Attributes["display"].Value, "false");
                xmlNodeColToRender.AppendChild(xmlNodeNewElement);
            }
            return xmlNodeColToRender.ChildNodes;
        }

        /// <summary>
        /// Saves the reorder XML.
        /// </summary>
        /// <param name="colNodesToSave">The col nodes to save.</param>
        /// <param name="searchName">Name of the search.</param>
        /// <param name="asset">The asset.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public XmlDocument SaveReorderXml(string colNodesToSave, string searchName, string asset, string userId)
        {
            XmlDocument userReorderXml = null;

            if(((MOSSServiceManager)objMOSSController).IsDocLibFileExist(REORDERDOCLIB, userId))
            {
                userReorderXml = ((MOSSServiceManager)objMOSSController).GetDocLibXMLFile(REORDERDOCLIB, userId);
            }
            else
            {
                userReorderXml = new XmlDocument();
                XmlElement rootElement = userReorderXml.CreateElement("searchtypes");
                userReorderXml.AppendChild(rootElement);
            }


            XmlNode searchTypeNode = userReorderXml.SelectSingleNode("searchtypes/searchtype[(@name='" + searchName + "') and (@asset = '" + asset + "')]");
            if(searchTypeNode == null)
            {
                XmlNode ndSearchType = userReorderXml.CreateElement("searchtype");
                XmlAttribute atName = userReorderXml.CreateAttribute("name");
                atName.Value = searchName;
                ndSearchType.Attributes.Append(atName);
                XmlAttribute atAsset = userReorderXml.CreateAttribute("asset");
                atAsset.Value = asset;
                ndSearchType.Attributes.Append(atAsset);
                userReorderXml.SelectSingleNode("searchtypes").AppendChild(ndSearchType);
                ndSearchType.InnerXml = colNodesToSave;
            }
            else
            {
                searchTypeNode.InnerXml = colNodesToSave;
            }
            ((MOSSServiceManager)objMOSSController).UploadToDocumentLib(REORDERDOCLIB, userId, userReorderXml);
            return userReorderXml;
        }



        /// <summary>
        /// Determines whether [is AHTVD column] [the specified column name].
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>
        /// 	<c>true</c> if [is AHTVD column] [the specified column name]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsAHTVDColumn(string columnName)
        {
            bool blnAHTVDColumn = false;
            if(columnName.Contains(" AH") || columnName.Contains(" TV"))
            {
                blnAHTVDColumn = true;
            }
            return blnAHTVDColumn;
        }

        /// <summary>
        /// Gets the name of the AHTV column.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        private string GetAHTVColumnName(string columnName)
        {
            string strChangedColumnName = string.Empty;
            if(columnName.Contains(" AH"))
            {
                strChangedColumnName = columnName.Replace(" AH", " TV");
            }
            else if(columnName.Contains(" TV"))
            {
                strChangedColumnName = columnName.Replace(" TV", " AH");
            }
            else
            {
                strChangedColumnName = columnName;
            }
            return strChangedColumnName;
        }

        /// <summary>
        /// Gets the CHK bx HTML.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="isChecked">if set to <c>true</c> [is checked].</param>
        /// <param name="isDisabled">if set to <c>true</c> [is disabled].</param>
        /// <returns></returns>
        private string GetChkBxHTML(string value, bool isChecked, bool isDisabled)
        {
            string strChkBx = string.Empty;
            strChkBx = "<input type=\"checkbox\" style=\"cursor:default\" value=\"" + value + "\"";

            if(isChecked)
            {
                strChkBx += "checked=\"checked\"";
            }
            if(isDisabled)
            {
                strChkBx += "disabled=\"disabled\"";
            }
            //Dream 4.0 code
            //start
            strChkBx += "onclick=\"javascript:OnReorderChkBxCheckUncheck(this,'tblShowHideColOption');\"";
            //end
            strChkBx += "</input>";
            return strChkBx;
        }

        /// <summary>
        /// Creates the column node.
        /// </summary>
        /// <param name="colReferenceXml">The col reference XML.</param>
        /// <param name="name">The name.</param>
        /// <param name="display">The display.</param>
        /// <param name="mandatory">The mandatory.</param>
        /// <returns></returns>
        private XmlNode CreateColumnNode(XmlDocument colReferenceXml, string name, string display, string mandatory)
        {
            XmlElement colElement = colReferenceXml.CreateElement("column");
            colElement.Attributes.Append(CreateColAttribute(colReferenceXml, "name", name));
            colElement.Attributes.Append(CreateColAttribute(colReferenceXml, "display", display));
            colElement.Attributes.Append(CreateColAttribute(colReferenceXml, "mandatory", mandatory));
            return colElement;
        }

        /// <summary>
        /// Creates the col attribute.
        /// </summary>
        /// <param name="colReferenceXml">The col reference XML.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="attributeValue">The attribute value.</param>
        /// <returns></returns>
        private XmlAttribute CreateColAttribute(XmlDocument colReferenceXml, string attributeName, string attributeValue)
        {
            XmlAttribute nodeAttribute = colReferenceXml.CreateAttribute(attributeName);
            nodeAttribute.Value = attributeValue;
            return nodeAttribute;
        }

        /// <summary>
        /// Saves the reorder XML.
        /// </summary>
        /// <param name="colNodesToSave">The col nodes to save.</param>
        /// <param name="searchName">Name of the search.</param>
        /// <param name="asset">The asset.</param>
        /// <returns></returns>
        private XmlDocument SaveReorderXml(string colNodesToSave, string searchName, string asset)
        {
            XmlDocument userReorderXml = null;

            if(((MOSSServiceManager)objMOSSController).IsDocLibFileExist(REORDERDOCLIB, objCommonUtility.GetReorderUserName()))
            {
                userReorderXml = ((MOSSServiceManager)objMOSSController).GetDocLibXMLFile(REORDERDOCLIB, objCommonUtility.GetReorderUserName());
            }
            else
            {
                userReorderXml = new XmlDocument();
                XmlElement rootElement = userReorderXml.CreateElement("searchtypes");
                userReorderXml.AppendChild(rootElement);
            }


            XmlNode searchTypeNode = userReorderXml.SelectSingleNode("searchtypes/searchtype[(@name='" + searchName + "') and (@asset = '" + asset + "')]");
            if(searchTypeNode == null)
            {
                XmlNode ndSearchType = userReorderXml.CreateElement("searchtype");
                XmlAttribute atName = userReorderXml.CreateAttribute("name");
                atName.Value = searchName;
                ndSearchType.Attributes.Append(atName);
                XmlAttribute atAsset = userReorderXml.CreateAttribute("asset");
                atAsset.Value = asset;
                ndSearchType.Attributes.Append(atAsset);
                userReorderXml.SelectSingleNode("searchtypes").AppendChild(ndSearchType);
                ndSearchType.InnerXml = colNodesToSave;
            }
            else
            {
                searchTypeNode.InnerXml = colNodesToSave;
            }
            ((MOSSServiceManager)objMOSSController).UploadToDocumentLib(REORDERDOCLIB, objCommonUtility.GetReorderUserName(), userReorderXml);
            return userReorderXml;
        }

        /// <summary>
        /// Gets the reorder XML.
        /// </summary>
        /// <param name="colNodesToSave">The col nodes to save.</param>
        /// <param name="searchName">Name of the search.</param>
        /// <param name="asset">The asset.</param>
        /// <returns></returns>
        private XmlDocument GetReorderXml(string colNodesToSave, string searchName, string asset)
        {
            XmlDocument userReorderXml = new XmlDocument();
            XmlElement rootElement = userReorderXml.CreateElement("searchtypes");
            userReorderXml.AppendChild(rootElement);
            XmlNode ndSearchType = userReorderXml.CreateElement("searchtype");
            XmlAttribute atName = userReorderXml.CreateAttribute("name");
            atName.Value = searchName;
            ndSearchType.Attributes.Append(atName);
            XmlAttribute atAsset = userReorderXml.CreateAttribute("asset");
            atAsset.Value = asset;
            ndSearchType.Attributes.Append(atAsset);
            userReorderXml.SelectSingleNode("searchtypes").AppendChild(ndSearchType);
            ndSearchType.InnerXml = colNodesToSave;
            return userReorderXml;
        }

        /// <summary>
        /// Gets the node list as string.
        /// </summary>
        /// <param name="xmlNodeListDisplayNodes">The XML node list display nodes.</param>
        /// <returns></returns>
        private string GetNodeListAsString(XmlNodeList xmlNodeListDisplayNodes)
        {
            StringBuilder strNodeListXml = new StringBuilder();
            foreach(XmlNode xmlNode in xmlNodeListDisplayNodes)
            {
                strNodeListXml.Append(xmlNode.OuterXml);
            }
            return strNodeListXml.ToString();
        }

        #region Roles & Profiles method

        #region Save context search and group header

        /// <summary>
        /// Saves the group header order.
        /// </summary>
        /// <param name="contextLinkNodesToSave">The context link nodes to save.</param>
        /// <param name="selectedGroupHeader">The selected group header.</param>
        /// <param name="groupHeaders">The group headers.</param>
        /// <param name="asset">The asset.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public XmlDocument SaveGroupHeaderOrder(string contextLinkNodesToSave, string selectedGroupHeader, string[] groupHeaders, string asset, string userId)
        {
            XmlDocument userReorderXml = null;

            if(((MOSSServiceManager)objMOSSController).IsDocLibFileExist(CONTEXTSEARCHDOCLIB, userId))
            {
                userReorderXml = ((MOSSServiceManager)objMOSSController).GetDocLibXMLFile(CONTEXTSEARCHDOCLIB, userId);
            }
            else
            {
                userReorderXml = new XmlDocument();
                XmlElement rootElement = userReorderXml.CreateElement("contextsearches");
                userReorderXml.AppendChild(rootElement);
            }
            XmlNode xmlNodeGroupHeaders = userReorderXml.SelectSingleNode("contextsearches/groupheaders[@asset = '" + asset + "']");
            if(xmlNodeGroupHeaders == null)
            {
                xmlNodeGroupHeaders = userReorderXml.CreateElement("groupheaders");
                XmlAttribute atAsset = userReorderXml.CreateAttribute("asset");
                atAsset.Value = asset;
                xmlNodeGroupHeaders.Attributes.Append(atAsset);
                userReorderXml.DocumentElement.AppendChild(xmlNodeGroupHeaders);
            }
            SaveGroupHeader(groupHeaders, userReorderXml, xmlNodeGroupHeaders);
            AddContextSearchLniks(userReorderXml, contextLinkNodesToSave, selectedGroupHeader, asset);
            ((MOSSServiceManager)objMOSSController).UploadToDocumentLib(CONTEXTSEARCHDOCLIB, userId, userReorderXml);
            return userReorderXml;
        }

        /// <summary>
        /// Saves the context search lniks.
        /// </summary>
        /// <param name="userRoleContextSearchXml">The user role context search XML.</param>
        /// <param name="contextLinkNodesToSave">The context link nodes to save.</param>
        /// <param name="groupHeader">The group header.</param>
        /// <param name="asset">The asset.</param>
        private void AddContextSearchLniks(XmlDocument userRoleContextSearchXml, string contextLinkNodesToSave, string groupHeader, string asset)
        {
            XmlNode xmlNodeGroupHeaders = userRoleContextSearchXml.SelectSingleNode("contextsearches/groupheaders[@asset = '" + asset + "']/groupheader[@name='" + groupHeader + "']");
            xmlNodeGroupHeaders.InnerXml = contextLinkNodesToSave;
        }


        /// <summary>
        /// Saves the group header.
        /// </summary>
        /// <param name="groupHeaders">The group headers.</param>
        /// <param name="userRoleContextSearchXml">The user role context search XML.</param>
        /// <param name="xmlNodeGroupHeaders">The XML node group headers.</param>
        private static void SaveGroupHeader(string[] groupHeaders, XmlDocument userRoleContextSearchXml, XmlNode xmlNodeGroupHeaders)
        {
            XmlNode xmlNodeGroupHeader = null;
            XmlAttribute atName = null;
            XmlAttribute atOrder = null;

            for(int intIndex = 0; intIndex < groupHeaders.Length; intIndex++)
            {
                xmlNodeGroupHeader = xmlNodeGroupHeaders.SelectSingleNode("groupheader[@name='" + groupHeaders[intIndex] + "']");
                if(xmlNodeGroupHeader == null)
                {
                    xmlNodeGroupHeader = userRoleContextSearchXml.CreateElement("groupheader");
                    atName = userRoleContextSearchXml.CreateAttribute("name");
                    atName.Value = groupHeaders[intIndex];
                    xmlNodeGroupHeader.Attributes.Append(atName);
                    atOrder = userRoleContextSearchXml.CreateAttribute("order");
                    atOrder.Value = (intIndex + 1).ToString();
                    xmlNodeGroupHeader.Attributes.Append(atOrder);
                    xmlNodeGroupHeaders.AppendChild(xmlNodeGroupHeader);
                }
                else
                {
                    xmlNodeGroupHeader.Attributes.GetNamedItem("order").Value = (intIndex + 1).ToString();
                }
            }
        }
        #endregion
        /// <summary>
        /// Sets the source N destination item list.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="searchName">Name of the search.</param>
        /// <param name="asset">The asset.</param>
        /// <param name="xmlDocColResponseXml">The XML doc col response XML.</param>
        /// <param name="xmlNdLstSourceCol">The XML nd LST source col.</param>
        /// <param name="xmlNdLstDestinationCol">The XML nd LST destination col.</param>
        public void SetSourceNDestinationItemList(string role, string searchName, string asset, XmlDocument xmlDocColResponseXml, out XmlNodeList xmlNdLstSourceCol, out XmlNodeList xmlNdLstDestinationCol)
        {
            XmlNode xmlNodeColToRender = null;
            XmlDocument responseXml = new XmlDocument();
            XmlDocument userReorderXml = new XmlDocument();
            XmlNode xmlNodeNewElement = null;
            XmlNode xmlNodeColInResponsexml = null;
            XmlNode xmlNodeColPreferences = null;
            XmlNode xmlNodeToRemove = null;
            if(((MOSSServiceManager)objMOSSController).IsDocLibFileExist(REORDERDOCLIB, role))
            {
                userReorderXml = ((MOSSServiceManager)objMOSSController).GetDocLibXMLFile(REORDERDOCLIB, role);
            }
            else
            {
                xmlNdLstSourceCol = xmlDocColResponseXml.SelectNodes("response/report/record[1]/attribute[@display !='false' and @name !='Depth References' and @mandatory !='true']/@name");
                xmlNdLstDestinationCol = xmlDocColResponseXml.SelectNodes("response/report/record[1]/attribute[@display !='false' and @name !='Depth References' and @mandatory ='true']");
                return;
            }
            responseXml.LoadXml(xmlDocColResponseXml.OuterXml);
            xmlNodeColInResponsexml = responseXml.SelectSingleNode("response/report/record[1]");
            xmlNodeColPreferences = userReorderXml.SelectSingleNode(string.Concat("searchtypes/searchtype[(@name='", searchName, "') and (@asset = '", asset, "')]"));

            bool blnUpdateSaveXml = false;
            bool blnMandatory;
            if(xmlNodeColPreferences != null)
            {
                xmlNodeColToRender = xmlNodeColPreferences;

                for(int intNodeCounter = xmlNodeColPreferences.ChildNodes.Count - 1; intNodeCounter >= 0; intNodeCounter--)
                {
                    xmlNodeToRemove = null;
                    if((xmlNodeToRemove = xmlNodeColInResponsexml.SelectSingleNode(string.Concat("attribute[@name = \"", xmlNodeColPreferences.ChildNodes[intNodeCounter].Attributes["name"].Value, "\"]"))) != null)
                    {
                        blnMandatory = false;
                        if(xmlNodeToRemove.Attributes["mandatory"] != null)
                        {
                            blnMandatory = Convert.ToBoolean(xmlNodeToRemove.Attributes["mandatory"].Value);
                        }
                        xmlNodeColPreferences.ChildNodes[intNodeCounter].Attributes.Append(CreateColAttribute(userReorderXml, "mandatory", blnMandatory.ToString().ToLowerInvariant()));
                        xmlNodeColInResponsexml.RemoveChild(xmlNodeToRemove);
                    }
                    else if((IsAHTVDColumn(xmlNodeColPreferences.ChildNodes[intNodeCounter].Attributes["name"].Value)) &&
                        ((xmlNodeToRemove = xmlNodeColInResponsexml.SelectSingleNode(string.Concat("attribute[@name = \"", GetAHTVColumnName(xmlNodeColPreferences.ChildNodes[intNodeCounter].Attributes["name"].Value), "\"]"))) != null)
                        )
                    {
                        blnMandatory = false;
                        if(xmlNodeToRemove.Attributes["mandatory"] != null)
                        {
                            blnMandatory = Convert.ToBoolean(xmlNodeToRemove.Attributes["mandatory"].Value);
                        }
                        xmlNodeColPreferences.ChildNodes[intNodeCounter].Attributes.Append(CreateColAttribute(userReorderXml, "mandatory", blnMandatory.ToString().ToLowerInvariant()));
                        xmlNodeColPreferences.ChildNodes[intNodeCounter].Attributes["name"].Value = GetAHTVColumnName(xmlNodeColPreferences.ChildNodes[intNodeCounter].Attributes["name"].Value);
                        xmlNodeColInResponsexml.RemoveChild(xmlNodeToRemove);
                    }
                    else
                    {
                        xmlNodeColToRender.RemoveChild(xmlNodeColPreferences.ChildNodes[intNodeCounter]);
                        blnUpdateSaveXml = true;
                    }
                }
            }
            else
            {
                xmlNdLstSourceCol = xmlDocColResponseXml.SelectNodes("response/report/record[1]/attribute[@display !='false' and @name !='Depth References' and @mandatory !='true']/@name");
                xmlNdLstDestinationCol = xmlDocColResponseXml.SelectNodes("response/report/record[1]/attribute[@display !='false' and @name !='Depth References' and @mandatory ='true']");
                return;
            }
            if((blnUpdateSaveXml) && (((MOSSServiceManager)objMOSSController).IsDocLibFileExist(REORDERDOCLIB, role)))
            {
                ((MOSSServiceManager)objMOSSController).UploadToDocumentLib(REORDERDOCLIB, role, userReorderXml);
            }
            foreach(XmlNode xmlNode in xmlNodeColInResponsexml.SelectNodes("attribute[@display !='false' and @name !='Depth References']"))
            {
                xmlNodeNewElement = CreateColumnNode(userReorderXml, xmlNode.Attributes["name"].Value, "false", "false");
                xmlNodeColToRender.AppendChild(xmlNodeNewElement);
            }
            xmlNdLstSourceCol = xmlNodeColToRender.SelectNodes("column[@display ='false']/@name");
            xmlNdLstDestinationCol = xmlNodeColToRender.SelectNodes("column[@display !='false']");
        }

        /// <summary>
        /// Sets the SRC N dest nd LST for context search.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="groupHeader">The group header.</param>
        /// <param name="asset">The asset.</param>
        /// <param name="xmlNdLstSourceCol">The XML nd LST source col.</param>
        /// <param name="xmlNdLstDestinationCol">The XML nd LST destination col.</param>
        public void SetSrcNDestNdLstForContextSearch(string role, string groupHeader, string asset, out XmlNodeList xmlNdLstSourceCol, out XmlNodeList xmlNdLstDestinationCol)
        {
            XmlNode xmlNodeColToRender = null;
            XmlDocument responseXml = new XmlDocument();
            XmlDocument userRoleContextSearchXml = new XmlDocument();
            XmlNode xmlNodeColInResponsexml = null;
            XmlNode xmlNodeColPreferences = null;
            XmlNode xmlNodeToRemove = null;
            XmlNode xmlNodeNewElement = null;
            if(((MOSSServiceManager)objMOSSController).IsDocLibFileExist(CONTEXTSEARCHDOCLIB, role))
            {
                userRoleContextSearchXml = ((MOSSServiceManager)objMOSSController).GetDocLibXMLFile(CONTEXTSEARCHDOCLIB, role);
            }
            else
            {
                userRoleContextSearchXml = GetDataTableAsXml(asset, groupHeader);
                xmlNdLstSourceCol = userRoleContextSearchXml.SelectNodes("NewDataSet/Context_x0020_Search/Title/text()");
                xmlNdLstDestinationCol = null;
                return;
            }
            responseXml = GetDataTableAsXml(asset, groupHeader);
            xmlNodeColInResponsexml = responseXml.SelectSingleNode("NewDataSet");
            xmlNodeColPreferences = userRoleContextSearchXml.SelectSingleNode("contextsearches/groupheaders[@asset = '" + asset + "']/groupheader[@name = '" + groupHeader + "']");
            bool blnUpdateSaveXml = false;
            if(xmlNodeColPreferences != null)
            {
                xmlNodeColToRender = xmlNodeColPreferences;
                for(int intNodeCounter = xmlNodeColPreferences.ChildNodes.Count - 1; intNodeCounter >= 0; intNodeCounter--)
                {
                    xmlNodeToRemove = null;
                    if((xmlNodeToRemove = xmlNodeColInResponsexml.SelectSingleNode(string.Concat("Context_x0020_Search[Title/text() = \"", xmlNodeColPreferences.ChildNodes[intNodeCounter].Attributes["name"].Value, "\"]"))) != null)
                    {
                        xmlNodeColInResponsexml.RemoveChild(xmlNodeToRemove);
                    }
                    else
                    {
                        xmlNodeColToRender.RemoveChild(xmlNodeColPreferences.ChildNodes[intNodeCounter]);
                        blnUpdateSaveXml = true;
                    }
                }
            }
            else
            {
                userRoleContextSearchXml = GetDataTableAsXml(asset, groupHeader);
                xmlNdLstSourceCol = userRoleContextSearchXml.SelectNodes("NewDataSet/Context_x0020_Search/Title/text()");
                xmlNdLstDestinationCol = null;
                return;
            }
            if((blnUpdateSaveXml) && (((MOSSServiceManager)objMOSSController).IsDocLibFileExist(CONTEXTSEARCHDOCLIB, role)))
            {
                ((MOSSServiceManager)objMOSSController).UploadToDocumentLib(CONTEXTSEARCHDOCLIB, role, userRoleContextSearchXml);
            }
            foreach(XmlNode xmlNode in xmlNodeColInResponsexml.SelectNodes("Context_x0020_Search/Title"))
            {
                xmlNodeNewElement = CreateContextSearchNode(userRoleContextSearchXml, xmlNode.InnerText, "false");
                xmlNodeColToRender.AppendChild(xmlNodeNewElement);
            }
            xmlNdLstSourceCol = xmlNodeColToRender.SelectNodes("contextsearch[@display = 'false']/@name");
            xmlNdLstDestinationCol = xmlNodeColToRender.SelectNodes("contextsearch[@display != 'false']/@name");
        }

        /// <summary>
        /// Creates the context search node.
        /// </summary>
        /// <param name="colReferenceXml">The col reference XML.</param>
        /// <param name="name">The name.</param>
        /// <param name="display">The display.</param>
        /// <returns></returns>
        private XmlNode CreateContextSearchNode(XmlDocument colReferenceXml, string name, string display)
        {
            XmlElement colElement = colReferenceXml.CreateElement("contextsearch");
            colElement.Attributes.Append(CreateColAttribute(colReferenceXml, "name", name));
            colElement.Attributes.Append(CreateColAttribute(colReferenceXml, "display", display));
            return colElement;
        }

        /// <summary>
        /// Gets the datable as XML.
        /// </summary>
        /// <param name="asset">The asset.</param>
        /// <param name="groupHeader">The group header.</param>
        /// <returns></returns>
        private XmlDocument GetDataTableAsXml(string asset, string groupHeader)
        {
            DataTable objDataTable = null;
            XmlDocument objXmlDocument = null;
            string strCamlQuery = "<Where><And><Eq><FieldRef Name=\"Asset_x0020_Type\" /><Value Type=\"Lookup\">" + asset + "</Value></Eq><And><Eq><FieldRef Name=\"Group_x0020_Header\" /><Value Type=\"Choice\">" + groupHeader + "</Value></Eq><Eq><FieldRef Name=\"Active\" /><Value Type=\"Boolean\">1</Value></Eq></And></And></Where><OrderBy><FieldRef Name=\"LinkOrder\" Ascending=\"True\" /><FieldRef Name=\"Title\" Ascending=\"True\" /></OrderBy>";
            string strViewFields = @"<FieldRef Name='Title'/>";
            try
            {
                objDataTable = ((MOSSServiceManager)objMOSSController).ReadList(objCommonUtility.GetSiteURL(), "Context Search", strCamlQuery, strViewFields);
                DataView objDataView = objDataTable.DefaultView;
                objDataTable = objDataView.ToTable(true, "Title");
                objXmlDocument = new XmlDocument();
                StringWriter objStringWriter = new StringWriter();
                objDataTable.WriteXml(objStringWriter, XmlWriteMode.WriteSchema);
                objXmlDocument.LoadXml(objStringWriter.ToString());
            }
            finally
            {
                if(objDataTable != null)
                {
                    objDataTable.Dispose();
                }
            }
            return objXmlDocument;
        }

        #region Load Category
        /// <summary>
        /// Gets the nd LST for category.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="asset">The asset.</param>
        /// <returns></returns>
        public XmlNodeList GetNdLstForCategory(string role, string asset)
        {
            XmlNode xmlNodeColToRender = null;
            XmlDocument responseXml = new XmlDocument();
            XmlDocument userRoleContextSearchXml = new XmlDocument();
            XmlNodeList xmlNdLstSourceCol = null;
            XmlNode xmlNodeColInResponsexml = null;
            XmlNode xmlNodeToRemove = null;
            XmlNode xmlNodeNewElement = null;
            XmlNode xmlNodeColPreferences = null;

            if(((MOSSServiceManager)objMOSSController).IsDocLibFileExist(CONTEXTSEARCHDOCLIB, role))
            {
                userRoleContextSearchXml = ((MOSSServiceManager)objMOSSController).GetDocLibXMLFile(CONTEXTSEARCHDOCLIB, role);
            }
            else
            {
                userRoleContextSearchXml = GetCategoryDatableAsXml(asset);
                if(userRoleContextSearchXml == null)//when there are no category for particular asset ,then return null
                {
                    return null;
                }
                xmlNdLstSourceCol = userRoleContextSearchXml.SelectNodes("NewDataSet/Context_x0020_Search/Group_x005F_x0020_Header/text()");
                return xmlNdLstSourceCol;
            }
            responseXml = GetCategoryDatableAsXml(asset);
            if(responseXml == null)//when there are no category for particular asset ,then return null
            {
                return null;
            }
            xmlNodeColInResponsexml = responseXml.SelectSingleNode("NewDataSet");
            xmlNodeColPreferences = userRoleContextSearchXml.SelectSingleNode("contextsearches/groupheaders[@asset = '" + asset + "']");

            bool blnUpdateSaveXml = false;
            if(xmlNodeColPreferences != null && xmlNodeColPreferences.ChildNodes.Count > 0)
            {
                xmlNodeColToRender = xmlNodeColPreferences;
                for(int intNodeCounter = xmlNodeColPreferences.ChildNodes.Count - 1; intNodeCounter >= 0; intNodeCounter--)
                {
                    xmlNodeToRemove = null;
                    if((xmlNodeToRemove = xmlNodeColInResponsexml.SelectSingleNode(string.Concat("Context_x0020_Search[Group_x005F_x0020_Header/text() = \"", xmlNodeColPreferences.ChildNodes[intNodeCounter].Attributes["name"].Value, "\"]"))) != null)
                    {
                        xmlNodeColInResponsexml.RemoveChild(xmlNodeToRemove);
                    }
                    else
                    {
                        xmlNodeColToRender.RemoveChild(xmlNodeColPreferences.ChildNodes[intNodeCounter]);
                        blnUpdateSaveXml = true;
                    }
                }
            }
            else
            {
                userRoleContextSearchXml = GetCategoryDatableAsXml(asset);
                if(userRoleContextSearchXml == null)//when there are no category for particular asset ,then return null
                {
                    return null;
                }
                xmlNdLstSourceCol = userRoleContextSearchXml.SelectNodes("NewDataSet/Context_x0020_Search/Group_x005F_x0020_Header/text()");
                return xmlNdLstSourceCol;
            }
            if((blnUpdateSaveXml) && (((MOSSServiceManager)objMOSSController).IsDocLibFileExist(CONTEXTSEARCHDOCLIB, role)))
            {
                ((MOSSServiceManager)objMOSSController).UploadToDocumentLib(CONTEXTSEARCHDOCLIB, role, userRoleContextSearchXml);
            }
            foreach(XmlNode xmlNode in xmlNodeColInResponsexml.SelectNodes("Context_x0020_Search/Group_x005F_x0020_Header"))
            {
                xmlNodeNewElement = CreateCategorySearchNode(userRoleContextSearchXml, xmlNode.InnerText, xmlNodeColToRender.ChildNodes.Count.ToString());
                xmlNodeColToRender.AppendChild(xmlNodeNewElement);
            }
            xmlNdLstSourceCol = xmlNodeColToRender.SelectNodes("groupheader/@name");
            return xmlNdLstSourceCol;
        }

        /// <summary>
        /// Creates the category search node.
        /// </summary>
        /// <param name="colReferenceXml">The col reference XML.</param>
        /// <param name="name">The name.</param>
        /// <param name="order">The order.</param>
        /// <returns></returns>
        private XmlNode CreateCategorySearchNode(XmlDocument colReferenceXml, string name, string order)
        {
            XmlElement colElement = colReferenceXml.CreateElement("groupheader");
            colElement.Attributes.Append(CreateColAttribute(colReferenceXml, "name", name));
            colElement.Attributes.Append(CreateColAttribute(colReferenceXml, "order", order));
            return colElement;
        }

        /// <summary>
        /// Gets the category datable as XML.
        /// </summary>
        /// <param name="asset">The asset.</param>
        /// <returns></returns>
        private XmlDocument GetCategoryDatableAsXml(string asset)
        {
            DataTable objDataTable = null;
            XmlDocument objXmlDocument = null;
            string strCamlQuery = "<Where><And><Eq><FieldRef Name=\"Asset_x0020_Type\" /><Value Type=\"Lookup\">" + asset + "</Value></Eq><Eq><FieldRef Name=\"Active\" /><Value Type=\"Boolean\">1</Value></Eq></And></Where><OrderBy><FieldRef Name=\"HeaderOrder\" Ascending=\"True\" /><FieldRef Name=\"Group_x0020_Header\" Ascending=\"True\" /></OrderBy>";
            string strViewFields = @"<FieldRef Name='Group_x0020_Header'/>";
            try
            {
                objDataTable = ((MOSSServiceManager)objMOSSController).ReadList(objCommonUtility.GetSiteURL(), "Context Search", strCamlQuery, strViewFields);
                if(objDataTable != null && objDataTable.Rows.Count > 0)
                {
                    DataView objDataView = objDataTable.DefaultView;
                    objDataTable = objDataView.ToTable(true, "Group_x0020_Header");
                    objXmlDocument = new XmlDocument();
                    StringWriter objStringWriter = new StringWriter();
                    objDataTable.WriteXml(objStringWriter, XmlWriteMode.WriteSchema);
                    objXmlDocument.LoadXml(objStringWriter.ToString());
                }
                else
                {
                    objXmlDocument = null;
                }
            }
            finally
            {
                if(objDataTable != null)
                {
                    objDataTable.Dispose();
                }
            }
            return objXmlDocument;
        }

        #endregion
        #endregion
    }
}

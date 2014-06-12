#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : SyncList.cs
#endregion

using System;
using System.Text;
using System.Collections;
using Microsoft.SharePoint;
using System.Web;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Web.Services.Protocols;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;


namespace Shell.Sharepoint.WebParts.DREAM.SyncList
{
    /// <summary>
    /// This class is to sync basin values from power hub to sharepoint list.
    /// </summary>
    public class SyncList :System.Web.UI.WebControls.WebParts.WebPart
    {
        #region DECLARATION
        Button btnClose;
        Label lblMessage;
        Label lblInformation;
        #region DREAM 4.0 Sync Country List
        CheckBoxList chkblAssetType;
        Button btnContinue;
        Button btnCancel;
        #endregion

        const string STAROPERATOR = "*";
        const string LIKEOPERATOR = "LIKE";
        const string BASINLIST = "Basin";
        #region DREAM 4.0 Sync Country List
        const string COUNTRYLIST = "Country";
        #endregion
        const string VALUE = "value";
        const string RECORDXPATH = "response/report/record";
        const string NODENOTFOUND = "XML Nodes not Found";
        const string STATUSMESSAGE = "CDS List has been updated successfully.";
        const string REPORTTITLE = "Synchronization of CDS lists";
        const string NORECORDSFOUND = "No records were found that matched your search parameters.Please modify your parameters and run the search again.";
        const string REPORTSERVICE = "ReportService";
        string strCurrSiteUrl = string.Empty;
        int intListValMaxRecord = 65000;
        CommonUtility objCommonUtility = new CommonUtility();
        #endregion

        #region Protected Methods
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            //this will render the busy box frame.
            objCommonUtility.RenderAjaxBusyBox(this.Page);
        }
        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            //Fix for the UpdatePanel postback behaviour.
            //Dream 4.0 code start
            EnsurePanelFix(typeof(SyncList));

            strCurrSiteUrl = HttpContext.Current.Request.Url.ToString();

            //create the messege lable for displaying messege to user like Maxrecords exceeds or customerror messege information.
            CreateMessageLable();

            CreateInformationLable();

            CreateCloseButton();

            #region DREAM 4.0 Sync Country List

            CreateAssetTypeCheckBox();

            CreateContinueButton();

            CreateCancelButton();

            #endregion
        }
        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            try
            {
                RenderParentTable(writer);
                writer.Write("<tr>");
                writer.Write("<td ID=\"continueSearch\" width=\"100%\" colspan=\"4\" border=\"0\"><br />");
                lblInformation.RenderControl(writer);
                #region DREAM 4.0 changes
                chkblAssetType.RenderControl(writer);
                writer.Write("<div align=\"center\">");
                btnClose.RenderControl(writer);
                writer.Write("</div>");
                writer.Write("</td>");
                writer.Write("</tr>");
                writer.Write("<tr><td ID=\"continueSearch2\" colspan=\"4\" width=\"100%\" align=\"center\"><br />");
                if(btnContinue != null && chkblAssetType != null)
                {
                    btnContinue.OnClientClick = "return ValidateSyncListOptions('" + chkblAssetType.ClientID + "');";
                }
                btnContinue.RenderControl(writer);
                writer.Write("&nbsp;");
                btnCancel.RenderControl(writer);
                writer.Write("</table>");
                #endregion
            }
            catch(SoapException soapEx)
            {
                if(!string.Equals(soapEx.Message.ToString(), NORECORDSFOUND))
                {
                    CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), soapEx, 1);
                }
                RenderParentTable(writer);
                writer.Write("<tr><td><br/>");
                lblMessage.Visible = true;
                lblMessage.Text = soapEx.Message;
                lblMessage.RenderControl(writer);
                writer.Write("</td></tr></table>");

            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
            finally
            {
                objCommonUtility.CloseAjaxBusyBox(this.Page);
                writer.Write("<Script language=\"javascript\">setWindowTitle('Synchronization of CDS lists');</Script>");
            }
        }
        /// <summary>
        /// Handles the Click event of the btnContinue control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnContinue_Click(object sender, EventArgs e)
        {
            try
            {
                ServiceProvider objFactory = new ServiceProvider();
                AbstractController objReportController = objFactory.GetServiceManager(REPORTSERVICE);
                XmlDocument xmlDocResponse = null;
                //loads the basin list from powerhub database.
                foreach(ListItem item in chkblAssetType.Items)
                {
                    if(item.Selected)
                    {
                        xmlDocResponse = objReportController.GetSearchResults(GetRequestInfo(), intListValMaxRecord, item.Value, null, 0);
                        SynchronizeSPList(xmlDocResponse, item.Value);
                    }
                }
                RenderException(STATUSMESSAGE);
            }
            catch(SoapException soapEx)
            {
                if(!string.Equals(soapEx.Message, NORECORDSFOUND))
                {
                    CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                }
                RenderException(soapEx.Message);
            }
            catch(WebException webEx)
            {
                CommonUtility.HandleException(strCurrSiteUrl, webEx, 1);
                RenderException(webEx.Message);
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Renders the exception.
        /// </summary>
        /// <param name="message">The message.</param>
        private void RenderException(string message)
        {
            lblInformation.Text = message;

            #region DREAM 4.0 changes
            lblInformation.Visible = true;
            btnClose.Visible = true;
            chkblAssetType.Visible = false;
            btnContinue.Visible = false;
            btnCancel.Visible = false;
            #endregion
        }

        /// <summary>
        /// Assigns the values to drop down.
        /// </summary>
        /// <param name="responseXml">The response XML.</param>
        /// <param name="listName">Name of the list.</param>
        private void SynchronizeSPList(XmlDocument responseXml, string listName)
        {
            XmlNodeList xmlNodeListRecords = null;
            SPList list = null;
            SPQuery query = null;
            string strTitle = string.Empty;
            string strValue = string.Empty;
            const string COUNTRYXPATH = "attribute[@name = 'Country Name']/@value";
            const string COUNTRYCODEXPATH = "attribute[@name = 'Country']/@value";
            const string BASINXPATH = "/attribute[@name = 'Basin Name' and @value != '']/@value";
            //Check all the required objects are NULL
            if(responseXml != null)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                   {
                       using(SPSite site = new SPSite(SPContext.Current.Site.Url))
                       {
                           using(SPWeb web = site.OpenWeb())
                           {
                               if(listName.ToLowerInvariant().Equals(BASINLIST.ToLowerInvariant()))
                               {
                                   xmlNodeListRecords = responseXml.SelectNodes(RECORDXPATH + BASINXPATH);
                               }
                               else if(listName.ToLowerInvariant().Equals(COUNTRYLIST.ToLowerInvariant()))
                               {
                                   //** Comments
                                   //In response xml Country is country code 
                                   //Country Name is Country Name
                                   xmlNodeListRecords = responseXml.SelectNodes(RECORDXPATH);
                               }
                               //If sub node has value then added it to Basin list
                               if(xmlNodeListRecords != null)
                               {
                                   list = web.Lists[listName];
                                   web.AllowUnsafeUpdates = true;
                                   foreach(XmlNode objXmlNode in xmlNodeListRecords)
                                   {
                                       query = new SPQuery();
                                       if(listName.ToLowerInvariant().Equals(BASINLIST.ToLowerInvariant()))
                                       {
                                           strTitle = objXmlNode.Value;
                                           strValue = objXmlNode.Value;
                                           query.Query = "<Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">" + strValue + "</Value></Eq></Where>";
                                       }
                                       else if(listName.ToLowerInvariant().Equals(COUNTRYLIST.ToLowerInvariant()))
                                       {
                                           if(objXmlNode.SelectSingleNode(COUNTRYXPATH) != null)
                                           {
                                               strTitle = objXmlNode.SelectSingleNode(COUNTRYXPATH).Value;
                                           }
                                           if(objXmlNode.SelectSingleNode(COUNTRYCODEXPATH) != null)
                                           {
                                               strValue = objXmlNode.SelectSingleNode(COUNTRYCODEXPATH).Value;
                                           }
                                           query.Query = "<Where><Eq><FieldRef Name=\"Country_x0020_Code\" /><Value Type=\"Text\">" + strValue + "</Value></Eq></Where>";
                                       }
                                       //this will check item already exist in the list or not
                                       if(!IsListItemExist(list, query))
                                       {
                                           AddListItem(list, strTitle, strValue);
                                       }
                                   }
                                   web.AllowUnsafeUpdates = false;
                               }
                           }
                       }
                   });
            }//Main IF
        }

        /// <summary>
        /// Determines whether [is list item exist] [the specified list].
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="query">The query.</param>
        /// <returns>
        /// 	<c>true</c> if [is list item exist] [the specified list]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsListItemExist(SPList list, SPQuery query)
        {
            bool blnItemExist = false;
            SPListItemCollection listitems = list.GetItems(query);
            if((listitems != null) && (listitems.Count <= 0))
            {
                blnItemExist = false;
            }
            else
            {
                blnItemExist = true;
            }
            return blnItemExist;
        }
        /// <summary>
        /// Inserts the Basins.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="title">The title.</param>
        /// <param name="value">The value.</param>
        private void AddListItem(SPList list, string title, string value)
        {
            if(!string.IsNullOrEmpty(title))
            {
                SPListItem listItem = list.Items.Add();
                listItem["Title"] = title;
                //adds the new basin entry to the sharepoint list.
                if(list.Title.ToLowerInvariant().Equals(COUNTRYLIST.ToLowerInvariant()))
                {
                    listItem["Country_x0020_Code"] = value;
                }
                listItem.Update();
            }
        }
        /// <summary>
        /// Sets the RequestInfo object.
        /// </summary>
        /// <returns></returns>
        private RequestInfo GetRequestInfo()
        {
            RequestInfo objRequestInfo = new RequestInfo();
            Entity objEntity = new Entity();
            objEntity.Criteria = GetCriteria();
            objRequestInfo.Entity = objEntity;
            return objRequestInfo;
        }
        /// <summary>
        /// Sets the criteria.
        /// </summary>
        /// <returns></returns>
        private Criteria GetCriteria()
        {
            Criteria objCriteria = new Criteria();
            objCriteria.Value = STAROPERATOR;
            objCriteria.Operator = LIKEOPERATOR;
            return objCriteria;
        }
        /// <summary>
        /// Renders the parent table.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderParentTable(HtmlTextWriter writer)
        {
            writer.Write("<table class=\"tableAdvSrchBorder\" cellspacing=\"0\" cellpadding=\"4\" border=\"0\" width=\"100%\">");
            writer.Write("<tr><td class=\"tdBasinAdvSrchHeader\" colspan=\"4\"  valign=\"top\"><b>" + REPORTTITLE + "</b></td></tr>");
        }

        /// <summary>
        /// Creates the cancel button.
        /// </summary>
        private void CreateCancelButton()
        {
            btnCancel = new Button();
            btnCancel.ID = "btnCancel";
            btnCancel.OnClientClick = "window.close();";
            btnCancel.CssClass = "buttonAdvSrch";
            btnCancel.Text = "Cancel";
            this.Controls.Add(btnCancel);
        }

        /// <summary>
        /// Creates the continue button.
        /// </summary>
        private void CreateContinueButton()
        {
            btnContinue = new Button();
            btnContinue.ID = "btnContinue";
            btnContinue.Click += new EventHandler(btnContinue_Click);
            btnContinue.CssClass = "buttonAdvSrch";
            btnContinue.Text = "Continue";
            this.Controls.Add(btnContinue);
        }

        /// <summary>
        /// Creates the asset type check box.
        /// </summary>
        private void CreateAssetTypeCheckBox()
        {
            chkblAssetType = new CheckBoxList();
            chkblAssetType.ID = "chkblAssetType";
            chkblAssetType.RepeatDirection = RepeatDirection.Vertical;
            chkblAssetType.RepeatLayout = RepeatLayout.Table;
            chkblAssetType.Items.Add(new ListItem(BASINLIST, BASINLIST));
            chkblAssetType.Items.Add(new ListItem(COUNTRYLIST, COUNTRYLIST));
            this.Controls.Add(chkblAssetType);
        }

        /// <summary>
        /// Creates the close button.
        /// </summary>
        private void CreateCloseButton()
        {
            btnClose = new Button();
            btnClose.Visible = false;
            btnClose.Text = "Close";
            btnClose.OnClientClick = "window.close();";
            btnClose.CssClass = "buttonAdvSrch";
        }

        /// <summary>
        /// Creates the information lable.
        /// </summary>
        private void CreateInformationLable()
        {
            lblInformation = new Label();
            lblInformation.Visible = false;
            lblInformation.Text = "Synchonization between Basin,Country list and power hub database";
        }

        /// <summary>
        /// Creates the message lable.
        /// </summary>
        private void CreateMessageLable()
        {
            lblMessage = new Label();
            lblMessage.ID = "lblMessage";
            lblMessage.CssClass = "labelMessage";
            lblMessage.Visible = false;
        }
        #region FixingFormAction
        /// <summary>
        /// Ensures the panel fix.
        /// </summary>
        /// <param name="type">The type.</param>
        private void EnsurePanelFix(Type type)
        {
            if(this.Page.Form != null)
            {
                string formOnSubmitAtt = this.Page.Form.Attributes["onsubmit"];
                if(formOnSubmitAtt == "return _spFormOnSubmitWrapper();")
                {
                    this.Page.Form.Attributes["onsubmit"] = "_spFormOnSubmitWrapper();";
                }
            }
            ScriptManager.RegisterStartupScript(this, type, "UpdatePanelFixup", "_spOriginalFormAction = document.forms[0].action; _spSuppressFormOnSubmitWrapper=true;", true);
        }

        #endregion FixingFormAction
        #endregion
    }
}

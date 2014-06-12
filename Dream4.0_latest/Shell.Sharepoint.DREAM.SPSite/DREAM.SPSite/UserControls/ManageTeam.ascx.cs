#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: ManageTeam.cs
#endregion
using System.Collections.Generic;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;
using ShellEntities = Shell.SharePoint.DREAM.Business.Entities;
using System.Collections;
using System.Data;
using System;
using System.Web.UI.WebControls;
using System.Net;
using System.Web.Services.Protocols;
using System.Xml;

namespace Shell.SharePoint.DREAM.Site.UI
{
    /// <summary>
    /// Manage Team usercontrol handles the UI for team management
    /// </summary>
    public partial class ManageTeam : UIHelper
    {
        #region Declaration
        const string OWPROJECTS = "OWProjects";
        const string ATTRIBUTENODEXPATH = "/response/report/record/attribute";
        #endregion

        #region Protected Methods
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"></see> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnLoad(System.EventArgs e)
        {
            string strCamlQueryTeamId = string.Empty;
            try
            {
                if (!Page.IsPostBack)
                {
                    /// Multi Team Owner Implementation
                    /// Changed By: Yasotha
                    /// Date : 13-Jan-2010                    
                    lstTeamOwner.Items.Clear();
                    lstTeamOwner.Items.Add(new ListItem(DEFAULTDROPDOWNTEXT, string.Empty));
                    /// Multi Team Owner Implementation
                    /// End
                    cboPROJECTNAME.Items.Clear();
                    cboPROJECTNAME.Items.Add(new ListItem(DEFAULTDROPDOWNTEXT, string.Empty));

                    objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                    LoadMapBookMarks();

                    #region Load Project Name from Web Service
                    LoadListOfProjectsOW(cboPROJECTNAME, OWPROJECTS);
                    #endregion Load Project Name from Web Service
                    //load the control values in Edit Mode
                    if (Request.QueryString["idValue"] != null)
                    {
                        strCamlQueryTeamId = @"<Where><Or><Eq><FieldRef Name='TeamID' /><Value Type='Text'>" + Request.QueryString["idValue"] + "</Value></Eq><Eq><FieldRef Name='TeamID' /><Value Type='Text'>0</Value></Eq></Or></Where>";
                        /// Multi Team Owner Implementation
                        /// Changed By: Yasotha
                        /// Date : 13-Jan-2010   

                        ((MOSSServiceManager)objMossController).GetListItemsToComboBox(strCurrSiteUrl, USERACCESSREQUESTLIST, lstTeamOwner, strCamlQueryTeamId, "<FieldRef Name='DisplayName' /><FieldRef Name='ID' />");
                        /// Multi Team Owner Implementation
                        /// End
                        PopulateTeamList();
                    }
                    else
                    {
                        strCamlQueryTeamId = @"<Where><And><And><Eq><FieldRef Name='TeamID' /><Value Type='Text'>0</Value></Eq><Eq><FieldRef Name='IsTeamOwner' /><Value Type='Text'>No</Value></Eq></And><Eq><FieldRef Name='Active' /><Value Type='Text'>Yes</Value></Eq></And></Where>";

                        /// Multi Team Owner Implementation
                        /// Changed By: Yasotha
                        /// Date : 13-Jan-2010  

                        ((MOSSServiceManager)objMossController).GetListItemsToComboBox(strCurrSiteUrl, USERACCESSREQUESTLIST, lstTeamOwner, strCamlQueryTeamId, "<FieldRef Name='DisplayName' /><FieldRef Name='ID' />");
                        /// Multi Team Owner Implementation
                        /// End
                    }

                }
            }
            catch (WebException webEx)
            {
                CommonUtility.HandleException(strCurrSiteUrl, webEx, 1);
                ShowLableMessage(webEx.Message);
            }
            catch (SoapException soapEx)
            {
                CommonUtility.HandleException(strCurrSiteUrl, soapEx, 1);
                ShowLableMessage(soapEx.Message);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
            finally
            {
                if (dtListValues != null)
                    dtListValues.Dispose();
            }
        }

        /// <summary>
        /// Adds the team.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void AddTeam(object sender, System.EventArgs e)
        {
            string strSelectedTeamOwner = string.Empty;
            try
            {
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                Dictionary<string, string> dicListValues = null;

                dicListValues = new Dictionary<string, string>();

                if (Request.QueryString["idValue"] != null)
                {
                    dicListValues.Add("ID", Request.QueryString["idValue"].ToString());
                }

                dicListValues.Add("Title", txtTEAMNAME.Text.Trim());
                /// Multi Team Owner Implementation
                /// Changed By: Yasotha
                /// Date : 13-Jan-2010  

                foreach (ListItem listItem in lstTeamOwner.Items)
                {
                    if (listItem.Selected)
                    {
                        strSelectedTeamOwner += listItem.Text;
                        strSelectedTeamOwner += ";";
                    }

                }
                if (!string.IsNullOrEmpty(strSelectedTeamOwner))
                {
                    if (strSelectedTeamOwner.LastIndexOf(";") != -1 && strSelectedTeamOwner.LastIndexOf(";").Equals(strSelectedTeamOwner.Length - 1))
                    {
                        strSelectedTeamOwner = strSelectedTeamOwner.Remove(strSelectedTeamOwner.LastIndexOf(";"), 1);
                    }
                    dicListValues.Add("TeamOwner", strSelectedTeamOwner);
                }
                #region Project name is loaded from webservice since DREAM 3.0 release
                dicListValues.Add("ProjectTitle", cboPROJECTNAME.SelectedItem.Text);
                #endregion Project name is loaded from webservice since DREAM 3.0 release
                if (cboMAPBOOKMARK.SelectedIndex != 0)
                {
                    dicListValues.Add("MapBookMark", cboMAPBOOKMARK.SelectedItem.Text);
                }
                string strItemId = ((MOSSServiceManager)objMossController).UpdateListItem(dicListValues, TEAMREGISTRATIONLIST);

                if (hidTeamOwner.Value.Length > 0)
                {
                    string[] strTeamOwners = hidTeamOwner.Value.Split(";".ToCharArray());
                    if (strTeamOwners != null && strTeamOwners.Length > 0)
                    {
                        Dictionary<string, string> dicUserValues = new Dictionary<string, string>();
                        foreach (string strTeamOwner in strTeamOwners)
                        {
                            /// FindByValue must be used since hidTeamOwner.Value is Value field of ListItem. 
                            if (lstTeamOwner.Items.FindByValue(strTeamOwner) != null && !(lstTeamOwner.Items.FindByValue(strTeamOwner).Selected))
                            {
                                /// Reset the Dictionary object foreach item to avoid DuplicateException
                                dicUserValues = new Dictionary<string, string>();
                                dicUserValues.Add("ID", strTeamOwner);
                                dicUserValues.Add("TeamID", strItemId);
                                dicUserValues.Add("IsTeamOwner", "No");
                                ((MOSSServiceManager)objMossController).UpdateListItem(dicUserValues, USERACCESSREQUESTLIST);
                            }
                        }
                    }
                }
                foreach (ListItem listItem in lstTeamOwner.Items)
                {
                    /// Reset the Dictionary object foreach item to avoid DuplicateException
                    dicListValues = new Dictionary<string, string>();
                    if (listItem.Selected)
                    {
                        dicListValues.Add("ID", listItem.Value);
                        dicListValues.Add("TeamID", strItemId);
                        dicListValues.Add("IsTeamOwner", "Yes");
                        ((MOSSServiceManager)objMossController).UpdateListItem(dicListValues, USERACCESSREQUESTLIST);
                    }
                }
                /// Multi Team Owner Implementation
                /// End
                Response.Redirect("/Pages/TeamManagement.aspx", false);
            }
            catch (WebException webEx)
            {
                ShowLableMessage(webEx.Message);
            }
            catch (Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
        }

        /// <summary>
        /// Handles the ServerClick event of the BtnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void BtnCancel_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/TeamManagement.aspx", false);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Loads the map book marks.
        /// </summary>
        private void LoadMapBookMarks()
        {
            cboMAPBOOKMARK.Items.Clear();
            cboMAPBOOKMARK.Items.Add(new ListItem(DEFAULTDROPDOWNTEXT, string.Empty));

            ((MOSSServiceManager)objMossController).GetListItemsToComboBox(strCurrSiteUrl, MAPBOOKMARKSLIST, cboMAPBOOKMARK, "<Where><Eq><FieldRef Name=\"IsPublic\"/><Value Type=\"Boolean\">1</Value></FieldRef></Eq></Where>", "<FieldRef Name='BookMarkName' /><FieldRef Name='MapExtent' /><FieldRef Name='IsDefaultExtent' />");

        }

        /// <summary>
        /// Loads the list of projects OW.
        /// </summary>
        /// <param name="dropdown">The dropdown.</param>
        /// <param name="entityName">Name of the entity.</param>
        private void LoadListOfProjectsOW(DropDownList dropdown, string entityName)
        {
            XmlDocument objListOfProjectsXml = GetListOfProjectsOW(entityName);
            if (objListOfProjectsXml != null)
            {
                XmlNodeList objXmlNodeList = objListOfProjectsXml.SelectNodes(ATTRIBUTENODEXPATH);
                ListItem item = null;
                foreach (XmlNode objNode in objXmlNodeList)
                {
                    item = new ListItem(objNode.Attributes[VALUE].Value, objNode.Attributes[VALUE].Value, true);
                    dropdown.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// Gets the list of projects OW.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <returns></returns>
        private XmlDocument GetListOfProjectsOW(string entityName)
        {
            XmlDocument objListOfProjectsXml = null;
            XmlDocument objRequestXML = null;
            objRequestInfo = new ShellEntities.RequestInfo();
            objRequestInfo.Entity = new ShellEntities.Entity();
            objRequestInfo.Entity.Name = entityName;
            ShellEntities.Attributes objAttributes = new ShellEntities.Attributes();
            ShellEntities.Value objValue = new ShellEntities.Value();
            objAttributes.Name = "source";
            objValue.InnerText = STAROPERATOR;
            objAttributes.Value = new ArrayList();
            objAttributes.Value.Add(objValue);
            objAttributes.Operator = GetOperator(objAttributes.Value);
            objRequestInfo.Entity.Attribute = new ArrayList();
            objRequestInfo.Entity.Attribute.Add(objAttributes);
            objFactory = new ServiceProvider();
            objReportController = objFactory.GetServiceManager(REPORTSERVICE);
            objRequestInfo.Entity.ResponseType = string.Empty;
            objRequestXML = objReportController.CreateSearchRequest(objRequestInfo);
            objListOfProjectsXml = objReportController.GetSearchResults(objRequestXML, -1, OWPROJECTS, null, 0);
            return objListOfProjectsXml;
        }
        /// <summary>
        /// Shows the lable message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void ShowLableMessage(string message)
        {
            lblErrorMessage.Visible = true;
            lblErrorMessage.Text = message;
        }
        /// <summary>
        /// Populates the team list.
        /// </summary>
        private void PopulateTeamList()
        {
            string strCamlQueryId = string.Empty;
            string strViewFields = string.Empty;
            strCamlQueryId = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + Request.QueryString["idValue"] + "</Value></Eq></Where>";
            strViewFields = "<FieldRef Name='Title' /><FieldRef Name='TeamOwner' /><FieldRef Name='ProjectTitle' /><FieldRef Name='MapBookMark' />";
            dtListValues = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, TEAMREGISTRATIONLIST, strCamlQueryId, strViewFields);
            if((dtListValues != null) && (dtListValues.Rows.Count > 0))
            {
                txtTEAMNAME.Text = dtListValues.Rows[0]["Title"].ToString();
                lblAddTeam.Text = "Edit Team";
                lblTeam.Text = " : " + txtTEAMNAME.Text;
                cboPROJECTNAME.SelectedIndex = -1;

                /// Multi Team Owner Implementation
                /// Changed By: Yasotha
                /// Date : 13-Jan-2010   

                string[] strTeamOwners = dtListValues.Rows[0]["TeamOwner"].ToString().Split(";".ToCharArray());
                if(strTeamOwners != null && strTeamOwners.Length > 0)
                {
                    /// Clear previous selection
                    lstTeamOwner.ClearSelection();
                    foreach(string strTeamOwner in strTeamOwners)
                    {
                        if(lstTeamOwner.Items.FindByText(strTeamOwner) != null)
                        {
                            lstTeamOwner.Items.FindByText(strTeamOwner).Selected = true;

                            hidTeamOwner.Value += lstTeamOwner.Items.FindByText(strTeamOwner).Value;
                            hidTeamOwner.Value += ";";
                        }
                    }

                    if(hidTeamOwner.Value.LastIndexOf(";") != -1 && hidTeamOwner.Value.LastIndexOf(";").Equals(hidTeamOwner.Value.Length - 1))
                    {
                        hidTeamOwner.Value = hidTeamOwner.Value.Remove(hidTeamOwner.Value.LastIndexOf(";"), 1);
                    }
                }

                /// Multi Team Owner Implementation
                /// End
                #region Setting selected Project Name from SharePoint List
                if(cboPROJECTNAME.Items.FindByText(dtListValues.Rows[0]["ProjectTitle"].ToString()) != null)
                {
                    /// Clear previous selection
                    cboPROJECTNAME.ClearSelection();
                    cboPROJECTNAME.Items.FindByText(dtListValues.Rows[0]["ProjectTitle"].ToString()).Selected = true;
                }
                #endregion Setting selected Project Name from SharePoint List
                if(cboMAPBOOKMARK.Items.FindByText(dtListValues.Rows[0]["MapBookMark"].ToString()) != null)
                {
                    /// Clear previous selection
                    cboMAPBOOKMARK.ClearSelection();
                    cboMAPBOOKMARK.Items.FindByText(dtListValues.Rows[0]["MapBookMark"].ToString()).Selected = true;
                }
            }
        }
        #endregion

    }
}
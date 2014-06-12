#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: MOSSServiceManager.cs
#endregion

/// <summary> 
/// This class is the MOSSServiceManager which inherits from Abstract Controller.
/// </summary>
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Collections.Specialized;
using System.Xml;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Shell.SharePoint.DREAM.MOSSProcess;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.DREAM.Controller
{
    /// <summary>
    /// MOSSService Manager Class.
    /// </summary>
    public class MOSSServiceManager :AbstractController
    {
        #region PRIVATE MEMBERS
        const string DEFAULTPREFERENCESLIST = "Default Preferences";
        SaveSearchHandler objSaveSearchHandler;
        const string SAVESEARCHXSLPATH = "/saveSearchRequests/saveSearchRequest";
        const string ENTITYPATH = "requestinfo/entity";
        const string ATTRITYPE = "type";
        const string USERACCESSREQUESTLIST = "User Access Request";
        const string TEAMREGISTRATIONLIST = "Team Registration";
        const string PROJECTLIST = "Project";
        const string MAPBOOKMARKSLIST = "MapBookmarks";
        #endregion

        #region ABSTRACTMETHODS
        /// <summary>
        /// Fetches the results.
        /// </summary>
        /// <param name="searchRequest">The search request.</param>
        /// <returns></returns>
        public override XmlDocument FetchResults(XmlDocument searchRequest)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// Creates the search request.
        /// </summary>
        /// <param name="requestObject">The request object.</param>
        /// <returns></returns>
        public override XmlDocument CreateSearchRequest(object requestObject)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// Gets the search results.
        /// </summary>
        /// <returns></returns>
        public override XmlDocument GetSearchResults()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// Gets the search results.
        /// </summary>
        /// <param name="requestInfo">The request info.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns></returns>
        public override XmlDocument GetSearchResults(RequestInfo requestInfo, int maxRecords, string searchType, string sortColumn, int sortOrder)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// Gets the search results.
        /// </summary>
        /// <param name="searchRequest">The search request.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns></returns>
        public override XmlDocument GetSearchResults(XmlDocument searchRequest, int maxRecords, string searchType, string sortColumn, int sortOrder)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        #endregion

        #region MOSS PROCESS METHODS
        /// <summary>
        /// Reads the list.
        /// </summary>
        /// <param name="parentSiteUrl">The parent site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns>Gives the filtered row of a list</returns>
        public DataTable ReadList(String parentSiteUrl, String listName, String queryString)
        {
            DataTable objDataTable = null;
            Common objCommon = null;
            try
            {
                objDataTable = new DataTable();
                objCommon = new Common();
                //Reads the sharepoint List
                if(queryString.Length == 0)
                {
                    objDataTable = objCommon.ReadList(parentSiteUrl, listName);
                }
                else
                {
                    objDataTable = objCommon.ReadList(parentSiteUrl, listName, queryString);
                }
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if(objDataTable != null)
                    objDataTable.Dispose();
            }
            return objDataTable;
        }


        /// <summary>
        /// Reads the list.
        /// </summary>
        /// <param name="parentSiteUrl">The parent site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="queryString">The query string.</param>
        /// <param name="viewFields">The view fields.</param>
        /// <returns></returns>
        public DataTable ReadList(String parentSiteUrl, String listName, String queryString, string viewFields)
        {
            DataTable objDataTable = null;
            Common objCommon = null;
            try
            {
                objDataTable = new DataTable();
                objCommon = new Common();
                //Reads the sharepoint List
                if(queryString.Length == 0)
                {
                    objDataTable = objCommon.ReadList(parentSiteUrl, listName);
                }
                else
                {
                    objDataTable = objCommon.ReadList(parentSiteUrl, listName, queryString, viewFields);
                }
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if(objDataTable != null)
                    objDataTable.Dispose();
            }
            return objDataTable;
        }

        /// <summary>
        /// Determines whether the logged in user is a valid user or not.
        /// </summary>
        /// <param name="parentSiteURL">The parent site URL.</param>
        /// <param name="strUserID">Logged in userID.</param>
        /// <returns>
        /// 	<c>true</c> if the logged in user is a valid AD user admin; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValidUser(String parentSiteURL, String userID)
        {
            bool blnValidUser = false;
            Admin objAdmin = null;
            try
            {
                objAdmin = new Admin();
                //validates the user from AD
                blnValidUser = objAdmin.IsValidUser(parentSiteURL, userID);
            }
            catch(Exception)
            {
                throw;
            }
            return blnValidUser;
        }


        /// <summary>
        /// Determines whether the logged in user is a site admin or not.
        /// </summary>
        /// <param name="parentSiteURL">The parent site URL.</param>
        /// <returns>
        /// 	<c>true</c> if the logged in user is a site admin; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAdmin(String parentSiteURL, String userID)
        {
            bool blnAdmin = false;
            Admin objAdmin = null;
            try
            {
                objAdmin = new Admin();
                //Validats whether logged in user is a site admin 
                blnAdmin = objAdmin.IsAdmin(parentSiteURL, userID);
            }
            catch(Exception)
            {
                throw;
            }
            return blnAdmin;
        }


        /// <summary>
        /// Determines whether [is current user team owner] [the specified parent site URL].
        /// </summary>
        /// <param name="parentSiteURL">The parent site URL.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns>
        /// 	<c>true</c> if [is current user team owner] [the specified parent site URL]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsCurrentUserTeamOwner(string parentSiteURL, string userName)
        {
            try
            {
                DataTable dtUserTeam = ReadList(parentSiteURL, USERACCESSREQUESTLIST, "<Where><And><Eq><FieldRef Name='Title'/><Value Type=\"Text\">" + userName + "</Value></Eq><Eq><FieldRef Name=\"IsTeamOwner\"/><Value Type=\"Text\">Yes</Value></Eq></And></Where>");

                if(dtUserTeam.Rows.Count > 0)
                {
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Creates the access request.
        /// </summary>
        /// <param name="strUserId">The STR user id.</param>
        /// <param name="regionID">The region ID.</param>
        /// <param name="purpose">The purpose.</param>
        public void CreateAccessRequest(string userID, string regionID, string purpose, string teamId, string listName)
        {
            Admin objAdmin = null;
            try
            {
                objAdmin = new Admin();
                objAdmin.CreateAccessRequest(userID, regionID, purpose, teamId, listName);
            }
            catch(Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Determines whether [is request exist] [the specified parent site URL].
        /// </summary>
        /// <param name="parentSiteURL">The parent site URL.</param>
        /// <param name="strUserID">The STR user ID.</param>
        /// <returns>
        /// 	<c>true</c> if [is request exist] [the specified parent site URL]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsRequestExist(string parentSiteURL, string userID)
        {
            DataTable objListData = null;
            string strCamlQuery = string.Empty;
            Common objCommon = null;
            try
            {
                objCommon = new Common();
                strCamlQuery = "<OrderBy><FieldRef Name=\"LinkTitleNoMenu\" /></OrderBy><Where><And><Eq><FieldRef Name=\"LinkTitle\" /><Value Type=\"Computed\">" + userID + "</Value></Eq><Eq><FieldRef Name=\"Access_x0020_Approval_x0020_Stat\" /><Value Type=\"Choice\">Approved</Value></Eq></And></Where>";
                objListData = objCommon.ReadList(parentSiteURL, "User Access Request", strCamlQuery);

                if(objListData.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if(objListData != null)
                    objListData.Dispose();
            }
        }
        #endregion
        #region CUSTOM MESSAGE HANDLING
        ///// <summary>
        ///// Gets the custom exception message.
        ///// </summary>
        ///// <param name="strCustomExceptionID">The STR custom exception ID.</param>
        ///// <returns>Custom Exception Message</returns>
        public string GetCustomExceptionMessage(string parentSiteUrl, string exceptionID)
        {
            Configuration objConfiguration = new Configuration();
            try
            {
                return objConfiguration.GetCustomExceptionMessage(parentSiteUrl, exceptionID);
            }

            catch(Exception)
            {
                throw;
            }
        }
        #endregion
        #region USERPREFERENES
        /// <summary>
        /// Gets the user preferences.
        /// </summary>
        /// <param name="strUserID">Current logged in User ID.</param>
        /// <param name="strSiteLocation">Site location.</param>
        /// <returns>UserPerferences object</returns>
        public UserPreferences GetUserPreferences(string userID)
        {
            UserPreferenceHandler objUserPreferenceHandler = null;
            UserPreferences objPreferences = null;
            try
            {
                objPreferences = new UserPreferences();
                objUserPreferenceHandler = new UserPreferenceHandler();
                //reads the user preferences.
                objPreferences = objUserPreferenceHandler.GetUserPreferences(userID);
            }
            catch(Exception)
            {
                throw;
            }
            return objPreferences;
        }

        /// <summary>
        /// Gets the default user preferences.
        /// </summary>
        /// <param name="strUserID">Current logged in User ID.</param>
        /// <param name="strSiteLocation">Site location.</param>
        /// <returns>UserPerferences object</returns>
        public UserPreferences GetDefaultUserPreferences(string userID)
        {
            UserPreferenceHandler objUserPreferenceHandler = null;
            UserPreferences objPreferences = null;
            try
            {
                objPreferences = new UserPreferences();
                objUserPreferenceHandler = new UserPreferenceHandler();
                objPreferences = objUserPreferenceHandler.GetDefaultUserPreferences(userID);
            }
            catch(Exception)
            {
                throw;
            }
            return objPreferences;
        }


        /// <summary>
        /// Updates the feedback.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <param name="feedbackInfo">The feedback info.</param>
        /// <returns></returns>
        public bool UpdateFeedback(string userID, Feedback feedbackInfo)
        {
            bool blnIsUpdated = false;
            FeedbackHandler objFeedbackHandler = null;
            try
            {
                objFeedbackHandler = new FeedbackHandler();
                //updates the Feedback and sets the Flag.
                blnIsUpdated = objFeedbackHandler.UpdateFeedback(userID, feedbackInfo);
            }
            catch(Exception)
            {
                throw;
            }
            return blnIsUpdated;
        }
        /// <summary>
        /// Updates the user preferences.
        /// </summary>
        /// <param name="strUserID">user ID.</param>
        /// <param name="objPreferences">user preferences object.</param>
        /// <param name="strAction">action.</param>
        /// <param name="strStorageURL">storage URL.</param>
        /// <returns></returns>
        public bool UpdateUserPreferences(string userID, UserPreferences objPreferences, string action)
        {
            bool blnIsUpdated = false;
            UserPreferenceHandler objPreferencesHandler = null;
            try
            {
                objPreferencesHandler = new UserPreferenceHandler();
                //updates the user preferences and sets the Flag.
                blnIsUpdated = objPreferencesHandler.UpdateUserPreferences(userID, objPreferences, action);
            }
            catch(Exception)
            {
                throw;
            }
            return blnIsUpdated;
        }

        /// <summary>
        /// Gets the userpreferences value.
        /// </summary>
        /// <returns></returns>
        public UserPreferences GetUserPreferencesValue(string userID, string parentSiteURL)
        {
            Common objCommon = null;
            UserPreferences objPreferences = null;
            UserPreferenceHandler objUserPreferenceHandler = null;
            DataTable objDefaultPreferences = null;
            try
            {
                objCommon = new Common();
                objPreferences = new UserPreferences();
                objUserPreferenceHandler = new UserPreferenceHandler();

                objPreferences = objUserPreferenceHandler.GetUserPreferences(userID);
                if(objPreferences != null)
                {
                    return objPreferences;
                }
                else
                {
                    //Read 'Default Preferences' list and set to the object
                    objDefaultPreferences = new DataTable();
                    objPreferences = new UserPreferences();
                    objDefaultPreferences = objCommon.ReadList(parentSiteURL, DEFAULTPREFERENCESLIST);
                    foreach(DataRow objDataRow in objDefaultPreferences.Rows)
                    {
                        if(string.Equals(objDataRow["Title"].ToString(), "Display"))
                        {
                            objPreferences.Display = objDataRow["Default_x0020_Value"].ToString();
                        }
                        else if(string.Equals(objDataRow["Title"].ToString(), "DepthUnits"))
                        {
                            objPreferences.DepthUnits = objDataRow["Default_x0020_Value"].ToString();
                        }
                        else if(string.Equals(objDataRow["Title"].ToString(), "RecordsPerPage"))
                        {
                            objPreferences.RecordsPerPage = objDataRow["Default_x0020_Value"].ToString();
                        }
                    }
                    return objPreferences;
                }
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if(objDefaultPreferences != null)
                    objDefaultPreferences.Dispose();
            }
        }
        /// <summary>
        /// Gets the userpreferences from session.
        /// </summary>
        /// <returns></returns>
        public UserPreferences GetUserPreferencesFromSession(Page page, string userID, string parentSiteURL)
        {
            UserPreferences objPreferences = new UserPreferences();
            object objSessionUserPreference = null;
            try
            {
                objSessionUserPreference = CommonUtility.GetSessionVariable(page, enumSessionVariable.UserPreferences.ToString());
                //validates the user preferences session variable.
                if(objSessionUserPreference != null)
                {
                    objPreferences = (UserPreferences)objSessionUserPreference;
                }
                else
                {
                    objPreferences = GetUserPreferencesValue(userID, parentSiteURL);
                }
            }
            catch(Exception)
            {
                throw;
            }
            return objPreferences;
        }
        #endregion
        #region XML XSL Methods
        /// <summary>
        /// Gets the XSL template.
        /// </summary>
        /// <param name="strType">type</param>
        /// <param name="strParentSiteUrl">parent site url.</param>
        /// <returns></returns>
        public XmlTextReader GetXSLTemplate(string type, string parentSiteUrl)
        {
            XmlTextReader objXmlTextReader = null;
            Common objCommon = null;
            try
            {
                objCommon = new Common();
                //Get the XSLTemplate from sharepoint list
                objXmlTextReader = objCommon.GetXSLTemplate(type, parentSiteUrl);
            }
            catch(Exception)
            {
                throw;
            }
            return objXmlTextReader;
        }
        #endregion
        #region SAVESEARCH
        /// <summary>
        /// Loads the save search.
        /// </summary>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="comboBoxControl">The combo box control.</param>
        public void LoadSaveSearch(string searchType, DropDownList comboBoxControl)
        {
            objSaveSearchHandler = new SaveSearchHandler();
            try
            {
                objSaveSearchHandler.LoadSaveSearch(searchType, comboBoxControl);
            }
            catch(Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the doc lib XML file.
        /// </summary>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="userID">The user ID.</param>
        /// <returns></returns>
        public XmlDocument GetDocLibXMLFile(string searchType, string userID)
        {
            XmlDocument objXmlDocument = new XmlDocument();
            objSaveSearchHandler = new SaveSearchHandler();
            try
            {
                objXmlDocument = objSaveSearchHandler.GetDocLibXMLFile(searchType, userID);
            }
            catch(Exception)
            {
                throw;
            }
            return objXmlDocument;
        }

        /// <summary>
        /// Gets the admin save search XML.
        /// </summary>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="userID">The user ID.</param>
        /// <param name="strSaveSearchName">Name of the STR save search.</param>
        /// <returns></returns>
        public XmlDocument GetAdminSaveSearchXML(string searchType, string userID, string saveSearchName)
        {
            XmlDocument objXmlDocument = new XmlDocument();
            objSaveSearchHandler = new SaveSearchHandler();
            try
            {
                objXmlDocument = objSaveSearchHandler.GetAdminSaveSearchXML(searchType, userID, saveSearchName);
            }
            catch(Exception)
            {
                throw;
            }
            return objXmlDocument;
        }

        /// <summary>
        /// Gets the save order number.
        /// </summary>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="userID">The user ID.</param>
        /// <returns></returns>
        public ArrayList GetSaveOrderNumber(string searchType, string userID)
        {
            ArrayList arlListSaveSearch = new ArrayList();
            objSaveSearchHandler = new SaveSearchHandler();
            try
            {
                arlListSaveSearch = objSaveSearchHandler.GetSaveOrderNumber(searchType, userID);
            }
            catch(Exception)
            {
                throw;
            }
            return arlListSaveSearch;
        }

        /// <summary>
        /// Gets the name of the save search.
        /// </summary>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="userID">The user ID.</param>
        /// <returns></returns>
        public ArrayList GetSaveSearchName(string searchType, string userID)
        {
            ArrayList arlListSaveSearch = new ArrayList();
            objSaveSearchHandler = new SaveSearchHandler();
            try
            {
                //get the saved search names based on the search type.
                arlListSaveSearch = objSaveSearchHandler.GetSaveSearchName(searchType, userID);
            }
            catch(Exception)
            {
                throw;
            }
            return arlListSaveSearch;
        }

        /// <summary>
        /// Gets the name of the shared save search.
        /// </summary>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="userID">The user ID.</param>
        /// <returns></returns>
        public ArrayList GetSharedSaveSearchName(string searchType, string userID, bool fetchAll)
        {
            ArrayList arlListSaveSearch = new ArrayList();
            objSaveSearchHandler = new SaveSearchHandler();
            try
            {
                //get the shared save search names based on the search type.
                arlListSaveSearch = objSaveSearchHandler.GetSharedSaveSearchName(searchType, userID, fetchAll);
            }
            catch(Exception)
            {
                throw;
            }
            return arlListSaveSearch;
        }
        #endregion
        /// <summary>
        /// Reads the list.
        /// </summary>
        /// <param name="parentSiteUrl">The parent site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns>Gives the filtered row of a list</returns>
        public DataTable ReadFolderList(String parentSiteUrl, String listName, String queryString)
        {
            DataTable objDataTable = null;
            Common objCommon = null;
            try
            {
                objDataTable = new DataTable();
                objCommon = new Common();
                //Reads the sharepoint List
                if(queryString.Length == 0)
                {
                    objDataTable = objCommon.ReadFolderList(parentSiteUrl, listName, queryString);
                }
                else
                {
                    objDataTable = objCommon.ReadFolderList(parentSiteUrl, listName, queryString);
                }
            }
            catch(Exception)
            {
                throw;
            }
            return objDataTable;
        }
        /// <summary>
        /// Deletes the save search.
        /// </summary>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="userID">The user ID.</param>
        /// <param name="saveSearchName">Name of the save search.</param>
        public void DeleteSaveSearch(string searchType, string userID, string saveSearchName)
        {
            XmlDocument xmlSaveSearchDoc = null;
            XmlNodeList xmlnodelistSaveSearch;
            int intSearchOrderCount = 0;
            try
            {
                objSaveSearchHandler = new SaveSearchHandler();
                //Gets the Xml Document for the current user.
                xmlSaveSearchDoc = GetDocLibXMLFile(searchType, userID);
                if(xmlSaveSearchDoc != null)
                {
                    xmlnodelistSaveSearch = xmlSaveSearchDoc.SelectNodes(SAVESEARCHXSLPATH);
                    foreach(XmlNode xmlNodeSaveSearch in xmlnodelistSaveSearch)
                    {
                        //Checks whether the name of the XmlNode is equals to the SaveSearch Name user has selected to delete.
                        if(string.Compare(xmlNodeSaveSearch.Attributes.GetNamedItem("name").Value.ToString(), saveSearchName) == 0)
                        {
                            xmlSaveSearchDoc.DocumentElement.RemoveChild(xmlNodeSaveSearch);
                            intSearchOrderCount++;
                        }
                        if(intSearchOrderCount > 0)
                        {
                            if(xmlNodeSaveSearch.Attributes.GetNamedItem("order") != null)
                            {
                                int intOrder = Convert.ToInt32(xmlNodeSaveSearch.Attributes.GetNamedItem("order").Value.ToString());
                                intOrder = intOrder - 1;
                                xmlNodeSaveSearch.Attributes.GetNamedItem("order").Value = intOrder.ToString();
                            }
                        }
                    }
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xmlSaveSearchDoc.OuterXml);
                    if(xmlSaveSearchDoc != null)
                        objSaveSearchHandler.UploadToDocumentLib(searchType, userID, xmlDoc);
                }
            }
            catch(Exception Ex)
            {
                throw Ex;
            }
        }
        /// <summary>
        /// Deletes the save search.
        /// </summary>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="userID">The user ID.</param>
        /// <param name="saveSearchName">Name of the save search.</param>
        public void ModifySaveSearch(string searchType, string userID, string saveSearchName, XmlDocument updatedXmlDocument, string shared)
        {
            XmlDocument xmlSaveSearchDoc = null;
            XmlNodeList xmlnodelistSaveSearch;
            try
            {
                objSaveSearchHandler = new SaveSearchHandler();
                //Gets the Xml Document for the current user.
                xmlSaveSearchDoc = GetDocLibXMLFile(searchType, userID);
                if(xmlSaveSearchDoc != null)
                {
                    xmlnodelistSaveSearch = xmlSaveSearchDoc.SelectNodes(SAVESEARCHXSLPATH);
                    foreach(XmlNode xmlNodeSaveSearch in xmlnodelistSaveSearch)
                    {
                        //Checks whether the name of the XmlNode is equals to the SaveSearch Name user has selected to delete.
                        if(string.Compare(xmlNodeSaveSearch.Attributes.GetNamedItem("name").Value.ToString(), saveSearchName) == 0)
                        {
                            xmlNodeSaveSearch.RemoveChild(xmlNodeSaveSearch.FirstChild);
                            xmlNodeSaveSearch.Attributes.GetNamedItem("shared").Value = shared;
                            xmlNodeSaveSearch.InnerXml = updatedXmlDocument.OuterXml;
                            if(updatedXmlDocument.SelectSingleNode(ENTITYPATH).Attributes[ATTRITYPE] != null)
                            {
                                string strType = updatedXmlDocument.SelectSingleNode(ENTITYPATH).Attributes[ATTRITYPE].Value;
                                xmlNodeSaveSearch.Attributes.GetNamedItem(ATTRITYPE).Value = strType;
                            }
                        }
                    }
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xmlSaveSearchDoc.OuterXml);
                    if(xmlDoc != null)
                        objSaveSearchHandler.UploadToDocumentLib(searchType, userID, xmlDoc);
                }
            }
            catch(Exception Ex)
            {
                throw Ex;
            }
        }
        /// <summary>
        /// Sends the feed back mail.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="messageBody">The message body.</param>
        public void SendFeedBackMail(StringDictionary header, string messageBody)
        {
            FeedbackHandler objFeedBack = null;
            try
            {
                objFeedBack = new FeedbackHandler();
                objFeedBack.SendAlertMail(header, messageBody);
            }
            catch(Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Saves the date format.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void SaveDateFormat(string key, string value, Dictionary<string, string> configItems)
        {
            Common objCommon = null;
            try
            {
                objCommon = new Common();
                objCommon.SaveDateFormat(key, value, configItems);
            }
            catch(Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Gets the tree view XML.
        /// </summary>
        /// <returns></returns>
        public XmlDocument GetTreeViewXML(string siteURL)
        {
            try
            {
                QuerySearchTreeViewGenerator objTreeGenerator = new QuerySearchTreeViewGenerator();
                return (objTreeGenerator.CreateTreeViewXml(siteURL));
            }
            catch(Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes the list item.
        /// </summary>
        /// <param name="listName">Name of the list.</param>
        /// <param name="itemId">The item id.</param>
        public void DeleteListItem(string listName, string itemId)
        {
            Common objCommon = null;
            try
            {
                objCommon = new Common();
                objCommon.DeleteListItem(listName, itemId);
            }
            catch
            {
                throw;
            }
        }



        /// <summary>
        /// Updates the list item.
        /// </summary>
        /// <param name="listValues">The list values.</param>
        /// <param name="listName">Name of the list.</param>
        public string UpdateListItem(Dictionary<string, string> listValues, string listName)
        {
            Common objCommon = null;
            try
            {
                objCommon = new Common();
                return objCommon.UpdateListItem(listValues, listName);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Gets the list items to combo box.
        /// </summary>
        /// <param name="parentSiteUrl">The parent site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="comboDropDown">The combo drop down.</param>
        /// <param name="query">The query.</param>
        /// <param name="viewFields">The view fields.</param>
        public void GetListItemsToComboBox(string parentSiteUrl, string listName, ListControl comboDropDown, string query, string viewFields)
        {
            try
            {
                DataTable objDataTable = ReadList(parentSiteUrl, listName, query, viewFields);
                comboDropDown.SelectedIndex = -1;
                ListItem objListItem = null;

                foreach(DataRow objRow in objDataTable.Rows)
                {
                    switch(listName)
                    {
                        case TEAMREGISTRATIONLIST:
                            {
                                objListItem = new ListItem(objRow["Title"].ToString(), objRow["ID"].ToString());
                                break;
                            }
                        case USERACCESSREQUESTLIST:
                            {
                                objListItem = new ListItem(objRow["DisplayName"].ToString(), objRow["ID"].ToString());
                                break;
                            }
                        case MAPBOOKMARKSLIST:
                            {
                                objListItem = new ListItem(objRow["BookMarkName"].ToString(), objRow["MapExtent"].ToString());
                                break;
                            }
                        case PROJECTLIST:
                            {
                                objListItem = new ListItem(objRow["Title"].ToString(), objRow["ID"].ToString());
                                break;
                            }

                    }

                    comboDropDown.Items.Add(objListItem);
                }
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Reads the XML file from share point.
        /// </summary>
        /// <param name="currentSiteUrl">The current site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public XmlDocument ReadXmlFileFromSharePoint(string currentSiteUrl, string listName, string query)
        {
            Common objCommon = null;
            try
            {
                objCommon = new Common();
                return objCommon.ReadXmlFileFromSharePoint(currentSiteUrl, listName, query);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Uploads to document lib.
        /// </summary>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="userID">The user ID.</param>
        /// <param name="finalDocument">The final document.</param>
        public void UploadToDocumentLib(string searchType, string userID, XmlDocument finalDocument)
        {
            objSaveSearchHandler = new SaveSearchHandler();
            try
            {
                if(finalDocument != null)
                {
                    objSaveSearchHandler.UploadToDocumentLib(searchType, userID, finalDocument);
                }
            }
            catch(Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Determines whether [is doc lib file exist] [the specified doc lib name].
        /// </summary>
        /// <param name="docLibName">Name of the doc lib.</param>
        /// <param name="userID">The user ID.</param>
        /// <returns>
        /// 	<c>true</c> if [is doc lib file exist] [the specified doc lib name]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsDocLibFileExist(string docLibName, string userID)
        {
            objSaveSearchHandler = new SaveSearchHandler();
            bool blnIsDocLibFileExist = false;
            try
            {
                blnIsDocLibFileExist = objSaveSearchHandler.IsDocLibFileExist(docLibName, userID);
            }
            catch(Exception)
            {
                throw;
            }
            return blnIsDocLibFileExist;
        }
        /// <summary>
        /// Gets the chached list items.
        /// </summary>
        /// <param name="currentSiteUrl">The current site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public SiteMapNodeCollection GetChachedListItems(string currentSiteUrl, string listName, string query)
        {
            Common objCommon = new Common();
            return objCommon.GetChachedListItems(currentSiteUrl, listName, query);
        }

        /// <summary>
        /// Gets the chached list items.
        /// </summary>
        /// <param name="currentSiteUrl">The current site URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="spQuery">The sp query.</param>
        /// <returns></returns>
        public SiteMapNodeCollection GetChachedListItems(string currentSiteUrl, string listName, SPQuery spQuery)
        {
            Common objCommon = new Common();
            return objCommon.GetChachedListItems(currentSiteUrl, listName, spQuery);
        }
        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="currentSiteUrl">The current site URL.</param>
        /// <param name="docLibName">Name of the doc lib.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public bool DeleteXMLFile(string currentSiteUrl, string docLibName, string fileName)
        {
            Common objCommon = new Common();
            return objCommon.DeleteXMLFile(currentSiteUrl, docLibName, fileName);
        }
        /// <summary>
        /// Determines whether [is doc lib exist] [the specified doc lib name].
        /// </summary>
        /// <param name="docLibName">Name of the doc lib.</param>
        /// <param name="currentSiteUrl">The current site URL.</param>
        /// <returns>
        /// 	<c>true</c> if [is doc lib exist] [the specified doc lib name]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsDocLibExist(string docLibName, string currentSiteUrl)
        {
            objSaveSearchHandler = new SaveSearchHandler();
            return objSaveSearchHandler.IsDocLibExist(docLibName, currentSiteUrl);
        }
    }
}



#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: UserPreferenceHandler.cs
#endregion

using System;
using Microsoft.SharePoint;
using System.Collections;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.DREAM.MOSSProcess
{
    /// <summary>
    /// The UserPreferenceHandler class to handle all the userpreference related functions.
    /// </summary>
    public class UserPreferenceHandler
    {
        #region Declaration
        const string USERPREFERENCESLIST = "User Preferences";
        const string USERDEFINEDLINKSLIST = "User Defined Links";
        const string SELECT = "---Select---";
        const string UPDATE = "Update";
        const string CREATE = "Create";
        SPQuery objSPQuery = new SPQuery();
        #endregion
        #region Public Methods
        /// <summary>
        /// Gets the preferences set by the logged in User. This method returns the User Preferences Object
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <returns>UserPerferences object</returns>
        internal UserPreferences GetUserPreferences(string userID)
        {
            SPListItemCollection prefListItemColl = null;
            SPListItemCollection userDefLinksColl = null;

            UserPreferences objPreferences = null;
            string strSiteLocation = string.Empty;
            try
            {
                strSiteLocation = SPContext.Current.Site.Url.ToString();
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(strSiteLocation))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            objSPQuery.Query = "<Where><Eq><FieldRef Name=\"UserID\" /><Value Type=\"Text\">" + userID
                                             + "</Value></Eq></Where>";
                            prefListItemColl = web.Lists[USERPREFERENCESLIST].GetItems(objSPQuery);

                            if (prefListItemColl != null && prefListItemColl.Count != 0)
                            {
                                objPreferences = new UserPreferences();

                                if (prefListItemColl[0]["Default_x0020_Asset"] != null)
                                    objPreferences.Asset = prefListItemColl[0]["Default_x0020_Asset"].ToString();

                                if (prefListItemColl[0]["Default_x0020_Basin"] != null)
                                    objPreferences.Basin = prefListItemColl[0]["Default_x0020_Basin"].ToString();

                                if (prefListItemColl[0]["Default_x0020_Country"] != null)
                                    objPreferences.Country = prefListItemColl[0]["Default_x0020_Country"].ToString();
                                if (prefListItemColl[0]["Title"] != null)
                                    objPreferences.Display = prefListItemColl[0]["Title"].ToString();
                                if (prefListItemColl[0]["Records_x0020_Per_x0020_Page"] != null)
                                    objPreferences.RecordsPerPage = prefListItemColl[0]["Records_x0020_Per_x0020_Page"].ToString();
                                if (prefListItemColl[0]["Depth_x0020_Units"] != null)
                                    objPreferences.DepthUnits = prefListItemColl[0]["Depth_x0020_Units"].ToString();
                                //Dream 3.0 code
                                //Start
                                if (prefListItemColl[0]["Pressure_x0020_Units"] != null)
                                    objPreferences.PressureUnits = prefListItemColl[0]["Pressure_x0020_Units"].ToString();
                                if (prefListItemColl[0]["Temperature_x0020_Units"] != null)
                                    objPreferences.TemperatureUnits = prefListItemColl[0]["Temperature_x0020_Units"].ToString();
                                //End

                                objSPQuery.Query = "<Where><Eq><FieldRef Name=\"UserID\" /><Value Type=\"Text\">" + userID
                                                 + "</Value></Eq></Where>";
                                userDefLinksColl = web.Lists[USERDEFINEDLINKSLIST].GetItems(objSPQuery);

                                ArrayList arlUserDefLinks = new ArrayList();
                                for (int index = 0; index < userDefLinksColl.Count; index++)
                                {
                                    URL objURL = new URL();
                                    if (userDefLinksColl[index]["Title"] != null)
                                        objURL.URLTitle = userDefLinksColl[index]["Title"].ToString();
                                    if (userDefLinksColl[index]["URL"] != null)
                                        objURL.URLValue = userDefLinksColl[index]["URL"].ToString();
                                    arlUserDefLinks.Add(objURL);
                                }
                                objPreferences.URL = arlUserDefLinks;
                            }
                        }
                    }
                });
            }
            catch (Exception)
            {
                throw;
            }
            return objPreferences;
        }

        /// <summary>
        /// Gets the default user preferences stored in the Sharepoint List. 
        /// if the user has not set any Preferences explicitly, the default preferences will be taken from the 
        /// Sharepoint list with the default values
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <returns>UserPerferences object</returns>
        internal UserPreferences GetDefaultUserPreferences(string userID)
        {
            SPListItemCollection prefListItemColl = null;
            SPListItemCollection userDefLinksColl = null;
            UserPreferences objPreferences = null;
            string strSiteLocation = string.Empty;
            try
            {
                strSiteLocation = SPContext.Current.Site.Url.ToString();
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(strSiteLocation))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            objSPQuery.Query = "<Where><Eq><FieldRef Name=\"UserID\" /><Value Type=\"Text\">" + userID
                                             + "</Value></Eq></Where>";
                            prefListItemColl = web.Lists[USERPREFERENCESLIST].GetItems(objSPQuery);

                            if (prefListItemColl != null && prefListItemColl.Count != 0)
                            {
                                objPreferences = new UserPreferences();

                                if (prefListItemColl[0]["Default_x0020_Asset"] != null)
                                    objPreferences.Asset = prefListItemColl[0]["Default_x0020_Asset"].ToString();

                                if (prefListItemColl[0]["Default_x0020_Basin"] != null)
                                    objPreferences.Basin = prefListItemColl[0]["Default_x0020_Basin"].ToString();
                                if (prefListItemColl[0]["Default_x0020_Country"] != null)
                                    objPreferences.Country = prefListItemColl[0]["Default_x0020_Country"].ToString();
                                if (prefListItemColl[0]["Title"] != null)
                                    objPreferences.Display = prefListItemColl[0]["Title"].ToString();
                                //objPreferences.Field = prefListItemColl[0]["Default_x0020_Field"].ToString();
                                if (prefListItemColl[0]["Records_x0020_Per_x0020_Page"] != null)
                                    objPreferences.RecordsPerPage = prefListItemColl[0]["Records_x0020_Per_x0020_Page"].ToString();
                                if (prefListItemColl[0]["Depth_x0020_Units"] != null)
                                    objPreferences.DepthUnits = prefListItemColl[0]["Depth_x0020_Units"].ToString();
                                //Dream 3.0 code
                                //Start
                                if (prefListItemColl[0]["Pressure_x0020_Units"] != null)
                                    objPreferences.PressureUnits = prefListItemColl[0]["Pressure_x0020_Units"].ToString();
                                if (prefListItemColl[0]["Temperature_x0020_Units"] != null)
                                    objPreferences.TemperatureUnits = prefListItemColl[0]["Temperature_x0020_Units"].ToString();
                                //End

                                objSPQuery.Query = "<Where><Eq><FieldRef Name=\"UserID\" /><Value Type=\"Text\">" + userID
                                                 + "</Value></Eq></Where>";
                                userDefLinksColl = web.Lists[USERDEFINEDLINKSLIST].GetItems(objSPQuery);
                                URL objURL = new URL();
                                ArrayList arlUserDefLinks = new ArrayList();
                                for (int index = 0; index < userDefLinksColl.Count; index++)
                                {
                                    objURL.URLTitle = userDefLinksColl[0]["Title"].ToString();
                                    objURL.URLValue = userDefLinksColl[0]["URL"].ToString();
                                    //objURL.Tooltip = userDefLinksColl[0]["Tooltip"].ToString();
                                    arlUserDefLinks.Add(objURL);
                                }
                                objPreferences.URL = arlUserDefLinks;
                            }
                        }
                    }
                });
            }
            catch (Exception)
            {
                throw;
            }
            return objPreferences;
        }

        /// <summary>
        /// Updates the user preferences when the user changes his preferences data.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <param name="preferencesSetByUser">The preferences set by user.</param>
        /// <param name="action">The action.</param>
        /// <returns>
        /// boolean returns whether update is success
        /// </returns>        
        internal bool UpdateUserPreferences(string userID, UserPreferences preferencesSetByUser, string action)
        {
            #region Method Variables
            bool blnIsSuccess = false;
            SPList preferencesList, userDefLinksList;
            SPQuery query;
            SPListItemCollection preferencesItemsColl = null;
            SPListItemCollection userDefLinksColl = null;
            SPListItem preferencesItem;
            #endregion

            try
            {
                string strStorageURL = SPContext.Current.Site.Url;
                // using(SPSite newSite = new SPSite(strStorageURL))
                SPSecurity.RunWithElevatedPrivileges(delegate()
                 {
                     using (SPSite site = new SPSite(strStorageURL))
                     {
                         using(SPWeb web = site.OpenWeb())
                         {
                             preferencesList = web.Lists[USERPREFERENCESLIST];
                             userDefLinksList = web.Lists[USERDEFINEDLINKSLIST];

                             //if user has not selected anything, blank value needs to be updated in the 
                             //list instead "---Select---"
                             if (string.Compare(preferencesSetByUser.Asset, SELECT, true) == 0)
                                 preferencesSetByUser.Asset = " ";

                             if (string.Compare(preferencesSetByUser.Basin, SELECT, true) == 0)
                                 preferencesSetByUser.Basin = " ";

                             if (string.Compare(preferencesSetByUser.Country, SELECT, true) == 0)
                                 preferencesSetByUser.Country = " ";

                             if (string.Compare(preferencesSetByUser.Field, SELECT, true) == 0)
                                 preferencesSetByUser.Field = " ";


                             if (action == UPDATE)
                             {
                                 web.AllowUnsafeUpdates = true;
                                 query = new SPQuery();
                                 query.Query = "<Where><Eq><FieldRef Name=\"UserID\" /><Value Type=\"Text\">" + userID + "</Value></Eq></Where>";

                                 //update items in UserPreferences List
                                 preferencesItemsColl = preferencesList.GetItems(query);
                                 preferencesItem = preferencesItemsColl[0];
                                 preferencesItem["Default_x0020_Asset"] = preferencesSetByUser.Asset;
                                 preferencesItem["Default_x0020_Basin"] = preferencesSetByUser.Basin;
                                 preferencesItem["Default_x0020_Country"] = preferencesSetByUser.Country;
                                 preferencesItem["Depth_x0020_Units"] = preferencesSetByUser.DepthUnits;
                                 //preferencesItem["Map_x0020_Display"] = preferencesSetByUser.MapDisplay;
                                 preferencesItem["Title"] = preferencesSetByUser.Display;
                                 //preferencesItem["Default_x0020_Field"] = preferencesSetByUser.Field;
                                 preferencesItem["Records_x0020_Per_x0020_Page"] = preferencesSetByUser.RecordsPerPage;
                                 //Dream 3.0 code
                                 //Start
                                 preferencesItem["Pressure_x0020_Units"] = preferencesSetByUser.PressureUnits;
                                 preferencesItem["Temperature_x0020_Units"] = preferencesSetByUser.TemperatureUnits;
                                 //End
                                 preferencesItem.Update();

                                 //update items in UserDefinedLinks list
                                 userDefLinksColl = userDefLinksList.GetItems(query);
                                 DeleteLinkItems(userDefLinksColl);  //Delete all existing links; and add new
                                 AddUserDefinedLinks(preferencesSetByUser.URL, userDefLinksList, userID);  //add new item to UserDefinedLinks list
                                 web.AllowUnsafeUpdates = false;
                                 blnIsSuccess = true;
                             }
                             else if (action == CREATE)
                             {
                                 web.AllowUnsafeUpdates = true;

                                 //add new item to UserPreferences list
                                 preferencesItem = preferencesList.Items.Add();
                                 preferencesItem["Default_x0020_Asset"] = preferencesSetByUser.Asset;
                                 preferencesItem["Default_x0020_Basin"] = preferencesSetByUser.Basin;
                                 preferencesItem["Default_x0020_Country"] = preferencesSetByUser.Country;
                                 preferencesItem["Depth_x0020_Units"] = preferencesSetByUser.DepthUnits;
                                 //preferencesItem["Map_x0020_Display"] = preferencesSetByUser.MapDisplay;
                                 preferencesItem["Title"] = preferencesSetByUser.Display;
                                 //preferencesItem["Default_x0020_Field"] = preferencesSetByUser.Field;
                                 preferencesItem["Records_x0020_Per_x0020_Page"] = preferencesSetByUser.RecordsPerPage;
                                 preferencesItem["UserID"] = userID;
                                 //Dream 3.0 code
                                 //Start
                                 preferencesItem["Pressure_x0020_Units"] = preferencesSetByUser.PressureUnits;
                                 preferencesItem["Temperature_x0020_Units"] = preferencesSetByUser.TemperatureUnits;
                                 //End
                                 preferencesItem.Update();

                                 AddUserDefinedLinks(preferencesSetByUser.URL, userDefLinksList, userID);  //add new item to UserDefinedLinks list
                                 web.AllowUnsafeUpdates = false;
                                 blnIsSuccess = true;
                             }
                         }
                     }
                 });
            }
            catch (Exception)
            {
                throw;
            }
            return blnIsSuccess;
        }
        #endregion
        #region Private Methods
        /// <summary>
        /// Deletes the list items from specified ListCollection.
        /// </summary>
        /// <param name="listItems">The listItem collection.</param>
        private void DeleteLinkItems(SPListItemCollection listItems)
        {
            try
            {
                for (int intIndex = listItems.Count - 1; intIndex > -1; intIndex--)
                {
                    listItems.Delete(intIndex);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Adds the list items from specified ListCollection.
        /// </summary>
        /// <param name="alURL">The arraylist of URL object.</param>
        /// <param name="userDefLinksList">The SPList to add items.</param>
        /// <param name="strUserID">logged in userid.</param>
        private void AddUserDefinedLinks(ArrayList URL, SPList userDefLinksList, string userID)
        {
            SPListItem userDefLinksItem;
            try
            {
                URL objURL = new URL();
                for (int intIndex = 0; intIndex < URL.Count; intIndex++)
                {
                    objURL = (URL)URL[intIndex];
                    if (objURL.URLTitle.Length > 0 || objURL.URLValue.Length > 0)
                    {
                        userDefLinksItem = userDefLinksList.Items.Add();
                        userDefLinksItem["UserID"] = userID;
                        userDefLinksItem["Title"] = objURL.URLTitle;
                        userDefLinksItem["URL"] = objURL.URLValue;
                        userDefLinksItem["Tooltip"] = objURL.URLValue;
                        userDefLinksItem.Update();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}

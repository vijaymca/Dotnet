#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: PortalConfiguration.cs
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using Shell.SharePoint.DREAM.Controller;
using System.Web;
using System.Data;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing.Navigation;
namespace Shell.SharePoint.DREAM.Utilities
{
    /// <summary>
    /// This class will be used to read the portal configurable items.
    /// This class wills perform/handle the following activities
    /// A) Retrieving the configurable items based on key pair 
    /// B) Storing the configurable items in cache (static class)
    /// C) Updating the configurable items whenever its getting changed (Add/Modify/Delete)
    /// D) Implementing the singleton pattern to avoid creating the multiple 
    ///    instance of the same class
    /// </summary>
    public class PortalConfiguration
    {
        #region Declaration
        private static PortalConfiguration objInstance;
        private const string CONFIGLISTNAME = "Portal Configurations";
        private const string TITLE = "Title";
        private const string VALUE = "Value";
        private const string NEW = "New";
        private const string UPDATE = "Update";
        private const string DELETE = "Delete";
        private static Dictionary<string, string> ConfigItems = new Dictionary<string, string>();
        AbstractController objMossController;
        #endregion
        #region Consructor
        /// <summary>
        /// Private construction - to prevent creating instance.
        /// </summary>
        private PortalConfiguration()
        {
        }
        #endregion
        # region Methods
      

        /// <summary>
        /// Provide the instance of class 
        /// </summary>
        /// <returns></returns>
        public static PortalConfiguration GetInstance()
        {
            // Uses "Lazy initialization"
            if (objInstance == null)
                objInstance = new PortalConfiguration();
            return objInstance;
        }
     
        /// <summary>
        /// Finds the key from event handler as website URL is required to access the sharepoint list.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string GetKeyForEventHandler(string key, string siteURL)
        {
            return FindKey(key, siteURL);
        }
        #region Old FindKey
        ///// <summary>
        ///// Finds the key.
        ///// </summary>
        ///// <param name="key">The key.</param>
        ///// <param name="siteURL">The site URL.</param>
        ///// <returns></returns>
        //private string FindKey(string key, string siteURL)
        //{
        //    DataTable dtListValues = null;
        //    string strOutput = string.Empty;
        //    try
        //    {
        //        ServiceProvider objFactory = new ServiceProvider();
        //        objMossController = objFactory.GetServiceManager("MossService");

        //        //check wheather cache is null, if null populate the cache
        //        //if (ConfigItems.Count == 0)
        //        //{
        //        dtListValues = ((MOSSServiceManager)objMossController).ReadList(siteURL, CONFIGLISTNAME, string.Empty);
        //        PopulateConfigItems(dtListValues);
        //        //}
        //        //if cache is not null get the value from cache
        //        strOutput = GetValue(key);
        //    }

        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if (dtListValues != null)
        //            dtListValues.Dispose();
        //    }
        //    return strOutput;
        //} 
        #endregion

        /// <summary>
        /// Finds the key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="siteURL">The site URL.</param>
        /// <returns></returns>
        private string FindKey(string key, string siteURL)
        {
            string strOutput = string.Empty;
            ServiceProvider objFactory = new ServiceProvider();
            objMossController = objFactory.GetServiceManager("MossService");
            string strQuery = string.Empty;
            strQuery = "<Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">" + key + "</Value></Eq></Where>";
            DataTable dtConfigItems = ((MOSSServiceManager)objMossController).ReadList(siteURL, CONFIGLISTNAME, strQuery);
            if ((dtConfigItems != null) && (dtConfigItems.Rows.Count > 0))
            {
                strOutput = dtConfigItems.Rows[0]["Value"].ToString();//(string)((PortalListItemSiteMapNode)siteMapNodeColConfigItems[0])["Value"];
            }
            if (strOutput == null)
                strOutput = string.Empty;
            return strOutput;
        }
        /// <summary>
        /// Gets the key from sharepoint list or cache
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string GetKey(string key)
        {
            try
            {
                string siteURL = string.Empty;
                siteURL = HttpContext.Current.Request.Url.ToString();
                //returns the key.
                return FindKey(key, siteURL);
            }
            catch (Exception)
            {
                throw;
            }

        }
        /// <summary>
        /// Sets the key.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetKey(string key, string value)
        {
            ServiceProvider objFactory = new ServiceProvider();
            objMossController = objFactory.GetServiceManager("MossService");
            ((MOSSServiceManager)objMossController).SaveDateFormat(key, value, ConfigItems);
        }
        /// <summary>
        /// Gets the key from sharepoint list or cache
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string GetKey(string key, string siteURL)
        {
            try
            {
                //returns the key.
                return FindKey(key, siteURL);
            }
            catch (Exception)
            {
                throw;
            }

        }


        #region For WebService

     
        /// <summary>
        /// Finds the key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="siteURL">The site URL.</param>
        /// <returns></returns>
        public string FindWebServiceKey(string key, string siteURL,bool isWebServiceCall)
        {
            DataTable dtListValues = null;
            string strOutput = string.Empty;
            try
            {
                if (isWebServiceCall)
                {
                    ServiceProvider objFactory = new ServiceProvider();
                    objMossController = objFactory.GetServiceManager("MossService");
                    string strCamlQuery = string.Format("<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>{0}</Value></Eq></Where>",key);
                    dtListValues = ((MOSSServiceManager)objMossController).ReadList(siteURL, CONFIGLISTNAME, strCamlQuery);
                    if (dtListValues != null && dtListValues.Rows.Count > 0)
                    {
                        strOutput = Convert.ToString(dtListValues.Rows[0]["Value"]);
                    }                    
                }
            }

            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dtListValues != null)
                    dtListValues.Dispose();
            }
            return strOutput;
        }
         
       
        #endregion
        #endregion
    }


}

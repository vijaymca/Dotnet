#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: TooltipManager.cs
#endregion
using System;
using Shell.SharePoint.DREAM.Controller;
using System.Web;
using System.Data;
using Microsoft.SharePoint;
namespace Shell.SharePoint.DREAM.Utilities
{
    /// <summary>
    /// This is the class for Tooltip manager.
    /// </summary>
    public class TooltipManager
    {
        #region Declaration
        private static TooltipManager objInstance;
        string strCurrSiteUrl = HttpContext.Current.Request.Url.ToString();
        private const string CONFIGLISTNAME = "Tooltip Configuration";
        private const string TITLE = "Title";
        private const string VALUE = "Value";
        private const string NEW = "New";
        private const string UPDATE = "Update";
        private const string DELETE = "Delete";
        private static DataTable dtTooltipConfigItems = new DataTable();
        #endregion
        #region Consructor
        /// <summary>
        /// Private construction - to prevent creating instance.
        /// </summary>
        private TooltipManager()
        {
        }
        #endregion
        #region Property
        /// <summary>
        /// Gets or sets the tooltip config items.
        /// </summary>
        /// <value>The tooltip config items.</value>
        DataTable TooltipConfigItems
        {
            get
            {
                return dtTooltipConfigItems;
            }
            set
            {
                dtTooltipConfigItems = value;
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Populates the tooltipconfig items from sharepoint list to data
        /// static data table
        /// </summary>
        /// <param name="listems">The listems.</param>
        private DataTable PopulateTooltipConfigItems()
        {
            AbstractController objMossController = null;
            TooltipConfigItems = new DataTable();
            try
            {
                ServiceProvider objFactory = new ServiceProvider();
                objMossController = objFactory.GetServiceManager("MossService");

                //check wheather cache is null, if null populate the cache
                if (TooltipConfigItems.Rows.Count == 0)
                {
                    TooltipConfigItems = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, CONFIGLISTNAME, string.Empty);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return TooltipConfigItems;       
        }
        /// <summary>
        /// Get the tootips from datatable
        /// </summary>
        /// <returns></returns>
        public DataTable GetTooltip()
        {
            DataTable dtFilterConfigItems = null;
            try
            {
                if (TooltipConfigItems.Rows.Count == 0)
                {
                    dtFilterConfigItems = new DataTable();
                    dtFilterConfigItems = PopulateTooltipConfigItems();
                    TooltipConfigItems = dtFilterConfigItems;
                }
            }
            catch (Exception) { throw; }
            finally { if(dtFilterConfigItems != null) dtFilterConfigItems.Dispose(); }
            return TooltipConfigItems;
        }
        /// <summary>
        /// Filter datatable basd on columns
        /// </summary>
        /// <param name="FilterConfigItems"></param>
        /// <returns></returns>
        public DataTable FilterDataTable(DataTable FilterConfigItems, string filterColumn)
        {
            string strFiltercolumn = string.Empty;
            DataTable dtFilterForSearchType = null;
            DataView dvTooltipFilter = null; 
            if (FilterConfigItems.Rows.Count > 0)
            {
                dtFilterForSearchType = new DataTable();                
                strFiltercolumn = filterColumn +"=1";
                dvTooltipFilter = new DataView(FilterConfigItems);
                dvTooltipFilter.RowFilter = strFiltercolumn;
                dtFilterForSearchType = dvTooltipFilter.ToTable();
            }
            return dtFilterForSearchType;
        }
        /// <summary>
        /// Refresh the cache when its changes(Add/Modify/Delete)
        /// Used in Event handler..new cases will be added in the future.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <param name="mode">The mode.</param>
        public void RefreshTooltipConfigItems(string mode)
        {
            switch (mode)
            {
                case "Update":
                    //Update mode       
                    if (TooltipConfigItems.Rows.Count != 0)//For first time when cusotm cache is not populated
                    {
                        TooltipConfigItems.Clear();
                        PopulateTooltipConfigItems();
                    }
                    else
                    {
                        PopulateTooltipConfigItems();
                    }
                    break;
            }
        }
        /// <summary>
        /// Provide the instance of class 
        /// </summary>
        /// <returns></returns>
        public static TooltipManager GetInstance()
        {
            // Uses "Lazy initialization"
            if (objInstance == null)
                objInstance = new TooltipManager();
            return objInstance;
        }
        #endregion
    }
}

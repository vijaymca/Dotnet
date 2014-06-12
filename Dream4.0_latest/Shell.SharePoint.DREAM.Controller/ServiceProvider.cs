#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: ServiceProvider.cs
#endregion

namespace Shell.SharePoint.DREAM.Controller
{
    /// <summary>
    /// Service Provider class for UI.
    /// </summary>
    public class ServiceProvider
    {
        #region Declaration

        #region ServiceManager Constants
        const string REPORT_SERVICE = "REPORT SERVICE";
        const string RESOURCEMANAGER = "RESOURCEMANAGER";
        const string MOSSSERVICE = "MOSSSERVICE";
        const string REPORTSERVICE = "REPORTSERVICE";
        const string QUERYSERVICE = "QUERYSERVICE";
        const string EVENTSERVICE = "EVENTSERVICE";
          #endregion

        AbstractController objServiceManager; 
        #endregion

        /// <summary>
        /// Gets the service manager.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns></returns>
        public AbstractController GetServiceManager(string serviceName)
        {
            //Initializing Factory objects.
            switch (serviceName.ToString().ToUpper())
            {
                case REPORT_SERVICE: objServiceManager = new ReportServiceManager();
                    break;
                case REPORTSERVICE: objServiceManager = new ReportServiceManager();
                    break;
                case RESOURCEMANAGER: objServiceManager = new ResourceServiceManager();
                    break;
                case MOSSSERVICE: objServiceManager = new MOSSServiceManager();
                    break;
                case QUERYSERVICE: objServiceManager = new QueryBuilderManager();
                    break;
                case EVENTSERVICE: objServiceManager = new EventServiceManager();
                    break;
                default: objServiceManager = new MOSSServiceManager();
                    break;
            }
            return objServiceManager;
        }
        /// <summary>
        /// Gets the service manager.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns></returns>
        public AbstractController GetServiceManager(string serviceName, string siteURL)
        {
            //Initializing Factory objects.
            switch (serviceName.ToString().ToUpper())
            {
                case REPORT_SERVICE: objServiceManager = new ReportServiceManager(siteURL);
                    break;
                case REPORTSERVICE: objServiceManager = new ReportServiceManager(siteURL);
                    break;                
                case EVENTSERVICE: objServiceManager = new EventServiceManager(siteURL);
                    break;
                default: objServiceManager = new ReportServiceManager(siteURL);
                    break;
            }
            return objServiceManager;
        }
    }
}
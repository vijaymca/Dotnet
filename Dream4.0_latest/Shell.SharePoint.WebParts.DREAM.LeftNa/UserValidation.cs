#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: UserValidation.cs
#endregion

using System;
using Shell.SharePoint.DREAM.Controller;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;

using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.WebParts.DREAM.LeftNavigation
{
    /// <summary>
    /// This is used to check the Authorization of the user across the site.
    /// This class is initiated in left nav.
    /// </summary>
    public class UserValidation
    {
        #region Declaration
        const string VALIDUSERSESSION = "ValidUser";
        const string YES = "Y";
        const string NO = "N";
        #endregion
        /// <summary>
        /// This function is used to alidates the user.
        /// </summary>
        /// <returns>
        /// true  : if the logged in user is a valid user;
        /// false : if the logged in user is not a valid user;
        /// </returns>
        public static bool ValidateUser()
        {
            bool blnIsValidUser = true;
            string strUserID = string.Empty;
            string strParentSiteURL = string.Empty;
            AbstractController objController = null;
            ServiceProvider objFactory = null;
            try
            {
                if (HttpContext.Current.Session[VALIDUSERSESSION] != null)
                {
                    if (HttpContext.Current.Session[VALIDUSERSESSION].ToString() == YES)
                    {
                        blnIsValidUser = true;
                    }
                    else if (HttpContext.Current.Session[VALIDUSERSESSION].ToString() == NO)
                    {
                        blnIsValidUser = false;
                    }
                }
                else
                {
                    objFactory = new ServiceProvider();
                    objController = objFactory.GetServiceManager("MOSSService");
                    strUserID = HttpContext.Current.User.Identity.Name.ToString();
                    strParentSiteURL = SPContext.Current.Site.Url.ToString();
                    //if the logged in user is a local system Admin, then no need to check in AD.
                    if (!((MOSSServiceManager)objController).IsAdmin(strParentSiteURL, strUserID))
                    {
                        if (!((MOSSServiceManager)objController).IsValidUser(strParentSiteURL, strUserID))
                        {
                            HttpContext.Current.Session[VALIDUSERSESSION] = NO;
                            blnIsValidUser = false;
                        }
                        else
                        {
                            HttpContext.Current.Session[VALIDUSERSESSION] = YES;
                            blnIsValidUser = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return blnIsValidUser;
        }

        #region Dream 4.0 code
        /// <summary>
        /// Determines whether [is site under maintenance].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is site under maintenance]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSiteUnderMaintenance()
        {
            bool blnIsSiteUnderMaintenance = true;
            string strParentSiteURL = string.Empty;
            string strUserID = string.Empty;
            AbstractController objController = null;
            ServiceProvider objFactory = null;
            string strIsSiteUnderMaintenace = string.Empty;

            try
            {

                objFactory = new ServiceProvider();
                objController = objFactory.GetServiceManager("MOSSService");
                strUserID = HttpContext.Current.User.Identity.Name.ToString();
                strParentSiteURL = SPContext.Current.Site.Url.ToString();
                //if the logged in user is a local system Admin, then no need to check in AD.
                if (!((MOSSServiceManager)objController).IsAdmin(strParentSiteURL, strUserID))
                {
                    strIsSiteUnderMaintenace = PortalConfiguration.GetInstance().GetKey("SiteMaintenance");//.ToString().ToLowerInvariant();

                    if (!string.IsNullOrEmpty(strIsSiteUnderMaintenace))
                    {
                        if (strIsSiteUnderMaintenace.ToLowerInvariant().Equals("no"))
                        {
                            blnIsSiteUnderMaintenance = false;
                        }
                        else if (strIsSiteUnderMaintenace.ToLowerInvariant().Equals("yes"))
                        {
                            blnIsSiteUnderMaintenance = true;
                        }
                    }
                }
                else
                {
                    blnIsSiteUnderMaintenance = false;
                }

            }
            catch (Exception)
            {
                throw;
            }
            return blnIsSiteUnderMaintenance;
        } 
        #endregion
    }
}

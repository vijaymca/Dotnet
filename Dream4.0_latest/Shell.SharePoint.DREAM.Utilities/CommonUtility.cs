#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: CommonUtility.cs
#endregion

/// <summary> 
/// This is the general Utilities class used for the Portal
/// </summary>
using System;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Administration;
using Shell.SharePoint.SAEF.Diagnostics;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Business.Entities;


namespace Shell.SharePoint.DREAM.Utilities
{

    /// <summary>
    /// This is the general Utilities class used for the Portal
    /// </summary>
    public class CommonUtility
    {
        #region Declaration
        const string SENDERSEMAILID = "Senders E-mail Id";
        const string EXCEPTIONPAGE = "/pages/Exception.aspx";
        const string MAXRECORDSXPATH = "/response/report";
        const string MAXRECORDSATTRIB = "recordsExceeded";
        const string CONFIGLISTNAME = "Admin E-mail Configurations";
        const string EXCEPTIONLIST = "Exception Messages";
        AbstractController objMossController;
        ActiveDirectoryService objADS;
        ServiceProvider objFactory = new ServiceProvider();
        const string EMAILTEMPLATELIST = "E-Mail Templates";
        const string USERACCESSREQUESTLIST = "User Access Request";
        const string TEAMREGISTRATIONLIST = "Team Registration";
        const string DREAMADMINPERMISSION = "DreamAdmin";
        const string TEAMOWNERPERMISSION = "TeamOwner";
        const string STAFFPERMISSION = "StaffUser";
        const string NONREGUSERPERMISSION = "NonRegUser";
        //Dream 4.0
        //start
        const string MOSSSERVICE = "MossService";
        const string EVENTTARGET = "__EVENTTARGET";
        const string STAROPERATOR = "*";
        const string AMPERSAND = "%";
        const string EQUALSOPERATOR = "EQUALS";
        const string INOPERATOR = "IN";
        const string LIKEOPERATOR = "LIKE";
        const string ANDOPERATOR = "AND";

        //end
        #endregion
        #region Methods
        /// <summary>
        /// Gets the parent site URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public String GetParentSiteUrl(String url)
        {
            try
            {
                String strUrlWithoutHttp = url.Substring(7);
                String strParentUrlWithoutHttp = strUrlWithoutHttp.Substring(0, strUrlWithoutHttp.IndexOf('/'));
                String strParentUrl = "http://" + strParentUrlWithoutHttp;
                return strParentUrl;
            }
            catch(Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the email body.
        /// </summary>
        /// <param name="camlQuery">The caml query.</param>
        /// <returns></returns>
        private void SendAccessRequestEmail(string camlQuery, string message, string userMailID)
        {
            StringBuilder strMessageBody = new StringBuilder();
            StringBuilder strAdminMailIDs = new StringBuilder();
            string[] arrEmailID = null;
            string strParentSiteURL = string.Empty;
            string strSubject = string.Empty;
            string strSignature = string.Empty;
            StringDictionary objHeaders = null;
            DataTable objDtListValue = null;
            DataRow objListRow;

            try
            {
                strParentSiteURL = HttpContext.Current.Request.Url.ToString();
                objDtListValue = new DataTable();
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                objHeaders = new StringDictionary();
                arrEmailID = new string[GetAdminEmailID().Split(';').Length];

                arrEmailID = GetAdminEmailID().Split(';');
                if(arrEmailID.Length > 0)
                {
                    for(int intArrIndex = 0; intArrIndex < arrEmailID.Length - 1; intArrIndex++)
                    {
                        strAdminMailIDs.Append(arrEmailID[intArrIndex] + ";");
                    }
                }
                else if(!string.IsNullOrEmpty(arrEmailID[0]))
                {
                    strAdminMailIDs.Append(arrEmailID[0]);
                }

                // Get the Email Body
                objDtListValue = ((MOSSServiceManager)objMossController).ReadList(strParentSiteURL, EMAILTEMPLATELIST, camlQuery);
                if(objDtListValue.Rows.Count > 0)
                {
                    objListRow = objDtListValue.Rows[0];
                    strSubject = objListRow["Subject"].ToString();
                    strMessageBody.Append(objListRow["BodyLine1"].ToString());
                    strMessageBody.Append("<BR><BR>" + objListRow["BodyLine2"].ToString());
                    strMessageBody = strMessageBody.Replace("{DREAM portal}", "<a href='" + SPContext.Current.Site.Url + "' >DREAM Portal</a>");
                    strSignature = objListRow["Signature"].ToString();
                    strSignature = strSignature.Replace("{Region}", SPContext.Current.Web.Title);
                }
                strMessageBody.Append(message);
                strMessageBody.Append("<BR><BR>" + strSignature);

                //build String Dictionary Object

                objHeaders.Add("from", GetSenderEmailId());
                objHeaders.Add("to", strAdminMailIDs.ToString());
                objHeaders.Add("cc", userMailID);
                objHeaders.Add("subject", strSubject);
                ((MOSSServiceManager)objMossController).SendFeedBackMail(objHeaders, strMessageBody.ToString());
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if(objDtListValue != null)
                    objDtListValue.Dispose();
            }
        }

        /// <summary>
        /// Gets the sender email id.
        /// </summary>
        /// <returns></returns>
        private string GetSenderEmailId()
        {
            string strSenderEmailId = PortalConfiguration.GetInstance().GetKey(SENDERSEMAILID);
            if(string.IsNullOrEmpty(strSenderEmailId))
            {
                strSenderEmailId = SPContext.Current.Site.WebApplication.OutboundMailSenderAddress;
            }
            return strSenderEmailId;
        }



        /// <summary>
        /// Sends the team access request email.
        /// </summary>
        /// <param name="userMailID">The user mail ID.</param>
        /// <param name="message">The message.</param>
        /// <param name="teamName">Name of the team.</param>
        /// <param name="teamOwner">The team owner.</param>
        public void SendTeamAccessRequestEmail(string userMailID, string message, string teamName, string teamOwner)
        {
            StringBuilder strMessageBody = new StringBuilder();
            StringBuilder strAdminMailIDs = new StringBuilder();


            string strSubject = string.Empty;
            string strSignature = string.Empty;
            StringDictionary objHeaders = null;
            DataTable objDtListValue = null;
            DataRow objListRow;
            DataTable dtTeam = null;
            string strTeamOwnerEmailId = string.Empty;
            string[] strTeamOwners;
            string strCamlQuery = "<FieldRef Name=\"Title\"/><Where><Eq><FieldRef Name=\"Category\" /><Value Type=\"Choice\">"
                           + "Team Access Request</Value></Eq></Where>";
            try
            {


                objDtListValue = new DataTable();
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                objHeaders = new StringDictionary();

                /// Multi Team Owner Implementation
                /// Changed By: Yasotha
                /// Date : 20-Jan-2010

                if(teamOwner.LastIndexOf(";") != -1)
                {
                    strTeamOwners = teamOwner.Split(";".ToCharArray());
                }
                else
                {
                    strTeamOwners = new string[1];
                    strTeamOwners[0] = teamOwner;
                }
                for(int intOwnerIndex = 0; intOwnerIndex < strTeamOwners.Length; intOwnerIndex++)
                {
                    dtTeam = ((MOSSServiceManager)objMossController).ReadList(HttpContext.Current.Request.Url.ToString(), USERACCESSREQUESTLIST, "<Where><Eq><FieldRef Name=\"DisplayName\"/><Value Type=\"Text\">" + strTeamOwners[intOwnerIndex] + "</Value></Eq></Where>");

                    if(dtTeam != null && dtTeam.Rows.Count > 0)
                        strTeamOwnerEmailId = dtTeam.Rows[0]["Email"].ToString();

                    // Get the Email Body
                    objDtListValue = ((MOSSServiceManager)objMossController).ReadList(HttpContext.Current.Request.Url.ToString(), EMAILTEMPLATELIST, strCamlQuery);
                    if(objDtListValue.Rows.Count > 0)
                    {
                        objListRow = objDtListValue.Rows[0];
                        strSubject = objListRow["Subject"].ToString();
                        objListRow["BodyLine1"] = objListRow["BodyLine1"].ToString().Replace("{TeamOwner}", strTeamOwners[intOwnerIndex]);
                        objListRow["BodyLine1"] = objListRow["BodyLine1"].ToString().Replace("{TeamName}", teamName);

                        strMessageBody.Append(objListRow["BodyLine1"].ToString());
                        strMessageBody.Append("<BR><BR>" + objListRow["BodyLine2"].ToString());
                        strMessageBody = strMessageBody.Replace("{DREAM portal}", "<a href='" + SPContext.Current.Site.Url + "' >DREAM Portal</a>");
                        strSignature = objListRow["Signature"].ToString();
                        strSignature = strSignature.Replace("{Region}", SPContext.Current.Web.Title);
                    }
                    strMessageBody.Append(message);
                    strMessageBody.Append("<BR><BR>" + strSignature);

                    //build String Dictionary Object
                    objHeaders.Add("from", GetSenderEmailId());
                    objHeaders.Add("to", strTeamOwnerEmailId);
                    objHeaders.Add("cc", userMailID);
                    objHeaders.Add("subject", strSubject);
                    ((MOSSServiceManager)objMossController).SendFeedBackMail(objHeaders, strMessageBody.ToString());
                }
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if(objDtListValue != null)
                    objDtListValue.Dispose();
                if(dtTeam != null)
                    dtTeam.Dispose();
            }
        }


        /// <summary>
        /// Sends the email for team access status.
        /// </summary>
        /// <param name="toEmailID">To email ID.</param>
        /// <param name="status">The status.</param>
        /// <param name="teamId">The team id.</param>
        public void SendEmailForTeamAccessStatus(string userID, string status, string teamId)
        {
            DataTable objDtListValue = null;
            objADS = new ActiveDirectoryService();

            string strSubject = string.Empty;
            string strBodyLine1 = string.Empty;
            string strBodyLine2 = string.Empty;
            string strSignature = string.Empty;
            string strCamlQuery = string.Empty;

            string strTeamOwner = string.Empty;
            string strTeamName = string.Empty;
            string strUserEmail = string.Empty;
            string strTeamOwnerEmail = string.Empty;


            /// Multi Team Owner Implementation
            /// Changed By: Yasotha
            /// Date : 21-Jan-2010
            string[] strTeamOwners;
            objMossController = objFactory.GetServiceManager(MOSSSERVICE);


            objDtListValue = ((MOSSServiceManager)objMossController).ReadList(HttpContext.Current.Request.Url.ToString(), TEAMREGISTRATIONLIST, "<Where><Eq><FieldRef Name='ID'/><Value Type=\"Counter\">" + teamId + "</Value></Eq></Where>");

            if(objDtListValue != null)
            {
                if(objDtListValue.Rows.Count > 0)
                {
                    strTeamOwner = objDtListValue.Rows[0]["TeamOwner"].ToString();
                    strTeamName = objDtListValue.Rows[0]["Title"].ToString();
                }
            }


            objDtListValue = ((MOSSServiceManager)objMossController).ReadList(HttpContext.Current.Request.Url.ToString(), USERACCESSREQUESTLIST, "<Where><Eq><FieldRef Name='ID'/><Value Type=\"Counter\">" + userID + "</Value></Eq></Where>");

            if(objDtListValue != null)
            {
                if(objDtListValue.Rows.Count > 0)
                {
                    strUserEmail = objDtListValue.Rows[0]["Email"].ToString();
                }
            }


            /// Multi Team Owner Implementation
            /// Changed By: Yasotha
            /// Date : 21-Jan-2010
            /// strTeamOwner - possible values are teamowner1;teamowner2 or teamowner
            /// If comes with multiple team owner, get email id of both team owner and send emails to both.
            if(strTeamOwner.IndexOf(";") != -1)
            {
                strTeamOwners = strTeamOwner.Split(";".ToCharArray());
            }
            else
            {
                strTeamOwners = new string[1];
                strTeamOwners[0] = strTeamOwner;
            }
            foreach(string teamOwner in strTeamOwners)
            {
                objDtListValue = ((MOSSServiceManager)objMossController).ReadList(HttpContext.Current.Request.Url.ToString(), USERACCESSREQUESTLIST, "<Where><Eq><FieldRef Name='DisplayName'/><Value Type=\"Text\">" + teamOwner + "</Value></Eq></Where>");

                if(objDtListValue != null)
                {
                    if(objDtListValue.Rows.Count > 0)
                    {
                        strTeamOwnerEmail = objDtListValue.Rows[0]["Email"].ToString();
                    }
                }





                StringBuilder sbMessageBody = new StringBuilder();
                if(status == "Rejected")
                {
                    strCamlQuery = "<OrderBy><FieldRef Name=\"Title\"/><FieldRef Name=\"Subject\" />" + "<FieldRef                                Name=\"BodyLine1\" /><FieldRef Name=\"BodyLine2\" />"
                                          + "<FieldRef Name=\"Signature\" /></OrderBy><Where><Eq>"
                                          + "<FieldRef Name=\"Category\" /><Value Type=\"Choice\">"
                                          + "Team Access Rejected</Value></Eq></Where>";
                }
                else
                {
                    strCamlQuery = "<OrderBy><FieldRef Name=\"Title\"/><FieldRef Name=\"Subject\" />" + "<FieldRef                                Name=\"BodyLine1\" /><FieldRef Name=\"BodyLine2\" />"
                                          + "<FieldRef Name=\"Signature\" /></OrderBy><Where><Eq>"
                                          + "<FieldRef Name=\"Category\" /><Value Type=\"Choice\">"
                                          + "Team Access Approval</Value></Eq></Where>";
                }

                try
                {
                    // Get the DocumentLibrary Deals
                    objDtListValue = ((MOSSServiceManager)objMossController).ReadList(HttpContext.Current.Request.Url.ToString(), EMAILTEMPLATELIST, strCamlQuery);

                    if(objDtListValue != null)
                    {
                        if(objDtListValue.Rows.Count > 0)
                        {
                            strSubject = objDtListValue.Rows[0]["Subject"].ToString();
                            strBodyLine1 = objDtListValue.Rows[0]["BodyLine1"].ToString();
                            strBodyLine2 = objDtListValue.Rows[0]["BodyLine2"].ToString();
                            strSignature = objDtListValue.Rows[0]["Signature"].ToString();
                            strBodyLine2 = strBodyLine2.Replace("{TeamOwner}", teamOwner);
                            strBodyLine2 = strBodyLine2.Replace("{TeamName}", strTeamName);
                            strSignature = strSignature.Replace("{RegardsFrom}", objADS.GetDisplayName(GetUserName()));
                            strSignature = strSignature.Replace("{Region}", SPContext.Current.Web.Title);
                            sbMessageBody.Append(strBodyLine1);
                            sbMessageBody.Append("<BR>" + strBodyLine2);
                            sbMessageBody.Append("<BR>" + strSignature);
                        }
                    }
                    SendTeamAccessApprovalMail(strUserEmail, strTeamOwnerEmail, strSubject, sbMessageBody.ToString());
                }

                catch(Exception ex)
                {
                    HandleException("DREAM - Access Request Event Handler", ex);
                }
                finally
                {
                    if(objDtListValue != null)
                        objDtListValue.Dispose();
                }
            }
        }



        /// <summary>
        /// Sends the team access approval mail.
        /// </summary>
        /// <param name="toEmailID">To email ID.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="messageBody">The message body.</param>
        public void SendTeamAccessApprovalMail(string toEmailID, string teamOwnerEmail, String subject, String messageBody)
        {
            StringDictionary objHeaders = null;
            objADS = new ActiveDirectoryService();
            try
            {
                objHeaders = new StringDictionary();
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                //build String Dictionary Object
                objHeaders.Add("from", GetSenderEmailId());
                objHeaders.Add("to", toEmailID);
                objHeaders.Add("cc", teamOwnerEmail);
                objHeaders.Add("subject", subject);
                ((MOSSServiceManager)objMossController).SendFeedBackMail(objHeaders, messageBody);
            }
            catch(Exception ex)
            {
                HandleException("Common Utility", ex);
            }
        }


        /// <summary>
        /// Send New Feedback alert mail to administrator.
        /// </summary>
        /// <param name="userMailID">MailID of user.</param>
        /// <param name="message">message.</param>
        /// <returns></returns>
        public void SendAlertMailforNewFeedback(string message)
        {
            StringBuilder strMessageBody = new StringBuilder();
            StringBuilder strAdminMailIDs = new StringBuilder();
            string[] arrEmailID = null;
            string strParentSiteURL = string.Empty;
            string strSubject = string.Empty;
            string strSignature = string.Empty;
            string camlQuery = string.Empty;
            StringDictionary objHeaders = null;
            DataTable objDtListValue = null;
            DataRow objListRow;

            try
            {
                strParentSiteURL = HttpContext.Current.Request.Url.ToString();
                objDtListValue = new DataTable();
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                objHeaders = new StringDictionary();
                arrEmailID = new string[GetAdminEmailID().Split(';').Length];

                arrEmailID = GetAdminEmailID().Split(';');
                if(arrEmailID.Length > 0)
                {
                    for(int intArrIndex = 0; intArrIndex < arrEmailID.Length - 1; intArrIndex++)
                    {
                        strAdminMailIDs.Append(arrEmailID[intArrIndex] + ";");
                    }
                }
                else if(!string.IsNullOrEmpty(arrEmailID[0]))
                {
                    strAdminMailIDs.Append(arrEmailID[0]);
                }

                // Get the Email Body
                camlQuery = "<FieldRef Name=\"Title\"/><Where><Eq><FieldRef Name=\"Category\" /><Value Type=\"Choice\">"
                               + "New Feedback</Value></Eq></Where>";
                objDtListValue = ((MOSSServiceManager)objMossController).ReadList(strParentSiteURL, EMAILTEMPLATELIST, camlQuery);
                if(objDtListValue.Rows.Count > 0)
                {
                    objListRow = objDtListValue.Rows[0];
                    strSubject = objListRow["Subject"].ToString();
                    strMessageBody.Append(objListRow["BodyLine1"].ToString());
                    strMessageBody.Append("<BR><BR>" + objListRow["BodyLine2"].ToString());
                    strMessageBody = strMessageBody.Replace("{DREAM portal}", "<a href='" + SPContext.Current.Site.Url + "' >DREAM Portal</a>");
                    strSignature = objListRow["Signature"].ToString();
                    strSignature = strSignature.Replace("{Region}", SPContext.Current.Web.Title);
                }
                strMessageBody.Append(message);
                strMessageBody.Append("<BR><BR>" + strSignature);

                //build String Dictionary Object
                objHeaders.Add("from", GetSenderEmailId());
                objHeaders.Add("to", strAdminMailIDs.ToString());
                objHeaders.Add("subject", strSubject);
                ((MOSSServiceManager)objMossController).SendFeedBackMail(objHeaders, strMessageBody.ToString());
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if(objDtListValue != null)
                    objDtListValue.Dispose();
            }
        }




        /// <summary>
        /// Sends the mailfor user access request.
        /// </summary>
        /// <param name="mailID">The mail ID.</param>
        /// <param name="message">The message.</param>
        public void SendMailforUserAccessRequest(String mailID, string message)
        {
            string strCamlQuery = string.Empty;
            try
            {
                strCamlQuery = "<FieldRef Name=\"Title\"/><Where><Eq><FieldRef Name=\"Category\" /><Value Type=\"Choice\">"
                            + "Access Request</Value></Eq></Where>";

                SendAccessRequestEmail(strCamlQuery, message, mailID);
            }
            catch(Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Gets the current pagename.
        /// </summary>
        /// <param name="query">if set to <c>true</c> [query].</param>
        /// <returns></returns>
        public String GetCurrentPageName(bool query)
        {
            string strFullString = string.Empty;
            string strCurrentPath = System.Web.HttpContext.Current.Request.Url.PathAndQuery;
            string[] arrTempPageName = new string[3];
            string[] arrPageName = new string[3];
            StringBuilder strResultString = new StringBuilder();
            arrTempPageName = strCurrentPath.Split('/');
            strFullString = arrTempPageName[arrTempPageName.Length - 1].ToString();
            try
            {
                if(query)
                {
                    if(strFullString.IndexOf('&') > 0)
                    {
                        arrPageName = strFullString.Split('&');
                        for(int intIndex = 0; intIndex < arrPageName.Length; intIndex++)
                        {
                            if(arrPageName[intIndex].ToLower().IndexOf("pagenumber") >= 0)
                                break;
                            strResultString.Append(arrPageName[intIndex] + "&");
                        }
                    }
                    else
                    {
                        strResultString.Append(strFullString + "&");
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
            return strResultString.ToString();
        }

        /// <summary>
        /// Gets the current pagename.
        /// </summary>
        /// <returns></returns>
        public String GetCurrentPageName()
        {
            try
            {
                string strCurrentPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
                string[] arrPageName = new string[3];
                arrPageName = strCurrentPath.Split('/');
                return arrPageName[arrPageName.Length - 1].ToString();
            }
            catch(Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the display name of the admin.
        /// </summary>
        /// <returns></returns>
        public string GetAdminDisplayName()
        {
            DataTable objDtListValue = null;
            DataRow objListRow;
            string strAdminDisplayName = string.Empty;
            string strParentSiteURL = string.Empty;
            try
            {
                strParentSiteURL = HttpContext.Current.Request.Url.ToString();
                objDtListValue = new DataTable();
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                objDtListValue = ((MOSSServiceManager)objMossController).ReadList(strParentSiteURL, CONFIGLISTNAME, string.Empty);
                if(objDtListValue.Rows.Count > 0)
                {
                    for(int intIndex = 0; intIndex < objDtListValue.Rows.Count; intIndex++)
                    {
                        objListRow = objDtListValue.Rows[intIndex];
                        strAdminDisplayName = objListRow["Admin_x0020_Display_x0020_Name"].ToString();
                        if(!string.IsNullOrEmpty(strAdminDisplayName))
                            break;
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if(objDtListValue != null)
                    objDtListValue.Dispose();
            }
            return strAdminDisplayName;
        }

        /// <summary>
        /// Gets the Source Email ID of the admin.
        /// </summary>
        /// <returns></returns>
        public string GetSourceAdminEmailID()
        {
            DataTable objDtListValue = null;
            DataRow objListRow;
            string strAdminEmailID = string.Empty;
            string strCAMLQuery = string.Empty;
            string strParentSiteURL = string.Empty;
            try
            {
                strParentSiteURL = HttpContext.Current.Request.Url.ToString();
                objDtListValue = new DataTable();
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                strCAMLQuery = "<OrderBy><FieldRef Name=\"Title\" /></OrderBy><Where><Eq><FieldRef Name=\"IsUsedToSendMail\" /><Value Type=\"Choice\">Yes</Value></Eq></Where>";
                objDtListValue = ((MOSSServiceManager)objMossController).ReadList(strParentSiteURL, CONFIGLISTNAME, strCAMLQuery);
                if(objDtListValue.Rows.Count > 0)
                {
                    //Loop through the values in ConfigList.
                    for(int intIndex = 0; intIndex < objDtListValue.Rows.Count; intIndex++)
                    {
                        objListRow = objDtListValue.Rows[intIndex];
                        strAdminEmailID = objListRow["Title"].ToString();
                        if(!string.IsNullOrEmpty(strAdminEmailID))
                            break;
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if(objDtListValue != null)
                    objDtListValue.Dispose();
            }
            return strAdminEmailID;
        }

        /// <summary>
        /// Gets the Email ID of the admin.
        /// </summary>
        /// <returns></returns>
        public string GetAdminEmailID()
        {
            DataTable objDtListValue = null;
            DataRow objListRow;
            StringBuilder strAdminEmailID = new StringBuilder();
            string strParentSiteURL = string.Empty;
            try
            {
                strParentSiteURL = HttpContext.Current.Request.Url.ToString();
                objDtListValue = new DataTable();
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                objDtListValue = ((MOSSServiceManager)objMossController).ReadList(strParentSiteURL, CONFIGLISTNAME, string.Empty);
                if(objDtListValue.Rows.Count > 0)
                {
                    //Loop through the values in the list.
                    for(int intIndex = 0; intIndex < objDtListValue.Rows.Count; intIndex++)
                    {
                        objListRow = objDtListValue.Rows[intIndex];
                        strAdminEmailID.Append(objListRow["Title"].ToString() + ";");
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if(objDtListValue != null)
                    objDtListValue.Dispose();
            }
            return strAdminEmailID.ToString();
        }
        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="errorLocation">The error location.</param>
        /// <param name="exception">The exception.</param>
        public static void HandleException(string errorLocation, Exception exception)
        {
            StringBuilder strExceptionMessage = new StringBuilder();
            strExceptionMessage.AppendLine("This exception has been occurred in the following location >> " + errorLocation);
            strExceptionMessage.AppendLine("Exception:" + exception.Message.ToString() + "<BR>" + exception.StackTrace.ToString());
            Log.RegisterTraceProvider();
            Log.CategoryType categoryType = Log.CategoryType.Shell_Configurations;
            EventSeverity eventSeverity = EventSeverity.Error;
            TraceSeverity traceSeverity = TraceSeverity.Monitorable;

            Log.LogMessage(strExceptionMessage.ToString(), errorLocation, categoryType, 10, traceSeverity, eventSeverity, Guid.Empty);
            Log.UnregisterTraceProvider();
            if(string.Compare(errorLocation, "DREAM Scheduler") != 0)
            {

                //**Needs to be uncommented 
                //HttpContext.Current.Response.Redirect(EXCEPTIONPAGE, false);
                //**Needs to be commented 
                HttpContext.Current.Response.Write(exception.Message + "<BR/>" + exception.StackTrace);
            }
        }
        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="errorLocation">The error location.</param>
        /// <param name="exception">The exception.</param>
        public static void HandleException(string errorLocation, Exception exception, int errorCode)
        {
            CommonUtility objCommonUtility = new CommonUtility();
            StringBuilder strExceptionMessage = new StringBuilder();
            strExceptionMessage.AppendLine("This exception has been occurred in the following location >> " + errorLocation);
            strExceptionMessage.AppendLine("Exception:" + exception.Message.ToString() + "<BR>" + exception.StackTrace.ToString());
            //If User does not belong to Active Directory.
            if(errorCode == 5)
            {
                Log.RegisterTraceProvider();
                Log.CategoryType categoryType = Log.CategoryType.Shell_Configurations;
                EventSeverity eventSeverity = EventSeverity.ErrorCritical;
                TraceSeverity traceSeverity = TraceSeverity.High;
                Log.LogMessage(exception.Message.ToString(), errorLocation, categoryType, 10, traceSeverity, eventSeverity, Guid.Empty);
                Log.UnregisterTraceProvider();
            }
            else
            {
                objCommonUtility.SetExceptionSeverity(exception.Message.ToString(), errorLocation);
            }
        }

        /// <summary>
        /// Sets the exception severity.
        /// </summary>
        /// <param name="p">The p.</param>
        private void SetExceptionSeverity(string exceptionMessage, string errorLocation)
        {
            string strExceptionSeverity = string.Empty;
            strExceptionSeverity = GetExceptionMessages(exceptionMessage);
            Log.RegisterTraceProvider();
            Log.CategoryType categoryType = Log.CategoryType.Shell_Configurations;
            EventSeverity eventSeverity;
            TraceSeverity traceSeverity;
            switch(strExceptionSeverity)
            {
                case "Error":
                    eventSeverity = EventSeverity.Error;
                    traceSeverity = TraceSeverity.Medium;
                    break;
                case "Critical":
                    eventSeverity = EventSeverity.ErrorCritical;
                    traceSeverity = TraceSeverity.High;
                    break;
                case "Warning":
                    eventSeverity = EventSeverity.Warning;
                    traceSeverity = TraceSeverity.Monitorable;
                    break;
                case "Information":
                    eventSeverity = EventSeverity.Information;
                    traceSeverity = TraceSeverity.None;
                    break;
                case "Service Unavailable":
                    eventSeverity = EventSeverity.ErrorServiceUnavailable;
                    traceSeverity = TraceSeverity.High;
                    break;
                case "Success":
                    eventSeverity = EventSeverity.Success;
                    traceSeverity = TraceSeverity.None;
                    break;
                default:
                    eventSeverity = EventSeverity.Error;
                    traceSeverity = TraceSeverity.Medium;
                    break;
            }
            Log.LogMessage(exceptionMessage, errorLocation, categoryType, 10, traceSeverity, eventSeverity, Guid.Empty);
            Log.UnregisterTraceProvider();
        }
        /// <summary>
        /// Gets the Email ID of the admin.
        /// </summary>
        /// <returns></returns>
        private string GetExceptionMessages(string exceptionMessage)
        {
            DataTable objDtListValue = null;
            DataRow objListRow;
            StringBuilder strExceptionSeverity = new StringBuilder();

            string strCamlQuery = string.Empty;
            try
            {

                objDtListValue = new DataTable();
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                strCamlQuery = "<Query><Where><Eq><FieldRef Name='Title' /><Value Type='Text'>" + exceptionMessage + "</Value></Eq></Where></Query>";
                objDtListValue = ((MOSSServiceManager)objMossController).ReadList(HttpContext.Current.Request.Url.ToString(), EXCEPTIONLIST, strCamlQuery);
                if(objDtListValue.Rows.Count > 0)
                {
                    //Loop through the values in the list.
                    for(int intIndex = 0; intIndex < objDtListValue.Rows.Count; intIndex++)
                    {
                        objListRow = objDtListValue.Rows[intIndex];
                        if(string.Equals(objListRow["Title"].ToString(), exceptionMessage.ToString()))
                        {
                            strExceptionSeverity.Append(objListRow["Severity_x0020_Level"].ToString());
                            break;
                        }
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if(objDtListValue != null)
                    objDtListValue.Dispose();
            }
            return strExceptionSeverity.ToString();
        }
        #endregion
        #region Session Methods
        /// <summary>
        /// Gets the session variable.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static object GetSessionVariable(Page pageReference, string sessionVariableName)
        {
            object objectValue = null;
            if(pageReference.Session != null && pageReference.Session[sessionVariableName] != null)
            {
                objectValue = pageReference.Session[sessionVariableName];
            }
            return objectValue;
        }

        /// <summary>
        /// Sets the session variable.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public static void SetSessionVariable(Page pageReference, string sessionVariableName, object sessionObjectValue)
        {
            if(pageReference.Session != null)
            {
                pageReference.Session.Add(sessionVariableName, sessionObjectValue);
            }
        }
        /// <summary>
        /// Sets the session variable.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public static void RemoveSessionVariable(Page pageReference, string sessionVariableName)
        {
            if(pageReference.Session != null)
            {
                pageReference.Session.Remove(sessionVariableName);
            }
        }
        #endregion
        #region UserName Methods
        /// <summary>
        /// Gets the name alone for the logged in user.
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            try
            {
                string strLoggedinUserName = string.Empty;
                strLoggedinUserName = HttpContext.Current.User.Identity.Name.ToString();
                string[] arrUserName = new string[3];
                strLoggedinUserName = strLoggedinUserName.Replace("\\", "|");
                arrUserName = strLoggedinUserName.Split('|');
                return arrUserName[arrUserName.Length - 1];
            }
            catch(Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the domain name of the current logged in user 
        /// </summary>
        /// <returns></returns>
        public string GetUserDomain()
        {
            try
            {
                string strLoggedinUserName = string.Empty;
                strLoggedinUserName = HttpContext.Current.User.Identity.Name.ToString();
                string[] arrUserName = new string[3];
                strLoggedinUserName = strLoggedinUserName.Replace("\\", "|");
                arrUserName = strLoggedinUserName.Split('|');
                return arrUserName[0];
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Gets the user info.
        /// </summary>
        /// <param name="detail">The detail.</param>
        /// <returns></returns>
        public string GetUserTeamID()
        {
            DataTable dtUserTeam = null;
            try
            {
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                dtUserTeam = ((MOSSServiceManager)objMossController).ReadList(HttpContext.Current.Request.Url.ToString(), USERACCESSREQUESTLIST, "<Where><Eq><FieldRef Name='Title'/><Value Type=\"Text\">" + GetUserName() + "</Value></Eq></Where>", "<FieldRef Name=\"TeamID\"/>");

                if((dtUserTeam != null) && (dtUserTeam.Rows.Count > 0))
                {
                    return dtUserTeam.Rows[0]["TeamID"].ToString();
                }
                else
                    return "0";
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if(dtUserTeam != null)
                {
                    dtUserTeam.Dispose();
                }
            }
        }

        public string GetCurrentUserProjectName()
        {
            DataTable dtTeamRegistration = null;
            try
            {
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                string strQuery = "<Where><Eq><FieldRef Name=\"ID\" /><Value Type=\"Counter\">" + GetUserTeamID() + "</Value></Eq></Where>";
                string strViewField = "<FieldRef Name=\"ProjectTitle\"/>";
                dtTeamRegistration = ((MOSSServiceManager)objMossController).ReadList(HttpContext.Current.Request.Url.ToString(), TEAMREGISTRATIONLIST, strQuery, strViewField);

                if((dtTeamRegistration != null) && (dtTeamRegistration.Rows.Count > 0))
                {
                    return (string)dtTeamRegistration.Rows[0]["ProjectTitle"];
                }
                else
                    return string.Empty;
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if(dtTeamRegistration != null)
                {
                    dtTeamRegistration.Dispose();
                }
            }
        }
        /// <summary>
        /// Gets the name of the save search user.
        /// </summary>
        /// <returns></returns>
        public string GetSaveSearchUserName()
        {
            try
            {
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                string strLoggedinUserName = string.Empty;
                string strUserName = string.Empty;

                //this will set the current logged in user name.
                strLoggedinUserName = HttpContext.Current.User.Identity.Name.ToString();

                if(((MOSSServiceManager)objMossController).IsAdmin(HttpContext.Current.Request.Url.ToString(), strLoggedinUserName))
                {
                    strUserName = "Administrator";
                    return strUserName;
                }
                else
                {
                    string[] arrUserName = new string[3];
                    strLoggedinUserName = strLoggedinUserName.Replace("\\", "|");
                    arrUserName = strLoggedinUserName.Split('|');
                    return arrUserName[arrUserName.Length - 1];
                }
            }
            catch(Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the name of the save search user.
        /// </summary>
        /// <returns></returns>
        public string GetReorderUserName()
        {
            objMossController = objFactory.GetServiceManager(MOSSSERVICE);
            string strLoggedinUserName = string.Empty;
            string strUserName = string.Empty;

            //this will set the current logged in user name.
            strLoggedinUserName = HttpContext.Current.User.Identity.Name;
            string[] arrUserName = new string[3];
            strLoggedinUserName = strLoggedinUserName.Replace("\\", "|");
            arrUserName = strLoggedinUserName.Split('|');
            return arrUserName[arrUserName.Length - 1];
        }
        /// <summary>
        /// Gets the team permission.
        /// </summary>
        /// <param name="siteUrl">The site URL.</param>
        /// <returns></returns>
        public string GetTeamPermission(string siteUrl)
        {

            string strPermission = string.Empty;
            objMossController = objFactory.GetServiceManager(MOSSSERVICE);

            if(((MOSSServiceManager)objMossController).IsCurrentUserTeamOwner(siteUrl, GetUserName()))
            {
                strPermission = TEAMOWNERPERMISSION;
            }
            else if(!string.Equals(GetUserTeamID(), "0"))
            {
                strPermission = STAFFPERMISSION;
            }
            else
            {
                strPermission = NONREGUSERPERMISSION;
            }
            return strPermission;
        }
        /// <summary>
        /// Gets the site URL.
        /// </summary>
        /// <returns></returns>
        public string GetSiteURL()
        {
            return HttpContext.Current.Request.Url.ToString();
        }

        #region Dream 4.0
        /// <summary>
        /// Gets the user role.
        /// </summary>
        /// <returns></returns>
        public string GetUserRole()
        {
            DataTable dtUserAccessList = null;
            try
            {
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                dtUserAccessList = ((MOSSServiceManager)objMossController).ReadList(HttpContext.Current.Request.Url.ToString(), USERACCESSREQUESTLIST, "<Where><Eq><FieldRef Name='Title'/><Value Type=\"Text\">" + GetUserName() + "</Value></Eq></Where>", "<FieldRef Name=\"Role\"/>");

                if((dtUserAccessList != null) && (dtUserAccessList.Rows.Count > 0) && (dtUserAccessList.Rows[0]["Role"] != null) && (!string.IsNullOrEmpty((string)dtUserAccessList.Rows[0]["Role"])))
                {
                    return (string)dtUserAccessList.Rows[0]["Role"];
                }
                else
                    return "NormalUser";//If user does not belong to any role,Then his role is a normal user,He can be admin also
            }
            finally
            {
                if(dtUserAccessList != null)
                {
                    dtUserAccessList.Dispose();
                }
            }

        }
        #endregion

        #endregion


        #region FindControl Methods
        /// <summary>
        /// Determines whether [is max record exceeds] [the specified result document].
        /// </summary>
        /// <param name="resultDocument">The result document.</param>
        /// <returns>
        /// 	<c>true</c> if [is max record exceeds] [the specified result document]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMaxRecordExceeds(XmlDocument resultDocument, bool isClicked)
        {
            bool blnIsMaxRecordExist = false;
            string strValue = string.Empty;
            try
            {
                if(!isClicked)
                {
                    if(resultDocument != null)
                    {
                        XmlNodeList xmlNodeList = resultDocument.SelectNodes(MAXRECORDSXPATH);
                        //Loops through the nodes in XmlDocument.
                        foreach(XmlNode xmlnodeValue in xmlNodeList)
                        {
                            if(xmlnodeValue.Attributes.GetNamedItem(MAXRECORDSATTRIB) != null)
                            {
                                strValue = xmlnodeValue.Attributes.GetNamedItem(MAXRECORDSATTRIB).Value.ToString();
                                blnIsMaxRecordExist = Convert.ToBoolean(strValue);
                            }
                        }
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
            return blnIsMaxRecordExist;
        }
        /// <summary>
        /// Finds the control recursive.
        /// </summary>
        /// <param name="Root">The root control.</param>
        /// <param name="clientId">The client id.</param>
        /// <returns>Control</returns>
        public static Control FindControlRecursive(Control rootControl, string clientId)
        {
            if(string.Equals(rootControl.ClientID, clientId))
                return rootControl;
            #region "looping the controls"
            foreach(Control objControl in rootControl.Controls)
            {
                Control ctlFound = FindControlRecursive(objControl, clientId);
                if(ctlFound != null)
                    return ctlFound;
            }
            #endregion
            return null;
        }
        /// <summary>
        /// Finds the control.
        /// </summary>
        /// <param name="controlName">Name of the control.</param>
        /// <param name="currentPage">The current page.</param>
        /// <returns>Control</returns>
        public static Control FindControl(string controlName, Page currentPage)
        {
            #region "Declaring local variables"
            Control ctlBuddy;
            string strWebPartControl;
            #endregion
            try
            {
                #region "setting buddy control"
                if(currentPage == null || controlName == null)
                    return null;
                ctlBuddy = currentPage.FindControl(controlName);
                if(ctlBuddy == null)
                {
                    strWebPartControl = GetControlUniqueID(controlName, currentPage.Controls);
                    if(strWebPartControl != null)
                        ctlBuddy = currentPage.FindControl(strWebPartControl);
                    else
                        ctlBuddy = currentPage.FindControl(controlName);
                }
                #endregion
            }
            catch
            {
                throw;
            }
            return ctlBuddy;
        }
        /// <summary>
        /// Gets the control unique ID.
        /// </summary>
        /// <param name="controlID">The control ID.</param>
        /// <param name="controls">The controls.</param>
        /// <returns>Control Unique ID</returns>
        public static string GetControlUniqueID(string controlID, ControlCollection controls)
        {
            #region "Declaring Local Variables"
            Control control;
            string strUniqueID = null;
            #endregion
            try
            {
                #region "looping the controls and getting the controls unique ID"
                //Loop through the controls in control collection
                for(int intIndex = 0; intIndex < controls.Count; ++intIndex)
                {
                    control = controls[intIndex];
                    if(string.Equals(control.ID, controlID))
                    {
                        strUniqueID = control.UniqueID;
                        break;
                    }
                    if(control.Controls.Count > 0)
                    {
                        strUniqueID = GetControlUniqueID(controlID, control.Controls);
                        if(strUniqueID.Length > 0)
                            break;
                    }
                }
                #endregion
            }
            catch
            {
                throw;
            }
            return strUniqueID;
        }
        /// <summary>
        /// Cleans the session variable.
        /// </summary>
        /// <param name="isRequired">if set to <c>true</c> [is required].</param>
        /// <param name="currentPage">The current page.</param>
        public static void CleanSessionVariable(bool isRequired, Page currentPage)
        {
            try
            {
                if(isRequired && currentPage != null)
                {
                    SetSessionVariable(currentPage, enumSessionVariable.searchType.ToString(), null);
                    SetSessionVariable(currentPage, enumSessionVariable.geometry.ToString(), null);
                    SetSessionVariable(currentPage, enumSessionVariable.whereClause.ToString(), null);
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Gets the paging sorting params.
        /// </summary>
        /// <param name="paramsList">The params list.</param>
        /// <returns></returns>
        public Hashtable GetPagingSortingParams(string paramsList)
        {
            Hashtable hstblParams = null;
            if((!string.IsNullOrEmpty(paramsList)) && (paramsList.Contains("&")))
            {
                hstblParams = new Hashtable();
                string[] arrParams = paramsList.Split("&".ToCharArray());
                foreach(string strParam in arrParams)
                {
                    if(strParam.Contains("="))
                    {
                        string[] arrKeyValue = strParam.Split("=".ToCharArray());
                        hstblParams.Add(arrKeyValue[0].Trim(), arrKeyValue[1].Trim());
                    }
                }
            }
            return hstblParams;
        }
        /// <summary>
        /// Renders the busy box.
        /// </summary>
        public void RenderBusyBox(Page page)
        {
            ScriptManager objScriptManager = ScriptManager.GetCurrent(page);
            if(!objScriptManager.IsInAsyncPostBack)
            {
                page.Response.Write("<script language=\"javascript\" src=\"/_Layouts/DREAM/Javascript/CastleBusyBoxRel2_1.js\"></script>");
                page.Response.Write("<iframe id=\"BusyBoxIFrame\" style=\"border:3px double #D2D2D2\" name=\"BusyBoxIFrame\" frameBorder=\"0\" scrolling=\"no\" ondrop=\"return false;\" src=\"/_layouts/dream/Busybox.htm\"></iframe>");
                page.Response.Write("<script>");
                page.Response.Write("var busyBox = new BusyBox(\"BusyBoxIFrame\", \"busyBox\", 1, \"processing\", \".gif\", 125, 147, 207,\"/_layouts/dream/InitialBusybox.htm\");");
                page.Response.Write("busyBox.Show();");
                page.Response.Write("</script>");
            }
        }
        /// <summary>
        /// Renders the busy box.
        /// </summary>
        public void RenderAjaxBusyBox(Page page)
        {
            ScriptManager objScriptManager = ScriptManager.GetCurrent(page);
            if(!objScriptManager.IsInAsyncPostBack)
            {
                page.Response.Write("<script language=\"javascript\" src=\"/_Layouts/DREAM/Javascript/CastleBusyBoxRel2_1.js\"></script>");
                page.Response.Write("<iframe id=\"BusyBoxIFrame\" style=\"border:3px double #D2D2D2\" name=\"BusyBoxIFrame\" frameBorder=\"0\" scrolling=\"no\" ondrop=\"return false;\" src=\"/_layouts/dream/BasinSyncBusyBox.htm\"></iframe>");
                page.Response.Write("<script>");
                page.Response.Write("var busyBox = new BusyBox(\"BusyBoxIFrame\", \"busyBox\", 1, \"processing\", \".gif\", 125, 147, 207,\"/_layouts/dream/BasinSyncBusyBox.htm\");");
                page.Response.Write("busyBox.Show();");
                page.Response.Write("</script>");
            }
        }
        /// <summary>
        /// Registers the on load client script.
        /// </summary>
        /// <param name="script">The script.</param>
        public void RegisterOnLoadClientScript(Page page, string script)
        {
            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language=\"javascript\">try{");
            strScript.Append(script);
            strScript.Append("}catch(Ex){}</script>");
            ScriptManager.RegisterStartupScript(page, page.GetType(), "OnLoadClientScript", strScript.ToString(), false);
        }
        /// <summary>
        /// Closes the busy box.
        /// </summary>
        public void CloseBusyBox(Page page)
        {
            string strScript = "<script language=\"javascript\">try{busyBox.Hide();ChangeBusyBoxSource();}catch(Ex){}</script>";
            page.ClientScript.RegisterStartupScript(page.GetType(), "BusyBoxCloseScript", strScript);
        }
        /// <summary>
        /// Closes the busy box.
        /// </summary>
        public void CloseAjaxBusyBox(Page page)
        {
            string strScript = "<script language=\"javascript\">try{busyBox.Hide();}catch(Ex){}</script>";
            page.ClientScript.RegisterStartupScript(page.GetType(), "BusyBoxCloseScript", strScript);
        }
        /// <summary>
        /// Gets the post back control.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns>System.Web.UI.Control</returns>
        public Control GetPostBackControl(Page page)
        {
            Control objControl = null;
            string strCtrlname = page.Request.Params[EVENTTARGET];
            if(!string.IsNullOrEmpty(strCtrlname))
            {
                objControl = page.FindControl(strCtrlname);
            }
            // if __EVENTTARGET is null, the control is a button type and we need to 
            // iterate over the form collection to find it
            else
            {
                string strCtrl = String.Empty;
                Control objCtrl = null;
                foreach(string strCtl in page.Request.Form)
                {
                    // handle ImageButton controls ...
                    if(strCtl.EndsWith(".x") || strCtl.EndsWith(".y"))
                    {
                        strCtrl = strCtl.Substring(0, strCtl.Length - 2);
                        objCtrl = page.FindControl(strCtrl);
                    }
                    else
                    {
                        objCtrl = page.FindControl(strCtl);
                    }
                    if(objCtrl is System.Web.UI.WebControls.Button ||
                             objCtrl is System.Web.UI.WebControls.ImageButton)
                    {
                        objControl = objCtrl;
                        break;
                    }
                }
            }
            return objControl;
        }
        /// <summary>
        /// Determines whether [is post back] [the specified page].
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns>
        /// 	<c>true</c> if [is post back] [the specified page]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsPostBack(Page page)
        {
            string strCtrlname = page.Request.Params[EVENTTARGET];
            bool blnIsPostBack = false;
            if((!string.IsNullOrEmpty(strCtrlname)) || (GetPostBackControl(page) != null))
            {
                blnIsPostBack = true;
            }
            return blnIsPostBack;
        }
        #endregion

        #region Request XML logger created in DREAM 4.0
        /// <summary>
        /// Requests the XML logger.
        /// </summary>
        /// <param name="requestXml">The request XML.</param>
        /// <param name="docLibName">Name of the doc lib.</param>
        public void XmlLogger(XmlDocument requestXml, string xmlType, string docLibName)
        {
            #region Varaible Declaration
            const string ENABLEREQUESTXMLLOGVALUE = "yes";
            const string XMLTYPEREQUEST = "request";
            string strPortalKey = "EnableRequestXmlLog";
            string strRootElementName = "requestinfos";
            string strChildNodeName = "requestinfo";
            string DATEFORMAT = "dd-MM-yyyy";
            string strLogFileName = string.Empty;
            string strRequestXmlLogEnabled = string.Empty;
            bool blnDocLibExist = false;
            bool blnDocLibFileExist = false;
            XmlDocument xmlDocLog = null;
            #endregion
            if(xmlType.ToLowerInvariant().Equals(XMLTYPEREQUEST))
            {
                strPortalKey = "EnableRequestXmlLog";
                strRootElementName = "requestinfos";
                strChildNodeName = "requestinfo";
            }
            else
            {
                strPortalKey = "EnableResponseXmlLog";
                strRootElementName = "responses";
                strChildNodeName = "response";
            }
            strRequestXmlLogEnabled = PortalConfiguration.GetInstance().GetKey(strPortalKey);
            if((requestXml != null) && (!string.IsNullOrEmpty(strRequestXmlLogEnabled)) && (strRequestXmlLogEnabled.ToLower().Equals(ENABLEREQUESTXMLLOGVALUE)))
            {
                objMossController = objFactory.GetServiceManager(MOSSSERVICE);
                blnDocLibExist = ((MOSSServiceManager)objMossController).IsDocLibExist(docLibName, GetSiteURL());
                if(blnDocLibExist)
                {
                    strLogFileName = DateTime.Today.ToString(DATEFORMAT) + "_" + xmlType;
                    blnDocLibFileExist = ((MOSSServiceManager)objMossController).IsDocLibFileExist(docLibName, strLogFileName);
                    if(blnDocLibFileExist)
                    {
                        xmlDocLog = ((MOSSServiceManager)objMossController).GetDocLibXMLFile(docLibName, strLogFileName);
                    }
                    else
                    {
                        xmlDocLog = new XmlDocument();
                        XmlElement xmlElementRequestInfos = xmlDocLog.CreateElement(strRootElementName);
                        xmlDocLog.AppendChild(xmlElementRequestInfos);
                    }
                    XmlElement xmlElementRequestInfo = xmlDocLog.CreateElement(strChildNodeName);
                    xmlElementRequestInfo.InnerXml = requestXml.DocumentElement.InnerXml;
                    xmlDocLog.DocumentElement.AppendChild(xmlElementRequestInfo);
                    ((MOSSServiceManager)objMossController).UploadToDocumentLib(docLibName, strLogFileName, xmlDocLog);
                }
            }
        }
        #endregion

        #region DREAM 3.1 eWB2 methods
        /// <summary>
        /// Sends the mailfor print update.
        /// </summary>
        /// <param name="strToMailID">The STR to mail ID.</param>
        /// <param name="message">The message.</param>
        /// <param name="parentSiteURL">The parent site URL.</param>
        /// <param name="address">The address.</param>
        /// <param name="contextForMail">The context for mail.</param>
        public void SendMailforPrintUpdate(string strToMailID, string message, string parentSiteURL, string address, SPContext contextForMail)
        {
            try
            {
                StringBuilder strMessageBody = new StringBuilder();
                StringBuilder strAdminMailIDs = new StringBuilder();

                string strSubject = string.Empty;
                string strSignature = string.Empty;
                StringDictionary objHeaders = null;
                DataTable objDtListValue = null;
                DataRow objListRow;

                objDtListValue = new DataTable();
                objMossController = objFactory.GetServiceManager("MossService");
                objHeaders = new StringDictionary();
                string strCamlQuery = "<FieldRef Name=\"Title\"/><Where><Eq><FieldRef Name=\"Category\" /><Value Type=\"Choice\">"
                                + "eWB2 Printing</Value></Eq></Where>";

                objDtListValue = ((MOSSServiceManager)objMossController).ReadList(parentSiteURL, EMAILTEMPLATELIST, strCamlQuery);
                if(objDtListValue.Rows.Count > 0)
                {
                    objListRow = objDtListValue.Rows[0];
                    strSubject = objListRow["Subject"].ToString();
                    strMessageBody.Append(objListRow["BodyLine1"].ToString());
                    strMessageBody.Append("<BR><BR>" + objListRow["BodyLine2"].ToString());
                    strMessageBody = strMessageBody.Replace("{DREAM portal}", "<a href='" + parentSiteURL + "' >DREAM Portal</a>");
                    strSignature = objListRow["Signature"].ToString();
                    strSignature = strSignature.Replace("{Region}", "EPE");
                }
                strMessageBody.Append("<a href='" + message + "'>Click here</a> to access the PDF file.<BR>");
                strMessageBody.Append("<BR><BR>" + strSignature);

                //build String Dictionary Object
                objHeaders.Add("from", GetSenderEmailId());
                objHeaders.Add("to", strToMailID.ToString());
                objHeaders.Add("subject", strSubject);
                SendAlertMail(objHeaders, strMessageBody.ToString(), contextForMail);
            }
            catch(Exception Ex)
            {
                throw Ex;
            }
        }

        /// <summary>
        /// Sends the alert mail.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="messageBody">The message body.</param>
        /// <param name="contextForMail">The context for mail.</param>
        private void SendAlertMail(StringDictionary header, string messageBody, SPContext contextForMail)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    SPUtility.SendEmail(contextForMail.Site.OpenWeb(), header, messageBody.ToString());
                });
            }
            catch(Exception Ex)
            {
                throw Ex;
            }
        }

        #endregion
        #region DREAM 4.0
        /// <summary>
        /// Gets the pipe seperated STR as array.
        /// </summary>
        /// <param name="pipeSeperatedValue">The pipe seperated value.</param>
        /// <returns></returns>
        public string[] GetPipeSeperatedStrAsArray(string pipeSeperatedValue)
        {
            const string STRINGPATTERN = @"\r\n";
            string[] arrValues = new string[] { };
            Regex regExMyAssetPattern = new Regex(STRINGPATTERN);
            string strTrimmedValues = string.Empty;
            if(!string.IsNullOrEmpty(pipeSeperatedValue))
            {
                strTrimmedValues = regExMyAssetPattern.Replace(pipeSeperatedValue, string.Empty);
                arrValues = strTrimmedValues.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }
            return arrValues;
        }
        /// <summary>
        /// Populates the asset list control.
        /// </summary>
        /// <param name="listControl">The list control.</param>
        /// <param name="arrText">The arr text.</param>
        /// <param name="arrValue">The arr value.</param>
        public void PopulateAssetListControl(ListControl listControl, string[] arrText, string[] arrValue)
        {
            if(listControl != null && arrText != null)
            {
                SortedList sortedAssetNames = new SortedList();
                for(int intIndex = 0; intIndex < arrText.Length; intIndex++)
                {
                    sortedAssetNames.Add(arrValue[intIndex], arrText[intIndex]);
                }
                sortedAssetNames.TrimToSize();
                listControl.DataSource = sortedAssetNames;
                listControl.DataValueField = "key";
                listControl.DataTextField = "value";
                listControl.DataBind();
            }
        }
        /// <summary>
        /// Gets the form control value.
        /// </summary>
        /// <param name="controlId">The control id.</param>
        /// <returns></returns>
        public string GetFormControlValue(string controlId)
        {
            #region Decaleration
            string strControlId = string.Empty;
            string strValue = string.Empty;
            string strFirstPart = string.Empty;
            string strSecondPart = string.Empty;
            string strIds = string.Empty;
            string[] arrControlIds = null;
            #endregion
            arrControlIds = HttpContext.Current.Request.Form.AllKeys;
            if(arrControlIds.Length > 0)
            {
                strIds = string.Join("|", arrControlIds);
                if(arrControlIds.Length == 1)
                {
                    strControlId = strIds;
                }
                else if(strIds.IndexOf(controlId) != -1)
                {
                    strFirstPart = strIds.Substring(0, strIds.IndexOf(controlId));
                    strSecondPart = strIds.Substring(strIds.IndexOf(controlId));
                    strControlId = strFirstPart.Substring(strFirstPart.LastIndexOf("|") + 1) + (strSecondPart.Contains("|") ? strSecondPart.Substring(0, strSecondPart.IndexOf("|")) : strSecondPart);
                }
                if(HttpContext.Current.Request.Form[strControlId] != null)
                {
                    strValue = HttpContext.Current.Request.Form[strControlId];
                }
            }
            return strValue;
        }
        /// <summary>
        /// Adds the attribute.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="operation">The operation.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public Attributes AddAttribute(string name, string operation, string[] values)
        {
            Attributes objAttribute = new Attributes();

            objAttribute.Value = new ArrayList();

            Value objValue = null;

            objAttribute.Name = name;

            objAttribute.Operator = operation;
            foreach(string strValue in values)
            {
                if(!string.IsNullOrEmpty(strValue.Trim()))
                {
                    objValue = new Value();
                    objValue.InnerText = strValue.Trim();
                    objAttribute.Value.Add(objValue);
                }
            }
            return objAttribute;
        }

        /// <summary>
        /// Gets the operator.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public string GetOperator(string[] values)
        {
            string strOperator = string.Empty;
            if(values.Length > 1)
            {
                strOperator = INOPERATOR;
            }
            else
            {
                foreach(string strValue in values)
                {
                    if((strValue.Contains(STAROPERATOR)) || (strValue.Contains(AMPERSAND)))
                        strOperator = LIKEOPERATOR;
                    else
                        strOperator = EQUALSOPERATOR;
                }
            }
            return strOperator;
        }
        #region FixingFormAction
        /// <summary>
        /// Ensures the panel fix.
        /// </summary>
        /// <param name="type">The type.</param>
        public void EnsurePanelFix(Page page,Type type)
        {
            if(page.Form != null)
            {
                string formOnSubmitAtt = page.Form.Attributes["onsubmit"];
                if(formOnSubmitAtt == "return _spFormOnSubmitWrapper();")
                {
                    page.Form.Attributes["onsubmit"] = "_spFormOnSubmitWrapper();";
                }
            }
            ScriptManager.RegisterStartupScript(page, type, "UpdatePanelFixup", "_spOriginalFormAction = document.forms[0].action; _spSuppressFormOnSubmitWrapper=true;", true);
        }

        #endregion FixingFormAction
        #endregion
    }


    #region ENUM Session Variables
    /// <summary>
    /// Session Variable Collection.
    /// </summary>
    public enum enumSessionVariable
    {
        UserPreferences,
        selectedIdentifierForEPCatalog,
        picksFilterValues,
        QSCriteriaValue,
        ContextMenu,
        /// <summary>
        /// Used for Map Identifier column Index.
        /// </summary>
        ColumnIndex,
        /// <summary>
        /// Whether to display Context Search For Map or not.
        /// </summary>
        IsDisplayContextSearch,
        /// <summary>
        /// Map layer name
        /// </summary>
        AssetType,
        // Get all records
        /// <summary>
        /// Whether to fetch all records or not
        /// </summary>
        IsFetchAll,
        /// <summary>
        /// sets the maxRecord count.
        /// </summary>
        maxRecordsCount,
        /// <summary>
        /// sets the search type.
        /// </summary>
        searchType,
        /// <summary>
        /// map geometry
        /// </summary>
        geometry,
        /// <summary>
        /// the where clause used for map search.
        /// </summary>
        whereClause,
        /// <summary>
        /// the map asset value.
        /// </summary>
        dropDownBoxValue,
        /// <summary>
        /// To hold the exact error message.
        /// </summary>
        ErrorMessage,
        /// <summary>
        /// To hold the length measured.
        /// </summary>
        MeasureLength,
        /// <summary>
        /// To hold the units from the preference.
        /// </summary>
        MeasureUnits,
        /// <summary>
        ///cantains all selected rows unique ids for map webpart
        /// </summary>
        ZoomListIds,
        /// <summary>
        /// User rights
        /// </summary>
        UserPrivileges,
        // Dream 4.0 code start
        /// <summary>
        /// Well Events Filter Option
        /// </summary>
        WellEventsFilterOption,
        /// <summary>
        /// Change dEvents
        /// </summary>
        ChangedEvents,
        //Dream 4.0 code end
    }
    #endregion


}

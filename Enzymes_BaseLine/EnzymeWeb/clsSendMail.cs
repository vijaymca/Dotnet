#region " Imports "

using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Web;
//using eImportBAL;
//using eImportProp;
using System.Data.SqlTypes;
using System.Reflection;

#endregion

namespace CommonBAL
{
    ////////////////////////////////////////////////////////////////////////////////////
    //
    //	File Description	: This file is used to Send Emails to Reciptents
    //                        
    // ---------------------------------------------------------------------------------
    //	Date Created		: JUN 21, 2012
    //	Author			    : SATISH KUMAR K
    // ---------------------------------------------------------------------------------
    // 	Change History
    //	Date Modified		: 
    //	Changed By		    :
    //	Change Description  : 
    //
    ////////////////////////////////////////////////////////////////////////////////////

    public class clsSendMail
    {
        #region " Methods "

        /// <summary>
        /// Send Mail to reciptents by passing dataset as parameter
        /// </summary>
        /// <param name="dsMailList"></param>
        /// <returns></returns>
        public bool SendMail(DataTable dtMailList)
        {
            DataTable dtMailLog = null;
            DataRow drMailLog = null;
            SendMailProp objSendMail = null;
            SendMailBO objSendMailBO = null;
            string msgNotDelivIDS = string.Empty;
            bool mailSent = false;
            int region_ID = 0;

            try
            {
                dtMailLog = new DataTable();

                //Create datatable structure
                Create_MailLog(ref dtMailLog);

                foreach (DataRow dr in dtMailList.Rows)
                {
                    objSendMail = new SendMailProp();
                    objSendMail.SendTo = Convert.ToString(dr["EmailTo_nm"]);
                    objSendMail.SendCCTo = Convert.ToString(dr["EmailCc_nm"]);

                    if (Convert.ToString(dr["EmailBcc_nm"]) != "")
                        objSendMail.SendBCCTo = Convert.ToString(dr["EmailBcc_nm"]);

                    objSendMail.SendFrom = ConfigurationManager.AppSettings["FromMailID"].ToString();
                    objSendMail.MailSubject = Convert.ToString(dr["EmailSubject_nm"]);
                    objSendMail.MailBody = Convert.ToString(dr["EmailBody_nm"]);

                    if (Convert.ToString(dr["FilePath"]) != "")
                        objSendMail.FilePath = Convert.ToString(dr["FilePath"]);

                    mailSent = SendMail_to_Reciptents(ref objSendMail, ref msgNotDelivIDS);

                    drMailLog = dtMailLog.NewRow();
                    drMailLog.ItemArray = new object[] { 
                        Convert.ToInt32(Convert.ToString(dr["EmailLog_id"])), 
                        msgNotDelivIDS, 
                        System.DateTime.Now,
                        Convert.ToInt32(mailSent)
                    };

                    dtMailLog.Rows.Add(drMailLog);

                    msgNotDelivIDS = string.Empty;
                }
            }
            catch
            {
                mailSent = false;
            }
            finally
            {
                if (dtMailLog != null && dtMailLog.Rows.Count > 0)
                {
                    if (System.Web.HttpContext.Current.Session["RegionID"] != null)
                        region_ID = Convert.ToInt32(System.Web.HttpContext.Current.Session["RegionID"]);

                    objSendMailBO = new SendMailBO();
                    objSendMailBO.Upd_MailStatus(dtMailLog, region_ID);
                }

                objSendMail = null;
                dtMailLog = null;
                drMailLog = null;
                objSendMailBO = null;
            }

            return mailSent;
        }

        /// <summary>
        /// Send Mail to reciptents by passing property object as parameter
        /// </summary>
        /// <param name="objSendMailProp"></param>
        public bool SendMail(SendMailProp objSendMailProp)
        {
            //Declare Variables
            DataTable dtItem = null;
            DataRow drItem = null;

            try
            {
                //Create Instance of datatable
                dtItem = new DataTable();

                //Create datatable structure
                Create_MailList(ref dtItem);

                //Create New row for the table
                drItem = dtItem.NewRow();

                drItem.ItemArray = new object[]
                                        {
                                            objSendMailProp.SendTo,                                            
                                            objSendMailProp.SendCCTo,
                                            objSendMailProp.SendBCCTo,
                                            objSendMailProp.MailSubject,
                                            objSendMailProp.MailBody,
                                            objSendMailProp.FilePath,
                                            0
                                        };

                //Add row to the table
                dtItem.Rows.Add(drItem);

                //Send Mail to reciptents by passing datatable as parameter
                return SendMail(dtItem);
            }
            finally
            {
                //Dispose Objects
                dtItem = null;
                drItem = null;
            }

        }

        /// <summary>
        /// To send mail based on the passed on values
        /// </summary>
        /// <param name="objSendMail"></param>
        /// <param name="msgNotDelivIDS"></param>
        /// <returns></returns>
        private bool SendMail_to_Reciptents(ref SendMailProp objSendMail, ref string msgNotDelivIDS)
        {
            char[] ch = { ',', ';' };
            bool IsMailSend = false;
            MailMessage objMail = new MailMessage();
            MemoryStream dataStream = null;
            int ActualCount = 0;
            int DeliveryCount = 0;
            Attachment attachFile = null;
            SmtpClient emailClient = null;
            clsDataFunctionsBAL objDataFunctionsBAL = null;

            try
            {
                objDataFunctionsBAL = new clsDataFunctionsBAL();

                if ((objSendMail.SendTo != string.Empty))
                {
                    string[] AllToAddresses = objSendMail.SendTo.Trim(ch).Split(ch);

                    foreach (string ToAddress in AllToAddresses)
                    {
                        if (!string.IsNullOrEmpty(ToAddress.Trim()))
                            objMail.To.Add(new MailAddress(ToAddress.Trim()));
                    }
                }

                if ((objSendMail.SendCCTo != string.Empty))
                {
                    string[] AllCCAddresses = objSendMail.SendCCTo.Trim(ch).Split(ch);

                    foreach (string CCAddress in AllCCAddresses)
                    {
                        if (!string.IsNullOrEmpty(CCAddress.Trim()))
                            objMail.CC.Add(new MailAddress(CCAddress.Trim()));
                    }
                }

                if ((objSendMail.SendBCCTo != string.Empty))
                {
                    string[] AllBCCAddresses = objSendMail.SendBCCTo.Trim(ch).Split(ch);

                    foreach (string BCCAddress in AllBCCAddresses)
                    {
                        if (!string.IsNullOrEmpty(BCCAddress.Trim()))
                            objMail.Bcc.Add(new MailAddress(BCCAddress.Trim()));
                    }
                }

                if ((objSendMail.SendFrom != string.Empty))
                {
                    objMail.From = new MailAddress(objSendMail.SendFrom);
                }

                if (objSendMail.MailAttachByteArray != null)
                {
                    dataStream = new MemoryStream(objSendMail.MailAttachByteArray);
                    attachFile = new Attachment(dataStream, objSendMail.MailAttacmentName, objSendMail.MailAttachContentType);
                    objMail.Attachments.Add(attachFile);
                }

                if (objSendMail.FilePath != string.Empty)
                {
                    string[] strAttachements = objSendMail.FilePath.ToString().Split('|');
                    foreach (string attackement in strAttachements)
                    {
                        if (File.Exists(attackement))
                        {
                            attachFile = new Attachment(attackement);
                            objMail.Attachments.Add(attachFile);
                        }
                    }
                }

                if ((objSendMail.MailSubject != string.Empty))
                {
                    objMail.Subject = objSendMail.MailSubject;
                }

                if ((objSendMail.MailBody != string.Empty))
                {
                    objMail.Body = objSendMail.MailBody;
                }

                objMail.Priority = System.Net.Mail.MailPriority.High;
                ActualCount = objMail.To.Count;
                objMail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                if ((ConfigurationManager.AppSettings["SMTP"] != null) && objSendMail.SendTo != string.Empty && objSendMail.SendFrom != string.Empty)
                {
                    objMail.IsBodyHtml = true;

                    #region "SMTP"
                    try
                    {
                        emailClient = new SmtpClient(ConfigurationManager.AppSettings["SMTP"]);
                        emailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        emailClient.EnableSsl = false;

                        emailClient.Send(objMail);
                        IsMailSend = true;
                    }
                    catch (SmtpFailedRecipientsException excc)
                    {
                        foreach (SmtpFailedRecipientException exc in excc.InnerExceptions)
                        {
                            msgNotDelivIDS = msgNotDelivIDS + exc.FailedRecipient + ",";
                        }

                        if (msgNotDelivIDS != string.Empty)
                        {
                            msgNotDelivIDS = msgNotDelivIDS.Substring(0, msgNotDelivIDS.Length - 1);
                            DeliveryCount = msgNotDelivIDS.Split(',').Length;
                        }

                        IsMailSend = CountTest(ActualCount, DeliveryCount);

                        if (IsMailSend)
                            msgNotDelivIDS = "Mail not sent to following email id's " + msgNotDelivIDS + " " + ExceptionStatusMessage(excc.StatusCode);
                        else
                            msgNotDelivIDS = "Mail not sent due to " + ExceptionStatusMessage(excc.StatusCode);

                    }
                    catch (SmtpFailedRecipientException exx)
                    {
                        msgNotDelivIDS = msgNotDelivIDS + exx.FailedRecipient;

                        IsMailSend = CountTest(ActualCount, 1);//Default count is one for smtpfailedreceipient exception

                        if (IsMailSend)
                            msgNotDelivIDS = "Mail not sent to following email id's " + msgNotDelivIDS + " " + ExceptionStatusMessage(exx.StatusCode);
                        else
                            msgNotDelivIDS = "Mail not sent due to " + ExceptionStatusMessage(exx.StatusCode);
                    }
                    catch (Exception ex)
                    {
                        objDataFunctionsBAL.SaveErrorLog(string.Format("{0} - Problem while sending Mail :{1}", "SMTP", ex.Message), this.GetType().BaseType.Name, MethodBase.GetCurrentMethod().Name);

                        msgNotDelivIDS += "Server 1(SMTP):" + ex.Message;

                        #region "SMTP1"
                        try
                        {
                            if (ConfigurationManager.AppSettings["SMTP1"] != null)
                            {
                                emailClient = new SmtpClient(ConfigurationManager.AppSettings["SMTP1"]);
                                emailClient.Send(objMail);
                                IsMailSend = true;
                            }
                            else
                            {
                                IsMailSend = false;
                            }
                        }
                        catch (SmtpFailedRecipientsException excc)
                        {
                            foreach (SmtpFailedRecipientException exc in excc.InnerExceptions)
                            {
                                msgNotDelivIDS = msgNotDelivIDS + exc.FailedRecipient + ",";
                            }

                            if (msgNotDelivIDS != string.Empty)
                            {
                                msgNotDelivIDS = msgNotDelivIDS.Substring(0, msgNotDelivIDS.Length - 1);
                                DeliveryCount = msgNotDelivIDS.Split(',').Length;
                            }

                            IsMailSend = CountTest(ActualCount, DeliveryCount);

                            if (IsMailSend)
                                msgNotDelivIDS = "Mail not sent to following email id's " + msgNotDelivIDS + " " + ExceptionStatusMessage(excc.StatusCode);
                            else
                                msgNotDelivIDS = "Mail not sent due to " + ExceptionStatusMessage(excc.StatusCode);

                        }
                        catch (SmtpFailedRecipientException exx)
                        {
                            msgNotDelivIDS = msgNotDelivIDS + exx.FailedRecipient;
                            IsMailSend = CountTest(ActualCount, 1);//Default count is one for smtpfailedreceipient exception

                            if (IsMailSend)
                                msgNotDelivIDS = "Mail not sent to following email id's " + msgNotDelivIDS + " " + ExceptionStatusMessage(exx.StatusCode);
                            else
                                msgNotDelivIDS = "Mail not sent due to " + ExceptionStatusMessage(exx.StatusCode);
                        }
                        catch (Exception ex1)
                        {
                            objDataFunctionsBAL.SaveErrorLog(string.Format("{0} - Problem while sending Mail :{1}", "SMTP1", ex.Message), this.GetType().BaseType.Name, MethodBase.GetCurrentMethod().Name);

                            msgNotDelivIDS += ", Server 2:" + ex1.Message;

                            #region "SMTP2"

                            try
                            {
                                if (ConfigurationManager.AppSettings["SMTP2"] != null)
                                {
                                    emailClient = new SmtpClient(ConfigurationManager.AppSettings["SMTP2"]);
                                    emailClient.Send(objMail);
                                    IsMailSend = true;
                                }
                                else
                                {
                                    IsMailSend = false;
                                }
                            }
                            catch (SmtpFailedRecipientsException excc)
                            {
                                foreach (SmtpFailedRecipientException exc in excc.InnerExceptions)
                                {
                                    msgNotDelivIDS = msgNotDelivIDS + exc.FailedRecipient + ",";
                                }

                                if (msgNotDelivIDS != string.Empty)
                                {
                                    msgNotDelivIDS = msgNotDelivIDS.Substring(0, msgNotDelivIDS.Length - 1);
                                    DeliveryCount = msgNotDelivIDS.Split(',').Length;
                                }

                                IsMailSend = CountTest(ActualCount, DeliveryCount);

                                if (IsMailSend)
                                    msgNotDelivIDS = "Mail not sent to following email id's " + msgNotDelivIDS + " " + ExceptionStatusMessage(excc.StatusCode);
                                else
                                    msgNotDelivIDS = "Mail not sent due to " + ExceptionStatusMessage(excc.StatusCode);
                            }
                            catch (SmtpFailedRecipientException exx)
                            {
                                msgNotDelivIDS = msgNotDelivIDS + exx.FailedRecipient;
                                IsMailSend = CountTest(ActualCount, 1);//Default count is one for smtpfailedreceipient exception

                                if (IsMailSend)
                                    msgNotDelivIDS = "Mail not sent to following email id's " + msgNotDelivIDS + " " + ExceptionStatusMessage(exx.StatusCode);
                                else
                                    msgNotDelivIDS = "Mail not sent due to " + ExceptionStatusMessage(exx.StatusCode);
                            }
                            catch (Exception ex2)
                            {
                                msgNotDelivIDS += ", Server 3:" + ex2.Message;

                                objDataFunctionsBAL.SaveErrorLog(string.Format("{0} - Problem while sending Mail :{1}", "SMTP2", ex.Message), this.GetType().BaseType.Name, MethodBase.GetCurrentMethod().Name);

                                IsMailSend = false;
                            }
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    IsMailSend = false;
                }
            }
            catch
            {
            }
            finally
            {
                //objErrorLog = null;
                emailClient = null;

                if (objMail != null)
                    objMail.Dispose();

                if (attachFile != null)
                    attachFile.Dispose();

                if (dataStream != null)
                    dataStream.Dispose();

            }

            return IsMailSend;
        }

        /// <summary>
        /// Set Proper User defined message for Status codes
        /// </summary>
        /// <param name="StatusCode"></param>
        /// <param name="StatusMsg"></param>
        private string ExceptionStatusMessage(SmtpStatusCode StatusCode)
        {
            string StatusMsg = string.Empty;

            switch (StatusCode)
            {
                case SmtpStatusCode.ExceededStorageAllocation: StatusMsg = "The message is too large to be stored in the recipient(s) mailbox."; break;
                case SmtpStatusCode.GeneralFailure: StatusMsg = "Mail Server not found (or) not responding"; break;
                case SmtpStatusCode.InsufficientStorage: StatusMsg = "The mail service does not have sufficent storage to complete the request"; break;
                case SmtpStatusCode.LocalErrorInProcessing: StatusMsg = "Mail Server not found (or) not responding"; break;
                case SmtpStatusCode.MailboxBusy: StatusMsg = "Recipient(s) mailbox is busy"; break;
                case SmtpStatusCode.MailboxNameNotAllowed: StatusMsg = "The syntax used to specify the recipient(s) mail Id's is incorrect"; break;
                case SmtpStatusCode.MailboxUnavailable: StatusMsg = "The Recipient(s) mail account was not found or could not be accessed"; break;
                case SmtpStatusCode.ServiceNotAvailable: StatusMsg = "Mail Server not found (or) not responding"; break;
                case SmtpStatusCode.SyntaxError: StatusMsg = "The syntax used to specify the recipient(s) mail Id's is incorrect"; break;
                case SmtpStatusCode.TransactionFailed: StatusMsg = "Transaction failed : Mail Server not found (or) not responding"; break;
                default: StatusMsg = "Transaction failed : Mail Server not found (or) not responding"; break;
            }

            return StatusMsg;
        }

        /// <summary>
        /// Differntiate the actual no. of Mails sent and actual no. of Mails received
        /// </summary>
        /// <param name="ActualCount"></param>
        /// <param name="DeliveryCount"></param>
        /// <returns></returns>
        private bool CountTest(int ActualCount, int DeliveryCount)
        {
            if (ActualCount == DeliveryCount)
            {
                return false;
            }
            else if (ActualCount > DeliveryCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Create structure for Mail Log
        /// </summary>
        /// <param name="dtItem"></param>
        private static void Create_MailLog(ref DataTable dtItem)
        {
            //Log specific                                                 
            dtItem.Columns.Add(new DataColumn("EmailLog_id", System.Type.GetType("System.Int32")));
            dtItem.Columns.Add(new DataColumn("EmailSent_Info", System.Type.GetType("System.String")));
            dtItem.Columns.Add(new DataColumn("EmailSent_Date", System.Type.GetType("System.DateTime")));
            dtItem.Columns.Add(new DataColumn("EmailStatus_Fg", System.Type.GetType("System.Int32")));

        }

        /// <summary>
        /// Create structure for Mail List
        /// </summary>
        /// <param name="dtItem"></param>
        public static void Create_MailList(ref DataTable dtItem)
        {
            //Mail specific                                                 
            dtItem.Columns.Add(new DataColumn("EmailTo_nm", System.Type.GetType("System.String")));
            dtItem.Columns.Add(new DataColumn("EmailCc_nm", System.Type.GetType("System.String")));
            dtItem.Columns.Add(new DataColumn("EmailBcc_nm", System.Type.GetType("System.String")));
            dtItem.Columns.Add(new DataColumn("EmailSubject_nm", System.Type.GetType("System.String")));
            dtItem.Columns.Add(new DataColumn("EmailBody_nm", System.Type.GetType("System.String")));
            dtItem.Columns.Add(new DataColumn("FilePath", System.Type.GetType("System.String")));
            dtItem.Columns.Add(new DataColumn("EmailLog_id", System.Type.GetType("System.Int32")));
        }

        #endregion
    }
}

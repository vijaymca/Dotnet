#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: FeedbackHandler.cs
#endregion
using System;
using Shell.SharePoint.DREAM.Business.Entities;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.Collections.Specialized;
using System.Web;

namespace Shell.SharePoint.DREAM.MOSSProcess
{
    /// <summary>
    /// The FeedbackHandler class to handle all the Feedback related functions.
    /// </summary>
    public class FeedbackHandler
    {
        #region Declaration
        const string FEEDBACKLIST = "Feedback";
        const int RECORDSLIMIT = 2000;
        const string GENERALFEEDBACK = "General Feedback";
        #endregion
        #region Public Methods
        /// <summary>
        /// Updates the Feedback.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <param name="FeedbackInfo">The feedback submitted by user.</param>
        /// <param name="action">The action.</param>
        /// <returns>
        /// boolean returns whether update is success
        /// </returns>
        internal bool UpdateFeedback(string userID, Feedback feedbackInfo)
        {
            #region Method Variables
            bool blnIsSuccess = false;
            SPList feedbackList;
            SPFolder currentFolder;
            #endregion
            try
            {
                string strStorageURL = SPContext.Current.Site.Url;
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using(SPSite site = new SPSite(strStorageURL))
                    {
                        using(SPWeb web = site.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;
                            feedbackList = web.Lists[FEEDBACKLIST];
                            /// This condition will check any folders aleady exist inside list or not.
                            if(feedbackList.Folders.Count == 0)
                            {
                                string strFolderName = "Feedbacks(1-" + Convert.ToString(((feedbackList.Folders.Count) * RECORDSLIMIT) + RECORDSLIMIT) + ")";
                                SPListItem newFolder = feedbackList.Items.Add(string.Empty, SPFileSystemObjectType.Folder, strFolderName);
                                newFolder.Update();
                                AddFeedback(feedbackList, userID, feedbackInfo);
                                blnIsSuccess = true;
                            }
                            else
                            {
                                currentFolder = feedbackList.Folders[feedbackList.Folders.Count - 1].Folder;
                                int intLimit = ((feedbackList.Folders.Count - 1) * RECORDSLIMIT) + RECORDSLIMIT;
                                /// This condition will check for items in current folder exceeded the limit or not.
                                if(feedbackList.Items.Count < intLimit)
                                {
                                    AddFeedback(feedbackList, userID, feedbackInfo);
                                    blnIsSuccess = true;
                                }
                                else
                                {
                                    /// This will get the current folder in list.
                                    currentFolder = feedbackList.Folders[feedbackList.Folders.Count - 1].Folder;
                                    string strFolderName;
                                    /// Validates the folder name to be created with the correct bounderies.
                                    if(((Convert.ToInt32((currentFolder.Name.Split('-')[1].Split(')')[0]))) / RECORDSLIMIT) != feedbackList.Folders.Count)
                                    {
                                        int intRecordCount = ((Convert.ToInt32((currentFolder.Name.Split('-')[1].Split(')'))[0])) / RECORDSLIMIT);
                                        strFolderName = "Feedbacks(" + Convert.ToString((intRecordCount) * RECORDSLIMIT) + "-" + Convert.ToString(((intRecordCount) * RECORDSLIMIT) + RECORDSLIMIT) + ")";
                                    }
                                    else
                                    {
                                        strFolderName = "Feedbacks(" + Convert.ToString((feedbackList.Folders.Count) * RECORDSLIMIT) + "-" + Convert.ToString(((feedbackList.Folders.Count) * RECORDSLIMIT) + RECORDSLIMIT) + ")";
                                    }
                                    SPListItem newFolder = feedbackList.Items.Add(string.Empty, SPFileSystemObjectType.Folder, strFolderName);
                                    newFolder.Update();
                                    AddFeedback(feedbackList, userID, feedbackInfo);
                                    blnIsSuccess = true;
                                }
                            }
                        }
                    }
                });
            }
            catch(Exception)
            {
                throw;
            }
            return blnIsSuccess;
        }
        /// <summary>
        /// Adds the Feedback.
        /// </summary>
        internal void AddFeedback(SPList feedbackList, string userID, Feedback feedbackInfo)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    //Adds the new Feedback to the current folder.
                    SPFolder currentFolder = feedbackList.Folders[feedbackList.Folders.Count - 1].Folder;
                    ;
                    SPListItem newItem = feedbackList.Items.Add(currentFolder.ServerRelativeUrl, SPFileSystemObjectType.File, null);
                    newItem["Title"] = userID + "_" + DateTime.Now.ToString("MMM-dd-yyyy");
                    newItem["Rating"] = feedbackInfo.Rating;
                    newItem["Reason for Rating"] = feedbackInfo.Reason;
                    newItem["Additional Information"] = feedbackInfo.AdditionalInformation;
                    newItem["Type of Feedback"] = feedbackInfo.TypeofFeedback;
                    newItem["Page Level Comment"] = feedbackInfo.Comment;
                    if(feedbackInfo.TypeofFeedback.ToString() == GENERALFEEDBACK)
                        newItem["Page Name"] = string.Empty;
                    else
                        newItem["Page Name"] = feedbackInfo.PageName;
                    if(feedbackInfo.FileAttached != null)
                    {
                        newItem.Attachments.Add(feedbackInfo.FileName, feedbackInfo.FileAttached);
                    }

                    newItem.Update();
                });
            }
            catch(Exception)
            {
                throw;
            }

        }
        /// <summary>
        /// Send New Feedback alert mail to administrator.
        /// </summary>
        /// <param name="userMailID">MailID of user.</param>
        /// <param name="message">message.</param>
        /// <returns></returns>
        internal void SendAlertMail(StringDictionary header, string messageBody)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPUtility.SendEmail(SPContext.Current.Site.OpenWeb(), header, messageBody.ToString());
            });
        }
        #endregion
    }
}

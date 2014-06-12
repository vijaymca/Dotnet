#region Shell Copyright.2011
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: DeleteMarkedItems.cs
#endregion

using System;
using System.Diagnostics;
using System.Text;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace Shell.SharePoint.DWB.TimerJob.DeleteMarkedItems
{
    /// <summary>
    /// Timer job to delete items to be marked in DWB Books, DWB Chapters list and its corresponding items in other lists.
    /// </summary>
    public class DeleteMarkedItems :SPJobDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteMarkedItems"/> class.
        /// </summary>
        public DeleteMarkedItems():base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteMarkedItems"/> class.
        /// </summary>
        /// <param name="jobName">Name of the job.</param>
        /// <param name="webApplication">The web application.</param>
        public DeleteMarkedItems(string jobName, SPWebApplication webApplication)
            : base(jobName, webApplication, null, SPJobLockType.ContentDatabase)
        {
            this.Title = "eWB Deletion Timer Job Scheduler";
        }

        /// <summary>
        /// Executes the job definition.
        /// </summary>
        /// <param name="targetInstanceId">For target types of <see cref="T:Microsoft.SharePoint.Administration.SPContentDatabase"></see> this is the database ID of the content database being processed by the running job. This value is Guid.Empty for all other target types.</param>
        public override void Execute(Guid targetInstanceId)
        {
            try
            {
                base.Execute(targetInstanceId);
                DeleteItemsHelper objDeleteItemsHelper = new DeleteItemsHelper();
                SPWebApplication webApplication = this.Parent as SPWebApplication;
                string strCurrrentSiteURL = string.Empty;
                using (SPSite spSite = webApplication.Sites[0])
                {
                    strCurrrentSiteURL = spSite.Url;
                }
                if (!string.IsNullOrEmpty(strCurrrentSiteURL))
                {
                    objDeleteItemsHelper.DeleteItems(strCurrrentSiteURL);
                }
            }
            catch (Exception ex)
            {

                StringBuilder strExceptionMessage = new StringBuilder();
                strExceptionMessage.AppendLine(ex.Message.ToString());
                strExceptionMessage.AppendLine(ex.StackTrace.ToString());
                EventLog.WriteEntry("DREAM 4.0 eWB2 Delete Items Scheduler", strExceptionMessage.ToString());
            }
        }
    }
}

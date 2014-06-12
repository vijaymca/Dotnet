#region Shell Copyright.2011
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: DeleteMarkedItemsInstaller.cs
#endregion

using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace Shell.SharePoint.DWB.TimerJob.DeleteMarkedItems
{
    /// <summary>
    /// Feature receiver to install the timer job to delete items marked to be deleted in eWb2
    /// </summary>
   public class DeleteMarkedItemsInstaller:SPFeatureReceiver
    {
        /// <summary>
        /// Timer job name
        /// </summary>
       const string TIMER_JOB_NAME = "eWBDeletionTimerJob";

       /// <summary>
       /// Occurs after a Feature is activated.
       /// </summary>
       /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties"></see> object that represents the properties of the event.</param>
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            // register the the current web
            SPSite site = properties.Feature.Parent as SPSite;

            // make sure the job isn't already registered
            foreach (SPJobDefinition job in site.WebApplication.JobDefinitions)
            {
                if (job.Name == TIMER_JOB_NAME)
                    job.Delete();
            }

            // install the job
            DeleteMarkedItems taskLoggerJob = new DeleteMarkedItems(TIMER_JOB_NAME, site.WebApplication);
            SPMinuteSchedule minuteSchedule = new SPMinuteSchedule();
            minuteSchedule.BeginSecond = 0;
            minuteSchedule.EndSecond = 59;
            minuteSchedule.Interval = 1;

            taskLoggerJob.Schedule = minuteSchedule;   //SPDailySchedule.FromString("daily at 23:59:59");
            taskLoggerJob.Update();
        }

        /// <summary>
        /// Occurs when a Feature is deactivated.
        /// </summary>
        /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties"></see> object that represents the properties of the event.</param>
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPSite site = properties.Feature.Parent as SPSite;

            // delete the job
            foreach (SPJobDefinition job in site.WebApplication.JobDefinitions)
            {
                if (job.Name == TIMER_JOB_NAME)
                    job.Delete();
            }
        }

        /// <summary>
        /// Occurs after a Feature is installed.
        /// </summary>
        /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties"></see> object that represents the properties of the event.</param>
        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
          
        }

        /// <summary>
        /// Occurs when a Feature is uninstalled.
        /// </summary>
        /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties"></see> object that represents the properties of the event.</param>
        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
          
        }
    }
}

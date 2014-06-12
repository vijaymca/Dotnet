using System;
using System.Collections.Generic;
using System.Text;

namespace Shell.SharePoint.DWB.Business.DataObjects
{
   public interface IListRefs
    {

        string MasterPageList
        { get;set;}


        string MasterPageAuditList
        {
            get;
            set;
        }


        string MasterPageURL
        {
            get;
            set;
        }


        string TemplateList
        {
            get;
            set;
        }

        string TemplateAuditList
        {
            get;
            set;
        }


        string TemplatePagesMappingList
        {
            get;
            set;
        }


        string TemplateConfigurationAudit
        {
            get;
            set;
        }

       string TemplatePagesMappingAuditList
       {
           get;
           set;
       }
    }
}
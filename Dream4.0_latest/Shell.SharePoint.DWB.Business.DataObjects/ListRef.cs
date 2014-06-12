using System;
using System.Collections.Generic;
using System.Text;

namespace Shell.SharePoint.DWB.Business.DataObjects
{
   public class ListRef
    {
        private string strAppListName;
        protected string MasterPageList
        {
            get { return strAppListName; }
            set { strAppListName = value; }
        }

        private string strMASTERPAGEAUDITLIST;
        protected string MasterPageAuditList
        {
            get { return strMASTERPAGEAUDITLIST; }
            set { strMASTERPAGEAUDITLIST = value; }
        }

        private string strMASTERPAGEURL;
        protected string MasterPageURL
        {
            get { return strMASTERPAGEURL; }
            set { strMASTERPAGEURL = value; }
        }

        private string strTEMPLATELIST;
        protected string TemplateList
        {
            get { return strTEMPLATELIST; }
            set { strTEMPLATELIST = value; }
        }
        private string strTEMPLATEAUDITLIST;
        protected string TemplateAuditList
        {
            get { return strTEMPLATEAUDITLIST; }
            set { strTEMPLATEAUDITLIST = value; }
        }

        private string strTEMPLATEPAGESMAPPINGLIST;
        protected string TemplatePagesMappingList
        {
            get { return strTEMPLATEPAGESMAPPINGLIST; }
            set { strTEMPLATEPAGESMAPPINGLIST = value; }
        }

        private string strTEMPLATECONFIGURATIONAUDIT;
        protected string TemplateConfigurationAudit
        {
            get { return strTEMPLATECONFIGURATIONAUDIT; }
            set { strTEMPLATECONFIGURATIONAUDIT = value; }
        }

    }
}

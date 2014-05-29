using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EnzymeBAL;
using System.Data;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;

using DevExpress.Web.ASPxFormLayout;
using DevExpress.Web.Data;

namespace EnzymeWeb
{
    public partial class DemographicDataView : System.Web.UI.Page
    {
        clsDemographicBAL objclsDemographicBAL = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            objclsDemographicBAL = new clsDemographicBAL();
            DataSet objDS = new DataSet();

            objclsDemographicBAL.getDemographicDetails_BAL(ref objDS);

            if (objDS.Tables.Count > 0)
            {
                gvDemographicData.DataSource = objDS.Tables[0];
                gvDemographicData.DataBind();
            }

            if (!Page.IsPostBack)
            {
                ApplyLayout(0);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void BindData()
        {






        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnRowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            var x = e.Keys[0];
            //var y = e.

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnRowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            int id = Convert.ToInt32(e.Keys[0]);
            string ModifiedBy = "testuserdel";
            objclsDemographicBAL = new clsDemographicBAL();
            DataSet objDS = new DataSet();

            objclsDemographicBAL.DeteteDemographicDetails_BAL(id, ModifiedBy, ref objDS);

            objclsDemographicBAL = new clsDemographicBAL();
            objclsDemographicBAL.getDemographicDetails_BAL(ref objDS);
            if (objDS.Tables.Count > 0)
            {
                gvDemographicData.DataSource = objDS.Tables[0];
                gvDemographicData.DataBind();
            }
            ApplyLayout(0);
            e.Cancel = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="layoutIndex"></param>
        protected void ApplyLayout(int layoutIndex)
        {
            gvDemographicData.BeginUpdate();
            try
            {
                gvDemographicData.ClearSort();
                switch (layoutIndex)
                {
                    case 0:
                        gvDemographicData.GroupBy(gvDemographicData.Columns["Region_Name"]);
                        gvDemographicData.GroupBy(gvDemographicData.Columns["Country_Name"]);
                        break;
                    case 1:
                        gvDemographicData.GroupBy(gvDemographicData.Columns["Country_Name"]);
                        break;
                    case 2:
                        break;
                }
            }
            finally
            {
                gvDemographicData.EndUpdate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RowEditing(object sender, ASPxStartRowEditingEventArgs e)
        {
            int id = Convert.ToInt32(e.EditingKeyValue);

            if (Page.IsCallback)
                DevExpress.Web.ASPxClasses.ASPxWebControl.RedirectOnCallback("NewDemographicData.aspx?did=" + id);
            else
                Response.Redirect("NewDemographicData.aspx?divid=22&&ID=" + id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsCallback)
                DevExpress.Web.ASPxClasses.ASPxWebControl.RedirectOnCallback("NewDemographicData.aspx");
            else
                Response.Redirect("NewDemographicData.aspx");
        }
    }
}
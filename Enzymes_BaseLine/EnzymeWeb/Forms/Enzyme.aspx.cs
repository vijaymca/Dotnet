using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EnzymeBAL;
using System.Data;
using System.Collections;

namespace EnzymeWeb.Forms
{
    public partial class Enzyme : System.Web.UI.Page
    {
        string title = string.Empty;
        string createdby = "testuser";
        clsDemographicBAL objclsDemographicBAL = null;


        protected void Page_Load(object sender, EventArgs e)
        {
            BindGrid();
        }
        
        //common method for binding the gridview
        public void BindGrid()
        {

            objclsDemographicBAL = new clsDemographicBAL();
            DataSet objDS = new DataSet();

            //objclsDemographicBAL.getDemographicDetails_BAL(ref objDS);
            objclsDemographicBAL.getEnzynmeData_BAL(ref objDS);
            if (objDS.Tables.Count > 0)
            {

                gvEnzyme.DataSource = objDS.Tables[0];
                gvEnzyme.DataBind();
            }



        }
        //update event handled here
        protected void OnRowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {

            if (!IsPostBack)
            {
            }
            else
            {
                int id = Convert.ToInt32(e.Keys[0]);
                IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
                enumerator.Reset();
                while (enumerator.MoveNext())

                    title = enumerator.Value.ToString();

                objclsDemographicBAL = new clsDemographicBAL();
                DataSet objDS = new DataSet();

                //objclsDemographicBAL.getDemographicDetails_BAL(ref objDS);
                objclsDemographicBAL.UpdateEnzymeData_BAL(id, title, createdby, ref objDS);

                // grid.FocusedRowIndex = -1;
                if (objDS.Tables.Count > 0)
                {
                    //int resultid = Convert.ToInt32(objDS.Tables[0].Rows[0][0]);
                   // if (resultid > 0)
                    {
                        BindGrid();
                    }
                }
            }
            gvEnzyme.CancelEdit();

            e.Cancel = true;

            BindGrid();

        }
        //delete event
        protected void OnRowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            int outputparam = 1;
            int id = Convert.ToInt32(e.Keys[0]);

            objclsDemographicBAL = new clsDemographicBAL();
            DataSet objDS = new DataSet();

            objclsDemographicBAL.DeleteEnzymeData_BAL(id, createdby, outputparam);

            gvEnzyme.CancelEdit();
            e.Cancel = true;
            BindGrid();

            //if (objDS.Tables.Count > 0)
            //{
            //    int resultid = Convert.ToInt32(objDS.Tables[0].Rows[0][0]);

            //    if (resultid > 0)
            //    {
            //        BindGrid();
            //    }
            //}
        }
        //insert event
        protected void OnRowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            int outputparam = 0;

            if (IsPostBack)
            {
                string createdby = "test";
                IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
                string x = e.NewValues.ToString();

                enumerator.Reset();

                while (enumerator.MoveNext())
                {
                    title = enumerator.Value.ToString();
                }

                objclsDemographicBAL = new clsDemographicBAL();
                DataSet objDS = new DataSet();

                objclsDemographicBAL.InsertEnzymeData_BAL(title, createdby, outputparam);

                gvEnzyme.CancelEdit();
                e.Cancel = true;
                BindGrid();
            }
        }
    }
}
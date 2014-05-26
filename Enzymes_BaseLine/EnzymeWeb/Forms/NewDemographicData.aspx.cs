using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

using System.Web.UI.WebControls;
using EnzymeBAL;
using System.Data;
using EnzymeEntities.Entity;

using DevExpress.XtraEditors.Repository;

namespace EnzymeWeb
{
    public partial class NewDemographicData : System.Web.UI.Page
    {
        clsDemographicBAL objclsDemographicBAL = null;
        DemographicInfo demographics = null;

        static string demographicid = null;
        static bool IsOther = false;
        static bool IsonlyCampaign = false;
        static bool IsExit = false;
        static string compid;

        //static bool IsUpdate = false;

        //Constant lables
        const string NEW = "new";
        const string DEMOGRAPHICUPDATE = "demographicUpdate";
        const string CAMPAIGNUPDATE = "campaignUpdate";
        const string CREATENEWCAMP = "addnewCampaign";
        const string SAVEEXIT = "Save & Exit";
        const string SAVENEW = "Save";
        const string UPDATEEXIT = "Update & Exit";
        const string UPDATE = "Update";
        const string CANCEL = "Cancel";


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindDropDown();
                    if (Request.QueryString["did"] != null)
                    {
                        demographicid = Request.QueryString["did"].ToString();
                        fillEnzymeAndDemographic();
                        btnAddEnzyme.Visible = true;
                        IsExit = true;
                    }
                    else
                    {
                        pnlCompaign.Visible = true;
                        Session["Mode"] = NEW;
                        btnAddEnzyme.Visible = false;
                        IsExit = true;

                        ControlCollection ctrls = pnlCompaign.Controls;

                        foreach (var ctrl in ctrls)
                        {
                            if (ctrl is DevExpress.Web.ASPxEditors.ASPxTextBox)
                            {
                                ((DevExpress.Web.ASPxEditors.ASPxTextBox)ctrl).Text = "0";
                            }
                        }
                        txtTotalSitePop.Text = "0";
                        txtMedicalMonitor.Text = "0";
                    }
                    btnSaveExit.Text = SAVEEXIT;
                    btnSaveNew.Text = SAVENEW;
                }

            }
            // catch{Exception ex){}
            finally { }
            // return bReturn;
        }

        public void BindDropDown()
        {
            try
            {
                objclsDemographicBAL = new clsDemographicBAL();
                DataSet objDS = new DataSet();

                //Fill Regions
                objclsDemographicBAL.getRegionsDetails_BAL(ref objDS);

                if (objDS.Tables.Count > 0)
                {
                    drpRegion.DataSource = objDS.Tables[0];
                    drpRegion.DataBind();
                    drpRegion.Items.Insert(0, new DevExpress.Web.ASPxEditors.ListEditItem("Select a Region"));
                    drpRegion.SelectedIndex = 0;
                }

                objDS.Dispose();

                //Fill FiscalYear
                objclsDemographicBAL.getFiscalYr_BAL(ref objDS);

                if (objDS.Tables.Count > 0)
                {
                    drpFiscalYear.DataSource = objDS.Tables[0];
                    drpFiscalYear.DataBind();
                    drpFiscalYear.Items.Insert(0, new DevExpress.Web.ASPxEditors.ListEditItem("Select Fiscal Year"));
                    drpFiscalYear.SelectedIndex = 0;
                }

                objDS.Dispose();
                //Fill Enzyme
                objclsDemographicBAL.getEnzymeDetails_BAL(ref objDS);

                if (objDS.Tables.Count > 0)
                {
                    drpEnzyme.DataSource = objDS.Tables[0];
                    drpEnzyme.DataBind();
                    drpEnzyme.Items.Insert(0, new DevExpress.Web.ASPxEditors.ListEditItem("Select a Enzyme Name and Antigen Number"));
                    drpEnzyme.SelectedIndex = 0;
                }

                drpCountry.Items.Insert(0, new DevExpress.Web.ASPxEditors.ListEditItem("Select a Country"));
                drpCountry.SelectedIndex = 0;

                drpSiteName.Items.Insert(0, new DevExpress.Web.ASPxEditors.ListEditItem("Select a Site"));
                drpSiteName.SelectedIndex = 0;
            }
            finally
            {
                objclsDemographicBAL = null;
            }
        }

        protected void drpRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (drpRegion.SelectedIndex != 0)
                {
                    objclsDemographicBAL = new clsDemographicBAL();
                    DataSet objDS = new DataSet();
                    int regID = Convert.ToInt32(drpRegion.SelectedItem.Value);

                    objclsDemographicBAL.getCountryDetails_BAL(regID, ref objDS);

                    if (objDS.Tables.Count > 0)
                    {
                        drpCountry.DataSource = objDS.Tables[0];
                        drpCountry.DataBind();
                        drpCountry.Items.Insert(0, new DevExpress.Web.ASPxEditors.ListEditItem("Select a Country"));
                        drpCountry.SelectedIndex = 0;
                    }
                }
            }
            finally
            {
                objclsDemographicBAL = null;
            }
        }

        protected void drpCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (drpCountry.SelectedIndex != 0)
                {
                    objclsDemographicBAL = new clsDemographicBAL();
                    DataSet objDS = new DataSet();

                    int regID = Convert.ToInt32(drpRegion.SelectedItem.Value);
                    int cntryID = Convert.ToInt32(drpCountry.SelectedItem.Value);


                    objclsDemographicBAL.getSiteNameDetails_BAL(regID, cntryID, ref objDS);

                    if (objDS.Tables.Count > 0)
                    {
                        drpSiteName.DataSource = objDS.Tables[0];
                        drpSiteName.DataBind();
                        drpSiteName.Items.Insert(0, new DevExpress.Web.ASPxEditors.ListEditItem("Select a Site"));
                        drpSiteName.SelectedIndex = 0;
                    }
                }
                lblBusinessUnit.Text = string.Empty;
                lblCategory.Text = string.Empty;
                lblSector.Text = string.Empty;
            }
            finally
            {
                objclsDemographicBAL = null;
            }
        }

        protected void drpSiteName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (drpSiteName.SelectedIndex != 0)
                {
                    objclsDemographicBAL = new clsDemographicBAL();
                    DataSet objDS = new DataSet();

                    int siteID = Convert.ToInt32(drpSiteName.SelectedItem.Value);

                    objclsDemographicBAL.getBsnsCatSectr_BAL(siteID, ref objDS);

                    if (objDS.Tables[0].Rows.Count > 0)
                    {
                        lblBusinessUnit.Text = objDS.Tables[0].Rows[0]["GBU"] == null ? string.Empty : objDS.Tables[0].Rows[0]["GBU"].ToString();
                        hdnBusinessUnit.Value = objDS.Tables[0].Rows[0]["GBU_ID"] == null ? string.Empty : objDS.Tables[0].Rows[0]["GBU_ID"].ToString();


                        lblCategory.Text = objDS.Tables[0].Rows[0]["CategoryOwner"] == null ? string.Empty : objDS.Tables[0].Rows[0]["CategoryOwner"].ToString();
                        hdnCategory.Value = objDS.Tables[0].Rows[0]["CategoryOwner_ID"] == null ? string.Empty : objDS.Tables[0].Rows[0]["CategoryOwner_ID"].ToString();


                        lblSector.Text = objDS.Tables[0].Rows[0]["Sector"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Sector"].ToString();
                        hdnSector.Value = objDS.Tables[0].Rows[0]["Sector_ID"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Sector_ID"].ToString();

                    }
                    else
                    {
                        lblBusinessUnit.Text = string.Empty;
                        lblCategory.Text = string.Empty;
                        lblSector.Text = string.Empty;
                    }
                }
                else
                {
                    lblBusinessUnit.Text = string.Empty;
                    lblCategory.Text = string.Empty;
                    lblSector.Text = string.Empty;
                }
            }
            finally
            {
                objclsDemographicBAL = null;
            }
        }

        public void showCompaignNewForm()
        {
            pnlCompaign.Visible = true;
        }

        protected void fillEnzymeAndDemographic()
        {
            try
            {
                objclsDemographicBAL = new clsDemographicBAL();
                DataSet objDS = new DataSet();
                int demogrId = Convert.ToInt32(demographicid);

                objclsDemographicBAL.getEnzymeDetails_BAL(demogrId, ref objDS);

                if (objDS.Tables.Count > 0)
                {
                    grdEnzymes.DataSource = objDS.Tables[0];
                    grdEnzymes.DataBind();

                    lblCategory.Text = objDS.Tables[0].Rows[0]["Category"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Category"].ToString();
                    hdnCategory.Value = objDS.Tables[0].Rows[0]["Category_ID"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Category_ID"].ToString();


                    lblBusinessUnit.Text = objDS.Tables[0].Rows[0]["BusinessUnit"] == null ? string.Empty : objDS.Tables[0].Rows[0]["BusinessUnit"].ToString();
                    hdnBusinessUnit.Value = objDS.Tables[0].Rows[0]["BusinessUnit_ID"] == null ? string.Empty : objDS.Tables[0].Rows[0]["BusinessUnit_ID"].ToString();


                    lblSector.Text = objDS.Tables[0].Rows[0]["Sector"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Sector"].ToString();
                    hdnSector.Value = objDS.Tables[0].Rows[0]["Sector_ID"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Sector_ID"].ToString();


                    string pltform = objDS.Tables[0].Rows[0]["Platform"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Platform"].ToString();

                    if (pltform == "Liquid")
                    {
                        rdPlatform.Items[0].Selected = true;
                    }
                    else if (pltform == "Granules")
                    {
                        rdPlatform.Items[1].Selected = true;
                    }
                    else if (pltform == "Soluble Unit Dose [Pods]")
                    {
                        rdPlatform.Items[2].Selected = true;
                    }
                    else
                    {
                        rdPlatform.Items[3].Selected = true;
                        txtOther.Visible = true;
                        txtOther.Text = pltform;
                        IsOther = true;
                    }

                    string empStatus = objDS.Tables[0].Rows[0]["EmploymentStatus"] == null ? string.Empty : objDS.Tables[0].Rows[0]["EmploymentStatus"].ToString();
                    if (empStatus == "Contractors")
                    {
                        rdEmpStatus.Items[1].Selected = true;
                    }
                    else
                    {
                        rdEmpStatus.Items[0].Selected = true;
                    }


                    string fiscalYear = objDS.Tables[0].Rows[0]["FiscalYear"] == null ? "0" : objDS.Tables[0].Rows[0]["FiscalYear_ID"].ToString();


                    //if (fiscalYear == "FY 12-13")
                    //{
                    //    drpFiscalYear.Items[1].Selected = true;
                    //}
                    //else if (fiscalYear == "FY 13-14")
                    //{
                    //    drpFiscalYear.Items[2].Selected = true;
                    //}
                    //else if (fiscalYear == "FY 14-15")
                    //{
                    //    drpFiscalYear.Items[3].Selected = true;
                    //}
                    //else
                    //{
                    //    drpFiscalYear.Items[0].Selected = true;
                    //}


                    drpFiscalYear.Items.FindByValue(fiscalYear).Selected = true;


                    string campaign = objDS.Tables[0].Rows[0]["Campaign"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Campaign"].ToString();

                    if (campaign == "Spring")
                    {
                        rdCompaign.Items[0].Selected = true;
                    }
                    else if (campaign == "Fall")
                    {
                        rdCompaign.Items[1].Selected = true;
                    }
                    else if (campaign == "Annual")
                    {
                        rdCompaign.Items[2].Selected = true;
                    }
                    else
                    {
                        rdCompaign.Items[3].Selected = true;
                    }




                    string totalSitePopulation = objDS.Tables[0].Rows[0]["TotalSitePopulation"] == null ? string.Empty : objDS.Tables[0].Rows[0]["TotalSitePopulation"].ToString();
                    txtTotalSitePop.Text = totalSitePopulation;

                    string medicalMonitoringPro = objDS.Tables[0].Rows[0]["NumberofIndividualsGrade1"] == null ? string.Empty : objDS.Tables[0].Rows[0]["NumberofIndividualsGrade1"].ToString();
                    txtMedicalMonitor.Text = medicalMonitoringPro;

                    string principalreporter = objDS.Tables[0].Rows[0]["PrincipalReporter"] == null ? string.Empty : objDS.Tables[0].Rows[0]["PrincipalReporter"].ToString();
                    txtReporter.Text = principalreporter;



                    //Fill Region Dropdown
                    string region = objDS.Tables[0].Rows[0]["Region_ID"] == null ? "0" : objDS.Tables[0].Rows[0]["Region_ID"].ToString();

                    string country = objDS.Tables[0].Rows[0]["Country_ID"] == null ? "0" : objDS.Tables[0].Rows[0]["Country_ID"].ToString();

                    string site = objDS.Tables[0].Rows[0]["SiteName_ID"] == null ? string.Empty : objDS.Tables[0].Rows[0]["SiteName_ID"].ToString();

                    drpRegion.Items.FindByValue(region).Selected = true;
                    objDS = null;

                    //Fill Country dropdown

                    int regID = Convert.ToInt32(drpRegion.SelectedItem.Value);

                    objclsDemographicBAL.getCountryDetails_BAL(regID, ref objDS);

                    if (objDS.Tables.Count > 0)
                    {
                        drpCountry.DataSource = objDS.Tables[0];
                        drpCountry.DataBind();
                    }


                    drpCountry.Items.FindByValue(country).Selected = true;

                    //Fill Site dropdown
                    int reg2ID = Convert.ToInt32(drpRegion.SelectedItem.Value);
                    int cntryID = Convert.ToInt32(drpCountry.SelectedItem.Value);
                    objDS = null;

                    objclsDemographicBAL.getSiteNameDetails_BAL(reg2ID, cntryID, ref objDS);

                    if (objDS.Tables.Count > 0)
                    {
                        drpSiteName.DataSource = objDS.Tables[0];
                        drpSiteName.DataBind();
                    }

                    drpSiteName.Items.FindByValue(site).Selected = true;
                }

                btnSaveExit.Text = UPDATEEXIT;
                btnSaveNew.Text = UPDATE;
                // IsUpdate = true;
                Session["Mode"] = DEMOGRAPHICUPDATE;
                IsonlyCampaign = false;
            }

            finally
            {
                objclsDemographicBAL = null;
            }

        }

        protected void RowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            string id = e.EditingKeyValue.ToString();

            if (Page.IsCallback)
                pnlCompaign.Visible = true;
            else
                pnlCompaign.Visible = true;
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            //object id = grdEnzymes.GetRowValuesByKeyValue(null, "CampaignID");
            // string str = Convert.ToString(grdEnzymes.GetRowValuesByKeyValue("", "CampaignID"));

        }

        protected void rdPlatform_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdPlatform.SelectedIndex == 0 || rdPlatform.SelectedIndex == 1 || rdPlatform.SelectedIndex == 2)
            {
                txtOther.Visible = false;
                IsOther = false;
                //if (Session["Mode"].ToString() != DEMOGRAPHICUPDATE && Session["Mode"].ToString() != NEW)
                //displayEnzyme();
            }
            else
            {
                txtOther.Visible = true;
                IsOther = true;
                //if (Session["Mode"].ToString() != DEMOGRAPHICUPDATE && Session["Mode"].ToString() != NEW)
                //displayEnzyme();
            }
        }

        protected void btnAddEnzyme_Click(object sender, EventArgs e)
        {
            pnlCompaign.Visible = true;

            drpRegion.Enabled = false;
            drpCountry.Enabled = false;
            drpSiteName.Enabled = false;
            rdPlatform.Enabled = false;
            drpFiscalYear.Enabled = false;
            rdEmpStatus.Enabled = false;
            rdCompaign.Enabled = false;
            txtTotalSitePop.Enabled = false;
            txtMedicalMonitor.Enabled = false;
            txtReporter.Enabled = false;
            btnPersonalListing.Enabled = false;

            drpEnzyme.Visible = true;
            lblEnzyme.Visible = false;

            IsonlyCampaign = true;

            ControlCollection ctrls = pnlCompaign.Controls;

            foreach (var ctrl in ctrls)
            {
                if (ctrl is DevExpress.Web.ASPxEditors.ASPxTextBox)
                {
                    ((DevExpress.Web.ASPxEditors.ASPxTextBox)ctrl).Text = "0";
                }
            }

            Session["Mode"] = CREATENEWCAMP;

            IsExit = false;

            btnSaveExit.Text = SAVEEXIT;
            btnSaveNew.Text = SAVENEW;
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            IsExit = true;
            finalOperations();
        }

        protected void finalOperations()
        {
            string mode = Session["Mode"].ToString();
            switch (mode)
            {
                case NEW:
                    InsertDemoGraphicCampaign();
                    break;

                case DEMOGRAPHICUPDATE:
                    UpdateDemographInfo();
                    break;

                case CAMPAIGNUPDATE:
                    UpdateCampaign();
                    break;

                case CREATENEWCAMP:
                    InsertDemoGraphicCampaign();
                    fillEnzymeAndDemographic();
                    pnlCompaign.Visible = false;
                    break;
            }
            if (IsExit)
            {
                Response.Redirect("DemographicDataView.aspx");
            }
        }

        protected void rowcommand(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewRowCommandEventArgs e)
        {
            compid = e.KeyValue.ToString();
            Session["CampID"] = compid;
            displayEnzyme();
        }

        protected void displayEnzyme()
        {
            try
            {
                pnlCompaign.Visible = true;

                objclsDemographicBAL = new clsDemographicBAL();
                DataSet objDS = new DataSet();

                objclsDemographicBAL.getCompaignInfo_BAL(Convert.ToInt32(compid), ref objDS);

                if (objDS.Tables.Count > 0)
                {
                    string enzyme = objDS.Tables[0].Rows[0]["EnzymeName"] == null ? string.Empty : objDS.Tables[0].Rows[0]["EnzymeName"].ToString();

                    lblEnzyme.Text = enzyme;
                    lblEnzyme.Visible = true;
                    drpEnzyme.Visible = false;

                    //MAKING
                    txtTotalnumberofexposedindividuals.Text = objDS.Tables[0].Rows[0]["Making_ExposedInd"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Making_ExposedInd"].ToString();
                    txtTotalNumberofPreviousPositivesStillWorking.Text = objDS.Tables[0].Rows[0]["Making_PrePosWorking"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Making_PrePosWorking"].ToString();
                    txtTot_Num_Basline_Pos_Ind.Text = objDS.Tables[0].Rows[0]["Making_totNumBaseline"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Making_totNumBaseline"].ToString();
                    txtTot_Ava_for_testing.Text = objDS.Tables[0].Rows[0]["Making_TotalnotAvail"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Making_TotalnotAvail"].ToString();
                    txtTot_tobe_Tested.Text = objDS.Tables[0].Rows[0]["Making_Totaltotested"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Making_Totaltotested"].ToString();
                    txt_Tot_new_pos_wit_prior_nega_skin_tes.Text = objDS.Tables[0].Rows[0]["Making_SkinPos"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Making_SkinPos"].ToString();
                    txt_num_skin_test_neg.Text = objDS.Tables[0].Rows[0]["Making_SkinNeg"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Making_SkinNeg"].ToString();
                    txtNum_non_participants.Text = objDS.Tables[0].Rows[0]["Making_NonParticipants"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Making_NonParticipants"].ToString();


                    //PACKING
                    txt2Tot_num_f_exp_ind.Text = objDS.Tables[0].Rows[0]["Packing_ExposedInd"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Packing_ExposedInd"].ToString();
                    txt2_tot_num_f_pre_pos_sti_wor.Text = objDS.Tables[0].Rows[0]["Packing_PrePosWorking"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Packing_PrePosWorking"].ToString();
                    txt2_num_f_bas_pos_ind.Text = objDS.Tables[0].Rows[0]["Packing_totNumBaseline"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Packing_totNumBaseline"].ToString();
                    txt2_tot_avai_f_test.Text = objDS.Tables[0].Rows[0]["Packing_TotalnotAvail"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Packing_TotalnotAvail"].ToString();
                    txt2_tot_tob_test.Text = objDS.Tables[0].Rows[0]["Packing_Totaltotested"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Packing_Totaltotested"].ToString();
                    txt2_tot_new_pos_wt_pri_neg_skintest.Text = objDS.Tables[0].Rows[0]["Packing_SkinPos"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Packing_SkinPos"].ToString();
                    txt2num_f_ski_tes_neg.Text = objDS.Tables[0].Rows[0]["Packing_SkinNeg"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Packing_SkinNeg"].ToString();
                    txt2num_f_non_participants.Text = objDS.Tables[0].Rows[0]["Packing_NonParticipants"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Packing_NonParticipants"].ToString();


                    //DISTRIBUTION
                    txt_tot_num_of_exp_ind_distr.Text = objDS.Tables[0].Rows[0]["Distribution_ExposedInd"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Distribution_ExposedInd"].ToString();
                    txt_tot_num_pre_pos_st_wrk_distr.Text = objDS.Tables[0].Rows[0]["Distribution_PrePosWorking"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Distribution_PrePosWorking"].ToString();
                    txt_tot_num_of_base_pos_ind_distr.Text = objDS.Tables[0].Rows[0]["Distribution_totNumBaseline"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Distribution_totNumBaseline"].ToString();
                    txt_tot_not_ava_for_test_distr.Text = objDS.Tables[0].Rows[0]["Distribution_TotalnotAvail"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Distribution_TotalnotAvail"].ToString();
                    txt_tot_to_be_test_distr.Text = objDS.Tables[0].Rows[0]["Distribution_Totaltotested"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Distribution_Totaltotested"].ToString();
                    txt_tot_new_pos_wit_pri_neg_skin_test_distr.Text = objDS.Tables[0].Rows[0]["Distribution_SkinPos"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Distribution_SkinPos"].ToString();
                    txt_num_of_skin_test_neg_distr.Text = objDS.Tables[0].Rows[0]["Distribution_SkinNeg"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Distribution_SkinNeg"].ToString();
                    txt_num_of_non_part_distr.Text = objDS.Tables[0].Rows[0]["Distribution_NonParticipants"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Distribution_NonParticipants"].ToString();


                    //OTHERS
                    txt_tot_num_of_exp_ind_Other.Text = objDS.Tables[0].Rows[0]["Others_ExposedInd"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Others_ExposedInd"].ToString();
                    tot_num_of_pre_pos_stil_work_other.Text = objDS.Tables[0].Rows[0]["Others_PrePosWorking"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Others_PrePosWorking"].ToString();
                    txt_tot_num_of_base_pos_ind_other.Text = objDS.Tables[0].Rows[0]["Other_totNumBaseline"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Other_totNumBaseline"].ToString();
                    txt_tot_not_avai_for_test_Other.Text = objDS.Tables[0].Rows[0]["Others_TotalnotAvail"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Others_TotalnotAvail"].ToString();
                    txt_tot_to_be_test_Other.Text = objDS.Tables[0].Rows[0]["Others_Totaltotested"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Others_Totaltotested"].ToString();
                    txt_tot_new_pos_wit_pri_neg_ski_test_Other.Text = objDS.Tables[0].Rows[0]["Others_SkinPos"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Others_SkinPos"].ToString();
                    txt_num_of_ski_neg_Other.Text = objDS.Tables[0].Rows[0]["Others_SkinNeg"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Others_SkinNeg"].ToString();
                    txt_num_non_parti_other.Text = objDS.Tables[0].Rows[0]["Others_NonParticipants"] == null ? string.Empty : objDS.Tables[0].Rows[0]["Others_NonParticipants"].ToString();

                }
                Session["Mode"] = CAMPAIGNUPDATE;


                //Demographic enable
                drpRegion.Enabled = true;
                drpCountry.Enabled = true;
                drpSiteName.Enabled = true;
                rdPlatform.Enabled = true;
                drpFiscalYear.Enabled = true;
                rdEmpStatus.Enabled = true;
                rdCompaign.Enabled = true;
                txtTotalSitePop.Enabled = true;
                txtMedicalMonitor.Enabled = true;
                txtReporter.Enabled = true;
                btnPersonalListing.Enabled = true;

                drpEnzyme.Visible = false;
                lblEnzyme.Visible = true;
                //Demographic Enable

            }
            finally
            {
                objclsDemographicBAL = null;
            }
        }

        protected void UpdateDemographInfo()
        {
            try
            {
                objclsDemographicBAL = new clsDemographicBAL();
                DataSet objDS = new DataSet();

                demographics = new DemographicInfo();

                demographics.Region = Convert.ToInt32(drpRegion.SelectedItem.Value);
                demographics.Country = Convert.ToInt32(drpCountry.SelectedItem.Value);
                demographics.SiteName = Convert.ToInt32(drpSiteName.SelectedItem.Value);
                demographics.BusinessUnit = Convert.ToInt32(hdnBusinessUnit.Value);
                demographics.Category = Convert.ToInt32(hdnCategory.Value);
                demographics.Sector = Convert.ToInt32(hdnSector.Value);

                if (IsOther)
                {
                    demographics.Platform = txtOther.Text;
                }
                else
                {
                    demographics.Platform = rdPlatform.SelectedItem.Text;
                }

                demographics.EmpployeeStatus = rdEmpStatus.SelectedItem.Text;
                demographics.FiscalYr = Convert.ToInt32(drpFiscalYear.SelectedItem.Value);
                demographics.Campaign = rdCompaign.SelectedItem.Text;
                demographics.TotalSitePop = Convert.ToInt32(txtTotalSitePop.Value);
                demographics.NumMedicalMoniProg = Convert.ToInt32(txtMedicalMonitor.Text);
                // demographics.PrincipalReporter = Convert.ToInt32(txtReporter.Text);
                demographics.PrincipalReporter = 1;

                demographics.DemographicID = Convert.ToInt32(demographicid);

                string strout = string.Empty;

                objclsDemographicBAL.UpdateDemographic_BAL(demographics, IsOther,ref strout, ref objDS);
                
                lblReturnStatus.Text = strout;

            }
            finally
            {
                demographics = null;
                objclsDemographicBAL = null;
            }
        }

        protected void InsertDemoGraphicCampaign()
        {

            try
            {
                demographics = new DemographicInfo();

                if (!IsonlyCampaign)
                {
                    demographics.Region = Convert.ToInt32(drpRegion.SelectedItem.Value);
                    demographics.Country = Convert.ToInt32(drpCountry.SelectedItem.Value);
                    demographics.SiteName = Convert.ToInt32(drpSiteName.SelectedItem.Value);

                    demographics.BusinessUnit = Convert.ToInt32(hdnBusinessUnit.Value);
                    demographics.Category = Convert.ToInt32(hdnCategory.Value);
                    demographics.Sector = Convert.ToInt32(hdnSector.Value);

                    if (IsOther)
                    {
                        demographics.Platform = txtOther.Text;
                    }
                    else
                    {
                        demographics.Platform = rdPlatform.SelectedItem.Text;
                    }

                    demographics.EmpployeeStatus = rdEmpStatus.SelectedItem.Text;
                    demographics.FiscalYr = Convert.ToInt32(drpFiscalYear.SelectedItem.Value);
                    demographics.Campaign = rdCompaign.SelectedItem.Text;
                    demographics.TotalSitePop = Convert.ToInt32(txtTotalSitePop.Value);
                    demographics.NumMedicalMoniProg = Convert.ToInt32(txtMedicalMonitor.Value);
                    //demographics.PrincipalReporter = txtReporter.Text;
                    demographics.PrincipalReporter = 1;
                }
                else
                {
                    demographics.DemographicID = Convert.ToInt32(demographicid);
                }

                //MAKING
                demographics.EnzymeID = Convert.ToInt32(drpEnzyme.SelectedItem.Value);
                demographics.Total_num_of_Exposed_Individuals = Convert.ToInt32(txtTotalnumberofexposedindividuals.Text);
                demographics.Total_num_of_prev_pos_Stillwork = Convert.ToInt32(txtTotalNumberofPreviousPositivesStillWorking.Text);
                demographics.Tot_Num_f_Base_Pos_Ind = Convert.ToInt32(txtTot_Num_Basline_Pos_Ind.Text);
                demographics.Tot_not_avail_fr_test = Convert.ToInt32(txtTot_Ava_for_testing.Text);
                demographics.Tot_to_be_Testd = Convert.ToInt32(txtTot_tobe_Tested.Text);
                demographics.Tot_New_pos_wth_prior_neg_skintest = Convert.ToInt32(txt_Tot_new_pos_wit_prior_nega_skin_tes.Text);
                demographics.Num_of_Skin_TestNeg = Convert.ToInt32(txt_num_skin_test_neg.Text);
                demographics.Num_of_non_part = Convert.ToInt32(txtNum_non_participants.Text);

                //PACKING
                demographics.Tot_num_of_exp_ind2 = Convert.ToInt32(txt2Tot_num_f_exp_ind.Text);
                demographics.Tot_Num_of_Pre_Pos_Still_Work2 = Convert.ToInt32(txt2_tot_num_f_pre_pos_sti_wor.Text);
                demographics.Tot_Num_of_Base_Pos_Ind2 = Convert.ToInt32(txt2_num_f_bas_pos_ind.Text);
                demographics.Tot_not_avai_for_testing2 = Convert.ToInt32(txt2_tot_avai_f_test.Text);
                demographics.Tot_to_be_Test2 = Convert.ToInt32(txt2_tot_tob_test.Text);
                demographics.Tot_New_pos_with_pri_nega_skin_test2 = Convert.ToInt32(txt2_tot_new_pos_wt_pri_neg_skintest.Text);
                demographics.Num_of_Skin_Test_Nega2 = Convert.ToInt32(txt2num_f_ski_tes_neg.Text);
                demographics.Num_of_non_participants2 = Convert.ToInt32(txt2num_f_non_participants.Text);


                //DISTRIBUTION
                demographics.Tot_num_of_exp_ind_distr = Convert.ToInt32(txt_tot_num_of_exp_ind_distr.Text);
                demographics.Tot_Num_of_Pre_Pos_Still_Work_distr = Convert.ToInt32(txt_tot_num_pre_pos_st_wrk_distr.Text);
                demographics.Tot_Num_of_Base_Pos_Ind_distr = Convert.ToInt32(txt_tot_num_of_base_pos_ind_distr.Text);
                demographics.Tot_not_avai_for_testing_distr = Convert.ToInt32(txt_tot_not_ava_for_test_distr.Text);
                demographics.Tot_to_be_Test2_distr = Convert.ToInt32(txt_tot_to_be_test_distr.Text);
                demographics.Tot_New_pos_with_pri_nega_skin_test_distr = Convert.ToInt32(txt_tot_new_pos_wit_pri_neg_skin_test_distr.Text);

                demographics.Num_of_Skin_Test_Nega_distr = Convert.ToInt32(txt_num_of_skin_test_neg_distr.Text);
                demographics.Num_of_non_participants_distr = Convert.ToInt32(txt_num_of_non_part_distr.Text);

                //OTHER
                demographics.Tot_num_of_exp_ind_Other = Convert.ToInt32(txt_tot_num_of_exp_ind_Other.Text);
                demographics.Tot_Num_of_Pre_Pos_Still_Work_Other = Convert.ToInt32(tot_num_of_pre_pos_stil_work_other.Text);
                demographics.Tot_Num_of_Base_Pos_Ind_Other = Convert.ToInt32(txt_tot_num_of_base_pos_ind_other.Text);
                demographics.Tot_not_avai_for_testing_Other = Convert.ToInt32(txt_tot_not_avai_for_test_Other.Text);
                demographics.Tot_to_be_Test2_Other = Convert.ToInt32(txt_tot_to_be_test_Other.Text);
                demographics.Tot_New_pos_with_pri_nega_skin_test_Other = Convert.ToInt32(txt_tot_new_pos_wit_pri_neg_ski_test_Other.Text);

                demographics.Num_of_Skin_Test_Nega_Other = Convert.ToInt32(txt_num_of_ski_neg_Other.Text);
                demographics.Num_of_non_participants_Other = Convert.ToInt32(txt_num_non_parti_other.Text);


                objclsDemographicBAL = new clsDemographicBAL();
                DataSet objDS = new DataSet();

                string strout =string.Empty;
                objclsDemographicBAL.InsertInsertDemographicAndEnzyme_BAL(demographics, IsonlyCampaign, ref strout, ref objDS);

                lblReturnStatus.Text = strout;
            }
            finally
            {
                demographics = null;
                objclsDemographicBAL = null;
            }
        }

        protected void UpdateCampaign()
        {
            try
            {
                demographics = new DemographicInfo();


                demographics.CampaignID = Convert.ToInt32(Session["CampID"].ToString());

                //MAKING
                demographics.EnzymeName = drpEnzyme.SelectedItem.Text;
                demographics.Total_num_of_Exposed_Individuals = Convert.ToInt32(txtTotalnumberofexposedindividuals.Text);
                demographics.Total_num_of_prev_pos_Stillwork = Convert.ToInt32(txtTotalNumberofPreviousPositivesStillWorking.Text);
                demographics.Tot_Num_f_Base_Pos_Ind = Convert.ToInt32(txtTot_Num_Basline_Pos_Ind.Text);
                demographics.Tot_not_avail_fr_test = Convert.ToInt32(txtTot_Ava_for_testing.Text);
                demographics.Tot_to_be_Testd = Convert.ToInt32(txtTot_tobe_Tested.Text);
                demographics.Tot_New_pos_wth_prior_neg_skintest = Convert.ToInt32(txt_Tot_new_pos_wit_prior_nega_skin_tes.Text);
                demographics.Num_of_Skin_TestNeg = Convert.ToInt32(txt_num_skin_test_neg.Text);
                demographics.Num_of_non_part = Convert.ToInt32(txtNum_non_participants.Text);

                //PACKING
                demographics.Tot_num_of_exp_ind2 = Convert.ToInt32(txt2Tot_num_f_exp_ind.Text);
                demographics.Tot_Num_of_Pre_Pos_Still_Work2 = Convert.ToInt32(txt2_tot_num_f_pre_pos_sti_wor.Text);
                demographics.Tot_Num_of_Base_Pos_Ind2 = Convert.ToInt32(txt2_num_f_bas_pos_ind.Text);
                demographics.Tot_not_avai_for_testing2 = Convert.ToInt32(txt2_tot_avai_f_test.Text);
                demographics.Tot_to_be_Test2 = Convert.ToInt32(txt2_tot_tob_test.Text);
                demographics.Tot_New_pos_with_pri_nega_skin_test2 = Convert.ToInt32(txt2_tot_new_pos_wt_pri_neg_skintest.Text);
                demographics.Num_of_Skin_Test_Nega2 = Convert.ToInt32(txt2num_f_ski_tes_neg.Text);
                demographics.Num_of_non_participants2 = Convert.ToInt32(txt2num_f_non_participants.Text);


                //DISTRIBUTION

                demographics.Tot_num_of_exp_ind_distr = Convert.ToInt32(txt_tot_num_of_exp_ind_distr.Text);
                demographics.Tot_Num_of_Pre_Pos_Still_Work_distr = Convert.ToInt32(txt_tot_num_pre_pos_st_wrk_distr.Text);
                demographics.Tot_Num_of_Base_Pos_Ind_distr = Convert.ToInt32(txt_tot_num_of_base_pos_ind_distr.Text);
                demographics.Tot_not_avai_for_testing_distr = Convert.ToInt32(txt_tot_not_ava_for_test_distr.Text);
                demographics.Tot_to_be_Test2_distr = Convert.ToInt32(txt_tot_to_be_test_distr.Text);
                demographics.Tot_New_pos_with_pri_nega_skin_test_distr = Convert.ToInt32(txt_tot_new_pos_wit_pri_neg_skin_test_distr.Text);

                demographics.Num_of_Skin_Test_Nega_distr = Convert.ToInt32(txt_num_of_skin_test_neg_distr.Text);
                demographics.Num_of_non_participants_distr = Convert.ToInt32(txt_num_of_non_part_distr.Text);

                //OTHER
                demographics.Tot_num_of_exp_ind_Other = Convert.ToInt32(txt_tot_num_of_exp_ind_Other.Text);
                demographics.Tot_Num_of_Pre_Pos_Still_Work_Other = Convert.ToInt32(tot_num_of_pre_pos_stil_work_other.Text);
                demographics.Tot_Num_of_Base_Pos_Ind_Other = Convert.ToInt32(txt_tot_num_of_base_pos_ind_other.Text);
                demographics.Tot_not_avai_for_testing_Other = Convert.ToInt32(txt_tot_not_avai_for_test_Other.Text);
                demographics.Tot_to_be_Test2_Other = Convert.ToInt32(txt_tot_to_be_test_Other.Text);
                demographics.Tot_New_pos_with_pri_nega_skin_test_Other = Convert.ToInt32(txt_tot_new_pos_wit_pri_neg_ski_test_Other.Text);

                demographics.Num_of_Skin_Test_Nega_Other = Convert.ToInt32(txt_num_of_ski_neg_Other.Text);
                demographics.Num_of_non_participants_Other = Convert.ToInt32(txt_num_non_parti_other.Text);


                objclsDemographicBAL = new clsDemographicBAL();
                DataSet objDS = new DataSet();

                string strout = string.Empty;

                objclsDemographicBAL.UpdateCompaign_BAL(demographics, ref strout, ref objDS);

                UpdateDemographInfo();

                lblReturnStatus.Text = strout;

            }
            finally
            {
                demographics = null;
                objclsDemographicBAL = null;
            }

        }

        protected void OnRowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            int id = Convert.ToInt32(e.Keys[0]);
            string ModifiedBy = "testuserdel";
            objclsDemographicBAL = new clsDemographicBAL();

            DataSet objDS = new DataSet();
            string strout = string.Empty;

            objclsDemographicBAL.DeteteCampaign_BAL(id, ModifiedBy,ref strout, ref objDS);

            objclsDemographicBAL = new clsDemographicBAL();

            //Fill Enzyme Campaign
            int demogrId = Convert.ToInt32(demographicid);
            objclsDemographicBAL.getEnzymeDetails_BAL(demogrId, ref objDS);
            if (objDS.Tables.Count > 0)
            {
                grdEnzymes.DataSource = objDS.Tables[0];
                grdEnzymes.DataBind();
            }
            //ApplyLayout(0);
            e.Cancel = true;
            lblReturnStatus.Text = strout;
        }

        protected void btnSaveNew_Click(object sender, EventArgs e)
        {
            IsExit = false;
            finalOperations();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (IsExit)
                Response.Redirect("DemographicDataView.aspx");
            else
            {
                pnlCompaign.Visible = false;
                drpRegion.Enabled = true;
                drpCountry.Enabled = true;
                drpSiteName.Enabled = true;
                rdPlatform.Enabled = true;
                drpFiscalYear.Enabled = true;
                rdEmpStatus.Enabled = true;
                rdCompaign.Enabled = true;
                txtTotalSitePop.Enabled = true;
                txtMedicalMonitor.Enabled = true;
                txtReporter.Enabled = true;
                btnPersonalListing.Enabled = true;

                drpEnzyme.Visible = false;
                lblEnzyme.Visible = true;

                IsonlyCampaign = false;

                btnSaveExit.Text = UPDATEEXIT;
                btnSaveNew.Text = UPDATE;
                IsExit = true;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using EnzymeDBPool;
using System.Data;
using EnzymeEntities.Entity;

namespace EnzymeDAL
{
    public class clsDemographicDAL
    {
        String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EnzymeConn"].ConnectionString;

        DBPool objDBPool = null;
        SqlParameter[] paramIn = null;
        SqlParameter[] paramOut = null;

        public clsDemographicDAL()
        {
            objDBPool = new DBPool();
            objDBPool.ConnectionString = ConnectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intGBUID"></param>
        /// <param name="objDS"></param>
        /// <returns></returns>
        public bool getCountrySitesDetails_DO(int RegionID, int CountryID, ref DataSet objDS)
        {
            try
            {
                paramIn = new SqlParameter[2];
                paramIn[0] = new SqlParameter("@p_RegionID", SqlDbType.Int);
                paramIn[0].Value = 31;

                paramIn[1] = new SqlParameter("@p_CountryID", SqlDbType.Int);
                paramIn[1].Value = 182;

                if (objDBPool.SpQueryDataset("USP_GET_CountrySites", paramIn, ref objDS))
                    return true;
                else return false;
            }
            finally
            {
                objDBPool = null;
                paramIn = null;
            }
        }

        public bool getCountryDetailsDAL(int regID, ref DataSet objDS)
        {
            try
            {
                paramIn = new SqlParameter[1];
                paramIn[0] = new SqlParameter("@p_RegionID", SqlDbType.Int);
                paramIn[0].Value = regID;

                if (objDBPool.SpQueryDataset("USP_GET_RegionCountry", paramIn, ref objDS))
                    return true;
                else return false;
            }
            finally
            {
                objDBPool = null;
                //paramIn = null;
            }
        }

        public bool getBsnsCatSectrDAL(int siteID, ref DataSet objDS)
        {
            try
            {
                paramIn = new SqlParameter[1];
                paramIn[0] = new SqlParameter("@p_ID", SqlDbType.Int);
                paramIn[0].Value = siteID;

                if (objDBPool.SpQueryDataset("USP_GET_lst_Sites_basedonSiteID", paramIn, ref objDS))
                    return true;
                else return false;
            }
            finally
            {
                objDBPool = null;
                //paramIn = null;
            }
        }

        public bool getCompaignInfoDAL(int CompId, ref DataSet objDS)
        {
            try
            {
                paramIn = new SqlParameter[1];
                paramIn[0] = new SqlParameter("@P_CompaignID", SqlDbType.Int);
                paramIn[0].Value = CompId;

                if (objDBPool.SpQueryDataset("USP_GET_Campaign_Data", paramIn, ref objDS))
                    return true;
                else return false;
            }
            finally
            {
                objDBPool = null;
            }
        }

        public bool getRegionDetailsDAL(ref DataSet objDS)
        {
            try
            {
                if (objDBPool.SpQueryDataset("USp_GET_Region", ref objDS))
                    return true;
                else return false;
            }

            finally
            {
                objDBPool = null;
                //paramIn = null;
            }
        }

        public bool getSiteNameDetailsDAL(int regID, int cntryID, ref DataSet objDS)
        {
            try
            {
                paramIn = new SqlParameter[2];
                paramIn[0] = new SqlParameter("@p_RegionID", SqlDbType.Int);
                paramIn[0].Value = regID;

                paramIn[1] = new SqlParameter("@p_CountryID", SqlDbType.Int);
                paramIn[1].Value = cntryID;

                if (objDBPool.SpQueryDataset("USP_GET_CountrySites", paramIn, ref objDS))
                    return true;
                else return false;
            }
            finally
            {
                objDBPool = null;
                //paramIn = null;
            }

        }

        public bool getDemographicDetailsDAL(ref DataSet objDS)
        {
            try
            {
                if (objDBPool.SpQueryDataset("USP_GET_EnzymeDemographic_Data", ref objDS))
                    return true;
                else return false;
            }

            finally
            {
                objDBPool = null;
            }
        }

        public bool getEnzymeDetailsDAL(ref DataSet objDS)
        {
            try
            {
                if (objDBPool.SpQueryDataset("USP_GET_Enzyme_Data", ref objDS))
                    return true;
                else return false;
            }

            finally
            {
                objDBPool = null;
            }
        }

        public bool getEnzymeDetailsDAL(int demogrId, ref DataSet objDS)
        {
            try
            {
                paramIn = new SqlParameter[1];
                paramIn[0] = new SqlParameter("@P_DemographicID", SqlDbType.Int);
                paramIn[0].Value = demogrId;

                if (objDBPool.SpQueryDataset("USP_GET_EnzymeDemographic_Campaign_Data", paramIn, ref objDS))
                    return true;
                else return false;
            }
            finally
            {
                objDBPool = null;
            }

        }

        public bool InsertDemographicAndEnzymeDAL(DemographicInfo demogrpahic,ref string strout,  ref DataSet objDS)
        {
            bool tmp = false;
            try
            {
                paramIn = new SqlParameter[49];
                paramIn[0] = new SqlParameter("@P_DemographicName", SqlDbType.VarChar);
                paramIn[0].Value = "";

                paramIn[1] = new SqlParameter("@P_Region_ID", SqlDbType.Int);
                paramIn[1].Value = demogrpahic.Region;

                paramIn[2] = new SqlParameter("@P_Country_ID", SqlDbType.Int);
                paramIn[2].Value = demogrpahic.Country;

                paramIn[3] = new SqlParameter("@P_SiteName_ID", SqlDbType.Int);
                paramIn[3].Value = demogrpahic.SiteName;

                paramIn[4] = new SqlParameter("@P_BusinessUnit_ID", SqlDbType.Int);
                paramIn[4].Value = demogrpahic.BusinessUnit;

                paramIn[5] = new SqlParameter("@P_Category_ID", SqlDbType.Int);
                paramIn[5].Value = demogrpahic.Category;

                paramIn[6] = new SqlParameter("@P_Sector_ID", SqlDbType.Int);
                paramIn[6].Value = demogrpahic.Sector;

                paramIn[7] = new SqlParameter("@P_Platform", SqlDbType.VarChar);
                paramIn[7].Value = demogrpahic.Platform;

                paramIn[8] = new SqlParameter("@P_EmploymentStatus", SqlDbType.VarChar);
                paramIn[8].Value = demogrpahic.EmpployeeStatus;

                paramIn[9] = new SqlParameter("@P_FiscalYear_ID", SqlDbType.Int);
                paramIn[9].Value = demogrpahic.FiscalYr;

                paramIn[10] = new SqlParameter("@P_Campaign", SqlDbType.VarChar);
                paramIn[10].Value = demogrpahic.Campaign;

                paramIn[11] = new SqlParameter("@P_TotalSitePopulation", SqlDbType.Int);
                paramIn[11].Value = demogrpahic.TotalSitePop;

                paramIn[12] = new SqlParameter("@P_NumberofIndividualsGrade1", SqlDbType.Int);
                paramIn[12].Value = demogrpahic.NumMedicalMoniProg;

                paramIn[13] = new SqlParameter("@P_PrincipalReporter_ID", SqlDbType.Int);
                paramIn[13].Value = demogrpahic.PrincipalReporter;

                paramIn[14] = new SqlParameter("@P_CreatedBy", SqlDbType.VarChar);
                paramIn[14].Value = "Stati User";

                //Campaign information

                paramIn[15] = new SqlParameter("@P_EnzymeName_ID", SqlDbType.Int);
                paramIn[15].Value = demogrpahic.EnzymeID;

                // Making
                paramIn[16] = new SqlParameter("@P_Making_ExposedInd", SqlDbType.Int);
                paramIn[17] = new SqlParameter("@P_Making_PrePosWorking", SqlDbType.Int);
                paramIn[18] = new SqlParameter("@P_Making_totNumBaseline", SqlDbType.Int);
                paramIn[19] = new SqlParameter("@P_Making_TotalnotAvail", SqlDbType.Int);
                paramIn[20] = new SqlParameter("@P_Making_Totaltotested", SqlDbType.Int);
                paramIn[21] = new SqlParameter("@P_Making_SkinPos", SqlDbType.Int);
                paramIn[22] = new SqlParameter("@P_Making_SkinNeg", SqlDbType.Int);
                paramIn[23] = new SqlParameter("@P_Making_NonParticipants", SqlDbType.Int);

                //PACKING
                paramIn[24] = new SqlParameter("@P_Packing_ExposedInd", SqlDbType.Int);
                paramIn[25] = new SqlParameter("@P_Packing_PrePosWorking", SqlDbType.Int);
                paramIn[26] = new SqlParameter("@P_Packing_totNumBaseline", SqlDbType.Int);
                paramIn[27] = new SqlParameter("@P_Packing_TotalnotAvail", SqlDbType.Int);
                paramIn[28] = new SqlParameter("@P_Packing_Totaltotested", SqlDbType.Int);
                paramIn[29] = new SqlParameter("@P_Packing_SkinPos", SqlDbType.Int);
                paramIn[30] = new SqlParameter("@P_Packing_SkinNeg", SqlDbType.Int);
                paramIn[31] = new SqlParameter("@P_Packing_NonParticipants", SqlDbType.Int);

                //DISTRiBUTION
                paramIn[32] = new SqlParameter("@P_Distribution_ExposedInd", SqlDbType.Int);
                paramIn[33] = new SqlParameter("@P_Distribution_PrePosWorking", SqlDbType.Int);
                paramIn[34] = new SqlParameter("@P_Distribution_totNumBaseline", SqlDbType.Int);
                paramIn[35] = new SqlParameter("@P_Distribution_TotalnotAvail", SqlDbType.Int);
                paramIn[36] = new SqlParameter("@P_Distribution_Totaltotested", SqlDbType.Int);
                paramIn[37] = new SqlParameter("@P_Distribution_SkinPos", SqlDbType.Int);
                paramIn[38] = new SqlParameter("@P_Distribution_SkinNeg", SqlDbType.Int);
                paramIn[39] = new SqlParameter("@P_Distribution_NonParticipants", SqlDbType.Int);

                //OTHERS
                paramIn[40] = new SqlParameter("@P_Others_ExposedInd", SqlDbType.Int);
                paramIn[41] = new SqlParameter("@P_Others_PrePosWorking", SqlDbType.Int);
                paramIn[42] = new SqlParameter("@P_Other_totNumBaseline", SqlDbType.Int);
                paramIn[43] = new SqlParameter("@P_Others_TotalnotAvail", SqlDbType.Int);
                paramIn[44] = new SqlParameter("@P_Others_Totaltotested", SqlDbType.Int);
                paramIn[45] = new SqlParameter("@P_Others_SkinPos", SqlDbType.Int);
                paramIn[46] = new SqlParameter("@P_Others_SkinNeg", SqlDbType.Int);
                paramIn[47] = new SqlParameter("@P_Others_NonParticipants", SqlDbType.Int);

                paramIn[48] = new SqlParameter("@P_ISCampaign", SqlDbType.VarChar);
                paramIn[48].Value = "yes";


                //MAKING
                paramIn[16].Value = demogrpahic.Total_num_of_Exposed_Individuals;
                paramIn[17].Value = demogrpahic.Total_num_of_prev_pos_Stillwork;
                paramIn[18].Value = demogrpahic.Tot_Num_f_Base_Pos_Ind;
                paramIn[19].Value = demogrpahic.Tot_not_avail_fr_test;
                paramIn[20].Value = demogrpahic.Tot_to_be_Testd;
                paramIn[21].Value = demogrpahic.Tot_New_pos_wth_prior_neg_skintest;
                paramIn[22].Value = demogrpahic.Num_of_Skin_TestNeg;
                paramIn[23].Value = demogrpahic.Num_of_non_part;


                //PACKING
                paramIn[24].Value = demogrpahic.Tot_num_of_exp_ind2;
                paramIn[25].Value = demogrpahic.Tot_Num_of_Pre_Pos_Still_Work2;
                paramIn[26].Value = demogrpahic.Tot_Num_of_Base_Pos_Ind2;
                paramIn[27].Value = demogrpahic.Tot_not_avai_for_testing2;
                paramIn[28].Value = demogrpahic.Tot_to_be_Test2;
                paramIn[29].Value = demogrpahic.Tot_New_pos_with_pri_nega_skin_test2;
                paramIn[30].Value = demogrpahic.Num_of_Skin_Test_Nega2;
                paramIn[31].Value = demogrpahic.Num_of_non_participants2;


                //DISTRIBUTE
                paramIn[32].Value = demogrpahic.Tot_num_of_exp_ind_distr;
                paramIn[33].Value = demogrpahic.Tot_Num_of_Pre_Pos_Still_Work_distr;
                paramIn[34].Value = demogrpahic.Tot_Num_of_Base_Pos_Ind_distr;
                paramIn[35].Value = demogrpahic.Tot_not_avai_for_testing_distr;
                paramIn[36].Value = demogrpahic.Tot_to_be_Test2_distr;
                paramIn[37].Value = demogrpahic.Tot_New_pos_with_pri_nega_skin_test_distr;
                paramIn[38].Value = demogrpahic.Num_of_Skin_Test_Nega_distr;
                paramIn[39].Value = demogrpahic.Num_of_non_participants_distr;


                //OTHERS
                paramIn[40].Value = demogrpahic.Tot_num_of_exp_ind_Other;
                paramIn[41].Value = demogrpahic.Tot_Num_of_Pre_Pos_Still_Work_Other;
                paramIn[42].Value = demogrpahic.Tot_Num_of_Base_Pos_Ind_Other;
                paramIn[43].Value = demogrpahic.Tot_not_avai_for_testing_Other;
                paramIn[44].Value = demogrpahic.Tot_to_be_Test2_Other;
                paramIn[45].Value = demogrpahic.Tot_New_pos_with_pri_nega_skin_test_Other;
                paramIn[46].Value = demogrpahic.Num_of_Skin_Test_Nega_Other;
                paramIn[47].Value = demogrpahic.Num_of_non_participants_Other;

                paramOut = new SqlParameter[1];
                paramOut[0] = new SqlParameter("@p_output", SqlDbType.VarChar, 50);


                if (objDBPool.SpQueryOutputParam("USP_INSERT_EnzymeDemographic_Campaign_Data", paramIn,ref paramOut,true,ref objDS))

                    tmp = true;
                else
                    tmp = false;

                strout = paramOut[0].Value.ToString();

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally { objDBPool = null; }
            return tmp;
        }

        public bool InsertCompaignDAL(DemographicInfo demogrpahic, ref string strout ,ref DataSet objDS)
        {
            bool tmp = false;
            try
            {
                paramIn = new SqlParameter[35];

                paramIn[0] = new SqlParameter("@P_EnzymeName_ID", SqlDbType.Int);
                paramIn[0].Value = demogrpahic.EnzymeID;

                // Making
                paramIn[1] = new SqlParameter("@P_Making_ExposedInd", SqlDbType.Int);
                paramIn[2] = new SqlParameter("@P_Making_PrePosWorking", SqlDbType.Int);
                paramIn[3] = new SqlParameter("@P_Making_totNumBaseline", SqlDbType.Int);
                paramIn[4] = new SqlParameter("@P_Making_TotalnotAvail", SqlDbType.Int);
                paramIn[5] = new SqlParameter("@P_Making_Totaltotested", SqlDbType.Int);
                paramIn[6] = new SqlParameter("@P_Making_SkinPos", SqlDbType.Int);
                paramIn[7] = new SqlParameter("@P_Making_SkinNeg", SqlDbType.Int);
                paramIn[8] = new SqlParameter("@P_Making_NonParticipants", SqlDbType.Int);

                //PACKING
                paramIn[9] = new SqlParameter("@P_Packing_ExposedInd", SqlDbType.Int);
                paramIn[10] = new SqlParameter("@P_Packing_PrePosWorking", SqlDbType.Int);
                paramIn[11] = new SqlParameter("@P_Packing_totNumBaseline", SqlDbType.Int);
                paramIn[12] = new SqlParameter("@P_Packing_TotalnotAvail", SqlDbType.Int);
                paramIn[13] = new SqlParameter("@P_Packing_Totaltotested", SqlDbType.Int);
                paramIn[14] = new SqlParameter("@P_Packing_SkinPos", SqlDbType.Int);
                paramIn[15] = new SqlParameter("@P_Packing_SkinNeg", SqlDbType.Int);
                paramIn[16] = new SqlParameter("@P_Packing_NonParticipants", SqlDbType.Int);

                //DISTRiBUTION
                paramIn[17] = new SqlParameter("@P_Distribution_ExposedInd", SqlDbType.Int);
                paramIn[18] = new SqlParameter("@P_Distribution_PrePosWorking", SqlDbType.Int);
                paramIn[19] = new SqlParameter("@P_Distribution_totNumBaseline", SqlDbType.Int);
                paramIn[20] = new SqlParameter("@P_Distribution_TotalnotAvail", SqlDbType.Int);
                paramIn[21] = new SqlParameter("@P_Distribution_Totaltotested", SqlDbType.Int);
                paramIn[22] = new SqlParameter("@P_Distribution_SkinPos", SqlDbType.Int);
                paramIn[23] = new SqlParameter("@P_Distribution_SkinNeg", SqlDbType.Int);
                paramIn[24] = new SqlParameter("@P_Distribution_NonParticipants", SqlDbType.Int);

                //OTHERS
                paramIn[25] = new SqlParameter("@P_Others_ExposedInd", SqlDbType.Int);
                paramIn[26] = new SqlParameter("@P_Others_PrePosWorking", SqlDbType.Int);
                paramIn[27] = new SqlParameter("@P_Other_totNumBaseline", SqlDbType.Int);
                paramIn[28] = new SqlParameter("@P_Others_TotalnotAvail", SqlDbType.Int);
                paramIn[29] = new SqlParameter("@P_Others_Totaltotested", SqlDbType.Int);
                paramIn[30] = new SqlParameter("@P_Others_SkinPos", SqlDbType.Int);
                paramIn[31] = new SqlParameter("@P_Others_SkinNeg", SqlDbType.VarChar);
                paramIn[32] = new SqlParameter("@P_Others_NonParticipants", SqlDbType.Int);

                paramIn[33] = new SqlParameter("@P_DemographicID", SqlDbType.Int);
                paramIn[33].Value = demogrpahic.DemographicID;

                paramIn[34] = new SqlParameter("@P_CreatedBy", SqlDbType.VarChar);
                paramIn[34].Value = "test";
                
                //MAKING
                paramIn[1].Value = demogrpahic.Total_num_of_Exposed_Individuals;
                paramIn[2].Value = demogrpahic.Total_num_of_prev_pos_Stillwork;
                paramIn[3].Value = demogrpahic.Tot_Num_f_Base_Pos_Ind;
                paramIn[4].Value = demogrpahic.Tot_not_avail_fr_test;
                paramIn[5].Value = demogrpahic.Tot_to_be_Testd;
                paramIn[6].Value = demogrpahic.Tot_New_pos_wth_prior_neg_skintest;
                paramIn[7].Value = demogrpahic.Num_of_Skin_TestNeg;
                paramIn[8].Value = demogrpahic.Num_of_non_part;

                //PACKING
                paramIn[9].Value = demogrpahic.Tot_num_of_exp_ind2;
                paramIn[10].Value = demogrpahic.Tot_Num_of_Pre_Pos_Still_Work2;
                paramIn[11].Value = demogrpahic.Tot_Num_of_Base_Pos_Ind2;
                paramIn[12].Value = demogrpahic.Tot_not_avai_for_testing2;
                paramIn[13].Value = demogrpahic.Tot_to_be_Test2;
                paramIn[14].Value = demogrpahic.Tot_New_pos_with_pri_nega_skin_test2;
                paramIn[15].Value = demogrpahic.Num_of_Skin_Test_Nega2;
                paramIn[16].Value = demogrpahic.Num_of_non_participants2;

                //DISTRIBUTE
                //DISTRIBUTE
                paramIn[17].Value = demogrpahic.Tot_num_of_exp_ind_distr;
                paramIn[18].Value = demogrpahic.Tot_Num_of_Pre_Pos_Still_Work_distr;
                paramIn[19].Value = demogrpahic.Tot_Num_of_Base_Pos_Ind_distr;
                paramIn[20].Value = demogrpahic.Tot_not_avai_for_testing_distr;
                paramIn[21].Value = demogrpahic.Tot_to_be_Test2_distr;
                paramIn[22].Value = demogrpahic.Tot_New_pos_with_pri_nega_skin_test_distr;
                paramIn[23].Value = demogrpahic.Num_of_Skin_Test_Nega_distr;
                paramIn[24].Value = demogrpahic.Num_of_non_participants_distr;


                //OTHERS
                paramIn[25].Value = demogrpahic.Tot_num_of_exp_ind_Other;
                paramIn[26].Value = demogrpahic.Tot_Num_of_Pre_Pos_Still_Work_Other;
                paramIn[27].Value = demogrpahic.Tot_Num_of_Base_Pos_Ind_Other;
                paramIn[28].Value = demogrpahic.Tot_not_avai_for_testing_Other;
                paramIn[29].Value = demogrpahic.Tot_to_be_Test2_Other;
                paramIn[30].Value = demogrpahic.Tot_New_pos_with_pri_nega_skin_test_Other;
                paramIn[31].Value = demogrpahic.Num_of_Skin_Test_Nega_Other;
                paramIn[32].Value = demogrpahic.Num_of_non_participants_Other;


                paramOut = new SqlParameter[1];
                paramOut[0] = new SqlParameter("@P_OUTPUT", SqlDbType.VarChar, 50);
                

                if (objDBPool.SpQueryOutputParam("USP_INSERT_EnzymeCampaign_Data", paramIn, ref paramOut, true))

                    tmp = true;
                else
                    tmp = false;
                strout = paramOut[0].Value.ToString();        
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally { objDBPool = null; }
            return tmp;
        }

        public bool UpdateDemographicDAL(DemographicInfo demogrpahic, bool IsOther,ref string strout, ref DataSet objDS)
        {
            bool tmp = false;
            try
            {
                paramIn = new SqlParameter[16];
                paramIn[0] = new SqlParameter("@P_DemographicID", SqlDbType.Int);
                paramIn[0].Value = demogrpahic.DemographicID;

                paramIn[1] = new SqlParameter("@P_Region_ID", SqlDbType.Int);
                paramIn[1].Value = demogrpahic.Region;

                paramIn[2] = new SqlParameter("@P_Country_ID", SqlDbType.Int);
                paramIn[2].Value = demogrpahic.Country;

                paramIn[3] = new SqlParameter("@P_SiteName_ID", SqlDbType.Int);
                paramIn[3].Value = demogrpahic.SiteName;

                paramIn[4] = new SqlParameter("@P_BusinessUnit_ID", SqlDbType.Int);
                paramIn[4].Value = demogrpahic.BusinessUnit;

                paramIn[5] = new SqlParameter("@P_Category_ID", SqlDbType.Int);
                paramIn[5].Value = demogrpahic.Category;

                paramIn[6] = new SqlParameter("@P_Sector_ID", SqlDbType.Int);
                paramIn[6].Value = demogrpahic.Sector;

                paramIn[7] = new SqlParameter("@P_Platform", SqlDbType.VarChar);
                paramIn[7].Value = demogrpahic.Platform;

                paramIn[8] = new SqlParameter("@P_EmploymentStatus", SqlDbType.VarChar);
                paramIn[8].Value = demogrpahic.EmpployeeStatus;

                paramIn[9] = new SqlParameter("@P_FiscalYear_ID", SqlDbType.Int);
                paramIn[9].Value = demogrpahic.FiscalYr;

                paramIn[10] = new SqlParameter("@P_Campaign", SqlDbType.VarChar);
                paramIn[10].Value = demogrpahic.Campaign;

                paramIn[11] = new SqlParameter("@P_TotalSitePopulation", SqlDbType.Int);
                paramIn[11].Value = demogrpahic.TotalSitePop;

                paramIn[12] = new SqlParameter("@P_NumberofIndividualsGrade1", SqlDbType.Int);
                paramIn[12].Value = demogrpahic.NumMedicalMoniProg;

                paramIn[13] = new SqlParameter("@P_PrincipalReporter_ID", SqlDbType.Int);
                paramIn[13].Value = demogrpahic.PrincipalReporter;

                paramIn[14] = new SqlParameter("@P_ModifiedBy", SqlDbType.VarChar);
                paramIn[14].Value = "TestU";

                paramIn[15] = new SqlParameter("@P_DemographicName", SqlDbType.VarChar);
                paramIn[15].Value = "";

                paramOut = new SqlParameter[1];

                paramOut[0] = new SqlParameter("@P_output", SqlDbType.VarChar, 50);
                paramOut[0].Direction = ParameterDirection.Output;

                if (objDBPool.SpQueryOutputParam("USP_UPDATE_EnzymeDemographicData", paramIn, ref paramOut, true, ref objDS))

                    tmp = true;
                else
                    tmp = false;

                strout = paramOut[0].Value.ToString();

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally { objDBPool = null; }
            return tmp;
        }

        public bool UpdateCompaignDAL(DemographicInfo demogrpahic, ref string strout, ref DataSet objDS)
        {
            bool tmp = false;
            try
            {
                paramIn = new SqlParameter[34];

                // Making

                paramIn[0] = new SqlParameter("@P_CampaignID", SqlDbType.Int);
                paramIn[0].Value = demogrpahic.CampaignID;

                paramIn[1] = new SqlParameter("@P_Making_ExposedInd", SqlDbType.Int);
                paramIn[2] = new SqlParameter("@P_Making_PrePosWorking", SqlDbType.Int);
                paramIn[3] = new SqlParameter("@P_Making_totNumBaseline", SqlDbType.Int);
                paramIn[4] = new SqlParameter("@P_Making_TotalnotAvail", SqlDbType.Int);
                paramIn[5] = new SqlParameter("@P_Making_Totaltotested", SqlDbType.Int);
                paramIn[6] = new SqlParameter("@P_Making_SkinPos", SqlDbType.Int);
                paramIn[7] = new SqlParameter("@P_Making_SkinNeg", SqlDbType.Int);
                paramIn[8] = new SqlParameter("@P_Making_NonParticipants", SqlDbType.Int);

                //PACKING
                paramIn[9] = new SqlParameter("@P_Packing_ExposedInd", SqlDbType.Int);
                paramIn[10] = new SqlParameter("@P_Packing_PrePosWorking", SqlDbType.Int);
                paramIn[11] = new SqlParameter("@P_Packing_totNumBaseline", SqlDbType.Int);
                paramIn[12] = new SqlParameter("@P_Packing_TotalnotAvail", SqlDbType.Int);
                paramIn[13] = new SqlParameter("@P_Packing_Totaltotested", SqlDbType.Int);
                paramIn[14] = new SqlParameter("@P_Packing_SkinPos", SqlDbType.Int);
                paramIn[15] = new SqlParameter("@P_Packing_SkinNeg", SqlDbType.Int);
                paramIn[16] = new SqlParameter("@P_Packing_NonParticipants", SqlDbType.Int);

                //DISTRiBUTION
                paramIn[17] = new SqlParameter("@P_Distribution_ExposedInd", SqlDbType.Int);
                paramIn[18] = new SqlParameter("@P_Distribution_PrePosWorking", SqlDbType.Int);
                paramIn[19] = new SqlParameter("@P_Distribution_totNumBaseline", SqlDbType.Int);
                paramIn[20] = new SqlParameter("@P_Distribution_TotalnotAvail", SqlDbType.Int);
                paramIn[21] = new SqlParameter("@P_Distribution_Totaltotested", SqlDbType.Int);
                paramIn[22] = new SqlParameter("@P_Distribution_SkinPos", SqlDbType.Int);
                paramIn[23] = new SqlParameter("@P_Distribution_SkinNeg", SqlDbType.Int);
                paramIn[24] = new SqlParameter("@P_Distribution_NonParticipants", SqlDbType.Int);

                //OTHERS
                paramIn[25] = new SqlParameter("@P_Others_ExposedInd", SqlDbType.Int);
                paramIn[26] = new SqlParameter("@P_Others_PrePosWorking", SqlDbType.Int);
                paramIn[27] = new SqlParameter("@P_Other_totNumBaseline", SqlDbType.Int);
                paramIn[28] = new SqlParameter("@P_Others_TotalnotAvail", SqlDbType.Int);
                paramIn[29] = new SqlParameter("@P_Others_Totaltotested", SqlDbType.Int);
                paramIn[30] = new SqlParameter("@P_Others_SkinPos", SqlDbType.Int);
                paramIn[31] = new SqlParameter("@P_Others_SkinNeg", SqlDbType.VarChar);
                paramIn[32] = new SqlParameter("@P_Others_NonParticipants", SqlDbType.Int);



                //MAKING
                paramIn[1].Value = demogrpahic.Total_num_of_Exposed_Individuals;
                paramIn[2].Value = demogrpahic.Total_num_of_prev_pos_Stillwork;
                paramIn[3].Value = demogrpahic.Tot_Num_f_Base_Pos_Ind;
                paramIn[4].Value = demogrpahic.Tot_not_avail_fr_test;
                paramIn[5].Value = demogrpahic.Tot_to_be_Testd;
                paramIn[6].Value = demogrpahic.Tot_New_pos_wth_prior_neg_skintest;
                paramIn[7].Value = demogrpahic.Num_of_Skin_TestNeg;
                paramIn[8].Value = demogrpahic.Num_of_non_part;

                //PACKING
                paramIn[9].Value = demogrpahic.Tot_num_of_exp_ind2;
                paramIn[10].Value = demogrpahic.Tot_Num_of_Pre_Pos_Still_Work2;
                paramIn[11].Value = demogrpahic.Tot_Num_of_Base_Pos_Ind2;
                paramIn[12].Value = demogrpahic.Tot_not_avai_for_testing2;
                paramIn[13].Value = demogrpahic.Tot_to_be_Test2;
                paramIn[14].Value = demogrpahic.Tot_New_pos_with_pri_nega_skin_test2;
                paramIn[15].Value = demogrpahic.Num_of_Skin_Test_Nega2;
                paramIn[16].Value = demogrpahic.Num_of_non_participants2;

                //DISTRIBUTE
                paramIn[17].Value = demogrpahic.Tot_num_of_exp_ind_distr;
                paramIn[18].Value = demogrpahic.Tot_Num_of_Pre_Pos_Still_Work_distr;
                paramIn[19].Value = demogrpahic.Tot_Num_of_Base_Pos_Ind_distr;
                paramIn[20].Value = demogrpahic.Tot_not_avai_for_testing_distr;
                paramIn[21].Value = demogrpahic.Tot_to_be_Test2_distr;
                paramIn[22].Value = demogrpahic.Tot_New_pos_with_pri_nega_skin_test_distr;
                paramIn[23].Value = demogrpahic.Num_of_Skin_Test_Nega_distr;
                paramIn[24].Value = demogrpahic.Num_of_non_participants_distr;


                //OTHERS
                paramIn[25].Value = demogrpahic.Tot_num_of_exp_ind_Other;
                paramIn[26].Value = demogrpahic.Tot_Num_of_Pre_Pos_Still_Work_Other;
                paramIn[27].Value = demogrpahic.Tot_Num_of_Base_Pos_Ind_Other;
                paramIn[28].Value = demogrpahic.Tot_not_avai_for_testing_Other;
                paramIn[29].Value = demogrpahic.Tot_to_be_Test2_Other;
                paramIn[30].Value = demogrpahic.Tot_New_pos_with_pri_nega_skin_test_Other;
                paramIn[31].Value = demogrpahic.Num_of_Skin_Test_Nega_Other;
                paramIn[32].Value = demogrpahic.Num_of_non_participants_Other;

                paramIn[33] = new SqlParameter("@P_ModifiedBy", SqlDbType.VarChar);
                paramIn[33].Value = "TempUser";

                paramOut = new SqlParameter[1];
                paramOut[0] = new SqlParameter("@P_output", SqlDbType.VarChar, 50);
                paramOut[0].Direction = ParameterDirection.Output;

                if (objDBPool.SpQueryOutputParam("USP_UPDATE_EnzymeCampaign_Data", paramIn, ref paramOut, true,ref objDS))
                    tmp = true;
                else
                    tmp = false;

                strout = paramOut[0].Value.ToString();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally { objDBPool = null; }
            return tmp;
        }

        public bool DeteteCampaignDAL(int ID, string ModifiedBy, ref string strout, ref DataSet objDS)
        {
            try
            {
                paramIn = new SqlParameter[2];
                paramIn[0] = new SqlParameter("@P_CampaignID", SqlDbType.Int);
                paramIn[0].Value = ID;

                paramIn[1] = new SqlParameter("@P_ModifiedBy", SqlDbType.NVarChar);
                paramIn[1].Value = ModifiedBy;

                paramOut = new SqlParameter[1];
                paramOut[0] = new SqlParameter("@p_output", SqlDbType.VarChar, 50);

                if (objDBPool.SpQueryOutputParam("USP_DELETE_EnzymeCampaign_Data", paramIn, ref paramOut,true,ref objDS))
                    return true;
                else return false;

                strout = paramOut[0].Value.ToString();
            }

            finally
            {
                objDBPool = null;
            }
        }

        public bool getEnzynmeDataDAL(ref DataSet objDS)
        {
            try
            {
                if (objDBPool.SpQueryDataset("USP_GET_Enzyme_Data", ref objDS))
                    return true;
                else return false;
            }

            finally
            {
                objDBPool = null;
            }
        }

        public bool UpdateEnzymeDataDAL(int Id, string Title, string CreatedBy, ref DataSet objDS)
        {
            try
            {
                paramIn = new SqlParameter[3];

                paramIn[0] = new SqlParameter("@p_Enzyme_ID ", SqlDbType.VarChar);
                paramIn[0].Value = Id;

                paramIn[1] = new SqlParameter("@P_EnzymeName", SqlDbType.VarChar);
                paramIn[1].Value = Title;

                paramIn[2] = new SqlParameter("@P_ModifiedBy", SqlDbType.VarChar);
                paramIn[2].Value = CreatedBy;

                //paramOut = new SqlParameter[1];
                //paramOut[0] = new SqlParameter("@P_Output", SqlDbType.VarChar, 50);
                //paramOut[0].Direction = ParameterDirection.Output;

                if (objDBPool.SpQueryDataset("USP_UPDATE_Enzyme_Data", paramIn, ref objDS))
                    return true;
                else return false;
            }

            finally
            {
                objDBPool = null;
            }
        }

        public bool DeleteEnzymeDataDAL(int Id, string CreatedBy, int outputparam)
        {
            try
            {
                paramIn = new SqlParameter[2];

                paramIn[0] = new SqlParameter("@P_EnzymeID", SqlDbType.VarChar);
                paramIn[0].Value = Id;


                paramIn[1] = new SqlParameter("@P_ModifiedBy", SqlDbType.VarChar);
                paramIn[1].Value = CreatedBy;


                //paramOut = new SqlParameter[1];
                //paramOut[0] = new SqlParameter("P_Output", SqlDbType.Int);
                //paramOut[0].Value = outputparam;

                //if (objDBPool.SpQueryOutputParam("USP_DELETE_Enzyme_Data", paramIn,ref paramOut,true))
                //    return true;

                if (objDBPool.SpQueryExecuteNonQuery("USP_DELETE_Enzyme_Data", paramIn))
                    return true;
                else return false;
            }

            finally
            {
                objDBPool = null;
            }
        }

        public bool InsertEnzymeDataDAL(string Title, string CreatedBy, int Outputparameter)
        {
            try
            {
                paramIn = new SqlParameter[2];

                paramIn[0] = new SqlParameter("@P_EnzymeName", SqlDbType.VarChar);
                paramIn[0].Value = Title;

                paramIn[1] = new SqlParameter("@P_CreatedBy", SqlDbType.VarChar);
                paramIn[1].Value = CreatedBy;

                paramOut = new SqlParameter[1];
                paramOut[0] = new SqlParameter("P_Output", SqlDbType.VarChar,50);

                paramOut[0].Value = "s";

                if (objDBPool.SpQueryOutputParam("USP_INSERT_Enzyme_Data", paramIn, ref paramOut, true))
                    return true;
                else return false;
            }

            finally
            {
                objDBPool = null;
            }
        }

        public bool DeteteDemographicDetailsDAL(int ID, string ModifiedBy, ref DataSet objDS)
        {
            try
            {
                paramIn = new SqlParameter[2];
                paramIn[0] = new SqlParameter("@P_DemographicID", SqlDbType.Int);
                paramIn[0].Value = ID;
                paramIn[1] = new SqlParameter("@P_ModifiedBy", SqlDbType.NVarChar);
                paramIn[1].Value = ModifiedBy;

                paramOut = new SqlParameter[1];
                paramOut[0] = new SqlParameter("@p_output", SqlDbType.VarChar, 50);

                if (objDBPool.SpQueryOutputParam("USP_DELETE_EnzymeDemographic_Campaign_Data", paramIn, ref paramOut,true))
                    return true;
                else return false;
            }

            finally
            {
                objDBPool = null;
            }
        }

        public bool getFiscalYrDAL(ref DataSet objDS)
        {
            try
            {
                if (objDBPool.SpQueryDataset("USP_GET_FiscalYear", ref objDS))
                    return true;
                else return false;
            }

            finally
            {
                objDBPool = null;
            }
        }
    }
}


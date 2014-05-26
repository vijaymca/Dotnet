using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnzymeEntities.Entity
{
    public class DemographicInfo
    {
        //Demographic info
        public int Region { get; set; }
        public int Country { get; set; }
        public int SiteName { get; set; }
        public int BusinessUnit { get; set; }
        public int Category { get; set; }
        public int Sector { get; set; }
        public string Platform { get; set; }
        public string EmpployeeStatus { get; set; }
        public int FiscalYr { get; set; }
        public string Campaign { get; set; }
        public int TotalSitePop { get; set; }
        public int NumMedicalMoniProg{ get; set; }
        public int PrincipalReporter { get; set; }
        public int DemographicID { get; set; }
        
        //Compaign info
        public int CampaignID { get; set; }
        public string EnzymeName { get; set; }
        public int EnzymeID { get; set; }

        //PACKING
        public int Total_num_of_Exposed_Individuals { get; set; }
        public int Total_num_of_prev_pos_Stillwork{ get; set; }
        public int Tot_Num_f_Base_Pos_Ind { get; set; }
        public int Tot_not_avail_fr_test { get; set; }
        public int Tot_to_be_Testd { get; set; }
        public int Tot_New_pos_wth_prior_neg_skintest { get; set; }
        public int Num_of_Skin_TestNeg { get; set; }
        public int Num_of_non_part  { get; set; }

        //MAKING
        public int Tot_num_of_exp_ind2  { get; set; }
        public int Tot_Num_of_Pre_Pos_Still_Work2 { get; set; }
        public int Tot_Num_of_Base_Pos_Ind2 { get; set; }
        public int Tot_not_avai_for_testing2  { get; set; }
        public int Tot_to_be_Test2 { get; set; }
        public int Tot_New_pos_with_pri_nega_skin_test2  { get; set; }
        public int Num_of_Skin_Test_Nega2  { get; set; }
        public int Num_of_non_participants2 { get; set; }

        //DISTRIBUTION
        public int Tot_num_of_exp_ind_distr { get; set; }
        public int Tot_Num_of_Pre_Pos_Still_Work_distr { get; set; }
        public int Tot_Num_of_Base_Pos_Ind_distr { get; set; }
        public int Tot_not_avai_for_testing_distr { get; set; }
        public int Tot_to_be_Test2_distr { get; set; }
        public int Tot_New_pos_with_pri_nega_skin_test_distr { get; set; }
        public int Num_of_Skin_Test_Nega_distr { get; set; }
        public int Num_of_non_participants_distr { get; set; }
                
        //OTHERS
        public int Tot_num_of_exp_ind_Other { get; set; }
        public int Tot_Num_of_Pre_Pos_Still_Work_Other { get; set; }
        public int Tot_Num_of_Base_Pos_Ind_Other { get; set; }
        public int Tot_not_avai_for_testing_Other { get; set; }
        public int Tot_to_be_Test2_Other { get; set; }
        public int Tot_New_pos_with_pri_nega_skin_test_Other { get; set; }
        public int Num_of_Skin_Test_Nega_Other { get; set; }
        public int Num_of_non_participants_Other { get; set; }

    }
}

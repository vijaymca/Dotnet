

var Total_number_of_exposed_individuals = null;
var Total_Number_of_Previous_Positives_Still_Working = null;
var Total_Number_of_Baseline_Positive_Individuals = null;
var Total_not_available_for_testing = null;

$(document).ready(function () {


    $("#PlaceHolderMain_txtMedicalMonitor_I").keypress(function (e) {
        if (String.fromCharCode(e.keyCode).match(/[^0-9]/g)) return false;
    });


    $("#PlaceHolderMain_txtTotalSitePop_I").keypress(function (e) {
        if (String.fromCharCode(e.keyCode).match(/[^0-9]/g)) return false;
    });



    $("#PlaceHolderMain_pnlCompaign input[type='text']").each(
	function () {
	    $(this).keypress(function (e) {
	        if (String.fromCharCode(e.keyCode).match(/[^0-9]/g))
	            return false;
	    });
	   // $(this).val('0');
	});

    //Medical in Demographic
    $('#PlaceHolderMain_txtTotalSitePop_I').focusout(function () {
        chkMedical();
    });

    $('#PlaceHolderMain_txtMedicalMonitor_I').focusout(function () {
        chkMedical();
    });


    //MAKING
    $('#PlaceHolderMain_pnlCompaign_txtTotalnumberofexposedindividuals_I').focusout(function () {
        checkMaking();
    });

    $('#PlaceHolderMain_pnlCompaign_txtTotalNumberofPreviousPositivesStillWorking_I').focusout(function () {
        checkMaking();
    });

    $('#PlaceHolderMain_pnlCompaign_txtTot_Num_Basline_Pos_Ind_I').focusout(function () {
        checkMaking();
    });

    $('#PlaceHolderMain_pnlCompaign_txtTot_Ava_for_testing_I').focusout(function () {
        checkMaking();
    });

    $('#PlaceHolderMain_pnlCompaign_txt_Tot_new_pos_wit_prior_nega_skin_tes_I').focusout(function () {
        checkMaking();
    });

    $('#PlaceHolderMain_pnlCompaign_txt_num_skin_test_neg_I').focusout(function () {
        checkMaking();
    });


    //PACKING
    $('#PlaceHolderMain_pnlCompaign_txt2Tot_num_f_exp_ind_I').focusout(function () {
        chkPacking();
    });

    $('#PlaceHolderMain_pnlCompaign_txt2_tot_num_f_pre_pos_sti_wor_I').focusout(function () {
        chkPacking();
    });

    $('#PlaceHolderMain_pnlCompaign_txt2_num_f_bas_pos_ind_I').focusout(function () {
        chkPacking();
    });

    $('#PlaceHolderMain_pnlCompaign_txt2_tot_avai_f_test_I').focusout(function () {
        chkPacking();
    });

    $('#PlaceHolderMain_pnlCompaign_txt2_tot_new_pos_wt_pri_neg_skintest_I').focusout(function () {
        chkPacking();
    });

    $('#PlaceHolderMain_pnlCompaign_txt2num_f_ski_tes_neg_I').focusout(function () {
        chkPacking();
    });


    //DISTRIBUTE
    $('#PlaceHolderMain_pnlCompaign_txt_tot_num_of_exp_ind_distr_I').focusout(function () {
        chkDistribute();
    });

    $('#PlaceHolderMain_pnlCompaign_txt_tot_num_pre_pos_st_wrk_distr_I').focusout(function () {
        chkDistribute();
    });

    $('#PlaceHolderMain_pnlCompaign_txt_tot_num_of_base_pos_ind_distr_I').focusout(function () {
        chkDistribute();
    });

    $('#PlaceHolderMain_pnlCompaign_txt_tot_not_ava_for_test_distr_I').focusout(function () {
        chkDistribute();
    });

    $('#PlaceHolderMain_pnlCompaign_txt_tot_new_pos_wit_pri_neg_skin_test_distr_I').focusout(function () {
        chkDistribute();
    });

    $('#PlaceHolderMain_pnlCompaign_txt_num_of_skin_test_neg_distr_I').focusout(function () {
        chkDistribute();
    });


    //OTHER
    $('#PlaceHolderMain_pnlCompaign_txt_tot_num_of_exp_ind_Other_I').focusout(function () {
        chkOther();
    });

    $('#PlaceHolderMain_pnlCompaign_tot_num_of_pre_pos_stil_work_other_I').focusout(function () {
        chkOther();
    });

    $('#PlaceHolderMain_pnlCompaign_txt_tot_num_of_base_pos_ind_other_I').focusout(function () {
        chkOther();
    });

    $('#PlaceHolderMain_pnlCompaign_txt_tot_not_avai_for_test_Other_I').focusout(function () {
        chkOther();
    });

    $('#PlaceHolderMain_pnlCompaign_txt_tot_new_pos_wit_pri_neg_ski_test_Other_I').focusout(function () {
        chkOther();
    });

    $('#PlaceHolderMain_pnlCompaign_txt_num_of_ski_neg_Other_I').focusout(function () {
        chkOther();
    });

});

//check
function chkMedical() {
    // alert('Medical');
    var TotalSitePopulation = parseInt($('#PlaceHolderMain_txtTotalSitePop_I').val());
    var Grade1MedicalMonitor = parseInt($('#PlaceHolderMain_txtMedicalMonitor_I').val());

    if (TotalSitePopulation < Grade1MedicalMonitor) {
        alert('Number of Individuals In Grade 1 Medical Monitoring Program can not exceed Total Site Population');

        $('#PlaceHolderMain_txtMedicalMonitor_I').focus();
    }
}


function checkMaking() {
    Total_number_of_exposed_individuals = $('#PlaceHolderMain_pnlCompaign_txtTotalnumberofexposedindividuals_I').val();
    Total_Number_of_Previous_Positives_Still_Working = $('#PlaceHolderMain_pnlCompaign_txtTotalNumberofPreviousPositivesStillWorking_I').val();
    Total_Number_of_Baseline_Positive_Individuals = $('#PlaceHolderMain_pnlCompaign_txtTot_Num_Basline_Pos_Ind_I').val();
    Total_not_available_for_testing = $('#PlaceHolderMain_pnlCompaign_txtTot_Ava_for_testing_I').val();

    var Total_to_be_tested = Total_number_of_exposed_individuals - Total_Number_of_Previous_Positives_Still_Working - Total_Number_of_Baseline_Positive_Individuals - Total_not_available_for_testing;
    $('#PlaceHolderMain_pnlCompaign_txtTot_tobe_Tested_I').val(Total_to_be_tested);

    if (Total_to_be_tested < 0) {
        alert('MAKING:Total to be Tested should not be less than 0.');
    }

    var Tot_New_pos_with_pri_neg_skin_test = $('#PlaceHolderMain_pnlCompaign_txt_Tot_new_pos_wit_prior_nega_skin_tes_I').val();
    var Num_of_Skin_Test_Neg = $('#PlaceHolderMain_pnlCompaign_txt_num_skin_test_neg_I').val();
    var non_participants = parseInt(Total_to_be_tested) - parseInt(Num_of_Skin_Test_Neg) - parseInt(Tot_New_pos_with_pri_neg_skin_test);

    $('#PlaceHolderMain_pnlCompaign_txtNum_non_participants_I').val(non_participants);

    if (non_participants < 0) {
        alert('MAKING:Number of non-participants (available but not tested) should not be less than 0');
    }
}


function chkPacking() {
    var tot_num_of_expo_ind = $('#PlaceHolderMain_pnlCompaign_txt2Tot_num_f_exp_ind_I').val();
    var tot_num_of_Prev_Sti_work = $('#PlaceHolderMain_pnlCompaign_txt2_tot_num_f_pre_pos_sti_wor_I').val();
    var tot_num_of_base_pos_ind = $('#PlaceHolderMain_pnlCompaign_txt2_num_f_bas_pos_ind_I').val();
    var tot_nt_ava_for_test = $('#PlaceHolderMain_pnlCompaign_txt2_tot_avai_f_test_I').val();

    var tot_to_be_testOther = tot_num_of_expo_ind - tot_num_of_Prev_Sti_work - tot_num_of_base_pos_ind - tot_nt_ava_for_test;
    $('#PlaceHolderMain_pnlCompaign_txt2_tot_tob_test_I').val(tot_to_be_testOther);

    if (tot_to_be_testOther < 0) {
        alert('PACKING:Total to be Tested should not be less than 0.');
    }

    var Tot_New_pos_with_prior_neg_skin_test = $('#PlaceHolderMain_pnlCompaign_txt2_tot_new_pos_wt_pri_neg_skintest_I').val();
    var Num_of_Skin_Test_Nega = $('#PlaceHolderMain_pnlCompaign_txt2num_f_ski_tes_neg_I').val();
    var non_participants_Packing = parseInt(tot_to_be_testOther) - parseInt(Tot_New_pos_with_prior_neg_skin_test) - parseInt(Num_of_Skin_Test_Nega);

    $('#PlaceHolderMain_pnlCompaign_txt2num_f_non_participants_I').val(non_participants_Packing);

    if (non_participants_Packing < 0) {
        alert('PACKING:Number of non-participants (available but not tested) should not be less than 0');
    }
}


function chkDistribute() {
    var tot_num_of_expo_ind = $('#PlaceHolderMain_pnlCompaign_txt_tot_num_of_exp_ind_distr_I').val();
    var tot_num_of_Prev_Sti_work = $('#PlaceHolderMain_pnlCompaign_txt_tot_num_pre_pos_st_wrk_distr_I').val();
    var tot_num_of_base_pos_ind = $('#PlaceHolderMain_pnlCompaign_txt_tot_num_of_base_pos_ind_distr_I').val();
    var tot_nt_ava_for_test = $('#PlaceHolderMain_pnlCompaign_txt_tot_not_ava_for_test_distr_I').val();


    var tot_to_be_testDistr = tot_num_of_expo_ind - tot_num_of_Prev_Sti_work - tot_num_of_base_pos_ind - tot_nt_ava_for_test;
    $('#PlaceHolderMain_pnlCompaign_txt_tot_to_be_test_distr_I').val(tot_to_be_testDistr);

    if (tot_to_be_testDistr < 0) {
        alert('DISTRUBUTION:Total to be Tested should not be less than 0.');
    }

    var Tot_New_pos_with_prior_neg_skin_test = $('#PlaceHolderMain_pnlCompaign_txt_tot_new_pos_wit_pri_neg_skin_test_distr_I').val();
    var Num_of_Skin_Test_Nega = $('#PlaceHolderMain_pnlCompaign_txt_num_of_skin_test_neg_distr_I').val();
    var non_participants_Packing = parseInt(tot_to_be_testDistr) - parseInt(Tot_New_pos_with_prior_neg_skin_test) - parseInt(Num_of_Skin_Test_Nega);
    $('#PlaceHolderMain_pnlCompaign_txt_num_of_non_part_distr_I').val(non_participants_Packing);

    if (non_participants_Packing < 0) {
        alert('DISTRIBUTION:Number of non-participants (available but not tested) should not be less than 0');
    }
}


function chkOther() {

    var tot_num_of_expo_ind = $('#PlaceHolderMain_pnlCompaign_txt_tot_num_of_exp_ind_Other_I').val();
    var tot_num_of_Prev_Sti_work = $('#PlaceHolderMain_pnlCompaign_tot_num_of_pre_pos_stil_work_other_I').val();
    var tot_num_of_base_pos_ind = $('#PlaceHolderMain_pnlCompaign_txt_tot_num_of_base_pos_ind_other_I').val();
    var tot_nt_ava_for_test = $('#PlaceHolderMain_pnlCompaign_txt_tot_not_avai_for_test_Other_I').val();


    var tot_to_be_testOther = tot_num_of_expo_ind - tot_num_of_Prev_Sti_work - tot_num_of_base_pos_ind - tot_nt_ava_for_test;
    $('#PlaceHolderMain_pnlCompaign_txt_tot_to_be_test_Other_I').val(tot_to_be_testOther);

    if (tot_to_be_testOther < 0) {
        alert('OTHER:Total to be Tested should not be less than 0.');
    }

    var Tot_New_pos_with_prior_neg_skin_test = $('#PlaceHolderMain_pnlCompaign_txt_tot_new_pos_wit_pri_neg_ski_test_Other_I').val();
    var Num_of_Skin_Test_Nega = $('#PlaceHolderMain_pnlCompaign_txt_num_of_ski_neg_Other_I').val();
    var non_participants_other = parseInt(tot_to_be_testOther) - parseInt(Tot_New_pos_with_prior_neg_skin_test) - parseInt(Num_of_Skin_Test_Nega);
    $('#PlaceHolderMain_pnlCompaign_txt_num_non_parti_other_I').val(non_participants_other);
    
    if (tot_to_be_testOther < 0) {
        alert('OTHER:Number of non-participants (available but not tested) should not be less than 0');
    }
}




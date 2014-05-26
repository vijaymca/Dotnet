<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/EnzymeMain.Master"
    AutoEventWireup="true" CodeBehind="NewDemographicData.aspx.cs" Inherits="EnzymeWeb.NewDemographicData" %>

<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../Scripts/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src="../Scripts/clientEvents.js"></script>
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderMain" runat="server">

    <div style="background-color: White">
        <table width="100%">
            <tr>
                <td colspan="2" class="enHeader">
                    Demographic Information:
                </td>
            </tr>
            <tr>
                <td>
                    Region:
                </td>
                <td>
                    <dx:ASPxComboBox ID="drpRegion" runat="server" TextField="Region_Name" ValueField="Region_ID"
                        AutoPostBack="True" OnSelectedIndexChanged="drpRegion_SelectedIndexChanged">
                    </dx:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    Country:
                </td>
                <td>
                    <dx:ASPxComboBox ID="drpCountry" runat="server" TextField="Country_Name" ValueField="Country_ID"
                        AutoPostBack="True" OnSelectedIndexChanged="drpCountry_SelectedIndexChanged">
                    </dx:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    Site Name
                </td>
                <td>
                    <dx:ASPxComboBox ID="drpSiteName" runat="server" TextField="SiteName" ValueField="Site_ID"
                        AutoPostBack="True" OnSelectedIndexChanged="drpSiteName_SelectedIndexChanged">
                    </dx:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    Business Unit:
                </td>
                <td>
                    <dx:ASPxLabel ID="lblBusinessUnit" runat="server" Text="">
                    </dx:ASPxLabel>
                    <asp:HiddenField ID="hdnBusinessUnit" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Category:
                </td>
                <td>
                    <dx:ASPxLabel ID="lblCategory" runat="server" Text="">
                    </dx:ASPxLabel>
                    <asp:HiddenField ID="hdnCategory" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Sector:
                </td>
                <td>
                    <dx:ASPxLabel ID="lblSector" runat="server" Text="">
                    </dx:ASPxLabel>
                    <asp:HiddenField ID="hdnSector" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="auto-style1">
                    Platform:
                </td>
                <td class="auto-style1">
                    <table>
                        <tr>
                            <td>
                                <dx:ASPxRadioButtonList ID="rdPlatform" runat="server" ValueType="System.String" AutoPostBack="true"
                                   RepeatDirection="Horizontal" OnSelectedIndexChanged="rdPlatform_SelectedIndexChanged" ClientSideEvents-SelectedIndexChanged="clsevent">
                                    <Items>
                                        <dx:ListEditItem Text="Liquid" Value="0" />
                                        <dx:ListEditItem Text="Granules" Value="1" />
                                        <dx:ListEditItem Text="Soluble Unit Dose [Pods]" Value="2" />
                                        <dx:ListEditItem Text="Other (includes Distribution Module)" Value="3" />
                                    </Items>
                                </dx:ASPxRadioButtonList>
                            </td>
                            <td>
                                <dx:ASPxTextBox Visible="false" ID="txtOther" runat="server" Width="170px">
                                </dx:ASPxTextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    Employment Status:
                </td>
                <td>
                    <dx:ASPxRadioButtonList runat="server" ValueType="System.String" ID="rdEmpStatus"
                        RepeatDirection="Horizontal">
                        <Items>
                            <dx:ListEditItem Text="PG Employees" Value="PG Employees" />
                            <dx:ListEditItem Text="Contractors" Value="Contractors" />
                        </Items>
                    </dx:ASPxRadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    Fiscal Year:
                </td>
                <td>
                    <dx:ASPxComboBox TextField="FiscalYear" ValueField="FiscalYear_ID" ID="drpFiscalYear" runat="server" SelectedIndex="0">
                    </dx:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    Campaign:
                </td>
                <td>
                    <dx:ASPxRadioButtonList RepeatDirection="Horizontal" ID="rdCompaign" runat="server"
                        ValueType="System.String">
                        <Items>
                            <dx:ListEditItem Text="Spring" Value="0" />
                            <dx:ListEditItem Text="Fall" Value="1" />
                            <dx:ListEditItem Text="Annual" Value="2" />
                            <dx:ListEditItem Text="Special" Value="3" />
                        </Items>
                    </dx:ASPxRadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    Total Site Population:
                </td>
                <td>
                    <dx:ASPxTextBox ID="txtTotalSitePop" runat="server" Width="170px">
                    
                    </dx:ASPxTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Number Individuals In Grade 1 Medical Monitoring Program:
                </td>
                <td>
                    <dx:ASPxTextBox ID="txtMedicalMonitor" runat="server" Width="170px">
                    </dx:ASPxTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Principal Reporter:
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <dx:ASPxTextBox ID="txtReporter" runat="server" Width="170px">
                                </dx:ASPxTextBox>
                                <asp:HiddenField ID="hdnReporter" runat="server" />
                            </td>
                            <td>
                                <dx:ASPxButton ID="btnPersonalListing" runat="server" Text="Personnel Listing">
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table width="100%">
                        <tr>
                            <td class="enHeader">
                                Campaign Information:
                                <div style="float:right;width:20%">
                                <dx:ASPxButton ID="btnAddEnzyme" runat="server" Text="Add another Antigen" OnClick="btnAddEnzyme_Click">
                                </dx:ASPxButton>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <dx:ASPxGridView ID="grdEnzymes" ClientInstanceName="grdEnzymes" runat="server" KeyFieldName="CampaignID"
                                    Width="100%" AutoGenerateColumns="False" OnStartRowEditing="RowEditing" OnRowDeleting="OnRowDeleting"
                                    OnRowCommand="rowcommand">
                                    <Columns>
                                   
                                        <dx:GridViewDataColumn VisibleIndex="4" Name="Edit" Caption="Edit">
                                            <DataItemTemplate>
                                                <dx:ASPxButton ID="btnEdit" OnClick="btnEdit_Click" runat="server" AutoPostBack="false"
                                                    CausesValidation="false" ToolTip="Edit" EnableDefaultAppearance="false" Image-Url="~/styles/Images/edit_icon.gif">
                                                </dx:ASPxButton>
                                            </DataItemTemplate>
                                            <HeaderStyle Wrap="True" HorizontalAlign="Center" />
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewCommandColumn ShowDeleteButton="true" VisibleIndex="5" />
                                        <dx:GridViewDataColumn FieldName="CampaignID" VisibleIndex="0" />
                                        <dx:GridViewDataColumn FieldName="EnzymeName" VisibleIndex="1" />
                                        <dx:GridViewDataColumn FieldName="CreatedBy" VisibleIndex="2" />
                                        <dx:GridViewDataColumn FieldName="CreatedDate" VisibleIndex="3" />
                                    </Columns>
                                </dx:ASPxGridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <dx:ASPxPanel ID="pnlCompaign" runat="server" Visible="false">
                        <PanelCollection>
                            <dx:PanelContent>
                                <table>
                                    <tr>
                                        <th>
                                            Current Campaign
                                        </th>
                                    </tr>
                                    <tr>
                                        <td>
                                            Antigen Number and Enzyme Name
                                        </td>
                                        <td colspan="3">
                                            <dx:ASPxComboBox SelectedIndex="0" ID="drpEnzyme" Width="300" TextField="EnzymeName" ValueField="Enzyme_ID"
                                                runat="server" ValueType="System.String">
                                            </dx:ASPxComboBox>
                                            <dx:ASPxLabel ID="lblEnzyme" Visible="false" runat="server" Text="ASPxLabel">
                                            </dx:ASPxLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            Module:
                                        </th>
                                        <th>
                                            MAKING
                                        </th>
                                        <th>
                                            DISTRIBUTION
                                        </th>
                                    </tr>
                                    <tr>
                                        <td>
                                            Total number of exposed individuals
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" Increment="1" ID="txtTotalnumberofexposedindividuals" runat="server"
                                                Width="170px" Number="0">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" Increment="1" ID="txt_tot_num_of_exp_ind_distr" runat="server" Width="170px" Number="0">
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Total Number of Previous Positives Still Working
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" Number="0" ID="txtTotalNumberofPreviousPositivesStillWorking" runat="server"
                                                Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" Number="0" ID="txt_tot_num_pre_pos_st_wrk_distr" runat="server" Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Total Number of Baseline Positive Individuals
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" Number="0" ID="txtTot_Num_Basline_Pos_Ind" runat="server" Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" Number="0" ID="txt_tot_num_of_base_pos_ind_distr" runat="server" Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Total not available for testing
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" Number="0" ID="txtTot_Ava_for_testing" runat="server" Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" Number="0" ID="txt_tot_not_ava_for_test_distr" runat="server" Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Total to be Tested:
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" ReadOnly="true" Number="0" ID="txtTot_tobe_Tested"
                                                runat="server" Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" ReadOnly="true" Number="0" ID="txt_tot_to_be_test_distr" runat="server"
                                                Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Total New positives with prior negative skin test
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" Number="0" ID="txt_Tot_new_pos_wit_prior_nega_skin_tes" runat="server"
                                                Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" Number="0" ID="txt_tot_new_pos_wit_pri_neg_skin_test_distr" runat="server" Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Number of Skin Test Negatives
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" Number="0" ID="txt_num_skin_test_neg" runat="server" Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" Number="0" ID="txt_num_of_skin_test_neg_distr" runat="server" Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Number of non-participants (available but not tested)
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" ReadOnly="true" Number="0" ID="txtNum_non_participants"
                                                runat="server" Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" ReadOnly="true" Number="0" ID="txt_num_of_non_part_distr" runat="server"
                                                Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                      Module:
                                        </th>
                                        <th>PACKING</th>
                                        <th>OTHERS</th>
                                    </tr>
                                    <tr>
                                        <td>
                                            Total number of exposed individuals
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" MinValue="0" Number="0" ID="txt2Tot_num_f_exp_ind" runat="server"
                                                Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" MinValue="0" Number="0" ID="txt_tot_num_of_exp_ind_Other" runat="server" Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Total Number of Previous Positives Still Working
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" MinValue="0" Number="0" ID="txt2_tot_num_f_pre_pos_sti_wor" runat="server"
                                                Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" MinValue="0" Number="0" ID="tot_num_of_pre_pos_stil_work_other" runat="server" Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Total Number of Baseline Positive Individuals
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" MinValue="0" Number="0" ID="txt2_num_f_bas_pos_ind" runat="server"
                                                Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" MinValue="0" Number="0" ID="txt_tot_num_of_base_pos_ind_other" runat="server" Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Total not available for testing
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" MinValue="0" Number="0" ID="txt2_tot_avai_f_test" runat="server"
                                                Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" MinValue="0" Number="0" ID="txt_tot_not_avai_for_test_Other" runat="server" Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Total to be Tested:
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" MinValue="0" ReadOnly="true" Number="0" ID="txt2_tot_tob_test"
                                                runat="server" Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" MinValue="0" ReadOnly="true" Number="0" ID="txt_tot_to_be_test_Other"
                                                runat="server" Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Total New positives with prior negative skin test
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" MinValue="0" Number="0" ID="txt2_tot_new_pos_wt_pri_neg_skintest"
                                                runat="server" Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" MinValue="0" Number="0" ID="txt_tot_new_pos_wit_pri_neg_ski_test_Other" runat="server" Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Number of Skin Test Negatives
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" MinValue="0" Number="0" ID="txt2num_f_ski_tes_neg" runat="server"
                                                Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" MinValue="0" Number="0" ID="txt_num_of_ski_neg_Other" runat="server" Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Number of non-participants (available but not tested)
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" MinValue="0" ReadOnly="true" Number="0" ID="txt2num_f_non_participants"
                                                runat="server" Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" MinValue="0" ReadOnly="true" Number="0" ID="txt_num_non_parti_other"
                                                runat="server" Width="170px" ValidationSettings-ValidateOnLeave="true" >
                                            </dx:ASPxTextBox>
                                            
                                        </td>
                                    </tr>
                                </table>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxPanel>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <table>
                        <tr>
                            <td>
                                <dx:ASPxButton ID="btnSaveExit" runat="server" Text="Save & Exit" OnClick="btnSaveExit_Click">
                                </dx:ASPxButton>
                            </td>
                            <td>
                                <dx:ASPxButton ID="btnSaveNew" runat="server" Text="Save & Create New" 
                                    onclick="btnSaveNew_Click">
                                </dx:ASPxButton>
                            </td>
                            <td>
                                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" 
                                    onclick="btnCancel_Click">
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div style="font-weight:bold;padding:20px 20px 100px 20px;text-align:center">
            <dx:ASPxLabel ID="lblReturnStatus" runat="server" Text="">
            </dx:ASPxLabel>
        </div>
    </div>
    <script type="text/javascript" src="../Scripts/validations.js"></script>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/EnzymeMain.Master"
    AutoEventWireup="true" CodeBehind="NewDemographicData.aspx.cs" Inherits="EnzymeWeb.NewDemographicData" %>

<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../Scripts/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src="../Scripts/clientEvents.js"></script>
    <script type="text/javascript">
        function EnzymesPersonsLookup(s, e) {
            try {
                var varPrincipalReporter;
                var splitKEY;
                varPrincipalReporter = window.showModalDialog('PersonnelListingNew.aspx?ref=enZymes', 'Personnel Listing', 'status:no; dialogWidth:800px; dialogHeight:550px; help:no; scroll:Yes; menubar:no; resizable:No; Maximize:No');
                if (varPrincipalReporter == null || varPrincipalReporter == undefined)
                    varPrincipalReporter = window.returnValue;
                if (varPrincipalReporter != null) {
                    splitKEY = varPrincipalReporter.split("~");
                    txtReporter.SetText(splitKEY[1]);
                    PersonalListingCallBack.PerformCallback(splitKEY[0]);
                } else {
                    txtBioinformatics.SetText("");
                    PersonalListingCallBack.PerformCallback(0);
                }
            }
            catch (e) {
                alert("Exception caught in EnzymesPersonsLookup method");
                return false;
            }
        }

	
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div style="background-color: White">
        <table width="100%" style="border-top-color: #edca84; border-right-color: #edca84;
            border-bottom-color: #edca84; border-left-color: #edca84; border-top-width: 1px;
            border-right-width: 1px; border-bottom-width: 1px; border-left-width: 1px; border-top-style: solid;
            border-right-style: solid; border-bottom-style: solid; border-left-style: solid;">
            <tr>
                <td colspan="2" class="enHeader">
                    Demographic Information:
                </td>
            </tr>
            <tr>
                <td>
                    Region <span class="clsmandtr">*</span>
                </td>
                <td>
                    <dx:ASPxComboBox ID="drpRegion" runat="server" TextField="Region_Name" ValueField="Region_ID"
                        EnableIncrementalFiltering="False" IncrementalFilteringMode="StartsWith" AutoPostBack="True"
                        OnSelectedIndexChanged="drpRegion_SelectedIndexChanged" NullText="- Select Region -" >
                        <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="valStudyInfo">
                            <RequiredField IsRequired="True" ErrorText="Please select Region" />
                            <RequiredField IsRequired="True"></RequiredField>
                        </ValidationSettings>
                    </dx:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    Country <span class="clsmandtr">*</span>
                </td>
                <td>
                    <dx:ASPxComboBox ID="drpCountry" runat="server" TextField="Country_Name" ValueField="Country_ID"
                        IncrementalFilteringMode="StartsWith" EnableIncrementalFiltering="False" AutoPostBack="True"
                        OnSelectedIndexChanged="drpCountry_SelectedIndexChanged" NullText="- Select Country-">
                        <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="valStudyInfo">
                            <RequiredField IsRequired="True" ErrorText="Please select Country" />
                            <RequiredField IsRequired="True"></RequiredField>
                        </ValidationSettings>
                    </dx:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    Site Name <span class="clsmandtr">*</span>
                </td>
                <td>
                    <dx:ASPxComboBox ID="drpSiteName" runat="server" TextField="SiteName" ValueField="Site_ID"
                        IncrementalFilteringMode="StartsWith" EnableIncrementalFiltering="False" AutoPostBack="True"
                        OnSelectedIndexChanged="drpSiteName_SelectedIndexChanged" NullText="- Select Site-">
                        <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="valStudyInfo">
                            <RequiredField IsRequired="True" ErrorText="Please select Site Name" />
                            <RequiredField IsRequired="True"></RequiredField>
                        </ValidationSettings>
                    </dx:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    Business Unit
                </td>
                <td>
                    <dx:ASPxLabel ID="lblBusinessUnit" runat="server" Text="">
                    </dx:ASPxLabel>
                    <asp:HiddenField ID="hdnBusinessUnit" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Category
                </td>
                <td>
                    <dx:ASPxLabel ID="lblCategory" runat="server" Text="">
                    </dx:ASPxLabel>
                    <asp:HiddenField ID="hdnCategory" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Sector
                </td>
                <td>
                    <dx:ASPxLabel ID="lblSector" runat="server" Text="">
                    </dx:ASPxLabel>
                    <asp:HiddenField ID="hdnSector" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Platform <span class="clsmandtr">*</span>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <dx:ASPxRadioButtonList ID="rdPlatform" runat="server" ValueType="System.String"
                                    AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="rdPlatform_SelectedIndexChanged"
                                    ClientSideEvents-SelectedIndexChanged="clsevent">
                                    <Items>
                                        <dx:ListEditItem Text="Liquid" Value="0" />
                                        <dx:ListEditItem Text="Granules" Value="1" />
                                        <dx:ListEditItem Text="Soluble Unit Dose [Pods]" Value="2" />
                                        <dx:ListEditItem Text="Other (includes Distribution Module)" Value="3" />
                                    </Items>
                                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="valStudyInfo">
                            <RequiredField IsRequired="True" ErrorText="Please select Platform" />
                            <RequiredField IsRequired="True"></RequiredField>
                        </ValidationSettings>
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
                    Employment Status <span class="clsmandtr">*</span>
                </td>
                <td>
                    <dx:ASPxRadioButtonList runat="server" ValueType="System.String" ID="rdEmpStatus"
                        RepeatDirection="Horizontal">
                        <Items>
                            <dx:ListEditItem Text="PG Employees" Value="PG Employees" />
                            <dx:ListEditItem Text="Contractors" Value="Contractors" />
                        </Items>
                        <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="valStudyInfo">
                            <RequiredField IsRequired="True" ErrorText="Please select Employment Status" />
                            <RequiredField IsRequired="True"></RequiredField>
                        </ValidationSettings>
                    </dx:ASPxRadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    Fiscal Year <span class="clsmandtr">*</span>
                </td>
                <td>
                    <dx:ASPxComboBox TextField="FiscalYear" ValueField="FiscalYear_ID" ID="drpFiscalYear"
                        runat="server" EnableIncrementalFiltering="False" IncrementalFilteringMode="StartsWith" NullText="- Select Fiscal Year-">
                        <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="valStudyInfo">
                            <RequiredField IsRequired="True" ErrorText="Please select Fiscal Year" />
                            <RequiredField IsRequired="True"></RequiredField>
                        </ValidationSettings>
                    </dx:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    Campaign <span class="clsmandtr">*</span>
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
                       <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="valStudyInfo">
                            <RequiredField IsRequired="True" ErrorText="Please select Campaign" />
                            <RequiredField IsRequired="True"></RequiredField>
                        </ValidationSettings>
                    </dx:ASPxRadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    Total Site Population <span class="clsmandtr">*</span>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <dx:ASPxTextBox ID="txtTotalSitePop" runat="server" Width="170px">
                                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                        <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </td>
                            <td>
                                Note: Enter Numeric values only.
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    Number Individuals In Grade 1 Medical Monitoring Program <span class="clsmandtr">*</span>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <dx:ASPxTextBox ID="txtMedicalMonitor" runat="server" Width="170px">
                                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                        <RequiredField IsRequired="true" ErrorText="Please enter value" />
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </td>
                            <td>
                                Note: Number can not exceed Total Site Population.
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    Principal Reporter <span class="clsmandtr">*</span>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <dx:ASPxTextBox ID="txtReporter" runat="server" Width="170px" ClientInstanceName="txtReporter"
                                    ReadOnly="true">
                                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="valStudyInfo">
                                        <RequiredField IsRequired="True" ErrorText="Please select tissue type" />
                                        <RequiredField IsRequired="True"></RequiredField>
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </td>
                            <td>
                                <dx:ASPxButton ID="btnPersonalListing" ClientInstanceName="btnPersonalListing" runat="server"
                                    Width="111" Border-BorderStyle="None" BackgroundImage-ImageUrl="~/styles/Images/Personallisting.jpg">
                                    <ClientSideEvents Click="EnzymesPersonsLookup" />
                                </dx:ASPxButton>
                            </td>
                            <td>
                                <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" ClientInstanceName="PersonalListingCallBack"
                                    runat="server" Width="200px" OnCallback="PersonalListingCallBack_Callback">
                                    <ClientSideEvents EndCallback="function(s, e) {
    if (s.cpPersonalListing != '') {
       txtReporter.SetText(s.cpPersonalListing);
       txtReporter.GetMainElement().style.backgroundColor = 'green';
       txtReporter.GetMainElement().style.color = 'white';
    }
    else {
       txtReporter.SetText('');
    }
         }" />
                                </dx:ASPxCallbackPanel>
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
                                <div style="float: right">
                                    <dx:ASPxButton ID="btnAddEnzyme" runat="server" Text="Add Another Antigen" OnClick="btnAddEnzyme_Click">
                                    </dx:ASPxButton>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%--                                <dx:ASPxGridView ID="grdEnzymes" ClientInstanceName="grdEnzymes" runat="server" KeyFieldName="CampaignID"
                                    Width="100%" AutoGenerateColumns="False" OnStartRowEditing="RowEditing" OnRowDeleting="OnRowDeleting"
                                    OnRowCommand="rowcommand">
                                    <Columns>
                                        <dx:GridViewCommandColumn VisibleIndex="4" Name="Edit" Caption="Edit" CellStyle-HorizontalAlign="Center">
                                            <EditButton Visible="true">
                                            </EditButton>
                                        </dx:GridViewCommandColumn>
                                        <dx:GridViewCommandColumn ShowDeleteButton="true" VisibleIndex="5" Caption="Delete"
                                            CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <dx:GridViewDataColumn FieldName="EnzymeName" VisibleIndex="1" />
                                        <dx:GridViewDataColumn FieldName="CreatedBy" VisibleIndex="2" Caption="Created" />
                                        <dx:GridViewDataColumn FieldName="CreatedDate" VisibleIndex="3" />
                                    </Columns>
                                    <SettingsBehavior ConfirmDelete="true" />
                                    <SettingsText ConfirmDelete="Are you sure you want to delete" />
                                </dx:ASPxGridView>--%>
                                <dx:ASPxGridView ID="grdEnzymes" ClientInstanceName="grdEnzymes" runat="server" KeyFieldName="CampaignID"
                                    Width="100%" AutoGenerateColumns="False" OnStartRowEditing="RowEditing" OnRowDeleting="OnRowDeleting"
                                    OnRowCommand="rowcommand">
                                    <Columns>
                                        <dx:GridViewDataColumn VisibleIndex="4" Name="Edit" Caption="Edit" CellStyle-HorizontalAlign="Center">
                                            <DataItemTemplate>
                                                <dx:ASPxButton HorizontalAlign="Center" ID="btnEdit" OnClick="btnEdit_Click" runat="server"
                                                    AutoPostBack="false" CausesValidation="false" ToolTip="Edit" EnableDefaultAppearance="false"
                                                    Image-Url="~/styles/Images/edit_icon.gif">
                                                </dx:ASPxButton>
                                            </DataItemTemplate>
                                            <HeaderStyle Wrap="True" HorizontalAlign="Center" />
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewCommandColumn ShowDeleteButton="true" VisibleIndex="5" Caption="Delete"
                                            HeaderStyle-HorizontalAlign="Center" />
                                        <dx:GridViewDataColumn FieldName="EnzymeName" VisibleIndex="1" />
                                        <dx:GridViewDataColumn FieldName="CreatedBy" VisibleIndex="2" Caption="Created" />
                                        <dx:GridViewDataColumn FieldName="CreatedDate" VisibleIndex="3" />
                                    </Columns>
                                    <SettingsBehavior ConfirmDelete="true" />
                                    <SettingsText ConfirmDelete="Are you sure you want to delete" />
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
                                <table width="100%">
                                    <tr>
                                        <td class="clsCampaignhdr" colspan="3">
                                            Current Campaign
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="400px">
                                            Antigen Number and Enzyme Name <span class="clsmandtr">*</span>
                                            
                                        </td>
                                        <td >
                                        <dx:ASPxLabel ID="lblEnzyme" Visible="false" runat="server" Text="ASPxLabel">
                                            </dx:ASPxLabel>
                                            <dx:ASPxComboBox SelectedIndex="0" ID="drpEnzyme" Width="300" TextField="EnzymeName"
                                                ValueField="Enzyme_ID" runat="server" ValueType="System.String">
                                            </dx:ASPxComboBox>
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <div class="moduleCont">
                                                <table width="100%">
                                                    <tr class="clsModule">
                                                        <td width="300px">
                                                            Module:
                                                        </td>
                                                        <td width="250px">
                                                            MAKING
                                                        </td>
                                                        <td width="500px">
                                                            DISTRIBUTION
                                                            <div style="font-weight: normal; float: right; padding-right: 5px">
                                                                Note: Enter Numeric values only</div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Total number of exposed individuals <span class="clsmandtr">*</span>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox ID="txtTotalnumberofexposedindividuals" runat="server" Width="170px"
                                                                TabIndex="1">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox TabIndex="6" ID="txt_tot_num_of_exp_ind_distr" runat="server" Width="170px"
                                                                Number="0">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Total Number of Previous Positives Still Working<span class="clsmandtr">*</span>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" Number="0" ID="txtTotalNumberofPreviousPositivesStillWorking"
                                                                runat="server" Width="170px" TabIndex="1">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox TabIndex="7" ID="txt_tot_num_pre_pos_st_wrk_distr" runat="server"
                                                                Width="170px">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Total Number of Baseline Positive Individuals<span class="clsmandtr">*</span>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox TabIndex="2" ID="txtTot_Num_Basline_Pos_Ind" runat="server" Width="170px">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox TabIndex="8" ID="txt_tot_num_of_base_pos_ind_distr" runat="server"
                                                                Width="170px">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Total not available for testing<span class="clsmandtr">*</span>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox TabIndex="3" ID="txtTot_Ava_for_testing" runat="server" Width="170px">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox TabIndex="9" ID="txt_tot_not_ava_for_test_distr" runat="server" Width="170px">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Total to be Tested<span class="clsmandtr">*</span>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" ReadOnly="true" Number="0"
                                                                ID="txtTot_tobe_Tested" runat="server" Width="170px">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" ReadOnly="true" Number="0"
                                                                ID="txt_tot_to_be_test_distr" runat="server" Width="170px">
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Total New positives with prior negative skin test<span class="clsmandtr">*</span>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox TabIndex="4" ID="txt_Tot_new_pos_wit_prior_nega_skin_tes" runat="server"
                                                                Width="170px">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox TabIndex="10" ID="txt_tot_new_pos_wit_pri_neg_skin_test_distr" runat="server"
                                                                Width="170px">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Number of Skin Test Negatives<span class="clsmandtr">*</span>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox TabIndex="5" ID="txt_num_skin_test_neg" runat="server" Width="170px">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox TabIndex="11" ID="txt_num_of_skin_test_neg_distr" runat="server"
                                                                Width="170px">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Number of non-participants (available but not tested)<span class="clsmandtr">*</span>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox ReadOnly="true" Number="0" ID="txtNum_non_participants" runat="server"
                                                                Width="170px">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" ReadOnly="true" Number="0"
                                                                ID="txt_num_of_non_part_distr" runat="server" Width="170px">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <div class="moduleCont">
                                                <table width="100%">
                                                    <tr class="clsModule">
                                                        <td width="300px">
                                                            Module:
                                                        </td>
                                                        <td width="250px">
                                                            PACKING
                                                        </td>
                                                        <td width="500px">
                                                            OTHERS
                                                            <div style="font-weight: normal; float: right; padding-right: 5px">
                                                                Note: Enter Numeric values only</div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Total number of exposed individuals<span class="clsmandtr">*</span>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox TabIndex="12" Number="0" ID="txt2Tot_num_f_exp_ind" runat="server"
                                                                Width="170px">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox TabIndex="18" ID="txt_tot_num_of_exp_ind_Other" runat="server" Width="170px">
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Total Number of Previous Positives Still Working<span class="clsmandtr">*</span>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox TabIndex="13" Number="0" ID="txt2_tot_num_f_pre_pos_sti_wor" runat="server"
                                                                Width="170px">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox TabIndex="19" ID="tot_num_of_pre_pos_stil_work_other" runat="server"
                                                                Width="170px">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Total Number of Baseline Positive Individuals<span class="clsmandtr">*</span>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox TabIndex="14" ID="txt2_num_f_bas_pos_ind" runat="server" Width="170px">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox TabIndex="20" ID="txt_tot_num_of_base_pos_ind_other" runat="server"
                                                                Width="170px">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Total not available for testing<span class="clsmandtr">*</span>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox TabIndex="15" ID="txt2_tot_avai_f_test" runat="server" Width="170px">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox TabIndex="21" ID="txt_tot_not_avai_for_test_Other" runat="server"
                                                                Width="170px">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Total to be Tested<span class="clsmandtr">*</span>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" MinValue="0" ReadOnly="true"
                                                                Number="0" ID="txt2_tot_tob_test" runat="server" Width="170px">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" MinValue="0" ReadOnly="true"
                                                                Number="0" ID="txt_tot_to_be_test_Other" runat="server" Width="170px">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Total New positives with prior negative skin test<span class="clsmandtr">*</span>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox TabIndex="16" ID="txt2_tot_new_pos_wt_pri_neg_skintest" runat="server"
                                                                Width="170px">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox TabIndex="22" ID="txt_tot_new_pos_wit_pri_neg_ski_test_Other" runat="server"
                                                                Width="170px">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Number of Skin Test Negatives<span class="clsmandtr">*</span>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox TabIndex="17" ID="txt2num_f_ski_tes_neg" runat="server" Width="170px">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox TabIndex="23" ID="txt_num_of_ski_neg_Other" runat="server" Width="170px">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Number of non-participants (available but not tested)<span class="clsmandtr">*</span>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" MinValue="0" ReadOnly="true"
                                                                Number="0" ID="txt2num_f_non_participants" runat="server" Width="170px">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox SpinButtons-ShowIncrementButtons="false" MinValue="0" ReadOnly="true"
                                                                Number="0" ID="txt_num_non_parti_other" runat="server" Width="170px" ValidationSettings-ValidateOnLeave="true">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="true" ErrorText="Please enter Value" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
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
                                <dx:ASPxButton ID="btnSaveExit" runat="server" Text="Save & Exit" OnClick="btnSaveExit_Click"
                                    ValidationGroup="valStudyInfo">
                                </dx:ASPxButton>
                            </td>
                            <td>
                                <dx:ASPxButton ID="btnSaveNew" runat="server" Text="Save & Create New" OnClick="btnSaveNew_Click"
                                    ValidationGroup="valStudyInfo">
                                </dx:ASPxButton>
                            </td>
                            <td>
                                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click">
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div style="font-weight: bold; padding: 20px 20px 100px 20px; text-align: center">
            <dx:ASPxLabel ID="lblReturnStatus" runat="server" Text="">
            </dx:ASPxLabel>
        </div>
    </div>
    <script type="text/javascript" src="../Scripts/validations.js"></script>
</asp:Content>

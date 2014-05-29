<%@ Page Title="Demographic View" Language="C#" MasterPageFile="~/MasterPages/EnzymeMain.Master"
	AutoEventWireup="true" CodeBehind="DemographicDataView.aspx.cs" Inherits="EnzymeWeb.DemographicDataView" %>

<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderMain" runat="server">
	<div>
	<%--	<table class="OptionsTable BottomMargin">
			<tr>
				<td>
					<dx:ASPxLabel ID="ASPxLabel1" runat="server" />
				</td>
				<td style="padding-right: 16px">
				</td>
				<td>
				</td>
				<td>
				</td>
			</tr>
		</table>--%><asp:Button ID="btnSubmit" runat="server" Text="Add New" OnClick="btnSubmit_Click" />
		<dx:ASPxGridView ID="gvDemographicData" ClientInstanceName="gvDemographicData" runat="server"
			OnRowUpdating="OnRowUpdating" OnRowDeleting="OnRowDeleting" KeyFieldName="EnzymeDemographic_ID"
			Width="100%" AutoGenerateColumns="False" OnStartRowEditing="RowEditing" SettingsBehavior-ConfirmDelete="True">
			<%--<SettingsEditing  Mode="PopupEditForm">
        </SettingsEditing>--%>
			<SettingsBehavior ConfirmDelete="true"  />
			<Columns>
				<dx:GridViewDataColumn FieldName="Region_Name" VisibleIndex="0" Caption="Region" />
				<dx:GridViewDataColumn FieldName="Country_Name" VisibleIndex="1" Caption="Country" />
				<dx:GridViewDataColumn FieldName="SiteName" VisibleIndex="2" Caption="Site Name" />
				<%--           <dx:GridViewDataColumn FieldName="BusinessUnit" VisibleIndex="1" />
           <dx:GridViewDataColumn FieldName="Category" VisibleIndex="1" />
           <dx:GridViewDataColumn FieldName="Sector" VisibleIndex="1" />--%>
				<dx:GridViewDataColumn FieldName="Platform" VisibleIndex="3" Caption="Platform" />
				<dx:GridViewDataColumn FieldName="EmploymentStatus" VisibleIndex="4" />
				<dx:GridViewDataColumn FieldName="FiscalYear" VisibleIndex="5" />
				<dx:GridViewDataColumn FieldName="Campaign" VisibleIndex="6" />
				<%--         <dx:GridViewDataColumn FieldName="TotalSitePopulation" VisibleIndex="1" />
              <dx:GridViewDataColumn FieldName="NumberofIndividualsGrade1"/>--%>
				<dx:GridViewDataColumn FieldName="PrincipalReporter" VisibleIndex="7" />
				<dx:GridViewCommandColumn ShowEditButton="true" VisibleIndex="8" Caption=" " />
				<dx:GridViewCommandColumn ShowDeleteButton="true" VisibleIndex="9" Caption=" " />
			</Columns>
			<SettingsBehavior FilterRowMode="OnClick" />
			<SettingsPager AlwaysShowPager="True">
				<FirstPageButton Visible="True">
				</FirstPageButton>
				<LastPageButton Visible="True">
				</LastPageButton>
			</SettingsPager>
			<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" 
                ShowFilterRowMenuLikeItem="True" />
			<SettingsText ConfirmDelete="Are you sure you want to delete?" />
		</dx:ASPxGridView>
		
	</div>
</asp:Content>

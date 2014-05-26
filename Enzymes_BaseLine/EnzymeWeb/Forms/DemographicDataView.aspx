<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/EnzymeMain.Master"
    AutoEventWireup="true" CodeBehind="DemographicDataView.aspx.cs" Inherits="EnzymeWeb.DemographicDataView" %>

<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div>
    <table class="OptionsTable BottomMargin">
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Group by:" />
            </td>
            <td style="padding-right: 16px">
                
            </td>
            <td>
                
            </td>
            <td>
                
            </td>
        </tr>
    </table>
    <dx:ASPxGridView ID="gvDemographicData" ClientInstanceName="gvDemographicData" runat="server"
            OnRowUpdating="OnRowUpdating" OnRowDeleting="OnRowDeleting"
        KeyFieldName="EnzymeDemographic_ID"   Width="100%" AutoGenerateColumns="False" 
            onstartrowediting="RowEditing">
        <Columns>
        <dx:GridViewCommandColumn  ShowEditButton="true" VisibleIndex="0" />
        <dx:GridViewCommandColumn ShowDeleteButton="true" VisibleIndex="0" />
       
            <dx:GridViewDataColumn FieldName="Region_Name" VisibleIndex="1" />
           <dx:GridViewDataColumn FieldName="Country_Name" VisibleIndex="1" />
           <dx:GridViewDataColumn FieldName="SiteName" VisibleIndex="1" />
           <dx:GridViewDataColumn FieldName="BusinessUnit" VisibleIndex="1" />
           <dx:GridViewDataColumn FieldName="Category" VisibleIndex="1" />
           <dx:GridViewDataColumn FieldName="Sector" VisibleIndex="1" />
           <dx:GridViewDataColumn FieldName="Platform" VisibleIndex="1" />
           <dx:GridViewDataColumn FieldName="EmploymentStatus" VisibleIndex="1" />
           <dx:GridViewDataColumn FieldName="FiscalYear" VisibleIndex="1" />
           <dx:GridViewDataColumn FieldName="Campaign" VisibleIndex="1" />
           <dx:GridViewDataColumn FieldName="TotalSitePopulation" VisibleIndex="1" />
              <dx:GridViewDataColumn FieldName="NumberofIndividualsGrade1"/>
           <dx:GridViewDataColumn FieldName="PrincipalReporter"  />
            <dx:GridViewDataColumn FieldName="EnzymeDemographic_ID"  />
         </Columns>
        
       
        <%--<SettingsEditing  Mode="PopupEditForm">
        </SettingsEditing>--%>
    </dx:ASPxGridView>
    <asp:Button ID ="btnSubmit" runat="server" Text="Add New" onclick="btnSubmit_Click" />
    </div>
    
</asp:Content>

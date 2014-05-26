<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/EnzymeMain.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="EnzymeWeb.Home" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div style="padding:20px">
    <table>
        <tr><td style="font-weight:bold">Enzyme DB Options Menu</td></tr>
        <tr><td>Enzyme DB(HSKE)</td></tr>
        <tr><td><a href="DemographicDataView.aspx">View Existing Demographic Data</a></td></tr>
        <tr><td><a href="NewDemographicData.aspx">Create New Demographic Data</a></td></tr>
    </table>
    </div>
</asp:Content>

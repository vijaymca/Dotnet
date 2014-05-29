<%@ Page Title="Home" Language="C#" MasterPageFile="~/MasterPages/EnzymeMain.Master"
    AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="EnzymeWeb.Home" %>

<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<script type="text/javascript">
    function viewpage() {
        location.href = "DemographicDataView.aspx";
    }

    function newpage() {
        location.href = "NewDemographicData.aspx";
    }

</script>

    <div style="padding: 20px; background-color: White; height: 100%">
        <table align="center">
            <tr>
                <td style="font-weight: bold; color: #4c4c4c; font-size: 12pt; font-family: verdana,arial,helvetica,sans-serif;
                    text-align: center">
                    Enzyme DB Options Menu
                </td>
            </tr>
            <tr>
                <td style="text-align: center">
                    Enzyme DB(HSKE)
                </td>
            </tr>
            <tr>
                <td>
                    <a style="border-style: none;cursor:pointer" onclick="viewpage()" >
                        <img alt="" src="../styles/Images/View-Existing-Demographic-Data.jpg" />
                    </a>
                </td>
            </tr>
            <tr>
                <td>
                    <a style="border-style: none;cursor:pointer" onclick="newpage()">
                        <img alt="" src="../styles/Images/Create-New-Demographic-Data.jpg" />
                    </a>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

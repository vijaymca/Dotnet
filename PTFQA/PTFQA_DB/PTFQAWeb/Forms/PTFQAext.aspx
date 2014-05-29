<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/PTFQAMain.Master" AutoEventWireup="true"
    CodeBehind="PTFQAext.aspx.cs" Inherits="PTFQAWeb.Forms.PTFQAext" %>

<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v13.2, Version=13.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxUploadControl" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div>
        <table>
            <tr>
                <td colspan="2">

                </td>
            </tr>
            <tr>
                <td>
                    Region:
                </td>
                <td>
                    <dx:ASPxComboBox ID="drpRegion" runat="server" ValueType="System.String">
                    </dx:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    Country:
                </td>
                <td>
                    <dx:ASPxComboBox ID="drpCountry" runat="server" ValueType="System.String">
                    </dx:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    Site:
                </td>
                <td>
                    <dx:ASPxComboBox ID="drpSite" runat="server" ValueType="System.String">
                    </dx:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    OMI SiteID Code:
                </td>
                <td>
                    <dx:ASPxComboBox ID="drpOMISiteID" runat="server" ValueType="System.String">
                    </dx:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    Business Unit:
                </td>
                <td>
                    <dx:ASPxComboBox ID="drpBusinessUnit" runat="server" ValueType="System.String">
                    </dx:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    Site Operator:
                </td>
                <td>
                    <dx:ASPxComboBox ID="drpSiteOpe" runat="server" ValueType="System.String">
                    </dx:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    Last QA Review:
                </td>
                <td>
                    <dx:ASPxTextBox ID="txtLastQARev" runat="server" Width="170px">
                    </dx:ASPxTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    QA Review Due:
                </td>
                <td>
                    <dx:ASPxTextBox ID="txtQARevDue" runat="server" Width="170px">
                    </dx:ASPxTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Current Review:
                </td>
                <td>
                    <dx:ASPxCheckBox ID="chkbxCurrentReview" runat="server">
                    </dx:ASPxCheckBox>
                </td>
            </tr>
            <tr>
                <td>
                    PFT - FEVI SCORE(%):
                </td>
                <td>
                    <dx:ASPxTextBox ID="txtFEVISCORE" runat="server" Width="170px">
                    </dx:ASPxTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    PFT - FVCSCORE(%):
                </td>
                <td>
                    <dx:ASPxTextBox ID="txtFVCSCORE" runat="server" Width="170px">
                    </dx:ASPxTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    PFT - OVERALL SCORE(%):
                </td>
                <td>
                    <dx:ASPxTextBox ID="txtOVERALL" runat="server" Width="170px">
                    </dx:ASPxTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Pass/Fail:
                </td>
                <td>
                    <dx:ASPxRadioButtonList ID="btnPassFail" runat="server" RepeatDirection="Horizontal">
                        <Items>
                            <dx:ListEditItem Text="Pass" Value="" />
                            <dx:ListEditItem Text="Fail" Value="" />
                        </Items>
                    </dx:ASPxRadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    Other Comments:
                </td>
                <td>
                    <dx:ASPxHtmlEditor ID="txtHTMLComments" runat="server">
                    </dx:ASPxHtmlEditor>
                </td>
            </tr>
             <tr>
                <td>Attach:
                </td>
                <td>
                    <dx:ASPxUploadControl ID="ASPxUploadControl1" runat="server" UploadMode="Auto" Width="280px">
                    </dx:ASPxUploadControl>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <dx:ASPxButton ID="btnOK" runat="server" Text="OK">
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel">
                    </dx:ASPxButton>
                </td>
            </tr>
           
        </table>
    </div>
</asp:Content>

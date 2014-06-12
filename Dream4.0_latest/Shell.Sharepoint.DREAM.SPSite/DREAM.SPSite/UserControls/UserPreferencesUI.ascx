<%@ Control Language="C#" AutoEventWireup="true" Codebehind="UserPreferencesUI.ascx.cs"
    Inherits="Shell.SharePoint.DREAM.Site.UI.UserControls.UserPreferencesUI" %>
<asp:Panel ID="UserPreferencesPanel" runat="server" Width="100%" CssClass="panelPadding">
    <table border="0" cellpadding="4" cellspacing="0" class="tableAdvSrchBorder" width="100%">
        <tr>
            <td class="tdAdvSrchSubHeader" colspan="2">
                Preferences Screen</td>
        </tr>
    </table>
    <asp:Panel ID="ExceptionPanel" Visible="false" runat="server">
        <asp:Label ID="lblException" runat="server" Text="" CssClass="labelMessage"></asp:Label>
    </asp:Panel>
    <table border="0" cellpadding="4" cellspacing="0" class="tableAdvSrchBorder" width="100%">
        <tr>
            <td class="tdAdvSrchItem" width="20%">
                Default Result Display</td>
            <td class="tdAdvSrchItem">
                <asp:DropDownList ID="cboDisplay" runat="server" CssClass="dropdownAdvSrch" Width="190px">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td class="tdAdvSrchItem" width="20%">
                Depth Units</td>
            <td class="tdAdvSrchItem">
                <asp:DropDownList ID="cboDepthUnits" runat="server" CssClass="dropdownAdvSrch" Width="190px">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td class="tdAdvSrchItem" width="20%">
                Pressure Units</td>
            <td class="tdAdvSrchItem">
                <asp:DropDownList ID="cboPressureUnits" runat="server" CssClass="dropdownAdvSrch"
                    Width="190px">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td class="tdAdvSrchItem" width="20%">
                Temperature Units</td>
            <td class="tdAdvSrchItem">
                <asp:DropDownList ID="cboTemperatureUnits" runat="server" CssClass="dropdownAdvSrch"
                    Width="190px">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td class="tdAdvSrchItem" width="20%">
                Default Country</td>
            <td class="tdAdvSrchItem">
                <asp:DropDownList ID="cboCountry" runat="server" CssClass="dropdownAdvSrch" Width="190px">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td class="tdAdvSrchItem" width="20%">
                Default Asset</td>
            <td class="tdAdvSrchItem">
                <asp:DropDownList ID="cboAsset" runat="server" CssClass="dropdownAdvSrch" Width="190px">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td class="tdAdvSrchItem" width="20%">
                Records per page</td>
            <td class="tdAdvSrchItem">
                <asp:DropDownList ID="cboRecordsPerPage" runat="server" CssClass="dropdownAdvSrch"
                    Width="190px">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td class="tdAdvSrchItem" width="20%">
                Default Basin</td>
            <td class="tdAdvSrchItem">
                <asp:DropDownList ID="cboBasin" runat="server" CssClass="dropdownAdvSrch" Width="190px">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td class="tdAdvSrchSubHeader" colspan="2">
                <img src="/_LAYOUTS/DREAM/images/info_off.gif" id="imgLinks" onclick="HideDisplayLinks('tblLinks', 'imgLinks')" />Links</td>
        </tr>
        <tr>
            <td class="tdAdvSrchItem" colspan="2">
                <table id="tblLinks" width="100%" border="0" cellpadding="4" cellspacing="0" class="tableAdvSrchBorder">
                    <tr>
                        <td class="tdAdvSrchItem" width="20%">
                            Title</td>
                        <td class="tdAdvSrchItem" width="25%">
                            <asp:TextBox ID="txtLinkTitle1" runat="server" CssClass="textboxAdvSrch" Width="200px"></asp:TextBox></td>
                        <td class="tdAdvSrchItem" width="15%">
                            URL</td>
                        <td class="tdAdvSrchItem">
                            <asp:TextBox ID="txtLinkUrl1" runat="server" CssClass="textboxAdvSrch" Width="200px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tdAdvSrchItem" width="20%">
                            Title</td>
                        <td class="tdAdvSrchItem" width="25%">
                            <asp:TextBox ID="txtLinkTitle2" runat="server" CssClass="textboxAdvSrch" Width="200px"></asp:TextBox></td>
                        <td class="tdAdvSrchItem" width="15%">
                            URL</td>
                        <td class="tdAdvSrchItem">
                            <asp:TextBox ID="txtLinkUrl2" runat="server" CssClass="textboxAdvSrch" Width="200px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tdAdvSrchItem" width="20%">
                            Title</td>
                        <td class="tdAdvSrchItem" width="25%">
                            <asp:TextBox ID="txtLinkTitle3" runat="server" CssClass="textboxAdvSrch" Width="200px"></asp:TextBox></td>
                        <td class="tdAdvSrchItem" width="15%">
                            URL</td>
                        <td class="tdAdvSrchItem">
                            <asp:TextBox ID="txtLinkUrl3" runat="server" CssClass="textboxAdvSrch" Width="200px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td width="20%">
                            Title</td>
                        <td width="25%">
                            <asp:TextBox ID="txtLinkTitle4" runat="server" CssClass="textboxAdvSrch" Width="200px"></asp:TextBox></td>
                        <td width="15%">
                            URL</td>
                        <td>
                            <asp:TextBox ID="txtLinkUrl4" runat="server" CssClass="textboxAdvSrch" Width="200px"></asp:TextBox></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center" height="32px">
                <asp:Button ID="cmdSubmit" runat="server" CssClass="buttonAdvSrch" Text="Submit"
                    Width="77px" OnClick="cmdSubmit_Click" OnClientClick="return ValidateUserPreferences()" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="cmdReset" runat="server" CssClass="buttonAdvSrch" Text="Reset" Width="77px"
                    OnClientClick="return ResetUserPreferences()" />
            </td>
        </tr>
    </table>
</asp:Panel>

<%@ Control Language="C#" AutoEventWireup="true" Codebehind="RegionFormatSettings.ascx.cs"
    Inherits="Shell.SharePoint.DREAM.Site.UI.UserControls.RegionFormatSettings" %>
<table border="0" cellpadding="5px" cellspacing="0" class="tableAdvSrchBorder" width="100%"
    id="TABLE1">
    <tr>
        <td class="tdBasinAdvSrchHeader" style="font-weight: bold;" colspan="2">
            Format Date
        </td>
    </tr>
    <tr>
    <tr>
        <td class="tdAccessData" colspan="2" style="color: Red; font-size: 10;">
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </td>
    </tr>
    <tr id="trSelectLayout" visible="true" runat="server">
        <td nowrap class="tdAccessData" style="width: 50%; height: 28px;">
            DateTime</td>
        <td class="tdAccessData" width="80%" style="height: 28px">
            <asp:DropDownList ID="cboDateFormat" runat="server" AutoPostBack="False" CssClass="dropdownAdvSrch"
                Width="100%" />
        </td>
    </tr>
    <tr>
        <td class="tdAccessData" colspan="2" align="right" style="width: 20%">
            <asp:Button ID="cmdGoSelect" runat="server" CssClass="buttonAdvSrch" OnClick="cmdGoSelect_Click"
                Text="Update" />
            <asp:Button ID="btnClose" runat="server" CssClass="buttonAdvSrch" OnClientClick="CloseWindow();"
                Text="Close" />&nbsp;
        </td>
    </tr>
</table>

<script type="text/JavaScript">
    setWindowTitle('Regional Date Format');    
</script>


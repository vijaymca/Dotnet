<%@ Control Language="C#" AutoEventWireup="true" Codebehind="ManageTeam.ascx.cs"
    Inherits="Shell.SharePoint.DREAM.Site.UI.ManageTeam" %>
<asp:Panel ID="AdvancedSearchContent" runat="server" Width="100%" DefaultButton="btnSave">
    <table class="tableAdvSrchBorder" cellspacing="0" cellpadding="2" border="0" width="100%">
        <tr>
            <td class="tdAdvSrchHeader" colspan="2" style="font-weight: bold">
                &nbsp;<asp:Label ID="lblAddTeam" runat="server" Text="New Team"></asp:Label>
                <asp:Label ID="lblTeam" runat="server" Text="" />
            </td>
        </tr>
    </table>
    <br />
    <table width="100%" class="tableAdvSrchBorder" cellpadding="4" cellspacing="0">
        <tr>
            <td class="AdvancedColumn" colspan="2">
                <b>Details</b> <span class="AlertTxt">[* Indicates Mandatory Field]</span></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblErrorMessage" runat="server" Text="" Visible="false" CssClass="labelMessage" />
            </td>
        </tr>
        <tr class="tdAdvSrchItemNbrdr">
            <td style="width: 39%; text-align: right">
                <span class="AlertTxt">*</span>
            Name
            <td style="width: 80%">
                <asp:TextBox ID="txtTEAMNAME" runat="server" CssClass="queryfieldmini" Width="200px"
                    MaxLength="80"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" valign="top">
            </td>
        </tr>
        <tr class="tdAdvSrchItemNbrdr">
            <td style="width: 39%; text-align: right">
                <span class="AlertTxt">*</span> Team Owner
            </td>
            <td style="width: 80%">
                <%--  Multi Team Owner Implementation --%>
                <%--<asp:DropDownList  ID="cboTEAMOWNER" style="display:none;" runat="server" CssClass="dropdownAdvSrch" Width="200px"></asp:DropDownList>--%>
                <asp:ListBox ID="lstTeamOwner" EnableViewState="true" runat="server" Width="200px"
                    CssClass="dropdownAdvSrch" SelectionMode="Multiple"></asp:ListBox>
                <%--  Multi Team Owner Implementation --%>
            </td>
        </tr>
        <tr class="tdAdvSrchItemNbrdr">
            <td align="center" colspan="2" valign="top">
            </td>
        </tr>
        <tr class="tdAdvSrchItemNbrdr">
            <td style="width: 39%; text-align: right;">
                <span class="AlertTxt">*</span> Project
            </td>
            <td style="width: 80%">
                <asp:DropDownList ID="cboPROJECTNAME" runat="server" CssClass="dropdownAdvSrch" Width="200px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="tdAdvSrchItemNbrdr">
            <td align="center" colspan="2" valign="top">
            </td>
        </tr>
        <tr class="tdAdvSrchItemNbrdr">
            <td style="width: 39%; text-align: right">
                Map Bookmark
            </td>
            <td style="width: 80%">
                <asp:DropDownList ID="cboMAPBOOKMARK" runat="server" CssClass="dropdownAdvSrch" Width="200px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="tdAdvSrchItemNbrdr">
            <td align="center" colspan="2" valign="top">
                <br />
            </td>
        </tr>
        <tr class="tdAdvSrchItemNbrdr">
            <td valign="top" align="center" colspan="2">
                <asp:Button ID="btnSave" runat="server" Text="Save" Width="90px" CssClass="buttonAdvSrch"
                    OnClientClick="return ValidateAddTeam();" OnClick="AddTeam" />
                &nbsp;&nbsp;
                <input type="button" id="btnCancel" runat="server" value="Cancel" class="buttonAdvSrch"
                    style="width: 90px" onserverclick="BtnCancel_ServerClick" />&nbsp;
            </td>
        </tr>
    </table>
    <input type="hidden" id="hidTeamOwner" runat="server" /></asp:Panel>

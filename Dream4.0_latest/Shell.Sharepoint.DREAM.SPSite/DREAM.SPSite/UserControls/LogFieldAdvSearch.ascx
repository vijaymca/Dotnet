<%@ Control Language="C#" AutoEventWireup="true" Codebehind="LogFieldAdvSearch.ascx.cs"
    Inherits="Shell.SharePoint.DREAM.Site.UI.UserControls.LogFieldAdvSearch" %>
<asp:Panel ID="AdvancedSearchContent" DefaultButton="cmdSearch" runat="server" Width="100%">
    <table class="tableAdvSrchBorder" cellspacing="0" cellpadding="0" border="0" width="100%">
        <tr>
            <td class="tdBasinAdvSrchHeader" colspan="4">
                &nbsp;<b>Special Searches - <i>Logs by Field / Depth</i></b>
            </td>
        </tr>
    </table>
    <asp:Panel ID="ExceptionBlock" Visible="false" runat="server">
        <br />
        <asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label>
    </asp:Panel>
    <br />
    <table class="tableAdvSrchBorder" cellspacing="0" cellpadding="0" border="0" width="100%">
        <tr>
            <td width="20%" height="25px" class="tdAdvSrchItemNbrdr">
                &nbsp;Saved Search</td>
            <td width="80%" class="tdAdvSrchItemNbrdr">
                <asp:DropDownList ID="cboSavedSearch" runat="server" CssClass="dropdownAdvSrch" Width="185px"
                    AutoPostBack="True" OnSelectedIndexChanged="cboSavedSearch_SelectedIndexChanged">
                    <asp:ListItem>---Select---</asp:ListItem>
                </asp:DropDownList><asp:Image ID="imgSavedSearch" runat="server" ImageAlign="AbsMiddle"
                    ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
        </tr>
    </table>
    <br />
    <table class="tableAdvSrchBorder" cellspacing="0" cellpadding="4" border="0" width="100%">
        <tr>
            <td class="tdAdvSrchSubHeader" colspan="2" height="18px" rowspan="">
                <b>Search</b> &nbsp;</td>
            <td class="tdAdvSrchSubHeader" colspan="2" align="right" height="18px">
                [<span style="text-align: right;" class="spanColor">*</span> indicates required
                field] &nbsp;</td>
        </tr>
        <tr>
            <td class="tdAdvSrchItem" width="20%">
                Depth Units</td>
            <td class="tdAdvSrchItem">
                <asp:RadioButton ID="rdoDepthUnitsFeet" runat="server" GroupName="DepthUnits" Text="Feet" />&nbsp;
                <asp:RadioButton ID="rdoDepthUnitsMetres" runat="server" GroupName="DepthUnits" Text="Metres" />
            </td>
            <td colspan="2" class="tdAdvSrchItem">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="tdAdvSrchItem" width="20%">
                Field Name<span class="spanColor">*</span></td>
            <td class="tdAdvSrchItem">
                <asp:TextBox ID="txtFieldName" runat="server" Width="180px" CssClass="queryfieldmini"></asp:TextBox>
                <asp:Image ID="imgFieldName" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
            <td colspan="2" align="left" class="tdAdvSrchItem">
                [Wildcard = * OR %]</td>
        </tr>
        <tr>
            <td class="tdAdvSrchItem" width="20%">
                Curve Top Depth<span class="spanColor">*</span></td>
            <td class="tdAdvSrchItem">
                <asp:TextBox ID="txtCurveTopDepth" runat="server" Width="180px" CssClass="queryfieldmini"></asp:TextBox>
                <asp:Image ID="imgCurveTopDepth" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" />
            </td>
            <td class="tdAdvSrchItem" width="20%" align="left">
                Curve Bottom Depth<span class="spanColor">*</span></td>
            <td class="tdAdvSrchItem">
                <asp:TextBox ID="txtCurveBottomDepth" runat="server" Width="180px" CssClass="queryfieldmini"></asp:TextBox>
                <asp:Image ID="imgCurveBottomDepth" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" />
            </td>
        </tr>
        <tr>
            <td class="tdAdvSrchSubHeader" colspan="4" height="18px">
                <b>Save Search Criteria</b></td>
        </tr>
        <tr>
            <td width="20%" class="tdAdvSrchItemNbrdr">
                Search Name</td>
            <td width="30%" class="tdAdvSrchItemNbrdr">
                <asp:TextBox ID="txtSaveSearch" runat="server" CssClass="queryfieldmini" Width="180px"></asp:TextBox>
                <asp:Image ID="imgSearchName" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" />
                <input type="hidden" name="hidEnableButton" value="" />
            </td>
            <td height="25px" width="10%" class="tdAdvSrchItemNbrdr">
                <asp:CheckBox Height="18px" ToolTip="Type of Save Search(Personal/Shared)" Text="Shared"
                    ID="chbShared" runat="server" />
            </td>
            <td align="left" colspan="2" width="30%" class="tdAdvSrchItemNbrdr">
                <input type="button" id="cmdSaveSearch" runat="server" value="Save Search" class="buttonAdvSrch"
                    onserverclick="cmdSaveSearch_Click" onclick="if(!ValidateSaveSrchLogByField())return false;" />&nbsp;
                <asp:Button ID="cmdSearch" runat="server" Text="Search" CssClass="buttonAdvSrch"
                    OnClientClick="if(!ValidateLogByField())return false;" OnClick="cmdSearch_Click" />
                <input type="button" id="cmdReset" runat="server" value="Reset" class="buttonAdvSrch"
                    onclick="if(!ValidateAdvSearchReset())return false;" onserverclick="cmdReset_Click" /></td>
        </tr>
    </table>
</asp:Panel>

<script type="text/JavaScript">
    setWindowTitle('Special Searches - Logs by Field / Depth');
</script>


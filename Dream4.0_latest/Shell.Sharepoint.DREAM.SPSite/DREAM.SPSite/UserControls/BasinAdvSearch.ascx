<%@ Control Language="C#" AutoEventWireup="true" Codebehind="BasinAdvSearch.ascx.cs"
    Inherits="Shell.SharePoint.DREAM.Site.UI.UserControls.BasinAdvSearch" %>
<asp:Panel ID="AdvancedSearchContent" DefaultButton="cmdSearch" runat="server" Width="100%">
    <table class="tableAdvSrchBorder" cellspacing="0" cellpadding="0" border="0" width="100%">
        <tr>
            <td class="tdBasinAdvSrchHeader" style="font-weight: bold;" colspan="4">
                &nbsp;Advanced Search - Basin
            </td>
        </tr>
    </table>
    <asp:Panel ID="ExceptionBlock" Visible="false" runat="server">
        <asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label>
    </asp:Panel>
    <br />
    <span id="spanLstBxSelectionErr" class="labelMessage" style="display: none">Some error(s)
        occurred while processing the request. Please see the fields marked in * for more
        details.<br />
        <br />
    </span>
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
            <td class="tdAdvSrchSubHeader" colspan="4" height="18px">
                <b>Search By File</b>[<a href="javascript:ResetFileSearchCriteria()" class="LinkTxt">Reset File Search Criteria</a>]</td>
        </tr>
        <tr>
            <td valign="top" class="tdAdvSrchItem" style="width: 20%">
                Search By</td>
            <td colspan="3" class="tdAdvSrchItem">
                <asp:DropDownList ID="cboSearchCriteria" runat="server" CssClass="dropdownAdvSrch"
                    Width="185px">
                    <asp:ListItem>---Select---</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td valign="top" class="tdAdvSrchItem" style="width: 20%">
                Select File to search</td>
            <td colspan="3" class="tdAdvSrchItem">
                <input type="file" id="fileUploader" runat="server" class="button" contenteditable="false" /></td>
        </tr>
        <tr>
            <td class="tdAdvSrchSubHeader" colspan="4" height="18px">
                <b>Search</b> &nbsp;</td>
        </tr>
        <tr>
            <td class="tdAdvSrchItem">
                <sup id="spanBasinSup" class="labelMessage" style="display: none">*</sup><span>Basin
                    Identifier</span></td>
            <td class="tdAdvSrchItem" colspan="3">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr valign="top">
                        <td>
                            <asp:ListBox ID="lstBasin" EnableViewState="true" runat="server" Width="185px" CssClass="dropdownAdvSrch"
                                SelectionMode="Multiple"></asp:ListBox>
                            <asp:Image ID="imgBasin" runat="server" ImageAlign="Top" ImageUrl="/_layouts/DREAM/images/icon_help.gif" />&nbsp;&nbsp;
                        </td>
                        <td>
                            <span id="spanBasinSelectionError" class="labelMessage" style="display: none">The maximum
                                number of items per selection criteria is 999.
                                <br />
                                You have exceeded this for "Basin". Please amend your
                                <br />
                                criteria and try again.</span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdAdvSrchItem" style="width: 178px;">
                Description</td>
            <td class="tdAdvSrchItem">
                <asp:TextBox ID="txtDescription" runat="server" Width="180px" CssClass="queryfieldmini"></asp:TextBox>
                <asp:Image ID="imgDescription" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
            <td class="tdAdvSrchItem" colspan="2" style="width: 50%">
                [Wildcard = * OR %]</td>
        </tr>
        <tr>
            <td class="tdAdvSrchItem" style="width: 178px;">
                Status</td>
            <td class="tdAdvSrchItem" colspan="3">
                <asp:TextBox ID="txtStatus" runat="server" Width="180px" CssClass="queryfieldmini"></asp:TextBox>
                <asp:Image ID="imgStatus" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
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
                    onserverclick="cmdSaveSearch_Click" onclick="if(!ValidateSaveSrchBasin())return false;" />&nbsp;
                <asp:Button ID="cmdSearch" runat="server" Text="Search" CssClass="buttonAdvSrch"
                    OnClientClick="return ValidateBasinSearch();" OnClick="cmdSearch_Click" />
                <input type="button" id="cmdReset" runat="server" value="Reset" class="buttonAdvSrch"
                    onclick="if(!ValidateAdvSearchReset())return false;" onserverclick="cmdReset_Click" />
            </td>
        </tr>
    </table>
</asp:Panel>

<script type="text/JavaScript">
    setWindowTitle('Basin Advanced Search');
    SetListBoxSelectedValues('<%= lstBasin.ClientID %>');     
</script>

<input type="hidden" id="hidWordContent" runat="server" />
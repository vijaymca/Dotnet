<%@ Control Language="C#" AutoEventWireup="true" Codebehind="RolesAndProfiles.ascx.cs"
    Inherits="Shell.SharePoint.DREAM.Site.UI.UserControls.RolesAndProfiles" %>
<%@ Register Assembly="Telerik.Web.UI, Version=2010.1.415.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4"
    Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<div>
    <table class="tableAdvSrchBorder" width="100%" cellspacing="0" cellpadding="2" border="0">
        <tr>
            <td class="tdAdvSrchHeader">
                <b>Roles and Profiles</b></td>
        </tr>
    </table>
    <br />
    <asp:UpdatePanel ID="updatePanelRolesProfiles" runat="server">
        <contenttemplate>
    <table class="tableAdvSrchBorder" width="100%" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td >
                <table width="100%" cellspacing="0" cellpadding="5" border="0">
                    <tr runat="server" id="trRole">
                        <td class="tdAdvSrchItem" >
                         <span class="rolesProfilesLabel">Roles</span>
                        </td>
                        <td class="tdAdvSrchItem">
                            <asp:DropDownList ID="ddlRole" runat="server" CssClass="dropdownAdvSrch" Width="200"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr runat="server" id="trProfileType">
                        <td class="tdAdvSrchItem">
                            <span class="rolesProfilesLabel">Profile Type</span>
                        </td>
                        <td class="tdAdvSrchItem">
                            <asp:DropDownList ID="ddlProfileType" runat="server" CssClass="dropdownAdvSrch" Width="200"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlProfileType_SelectedIndexChanged">
                            </asp:DropDownList>
                             <asp:ImageButton ID="btnDeleteProfile" ToolTip="Click to delete profile for the role." runat="server" ImageUrl="/_layouts/DREAM/images/MyAssetDelete.gif" OnClick="btnDeleteProfile_btnclick"></asp:ImageButton>
                        </td>
                        <td class="tdAdvSrchItem">
                        
                        </td>
                    </tr>
                    <tr runat="server" id="trAssetType">
                        <td class="tdAdvSrchItem">
                            <span class="rolesProfilesLabel">Asset Type</span>
                        </td>
                        <td class="tdAdvSrchItem">
                       
                            <asp:DropDownList ID="ddlAssetType" runat="server" CssClass="dropdownAdvSrch" Width="200"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlAssetType_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr runat="server" id="trResultType">
                        <td class="tdAdvSrchItem">
                            <span class="rolesProfilesLabel">Search Name</span>
                        </td>
                        <td class="tdAdvSrchItem">
                            <asp:DropDownList ID="ddlSearchNames" runat="server" CssClass="dropdownAdvSrch" Width="200"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlSearchNames_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr runat="server" id="trCategory">
                    <td class="tdAdvSrchItem" valign="top">
                    <span class="rolesProfilesLabel">Group Header</span>
                    </td>
                    <td class="tdAdvSrchItem">
                    <telerik:RadListBox runat="server" ID="radLstBxCategory" Width="200px" Height="140" AllowReorder="true" ButtonSettings-VerticalAlign="Middle" SelectionMode="Multiple" ButtonSettings-ReorderButtons=All EnableDragAndDrop="true" AutoPostBack="true" OnSelectedIndexChanged="radLstBxCategory_SelectedIndexChanged" EmptyMessage="No items to display" ></telerik:RadListBox>
                    </td>
                    </tr>
                    
                    </table>
            </td>
        </tr>
        <tr >
            <td >
                <table width="100%" border="0" cellpadding="5" cellspacing="0">
                <tr runat="server" id="trAddRemoveItems">
                    <td class="tdAdvSrchItem" width="32%">
                            <span class="rolesProfilesLabel">List of Context Searches</span>
                        </td>
                        <td class="tdAdvSrchItem">
                            <span class="rolesProfilesLabel">Selected Context Searches</span>
                        </td>
                    </tr>
                    <tr runat="server" id="trAddRemoveItemsHeader">
                        <td class="tdAdvSrchItem" width="31%">
                            <span class="rolesProfilesLabel">List of Columns</span>
                        </td>
                        <td class="tdAdvSrchItem">
                            <span class="rolesProfilesLabel">Preferred Columns</span>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="30%" class="tdAdvSrchItem">
                                <telerik:RadListBox runat="server" ID="radLstBxSource" Width="200px" Height="200px" AllowTransfer="true" TransferToID="radLstBxDestination" ButtonSettings-VerticalAlign="Middle" SelectionMode="Multiple">
                                </telerik:RadListBox>
                                </td>
                        <td  valign="top" class="tdAdvSrchItem">
                                 <telerik:RadListBox runat="server" ID="radLstBxDestination" Width="200px" Height="200px" AllowReorder="true" ButtonSettings-VerticalAlign="Middle" SelectionMode="Multiple" ButtonSettings-ReorderButtons=All EnableDragAndDrop="true"></telerik:RadListBox>
                                </td>
                    </tr>
                    <tr runat="server" id="trAddRemoveItemsButton">
                        <td align="center" colspan="2" height="100" >
                            <asp:Button ID="btnSave" runat="server" CssClass="buttonAdvSrch" Text="Save" Width="90px"
                                OnClick="btnSave_btnclick"/>
                            &nbsp; &nbsp; &nbsp;
                            <input id="btnReset" type="button" value="Close" class="buttonAdvSrch" style="width:90px" onclick="javascript:window.close();"/>
                            &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </contenttemplate>
    </asp:UpdatePanel>
    <!-- Regional Default Enhancements ends-->
</div>

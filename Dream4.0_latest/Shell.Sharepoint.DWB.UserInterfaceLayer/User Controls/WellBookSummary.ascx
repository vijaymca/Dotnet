<%@ Control Language="C#" AutoEventWireup="true" Codebehind="WellBookSummary.ascx.cs"
    Inherits="Shell.SharePoint.DWB.UserInterfaceLayer.WellBookSummary" %>
<%@ Register Assembly="Telerik.Web.UI, Version=2010.1.415.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4"
    Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<link href="/_Layouts/DREAM/Styles/DWBStyleSheetRel2_0.css" rel="stylesheet" type="text/css" />
<link href="/_Layouts/DREAM/Styles/DWBReportLayout.css" rel="stylesheet" type="text/css" />
<asp:Panel ID="pnlTemplate" Visible="false" runat="server" Width="100%">
    <asp:Panel ID="ExceptionBlock" Visible="false" runat="server">
        <asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label>
        <br />
    </asp:Panel>
    <asp:HiddenField ID="hdnPageOwner" runat="server" />
    <asp:HiddenField ID="hdnIncludeStoryBoard" runat="server" />
    <asp:HiddenField ID="hdnIncludePageTitle" runat="server" />
    <asp:HiddenField ID="hdnPrintMyPages" runat="server" />
    <asp:HiddenField ID="hdnIncludeFilter" runat="server" />
    <asp:HiddenField ID="hdnSignedOffPages" runat="server" />
    <asp:HiddenField ID="hdnEmptyPages" runat="server" />
    <asp:HiddenField ID="hdnPageType" runat="server" />
    <asp:HiddenField ID="hdnPageName" runat="server" />
    <asp:HiddenField ID="hdnDiscipline" runat="server" />
    
    <%-- <asp:HiddenField ID="hdnSelectedStatus" runat="server" />--%>
    <asp:HiddenField ID="hdnWellBookId" runat="server" />
    <table class="DWBtableAdvSrchBorder" cellspacing="2" cellpadding="2" border="0" width="100%">
        <tr>
            <td class="DWBtdAdvSrchSubHeader" colspan="2" style="height: 18px" align="right">
            </td>
        </tr>
        <tr>
            <td class="DWItemText" style="width: 28%; height: 28px;">
                Team</td>
            <td class="DWBtdAdvSrchItem" style="width: 80%; height: 28px;">
                &nbsp;<asp:TextBox ID="txtTeam" runat="server" Width="50%" CssClass="DWBqueryfieldmini"
                    Enabled="false"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="DWItemText" style="width: 28%; height: 26px;">
                <asp:Label ID="lblAssetValue" runat="server" CssClass="DWItemText" Text="Owner"></asp:Label></td>
            <td class="DWBtdAdvSrchItem" style="width: 80%; height: 26px;">
                &nbsp;<asp:TextBox ID="txtOwner" runat="server" Width="50%" CssClass="DWBqueryfieldmini"
                    Enabled="false"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="DWItemText" style="width: 28%; height: 35px">
                Sign Off</td>
            <td class="DWBtdAdvSrchItem" style="width: 80%; height: 35px">
                &nbsp;<asp:TextBox ID="txtSignOffStatus" runat="server" Width="10%" CssClass="DWBqueryfieldmini"
                    Enabled="false"></asp:TextBox>
                <asp:Button ID="btnSignOff" runat="server" Width="20%" OnClick="btnSignOff_Click"
                    CssClass="DWBbuttonAdvSrch" Visible="false" />&nbsp;
                <asp:Button ID="btnPrint" runat="Server" Text="Print this Book" OnClick="btnPrint_Click"
                    CssClass="DWBbuttonAdvSrch" Visible="false" />&nbsp;
                <asp:Button ID="btnBatchImport" runat="Server" Text="Batch Import" CssClass="DWBbuttonAdvSrch"
                    Visible="false" />&nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 100%; height: 166px;" colspan="2" align="center">
                <asp:GridView ID="grdWellBookSummary" AutoGenerateColumns="False" runat="server"
                    ShowFooter="False" Width="65%" OnRowDataBound="grdWellBookSummary_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="Page_Owner" />
                        <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:LinkButton ID="TotalId" runat="server" Text='<%# Eval("Total") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:LinkButton ID="SignOffId" runat="server" Text='<%# Eval("Signed_Off") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:LinkButton ID="NotSigned_OffId" runat="server" Text='<%# Eval("NotSigned_Off") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:LinkButton ID="EmptyId" runat="server" Text='<%# Eval("Empty") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="width: 100%" align="center">
                <asp:Button ID="btnCustomiseChapters" runat="server" Text="Customise Chapters" CssClass="DWBbuttonAdvSrch"
                    Visible="false"  />
                <asp:Button ID="btnFilterPages" runat="server" Text="Filter Pages" CssClass="DWBbuttonAdvSrch"
                    Visible="false" />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="width: 100%" align="center">
                <%--<asp:UpdatePanel ID="updatePanelRolesProfiles" runat="server">
                    <contenttemplate>--%>
  <div id="diveWBCustomiseChapters" class="popup"  style="left: 10px; top: 91px" runat="server">
                <table>
                    <tr>
                        <td class="reorderHeader">
                           <input type="checkbox" runat="server" id="chkSelectDeselectAll" tooltip="Select All" checked="checked" /> <b>List Of Chapters</b></td>
                        <td align="right">
                            <img src="/_layouts/DREAM/Images/CMSRemoveImage.GIF" onclick="return HideeWBChpaterReorderPopUp('diveWBCustomiseChapters');" alt="Close" /></td>
                    </tr>
                    <tr>
                        <td style="padding: 5px 5px 5px 5px" colspan="2">
                            <telerik:RadListBox ID="radLstChapters" runat="server" AllowReorder="True" CheckBoxes="True" EnableDragAndDrop="True"
                                Width="200px" Height="200px" AllowTransfer="False" ButtonSettings-VerticalAlign="Middle"
                                SelectionMode="Multiple" ButtonSettings-ReorderButtons="All" RegisterWithScriptManager="true" PersistClientChanges="true"
                                 OnClientItemChecked="OnClientItemCheckedHandler"  >
                                               
                                <Localization MoveDown="Move Chapter Down" MoveUp="Move Chapter Up" ToBottom="Move Chapter To Bottom"
                                    ToTop="Move Chapter To Top" />
                                <ButtonSettings ReorderButtons="All" TransferButtons="All" VerticalAlign="Middle" />
                            </telerik:RadListBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="padding: 5px 5px 5px 5px">
                            &nbsp;<asp:Button ID="btnApply" runat="server" Text="Apply"  /> &nbsp;&nbsp;
                            <asp:Button ID="btnApplyAndSave" runat="server" Text="Apply & Save"  />
                        </td>
                    </tr>
                </table>
            </div>
         <%--   </contenttemplate>
                </asp:UpdatePanel>--%>
            </td>
        </tr>
    </table>
</asp:Panel>

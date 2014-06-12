<%@ Control Language="C#" AutoEventWireup="true" Codebehind="BatchImportConfiguration.ascx.cs"
    Inherits="Shell.SharePoint.DWB.UserInterfaceLayer.BatchImportConfiguration" %>

<%--<script language="javascript" src="/_Layouts/DREAM/Javascript/CastleBusyBoxRel2_1.js"></script>--%>

<script language="javascript" src="/_Layouts/DREAM/Javascript/DWBJavascriptFunctionRel2_0.js"></script>

<script language="javascript" src="/_Layouts/DREAM/Javascript/DREAMJavascriptFunctionsRel2_1.js"></script>

<link href="/_Layouts/DREAM/Styles/DWBStyleSheetRel2_0.css" rel="stylesheet" type="text/css" />
<div id="AdvancedSearchContainer">
    <asp:Panel ID="pnlTemplate" runat="server" Width="100%" HorizontalAlign="Center">
        &nbsp;<table class="DWBtableAdvSrchBorder" cellspacing="0" cellpadding="2" border="0"
            width="99%">
            <tr>
                <td class="DWBtdAdvSrchHeader" style="font-weight: bold" align="left">
                    &nbsp;<asp:Label ID="lblWellBookHeading" runat="server" Text="Batch Import Configuration"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:Panel ID="ExceptionBlock" Visible="false" runat="server">
            <asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label>
            <br />
        </asp:Panel>
        <asp:HiddenField ID="hdnResetStatus" runat="server" />
        <table cellspacing="2" cellpadding="2" border="0" width="100%">
            <tr>
                <td style="width: 100%; height: 166px;" align="center">
                    <asp:Panel ID="BatchImportConfigurationGridViewPanelID" runat="server" Width="100%"
                        CssClass="DWBBatchImportConfigurationPanel">
                        <asp:GridView ID="grdBatchImportConfiguration" AutoGenerateColumns="False" runat="server"
                            ShowFooter="False" Width="98%" OnRowDataBound="grdBatchImportConfiguration_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="Page_Name">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtSharedAreaPath" runat="server" CssClass="DWBqueryfieldmini" Width="98%">
                                        </asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:DropDownList ID="cboFileType" runat="server" CssClass="DWBdropdownAdvSrch" Width="100%">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:DropDownList ID="cboNamingConvention" runat="server" CssClass="DWBdropdownAdvSrch"
                                            Width="100%">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align="right" style="height: 28px">
                    &nbsp; &nbsp;<asp:Button ID="btnSaveAndContinue" runat="server" Text="Save & Continue"
                        CssClass="DWBbuttonAdvSrch" Width="15%" OnClick="btnSaveAndContinue_Click" />
                    &nbsp;<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="DWBbuttonAdvSrch"
                        Width="8%" OnClick="btnSave_Click" />
                    &nbsp;<asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="DWBbuttonAdvSrch"
                        Width="8%" OnClick="btnReset_Click" />
                    &nbsp;<asp:Button ID="btnResetToDefaultPath" runat="server" Text="ResetToDefaultPath" CssClass="DWBbuttonAdvSrch"
                        Width="17%" OnClick="btnResetToDefaultPath_Click" />  
                    &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="DWBbuttonAdvSrch"
                        Width="8%" OnClick="btnCancel_Click" />&nbsp;&nbsp;</td>
            </tr>
        </table>
    </asp:Panel>
</div>

<script type="text/JavaScript">
    setWindowTitle('Batch Import Configuration');
</script>


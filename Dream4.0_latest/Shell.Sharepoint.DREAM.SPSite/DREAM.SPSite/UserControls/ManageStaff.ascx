<%@ Control Language="C#" AutoEventWireup="true" Codebehind="ManageStaff.ascx.cs"
    Inherits="Shell.SharePoint.DREAM.Site.UI.ManageStaff" %>
<%@ Register Assembly="Shell.SharePoint.DWB.DualListControl" Namespace="Shell.SharePoint.DWB.DualListControl"
    TagPrefix="cc1" %>
<link href="/_Layouts/DREAM/Styles/DWBStyleSheetRel2_0.css" rel="stylesheet" type="text/css" />
<link href="/_Layouts/DREAM/Styles/DWBReportLayout.css" rel="stylesheet" type="text/css" />

<script language="javascript" src="/_Layouts/DREAM/Javascript/DREAMDualListScriptRel2_1.js"></script>

<script language="javascript" src="/_Layouts/DREAM/Javascript/DynamicListBoxScript.js"></script>

<asp:Panel ID="AdvancedSearchContent" runat="server" Width="100%" DefaultButton="cmdSave">
    <table class="tableAdvSrchBorder" cellspacing="0" cellpadding="2" border="0" width="100%">
        <tr>
            <td class="tdAdvSrchHeader" colspan="2" style="font-weight: bold; word-wrap: break-word"
                width="800" height="40px">
                Add / Remove Staff : &nbsp;<asp:Label ID="lblManageStaffTitle" runat="server" Font-Bold="True"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <table width="100%" class="tableAdvSrchBorder" cellpadding="0" cellspacing="0">
        <tr>
            <td colspan="2">
                <asp:Label ID="lblErrorMessage" runat="server" Text="" Visible="false" CssClass="labelMessage" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table class="tableAdvSrchBorder" cellspacing="2" cellpadding="2" border="0" width="100%">
                    <tr>
                        <td class="tdAdvSrchSubHeader1" style="height: 18px">
                            <asp:Panel ID="pnlStaff" runat="server" Width="100%" HorizontalAlign="Center" Height="247px">
                                <table>
                                    <tr>
                                        <td rowspan="5" colspan="3" style="width: 632px; height: 226px;">
                                            <cc1:DualList ID="dualManageStaff" runat="server" EnableMoveAll="true" EnableMoveUpDown="false"
                                                MoveAllLeftButtonText="<<" MoveAllRightButtonText=">>" MoveLeftButtonText="<"
                                                MoveRightButtonText=">" LeftListLabelText="" RightListLabelText="" Width="100%"
                                                EnableMoveLeft="true" EnableMoveRight="true" ShowLeftBox="true">
                                                <ButtonStyle Font-Bold="True" CssClass="buttonAdvSrch"></ButtonStyle>
                                                <LeftListStyle CssClass="DREAMDlstLeftList" Width="250px" />
                                                <RightListStyle CssClass="DREAMDlstRightList" Width="250px" />
                                            </cc1:DualList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="cmdSave" runat="server" Text="Save" CssClass="buttonAdvSrch" Width="8%"
                                OnClick="CmdSave_Click" />
                            <asp:Button ID="cmdCancel" runat="server" Text="Cancel" CssClass="buttonAdvSrch"
                                OnClick="CmdCancel_Click" Width="8%" />&nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <input type="hidden" id="hidTeamOwnerUserId" runat="server" />
</asp:Panel>

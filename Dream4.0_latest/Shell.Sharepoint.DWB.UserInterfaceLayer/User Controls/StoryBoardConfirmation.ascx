<%@ Control Language="C#" AutoEventWireup="true" Codebehind="StoryBoardConfirmation.ascx.cs"
    Inherits="Shell.SharePoint.DWB.UserInterfaceLayer.StoryBoardConfirmation" %>
<link href="/_LAYOUTS/DREAM/styles/DREAMStyleSheetRel2_1.css" rel="stylesheet" type="text/css" />
<link href="/_LAYOUTS/DREAM/styles/DWBStyleSheetRel2_0.css" rel="stylesheet" type="text/css" />

<script language="javascript" src="/_Layouts/DREAM/Javascript/DWBJavascriptFunctionRel2_0.js"></script>

<table width="100%">
    <tr>
        <td style="text-align: left; width: 100%; padding: 2px 2px 2px 2px; vertical-align: top;"
            class="DWBtdAdvSrchSubHeader">
            <b>Print Options</b></td>
    </tr>
    <tr>
        <td style="text-align: left; width: 100%;">
            <asp:Panel ID="ExceptionBlock" Visible="false" runat="server">
                <asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label>
                <br />
            </asp:Panel>
            <table width="100%">
                <tr>
                    <td style="width: 100%; text-align: left;">
                        <table width="100%" style="text-align: left;">
                            <tr>
                                <td style="width: 15%; text-align: right;">
                                    &nbsp;
                                </td>
                                <td style="vertical-align: middle; text-align: left" width="30%">
                                    <asp:Label ID="lblPageTitleConfirm" runat="server" Text="Title Page" CssClass="DWItemText"
                                        Width="100%"></asp:Label>
                                </td>
                                <td style="width: 30%; text-align: left">
                                    <asp:RadioButtonList ID="rblPageTitleConfirm" runat="server" RepeatDirection="Horizontal"
                                        Width="100%">
                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td style="vertical-align: middle; text-align: left" width="25%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%; text-align: right;">
                                    &nbsp;
                                </td>
                                <td style="vertical-align: middle; text-align: left;" width="30%">
                                    <asp:Label ID="lblStoryBoardInfo" runat="server" Text="StoryBoard Info" CssClass="DWItemText"
                                        Width="100%"></asp:Label>
                                </td>
                                <td style="width: 30%; text-align: left;">
                                    <asp:RadioButtonList ID="rblStoryBoardConfirm" runat="server" RepeatDirection="Horizontal"
                                        Width="100%">
                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td width="25%">
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlPrintMyPages" runat="server" Width="100%">
                            <table width="100%" style="text-align: left;">
                                <tr>
                                    <td style="width: 35%; text-align: left; vertical-align: top;">
                                        <asp:CheckBox ID="chkPrintMyPagesOnly" runat="server" Text="Print My Pages Only" /></td>
                                    <td style="width: 40%;">
                                        &nbsp;
                                    </td>
                                    <td style="width: 25%; text-align: right;">
                                        <asp:Panel ID="pnlIncludeFilter" runat="server" GroupingText="Include Filter" HorizontalAlign="Center"
                                            CssClass="DWBRadioBtnPanelCSS" Width="100%">
                                            <asp:RadioButtonList ID="rblIncludeFilter" runat="server" RepeatDirection="Horizontal"
                                                Width="100%">
                                                <asp:ListItem Text="Yes" Value="1" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td width="100%" style="text-align: left;">
                        <asp:Panel ID="pnlFilterOptions" runat="server" GroupingText="Filter Options" Visible="true"
                            CssClass="DWBRadioBtnPanelCSS" Width="100%">
                            <table width="100%" style="text-align: left;">
                                <tr>
                                    <td style="width: 100%; text-align: left;" colspan="2">
                                        <table style="width: 100%; text-align: left;">
                                            <tr>
                                                <td style="width: 20%; text-align: right">
                                                    <asp:Label ID="lblPageSignedOff" runat="server" Text="Signed Off" CssClass="DWItemText"
                                                        Width="100%"></asp:Label></td>
                                                <td style="vertical-align: middle; text-align: left" width="30%">
                                                    <asp:CheckBoxList ID="chklPageSignedOff" runat="server" RepeatDirection="Horizontal"
                                                        Width="100%">
                                                        <asp:ListItem Text="Yes" Value="1" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                                    </asp:CheckBoxList></td>
                                                <td style="width: 20%; text-align: right">
                                                    <asp:Label ID="lblPageEmpty" runat="server" Text="Empty" CssClass="DWItemText" Width="100%"></asp:Label>
                                                </td>
                                                <td style="vertical-align: middle; text-align: left" width="30%">
                                                    <asp:CheckBoxList ID="chklPageEmpty" runat="server" RepeatDirection="Horizontal"
                                                        Width="100%">
                                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; border: Solid 1px #ededed;" height="100%" width="40%">
                                        <table width="100%">
                                            <tr>
                                                <td style="vertical-align: top; text-align: right" width="26%">
                                                    <asp:Label ID="lblPageType" runat="server" Text="Page Type"></asp:Label>&nbsp;
                                                </td>
                                                <td style="vertical-align: middle; text-align: left" width="24%">
                                                    <asp:CheckBoxList ID="chkPageType" runat="server" RepeatDirection="Vertical" Width="100%">
                                                        <asp:ListItem Text="Automated" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Published" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="User Defined" Value="2"></asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="text-align: right; border: Solid 1px #ededed;" height="100%" width="60%">
                                        <table width="100%" cellspacing="2px">
                                            <tr>
                                                <td style="vertical-align: middle; text-align: right" width="15%">
                                                    <asp:Label ID="lblPageName" runat="server" Text="Page Name"></asp:Label>&nbsp;
                                                </td>
                                                <td style="vertical-align: middle; text-align: left" width="35%">
                                                    <asp:DropDownList ID="cboPageName" runat="server" Width="100%" CssClass="DWBdropdownAdvSrch"
                                                        AppendDataBoundItems="true">
                                                        <asp:ListItem Text="Select All" Value="-1" Selected="True" />
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="vertical-align: middle; text-align: right" width="15%">
                                                    <asp:Label ID="lblDiscipline" runat="server" Text="Discipline"></asp:Label>&nbsp;
                                                </td>
                                                <td style="vertical-align: middle; text-align: left" width="35%">
                                                    <asp:DropDownList ID="cboDisciplineName" runat="server" Width="100%" CssClass="DWBdropdownAdvSrch"
                                                        AppendDataBoundItems="true">
                                                        <asp:ListItem Text="Select All" Value="-1" Selected="True" />
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right" height="100%">
                        <input type="button" id="btnContinue" name="Continue" value="Continue" onclick="javascript:return HiddenPrintByPageValues('<%=blnType%>'); this.focus();"
                            class="DWBbuttonAdvSrch" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="DWBbuttonAdvSrch"
                            OnClientClick="window.close();return false;" />
                        <br />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>

<script language="javascript" type="text/javascript">
var isBookOwnerOrAD = '<%=blnIsBookOwnerOrAdmin%>'
if(isBookOwnerOrAD == "True")
{
    document.getElementById(GetObjectID("rblIncludeFilter","table")).disabled = true;
}
else
{
    document.getElementById(GetObjectID("chkPrintMyPagesOnly","input")).disabled = true;
    document.getElementById(GetObjectID("rblIncludeFilter","table")).disabled = true;    
}
</script>


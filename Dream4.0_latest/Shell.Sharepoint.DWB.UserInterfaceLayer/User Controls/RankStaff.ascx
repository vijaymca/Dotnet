<%@ Control Language="C#" AutoEventWireup="true" Codebehind="RankStaff.ascx.cs" Inherits="Shell.SharePoint.DWB.UserInterfaceLayer.RankStaff" %>
<%@ Register Assembly="Shell.SharePoint.DWB.DualListControl" Namespace="Shell.SharePoint.DWB.DualListControl"
    TagPrefix="cc1" %>

<%--<script language="javascript" src="/_Layouts/DREAM/Javascript/CastleBusyBoxRel2_1.js"></script>--%>

<script language="javascript" src="/_Layouts/DREAM/Javascript/DWBJavascriptFunctionRel2_0.js"></script>

<script language="javascript" src="/_Layouts/DREAM/Javascript/DualListScript.js"></script>

<script language="javascript" src="/_Layouts/DREAM/Javascript/DynamicListBoxScript.js"></script>

<link href="/_Layouts/DREAM/Styles/DWBStyleSheetRel2_0.css" rel="stylesheet" type="text/css" />
<link href="/_Layouts/DREAM/Styles/DWBReportLayout.css" rel="stylesheet" type="text/css" />
<link href="/_Layouts/DREAM/Styles/DWBStyleSheetRel2_0.css" rel="stylesheet" type="text/css" />
<!--Display Progress bar Frame starts here-->
<%--<iframe id="BusyBoxIFrame" visible="false" style="border: 3px double #D2D2D2" name="BusyBoxIFrame"
    frameborder="0" scrolling="no" ondrop="return false;"></iframe>

<script>
	/// Instantiate our BusyBox object
	var busyBox = new BusyBox("BusyBoxIFrame", "busyBox", 1, "processing", ".gif", 125, 147, 207);	
</script>--%>

<asp:HiddenField ID="hdnParentName" Value="" runat="server" />
<asp:HiddenField ID="hdnParentID" Value="" runat="server" />
<div id="AdvancedSearchContainer">
    <asp:Panel ID="pnlTemplate" runat="server" Width="100%">
        <table class="DWBtableAdvSrchBorder" cellspacing="0" cellpadding="2" border="0" width="100%">
            <tr>
                <td class="DWBtdAdvSrchHeader" colspan="2" style="font-weight: bold">
                    &nbsp;<asp:Label ID="lblHeading" runat="server" Text="Rank Staff: "></asp:Label>
                    <asp:Label ID="lblParentName" runat="server" Text="" />
                </td>
            </tr>
        </table>
        <br />
        <asp:Panel ID="ExceptionBlock" Visible="false" runat="server">
            <asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label>
            <br />
        </asp:Panel>
        <table class="DWBtableAdvSrchBorder" cellspacing="2" cellpadding="2" border="0" width="100%">
            <tr>
                <td width="50%">
                    <asp:Label ID="lblDiscipline" runat="server" Text="Discipline" Font-Bold="true"></asp:Label>
                </td>
                <td width="50%">
                    <asp:DropDownList ID="cboDiscipline" runat="server" AutoPostBack="true" OnSelectedIndexChanged="CboDiscipline_SelectedIndexChanged"
                        Width="100%">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="DWBtdAdvSrchSubHeader" colspan="2" style="height: 18px">
                    <asp:Panel ID="pnlPrivileges" runat="server" Width="100%" HorizontalAlign="Left"
                        Height="247px">
                        <table title="Rank" width="100%">
                            <tr>
                                <td><asp:Label ID="lblRankStaffs" Text="Staff rankings" runat="server" Width="100%"></asp:Label></td>
                                <td rowspan="5" colspan="3" style="width: 50%; height: 226px;" align="left">
                                    <cc1:DualList ID="dualListPrivileges" runat="server" EnableMoveAll="false" EnableMoveUpDown="true"
                                        MoveAllLeftButtonText="<<" MoveAllRightButtonText=">>" MoveLeftButtonText="<"
                                        MoveRightButtonText=">" LeftListLabelText="" RightListLabelText="Staff with the selected discipline in the team" Width="50%"
                                        EnableMoveLeft="false" EnableMoveRight="false" ShowLeftBox="false">
                                        <ButtonStyle Font-Bold="True"></ButtonStyle>
                                        <LeftListStyle CssClass="DWBDlstLeftListHide" />
                                        <RightListStyle CssClass="DWBDlstRightList" Width="250px" />
                                    </cc1:DualList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    &nbsp;<asp:Button ID="cmdSave" runat="server" Text="Save" CssClass="DWBbuttonAdvSrch" Width="8%"
                        OnClick="CmdSave_Click" />
                    <asp:Button ID="cmdSaveContinue" runat="server" Text="Save & Change more" CssClass="DWBbuttonAdvSrch"
                        Width="20%" OnClick="CmdSaveContinue_Click" />
                    <asp:Button ID="cmdCancel" runat="server" Text="Cancel" CssClass="DWBbuttonAdvSrch"
                        OnClick="CmdCancel_Click" Font-Size="X-Small" Width="8%" /></td>
            </tr>
        </table>
    </asp:Panel>
</div>

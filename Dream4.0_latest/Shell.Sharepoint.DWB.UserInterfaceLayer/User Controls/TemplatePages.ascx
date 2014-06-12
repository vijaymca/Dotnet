<%@ Control Language="C#" AutoEventWireup="true" Codebehind="TemplatePages.ascx.cs"
    Inherits="Shell.SharePoint.DWB.UserInterfaceLayer.TemplatePages" %>
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
	// Instantiate our BusyBox object
	var busyBox = new BusyBox("BusyBoxIFrame", "busyBox", 1, "processing", ".gif", 125, 147, 207);	
</script>--%>
<asp:HiddenField ID="hdnTemplateType" runat="server" Value="" />
<div id="AdvancedSearchContainer">
    <asp:Panel ID="pnlTemplate" runat="server" Width="100%">
        <table class="DWBtableAdvSrchBorder" cellspacing="0" cellpadding="2" border="0" width="100%">
            <tr>
                <td class="DWBtdAdvSrchHeader" colspan="2" style="font-weight: bold">
                    &nbsp;<asp:Label ID="lblTemplateHeading" runat="server" Text="Add / Remove Page(s):"></asp:Label>
                    <asp:Label ID="lblTemplateTitle" runat="server" Text="" />
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
                <td class="DWBtdAdvSrchSubHeader" style="height: 18px">
                    <asp:Panel ID="pnlMasterPages" runat="server" Width="100%" 
                        HorizontalAlign="Center" Height="247px">
                        <table title="Master Page">
                            <tr>
                                <td rowspan="5" colspan="3" style="width: 632px; height: 226px;">
                                   <cc1:DualList ID="duallistTemplatePages" runat="server" EnableMoveAll="true" EnableMoveUpDown="false"
                                        RightListLabelText="Pages in Template" LeftListLabelText="Pages in Library" MoveAllLeftButtonText="<<"
                                        MoveAllRightButtonText=">>" MoveLeftButtonText="<" MoveRightButtonText=">" Width="100%" EnableMoveLeft="true" EnableMoveRight="true" ShowLeftBox="true" >
                                        <ButtonStyle Font-Bold="True"></ButtonStyle>
                                        <LeftListStyle CssClass="DWBDlstLefttList" Width="250px" />
                                        <RightListStyle CssClass="DWBDlstRightList" Width="250px" />
                                    </cc1:DualList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
            <td align="center" colspan="2" style="height: 28px">
                &nbsp;<asp:Button ID="cmdSave" runat="server" Text="Save"  CssClass="DWBbuttonAdvSrch"  Width="8%" OnClick="CmdSave_Click" />
             <asp:Button ID="cmdCancel" runat="server" Text="Cancel" CssClass="DWBbuttonAdvSrch" OnClick="CmdCancel_Click" Font-Size="X-Small" Width="8%" /></td>
            </tr>
        </table>
    </asp:Panel>
</div>

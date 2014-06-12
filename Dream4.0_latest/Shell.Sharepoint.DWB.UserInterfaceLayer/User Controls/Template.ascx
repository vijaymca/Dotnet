<%@ Control Language="C#" AutoEventWireup="true" Codebehind="Template.ascx.cs" Inherits="Shell.SharePoint.DWB.UserInterfaceLayer.Template" %>
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

<asp:HiddenField ID="hdnTemplateID" runat="server" Value="" />
<asp:HiddenField ID="hdnTerminated" runat="server" Value="" />
<!--Display Progress bar Frame Ends here-->
<div id="AdvancedSearchContainer">
    <asp:Panel ID="pnlTemplate" runat="server" Width="100%" >
        <table class="DWBtableAdvSrchBorder" cellspacing="0" cellpadding="2" border="0" width="100%">
            <tr>
                <td class="DWBtdAdvSrchHeader" colspan="2" style="font-weight: bold">
                    &nbsp;<asp:Label ID="lblTemplateHeading" runat="server" Text="New Template" Width="103px"></asp:Label>
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
                <td class="DWBtdAdvSrchSubHeader" colspan="2" style="height: 18px">
                    <b>Details</b> &nbsp;[ <span class="DWBMandatoryMessage">* indicates mandatory field</span>
                    ]</td>
            </tr>
            <tr>
                <td class="DWBtdAdvSrchItem" style="width: 20%; height: 28px;">
                     <span class="DWBMandatory">*</span><asp:Label ID="lblTitle" runat="server" Text="Template Name"></asp:Label></td>
                <td class="DWBtdAdvSrchItem" style="width: 80%; height: 28px;">
                    <asp:TextBox ID="txtTemplateTitle" runat="server" Width="35%" CssClass="DWBqueryfieldmini" MaxLength="80"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="DWBtdAdvSrchItem" style="width: 20%">
                    <span class="DWBMandatory">*</span><asp:Label ID="lblAssetType" runat="server" Text="Asset Type"></asp:Label></td>
                <td class="DWBtdAdvSrchItem" style="width: 80%;">
                    <asp:DropDownList ID="cboAssetType" runat="server" CssClass="DWBdropdownAdvSrch"
                        Width="35%" AutoPostBack="True" OnSelectedIndexChanged="cboAssetType_SelectedIndexChanged">
                        <asp:ListItem>--Select--</asp:ListItem>
                        <%--<asp:ListItem>Well</asp:ListItem>
                    <asp:ListItem>Field</asp:ListItem>
                    <asp:ListItem>Wellbore</asp:ListItem>--%>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="DWBtdAdvSrchItem" style="width: 20%">
                    <asp:Label ID="lblCopyMasterPage" runat="server" Text="Copy Master Pages From"></asp:Label>
                    <%-- <span>Copy Master Pages From</span></td>--%>
                    <td class="DWBtdAdvSrchItem" style="width: 80%;">
                        <asp:DropDownList ID="cboTemplates" runat="server" CssClass="DWBdropdownAdvSrch"
                            Width="35%" AutoPostBack="False">
                            <asp:ListItem>--None--</asp:ListItem>
                            <%--<asp:ListItem>Well</asp:ListItem>
                    <asp:ListItem>Field</asp:ListItem>
                    <asp:ListItem>Wellbore</asp:ListItem>--%>
                        </asp:DropDownList>
                    </td>
            </tr>
            <%--<tr>
                <td class="DWBtdAdvSrchItem" style="width: 20%; height: 26px;">
                    <span></span>
                </td>
                <td class="DWBtdAdvSrchItem" style="width: 80%; height: 26px;">
                    &nbsp;</td>
            </tr>--%>
            <%--<tr>
                <td style="width: 54%; height: 26px;" colspan="3">
                    <asp:Panel ID="pnlMasterPages" runat="server" Width="90%" GroupingText="Master Pages"
                        HorizontalAlign="Center" Height="247px">
                        <table title="Master Page">
                            <tr>
                                <td rowspan="5" colspan="3" style="width: 632px; height: 226px;">
                                    <cc1:DualList ID="dualLstMasterPages" runat="server" EnableMoveAll="True" EnableMoveUpDown="True"
                                        MoveAllLeftButtonText="<<" MoveAllRightButtonText=">>" MoveDownButtonText=""
                                        MoveLeftButtonText="<" MoveRightButtonText=">" Width="100%" LeftListLabelText="From Library"
                                        RightListLabelText="Template Pages" Font-Bold="True" ListRows="12" MoveUpButtonText="">
                                        <ButtonStyle Font-Bold="True"></ButtonStyle>
                                        <LeftListStyle CssClass="DWBDlstLefttList" Width="250px" />
                                        <RightListStyle CssClass="DWBDlstRightList" Width="250px" />
                                    </cc1:DualList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>--%>
            <%--<tr>
                <td colspan="3" style="width: 54%; height: 26px">
                    <asp:GridView ID="tblMasterPages" runat="server" AutoGenerateColumns="False" CssClass="DWBmasterPagesContainer"
                        Width="60%" Height="10%" OnRowDataBound="tblMasterPages_RowDataBound">
                        <Columns>
                            <asp:BoundField AccessibleHeaderText="Page Sequence" DataField="Master_x0020_Page_x0020_Sequence"
                                HeaderText="Page Sequence" DataFormatString="{0:0000}">
                                <HeaderStyle Width="40%" />
                                <FooterStyle Width="25%" />
                            </asp:BoundField>
                            <asp:BoundField AccessibleHeaderText="Page Title " DataField="Title" HeaderText="Page Title">
                                <HeaderStyle Font-Underline="False" HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                        <RowStyle CssClass="evenRowStyle" />
                        <HeaderStyle CssClass="fixedHeader" />
                        <AlternatingRowStyle CssClass="oddRowStyle" />
                    </asp:GridView>
                </td>
            </tr>--%>
            <tr>
                <td align="center" colspan="2" style="height: 28px">
                    &nbsp;<asp:Button ID="cmdOK" runat="server" Text="Save" OnClientClick="javascript:return ValidateDWBTemplate();"
                        CssClass="DWBbuttonAdvSrch" OnClick="cmdOK_Click" Width="8%" />
                    <asp:Button ID="cmdCancel" runat="server" Text="Cancel" CssClass="DWBbuttonAdvSrch"
                        OnClick="cmdCancel_Click" Font-Size="X-Small" Width="8%" /></td>
            </tr>
        </table>
    </asp:Panel>
</div>

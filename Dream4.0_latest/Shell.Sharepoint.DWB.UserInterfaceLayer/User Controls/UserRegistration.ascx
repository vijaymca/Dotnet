<%@ Control Language="C#" AutoEventWireup="true" Codebehind="UserRegistration.ascx.cs"
    Inherits="Shell.SharePoint.DWB.UserInterfaceLayer.UserRegistration" %>

<%--<script language="javascript" src="/_Layouts/DREAM/Javascript/CastleBusyBoxRel2_1.js"></script>--%>

<script type="text/javascript" language="javascript" src="/_Layouts/DREAM/Javascript/DWBJavascriptFunctionRel2_0.js"></script>

<link href="/_Layouts/DREAM/Styles/DWBStyleSheetRel2_0.css" rel="stylesheet" type="text/css" />
<!--Display Progress bar Frame starts here-->
<%--<iframe id="BusyBoxIFrame" visible="false" style="border: 3px double #D2D2D2" name="BusyBoxIFrame"
    frameborder="0" scrolling="no" ondrop="return false;"></iframe>

<script>
	// Instantiate our BusyBox object
	var busyBox = new BusyBox("BusyBoxIFrame", "busyBox", 1, "processing", ".gif", 125, 147, 207);	
</script>--%>
<asp:HiddenField ID="hdnUserTerminated" Value="" runat="server" />
<!--Display Progress bar Frame Ends here-->
<div id="AdvancedSearchContainer">
    <asp:Panel ID="pnlTemplate" runat="server" Width="100%">
        <table class="DWBtableAdvSrchBorder" cellspacing="0" cellpadding="2" border="0" width="100%">
            <tr>
                <td class="DWBtdAdvSrchHeader" colspan="2" style="font-weight: normal">
                    &nbsp;<asp:Label ID="lblAddUser" runat="server" Text="New User" Font-Bold="True"></asp:Label>
                    <asp:Label ID="lblUser" runat="server" Text="" />
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
            <%--<tr>
                <td class="DWItemText" style="width: 28%; height: 28px;">
                    <span class="DWBMandatory">*</span>
                    <asp:Label ID="lblName" runat="server" Text="Name"></asp:Label></td>
                <td class="DWBtdAdvSrchItem" style="width: 80%; height: 28px;">
                    <asp:TextBox ID="txtUserName" runat="server" Width="40%" MaxLength="80" CssClass="DWBqueryfieldmini"></asp:TextBox>
                </td>
            </tr>--%>
            <%--<tr>
                <td class="DWItemText" style="width: 28%; height: 28px;">
                    <span class="DWBMandatory">*</span>
                    <asp:Label ID="lblUserId" runat="server" Text="Windows User Id"></asp:Label></td>
                <td class="DWBtdAdvSrchItem" style="width: 80%; height: 28px;">
                    <asp:TextBox ID="txtUserId" runat="server" Width="40%" MaxLength="80" CssClass="DWBqueryfieldmini"></asp:TextBox>
                </td>
            </tr>--%>
            <tr>
                <td class="DWItemText" style="width: 28%; height: 28px;">
                    <span class="DWBMandatory">*</span>
                    <asp:Label ID="lblUserId" runat="server" Text="Windows User Id"></asp:Label></td>
                <td class="DWBtdAdvSrchItem" style="width: 80%; height: 28px;">
                    <asp:DropDownList ID="cboUserId" runat="server" Width="40%" CssClass="DWBdropdownAdvSrch">
                        <asp:ListItem Value="--Select--"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
             <%-- <tr>
                <td class="DWItemText" style="width: 28%; height: 28px;">
                    <span class="DWBMandatory">*</span>
                    <asp:Label ID="lblPrivilege" runat="server" Text="Privilege"></asp:Label></td>
                <td class="DWBtdAdvSrchItem" style="width: 80%; height: 28px;">
                    <asp:DropDownList ID="cboPrivileges" runat="server" Width="40%" CssClass="DWBdropdownAdvSrch">
                        <asp:ListItem Value="--Select--"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>--%>
             <tr>
                <td class="DWItemText" style="width: 28%; height: 28px;">
                    
                    <asp:Label ID="lblIsAdmin" runat="server" Text="IsAdmin"></asp:Label></td>
                <td class="DWBtdAdvSrchItem" style="width: 80%; height: 28px;">
                    <asp:CheckBox ID="cbIsAdmin" runat="server" checked="false" OnCheckedChanged="cbIsAdmin_CheckedChanged" ></asp:checkbox>
                </td>
            </tr>
            <tr>
                <td class="DWItemText" style="width: 28%; height: 28px;">
                    <span class="DWBMandatory">*</span>
                    <asp:Label ID="lblDiscipline" runat="server" Text="Discipline"></asp:Label></td>
                <td class="DWBtdAdvSrchItem" style="width: 80%; height: 28px;">
                    <asp:DropDownList ID="cboDiscipline" runat="server" Width="40%" CssClass="DWBdropdownAdvSrch">
                        <asp:ListItem Value="--Select--"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="DWItemText" style="width: 28%; height: 28px;">
                    <span class="DWBMandatory" id="spnTeamMandotory" runat="server">*</span>
                    <asp:Label ID="lblTeam" runat="server" Text="Add to Team(s)"></asp:Label></td>
                <td class="DWBtdAdvSrchItem" style="width: 80%; height: 28px;">
                    <asp:ListBox ID="lstTeams" runat="server" Width="40%" CssClass="DWBdropdownAdvSrch"
                        SelectionMode="Multiple"></asp:ListBox>
                </td>
            </tr>
            <tr>
                <td align="center" style="height: 28px" colspan="2">
                    &nbsp;<asp:Button ID="cmdOK" runat="server" Text="Save" CssClass="DWBbuttonAdvSrch" Width="10%" OnClick="cmdOK_Click" OnClientClick="JavaScript:return ValidateDWBUser();" />
                    <asp:Button ID="cmdCancel" runat="server" Text="Cancel" CssClass="DWBbuttonAdvSrch"
                        Font-Size="X-Small" Width="10%" OnClick="cmdCancel_Click" /></td>
            </tr>
        </table>
    </asp:Panel>
</div>

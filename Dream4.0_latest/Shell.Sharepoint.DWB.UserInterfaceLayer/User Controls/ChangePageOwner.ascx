<%@ Control Language="C#" AutoEventWireup="true" Codebehind="ChangePageOwner.ascx.cs"
    Inherits="Shell.SharePoint.DWB.UserInterfaceLayer.ChangePageOwner" %>

<%--<script language="javascript" src="/_Layouts/DREAM/Javascript/CastleBusyBoxRel2_1.js"></script>--%>

<script language="javascript" src="/_Layouts/DREAM/Javascript/DWBJavascriptFunctionRel2_0.js"></script>

<script language="javascript" src="/_Layouts/DREAM/Javascript/DREAMJavaScriptFunctionsRel2_1.js"></script>

<link href="/_Layouts/DREAM/Styles/DWBStyleSheetRel2_0.css" rel="stylesheet" type="text/css" />
<!--Display Progress bar Frame starts here-->
<%--<iframe id="BusyBoxIFrame" visible="false" style="border:3px double #D2D2D2" name="BusyBoxIFrame" frameBorder="0" scrolling="no" ondrop="return false;">
</iframe>--%>

<%--<script>
	// Instantiate our BusyBox object
	//var busyBox = new BusyBox("BusyBoxIFrame", "busyBox", 1, "processing", ".gif", 125, 147, 207);	
</script>--%>

<!--Display Progress bar Frame Ends here-->
<div id="AdvancedSearchContainer">
    <asp:Panel ID="pnlTemplate" runat="server" Width="100%">
        <table class="DWBtableAdvSrchBorder" cellspacing="0" cellpadding="2" border="0" width="100%">
            <tr>
                <td class="DWBtdAdvSrchHeader" colspan="2" style="font-weight: bold">
                    &nbsp;<asp:Label ID="lblWellBookHeading" runat="server" Text="Change Page Owner :"></asp:Label>
                    <asp:Label ID="lblBookTitle" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <asp:Panel ID="ExceptionBlock" Visible="false" runat="server">
            <asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label>
            <br />
        </asp:Panel>
        <asp:HiddenField ID="hdnListitemStatus" runat="server" />
        <table class="DWBtableAdvSrchBorder" cellspacing="2" cellpadding="2" border="0" width="100%"
            style="height: 154px">
            <tr>
                <td class="DWBtdAdvSrchSubHeader" colspan="3" style="height: 18px" width="100%">
                    <b>Details</b> &nbsp;</td>
            </tr>
            <tr>
                <td class="DWItemText" style="width: 40%; height: 26px;" >
                    <asp:Label ID="lblSelectCurrentOwner" runat="server" CssClass="DWItemText" Style="text-align: right"
                        Text="Select Current Owner" Width="99%"></asp:Label></td>
                <td class="DWBtdAdvSrchItem" style="width: 30%; height: 26px" >
                    <asp:DropDownList ID="cboOwner" runat="server" CssClass="DWBdropdownAdvSrch" AutoPostBack="True"
                        OnSelectedIndexChanged="CboOwner_SelectedIndexChanged" Width="136px">
                        <asp:ListItem>--Select--</asp:ListItem>
                    </asp:DropDownList></td>
                <td class="DWBtdAdvSrchItem" style="width: 20%; height: 26px" >
                    <asp:Panel runat="server" ID="pnlRadiobuttonListID" CssClass="DWBRadioBtnPanelCSS"
                        GroupingText="Show" HorizontalAlign="Left">
                        <asp:RadioButtonList ID="rblStatus" runat="server" CssClass="ReportRadioBtnCSS" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" AutoPostBack="true">
                            <asp:ListItem Text="Active" Selected="True" />
                            <asp:ListItem Text="Terminated" />
                        </asp:RadioButtonList>
                    </asp:Panel>
                </td>
            </tr>
            <tr><td style="width:100%" colspan="3" >
            <asp:Panel runat="server" ID="pnlFilterOptions" CssClass="DWBRadioBtnPanelCSS" GroupingText="Filter Options"
                    HorizontalAlign="Left">
                     <table style="width:100%;">
                        <tr>           
                    <td class="DWItemText" style="width: 14%; height: 35px;" align="right" valign="top">
                        <asp:Label ID="lblPageTitle" runat="server" CssClass="DWItemText" Text="Page Name"
                            Style="text-align: center"></asp:Label></td>
                    <td class="DWBtdAdvSrchItem" style="height: 35px; width: 20%;" align="left" valign="top">
                        <asp:DropDownList ID="cboPageTitle" runat="server" CssClass="DWBdropdownAdvSrch"
                            Width="100%" AppendDataBoundItems="true">
                            <asp:ListItem style="align:center;">--Select All--</asp:ListItem>
                        </asp:DropDownList></td>
                    <td class="DWItemText" style="width: 10%; height: 35px;" align="right" valign="top">
                        <asp:Label ID="lblChapterName" runat="server" CssClass="DWItemText" Text="Chapter Name"
                            Style="text-align: center"></asp:Label></td>
                    <td class="DWBtdAdvSrchItem" style="width: 26%; height: 35px;" align="left">
                        <asp:ListBox ID="lstChapterNames" style="overflow:auto;" runat="server" SelectionMode="Multiple" Width="93%">
                        </asp:ListBox></td>
                        <td style="height: 28px" width="5%" align="right" valign="bottom"> &nbsp;
                        </td>
                    <td style="height: 35px" width="25%" align="right" valign="bottom">
                        <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="DWBbuttonAdvSrch"
                            OnClick="BtnFilter_Click" />
                            <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="DWBbuttonAdvSrch" 
                             OnClick="BtnReset_Click"/>
                             </td>
                    
                
            </tr>
            </table>
                    </asp:Panel>
           
            </td></tr>
            
            <tr>
                <td colspan="3">
                    <asp:Panel ID="PageOwnerGridViewPanelID" runat="server" CssClass="grdPanelStyle">
                    </asp:Panel>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="DWItemText" style="width: 40%; height: 26px">
                    <asp:Label ID="lblNewOwner" runat="server" CssClass="DWItemText" Text="Select New Owner"
                        Width="135px"></asp:Label></td>
                <td class="DWBtdAdvSrchItem" style="width: 30%; height: 26px">
                    <asp:DropDownList ID="cboNewOwner" runat="server" CssClass="DWBdropdownAdvSrch" Width="44%">
                        <asp:ListItem>--Select--</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td align="center" style="height: 28px" colspan="3">
                    &nbsp; &nbsp;<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="DWBbuttonAdvSrch"
                        Width="12%" OnClick="BtnSave_Click" />
                    &nbsp;<asp:Button ID="btnSaveAndChangeMore" runat="server" Text="Save & Change More"
                        CssClass="DWBbuttonAdvSrch" Width="20%" OnClick="BtnSaveAndChangeMore_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="DWBbuttonAdvSrch"
                        Width="12%" OnClick="BtnCancel_Click" /></td>
            </tr>
        </table>
    </asp:Panel>
</div>

<script type="text/JavaScript">
    setWindowTitle('Change Page Owner');    
</script>


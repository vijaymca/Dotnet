<%@ Control Language="C#" AutoEventWireup="true" Codebehind="AHDTVDConverter.ascx.cs"
    Inherits="Shell.SharePoint.DREAM.Site.UI.UserControls.AHDTVDConverter" %>
<!--Display Progress bar Frame starts here-->

<script src="/_Layouts/DREAM/Javascript/AHTVDCalculatorRel2_1.js" language="javascript"
    type="text/JavaScript"></script>
<table class="tableAdvSrchBorder" cellspacing="0" cellpadding="0" border="0" width="100%">
<tr>
            <td class="tdBasinAdvSrchHeader" style="font-weight:bold;" >
                &nbsp;AHD-TVD Converter
            </td>
        </tr>
</table>
<br />
<asp:Panel ID="pnlSoapError" runat="server" Visible="false">
    <table class="tableAdvSrchBorder" cellspacing="0" cellpadding="4" border="0" width="100%">
        <tr>
            <td style="white-space: normal;">
                <br />
                <asp:Label ID="lblErrorMessage" CssClass="labelMessage" runat="server" Visible="false"></asp:Label>
            </td>
            <td>
                <asp:Button ID="btn_goBack" runat="server" CssClass="buttonAdvSrch" Visible="false"
                    Text="Back" OnClientClick="javascript:history.go(-1)" />&nbsp;
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="pnlConverterContent" runat="server" Width="100%">
    <table width="100%">
        <tr>
            <td width="100%">
                <table class="tableAdvSrchBorder" cellspacing="0" cellpadding="4" border="0" width="100%">
                    <tr>
                        <td width="8%" align="left" class="tdAdvSrchItem">
                            <asp:Label ID="lblWellbore" runat="server" Text="Wellbore" CssClass="labelMessage1"></asp:Label>
                        </td>
                        <td width="42%" align="left" class="tdAdvSrchItem">
                            <asp:DropDownList ID="drpWellbore" runat="server" CssClass="dropdownAdvSrch" EnableViewState="true"
                                AutoPostBack="true" Width="155px" onchange="return AHTVConvertor(this);">
                            </asp:DropDownList>
                        </td>
                        <td width="20%" align="right" class="tdAdvSrchItem">
                            <asp:Label ID="lblDepthReference" runat="server" Text="Depth Reference" CssClass="labelMessage1"></asp:Label>
                        </td>
                        <td width="15%" align="right" class="tdAdvSrchItem">
                            <asp:DropDownList ID="drpDepthReference" runat="server" CssClass="dropdownAdvSrch"
                                Width="155px" onchange="return AHTVConvertor(this);">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td width="8%" align="left" class="tdAdvSrchItemNbrdr">
                            <asp:Label ID="lblDepthUnit" Text="Depth Unit" runat="server" CssClass="labelMessage1"></asp:Label>
                        </td>
                        <td width="42%" class="tdAdvSrchItemNbrdr">
                            <asp:RadioButton ID="rdoDepthUnitsFeet" runat="server" GroupName="DepthUnits" Text="Feet"
                                onClick="javascript:FeetMeter('ft');" />&nbsp;
                            <asp:RadioButton ID="rdoDepthUnitsMetres" runat="server" GroupName="DepthUnits" Text="Metres"
                                onClick="javascript:FeetMeter('m');" />
                        </td>
                        <td width="20%" align="right" class="tdAdvSrchItemNbrdr">
                        </td>
                        <td width="15%" align="right" class="tdAdvSrchItemNbrdr">
                            Export Page <a href="javascript:AHTVCalculatorExport();" class="resultHyperLink">
                                <img src="/_layouts/DREAM/images/icon_Excel.gif" /></a></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
</asp:Panel>
<asp:Panel ID="pnlTable" runat="server" Visible="true" Width="100%">
    <table width="100%" class="tableAdvSrchBorder">
        <tr>
            <td>
                <br />
                <table width="100%" class="convertAHTVtable" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="20%" style="height: 10px" class="tdCountry">
                            <asp:Label ID="lblCountry" runat="server" CssClass="labelMessage1">Country</asp:Label>
                        </td>
                        <td width="30%" style="height: 10px" class="tdCountry">
                            <asp:Label ID="lblProject" runat="server" CssClass="labelMessage1">Projected Coordinate System</asp:Label>
                        </td>
                        <td width="30%" style="height: 10px" class="tdCountry">
                            <asp:Label ID="lblWellborePath" runat="server" CssClass="labelMessage1">Wellbore Path</asp:Label>
                        </td>
                        <td width="20%" style="height: 10px" class="tdCountry">
                            <asp:Label ID="lblPDL" runat="server" CssClass="labelMessage1">PDL</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="20%" style="height: 10px" class="tdCountrylbl">
                            <asp:Label ID="lblResultCountry" runat="server" CssClass="labelMessage3">Country</asp:Label>
                        </td>
                        <td width="30%" style="height: 10px" class="tdCountrylbl">
                            <asp:Label ID="lblResultProject" runat="server" CssClass="labelMessage3">Projected Coordinate System</asp:Label>
                        </td>
                        <td width="30%" style="height: 10px" class="tdCountrylbl">
                            <asp:Label ID="lblResultWellborePath" runat="server" CssClass="labelMessage3">Wellbore Path</asp:Label>
                        </td>
                        <td width="20%" style="height: 10px" class="tdCountrylbl">
                            <asp:Label ID="lblResultPDL" runat="server" CssClass="labelMessage3">PDL</asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <br />
                            <asp:Button ID="btnConvert" runat="server" CssClass="buttonAdvSrch" Text="Convert"
                                OnClick="btnConvert_Click" OnClientClick="return ValidateTable();" />&nbsp;
                            <asp:Button ID="btnClear" runat="server" CssClass="buttonAdvSrch" Text="Clear" OnClientClick="return clearRows();" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr valign="top">
            <td valign="top" nowrap="nowrap" class="tableContainer">
                <div id="tableContainer" class="tableContainer">
                    <asp:Panel ID="pnl" runat="server">
                    </asp:Panel>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <br />
                            <asp:Button ID="btnConvert1" runat="server" Text="Convert" CssClass="buttonAdvSrch"
                                OnClick="btnConvert_Click" OnClientClick="return ValidateTable();" />&nbsp;
                            <asp:Button ID="btnClear1" runat="server" Text="Clear" CssClass="buttonAdvSrch" OnClientClick="return clearRows();" />&nbsp;
                            <asp:Button ID="btnAddRows" runat="server" Text="Add Rows" CssClass="buttonAdvSrch"
                                OnClientClick="return jsInputBox();" OnClick="btnAddRows_Click" />&nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="pnlTableErrorMessage" runat="server" Visible="false">
    <table class="tableAdvSrchBorder1" cellspacing="0" cellpadding="4" border="0" width="100%">
        <tr>
            <td style="white-space: normal;">
                <br />
                <asp:Label ID="lblTableErrorMessage" CssClass="labelMessage" runat="server">No Wellbore details are available.</asp:Label>
            </td>
        </tr>
    </table>
</asp:Panel>
<input type="hidden" name="hidSelectedCriteriaName" id="hidSelectedCriteriaName"
    runat="server" />
<input type="hidden" name="hidReportSelectRow" id="hidReportSelectRow" runat="server" />
<input type="hidden" name="hidSelectedRows" id="hidSelectedRows" runat="server" />
<input type="hidden" name="hidRows" id="hidRows" runat="server" />
<input type="hidden" name="hidDrpValue" id="hidDrpValue" runat="server" />
<input type="hidden" name="hidDepthMode" id="hidDepthMode" runat="server" />
<input type="hidden" name="hdnTabIndex" id="hdnTabIndex" runat="server" />
<input type="hidden" name="hdnFirstAHDepthValue" id="hdnFirstAHDepthValue" runat="server" />
<input type="hidden" name="hdnLastAHDepthValue" id="hdnLastAHDepthValue" runat="server" />
<asp:HiddenField ID="hidDepthRefDefaultUnit" runat="server" />

<script language="javascript">
 setWindowTitle('AHD-TVD Converter');  
</script>


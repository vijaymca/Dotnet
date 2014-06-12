<%@ Control Language="C#" AutoEventWireup="true" Codebehind="FunctionalityUsageAdvReport.ascx.cs"
    Inherits="Shell.SharePoint.DREAM.Site.UI.UserControls.FunctionalityUsageAdvReport" %>
<style type="text/css">
.LinkTxt
{
	
	
	font-family:Verdana;
	color:#000;
	font-weight:normal;
}

</style>
<table class="tableAdvSrchBorder" cellspacing="0" cellpadding="4" border="0" width="100%">
    <tr>
        <td class="tdAdvSrchHeader" colspan="4">
            <b>Functionality Usage Report</b>
        </td>
    </tr>
</table>
<asp:Panel ID="ExceptionBlock" Visible="false" runat="server">
    <br />
    <asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label>
</asp:Panel>
<br />
<table class="tableAdvSrchBorder" cellspacing="0" cellpadding="4" border="0" width="100%">
    <tr>
        <td colspan="5" class="SearchColumnPARS" style="height: 27px">
            <b>Search</b></td>
    </tr>
    <tr valign="top">
        <td class="tdAdvSrchItem" style="height: 77px" width="100%" colspan="3">
            <span style="word-wrap: normal">Search Name</span></td>
        <td class="tdAdvSrchItem" colspan="1" style="height: 77px" align="left">
            <asp:ListBox ID="lstFURSearchName" EnableViewState="true" runat="server" Width="270px"
                CssClass="dropdownAdvSrch" SelectionMode="Multiple"></asp:ListBox></td>
        <td class="tdAdvSrchItem" colspan="1">
            <asp:Image ID="imgSearchNam" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" />
        </td>
    </tr>
    <tr valign="top">
        <td class="tdAdvSrchItem" style="height: 77px" width="100%" colspan="3">
            <span style="word-wrap: normal">User Name</span>
        </td>
        <td class="tdAdvSrchItem" colspan="1" style="height: 77px" align="left">
            <asp:ListBox ID="lstUserName" EnableViewState="true" runat="server" Width="270px"
                CssClass="dropdownAdvSrch" SelectionMode="Multiple"></asp:ListBox>&nbsp;</td>
        <td class="tdAdvSrchItem" colspan="1">
            <asp:Image ID="imgUserName" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
    </tr>
    <tr>
        <td class="tdAdvSrchSubHeader" colspan="5">
            <b>Date</b>[<a href="javascript:resetFunctionalityUsageDateTable()" class="LinkTxt">Reset
                Date Criteria</a>]
        </td>
    </tr>
    <tr>
        <td class="tdAdvSrchItem" colspan="5" width="100%">
            <table width="100%">
                <tr>
                    <td>
                        <asp:RadioButton ID="rbCurrentYear" runat="server" GroupName="ReportDate" Text="Current Year"
                            onclick="javascript:EnableDisableDates(this);" />
                    </td>
                    <td>
                        <asp:RadioButton ID="rbLast1Year" runat="server" GroupName="ReportDate" Text="Last 1 Year"
                            onclick="javascript:EnableDisableDates(this);" />
                    </td>
                    <td>
                        <asp:RadioButton ID="rbLast2Year" runat="server" GroupName="ReportDate" Text="Last 2 Years"
                            onclick="javascript:EnableDisableDates(this);" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButton ID="rbCurrentMonth" runat="server" GroupName="ReportDate" Text="Current Month"
                            onclick="javascript:EnableDisableDates(this);" />
                    </td>
                    <td>
                        <asp:RadioButton ID="rbLast6Month" runat="server" GroupName="ReportDate" Text="Last 6 Months"
                            onclick="javascript:EnableDisableDates(this);" />
                    </td>
                    <td>
                        <asp:RadioButton ID="rbSelectDates" runat="server" GroupName="ReportDate" Text="Select Dates"
                            onclick="javascript:EnableDisableDates(this);" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr id="trDates" runat="server" visible="true" style="display: none;">
        <td id="Td1" class="tdAdvSrchItem" height="10px" runat="server" width="30%" colspan="1"
            nowrap="nowrap">
            Start Date
            <asp:TextBox ID="txtStartDate" runat="server" CssClass="queryfieldmini" Width="100px"></asp:TextBox>
            <img src="/_layouts/images/calendar.gif" align="Middle" onclick="showCalendarControl('txtStartDate');"
                style="cursor: hand;" />
        </td>
        <td colspan="1" class="tdAdvSrchItem">
            &nbsp;&nbsp;
        </td>
        <td id="Td2" class="tdAdvSrchItem" height="10px" runat="server" colspan="1" nowrap="nowrap"
            align="right" style="width: 31%">
            End Date
            <asp:TextBox ID="txtEndDate" runat="server" CssClass="queryfieldmini" Width="100px"></asp:TextBox>
            <img src="/_layouts/images/calendar.gif" align="Middle" onclick="showCalendarControl('txtEndDate');"
                style="cursor: hand;" />
        </td>
        <td colspan="2" class="tdAdvSrchItem">
            &nbsp;&nbsp;
        </td>
    </tr>
    <tr>
        <td align="right" colspan="5" class="tdAdvSrchItem">
            <asp:Button ID="cmdSearch" runat="server" Text="Search" CssClass="buttonAdvSrch"
                OnClick="cmdSearch_Click" />
            <input type="button" id="cmdReset" value="Reset" class="buttonAdvSrch" onclick="if(!ValidateAdvSearchReset())return false;ResetFunctionalityUsage();" />&nbsp;
        </td>
    </tr>
    <tr>
        <td colspan="5">
            <span style="color: Red">*</span><span style="font-family: Verdana; font-size: 11px;
                color: Black">Unless date is specified, all data will be fetched for current month.</span>
        </td>
    </tr>
</table>

<script type="text/javascript" language="javascript">
        setWindowTitle('Functionality Usage Report');
</script>


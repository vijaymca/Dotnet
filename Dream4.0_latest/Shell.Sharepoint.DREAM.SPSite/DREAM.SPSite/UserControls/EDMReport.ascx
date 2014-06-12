<%@ Control Language="C#" AutoEventWireup="true" Codebehind="EDMReport.ascx.cs" Inherits="Shell.SharePoint.DREAM.Site.UI.UserControls.EDMReport" %>
<asp:Panel ID="EDMReportContent" DefaultButton="btnSubmit" runat="server" Width="100%">
    <asp:Panel ID="ExceptionBlock" Visible="false" runat="server">
        <asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label>
    </asp:Panel>
    <table class="tableAdvSrchBorder" cellspacing="0" cellpadding="4" border="0" width="100%">
        <tr>
            <td style="vertical-align: middle" class="searchFilterHeader">
                <img id="expImage" onclick="ShowHideFilterOptions('expImage','FilterDiv')" src="/_layouts/DREAM/Images/Minus.gif"
                    width="14" alt="" />
                &nbsp;<b>EDM Specific Options</b></td>
        </tr>
        <tr>
            <td>
                <div id="FilterDiv" style="display: block; border: solid 1px #bdbdbd">
                    <table width="100%" cellspacing="0" cellpadding="4">
                        <tr>
                            <td class="tdPARSAdvSrchSubHeader" style="height: 27px">
                            <b>Wellbore</b>
                            </td>
                            <td class="tdPARSAdvSrchSubHeader" style="height: 27px">
                                <b>Select level(s)</b></td>
                            <td class="tdPARSAdvSrchSubHeader" style="height: 27px">
                                <b>Select the period (date range) you want to display</b></td>
                            <td class="tdPARSAdvSrchSubHeader" style="height: 27px">
                                <b>Include event(s) without end date</b></td>
                        </tr>
                        <tr valign="top">
                        <td class="tdAdvSrchItem" runat=server id="tdDDLAssets">
                        
                        </td>
                            <td id="tdReportLevel" class="tdAdvSrchItem">
                                <asp:CheckBoxList ID="chbLstReportLevel" runat="server">
                                    <asp:ListItem Selected="True" Value="EDM Event" onclick="EnableDisabeYesNo();">Event</asp:ListItem>
                                    <asp:ListItem Selected="True" Value="EDM Daily Summary">Daily Summary</asp:ListItem>
                                    <asp:ListItem Selected="True" Value="EDM Activity">Reported Activities</asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                            <td class="tdAdvSrchItem">
                                <table>
                                    <tr>
                                        <td>
                                            Period
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cboTimePeriod" runat="server" CssClass="dropdownAdvSrch" Width="155px"
                                                EnableViewState="true">
                                                <asp:ListItem>--Select--</asp:ListItem>
                                                <asp:ListItem>Last 7 Days</asp:ListItem>
                                                <asp:ListItem>Last 31 Days</asp:ListItem>
                                                <asp:ListItem>Current Week</asp:ListItem>
                                                <asp:ListItem>Current Month</asp:ListItem>
                                                <asp:ListItem>Current Year</asp:ListItem>
                                                <asp:ListItem>Last Week</asp:ListItem>
                                                <asp:ListItem>Last Month</asp:ListItem>
                                                <asp:ListItem>Last year</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Start Date
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="queryfieldmini" Width="130px"
                                                onchange="OnSelectedDateChange();"></asp:TextBox>
                                            <img src="/_layouts/images/calendar.gif" align="middle" onclick="showCalendarControl('txtStartDate');"
                                                style="cursor: hand;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            End Date
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="queryfieldmini" Width="130px"
                                                onchange="OnSelectedDateChange();"></asp:TextBox>
                                            <img src="/_layouts/images/calendar.gif" align="middle" onclick="showCalendarControl('txtEndDate');"
                                                style="cursor: hand;" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td id="tdYesNo" class="tdAdvSrchItem">
                                <asp:RadioButtonList ID="rbLstYesNo" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem>Yes</asp:ListItem>
                                    <asp:ListItem>No</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td colspan="3" class="tdAdvSrchItem">
                                <table>
                                    <tr>
                                        <td>
                                            <b>View:</b></td>
                                        <td>
                                            <asp:RadioButtonList ID="rbLstDisplayFormat" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Selected="True" onclick="EnableDisableReportLevel();">Tabular</asp:ListItem>
                                                <asp:ListItem onclick="EnableDisableReportLevel();">Hierarchical</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="1" class="tdAdvSrchItem" align="right">
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="buttonAdvSrch"
                                    OnClick="BtnSubmit_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Panel>

<script type="text/JavaScript">
    EnableDisabeYesNo();
    EnableDisableReportLevel();
   setWindowTitle('Daily Wells Reporting');   
</script>


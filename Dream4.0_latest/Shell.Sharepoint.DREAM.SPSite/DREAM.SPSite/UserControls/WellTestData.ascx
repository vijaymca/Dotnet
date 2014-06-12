<%@ Control Language="C#" AutoEventWireup="true" Codebehind="WellTestData.ascx.cs"
    Inherits="Shell.SharePoint.DREAM.Site.UI.UserControls.WellTestData" EnableViewState="true" %>
<asp:Panel ID="pnlWellTestData" DefaultButton="btnSubmit" runat="server" Width="100%"
    EnableViewState="true">
    <asp:Panel ID="ExceptionBlock" Visible="false" runat="server">
        <asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label>
    </asp:Panel>
    <table class="tableAdvSrchBorder" cellpadding="5px" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="vertical-align: middle" class="searchFilterHeader">
                <img id="expImage" onclick="ShowHideFilterOptions('expImage','FilterDiv')" src="/_layouts/DREAM/Images/Minus.gif"
                    width="14" alt="" />
                &nbsp;<b>Pressure Survey Data Filter Options</b></td>
        </tr>
        <tr>
            <td>
                <div id="FilterDiv" style="display: block; border: solid 1px #bdbdbd">
                    <table width="100%" cellpadding="5px" cellspacing="0">
                        <tr valign="top">
                            <td >
                                <b>Wellbore:</b>
                            </td>
                            <td  runat="server" id="tdDDLAssets">
                            </td>
                            <td >
                                <fieldset>
                                    <legend class="classLegend">Date Range</legend>
                                    <table>
                                        <tr>
                                            <td align="right">
                                                From:</td>
                                            <td>
                                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="queryfieldmini" Width="130px"
                                                    onchange="OnSelectedDateChange();"></asp:TextBox>
                                                <img src="/_layouts/images/calendar.gif" align="middle" onclick="showCalendarControl('txtStartDate');"
                                                    style="cursor: hand;" alt="" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                To:</td>
                                            <td>
                                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="queryfieldmini" Width="130px"
                                                    onchange="OnSelectedDateChange();"></asp:TextBox>
                                                <img src="/_layouts/images/calendar.gif" align="middle" onclick="showCalendarControl('txtEndDate');"
                                                    style="cursor: hand;" alt="" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                            <td align="right" >
                                <b>Test Types:</b>
                            </td>
                            <td >
                                <asp:ListBox ID="lstTest_Type" runat="server" SelectionMode="Multiple" Width="185px"
                                    CssClass="dropdownAdvSrch"></asp:ListBox>
                            </td>
                            <td align="right" valign="bottom">
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="buttonAdvSrch"
                                    OnClientClick="return ValidateWellTestReportDates();" OnClick="btnSubmit_Click" />
                            </td>
                        </tr>
                        <tr id="trViewMode" runat="server">
                            <td colspan="4" style="white-space: nowrap">
                                <table>
                                    <tr>
                                        <td align="right">
                                            <b>View: </b>
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rbLstDisplayFormat" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Selected="True">Tabular</asp:ListItem>
                                                <asp:ListItem>DataSheet</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Panel>

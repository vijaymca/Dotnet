<%@ Control Language="C#" AutoEventWireup="true" Codebehind="InterpolatedLogs.ascx.cs"
    Inherits="Shell.SharePoint.DREAM.Site.UI.UserControls.InterpolatedLogs" %>
<table class="tableAdvSrchBorder" border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr runat="server" id="trFilterOptions">
        <td style="vertical-align: middle" class="searchFilterHeader">
            <img id="expImage" onclick="ShowHideFilterOptions('expImage','FilterDiv')" src="/_layouts/DREAM/Images/Minus.gif"
                width="14" alt="" />
            &nbsp;<b><span id="lblChnage" runat="server"> Position Log Filter Options</span></b>
            <b><span id="lblChange1" runat="server">Interpolated Positional Logs Filter Options</span></b></td>
    </tr>
    <tr>
        <td width="60%">
            <div id="FilterDiv">
                <fieldset class="classFieldset">
                    <legend class="classLegend">Filter Options</legend>
                    <table border="0" width="100%" cellspacing="0" cellpadding="0" id="tblMain">
                        <tr>
                            <td>
                                <table border="0" cellspacing="2" cellpadding="7">
                                    <tr>
                                        <td align="right">
                                            WellBore&nbsp;</td>
                                        <td>
                                            <asp:DropDownList CssClass="dropdownAdvSrch" ID="ddlWellBore" runat="server" Width="170px">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;ProjectionCoordinateSystem
                                        </td>
                                        <td>
                                            <asp:DropDownList CssClass="dropdownAdvSrch" ID="ddlProjectionSystem" runat="server"
                                                Width="169px">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr runat="server" id="trDepthInterval">
                                        <td>
                                            &nbsp;&nbsp; Depth Interval (AlongHole)</td>
                                        <td>
                                            <asp:DropDownList CssClass="dropdownAdvSrch" ID="ddlDepthInterval" runat="server"
                                                Width="107px">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblDepthInterval" runat="server" Text="(m)" Width="26px"></asp:Label></td>
                                    </tr>
                                    <tr runat="server" id="trDataPreferences">
                                        <td>
                                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; Data Preference
                                        </td>
                                        <td>
                                            <asp:DropDownList CssClass="dropdownAdvSrch" ID="ddlDataPreference" runat="server"
                                                Width="170px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="padding-right: 60px">
                                <fieldset>
                                    <legend>Filter Options</legend>
                                    <table border="0" cellpadding="4" cellspacing="2">
                                        <tr>
                                            <td>
                                                From:</td>
                                            <td style="width: 78px">
                                                <asp:TextBox CssClass="queryfieldmini" ID="txtbxFrom" runat="server" onblur="javascript:return SetToZeroIfEmpty(this);"></asp:TextBox></td>
                                            <td style="width: 1px">
                                                <asp:Label ID="lblFrom" runat="server" Text="(m)"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                To:</td>
                                            <td>
                                                <asp:TextBox CssClass="queryfieldmini" ID="txtbxTo" runat="server"></asp:TextBox></td>
                                            <td style="width: 1px">
                                                <asp:Label ID="lblTo" runat="server" Text="(m)"></asp:Label></td>
                                        </tr>
                                        <tr runat="server" id="trMode">
                                            <td align="right">
                                                Mode:
                                            </td>
                                            <td>
                                                <asp:DropDownList CssClass="dropdownAdvSrch" ID="ddlMode" runat="server" Width="108px">
                                                </asp:DropDownList></td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top" style="padding-right: 60px; padding-bottom: 9px" colspan="2">
                                <asp:Button CssClass="buttonAdvSrch" ID="btnSearch" runat="server" Text="Search"
                                    OnClick="btnSearch_Click" OnClientClick="javascript:return ValidateInterPolatedSearch();"
                                    Height="22px" /></td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </td>
    </tr>
</table>

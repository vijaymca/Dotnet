<%@ Control Language="C#" AutoEventWireup="true" Codebehind="GeoPressure.ascx.cs"
    Inherits="Shell.SharePoint.DREAM.Site.UI.UserControls.GeoPressure" %>
<asp:Panel ID="GeoPressureFilterContent" DefaultButton="btnSubmit" runat="server"
    Width="100%">
    <asp:Panel ID="ExceptionBlock" Visible="false" runat="server">
        <asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label>
    </asp:Panel>
    <table class="tableAdvSrchBorder" cellspacing="0" cellpadding="4" border="0" width="100%">
        <tr>
            <td style="vertical-align: middle" class="searchFilterHeader">
                <img id="expImage" onclick="ShowHideFilterOptions('expImage','FilterDiv')" src="/_layouts/DREAM/Images/Minus.gif"
                    width="14" alt="" />
                &nbsp;<b>Geopressure Filter Options</b></td>
        </tr>
        <tr>
            <td>
                <div id="FilterDiv" style="display: block; border: solid 1px #bdbdbd">
                    <table width="100%">
                        <tr valign="top">
                            <td>
                                <b>Test Type:</b>
                            </td>
                            <td>
                                <asp:DropDownList ID="cboTestType" runat="server" CssClass="dropdownAdvSrch" Width="155px"
                                    EnableViewState="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <b>Data Origin:</b>
                            </td>
                            <td>
                                <asp:DropDownList ID="cboDataSource" runat="server" CssClass="dropdownAdvSrch" Width="155px"
                                    EnableViewState="true">
                                </asp:DropDownList>
                            </td>
                            <td align="right" valign="bottom">
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="buttonAdvSrch"
                                    OnClick="btnSubmit_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Panel>

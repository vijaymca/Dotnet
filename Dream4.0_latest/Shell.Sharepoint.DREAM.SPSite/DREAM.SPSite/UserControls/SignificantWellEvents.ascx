<%@ Control Language="C#" AutoEventWireup="true" Codebehind="SignificantWellEvents.ascx.cs"
    Inherits="Shell.SharePoint.DREAM.Site.UI.UserControls.SignificantWellEvents" %>
<asp:Panel ID="Panel1" runat="server" DefaultButton="btnFilter">
    <table id="tblEventsContainer" class="tableAdvSrchBorder" cellspacing="0" cellpadding="3"
        style="width: 100%">
        <tr>
            <td class="searchFilterHeader" style="vertical-align: middle;" width="100%">
                <img alt="" src="/_layouts/DREAM/Images/Minus.gif" id="expImage" onclick="ShowHideFilterOptions('expImage','FilterDiv')"
                    width="14">&nbsp;&nbsp;<b>Filter Options</b>
            </td>
        </tr>
        <tr>
            <td width="100%">
                <div id="FilterDiv" style="display: block;">
                    <table id="tblFilter" border="0" cellpadding="5" cellspacing="0" style="width: 100%">
                        <tr>
                            <td width="25%">
                                <b>Wellbore: </b>
                            </td>
                            <td class="borderRightStyle" width="25%" runat="server" id="tdDDLAssets">
                            </td>
                            <td colspan="2" width="50%">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td width="25%">
                                <span id="spnEventGroup">
                                    <asp:CheckBox ID="cboEventGroup" runat="server" Text="Only Event of Group" onclick="javascript:return MakeBold(this,'spnEventGroup');" />
                                </span>
                            </td>
                            <td class="borderRightStyle" width="25%">
                                <asp:DropDownList onclick="SelectRadCheckBox('cboEventGroup','spnEventGroup')" ID="cblEventsGroup"
                                    CssClass="ddlSignificantWellEvents" runat="server">
                                </asp:DropDownList></td>
                            <td width="25%">
                                <span id="spnCreatedBy">
                                    <asp:CheckBox ID="cboCreatedBy" runat="server" Text="Only Created By" onclick="javascript:return MakeBold(this,'spnCreatedBy');" /></span>
                            </td>
                            <td width="25%">
                                <asp:TextBox onclick="SelectRadCheckBox('cboCreatedBy','spnCreatedBy')" ID="txtCREATEDBY"
                                    CssClass="queryfieldmini" Width="155px" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td width="25%">
                                <span id="spnEventType">
                                    <asp:CheckBox ID="cboEventType" runat="server" Text="Only Event of Type" onclick="javascript:return MakeBold(this,'spnEventType');" /></span>
                            </td>
                            <td class="borderRightStyle" width="25%">
                                <asp:DropDownList onclick="SelectRadCheckBox('cboEventType','spnEventType')" ID="cblEventsType"
                                    CssClass="ddlSignificantWellEvents" runat="server">
                                </asp:DropDownList></td>
                            <td width="25%">
                                <span id="spnUpdatedBy">
                                    <asp:CheckBox ID="cboUpdatedBy" runat="server" Text="Only Updated By" onclick="javascript:return MakeBold(this,'spnUpdatedBy');" />
                                </span>
                            </td>
                            <td width="25%">
                                <asp:TextBox onclick="SelectRadCheckBox('cboUpdatedBy','spnUpdatedBy')" ID="txtUPDATEDBY"
                                    CssClass="queryfieldmini" Width="155px" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td width="25%" class="tdAdvSrchItem">
                                <span id="spnPriority">
                                    <asp:CheckBox ID="cboEventPriority" runat="server" Text="Only Event with Priority"
                                        onclick="javascript:return MakeBold(this,'spnPriority');" />
                                </span>
                            </td>
                            <td class="borderRightStyle tdAdvSrchItem" width="25%">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chbHigh" runat="server" Text="High" onclick="javascript:SelectExportFilteredEventsCheckBox('cboEventPriority')" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chbMedium" runat="server" Text="Medium" onclick="javascript:SelectExportFilteredEventsCheckBox('cboEventPriority')" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chbLow" runat="server" Text="Low" onclick="javascript:SelectExportFilteredEventsCheckBox('cboEventPriority')" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td width="25%" class="tdAdvSrchItem">
                                <span id="spnOwnedBy">
                                    <asp:CheckBox ID="cboOwnedBy" runat="server" Text="Only Owned By" onclick="javascript:return MakeBold(this,'spnOwnedBy');" />
                                </span>
                            </td>
                            <td width="25%" class="tdAdvSrchItem">
                                <asp:TextBox onclick="SelectRadCheckBox('cboOwnedBy','spnOwnedBy')" Width="155px"
                                    ID="txtOWNEDBY" CssClass="queryfieldmini" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="right" width="100%">
                                <asp:Button ID="btnFilter" Text="Filter" OnClick="BtnFilter_Click" CssClass="buttonAdvSrch"
                                    runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td width="100%">
                <asp:Label ID="lblErrorMessage" runat="server" CssClass="labelMessage"></asp:Label></td>
        </tr>
    </table>
    <asp:HiddenField ID="hdnSwedUrl" runat="server" />
</asp:Panel>

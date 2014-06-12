<%@ Control Language="C#" AutoEventWireup="true" Codebehind="EPCatalogFilter.ascx.cs"
    Inherits="Shell.SharePoint.DREAM.Site.UI.UserControls.EPCatalogFilter" %>
<%@ Register Assembly="Telerik.Web.UI, Version=2010.1.415.35, Culture=neutral,PublicKeyToken=121fae78165ba3d4"
    Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<script type="text/javascript" language="javascript" src="/_layouts/DREAM/Javascript/EPCatalogFilter.js"></script>

<div>
    <table class="tableAdvSrchBorder" cellspacing="0" cellpadding="2" border="0" width="100%">
        <tr>
            <td class="tdAdvSrchHeader" colspan="3">
                <b>EP Catalog Filter Options</b>
            </td>
        </tr>
    </table>
    <br />
    <table class="tableAdvSrchBorder" cellspacing="0" cellpadding="4" border="0" width="100%"
        id="tblEPCatalogFilter" runat="server">
        <tr id="trDates" valign="top">
            <td id="Td1" width="50%">
                <span style="word-wrap: normal">Published Date</span>
            </td>
            <td id="Td2" align="right" nowrap="nowrap">
                <table>
                    <tr>
                        <td align="left">
                            <span style="word-wrap: normal">From</span></td>
                        <td>
                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="queryfieldmini" Width="200px"></asp:TextBox>
                            <img src="/_layouts/images/calendar.gif" align="Middle" onclick="showCalendarControl('txtStartDate');"
                                style="cursor: hand;" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <span style="word-wrap: normal">To</span></td>
                        <td style="white-space: nowrap">
                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="queryfieldmini" Width="200px"></asp:TextBox>
                            <img src="/_layouts/images/calendar.gif" align="Middle" onclick="showCalendarControl('txtEndDate');"
                                style="cursor: hand;" />
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <asp:Image ID="imgPublishedDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" />
            </td>
        </tr>
        <tr>
            <td colspan="3" class="tdAdvSrchItem">
                <span class="warningstyle">*Declared published date is not the date added to EP Catalog.</span></td>
        </tr>
        <tr valign="top">
            <td class="tdAdvSrchItem" width="50%">
                <span style="word-wrap: normal">Author name</span></td>
            <td class="tdAdvSrchItem" align="right">
                <asp:TextBox ID="txtAUTHORNAME" runat="server" Width="253px" CssClass="queryfieldmini"></asp:TextBox>
            </td>
            <td class="tdAdvSrchItem">
                <asp:Image ID="imgAuthorName" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" />
            </td>
        </tr>
        <tr>
            <td colspan="3" class="tdAdvSrchItem">
                <asp:RadioButtonList ID="rbLstEPCatalogSrchType" runat="server" RepeatDirection="Horizontal"
                    Width="100%">
                    <asp:ListItem Value="ProductType" Selected="True" onclick="javascript:EPCatalogSrchTypeOnChange(this,'rbLstEPCatalogSrchType');">Product Type</asp:ListItem>
                    <asp:ListItem Value="KidType" onclick="javascript:EPCatalogSrchTypeOnChange(this,'rbLstEPCatalogSrchType');">Kid Type</asp:ListItem>
                    <asp:ListItem Value="Discipline" onclick="javascript:EPCatalogSrchTypeOnChange(this,'rbLstEPCatalogSrchType');">Discipline</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr valign="top">
            <td valign="top" class="tdAdvSrchItem" width="50%">
                <table cellspacing="0" border="0">
                    <tr>
                        <td>
                            <span id="lblProdType1">EP Catalog Group of Product Types</span></td>
                    </tr>
                    <tr style="height: 10px">
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtBxSrchGrpOfProdType" runat="server" onkeyup="getSearchResults(this.value,'lstGrpOfProdType')"
                                class="epcatalogTxtBxSearch"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadListBox ID="lstGrpOfProdType" runat="server" Height="100px" Width="253px"
                                AllowTransfer="true" ButtonSettings-TransferButtons="TransferFrom,TransferAllFrom"
                                TransferMode="Copy" TransferToID="lstSelectedProdType" ButtonSettings-VerticalAlign="Middle"
                                SelectionMode="Multiple">
                            </telerik:RadListBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span id="lblProdType2">Regional Group of Product Types</span></td>
                    </tr>
                    <tr style="height: 10px">
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtBxSrchRgnGrpOfProdType" runat="server" onkeyup="getSearchResults(this.value,'lstRgnGrpOfProdType')"
                                class="epcatalogTxtBxSearch"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadListBox ID="lstRgnGrpOfProdType" runat="server" Height="100px" Width="253px"
                                AllowTransfer="true" ButtonSettings-TransferButtons="TransferFrom,TransferAllFrom"
                                TransferMode="Copy" TransferToID="lstSelectedProdType" ButtonSettings-VerticalAlign="Middle"
                                SelectionMode="Multiple">
                            </telerik:RadListBox>
                        </td>
                    </tr>
                    <tr id="trAdditonalProdType1">
                        <td>
                            <span>Additional Product Types</span>
                        </td>
                    </tr>
                    <tr style="height: 10px">
                        <td>
                        </td>
                    </tr>
                    <tr id="trAdditonalProdType2">
                        <td>
                            <asp:TextBox ID="txtBxSrchAdditonalProdType" runat="server" onkeyup="getSearchResults(this.value,'lstAdditonalProdType')"
                                class="epcatalogTxtBxSearch"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trAdditonalProdType3">
                        <td>
                            <telerik:RadListBox ID="lstAdditonalProdType" runat="server" Height="100px" Width="253px"
                                AllowTransfer="true" ButtonSettings-TransferButtons="TransferFrom,TransferAllFrom"
                                TransferMode="Copy" TransferToID="lstSelectedProdType" ButtonSettings-VerticalAlign="Middle"
                                SelectionMode="Multiple">
                            </telerik:RadListBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top" class="tdAdvSrchItem" align="right">
                <table>
                    <tr>
                        <td align="left">
                            <span id="lblSelectedTypes">Selected Product Types</span><br />
                            <br />
                            <telerik:RadListBox ID="lstSelectedProdType" runat="server" Height="440px" Width="253px"
                                AllowDelete="true" ButtonSettings-Position="Bottom" SelectionMode="Multiple"
                                ButtonSettings-RenderButtonText="true">
                            </telerik:RadListBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td class="tdAdvSrchItem">
                <asp:Image ID="imgSelectedProdType" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" />
            </td>
        </tr>
        <tr valign="top">
            <td colspan="1">
            </td>
            <td align="right" colspan="1">
                <asp:Button ID="cmdSearch" runat="server" Text="Search" CssClass="buttonAdvSrch"
                    OnClick="CmdSearch_Click" />
                <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="buttonAdvSrch" OnClick="CmdReset_Click" />&nbsp;
            </td>
            <td>
            </td>
        </tr>
    </table>
</div>

<script type="text/JavaScript">
 setWindowTitle('EP Catalog Filter Options');   
</script>
<input type="hidden" runat="server" id="hidIdentifiers" />
<input type="hidden" runat="server" id="hidColumnName" />
<input type="hidden" runat="server" id="hidAssetType" />
<%@ Control Language="C#" AutoEventWireup="true" Codebehind="AddChapter.ascx.cs"
    Inherits="Shell.SharePoint.DWB.UserInterfaceLayer.AddChapter" %>

<script language="javascript" src="/_Layouts/DREAM/Javascript/DWBJavascriptFunctionRel2_0.js"></script>
<link href="/_Layouts/DREAM/Styles/DWBStyleSheetRel2_0.css" rel="stylesheet" type="text/css" />

<div id="AdvancedSearchContainer">
    <asp:Panel ID="pnlTemplate" runat="server" Width="100%">
        <table class="DWBtableAdvSrchBorder" cellspacing="0" cellpadding="2" border="0" width="100%">
            <tr>
                <td class="DWBtdAdvSrchHeader" colspan="2" style="font-weight: normal">
                    &nbsp;<asp:Label ID="lblChapter" runat="server" Text="Add Chapter" Width="90%" Font-Bold="True"></asp:Label>                    
                </td>
            </tr>
        </table>
        <br />
        <asp:Panel ID="ExceptionBlock" Visible="false" runat="server">
            <asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label>
            <br />
        </asp:Panel>
        <asp:HiddenField ID="hdnListitemStatus" runat="server" />
        <table class="DWBtableAdvSrchBorder" cellspacing="2" cellpadding="2" border="0" width="100%">
            <tr>
                <td class="DWBtdAdvSrchSubHeader" colspan="2" style="height: 18px">
                    <b>Details</b> &nbsp;[ <span class="DWBMandatoryMessage">* indicates mandatory field</span>
                    ]</td>
            </tr>
            <tr>
            <tr style="display: none">
                <td class="DWBtdAdvSrchSubHeader" colspan="2" style="height: 18px">
                    <asp:ImageButton ID="imgShowHide" runat="server" OnClientClick="javascript:HideShowSearchCriteria();" />
                    <b>Details</b> &nbsp;[ <span class="DWBMandatoryMessage">* indicates mandatory field</span>]</td>
            </tr>
            <tr>
                <td class="DWItemText" style="width: 28%; height: 28px;">
                    Country</td>
                <td class="DWBtdAdvSrchItem" style="width: 80%; height: 28px;">
                    &nbsp;<asp:DropDownList ID="cboDWBCountry" runat="server" Width="208px" CssClass="DWBdropdownAdvSrch">
                        <asp:ListItem Value="--Any Country--"></asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td class="DWItemText" style="width: 28%; height: 28px;">
                    <span class="DWBMandatory">*</span> Asset Type</td>
                <td class="DWBtdAdvSrchItem" style="width: 80%; height: 28px;">
                    &nbsp;<asp:DropDownList ID="cboAssetType" runat="server" Width="208px" CssClass="DWBdropdownAdvSrch"
                        AutoPostBack="True" OnSelectedIndexChanged="cboAssetType_SelectedIndexChanged">
                        <asp:ListItem Value="--Select--"></asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td class="DWItemText" style="width: 28%; height: 28px;">
                    Column Name</td>
                <td class="DWBtdAdvSrchItem" style="width: 80%; height: 28px;">
                    &nbsp;<asp:DropDownList ID="cboColumnName" runat="server" Width="208px" CssClass="DWBdropdownAdvSrch">
                        <asp:ListItem Value="--Select--"></asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td class="DWItemText" style="width: 28%; height: 28px;">
                    <span class="DWBMandatory">*</span> Search Criteria</td>
                <td class="DWBtdAdvSrchItem" style="width: 80%; height: 28px;">
                    &nbsp;<asp:TextBox ID="txtCriteria" runat="server" Width="105px" CssClass="DWBqueryfieldmini"
                        Style="color: silver" onfocus="ClearDefaultText(this,'Wildcard = % or *');" onblur="ResetToDefaultText(this,'Wildcard = % or *');"
                        Text="Wildcard = % or *"></asp:TextBox>
                        &nbsp;<asp:Button runat="server" ID="btnSearch" Text="Find Assets" CssClass="button" 
                        OnClientClick="javascript:return ValidateDWBChapterCriteria();" OnClick="btnSearch_Click" />
                </td>
            </tr>
            <tr>
                <td class="DWItemText" style="width: 28%; height: 26px;">
                    <span class="DWBMandatory">*</span>
                    <asp:Label ID="lblAssetValue" runat="server" CssClass="DWItemText" Text="Asset Value"></asp:Label></td>
                <td class="DWBtdAdvSrchItem" style="width: 80%; height: 26px;">
                    &nbsp;<asp:ListBox ID="lstAssetValues" runat="server" Width="208px"></asp:ListBox></td>
            </tr>
            <tr>
                <td class="DWItemText" style="width: 28%; height: 35px">
                    <span class="DWBMandatory">*</span> Template</td>
                <td class="DWBtdAdvSrchItem" style="width: 80%; height: 35px">
                    &nbsp;<asp:DropDownList ID="cboTemplate" runat="server" CssClass="DWBdropdownAdvSrch"
                        Width="208px">
                        <asp:ListItem Value="--Select--"></asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td class="DWItemText" style="width: 28%; height: 26px;">
                    <span class="DWBMandatory">*</span> Chapter Title&nbsp;</td>
                <td class="DWBtdAdvSrchItem" style="width: 80%; height: 26px;">
                    &nbsp;<asp:TextBox ID="txtChapterTitle" runat="server" Width="206px" MaxLength="50"
                        CssClass="DWBqueryfieldmini"></asp:TextBox></td>
            </tr>            
            <tr>
                <td class="DWItemText" style="width: 28%; height: 80px;">
                    Description</td>
                <td class="DWBtdAdvSrchItem" style="width: 80%; height: 80px;">
                    &nbsp;<asp:TextBox ID="txtDescription" runat="server" Height="58px" TextMode="MultiLine"
                        Width="208px" CssClass="DWBqueryfieldmini" onkeydown="return ValidateTextLength(this,2000,'Description');"
                        MaxLength="2000"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="center" style="height: 28px" colspan="2">
                    <asp:Button ID="cmdOK" runat="server" Text="Save" CssClass="DWBbuttonAdvSrch" OnClientClick="javascript:return ValidateDWBChapter();"
                        OnClick="cmdOK_Click" Width="10%" />
                    <asp:Button ID="cmdCancel" runat="server" Text="Cancel" CssClass="DWBbuttonAdvSrch"
                        OnClick="cmdCancel_Click" Width="10%" />&nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
</div>
<%--<script type="text/JavaScript">
    setWindowTitle('Add Chapter');    
</script>
--%>

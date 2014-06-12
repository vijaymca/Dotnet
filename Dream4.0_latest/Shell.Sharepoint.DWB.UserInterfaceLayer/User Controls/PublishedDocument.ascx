<%@ Control Language="C#" AutoEventWireup="true" Codebehind="PublishedDocument.ascx.cs"
    Inherits="Shell.SharePoint.DWB.UserInterfaceLayer.PublishedDocument" %>
<link href="/_LAYOUTS/DREAM/styles/DREAMStyleSheetRel2_1.css" rel="stylesheet" type="text/css" />
<link href="/_LAYOUTS/DREAM/styles/DWBStyleSheetRel2_0.css" rel="stylesheet" type="text/css" />

<%--<script language="javascript" src="/_Layouts/DREAM/Javascript/CastleBusyBoxRel2_1.js"></script>--%>

<script language="javascript" src="/_Layouts/DREAM/Javascript/DREAMJavaScriptFunctionsRel2_1.js"></script>

<script language="javascript" src="/_Layouts/DREAM/Javascript/DWBJavascriptFunctionRel2_0.js"></script>

<asp:Panel ID="AdvancedSearchContent" Visible="false" runat="server" Width="100%">
    <table width="100%" cellpadding="2" cellspacing="0">
        <tr>
            <td colspan="4">
                <asp:Label ID="lblErrorMessage" runat="server" Text="" Visible="false" CssClass="labelMessage" /></td>
        </tr>
        <tr bgcolor="#dcdcdc">
            <td valign="top" align="left" style="word-wrap: break-word;height:18px; ">
                <asp:Label ID="lblTitleTemplate" runat="server" CssClass="DWBqueryfieldmini" Style="margin-top: 2px;
                    padding-left: 10px"></asp:Label>
                    &nbsp;Owner&nbsp;<asp:Label ID="lblOwner" runat="server" CssClass="DWBqueryfieldmini"
                    Style="margin-top: 2px; padding-left: 10px"></asp:Label>
                    &nbsp;Signed Off&nbsp;:&nbsp;<asp:Label ID="lblSignedOff" runat="server" CssClass="DWBqueryfieldmini"
                    Style="margin-top: 2px;"></asp:Label>
            </td>
            <td valign="top" align="left" style="word-wrap: break-word;">
                
            </td>
            <td valign="top" align="left">
                
            </td>            
        </tr>
        <tr>
        <td valign="top" colspan="1" align="left">
        <asp:Label ID="lblModifiedDate" runat="server" CssClass="DWBqueryfieldmini" Width="190px" 
                    Style="margin-top: 1px; padding-left: 10px; word-wrap: break-word; background-color:#dcdcdc"></asp:Label>
        </td>
            <td valign="top" align="right">
            <a href="javascript:void(0)" id="linkEPCatalogFilter" visible="false" runat="server" onclick="javascript:openEPCatalgueFilter()">
                    EP Catalog Filter</a> &nbsp;
                <%--<input type="button" id="btnSignOff" value="Sign Off" runat="server" class="button"
                    onserverclick="BtnSignOff_ServerClick" visible="false" />--%>
                    <asp:Button runat="server" ID="btnSignOff" CssClass="button" OnClick="BtnSignOff_ServerClick" Visible="false" />
                &nbsp; 
                <input type="button" id="btnUpdate" value="Update" class="button" runat="server" visible="false" />
                <asp:Button ID="btnPrint" Text="Print this Page" CssClass="button" runat="server" OnClick="btnPrint_ServerClick" />
               <%-- <input type="button" id="btnPrint" value="Print this Page" class="button" runat="server"  onserverclick="btnPrint_ServerClick" />  --%>              
            </td>
        </tr>
        <tr>
            <td valign="top" colspan="4" style="height:100%">
                <div id="docviewerdiv" runat="server" style="width: 100%; overflow:auto;">
                </div>
            </td>
        </tr>
    </table>
    <input type="hidden" id="hidPageId" runat="server" />
    <input type="hidden" id="hidAssetType" runat="server" />
    <asp:HiddenField id="hidSelectedCriteriaName" runat="server" Value="UWBI" />
    <asp:HiddenField id="hidSelectedRows" runat="server" Value="" />
     <asp:HiddenField ID="hdnIncludeStoryBoard" runat="server" />
      <asp:HiddenField ID="hdnIncludePageTitle" runat="server" />
    <asp:Button runat="server" ID="btnFirePostBack" OnClick="btnFirePostBack_Click" OnClientClick="Javascript:return FirePostBack('true');" Style="display:none;" />
</asp:Panel>

<script language="javascript" type="text/javascript">
window.onresize = window.onload = Resize;
</script>

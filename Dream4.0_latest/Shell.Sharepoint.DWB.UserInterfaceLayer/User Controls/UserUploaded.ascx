<%@ Control Language="C#" AutoEventWireup="true" Codebehind="UserUploaded.ascx.cs"
    Inherits="Shell.SharePoint.DWB.UserInterfaceLayer.UserUploaded" %>
<link href="/_LAYOUTS/DREAM/styles/DREAMStyleSheetRel2_1.css" rel="stylesheet" type="text/css" />
<link href="/_LAYOUTS/DREAM/styles/DWBStyleSheetRel2_0.css" rel="stylesheet" type="text/css" />

<%--<script language="javascript" src="/_Layouts/DREAM/Javascript/CastleBusyBoxRel2_1.js"></script>--%>

<script language="javascript" src="/_Layouts/DREAM/Javascript/DREAMJavaScriptFunctionsRel2_1.js"></script>

<script language="javascript" src="/_Layouts/DREAM/Javascript/DWBJavascriptFunctionRel2_0.js"></script>

<asp:Panel ID="AdvancedSearchContent" Visible="false" runat="server" Width="100%">
    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td colspan="4">
                <asp:Label ID="lblErrorMessage" runat="server" Text="" Visible="false" CssClass="labelMessage" /></td>
        </tr>
        <tr bgcolor="#dcdcdc">
            <td valign="top" colspan="4" align="left">
                Page Title<asp:Label ID="lblTitleTemplate" runat="server" CssClass="DWBqueryfieldmini" Width="240px"
                    Style="margin-top: 2px; padding-left: 10px; word-wrap: break-word"></asp:Label>&nbsp;
                    Owner&nbsp;<asp:Label ID="lblOwner" runat="server" CssClass="DWBqueryfieldmini" Style="margin-top: 2px;
                    padding-left: 10px; word-wrap: break-word" Width="140px"></asp:Label>
                 Signed Off&nbsp;:&nbsp;<asp:Label ID="lblSignedOff" runat="server" CssClass="DWBqueryfieldmini"
                    Width="40px" Style="margin-top: 2px;"></asp:Label>   
            </td>                                
        </tr>
        <tr>
        <td valign="top" colspan="1" align="left">
        <asp:Label ID="lblModifiedDate" runat="server" CssClass="DWBqueryfieldmini" Width="190px" 
                    Style="margin-top: 1px; padding-left: 10px; word-wrap: break-word; background-color:#dcdcdc"></asp:Label>
        </td>
        <td valign="top" colspan="3" align="right">
                <input type="button" id="btnSignOff" value="Sign Off" runat="server" class="button"
                    onserverclick="BtnSignOff_ServerClick" visible="false" />
                &nbsp;
                <input type="button" id="btnUpdate" value="Update" class="button" runat="server" visible="false" />
                &nbsp;
                   <asp:Button ID="btnPrint" Text="Print this Page" CssClass="button" runat="server" OnClick="btnPrintPage_ServerClick" />
                <%--<input type="button" id="btnPrint" value="Print this Page" class="button" runat="server" visible="true" onserverclick="btnPrintPage_ServerClick" />--%>
            </td>
        </tr>
        <tr>
            <td valign="top" colspan="4">
                <div id="docviewerdiv" runat="server" style="width: 100%; overflow:auto;">
                </div>
            </td>
        </tr>
    </table>
    <input type="hidden" id="hidPageId" runat="server" />
    <input type="hidden" id="hidtype3uploaded" runat="server" />
     <asp:HiddenField ID="hdnIncludeStoryBoard" runat="server" />
      <asp:HiddenField ID="hdnIncludePageTitle" runat="server" />
    <asp:Button runat="server" ID="btnFirePostBack" style="display:none;" OnClientClick="Javascript:return FirePostBack('true');" OnClick="btnFirePOstBack_Click" />
    </asp:Panel>

<script language="javascript" type="text/javascript">

window.onresize = window.onload = Resize;
window.attachEvent("onunload",closeAllChildWellReview);

</script>


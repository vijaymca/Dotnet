<%@ Control Language="C#" AutoEventWireup="true" Codebehind="AssetTree.ascx.cs" Inherits="Shell.SharePoint.DREAM.Site.UI.UserControls.AssetTree" %>
<%@ Register Assembly="Telerik.Web.UI, Version=2010.1.415.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4"
    Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<link rel="Stylesheet" href="/_Layouts/DREAM/Styles/AssetTreeRel3_0.css" type="text/css" />

<script type="text/ecmascript" language="javascript" src="/_Layouts/DREAM/Javascript/AssetTreeRel3_0.js"></script>

<table id="tblMain" class="tableEnable" cellspacing="0" cellpadding="4">
    <tr>
        <td class="tdAssetTreeHeader" style="font-weight: bold;">
            &nbsp;Asset Tree
        </td>
    </tr>
    <tr oncontextmenu="return false;">
        <td>
            <asp:UpdatePanel ID="updatePanelAssetTree" runat="server">
                <contenttemplate>
    <asp:Panel ID="pnlMain" runat="server">
        <telerik:RadTreeView ID="trvAssetTree" runat="server" OnNodeExpand="trvAssetTree_NodeExpand"
            OnNodeClick="trvAssetTree_NodeClick" EnableViewState="true"
            EnableEmbeddedScripts="true" OnClientNodeClicking="onNodeClicking" OnNodeCollapse="trvAssetTree_NodeCollapse" OnClientContextMenuItemClicked="onContextMenuItemClicked"
            Font-Names="Verdana" Font-Size="11px" DataTextField="l" SingleExpandPath="false" CheckBoxes="true" OnClientNodeChecking="ClientNodeChecking">
        </telerik:RadTreeView>
    </asp:Panel>
    <div>
        <asp:HiddenField ID="hidPageNumber" runat="server" />
        <asp:HiddenField ID="hidClickedPage" runat="server" />
        <asp:HiddenField ID="hidSearchText" runat="server" />
        <asp:HiddenField ID="hidSearchBoxStatus" runat="server" />
        <asp:HiddenField ID="hidRecordCount" runat="server" />
    </div>
</contenttemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>

<script type="text/javascript" language="javascript">
        if(isPostBack)
        {
            HideShowLeftNav();
        }
</script>


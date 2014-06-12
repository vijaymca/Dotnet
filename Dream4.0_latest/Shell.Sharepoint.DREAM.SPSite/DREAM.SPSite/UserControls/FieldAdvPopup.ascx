<%@ Control Language="C#" AutoEventWireup="true" Codebehind="FieldAdvPopup.ascx.cs"
    Inherits="Shell.SharePoint.DREAM.Site.UI.UserControls.FieldAdvPopup" %>

<script type="text/javascript" language="javascript" src="/_Layouts/DREAM/Javascript/SRPJavaScriptFunctionsRel3_0.js"></script>

<table class="tableAdvSrchBorder" cellpadding="4" cellspacing="0" border="0" width="100%">
    <!-- class="tableAdvSrchBorder" cellspacing="0" cellpadding="4" border="0" width="100%" -->
    <tr>
        <td style="vertical-align: middle" class="tdAdvSrchHeader" colspan="2">
            <asp:Label ID="lblName" runat="server" Font-Bold="true"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <br />
            <asp:ListBox ID="lstFieldNames" runat="server" SelectionMode="Single" Width="185px"
                CssClass="dropdownAdvSrch"></asp:ListBox>
        </td>
        <td>
            <br />
            <asp:Label ID="lblMessage" runat="server" Text="" CssClass="labelMessage" Visible="false"></asp:Label>
        </td>
    </tr>
</table>
<br />
<input type="button" id="cmdSelect" runat="server" value="Select" class="buttonAdvSrch" />
<input type="button" id="cmdClose" runat="server" value="Close" class="buttonAdvSrch" />

<script type="text/JavaScript">
    setWindowTitle("<%=lblName.Text%>");      
</script>


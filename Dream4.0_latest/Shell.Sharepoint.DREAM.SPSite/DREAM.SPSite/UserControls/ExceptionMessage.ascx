<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExceptionMessage.ascx.cs" Inherits="Shell.SharePoint.DREAM.Site.UI.UserControls.ExceptionMessage" %>

<table border="0" cellpadding="4" cellspacing="0" class="tableAdvSrchBorder" width="100%">
    <tr>
        <td class="tdAdvSrchHeader">
            <b>Exception Details</b>
         </td>
    </tr>
    <tr>
        <td class="exception">
            Unexpected error occured. Sorry for the inconvenience. Please contact your administrator.
            <asp:Label ID="lblMessage" runat="server" CssClass="exception"></asp:Label>
         </td>
    </tr>
</table>


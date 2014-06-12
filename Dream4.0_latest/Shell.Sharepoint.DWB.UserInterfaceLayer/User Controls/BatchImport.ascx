<%@ Control Language="C#" AutoEventWireup="true" Codebehind="BatchImport.ascx.cs"
    Inherits="Shell.SharePoint.DWB.UserInterfaceLayer.BatchImport" %>
<link href="/_LAYOUTS/DREAM/styles/DREAMStyleSheetRel2_1.css" rel="stylesheet" type="text/css" />
<link href="/_LAYOUTS/DREAM/styles/DWBStyleSheetRel2_0.css" rel="stylesheet" type="text/css" />

<script language="javascript" src="/_Layouts/DREAM/Javascript/DWBJavascriptFunctionRel2_0.js"></script>

<asp:Panel ID="ExceptionBlock" Visible="false" runat="server">
    <asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label>
    <br />
</asp:Panel>
<asp:Panel ID="pnlUploadOption" GroupingText="Upload Option" runat="server">
    <table width="100%" style="text-align: left;">
        <tr>
                <td style="width: 10%;"></td>
                <td style="width: 70%; text-align:left; vertical-align:text-bottom;"><asp:Label ID="lblWarningMsg" runat="server" CssClass="labelMessage" Text="Please ensure valid path to continue the Batch Import process."></asp:Label></td>
                <td style="width: 20%;">&nbsp;</td>
        </tr>
        <tr>
            <td style="text-align: right; width: 30%;">
                <asp:Label ID="lblDefaultPath" runat="server" Text="Default Path"></asp:Label> </td>
            <td style="text-align: left; width: 50%;">
                &nbsp;<asp:TextBox ID="txtSharedPath" runat="server"  Width="95%" /></td>
            <td style="text-align: left; width: 20%;">
                <input type="button" id="btnChange" name="Change" value="Change" onclick="javascript:return enableTextbox();"  class="DWBbuttonAdvSrch"/>
            </td>
        </tr>
        <tr style="height: 5%;">
            <td colspan="3" style="text-align: left; width: 100%;">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="text-align: right" colspan="3" height="100%">
                <asp:Button ID="btnContinue" runat="server" Text="Continue" CssClass="DWBbuttonAdvSrch" OnClick="btnContinue_Click" />&nbsp;&nbsp;
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="DWBbuttonAdvSrch"
                    OnClientClick="window.close();return false;" />
                <br />
            </td>
        </tr>
        <tr style="height: 5%;">
            <td colspan="3" style="text-align: left; width: 100%;">
                &nbsp;</td>
        </tr>
    </table>
</asp:Panel>
<script language="javascript" type="text/javascript">
 document.getElementById(GetObjectID("txtSharedPath","input")).disabled = true;
 document.getElementById(GetObjectID("lblWarningMsg","span")).style.display = "none";
 </script>
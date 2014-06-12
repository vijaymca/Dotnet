<%@ Control Language="C#" AutoEventWireup="true" Codebehind="BatchImportStatus.ascx.cs"
    Inherits="Shell.SharePoint.DWB.UserInterfaceLayer.BatchImportStatus" %>

<script language="javascript" src="/_Layouts/DREAM/Javascript/DWBJavascriptFunctionRel2_0.js"></script>

<table width="100%" style="padding-left:10px;">
    <tr>
        <td style="text-align:left;"><asp:Label ID="lblStatusMsg" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td style="text-align:left;"><asp:LinkButton ID="lnkShowBatchImportStatus" runat="server"></asp:LinkButton></td>
    </tr>
</table>
